/*=============================================================================================================
Open Dental is a dental practice management program.
Copyright (C) 2003-2013  Jordan Sparks, DMD.  http://www.opendental.com

This program is free software; you can redistribute it and/or modify it under the terms of the
GNU Db Public License as published by the Free Software Foundation; either version 2 of the License,
or (at your option) any later version.

This program is distributed in the hope that it will be useful, but without any warranty. See the GNU Db Public License
for more details, available at http://www.opensource.org/licenses/gpl-license.php

Any changes to this program must follow the guidelines of the GPL license if a modified version is to be
redistributed.
===============================================================================================================*/


//#define ORA_DB
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Media;
using Microsoft.Win32;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Principal;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDentBusiness.UI;
using CodeBase;
using System.Security.AccessControl;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using SparksToothChart;
//using OpenDental.SmartCards;
using OpenDental.UI;
using System.ServiceProcess;
using System.Linq;
using OpenDental.Bridges;
using OpenDentBusiness.WebServiceMainHQ;
using ServiceManager;
using System.DirectoryServices;
#if EHRTEST
using EHR;
#endif

//#if(ORA_DB)
//using OD_CRYPTO;
//#endif

namespace OpenDental{
	///<summary></summary>
	public class FormOpenDental:ODForm {
		#region Classwide Variables
		///<summary>This is the singleton instance of the FormOpenDental. This allows us to have S_ methods that are public static
		///and can be called from anywhere in the program to update FormOpenDental.</summary>
		private static FormOpenDental _formOpenDentalS;

		private System.ComponentModel.IContainer components;
		//private bool[,] buttonDown=new bool[2,6];
		private System.Windows.Forms.Timer timerTimeIndic;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItemSettings;
		private System.Windows.Forms.MenuItem menuItemReportsHeader;
		private System.Windows.Forms.MenuItem menuItemPrinter;
		private System.Windows.Forms.MenuItem menuItemDataPath;
		private System.Windows.Forms.MenuItem menuItemConfig;
		private System.Windows.Forms.MenuItem menuItemAutoCodes;
		private System.Windows.Forms.MenuItem menuItemDefinitions;
		private System.Windows.Forms.MenuItem menuItemInsCats;
		private System.Windows.Forms.MenuItem menuItemLinks;
		private System.Windows.Forms.MenuItem menuItemRecall;
		private System.Windows.Forms.MenuItem menuItemEmployees;
		private System.Windows.Forms.MenuItem menuItemPractice;
		private System.Windows.Forms.MenuItem menuItemPrescriptions;
		private System.Windows.Forms.MenuItem menuItemProviders;
		private System.Windows.Forms.MenuItem menuItemProcCodes;
		private System.Windows.Forms.MenuItem menuItemPrintScreen;
		private System.Windows.Forms.MenuItem menuItemFinanceCharge;
		private System.Windows.Forms.MenuItem menuItemAging;
		private System.Windows.Forms.MenuItem menuItemSched;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItemTranslation;
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItemLists;
		private System.Windows.Forms.MenuItem menuItemTools;
		private System.Windows.Forms.MenuItem menuItemReferrals;
		private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.MenuItem menuItemDatabaseMaintenance;
		private System.Windows.Forms.MenuItem menuItemProcedureButtons;
		private System.Windows.Forms.MenuItem menuItemZipCodes;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuTelephone;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItemHelpIndex;
		private System.Windows.Forms.MenuItem menuItemClaimForms;
		private System.Windows.Forms.MenuItem menuItemContacts;
		private System.Windows.Forms.MenuItem menuItemMedications;
		private System.Windows.Forms.ImageList imageList32;
		private System.Windows.Forms.MenuItem menuItemApptViews;
		private System.Windows.Forms.MenuItem menuItemComputers;
		private System.Windows.Forms.MenuItem menuItemEmployers;
		private System.Windows.Forms.MenuItem menuItemEasy;
		private System.Windows.Forms.MenuItem menuItemCarriers;
		private System.Windows.Forms.MenuItem menuItemSchools;
		private System.Windows.Forms.MenuItem menuItemCounties;
		private System.Windows.Forms.MenuItem menuItemScreening;
		private System.Windows.Forms.MenuItem menuItemEmail;
		private System.Windows.Forms.MenuItem menuItemHelpContents;
		private System.Windows.Forms.MenuItem menuItemHelp;
		private System.Windows.Forms.MenuItem menuItemClearinghouses;
		private System.Windows.Forms.MenuItem menuItemUpdate;
		private System.Windows.Forms.MenuItem menuItemHelpWindows;
		private System.Windows.Forms.MenuItem menuItemMisc;
		private System.Windows.Forms.MenuItem menuItemRemote;
		private System.Windows.Forms.MenuItem menuItemSchoolClass;
		private System.Windows.Forms.MenuItem menuItemSchoolCourses;
		private System.Windows.Forms.MenuItem menuItemSecurity;
		private System.Windows.Forms.MenuItem menuItemLogOff;
		private System.Windows.Forms.MenuItem menuItemInsPlans;
		private System.Windows.Forms.MenuItem menuItemClinics;
		private System.Windows.Forms.MenuItem menuItemOperatories;
		private System.Windows.Forms.Timer timerSignals;
		///<summary>When user logs out, this keeps track of where they were for when they log back in.</summary>
		private int LastModule;
		private System.Windows.Forms.MenuItem menuItemRepeatingCharges;
		private MenuItem menuItemTimeCards;
		private MenuItem menuItemApptRules;
		private MenuItem menuItemAuditTrail;
		private MenuItem menuItemPatFieldDefs;
		private MenuItem menuItemTerminal;
		private MenuItem menuItemTerminalManager;
		private MenuItem menuItemQuestions;
		private MenuItem menuItemCustomReports;
		private MenuItem menuItemMessaging;
		private OpenDental.UI.LightSignalGrid lightSignalGrid1;
		private MenuItem menuItemMessagingButs;
		private FormSplash Splash;
		private Bitmap bitmapIcon;
		private MenuItem menuItemCreateAtoZFolders;
		private MenuItem menuItemLaboratories;
		///<summary>A list of button definitions for this computer.</summary>
		private SigButDef[] SigButDefList;
		/// <summary>Added these 3 fields for Oracle encrypted connection string</summary>
		private string connStr;
		private string key;
		private MenuItem menuItemGraphics;
		private MenuItem menuItemLabCases;
		private MenuItem menuItemRequirementsNeeded;
		private MenuItem menuItemReqStudents;
		private MenuItem menuItemAutoNotes;
		private MenuItem menuItemDisplayFields;
		private Panel panelSplitter;
		//private string dconnStr;
		private bool MouseIsDownOnSplitter;
		private Point SplitterOriginalLocation;
		private ContextMenu menuSplitter;
		private MenuItem menuItemDockBottom;
		private MenuItem menuItemDockRight;
		private ImageList imageListMain;
		private ContextMenu menuPatient;
		private ContextMenu menuLabel;
		private ContextMenu menuEmail;
		private ContextMenu menuLetter;
		private Point OriginalMousePos;
		private MenuItem menuItemCustomerManage;
		private System.Windows.Forms.Timer timerDisabledKey;
		///<summary>This list will only contain events for this computer where the users clicked to disable a popup for a specified period of time.  So it won't typically have many items in it.</summary>
		private List<PopupEvent> PopupEventList;
		private MenuItem menuItemPharmacies;
		private MenuItem menuItemSheets;
		private MenuItem menuItemRequestFeatures;
		private MenuItem menuItemModules;
		private MenuItem menuItemRecallTypes;
		private MenuItem menuItemFeeScheds;
		private MenuItem menuItemMobileSetup;
		private MenuItem menuItemLetters;
		//private UserControlPhonePanel phonePanel;
		///<summary>Command line args passed in when program starts.</summary>
		public string[] CommandLineArgs;
		///<summary>True if there is already a different instance of OD running.  This prevents attempting to start the listener.</summary>
		public bool IsSecondInstance;
		private UserControlTasks userControlTasks1;
		private ContrAppt ContrAppt2;
		private ContrFamily ContrFamily2;
		private ContrFamilyEcw ContrFamily2Ecw;
		private ContrAccount ContrAccount2;
		private ContrTreat ContrTreat2;
		private ContrChart ContrChart2;
		private ContrImages ContrImages2;
		private ContrStaff ContrManage2;
		private OutlookBar myOutlookBar;
		private MenuItem menuItemShutdown;
		private MenuItem menuItemInsFilingCodes;
		private MenuItem menuItemReplication;
		private MenuItem menuItemAutomation;
		private MenuItem menuItemMergePatients;
		private MenuItem menuItemDuplicateBlockouts;
		private OpenDental.UI.ODToolBar ToolBarMain;
		private MenuItem menuItem3;
		private MenuItem menuApptFieldDefs;
		private MenuItem menuItemWebForms;
		private FormTerminalManager formTerminalManager;
		private FormPhoneTiles formPhoneTiles;
		private System.Windows.Forms.Timer timerWebHostSynch;
		private MenuItem menuItemCCRecurring;
		private UserControlPhoneSmall phoneSmall;
		/////<summary>This will be null if EHR didn't load up.  EHRTEST conditional compilation constant is used because the EHR project is only part of the solution here at HQ.  We need to use late binding in a few places so that it will still compile for people who download our sourcecode.  But late binding prevents us from stepping through for debugging, so the EHRTEST lets us switch to early binding.</summary>
		//public static object ObjFormEhrMeasures;
		private MenuItem menuItemEHR;
		private System.Windows.Forms.Timer timerLogoff;
		//<summary>This will be null if EHR didn't load up.</summary>
		//public static Assembly AssemblyEHR;
		private Form FormRecentlyOpenForLogoff;
		private MenuItem menuItemPayerIDs;
		private MenuItem menuItemTestLatency;
		private FormLogOn FormLogOn_;
		private System.Windows.Forms.Timer timerReplicationMonitor;
		///<summary>When auto log off is in use, we don't want to log off user if they are in the FormLogOn window.  Mostly a problem when using web service because CurUser is not null.</summary>
		private bool IsFormLogOnLastActive;
		private Panel panelPhoneSmall;
		private UI.Button butTriage;
		private UI.Button butBigPhones;
		private Label labelWaitTime;
		private Label labelTriage;
		private Label labelMsg;
		///<summary>This thread fills labelMsg</summary>
		private Thread ThreadVM;
		private ODThread _odThreadHqMetrics;
		private MenuItem menuItemWiki;
		private MenuItem menuItemProcLockTool;
		private MenuItem menuItemHL7;
		private MenuItem menuItemNewCropBilling;
		private MenuItem menuItemSpellCheck;
		private MenuItem menuItemResellers;
		private MenuItem menuItemXChargeReconcile;
		private FormCreditRecurringCharges FormCRC;
		private UI.Button butMapPhones;
		private ComboBox comboTriageCoordinator;
		private Label labelFieldType;
		///<summary>If the local computer is the computer where Podium invitations are sent, then this thread runs in the background and checks for 
		///appointments that started 10-40 minutes ago (depending on in the patient is a new patient) at 10 minute intervals.  No preferences.
		///In the future, some sort of identification should be made to tell if this thread is running on any computer.</summary>
		private ODThread _threadPodium;
		///<summary>If the local computer is the computer where claim reports are retrieved then this thread runs in the background and will retrieve
		///and import reports for the default clearinghouse or for clearinghouses where both the Payors field is not empty plus the Eformat matches the
		///region the user is in.  If an error is returned from the importation, this thread will silently fail.</summary>
		private ODThread _threadClaimReportRetrieve;
		private TimeSpan _timeEmailInboxesLastChecked=TimeSpan.Zero;
				/// <summary>If OpenDental is running on the same machine as the mysql server, then a thread is runs in the background to update the local machine's time
		///using NTPv4 from the NIST time server set in the NistTimeServerUrl pref./// </summary>
		private Thread _threadTimeSynch;
		private bool _isTimeSynchThreadRunning=false;
		private MenuItem menuItemAppointments;
		private MenuItem menuItem8;
		private MenuItem menuItemPreferencesAppts;
		private MenuItem menuItemFamily;
		private MenuItem menuItem10;
		private MenuItem menuItemPreferencesFamily;
		private MenuItem menuItemChart;
		private MenuItem menuItem13;
		private MenuItem menuItemPreferencesChart;
		private MenuItem menuItemImages;
		private MenuItem menuItemManage;
		private MenuItem menuItem17;
		private MenuItem menuItemPreferencesManage;
		private MenuItem menuItem19;
		private MenuItem menuItem20;
		private MenuItem menuItemObsolete;
		private MenuItem menuItemAdvancedSetup;
		private MenuItem menuItemAccount;
		private MenuItem menuItemPreferencesTreatPlan;
		private MenuItem menuItemPreferencesImages;
		private MenuItem menuItem2;
		private MenuItem menuItemImagingPerComp;
		private MenuItem menuItemAllergies;
		private MenuItem menuItemProblems;
		private MenuItem menuItemDentalSchools;
		private MenuItem menuItemEvaluations;
		private AutoResetEvent _timeSynchSleep=new AutoResetEvent(false);
		///<summary>Used to stop threads from running during updates.  Currently only used to stop Mobile Synch.</summary>
		private bool _isStartingUp;
		private long _previousPatNum;
		private MenuItem menuItemDispensary;
		private MenuItem menuItemApptTypes;
		private DateTime _datePopupDelay;
		///<summary>A secondary cache only used to determine if preferences related to the redrawing of the Chart module have been changed.</summary>
		private Dictionary<string,object> dictChartPrefsCache=new Dictionary<string,object>();
		///<summary>A secondary cache only used to determine if preferences related to the redrawing of the non-modal task list have been changed.</summary>
		private Dictionary<string,object> dictTaskListPrefsCache=new Dictionary<string,object>();
		//(Deprecated) Moved to Clinics.ClinicNum
		//public static long ClinicNum=0;
		///<summary>This is used to determine how Open Dental closed.  If this is set to anything but 0 then some kind of error occurred and Open Dental was forced to close.  Currently only used when updating Open Dental silently.</summary>
		public static int ExitCode=0;
		///<summary>Will be set to true if the STOP SLAVE SQL was run on the replication server for which the local replication monitor is watching.
		///Replicaiton is NOT broken when this flag is true, because the user can re-enable replicaiton using the START SLAVE SQL without any ill effects.
		///This flag is used to display a warning to the user, but will not ever block the user from using OD.</summary>
		private bool _isReplicationSlaveStopped=false;
		private MenuItem menuClinics;
		private MenuItem menuItemEServices;
		private MenuItem menuItemPatientPortal;
		private MenuItem menuMobileWeb;
		private MenuItem menuItemIntegratedTexting;
		private MenuItem menuItemWebSched;
		private MenuItem menuItemERemind;
		private MenuItem menuItemEConfirm;
		private MenuItem menuItemEMisc;
		private MenuItem menuItem14;
		private MenuItem menuItemListenerService;
		///<summary>HQ only. Keep track of last time triage task labels were filled. Too taxing on the server to perform every 1.6 seconds with the rest of the HQ thread metrics. Triage labels will be refreshed on ProcessSigsIntervalInSecs interval.</summary>
		DateTime _hqTriageMetricsLastRefreshed=DateTime.MinValue;
		///<summary>HQ only. Keep track of last time EServiceMetrics were filled. Server is only updating every 30 seconds so no need to go any faster than that.</summary>
		DateTime _hqEServiceMetricsLastRefreshed=DateTime.MinValue;
		private ODThread _odThreadEServices;
		private ODThread _odThreadFillCache;
		private ContextMenu menuText;
		private MenuItem menuItemTextMessagesReceived;
		private MenuItem menuItemTextMessagesSent;
		private MenuItem menuItemTextMessagesAll;
		private MenuItem menuItemRemoteSupport;
		///<summary>A specific reference to the "Text" button.  This special reference helps us preserve the notification text on the button after setup is modified.</summary>
		private ODToolBarButton _butText;
		///<summary>A specific reference to the "Task" button.
		///This special reference helps us refresh the notification text on the button after the user changes.</summary>
		private ODToolBarButton _butTask;
		private MenuItem menuItemMoveSubscribers;
		private string _showForm="";
		private MenuItem menuItemPreferencesAccount;
		private MenuItem menuItemCCProcs;
		private MenuItem menuItem12;
		private MenuItem menuItemRequiredFields;
		private MenuItem menuItemJobManager;
		private MenuItem menuItemStateAbbrs;
		private MenuItem menuItemMergeReferrals;
		private MenuItem menuItemMergeProviders;
		private MenuItem menuItemMergeMedications;
		private MenuItem menuItem4;
		private MenuItem menuItemPassword;
		private MenuItem menuItemEmailSettings;
    private MenuItem menuItemInsVerification;
    private FormSmsTextMessaging _formSmsTextMessaging;
		private FormQuery _formUserQuery;
		private OpenDentalGraph.FormDashboardEditTab _formDashboardEditTab;
		private MenuItem menuItemReportsStandard;
		private MenuItem menuItemReportsGraphic;
		private MenuItem menuItemReportsUserQuery;
		private MenuItem menuItemPendingPayments;
		private MenuItem menuItemAlerts;
		private MenuItem menuItemNoAlerts;
		private MenuItem menuItemAutoClosePayPlans;
		///<summary>HQ only. Multiple phone maps can be opened at the same time. This keeps a list of all that are open so we can modify their contents.</summary>
		private static List<FormMapHQ> _listMaps=new List<FormMapHQ>();
		private MenuItem menuItemServiceManager;
		private System.Windows.Forms.Timer timerUpdateTime;
		private static Dictionary<long,Dictionary<long,DateTime>> _dicBlockedAutomations;
		///<summary>Dictionary of AutomationNums mapped to a dictionary of patNums and dateTimes. 
		///<para>The dateTime is the time that the given automation for a specific patient should be blocked until.</para>
		///<para>Dictionary removes any entries whos blocked until dateTime is greater than DateTime.Now before returning.</para>
		///<para>Currently only used when triggered Automation.AutoAction == AutomationAction.PopUp</para></summary>
		public static Dictionary<long,Dictionary<long,DateTime>> DicBlockedAutomations {
			get {
				if(_dicBlockedAutomations==null){
					_dicBlockedAutomations=new Dictionary<long,Dictionary<long,DateTime>>();
					return _dicBlockedAutomations;
				}
				List<long> listAutoNums=_dicBlockedAutomations.Keys.ToList();
				List<long> listPatNums;
				foreach(long automationNum in listAutoNums) {//Key is an AutomationNum
					listPatNums=_dicBlockedAutomations[automationNum].Keys.ToList();
					foreach(long patNum in listPatNums) {//Key is a patNum for current AutomationNum key.
						if(_dicBlockedAutomations[automationNum][patNum]>DateTime.Now) {//Disable time has not expired yet.
							continue;
						}
						_dicBlockedAutomations[automationNum].Remove(patNum);//Remove automation for current user since block time has expired.
						//Since we removed an entry from the lower level dictionary we need to check if there are still entries in the top level dictionary. 
					}
					if(_dicBlockedAutomations[automationNum].Count()==0) {//Top level dictionary no longer contains entries for current automationNum.
						_dicBlockedAutomations.Remove(automationNum);
					}
				}
				return _dicBlockedAutomations;
			}
		}
		private MenuItem menuItemFHIR;
		private FormJobManager2 _formJobManager2; //singleton
		private ContextMenu menuTask;
		private MenuItem menuItemTaskNewForUser;
		private MenuItem menuItemTaskReminders;
		///<summary>Tracks the reminder tasks for the currently logged in user.  Is null until the first signal refresh.  
		///Includes new and viewed tasks.</summary>
		private List<Task> _listReminderTasks=null;
		///<summary>Tracks the normal (non-reminder) tasks for the currently logged in user.  Is null until the first signal refresh.</summary>
		private List<long> _listNormalTaskNums=null;
		///<summary>Tracks the UserNum of the user for which the _listReminderTaskNums and _listOtherTaskNums belong to
		///so we can compensate for different users logging off/on.</summary>
		private long _tasksUserNum=0;
		///<summary>The date the appointment module reminders tab was last refreshed.</summary>
		private DateTime _dateReminderRefresh=DateTime.MinValue;
		private MenuItem menuItemXWebTrans;
		///<summary>HQ only. Keep track of the last time the office down was checked. Too taxing on the server to perform every 1.6 seconds with the rest 
		///of the HQ thread metrics. Will be refreshed on ProcessSigsIntervalInSecs interval.</summary>
		private DateTime _hqOfficeDownLastRefreshed=DateTime.MinValue;
		///<summary>List of AlerReads for the current User.</summary>
		List<AlertRead> _listAlertReads=new List<AlertRead>();
		///<summary>List of AlertItems for the current user and clinic.</summary>
		List<AlertItem> _listAlertItems=new List<AlertItem>();
		private MenuItem menuItemTasks;
		private FormXWebTransactions FormXWT;
		private MenuItem menuItemAbout;
		private MenuItem menuItem11;
		private UI.Button butVoiceMails;
		private MenuItem menuItemOrtho;
		private static bool _isTreatPlanSortByTooth;
		private MenuItem menuItemAutoOrtho;
		private FormVoiceMails _formVM;
		///<summary>In most cases, CurPatNum should be used instead of _curPatNum.</summary>
		private static long _curPatNum;
		private MenuItem menuItemDiscountPlans;
		private ContextMenu menuCommlog;
		private MenuItem menuItemCommlogPersistent;
		private MenuItem menuItemConfirmations;
		private MenuItem menuItemSetupWizard;
		private MenuItem menuItemSignup;
		private MenuItem menuItemMergeDPs;
		private MenuItem menuItemAsapList;
		private MenuItem menuItemReports;
		private MenuItem menuItemAlertCategory;
		private MenuItem menuItemQuickPasteNotes;
		private MenuItem menuItemWebForm;
		private MenuItem menuItemReportsUnfinalizedPay;
		private MenuItem menuItemDatabaseMaintenancePat;
		private MenuItem menuItemUserSettings;
		private MenuItem menuItemAddUser;
		private MenuItem menuItemSecuritySettings;
		private MenuItem menuItemWebinar;
		private FormConnectionLost _formConnectionLost=null;
		private FormLoginFailed _formLoginFailed=null;
		#endregion Classwide Variables

		///<summary>Represents if the regkey is a developer regkey.</summary>
		public static bool RegKeyIsForTesting;
		///<summary>PatNum for currently loaded patient.</summary>
		public static long CurPatNum {
			get {
				return _curPatNum;
			}
			set {
				if(value==_curPatNum) {
					return;
				}
				_curPatNum=value;
				PatientChangedEvent.Fire(new ODEventArgs("FormOpenDental",value));
			}
		}
		public static PhoneTile PhoneTile {
			get {
				return _formOpenDentalS.phoneSmall.PhoneTile;
			}
		}
		
		///<summary>Inherits value from PrefName.TreatPlanSortByTooth on startup.  
		///The user can change this value without changing the pref from the treatplan module.</summary>
		public static bool IsTreatPlanSortByTooth {
			get {
				return _isTreatPlanSortByTooth;
			}
			set {
				_isTreatPlanSortByTooth=value;
				PrefC.IsTreatPlanSortByTooth=value;
			}
		}

		///<summary>Do not respond to F1 key with help.</summary>
		protected override bool HasHelpKey {
			get {
				return false;
			}
		}

		///<summary></summary>
		public FormOpenDental(string[] cla){
			//The default SecurityProtocol is "Ssl3|Tls".  We must add Tls12 in order to support Tls1.2 web reference handshakes, 
			//without breaking any web references using Ssl3 or Tls.
			ServicePointManager.SecurityProtocol|=SecurityProtocolType.Tls12;
			_formOpenDentalS=this;
			Logger.openlog.Log("Initializing Open Dental...",Logger.Severity.INFO);
			CommandLineArgs=cla;
			ODProgressWindow splashWindow=null;
			if(CommandLineArgs.Length==0) {
				splashWindow=ODProgressOld.ShowProgressSplash("SplashScreenProgress",this);
			}
			InitializeComponent();
			SystemEvents.SessionSwitch+=new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
			//toolbar		
			ToolBarMain=new ODToolBar();
			UpdateSplashProgress("Loading toolbar",5);
			ToolBarMain.Location=new Point(51,0);
			ToolBarMain.Size=new Size(931,25);
			ToolBarMain.Dock=DockStyle.Top;
			ToolBarMain.ImageList=imageListMain;
			ToolBarMain.ButtonClick+=new ODToolBarButtonClickEventHandler(ToolBarMain_ButtonClick);
			this.Controls.Add(ToolBarMain);
			//outlook bar
			UpdateSplashProgress("Loading outlook bar",10);
			myOutlookBar=new OutlookBar();
			myOutlookBar.Location=new Point(0,0);
			myOutlookBar.Size=new Size(51,626);
			myOutlookBar.ImageList=imageList32;
			myOutlookBar.Dock=DockStyle.Left;
			myOutlookBar.ButtonClicked+=new ButtonClickedEventHandler(myOutlookBar_ButtonClicked);
			this.Controls.Add(myOutlookBar);
			//MAIN MODULE CONTROLS
			//contrAppt
			UpdateSplashProgress("Initializing appointment module",15);
			ContrAppt2=new ContrAppt() { Visible=false };
			this.Controls.Add(ContrAppt2);
			//contrFamily
			UpdateSplashProgress("Initializing family module",20);
			ContrFamily2=new ContrFamily() { Visible=false };
			this.Controls.Add(ContrFamily2);
			//contrFamilyEcw
			UpdateSplashProgress("Initializing family ecw",25);
			ContrFamily2Ecw=new ContrFamilyEcw() { Visible=false };
			this.Controls.Add(ContrFamily2Ecw);
			//contrAccount
			UpdateSplashProgress("Initializing account module",30);
			ContrAccount2=new ContrAccount() { Visible=false };
			this.Controls.Add(ContrAccount2);
			//contrTreat
			UpdateSplashProgress("Initializing treatement module",35);
			ContrTreat2=new ContrTreat() { Visible=false };
			this.Controls.Add(ContrTreat2);
			//contrChart
			UpdateSplashProgress("Initializing chart module",40);
			ContrChart2=new ContrChart() { Visible=false };
			this.Controls.Add(ContrChart2);
			//contrImages
			UpdateSplashProgress("Initializing document module",70);
			ContrImages2=new ContrImages() { Visible=false };
			this.Controls.Add(ContrImages2);
			//contrManage
			UpdateSplashProgress("Initializing management module",80);
			ContrManage2=new ContrStaff() { Visible=false };
			this.Controls.Add(ContrManage2);
			UpdateSplashProgress("Loading tasks",90);
			userControlTasks1=new UserControlTasks() { Visible=false };
			this.Controls.Add(userControlTasks1);
			panelSplitter.ContextMenu=menuSplitter;
			menuItemDockBottom.Checked=true;
			UpdateSplashProgress("Loading phone panel",95);
			phoneSmall=new UserControlPhoneSmall();
			phoneSmall.GoToChanged += new System.EventHandler(this.phoneSmall_GoToChanged);
			//phoneSmall.Visible=false;
			//this.Controls.Add(phoneSmall);
			panelPhoneSmall.Controls.Add(phoneSmall);
			panelPhoneSmall.Visible=false;
			//phonePanel=new UserControlPhonePanel();
			//phonePanel.Visible=false;
			//this.Controls.Add(phonePanel);
			//phonePanel.GoToChanged += new System.EventHandler(this.phonePanel_GoToChanged);
			Logger.openlog.Log("Open Dental initialization complete.",Logger.Severity.INFO);
			//Plugins.HookAddCode(this,"FormOpenDental.Constructor_end");//Can't do this because no plugins loaded.
			UpdateSplashProgress("Initialization complete",100);
			splashWindow?.Close();
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
				if(FormCRC!=null) {
					FormCRC.Dispose();
				}
				}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOpenDental));
			this.menuItemSecuritySettings = new System.Windows.Forms.MenuItem();
			this.menuItemAddUser = new System.Windows.Forms.MenuItem();
			this.timerTimeIndic = new System.Windows.Forms.Timer(this.components);
			this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
			this.menuItemLogOff = new System.Windows.Forms.MenuItem();
			this.menuItemFile = new System.Windows.Forms.MenuItem();
			this.menuItemPassword = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItemEmailSettings = new System.Windows.Forms.MenuItem();
			this.menuItemUserSettings = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItemPrinter = new System.Windows.Forms.MenuItem();
			this.menuItemGraphics = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItemConfig = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
			this.menuItemSettings = new System.Windows.Forms.MenuItem();
			this.menuItemAppointments = new System.Windows.Forms.MenuItem();
			this.menuItemPreferencesAppts = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuApptFieldDefs = new System.Windows.Forms.MenuItem();
			this.menuItemApptRules = new System.Windows.Forms.MenuItem();
			this.menuItemApptTypes = new System.Windows.Forms.MenuItem();
			this.menuItemApptViews = new System.Windows.Forms.MenuItem();
			this.menuItemAsapList = new System.Windows.Forms.MenuItem();
			this.menuItemConfirmations = new System.Windows.Forms.MenuItem();
			this.menuItemInsVerification = new System.Windows.Forms.MenuItem();
			this.menuItemOperatories = new System.Windows.Forms.MenuItem();
			this.menuItemRecall = new System.Windows.Forms.MenuItem();
			this.menuItemRecallTypes = new System.Windows.Forms.MenuItem();
			this.menuItemFamily = new System.Windows.Forms.MenuItem();
			this.menuItemPreferencesFamily = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuItemClaimForms = new System.Windows.Forms.MenuItem();
			this.menuItemClearinghouses = new System.Windows.Forms.MenuItem();
			this.menuItemInsCats = new System.Windows.Forms.MenuItem();
			this.menuItemInsFilingCodes = new System.Windows.Forms.MenuItem();
			this.menuItemPatFieldDefs = new System.Windows.Forms.MenuItem();
			this.menuItemPayerIDs = new System.Windows.Forms.MenuItem();
			this.menuItemAccount = new System.Windows.Forms.MenuItem();
			this.menuItemPreferencesAccount = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.menuItemCCProcs = new System.Windows.Forms.MenuItem();
			this.menuItemPreferencesTreatPlan = new System.Windows.Forms.MenuItem();
			this.menuItemChart = new System.Windows.Forms.MenuItem();
			this.menuItemPreferencesChart = new System.Windows.Forms.MenuItem();
			this.menuItem13 = new System.Windows.Forms.MenuItem();
			this.menuItemEHR = new System.Windows.Forms.MenuItem();
			this.menuItemProcedureButtons = new System.Windows.Forms.MenuItem();
			this.menuItemImages = new System.Windows.Forms.MenuItem();
			this.menuItemPreferencesImages = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItemImagingPerComp = new System.Windows.Forms.MenuItem();
			this.menuItemManage = new System.Windows.Forms.MenuItem();
			this.menuItemPreferencesManage = new System.Windows.Forms.MenuItem();
			this.menuItem17 = new System.Windows.Forms.MenuItem();
			this.menuItemEmail = new System.Windows.Forms.MenuItem();
			this.menuItemMessaging = new System.Windows.Forms.MenuItem();
			this.menuItemMessagingButs = new System.Windows.Forms.MenuItem();
			this.menuItemTimeCards = new System.Windows.Forms.MenuItem();
			this.menuItem19 = new System.Windows.Forms.MenuItem();
			this.menuItemAdvancedSetup = new System.Windows.Forms.MenuItem();
			this.menuItemComputers = new System.Windows.Forms.MenuItem();
			this.menuItemFHIR = new System.Windows.Forms.MenuItem();
			this.menuItemHL7 = new System.Windows.Forms.MenuItem();
			this.menuItemReplication = new System.Windows.Forms.MenuItem();
			this.menuItemEasy = new System.Windows.Forms.MenuItem();
			this.menuItemAlertCategory = new System.Windows.Forms.MenuItem();
			this.menuItemAutoCodes = new System.Windows.Forms.MenuItem();
			this.menuItemAutomation = new System.Windows.Forms.MenuItem();
			this.menuItemAutoNotes = new System.Windows.Forms.MenuItem();
			this.menuItemDataPath = new System.Windows.Forms.MenuItem();
			this.menuItemDefinitions = new System.Windows.Forms.MenuItem();
			this.menuItemDentalSchools = new System.Windows.Forms.MenuItem();
			this.menuItemDisplayFields = new System.Windows.Forms.MenuItem();
			this.menuItemFeeScheds = new System.Windows.Forms.MenuItem();
			this.menuItemLaboratories = new System.Windows.Forms.MenuItem();
			this.menuItemMisc = new System.Windows.Forms.MenuItem();
			this.menuItemModules = new System.Windows.Forms.MenuItem();
			this.menuItemOrtho = new System.Windows.Forms.MenuItem();
			this.menuItemPractice = new System.Windows.Forms.MenuItem();
			this.menuItemLinks = new System.Windows.Forms.MenuItem();
			this.menuItemQuickPasteNotes = new System.Windows.Forms.MenuItem();
			this.menuItemReports = new System.Windows.Forms.MenuItem();
			this.menuItemRequiredFields = new System.Windows.Forms.MenuItem();
			this.menuItemRequirementsNeeded = new System.Windows.Forms.MenuItem();
			this.menuItemSched = new System.Windows.Forms.MenuItem();
			this.menuItemSecurity = new System.Windows.Forms.MenuItem();
			this.menuItemSheets = new System.Windows.Forms.MenuItem();
			this.menuItemSpellCheck = new System.Windows.Forms.MenuItem();
			this.menuItemTasks = new System.Windows.Forms.MenuItem();
			this.menuItemWebForm = new System.Windows.Forms.MenuItem();
			this.menuItem20 = new System.Windows.Forms.MenuItem();
			this.menuItemObsolete = new System.Windows.Forms.MenuItem();
			this.menuItemLetters = new System.Windows.Forms.MenuItem();
			this.menuItemQuestions = new System.Windows.Forms.MenuItem();
			this.menuItemLists = new System.Windows.Forms.MenuItem();
			this.menuItemProcCodes = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItemAllergies = new System.Windows.Forms.MenuItem();
			this.menuItemClinics = new System.Windows.Forms.MenuItem();
			this.menuItemContacts = new System.Windows.Forms.MenuItem();
			this.menuItemCounties = new System.Windows.Forms.MenuItem();
			this.menuItemSchoolClass = new System.Windows.Forms.MenuItem();
			this.menuItemSchoolCourses = new System.Windows.Forms.MenuItem();
			this.menuItemDiscountPlans = new System.Windows.Forms.MenuItem();
			this.menuItemEmployees = new System.Windows.Forms.MenuItem();
			this.menuItemEmployers = new System.Windows.Forms.MenuItem();
			this.menuItemCarriers = new System.Windows.Forms.MenuItem();
			this.menuItemInsPlans = new System.Windows.Forms.MenuItem();
			this.menuItemLabCases = new System.Windows.Forms.MenuItem();
			this.menuItemMedications = new System.Windows.Forms.MenuItem();
			this.menuItemPharmacies = new System.Windows.Forms.MenuItem();
			this.menuItemProblems = new System.Windows.Forms.MenuItem();
			this.menuItemProviders = new System.Windows.Forms.MenuItem();
			this.menuItemPrescriptions = new System.Windows.Forms.MenuItem();
			this.menuItemReferrals = new System.Windows.Forms.MenuItem();
			this.menuItemSchools = new System.Windows.Forms.MenuItem();
			this.menuItemStateAbbrs = new System.Windows.Forms.MenuItem();
			this.menuItemZipCodes = new System.Windows.Forms.MenuItem();
			this.menuItemReportsHeader = new System.Windows.Forms.MenuItem();
			this.menuItemReportsStandard = new System.Windows.Forms.MenuItem();
			this.menuItemReportsGraphic = new System.Windows.Forms.MenuItem();
			this.menuItemReportsUserQuery = new System.Windows.Forms.MenuItem();
			this.menuItemReportsUnfinalizedPay = new System.Windows.Forms.MenuItem();
			this.menuItemCustomReports = new System.Windows.Forms.MenuItem();
			this.menuItemTools = new System.Windows.Forms.MenuItem();
			this.menuItemJobManager = new System.Windows.Forms.MenuItem();
			this.menuItemPrintScreen = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItemAutoClosePayPlans = new System.Windows.Forms.MenuItem();
			this.menuItemDuplicateBlockouts = new System.Windows.Forms.MenuItem();
			this.menuItemCreateAtoZFolders = new System.Windows.Forms.MenuItem();
			this.menuItemDatabaseMaintenancePat = new System.Windows.Forms.MenuItem();
			this.menuItemMergeDPs = new System.Windows.Forms.MenuItem();
			this.menuItemMergeMedications = new System.Windows.Forms.MenuItem();
			this.menuItemMergePatients = new System.Windows.Forms.MenuItem();
			this.menuItemMergeProviders = new System.Windows.Forms.MenuItem();
			this.menuItemMergeReferrals = new System.Windows.Forms.MenuItem();
			this.menuItemMoveSubscribers = new System.Windows.Forms.MenuItem();
			this.menuItemProcLockTool = new System.Windows.Forms.MenuItem();
			this.menuItemServiceManager = new System.Windows.Forms.MenuItem();
			this.menuItemShutdown = new System.Windows.Forms.MenuItem();
			this.menuTelephone = new System.Windows.Forms.MenuItem();
			this.menuItemTestLatency = new System.Windows.Forms.MenuItem();
			this.menuItemXChargeReconcile = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItemAging = new System.Windows.Forms.MenuItem();
			this.menuItemAuditTrail = new System.Windows.Forms.MenuItem();
			this.menuItemAutoOrtho = new System.Windows.Forms.MenuItem();
			this.menuItemFinanceCharge = new System.Windows.Forms.MenuItem();
			this.menuItemCCRecurring = new System.Windows.Forms.MenuItem();
			this.menuItemCustomerManage = new System.Windows.Forms.MenuItem();
			this.menuItemDatabaseMaintenance = new System.Windows.Forms.MenuItem();
			this.menuItemDispensary = new System.Windows.Forms.MenuItem();
			this.menuItemEvaluations = new System.Windows.Forms.MenuItem();
			this.menuItemTerminal = new System.Windows.Forms.MenuItem();
			this.menuItemTerminalManager = new System.Windows.Forms.MenuItem();
			this.menuItemTranslation = new System.Windows.Forms.MenuItem();
			this.menuItemMobileSetup = new System.Windows.Forms.MenuItem();
			this.menuItemNewCropBilling = new System.Windows.Forms.MenuItem();
			this.menuItemPendingPayments = new System.Windows.Forms.MenuItem();
			this.menuItemScreening = new System.Windows.Forms.MenuItem();
			this.menuItemRepeatingCharges = new System.Windows.Forms.MenuItem();
			this.menuItemResellers = new System.Windows.Forms.MenuItem();
			this.menuItemSetupWizard = new System.Windows.Forms.MenuItem();
			this.menuItemReqStudents = new System.Windows.Forms.MenuItem();
			this.menuItemWebForms = new System.Windows.Forms.MenuItem();
			this.menuItemWiki = new System.Windows.Forms.MenuItem();
			this.menuItemXWebTrans = new System.Windows.Forms.MenuItem();
			this.menuClinics = new System.Windows.Forms.MenuItem();
			this.menuItemEServices = new System.Windows.Forms.MenuItem();
			this.menuItemSignup = new System.Windows.Forms.MenuItem();
			this.menuMobileWeb = new System.Windows.Forms.MenuItem();
			this.menuItemPatientPortal = new System.Windows.Forms.MenuItem();
			this.menuItemWebSched = new System.Windows.Forms.MenuItem();
			this.menuItemIntegratedTexting = new System.Windows.Forms.MenuItem();
			this.menuItemERemind = new System.Windows.Forms.MenuItem();
			this.menuItemEConfirm = new System.Windows.Forms.MenuItem();
			this.menuItemEMisc = new System.Windows.Forms.MenuItem();
			this.menuItem14 = new System.Windows.Forms.MenuItem();
			this.menuItemListenerService = new System.Windows.Forms.MenuItem();
			this.menuItemAlerts = new System.Windows.Forms.MenuItem();
			this.menuItemNoAlerts = new System.Windows.Forms.MenuItem();
			this.menuItemHelp = new System.Windows.Forms.MenuItem();
			this.menuItemRemote = new System.Windows.Forms.MenuItem();
			this.menuItemHelpWindows = new System.Windows.Forms.MenuItem();
			this.menuItemHelpContents = new System.Windows.Forms.MenuItem();
			this.menuItemHelpIndex = new System.Windows.Forms.MenuItem();
			this.menuItemWebinar = new System.Windows.Forms.MenuItem();
			this.menuItemRemoteSupport = new System.Windows.Forms.MenuItem();
			this.menuItemRequestFeatures = new System.Windows.Forms.MenuItem();
			this.menuItemUpdate = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.menuItemAbout = new System.Windows.Forms.MenuItem();
			this.imageList32 = new System.Windows.Forms.ImageList(this.components);
			this.timerSignals = new System.Windows.Forms.Timer(this.components);
			this.panelSplitter = new System.Windows.Forms.Panel();
			this.menuSplitter = new System.Windows.Forms.ContextMenu();
			this.menuItemDockBottom = new System.Windows.Forms.MenuItem();
			this.menuItemDockRight = new System.Windows.Forms.MenuItem();
			this.imageListMain = new System.Windows.Forms.ImageList(this.components);
			this.menuPatient = new System.Windows.Forms.ContextMenu();
			this.menuLabel = new System.Windows.Forms.ContextMenu();
			this.menuEmail = new System.Windows.Forms.ContextMenu();
			this.menuLetter = new System.Windows.Forms.ContextMenu();
			this.timerDisabledKey = new System.Windows.Forms.Timer(this.components);
			this.timerWebHostSynch = new System.Windows.Forms.Timer(this.components);
			this.timerLogoff = new System.Windows.Forms.Timer(this.components);
			this.timerReplicationMonitor = new System.Windows.Forms.Timer(this.components);
			this.panelPhoneSmall = new System.Windows.Forms.Panel();
			this.butVoiceMails = new OpenDental.UI.Button();
			this.labelFieldType = new System.Windows.Forms.Label();
			this.comboTriageCoordinator = new System.Windows.Forms.ComboBox();
			this.labelMsg = new System.Windows.Forms.Label();
			this.butMapPhones = new OpenDental.UI.Button();
			this.butTriage = new OpenDental.UI.Button();
			this.butBigPhones = new OpenDental.UI.Button();
			this.labelWaitTime = new System.Windows.Forms.Label();
			this.labelTriage = new System.Windows.Forms.Label();
			this.menuText = new System.Windows.Forms.ContextMenu();
			this.menuItemTextMessagesAll = new System.Windows.Forms.MenuItem();
			this.menuItemTextMessagesReceived = new System.Windows.Forms.MenuItem();
			this.menuItemTextMessagesSent = new System.Windows.Forms.MenuItem();
			this.timerUpdateTime = new System.Windows.Forms.Timer(this.components);
			this.menuTask = new System.Windows.Forms.ContextMenu();
			this.menuItemTaskNewForUser = new System.Windows.Forms.MenuItem();
			this.menuItemTaskReminders = new System.Windows.Forms.MenuItem();
			this.menuCommlog = new System.Windows.Forms.ContextMenu();
			this.menuItemCommlogPersistent = new System.Windows.Forms.MenuItem();
			this.lightSignalGrid1 = new OpenDental.UI.LightSignalGrid();
			this.panelPhoneSmall.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuItemSecuritySettings
			// 
			this.menuItemSecuritySettings.Index = 0;
			this.menuItemSecuritySettings.Text = "Security Settings";
			this.menuItemSecuritySettings.Click += new System.EventHandler(this.menuItemSecuritySettings_Click);
			// 
			// menuItemAddUser
			// 
			this.menuItemAddUser.Index = 1;
			this.menuItemAddUser.Text = "Add User";
			this.menuItemAddUser.Click += new System.EventHandler(this.menuItemAddUser_Click);
			// 
			// timerTimeIndic
			// 
			this.timerTimeIndic.Interval = 60000;
			this.timerTimeIndic.Tick += new System.EventHandler(this.timerTimeIndic_Tick);
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemLogOff,
            this.menuItemFile,
            this.menuItemSettings,
            this.menuItemLists,
            this.menuItemReportsHeader,
            this.menuItemCustomReports,
            this.menuItemTools,
            this.menuClinics,
            this.menuItemEServices,
            this.menuItemAlerts,
            this.menuItemHelp});
			// 
			// menuItemLogOff
			// 
			this.menuItemLogOff.Index = 0;
			this.menuItemLogOff.Text = "Log &Off";
			this.menuItemLogOff.Click += new System.EventHandler(this.menuItemLogOff_Click);
			// 
			// menuItemFile
			// 
			this.menuItemFile.Index = 1;
			this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPassword,
            this.menuItem4,
            this.menuItem3,
            this.menuItemPrinter,
            this.menuItemGraphics,
            this.menuItem6,
            this.menuItemConfig,
            this.menuItem7,
            this.menuItemExit});
			this.menuItemFile.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.menuItemFile.Text = "&File";
			// 
			// menuItemPassword
			// 
			this.menuItemPassword.Index = 0;
			this.menuItemPassword.Text = "User Password";
			this.menuItemPassword.Click += new System.EventHandler(this.menuItemPassword_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemEmailSettings,
            this.menuItemUserSettings});
			this.menuItem4.Text = "User Settings";
			// 
			// menuItemEmailSettings
			// 
			this.menuItemEmailSettings.Index = 0;
			this.menuItemEmailSettings.Text = "Email Settings";
			this.menuItemEmailSettings.Click += new System.EventHandler(this.menuItemEmailSettings_Click);
			// 
			// menuItemUserSettings
			// 
			this.menuItemUserSettings.Index = 1;
			this.menuItemUserSettings.Text = "Other Settings";
			this.menuItemUserSettings.Click += new System.EventHandler(this.menuItemUserSettings_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 2;
			this.menuItem3.Text = "-";
			// 
			// menuItemPrinter
			// 
			this.menuItemPrinter.Index = 3;
			this.menuItemPrinter.Text = "&Printers";
			this.menuItemPrinter.Click += new System.EventHandler(this.menuItemPrinter_Click);
			// 
			// menuItemGraphics
			// 
			this.menuItemGraphics.Index = 4;
			this.menuItemGraphics.Text = "Graphics";
			this.menuItemGraphics.Click += new System.EventHandler(this.menuItemGraphics_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 5;
			this.menuItem6.Text = "-";
			// 
			// menuItemConfig
			// 
			this.menuItemConfig.Index = 6;
			this.menuItemConfig.Text = "&Choose Database";
			this.menuItemConfig.Click += new System.EventHandler(this.menuItemConfig_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 7;
			this.menuItem7.Text = "-";
			// 
			// menuItemExit
			// 
			this.menuItemExit.Index = 8;
			this.menuItemExit.ShowShortcut = false;
			this.menuItemExit.Text = "E&xit";
			this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
			// 
			// menuItemSettings
			// 
			this.menuItemSettings.Index = 2;
			this.menuItemSettings.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAppointments,
            this.menuItemFamily,
            this.menuItemAccount,
            this.menuItemPreferencesTreatPlan,
            this.menuItemChart,
            this.menuItemImages,
            this.menuItemManage,
            this.menuItem19,
            this.menuItemAdvancedSetup,
            this.menuItemAlertCategory,
            this.menuItemAutoCodes,
            this.menuItemAutomation,
            this.menuItemAutoNotes,
            this.menuItemDataPath,
            this.menuItemDefinitions,
            this.menuItemDentalSchools,
            this.menuItemDisplayFields,
            this.menuItemFeeScheds,
            this.menuItemLaboratories,
            this.menuItemMisc,
            this.menuItemModules,
            this.menuItemOrtho,
            this.menuItemPractice,
            this.menuItemLinks,
            this.menuItemQuickPasteNotes,
            this.menuItemReports,
            this.menuItemRequiredFields,
            this.menuItemRequirementsNeeded,
            this.menuItemSched,
            this.menuItemSecurity,
            this.menuItemSheets,
            this.menuItemSpellCheck,
            this.menuItemTasks,
            this.menuItemWebForm,
            this.menuItem20,
            this.menuItemObsolete});
			this.menuItemSettings.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.menuItemSettings.Text = "&Setup";
			// 
			// menuItemAppointments
			// 
			this.menuItemAppointments.Index = 0;
			this.menuItemAppointments.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPreferencesAppts,
            this.menuItem8,
            this.menuApptFieldDefs,
            this.menuItemApptRules,
            this.menuItemApptTypes,
            this.menuItemApptViews,
            this.menuItemAsapList,
            this.menuItemConfirmations,
            this.menuItemInsVerification,
            this.menuItemOperatories,
            this.menuItemRecall,
            this.menuItemRecallTypes});
			this.menuItemAppointments.Text = "Appointments";
			// 
			// menuItemPreferencesAppts
			// 
			this.menuItemPreferencesAppts.Index = 0;
			this.menuItemPreferencesAppts.Text = "Appts Preferences";
			this.menuItemPreferencesAppts.Click += new System.EventHandler(this.menuItemPreferencesAppts_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 1;
			this.menuItem8.Text = "-";
			// 
			// menuApptFieldDefs
			// 
			this.menuApptFieldDefs.Index = 2;
			this.menuApptFieldDefs.Text = "Appointment Field Defs";
			this.menuApptFieldDefs.Click += new System.EventHandler(this.menuItemApptFieldDefs_Click);
			// 
			// menuItemApptRules
			// 
			this.menuItemApptRules.Index = 3;
			this.menuItemApptRules.Text = "Appointment Rules";
			this.menuItemApptRules.Click += new System.EventHandler(this.menuItemApptRules_Click);
			// 
			// menuItemApptTypes
			// 
			this.menuItemApptTypes.Index = 4;
			this.menuItemApptTypes.Text = "Appointment Types";
			this.menuItemApptTypes.Click += new System.EventHandler(this.menuItemApptTypes_Click);
			// 
			// menuItemApptViews
			// 
			this.menuItemApptViews.Index = 5;
			this.menuItemApptViews.Text = "Appointment Views";
			this.menuItemApptViews.Click += new System.EventHandler(this.menuItemApptViews_Click);
			// 
			// menuItemAsapList
			// 
			this.menuItemAsapList.Index = 6;
			this.menuItemAsapList.Text = "ASAP List";
			this.menuItemAsapList.Click += new System.EventHandler(this.menuItemAsapList_Click);
			// 
			// menuItemConfirmations
			// 
			this.menuItemConfirmations.Index = 7;
			this.menuItemConfirmations.Text = "Confirmations";
			this.menuItemConfirmations.Click += new System.EventHandler(this.menuItemConfirmations_Click);
			// 
			// menuItemInsVerification
			// 
			this.menuItemInsVerification.Index = 8;
			this.menuItemInsVerification.Text = "Insurance Verification";
			this.menuItemInsVerification.Click += new System.EventHandler(this.menuItemInsVerify_Click);
			// 
			// menuItemOperatories
			// 
			this.menuItemOperatories.Index = 9;
			this.menuItemOperatories.Text = "Operatories";
			this.menuItemOperatories.Click += new System.EventHandler(this.menuItemOperatories_Click);
			// 
			// menuItemRecall
			// 
			this.menuItemRecall.Index = 10;
			this.menuItemRecall.Text = "Recall";
			this.menuItemRecall.Click += new System.EventHandler(this.menuItemRecall_Click);
			// 
			// menuItemRecallTypes
			// 
			this.menuItemRecallTypes.Index = 11;
			this.menuItemRecallTypes.Text = "Recall Types";
			this.menuItemRecallTypes.Click += new System.EventHandler(this.menuItemRecallTypes_Click);
			// 
			// menuItemFamily
			// 
			this.menuItemFamily.Index = 1;
			this.menuItemFamily.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPreferencesFamily,
            this.menuItem10,
            this.menuItemClaimForms,
            this.menuItemClearinghouses,
            this.menuItemInsCats,
            this.menuItemInsFilingCodes,
            this.menuItemPatFieldDefs,
            this.menuItemPayerIDs});
			this.menuItemFamily.Text = "Family / Insurance";
			// 
			// menuItemPreferencesFamily
			// 
			this.menuItemPreferencesFamily.Index = 0;
			this.menuItemPreferencesFamily.Text = "Family Preferences";
			this.menuItemPreferencesFamily.Click += new System.EventHandler(this.menuItemPreferencesFamily_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 1;
			this.menuItem10.Text = "-";
			// 
			// menuItemClaimForms
			// 
			this.menuItemClaimForms.Index = 2;
			this.menuItemClaimForms.Text = "Claim Forms";
			this.menuItemClaimForms.Click += new System.EventHandler(this.menuItemClaimForms_Click);
			// 
			// menuItemClearinghouses
			// 
			this.menuItemClearinghouses.Index = 3;
			this.menuItemClearinghouses.Text = "Clearinghouses";
			this.menuItemClearinghouses.Click += new System.EventHandler(this.menuItemClearinghouses_Click);
			// 
			// menuItemInsCats
			// 
			this.menuItemInsCats.Index = 4;
			this.menuItemInsCats.Text = "Insurance Categories";
			this.menuItemInsCats.Click += new System.EventHandler(this.menuItemInsCats_Click);
			// 
			// menuItemInsFilingCodes
			// 
			this.menuItemInsFilingCodes.Index = 5;
			this.menuItemInsFilingCodes.Text = "Insurance Filing Codes";
			this.menuItemInsFilingCodes.Click += new System.EventHandler(this.menuItemInsFilingCodes_Click);
			// 
			// menuItemPatFieldDefs
			// 
			this.menuItemPatFieldDefs.Index = 6;
			this.menuItemPatFieldDefs.Text = "Patient Field Defs";
			this.menuItemPatFieldDefs.Click += new System.EventHandler(this.menuItemPatFieldDefs_Click);
			// 
			// menuItemPayerIDs
			// 
			this.menuItemPayerIDs.Index = 7;
			this.menuItemPayerIDs.Text = "Payer IDs";
			this.menuItemPayerIDs.Click += new System.EventHandler(this.menuItemPayerIDs_Click);
			// 
			// menuItemAccount
			// 
			this.menuItemAccount.Index = 2;
			this.menuItemAccount.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPreferencesAccount,
            this.menuItem12,
            this.menuItemCCProcs});
			this.menuItemAccount.Text = "Account";
			this.menuItemAccount.Click += new System.EventHandler(this.menuItemPreferencesAccount_Click);
			// 
			// menuItemPreferencesAccount
			// 
			this.menuItemPreferencesAccount.Index = 0;
			this.menuItemPreferencesAccount.Text = "Account Preferences";
			this.menuItemPreferencesAccount.Click += new System.EventHandler(this.menuItemPreferencesAccount_Click);
			// 
			// menuItem12
			// 
			this.menuItem12.Index = 1;
			this.menuItem12.Text = "-";
			// 
			// menuItemCCProcs
			// 
			this.menuItemCCProcs.Index = 2;
			this.menuItemCCProcs.Text = "Default CC Procedures";
			this.menuItemCCProcs.Click += new System.EventHandler(this.menuItemDefaultCCProcs_Click);
			// 
			// menuItemPreferencesTreatPlan
			// 
			this.menuItemPreferencesTreatPlan.Index = 3;
			this.menuItemPreferencesTreatPlan.Text = "Treat\' Plan";
			this.menuItemPreferencesTreatPlan.Click += new System.EventHandler(this.menuItemPreferencesTreatPlan_Click);
			// 
			// menuItemChart
			// 
			this.menuItemChart.Index = 4;
			this.menuItemChart.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPreferencesChart,
            this.menuItem13,
            this.menuItemEHR,
            this.menuItemProcedureButtons});
			this.menuItemChart.Text = "Chart";
			// 
			// menuItemPreferencesChart
			// 
			this.menuItemPreferencesChart.Index = 0;
			this.menuItemPreferencesChart.Text = "Chart Preferences";
			this.menuItemPreferencesChart.Click += new System.EventHandler(this.menuItemPreferencesChart_Click);
			// 
			// menuItem13
			// 
			this.menuItem13.Index = 1;
			this.menuItem13.Text = "-";
			// 
			// menuItemEHR
			// 
			this.menuItemEHR.Index = 2;
			this.menuItemEHR.Text = "EHR";
			this.menuItemEHR.Click += new System.EventHandler(this.menuItemEHR_Click);
			// 
			// menuItemProcedureButtons
			// 
			this.menuItemProcedureButtons.Index = 3;
			this.menuItemProcedureButtons.Text = "Procedure Buttons";
			this.menuItemProcedureButtons.Click += new System.EventHandler(this.menuItemProcedureButtons_Click);
			// 
			// menuItemImages
			// 
			this.menuItemImages.Index = 5;
			this.menuItemImages.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPreferencesImages,
            this.menuItem2,
            this.menuItemImagingPerComp});
			this.menuItemImages.Text = "Images";
			// 
			// menuItemPreferencesImages
			// 
			this.menuItemPreferencesImages.Index = 0;
			this.menuItemPreferencesImages.Text = "Images Preferences";
			this.menuItemPreferencesImages.Click += new System.EventHandler(this.menuItemPreferencesImages_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "-";
			// 
			// menuItemImagingPerComp
			// 
			this.menuItemImagingPerComp.Index = 2;
			this.menuItemImagingPerComp.Text = "Imaging Quality";
			this.menuItemImagingPerComp.Click += new System.EventHandler(this.menuItemImaging_Click);
			// 
			// menuItemManage
			// 
			this.menuItemManage.Index = 6;
			this.menuItemManage.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPreferencesManage,
            this.menuItem17,
            this.menuItemEmail,
            this.menuItemMessaging,
            this.menuItemMessagingButs,
            this.menuItemTimeCards});
			this.menuItemManage.Text = "Manage";
			// 
			// menuItemPreferencesManage
			// 
			this.menuItemPreferencesManage.Index = 0;
			this.menuItemPreferencesManage.Text = "Manage Preferences";
			this.menuItemPreferencesManage.Click += new System.EventHandler(this.menuItemPreferencesManage_Click);
			// 
			// menuItem17
			// 
			this.menuItem17.Index = 1;
			this.menuItem17.Text = "-";
			// 
			// menuItemEmail
			// 
			this.menuItemEmail.Index = 2;
			this.menuItemEmail.Text = "E-mail";
			this.menuItemEmail.Click += new System.EventHandler(this.menuItemEmail_Click);
			// 
			// menuItemMessaging
			// 
			this.menuItemMessaging.Index = 3;
			this.menuItemMessaging.Text = "Messaging";
			this.menuItemMessaging.Click += new System.EventHandler(this.menuItemMessaging_Click);
			// 
			// menuItemMessagingButs
			// 
			this.menuItemMessagingButs.Index = 4;
			this.menuItemMessagingButs.Text = "Messaging Buttons";
			this.menuItemMessagingButs.Click += new System.EventHandler(this.menuItemMessagingButs_Click);
			// 
			// menuItemTimeCards
			// 
			this.menuItemTimeCards.Index = 5;
			this.menuItemTimeCards.Text = "Time Cards";
			this.menuItemTimeCards.Click += new System.EventHandler(this.menuItemTimeCards_Click);
			// 
			// menuItem19
			// 
			this.menuItem19.Index = 7;
			this.menuItem19.Text = "-";
			// 
			// menuItemAdvancedSetup
			// 
			this.menuItemAdvancedSetup.Index = 8;
			this.menuItemAdvancedSetup.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemComputers,
            this.menuItemFHIR,
            this.menuItemHL7,
            this.menuItemReplication,
            this.menuItemEasy});
			this.menuItemAdvancedSetup.Text = "Advanced Setup";
			// 
			// menuItemComputers
			// 
			this.menuItemComputers.Index = 0;
			this.menuItemComputers.Text = "Computers";
			this.menuItemComputers.Click += new System.EventHandler(this.menuItemComputers_Click);
			// 
			// menuItemFHIR
			// 
			this.menuItemFHIR.Index = 1;
			this.menuItemFHIR.Text = "FHIR";
			this.menuItemFHIR.Click += new System.EventHandler(this.menuItemFHIR_Click);
			// 
			// menuItemHL7
			// 
			this.menuItemHL7.Index = 2;
			this.menuItemHL7.Text = "HL7";
			this.menuItemHL7.Click += new System.EventHandler(this.menuItemHL7_Click);
			// 
			// menuItemReplication
			// 
			this.menuItemReplication.Index = 3;
			this.menuItemReplication.Text = "Replication";
			this.menuItemReplication.Click += new System.EventHandler(this.menuItemReplication_Click);
			// 
			// menuItemEasy
			// 
			this.menuItemEasy.Index = 4;
			this.menuItemEasy.Text = "Show Features";
			this.menuItemEasy.Click += new System.EventHandler(this.menuItemEasy_Click);
			// 
			// menuItemAlertCategory
			// 
			this.menuItemAlertCategory.Index = 9;
			this.menuItemAlertCategory.Text = "Alert Categories";
			this.menuItemAlertCategory.Click += new System.EventHandler(this.menuItemAlertCategories_Click);
			// 
			// menuItemAutoCodes
			// 
			this.menuItemAutoCodes.Index = 10;
			this.menuItemAutoCodes.Text = "Auto Codes";
			this.menuItemAutoCodes.Click += new System.EventHandler(this.menuItemAutoCodes_Click);
			// 
			// menuItemAutomation
			// 
			this.menuItemAutomation.Index = 11;
			this.menuItemAutomation.Text = "Automation";
			this.menuItemAutomation.Click += new System.EventHandler(this.menuItemAutomation_Click);
			// 
			// menuItemAutoNotes
			// 
			this.menuItemAutoNotes.Index = 12;
			this.menuItemAutoNotes.Text = "Auto Notes";
			this.menuItemAutoNotes.Click += new System.EventHandler(this.menuItemAutoNotes_Click);
			// 
			// menuItemDataPath
			// 
			this.menuItemDataPath.Index = 13;
			this.menuItemDataPath.Text = "Data Paths";
			this.menuItemDataPath.Click += new System.EventHandler(this.menuItemDataPath_Click);
			// 
			// menuItemDefinitions
			// 
			this.menuItemDefinitions.Index = 14;
			this.menuItemDefinitions.Text = "Definitions";
			this.menuItemDefinitions.Click += new System.EventHandler(this.menuItemDefinitions_Click);
			// 
			// menuItemDentalSchools
			// 
			this.menuItemDentalSchools.Index = 15;
			this.menuItemDentalSchools.Text = "Dental Schools";
			this.menuItemDentalSchools.Click += new System.EventHandler(this.menuItemDentalSchools_Click);
			// 
			// menuItemDisplayFields
			// 
			this.menuItemDisplayFields.Index = 16;
			this.menuItemDisplayFields.Text = "Display Fields";
			this.menuItemDisplayFields.Click += new System.EventHandler(this.menuItemDisplayFields_Click);
			// 
			// menuItemFeeScheds
			// 
			this.menuItemFeeScheds.Index = 17;
			this.menuItemFeeScheds.Text = "Fee Schedules";
			this.menuItemFeeScheds.Click += new System.EventHandler(this.menuItemFeeScheds_Click);
			// 
			// menuItemLaboratories
			// 
			this.menuItemLaboratories.Index = 18;
			this.menuItemLaboratories.Text = "Laboratories";
			this.menuItemLaboratories.Click += new System.EventHandler(this.menuItemLaboratories_Click);
			// 
			// menuItemMisc
			// 
			this.menuItemMisc.Index = 19;
			this.menuItemMisc.Text = "Miscellaneous";
			this.menuItemMisc.Click += new System.EventHandler(this.menuItemMisc_Click);
			// 
			// menuItemModules
			// 
			this.menuItemModules.Index = 20;
			this.menuItemModules.Text = "Module Preferences";
			this.menuItemModules.Click += new System.EventHandler(this.menuItemModules_Click);
			// 
			// menuItemOrtho
			// 
			this.menuItemOrtho.Index = 21;
			this.menuItemOrtho.Text = "Ortho";
			this.menuItemOrtho.Click += new System.EventHandler(this.menuItemOrtho_Click);
			// 
			// menuItemPractice
			// 
			this.menuItemPractice.Index = 22;
			this.menuItemPractice.Text = "Practice";
			this.menuItemPractice.Click += new System.EventHandler(this.menuItemPractice_Click);
			// 
			// menuItemLinks
			// 
			this.menuItemLinks.Index = 23;
			this.menuItemLinks.Text = "Program Links";
			this.menuItemLinks.Click += new System.EventHandler(this.menuItemLinks_Click);
			// 
			// menuItemQuickPasteNotes
			// 
			this.menuItemQuickPasteNotes.Index = 24;
			this.menuItemQuickPasteNotes.Text = "Quick Paste Notes";
			this.menuItemQuickPasteNotes.Click += new System.EventHandler(this.menuItemQuickPasteNotes_Click);
			// 
			// menuItemReports
			// 
			this.menuItemReports.Index = 25;
			this.menuItemReports.Text = "Reports";
			this.menuItemReports.Click += new System.EventHandler(this.menuItemReports_Click);
			// 
			// menuItemRequiredFields
			// 
			this.menuItemRequiredFields.Index = 26;
			this.menuItemRequiredFields.Text = "Required Fields";
			this.menuItemRequiredFields.Click += new System.EventHandler(this.menuItemRequiredFields_Click);
			// 
			// menuItemRequirementsNeeded
			// 
			this.menuItemRequirementsNeeded.Index = 27;
			this.menuItemRequirementsNeeded.Text = "Requirements Needed";
			this.menuItemRequirementsNeeded.Click += new System.EventHandler(this.menuItemRequirementsNeeded_Click);
			// 
			// menuItemSched
			// 
			this.menuItemSched.Index = 28;
			this.menuItemSched.Text = "Schedules";
			this.menuItemSched.Click += new System.EventHandler(this.menuItemSched_Click);
			// 
			// menuItemSecurity
			// 
			this.menuItemSecurity.Index = 29;
			this.menuItemSecurity.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSecuritySettings,
            this.menuItemAddUser});
			this.menuItemSecurity.Text = "Security";
			// 
			// menuItemSheets
			// 
			this.menuItemSheets.Index = 30;
			this.menuItemSheets.Text = "Sheets";
			this.menuItemSheets.Click += new System.EventHandler(this.menuItemSheets_Click);
			// 
			// menuItemSpellCheck
			// 
			this.menuItemSpellCheck.Index = 31;
			this.menuItemSpellCheck.Text = "Spell Check";
			this.menuItemSpellCheck.Click += new System.EventHandler(this.menuItemSpellCheck_Click);
			// 
			// menuItemTasks
			// 
			this.menuItemTasks.Index = 32;
			this.menuItemTasks.Text = "Tasks";
			this.menuItemTasks.Click += new System.EventHandler(this.menuItemTask_Click);
			// 
			// menuItemWebForm
			// 
			this.menuItemWebForm.Index = 33;
			this.menuItemWebForm.Text = "Web Forms";
			this.menuItemWebForm.Click += new System.EventHandler(this.menuItemWebForm_Click);
			// 
			// menuItem20
			// 
			this.menuItem20.Index = 34;
			this.menuItem20.Text = "-";
			// 
			// menuItemObsolete
			// 
			this.menuItemObsolete.Index = 35;
			this.menuItemObsolete.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemLetters,
            this.menuItemQuestions});
			this.menuItemObsolete.Text = "Obsolete";
			// 
			// menuItemLetters
			// 
			this.menuItemLetters.Index = 0;
			this.menuItemLetters.Text = "Letters";
			this.menuItemLetters.Click += new System.EventHandler(this.menuItemLetters_Click);
			// 
			// menuItemQuestions
			// 
			this.menuItemQuestions.Index = 1;
			this.menuItemQuestions.Text = "Questionnaire";
			this.menuItemQuestions.Click += new System.EventHandler(this.menuItemQuestions_Click);
			// 
			// menuItemLists
			// 
			this.menuItemLists.Index = 3;
			this.menuItemLists.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemProcCodes,
            this.menuItem5,
            this.menuItemAllergies,
            this.menuItemClinics,
            this.menuItemContacts,
            this.menuItemCounties,
            this.menuItemSchoolClass,
            this.menuItemSchoolCourses,
            this.menuItemDiscountPlans,
            this.menuItemEmployees,
            this.menuItemEmployers,
            this.menuItemCarriers,
            this.menuItemInsPlans,
            this.menuItemLabCases,
            this.menuItemMedications,
            this.menuItemPharmacies,
            this.menuItemProblems,
            this.menuItemProviders,
            this.menuItemPrescriptions,
            this.menuItemReferrals,
            this.menuItemSchools,
            this.menuItemStateAbbrs,
            this.menuItemZipCodes});
			this.menuItemLists.Shortcut = System.Windows.Forms.Shortcut.CtrlI;
			this.menuItemLists.Text = "&Lists";
			// 
			// menuItemProcCodes
			// 
			this.menuItemProcCodes.Index = 0;
			this.menuItemProcCodes.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF;
			this.menuItemProcCodes.Text = "&Procedure Codes";
			this.menuItemProcCodes.Click += new System.EventHandler(this.menuItemProcCodes_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 1;
			this.menuItem5.Text = "-";
			// 
			// menuItemAllergies
			// 
			this.menuItemAllergies.Index = 2;
			this.menuItemAllergies.Text = "Allergies";
			this.menuItemAllergies.Click += new System.EventHandler(this.menuItemAllergies_Click);
			// 
			// menuItemClinics
			// 
			this.menuItemClinics.Index = 3;
			this.menuItemClinics.Text = "Clinics";
			this.menuItemClinics.Click += new System.EventHandler(this.menuItemClinics_Click);
			// 
			// menuItemContacts
			// 
			this.menuItemContacts.Index = 4;
			this.menuItemContacts.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftC;
			this.menuItemContacts.Text = "&Contacts";
			this.menuItemContacts.Click += new System.EventHandler(this.menuItemContacts_Click);
			// 
			// menuItemCounties
			// 
			this.menuItemCounties.Index = 5;
			this.menuItemCounties.Text = "Counties";
			this.menuItemCounties.Click += new System.EventHandler(this.menuItemCounties_Click);
			// 
			// menuItemSchoolClass
			// 
			this.menuItemSchoolClass.Index = 6;
			this.menuItemSchoolClass.Text = "Dental School Classes";
			this.menuItemSchoolClass.Click += new System.EventHandler(this.menuItemSchoolClass_Click);
			// 
			// menuItemSchoolCourses
			// 
			this.menuItemSchoolCourses.Index = 7;
			this.menuItemSchoolCourses.Text = "Dental School Courses";
			this.menuItemSchoolCourses.Click += new System.EventHandler(this.menuItemSchoolCourses_Click);
			// 
			// menuItemDiscountPlans
			// 
			this.menuItemDiscountPlans.Index = 8;
			this.menuItemDiscountPlans.Text = "Discount Plans";
			this.menuItemDiscountPlans.Click += new System.EventHandler(this.menuItemDiscountPlans_Click);
			// 
			// menuItemEmployees
			// 
			this.menuItemEmployees.Index = 9;
			this.menuItemEmployees.Text = "&Employees";
			this.menuItemEmployees.Click += new System.EventHandler(this.menuItemEmployees_Click);
			// 
			// menuItemEmployers
			// 
			this.menuItemEmployers.Index = 10;
			this.menuItemEmployers.Text = "Employers";
			this.menuItemEmployers.Click += new System.EventHandler(this.menuItemEmployers_Click);
			// 
			// menuItemCarriers
			// 
			this.menuItemCarriers.Index = 11;
			this.menuItemCarriers.Text = "Insurance Carriers";
			this.menuItemCarriers.Click += new System.EventHandler(this.menuItemCarriers_Click);
			// 
			// menuItemInsPlans
			// 
			this.menuItemInsPlans.Index = 12;
			this.menuItemInsPlans.Text = "&Insurance Plans";
			this.menuItemInsPlans.Click += new System.EventHandler(this.menuItemInsPlans_Click);
			// 
			// menuItemLabCases
			// 
			this.menuItemLabCases.Index = 13;
			this.menuItemLabCases.Text = "Lab Cases";
			this.menuItemLabCases.Click += new System.EventHandler(this.menuItemLabCases_Click);
			// 
			// menuItemMedications
			// 
			this.menuItemMedications.Index = 14;
			this.menuItemMedications.Text = "&Medications";
			this.menuItemMedications.Click += new System.EventHandler(this.menuItemMedications_Click);
			// 
			// menuItemPharmacies
			// 
			this.menuItemPharmacies.Index = 15;
			this.menuItemPharmacies.Text = "Pharmacies";
			this.menuItemPharmacies.Click += new System.EventHandler(this.menuItemPharmacies_Click);
			// 
			// menuItemProblems
			// 
			this.menuItemProblems.Index = 16;
			this.menuItemProblems.Text = "Problems";
			this.menuItemProblems.Click += new System.EventHandler(this.menuItemProblems_Click);
			// 
			// menuItemProviders
			// 
			this.menuItemProviders.Index = 17;
			this.menuItemProviders.Text = "Providers";
			this.menuItemProviders.Click += new System.EventHandler(this.menuItemProviders_Click);
			// 
			// menuItemPrescriptions
			// 
			this.menuItemPrescriptions.Index = 18;
			this.menuItemPrescriptions.Text = "Pre&scriptions";
			this.menuItemPrescriptions.Click += new System.EventHandler(this.menuItemPrescriptions_Click);
			// 
			// menuItemReferrals
			// 
			this.menuItemReferrals.Index = 19;
			this.menuItemReferrals.Text = "&Referrals";
			this.menuItemReferrals.Click += new System.EventHandler(this.menuItemReferrals_Click);
			// 
			// menuItemSchools
			// 
			this.menuItemSchools.Index = 20;
			this.menuItemSchools.Text = "Sites";
			this.menuItemSchools.Click += new System.EventHandler(this.menuItemSites_Click);
			// 
			// menuItemStateAbbrs
			// 
			this.menuItemStateAbbrs.Index = 21;
			this.menuItemStateAbbrs.Text = "State Abbreviations";
			this.menuItemStateAbbrs.Click += new System.EventHandler(this.menuItemStateAbbrs_Click);
			// 
			// menuItemZipCodes
			// 
			this.menuItemZipCodes.Index = 22;
			this.menuItemZipCodes.Text = "&Zip Codes";
			this.menuItemZipCodes.Click += new System.EventHandler(this.menuItemZipCodes_Click);
			// 
			// menuItemReportsHeader
			// 
			this.menuItemReportsHeader.Index = 4;
			this.menuItemReportsHeader.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemReportsStandard,
            this.menuItemReportsGraphic,
            this.menuItemReportsUserQuery,
            this.menuItemReportsUnfinalizedPay});
			this.menuItemReportsHeader.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
			this.menuItemReportsHeader.Text = "&Reports";
			this.menuItemReportsHeader.Popup += new System.EventHandler(this.menuItemReportsHeader_Popup);
			// 
			// menuItemReportsStandard
			// 
			this.menuItemReportsStandard.Index = 0;
			this.menuItemReportsStandard.Text = "&Standard";
			this.menuItemReportsStandard.Click += new System.EventHandler(this.menuItemReportsStandard_Click);
			// 
			// menuItemReportsGraphic
			// 
			this.menuItemReportsGraphic.Index = 1;
			this.menuItemReportsGraphic.Text = "&Graphic";
			this.menuItemReportsGraphic.Click += new System.EventHandler(this.menuItemReportsGraphic_Click);
			// 
			// menuItemReportsUserQuery
			// 
			this.menuItemReportsUserQuery.Index = 2;
			this.menuItemReportsUserQuery.Text = "&User Query";
			this.menuItemReportsUserQuery.Click += new System.EventHandler(this.menuItemReportsUserQuery_Click);
			// 
			// menuItemReportsUnfinalizedPay
			// 
			this.menuItemReportsUnfinalizedPay.Index = 3;
			this.menuItemReportsUnfinalizedPay.Text = "Unfinalized Payments";
			this.menuItemReportsUnfinalizedPay.Click += new System.EventHandler(this.menuItemReportsUnfinalizedPay_Click);
			// 
			// menuItemCustomReports
			// 
			this.menuItemCustomReports.Index = 5;
			this.menuItemCustomReports.Text = "Custom Reports";
			// 
			// menuItemTools
			// 
			this.menuItemTools.Index = 6;
			this.menuItemTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemJobManager,
            this.menuItemPrintScreen,
            this.menuItem1,
            this.menuItem9,
            this.menuItemAging,
            this.menuItemAuditTrail,
            this.menuItemAutoOrtho,
            this.menuItemFinanceCharge,
            this.menuItemCCRecurring,
            this.menuItemCustomerManage,
            this.menuItemDatabaseMaintenance,
            this.menuItemDispensary,
            this.menuItemEvaluations,
            this.menuItemTerminal,
            this.menuItemTerminalManager,
            this.menuItemTranslation,
            this.menuItemMobileSetup,
            this.menuItemNewCropBilling,
            this.menuItemPendingPayments,
            this.menuItemScreening,
            this.menuItemRepeatingCharges,
            this.menuItemResellers,
            this.menuItemSetupWizard,
            this.menuItemReqStudents,
            this.menuItemWebForms,
            this.menuItemWiki,
            this.menuItemXWebTrans});
			this.menuItemTools.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
			this.menuItemTools.Text = "&Tools";
			// 
			// menuItemJobManager
			// 
			this.menuItemJobManager.Index = 0;
			this.menuItemJobManager.Text = "Job Manager";
			this.menuItemJobManager.Visible = false;
			this.menuItemJobManager.Click += new System.EventHandler(this.menuItemJobManager_Click);
			// 
			// menuItemPrintScreen
			// 
			this.menuItemPrintScreen.Index = 1;
			this.menuItemPrintScreen.Text = "&Print Screen Tool";
			this.menuItemPrintScreen.Click += new System.EventHandler(this.menuItemPrintScreen_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 2;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAutoClosePayPlans,
            this.menuItemDuplicateBlockouts,
            this.menuItemCreateAtoZFolders,
            this.menuItemDatabaseMaintenancePat,
            this.menuItemMergeDPs,
            this.menuItemMergeMedications,
            this.menuItemMergePatients,
            this.menuItemMergeProviders,
            this.menuItemMergeReferrals,
            this.menuItemMoveSubscribers,
            this.menuItemProcLockTool,
            this.menuItemServiceManager,
            this.menuItemShutdown,
            this.menuTelephone,
            this.menuItemTestLatency,
            this.menuItemXChargeReconcile});
			this.menuItem1.Text = "Misc Tools";
			// 
			// menuItemAutoClosePayPlans
			// 
			this.menuItemAutoClosePayPlans.Index = 0;
			this.menuItemAutoClosePayPlans.Text = "Auto-Close Payment Plans";
			this.menuItemAutoClosePayPlans.Click += new System.EventHandler(this.menuItemAutoClosePayPlans_Click);
			// 
			// menuItemDuplicateBlockouts
			// 
			this.menuItemDuplicateBlockouts.Index = 1;
			this.menuItemDuplicateBlockouts.Text = "Clear Duplicate Blockouts";
			this.menuItemDuplicateBlockouts.Click += new System.EventHandler(this.menuItemDuplicateBlockouts_Click);
			// 
			// menuItemCreateAtoZFolders
			// 
			this.menuItemCreateAtoZFolders.Index = 2;
			this.menuItemCreateAtoZFolders.Text = "Create A to Z Folders";
			this.menuItemCreateAtoZFolders.Click += new System.EventHandler(this.menuItemCreateAtoZFolders_Click);
			// 
			// menuItemDatabaseMaintenancePat
			// 
			this.menuItemDatabaseMaintenancePat.Index = 3;
			this.menuItemDatabaseMaintenancePat.Text = "Database Maintenance Pat";
			this.menuItemDatabaseMaintenancePat.Click += new System.EventHandler(this.menuItemDatabaseMaintenancePat_Click);
			// 
			// menuItemMergeDPs
			// 
			this.menuItemMergeDPs.Index = 4;
			this.menuItemMergeDPs.Text = "Merge Discount Plans";
			this.menuItemMergeDPs.Click += new System.EventHandler(this.menuItemMergeDPs_Click);
			// 
			// menuItemMergeMedications
			// 
			this.menuItemMergeMedications.Index = 5;
			this.menuItemMergeMedications.Text = "Merge Medications";
			this.menuItemMergeMedications.Click += new System.EventHandler(this.menuItemMergeMedications_Click);
			// 
			// menuItemMergePatients
			// 
			this.menuItemMergePatients.Index = 6;
			this.menuItemMergePatients.Text = "Merge Patients";
			this.menuItemMergePatients.Click += new System.EventHandler(this.menuItemMergePatients_Click);
			// 
			// menuItemMergeProviders
			// 
			this.menuItemMergeProviders.Index = 7;
			this.menuItemMergeProviders.Text = "Merge Providers";
			this.menuItemMergeProviders.Click += new System.EventHandler(this.menuItemMergeProviders_Click);
			// 
			// menuItemMergeReferrals
			// 
			this.menuItemMergeReferrals.Index = 8;
			this.menuItemMergeReferrals.Text = "Merge Referrals";
			this.menuItemMergeReferrals.Click += new System.EventHandler(this.menuItemMergeReferrals_Click);
			// 
			// menuItemMoveSubscribers
			// 
			this.menuItemMoveSubscribers.Index = 9;
			this.menuItemMoveSubscribers.Text = "Move Subscribers";
			this.menuItemMoveSubscribers.Click += new System.EventHandler(this.menuItemMoveSubscribers_Click);
			// 
			// menuItemProcLockTool
			// 
			this.menuItemProcLockTool.Index = 10;
			this.menuItemProcLockTool.Text = "Procedure Lock Tool";
			this.menuItemProcLockTool.Click += new System.EventHandler(this.menuItemProcLockTool_Click);
			// 
			// menuItemServiceManager
			// 
			this.menuItemServiceManager.Index = 11;
			this.menuItemServiceManager.Text = "Service Manager";
			this.menuItemServiceManager.Click += new System.EventHandler(this.menuItemServiceManager_Click);
			// 
			// menuItemShutdown
			// 
			this.menuItemShutdown.Index = 12;
			this.menuItemShutdown.Text = "Shutdown All Workstations";
			this.menuItemShutdown.Click += new System.EventHandler(this.menuItemShutdown_Click);
			// 
			// menuTelephone
			// 
			this.menuTelephone.Index = 13;
			this.menuTelephone.Text = "Telephone Numbers";
			this.menuTelephone.Click += new System.EventHandler(this.menuTelephone_Click);
			// 
			// menuItemTestLatency
			// 
			this.menuItemTestLatency.Index = 14;
			this.menuItemTestLatency.Text = "Test Latency";
			this.menuItemTestLatency.Click += new System.EventHandler(this.menuItemTestLatency_Click);
			// 
			// menuItemXChargeReconcile
			// 
			this.menuItemXChargeReconcile.Index = 15;
			this.menuItemXChargeReconcile.Text = "X-Charge Reconcile";
			this.menuItemXChargeReconcile.Visible = false;
			this.menuItemXChargeReconcile.Click += new System.EventHandler(this.menuItemXChargeReconcile_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 3;
			this.menuItem9.Text = "-";
			// 
			// menuItemAging
			// 
			this.menuItemAging.Index = 4;
			this.menuItemAging.Text = "&Aging";
			this.menuItemAging.Click += new System.EventHandler(this.menuItemAging_Click);
			// 
			// menuItemAuditTrail
			// 
			this.menuItemAuditTrail.Index = 5;
			this.menuItemAuditTrail.Text = "Audit Trail";
			this.menuItemAuditTrail.Click += new System.EventHandler(this.menuItemAuditTrail_Click);
			// 
			// menuItemAutoOrtho
			// 
			this.menuItemAutoOrtho.Index = 6;
			this.menuItemAutoOrtho.Text = "Auto Ortho Tool";
			this.menuItemAutoOrtho.Click += new System.EventHandler(this.menuItemAutoOrtho_Click);
			// 
			// menuItemFinanceCharge
			// 
			this.menuItemFinanceCharge.Index = 7;
			this.menuItemFinanceCharge.Text = "Billing/&Finance Charges";
			this.menuItemFinanceCharge.Click += new System.EventHandler(this.menuItemFinanceCharge_Click);
			// 
			// menuItemCCRecurring
			// 
			this.menuItemCCRecurring.Index = 8;
			this.menuItemCCRecurring.Text = "CC Recurring Charges";
			this.menuItemCCRecurring.Click += new System.EventHandler(this.menuItemCCRecurring_Click);
			// 
			// menuItemCustomerManage
			// 
			this.menuItemCustomerManage.Index = 9;
			this.menuItemCustomerManage.Text = "Customer Management";
			this.menuItemCustomerManage.Click += new System.EventHandler(this.menuItemCustomerManage_Click);
			// 
			// menuItemDatabaseMaintenance
			// 
			this.menuItemDatabaseMaintenance.Index = 10;
			this.menuItemDatabaseMaintenance.Text = "Database Maintenance";
			this.menuItemDatabaseMaintenance.Click += new System.EventHandler(this.menuItemDatabaseMaintenance_Click);
			// 
			// menuItemDispensary
			// 
			this.menuItemDispensary.Index = 11;
			this.menuItemDispensary.Text = "Dispensary";
			this.menuItemDispensary.Visible = false;
			this.menuItemDispensary.Click += new System.EventHandler(this.menuItemDispensary_Click);
			// 
			// menuItemEvaluations
			// 
			this.menuItemEvaluations.Index = 12;
			this.menuItemEvaluations.Text = "Evaluations";
			this.menuItemEvaluations.Click += new System.EventHandler(this.menuItemEvaluations_Click);
			// 
			// menuItemTerminal
			// 
			this.menuItemTerminal.Index = 13;
			this.menuItemTerminal.Text = "Kiosk";
			this.menuItemTerminal.Click += new System.EventHandler(this.menuItemTerminal_Click);
			// 
			// menuItemTerminalManager
			// 
			this.menuItemTerminalManager.Index = 14;
			this.menuItemTerminalManager.Text = "Kiosk Manager";
			this.menuItemTerminalManager.Click += new System.EventHandler(this.menuItemTerminalManager_Click);
			// 
			// menuItemTranslation
			// 
			this.menuItemTranslation.Index = 15;
			this.menuItemTranslation.Text = "Language Translation";
			this.menuItemTranslation.Click += new System.EventHandler(this.menuItemTranslation_Click);
			// 
			// menuItemMobileSetup
			// 
			this.menuItemMobileSetup.Index = 16;
			this.menuItemMobileSetup.Text = "Mobile Synch";
			this.menuItemMobileSetup.Click += new System.EventHandler(this.menuItemMobileSetup_Click);
			// 
			// menuItemNewCropBilling
			// 
			this.menuItemNewCropBilling.Index = 17;
			this.menuItemNewCropBilling.Text = "NewCrop Billing";
			this.menuItemNewCropBilling.Click += new System.EventHandler(this.menuItemNewCropBilling_Click);
			// 
			// menuItemPendingPayments
			// 
			this.menuItemPendingPayments.Index = 18;
			this.menuItemPendingPayments.Text = "Pending &Online Payments";
			this.menuItemPendingPayments.Click += new System.EventHandler(this.menuItemPendingPayments_Click);
			// 
			// menuItemScreening
			// 
			this.menuItemScreening.Index = 19;
			this.menuItemScreening.Text = "Public Health Screening";
			this.menuItemScreening.Click += new System.EventHandler(this.menuItemScreening_Click);
			// 
			// menuItemRepeatingCharges
			// 
			this.menuItemRepeatingCharges.Index = 20;
			this.menuItemRepeatingCharges.Text = "Repeating Charges";
			this.menuItemRepeatingCharges.Click += new System.EventHandler(this.menuItemRepeatingCharges_Click);
			// 
			// menuItemResellers
			// 
			this.menuItemResellers.Index = 21;
			this.menuItemResellers.Text = "Resellers";
			this.menuItemResellers.Visible = false;
			this.menuItemResellers.Click += new System.EventHandler(this.menuItemResellers_Click);
			// 
			// menuItemSetupWizard
			// 
			this.menuItemSetupWizard.Index = 22;
			this.menuItemSetupWizard.Text = "Setup Wizard";
			this.menuItemSetupWizard.Click += new System.EventHandler(this.menuItemSetupWizard_Click);
			// 
			// menuItemReqStudents
			// 
			this.menuItemReqStudents.Index = 23;
			this.menuItemReqStudents.Text = "Student Requirements";
			this.menuItemReqStudents.Click += new System.EventHandler(this.menuItemReqStudents_Click);
			// 
			// menuItemWebForms
			// 
			this.menuItemWebForms.Index = 24;
			this.menuItemWebForms.Text = "Web Forms";
			this.menuItemWebForms.Click += new System.EventHandler(this.menuItemWebForms_Click);
			// 
			// menuItemWiki
			// 
			this.menuItemWiki.Index = 25;
			this.menuItemWiki.Text = "Wiki";
			this.menuItemWiki.Click += new System.EventHandler(this.menuItemWiki_Click);
			// 
			// menuItemXWebTrans
			// 
			this.menuItemXWebTrans.Index = 26;
			this.menuItemXWebTrans.Text = "&XWeb Transactions";
			this.menuItemXWebTrans.Click += new System.EventHandler(this.menuItemXWebTrans_Click);
			// 
			// menuClinics
			// 
			this.menuClinics.Index = 7;
			this.menuClinics.Text = "&Clinics";
			// 
			// menuItemEServices
			// 
			this.menuItemEServices.Index = 8;
			this.menuItemEServices.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSignup,
            this.menuMobileWeb,
            this.menuItemPatientPortal,
            this.menuItemWebSched,
            this.menuItemIntegratedTexting,
            this.menuItemERemind,
            this.menuItemEConfirm,
            this.menuItemEMisc,
            this.menuItem14,
            this.menuItemListenerService});
			this.menuItemEServices.Text = "eServices";
			// 
			// menuItemSignup
			// 
			this.menuItemSignup.Index = 0;
			this.menuItemSignup.Text = "Signup";
			this.menuItemSignup.Click += new System.EventHandler(this.menuItemSignup_Click);
			// 
			// menuMobileWeb
			// 
			this.menuMobileWeb.Index = 1;
			this.menuMobileWeb.Text = "Mobile Web";
			this.menuMobileWeb.Click += new System.EventHandler(this.menuMobileWeb_Click);
			// 
			// menuItemPatientPortal
			// 
			this.menuItemPatientPortal.Index = 2;
			this.menuItemPatientPortal.Text = "Patient Portal";
			this.menuItemPatientPortal.Click += new System.EventHandler(this.menuItemPatientPortal_Click);
			// 
			// menuItemWebSched
			// 
			this.menuItemWebSched.Index = 3;
			this.menuItemWebSched.Text = "Web Sched";
			this.menuItemWebSched.Click += new System.EventHandler(this.menuItemWebSched_Click);
			// 
			// menuItemIntegratedTexting
			// 
			this.menuItemIntegratedTexting.Index = 4;
			this.menuItemIntegratedTexting.Text = "Texting";
			this.menuItemIntegratedTexting.Click += new System.EventHandler(this.menuItemIntegratedTexting_Click);
			// 
			// menuItemERemind
			// 
			this.menuItemERemind.Index = 5;
			this.menuItemERemind.Text = "eReminder";
			this.menuItemERemind.Click += new System.EventHandler(this.menuItemEConfirmRemind_Click);
			// 
			// menuItemEConfirm
			// 
			this.menuItemEConfirm.Index = 6;
			this.menuItemEConfirm.Text = "eConfirmation";
			this.menuItemEConfirm.Click += new System.EventHandler(this.menuItemEConfirmRemind_Click);
			// 
			// menuItemEMisc
			// 
			this.menuItemEMisc.Index = 7;
			this.menuItemEMisc.Text = "Misc";
			this.menuItemEMisc.Click += new System.EventHandler(this.menuItemEMisc_Click);
			// 
			// menuItem14
			// 
			this.menuItem14.Index = 8;
			this.menuItem14.Text = "-";
			// 
			// menuItemListenerService
			// 
			this.menuItemListenerService.Index = 9;
			this.menuItemListenerService.Text = "eConnector Service";
			this.menuItemListenerService.Click += new System.EventHandler(this.menuItemListenerService_Click);
			// 
			// menuItemAlerts
			// 
			this.menuItemAlerts.Index = 9;
			this.menuItemAlerts.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemNoAlerts});
			this.menuItemAlerts.OwnerDraw = true;
			this.menuItemAlerts.Text = "Alerts (0)";
			this.menuItemAlerts.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.menuItemAlerts_DrawItem);
			this.menuItemAlerts.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.menuItemAlerts_MeasureItem);
			this.menuItemAlerts.Popup += new System.EventHandler(this.menuItemAlerts_Popup);
			// 
			// menuItemNoAlerts
			// 
			this.menuItemNoAlerts.Index = 0;
			this.menuItemNoAlerts.Text = "No alerts";
			// 
			// menuItemHelp
			// 
			this.menuItemHelp.Index = 10;
			this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemRemote,
            this.menuItemHelpWindows,
            this.menuItemHelpContents,
            this.menuItemHelpIndex,
            this.menuItemWebinar,
            this.menuItemRemoteSupport,
            this.menuItemRequestFeatures,
            this.menuItemUpdate,
            this.menuItem11,
            this.menuItemAbout});
			this.menuItemHelp.Text = "&Help";
			// 
			// menuItemRemote
			// 
			this.menuItemRemote.Index = 0;
			this.menuItemRemote.Text = "Online Support";
			this.menuItemRemote.Click += new System.EventHandler(this.menuItemRemote_Click);
			// 
			// menuItemHelpWindows
			// 
			this.menuItemHelpWindows.Index = 1;
			this.menuItemHelpWindows.Text = "Local Help-Windows";
			this.menuItemHelpWindows.Click += new System.EventHandler(this.menuItemHelpWindows_Click);
			// 
			// menuItemHelpContents
			// 
			this.menuItemHelpContents.Index = 2;
			this.menuItemHelpContents.Text = "Online Help - Contents";
			this.menuItemHelpContents.Click += new System.EventHandler(this.menuItemHelpContents_Click);
			// 
			// menuItemHelpIndex
			// 
			this.menuItemHelpIndex.Index = 3;
			this.menuItemHelpIndex.Shortcut = System.Windows.Forms.Shortcut.ShiftF1;
			this.menuItemHelpIndex.Text = "Online Help - Index";
			this.menuItemHelpIndex.Click += new System.EventHandler(this.menuItemHelpIndex_Click);
			// 
			// menuItemWebinar
			// 
			this.menuItemWebinar.Index = 4;
			this.menuItemWebinar.Text = "Training Videos";
			this.menuItemWebinar.Click += new System.EventHandler(this.menuItemWebinar_Click);
			// 
			// menuItemRemoteSupport
			// 
			this.menuItemRemoteSupport.Index = 5;
			this.menuItemRemoteSupport.Text = "Remote Support with Code";
			this.menuItemRemoteSupport.Click += new System.EventHandler(this.menuItemRemoteSupport_Click);
			// 
			// menuItemRequestFeatures
			// 
			this.menuItemRequestFeatures.Index = 6;
			this.menuItemRequestFeatures.Text = "Request Features";
			this.menuItemRequestFeatures.Click += new System.EventHandler(this.menuItemRequestFeatures_Click);
			// 
			// menuItemUpdate
			// 
			this.menuItemUpdate.Index = 7;
			this.menuItemUpdate.Text = "&Update";
			this.menuItemUpdate.Click += new System.EventHandler(this.menuItemUpdate_Click);
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 8;
			this.menuItem11.Text = "-";
			// 
			// menuItemAbout
			// 
			this.menuItemAbout.Index = 9;
			this.menuItemAbout.Text = "About";
			this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
			// 
			// imageList32
			// 
			this.imageList32.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList32.ImageStream")));
			this.imageList32.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList32.Images.SetKeyName(0, "Appt32.gif");
			this.imageList32.Images.SetKeyName(1, "Family32b.gif");
			this.imageList32.Images.SetKeyName(2, "Account32b.gif");
			this.imageList32.Images.SetKeyName(3, "TreatPlan3D.gif");
			this.imageList32.Images.SetKeyName(4, "chart32.gif");
			this.imageList32.Images.SetKeyName(5, "Images32.gif");
			this.imageList32.Images.SetKeyName(6, "Manage32.gif");
			this.imageList32.Images.SetKeyName(7, "TreatPlanMed32.gif");
			this.imageList32.Images.SetKeyName(8, "ChartMed32.gif");
			// 
			// timerSignals
			// 
			this.timerSignals.Tick += new System.EventHandler(this.timerSignals_Tick);
			// 
			// panelSplitter
			// 
			this.panelSplitter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelSplitter.Cursor = System.Windows.Forms.Cursors.HSplit;
			this.panelSplitter.Location = new System.Drawing.Point(71, 542);
			this.panelSplitter.Name = "panelSplitter";
			this.panelSplitter.Size = new System.Drawing.Size(769, 7);
			this.panelSplitter.TabIndex = 50;
			this.panelSplitter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelSplitter_MouseDown);
			this.panelSplitter.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelSplitter_MouseMove);
			this.panelSplitter.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelSplitter_MouseUp);
			// 
			// menuSplitter
			// 
			this.menuSplitter.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemDockBottom,
            this.menuItemDockRight});
			// 
			// menuItemDockBottom
			// 
			this.menuItemDockBottom.Index = 0;
			this.menuItemDockBottom.Text = "Dock to Bottom";
			this.menuItemDockBottom.Click += new System.EventHandler(this.menuItemDockBottom_Click);
			// 
			// menuItemDockRight
			// 
			this.menuItemDockRight.Index = 1;
			this.menuItemDockRight.Text = "Dock to Right";
			this.menuItemDockRight.Click += new System.EventHandler(this.menuItemDockRight_Click);
			// 
			// imageListMain
			// 
			this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
			this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListMain.Images.SetKeyName(0, "Pat.gif");
			this.imageListMain.Images.SetKeyName(1, "commlog.gif");
			this.imageListMain.Images.SetKeyName(2, "email.gif");
			this.imageListMain.Images.SetKeyName(3, "tasksNicer.gif");
			this.imageListMain.Images.SetKeyName(4, "label.gif");
			this.imageListMain.Images.SetKeyName(5, "Text.gif");
			// 
			// menuPatient
			// 
			this.menuPatient.Popup += new System.EventHandler(this.menuPatient_Popup);
			// 
			// menuLabel
			// 
			this.menuLabel.Popup += new System.EventHandler(this.menuLabel_Popup);
			// 
			// menuEmail
			// 
			this.menuEmail.Popup += new System.EventHandler(this.menuEmail_Popup);
			// 
			// menuLetter
			// 
			this.menuLetter.Popup += new System.EventHandler(this.menuLetter_Popup);
			// 
			// timerDisabledKey
			// 
			this.timerDisabledKey.Enabled = true;
			this.timerDisabledKey.Interval = 600000;
			this.timerDisabledKey.Tick += new System.EventHandler(this.timerDisabledKey_Tick);
			// 
			// timerWebHostSynch
			// 
			this.timerWebHostSynch.Enabled = true;
			this.timerWebHostSynch.Interval = 30000;
			this.timerWebHostSynch.Tick += new System.EventHandler(this.timerWebHostSynch_Tick);
			// 
			// timerLogoff
			// 
			this.timerLogoff.Interval = 15000;
			this.timerLogoff.Tick += new System.EventHandler(this.timerLogoff_Tick);
			// 
			// timerReplicationMonitor
			// 
			this.timerReplicationMonitor.Interval = 10000;
			this.timerReplicationMonitor.Tick += new System.EventHandler(this.timerReplicationMonitor_Tick);
			// 
			// panelPhoneSmall
			// 
			this.panelPhoneSmall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.panelPhoneSmall.Controls.Add(this.butVoiceMails);
			this.panelPhoneSmall.Controls.Add(this.labelFieldType);
			this.panelPhoneSmall.Controls.Add(this.comboTriageCoordinator);
			this.panelPhoneSmall.Controls.Add(this.labelMsg);
			this.panelPhoneSmall.Controls.Add(this.butMapPhones);
			this.panelPhoneSmall.Controls.Add(this.butTriage);
			this.panelPhoneSmall.Controls.Add(this.butBigPhones);
			this.panelPhoneSmall.Controls.Add(this.labelWaitTime);
			this.panelPhoneSmall.Controls.Add(this.labelTriage);
			this.panelPhoneSmall.Location = new System.Drawing.Point(71, 333);
			this.panelPhoneSmall.Name = "panelPhoneSmall";
			this.panelPhoneSmall.Size = new System.Drawing.Size(213, 184);
			this.panelPhoneSmall.TabIndex = 56;
			// 
			// butVoiceMails
			// 
			this.butVoiceMails.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butVoiceMails.Autosize = true;
			this.butVoiceMails.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butVoiceMails.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butVoiceMails.CornerRadius = 4F;
			this.butVoiceMails.Location = new System.Drawing.Point(1, 0);
			this.butVoiceMails.Name = "butVoiceMails";
			this.butVoiceMails.Size = new System.Drawing.Size(18, 24);
			this.butVoiceMails.TabIndex = 89;
			this.butVoiceMails.Text = "V";
			this.butVoiceMails.Click += new System.EventHandler(this.butVoiceMails_Click);
			// 
			// labelFieldType
			// 
			this.labelFieldType.Location = new System.Drawing.Point(4, 25);
			this.labelFieldType.Name = "labelFieldType";
			this.labelFieldType.Size = new System.Drawing.Size(143, 15);
			this.labelFieldType.TabIndex = 88;
			this.labelFieldType.Text = "Triage Coordinator";
			this.labelFieldType.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboTriageCoordinator
			// 
			this.comboTriageCoordinator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTriageCoordinator.FormattingEnabled = true;
			this.comboTriageCoordinator.Location = new System.Drawing.Point(0, 42);
			this.comboTriageCoordinator.MaxDropDownItems = 10;
			this.comboTriageCoordinator.Name = "comboTriageCoordinator";
			this.comboTriageCoordinator.Size = new System.Drawing.Size(213, 21);
			this.comboTriageCoordinator.TabIndex = 87;
			this.comboTriageCoordinator.SelectionChangeCommitted += new System.EventHandler(this.comboTriageCoordinator_SelectionChangeCommitted);
			// 
			// labelMsg
			// 
			this.labelMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelMsg.ForeColor = System.Drawing.Color.Firebrick;
			this.labelMsg.Location = new System.Drawing.Point(20, 2);
			this.labelMsg.Name = "labelMsg";
			this.labelMsg.Size = new System.Drawing.Size(35, 20);
			this.labelMsg.TabIndex = 53;
			this.labelMsg.Text = "00";
			this.labelMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butMapPhones
			// 
			this.butMapPhones.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMapPhones.Autosize = true;
			this.butMapPhones.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMapPhones.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMapPhones.CornerRadius = 4F;
			this.butMapPhones.Location = new System.Drawing.Point(192, 0);
			this.butMapPhones.Name = "butMapPhones";
			this.butMapPhones.Size = new System.Drawing.Size(19, 24);
			this.butMapPhones.TabIndex = 54;
			this.butMapPhones.Text = "M";
			this.butMapPhones.Click += new System.EventHandler(this.butMapPhones_Click);
			// 
			// butTriage
			// 
			this.butTriage.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butTriage.Autosize = true;
			this.butTriage.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTriage.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTriage.CornerRadius = 4F;
			this.butTriage.Location = new System.Drawing.Point(154, 0);
			this.butTriage.Name = "butTriage";
			this.butTriage.Size = new System.Drawing.Size(18, 24);
			this.butTriage.TabIndex = 52;
			this.butTriage.Text = "T";
			this.butTriage.Click += new System.EventHandler(this.butTriage_Click);
			// 
			// butBigPhones
			// 
			this.butBigPhones.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBigPhones.Autosize = true;
			this.butBigPhones.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBigPhones.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBigPhones.CornerRadius = 4F;
			this.butBigPhones.Location = new System.Drawing.Point(173, 0);
			this.butBigPhones.Name = "butBigPhones";
			this.butBigPhones.Size = new System.Drawing.Size(18, 24);
			this.butBigPhones.TabIndex = 52;
			this.butBigPhones.Text = "B";
			this.butBigPhones.Click += new System.EventHandler(this.butBigPhones_Click);
			// 
			// labelWaitTime
			// 
			this.labelWaitTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelWaitTime.ForeColor = System.Drawing.Color.Black;
			this.labelWaitTime.Location = new System.Drawing.Point(123, 2);
			this.labelWaitTime.Name = "labelWaitTime";
			this.labelWaitTime.Size = new System.Drawing.Size(30, 20);
			this.labelWaitTime.TabIndex = 53;
			this.labelWaitTime.Text = "00m";
			this.labelWaitTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelTriage
			// 
			this.labelTriage.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTriage.ForeColor = System.Drawing.Color.Black;
			this.labelTriage.Location = new System.Drawing.Point(71, 2);
			this.labelTriage.Name = "labelTriage";
			this.labelTriage.Size = new System.Drawing.Size(41, 20);
			this.labelTriage.TabIndex = 53;
			this.labelTriage.Text = "T:000";
			this.labelTriage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// menuText
			// 
			this.menuText.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemTextMessagesAll,
            this.menuItemTextMessagesReceived,
            this.menuItemTextMessagesSent});
			// 
			// menuItemTextMessagesAll
			// 
			this.menuItemTextMessagesAll.Index = 0;
			this.menuItemTextMessagesAll.Text = "Text Messages All";
			this.menuItemTextMessagesAll.Click += new System.EventHandler(this.menuItemTextMessagesAll_Click);
			// 
			// menuItemTextMessagesReceived
			// 
			this.menuItemTextMessagesReceived.Index = 1;
			this.menuItemTextMessagesReceived.Text = "Text Messages Received";
			this.menuItemTextMessagesReceived.Click += new System.EventHandler(this.menuItemTextMessagesReceived_Click);
			// 
			// menuItemTextMessagesSent
			// 
			this.menuItemTextMessagesSent.Index = 2;
			this.menuItemTextMessagesSent.Text = "Text Messages Sent";
			this.menuItemTextMessagesSent.Click += new System.EventHandler(this.menuItemTextMessagesSent_Click);
			// 
			// timerUpdateTime
			// 
			this.timerUpdateTime.Interval = 1000;
			this.timerUpdateTime.Tick += new System.EventHandler(this.timerUpdateTime_Tick);
			// 
			// menuTask
			// 
			this.menuTask.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemTaskNewForUser,
            this.menuItemTaskReminders});
			this.menuTask.Popup += new System.EventHandler(this.menuTask_Popup);
			// 
			// menuItemTaskNewForUser
			// 
			this.menuItemTaskNewForUser.Index = 0;
			this.menuItemTaskNewForUser.Text = "New for [User]";
			this.menuItemTaskNewForUser.Click += new System.EventHandler(this.menuItemTaskNewForUser_Click);
			// 
			// menuItemTaskReminders
			// 
			this.menuItemTaskReminders.Index = 1;
			this.menuItemTaskReminders.Text = "Reminders";
			this.menuItemTaskReminders.Click += new System.EventHandler(this.menuItemTaskReminders_Click);
			// 
			// menuCommlog
			// 
			this.menuCommlog.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemCommlogPersistent});
			// 
			// menuItemCommlogPersistent
			// 
			this.menuItemCommlogPersistent.Index = 0;
			this.menuItemCommlogPersistent.Text = "Persistent";
			this.menuItemCommlogPersistent.Click += new System.EventHandler(this.menuItemCommlogPersistent_Click);
			// 
			// lightSignalGrid1
			// 
			this.lightSignalGrid1.Location = new System.Drawing.Point(0, 463);
			this.lightSignalGrid1.Name = "lightSignalGrid1";
			this.lightSignalGrid1.Size = new System.Drawing.Size(50, 206);
			this.lightSignalGrid1.TabIndex = 20;
			this.lightSignalGrid1.Text = "lightSignalGrid1";
			this.lightSignalGrid1.ButtonClick += new OpenDental.UI.ODLightSignalGridClickEventHandler(this.lightSignalGrid1_ButtonClick);
			// 
			// FormOpenDental
			// 
			this.ClientSize = new System.Drawing.Size(982, 413);
			this.Controls.Add(this.panelPhoneSmall);
			this.Controls.Add(this.panelSplitter);
			this.Controls.Add(this.lightSignalGrid1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.Name = "FormOpenDental";
			this.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Text = "Open Dental";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Deactivate += new System.EventHandler(this.FormOpenDental_Deactivate);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormOpenDental_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormOpenDental_FormClosed);
			this.Load += new System.EventHandler(this.FormOpenDental_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormOpenDental_KeyDown);
			this.Resize += new System.EventHandler(this.FormOpenDental_Resize);
			this.panelPhoneSmall.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private void FormOpenDental_Load(object sender,System.EventArgs e) {
			//FormSplash can cause FormOpenDental to open behind other applications.
			TopMost=true;
			Application.DoEvents();
			TopMost=false;
			Activate();
			_isStartingUp=true;//Halts mobile synch during updates.
			//This will be increased to 4 hours below but only after the convert script has succeeded.
			DataConnection.ConnectionRetrySeconds=(int)TimeSpan.FromMinutes(1).TotalSeconds;
			allNeutral();
			string odUser="";
			string odPassHash="";
			string webServiceUri="";
			YN webServiceIsEcw=YN.Unknown;
			bool isSilentUpdate=false;
			string odPassword="";
			string serverName="";
			string databaseName="";
			string mySqlUser="";
			string mySqlPassword="";
			string mySqlPassHash="";
			if(CommandLineArgs.Length!=0) {
				for(int i=0;i<CommandLineArgs.Length;i++) {
					if(CommandLineArgs[i].StartsWith("UserName=") && CommandLineArgs[i].Length>9) {
						odUser=CommandLineArgs[i].Substring(9).Trim('"');
					}
					if(CommandLineArgs[i].StartsWith("PassHash=") && CommandLineArgs[i].Length>9) {
						odPassHash=CommandLineArgs[i].Substring(9).Trim('"');
					}
					if(CommandLineArgs[i].StartsWith("WebServiceUri=") && CommandLineArgs[i].Length>14) {
						webServiceUri=CommandLineArgs[i].Substring(14).Trim('"');
					}
					if(CommandLineArgs[i].StartsWith("WebServiceIsEcw=") && CommandLineArgs[i].Length>16) {
						if(CommandLineArgs[i].Substring(16).Trim('"')=="True") {
							webServiceIsEcw=YN.Yes;
						}
						else {
							webServiceIsEcw=YN.No;
						}
					}
					if(CommandLineArgs[i].StartsWith("OdPassword=") && CommandLineArgs[i].Length>11) {
						odPassword=CommandLineArgs[i].Substring(11).Trim('"');
					}
					if(CommandLineArgs[i].StartsWith("ServerName=") && CommandLineArgs[i].Length>11) {
						serverName=CommandLineArgs[i].Substring(11).Trim('"');
					}
					if(CommandLineArgs[i].StartsWith("DatabaseName=") && CommandLineArgs[i].Length>13) {
						databaseName=CommandLineArgs[i].Substring(13).Trim('"');
					}
					if(CommandLineArgs[i].StartsWith("MySqlUser=") && CommandLineArgs[i].Length>10) {
						mySqlUser=CommandLineArgs[i].Substring(10).Trim('"');
					}
					if(CommandLineArgs[i].StartsWith("MySqlPassword=") && CommandLineArgs[i].Length>14) {
						mySqlPassword=CommandLineArgs[i].Substring(14).Trim('"');
					}
					if(CommandLineArgs[i].StartsWith("MySqlPassHash=") && CommandLineArgs[i].Length>14) {
						mySqlPassHash=CommandLineArgs[i].Substring(14).Trim('"');
					}
					if(CommandLineArgs[i].StartsWith("IsSilentUpdate=") && CommandLineArgs[i].Length>15) {
						if(CommandLineArgs[i].Substring(15).Trim('"').ToLower()=="true") {
							isSilentUpdate=true;
						}
						else {
							isSilentUpdate=false;
						}
					}
				}
			}
			YN noShow=YN.Unknown;
			if(webServiceUri!="") {//a web service was specified
				if(odUser!="" && odPassword!="") {//and both a username and password were specified
					noShow=YN.Yes;
				}
			}
			else if(databaseName!="") {
				noShow=YN.Yes;
			}
			//Users that want to silently update MUST pass in the following command line args.
			if(isSilentUpdate && (odUser.Trim()==""
					|| (odPassword.Trim()=="" && odPassHash.Trim()=="")
					|| serverName.Trim()==""
					|| databaseName.Trim()==""
					|| mySqlUser.Trim()==""
					|| (mySqlPassword.Trim()=="" && mySqlPassHash.Trim()==""))) 
			{
				ExitCode=104;//Required command line arguments have not been set for silent updating
				Environment.Exit(ExitCode);
				return;
			}
			Version versionOd=Assembly.GetAssembly(typeof(FormOpenDental)).GetName().Version;
			Version versionObBus=Assembly.GetAssembly(typeof(Db)).GetName().Version;
			if(versionOd!=versionObBus) {
				if(isSilentUpdate) {
					ExitCode=105;//File versions do not match
				}
				else {//Not a silent update.  Show a warning message.
					//No MsgBox or Lan.g() here, because we don't want to access the database if there is a version conflict.
					MessageBox.Show("Mismatched program file versions. Please run the Open Dental setup file again on this computer.");
				}
				Environment.Exit(ExitCode);
				return;
			}
			ChooseDatabaseModel chooseDatabaseModel=new ChooseDatabaseModel();
			try {
				chooseDatabaseModel=ChooseDatabaseModel.GetChooseDatabaseModelFromConfig(webServiceUri,webServiceIsEcw,odUser,serverName,databaseName
					,mySqlUser,mySqlPassword,mySqlPassHash,noShow);
			}
			catch(ODException ode) {
				if(isSilentUpdate) {
					//Technically the only way GetChooseDatabaseModelFromConfig() can throw an exception when silent updating is if DatabaseName wasn't set.
					ExitCode=104;//Required command line arguments have not been set for silent updating
				}
				else {
					MessageBox.Show(ode.Message);
				}
				Environment.Exit(ExitCode);
				return;
			}
			FormChooseDatabase FormCD=new FormChooseDatabase();
			ChooseDatabaseController chooseDatabaseController=new ChooseDatabaseController(FormCD);
			while(true) {//Most users will loop through once.  If user tries to connect to a db with replication failure, they will loop through again.
				if(chooseDatabaseModel.NoShow==YN.Yes) {
					try {
						CentralConnections.TryToConnect(chooseDatabaseModel.CentralConnectionCur,chooseDatabaseModel.DbType,chooseDatabaseModel.ConnectionString
							,(chooseDatabaseModel.NoShow==YN.Yes),chooseDatabaseModel.ListAdminCompNames,(CommandLineArgs.Length!=0));
					}
					catch(Exception) {
						if(isSilentUpdate) {
							ExitCode=106;//Connection to specified database has failed
							Environment.Exit(ExitCode);
							return;
						}
						//The current connection settings are invalid so simply show the choose database window for the user to correct them.
						FormCD.ShowDialog(chooseDatabaseModel,chooseDatabaseController);
						if(FormCD.DialogResult==DialogResult.Cancel) {
							Environment.Exit(ExitCode);
							return;
						}
					}
				}
				else {
					FormCD.ShowDialog(chooseDatabaseModel,chooseDatabaseController);
					if(FormCD.DialogResult==DialogResult.Cancel) {
						Environment.Exit(ExitCode);
						return;
					}
				}
				Cursor=Cursors.WaitCursor;
				//theme
				try {
					ODToolBar.UseBlueTheme=PrefC.GetBool(PrefName.ColorTheme);
					ODGrid.UseBlueTheme=PrefC.GetBool(PrefName.ColorTheme);
				}
				catch {
					//try/catch in case you are trying to convert from an older version of OD and need to update the DB.
				}
				this.Invalidate();//apply them at next repaint.
				try {
					Plugins.LoadAllPlugins(this);//moved up from near RefreshLocalData(invalidTypes). New position might cause problems.
				}
				catch {
					//Do nothing since this will likely only fail if a column is added to the program table, 
					//due to this method getting called before the update script.  If the plugins do not load, then the simple solution is to restart OD.
				}
				Splash=new FormSplash();
				if(CommandLineArgs.Length==0) {
					Splash.Show();
				}
				if(!PrefsStartup(isSilentUpdate)) {//In Release, refreshes the Pref cache if conversion successful.
					Cursor=Cursors.Default;
					Splash.Dispose();
					if(ExitCode==0) {
						//PrefsStartup failed and ExitCode is still 0 which means an unexpected error must have occured.
						//Set the exit code to 999 which will represent an Unknown Error
						ExitCode=999;
					}
					Environment.Exit(ExitCode);
					return;
				}
				if(isSilentUpdate) {
					//The db was successfully updated so there is nothing else that needs to be done after this point.
					Application.Exit();//Exits with ExitCode=0
					return;
				}
				if(ReplicationServers.Server_id!=0 && ReplicationServers.Server_id==PrefC.GetLong(PrefName.ReplicationFailureAtServer_id)) {
					MsgBox.Show(this,"This database is temporarily unavailable.  Please connect instead to your alternate database at the other location.");
					chooseDatabaseModel.NoShow=YN.No;//This ensures they will get a choose db window next time through the loop.
					ReplicationServers.Server_id=-1;
					continue;
				}
				break;
			}
			Logger.DoVerboseLogging=PrefC.IsVerboseLoggingSession;
			if(Programs.UsingEcwTightOrFullMode()) {
				Splash.Dispose();//We don't show splash screen when bridging to eCW.
			}
			CreateFHIRConfigFile();
			//Setting the time that we want to wait when the database connection has been lost.
			//We don't want a LostConnection event to fire when updating because of Silent Updating which would fail due to window pop-ups from this event.
			//When the event is triggered a "connection lost" window will display allowing the user to attempt reconnecting to the database
			//and then resume what they were doing.  The purpose of this is to prevent UE's from happening with poor connections or temporary outages.
			DataConnection.ConnectionRetrySeconds=(int)TimeSpan.FromHours(4).TotalSeconds;
			DataConnectionEvent.Fired+=DataConnection_ConnectionLost;//Hook up the connection lost event. Nothing prior to this point will have LostConnection events fired.
			CredentialsFailedAfterLoginEvent.Fired+=DataConnection_CredentialsFailedAfterLogin;
			RefreshLocalData(InvalidType.Prefs);//should only refresh preferences so that SignalLastClearedDate preference can be used in ClearOldSignals()
			Signalods.ClearOldSignals();
			ODThread featureThread=new ODThread((o) => { EnableAdditionalFeatures(); });
			featureThread.AddExceptionHandler((ex) => { });//silently fail.
			featureThread.Name="EnableAdditionalFeaturesThread";
			featureThread.Start(true);
			//We no longer do this shotgun approach because it can slow the loading time.
			//RefreshLocalData(InvalidType.AllLocal);
			List<InvalidType> invalidTypes=new List<InvalidType>();
			//invalidTypes.Add(InvalidType.Prefs);//Preferences were refreshed above.  The only preference which might be stale is SignalLastClearedDate, but it is not used anywhere after calling ClearOldSignals() above.
			invalidTypes.Add(InvalidType.Defs);
			invalidTypes.Add(InvalidType.Providers);//obviously heavily used
			invalidTypes.Add(InvalidType.Programs);//already done above, but needs to be done explicitly to trigger the PostCleanup 
			invalidTypes.Add(InvalidType.ToolBut);//so program buttons will show in all the toolbars
			//InvalidType.PatFields is necessary because the appts for the day are drawn in parallel threads and without it the cache is filled in each
			//thread resulting in multiple calls to fill the ApptFieldDef and PatFieldDef caches.
			invalidTypes.Add(InvalidType.PatFields);
			if(Programs.UsingEcwTightMode()) {
				lightSignalGrid1.Visible=false;
			}
			else{
				invalidTypes.Add(InvalidType.SigMessages);//so when mouse moves over light buttons, it won't crash
			}
			//Plugins.LoadAllPlugins(this);//moved up from right after optimizing tooth chart graphics.  New position might cause problems.
			//It was moved because RefreshLocalData()=>RefreshLocalDataPostCleanup()=>ContrChart2.InitializeLocalData()=>LayoutToolBar() has a hook.
			//Moved it up again on 10/3/13
			RefreshLocalData(invalidTypes.ToArray());
			FillSignalButtons(null);
			ContrManage2.InitializeOnStartup();//so that when a signal is received, it can handle it.
			//Lan.Refresh();//automatically skips if current culture is en-US
			//LanguageForeigns.Refresh(CultureInfo.CurrentCulture);//automatically skips if current culture is en-US
			//DataValid.BecameInvalid += new OpenDental.ValidEventHandler(DataValid_BecameInvalid);
			Signalods.SignalLastRefreshed=MiscData.GetNowDateTime();
			if(PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs)==0) {
				//I don't know why we would ever want to allow anyone to disable signals entirely.
				timerSignals.Enabled=false;
			}
			else {
				timerSignals.Interval=PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs)*1000;
				timerSignals.Enabled=true;
			}
			timerTimeIndic.Enabled=true;
			comboTriageCoordinator.MouseWheel += new MouseEventHandler(comboTriageCoordinator_MouseWheel);
			myOutlookBar.Buttons[0].Caption=Lan.g(this,"Appts");
			myOutlookBar.Buttons[1].Caption=Lan.g(this,"Family");
			myOutlookBar.Buttons[2].Caption=Lan.g(this,"Account");
			myOutlookBar.Buttons[3].Caption=Lan.g(this,"Treat' Plan");
			//myOutlookBar.Buttons[4].Caption=Lan.g(this,"Chart");//??done in RefreshLocalData
			myOutlookBar.Buttons[5].Caption=Lan.g(this,"Images");
			myOutlookBar.Buttons[6].Caption=Lan.g(this,"Manage");
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum/*Always 0 at this point*/)) { //Changes the Chart and Treatment Plan icons to ones without teeth
				myOutlookBar.Buttons[3].ImageIndex=7;
				myOutlookBar.Buttons[4].ImageIndex=8;
			}
			Lan.C("MainMenu",mainMenu);
			if(CultureInfo.CurrentCulture.Name=="en-US") {
				//for the business layer, this functionality is duplicated in the Lan class.  Need for SL.
				CultureInfo cInfo=(CultureInfo)CultureInfo.CurrentCulture.Clone();
				cInfo.DateTimeFormat.ShortDatePattern="MM/dd/yyyy";
				Application.CurrentCulture=cInfo;
				//
				//if(CultureInfo.CurrentCulture.TwoLetterISOLanguageName=="en"){
				menuItemTranslation.Visible=false;
			}
			else {//not en-US
				CultureInfo cInfo=(CultureInfo)CultureInfo.CurrentCulture.Clone();
				string dateFormatCur=cInfo.DateTimeFormat.ShortDatePattern;
				//The carrot indicates the beginning of a word.  {2} means that there has to be exactly 2 y's.  [^a-z] means any character except a through z.  $ means end of word.
				if(Regex.IsMatch(dateFormatCur,"^y{2}[^a-z]")
					|| Regex.IsMatch(dateFormatCur,"[^a-z]y{2}$")) 
				{
					//We know there are only two y's in the format.  Force there to be four.
					cInfo.DateTimeFormat.ShortDatePattern=dateFormatCur.Replace("yy","yyyy");
					Application.CurrentCulture=cInfo;
				}
			}
			//"In the .NET Framework 4 and previous versions, by default, the culture of all threads is set to the Windows system culture.  In the .NET
			//Framework 4.5, the DefaultThreadCurrentCulture property enables an application to define the default culture of all threads in an
			//application domain."  https://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo.defaultthreadcurrentculture(v=vs.110).aspx
			//If we don't set this, dates are formated as M/d/yyyy, e.g. instead of 03/01/2017 you get 3/1/2017.  This causes issues since we sometimes
			//use date strings as part of key data when setting and validating signatures.  The key data inside of a thread will not match the key data
			//outside of a thread.  By defining the DefaultThreadCurrentCulture equal to Application.CurrentCulture, we dictate that, unless explicitly
			//overwritten when a thread is instantiated, all threads spawned by OD will default to the application's current culture not the system culture.
			CultureInfo.DefaultThreadCurrentCulture=Application.CurrentCulture;
			CultureInfo.DefaultThreadCurrentUICulture=Application.CurrentCulture;
			//For EHR users we want to load up the EHR code list from the obfuscated dll in a background thread because it takes roughly 11 seconds to load up.
			if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				try {
					Thread threadPreloadEhrCodeList=new Thread((ThreadStart)EhrCodes.UpdateList);//Background loading of the EHR.dll code list.
					threadPreloadEhrCodeList.Start();
				}
				catch { 
					//This should never happen.  It would most likely be due to a corrupt dll issue but I don't want to stop the start up sequence.
					//Users could theoretically use Open Dental for an entire day and never hit the code that utilizes the EhrCodes class.
					//Therefore, we do not want to cause any issues and the worst case scenario is the users has to put up with the 11 second delay (old behavior). 
				}
			}
			if(!File.Exists("Help.chm")){
				menuItemHelpWindows.Visible=false;
			}
			if(Environment.OSVersion.Platform==PlatformID.Unix){//Create A to Z unsupported on Unix for now.
				menuItemCreateAtoZFolders.Visible=false;
			}
			if(!PrefC.GetBool(PrefName.ProcLockingIsAllowed)) {
				menuItemProcLockTool.Visible=false;
			}
			if(!PrefC.GetBool(PrefName.ADAdescriptionsReset)) {
				ProcedureCodes.ResetADAdescriptions();
				Prefs.UpdateBool(PrefName.ADAdescriptionsReset,true);
			}
			//Spawn a thread so that attempting to start services on this computer does not hinder the loading time of Open Dental.
			ODThread odThreadServiceStarter=new ODThread(StartODServices);
			//If the thread that attempts to start all Open Dental services fails for any reason, silently fail.
			odThreadServiceStarter.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception ex) => { }));
			odThreadServiceStarter.Name="ServiceStarterThread";
			odThreadServiceStarter.Start(true);
			Splash.Dispose();
			if(ODEnvironment.IsRunningOnDbServer(MiscData.GetODServer()) && PrefC.GetBool(PrefName.ShowFeatureEhr)) {//OpenDental has EHR enabled and is running on the same machine as the mysql server it is connected to.*/
				_threadTimeSynch=new Thread(new ThreadStart(ThreadTimeSynch_Synch));
				_threadTimeSynch.Start();
			}
			_isStartingUp=false;//Used to allow Mobile Synch to continue
			LogOnOpenDentalUser(odUser,odPassword);
			//If clinics are enabled, we will set the public ClinicNum variable
			//If the user is restricted to a clinic(s), and the computerpref clinic is not one of the user's restricted clinics, the user's clinic will be selected
			//If the user is not restricted, or if the user is restricted but has access to the computerpref clinic, the computerpref clinic will be selected
			//The ClinicNum will determine which view is loaded, either from the computerpref table or from the userodapptview table
			if(PrefC.HasClinicsEnabled && Security.CurUser!=null) {//If block must be run before StartCacheFillForFees() so correct clinic filtration occurs.
				Clinics.LoadClinicNumForUser();
				RefreshMenuClinics();
			}
			StartCacheFillForFees(); //Must be called after the clinics are loaded, so that the current user clinic will be included in the FeeCache
			FillSignalButtons(null);
			if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {
				string prefImagePath=ImageStore.GetPreferredAtoZpath();
				if(prefImagePath==null || !Directory.Exists(prefImagePath)) {//AtoZ folder not found
					//Cache.Refresh(InvalidType.Security);
					FormPath FormP=new FormPath();
					FormP.IsStartingUp=true;
					FormP.ShowDialog();
					if(FormP.DialogResult!=DialogResult.OK) {
						MsgBox.Show(this,"Invalid A to Z path.  Closing program.");
						Application.Exit();
					}
				}
			}
			IsTreatPlanSortByTooth=PrefC.GetBool(PrefName.TreatPlanSortByTooth); //not a great place for this, but we don't have a better alternative.
			if(userControlTasks1.Visible) {
				userControlTasks1.InitializeOnStartup();
			}
			myOutlookBar.SelectedIndex=Security.GetModule(0);//for eCW, this fails silently.
			//if(Programs.UsingEcwTight()) {
			if(Programs.UsingEcwTightOrFullMode()
				|| (HL7Defs.IsExistingHL7Enabled() && !HL7Defs.GetOneDeepEnabled().ShowAppts)) {
				myOutlookBar.SelectedIndex=4;//Chart module
				//ToolBarMain.Height=0;//this should force the modules further up on the screen
				//ToolBarMain.Visible=false;
				LayoutControls();
			}
			if(Programs.UsingOrion) {
				myOutlookBar.SelectedIndex=1;//Family module
			}
			myOutlookBar.Invalidate();
			LayoutToolBar();
			RefreshMenuReports();
			Cursor=Cursors.Default;
			if(myOutlookBar.SelectedIndex==-1){
				MsgBox.Show(this,"You do not have permission to use any modules.");
			}
			Bridges.Trojan.StartupCheck();
			FormUAppoint.StartThreadIfEnabled();
			Bridges.ICat.StartFileWatcher();
			Bridges.TigerView.StartFileWatcher();
			if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {
				menuItemJobManager.Visible=true;
				menuItemResellers.Visible=true;
				menuItemXChargeReconcile.Visible=true;
#if !DEBUG
					if(Process.GetProcessesByName("ProximityOD").Length==0) {
						try {
							Process.Start("ProximityOD.exe");
						}
						catch { }//for example, if working from home.
					}
#endif
				ODThread odThreadVMs=new ODThread(SetVoiceMailLabel);
				odThreadVMs.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception ex) => { }));//Silently fail
				odThreadVMs.Start(true);
				//Only run this thread every 1.6 seconds.
				_odThreadHqMetrics=new ODThread(1600,ProcessHqMetrics);
				_odThreadHqMetrics.AddExceptionHandler(new ODThread.ExceptionDelegate(OnThreadHqMetricsException));
				_odThreadHqMetrics.Name="HQ Metrics Thread";
				_odThreadHqMetrics.Start();
				FillComboTriageCoordinator();
			}
			#if !TRIALONLY
				if(PrefC.GetDate(PrefName.BackupReminderLastDateRun).AddMonths(1)<DateTime.Today) {
					FormBackupReminder FormBR=new FormBackupReminder();
					FormBR.ShowDialog();
					if(FormBR.DialogResult==DialogResult.OK){
						Prefs.UpdateDateT(PrefName.BackupReminderLastDateRun,DateTimeOD.Today);
					}
					else{
						Application.Exit();
						return;
					}
				}
				//==tg  Commented out on 04/24/2014 after discussion deciding it was not necessary to synchronize time on start-up. Timeouts and other errors were causing complaints from customers.
				//if(PrefC.GetBool(PrefName.ShowFeatureEhr) && !_isTimeSynchThreadRunning) {
				//	FormEhrTimeSynch FormEhrTS = new FormEhrTimeSynch();
				//	FormEhrTS.IsAutoLaunch=true;
				//	if(!FormEhrTS.TimesInSynchFast()) {
				//		FormEhrTS.ShowDialog();
				//	}
				//}
			#endif
			FillPatientButton(null);
			//ThreadCommandLine=new Thread(new ThreadStart(Listen));
			//if(!IsSecondInstance) {//can't use a port that's already in use.
			//	//js We can't do this yet.  I tried it already, and it consumes nearly 100% CPU.  Not while testing, but later.
			//	//So until we really need to do it, it's easiest no just not start the thread for now.
			//	//ThreadCommandLine.Start();
			//}
			//if(CommandLineArgs.Length>0) {
			ProcessCommandLine(CommandLineArgs);
			//}
			try {
				Computers.UpdateHeartBeat(Environment.MachineName,true);
			}
			catch { }
			//string dllPathEHR=ODFileUtils.CombinePaths(Application.StartupPath,"EHR.dll");
			//if(PrefC.GetBoolSilent(PrefName.ShowFeatureEhr,false)) {
			//	#if EHRTEST
			//		FormEHR=new FormEHR();
			//		ContrChart2.InitializeLocalData();//because toolbar is now missing the EHR button.  Only a problem if a db conversion is done when opening the program.
			//	#else
			//		ObjFormEhrMeasures=null;
			//		AssemblyEHR=null;
			//		if(File.Exists(dllPathEHR)) {//EHR.dll is available, so load it up
			//			AssemblyEHR=Assembly.LoadFile(dllPathEHR);
			//			Type type=AssemblyEHR.GetType("EHR.FormEHR");//namespace.class
			//			ObjFormEhrMeasures=Activator.CreateInstance(type);
			//		}
			//	#endif
			//}
			Text=PatientL.GetMainTitle(Patients.GetPat(CurPatNum),Clinics.ClinicNum);
			Security.DateTimeLastActivity=DateTime.Now;
			timerLogoff.Enabled=true;
			timerReplicationMonitor.Enabled=true;
			ODThread threadEmailInbox=new ODThread(10000,ThreadEmailInbox_Receive);
			threadEmailInbox.AddExceptionHandler(new ODThread.ExceptionDelegate((ex) => {
				//Do not let the program crash for the email inbox thread.  The user can manually launch the inbox window to see what problems there are.
			}));
			threadEmailInbox.Name="EmailInboxThread";
			threadEmailInbox.Start();
			if(!PrefC.GetBool(PrefName.ClaimReportReceivedByService)) {
				int claimReportRetrieveIntervalMS=(int)TimeSpan.FromMinutes(PrefC.GetInt(PrefName.ClaimReportReceiveInterval)).TotalMilliseconds;
				_threadClaimReportRetrieve=new ODThread(claimReportRetrieveIntervalMS,ThreadClaimReportRetrieve);
				_threadClaimReportRetrieve.AddExceptionHandler(ThreadClaimReportException);
				_threadClaimReportRetrieve.Name="Claim Report Thread";
				_threadClaimReportRetrieve.Start();
			}
			_threadPodium=new ODThread(Podium.PodiumThreadIntervalMS,((ODThread o) => { Podium.ThreadPodiumSendInvitations(false); }));
			_threadPodium.AddExceptionHandler(PodiumMonitoringException);
			_threadPodium.Name="Podium Thread";
			_threadPodium.Start();
			ODThread threadCompHeartbeat=new ODThread(180000,o => {//Every three minutes
				Computers.UpdateHeartBeat(Environment.MachineName,false);
			});
			threadCompHeartbeat.AddExceptionHandler(ex => ex.DoNothing());
			threadCompHeartbeat.Name="ComputerHeartbeat";
			threadCompHeartbeat.Start();
			StartEServiceMonitoring();
			Patient pat=Patients.GetPat(CurPatNum);
			if(pat!=null && (_showForm=="popup" || _showForm=="popups") && myOutlookBar.SelectedIndex!=-1) {
				FormPopupsForFam FormP=new FormPopupsForFam(PopupEventList);
				FormP.PatCur=pat;
				FormP.ShowDialog();
			}
			bool isApptModuleSelected=false;
			if(myOutlookBar.SelectedIndex==0){
				isApptModuleSelected=true;
			}
			if(CurPatNum!=0 && _showForm=="apptsforpatient" && isApptModuleSelected) {
				ContrAppt2.DisplayOtherDlg(false);
			}
			if(_showForm=="searchpatient") {
				FormPatientSelect formPS=new FormPatientSelect();
				formPS.ShowDialog();
				if(formPS.DialogResult==DialogResult.OK) {
					CurPatNum=formPS.SelectedPatNum;
					pat=Patients.GetPat(CurPatNum);
					if(ContrChart2.Visible) {
						ContrChart2.ModuleSelectedErx(CurPatNum);
					}
					else {
						RefreshCurrentModule();
					}
					FillPatientButton(pat);
				}
			}
			if(!PrefC.IsODHQ) {
				//Remove the menu items that are only needed for HQ like Default CC Procedures
				menuItemAccount.MenuItems.Clear();
			}
			if(PrefC.GetString(PrefName.LanguageAndRegion)!=CultureInfo.CurrentCulture.Name && !ComputerPrefs.LocalComputer.NoShowLanguage) {
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Warning, having mismatched language setting between the workstation and server may cause the program "
					+"to behave in unexpected ways. Would you like to view the setup window?"))
				{
					FormLanguageAndRegion FormLAR=new FormLanguageAndRegion();
					FormLAR.ShowDialog();
				}
			}
			if(CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits != 2 //We want our users to have their currency decimal setting set to 2.
				&& !ComputerPrefs.LocalComputer.NoShowDecimal) 
			{
				FormDecimalSettings FormDS=new FormDecimalSettings();
				FormDS.ShowDialog();
			}
			//Choose a default DirectX format when no DirectX format has been specified and running in DirectX tooth chart mode.
			if(ComputerPrefs.LocalComputer.GraphicsSimple==DrawingMode.DirectX && ComputerPrefs.LocalComputer.DirectXFormat=="") {
				try {
					ComputerPrefs.LocalComputer.DirectXFormat=FormGraphics.GetPreferredDirectXFormat(this);
					if(ComputerPrefs.LocalComputer.DirectXFormat=="invalid") {
						//No valid local DirectX format could be found.
						ComputerPrefs.LocalComputer.GraphicsSimple=DrawingMode.Simple2D;
					}
					ComputerPrefs.Update(ComputerPrefs.LocalComputer);
					//Reinitialize the tooth chart because the graphics mode was probably changed which should change the tooth chart appearence.
					ContrChart2.InitializeOnStartup();
				}
				catch(Exception) {
					//The tooth chart will default to Simple2D mode if the above code fails for any reason.  This will at least get the user into the program.
				}
			}
			ComputerPrefs.UpdateLocalComputerOS();
			timerUpdateTime.Start();
			WikiPages.NavPageDelegate=S_WikiLoadPage;
			CheckAlerts();
			CanadianItransCarrierThread();
			Plugins.HookAddCode(this,"FormOpenDental.Load_end");
		}

		private void menuItemInternalTools_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			PhoneEmpSubGroupType tabDefault=PhoneEmpSubGroupType.Avail;
			Enum.TryParse(((MenuItem)sender).Text,out tabDefault);
			FormPhoneEmpDefaultEscalationEdit FormPEDEE=new FormPhoneEmpDefaultEscalationEdit(tabDefault);
			FormPEDEE.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Escalation team changed");
		}

		///<summary>Creates an OpenDentalFHIRConfig.xml file if one does not exist.</summary>
		private void CreateFHIRConfigFile() {
			//We don't include OpenDentalFHIRConfig.xml in Setup.exe because we don't want to overwrite existing settings. We will check to see if it 
			//exists, and create it if it doesn't.
			if(!Directory.Exists(ODFileUtils.CombinePaths(Application.StartupPath,"OpenDentalFHIR"))
				|| File.Exists(ODFileUtils.CombinePaths(Application.StartupPath,"OpenDentalFHIR/OpenDentalFHIRConfig.xml"))) 
			{
				return;
			}
			//Use default values that can be edited later if needed.
			string dbType="MySQL";
			string connectionString="";
			string computerName="localhost";
			string databaseName="opendental";
			string user="root";
			string password="";
			string passwordHash="";
			XmlDocument document=new XmlDocument();
			try {
				document.Load(ODFileUtils.CombinePaths(Application.StartupPath,"FreeDentalConfig.xml"));
				XPathNavigator Navigator=document.CreateNavigator();
				XPathNavigator nav;
				//Database type
				nav=Navigator.SelectSingleNode("//DatabaseType");
				if(nav!=null) {
					dbType=nav.Value;
				}
				//See if there's a ConnectionString
				nav=Navigator.SelectSingleNode("//ConnectionString");
				if(nav!=null) {
					connectionString=nav.Value;
				}
				//See if there's a DatabaseConnection
				nav=Navigator.SelectSingleNode("//DatabaseConnection");
				if(nav!=null) {
					computerName=nav.SelectSingleNode("ComputerName").Value;
					databaseName=nav.SelectSingleNode("Database").Value;
					user=nav.SelectSingleNode("User").Value;
					password=nav.SelectSingleNode("Password").Value;
					XPathNavigator encryptedPwdNode=nav.SelectSingleNode("MySQLPassHash");
					if(password==""
						&& encryptedPwdNode!=null
						&& encryptedPwdNode.Value!="") 
					{
						passwordHash=encryptedPwdNode.Value;
					}
				}
				XmlWriterSettings settings=new XmlWriterSettings();
				settings.Indent=true;
				settings.IndentChars=("    ");
				using(XmlWriter writer=XmlWriter.Create(ODFileUtils.CombinePaths(Application.StartupPath,"OpenDentalFHIR/OpenDentalFHIRConfig.xml"),
					settings)) {
					writer.WriteStartElement("ConnectionSettings");
					if(connectionString!="") {
						writer.WriteStartElement("ConnectionString");
						writer.WriteString(connectionString);
						writer.WriteEndElement();
					}
					else {
						writer.WriteStartElement("DatabaseConnection");
						writer.WriteStartElement("ComputerName");
						writer.WriteString(computerName);
						writer.WriteEndElement();
						writer.WriteStartElement("Database");
						writer.WriteString(databaseName);
						writer.WriteEndElement();
						writer.WriteStartElement("User");
						writer.WriteString(user);
						writer.WriteEndElement();
						writer.WriteStartElement("Password");
						writer.WriteString(string.IsNullOrEmpty(passwordHash) ? password : "");
						writer.WriteEndElement();
						writer.WriteStartElement("MySQLPassHash");
						writer.WriteString(passwordHash);
						writer.WriteEndElement();
						writer.WriteStartElement("UserLow");
						writer.WriteString("");
						writer.WriteEndElement();
						writer.WriteStartElement("PasswordLow");
						writer.WriteString("");
						writer.WriteEndElement();
						writer.WriteEndElement();
						writer.WriteEndElement();
					}
					writer.WriteStartElement("DatabaseType");
					writer.WriteString(dbType);
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.Flush();
				}//using writer
			}
			catch(Exception) {
				//Config file not created.
			}
		}

		private void EnableAdditionalFeatures() {
			Pref featurePref = Prefs.GetPref(PrefName.ProgramAdditionalFeatures.ToString());
			if(featurePref==null || PrefC.GetDateT(PrefName.ProgramAdditionalFeatures)>MiscData.GetNowDateTime()) {
				return;
			}
			DateTime dateOriginal = MiscData.GetNowDateTime().AddMinutes(-30);
			featurePref.ValueString=dateOriginal.AddDays(1).ToString(CultureInfo.InvariantCulture);//default try again in one day unless set below.
			Prefs.Update(featurePref);
			Signalods.SetInvalid(InvalidType.Prefs);
			string response=WebServiceMainHQProxy.GetWebServiceMainHQInstance()
				.EnableAdditionalFeatures(WebServiceMainHQProxy.CreateWebServiceHQPayload("",eServiceCode.Undefined));
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(response);
			XmlNode node;
			bool refreshNeeded = false;
			//Update all "Disable Advertising HQ" program links based on what HQ provided.
			refreshNeeded|=SetAdvertising(ProgramName.CentralDataStorage,doc);
			refreshNeeded|=SetAdvertising(ProgramName.DentalTekSmartOfficePhone,doc);
			refreshNeeded|=SetAdvertising(ProgramName.Podium,doc);
			refreshNeeded|=SetAdvertising(ProgramName.RapidCall,doc);
			refreshNeeded|=SetAdvertising(ProgramName.Transworld,doc);
			refreshNeeded|=SetAdvertising(ProgramName.DentalIntel,doc);
			refreshNeeded|=SetAdvertising(ProgramName.PracticeByNumbers,doc);
			if(refreshNeeded) {
				Signalods.SetInvalid(InvalidType.Programs);
			}
			node = doc.SelectSingleNode("//NextIntervalDays");
			if(node!=null) {
				long days = 7;//default value;
				long.TryParse(node.InnerText,out days);
				Prefs.UpdateDateT(PrefName.ProgramAdditionalFeatures,dateOriginal.AddDays(days));
				Prefs.Update(featurePref);
			}
		}

		private bool SetAdvertising(ProgramName progName, XmlDocument doc) {
			ProgramProperty property=ProgramProperties.GetForProgram(Programs.GetCur(progName).ProgramNum).FirstOrDefault(x => x.PropertyDesc=="Disable Advertising HQ");
			ProgramProperty propOld=null;
			XmlNode node=doc.SelectSingleNode("//"+progName.ToString());
			if(node==null) {
				return false;
			}
			if(property==null) {
				property=new ProgramProperty();
				property.PropertyDesc="Disable Advertising HQ";
				property.ProgramNum=Programs.GetCur(progName).ProgramNum;
			}
			else {
				propOld=property.Copy();
			}
			//"true" from HQ == 0 for the property value.
			//"false" from HQ == 1 for the property value.
			//This is because the boolean from HQ is whether or not to show the advertisement, whereas in OD the boolean is whether or not to hide the advertisement
			bool isDisabledByHQ=!(node.InnerText.ToLower()=="true");
			property.PropertyValue=POut.Bool(isDisabledByHQ);
			if(propOld==null) {
				ProgramProperties.Insert(property);
				return true;
			}
			else {
				return ProgramProperties.Update(property,propOld);
			}
		}

		private void comboTriageCoordinator_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboTriageCoordinator.SelectedIndex<0) {
				return;
			}
			ODException.SwallowAnyException(() => {
				if(SiteLinks.UpdateTriageCoordinator(SiteLinks.GetSiteLinkByIP().SiteLinkNum
					,((ODBoxItem<Employee>)comboTriageCoordinator.SelectedItem).Tag.EmployeeNum))
				{
					DataValid.SetInvalid(InvalidType.Sites);
				}
			});
		}

		private bool PrefsStartup() {
			return PrefsStartup(false);
		}

		///<summary>Returns false if it can't complete a conversion, find datapath, or validate registration key.</summary>
		private bool PrefsStartup(bool isSilentUpdate){
			try {
				Cache.Refresh(InvalidType.Prefs);
			}
			catch(Exception ex) {
				if(isSilentUpdate) {
					ExitCode=100;//Database could not be accessed for cache refresh
					Environment.Exit(ExitCode);
					return false;
				}
				MessageBox.Show(ex.Message);
				return false;//shuts program down.
			}
			if(!PrefL.CheckMySqlVersion(isSilentUpdate)){
				return false;
			}
			if(DataConnection.DBtype==DatabaseType.MySql) {
				try {
					MiscData.SetSqlMode();
				}
				catch {
					if(isSilentUpdate) {
						ExitCode=111;//Global SQL mode could not be set
						Environment.Exit(ExitCode);
						return false;
					}
					MessageBox.Show("Unable to set global sql mode.  User probably does not have enough permission.");
					return false;
				}
				string updateComputerName=PrefC.GetStringSilent(PrefName.UpdateInProgressOnComputerName);
				if(updateComputerName != "" && Environment.MachineName.ToUpper() != updateComputerName.ToUpper()) {
					if(isSilentUpdate) {
						ExitCode=120;//Computer trying to access DB during update
						Environment.Exit(ExitCode);
						return false;
					}
					FormUpdateInProgress formUIP=new FormUpdateInProgress(updateComputerName);
					DialogResult result=formUIP.ShowDialog();
					if(result!=DialogResult.OK) {
						return false;//Either the user canceled out of the window or clicked the override button which 
					}
				}
			}
			//if RemotingRole.ClientWeb, version will have already been checked at login, so no danger here.
			//ClientWeb version can be older than this version, but that will be caught in a moment.
			if(isSilentUpdate) {
				if(!PrefL.ConvertDB(true,Application.ProductVersion,this)) {//refreshes Prefs if converted successfully.
					if(ExitCode==0) {//Unknown error occurred
						ExitCode=200;//Convert Database has failed during execution (Unknown Error)
					}
					Environment.Exit(ExitCode);
					return false;
				}
			}
			else {
				if(!PrefL.ConvertDB(this)) {//refreshes Prefs if converted successfully.
					return false;
				}
			}
			if(!isSilentUpdate) {
				PrefL.MySqlVersion55Remind();
			}
			if(!PrefL.CheckProgramVersion(isSilentUpdate)){
				return false;
			}
			if(!FormRegistrationKey.ValidateKey(PrefC.GetString(PrefName.RegistrationKey))){
				if(isSilentUpdate) {
					ExitCode=311;//Registration Key could not be validated
					Environment.Exit(ExitCode);
					return false;
				}
				FormRegistrationKey FormR=new FormRegistrationKey();
				FormR.ShowDialog();
				if(FormR.DialogResult!=DialogResult.OK){
					Environment.Exit(ExitCode);
					return false;
				}
				Cache.Refresh(InvalidType.Prefs);
			}
			ODThread regkeyThread=new ODThread(o => {
					RegKeyIsForTesting=PrefL.IsRegKeyForTesting();
					PatientL.GetMainTitleSamePat();
				}
			);
			regkeyThread.AddExceptionHandler((ex) => { });//silently fail.
			regkeyThread.Name="RegKeyIsForTestingThread";
			regkeyThread.Start(true);
			//This must be done at startup in case the user does not perform any action to save something to temp file.
			//This will cause slowdown, but only for the first week.
			if(DateTime.Today<PrefC.GetDate(PrefName.TempFolderDateFirstCleaned).AddDays(7)) {
				PrefC.GetTempFolderPath();//We don't care about the return value.  Just trying to trigger the one-time cleanup and create the temp/opendental directory.
			}
			Lans.RefreshCache();//automatically skips if current culture is en-US
			LanguageForeigns.RefreshCache();//automatically skips if current culture is en-US
			//menuItemMergeDatabases.Visible=PrefC.GetBool(PrefName.RandomPrimaryKeys");
			return true;
		}

		///<summary>Refreshes certain rarely used data from database.  Must supply the types of data to refresh as flags.  Also performs a few other tasks that must be done when local data is changed.</summary>
		private void RefreshLocalData(params InvalidType[] arrayITypes) {
			RefreshLocalData(true,arrayITypes);
		}
		
		///<summary>Refreshes certain rarely used data from database.  Must supply the types of data to refresh as flags.  Also performs a few other tasks that must be done when local data is changed.</summary>
		private void RefreshLocalData(bool doRefreshServerCache,params InvalidType[] arrayITypes) {
			if(arrayITypes==null || arrayITypes.Length==0) {
				return;//Just in case.
			}
			Cache.Refresh(doRefreshServerCache,arrayITypes);
			RefreshLocalDataPostCleanup(arrayITypes);
		}

		///<summary>Performs a few tasks that must be done when local data is changed.</summary>
		private void RefreshLocalDataPostCleanup(params InvalidType[] arrayITypes) {//This is where the flickering and reset of windows happens
			bool isAll=arrayITypes.Contains(InvalidType.AllLocal);
			#region IvalidType.Prefs
			if(arrayITypes.Contains(InvalidType.Prefs) || isAll) {
				if(PrefC.GetBool(PrefName.EasyHidePublicHealth)) {
					menuItemSchools.Visible=false;
					menuItemCounties.Visible=false;
					menuItemScreening.Visible=false;
				}
				else {
					menuItemSchools.Visible=true;
					menuItemCounties.Visible=true;
					menuItemScreening.Visible=true;
				}
				if(PrefC.GetBool(PrefName.EasyNoClinics)) {
					menuItemClinics.Visible=false;
					menuClinics.Visible=false;
				}
				else {
					menuItemClinics.Visible=true;
					menuClinics.Visible=true;
				}
				if(PrefC.GetBool(PrefName.EasyHideClinical)) {
					myOutlookBar.Buttons[4].Caption=Lan.g(this,"Procs");
				}
				else {
					myOutlookBar.Buttons[4].Caption=Lan.g(this,"Chart");
				}
				if(PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
					menuItemSchoolClass.Visible=false;
					menuItemSchoolCourses.Visible=false;
					menuItemDentalSchools.Visible=false;
					menuItemRequirementsNeeded.Visible=false;
					menuItemReqStudents.Visible=false;
					menuItemEvaluations.Visible=false;
				}
				else {
					menuItemSchoolClass.Visible=true;
					menuItemSchoolCourses.Visible=true;
					menuItemRequirementsNeeded.Visible=true;
					menuItemReqStudents.Visible=true;
				}
				if(PrefC.GetBool(PrefName.EasyHideRepeatCharges)) {
					menuItemRepeatingCharges.Visible=false;
				}
				else {
					menuItemRepeatingCharges.Visible=true;
				}
				if(PrefC.HasOnlinePaymentEnabled()) {
					menuItemPendingPayments.Visible=true;
					menuItemXWebTrans.Visible=true;
				}
				else {
					menuItemPendingPayments.Visible=false;
					menuItemXWebTrans.Visible=false;
				}
				if(PrefC.GetString(PrefName.DistributorKey)=="") {
					menuItemCustomerManage.Visible=false;
					menuItemNewCropBilling.Visible=false;
				}
				else {
					menuItemCustomerManage.Visible=true;
					menuItemNewCropBilling.Visible=true;
				}
				CheckCustomReports();
				if(NeedsRedraw("ChartModule")) {
					ContrChart2.InitializeLocalData();
				}
				if(NeedsRedraw("TaskLists")) {
					if(PrefC.GetBool(PrefName.TaskListAlwaysShowsAtBottom)) {//Refreshing task list here may not be the best course of action.
						//separate if statement to prevent database call if not showing task list at bottom to begin with
						//ComputerPref computerPref = ComputerPrefs.GetForLocalComputer();
						if(ComputerPrefs.LocalComputer.TaskKeepListHidden) {
							userControlTasks1.Visible = false;
						}
						else if(this.WindowState!=FormWindowState.Minimized) {//task list show and window is not minimized.
							userControlTasks1.Visible = true;
							userControlTasks1.InitializeOnStartup();
							if(ComputerPrefs.LocalComputer.TaskDock == 0) {//bottom
								menuItemDockBottom.Checked = true;
								menuItemDockRight.Checked = false;
								panelSplitter.Cursor=Cursors.HSplit;
								panelSplitter.Height=7;
								int splitterNewY=540;
								if(ComputerPrefs.LocalComputer.TaskY!=0) {
									splitterNewY=ComputerPrefs.LocalComputer.TaskY;
									if(splitterNewY<300) {
										splitterNewY=300;//keeps it from going too high
									}
									if(splitterNewY>ClientSize.Height-50) {
										splitterNewY=ClientSize.Height-panelSplitter.Height-50;//keeps it from going off the bottom edge
									}
								}
								panelSplitter.Location=new Point(myOutlookBar.Width,splitterNewY);
							}
							else {//right
								menuItemDockRight.Checked = true;
								menuItemDockBottom.Checked = false;
								panelSplitter.Cursor=Cursors.VSplit;
								panelSplitter.Width=7;
								int splitterNewX=900;
								if(ComputerPrefs.LocalComputer.TaskX!=0) {
									splitterNewX=ComputerPrefs.LocalComputer.TaskX;
									if(splitterNewX<300) {
										splitterNewX=300;//keeps it from going too far to the left
									}
									if(splitterNewX>ClientSize.Width-60) {
										splitterNewX=ClientSize.Width-panelSplitter.Width-60;//keeps it from going off the right edge
									}
								}
								panelSplitter.Location=new Point(splitterNewX,ToolBarMain.Height);
							}
						}
					}
					else {
						userControlTasks1.Visible = false;
					}
				}
				LayoutControls();
			}
			#endregion
			#region InvalidType.Signals
			if(arrayITypes.Contains(InvalidType.SigMessages) || isAll) {
				FillSignalButtons(null);
			}
			#endregion
			#region InvalidType.Programs
			if(arrayITypes.Contains(InvalidType.Programs) || isAll) {
				if(Programs.GetCur(ProgramName.PT).Enabled) {
					Bridges.PaperlessTechnology.InitializeFileWatcher();
				}
			}
			#endregion
			#region InvalidType.Programs OR InvalidType.Prefs
			if(arrayITypes.Contains(InvalidType.Programs) || arrayITypes.Contains(InvalidType.Prefs) || isAll) {
				if(PrefC.GetBool(PrefName.EasyBasicModules)) {
					myOutlookBar.Buttons[3].Visible=false;
					myOutlookBar.Buttons[5].Visible=false;
					myOutlookBar.Buttons[6].Visible=false;
				}
				else {
					myOutlookBar.Buttons[3].Visible=true;
					myOutlookBar.Buttons[5].Visible=true;
					myOutlookBar.Buttons[6].Visible=true;
				}
				if(Programs.UsingEcwTightOrFullMode()) {//has nothing to do with HL7
					if(ProgramProperties.GetPropVal(ProgramName.eClinicalWorks,"ShowImagesModule")=="1") {
						myOutlookBar.Buttons[5].Visible=true;
					}
					else {
						myOutlookBar.Buttons[5].Visible=false;
					}
				}
				if(Programs.UsingEcwTightMode()) {//has nothing to do with HL7
					myOutlookBar.Buttons[6].Visible=false;
				}
				if(Programs.UsingEcwTightOrFullMode()) {//old eCW interfaces
					if(Programs.UsingEcwTightMode()) {
						myOutlookBar.Buttons[0].Visible=false;//Appt
						myOutlookBar.Buttons[2].Visible=false;//Account
					}
					else if(Programs.UsingEcwFullMode()) {
						//We might create a special Appt module for eCW full users so they can access Recall.
						myOutlookBar.Buttons[0].Visible=false;//Appt
					}
				}
				else if(HL7Defs.IsExistingHL7Enabled()) {//There may be a def enabled as well as the old program link enabled. In this case, do not look at the def for whether or not to show the appt and account modules, instead go by the eCW interface enabled.
					HL7Def def=HL7Defs.GetOneDeepEnabled();
					myOutlookBar.Buttons[0].Visible=def.ShowAppts;//Appt
					myOutlookBar.Buttons[2].Visible=def.ShowAccount;//Account
				}
				else {//no def and not using eCW tight or full program link
					myOutlookBar.Buttons[0].Visible=true;//Appt
					myOutlookBar.Buttons[2].Visible=true;//Account
				}
				if(Programs.UsingOrion) {
					myOutlookBar.Buttons[0].Visible=false;//Appt module
					myOutlookBar.Buttons[2].Visible=false;//Account module
					myOutlookBar.Buttons[3].Visible=false;//TP module
				}
				myOutlookBar.Invalidate();
			}
			#endregion
			#region InvalidType.ToolBut
			if(arrayITypes.Contains(InvalidType.ToolBut) || isAll) {
				ContrAccount2.LayoutToolBar();
				ContrAppt2.LayoutToolBar();
				ContrChart2.LayoutToolBar();
				ContrImages2.LayoutToolBar();
				ContrFamily2.LayoutToolBar();
			}
			#endregion
			#region InvalidType.Views
			if(arrayITypes.Contains(InvalidType.Views) || isAll) {
				ContrAppt2.FillViews();
			}
			#endregion
			#region HQ Only
			if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {
				if(arrayITypes.Contains(InvalidType.Employees) || arrayITypes.Contains(InvalidType.Sites) || isAll) { 
					FillComboTriageCoordinator();
				}
			}
			#endregion
			//TODO: If there are still issues with TP refreshing, include TP prefs in needsRedraw()
			ContrTreat2.InitializeLocalData();//easier to leave this here for now than to split it.
			dictChartPrefsCache.Clear();
			dictTaskListPrefsCache.Clear();
			//Chart Drawing Prefs
			dictChartPrefsCache.Add(PrefName.DistributorKey.ToString(),PrefC.GetBool(PrefName.DistributorKey));
			dictChartPrefsCache.Add(PrefName.UseInternationalToothNumbers.ToString(),PrefC.GetInt(PrefName.UseInternationalToothNumbers));
			dictChartPrefsCache.Add("GraphicsUseHardware",ComputerPrefs.LocalComputer.GraphicsUseHardware);
			dictChartPrefsCache.Add("PreferredPixelFormatNum",ComputerPrefs.LocalComputer.PreferredPixelFormatNum);
			dictChartPrefsCache.Add("GraphicsSimple",ComputerPrefs.LocalComputer.GraphicsSimple);
			dictChartPrefsCache.Add(PrefName.ShowFeatureEhr.ToString(),PrefC.GetBool(PrefName.ShowFeatureEhr));
			dictChartPrefsCache.Add("DirectXFormat",ComputerPrefs.LocalComputer.DirectXFormat);
			//Task list drawing prefs
			dictTaskListPrefsCache.Add("TaskDock",ComputerPrefs.LocalComputer.TaskDock);
			dictTaskListPrefsCache.Add("TaskY",ComputerPrefs.LocalComputer.TaskY);
			dictTaskListPrefsCache.Add("TaskX",ComputerPrefs.LocalComputer.TaskX);
			dictTaskListPrefsCache.Add(PrefName.TaskListAlwaysShowsAtBottom.ToString(),PrefC.GetBool(PrefName.TaskListAlwaysShowsAtBottom));
			dictTaskListPrefsCache.Add(PrefName.TasksUseRepeating.ToString(),PrefC.GetBool(PrefName.TasksUseRepeating));
			dictTaskListPrefsCache.Add(PrefName.TasksNewTrackedByUser.ToString(),PrefC.GetBool(PrefName.TasksNewTrackedByUser));
			dictTaskListPrefsCache.Add(PrefName.TasksShowOpenTickets.ToString(),PrefC.GetBool(PrefName.TasksShowOpenTickets));
			dictTaskListPrefsCache.Add("TaskKeepListHidden",ComputerPrefs.LocalComputer.TaskKeepListHidden);
			if(Security.IsAuthorized(Permissions.UserQueryAdmin,true)) {
				menuItemReportsUserQuery.Text=Lan.g(this,"User Query");
			}
			else {
				menuItemReportsUserQuery.Text=Lan.g(this,"Released User Queries");
			}
		}

		private void FillComboTriageCoordinator() {
			ODException.SwallowAnyException(() => {
				comboTriageCoordinator.Items.Clear();
				//The following line will purposefully throw an exception if there is not a valid site link for the current IP octet.
				//We need the triage combo box to look incorrect so that we have a visual indicator to go fix our sites.
				Employee currentTriageCoordinator=Employees.GetEmp(SiteLinks.GetSiteLinkByIP().EmployeeNum);
				int iSelItem=-1;
				foreach(Employee emp in Employees.GetDeepCopy(true)) {
					int iNewItem=comboTriageCoordinator.Items.Add(new ODBoxItem<Employee>(Employees.GetNameFL(emp),emp));
					if(currentTriageCoordinator!=null && currentTriageCoordinator.EmployeeNum==emp.EmployeeNum) {
						iSelItem=iNewItem;
					}
				}
				if(iSelItem>=0) {
					comboTriageCoordinator.SelectedIndex=iSelItem;
				}
			});
		}

		///<summary>Compares preferences related to sections of the program that require redraws and returns true if a redraw is necessary, false otherwise.  If anything goes wrong with checking the status of any preference this method will return true.</summary>
		private bool NeedsRedraw(string section) {
			try {
				switch(section) {
					case "ChartModule":
						if(dictChartPrefsCache.Count==0
							|| PrefC.GetBool(PrefName.DistributorKey)!=(bool)dictChartPrefsCache["DistributorKey"]
							|| PrefC.GetInt(PrefName.UseInternationalToothNumbers)!=(int)dictChartPrefsCache["UseInternationalToothNumbers"]
							|| ComputerPrefs.LocalComputer.GraphicsUseHardware!=(bool)dictChartPrefsCache["GraphicsUseHardware"]
							|| ComputerPrefs.LocalComputer.PreferredPixelFormatNum!=(int)dictChartPrefsCache["PreferredPixelFormatNum"]
							|| ComputerPrefs.LocalComputer.GraphicsSimple!=(DrawingMode)dictChartPrefsCache["GraphicsSimple"]
							|| PrefC.GetBool(PrefName.ShowFeatureEhr)!=(bool)dictChartPrefsCache["ShowFeatureEhr"]
							|| ComputerPrefs.LocalComputer.DirectXFormat!=(string)dictChartPrefsCache["DirectXFormat"]) 
						{
							return true;
						}
						break;
					case "TaskLists":
						if(dictTaskListPrefsCache.Count==0
							|| ComputerPrefs.LocalComputer.TaskDock!=(int)dictTaskListPrefsCache["TaskDock"] //Checking for task list redrawing
							|| ComputerPrefs.LocalComputer.TaskY!=(int)dictTaskListPrefsCache["TaskY"]
							|| ComputerPrefs.LocalComputer.TaskX!=(int)dictTaskListPrefsCache["TaskX"]
							|| PrefC.GetBool(PrefName.TaskListAlwaysShowsAtBottom)!=(bool)dictTaskListPrefsCache["TaskListAlwaysShowsAtBottom"]
							|| PrefC.GetBool(PrefName.TasksUseRepeating)!=(bool)dictTaskListPrefsCache["TasksUseRepeating"]
							|| PrefC.GetBool(PrefName.TasksNewTrackedByUser)!=(bool)dictTaskListPrefsCache["TasksNewTrackedByUser"]
							|| PrefC.GetBool(PrefName.TasksShowOpenTickets)!=(bool)dictTaskListPrefsCache["TasksShowOpenTickets"]
							|| ComputerPrefs.LocalComputer.TaskKeepListHidden!=(bool)dictTaskListPrefsCache["TaskKeepListHidden"]) 
						{
							return true;
						}
						break;
					//case "TreatmentPlan":
					//	//If needed implement this section
					//	break;
				}//end switch
				return false;
			}
			catch {
				return true;//Should never happen.  Would most likely be caused by invalid preferences within the database.
			}
		}

		///<summary>Sets up the custom reports list in the main menu when certain requirements are met, or disables the custom reports menu item when those same conditions are not met. This function is called during initialization, and on the event that the A to Z folder usage has changed.</summary>
		private void CheckCustomReports(){
			menuItemCustomReports.MenuItems.Clear();
			//Try to load custom reports, but only if using the A to Z folders.
			if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {
				string imagePath=ImageStore.GetPreferredAtoZpath();
				string reportFolderName=PrefC.GetString(PrefName.ReportFolderName);
				string reportDir=ODFileUtils.CombinePaths(imagePath,reportFolderName);
				try {
					if(Directory.Exists(reportDir)) {
						DirectoryInfo infoDir=new DirectoryInfo(reportDir);
						FileInfo[] filesRdl=infoDir.GetFiles("*.rdl");
						for(int i=0;i<filesRdl.Length;i++) {
							string itemName=Path.GetFileNameWithoutExtension(filesRdl[i].Name);
							menuItemCustomReports.MenuItems.Add(itemName,new System.EventHandler(this.menuItemRDLReport_Click));
						}
					}
				}
				catch {
					MsgBox.Show(this,"Failed to retrieve custom reports.");
				}
			}
			if(menuItemCustomReports.MenuItems.Count==0) {
				menuItemCustomReports.Visible=false;
			}else{
				menuItemCustomReports.Visible=true;
			}
		}

		///<summary>Causes the toolbar to be laid out again.</summary>
		private void LayoutToolBar() {
			ToolBarMain.Buttons.Clear();
			ODToolBarButton button;
			button=new ODToolBarButton(Lan.g(this,"Select Patient"),0,"","Patient");
			button.Style=ODToolBarButtonStyle.DropDownButton;
			button.DropDownMenu=menuPatient;
			ToolBarMain.Buttons.Add(button);
			if(!Programs.UsingEcwTightMode()) {//eCW tight only gets Patient Select and Popups toolbar buttons
				button=new ODToolBarButton(Lan.g(this,"Commlog"),1,Lan.g(this,"New Commlog Entry"),"Commlog");
				if(PrefC.IsODHQ) {
					button.Style=ODToolBarButtonStyle.DropDownButton;
					button.DropDownMenu=menuCommlog;
				}
				ToolBarMain.Buttons.Add(button);
				button=new ODToolBarButton(Lan.g(this,"E-mail"),2,Lan.g(this,"Send E-mail"),"Email");
				button.Style=ODToolBarButtonStyle.DropDownButton;
				button.DropDownMenu=menuEmail;
				ToolBarMain.Buttons.Add(button);
				button=new ODToolBarButton(Lan.g(this,"WebMail"),2,Lan.g(this,"Secure WebMail"),"WebMail");
				button.Enabled=true;//Always enabled.  If the patient does not have an email address, then the user will be blocked from the FormWebMailMessageEdit window.
				ToolBarMain.Buttons.Add(button);
				if(_butText==null) {//If laying out again (after modifying setup), we keep the button to preserve the current notification text.
					_butText=new ODToolBarButton(Lan.g(this,"Text"),5,Lan.g(this,"Send Text Message"),"Text");
					_butText.Style=ODToolBarButtonStyle.DropDownButton;
					_butText.DropDownMenu=menuText;
					_butText.Enabled=Programs.IsEnabled(ProgramName.CallFire)||SmsPhones.IsIntegratedTextingEnabled();
					//The Notification text has not been set since startup.  We need an accurate starting count.
					if(SmsPhones.IsIntegratedTextingEnabled()) {
						SetSmsNotificationText();
					}
				}
				ToolBarMain.Buttons.Add(_butText);
				button=new ODToolBarButton(Lan.g(this,"Letter"),-1,Lan.g(this,"Quick Letter"),"Letter");
				button.Style=ODToolBarButtonStyle.DropDownButton;
				button.DropDownMenu=menuLetter;
				ToolBarMain.Buttons.Add(button);
				button=new ODToolBarButton(Lan.g(this,"Forms"),-1,"","Form");
				//button.Style=ODToolBarButtonStyle.DropDownButton;
				//button.DropDownMenu=menuForm;
				ToolBarMain.Buttons.Add(button);
				if(_butTask==null) {
					_butTask=new ODToolBarButton(Lan.g(this,"Tasks"),3,Lan.g(this,"Open Tasks"),"Tasklist");
					_butTask.Style=ODToolBarButtonStyle.DropDownButton;
					_butTask.DropDownMenu=menuTask;
				}
				ToolBarMain.Buttons.Add(_butTask);
				button=new ODToolBarButton(Lan.g(this,"Label"),4,Lan.g(this,"Print Label"),"Label");
				button.Style=ODToolBarButtonStyle.DropDownButton;
				button.DropDownMenu=menuLabel;
				ToolBarMain.Buttons.Add(button);
			}
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Popups"),-1,Lan.g(this,"Edit popups for this patient"),"Popups"));
			ProgramL.LoadToolbar(ToolBarMain,ToolBarsAvail.MainToolbar);
			Plugins.HookAddCode(this,"FormOpenDental.LayoutToolBar_end");
			ToolBarMain.Invalidate();
		}

		private void menuPatient_Popup(object sender,EventArgs e) {
			if(CurPatNum==0){
				return;
			}
			Family fam=Patients.GetFamily(CurPatNum);
			PatientL.AddFamilyToMenu(menuPatient,new EventHandler(menuPatient_Click),CurPatNum,fam);
		}

		private void ToolBarMain_ButtonClick(object sender,ODToolBarButtonClickEventArgs e) {
			if(e.Button.Tag.GetType()==typeof(string)) {
				//standard predefined button
				switch(e.Button.Tag.ToString()) {
					case "Patient":
						OnPatient_Click();
						break;
					case "Commlog":
						OnCommlog_Click();
						break;
					case "Email":
						OnEmail_Click();
						break;
					case "WebMail":
						OnWebMail_Click();
						break;
					case "Text":
						OnTxtMsg_Click(CurPatNum);
						break;
					case "Letter":
						OnLetter_Click();
						break;
					case "Form":
						OnForm_Click();
						break;
					case "Tasklist":
						OnTasks_Click();
						break;
					case "Label":
						OnLabel_Click();
						break;
					case "Popups":
						OnPopups_Click();
						break;
				}
			}
			else if(e.Button.Tag.GetType()==typeof(Program)) {
				ProgramL.Execute(((Program)e.Button.Tag).ProgramNum,Patients.GetPat(CurPatNum));
			}
		}

		private void OnPatient_Click() {
			FormPatientSelect formPS=new FormPatientSelect();
			formPS.ShowDialog();
			if(formPS.DialogResult==DialogResult.OK) {
				CurPatNum=formPS.SelectedPatNum;
				Patient pat=Patients.GetPat(CurPatNum);
				if(ContrChart2.Visible) {
					userControlTasks1.RefreshPatTicketsIfNeeded();//This is a special case.  Normally it's called in RefreshCurrentModule()
					ContrChart2.ModuleSelectedErx(CurPatNum);
				}
				else {
					RefreshCurrentModule();
				}
				if(ContrManage2.FormT!=null && !ContrManage2.FormT.IsDisposed) {
					ContrManage2.FormT.RefreshUserControlTasks();
				}
				FillPatientButton(pat);
				Plugins.HookAddCode(this,"FormOpenDental.OnPatient_Click_end");   
			}
		}

		private void menuPatient_Click(object sender,System.EventArgs e) {
			Family fam=Patients.GetFamily(CurPatNum);
			CurPatNum=PatientL.ButtonSelect(menuPatient,sender,fam);
			//new family now
			Patient pat=Patients.GetPat(CurPatNum);
			RefreshCurrentModule();
			FillPatientButton(pat);
		}

		///<summary>If the call to this is followed by ModuleSelected or GotoModule, set isRefreshCurModule=false to prevent the module from being
		///refreshed twice.  If the current module is ContrAppt and the call to this is preceded by a call to RefreshModuleDataPatient, set
		///isApptRefreshDataPat=false so the query to get the patient does not run twice.</summary>
		public static void S_Contr_PatientSelected(Patient pat,bool isRefreshCurModule,bool isApptRefreshDataPat=true) {
			_formOpenDentalS.Contr_PatientSelected(pat,isRefreshCurModule,isApptRefreshDataPat);
		}

		///<summary>Happens when any of the modules changes the current patient or when this main form changes the patient.  The calling module should
		///refresh itself.  The current patNum is stored here in the parent form so that when switching modules, the parent form knows which patient to
		///call up for that module.</summary>
		private void Contr_PatientSelected(Patient pat,bool isRefreshCurModule,bool isApptRefreshDataPat) {
			CurPatNum=pat.PatNum;
			if(isRefreshCurModule) {
				RefreshCurrentModule(false,isApptRefreshDataPat);
			}
			userControlTasks1.RefreshPatTicketsIfNeeded();
			FillPatientButton(pat);
		}

		///<Summary>Serves four functions.  
		///1. Sends the new patient to the dropdown menu for select patient.  
		///2. Changes which toolbar buttons are enabled.  
		///3. Sets main form text. 
		///4. Displays any popup.</Summary>
		private void FillPatientButton(Patient pat){
			if(pat==null) {
				pat=new Patient();
			}
			bool patChanged=PatientL.AddPatsToMenu(menuPatient,new EventHandler(menuPatient_Click),pat.GetNameLF(),pat.PatNum);
			if(patChanged){
				if(AutomationL.Trigger(AutomationTrigger.OpenPatient,null,pat.PatNum)) {//if a trigger happened
					if(ContrAppt2.Visible) {
						ContrAppt2.MouseUpForced();
					}
				}
			}
			if(ToolBarMain.Buttons==null || ToolBarMain.Buttons.Count<2){//on startup.  js Not sure why it's checking count.
				return;
			}
			if(CurPatNum==0) {//Only on startup, I think.
				if(!Programs.UsingEcwTightMode()) {//eCW tight only gets Patient Select and Popups toolbar buttons
					//We need a drafts folder the user can view saved emails in before we allow the user to save email without a patient selected.
					ToolBarMain.Buttons["Email"].Enabled=false;
					ToolBarMain.Buttons["WebMail"].Enabled=false;
					ToolBarMain.Buttons["Commlog"].Enabled=false;
					ToolBarMain.Buttons["Letter"].Enabled=false;
					ToolBarMain.Buttons["Form"].Enabled=false;
					ToolBarMain.Buttons["Tasklist"].Enabled=true;
					ToolBarMain.Buttons["Label"].Enabled=false;
				}
				ToolBarMain.Buttons["Popups"].Enabled=false;
			}
			else {
				if(!Programs.UsingEcwTightMode()) {//eCW tight only gets Patient Select and Popups toolbar buttons
					ToolBarMain.Buttons["Commlog"].Enabled=true;
					ToolBarMain.Buttons["Email"].Enabled=true;
					if(_butText!=null) {
						_butText.Enabled=Programs.IsEnabled(ProgramName.CallFire) || SmsPhones.IsIntegratedTextingEnabled();
					}
					ToolBarMain.Buttons["WebMail"].Enabled=true;
					ToolBarMain.Buttons["Letter"].Enabled=true;
					ToolBarMain.Buttons["Form"].Enabled=true;
					ToolBarMain.Buttons["Tasklist"].Enabled=true;
					ToolBarMain.Buttons["Label"].Enabled=true;
				}
				ToolBarMain.Buttons["Popups"].Enabled=true;
			}
			ToolBarMain.Invalidate();
			Text=PatientL.GetMainTitle(pat,Clinics.ClinicNum);
			if(PopupEventList==null){
				PopupEventList=new List<PopupEvent>();
			}
			if(Plugins.HookMethod(this,"FormOpenDental.FillPatientButton_popups",pat,PopupEventList,patChanged)) {
				return;
			}
			if(!patChanged) {
				return;
			}
			if(ContrChart2.Visible) {
				TryNonPatientPopup();
			}
			//New patient selected.  Everything below here is for popups.
			//First, remove all expired popups from the event list.
			for(int i=PopupEventList.Count-1;i>=0;i--){//go backwards
				if(PopupEventList[i].DisableUntil<DateTime.Now){//expired
					PopupEventList.RemoveAt(i);
				}
			}
			//Now, loop through all popups for the patient.
			List<Popup> popList=Popups.GetForPatient(pat.PatNum);//get all possible 
			for(int i=0;i<popList.Count;i++) {
				//skip any popups that are disabled because they are on the event list
				bool popupIsDisabled=false;
				for(int e=0;e<PopupEventList.Count;e++){
					if(popList[i].PopupNum==PopupEventList[e].PopupNum){
						popupIsDisabled=true;
						break;
					}
				}
				if(popupIsDisabled){
					continue;
				}
				//This popup is not disabled, so show it.
				//A future improvement would be to assemble all the popups that are to be shown and then show them all in one large window.
				//But for now, they will show in sequence.
				if(ContrAppt2.Visible) {
					ContrAppt2.MouseUpForced();
				}
				FormPopupDisplay FormP=new FormPopupDisplay();
				FormP.PopupCur=popList[i];
				FormP.ShowDialog();
				if(FormP.MinutesDisabled>0){
					PopupEvent popevent=new PopupEvent();
					popevent.PopupNum=popList[i].PopupNum;
					popevent.DisableUntil=DateTime.Now+TimeSpan.FromMinutes(FormP.MinutesDisabled);
					popevent.LastViewed=DateTime.Now;
					PopupEventList.Add(popevent);
					PopupEventList.Sort();
				}
			}
		}

		private void OnEmail_Click() {
			if(CurPatNum==0) {
				MsgBox.Show(this,"Please select a patient to send an email.");
				return;
			}
			if(!Security.IsAuthorized(Permissions.EmailSend)){
				return;
			}
			EmailMessage message=new EmailMessage();
			message.PatNum=CurPatNum;
			Patient pat=Patients.GetPat(CurPatNum);
			message.ToAddress=pat.Email;
			EmailAddress selectedAddress = EmailAddresses.GetNewEmailDefault(Security.CurUser.UserNum,pat.ClinicNum);
			message.FromAddress=selectedAddress.GetFrom();
			FormEmailMessageEdit FormE=new FormEmailMessageEdit(message,selectedAddress);
			FormE.IsNew=true;
			FormE.ShowDialog();
			if(FormE.DialogResult==DialogResult.OK) {
				RefreshCurrentModule();
			}
		}

		private void menuEmail_Popup(object sender,EventArgs e) {
			menuEmail.MenuItems.Clear();
			MenuItem menuItem;
			menuItem=new MenuItem(Lan.g(this,"Referrals:"));
			menuItem.Tag=null;
			menuEmail.MenuItems.Add(menuItem);
			List<RefAttach> refAttaches=RefAttaches.Refresh(CurPatNum);
			string referralDescript=DisplayFields.GetForCategory(DisplayFieldCategory.PatientInformation)
				.FirstOrDefault(x => x.InternalName=="Referrals")?.Description;
			if(string.IsNullOrWhiteSpace(referralDescript)) {//either not displaying the Referral field or no description entered, default to 'Referral'
				referralDescript=Lan.g(this,"Referral");
			}
			Referral refer;
			string str;
			for(int i=0;i<refAttaches.Count;i++) {
				if(!Referrals.TryGetReferral(refAttaches[i].ReferralNum,out refer)) {
					continue;
				}
				if(refAttaches[i].RefType==ReferralType.RefFrom) {
					str=Lan.g(this,"From");
				}
				else if(refAttaches[i].RefType==ReferralType.RefTo) {
					str=Lan.g(this,"To");
				}
				else {
					str=referralDescript;
				}
				str+=" "+Referrals.GetNameFL(refer.ReferralNum)+" <";
				if(refer.EMail==""){
					str+=Lan.g(this,"no email");
				}
				else{
					str+=refer.EMail;
				}
				str+=">";
				menuItem=new MenuItem(str,menuEmail_Click);
				menuItem.Tag=refer;
				menuEmail.MenuItems.Add(menuItem);
			}
		}

		private void OnWebMail_Click() {
			if(!Security.IsAuthorized(Permissions.WebMailSend)) {
				return;
			}
			FormWebMailMessageEdit FormWMME=new FormWebMailMessageEdit(CurPatNum);
			FormWMME.ShowDialog();
		}

		private void menuEmail_Click(object sender,System.EventArgs e) {
			if(((MenuItem)sender).Tag==null){
				return;
			}
			LabelSingle label=new LabelSingle();
			if(((MenuItem)sender).Tag.GetType()==typeof(Referral)) {
				Referral refer=(Referral)((MenuItem)sender).Tag;
				if(refer.EMail==""){
					return;
					//MsgBox.Show(this,"");
				}
				EmailMessage message=new EmailMessage();
				message.PatNum=CurPatNum;
				Patient pat=Patients.GetPat(CurPatNum);
				message.ToAddress=refer.EMail;//pat.Email;
				EmailAddress address=EmailAddresses.GetByClinic(pat.ClinicNum);
				message.FromAddress=address.GetFrom();
				message.Subject=Lan.g(this,"RE: ")+pat.GetNameFL();
				FormEmailMessageEdit FormE=new FormEmailMessageEdit(message,address);
				FormE.IsNew=true;
				FormE.ShowDialog();
				if(FormE.DialogResult==DialogResult.OK) {
					RefreshCurrentModule();
				}
			}
		}

		private void OnCommlog_Click() {
			if(Plugins.HookMethod(this,"FormOpenDental.OnCommlog_Click",CurPatNum)) {
				return;
			}
			FormCommItem FormCI=new FormCommItem();
			if(FormCI.ShowDialog(
				new CommItemModel() { CommlogCur=GetNewCommlog() },
				new CommItemController(FormCI) { IsNew=true })==DialogResult.OK) 
			{
				RefreshCurrentModule();
			}
		}

		private void menuItemCommlogPersistent_Click(object sender,EventArgs e) {
			FormCommItem FormCI=Application.OpenForms.OfType<FormCommItem>().FirstOrDefault(x => !x.IsDisposed);
			if(FormCI==null) {
				FormCI=new FormCommItem();
				//This manually connects the MVC wiring which will allow use to call Form.Show() below for either case.
				FormCI.Init(new CommItemModel() { CommlogCur=GetNewCommlog() },new CommItemController(FormCI) { IsPersistent=true });
			}
			if(FormCI.WindowState==FormWindowState.Minimized) {
				FormCI.WindowState=FormWindowState.Normal;
			}
			FormCI.Show();
			FormCI.BringToFront();
		}

		///<summary>This is a helper method to get a new commlog object for the commlog tool bar buttons.</summary>
		private Commlog GetNewCommlog() {
			Commlog commlog=new Commlog();
			commlog.PatNum=CurPatNum;
			commlog.CommDateTime=DateTime.Now;
			commlog.CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.MISC);
			if(PrefC.GetBool(PrefName.DistributorKey)) {//for OD HQ
				commlog.Mode_=CommItemMode.None;
				commlog.SentOrReceived=CommSentOrReceived.Neither;
			}
			else {
				commlog.Mode_=CommItemMode.Phone;
				commlog.SentOrReceived=CommSentOrReceived.Received;
			}
			commlog.UserNum=Security.CurUser.UserNum;
			return commlog;
		}

		private void OnLetter_Click() {
			FormSheetPicker FormS=new FormSheetPicker();
			FormS.SheetType=SheetTypeEnum.PatientLetter;
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK){
				return;
			}
			SheetDef sheetDef=FormS.SelectedSheetDefs[0];
			Sheet sheet=SheetUtil.CreateSheet(sheetDef,CurPatNum);
			SheetParameter.SetParameter(sheet,"PatNum",CurPatNum);
			//SheetParameter.SetParameter(sheet,"ReferralNum",referral.ReferralNum);
			SheetFiller.FillFields(sheet);
			SheetUtil.CalculateHeights(sheet);
			FormSheetFillEdit.ShowForm(sheet,FormSheetFillEdit_FormClosing);
			//Patient pat=Patients.GetPat(CurPatNum);
			//FormLetters FormL=new FormLetters(pat);
			//FormL.ShowDialog();
		}

		private void menuLetter_Popup(object sender,EventArgs e) {
			menuLetter.MenuItems.Clear();
			MenuItem menuItem;
			menuItem=new MenuItem(Lan.g(this,"Merge"),menuLetter_Click);
			menuItem.Tag="Merge";
			menuLetter.MenuItems.Add(menuItem);
			//menuItem=new MenuItem(Lan.g(this,"Stationery"),menuLetter_Click);
			//menuItem.Tag="Stationery";
			//menuLetter.MenuItems.Add(menuItem);
			menuLetter.MenuItems.Add("-");
			//Referrals---------------------------------------------------------------------------------------
			menuItem=new MenuItem(Lan.g(this,"Referrals:"));
			menuItem.Tag=null;
			menuLetter.MenuItems.Add(menuItem);
			string referralDescript=DisplayFields.GetForCategory(DisplayFieldCategory.PatientInformation)
				.FirstOrDefault(x => x.InternalName=="Referrals")?.Description;
			if(string.IsNullOrWhiteSpace(referralDescript)) {//either not displaying the Referral field or no description entered, default to 'Referral'
				referralDescript=Lan.g(this,"Referral");
			}
			List<RefAttach> refAttaches=RefAttaches.Refresh(CurPatNum);
			Referral refer;
			string str;
			for(int i=0;i<refAttaches.Count;i++) {
				if(!Referrals.TryGetReferral(refAttaches[i].ReferralNum,out refer)) {
					continue;
				}
				if(refAttaches[i].RefType==ReferralType.RefFrom) {
					str=Lan.g(this,"From");
				}
				else if(refAttaches[i].RefType==ReferralType.RefTo) {
					str=Lan.g(this,"To");
				}
				else {
					str=referralDescript;
				}
				str+=" "+Referrals.GetNameFL(refer.ReferralNum);
				menuItem=new MenuItem(str,menuLetter_Click);
				menuItem.Tag=refer;
				menuLetter.MenuItems.Add(menuItem);
			}
		}

		///<summary>If this is the computer that is set as the "Update Server Name", try to start all Open Dental services.</summary>
		private void StartODServices(ODThread odThread) {
			if(PrefC.GetString(PrefName.WebServiceServerName)!="" && ODEnvironment.IdIsThisComputer(PrefC.GetString(PrefName.WebServiceServerName))) {
				//An InvalidOperationException can get thrown if services could not start.  E.g. current user is not running Open Dental as an 
				//administrator.	We do not want to halt the startup sequence here.  If we want to notify customers of a downed service, there needs to 
				//be an additional monitoring service installed.
				ServicesHelper.StartServices(ServicesHelper.GetAllOpenDentServices());
			}
		}

		private void menuLetter_Click(object sender,System.EventArgs e) {
			if(((MenuItem)sender).Tag==null) {
				return;
			}
			Patient pat=Patients.GetPat(CurPatNum);
			if(((MenuItem)sender).Tag.GetType()==typeof(string)) {
				if(((MenuItem)sender).Tag.ToString()=="Merge") {
					FormLetterMerges FormL=new FormLetterMerges(pat);
					FormL.ShowDialog();
				}
				//if(((MenuItem)sender).Tag.ToString()=="Stationery") {
				//	FormCommunications.PrintStationery(pat);
				//}
			}
			if(((MenuItem)sender).Tag.GetType()==typeof(Referral)) {
				Referral refer=(Referral)((MenuItem)sender).Tag;
				FormSheetPicker FormS=new FormSheetPicker();
				FormS.SheetType=SheetTypeEnum.ReferralLetter;
				FormS.ShowDialog();
				if(FormS.DialogResult!=DialogResult.OK){
					return;
				}
				SheetDef sheetDef=FormS.SelectedSheetDefs[0];
				Sheet sheet=SheetUtil.CreateSheet(sheetDef,CurPatNum);
				SheetParameter.SetParameter(sheet,"PatNum",CurPatNum);
				SheetParameter.SetParameter(sheet,"ReferralNum",refer.ReferralNum);
				//Don't fill these params if the sheet doesn't use them.
				if(sheetDef.SheetFieldDefs.Any(x =>
					(x.FieldType==SheetFieldType.Grid && x.FieldName=="ReferralLetterProceduresCompleted")
					|| (x.FieldType==SheetFieldType.Special && x.FieldName=="toothChart")))
				{
					List<Procedure> listProcs=Procedures.GetCompletedForDateRange(sheet.DateTimeSheet,sheet.DateTimeSheet
						,listPatNums:new List<long>() { CurPatNum }
						,includeNote:true
						,includeGroupNote:true);
					if(sheetDef.SheetFieldDefs.Any(x => x.FieldType==SheetFieldType.Grid && x.FieldName=="ReferralLetterProceduresCompleted")) {
						SheetParameter.SetParameter(sheet,"CompletedProcs",listProcs);
					}
					if(sheetDef.SheetFieldDefs.Any(x => x.FieldType==SheetFieldType.Special && x.FieldName=="toothChart")) {
						SheetParameter.SetParameter(sheet,"toothChartImg",SheetPrinting.GetToothChartHelper(CurPatNum,false,listProceduresFilteredOverride:listProcs));
					}
				}
				SheetFiller.FillFields(sheet);
				SheetUtil.CalculateHeights(sheet);
				FormSheetFillEdit.ShowForm(sheet,FormSheetFillEdit_FormClosing);
				//FormLetters FormL=new FormLetters(pat);
				//FormL.ReferralCur=refer;
				//FormL.ShowDialog();
			}
		}

		/// <summary>Event handler for closing FormSheetFillEdit when it is non-modal.</summary>
		private void FormSheetFillEdit_FormClosing(object sender,FormClosingEventArgs e) {
			if(((FormSheetFillEdit)sender).DialogResult==DialogResult.OK) {
				RefreshCurrentModule();
			}
		}

		private void OnForm_Click() {
			FormPatientForms formP=new FormPatientForms();
			formP.PatNum=CurPatNum;
			formP.ShowDialog();
			//if(ContrAccount2.Visible || ContrChart2.Visible//The only two modules where a new form would show.
			//	|| ContrFamily2.Visible){//patient info
			//always refresh, especially to get the titlebar right after an import.
			Patient pat=Patients.GetPat(CurPatNum);
			RefreshCurrentModule();
			FillPatientButton(pat);
			//}
		}

		private void OnTasks_Click(){
			FormTaskListSelect FormT=new FormTaskListSelect(TaskObjectType.Patient);
			FormT.Location=new Point(50,50);
			FormT.Text=Lan.g(FormT,"Add Task")+" - "+FormT.Text;
			FormT.ShowDialog();
			if(FormT.DialogResult!=DialogResult.OK) {
				return;
			}
			Task task=new Task();
			task.TaskListNum=-1;//don't show it in any list yet.
			Tasks.Insert(task);
			Task taskOld=task.Copy();
			task.KeyNum=CurPatNum;
			task.ObjectType=TaskObjectType.Patient;
			task.TaskListNum=FormT.ListSelectedLists[0];
			task.UserNum=Security.CurUser.UserNum;
			FormTaskEdit FormTE=new FormTaskEdit(task,taskOld);
			FormTE.IsNew=true;
			FormTE.Show();
		}

		private void menuTask_Popup(object sender,EventArgs e) {
			menuItemTaskNewForUser.Text=Lan.g(this,"for")+" "+Security.CurUser.UserName;
			menuItemTaskReminders.Text=Lan.g(this,"Reminders");
			int reminderTaskNewCount=GetNewReminderTaskCount();
			if(reminderTaskNewCount > 0) {
				menuItemTaskReminders.Text+=" ("+reminderTaskNewCount+")";
			}
			int otherTaskCount=(_listNormalTaskNums!=null)?_listNormalTaskNums.Count:0;
			if(otherTaskCount > 0) {
				menuItemTaskNewForUser.Text+=" ("+otherTaskCount+")";
			}			
		}

		private void RefreshTasksNotification() {
			if(_butTask==null) {
				return;
			}
			Logger.LogToPath("",LogPath.Signals,LogPhase.Start);
			int otherTaskCount=(_listNormalTaskNums!=null)?_listNormalTaskNums.Count:0;
			int totalTaskCount=GetNewReminderTaskCount()+otherTaskCount;
			string notificationText="";
			if(totalTaskCount > 0) {
				notificationText=Math.Min(totalTaskCount,99).ToString();
			}
			if(notificationText!=_butTask.NotificationText) {
				_butTask.NotificationText=notificationText;
				ToolBarMain.Invalidate(_butTask.Bounds);//Cause the notification text on the Task button to update as soon as possible.
			}
			Logger.LogToPath("",LogPath.Signals,LogPhase.End);
		}

		private int GetNewReminderTaskCount() {
			if(_listReminderTasks==null) {
				return 0;
			}
			//Mimics how checkNew is set in FormTaskEdit.
			if(PrefC.GetBool(PrefName.TasksNewTrackedByUser)) {//Per definition of task.IsUnread.
				return _listReminderTasks.FindAll(x => x.IsUnread && x.DateTimeEntry<=DateTime.Now).Count;
			}
			return _listReminderTasks.FindAll(x => x.TaskStatus==TaskStatusEnum.New && x.DateTimeEntry<=DateTime.Now).Count;
		}

		private void menuItemTaskNewForUser_Click(object sender,EventArgs e) {
			ContrManage2.LaunchTaskWindow(false,UserControlTasksTab.ForUser);//Set the tab to the "for [User]" tab.
		}

		private void menuItemTaskReminders_Click(object sender,EventArgs e) {
			ContrManage2.LaunchTaskWindow(false,UserControlTasksTab.Reminders);//Set the tab to the "Reminders" tab
		}

		private delegate void ToolBarMainClick(long patNum);

		private void OnLabel_Click() {
			//The reason we are using a delegate and BeginInvoke() is because of a Microsoft bug that causes the Print Dialog window to not be in focus			
			//when it comes from a toolbar click.
			//https://social.msdn.microsoft.com/Forums/windows/en-US/681a50b4-4ae3-407a-a747-87fb3eb427fd/first-mouse-click-after-showdialog-hits-the-parent-form?forum=winforms
			ToolBarMainClick toolClick=LabelSingle.PrintPat;
			this.BeginInvoke(toolClick,CurPatNum);
		}

		private void menuLabel_Popup(object sender,EventArgs e) {
			menuLabel.MenuItems.Clear();
			MenuItem menuItem;
			List<SheetDef> LabelList=SheetDefs.GetCustomForType(SheetTypeEnum.LabelPatient);
			if(LabelList.Count==0){
				menuItem=new MenuItem(Lan.g(this,"LName, FName, Address"),menuLabel_Click);
				menuItem.Tag="PatientLFAddress";
				menuLabel.MenuItems.Add(menuItem);
				menuItem=new MenuItem(Lan.g(this,"Name, ChartNumber"),menuLabel_Click);
				menuItem.Tag="PatientLFChartNumber";
				menuLabel.MenuItems.Add(menuItem);
				menuItem=new MenuItem(Lan.g(this,"Name, PatNum"),menuLabel_Click);
				menuItem.Tag="PatientLFPatNum";
				menuLabel.MenuItems.Add(menuItem);
				menuItem=new MenuItem(Lan.g(this,"Radiograph"),menuLabel_Click);
				menuItem.Tag="PatRadiograph";
				menuLabel.MenuItems.Add(menuItem);
			}
			else{
				for(int i=0;i<LabelList.Count;i++) {
					menuItem=new MenuItem(LabelList[i].Description,menuLabel_Click);
					menuItem.Tag=LabelList[i];
					menuLabel.MenuItems.Add(menuItem);
				}
			}
			menuLabel.MenuItems.Add("-");
			//Carriers---------------------------------------------------------------------------------------
			Family fam=Patients.GetFamily(CurPatNum);
			List <PatPlan> PatPlanList=PatPlans.Refresh(CurPatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> PlanList=InsPlans.RefreshForSubList(subList);
			Carrier carrier;
			InsPlan plan;
			InsSub sub;
			for(int i=0;i<PatPlanList.Count;i++) {
				sub=InsSubs.GetSub(PatPlanList[i].InsSubNum,subList);
				plan=InsPlans.GetPlan(sub.PlanNum,PlanList);
				carrier=Carriers.GetCarrier(plan.CarrierNum);
				menuItem=new MenuItem(carrier.CarrierName,menuLabel_Click);
				menuItem.Tag=carrier;
				menuLabel.MenuItems.Add(menuItem);
			}
			menuLabel.MenuItems.Add("-");
			//Referrals---------------------------------------------------------------------------------------
			menuItem=new MenuItem(Lan.g(this,"Referrals:"));
			menuItem.Tag=null;
			menuLabel.MenuItems.Add(menuItem);
			string referralDescript=DisplayFields.GetForCategory(DisplayFieldCategory.PatientInformation)
				.FirstOrDefault(x => x.InternalName=="Referrals")?.Description;
			if(string.IsNullOrWhiteSpace(referralDescript)) {//either not displaying the Referral field or no description entered, default to 'Referral'
				referralDescript=Lan.g(this,"Referral");
			}
			List<RefAttach> refAttaches=RefAttaches.Refresh(CurPatNum);
			Referral refer;
			string str;
			for(int i=0;i<refAttaches.Count;i++){
				if(!Referrals.TryGetReferral(refAttaches[i].ReferralNum,out refer)) {
					continue;
				}
				if(refAttaches[i].RefType==ReferralType.RefFrom) {
					str=Lan.g(this,"From");
				}
				else if(refAttaches[i].RefType==ReferralType.RefTo) {
					str=Lan.g(this,"To");
				}
				else {
					str=referralDescript;
				}
				str+=" "+Referrals.GetNameFL(refer.ReferralNum);
				menuItem=new MenuItem(str,menuLabel_Click);
				menuItem.Tag=refer;
				menuLabel.MenuItems.Add(menuItem);
			}
		}

		private void menuLabel_Click(object sender,System.EventArgs e) {
			if(((MenuItem)sender).Tag==null) {
				return;
			}
			//LabelSingle label=new LabelSingle();
			if(((MenuItem)sender).Tag.GetType()==typeof(string)){
				if(((MenuItem)sender).Tag.ToString()=="PatientLFAddress"){
					LabelSingle.PrintPatientLFAddress(CurPatNum);
				}
				if(((MenuItem)sender).Tag.ToString()=="PatientLFChartNumber") {
					LabelSingle.PrintPatientLFChartNumber(CurPatNum);
				}
				if(((MenuItem)sender).Tag.ToString()=="PatientLFPatNum") {
					LabelSingle.PrintPatientLFPatNum(CurPatNum);
				}
				if(((MenuItem)sender).Tag.ToString()=="PatRadiograph") {
					LabelSingle.PrintPatRadiograph(CurPatNum);
				}
			}
			else if(((MenuItem)sender).Tag.GetType()==typeof(SheetDef)){
				LabelSingle.PrintCustomPatient(CurPatNum,(SheetDef)((MenuItem)sender).Tag);
			}
			else if(((MenuItem)sender).Tag.GetType()==typeof(Carrier)){
				Carrier carrier=(Carrier)((MenuItem)sender).Tag;
				LabelSingle.PrintCarrier(carrier.CarrierNum);
			}
			else if(((MenuItem)sender).Tag.GetType()==typeof(Referral)) {
				Referral refer=(Referral)((MenuItem)sender).Tag;
				LabelSingle.PrintReferral(refer.ReferralNum);
			}
		}

		private void OnPopups_Click() {
			FormPopupsForFam FormPFF=new FormPopupsForFam(PopupEventList);
			FormPFF.PatCur=Patients.GetPat(CurPatNum);
			FormPFF.ShowDialog();
		}

		#region SMS Text Messaging

		///<summary>Returns true if the message was sent successfully.</summary>
		public static bool S_OnTxtMsg_Click(long patNum,string startingText="") {
			return _formOpenDentalS.OnTxtMsg_Click(patNum,startingText);
		}

		///<summary>Called from the text message button and the right click context menu for an appointment. Returns true if the message was sent
		///successfully.</summary>
		private bool OnTxtMsg_Click(long patNum,string startingText="") {
			if(patNum==0) {
				FormTxtMsgEdit FormTxtME=new FormTxtMsgEdit();
				FormTxtME.Message=startingText;
				FormTxtME.PatNum=0;
				FormTxtME.ShowDialog();
				if(FormTxtME.DialogResult==DialogResult.OK) {
					RefreshCurrentModule();
					return true;
				}
				return false;
			}
			Patient pat=Patients.GetPat(patNum);
			bool updateTextYN=false;
			if(pat.TxtMsgOk==YN.No){
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"This patient is marked to not receive text messages. "
					+"Would you like to mark this patient as okay to receive text messages?")) 
				{
					updateTextYN=true;
				}
				else {
					return false;
				}
			}
			if(pat.TxtMsgOk==YN.Unknown && PrefC.GetBool(PrefName.TextMsgOkStatusTreatAsNo)){
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"This patient might not want to receive text messages. "
					+"Would you like to mark this patient as okay to receive text messages?")) 
				{
					updateTextYN=true;
				}
				else {
					return false;
				}
			}
			if(updateTextYN) {
				Patient patOld=pat.Copy();
				pat.TxtMsgOk=YN.Yes;
				Patients.Update(pat,patOld);
			}
			FormTxtMsgEdit FormTME=new FormTxtMsgEdit();
			FormTME.Message=startingText;
			FormTME.PatNum=patNum;
			FormTME.WirelessPhone=pat.WirelessPhone;
			FormTME.TxtMsgOk=pat.TxtMsgOk;
			FormTME.ShowDialog();
			if(FormTME.DialogResult==DialogResult.OK) {
				RefreshCurrentModule();
				return true;
			}
			return false;
		}

		private void menuItemTextMessagesReceived_Click(object sender,EventArgs e) {
			ShowFormTextMessagingModeless(false,true);
		}

		private void menuItemTextMessagesSent_Click(object sender,EventArgs e) {
			ShowFormTextMessagingModeless(true,false);
		}

		private void menuItemTextMessagesAll_Click(object sender,EventArgs e) {
			ShowFormTextMessagingModeless(true,true);
		}

		private void ShowFormTextMessagingModeless(bool isSent, bool isReceived) {
			if(_formSmsTextMessaging==null || _formSmsTextMessaging.IsDisposed) {
				_formSmsTextMessaging=new FormSmsTextMessaging(isSent,isReceived,() => { SetSmsNotificationText(); });
				_formSmsTextMessaging.FormClosed+=new FormClosedEventHandler((o,e) => { _formSmsTextMessaging=null; });
			}
			_formSmsTextMessaging.Show();
			_formSmsTextMessaging.BringToFront();
		}

		///<summary>Set signalSmsCount to null if you want to query the db for the current value and send a signal.
		///If responding to a signal then the structured data will be parsed from signalSmsCount.MsgValue and not new signal will be generated.</summary>
		private void SetSmsNotificationText(Signalod signalSmsCount=null) {
			if(_butText==null) {
				return;//This button does not exist in eCW tight integration mode.
			}
			try {
				if(!_butText.Enabled) {
					return;//This button is disabled when neither of the Text Messaging bridges have been enabled.
				}
				List<SmsFromMobiles.SmsNotification> listNotifications=null;
				if(signalSmsCount!=null) { //Try to pull structured data out of the signal directly. We will get null back if this fails.
					listNotifications=SmsFromMobiles.SmsNotification.GetListFromJson(signalSmsCount.MsgValue);
				}
				if(listNotifications==null) { //Notification not provided or signal was malformed. Either way recalculate and post a new signal.
					listNotifications=SmsFromMobiles.UpdateSmsNotification();
				}
				int smsUnreadCount=0;
				if(!PrefC.HasClinicsEnabled||Clinics.ClinicNum==0) {
					//No clinics or HQ clinic is active so sum them all.
					smsUnreadCount=listNotifications.Sum(x => x.Count);
				}
				else {
					//Only count the active clinic.
					smsUnreadCount=listNotifications.Where(x => x.ClinicNum==Clinics.ClinicNum).Sum(x => x.Count);
				}
				//Default to empty so we show nothing if there aren't any notifications.
				string smsNotificationText="";
				if(smsUnreadCount>99) { //We only have room in the UI for a 2-digit number.
					smsNotificationText="99";
				}
				else if(smsUnreadCount>0) { //We have a "real" number so show it.
					smsNotificationText=smsUnreadCount.ToString();
				}
				if(_butText.NotificationText==smsNotificationText) { //Prevent the toolbar from being invalidated unnecessarily.
					return;
				}
				_butText.NotificationText=smsNotificationText;
				if(menuItemTextMessagesReceived.Text.Contains("(")) {//Remove the old count from the menu item.
					menuItemTextMessagesReceived.Text=menuItemTextMessagesReceived.Text.Substring(0,menuItemTextMessagesReceived.Text.IndexOf("(")-1);
				}
				if(smsNotificationText!="") {
					menuItemTextMessagesReceived.Text+=" ("+smsNotificationText+")";
				}
			}
			finally { //Always redraw the toolbar item.
				ToolBarMain.Invalidate(_butText.Bounds);//To cause the Text button to redraw.			
			}
		}

		#endregion SMS Text Messaging

		private void RefreshMenuClinics() {
			menuClinics.MenuItems.Clear();
			List<Clinic> listClinics=Clinics.GetForUserod(Security.CurUser);
			if(listClinics.Count<30) { //This number of clinics will fit in a 990x735 form.
				MenuItem menuItem;
				if(!Security.CurUser.ClinicIsRestricted) {
					menuItem=new MenuItem(Lan.g(this,"Headquarters"),menuClinic_Click);
					menuItem.Tag=new Clinic();//Having a ClinicNum of 0 will make OD act like 'Headquarters'.  This allows the user to see unassigned appt views, all operatories, etc.
					if(Clinics.ClinicNum==0) {
						menuItem.Checked=true;
					}
					menuClinics.MenuItems.Add(menuItem);
					menuClinics.MenuItems.Add("-");//Separator
				}
				for(int i=0;i<listClinics.Count;i++) {
					menuItem=new MenuItem(listClinics[i].Abbr,menuClinic_Click);
					menuItem.Tag=listClinics[i];
					if(Clinics.ClinicNum==listClinics[i].ClinicNum) {
						menuItem.Checked=true;
					}
					menuClinics.MenuItems.Add(menuItem);
				}
			}
			else {//too many clinics to put in a menu drop down
				menuClinics.Click-=menuClick_OpenPickList;
				menuClinics.Click+=menuClick_OpenPickList;
			}
			RefreshLocalData(InvalidType.Views);//fills apptviews, sets the view, and then calls ContrAppt.ModuleSelected
			if(!ContrAppt2.Visible) {
				RefreshCurrentModule();//calls ModuleSelected of the current module, don't do this if ContrAppt2 is visible since it was just done above
			}
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) { //Changes the Chart and Treatment Plan icons to ones without teeth
				myOutlookBar.Buttons[3].ImageIndex=7;
				myOutlookBar.Buttons[4].ImageIndex=8;
			}
			else {//Change back to the normal Chart and Treatment Plan icons
				myOutlookBar.Buttons[3].ImageIndex=3;
				myOutlookBar.Buttons[4].ImageIndex=4;
			}
			myOutlookBar.Invalidate();
		}

		private void menuClick_OpenPickList(object sender,EventArgs e) {
			FormClinics FormC=new FormClinics();
			FormC.IsSelectionMode=true;
			if(!Security.CurUser.ClinicIsRestricted) {
				FormC.IncludeHQInList=true;
			}
			FormC.ShowDialog();
			if(FormC.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormC.SelectedClinicNum==0) {//'Headquarters' was selected.
				RefreshCurrentClinic(new Clinic());
				return;
			}
			Clinic clinicCur=Clinics.GetFirstOrDefault(x => x.ClinicNum==FormC.SelectedClinicNum);
			if(clinicCur!=null) { //Should never be null because the clinic should always be in the list
				RefreshCurrentClinic(clinicCur);
			}
			CheckAlerts();
		}

		///<summary>This is will set the private class wide variable _clinicNum and refresh the current module.</summary>
		private void menuClinic_Click(object sender,System.EventArgs e) {
			if(sender.GetType()!=typeof(MenuItem) && ((MenuItem)sender).Tag!=null) {
				return;
			}
			Clinic clinicCur=(Clinic)((MenuItem)sender).Tag;
			RefreshCurrentClinic(clinicCur);
		}

		///<summary>This is used to set the private class wide variable _clinicNum and refreshes the current module.</summary>
		private void RefreshCurrentClinic(Clinic clinicCur) {
			Clinics.ClinicNum=clinicCur.ClinicNum;
			SetSmsNotificationText();
			if(PrefC.GetBool(PrefName.AppointmentClinicTimeReset)) {
				AppointmentL.DateSelected=DateTimeOD.Today;
				if(AppointmentL.DateSelected.DayOfWeek==DayOfWeek.Sunday) {
					ContrAppt.WeekStartDate=AppointmentL.DateSelected.AddDays(-6).Date;//go back to previous monday
				}
				else {
					ContrAppt.WeekStartDate=AppointmentL.DateSelected.AddDays(1-(int)AppointmentL.DateSelected.DayOfWeek).Date;//go back to current monday
				}
				ContrAppt.WeekEndDate=ContrAppt.WeekStartDate.AddDays(ApptDrawing.NumOfWeekDaysToDisplay-1).Date;
			}
			RefreshMenuClinics();
		}

		private void FormOpenDental_Resize(object sender,EventArgs e) {
			LayoutControls();
			if(Plugins.PluginsAreLoaded) {
				Plugins.HookAddCode(this,"FormOpenDental.FormOpenDental_Resize_end");
			}
		}

		///<summary>This used to be called much more frequently when it was an actual layout event.</summary>
		private void LayoutControls(){
			//Debug.WriteLine("layout");
			if(this.WindowState==FormWindowState.Minimized) {
				return;
			}
			if(Width<200){
				Width=200;
			}
			Point position=new Point(myOutlookBar.Width,ToolBarMain.Height);
			int width=this.ClientSize.Width-position.X;
			int height=this.ClientSize.Height-position.Y;
			if(userControlTasks1.Visible) {
				if(menuItemDockBottom.Checked) {
					if(panelSplitter.Height>8) {//docking needs to be changed
						panelSplitter.Height=7;
						panelSplitter.Location=new Point(position.X,540);
					}
					panelSplitter.Location=new Point(position.X,panelSplitter.Location.Y);
					panelSplitter.Width=width;
					panelSplitter.Visible=true;
					if(PrefC.GetBool(PrefName.DockPhonePanelShow)){
						//phoneSmall.Visible=true;
						//phoneSmall.Location=new Point(position.X,panelSplitter.Bottom+butBigPhones.Height);
						phoneSmall.Location=new Point(0,comboTriageCoordinator.Bottom);
						userControlTasks1.Location=new Point(position.X+phoneSmall.Width,panelSplitter.Bottom);
						userControlTasks1.Width=width-phoneSmall.Width;
						//butBigPhones.Visible=true;
						//butBigPhones.Location=new Point(position.X+phoneSmall.Width-butBigPhones.Width,panelSplitter.Bottom);
						//butBigPhones.BringToFront();
						//labelMsg.Visible=true;
						//labelMsg.Location=new Point(position.X+phoneSmall.Width-butBigPhones.Width-labelMsg.Width,panelSplitter.Bottom);
						//labelMsg.BringToFront();
						panelPhoneSmall.Visible=true;
						panelPhoneSmall.Location=new Point(position.X,panelSplitter.Bottom);
						panelPhoneSmall.BringToFront();
					}
					else{
						//phoneSmall.Visible=false;
						//phonePanel.Visible=false;
						//butBigPhones.Visible=false;
						//labelMsg.Visible=false;
						panelPhoneSmall.Visible=false;
						userControlTasks1.Location=new Point(position.X,panelSplitter.Bottom);
						userControlTasks1.Width=width;
					}
					userControlTasks1.Height=this.ClientSize.Height-userControlTasks1.Top;
					height=ClientSize.Height-panelSplitter.Height-userControlTasks1.Height-ToolBarMain.Height;
				}
				else {//docked Right
					//phoneSmall.Visible=false;
					//phonePanel.Visible=false;
					//butBigPhones.Visible=false;
					//labelMsg.Visible=false;
					panelPhoneSmall.Visible=false;
					if(panelSplitter.Width>8) {//docking needs to be changed
						panelSplitter.Width=7;
						panelSplitter.Location=new Point(900,position.Y);
					}
					panelSplitter.Location=new Point(panelSplitter.Location.X,position.Y);
					panelSplitter.Height=height;
					panelSplitter.Visible=true;
					userControlTasks1.Location=new Point(panelSplitter.Right,position.Y);
					userControlTasks1.Height=height;
					userControlTasks1.Width=this.ClientSize.Width-userControlTasks1.Left;
					width=ClientSize.Width-panelSplitter.Width-userControlTasks1.Width-position.X;
				}
				panelSplitter.BringToFront();
				panelSplitter.Invalidate();
			}
			else {
				//phoneSmall.Visible=false;
				//phonePanel.Visible=false;
				//butBigPhones.Visible=false;
				//labelMsg.Visible=false;
				panelPhoneSmall.Visible=false;
				panelSplitter.Visible=false;
			}
			ContrAccount2.Location=position;
			ContrAccount2.Width=width;
			ContrAccount2.Height=height;
			ContrAppt2.Location=position;
			ContrAppt2.Width=width;
			ContrAppt2.Height=height;
			ContrChart2.Location=position;
			ContrChart2.Width=width;
			ContrChart2.Height=height;
			ContrImages2.Location=position;
			ContrImages2.Width=width;
			ContrImages2.Height=height;
			ContrFamily2.Location=position;
			ContrFamily2.Width=width;
			ContrFamily2.Height=height;
			ContrFamily2Ecw.Location=position;
			ContrFamily2Ecw.Width=width;
			ContrFamily2Ecw.Height=height;
			ContrManage2.Location=position;
			ContrManage2.Width=width;
			ContrManage2.Height=height;
			ContrTreat2.Location=position;
			ContrTreat2.Width=width;
			ContrTreat2.Height=height;
			FillSignalButtons(null);
		}

		private void panelSplitter_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			MouseIsDownOnSplitter=true;
			SplitterOriginalLocation=panelSplitter.Location;
			OriginalMousePos=new Point(panelSplitter.Left+e.X,panelSplitter.Top+e.Y);
		}

		private void panelSplitter_MouseMove(object sender,System.Windows.Forms.MouseEventArgs e) {
			if(!MouseIsDownOnSplitter){
				return;
			}
			if(menuItemDockBottom.Checked){
				int splitterNewY=SplitterOriginalLocation.Y+(panelSplitter.Top+e.Y)-OriginalMousePos.Y;
				if(splitterNewY<300){
					splitterNewY=300;//keeps it from going too high
				}
				if(splitterNewY>ClientSize.Height-50){
					splitterNewY=ClientSize.Height-panelSplitter.Height-50;//keeps it from going off the bottom edge
				}
				panelSplitter.Top=splitterNewY;
			}
			else{//docked right
				int splitterNewX=SplitterOriginalLocation.X+(panelSplitter.Left+e.X)-OriginalMousePos.X;
				if(splitterNewX<300) {
					splitterNewX=300;//keeps it from going too far to the left
				}
				if(splitterNewX>ClientSize.Width-60) {
					splitterNewX=ClientSize.Width-panelSplitter.Width-60;//keeps it from going off the right edge
				}
				panelSplitter.Left=splitterNewX;
			}
			LayoutControls();
		}

		private void panelSplitter_MouseUp(object sender,System.Windows.Forms.MouseEventArgs e) {
			MouseIsDownOnSplitter=false;
			TaskDockSavePos();
		}

		private void menuItemDockBottom_Click(object sender,EventArgs e) {
			menuItemDockBottom.Checked=true;
			menuItemDockRight.Checked=false;
			panelSplitter.Cursor=Cursors.HSplit;
			TaskDockSavePos();
			LayoutControls();
		}

		private void menuItemDockRight_Click(object sender,EventArgs e) {
			menuItemDockBottom.Checked=false;
			menuItemDockRight.Checked=true;
			//included now with layoutcontrols
			panelSplitter.Cursor=Cursors.VSplit;
			TaskDockSavePos();
			LayoutControls();
		}

		///<summary>Every time user changes doc position, it will save automatically.</summary>
		private void TaskDockSavePos(){
			//ComputerPref computerPref = ComputerPrefs.GetForLocalComputer();
			if(menuItemDockBottom.Checked){
				ComputerPrefs.LocalComputer.TaskY = panelSplitter.Top;
				ComputerPrefs.LocalComputer.TaskDock = 0;
			}
			else{
				ComputerPrefs.LocalComputer.TaskX = panelSplitter.Left;
				ComputerPrefs.LocalComputer.TaskDock = 1;
			}
			ComputerPrefs.Update(ComputerPrefs.LocalComputer);
		}

		public static void S_DataValid_BecomeInvalid(OpenDental.ValidEventArgs e) {
			_formOpenDentalS?.DataValid_BecameInvalid(e);//Can be null if called from other projects like CEMT
		}
		
		///<summary>This is called when any local data becomes outdated.  It's purpose is to tell the other computers to update certain local data.</summary>
		private void DataValid_BecameInvalid(OpenDental.ValidEventArgs e){
			if(e.OnlyLocal){//Currently used after doing a restore from FormBackup so that the local cache is forcefully updated.
				if(!PrefsStartup()){//??
					return;
				}
				RefreshLocalData(InvalidType.AllLocal);//does local computer only
				return;
			}
			if(!e.ITypes.Contains(InvalidType.Appointment) //local refresh for dates is handled within ContrAppt, not here
				&& !e.ITypes.Contains(InvalidType.Task)//Tasks are not "cached" data.
				&& !e.ITypes.Contains(InvalidType.TaskPopup))
			{
				RefreshLocalData(e.ITypes);//does local computer
			}
			if(e.ITypes.Contains(InvalidType.Task) || e.ITypes.Contains(InvalidType.TaskPopup)) {
				Plugins.HookAddCode(this,"FormOpenDental.DataValid_BecameInvalid_taskInvalidTypes");
				//One of the two task lists needs to be refreshed on this instance of OD
				if(userControlTasks1.Visible) {
					userControlTasks1.RefreshTasks();
				}
				//See if FormTasks is currently open.
				if(ContrManage2!=null && ContrManage2.FormT!=null && !ContrManage2.FormT.IsDisposed) {
					ContrManage2.FormT.RefreshUserControlTasks();
				}
				if(ContrChart2?.Visible??false) {
					ContrChart2.FillProgNotes(true);
				}
			}
			foreach(InvalidType iType in e.ITypes) {
				Signalod sig=new Signalod();
				sig.IType=iType;
				if(iType==InvalidType.Task || iType==InvalidType.TaskPopup) {
					sig.FKey=e.TaskNum;
					sig.FKeyType=KeyType.Task;
				}
				Signalods.Insert(sig);
			}
		}

		///<summary>Referenced at least 40 times indirectly.</summary>
		public static void S_GotoModule_ModuleSelected(ModuleEventArgs e) {
			_formOpenDentalS.GotoModule_ModuleSelected(e);
		}

		///<summary>This is a way that any form within Open Dental can ask the main form to refresh whatever module is currently selected.</summary>
		public static void S_RefreshCurrentModule(bool hasForceRefresh=false,bool isApptRefreshDataPat=true,bool isClinicRefresh=true) {
			_formOpenDentalS.RefreshCurrentModule(hasForceRefresh,isApptRefreshDataPat,isClinicRefresh);
		}

		private void GotoModule_ModuleSelected(ModuleEventArgs e){
			if(e.DateSelected.Year>1880){
				AppointmentL.DateSelected=e.DateSelected;
			}
			if(e.SelectedAptNum>0){
				ContrApptSingle.SelectedAptNum=e.SelectedAptNum;
			}
			//patient can also be set separately ahead of time instead of doing it this way:
			if(e.PatNum !=0) {
				CurPatNum=e.PatNum;
				Patient pat=Patients.GetPat(CurPatNum);
				FillPatientButton(pat);
			}
			UnselectActive();
			allNeutral();
			if(e.ClaimNum>0){
				myOutlookBar.SelectedIndex=e.IModule;
				ContrAccount2.Visible=true;
				this.ActiveControl=this.ContrAccount2;
				ContrAccount2.ModuleSelected(CurPatNum,e.ClaimNum);
			}
			else if(e.ListPinApptNums.Count!=0){
				myOutlookBar.SelectedIndex=e.IModule;
				ContrAppt2.Visible=true;
				this.ActiveControl=this.ContrAppt2;
				ContrAppt2.ModuleSelectedWithPinboard(CurPatNum,e.ListPinApptNums);
			}
			else if(e.DocNum>0) {
				myOutlookBar.SelectedIndex=e.IModule;
				ContrImages2.Visible=true;
				this.ActiveControl=this.ContrImages2;
				ContrImages2.ModuleSelected(CurPatNum,e.DocNum);
			}
			else if(e.IModule!=-1){
				myOutlookBar.SelectedIndex=e.IModule;
				SetModuleSelected();
			}
			myOutlookBar.Invalidate();
		}

		///<summary>If this is initial call when opening program, then set sigListButs=null.
		///This will cause a call to be made to database to get current status of buttons.
		///Otherwise, it adds the signals passed in to the current state, then paints.</summary>
		private void FillSignalButtons(List<SigMessage> listSigMessages) {
			if(!Security.IsUserLoggedIn){
				return;
			}
			if(!lightSignalGrid1.Visible && Programs.UsingEcwTightOrFullMode()) {//for faster eCW loading
				return;
			}
			if(listSigMessages==null) {
				SigButDefList=SigButDefs.GetByComputer(SystemInformation.ComputerName);//initially set from FormOpenDental_Load
				lightSignalGrid1.SetButtons(SigButDefList);
				listSigMessages=SigMessages.RefreshCurrentButState();
			}
			lightSignalGrid1.Visible=(SigButDefList?.Length??0)>0;
			int maxButton = SigButDefList.Select(x => x.ButtonIndex).DefaultIfEmpty(-1).Max()+1;
			int lightGridHeightOld = lightSignalGrid1.Height;
			int lightGridHeightNew = Math.Min(maxButton*25+1,this.ClientRectangle.Height-lightSignalGrid1.Location.Y);
			if(lightGridHeightOld!=lightGridHeightNew) {
				lightSignalGrid1.Visible=false;//"erases" light signal grid that has been drawn on FormOpenDental
				lightSignalGrid1.Height=lightGridHeightNew;
				lightSignalGrid1.Visible=true;//re-draws light signal grid to the correct size.
			}
			SigButDef butDef;
			int row;
			Color color;
			for(int i=0;i<listSigMessages.Count;i++){
				if(listSigMessages[i].AckDateTime.Year > 1880) {//process ack
					int buttonIndex=lightSignalGrid1.ProcessAck(listSigMessages[i].SigMessageNum);
					if(buttonIndex!=-1) {
						butDef=SigButDefs.GetByIndex(buttonIndex,SigButDefList);
						if(butDef!=null) {
							PaintOnIcon(butDef.SynchIcon,Color.White);
						}
					}
				}
				else {//process normal message
					row=0;
					color=Color.White;
					List<SigElementDef> listSigElementDefs=SigElementDefs.GetDefsForSigMessage(listSigMessages[i]);
					foreach(SigElementDef sigElementDef in listSigElementDefs) {
						if(sigElementDef.LightRow!=0) {
							row=sigElementDef.LightRow;
						}
						if(sigElementDef.LightColor.ToArgb()!=Color.White.ToArgb()) {
							color=sigElementDef.LightColor;
						}
					}
					if(row!=0 && color!=Color.White) {
						lightSignalGrid1.SetButtonActive(row-1,color,listSigMessages[i]);
						butDef=SigButDefs.GetByIndex(row-1,SigButDefList);
						if(butDef!=null){
							try{
								PaintOnIcon(butDef.SynchIcon,color);
							}
							catch{
								MessageBox.Show("Error painting on program icon.  Probably too many non-ack'd messages.");
							}
						}
					}
				}
			}
		}

		///<summary>Pass in the cellNum as 1-based.</summary>
		private void PaintOnIcon(int cellNum,Color color){
			Graphics g;
			if(bitmapIcon==null){
				bitmapIcon=new Bitmap(16,16);
				g=Graphics.FromImage(bitmapIcon);
				g.FillRectangle(new SolidBrush(Color.White),0,0,15,15);
				//horizontal
				g.DrawLine(Pens.Black,0,0,15,0);
				g.DrawLine(Pens.Black,0,5,15,5);
				g.DrawLine(Pens.Black,0,10,15,10);
				g.DrawLine(Pens.Black,0,15,15,15);
				//vertical
				g.DrawLine(Pens.Black,0,0,0,15);
				g.DrawLine(Pens.Black,5,0,5,15);
				g.DrawLine(Pens.Black,10,0,10,15);
				g.DrawLine(Pens.Black,15,0,15,15);
				g.Dispose();
			}
			if(cellNum==0){
				return;
			}
			g=Graphics.FromImage(bitmapIcon);
			int x=0;
			int y=0;
			switch(cellNum){
				case 1: x=1; y=1; break;
				case 2: x=6; y=1; break;
				case 3: x=11; y=1; break;
				case 4: x=1; y=6; break;
				case 5: x=6; y=6; break;
				case 6: x=11; y=6; break;
				case 7: x=1; y=11; break;
				case 8: x=6; y=11; break;
				case 9: x=11; y=11; break;
			}
			g.FillRectangle(new SolidBrush(color),x,y,4,4);
			IntPtr intPtr=bitmapIcon.GetHicon();
			Icon icon=Icon.FromHandle(intPtr);
			Icon=(Icon)icon.Clone();
			DestroyIcon(intPtr);
			icon.Dispose();
			g.Dispose();
		}

		[System.Runtime.InteropServices.DllImport("user32.dll",CharSet = CharSet.Auto)]
		extern static bool DestroyIcon(IntPtr handle);

		private void lightSignalGrid1_ButtonClick(object sender,OpenDental.UI.ODLightSignalGridClickEventArgs e) {
			if(e.ActiveSignal!=null) {//user trying to ack an existing light signal
				SigMessages.AckButton(e.ButtonIndex+1,Signalods.SignalLastRefreshed);
				//then, manually ack the light on this computer.  The second ack in a few seconds will be ignored.
				lightSignalGrid1.SetButtonActive(e.ButtonIndex,Color.White,null);
				SigButDef butDef=SigButDefs.GetByIndex(e.ButtonIndex,SigButDefList);
				if(butDef!=null) {
					PaintOnIcon(butDef.SynchIcon,Color.White);
				}
				//We do not want to invalidate the SigElementDef cache here. That caused slowness with messaging buttons.
				return;
			}
			if(e.ButtonDef==null || (e.ButtonDef.SigElementDefNumUser==0 && e.ButtonDef.SigElementDefNumExtra==0 && e.ButtonDef.SigElementDefNumMsg==0)) {
				return;//There is no signal to send.
			}
			//user trying to send a signal
			SigMessage sigMessage=new SigMessage();
			//need to do this all as a transaction?
			int row=0;
			Color color=Color.White;
			sigMessage.SigElementDefNumUser= e.ButtonDef.SigElementDefNumUser ;
			sigMessage.SigElementDefNumExtra=e.ButtonDef.SigElementDefNumExtra;
			sigMessage.SigElementDefNumMsg=  e.ButtonDef.SigElementDefNumMsg  ;
			//Document: Preference for row color and row number is Msg, Extra, User; In that order.
			SigElementDef sigElementDefUser=SigElementDefs.GetElementDef(e.ButtonDef.SigElementDefNumUser);
			if(sigElementDefUser!=null) {
				sigMessage.ToUser=sigElementDefUser.SigText;
				if(sigElementDefUser.LightRow!=0) {
					row=sigElementDefUser.LightRow;
				}
				if(sigElementDefUser.LightColor.ToArgb()!=Color.White.ToArgb()) {
					color=sigElementDefUser.LightColor;
				}
			}
			SigElementDef sigElementDefExtra=SigElementDefs.GetElementDef(e.ButtonDef.SigElementDefNumExtra);
			if(sigElementDefExtra!=null) {
				if(sigElementDefExtra.LightRow!=0) {
					row=sigElementDefExtra.LightRow;
				}
				if(sigElementDefExtra.LightColor.ToArgb()!=Color.White.ToArgb()) {
					color=sigElementDefExtra.LightColor;
				}
			}
			SigElementDef sigElementDefMsg=SigElementDefs.GetElementDef(e.ButtonDef.SigElementDefNumMsg);
			if(sigElementDefMsg!=null) {
				if(sigElementDefMsg.LightRow!=0) {
					row=sigElementDefMsg.LightRow;
				}
				if(sigElementDefMsg.LightColor.ToArgb()!=Color.White.ToArgb()) {
					color=sigElementDefMsg.LightColor;
				}
			}
			if(row!=0 && color!=Color.White) {
				lightSignalGrid1.SetButtonActive(row-1,color,sigMessage);//this just makes it seem more responsive.
				//we can skip painting on the icon
			}
			SigMessages.Insert(sigMessage);
			//Let the other computers in the office know to refresh this specific light.
			Signalod signal=new Signalod();
			signal.IType=InvalidType.SigMessages;
			signal.FKeyType=KeyType.SigMessage;
			signal.FKey=sigMessage.SigMessageNum;
			Signalods.Insert(signal);
		}

		private void timerTimeIndic_Tick(object sender,System.EventArgs e) {
			//every minute:
			if(WindowState!=FormWindowState.Minimized && ContrAppt2.Visible) {
				ContrAppt2.TickRefresh();
			}
		}

		///<summary>Usually set at 4 to 6 second intervals.</summary>
		private void timerSignals_Tick(object sender,System.EventArgs e) {
			try {
				Logger.LogToPath("",LogPath.Signals,LogPhase.Start);
				//This checks if any forms are open that make us want to continue processing signals even if inactive. Currently only FormTerminal.
				if(Application.OpenForms.OfType<Form>().All(x => x.Name!="FormTerminal")) {
					DateTime dtInactive=Security.DateTimeLastActivity+TimeSpan.FromMinutes((double)PrefC.GetInt(PrefName.SignalInactiveMinutes));
					if((double)PrefC.GetInt(PrefName.SignalInactiveMinutes)!=0 && DateTime.Now>dtInactive) {
						return;
					}
				}
				if(Security.CurUser==null) {
					//User must be at the log in screen, so no need to process signals. We will need to look for shutdown signals since the last refreshed time when the user attempts to log in.
					return;
				}
				CheckAlerts();
			}
			catch {
				//Currently do nothing.
			}
			#region Task Preprocessing
			if(_tasksUserNum!=Security.CurUser.UserNum //The user has changed since the last signal tick was run (when logoff then logon),
				|| _listReminderTasks==null || _listNormalTaskNums==null)//or first time processing signals since the program started.
			{
				Logger.LogToPath("CurUser change",LogPath.Signals,LogPhase.Start);
				_tasksUserNum=Security.CurUser.UserNum;
				List<Task> listRefreshedTasks=Tasks.GetNewTasksThisUser(Security.CurUser.UserNum);//Get all tasks pertaining to current user.
				_listNormalTaskNums=new List<long>();
				_listReminderTasks=new List<Task>();
				List<UserOdPref> listBlockedTaskLists=UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.TaskListBlock);
				foreach(Task taskForUser in listRefreshedTasks) {//Construct the initial task meta data for the current user's tasks.
					bool isTrackedByUser=PrefC.GetBool(PrefName.TasksNewTrackedByUser);
					if(String.IsNullOrEmpty(taskForUser.ReminderGroupId)) {//A normal task.
						//Mimics how checkNew is set in FormTaskEdit.
						if((isTrackedByUser && taskForUser.IsUnread) || (!isTrackedByUser && taskForUser.TaskStatus==TaskStatusEnum.New)) {//See def of task.IsUnread
							_listNormalTaskNums.Add(taskForUser.TaskNum);
						}
					}
					else if(!PrefC.GetBool(PrefName.TasksUseRepeating)) {//A reminder task (new or viewed).  Reminders not allowed if repeating tasks enabled.
						_listReminderTasks.Add(taskForUser);
						if(taskForUser.DateTimeEntry<=DateTime.Now) {//Do not show reminder popups for future reminders which are not due yet.
							//Mimics how checkNew is set in FormTaskEdit.
							if((isTrackedByUser && taskForUser.IsUnread) || (!isTrackedByUser && taskForUser.TaskStatus==TaskStatusEnum.New)) {//See def of task.IsUnread
								//NOTE: POPUPS ONLY HAPPEN IF THEY ARE MARKED AS NEW. (Also, they will continue to pop up as long as they are marked "new")
								TaskPopupHelper(taskForUser,listBlockedTaskLists);
							}
						}
					}
				}
				//Refresh the appt module to show the current list of reminders, even if the appt module not visible.  This refresh is fast.
				//The user will load the appt module eventually and these refreshes are the only updates the appointment module receives for reminders.
				ContrAppt2.RefreshReminders(_listReminderTasks);
				_dateReminderRefresh=DateTimeOD.Today;
				Logger.LogToPath("CurUser change",LogPath.Signals,LogPhase.End);
			}
			//Check to see if a reminder task became due between the last signal interval and the current signal interval.
			else if(_listReminderTasks.FindAll(x => x.DateTimeEntry <= DateTime.Now
				&& x.DateTimeEntry >= DateTime.Now.AddSeconds(-PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs))).Count > 0)
			{
				Logger.LogToPath("Reminder task due",LogPath.Signals,LogPhase.Start);
				//One of the two task lists needs to be refreshed on this instance of OD
				if(userControlTasks1.Visible) {
					userControlTasks1.RefreshTasks();
				}
				//See if FormTasks is currently open.
				if(ContrManage2!=null && ContrManage2.FormT!=null && !ContrManage2.FormT.IsDisposed) {
					ContrManage2.FormT.RefreshUserControlTasks();
				}
				List<UserOdPref> listBlockedTaskLists=UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.TaskListBlock);
				List<Task> listDueReminderTasks=_listReminderTasks.FindAll(x => x.DateTimeEntry <= DateTime.Now
					&& x.DateTimeEntry >= DateTime.Now.AddSeconds(-PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs)));
				foreach(Task reminderTask in listDueReminderTasks) {
					TaskPopupHelper(reminderTask,listBlockedTaskLists);
				}
				Logger.LogToPath("Reminder task due",LogPath.Signals,LogPhase.End);
			}
			else if(_dateReminderRefresh.Date < DateTimeOD.Today) {
				Logger.LogToPath("Daily reminder refresh is due",LogPath.Signals,LogPhase.Unspecified);
				//Refresh the appt module to show the current list of reminders, even if the appt module not visible.  This refresh is fast.
				//The user will load the appt module eventually and these refreshes are the only updates the appointment module receives for reminders.
				ContrAppt2.RefreshReminders(_listReminderTasks);
				_dateReminderRefresh=DateTimeOD.Today;
			}
			RefreshTasksNotification();
			#endregion Task Preprocessing
			//Signal Processing
			timerSignals.Stop();
			var processSignalForms=new Action<List<ISignalProcessor>,List<Signalod>>((listISignalProcessors,listSignals) => {
				//Broadcast to all subscribed signal processors.
				this.Invoke((() => {
					listISignalProcessors.ForEach(x => {
						try {
							x.ProcessObjects(listSignals);
						}
						catch(Exception ex) {
							ex.DoNothing();
						}
					});
				}));
			});
			Signalods.SignalsTick(new Action(() => this.Invoke(OnShutdown)),processSignalForms,new Action(() => this.Invoke(timerSignals.Start)));
			//Be careful about doing anything that takes a long amount of computation time after the SignalsTick.
			//The UI will appear invalid for the time it takes any methods to process.
			//Post Signal Processing
			if(PrefC.IsODHQ) {//No actual signals are sent, so this must happen independantly from SignalsTick.
				Logger.LogToPath("PhoneConf",LogPath.Signals,LogPhase.Start);
				ODThread threadPhoneConfs=new ODThread(new ODThread.WorkerDelegate((o) => {
					List<PhoneConf> listPhoneConfs=PhoneConfs.GetAll();
					this.Invoke((() => lightSignalGrid1.SetConfs(listPhoneConfs)));
				Logger.LogToPath("PhoneConf",LogPath.Signals,LogPhase.End);
				}));
				threadPhoneConfs.Start(true);
			}
			//STOP! 
			//If you are trying to do something in FormOpenDental that uses a signal, you should use FormOpenDental.OnProcessSignals() instead.
			//This Function is only for processing things at regular intervals IF IT DOES NOT USE SIGNALS.
			Logger.LogToPath("",LogPath.Signals,LogPhase.End);
		}
		
		///<summary>Called when we want to update the main toolbars 'Alert' menu.
		///Only shows Alerts that the current user is subscribed to for the current clinic.</summary>
		private void CheckAlerts(bool doRunOnThread=true) {
			long clinicNumCur=Clinics.ClinicNum;
			long userNumCur=Security.CurUser.UserNum;
			List<AlertItem> listAlertItems=new List<AlertItem>();
			List<AlertRead> listAlertItemReads=new List<AlertRead>();
			ODThread.WorkerDelegate getAlerts=new ODThread.WorkerDelegate((o) => {
				Logger.LogToPath("",LogPath.Signals,LogPhase.Start);
			//List of AlertSubs for current clinic and user combo.
			List<AlertSub> listUserAlertSubs=AlertSubs.GetAllForUser(userNumCur,clinicNumCur);
			List<long> listAlertCatNums=listUserAlertSubs.Select(y => y.AlertCategoryNum).ToList();
			//AlertTypes current user is subscribed to.
			List<AlertType> listUserAlertLinks=AlertCategoryLinks.GetWhere(x => listAlertCatNums.Contains(x.AlertCategoryNum))
				.Select(x => x.AlertType).ToList();
				listAlertItems=AlertItems.RefreshForClinicAndTypes(clinicNumCur,listUserAlertLinks);
				//Update listUserAlertTypes to only those with active AlertItems.
				listUserAlertLinks=listAlertItems.Select(x => x.Type).ToList();
				listAlertItemReads=AlertReads.RefreshForAlertNums(userNumCur,listAlertItems.Select(x => x.AlertItemNum).ToList());
				this.Invoke(() => {
					//Assigning this inside Invoke so that we don't have to lock _listAlertItems and _listAlertReads.
					_listAlertItems=listAlertItems;
					_listAlertReads=listAlertItemReads;
					AddAlertsToMenu();
				});
			});
			if(!doRunOnThread) {
				getAlerts(null);
				return;
			}
			ODThread threadCheckAlerts=new ODThread(getAlerts);
			threadCheckAlerts.AddExceptionHandler((ex) => {
				//Do not let the program crash due to this thread.
			});
			threadCheckAlerts.Name="CheckAlerts";
			threadCheckAlerts.Start(true);
		}

		private void CanadianItransCarrierThread() {
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canada
				return;
			}
			ODThread odThread=new ODThread(3600000,ItransThreadStart);//1 hour
			odThread.Start();
		}

		private void ItransThreadStart(ODThread odThread) {
			ODException.SwallowAnyException(() => {
				ItransNCpl.TryCarrierUpdate();
			});
		}

		///<summary>Adds the alert items to the alert menu item.</summary>
		private void AddAlertsToMenu() {
			//At this point _listAlertItems and _listAlertReads should be user, clinic and subscription filtered.
			//If the counts match this means they have read all AlertItems. 
			//This will result in the 'Alerts' menu item to not be colored.
			int alertCount=_listAlertItems.Count-_listAlertReads.Count;
			if(alertCount>99) {
				menuItemAlerts.Text=Lan.g(this,"Alerts")+" (99)";
			}
			else {
				menuItemAlerts.Text=Lan.g(this,"Alerts")+" ("+alertCount+")";
			}
			List<MenuItem> listMenuItem=menuItemAlerts.MenuItems.Cast<MenuItem>().ToList();
			bool doRedrawMenu=false;
			foreach(MenuItem menuItem in listMenuItem) {
				if(menuItem==menuItemAlerts || menuItem==menuItemNoAlerts) {//Never want to remove these MenuItems.
					continue;
				}
				if(_listAlertItems.Any(x => x.AlertItemNum==((AlertItem)menuItem.Tag).AlertItemNum)) {
					continue;//A menu item already exists for this alert. May update the description later.
				}
				menuItemAlerts.MenuItems.Remove(menuItem);//New MenuItem needed for new AlertItem.
				doRedrawMenu=true;
			}
			List<ActionType> listActionTypes=Enum.GetValues(typeof(ActionType)).Cast<ActionType>().ToList();
			listActionTypes.Sort(AlertItem.CompareActionType);
			//Loop through the _listAlertItems to either update or create our MenuItems.
			foreach(AlertItem alertItemCur in _listAlertItems) {
				string alertItemKey=alertItemCur.Type.ToString();
				string alertDescriptNew=AlertMenuItemHelper(alertItemCur)+alertItemCur.Description;
				MenuItem menuItem=listMenuItem.Where(x => x!=menuItemAlerts && x!=menuItemNoAlerts)
					.FirstOrDefault(x => alertItemCur.AlertItemNum==((AlertItem)x.Tag).AlertItemNum);
				if(menuItem!=null) {//Menu already has an item for this alert, so update text if needed.
					if(menuItem.Text!=alertDescriptNew) {
						menuItem.Text=alertDescriptNew;
						doRedrawMenu=true;
					}
					continue;
				}
				//A List of sub menuitems based off of the available actions for the current AlertItem.
				List<MenuItem> listSubMenuItems=new List<MenuItem>();
				foreach(ActionType actionTypeCur in listActionTypes) {
					if(actionTypeCur==ActionType.None || //This should never be shown to the user. Simply a default ActionType.
						!alertItemCur.Actions.HasFlag(actionTypeCur))//Current AlertItem does not have this ActionType associated with it.
					{
						continue;
					}
					MenuItem menuItemSub=new MenuItem(AlertSubMenuItemHelper(actionTypeCur,alertItemCur));
					menuItemSub.Name=actionTypeCur.ToString();//Used in menuItemAlerts_Click(...) 
					menuItemSub.Tag=alertItemCur;//Used in menuItemAlerts_Click(...) .
					menuItemSub.Click+=this.menuItemAlerts_Click;
					listSubMenuItems.Add(menuItemSub);
				}
				MenuItem itemCur=new MenuItem(alertDescriptNew,items:listSubMenuItems.ToArray());
				itemCur.Name=alertItemKey;//Used to find existing menuitems.
				itemCur.Tag=alertItemCur;//Used in menuItemAlerts_DrawItem(...) .
				itemCur.OwnerDraw=true;
				itemCur.DrawItem+=this.menuItemAlerts_DrawItem;
				itemCur.MeasureItem+=this.menuItemAlerts_MeasureItem;
				menuItemAlerts.MenuItems.Add(itemCur);
				doRedrawMenu=true;
			}
			menuItemAlerts.MenuItems[0].Visible=!(menuItemAlerts.MenuItems.Count>1);//1 for 'No Alerts' MenuItem which is always there.
			if(doRedrawMenu) {
				InvalidateAlertsMenuItem();//Forces menuItemAlerts_DrawItem(...) logic to run again.
			}
			if(PrefC.IsODHQ) {
				//Disable the Get Conf Room button when the PhoneTrackingServer goes down.
				phoneSmall.SetEnabledStateForControls(!_listAlertItems.Any(x => x.Type==AlertType.AsteriskServerMonitor));
			}
			Logger.LogToPath("CheckAlerts",LogPath.Signals,LogPhase.End);
		}
		
		///<summary>Helper function to translate the title for the given alertItem.</summary>
		private string AlertMenuItemHelper(AlertItem alertItem) {
			string value="";
			switch(alertItem.Type) {
				case AlertType.Generic:
					break;
				case AlertType.OnlinePaymentsPending:
					value+=Lan.g(this,"Pending Online Payments")+": ";
					break;
				case AlertType.VoiceMailMonitor:
					value+=Lan.g(this,"Voice Mail Monitor")+": ";
					break;
				case AlertType.RadiologyProcedures:
					value+=Lan.g(this,"Radiology Orders")+": ";
					break;
				case AlertType.CallbackRequested:
					value+=Lan.g(this,"Patient would like a callback regarding this appointment")+": ";
					break;
				case AlertType.WebSchedNewPat:
					value+=Lan.g(this,"eServices")+": ";
					break;
				case AlertType.WebSchedNewPatApptCreated:
					value+=Lan.g(this,"New Web Sched New Patient Appointment")+": ";
					break;
				case AlertType.MaxConnectionsMonitor:
					value+=Lan.g(this,"MySQL Max Connections")+": ";
					break;
				case AlertType.WebSchedASAPApptCreated:
					value+=Lan.g(this,"New Web Sched ASAP Appointment")+": ";
					break;
				case AlertType.AsteriskServerMonitor:
					value+=Lan.g(this,"Phone Tracking Server")+": ";
					break;
				case AlertType.WebSchedRecallApptCreated:
					value+=Lan.g(this,"New Web Sched Recall Appointment")+": ";
					break;
				case AlertType.NumberBarredFromTexting:
				case AlertType.MultipleEConnectors:
				case AlertType.EConnectorDown:
				case AlertType.EConnectorError:
				case AlertType.DoseSpotProviderRegistered:
				case AlertType.DoseSpotClinicRegistered:
				default:
					value+=Lan.g(this,alertItem.Type.GetDescription())+": ";
					break;
			}
			return value;
		}
		
		///<summary>Helper function to translate the title for the given alerttype and alertItem.</summary>
		private string AlertSubMenuItemHelper(ActionType actionType, AlertItem parentAlertItem) {
			string value="";
			switch(actionType) {
				case ActionType.None://This should never happen.
					value+=Lan.g(this,"None");
				break;
				case ActionType.MarkAsRead:
					value+=Lan.g(this,"Mark As Read");
				break;
				case ActionType.OpenForm:
					value+=Lan.g(this,"Open "+parentAlertItem.FormToOpen.GetDescription());
				break;
				case ActionType.Delete:
					value+=Lan.g(this,"Delete Alert");
					break;
				case ActionType.ShowItemValue:
					value+=Lan.g(this,"View Details");
					break;
			}
			return value;
		}

		///<summary>Takes one task and determines if it should popup for the current user.  Displays task popup if needed.</summary>
		private void TaskPopupHelper(Task taskPopup,List<UserOdPref> listBlockedTaskLists,List<TaskNote> listNotesForTask=null) {
			try {
				Logger.LogToPath("",LogPath.Signals,LogPhase.Start,"TaskNum: "+taskPopup.TaskNum.ToString());
				if(taskPopup.DateTimeEntry>DateTime.Now && taskPopup.ReminderType!=TaskReminderType.NoReminder) {
					return;//Don't pop up future dated reminder tasks
				}
				//Even though this is triggered to popup, if this is my own task, then do not popup.
				List<TaskNote> notesForThisTask=(listNotesForTask??TaskNotes.GetForTask(taskPopup.TaskNum)).OrderBy(x => x.DateTimeNote).ToList();
				if(taskPopup.ReminderType==TaskReminderType.NoReminder) {//We care about notes and task sender only if it's not a reminder.
					if(notesForThisTask.Count==0) {//'sender' is the usernum on the task and it's not a reminder
						if(taskPopup.UserNum==Security.CurUser.UserNum) {
							return;
						}
					}
					else {//'sender' is the user on the last added note
						if(notesForThisTask[notesForThisTask.Count-1].UserNum==Security.CurUser.UserNum) {
							return;
						}
					}
				}
				if(RemotingClient.RemotingRole==RemotingRole.ClientWeb && !Security.IsUserLoggedIn) {//User isn't actually logged in, so don't popup a task on their computer.
					return;
				}
				List<TaskList> listUserTaskListSubsTrunk=TaskLists.RefreshUserTrunk(Security.CurUser.UserNum);//Get the list of directly subscribed tasklists.
				List<long> listUserTaskListSubNums=listUserTaskListSubsTrunk.Select(x => x.TaskListNum).ToList();
				bool isUserSubscribed=listUserTaskListSubNums.Contains(taskPopup.TaskListNum);//First check if user is directly subscribed.
				if(!isUserSubscribed) {
					isUserSubscribed=listUserTaskListSubsTrunk.Any(x => TaskLists.IsAncestor(x.TaskListNum,taskPopup.TaskListNum));//Check ancestors for subscription.
				}
				if(isUserSubscribed) {//User is subscribed to this TaskList, or one of its ancestors.
					if(!listBlockedTaskLists.Any(x => x.Fkey==taskPopup.TaskListNum && PIn.Bool(x.ValueString))){//Subscribed and Unblocked, Show it!
						SoundPlayer soundplay=new SoundPlayer(Properties.Resources.notify);
						soundplay.Play();
						FormTaskEdit FormT=new FormTaskEdit(taskPopup,taskPopup.Copy());
						FormT.IsPopup=true;
						FormT.Show();//non-modal
					}
				}
			}
			finally {
				Logger.LogToPath("",LogPath.Signals,LogPhase.End,"TaskNum: "+taskPopup.TaskNum.ToString());
			}
		}

		///<summary>MenuItem does not have an invalidate or refresh so we quickly disable and enable the menu item so that the OwnerDraw methods get called.</summary>
		private void InvalidateAlertsMenuItem() {
			menuItemAlerts.Enabled=false;
			menuItemAlerts.Enabled=true;
			foreach(MenuItem menuItem in menuItemAlerts.MenuItems) {
					menuItem.Enabled=false;
					menuItem.Enabled=true;
			}
		}

		///<summary>Called when a shutdown signal is found.</summary>
		private void OnShutdown() {
			timerSignals.Enabled=false;//quit receiving signals.
			string msg = "";
			if(Process.GetCurrentProcess().ProcessName=="OpenDental") {
				msg+="All copies of Open Dental ";
			}
			else {
				msg+=Process.GetCurrentProcess().ProcessName+" ";
			}
			msg+=Lan.g(this,"will shut down in 15 seconds.  Quickly click OK on any open windows with unsaved data.");
			MsgBoxCopyPaste msgbox = new MsgBoxCopyPaste(msg);
			msgbox.Size=new Size(300,300);
			msgbox.TopMost=true;
			msgbox.Show();
			ODThread killThread = new ODThread((o) => {
				if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {
					ODThread webCamKillThread = new ODThread(((o2) => { Process.GetProcessesByName("WebCamOD").ToList().ForEach(x => x.Kill()); }));
					webCamKillThread.AddExceptionHandler((ex) => { });
					webCamKillThread.Start(true);
					ODThread proximityKillThread = new ODThread(((o2) => { Process.GetProcessesByName("ProximityOD").ToList().ForEach(x => x.Kill()); }));
					proximityKillThread.AddExceptionHandler((ex) => { });
					proximityKillThread.Start(true);
				}
				Thread.Sleep(15000);//15 seconds
				BeginInvoke((Action)(() => { CloseOpenForms(true); }));
				Thread.Sleep(1000);//1 second
				Invoke((Action)Application.Exit);
			});
			killThread.Start(true);
			return;
		}

		private void timerDisabledKey_Tick(object sender,EventArgs e) {
			//every 10 minutes:
			if(PrefC.GetBoolSilent(PrefName.RegistrationKeyIsDisabled,false)) {
				MessageBox.Show("Registration key has been disabled.  You are using an unauthorized version of this program.","Warning",
					MessageBoxButtons.OK,MessageBoxIcon.Warning);
			}
		}

		///<summary>This only contains UI signal processing. See Signalods.SignalsTick() for cache updates.</summary>
		public override void OnProcessSignals(List<Signalod> listSignals) {
			if(listSignals.Exists(x => x.IType==InvalidType.Programs)) {
				RefreshMenuReports();
			}
			if(listSignals.Exists(x => x.IType==InvalidType.Prefs)) {
				PrefC.InvalidateVerboseLogging();
			}
			#region SMS Notifications
			Logger.LogToPath("SMS Notifications",LogPath.Signals,LogPhase.Start);
			Signalod signalSmsCount=listSignals.OrderByDescending(x => x.SigDateTime)
				.FirstOrDefault(x => x.IType==InvalidType.SmsTextMsgReceivedUnreadCount && x.FKeyType==KeyType.SmsMsgUnreadCount);
			if(signalSmsCount!=null) {
				//Provide the pre-existing value here. This will act as a flag indicating that we should not resend the signal.  This would cause infinite signal loop.
				SetSmsNotificationText(signalSmsCount);
			}
			Logger.LogToPath("SMS Notifications",LogPath.Signals,LogPhase.End);
			#endregion SMS Notifications
			#region Tasks
			List <Signalod> listSignalTasks=listSignals.FindAll(x => x.IType==InvalidType.Task || x.IType==InvalidType.TaskPopup);
			List<long> listEditedTaskNums=listSignalTasks.FindAll(x => x.FKeyType==KeyType.Task).Select(x => x.FKey).ToList();
			ODThread threadTasks=new ODThread(new ODThread.WorkerDelegate((o) => {
				List<TaskNote> listRefreshedTaskNotes=null;
				List<UserOdPref> listBlockedTaskLists=null;
				List<Task> listRefreshedTasks=Tasks.GetNewTasksThisUser(Security.CurUser.UserNum,listEditedTaskNums);
				if(listRefreshedTasks.Count > 0) {
					listRefreshedTaskNotes=TaskNotes.GetForTasks(listRefreshedTasks.Select(x => x.TaskNum).ToList());
					listBlockedTaskLists=UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.TaskListBlock);
				}
				this.Invoke((() => HandleRefreshedTasks(listSignalTasks,listEditedTaskNums,listRefreshedTasks,listRefreshedTaskNotes,
					listBlockedTaskLists)));
			}));
			threadTasks.Name="TasksRefresh";
			threadTasks.Start(true);			
			#endregion Tasks
			#region Appointment Module
			if(ContrAppt2.Visible) {
				bool isRefreshAppts=Signalods.IsApptRefreshNeeded(AppointmentL.DateSelected.Date,listSignals);
				bool isRefreshScheds=Signalods.IsSchedRefreshNeeded(AppointmentL.DateSelected.Date,listSignals);
				if(isRefreshAppts || isRefreshScheds) {
					Logger.LogToPath("RefreshPeriod",LogPath.Signals,LogPhase.Start);
					ContrAppt2.RefreshPeriod(false,isRefreshAppointments:isRefreshAppts,isRefreshSchedules:isRefreshScheds);
					Logger.LogToPath("RefreshPeriod",LogPath.Signals,LogPhase.End);
				}
			}
			#endregion Appointment Module
			#region Unfinalize Pay Menu Update
			UpdateUnfinalizedPayCount(listSignals.FindAll(x => x.IType==InvalidType.UnfinalizedPayMenuUpdate));
			#endregion Unfinalize Pay Menu Update
			#region Refresh
			InvalidType[] arrInvalidTypes=Signalods.GetInvalidTypes(listSignals);
			if(arrInvalidTypes.Length > 0) {
				RefreshLocalDataPostCleanup(arrInvalidTypes);
			}
			#endregion Refresh
			//Sig Messages must be the last code region to run in the process signals method because it changes the application icon.
			#region Sig Messages (In the manual as "Internal Messages")
			//Check to see if any signals are sigmessages.
			List<long> listSigMessageNums=listSignals.FindAll(x => x.IType==InvalidType.SigMessages && x.FKeyType==KeyType.SigMessage).Select(x => x.FKey).ToList();
			if(listSigMessageNums.Count>0) {
				Logger.LogToPath("SigMessages",LogPath.Signals,LogPhase.Start);
				//Any SigMessage iType means we need to refresh our lights or buttons.
				List<SigMessage> listSigMessages=SigMessages.GetSigMessages(listSigMessageNums);
				ContrManage2.LogMsgs(listSigMessages);
				FillSignalButtons(listSigMessages);
				//Need to add a test to this: do not play messages that are over 2 minutes old.
				Thread newThread=new Thread(new ParameterizedThreadStart(PlaySounds));
				newThread.Start(listSigMessages);
				Logger.LogToPath("SigMessages",LogPath.Signals,LogPhase.End);
			}
			#endregion Sig Messages
			Plugins.HookAddCode(this,"FormOpenDental.ProcessSignals_end",listSignals);
		}

		///<summary>Refreshes tasks and pops up as necessary. Invoked from thread callback in OnProcessSignals() only.</summary>
		private void HandleRefreshedTasks(List<Signalod> listSignalTasks,List<long> listEditedTaskNums,List<Task> listRefreshedTasks,
			List<TaskNote> listRefreshedTaskNotes,List<UserOdPref> listBlockedTaskLists) 
		{
			bool hasChangedReminders=false;
			for(int i=0;i<listEditedTaskNums.Count;i++) {//Update the task meta data for the current user based on the query results.
				long editedTaskNum=listEditedTaskNums[i];//The tasknum mentioned in the signal.
				Task taskForUser=listRefreshedTasks.FirstOrDefault(x => x.TaskNum==editedTaskNum);
				Task taskNewForUser=null;
				if(taskForUser!=null) {
					bool isTrackedByUser=PrefC.GetBool(PrefName.TasksNewTrackedByUser);
					//Mimics how checkNew is set in FormTaskEdit.
					if((isTrackedByUser && taskForUser.IsUnread) || (!isTrackedByUser && taskForUser.TaskStatus==TaskStatusEnum.New)) {//See def of task.IsUnread
						taskNewForUser=taskForUser;
					}
				}
				Task taskReminderOld=_listReminderTasks.FirstOrDefault(x => x.TaskNum==editedTaskNum);
				if(taskReminderOld!=null) {//The task is a reminder which is relevant to the current user.
					hasChangedReminders=true;
					_listReminderTasks.RemoveAll(x => x.TaskNum==editedTaskNum);//Remove the old copy of the task.
					if(taskForUser!=null) {//The updated reminder task is relevant to the current user.
						_listReminderTasks.Add(taskForUser);//Add the updated reminder task into the list (replacing the old reminder task).
					}
				}
				else if(_listNormalTaskNums.Contains(editedTaskNum)) {//The task is a normal task which is relevant to the current user.
					if(taskNewForUser==null) {//But now the task is no longer relevant to the user.
						_listNormalTaskNums.Remove(editedTaskNum);
					}
				}
				else {//The edited tasknum is not currently in our meta data.
					if(taskNewForUser!=null && String.IsNullOrEmpty(taskNewForUser.ReminderGroupId)) {//A new normal task has now become relevant.
						_listNormalTaskNums.Add(editedTaskNum);
					}
					else if(taskForUser!=null && !String.IsNullOrEmpty(taskForUser.ReminderGroupId)) {//A reminder task has become relevant (new or viewed)
						hasChangedReminders=true;
						_listReminderTasks.Add(taskForUser);
					}
				}//else
			}//for
			RefreshTasksNotification();
			List<Task> tasksPopup=new List<Task>();
			for(int i=0;i<listRefreshedTasks.Count;i++) {//Locate any popup tasks in the returned list of tasks.
				bool taskIsOpen=false;
				for(int j=0;j<Application.OpenForms.Count;j++) {
					Form form=Application.OpenForms[j];
					if(!(form is FormTaskEdit)) {
						continue;
					}
					FormTaskEdit FormTE=(FormTaskEdit)form;
					if(FormTE.TaskNumCur==listRefreshedTasks[i].TaskNum) {
						FormTE.OnTaskEdited();
						taskIsOpen=true;
					}
				}
				//Verify the current task is a popup task.
				if(!listSignalTasks.Exists(x => x.FKeyType==KeyType.Task && x.IType==InvalidType.TaskPopup && x.FKey==listRefreshedTasks[i].TaskNum)
					|| taskIsOpen) 
				{
					continue;//Not a popup task.
				}
				tasksPopup.Add(listRefreshedTasks[i]);
			}
			for(int i=0;i<tasksPopup.Count;i++) {
				//Reminders sent to a subscribed tasklist will pop up prior to the reminder date/time.
				TaskPopupHelper(tasksPopup[i],listBlockedTaskLists,listRefreshedTaskNotes.FindAll(x => x.TaskNum==tasksPopup[i].TaskNum));
			}
			if(listSignalTasks.Count > 0 || tasksPopup.Count>0) {
				if(userControlTasks1.Visible) {
					userControlTasks1.RefreshTasks();
				}
			}
			//Refresh the appt module if reminders have changed, even if the appt module not visible.
			//The user will load the appt module eventually and these refreshes are the only updates the appointment module receives for reminders.
			if(hasChangedReminders) { 
				ContrAppt2.RefreshReminders(_listReminderTasks);
				_dateReminderRefresh=DateTimeOD.Today;
			}
			}

		///<summary>Starts the eService monitoring thread that will run once a minute.  Only runs if the user currently logged in has the eServices permission.</summary>
		private void StartEServiceMonitoring() {
			//If the user currently logged in has permission to view eService settings, turn on the listener monitor.
			if(Security.CurUser==null || !Security.IsAuthorized(Permissions.EServicesSetup,true)) {
				return;//Do not start the listener service monitor for users without permission.
			}
			if(_odThreadEServices==null) {
				//Process any Error signals that happened due to an update:
				EServiceSignals.ProcessErrorSignalsAroundTime(PrefC.GetDateT(PrefName.ProgramVersionLastUpdated));
				//Create a separate thread that will run every 60 seconds to monitor eService signals.
				_odThreadEServices=new ODThread(60000,ProcessEServiceSignals);
				//Add exception handling just in case MySQL is unreachable at any point in the lifetime of this session.
				_odThreadEServices.AddExceptionHandler(EServiceMonitoringException);
				_odThreadEServices.Name="eService Monitoring Thread";
				_odThreadEServices.GroupName="eServiceThreads";
			}
			_odThreadEServices.Start();
		}

		///<summary>Stops the eService monitoring thread and sets the eServices menu item colors to a disabled state because the Log On window will be shown next.</summary>
		private void StopEServiceMonitoring() {
			if(_odThreadEServices==null) {
				return;//Nothing to do, the service was already stopped.
			}
			//QuitSync the thread just because it has the power to live for up to a minute which is unnecessary.
			//There is no reason to wait more than 1 second for the thread to quit.
			_odThreadEServices.QuitSync(1000);
			_odThreadEServices=null;
		}

		///<summary>The exception delegate for the eService monitoring thread.</summary>
		private void EServiceMonitoringException(Exception ex) {
			//Currently we don't want to do anything if the eService signal processing fails.  Simply try again in a minute.  
			//Most likely cause for exceptions will be database IO when computers are just sitting around not doing anything.
			//Implementing this delegate allows us to NOT litter ProcessEServiceSignals() with try catches.  
		}

		///<summary>The exception delegate for the Podium monitoring thread.</summary>
		private void PodiumMonitoringException(Exception ex) {
			Logger.WriteException(ex,Podium.LOG_DIRECTORY_PODIUM);
		}

		///<summary>Worker method for _odThreadEServices.  Call StartEServiceMonitoring() to start monitoring eService signals instead of calling this method directly.
		///This thread's only job is to check to see if the eConnector's current status is critical and if it is critical, create a High severity alert.</summary>
		private void ProcessEServiceSignals(ODThread odThread) {
			//The listener service will have a local heartbeat every 5 minutes so it's overkill to check every time timerSignals_Tick fires.
 			//Only check the Listener Service status once a minute.
			//The downside to doing this is that the menu item will stay red up to one minute when a user wants to stop monitoring the service.
			eServiceSignalSeverity listenerStatus=EServiceSignals.GetListenerServiceStatus();
			if(listenerStatus==eServiceSignalSeverity.None) {
				//This office has never had a valid listener service running and does not have more than 5 patients set up to use the listener service.
				//Quit the thread so that this computer does not waste its time sending queries to the server every minute.
				odThread.QuitAsync();
				return;
			}
			if(listenerStatus!=eServiceSignalSeverity.Critical) { //Not a critical event so no need to continue.
				return;
			}
			if(AlertItems.RefreshForType(AlertType.EConnectorDown).Count>0) { //Alert already exists to no need to continue.
				return;
			}
			//Create an alert.
			AlertItems.Insert(new AlertItem {
				//Do not allow delete. The only way for this alert to be deleted is for the eConnector to insert a heartbeat, which will in-turn delete this alert.
				Actions=ActionType.MarkAsRead|ActionType.OpenForm,
				Description=Lans.g("EConnector","eConnector needs to be restarted"),
				Severity=SeverityType.High,
				Type=AlertType.EConnectorDown,
				FormToOpen=FormType.FormEServicesEConnector,
			});
			//We just inserted an alert so update the alert menu.
			this.Invoke((Action)(() => CheckAlerts()));
		}
		
		///<summary>Starts a thread that will fill the fee cache while Open Dental is still loading.</summary>
		private void StartCacheFillForFees() {
			//This is not a typical pattern. This fillCache is only for fees at the moment because it has been thought about heavily and the ramifications of this have been considered.
			if(_odThreadFillCache==null) {
				Fees.IsFilledByThread=true;//Make all other threads wait until this thread has successfully filled the fee cache the first time.
				//Create a separate thread that will run once
				_odThreadFillCache=new ODThread(FillCaches);
				//Add exception handling just in case MySQL is unreachable at any point in the lifetime of this session.
				_odThreadFillCache.AddExceptionHandler(FillCachesException);
				_odThreadFillCache.Name="Fill Caches Thread";
				_odThreadFillCache.GroupName="cacheThreads";
				_odThreadFillCache.SetConnectionRetrySeconds((int)TimeSpan.FromMinutes(1).TotalSeconds);
			}
			_odThreadFillCache.Start(true);
		}

		///<summary>Worker method for _odThreadFillCache.  Call StartCacheFillForFees() to start filling the cache. Currently only fills the fee cache.</summary>
		private void FillCaches(ODThread odThread) {
			//Other caches that need to be filled can be added later as long as they follow the thread safe cache pattern.
			Fees.FillCache();
		}

		///<summary>The exception delegate for the cache filling thread.</summary>
		private void FillCachesException(Exception ex) {
			Fees.IsFilledByThread=false;//So that when they try to access the fee cache, it won't wait for 60 seconds before throwing.
		}

		///<summary></summary>
		public void ProcessKillCommand() {
			//It is crucial that every form be forcefully closed so that they do not stay connected to a database that has been updated to a more recent version.
			CloseOpenForms(true);
			Application.Exit();//This will call FormOpenDental's closing event which will clean up all threads that are currently running.
		}

		///<summary></summary>
		public static void S_ProcessKillCommand() {
			_formOpenDentalS.ProcessKillCommand();
		}

		private void PlaySounds(object objSignalList) {
			List<SigMessage> listSigMessages=(List<SigMessage>)objSignalList;
			byte[] rawData;
			MemoryStream stream=null;
			SoundPlayer simpleSound=null;
			try {
				//loop through each signal
				foreach(SigMessage sigMessage in listSigMessages) { 
					if(sigMessage.AckDateTime.Year > 1880) {
						continue;//don't play any sounds for acks.
					}
					//play all the sounds.
					List<SigElementDef> listSigElementDefs=SigElementDefs.GetDefsForSigMessage(sigMessage);
					foreach(SigElementDef sigElement in listSigElementDefs) {
						if(sigElement.Sound=="") {
							continue;
						}
						try {
							rawData=Convert.FromBase64String(sigElement.Sound);
							stream=new MemoryStream(rawData);
							simpleSound=new SoundPlayer(stream);
							simpleSound.PlaySync();//sound will finish playing before thread continues
						}
						catch {
							//do nothing
						}
					}
					Thread.Sleep(1000);//pause 1 second between signals.
				}
			}
			finally {
				if(stream!=null) {
					stream.Dispose();
				}
				if(simpleSound!=null) {
					simpleSound.Dispose();
				}
			}
		}

		private void myOutlookBar_ButtonClicked(object sender, OpenDental.ButtonClicked_EventArgs e){
			switch(myOutlookBar.SelectedIndex){
				case 0:
					if(!Security.IsAuthorized(Permissions.AppointmentsModule)){
						e.Cancel=true;
						return;
					}
					break;
				case 1:
					if(PrefC.GetBool(PrefName.EhrEmergencyNow)) {//if red emergency button is on
						if(Security.IsAuthorized(Permissions.EhrEmergencyAccess,true)) {
							break;//No need to check other permissions.
						}
					}
					//Whether or not they were authorized by the special situation above,
					//they can get into the Family module with the ordinary permissions.
					if(!Security.IsAuthorized(Permissions.FamilyModule)) {
						e.Cancel=true;
						return;
					}
					break;
				case 2:
					if(!Security.IsAuthorized(Permissions.AccountModule)){
						e.Cancel=true;
						return;
					}
					break;
				case 3:
					if(!Security.IsAuthorized(Permissions.TPModule)){
						e.Cancel=true;
						return;
					}
					break;
				case 4:
					if(!Security.IsAuthorized(Permissions.ChartModule)){
						e.Cancel=true;
						return;
					}
					break;
				case 5:
					if(!Security.IsAuthorized(Permissions.ImagesModule)){
						e.Cancel=true;
						return;
					}
					break;
				case 6:
					if(!Security.IsAuthorized(Permissions.ManageModule)){
						e.Cancel=true;
						return;
					}
					break;
			}
			UnselectActive();
			allNeutral();
			SetModuleSelected(true);
		}

		///<summary>Returns the translated name of the currently selected module.</summary>
		public string GetSelectedModuleName() {
			try {
				return myOutlookBar.Buttons[myOutlookBar.SelectedIndex].Caption;
			}
			catch(Exception ex) {
				ex.DoNothing();
				return "";
			}
		}

		///<summary>Sets the currently selected module based on the selectedIndex of the outlook bar. If selectedIndex is -1, which might happen if user does not have permission to any module, then this does nothing.</summary>
		private void SetModuleSelected() {
			SetModuleSelected(false);
		}

		///<summary>Sets the currently selected module based on the selectedIndex of the outlook bar. If selectedIndex is -1, which might happen if user does not have permission to any module, then this does nothing. The menuBarClicked variable should be set to true when a module button is clicked, and should be false when called for refresh purposes.</summary>
		private void SetModuleSelected(bool menuBarClicked){
			switch(myOutlookBar.SelectedIndex){
				case 0:
					ContrAppt2.InitializeOnStartup();
					ContrAppt2.Visible=true;
					this.ActiveControl=this.ContrAppt2;
					ContrAppt2.ModuleSelected(CurPatNum);
					break;
				case 1:
					if(HL7Defs.IsExistingHL7Enabled()) {
						HL7Def def=HL7Defs.GetOneDeepEnabled();
						if(def.ShowDemographics==HL7ShowDemographics.Hide) {
							ContrFamily2Ecw.Visible=true;
							this.ActiveControl=this.ContrFamily2Ecw;
							ContrFamily2Ecw.ModuleSelected(CurPatNum);
						}
						else {
							ContrFamily2.InitializeOnStartup();
							ContrFamily2.Visible=true;
							this.ActiveControl=this.ContrFamily2;
							ContrFamily2.ModuleSelected(CurPatNum);
						}
					}
					else {
						if(Programs.UsingEcwTightMode()) {
							ContrFamily2Ecw.Visible=true;
							this.ActiveControl=this.ContrFamily2Ecw;
							ContrFamily2Ecw.ModuleSelected(CurPatNum);
						}
						else {
							ContrFamily2.InitializeOnStartup();
							ContrFamily2.Visible=true;
							this.ActiveControl=this.ContrFamily2;
							ContrFamily2.ModuleSelected(CurPatNum);
						}
					}
					break;
				case 2:
					ContrAccount2.InitializeOnStartup();
					ContrAccount2.Visible=true;
					this.ActiveControl=this.ContrAccount2;
					ContrAccount2.ModuleSelected(CurPatNum);
					break;
				case 3:
					ContrTreat2.InitializeOnStartup();
					ContrTreat2.Visible=true;
					this.ActiveControl=this.ContrTreat2;
					if(menuBarClicked) {
						ContrTreat2.ModuleSelected(CurPatNum,true);//Set default date to true when button is clicked.
					}
					else {
						ContrTreat2.ModuleSelected(CurPatNum);
					}
					break;
				case 4:
					ContrChart2.InitializeOnStartup();
					ContrChart2.Visible=true;
					this.ActiveControl=this.ContrChart2;
					if(menuBarClicked) {
						ContrChart2.ModuleSelectedErx(CurPatNum);
					}
					else {
						ContrChart2.ModuleSelected(CurPatNum,true);
					}
					TryNonPatientPopup();
					break;
				case 5:
					ContrImages2.InitializeOnStartup();
					ContrImages2.Visible=true;
					this.ActiveControl=this.ContrImages2;
					ContrImages2.ModuleSelected(CurPatNum);
					break;
				case 6:
					//ContrManage2.InitializeOnStartup();//This gets done earlier.
					ContrManage2.Visible=true;
					this.ActiveControl=this.ContrManage2;
					ContrManage2.ModuleSelected(CurPatNum);
					break;
			}
		}

		private void allNeutral(){
			ContrAppt2.Visible=false;
			ContrFamily2.Visible=false;
			ContrFamily2Ecw.Visible=false;
			ContrAccount2.Visible=false;
			ContrTreat2.Visible=false;
			ContrChart2.Visible=false;
			ContrImages2.Visible=false;
			ContrManage2.Visible=false;
		}

		private void UnselectActive(){
			if(ContrAppt2.Visible){
				ContrAppt2.ModuleUnselected();
			}
			if(ContrFamily2.Visible){
				ContrFamily2.ModuleUnselected();
			}
			if(ContrFamily2Ecw.Visible) {
				//ContrFamily2Ecw.ModuleUnselected();
			}
			if(ContrAccount2.Visible){
				ContrAccount2.ModuleUnselected();
			}
			if(ContrTreat2.Visible){
				ContrTreat2.ModuleUnselected();
			}
			if(ContrChart2.Visible){
				ContrChart2.ModuleUnselected();
			}
			if(ContrImages2.Visible){
				ContrImages2.ModuleUnselected();
			}
		}

		///<Summary>This also passes CurPatNum down to the currently selected module (except the Manage module).  If calling from ContrAppt and
		///RefreshModuleDataPatient was called before calling this method, set isApptRefreshDataPat=false so the get pat query isn't run twice.</Summary>
		private void RefreshCurrentModule(bool hasForceRefresh=false,bool isApptRefreshDataPat=true,bool isClinicRefresh=true){
			if(ContrAppt2.Visible){
				if(hasForceRefresh) {
					ContrAppt2.ModuleSelected(CurPatNum);
				}
				else {
					if(isApptRefreshDataPat) {//don't usually skip data refresh, only if CurPatNum was set just prior to calling this method
					ContrAppt2.RefreshModuleDataPatient(CurPatNum);
					}
					ContrAppt2.RefreshModuleScreenPatient();
				}
			}
			if(ContrFamily2.Visible){
				ContrFamily2.ModuleSelected(CurPatNum);
			}
			if(ContrFamily2Ecw.Visible) {
				ContrFamily2Ecw.ModuleSelected(CurPatNum);
			}
			if(ContrAccount2.Visible){
				ContrAccount2.ModuleSelected(CurPatNum);
			}
			if(ContrTreat2.Visible){
				ContrTreat2.ModuleSelected(CurPatNum);
			}
			if(ContrChart2.Visible){
				ContrChart2.ModuleSelected(CurPatNum,isClinicRefresh);
			}
			if(ContrImages2.Visible){
				ContrImages2.ModuleSelected(CurPatNum);
			}
			if(ContrManage2.Visible){
				ContrManage2.ModuleSelected(CurPatNum);
			}
			userControlTasks1.RefreshPatTicketsIfNeeded();
		}

		/// <summary>sends function key presses to the appointment module and chart module</summary>
		private void FormOpenDental_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			if(ContrAppt2.Visible && e.KeyCode>=Keys.F1 && e.KeyCode<=Keys.F12){
				ContrAppt2.FunctionKeyPress(e.KeyCode);
				return;
			}
			if(ContrChart2.Visible && e.KeyCode>=Keys.F1 && e.KeyCode<=Keys.F12) {
				ContrChart2.FunctionKeyPressContrChart(e.KeyCode);
				return;
			}
			//Ctrl-Alt-R is supposed to show referral window, but it doesn't work on some computers.
			//so we're also going to use Ctrl-X to show the referral window.
			if(CurPatNum!=0
				&& (e.Modifiers==(Keys.Alt|Keys.Control) && e.KeyCode==Keys.R)
					|| (e.Modifiers==Keys.Control && e.KeyCode==Keys.X))
			{
				FormReferralsPatient FormRE=new FormReferralsPatient();
				FormRE.PatNum=CurPatNum;
				FormRE.ShowDialog();
			}
			Plugins.HookAddCode(this,"FormOpenDental_KeyDown_end",e);
		}

		private void ThreadClaimReportRetrieve(ODThread worker) {
			string claimReportComputer=PrefC.GetString(PrefName.ClaimReportComputerName);
			if(claimReportComputer=="" || claimReportComputer!=Dns.GetHostName()) {
				return;
			}
			Clearinghouses.RetrieveReportsAutomatic(false);//only run for the selected clinic, if clinics are enabled
		}

		private void ThreadClaimReportException(Exception ex) {
			//Currently do nothing.
		}

		///<summary>If the local computer is the computer where incoming email is fetched, then this thread runs in the background and checks for new
		///messages every x number of minutes (1 to 60) based on preference value.</summary>
		private void ThreadEmailInbox_Receive(ODThread odThread) {
			//Certificate stores need to be created on all computers since any of the computers are able to potentially send encrypted email.
			try {
				EmailMessages.CreateCertificateStoresIfNeeded();
			}
			catch {
				//Probably a permission issue creating the stores.  Nothing we can do except explain in the manual.
			}
			if(!ODEnvironment.IdIsThisComputer(PrefC.GetString(PrefName.EmailInboxComputerName))) {
				return;//If email inbox computer name is not setup, or if the name does not match this computer, then do not get email from this computer.
			}
			//Do not perform the first email inbox receive within the first minute of program startup (thread startup), in order to prevent slow login.
			//The EmailInboxCheckInterval preference defines the number of minutes between automatic inbox receiving (minimum 1 minute).
			if((odThread.GetTimeElapsed()-_timeEmailInboxesLastChecked).TotalMinutes < PrefC.GetInt(PrefName.EmailInboxCheckInterval)) {
				return;
			}
			_timeEmailInboxesLastChecked=odThread.GetTimeElapsed();
			List<EmailAddress> listEmailAddresses=EmailAddresses.GetAll();
			for(int i=0;i<listEmailAddresses.Count;i++) {
				EmailAddress emailAddress=listEmailAddresses[i];
				if(emailAddress.Pop3ServerIncoming=="") {//not active
					continue;//Email address not setup.
				}
				List<ODThread> listEmailThreads=ODThread.GetThreadsByGroupName("EmailInbox - #"+emailAddress.EmailAddressNum);
				if(listEmailThreads.Count==0) {//Only launch a thread for this email address if one is not still running.
					ODThread threadEmailRetrieve=new ODThread(RetrieveForAddress);
					threadEmailRetrieve.GroupName="EmailInbox - #"+emailAddress.EmailAddressNum;
					threadEmailRetrieve.Tag=emailAddress;
					threadEmailRetrieve.Start();
				}
			}
		}

		///<summary>This thread runs one time per each email address per each retrieve interval.</summary>
		private void RetrieveForAddress(ODThread odThread) {
			EmailAddress emailAddress=(EmailAddress)odThread.Tag;
			try {
				EmailMessages.ReceiveFromInbox(0,emailAddress);
				Plugins.HookAddCode(this,"FormOpenDental.RetrieveForAddress_after_received");
			}
			catch(Exception) {
				//Do not tell the user, because it would be annoying to see an error every 30 seconds (if the server was down for instance).
				//Maybe we could log to the system EventViewer Application log someday if users complain. Keep in mind that the user can always manually go 
				//to Manage | Email Inbox to see the error message.
			}
			odThread.QuitAsync();
		}

		private void ThreadTimeSynch_Synch() {
			_isTimeSynchThreadRunning=true;
			while(_isTimeSynchThreadRunning) {
				if(!_isTimeSynchThreadRunning) {
					break;
				}
				NTPv4 ntp=new NTPv4();
				double nistOffset=double.MaxValue;
				try {
					nistOffset=ntp.getTime(PrefC.GetString(PrefName.NistTimeServerUrl));
				}
				catch { } //Invalid NIST Server URL
				if(nistOffset!=double.MaxValue) {
					//Did not timeout, or have invalid NIST server URL
					try {
						WindowsTime.SetTime(DateTime.Now.AddMilliseconds(nistOffset)); //Sets local machine time
					}
					catch { } //Error setting local machine time
				}
				_timeSynchSleep.WaitOne(TimeSpan.FromMinutes(240));
			}
		}

		///<summary>Gets the encrypted connection string for the Oracle database from a config file.</summary>
		public bool GetOraConfig() {
			if(!File.Exists("ODOraConfig.xml")) {
				return false;
			}
			try {
				XmlDocument document=new XmlDocument();
				document.Load("ODOraConfig.xml");
				XPathNavigator Navigator=document.CreateNavigator();
				XPathNavigator nav;
				nav=Navigator.SelectSingleNode("//DatabaseConnection");
				if(nav!=null) {
					connStr=nav.SelectSingleNode("ConnectionString").Value;
					key=nav.SelectSingleNode("Key").Value;
				}
				DataConnection.DBtype=DatabaseType.Oracle;
				return true;
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return false;
			}
		}

		///<summary>Decrypt the connection string and try to connect to the database directly. Only called if using a connection string and ChooseDatabase is not to be shown. Must call GetOraConfig first.</summary>
		public bool TryWithConnStr() {
			OpenDentBusiness.DataConnection dcon=new OpenDentBusiness.DataConnection();
			try {
				if(connStr!=null) {
#if ORA_DB
					OD_CRYPTO.Decryptor crypto=new OD_CRYPTO.Decryptor();
					dconnStr=crypto.Decrypt(connStr,key);
					crypto=null;
					dcon.SetDb(dconnStr,"",DatabaseType.Oracle);
#endif
				}
				//a direct connection does not utilize lower privileges.
				RemotingClient.RemotingRole=RemotingRole.ClientDirect;
				return true;
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return false;
			}
		}

		///<summary>This method stops all (local) timers and displays a connection lost window that will let users attempt to reconnect.
		///At any time during the lifespan of the application connection to the database can be lost for unknown reasons.
		///When anything spawned by FormOpenDental (main thread) tries to connect to the database and fails, this event will get fired.</summary>
		private void DataConnection_ConnectionLost(DataConnectionEventArgs e) {
			if(InvokeRequired) {
				this.BeginInvoke(() => DataConnection_ConnectionLost(e));
				return;
			}
			if(RemotingClient.RemotingRole!=RemotingRole.ClientDirect) {
				return;
			}
			if(e==null || e.Name!="DataConnection"
				|| e.IsConnectionRestored //We do not want to show the window if the event is saying that the connection has been restored.
				|| _formConnectionLost!=null) //The Connection Lost window is already open, no need to open another one.
			{
				return;
			}
			SetTimersEnabled(false);
			string exceptionText=(string)e.Tag;
			_formConnectionLost=new FormConnectionLost(exceptionText);
			if(_formConnectionLost.ShowDialog()==DialogResult.Cancel) {
				//This is problematic because it causes DirectX to cause a UE but there doesn't seem to be a better way to close without using the database.
				Environment.Exit(0);
			}
			_formConnectionLost=null;
			SetTimersEnabled(true);
		}

		///<summary>This method stops all (local) timers and displays a bad credentials window that will let users attempt to login again.  This is to
		///handle the situation where a user is logged into multiple computers via middle tier and changes their password on 1 connection.  The other
		///connection(s) would attempt to access the database using the old password (for signal refreshes etc) and lock the user's account for too many
		///failed attempts.  FormLoginFailed will not allow a different user to login, only the current user or exit the program.</summary>
		private void DataConnection_CredentialsFailedAfterLogin(ODEventArgs e) {
			if(InvokeRequired) {
				this.BeginInvoke(() => DataConnection_CredentialsFailedAfterLogin(e));
				return;
			}
			if(RemotingClient.RemotingRole!=RemotingRole.ClientWeb) {
				return;
			}
			if(e!=null && e.Name!="ServiceCredentials") {
				return;
			}
			if(Security.CurUser==null) {
				Environment.Exit(0);//shouldn't be possible, would have to have a user logged in to get here, but just in case, exit the program
			}
			if(RemotingClient.HasLoginFailed || (_formLoginFailed!=null && !_formLoginFailed.IsDisposed)) {//_formLoginFailed already displayed, wait for _formLoginFailed to close
				return;
			}
			RemotingClient.HasLoginFailed=true;//first thread to get the lock (or invoke this method so the main thread gets the lock) will display the login form
			try {
				SetTimersEnabled(false);
				Security.IsUserLoggedIn=false;
				string errorMsg=(string)e.Tag;
				_formLoginFailed=new FormLoginFailed(errorMsg);
				_formLoginFailed.ShowDialog();
				if(_formLoginFailed.DialogResult==DialogResult.Cancel) {
					Environment.Exit(0);
				}
				SetTimersEnabled(true);
				Security.DateTimeLastActivity=DateTime.Now;
			}
			catch(Exception ex) {
				ex.DoNothing();
				throw;
			}
			finally {
				RemotingClient.HasLoginFailed=false;
				_formLoginFailed=null;
			}
		}

		///<summary>Starts or stops all local timers.  Only starts signal timer if interval preference is set to non-zero value.
		///Also starts/stops EServiceMonitoring.</summary>
		private void SetTimersEnabled(bool doStart) {
			if(doStart) {
				if(PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs)!=0) {
					timerSignals.Start();
				}
				timerReplicationMonitor.Start();
				timerLogoff.Start();
				timerTimeIndic.Start();
				timerWebHostSynch.Start();
				timerDisabledKey.Start();
				StartEServiceMonitoring();
			}
			else {			
				timerSignals.Stop();
				timerReplicationMonitor.Stop();
				timerLogoff.Stop();
				timerTimeIndic.Stop();
				timerWebHostSynch.Stop();
				timerDisabledKey.Stop();
				StopEServiceMonitoring();
			}
		}

		public static void S_TaskGoTo(TaskObjectType taskOT,long keyNum) {
			_formOpenDentalS.TaskGoTo(taskOT,keyNum);
		}

		private void TaskGoTo(TaskObjectType taskOT,long keyNum){
			if(taskOT==TaskObjectType.None || keyNum==0) {
				return;
			}
			if(taskOT==TaskObjectType.Patient) {
				CurPatNum=keyNum;
				Patient pat=Patients.GetPat(CurPatNum);
				RefreshCurrentModule();
				FillPatientButton(pat);
			}
			if(taskOT==TaskObjectType.Appointment) {
				Appointment apt=Appointments.GetOneApt(keyNum);
				if(apt==null) {
					MsgBox.Show(this,"Appointment has been deleted, so it's not available.");
					return;
				}
				DateTime dateSelected=DateTime.MinValue;
				if(apt.AptStatus==ApptStatus.Planned || apt.AptStatus==ApptStatus.UnschedList) {
					//I did not add feature to put planned or unsched apt on pinboard.
					MsgBox.Show(this,"Cannot navigate to appointment.  Use the Other Appointments button.");
					//return;
				}
				else {
					dateSelected=apt.AptDateTime;
				}
				CurPatNum=apt.PatNum;//OnPatientSelected(apt.PatNum);
				FillPatientButton(Patients.GetPat(CurPatNum));
				GotoModule.GotoAppointment(dateSelected,apt.AptNum);
			}
		}

		private void comboTriageCoordinator_MouseWheel(object sender, MouseEventArgs e) {
			ComboBox comboControl = (ComboBox)sender;
			if(!comboControl.DroppedDown) {
				((HandledMouseEventArgs)e).Handled = true;
			}
		}

		private void butBigPhones_Click(object sender,EventArgs e) {
			if(formPhoneTiles==null || formPhoneTiles.IsDisposed) {
				formPhoneTiles=new FormPhoneTiles();
				formPhoneTiles.GoToChanged += new System.EventHandler(this.phonePanel_GoToChanged);
				formPhoneTiles.Show();
				Rectangle rect=System.Windows.Forms.Screen.FromControl(this).Bounds;
				formPhoneTiles.Location=new Point((rect.Width-formPhoneTiles.Width)/2+rect.X,10);
				formPhoneTiles.BringToFront();
			}
			else {
				if(formPhoneTiles.WindowState==FormWindowState.Minimized) {
					formPhoneTiles.WindowState=FormWindowState.Normal;
				}
				formPhoneTiles.Show();
				formPhoneTiles.BringToFront();
			}
		}

		private void butMapPhones_Click(object sender,EventArgs e) {
			FormMapHQ formMapHQ;
			if(_listMaps.Count==0) {
				formMapHQ=new FormMapHQ();
				formMapHQ.RoomControlClicked+=FormMapHQ_RoomControlClicked;
				formMapHQ.ExtraMapClicked+=FormMapHQ_ExtraMapClicked;
				formMapHQ.GoToChanged+=MapAreaRoomControl_GoToChanged;
			}
			else {
				formMapHQ=_listMaps[0]; //always just take the first one.
				if(formMapHQ.WindowState==FormWindowState.Minimized) {
					_listMaps[0].WindowState=FormWindowState.Normal; //always just take the first map in the list
				}
			}
			formMapHQ.Show();
			formMapHQ.BringToFront();
		}

		private void butTriage_Click(object sender,EventArgs e) {
			ContrManage2.JumpToTriageTaskWindow();
		}

		private void butVoiceMails_Click(object sender,EventArgs e) {
			//Change the ClockStatus to TeamAssist if the logged on user is clocked in and the same user as the extension.
			if(PhoneTile.PhoneCur!=null
				&& ClockEvents.IsClockedIn(Security.CurUser.EmployeeNum)
				&& Security.CurUser.EmployeeNum==PhoneTile.PhoneCur.EmployeeNum) 
			{
				phoneSmall.SetTeamAssist();
			}
			if(_formVM==null || _formVM.IsDisposed) {
				_formVM=new FormVoiceMails();
				_formVM.FormClosed+=new FormClosedEventHandler((o,e1) => { _formVM=null; });
				_formVM.Show();
			}
			if(_formVM.WindowState==FormWindowState.Minimized) {
				_formVM.WindowState=FormWindowState.Normal;
			}
			_formVM.BringToFront();
		}

		public static void S_SetPhoneStatusAvailable() {
			_formOpenDentalS.phoneSmall.SetAvailable();
		}

		private void phonePanel_GoToChanged(object sender,EventArgs e) {
			if(formPhoneTiles.GotoPatNum!=0) {
				CurPatNum=formPhoneTiles.GotoPatNum;
				Patient pat=Patients.GetPat(CurPatNum);
				RefreshCurrentModule();
				FillPatientButton(pat);
			}
		}

		private void MapAreaRoomControl_GoToChanged(object sender,EventArgs e) {
			MapAreaRoomControl room=(MapAreaRoomControl)sender;
			CurPatNum=room.getPhoneCur().PatNum;
			Patient pat=Patients.GetPat(CurPatNum);
			RefreshCurrentModule();
			FillPatientButton(pat);
		}

		private void phoneSmall_GoToChanged(object sender,EventArgs e) {
			if(phoneSmall.GotoPatNum==0) {
				return;
			}
			CurPatNum=phoneSmall.GotoPatNum;
			Patient pat=Patients.GetPat(CurPatNum);
			RefreshCurrentModule();
			FillPatientButton(pat);
			Commlog commlog=Commlogs.GetIncompleteEntry(Security.CurUser.UserNum,CurPatNum);
			PhoneEmpDefault ped=PhoneEmpDefaults.GetByExtAndEmp(phoneSmall.Extension,Security.CurUser.EmployeeNum);
			if(ped!=null && ped.IsTriageOperator) {
				if(Plugins.HookMethod(this,"FormOpenDental.phoneSmall_GoToChanged_IsTriage",pat,phoneSmall.Extension)) {
					return;
				}
				Task task=new Task();
				task.TaskListNum=-1;//don't show it in any list yet.
				Tasks.Insert(task);
				Task taskOld=task.Copy();
				task.KeyNum=CurPatNum;
				task.ObjectType=TaskObjectType.Patient;
				task.TaskListNum=1697;//Hardcoded for internal Triage task list.
				task.UserNum=Security.CurUser.UserNum;
				task.Descript=Phones.GetPhoneForExtensionDB(phoneSmall.Extension).CustomerNumberRaw+" ";//Prefill description with customers number.
				FormTaskEdit FormTE=new FormTaskEdit(task,taskOld);
				FormTE.IsNew=true;
				FormTE.Show();
			}
			else {//Not a triage operator.
				if(Plugins.HookMethod(this,"FormOpenDental.phoneSmall_GoToChanged_NotTriage",pat)) {
					return;
				}
				FormCommItem FormCI=new FormCommItem();
				CommItemController commItemController=new CommItemController(FormCI);
				if(commlog==null) {
					commItemController.IsNew=true;
					commlog=new Commlog();
					commlog.PatNum = CurPatNum;
					commlog.CommDateTime = DateTime.Now;
					commlog.CommType =Commlogs.GetTypeAuto(CommItemTypeAuto.MISC);
					commlog.Mode_=CommItemMode.Phone;
					commlog.SentOrReceived=CommSentOrReceived.Received;
					commlog.UserNum=Security.CurUser.UserNum;
				}
				if(FormCI.ShowDialog(new CommItemModel() { CommlogCur=commlog },commItemController)==DialogResult.OK) {
					RefreshCurrentModule();
				}
			}
		}

		private delegate void DelegateSetString(bool hasError,int voiceMailCount,TimeSpan ageOfOldestVoicemail);//typically at namespace level rather than class level

		///<summary>Always called using ThreadVM.</summary>
		private void SetVoiceMailLabel(ODThread odTread) {
			while(true) {
				try {
					List<VoiceMail> listVoiceMails=VoiceMails.GetAll(false,false).FindAll(x => x.UserNum==0);//Only include unclaimed VMs in the count and timer
					DateTime oldestVoicemail=DateTime.MaxValue;
					foreach(VoiceMail voiceMail in listVoiceMails) {
						if(voiceMail.DateCreated.AddSeconds(voiceMail.Duration)<oldestVoicemail) {
							oldestVoicemail=voiceMail.DateCreated.AddSeconds(voiceMail.Duration);//Adding Duration so the timer starts at the end of the VM
						}
					}
					TimeSpan ageOfOldestVoicemail=new TimeSpan(0);
					if(oldestVoicemail!=DateTime.MaxValue) {
						ageOfOldestVoicemail=DateTime.Now-oldestVoicemail;
					}
					Invoke(new DelegateSetString(SetVoicemailMetrics),new object[] { false,listVoiceMails.Count,ageOfOldestVoicemail });					
				}
				catch(ThreadAbortException) {//OnClosing will abort the thread.
					return;//Exits the loop.
				}
				catch(Exception ex) {
					//Something went wrong with determining how many voicemails there are.  Sleep for 4 minutes than try again.
					Invoke(new DelegateSetString(SetVoicemailMetrics),new object[] { true,0,new TimeSpan(0) });
					Thread.Sleep(240000);//4 minutes
					ex.DoNothing();
					continue;
				}
				Thread.Sleep(3000);
			}
		}

		///<summary>Called from worker thread using delegate and Control.Invoke</summary>
		private void SetVoicemailMetrics(bool hasError,int voiceMailCount,TimeSpan ageOfOldestVoicemail) {
			if(hasError) {
				labelMsg.Font=new Font(FontFamily.GenericSansSerif,8.25f,FontStyle.Bold);
				labelMsg.Text="error";
				labelMsg.ForeColor=Color.Firebrick;
				return;
			}
			labelMsg.Text=voiceMailCount.ToString();
			if(voiceMailCount==0) {
				labelMsg.Font=new Font(FontFamily.GenericSansSerif,7.75f,FontStyle.Regular);
				labelMsg.ForeColor=Color.Black;
			}
			else {
				labelMsg.Font=new Font(FontFamily.GenericSansSerif,7.75f,FontStyle.Bold);
				labelMsg.ForeColor=Color.Firebrick;
			}
			foreach(FormMapHQ formMapHQ in _listMaps) {
				formMapHQ.SetVoicemailRed(voiceMailCount,ageOfOldestVoicemail);
			}
			if(formPhoneTiles!=null && !formPhoneTiles.IsDisposed) {
				formPhoneTiles.SetVoicemailCount(voiceMailCount);
			}
		}

		/*private void moduleStaffBilling_GoToChanged(object sender,GoToEventArgs e) {
			if(e.PatNum==0) {
				return;
			}
			CurPatNum=e.PatNum;
			Patient pat=Patients.GetPat(CurPatNum);
			RefreshCurrentModule();
			FillPatientButton(CurPatNum,pat.GetNameLF(),pat.Email!="",pat.ChartNumber);
		}*/

		#region MenuEvents
		private void menuItemLogOff_Click(object sender, System.EventArgs e) {
			UserOdPref logOffMessage=UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.SuppressLogOffMessage).FirstOrDefault();
			if(logOffMessage==null) {//Doesn't exist in the database
				InputBox checkResult=new InputBox(Lan.g(this,"Are you sure you would like to log off?"),Lan.g(this,"Do not show me this message again."),true,new Point(0,40));
				checkResult.ShowDialog();
				if(checkResult.DialogResult==DialogResult.Cancel) {
					return;
				}
				else if(checkResult.DialogResult==DialogResult.OK) {
					if(checkResult.checkBoxResult.Checked) {
						UserOdPrefs.Insert(new UserOdPref() {
							UserNum=Security.CurUser.UserNum,
							FkeyType=UserOdFkeyType.SuppressLogOffMessage
						});
					}
				}
			}
			LogOffNow(false);
		}

		#region File

		//File
		private void menuItemPassword_Click(object sender,EventArgs e) {
			ChangePassword(false);
		}
		
		///<summary>Called to change the password for Security.CurUser.
		///Returns true if password was changed successfully</summary>
		public bool ChangePassword(bool isForcedLogOff) {
			//no security blocking because everyone is allowed to change their own password.
			if(Security.CurUser.UserNumCEMT!=0) {
				MsgBox.Show(this,"Use the CEMT tool to change your password.");
				return false;
			}
			FormUserPassword FormU=new FormUserPassword(false,Security.CurUser.UserName);
			FormU.ShowDialog();
			if(FormU.DialogResult==DialogResult.Cancel) {
				if(isForcedLogOff) {
					LogOffNow(true);
				}
				return false;
			}
			bool isPasswordStrong=FormU.PasswordIsStrong;
			try {
				Userods.UpdatePassword(Security.CurUser,FormU.HashedResult,isPasswordStrong);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return false;
			}
			Security.CurUser.Password=FormU.HashedResult;
			Security.CurUser.PasswordIsStrong=FormU.PasswordIsStrong;
			Security.PasswordTyped=FormU.PasswordTyped;
			DataValid.SetInvalid(InvalidType.Security);
			return true;
		}

		private void menuItemEmailSettings_Click(object sender,EventArgs e) {
			EmailAddress emailAddressCur=EmailAddresses.GetForUser(Security.CurUser.UserNum);
			FormEmailAddressEdit FormEAE=new FormEmailAddressEdit();
			if(emailAddressCur==null) {
				emailAddressCur=new EmailAddress();
				emailAddressCur.UserNum=Security.CurUser.UserNum;
				FormEAE.IsNew=true;
			}
			FormEAE.EmailAddressCur=emailAddressCur;
			FormEAE.ShowDialog();
		}
		
		private void menuItemUserSettings_Click(object sender,EventArgs e) {
			FormUserSetting FormUS=new OpenDental.FormUserSetting();
			FormUS.ShowDialog();
		}

		private void menuItemPrinter_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormPrinterSetup FormPS=new FormPrinterSetup();
			FormPS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Printers");
		}

		private void menuItemGraphics_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.GraphicsEdit)) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			FormGraphics fg=new FormGraphics();
			fg.ShowDialog();
			Cursor=Cursors.Default;
			if(fg.DialogResult==DialogResult.OK) {
				ContrChart2.InitializeLocalData();
				RefreshCurrentModule();
			}
		}

		private void menuItemConfig_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.ChooseDatabase)){
				return;
			}
			SecurityLogs.MakeLogEntry(Permissions.ChooseDatabase,0,"");//make the entry before switching databases.
			ChooseDatabaseModel chooseDatabaseModel=ChooseDatabaseModel.GetChooseDatabaseModelFromConfig();
			chooseDatabaseModel.IsAccessedFromMainMenu=true;
			FormChooseDatabase FormCD=new FormChooseDatabase();
			if(FormCD.ShowDialog(chooseDatabaseModel,new ChooseDatabaseController(FormCD))!=DialogResult.OK) {
				return;
			}
			CurPatNum=0;
			RefreshCurrentModule();//clumsy but necessary. Sets child PatNums to 0.
			FillPatientButton(null);
			if(!PrefsStartup()){
				return;
			}
			RefreshLocalData(InvalidType.AllLocal);
			//RefreshCurrentModule();
			menuItemLogOff_Click(this,e);//this is a quick shortcut.
		}

		private void menuItemExit_Click(object sender, System.EventArgs e) {
			//Thread2.Abort();
			//if(this.TcpListener2!=null){
			//	this.TcpListener2.Stop();  
			//}
			Application.Exit();
		}

		#endregion

		#region Setup

		//FormBackupJobsSelect FormBJS=new FormBackupJobsSelect();
		//FormBJS.ShowDialog();	

		//Setup
		private void menuItemApptFieldDefs_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormApptFieldDefs FormA=new FormApptFieldDefs();
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Appointment Field Defs");
		}

		private void menuItemApptRules_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormApptRules FormA=new FormApptRules();
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Appointment Rules");
		}

		private void menuItemApptTypes_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormApptTypes FormA=new FormApptTypes();
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Appointment Types");
		}

		private void menuItemApptViews_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormApptViews FormAV=new FormApptViews();
			FormAV.ShowDialog();
			RefreshCurrentModule(true);
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Appointment Views");
		}

		private void menuItemAlertCategories_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.SecurityAdmin)){
				return;
			}
			FormAlertCategorySetup FormACS=new FormAlertCategorySetup();
			FormACS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.SecurityAdmin,0,"Alert Categories");
		}

		private void menuItemAutoCodes_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormAutoCode FormAC=new FormAutoCode();
			FormAC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Auto Codes");
		}

		private void menuItemAutomation_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormAutomation FormA=new FormAutomation();
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Automation");
		}

		private void menuItemAutoNotes_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AutoNoteQuickNoteEdit)) {
				return;
			}
			FormAutoNotes FormA=new FormAutoNotes();
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.AutoNoteQuickNoteEdit,0,"Auto Notes Setup");
		}

		private void menuItemClaimForms_Click(object sender, System.EventArgs e) {
			if(PrefC.AtoZfolderUsed==DataStorageType.InDatabase){
				MsgBox.Show(this,"Claim Forms feature is unavailable when data path A to Z folder is disabled.");
				return;
			}
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormClaimForms FormCF=new FormClaimForms();
			FormCF.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Claim Forms");
		}

		private void menuItemClearinghouses_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormClearinghouses FormC=new FormClearinghouses();
			FormC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Clearinghouses");
		}

		private void menuItemDiscountPlans_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormDiscountPlans FormDP=new FormDiscountPlans();
			FormDP.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Discount Plans");
		}

		private void menuItemComputers_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormComputers FormC=new FormComputers();
			FormC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Computers");
		}

		private void menuItemDataPath_Click(object sender, System.EventArgs e) {
			//security is handled from within the form.
			FormPath FormP=new FormPath();
			FormP.ShowDialog();
			CheckCustomReports();
			this.RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Data Path");	
		}

		private void menuItemDefaultCCProcs_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormDefaultCCProcs FormD=new FormDefaultCCProcs();
			FormD.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Default CC Procedures");
		}

		private void menuItemDefinitions_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormDefinitions FormD=new FormDefinitions(DefCat.AccountColors);//just the first cat.
			FormD.ShowDialog();
			RefreshCurrentModule(true);
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Definitions");
		}

		private void menuItemDentalSchools_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormDentalSchoolSetup FormDSS=new FormDentalSchoolSetup();
			FormDSS.ShowDialog();
			RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Dental Schools");
		}

		private void menuItemDisplayFields_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormDisplayFieldCategories FormD=new FormDisplayFieldCategories();
			FormD.ShowDialog();
			RefreshCurrentModule(true);
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Display Fields");
		}

		private void menuItemEmail_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormEmailAddresses FormEA=new FormEmailAddresses();
			FormEA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Email");
		}

		private void menuItemEHR_Click(object sender,EventArgs e) {
			//if(!Security.IsAuthorized(Permissions.Setup)) {
			//  return;
			//}
			FormEhrSetup FormE=new FormEhrSetup();
			FormE.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"EHR");
		}

		private void menuItemFeeScheds_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.FeeSchedEdit)){
				return;
			}
			FormFeeScheds FormF=new FormFeeScheds(false);
			FormF.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.FeeSchedEdit,0,"Fee Schedules");
		}

		private void menuItemFHIR_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormFHIRSetup FormFS=new FormFHIRSetup();
			FormFS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"FHIR");
		}

		private void menuItemHL7_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormHL7Defs FormH=new FormHL7Defs();
			FormH.CurPatNum=CurPatNum;
			FormH.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"HL7");
		}

		private void menuItemImaging_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormImagingSetup FormI=new FormImagingSetup();
			FormI.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Imaging");
		}

		private void menuItemInsCats_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormInsCatsSetup FormE=new FormInsCatsSetup();
			FormE.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Insurance Categories");
		}

		private void menuItemInsFilingCodes_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormInsFilingCodes FormF=new FormInsFilingCodes();
			FormF.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Insurance Filing Codes");
		}

		private void menuItemLaboratories_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			if(Plugins.HookMethod(this,"FormOpenDental.menuItemLaboratories_Click")) {
				return;
			}
			FormLaboratories FormL=new FormLaboratories();
			FormL.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Laboratories");
		}

		private void menuItemLetters_Click(object sender,EventArgs e) {
			FormLetters FormL=new FormLetters();
			FormL.ShowDialog();
		}

		private void menuItemMessaging_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormMessagingSetup FormM=new FormMessagingSetup();
			FormM.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Messaging");
		}

		private void menuItemMessagingButs_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormMessagingButSetup FormM=new FormMessagingButSetup();
			FormM.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Messaging");
		}

		private void menuItemMisc_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormMisc FormM=new FormMisc();
			if(FormM.ShowDialog()!=DialogResult.OK) {
				return;
			}
			RecursiveInvalidate(this);
			//this.Invalidate();
			//menuItemMergeDatabases.Visible=PrefC.GetBool(PrefName.RandomPrimaryKeys");
			//if(timerSignals.Interval==0){
			if(PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs)==0){
				timerSignals.Enabled=false;
			}
			else{
				timerSignals.Interval=PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs)*1000;
				timerSignals.Enabled=true;
			}
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Misc");
		}

		///<summary>Only used when blue theme is changed.</summary>
		private void RecursiveInvalidate(Control control){
			foreach(Control c in control.Controls) {
				RecursiveInvalidate(c);
			}
			control.Invalidate();
		}

		private void menuItemModules_Click(object sender,EventArgs e) {
			LaunchModuleSetupWithTab(0);//Default to Appts tab.
		}

		private void menuItemOrtho_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormOrthoSetup FormOS = new FormOrthoSetup();
			FormOS.ShowDialog();
		}

		private void menuItemPreferencesAppts_Click(object sender,EventArgs e) {
			LaunchModuleSetupWithTab(0);
		}

		private void menuItemPreferencesFamily_Click(object sender,EventArgs e) {
			LaunchModuleSetupWithTab(1);
		}

		private void menuItemPreferencesAccount_Click(object sender,EventArgs e) {
			LaunchModuleSetupWithTab(2);
		}

		private void menuItemPreferencesTreatPlan_Click(object sender,EventArgs e) {
			LaunchModuleSetupWithTab(3);
		}

		private void menuItemPreferencesChart_Click(object sender,EventArgs e) {
			LaunchModuleSetupWithTab(4);
		}

		private void menuItemPreferencesImages_Click(object sender,EventArgs e) {
			LaunchModuleSetupWithTab(5);
		}

		private void menuItemPreferencesManage_Click(object sender,EventArgs e) {
			LaunchModuleSetupWithTab(6);
		}

		///<summary>Checks setup permission, launches the module setup window with the specified tab and then makes an audit entry.
		///This is simply a helper method because every preferences menu item will do the exact same code.</summary>
		private void LaunchModuleSetupWithTab(int selectedTab) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormModuleSetup FormM=new FormModuleSetup(selectedTab);
			if(FormM.ShowDialog()!=DialogResult.OK) {
				return;
			}
			FillPatientButton(Patients.GetPat(CurPatNum));
			RefreshCurrentModule(true);
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Modules");
		}

		private void menuItemOperatories_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormOperatories FormO=new FormOperatories();
			FormO.ShowDialog();
			if(FormO.ListConflictingAppts.Count > 0) {
				FormApptConflicts FormAC=new FormApptConflicts(FormO.ListConflictingAppts);
				FormAC.Show();
				FormAC.BringToFront();
			}
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Operatories");
		}

		private void menuItemPatFieldDefs_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormPatFieldDefs FormP=new FormPatFieldDefs();
			FormP.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Patient Field Defs");
		}

		private void menuItemPayerIDs_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormElectIDs FormE=new FormElectIDs();
			FormE.IsSelectMode=false;
			FormE.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Payer IDs");
		}

		private void menuItemPractice_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormPractice FormPr=new FormPractice();
			FormPr.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Practice Info");
			if(FormPr.DialogResult!=DialogResult.OK) {
				return;
			}
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) { //Changes the Chart and Treatment Plan icons to ones without teeth
				myOutlookBar.Buttons[3].ImageIndex=7;
				myOutlookBar.Buttons[4].ImageIndex=8;
			}
			else { //Change back to normal icons
				myOutlookBar.Buttons[3].ImageIndex=3;
				myOutlookBar.Buttons[4].ImageIndex=4;
			}
			RefreshCurrentModule();
		}

		private void menuItemProblems_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormDiseaseDefs FormD=new FormDiseaseDefs();
			FormD.ShowDialog();
			//RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Disease Defs");
		}

		private void menuItemProcedureButtons_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormProcButtons FormPB=new FormProcButtons();
			FormPB.Owner=this;
			FormPB.ShowDialog();
			SetModuleSelected();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Procedure Buttons");	
		}

		private void menuItemLinks_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormProgramLinks FormPL=new FormProgramLinks();
			FormPL.ShowDialog();
			ContrChart2.InitializeLocalData();//for eCW
			LayoutToolBar();
			RefreshMenuReports();
			if(CurPatNum>0) {
				Patient pat=Patients.GetPat(CurPatNum);
				FillPatientButton(pat);
			}
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Program Links");
		}

		/*
		private void menuItem_ProviderAllocatorSetup_Click(object sender,EventArgs e) {
			// Check Permissions
			if(!Security.IsAuthorized(Permissions.Setup)) {
				// Failed security prompts message box. Consider adding overload to not show message.
				//MessageBox.Show("Not Authorized to Run Setup for Provider Allocation Tool");
				return;
			}
			Reporting.Allocators.MyAllocator1.FormInstallAllocator_Provider fap = new OpenDental.Reporting.Allocators.MyAllocator1.FormInstallAllocator_Provider();
			fap.ShowDialog();
		}*/

		private void menuItemAsapList_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormAsapSetup FormAS=new FormAsapSetup();
			FormAS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"ASAP List Setup");
		}

		private void menuItemConfirmations_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormConfirmationSetup FormCS=new FormConfirmationSetup();
			FormCS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Confirmation Setup");
		}

		private void menuItemInsVerify_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormInsVerificationSetup FormIV=new FormInsVerificationSetup();
			FormIV.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Insurance Verification");
        }

        private void menuItemQuestions_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormQuestionDefs FormQ=new FormQuestionDefs();
			FormQ.ShowDialog();
			//RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Questionnaire");
		}

		private void menuItemRecall_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormRecallSetup FormRS=new FormRecallSetup();
			FormRS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Recall");	
		}

		private void menuItemRecallTypes_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormRecallTypes FormRT=new FormRecallTypes();
			FormRT.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Recall Types");	
		}

		private void menuItemReplication_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.ReplicationSetup)) {
				return;
			}
			FormReplicationSetup FormRS=new FormReplicationSetup();
			FormRS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.ReplicationSetup,0,"Replication setup.");
		}
		
		private void menuItemReports_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormReportSetup FormRS=new FormReportSetup(0,false);
			FormRS.ShowDialog();
		}

		private void menuItemRequiredFields_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormRequiredFields FormRF=new FormRequiredFields();
			FormRF.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Required Fields");
		}

		private void menuItemRequirementsNeeded_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormReqNeededs FormR=new FormReqNeededs();
			FormR.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Requirements Needed");
		}

		private void menuItemSched_Click(object sender,EventArgs e) {
			//anyone should be able to view. Security must be inside schedule window.
			//if(!Security.IsAuthorized(Permissions.Schedules)) {
			//	return;
			//}
			FormSchedule FormS=new FormSchedule();
			FormS.ShowDialog();
			//SecurityLogs.MakeLogEntry(Permissions.Schedules,0,"");
		}

		/*private void menuItemBlockoutDefault_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Blockouts)) {
				return;
			}
			FormSchedDefault FormSD=new FormSchedDefault(ScheduleType.Blockout);
			FormSD.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Blockouts,0,"Default");
		}*/

		public static void S_MenuItemSecurity_Click(object sender,EventArgs e) {
			_formOpenDentalS.menuItemSecuritySettings_Click(sender,e);
		}

		private void menuItemSecuritySettings_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.SecurityAdmin)) {
				return;
			}
			FormSecurity FormS=new FormSecurity();
			FormS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.SecurityAdmin,0,"Security Window");
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {//clinics not enabled, refresh current module and return
				RefreshCurrentModule();
				return;
			}
			//clinics is enabled
			if(Security.CurUser.ClinicIsRestricted) {
				Clinics.ClinicNum=Security.CurUser.ClinicNum;
			}
			Text=PatientL.GetMainTitle(Patients.GetPat(CurPatNum),Clinics.ClinicNum);
			SetSmsNotificationText();
			RefreshMenuClinics();//this calls ModuleSelected, so no need to call RefreshCurrentModule
		}

		private void menuItemAddUser_Click(object sender,EventArgs e) {
			if(!(Security.IsAuthorized(Permissions.AddNewUser,true) || (Security.IsAuthorized(Permissions.SecurityAdmin,true)))) 
			{
				MsgBox.Show(this,"Not authorized to add a new user.");
				return;
			}
			if(PrefC.GetLong(PrefName.DefaultUserGroup)==0) {
				MsgBox.Show(this,"Default user group is not set. Make sure to set a default user group.");
				return;
			}
			FormUserEdit FormUE=new FormUserEdit(new Userod(),true);
			FormUE.IsNew=true;
			FormUE.ShowDialog();
		}

		private void menuItemSheets_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormSheetDefs FormSD=new FormSheetDefs();
			FormSD.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Sheets");
		}

		//This shows as "Show Features"
		private void menuItemEasy_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormShowFeatures FormE=new FormShowFeatures();
			FormE.ShowDialog();
			ContrAccount2.LayoutToolBar();//for repeating charges
			RefreshCurrentModule(true);
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Show Features");
		}

		private void menuItemSpellCheck_Click(object sender,EventArgs e) {
			FormSpellCheck FormD=new FormSpellCheck();
			FormD.ShowDialog();
		}

		private void menuItemTimeCards_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormTimeCardSetup FormTCS=new FormTimeCardSetup();
			FormTCS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Time Card Setup");
		}

		private void menuItemTask_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormTaskPreferences formTaskSetup = new FormTaskPreferences();
			if(formTaskSetup.ShowDialog()!=DialogResult.OK) {
				return;
			}
			if(userControlTasks1.Visible) {
				userControlTasks1.InitializeOnStartup();
			}
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Task");
		}
		
		private void menuItemQuickPasteNotes_Click(object sender,EventArgs e) {
			FormQuickPaste formQP=new FormQuickPaste();
			formQP.QuickType=QuickPasteType.None;
			formQP.ShowDialog();
		}

		private void menuItemWebForm_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormWebFormSetup formWFS=new FormWebFormSetup();
			formWFS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Web Forms Setup");
		}

		#endregion

		#region Lists

		//Lists
		private void menuItemProcCodes_Click(object sender, System.EventArgs e) {
			//security handled within form
			FormProcCodes FormP=new FormProcCodes(true);
			FormP.ProcCodeSort=(ProcCodeListSort)PrefC.GetInt(PrefName.ProcCodeListSortOrder);
			FormP.ShowDialog();	
		}

		private void menuItemAllergies_Click(object sender,EventArgs e) {
			new FormAllergySetup().ShowDialog();
		}

		private void menuItemClinics_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormClinics FormC=new FormClinics();
			FormC.IncludeHQInList=true;
			FormC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Clinics");
			//this menu item is only visible if the clinics show feature is enabled (!EasyNoClinics)
			if(Clinics.GetDesc(Clinics.ClinicNum)=="") {//will be empty string if ClinicNum is not valid, in case they deleted the clinic
				Clinics.ClinicNum=Security.CurUser.ClinicNum;
				SetSmsNotificationText();
			}
			RefreshMenuClinics();
			//reset the main title bar in case the user changes the clinic description for the selected clinic
			Patient pat=Patients.GetPat(CurPatNum);
			Text=PatientL.GetMainTitle(pat,Clinics.ClinicNum);
			//reset the tip text in case the user changes the clinic description
		}
		
		private void menuItemContacts_Click(object sender, System.EventArgs e) {
			FormContacts FormC=new FormContacts();
			FormC.ShowDialog();
		}

		private void menuItemCounties_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormCounties FormC=new FormCounties();
			FormC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Counties");
		}

		private void menuItemSchoolClass_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormSchoolClasses FormS=new FormSchoolClasses();
			FormS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Dental School Classes");
		}

		private void menuItemSchoolCourses_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormSchoolCourses FormS=new FormSchoolCourses();
			FormS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Dental School Courses");
		}

		private void menuItemEmployees_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormEmployeeSelect FormEmp=new FormEmployeeSelect();
			FormEmp.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Employees");	
		}

		private void menuItemEmployers_Click(object sender, System.EventArgs e) {
			FormEmployers FormE=new FormEmployers();
			FormE.ShowDialog();
		}

		private void menuItemInstructors_Click(object sender, System.EventArgs e) {
			/*if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormInstructors FormI=new FormInstructors();
			FormI.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Dental School Instructors");*/
		}

		private void menuItemCarriers_Click(object sender, System.EventArgs e) {
			FormCarriers FormC=new FormCarriers();
			FormC.ShowDialog();
			RefreshCurrentModule();
		}

		private void menuItemInsPlans_Click(object sender, System.EventArgs e) {
			FormInsPlans FormIP = new FormInsPlans();
			FormIP.ShowDialog();
			RefreshCurrentModule(true);
		}

		private void menuItemJobManager_Click(object sender,System.EventArgs e) {
			if(_formJobManager2==null || _formJobManager2.IsDisposed) {
				_formJobManager2=new FormJobManager2();
			}
			_formJobManager2.Show();
			if(_formJobManager2.WindowState==FormWindowState.Minimized) {
				_formJobManager2.WindowState=FormWindowState.Normal;
			}
			_formJobManager2.BringToFront();
		}

		public static void S_GoToJob(long jobNum) {
			_formOpenDentalS.GoToJob(jobNum);
		}

		///<summary>Can be called from anywhere in OD layer to load job. 
		///It is in FormOpenDental because this is where the static reference to theJob Manager is.</summary>
		private void GoToJob(long jobNum) {
			if(_formJobManager2==null || _formJobManager2.IsDisposed) {
				_formJobManager2=new FormJobManager2();
			}
			_formJobManager2.Show();
			if(_formJobManager2.WindowState==FormWindowState.Minimized) {
				_formJobManager2.WindowState=FormWindowState.Normal;
			}
			_formJobManager2.BringToFront();
			_formJobManager2.GoToJob(jobNum);
		}

		private void menuItemLabCases_Click(object sender,EventArgs e) {
			FormLabCases FormL=new FormLabCases();
			FormL.ShowDialog();
			if(FormL.GoToAptNum!=0) {
				Appointment apt=Appointments.GetOneApt(FormL.GoToAptNum);
				Patient pat=Patients.GetPat(apt.PatNum);
				S_Contr_PatientSelected(pat,false);
				//OnPatientSelected(pat.PatNum,pat.GetNameLF(),pat.Email!="",pat.ChartNumber);
				GotoModule.GotoAppointment(apt.AptDateTime,apt.AptNum);
			}
		}

		private void menuItemMedications_Click(object sender, System.EventArgs e) {
			FormMedications FormM=new FormMedications();
			FormM.ShowDialog();
		}

		private void menuItemPharmacies_Click(object sender,EventArgs e) {
			FormPharmacies FormP=new FormPharmacies();
			FormP.ShowDialog();
		}

		private void menuItemProviders_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Providers,true) && !Security.IsAuthorized(Permissions.AdminDentalStudents,true)) {
				MessageBox.Show(Lans.g("Security","Not authorized for")+"\r\n"
					+GroupPermissions.GetDesc(Permissions.Providers)+" "+Lans.g("Security","or")+" "+GroupPermissions.GetDesc(Permissions.AdminDentalStudents));
				return;
			}
			FormProviderSetup FormPS=new FormProviderSetup();
			FormPS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Providers");		
		}

		private void menuItemPrescriptions_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormRxSetup FormRxSetup2=new FormRxSetup();
			FormRxSetup2.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Rx");		
		}

		private void menuItemReferrals_Click(object sender, System.EventArgs e) {
			FormReferralSelect FormRS=new FormReferralSelect();
			FormRS.ShowDialog();		
		}

		private void menuItemSites_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormSites FormS=new FormSites();
			FormS.ShowDialog();
			RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Sites");
		}

		private void menuItemStateAbbrs_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormStateAbbrs formSA=new FormStateAbbrs();
			formSA.ShowDialog();
			RefreshCurrentModule();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"StateAbbrs");
		}

		private void menuItemZipCodes_Click(object sender, System.EventArgs e) {
			//if(!Security.IsAuthorized(Permissions.Setup)){
			//	return;
			//}
			FormZipCodes FormZ=new FormZipCodes();
			FormZ.ShowDialog();
			//SecurityLogs.MakeLogEntry(Permissions.Setup,"Zip Codes");
		}

		#endregion

		#region Reports

		private void menuItemReportsStandard_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Reports)) {
				return;
			}
			FormReportsMore FormR=new FormReportsMore();
			FormR.ShowDialog();
			NonModalReportSelectionHelper(FormR.RpNonModalSelection);
		}

		private void menuItemReportsGraphic_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.GraphicalReports)) {
				return;
			}
			if(_formDashboardEditTab!=null) {
				_formDashboardEditTab.BringToFront();
				return;
			}
			//on extremely large dbs, the ctor can take a few seconds to load, so show the wait cursor.
			Cursor=Cursors.WaitCursor;
			_formDashboardEditTab=new OpenDentalGraph.FormDashboardEditTab() { IsEditMode=false,};
			_formDashboardEditTab.FormClosed+=new FormClosedEventHandler((object senderF,FormClosedEventArgs eF) => { _formDashboardEditTab=null; });
			Cursor=Cursors.Default;
			_formDashboardEditTab.Show();
		}
				
		private void menuItemReportsUserQuery_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.UserQuery)) {
				return;
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				MsgBox.Show(this,"Not allowed while using Oracle.");
				return;
			}
			if(Security.IsAuthorized(Permissions.UserQueryAdmin,true)) {
				SecurityLogs.MakeLogEntry(Permissions.UserQuery,0,Lan.g(this,"User query form accessed."));
				if(_formUserQuery!=null) {
					_formUserQuery.BringToFront();
					return;
				}
				_formUserQuery=new FormQuery(null);
				_formUserQuery.FormClosed+=new FormClosedEventHandler((object senderF,FormClosedEventArgs eF) => { _formUserQuery=null; });
				_formUserQuery.Show();
			}
			else {
				FormQueryFavorites FormQF = new FormQueryFavorites();
				FormQF.ShowDialog();
				if(FormQF.DialogResult == DialogResult.OK) {
					ExecuteQueryFavorite(FormQF.UserQueryCur);
				}
			}
		}

		private void menuItemReportsUnfinalizedPay_Click(object sender,EventArgs e) {
			FormRpUnfinalizedInsPay formRp=new FormRpUnfinalizedInsPay();
			formRp.ShowDialog();
		}

		private void UpdateUnfinalizedPayCount(List<Signalod> listSignals) {
			if(listSignals.Count==0) {
				menuItemReportsUnfinalizedPay.Text=Lan.g(this,"Unfinalized Payments");
				return;
			}
			Signalod signal=listSignals.OrderByDescending(x => x.SigDateTime).First();
			menuItemReportsUnfinalizedPay.Text=Lan.g(this,"Unfinalized Payments")+": "+signal.MsgValue;
		}

		private void RefreshMenuReports() {
			//Find the index of the last separator which separates the static menu items from the dynamic menu items.
			int separatorIndex=-1;
			for(int i=0;i<menuItemReportsHeader.MenuItems.Count;i++) {
				if(menuItemReportsHeader.MenuItems[i].Text=="-") {
					separatorIndex=i;
				}
			}
			//Remove dynamic items and separator.  Leave hard coded items.
			if(separatorIndex!=-1) {
				for(int i=menuItemReportsHeader.MenuItems.Count-1;i>=separatorIndex;i--) {
					menuItemReportsHeader.MenuItems.RemoveAt(i);
				}
			}
			List<ToolButItem> listToolButItems=ToolButItems.GetForToolBar(ToolBarsAvail.ReportsMenu);
			if(listToolButItems.Count==0) {
				return;//Return early to avoid adding a useless separator in the menu.
			}
			//Add separator, then dynamic items to the bottom of the menu.
			menuItemReportsHeader.MenuItems.Add("-");//Separator			
			listToolButItems.Sort(ToolButItem.Compare);//Alphabetical order
			foreach(ToolButItem toolButItemCur in listToolButItems) {
				MenuItem menuItem=new MenuItem(toolButItemCur.ButtonText,menuReportLink_Click);
				menuItem.Tag=toolButItemCur;
				menuItemReportsHeader.MenuItems.Add(menuItem);
			}
		}

		private void menuReportLink_Click(object sender,System.EventArgs e) {
			MenuItem menuItem=(MenuItem)sender;
			ToolButItem toolButItemCur=((ToolButItem)menuItem.Tag);			
			ProgramL.Execute(toolButItemCur.ProgramNum,Patients.GetPat(CurPatNum));
		}

		#endregion

		#region CustomReports

		//Custom Reports
		private void menuItemRDLReport_Click(object sender,System.EventArgs e) {
			//This point in the code is only reached if the A to Z folders are enabled, thus
			//the image path should exist.
			FormReportCustom FormR=new FormReportCustom();
			FormR.SourceFilePath=
				ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),PrefC.GetString(PrefName.ReportFolderName),((MenuItem)sender).Text+".rdl");
			FormR.ShowDialog();
		}

		#endregion

		#region Tools

		//Tools
		private void menuItemPrintScreen_Click(object sender, System.EventArgs e) {
			FormPrntScrn FormPS=new FormPrntScrn();
			FormPS.ShowDialog();
		}

		#region MiscTools
		private void menuItemDuplicateBlockouts_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormBlockoutDuplicatesFix form=new FormBlockoutDuplicatesFix();
			Cursor=Cursors.WaitCursor;
			form.ShowDialog();
			Cursor=Cursors.Default;
			//Security log entries are made from within the form.
		}

		private void menuItemCreateAtoZFolders_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormAtoZFoldersCreate FormA=new FormAtoZFoldersCreate();
			FormA.ShowDialog();
			//Security log entries are made from within the form.
		}

		private void menuItemDatabaseMaintenancePat_Click(object sender,EventArgs e) {
			//Purposefully not checking permissions.  All users need the ability to call patient specific DBMs ATM.
			//Get all patient specific DBM methods via reflection.
			List<MethodInfo> listPatDbmMethods=DatabaseMaintenances.GetMethodsForDisplay(Clinics.ClinicNum,true);
			//Add any missing patient specific DBM methods to the database that are not currently present.
			DatabaseMaintenances.InsertMissingDBMs(listPatDbmMethods.Select(x => x.Name).ToList());
			//Get the names of all DBM methods that are not hidden.
			List<string> listNonHiddenDbmMethods=DatabaseMaintenances.GetAll()
				.FindAll(x => x.IsHidden==false && x.IsOld==false)
				.Select(y => y.MethodName).ToList();
			//Filter down our list of patient specific DBM methods found via reflection based on hidden status.
			listPatDbmMethods.RemoveAll(x => !x.Name.In(listNonHiddenDbmMethods));
			if(listPatDbmMethods.Count==0) {
				MsgBox.Show(this,"All patient database maintenance methods are marked as hidden.");
				return;
			}
			FormDatabaseMaintenancePat FormDMP=new FormDatabaseMaintenancePat(listPatDbmMethods,CurPatNum);
			FormDMP.ShowDialog();
		}

		private void menuItemMergeDPs_Click(object sender,EventArgs e) {
			FormDiscountPlanMerge FormDPM=new FormDiscountPlanMerge();
			FormDPM.ShowDialog();
		}

		private void menuItemMergeMedications_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.MedicationMerge)) {
				return;
			}
			FormMedicationMerge FormMM=new FormMedicationMerge();
			FormMM.ShowDialog();
			//Securitylog entries are handled within the form.
		}

		private void menuItemMergePatients_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.PatientMerge)) {
				return;
			}
			FormPatientMerge fpm=new FormPatientMerge();
			fpm.ShowDialog();
			//Security log entries are made from within the form.
		}

		private void menuItemMergeReferrals_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.ReferralMerge)) {
				return;
			}
			FormReferralMerge FormRM=new FormReferralMerge();
			FormRM.ShowDialog();
			//Security log entries are made from within the form.
		}

		private void menuItemMergeProviders_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.ProviderMerge)) {
				return;
			}
			FormProviderMerge FormPM=new FormProviderMerge();
			FormPM.ShowDialog();
			//Security log entries are made from within the form.
		}

		private void menuItemMoveSubscribers_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsPlanChangeSubsc)) {
				return;
			}
			FormSubscriberMove formSM=new FormSubscriberMove();
			formSM.ShowDialog();
			//Security log entries are made from within the form.
		}

		private void menuItemProcLockTool_Click(object sender,EventArgs e) {
			FormProcLockTool FormT=new FormProcLockTool();
			FormT.ShowDialog();
			//security entries made inside the form
			//SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Proc Lock Tool");
		}

		private void menuItemSetupWizard_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormSetupWizard FormSW = new FormSetupWizard();
			FormSW.ShowDialog();
		}

		private void menuItemServiceManager_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormMain FormM=new FormMain();
			FormM.ShowDialog();
		}

		private void menuItemShutdown_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormShutdown FormS=new FormShutdown();
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			//turn off signal reception for 5 seconds so this workstation will not shut down.
			Signalods.SignalLastRefreshed=MiscData.GetNowDateTime().AddSeconds(5);
			Signalod sig=new Signalod();
			sig.IType=InvalidType.ShutDownNow;
			Signalods.Insert(sig);
			Computers.ClearAllHeartBeats(Environment.MachineName);//always assume success
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Shutdown all workstations.");
		}
		
		private void menuTelephone_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormTelephone FormT=new FormTelephone();
			FormT.ShowDialog();
			//Security log entries are made from within the form.
		}

		private void menuItemTestLatency_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormTestLatency formTL=new FormTestLatency();
			formTL.ShowDialog();
		}
		
		private void menuItemXChargeReconcile_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Accounting)) {
				return;
			}
			FormXChargeReconcile FormXCR=new FormXChargeReconcile();
			FormXCR.ShowDialog();
		}
		#endregion MiscTools

		private void menuItemAging_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormAging FormAge=new FormAging();
			FormAge.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Aging Update");
		}

		private void menuItemAuditTrail_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AuditTrail)) {
				return;
			}
			FormAudit FormA=new FormAudit();
			FormA.CurPatNum=CurPatNum;
			FormA.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.AuditTrail,0,"Audit Trail");
		}

		private void menuItemFinanceCharge_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormFinanceCharges FormFC=new FormFinanceCharges();
			FormFC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Run Finance Charges");
		}

		private void menuItemCCRecurring_Click(object sender,EventArgs e) {
			if(FormCRC==null || FormCRC.IsDisposed) {
				FormCRC=new FormCreditRecurringCharges();
			}
			Cursor=Cursors.WaitCursor;
			FormCRC.Show();
			Cursor=Cursors.Default;
			if(FormCRC.WindowState==FormWindowState.Minimized) {
				FormCRC.WindowState=FormWindowState.Normal;
			}
			FormCRC.BringToFront();
		}

		private void menuItemCustomerManage_Click(object sender,EventArgs e) {
			FormCustomerManagement FormC=new FormCustomerManagement();
			FormC.ShowDialog();
			if(FormC.SelectedPatNum!=0) {
				CurPatNum=FormC.SelectedPatNum;
				Patient pat=Patients.GetPat(CurPatNum);
				RefreshCurrentModule();
				FillPatientButton(pat);
			}
		}

		private void menuItemDatabaseMaintenance_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			FormDatabaseMaintenance FormDM=new FormDatabaseMaintenance();
			FormDM.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Database Maintenance");
		}

		private void menuItemDispensary_Click(object sender,System.EventArgs e) {
			FormDispensary FormD=new FormDispensary();
			FormD.ShowDialog();
		}

		private void menuItemEvaluations_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AdminDentalEvaluations,true) && (Security.CurUser.ProvNum==0 || Providers.GetProv(Security.CurUser.ProvNum).SchoolClassNum!=0)) {
				MsgBox.Show(this,"Only Instructors may view or edit evaluations.");
				return;
			}
			FormEvaluations FormE=new FormEvaluations();
			FormE.ShowDialog();
		}

		private void menuItemTerminal_Click(object sender,EventArgs e) {
			if(PrefC.GetLong(PrefName.ProcessSigsIntervalInSecs)==0) {
				MsgBox.Show(this,"Cannot open terminal unless process signal interval is set. To set it, go to Setup > Miscellaneous.");
				return;
			}
			FormTerminal FormT=new FormTerminal();
			FormT.ShowDialog(); 
			Application.Exit();//always close after coming out of terminal mode as a safety precaution.*/
		}

		private void menuItemTerminalManager_Click(object sender,EventArgs e) {
			if(formTerminalManager==null || formTerminalManager.IsDisposed) {
				formTerminalManager=new FormTerminalManager();
			}
			formTerminalManager.Show();
			formTerminalManager.BringToFront();
		}

		private void menuItemTranslation_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormTranslationCat FormTC=new FormTranslationCat();
			FormTC.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Translations");
		}

		private void menuItemMobileSetup_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)){
				return;
			}
			ShowEServicesSetup(FormEServicesSetup.EService.MobileOld);
		}

		private void menuItemNewCropBilling_Click(object sender,EventArgs e) {
			FormNewCropBilling FormN=new FormNewCropBilling();
			FormN.ShowDialog();
		}

		private void menuItemPendingPayments_Click(object sender,EventArgs e) {
			FormPendingPayments FormPP=new FormPendingPayments();
			FormPP.Show();//Non-modal so the user can view the patient's account
		}

		private void menuItemRepeatingCharges_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.RepeatChargeTool)) {
				return;
			}
			FormRepeatChargesUpdate FormR=new FormRepeatChargesUpdate();
			FormR.ShowDialog();
		}

		private void menuItemResellers_Click(object sender,EventArgs e) {
			FormResellers FormR=new FormResellers();
			FormR.ShowDialog();
		}

		private void menuItemScreening_Click(object sender,System.EventArgs e) {
			FormScreenGroups FormS=new FormScreenGroups();
			FormS.ShowDialog();
		}

		private void menuItemReqStudents_Click(object sender,EventArgs e) {
			Provider prov=Providers.GetProv(Security.CurUser.ProvNum);
			if(prov==null) {
				MsgBox.Show(this,"The current user is not attached to a provider. Attach the user to a provider to gain access to this feature.");
				return;
			}
			if(!prov.IsInstructor){//if a student is logged in
				//the student always has permission to view their own requirements
				FormReqStudentOne FormO=new FormReqStudentOne();
				FormO.ProvNum=prov.ProvNum;
				FormO.ShowDialog();
				return;
			}
			if(prov.IsInstructor) {
				FormReqStudentsMany FormM=new FormReqStudentsMany();
				FormM.ShowDialog();
			}
		}

		private void menuItemWebForms_Click(object sender,EventArgs e) {
			FormWebForms FormWF = new FormWebForms();
			FormWF.Show();
		}

		private void menuItemWiki_Click(object sender,EventArgs e) {
			if(Plugins.HookMethod(this,"FormOpenDental.menuItemWiki_Click")) {
				return;
			}
			//We want to allow as many wiki pages open as possible to maximize efficiency in the office.
			new FormWiki().Show();
		}

		private void menuItemXWebTrans_Click(object sender,EventArgs e) {
			if(FormXWT==null || FormXWT.IsDisposed) {
				FormXWT=new FormXWebTransactions();
				FormXWT.FormClosed+=new FormClosedEventHandler((o,e1) => { FormXWT=null; });
				FormXWT.Show();
			}
			if(FormXWT.WindowState==FormWindowState.Minimized) {
				FormXWT.WindowState=FormWindowState.Normal;
			}
			FormXWT.BringToFront();
		}

		public static void S_WikiLoadPage(string pageTitle) {
			if(!PrefC.GetBool(PrefName.WikiCreatePageFromLink) && !WikiPages.CheckPageNamesExist(new List<string>{ pageTitle })[0]) {
				MsgBox.Show("FormOpenDental","Wiki page does not exist.");
				return;
			}
			FormWiki FormW=new FormWiki();
			FormW.Show();
			FormW.LoadWikiPagePublic(pageTitle);//This has to be after the form has loaded
		}

		private void menuItemAutoClosePayPlans_Click(object sender,EventArgs e) {
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				MsgBox.Show(this,"Tool does not currently support Oracle.  Please call support to see if you need this fix.");
				return;
			}
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Running this tool will automatically mark all payment plans that have"
				+" been paid off and have no future charges as closed.  Do you want to continue?")) 
			{
				return;
			}
			long plansClosed=PayPlans.AutoClose(); //returns # of payplans closed.
			string msgText;
			if(plansClosed>0) {
				msgText=Lan.g(this,"Success.")+"  "+plansClosed+" "+Lan.g(this,"plan(s) closed.");
			}
			else {
				msgText=Lan.g(this,"There were no plans to close.");
			}
			MessageBox.Show(msgText);
		}

		private void menuItemAutoOrtho_Click(object sender,EventArgs e) {
			FormOrthoAutoClaims FormOAC = new FormOrthoAutoClaims();
			FormOAC.ShowDialog();
		}

		#endregion

		#region Clinics
		//menuClinics is a dynamic menu that is maintained within RefreshMenuClinics()
		#endregion

		#region eServices

		private void ShowEServicesSetup(FormEServicesSetup.EService eService) {
			FormEServicesSetup FormESS=new FormEServicesSetup(eService);
			FormESS.ShowDialog();
			if(_butText!=null) { //User may just have signed up for texting.
				_butText.Enabled=Programs.IsEnabled(ProgramName.CallFire)||SmsPhones.IsIntegratedTextingEnabled();
			}
		}

		private void menuItemSignup_Click(object sender,EventArgs e) {
			ShowEServicesSetup(FormEServicesSetup.EService.SignupPortal);
		}

		private void menuMobileWeb_Click(object sender,EventArgs e) {
			ShowEServicesSetup(FormEServicesSetup.EService.MobileNew);
		}

		private void menuItemPatientPortal_Click(object sender,EventArgs e) {
			ShowEServicesSetup(FormEServicesSetup.EService.PatientPortal);
		}

		private void menuItemIntegratedTexting_Click(object sender,EventArgs e) {
			ShowEServicesSetup(FormEServicesSetup.EService.SmsService);
		}

		private void menuItemWebSched_Click(object sender,EventArgs e) {
			ShowEServicesSetup(FormEServicesSetup.EService.WebSched);
		}

		private void menuItemEConfirmRemind_Click(object sender,EventArgs e) {
			ShowEServicesSetup(FormEServicesSetup.EService.eConfirmRemind);
		}

		private void menuItemEMisc_Click(object sender,EventArgs e) {
			ShowEServicesSetup(FormEServicesSetup.EService.eMisc);
		}

		private void menuItemListenerService_Click(object sender,EventArgs e) {
			ShowEServicesSetup(FormEServicesSetup.EService.ListenerService);
		}

		#endregion

		#region Alerts

		private void menuItemAlerts_Popup(object sender,EventArgs e) {
			CheckAlerts(false);
		}

		///<summary>Handles the drawing and coloring for the Alerts menu and its sub items.</summary>
		private void menuItemAlerts_DrawItem(object sender,DrawItemEventArgs e) {
			MenuItem menuItem=(MenuItem)sender;
			AlertItem alertItem=((AlertItem)menuItem.Tag);//Can be Null
			Color colorText=SystemColors.MenuText;
			Color backGroundColor=SystemColors.Control;
			if(menuItem==menuItemAlerts) {
				if(_listAlertItems!=null && _listAlertReads!=null) {
					List<long> listAlertItemNums=_listAlertItems.Select(x => x.AlertItemNum).ToList();//All alert nums for current alertItems.
					List<long> listAlertReadItemNums=_listAlertReads.Select(x => x.AlertItemNum).ToList();//All alert nums for read alertItems.
					if(!menuItemNoAlerts.Visible && //menuItemNoAlerts is only Visible when there are no AlertItems to show.
							!listAlertItemNums.All(x => listAlertReadItemNums.Contains(x)))
					{
						//Max SeverityType for all unread AlertItems.
						SeverityType maxSeverity=_listAlertItems.FindAll(x => !listAlertReadItemNums.Contains(x.AlertItemNum)).Select(x => x.Severity).Max();
						backGroundColor=AlertBackgroudColorHelper(maxSeverity);
						colorText=AlertTextColorHelper(maxSeverity);
					}
					else {//Either there are no AlertItems to show or they all have an AlertRead row.
						colorText=SystemColors.MenuText;
					}
				}
			}
			else if(menuItem==menuItemNoAlerts) {
				//Keep this menuItem colors as system defaults.
			}
			else {//This is an alert menuItem.
				if(!_listAlertReads.Exists(x => x.AlertItemNum==alertItem.AlertItemNum)) {//User has not acknowleged alert yet.
					backGroundColor=AlertBackgroudColorHelper(alertItem.Severity);
					colorText=AlertTextColorHelper(alertItem.Severity);
				}
				else {//User has an AlertRead row for this AlertItem.
						colorText=SystemColors.MenuText;
				}
			}
			if(!menuItem.Enabled || e.State==(DrawItemState.NoAccelerator | DrawItemState.Inactive)) {
				colorText=SystemColors.ControlDark;
			}
			//Check if selected or hovering over.
			if(e.State==(DrawItemState.NoAccelerator | DrawItemState.Selected) 
				|| e.State==(DrawItemState.NoAccelerator | DrawItemState.HotLight)) 
			{
				if(backGroundColor==Color.OrangeRed || backGroundColor==Color.DarkOrange) {
					colorText=Color.Yellow;
				}
				else if(backGroundColor==Color.LightGoldenrodYellow) {
					colorText=Color.OrangeRed;
				}
				else {
					backGroundColor=SystemColors.Highlight;
					colorText=SystemColors.HighlightText;
				}
			}
			using(SolidBrush brushBackground=new SolidBrush(backGroundColor))
			using(SolidBrush brushFont=new SolidBrush(colorText)) {
				//Get the text that is displaying from the menu item compenent.
				string menuText=menuItem.Text;
				//Create a string format to center the text to mimic the other menu items.
				StringFormat stringFormat=new StringFormat();
				stringFormat.Alignment=StringAlignment.Center;
				if(menuItem!=menuItemAlerts) {
					stringFormat.Alignment=StringAlignment.Near;
				}
				Rectangle rect=new Rectangle(e.Bounds.X,e.Bounds.Y,e.Bounds.Width,e.Bounds.Height);//Copy e.bounds as default.
				if(menuItem!=menuItemAlerts) {
					rect=new Rectangle(e.Bounds.X,e.Bounds.Y,e.Bounds.Width+30,e.Bounds.Height);//Sub menu items need some extra width.
				}
				e.Graphics.FillRectangle(brushBackground,rect);
				if(menuItem!=menuItemAlerts) {
					rect=new Rectangle(e.Bounds.X+15,e.Bounds.Y,e.Bounds.Width,e.Bounds.Height);//Mimic the spacing of other menu items.
				}
				//We use the standard menu font so that the font on this one menu item will match the rest of the menu.
				e.Graphics.DrawString(menuText,SystemInformation.MenuFont,brushFont,rect,stringFormat);
			}
		}

		///<summary>Helper function to determin backgroud color of an AlertItem.</summary>
		private Color AlertBackgroudColorHelper(SeverityType type) {
			switch(type) {
				default:
				case SeverityType.Normal:
					return SystemColors.Control;
				case SeverityType.Low:
					return Color.LightGoldenrodYellow;
				case SeverityType.Medium:
					return Color.DarkOrange;
				case SeverityType.High:
					return Color.OrangeRed;
			}
		}

		///<summary>Helper function to determin text color of an AlertItem.</summary>
		private Color AlertTextColorHelper(SeverityType type) {
			switch(type) {
				default:
					return Color.White;
				case SeverityType.Low:
					return Color.Black;
			}
		}

		private void menuItemAlerts_MeasureItem(object sender,MeasureItemEventArgs e) {
			//Measure the text showing.
			MenuItem menuItem=(MenuItem)sender;
			Size sizeString=TextRenderer.MeasureText(menuItem.Text,SystemInformation.MenuFont);
			e.ItemWidth=sizeString.Width;
			if(menuItem!=menuItemAlerts) {
				e.ItemWidth=sizeString.Width+15;//Due to logic in menuItemAlerts_DrawItem(...).
			}
			e.ItemHeight=sizeString.Height+5;//Pad the bottom
		}

		private void menuItemAlerts_Click(object sender,EventArgs e) {
			MenuItem menuItem=(MenuItem)sender;
			AlertItem alertItem=(AlertItem)menuItem.Tag;
			alertReadsHelper(alertItem);
			if(menuItem.Name==ActionType.MarkAsRead.ToString()) {
				CheckAlerts(false);
				return;
			}
			if(menuItem.Name==ActionType.Delete.ToString()) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will delete the alert for all users. Are you sure you want to delete it?")) {
					return;
				}
				AlertItems.Delete(alertItem.AlertItemNum);
				CheckAlerts(false);
				return;
			}
			if(menuItem.Name==ActionType.OpenForm.ToString()) {
				switch(alertItem.FormToOpen) {
					case FormType.FormPendingPayments:
						FormPendingPayments FormPP=new FormPendingPayments();
						FormPP.Show();//Non-modal so the user can view the patient's account
						FormPP.FormClosed+=this.alertFormClosingHelper;
						break;
					case FormType.FormEServicesWebSchedRecall:
						ShowEServicesSetup(FormEServicesSetup.EService.WebSched);
						break;
					case FormType.FormRadOrderList:
						List<FormRadOrderList> listFormROLs=Application.OpenForms.OfType<FormRadOrderList>().ToList();
						if(listFormROLs.Count > 0) {
							listFormROLs[0].RefreshRadOrdersForUser(Security.CurUser);
							listFormROLs[0].BringToFront();
						}
						else {
							FormRadOrderList FormROL=new FormRadOrderList(Security.CurUser);
							FormROL.Show();
							FormROL.FormClosed+=this.alertFormClosingHelper;
						}
						break;
					case FormType.FormEServicesSignupPortal:
						ShowEServicesSetup(FormEServicesSetup.EService.SignupPortal);
						break;
					case FormType.FormEServicesWebSchedNewPat:
						ShowEServicesSetup(FormEServicesSetup.EService.WebSchedNewPat);
						break;
					case FormType.FormEServicesEConnector:
						ShowEServicesSetup(FormEServicesSetup.EService.ListenerService);
						break;
					case FormType.FormApptEdit:
						Appointment appt=Appointments.GetOneApt(alertItem.FKey);
						Patient pat=Patients.GetPat(appt.PatNum);
						S_Contr_PatientSelected(pat,false);
						FormApptEdit FormAE=new FormApptEdit(appt.AptNum);
						FormAE.ShowDialog();
						break;
					case FormType.FormWebSchedAppts:
						FormWebSchedAppts FormWebSchedAppts=new FormWebSchedAppts(alertItem.Type==AlertType.WebSchedNewPatApptCreated,
							alertItem.Type==AlertType.WebSchedRecallApptCreated,alertItem.Type==AlertType.WebSchedASAPApptCreated);
						FormWebSchedAppts.Show();
						break;
					case FormType.FormPatientEdit:
						pat=Patients.GetPat(alertItem.FKey);
						Family fam=Patients.GetFamily(pat.PatNum);
						S_Contr_PatientSelected(pat,false);
						FormPatientEdit FormPE=new FormPatientEdit(pat,fam);
						FormPE.ShowDialog();
						break;
					case FormType.FormDoseSpotAssignUserId:
						if(!Security.IsAuthorized(Permissions.SecurityAdmin)) {
							break;
				}
						FormDoseSpotAssignUserId FormAU=new FormDoseSpotAssignUserId(alertItem.FKey);
						FormAU.ShowDialog();
						break;
			}
			}
			if(menuItem.Name==ActionType.ShowItemValue.ToString()) {
				MsgBoxCopyPaste msgBCP=new MsgBoxCopyPaste(alertItem.ItemValue);
				msgBCP.Show();
			}
		}

		///<summary>This is used to force the alert logic to run on the server in OpenDentalService.
		///OpenDentalService Alerts logic will re run on signal update interval time.
		///This could be enhanced eventually only invalidate when something from the form changed.</summary>
		private void alertFormClosingHelper(object sender,FormClosedEventArgs e) {
			DataValid.SetInvalid(InvalidType.AlertItems);//THIS IS NOT CACHED. But is used to make server run the alert logic in OpenDentalService.
		}

		///<summary>Refreshes AlertReads for current user and creates a new one if one does not exist for given alertItem.</summary>
		private void alertReadsHelper(AlertItem alertItem) {
			if(_listAlertReads.Exists(x => x.AlertItemNum==alertItem.AlertItemNum)) {//User has already read this alertitem.
				return;
			}
			AlertReads.Insert(new AlertRead(alertItem.AlertItemNum,Security.CurUser.UserNum));
		}
		#endregion Alerts

		#region Standard and Query reports
		private void menuItemReportsHeader_Popup(object sender,EventArgs e) {
			menuItemReportsStandard.MenuItems.Clear();
			menuItemReportsUserQuery.MenuItems.Clear();
			if(Security.CurUser==null) {
				return;
			}
			#region Standard
			List<DisplayReport> listDisplayReports=DisplayReports.GetSubMenuReports();
			if(listDisplayReports.Count>0) {
				List<long> listReportPermissionFkeys=GroupPermissions.GetPermsForReports()
					.Where(x => Security.CurUser.IsInUserGroup(x.UserGroupNum))
					.Select(x => x.FKey)
					.ToList();
				listDisplayReports.RemoveAll(x => !listReportPermissionFkeys.Contains(x.DisplayReportNum));//Remove reports user does not have permission for
				menuItemReportsStandard.MenuItems.Add(Lans.g(this,"Standard Reports"),menuItemReportsStandard_Click);
				menuItemReportsStandard.MenuItems.Add("-");//Horizontal line.
				listDisplayReports.ForEach(x => {
					MenuItem menuItem=new MenuItem(x.Description,StandardReport_ClickEvent);
					menuItem.Tag=x;
					menuItemReportsStandard.MenuItems.Add(menuItem);
				});
			}
			#endregion
			#region UserQueries
			List<UserQuery> listReleasedQuries=UserQueries.GetDeepCopy(true);
			if(listReleasedQuries.Count>0) {
				menuItemReportsUserQuery.MenuItems.Add(Lans.g(this,"User Query"),menuItemReportsUserQuery_Click);
				menuItemReportsUserQuery.MenuItems.Add("-");//Horizontal line.
				listReleasedQuries.ForEach(x => {
					MenuItem menuItem=new MenuItem(x.Description,UserQuery_ClickEvent);
					menuItem.Tag=x;
					menuItemReportsUserQuery.MenuItems.Add(menuItem);
				});
			}
			#endregion
		}

		private void StandardReport_ClickEvent(object sender,EventArgs e) {
			DisplayReport displayReport=(DisplayReport) ((MenuItem) sender).Tag;
			ReportNonModalSelection selection=FormReportsMore.OpenReportHelper(displayReport,doValidatePerm:false);//Permission already validated.
			NonModalReportSelectionHelper(selection);
		}

		private void NonModalReportSelectionHelper(ReportNonModalSelection selection) {
			switch(selection) {
				case ReportNonModalSelection.TreatmentFinder:
					FormRpTreatmentFinder FormT=new FormRpTreatmentFinder();
					FormT.Show();
					break;
				case ReportNonModalSelection.OutstandingIns:
					FormRpOutstandingIns FormOI=new FormRpOutstandingIns();
					FormOI.Show();
					break;
				case ReportNonModalSelection.UnfinalizedInsPay:
					FormRpUnfinalizedInsPay FormU=new FormRpUnfinalizedInsPay();
					FormU.Show();
					break;
				case ReportNonModalSelection.UnsentClaim:
					FormRpClaimNotSent FormCNS=new FormRpClaimNotSent();
					FormCNS.Show();
					break;
				case ReportNonModalSelection.WebSchedAppointments:
					FormWebSchedAppts formWSA=new FormWebSchedAppts(true,true,true);
					formWSA.Show();
					break;
				case ReportNonModalSelection.CustomAging:
					FormRpCustomAging FormCAO=new FormRpCustomAging();
					FormCAO.Show();
					break;
				case ReportNonModalSelection.IncompleteProcNotes:
					FormRpProcNote FormPN=new FormRpProcNote();
					FormPN.Show();
					break;
				case ReportNonModalSelection.None:
				default:
					//Do nothing.
					break;
			}
		}

		private void UserQuery_ClickEvent(object sender,EventArgs e) {
			UserQuery userQuery=(UserQuery) ((MenuItem) sender).Tag;
			ExecuteQueryFavorite(userQuery);
		}

		private void ExecuteQueryFavorite(UserQuery userQuery) {
			_formUserQuery=new FormQuery(null,true);
			_formUserQuery.FormClosed+=new FormClosedEventHandler((object senderF,FormClosedEventArgs eF) => { _formUserQuery=null; });
			_formUserQuery.textQuery.Text=userQuery.QueryText;
			_formUserQuery.textTitle.Text=userQuery.FileName;
			SecurityLogs.MakeLogEntry(Permissions.UserQuery,0,Lan.g(this,"User query form accessed."));
			_formUserQuery.Show();
		}

		#endregion

		#region Help

		//Help
		private void menuItemRemote_Click(object sender,System.EventArgs e) {
			try {
				Process.Start("http://www.opendental.com/contact.html");
			}
			catch(Exception) {
				MessageBox.Show(Lan.g(this,"Could not find")+" http://www.opendental.com/contact.html" + "\r\n"
					+Lan.g(this,"Please set up a default web browser."));
			}
			/*
			if(!MsgBox.Show(this,true,"A remote connection will now be attempted. Do NOT continue unless you are already on the phone with us.  Do you want to continue?"))
			{
				return;
			}
			try{
				Process.Start("remoteclient.exe");//Network streaming remote client or any other similar client
			}
			catch{
				MsgBox.Show(this,"Could not find file.");
			}*/
		}

		private void menuItemHelpWindows_Click(object sender, System.EventArgs e) {
			try{
				Process.Start("Help.chm");
			}
			catch{
				MsgBox.Show(this,"Could not find file.");
			}
		}

		private void menuItemHelpContents_Click(object sender, System.EventArgs e) {
			try{
				Process.Start("http://www.opendental.com/manual/toc.html");
			}
			catch{
				MsgBox.Show(this,"Could not find file.");
			}
		}

		private void menuItemHelpIndex_Click(object sender, System.EventArgs e) {
			try{
				Process.Start(@"http://www.opendental.com/manual/alphabetical.html");
			}
			catch{
				MsgBox.Show(this,"Could not find file.");
			}
		}
		
		private void menuItemWebinar_Click(object sender,EventArgs e) {
			try{
				Process.Start(@"http://opendental.com/manual/webinars.html");
			}
			catch{
				MsgBox.Show(this,"Could not open page.");
			}
		}

		private void menuItemRemoteSupport_Click(object sender,EventArgs e) {
			//Check the installation directory for the GoToAssist corporate exe.
			string fileGTA=CodeBase.ODFileUtils.CombinePaths(Application.StartupPath,"GoToAssist_Corporate_Customer_ver11_9.exe");
			try {
				if(!File.Exists(fileGTA)) {
					throw new ApplicationException();//No message because a different message shows below.
				}
				//GTA exe is available, so load it up
				Process.Start(fileGTA);
			}
			catch {
				MsgBox.Show(this,"Could not find file.  Please use Online Support instead.");
			}
		}

		private void menuItemRequestFeatures_Click(object sender,EventArgs e) {
			FormFeatureRequest FormF=new FormFeatureRequest();
			FormF.Show();
		}

		private void menuItemUpdate_Click(object sender, System.EventArgs e) {
			//If A to Z folders are disabled, this menu option is unavailable, since
			//updates are handled more automatically.
			FormUpdate FormU = new FormUpdate();
			FormU.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Update Version");
		}

		private void menuItemAbout_Click(object sender, System.EventArgs e) {
			FormAbout FormA=new FormAbout();
			FormA.ShowDialog();
		}
		#endregion

		#endregion

		//private void OnPatientCardInserted(object sender, PatientCardInsertedEventArgs e) {
		//  if (InvokeRequired) {
		//    Invoke(new PatientCardInsertedEventHandler(OnPatientCardInserted), new object[] { sender, e });
		//    return;
		//  }
		//  if (MessageBox.Show(this, string.Format(Lan.g(this, "A card belonging to {0} has been inserted. Do you wish to search for this patient now?"), e.Patient.GetNameFL()), "Open Dental", MessageBoxButtons.YesNo) != DialogResult.Yes)
		//  {
		//    return;
		//  }
		//  using (FormPatientSelect formPS = new FormPatientSelect()) {
		//    formPS.PreselectPatient(e.Patient);
		//    if(formPS.ShowDialog() == DialogResult.OK) {
		//      // OnPatientSelected(formPS.SelectedPatNum);
		//      // ModuleSelected(formPS.SelectedPatNum);
		//    }
		//  }
		//}

		///<summary>separate thread</summary>
		//public void Listen() {
		//	IPAddress ipAddress = Dns.GetHostAddresses("localhost")[0];
		//	TcpListenerCommandLine=new TcpListener(ipAddress,2123);
		//	TcpListenerCommandLine.Start();
		//	while(true) {
		//		if(!TcpListenerCommandLine.Pending()) {
		//			//Thread.Sleep(1000);//for 1 second
		//			continue;
		//		}
		//		TcpClient TcpClientRec = TcpListenerCommandLine.AcceptTcpClient();
		//		NetworkStream ns = TcpClientRec.GetStream();
		//		XmlSerializer serializer=new XmlSerializer(typeof(string[]));
		//		string[] args=(string[])serializer.Deserialize(ns);
		//		Invoke(new ProcessCommandLineDelegate(ProcessCommandLine),new object[] { args });
		//		ns.Close();
		//		TcpClientRec.Close();
		//	}
		//}

		#region Startup methods
		///<summary></summary>
		private void ProcessCommandLine(string[] args) {
			//if(!Programs.UsingEcwTight() && args.Length==0){
			if(!Programs.UsingEcwTightOrFullMode() && args.Length==0){//May have to modify to accept from other sw.
				SetModuleSelected();
				return;
			}
			/*string descript="";
			for(int i=0;i<args.Length;i++) {
				if(i>0) {
					descript+="\r\n";
				}
				descript+=args[i];
			}
			MessageBox.Show(descript);*/
			/*
			PatNum (the integer primary key)
			ChartNumber (alphanumeric)
			SSN (exactly nine digits. If required, we can gracefully handle dashes, but that is not yet implemented)
			UserName
			Password*/
			long patNum=0;
			string chartNumber="";
			string ssn="";
			string userName="";
			string passHash="";
			string aptNum="";
			string ecwConfigPath="";
			long userId=0;
			string jSessionId = "";
			string jSessionIdSSO = "";
			string lbSessionId="";
			Dictionary<string,int> dictModules=new Dictionary<string,int>();
			dictModules.Add("appt",0);
			dictModules.Add("family",1);
			dictModules.Add("account",2);
			dictModules.Add("txplan",3);
			dictModules.Add("treatplan",3);
			dictModules.Add("chart",4);
			dictModules.Add("images",5);
			dictModules.Add("manage",6);
			int startingModuleIdx=-1;
			for(int i=0;i<args.Length;i++) {
				if(args[i].StartsWith("PatNum=") && args[i].Length>7) {
					string patNumStr=args[i].Substring(7).Trim('"');
					try {
						patNum=Convert.ToInt64(patNumStr);
					}
					catch { }
				}
				if(args[i].StartsWith("ChartNumber=") && args[i].Length>12) {
					chartNumber=args[i].Substring(12).Trim('"');
				}
				if(args[i].StartsWith("SSN=") && args[i].Length>4) {
					ssn=args[i].Substring(4).Trim('"');
				}
				if(args[i].StartsWith("UserName=") && args[i].Length>9) {
					userName=args[i].Substring(9).Trim('"');
				}
				if(args[i].StartsWith("PassHash=") && args[i].Length>9) {
					passHash=args[i].Substring(9).Trim('"');
				}
				if(args[i].StartsWith("AptNum=") && args[i].Length>7) {
					aptNum=args[i].Substring(7).Trim('"');
				}
				if(args[i].StartsWith("EcwConfigPath=") && args[i].Length>14) {
					ecwConfigPath=args[i].Substring(14).Trim('"');
				}
				if(args[i].StartsWith("UserId=") && args[i].Length>7) {
					string userIdStr=args[i].Substring(7).Trim('"');
					try {
						userId=Convert.ToInt64(userIdStr);
					}
					catch { }
				}
				if(args[i].StartsWith("JSESSIONID=") && args[i].Length > 11) {
					jSessionId=args[i].Substring(11).Trim('"');
				}
				if(args[i].StartsWith("JSESSIONIDSSO=") && args[i].Length > 14) {
					jSessionIdSSO = args[i].Substring(14).Trim('"');
				}
				if(args[i].StartsWith("LBSESSIOINID=") && args[i].Length>12) {
					lbSessionId=args[i].Substring(12).Trim('"');
				}
				if(args[i].ToLower().StartsWith("module=") && args[i].Length>7) {
					string moduleName=args[i].Substring(7).Trim('"').ToLower();
					if(dictModules.ContainsKey(moduleName)) {
						startingModuleIdx=dictModules[moduleName];
					}
				}
				if(args[i].ToLower().StartsWith("show=") && args[i].Length>5) {
					_showForm=args[i].Substring(5).Trim('"').ToLower();//Currently only looks for "Popup", "ApptsForPatient", and "SearchPatient"
				}
			}
			if(ProgramProperties.GetPropVal(Programs.GetProgramNum(ProgramName.eClinicalWorks),"IsLBSessionIdExcluded")=="1" //if check box in Program Links is checked
				&& lbSessionId=="" //if lbSessionId not previously set
				&& args.Length > 0 //there is at least one argument passed in
				&& !args[args.Length-1].StartsWith("LBSESSIONID="))//if there is an argument that is the last argument that is not called "LBSESSIONID", then use that argument, including the "name=" part
			{
				//An example of this is command line includes LBSESSIONID= icookie=ECWAPP3ECFH. The space makes icookie a separate parameter. We want to set lbSessionId="icookie=ECWAPP3ECFH". 
				//We are not guaranteed that the parameter is always going to be named icookie, in fact it will be different on each load balancer depending on the setup of the LB.  
				//Therefore, we cannot look for parameter name, but Aislinn from eCW guaranteed that it would be the last parameter every time during our (Cameron and Aislinn's) conversation on 3/5/2014.
				//jsalmon - This is very much a hack but the customer is very large and needs this change ASAP.  Nathan has suggested that we create a ticket with eCW to complain about this and make them fix it.
				lbSessionId=args[args.Length-1].Trim('"');
			}
			//eCW bridge values-------------------------------------------------------------
			Bridges.ECW.AptNum=PIn.Long(aptNum);
			Bridges.ECW.EcwConfigPath=ecwConfigPath;
			Bridges.ECW.UserId=userId;
			Bridges.ECW.JSessionId=jSessionId;
			Bridges.ECW.JSessionIdSSO=jSessionIdSSO;
			Bridges.ECW.LBSessionId=lbSessionId;
			//Username and password-----------------------------------------------------
			//users are allowed to use ecw tight integration without command line.  They can manually launch Open Dental.
			//if((Programs.UsingEcwTight() && Security.CurUser==null)//We always want to trigger login window for eCW tight, even if no username was passed in.
			if((Programs.UsingEcwTightOrFullMode() && Security.CurUser==null)//We always want to trigger login window for eCW tight, even if no username was passed in.
				|| (userName!=""//if a username was passed in, but not in tight eCW mode
				&& (Security.CurUser==null || Security.CurUser.UserName != userName))//and it's different from the current user
			) {
				//The purpose of this loop is to use the username and password that were passed in to determine which user to log in
				//log out------------------------------------
				LastModule=myOutlookBar.SelectedIndex;
				myOutlookBar.SelectedIndex=-1;
				myOutlookBar.Invalidate();
				UnselectActive();
				allNeutral();
				Userod user=Userods.GetUserByName(userName,true);
				if(user==null) {
					//if(Programs.UsingEcwTight() && userName!="") {
					if(Programs.UsingEcwTightOrFullMode() && userName!="") {
						user=new Userod();
						user.UserName=userName;
						if(passHash=="") {
							user.Password="";
						}
						else {
							user.Password=passHash;
						}
						//This can fail if duplicate username because of capitalization differences.
						Userods.Insert(user,new List<long> { PIn.Long(ProgramProperties.GetPropVal(ProgramName.eClinicalWorks,"DefaultUserGroup")) });
						DataValid.SetInvalid(InvalidType.Security);
					}
					else {//not using eCW in tight integration mode
						//So present logon screen
						ShowLogOn();
						user=Security.CurUser.Copy();
					}
				}
				//Can't use Userods.CheckPassword, because we only have the hashed password.
				//if(passHash!=user.Password || !Programs.UsingEcwTight())//password not accepted or not using eCW
				if(passHash!=user.Password || !Programs.UsingEcwTightOrFullMode())//password not accepted or not using eCW
				{
					//So present logon screen
					ShowLogOn();
				}
				else {//password accepted and using eCW tight.
					//this part usually happens in the logon window
					Security.CurUser = user.Copy();
				}
				myOutlookBar.SelectedIndex=Security.GetModule(LastModule);
				myOutlookBar.Invalidate();
				SetModuleSelected();
				Patient pat=Patients.GetPat(CurPatNum);//pat could be null
				Text=PatientL.GetMainTitle(pat,Clinics.ClinicNum);//handles pat==null by not displaying pat name in title bar
				if(userControlTasks1.Visible) {
					userControlTasks1.InitializeOnStartup();
				}
				if(myOutlookBar.SelectedIndex==-1) {
					MsgBox.Show(this,"You do not have permission to use any modules.");
				}
			}
			if(startingModuleIdx!=-1 && startingModuleIdx==Security.GetModule(startingModuleIdx)) {
				UnselectActive();
				allNeutral();//Sets all controls to false.  Needed to set the new module as selected.
				myOutlookBar.SelectedIndex=startingModuleIdx;
				myOutlookBar.Invalidate();
			}
			SetModuleSelected();
			//patient id----------------------------------------------------------------
			if(patNum!=0) {
				Patient pat=Patients.GetPat(patNum);
				if(pat==null) {
					CurPatNum=0;
					RefreshCurrentModule();
					FillPatientButton(null);
				}
				else {
					CurPatNum=patNum;
					RefreshCurrentModule();
					FillPatientButton(pat);
				}
			}
			else if(chartNumber!="") {
				Patient pat=Patients.GetPatByChartNumber(chartNumber);
				if(pat==null) {
					//todo: decide action
					CurPatNum=0;
					RefreshCurrentModule();
					FillPatientButton(null);
				}
				else {
					CurPatNum=pat.PatNum;
					RefreshCurrentModule();
					FillPatientButton(pat);
				}
			}
			else if(ssn!="") {
				Patient pat=Patients.GetPatBySSN(ssn);
				if(pat==null) {
					//todo: decide action
					CurPatNum=0;
					RefreshCurrentModule();
					FillPatientButton(null);
				}
				else {
					CurPatNum=pat.PatNum;
					RefreshCurrentModule();
					FillPatientButton(pat);
				}
			}
			else{
					FillPatientButton(null);
			}
		}

		///<summary>Fires an OD event for the Splash Screen Progress Bar</summary>
		private void UpdateSplashProgress(string status,int percentage) {
			SplashProgressEvent.Fire(new ODEventArgs("SplashScreenProgress",new ProgressBarHelper(Lan.g(this,status),percentage.ToString()+"%",percentage,100,ProgBarStyle.Continuous,"Update")));
		}
		#endregion Startup methods

		#region HQ only metrics

		///<summary>HQ only. ODThread wraps exception handling for us. Nothing to do here so we will just swallow any exceptions.</summary>
		private void OnThreadHqMetricsException(Exception e) {			
		}

		///<summary>HQ only. Runs every 1.6 seconds. This method runs in a thread so any access to form controls must be invoked.</summary>
		private void ProcessHqMetrics(ODThread odThread) {
			ProcessHqMetricsPhones();
			ProcessHqMetricsEServices();
		}

		///<summary>HQ only. Called from ProcessHqMetrics(). Deals with HQ phone panel. This method runs in a thread so any access to form controls must be invoked.</summary>
		private void ProcessHqMetricsPhones() {
			if(Security.CurUser==null) {
				return;//Don't waste time processing phone metrics when no one is logged in and sitting at the log on screen.
			}
#if DEBUG
			new DataConnection().SetDbT("localhost","customers","root","","","",DatabaseType.MySql,true);
#else
			new DataConnection().SetDbT("server","customers","root","","","",DatabaseType.MySql,true);
#endif
			if(_listMaps.Count>0 
				&& DateTime.Now.Subtract(_hqOfficeDownLastRefreshed).TotalSeconds>PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs)) 
			{
				List<Task> listOfficesDowns=Tasks.GetOfficeDowns();
				if(!IsDisposed) {
					Invoke(new ProcessOfficeDownArgs(ProcessOfficeDowns),new object[] { listOfficesDowns });
				}
				_hqOfficeDownLastRefreshed=DateTime.Now;
			}
			if( //Fill the triage labels at the fastest interval if the HQ map is open. This is only typically for the project PC in the HQ call center.
				_listMaps.Count>0 //Always run if the HQ map is open. 
				||  //For everyone else, Only fill triage labels at given interval. Too taxing on the server to perform every 1.6 seconds.
					DateTime.Now.Subtract(_hqTriageMetricsLastRefreshed).TotalSeconds>PrefC.GetInt(PrefName.ProcessSigsIntervalInSecs)) 
			{
				TriageMetric triageMetrics=Phones.GetTriageMetrics();
				Invoke(new FillTriageLabelsResultsArgs(OnFillTriageLabelsResults),triageMetrics);
				//Reset the interval timer.
				_hqTriageMetricsLastRefreshed=DateTime.Now;
			}
			List<PhoneEmpDefault> listPED=PhoneEmpDefaults.Refresh();
			List<PhoneEmpSubGroup> listSubGroups=PhoneEmpSubGroups.GetAll();
			//Get the extension linked to this machine.
			PhoneComp phoneComp=PhoneComps.GetFirstOrDefault(x => x.ComputerName.ToUpper()==Environment.MachineName.ToUpper());
			int extension=phoneComp?.PhoneExt??0;
			//Get the phoneempdefault row that is currently associated to the corresponding extension.
			PhoneEmpDefault pedCur=listPED.FirstOrDefault(x => x.PhoneExt==extension);
			bool isTriageOperator=pedCur?.IsTriageOperator??false;
			//Now get the Phone object for this extension. Phone table matches PhoneEmpDefault table more or less 1:1. 
			//Phone fields represent current state of the PhoneEmpDefault table and will be modified by the phone tracking server anytime a phone state changes for a given extension 
			//(EG... incoming call, outgoing call, hangup, etc).
			List<Phone> listPhones=Phones.GetPhoneList();
			Phone phone=listPhones.FirstOrDefault(x => x.Extension==extension);
			List<ChatUser> listChatUsers=ChatUsers.GetAll();
			//send the results back to the UI layer for action.
			if(!this.IsDisposed) {
				Invoke(new ProcessHqMetricsResultsArgs(OnProcessHqMetricsResults),new object[] { listPED,listPhones,listSubGroups,listChatUsers,phone,isTriageOperator });
			}
		}

		///<summary>Filtered all the way up from MapAreaRoomControl.</summary>
		private void FormMapHQ_RoomControlClicked(object sender,EventArgs e) {
			MapAreaRoomControl clickedPhone=(MapAreaRoomControl)sender;
			if(clickedPhone==null) {
				return;
			}
			//all we need to do is the Database and let ProcessHQMetrics handle the rest.
			Phones.SetPhoneStatus(ClockStatusEnum.HelpOnTheWay,PIn.Int(clickedPhone.Extension));
			_odThreadHqMetrics.Wakeup();
		}

		private void FormMapHQ_ExtraMapClicked(object sender,EventArgs e) {
			FormMapHQ formMapHQ;
			formMapHQ=new FormMapHQ();
			formMapHQ.RoomControlClicked+=FormMapHQ_RoomControlClicked;
			formMapHQ.ExtraMapClicked+=FormMapHQ_ExtraMapClicked;
			formMapHQ.GoToChanged+=MapAreaRoomControl_GoToChanged;
			formMapHQ.Show();
			formMapHQ.BringToFront();
		}


		///<summary>HQ only. Called from ProcessHqMetrics(). Deals with HQ EServices. This method runs in a thread so any access to form controls must be invoked.</summary>
		private void ProcessHqMetricsEServices() {
			if(DateTime.Now.Subtract(_hqEServiceMetricsLastRefreshed).TotalSeconds<10) {
				return;
			}
			if(_listMaps.Count==0) { //Do not run if the HQ map is not open.
				return;
			}
			_hqEServiceMetricsLastRefreshed=DateTime.Now;
			//Get important metrics from serviceshq db.
			EServiceMetrics metricsToday=EServiceMetrics.GetEServiceMetricsFromSignalHQ();
			if(metricsToday==null) {
				return;
			}
			foreach(FormMapHQ formMapHQ in _listMaps) {
				formMapHQ.Invoke(new MethodInvoker(delegate { formMapHQ.SetEServiceMetrics(metricsToday); }));
			}
		}

		/// <summary>HQ Only. OnProcessHqMetricsResults must be invoked from a worker thread. These are the arguments necessary.</summary>
		protected delegate void ProcessHqMetricsResultsArgs(List<PhoneEmpDefault> phoneEmpDefaultList,List<Phone> phoneList,
			List<PhoneEmpSubGroup> listSubGroups,List<ChatUser> listChatUsers,Phone phone,bool isTriageOperator);
		/// <summary>HQ Only. ProcessOfficeDowns must be invoked from a worker thread. These are the arguments necessary.</summary>
		protected delegate void ProcessOfficeDownArgs(List<Task> listOfficeDowns);

		///<summary>HQ Only. Digest results of ProcessHqMetrics and update form controls accordingly.
		///phoneList is the list of all phone rows just pulled from the database.  phone is the one that we should display here, and it can be null.</summary>
		private void OnProcessHqMetricsResults(List<PhoneEmpDefault> phoneEmpDefaultList,List<Phone> phoneList,List<PhoneEmpSubGroup> listSubGroups,
			List<ChatUser> listChatUsers,Phone phone,bool isTriageOperator) {
			try {
				//Send the phoneList to the 2 places where it's needed.
				//1) Send to the small display in the main OD form (quick-glance).
				phoneSmall.SetPhoneList(phoneEmpDefaultList,phoneList);
				if(formPhoneTiles!=null && !formPhoneTiles.IsDisposed) { //2) Send to the big phones panel if it is open.
					formPhoneTiles.SetPhoneList(phoneEmpDefaultList,phoneList,listChatUsers);
				}
				foreach(FormMapHQ formMapHQ in _listMaps) {
					formMapHQ.SetPhoneList(phoneEmpDefaultList,phoneList,listSubGroups, listChatUsers);
				}
				//Now set the small display's current phone extension info.
				long employeeNum=0;
				ChatUser chatUserCur;
				if(phone==null) {
					phoneSmall.Extension=0;
					if(Security.CurUser!=null) {
						employeeNum=Security.CurUser.EmployeeNum;
					}
					chatUserCur=null;
				}
				else {
					phoneSmall.Extension=phone.Extension;
					employeeNum=phone.EmployeeNum;
					chatUserCur=listChatUsers.Where(x => x.Extension == phone.Extension).FirstOrDefault();
				}
				phoneSmall.SetPhone(phone,PhoneEmpDefaults.GetEmpDefaultFromList(employeeNum,phoneEmpDefaultList),chatUserCur,isTriageOperator);
			}
			catch(Exception e) {
				e.DoNothing();
				//HQ users are complaining of unhandled exception when they close OD.
				//Suspect it could be caused here if the thread tries to access a control that has been disposed.
			}
		}

		///<summary>HQ Only. Send the office downs to any Call Center Maps that are open.</summary>
		private void ProcessOfficeDowns(List<Task> listOfficeDowns) {
			try {
				foreach(FormMapHQ formMapHQ in _listMaps) {
					formMapHQ.SetOfficesDownList(listOfficeDowns);
				}
			}
			catch {
				//HQ users are complaining of unhandled exception when they close OD.
				//Suspect it could be caused here if the thread tries to access a control that has been disposed.
			}
		}

		public static void AddMapToList(FormMapHQ map) {
			_listMaps.Add(map);
		}
		public static void RemoveMapFromList(FormMapHQ map) {
			_listMaps.Remove(map);
		}

		/// <summary>HQ Only. OnFillTriageLabelsResults must be invoked from a worker thread. These are the arguments necessary.</summary>
		private delegate void FillTriageLabelsResultsArgs(TriageMetric triageMetric);

		/// <summary>HQ Only. Digest results of Phones.GetTriageMetrics() in ProcessHqMetrics(). Fills the triage labels and update form controls accordingly.</summary>
		private void OnFillTriageLabelsResults(TriageMetric triageMetric) {
			int countBlueTasks=triageMetric.CountBlueTasks;
			int countWhiteTasks=triageMetric.CountWhiteTasks;
			int countRedTasks=triageMetric.CountRedTasks;
			DateTime timeOfOldestBlueTaskNote=triageMetric.DateTimeOldestTriageTaskOrTaskNote;
			DateTime timeOfOldestRedTaskNote=triageMetric.DateTimeOldestUrgentTaskOrTaskNote;
			TimeSpan triageBehind=new TimeSpan(0);
			if(timeOfOldestBlueTaskNote.Year>1880 && timeOfOldestRedTaskNote.Year>1880) {
				if(timeOfOldestBlueTaskNote<timeOfOldestRedTaskNote) {
					triageBehind=DateTime.Now-timeOfOldestBlueTaskNote;
				}
				else {//triageBehind based off of older RedTask
					triageBehind=DateTime.Now-timeOfOldestRedTaskNote;
				}
			}
			else if(timeOfOldestBlueTaskNote.Year>1880) {
				triageBehind=DateTime.Now-timeOfOldestBlueTaskNote;
			}
			else if(timeOfOldestRedTaskNote.Year>1880) {
				triageBehind=DateTime.Now-timeOfOldestRedTaskNote;
			}
			string countStr="0";
			if(countBlueTasks>0 || countRedTasks>0) {//Triage show red so users notice more.
				countStr=(countBlueTasks+countRedTasks).ToString();
				labelTriage.ForeColor=Color.Firebrick;
			}
			else {
				if(countWhiteTasks>0) {
					countStr="("+countWhiteTasks.ToString()+")";
				}
				labelTriage.ForeColor=Color.Black;
			}
			labelTriage.Text="T:"+countStr;
			labelWaitTime.Text=((int)triageBehind.TotalMinutes).ToString()+"m";
			foreach(FormMapHQ formMapHQ in _listMaps) {
				formMapHQ.SetTriageNormal(countWhiteTasks,countBlueTasks,triageBehind,countRedTasks);
				TimeSpan urgentTriageBehind=new TimeSpan(0);
				if(timeOfOldestRedTaskNote.Year>1880) {
					urgentTriageBehind=DateTime.Now-timeOfOldestRedTaskNote;
				}
				formMapHQ.SetTriageUrgent(countRedTasks,urgentTriageBehind);
			}
		}
		#endregion

		private void TryNonPatientPopup() {
			if(CurPatNum!=0 && _previousPatNum!=CurPatNum) {
				_datePopupDelay=DateTime.Now;
				_previousPatNum=CurPatNum;
			}
			if(!PrefC.GetBool(PrefName.ChartNonPatientWarn)) {
				return;
			}
			Patient patCur=Patients.GetPat(CurPatNum);
			if(patCur!=null 
						&& patCur.PatStatus.ToString()=="NonPatient"
						&& _datePopupDelay<=DateTime.Now) {
				MsgBox.Show(this,"A patient with the status NonPatient is currently selected.");
				_datePopupDelay=DateTime.Now.AddMinutes(5);
			}
		}

		/// <summary>This is set to 30 seconds</summary>
		private void timerWebHostSynch_Tick(object sender,EventArgs e) {
			if(_isStartingUp) {//If the program is starting up it may be updating and we do not want to synch yet.
				return;
			}
			try {
				string interval=PrefC.GetStringSilent(PrefName.MobileSyncIntervalMinutes);
				if(interval=="" || interval=="0") {//not a paid customer or chooses not to synch
					return;
				}
				if(System.Environment.MachineName.ToUpper()!=PrefC.GetStringSilent(PrefName.MobileSyncWorkstationName).ToUpper()) {
					//Since GetStringSilent returns "" before OD is connected to db, this gracefully loops out
					return;
				}
				if(PrefC.GetDate(PrefName.MobileExcludeApptsBeforeDate).Year<1880) {
					//full synch never run
					return;
				}
				FormEServicesSetup.SynchFromMain(false);
			}
			catch { }//If MySQL service has been lost will not automatically UE
		}

		private void timerReplicationMonitor_Tick(object sender,EventArgs e) {
			try {
				//this timer doesn't get turned on until after user successfully logs in.
				if(ReplicationServers.GetCount()==0) {//Listt will be automatically refreshed if null.
					return;//user must not be using any replication
				}
				bool isSlaveMonitor=ReplicationServers.GetExists(x => x.SlaveMonitor.ToString()==Dns.GetHostName());
				if(!isSlaveMonitor) {
					return;
				}
				DataTable table=ReplicationServers.GetSlaveStatus();
				if(table.Rows.Count==0) {
					return;
				}
				string replicatedDbs=table.Rows[0]["Replicate_Do_Db"].ToString().ToLower();
				string dbName=DataConnection.GetDatabaseName().ToLower();
				//If multiple databases are being replicated, Replicate_Do_Db will contain the databases separated by a comma. Keep in mind that a database
				//name can contain a comma.
				bool isDbReplicated=(!dbName.Contains(',') && replicatedDbs.Split(',').Contains(dbName))
					|| (dbName.Contains(',') && replicatedDbs.Contains(dbName));
				if(!isDbReplicated) {
					return;//if the database we're connected to is not even involved in replication
				}
				string statusSqlRunning=table.Rows[0]["Slave_SQL_Running"].ToString().ToUpper();
				string statusIoRunning=table.Rows[0]["Slave_IO_Running"].ToString().ToUpper();
				if(statusSqlRunning=="YES" && statusIoRunning=="YES") {
					_isReplicationSlaveStopped=false;
					return;
				}
				if(table.Rows[0]["Last_Errno"].ToString()=="0" && table.Rows[0]["Last_Error"].ToString()=="") {
					//The slave SQL is not running, but there was not an error.
					//This happens when the slave is manually stopped with an SQL statement, but stopping the slave does not hurt anything, so we should not prevent the user from using OD.
					if(!_isReplicationSlaveStopped) {
						_isReplicationSlaveStopped=true;
						MessageBox.Show(Lan.g(this,"Warning: Replication data receive is off at server ")+ReplicationServers.GetForLocalComputer().Descript+".\r\n"
							+Lan.g(this,"The server will not receive updates until the slave is started again.")+"\r\n"
							+Lan.g(this,"Contact your IT admin to run the SQL command START SLAVE.")
							);
					}
					return;
				}
				//Shut down all copies of OD and set ReplicationFailureAtServer_id to this server_id
				//No workstations will be able to connect to this single server while this flag is set.
				Prefs.UpdateLong(PrefName.ReplicationFailureAtServer_id,ReplicationServers.Server_id);
				//shut down all workstations on all servers
				Signalods.SignalLastRefreshed=MiscData.GetNowDateTime().AddSeconds(5);
				Signalod sig=new Signalod();
				sig.IType=InvalidType.ShutDownNow;
				Signalods.Insert(sig);
				Computers.ClearAllHeartBeats(Environment.MachineName);//always assume success
				timerReplicationMonitor.Enabled=false;
				MsgBox.Show(this,"This database is temporarily unavailable.  Please connect instead to your alternate database at the other location.");
				Application.Exit();
			}
			catch { }
		}
		#region LogOn
		///<summary>Logs on a user using the passed in credentials or Active Directory or the good old-fashioned log on window.</summary>
		private void LogOnOpenDentalUser(string odUser,string odPassword) {
			if(Security.CurUser==null) {//It could already be set if using web service because login from ChooseDatabase window.
				if(Programs.UsingEcwTightOrFullMode() && odUser!="") {//only leave it null if a user was passed in on the commandline.  If starting OD manually, it will jump into the else.
					//leave user as null
				}
				else {
					if(odUser!="" && odPassword!="") {//if a username and password was passed in
						try {
							bool isEcwTightOrFullMode=Programs.UsingEcwTightOrFullMode();
							Security.CurUser=Userods.CheckUserAndPassword(odUser,odPassword,isEcwTightOrFullMode);
							if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
								string pw=odPassword;
								if(isEcwTightOrFullMode) {//ecw requires hash, but non-ecw requires actual password
									pw=Userods.HashPassword(pw,true);
								}
								Security.PasswordTyped=pw;
							}
						}
						catch(Exception ex) {
							MessageBox.Show(ex.Message);
							Application.Exit();
							return;
						}
					}
					if(Security.CurUser==null) {
						Userod adminUser=Userods.GetAdminUser();
						if(adminUser==null) {
							MsgBox.Show(this,"There are no users with the SecurityAdmin permission.  Call support.");
							Application.Exit();
							return;
						}
						if(adminUser.Password=="") {
							Security.CurUser=adminUser.Copy();
							CheckForPasswordReset();
							SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff,0,"User: "+Security.CurUser.UserName+" has logged on.");
						}
						else if(PrefC.GetBool(PrefName.DomainLoginEnabled) && !string.IsNullOrWhiteSpace(PrefC.GetString(PrefName.DomainLoginPath))) {
							string loginPath=PrefC.GetString(PrefName.DomainLoginPath);
							try {
								DirectoryEntry loginEntry=new DirectoryEntry(loginPath);
								string distinguishedName=loginEntry.Properties["distinguishedName"].Value.ToString();
								//All LDAP servers must expose a special entry, called the root DSE. This gets the current user's domain path.
								DirectoryEntry rootDSE=new DirectoryEntry("LDAP://RootDSE");
								string defaultNamingContext=rootDSE.Properties["defaultNamingContext"].Value.ToString();
								if(!distinguishedName.ToLower().Contains(defaultNamingContext.ToLower())) {
									//If the domain of the current user doesn't match the provided LDAP Path, log on normally
									ShowLogOn();
									return;
								}
								List<Userod> users=Userods.GetUsersByDomainUserName(Environment.UserName);
								if(users.Count==0) { //Log on normally if no user linked the current domain user
									ShowLogOn();
								}
								else if(users.Count>1) { //Select a user if multiple users linked to the current domain user
									InputBox box=new InputBox(Lan.g(this,"Select an Open Dental user to log in with:")+" ",users.Select(x=>x.UserName).ToList());
									box.ShowDialog();
									if(box.DialogResult==DialogResult.OK) {
										Security.CurUser=users[box.SelectedIndex];
										CheckForPasswordReset();
										SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff,0,"User: "+Security.CurUser.UserName+" has logged on automatically via ActiveDirectory.");
									}
									else {
										ShowLogOn();
									}
								}
								else { //log on automatically if only one user is linked to current domain user
									Security.CurUser=users.First();
									CheckForPasswordReset();
									SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff,0,"User: "+Security.CurUser.UserName+" has logged on automatically via ActiveDirectory.");
								}
							}
							catch(Exception ex) {
								ex.DoNothing();
								ShowLogOn();
								return;
							}
						}
						else {
							ShowLogOn();
						}
					}
				}
			}
			Security.IsUserLoggedIn=true;//User is guaranteed to be logged in at this point.
		}

		///<summary>Show the log on window.</summary>
		private void ShowLogOn() {
			FormLogOn_=new FormLogOn();
			FormLogOn_.ShowDialog(this);
			if(FormLogOn_.DialogResult==DialogResult.Cancel) {
				Cursor=Cursors.Default;
				Application.Exit();
			}
			CheckForPasswordReset();
		}

		///<summary>Checks to see if the currently logged-in user needs to reset their password.</summary>
		private void CheckForPasswordReset() {
			if(Security.CurUser.IsPasswordResetRequired){
				FormUserPassword FormUP=new FormUserPassword(false,Security.CurUser.UserName,true);
				FormUP.ShowDialog(); 
				bool isPasswordStrong=FormUP.PasswordIsStrong;
				try {
					Security.CurUser.IsPasswordResetRequired=false;
					Userods.Update(Security.CurUser);
					Userods.UpdatePassword(Security.CurUser,FormUP.HashedResult,isPasswordStrong);
				}
				catch(Exception ex) {
					MessageBox.Show(ex.Message);
				}
				Security.CurUser.Password=FormUP.HashedResult;
				Security.CurUser.PasswordIsStrong=FormUP.PasswordIsStrong;
				Security.PasswordTyped=FormUP.PasswordTyped;
				DataValid.SetInvalid(InvalidType.Security);
			}
		}
		#endregion LogOn
		#region Logoff
		///<summary>This is set to 15 seconds.  This interval must be longer than the interval of the timer in FormLogoffWarning (10s), or it will go into a loop.</summary>
		private void timerLogoff_Tick(object sender,EventArgs e) {
			if(PrefC.GetInt(PrefName.SecurityLogOffAfterMinutes)==0) {
				return;
			}
			for(int f=Application.OpenForms.Count-1;f>=0;f--) {//This checks if any forms are open that make us not want to automatically log off. Currently only FormTerminal is checked for.
				Form openForm=Application.OpenForms[f];
				if(openForm.Name=="FormTerminal") {
					return;
				}
				//If anything is in progress we should halt the autologoff. After the window finishes, this will get hit after a maximum of 15 seconds and perform the auto-logoff.
				if(openForm.Name=="FormProgress") {
					return;
				}
			}
			//Warning.  When debugging this, the ActiveForm will be impossible to determine by setting breakpoints.
			//string activeFormText=Form.ActiveForm.Text;
			//If a breakpoint is set below here, ActiveForm will erroneously show as null.
			if(Form.ActiveForm==null) {//some other program has focus
				FormRecentlyOpenForLogoff=null;
				//Do not alter IsFormLogOnLastActive because it could still be active in background.
			}
			else if(Form.ActiveForm==this) {//main form active
				FormRecentlyOpenForLogoff=null;
				//User must have logged back in so IsFormLogOnLastActive should be false.
				IsFormLogOnLastActive=false;
			}
			else {//Some Open Dental dialog is active.
				if(Form.ActiveForm==FormRecentlyOpenForLogoff) {
					//The same form is active as last time, so don't add events again.
					//The active form will now be constantly resetting the dateTimeLastActivity.
				}
				else {//this is the first time this form has been encountered, so attach events and don't do anything else
					FormRecentlyOpenForLogoff=Form.ActiveForm;
					Security.DateTimeLastActivity=DateTime.Now;
					//Flag FormLogOn as the active form so that OD doesn't continue trying to log the user off when using the web service.
					if(Form.ActiveForm.GetType()==typeof(FormLogOn)) {
						IsFormLogOnLastActive=true;
					}
					else {
						IsFormLogOnLastActive=false;
					}
					return;
				}
			}
			DateTime dtDeadline=Security.DateTimeLastActivity+TimeSpan.FromMinutes((double)PrefC.GetInt(PrefName.SecurityLogOffAfterMinutes));
			//Debug.WriteLine("Now:"+DateTime.Now.ToLongTimeString()+", Deadline:"+dtDeadline.ToLongTimeString());
			if(DateTime.Now<dtDeadline) {
				return;
			}
			if(Security.CurUser==null) {//nobody logged on
				return;
			}
			//The above check works unless using web service.  With web service, CurUser is not set to null when FormLogOn is shown.
			if(IsFormLogOnLastActive) {//Don't try to log off a user that is already logged off.
				return;
			}
			FormLogoffWarning formW=new FormLogoffWarning();
			formW.ShowDialog();
			//User could be working outside of OD and the Log On window will never become "active" so we set it here for a fail safe.
			IsFormLogOnLastActive=true;
			if(formW.DialogResult!=DialogResult.OK) {
				Security.DateTimeLastActivity=DateTime.Now;
				return;//user hit cancel, so don't log off
			}
			try {
				LogOffNow(true);
			}
			catch { }
		}

		///<summary>Closes all open forms except FormOpenDental.  Set isForceClose to true if you want to ignore all potential closing events.  E.g. FormWikiEdit will ask users on closing if they are sure they want to discard unsaved work.  Returns false if there is an open form that requests attention, thus needs to stop the closing of the forms.</summary>
		private bool CloseOpenForms(bool isForceClose) {
			for(int f=Application.OpenForms.Count-1;f>=0;f--) {//Loop backwards so we don't get an array out of bounds error
				if(Application.OpenForms[f]==this) {// main form
					continue;
				}
				//If force closing, we HAVE to forcefully close everything related to Open Dental, regardless of plugins.  Otherwise, give plugins a chance to stop the log off event.
				if(!isForceClose) {
					//This hook was moved into this method so that the form closing loop could be shared.
					//It is correctly named and was not updated to say "FormOpenDental.CloseOpenForms" on purpose for backwards compatibility.
					if(Plugins.HookMethod(this,"FormOpenDental.LogOffNow_loopingforms",Application.OpenForms[f])) {
						continue;//if some criteria are met in the hook, don't close a certain form
					}
				}
				Form openForm=Application.OpenForms[f];//Copy so we have a reference to it after we close it.
				openForm.Hide();
				//Currently there is no way to tell if LogOffNow got called from a user initiating the log off, or if this is the auto log off feature.
				//Therefore, when the auto log off feature is enabled, we will forcefully close all forms because some forms might have pop ups within FormClosing prevent the form from closing.
				//That introduces the possibility of sensitive information staying visible in the background while the program waits for user input.
				if(openForm.Name=="FormWikiEdit") {
					if(!isForceClose) {
						if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"You are currently editing a wiki page and it will be saved as a draft.  Continue?")) {
							openForm.Show();//Show that form again
							return false;//This form needs to stay open and stop all other forms from being closed.
						}
					}
					WikiSaveEvent.Fire(new ODEventArgs("ForceSaveWiki"));
				}
				if(openForm.Name=="FormCommItem") {
					CommItemSaveEvent.Fire(new ODEventArgs("CommItemSave"));
				}
				if(openForm.Name=="FormEmailMessageEdit") {
					EmailSaveEvent.Fire(new ODEventArgs("EmailSave"));
				}
				if(isForceClose) {
					openForm.Dispose();//Strictly disposing of a form will not perform the closing events.
				}
				else {
					//Gracefully close each window.  If a window requesting attention causes the form to stay open.  Stop the log off event because the user chose to.
					openForm.Close();//Attempt to close the form
					if(openForm.IsDisposed==false) {//If the form was not closed.  
						//E.g. The wiki edit window will ask users if they want to lose their work or continue working.  This will get hit if they chose to continue working.
						openForm.Show();//Show that form again
						return false;//This form needs to stay open and stop all other forms from being closed.
					}
				}
			}
			return true;//All open forms have been closed at this point.
		}

		private void LogOffNow() {
			bool isForceClose=PrefC.GetLong(PrefName.SecurityLogOffAfterMinutes)>0;
			LogOffNow(isForceClose);
		}

		private void LogOffNow(bool isForced) {
			if(!CloseOpenForms(isForced)) {
				return;//A form is still open.  Do not continue to log the user off.
			}
			LastModule=myOutlookBar.SelectedIndex;
			myOutlookBar.SelectedIndex=-1;
			myOutlookBar.Invalidate();
			UnselectActive();
			allNeutral();
			if(userControlTasks1.Visible) {
				userControlTasks1.ClearLogOff();
			}
			if(isForced) {
				SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff,0,"User: "+Security.CurUser.UserName+" has auto logged off.");
			}
			else {
				SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff,0,"User: "+Security.CurUser.UserName+" has logged off.");
			}
			Clinics.LogOff();
			Userod oldUser=Security.CurUser;
			Security.CurUser=null;
			_listReminderTasks=null;
			_listNormalTaskNums=null;
			ContrAppt2.RefreshReminders(new List<Task>());
			RefreshTasksNotification();
			Security.IsUserLoggedIn=false;
			Text=PatientL.GetMainTitle(null,0);
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Security.CurUser=oldUser;//so that the queries in FormLogOn() will work for the web service, since the web service requires a valid user to run queries.
			}
			StopEServiceMonitoring();
			ShowLogOn();
			//If a different user logs on and they have clinics enabled, then clear the patient drop down history
			//since the current user may not have permission to access patients from the same clinic(s) as the old user
			if(oldUser.UserNum!=Security.CurUser.UserNum && !PrefC.GetBool(PrefName.EasyNoClinics)) {
				CurPatNum=0;
				PatientL.RemoveAllFromMenu(menuPatient);
			}
			myOutlookBar.SelectedIndex=Security.GetModule(LastModule);
			myOutlookBar.Invalidate();
			if(PrefC.HasClinicsEnabled) {
				Clinics.LoadClinicNumForUser();
				RefreshMenuClinics();
			}
			SetModuleSelected();
			Patient pat=Patients.GetPat(CurPatNum);//pat could be null
			Text=PatientL.GetMainTitle(pat,Clinics.ClinicNum);//handles pat==null by not displaying pat name in title bar
			FillPatientButton(pat);
			if(userControlTasks1.Visible) {
				userControlTasks1.InitializeOnStartup();
			}
			StartEServiceMonitoring();
			//User logged back in so log on form is no longer the active window.
			IsFormLogOnLastActive=false;
			CheckAlerts();
			Security.DateTimeLastActivity=DateTime.Now;
			if(myOutlookBar.SelectedIndex==-1) {
				MsgBox.Show(this,"You do not have permission to use any modules.");
			}
		}
		#endregion Logoff

		private void SystemEvents_SessionSwitch(object sender,SessionSwitchEventArgs e) {
			if(e.Reason!=SessionSwitchReason.SessionLock) {
				return;
			}
			//CurUser will be null if Open Dental is already in a 'logged off' state.
			//Also catches the case where Open Dental has NEVER connected to a database yet and checking PrefC would throw an exception (no db conn).
			if(Security.CurUser==null) {
				return;
			}
			if(!PrefC.GetBool(PrefName.SecurityLogOffWithWindows)) {
				return;
			}
			LogOffNow(true);
		}

		private void FormOpenDental_Deactivate(object sender,EventArgs e) {
			//There is a chance that the user has gone to a non-modal form (e.g. task) and can change the patient from that form.
			//We need to save the Treatment Note in the chart module because the "on leave" event might not get fired for the text box.
			if(ContrChart2.TreatmentNoteChanged) {
				ContrChart2.UpdateTreatmentNote();
			}
			if(ContrAccount2.UrgFinNoteChanged) {
				ContrAccount2.UpdateUrgFinNote();
			}
			if(ContrAccount2.FinNoteChanged) {
				ContrAccount2.UpdateFinNote();
			}
			if(ContrTreat2.HasNoteChanged) {
				ContrTreat2.UpdateTPNoteIfNeeded();
			}
		}

		private void FormOpenDental_FormClosing(object sender,FormClosingEventArgs e) {
			//ExitCode will only be set if trying to silently update.  
			//If we start using ExitCode for anything other than silently updating, this can be moved towards the bottom of this closing.
			//If moved to the bottom, all of the clean up code that this closing event does needs to be considered in regards to updating silently from a CEMT computer.
			if(ExitCode!=0) {
				Environment.Exit(ExitCode);
			}
			//CloseOpenForms should have already been called with isForceClose=true if we are force closing Open Dental
			//In that scenario, calling CloseOpenForms with isForceClose=false should not leave the program open.
			//However, if Open Dental is closing from any other means, we want to give all forms the opportunity to stop closing.
			//Example, if you have FormWikiEdit open, it will attempt to save it as a draft unless the user wants to back out.
			if(!CloseOpenForms(false)) {
				e.Cancel=true;
				return;
			}
			try {
				Programs.ScrubExportedPatientData();//Required for EHR module d.7.
			}
			catch {
				//Can happen if cancel is clicked in Choose Database window.
			}
			try {
				Computers.ClearHeartBeat(Environment.MachineName);
			}
			catch { }
			FormUAppoint.AbortThread();
			//ICat.AbortThread
			////earlier, this wasn't working.  But I haven't tested it since moving it from Closing to FormClosing.
			//if(ThreadCommandLine!=null) {
			//	ThreadCommandLine.Abort();
			//}
			if(ThreadVM!=null) {
				ThreadVM.Abort();
				ThreadVM.Join();
				ThreadVM=null;
			}
			if(_isTimeSynchThreadRunning) {
				_isTimeSynchThreadRunning=false;
				_timeSynchSleep.Set();
				_threadTimeSynch.Abort();
				_threadTimeSynch.Join();
				_threadTimeSynch=null;
			}
			ODThread.QuitSyncAllOdThreads();
			if(Security.CurUser!=null) {
				SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff,0,"User: "+Security.CurUser.UserName+" has logged off.");
				Clinics.LogOff();
			}
			//if(PrefC.GetBool(PrefName.DistributorKey)) {//for OD HQ
			//  for(int f=Application.OpenForms.Count-1;f>=0;f--) {
			//    if(Application.OpenForms[f]==this) {// main form
			//      continue;
			//    }
			//    Application.OpenForms[f].Close();
			//  }
			//}
			string tempPath="";
			string[] arrayFileNames;
			List <string> listDirectories;
			try {
				tempPath=PrefC.GetTempFolderPath();
				arrayFileNames=Directory.GetFiles(tempPath,"*.*",SearchOption.AllDirectories);//All files in the current directory plus all files in all subdirectories.
				listDirectories=new List<string>(Directory.GetDirectories(tempPath,"*",SearchOption.AllDirectories));//All subdirectories.
			}
			catch {
				//We will only reach here if we error out of getting the temp folder path
				//If we can't get the path, then none of the stuff below matters
				Plugins.HookAddCode(null,"FormOpenDental.FormClosing_end");
				return;
			}
			for(int i=0;i<arrayFileNames.Length;i++) {
				try {
					//All files related to updates need to stay.  They do not contain PHI information and will not harm anything if left around.
					if(arrayFileNames[i].Contains("UpdateFileCopier.exe")) {
						continue;//Skip any files related to updates.
					}
					//When an update is in progress, the binaries will be stored in a subfolder called UpdateFiles within the temp directory.
					if(arrayFileNames[i].Contains("UpdateFiles")) {
						continue;//Skip any files related to updates.
					}
					//The UpdateFileCopier will create temporary backups of source and destination setup files so that it can revert if copying fails.
					if(arrayFileNames[i].Contains("updatefilecopier")) {
						continue;//Skip any files related to updates.
					}
					File.Delete(arrayFileNames[i]);
				}
				catch {
					//Do nothing because the file could have been in use or there were not sufficient permissions.
					//This file will most likely get deleted next time a temp file is created.
				}
			}
			listDirectories.Sort();//We need to sort so that we know for certain which directories are parent directories of other directories.
			for(int i=listDirectories.Count-1;i>=0;i--) {//Easier than recursion.  Since the list is ordered ascending, then going backwards means we delete subdirectories before their parent directories.
				try {
					//When an update is in progress, the binaries will be stored in a subfolder called UpdateFiles within the temp directory.
					if(listDirectories[i].Contains("UpdateFiles")) {
						continue;//Skip any files related to updates.
					}
					//The UpdateFileCopier will create temporary backups of source and destination setup files so that it can revert if copying fails.
					if(listDirectories[i].Contains("updatefilecopier")) {
						continue;//Skip any files related to updates.
					}
					Directory.Delete(listDirectories[i]);
				}
				catch {
					//Do nothing because the folder could have been in use or there were not sufficient permissions.
					//This folder will most likely get deleted next time Open Dental closes.
				}
			}
			Plugins.HookAddCode(null,"FormOpenDental.FormClosing_end");
		}

		private void FormOpenDental_FormClosed(object sender,FormClosedEventArgs e) {
			//Cleanup all resources related to the program which have their Dispose methods properly defined.
			//This helps ensure that the chart module and its tooth chart wrapper are properly disposed of in particular.
			//This step is necessary so that graphics memory does not fill up.
			Dispose();
			//"=====================================================
			//https://msdn.microsoft.com/en-us/library/system.environment.exit%28v=vs.110%29.aspx
			//Environment.Exit Method:
			//Terminates this process and gives the underlying operating system the specified exit code.
			//For the exitCode parameter, use a non-zero number to indicate an error. In your application, you can define your own error codes in an
			//enumeration, and return the appropriate error code based on the scenario. For example, return a value of 1 to indicate that the required file
			//is not present and a value of 2 to indicate that the file is in the wrong format. For a list of exit codes used by the Windows operating
			//system, see System Error Codes in the Windows documentation.
			//Calling the Exit method differs from using your programming language's return statement in the following ways:
			//*Exit always terminates an application. Using the return statement may terminate an application only if it is used in the application entry
			//	point, such as in the Main method.
			//*Exit terminates an application immediately, even if other threads are running. If the return statement is called in the application entry
			//	point, it causes an application to terminate only after all foreground threads have terminated.
			//*Exit requires the caller to have permission to call unmanaged code. The return statement does not.
			//*If Exit is called from a try or finally block, the code in any catch block does not execute. If the return statement is used, the code in the
			//catch block does execute.
			//====================================================="
			//Call Environment.Exit() to kill all threads which we forgot to close.  Also sends exit code 0 to the command line to indicate success.
			//If a thread needs to be gracefully quit, then it is up to the designing engineer to Join() to that thread before we get to this point.
			//We considered trying to get a list of active threads and logging debug information for those threads, but there is no way
			//to get the list of managed threads from the system.  It is our responsibility to keep track of our own managed threads.  There is a way
			//to get the list of unmanaged system threads for our application using Process.GetCurrentProcess().Threads, but that does not help us enough.
			//See http://stackoverflow.com/questions/466799/how-can-i-enumerate-all-managed-threads-in-c.  To keep track of a managed thread, use ODThread.
			//Environment.Exit requires permission for unmanaged code, which we have explicitly specified in the solution already.
			Environment.Exit(0);//Guaranteed to kill any threads which are still running.
		}

		///<summary>Updates the title text when the update time preference is in the future.  
		///Always rounds down so as not to give users the impression that there is more time than there really is until the update.</summary>
		private void timerUpdateTime_Tick(object sender,EventArgs e) {
			this.Text=PatientL.GetMainTitleSamePat();
		}
	}

	public class PopupEvent:IComparable{
		public long PopupNum;
		///<summary>Disable this popup until this time.</summary>
		public DateTime DisableUntil;
		///<summary>The last time that this popup popped up.</summary>
		public DateTime LastViewed;

		public int CompareTo(object obj) {
			PopupEvent pop=(PopupEvent)obj;
			return DisableUntil.CompareTo(pop.DisableUntil);
		}

		public override string ToString() {
			return PopupNum.ToString()+", "+DisableUntil.ToString();
		}
	}

	///<summary>This is a global class because it must run at the application level in order to catch application level system input events.
	///WM_KEYDOWN (0x0100) message details: https://msdn.microsoft.com/en-us/library/windows/desktop/ms646280(v=vs.85).aspx.
	///WM_MOUSEMOVE (0x0200) message details: https://msdn.microsoft.com/en-us/library/windows/desktop/ms645616(v=vs.85).aspx. ///</summary>
	public class ODGlobalUserActiveHandler:IMessageFilter {
		///<summary>Compare position of mouse at the time of the message to the previously stored mouse position to correctly identify a mouse movement.
		///In testing, a mouse will sometimes fire a series of multiple MouseMove events with the same position, possibly due to wireless mouse chatter.
		///Comparing to previos position allows us to only update the last activity timer when the mouse actually changes position.</summary>
		private Point _prevMousePos;

		///<summary>Returning false guarantees that the message will continue to the next filter control.  Therefore this method inspects the messages,
		///but the messages are not consumed.</summary>
		public bool PreFilterMessage(ref Message m) {
			if(m.Msg==0x0100) {//Any keyboard input (WM_KEYDOWN=0x0100).
				Security.DateTimeLastActivity=DateTime.Now;
			}
			else if(m.Msg==0x0200 && _prevMousePos!=Cursor.Position) {//Mouse input (WM_MOUSEMOVE=0x0200) and position changed since last checked.
				_prevMousePos=Cursor.Position;
				Security.DateTimeLastActivity=DateTime.Now;
			}
			return false;//Always allow the message to continue to the next filter control
		}
	}
}
