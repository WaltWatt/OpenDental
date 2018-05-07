/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDental.DivvyConnect;
using System.Net;
using System.Xml;
using System.Text;
using CodeBase;
using System.Linq;
using System.Threading;

namespace OpenDental{
///<summary></summary>
	public class FormRecallList:ODForm {
		#region UI Variables
		private OpenDental.UI.Button butClose;
		private System.ComponentModel.Container components = null;
		private ContextMenu menuRightClick;
		private MenuItem menuItemSeeFamily;
		private MenuItem menuItemSeeAccount;
		private TabControl tabControl;
		private TabPage tabPageRecalls;
		private GroupBox groupBox2;
		private ComboBox comboEmailFrom;
		private Panel panelWebSched;
		private UI.Button butUndo;
		private UI.Button butGotoFamily;
		private UI.Button butCommlog;
		private UI.Button butGotoAccount;
		private UI.Button butSchedFam;
		private UI.Button butLabelOne;
		private UI.Button butSchedPat;
		private Label labelPatientCount;
		private UI.Button butECards;
		private UI.Button butEmail;
		private UI.Button butPostcards;
		private UI.Button butPrint;
		private ODGrid gridMain;
		private GroupBox groupBox3;
		private ComboBox comboStatus;
		private UI.Button butSetStatus;
		private UI.Button butLabels;
		private UI.Button butReport;
		private GroupBox groupBox1;
		private CheckBox checkConflictingTypes;
		private ComboBox comboNumberReminders;
		private Label label3;
		private ComboBox comboSort;
		private Label label5;
		private ComboBox comboSite;
		private Label labelSite;
		private ComboBox comboClinic;
		private Label labelClinic;
		private ComboBox comboProv;
		private Label label4;
		private CheckBox checkGroupFamilies;
		private ValidDate textDateEnd;
		private ValidDate textDateStart;
		private Label label2;
		private Label label1;
		private UI.Button butRefresh;
		private TabPage tabPageRecentlyContacted;
		private ODGrid gridRecentlyContacted;
		private GroupBox groupBox4;
		private ComboBoxClinic comboClinicRecent;
		private Label labelClinicRecent;
		private UI.Button butRefreshRecent;
		private ODDateRangePickerVertical datePickerRecent;
		#endregion UI Variables
		private int pagesPrinted;
		private DataTable addrTable;
		private int patientsPrinted;
		private OpenDental.UI.FormPrintPreview printPreview;
		private PrintDocument pd;
		///<summary>This is the patNum of the current (or last) patient selected.  The calling form should then make use of this to refresh to that patient.  If 0, then calling form should not refresh.</summary>
		public long SelectedPatNum;
		private DataTable _tableRecalls;
		private bool headingPrinted;
		private int headingPrintH;
		private List<Clinic> _listUserClinics;
		private List<EmailAddress> _listEmailAddresses;
		///<summary>The clinics that are signed up for Web Sched.</summary>
		private List<long> _listClinicNumsWebSched=new List<long>();
		ODThread _threadWebSchedSignups=null;
		///<summary>The user has clicked the Web Sched button while a thread was busy checking which clinics are signed up for Web Sched.</summary>
		private bool _hasClickedWebSched;
		private List<Provider> _listProviders;
		private List<Site> _listSites;
		private List<Def> _listRecallUnschedStatusDefs;
		///<summary>A Func that can be called to get the main recall list table.</summary>
		private Func<DataTable> _getRecallTable;

		///<summary>True if the thread checking the clinics that are signed up for Web Sched has finished.</summary>
		private bool _isDoneCheckingWebSchedClinics {
			get { return _threadWebSchedSignups==null; }
		}
		
		///<summary></summary>
		public FormRecallList(){
			InitializeComponent();// Required for Windows Form Designer support
			gridMain.ContextMenu=menuRightClick;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecallList));
			this.butClose = new OpenDental.UI.Button();
			this.menuRightClick = new System.Windows.Forms.ContextMenu();
			this.menuItemSeeFamily = new System.Windows.Forms.MenuItem();
			this.menuItemSeeAccount = new System.Windows.Forms.MenuItem();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPageRecalls = new System.Windows.Forms.TabPage();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.comboEmailFrom = new System.Windows.Forms.ComboBox();
			this.panelWebSched = new System.Windows.Forms.Panel();
			this.butUndo = new OpenDental.UI.Button();
			this.butGotoFamily = new OpenDental.UI.Button();
			this.butCommlog = new OpenDental.UI.Button();
			this.butGotoAccount = new OpenDental.UI.Button();
			this.butSchedFam = new OpenDental.UI.Button();
			this.butLabelOne = new OpenDental.UI.Button();
			this.butSchedPat = new OpenDental.UI.Button();
			this.labelPatientCount = new System.Windows.Forms.Label();
			this.butECards = new OpenDental.UI.Button();
			this.butEmail = new OpenDental.UI.Button();
			this.butPostcards = new OpenDental.UI.Button();
			this.butPrint = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.comboStatus = new System.Windows.Forms.ComboBox();
			this.butSetStatus = new OpenDental.UI.Button();
			this.butLabels = new OpenDental.UI.Button();
			this.butReport = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkConflictingTypes = new System.Windows.Forms.CheckBox();
			this.comboNumberReminders = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.comboSort = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.comboSite = new System.Windows.Forms.ComboBox();
			this.labelSite = new System.Windows.Forms.Label();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.comboProv = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.checkGroupFamilies = new System.Windows.Forms.CheckBox();
			this.textDateEnd = new OpenDental.ValidDate();
			this.textDateStart = new OpenDental.ValidDate();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.butRefresh = new OpenDental.UI.Button();
			this.tabPageRecentlyContacted = new System.Windows.Forms.TabPage();
			this.gridRecentlyContacted = new OpenDental.UI.ODGrid();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.butRefreshRecent = new OpenDental.UI.Button();
			this.comboClinicRecent = new OpenDental.UI.ComboBoxClinic();
			this.labelClinicRecent = new System.Windows.Forms.Label();
			this.datePickerRecent = new OpenDental.UI.ODDateRangePickerVertical();
			this.tabControl.SuspendLayout();
			this.tabPageRecalls.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPageRecentlyContacted.SuspendLayout();
			this.groupBox4.SuspendLayout();
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
			this.butClose.Location = new System.Drawing.Point(906, 733);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// menuRightClick
			// 
			this.menuRightClick.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSeeFamily,
            this.menuItemSeeAccount});
			// 
			// menuItemSeeFamily
			// 
			this.menuItemSeeFamily.Index = 0;
			this.menuItemSeeFamily.Text = "See Family";
			this.menuItemSeeFamily.Click += new System.EventHandler(this.menuItemSeeFamily_Click);
			// 
			// menuItemSeeAccount
			// 
			this.menuItemSeeAccount.Index = 1;
			this.menuItemSeeAccount.Text = "See Account";
			this.menuItemSeeAccount.Click += new System.EventHandler(this.menuItemSeeAccount_Click);
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabPageRecalls);
			this.tabControl.Controls.Add(this.tabPageRecentlyContacted);
			this.tabControl.Location = new System.Drawing.Point(2, 3);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(979, 725);
			this.tabControl.TabIndex = 3;
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			// 
			// tabPageRecalls
			// 
			this.tabPageRecalls.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageRecalls.Controls.Add(this.groupBox2);
			this.tabPageRecalls.Controls.Add(this.panelWebSched);
			this.tabPageRecalls.Controls.Add(this.butUndo);
			this.tabPageRecalls.Controls.Add(this.butGotoFamily);
			this.tabPageRecalls.Controls.Add(this.butCommlog);
			this.tabPageRecalls.Controls.Add(this.butGotoAccount);
			this.tabPageRecalls.Controls.Add(this.butSchedFam);
			this.tabPageRecalls.Controls.Add(this.butLabelOne);
			this.tabPageRecalls.Controls.Add(this.butSchedPat);
			this.tabPageRecalls.Controls.Add(this.labelPatientCount);
			this.tabPageRecalls.Controls.Add(this.butECards);
			this.tabPageRecalls.Controls.Add(this.butEmail);
			this.tabPageRecalls.Controls.Add(this.butPostcards);
			this.tabPageRecalls.Controls.Add(this.butPrint);
			this.tabPageRecalls.Controls.Add(this.gridMain);
			this.tabPageRecalls.Controls.Add(this.groupBox3);
			this.tabPageRecalls.Controls.Add(this.butLabels);
			this.tabPageRecalls.Controls.Add(this.butReport);
			this.tabPageRecalls.Controls.Add(this.groupBox1);
			this.tabPageRecalls.Location = new System.Drawing.Point(4, 22);
			this.tabPageRecalls.Name = "tabPageRecalls";
			this.tabPageRecalls.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageRecalls.Size = new System.Drawing.Size(971, 699);
			this.tabPageRecalls.TabIndex = 0;
			this.tabPageRecalls.Text = "Recalls";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.comboEmailFrom);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(704, 49);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(263, 40);
			this.groupBox2.TabIndex = 125;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Email From";
			// 
			// comboEmailFrom
			// 
			this.comboEmailFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboEmailFrom.Location = new System.Drawing.Point(17, 14);
			this.comboEmailFrom.MaxDropDownItems = 40;
			this.comboEmailFrom.Name = "comboEmailFrom";
			this.comboEmailFrom.Size = new System.Drawing.Size(233, 21);
			this.comboEmailFrom.TabIndex = 65;
			// 
			// panelWebSched
			// 
			this.panelWebSched.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.panelWebSched.BackgroundImage = global::OpenDental.Properties.Resources.webSched_PV_Button;
			this.panelWebSched.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.panelWebSched.Location = new System.Drawing.Point(759, 668);
			this.panelWebSched.Name = "panelWebSched";
			this.panelWebSched.Size = new System.Drawing.Size(120, 24);
			this.panelWebSched.TabIndex = 138;
			this.panelWebSched.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelWebSched_MouseClick);
			// 
			// butUndo
			// 
			this.butUndo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butUndo.Autosize = true;
			this.butUndo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUndo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUndo.CornerRadius = 4F;
			this.butUndo.Location = new System.Drawing.Point(3, 668);
			this.butUndo.Name = "butUndo";
			this.butUndo.Size = new System.Drawing.Size(119, 24);
			this.butUndo.TabIndex = 137;
			this.butUndo.Text = "Undo";
			this.butUndo.Click += new System.EventHandler(this.butUndo_Click);
			// 
			// butGotoFamily
			// 
			this.butGotoFamily.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGotoFamily.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butGotoFamily.Autosize = true;
			this.butGotoFamily.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGotoFamily.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGotoFamily.CornerRadius = 4F;
			this.butGotoFamily.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butGotoFamily.Location = new System.Drawing.Point(443, 642);
			this.butGotoFamily.Name = "butGotoFamily";
			this.butGotoFamily.Size = new System.Drawing.Size(96, 24);
			this.butGotoFamily.TabIndex = 136;
			this.butGotoFamily.Text = "Go to Family";
			this.butGotoFamily.Click += new System.EventHandler(this.butGotoFamily_Click);
			// 
			// butCommlog
			// 
			this.butCommlog.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCommlog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butCommlog.Autosize = true;
			this.butCommlog.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCommlog.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCommlog.CornerRadius = 4F;
			this.butCommlog.Image = global::OpenDental.Properties.Resources.commlog;
			this.butCommlog.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCommlog.Location = new System.Drawing.Point(545, 668);
			this.butCommlog.Name = "butCommlog";
			this.butCommlog.Size = new System.Drawing.Size(88, 24);
			this.butCommlog.TabIndex = 135;
			this.butCommlog.Text = "Comm";
			this.butCommlog.Click += new System.EventHandler(this.butCommlog_Click);
			// 
			// butGotoAccount
			// 
			this.butGotoAccount.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGotoAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butGotoAccount.Autosize = true;
			this.butGotoAccount.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGotoAccount.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGotoAccount.CornerRadius = 4F;
			this.butGotoAccount.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butGotoAccount.Location = new System.Drawing.Point(443, 668);
			this.butGotoAccount.Name = "butGotoAccount";
			this.butGotoAccount.Size = new System.Drawing.Size(96, 24);
			this.butGotoAccount.TabIndex = 134;
			this.butGotoAccount.Text = "Go to Account";
			this.butGotoAccount.Click += new System.EventHandler(this.butGotoAccount_Click);
			// 
			// butSchedFam
			// 
			this.butSchedFam.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSchedFam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSchedFam.Autosize = true;
			this.butSchedFam.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSchedFam.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSchedFam.CornerRadius = 4F;
			this.butSchedFam.Image = global::OpenDental.Properties.Resources.butPin;
			this.butSchedFam.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSchedFam.Location = new System.Drawing.Point(639, 668);
			this.butSchedFam.Name = "butSchedFam";
			this.butSchedFam.Size = new System.Drawing.Size(114, 24);
			this.butSchedFam.TabIndex = 129;
			this.butSchedFam.Text = "Sched Family";
			this.butSchedFam.Click += new System.EventHandler(this.butSchedFam_Click);
			// 
			// butLabelOne
			// 
			this.butLabelOne.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLabelOne.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butLabelOne.Autosize = true;
			this.butLabelOne.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLabelOne.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLabelOne.CornerRadius = 4F;
			this.butLabelOne.Image = global::OpenDental.Properties.Resources.butLabel;
			this.butLabelOne.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butLabelOne.Location = new System.Drawing.Point(128, 642);
			this.butLabelOne.Name = "butLabelOne";
			this.butLabelOne.Size = new System.Drawing.Size(119, 24);
			this.butLabelOne.TabIndex = 133;
			this.butLabelOne.Text = "Single Labels";
			this.butLabelOne.Click += new System.EventHandler(this.butLabelOne_Click);
			// 
			// butSchedPat
			// 
			this.butSchedPat.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSchedPat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSchedPat.Autosize = true;
			this.butSchedPat.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSchedPat.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSchedPat.CornerRadius = 4F;
			this.butSchedPat.Image = global::OpenDental.Properties.Resources.butPin;
			this.butSchedPat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSchedPat.Location = new System.Drawing.Point(639, 642);
			this.butSchedPat.Name = "butSchedPat";
			this.butSchedPat.Size = new System.Drawing.Size(114, 24);
			this.butSchedPat.TabIndex = 128;
			this.butSchedPat.Text = "Sched Patient";
			this.butSchedPat.Click += new System.EventHandler(this.butSchedPat_Click);
			// 
			// labelPatientCount
			// 
			this.labelPatientCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelPatientCount.Location = new System.Drawing.Point(854, 640);
			this.labelPatientCount.Name = "labelPatientCount";
			this.labelPatientCount.Size = new System.Drawing.Size(114, 14);
			this.labelPatientCount.TabIndex = 132;
			this.labelPatientCount.Text = "Patient Count:";
			this.labelPatientCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butECards
			// 
			this.butECards.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butECards.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butECards.Autosize = true;
			this.butECards.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butECards.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butECards.CornerRadius = 4F;
			this.butECards.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butECards.Location = new System.Drawing.Point(253, 642);
			this.butECards.Name = "butECards";
			this.butECards.Size = new System.Drawing.Size(91, 24);
			this.butECards.TabIndex = 130;
			this.butECards.Text = "eCards";
			this.butECards.Visible = false;
			this.butECards.Click += new System.EventHandler(this.butECards_Click);
			// 
			// butEmail
			// 
			this.butEmail.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butEmail.Autosize = true;
			this.butEmail.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEmail.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEmail.CornerRadius = 4F;
			this.butEmail.Image = global::OpenDental.Properties.Resources.email1;
			this.butEmail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEmail.Location = new System.Drawing.Point(253, 668);
			this.butEmail.Name = "butEmail";
			this.butEmail.Size = new System.Drawing.Size(91, 24);
			this.butEmail.TabIndex = 131;
			this.butEmail.Text = "E-Mail";
			this.butEmail.Click += new System.EventHandler(this.butEmail_Click);
			// 
			// butPostcards
			// 
			this.butPostcards.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPostcards.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPostcards.Autosize = true;
			this.butPostcards.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPostcards.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPostcards.CornerRadius = 4F;
			this.butPostcards.Image = global::OpenDental.Properties.Resources.butPreview;
			this.butPostcards.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPostcards.Location = new System.Drawing.Point(3, 642);
			this.butPostcards.Name = "butPostcards";
			this.butPostcards.Size = new System.Drawing.Size(119, 24);
			this.butPostcards.TabIndex = 124;
			this.butPostcards.Text = "Postcard Preview";
			this.butPostcards.Click += new System.EventHandler(this.butPostcards_Click);
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(350, 668);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(87, 24);
			this.butPrint.TabIndex = 127;
			this.butPrint.Text = "Print List";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasLinkDetect = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = true;
			this.gridMain.Location = new System.Drawing.Point(3, 93);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(965, 544);
			this.gridMain.TabIndex = 126;
			this.gridMain.Title = "Recall List";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableRecallList";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.comboStatus);
			this.groupBox3.Controls.Add(this.butSetStatus);
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(704, 6);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(263, 40);
			this.groupBox3.TabIndex = 123;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Set Status";
			// 
			// comboStatus
			// 
			this.comboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStatus.Location = new System.Drawing.Point(17, 14);
			this.comboStatus.MaxDropDownItems = 40;
			this.comboStatus.Name = "comboStatus";
			this.comboStatus.Size = new System.Drawing.Size(160, 21);
			this.comboStatus.TabIndex = 15;
			// 
			// butSetStatus
			// 
			this.butSetStatus.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSetStatus.Autosize = true;
			this.butSetStatus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSetStatus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSetStatus.CornerRadius = 4F;
			this.butSetStatus.Location = new System.Drawing.Point(183, 11);
			this.butSetStatus.Name = "butSetStatus";
			this.butSetStatus.Size = new System.Drawing.Size(67, 24);
			this.butSetStatus.TabIndex = 14;
			this.butSetStatus.Text = "Set";
			this.butSetStatus.Click += new System.EventHandler(this.butSetStatus_Click);
			// 
			// butLabels
			// 
			this.butLabels.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLabels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butLabels.Autosize = true;
			this.butLabels.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLabels.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLabels.CornerRadius = 4F;
			this.butLabels.Image = global::OpenDental.Properties.Resources.butLabel;
			this.butLabels.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butLabels.Location = new System.Drawing.Point(128, 668);
			this.butLabels.Name = "butLabels";
			this.butLabels.Size = new System.Drawing.Size(119, 24);
			this.butLabels.TabIndex = 122;
			this.butLabels.Text = "Label Preview";
			this.butLabels.Click += new System.EventHandler(this.butLabels_Click);
			// 
			// butReport
			// 
			this.butReport.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butReport.Autosize = true;
			this.butReport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butReport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butReport.CornerRadius = 4F;
			this.butReport.Location = new System.Drawing.Point(350, 642);
			this.butReport.Name = "butReport";
			this.butReport.Size = new System.Drawing.Size(87, 24);
			this.butReport.TabIndex = 121;
			this.butReport.Text = "R&un Report";
			this.butReport.Click += new System.EventHandler(this.butReport_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.checkConflictingTypes);
			this.groupBox1.Controls.Add(this.comboNumberReminders);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.comboSort);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.comboSite);
			this.groupBox1.Controls.Add(this.labelSite);
			this.groupBox1.Controls.Add(this.comboClinic);
			this.groupBox1.Controls.Add(this.labelClinic);
			this.groupBox1.Controls.Add(this.comboProv);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.checkGroupFamilies);
			this.groupBox1.Controls.Add(this.textDateEnd);
			this.groupBox1.Controls.Add(this.textDateStart);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.butRefresh);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(3, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(695, 83);
			this.groupBox1.TabIndex = 120;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "View";
			// 
			// checkConflictingTypes
			// 
			this.checkConflictingTypes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkConflictingTypes.Location = new System.Drawing.Point(26, 33);
			this.checkConflictingTypes.Name = "checkConflictingTypes";
			this.checkConflictingTypes.Size = new System.Drawing.Size(159, 18);
			this.checkConflictingTypes.TabIndex = 40;
			this.checkConflictingTypes.Text = "Show Conflicting Types";
			this.checkConflictingTypes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkConflictingTypes.UseVisualStyleBackColor = true;
			this.checkConflictingTypes.Click += new System.EventHandler(this.checkRecallTypes_Click);
			// 
			// comboNumberReminders
			// 
			this.comboNumberReminders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboNumberReminders.Location = new System.Drawing.Point(103, 57);
			this.comboNumberReminders.MaxDropDownItems = 40;
			this.comboNumberReminders.Name = "comboNumberReminders";
			this.comboNumberReminders.Size = new System.Drawing.Size(82, 21);
			this.comboNumberReminders.TabIndex = 39;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(3, 60);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(99, 14);
			this.label3.TabIndex = 38;
			this.label3.Text = "Show Reminders";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboSort
			// 
			this.comboSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSort.Location = new System.Drawing.Point(242, 56);
			this.comboSort.MaxDropDownItems = 40;
			this.comboSort.Name = "comboSort";
			this.comboSort.Size = new System.Drawing.Size(118, 21);
			this.comboSort.TabIndex = 37;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(186, 59);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(55, 14);
			this.label5.TabIndex = 36;
			this.label5.Text = "Sort";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboSite
			// 
			this.comboSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSite.Location = new System.Drawing.Point(437, 57);
			this.comboSite.MaxDropDownItems = 40;
			this.comboSite.Name = "comboSite";
			this.comboSite.Size = new System.Drawing.Size(160, 21);
			this.comboSite.TabIndex = 25;
			// 
			// labelSite
			// 
			this.labelSite.Location = new System.Drawing.Point(365, 60);
			this.labelSite.Name = "labelSite";
			this.labelSite.Size = new System.Drawing.Size(70, 14);
			this.labelSite.TabIndex = 24;
			this.labelSite.Text = "Site";
			this.labelSite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(437, 34);
			this.comboClinic.MaxDropDownItems = 40;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(160, 21);
			this.comboClinic.TabIndex = 23;
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(365, 37);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(70, 14);
			this.labelClinic.TabIndex = 22;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboProv
			// 
			this.comboProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProv.Location = new System.Drawing.Point(437, 11);
			this.comboProv.MaxDropDownItems = 40;
			this.comboProv.Name = "comboProv";
			this.comboProv.Size = new System.Drawing.Size(160, 21);
			this.comboProv.TabIndex = 21;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(365, 14);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(70, 14);
			this.label4.TabIndex = 20;
			this.label4.Text = "Provider";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkGroupFamilies
			// 
			this.checkGroupFamilies.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkGroupFamilies.Location = new System.Drawing.Point(77, 14);
			this.checkGroupFamilies.Name = "checkGroupFamilies";
			this.checkGroupFamilies.Size = new System.Drawing.Size(108, 18);
			this.checkGroupFamilies.TabIndex = 19;
			this.checkGroupFamilies.Text = "Group Families";
			this.checkGroupFamilies.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkGroupFamilies.UseVisualStyleBackColor = true;
			this.checkGroupFamilies.Click += new System.EventHandler(this.checkGroupFamilies_Click);
			// 
			// textDateEnd
			// 
			this.textDateEnd.Location = new System.Drawing.Point(283, 34);
			this.textDateEnd.Name = "textDateEnd";
			this.textDateEnd.Size = new System.Drawing.Size(77, 20);
			this.textDateEnd.TabIndex = 18;
			// 
			// textDateStart
			// 
			this.textDateStart.Location = new System.Drawing.Point(283, 13);
			this.textDateStart.Name = "textDateStart";
			this.textDateStart.Size = new System.Drawing.Size(77, 20);
			this.textDateStart.TabIndex = 17;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(198, 37);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(82, 14);
			this.label2.TabIndex = 12;
			this.label2.Text = "End Date";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(198, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 14);
			this.label1.TabIndex = 11;
			this.label1.Text = "Start Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(609, 53);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(80, 24);
			this.butRefresh.TabIndex = 2;
			this.butRefresh.Text = "&Refresh List";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// tabPageRecentlyContacted
			// 
			this.tabPageRecentlyContacted.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageRecentlyContacted.Controls.Add(this.gridRecentlyContacted);
			this.tabPageRecentlyContacted.Controls.Add(this.groupBox4);
			this.tabPageRecentlyContacted.Location = new System.Drawing.Point(4, 22);
			this.tabPageRecentlyContacted.Name = "tabPageRecentlyContacted";
			this.tabPageRecentlyContacted.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageRecentlyContacted.Size = new System.Drawing.Size(971, 699);
			this.tabPageRecentlyContacted.TabIndex = 1;
			this.tabPageRecentlyContacted.Text = "Recently Contacted";
			// 
			// gridRecentlyContacted
			// 
			this.gridRecentlyContacted.AllowSortingByColumn = true;
			this.gridRecentlyContacted.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridRecentlyContacted.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridRecentlyContacted.HasAddButton = false;
			this.gridRecentlyContacted.HasDropDowns = false;
			this.gridRecentlyContacted.HasMultilineHeaders = false;
			this.gridRecentlyContacted.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridRecentlyContacted.HeaderHeight = 15;
			this.gridRecentlyContacted.HScrollVisible = true;
			this.gridRecentlyContacted.Location = new System.Drawing.Point(3, 86);
			this.gridRecentlyContacted.Name = "gridRecentlyContacted";
			this.gridRecentlyContacted.ScrollValue = 0;
			this.gridRecentlyContacted.Size = new System.Drawing.Size(965, 607);
			this.gridRecentlyContacted.TabIndex = 127;
			this.gridRecentlyContacted.Title = "Recall Patients Recently Contacted";
			this.gridRecentlyContacted.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridRecentlyContacted.TitleHeight = 18;
			this.gridRecentlyContacted.TranslationName = "";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.butRefreshRecent);
			this.groupBox4.Controls.Add(this.comboClinicRecent);
			this.groupBox4.Controls.Add(this.labelClinicRecent);
			this.groupBox4.Controls.Add(this.datePickerRecent);
			this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox4.Location = new System.Drawing.Point(35, 6);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(395, 74);
			this.groupBox4.TabIndex = 121;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "View";
			// 
			// butRefreshRecent
			// 
			this.butRefreshRecent.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefreshRecent.Autosize = true;
			this.butRefreshRecent.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefreshRecent.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefreshRecent.CornerRadius = 4F;
			this.butRefreshRecent.Location = new System.Drawing.Point(307, 40);
			this.butRefreshRecent.Name = "butRefreshRecent";
			this.butRefreshRecent.Size = new System.Drawing.Size(80, 24);
			this.butRefreshRecent.TabIndex = 2;
			this.butRefreshRecent.Text = "&Refresh List";
			this.butRefreshRecent.Click += new System.EventHandler(this.butRefreshRecent_Click);
			// 
			// comboClinicRecent
			// 
			this.comboClinicRecent.DoIncludeAll = true;
			this.comboClinicRecent.DoIncludeUnassigned = true;
			this.comboClinicRecent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinicRecent.Location = new System.Drawing.Point(227, 14);
			this.comboClinicRecent.MaxDropDownItems = 40;
			this.comboClinicRecent.Name = "comboClinicRecent";
			this.comboClinicRecent.Size = new System.Drawing.Size(160, 21);
			this.comboClinicRecent.TabIndex = 23;
			// 
			// labelClinicRecent
			// 
			this.labelClinicRecent.Location = new System.Drawing.Point(161, 17);
			this.labelClinicRecent.Name = "labelClinicRecent";
			this.labelClinicRecent.Size = new System.Drawing.Size(64, 14);
			this.labelClinicRecent.TabIndex = 22;
			this.labelClinicRecent.Text = "Clinic";
			this.labelClinicRecent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// datePickerRecent
			// 
			this.datePickerRecent.BackColor = System.Drawing.Color.Transparent;
			this.datePickerRecent.DefaultDateTimeFrom = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
			this.datePickerRecent.DefaultDateTimeTo = new System.DateTime(2018, 4, 16, 0, 0, 0, 0);
			this.datePickerRecent.Location = new System.Drawing.Point(6, 13);
			this.datePickerRecent.MaximumSize = new System.Drawing.Size(453, 185);
			this.datePickerRecent.MinimumSize = new System.Drawing.Size(381, 70);
			this.datePickerRecent.Name = "datePickerRecent";
			this.datePickerRecent.Size = new System.Drawing.Size(381, 70);
			this.datePickerRecent.TabIndex = 24;
			// 
			// FormRecallList
			// 
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(985, 761);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(991, 385);
			this.Name = "FormRecallList";
			this.Text = "Recall List";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRecallList_FormClosing);
			this.Load += new System.EventHandler(this.FormRecallList_Load);
			this.tabControl.ResumeLayout(false);
			this.tabPageRecalls.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabPageRecentlyContacted.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormRecallList_Load(object sender, System.EventArgs e) {
			//AptNumsSelected=new List<long>();
			CheckClinicsSignedUpForWebSched();
#if DEBUG
			butECards.Visible=true;
#endif
			checkGroupFamilies.Checked=PrefC.GetBool(PrefName.RecallGroupByFamily);
			for(int i=0;i<Enum.GetNames(typeof(RecallListSort)).Length;i++){
				comboSort.Items.Add(Lan.g("enumRecallListSort",Enum.GetNames(typeof(RecallListSort))[i]));
			}
			comboSort.SelectedIndex=0;
			comboNumberReminders.Items.Add(Lan.g(this,"all"));
			comboNumberReminders.Items.Add("0");
			comboNumberReminders.Items.Add("1");
			comboNumberReminders.Items.Add("2");
			comboNumberReminders.Items.Add("3");
			comboNumberReminders.Items.Add("4");
			comboNumberReminders.Items.Add("5");
			comboNumberReminders.Items.Add("6+");
			comboNumberReminders.SelectedIndex=0;
			int daysPast=PrefC.GetInt(PrefName.RecallDaysPast);
			int daysFuture=PrefC.GetInt(PrefName.RecallDaysFuture);
			if(daysPast==-1){
				textDateStart.Text="";
			}
			else{
				textDateStart.Text=DateTime.Today.AddDays(-daysPast).ToShortDateString();
			}
			if(daysFuture==-1) {
				textDateEnd.Text="";
			}
			else {
				textDateEnd.Text=DateTime.Today.AddDays(daysFuture).ToShortDateString();
			}
			comboProv.Items.Add(Lan.g(this,"All"));
			comboProv.SelectedIndex=0;
			_listProviders=Providers.GetDeepCopy(true);
			for(int i=0;i<_listProviders.Count;i++) {
				comboProv.Items.Add(_listProviders[i].GetLongDesc());
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)){
				comboClinic.Visible=false;
				labelClinic.Visible=false;
				labelClinicRecent.Visible=false;
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
			if(PrefC.GetBool(PrefName.EasyHidePublicHealth)){
				comboSite.Visible=false;
				labelSite.Visible=false;
			}
			else{
				comboSite.Items.Add(Lan.g(this,"All"));
				comboSite.SelectedIndex=0;
				_listSites=Sites.GetDeepCopy();
				for(int i=0;i<_listSites.Count;i++) {
					comboSite.Items.Add(_listSites[i].Description);
				}
			}
			_listRecallUnschedStatusDefs=Defs.GetDefsForCategory(DefCat.RecallUnschedStatus,true);
			comboStatus.Items.Clear();
			comboStatus.Items.Add(Lan.g(this,"none"));
			comboStatus.SelectedIndex=0;
			for(int i=0;i<_listRecallUnschedStatusDefs.Count;i++){
				comboStatus.Items.Add(_listRecallUnschedStatusDefs[i].ItemName);
			}
			FillMain();
			FillComboEmail();
			Plugins.HookAddCode(this,"FormRecallList.Load_End",_tableRecalls);
		}

		private void CheckClinicsSignedUpForWebSched() {
			if(_threadWebSchedSignups!=null) {
				return;
			}
			_threadWebSchedSignups=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				_listClinicNumsWebSched=WebServiceMainHQProxy.GetEServiceClinicsAllowed(
					Clinics.GetDeepCopy().Select(x => x.ClinicNum).ToList(),
					eServiceCode.WebSched);
			}));
			//Swallow all exceptions and allow thread to exit gracefully.
			_threadWebSchedSignups.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception ex) => { 	}));
			_threadWebSchedSignups.AddExitHandler(new ODThread.WorkerDelegate((ODThread o) => {
				ThreadWebSchedSignupsExitHandler();
			}));
			_threadWebSchedSignups.Name="CheckWebSchedSignups";
			_threadWebSchedSignups.Start(true);
		}

		private void ThreadWebSchedSignupsExitHandler() { 
			if(IsDisposed) {
				return;
			}
			if(InvokeRequired) {
				Invoke((Action)(() => { ThreadWebSchedSignupsExitHandler(); }));
				return;
			}
			_threadWebSchedSignups=null;
			Cursor=Cursors.Default;
			if(_hasClickedWebSched) {
				try {
					SendWebSched();
				}
				catch(Exception ex) {
					MessageBox.Show(Lan.g(this,"Error sending Web Sched notifications. Error message:")+" "+ex.Message);
				}
			}
			_hasClickedWebSched=false;
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

		///<summary>OK to pass in null for excludePatNums.</summary>
		private void FillMain(){
			if(textDateStart.errorProvider1.GetError(textDateStart)!=""
				|| textDateEnd.errorProvider1.GetError(textDateEnd)!="")
			{
				return;
			}
			//remember which recallnums were selected
			List<string> recallNums=new List<string>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++){
				recallNums.Add(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["RecallNum"].ToString());
			}
			DateTime fromDate;
			DateTime toDate;
			if(textDateStart.Text==""){
				fromDate=DateTime.MinValue;
			}
			else{
				fromDate=PIn.Date(textDateStart.Text);
			}
			if(textDateEnd.Text=="") {
				toDate=DateTime.MaxValue;
			}
			else {
				toDate=PIn.Date(textDateEnd.Text);
			}
			long provNum=0;
			if(comboProv.SelectedIndex!=0){
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
			long siteNum=0;
			if(!PrefC.GetBool(PrefName.EasyHidePublicHealth) && comboSite.SelectedIndex!=0) {
				siteNum=_listSites[comboSite.SelectedIndex-1].SiteNum;
			}
			RecallListSort sortBy=(RecallListSort)comboSort.SelectedIndex;
			RecallListShowNumberReminders showReminders=(RecallListShowNumberReminders)comboNumberReminders.SelectedIndex;
			bool checkGroupFamilesChecked=checkGroupFamilies.Checked;
			_getRecallTable=new Func<DataTable>(() => {
				//Storing this as a Func so that we can make the exact same call before sending Web Sched.
				return Recalls.GetRecallList(fromDate,toDate,checkGroupFamilesChecked,provNum,clinicNum,siteNum,sortBy,showReminders);
			});
			_tableRecalls=_getRecallTable();
			int scrollval=gridMain.ScrollValue;
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			List<DisplayField> fields=DisplayFields.GetForCategory(DisplayFieldCategory.RecallList);
			for(int i=0;i<fields.Count;i++) {
				if(fields[i].Description=="") {
					col=new ODGridColumn(fields[i].InternalName,fields[i].ColumnWidth);
				}
				else {
					col=new ODGridColumn(fields[i].Description,fields[i].ColumnWidth);
				}
				col.Tag=fields[i].InternalName;
				gridMain.Columns.Add(col);
			}
			gridMain.Rows.Clear();
			ODGridRow row;
			List<long> listConflictingPatNums=new List<long>();
			if(checkConflictingTypes.Checked) {
				listConflictingPatNums=GetConflictingPatNums(_tableRecalls.Rows.OfType<DataRow>().Select(x => PIn.Long(x["PatNum"].ToString())).ToList());
			}
			for(int i=0;i<_tableRecalls.Rows.Count;i++){
				if(checkConflictingTypes.Checked) {
					//If the RecallType checkbox is checked, show patients with future scheduled appointments that have conflicting recall appointments.
					//Ex. A patient is scheduled for a perio recall but their recall type is set to prophy
					long patNum=PIn.Long(_tableRecalls.Rows[i]["PatNum"].ToString());
					long recallTypeNum=PIn.Long(_tableRecalls.Rows[i]["RecallTypeNum"].ToString());
					if(!patNum.In(listConflictingPatNums)) {
						//The patient does not have any conflicting recall type.
						//Continue since we don't want to show them when the RecallTypes checkbox is checked. 
						continue;
					}
					if(!RecallTypes.IsSpecialRecallType(recallTypeNum)) {
						//Make sure recall type is Perio or Prophy
						continue;
					}
				}
				row=new ODGridRow();
				for(int f=0;f<fields.Count;f++) {
					switch(fields[f].InternalName){
						case "Due Date":
							row.Cells.Add(_tableRecalls.Rows[i]["dueDate"].ToString());
							break;
						case "Patient":
							row.Cells.Add(_tableRecalls.Rows[i]["patientName"].ToString());
							break;
						case "Age":
							row.Cells.Add(_tableRecalls.Rows[i]["age"].ToString());
							break;
						case "Type":
							row.Cells.Add(_tableRecalls.Rows[i]["recallType"].ToString());
							break;
						case "Interval":
							row.Cells.Add(_tableRecalls.Rows[i]["recallInterval"].ToString());
							break;
						case "#Remind":
							row.Cells.Add(_tableRecalls.Rows[i]["numberOfReminders"].ToString());
							break;
						case "LastRemind":
							row.Cells.Add(_tableRecalls.Rows[i]["dateLastReminder"].ToString());
							break;
						case "Contact":
							row.Cells.Add(_tableRecalls.Rows[i]["contactMethod"].ToString());
							break;
						case "Status":
							row.Cells.Add(_tableRecalls.Rows[i]["status"].ToString());
							break;
						case "Note":
							row.Cells.Add(_tableRecalls.Rows[i]["Note"].ToString());
							break;
						case "BillingType":
							row.Cells.Add(_tableRecalls.Rows[i]["billingType"].ToString());
							break;
						case "WebSched":
							row.Cells.Add(_tableRecalls.Rows[i]["webSchedSendDesc"].ToString());
							break;
					}
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			//reselect original items
			for(int i=0;i<_tableRecalls.Rows.Count;i++){
				if(recallNums.Contains(_tableRecalls.Rows[i]["RecallNum"].ToString())){
					gridMain.SetSelected(i,true);
				}
			}
			labelPatientCount.Text=Lan.g(this,"Patient Count:")+" "+_tableRecalls.Rows.Count.ToString();
		}

		///<summary>Returns a list of PatNums of patients with conflicting Recall type.
		///A conflicting recall type is when a patient is scheduled for a perio recall but their recall type is set to prophy and vice versa.
		///Only checks for Prophy and Perio recall types.</summary>
		private List<long> GetConflictingPatNums(List<long> listPatNums) {
			List<long> retVal=new List<long>();
			Dictionary<long,List<Procedure>> dictProcsOnFutureApts=new Dictionary<long, List<Procedure>>();
			List<RecallType> listRecallTypePractice=RecallTypes.GetActive().FindAll(x => RecallTypes.IsSpecialRecallType(x.RecallTypeNum));
			List<ProcedureCode> listRecallProcsPractice=ProcedureCodes.GetFromCommaDelimitedList(
				string.Join(",",listRecallTypePractice.Select(x=>x.Procedures).ToList()));
			//List of practice recall TP procedures on future scheduled appointments for the listPatNums.
			List<Procedure> listProcsOnFutureApts=Procedures.GetProcsAttatchedToFutureAppt(listPatNums,
				listRecallProcsPractice.Select(x => x.CodeNum).ToList());
			//Dictionary of PatNums and List of Procedures.
			dictProcsOnFutureApts=listProcsOnFutureApts.GroupBy(x => x.PatNum)
				.ToDictionary(x => x.Key,x => x.ToList());
			//Check for conflicting recall types
			//A conflicting recall type is when a patient is scheduled for a perio recall but their recall type is set to prophy and vice versa.
			//Only Checks for Prophy and Perio recall types.
			List<Recall> listRecalls=Recalls.GetList(listPatNums);
			foreach(KeyValuePair<long,List<Procedure>> pat in dictProcsOnFutureApts) {
				List<ProcedureCode> listOppositeRecallProcs=new List<ProcedureCode>();
				List<Recall> listRecallPat=listRecalls.FindAll(x => x.PatNum==pat.Key);
				if(listRecallPat.Count==0) {
					continue;
				}
				if(RecallTypes.ProphyType.In(listRecallPat.Select(x => x.RecallTypeNum).ToList())) {
					//Patient has Prophy recall type set. Get procs for Perio recall type.
					listOppositeRecallProcs=ProcedureCodes.GetFromCommaDelimitedList(
						string.Join(",",RecallTypes.GetProcs(RecallTypes.PerioType)));
				}
				else {
					//Patient has Perio recall type set. Get procs for Prophy recall type.
					listOppositeRecallProcs=ProcedureCodes.GetFromCommaDelimitedList(
						string.Join(",",RecallTypes.GetProcs(RecallTypes.ProphyType).Union(RecallTypes.GetProcs(RecallTypes.ChildProphyType))));
				}
				List<long> listCodeNumsOppositeRecallType=listOppositeRecallProcs.Select(x=>x.CodeNum).ToList();
				List<long> listCodeNumsOnFutureApt=pat.Value.Select(x=>x.CodeNum).ToList();
				//Recall confliction exists when the patients future scheduled TP procedures contain all of the procedures of the conflicting recall type.
				if(listCodeNumsOppositeRecallType.All(x => x.In(listCodeNumsOnFutureApt))) {
					//PatNum has conflicting recall type. 
					retVal.Add(pat.Key);
				}
			}
			return retVal;
		}

		private void gridMain_CellClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			//row selected before this event triggered
			SetFamilyColors();
			//comboStatus.SelectedIndex=-1;//mess with this later
			//SelectedPatNum=PIn.PLong(table.Rows[e.Row]["PatNum"].ToString());
		}

		private void SetFamilyColors() {
			if(gridMain.SelectedIndices.Length!=1) {
				for(int i=0;i<gridMain.Rows.Count;i++) {
					gridMain.Rows[i].ColorText=Color.Black;
				}
				gridMain.Invalidate();
				return;
			}
			long guar=PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[0]]["Guarantor"].ToString());
			int famCount=0;
			for(int i=0;i<gridMain.Rows.Count;i++) {
				if(PIn.Long(_tableRecalls.Rows[i]["Guarantor"].ToString())==guar){
					famCount++;
					gridMain.Rows[i].ColorText=Color.Red;
				}
				else {
					gridMain.Rows[i].ColorText=Color.Black;
				}
			}
			if(famCount==1) {//only the highlighted patient is red at this point
				gridMain.Rows[gridMain.SelectedIndices[0]].ColorText=Color.Black;
			}
			gridMain.Invalidate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(gridMain.Columns[e.Col].Tag.ToString()=="WebSched") {//A column's tag is its display field internal name.
				MsgBoxCopyPaste msgBox=new MsgBoxCopyPaste(PIn.String(_tableRecalls.Rows[e.Row]["webSchedSendError"].ToString()));
				msgBox.Text=Lan.g(this,"Web Sched Notification Send Error");
				msgBox.ShowDialog();
				return;
			}
			SelectedPatNum=PIn.Long(_tableRecalls.Rows[e.Row]["PatNum"].ToString());
			Recall recall=Recalls.GetRecall(PIn.Long(_tableRecalls.Rows[e.Row]["RecallNum"].ToString()));
			FormRecallEdit FormR=new FormRecallEdit();
			FormR.RecallCur=recall.Copy();
			FormR.ShowDialog();
			if(FormR.DialogResult!=DialogResult.OK) {
				return;
			}
			if(recall.RecallStatus!=FormR.RecallCur.RecallStatus//if the status has changed
				|| (recall.IsDisabled != FormR.RecallCur.IsDisabled)//or any of the three disabled fields was changed
				|| (recall.DisableUntilDate != FormR.RecallCur.DisableUntilDate)
				|| (recall.DisableUntilBalance != FormR.RecallCur.DisableUntilBalance)
				|| (recall.Note != FormR.RecallCur.Note))//or a note was added
			{
				//make a commlog entry
				//unless there is an existing recall commlog entry for today
				bool recallEntryToday=false;
				List<Commlog> CommlogList=Commlogs.Refresh(SelectedPatNum);
				for(int i=0;i<CommlogList.Count;i++) {
					if(CommlogList[i].CommDateTime.Date==DateTime.Today	
						&& CommlogList[i].CommType==Commlogs.GetTypeAuto(CommItemTypeAuto.RECALL)) {
						recallEntryToday=true;
					}
				}
				if(!recallEntryToday) {
					Commlog CommlogCur=new Commlog();
					CommlogCur.CommDateTime=DateTime.Now;
					CommlogCur.CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.RECALL);
					CommlogCur.PatNum=SelectedPatNum;
					CommlogCur.Note="";
					if(recall.RecallStatus!=FormR.RecallCur.RecallStatus) {
						if(FormR.RecallCur.RecallStatus==0) {
							CommlogCur.Note+=Lan.g(this,"Status None");
						}
						else {
							CommlogCur.Note+=Defs.GetName(DefCat.RecallUnschedStatus,FormR.RecallCur.RecallStatus);
						}
					}
					if(recall.DisableUntilDate!=FormR.RecallCur.DisableUntilDate && FormR.RecallCur.DisableUntilDate.Year>1880) {
						if(CommlogCur.Note!="") {
							CommlogCur.Note+=",  ";
						}
						CommlogCur.Note+=Lan.g(this,"Disabled until ")+FormR.RecallCur.DisableUntilDate.ToShortDateString();
					}
					if(recall.DisableUntilBalance!=FormR.RecallCur.DisableUntilBalance && FormR.RecallCur.DisableUntilBalance>0) {
						if(CommlogCur.Note!="") {
							CommlogCur.Note+=",  ";
						}
						CommlogCur.Note+=Lan.g(this,"Disabled until balance below ")+FormR.RecallCur.DisableUntilBalance.ToString("c");
					}
					if(recall.Note!=FormR.RecallCur.Note) {
						if(CommlogCur.Note!="") {
							CommlogCur.Note+=",  ";
						}
						CommlogCur.Note+=FormR.RecallCur.Note;
					}
					CommlogCur.Note+=".  ";
					CommlogCur.UserNum=Security.CurUser.UserNum;
					FormCommItem FormCI=new FormCommItem();
					FormCI.ShowDialog(new CommItemModel() { CommlogCur=CommlogCur },new CommItemController(FormCI) { IsNew=true });
				}
			}
			FillMain();
			for(int i=0;i<gridMain.Rows.Count;i++) {
				if(PIn.Long(_tableRecalls.Rows[i]["PatNum"].ToString())==SelectedPatNum){
					gridMain.SetSelected(i,true);
				}
			}
			SetFamilyColors();
		}

		private void butSchedPat_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a patient first.");
				return;
			}
			if(gridMain.SelectedIndices.Length>1) {
				MsgBox.Show(this,"Please select only one patient first.");
				return;
			}
			if(!Security.IsAuthorized(Permissions.AppointmentCreate)) {
				return;
			}
			SelectedPatNum=PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[0]]["PatNum"].ToString());
			if(PatRestrictionL.IsRestricted(SelectedPatNum,PatRestrict.ApptSchedule)) {
				return;
			}
			Family fam=Patients.GetFamily(SelectedPatNum);
			Patient pat=fam.GetPatient(SelectedPatNum);
			List<Procedure> procList;
			Appointment apt=null;
			procList=Procedures.Refresh(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			long recallNum=PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[0]]["RecallNum"].ToString());
			try{
				apt=AppointmentL.CreateRecallApt(pat,procList,planList,recallNum,subList);
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
				return;
			}
			//PinClicked=true;
			//AptNumsSelected.Add(apt.AptNum);
			WindowState=FormWindowState.Minimized;
			List<long> pinAptNums=new List<long>();
			pinAptNums.Add(apt.AptNum);
			GotoModule.PinToAppt(pinAptNums,SelectedPatNum);
			//no securitylog entry.  It will happen when they drag off pinboard.
			gridMain.SetSelected(false);
			FillMain();
		}

		private void butSchedFam_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0){
				MsgBox.Show(this,"Please select a patient first.");
				return;
			}
			if(gridMain.SelectedIndices.Length>1) {
				MsgBox.Show(this,"Please select only one patient first.");
				return;
			}
			if(!Security.IsAuthorized(Permissions.AppointmentCreate)) {
				return;
			}
			SelectedPatNum=PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[0]]["PatNum"].ToString());
			Family fam=Patients.GetFamily(SelectedPatNum);
			List<Procedure> procList;
			List <InsPlan> planList;
			List<InsSub> subList;
			Appointment apt;
			//List<long> patNums;
			List<long> pinAptNums=new List<long>();
			int patsRestricted=0;
			for(int i=0;i<fam.ListPats.Length;i++) {
				if(PatRestrictionL.IsRestricted(fam.ListPats[i].PatNum,PatRestrict.ApptSchedule,true)) {
					patsRestricted++;
					continue;
				}
				procList=Procedures.Refresh(fam.ListPats[i].PatNum);
				subList=InsSubs.RefreshForFam(fam);
				planList=InsPlans.RefreshForSubList(subList);
				try{
					apt=AppointmentL.CreateRecallApt(fam.ListPats[i],procList,planList,-1,subList);
				}
				catch{//(Exception ex){
					continue;
				}
				pinAptNums.Add(apt.AptNum);
			}
			if(patsRestricted>0) {
				MessageBox.Show(Lan.g(this,"Family members skipped due to patient restriction")+" "+PatRestrictions.GetPatRestrictDesc(PatRestrict.ApptSchedule)
					+": "+patsRestricted+".");
			}
			if(pinAptNums.Count==0) {
				MsgBox.Show(this,"No recall is due.");
				return;
			}
			WindowState=FormWindowState.Minimized;
			GotoModule.PinToAppt(pinAptNums,SelectedPatNum);
			//no securitylog entry needed.  It will be made as each appt is dragged off pinboard.
			gridMain.SetSelected(false);
			FillMain();
		}

		///<summary>Automatically open the eService Setup window so that they can easily click the Enable button. 
		///Calls CheckClinicsSignedUpForWebSched() before exiting.</summary>
		private void OpenSignupPortal() {
			FormEServicesSetup FormESS=new FormEServicesSetup(FormEServicesSetup.EService.SignupPortal);
			FormESS.ShowDialog();
			//User may have made changes to signups. Reload the valid clinics from HQ.
			CheckClinicsSignedUpForWebSched();
		}

		private void panelWebSched_MouseClick(object sender,MouseEventArgs e) {
			SendWebSched();
		}

		private void SendWebSched() {
			#region Check Web Sched Pref and Show Promo
			if(IsDisposed) {//The user closed the form while the thread checking Web Sched signups was still running.
				return;
			}
			if(!_isDoneCheckingWebSchedClinics) {//The thread has not finished getting the list. 
				_hasClickedWebSched=true;//The thread checking Web Sched signups will call this method on exit.
				Cursor=Cursors.AppStarting;
				return;
			}
			if(_listClinicNumsWebSched.Count==0) {//No clinics are signed up for Web Sched
				string message=PrefC.HasClinicsEnabled ?
					"No clinics are signed up for Web Sched Recall. Open Sign Up Portal?" : 
					"This practice is not signed up for Web Sched Recall. Open Sign Up Portal?";
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,message)) {
					return;
				}
				OpenSignupPortal();
				return;
			}
			//At least one clinic is signed up for Web Sched.
			List<long> listClinicNumsNotSignedUp=new List<long>();
			List<int> listGridIndicesNotSignUp=new List<int>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				long clinicNum=PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["ClinicNum"].ToString());
				//We don't want to send users to the sign up portal for clinic 0 if clinics are enabled because there will be nothing for them to do there. 
				if(clinicNum==0 && !_listClinicNumsWebSched.Contains(0)) {
					continue;//We will deselect these rows later.
				}
				if(!_listClinicNumsWebSched.Contains(clinicNum)) {
					listClinicNumsNotSignedUp.Add(clinicNum);
				}
				listGridIndicesNotSignUp.Add(i);
			}
			if(listClinicNumsNotSignedUp.Count > 0) {
				string message=Lan.g(this,"You have selected recalls whose clinic is not signed up for Web Sched recall. "
					+"Do you want to go to the sign up portal to sign these clinics up? "
					+"Clicking 'No' will deselect these recalls and send the remaining.");
				if(MessageBox.Show(message,"",MessageBoxButtons.YesNo)==DialogResult.Yes) {
					OpenSignupPortal();
					return;
				}
				//De-select any rows that are not allowed to send WebSched.				
				for(int i=0;i<listGridIndicesNotSignUp.Count;i++) {
					gridMain.SetSelected(listGridIndicesNotSignUp[i],false);
				}
			}
			#endregion
			#region Recall List Validation
			if(gridMain.Rows.Count < 1) {
				MessageBox.Show(Lan.g(this,"There are no Patients in the Recall table.  Must have at least one."));
				return;
			}
			if(!EmailAddresses.ExistsValidEmail()) {
				MsgBox.Show(this,"You need to enter an SMTP server name in email setup before you can send email.");
				return;
			}
			if(PrefC.GetLong(PrefName.RecallStatusEmailed)==0
				|| PrefC.GetLong(PrefName.RecallStatusTexted)==0
				|| PrefC.GetLong(PrefName.RecallStatusEmailedTexted)==0) 
			{
				MsgBox.Show(this,"You need to set an email status, text status, and email and text status first in the Recall Setup window.");
				return;
			}
#if !DEBUG
			if(EServiceSignals.GetListenerServiceStatus().In(
				eServiceSignalSeverity.None,
				eServiceSignalSeverity.NotEnabled,
				eServiceSignalSeverity.Critical)) 
			{
				MsgBox.Show(this,"Your eConnector is not currently running. Please enable the eConnector to send Web Sched Recalls.");
				return;
			}
#endif
			//If the user didn't manually select any recalls we will automatically select all rows that have an email or text prefer recall method.
			if(gridMain.SelectedIndices.Length==0) {
				ContactMethod cmeth;
				for(int i=0;i<_tableRecalls.Rows.Count;i++) {
					cmeth=(ContactMethod)PIn.Long(_tableRecalls.Rows[i]["PreferRecallMethod"].ToString());
					if(!cmeth.In(ContactMethod.Email,ContactMethod.TextMessage)) {
						continue;
					}
					gridMain.SetSelected(i,true);
				}
			}
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"No patients prefer contact via email or text.");
				return;
			}
			//Now that there are rows guaranteed to be selected, check each row to see if their recall will yield available Web Sched time slots.
			//Deselect the ones that do not have email or wireless phone specified or are assigned to clinic num 0 when clinics are enabled.
			//Also deselect ones that were just sent but are still in the list because the grid hasn't refreshed yet.
			int skippedContact=0;
			int skippedTimeSlot=0;
			int skippedClinic0=0;
			int skippedNotInList=0;
			DataTable tableRecallsCur=_getRecallTable();
			List<long> listPatNumsInTableRecallCur=tableRecallsCur.Select().Select(x => PIn.Long(x["PatNum"].ToString())).ToList();
			for(int i=gridMain.SelectedIndices.Length-1;i>=0;i--) {
				DataRow row=_tableRecalls.Rows[gridMain.SelectedIndices[i]];
				//Check that they at least have an email or wireless phone.
				string email=_tableRecalls.Rows[gridMain.SelectedIndices[i]]["Email"].ToString();
				string wirelessPhone=_tableRecalls.Rows[gridMain.SelectedIndices[i]]["WirelessPhone"].ToString();
				if(email.Trim()=="" && wirelessPhone.Trim()=="") {
					skippedContact++;
					gridMain.SetSelected(gridMain.SelectedIndices[i],false);
					continue;
				}
				long clinicNum=PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["ClinicNum"].ToString());
				//If this practice has clinics enabled for Web Sched, then they will not have Web Sched enabled for clinic num 0. They will need to assign
				//patients to a clinic in order to send Web Sched. We will prompt them below to assign them.
				if(clinicNum==0 && !_listClinicNumsWebSched.Contains(0)) {
					skippedClinic0++;
					gridMain.SetSelected(gridMain.SelectedIndices[i],false);
					continue;
				}
				//Check that the patient is still in the recall list (they haven't been sent something since the grid has been refreshed)
				long patNum=PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["PatNum"].ToString());
				if(!patNum.In(listPatNumsInTableRecallCur)) {
					skippedNotInList++;
					gridMain.SetSelected(gridMain.SelectedIndices[i],false);
					continue;
				}
				AutoCommStatus smsSendStatus=PIn.Enum<AutoCommStatus>(PIn.Int(row["webSchedSmsSendStatus"].ToString()));
				AutoCommStatus emailSendStatus=PIn.Enum<AutoCommStatus>(PIn.Int(row["webSchedEmailSendStatus"].ToString()));
				//The eConnector will attempt to send a webschedrecall if either SmsSendStatus or EmailSendStatus is SendNotAttempted and neither of them
				//are SendFailed or SendSuccessful.
				if((smsSendStatus==AutoCommStatus.SendNotAttempted || emailSendStatus==AutoCommStatus.SendNotAttempted)
					&& smsSendStatus.In(AutoCommStatus.SendNotAttempted,AutoCommStatus.DoNotSend)
					&& emailSendStatus.In(AutoCommStatus.SendNotAttempted,AutoCommStatus.DoNotSend))
				{
					continue;//The eConnector is about to send this anyway.
				}
				//Check to see if they'll have any potential time slots via their Web Sched link.
				long recallNum=PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["RecallNum"].ToString());
				DateTime dateDue=PIn.Date(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["DateDue"].ToString());
				if(dateDue.Date<=DateTime.Now.Date) {
					dateDue=DateTime.Now;
				}
				DateTime dateEnd=dateDue.AddMonths(2);
				//This takes a long time to run for lots of recalls.  Might consider making a faster overload in the future (213 recalls ~ 10 seconds).
				bool hasTimeSlots=false;
				try {
					hasTimeSlots=(OpenDentBusiness.WebTypes.WebSched.TimeSlot.TimeSlots.GetAvailableWebSchedTimeSlots(recallNum,dateDue,dateEnd).Count > 0);
				}
				catch(Exception) {
				}
				if(!hasTimeSlots) {
					skippedTimeSlot++;
					gridMain.SetSelected(gridMain.SelectedIndices[i],false);
					continue;
				}
			}
			string skippedMessage="";
			if(skippedContact > 0) {
				skippedMessage+=Lan.g(this,"Selected patients skipped due to missing email addresses and wireless phone:")+" "
					+skippedContact.ToString()+"\r\n";
			}
			if(skippedTimeSlot > 0) {
				skippedMessage+=Lan.g(this,"Selected patients skipped due to no available Web Sched time slots found:")+" "
					+skippedTimeSlot.ToString()+"\r\n";
			}
			if(skippedClinic0 > 0) {
				skippedMessage+=Lan.g(this,"Selected patients skipped due to not being assigned to a clinic:")+" "
					+skippedClinic0.ToString()+"\r\n";
			}
			if(skippedNotInList > 0) {
				FillMain();
				skippedMessage+=Lan.g(this,"Selected patients skipped due to no longer being in the recall list:")+" "
					+skippedNotInList.ToString()+"\r\n";
			}
			if(skippedMessage != "") {
				MessageBox.Show(skippedMessage);
			}
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"No Web Sched emails or texts sent.");
				return;
			}
#endregion
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Send Web Sched emails and/or texts to all of the selected patients?")) {
				return;
			}
			Cursor.Current=Cursors.WaitCursor;
			List<long> recallNums=new List<long>();
			//Loop through all selected patients and grab their corresponding RecallNum.
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				recallNums.Add(PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["RecallNum"].ToString()));
			}
			EmailAddress emailAddressFrom;
			if(comboEmailFrom.SelectedIndex==0) {
				//clinic/practice default. Set to null here to pull patient's clinic email address in Recalls.SendWebSchedNotifications().
				emailAddressFrom=null;
			}
			else { //me or static email address, email address for 'me' is the first one in _listEmailAddresses
				emailAddressFrom=_listEmailAddresses[comboEmailFrom.SelectedIndex-1];//-1 to account for predefined "Clinic/Practice" items in combobox
			}
			List<string> listWebSchedErrors=Recalls.PrepWebSchedNotifications(recallNums
				,checkGroupFamilies.Checked
				,(RecallListSort)comboSort.SelectedIndex
				,WebSchedRecallSource.FormRecallList
				,emailAddressFrom);
			Cursor=Cursors.Default;
			if(listWebSchedErrors.Count > 0) {
				//Show the error (already translated) to the user and then refresh the grid in case any were successful.
				MsgBoxCopyPaste msgBCP=new MsgBoxCopyPaste(string.Join("\r\n",listWebSchedErrors));
				msgBCP.Show();
			}
			FillMain();
		}

		private void checkGroupFamilies_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			FillMain();
			Cursor=Cursors.Default;
		}

		private void checkRecallTypes_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			FillMain();
			Cursor=Cursors.Default;
		}

		private void butReport_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.UserQuery)) {
				return;
			}
		  if(gridMain.Rows.Count < 1){
        MessageBox.Show(Lan.g(this,"There are no Patients in the Recall table.  Must have at least one to run report."));    
        return;
      }
			List<long> recallNums=new List<long>();
      if(gridMain.SelectedIndices.Length < 1){
        for(int i=0;i<gridMain.Rows.Count;i++){
          recallNums.Add(PIn.Long(_tableRecalls.Rows[i]["RecallNum"].ToString()));
        }
      }
      else{
        for(int i=0;i<gridMain.SelectedIndices.Length;i++){
          recallNums.Add(PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["RecallNum"].ToString()));
        }
      }
      FormRpRecall FormRPR=new FormRpRecall(recallNums);
      FormRPR.ShowDialog();
		}

		private void butLabels_Click(object sender, System.EventArgs e) {
			if(gridMain.Rows.Count < 1){
        MessageBox.Show(Lan.g(this,"There are no Patients in the Recall table.  Must have at least one to print."));    
        return;
      }
			if(PrefC.GetLong(PrefName.RecallStatusMailed)==0){
				MsgBox.Show(this,"You need to set a status first in the Recall Setup window.");
				return;
			}
			if(gridMain.SelectedIndices.Length==0){
				ContactMethod cmeth;
				for(int i=0;i<_tableRecalls.Rows.Count;i++){
					if(_tableRecalls.Rows[i]["status"].ToString()!=""){//we only want rows without a status
						continue;
					}
					cmeth=(ContactMethod)PIn.Long(_tableRecalls.Rows[i]["PreferRecallMethod"].ToString());
					if(cmeth!=ContactMethod.Mail && cmeth!=ContactMethod.None){
						continue;
					}
					gridMain.SetSelected(i,true);
				}
				if(gridMain.SelectedIndices.Length==0){
					MsgBox.Show(this,"No patients of mail type.");
					return;
				}
				if(!MsgBox.Show(this,true,"Preview labels for all of the selected patients?")) {
					return;
				}
			}
			List<long> recallNums=new List<long>();
      for(int i=0;i<gridMain.SelectedIndices.Length;i++){
        recallNums.Add(PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["RecallNum"].ToString()));
      }
			RecallListSort sortBy=(RecallListSort)comboSort.SelectedIndex;
			addrTable=Recalls.GetAddrTable(recallNums,checkGroupFamilies.Checked,sortBy);
			pagesPrinted=0;
			patientsPrinted=0;
			pd=new PrintDocument();
			pd.PrintPage+=new PrintPageEventHandler(this.pdLabels_PrintPage);
			pd.OriginAtMargins=true;
			pd.DefaultPageSettings.Margins=new Margins(0,0,0,0);
			printPreview=new FormPrintPreview(PrintSituation.LabelSheet
				,pd,(int)Math.Ceiling((double)addrTable.Rows.Count/30),0,"Recall list labels printed");
			//printPreview.Document=pd;
			//printPreview.TotalPages=;
			printPreview.ShowDialog();
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Change statuses and make commlog entries for all of the selected patients?")) {
				Cursor=Cursors.WaitCursor;
				for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
					//make commlog entries for each patient
					Commlogs.InsertForRecall(PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["PatNum"].ToString()),CommItemMode.Mail,
						PIn.Int(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["numberOfReminders"].ToString()),PrefC.GetLong(PrefName.RecallStatusMailed));
				}
				for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
					Recalls.UpdateStatus(
						PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["RecallNum"].ToString()),PrefC.GetLong(PrefName.RecallStatusMailed));
				}
			}
			FillMain();
			Cursor=Cursors.Default;
		}

		private void butLabelOne_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select patient(s) first.");
				return;
			}
			List<long> recallNums=new List<long>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				recallNums.Add(PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["RecallNum"].ToString()));
			}
			RecallListSort sortBy=(RecallListSort)comboSort.SelectedIndex;
			addrTable=Recalls.GetAddrTable(recallNums,checkGroupFamilies.Checked,sortBy);
			patientsPrinted=0;
			string text;
			while(patientsPrinted<addrTable.Rows.Count) {
				text="";
				if(checkGroupFamilies.Checked && addrTable.Rows[patientsPrinted]["famList"].ToString()!="") {//print family label
					text=addrTable.Rows[patientsPrinted]["guarLName"].ToString()+" "+Lan.g(this,"Household")+"\r\n";
				}
				else {//print single label
					text=addrTable.Rows[patientsPrinted]["patientNameFL"].ToString()+"\r\n";
				}
				text+=addrTable.Rows[patientsPrinted]["address"].ToString()+"\r\n";
				text+=addrTable.Rows[patientsPrinted]["City"].ToString()+", "
					+addrTable.Rows[patientsPrinted]["State"].ToString()+" "
					+addrTable.Rows[patientsPrinted]["Zip"].ToString()+"\r\n";
				LabelSingle.PrintText(0,text);
				patientsPrinted++;
			}
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Did all the labels finish printing correctly?  Statuses will be changed and commlog entries made for all of the selected patients.  Click Yes only if labels printed successfully.")) {
				Cursor=Cursors.WaitCursor;
				for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
					//make commlog entries for each patient
					Commlogs.InsertForRecall(PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["PatNum"].ToString()),CommItemMode.Mail,
						PIn.Int(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["numberOfReminders"].ToString()),PrefC.GetLong(PrefName.RecallStatusMailed));
				}
				for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
					Recalls.UpdateStatus(
						PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["RecallNum"].ToString()),PrefC.GetLong(PrefName.RecallStatusMailed));
				}
			}
			FillMain();
			Cursor=Cursors.Default;
		}

		///<summary>Changes made to printing recall postcards need to be made in FormConfirmList.butPostcards_Click() as well.</summary>
		private void butPostcards_Click(object sender,System.EventArgs e) {
			if(gridMain.Rows.Count < 1) {
				MessageBox.Show(Lan.g(this,"There are no Patients in the Recall table.  Must have at least one to print."));
				return;
			}
			if(PrefC.GetLong(PrefName.RecallStatusMailed)==0) {
				MsgBox.Show(this,"You need to set a status first in the Recall Setup window.");
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {
				ContactMethod cmeth;
				for(int i=0;i<_tableRecalls.Rows.Count;i++) {
					//if(table.Rows[i]["status"].ToString()!=""){//we only want rows without a status
					//	continue;
					//}
					cmeth=(ContactMethod)PIn.Long(_tableRecalls.Rows[i]["PreferRecallMethod"].ToString());
					if(cmeth!=ContactMethod.Mail && cmeth!=ContactMethod.None) {
						continue;
					}
					gridMain.SetSelected(i,true);
				}
				if(gridMain.SelectedIndices.Length==0) {
					MsgBox.Show(this,"No patients of mail type.");
					return;
				}
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Preview postcards for all of the selected patients?")) {
					return;
				}
			}
			List<long> recallNums=new List<long>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				recallNums.Add(PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["RecallNum"].ToString()));
			}
			RecallListSort sortBy=(RecallListSort)comboSort.SelectedIndex;
			addrTable=Recalls.GetAddrTable(recallNums,checkGroupFamilies.Checked,sortBy);
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
			int totalPages=(int)Math.Ceiling((double)addrTable.Rows.Count/(double)PrefC.GetLong(PrefName.RecallPostcardsPerSheet));
			printPreview=new FormPrintPreview(PrintSituation.Postcard,pd,totalPages,0,"Recall list postcards printed");
			printPreview.ShowDialog();
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Did all the postcards finish printing correctly?  Statuses will be changed and commlog entries made for all of the selected patients.  Click Yes only if postcards printed successfully.")) {
				Cursor=Cursors.WaitCursor;
				for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
					//make commlog entries for each patient
					Commlogs.InsertForRecall(PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["PatNum"].ToString()),CommItemMode.Mail,
						PIn.Int(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["numberOfReminders"].ToString()),PrefC.GetLong(PrefName.RecallStatusMailed));
				}
				for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
					Recalls.UpdateStatus(
						PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["RecallNum"].ToString()),PrefC.GetLong(PrefName.RecallStatusMailed));
				}
			}
			FillMain();
			Cursor=Cursors.Default;
		}

		private void butUndo_Click(object sender,EventArgs e) {
			FormRecallListUndo form=new FormRecallListUndo();
			form.ShowDialog();
			if(form.DialogResult==DialogResult.OK) {
				FillMain();
			}
		}

		private void butECards_Click(object sender,EventArgs e) {
			if(!Programs.IsEnabled(ProgramName.Divvy)) {
				if(MsgBox.Show(this,MsgBoxButtons.OKCancel,"The Divvy Program Link is not enabled. Would you like to enable it now?")) {
					FormProgramLinkEdit FormPE=new FormProgramLinkEdit();
					FormPE.ProgramCur=Programs.GetCur(ProgramName.Divvy);
					FormPE.ShowDialog();
					DataValid.SetInvalid(InvalidType.Programs);
				}
				if(!Programs.IsEnabled(ProgramName.Divvy)) {
					return;
				}
			}
			if(gridMain.Rows.Count < 1) {
				MessageBox.Show(Lan.g(this,"There are no Patients in the Recall table.  Must have at least one to send."));
				return;
			}
			if(PrefC.GetLong(PrefName.RecallStatusMailed)==0) {
				MsgBox.Show(this,"You need to set a status first in the Recall Setup window.");
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {
				ContactMethod cmeth;
				for(int i=0;i<_tableRecalls.Rows.Count;i++) {
					cmeth=(ContactMethod)PIn.Long(_tableRecalls.Rows[i]["PreferRecallMethod"].ToString());
					if(cmeth!=ContactMethod.Mail && cmeth!=ContactMethod.None) {
						continue;
					}
					gridMain.SetSelected(i,true);
				}
				if(gridMain.SelectedIndices.Length==0) {
					MsgBox.Show(this,"No patients of mail type.");
					return;
				}
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Send postcards for all of the selected patients?")) {
				return;
			}
			RecallListSort sortBy=(RecallListSort)comboSort.SelectedIndex;
			List<long> recallNums=new List<long>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				recallNums.Add(PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["RecallNum"].ToString()));
			}
			addrTable=Recalls.GetAddrTable(recallNums,checkGroupFamilies.Checked,sortBy);
			DivvyConnect.Postcard postcard;
			DivvyConnect.Recipient recipient;
			DivvyConnect.Postcard[] listPostcards=new DivvyConnect.Postcard[gridMain.SelectedIndices.Length];
			string message;
			long clinicNum;
			Clinic clinic;
			string phone;
			for(int i=0;i<addrTable.Rows.Count;i++) {
				postcard=new DivvyConnect.Postcard();
				recipient=new DivvyConnect.Recipient();
				recipient.Name=addrTable.Rows[i]["patientNameFL"].ToString();
				recipient.ExternalRecipientID=addrTable.Rows[i]["patNums"].ToString();
				recipient.Address1=addrTable.Rows[i]["Address"].ToString();//Includes Address2
				recipient.City=addrTable.Rows[i]["City"].ToString();
				recipient.State=addrTable.Rows[i]["State"].ToString();
				recipient.Zip=addrTable.Rows[i]["Zip"].ToString();
				postcard.AppointmentDateTime=PIn.Date(addrTable.Rows[i]["dateDue"].ToString());//js I don't know why they would ask for this.  We put this in our message.
				//Body text, family card ------------------------------------------------------------------
				if(checkGroupFamilies.Checked	&& addrTable.Rows[i]["famList"].ToString()!=""){
					if(addrTable.Rows[i]["numberOfReminders"].ToString()=="0") {
						message=PrefC.GetString(PrefName.RecallPostcardFamMsg);
					}
					else if(addrTable.Rows[i]["numberOfReminders"].ToString()=="1") {
						message=PrefC.GetString(PrefName.RecallPostcardFamMsg2);
					}
					else {
						message=PrefC.GetString(PrefName.RecallPostcardFamMsg3);
					}
					message=message.Replace("[FamilyList]",addrTable.Rows[i]["famList"].ToString());
				}
				//Body text, single card-------------------------------------------------------------------
				else{
					if(addrTable.Rows[i]["numberOfReminders"].ToString()=="0") {
						message=PrefC.GetString(PrefName.RecallPostcardMessage);
					}
					else if(addrTable.Rows[i]["numberOfReminders"].ToString()=="1") {
						message=PrefC.GetString(PrefName.RecallPostcardMessage2);
					}
					else {
						message=PrefC.GetString(PrefName.RecallPostcardMessage3);
					}
					message=message.Replace("[DueDate]",addrTable.Rows[i]["dateDue"].ToString());
					message=message.Replace("[NameF]",addrTable.Rows[i]["patientNameF"].ToString());
					message=message.Replace("[NameFL]", addrTable.Rows[i]["patientNameFL"].ToString());
				}
				Clinic clinicCur=Clinics.GetClinicForRecall(PIn.Long(addrTable.Rows[i]["clinicNum"].ToString()));
				message=message.Replace("[ClinicName]",clinicCur.Abbr);
				message=message.Replace("[ClinicPhone]",clinicCur.Phone);
				message=message.Replace("[PracticeName]",PrefC.GetString(PrefName.PracticeTitle));
				message=message.Replace("[PracticePhone]",PrefC.GetString(PrefName.PracticePhone));
				string officePhone=clinicCur.Phone;
				if(string.IsNullOrEmpty(officePhone)) {
					officePhone=PrefC.GetString(PrefName.PracticePhone);
				}
				message=message.Replace("[OfficePhone]",clinicCur.Phone);
				postcard.Message=message;
				postcard.Recipient=recipient;
				postcard.DesignID=PIn.Int(ProgramProperties.GetPropVal(ProgramName.Divvy,"DesignID for Recall Cards"));
				listPostcards[i]=postcard;
			}
			DivvyConnect.Practice practice=new DivvyConnect.Practice();
			clinicNum=PIn.Long(addrTable.Rows[patientsPrinted]["ClinicNum"].ToString());
			if(PrefC.HasClinicsEnabled && Clinics.GetCount() > 0 //if using clinics
				&& Clinics.GetClinic(clinicNum)!=null)//and this patient assigned to a clinic
			{
				clinic=Clinics.GetClinic(clinicNum);
				practice.Company=clinic.Description;
				practice.Address1=clinic.Address;
				practice.Address2=clinic.Address2;
				practice.City=clinic.City;
				practice.State=clinic.State;
				practice.Zip=clinic.Zip;
				phone=clinic.Phone;
			}
			else {
				practice.Company=PrefC.GetString(PrefName.PracticeTitle);
				practice.Address1=PrefC.GetString(PrefName.PracticeAddress);
				practice.Address2=PrefC.GetString(PrefName.PracticeAddress2);
				practice.City=PrefC.GetString(PrefName.PracticeCity);
				practice.State=PrefC.GetString(PrefName.PracticeST);
				practice.Zip=PrefC.GetString(PrefName.PracticeZip);
				phone=PrefC.GetString(PrefName.PracticePhone);
			}
			if(phone.Length==10 
				&& (CultureInfo.CurrentCulture.Name=="en-US" || 
				CultureInfo.CurrentCulture.Name.EndsWith("CA"))) //Canadian. en-CA or fr-CA
			{
				practice.Phone="("+phone.Substring(0,3)+")"+phone.Substring(3,3)+"-"+phone.Substring(6);
			}
			else {
				practice.Phone=phone;
			}
			DivvyConnect.PostcardServiceClient client=new DivvyConnect.PostcardServiceClient();
			DivvyConnect.PostcardReturnMessage returnMessage=new DivvyConnect.PostcardReturnMessage();
			string messages="";
			Cursor=Cursors.WaitCursor;
			try {
				returnMessage=client.SendPostcards(
				  Guid.Parse(ProgramProperties.GetPropVal(ProgramName.Divvy,"API Key")),
				  ProgramProperties.GetPropVal(ProgramName.Divvy,"Username"),
				  ProgramProperties.GetPropVal(ProgramName.Divvy,"Password"),
				  listPostcards,practice);
			}
			catch (Exception ex) {
				messages+="Exception: "+ex.Message+"\r\nData: "+ex.Data+"\r\n";
			}
			messages+="MessageCode: "+returnMessage.MessageCode.ToString();//MessageCode enum. 0=CompletedSuccessfully, 1=CompletedWithErrors, 2=Failure
			MsgBox.Show(this,"Return Messages: "+returnMessage.Message+"\r\n"+messages);
			if(returnMessage.MessageCode==DivvyConnect.MessageCode.CompletedSucessfully) {
				Cursor=Cursors.WaitCursor;
				for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
					//make commlog entries for each patient
					//Commlogs.InsertForRecall(PIn.Long(table.Rows[gridMain.SelectedIndices[i]]["PatNum"].ToString()),CommItemMode.Mail,
					//  PIn.Int(table.Rows[gridMain.SelectedIndices[i]]["numberOfReminders"].ToString()),PrefC.GetLong(PrefName.RecallStatusMailed));					
					Commlogs.InsertForRecall(1,CommItemMode.Mail,
						PIn.Int(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["numberOfReminders"].ToString()),PrefC.GetLong(PrefName.RecallStatusMailed));
				}
				for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
					Recalls.UpdateStatus(
						PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["RecallNum"].ToString()),PrefC.GetLong(PrefName.RecallStatusMailed));
				}
			}
			else if(returnMessage.MessageCode==DivvyConnect.MessageCode.CompletedWithErrors) {
				for(int i=0;i<returnMessage.PostcardMessages.Length;i++) {
					//todo: process return messages. Update commlog and change recall statuses for postcards that were sent.
				}
			}
			FillMain();
			Cursor=Cursors.Default;
		}

		private void butEmail_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.EmailSend)) {
				return;
			}
			if(gridMain.Rows.Count < 1){
        MessageBox.Show(Lan.g(this,"There are no Patients in the Recall table.  Must have at least one."));    
        return;
			}
			if(!EmailAddresses.ExistsValidEmail()) {
				MsgBox.Show(this,"You need to enter an SMTP server name in e-mail setup before you can send e-mail.");
				return;
			}
			if(PrefC.GetLong(PrefName.RecallStatusEmailed)==0){
				MsgBox.Show(this,"You need to set a status first in the Recall Setup window.");
				return;
			}
			if(gridMain.SelectedIndices.Length==0){
				ContactMethod cmeth;
				for(int i=0;i<_tableRecalls.Rows.Count;i++){
					cmeth=(ContactMethod)PIn.Long(_tableRecalls.Rows[i]["PreferRecallMethod"].ToString());
					if(cmeth!=ContactMethod.Email){
						continue;
					}
					if(_tableRecalls.Rows[i]["Email"].ToString()=="") {
						continue;
					}
					gridMain.SetSelected(i,true);
				}
				if(gridMain.SelectedIndices.Length==0){
					MsgBox.Show(this,"No patients of email type.");
					return;
				}
			}
			else{//deselect the ones that do not have email addresses specified
				int skipped=0;
				for(int i=gridMain.SelectedIndices.Length-1;i>=0;i--){
					if(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["Email"].ToString()==""){
						skipped++;
						gridMain.SetSelected(gridMain.SelectedIndices[i],false);
					}
				}
				if(gridMain.SelectedIndices.Length==0){
					MsgBox.Show(this,"None of the selected patients had email addresses entered.");
					return;
				}
				if(skipped>0){
					MessageBox.Show(Lan.g(this,"Selected patients skipped due to missing email addresses: ")+skipped.ToString());
				}
			}
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Send email to all of the selected patients?")) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			List<long> recallNums=new List<long>();
      for(int i=0;i<gridMain.SelectedIndices.Length;i++){
        recallNums.Add(PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["RecallNum"].ToString()));
      }
			RecallListSort sortBy=(RecallListSort)comboSort.SelectedIndex;
			addrTable=Recalls.GetAddrTable(recallNums,checkGroupFamilies.Checked,sortBy);
			EmailMessage message;
			string str="";
			string[] recallNumArray;
			string[] patNumArray;
			EmailAddress emailAddress;
			int sentEmailCount=0;
			for(int i=0;i<addrTable.Rows.Count;i++){
				message=new EmailMessage();
				message.PatNum=PIn.Long(addrTable.Rows[i]["emailPatNum"].ToString());
				message.ToAddress=PIn.String(addrTable.Rows[i]["email"].ToString());//might be guarantor email
				if(comboEmailFrom.SelectedIndex==0) { //clinic/practice default
					emailAddress=EmailAddresses.GetByClinic(PIn.Long(addrTable.Rows[i]["ClinicNum"].ToString()));
				}
				else { //me or static email address, email address for 'me' is the first one in _listEmailAddresses
					emailAddress=_listEmailAddresses[comboEmailFrom.SelectedIndex-1];//-1 to account for predefined "Clinic/Practice" item in combobox
				}
				message.FromAddress=emailAddress.GetFrom();
				if(addrTable.Rows[i]["numberOfReminders"].ToString()=="0") {
					message.Subject=PrefC.GetString(PrefName.RecallEmailSubject);
				}
				else if(addrTable.Rows[i]["numberOfReminders"].ToString()=="1") {
					message.Subject=PrefC.GetString(PrefName.RecallEmailSubject2);
				}
				else {
					message.Subject=PrefC.GetString(PrefName.RecallEmailSubject3);
				}
				//family
				if(checkGroupFamilies.Checked && addrTable.Rows[i]["famList"].ToString()!="") {
					if(addrTable.Rows[i]["numberOfReminders"].ToString()=="0") {
						str=PrefC.GetString(PrefName.RecallEmailFamMsg);
					}
					else if(addrTable.Rows[i]["numberOfReminders"].ToString()=="1") {
						str=PrefC.GetString(PrefName.RecallEmailFamMsg2);
					}
					else {
						str=PrefC.GetString(PrefName.RecallEmailFamMsg3);
					}
					str=str.Replace("[FamilyList]",addrTable.Rows[i]["famList"].ToString());
				}
				//single
				else {
					if(addrTable.Rows[i]["numberOfReminders"].ToString()=="0") {
						str=PrefC.GetString(PrefName.RecallEmailMessage);
					}
					else if(addrTable.Rows[i]["numberOfReminders"].ToString()=="1") {
						str=PrefC.GetString(PrefName.RecallEmailMessage2);
					}
					else {
						str=PrefC.GetString(PrefName.RecallEmailMessage3);
					}
					str=str.Replace("[DueDate]",PIn.Date(addrTable.Rows[i]["dateDue"].ToString()).ToShortDateString());
					str=str.Replace("[NameF]",addrTable.Rows[i]["patientNameF"].ToString());
					str=str.Replace("[NameFL]",addrTable.Rows[i]["patientNameFL"].ToString());
				}
				Clinic clinicCur=Clinics.GetClinicForRecall(PIn.Long(addrTable.Rows[i]["clinicNum"].ToString()));
				string officePhone="";
				if(clinicCur==null) {
					str=str.Replace("[ClinicName]",PrefC.GetString(PrefName.PracticeTitle));
					str=str.Replace("[ClinicPhone]",PrefC.GetString(PrefName.PracticePhone));
				}
				else {
					str=str.Replace("[ClinicName]",clinicCur.Abbr);
					str=str.Replace("[ClinicPhone]",clinicCur.Phone);
					officePhone=clinicCur.Phone;
				}
				str=str.Replace("[PracticeName]",PrefC.GetString(PrefName.PracticeTitle));
				str=str.Replace("[PracticePhone]",PrefC.GetString(PrefName.PracticePhone));
				if(string.IsNullOrEmpty(officePhone)) {
					officePhone=PrefC.GetString(PrefName.PracticePhone);
				}
				str=str.Replace("[OfficePhone]",officePhone);
				message.BodyText=str;
				try{
					EmailMessages.SendEmailUnsecure(message,emailAddress);
					sentEmailCount++;
				}
				catch(Exception ex){
					Cursor=Cursors.Default;
					str=ex.Message+"\r\n";
					if(ex.GetType()==typeof(System.ArgumentException)){
						str+="Go to Setup, Recall.  The subject for an email may not be multiple lines.\r\n";
					}
					MessageBox.Show(str+"Patient:"+addrTable.Rows[i]["patientNameFL"].ToString());
					break;
				}
				message.MsgDateTime=DateTime.Now;
				message.SentOrReceived=EmailSentOrReceived.Sent;
				EmailMessages.Insert(message);
				recallNumArray=addrTable.Rows[i]["recallNums"].ToString().Split(',');
				patNumArray=addrTable.Rows[i]["patNums"].ToString().Split(',');
				for(int r=0;r<recallNumArray.Length;r++){
					Commlogs.InsertForRecall(PIn.Long(patNumArray[r]),CommItemMode.Email,PIn.Int(addrTable.Rows[i]["numberOfReminders"].ToString()),
						PrefC.GetLong(PrefName.RecallStatusEmailed));
					Recalls.UpdateStatus(PIn.Long(recallNumArray[r]),PrefC.GetLong(PrefName.RecallStatusEmailed));
				}
			}
			FillMain();
			if(sentEmailCount>0) {
				SecurityLogs.MakeLogEntry(Permissions.EmailSend,0,"Recall Emails Sent: "+sentEmailCount);
			}
			Cursor=Cursors.Default;
		}

		///<summary>raised for each page to be printed.</summary>
		private void pdLabels_PrintPage(object sender, PrintPageEventArgs ev){
			int totalPages=(int)Math.Ceiling((double)addrTable.Rows.Count/30);
			Graphics g=ev.Graphics;
			float yPos=63;//75;
			float xPos=50;
			string text="";
			while(yPos<1000 && patientsPrinted<addrTable.Rows.Count){
				text="";
				if(checkGroupFamilies.Checked && addrTable.Rows[patientsPrinted]["famList"].ToString()!=""){//print family label
					text=addrTable.Rows[patientsPrinted]["guarLName"].ToString()+" "+Lan.g(this,"Household")+"\r\n";
				}
				else {//print single label
					text=addrTable.Rows[patientsPrinted]["patientNameFL"].ToString()+"\r\n";
				}
				text+=addrTable.Rows[patientsPrinted]["address"].ToString()+"\r\n";
				text+=addrTable.Rows[patientsPrinted]["City"].ToString()+", "
					+addrTable.Rows[patientsPrinted]["State"].ToString()+" "
					+addrTable.Rows[patientsPrinted]["Zip"].ToString()+"\r\n";
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
			if(pagesPrinted==totalPages){
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
			int totalPages=(int)Math.Ceiling((double)addrTable.Rows.Count/(double)PrefC.GetLong(PrefName.RecallPostcardsPerSheet));
			Graphics g=ev.Graphics;
			int yAdj=(int)(PrefC.GetDouble(PrefName.RecallAdjustDown)*100);
			int xAdj=(int)(PrefC.GetDouble(PrefName.RecallAdjustRight)*100);
			float yPos=0+yAdj;//these refer to the upper left origin of each postcard
			float xPos=0+xAdj;
			long clinicNum;
			Clinic clinic;
			string str;
			while(yPos<ev.PageBounds.Height-100 && patientsPrinted<addrTable.Rows.Count){
				//Return Address--------------------------------------------------------------------------
				clinicNum=PIn.Long(addrTable.Rows[patientsPrinted]["ClinicNum"].ToString());
				if(PrefC.GetBool(PrefName.RecallCardsShowReturnAdd)){
					if(PrefC.HasClinicsEnabled && Clinics.GetCount() > 0 //if using clinics
						&& Clinics.GetClinic(clinicNum)!=null)//and this patient assigned to a clinic
					{
						clinic=Clinics.GetClinic(clinicNum);
						str=clinic.Description+"\r\n";
						g.DrawString(str,new Font(FontFamily.GenericSansSerif,9,FontStyle.Bold),Brushes.Black,xPos+45,yPos+60);
						str=clinic.Address+"\r\n";
						if(clinic.Address2!="") {
							str+=clinic.Address2+"\r\n";
						}
						str+=clinic.City+",  "+clinic.State+"  "+clinic.Zip+"\r\n";
						string phone=clinic.Phone;
						if(CultureInfo.CurrentCulture.Name=="en-US" && phone.Length==10) {
							str+="("+phone.Substring(0,3)+")"+phone.Substring(3,3)+"-"+phone.Substring(6);
						}
						else {//any other phone format
							str+=phone;
						}
					}
					else {
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
					g.DrawString(str,new Font(FontFamily.GenericSansSerif,8),Brushes.Black,xPos+45,yPos+75);
				}
				//Body text, family card ------------------------------------------------------------------
				if(checkGroupFamilies.Checked	&& addrTable.Rows[patientsPrinted]["famList"].ToString()!=""){
					if(addrTable.Rows[patientsPrinted]["numberOfReminders"].ToString()=="0") {
						str=PrefC.GetString(PrefName.RecallPostcardFamMsg);
					}
					else if(addrTable.Rows[patientsPrinted]["numberOfReminders"].ToString()=="1") {
						str=PrefC.GetString(PrefName.RecallPostcardFamMsg2);
					}
					else {
						str=PrefC.GetString(PrefName.RecallPostcardFamMsg3);
					}
					str=str.Replace("[FamilyList]",addrTable.Rows[patientsPrinted]["famList"].ToString());
				}
				//Body text, single card-------------------------------------------------------------------
				else{
					if(addrTable.Rows[patientsPrinted]["numberOfReminders"].ToString()=="0") {
						str=PrefC.GetString(PrefName.RecallPostcardMessage);
					}
					else if(addrTable.Rows[patientsPrinted]["numberOfReminders"].ToString()=="1") {
						str=PrefC.GetString(PrefName.RecallPostcardMessage2);
					}
					else {
						str=PrefC.GetString(PrefName.RecallPostcardMessage3);
					}
					str=str.Replace("[DueDate]",addrTable.Rows[patientsPrinted]["dateDue"].ToString());
					str=str.Replace("[NameF]",addrTable.Rows[patientsPrinted]["patientNameF"].ToString());
					str=str.Replace("[NameFL]",addrTable.Rows[patientsPrinted]["patientNameFL"].ToString());
				}
				if(PrefC.HasClinicsEnabled && Clinics.GetClinic(clinicNum)!=null) {//has clinics and patient is assigned to a clinic.  
					Clinic clinicCur=Clinics.GetClinic(clinicNum); 
					str=str.Replace("[ClinicName]",clinicCur.Abbr);
					str=str.Replace("[ClinicPhone]",clinicCur.Phone);
					string officePhone=clinicCur.Phone;
					if(string.IsNullOrEmpty(officePhone)) {
						officePhone=PrefC.GetString(PrefName.PracticePhone);
					}
					str=str.Replace("[OfficePhone]",officePhone);
				}
				else {//use practice information for default. 
					str=str.Replace("[ClinicName]",PrefC.GetString(PrefName.PracticeTitle));
					str=str.Replace("[ClinicPhone]",PrefC.GetString(PrefName.PracticePhone));
					str=str.Replace("[OfficePhone]",PrefC.GetString(PrefName.PracticePhone));
				}
				str=str.Replace("[PracticeName]",PrefC.GetString(PrefName.PracticeTitle));
				str=str.Replace("[PracticePhone]",PrefC.GetString(PrefName.PracticePhone));
				g.DrawString(str,new Font(FontFamily.GenericSansSerif,10),Brushes.Black,new RectangleF(xPos+45,yPos+180,250,190));
				//Patient's Address-----------------------------------------------------------------------
				if(checkGroupFamilies.Checked
					&& addrTable.Rows[patientsPrinted]["famList"].ToString()!="")//print family card
				{
					str=addrTable.Rows[patientsPrinted]["guarLName"].ToString()+" "+Lan.g(this,"Household")+"\r\n";
				}
				else{//print single card
					str=addrTable.Rows[patientsPrinted]["patientNameFL"].ToString()+"\r\n";
				}
				str+=addrTable.Rows[patientsPrinted]["address"].ToString()+"\r\n";
				str+=addrTable.Rows[patientsPrinted]["City"].ToString()+", "
					+addrTable.Rows[patientsPrinted]["State"].ToString()+" "
					+addrTable.Rows[patientsPrinted]["Zip"].ToString()+"\r\n";
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
			gridMain.SetSelected(false);
			FillMain();
		}

		private void butSetStatus_Click(object sender, System.EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a patient first.");
				return;
			}
			//bool makeCommEntries=MsgBox.Show(this,MsgBoxButtons.OKCancel,"Add Commlog (reminder) entries for each patient?");
			long newStatus=0;
			if(comboStatus.SelectedIndex>0) {
				newStatus=_listRecallUnschedStatusDefs[comboStatus.SelectedIndex-1].DefNum;
			}
			for(int i=0;i<gridMain.SelectedIndices.Length;i++){
				Recalls.UpdateStatus(PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["RecallNum"].ToString()),newStatus);
			}
			//show the first one, and then make all the others very similar
			Commlog CommlogCur=new Commlog();
			CommlogCur.PatNum=PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[0]]["PatNum"].ToString());
			CommlogCur.CommDateTime=DateTime.Now;
			CommlogCur.SentOrReceived=CommSentOrReceived.Sent;
			CommlogCur.Mode_=CommItemMode.Phone;//user can change this, of course.
			CommlogCur.CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.RECALL);
			CommlogCur.UserNum=Security.CurUser.UserNum;
			CommlogCur.Note=Lan.g(this,"Recall reminder.");
			if(comboStatus.SelectedIndex>0) {
				CommlogCur.Note+="  "+_listRecallUnschedStatusDefs[comboStatus.SelectedIndex-1].ItemName;
			}
			else{
				CommlogCur.Note+="  "+Lan.g(this,"Status None");
			}
			FormCommItem FormCI=new FormCommItem();
			if(FormCI.ShowDialog(
				new CommItemModel() { CommlogCur=CommlogCur },
				new CommItemController(FormCI) { IsNew=true })!=DialogResult.OK) 
			{
				FillMain();
				return;
			}
			for(int i=1;i<gridMain.SelectedIndices.Length;i++) {
				CommlogCur.PatNum=PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["PatNum"].ToString());
				Commlogs.Insert(CommlogCur);
			}
			FillMain();
		}

		private void menuItemSeeFamily_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.FamilyModule)) {
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a patient first.");
				return;
			}
			long patNum=PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[0]]["PatNum"].ToString());
			GotoModule.GotoFamily(patNum);
		}

		private void menuItemSeeAccount_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AccountModule)) {
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a patient first.");
				return;
			}
			//WindowState=FormWindowState.Minimized;
			long patNum=PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[0]]["PatNum"].ToString());
			GotoModule.GotoAccount(patNum);
		}

		private void butGotoFamily_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.FamilyModule)) {
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a patient first.");
				return;
			}
			WindowState=FormWindowState.Minimized;
			long patNum=PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[0]]["PatNum"].ToString());
			GotoModule.GotoFamily(patNum);
		}

		private void butGotoAccount_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AccountModule)) {
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a patient first.");
				return;
			}
			WindowState=FormWindowState.Minimized;
			long patNum=PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[0]]["PatNum"].ToString());
			GotoModule.GotoAccount(patNum);
		}

		private void butCommlog_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a patient first.");
				return;
			}
			//show the first one, and then make all the others very similar
			Commlog CommlogCur=new Commlog();
			CommlogCur.PatNum=PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[0]]["PatNum"].ToString());
			CommlogCur.CommDateTime=DateTime.Now;
			CommlogCur.SentOrReceived=CommSentOrReceived.Sent;
			CommlogCur.Mode_=CommItemMode.Phone;//user can change this, of course.
			CommlogCur.CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.RECALL);
			CommlogCur.UserNum=Security.CurUser.UserNum;
			FormCommItem FormCI=new FormCommItem();
			if(FormCI.ShowDialog(
				new CommItemModel() { CommlogCur=CommlogCur },
				new CommItemController(FormCI) { IsNew=true })!=DialogResult.OK) 
			{
				return;
			}
			for(int i=1;i<gridMain.SelectedIndices.Length;i++) {
				CommlogCur.PatNum=PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[i]]["PatNum"].ToString());
				Commlogs.Insert(CommlogCur);
			}
			FillMain();
		}
		
		private void butPrint_Click(object sender,EventArgs e) {
			pagesPrinted=0;
			pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			//pd.OriginAtMargins=true;
			pd.DefaultPageSettings.Landscape=true;
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
					if(PrinterL.SetPrinter(pd,PrintSituation.Default,0,"Recall list printed")) {
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
				text=Lan.g(this,"Recall List");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				text=textDateStart.Text+" "+Lan.g(this,"to")+" "+textDateEnd.Text;
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

		public override void OnProcessSignals(List<Signalod> listSignals) {
			if(listSignals.Any(x => x.IType==InvalidType.WebSchedRecallReminders)) {
				FillMain();
			}
		}

		private void tabControl_SelectedIndexChanged(object sender,EventArgs e) {
			if(tabControl.SelectedTab==tabPageRecentlyContacted && gridRecentlyContacted.Columns.Count==0) {//The grid has not been initialized yet.
				datePickerRecent.SetDateTimeFrom(DateTime.Today.AddMonths(-1));
				datePickerRecent.SetDateTimeTo(DateTime.Today);
				FillGridRecent();
			}
		}

		private void FillGridRecent() {
			Cursor=Cursors.WaitCursor;
			List<Recalls.RecallRecent> listRecent=Recalls.GetRecentRecalls(datePickerRecent.GetDateTimeFrom(),datePickerRecent.GetDateTimeTo(),
				comboClinicRecent.ListSelectedClinicNums);
			gridRecentlyContacted.BeginUpdate();
			gridRecentlyContacted.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Date Time Sent"),140,GridSortingStrategy.DateParse);
			gridRecentlyContacted.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Patient"),200);
			gridRecentlyContacted.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Reminder Type"),180);
			gridRecentlyContacted.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Age"),50,GridSortingStrategy.AmountParse);
			gridRecentlyContacted.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Due Date"),100,GridSortingStrategy.DateParse);
			gridRecentlyContacted.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Recall Type"),130);
			gridRecentlyContacted.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Recall Status"),130);
			gridRecentlyContacted.Columns.Add(col);
			gridRecentlyContacted.Rows.Clear();
			foreach(Recalls.RecallRecent recent in listRecent) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(recent.DateSent.ToString());
				row.Cells.Add(recent.PatientName);
				row.Cells.Add(recent.ReminderType);
				row.Cells.Add(recent.Age.ToString());
				if(recent.DueDate.Year < 1880) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(recent.DueDate.ToShortDateString());
				}
				row.Cells.Add(recent.RecallType);
				row.Cells.Add(recent.RecallStatus);
				row.Tag=recent;
				gridRecentlyContacted.Rows.Add(row);
			}
			gridRecentlyContacted.EndUpdate();
			Cursor=Cursors.Default;
		}

		private void butRefreshRecent_Click(object sender,EventArgs e) {
			FillGridRecent();
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormRecallList_FormClosing(object sender,FormClosingEventArgs e) {
			if(gridMain.SelectedIndices.Length==1){
				SelectedPatNum=PIn.Long(_tableRecalls.Rows[gridMain.SelectedIndices[0]]["PatNum"].ToString());
			}
		}

	}
	
}
