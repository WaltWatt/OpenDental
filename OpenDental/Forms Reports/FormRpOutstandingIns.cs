using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using OpenDentBusiness;
using System.Collections;
using OpenDental.UI;
using CodeBase;

namespace OpenDental {
	public partial class FormRpOutstandingIns:ODForm {
		#region Designer Variables
		private ODGrid gridMain;
		private CheckBox checkPreauth;
		private Label labelProv;
		private UI.Button butCancel;
		private UI.Button butPrint;
		private ComboBoxMulti comboBoxMultiProv;
		private Label label2;
		private TextBox textBox1;
		private UI.Button butExport;
		private UI.Button butRefresh;
		private ComboBoxMulti comboBoxMultiClinics;
		private Label labelClinic;
		private CheckBox checkIgnoreCustom;
		public TextBox textCarrier;
		private Label labelCarrier;
		private GroupBox groupBox2;
		private UI.Button buttonUpdateCustomTrack;
		private Label labelClaimCount;
		private Label label4;
		private ComboBox comboLastClaimTrack;
		private GroupBox groupBoxUpdateCustomTracking;
		private Label labelCustomTracking;
		private Label label3;
		private ComboBox comboErrorDef;
		private Label labelForUser;
		private UI.Button butPickUser;
		private UI.Button butAssignUser;
		private ComboBoxMulti comboUserAssigned;
		private UI.Button butMine;
		private ContextMenu rightClickMenu=new ContextMenu();
		private GroupBox groupBox1;
		private TextBox textGroupName;
		private TextBox textGroupNumber;
		private TextBox textCarrierPhone;
		private TextBox textCarrierName;
		private Label label8;
		private Label label7;
		private Label label6;
		private Label label5;
		private GroupBox groupBox3;
		private TextBox textSubscriberID;
		private TextBox textSubscriberDOB;
		private TextBox textSubscriberName;
		private Label label10;
		private Label label11;
		private Label label12;
		private TextBox textPatDOB;
		private Label label9;
		private Label label1;
		private ComboBox comboDateFilterBy;
		#endregion
		#region Private Variables
		private bool isPreauth;
		private bool headingPrinted;
		private int pagesPrinted;
		private int headingPrintH;
		private decimal total;
		///<summary>List of non-hidden users with ClaimSentEdit permission.</summary>
		private List<Userod> _listClaimSentEditUsers=new List<Userod>();
		private List<ClaimTracking> _listNewClaimTrackings=new List<ClaimTracking>();
		private TabControl tabControlDate;
		private TabPage tabDaysOld;
		private ValidNum textDaysOldMax;
		private Label labelDaysOldMax;
		private Label labelDateMinMaxAdvice;
		private Label labelDaysOldMin;
		private TabPage tabDateRange;
		private ValidNum textDaysOldMin;
		private ODDateRangePickerVertical dateRangePickerVertical;
		private List<ClaimTracking> _listOldClaimTrackings=new List<ClaimTracking>();
		#endregion
		#region Properties
		private List<long> _listSelectedProvNums {
			get {
				List<long> listProvNums = new List<long>();
				if(!comboBoxMultiProv.ListSelectedItems.Select(x => ((ODBoxItem<Provider>)x).Tag).Contains(null)) { //"All" is selected.
					listProvNums = comboBoxMultiProv.ListSelectedItems.Select(x => ((ODBoxItem<Provider>)x).Tag.ProvNum).ToList();
				}
				return listProvNums;
			}
		}

		private List<long> _listSelectedClinicNums {
			get {
				List<long> listClinicNums = new List<long>();
				if(!comboBoxMultiClinics.ListSelectedItems.Select(x => ((ODBoxItem<Clinic>)x).Tag).Contains(null)) { //"All" is selected.
					listClinicNums = comboBoxMultiClinics.ListSelectedItems.Select(x => ((ODBoxItem<Clinic>)x).Tag.ClinicNum).ToList();
				}
				return listClinicNums;
			}
		}

		private List<long> _listSelectedUserNums {
			get {
				List<long> listUserNums = new List<long>();
				if(!comboUserAssigned.ListSelectedItems.Select(x => ((ODBoxItem<Userod>)x).Tag).Contains(null)) { //"All" is selected.
					listUserNums = comboUserAssigned.ListSelectedItems.Select(x => ((ODBoxItem<Userod>)x).Tag.UserNum).ToList();
				}
				return listUserNums;
			}
		}
		#endregion

		public FormRpOutstandingIns() {
			InitializeComponent();
			gridMain.ContextMenu=rightClickMenu;
			Lan.F(this);
		}

		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpOutstandingIns));
			this.checkPreauth = new System.Windows.Forms.CheckBox();
			this.labelProv = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.butRefresh = new OpenDental.UI.Button();
			this.butExport = new OpenDental.UI.Button();
			this.comboBoxMultiProv = new OpenDental.UI.ComboBoxMulti();
			this.butPrint = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.comboBoxMultiClinics = new OpenDental.UI.ComboBoxMulti();
			this.labelClinic = new System.Windows.Forms.Label();
			this.checkIgnoreCustom = new System.Windows.Forms.CheckBox();
			this.textCarrier = new System.Windows.Forms.TextBox();
			this.labelCarrier = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.tabControlDate = new System.Windows.Forms.TabControl();
			this.tabDaysOld = new System.Windows.Forms.TabPage();
			this.textDaysOldMax = new OpenDental.ValidNum();
			this.labelDaysOldMax = new System.Windows.Forms.Label();
			this.textDaysOldMin = new OpenDental.ValidNum();
			this.labelDateMinMaxAdvice = new System.Windows.Forms.Label();
			this.labelDaysOldMin = new System.Windows.Forms.Label();
			this.tabDateRange = new System.Windows.Forms.TabPage();
			this.dateRangePickerVertical = new OpenDental.UI.ODDateRangePickerVertical();
			this.comboDateFilterBy = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.comboErrorDef = new System.Windows.Forms.ComboBox();
			this.butMine = new OpenDental.UI.Button();
			this.comboUserAssigned = new OpenDental.UI.ComboBoxMulti();
			this.butPickUser = new OpenDental.UI.Button();
			this.labelForUser = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.comboLastClaimTrack = new System.Windows.Forms.ComboBox();
			this.buttonUpdateCustomTrack = new OpenDental.UI.Button();
			this.labelClaimCount = new System.Windows.Forms.Label();
			this.groupBoxUpdateCustomTracking = new System.Windows.Forms.GroupBox();
			this.labelCustomTracking = new System.Windows.Forms.Label();
			this.butAssignUser = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textGroupName = new System.Windows.Forms.TextBox();
			this.textGroupNumber = new System.Windows.Forms.TextBox();
			this.textCarrierPhone = new System.Windows.Forms.TextBox();
			this.textCarrierName = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.textPatDOB = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.textSubscriberID = new System.Windows.Forms.TextBox();
			this.textSubscriberDOB = new System.Windows.Forms.TextBox();
			this.textSubscriberName = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.groupBox2.SuspendLayout();
			this.tabControlDate.SuspendLayout();
			this.tabDaysOld.SuspendLayout();
			this.tabDateRange.SuspendLayout();
			this.groupBoxUpdateCustomTracking.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkPreauth
			// 
			this.checkPreauth.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPreauth.Checked = true;
			this.checkPreauth.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkPreauth.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPreauth.Location = new System.Drawing.Point(301, 8);
			this.checkPreauth.Name = "checkPreauth";
			this.checkPreauth.Size = new System.Drawing.Size(157, 17);
			this.checkPreauth.TabIndex = 51;
			this.checkPreauth.Text = "Include Preauths";
			this.checkPreauth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPreauth.CheckedChanged += new System.EventHandler(this.checkPreauth_CheckedChanged);
			// 
			// labelProv
			// 
			this.labelProv.Location = new System.Drawing.Point(456, 17);
			this.labelProv.Name = "labelProv";
			this.labelProv.Size = new System.Drawing.Size(86, 16);
			this.labelProv.TabIndex = 48;
			this.labelProv.Text = "Treat Provs";
			this.labelProv.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(1069, 668);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(57, 18);
			this.label2.TabIndex = 46;
			this.label2.Text = "Total";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(1132, 667);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(75, 20);
			this.textBox1.TabIndex = 56;
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(1125, 65);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(82, 24);
			this.butRefresh.TabIndex = 58;
			this.butRefresh.Text = "&Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// butExport
			// 
			this.butExport.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butExport.Autosize = true;
			this.butExport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butExport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butExport.CornerRadius = 4F;
			this.butExport.Image = global::OpenDental.Properties.Resources.butExport;
			this.butExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butExport.Location = new System.Drawing.Point(12, 663);
			this.butExport.Name = "butExport";
			this.butExport.Size = new System.Drawing.Size(79, 24);
			this.butExport.TabIndex = 57;
			this.butExport.Text = "&Export";
			this.butExport.Click += new System.EventHandler(this.butExport_Click);
			// 
			// comboBoxMultiProv
			// 
			this.comboBoxMultiProv.ArraySelectedIndices = new int[0];
			this.comboBoxMultiProv.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiProv.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiProv.Items")));
			this.comboBoxMultiProv.Location = new System.Drawing.Point(543, 16);
			this.comboBoxMultiProv.Name = "comboBoxMultiProv";
			this.comboBoxMultiProv.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiProv.SelectedIndices")));
			this.comboBoxMultiProv.Size = new System.Drawing.Size(160, 21);
			this.comboBoxMultiProv.TabIndex = 53;
			this.comboBoxMultiProv.Leave += new System.EventHandler(this.comboBoxMultiProv_Leave);
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
			this.butPrint.Location = new System.Drawing.Point(12, 693);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(79, 24);
			this.butPrint.TabIndex = 52;
			this.butPrint.Text = "&Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
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
			this.butCancel.Location = new System.Drawing.Point(1132, 693);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 45;
			this.butCancel.Text = "&Close";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// gridMain
			// 
			this.gridMain.AllowSortingByColumn = true;
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = true;
			this.gridMain.Location = new System.Drawing.Point(12, 95);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(1206, 519);
			this.gridMain.TabIndex = 1;
			this.gridMain.Title = "Claims";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableOustandingInsClaims";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// comboBoxMultiClinics
			// 
			this.comboBoxMultiClinics.ArraySelectedIndices = new int[0];
			this.comboBoxMultiClinics.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiClinics.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiClinics.Items")));
			this.comboBoxMultiClinics.Location = new System.Drawing.Point(543, 38);
			this.comboBoxMultiClinics.Name = "comboBoxMultiClinics";
			this.comboBoxMultiClinics.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiClinics.SelectedIndices")));
			this.comboBoxMultiClinics.Size = new System.Drawing.Size(160, 21);
			this.comboBoxMultiClinics.TabIndex = 59;
			this.comboBoxMultiClinics.Visible = false;
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(456, 38);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(86, 16);
			this.labelClinic.TabIndex = 60;
			this.labelClinic.Text = "Clinics";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			this.labelClinic.Visible = false;
			// 
			// checkIgnoreCustom
			// 
			this.checkIgnoreCustom.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIgnoreCustom.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIgnoreCustom.Location = new System.Drawing.Point(301, 27);
			this.checkIgnoreCustom.Name = "checkIgnoreCustom";
			this.checkIgnoreCustom.Size = new System.Drawing.Size(157, 17);
			this.checkIgnoreCustom.TabIndex = 61;
			this.checkIgnoreCustom.Text = "Ignore Custom Tracking";
			this.checkIgnoreCustom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIgnoreCustom.Click += new System.EventHandler(this.checkIgnoreCustom_Click);
			// 
			// textCarrier
			// 
			this.textCarrier.Location = new System.Drawing.Point(910, 37);
			this.textCarrier.Name = "textCarrier";
			this.textCarrier.Size = new System.Drawing.Size(141, 20);
			this.textCarrier.TabIndex = 105;
			// 
			// labelCarrier
			// 
			this.labelCarrier.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelCarrier.Location = new System.Drawing.Point(784, 39);
			this.labelCarrier.Name = "labelCarrier";
			this.labelCarrier.Size = new System.Drawing.Size(118, 16);
			this.labelCarrier.TabIndex = 106;
			this.labelCarrier.Text = "Carrier";
			this.labelCarrier.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.tabControlDate);
			this.groupBox2.Controls.Add(this.comboDateFilterBy);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.checkPreauth);
			this.groupBox2.Controls.Add(this.checkIgnoreCustom);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.comboErrorDef);
			this.groupBox2.Controls.Add(this.butMine);
			this.groupBox2.Controls.Add(this.comboUserAssigned);
			this.groupBox2.Controls.Add(this.butPickUser);
			this.groupBox2.Controls.Add(this.labelForUser);
			this.groupBox2.Controls.Add(this.textCarrier);
			this.groupBox2.Controls.Add(this.labelCarrier);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.comboLastClaimTrack);
			this.groupBox2.Controls.Add(this.labelProv);
			this.groupBox2.Controls.Add(this.comboBoxMultiProv);
			this.groupBox2.Controls.Add(this.labelClinic);
			this.groupBox2.Controls.Add(this.comboBoxMultiClinics);
			this.groupBox2.Location = new System.Drawing.Point(12, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(1056, 91);
			this.groupBox2.TabIndex = 248;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Filters";
			// 
			// tabControlDate
			// 
			this.tabControlDate.Controls.Add(this.tabDaysOld);
			this.tabControlDate.Controls.Add(this.tabDateRange);
			this.tabControlDate.Location = new System.Drawing.Point(6, 12);
			this.tabControlDate.Name = "tabControlDate";
			this.tabControlDate.SelectedIndex = 0;
			this.tabControlDate.Size = new System.Drawing.Size(246, 76);
			this.tabControlDate.TabIndex = 264;
			this.tabControlDate.SelectedIndexChanged += new System.EventHandler(this.tabControlDate_SelectedIndexChanged);
			// 
			// tabDaysOld
			// 
			this.tabDaysOld.Controls.Add(this.textDaysOldMax);
			this.tabDaysOld.Controls.Add(this.labelDaysOldMax);
			this.tabDaysOld.Controls.Add(this.textDaysOldMin);
			this.tabDaysOld.Controls.Add(this.labelDateMinMaxAdvice);
			this.tabDaysOld.Controls.Add(this.labelDaysOldMin);
			this.tabDaysOld.Location = new System.Drawing.Point(4, 22);
			this.tabDaysOld.Name = "tabDaysOld";
			this.tabDaysOld.Padding = new System.Windows.Forms.Padding(3);
			this.tabDaysOld.Size = new System.Drawing.Size(238, 50);
			this.tabDaysOld.TabIndex = 0;
			this.tabDaysOld.Text = "Days Old";
			this.tabDaysOld.UseVisualStyleBackColor = true;
			// 
			// textDaysOldMax
			// 
			this.textDaysOldMax.Location = new System.Drawing.Point(90, 27);
			this.textDaysOldMax.MaxVal = 50000;
			this.textDaysOldMax.MinVal = 0;
			this.textDaysOldMax.Name = "textDaysOldMax";
			this.textDaysOldMax.Size = new System.Drawing.Size(60, 20);
			this.textDaysOldMax.TabIndex = 57;
			this.textDaysOldMax.Text = "0";
			this.textDaysOldMax.TextChanged += new System.EventHandler(this.textDaysOldMax_TextChanged);
			// 
			// labelDaysOldMax
			// 
			this.labelDaysOldMax.Location = new System.Drawing.Point(2, 25);
			this.labelDaysOldMax.Name = "labelDaysOldMax";
			this.labelDaysOldMax.Size = new System.Drawing.Size(85, 18);
			this.labelDaysOldMax.TabIndex = 55;
			this.labelDaysOldMax.Text = "(max)";
			this.labelDaysOldMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDaysOldMin
			// 
			this.textDaysOldMin.Location = new System.Drawing.Point(90, 4);
			this.textDaysOldMin.MaxVal = 50000;
			this.textDaysOldMin.MinVal = 0;
			this.textDaysOldMin.Name = "textDaysOldMin";
			this.textDaysOldMin.Size = new System.Drawing.Size(60, 20);
			this.textDaysOldMin.TabIndex = 58;
			this.textDaysOldMin.Text = "30";
			this.textDaysOldMin.TextChanged += new System.EventHandler(this.textDaysOldMin_TextChanged);
			// 
			// labelDateMinMaxAdvice
			// 
			this.labelDateMinMaxAdvice.Location = new System.Drawing.Point(168, 2);
			this.labelDateMinMaxAdvice.Name = "labelDateMinMaxAdvice";
			this.labelDateMinMaxAdvice.Size = new System.Drawing.Size(70, 48);
			this.labelDateMinMaxAdvice.TabIndex = 59;
			this.labelDateMinMaxAdvice.Text = "(leave both blank to show all)";
			this.labelDateMinMaxAdvice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelDaysOldMin
			// 
			this.labelDaysOldMin.Location = new System.Drawing.Point(2, 4);
			this.labelDaysOldMin.Name = "labelDaysOldMin";
			this.labelDaysOldMin.Size = new System.Drawing.Size(85, 18);
			this.labelDaysOldMin.TabIndex = 56;
			this.labelDaysOldMin.Text = "Days Old (min)";
			this.labelDaysOldMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabDateRange
			// 
			this.tabDateRange.Controls.Add(this.dateRangePickerVertical);
			this.tabDateRange.Location = new System.Drawing.Point(4, 22);
			this.tabDateRange.Name = "tabDateRange";
			this.tabDateRange.Padding = new System.Windows.Forms.Padding(3);
			this.tabDateRange.Size = new System.Drawing.Size(238, 50);
			this.tabDateRange.TabIndex = 1;
			this.tabDateRange.Text = "Date Range";
			this.tabDateRange.UseVisualStyleBackColor = true;
			// 
			// dateRangePickerVertical
			// 
			this.dateRangePickerVertical.BackColor = System.Drawing.Color.Transparent;
			this.dateRangePickerVertical.DefaultDateTimeFrom = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
			this.dateRangePickerVertical.DefaultDateTimeTo = new System.DateTime(2018, 4, 9, 0, 0, 0, 0);
			this.dateRangePickerVertical.Location = new System.Drawing.Point(0, 4);
			this.dateRangePickerVertical.MaximumSize = new System.Drawing.Size(453, 185);
			this.dateRangePickerVertical.MinimumSize = new System.Drawing.Size(383, 186);
			this.dateRangePickerVertical.Name = "dateRangePickerVertical";
			this.dateRangePickerVertical.Size = new System.Drawing.Size(383, 186);
			this.dateRangePickerVertical.TabIndex = 0;
			this.dateRangePickerVertical.CalendarClosed += new OpenDental.UI.CalendarClosedHandler(this.dateRangePickerVertical_CalendarClosed);
			this.dateRangePickerVertical.CalendarSelectionChanged += new OpenDental.UI.CalendarSelectionHandler(this.dateRangePickerVertical_CalendarSelectionChanged);
			// 
			// comboDateFilterBy
			// 
			this.comboDateFilterBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDateFilterBy.FormattingEnabled = true;
			this.comboDateFilterBy.Location = new System.Drawing.Point(290, 63);
			this.comboDateFilterBy.Name = "comboDateFilterBy";
			this.comboDateFilterBy.Size = new System.Drawing.Size(168, 21);
			this.comboDateFilterBy.TabIndex = 262;
			this.comboDateFilterBy.SelectedIndexChanged += new System.EventHandler(this.comboDateFilterBy_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(287, 46);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(171, 16);
			this.label1.TabIndex = 263;
			this.label1.Text = "Date Range Applies To:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(781, 59);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(128, 16);
			this.label3.TabIndex = 108;
			this.label3.Text = "Last Error Definition";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// comboErrorDef
			// 
			this.comboErrorDef.FormattingEnabled = true;
			this.comboErrorDef.Location = new System.Drawing.Point(910, 58);
			this.comboErrorDef.Name = "comboErrorDef";
			this.comboErrorDef.Size = new System.Drawing.Size(141, 21);
			this.comboErrorDef.TabIndex = 107;
			// 
			// butMine
			// 
			this.butMine.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMine.Autosize = false;
			this.butMine.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMine.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMine.CornerRadius = 2F;
			this.butMine.Location = new System.Drawing.Point(704, 60);
			this.butMine.Name = "butMine";
			this.butMine.Size = new System.Drawing.Size(51, 21);
			this.butMine.TabIndex = 111;
			this.butMine.Text = "Mine";
			this.butMine.Click += new System.EventHandler(this.butMine_Click);
			// 
			// comboUserAssigned
			// 
			this.comboUserAssigned.ArraySelectedIndices = new int[0];
			this.comboUserAssigned.BackColor = System.Drawing.SystemColors.Window;
			this.comboUserAssigned.Items = ((System.Collections.ArrayList)(resources.GetObject("comboUserAssigned.Items")));
			this.comboUserAssigned.Location = new System.Drawing.Point(543, 60);
			this.comboUserAssigned.Name = "comboUserAssigned";
			this.comboUserAssigned.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboUserAssigned.SelectedIndices")));
			this.comboUserAssigned.Size = new System.Drawing.Size(136, 21);
			this.comboUserAssigned.TabIndex = 110;
			// 
			// butPickUser
			// 
			this.butPickUser.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickUser.Autosize = false;
			this.butPickUser.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickUser.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickUser.CornerRadius = 2F;
			this.butPickUser.Location = new System.Drawing.Point(680, 60);
			this.butPickUser.Name = "butPickUser";
			this.butPickUser.Size = new System.Drawing.Size(23, 21);
			this.butPickUser.TabIndex = 109;
			this.butPickUser.Text = "...";
			this.butPickUser.Click += new System.EventHandler(this.butPickUser_Click);
			// 
			// labelForUser
			// 
			this.labelForUser.Location = new System.Drawing.Point(456, 60);
			this.labelForUser.Name = "labelForUser";
			this.labelForUser.Size = new System.Drawing.Size(86, 16);
			this.labelForUser.TabIndex = 108;
			this.labelForUser.Text = "For User";
			this.labelForUser.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(754, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(155, 16);
			this.label4.TabIndex = 63;
			this.label4.Text = "Last Custom Tracking Status";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// comboLastClaimTrack
			// 
			this.comboLastClaimTrack.FormattingEnabled = true;
			this.comboLastClaimTrack.Location = new System.Drawing.Point(910, 15);
			this.comboLastClaimTrack.Name = "comboLastClaimTrack";
			this.comboLastClaimTrack.Size = new System.Drawing.Size(141, 21);
			this.comboLastClaimTrack.TabIndex = 62;
			// 
			// buttonUpdateCustomTrack
			// 
			this.buttonUpdateCustomTrack.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.buttonUpdateCustomTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonUpdateCustomTrack.Autosize = true;
			this.buttonUpdateCustomTrack.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.buttonUpdateCustomTrack.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.buttonUpdateCustomTrack.CornerRadius = 4F;
			this.buttonUpdateCustomTrack.Location = new System.Drawing.Point(12, 65);
			this.buttonUpdateCustomTrack.Name = "buttonUpdateCustomTrack";
			this.buttonUpdateCustomTrack.Size = new System.Drawing.Size(134, 24);
			this.buttonUpdateCustomTrack.TabIndex = 249;
			this.buttonUpdateCustomTrack.Text = "Update Custom Tracking";
			this.buttonUpdateCustomTrack.Click += new System.EventHandler(this.buttonUpdateCustomTrack_Click);
			// 
			// labelClaimCount
			// 
			this.labelClaimCount.Location = new System.Drawing.Point(186, 65);
			this.labelClaimCount.Name = "labelClaimCount";
			this.labelClaimCount.Size = new System.Drawing.Size(60, 24);
			this.labelClaimCount.TabIndex = 250;
			this.labelClaimCount.Text = "claims";
			this.labelClaimCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBoxUpdateCustomTracking
			// 
			this.groupBoxUpdateCustomTracking.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxUpdateCustomTracking.Controls.Add(this.labelCustomTracking);
			this.groupBoxUpdateCustomTracking.Controls.Add(this.buttonUpdateCustomTrack);
			this.groupBoxUpdateCustomTracking.Controls.Add(this.labelClaimCount);
			this.groupBoxUpdateCustomTracking.Location = new System.Drawing.Point(803, 618);
			this.groupBoxUpdateCustomTracking.Name = "groupBoxUpdateCustomTracking";
			this.groupBoxUpdateCustomTracking.Size = new System.Drawing.Size(280, 107);
			this.groupBoxUpdateCustomTracking.TabIndex = 254;
			this.groupBoxUpdateCustomTracking.TabStop = false;
			this.groupBoxUpdateCustomTracking.Text = "Custom Tracking";
			// 
			// labelCustomTracking
			// 
			this.labelCustomTracking.Location = new System.Drawing.Point(9, 21);
			this.labelCustomTracking.Name = "labelCustomTracking";
			this.labelCustomTracking.Size = new System.Drawing.Size(220, 41);
			this.labelCustomTracking.TabIndex = 252;
			this.labelCustomTracking.Text = "Clicking Update will change the \r\nstatus of all of the claims in the grid.";
			this.labelCustomTracking.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butAssignUser
			// 
			this.butAssignUser.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAssignUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAssignUser.Autosize = true;
			this.butAssignUser.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAssignUser.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAssignUser.CornerRadius = 4F;
			this.butAssignUser.Location = new System.Drawing.Point(12, 630);
			this.butAssignUser.Name = "butAssignUser";
			this.butAssignUser.Size = new System.Drawing.Size(79, 24);
			this.butAssignUser.TabIndex = 110;
			this.butAssignUser.Text = "Assign User";
			this.butAssignUser.Click += new System.EventHandler(this.butAssignUser_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.textGroupName);
			this.groupBox1.Controls.Add(this.textGroupNumber);
			this.groupBox1.Controls.Add(this.textCarrierPhone);
			this.groupBox1.Controls.Add(this.textCarrierName);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Location = new System.Drawing.Point(149, 618);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(321, 107);
			this.groupBox1.TabIndex = 255;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Carrier/Plan Info";
			// 
			// textGroupName
			// 
			this.textGroupName.Location = new System.Drawing.Point(116, 80);
			this.textGroupName.Name = "textGroupName";
			this.textGroupName.ReadOnly = true;
			this.textGroupName.Size = new System.Drawing.Size(190, 20);
			this.textGroupName.TabIndex = 259;
			// 
			// textGroupNumber
			// 
			this.textGroupNumber.Location = new System.Drawing.Point(116, 58);
			this.textGroupNumber.Name = "textGroupNumber";
			this.textGroupNumber.ReadOnly = true;
			this.textGroupNumber.Size = new System.Drawing.Size(190, 20);
			this.textGroupNumber.TabIndex = 258;
			// 
			// textCarrierPhone
			// 
			this.textCarrierPhone.Location = new System.Drawing.Point(116, 36);
			this.textCarrierPhone.Name = "textCarrierPhone";
			this.textCarrierPhone.ReadOnly = true;
			this.textCarrierPhone.Size = new System.Drawing.Size(190, 20);
			this.textCarrierPhone.TabIndex = 257;
			// 
			// textCarrierName
			// 
			this.textCarrierName.Location = new System.Drawing.Point(116, 14);
			this.textCarrierName.Name = "textCarrierName";
			this.textCarrierName.ReadOnly = true;
			this.textCarrierName.Size = new System.Drawing.Size(190, 20);
			this.textCarrierName.TabIndex = 256;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(5, 82);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(110, 15);
			this.label8.TabIndex = 255;
			this.label8.Text = "Group Name:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(5, 38);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(110, 15);
			this.label7.TabIndex = 254;
			this.label7.Text = "Carrier Phone:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(5, 60);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(110, 15);
			this.label6.TabIndex = 253;
			this.label6.Text = "Group Number:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(5, 16);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(110, 15);
			this.label5.TabIndex = 252;
			this.label5.Text = "Carrier:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.textPatDOB);
			this.groupBox3.Controls.Add(this.label9);
			this.groupBox3.Controls.Add(this.textSubscriberID);
			this.groupBox3.Controls.Add(this.textSubscriberDOB);
			this.groupBox3.Controls.Add(this.textSubscriberName);
			this.groupBox3.Controls.Add(this.label10);
			this.groupBox3.Controls.Add(this.label11);
			this.groupBox3.Controls.Add(this.label12);
			this.groupBox3.Location = new System.Drawing.Point(476, 618);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(321, 107);
			this.groupBox3.TabIndex = 260;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Patient/Subscriber Info";
			// 
			// textPatDOB
			// 
			this.textPatDOB.Location = new System.Drawing.Point(116, 14);
			this.textPatDOB.Name = "textPatDOB";
			this.textPatDOB.ReadOnly = true;
			this.textPatDOB.Size = new System.Drawing.Size(190, 20);
			this.textPatDOB.TabIndex = 260;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(5, 17);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(110, 15);
			this.label9.TabIndex = 259;
			this.label9.Text = "Patient DOB:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSubscriberID
			// 
			this.textSubscriberID.Location = new System.Drawing.Point(116, 80);
			this.textSubscriberID.Name = "textSubscriberID";
			this.textSubscriberID.ReadOnly = true;
			this.textSubscriberID.Size = new System.Drawing.Size(190, 20);
			this.textSubscriberID.TabIndex = 258;
			// 
			// textSubscriberDOB
			// 
			this.textSubscriberDOB.Location = new System.Drawing.Point(116, 58);
			this.textSubscriberDOB.Name = "textSubscriberDOB";
			this.textSubscriberDOB.ReadOnly = true;
			this.textSubscriberDOB.Size = new System.Drawing.Size(190, 20);
			this.textSubscriberDOB.TabIndex = 257;
			// 
			// textSubscriberName
			// 
			this.textSubscriberName.Location = new System.Drawing.Point(116, 36);
			this.textSubscriberName.Name = "textSubscriberName";
			this.textSubscriberName.ReadOnly = true;
			this.textSubscriberName.Size = new System.Drawing.Size(190, 20);
			this.textSubscriberName.TabIndex = 256;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(5, 60);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(110, 15);
			this.label10.TabIndex = 254;
			this.label10.Text = "Subscriber DOB:";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(5, 82);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(110, 15);
			this.label11.TabIndex = 253;
			this.label11.Text = "Subscriber ID:";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(5, 38);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(110, 15);
			this.label12.TabIndex = 252;
			this.label12.Text = "Subscriber Name:";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormRpOutstandingIns
			// 
			this.ClientSize = new System.Drawing.Size(1230, 729);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butAssignUser);
			this.Controls.Add(this.groupBoxUpdateCustomTracking);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.butExport);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.gridMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(300, 300);
			this.Name = "FormRpOutstandingIns";
			this.Text = "Outstanding Insurance Claims";
			this.Load += new System.EventHandler(this.FormRpOutIns_Load);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabControlDate.ResumeLayout(false);
			this.tabDaysOld.ResumeLayout(false);
			this.tabDaysOld.PerformLayout();
			this.tabDateRange.ResumeLayout(false);
			this.groupBoxUpdateCustomTracking.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private void FormRpOutIns_Load(object sender,EventArgs e) {
			SetDates(DateTime.MinValue,DateTime.Today.Date.AddDays(-30));
			if(PrefC.GetInt(PrefName.OutstandingInsReportDateFilterTab)==(int)RpOutstandingIns.DateFilterTab.DaysOld) {
				tabControlDate.SelectTab(tabDaysOld);
			}
			else {
				tabControlDate.SelectTab(tabDateRange);
			}
			FillProvs();
			FillDateFilterBy();
			_listClaimSentEditUsers=Userods.GetUsersByPermission(Permissions.ClaimSentEdit,false);
			FillUsers();
			_listOldClaimTrackings=ClaimTrackings.RefreshForUsers(ClaimTrackingType.ClaimUser,_listClaimSentEditUsers.Select(x => x.UserNum).ToList());
			_listNewClaimTrackings=_listOldClaimTrackings.Select(x => x.Copy()).ToList();
			if(PrefC.HasClinicsEnabled) {
				comboBoxMultiClinics.Visible=true;
				labelClinic.Visible=true;
				FillClinics();
			}
			if(!Security.IsAuthorized(Permissions.UpdateCustomTracking,true)) {
				buttonUpdateCustomTrack.Enabled=false;
			}
			List<MenuItem> listMenuItems=new List<MenuItem>();
			//The first item in the list will always exists, but we toggle it's visibility to only show when 1 row is selected.
			listMenuItems.Add(new MenuItem(Lan.g(this,"Go to Account"),new EventHandler(gridMain_RightClickHelper)));
			listMenuItems[0].Tag=0;//Tags are used to identify what to do in gridMain_RightClickHelper.
			listMenuItems.Add(new MenuItem(Lan.g(this,"Assign to Me"),new EventHandler(gridMain_RightClickHelper)));
			listMenuItems[1].Tag=1;
			listMenuItems.Add(new MenuItem(Lan.g(this,"Assign to User")));
			List<MenuItem> listSubUserMenu=new List<MenuItem>();
			_listClaimSentEditUsers.ForEach(x => { 
				listSubUserMenu.Add(new MenuItem(x.UserName,new EventHandler(gridMain_RightClickHelper)));
				listSubUserMenu[listSubUserMenu.Count-1].Tag=2;
			});
			listMenuItems[2].MenuItems.AddRange(listSubUserMenu.ToArray());
			Menu.MenuItemCollection menuItemCollection=new Menu.MenuItemCollection(rightClickMenu);
			menuItemCollection.AddRange(listMenuItems.ToArray());
			rightClickMenu.Popup+=new EventHandler((o,ea) => {
				rightClickMenu.MenuItems[0].Visible=(gridMain.SelectedIndices.Count()==1);//Only show 'Go to Account' when there is exactly 1 row selected.
			});
			FillCustomTrack();
			FillErrorDef();
			FillGrid(true);
		}

		private void FillProvs() {
			comboBoxMultiProv.Items.Add(new ODBoxItem<Provider>(Lan.g(this,"All"))); //tag = null
			foreach(Provider provCur in Providers.GetListReports()) {
				comboBoxMultiProv.Items.Add(new ODBoxItem<Provider>(provCur.GetLongDesc(),provCur));
			}
			comboBoxMultiProv.SetSelected(0,true);
		}

		private void FillUsers() {
			comboUserAssigned.Items.Add(new ODBoxItem<Userod>(Lans.g(this,"All"))); //tag=null
			comboUserAssigned.Items.Add(new ODBoxItem<Userod>(Lans.g(this,"Unassigned"),new Userod() {UserNum=0}));
			_listClaimSentEditUsers.ForEach(x => comboUserAssigned.Items.Add(new ODBoxItem<Userod>(x.UserName,x)));
			comboUserAssigned.SetSelected(0,true);//Default to all
		}

		private void FillClinics() {
			List<Clinic> listUserClinics=Clinics.GetForUserod(Security.CurUser,true,"Unassigned");
			comboBoxMultiClinics.Items.Clear();
			comboBoxMultiClinics.Items.Add(new ODBoxItem<Clinic>("All"));//tag=null
			foreach(Clinic clinic in listUserClinics) {
				int idx=comboBoxMultiClinics.Items.Add(new ODBoxItem<Clinic>(clinic.Abbr,clinic));
				if(Clinics.ClinicNum==clinic.ClinicNum) {
					comboBoxMultiClinics.SetSelected(idx,true);
				}
			}
			if(Clinics.ClinicNum==0) {
				comboBoxMultiClinics.SetSelected(false);//unselect 'Unassigned' if it was selected
				comboBoxMultiClinics.SetSelected(0,true);//default to all
			}
		}

		private void FillCustomTrack() {
			comboLastClaimTrack.Items.Add(new ODBoxItem<Def>("All"));//tag=null
			Def[] arrayDefs=Defs.GetDefsForCategory(DefCat.ClaimCustomTracking,true).ToArray();
			foreach(Def definition in arrayDefs) {
				comboLastClaimTrack.Items.Add(new ODBoxItem<Def>(definition.ItemName,definition));
			}
			comboLastClaimTrack.SelectedIndex=0;
		}

		private void FillErrorDef() {
			List<Def> listErrorDefs = Defs.GetDefsForCategory(DefCat.ClaimErrorCode,true);
			ODBoxItem<Def> errorItem=new ODBoxItem<Def>(Lan.g(this,"None"));//tag=null
			comboErrorDef.Items.Add(errorItem);
			comboErrorDef.SelectedIndex=0;
			foreach(Def errorDef in listErrorDefs) {
				errorItem=new ODBoxItem<Def>(errorDef.ItemName,errorDef);
				comboErrorDef.Items.Add(errorItem);
			}
		}

		private void FillDateFilterBy() {
			foreach(RpOutstandingIns.DateFilterBy filterCur in Enum.GetValues(typeof(RpOutstandingIns.DateFilterBy))) {
				comboDateFilterBy.Items.Add(new ODBoxItem<RpOutstandingIns.DateFilterBy>(Lan.g(this,filterCur.GetDescription()),filterCur));
			}
			comboDateFilterBy.SelectedIndex=0;
		}

		private void FillGrid(bool isOnLoad=false) {
			isPreauth=checkPreauth.Checked;
			List<RpOutstandingIns.OutstandingInsClaim> listOustandingInsClaims =
				RpOutstandingIns.GetOutInsClaims(_listSelectedProvNums,GetDateFrom(),GetDateTo(),
				isPreauth,_listSelectedClinicNums,textCarrier.Text,_listSelectedUserNums,comboDateFilterBy.SelectedTag<RpOutstandingIns.DateFilterBy>());
			if(isOnLoad && listOustandingInsClaims.Any(x => x.CustomTrackingDefNum != 0)) {
				//If on load and the results have custom tracking entries, uncheck the "Ignore custom tracking" box so we can show it.
				//If it's not on load don't do this check as the user manually set filters.
				checkIgnoreCustom.Checked=false;
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			List<DisplayField> listDisplayFields = DisplayFields.GetForCategory(DisplayFieldCategory.OutstandingInsReport);
			foreach(DisplayField fieldCur in listDisplayFields) {
				GridSortingStrategy sortingStrat = GridSortingStrategy.StringCompare;
				if(fieldCur.InternalName == "DateService"
					|| fieldCur.InternalName == "DateSent"
					|| fieldCur.InternalName == "DateStat"
					|| fieldCur.InternalName == "SubDOB"
					|| fieldCur.InternalName == "PatDOB") 
				{
					sortingStrat=GridSortingStrategy.DateParse;
				}
				else if(fieldCur.InternalName == "Amount") {
					sortingStrat=GridSortingStrategy.AmountParse;
				}
				col=new ODGridColumn(string.IsNullOrEmpty(fieldCur.Description) ? fieldCur.InternalName : fieldCur.Description,fieldCur.ColumnWidth,sortingStrat);
				gridMain.Columns.Add(col);
			}
			gridMain.Rows.Clear();
			ODGridRow row;
			string type;
			total=0;
			List<Def> listErrorDefs=Defs.GetDefsForCategory(DefCat.ClaimErrorCode,true);
			foreach(RpOutstandingIns.OutstandingInsClaim claimCur in listOustandingInsClaims) {
				if(!checkIgnoreCustom.Checked && claimCur.DateLog.AddDays(claimCur.DaysSuppressed)>DateTime.Today) {
					continue;
				}
				if(comboLastClaimTrack.SelectedTag<Def>() != null  
					&& claimCur.CustomTrackingDefNum!=comboLastClaimTrack.SelectedTag<Def>().DefNum) 
				{
					continue;
				}
				if(comboErrorDef.SelectedTag<Def>() != null
					&& comboErrorDef.SelectedTag<Def>().DefNum != claimCur.ErrorCodeDefNum) 
				{
					continue;
				}
				row=new ODGridRow();
				foreach(DisplayField fieldCur in listDisplayFields) {
					switch(fieldCur.InternalName) {
						case "Carrier":
							row.Cells.Add(claimCur.CarrierName);
							break;
						case "Phone":
							row.Cells.Add(claimCur.CarrierPhone);
							break;
						case "Type":
							switch(claimCur.ClaimType) {
								case "P":
									type="Pri";
									break;
								case "S":
									type="Sec";
									break;
								case "PreAuth":
									type="Preauth";
									break;
								case "Other":
									type="Other";
									break;
								case "Cap":
									type="Cap";
									break;
								case "Med":
									type="Medical";//For possible future use.
									break;
								default:
									type="Error";//Not allowed to be blank.
									break;
							}
							row.Cells.Add(Lan.g(this,type));
							break;
						case "User":
							row.Cells.Add(Userods.GetName(claimCur.UserNum));
							break;
						case "PatName":
							string patName=claimCur.PatLName+", "+claimCur.PatFName+" "+claimCur.PatMiddleI;
							if(PrefC.GetBool(PrefName.ReportsShowPatNum)) {
								row.Cells.Add(claimCur.PatNum+"-"+patName);
							}
							else {
								row.Cells.Add(patName);
							}
							break;
						case "Clinic":
							row.Cells.Add(Clinics.GetAbbr(claimCur.ClinicNum));
							break;
						case "DateService":
							row.Cells.Add(claimCur.DateService.ToShortDateString());
							break;
						case "DateSent":
							row.Cells.Add(claimCur.DateSent.ToShortDateString());
							break;
						case "DateSentOrig":
							row.Cells.Add(claimCur.DateOrigSent.ToShortDateString());
							break;
						case "TrackStat":
							row.Cells.Add(Defs.GetDefsForCategory(DefCat.ClaimCustomTracking,true)
								.FirstOrDefault(x => x.DefNum==claimCur.CustomTrackingDefNum)?.ItemName??"-");
							break;
						case "DateStat":
							row.Cells.Add(claimCur.DateLog.ToShortDateString());
							break;
						case "Error":
							row.Cells.Add(listErrorDefs.FirstOrDefault(x => x.DefNum == claimCur.ErrorCodeDefNum)?.ItemName??"-");
							break;
						case "Amount":
							row.Cells.Add(claimCur.ClaimFee.ToString("f"));
							break;
						case "GroupNum":
							row.Cells.Add(claimCur.GroupNum);
							break;
						case "GroupName":
							row.Cells.Add(claimCur.GroupName);
							break;
						case "SubName":
							row.Cells.Add(claimCur.SubName);
							break;
						case "SubDOB":
							row.Cells.Add(claimCur.SubDOB.ToShortDateString());
							break;
						case "SubID":
							row.Cells.Add(claimCur.SubID);
							break;
						case "PatDOB":
							row.Cells.Add(claimCur.PatDOB.ToShortDateString());
							break;
						default:
							row.Cells.Add(Lan.g(this,"MISSING"));
							break;
					}
				}
				row.Tag=claimCur;
				gridMain.Rows.Add(row);
				total+=claimCur.ClaimFee;
			}
			gridMain.EndUpdate();
			textBox1.Text=total.ToString("c");
			labelClaimCount.Text=string.Format("{0} {1}",gridMain.Rows.Count,gridMain.Rows.Count==1 ? Lan.g(this,"claim") : Lan.g(this,"claims"));
			RefreshSelectedInfo();
		}
		
		private void butRefresh_Click(object sender,EventArgs e) {
			Plugins.HookAddCode(this,"FormRpOutstandingIns.butRefresh_begin");
			FillGrid();
		}

		private void checkPreauth_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void checkIgnoreCustom_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void comboBoxMultiProv_Leave(object sender,EventArgs e) {
			if(comboBoxMultiProv.SelectedIndices.Count==0) {
				comboBoxMultiProv.SetSelected(0,true);
			}
			else if(comboBoxMultiProv.SelectedIndices.Contains(0)) {
				comboBoxMultiProv.SelectedIndicesClear();
				comboBoxMultiProv.SetSelected(0,true);
			}
		}

		///<summary>Click method used by all gridMain right click options.
		///We identify what logic to run by the menuItem.Tag.</summary>
		private void gridMain_RightClickHelper(object sender,EventArgs e) {
			int index=gridMain.GetSelectedIndex();
			if(index==-1) {
				return;
			}
			int menuCode=(int)((MenuItem)sender).Tag;
			switch(menuCode) {
				case 0://Go to Account
					GotoModule.GotoAccount(((RpOutstandingIns.OutstandingInsClaim)gridMain.Rows[index].Tag).PatNum);
				break;
				case 1://Assign to Me
					AssignUserHelper(Security.CurUser.UserNum);
				break;
				case 2://Assign to User
					AssignUserHelper(_listClaimSentEditUsers[((MenuItem)sender).Index].UserNum);
				break;
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.ClaimView)) {
				return;
			}
			Claim claim=Claims.GetClaim(((RpOutstandingIns.OutstandingInsClaim)gridMain.Rows[e.Row].Tag).ClaimNum);
			if(claim==null) {
				MsgBox.Show(this,"The claim has been deleted.");
				FillGrid();
				return;
			}
			Patient pat=Patients.GetPat(claim.PatNum);
			Family fam=Patients.GetFamily(pat.PatNum);
			FormClaimEdit FormCE=new FormClaimEdit(claim,pat,fam);
			FormCE.IsNew=false;
			FormCE.ShowDialog();
		}

		private void buttonUpdateCustomTrack_Click(object sender,EventArgs e) {			
			List<long> listClaimNum=new List<long>();
			for(int i = 0;i<gridMain.Rows.Count;i++) {
				listClaimNum.Add(((RpOutstandingIns.OutstandingInsClaim)gridMain.Rows[i].Tag).ClaimNum);
			}
			if(listClaimNum.Count==0) {
				MsgBox.Show(this,"No claims in list. Must have at least one claim.");
				return;
			}
			List<Claim> listClaims=Claims.GetClaimsFromClaimNums(listClaimNum);
			FormClaimCustomTrackingUpdate FormCT=new FormClaimCustomTrackingUpdate(listClaims);
			if(FormCT.ShowDialog()==DialogResult.OK) {
				FillGrid();
			}
		}

		private void butCancel_Click(object sender,EventArgs e) {
			Close();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			pagesPrinted=0;
			PrintDocument pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
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
					if(PrinterL.SetPrinter(pd,PrintSituation.Default,0,"Outstanding insurance report printed")) {
						pd.Print();
					}
			#endif
			}
			catch {
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
		}

		private void butAssignUser_Click(object sender,EventArgs e) {
			long userNum=PickUser(true);
			if(userNum==-2) {//User canceled selection.
				return;
			}
			AssignUserHelper(userNum);
		}

		///<summary>Loops through gridMain.SelectedIndices to create or update ClaimTracking rows.</summary>
		private void AssignUserHelper(long assignUserNum) {
			if(gridMain.SelectedIndices.Count()==0) {
				MsgBox.Show(this,"Please select at least one claim to assign a user.");
				return;
			}
			List<ODTuple<long,long>> listTrackingNumsAndClaimNums=new List<ODTuple<long,long>>();
			foreach(int index in gridMain.SelectedIndices) {
				RpOutstandingIns.OutstandingInsClaim outstandingInsClaim = (RpOutstandingIns.OutstandingInsClaim)gridMain.Rows[index].Tag;
				long claimTrackingNum=_listNewClaimTrackings.FirstOrDefault(x => x.ClaimNum==outstandingInsClaim.ClaimNum)?.ClaimTrackingNum??0;
				long claimNum = outstandingInsClaim?.ClaimNum??0;
				listTrackingNumsAndClaimNums.Add(new Tuple<long, long>(claimTrackingNum,claimNum));
			}
			_listNewClaimTrackings=ClaimTrackings.Assign(listTrackingNumsAndClaimNums,assignUserNum);
			_listOldClaimTrackings.Clear();//After sync, the old list is updated.
			_listNewClaimTrackings.ForEach(x => _listOldClaimTrackings.Add(x.Copy()));
			FillGrid();
		}

		private void butPickUser_Click(object sender,EventArgs e) {
			long userNum=PickUser(false);
			if(userNum==-2) {//User canceled selection.
				return;
			}
			ComboUserPickHelper(userNum);
		}

		///<summary>After calling PickUser(false) this is used to set comboUserAssigneds selection.
		///Also calls FillGrid() to reflect new selection.</summary>
		private void ComboUserPickHelper(long filterUserNum) {
			int selectedIndex=0;//Defaults to 'All', filterUserNum will be -1 in this case.
			if(filterUserNum==0) {//None or 'Unassigned'
				selectedIndex=1;//'Unassigned'
			}
			else if(filterUserNum!=-1){//Not 'All', this is a specific user selection.
				int index=_listClaimSentEditUsers.FindIndex(x => x.UserNum==filterUserNum);
				if(index==-1) {//Just in case.
					return;
				}
				selectedIndex=index+2;
			}
			comboUserAssigned.SelectedIndicesClear();
			comboUserAssigned.SetSelected(selectedIndex,true);//Offset by 2 for 'All' and 'Unassigned'
			FillGrid();
		}

		///<summary>Opens FormUserPick to allow user to select a user.
		///Returns UserNum associated to selection.
		///0 represents Unassigned
		///-1 represents All
		///-2 represents a canceled selection</summary>
		private long PickUser(bool isAssigning) {
			FormUserPick FormUP=new FormUserPick();
			FormUP.IsSelectionmode=true;
			FormUP.ListUserodsFiltered=_listClaimSentEditUsers;
			if(!isAssigning) {
				FormUP.IsPickAllAllowed=true;
			}
			FormUP.IsPickNoneAllowed=true;
			FormUP.ShowDialog();
			if(FormUP.DialogResult!=DialogResult.OK) {
				return -2;
			}
			return FormUP.SelectedUserNum;
		}

		/// <summary>Sets date controls in both date tabs</summary>
		private void SetDates(DateTime dateFrom,DateTime dateTo) {
			string daysOldMin=POut.Int((int)Math.Round((DateTime.Today-dateTo.Date).TotalDays,0));//Calculate min days old from dateTo.
			string daysOldMax="";
			if(dateFrom>dateTo) {
				dateFrom=dateTo.Date;//dateFrom cannot be after dateTo
			}
			if(dateFrom>DateTime.MinValue) {
				daysOldMax=POut.Int((int)Math.Round((DateTime.Today-dateFrom.Date).TotalDays,0));//Calculate max days old from dateFrom.
			}
			else {
				dateFrom=DateTime.MinValue;//MinValue, but show a blank in the date text box.
				daysOldMax="";//DaysOld max field should be blank.
			}
			textDaysOldMax.Text=daysOldMax;//DateFrom
			dateRangePickerVertical.SetDateTimeFrom(dateFrom);//DateFrom
			textDaysOldMin.Text=daysOldMin;////DateTo
			dateRangePickerVertical.SetDateTimeTo(dateTo);//DateTo			
		}

		///<summary>Gets date control for report</summary>
		private DateTime GetDateTo() {
			return GetDateTo(tabControlDate.SelectedTab);
		}

		///<summary>Gets date control for report from a specific tab in tabControlDate</summary>
		private DateTime GetDateTo(TabPage tabPageCur) {
			DateTime dateMin=DateTime.Today;
			if(tabPageCur==tabDaysOld) {
				int daysOldMin=0;
				int.TryParse(textDaysOldMin.Text.Trim(),out daysOldMin);
				//can't use error provider here because this fires on text changed and cursor may not have left the control, so there is no error message yet
				if(daysOldMin>0 && daysOldMin.Between(textDaysOldMin.MinVal,textDaysOldMin.MaxVal)) {
					dateMin=DateTimeOD.Today.AddDays(-1*daysOldMin);
				}
			}
			else if(tabPageCur==tabDateRange) {
				dateMin=dateRangePickerVertical.GetDateTimeTo();//Very end of day.
				if(dateMin>DateTime.Today) {
					dateMin=DateTime.Today.Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
					dateRangePickerVertical.SetDateTimeTo(dateMin);
				}
			}
			return dateMin;
		}

		///<summary>Gets date control for report</summary>
		private DateTime GetDateFrom() {
			return GetDateFrom(tabControlDate.SelectedTab);
		}
		
		///<summary>Gets date control for report from a specific tab in tabControlDate</summary>
		private DateTime GetDateFrom(TabPage tabPageCur) {
			DateTime dateMax=DateTime.MinValue;
			if(tabPageCur==tabDaysOld) {
				int daysOldMax=0;
				int.TryParse(textDaysOldMax.Text.Trim(),out daysOldMax);
				//can't use error provider here because this fires on text changed and cursor may not have left the control, so there is no error message yet
				if(daysOldMax>0 && daysOldMax.Between(textDaysOldMax.MinVal,textDaysOldMax.MaxVal)) {
					dateMax=DateTimeOD.Today.AddDays(-1*daysOldMax);
				}
			}
			else if(tabPageCur==tabDateRange) {
				dateMax=dateRangePickerVertical.GetDateTimeFrom().Date;//Very beginning of day.
				if(dateMax>DateTime.Today) {
					dateMax=DateTime.Today;
					dateRangePickerVertical.SetDateTimeFrom(dateMax);
				}
			}
			return dateMax;
		}

		private void butMine_Click(object sender,EventArgs e) {
			FillClinics();
			ComboUserPickHelper(Security.CurUser.UserNum);
		}

		private void comboDateFilterBy_SelectedIndexChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textDaysOldMin_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textDaysOldMax_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void tabControlDate_SelectedIndexChanged(object sender,EventArgs e) {
			TabPage tabPagePrevious=(tabControlDate.SelectedTab==tabDaysOld?tabDateRange:tabDaysOld);//Get dates from tab we are leaving.
			DateTime dateFrom=GetDateFrom(tabPagePrevious);
			DateTime dateTo=GetDateTo(tabPagePrevious);
			SetDates(dateFrom,dateTo);//Make sure both tabDaysOld and tabDateRange are concurrent.
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			RefreshSelectedInfo();
		}

		private void dateRangePickerVertical_CalendarClosed(object sender,EventArgs e) {
			FillGrid();
		}

		private void dateRangePickerVertical_CalendarSelectionChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void RefreshSelectedInfo() {
			List<RpOutstandingIns.OutstandingInsClaim> listSelected=gridMain.SelectedGridRows.Select(x => x.Tag)
				.OfType<RpOutstandingIns.OutstandingInsClaim>().ToList();
			if(listSelected.Count == 0) {
				//clear textboxes
				textPatDOB.Text="";
				textSubscriberID.Text="";
				textSubscriberName.Text="";
				textSubscriberDOB.Text="";
				textCarrierName.Text="";
				textCarrierPhone.Text="";
				textGroupName.Text="";
				textGroupNumber.Text="";
			}
			else if(listSelected.Count == 1) {
				RpOutstandingIns.OutstandingInsClaim insClaim = listSelected.First();
				//fill textboxes
				textPatDOB.Text=insClaim.PatDOB.ToShortDateString();
				textSubscriberID.Text=insClaim.SubID;
				textSubscriberName.Text=insClaim.SubName;
				textSubscriberDOB.Text=insClaim.SubDOB.ToShortDateString();
				textCarrierName.Text=insClaim.CarrierName;
				textCarrierPhone.Text=insClaim.CarrierPhone;
				textGroupName.Text=insClaim.GroupName;
				textGroupNumber.Text=insClaim.GroupNum;
			}
			else if(listSelected.Count > 1) {
				//fill textboxes
				string multiSelectedStr = "<"+Lans.g(this,"Multiple Selected")+">";
				textPatDOB.Text=multiSelectedStr;
				textSubscriberID.Text=multiSelectedStr;
				textSubscriberName.Text=multiSelectedStr;
				textSubscriberDOB.Text=multiSelectedStr;
				textCarrierName.Text=multiSelectedStr;
				textCarrierPhone.Text=multiSelectedStr;
				textGroupName.Text=multiSelectedStr;
				textGroupNumber.Text=multiSelectedStr;
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
				text=Lan.g(this,"Outstanding Insurance Claims");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				if(isPreauth) {
					text=Lan.g(this,"Including Preauthorization");
				}
				else {
					text=Lan.g(this,"Not Including Preauthorization");
				}
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
				//print today's date
				text = Lan.g(this,"Run On:")+" "+DateTimeOD.Today.ToShortDateString();
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
				if(_listSelectedProvNums.Count==0) {
					text=Lan.g(this,"For All Providers");
				}
				else {
					text=Lan.g(this,"For Providers:")+" ";
					for(int i=0;i<_listSelectedProvNums.Count;i++) {
						text+=Providers.GetFormalName(_listSelectedProvNums[i]);
					}
				}
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
				text="Total: $"+total.ToString("F");
				g.DrawString(text,subHeadingFont,Brushes.Black,center+bounds.Width/2-g.MeasureString(text,subHeadingFont).Width,yPos);
			}
			g.Dispose();
		}

		private void butExport_Click(object sender,System.EventArgs e) {
			SaveFileDialog saveFileDialog=new SaveFileDialog();
			saveFileDialog.AddExtension=true;
			saveFileDialog.FileName="Outstanding Insurance Claims";
			if(!Directory.Exists(PrefC.GetString(PrefName.ExportPath))) {
				try {
					Directory.CreateDirectory(PrefC.GetString(PrefName.ExportPath));
					saveFileDialog.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
				}
				catch {
					//initialDirectory will be blank
				}
			}
			else {
				saveFileDialog.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
			}
			saveFileDialog.Filter="Text files(*.txt)|*.txt|Excel Files(*.xls)|*.xls|All files(*.*)|*.*";
			saveFileDialog.FilterIndex=0;
			if(saveFileDialog.ShowDialog()!=DialogResult.OK) {
				return;
			}
			try {
				using(StreamWriter sw=new StreamWriter(saveFileDialog.FileName,false))
				//new FileStream(,FileMode.Create,FileAccess.Write,FileShare.Read)))
				{
					String line="";
					for(int i=0;i<gridMain.Columns.Count;i++) {
						line+=gridMain.Columns[i].Heading+"\t";
					}
					sw.WriteLine(line);
					for(int i=0;i<gridMain.Rows.Count;i++) {
						line="";
						for(int j=0;j<gridMain.Columns.Count;j++) {
							line+=gridMain.Rows[i].Cells[j].Text;
							if(j<gridMain.Columns.Count-1) {
								line+="\t";
							}
						}
						sw.WriteLine(line);
					}
				}
			}
			catch {
				MessageBox.Show(Lan.g(this,"File in use by another program.  Close and try again."));
				return;
			}
			MessageBox.Show(Lan.g(this,"File created successfully"));
		}

	}
}