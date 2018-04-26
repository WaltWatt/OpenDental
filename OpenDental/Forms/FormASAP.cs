using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	///<summary></summary>
	public class FormASAP:ODForm {
		private IContainer components;
		private OpenDental.UI.Button butClose;
		///<summary></summary>
		public bool PinClicked=false;
		///<summary></summary>
		public static string procsForCur;
		private OpenDental.UI.ODGrid gridAppts;
		private OpenDental.UI.Button butPrint;
		private List<Appointment> _listASAPs;
		private PrintDocument pd;
		private bool headingPrinted;
		private int pagesPrinted;
		///<summary>Only used if PinClicked=true</summary>
		public long AptSelected;
		private ComboBox comboProv;
		private Label label4;
		private ComboBox comboSite;
		private Label labelSite;
		///<summary>When this form closes, this will be the patNum of the last patient viewed.  The calling form should then make use of this to refresh to that patient.  If 0, then calling form should not refresh.</summary>
		public long SelectedPatNum;
		private ComboBox comboClinic;
		private Label labelClinic;
		private Dictionary<long,string> _dictPatientNames;
		private List<Clinic> _listUserClinics;
		private ToolStripMenuItem toolStripMenuItemSelectPatient;
		private ToolStripMenuItem toolStripMenuItemSeeChart;
		private ToolStripMenuItem menuApptSendToPinboard;
		private ToolStripMenuItem toolStripMenuItemRemoveFromASAP;
		private ToolStripMenuItem toolStripMenuItemMakeAppt;
		private UI.Button butText;
		private ContextMenuStrip menuApptsRightClick;
		private ContextMenuStrip menuRecallsRightClick;
		private ComboBox comboEnd;
		private ComboBox comboStart;
		private Label labelEnd;
		private Label labelStart;
		private UI.Button butSendWebSched;

		///<summary>When texting patients on this list, this will be when the beginning of the slot is.</summary>
		private DateTime _dateTimeChosen;
		///<summary>When texting patients on this list, this will be when the beginning of the slot is.</summary>
		private DateTime _dateTimeSlotStart;
		///<summary>When texting patients on this list, this will be when the beginning of the slot is.</summary>
		private DateTime _dateTimeSlotEnd;
		private UI.Button butWebSchedHist;
		private GroupBox groupWebSched;
		///<summary>The clinics that are signed up for Web Sched.</summary>
		private List<long> _listClinicNumsWebSched=new List<long>();
		ODThread _threadWebSchedSignups=null;
		///<summary>The user has clicked the Web Sched button while a thread was busy checking which clinics are signed up for Web Sched.</summary>
		private bool _hasClickedWebSched;
		///<summary>The operatory selected to send Web Sched messages for.</summary>
		private long _opNum;
		private SplitContainer splitContainer;
		private ODGrid gridWebSched;
		private Label labelOperatory;
		private TabControl tabControl;
		private TabPage tabPageAppts;
		private TabPage tabPageRecalls;
		private ODGrid gridRecalls;
		private ComboBox comboNumberReminders;
		private Label label3;
		private CheckBox checkGroupFamilies;
		private ValidDate textDateEnd;
		private ValidDate textDateStart;
		private Label label2;
		private Label label1;
		private ComboBoxMulti comboAptStatus;
		private Label labelAptStatus;
		private ToolStripMenuItem menuApptSelectPatient;
		private ToolStripMenuItem menuApptSeeChart;
		private ToolStripMenuItem menuApptRemoveFromASAP;
		private MainMenu mainMenu;
		private MenuItem menuItemSettings;
		private UI.Button butRefresh;

		///<summary>Classwide instance of FormWebSchedASAPHistory.</summary>
		private FormWebSchedASAPHistory _formWebSchedHist;
		private List<Provider> _listProviders;
		private List<Site> _listSites;

		///<summary>True if the thread checking the clinics that are signed up for Web Sched has finished.</summary>
		private bool _isDoneCheckingWebSchedClinics {
			get { return _threadWebSchedSignups==null; }
		}

		private DateTime _selectedTimeStart {
			get {
				ComboTimeItem comboItem=(ComboTimeItem)comboStart.SelectedItem;
				if(comboItem==null) {
					return DateTime.MinValue;
				}
				return comboItem.Time;
			}
		}

		private DateTime _selectedTimeEnd {
			get {
				ComboTimeItem comboItem=(ComboTimeItem)comboEnd.SelectedItem;
				if(comboItem==null) {
					return DateTime.MinValue;
				}
				return comboItem.Time;
			}
		}

		///<summary>Returns 0 if clinics are not enabled or if 'All' is selected.</summary>
		private long _selectedClinicNum {
			get {
				//if clinics are not enabled, comboClinic.SelectedIndex will be -1, so clinicNum will be 0 and list will not be filtered by clinic
				if(Security.CurUser.ClinicIsRestricted && comboClinic.SelectedIndex>-1) {
					return _listUserClinics[comboClinic.SelectedIndex].ClinicNum;
				}
				else if(comboClinic.SelectedIndex > 0) {//if user is not restricted, clinicNum will be 0 and the query will get all clinic data
					return _listUserClinics[comboClinic.SelectedIndex-1].ClinicNum;//if user is not restricted, comboClinic will contain "All" so minus 1
				}
				return 0;
			}
			set {
				if(!PrefC.HasClinicsEnabled) {
					return;
				}
				if(Security.CurUser.ClinicIsRestricted) {
					comboClinic.SelectedIndex=_listUserClinics.FindIndex(x => x.ClinicNum==value);//Will be -1 if the clinic is not found
				}
				else {
					if(value==0) {
						comboClinic.SelectedIndex=0;//'All'
					}
					else {
						comboClinic.SelectedIndex=_listUserClinics.FindIndex(x => x.ClinicNum==value)+1;//Plus 1 to account for 'All'
					}
				}
			}
		}

		private List<Appointment> _listSelectedAppts {
			get {
				return gridAppts.SelectedIndices.Select(x => (Appointment)gridAppts.Rows[x].Tag).ToList();
			}
		}

		private List<Recall> _listSelectedRecalls {
			get {
				return gridRecalls.SelectedIndices.Select(x => (Recall)gridRecalls.Rows[x].Tag).ToList();
			}
		}

		///<summary>Whether or not the user is sending Web Sched notifications to patients.</summary>
		private bool _isSendingWebSched {
			get {
				return _opNum!=0;
			}
			set {
				if(!value) {
					_opNum=0;
				}
			}
		}

		///<summary></summary>
		public FormASAP() {
			InitializeComponent();// Required for Windows Form Designer support
			Lan.F(this);
		}

		///<summary></summary>
		///<param name="dateTimeChosen">When texting patients on this list, this will be when the beginning of the slot is</param>
		public FormASAP(DateTime dateTimeChosen) : this() {
			_dateTimeChosen=dateTimeChosen;
		}

		public FormASAP(DateTime dateTimeChosen,DateTime dateTimeSlotStart,DateTime dateTimeSlotEnd,long opNum) : this(dateTimeChosen) {
			_dateTimeChosen=dateTimeChosen;
			_dateTimeSlotStart=dateTimeSlotStart;
			_dateTimeSlotEnd=dateTimeSlotEnd;
			_opNum=opNum;
		}
		
		///<summary></summary>
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormASAP));
			this.butClose = new OpenDental.UI.Button();
			this.menuApptsRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuApptSelectPatient = new System.Windows.Forms.ToolStripMenuItem();
			this.menuApptSeeChart = new System.Windows.Forms.ToolStripMenuItem();
			this.menuApptSendToPinboard = new System.Windows.Forms.ToolStripMenuItem();
			this.menuApptRemoveFromASAP = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemSelectPatient = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemSeeChart = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemRemoveFromASAP = new System.Windows.Forms.ToolStripMenuItem();
			this.menuRecallsRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItemMakeAppt = new System.Windows.Forms.ToolStripMenuItem();
			this.butPrint = new OpenDental.UI.Button();
			this.comboProv = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.comboSite = new System.Windows.Forms.ComboBox();
			this.labelSite = new System.Windows.Forms.Label();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.butText = new OpenDental.UI.Button();
			this.comboEnd = new System.Windows.Forms.ComboBox();
			this.comboStart = new System.Windows.Forms.ComboBox();
			this.labelEnd = new System.Windows.Forms.Label();
			this.labelStart = new System.Windows.Forms.Label();
			this.butSendWebSched = new OpenDental.UI.Button();
			this.butWebSchedHist = new OpenDental.UI.Button();
			this.groupWebSched = new System.Windows.Forms.GroupBox();
			this.labelOperatory = new System.Windows.Forms.Label();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPageAppts = new System.Windows.Forms.TabPage();
			this.comboAptStatus = new OpenDental.UI.ComboBoxMulti();
			this.labelAptStatus = new System.Windows.Forms.Label();
			this.gridAppts = new OpenDental.UI.ODGrid();
			this.tabPageRecalls = new System.Windows.Forms.TabPage();
			this.gridRecalls = new OpenDental.UI.ODGrid();
			this.textDateStart = new OpenDental.ValidDate();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.comboNumberReminders = new System.Windows.Forms.ComboBox();
			this.textDateEnd = new OpenDental.ValidDate();
			this.label3 = new System.Windows.Forms.Label();
			this.checkGroupFamilies = new System.Windows.Forms.CheckBox();
			this.gridWebSched = new OpenDental.UI.ODGrid();
			this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
			this.menuItemSettings = new System.Windows.Forms.MenuItem();
			this.butRefresh = new OpenDental.UI.Button();
			this.menuApptsRightClick.SuspendLayout();
			this.menuRecallsRightClick.SuspendLayout();
			this.groupWebSched.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.tabPageAppts.SuspendLayout();
			this.tabPageRecalls.SuspendLayout();
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
			this.butClose.Location = new System.Drawing.Point(858, 606);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(87, 24);
			this.butClose.TabIndex = 7;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// menuApptsRightClick
			// 
			this.menuApptsRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuApptSelectPatient,
            this.menuApptSeeChart,
            this.menuApptSendToPinboard,
            this.menuApptRemoveFromASAP});
			this.menuApptsRightClick.Name = "_menuRightClick";
			this.menuApptsRightClick.Size = new System.Drawing.Size(179, 92);
			// 
			// menuApptSelectPatient
			// 
			this.menuApptSelectPatient.Name = "menuApptSelectPatient";
			this.menuApptSelectPatient.Size = new System.Drawing.Size(178, 22);
			this.menuApptSelectPatient.Text = "Select Patient";
			this.menuApptSelectPatient.Click += new System.EventHandler(this.gridMenuRight_click);
			// 
			// menuApptSeeChart
			// 
			this.menuApptSeeChart.Name = "menuApptSeeChart";
			this.menuApptSeeChart.Size = new System.Drawing.Size(178, 22);
			this.menuApptSeeChart.Text = "See Chart";
			this.menuApptSeeChart.Click += new System.EventHandler(this.gridMenuRight_click);
			// 
			// menuApptSendToPinboard
			// 
			this.menuApptSendToPinboard.Name = "menuApptSendToPinboard";
			this.menuApptSendToPinboard.Size = new System.Drawing.Size(178, 22);
			this.menuApptSendToPinboard.Text = "Send to Pinboard";
			this.menuApptSendToPinboard.Click += new System.EventHandler(this.gridMenuRight_click);
			// 
			// menuApptRemoveFromASAP
			// 
			this.menuApptRemoveFromASAP.Name = "menuApptRemoveFromASAP";
			this.menuApptRemoveFromASAP.Size = new System.Drawing.Size(178, 22);
			this.menuApptRemoveFromASAP.Text = "Remove from ASAP";
			this.menuApptRemoveFromASAP.Click += new System.EventHandler(this.gridMenuRight_click);
			// 
			// toolStripMenuItemSelectPatient
			// 
			this.toolStripMenuItemSelectPatient.Name = "toolStripMenuItemSelectPatient";
			this.toolStripMenuItemSelectPatient.Size = new System.Drawing.Size(178, 22);
			this.toolStripMenuItemSelectPatient.Text = "Select Patient";
			this.toolStripMenuItemSelectPatient.Click += new System.EventHandler(this.gridMenuRight_click);
			// 
			// toolStripMenuItemSeeChart
			// 
			this.toolStripMenuItemSeeChart.Name = "toolStripMenuItemSeeChart";
			this.toolStripMenuItemSeeChart.Size = new System.Drawing.Size(178, 22);
			this.toolStripMenuItemSeeChart.Text = "See Chart";
			this.toolStripMenuItemSeeChart.Click += new System.EventHandler(this.gridMenuRight_click);
			// 
			// toolStripMenuItemRemoveFromASAP
			// 
			this.toolStripMenuItemRemoveFromASAP.Name = "toolStripMenuItemRemoveFromASAP";
			this.toolStripMenuItemRemoveFromASAP.Size = new System.Drawing.Size(178, 22);
			this.toolStripMenuItemRemoveFromASAP.Text = "Remove from ASAP";
			this.toolStripMenuItemRemoveFromASAP.Click += new System.EventHandler(this.gridMenuRight_click);
			// 
			// menuRecallsRightClick
			// 
			this.menuRecallsRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSelectPatient,
            this.toolStripMenuItemSeeChart,
            this.toolStripMenuItemMakeAppt,
            this.toolStripMenuItemRemoveFromASAP});
			this.menuRecallsRightClick.Name = "_menuRightClick";
			this.menuRecallsRightClick.Size = new System.Drawing.Size(179, 92);
			// 
			// toolStripMenuItemMakeAppt
			// 
			this.toolStripMenuItemMakeAppt.Name = "toolStripMenuItemMakeAppt";
			this.toolStripMenuItemMakeAppt.Size = new System.Drawing.Size(178, 22);
			this.toolStripMenuItemMakeAppt.Text = "Make Appointment";
			this.toolStripMenuItemMakeAppt.Click += new System.EventHandler(this.gridMenuRight_click);
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
			this.butPrint.Location = new System.Drawing.Point(858, 515);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(87, 24);
			this.butPrint.TabIndex = 21;
			this.butPrint.Text = "Print List";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// comboProv
			// 
			this.comboProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProv.Location = new System.Drawing.Point(89, 0);
			this.comboProv.MaxDropDownItems = 40;
			this.comboProv.Name = "comboProv";
			this.comboProv.Size = new System.Drawing.Size(181, 21);
			this.comboProv.TabIndex = 33;
			this.comboProv.SelectionChangeCommitted += new System.EventHandler(this.combo_SelectionChangeCommitted);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(18, 4);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(69, 14);
			this.label4.TabIndex = 32;
			this.label4.Text = "Provider";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboSite
			// 
			this.comboSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSite.Location = new System.Drawing.Point(632, 0);
			this.comboSite.MaxDropDownItems = 40;
			this.comboSite.Name = "comboSite";
			this.comboSite.Size = new System.Drawing.Size(181, 21);
			this.comboSite.TabIndex = 37;
			this.comboSite.SelectionChangeCommitted += new System.EventHandler(this.combo_SelectionChangeCommitted);
			// 
			// labelSite
			// 
			this.labelSite.Location = new System.Drawing.Point(560, 4);
			this.labelSite.Name = "labelSite";
			this.labelSite.Size = new System.Drawing.Size(71, 14);
			this.labelSite.TabIndex = 36;
			this.labelSite.Text = "Site";
			this.labelSite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(365, 0);
			this.comboClinic.MaxDropDownItems = 40;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(181, 21);
			this.comboClinic.TabIndex = 39;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.combo_SelectionChangeCommitted);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(287, 4);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(77, 14);
			this.labelClinic.TabIndex = 38;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butText
			// 
			this.butText.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butText.Autosize = false;
			this.butText.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butText.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butText.CornerRadius = 4F;
			this.butText.Image = global::OpenDental.Properties.Resources.Text;
			this.butText.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butText.Location = new System.Drawing.Point(858, 171);
			this.butText.Name = "butText";
			this.butText.Size = new System.Drawing.Size(87, 24);
			this.butText.TabIndex = 62;
			this.butText.Text = "Text";
			this.butText.Click += new System.EventHandler(this.butText_Click);
			// 
			// comboEnd
			// 
			this.comboEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboEnd.FormattingEnabled = true;
			this.comboEnd.Location = new System.Drawing.Point(13, 101);
			this.comboEnd.MaxDropDownItems = 48;
			this.comboEnd.Name = "comboEnd";
			this.comboEnd.Size = new System.Drawing.Size(108, 21);
			this.comboEnd.TabIndex = 66;
			this.comboEnd.Visible = false;
			this.comboEnd.SelectionChangeCommitted += new System.EventHandler(this.comboEnd_SelectionChangeCommitted);
			// 
			// comboStart
			// 
			this.comboStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStart.FormattingEnabled = true;
			this.comboStart.Location = new System.Drawing.Point(13, 59);
			this.comboStart.MaxDropDownItems = 48;
			this.comboStart.Name = "comboStart";
			this.comboStart.Size = new System.Drawing.Size(108, 21);
			this.comboStart.TabIndex = 65;
			this.comboStart.Visible = false;
			this.comboStart.SelectedIndexChanged += new System.EventHandler(this.comboStart_SelectedIndexChanged);
			// 
			// labelEnd
			// 
			this.labelEnd.Location = new System.Drawing.Point(15, 82);
			this.labelEnd.Name = "labelEnd";
			this.labelEnd.Size = new System.Drawing.Size(106, 16);
			this.labelEnd.TabIndex = 64;
			this.labelEnd.Text = "End Time";
			this.labelEnd.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.labelEnd.Visible = false;
			// 
			// labelStart
			// 
			this.labelStart.Location = new System.Drawing.Point(14, 42);
			this.labelStart.Name = "labelStart";
			this.labelStart.Size = new System.Drawing.Size(107, 16);
			this.labelStart.TabIndex = 63;
			this.labelStart.Text = "Start Time";
			this.labelStart.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.labelStart.Visible = false;
			// 
			// butSendWebSched
			// 
			this.butSendWebSched.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSendWebSched.Autosize = false;
			this.butSendWebSched.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSendWebSched.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSendWebSched.CornerRadius = 4F;
			this.butSendWebSched.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSendWebSched.Location = new System.Drawing.Point(26, 132);
			this.butSendWebSched.Name = "butSendWebSched";
			this.butSendWebSched.Size = new System.Drawing.Size(87, 24);
			this.butSendWebSched.TabIndex = 67;
			this.butSendWebSched.Text = "Send";
			this.butSendWebSched.Click += new System.EventHandler(this.butWebSched_Click);
			// 
			// butWebSchedHist
			// 
			this.butWebSchedHist.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butWebSchedHist.Autosize = false;
			this.butWebSchedHist.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butWebSchedHist.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butWebSchedHist.CornerRadius = 4F;
			this.butWebSchedHist.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butWebSchedHist.Location = new System.Drawing.Point(26, 164);
			this.butWebSchedHist.Name = "butWebSchedHist";
			this.butWebSchedHist.Size = new System.Drawing.Size(87, 24);
			this.butWebSchedHist.TabIndex = 68;
			this.butWebSchedHist.Text = "History";
			this.butWebSchedHist.Click += new System.EventHandler(this.butWebSchedHist_Click);
			// 
			// groupWebSched
			// 
			this.groupWebSched.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupWebSched.Controls.Add(this.labelOperatory);
			this.groupWebSched.Controls.Add(this.butSendWebSched);
			this.groupWebSched.Controls.Add(this.butWebSchedHist);
			this.groupWebSched.Controls.Add(this.labelEnd);
			this.groupWebSched.Controls.Add(this.comboEnd);
			this.groupWebSched.Controls.Add(this.comboStart);
			this.groupWebSched.Controls.Add(this.labelStart);
			this.groupWebSched.Location = new System.Drawing.Point(831, 228);
			this.groupWebSched.Name = "groupWebSched";
			this.groupWebSched.Size = new System.Drawing.Size(140, 198);
			this.groupWebSched.TabIndex = 69;
			this.groupWebSched.TabStop = false;
			this.groupWebSched.Text = "Web Sched ASAP";
			// 
			// labelOperatory
			// 
			this.labelOperatory.Location = new System.Drawing.Point(6, 16);
			this.labelOperatory.Name = "labelOperatory";
			this.labelOperatory.Size = new System.Drawing.Size(130, 26);
			this.labelOperatory.TabIndex = 69;
			this.labelOperatory.Text = "Operatory:";
			this.labelOperatory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelOperatory.Visible = false;
			// 
			// splitContainer
			// 
			this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer.Location = new System.Drawing.Point(12, 35);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.tabControl);
			this.splitContainer.Panel1MinSize = 50;
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.gridWebSched);
			this.splitContainer.Panel2MinSize = 0;
			this.splitContainer.Size = new System.Drawing.Size(811, 598);
			this.splitContainer.SplitterDistance = 450;
			this.splitContainer.TabIndex = 70;
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabPageAppts);
			this.tabControl.Controls.Add(this.tabPageRecalls);
			this.tabControl.Location = new System.Drawing.Point(0, 3);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(811, 448);
			this.tabControl.TabIndex = 73;
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			// 
			// tabPageAppts
			// 
			this.tabPageAppts.BackColor = System.Drawing.Color.Transparent;
			this.tabPageAppts.Controls.Add(this.comboAptStatus);
			this.tabPageAppts.Controls.Add(this.labelAptStatus);
			this.tabPageAppts.Controls.Add(this.gridAppts);
			this.tabPageAppts.Location = new System.Drawing.Point(4, 22);
			this.tabPageAppts.Name = "tabPageAppts";
			this.tabPageAppts.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageAppts.Size = new System.Drawing.Size(803, 422);
			this.tabPageAppts.TabIndex = 0;
			this.tabPageAppts.Text = "Appointments";
			// 
			// comboAptStatus
			// 
			this.comboAptStatus.ArraySelectedIndices = new int[0];
			this.comboAptStatus.BackColor = System.Drawing.SystemColors.Window;
			this.comboAptStatus.Items = ((System.Collections.ArrayList)(resources.GetObject("comboAptStatus.Items")));
			this.comboAptStatus.Location = new System.Drawing.Point(125, 6);
			this.comboAptStatus.Name = "comboAptStatus";
			this.comboAptStatus.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboAptStatus.SelectedIndices")));
			this.comboAptStatus.Size = new System.Drawing.Size(181, 21);
			this.comboAptStatus.TabIndex = 84;
			this.comboAptStatus.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.combo_SelectionChangeCommitted);
			// 
			// labelAptStatus
			// 
			this.labelAptStatus.Location = new System.Drawing.Point(8, 10);
			this.labelAptStatus.Name = "labelAptStatus";
			this.labelAptStatus.Size = new System.Drawing.Size(115, 14);
			this.labelAptStatus.TabIndex = 83;
			this.labelAptStatus.Text = "Appointment Status";
			this.labelAptStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gridAppts
			// 
			this.gridAppts.AllowSortingByColumn = true;
			this.gridAppts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridAppts.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridAppts.ContextMenuStrip = this.menuApptsRightClick;
			this.gridAppts.HasAddButton = false;
			this.gridAppts.HasDropDowns = false;
			this.gridAppts.HasMultilineHeaders = false;
			this.gridAppts.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridAppts.HeaderHeight = 15;
			this.gridAppts.HScrollVisible = true;
			this.gridAppts.Location = new System.Drawing.Point(1, 33);
			this.gridAppts.Name = "gridAppts";
			this.gridAppts.ScrollValue = 0;
			this.gridAppts.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridAppts.Size = new System.Drawing.Size(796, 389);
			this.gridAppts.TabIndex = 8;
			this.gridAppts.Title = "Appointment ASAP List";
			this.gridAppts.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridAppts.TitleHeight = 18;
			this.gridAppts.TranslationName = "TableASAP";
			this.gridAppts.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAppts_CellDoubleClick);
			this.gridAppts.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridAppts_MouseUp);
			// 
			// tabPageRecalls
			// 
			this.tabPageRecalls.BackColor = System.Drawing.Color.Transparent;
			this.tabPageRecalls.Controls.Add(this.gridRecalls);
			this.tabPageRecalls.Controls.Add(this.textDateStart);
			this.tabPageRecalls.Controls.Add(this.label1);
			this.tabPageRecalls.Controls.Add(this.label2);
			this.tabPageRecalls.Controls.Add(this.comboNumberReminders);
			this.tabPageRecalls.Controls.Add(this.textDateEnd);
			this.tabPageRecalls.Controls.Add(this.label3);
			this.tabPageRecalls.Controls.Add(this.checkGroupFamilies);
			this.tabPageRecalls.Location = new System.Drawing.Point(4, 22);
			this.tabPageRecalls.Name = "tabPageRecalls";
			this.tabPageRecalls.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageRecalls.Size = new System.Drawing.Size(803, 422);
			this.tabPageRecalls.TabIndex = 1;
			this.tabPageRecalls.Text = "Recalls";
			// 
			// gridRecalls
			// 
			this.gridRecalls.AllowSortingByColumn = true;
			this.gridRecalls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridRecalls.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridRecalls.ContextMenuStrip = this.menuRecallsRightClick;
			this.gridRecalls.HasAddButton = false;
			this.gridRecalls.HasDropDowns = false;
			this.gridRecalls.HasMultilineHeaders = false;
			this.gridRecalls.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridRecalls.HeaderHeight = 15;
			this.gridRecalls.HScrollVisible = true;
			this.gridRecalls.Location = new System.Drawing.Point(1, 55);
			this.gridRecalls.Name = "gridRecalls";
			this.gridRecalls.ScrollValue = 0;
			this.gridRecalls.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridRecalls.Size = new System.Drawing.Size(801, 366);
			this.gridRecalls.TabIndex = 9;
			this.gridRecalls.Title = "Recall ASAP List";
			this.gridRecalls.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridRecalls.TitleHeight = 18;
			this.gridRecalls.TranslationName = "TableASAP";
			this.gridRecalls.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridRecalls_CellDoubleClick);
			// 
			// textDateStart
			// 
			this.textDateStart.Location = new System.Drawing.Point(91, 5);
			this.textDateStart.Name = "textDateStart";
			this.textDateStart.Size = new System.Drawing.Size(136, 20);
			this.textDateStart.TabIndex = 81;
			this.textDateStart.Leave += new System.EventHandler(this.combo_SelectionChangeCommitted);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 14);
			this.label1.TabIndex = 79;
			this.label1.Text = "Start Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(82, 14);
			this.label2.TabIndex = 80;
			this.label2.Text = "End Date";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboNumberReminders
			// 
			this.comboNumberReminders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboNumberReminders.Location = new System.Drawing.Point(354, 27);
			this.comboNumberReminders.MaxDropDownItems = 40;
			this.comboNumberReminders.Name = "comboNumberReminders";
			this.comboNumberReminders.Size = new System.Drawing.Size(82, 21);
			this.comboNumberReminders.TabIndex = 78;
			this.comboNumberReminders.SelectionChangeCommitted += new System.EventHandler(this.combo_SelectionChangeCommitted);
			// 
			// textDateEnd
			// 
			this.textDateEnd.Location = new System.Drawing.Point(91, 29);
			this.textDateEnd.Name = "textDateEnd";
			this.textDateEnd.Size = new System.Drawing.Size(136, 20);
			this.textDateEnd.TabIndex = 82;
			this.textDateEnd.Leave += new System.EventHandler(this.combo_SelectionChangeCommitted);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(233, 29);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(120, 14);
			this.label3.TabIndex = 77;
			this.label3.Text = "Show Reminders";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkGroupFamilies
			// 
			this.checkGroupFamilies.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkGroupFamilies.Location = new System.Drawing.Point(279, 5);
			this.checkGroupFamilies.Name = "checkGroupFamilies";
			this.checkGroupFamilies.Size = new System.Drawing.Size(157, 18);
			this.checkGroupFamilies.TabIndex = 74;
			this.checkGroupFamilies.Text = "Group Families";
			this.checkGroupFamilies.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkGroupFamilies.UseVisualStyleBackColor = true;
			this.checkGroupFamilies.CheckedChanged += new System.EventHandler(this.combo_SelectionChangeCommitted);
			// 
			// gridWebSched
			// 
			this.gridWebSched.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridWebSched.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridWebSched.ContextMenuStrip = this.menuApptsRightClick;
			this.gridWebSched.HasAddButton = false;
			this.gridWebSched.HasDropDowns = false;
			this.gridWebSched.HasMultilineHeaders = false;
			this.gridWebSched.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridWebSched.HeaderHeight = 15;
			this.gridWebSched.HScrollVisible = false;
			this.gridWebSched.Location = new System.Drawing.Point(5, 3);
			this.gridWebSched.Name = "gridWebSched";
			this.gridWebSched.ScrollValue = 0;
			this.gridWebSched.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridWebSched.Size = new System.Drawing.Size(796, 138);
			this.gridWebSched.TabIndex = 9;
			this.gridWebSched.TabStop = false;
			this.gridWebSched.Title = "Web Sched ASAP Messages";
			this.gridWebSched.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridWebSched.TitleHeight = 18;
			this.gridWebSched.TranslationName = "";
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSettings});
			// 
			// menuItemSettings
			// 
			this.menuItemSettings.Index = 0;
			this.menuItemSettings.Text = "Settings";
			this.menuItemSettings.Click += new System.EventHandler(this.menuItemSettings_Click);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(857, 38);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(87, 23);
			this.butRefresh.TabIndex = 71;
			this.butRefresh.Text = "&Refresh";
			this.butRefresh.UseVisualStyleBackColor = true;
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// FormASAP
			// 
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(980, 654);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.groupWebSched);
			this.Controls.Add(this.butText);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.comboSite);
			this.Controls.Add(this.labelSite);
			this.Controls.Add(this.comboProv);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.splitContainer);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Menu = this.mainMenu;
			this.MinimumSize = new System.Drawing.Size(882, 600);
			this.Name = "FormASAP";
			this.Text = "ASAP List";
			this.Load += new System.EventHandler(this.FormASAP_Load);
			this.menuApptsRightClick.ResumeLayout(false);
			this.menuRecallsRightClick.ResumeLayout(false);
			this.groupWebSched.ResumeLayout(false);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.tabControl.ResumeLayout(false);
			this.tabPageAppts.ResumeLayout(false);
			this.tabPageRecalls.ResumeLayout(false);
			this.tabPageRecalls.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormASAP_Load(object sender,System.EventArgs e) {
			Cursor=Cursors.WaitCursor;
			CheckClinicsSignedUpForWebSched();
			/*comboOrder.Items.Add(Lan.g(this,"Status"));
			comboOrder.Items.Add(Lan.g(this,"Alphabetical"));
			comboOrder.Items.Add(Lan.g(this,"Date"));
			comboOrder.SelectedIndex=0;*/
			comboProv.Items.Add(Lan.g(this,"All"));
			comboProv.SelectedIndex=0;
			_listProviders=Providers.GetDeepCopy(true);
			for(int i=0;i<_listProviders.Count;i++) {
				comboProv.Items.Add(_listProviders[i].GetLongDesc());
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
			splitContainer.Panel2Collapsed=true;
			if(PrefC.GetBool(PrefName.WebSchedAsapEnabled)) {
				if(_isSendingWebSched) {
					FillForWebSched();
				}
				else {//Not sending Web Sched ASAP
					butSendWebSched.Location=new Point(butSendWebSched.Location.X,labelOperatory.Location.Y+6);
					butWebSchedHist.Location=new Point(butWebSchedHist.Location.X,labelStart.Location.Y+9);
					groupWebSched.Height=butSendWebSched.Height+60;
				}
			}
			else {//Not signed up for Web Sched ASAP
				butSendWebSched.Location=new Point(butSendWebSched.Location.X,labelOperatory.Location.Y+2);
				butSendWebSched.Text=Lan.g(this,"Sign up");
				butWebSchedHist.Visible=false;
				groupWebSched.Height=butSendWebSched.Height+26;
			}
			ODBoxItem<ApptStatus> boxItem=new ODBoxItem<ApptStatus>(Lan.g(this,ApptStatus.Scheduled.ToString()),ApptStatus.Scheduled);
			comboAptStatus.Items.Add(boxItem);
			boxItem=new ODBoxItem<ApptStatus>(Lan.g(this,ApptStatus.Planned.ToString()),ApptStatus.Planned);
			comboAptStatus.Items.Add(boxItem);
			boxItem=new ODBoxItem<ApptStatus>(Lan.g(this,ApptStatus.UnschedList.ToString()),ApptStatus.UnschedList);
			comboAptStatus.Items.Add(boxItem);
			checkGroupFamilies.Checked=PrefC.GetBool(PrefName.RecallGroupByFamily);
			comboNumberReminders.Items.Add(Lan.g(this,"All"));
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
			FillGridAppts();
			FillTimeCombos();
			if(_isSendingWebSched) {
				FillWebSchedSent();
			}
			Cursor=Cursors.Default;
		}
		
		private void tabControl_SelectedIndexChanged(object sender,EventArgs e) {
			switch((sender as TabControl).SelectedIndex) {
				case 0:
				default:
					FillGridAppts();
					break;
				case 1:
					FillGridRecalls();
					break;
			}
		}

		#region Appointments Tab

		private void FillGridAppts(){
			this.Cursor=Cursors.WaitCursor;
			/*string order="";
			switch(comboOrder.SelectedIndex) {
				case 0:
					order="status";
					break;
				case 1:
					order="alph";
					break;
				case 2:
					order="date";
					break;
			}*/
			long provNum=0;
			if(comboProv.SelectedIndex!=0) {
				provNum=_listProviders[comboProv.SelectedIndex-1].ProvNum;
			}
			long siteNum=0;
			if(!PrefC.GetBool(PrefName.EasyHidePublicHealth) && comboSite.SelectedIndex!=0) {
				siteNum=_listSites[comboSite.SelectedIndex-1].SiteNum;
			}
			if(!SmsPhones.IsIntegratedTextingEnabled()) {
				butText.Enabled=false;
			}
			List<ApptStatus> listAppStatuses=comboAptStatus.ListSelectedItems.OfType<ODBoxItem<ApptStatus>>().Select(x => x.Tag).ToList();
			_listASAPs=Appointments.RefreshASAP(provNum,siteNum,_selectedClinicNum,listAppStatuses);
			int scrollVal=gridAppts.ScrollValue;
			gridAppts.BeginUpdate();
			gridAppts.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableASAP","Patient"),140);
			gridAppts.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableASAP","Date"),65);
			gridAppts.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableASAP","Unsched Status"),110);
			gridAppts.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableASAP","Apt Status"),110);
			gridAppts.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableASAP","Prov"),50);
			gridAppts.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableASAP","Procedures"),150);
			gridAppts.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableASAP","Length"),60);
			gridAppts.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableASAP","Notes"),200);
			gridAppts.Columns.Add(col);
			int widths=0;
			for(int i=0;i<gridAppts.Columns.Count;i++) {
				widths+=gridAppts.Columns[i].ColWidth;
			}
			if(widths > Width) {
				gridAppts.HScrollVisible=true;
			}
			gridAppts.Rows.Clear();
			ODGridRow row;
			_dictPatientNames=Patients.GetPatientNames(_listASAPs.Select(x => x.PatNum).ToList());
			for(int i=0;i<_listASAPs.Count;i++) {
				row=new ODGridRow();
				string patName=Lan.g(this,"UNKNOWN");
				_dictPatientNames.TryGetValue(_listASAPs[i].PatNum,out patName);
				row.Cells.Add(patName);
				if(_listASAPs[i].AptDateTime.Year < 1880){//shouldn't be possible.
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(_listASAPs[i].AptDateTime.ToShortDateString());
				}
				row.Cells.Add(Defs.GetName(DefCat.RecallUnschedStatus,_listASAPs[i].UnschedStatus));
				row.Cells.Add(_listASAPs[i].AptStatus.ToString());
				if(_listASAPs[i].IsHygiene) {
					row.Cells.Add(Providers.GetAbbr(_listASAPs[i].ProvHyg));
				}
				else {
					row.Cells.Add(Providers.GetAbbr(_listASAPs[i].ProvNum));
				}
				row.Cells.Add(_listASAPs[i].ProcDescript);
				row.Cells.Add(_listASAPs[i].Length.ToString());
				row.Cells.Add(_listASAPs[i].Note);
				row.Tag=_listASAPs[i];
				gridAppts.Rows.Add(row);
			}
			gridAppts.EndUpdate();
			gridAppts.ScrollValue=scrollVal;
			SetSelectedAppts();
			Cursor=Cursors.Default;
		}

		private void gridAppts_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			int currentSelection=e.Row;
			int currentScroll=gridAppts.ScrollValue;
			SelectedPatNum=((Appointment)gridAppts.Rows[e.Row].Tag).PatNum;	//check against grid
			Patient pat=Patients.GetPat(SelectedPatNum);
			FormOpenDental.S_Contr_PatientSelected(pat,true);
			FormApptEdit FormAE=new FormApptEdit(((Appointment)gridAppts.Rows[e.Row].Tag).AptNum);
			FormAE.PinIsVisible=true;
			FormAE.ShowDialog();
			if(FormAE.DialogResult!=DialogResult.OK){
				return;
			}
			if(FormAE.PinClicked) {
				SendPinboard_Click(); //Whatever they double clicked on will still be selected, just fire the event to send it to the pinboard.
				DialogResult=DialogResult.OK;
			}
			else {
				FillGridAppts();
				gridAppts.SetSelected(currentSelection,true);
				gridAppts.ScrollValue=currentScroll;
			}
		}

		private void gridAppts_MouseUp(object sender,MouseEventArgs e) {
			if(_isSendingWebSched) {
				FillWebSchedSent();
			}
		}

		private void RemoveAppts_Click() {
			if(!Security.IsAuthorized(Permissions.AppointmentEdit)) {
				return;
			}
			if(gridAppts.SelectedIndices.Length>0 && !MsgBox.Show(this,MsgBoxButtons.OKCancel,"Change priority to normal for all selected appointments?")) {
				return;
			}
			for(int i=0;i<gridAppts.SelectedIndices.Length;i++) {
				Appointment apt=(Appointment)gridAppts.SelectedGridRows[i].Tag;
				DateTime datePrevious=apt.DateTStamp;
				Appointments.SetPriority(apt,ApptPriority.Normal);
				SecurityLogs.MakeLogEntry(Permissions.AppointmentEdit,apt.PatNum,"Appointment priority set from ASAP to normal from ASAP list.",apt.AptNum,
					datePrevious);
			}
			FillGridAppts();
		}

		private void SendPinboard_Click() {
			if(gridAppts.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an appointment first.");
				return;
			}
			List<long> listAptSelected=new List<long>();
			Dictionary<long,Patient> dictPats=Patients.GetMultPats(_listSelectedAppts.Select(x => x.PatNum).ToList())
				.ToDictionary(x => x.PatNum,x => x);
			int patsArchivedOrDeceased=0;
			foreach(Appointment appt in _listSelectedAppts) {
				Patient pat;
				if(dictPats.TryGetValue(appt.PatNum,out pat) && pat.PatStatus.In(PatientStatus.Archived,PatientStatus.Deceased)) {
					patsArchivedOrDeceased++;
					continue;
				}
				if(appt.AptStatus==ApptStatus.Planned) {
					if(Procedures.GetProcsForSingle(appt.AptNum,true).Count(x => x.ProcStatus==ProcStat.C) > 0) {
						MsgBox.Show(this,"Not allowed to send a planned appointment to the pinboard if completed procedures are attached. Edit the planned "
							+"appointment first.");
						return;
					}
					Appointment nextApt=Appointments.GetScheduledPlannedApt(appt.AptNum);
					if(nextApt!=null) {
						if(nextApt.AptStatus==ApptStatus.Complete) {
							//Warn the user they are moving a completed appointment.
							if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"You are about to move an already completed appointment.  Continue?")) {
								return;
							}
						}
						else if(nextApt.AptStatus==ApptStatus.Scheduled) {
							//Warn the user they are moving an already scheduled appointment.
							if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"You are about to move an appointment already on the schedule.  Continue?")) {
								return;
							}
						}
					}
				}
				listAptSelected.Add(appt.AptNum);
			}
			List<string> listUserMsgs=new List<string>();
			if(patsArchivedOrDeceased > 0) {
				listUserMsgs.Add(Lan.g(this,"Appointments skipped because patient status is archived or deceased:")+" "+patsArchivedOrDeceased+".");
			}
			if(listAptSelected.Count==0) {
				listUserMsgs.Add(Lan.g(this,"There are no appointments to send to the pinboard."));
			}
			if(listUserMsgs.Count>0) {
				MessageBox.Show(string.Join("\r\n",listUserMsgs));
				if(listAptSelected.Count==0) {
					return;
				}
			}
			GotoModule.PinToAppt(listAptSelected,0); //Pins all appointments to the pinboard that were in listAptSelected.
		}
		#endregion Appointments Tab

		#region Recalls Tab

		private void FillGridRecalls() {
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
			if(comboProv.SelectedIndex!=0) {
				provNum=_listProviders[comboProv.SelectedIndex-1].ProvNum;
			}
			long siteNum=0;
			if(!PrefC.GetBool(PrefName.EasyHidePublicHealth) && comboSite.SelectedIndex!=0) {
				siteNum=_listSites[comboSite.SelectedIndex-1].SiteNum;
			}
			RecallListShowNumberReminders showReminders=(RecallListShowNumberReminders)comboNumberReminders.SelectedIndex;
			DataTable tableRecalls=Recalls.GetRecallListASAP(fromDate,toDate,checkGroupFamilies.Checked,provNum,_selectedClinicNum,siteNum,
				RecallListSort.DueDate,showReminders);
			List<Recall> listRecalls=Recalls.GetMultRecalls(tableRecalls.Rows.OfType<DataRow>().Select(x => PIn.Long(x["RecallNum"].ToString())).ToList());
			gridRecalls.BeginUpdate();
			gridRecalls.Columns.Clear();
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
				gridRecalls.Columns.Add(col);
			}
			gridRecalls.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<tableRecalls.Rows.Count;i++){
				row=new ODGridRow();
				for(int f=0;f<fields.Count;f++) {
					switch(fields[f].InternalName){
						case "Due Date":
							row.Cells.Add(tableRecalls.Rows[i]["dueDate"].ToString());
							break;
						case "Patient":
							row.Cells.Add(tableRecalls.Rows[i]["patientName"].ToString());
							break;
						case "Age":
							row.Cells.Add(tableRecalls.Rows[i]["age"].ToString());
							break;
						case "Type":
							row.Cells.Add(tableRecalls.Rows[i]["recallType"].ToString());
							break;
						case "Interval":
							row.Cells.Add(tableRecalls.Rows[i]["recallInterval"].ToString());
							break;
						case "#Remind":
							row.Cells.Add(tableRecalls.Rows[i]["numberOfReminders"].ToString());
							break;
						case "LastRemind":
							row.Cells.Add(tableRecalls.Rows[i]["dateLastReminder"].ToString());
							break;
						case "Contact":
							row.Cells.Add(tableRecalls.Rows[i]["contactMethod"].ToString());
							break;
						case "Status":
							row.Cells.Add(tableRecalls.Rows[i]["status"].ToString());
							break;
						case "Note":
							row.Cells.Add(tableRecalls.Rows[i]["Note"].ToString());
							break;
						case "BillingType":
							row.Cells.Add(tableRecalls.Rows[i]["billingType"].ToString());
							break;
						case "WebSched":
							row.Cells.Add(tableRecalls.Rows[i]["webSchedSendDesc"].ToString());
							break;
					}
				}
				row.Tag=listRecalls.FirstOrDefault(x => x.RecallNum==PIn.Long(tableRecalls.Rows[i]["RecallNum"].ToString()));
				gridRecalls.Rows.Add(row);
			}
			gridRecalls.EndUpdate();
			SetSelectedRecalls();
		}
		private void gridRecalls_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Recall recall=(Recall)gridRecalls.Rows[e.Row].Tag;
			Patient pat=Patients.GetPat(recall.PatNum);
			FormOpenDental.S_Contr_PatientSelected(pat,true);
			FormRecallEdit formRE=new FormRecallEdit();
			formRE.RecallCur=recall;
			formRE.ShowDialog();
			if(formRE.DialogResult!=DialogResult.OK) {
				return;
			}
			FillGridRecalls();
		}

		private void RemoveRecalls_Click() {
			if(gridRecalls.SelectedIndices.Length==0 
				|| !MsgBox.Show(this,MsgBoxButtons.OKCancel,"Change priority to normal for all selected recalls?")) 
			{
				return;
			}
			foreach(Recall recall in _listSelectedRecalls) { 
				recall.Priority=RecallPriority.Normal;
				Recalls.Update(recall);
				SecurityLogs.MakeLogEntry(Permissions.RecallEdit,recall.PatNum,"Recall priority set to Normal from the ASAP List.");
			}
			FillGridRecalls();
		}

		private void MakeAppointment_Click() {
			if(!Security.IsAuthorized(Permissions.AppointmentCreate) || gridRecalls.SelectedIndices.Length==0) {
				return;
			}
			foreach(Recall recall in _listSelectedRecalls) { 
				Patient patCur=Patients.GetPat(recall.PatNum);
				AppointmentL.PromptForMerge(patCur,out patCur);
				if(patCur.PatStatus.In(PatientStatus.Archived,PatientStatus.Deceased)) {
					MsgBox.Show(this,"Appointments cannot be scheduled for "+patCur.PatStatus.ToString().ToLower()+" patients.");
					return;
				}
				List<Procedure> procsList=Procedures.Refresh(patCur.PatNum);
				Family fam=Patients.GetFamily(patCur.PatNum);
				List<InsSub> subList=InsSubs.RefreshForFam(fam);
				List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
				Appointment aptCur=AppointmentL.CreateRecallApt(patCur,procsList,planList,recall.RecallNum,subList);
				GotoModule.PinToAppt(new List<long>() { aptCur.AptNum },patCur.PatNum);
			}
			FillGridRecalls();
		}
		#endregion Recalls Tab
		
		private void gridMenuRight_click(object sender,System.EventArgs e) {
			ToolStripMenuItem menu=(ToolStripMenuItem)sender;
			if(tabControl.SelectedIndex==0) {
				if(gridAppts.SelectedIndices.Length<=0) {
					return;
				}
				switch(menuApptsRightClick.Items.IndexOf(menu)) {
					case 0:
						SelectPatient_Click(((Appointment)gridAppts.SelectedGridRows.Last().Tag).PatNum);
						break;
					case 1:
						if(gridAppts.SelectedIndices.Length==0) {
							MsgBox.Show(this,"Please select an appointment first.");
							return;
						}
						SeeChart_Click(((Appointment)gridAppts.SelectedGridRows.Last().Tag).PatNum);
						break;
					case 2:
						SendPinboard_Click();
						break;
					case 3:
						RemoveAppts_Click();
						break;
				}
			}
			else {//Recall tab selected
				if(gridRecalls.SelectedIndices.Length<=0) {
					return;
				}
				Recall recall=_listSelectedRecalls.FirstOrDefault();
				switch(menuRecallsRightClick.Items.IndexOf(menu)) {
					case 0:
						SelectPatient_Click(recall.PatNum);
						break;
					case 1:
						if(gridRecalls.SelectedIndices.Length==0) {
							MsgBox.Show(this,"Please select a recall first first.");
							return;
						}
						SeeChart_Click(recall.PatNum);
						break;
					case 2:
						MakeAppointment_Click();
						break;
					case 3:
						RemoveRecalls_Click();
						break;
				}
			}
		}

		private void SelectPatient_Click(long patnum) {
			Patient pat=Patients.GetPat(patnum);//If multiple selected, just take the last one to remain consistent with SendPinboard_Click.
			FormOpenDental.S_Contr_PatientSelected(pat,true);
		}

		///<summary>If multiple patients are selected in the list, it will use the last patient to show the chart for.</summary>
		private void SeeChart_Click(long patnum) {
			Patient pat=Patients.GetPat(patnum); //If multiple selected, just use the last one.
			FormOpenDental.S_Contr_PatientSelected(pat,false); //Selects the patient in OpenDental.
			GotoModule.GotoChart(pat.PatNum);
		}

		private void combo_SelectionChangeCommitted(object sender,EventArgs e) {
			if(tabControl.SelectedIndex==0) {
				FillGridAppts();
			}
			else {
				FillGridRecalls();
			}
		}
		
		private void butText_Click(object sender,EventArgs e) {
			Clinic curClinic=Clinics.GetClinic(Clinics.ClinicNum)??Clinics.GetDefaultForTexting()??Clinics.GetPracticeAsClinicZero();
			List<PatComm> listPatCommsToSend;
			if(tabControl.SelectedIndex==0) {//Appt Tab selected
				Func<int,long> getPatNumFromGridRow=new Func<int,long>((rowIdx) =>	{
					return ((Appointment)gridAppts.Rows[rowIdx].Tag).PatNum;
				});
				listPatCommsToSend=GetPatCommsForTexting(curClinic,gridAppts,_listASAPs.Select(x => x.PatNum).ToList(),getPatNumFromGridRow);
			}
			else {//Recall tab selected
				List<Recall> listRecalls=new List<Recall>();
				foreach(ODGridRow row in gridRecalls.Rows) {
					listRecalls.Add((Recall)row.Tag);
				}
				Func<int,long> getPatNumFromGridRow=new Func<int,long>((rowIdx) =>	{
					return ((Recall)gridRecalls.Rows[rowIdx].Tag).PatNum;
				});
				listPatCommsToSend=GetPatCommsForTexting(curClinic,gridRecalls,listRecalls.Select(x => x.PatNum).ToList(),getPatNumFromGridRow);
			}
			if(listPatCommsToSend.Count==0) {
				return;
			}
			string textTemplate=GetTextMessageText(curClinic);
			FormTxtMsgMany FormTMM=new FormTxtMsgMany(listPatCommsToSend,textTemplate,curClinic.ClinicNum,SmsMessageSource.AsapManual);
			FormTMM.ShowDialog();
		}

		private List<PatComm> GetPatCommsForTexting(Clinic clinic,ODGrid grid,List<long> listPatNumsInGrid,Func<int,long> getPatNumFromGridRow) {
			List<PatComm> listPatComms=Patients.GetPatComms(listPatNumsInGrid,clinic,false);
			List<PatComm> listPatCommsToSend=new List<PatComm>();
			if(grid.SelectedIndices.Length==0) {//None selected. Select all that can be texted.
				for(int i=0;i<listPatNumsInGrid.Count;i++) {
					PatComm patCommForAppt=listPatComms.FirstOrDefault(x => x.PatNum==listPatNumsInGrid[i]);
					if(patCommForAppt==null || !patCommForAppt.IsSmsAnOption) {
						continue;
					}
					grid.SetSelected(i,true);
				}
				if(grid.SelectedIndices.Length==0) {
					MsgBox.Show(this,"None of the patients in the list are able to receive text messages.");
					return listPatCommsToSend;
				}
			}
			//deselect the ones that are not okay to send texts to.
			List<string> listPatsSkipped=new List<string>();
			int numRowsSelected=grid.SelectedIndices.Length;
			for(int i=grid.SelectedIndices.Length-1;i>=0;i--) {//Backwards since we are deselecting rows.
				PatComm patCommForAppt=listPatComms.FirstOrDefault(x => x.PatNum==getPatNumFromGridRow(grid.SelectedIndices[i]));
				if(patCommForAppt!=null && patCommForAppt.IsSmsAnOption) {
					listPatCommsToSend.Add(patCommForAppt);
					continue;
				}
				//Cannot send text message to this patient
				if(patCommForAppt==null) {//Shouldn't happen
					listPatsSkipped.Add(Lan.g(this,"Unknown patient")+": "+Lan.g(this,"Cannot find contact info"));
				}
				else if(string.IsNullOrEmpty(patCommForAppt.SmsPhone)) {
					listPatsSkipped.Add(patCommForAppt.FName+" "+patCommForAppt.LName+": "+Lan.g(this,"No wireless phone number"));
				}
				else {
					listPatsSkipped.Add(patCommForAppt.FName+" "+patCommForAppt.LName+": "+Lan.g(this,"Marked to not receive text messages"));
				}
				grid.SetSelected(grid.SelectedIndices[i],false);
			}
			if(listPatsSkipped.Count > 0) {
				MessageBox.Show(listPatsSkipped.Count+" "+Lan.g(this,"of the")+" "+numRowsSelected+" "
					+Lan.g(this,"selected patients cannot receive text messages and have been deselected:")+"\r\n"+string.Join("\r\n",listPatsSkipped));
			}
			return listPatCommsToSend;
		}		

		///<summary>Gets the template for this clinic and fills in the tags.</summary>
		private string GetTextMessageText(Clinic curClinic) {
			string textTemplate;
			ClinicPref clinicPref=ClinicPrefs.GetPref(PrefName.ASAPTextTemplate,Clinics.ClinicNum);
			if(clinicPref==null) {
				textTemplate=PrefC.GetString(PrefName.ASAPTextTemplate);
			}
			else {
				textTemplate=clinicPref.ValueString;
			}
			textTemplate=textTemplate.Replace("[OfficeName]",curClinic.Description);
			textTemplate=textTemplate.Replace("[OfficePhone]",curClinic.Phone);
			if(_dateTimeChosen.Year > 1880) {//The user clicked on the appt schedule to get here.
				textTemplate=textTemplate.Replace("[Date]",_dateTimeChosen.ToString(PrefC.PatientCommunicationDateFormat));
				textTemplate=textTemplate.Replace("[Time]",_dateTimeChosen.ToShortTimeString());
				return textTemplate;
			}
			//Need to use an input box to specify the date and time.
			List<InputBoxParam> listInputBoxParams=new List<InputBoxParam>();
			if(textTemplate.Contains("[Date]")) {
				listInputBoxParams.Add(new InputBoxParam(InputBoxType.ValidDate,"Enter date for available time"));
			}
			if(textTemplate.Contains("[Time]")) {
				listInputBoxParams.Add(new InputBoxParam(InputBoxType.ValidTime,"Enter time for available time"));
			}
			InputBox inputBox=new InputBox(listInputBoxParams);
			if(listInputBoxParams.Count > 0 && inputBox.ShowDialog()==DialogResult.OK) {
				if(inputBox.DateEntered.Year > 1880) {
					textTemplate=textTemplate.Replace("[Date]",inputBox.DateEntered.ToString(PrefC.PatientCommunicationDateFormat));
				}
				if(textTemplate.Contains("[Time]") && inputBox.ListTextEntered[listInputBoxParams.Count-1]!="") {//A time was entered.
					textTemplate=textTemplate.Replace("[Time]",inputBox.TimeEntered.ToShortTimeString());
				}
			}
			return textTemplate;
		}

		#region Web Sched

		///<summary>Use this instead of Show() or ShowDialog() in order to send Web Sched messages.</summary>
		public void ShowFormForWebSched(DateTime dateTimeChosen,DateTime dateTimeSlotStart,DateTime dateTimeSlotEnd,long opNum) {
			_dateTimeChosen=dateTimeChosen;
			_dateTimeSlotStart=dateTimeSlotStart;
			_dateTimeSlotEnd=dateTimeSlotEnd;
			_opNum=opNum;
			Show();
			if(WindowState==FormWindowState.Minimized) {
				WindowState=FormWindowState.Normal;
			}
			BringToFront();
			_selectedClinicNum=ODMethodsT.Coalesce(Operatories.GetOperatory(_opNum)).ClinicNum;
			FillGridAppts();
			FillTimeCombos();
		}

		private void FillForWebSched() {
			if(_dateTimeChosen.Date < DateTimeOD.Today) {
				labelOperatory.Text=Lan.g(this,"Cannot send for a past time slot.");
				labelOperatory.Visible=true;
				_isSendingWebSched=false;
				return;
			}
			_selectedClinicNum=ODMethodsT.Coalesce(Operatories.GetOperatory(_opNum)).ClinicNum;
			comboClinic.Enabled=false;//We only want them to choose appointments from the clinic of the operatory selected.
			labelOperatory.Text=Lan.g(this,"Operatory:")+" "+Operatories.GetOperatory(_opNum).Abbrev;
			splitContainer.Panel2Collapsed=false;
			labelOperatory.Visible=true;
			labelStart.Visible=true;
			comboStart.Visible=true;
			labelEnd.Visible=true;
			comboEnd.Visible=true;
		}

		private void FillTimeCombos() {
			if(!_isSendingWebSched) {
				return;
			}
			int timeIncrement=PrefC.GetInt(PrefName.AppointmentTimeIncrement);
			comboStart.Items.Clear();
			for(DateTime time=_dateTimeSlotStart;time<_dateTimeSlotEnd;time=time.AddMinutes(timeIncrement)) {
				int addedIdx=comboStart.Items.Add(new ComboTimeItem(time));
				if(time==_dateTimeChosen) {
					comboStart.SelectedIndex=addedIdx;
				}
			}
			FillComboEnd();
		}

		private void FillComboEnd() {
			int timeIncrement=PrefC.GetInt(PrefName.AppointmentTimeIncrement);
			DateTime selectedTimeEnd=_selectedTimeEnd;
			comboEnd.Items.Clear();
			DateTime firstTime=ODMathLib.Max(_selectedTimeStart,DateTime.Today).AddMinutes(timeIncrement);
			for(DateTime time=firstTime;time<=_dateTimeSlotEnd;time=time.AddMinutes(timeIncrement)) {
				int idx=comboEnd.Items.Add(new ComboTimeItem(time));
				if(selectedTimeEnd==time) {
					comboEnd.SelectedIndex=idx;
				}
			}
			if(comboEnd.SelectedIndex==-1) {
				//Put the end time one hour after the start time or the last available time.
				int idx1HourAfterStart=60/timeIncrement-1;
				comboEnd.SelectedIndex=Math.Min(idx1HourAfterStart,comboEnd.Items.Count-1);
			}
		}

		private void comboStart_SelectedIndexChanged(object sender,EventArgs e) {
			FillComboEnd();
			SetSelectedAppts();
			SetSelectedRecalls();
		}

		private void comboEnd_SelectionChangeCommitted(object sender,EventArgs e) {
			SetSelectedAppts();
			SetSelectedRecalls();
		}

		private void FillWebSchedSent() {
			List<long> listPatNumsSelected=_listSelectedAppts.Select(x => x.PatNum).Distinct().ToList();
			listPatNumsSelected.AddRange(_listSelectedRecalls.Select(x => x.PatNum));
			Func<AsapComms.AsapCommHist,DateTime> funcOrderBy=new Func<AsapComms.AsapCommHist,DateTime>(x => {
				DateTime smsSent=DateTime.MinValue;
				if(x.AsapComm.SmsSendStatus==AutoCommStatus.SendSuccessful) {
					smsSent=x.AsapComm.DateTimeSmsSent;
				}
				else if(x.AsapComm.SmsSendStatus==AutoCommStatus.SendNotAttempted) {
					smsSent=x.AsapComm.DateTimeSmsScheduled;
				}
				return smsSent;
			});
			List<AsapComms.AsapCommHist> listAsapHists=AsapComms.GetHist(DateTimeOD.Today.AddMonths(-1),DateTimeOD.Today,listPatNumsSelected)
				.OrderByDescending(funcOrderBy)
				.ThenBy(x => _dictPatientNames[x.AsapComm.PatNum]).ToList();
			gridWebSched.BeginUpdate();
			gridWebSched.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"Patient"),150);
			gridWebSched.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Text Send Time"),120);
			gridWebSched.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Email Send Time"),120);
			gridWebSched.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Time Slot Start"),120);
			gridWebSched.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Notes"),300);
			gridWebSched.Columns.Add(col);
			gridWebSched.Rows.Clear();
			foreach(AsapComms.AsapCommHist asapCommHist in listAsapHists) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(_dictPatientNames[asapCommHist.AsapComm.PatNum]);
				string smsSent;
				if(asapCommHist.AsapComm.SmsSendStatus==AutoCommStatus.SendSuccessful) {
					smsSent=asapCommHist.AsapComm.DateTimeSmsSent.ToString();
				}
				else if(asapCommHist.AsapComm.SmsSendStatus==AutoCommStatus.SendNotAttempted) {
					smsSent=asapCommHist.AsapComm.DateTimeSmsScheduled.ToString();
				}
				else {
					smsSent="";
				}
				row.Cells.Add(smsSent);
				string emailSent;
				if(asapCommHist.AsapComm.EmailSendStatus==AutoCommStatus.SendSuccessful) {
					emailSent=asapCommHist.AsapComm.DateTimeEmailSent.ToString();
				}
				else {
					emailSent="";
				}
				row.Cells.Add(emailSent);
				row.Cells.Add(asapCommHist.DateTimeSlotStart.ToString());
				row.Cells.Add(asapCommHist.AsapComm.Note);
				row.Tag=asapCommHist;
				gridWebSched.Rows.Add(row);
			}
			gridWebSched.EndUpdate();
		}

		private void SetSelectedAppts() {
			if(!_isSendingWebSched) {
				return;
			}
			int slotLength=(int)(_selectedTimeEnd-_selectedTimeStart).TotalMinutes;
			for(int i=0;i<gridAppts.Rows.Count;i++) {
				if(((Appointment)gridAppts.Rows[i].Tag).Length<=slotLength) {
					gridAppts.SetSelected(i,true);
				}
				else {
					gridAppts.SetSelected(i,false);
				}
			}
		}

		private void SetSelectedRecalls() {
			if(!_isSendingWebSched) {
				return;
			}
			int timeIncrements=PrefC.GetInt(PrefName.AppointmentTimeIncrement);
			int slotLength=(int)(_selectedTimeEnd-_selectedTimeStart).TotalMinutes;
			for(int i=0;i<gridRecalls.Rows.Count;i++) {
				Recall recallCur=gridRecalls.Rows[i].Tag as Recall;
				string timePattern=RecallTypes.GetTimePattern(recallCur.RecallTypeNum);
				int length=timePattern.Length*timeIncrements;
				if(length<=slotLength) {
					gridRecalls.SetSelected(i,true);
				}
			}
		}

		private void CheckClinicsSignedUpForWebSched() {
			if(_threadWebSchedSignups!=null) {
				return;
			}
			_threadWebSchedSignups=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				_listClinicNumsWebSched=WebServiceMainHQProxy.GetEServiceClinicsAllowed(
					Clinics.GetDeepCopy().Select(x => x.ClinicNum).ToList(),
					eServiceCode.WebSchedASAP);
				bool isAllowedByHq=(_listClinicNumsWebSched.Count > 0);
				if(isAllowedByHq!=PrefC.GetBool(PrefName.WebSchedAsapEnabled)) {
					Prefs.UpdateBool(PrefName.WebSchedAsapEnabled,isAllowedByHq);
					DataValid.SetInvalid(InvalidType.Prefs);
				}
			}));
			//Swallow all exceptions and allow thread to exit gracefully.
			_threadWebSchedSignups.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception ex) => { }));
			_threadWebSchedSignups.AddExitHandler(new ODThread.WorkerDelegate((ODThread o) => {
				try {
					ThreadWebSchedSignupsExitHandler();
				}
				catch(Exception ex) {
					ex.DoNothing();
				}
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
					FriendlyException.Show(Lan.g(this,"Error sending Web Sched messages."),ex);
				}
			}
			_hasClickedWebSched=false;
		}

		///<summary>Automatically open the eService Setup window so that they can easily click the Enable button. 
		///Calls CheckClinicsSignedUpForWebSched() before exiting.</summary>
		private void OpenSignupPortal() {
			FormEServicesSetup FormESS=new FormEServicesSetup(FormEServicesSetup.EService.SignupPortal);
			FormESS.ShowDialog();
			//User may have made changes to signups. Reload the valid clinics from HQ.
			CheckClinicsSignedUpForWebSched();
		}

		private void butWebSched_Click(object sender,EventArgs e) {
			SendWebSched();
		}

		private void SendWebSched() {
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
					"No clinics are signed up for Web Sched ASAP. Open Sign Up Portal?" :
					"This practice is not signed up for Web Sched ASAP. Open Sign Up Portal?";
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,message)) {
					return;
				}
				OpenSignupPortal();
				return;
			}
			//At least one clinic is signed up for Web Sched.
			if(!_isSendingWebSched) {//Did not get to this window by right-clicking in the Appointment module.
				MsgBox.Show(this,
					"To use this feature, right-click on the appointment schedule where no appointment is scheduled and select \"Text ASAP List\".");
				return;
			}
			if(gridAppts.SelectedIndices.Length==0 && gridRecalls.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select at least one patient from the appointment or recall list.");
				return;
			}
			if(comboStart.SelectedIndex==-1|| comboEnd.SelectedIndex==-1) {
				MsgBox.Show(this,"Please select a valid start and end time.");
				return;
			}
			long clinicNum=ODMethodsT.Coalesce(Operatories.GetOperatory(_opNum)).ClinicNum;
			if(!clinicNum.In(_listClinicNumsWebSched)) {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,
					"The clinic the selected operatory belongs to is not signed up for Web Sched ASAP. Open Sign Up Portal?")) 
				{
					return;
				}
				OpenSignupPortal();
				return;
			}
			List<Appointment> listAppts=new List<Appointment>();
			List<Recall> listRecalls=new List<Recall>();
			if(tabControl.SelectedIndex==0) {
				listAppts=_listSelectedAppts;
			}
			else {
				listRecalls=_listSelectedRecalls;
			}
			FormWebSchedASAPSend FormWSAS=new FormWebSchedASAPSend(clinicNum,_opNum,_selectedTimeStart,_selectedTimeEnd,_listSelectedAppts,_listSelectedRecalls);
			FormWSAS.ShowDialog();
		}

		private void butWebSchedHist_Click(object sender,EventArgs e) {
			if(_formWebSchedHist==null || _formWebSchedHist.IsDisposed) {
				_formWebSchedHist=new FormWebSchedASAPHistory();
			}
			_formWebSchedHist.Show();
			if(_formWebSchedHist.WindowState==FormWindowState.Minimized) {
				_formWebSchedHist.WindowState=FormWindowState.Normal;
			}
			_formWebSchedHist.BringToFront();
		}

		private class ComboTimeItem {
			public DateTime Time;
			public ComboTimeItem(DateTime time) {
				Time=time;
			}
			public override string ToString() {
				return Time.ToShortTimeString();
			}
		}

		#endregion Web Sched

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGridAppts();
			FillGridRecalls();
		}

		private void menuItemSettings_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormAsapSetup FormAS=new FormAsapSetup();
			FormAS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"ASAP List Setup");
		}

		private void butPrint_Click(object sender,EventArgs e) {
			pagesPrinted=0;
			pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			//pd.OriginAtMargins=true;
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			headingPrinted=false;
			#if DEBUG
				FormRpPrintPreview pView = new FormRpPrintPreview();
				pView.printPreviewControl2.Document=pd;
				pView.ShowDialog();
			#else
				if(!PrinterL.SetPrinter(pd,PrintSituation.Default,0,"ASAP list printed")) {
					return;
				}
				try{
					pd.Print();
				}
				catch(Exception ex){
					MessageBox.Show(ex.Message);
				}
			#endif			
		}

		private void pd_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			//new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int y=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			int headingPrintH=0;
			if(!headingPrinted) {
				text=Lan.g(this,"ASAP List");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,y);
				//yPos+=(int)g.MeasureString(text,headingFont).Height;
				//text=textDateFrom.Text+" "+Lan.g(this,"to")+" "+textDateTo.Text;
				//g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				y+=25;
				headingPrinted=true;
				headingPrintH=y;
			}
			#endregion
			y=gridAppts.PrintPage(g,pagesPrinted,bounds,headingPrintH);
			pagesPrinted++;
			if(y==-1) {
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
