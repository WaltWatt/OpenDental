/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.HL7;
using OpenDentBusiness.UI;

namespace OpenDental {

	///<summary></summary>
	public class ContrAppt:System.Windows.Forms.UserControl {
		private OpenDental.ContrApptSheet ContrApptSheet2;
		private System.Windows.Forms.MonthCalendar Calendar2;
		private System.Windows.Forms.Label labelDate;
		private System.Windows.Forms.Label labelDate2;
		private System.ComponentModel.IContainer components;// Required designer variable.
		private bool mouseIsDown=false;
		///<summary>The point where the mouse was originally down.  In Appt Sheet coordinates</summary>
		private Point mouseOrigin = new Point();
		///<summary>Control origin.  If moving an appointment, this is the location where the appointment was at the beginning of the drag.</summary>
		private Point contOrigin = new Point();
		///<summary>Imported that this is constructed here. Use ContrApptSingle.ResetData() to set it's data fields.</summary>
		private ContrApptSingle TempApptSingle=new ContrApptSingle();
		private System.Windows.Forms.ImageList imageListMain;
		private bool boolAptMoved=false;
		private OpenDental.UI.Button butToday;
		private System.Windows.Forms.Panel panelSheet;
		private System.Windows.Forms.Panel panelCalendar;
		private System.Windows.Forms.Panel panelArrows;
		private System.Windows.Forms.VScrollBar vScrollBar1;
		private System.Windows.Forms.Panel panelOps;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ListBox listConfirmed;
		private System.Windows.Forms.Button butComplete;
		private System.Windows.Forms.Button butUnsched;
		private System.Windows.Forms.Button butDelete;
		private System.Windows.Forms.Button butBreak;
		///<summary>The actual operatoryNum of the clicked op.</summary>
		public static long SheetClickedonOp;
		///<summary></summary>
		public static int SheetClickedonHour;
		///<summary>The exact minute the user clicked on within the hour.  E.g. 58</summary>
		public static int SheetClickedonMin;
		private System.Drawing.Printing.PrintDocument pd2;
		private OpenDental.UI.Button butBack;
		private OpenDental.UI.Button butClearPin;
		private OpenDental.UI.Button butFwd;
		private System.Windows.Forms.Panel panelAptInfo;
		private System.Windows.Forms.Label label2;
		private OpenDental.UI.ODToolBar ToolBarMain;
		private System.Windows.Forms.TextBox textLab;
		private System.Windows.Forms.TextBox textProduction;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox comboView;
		private System.Windows.Forms.ContextMenu menuPatient;
		///<summary></summary>
		public FormRpPrintPreview pView;
		private OpenDental.UI.Button butMakeAppt;
		private bool cardPrintFamily;
		private System.Windows.Forms.ContextMenu menuApt;
		private MenuItem menuItemBreakAppt;
		private System.Windows.Forms.ContextMenu menuBlockout;
		private System.Windows.Forms.ContextMenu menuWeeklyApt;
		private List<Schedule> SchedListPeriod;
		private OpenDental.UI.Button butSearch;
		private System.Windows.Forms.GroupBox groupSearch;
		private OpenDental.UI.Button butSearchNext;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox textBefore;
		private System.Windows.Forms.RadioButton radioBeforeAM;
		private System.Windows.Forms.RadioButton radioBeforePM;
		private System.Windows.Forms.RadioButton radioAfterPM;
		private System.Windows.Forms.RadioButton radioAfterAM;
		private System.Windows.Forms.TextBox textAfter;
		private System.Windows.Forms.Label label11;
		private OpenDental.UI.Button butSearchClose;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button butSearchCloseX;
		private System.Windows.Forms.ListBox listProviders;
		private System.Windows.Forms.ListBox listSearchResults;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.DateTimePicker dateSearch;
		private List<DateTime> SearchResults;
		private OpenDental.UI.Button butRefresh;
		private bool ResizingAppt;
		private int ResizingOrigH;
		//private bool isWeeklyView;
		public static DateTime WeekStartDate;
		public static DateTime WeekEndDate;
		private OpenDental.UI.Button butLab;
		///<summary>The index of the day as shown on the screen.  Only used in weekly view.</summary>
		public static int SheetClickedonDay;
		///<summary></summary>
		private Panel infoBubble;
		///<summary>If the user has done a blockout/copy, then this will contain the blockout that is on the "clipboard".</summary>
		private Schedule BlockoutClipboard;
		///<summary>This has to be tracked globally because mouse might move directly from one appt to another without any break.  This is the only way to know if we are still over the same appt.</summary>
		private long bubbleAptNum;
		private DateTime bubbleTime;
		private Point bubbleLocation;
		private ODGrid gridEmpSched;
		private OpenDental.UI.ODPictureBox PicturePat;
		//private string PatCurName;
		//private int PatCurNum;
		private Timer timerInfoBubble;
		//private string PatCurChartNumber;
		private TabControl tabControl;
		private TabPage tabWaiting;
		private TabPage tabSched;
		private ODGrid gridWaiting;
		private OpenDental.UI.Button butMonth;
		//<summary></summary>
		//public static Size PinboardSize=new Size(106,92);
		private PinBoard pinBoard;
		//private ContrApptSingle PinApptSingle;
		///<summary>Local computer time.  Used by waiting room feature as delta time for display refresh.</summary>
		private DateTime LastTimeDataRetrieved;
		private Timer timerWaitingRoom;
		private OpenDental.UI.Button butBackMonth;
		private OpenDental.UI.Button butFwdMonth;
		private OpenDental.UI.Button butBackWeek;
		private OpenDental.UI.Button butFwdWeek;
		private RadioButton radioDay;
		private RadioButton radioWeek;
		private bool InitializedOnStartup;
		public Patient PatCur;
		private FormRecallList FormRecallL;
		private FormASAP _formASAP;
		private FormConfirmList FormConfirmL;
		private OpenDental.UI.Button butGraph;
		private Timer timerTests;
		//private int stressCounter;
		///<summary>When a popup happens durring attempted drag off pinboard, this helps cancel the drag.</summary>
		private bool CancelPinMouseDown;
		private DateTime apptPrintStartTime;
		private DateTime apptPrintStopTime;
		private int apptPrintFontSize;
		private UI.Button butProvPick;
		private int apptPrintColsPerPage;
		private GroupBox groupBox1;
		private UI.Button butProvHygenist;
		private UI.Button butProvDentist;
		public List<Provider> ProviderList;
		private int pagesPrinted;
		private int pageRow;
		private int pageColumn;
		private UI.Button butFamRecall;
		private UI.Button butViewAppts;
		private UI.Button butMakeRecall;
		private Panel panelMakeButtons;
		private List<DisplayField> _aptBubbleDefs;
		///<summary>This is a list of ApptViews that are available in comboView, which will be filtered for the currently selected clinic if clincs are
		///enabled.  This list will contain the same number of items as comboView minus 1 for 'none' and is filled at the same time as comboView.
		///Use this list when accessing the view by comboView.SelectedIndex.</summary>
		private List<ApptView> _listApptViews;
		private FormTrackNext FormTN;
		private TabPage tabProv;
		private ODGrid gridProv;
		private FormUnsched FormUnsched2;
		///<summary></summary>
		[Category("Data"),Description("Occurs when a user has taken action on an item needing an action taken.")]
		public event ActionNeededEventHandler ActionTaken=null;
		private bool _isPrintPreview;
		private Label labelNoneView;
		private TabPage tabReminders;
		private ODGrid gridReminders;
		private ImageList imageListTasks;
		private ContextMenu menuReminderEdit;
		private MenuItem menuItemReminderDone;
		private MenuItem menuItemReminderGoto;
		private List<OpPanel> _listOpPanels;
		private ContextMenuStrip _menuOp;
		private TextBox textProdGoal;
		private Label labelProdGoal;

		///<summary>Used to determine whether the scrollbar position needs to be set.</summary>
		private bool _hasLayedOutScrollBar=false;

		///<summary>The appointment table. Refreshed in RefreshAppointmentsIfNeeded.</summary>
		private DataTable _dtAppointments;
		///<summary>The employee schedule table. Refreshed in RefreshSchedulesIfNeeded.</summary>
		private DataTable _dtEmpSched;
		///<summary>The provider schedule table. Refreshed in RefreshSchedulesIfNeeded.</summary>
		private DataTable _dtProvSched;
		///<summary>The schedule table. Refreshed in RefreshSchedulesIfNeeded.</summary>
		private DataTable _dtSchedule;
		///<summary>The waiting room table. Refreshed in RefreshWaitingRoomTable.</summary>
		private DataTable _dtWaitingRoom;
		///<summary>The appointment fields table. Refreshed in RefreshAppointmentsIfNeeded.</summary>
		private DataTable _dtApptFields;
		///<summary>The patient fields table. Refreshed in RefreshAppointmentsIfNeeded.</summary>
		private DataTable _dtPatFields;

		///<summary></summary>
		public ContrAppt() {
			Logger.openlog.Log("Initializing appointment module...",Logger.Severity.INFO);
			InitializeComponent();// This call is required by the Windows.Forms Form Designer.
			menuWeeklyApt=new System.Windows.Forms.ContextMenu();
			infoBubble=new Panel();
			infoBubble.Visible=false;
			infoBubble.Size=new Size(200,300);
			infoBubble.MouseMove+=new MouseEventHandler(InfoBubble_MouseMove);
			infoBubble.BackColor=Color.FromArgb(255,250,190);
			PicturePat=new OpenDental.UI.ODPictureBox();
			PicturePat.Location=new Point(6,17);
			PicturePat.Size=new Size(100,100);
			PicturePat.BackColor=Color.FromArgb(232,220,190);
			PicturePat.TextNullImage=Lan.g(this,"Patient Picture Unavailable");
			PicturePat.MouseMove+=new MouseEventHandler(PicturePat_MouseMove);
			infoBubble.Controls.Clear();
			infoBubble.Controls.Add(PicturePat);
			this.Controls.Add(infoBubble);
			ContrApptSheet2.MouseWheel+=new MouseEventHandler(ContrApptSheet2_MouseWheel);
			_listApptViews=new List<ApptView>();
			_listOpPanels=new List<OpPanel>();
			Lan.C(this,menuReminderEdit);
			gridReminders.ContextMenu=menuReminderEdit;
			//Add this control once. We will use ResetData() and Visible=true/false to control its visibility and layout.
			Controls.Add(TempApptSingle);
		}

		///<summary></summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components != null) {
					components.Dispose();
				}
				for(int i=0;i<pinBoard.ApptList.Count;i++) {
					ContrApptSingle row=pinBoard.ApptList[i];
					if(row.AptStatus==ApptStatus.UnschedList && row.AptDateTime.Year<1880) {
						Appointment aptCur=Appointments.GetOneApt(row.AptNum);
						if(aptCur.AptDateTime.Year<1880) {//if the date was not updated since put on the pinboard
							Appointments.Delete(aptCur.AptNum,true);
							string logText=Lan.g(this,"Deleted from pinboard while closing Open Dental")+": ";
							if(aptCur.AptDateTime.Year>1880) {
								logText+=aptCur.AptDateTime.ToString()+", ";
							}
							logText+=aptCur.ProcDescript;
							SecurityLogs.MakeLogEntry(Permissions.AppointmentEdit,aptCur.PatNum,logText);
						}
					}
				}
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContrAppt));
			this.imageListMain = new System.Windows.Forms.ImageList(this.components);
			this.Calendar2 = new System.Windows.Forms.MonthCalendar();
			this.labelDate = new System.Windows.Forms.Label();
			this.labelDate2 = new System.Windows.Forms.Label();
			this.panelArrows = new System.Windows.Forms.Panel();
			this.butBackMonth = new OpenDental.UI.Button();
			this.butFwdMonth = new OpenDental.UI.Button();
			this.butBackWeek = new OpenDental.UI.Button();
			this.butFwdWeek = new OpenDental.UI.Button();
			this.butToday = new OpenDental.UI.Button();
			this.butBack = new OpenDental.UI.Button();
			this.butFwd = new OpenDental.UI.Button();
			this.panelSheet = new System.Windows.Forms.Panel();
			this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
			this.ContrApptSheet2 = new OpenDental.ContrApptSheet();
			this.labelNoneView = new System.Windows.Forms.Label();
			this.panelAptInfo = new System.Windows.Forms.Panel();
			this.listConfirmed = new System.Windows.Forms.ListBox();
			this.butComplete = new System.Windows.Forms.Button();
			this.butUnsched = new System.Windows.Forms.Button();
			this.butDelete = new System.Windows.Forms.Button();
			this.butBreak = new System.Windows.Forms.Button();
			this.panelCalendar = new System.Windows.Forms.Panel();
			this.radioWeek = new System.Windows.Forms.RadioButton();
			this.radioDay = new System.Windows.Forms.RadioButton();
			this.butGraph = new OpenDental.UI.Button();
			this.butMonth = new OpenDental.UI.Button();
			this.pinBoard = new OpenDental.UI.PinBoard();
			this.butLab = new OpenDental.UI.Button();
			this.butSearch = new OpenDental.UI.Button();
			this.textProduction = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.textLab = new System.Windows.Forms.TextBox();
			this.comboView = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.butClearPin = new OpenDental.UI.Button();
			this.panelOps = new System.Windows.Forms.Panel();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.pd2 = new System.Drawing.Printing.PrintDocument();
			this.menuApt = new System.Windows.Forms.ContextMenu();
			this.menuPatient = new System.Windows.Forms.ContextMenu();
			this.menuBlockout = new System.Windows.Forms.ContextMenu();
			this.groupSearch = new System.Windows.Forms.GroupBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butProvHygenist = new OpenDental.UI.Button();
			this.butProvDentist = new OpenDental.UI.Button();
			this.butProvPick = new OpenDental.UI.Button();
			this.butRefresh = new OpenDental.UI.Button();
			this.listSearchResults = new System.Windows.Forms.ListBox();
			this.listProviders = new System.Windows.Forms.ListBox();
			this.butSearchClose = new OpenDental.UI.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.textAfter = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.radioBeforePM = new System.Windows.Forms.RadioButton();
			this.radioBeforeAM = new System.Windows.Forms.RadioButton();
			this.textBefore = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.radioAfterAM = new System.Windows.Forms.RadioButton();
			this.radioAfterPM = new System.Windows.Forms.RadioButton();
			this.dateSearch = new System.Windows.Forms.DateTimePicker();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.butSearchCloseX = new System.Windows.Forms.Button();
			this.butSearchNext = new OpenDental.UI.Button();
			this.timerInfoBubble = new System.Windows.Forms.Timer(this.components);
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabWaiting = new System.Windows.Forms.TabPage();
			this.gridWaiting = new OpenDental.UI.ODGrid();
			this.tabSched = new System.Windows.Forms.TabPage();
			this.gridEmpSched = new OpenDental.UI.ODGrid();
			this.tabProv = new System.Windows.Forms.TabPage();
			this.gridProv = new OpenDental.UI.ODGrid();
			this.tabReminders = new System.Windows.Forms.TabPage();
			this.gridReminders = new OpenDental.UI.ODGrid();
			this.timerWaitingRoom = new System.Windows.Forms.Timer(this.components);
			this.timerTests = new System.Windows.Forms.Timer(this.components);
			this.panelMakeButtons = new System.Windows.Forms.Panel();
			this.butMakeAppt = new OpenDental.UI.Button();
			this.butFamRecall = new OpenDental.UI.Button();
			this.butMakeRecall = new OpenDental.UI.Button();
			this.butViewAppts = new OpenDental.UI.Button();
			this.imageListTasks = new System.Windows.Forms.ImageList(this.components);
			this.ToolBarMain = new OpenDental.UI.ODToolBar();
			this.menuReminderEdit = new System.Windows.Forms.ContextMenu();
			this.menuItemReminderDone = new System.Windows.Forms.MenuItem();
			this.menuItemReminderGoto = new System.Windows.Forms.MenuItem();
			this._menuOp = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.textProdGoal = new System.Windows.Forms.TextBox();
			this.labelProdGoal = new System.Windows.Forms.Label();
			this.panelArrows.SuspendLayout();
			this.panelSheet.SuspendLayout();
			this.panelAptInfo.SuspendLayout();
			this.panelCalendar.SuspendLayout();
			this.groupSearch.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.tabWaiting.SuspendLayout();
			this.tabSched.SuspendLayout();
			this.tabProv.SuspendLayout();
			this.tabReminders.SuspendLayout();
			this.panelMakeButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageListMain
			// 
			this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
			this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListMain.Images.SetKeyName(0, "Pat.gif");
			this.imageListMain.Images.SetKeyName(1, "print.gif");
			this.imageListMain.Images.SetKeyName(2, "apptLists.gif");
			this.imageListMain.Images.SetKeyName(3, "DT Rapid Call.png");
			// 
			// Calendar2
			// 
			this.Calendar2.Location = new System.Drawing.Point(0, 24);
			this.Calendar2.Name = "Calendar2";
			this.Calendar2.ScrollChange = 1;
			this.Calendar2.TabIndex = 23;
			this.Calendar2.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.Calendar2_DateSelected);
			// 
			// labelDate
			// 
			this.labelDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelDate.Location = new System.Drawing.Point(2, 4);
			this.labelDate.Name = "labelDate";
			this.labelDate.Size = new System.Drawing.Size(56, 16);
			this.labelDate.TabIndex = 24;
			this.labelDate.Text = "Wed";
			// 
			// labelDate2
			// 
			this.labelDate2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelDate2.Location = new System.Drawing.Point(46, 4);
			this.labelDate2.Name = "labelDate2";
			this.labelDate2.Size = new System.Drawing.Size(100, 20);
			this.labelDate2.TabIndex = 25;
			this.labelDate2.Text = "-  Oct 20";
			// 
			// panelArrows
			// 
			this.panelArrows.Controls.Add(this.butBackMonth);
			this.panelArrows.Controls.Add(this.butFwdMonth);
			this.panelArrows.Controls.Add(this.butBackWeek);
			this.panelArrows.Controls.Add(this.butFwdWeek);
			this.panelArrows.Controls.Add(this.butToday);
			this.panelArrows.Controls.Add(this.butBack);
			this.panelArrows.Controls.Add(this.butFwd);
			this.panelArrows.Location = new System.Drawing.Point(1, 189);
			this.panelArrows.Name = "panelArrows";
			this.panelArrows.Size = new System.Drawing.Size(217, 24);
			this.panelArrows.TabIndex = 32;
			// 
			// butBackMonth
			// 
			this.butBackMonth.AdjustImageLocation = new System.Drawing.Point(-3, -1);
			this.butBackMonth.Autosize = true;
			this.butBackMonth.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBackMonth.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBackMonth.CornerRadius = 4F;
			this.butBackMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butBackMonth.Image = ((System.Drawing.Image)(resources.GetObject("butBackMonth.Image")));
			this.butBackMonth.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butBackMonth.Location = new System.Drawing.Point(1, 0);
			this.butBackMonth.Name = "butBackMonth";
			this.butBackMonth.Size = new System.Drawing.Size(32, 22);
			this.butBackMonth.TabIndex = 57;
			this.butBackMonth.Text = "M";
			this.butBackMonth.Click += new System.EventHandler(this.butBackMonth_Click);
			// 
			// butFwdMonth
			// 
			this.butFwdMonth.AdjustImageLocation = new System.Drawing.Point(5, -1);
			this.butFwdMonth.Autosize = false;
			this.butFwdMonth.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butFwdMonth.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butFwdMonth.CornerRadius = 4F;
			this.butFwdMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butFwdMonth.Image = ((System.Drawing.Image)(resources.GetObject("butFwdMonth.Image")));
			this.butFwdMonth.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.butFwdMonth.Location = new System.Drawing.Point(188, 0);
			this.butFwdMonth.Name = "butFwdMonth";
			this.butFwdMonth.Size = new System.Drawing.Size(29, 22);
			this.butFwdMonth.TabIndex = 56;
			this.butFwdMonth.Text = "M";
			this.butFwdMonth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butFwdMonth.Click += new System.EventHandler(this.butFwdMonth_Click);
			// 
			// butBackWeek
			// 
			this.butBackWeek.AdjustImageLocation = new System.Drawing.Point(-3, -1);
			this.butBackWeek.Autosize = true;
			this.butBackWeek.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBackWeek.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBackWeek.CornerRadius = 4F;
			this.butBackWeek.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butBackWeek.Image = ((System.Drawing.Image)(resources.GetObject("butBackWeek.Image")));
			this.butBackWeek.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butBackWeek.Location = new System.Drawing.Point(33, 0);
			this.butBackWeek.Name = "butBackWeek";
			this.butBackWeek.Size = new System.Drawing.Size(33, 22);
			this.butBackWeek.TabIndex = 55;
			this.butBackWeek.Text = "W";
			this.butBackWeek.Click += new System.EventHandler(this.butBackWeek_Click);
			// 
			// butFwdWeek
			// 
			this.butFwdWeek.AdjustImageLocation = new System.Drawing.Point(5, -1);
			this.butFwdWeek.Autosize = false;
			this.butFwdWeek.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butFwdWeek.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butFwdWeek.CornerRadius = 4F;
			this.butFwdWeek.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butFwdWeek.Image = ((System.Drawing.Image)(resources.GetObject("butFwdWeek.Image")));
			this.butFwdWeek.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.butFwdWeek.Location = new System.Drawing.Point(158, 0);
			this.butFwdWeek.Name = "butFwdWeek";
			this.butFwdWeek.Size = new System.Drawing.Size(30, 22);
			this.butFwdWeek.TabIndex = 54;
			this.butFwdWeek.Text = "W";
			this.butFwdWeek.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butFwdWeek.Click += new System.EventHandler(this.butFwdWeek_Click);
			// 
			// butToday
			// 
			this.butToday.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butToday.Autosize = false;
			this.butToday.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butToday.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butToday.CornerRadius = 4F;
			this.butToday.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butToday.Location = new System.Drawing.Point(85, 0);
			this.butToday.Name = "butToday";
			this.butToday.Size = new System.Drawing.Size(54, 22);
			this.butToday.TabIndex = 29;
			this.butToday.Text = "Today";
			this.butToday.Click += new System.EventHandler(this.butToday_Click);
			// 
			// butBack
			// 
			this.butBack.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBack.Autosize = true;
			this.butBack.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBack.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBack.CornerRadius = 4F;
			this.butBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butBack.Image = ((System.Drawing.Image)(resources.GetObject("butBack.Image")));
			this.butBack.Location = new System.Drawing.Point(66, 0);
			this.butBack.Name = "butBack";
			this.butBack.Size = new System.Drawing.Size(19, 22);
			this.butBack.TabIndex = 51;
			this.butBack.Click += new System.EventHandler(this.butBack_Click);
			// 
			// butFwd
			// 
			this.butFwd.AdjustImageLocation = new System.Drawing.Point(5, -1);
			this.butFwd.Autosize = false;
			this.butFwd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butFwd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butFwd.CornerRadius = 4F;
			this.butFwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butFwd.Image = ((System.Drawing.Image)(resources.GetObject("butFwd.Image")));
			this.butFwd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.butFwd.Location = new System.Drawing.Point(139, 0);
			this.butFwd.Name = "butFwd";
			this.butFwd.Size = new System.Drawing.Size(19, 22);
			this.butFwd.TabIndex = 53;
			this.butFwd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butFwd.Click += new System.EventHandler(this.butFwd_Click);
			// 
			// panelSheet
			// 
			this.panelSheet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelSheet.Controls.Add(this.vScrollBar1);
			this.panelSheet.Controls.Add(this.ContrApptSheet2);
			this.panelSheet.Controls.Add(this.labelNoneView);
			this.panelSheet.Location = new System.Drawing.Point(0, 17);
			this.panelSheet.Name = "panelSheet";
			this.panelSheet.Size = new System.Drawing.Size(235, 726);
			this.panelSheet.TabIndex = 44;
			this.panelSheet.Resize += new System.EventHandler(this.panelSheet_Resize);
			// 
			// vScrollBar1
			// 
			this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
			this.vScrollBar1.Location = new System.Drawing.Point(216, 0);
			this.vScrollBar1.Name = "vScrollBar1";
			this.vScrollBar1.Size = new System.Drawing.Size(17, 724);
			this.vScrollBar1.TabIndex = 23;
			this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
			// 
			// ContrApptSheet2
			// 
			this.ContrApptSheet2.Location = new System.Drawing.Point(2, -550);
			this.ContrApptSheet2.Name = "ContrApptSheet2";
			this.ContrApptSheet2.Size = new System.Drawing.Size(60, 1728);
			this.ContrApptSheet2.TabIndex = 22;
			this.ContrApptSheet2.DoubleClick += new System.EventHandler(this.ContrApptSheet2_DoubleClick);
			this.ContrApptSheet2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ContrApptSheet2_MouseDown);
			this.ContrApptSheet2.MouseLeave += new System.EventHandler(this.ContrApptSheet2_MouseLeave);
			this.ContrApptSheet2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ContrApptSheet2_MouseMove);
			this.ContrApptSheet2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ContrApptSheet2_MouseUp);
			// 
			// labelNoneView
			// 
			this.labelNoneView.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelNoneView.AutoSize = true;
			this.labelNoneView.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelNoneView.Location = new System.Drawing.Point(-57, 248);
			this.labelNoneView.Name = "labelNoneView";
			this.labelNoneView.Size = new System.Drawing.Size(324, 66);
			this.labelNoneView.TabIndex = 83;
			this.labelNoneView.Text = "Please select a clinic \r\nor an appointment view.";
			this.labelNoneView.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panelAptInfo
			// 
			this.panelAptInfo.Controls.Add(this.listConfirmed);
			this.panelAptInfo.Controls.Add(this.butComplete);
			this.panelAptInfo.Controls.Add(this.butUnsched);
			this.panelAptInfo.Controls.Add(this.butDelete);
			this.panelAptInfo.Controls.Add(this.butBreak);
			this.panelAptInfo.Location = new System.Drawing.Point(665, 423);
			this.panelAptInfo.Name = "panelAptInfo";
			this.panelAptInfo.Size = new System.Drawing.Size(107, 116);
			this.panelAptInfo.TabIndex = 45;
			// 
			// listConfirmed
			// 
			this.listConfirmed.IntegralHeight = false;
			this.listConfirmed.Location = new System.Drawing.Point(31, 2);
			this.listConfirmed.Name = "listConfirmed";
			this.listConfirmed.Size = new System.Drawing.Size(73, 111);
			this.listConfirmed.TabIndex = 75;
			this.listConfirmed.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listConfirmed_MouseDown);
			// 
			// butComplete
			// 
			this.butComplete.BackColor = System.Drawing.SystemColors.Control;
			this.butComplete.Image = ((System.Drawing.Image)(resources.GetObject("butComplete.Image")));
			this.butComplete.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.butComplete.Location = new System.Drawing.Point(2, 57);
			this.butComplete.Name = "butComplete";
			this.butComplete.Size = new System.Drawing.Size(28, 28);
			this.butComplete.TabIndex = 69;
			this.butComplete.TabStop = false;
			this.butComplete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butComplete.UseVisualStyleBackColor = false;
			this.butComplete.Click += new System.EventHandler(this.butComplete_Click);
			// 
			// butUnsched
			// 
			this.butUnsched.BackColor = System.Drawing.SystemColors.Control;
			this.butUnsched.Image = ((System.Drawing.Image)(resources.GetObject("butUnsched.Image")));
			this.butUnsched.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.butUnsched.Location = new System.Drawing.Point(2, 1);
			this.butUnsched.Name = "butUnsched";
			this.butUnsched.Size = new System.Drawing.Size(28, 28);
			this.butUnsched.TabIndex = 68;
			this.butUnsched.TabStop = false;
			this.butUnsched.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUnsched.UseVisualStyleBackColor = false;
			this.butUnsched.Click += new System.EventHandler(this.butUnsched_Click);
			// 
			// butDelete
			// 
			this.butDelete.BackColor = System.Drawing.SystemColors.Control;
			this.butDelete.Image = ((System.Drawing.Image)(resources.GetObject("butDelete.Image")));
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.butDelete.Location = new System.Drawing.Point(2, 85);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(28, 28);
			this.butDelete.TabIndex = 66;
			this.butDelete.TabStop = false;
			this.butDelete.UseVisualStyleBackColor = false;
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butBreak
			// 
			this.butBreak.BackColor = System.Drawing.SystemColors.Control;
			this.butBreak.Image = ((System.Drawing.Image)(resources.GetObject("butBreak.Image")));
			this.butBreak.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.butBreak.Location = new System.Drawing.Point(2, 29);
			this.butBreak.Name = "butBreak";
			this.butBreak.Size = new System.Drawing.Size(28, 28);
			this.butBreak.TabIndex = 65;
			this.butBreak.TabStop = false;
			this.butBreak.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butBreak.UseVisualStyleBackColor = false;
			this.butBreak.Click += new System.EventHandler(this.butBreak_Click);
			// 
			// panelCalendar
			// 
			this.panelCalendar.Controls.Add(this.textProdGoal);
			this.panelCalendar.Controls.Add(this.labelProdGoal);
			this.panelCalendar.Controls.Add(this.radioWeek);
			this.panelCalendar.Controls.Add(this.panelArrows);
			this.panelCalendar.Controls.Add(this.radioDay);
			this.panelCalendar.Controls.Add(this.butGraph);
			this.panelCalendar.Controls.Add(this.butMonth);
			this.panelCalendar.Controls.Add(this.pinBoard);
			this.panelCalendar.Controls.Add(this.butLab);
			this.panelCalendar.Controls.Add(this.butSearch);
			this.panelCalendar.Controls.Add(this.textProduction);
			this.panelCalendar.Controls.Add(this.label7);
			this.panelCalendar.Controls.Add(this.textLab);
			this.panelCalendar.Controls.Add(this.comboView);
			this.panelCalendar.Controls.Add(this.label2);
			this.panelCalendar.Controls.Add(this.butClearPin);
			this.panelCalendar.Controls.Add(this.Calendar2);
			this.panelCalendar.Controls.Add(this.labelDate);
			this.panelCalendar.Controls.Add(this.labelDate2);
			this.panelCalendar.Location = new System.Drawing.Point(665, 28);
			this.panelCalendar.Name = "panelCalendar";
			this.panelCalendar.Size = new System.Drawing.Size(219, 394);
			this.panelCalendar.TabIndex = 46;
			// 
			// radioWeek
			// 
			this.radioWeek.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioWeek.Location = new System.Drawing.Point(43, 238);
			this.radioWeek.Name = "radioWeek";
			this.radioWeek.Size = new System.Drawing.Size(68, 16);
			this.radioWeek.TabIndex = 92;
			this.radioWeek.Text = "Week";
			this.radioWeek.Click += new System.EventHandler(this.radioWeek_Click);
			// 
			// radioDay
			// 
			this.radioDay.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioDay.Location = new System.Drawing.Point(43, 218);
			this.radioDay.Name = "radioDay";
			this.radioDay.Size = new System.Drawing.Size(68, 16);
			this.radioDay.TabIndex = 91;
			this.radioDay.Text = "Day";
			this.radioDay.Click += new System.EventHandler(this.radioDay_Click);
			// 
			// butGraph
			// 
			this.butGraph.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGraph.Autosize = true;
			this.butGraph.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGraph.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGraph.CornerRadius = 4F;
			this.butGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butGraph.Location = new System.Drawing.Point(3, 309);
			this.butGraph.Name = "butGraph";
			this.butGraph.Size = new System.Drawing.Size(42, 24);
			this.butGraph.TabIndex = 78;
			this.butGraph.TabStop = false;
			this.butGraph.Text = "Emp";
			this.butGraph.Visible = false;
			this.butGraph.Click += new System.EventHandler(this.butGraph_Click);
			// 
			// butMonth
			// 
			this.butMonth.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMonth.Autosize = false;
			this.butMonth.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMonth.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMonth.CornerRadius = 4F;
			this.butMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butMonth.Location = new System.Drawing.Point(152, 1);
			this.butMonth.Name = "butMonth";
			this.butMonth.Size = new System.Drawing.Size(65, 22);
			this.butMonth.TabIndex = 79;
			this.butMonth.Text = "Month";
			this.butMonth.Visible = false;
			this.butMonth.Click += new System.EventHandler(this.butMonth_Click);
			// 
			// pinBoard
			// 
			this.pinBoard.Location = new System.Drawing.Point(119, 213);
			this.pinBoard.Name = "pinBoard";
			this.pinBoard.SelectedIndex = -1;
			this.pinBoard.Size = new System.Drawing.Size(99, 96);
			this.pinBoard.TabIndex = 78;
			this.pinBoard.Text = "pinBoard";
			this.pinBoard.SelectedIndexChanged += new System.EventHandler(this.pinBoard_SelectedIndexChanged);
			this.pinBoard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pinBoard_MouseDown);
			this.pinBoard.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pinBoard_MouseMove);
			this.pinBoard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pinBoard_MouseUp);
			// 
			// butLab
			// 
			this.butLab.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLab.Autosize = true;
			this.butLab.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLab.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLab.CornerRadius = 4F;
			this.butLab.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butLab.Location = new System.Drawing.Point(3, 333);
			this.butLab.Name = "butLab";
			this.butLab.Size = new System.Drawing.Size(79, 21);
			this.butLab.TabIndex = 77;
			this.butLab.Text = "Lab Cases";
			this.butLab.Click += new System.EventHandler(this.butLab_Click);
			// 
			// butSearch
			// 
			this.butSearch.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSearch.Autosize = true;
			this.butSearch.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSearch.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSearch.CornerRadius = 4F;
			this.butSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSearch.Location = new System.Drawing.Point(43, 285);
			this.butSearch.Name = "butSearch";
			this.butSearch.Size = new System.Drawing.Size(75, 24);
			this.butSearch.TabIndex = 40;
			this.butSearch.Text = "Search";
			this.butSearch.Click += new System.EventHandler(this.butSearch_Click);
			// 
			// textProduction
			// 
			this.textProduction.BackColor = System.Drawing.Color.White;
			this.textProduction.Location = new System.Drawing.Point(85, 353);
			this.textProduction.Name = "textProduction";
			this.textProduction.ReadOnly = true;
			this.textProduction.Size = new System.Drawing.Size(133, 20);
			this.textProduction.TabIndex = 38;
			this.textProduction.Text = "$100";
			this.textProduction.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 357);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(68, 15);
			this.label7.TabIndex = 39;
			this.label7.Text = "Daily Prod";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textLab
			// 
			this.textLab.BackColor = System.Drawing.Color.White;
			this.textLab.Location = new System.Drawing.Point(85, 333);
			this.textLab.Name = "textLab";
			this.textLab.ReadOnly = true;
			this.textLab.Size = new System.Drawing.Size(133, 20);
			this.textLab.TabIndex = 36;
			this.textLab.Text = "All Received";
			this.textLab.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// comboView
			// 
			this.comboView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboView.Location = new System.Drawing.Point(85, 312);
			this.comboView.MaxDropDownItems = 30;
			this.comboView.Name = "comboView";
			this.comboView.Size = new System.Drawing.Size(133, 21);
			this.comboView.TabIndex = 35;
			this.comboView.SelectionChangeCommitted += new System.EventHandler(this.comboView_SelectionChangeCommitted);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(17, 314);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 16);
			this.label2.TabIndex = 34;
			this.label2.Text = "View";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butClearPin
			// 
			this.butClearPin.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClearPin.Autosize = true;
			this.butClearPin.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClearPin.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClearPin.CornerRadius = 4F;
			this.butClearPin.Image = ((System.Drawing.Image)(resources.GetObject("butClearPin.Image")));
			this.butClearPin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butClearPin.Location = new System.Drawing.Point(43, 260);
			this.butClearPin.Name = "butClearPin";
			this.butClearPin.Size = new System.Drawing.Size(75, 24);
			this.butClearPin.TabIndex = 33;
			this.butClearPin.Text = "Clear";
			this.butClearPin.Click += new System.EventHandler(this.butClearPin_Click);
			// 
			// panelOps
			// 
			this.panelOps.Location = new System.Drawing.Point(0, 0);
			this.panelOps.Name = "panelOps";
			this.panelOps.Size = new System.Drawing.Size(676, 17);
			this.panelOps.TabIndex = 48;
			// 
			// toolTip1
			// 
			this.toolTip1.AutoPopDelay = 5000;
			this.toolTip1.InitialDelay = 100;
			this.toolTip1.ReshowDelay = 100;
			// 
			// pd2
			// 
			this.pd2.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.pd2_PrintPage);
			// 
			// menuApt
			// 
			this.menuApt.Popup += new System.EventHandler(this.menuApt_Popup);
			// 
			// groupSearch
			// 
			this.groupSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.groupSearch.Controls.Add(this.groupBox1);
			this.groupSearch.Controls.Add(this.butProvPick);
			this.groupSearch.Controls.Add(this.butRefresh);
			this.groupSearch.Controls.Add(this.listSearchResults);
			this.groupSearch.Controls.Add(this.listProviders);
			this.groupSearch.Controls.Add(this.butSearchClose);
			this.groupSearch.Controls.Add(this.groupBox2);
			this.groupSearch.Controls.Add(this.label8);
			this.groupSearch.Controls.Add(this.butSearchCloseX);
			this.groupSearch.Controls.Add(this.butSearchNext);
			this.groupSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupSearch.Location = new System.Drawing.Point(380, 340);
			this.groupSearch.Name = "groupSearch";
			this.groupSearch.Size = new System.Drawing.Size(219, 366);
			this.groupSearch.TabIndex = 74;
			this.groupSearch.TabStop = false;
			this.groupSearch.Text = "Search For Opening";
			this.groupSearch.Visible = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butProvHygenist);
			this.groupBox1.Controls.Add(this.butProvDentist);
			this.groupBox1.Location = new System.Drawing.Point(130, 253);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(85, 63);
			this.groupBox1.TabIndex = 89;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Search by";
			// 
			// butProvHygenist
			// 
			this.butProvHygenist.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butProvHygenist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butProvHygenist.Autosize = true;
			this.butProvHygenist.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butProvHygenist.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butProvHygenist.CornerRadius = 4F;
			this.butProvHygenist.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butProvHygenist.Location = new System.Drawing.Point(6, 37);
			this.butProvHygenist.Name = "butProvHygenist";
			this.butProvHygenist.Size = new System.Drawing.Size(73, 22);
			this.butProvHygenist.TabIndex = 92;
			this.butProvHygenist.Text = "Hygienists";
			this.butProvHygenist.Click += new System.EventHandler(this.butProvHygenist_Click);
			// 
			// butProvDentist
			// 
			this.butProvDentist.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butProvDentist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butProvDentist.Autosize = true;
			this.butProvDentist.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butProvDentist.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butProvDentist.CornerRadius = 4F;
			this.butProvDentist.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butProvDentist.Location = new System.Drawing.Point(6, 14);
			this.butProvDentist.Name = "butProvDentist";
			this.butProvDentist.Size = new System.Drawing.Size(73, 22);
			this.butProvDentist.TabIndex = 91;
			this.butProvDentist.Text = "Providers";
			this.butProvDentist.Click += new System.EventHandler(this.butProvDentist_Click);
			// 
			// butProvPick
			// 
			this.butProvPick.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butProvPick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butProvPick.Autosize = true;
			this.butProvPick.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butProvPick.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butProvPick.CornerRadius = 4F;
			this.butProvPick.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butProvPick.Location = new System.Drawing.Point(6, 340);
			this.butProvPick.Name = "butProvPick";
			this.butProvPick.Size = new System.Drawing.Size(82, 22);
			this.butProvPick.TabIndex = 88;
			this.butProvPick.Text = "Providers...";
			this.butProvPick.Click += new System.EventHandler(this.butProvPick_Click);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butRefresh.Location = new System.Drawing.Point(153, 318);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(62, 22);
			this.butRefresh.TabIndex = 88;
			this.butRefresh.Text = "Search";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// listSearchResults
			// 
			this.listSearchResults.IntegralHeight = false;
			this.listSearchResults.Location = new System.Drawing.Point(6, 32);
			this.listSearchResults.Name = "listSearchResults";
			this.listSearchResults.Size = new System.Drawing.Size(193, 134);
			this.listSearchResults.TabIndex = 87;
			this.listSearchResults.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listSearchResults_MouseDown);
			// 
			// listProviders
			// 
			this.listProviders.Location = new System.Drawing.Point(6, 269);
			this.listProviders.Name = "listProviders";
			this.listProviders.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listProviders.Size = new System.Drawing.Size(118, 69);
			this.listProviders.TabIndex = 86;
			// 
			// butSearchClose
			// 
			this.butSearchClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSearchClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSearchClose.Autosize = true;
			this.butSearchClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSearchClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSearchClose.CornerRadius = 4F;
			this.butSearchClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSearchClose.Location = new System.Drawing.Point(153, 342);
			this.butSearchClose.Name = "butSearchClose";
			this.butSearchClose.Size = new System.Drawing.Size(62, 22);
			this.butSearchClose.TabIndex = 85;
			this.butSearchClose.Text = "Close";
			this.butSearchClose.Click += new System.EventHandler(this.butSearchClose_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.textAfter);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.radioBeforePM);
			this.groupBox2.Controls.Add(this.radioBeforeAM);
			this.groupBox2.Controls.Add(this.textBefore);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.panel1);
			this.groupBox2.Controls.Add(this.dateSearch);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(6, 168);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(193, 84);
			this.groupBox2.TabIndex = 84;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Date/Time Restrictions";
			// 
			// textAfter
			// 
			this.textAfter.Location = new System.Drawing.Point(57, 60);
			this.textAfter.Name = "textAfter";
			this.textAfter.Size = new System.Drawing.Size(44, 20);
			this.textAfter.TabIndex = 88;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(1, 62);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(53, 16);
			this.label11.TabIndex = 87;
			this.label11.Text = "After";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// radioBeforePM
			// 
			this.radioBeforePM.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioBeforePM.Location = new System.Drawing.Point(151, 41);
			this.radioBeforePM.Name = "radioBeforePM";
			this.radioBeforePM.Size = new System.Drawing.Size(37, 15);
			this.radioBeforePM.TabIndex = 86;
			this.radioBeforePM.Text = "pm";
			// 
			// radioBeforeAM
			// 
			this.radioBeforeAM.Checked = true;
			this.radioBeforeAM.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioBeforeAM.Location = new System.Drawing.Point(108, 41);
			this.radioBeforeAM.Name = "radioBeforeAM";
			this.radioBeforeAM.Size = new System.Drawing.Size(37, 15);
			this.radioBeforeAM.TabIndex = 85;
			this.radioBeforeAM.TabStop = true;
			this.radioBeforeAM.Text = "am";
			// 
			// textBefore
			// 
			this.textBefore.Location = new System.Drawing.Point(57, 38);
			this.textBefore.Name = "textBefore";
			this.textBefore.Size = new System.Drawing.Size(44, 20);
			this.textBefore.TabIndex = 84;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(1, 40);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(53, 16);
			this.label10.TabIndex = 83;
			this.label10.Text = "Before";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.radioAfterAM);
			this.panel1.Controls.Add(this.radioAfterPM);
			this.panel1.Location = new System.Drawing.Point(105, 60);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(84, 20);
			this.panel1.TabIndex = 86;
			// 
			// radioAfterAM
			// 
			this.radioAfterAM.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioAfterAM.Location = new System.Drawing.Point(3, 2);
			this.radioAfterAM.Name = "radioAfterAM";
			this.radioAfterAM.Size = new System.Drawing.Size(37, 15);
			this.radioAfterAM.TabIndex = 89;
			this.radioAfterAM.Text = "am";
			// 
			// radioAfterPM
			// 
			this.radioAfterPM.Checked = true;
			this.radioAfterPM.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioAfterPM.Location = new System.Drawing.Point(46, 2);
			this.radioAfterPM.Name = "radioAfterPM";
			this.radioAfterPM.Size = new System.Drawing.Size(36, 15);
			this.radioAfterPM.TabIndex = 90;
			this.radioAfterPM.TabStop = true;
			this.radioAfterPM.Text = "pm";
			// 
			// dateSearch
			// 
			this.dateSearch.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateSearch.Location = new System.Drawing.Point(57, 16);
			this.dateSearch.Name = "dateSearch";
			this.dateSearch.Size = new System.Drawing.Size(130, 20);
			this.dateSearch.TabIndex = 90;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(1, 19);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(54, 16);
			this.label9.TabIndex = 89;
			this.label9.Text = "After";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(6, 251);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(92, 16);
			this.label8.TabIndex = 80;
			this.label8.Text = "Providers";
			this.label8.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butSearchCloseX
			// 
			this.butSearchCloseX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butSearchCloseX.ForeColor = System.Drawing.SystemColors.Control;
			this.butSearchCloseX.Image = ((System.Drawing.Image)(resources.GetObject("butSearchCloseX.Image")));
			this.butSearchCloseX.Location = new System.Drawing.Point(185, 7);
			this.butSearchCloseX.Name = "butSearchCloseX";
			this.butSearchCloseX.Size = new System.Drawing.Size(16, 16);
			this.butSearchCloseX.TabIndex = 0;
			this.butSearchCloseX.Click += new System.EventHandler(this.butSearchCloseX_Click);
			// 
			// butSearchNext
			// 
			this.butSearchNext.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSearchNext.Autosize = true;
			this.butSearchNext.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSearchNext.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSearchNext.CornerRadius = 4F;
			this.butSearchNext.Image = ((System.Drawing.Image)(resources.GetObject("butSearchNext.Image")));
			this.butSearchNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.butSearchNext.Location = new System.Drawing.Point(111, 9);
			this.butSearchNext.Name = "butSearchNext";
			this.butSearchNext.Size = new System.Drawing.Size(71, 22);
			this.butSearchNext.TabIndex = 77;
			this.butSearchNext.Text = "More";
			this.butSearchNext.Click += new System.EventHandler(this.butSearchMore_Click);
			// 
			// timerInfoBubble
			// 
			this.timerInfoBubble.Interval = 300;
			this.timerInfoBubble.Tick += new System.EventHandler(this.timerInfoBubble_Tick);
			// 
			// tabControl
			// 
			this.tabControl.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.tabControl.Controls.Add(this.tabWaiting);
			this.tabControl.Controls.Add(this.tabSched);
			this.tabControl.Controls.Add(this.tabProv);
			this.tabControl.Controls.Add(this.tabReminders);
			this.tabControl.Location = new System.Drawing.Point(665, 542);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(219, 166);
			this.tabControl.TabIndex = 78;
			// 
			// tabWaiting
			// 
			this.tabWaiting.Controls.Add(this.gridWaiting);
			this.tabWaiting.Location = new System.Drawing.Point(4, 22);
			this.tabWaiting.Name = "tabWaiting";
			this.tabWaiting.Padding = new System.Windows.Forms.Padding(3);
			this.tabWaiting.Size = new System.Drawing.Size(211, 140);
			this.tabWaiting.TabIndex = 0;
			this.tabWaiting.Text = "Waiting";
			this.tabWaiting.UseVisualStyleBackColor = true;
			// 
			// gridWaiting
			// 
			this.gridWaiting.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridWaiting.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridWaiting.HasAddButton = false;
			this.gridWaiting.HasDropDowns = false;
			this.gridWaiting.HasMultilineHeaders = false;
			this.gridWaiting.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridWaiting.HeaderHeight = 15;
			this.gridWaiting.HScrollVisible = false;
			this.gridWaiting.Location = new System.Drawing.Point(0, 0);
			this.gridWaiting.Margin = new System.Windows.Forms.Padding(0);
			this.gridWaiting.Name = "gridWaiting";
			this.gridWaiting.ScrollValue = 0;
			this.gridWaiting.Size = new System.Drawing.Size(211, 140);
			this.gridWaiting.TabIndex = 78;
			this.gridWaiting.Title = "Waiting Room";
			this.gridWaiting.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridWaiting.TitleHeight = 18;
			this.gridWaiting.TranslationName = "TableApptWaiting";
			// 
			// tabSched
			// 
			this.tabSched.Controls.Add(this.gridEmpSched);
			this.tabSched.Location = new System.Drawing.Point(4, 22);
			this.tabSched.Name = "tabSched";
			this.tabSched.Padding = new System.Windows.Forms.Padding(3);
			this.tabSched.Size = new System.Drawing.Size(211, 161);
			this.tabSched.TabIndex = 1;
			this.tabSched.Text = "Emp";
			this.tabSched.UseVisualStyleBackColor = true;
			// 
			// gridEmpSched
			// 
			this.gridEmpSched.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridEmpSched.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridEmpSched.HasAddButton = false;
			this.gridEmpSched.HasDropDowns = false;
			this.gridEmpSched.HasMultilineHeaders = false;
			this.gridEmpSched.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridEmpSched.HeaderHeight = 15;
			this.gridEmpSched.HScrollVisible = true;
			this.gridEmpSched.Location = new System.Drawing.Point(0, 0);
			this.gridEmpSched.Margin = new System.Windows.Forms.Padding(0);
			this.gridEmpSched.Name = "gridEmpSched";
			this.gridEmpSched.ScrollValue = 0;
			this.gridEmpSched.Size = new System.Drawing.Size(211, 161);
			this.gridEmpSched.TabIndex = 77;
			this.gridEmpSched.Title = "Employee Schedules";
			this.gridEmpSched.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridEmpSched.TitleHeight = 18;
			this.gridEmpSched.TranslationName = "TableApptEmpSched";
			this.gridEmpSched.DoubleClick += new System.EventHandler(this.gridEmpSched_DoubleClick);
			// 
			// tabProv
			// 
			this.tabProv.Controls.Add(this.gridProv);
			this.tabProv.Location = new System.Drawing.Point(4, 22);
			this.tabProv.Name = "tabProv";
			this.tabProv.Size = new System.Drawing.Size(211, 161);
			this.tabProv.TabIndex = 2;
			this.tabProv.Text = "Prov";
			this.tabProv.UseVisualStyleBackColor = true;
			// 
			// gridProv
			// 
			this.gridProv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridProv.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridProv.HasAddButton = false;
			this.gridProv.HasDropDowns = false;
			this.gridProv.HasMultilineHeaders = false;
			this.gridProv.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridProv.HeaderHeight = 15;
			this.gridProv.HScrollVisible = true;
			this.gridProv.Location = new System.Drawing.Point(0, 0);
			this.gridProv.Margin = new System.Windows.Forms.Padding(0);
			this.gridProv.Name = "gridProv";
			this.gridProv.ScrollValue = 0;
			this.gridProv.Size = new System.Drawing.Size(211, 161);
			this.gridProv.TabIndex = 79;
			this.gridProv.Title = "Provider Schedules";
			this.gridProv.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridProv.TitleHeight = 18;
			this.gridProv.TranslationName = "TableAppProv";
			this.gridProv.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridProv_CellDoubleClick);
			// 
			// tabReminders
			// 
			this.tabReminders.Controls.Add(this.gridReminders);
			this.tabReminders.Location = new System.Drawing.Point(4, 22);
			this.tabReminders.Name = "tabReminders";
			this.tabReminders.Size = new System.Drawing.Size(211, 161);
			this.tabReminders.TabIndex = 3;
			this.tabReminders.Text = "Reminders";
			this.tabReminders.UseVisualStyleBackColor = true;
			// 
			// gridReminders
			// 
			this.gridReminders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridReminders.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridReminders.HasAddButton = false;
			this.gridReminders.HasDropDowns = false;
			this.gridReminders.HasMultilineHeaders = false;
			this.gridReminders.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridReminders.HeaderHeight = 15;
			this.gridReminders.HScrollVisible = false;
			this.gridReminders.Location = new System.Drawing.Point(0, 0);
			this.gridReminders.Name = "gridReminders";
			this.gridReminders.ScrollValue = 0;
			this.gridReminders.Size = new System.Drawing.Size(211, 161);
			this.gridReminders.TabIndex = 0;
			this.gridReminders.Title = "Reminders";
			this.gridReminders.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridReminders.TitleHeight = 18;
			this.gridReminders.TranslationName = "TableReminders";
			this.gridReminders.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridReminders_CellDoubleClick);
			this.gridReminders.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridReminders_MouseDown);
			// 
			// timerWaitingRoom
			// 
			this.timerWaitingRoom.Enabled = true;
			this.timerWaitingRoom.Interval = 1000;
			this.timerWaitingRoom.Tick += new System.EventHandler(this.timerWaitingRoom_Tick);
			// 
			// timerTests
			// 
			this.timerTests.Tick += new System.EventHandler(this.timerTests_Tick);
			// 
			// panelMakeButtons
			// 
			this.panelMakeButtons.Controls.Add(this.butMakeAppt);
			this.panelMakeButtons.Controls.Add(this.butFamRecall);
			this.panelMakeButtons.Controls.Add(this.butMakeRecall);
			this.panelMakeButtons.Controls.Add(this.butViewAppts);
			this.panelMakeButtons.Location = new System.Drawing.Point(772, 423);
			this.panelMakeButtons.Name = "panelMakeButtons";
			this.panelMakeButtons.Size = new System.Drawing.Size(112, 116);
			this.panelMakeButtons.TabIndex = 82;
			// 
			// butMakeAppt
			// 
			this.butMakeAppt.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMakeAppt.Autosize = true;
			this.butMakeAppt.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMakeAppt.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMakeAppt.CornerRadius = 4F;
			this.butMakeAppt.Image = ((System.Drawing.Image)(resources.GetObject("butMakeAppt.Image")));
			this.butMakeAppt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butMakeAppt.Location = new System.Drawing.Point(5, 5);
			this.butMakeAppt.Name = "butMakeAppt";
			this.butMakeAppt.Size = new System.Drawing.Size(103, 24);
			this.butMakeAppt.TabIndex = 76;
			this.butMakeAppt.TabStop = false;
			this.butMakeAppt.Text = "Make Appt";
			this.butMakeAppt.Click += new System.EventHandler(this.butMakeAppt_Click);
			// 
			// butFamRecall
			// 
			this.butFamRecall.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butFamRecall.Autosize = true;
			this.butFamRecall.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butFamRecall.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butFamRecall.CornerRadius = 4F;
			this.butFamRecall.Image = global::OpenDental.Properties.Resources.butRecall;
			this.butFamRecall.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butFamRecall.Location = new System.Drawing.Point(5, 57);
			this.butFamRecall.Name = "butFamRecall";
			this.butFamRecall.Size = new System.Drawing.Size(103, 24);
			this.butFamRecall.TabIndex = 81;
			this.butFamRecall.TabStop = false;
			this.butFamRecall.Text = "Fam Recall";
			this.butFamRecall.Click += new System.EventHandler(this.butFamRecall_Click);
			// 
			// butMakeRecall
			// 
			this.butMakeRecall.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMakeRecall.Autosize = true;
			this.butMakeRecall.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMakeRecall.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMakeRecall.CornerRadius = 4F;
			this.butMakeRecall.Image = global::OpenDental.Properties.Resources.butRecall;
			this.butMakeRecall.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butMakeRecall.Location = new System.Drawing.Point(5, 31);
			this.butMakeRecall.Name = "butMakeRecall";
			this.butMakeRecall.Size = new System.Drawing.Size(103, 24);
			this.butMakeRecall.TabIndex = 79;
			this.butMakeRecall.TabStop = false;
			this.butMakeRecall.Text = "Make Recall";
			this.butMakeRecall.Click += new System.EventHandler(this.butMakeRecall_Click);
			// 
			// butViewAppts
			// 
			this.butViewAppts.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butViewAppts.Autosize = true;
			this.butViewAppts.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butViewAppts.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butViewAppts.CornerRadius = 4F;
			this.butViewAppts.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butViewAppts.Location = new System.Drawing.Point(5, 83);
			this.butViewAppts.Name = "butViewAppts";
			this.butViewAppts.Size = new System.Drawing.Size(103, 24);
			this.butViewAppts.TabIndex = 80;
			this.butViewAppts.TabStop = false;
			this.butViewAppts.Text = "View Pat Appts";
			this.butViewAppts.Click += new System.EventHandler(this.butViewAppts_Click);
			// 
			// imageListTasks
			// 
			this.imageListTasks.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTasks.ImageStream")));
			this.imageListTasks.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListTasks.Images.SetKeyName(0, "TaskList.gif");
			this.imageListTasks.Images.SetKeyName(1, "checkBoxChecked.gif");
			this.imageListTasks.Images.SetKeyName(2, "checkBoxUnchecked.gif");
			this.imageListTasks.Images.SetKeyName(3, "TaskListHighlight.gif");
			this.imageListTasks.Images.SetKeyName(4, "checkBoxNew.gif");
			// 
			// ToolBarMain
			// 
			this.ToolBarMain.ImageList = this.imageListMain;
			this.ToolBarMain.Location = new System.Drawing.Point(680, 2);
			this.ToolBarMain.Name = "ToolBarMain";
			this.ToolBarMain.Size = new System.Drawing.Size(203, 25);
			this.ToolBarMain.TabIndex = 73;
			this.ToolBarMain.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.ToolBarMain_ButtonClick);
			// 
			// menuReminderEdit
			// 
			this.menuReminderEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemReminderDone,
            this.menuItemReminderGoto});
			this.menuReminderEdit.Popup += new System.EventHandler(this.menuReminderEdit_Popup);
			// 
			// menuItemReminderDone
			// 
			this.menuItemReminderDone.Index = 0;
			this.menuItemReminderDone.Text = "Done (affects all users)";
			this.menuItemReminderDone.Click += new System.EventHandler(this.menuItemReminderDone_Click);
			// 
			// menuItemReminderGoto
			// 
			this.menuItemReminderGoto.Index = 1;
			this.menuItemReminderGoto.Text = "Go To";
			this.menuItemReminderGoto.Click += new System.EventHandler(this.menuItemReminderGoto_Click);
			// 
			// _menuOp
			// 
			this._menuOp.Name = "_menuRightClick";
			this._menuOp.Size = new System.Drawing.Size(61, 4);
			// 
			// textProdGoal
			// 
			this.textProdGoal.BackColor = System.Drawing.Color.White;
			this.textProdGoal.Location = new System.Drawing.Point(85, 371);
			this.textProdGoal.Name = "textProdGoal";
			this.textProdGoal.ReadOnly = true;
			this.textProdGoal.Size = new System.Drawing.Size(133, 20);
			this.textProdGoal.TabIndex = 93;
			this.textProdGoal.Text = "$100";
			this.textProdGoal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelProdGoal
			// 
			this.labelProdGoal.Location = new System.Drawing.Point(5, 375);
			this.labelProdGoal.Name = "labelProdGoal";
			this.labelProdGoal.Size = new System.Drawing.Size(79, 15);
			this.labelProdGoal.TabIndex = 94;
			this.labelProdGoal.Text = "Daily Goal";
			this.labelProdGoal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ContrAppt
			// 
			this.Controls.Add(this.groupSearch);
			this.Controls.Add(this.panelMakeButtons);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.ToolBarMain);
			this.Controls.Add(this.panelOps);
			this.Controls.Add(this.panelCalendar);
			this.Controls.Add(this.panelAptInfo);
			this.Controls.Add(this.panelSheet);
			this.Name = "ContrAppt";
			this.Size = new System.Drawing.Size(939, 708);
			this.Load += new System.EventHandler(this.ContrAppt_Load);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.ContrAppt_Layout);
			this.Resize += new System.EventHandler(this.ContrAppt_Resize);
			this.panelArrows.ResumeLayout(false);
			this.panelSheet.ResumeLayout(false);
			this.panelSheet.PerformLayout();
			this.panelAptInfo.ResumeLayout(false);
			this.panelCalendar.ResumeLayout(false);
			this.panelCalendar.PerformLayout();
			this.groupSearch.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.tabControl.ResumeLayout(false);
			this.tabWaiting.ResumeLayout(false);
			this.tabSched.ResumeLayout(false);
			this.tabProv.ResumeLayout(false);
			this.tabReminders.ResumeLayout(false);
			this.panelMakeButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void ContrApptSheet2_MouseWheel(object sender,System.Windows.Forms.MouseEventArgs e) {
			int max=vScrollBar1.Maximum-vScrollBar1.LargeChange;//panelTable.Height-panelScroll.Height+3;
			int newScrollVal=vScrollBar1.Value-(int)(e.Delta/4);
			if(newScrollVal > max) {
				vScrollBar1.Value=max;
			}
			else if(newScrollVal < vScrollBar1.Minimum) {
				vScrollBar1.Value=vScrollBar1.Minimum;
			}
			else {
				vScrollBar1.Value=newScrollVal;
			}
			ContrApptSheet2.Location=new Point(0,-vScrollBar1.Value);
		}

		///<summary>Overload used when jumping here from another module, and you want to place appointments on the pinboard.</summary>
		public void ModuleSelectedWithPinboard(long patNum,List<long> listPinApptNums,List<long> listOpNums=null,List<long> listProvNums=null) {
			ModuleSelected(patNum,listPinApptNums,listOpNums,listProvNums);
			SendToPinboard(listPinApptNums);
		}

		///<summary>Refreshes the module for the passed in patient.  A patNum of 0 is acceptable.
		///Any ApptNums within listPinApptNums will get forcefully added to the main DataSet for the appointment module.</summary>
		public void ModuleSelected(long patNum,List<long> listPinApptNums=null,List<long> listOpNums=null,List<long> listProvNums=null) {
			if(IsHqNoneView()) {
				return;
			}
			RefreshModuleDataPatient(patNum);
			RefreshModuleDataPeriod(listPinApptNums,listOpNums,listProvNums,isRefreshSchedules:true);
			LayoutScrollOpProv();
			RefreshModuleScreenPatient();
			RefreshModuleScreenPeriod();
			Plugins.HookAddCode(this,"ContrAppt.ModuleSelected_end",patNum);
		}

		///<summary>Refreshes everything except the patient info. If false, will not refresh the appointment bubble.
		///If another workstation made a change, then refreshes datatables.</summary>
		public void RefreshPeriod(bool isRefreshBubble=true,List<long> listOpNums=null,List<long> listProvNums=null,bool isRefreshAppointments=true,
			bool isRefreshSchedules=false)
		{
			if(IsHqNoneView()) {
				return;
			}
			long oldBubbleNum=bubbleAptNum;
			RefreshModuleDataPeriod(listOpNums:listOpNums,listProvNums:listProvNums,isRefreshAppointments:isRefreshAppointments,isRefreshSchedules:isRefreshSchedules);
			LayoutScrollOpProv();
			RefreshModuleScreenPeriod();
			if(!isRefreshBubble) {
				bubbleAptNum=oldBubbleNum;
			}
		}

		/// <summary>Wrapper for RefreshPeriod, refreshes the schedules, but not the appointments</summary>
		public void RefreshPeriodSchedules() {
			RefreshPeriod(isRefreshAppointments:false,isRefreshSchedules:true);
		}

		///<summary>Returns true if the none appointment view is selected, clinics is turned on, and the Headquarters clinic is selected.
		///Also disables pretty much every control available in the appointment module if it is going to return true, otherwise re-enables them.</summary>
		private bool IsHqNoneView() {
			if(PrefC.HasClinicsEnabled && Clinics.ClinicNum==0 && comboView.SelectedIndex==0) {
				ContrApptSheet2.Visible=false;
				panelOps.Visible=false;
				vScrollBar1.Visible=false;
				labelNoneView.Visible=true;
				butBack.Enabled=false;
				butBackMonth.Enabled=false;
				butBackWeek.Enabled=false;
				butToday.Enabled=false;
				butFwd.Enabled=false;
				butFwdMonth.Enabled=false;
				butFwdWeek.Enabled=false;
				Calendar2.Enabled=false;
				panelMakeButtons.Enabled=false;
				pinBoard.ClearSelected();
				textLab.Text="";
				textProduction.Text="";
				//TODO Change this to only stop printing and lists
				ToolBarMain.Visible=false;
				return true;
			}
			else {
				ContrApptSheet2.Visible=true;
				panelOps.Visible=true;
				vScrollBar1.Visible=true;
				labelNoneView.Visible=false;
				butBack.Enabled=true;
				butBackMonth.Enabled=true;
				butBackWeek.Enabled=true;
				butToday.Enabled=true;
				butFwd.Enabled=true;
				butFwdMonth.Enabled=true;
				butFwdWeek.Enabled=true;
				Calendar2.Enabled=true;
				ToolBarMain.Visible=true;
				return false;
			}
		}

		///<summary>Fills PatCur from the database unless the patnum has not changed.</summary>
		public void RefreshModuleDataPatient(long patNum) {
			if(patNum==0) {
				PatCur=null;
				return;
			}
			//if(PatCur !=null && PatCur.PatNum==patNum) {//if patient has not changed
			//  return;//don't do anything
			//}
			//We have to go to the db because we need to get the most recent patient info. Mainly used for the AskedToArriveEarly time.
			PatCur=Patients.GetPat(patNum);
			Plugins.HookAddCode(this, "ContrAppt.RefreshModuleDataPatient_end");
		}

		///<summary>If needed, refreshes the _dtAppointments, _dtApptFields, and _dtPatFields tables.</summary>
		private void RefreshAppointmentsIfNeeded(DateTime dateStart,DateTime dateEnd,List<long> listPinApptNums=null,
			List<long> listOpNums=null,List<long> listProvNums=null,bool isRefreshNeeded=false)
		{
			if(_dtAppointments!=null && _dtApptFields!=null && _dtPatFields!=null && !isRefreshNeeded) {
				return;//If all data is already in memory and we are not forcing the refresh.
			}
			_dtAppointments=Appointments.GetPeriodApptsTable(dateStart,dateEnd,0,false,listPinApptNums,listOpNums,listProvNums);
			_dtApptFields=Appointments.GetApptFields(_dtAppointments);
			_dtPatFields=Appointments.GetPatFields(_dtAppointments.Select().Select(x => PIn.Long(x["PatNum"].ToString())).ToList());
		}

		/// <summary>If needed, refreshes the _dtSchedule, _dtEmpSched, and _dtProvSched tables.</summary>
		private void RefreshSchedulesIfNeeded(DateTime dateStart,DateTime dateEnd,List<long> listOpNums,bool isRefreshNeeded=false) {
			if(_dtSchedule!=null && _dtEmpSched!=null && _dtProvSched!=null && !isRefreshNeeded) {
				return;//If all data is already in memory and we are not forcing the refresh.
			}
			_dtEmpSched=Schedules.GetPeriodEmployeeSchedTable(dateStart,dateEnd,Clinics.ClinicNum);
			_dtProvSched=Schedules.GetPeriodProviderSchedTable(dateStart,dateEnd,Clinics.ClinicNum);
			_dtSchedule=Schedules.GetPeriodSchedule(dateStart,dateEnd,listOpNums);
		}

		///<summary>Always refreshes the _dtWaitingRoom table.</summary>
		private void RefreshWaitingRoomTable() {
			_dtWaitingRoom=Appointments.GetPeriodWaitingRoomTable();
		}

		///<summary>Gets op and prov indices for current view. Will refresh the appointments and schedules if the respective doRefreshes are set.</summary>
		private void RefreshModuleDataPeriod(List<long> listPinApptNums = null,List<long> listOpNums = null,List<long> listProvNums = null
			,bool isRefreshAppointments=true,bool isRefreshSchedules=false)
		{
			_aptBubbleDefs=DisplayFields.GetForCategory(DisplayFieldCategory.AppointmentBubble);
			bubbleAptNum=0;
			DateTime startDate;
			DateTime endDate;
			if(ApptDrawing.IsWeeklyView) {
				startDate=WeekStartDate;
				endDate=WeekEndDate;
			}
			else {
				startDate=AppointmentL.DateSelected;
				endDate=AppointmentL.DateSelected;
			}
			if(startDate.Year<1880 || endDate.Year<1880) {
				return;
			}
			//Calendar2.SetSelectionRange(startDate,endDate);
			if(PatCur==null) {
				//there cannot be a selected appointment if no patient is loaded.
				ContrApptSingle.SelectedAptNum=-1;//fixes a minor bug.
			}
			//Regardless of clinics, none view gets all providers and operatories (or close to) and we should not filter based on ops and provs.
			if(comboView.SelectedIndex<1) {
				listOpNums=null;
				listProvNums=null;
			}
			else {
				long apptViewNum=GetApptViewNumForUser();
				if(listOpNums==null) {
					listOpNums=ApptViewItems.GetOpsForView(apptViewNum);
				}
				if(listProvNums==null) {
					listProvNums=ApptViewItems.GetProvsForView(apptViewNum);
				}
			}
			RefreshAppointmentsIfNeeded(startDate,endDate,listPinApptNums,listOpNums,listProvNums,isRefreshAppointments);
			RefreshSchedulesIfNeeded(startDate,endDate,listOpNums,isRefreshSchedules);
			RefreshWaitingRoomTable();
			LastTimeDataRetrieved=DateTime.Now;
			SchedListPeriod=Schedules.ConvertTableToList(_dtSchedule);
			ApptView viewCur=null;
			if(comboView.SelectedIndex>0) {
				viewCur=_listApptViews[comboView.SelectedIndex-1];
			}
			ApptViewItemL.GetForCurView(viewCur,ApptDrawing.IsWeeklyView,SchedListPeriod);
		}

		/// <summary>Called from both ModuleSelected and from RefreshPeriod.  Do not call it from any event like Layout.  This also clears listConfirmed.</summary>
		public void LayoutScrollOpProv() {
			//the scrollbar logic cannot be moved to someplace where it will be activated while working in apptbook
			int oldHeight=ContrApptSheet2.Height;
			int oldVScrollVal=vScrollBar1.Value;
			if(!Defs.GetDictIsNull()) {
				ApptView viewCur=null;
				if(comboView.SelectedIndex>0) {
					viewCur=_listApptViews[comboView.SelectedIndex-1];
				}
				ApptViewItemL.GetForCurView(viewCur,ApptDrawing.IsWeeklyView,SchedListPeriod);//refreshes visops,etc
				ApptDrawing.ApptSheetWidth=panelSheet.Width-vScrollBar1.Width;
				ApptDrawing.ComputeColWidth(0);
				ContrApptSheet2.Height=ApptDrawing.LineH*24*ApptDrawing.RowsPerHr;
			}
			this.SuspendLayout();
			vScrollBar1.Enabled=true;
			vScrollBar1.Minimum=0;
			vScrollBar1.LargeChange=12*ApptDrawing.LineH;//12 rows
			vScrollBar1.Maximum=ContrApptSheet2.Height-panelSheet.Height+vScrollBar1.LargeChange;
			if(vScrollBar1.Maximum<0) {
				vScrollBar1.Maximum=0;
			}
			//Max is set again in Resize event
			vScrollBar1.SmallChange=6*ApptDrawing.LineH;//6 rows
			if(!_hasLayedOutScrollBar && vScrollBar1.Value==0) {//Should only run on startup
				int rowsPerHr=60/ApptDrawing.MinPerIncr*ApptDrawing.RowsPerIncr;
				//use the row setting from the selected view.
				if(_listApptViews.Count>0 && comboView.SelectedIndex>0) {					
					TimeSpan apptTimeScrollStart=_listApptViews[comboView.SelectedIndex-1].ApptTimeScrollStart;
					if(_listApptViews[comboView.SelectedIndex-1].IsScrollStartDynamic) {//Scroll start time at the earliest scheduled operatory or appointment
						//Get the schedules that have any operatory visible
						List<Schedule> listVisScheds=new List<Schedule>();
						foreach(Schedule sched in SchedListPeriod) {
							if(sched.Ops.Any(x => ApptDrawing.VisOps.Exists(y => x==y.OperatoryNum))//The schedule is linked to a visible operatory
								|| ApptDrawing.VisOps.Exists(x => x.ProvDentist==sched.ProvNum && !x.IsHygiene)//The dentist is in a visible operatory
								|| ApptDrawing.VisOps.Exists(x => x.ProvHygienist==sched.ProvNum && x.IsHygiene))//The hygienist is in a visible operatory
							{
								listVisScheds.Add(sched);
							}
						}
						long schedProvUnassinged=PrefC.GetLong(PrefName.ScheduleProvUnassigned);
						bool opShowsDefaultProv=false;
						foreach(Operatory op in ApptDrawing.VisOps) {
							if((op.ProvDentist!=0 && !op.IsHygiene)
								||(op.ProvHygienist!=0 && op.IsHygiene))
							{
								continue;//The operatory has a provider assigned to it
							}
							if(SchedListPeriod.Any(x => x.Ops.Contains(op.OperatoryNum))) {
								continue;//The operatory has a scheduled assigned to it
							}
							opShowsDefaultProv=true;//The operatory will have the provider for unassigned operatories
							break;
						}
						if(opShowsDefaultProv && SchedListPeriod.Exists(x => x.ProvNum==schedProvUnassinged)) {//The provider for unassigned ops has a schedule
							//Add that provider's earliest schedule
							listVisScheds.Add(SchedListPeriod.FindAll(x => x.ProvNum==schedProvUnassinged).OrderBy(x => x.StartTime).FirstOrDefault());
						}
						//Get the appointment times that are in a visible operatory
						List<TimeSpan> listVisAptTimes=new List<TimeSpan>();
						//ContrApptSheet2.AllContrApptSingles has not been updated yet so we must use the DataTable here.
						foreach(DataRow row in  _dtAppointments.Rows) {
							long opNum=PIn.Long(row["Op"].ToString());
							if(!ApptDrawing.VisOps.Exists(x => x.OperatoryNum==opNum) //The appointment is in a visible operatory
								|| !new[] { "1","2","4","5","7","8" }.Contains(row["AptStatus"].ToString())) //Scheduled,Complete,ASAP,Broken,PtNote,PtNoteComp
							{
								continue;
							}
							listVisAptTimes.Add(PIn.Date(row["AptDateTime"].ToString()).TimeOfDay);
						}
						TimeSpan earliestApt=new TimeSpan();
						TimeSpan earliestOp=new TimeSpan();
						if(listVisAptTimes.Count>0 && listVisScheds.Count>0) {//There is at least one schedule and at least one appointment visible
							earliestApt=listVisScheds.Min(x => x.StartTime);
							earliestOp=listVisAptTimes.Min();
							if(TimeSpan.Compare(earliestOp,earliestApt)==1) {//earliestOp is later than earliestApt
								apptTimeScrollStart=earliestApt;
							}
							else {//earliestApt is later than earliestOp or they are both equal
								apptTimeScrollStart=earliestOp;
							}
						}
						else if(listVisScheds.Count>0) {//There is at least one visible schedule and no visible appointments
							apptTimeScrollStart=listVisScheds.Min(x => x.StartTime);
						}
						else if(listVisAptTimes.Count>0) {//There is at least one visible appointment and no visible schedules
							apptTimeScrollStart=listVisAptTimes.Min();
						}
						//else apptTimeScrollStart will remain as the start time listed in the appt view		
					}
					rowsPerHr=60/ApptDrawing.MinPerIncr*_listApptViews[comboView.SelectedIndex-1].RowsPerIncr;//comboView.SelectedIndex-1 because combo box contains none but list does not.
					double apptTimeHrs=((apptTimeScrollStart.Hours*60)+apptTimeScrollStart.Minutes)/60.0;
					if(apptTimeHrs*rowsPerHr*ApptDrawing.LineH<vScrollBar1.Maximum-vScrollBar1.LargeChange) {
						vScrollBar1.Value=(int)(apptTimeHrs*rowsPerHr*ApptDrawing.LineH);
					}
					else {
						vScrollBar1.Value=vScrollBar1.Maximum;
					}
				}
				else if(8*rowsPerHr*ApptDrawing.LineH<vScrollBar1.Maximum-vScrollBar1.LargeChange) {
					vScrollBar1.Value=8*rowsPerHr*ApptDrawing.LineH;//8am
				}
			}
			else if(ContrApptSheet2.Height!=oldHeight && oldHeight > 0) {//Try not to move the scroll bar around when changing between views that have different row increment values.
				//the max prevents setting scroll value to a negative, the min prevents setting scroll value beyond maximum scroll allowed
				vScrollBar1.Value=Math.Min(oldVScrollVal*ContrApptSheet2.Height/oldHeight,Math.Max(vScrollBar1.Maximum-vScrollBar1.LargeChange,0));
			}
			_hasLayedOutScrollBar=true;
			if(vScrollBar1.Value>vScrollBar1.Maximum-vScrollBar1.LargeChange) {
				if(vScrollBar1.Maximum-vScrollBar1.LargeChange>=0) {//but don't allow setting negative number
					vScrollBar1.Value=vScrollBar1.Maximum-vScrollBar1.LargeChange;
				}
			}
			ContrApptSheet2.Location=new Point(0,-vScrollBar1.Value);
			toolTip1.RemoveAll();//without this line, the program becomes sluggish.
			Operatory curOp;
			//If the the Operatory name does not fit on one line, expand panelOps so that two lines will fit
			int panelHeight=17;
			if(!ApptDrawing.IsWeeklyView) {			
				for(int i=0;i<ApptDrawing.ColCount;i++) {
					curOp=ApptDrawing.VisOps[i];
					Size textSize=TextRenderer.MeasureText(curOp.OpName,new Font("Microsoft Sans Serif",8.25f,FontStyle.Regular));
					if(textSize.Width>ApptDrawing.ColWidth) {
						panelHeight=30;//Enough to fit two lines of text
						break;
					}
				}
			}
			panelOps.Height=panelHeight;
			panelSheet.Location=new Point(panelSheet.Location.X,panelOps.Height);
			int totalColumns=0;
			for(int i=0;i<ApptDrawing.ProvCount;i++) {
				OpPanel opPanel;
				if(totalColumns>=_listOpPanels.Count) {
					opPanel=new OpPanel(this,ApptDrawing.VisProvs[i],panelOps.Height,new Point(2+(int)ApptDrawing.TimeWidth+(int)ApptDrawing.ProvWidth*i,0),i==0 ? true : false);
					panelOps.Controls.Add(opPanel.GetPanel());
					_listOpPanels.Add(opPanel);
				}
				else {
					opPanel=_listOpPanels[totalColumns];
					opPanel.ResetProvPanel(this,ApptDrawing.VisProvs[i],panelOps.Height,new Point(2+(int)ApptDrawing.TimeWidth+(int)ApptDrawing.ProvWidth*i,0),i==0 ? true : false);
				}
				toolTip1.SetToolTip(opPanel.GetPanel(),ApptDrawing.VisProvs[i].Abbr);
				totalColumns++;
			}
			if(ApptDrawing.IsWeeklyView) {
				for(int i=0;i<ApptDrawing.NumOfWeekDaysToDisplay;i++) {
					OpPanel opPanel;
					if(totalColumns>=_listOpPanels.Count) {
						opPanel=new OpPanel(this,panelOps.Height,new Point(2+(int)ApptDrawing.TimeWidth+i*(int)ApptDrawing.ColDayWidth,0),i);
						panelOps.Controls.Add(opPanel.GetPanel());
						_listOpPanels.Add(opPanel);
					}
					else {
						opPanel=_listOpPanels[totalColumns];
						opPanel.ResetOpPanelWeekly(this,panelOps.Height,new Point(2+(int)ApptDrawing.TimeWidth+i*(int)ApptDrawing.ColDayWidth,0),i);
					}
					totalColumns++;
				}
			}
			else {
				for(int i=0;i<ApptDrawing.ColCount;i++) {
					curOp=ApptDrawing.VisOps[i];
					OpPanel opPanel;
					if(totalColumns>=_listOpPanels.Count) {
						//We can enhance this later to include Hygienists as an additional option in the menu.
						opPanel=new OpPanel(this,curOp,Providers.GetProv(curOp.ProvDentist),panelOps.Height,new Point(2+(int)(ApptDrawing.TimeWidth+ApptDrawing.ProvWidth*ApptDrawing.ProvCount+i*ApptDrawing.ColWidth),0));
						panelOps.Controls.Add(opPanel.GetPanel());
						_listOpPanels.Add(opPanel);
					}
					else {
						opPanel=_listOpPanels[totalColumns];
						opPanel.ResetOpPanel(this,curOp,Providers.GetProv(curOp.ProvDentist),panelOps.Height,new Point(2+(int)(ApptDrawing.TimeWidth+ApptDrawing.ProvWidth*ApptDrawing.ProvCount+i*ApptDrawing.ColWidth),0));
					}
					panelOps.Controls[totalColumns].Visible=true;
					totalColumns++;
				}
			}
			this.ResumeLayout();
			listConfirmed.Items.Clear();
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.ApptConfirmed,true);
			for(int i=0;i<listDefs.Count;i++) {
				this.listConfirmed.Items.Add(listDefs[i].ItemValue);
			}
		}

		///<summary>Refreshes the appointment info panel to the right if an appointment is currently selected.</summary>
		public void RefreshModuleScreenPatient() {
			if(PatCur==null) {
				panelMakeButtons.Enabled=false;
			}
			else {
				panelMakeButtons.Enabled=true;
			}
			if(ContrApptSingle.SelectedAptNum>0) {
				butUnsched.Enabled=true;
				butBreak.Enabled=true;
				butComplete.Enabled=true;
				butDelete.Enabled=true;
				if(!Security.IsAuthorized(Permissions.ApptConfirmStatusEdit,true)) {//Suppress message because it would be very annoying to users.
					listConfirmed.Enabled=false;
				}
				else {
					listConfirmed.Enabled=true;
				}
			}
			else {
				butUnsched.Enabled=false;
				butBreak.Enabled=false;
				butComplete.Enabled=false;
				butDelete.Enabled=false;
				listConfirmed.Enabled=false;
			}			
			if(panelAptInfo.Enabled) { //Set the selection state of listConfirmed.
				long aptconfirmed=0;
				if(pinBoard.SelectedIndex==-1) {//No pinboard appt selected, use selected appointment.
					ContrApptSingle ctrl=ContrApptSheet2.ListContrApptSingles.FirstOrDefault(x => x.AptNum==ContrApptSingle.SelectedAptNum);
					if(ctrl!=null) {
						aptconfirmed=ctrl.Confirmed;
					}					
				}
				else {//Pinboard appt selected, use it.
					aptconfirmed=pinBoard.SelectedAppt.Confirmed;
				}
				listConfirmed.SelectedIndex=Defs.GetOrder(DefCat.ApptConfirmed,aptconfirmed);//could be -1
			}
			else {
				listConfirmed.SelectedIndex=-1;
			}
		}

		///<summary>Redraws screen based on data already gathered.  RefreshModuleDataPeriod will have already retrieved the data from the db.</summary>
		public void RefreshModuleScreenPeriod() {
			DateTime startDate;
			DateTime endDate;
			if(ApptDrawing.IsWeeklyView) {
				startDate=WeekStartDate;
				endDate=WeekEndDate;
			}
			else {
				startDate=AppointmentL.DateSelected;
				endDate=AppointmentL.DateSelected;
			}
			if(startDate.Year<1880 || endDate.Year<1880) {
				return;
			}
			Calendar2.SetSelectionRange(startDate,endDate);
			ApptDrawing.ProvBar=new int[ApptDrawing.VisProvs.Count][];
			for(int i=0;i<ApptDrawing.VisProvs.Count;i++) {
				ApptDrawing.ProvBar[i]=new int[24*ApptDrawing.RowsPerHr]; //[144]; or 24*6
			}
			//Remove all controls and shadows at this point so we can quickly re-add them below. This will also invalidate the main ContrApptSheet2 shadow.
			ContrApptSheet2.DisposeAppointments();
			labelDate.Text=startDate.ToString("ddd");
			labelDate2.Text=startDate.ToString("-  MMM d");
			//Convert the table rows to ContrApptSingle.
			foreach(DataRow row in _dtAppointments.Rows) {
				if(PIn.Date(row["AptDateTime"].ToString()).Date<startDate.Date || PIn.Date(row["AptDateTime"].ToString()).Date>endDate.Date){
					continue;//Appointment is outside of our date range.
				}
				ContrApptSingle contrApptSingle=new ContrApptSingle(
					row,
					_dtApptFields,
					_dtPatFields,
					ApptSingleDrawing.SetLocation(row,0,ApptDrawing.VisOps.Count,0));
				if(ContrApptSingle.SelectedAptNum==contrApptSingle.AptNum) {//if this is the selected apt
					//if the selected patient was changed from another module, then deselect the apt.
					if(PatCur==null || PatCur.PatNum!=contrApptSingle.PatNum) {
						ContrApptSingle.SelectedAptNum=-1;
					}
				}				
				if(!ApptDrawing.IsWeeklyView) {
					ApptDrawing.ProvBarShading(contrApptSingle.DataRoww);
				}
				//It is very important to add these controls directly to ContrApptSheet2.Controls. See ContrApptSheet.DoubleBufferDraw();
				ContrApptSheet2.Controls.Add(contrApptSingle);
			}//end for
			pinBoard.Invalidate();
			ApptDrawing.SchedListPeriod=SchedListPeriod;
			List<long> opNums = null;
			if(PrefC.HasClinicsEnabled && Clinics.ClinicNum>0) {
				opNums = Operatories.GetOpsForClinic(Clinics.ClinicNum).Select(x => x.OperatoryNum).ToList();
			}
			List<LabCase> labCaseList=LabCases.GetForPeriod(startDate,endDate,opNums);
			FillLab(labCaseList);
			FillProduction();
			FillProductionGoal(startDate,endDate);
			bool hasNotes=true;
			if(PrefC.IsODHQ) {//HQ
				hasNotes=Security.IsAuthorized(Permissions.Schedules,true);
			}
			FillProvSched(hasNotes);
			FillEmpSched(hasNotes);
			FillWaitingRoom();
			LayoutPanels();
			//It is safe to perform drawing from a thread from this point.
			ContrApptSheet2.RedrawAll(true);
		}

		///<summary>This is public so that FormOpenDental can pass refreshed tasks here in order to avoid an extra query.</summary>
		public void RefreshReminders(List <Task> listReminderTasks) {
			Logger.LogToPath("",LogPath.Signals,LogPhase.Start);
			List<Task> listSortedReminderTasks=listReminderTasks
				.Where(x => x.DateTimeEntry.Date <= DateTimeOD.Today)
				.OrderBy(x => x.DateTimeEntry)
				.ToList();
			tabReminders.Text=Lan.g(this,"Reminders");
			if(listSortedReminderTasks.Count > 0) {
				tabReminders.Text+="*";
			}
			gridReminders.BeginUpdate();
			if(gridReminders.Columns.Count==0) {
				gridReminders.Columns.Clear();
				ODGridColumn col=new ODGridColumn("",17);//The status column showing new/viewed in a checkbox.
				col.ImageList=imageListTasks;
				gridReminders.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableTasks","Description"),200);//any width
				gridReminders.Columns.Add(col);
			}
			gridReminders.Rows.Clear();
			for(int i=0;i<listSortedReminderTasks.Count;i++) {
				ODGridRow row=new ODGridRow();
				SetReminderGridRow(row,listSortedReminderTasks[i]);
				gridReminders.Rows.Add(row);
			}
			gridReminders.EndUpdate();
			Logger.LogToPath("",LogPath.Signals,LogPhase.End);
		}

		///<summary>This logic mimics filling a row within UserControlTasks.FillGrid().
		///However, the logic is simpler here because we are only dealing with reminders.</summary>
		private void SetReminderGridRow(ODGridRow row,Task reminderTask) {
			row.Tag=reminderTask;
			row.Cells.Clear();
			string dateStr="";
			if(reminderTask.DateTask.Year>1880) {
				if(reminderTask.DateType==TaskDateType.Day) {
					dateStr+=reminderTask.DateTask.ToShortDateString()+" - ";
				}
				else if(reminderTask.DateType==TaskDateType.Week) {
					dateStr+=Lan.g(this,"Week of")+" "+reminderTask.DateTask.ToShortDateString()+" - ";
				}
				else if(reminderTask.DateType==TaskDateType.Month) {
					dateStr+=reminderTask.DateTask.ToString("MMMM")+" - ";
				}
			}
			else if(reminderTask.DateTimeEntry.Year>1880) {
				dateStr+=reminderTask.DateTimeEntry.ToShortDateString()+" "+reminderTask.DateTimeEntry.ToShortTimeString()+" - ";
			}
			string objDesc="";
			if(reminderTask.TaskStatus==TaskStatusEnum.Done){
				objDesc=Lan.g(this,"Done:")+reminderTask.DateTimeFinished.ToShortDateString()+" - ";
			}
			if(reminderTask.ObjectType==TaskObjectType.Patient) {
				if(reminderTask.KeyNum!=0) {
					objDesc+=Patients.GetPat(reminderTask.KeyNum).GetNameLF()+" - ";
				}
			}
			else if(reminderTask.ObjectType==TaskObjectType.Appointment) {
				if(reminderTask.KeyNum!=0) {
					Appointment AptCur=Appointments.GetOneApt(reminderTask.KeyNum);
					if(AptCur!=null) {
						objDesc=Patients.GetPat(AptCur.PatNum).GetNameLF()//this is going to stay. Still not optimized, but here at HQ, we don't use it.
							+"  "+AptCur.AptDateTime.ToString()
							+"  "+AptCur.ProcDescript
							+"  "+AptCur.Note
							+" - ";
					}
				}
			}
			if(!reminderTask.Descript.StartsWith("==") && reminderTask.UserNum!=0) {
				objDesc+=Userods.GetName(reminderTask.UserNum)+" - ";
			}
			if(PrefC.GetBool(PrefName.TasksNewTrackedByUser)) {//The new way
				if(reminderTask.TaskStatus==TaskStatusEnum.Done) {
					row.Cells.Add("1");
				}
				else {
					if(reminderTask.IsUnread) {
						row.Cells.Add("4");
					}
					else{
						row.Cells.Add("2");
					}
				}
			}
			else {
				switch(reminderTask.TaskStatus) {
					case TaskStatusEnum.New:
						row.Cells.Add("4");
						break;
					case TaskStatusEnum.Viewed:
						row.Cells.Add("2");
						break;
					case TaskStatusEnum.Done:
						row.Cells.Add("1");
						break;
				}
			}
			row.Cells.Add(dateStr+objDesc+reminderTask.Descript);
			//No need to do any text detection for triage priorities, we'll just use the task priority colors.
			row.ColorBackG=Defs.GetColor(DefCat.TaskPriorities,reminderTask.PriorityDefNum);
		}

		///<summary>The logic for this function was copied from UserControlTasks.gridMain_MouseDown() and modified slightly for this scenaro.</summary>
		private void gridReminders_MouseDown(object sender,MouseEventArgs e) {
			int clickedI=gridReminders.PointToRow(e.Y);
			int clickedCol=gridReminders.PointToCol(e.X);
			if(clickedI==-1){
				return;
			}
			gridReminders.SetSelected(clickedI,true);//if right click.
			if(e.Button!=MouseButtons.Left) {
				return;
			}
			ODGridRow row=gridReminders.Rows[clickedI];
			Task reminderTask=((Task)row.Tag).Copy();
			if(clickedCol==0){//check tasks off
				if(PrefC.GetBool(PrefName.TasksNewTrackedByUser)) {
					long userNumInbox=TaskLists.GetMailboxUserNum(reminderTask.TaskListNum);
					if(userNumInbox != 0 && userNumInbox != Security.CurUser.UserNum) {
						MsgBox.Show(this,"Not allowed to mark off tasks in someone else's inbox.");
						return;
					}
					//might not need to go to db to get this info 
					//might be able to check this:
					//if(task.IsUnread) {
					//But seems safer to go to db.
					if(TaskUnreads.IsUnread(Security.CurUser.UserNum,reminderTask.TaskNum)) {
						TaskUnreads.SetRead(Security.CurUser.UserNum,reminderTask.TaskNum);
						reminderTask.TaskStatus=TaskStatusEnum.Viewed;
						gridReminders.BeginUpdate();
						SetReminderGridRow(row,reminderTask);//To get the status to immediately show up in the reminders grid.
						gridReminders.EndUpdate();
						DataValid.SetInvalidTask(reminderTask.TaskNum,false);
					}
					//if already read, nothing else to do.  If done, nothing to do
				}
				else {
					if(reminderTask.TaskStatus==TaskStatusEnum.New) {
						Task taskOld=reminderTask.Copy();
						reminderTask.TaskStatus=TaskStatusEnum.Viewed;
						try {
							Tasks.Update(reminderTask,taskOld);
							gridReminders.BeginUpdate();
							SetReminderGridRow(row,reminderTask);//To get the status to immediately show up in the reminders grid.
							gridReminders.EndUpdate();
							DataValid.SetInvalidTask(reminderTask.TaskNum,false);
						}
						catch(Exception ex) {
							MessageBox.Show(ex.Message);
							return;
						}
					}
					//no longer allowed to mark done from here
				}
			}
		}

		///<summary>Logic mimics UserControlTasks.gridMain_CellDoubleClick()</summary>
		private void gridReminders_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(e.Col==0){
				//no longer allow double click on checkbox, because it's annoying.
				return;
			}
			ODGridRow row=gridReminders.Rows[e.Row];
			Task reminderTask=((Task)row.Tag);
			//It's important to grab the task directly from the db because the status in this list is fake, being the "unread" status instead.
			Task task=Tasks.GetOne(reminderTask.TaskNum);
			FormTaskEdit FormT=new FormTaskEdit(task,task.Copy());
			FormT.Show();//non-modal
		}
		
		///<summary>Logic mimics UserControlTasks.SetMenusEnabled()</summary>
		private void menuReminderEdit_Popup(object sender,EventArgs e) {
			if(gridReminders.GetSelectedIndex()==-1) {
				return;
			}
			Task task=(Task)gridReminders.Rows[gridReminders.GetSelectedIndex()].Tag;
			menuItemReminderGoto.Enabled=true;
			if(task.ObjectType==TaskObjectType.None) {
				menuItemReminderGoto.Enabled=false;
			}
		}

		///<summary>Logic mimics UserControlTasks.DoneClicked()</summary>
		private void menuItemReminderDone_Click(object sender,EventArgs e) {
			if(gridReminders.GetSelectedIndex()==-1) {
				return;
			}
			Task task=(Task)gridReminders.Rows[gridReminders.GetSelectedIndex()].Tag;
			Task oldTask=task.Copy();
			task.TaskStatus=TaskStatusEnum.Done;
			if(task.DateTimeFinished.Year<1880) {
				task.DateTimeFinished=DateTime.Now;
			}
			try {
				Tasks.Update(task,oldTask);
			}
			catch(Exception ex) {
				//Revert the changes to the task because something went wrong.
				task.TaskStatus=oldTask.TaskStatus;
				task.DateTimeFinished=oldTask.DateTimeFinished;
				MessageBox.Show(ex.Message);
				return;
			}
			TaskUnreads.DeleteForTask(task.TaskNum);
			TaskHist taskHist=new TaskHist(oldTask);
			taskHist.UserNumHist=Security.CurUser.UserNum;
			TaskHists.Insert(taskHist);
			DataValid.SetInvalidTask(task.TaskNum,false);
			gridReminders.BeginUpdate();
			gridReminders.Rows.RemoveAt(gridReminders.GetSelectedIndex());
			gridReminders.EndUpdate();
		}

		///<summary>Logic mimics UserControlTasks.GoTo_Clicked()</summary>
		private void menuItemReminderGoto_Click(object sender,EventArgs e) {
			if(gridReminders.GetSelectedIndex()==-1) {
				return;
			}
			Task task=(Task)gridReminders.Rows[gridReminders.GetSelectedIndex()].Tag;
			FormOpenDental.S_TaskGoTo(task.ObjectType,task.KeyNum);
		}

		///<summary>Only used when viewing weekly.</summary>
		private void _backPanel_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			if(e.Button!=MouseButtons.Left) {
				return;
			}
			object tag;
			if(sender.GetType()==typeof(Panel)) {
				tag=((Panel)sender).Tag;
			}
			else {
				tag=((Label)sender).Tag;
			}
			if(tag.GetType() != typeof(int)) {
				return;
			}
			int dayI=(int)tag;
			AppointmentL.DateSelected=WeekStartDate.AddDays(dayI);
			SetWeeklyView(false);
		}

		///<summary>On right click of the operatory panel display the scheduled provider's information in a new menu.</summary>
		private void OpPanelProv_MouseDown(object sender,MouseEventArgs e) {
			_menuOp.Items.Clear();
			object tag;
			if(sender.GetType()==typeof(Panel)) {
				tag=((Panel)sender).Tag;
			}
			else {
				tag=((Label)sender).Tag;
			}
			if(tag.GetType() != typeof(Tuple<Provider,Operatory>)) {
				return;
			}
			Tuple<Provider,Operatory> tupleProvOp=(Tuple<Provider,Operatory>)tag;
			Provider prov=tupleProvOp.Item1;
			Operatory op=tupleProvOp.Item2;
			if(op==null) {//Should never happen.  If it does there is nothing to do
				return;
			}
			//If no provider is associated with an operatory inform the user
			if(prov==null) {
				ToolStripMenuItem noProv=new ToolStripMenuItem(("There is no provider associated with this operatory."));
				_menuOp.Items.Add(noProv);
			}
			else {//Provider associated to the Operatory
				Def defaultProvDef=Defs.GetDef(DefCat.ProviderSpecialties,prov.Specialty);
				if(defaultProvDef!=null) {
					ToolStripMenuItem strip=new ToolStripMenuItem(("Default: "+prov.FName+" "+prov.LName+", "+defaultProvDef.ItemName));
					_menuOp.Items.Add(strip);
				}
			}
			//Using opNum and the selected date to grab a list of ScheduleOps.
			List<Schedule> listScheds=Schedules.GetSchedsForOp(ApptDrawing.SchedListPeriod,op);
			Provider scheduledProv;
			//Get the provider for each ScheduleOp and populate the menu with the provider's info.
			foreach(Schedule scheduleCur in listScheds) {
				scheduledProv=Providers.GetProv(scheduleCur.ProvNum);
				if(scheduledProv==null) {
					return;
				}
				Def scheduledProvDef=Defs.GetDef(DefCat.ProviderSpecialties,scheduledProv.Specialty);
				string stripText=Lans.g(this,"Scheduled")+" "+scheduleCur.StartTime.ToShortTimeString()+"-"+scheduleCur.StopTime.ToShortTimeString()+": "+scheduledProv.GetFormalName()+", "+scheduledProvDef.ItemName;
				stripText+=!string.IsNullOrWhiteSpace(scheduledProv.SchedNote) ? "\r\n\t("+scheduledProv.SchedNote+")" : "";
				ToolStripMenuItem scheduledStrip=new ToolStripMenuItem(stripText);
				_menuOp.Items.Add(scheduledStrip);
			}
			if(PrefC.HasClinicsEnabled && Clinics.ClinicNum!=0) {//HQ can not have clinic specialties.
				Clinic clinicCur=Clinics.GetClinic(Clinics.ClinicNum);
				string clinSchedText=clinicCur.SchedNote;
				if(!string.IsNullOrEmpty(clinSchedText)) {
					if(_menuOp.Items.Count>0) {
						_menuOp.Items.Add("-");
					}
					List<DefLink> listSpecialties=DefLinks.GetListByFKey(clinicCur.ClinicNum,DefLinkType.Clinic);
					_menuOp.Items.Add(new ToolStripMenuItem(string.Join(", ",listSpecialties.Select(x => Defs.GetName(DefCat.ClinicSpecialty,x.DefNum)))
						+"\r\n"+clinSchedText));
				}
			}
			if(sender.GetType()==typeof(Panel)) {
				_menuOp.Show(((Panel)sender),new Point(e.X,e.Y));
			}
			else {
				_menuOp.Show(((Label)sender),new Point(e.X,e.Y));
			}
		}

		///<summary>Sets the ContrApptSingle array to null.</summary>
		public void ModuleUnselected() {
			//Cleanup the resources.
			ContrApptSheet2.DisposeAppointments();			
			Plugins.HookAddCode(this,"ContrAppt.ModuleUnselected_end");
		}
		
		/*This was resulting in too many firings of ModuleSelected
		///<summary>Currently only used when comboView really does change.  Otherwise, just call ModuleSelected.  Triggered in FunctionKeyPress, SetView, and  FillViews</summary>
		private void comboView_SelectedIndexChanged(object sender,System.EventArgs e) {
			if(PatCur==null) {
				ModuleSelected(0);
			}
			else {
				ModuleSelected(PatCur.PatNum);
			}
		}*/

		///<summary></summary>
		private void comboView_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboView.SelectedIndex==0) {
				SetView(0,true);
			}
			else {
				SetView(_listApptViews[comboView.SelectedIndex-1].ApptViewNum,true);
			}
		}

		///<summary>Not used</summary>
		private void ContrAppt_Layout(object sender,System.Windows.Forms.LayoutEventArgs e) {
			//This event actually happens quite frequently: once for every appointment placed on the screen.
			//Moved it all to Resize event.
		}

		///<summary>Might not be getting called enough.</summary>
		private void ContrAppt_Resize(object sender,System.EventArgs e) {
			//This didn't work so well.  Very slow and caused program to not be able to unminimize
			try {//so it doesn't crash if not connected to DB
				//ModuleSelected();
			}
			catch { }
			//even though the part above didn't work, we're going to try adding the stuff below.  It was important to get it out
			//of the Layout event, which fires very frequently.
			LayoutPanels();
		}

		///<summary>This might not be getting called frequently enough.  Done on resize and when refreshing the period.  But it used to be done very very frequently.</summary>
		private void LayoutPanels() {
			if(Calendar2.Width>panelCalendar.Width) {//for example, Chinese calendar
				panelCalendar.Width=Calendar2.Width+1;
				//panelAptInfo.Width=Calendar2.Width+1;
			}
			//Assumes widths of the first 2 panels were set the same in the designer,
			ToolBarMain.Location=new Point(ClientSize.Width-panelCalendar.Width-2,0);
			panelCalendar.Location=new Point(ClientSize.Width-panelCalendar.Width-2,ToolBarMain.Height);
			panelAptInfo.Location=new Point(ClientSize.Width-panelCalendar.Width-2,ToolBarMain.Height+panelCalendar.Height);
			//butOther.Location=new Point(panelAptInfo.Location.X+32,panelAptInfo.Location.Y+84);
			panelMakeButtons.Location=new Point(panelAptInfo.Right+2,panelAptInfo.Top);
			panelSheet.Width=ClientSize.Width-panelCalendar.Width-2;
			panelSheet.Height=ClientSize.Height-panelSheet.Location.Y;
			tabControl.Location=new Point(panelAptInfo.Left,panelAptInfo.Bottom+1);
			if(tabControl.Top>panelSheet.Bottom) {
				tabControl.Height=0;
			}
			else {
				tabControl.Height=panelSheet.Height-tabControl.Top+21;
			}
			if(!Defs.GetDictIsNull()) {
				ApptView viewCur=null;
				if(comboView.SelectedIndex>0) {
					viewCur=_listApptViews[comboView.SelectedIndex-1];
				}
				ApptViewItemL.GetForCurView(viewCur,ApptDrawing.IsWeeklyView,SchedListPeriod);//refreshes visops,etc
				ApptDrawing.ApptSheetWidth=panelSheet.Width-vScrollBar1.Width;
				ApptDrawing.ComputeColWidth(0);
			}
			panelOps.Width=panelSheet.Width;
		}

		///<summary>Called from FormOpenDental upon startup.</summary>
		public void InitializeOnStartup() {
			//jsparks-
			//This method was inefficient and was causing 4 refreshes: RefreshPeriod, FillViews->comboView_SelectedIndexChanged, SetView?, SetWeeklyView.
			//This was especially inefficient, because after calling this method, FormOD was refreshing this module anyway.  So about 5 refreshes on startup.
			//Now, InitializedOnStartup remains false until the end of this method, preventing all refreshes when inside this method.
			//Verified that it only does one RefreshPeriod call to the db.
			if(InitializedOnStartup) {
				return;
			}
			LayoutPanels();
			ApptDrawing.RowsPerIncr=1;
			AppointmentL.DateSelected=DateTime.Now;
			ContrApptSingle.SelectedAptNum=-1;
			//RefreshPeriod();//Don't think this is needed.
			FillViews();//this will load the recently used ApptView and set comboView.SelectedIndex and calls ModuleSelected
			menuWeeklyApt.MenuItems.Clear();
			menuWeeklyApt.MenuItems.Add(Lan.g(this,"Copy to Pinboard"),new EventHandler(menuWeekly_Click));
			menuApt.MenuItems.Clear();
			menuApt.MenuItems.Add(Lan.g(this,"Copy to Pinboard"),new EventHandler(menuApt_Click));
			menuApt.MenuItems.Add("-");
			menuApt.MenuItems.Add(Lan.g(this,"Send to Unscheduled List"),new EventHandler(menuApt_Click));
			menuItemBreakAppt=menuApt.MenuItems.Add(Lan.g(this,"Break Appointment"),new EventHandler(menuApt_Click));
			menuApt.MenuItems.Add(Lan.g(this,"Mark as ASAP"),new EventHandler(OnASAP_Click));
			menuApt.MenuItems.Add(Lan.g(this,"Set Complete"),new EventHandler(menuApt_Click));
			menuApt.MenuItems.Add(Lan.g(this,"Delete"),new EventHandler(menuApt_Click));
			menuApt.MenuItems.Add(Lan.g(this,"Other Appointments"),new EventHandler(menuApt_Click));
			menuApt.MenuItems.Add("-");
			menuApt.MenuItems.Add(Lan.g(this,"Print Label"),new EventHandler(menuApt_Click));
			menuApt.MenuItems.Add(Lan.g(this,"Print Card"),new EventHandler(menuApt_Click));
			menuApt.MenuItems.Add(Lan.g(this,"Print Card for Entire Family"),new EventHandler(menuApt_Click));
			menuApt.MenuItems.Add(Lan.g(this,"Routing Slip"),new EventHandler(menuApt_Click));
			menuBlockout.MenuItems.Clear();
			menuBlockout.MenuItems.Add(Lan.g(this,"Edit Blockout"),OnBlockEdit_Click);
			menuBlockout.MenuItems.Add(Lan.g(this,"Cut Blockout"),OnBlockCut_Click);
			menuBlockout.MenuItems.Add(Lan.g(this,"Copy Blockout"),OnBlockCopy_Click);
			menuBlockout.MenuItems.Add(Lan.g(this,"Paste Blockout"),OnBlockPaste_Click);
			menuBlockout.MenuItems.Add(Lan.g(this,"Delete Blockout"),OnBlockDelete_Click);
			menuBlockout.MenuItems.Add(Lan.g(this,"Add Blockout"),OnBlockAdd_Click);
			menuBlockout.MenuItems.Add(Lan.g(this,"Blockout Cut-Copy-Paste"),OnBlockCutCopyPaste_Click);
			menuBlockout.MenuItems.Add(Lan.g(this,"Clear All Blockouts for Day"),OnClearBlockouts_Click);
			menuBlockout.MenuItems.Add(Lan.g(this,"Clear All Blockouts for Day, Op only"),OnClearBlockoutsOp_Click);
			if(PrefC.HasClinicsEnabled) {
				menuBlockout.MenuItems.Add(Lan.g(this,"Clear All Blockouts for Day, Clinic only"),OnClearBlockoutsClinic_Click);
			}
			menuBlockout.MenuItems.Add(Lan.g(this,"Edit Blockout Types"),OnBlockTypes_Click);
			menuBlockout.MenuItems.Add("-");//Designer code to insert a horizontal separator
			MenuItem menuItem=menuBlockout.MenuItems.Add(Lan.g(this,"Text ASAP List"),OnTextASAPList_Click);
			menuItem.Name=MenuItemNames.TextAsapList;
			menuBlockout.MenuItems.Add(Lan.g(this,"Update Provs on Future Appts"),OnUpdateProvs_Click);
			Lan.C(this,new Control[]
				{
				butToday,
				//butTodayWk,
				butSearch,
				butClearPin,
				label2,
				label7,
				butMakeAppt,
				butMakeRecall,
				butFamRecall,
				butViewAppts,
				radioDay,
				radioWeek,
				tabWaiting,
				tabSched,
				tabProv,
				butLab,
				butBackWeek,
				butBackMonth,
				butFwdWeek,
				butFwdMonth,
				gridEmpSched,
				gridWaiting,
				gridProv
				});
			LayoutToolBar();
			//Appointment action buttons
			toolTip1.SetToolTip(butUnsched,Lan.g(this,"Send to Unscheduled List"));
			toolTip1.SetToolTip(butBreak,Lan.g(this,"Break"));
			toolTip1.SetToolTip(butComplete,Lan.g(this,"Set Complete"));
			toolTip1.SetToolTip(butDelete,Lan.g(this,"Delete"));
			//toolTip1.SetToolTip(butOther,Lan.g(this,"Other Appointments"));
			if(PrefC.GetString(PrefName.RegistrationKey).StartsWith("UPR6J92T29")) {
				butGraph.Visible=true;
			}
			SetWeeklyView(PrefC.GetBool(PrefName.ApptModuleDefaultToWeek));
			InitializedOnStartup=true;//moved this down to prevent view setting from triggering ModuleSelected().
		}

		///<summary></summary>
		public void LayoutToolBar() {
			ToolBarMain.Buttons.Clear();
			//ODToolBarButton button;
			//button=new ODToolBarButton("",0,Lan.g(this,"Select Patient"),"Patient");
			//button.Style=ODToolBarButtonStyle.DropDownButton;
			//button.DropDownMenu=menuPatient;
			//ToolBarMain.Buttons.Add(button);
			ToolBarMain.Buttons.Add(new ODToolBarButton("",2,Lan.g(this,"Appointment Lists"),"Lists"));
			ToolBarMain.Buttons.Add(new ODToolBarButton("",1,Lan.g(this,"Print Schedule"),"Print"));
			if(!ProgramProperties.IsAdvertisingDisabled(ProgramName.RapidCall)) {
				ToolBarMain.Buttons.Add(new ODToolBarButton("",3,Lan.g(this,"Rapid Call"),"RapidCall"));
			}
			ProgramL.LoadToolbar(ToolBarMain,ToolBarsAvail.ApptModule);
			ToolBarMain.Invalidate();
			Plugins.HookAddCode(this,"ContrAppt.LayoutToolBar_end",PatCur);
		}

		///<summary>Not in use.  See InstantClasses instead.</summary>
		private void ContrAppt_Load(object sender,System.EventArgs e) {

		}

		///<summary>The key press from the main form is passed down to this module.  This is guaranteed to be between the keys of F1 and F12.</summary>
		public void FunctionKeyPress(Keys keys) {
			string keyName=Enum.GetName(typeof(Keys),keys);//keyName will be F1, F2, ... F12
			int fKeyVal=int.Parse(keyName.TrimStart('F'));//strip off the F and convert to an int
			if(_listApptViews.Count<fKeyVal) {
				return;
			}
			SetView(_listApptViews[fKeyVal-1].ApptViewNum,true);
		}

		/// <summary>Sets the index of comboView for the specified ApptViewNum.  Then, does a ModuleSelected().  If saveToDb, then it will remember the ApptViewNum and currently selected ClinicNum for this workstation.</summary>
		private void SetView(long apptViewNum,bool saveToDb) {
			comboView.SelectedIndex=0;
			for(int i=0;i<_listApptViews.Count;i++) {
				if(apptViewNum==_listApptViews[i].ApptViewNum) {
					comboView.SelectedIndex=i+1;//+1 for 'none'
					break;
				}
			}
			if(!InitializedOnStartup) {
				return;//prevent ModuleSelected().
			}
			if(InitializedOnStartup && !Visible) {
				return;
			}
			if(saveToDb) {
				ComputerPrefs.LocalComputer.ApptViewNum=apptViewNum;
				ComputerPrefs.LocalComputer.ClinicNum=Clinics.ClinicNum;
				ComputerPrefs.Update(ComputerPrefs.LocalComputer);
				UserodApptViews.InsertOrUpdate(Security.CurUser.UserNum,Clinics.ClinicNum,apptViewNum);
			}
			if(PatCur==null) {
				ModuleSelected(0,listOpNums:ApptViewItems.GetOpsForView(apptViewNum),listProvNums:ApptViewItems.GetProvsForView(apptViewNum));
			}
			else {
				ModuleSelected(PatCur.PatNum,listOpNums:ApptViewItems.GetOpsForView(apptViewNum),listProvNums:ApptViewItems.GetProvsForView(apptViewNum));
			}
		}
		
		///<summary>Fills comboView and _listApptViews with the current list of views.  Also called from FormOpenDental.RefreshLocalData().</summary>
		public void FillViews() {
			comboView.Items.Clear();
			_listApptViews.Clear();
			comboView.Items.Add(Lan.g(this,"none"));
			string f="";
			foreach(ApptView apptView in ApptViews.GetDeepCopy()) {
				if(!PrefC.GetBool(PrefName.EasyNoClinics) && Clinics.ClinicNum!=apptView.ClinicNum) {
					//This is intentional, we do NOT want 'Headquarters' to have access to clinic specific apptviews.  
					//Likewise, we do not want clinic specific views to be accessible from specific clinic filters.
					continue;
				}
				_listApptViews.Add(apptView.Copy());
				if(_listApptViews.Count<=12)
					f="F"+_listApptViews.Count.ToString()+"-";
				else
					f="";
				comboView.Items.Add(f+apptView.Description);
			}
			ApptView apptViewCur=GetApptViewForUser();
			if(apptViewCur!=null) {
				SetView(apptViewCur.ApptViewNum,false);//this also triggers ModuleSelected()
			}
			else {
				SetView(0,false);//this also triggers ModuleSelected()
			}
		}

		///<summary>Returns an ApptView for the currently logged in user and clinic combination. Can return null.
		///Will return the first available appointment view if this is the first time that this computer has connected to this database.</summary>
		private ApptView GetApptViewForUser() {
			//load the recently used apptview from the db, either the userodapptview table if an entry exists or the computerpref table if an entry for this computer exists
			ApptView apptViewCur=null;
			UserodApptView userodApptViewCur=UserodApptViews.GetOneForUserAndClinic(Security.CurUser.UserNum,Clinics.ClinicNum);
			if(userodApptViewCur!=null) { //if there is an entry in the userodapptview table for this user
				if(InitializedOnStartup //if either ContrAppt has already been initialized
					|| (Security.CurUser.ClinicIsRestricted //or the current user is restricted
					&& Clinics.ClinicNum!=ComputerPrefs.LocalComputer.ClinicNum)) //and FormOpenDental.ClinicNum (set to the current user's clinic) is not the computerpref clinic
				{
					apptViewCur=ApptViews.GetApptView(userodApptViewCur.ApptViewNum); //then load the view for the user in the userodapptview table
				}
			}
			if(apptViewCur==null //if no entry in the userodapptview table
				&& Clinics.ClinicNum==ComputerPrefs.LocalComputer.ClinicNum) //and if the program level ClinicNum is the stored recent ClinicNum for this computer 
			{
				apptViewCur=ApptViews.GetApptView(ComputerPrefs.LocalComputer.ApptViewNum);//use the computerpref for this computer and user
			}
			//Larger offices do not want to take the time to load all the data required to display the "none" view.
			//Therefore, for a NEW computer that is connecting to the database for the first time, load up the first available view that is not the none view.
			if(apptViewCur==null //if no entry in the ComputerPref table
				&& _listApptViews.Count>0) //There is an appointment view other than "none" to select
			{ 
				apptViewCur=_listApptViews[0];
			}
			return apptViewCur;
		}

		private long GetApptViewNumForUser() {
			long apptViewNum=0;
			//First time loading the Appt Module for non-clinic users
			if(ApptViewItemL.ApptViewCur==null) {
				//GetApptViewForUser needs a CurUser to the specific appointment views for the clinic/user combination
				if(Security.CurUser!=null) {
					//Can return null here, which will cause apptViewNum to remain 0.
					ApptView apptViewCur=GetApptViewForUser();
					if(apptViewCur!=null) {
						apptViewNum=apptViewCur.ApptViewNum;
					}
				}
				else {
					//No valid user so the appointment view will be set to whatever the computerpref table has for the current computer
					apptViewNum=ComputerPrefs.LocalComputer.ApptViewNum;
				}
			}
			else {
				//Has been filled before, so it is safe to call
				apptViewNum=ApptViewItemL.ApptViewCur.ApptViewNum;
			}
			return apptViewNum;
		}

		///<summary>Clicked today.</summary>
		private void butToday_Click(object sender,System.EventArgs e) {
			AppointmentL.DateSelected=DateTimeOD.Today;
			SetWeeklyView(radioWeek.Checked);
		}

		///<summary>Clicked back one day.</summary>
		private void butBack_Click(object sender,System.EventArgs e) {
			AppointmentL.DateSelected=AppointmentL.DateSelected.AddDays(-1);
			SetWeeklyView(radioWeek.Checked);
		}

		///<summary>Clicked forward one day.</summary>
		private void butFwd_Click(object sender,System.EventArgs e) {
			AppointmentL.DateSelected=AppointmentL.DateSelected.AddDays(1);
			SetWeeklyView(radioWeek.Checked);
		}

		private void butBackMonth_Click(object sender,EventArgs e) {
			AppointmentL.DateSelected=AppointmentL.DateSelected.AddMonths(-1);
			SetWeeklyView(radioWeek.Checked);
		}

		private void butBackWeek_Click(object sender,EventArgs e) {
			AppointmentL.DateSelected=AppointmentL.DateSelected.AddDays(-7);
			SetWeeklyView(radioWeek.Checked);
		}

		private void butFwdWeek_Click(object sender,EventArgs e) {
			AppointmentL.DateSelected=AppointmentL.DateSelected.AddDays(7);
			SetWeeklyView(radioWeek.Checked);
		}

		private void butFwdMonth_Click(object sender,EventArgs e) {
			AppointmentL.DateSelected=AppointmentL.DateSelected.AddMonths(1);
			SetWeeklyView(radioWeek.Checked);
		}

		private void radioDay_Click(object sender,EventArgs e) {
			SetWeeklyView(false);
		}

		private void radioWeek_Click(object sender,EventArgs e) {
			SetWeeklyView(true);
		}

		///<summary>Clicked a date on the calendar.</summary>
		private void Calendar2_DateSelected(object sender,System.Windows.Forms.DateRangeEventArgs e) {
			AppointmentL.DateSelected=Calendar2.SelectionStart;
			SetWeeklyView(radioWeek.Checked);
		}

		///<summary>Switches between weekly view and daily view.  AppointmentL.DateSelected needs to be set first.  Calculates WeekStartDate and WeekEndDate based on AppointmentL.DateSelected.  Then calls either RefreshPeriod or ModuleSelected.</summary>
		private void SetWeeklyView(bool isWeeklyView) {
			//if the weekly view doesn't change, then just RefreshPeriod
			bool weeklyViewChanged=false;
			if(isWeeklyView!=ApptDrawing.IsWeeklyView) {
				weeklyViewChanged=true;
			}
			//for those few times when the radiobuttons aren't quite right:
			if(isWeeklyView) {
				radioWeek.Checked=true;
				butFwd.Enabled=false;
				butBack.Enabled=false;
			}
			else {
				radioDay.Checked=true;
				butFwd.Enabled=true;
				butBack.Enabled=true;
			}
			if((int)AppointmentL.DateSelected.DayOfWeek==0) {//if sunday
				WeekStartDate=AppointmentL.DateSelected.AddDays(-6).Date;//go back to previous monday
			}
			else {
				WeekStartDate=AppointmentL.DateSelected.AddDays(1-(int)AppointmentL.DateSelected.DayOfWeek).Date;//go back to current monday
			}
			WeekEndDate=WeekStartDate.AddDays(ApptDrawing.NumOfWeekDaysToDisplay-1).Date;
			ApptDrawing.IsWeeklyView=isWeeklyView;
			if(!InitializedOnStartup) {
				return;//prevent refreshing repeatedly on startup
			}
			long apptViewNum=0;
			if(ApptViewItemL.ApptViewCur!=null) {
				apptViewNum=ApptViewItemL.ApptViewCur.ApptViewNum;
			}
			if(weeklyViewChanged || isWeeklyView) {
				if(PatCur==null) {
					ModuleSelected(0,listOpNums:ApptViewItems.GetOpsForView(apptViewNum),listProvNums:ApptViewItems.GetProvsForView(apptViewNum));
				}
				else {
					ModuleSelected(PatCur.PatNum,listOpNums:ApptViewItems.GetOpsForView(apptViewNum),listProvNums:ApptViewItems.GetProvsForView(apptViewNum));
				}
			}
			else {
				RefreshPeriod(listOpNums:ApptViewItems.GetOpsForView(apptViewNum),listProvNums:ApptViewItems.GetProvsForView(apptViewNum),isRefreshSchedules:true);
			}
		}

		///<summary>Fills the lab summary for the day.</summary>
		private void FillLab(List<LabCase> labCaseList) {
			int notRec=0;
			for(int i=0;i<labCaseList.Count;i++) {
				if(labCaseList[i].DateTimeChecked.Year>1880) {
					continue;
				}
				if(labCaseList[i].DateTimeRecd.Year>1880) {
					continue;
				}
				notRec++;
			}
			if(notRec==0) {
				textLab.Font=new Font(FontFamily.GenericSansSerif,8,FontStyle.Regular);
				textLab.ForeColor=Color.Black;
				textLab.Text=Lan.g(this,"All Received");
			}
			else {
				textLab.Font=new Font(FontFamily.GenericSansSerif,8,FontStyle.Bold);
				textLab.ForeColor=Color.DarkRed;
				textLab.Text=notRec.ToString()+Lan.g(this," NOT RECEIVED");
			}
		}

		///<summary>Fills the production summary for the day. ContrApptSheet2.Controls should be current with ContrApptSingle(s) for the select Op and date.</summary>
		private void FillProduction() {
			if(!ApptViewItemL.ApptRows.Exists(x => x.ElementDesc=="Production")) {
				textProduction.Text="";
				return;
			}
			decimal grossproduction=0;
			decimal netproduction=0;
			int indexProv;
			foreach(ContrApptSingle ctrl in ContrApptSheet2.ListContrApptSingles) { 
				indexProv=-1;
				if(PrefC.GetBool(PrefName.ApptModuleProductionUsesOps)) {
					if(ctrl.IsHygiene) {
						if(ctrl.ProvHyg==0) {//if no hyg prov set.
							indexProv=ApptDrawing.GetIndexOp(ctrl.OpNum);
						}
						else {
							indexProv=ApptDrawing.GetIndexOp(ctrl.OpNum);
						}
					}
					else {//not hyg
						indexProv=ApptDrawing.GetIndexOp(ctrl.OpNum);
					}
				}
				else {//use provider bars in appointment view
					if(ctrl.IsHygiene) {
						if(ctrl.ProvHyg==0) {//if no hyg prov set.
							indexProv=ApptDrawing.GetIndexProv(ctrl.ProvNum);
						}
						else {
							indexProv=ApptDrawing.GetIndexProv(ctrl.ProvHyg);
						}
					}
					else {//not hyg
						indexProv=ApptDrawing.GetIndexProv(ctrl.ProvNum);
					}
				}
				if(indexProv==-1) {
					continue;
				}
				if(ctrl.AptStatus!=ApptStatus.Broken
					&& ctrl.AptStatus!=ApptStatus.UnschedList
					&& ctrl.AptStatus!=ApptStatus.PtNote
					&& ctrl.AptStatus!=ApptStatus.PtNoteCompleted) 
				{
					//When the program is restricted to a specific clinic, only count up production for the corresponding clinic.
					if(!PrefC.GetBool(PrefName.EasyNoClinics) 
						&& Clinics.ClinicNum!=0
						&& Clinics.ClinicNum!=ctrl.ClinicNum) {
						continue;//This appointment is for a different clinic.  Do not include this production in the daily prod.
					}
					//In order to get production numbers split by provider, it would require generating total production numbers
					//in another table from the business layer.  But that will only work if hyg procedures are appropriately assigned
					//when setting appointments.
					grossproduction+=ctrl.GrossProduction;
					netproduction+=ctrl.GrossProduction-ctrl.WriteoffPPO;
				}
			}
			//AdjustmentTotal field is static (one instance only) but held in each instance of ContrAppSingle for simplicity of access.
			if(PrefC.GetBool(PrefName.ApptModuleAdjustmentsInProd) && ContrApptSheet2.Controls.Count>0) { 
				netproduction+=ContrApptSheet2.ListContrApptSingles[0].AdjustmentTotal;
			}
			textProduction.Text=grossproduction.ToString("c0");
			if(grossproduction!=netproduction) {
				textProduction.Text+=Lan.g(this,", net:")+netproduction.ToString("c0");
			}
		}

		private void FillProductionGoal(DateTime start,DateTime end) {
			if(!ApptViewItemL.ApptRows.Exists(x => x.ElementDesc=="Production")) {
				textProdGoal.Text="";
				return;
			}
			decimal prodGoalAmt=0;
			List<long> listProvNumsForApptView=ApptViewItems.GetProvsForView(GetApptViewNumForUser());
			Dictionary<long,decimal> dictProvProdGoal=Providers.GetProductionGoalForProviders(listProvNumsForApptView,start,end);
			foreach(long provNum in listProvNumsForApptView) {
				if(dictProvProdGoal.ContainsKey(provNum)){
					prodGoalAmt+=dictProvProdGoal[provNum];
				}
			}
			textProdGoal.Text=prodGoalAmt.ToString("c0");
		}

		///<summary></summary>
		private void FillProvSched(bool hasNotes) {
			DataTable table=_dtProvSched;
			gridProv.BeginUpdate();
			gridProv.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableApptProvSched","Provider"),80);
			gridProv.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableApptProvSched","Schedule"),70);
			gridProv.Columns.Add(col);
			if(hasNotes) {
				col=new ODGridColumn(Lan.g("TableApptProvSched","Notes"),100);
				gridProv.Columns.Add(col);
			}
			gridProv.Rows.Clear();
			ODGridRow row;
			foreach(DataRow dRow in table.Rows) { 
				row=new ODGridRow();
				row.Cells.Add(dRow["ProvAbbr"].ToString());
				row.Cells.Add(dRow["schedule"].ToString());
				if(hasNotes) {
					row.Cells.Add(dRow["Note"].ToString());
				}
				gridProv.Rows.Add(row);
			}
			gridProv.EndUpdate();
		}

		///<summary></summary>
		private void FillEmpSched(bool hasNotes) {
			DataTable table=_dtEmpSched;
			gridEmpSched.BeginUpdate();
			gridEmpSched.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableApptEmpSched","Employee"),80);
			gridEmpSched.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableApptEmpSched","Schedule"),70);
			gridEmpSched.Columns.Add(col);
			if(hasNotes) {
				col=new ODGridColumn(Lan.g("TableApptEmpSched","Notes"),100);
				gridEmpSched.Columns.Add(col);
			}
			gridEmpSched.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<table.Rows.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(table.Rows[i]["empName"].ToString());
				row.Cells.Add(table.Rows[i]["schedule"].ToString());
				if(hasNotes) {
					row.Cells.Add(table.Rows[i]["Note"].ToString());
				}
				gridEmpSched.Rows.Add(row);
			}
			gridEmpSched.EndUpdate();
		}

		///<summary>Set refreshOpsForDay true in order to forcefully go to the database to get the current ops for the day for waiting room filtering.</summary>
		private void FillWaitingRoom() {
			if(_dtWaitingRoom==null) {
				return;
			}
			TimeSpan delta=DateTime.Now-LastTimeDataRetrieved;
			DataTable table=_dtWaitingRoom;
			List<Operatory> listOpsForClinic=new List<Operatory>();
			List<Operatory> listOpsForApptView=new List<Operatory>();
			if(PrefC.GetBool(PrefName.WaitingRoomFilterByView)) {
				//In order to filter the waiting room by appointment view, we need to always grab the operatories visible for TODAY.
				//This way, regardless of what day the customer is looking at, the waiting room will only change when they change appointment views.
				//Always use the schedules from SchedListPeriod which is refreshed any time RefreshModuleDataPeriod() is invoked.
				ApptView viewCur=null;
				if(comboView.SelectedIndex>0) {
					viewCur=_listApptViews[comboView.SelectedIndex-1];
				}
				List<Schedule> listSchedulesForToday=SchedListPeriod.FindAll(x => x.SchedDate==DateTime.Today);
				listOpsForApptView=ApptViewItemL.GetOpsForApptView(viewCur,ApptDrawing.IsWeeklyView,listSchedulesForToday);
			}
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Using clinics
				listOpsForClinic=Operatories.GetOpsForClinic(Clinics.ClinicNum);
			}
			gridWaiting.BeginUpdate();
			gridWaiting.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableApptWaiting","Patient"),130);
			gridWaiting.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableApptWaiting","Waited"),100,HorizontalAlignment.Center);
			gridWaiting.Columns.Add(col);
			gridWaiting.Rows.Clear();
			DateTime waitTime;
			ODGridRow row;
			int waitingRoomAlertTime=PrefC.GetInt(PrefName.WaitingRoomAlertTime);
			Color waitingRoomAlertColor=PrefC.GetColor(PrefName.WaitingRoomAlertColor);
			for(int i=0;i<table.Rows.Count;i++) {
				//Always filter the waiting room by appointment view first, regardless of using clinics or not.
				if(PrefC.GetBool(PrefName.WaitingRoomFilterByView)) {
					bool isInView=false;
					for(int j=0;j<listOpsForApptView.Count;j++) {
						if(listOpsForApptView[j].OperatoryNum==PIn.Long(table.Rows[i]["OpNum"].ToString())) {
							isInView=true;
							break;
						}
					}
					if(!isInView) {
						continue;
					}
				}
				//We only want to filter the waiting room by the clinic's operatories when clinics are enabled and they are not using 'Headquarters' mode.
				if(!PrefC.GetBool(PrefName.EasyNoClinics) && Clinics.ClinicNum!=0) {
					bool isInView=false;
					for(int j=0;j<listOpsForClinic.Count;j++) {
						if(listOpsForClinic[j].OperatoryNum==PIn.Long(table.Rows[i]["OpNum"].ToString())) {
							isInView=true;
							break;
						}
					}
					if(!isInView) {
						continue;
					}
				}
				row=new ODGridRow();
				row.Cells.Add(table.Rows[i]["patName"].ToString());
				waitTime=DateTime.Parse(table.Rows[i]["waitTime"].ToString());//we ignore date
				waitTime+=delta;
				row.Cells.Add(waitTime.ToString("H:mm:ss"));
				row.Bold=false;
				if(waitingRoomAlertTime>0 && waitingRoomAlertTime<=waitTime.Minute+(waitTime.Hour*60)) {
					row.ColorText=waitingRoomAlertColor;
					row.Bold=true;
				}
				gridWaiting.Rows.Add(row);
			}
			gridWaiting.EndUpdate();
		}

		private void gridEmpSched_DoubleClick(object sender,EventArgs e) {
			gridSchedDoubleClickHelper();
		}

		private void gridProv_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			gridSchedDoubleClickHelper();
		}

		private void gridSchedDoubleClickHelper() {
			if(ApptDrawing.IsWeeklyView) {
				MsgBox.Show(this,"Not available in weekly view");
				return;
			}
			if(!Security.IsAuthorized(Permissions.Schedules)) {
				return;
			}
			FormScheduleDayEdit FormSDE=new FormScheduleDayEdit(AppointmentL.DateSelected,Clinics.ClinicNum);
			FormSDE.ShowOkSchedule=true;
			FormSDE.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Schedules,0,"");
			SetWeeklyView(false);//to refresh
			if(FormSDE.GotoScheduleOnClose) {
				FormSchedule FormS = new FormSchedule();
				FormS.ShowDialog();
			}
		}
		
		///<summary>Gets the index within the array of appointment controls, based on the supplied primary key.</summary>
		private int GetIndex(long myAptNum) {
			return ContrApptSheet2.ListAptNums.FindIndex(x => x==myAptNum);			
		}

		private void SendToPinBoard(long aptNum) {
			List<long> list=new List<long>();
			list.Add(aptNum);
			SendToPinboard(list);
		}

		///<summary>Loads all info for for specified appointment into the control that displays the pinboard appointment. Runs RefreshModuleDataPatient.  
		///Sets pinboard appointment as selected.</summary>
		private void SendToPinboard(List<long> aptNums) {
			if(IsHqNoneView()) {
				MsgBox.Show(this,"Appointments can't be sent to the pinboard when an appointment view or clinic hasn't been selected.");
				return;
			}
			if(aptNums.Count==0) {
				return;
			}
			long patNum=0;
			foreach(long aptNum in aptNums) {
				//UI may be out of sync because we have not called RefreshPeriod() so let the PinBoard get the info it needs from the db.
				ContrApptSingle contrApptSingle=pinBoard.AddAppointment(aptNum);
				if(contrApptSingle!=null) { //Set the pt to the last appt on the pinboard.
					patNum=contrApptSingle.PatNum;
				}
			}
			RefreshModuleDataPatient(patNum); //Set the pt to the last appt on the pinboard.
			FormOpenDental.S_Contr_PatientSelected(PatCur,true,false);
			HideDraggableContrApptSingle();
			ContrApptSingle.SelectedAptNum=-1;
		}

		private void pinBoard_SelectedIndexChanged(object sender,EventArgs e) {
			RefreshModuleDataPatient(pinBoard.ApptList[pinBoard.SelectedIndex].PatNum);
			RefreshModuleScreenPatient();
			CancelPinMouseDown=false;
			FormOpenDental.S_Contr_PatientSelected(PatCur,false,false);
			//The line above can trigger a popup dialog which can cause the tempAppt to get stuck to the mouse
			//Since this is usually caused by user mouse, then it goes right into pinBoard_MouseDown().
		}

		///<Summary>Sets selected and prepares for drag.</Summary>
		private void pinBoard_MouseDown(object sender,MouseEventArgs e) {
			if(pinBoard.SelectedIndex==-1) {
				return;
			}
			if(mouseIsDown) {//User right clicked while draging appt around.
				return;
			}
			if(e.Button==MouseButtons.Right) {
				ContextMenu cmen=new ContextMenu();
				MenuItem menuItemProv=new MenuItem(Lan.g(this,"Change Provider"));
				menuItemProv.Click+=new EventHandler(menuItemProv_Click);
				cmen.MenuItems.Add(menuItemProv);
				cmen.Show(pinBoard,e.Location);
				return;
			}
			if(CancelPinMouseDown) {//I'm worried that setting this to false in pinBoard_SelectedIndexChanged is not frequent enough,
				//because a mouse down could happen without the selected index changing.
				//But in that case, a popup would already have happened.
				//Worst case scenario is that user would have to try again.
				CancelPinMouseDown=false;
				return;
			}
			ShowDraggableContrApptSingle(pinBoard.SelectedAppt);			
			ContrApptSingle.SelectedAptNum=-1;
			ContrApptSheet2.DoubleBufferDraw(drawToScreen: true);//to clear previous selection
			//mouseOrigin is in ContrAppt coordinates (essentially, the entire window)
			mouseOrigin.X=e.X+pinBoard.Location.X+panelCalendar.Location.X;
			//e.X+PinApptSingle.Location.X;
			mouseOrigin.Y=e.Y+pinBoard.SelectedAppt.Location.Y+pinBoard.Location.Y+panelCalendar.Location.Y;
			//e.Y+PinApptSingle.Location.Y;
			contOrigin=new Point(pinBoard.Location.X+panelCalendar.Location.X,
				pinBoard.SelectedAppt.Location.Y+pinBoard.Location.Y+panelCalendar.Location.Y);
			//PinApptSingle.Location;
		}

		void menuItemProv_Click(object sender,EventArgs e) {
			//throw new NotImplementedException();
			Appointment apt=Appointments.GetOneApt(pinBoard.SelectedAppt.AptNum);
			Appointment oldApt=apt.Copy();
			if(apt==null) {
				MessageBox.Show("Appointment not found.");
				return;
			}
			long provNum=apt.ProvNum;
			if(apt.IsHygiene) {
				provNum=apt.ProvHyg;
			}
			FormProviderPick formPick=new FormProviderPick();
			formPick.SelectedProvNum=provNum;
			formPick.ShowDialog();
			if(formPick.DialogResult!=DialogResult.OK) {
				return;
			}
			if(formPick.SelectedProvNum==provNum) {
				return;//provider not changed.
			}
			if(apt.IsHygiene) {
				apt.ProvHyg=formPick.SelectedProvNum;
			}
			else {
				apt.ProvNum=formPick.SelectedProvNum;
			}
			List<Procedure> procsForSingleApt=Procedures.GetProcsForSingle(apt.AptNum,false);
			List<long> codeNums=new List<long>();
			for(int p=0;p<procsForSingleApt.Count;p++) {
				codeNums.Add(procsForSingleApt[p].CodeNum);
			}
			string calcPattern=Appointments.CalculatePattern(apt.ProvNum,apt.ProvHyg,codeNums,true);
			if(apt.Pattern != calcPattern) {
				if(!apt.TimeLocked || MsgBox.Show(this,MsgBoxButtons.YesNo,"Appointment length is locked.  Change length for new provider anyway?")) {
					apt.Pattern=calcPattern;
				}
			}
			Appointments.Update(apt,oldApt);
			InsPlan aptInsPlan1 = InsPlans.GetPlan(apt.InsPlan1,null);//we only care about lining the fees up with the primary insurance plan
			bool isUpdatingFees=false;
			List<Procedure> listProcsNew=procsForSingleApt.Select(x => Procedures.UpdateProcInAppointment(apt,x.Copy())).ToList();
			if(procsForSingleApt.Exists(x => x.ProvNum!=listProcsNew.FirstOrDefault(y => y.ProcNum==x.ProcNum).ProvNum)) {//Either the primary or hygienist changed.
				string promptText="";
				isUpdatingFees=Procedures.FeeUpdatePromptHelper(listProcsNew,procsForSingleApt,aptInsPlan1,ref promptText);
				if(isUpdatingFees) {//Made it pass the pref check.
					if(promptText!="" && !MsgBox.Show(this,MsgBoxButtons.YesNo,promptText)) {
							isUpdatingFees=false;
					}
				}
			}
			Procedures.SetProvidersInAppointment(apt,procsForSingleApt,isUpdatingFees);
			List<long> listOpNums=null;
			List<long> listProvNums=null;
			if(Clinics.ClinicNum!=0 || comboView.SelectedIndex!=0) {
				listOpNums=ApptDrawing.VisOps.Select(x => x.OperatoryNum).ToList();
				listProvNums=ApptDrawing.VisProvs.Select(x => x.ProvNum).ToList();
			}
			ModuleSelected(PatCur.PatNum,listOpNums:listOpNums,listProvNums:listProvNums);			
			pinBoard.ResetData(apt.AptNum);
			//MessageBox.Show(Providers.GetAbbr(apt.ProvNum));
		}

		///<Summary>Moves pinboard appt if mouse is down.</Summary>
		private void pinBoard_MouseMove(object sender,MouseEventArgs e) {
			if(!mouseIsDown) {
				return;
			}
			if(pinBoard.SelectedAppt==null) {
				return;
			}
			if(TempApptSingle.Location==new Point(0,0)) {
				TempApptSingle.Height=1;//to prevent flicker in UL corner
			}
			TempApptSingle.Visible=true;
			boolAptMoved=true;
			TempApptSingle.Location=new Point(
				contOrigin.X+(e.X+pinBoard.Location.X+panelCalendar.Location.X)-mouseOrigin.X,
				contOrigin.Y+(e.Y+pinBoard.SelectedAppt.Location.Y+pinBoard.Location.Y+panelCalendar.Location.Y)-mouseOrigin.Y);
			if(TempApptSingle.Height==1) {
				TempApptSingle.Size=ApptSingleDrawing.SetSize(TempApptSingle.Pattern);
			}
		}

		///<Summary>Usually happens after a pinboard appt has been dragged onto main appt sheet.  Does not use the MoveAppointments() logic - This is similar, but not identical.</Summary>
		private void pinBoard_MouseUp(object sender,MouseEventArgs e) {
			//try/finally only. No catch. We probably want a hand exception to popup if there is a real bug.
			//Any return from this point forward will cause HideDraggableContrApptSingle();					
			try {
				if(!boolAptMoved) {
					return;
				}
				//Make sure there are operatories for the appointment to be scheduled and make sure the user dragged the appointment to a valid location.
				if(ApptDrawing.VisOps.Count==0||TempApptSingle.Location.X>ContrApptSheet2.Width||pinBoard.SelectedAppt==null) {
					return;
				}
				if(pinBoard.SelectedAppt.AptStatus==ApptStatus.Planned//if Planned appt is on pinboard
					&&(!Security.IsAuthorized(Permissions.AppointmentCreate)//and no permission to create a new appt
						||PatRestrictionL.IsRestricted(pinBoard.SelectedAppt.PatNum,PatRestrict.ApptSchedule)))//or pat restricted
				{
					return;
				}
				//security prevents moving an appointment by preventing placing it on the pinboard, not here
				//We no longer ask user this question.  It just slows things down: "Move Appointment?"
				//convert loc to new time
				Appointment aptCur=Appointments.GetOneApt(pinBoard.SelectedAppt.AptNum);
				if(aptCur==null) {
					MsgBox.Show(this,"This appointment has been deleted since it was moved to the pinboard. It will now be cleared from the pinboard.");
					pinBoard.ClearSelected();
					return;
				}
				//This hook is specifically called after we know a valid appointment has been identified.
				Plugins.HookAddCode(this, "ContrAppt.pinBoard_MouseUp_validation_end",aptCur);
				Appointment aptOld=aptCur.Copy();
				RefreshModuleDataPatient(pinBoard.SelectedAppt.PatNum);//redundant?
																	   //Patient pat=Patients.GetPat(pinBoard.SelectedAppt.PatNum);
				if(aptCur.IsNewPatient&&AppointmentL.DateSelected!=aptCur.AptDateTime) {
					Procedures.SetDateFirstVisit(AppointmentL.DateSelected,4,PatCur);
				}
				int tHr=ApptDrawing.ConvertToHour
					(TempApptSingle.Location.Y-ContrApptSheet2.Location.Y-panelSheet.Location.Y);
				int tMin=ApptDrawing.ConvertToMin
					(TempApptSingle.Location.Y-ContrApptSheet2.Location.Y-panelSheet.Location.Y);
				DateTime tDate=AppointmentL.DateSelected;
				if(ApptDrawing.IsWeeklyView) {
					tDate=WeekStartDate.AddDays(ApptDrawing.ConvertToDay(TempApptSingle.Location.X-ContrApptSheet2.Location.X));
				}
				DateTime fromDate=aptCur.AptDateTime.Date;
				aptCur.AptDateTime=new DateTime(tDate.Year,tDate.Month,tDate.Day,tHr,tMin,0);
				//Compare beginning of new appointment against end to see if the appointment spans two days
				if(aptCur.AptDateTime.Day!=aptCur.AptDateTime.AddMinutes(aptCur.Pattern.Length*5).Day) {
					MsgBox.Show(this,"You cannot have an appointment that starts and ends on different days.");
					return;
				}
				#region Prevent double-booking
				if(AppointmentRules.GetCount() > 0) { //If no rules exist then don't bother checking.
					List<Procedure> procsMultApts=Procedures.GetProcsMultApts(ContrApptSheet2.ListAptNums);
					Procedure[] procsForOne=Procedures.GetProcsOneApt(aptCur.AptNum,procsMultApts);
					ArrayList doubleBookedCodes=
					AppointmentL.GetDoubleBookedCodes(aptCur,_dtAppointments.Copy(),procsMultApts,procsForOne);
					if(doubleBookedCodes.Count>0) {//if some codes would be double booked
						if(AppointmentRules.IsBlocked(doubleBookedCodes)) {
							MessageBox.Show(Lan.g(this,"Not allowed to double book:")+" "
								+AppointmentRules.GetBlockedDescription(doubleBookedCodes));
							return;
						}
					}
				}
				#endregion
				Operatory curOp=ApptDrawing.VisOps[ApptDrawing.ConvertToOp(TempApptSingle.Location.X-ContrApptSheet2.Location.X)];
				aptCur.Op=curOp.OperatoryNum;
				//Set providers----------------------Similar to UpdateAppointments()
				long assignedDent=Schedules.GetAssignedProvNumForSpot(SchedListPeriod,curOp,false,aptCur.AptDateTime);
				long assignedHyg=Schedules.GetAssignedProvNumForSpot(SchedListPeriod,curOp,true,aptCur.AptDateTime);
				List<Procedure> procsForSingleApt=null;
				if(aptCur.AptStatus!=ApptStatus.PtNote&&aptCur.AptStatus!=ApptStatus.PtNoteCompleted) {
					#region Update Appt's DateTimeAskedToArrive
					if(PatCur.AskToArriveEarly>0) {
						aptCur.DateTimeAskedToArrive=aptCur.AptDateTime.AddMinutes(-PatCur.AskToArriveEarly);
						MessageBox.Show(Lan.g(this,"Ask patient to arrive")+" "+PatCur.AskToArriveEarly
							+" "+Lan.g(this,"minutes early at")+" "+aptCur.DateTimeAskedToArrive.ToShortTimeString()+".");
					}
					else {
						aptCur.DateTimeAskedToArrive=DateTime.MinValue;
					}
					#endregion Update Appt's DateTimeAskedToArrive
					#region Update Appt's Update Appt's ProvNum, ProvHyg, IsHygiene, Pattern
					//if no dentist/hygienist is assigned to spot, then keep the original dentist/hygienist without prompt.  All appts must have prov.
					if((assignedDent!=0&&assignedDent!=aptCur.ProvNum)||(assignedHyg!=0&&assignedHyg!=aptCur.ProvHyg)) {
						if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Change provider?")) {
							if(assignedDent!=0) {//the dentist will only be changed if the spot has a dentist.
								aptCur.ProvNum=assignedDent;
							}
							if(assignedHyg!=0||PrefC.GetBool(PrefName.ApptSecondaryProviderConsiderOpOnly)) {//the hygienist will only be changed if the spot has a hygienist.
								aptCur.ProvHyg=assignedHyg;
							}
							if(curOp.IsHygiene) {
								aptCur.IsHygiene=true;
							}
							else {//op not marked as hygiene op
								if(assignedDent==0) {//no dentist assigned
									if(assignedHyg!=0) {//hyg is assigned (we don't really have to test for this)
										aptCur.IsHygiene=true;
									}
								}
								else {//dentist is assigned
									if(assignedHyg==0) {//hyg is not assigned
										aptCur.IsHygiene=false;
									}
									//if both dentist and hyg are assigned, it's tricky
									//only explicitly set it if user has a dentist assigned to the op
									if(curOp.ProvDentist!=0) {
										aptCur.IsHygiene=false;
									}
								}
							}
							bool isplanned=aptCur.AptStatus==ApptStatus.Planned;
							procsForSingleApt=Procedures.GetProcsForSingle(aptCur.AptNum,isplanned);
							List<long> codeNums=new List<long>();
							for(int p = 0;p<procsForSingleApt.Count;p++) {
								codeNums.Add(procsForSingleApt[p].CodeNum);
							}
							string calcPattern=Appointments.CalculatePattern(aptCur.ProvNum,aptCur.ProvHyg,codeNums,true);
							if(aptCur.Pattern!=calcPattern&&!PrefC.GetBool(PrefName.AppointmentTimeIsLocked)) {
								if(aptCur.TimeLocked) {
									if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Appointment length is locked.  Change length for new provider anyway?")) {
										aptCur.Pattern=calcPattern;
									}
								}
								else {//appt time not locked
									if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Change length for new provider?")) {
										aptCur.Pattern=calcPattern;
									}
								}
							}
						}
					}
					#endregion Update Appt's ProvNum, ProvHyg, IsHygiene, Pattern
				}
				#region Prevent overlap
				if(!Appointments.TryAdjustAppointmentOp(aptCur,ApptDrawing.VisOps)) {
					MessageBox.Show(Lan.g(this,"Appointment overlaps existing appointment or blockout."));
					return;
				}
				#endregion Prevent overlap
				#region Detect Frequency Conflicts
				//Detect frequency conflicts with procedures in the appointment
				if(PrefC.GetBool(PrefName.InsChecksFrequency)) {
					procsForSingleApt=Procedures.GetProcsForSingle(aptCur.AptNum,false);
					string frequencyConflicts=""; 
					try {
						frequencyConflicts=Procedures.CheckFrequency(procsForSingleApt,aptCur.PatNum,aptCur.AptDateTime);
					}
					catch(Exception ex) {
						MessageBox.Show(Lan.g(this,"There was an error checking frequencies."
							+"  Disable the Insurance Frequency Checking feature or try to fix the following error:")
							+"\r\n"+ex.Message);
						return;
					}
					if(frequencyConflicts!="" && MessageBox.Show(Lan.g(this,"Scheduling this appointment for this date will cause frequency conflicts for the following procedures")
							+":\r\n"+frequencyConflicts+"\r\n"+Lan.g(this,"Do you want to continue?"),"",MessageBoxButtons.YesNo)==DialogResult.No) 
					{
						return;
					}
				}
				#endregion Detect Frequency Conflicts
				#region Patient status
				Operatory opCur=Operatories.GetOperatory(aptCur.Op);
				Operatory opOld=Operatories.GetOperatory(aptOld.Op);
				if(opOld==null||opCur.SetProspective!=opOld.SetProspective) {
					if(opCur.SetProspective&&PatCur.PatStatus!=PatientStatus.Prospective) { //Don't need to prompt if patient is already prospective.
						if(MsgBox.Show(this,MsgBoxButtons.OKCancel,"Patient's status will be set to Prospective.")) {
							Patient patOld=PatCur.Copy();
							PatCur.PatStatus=PatientStatus.Prospective;
							Patients.Update(PatCur,patOld);
						}
					}
					else if(!opCur.SetProspective&&PatCur.PatStatus==PatientStatus.Prospective) {
						//Do we need to warn about changing FROM prospective? Assume so for now.
						if(MsgBox.Show(this,MsgBoxButtons.OKCancel,"Patient's status will change from Prospective to Patient.")) {
							Patient patOld=PatCur.Copy();
							PatCur.PatStatus=PatientStatus.Patient;
							Patients.Update(PatCur,patOld);
						}
					}
				}
				#endregion Patient status
				#region Update Appt's AptStatus, ClinicNum
				if(aptCur.AptStatus==ApptStatus.Broken) {
					aptCur.AptStatus=ApptStatus.Scheduled;
				}
				if(aptCur.AptStatus==ApptStatus.UnschedList) {
					aptCur.AptStatus=ApptStatus.Scheduled;
				}
				//original position of provider settings
				if(curOp.ClinicNum==0) {
					aptCur.ClinicNum=PatCur.ClinicNum;
				}
				else {
					aptCur.ClinicNum=curOp.ClinicNum;
				}
				#endregion Update Appt's AptStatus, ClinicNum
				bool isCreate=false;
				#region Update/Insert Appt in db
				if(aptCur.AptStatus==ApptStatus.Planned) {//if Planned appt is on pinboard
					#region Planned appointment
					List<ApptField> listApptFields=new List<ApptField>();
					for(int i = 0;i<pinBoard.SelectedAppt.TableApptFields.Rows.Count;i++) {//Duplicate the appointment fields.
						if(aptOld.AptNum==PIn.Long(pinBoard.SelectedAppt.TableApptFields.Rows[i]["AptNum"].ToString())) {
							ApptField apptField = new ApptField();
							apptField.FieldName=PIn.String(pinBoard.SelectedAppt.TableApptFields.Rows[i]["FieldName"].ToString());
							apptField.FieldValue=PIn.String(pinBoard.SelectedAppt.TableApptFields.Rows[i]["FieldValue"].ToString());
							listApptFields.Add(apptField);
						}
					}
					bool procAlreadyAttached;
					try {
						ODTuple<Appointment,bool> aptTuple=Appointments.SchedulePlannedApt(aptCur,PatCur,listApptFields,aptCur.AptDateTime,aptCur.Op);//Appointments S-Class handles Signalods
						aptCur=aptTuple.Item1;
						procAlreadyAttached=aptTuple.Item2;
					}
					catch(ApplicationException ex) {
						MessageBox.Show(ex.Message);
						return;
					}
					if(procAlreadyAttached) {
						MessageBox.Show(Lan.g(this,"One or more procedures could not be scheduled because they were already attached to another appointment. "
							+"Someone probably forgot to update the Next appointment in the Chart module."));
						FormApptEdit formAE=new FormApptEdit(aptCur.AptNum);
						CheckStatus();
						formAE.IsNew=true;
						formAE.ShowDialog();//to force refresh of aptDescript
						if(formAE.DialogResult!=DialogResult.OK) {//apt gets deleted from within aptEdit window.
							RefreshModuleScreenPatient();
							RefreshPeriod();
							return;
						}
					}
					else {
						SecurityLogs.MakeLogEntry(Permissions.AppointmentCreate,aptCur.PatNum,
							aptCur.AptDateTime.ToString()+", "+aptCur.ProcDescript,
							aptCur.AptNum,aptOld.DateTStamp);
					}
					procsForSingleApt=Procedures.GetProcsForSingle(aptCur.AptNum,false);
					#endregion Planned appointment
				}
				else {//simple drag off pinboard to a new date/time
					#region Previously scheduled appointment (not a planned appointment)
					aptCur.Confirmed=Defs.GetFirstForCategory(DefCat.ApptConfirmed,true).DefNum;//Causes the confirmation status to be reset.
					try {
						Appointments.Update(aptCur,aptOld);//Appointments S-Class handles Signalods
						if(aptOld.AptStatus==ApptStatus.UnschedList&&aptOld.AptDateTime==DateTime.MinValue) { //If new appt is being added to schedule from pinboard
							SecurityLogs.MakeLogEntry(Permissions.AppointmentCreate,aptCur.PatNum,
								aptCur.AptDateTime.ToString()+", "+aptCur.ProcDescript,
								aptCur.AptNum,aptOld.DateTStamp);
							isCreate=true;
						}
						else { //If existing appt is being moved
							SecurityLogs.MakeLogEntry(Permissions.AppointmentMove,aptCur.PatNum,
								aptCur.ProcDescript+", from "+aptOld.AptDateTime.ToString()+", to "+aptCur.AptDateTime.ToString(),
								aptCur.AptNum,aptOld.DateTStamp);
						}
						if(aptCur.Confirmed!=aptOld.Confirmed) {
							//Log confirmation status changes.
							SecurityLogs.MakeLogEntry(Permissions.ApptConfirmStatusEdit,aptCur.PatNum,
								Lans.g(this,"Appointment confirmation status automatically changed from")+" "
								+Defs.GetName(DefCat.ApptConfirmed,aptOld.Confirmed)+" "+Lans.g(this,"to")+" "+Defs.GetName(DefCat.ApptConfirmed,aptCur.Confirmed)
								+Lans.g(this,"from the appointment module")+".",aptCur.AptNum,aptOld.DateTStamp);
						}
						//If there is an existing HL7 def enabled, send a SIU message if there is an outbound SIU message defined
						if(HL7Defs.IsExistingHL7Enabled()) {
							//S12 - New Appt Booking event, S13 - Appt Rescheduling
							MessageHL7 messageHL7=null;
							if(isCreate) {
								messageHL7=MessageConstructor.GenerateSIU(PatCur,Patients.GetPat(PatCur.Guarantor),EventTypeHL7.S12,aptCur);
							}
							else {
								messageHL7=MessageConstructor.GenerateSIU(PatCur,Patients.GetPat(PatCur.Guarantor),EventTypeHL7.S13,aptCur);
							}
							//Will be null if there is no outbound SIU message defined, so do nothing
							if(messageHL7!=null) {
								HL7Msg hl7Msg=new HL7Msg();
								hl7Msg.AptNum=aptCur.AptNum;
								hl7Msg.HL7Status=HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
								hl7Msg.MsgText=messageHL7.ToString();
								hl7Msg.PatNum=PatCur.PatNum;
								HL7Msgs.Insert(hl7Msg);
#if DEBUG
								MessageBox.Show(this,messageHL7.ToString());
#endif
							}
						}
					}
					catch(ApplicationException ex) {
						MessageBox.Show(ex.Message);
						return;
					}
					#endregion Previously scheduled appointment (not a planned appointment)
				}
				#endregion Update/Insert Appt in db
				if(procsForSingleApt==null) {
					procsForSingleApt=Procedures.GetProcsForSingle(aptCur.AptNum,false);
				}
				#region Update UI and cache
				InsPlan aptInsPlan1 = InsPlans.GetPlan(aptCur.InsPlan1,null);//we only care about lining the fees up with the primary insurance plan
				bool isUpdatingFees=false;
				List<Procedure> listProcsNew=procsForSingleApt.Select(x => Procedures.UpdateProcInAppointment(aptCur,x.Copy())).ToList();
				if(procsForSingleApt.Exists(x => x.ProvNum!=listProcsNew.FirstOrDefault(y => y.ProcNum==x.ProcNum).ProvNum)) {//Either the primary or hygienist changed.
					string promptText="";
					isUpdatingFees=Procedures.FeeUpdatePromptHelper(listProcsNew,procsForSingleApt,aptInsPlan1,ref promptText);
					if(isUpdatingFees) {//Made it pass the pref check.
						if(promptText!="" && !MsgBox.Show(this,MsgBoxButtons.YesNo,promptText)) {
								isUpdatingFees=false;
						}
					}
				}
				Procedures.SetProvidersInAppointment(aptCur,procsForSingleApt,isUpdatingFees);
				pinBoard.ClearSelected();
				ContrApptSingle.SelectedAptNum=aptCur.AptNum;
				RefreshModuleScreenPatient();
				RefreshPeriod(isRefreshSchedules:true);//date moving to for this computer; This line may not be needed
				AppointmentL.DateSelected=aptCur.AptDateTime;			
				if(isCreate) {//new appointment (not planned) is being added to the schedule from the pinboard, trigger ScheduleProcedure automation
					List<string> procCodes=procsForSingleApt.Select(x => ProcedureCodes.GetProcCode(x.CodeNum).ProcCode).ToList();					
					AutomationL.Trigger(AutomationTrigger.ScheduleProcedure,procCodes,aptCur.PatNum);
				}
				Recalls.SynchScheduledApptFull(aptCur.PatNum);
				Plugins.HookAddCode(this,"ContrAppt.pinBoard_MouseUp_end",aptCur,PatCur);
				#endregion Update UI and cache
			}
			finally {
				HideDraggableContrApptSingle();
			}
		}

		///<summary>Copied from FormApptsOther. Does not limit appointment creation, only warns user. This check should be run before creating a new appointment. </summary>
		private void CheckStatus() {
			if(PatCur.PatStatus == PatientStatus.Inactive
				|| PatCur.PatStatus == PatientStatus.Archived
				|| PatCur.PatStatus == PatientStatus.Prospective) {
				MsgBox.Show(this,"Warning. Patient is not active.");
			}
			if(PatCur.PatStatus == PatientStatus.Deceased) {
				MsgBox.Show(this,"Warning. Patient is deceased.");
			}
		}
		
		///<summary>Checks if the appointment's start time overlaps another appt in the Op which the apt resides.  Tests all appts for the day, even if not visible.
		///Calling RefreshPeriod() is not necessary before calling this method. It goes to the db only as much as is necessary.
		///Returns true if no overlap found. Returns false if given apt start time conflicts with another apt in the Op.</summary>
		private static bool HasValidStartTime(Appointment apt) {
			bool notUsed;
			//Only valid if no adjust was needed.
			return !Appointments.TryAdjustAppointment(apt,ApptDrawing.VisOps,false,false,false,out notUsed);
		}

		///<summary>Shortens apt.Pattern if overlap is found in neighboring op within apt.Op. Pattern will be adjusted to a minimum of 1 until no overlap occurs.
		///Calling RefreshPeriod() is not necessary before calling this method. It goes to the db only as much as is necessary.
		///Returns true if patter was adjusted. Returns false if pattern was not adjusted.</summary>
		public static bool TryAdjustAppointmentPattern(Appointment apt) {
			bool isPatternChanged;
			Appointments.TryAdjustAppointment(apt,ApptDrawing.VisOps,false,true,true,out isPatternChanged);
			return isPatternChanged;
		}

		///<summary>Returns the apptNum of the appointment at these coordinates, or 0 if none.  This is new code which is going to replace some of the outdated code on this page.</summary>
		private long HitTestAppt(Point point) {
			if(ApptDrawing.VisOps.Count==0) {//no ops visible.
				return 0;
			}
			int day=ApptDrawing.XPosToDay(point.X);
			if(ApptDrawing.IsWeeklyView) {
				//this is a workaround because we start on Monday:
				if(day==6) {
					day=0;
				}
				else {
					day=day+1;
				}
			}
			//if operatories were just hidden and VisOps is mismatched with ListShort
			int xOp=ApptDrawing.XPosToOpIdx(point.X);
			if(xOp>ApptDrawing.VisOps.Count-1) {
				return 0;
			}
			long op=ApptDrawing.VisOps[xOp].OperatoryNum;
			int hour=ApptDrawing.YPosToHour(point.Y);
			int minute=ApptDrawing.YPosToMin(point.Y);
			TimeSpan time=new TimeSpan(hour,minute,0);
			TimeSpan aptTime;
			foreach(ContrApptSingle ctrl in ContrApptSheet2.ListContrApptSingles.FindAll(x => x.OpNum==op)){
				if(ApptDrawing.IsWeeklyView && (int)ctrl.AptDateTime.DayOfWeek!=day) {
					continue;
				}
				aptTime=ctrl.AptDateTime.TimeOfDay;
				if(aptTime<=time
					&&time<aptTime+TimeSpan.FromMinutes(ctrl.Pattern.Length*5)) {
					return ctrl.AptNum;
				}
			}
			return 0;
		}

		///<summary>If the given point is in the bottom few pixels of an appointment, then this returns true.  Use HitTestAppt to figure out which appointment.</summary>
		private bool HitTestApptBottom(Point point) {
			long aptnum=HitTestAppt(point);
			if(aptnum==0) {
				return false;
			}
			//get the appointment control in order to measure
			ContrApptSingle apptSing=ContrApptSheet2.ListContrApptSingles.FirstOrDefault(x => x.AptNum==aptnum);
			if(apptSing==null) {
				return false;
			}			
			//find the bottom border of the appointment
			int bottom=apptSing.Bottom;
			if(point.Y>bottom-8) {
				return true;
			}
			return false;
		}

		///<summary>Mouse down event anywhere on the sheet.  Could be a blank space or on an actual appointment.</summary>
		private void ContrApptSheet2_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			if(Plugins.HookMethod(this,"ContrApptSheet2_MouseDown_start",ContrApptSingle.ClickedAptNum,e)) {
				return;
			}
			if(infoBubble.Visible) {
				infoBubble.Visible=false;
			}
			if(ApptDrawing.VisOps.Count==0) {//no ops visible.
				return;
			}
			if(mouseIsDown) {//if user clicks right mouse button while dragging
				return;
			}
			//some of this is a little redundant, but still necessary for now.
			SheetClickedonHour=ApptDrawing.YPosToHour(e.Y);
			SheetClickedonMin=ApptDrawing.YPosToMin(e.Y);
			TimeSpan sheetClickedOnTime=new TimeSpan(SheetClickedonHour,SheetClickedonMin,0);
			ContrApptSingle.ClickedAptNum=HitTestAppt(e.Location);
			SheetClickedonOp=ApptDrawing.VisOps[ApptDrawing.XPosToOpIdx(e.X)].OperatoryNum;
			SheetClickedonDay=ApptDrawing.XPosToDay(e.X);
			if(!ApptDrawing.IsWeeklyView) {
				//if Sunday is selected, we want to go to forward to the Sunday after the first day of the week (always Monday) not the Sunday before
				//used to determine if a blockout from a list of blockouts is on the day selected
				//this value will be added to the first day of the week, always Monday, to get the day selected
				//Example: if user clicks on Wednesday, (int)Wednesday=3, (int)Monday=1, SheetClickedonDay=3-1=2, Monday.AddDays(SheetClickedonDay)=Wednesday
				//Example: if user clicks on Sunday, (int)Sunday=0, SheetClickedonDay=6, Monday.AddDays(SheetClickedonDay)=the Sunday following the start of the week Monday
				if(AppointmentL.DateSelected.DayOfWeek==DayOfWeek.Sunday) {
					SheetClickedonDay=6;
				}
				else {
					SheetClickedonDay=((int)AppointmentL.DateSelected.DayOfWeek)-1;
				}
			}
			long prevSelectedAptNum=ContrApptSingle.SelectedAptNum;
			//Draw appt highlights boxes directly to the screen graphics. 
			//Normally we would write these to the double buffer bitmap but this is the rare case where we need to draw it to the screen.
			//We will also update the double buffer bitmap at the end of this method.
			Graphics grfx=ContrApptSheet2.CreateGraphics();
			if(ContrApptSingle.ClickedAptNum!=0) {//if clicked on an appt
				MouseDownAppointment(e,grfx);
			}
			else {//not clicked on appt
				MouseDownNonAppointment(e,grfx,sheetClickedOnTime);
			}
			grfx.Dispose();
			pinBoard.Invalidate();
			//We drew the appt highlight boxes directly to the screen above. 
			//Now we need to update the double buffer bitmap for next time the screen needs to be redrawn.
			ContrApptSheet2.DoubleBufferDraw(listAptNumsOnlyRedraw: new List<long> { ContrApptSingle.SelectedAptNum,prevSelectedAptNum },createApptShadows: true);
		}

		private void MouseDownAppointment(MouseEventArgs e,Graphics grfx) {
			ContrApptSingle thisApptCtrl=ContrApptSheet2[GetIndex(ContrApptSingle.ClickedAptNum)];
			pinBoard.SelectedIndex=-1;
			if(ContrApptSingle.SelectedAptNum!=-1//unselects previously selected unless it's the same appt
				&&ContrApptSingle.SelectedAptNum!=ContrApptSingle.ClickedAptNum) {
				int prevSel=GetIndex(ContrApptSingle.SelectedAptNum);
				//has to be done before refresh prev:
				ContrApptSingle.SelectedAptNum=ContrApptSingle.ClickedAptNum;
				if(prevSel!=-1) {
					ContrApptSingle prevApptCtrl=ContrApptSheet2[prevSel];
					prevApptCtrl.CreateShadow();
					if(prevApptCtrl.IsShadowValid) {
						grfx.DrawImage(prevApptCtrl.Shadow,prevApptCtrl.Location.X,prevApptCtrl.Location.Y);
					}
				}
			}
			//again, in case missed in loop above:
			ContrApptSingle.SelectedAptNum=ContrApptSingle.ClickedAptNum;
			thisApptCtrl.CreateShadow();
			if(thisApptCtrl.IsShadowValid) {
				grfx.DrawImage(thisApptCtrl.Shadow,thisApptCtrl.Location.X,thisApptCtrl.Location.Y);
			}
			long oldPatNum=(PatCur==null ? 0 : PatCur.PatNum);
			RefreshModuleDataPatient(thisApptCtrl.PatNum);
			if(e.Button==MouseButtons.Right) {
				if(PrefC.IsODHQ) {
					menuApt.MenuItems.RemoveByKey("Jobs");
					menuApt.MenuItems.RemoveByKey("JobSpacer");
					MenuItem menuSpacer=new MenuItem("-");
					menuSpacer.Name="JobSpacer";
					menuApt.MenuItems.Add(menuSpacer);
					MenuItem menuJobs=new MenuItem(Lan.g(this,"Jobs"));
					menuJobs.Name="Jobs";
					menuJobs.MenuItems.Add(Lan.g(this,"Attach Job"),new EventHandler(menuJobs_Attach));
					List<JobLink> jobLinks = JobLinks.GetForApptNum(ContrApptSingle.SelectedAptNum);
					List<Job> listJobs = Jobs.GetMany(jobLinks.Select(x => x.JobNum).ToList());
					foreach(Job job in listJobs) {
						MenuItem menuItemJob=new MenuItem(job.ToString(),new EventHandler(menuJobs_GoToJob));
						menuItemJob.Tag=job.JobNum;
						menuJobs.MenuItems.Add(menuItemJob);
					}
					menuApt.MenuItems.Add(menuJobs);
				}
				menuApt.MenuItems.RemoveByKey("Phone Div");
				menuApt.MenuItems.RemoveByKey("Home Phone");
				menuApt.MenuItems.RemoveByKey("Work Phone");
				menuApt.MenuItems.RemoveByKey("Wireless Phone");
				menuApt.MenuItems.RemoveByKey("Text Div");
				menuApt.MenuItems.RemoveByKey("Send Text");
				menuApt.MenuItems.RemoveByKey("Send Confirmation Text");
				menuApt.MenuItems.RemoveByKey("Ortho Chart");
				if(PrefC.GetBool(PrefName.ApptModuleShowOrthoChartItem)) {
					menuApt.MenuItems.Add(Lan.g(this,"Go To")+" "+OrthoChartTabs.GetFirst(true).TabName,new EventHandler(menuApt_Click));
					menuApt.MenuItems[menuApt.MenuItems.Count-1].Name="Ortho Chart";
				}
				//Phone numbers
				if(!String.IsNullOrEmpty(PatCur.HmPhone)||!String.IsNullOrEmpty(PatCur.WkPhone)||!String.IsNullOrEmpty(PatCur.WirelessPhone)) {
					menuApt.MenuItems.Add("-");
					menuApt.MenuItems[menuApt.MenuItems.Count-1].Name="Phone Div";
				}
				if(!String.IsNullOrEmpty(PatCur.HmPhone)) {
					menuApt.MenuItems.Add(Lan.g(this,"Call Home Phone")+" "+PatCur.HmPhone,new EventHandler(menuApt_Click));
					menuApt.MenuItems[menuApt.MenuItems.Count-1].Name="Home Phone";
				}
				if(!String.IsNullOrEmpty(PatCur.WkPhone)) {
					menuApt.MenuItems.Add(Lan.g(this,"Call Work Phone")+" "+PatCur.WkPhone,new EventHandler(menuApt_Click));
					menuApt.MenuItems[menuApt.MenuItems.Count-1].Name="Work Phone";
				}
				if(!String.IsNullOrEmpty(PatCur.WirelessPhone)) {
					menuApt.MenuItems.Add(Lan.g(this,"Call Wireless Phone")+" "+PatCur.WirelessPhone,new EventHandler(menuApt_Click));
					menuApt.MenuItems[menuApt.MenuItems.Count-1].Name="Wireless Phone";
				}
				//Texting
				menuApt.MenuItems.Add("-");
				menuApt.MenuItems[menuApt.MenuItems.Count-1].Name="Text Div";
				menuApt.MenuItems.Add(Lan.g(this,"Send Text"),menuApt_Click);
				menuApt.MenuItems[menuApt.MenuItems.Count-1].Name="Send Text";
				if(!SmsPhones.IsIntegratedTextingEnabled() && !Programs.IsEnabled(ProgramName.CallFire)) {
					menuApt.MenuItems[menuApt.MenuItems.Count-1].Enabled=false;
				}
				menuApt.MenuItems.Add(Lan.g(this,"Send Confirmation Text"),menuApt_Click);
				menuApt.MenuItems[menuApt.MenuItems.Count-1].Name="Send Confirmation Text";
				if(!SmsPhones.IsIntegratedTextingEnabled() && !Programs.IsEnabled(ProgramName.CallFire)) {
					menuApt.MenuItems[menuApt.MenuItems.Count-1].Enabled=false;
				}
				//menuApt.MenuItems.Add(Lan.g(this,"Send Reminder Text"),menuApt_Click);
				//if(!SmsPhones.IsIntegratedTextingEnabled() && !Programs.IsEnabled(ProgramName.CallFire)) {
				//	menuApt.MenuItems[menuApt.MenuItems.Count-1].Enabled=false;
				//}

				menuApt.Show(ContrApptSheet2,new Point(e.X,e.Y));
			}
			else {
				ShowDraggableContrApptSingle(thisApptCtrl);
				//mouseOrigin is in ApptSheet coordinates
				mouseOrigin=e.Location;
				contOrigin=thisApptCtrl.Location;
				if(HitTestApptBottom(e.Location)) {
					ResizingAppt=true;
					ResizingOrigH=TempApptSingle.Height;
				}
				else {
					ResizingAppt=false;
				}
			}
			if(PatCur.PatNum!=oldPatNum) {
				//This needs to be called after ShowDraggableContrApptSingle(...)
				//due to S_Contr_PatientSelected(...) indirectly stoping the drag if there are any popups.
				FormOpenDental.S_Contr_PatientSelected(PatCur,true,false);
			}
			else {
				//This is called either way. But we avoid some code by calling it directly here.
				RefreshModuleScreenPatient();
			}
		}

		private void MouseDownNonAppointment(MouseEventArgs e,Graphics grfx,TimeSpan sheetClickedOnTime) {
			if(e.Button==MouseButtons.Right) {
				MenuItem menuEdit=menuBlockout.MenuItems[0];
				MenuItem menuCut=menuBlockout.MenuItems[1];
				MenuItem menuCopy=menuBlockout.MenuItems[2];
				MenuItem menuPaste=menuBlockout.MenuItems[3];
				MenuItem menuDelete=menuBlockout.MenuItems[4];
				int clickedOnBlockCount=0;
				string blockoutFlags="";
				Schedule[] ListForType=Schedules.GetForType(SchedListPeriod,ScheduleType.Blockout,0);
				//List<ScheduleOp> listForSched;
				for(int i=0;i<ListForType.Length;i++) {
					if(ListForType[i].SchedDate.Date!=WeekStartDate.AddDays(SheetClickedonDay).Date) {
						continue;
					}
					if(ListForType[i].StartTime > sheetClickedOnTime
						|| ListForType[i].StopTime <= sheetClickedOnTime) {
						continue;
					}
					//listForSched=ScheduleOps.GetForSched(ListForType[i].ScheduleNum);
					for(int p=0;p<ListForType[i].Ops.Count;p++) {
						if(ListForType[i].Ops[p]==SheetClickedonOp) {
							clickedOnBlockCount++;
							blockoutFlags=Defs.GetDef(DefCat.BlockoutTypes,ListForType[i].BlockoutType).ItemValue;
							break;//out of ops loop
						}
					}
				}
				if(clickedOnBlockCount>0) {
					if(blockoutFlags.Contains(BlockoutType.DontCopy.GetDescription())) {
						menuEdit.Enabled=true;
						menuCut.Enabled=false;
						menuCopy.Enabled=false;
						menuPaste.Enabled=false;//Can't paste on top of an existing blockout
						menuDelete.Enabled=true;
						if(clickedOnBlockCount>1) {
							FormPopupFade.ShowMessage(this,"There are multiple blockouts in this slot.  You should try to delete or move one of them.");
						}
					}
					else {
						menuEdit.Enabled=true;
						menuCut.Enabled=true;
						menuCopy.Enabled=true;
						menuPaste.Enabled=false;//Can't paste on top of an existing blockout
						menuDelete.Enabled=true;
						if(clickedOnBlockCount>1) {
							FormPopupFade.ShowMessage(this,"There are multiple blockouts in this slot.  You should try to delete or move one of them.");
						}
					}
				}
				else {
					menuEdit.Enabled=false;
					menuCut.Enabled=false;
					menuCopy.Enabled=false;
					if(BlockoutClipboard==null) {
						menuPaste.Enabled=false;
					}
					else {
						menuPaste.Enabled=true;
					}
					menuDelete.Enabled=false;
				}
				if(!SmsPhones.IsIntegratedTextingEnabled()) {
					MenuItem[] arrMenuItems=menuBlockout.MenuItems.Find(MenuItemNames.TextAsapList,false);
					if(arrMenuItems.Length > 0) {
						arrMenuItems[0].Visible=false;
					}
				}
				else if(!PrefC.GetBool(PrefName.WebSchedAsapEnabled)) {
					MenuItem[] arrMenuItems=menuBlockout.MenuItems.Find(MenuItemNames.TextAsapList,false);
					if(arrMenuItems.Length > 0) {
						arrMenuItems[0].Text="Text ASAP List (manual)";
					}
				}
				//Fun, but not needed----
				menuBlockout.MenuItems[menuBlockout.MenuItems.Count-1].Text="Update Provs on Future Appts ("+Operatories.GetOperatory(SheetClickedonOp).Abbrev+")";
				menuBlockout.Show(ContrApptSheet2,new Point(e.X,e.Y));
			}
		}

		///<summary>Hide it, cleanup the double buffer bitmap, turn off mouseIsDown, boolAptMoved.</summary>
		private void HideDraggableContrApptSingle() {
			//Setting visible here may cause a temporary gray outline of TempApptSingle while the threaded drawing completes.
			//This issue does not occur on most modestly sized appointment views so it was not worth solving.
			TempApptSingle.Visible=false;
			TempApptSingle.DisposeShadow();
			mouseIsDown=false;
			boolAptMoved=false;
		}

		///<summary>Reset TempApptSingle fields and recreate the double buffer bitmap. Also sets mouseIsDown=true.</summary>
		private void ShowDraggableContrApptSingle(ContrApptSingle fromCtrl) {
			//We are now dragging so set the mouse down flag.
			mouseIsDown=true;
			//Control was already created and added to Controls on load. Just reset the fields here and redraw accordingly.
			TempApptSingle.ResetData(fromCtrl.DataRoww,
						fromCtrl.TableApptFields,
						fromCtrl.TablePatFields,
						ApptSingleDrawing.SetLocation(fromCtrl.DataRoww,0,ApptDrawing.VisOps.Count,0));			
			//Note: the control is now set to Visible=false. This flag gets flipped elsewhere when the mouse moves. We are just preparing it to be shown here.
			TempApptSingle.CreateShadow();
			TempApptSingle.BringToFront();
		}

		///<summary></summary>
		private void ContrApptSheet2_MouseMove(object sender,System.Windows.Forms.MouseEventArgs e) {
			if(!mouseIsDown) {
				InfoBubbleDraw(e.Location);
				//decide what the pointer should look like.
				if(HitTestApptBottom(e.Location)) {
					Cursor=Cursors.SizeNS;
				}
				else {
					Cursor=Cursors.Default;
				}
				return;
			}
			//if resizing an appointment
			if(ResizingAppt) {
				TempApptSingle.Location=new Point(
					contOrigin.X+ContrApptSheet2.Location.X+panelSheet.Location.X+1,//the 1 is an unknown factor
					contOrigin.Y+ContrApptSheet2.Location.Y+panelSheet.Location.Y+1);//ditto
				TempApptSingle.Height=ResizingOrigH+e.Y-mouseOrigin.Y;
				if(TempApptSingle.Height<4) {//unhandled exception if smaller.
					TempApptSingle.Height=4;
				}
				TempApptSingle.CreateShadow();
				TempApptSingle.Visible=true;
				return;
			}
			int thisIndex=GetIndex(ContrApptSingle.SelectedAptNum);
			if((Math.Abs(e.X-mouseOrigin.X)<3)//enhances double clicking
				&&(Math.Abs(e.Y-mouseOrigin.Y)<3)) {
				boolAptMoved=false;
				return;
			}
			boolAptMoved=true;
			TempApptSingle.Location=new Point(
				contOrigin.X+e.X-mouseOrigin.X+ContrApptSheet2.Location.X+panelSheet.Location.X,
				contOrigin.Y+e.Y-mouseOrigin.Y+ContrApptSheet2.Location.Y+panelSheet.Location.Y);
			TempApptSingle.Visible=true;
		}

		///<summary>Used by parent form when a dialog needs to be displayed on the mouse down.</summary>
		public void MouseUpForced() {
			if(pinBoard.SelectedIndex!=-1) {
				CancelPinMouseDown=true;
			}
			HideDraggableContrApptSingle();			
		}

		///<summary>Usually dropping an appointment to a new location.</summary>
		private void ContrApptSheet2_MouseUp(object sender,System.Windows.Forms.MouseEventArgs e) {
			if(!mouseIsDown) {
				return;
			}
			//try/finally only. No catch. We probably want a handy exception to popup if there is a real bug.
			//Any return from this point forward will cause HideDraggableContrApptSingle() and ResizingAppt=false.
			try {
				int thisIndex=GetIndex(ContrApptSingle.SelectedAptNum);
				Appointment aptOld;
				if(ResizingAppt) {
					#region Resizing, not moving.
					if(!TempApptSingle.Visible) {//click with no drag
						return;
					}
					//convert Bottom to a time
					int hr=ApptDrawing.ConvertToHour
					(TempApptSingle.Bottom-ContrApptSheet2.Location.Y-panelSheet.Location.Y);
					int minute=ApptDrawing.ConvertToMin
					(TempApptSingle.Bottom-ContrApptSheet2.Location.Y-panelSheet.Location.Y);
					TimeSpan bottomSpan=new TimeSpan(hr,minute,0);
					//subtract to get the new length of appt
					TimeSpan newspan=bottomSpan-TempApptSingle.AptDateTime.TimeOfDay;
					//check if the appointment is being dragged to the next day
					if(TempApptSingle.AptDateTime.Day!=(TempApptSingle.AptDateTime+newspan).Day) {
						MsgBox.Show(this,"You cannot have an appointment that starts and ends on different days.");
						return;
					}
					int newpatternL=(int)newspan.TotalMinutes/5;
					if(newpatternL<ApptDrawing.MinPerIncr/5) {//eg. if 1 < 10/5, would make appt too short. 
						newpatternL=ApptDrawing.MinPerIncr/5;//sets new pattern length at one increment, typically 2 or 3 5min blocks
					}
					else if(newpatternL>78) {//max length of 390 minutes
						newpatternL=78;
					}
					string pattern=TempApptSingle.Pattern;
					if(newpatternL<pattern.Length) {//shorten to match new pattern length
						pattern=pattern.Substring(0,newpatternL);
					}
					else if(newpatternL>pattern.Length) {//make pattern longer.
						pattern=pattern.PadRight(newpatternL,'/');
					}
					//Now, check for overlap with other appts.
					DateTime aptDateTimeBeingChanged=TempApptSingle.AptDateTime;
					//Loop through all other appts in the op and make sure the new pattern will not overlap.
					foreach(ContrApptSingle ctrl in ContrApptSheet2.ListContrApptSingles.FindAll(x => x.OpNum==TempApptSingle.OpNum && x.AptNum!=TempApptSingle.AptNum)) {
						DateTime aptDateTimeInSameOp=ctrl.AptDateTime;
						if(ApptDrawing.IsWeeklyView&&aptDateTimeInSameOp.Date!=aptDateTimeBeingChanged.Date) {
							continue;
						}
						if(aptDateTimeInSameOp<aptDateTimeBeingChanged) {
							continue;//we don't care about appointments that are earlier than this one
						}
						if(aptDateTimeInSameOp.TimeOfDay<aptDateTimeBeingChanged.TimeOfDay+TimeSpan.FromMinutes(5*pattern.Length)) {
							//New pattern overlaps so back it up to butt up against this appt. This will shorten the desired pattern to preven overlap.
							newspan=aptDateTimeInSameOp.TimeOfDay-aptDateTimeBeingChanged.TimeOfDay;
							newpatternL=(int)newspan.TotalMinutes/5;
							pattern=pattern.Substring(0,newpatternL);
						}
					}
					//Check for any overlap with blockouts with the "No Schedule" flag and shorten the pattern
					Appointment curApt=Appointments.GetOneApt(TempApptSingle.AptNum);
					if(ApptIsNull(curApt)) { return; }
					curApt.Pattern=pattern;
					DateTime datePrevious=curApt.DateTStamp;
					Schedule overlappingBlockout=Appointments.GetOverlappingBlockouts(curApt).OrderBy(x => x.StartTime).FirstOrDefault();
					if(overlappingBlockout!=null) {
						//Figure out the amount of time between them and divide by 5 because of how time patterns are stored for appointments.  
						//This happens when resizing, so we assume the appointment doesn't start on the blockout.
						newpatternL=(int)(overlappingBlockout.SchedDate.Add(overlappingBlockout.StartTime)-curApt.AptDateTime).TotalMinutes/5;
						pattern=pattern.Substring(0,newpatternL);	//Minimum newpatternL is 1, because an appointment can't be places on a valid blockout
					}
					if(pattern=="") {
						pattern="///";
					}
					Appointments.SetPattern(curApt,pattern); //Appointments S-Class handles Signalods
					SecurityLogs.MakeLogEntry(Permissions.AppointmentEdit,PatCur.PatNum,Lan.g(this,"Appointment resized from the appointment module."),
						TempApptSingle.AptNum,datePrevious);//Generate FKey to the appointment to show the audit entry in the ApptEdit window.
					RefreshModuleDataPatient(PatCur.PatNum);
					FormOpenDental.S_Contr_PatientSelected(PatCur,true,false);
					RefreshPeriod(true);
					#endregion Resizing, not moving.
					return;					
				}//end if(ResizingAppt)
				if((Math.Abs(e.X-mouseOrigin.X)<7) && (Math.Abs(e.Y-mouseOrigin.Y)<7)) { //Mouse has not moved enough to be considered an appt move.
					boolAptMoved=false;
				}
				if(!boolAptMoved) { //Click with no drag.
					return;
				}
				if(TempApptSingle.Location.X>ContrApptSheet2.Width) { //Dragging to pinboard, so place a copy there.
					#region Dragging to pinboard
					if(!Security.IsAuthorized(Permissions.AppointmentMove)
						||PatRestrictionL.IsRestricted(PatCur.PatNum,PatRestrict.ApptSchedule))//PatCur is updated in ContrApptSheet2_MouseDown 
					{
						return;
					}
					//cannot allow moving completed procedure because it could cause completed procs to change date.  Security must block this.
					if(TempApptSingle.AptStatus==ApptStatus.Complete) {//complete
						MsgBox.Show(this,"Not allowed to move completed appointments.");
						return;
					}
					int prevSel=GetIndex(ContrApptSingle.SelectedAptNum);
					List<long> list=new List<long>();
					list.Add(ContrApptSingle.SelectedAptNum);
					SendToPinboard(list);//sets selectedAptNum=-1. do before refresh prev
					if(prevSel!=-1) {
						ContrApptSheet2.DoubleBufferDraw(drawToScreen: true);
					}
					RefreshModuleDataPatient(PatCur.PatNum);
					FormOpenDental.S_Contr_PatientSelected(PatCur,true,false);
					#endregion Dragging to pinboard
					return;
				}
				//We go this far so we are moving the appt to a new location.
				Appointment apt=Appointments.GetOneApt(ContrApptSingle.SelectedAptNum);
				//This hook is specifically called after we know a valid appointment has been identified.
				Plugins.HookAddCode(this, "ContrAppt.ContrApptSheet2_MouseUp_validation_end",apt);
				aptOld=apt.Copy();
				int tHr=ApptDrawing.ConvertToHour
					(TempApptSingle.Location.Y-ContrApptSheet2.Location.Y-panelSheet.Location.Y);
				int tMin=ApptDrawing.ConvertToMin
					(TempApptSingle.Location.Y-ContrApptSheet2.Location.Y-panelSheet.Location.Y);
				long opNum=ApptDrawing.VisOps[ApptDrawing.ConvertToOp(TempApptSingle.Location.X-ContrApptSheet2.Location.X)].OperatoryNum;
				bool timeWasMoved=tHr!=apt.AptDateTime.Hour
				|| tMin!=apt.AptDateTime.Minute;
				bool isOpChanged=true;
				if(opNum==apt.Op) {
					isOpChanged=false;
				}
				if(timeWasMoved||isOpChanged) {//no question for notes
					#region Prompt or block
					if(apt.AptStatus==ApptStatus.PtNote|apt.AptStatus==ApptStatus.PtNoteCompleted) {
						if(!Security.IsAuthorized(Permissions.AppointmentMove)||PatRestrictionL.IsRestricted(apt.PatNum,PatRestrict.ApptSchedule)) {
							return;
						}
					}
					else {
						if(!Security.IsAuthorized(Permissions.AppointmentMove)
							||(apt.AptStatus==ApptStatus.Complete&&(!Security.IsAuthorized(Permissions.AppointmentCompleteEdit)))
							||PatRestrictionL.IsRestricted(apt.PatNum,PatRestrict.ApptSchedule)
							||!MsgBox.Show(this,true,"Move Appointment?"))
						{
							return;
						}
					}
					#endregion
				}
				//convert loc to new date/time
				DateTime tDate=apt.AptDateTime.Date;
				if(ApptDrawing.IsWeeklyView) {
					tDate=WeekStartDate.AddDays(ApptDrawing.ConvertToDay(TempApptSingle.Location.X-ContrApptSheet2.Location.X));
				}
				apt.AptDateTime=new DateTime(tDate.Year,tDate.Month,tDate.Day,tHr,tMin,0);
				//Compare beginning of new appointment against end to see if the appointment spans two days
				if(apt.AptDateTime.Day!=apt.AptDateTime.AddMinutes(apt.Pattern.Length*5).Day) {
					MsgBox.Show(this,"You cannot have an appointment that starts and ends on different days.");
					return;
				}
				#region Prevent double-booking
				if(AppointmentRules.GetCount() > 0) { //If no rules exist then don't bother checking.
					List<Procedure> procsMultApts=Procedures.GetProcsMultApts(ContrApptSheet2.ListAptNums);
					Procedure[] procsForOne=Procedures.GetProcsOneApt(apt.AptNum,procsMultApts);
					ArrayList doubleBookedCodes=
					AppointmentL.GetDoubleBookedCodes(apt,_dtAppointments.Copy(),procsMultApts,procsForOne);
					if(doubleBookedCodes.Count>0) {//if some codes would be double booked
						if(AppointmentRules.IsBlocked(doubleBookedCodes)) {
							MessageBox.Show(Lan.g(this,"Not allowed to double book:")+" "
								+AppointmentRules.GetBlockedDescription(doubleBookedCodes));
							return;
						}
					}
				}
				#endregion
				Operatory curOp=ApptDrawing.VisOps[ApptDrawing.ConvertToOp(TempApptSingle.Location.X-ContrApptSheet2.Location.X)];
				MoveAppointments(new List<Appointment>() { apt },new List<Appointment>() { aptOld },curOp,timeWasMoved,isOpChanged);//Apt's time has already been changed at this point.  Internally calls Appointments S-class to insert invalid signal.
				#region Update UI and cache
				InsPlan aptInsPlan1 = InsPlans.GetPlan(apt.InsPlan1,null);//we only care about lining the fees up with the primary insurance plan
				//check if the proc fees on the moved appointment need updating
				List<Procedure> procsForApt = Procedures.GetProcsForSingle(apt.AptNum,false);//get the procedures on the appointment
				bool isUpdatingFees = false;
				List<Procedure> listProcsNew = procsForApt.Select(x => Procedures.UpdateProcInAppointment(aptOld,x.Copy())).ToList();
				if(procsForApt.Exists(x => x.ProvNum!=listProcsNew.FirstOrDefault(y => y.ProcNum==x.ProcNum).ProvNum)) {//Either the primary or hygienist changed.
					string promptText = "";
					isUpdatingFees=Procedures.FeeUpdatePromptHelper(listProcsNew,procsForApt,aptInsPlan1,ref promptText);
					if(isUpdatingFees) {//Made it pass the pref check.
						if(promptText!="" && !MsgBox.Show(this,MsgBoxButtons.YesNo,promptText)) {
							isUpdatingFees=false;
						}
					}
				}
				Procedures.SetProvidersInAppointment(apt,procsForApt,isUpdatingFees);
				RefreshModuleDataPatient(PatCur.PatNum);
				FormOpenDental.S_Contr_PatientSelected(PatCur,true,false);
				RefreshPeriod();				
				Recalls.SynchScheduledApptFull(apt.PatNum);
				#endregion Update UI and cache
				Plugins.HookAddCode(this, "ContrAppt.ContrApptSheet2_MouseUp_end",apt,aptOld);
			}
			finally { //Cleanup. We are done with mouse up so we can't possibly be resizing or moving an appt.
				ResizingAppt=false;
				HideDraggableContrApptSingle();
			}
		}

		private void ContrApptSheet2_MouseLeave(object sender,EventArgs e) {
			InfoBubbleDraw(new Point(-1,-1));
			timerInfoBubble.Enabled=false;//redundant?
			Cursor=Cursors.Default;
			Plugins.HookAddCode(this,"ContrAppt.ContrApptSheet2_MouseLeave_end");
		}

		///<summary></summary>
		private void InfoBubble_MouseMove(object sender,System.Windows.Forms.MouseEventArgs e) {
			//Calculate the real point in sheet coordinates
			Point p=new Point(e.X+infoBubble.Left-ContrApptSheet2.Left-panelSheet.Left,
				e.Y+infoBubble.Top-ContrApptSheet2.Top-panelSheet.Top);
			InfoBubbleDraw(p);
		}

		///<summary></summary>
		private void PicturePat_MouseMove(object sender,System.Windows.Forms.MouseEventArgs e) {
			//Calculate the real point in sheet coordinates
			Point p=new Point(e.X+infoBubble.Left+PicturePat.Left-ContrApptSheet2.Left-panelSheet.Left,
				e.Y+infoBubble.Top+PicturePat.Top-ContrApptSheet2.Top-panelSheet.Top);
			InfoBubbleDraw(p);
		}

		///<summary>Does a hit test to determine if over an appointment.  Fills the bubble with data and then positions it.</summary>
		private void InfoBubbleDraw(Point p) {
			//remember where to draw for hover effect
			if((comboView.SelectedIndex==0 && PrefC.GetBool(PrefName.AppointmentBubblesDisabled))
					|| (comboView.SelectedIndex>0 && _listApptViews[comboView.SelectedIndex-1].IsApptBubblesDisabled))
			{
				infoBubble.Visible=false;
				timerInfoBubble.Enabled=false;
				return;
			}
			bubbleLocation=p;
			long aptNum=HitTestAppt(p);
			if(aptNum==0 || HitTestApptBottom(p)) {
				if(infoBubble.Visible) {
					infoBubble.Visible=false;
					timerInfoBubble.Enabled=false;
				}
				return;
			}
			int yval=p.Y+ContrApptSheet2.Top+panelSheet.Top+10;//TODO Figure out the Prov bar height
			int xval=p.X+ContrApptSheet2.Left+panelSheet.Left+10;
			if(aptNum==bubbleAptNum) {
				if(DateTime.Now.AddMilliseconds(-280) > bubbleTime | !PrefC.GetBool(PrefName.ApptBubbleDelay)) {
					infoBubble.Visible=true;
					if(yval > panelSheet.Bottom-infoBubble.Height) {
						yval=panelSheet.Bottom-infoBubble.Height;
					}
					infoBubble.Location=new Point(xval,yval);
				}
				return;
			}
			if(aptNum!=bubbleAptNum) {
				//reset timer for popup delay
				timerInfoBubble.Enabled=false;
				timerInfoBubble.Enabled=true;
				//delay for hover effect 0.28 sec
				bubbleTime=DateTime.Now;
				bubbleAptNum=aptNum;
				//most data is already present in DS.Appointment, but we do need to get the patient picture
				bool hasPatientPicture=false;
				for(int i=0;i<_aptBubbleDefs.Count;i++) {
					if(_aptBubbleDefs[i].InternalName=="Patient Picture") {
						hasPatientPicture=true;
					}
				}
				infoBubble.BackgroundImage=new Bitmap(infoBubble.Width,800);
				Image img=infoBubble.BackgroundImage;//alias
				Graphics g=Graphics.FromImage(img);//infoBubble.BackgroundImage);
				g.TextRenderingHint=TextRenderingHint.ClearTypeGridFit;
				g.SmoothingMode=SmoothingMode.HighQuality;
				g.FillRectangle(new SolidBrush(infoBubble.BackColor),0,0,img.Width,img.Height);
				ContrApptSingle row=ContrApptSheet2.ListContrApptSingles.FirstOrDefault(c => c.AptNum==aptNum);
				if(row==null) { //Just in case.
					return;
				}
				if(!hasPatientPicture) {
					infoBubble.Controls.Remove(PicturePat);
				}
				else {
					PicturePat.Location=new Point(6,17);
					if(!infoBubble.Controls.Contains(PicturePat)) {
						infoBubble.Controls.Add(PicturePat);
					}
					PicturePat.Image=null;
					if(row.ImageFolder!=""
						&& (PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ || CloudStorage.IsCloudStorage))//Do not use patient image when A to Z folders are disabled.
					{
						try {
							Bitmap patPict;
							Documents.GetPatPict(row.PatNum,
								ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),
									row.ImageFolder.Substring(0,1).ToUpper(),
									row.ImageFolder,""),
									out patPict);
							PicturePat.Image=patPict;
						}
						catch(ApplicationException) { }  //A customer called in and an exception got through.  Added exception parameter as attempted fix.
					}
				}
				Font font=new Font(FontFamily.GenericSansSerif,9f);
				Brush brush=Brushes.Black;
				float x=0;
				float y=0;
				float h=0;
				float rowH=g.MeasureString("X",font).Height;
				for(int i=0;i<_aptBubbleDefs.Count;i++) {
					if(i==0) {
						font=new Font(FontFamily.GenericSansSerif,10f,FontStyle.Bold);
						x=8;
						y=0;
					}
					if(i==1) {
						font=new Font(FontFamily.GenericSansSerif,9f);
						y-=3;
						if(hasPatientPicture) {
							x=110;
							PicturePat.Location=new Point(PicturePat.Location.X,(int)y+5);
						}
						else {
							x=2;
						}
					}
					if(hasPatientPicture && y>=(PicturePat.Height+PicturePat.Location.Y)) {
						x=2;
					}
					switch(_aptBubbleDefs[i].InternalName) {
						case "Patient Name":
							h=g.MeasureString(row.PatientName,font,infoBubble.Width-(int)x).Height;
							g.DrawString(row.PatientName,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
							y+=h;
							continue;
						case "Patient Picture":
							//We have already dealt with this above.
							continue;
						case "Appt Day":
							h=g.MeasureString(row.AptDay,font,infoBubble.Width-(int)x).Height;
							g.DrawString(row.AptDay,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
							y+=h;
							continue;
						case "Appt Date":
							h=g.MeasureString(row.AptDate,font,infoBubble.Width-(int)x).Height;
							g.DrawString(row.AptDate,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
							y+=h;
							continue;
						case "Appt Time":
							h=g.MeasureString(row.AptTime,font,infoBubble.Width-(int)x).Height;
							g.DrawString(row.AptTime,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
							y+=h;
							continue;
						case "Appt Length":
							h=g.MeasureString(row.AptLength,font,infoBubble.Width-(int)x).Height;
							g.DrawString(row.AptLength,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
							y+=h;
							continue;
						case "Provider":
							h=g.MeasureString(row.Provider,font,infoBubble.Width-(int)x).Height;
							g.DrawString(row.Provider,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
							y+=h;
							continue;
						case "Production":
							h=g.MeasureString(row.Production,font,infoBubble.Width-(int)x).Height;
							g.DrawString(row.Production,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
							y+=h;
							continue;
						case "Confirmed":
							h=g.MeasureString(row.ConfirmedFromDef,font,infoBubble.Width-(int)x).Height;
							g.DrawString(row.ConfirmedFromDef,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
							y+=h;
							continue;
						case "ASAP":
							if(row.Priority==ApptPriority.ASAP) {
								h=g.MeasureString(Lan.g(this,"ASAP"),font,infoBubble.Width-(int)x).Height;
								g.DrawString(Lan.g(this,"ASAP"),font,Brushes.Red,new RectangleF(x,y,infoBubble.Width-(int)x,h));
								y+=h;
							}
							continue;
						case "Med Flag":
							if(row.PreMedFlag!="") {
								h=g.MeasureString(row.PreMedFlag,font,infoBubble.Width-(int)x).Height;
								g.DrawString(row.PreMedFlag,font,Brushes.Red,new RectangleF(x,y,infoBubble.Width-(int)x,h));
								y+=h;
							}
							continue;
						case "Med Note":
							if(row.MedUrgNote!="") {
								h=g.MeasureString(row.MedUrgNote,font,infoBubble.Width-(int)x).Height;
								g.DrawString(row.MedUrgNote,font,Brushes.Red,new RectangleF(x,y,infoBubble.Width-(int)x,h));
								y+=h;
							}
							continue;
						case "Lab":
							if(row.Lab!="") {
								h=g.MeasureString(row.Lab,font,infoBubble.Width-(int)x).Height;
								g.DrawString(row.Lab,font,Brushes.Red,new RectangleF(x,y,infoBubble.Width-(int)x,h));
								y+=h;
							}
							continue;
						case "Procedures":
							if(row.Procs!="") {
								h=g.MeasureString(row.Procs,font,infoBubble.Width-(int)x).Height;
								g.DrawString(row.Procs,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
								y+=h;
							}
							continue;
						case "Note":
							string noteStr=row.Note;
							int maxNoteLength=PrefC.GetInt(PrefName.AppointmentBubblesNoteLength);
							if(noteStr.Trim()!="" && maxNoteLength>0 && noteStr.Length>maxNoteLength) {//Trim text
								noteStr=noteStr.Substring(0,maxNoteLength)+"...";
							}
							if(noteStr.Trim()!="") { //draw text
								h=g.MeasureString(noteStr,font,infoBubble.Width-(int)x).Height;
								g.DrawString(noteStr,font,Brushes.Blue,new RectangleF(x,y,infoBubble.Width-(int)x,h));
								y+=h;
							}
							continue;
						case "PatNum":
							h=g.MeasureString(row.PatNum.ToString(),font,infoBubble.Width-(int)x).Height;
							g.DrawString(row.PatNum.ToString(),font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
							y+=h;
							continue;
						case "ChartNum":
							if(row.ChartNumber!="") {
								h=g.MeasureString(row.ChartNumber,font,infoBubble.Width-(int)x).Height;
								g.DrawString(row.ChartNumber,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
								y+=h;
							}
							continue;
						case "Billing Type":
							h=g.MeasureString(row.BillingType,font,infoBubble.Width-(int)x).Height;
							g.DrawString(row.BillingType,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
							y+=h;
							continue;
						case "Horizontal Line":
							y+=3;
							g.DrawLine(new Pen(Brushes.Gray,1.5f),3,y,infoBubble.Width-3,y);
							y+=2;
							continue;
						case "Age":
							h=g.MeasureString(row.Age,font,infoBubble.Width-(int)x).Height;
							g.DrawString(row.Age,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
							y+=h;
							continue;
						case "Home Phone":
							h=g.MeasureString(row.HmPhone,font,infoBubble.Width-(int)x).Height;
							g.DrawString(row.HmPhone,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
							y+=h;
							continue;
						case "Work Phone":
							h=g.MeasureString(row.WkPhone,font,infoBubble.Width-(int)x).Height;
							g.DrawString(row.WkPhone,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
							y+=h;
							continue;
						case "Wireless Phone":
							h=g.MeasureString(row.WirelessPhone,font,infoBubble.Width-(int)x).Height;
							g.DrawString(row.WirelessPhone.ToString(),font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
							y+=h;
							continue;
						case "Contact Methods":
							if(row.ContactMethods!="") {
								h=g.MeasureString(row.ContactMethods,font,infoBubble.Width).Height;
								g.DrawString(row.ContactMethods,font,brush,new RectangleF(x,y,infoBubble.Width,h));
								y+=h;
							}
							continue;
						case "Insurance":
							if(row.Insurance!="") {//overkill since it's only one line
								h=g.MeasureString(row.Insurance,font,infoBubble.Width-(int)x).Height;
								g.DrawString(row.Insurance,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
								y+=h;
							}
							continue;
						case "Insurance Color":
							if(row.Insurance!="") {
								string[] insuranceArray=row.Insurance.Split(new string[] { "\r\n" },StringSplitOptions.None);
								foreach(string str in insuranceArray) {
									h=g.MeasureString(str,font,infoBubble.Width-(int)x).Height;
									Carrier carrier=Carriers.GetCarrierByName(str.Replace("Ins1: ","").Replace("Ins2: ",""));
									if(carrier!=null && carrier.ApptTextBackColor!=Color.Black) {
										g.FillRectangle(new SolidBrush(carrier.ApptTextBackColor),x-2,y+1,infoBubble.Width-(int)x+2,h); 
									}
									g.DrawString(str,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
									y+=h;
								}
							}
							continue;
						case "Address Note":
							if(row.AddrNote.ToString()!="") {
								h=g.MeasureString(row.AddrNote,font,infoBubble.Width-(int)x).Height;
								g.DrawString(row.AddrNote,font,Brushes.Red,new RectangleF(x,y,infoBubble.Width-(int)x,h));
								y+=h;
							}
							continue;
						case "Fam Note":
							if(row.FamFinUrgNote!="") {
								h=g.MeasureString(row.FamFinUrgNote,font,infoBubble.Width-(int)x).Height;
								g.DrawString(row.FamFinUrgNote,font,Brushes.Red,new RectangleF(x,y,infoBubble.Width-(int)x,h));
								y+=h;
							}
							continue;
						case "Appt Mod Note":
							if(row.ApptModNote!="") {
								h=g.MeasureString(row.ApptModNote,font,infoBubble.Width-(int)x).Height;
								g.DrawString(row.ApptModNote,font,Brushes.Red,new RectangleF(x,y,infoBubble.Width-(int)x,h));
								y+=h;
							}
							continue;
						case "ReferralFrom":
							if(row.ReferralFrom!="") {
								h=g.MeasureString(row.ReferralFrom,font,infoBubble.Width-(int)x).Height;
								g.DrawString(row.ReferralFrom,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
								y+=h;
							}
							continue;
						case "ReferralTo":
							if(row.ReferralTo!="") {
								h=g.MeasureString(row.ReferralTo,font,infoBubble.Width-(int)x).Height;
								g.DrawString(row.ReferralTo,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
								y+=h;
							}
							continue;
						case "Language":
							if(row.Language!="") {
								h=g.MeasureString(row.Language,font,infoBubble.Width-(int)x).Height;
								g.DrawString(row.Language,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
								y+=h;
							}
							continue;
						case "Email":
							if(row.Email!="") {
								h=g.MeasureString(row.Email,font,infoBubble.Width-(int)x).Height;
								g.DrawString(row.Email,font,brush,new RectangleF(x,y,infoBubble.Width-(int)x,h));
								y+=h;
							}
							continue;
					}
				}
				//other family members?
				if(hasPatientPicture && y<PicturePat.Height+PicturePat.Location.Y) {
					y=PicturePat.Height+PicturePat.Location.Y;
				}
				g.DrawRectangle(Pens.Gray,0,0,infoBubble.Width-1,(int)y+2);
				g.Dispose();
				infoBubble.Size=new Size(infoBubble.Width,(int)y+3);
				infoBubble.BringToFront();
			}
			if(yval > panelSheet.Bottom-infoBubble.Height) {
				yval=panelSheet.Bottom-infoBubble.Height;
			}
			infoBubble.Location=new Point(xval,yval);
			/*only show right away if option set for no delay, otherwise, it will not show
			until mouse had hovered for at least 0.28 seconds(arbitrary #)
			the timer fires at 0.30 seconds, so the difference was introduced because
			of what seemed to be inconsistencies in the timer function */
			if(!PrefC.GetBool(PrefName.ApptBubbleDelay)) {
				infoBubble.Visible=true;
			}
			else {
				infoBubble.Visible=false;
			}
		}

		///<summary>Double click on appt sheet or on a single appointment.</summary>
		private void ContrApptSheet2_DoubleClick(object sender,System.EventArgs e) {
			if(Plugins.HookMethod(this,"ContrApptSheet2_DoubleClick_start",ContrApptSingle.ClickedAptNum,e)) {
				return;
			}
			HideDraggableContrApptSingle();
			//this logic is a little different than mouse down for now because on the first click of a 
			//double click, an appointment control is created under the mouse.
			if(ContrApptSingle.ClickedAptNum!=0) {//on appt
				long patnum=TempApptSingle.PatNum;
				if(ApptIsNull(Appointments.GetOneApt(ContrApptSingle.ClickedAptNum))) {
					return;
				}
				//security handled inside the form
				FormApptEdit FormAE=new FormApptEdit(ContrApptSingle.ClickedAptNum);
				FormAE.ShowDialog();
				if(FormAE.DialogResult==DialogResult.OK) {
					Appointment apt=Appointments.GetOneApt(ContrApptSingle.ClickedAptNum);
					if(apt!=null) {
						Appointment aptOld=apt.Copy(); //this needs to happen before TryAdjustAppointmentPattern.
						if(TryAdjustAppointmentPattern(apt)) {
							MsgBox.Show(this,"Appointment is too long and would overlap another appointment.  Automatically shortened to fit.");
							try {
								Appointments.Update(apt,aptOld);//Appointments S-Class handles Signalods
							}
							catch(ApplicationException ex) {
								MessageBox.Show(ex.Message);
							}
						}
					}
					ModuleSelected(patnum);//apt.PatNum);//apt might be null if user deleted appt.
				}
				else if(FormAE.DialogResult==DialogResult.Cancel && FormAE.HasProcsChangedAndCancel) { //If user canceled but changed the procs on appt first
					//Refresh the grid, don't need to check length because it didn't change.  Plus user might not want to change length.
					ModuleSelected(patnum);
					Signalods.SetInvalidAppt(FormAE.GetAppointmentOld());//use old here because they cancelled.  Only calling this because there is no S-Class call.
				}
			}
			//not on apt, so trying to schedule an appointment---------------------------------------------------------------------
			else {
				if(!Security.IsAuthorized(Permissions.AppointmentCreate)) {
					return;
				}
				if(ApptDrawing.VisOps.Count==0) {//no ops visible.
					return;
				}
				//First we'll check to see if the location the user clicked on is a blockout with the type NoSchedule
				DateTime dateSelected=AppointmentL.DateSelected;
				int minutes=(SheetClickedonMin/ApptDrawing.MinPerIncr)*ApptDrawing.MinPerIncr;
				DateTime dateTimeStart=new DateTime(dateSelected.Year,dateSelected.Month,dateSelected.Day,SheetClickedonHour,minutes,0);
				if(Appointments.CheckTimeForBlockoutOverlap(dateTimeStart,SheetClickedonOp)) {
					MsgBox.Show(this, "Appointment cannot be created on a blockout marked as 'Block appointment scheduling'");
					return;
				}
				FormPatientSelect FormPS=new FormPatientSelect();
				if(PatCur!=null) {
					FormPS.InitialPatNum=PatCur.PatNum;
				}
				FormPS.ShowDialog();
				if(FormPS.DialogResult!=DialogResult.OK) {
					return;
				}
				if(PatCur==null || FormPS.SelectedPatNum!=PatCur.PatNum) {//if the patient was changed
					RefreshModuleDataPatient(FormPS.SelectedPatNum);
					FormOpenDental.S_Contr_PatientSelected(PatCur,true,false);
				}
				if(PatCur!=null && PatRestrictionL.IsRestricted(PatCur.PatNum,PatRestrict.ApptSchedule)) {
					return;
				}
				if(AppointmentL.PromptForMerge(PatCur,out PatCur)) {
					RefreshModuleDataPatient(PatCur.PatNum);
					FormOpenDental.S_Contr_PatientSelected(PatCur,true,false);
				}
				if(PatCur!=null && PatCur.PatStatus.In(PatientStatus.Archived,PatientStatus.Deceased)) {
					MsgBox.Show(this,"Appointments cannot be scheduled for "+PatCur.PatStatus.ToString().ToLower()+" patients.");
					return;
				}
				Appointment apt=null;
				bool updateAppt=false;
				if(FormPS.NewPatientAdded) {
					Operatory curOp=Operatories.GetOperatory(SheetClickedonOp);
					if(ApptDrawing.IsWeeklyView) {
						dateSelected=WeekStartDate.AddDays(SheetClickedonDay);
					}
					//minutes always rounded down.
					DateTime dateTimeAskedToArrive=DateTime.MinValue;
					if(PatCur.AskToArriveEarly > 0) {
						dateTimeAskedToArrive=dateTimeStart.AddMinutes(-PatCur.AskToArriveEarly);
						MessageBox.Show(Lan.g(this,"Ask patient to arrive")+" "+PatCur.AskToArriveEarly
							+" "+Lan.g(this,"minutes early at")+" "+dateTimeAskedToArrive.ToShortTimeString()+".");
					}
					apt=Appointments.CreateApptForNewPatient(PatCur,curOp,dateTimeStart,dateTimeAskedToArrive,null,SchedListPeriod);
					//New patient. Set to prospective if operatory is set to set prospective.
					if(curOp.SetProspective) {
						if(MsgBox.Show(this,MsgBoxButtons.OKCancel,"Patient's status will be set to Prospective.")) {
							Patient patOld=PatCur.Copy();
							PatCur.PatStatus=PatientStatus.Prospective;
							Patients.Update(PatCur,patOld);
						}
					}
					FormApptEdit FormAE=new FormApptEdit(apt.AptNum);//this is where security log entry is made
					FormAE.IsNew=true;
					FormAE.ShowDialog();
					if(FormAE.DialogResult==DialogResult.OK) {
						if(apt.IsNewPatient) {
							AutomationL.Trigger(AutomationTrigger.CreateApptNewPat,null,apt.PatNum,apt.AptNum);
						}
						AutomationL.Trigger(AutomationTrigger.CreateAppt,null,apt.PatNum,apt.AptNum);
						RefreshModuleDataPatient(PatCur.PatNum);
						FormOpenDental.S_Contr_PatientSelected(PatCur,true,false);
						if(!HasValidStartTime(apt)) {
							Appointment aptOld=apt.Copy();
							MsgBox.Show(this,"Appointment start time would overlap another appointment.  Moving appointment to pinboard.");
							SendToPinBoard(apt.AptNum);
							apt.AptStatus=ApptStatus.UnschedList;
							try {
								Appointments.Update(apt,aptOld);//Appointments S-Class handles Signalods
							}
							catch(ApplicationException ex) {
								MessageBox.Show(ex.Message);
							}
							RefreshPeriod();
							return;//It's ok to skip the rest of the method here. The appointment is now on the pinboard and must be rescheduled
						}
						apt=Appointments.GetOneApt(apt.AptNum);  //Need to get appt from DB so we have the time pattern
						updateAppt=true;						
					}
				}
				else {//new patient not added
					if(Appointments.HasOutstandingAppts(PatCur.PatNum) | (Plugins.HookMethod(this,"ContrAppt.ContrApptSheet2_DoubleClick_apptOtherShow"))) {
						DisplayOtherDlg(true);
					}
					else {
						FormApptsOther FormAO=new FormApptsOther(PatCur.PatNum);//doesn't actually get shown
						CheckStatus();
						FormAO.InitialClick=true;
						FormAO.MakeAppointment();
						if(FormAO.AptNumsSelected.Count>0) {
							ContrApptSingle.SelectedAptNum=FormAO.AptNumsSelected[0];
						}
						apt=Appointments.GetOneApt(ContrApptSingle.SelectedAptNum);
						updateAppt=true;
					}
				}
				if(updateAppt && apt!=null) {					
					Appointment aptOld=apt.Copy();
					if(TryAdjustAppointmentPattern(apt)) {
						MsgBox.Show(this,"Appointment is too long and would overlap another appointment.  Automatically shortened to fit.");
					}
					try {
						Appointments.Update(apt,aptOld);//Appointments S-Class handles Signalods
					}
					catch(ApplicationException ex) {
						MessageBox.Show(ex.Message);
					}
					RefreshPeriod();
				}
			}
		}

		///<summary>Displays the Other Appointments for the current patient, then refreshes screen as needed.  initialClick specifies whether the user doubleclicked on a blank time to get to this dialog.</summary>
		public void DisplayOtherDlg(bool initialClick) {
			if(PatCur==null) {
				return;
			}
			FormApptsOther FormAO=new FormApptsOther(PatCur.PatNum);
			FormAO.InitialClick=initialClick;
			FormAO.ShowDialog();
			if(FormAO.OResult==OtherResult.Cancel) {
				return;
			}
			switch(FormAO.OResult) {
				case OtherResult.CopyToPinBoard:
					SendToPinboard(FormAO.AptNumsSelected);
					RefreshModuleDataPatient(FormAO.SelectedPatNum);
					FormOpenDental.S_Contr_PatientSelected(PatCur,true,false);
					RefreshPeriod();
					break;
				case OtherResult.NewToPinBoard:
					SendToPinboard(FormAO.AptNumsSelected);
					RefreshModuleDataPatient(FormAO.SelectedPatNum);
					FormOpenDental.S_Contr_PatientSelected(PatCur,true,false);
					RefreshPeriod();
					break;
				case OtherResult.PinboardAndSearch:
					SendToPinboard(FormAO.AptNumsSelected);
					if(ApptDrawing.IsWeeklyView) {
						break;
					}
					dateSearch.Text=FormAO.DateJumpToString;
					if(!groupSearch.Visible) {//if search not already visible
						ShowSearch();
					}
					DoSearch();
					break;
				case OtherResult.CreateNew:
					ContrApptSingle.SelectedAptNum=FormAO.AptNumsSelected[0];
					RefreshModuleDataPatient(FormAO.SelectedPatNum);
					FormOpenDental.S_Contr_PatientSelected(PatCur,true,false);
					Appointment apt=Appointments.GetOneApt(ContrApptSingle.SelectedAptNum);
					Appointment aptOld=apt.Copy();
					if(!HasValidStartTime(apt)) {
						MsgBox.Show(this,"Appointment start time would overlap another appointment.  Moving appointment to pinboard.");
						SendToPinBoard(apt.AptNum);
						apt.AptStatus=ApptStatus.UnschedList;
						try {
							Appointments.Update(apt,aptOld);
						}
						catch(ApplicationException ex) {
							MessageBox.Show(ex.Message);
						}
						RefreshPeriod();
						break;
					}
					if(TryAdjustAppointmentPattern(apt)) {
						MsgBox.Show(this,"Appointment is too long and would overlap another appointment.  Automatically shortened to fit.");						
						try {
							Appointments.Update(apt,aptOld);//Appointments S-Class handles Signalods
						}
						catch(ApplicationException ex) {
							MessageBox.Show(ex.Message);
						}
					}
					RefreshPeriod();
					break;
				case OtherResult.GoTo:
					ContrApptSingle.SelectedAptNum=FormAO.AptNumsSelected[0];
					AppointmentL.DateSelected=PIn.Date(FormAO.DateJumpToString);
					RefreshModuleDataPatient(FormAO.SelectedPatNum);
					FormOpenDental.S_Contr_PatientSelected(PatCur,true,false);
					if((int)AppointmentL.DateSelected.DayOfWeek==0) {//if sunday
						WeekStartDate=AppointmentL.DateSelected.AddDays(-6).Date;//go back to the previous monday
					}
					else {
						WeekStartDate=AppointmentL.DateSelected.AddDays(1-(int)AppointmentL.DateSelected.DayOfWeek).Date;//go back to current monday
					}
					WeekEndDate=WeekStartDate.AddDays(ApptDrawing.NumOfWeekDaysToDisplay-1).Date;
					RefreshPeriod();
					break;
			}
		}

		private void ToolBarMain_ButtonClick(object sender,OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			if(e.Button.Tag.GetType()==typeof(string)) {
				//standard predefined button
				switch(e.Button.Tag.ToString()) {
					case "Lists":
						OnLists_Click();
						break;
					case "Print":
						OnPrint_Click();
						break;
					case "RapidCall":
						try {
							RapidCall.ShowPage();
						}
						catch(Exception ex) {
							MessageBox.Show(ex.Message);
						}
						break;
				}
			}
			else if(e.Button.Tag.GetType()==typeof(Program)) {
				Patient pat=null;
				if(PatCur!=null) {
					pat=Patients.GetPat(PatCur.PatNum);
				}
				ProgramL.Execute(((Program)e.Button.Tag).ProgramNum,pat);
			}
		}

		private void OnUnschedList_Click() {
			//Reselect existing window if available, if not create a new instance
			if(FormUnsched2==null || FormUnsched2.IsDisposed) {
				FormUnsched2=new FormUnsched();
			}
			FormUnsched2.Show();
			if(FormUnsched2.WindowState==FormWindowState.Minimized) {//only applicable if re-using an existing instance
				FormUnsched2.WindowState=FormWindowState.Normal;
			}
			FormUnsched2.BringToFront();
		}

		private void OnASAPList_Click() {
			if(_formASAP==null || _formASAP.IsDisposed) {
				_formASAP=new FormASAP();
			}
			_formASAP.Show();
			if(_formASAP.WindowState==FormWindowState.Minimized) {
				_formASAP.WindowState=FormWindowState.Normal;
			}
			_formASAP.BringToFront();
		}

		private void OnRadiology_Click() {
			List<FormRadOrderList> listFormROLs=Application.OpenForms.OfType<FormRadOrderList>().ToList();
			if(listFormROLs.Count > 0) {
				listFormROLs[0].RefreshRadOrdersForUser(Security.CurUser);
				listFormROLs[0].BringToFront();
			}
			else {
				FormRadOrderList FormPRL=new FormRadOrderList(Security.CurUser);
				FormPRL.Show();
			}
		}

		private void OnInsVerify_Click() {
            List<FormInsVerificationList> listFormROLs=Application.OpenForms.OfType<FormInsVerificationList>().ToList();
			if(listFormROLs.Count>0) {
				listFormROLs[0].FillGrids();
				listFormROLs[0].BringToFront();
			}
			else {
				FormInsVerificationList FormIVL=new FormInsVerificationList();
				FormIVL.FormClosing+=FormIVL_FormClosing;
				FormIVL.Show();
			}
        }

		private void FormIVL_FormClosing(object sender,FormClosingEventArgs e) {
            //Action does not currently need to be taken when leaving the insurance verification list window.
		}

		private void OnRecall_Click() {
			if(FormRecallL==null || FormRecallL.IsDisposed) {
				FormRecallL=new FormRecallList();
			}
			FormRecallL.Show();
			if(FormRecallL.WindowState==FormWindowState.Minimized) {
				FormRecallL.WindowState=FormWindowState.Normal;
			}
			FormRecallL.BringToFront();
		}

		private void OnConfirm_Click() {
			if(FormConfirmL==null || FormConfirmL.IsDisposed) {
				FormConfirmL=new FormConfirmList();
			}
			FormConfirmL.Show();
			if(FormConfirmL.WindowState==FormWindowState.Minimized) {
				FormConfirmL.WindowState=FormWindowState.Normal;
			}
			FormConfirmL.BringToFront();
		}

		private void OnTrack_Click() {
			if(FormTN==null || FormTN.IsDisposed) {
				FormTN=new FormTrackNext();
			}
			FormTN.Show();
			if(FormTN.WindowState==FormWindowState.Minimized) {
				FormTN.WindowState=FormWindowState.Normal;
			}
			FormTN.BringToFront();
		}

		private void OnLists_Click() {
			FormApptLists FormA=new FormApptLists();
			FormA.ShowDialog();
			if(FormA.DialogResult==DialogResult.Cancel) {
				return;
			}
			switch(FormA.SelectionResult) {
				case ApptListSelection.Recall:
					OnRecall_Click();
					break;
				case ApptListSelection.Confirm:
					OnConfirm_Click();
					break;
				case ApptListSelection.Planned:
					OnTrack_Click();
					break;
				case ApptListSelection.Unsched:
					OnUnschedList_Click();
					break;
				case ApptListSelection.ASAP:
					OnASAPList_Click();
					break;
				case ApptListSelection.Radiology:
					OnRadiology_Click();
					break;
				case ApptListSelection.InsVerify:
					OnInsVerify_Click();
					break;
			}
		}

		private void OnPrint_Click() {
			if(ApptDrawing.VisOps.Count==0) {//no ops visible.
				MsgBox.Show(this,"There must be at least one operatory showing in order to Print Schedule.");
				return;
			}
			if(PrinterSettings.InstalledPrinters.Count==0) {
				MessageBox.Show(Lan.g(this,"Printer not installed"));
				return;
			}
			List<long> listVisOpNums=ApptDrawing.VisOps.Select(x => x.OperatoryNum).ToList();
			List<long> listApptNums=ContrApptSheet2.ListContrApptSingles.FindAll(x => listVisOpNums.Contains(x.OpNum)).Select(x => x.AptNum).ToList();
			FormApptPrintSetup FormAPS=new FormApptPrintSetup(listApptNums);
			FormAPS.ShowDialog();
			if(FormAPS.DialogResult!=DialogResult.OK) {
				return;
			}
			apptPrintStartTime=FormAPS.ApptPrintStartTime;
			apptPrintStopTime=FormAPS.ApptPrintStopTime;
			apptPrintFontSize=FormAPS.ApptPrintFontSize;
			apptPrintColsPerPage=FormAPS.ApptPrintColsPerPage;
			_isPrintPreview=FormAPS.IsPrintPreview;
			pagesPrinted=0;
			pageRow=0;
			pageColumn=0;
			PrintReport();
			ApptDrawing.LineH=12;//Reset the LineH to default.
			CopyScheduleToClipboard();
			if(PatCur==null) {
				ModuleSelected(0);//Refresh the public variables in ApptDrawing.cs
			}
			else {
				ModuleSelected(PatCur.PatNum);//Refresh the public variables in ApptDrawing.cs
			}
		}

		///<summary></summary>
		public void PrintReport() {
			pd2=new PrintDocument();
			pd2.PrintPage += new PrintPageEventHandler(this.pd2_PrintPage);
			//pd2.DefaultPageSettings.Margins= new Margins(10,40,40,60);
			if(_isPrintPreview) {
				FormPrintPreview pView=new FormPrintPreview(PrintSituation.Appointments,pd2,0,"Daily appointment view for "+apptPrintStartTime.ToShortDateString()+" printed");
				pView.ShowDialog();
			}
			else {
#if DEBUG
				FormPrintPreview pView=new FormPrintPreview(PrintSituation.Appointments,pd2,0,"Daily appointment view for "+apptPrintStartTime.ToShortDateString()+" printed");
				pView.ShowDialog();
#else
				if(!PrinterL.SetPrinter(pd2,PrintSituation.Appointments,0,"Daily appointment view for "+apptPrintStartTime.ToShortDateString()+" printed")){
					return;
				}
				try{
					pd2.Print();
				}
				catch{
					MessageBox.Show(Lan.g(this,"Printer not available"));
				}
#endif
 			}
		}

		private void pd2_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			PrintApptSchedule(sender,e);
		}

		private void PrintApptSchedule(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			//Logic needs to be added here for calculating if printing will fit on the page. Then call drawing in a loop for number of required pages. 
			Rectangle pageBounds=e.PageBounds;
			int headerOffset=75;
			int footerOffset=40;
			int marginOffset=30;
			//The extra 15 is for the right side of that page.  For some reason the right margin needs a little extra room.
			ApptDrawing.ApptSheetWidth=pageBounds.Width-((marginOffset*2)+15);
			ApptDrawing.ComputeColWidth(apptPrintColsPerPage);
			ApptDrawing.SetLineHeight(apptPrintFontSize);//Measure size the user set to determine the line height for printout.
			int startHour=apptPrintStartTime.Hour;
			int stopHour=apptPrintStopTime.Hour;
			if(stopHour==0) {
				stopHour=24;
			}
			float totalHeight=ApptDrawing.LineH*ApptDrawing.RowsPerHr*(stopHour-startHour);
			//Figure out how many pages are needed to print.
			int pagesAcross=(int)Math.Ceiling((decimal)ApptDrawing.VisOps.Count/(decimal)apptPrintColsPerPage);
			int pagesTall=(int)Math.Ceiling((decimal)totalHeight/(decimal)(pageBounds.Height-(headerOffset+footerOffset)));
			int totalPages=pagesAcross*pagesTall;
			if(ApptDrawing.IsWeeklyView) {
				pagesAcross=1;
				totalPages=1*pagesTall;
			}
			//Decide what page currently on thus knowing what hours to print.
			#region HoursOnPage
			int hoursPerPage=(int)Math.Floor((decimal)(pageBounds.Height-(headerOffset+footerOffset))/(decimal)(ApptDrawing.LineH*ApptDrawing.RowsPerHr));
			int hourBegin=startHour;
			int hourEnd=hourBegin+hoursPerPage;
			if(pageRow>0) {
				hourBegin=startHour+(hoursPerPage*pageRow);
				hourEnd=hourBegin+hoursPerPage;
			}
			if(hourEnd>stopHour) {//Don't show too many hours.
				hourEnd=stopHour;
			}
			ApptDrawing.ApptSheetHeight=ApptDrawing.LineH*ApptDrawing.RowsPerHr*(hourEnd-hourBegin);
			if(hourEnd>23) {//Midnight must be 0.
				hourEnd=0;
			}
			DateTime beginTime=new DateTime(1,1,1,hourBegin,0,0);
			DateTime endTime=new DateTime(1,1,1,hourEnd,0,0);
			#endregion
			e.Graphics.TranslateTransform(marginOffset,headerOffset);//Compensate for header and margin.
			ApptDrawing.DrawAllButAppts(e.Graphics,false,beginTime,endTime,apptPrintColsPerPage,pageColumn,apptPrintFontSize,true);
			//Draw the appointments.
			#region ApptSingleDrawing
			//Clear out the ProvBar from previous page.
			ApptDrawing.ProvBar=new int[ApptDrawing.VisProvs.Count][];
			for(int i=0;i<ApptDrawing.VisProvs.Count;i++) {
				ApptDrawing.ProvBar[i]=new int[24*ApptDrawing.RowsPerHr]; //[144]; or 24*6
			}			
			foreach(ContrApptSingle ctrl in ContrApptSheet2.ListContrApptSingles) {
				if(!ApptDrawing.IsWeeklyView) {
					ApptDrawing.ProvBarShading(ctrl.DataRoww);//Always fill prov bars.
				}
				//Filter the list of appointments here for those those within the time frame.
				if(!ApptSingleDrawing.ApptWithinTimeFrame(ctrl.OpNum,ctrl.AptDateTime,ctrl.Pattern,beginTime,endTime,apptPrintColsPerPage,pageColumn)) {
					continue;
				}
				ContrApptSingle contrApptSingle=new ContrApptSingle(
					ctrl.DataRoww,
					ctrl.TableApptFields,
					ctrl.TablePatFields,
					ApptSingleDrawing.SetLocation(ctrl.DataRoww,hourBegin,apptPrintColsPerPage,pageColumn));
				e.Graphics.ResetTransform();
				e.Graphics.TranslateTransform(contrApptSingle.Location.X+marginOffset,contrApptSingle.Location.Y+headerOffset);
				ApptSingleDrawing.DrawEntireAppt(e.Graphics,contrApptSingle.DataRoww,contrApptSingle.PatternShowing,contrApptSingle.Size.Width,contrApptSingle.Size.Height,
					false,false,-1,ApptViewItemL.ApptRows,ApptViewItemL.ApptViewCur,contrApptSingle.TableApptFields,contrApptSingle.TablePatFields,apptPrintFontSize,true);
			}
			#endregion
			e.Graphics.ResetTransform();
			//Cover the portions of the appointments that don't belong on the page.
			e.Graphics.FillRectangle(new SolidBrush(Color.White),0,0,pageBounds.Width,headerOffset-1);
			e.Graphics.FillRectangle(new SolidBrush(Color.White),0,ApptDrawing.ApptSheetHeight+headerOffset,pageBounds.Width,totalHeight);
			//Draw the header
			DrawPrintingHeader(e.Graphics,totalPages,pageBounds.Width,pageBounds.Height);
			pagesPrinted++;
			pageColumn++;
			if(totalPages==pagesPrinted) {
				pagesPrinted=0;
				pageRow=0;
				pageColumn=0;
				e.HasMorePages=false;
			}
			else {
				e.HasMorePages=true;
				if(pagesPrinted==pagesAcross*(pageRow+1)) {
					pageRow++;
					pageColumn=0;
				}
			}
		}

		///<summary>Header and footer for printing.</summary>
		private void DrawPrintingHeader(Graphics g,int totalPages,float pageWidth,float pageHeight) {
			float xPos=0;//starting pos
			float yPos=25f;//starting pos
			//Print Title------------------------------------------------------------------------------
			string title;
			string date;
			if(ApptDrawing.IsWeeklyView) {
				title=Lan.g(this,"Weekly Appointments");
				date=WeekStartDate.DayOfWeek.ToString()+" "+WeekStartDate.ToShortDateString()
					+" - "+WeekEndDate.DayOfWeek.ToString()+" "+WeekEndDate.ToShortDateString();
			}
			else {
				title=Lan.g(this,"Daily Appointments");
				date=AppointmentL.DateSelected.DayOfWeek.ToString()+"   "+AppointmentL.DateSelected.ToShortDateString();
			}
			Font titleFont=new Font("Arial",12,FontStyle.Bold);
			float xTitle = (float)((pageWidth/2)-((g.MeasureString(title,titleFont).Width/2)));
			g.DrawString(title,titleFont,Brushes.Black,xTitle,yPos);//centered
			//Print Date--------------------------------------------------------------------------------
			Font dateFont=new Font("Arial",8,FontStyle.Regular);
			float xDate = (float)((pageWidth/2)-((g.MeasureString(date,dateFont).Width/2)));
			yPos+=20;
			g.DrawString(date,dateFont,Brushes.Black,xDate,yPos);//centered
			//Col titles-----------------------------------------------------------------------------
			if(!ApptDrawing.IsWeeklyView) {
				string[] headers = new string[apptPrintColsPerPage];
				Font headerFont=new Font("Arial",8);
				yPos+=15;
				xPos+=(int)(ApptDrawing.TimeWidth+(ApptDrawing.ProvWidth*ApptDrawing.ProvCount)+30);//30 for margins.
				int xCenter=0;
				for(int i=0;i<apptPrintColsPerPage;i++) {
					if(i==ApptDrawing.VisOps.Count) {
						break;
					}
					int k=apptPrintColsPerPage*pageColumn+i;
					if(k>=ApptDrawing.VisOps.Count) {
						break;
					}
					headers[i]=ApptDrawing.VisOps[k].OpName;
					if(g.MeasureString(headers[i],headerFont).Width>ApptDrawing.ColWidth) {
						RectangleF rf=new RectangleF(xPos,yPos,ApptDrawing.ColWidth,g.MeasureString(headers[i],headerFont).Height);
						g.DrawString(headers[i],headerFont,Brushes.Black,rf);
					}
					else {
						xCenter=(int)((ApptDrawing.ColWidth/2)-(g.MeasureString(headers[i],headerFont).Width/2));
						g.DrawString(headers[i],headerFont,Brushes.Black,(int)(xPos+xCenter),yPos);
					}
					xPos+=ApptDrawing.ColWidth;
				}
			}
			else {
				string columnDate="";
				Font headerFont=new Font("Arial",8);
				yPos+=15;
				xPos+=(int)(ApptDrawing.TimeWidth)+30;//30 for margins.
				int xCenter=0;
				int day=WeekStartDate.Day;
				int daysInMonth=DateTime.DaysInMonth(WeekStartDate.Year,WeekStartDate.Month);
				for(int i=0;i<7;i++) {
					switch(i) {
						case 0:
							columnDate="Monday-"+day;
							break;
						case 1:
							columnDate="Tuesday-"+day;
							break;
						case 2:
							columnDate="Wednesday-"+day;
							break;
						case 3:
							columnDate="Thursday-"+day;
							break;
						case 4:
							columnDate="Friday-"+day;
							break;
						case 5:
							columnDate="Saturday-"+day;
							break;
						case 6:
							columnDate="Sunday-"+day;
							break;
					}
					day++;
					if(day>daysInMonth) {
						day=1;//Week contains days in the next month.
					}
					xCenter=(int)((ApptDrawing.ColDayWidth/2)-(g.MeasureString(columnDate,headerFont).Width/2));
					g.DrawString(columnDate,headerFont,Brushes.Black,(int)(xPos+xCenter),yPos);
					xPos+=ApptDrawing.ColDayWidth;
				}
			}
			//Print Footer-----------------------------------------------------------------------------
			string page=(pagesPrinted+1)+" / "+totalPages;
			float xPage = (float)(400-((g.MeasureString(page,dateFont).Width/2)));
			yPos=pageHeight-40;
			g.DrawString(page,dateFont,Brushes.Black,xPage,yPos);
		}

		///<summary>Sends an image of the current appointment schedule to the clipboard.  Some users 'paste' to their own editor for more control.</summary>
		private void CopyScheduleToClipboard() {
			ArrayList aListStart=new ArrayList();
			ArrayList aListStop=new ArrayList();
			DateTime startTime;
			DateTime stopTime;
			for(int i=0;i<SchedListPeriod.Count;i++) {
				if(SchedListPeriod[i].SchedType!=ScheduleType.Provider) {
					continue;
				}
				if(SchedListPeriod[i].StartTime==TimeSpan.FromHours(0)) {//ignore notes at midnight
					continue;
				}
				aListStart.Add(SchedListPeriod[i].SchedDate+SchedListPeriod[i].StartTime);
				aListStop.Add(SchedListPeriod[i].SchedDate+SchedListPeriod[i].StopTime);
			}
			if(aListStart.Count > 0) {//makes sure there is at least one timeblock
				startTime=(DateTime)aListStart[0];
				for(int i=0;i<aListStart.Count;i++) {
					//if (A) OR (B AND C)
					if((((DateTime)(aListStart[i])).Hour < startTime.Hour) 
						|| (((DateTime)(aListStart[i])).Hour==startTime.Hour 
						&& ((DateTime)(aListStart[i])).Minute < startTime.Minute)) {
						startTime=(DateTime)aListStart[i];
					}
				}
				stopTime=(DateTime)aListStop[0];
				for(int i=0;i<aListStop.Count;i++) {
					//if (A) OR (B AND C)
					if((((DateTime)(aListStop[i])).Hour > stopTime.Hour) 
						|| (((DateTime)(aListStop[i])).Hour==stopTime.Hour 
						&& ((DateTime)(aListStop[i])).Minute > stopTime.Minute)) {
						stopTime=(DateTime)aListStop[i];
					}
				}
			}
			else {//office is closed
				startTime=new DateTime(AppointmentL.DateSelected.Year,AppointmentL.DateSelected.Month
					,AppointmentL.DateSelected.Day
					,ApptDrawing.ConvertToHour(-ContrApptSheet2.Location.Y)
					,ApptDrawing.ConvertToMin(-ContrApptSheet2.Location.Y)
					,0);
				if(ApptDrawing.ConvertToHour(-ContrApptSheet2.Location.Y)+12<23) {
					//we will be adding an extra hour later
					stopTime=new DateTime(AppointmentL.DateSelected.Year,AppointmentL.DateSelected.Month
						,AppointmentL.DateSelected.Day
						,ApptDrawing.ConvertToHour(-ContrApptSheet2.Location.Y)+12//add 12 hours
						,ApptDrawing.ConvertToMin(-ContrApptSheet2.Location.Y)
						,0);
				}
				else {
					stopTime=new DateTime(AppointmentL.DateSelected.Year,AppointmentL.DateSelected.Month
						,AppointmentL.DateSelected.Day
						,22
						,ApptDrawing.ConvertToMin(-ContrApptSheet2.Location.Y)
						,0);
				}
			}
			try {
				Bitmap imageTemp=ContrApptSheet2.GetShadowClone(startTime,stopTime);
				if(imageTemp!=null) {
					Clipboard.SetDataObject(imageTemp);
				}
			}
			catch { //Nothing to do here.								
			}			
		}

		///<summary>Clears the pinboard.</summary>
		private void butClearPin_Click(object sender,System.EventArgs e) {
			if(pinBoard.ApptList.Count==0) {
				MsgBox.Show(this,"There are no appointments on the pinboard to clear.");
				return;
			}
			if(pinBoard.SelectedIndex==-1) {
				MsgBox.Show(this,"Please select an appointment first.");
				return;
			}
			ContrApptSingle row=pinBoard.ApptList[pinBoard.SelectedIndex];
			pinBoard.ClearSelected();
			ContrApptSingle.SelectedAptNum=-1;
			if(row.AptStatus==ApptStatus.UnschedList) {//unscheduled status
				if(row.AptDateTime.Year<1880) {//Indicates that this was a brand new appt
					Appointment aptCur=Appointments.GetOneApt(row.AptNum);
					if(aptCur.AptDateTime.Year>1880){//if date is now present
						//don't do anything to db.  Appt removed from pinboard above, and Refresh will happen below.
					}
					else{
						Appointments.Delete(row.AptNum,true);
					}
				}
				else {//was actually on the unscheduled list
					//do nothing to database
				}
			}
			else if(row.AptDateTime.Year>1880) {//already scheduled
				//do nothing to database
			}
			else if(row.AptStatus==ApptStatus.Planned) {
				//do nothing except remove it from pinboard
			}
			else {//Not sure when this would apply, since new appts start out as unsched.  Maybe patient notes?  Leave it just in case.
				//this gets rid of new appointments that never made it off the pinboard
				Appointments.Delete(row.AptNum,true);
			}
			if(pinBoard.SelectedIndex==-1) {
				if(PatCur==null) {
					//Do nothing
				}
				else {
					ModuleSelected(PatCur.PatNum);
				}
			}
			else {
				RefreshModuleDataPatient(pinBoard.ApptList[pinBoard.SelectedIndex].PatNum);
				FormOpenDental.S_Contr_PatientSelected(PatCur,true,false);
			}
		}

		///<summary>The scrollbar has been moved by the user.</summary>
		private void vScrollBar1_Scroll(object sender,System.Windows.Forms.ScrollEventArgs e) {
			if(e.Type==ScrollEventType.ThumbTrack) {//moving
				ContrApptSheet2.IsScrolling=true;
				ContrApptSheet2.Location=new Point(0,-e.NewValue);
			}
			if(e.Type==ScrollEventType.EndScroll) {//done moving
				ContrApptSheet2.IsScrolling=true;
				ContrApptSheet2.Location=new Point(0,-e.NewValue);
				ContrApptSheet2.IsScrolling=false;
				ContrApptSheet2.Select();
			}
		}

		///<summary>Occurs whenever the panel holding the appt sheet is resized.</summary>
		private void panelSheet_Resize(object sender,System.EventArgs e) {
			vScrollBar1.Maximum=ContrApptSheet2.Height-panelSheet.Height+vScrollBar1.LargeChange;
		}

		private void menuWeekly_Click(object sender,System.EventArgs e) {
			switch(((MenuItem)sender).Index) {
				case 0:
					OnCopyToPin_Click();
					break;
			}
		}

		private void menuApt_Click(object sender,System.EventArgs e) {
			switch(((MenuItem)sender).Index) {
				case 0: //Menu: Copy to Pinboard
					OnCopyToPin_Click();
					break;
				//1: divider
				case 2: //Menu: Send to Unscheduled List
					OnUnsched_Click();
					break;
				case 3: //Menu: Break Appointment
					OnBreak_Click();
					break;
				case 5: // Menu: Set Complete
					OnComplete_Click();
					break;
				case 6: // Menu: Delete
					OnDelete_Click();
					break;
				case 7: // Menu: Other Appointments
					DisplayOtherDlg(false);
					break;
				//8: divider
				case 9: // Menu: Print Label
					PrintApptLabel();
					break;
				case 10: // Menu: Print Card
					cardPrintFamily=false;
					PrintApptCard();
					break;
				case 11: // Menu: Print Card for Entire Family
					cardPrintFamily=true;
					PrintApptCard();
					break;
				case 12: // Menu: Routing Slip
					//for now, this only allows one type of routing slip.  But it could be easily changed.
					Appointment apt=Appointments.GetOneApt(ContrApptSingle.ClickedAptNum);
					if(ApptIsNull(apt)) { return; }
					FormRpRouting FormR=new FormRpRouting();
					FormR.AptNum=ContrApptSingle.ClickedAptNum;
					List<SheetDef> customSheetDefs=SheetDefs.GetCustomForType(SheetTypeEnum.RoutingSlip);
					if(customSheetDefs.Count==0) {
						FormR.SheetDefNum=0;
					}
					else {
						FormR.SheetDefNum=customSheetDefs[0].SheetDefNum;
					}
					FormR.ShowDialog();
					break;
			}
			//cannot use menu item index because some menu items may not exist
			switch(((MenuItem)sender).Name) {
				case "Ortho Chart": //Open Patient Ortho Chart
					FormOrthoChart FormOC=new FormOrthoChart(PatCur);
					FormOC.ShowDialog();
					break;
				case "Home Phone": //Call Home Phone
					if(Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
						DentalTek.PlaceCall(PatCur.HmPhone);
					}
					else {
						AutomaticCallDialingDisabledMessage();
					}
					break;
				case "Work Phone": //Call Work Phone
					if(Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
						DentalTek.PlaceCall(PatCur.WkPhone);
					}
					else {
						AutomaticCallDialingDisabledMessage();
					}
					break;
				case "Wireless Phone": //Call Wireless Phone
					if(Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
						DentalTek.PlaceCall(PatCur.WirelessPhone);
					}
					else {
						AutomaticCallDialingDisabledMessage();
					}
					break;
				case "Send Text":
					Appointment appt=Appointments.GetOneApt(ContrApptSingle.ClickedAptNum);
					if(ApptIsNull(appt)) { return; }
					FormOpenDental.S_OnTxtMsg_Click(appt.PatNum,"");
					break;
				case "Send Confirmation Text":
					appt=Appointments.GetOneApt(ContrApptSingle.ClickedAptNum);
					if(ApptIsNull(appt)) { return; }
					Patient pat=Patients.GetPat(appt.PatNum);
					string message=PrefC.GetString(PrefName.ConfirmTextMessage);
					message=message.Replace("[NameF]",pat.GetNameFirst());
					message=message.Replace("[NameFL]",pat.GetNameFL());
					message=message.Replace("[date]",appt.AptDateTime.ToShortDateString());
					message=message.Replace("[time]",appt.AptDateTime.ToShortTimeString());
					bool wasTextSent=FormOpenDental.S_OnTxtMsg_Click(pat.PatNum,message);
					if(wasTextSent) {
						Appointments.SetConfirmed(appt,PrefC.GetLong(PrefName.ConfirmStatusTextMessaged));
					}
					break;
			}
		}

		private void menuApt_Popup(object sender,EventArgs e) {
			if(menuItemBreakAppt==null) {
				return;
			}
			menuItemBreakAppt.MenuItems.Clear();
			menuItemBreakAppt.Tag=ContrApptSingle.SelectedAptNum;//Refresh later, just in case.
			MenuItem item=null;
			BrokenApptProcedure brokenApptProcs=(BrokenApptProcedure)PrefC.GetInt(PrefName.BrokenApptProcedure);
			if(brokenApptProcs.In(BrokenApptProcedure.Missed,BrokenApptProcedure.Both)) {
				item=menuItemBreakAppt.MenuItems.Add(Lans.g(this,"Missed - Send to Unscheduled List"));
				item.Click+=new EventHandler(BreakToUnschedClick);
				item.Tag=ProcedureCodes.GetProcCode("D9986");
				item=menuItemBreakAppt.MenuItems.Add(Lans.g(this,"Missed - Copy To Pinboard"));
				item.Click+=new EventHandler(BreakToPinClick);
				item.Tag=ProcedureCodes.GetProcCode("D9986");
				item=menuItemBreakAppt.MenuItems.Add(Lans.g(this,"Missed - Leave on Appt Book"));
				item.Click+=new EventHandler(BreakApptClick);
				item.Tag=ProcedureCodes.GetProcCode("D9986");
			}
			if(brokenApptProcs.In(BrokenApptProcedure.Cancelled,BrokenApptProcedure.Both)) {				
				if(menuItemBreakAppt.MenuItems.Count > 0) {
					menuItemBreakAppt.MenuItems.Add("-");//Horizontal bar.
				}				
				item=menuItemBreakAppt.MenuItems.Add(Lans.g(this,"Cancelled - Send to Unscheduled List"));
				item.Click+=new EventHandler(BreakToUnschedClick);
				item.Tag=ProcedureCodes.GetProcCode("D9987");
				item=menuItemBreakAppt.MenuItems.Add(Lans.g(this,"Cancelled - Copy To Pinboard"));
				item.Click+=new EventHandler(BreakToPinClick);
				item.Tag=ProcedureCodes.GetProcCode("D9987");
				item=menuItemBreakAppt.MenuItems.Add(Lans.g(this,"Cancelled - Leave on Appt Book"));
				item.Click+=new EventHandler(BreakApptClick);
				item.Tag=ProcedureCodes.GetProcCode("D9987");
			}
		}

		private Appointment BreakApptHelper(object sender,bool suppressModuleSelected=false) {
			MenuItem item=(MenuItem)sender;
			MenuItem parentMenuItem=(MenuItem)item.Parent;
			Appointment appt=Appointments.GetOneApt((long)parentMenuItem.Tag);//Refresh since they could of waited to interact with menu.
			if(appt==null) {//This can happen if another user deleted the appt just after the current user right clicked on the appt.
				MsgBox.Show("Appointment","Appointment not found.");
				return null;
			}
			ProcedureCode procCode=(ProcedureCode)item.Tag;
			Patient pat=Patients.GetPat(appt.PatNum);
			AppointmentL.BreakApptHelper(appt,pat,procCode);
			if(!suppressModuleSelected) {
				ModuleSelected(pat.PatNum);
			}
			return appt;
		}

		private void BreakToUnschedClick(object sender,EventArgs e) {
			Appointment appt=BreakApptHelper(sender,true);
			if(appt!=null && AppointmentL.ValidateApptUnsched(appt)) {
				PromptTextASAPList(appt);
				AppointmentL.SetApptUnschedHelper(appt);
				ModuleSelected(appt.PatNum);
			}
		}

		private void BreakToPinClick(object sender,EventArgs e) {
			Appointment appt=BreakApptHelper(sender);
			if(appt!=null && AppointmentL.ValidateApptToPinboard(appt)) {
				PromptTextASAPList(appt);
				AppointmentL.CopyAptToPinboardHelper(appt);
			}
		}

		private void BreakApptClick(object sender,EventArgs e) {
			BreakApptHelper(sender);
		}

		private void PromptTextASAPList(Appointment appt) {
			if(!PrefC.GetBool(PrefName.WebSchedAsapEnabled) || Appointments.RefreshASAP(0,0,appt.ClinicNum, new List<ApptStatus>()).Count==0
				|| !MsgBox.Show("Appointment",MsgBoxButtons.YesNo,"Text patients on the ASAP List and offer them this opening?")) 
			{
				return;
			}
			DisplayFormAsapForWebSched(appt.Op,appt.AptDateTime,appt.AptNum);
		}

		///<summary>Brings up FormASAP ready to send for an open time slot.</summary>
		private void DisplayFormAsapForWebSched(long opNum,DateTime dateTimeChosen,long aptNum=0) {
			DateTime slotStart=AppointmentL.DateSelected.Date;//Midnight
			DateTime slotEnd=AppointmentL.DateSelected.Date.AddDays(1);//Midnight tomorrow
			//Loop through all other appts in the op to find a slot that will not overlap.
			foreach(ContrApptSingle ctrl in ContrApptSheet2.ListContrApptSingles.Where(x => x.OpNum==opNum && x.AptNum!=aptNum)) {
				if(ApptDrawing.IsWeeklyView && ctrl.AptDateTime.Date!=AppointmentL.DateSelected.Date) {
					continue;
				}
				DateTime dateEndApt=ctrl.AptDateTime.AddMinutes(ctrl.Pattern.Length*5);
				if(dateEndApt.Between(slotStart,dateTimeChosen)) {
					slotStart=dateEndApt;
				}
				if(ctrl.AptDateTime.Between(dateTimeChosen,slotEnd)) {
					slotEnd=ctrl.AptDateTime;
				}
			}
			slotStart=ODMathLib.Max(slotStart,dateTimeChosen.AddHours(-1));
			slotEnd=ODMathLib.Min(slotEnd,dateTimeChosen.AddHours(3));
			if(_formASAP==null || _formASAP.IsDisposed) {
				_formASAP=new FormASAP();
			}
			_formASAP.ShowFormForWebSched(dateTimeChosen,slotStart,slotEnd,opNum);
			_formASAP.FormClosed+=FormASAP_FormClosed;
		}

		private void AutomaticCallDialingDisabledMessage() {
			if(ProgramProperties.IsAdvertisingDisabled(ProgramName.DentalTekSmartOfficePhone)) {
				return;
			}
			MessageBox.Show(Lan.g(this,"Automatic dialing of patient phone numbers requires an additional service")+".\r\n"
							+Lan.g(this,"Contact Open Dental for more information")+".");
			try {
				Process.Start("http://www.opendental.com/manual/dentaltekinfo.html");
			}
			catch(Exception) {
				MessageBox.Show(Lan.g(this,"Could not find")+" http://www.opendental.com/contact.html" + "\r\n"
							+Lan.g(this,"Please set up a default web browser."));
			}
		}

		///<summary>Sends current appointment to unscheduled list.</summary>
		private void butUnsched_Click(object sender,System.EventArgs e) {
			OnUnsched_Click();
		}

		private void butBreak_Click(object sender,System.EventArgs e) {
			OnBreak_Click();
		}

		private void butComplete_Click(object sender,System.EventArgs e) {
			OnComplete_Click();
		}

		private void butDelete_Click(object sender,System.EventArgs e) {
			OnDelete_Click();
		}

		private void butMakeAppt_Click(object sender,System.EventArgs e) {
			if(PatCur==null) {
				return;
			}
			if(!Security.IsAuthorized(Permissions.AppointmentCreate)) {
				return;
			}
			if(PatRestrictionL.IsRestricted(PatCur.PatNum,PatRestrict.ApptSchedule)) {
				return;
			}
			if(AppointmentL.PromptForMerge(PatCur,out PatCur)) {
				RefreshModuleDataPatient(PatCur.PatNum);
				FormOpenDental.S_Contr_PatientSelected(PatCur,true,false);
			}
			if(PatCur!=null && PatCur.PatStatus.In(PatientStatus.Archived,PatientStatus.Deceased)) {
				MsgBox.Show(this,"Appointments cannot be scheduled for "+PatCur.PatStatus.ToString().ToLower()+" patients.");
				return;
			}
			if(Appointments.HasOutstandingAppts(PatCur.PatNum)) {
				DisplayOtherDlg(false);
				return;
			}
			FormApptsOther FormAO=new FormApptsOther(PatCur.PatNum);//doesn't actually get shown
			CheckStatus();
			FormAO.InitialClick=false;
			FormAO.MakeAppointment();
			SendToPinboard(FormAO.AptNumsSelected);
		}

		private void butMakeRecall_Click(object sender,EventArgs e) {
			if(PatCur==null) {
				return;
			}
			if(!Security.IsAuthorized(Permissions.AppointmentCreate)) {
				return;
			}
			if(PatRestrictionL.IsRestricted(PatCur.PatNum,PatRestrict.ApptSchedule)) {
				return;
			}
			if(AppointmentL.PromptForMerge(PatCur,out PatCur)) {
				RefreshModuleDataPatient(PatCur.PatNum);
				FormOpenDental.S_Contr_PatientSelected(PatCur,true,false);
			}
			if(PatCur!=null && PatCur.PatStatus.In(PatientStatus.Archived,PatientStatus.Deceased)) {
				MsgBox.Show(this,"Appointments cannot be scheduled for "+PatCur.PatStatus.ToString().ToLower()+" patients.");
				return;
			}
			if(Appointments.HasOutstandingAppts(PatCur.PatNum,true)) {
				DisplayOtherDlg(false);
				return;
			}
			FormApptsOther FormAO=new FormApptsOther(PatCur.PatNum);//doesn't actually get shown
			FormAO.InitialClick=false;
			FormAO.MakeRecallAppointment();
			if(FormAO.DialogResult!=DialogResult.OK) {
				return;
			}
			SendToPinboard(FormAO.AptNumsSelected);
			if(ApptDrawing.IsWeeklyView) {
				return;
			}
			dateSearch.Text=FormAO.DateJumpToString;
			if(!groupSearch.Visible) {//if search not already visible
				ShowSearch();
			}
			DoSearch();
		}

		private void butFamRecall_Click(object sender,EventArgs e) {
			if(PatCur==null) {
				return;
			}
			if(!Security.IsAuthorized(Permissions.AppointmentCreate)) {
				return;
			}
			if(Appointments.HasOutstandingAppts(PatCur.PatNum)) {
				DisplayOtherDlg(false);
				return;
			}
			FormApptsOther FormAO=new FormApptsOther(PatCur.PatNum);//doesn't actually get shown
			FormAO.InitialClick=false;
			FormAO.MakeRecallFamily();
			if(FormAO.DialogResult!=DialogResult.OK) {
				return;
			}
			SendToPinboard(FormAO.AptNumsSelected);
			if(ApptDrawing.IsWeeklyView) {
				return;
			}
			dateSearch.Text=FormAO.DateJumpToString;
			if(!groupSearch.Visible) {//if search not already visible
				ShowSearch();
			}
			DoSearch();
		}

		private void butViewAppts_Click(object sender,EventArgs e) {
			DisplayOtherDlg(false);
		}

		private void OnUnsched_Click() {
			if(ContrApptSingle.SelectedAptNum==-1) {
				MsgBox.Show(this,"Please select an appointment first.");
				return;
			}
			Appointment apt = Appointments.GetOneApt(ContrApptSingle.SelectedAptNum);
			if(ApptIsNull(apt)) { return; }
			if(!AppointmentL.ValidateApptUnsched(apt)) {
				return;
			}
			if(MessageBox.Show(Lan.g(this,"Send Appointment to Unscheduled List?")
				,"",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
				return;
			}
			int thisI=GetIndex(ContrApptSingle.SelectedAptNum);
			if(thisI==-1) {//selected appt is on a different day
				MsgBox.Show(this,"Please select an appointment first.");
				return;
			}			
			Patient pat=Patients.GetPat(ContrApptSheet2[thisI].PatNum);
			AppointmentL.SetApptUnschedHelper(apt,pat);
			ModuleSelected(pat.PatNum);
			Plugins.HookAddCode(this,"ContrAppt.OnUnsched_Click_end",apt,PatCur);
		}

		private void OnBreak_Click() {
			if(PrefC.GetBool(PrefName.BrokenApptAdjustment) 
				&& PrefC.GetLong(PrefName.BrokenAppointmentAdjustmentType)==0) 
			{
				//They want broken appointment adjustments but don't have it set up.
				MsgBox.Show(this,"Broken appointment adjustment type is not setup yet.  Please go to Setup | Appointment | Appts Preferences to fix this.");
				return;
			}
			int thisI=GetIndex(ContrApptSingle.SelectedAptNum);
			if(thisI==-1) {//selected appt is on a different day
				MsgBox.Show(this,"Please select an appointment first.");
				return;
			}
			ContrApptSingle apptSingle=ContrApptSheet2[thisI];
			Appointment apt=Appointments.GetOneApt(ContrApptSingle.SelectedAptNum);
			if(ApptIsNull(apt)) { return; }
			Patient pat=Patients.GetPat(apptSingle.PatNum);
			if((apt.AptStatus!=ApptStatus.Complete && !Security.IsAuthorized(Permissions.AppointmentEdit)) //seperate permissions for completed appts.
				|| (apt.AptStatus==ApptStatus.Complete && !Security.IsAuthorized(Permissions.AppointmentCompleteEdit))) 
			{
				return;
			}
			if(apt.AptStatus == ApptStatus.PtNote || apt.AptStatus == ApptStatus.PtNoteCompleted) {
				MsgBox.Show(this,"Only appointments may be broken, not notes.");
				return;
			}
			ProcedureCode procCodeBroke=null;//Will not chart if it stays null.
			ApptBreakSelection postBreakSelection=ApptBreakSelection.None;
			bool hasBrokenProcs=AppointmentL.HasBrokenApptProcs();//When true, we show FormApptBreak.cs
			if(hasBrokenProcs) {//If true, user can not get here from right click 'Break Appointment' directly.
				FormApptBreak formAB=new FormApptBreak(apt);
				if(formAB.ShowDialog()!=DialogResult.OK) {
					return;
				}
				procCodeBroke=formAB.SelectedProcCode;
				postBreakSelection=formAB.FormApptBreakSelection;
			}
			else if(!MsgBox.Show(this,true,"Break appointment?")) {
				return;
			}
			//This hook is specifically called after we know a valid appointment has been identified.
			Plugins.HookAddCode(this, "ContrAppt.OnBreak_Click_validation_end",apt);
			AppointmentL.BreakApptHelper(apt,pat,procCodeBroke);
			if(hasBrokenProcs) {//FormApptBreak was shown and user made a selection
				switch(postBreakSelection) {
					case ApptBreakSelection.Unsched:
						if(AppointmentL.ValidateApptUnsched(apt)) {
							AppointmentL.SetApptUnschedHelper(apt,pat);
						}
						break;
					case ApptBreakSelection.Pinboard:
						if(AppointmentL.ValidateApptToPinboard(apt)) {
							AppointmentL.CopyAptToPinboardHelper(apt);
						}
						break;
					case ApptBreakSelection.ApptBook:
						//Intentionally blank.
						break;
				}
			}
			ModuleSelected(pat.PatNum);//Must be ran after the "D9986" break logic due to the addition of a completed procedure.
			Plugins.HookAddCode(this,"ContrAppt.OnBreak_Click_end",apt,PatCur);
		}

		private void OnASAP_Click(object sender, EventArgs e) {
			OnASAP_Click();
		}
		private void OnASAP_Click() {
			if(!Security.IsAuthorized(Permissions.AppointmentEdit)) {
				return;
			}
			Appointment apt=Appointments.GetOneApt(ContrApptSingle.SelectedAptNum);
			if(ApptIsNull(apt)) { return; }
			PopupFade.Show(this,"Appointment status has been set to ASAP.");
			if(apt.Priority==ApptPriority.ASAP) {
				return;
			}
			Appointments.SetPriority(apt,ApptPriority.ASAP);
			Plugins.HookAddCode(this,"ContrAppt.OnASAP_Click_end",apt,PatCur);
		}

		private void OnComplete_Click() {
			if(!Security.IsAuthorized(Permissions.AppointmentEdit)) {
				return;
			}
			int thisI=GetIndex(ContrApptSingle.SelectedAptNum);
			if(thisI==-1) {//selected appt is on a different day
				MsgBox.Show(this,"Please select an appointment first.");
				return;
			}
			Appointment apt = Appointments.GetOneApt(ContrApptSingle.SelectedAptNum);
			if(ApptIsNull(apt)) { return; }
			if(apt.AptDateTime.Date>DateTime.Today) {
				if(!PrefC.GetBool(PrefName.ApptAllowFutureComplete)){
					MsgBox.Show(this,"Not allowed to set future appointments complete.");
					return;
				}
			}
			List<Procedure> listProcs=Procedures.GetProcsForSingle(apt.AptNum,false);
			if(apt.AptStatus!=ApptStatus.PtNote && apt.AptStatus!=ApptStatus.PtNoteCompleted  //Ptnote cannot have procs attached
				&& !PrefC.GetBool(PrefName.ApptAllowEmptyComplete)//Appointments must have at least 1 proc
				&& listProcs.Count==0)
			{
				MsgBox.Show(this,"Appointments without procedures attached can not be set complete.");
				return;
			}
			if(apt.AptStatus == ApptStatus.PtNoteCompleted) {
				return;
			}
			if(ProcedureCodes.DoAnyBypassLockDate()) {
				foreach(Procedure proc in listProcs) {
					if(!Security.IsAuthorized(Permissions.ProcComplCreate,apt.AptDateTime,proc.CodeNum,proc.ProcFee)) {
						return;
					}
				}
			}
			else if(!Security.IsAuthorized(Permissions.ProcComplCreate,apt.AptDateTime)) {
				return;
			}
			else if(listProcs.Count > 0 && apt.AptDateTime.Date > DateTime.Today.Date && !PrefC.GetBool(PrefName.FutureTransDatesAllowed)) {
				MsgBox.Show(this,"Not allowed to set procedures complete with future dates.");
				return;
			}
			bool removeCompletedProcs=ProcedureL.DoRemoveCompletedProcs(apt,listProcs.FindAll(x => x.ProcStatus==ProcStat.C));
			ODTuple<Appointment,List<Procedure>> result=Appointments.CompleteClick(apt,listProcs,removeCompletedProcs);
			apt=result.Item1;
			listProcs=result.Item2;
			if(apt.AptStatus!=ApptStatus.PtNote) {
				AutomationL.Trigger(AutomationTrigger.CompleteProcedure,listProcs.Select(x => ProcedureCodes.GetStringProcCode(x.CodeNum)).ToList(),apt.PatNum);
			}
			ModuleSelected(apt.PatNum);
			Plugins.HookAddCode(this,"ContrAppt.OnComplete_Click_end",apt,PatCur);
		}

		private void OnDelete_Click() {
			long selectedAptNum=ContrApptSingle.SelectedAptNum;
			Appointment apt = Appointments.GetOneApt(selectedAptNum);
			if(ApptIsNull(apt)) { return; }
			if((apt.AptStatus!=ApptStatus.Complete && !Security.IsAuthorized(Permissions.AppointmentEdit)) //seperate permission for completed appts.
				|| (apt.AptStatus==ApptStatus.Complete && !Security.IsAuthorized(Permissions.AppointmentCompleteEdit))) 
			{
				return;
			}
			int thisI=GetIndex(selectedAptNum);
			if(thisI==-1) {//selected appt is on a different day
				MsgBox.Show(this,"Please select an appointment first.");
				return;
			}
			if(apt.AptStatus == ApptStatus.PtNote | apt.AptStatus == ApptStatus.PtNoteCompleted) {
				if(!MsgBox.Show(this,true,"Delete Patient Note?")) {
					return;
				}
				if(apt.Note != "") {
					if(MessageBox.Show(Commlogs.GetDeleteApptCommlogMessage(apt.Note,apt.AptStatus),"Question...",MessageBoxButtons.YesNo) == DialogResult.Yes) {
						Commlog CommlogCur = new Commlog();
						CommlogCur.PatNum = apt.PatNum;
						CommlogCur.CommDateTime = DateTime.Now;
						CommlogCur.CommType =Commlogs.GetTypeAuto(CommItemTypeAuto.APPT);
						CommlogCur.Note = "Deleted Patient NOTE from schedule, saved copy: ";
						CommlogCur.Note += apt.Note;
						CommlogCur.UserNum=Security.CurUser.UserNum;
						//there is no dialog here because it is just a simple entry
						Commlogs.Insert(CommlogCur);
					}
				}
				SecurityLogs.MakeLogEntry(Permissions.AppointmentEdit,PatCur.PatNum,
					ContrApptSheet2[thisI].Procs+", "+ContrApptSheet2[thisI].AptDateTime.ToString()+", "+"NOTE Deleted",
					ContrApptSheet2[thisI].AptNum,apt.DateTStamp);
			}
			else {
				if(!MsgBox.Show(this,true,"Delete Appointment?")) {
					return;
				}
				if(apt.Note != "") {
					if(MessageBox.Show(Commlogs.GetDeleteApptCommlogMessage(apt.Note,apt.AptStatus),"Question...",MessageBoxButtons.YesNo) == DialogResult.Yes) {
						Commlog CommlogCur = new Commlog();
						CommlogCur.PatNum = apt.PatNum;
						CommlogCur.CommDateTime = DateTime.Now;
						CommlogCur.CommType =Commlogs.GetTypeAuto(CommItemTypeAuto.APPT);
						CommlogCur.Note = "Deleted Appointment & saved note: ";
						if(apt.ProcDescript != "") {
							CommlogCur.Note += apt.ProcDescript + ": ";
						}
						CommlogCur.Note += apt.Note;
						CommlogCur.UserNum=Security.CurUser.UserNum;
						//there is no dialog here because it is just a simple entry
						Commlogs.Insert(CommlogCur);
					}
				}
				if(apt.AptStatus!=ApptStatus.Complete) {// seperate log entry for editing completed appointments.
					SecurityLogs.MakeLogEntry(Permissions.AppointmentEdit,PatCur.PatNum,
						ContrApptSheet2[thisI].Procs+", "+ContrApptSheet2[thisI].AptDateTime.ToString()+", "+"Deleted",
						ContrApptSheet2[thisI].AptNum,apt.DateTStamp);
				}
				else {
					SecurityLogs.MakeLogEntry(Permissions.AppointmentCompleteEdit,PatCur.PatNum,
						ContrApptSheet2[thisI].Procs+", "+ContrApptSheet2[thisI].AptDateTime.ToString()+", "+"Deleted",
						ContrApptSheet2[thisI].AptNum,apt.DateTStamp);
				}
				//If there is an existing HL7 def enabled, send a SIU message if there is an outbound SIU message defined
				if(HL7Defs.IsExistingHL7Enabled()) {
					//S17 - Appt Deletion event
					MessageHL7 messageHL7=MessageConstructor.GenerateSIU(PatCur,Patients.GetPat(PatCur.Guarantor),EventTypeHL7.S17,apt);
					//Will be null if there is no outbound SIU message defined, so do nothing
					if(messageHL7!=null) {
						HL7Msg hl7Msg=new HL7Msg();
						hl7Msg.AptNum=apt.AptNum;
						hl7Msg.HL7Status=HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
						hl7Msg.MsgText=messageHL7.ToString();
						hl7Msg.PatNum=PatCur.PatNum;
						HL7Msgs.Insert(hl7Msg);
#if DEBUG
						MessageBox.Show(this,messageHL7.ToString());
#endif
					}
				}
			}
			Appointments.Delete(selectedAptNum,true);//Appointments S-Class handles Signalods
			ContrApptSingle.SelectedAptNum=-1;
			pinBoard.SelectedIndex=-1;
			for(int i=0;i<pinBoard.ApptList.Count;i++) {
				if(selectedAptNum==pinBoard.ApptList[i].AptNum) {
					pinBoard.SelectedIndex=i;
					pinBoard.ClearSelected();
					pinBoard.SelectedIndex=-1;
				}
			}
			if(PatCur==null) {
				ModuleSelected(0);
			}
			else {
				ModuleSelected(PatCur.PatNum);
			}
			Recalls.SynchScheduledApptFull(apt.PatNum);
			Plugins.HookAddCode(this,"ContrAppt.OnDelete_Click_end",apt,PatCur);
		}

		private void PrintApptLabel() {
			Appointment apt=Appointments.GetOneApt(ContrApptSingle.SelectedAptNum);
			if(ApptIsNull(apt)) { return; }
			LabelSingle.PrintAppointment(ContrApptSingle.SelectedAptNum);
		}

		private void OnBlockCopy_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Blockouts)) {
				return;
			}
			//not even enabled if not right click on a blockout
			Schedule SchedCur=GetClickedBlockout();
			if(SchedCur==null) {
				MessageBox.Show("Blockout not found.");
				return;//should never happen
			}
			BlockoutClipboard=SchedCur.Copy();
		}

		private void OnBlockCut_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Blockouts)) {
				return;
			}
			//not even enabled if not right click on a blockout
			Schedule SchedCur=GetClickedBlockout();
			if(SchedCur==null) {
				MessageBox.Show("Blockout not found.");
				return;//should never happen
			}
			BlockoutClipboard=SchedCur.Copy();
			Schedules.Delete(SchedCur);
			SecurityLogs.MakeLogEntry(Permissions.Blockouts,0,"Blockout cut.");
			RefreshPeriodSchedules();
		}

		private void OnBlockPaste_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Blockouts)) {
				return;
			}
			Schedule sched=BlockoutClipboard.Copy();
			sched.Ops=new List<long>();
			sched.Ops.Add(SheetClickedonOp);
			sched.SchedDate=AppointmentL.DateSelected;
			if(ApptDrawing.IsWeeklyView) {
				sched.SchedDate=WeekStartDate.AddDays(SheetClickedonDay);
			}
			TimeSpan span=sched.StopTime-sched.StartTime;
			TimeSpan timeOfDay=new TimeSpan(SheetClickedonHour,SheetClickedonMin,0);
			timeOfDay=TimeSpan.FromMinutes(
				((int)Math.Round((decimal)timeOfDay.TotalMinutes/(decimal)ApptDrawing.MinPerIncr))*ApptDrawing.MinPerIncr);
			sched.StartTime=timeOfDay;
			sched.StopTime=sched.StartTime.Add(span);
			if(sched.StopTime >= TimeSpan.FromDays(1)) {//long span that spills over to next day
				MsgBox.Show(this,"This Blockout would go past midnight.");
				return;
			}
			sched.ScheduleNum=0;//Because Schedules.Overlaps() ignores matching ScheduleNums and we used the Copy() function above. Also, we insert below, so a new key will be created anyway.
			if(Schedules.Overlaps(sched)) {
				MsgBox.Show(this,"Blockouts not allowed to overlap.");
				return;
			}
			Schedules.Insert(sched,true);
			SecurityLogs.MakeLogEntry(Permissions.Blockouts,0,"Blockout paste.");
			RefreshPeriodSchedules();
		}

		private void OnBlockEdit_Click(object sender,EventArgs e) {
			//Pre-calculate the list of blockouts to show.  If the user doesn't have the permission then a modified list is shown.
			List<Def> listUserBlockoutDefs=Defs.GetDefsForCategory(DefCat.BlockoutTypes, true);	
			if(!Security.IsAuthorized(Permissions.Blockouts,true)) {
				//The modified list will only show blockouts marked as "DontCopy" or "NoSchedule"
				listUserBlockoutDefs.RemoveAll(x => !x.ItemValue.Contains(BlockoutType.DontCopy.GetDescription()) 
					&& !x.ItemValue.Contains(BlockoutType.NoSchedule.GetDescription()));
			}
			//not even enabled if not right click on a blockout
			Schedule SchedCur=GetClickedBlockout();
			if(SchedCur==null) {
				MessageBox.Show("Blockout not found.");
				return;//should never happen
			}
			FormScheduleBlockEdit FormSB=new FormScheduleBlockEdit(SchedCur,Clinics.ClinicNum,listUserBlockoutDefs);
			FormSB.ShowDialog();
			RefreshPeriodSchedules();
		}

		private Schedule GetClickedBlockout() {
			DateTime startDate;
			DateTime endDate;
			if(ApptDrawing.IsWeeklyView) {
				startDate=WeekStartDate;
				endDate=WeekEndDate;
			}
			else {
				startDate=AppointmentL.DateSelected;
				endDate=AppointmentL.DateSelected;
			}
			//no need to do this since schedule is refreshed in RefreshPeriod().
			//SchedListPeriod=Schedules.RefreshPeriod(startDate,endDate);
			Schedule[] ListForType=Schedules.GetForType(SchedListPeriod,ScheduleType.Blockout,0);
			//now find which blockout
			Schedule SchedCur=null;
			//date is irrelevant. This is just for the time:
			DateTime SheetClickedonTime=new DateTime(2000,1,1,SheetClickedonHour,SheetClickedonMin,0);
			for(int i=0;i<ListForType.Length;i++) {
				//skip if op doesn't match
				if(!ListForType[i].Ops.Contains(SheetClickedonOp)) {
					continue;
				}
				if(ListForType[i].SchedDate.Date!=WeekStartDate.AddDays(SheetClickedonDay)) {
					continue;
				}
				if(ListForType[i].StartTime <= SheetClickedonTime.TimeOfDay
					&& SheetClickedonTime.TimeOfDay < ListForType[i].StopTime) {
					SchedCur=ListForType[i];
					break;
				}
			}
			return SchedCur;//might be null;
		}

		private void OnBlockDelete_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Blockouts)) {
				return;
			}
			Schedule SchedCur=GetClickedBlockout();
			if(SchedCur==null) {
				MessageBox.Show("Blockout not found.");
				return;//should never happen
			}
			Schedules.Delete(SchedCur);
			SecurityLogs.MakeLogEntry(Permissions.Blockouts,0,"Blockout delete.");
			RefreshPeriodSchedules();
		}

		private void OnBlockAdd_Click(object sender,EventArgs e) {

			//Pre-calculate the list of Blockout Types to show in FormScheduleBlockEdit
			List<Def> listUserBlockoutDefs=Defs.GetDefsForCategory(DefCat.BlockoutTypes, true);	
			if(!Security.IsAuthorized(Permissions.Blockouts,true)) {
				//This is a special case, so we only keep blockouts that are marked as NoSchedule or DontCopy
				listUserBlockoutDefs.RemoveAll(x => !x.ItemValue.Contains(BlockoutType.DontCopy.GetDescription()) 
					&& !x.ItemValue.Contains(BlockoutType.NoSchedule.GetDescription()));
			}
			Schedule SchedCur=new Schedule();
			SchedCur.SchedDate=AppointmentL.DateSelected;
			//Get the closest time in regards to the Appt View Time Increment preference.  Round the time down.
			int minutes=(int)((ContrAppt.SheetClickedonMin/ApptDrawing.MinPerIncr)*ApptDrawing.MinPerIncr);
			SchedCur.StartTime=new TimeSpan(ContrAppt.SheetClickedonHour,minutes,0);
			SchedCur.StopTime=new TimeSpan(ContrAppt.SheetClickedonHour+1,minutes,0);
			if(SchedCur.StartTime>TimeSpan.FromHours(23)) {//if user clicked anywhere during the last hour of the day, set blockout to the last hour of the day.
				SchedCur.StartTime=new TimeSpan(23,00,00);
				SchedCur.StopTime=new TimeSpan(23,59,00);
			}
			if(ApptDrawing.IsWeeklyView) {
				SchedCur.SchedDate=WeekStartDate.AddDays(SheetClickedonDay);
			}
			SchedCur.SchedType=ScheduleType.Blockout;
			FormScheduleBlockEdit FormSB=new FormScheduleBlockEdit(SchedCur,Clinics.ClinicNum, listUserBlockoutDefs);
			FormSB.IsNew=true;
			FormSB.ShowDialog();
			RefreshPeriodSchedules();
		}

		private void OnBlockCutCopyPaste_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Blockouts)) {
				return;
			}
			FormBlockoutCutCopyPaste FormB=new FormBlockoutCutCopyPaste();
			FormB.DateSelected=AppointmentL.DateSelected;
			if(ApptDrawing.IsWeeklyView) {
				FormB.DateSelected=WeekStartDate.AddDays(SheetClickedonDay);
			}
			if(comboView.SelectedIndex==0) {
				FormB.ApptViewNumCur=0;
			}
			else {
				FormB.ApptViewNumCur=_listApptViews[comboView.SelectedIndex-1].ApptViewNum;
			}
			FormB.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Blockouts,0,"Blockout cut copy paste.");
			RefreshPeriodSchedules();
		}

		private void OnClearBlockouts_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Blockouts)) {
				return;
			}
			if(!MsgBox.Show(this,true,"Clear all blockouts for day? (This may include blockouts not shown in the current appointment view)")) {
				return;
			}
			if(ApptDrawing.IsWeeklyView) {
				Schedules.ClearBlockoutsForDay(WeekStartDate.AddDays(SheetClickedonDay));
			}
			else {
				Schedules.ClearBlockoutsForDay(AppointmentL.DateSelected);
			}
			SecurityLogs.MakeLogEntry(Permissions.Blockouts,0,"Blockout clear.");
			RefreshPeriodSchedules();
		}

		private void OnClearBlockoutsOp_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Blockouts)) {
				return;
			}
			if(!MsgBox.Show(this,true,"Clear all blockouts for day and operatory?")) {
				return;
			}
			DateTime dateClear=AppointmentL.DateSelected;
			if(ApptDrawing.IsWeeklyView) {
				dateClear=WeekStartDate.AddDays(SheetClickedonDay);
			}
			Schedules.ClearBlockoutsForOp(SheetClickedonOp,dateClear);
			SecurityLogs.MakeLogEntry(Permissions.Blockouts,0,"Blockout clear operatory: "+Operatories.GetAbbrev(SheetClickedonOp));
			RefreshPeriodSchedules();
		}

		private void OnClearBlockoutsClinic_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Blockouts)) {
				return;
			}
			if(!MsgBox.Show(this,true,"Clear all blockouts for day and clinic?")) {
				return;
			}
			DateTime dateClear = AppointmentL.DateSelected;
			if(ApptDrawing.IsWeeklyView) {
				dateClear=WeekStartDate.AddDays(SheetClickedonDay);
			}
			Operatory operatory=Operatories.GetOperatory(SheetClickedonOp);
			Schedules.ClearBlockoutsForClinic(operatory.ClinicNum,dateClear);
			SecurityLogs.MakeLogEntry(Permissions.Blockouts,0,"Blockout clear clinic: "+Operatories.GetAbbrev(SheetClickedonOp));
			RefreshPeriodSchedules();
		}

		private void OnBlockTypes_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormDefinitions FormD=new FormDefinitions(DefCat.BlockoutTypes);
			FormD.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Definitions.");
			RefreshPeriodSchedules();
		}

		private void OnCopyToPin_Click() {
			if(!Security.IsAuthorized(Permissions.AppointmentMove)) {
				return;
			}
			//cannot allow moving completed procedure because it could cause completed procs to change date.  Security must block this.
			//ContrApptSingle3[thisIndex].DataRoww;
			Appointment appt=Appointments.GetOneApt(ContrApptSingle.SelectedAptNum);
			if(appt==null) {
				MsgBox.Show(this,"Appointment not found.");
				return;
			}
			if(appt.AptStatus==ApptStatus.Complete) {
				MsgBox.Show(this,"Not allowed to move completed appointments.");
				return;
			}
			if(PatRestrictionL.IsRestricted(appt.PatNum,PatRestrict.ApptSchedule)) {
				return;
			}
			int prevSel=GetIndex(ContrApptSingle.SelectedAptNum);
			SendToPinBoard(ContrApptSingle.SelectedAptNum);//sets selectedAptNum=-1. do before refresh prev
			if(prevSel!=-1) {
				ContrApptSheet2.DoubleBufferDraw(drawToScreen: true);
			}
		}

		private void OnTextASAPList_Click(object sender,EventArgs e) {
			if(PrefC.GetBool(PrefName.WebSchedAsapEnabled)) {
				//Get the closest time selected.
				int minutes=(SheetClickedonMin/ApptDrawing.MinPerIncr)*ApptDrawing.MinPerIncr;
				DateTime timeChosen=AppointmentL.DateSelected.Date.AddHours(SheetClickedonHour).AddMinutes(minutes);
				DisplayFormAsapForWebSched(SheetClickedonOp,timeChosen);
			}
			else {//Texting the ASAP list manually
				//Get the closest time selected.
				int minutes=(SheetClickedonMin/ApptDrawing.MinPerIncr)*ApptDrawing.MinPerIncr;
				DateTime timeChosen=AppointmentL.DateSelected.Date.AddHours(SheetClickedonHour).AddMinutes(minutes);
				if(_formASAP!=null && !_formASAP.IsDisposed) {
					_formASAP.Close();
				}
				_formASAP=new FormASAP(timeChosen);
				_formASAP.Show();
				if(_formASAP.WindowState==FormWindowState.Minimized) {
					_formASAP.WindowState=FormWindowState.Normal;
				}
				_formASAP.BringToFront();
			}
		}

		private void FormASAP_FormClosed(object sender,FormClosedEventArgs e) {
			RefreshModuleDataPeriod();
			RefreshModuleScreenPeriod();
		}

		private void OnUpdateProvs_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup) || !Security.IsAuthorized(Permissions.AppointmentEdit)) {
				return;
			}
			Operatory operatory=Operatories.GetOperatory(SheetClickedonOp);
			if(Security.CurUser.ClinicIsRestricted && !Clinics.GetForUserod(Security.CurUser).Exists(x => x.ClinicNum==operatory.ClinicNum)) {
				MsgBox.Show(this,"You are restricted from accessing the clinic belonging to the selected operatory.  No changes will be made.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,
				"WARNING: We recommend backing up your database before running this tool.  "
				+"This tool may take a very long time to run and should be run after hours.  "
				+"In addition, this tool could potentially change hundreds of appointments.  "
				+"The changes made by this tool can only be manually reversed.  "
				+"Are you sure you want to continue?"))
			{
				return;
			}
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Update Provs on Future Appts tool run on operatory "+operatory.Abbrev+".");
			List<Appointment> listAppts=Appointments.GetAppointmentsForOpsByPeriod(new List<long>() {operatory.OperatoryNum},DateTime.Now);
			List<Appointment> listApptsOld=new List<Appointment>();
			foreach(Appointment appt in listAppts) {
				listApptsOld.Add(appt.Copy());
			}
			MoveAppointments(listAppts,listApptsOld,operatory,false,false,true);
			MsgBox.Show(this,"Done");
		}

		///<summary>Updates appointment and procedure information based on information passed in.
		///Occurs when moving an appointment.  Also similar to the logic which runs in pinBoard_MouseUp(), but pinBoard_MouseUp() has additional things that are done.</summary>
		private void MoveAppointments(List<Appointment> listAppts,List<Appointment> listApptsOld,Operatory curOp,bool timeWasMoved,bool isOpChanged,bool isOpUpdate=false) {
			Appointment apt=null;
			Appointment aptOld=null;
			Patient patCur=null;
			List<Schedule> listSchedsForOp=Schedules.GetSchedsForOp(curOp,listAppts.Select(x => x.AptDateTime).ToList());
			for(int i=0;i<listAppts.Count;i++) {
				apt=listAppts[i];
				aptOld=listApptsOld[i];
				apt.Op=curOp.OperatoryNum;
				patCur=Patients.GetPat(apt.PatNum);
				bool provChanged=false;
				bool hygChanged=false;
				long assignedDent=0;
				long assignedHyg=0;
				if(isOpUpdate) {
					assignedDent=Schedules.GetAssignedProvNumForSpot(listSchedsForOp,curOp,false,apt.AptDateTime);
					assignedHyg=Schedules.GetAssignedProvNumForSpot(listSchedsForOp,curOp,true,apt.AptDateTime);
				}
				else { 
					assignedDent=Schedules.GetAssignedProvNumForSpot(SchedListPeriod,curOp,false,apt.AptDateTime);
					assignedHyg=Schedules.GetAssignedProvNumForSpot(SchedListPeriod,curOp,true,apt.AptDateTime);
				}
				List<Procedure> procsForSingleApt=null;
				if(apt.AptStatus!=ApptStatus.PtNote && apt.AptStatus!=ApptStatus.PtNoteCompleted) {
					if(timeWasMoved) {
						#region Update Appt's DateTimeAskedToArrive
						if(patCur.AskToArriveEarly>0) {
							apt.DateTimeAskedToArrive=apt.AptDateTime.AddMinutes(-patCur.AskToArriveEarly);
							MessageBox.Show(Lan.g(this,"Ask patient to arrive")+" "+patCur.AskToArriveEarly
								+" "+Lan.g(this,"minutes early at")+" "+apt.DateTimeAskedToArrive.ToShortTimeString()+".");
						}
						else {
							if(apt.DateTimeAskedToArrive.Year>1880 && (aptOld.AptDateTime-aptOld.DateTimeAskedToArrive).TotalMinutes>0) {
								apt.DateTimeAskedToArrive=apt.AptDateTime-(aptOld.AptDateTime-aptOld.DateTimeAskedToArrive);
								if(MessageBox.Show(Lan.g(this,"Ask patient to arrive")+" "+(aptOld.AptDateTime-aptOld.DateTimeAskedToArrive).TotalMinutes
									+" "+Lan.g(this,"minutes early at")+" "+apt.DateTimeAskedToArrive.ToShortTimeString()+"?","",MessageBoxButtons.YesNo)==DialogResult.No) {
									apt.DateTimeAskedToArrive=aptOld.DateTimeAskedToArrive;
								}
							}
							else {
								apt.DateTimeAskedToArrive=DateTime.MinValue;
							}
						}
						#endregion Update Appt's DateTimeAskedToArrive
					}
					#region Update Appt's Update Appt's ProvNum, ProvHyg, IsHygiene, Pattern
					//if no dentist/hygenist is assigned to spot, then keep the original dentist/hygenist without prompt.  All appts must have prov.
					if((assignedDent!=0 && assignedDent!=apt.ProvNum) || (assignedHyg!=0 && assignedHyg!=apt.ProvHyg)) {
						object[] parameters3={ apt,assignedDent,assignedHyg,procsForSingleApt,this };//Only used in following plugin hook.
						if((Plugins.HookMethod(this,"ContrAppt.ContrApptSheet2_MouseUp_apptProvChangeQuestion",parameters3))) {
							apt=(Appointment)parameters3[0];
							assignedDent=(long)parameters3[1];
							assignedDent=(long)parameters3[2];
							goto PluginApptProvChangeQuestionEnd;
						}
						if(isOpUpdate || MsgBox.Show(this,MsgBoxButtons.YesNo,"Change provider?")) {//Short circuit logic.  If we're updating op through right click, never ask.
							if(assignedDent!=0) {//the dentist will only be changed if the spot has a dentist.
								apt.ProvNum=assignedDent;
								provChanged=true;
							}
							if(assignedHyg!=0 || PrefC.GetBool(PrefName.ApptSecondaryProviderConsiderOpOnly)) {//the hygienist will only be changed if the spot has a hygienist.
								apt.ProvHyg=assignedHyg;
								hygChanged=true;
							}
							if(curOp.IsHygiene) {
								apt.IsHygiene=true;
							}
							else {//op not marked as hygiene op
								if(assignedDent==0) {//no dentist assigned
									if(assignedHyg!=0) {//hyg is assigned (we don't really have to test for this)
										apt.IsHygiene=true;
									}
								}
								else {//dentist is assigned
									if(assignedHyg==0) {//hyg is not assigned
										apt.IsHygiene=false;
									}
									//if both dentist and hyg are assigned, it's tricky
									//only explicitly set it if user has a dentist assigned to the op
									if(curOp.ProvDentist!=0) {
										apt.IsHygiene=false;
									}
								}
							}
							procsForSingleApt=Procedures.GetProcsForSingle(apt.AptNum,false);
							List<long> codeNums=new List<long>();
							for(int p = 0;p<procsForSingleApt.Count;p++) {
								codeNums.Add(procsForSingleApt[p].CodeNum);
							}
							if(!isOpUpdate) { 
								string calcPattern=Appointments.CalculatePattern(apt.ProvNum,apt.ProvHyg,codeNums,true);
								if(apt.Pattern!=calcPattern && !PrefC.GetBool(PrefName.AppointmentTimeIsLocked)) {//Updating op provs will not change apt lengths.
									if(apt.TimeLocked) {
										if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Appointment length is locked.  Change length for new provider anyway?")) {
											apt.Pattern=calcPattern;
										}
									}
									else {//appt time not locked
										if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Change length for new provider?")) {
											apt.Pattern=calcPattern;
										}
									}
								}
							}
						}
						PluginApptProvChangeQuestionEnd: { }
					}
					#endregion Update Appt's ProvNum, ProvHyg, IsHygiene, Pattern
				}
				#region Prevent overlap
				if(!isOpUpdate && !Appointments.TryAdjustAppointmentOp(apt,ApptDrawing.VisOps)) {
					MessageBox.Show(Lan.g(this,"Appointment overlaps existing appointment or blockout."));
					continue;
				}
				#endregion Prevent overlap
				#region Detect Frequency Conflicts
				//Detect frequency conflicts with procedures in the appointment
				if(!isOpUpdate && PrefC.GetBool(PrefName.InsChecksFrequency)) {
					procsForSingleApt=Procedures.GetProcsForSingle(apt.AptNum,false);
					string frequencyConflicts="";
					try {
						frequencyConflicts=Procedures.CheckFrequency(procsForSingleApt,apt.PatNum,apt.AptDateTime);
					}
					catch(Exception e) {
						MessageBox.Show(Lan.g(this,"There was an error checking frequencies."
							+"  Disable the Insurance Frequency Checking feature or try to fix the following error:")
							+"\r\n"+e.Message);
						continue;
					}
					if(frequencyConflicts!="" && MessageBox.Show(Lan.g(this,"Scheduling this appointment for this date will cause frequency conflicts for the following procedures")
						+":\r\n"+frequencyConflicts+"\r\n"+Lan.g(this,"Do you want to continue?"),"",MessageBoxButtons.YesNo)==DialogResult.No)
					{
						continue;
					}
				}
				#endregion Detect Frequency Conflicts
				#region Patient status
				if(!isOpUpdate) {
					Operatory opCur=Operatories.GetOperatory(apt.Op);
					Operatory opOld=Operatories.GetOperatory(aptOld.Op);
					if(opOld==null||opCur.SetProspective!=opOld.SetProspective) {
						if(opCur.SetProspective&&patCur.PatStatus!=PatientStatus.Prospective) { //Don't need to prompt if patient is already prospective.
							if(MsgBox.Show(this,MsgBoxButtons.OKCancel,"Patient's status will be set to Prospective.")) {
								Patient patOld=patCur.Copy();
								patCur.PatStatus=PatientStatus.Prospective;
								Patients.Update(patCur,patOld);
							}
						}
						else if(!opCur.SetProspective&&patCur.PatStatus==PatientStatus.Prospective) {
							//Do we need to warn about changing FROM prospective? Assume so for now.
							if(MsgBox.Show(this,MsgBoxButtons.OKCancel,"Patient's status will change from Prospective to Patient.")) {
								Patient patOld=patCur.Copy();
								patCur.PatStatus=PatientStatus.Patient;
								Patients.Update(patCur,patOld);
							}
						}
					}
				}
				#endregion Patient status
				#region Update Appt's AptStatus, ClinicNum, Confirmed
				object[] parameters2 = { apt.AptDateTime,aptOld.AptDateTime,apt.AptStatus };
				if((Plugins.HookMethod(this,"ContrAppt.ContrApptSheet2_MouseUp_apptDoNotUnbreakApptSameDay",parameters2))) {
					apt.AptStatus=(ApptStatus)parameters2[2];
					goto PluginApptDoNotUnbreakApptSameDay;
				}
				if(apt.AptStatus==ApptStatus.Broken&&(timeWasMoved||isOpChanged)) {
					apt.AptStatus=ApptStatus.Scheduled;
				}
				PluginApptDoNotUnbreakApptSameDay: { }
				//original location of provider code
				if(curOp.ClinicNum==0) {
					apt.ClinicNum=patCur.ClinicNum;
				}
				else {
					apt.ClinicNum=curOp.ClinicNum;
				}
				if(apt.AptDateTime!=aptOld.AptDateTime
					&&apt.Confirmed!=Defs.GetFirstForCategory(DefCat.ApptConfirmed,true).DefNum
					&&apt.AptDateTime.Date!=DateTime.Today) 
				{
					string prompt="Reset Confirmation Status?";
					if(PrefC.GetBool(PrefName.ApptConfirmAutoEnabled)) {
						prompt+=" This will resend the eConfirmation within the next few minutes.";
					}
					if(MsgBox.Show(this,MsgBoxButtons.YesNo,prompt)) {
						apt.Confirmed=Defs.GetFirstForCategory(DefCat.ApptConfirmed,true).DefNum;//Causes the confirmation status to be reset.
					}
				}
				#endregion Update Appt's AptStatus, ClinicNum, Confirmed
				try {
					if(isOpUpdate) {
						Appointments.MoveValidatedAppointment(apt,aptOld,patCur,curOp,listSchedsForOp,ApptDrawing.VisOps,provChanged,hygChanged,timeWasMoved,isOpChanged,isOpUpdate);
					}
					else { 
						Appointments.MoveValidatedAppointment(apt,aptOld,patCur,curOp,SchedListPeriod,ApptDrawing.VisOps,provChanged,hygChanged,timeWasMoved,isOpChanged,isOpUpdate);
					}
				}
				catch(Exception e) {
					MsgBox.Show(this,e.Message);
				}
			}
		}
		
		private void listConfirmed_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			if(listConfirmed.IndexFromPoint(e.X,e.Y)==-1) {
				return;
			}
			if(ContrApptSingle.SelectedAptNum==-1) {
				return;
			}
			Appointment aptOld=Appointments.GetOneApt(ContrApptSingle.SelectedAptNum);
			DateTime datePrevious=aptOld.DateTStamp;
			long newStatus=Defs.GetDefsForCategory(DefCat.ApptConfirmed,true)[listConfirmed.IndexFromPoint(e.X,e.Y)].DefNum;
			long oldStatus=aptOld.Confirmed;
			Appointments.SetConfirmed(aptOld,newStatus);//Appointments S-Class handles Signalods
			if(newStatus!=oldStatus) {
				//Log confirmation status changes.
				SecurityLogs.MakeLogEntry(Permissions.ApptConfirmStatusEdit,PatCur.PatNum,Lans.g(this,"Appointment confirmation status changed from")+" "
					+Defs.GetName(DefCat.ApptConfirmed,oldStatus)+" "+Lans.g(this,"to")+" "+Defs.GetName(DefCat.ApptConfirmed,newStatus)
					+" "+Lans.g(this,"from the appointment module")+".",ContrApptSingle.SelectedAptNum,datePrevious);
			}
			RefreshPeriod();
		}
		
		private void menuJobs_Attach(object sender,System.EventArgs e) {
			if(ContrApptSingle.SelectedAptNum<=0) {
				return;
			}
			//Atach new job
			FormJobSearch FormJS = new FormJobSearch();
			FormJS.ShowDialog();
			if(FormJS.DialogResult!=DialogResult.OK || FormJS.SelectedJob==null) {
				return;
			}
			JobLink jobLink = new JobLink();
			jobLink.JobNum=FormJS.SelectedJob.JobNum;
			jobLink.FKey=ContrApptSingle.SelectedAptNum;
			jobLink.LinkType=JobLinkType.Appointment;
			JobLinks.Insert(jobLink);
			Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,jobLink.JobNum);
			return;
		}

		private void menuJobs_GoToJob(object sender,System.EventArgs e) {
				FormOpenDental.S_GoToJob(((long)((MenuItem)sender).Tag));
		}

		private void butSearch_Click(object sender,System.EventArgs e) {
			if(pinBoard.ApptList.Count==0) {
				MsgBox.Show(this,"An appointment must be placed on the pinboard before a search can be done.");
				return;
			}
			if(pinBoard.SelectedIndex==-1) {
				if(pinBoard.ApptList.Count==1) {
					pinBoard.SelectedIndex=0;
				}
				else {
					MsgBox.Show(this,"An appointment on the pinboard must be selected before a search can be done.");
					return;
				}
			}
			if(!groupSearch.Visible) {//if search not already visible
				dateSearch.Text=DateTime.Today.ToShortDateString();
				ShowSearch();
			}
			DoSearch();
		}

		///<summary>Positions the search box, fills it with initial data except date, and makes it visible.</summary>
		private void ShowSearch() {
			ProviderList=new List<Provider>();
			List<Provider> listProvidersShort=Providers.GetDeepCopy(true);
			groupSearch.Location=new Point(panelCalendar.Location.X,panelCalendar.Location.Y+pinBoard.Bottom+2);
			textBefore.Text="";
			textAfter.Text="";
			listProviders.Items.Clear();
			for(int i=0;i<listProvidersShort.Count;i++) {
				if(pinBoard.SelectedAppt.IsHygiene
					&& listProvidersShort[i].ProvNum==pinBoard.SelectedAppt.ProvHyg) {
					listProviders.Items.Add(listProvidersShort[i].Abbr);//If their appiontment is hygine, the list will start with just their hygine provider
					ProviderList.Add(listProvidersShort[i]);
				}
				else if(!pinBoard.SelectedAppt.IsHygiene
					&& listProvidersShort[i].ProvNum==pinBoard.SelectedAppt.ProvNum) {
					listProviders.Items.Add(listProvidersShort[i].Abbr);//If their appointment is not hygine, they will start with just their primary provider
					ProviderList.Add(listProvidersShort[i]);
				}
			}
			Plugins.HookAddCode(this,"ContrAppt.ShowSearch_end",listProviders,pinBoard.SelectedAppt.AptNum);
			groupSearch.Visible=true;
		}

		private void DoSearch() {
			Cursor=Cursors.WaitCursor;
			DateTime afterDate;
			try {
				afterDate=PIn.Date(dateSearch.Text);
				if(afterDate.Year<1880) {
					throw new Exception();
				}
			}
			catch {
				Cursor=Cursors.Default;
				MsgBox.Show(this,"Invalid date.");
				return;
			}
			TimeSpan beforeTime=new TimeSpan(0);
			if(textBefore.Text!="") {
				try {
					string[] hrmin=textBefore.Text.Split(new char[] { ':' },StringSplitOptions.RemoveEmptyEntries);//doesn't work with foreign times.
					string hr="0";
					if(hrmin.Length>0) {
						hr=hrmin[0];
					}
					string min="0";
					if(hrmin.Length>1) {
						min=hrmin[1];
					}
					beforeTime=TimeSpan.FromHours(PIn.Double(hr))
						+TimeSpan.FromMinutes(PIn.Double(min));
					if(radioBeforePM.Checked && beforeTime.Hours<12) {
						beforeTime=beforeTime+TimeSpan.FromHours(12);
					}
				}
				catch {
					Cursor=Cursors.Default;
					MsgBox.Show(this,"Invalid time.");
					return;
				}
			}
			TimeSpan afterTime=new TimeSpan(0);
			if(textAfter.Text!="") {
				try {
					string[] hrmin=textAfter.Text.Split(new char[] { ':' },StringSplitOptions.RemoveEmptyEntries);//doesn't work with foreign times.
					string hr="0";
					if(hrmin.Length>0) {
						hr=hrmin[0];
					}
					string min="0";
					if(hrmin.Length>1) {
						min=hrmin[1];
					}
					afterTime=TimeSpan.FromHours(PIn.Double(hr))
						+TimeSpan.FromMinutes(PIn.Double(min));
					if(radioAfterPM.Checked && afterTime.Hours<12) {
						afterTime=afterTime+TimeSpan.FromHours(12);
					}
				}
				catch {
					Cursor=Cursors.Default;
					MsgBox.Show(this,"Invalid time.");
					return;
				}
			}
			if(listProviders.Items.Count==0) {
				Cursor=Cursors.Default;
				MsgBox.Show(this,"Please pick a provider.");
				return;
			}
			long[] providers=new long[listProviders.Items.Count];
			List<long> providerNums = new List<long>();
			for(int i=0;i<providers.Length;i++) {
				providers[i]=ProviderList[i].ProvNum;
				providerNums.Add(ProviderList[i].ProvNum);
				//providersList.Add(providers[i]);
			}
			//the result might be empty
			SearchResults=AppointmentL.GetSearchResults(pinBoard.SelectedAppt.AptNum,
				afterDate,providerNums,10,beforeTime,afterTime);
			listSearchResults.Items.Clear();
			for(int i=0;i<SearchResults.Count;i++) {
				listSearchResults.Items.Add(
					SearchResults[i].ToString("ddd")+"\t"+SearchResults[i].ToShortDateString()+"     "+SearchResults[i].ToShortTimeString());
			}
			if(listSearchResults.Items.Count>0) {
				listSearchResults.SetSelected(0,true);
				AppointmentL.DateSelected=SearchResults[0];
			}
			SetWeeklyView(false);//jump to that day.
			Cursor=Cursors.Default;
			//scroll to make visible?
			//highlight schedule?*/
		}

		private void butSearchMore_Click(object sender,System.EventArgs e) {
			if(pinBoard.SelectedAppt==null) {
				MsgBox.Show(this,"There is no appointment on the pinboard.");
				return;
			}
			if(SearchResults==null || SearchResults.Count<1) {
				return;
			}
			dateSearch.Text=SearchResults[SearchResults.Count-1].ToShortDateString();
			DoSearch();
		}

		private void listSearchResults_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			int clickedI=listSearchResults.IndexFromPoint(e.X,e.Y);
			if(clickedI==-1) {
				return;
			}
			AppointmentL.DateSelected=SearchResults[clickedI];
			SetWeeklyView(false);
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			if(pinBoard.SelectedAppt==null ) {
				if(pinBoard.ApptList.Count>0) {//if there are any appointments in the pinboard.
					pinBoard.SelectedIndex=pinBoard.ApptList.Count-1;//select last appt
				}
				else {
					MsgBox.Show(this,"There are no appointments on the pinboard.");
					return;
				}
			}
			DoSearch();
		}

		private void butSearchCloseX_Click(object sender,System.EventArgs e) {
			groupSearch.Visible=false;
		}

		private void butSearchClose_Click(object sender,System.EventArgs e) {
			groupSearch.Visible=false;
		}

		private void button1_Click_1(object sender,System.EventArgs e) {
			MessageBox.Show(Lan.g(this,this.GetType().Name));
		}

		private void butLab_Click(object sender,EventArgs e) {
			FormLabCases FormL=new FormLabCases();
			FormL.ShowDialog();
			if(FormL.GoToAptNum!=0) {
				Appointment apt=Appointments.GetOneApt(FormL.GoToAptNum);
				Patient pat=Patients.GetPat(apt.PatNum);
				//PatientSelectedEventArgs eArgs=new OpenDental.PatientSelectedEventArgs(pat.PatNum,pat.GetNameLF(),pat.Email!="",pat.ChartNumber);
				//if(PatientSelected!=null){
				//	PatientSelected(this,eArgs);
				//}
				//Contr_PatientSelected(this,eArgs);
				FormOpenDental.S_Contr_PatientSelected(pat,false,false);
				GotoModule.GotoAppointment(apt.AptDateTime,apt.AptNum);
			}
		}

		///<summary>Happens once per minute.  It used to just move the red timebar down without querying the database.  
		///But now it queries the database so that the waiting room list shows accurately.  Always updates the waiting room.</summary>
		public void TickRefresh() {
			try {
				DateTime startDate;
				DateTime endDate;
				if(ApptDrawing.IsWeeklyView) {
					startDate=WeekStartDate;
					endDate=WeekEndDate;
				}
				else {
					startDate=AppointmentL.DateSelected;
					endDate=AppointmentL.DateSelected;
				}
				if(PrefC.GetBool(PrefName.ApptModuleRefreshesEveryMinute)) {
					if(PrefC.GetLong(PrefName.ProcessSigsIntervalInSecs)==0) {//Signal processing is disabled.
						RefreshPeriod(false);
					}
					else {
						//This is a tricky scenario.  We decided to run an extra RefreshTimed() query here for the following reasons:
						//1) We wanted to stay true to the feature name and refresh as much as we need to in order to "refresh everything."
						//2) Running the extra signal query which is lighter weight will most often save running the heavy schedule refresh.
						//  When it does not, at least TickRefresh() only runs once per minute, and the extra signal query should be somewhat neglegible.
						//3) Running RefreshTimed() here again should not take much server processing time, as the query result or at least most of the data
						//  in the query result should be cached due to how often worstations on the network will be requesting the signal list.
						//4) Reusing the signal list from FormOpenDental which was creating using RefreshTimed() was run on a different interval and might be a
						//  bit too stale to be true to this feature that must "refresh everything."
						List <Signalod> listSignals=Signalods.RefreshTimed(Signalods.SignalLastRefreshed);
						bool isApptRefresh=Signalods.IsApptRefreshNeeded(startDate,endDate,listSignals);
						bool isSchedRefresh=Signalods.IsSchedRefreshNeeded(startDate,endDate,listSignals);
						//either we have signals from other machines telling us to refresh, or we aren't using signals, in which case we still want to refresh
						RefreshPeriod(false,isRefreshAppointments:isApptRefresh,isRefreshSchedules:isSchedRefresh);
					}
				}
				else {
					ContrApptSheet2.RedrawAll(true);
				}
			}
			catch {
				//prevents rare malfunctions. For instance, during editing of views, if tickrefresh happens.
			}
			//GC.Collect();	
		}

		///<summary>"Ganga's Code: Printing the Appointment Card - 9/9/2004"</summary>
		private void PrintApptCard() {
			pd2=new PrintDocument();
			pd2.PrintPage+=new PrintPageEventHandler(this.pd2_PrintApptCard);
			pd2.DefaultPageSettings.Margins=new Margins(0,0,0,0);
			pd2.OriginAtMargins=true;//forces origin to upper left of actual page
#if DEBUG
			FormRpPrintPreview pView=new FormRpPrintPreview();
			pView.printPreviewControl2.Document=pd2;
			pView.ShowDialog();
#else
			if(PrinterL.SetPrinter(pd2,PrintSituation.Postcard,PatCur.PatNum,"Appointment reminder postcard printed")) {
				pd2.Print();
			}
#endif
		}

		private void pd2_PrintApptCard(object sender,PrintPageEventArgs ev) {
			Graphics g=ev.Graphics;
			long apptClinicNum=0;
			ContrApptSingle ctrl=ContrApptSheet2.ListContrApptSingles.FirstOrDefault(x => x.AptNum==ContrApptSingle.SelectedAptNum);
			if(ctrl!=null) {
				apptClinicNum=ctrl.ClinicNum;
			}			
			Clinic clinic=Clinics.GetClinic(apptClinicNum);
			//Return Address--------------------------------------------------------------------------
			string str="";
			string phone="";
			if(PrefC.HasClinicsEnabled && clinic!=null) {//Use clinic on appointment if clinic exists and has clinics enabled
				str=clinic.Description+"\r\n";
				g.DrawString(str,new Font(FontFamily.GenericSansSerif,9,FontStyle.Bold),Brushes.Black,60,60);
				str=clinic.Address+"\r\n";
				if(clinic.Address2!="") {
					str+=clinic.Address2+"\r\n";
				}
				str+=clinic.City+"  "+clinic.State+"  "+clinic.Zip+"\r\n";
				phone=clinic.Phone;
			}
			else {//Otherwise use practice information
				str=PrefC.GetString(PrefName.PracticeTitle)+"\r\n";
				g.DrawString(str,new Font(FontFamily.GenericSansSerif,9,FontStyle.Bold),Brushes.Black,60,60);
				str=PrefC.GetString(PrefName.PracticeAddress)+"\r\n";
				if(PrefC.GetString(PrefName.PracticeAddress2)!="") {
					str+=PrefC.GetString(PrefName.PracticeAddress2)+"\r\n";
				}
				str+=PrefC.GetString(PrefName.PracticeCity)+"  "
					+PrefC.GetString(PrefName.PracticeST)+"  "
					+PrefC.GetString(PrefName.PracticeZip)+"\r\n";
				phone=PrefC.GetString(PrefName.PracticePhone);
			}
			if(CultureInfo.CurrentCulture.Name=="en-US" && phone.Length==10) {
				str+="("+phone.Substring(0,3)+")"+phone.Substring(3,3)+"-"+phone.Substring(6);
			}
			else {//any other phone format
				str+=phone;
			}
			g.DrawString(str,new Font(FontFamily.GenericSansSerif,8),Brushes.Black,60,75);
			//Body text-------------------------------------------------------------------------------
			string name;
			str="Appointment Reminders:"+"\r\n\r\n";
			Appointment[] aptsOnePat;
			Family fam=Patients.GetFamily(PatCur.PatNum);
			Patient pat=fam.GetPatient(PatCur.PatNum);
			for(int i=0;i<fam.ListPats.Length;i++) {
				if(!cardPrintFamily && fam.ListPats[i].PatNum!=pat.PatNum) {
					continue;
				}
				name=fam.ListPats[i].FName;
				if(name.Length>15) {//trim name so it won't be too long
					name=name.Substring(0,15);
				}
				aptsOnePat=Appointments.GetForPat(fam.ListPats[i].PatNum);
				for(int a=0;a<aptsOnePat.Length;a++) {
					if(aptsOnePat[a].AptDateTime.Date<=DateTime.Today) {
						continue;//ignore old appts
					}
					if(aptsOnePat[a].AptStatus!=ApptStatus.Scheduled){
						continue;
					}
					str+=name+": "+aptsOnePat[a].AptDateTime.ToShortDateString()+" "+aptsOnePat[a].AptDateTime.ToShortTimeString()+"\r\n";
				}
			}
			g.DrawString(str,new Font(FontFamily.GenericSansSerif,9),Brushes.Black,40,180);
			//Patient's Address-----------------------------------------------------------------------
			Patient guar;
			if(cardPrintFamily) {
				guar=fam.ListPats[0].Copy();
			}
			else {
				guar=pat.Copy();
			}
			str=guar.FName+" "+guar.LName+"\r\n"
				+guar.Address+"\r\n";
			if(guar.Address2!="") {
				str+=guar.Address2+"\r\n";
			}
			str+=guar.City+"  "+guar.State+"  "+guar.Zip;
			g.DrawString(str,new Font(FontFamily.GenericSansSerif,11),Brushes.Black,300,240);
			//CommLog entry---------------------------------------------------------------------------
			Commlog CommlogCur=new Commlog();
			CommlogCur.CommDateTime=DateTime.Now;
			CommlogCur.CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.MISC);
			CommlogCur.Note="Appointment card sent";
			CommlogCur.PatNum=pat.PatNum;
			CommlogCur.UserNum=Security.CurUser.UserNum;
			//there is no dialog here because it is just a simple entry
			Commlogs.Insert(CommlogCur);
			ev.HasMorePages = false;
		}

		private void timerInfoBubble_Tick(object sender,EventArgs e) {
			InfoBubbleDraw(bubbleLocation);
			timerInfoBubble.Enabled =false;
		}

		private void butMonth_Click(object sender,EventArgs e) {
			FormMonthView FormM=new FormMonthView();
			FormM.ShowDialog();
		}

		private void timerWaitingRoom_Tick(object sender,EventArgs e) {
			FillWaitingRoom();
		}

		private void timerTests_Tick(object sender,EventArgs e) {
			//stress test #2:
			//ContrApptSheet2.CreateShadow();
			//CreateAptShadows();
			//ContrApptSheet2.DrawShadow();
			//stress test #3:
			//stressCounter++;
			//if(Math.IEEERemainder((double)stressCounter,500d)==0) {
			//	Debug.WriteLine("stress counter: "+stressCounter.ToString());
			//}
			//FillWaitingRoom();
			//stress test #4:
			//LayoutScrollOpProv();
			//if(PatCur!=null) {
			//	this.ModuleSelected(PatCur.PatNum);
			//}
		}

		private void butGraph_Click(object sender,EventArgs e) {
			//only visible on computers at OD corporate.
			FormGraphEmployeeTime form=new FormGraphEmployeeTime();
			form.ShowDialog();
		}

		private void butProvPick_Click(object sender,EventArgs e) {
			FormProvidersMultiPick FormPMP=new FormProvidersMultiPick();
			FormPMP.SelectedProviders=ProviderList;
			FormPMP.ShowDialog();
			if(FormPMP.DialogResult!=DialogResult.OK) {
				return;
			}
			listProviders.Items.Clear();
			for(int i=0;i<FormPMP.SelectedProviders.Count;i++) {
				listProviders.Items.Add(FormPMP.SelectedProviders[i].Abbr);
			}
			ProviderList=FormPMP.SelectedProviders;
			if(pinBoard.SelectedAppt==null) {
				MsgBox.Show(this,"There is no appointment on the pinboard.");
				return;
			}
			DoSearch();
		}

		private void butProvHygenist_Click(object sender,EventArgs e) {
			ProviderList=new List<Provider>();
			listProviders.Items.Clear();
			List<Provider> listProvidersShort=Providers.GetDeepCopy(true);
			for(int i=0;i<listProvidersShort.Count;i++) {
				if(ApptViewItemL.ProvIsInView(listProvidersShort[i].ProvNum)) {
					if(listProvidersShort[i].IsSecondary) {
						ProviderList.Add(listProvidersShort[i]);
						listProviders.Items.Add(listProvidersShort[i].Abbr);
					}
				}
			}
			if(pinBoard.SelectedAppt==null) {
				MsgBox.Show(this,"There is no appointment on the pinboard.");
				return;
			}
			DoSearch();
		}

		private void butProvDentist_Click(object sender,EventArgs e) {
			ProviderList=new List<Provider>();
			listProviders.Items.Clear();
			List<Provider> listProvidersShort=Providers.GetDeepCopy(true);
			for(int i=0;i<listProvidersShort.Count;i++){
				if(ApptViewItemL.ProvIsInView(listProvidersShort[i].ProvNum)) {
					if(!listProvidersShort[i].IsSecondary) {
						ProviderList.Add(listProvidersShort[i]);
						listProviders.Items.Add(listProvidersShort[i].Abbr);
					}
				}
			}
			if(pinBoard.SelectedAppt==null) {
				MsgBox.Show(this,"There is no appointment on the pinboard.");
				return;
			}
			DoSearch();
		}

		///<summary>Handles the display and refresh when the appointment we are trying to operate on is null.</summary>
		private bool ApptIsNull(Appointment appt) {
			if(appt==null) {
				MsgBox.Show(this,"Selected appointment no longer exists.");
				RefreshPeriod();
				return true;
			}
			return false;
		}

		private class OpPanel {
			private Panel _backPanel;
			private Label _labelOpName;
			private ToolTip _opToolTip=new ToolTip();

			public OpPanel(ContrAppt apptModule,Operatory op,Provider prov,int panelHeight,Point location) {
				_backPanel=new Panel();
				_labelOpName=new Label();
				ResetOpPanel(apptModule,op,prov,panelHeight,location);
				_backPanel.Controls.Add(_labelOpName);
			}

			public OpPanel(ContrAppt apptModule,int panelHeight,Point location,int dayIdx) {
				_backPanel=new Panel();
				_labelOpName=new Label();
				ResetOpPanelWeekly(apptModule,panelHeight,location,dayIdx);
				_backPanel.Controls.Add(_labelOpName);
			}

			public OpPanel(ContrAppt apptModule,Provider prov,int panelHeight,Point location,bool isFirst) {
				_backPanel=new Panel();
				_labelOpName=new Label();
				ResetProvPanel(apptModule,prov,panelHeight,location,isFirst);
				_backPanel.Controls.Add(_labelOpName);
			}

			public void ResetOpPanel(ContrAppt apptModule,Operatory op,Provider prov,int panelHeight,Point location) {
				_labelOpName.Text=op.OpName;
				if(op.ProvDentist!=0 && !op.IsHygiene) {
					_backPanel.BackColor=Providers.GetColor(op.ProvDentist);
				}
				else if(op.ProvHygienist!=0 && op.IsHygiene) {
					_backPanel.BackColor=Providers.GetColor(op.ProvHygienist);
				}
				else {
					_backPanel.BackColor=SystemColors.Control;
				}
				Tuple<Provider,Operatory> tupleProvOp=new Tuple<Provider,Operatory>(prov,op);
				_backPanel.Location=location;
				_backPanel.Width=(int)ApptDrawing.ColWidth;
				_backPanel.Height=panelHeight;
				_backPanel.BorderStyle=BorderStyle.Fixed3D;
				_backPanel.ForeColor=Color.DarkGray;
				_backPanel.MouseDown-=new MouseEventHandler(apptModule._backPanel_MouseDown);
				_backPanel.MouseDown-=new MouseEventHandler(apptModule.OpPanelProv_MouseDown); //ensures that the same event doesn't get added twice.
				_backPanel.MouseDown+=new MouseEventHandler(apptModule.OpPanelProv_MouseDown);
				_backPanel.Tag=tupleProvOp;//store provider and operatory
				//add label within panOpName
				_labelOpName.Location=new Point(0,-2);//Hardcoded in the Panel
				_labelOpName.Width=_backPanel.Width;
				_labelOpName.Height=panelHeight;
				_labelOpName.TextAlign=ContentAlignment.MiddleCenter;
				_labelOpName.ForeColor=Color.Black;
				_labelOpName.MouseDown-=new MouseEventHandler(apptModule._backPanel_MouseDown);
				_labelOpName.MouseDown-=new MouseEventHandler(apptModule.OpPanelProv_MouseDown); //ensures that the same event doesn't get added twice.
				_labelOpName.MouseDown+=new MouseEventHandler(apptModule.OpPanelProv_MouseDown);
				_labelOpName.Tag=tupleProvOp;//store provider and operatory
				_opToolTip.SetToolTip(_labelOpName,_labelOpName.Text);//the _opName label is in the foreground.
			}

			public void ResetOpPanelWeekly(ContrAppt apptModule,int panelHeight,Point location,int dayIdx) {
				_labelOpName.Text=WeekStartDate.AddDays(dayIdx).ToString("dddd-d");
				_backPanel.BackColor=SystemColors.Control;
				_backPanel.Location=location;
				_backPanel.Width=(int)ApptDrawing.ColDayWidth;
				_backPanel.Height=18;
				_backPanel.BorderStyle=BorderStyle.Fixed3D;
				_backPanel.ForeColor=Color.DarkGray;
				_backPanel.MouseDown-=new MouseEventHandler(apptModule.OpPanelProv_MouseDown);
				_backPanel.MouseDown-=new MouseEventHandler(apptModule._backPanel_MouseDown); //ensures that the same event doesn't get added twice.
				_backPanel.MouseDown+=new MouseEventHandler(apptModule._backPanel_MouseDown);
				_backPanel.Tag=dayIdx;//stores the day index
				_labelOpName.Location=new Point(0,-2);//Hardcoded in the Panel
				_labelOpName.Width=_backPanel.Width;
				_labelOpName.Height=18;
				_labelOpName.TextAlign=ContentAlignment.MiddleCenter;
				_labelOpName.ForeColor=Color.Black;
				_labelOpName.MouseDown-=new MouseEventHandler(apptModule.OpPanelProv_MouseDown);
				_labelOpName.MouseDown-=new MouseEventHandler(apptModule._backPanel_MouseDown); //ensures that the same event doesn't get added twice.
				_labelOpName.MouseDown+=new MouseEventHandler(apptModule._backPanel_MouseDown);
				_labelOpName.Tag=dayIdx;//stores the day index
				_opToolTip.SetToolTip(_labelOpName,_labelOpName.Text);//the _opName label is in the foreground.
			}

			public void ResetProvPanel(ContrAppt apptModule,Provider prov,int panelHeight,Point location,bool isFirst) {
				_backPanel.BackColor=prov.ProvColor;
				_backPanel.Location=location;
				_backPanel.Width=(int)ApptDrawing.ProvWidth;
				if(isFirst) {//just looks a little nicer:
					_backPanel.Location=new Point(_backPanel.Location.X-1,_backPanel.Location.Y);
					_backPanel.Width=_backPanel.Width+1;
				}
				_backPanel.Height=panelHeight;
				_backPanel.BorderStyle=BorderStyle.Fixed3D;
				_backPanel.ForeColor=Color.DarkGray;
				_labelOpName.MouseDown-=new MouseEventHandler(apptModule._backPanel_MouseDown); //Otherwise clicking on the prov bar will change days
				_opToolTip.SetToolTip(_labelOpName,prov.Abbr);
			}

			public Panel GetPanel() {
				return _backPanel;
			}
		}

		private class MenuItemNames {
			public const string TextAsapList="Text Asap List";
		}


		//private void butTest_Click(object sender,EventArgs e) {
		//	timerTests.Enabled=!timerTests.Enabled;
		//}

		/*
		private void butStress_Click(object sender,EventArgs e) {
			timerStress.Enabled=true;
		}

		private void timerStress_Tick(object sender,EventArgs e) {
			
		}*/
	}
}