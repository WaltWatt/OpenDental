using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MigraDoc.DocumentObjectModel;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.HL7;
using OpenDentBusiness.UI;
using PdfSharp.Pdf;
using CodeBase;
using System.Collections;

namespace OpenDental{
	///<summary> AptCur.AptNum can not be trusted fully inside of this form. This form can create new appointments without inserting them into the DB.
	///Due to this make sure you consider new appointments and handle accordingly. See _isInsertRequired.
	///Edit window for appointments.  Will have a DialogResult of Cancel if the appointment was marked as new and is deleted.</summary>
	public class FormApptEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.ODGrid gridPatient;
		private OpenDental.UI.ODGrid gridComm;
		private IContainer components;
		private ComboBox comboConfirmed;
		private ComboBox comboUnschedStatus;
		private Label label4;
		private ComboBox comboStatus;
		private Label label5;
		private Label labelStatus;
		private OpenDental.UI.Button butAudit;
		private OpenDental.UI.Button butTask;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.Button butPin;
		private Label label24;
		private CheckBox checkIsHygiene;
		private ComboBox comboClinic;
		private Label labelClinic;
		private ComboBox comboAssistant;
		private ComboBox comboProvHyg;
		private ComboBox comboProv;
		private Label label12;
		private CheckBox checkIsNewPatient;
		private Label label3;
		private Label label2;
		private OpenDental.UI.ODGrid gridProc;
		private System.Windows.Forms.Button butSlider;
		private TableTimeBar tbTime;
		private Label label6;
		private TextBox textTime;
		private ODtextBox textNote;
		private Label labelApptNote;
		private OpenDental.UI.Button butAddComm;
		public bool PinIsVisible;
		public bool PinClicked;
		public bool IsNew;
		private Appointment AptCur;
		private Appointment AptOld;
		///<summary>The string time pattern in the current increment. Not in the 5 minute increment.</summary>
		private StringBuilder strBTime;
		private bool mouseIsDown;
		private Point mouseOrigin;
		private Point sliderOrigin;
		private List <InsPlan> PlanList;
		private List<InsSub> SubList;
		private Patient pat;
		private Family fam;
		private ToolTip toolTip1;
		private ContextMenu contextMenuTimeArrived;
		private MenuItem menuItemArrivedNow;
		private ContextMenu contextMenuTimeSeated;
		private MenuItem menuItemSeatedNow;
		private ContextMenu contextMenuTimeDismissed;
		private MenuItem menuItemDismissedNow;
		private OpenDental.UI.Button butAdd;
		private OpenDental.UI.Button butDeleteProc;
		private OpenDental.UI.Button butComplete;
		private CheckBox checkTimeLocked;
		private OpenDental.UI.Button butPickHyg;
		private OpenDental.UI.Button butPickDentist;
		private ODGrid gridFields;
		private TextBox textTimeAskedToArrive;
		private Label label8;
		private OpenDental.UI.Button butPDF;
		///<summary>This is the way to pass a "signal" up to the parent form that OD is to close.</summary>
		public bool CloseOD;
		private ListBox listQuickAdd;
		private Panel panel1;
		private TextBox textRequirement;
		private UI.Button butRequirement;
		private UI.Button butInsPlan2;
		private UI.Button butInsPlan1;
		private TextBox textInsPlan2;
		private Label labelInsPlan2;
		private TextBox textInsPlan1;
		private Label labelInsPlan1;
		private TextBox textTimeDismissed;
		private Label label7;
		private TextBox textTimeSeated;
		private Label label1;
		private TextBox textTimeArrived;
		private Label labelTimeArrived;
		private TextBox textLabCase;
		private UI.Button butLab;
		private Label label9;
		private UI.Button butColorClear;
		private System.Windows.Forms.Button butColor;
		private UI.Button butText;
		private Label labelQuickAdd;
		private UI.Button butSyndromicObservations;
		private Label labelSyndromicObservations;
		///<summary>True if appt was double clicked on from the chart module gridProg.  Currently only used to trigger an appointment overlap check.</summary>
		public bool IsInChartModule;
		private ComboBox comboApptType;
		private Label label10;
		///<summary>True if appt was double clicked on from the ApptsOther form.  Currently only used to trigger an appointment overlap check.</summary>
		public bool IsInViewPatAppts;
		///<summary>Matches list of appointments in comboAppointmentType. Does not include hidden types unless current appointment is of that type.</summary>
		private List<AppointmentType> _listAppointmentType;
		///<summary>Procedure were attached/detached from appt and the user clicked cancel or closed the form.
		///Used in ApptModule to tell if we need to refresh.</summary>
		public bool HasProcsChangedAndCancel;
		///<summary>Lab for the current appointment.  It may be null if there is no lab.</summary>
		private LabCase _labCur;
		///<summary>A list of all appointments that are associated to any procedures in the Procedures on this Appointment grid.</summary>
		private List<Appointment> _listAppointments;
		///<summary>Stale deep copy of _listAppointments to use with sync.</summary>
		private List<Appointment> _listAppointmentsOld;
		private bool _isPlanned;
		private DataTable _tableFields;
		private DataTable _tableComms;
		private Label labelPlannedComplete;
		///<summary>Local copy of the provider cache for convenience due to how frequently provider cache is accessed.</summary>
		private List<Provider> _listProvidersAll;
		///<summary>Cached list of clinics available to user. Also includes a dummy Clinic at index 0 for "none".</summary>
		private List<Clinic> _listClinics;
		///<summary>Filtered list of providers based on which clinic is selected. If no clinic is selected displays all providers. Does not include a "none" option.</summary>
		private List<Provider> _listProvs;
		///<summary>Filtered list of providers based on which clinic is selected. If no clinic is selected displays all providers. Also includes a dummy provider at index 0 for "none"</summary>
		private List<Provider> _listProvHygs;
		///<summary>Used to keep track of the current clinic selected. This is because it may be a clinic that is, rarely, not in _listClinics.</summary>
		private long _selectedClinicNum;
		///<summary>Instead of relying on _listProviders[comboProv.SelectedIndex] to determine the selected Provider we use this variable to store it explicitly.</summary>
		private long _selectedProvNum;
		///<summary>Instead of relying on _listProviders[comboProvHyg.SelectedIndex] to determine the selected Provider we use this variable to store it explicitly.</summary>
		private long _selectedProvHygNum;
		///<summary>All ProcNums attached to the appt when form opened.</summary>
		private List<long> _listProcNumsAttachedStart=new List<long>();
		///<summary>Used when first loading the form to skip calling fill methods multiple times.</summary>
		private bool _isOnLoad;
		///<summary>List of all procedures that show within the Procedures on this Appointment grid.  Filled on load.
		///Used to double check that we update other appointments that we could steal procedures from (e.g. planned appts with tp procs).</summary>
		private List<Procedure> _listProcsForAppt;
		///<summary>The selected appointment type when this form loads.</summary>
		private AppointmentType _selectedAptType;
		///<summary>The exact index of the selected item in comboApptType.</summary>
		private int _aptTypeIndex;
		private List<PatPlan> _listPatPlans;
		private UI.Button butAttachAll;
		List<Benefit> _benefitList;
		private bool _isDeleted;
		private bool _isClickLocked;
		private Timer timerLockDelay;
		private int indexStatusBroken;
		///<summary>Used when FormApptBreak is required to track what the user has selected.</summary>
		private ApptBreakSelection _formApptBreakSelection=ApptBreakSelection.None;
		private ProcedureCode _procCodeBroken=null;
		private List<Employee> _listEmployees;

		///<summary>eCW Tight or Full enabled and a DFT msg for this appt has already been sent.  The 'Finish &amp; Send' button will say 'Revise'</summary>
		private bool _isEcwHL7Sent=false;
		private CheckBox checkASAP;
		///<summary>If no aptNum was passed into this form, this boolean will be set to true to indicate that AptCur.AptNum cannot be trusted until after the insert occurs.
		///Someday we should consider using the IsNew flag instead after we remove all of the appointment pre-insert logic.</summary>
		private bool _isInsertRequired=false;
		private List<Def> _listRecallUnschedStatusDefs;
		private List<Def> _listApptConfirmedDefs;
		private List<Def> _listApptProcsQuickAddDefs;
		///<summary>The data necesary to load the form.</summary>
		private ApptEdit.LoadData _loadData;

		#region Properties

		///<summary>The currently selected ApptStatus.</summary>
		private ApptStatus _selectedApptStatus {
			get {
				if(AptCur.AptStatus==ApptStatus.Planned) {
					return AptCur.AptStatus;
				}
				else if(comboStatus.SelectedIndex==-1) {
					return ApptStatus.Scheduled;
				}
				else if(AptCur.AptStatus==ApptStatus.PtNote || AptCur.AptStatus==ApptStatus.PtNoteCompleted) {
					return (ApptStatus)comboStatus.SelectedIndex + 7;
				}
				else if(comboStatus.SelectedIndex==3) {//Broken
					return ApptStatus.Broken;
				}
				else {//Scheduled, Complete, Unscheduled
					return (ApptStatus)comboStatus.SelectedIndex+1;
				}
			}
		}

		#endregion Properties

		///<summary>When aptNum is 0, make sure to set a valid patNum because a new appointment will be created/inserted on OK click.
		///Set useApptDrawingSettings to true if the user double clicked on the appointment schedule in order to make a new appointment.</summary>
		public FormApptEdit(long aptNum,long patNum=0,bool useApptDrawingSettings=false,Patient patient=null) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			_isClickLocked=true;
			if(aptNum==0) {//Creating a new appointment
				_isInsertRequired=true;
				Patient pat=patient??Patients.GetPat(patNum);
				if(pat==null) {
					MsgBox.Show(this,"Invalid patient passed in.  Please call support or try again.");
					DialogResult=DialogResult.Cancel;
					if(!this.Modal) {
						Close();
					}
					return;
				}
				AptCur=AppointmentL.MakeNewAppointment(pat,useApptDrawingSettings);
			}
			else {
				AptCur=Appointments.GetOneApt(aptNum);//We need this query to get the PatNum for the appointment.
			}
		}

		/// <summary>Clean up any resources being used.</summary>
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

		public Appointment GetAppointmentCur() {
			return AptCur.Copy();
		}

		public Appointment GetAppointmentOld() {
			return AptOld.Copy();
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApptEdit));
			this.comboConfirmed = new System.Windows.Forms.ComboBox();
			this.comboUnschedStatus = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.comboStatus = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.labelStatus = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.checkIsHygiene = new System.Windows.Forms.CheckBox();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.comboAssistant = new System.Windows.Forms.ComboBox();
			this.comboProvHyg = new System.Windows.Forms.ComboBox();
			this.comboProv = new System.Windows.Forms.ComboBox();
			this.label12 = new System.Windows.Forms.Label();
			this.checkIsNewPatient = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.labelApptNote = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.textTime = new System.Windows.Forms.TextBox();
			this.butSlider = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.checkTimeLocked = new System.Windows.Forms.CheckBox();
			this.contextMenuTimeArrived = new System.Windows.Forms.ContextMenu();
			this.menuItemArrivedNow = new System.Windows.Forms.MenuItem();
			this.contextMenuTimeSeated = new System.Windows.Forms.ContextMenu();
			this.menuItemSeatedNow = new System.Windows.Forms.MenuItem();
			this.contextMenuTimeDismissed = new System.Windows.Forms.ContextMenu();
			this.menuItemDismissedNow = new System.Windows.Forms.MenuItem();
			this.textTimeAskedToArrive = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.listQuickAdd = new System.Windows.Forms.ListBox();
			this.labelQuickAdd = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.checkASAP = new System.Windows.Forms.CheckBox();
			this.comboApptType = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.labelSyndromicObservations = new System.Windows.Forms.Label();
			this.butSyndromicObservations = new OpenDental.UI.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.butColorClear = new OpenDental.UI.Button();
			this.butColor = new System.Windows.Forms.Button();
			this.textRequirement = new System.Windows.Forms.TextBox();
			this.butRequirement = new OpenDental.UI.Button();
			this.butInsPlan2 = new OpenDental.UI.Button();
			this.butInsPlan1 = new OpenDental.UI.Button();
			this.textInsPlan2 = new System.Windows.Forms.TextBox();
			this.labelInsPlan2 = new System.Windows.Forms.Label();
			this.textInsPlan1 = new System.Windows.Forms.TextBox();
			this.labelInsPlan1 = new System.Windows.Forms.Label();
			this.textTimeDismissed = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.textTimeSeated = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textTimeArrived = new System.Windows.Forms.TextBox();
			this.labelTimeArrived = new System.Windows.Forms.Label();
			this.textLabCase = new System.Windows.Forms.TextBox();
			this.butLab = new OpenDental.UI.Button();
			this.butPickHyg = new OpenDental.UI.Button();
			this.butPickDentist = new OpenDental.UI.Button();
			this.gridFields = new OpenDental.UI.ODGrid();
			this.gridPatient = new OpenDental.UI.ODGrid();
			this.gridComm = new OpenDental.UI.ODGrid();
			this.gridProc = new OpenDental.UI.ODGrid();
			this.labelPlannedComplete = new System.Windows.Forms.Label();
			this.butPDF = new OpenDental.UI.Button();
			this.butComplete = new OpenDental.UI.Button();
			this.butDeleteProc = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.textNote = new OpenDental.ODtextBox();
			this.butText = new OpenDental.UI.Button();
			this.butAddComm = new OpenDental.UI.Button();
			this.tbTime = new OpenDental.TableTimeBar();
			this.butAudit = new OpenDental.UI.Button();
			this.butTask = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butPin = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butAttachAll = new OpenDental.UI.Button();
			this.timerLockDelay = new System.Windows.Forms.Timer(this.components);
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// comboConfirmed
			// 
			this.comboConfirmed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboConfirmed.Location = new System.Drawing.Point(114, 65);
			this.comboConfirmed.MaxDropDownItems = 30;
			this.comboConfirmed.Name = "comboConfirmed";
			this.comboConfirmed.Size = new System.Drawing.Size(126, 21);
			this.comboConfirmed.TabIndex = 84;
			this.comboConfirmed.SelectionChangeCommitted += new System.EventHandler(this.comboConfirmed_SelectionChangeCommitted);
			// 
			// comboUnschedStatus
			// 
			this.comboUnschedStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboUnschedStatus.Location = new System.Drawing.Point(114, 44);
			this.comboUnschedStatus.MaxDropDownItems = 30;
			this.comboUnschedStatus.Name = "comboUnschedStatus";
			this.comboUnschedStatus.Size = new System.Drawing.Size(126, 21);
			this.comboUnschedStatus.TabIndex = 83;
			// 
			// label4
			// 
			this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label4.Location = new System.Drawing.Point(6, 46);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(106, 16);
			this.label4.TabIndex = 82;
			this.label4.Text = "Unscheduled Status";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboStatus
			// 
			this.comboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStatus.Location = new System.Drawing.Point(114, 4);
			this.comboStatus.MaxDropDownItems = 30;
			this.comboStatus.Name = "comboStatus";
			this.comboStatus.Size = new System.Drawing.Size(126, 21);
			this.comboStatus.TabIndex = 81;
			this.comboStatus.SelectionChangeCommitted += new System.EventHandler(this.comboStatus_SelectionChangeCommitted);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(6, 67);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(106, 16);
			this.label5.TabIndex = 80;
			this.label5.Text = "Confirmed";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelStatus
			// 
			this.labelStatus.Location = new System.Drawing.Point(6, 6);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(106, 16);
			this.labelStatus.TabIndex = 79;
			this.labelStatus.Text = "Status";
			this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(128, 170);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(113, 16);
			this.label24.TabIndex = 138;
			this.label24.Text = "(use hyg color)";
			// 
			// checkIsHygiene
			// 
			this.checkIsHygiene.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsHygiene.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsHygiene.Location = new System.Drawing.Point(6, 170);
			this.checkIsHygiene.Name = "checkIsHygiene";
			this.checkIsHygiene.Size = new System.Drawing.Size(121, 16);
			this.checkIsHygiene.TabIndex = 137;
			this.checkIsHygiene.Text = "Is Hygiene";
			this.checkIsHygiene.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(114, 105);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(126, 21);
			this.comboClinic.TabIndex = 136;
			this.comboClinic.SelectedIndexChanged += new System.EventHandler(this.comboClinic_SelectedIndexChanged);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(6, 107);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(106, 16);
			this.labelClinic.TabIndex = 135;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboAssistant
			// 
			this.comboAssistant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAssistant.Location = new System.Drawing.Point(114, 187);
			this.comboAssistant.MaxDropDownItems = 30;
			this.comboAssistant.Name = "comboAssistant";
			this.comboAssistant.Size = new System.Drawing.Size(126, 21);
			this.comboAssistant.TabIndex = 133;
			// 
			// comboProvHyg
			// 
			this.comboProvHyg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProvHyg.Location = new System.Drawing.Point(114, 147);
			this.comboProvHyg.MaxDropDownItems = 30;
			this.comboProvHyg.Name = "comboProvHyg";
			this.comboProvHyg.Size = new System.Drawing.Size(106, 21);
			this.comboProvHyg.TabIndex = 132;
			this.comboProvHyg.SelectedIndexChanged += new System.EventHandler(this.comboProvHyg_SelectedIndexChanged);
			// 
			// comboProv
			// 
			this.comboProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProv.Location = new System.Drawing.Point(114, 126);
			this.comboProv.MaxDropDownItems = 30;
			this.comboProv.Name = "comboProv";
			this.comboProv.Size = new System.Drawing.Size(106, 21);
			this.comboProv.TabIndex = 131;
			this.comboProv.SelectedIndexChanged += new System.EventHandler(this.comboProv_SelectedIndexChanged);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(6, 189);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(106, 16);
			this.label12.TabIndex = 129;
			this.label12.Text = "Assistant";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkIsNewPatient
			// 
			this.checkIsNewPatient.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsNewPatient.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsNewPatient.Location = new System.Drawing.Point(6, 88);
			this.checkIsNewPatient.Name = "checkIsNewPatient";
			this.checkIsNewPatient.Size = new System.Drawing.Size(121, 16);
			this.checkIsNewPatient.TabIndex = 128;
			this.checkIsNewPatient.Text = "New Patient";
			this.checkIsNewPatient.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 149);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(106, 16);
			this.label3.TabIndex = 127;
			this.label3.Text = "Hygienist";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 128);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(106, 16);
			this.label2.TabIndex = 126;
			this.label2.Text = "Provider";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelApptNote
			// 
			this.labelApptNote.Location = new System.Drawing.Point(20, 451);
			this.labelApptNote.Name = "labelApptNote";
			this.labelApptNote.Size = new System.Drawing.Size(197, 16);
			this.labelApptNote.TabIndex = 141;
			this.labelApptNote.Text = "Appointment Note";
			this.labelApptNote.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(6, 212);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(106, 16);
			this.label6.TabIndex = 65;
			this.label6.Text = "Time Length";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTime
			// 
			this.textTime.Location = new System.Drawing.Point(114, 210);
			this.textTime.Name = "textTime";
			this.textTime.ReadOnly = true;
			this.textTime.Size = new System.Drawing.Size(66, 20);
			this.textTime.TabIndex = 62;
			// 
			// butSlider
			// 
			this.butSlider.BackColor = System.Drawing.SystemColors.ControlDark;
			this.butSlider.Location = new System.Drawing.Point(6, 90);
			this.butSlider.Name = "butSlider";
			this.butSlider.Size = new System.Drawing.Size(12, 15);
			this.butSlider.TabIndex = 60;
			this.butSlider.UseVisualStyleBackColor = false;
			this.butSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.butSlider_MouseDown);
			this.butSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.butSlider_MouseMove);
			this.butSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.butSlider_MouseUp);
			// 
			// checkTimeLocked
			// 
			this.checkTimeLocked.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTimeLocked.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTimeLocked.Location = new System.Drawing.Point(6, 232);
			this.checkTimeLocked.Name = "checkTimeLocked";
			this.checkTimeLocked.Size = new System.Drawing.Size(121, 16);
			this.checkTimeLocked.TabIndex = 148;
			this.checkTimeLocked.Text = "Time Locked";
			this.checkTimeLocked.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTimeLocked.Click += new System.EventHandler(this.checkTimeLocked_Click);
			// 
			// contextMenuTimeArrived
			// 
			this.contextMenuTimeArrived.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemArrivedNow});
			// 
			// menuItemArrivedNow
			// 
			this.menuItemArrivedNow.Index = 0;
			this.menuItemArrivedNow.Text = "Now";
			this.menuItemArrivedNow.Click += new System.EventHandler(this.menuItemArrivedNow_Click);
			// 
			// contextMenuTimeSeated
			// 
			this.contextMenuTimeSeated.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSeatedNow});
			// 
			// menuItemSeatedNow
			// 
			this.menuItemSeatedNow.Index = 0;
			this.menuItemSeatedNow.Text = "Now";
			this.menuItemSeatedNow.Click += new System.EventHandler(this.menuItemSeatedNow_Click);
			// 
			// contextMenuTimeDismissed
			// 
			this.contextMenuTimeDismissed.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemDismissedNow});
			// 
			// menuItemDismissedNow
			// 
			this.menuItemDismissedNow.Index = 0;
			this.menuItemDismissedNow.Text = "Now";
			this.menuItemDismissedNow.Click += new System.EventHandler(this.menuItemDismissedNow_Click);
			// 
			// textTimeAskedToArrive
			// 
			this.textTimeAskedToArrive.Location = new System.Drawing.Point(114, 293);
			this.textTimeAskedToArrive.Name = "textTimeAskedToArrive";
			this.textTimeAskedToArrive.Size = new System.Drawing.Size(126, 20);
			this.textTimeAskedToArrive.TabIndex = 146;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(6, 295);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(106, 16);
			this.label8.TabIndex = 160;
			this.label8.Text = "Time Ask To Arrive";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// listQuickAdd
			// 
			this.listQuickAdd.IntegralHeight = false;
			this.listQuickAdd.Location = new System.Drawing.Point(282, 48);
			this.listQuickAdd.Name = "listQuickAdd";
			this.listQuickAdd.Size = new System.Drawing.Size(150, 355);
			this.listQuickAdd.TabIndex = 163;
			this.listQuickAdd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listQuickAdd_MouseDown);
			// 
			// labelQuickAdd
			// 
			this.labelQuickAdd.Location = new System.Drawing.Point(282, 7);
			this.labelQuickAdd.Name = "labelQuickAdd";
			this.labelQuickAdd.Size = new System.Drawing.Size(143, 39);
			this.labelQuickAdd.TabIndex = 162;
			this.labelQuickAdd.Text = "Single click on items in the list below to add them to this appointment.";
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.checkASAP);
			this.panel1.Controls.Add(this.comboApptType);
			this.panel1.Controls.Add(this.label10);
			this.panel1.Controls.Add(this.labelSyndromicObservations);
			this.panel1.Controls.Add(this.butSyndromicObservations);
			this.panel1.Controls.Add(this.label9);
			this.panel1.Controls.Add(this.butColorClear);
			this.panel1.Controls.Add(this.butColor);
			this.panel1.Controls.Add(this.textRequirement);
			this.panel1.Controls.Add(this.butRequirement);
			this.panel1.Controls.Add(this.butInsPlan2);
			this.panel1.Controls.Add(this.butInsPlan1);
			this.panel1.Controls.Add(this.textInsPlan2);
			this.panel1.Controls.Add(this.labelInsPlan2);
			this.panel1.Controls.Add(this.textInsPlan1);
			this.panel1.Controls.Add(this.labelInsPlan1);
			this.panel1.Controls.Add(this.textTimeDismissed);
			this.panel1.Controls.Add(this.label7);
			this.panel1.Controls.Add(this.textTimeSeated);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.textTimeArrived);
			this.panel1.Controls.Add(this.labelTimeArrived);
			this.panel1.Controls.Add(this.textLabCase);
			this.panel1.Controls.Add(this.butLab);
			this.panel1.Controls.Add(this.comboStatus);
			this.panel1.Controls.Add(this.checkIsNewPatient);
			this.panel1.Controls.Add(this.comboConfirmed);
			this.panel1.Controls.Add(this.label24);
			this.panel1.Controls.Add(this.textTimeAskedToArrive);
			this.panel1.Controls.Add(this.comboUnschedStatus);
			this.panel1.Controls.Add(this.label8);
			this.panel1.Controls.Add(this.checkIsHygiene);
			this.panel1.Controls.Add(this.comboClinic);
			this.panel1.Controls.Add(this.labelClinic);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.comboAssistant);
			this.panel1.Controls.Add(this.butPickHyg);
			this.panel1.Controls.Add(this.comboProvHyg);
			this.panel1.Controls.Add(this.comboProv);
			this.panel1.Controls.Add(this.butPickDentist);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.label12);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.labelStatus);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.textTime);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.checkTimeLocked);
			this.panel1.Location = new System.Drawing.Point(21, 2);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(260, 446);
			this.panel1.TabIndex = 164;
			// 
			// checkASAP
			// 
			this.checkASAP.BackColor = System.Drawing.SystemColors.Control;
			this.checkASAP.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkASAP.ForeColor = System.Drawing.SystemColors.ControlText;
			this.checkASAP.Location = new System.Drawing.Point(6, 27);
			this.checkASAP.Name = "checkASAP";
			this.checkASAP.Size = new System.Drawing.Size(121, 16);
			this.checkASAP.TabIndex = 184;
			this.checkASAP.Text = "ASAP";
			this.checkASAP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkASAP.UseVisualStyleBackColor = false;
			this.checkASAP.CheckedChanged += new System.EventHandler(this.checkASAP_CheckedChanged);
			// 
			// comboApptType
			// 
			this.comboApptType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboApptType.Location = new System.Drawing.Point(114, 270);
			this.comboApptType.MaxDropDownItems = 30;
			this.comboApptType.Name = "comboApptType";
			this.comboApptType.Size = new System.Drawing.Size(126, 21);
			this.comboApptType.TabIndex = 183;
			this.comboApptType.SelectionChangeCommitted += new System.EventHandler(this.comboApptType_SelectionChangeCommitted);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(6, 272);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(106, 16);
			this.label10.TabIndex = 182;
			this.label10.Text = "Appointment Type";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelSyndromicObservations
			// 
			this.labelSyndromicObservations.Location = new System.Drawing.Point(63, 508);
			this.labelSyndromicObservations.Name = "labelSyndromicObservations";
			this.labelSyndromicObservations.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.labelSyndromicObservations.Size = new System.Drawing.Size(174, 16);
			this.labelSyndromicObservations.TabIndex = 181;
			this.labelSyndromicObservations.Text = "(Syndromic Observations)";
			this.labelSyndromicObservations.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelSyndromicObservations.Visible = false;
			// 
			// butSyndromicObservations
			// 
			this.butSyndromicObservations.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSyndromicObservations.Autosize = true;
			this.butSyndromicObservations.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSyndromicObservations.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSyndromicObservations.CornerRadius = 4F;
			this.butSyndromicObservations.Location = new System.Drawing.Point(15, 506);
			this.butSyndromicObservations.Name = "butSyndromicObservations";
			this.butSyndromicObservations.Size = new System.Drawing.Size(46, 20);
			this.butSyndromicObservations.TabIndex = 180;
			this.butSyndromicObservations.Text = "Obs";
			this.butSyndromicObservations.Visible = false;
			this.butSyndromicObservations.Click += new System.EventHandler(this.butSyndromicObservations_Click);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(6, 251);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(106, 16);
			this.label9.TabIndex = 179;
			this.label9.Text = "Color";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butColorClear
			// 
			this.butColorClear.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butColorClear.Autosize = true;
			this.butColorClear.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butColorClear.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butColorClear.CornerRadius = 4F;
			this.butColorClear.Location = new System.Drawing.Point(137, 248);
			this.butColorClear.Name = "butColorClear";
			this.butColorClear.Size = new System.Drawing.Size(39, 20);
			this.butColorClear.TabIndex = 178;
			this.butColorClear.Text = "none";
			this.butColorClear.Click += new System.EventHandler(this.butColorClear_Click);
			// 
			// butColor
			// 
			this.butColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColor.Location = new System.Drawing.Point(114, 249);
			this.butColor.Name = "butColor";
			this.butColor.Size = new System.Drawing.Size(21, 19);
			this.butColor.TabIndex = 177;
			this.butColor.Click += new System.EventHandler(this.butColor_Click);
			// 
			// textRequirement
			// 
			this.textRequirement.Location = new System.Drawing.Point(63, 454);
			this.textRequirement.Multiline = true;
			this.textRequirement.Name = "textRequirement";
			this.textRequirement.ReadOnly = true;
			this.textRequirement.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textRequirement.Size = new System.Drawing.Size(177, 53);
			this.textRequirement.TabIndex = 164;
			// 
			// butRequirement
			// 
			this.butRequirement.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRequirement.Autosize = true;
			this.butRequirement.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRequirement.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRequirement.CornerRadius = 4F;
			this.butRequirement.Location = new System.Drawing.Point(15, 454);
			this.butRequirement.Name = "butRequirement";
			this.butRequirement.Size = new System.Drawing.Size(46, 20);
			this.butRequirement.TabIndex = 163;
			this.butRequirement.Text = "Req";
			this.butRequirement.Click += new System.EventHandler(this.butRequirement_Click);
			// 
			// butInsPlan2
			// 
			this.butInsPlan2.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butInsPlan2.Autosize = false;
			this.butInsPlan2.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butInsPlan2.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butInsPlan2.CornerRadius = 2F;
			this.butInsPlan2.Location = new System.Drawing.Point(222, 433);
			this.butInsPlan2.Name = "butInsPlan2";
			this.butInsPlan2.Size = new System.Drawing.Size(18, 20);
			this.butInsPlan2.TabIndex = 176;
			this.butInsPlan2.Text = "...";
			this.butInsPlan2.Click += new System.EventHandler(this.butInsPlan2_Click);
			// 
			// butInsPlan1
			// 
			this.butInsPlan1.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butInsPlan1.Autosize = false;
			this.butInsPlan1.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butInsPlan1.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butInsPlan1.CornerRadius = 2F;
			this.butInsPlan1.Location = new System.Drawing.Point(222, 412);
			this.butInsPlan1.Name = "butInsPlan1";
			this.butInsPlan1.Size = new System.Drawing.Size(18, 20);
			this.butInsPlan1.TabIndex = 175;
			this.butInsPlan1.Text = "...";
			this.butInsPlan1.Click += new System.EventHandler(this.butInsPlan1_Click);
			// 
			// textInsPlan2
			// 
			this.textInsPlan2.Location = new System.Drawing.Point(63, 433);
			this.textInsPlan2.Name = "textInsPlan2";
			this.textInsPlan2.ReadOnly = true;
			this.textInsPlan2.Size = new System.Drawing.Size(158, 20);
			this.textInsPlan2.TabIndex = 174;
			// 
			// labelInsPlan2
			// 
			this.labelInsPlan2.Location = new System.Drawing.Point(6, 435);
			this.labelInsPlan2.Name = "labelInsPlan2";
			this.labelInsPlan2.Size = new System.Drawing.Size(55, 16);
			this.labelInsPlan2.TabIndex = 173;
			this.labelInsPlan2.Text = "InsPlan 2";
			this.labelInsPlan2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsPlan1
			// 
			this.textInsPlan1.Location = new System.Drawing.Point(63, 412);
			this.textInsPlan1.Name = "textInsPlan1";
			this.textInsPlan1.ReadOnly = true;
			this.textInsPlan1.Size = new System.Drawing.Size(158, 20);
			this.textInsPlan1.TabIndex = 172;
			// 
			// labelInsPlan1
			// 
			this.labelInsPlan1.Location = new System.Drawing.Point(6, 414);
			this.labelInsPlan1.Name = "labelInsPlan1";
			this.labelInsPlan1.Size = new System.Drawing.Size(55, 16);
			this.labelInsPlan1.TabIndex = 171;
			this.labelInsPlan1.Text = "InsPlan 1";
			this.labelInsPlan1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTimeDismissed
			// 
			this.textTimeDismissed.Location = new System.Drawing.Point(114, 353);
			this.textTimeDismissed.Name = "textTimeDismissed";
			this.textTimeDismissed.Size = new System.Drawing.Size(126, 20);
			this.textTimeDismissed.TabIndex = 170;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(6, 355);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(106, 16);
			this.label7.TabIndex = 169;
			this.label7.Text = "Time Dismissed";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTimeSeated
			// 
			this.textTimeSeated.Location = new System.Drawing.Point(114, 333);
			this.textTimeSeated.Name = "textTimeSeated";
			this.textTimeSeated.Size = new System.Drawing.Size(126, 20);
			this.textTimeSeated.TabIndex = 168;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 335);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(106, 16);
			this.label1.TabIndex = 166;
			this.label1.Text = "Time Seated";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTimeArrived
			// 
			this.textTimeArrived.Location = new System.Drawing.Point(114, 313);
			this.textTimeArrived.Name = "textTimeArrived";
			this.textTimeArrived.Size = new System.Drawing.Size(126, 20);
			this.textTimeArrived.TabIndex = 167;
			// 
			// labelTimeArrived
			// 
			this.labelTimeArrived.Location = new System.Drawing.Point(6, 315);
			this.labelTimeArrived.Name = "labelTimeArrived";
			this.labelTimeArrived.Size = new System.Drawing.Size(106, 16);
			this.labelTimeArrived.TabIndex = 165;
			this.labelTimeArrived.Text = "Time Arrived";
			this.labelTimeArrived.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textLabCase
			// 
			this.textLabCase.AcceptsReturn = true;
			this.textLabCase.Location = new System.Drawing.Point(63, 376);
			this.textLabCase.Multiline = true;
			this.textLabCase.Name = "textLabCase";
			this.textLabCase.ReadOnly = true;
			this.textLabCase.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textLabCase.Size = new System.Drawing.Size(177, 34);
			this.textLabCase.TabIndex = 162;
			// 
			// butLab
			// 
			this.butLab.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLab.Autosize = true;
			this.butLab.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLab.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLab.CornerRadius = 4F;
			this.butLab.Location = new System.Drawing.Point(15, 376);
			this.butLab.Name = "butLab";
			this.butLab.Size = new System.Drawing.Size(46, 20);
			this.butLab.TabIndex = 161;
			this.butLab.Text = "Lab";
			this.butLab.Click += new System.EventHandler(this.butLab_Click);
			// 
			// butPickHyg
			// 
			this.butPickHyg.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickHyg.Autosize = false;
			this.butPickHyg.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickHyg.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickHyg.CornerRadius = 2F;
			this.butPickHyg.Location = new System.Drawing.Point(222, 148);
			this.butPickHyg.Name = "butPickHyg";
			this.butPickHyg.Size = new System.Drawing.Size(18, 20);
			this.butPickHyg.TabIndex = 158;
			this.butPickHyg.Text = "...";
			this.butPickHyg.Click += new System.EventHandler(this.butPickHyg_Click);
			// 
			// butPickDentist
			// 
			this.butPickDentist.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickDentist.Autosize = false;
			this.butPickDentist.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickDentist.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickDentist.CornerRadius = 2F;
			this.butPickDentist.Location = new System.Drawing.Point(222, 127);
			this.butPickDentist.Name = "butPickDentist";
			this.butPickDentist.Size = new System.Drawing.Size(18, 20);
			this.butPickDentist.TabIndex = 157;
			this.butPickDentist.Text = "...";
			this.butPickDentist.Click += new System.EventHandler(this.butPickDentist_Click);
			// 
			// gridFields
			// 
			this.gridFields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridFields.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridFields.HasAddButton = false;
			this.gridFields.HasDropDowns = false;
			this.gridFields.HasMultilineHeaders = false;
			this.gridFields.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridFields.HeaderHeight = 15;
			this.gridFields.HScrollVisible = false;
			this.gridFields.Location = new System.Drawing.Point(21, 578);
			this.gridFields.Name = "gridFields";
			this.gridFields.ScrollValue = 0;
			this.gridFields.Size = new System.Drawing.Size(259, 118);
			this.gridFields.TabIndex = 159;
			this.gridFields.Title = "Appt Fields";
			this.gridFields.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridFields.TitleHeight = 18;
			this.gridFields.TranslationName = "FormApptEdit";
			this.gridFields.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridFields_CellDoubleClick);
			// 
			// gridPatient
			// 
			this.gridPatient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridPatient.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridPatient.HasAddButton = false;
			this.gridPatient.HasDropDowns = false;
			this.gridPatient.HasMultilineHeaders = false;
			this.gridPatient.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridPatient.HeaderHeight = 15;
			this.gridPatient.HScrollVisible = false;
			this.gridPatient.Location = new System.Drawing.Point(282, 405);
			this.gridPatient.Name = "gridPatient";
			this.gridPatient.ScrollValue = 0;
			this.gridPatient.Size = new System.Drawing.Size(258, 291);
			this.gridPatient.TabIndex = 0;
			this.gridPatient.Title = "Patient Info";
			this.gridPatient.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridPatient.TitleHeight = 18;
			this.gridPatient.TranslationName = "TableApptPtInfo";
			this.gridPatient.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPatient_CellClick);
			this.gridPatient.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridPatient_MouseMove);
			// 
			// gridComm
			// 
			this.gridComm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridComm.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridComm.HasAddButton = false;
			this.gridComm.HasDropDowns = false;
			this.gridComm.HasMultilineHeaders = false;
			this.gridComm.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridComm.HeaderHeight = 15;
			this.gridComm.HScrollVisible = false;
			this.gridComm.Location = new System.Drawing.Point(542, 405);
			this.gridComm.Name = "gridComm";
			this.gridComm.ScrollValue = 0;
			this.gridComm.Size = new System.Drawing.Size(335, 291);
			this.gridComm.TabIndex = 1;
			this.gridComm.Title = "Communications Log - Appointment Scheduling";
			this.gridComm.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridComm.TitleHeight = 18;
			this.gridComm.TranslationName = "TableCommLog";
			this.gridComm.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridComm_CellDoubleClick);
			this.gridComm.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridComm_MouseMove);
			// 
			// gridProc
			// 
			this.gridProc.AllowSelection = false;
			this.gridProc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridProc.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridProc.HasAddButton = false;
			this.gridProc.HasDropDowns = false;
			this.gridProc.HasMultilineHeaders = false;
			this.gridProc.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridProc.HeaderHeight = 15;
			this.gridProc.HScrollVisible = false;
			this.gridProc.Location = new System.Drawing.Point(434, 28);
			this.gridProc.Name = "gridProc";
			this.gridProc.ScrollValue = 0;
			this.gridProc.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridProc.Size = new System.Drawing.Size(538, 375);
			this.gridProc.TabIndex = 139;
			this.gridProc.Title = "Procedures on this Appointment";
			this.gridProc.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridProc.TitleHeight = 18;
			this.gridProc.TranslationName = "TableApptProcs";
			this.gridProc.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridProc_CellDoubleClick);
			this.gridProc.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridProc_CellClick);
			// 
			// labelPlannedComplete
			// 
			this.labelPlannedComplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelPlannedComplete.Location = new System.Drawing.Point(664, 1);
			this.labelPlannedComplete.Name = "labelPlannedComplete";
			this.labelPlannedComplete.Size = new System.Drawing.Size(227, 26);
			this.labelPlannedComplete.TabIndex = 184;
			this.labelPlannedComplete.Text = "This planned appointment is attached\r\nto a completed appointment.";
			this.labelPlannedComplete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelPlannedComplete.Visible = false;
			// 
			// butPDF
			// 
			this.butPDF.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPDF.Autosize = true;
			this.butPDF.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPDF.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPDF.CornerRadius = 4F;
			this.butPDF.Location = new System.Drawing.Point(880, 457);
			this.butPDF.Name = "butPDF";
			this.butPDF.Size = new System.Drawing.Size(92, 24);
			this.butPDF.TabIndex = 161;
			this.butPDF.Text = "Notes PDF";
			this.butPDF.Visible = false;
			this.butPDF.Click += new System.EventHandler(this.butPDF_Click);
			// 
			// butComplete
			// 
			this.butComplete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butComplete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butComplete.Autosize = true;
			this.butComplete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butComplete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butComplete.CornerRadius = 4F;
			this.butComplete.Location = new System.Drawing.Point(880, 483);
			this.butComplete.Name = "butComplete";
			this.butComplete.Size = new System.Drawing.Size(92, 24);
			this.butComplete.TabIndex = 155;
			this.butComplete.Text = "Finish && Send";
			this.butComplete.Visible = false;
			this.butComplete.Click += new System.EventHandler(this.butComplete_Click);
			// 
			// butDeleteProc
			// 
			this.butDeleteProc.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDeleteProc.Autosize = true;
			this.butDeleteProc.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDeleteProc.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDeleteProc.CornerRadius = 4F;
			this.butDeleteProc.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDeleteProc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDeleteProc.Location = new System.Drawing.Point(434, 2);
			this.butDeleteProc.Name = "butDeleteProc";
			this.butDeleteProc.Size = new System.Drawing.Size(75, 24);
			this.butDeleteProc.TabIndex = 154;
			this.butDeleteProc.Text = "Delete";
			this.butDeleteProc.Click += new System.EventHandler(this.butDeleteProc_Click);
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
			this.butAdd.Location = new System.Drawing.Point(510, 2);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 24);
			this.butAdd.TabIndex = 152;
			this.butAdd.Text = "Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// textNote
			// 
			this.textNote.AcceptsTab = true;
			this.textNote.BackColor = System.Drawing.SystemColors.Window;
			this.textNote.DetectLinksEnabled = false;
			this.textNote.DetectUrls = false;
			this.textNote.Location = new System.Drawing.Point(21, 469);
			this.textNote.Name = "textNote";
			this.textNote.QuickPasteType = OpenDentBusiness.QuickPasteType.Appointment;
			this.textNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textNote.Size = new System.Drawing.Size(260, 106);
			this.textNote.TabIndex = 142;
			this.textNote.Text = "";
			// 
			// butText
			// 
			this.butText.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butText.Autosize = true;
			this.butText.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butText.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butText.CornerRadius = 4F;
			this.butText.Image = global::OpenDental.Properties.Resources.Text;
			this.butText.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butText.Location = new System.Drawing.Point(880, 431);
			this.butText.Name = "butText";
			this.butText.Size = new System.Drawing.Size(92, 24);
			this.butText.TabIndex = 143;
			this.butText.Text = "Text";
			this.butText.Click += new System.EventHandler(this.butText_Click);
			// 
			// butAddComm
			// 
			this.butAddComm.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddComm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAddComm.Autosize = true;
			this.butAddComm.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddComm.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddComm.CornerRadius = 4F;
			this.butAddComm.Image = global::OpenDental.Properties.Resources.commlog;
			this.butAddComm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddComm.Location = new System.Drawing.Point(880, 405);
			this.butAddComm.Name = "butAddComm";
			this.butAddComm.Size = new System.Drawing.Size(92, 24);
			this.butAddComm.TabIndex = 143;
			this.butAddComm.Text = "Co&mm";
			this.butAddComm.Click += new System.EventHandler(this.butAddComm_Click);
			// 
			// tbTime
			// 
			this.tbTime.BackColor = System.Drawing.SystemColors.Window;
			this.tbTime.Location = new System.Drawing.Point(4, 6);
			this.tbTime.Name = "tbTime";
			this.tbTime.ScrollValue = 150;
			this.tbTime.SelectedIndices = new int[0];
			this.tbTime.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.tbTime.Size = new System.Drawing.Size(15, 561);
			this.tbTime.TabIndex = 59;
			// 
			// butAudit
			// 
			this.butAudit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAudit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAudit.Autosize = true;
			this.butAudit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAudit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAudit.CornerRadius = 4F;
			this.butAudit.Location = new System.Drawing.Point(880, 509);
			this.butAudit.Name = "butAudit";
			this.butAudit.Size = new System.Drawing.Size(92, 24);
			this.butAudit.TabIndex = 125;
			this.butAudit.Text = "Audit Trail";
			this.butAudit.Click += new System.EventHandler(this.butAudit_Click);
			// 
			// butTask
			// 
			this.butTask.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butTask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butTask.Autosize = true;
			this.butTask.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTask.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTask.CornerRadius = 4F;
			this.butTask.Location = new System.Drawing.Point(880, 535);
			this.butTask.Name = "butTask";
			this.butTask.Size = new System.Drawing.Size(92, 24);
			this.butTask.TabIndex = 124;
			this.butTask.Text = "To Task List";
			this.butTask.Click += new System.EventHandler(this.butTask_Click);
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(880, 587);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(92, 24);
			this.butDelete.TabIndex = 123;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butPin
			// 
			this.butPin.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPin.Autosize = true;
			this.butPin.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPin.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPin.CornerRadius = 4F;
			this.butPin.Image = ((System.Drawing.Image)(resources.GetObject("butPin.Image")));
			this.butPin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPin.Location = new System.Drawing.Point(880, 561);
			this.butPin.Name = "butPin";
			this.butPin.Size = new System.Drawing.Size(92, 24);
			this.butPin.TabIndex = 122;
			this.butPin.Text = "&Pinboard";
			this.butPin.Click += new System.EventHandler(this.butPin_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(880, 640);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(92, 24);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
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
			this.butCancel.Location = new System.Drawing.Point(880, 666);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(92, 24);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butAttachAll
			// 
			this.butAttachAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAttachAll.Autosize = true;
			this.butAttachAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAttachAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAttachAll.CornerRadius = 4F;
			this.butAttachAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAttachAll.Location = new System.Drawing.Point(586, 2);
			this.butAttachAll.Name = "butAttachAll";
			this.butAttachAll.Size = new System.Drawing.Size(75, 24);
			this.butAttachAll.TabIndex = 185;
			this.butAttachAll.Text = "Attach All";
			this.butAttachAll.Click += new System.EventHandler(this.butAttachAll_Click);
			// 
			// timerLockDelay
			// 
			this.timerLockDelay.Tick += new System.EventHandler(this._timerLockDelay_Tick);
			// 
			// FormApptEdit
			// 
			this.ClientSize = new System.Drawing.Size(974, 698);
			this.Controls.Add(this.butAttachAll);
			this.Controls.Add(this.labelPlannedComplete);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.listQuickAdd);
			this.Controls.Add(this.labelQuickAdd);
			this.Controls.Add(this.butPDF);
			this.Controls.Add(this.gridFields);
			this.Controls.Add(this.butComplete);
			this.Controls.Add(this.butDeleteProc);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.labelApptNote);
			this.Controls.Add(this.butText);
			this.Controls.Add(this.butAddComm);
			this.Controls.Add(this.butSlider);
			this.Controls.Add(this.tbTime);
			this.Controls.Add(this.gridPatient);
			this.Controls.Add(this.gridComm);
			this.Controls.Add(this.gridProc);
			this.Controls.Add(this.butAudit);
			this.Controls.Add(this.butTask);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butPin);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormApptEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Appointment";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormApptEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormApptEdit_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormApptEdit_Load(object sender, System.EventArgs e) {
			_selectedAptType=null;
			_aptTypeIndex=0;
			if(PrefC.GetBool(PrefName.AppointmentTypeShowPrompt) && IsNew
				&& !AptCur.AptStatus.In(ApptStatus.PtNote,ApptStatus.PtNoteCompleted))
			{
				FormApptTypes FormAT=new FormApptTypes();
				FormAT.IsSelectionMode=true;
				FormAT.ShowDialog();
				if(FormAT.DialogResult==DialogResult.OK) {
					_selectedAptType=FormAT.SelectedAptType;
				}
			}
			_isOnLoad=true;
			timerLockDelay.Interval=Math.Max((int)(PrefC.GetDouble(PrefName.FormClickDelay)*1000),1);
			timerLockDelay.Start();
			tbTime.CellClicked += new OpenDental.ContrTable.CellEventHandler(tbTime_CellClicked);
			_loadData=ApptEdit.GetLoadData(AptCur,IsNew);
			_listProcsForAppt=_loadData.ListProcsForAppt;
			_listAppointments=_loadData.ListAppointments;
			if(_listAppointments.Find(x => x.AptNum==AptCur.AptNum)==null) {
				_listAppointments.Add(AptCur);//Add AptCur if there are no procs attached to it.
			}
			_listAppointmentsOld=_listAppointments.Select(x => x.Copy()).ToList();
			for(int i=0;i<_listAppointments.Count;i++) {
				if(_listAppointments[i].AptNum==AptCur.AptNum) {
					AptCur=_listAppointments[i];//Changing the variable pointer so all changes are done on the element in the list.
				}
			}
			AptOld=AptCur.Copy();
			if(IsNew){
				if(!Security.IsAuthorized(Permissions.AppointmentCreate)) { //Should have been checked before appointment was inserted into DB and this form was loaded.  Left here just in case.
					DialogResult=DialogResult.Cancel;
					if(!this.Modal) {
						Close();
					}
					return;
				}
			}
			else {
				//The order of the conditional matters; C# will not evaluate the second part of the conditional if it is not needed. 
				//Changing the order will cause unneeded Security MsgBoxes to pop up.
				if (AptCur.AptStatus!=ApptStatus.Complete && !Security.IsAuthorized(Permissions.AppointmentEdit)
					|| (AptCur.AptStatus==ApptStatus.Complete && !Security.IsAuthorized(Permissions.AppointmentCompleteEdit))) 
				{//completed apts have their own perm.
					butOK.Enabled=false;
					butDelete.Enabled=false;
					butPin.Enabled=false;
					butTask.Enabled=false;
					gridProc.Enabled=false;
					listQuickAdd.Enabled=false;
					butAdd.Enabled=false;
					butDeleteProc.Enabled=false;
					butInsPlan1.Enabled=false;
					butInsPlan2.Enabled=false;
					butComplete.Enabled=false;
				}
			}
			if(!Security.IsAuthorized(Permissions.ApptConfirmStatusEdit,true)) {//Suppress message because it would be very annoying to users.
				comboConfirmed.Enabled=false;
			}
			//The objects below are needed when adding procs to this appt.
			fam=_loadData.Family;
			pat=fam.GetPatient(AptCur.PatNum);
			_listPatPlans=_loadData.ListPatPlans;
			_benefitList=_loadData.ListBenefits;
			SubList=_loadData.ListInsSubs;
			PlanList=_loadData.ListInsPlans;
			if(!PatPlans.IsPatPlanListValid(_listPatPlans,listInsSubs:SubList)) {
				_listPatPlans=PatPlans.Refresh(AptCur.PatNum);
				SubList=InsSubs.RefreshForFam(fam);
				PlanList=InsPlans.RefreshForSubList(SubList);
			}
			_tableFields=_loadData.TableApptFields;
			_tableComms=_loadData.TableComms;
			_listProvidersAll=Providers.GetDeepCopy();
			_isPlanned=false;
			if(AptCur.AptStatus==ApptStatus.Planned) {
				_isPlanned=true;
			}
			_labCur=_loadData.Lab;
			if(PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
				butRequirement.Visible=false;
				textRequirement.Visible=false;
			}
			if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				butSyndromicObservations.Visible=true;
				labelSyndromicObservations.Visible=true;
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				labelClinic.Visible=false;
				comboClinic.Visible=false;
			}
			if(!PinIsVisible){
				butPin.Visible=false;
			}
			string titleText = this.Text;
			if(_isPlanned) {
				titleText=Lan.g(this,"Edit Planned Appointment")+" - "+pat.GetNameFL();
				labelStatus.Visible=false;
				comboStatus.Visible=false;
				butDelete.Visible=false;
				if(_listAppointments.FindAll(x => x.NextAptNum==AptCur.AptNum)//This planned appt is attached to a completed appt.
					.Exists(x => x.AptStatus==ApptStatus.Complete)) 
				{
					labelPlannedComplete.Visible=true;
				}
			}
			else if(AptCur.AptStatus==ApptStatus.PtNote) {
				labelApptNote.Text="Patient NOTE:";
				titleText=Lan.g(this,"Edit Patient Note")+" - "+pat.GetNameFL()+" on "+AptCur.AptDateTime.DayOfWeek+", "+AptCur.AptDateTime;
				comboStatus.Items.Add(Lan.g("enumApptStatus","Patient Note"));
				comboStatus.Items.Add(Lan.g("enumApptStatus","Completed Pt. Note"));
				comboStatus.SelectedIndex=(int)AptCur.AptStatus-7;
				labelQuickAdd.Visible=false;
				labelStatus.Visible=false;
				gridProc.Visible=false;
				listQuickAdd.Visible=false;
				butAdd.Visible=false;
				butDeleteProc.Visible=false;
				butAttachAll.Visible=false;
				//textNote.Width = 400;
			}
			else if(AptCur.AptStatus==ApptStatus.PtNoteCompleted) {
				labelApptNote.Text="Completed Patient NOTE:";
				titleText=Lan.g(this,"Edit Completed Patient Note")+" - "+pat.GetNameFL()+" on "+AptCur.AptDateTime.DayOfWeek+", "+AptCur.AptDateTime;
				comboStatus.Items.Add(Lan.g("enumApptStatus","Patient Note"));
				comboStatus.Items.Add(Lan.g("enumApptStatus","Completed Pt. Note"));
				comboStatus.SelectedIndex=(int)AptCur.AptStatus-7;
				labelQuickAdd.Visible=false;
				labelStatus.Visible=false;
				gridProc.Visible=false;
				listQuickAdd.Visible=false;
				butAdd.Visible=false;
				butDeleteProc.Visible=false;
				butAttachAll.Visible=false;
				//textNote.Width = 400;
			}
			else {
				titleText=Lan.g(this, "Edit Appointment")+" - "+pat.GetNameFL()+" on "+AptCur.AptDateTime.DayOfWeek+", "+AptCur.AptDateTime;
				comboStatus.Items.Add(Lan.g("enumApptStatus","Scheduled"));
				comboStatus.Items.Add(Lan.g("enumApptStatus","Complete"));
				comboStatus.Items.Add(Lan.g("enumApptStatus","UnschedList"));
				indexStatusBroken=comboStatus.Items.Add(Lan.g("enumApptStatus","Broken"));
				if(AptCur.AptStatus==ApptStatus.Broken) {
					comboStatus.SelectedIndex=indexStatusBroken;
				}
				else {
					comboStatus.SelectedIndex=(int)AptCur.AptStatus-1;
				}
			}
			if(AptCur.Op != 0) {
				titleText+=" | "+Operatories.GetAbbrev(AptCur.Op);
			}
			this.Text = titleText;
			checkASAP.Checked=AptCur.Priority==ApptPriority.ASAP;
			if(AptCur.AptStatus==ApptStatus.UnschedList) {
				if(Programs.UsingEcwTightOrFullMode()) {
					comboStatus.Enabled=true;
				}
				else if(HL7Defs.GetOneDeepEnabled()!=null && !HL7Defs.GetOneDeepEnabled().ShowAppts) {
					comboStatus.Enabled=true;
				}
				else {
					comboStatus.Enabled=false;
				}
			}
			comboUnschedStatus.Items.Add(Lan.g(this,"none"));
			comboUnschedStatus.SelectedIndex=0;
			_listRecallUnschedStatusDefs=Defs.GetDefsForCategory(DefCat.RecallUnschedStatus,true);
			_listApptConfirmedDefs=Defs.GetDefsForCategory(DefCat.ApptConfirmed,true);
			_listApptProcsQuickAddDefs=Defs.GetDefsForCategory(DefCat.ApptProcsQuickAdd,true);
			for(int i=0;i<_listRecallUnschedStatusDefs.Count;i++) {
				comboUnschedStatus.Items.Add(_listRecallUnschedStatusDefs[i].ItemName);
				if(_listRecallUnschedStatusDefs[i].DefNum==AptCur.UnschedStatus)
					comboUnschedStatus.SelectedIndex=i+1;
			}
			for(int i=0;i<_listApptConfirmedDefs.Count;i++) {
				comboConfirmed.Items.Add(_listApptConfirmedDefs[i].ItemName);
				if(_listApptConfirmedDefs[i].DefNum==AptCur.Confirmed) {
					comboConfirmed.SelectedIndex=i;
				}
			}
			checkTimeLocked.Checked=AptCur.TimeLocked;
			textNote.Text=AptCur.Note;
			for(int i=0;i<_listApptProcsQuickAddDefs.Count;i++) {
				listQuickAdd.Items.Add(_listApptProcsQuickAddDefs[i].ItemName);
			}
			//Fill Clinics
			_listClinics=new List<Clinic>() { new Clinic() { Abbr=Lan.g(this,"None") } }; //Seed with "None"
			Clinics.GetForUserod(Security.CurUser).ForEach(x => _listClinics.Add(x));//do not re-organize from cache. They could either be alphabetizeded or sorted by item order.
			_listClinics.ForEach(x => comboClinic.Items.Add(x.Abbr));
			//Set Selected Nums
			_selectedClinicNum=AptCur.ClinicNum;
			if(IsNew) {
				//Try to auto-select a provider when in Orion mode. Only for new appointments so we don't change historical data.
				AptCur.ProvNum=Providers.GetOrionProvNum(AptCur.ProvNum);
			}
			_selectedProvNum=AptCur.ProvNum;
			_selectedProvHygNum=AptCur.ProvHyg;
			//Set combo indexes for first pass through fillComboProvHyg
			comboProv.SelectedIndex=-1;//initializes to 0. Must be -1 for fillComboProvHyg.
			comboProvHyg.SelectedIndex=-1;//initializes to 0. Must be -1 for fillComboProvHyg.
			comboClinic.IndexSelectOrSetText(_listClinics.FindIndex(x => x.ClinicNum==_selectedClinicNum),() => { return Clinics.GetAbbr(_selectedClinicNum); });
			fillComboProvHyg();
			checkIsHygiene.Checked=AptCur.IsHygiene;
			//Fill comboAssistant with employees and none option
			comboAssistant.Items.Add(Lan.g(this,"none"));
			comboAssistant.SelectedIndex=0;
			_listEmployees=Employees.GetDeepCopy(true);
			for(int i=0;i<_listEmployees.Count;i++) {
				comboAssistant.Items.Add(_listEmployees[i].FName);
				if(_listEmployees[i].EmployeeNum==AptCur.Assistant)
					comboAssistant.SelectedIndex=i+1;
			}
			textLabCase.Text=GetLabCaseDescript();
			textTimeArrived.ContextMenu=contextMenuTimeArrived;
			textTimeSeated.ContextMenu=contextMenuTimeSeated;
			textTimeDismissed.ContextMenu=contextMenuTimeDismissed;
			if(AptCur.DateTimeAskedToArrive.TimeOfDay>TimeSpan.FromHours(0)) {
				textTimeAskedToArrive.Text=AptCur.DateTimeAskedToArrive.ToShortTimeString();
			}
			if(AptCur.DateTimeArrived.TimeOfDay>TimeSpan.FromHours(0)){
				textTimeArrived.Text=AptCur.DateTimeArrived.ToShortTimeString();
			}
			if(AptCur.DateTimeSeated.TimeOfDay>TimeSpan.FromHours(0)){
				textTimeSeated.Text=AptCur.DateTimeSeated.ToShortTimeString();
			}
			if(AptCur.DateTimeDismissed.TimeOfDay>TimeSpan.FromHours(0)){
				textTimeDismissed.Text=AptCur.DateTimeDismissed.ToShortTimeString();
			}
			if(AptCur.AptStatus==ApptStatus.Complete
				|| AptCur.AptStatus==ApptStatus.Broken
				|| AptCur.AptStatus==ApptStatus.PtNote
				|| AptCur.AptStatus==ApptStatus.PtNoteCompleted) 
			{
				textInsPlan1.Text=InsPlans.GetCarrierName(AptCur.InsPlan1,PlanList);
				textInsPlan2.Text=InsPlans.GetCarrierName(AptCur.InsPlan2,PlanList);
			}
			else {//Get the current ins plans for the patient.
				butInsPlan1.Enabled=false;
				butInsPlan2.Enabled=false;
				InsSub sub1=InsSubs.GetSub(PatPlans.GetInsSubNum(_listPatPlans,PatPlans.GetOrdinal(PriSecMed.Primary,_listPatPlans,PlanList,SubList)),SubList);
				InsSub sub2=InsSubs.GetSub(PatPlans.GetInsSubNum(_listPatPlans,PatPlans.GetOrdinal(PriSecMed.Secondary,_listPatPlans,PlanList,SubList)),SubList);
				AptCur.InsPlan1=sub1.PlanNum;
				AptCur.InsPlan2=sub2.PlanNum;
				textInsPlan1.Text=InsPlans.GetCarrierName(AptCur.InsPlan1,PlanList);
				textInsPlan2.Text=InsPlans.GetCarrierName(AptCur.InsPlan2,PlanList);
			}
			if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
				List<ReqStudent> listStudents=_loadData.ListStudents;
				string requirements="";
				for(int i=0;i<listStudents.Count;i++) {
					if(i > 0) {
						requirements+="\r\n";
					}
					Provider student=_listProvidersAll.First(x => x.ProvNum==listStudents[i].ProvNum);
					requirements+=student.LName+", "+student.FName+": "+listStudents[i].Descript;
				}
				textRequirement.Text=requirements;
			}
			//IsNewPatient is set well before opening this form.
			checkIsNewPatient.Checked=AptCur.IsNewPatient;
			butColor.BackColor=AptCur.ColorOverride;
			if(ApptDrawing.MinPerIncr==5) {
				tbTime.TopBorder[0,12]=System.Drawing.Color.Black;
				tbTime.TopBorder[0,24]=System.Drawing.Color.Black;
				tbTime.TopBorder[0,36]=System.Drawing.Color.Black;
			}
			else if(ApptDrawing.MinPerIncr==10) {
				tbTime.TopBorder[0,6]=System.Drawing.Color.Black;
				tbTime.TopBorder[0,12]=System.Drawing.Color.Black;
				tbTime.TopBorder[0,18]=System.Drawing.Color.Black;
				tbTime.TopBorder[0,24]=System.Drawing.Color.Black;
				tbTime.TopBorder[0,30]=System.Drawing.Color.Black;
				tbTime.TopBorder[0,36]=System.Drawing.Color.Black;
			}
			else if(ApptDrawing.MinPerIncr==15){
				tbTime.TopBorder[0,4]=System.Drawing.Color.Black;
				tbTime.TopBorder[0,8]=System.Drawing.Color.Black;
				tbTime.TopBorder[0,12]=System.Drawing.Color.Black;
				tbTime.TopBorder[0,16]=System.Drawing.Color.Black;
				tbTime.TopBorder[0,20]=System.Drawing.Color.Black;
				tbTime.TopBorder[0,24]=System.Drawing.Color.Black;
				tbTime.TopBorder[0,28]=System.Drawing.Color.Black;
				tbTime.TopBorder[0,32]=System.Drawing.Color.Black;
				tbTime.TopBorder[0,36]=System.Drawing.Color.Black;
			}
			if(Programs.UsingEcwTightOrFullMode() && !_isInsertRequired) {
				//These buttons are ONLY for eCW, not any other HL7 interface.
				butComplete.Visible=true;
				butPDF.Visible=true;
				//for eCW, we need to hide some things--------------------
				if(Bridges.ECW.AptNum==AptCur.AptNum) {
					butDelete.Visible=false;
				}
				butPin.Visible=false;
				butTask.Visible=false;
				butAddComm.Visible=false;
				if(HL7Msgs.MessageWasSent(AptCur.AptNum)) {
					_isEcwHL7Sent=true;
					butComplete.Text="Revise";
					//if(!Security.IsAuthorized(Permissions.Setup,true)) {
					//	butComplete.Enabled=false;
					//	butPDF.Enabled=false;
					//}
					butOK.Enabled=false;
					gridProc.Enabled=false;
					listQuickAdd.Enabled=false;
					butAdd.Enabled=false;
					butDeleteProc.Enabled=false;
				}
				else {//hl7 was not sent for this appt
					_isEcwHL7Sent=false;
					butComplete.Text="Finish && Send";
					if(Bridges.ECW.AptNum != AptCur.AptNum) {
						butComplete.Enabled=false;
					}
					butPDF.Enabled=false;
				}
			}
			else {
				butComplete.Visible=false;
				butPDF.Visible=false;
			}
			//Hide text message button sometimes
			if(pat.WirelessPhone=="" || (!Programs.IsEnabled(ProgramName.CallFire) && !SmsPhones.IsIntegratedTextingEnabled())) {
				butText.Enabled=false;
			}
			else {//Pat has a wireless phone number and CallFire is enabled
				butText.Enabled=true;//TxtMsgOk checking performed on button click.
			}
			//AppointmentType
			_listAppointmentType=AppointmentTypes.GetWhere(x => !x.IsHidden || x.AppointmentTypeNum==AptCur.AppointmentTypeNum);
			comboApptType.Items.Add(Lan.g(this,"None"));
			comboApptType.SelectedIndex=0;
			foreach(AppointmentType aptType in _listAppointmentType) {
				comboApptType.Items.Add(aptType.AppointmentTypeName);
			}
			int selectedIndex=-1;
			if(IsNew && _selectedAptType!=null) { //selectedAptType will be null if they didn't select anything.
				selectedIndex=_listAppointmentType.FindIndex(x => x.AppointmentTypeNum==_selectedAptType.AppointmentTypeNum);
			}
			else {
				selectedIndex=_listAppointmentType.FindIndex(x => x.AppointmentTypeNum==AptCur.AppointmentTypeNum);
			}
			comboApptType.SelectedIndex=selectedIndex+1;//+1 for none
			_aptTypeIndex=comboApptType.SelectedIndex;
			HasProcsChangedAndCancel=false;
			FillProcedures();
			if(IsNew && comboApptType.SelectedIndex!=0) {
				AptTypeHelper();
			}
			//if this is a new appointment with no procedures attached, set the time pattern using the default preference
			if(IsNew && gridProc.SelectedIndices.Length < 1) {
				AptCur.Pattern=Appointments.GetApptTimePatternForNoProcs();
			}
			//convert time pattern from 5 to current increment.
			strBTime=new StringBuilder();
			for(int i=0;i<AptCur.Pattern.Length;i++) {
				strBTime.Append(AptCur.Pattern.Substring(i,1));
				if(PrefC.GetLong(PrefName.AppointmentTimeIncrement)==10) {
					i++;
				}
				if(PrefC.GetLong(PrefName.AppointmentTimeIncrement)==15) {
					i++;
					i++;
				}
			}
			FillPatient();//Must be after FillProcedures(), so that the initial amount for the appointment can be calculated.
			FillTime();
			FillComm();
			FillFields();
			textNote.Focus();
			textNote.SelectionStart = 0;
			_isOnLoad=false;
			Plugins.HookAddCode(this,"FormApptEdit.Load_End",pat,butText);
			Plugins.HookAddCode(this,"FormApptEdit.Load_end2",AptCur);//Lower casing the code area (_end) is the newer pattern for this.
			Plugins.HookAddCode(this,"FormApptEdit.Load_end3",AptCur,pat);
		}

		private void _timerLockDelay_Tick(object sender,EventArgs e) {
			_isClickLocked=false;
			timerLockDelay.Stop();
		}

		private void comboClinic_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboClinic.SelectedIndex>-1) {
				_selectedClinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
			}
			if(!_isOnLoad) {//If not called when loading form
				fillComboProvHyg();
				FillProcedures();
			}
		}

		private void comboProv_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboProv.SelectedIndex>-1) {
				_selectedProvNum=_listProvs[comboProv.SelectedIndex].ProvNum;
			}
		}

		private void comboProvHyg_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboProvHyg.SelectedIndex>-1) {
				_selectedProvHygNum=_listProvHygs[comboProvHyg.SelectedIndex].ProvNum;
			}
		}

		private void butPickDentist_Click(object sender,EventArgs e) {
			FormProviderPick formp=new FormProviderPick(_listProvs);
			formp.SelectedProvNum=_selectedProvNum;
			formp.ShowDialog();
			if(formp.DialogResult!=DialogResult.OK) {
				return;
			}
			_selectedProvNum=formp.SelectedProvNum;
			comboProv.IndexSelectOrSetText(_listProvs.FindIndex(x => x.ProvNum==_selectedProvNum),() => { return Providers.GetAbbr(_selectedProvNum); });
		}

		private void butPickHyg_Click(object sender,EventArgs e) {
			FormProviderPick formp=new FormProviderPick(_listProvHygs);//add none option to select providers.
			formp.SelectedProvNum=_selectedProvHygNum;
			formp.ShowDialog();
			if(formp.DialogResult!=DialogResult.OK) {
				return;
			}
			_selectedProvHygNum=formp.SelectedProvNum;
			comboProvHyg.IndexSelectOrSetText(_listProvHygs.FindIndex(x => x.ProvNum==_selectedProvHygNum),() => { return Providers.GetAbbr(_selectedProvHygNum); });
		}

		///<summary>Fills combo provider based on which clinic is selected and attempts to preserve provider selection if any.</summary>
		private void fillComboProvHyg() {
			if(comboProv.SelectedIndex>-1) {//valid prov selected, non none or nothing.
				_selectedProvNum = _listProvs[comboProv.SelectedIndex].ProvNum;
			}
			if(comboProvHyg.SelectedIndex>-1) {
				_selectedProvHygNum = _listProvHygs[comboProvHyg.SelectedIndex].ProvNum;
			}
			_listProvs=Providers.GetProvsForClinic(_selectedClinicNum).OrderBy(x => x.ItemOrder).ToList();
			_listProvHygs=Providers.GetProvsForClinic(_selectedClinicNum);
			_listProvHygs.Add(new Provider() { Abbr="none" });
			_listProvHygs=_listProvHygs.OrderBy(x => x.ProvNum>0).ThenBy(x => x.ItemOrder).ToList();
			//Fill comboProv
			comboProv.Items.Clear();
			_listProvs.ForEach(x => comboProv.Items.Add(x.Abbr));
			comboProv.IndexSelectOrSetText(_listProvs.FindIndex(x => x.ProvNum==_selectedProvNum),() => { return Providers.GetAbbr(_selectedProvNum); });
			//Fill comboProvHyg
			comboProvHyg.Items.Clear();
			_listProvHygs.ForEach(x => comboProvHyg.Items.Add(x.Abbr));
			comboProvHyg.IndexSelectOrSetText(_listProvHygs.FindIndex(x => x.ProvNum==_selectedProvHygNum),() => { return Providers.GetAbbr(_selectedProvHygNum); });
		}

		private void butColor_Click(object sender,EventArgs e) {
			ColorDialog colorDialog1=new ColorDialog();
			colorDialog1.Color=butColor.BackColor;
			colorDialog1.ShowDialog();
			butColor.BackColor=colorDialog1.Color;
		}

		private void butColorClear_Click(object sender,EventArgs e) {
			butColor.BackColor=System.Drawing.Color.FromArgb(0);
		}

		private void FillPatient(){
			DataTable table=_loadData.PatientTable;
			gridPatient.BeginUpdate();
			gridPatient.Columns.Clear();
			ODGridColumn col=new ODGridColumn("",120);//Add 2 blank columns
			gridPatient.Columns.Add(col);
			col=new ODGridColumn("",120);
			gridPatient.Columns.Add(col);
			gridPatient.Rows.Clear();
			ODGridRow row;
			for(int i=1;i<table.Rows.Count;i++) {//starts with 1 to skip name
				row=new ODGridRow();
				row.Cells.Add(table.Rows[i]["field"].ToString());
				row.Cells.Add(table.Rows[i]["value"].ToString());
				if(table.Rows[i]["field"].ToString().EndsWith("Phone")  && Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
					row.Cells[row.Cells.Count-1].ColorText=System.Drawing.Color.Blue;
					row.Cells[row.Cells.Count-1].Underline=YN.Yes;
				}
				gridPatient.Rows.Add(row);
			}
			//Add a UI managed row to display the total fee for the selected procedures in this appointment.
			row=new ODGridRow();
			row.Cells.Add(Lan.g(this,"Fee This Appt"));
			row.Cells.Add("");//Calculated below
			gridPatient.Rows.Add(row);
			CalcPatientFeeThisAppt();
			gridPatient.EndUpdate();
			gridPatient.ScrollToEnd();
		}

		///<summary>Calculates the fee for this appointment using the highlighted procedures in the procedure list.</summary>
		private void CalcPatientFeeThisAppt() {
			double feeThisAppt=0;
			for(int i=0;i<gridProc.SelectedIndices.Length;i++) {
				Procedure proc=((Procedure)(gridProc.Rows[gridProc.SelectedIndices[i]].Tag));
				double fee=proc.ProcFee;
				int qty=proc.BaseUnits+proc.UnitQty;
				if(qty>0) {
					fee*=qty;
				}
				feeThisAppt+=fee;
			}
			gridPatient.Rows[gridPatient.Rows.Count-1].Cells[1].Text=POut.Double(feeThisAppt);
			gridPatient.Invalidate();
		}

		private void FillFields() {
			gridFields.BeginUpdate();
			gridFields.Columns.Clear();
			ODGridColumn col=new ODGridColumn("",100);
			gridFields.Columns.Add(col);
			col=new ODGridColumn("",100);
			gridFields.Columns.Add(col);
			gridFields.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_tableFields.Rows.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_tableFields.Rows[i]["FieldName"].ToString());
				row.Cells.Add(_tableFields.Rows[i]["FieldValue"].ToString());
				gridFields.Rows.Add(row);
			}
			gridFields.EndUpdate();
		}

		private void FillComm(){
			gridComm.BeginUpdate();
			gridComm.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableCommLog","DateTime"),80);
			gridComm.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableCommLog","Description"),80);
			gridComm.Columns.Add(col);
			gridComm.Rows.Clear();
			ODGridRow row;
			List<Def> listMiscColorDefs=Defs.GetDefsForCategory(DefCat.MiscColors);
			for(int i=0;i<_tableComms.Rows.Count;i++) {
				row=new ODGridRow();
				if(PIn.Long(_tableComms.Rows[i]["CommlogNum"].ToString())>0) {
					row.Cells.Add(PIn.Date(_tableComms.Rows[i]["commDateTime"].ToString()).ToShortDateString());
					row.Cells.Add(_tableComms.Rows[i]["Note"].ToString());
					if(_tableComms.Rows[i]["CommType"].ToString()==Commlogs.GetTypeAuto(CommItemTypeAuto.APPT).ToString()){
						row.ColorBackG=listMiscColorDefs[7].ItemColor;
					}
				}
				else if(PIn.Long(_tableComms.Rows[i]["EmailMessageNum"].ToString())>0) {
					if(((HideInFlags)PIn.Int(_tableComms.Rows[i]["EmailMessageHideIn"].ToString())).HasFlag(HideInFlags.ApptEdit)) {
						continue;
					}
					row.Cells.Add(PIn.Date(_tableComms.Rows[i]["commDateTime"].ToString()).ToShortDateString());
					row.Cells.Add(_tableComms.Rows[i]["Subject"].ToString());
				}
				row.Tag=_tableComms.Rows[i];
				gridComm.Rows.Add(row);
			}
			gridComm.EndUpdate();
			gridComm.ScrollToEnd();
		}

		private void gridComm_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			DataRow row=((DataRow)gridComm.Rows[e.Row].Tag);
			long commNum=PIn.Long(row["CommlogNum"].ToString());
			long msgNum=PIn.Long(row["EmailMessageNum"].ToString());
			if (commNum>0) {
				Commlog item=Commlogs.GetOne(commNum);
				if(item==null) {
					MsgBox.Show(this,"This commlog has been deleted by another user.");
					return;
				}
				FormCommItem FormCI=new FormCommItem();
				FormCI.ShowDialog(new CommItemModel() { CommlogCur=item },new CommItemController(FormCI));
			}
			else if (msgNum>0) {
				EmailMessage email=EmailMessages.GetOne(msgNum);
				if (email==null) {
					MsgBox.Show(this,"This e-mail has been deleted by another user.");
					return;
				}
				FormEmailMessageEdit FormEME=new FormEmailMessageEdit(email,isDeleteAllowed:false);
				FormEME.ShowDialog();
			}
			_tableComms=Appointments.GetCommTable(AptCur.PatNum.ToString(),AptCur.AptNum);
			FillComm();
		}

		private void FillProcedures(){
			//Every time the procedures available have been manipulated (associated to appt, deleted, etc) we need to refresh the list from the db.
			//This has the potential to call the database a lot (cell click via a grid) but we accept this insufficiency for the benefit of concurrency.
			//If the following call to the db is to be removed, make sure that all procedure manipulations from FormProcEdit, FormClaimProcEdit, etc.
			//  handle the changes accordingly.  Changing this call to the database should not be done 'lightly'.  Heed our warning.
			List<Procedure> listProcs=_listProcsForAppt;
			listProcs.Sort(ProcedureLogic.CompareProcedures);
			List<long> listNumsSelected=new List<long>();
			if(_isOnLoad && !_isInsertRequired) {//First time filling the grid and not a new appointment.
				if(_isPlanned) {
					_listProcNumsAttachedStart=listProcs.FindAll(x => x.PlannedAptNum==AptCur.AptNum).Select(x => x.ProcNum).ToList();
				}
				else {//regular appointment
					//set ProcNums attached to the appt when form opened for use in automation on closing.
					_listProcNumsAttachedStart=listProcs.FindAll(x => x.AptNum==AptCur.AptNum).Select(x => x.ProcNum).ToList();
				}
				listNumsSelected.AddRange(_listProcNumsAttachedStart);
				if(Programs.UsingEcwTightOrFullMode() && !_isEcwHL7Sent) {//for eCW only and only if not in 'Revise' mode, select completed procs from _listProcsForAppt with ProcDate==AptDateTime
					//Attach procs to this appointment in memory only so that Cancel button still works.
					listNumsSelected.AddRange(listProcs.Where(x => x.ProcStatus==ProcStat.C && x.ProcDate.Date==AptCur.AptDateTime.Date).Select(x=>x.ProcNum));
				}
			}
			else {//Filling the grid later on.
				listNumsSelected.AddRange(gridProc.SelectedIndices.OfType<int>().Select(x => ((Procedure)gridProc.Rows[x].Tag).ProcNum));
			}
			bool isMedical=Clinics.IsMedicalPracticeOrClinic(_selectedClinicNum);
			gridProc.BeginUpdate();
			gridProc.Rows.Clear();
			gridProc.Columns.Clear();
			List<DisplayField> listAptDisplayFields;
			if(AptCur.AptStatus==ApptStatus.Planned){
				listAptDisplayFields=DisplayFields.GetForCategory(DisplayFieldCategory.PlannedAppointmentEdit);
			}
			else {
				listAptDisplayFields=DisplayFields.GetForCategory(DisplayFieldCategory.AppointmentEdit);
			}
			foreach(DisplayField displayField in listAptDisplayFields) {
				if(isMedical && (displayField.InternalName=="Surf" || displayField.InternalName=="Tth")) {
					continue;
				}
				gridProc.Columns.Add(new ODGridColumn(displayField.InternalName,displayField.ColumnWidth));
			}
			if(listAptDisplayFields.Sum(x => x.ColumnWidth) > gridProc.Width) {
				gridProc.HScrollVisible=true;
			}
			ODGridRow row;
			foreach(Procedure proc in listProcs) {
				row=new ODGridRow();
				ProcedureCode procCode=ProcedureCodes.GetProcCode(proc.CodeNum);
				foreach(DisplayField displayField in listAptDisplayFields) {
					switch (displayField.InternalName) {
						case "Stat":
							row.Cells.Add(proc.ProcStatus.ToString());
							break;
						case "Priority":
							row.Cells.Add(Defs.GetName(DefCat.TxPriorities,proc.Priority));
							break;
						case "Code":
								row.Cells.Add(procCode.ProcCode);
							break;
						case "Tth":
							if(isMedical) {
								continue;
							}
							row.Cells.Add(Tooth.GetToothLabel(proc.ToothNum));
							break;
						case "Surf":
							if(isMedical) {
								continue;
							}
							row.Cells.Add(proc.Surf);
							break;
						case "Description":
							string descript="";
							//This descript is gotten the same way it was in Appointments.GetProcTable()
							if(_isPlanned && proc.PlannedAptNum!=0 && proc.PlannedAptNum!=AptCur.AptNum) {
								descript+=Lan.g(this,"(other appt) ");
							}
							else if (_isPlanned && proc.AptNum!=0 && proc.AptNum!=AptCur.AptNum) {
								descript+=Lan.g(this,"(scheduled appt) ");
							}
							else if (!_isPlanned && proc.PlannedAptNum!=0 && proc.PlannedAptNum!=AptCur.AptNum) {
								descript+=Lan.g(this,"(planned appt) ");
							}
							else if(!_isPlanned && proc.AptNum!=0 && proc.AptNum!=AptCur.AptNum) {
								descript+=Lan.g(this,"(other appt) ");
							}
							if(procCode.LaymanTerm=="") {
								descript+=procCode.Descript;
							}
							else {
								descript+=procCode.LaymanTerm;
							}
							if(proc.ToothRange!="") {
								descript+=" #"+Tooth.FormatRangeForDisplay(proc.ToothRange);
							}
							row.Cells.Add(descript);
							break;
						case "Fee":
							double fee=proc.ProcFee;
							int qty=proc.BaseUnits+proc.UnitQty;
							if(qty>0) {
								fee*=qty;
							}
							row.Cells.Add(fee.ToString("F"));
							break;
						case "Abbreviation":
							row.Cells.Add(procCode.AbbrDesc);
							break;
						case "Layman's Term":
							row.Cells.Add(procCode.LaymanTerm);
							break;
					}
				}
				row.Tag=proc;
				gridProc.Rows.Add(row);
			}
			gridProc.EndUpdate();
			for(int i=0;i<listProcs.Count;i++) {
				if(listNumsSelected.Contains(listProcs[i].ProcNum)) {
					gridProc.SetSelected(i,true);
				}
			}
		}
		
		private string GetLabCaseDescript() {
			string descript="";
			if(_labCur!=null) {
				descript=Laboratories.GetOne(_labCur.LaboratoryNum).Description;
				if(_labCur.DateTimeChecked.Year>1880) {//Logic from Appointments.cs lines 1818 to 1840
					descript+=", "+Lan.g(this,"Quality Checked");
				}
				else {
					if(_labCur.DateTimeRecd.Year>1880) {
						descript+=", "+Lan.g(this,"Received");
					}
					else {
						if(_labCur.DateTimeSent.Year>1880) {
							descript+=", "+Lan.g(this,"Sent");
						}
						else {
							descript+=", "+Lan.g(this,"Not Sent");
						}
						if(_labCur.DateTimeDue.Year>1880) {
							descript+=", "+Lan.g(this,"Due: ")+_labCur.DateTimeDue.ToString("ddd")+" "
								+_labCur.DateTimeDue.ToShortDateString()+" "
								+_labCur.DateTimeDue.ToShortTimeString();
						}
					}
				}
			}
			return descript;
		}

		private void butAddComm_Click(object sender,EventArgs e) {
			Commlog CommlogCur=new Commlog();
			CommlogCur.PatNum=AptCur.PatNum;
			CommlogCur.CommDateTime=DateTime.Now;
			CommlogCur.CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.APPT);
			CommlogCur.UserNum=Security.CurUser.UserNum;
			FormCommItem FormCI=new FormCommItem();
			FormCI.ShowDialog(new CommItemModel() { CommlogCur=CommlogCur },new CommItemController(FormCI) { IsNew=true });
			_tableComms=Appointments.GetCommTable(AptCur.PatNum.ToString(),AptCur.AptNum);	
			FillComm();
		}

		private void butText_Click(object sender,EventArgs e) {
			if(Plugins.HookMethod(this,"FormApptEdit.butText_Click_start",pat,AptCur,this)) {
				return;
			}
			bool updateTextYN=false;
			if(pat.TxtMsgOk==YN.No) {
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"This patient is marked to not receive text messages. "
					+"Would you like to mark this patient as okay to receive text messages?")) {
					updateTextYN=true;
				}
				else {
					return;
				}
			}
			if(pat.TxtMsgOk==YN.Unknown && PrefC.GetBool(PrefName.TextMsgOkStatusTreatAsNo)) {
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"This patient might not want to receive text messages. "
					+"Would you like to mark this patient as okay to receive text messages?")) {
					updateTextYN=true;
				}
				else {
					return;
				}
			}
			if(updateTextYN) {
				Patient patOld=pat.Copy();
				pat.TxtMsgOk=YN.Yes;
				Patients.Update(pat,patOld);
			}
			string message;
			message=PrefC.GetString(PrefName.ConfirmTextMessage);
			message=message.Replace("[NameF]",pat.GetNameFirst());
			message=message.Replace("[NameFL]",pat.GetNameFL());
			message=message.Replace("[date]",AptCur.AptDateTime.ToShortDateString());
			message=message.Replace("[time]",AptCur.AptDateTime.ToShortTimeString());
			FormTxtMsgEdit FormTME=new FormTxtMsgEdit();
			FormTME.PatNum=pat.PatNum;
			FormTME.WirelessPhone=pat.WirelessPhone;
			FormTME.Message=message;
			FormTME.TxtMsgOk=pat.TxtMsgOk;
			FormTME.ShowDialog();
		}

		///<summary>Will only invert the specified procedure in the grid, even if the procedure belongs to another appointment.</summary>
		private void InvertCurProcSelected(int index) {
			bool isSelected=gridProc.SelectedIndices.Contains(index);
			gridProc.SetSelected(index,!isSelected);//Invert selection.
		}

		private void gridProc_CellClick(object sender,ODGridClickEventArgs e) {
			if(_isClickLocked) {
				return;
			}
			InvertCurProcSelected(e.Row);
			CalculateTime();
			FillTime();
			CalcPatientFeeThisAppt();
		}

		private void gridProc_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(_isClickLocked) {
				return;
			}
			InvertCurProcSelected(e.Row);
			//This will put the selection back to what is was before the single click event.
			//Get fresh copy from DB so we are not editing a stale procedure
			//If this is to be changed, make sure that this window is registering for procedure changes via signals or by some other means.
			Procedure proc=Procedures.GetOneProc(((Procedure)gridProc.Rows[e.Row].Tag).ProcNum,true);
			FormProcEdit FormP=new FormProcEdit(proc,pat,fam);
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK){
				CalculateTime();
				FillTime();
				return;
			}
			_listProcsForAppt=Procedures.GetProcsForApptEdit(AptCur);//We need to refresh in case the user changed the ProcCode or set the proc complete.
			FillProcedures();
			CalculateTime();
			FillTime();
		}

		private void butDeleteProc_Click(object sender,EventArgs e) {
			//this button will not be enabled if user does not have permission for AppointmentEdit
			if(gridProc.SelectedIndices.Length==0){
				MsgBox.Show(this,"Please select one or more procedures first.");
				return;
			}
			if(!MsgBox.Show(this,true,"Permanently delete all selected procedure(s)?")){
				return;
			}
			int skipped=0;
			int skippedSecurity=0;
			try{
				for(int i=gridProc.SelectedIndices.Length-1;i>=0;i--) {
					Procedure proc=(Procedure)gridProc.Rows[gridProc.SelectedIndices[i]].Tag;
					if(!Procedures.IsProcComplEditAuthorized(proc)) {
							skipped++;
							skippedSecurity++;
							continue;
					}
					if(!proc.ProcStatus.In(ProcStat.C,ProcStat.EO,ProcStat.EC)
						&& !Security.IsAuthorized(Permissions.ProcDelete,proc.ProcDate,true)) 
					{
						skippedSecurity++;
						continue;
					}
					Procedures.Delete(proc.ProcNum);
					if(proc.ProcStatus.In(ProcStat.C,ProcStat.EO,ProcStat.EC)) {
						Permissions perm=Permissions.ProcComplEdit;
						if(proc.ProcStatus.In(ProcStat.EO,ProcStat.EC)) {
							perm=Permissions.ProcExistingEdit;
						}
						SecurityLogs.MakeLogEntry(perm,AptCur.PatNum,ProcedureCodes.GetProcCode(proc.CodeNum).ProcCode
							+" ("+proc.ProcStatus+"), "+proc.ProcFee.ToString("c")+", Deleted");
					}
					else {
						SecurityLogs.MakeLogEntry(Permissions.ProcDelete,AptCur.PatNum,ProcedureCodes.GetProcCode(proc.CodeNum).ProcCode
							+" ("+proc.ProcStatus+"), "+proc.ProcFee.ToString("c"));
					}
				}
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
			}
			_listProcsForAppt=Procedures.GetProcsForApptEdit(AptCur);
			FillProcedures();
			CalculateTime();
			FillTime();
			CalcPatientFeeThisAppt();
			if(skipped>0) {
				MessageBox.Show(Lan.g(this,"Procedures skipped due to lack of permission to edit completed procedures: ")+skipped.ToString());
			}
			if(skippedSecurity>0) {
				MessageBox.Show(Lan.g(this,"Procedures skipped due to lack of permission to delete procedures: ")+skippedSecurity.ToString());
			}
		}

		private void butAdd_Click(object sender,EventArgs e) {
			if(_selectedProvNum==0) {
				MsgBox.Show(this,"Please select a provider.");
				return;
			}
			FormProcCodes FormP=new FormProcCodes();
			FormP.IsSelectionMode=true;
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK) {
				return;
			}
			Procedure proc=Procedures.ConstructProcedureForAppt(FormP.SelectedCodeNum,AptCur,pat,_listPatPlans,PlanList,SubList);
			Procedures.Insert(proc);
			Procedures.ComputeEstimates(proc,pat.PatNum,new List<ClaimProc>(),true,PlanList,_listPatPlans,_benefitList,pat.Age,SubList);
			FormProcEdit FormPE=new FormProcEdit(proc,pat.Copy(),fam);
			FormPE.IsNew=true;
			if(Programs.UsingOrion) {
				FormPE.OrionProvNum=_selectedProvNum;
				FormPE.OrionDentist=true;
			}
			FormPE.ShowDialog();
			if(FormPE.DialogResult==DialogResult.Cancel) {
				//any created claimprocs are automatically deleted from within procEdit window.
				try {
					Procedures.Delete(proc.ProcNum);//also deletes the claimprocs
				}
				catch(Exception ex) {
					MessageBox.Show(ex.Message);
				}
				return;
			}
			_listProcsForAppt=Procedures.GetProcsForApptEdit(AptCur);
			FillProcedures();
			for(int i=0;i<gridProc.Rows.Count;i++) {
				if(proc.ProcNum==((Procedure)gridProc.Rows[i].Tag).ProcNum) {
					gridProc.SetSelected(i,true);//Select those that were just added.
				}
			}
			CalculateTime();
			FillTime();
			CalcPatientFeeThisAppt();
		}
		
		private void butAttachAll_Click(object sender,EventArgs e) {
			if(_isClickLocked) {
				return;
			}
			gridProc.SetSelected(true);
			CalculateTime();
			FillTime();
			CalcPatientFeeThisAppt();
		}

		private void butQuickAdd_Click(object sender,EventArgs e) {
			/*
			if(AptCur.AptStatus==ApptStatus.Complete) {
				//added procedures would be marked complete when form closes. We'll just stop it here.
				if(!Security.IsAuthorized(Permissions.ProcComplCreate)) {
					return;
				}
			}
			FormApptQuickAdd formAq=new FormApptQuickAdd();
			formAq.ParentFormLocation=this.Location;
			formAq.ShowDialog();
			if(formAq.DialogResult!=DialogResult.OK) {
				return;
			}
			Procedures.SetDateFirstVisit(AptCur.AptDateTime.Date,1,pat);
			List<PatPlan> PatPlanList=PatPlans.Refresh(AptCur.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(PatPlanList);
			List<ClaimProc> ClaimProcList=ClaimProcs.Refresh(AptCur.PatNum);
			List<long> selectedProcNums=new List<long>();//start with the originally selected list, then add the new ones.
			for(int i=0;i<gridProc.SelectedIndices.Length;i++) {
				selectedProcNums.Add(PIn.Long(DS.Tables["Procedure"].Rows[gridProc.SelectedIndices[i]]["ProcNum"].ToString()));
			}
			for(int i=0;i<formAq.SelectedCodeNums.Count;i++) {
				Procedure ProcCur=new Procedure();
				ProcCur.PatNum=AptCur.PatNum;
				if(AptCur.AptStatus!=ApptStatus.Planned) {
					ProcCur.AptNum=AptCur.AptNum;
				}
				ProcCur.CodeNum=formAq.SelectedCodeNums[i];
				ProcCur.ProcDate=AptCur.AptDateTime.Date;
				ProcCur.DateTP=AptCur.AptDateTime.Date;
				InsPlan priplan=null;
				if(PatPlanList.Count>0) {
					priplan=InsPlans.GetPlan(PatPlanList[0].PlanNum,PlanList);
				}
				double insfee=Fees.GetAmount0(ProcCur.CodeNum,Fees.GetFeeSched(pat,PlanList,PatPlanList));
				if(priplan!=null && priplan.PlanType=="p") {//PPO
					double standardfee=Fees.GetAmount0(ProcCur.CodeNum,Providers.GetProv(Patients.GetProvNum(pat)).FeeSched);
					if(standardfee>insfee) {
						ProcCur.ProcFee=standardfee;
					}
					else {
						ProcCur.ProcFee=insfee;
					}
				}
				else {
					ProcCur.ProcFee=insfee;
				}
				//surf
				//toothnum
				//toothrange
				//priority
				ProcCur.ProcStatus=ProcStat.TP;
				//procnote
				ProcCur.ProvNum=AptCur.ProvNum;
				//Dx
				ProcCur.ClinicNum=AptCur.ClinicNum;
				ProcCur.SiteNum=pat.SiteNum;
				if(AptCur.AptStatus==ApptStatus.Planned) {
					ProcCur.PlannedAptNum=AptCur.AptNum;
				}
				ProcCur.MedicalCode=ProcedureCodes.GetProcCode(ProcCur.CodeNum).MedicalCode;
				ProcCur.BaseUnits=ProcedureCodes.GetProcCode(ProcCur.CodeNum).BaseUnits;
				Procedures.Insert(ProcCur);//recall synch not required
				selectedProcNums.Add(ProcCur.ProcNum);
				Procedures.ComputeEstimates(ProcCur,pat.PatNum,ClaimProcList,false,PlanList,PatPlanList,benefitList,pat.Age);
			}
			//listQuickAdd.SelectedIndex=-1;
			DS.Tables.Remove("Procedure");
			DS.Tables.Add(Appointments.GetApptEdit(AptCur.AptNum).Tables["Procedure"].Copy());
			FillProcedures();
			for(int i=0;i<gridProc.Rows.Count;i++) {
				for(int j=0;j<selectedProcNums.Count;j++) {
					if(selectedProcNums[j].ToString()==DS.Tables["Procedure"].Rows[i]["ProcNum"].ToString()) {
						gridProc.SetSelected(i,true);
					}
				}
			}
			CalculateTime();
			FillTime();
			CalcPatientFeeThisAppt();*/
		}

		private void FillTime() {
			System.Drawing.Color provColor=System.Drawing.Color.Gray;
			if(_selectedProvNum!=0 && _listProvidersAll.Any(x=>x.ProvNum==_selectedProvNum)) {
				provColor=_listProvidersAll.FirstOrDefault(x=>x.ProvNum==_selectedProvNum).ProvColor;
			}
			if(AptCur.IsHygiene && _selectedProvHygNum!=0 && _listProvidersAll.Any(x => x.ProvNum==_selectedProvHygNum)) {
				//Show the hygiene color if appointment is set to IsHygiene
				provColor=_listProvidersAll.FirstOrDefault(x => x.ProvNum==_selectedProvHygNum).ProvColor;
			}
			if(strBTime.Length > tbTime.MaxRows) {
				strBTime.Remove(tbTime.MaxRows-1,strBTime.Length-tbTime.MaxRows+1);//example: Remove(40-1,78-40+1), start at 39, remove 39.
				MsgBox.Show(this,"Appointment time shortened.  10 and 15 minute increments allow longer appointments than 5 minute increments.");
			}
			for(int i=0;i<strBTime.Length;i++) {
				if(strBTime.ToString(i,1)=="X") {
					tbTime.BackGColor[0,i]=provColor;
					//.Cell[0,i]=strBTime.ToString(i,1);
				}
				else {
					tbTime.BackGColor[0,i]=System.Drawing.Color.White;
				}
			}
			for(int i=strBTime.Length;i<tbTime.MaxRows;i++) {
				//tbTime.Cell[0,i]="";
				tbTime.BackGColor[0,i]=System.Drawing.Color.FromName("Control");
			}
			tbTime.Refresh();
			butSlider.Location=new Point(tbTime.Location.X+2,(tbTime.Location.Y+strBTime.Length*14+1));
			textTime.Text=(strBTime.Length*ApptDrawing.MinPerIncr).ToString();
		}

		private void CalculateTime(bool ignoreTimeLocked = false) {
			if(!ignoreTimeLocked && checkTimeLocked.Checked) {
				return;
			}
			//We are using the providers selected for the appt rather than the providers for the procs.
			//Providers for the procs get reset when closing this form.
			long provDent=Patients.GetProvNum(pat);
			long provHyg=Patients.GetProvNum(pat);
			if(_selectedProvNum!=0){
				provDent=_selectedProvNum;
				provHyg=_selectedProvNum;
			}
			if(_selectedProvHygNum!=0) {
				provHyg=_selectedProvHygNum;
			}
			List<long> codeNums=new List<long>();
			foreach(int i in gridProc.SelectedIndices) {
				codeNums.Add(((Procedure)gridProc.Rows[i].Tag).CodeNum);
			}
			strBTime=new StringBuilder(Appointments.CalculatePattern(provDent,provHyg,codeNums,false));
			//Plugins.HookAddCode(this,"FormApptEdit.CalculateTime_end",strBTime,provDent,provHyg,codeNums);//set strBTime, but without using the 'new' keyword.--Hook removed.
		}

		private void checkTimeLocked_Click(object sender,EventArgs e) {
			CalculateTime();
			FillTime();
		}

		private void tbTime_CellClicked(object sender,CellEventArgs e) {
			if(e.Row<strBTime.Length) {
				if(strBTime[e.Row]=='/') {
					strBTime.Replace('/','X',e.Row,1);
				}
				else {
					strBTime.Replace(strBTime[e.Row],'/',e.Row,1);
				}
			}
			else if(e.Row > strBTime.Length) {
				MoveTime(false,e.Row);
			}
			FillTime();
		}

		private void butSlider_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			mouseIsDown=true;
			mouseOrigin=new Point(e.X+butSlider.Location.X,e.Y+butSlider.Location.Y);
			sliderOrigin=butSlider.Location;
		}

		private void butSlider_MouseMove(object sender,System.Windows.Forms.MouseEventArgs e) {
			if(!mouseIsDown){
				return;
			}
			int step=(int)(Math.Round((Decimal)(sliderOrigin.Y+(e.Y+butSlider.Location.Y)-mouseOrigin.Y-tbTime.Location.Y)/14));
			MoveTime(true,step);
		}

		///<summary>Sets the length of the appointment time throught the butSlider or single click.
		///sliderMouseY needs to be the Y position relative to tbTimes top location.</summary>
		private void MoveTime(bool canShorten,int step) {			
			if(step==strBTime.Length){
				return;
			}
			if(step<1){
				return;
			}
			if(step>tbTime.MaxRows-1){
				return;
			}
			if(step>strBTime.Length) {
				strBTime.Append('/',step-strBTime.Length);
			}
			if(step<strBTime.Length) {
				if(!canShorten) {
					return;
				}
				strBTime.Remove(step,strBTime.Length-step);
			}
			checkTimeLocked.Checked=true;
			FillTime();
		}

		private void butSlider_MouseUp(object sender,System.Windows.Forms.MouseEventArgs e) {
			mouseIsDown=false;
		}
		
		private void gridComm_MouseMove(object sender,MouseEventArgs e) {
			
		}

		private void gridPatient_MouseMove(object sender,MouseEventArgs e) {
			
		}

		private void gridPatient_CellClick(object sender,ODGridClickEventArgs e) {
			ODGridCell gridCellCur=gridPatient.Rows[e.Row].Cells[e.Col];
			//Only grid cells with phone numbers are blue and underlined.
			if(gridCellCur.ColorText==System.Drawing.Color.Blue && gridCellCur.Underline==YN.Yes && Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
				DentalTek.PlaceCall(gridCellCur.Text);
			}
		}

		private void listQuickAdd_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			if(_isClickLocked) {
				return;
			}
			if(_selectedProvNum==0){
				MsgBox.Show(this,"Please select a provider.");
				return;
			}
			if(listQuickAdd.IndexFromPoint(e.X,e.Y)==-1) {
				return;
			}
			if(AptCur.AptStatus==ApptStatus.Complete) {
				//added procedures would be marked complete when form closes. We'll just stop it here.
				if(!Security.IsAuthorized(Permissions.ProcComplCreate)) {
					return;
				}
			}
			string[] codes=_listApptProcsQuickAddDefs[listQuickAdd.IndexFromPoint(e.X,e.Y)].ItemValue.Split(',');
			for(int i=0;i<codes.Length;i++) {
				if(!ProcedureCodes.GetContainsKey(codes[i])) {
					MsgBox.Show(this,"Definition contains invalid code.");
					return;
				}
			}
			ODTuple<List<Procedure>,List<Procedure>> result=ApptEdit.QuickAddProcs(AptCur,pat,codes.ToList(),_selectedProvNum,_selectedProvHygNum,SubList,
				PlanList,_listPatPlans,_benefitList);
			List<Procedure> listAddedProcs=result.Item1;
			_listProcsForAppt=result.Item2;
			if(Programs.UsingOrion) {//Orion requires a DPC for every procedure. Force proc edit window open.
				foreach(Procedure proc in listAddedProcs) {
					FormProcEdit FormP=new FormProcEdit(proc,pat.Copy(),fam);
					FormP.IsNew=true;
					FormP.OrionDentist=true;
					FormP.ShowDialog();
					if(FormP.DialogResult==DialogResult.Cancel) {
						try {
							Procedures.Delete(proc.ProcNum);//also deletes the claimprocs
						}
						catch(Exception ex) {
							MessageBox.Show(ex.Message);
						}
					}
				}
			}
			listQuickAdd.SelectedIndex=-1;
			FillProcedures();
			for(int i=0;i<gridProc.Rows.Count;i++) {
				//at this point, all procedures in the list should have a Primary Key.
				long procNumCur=((Procedure)gridProc.Rows[i].Tag).ProcNum;
				if(listAddedProcs.Any(x => x.ProcNum==procNumCur)) {
					gridProc.SetSelected(i,true);//Select those that were just added.
				}
			}
			CalculateTime();
			FillTime();
			CalcPatientFeeThisAppt();
		}

		private void butLab_Click(object sender,EventArgs e) {
			if(_isInsertRequired && !UpdateListAndDB(false)) {
				return;
			}			
			if(_labCur==null) {//no labcase
				//so let user pick one to add
				FormLabCaseSelect FormL=new FormLabCaseSelect();
				FormL.PatNum=AptCur.PatNum;
				FormL.IsPlanned=_isPlanned;
				FormL.ShowDialog();
				if(FormL.DialogResult!=DialogResult.OK){
					return;
				}
				if(_isPlanned) {
					LabCases.AttachToPlannedAppt(FormL.SelectedLabCaseNum,AptCur.AptNum);
				}
				else{
					LabCases.AttachToAppt(FormL.SelectedLabCaseNum,AptCur.AptNum);
				}
			}
			else{//already a labcase attached
				FormLabCaseEdit FormLCE=new FormLabCaseEdit();
				FormLCE.CaseCur=_labCur;
				FormLCE.ShowDialog();
				if(FormLCE.DialogResult!=DialogResult.OK){
					return;
				}
				//Deleting or detaching labcase would have been done from in that window
			}
			_labCur=LabCases.GetForApt(AptCur);
			textLabCase.Text=GetLabCaseDescript();
		}

		private void butInsPlan1_Click(object sender,EventArgs e) {
			FormInsPlanSelect FormIPS=new FormInsPlanSelect(AptCur.PatNum);
			FormIPS.ShowNoneButton=true;
			FormIPS.ViewRelat=false;
			FormIPS.ShowDialog();
			if(FormIPS.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormIPS.SelectedPlan==null) {
				AptCur.InsPlan1=0;
				textInsPlan1.Text="";
				return;
			}
			AptCur.InsPlan1=FormIPS.SelectedPlan.PlanNum;
			textInsPlan1.Text=InsPlans.GetCarrierName(AptCur.InsPlan1,PlanList);
		}

		private void butInsPlan2_Click(object sender,EventArgs e) {
			FormInsPlanSelect FormIPS=new FormInsPlanSelect(AptCur.PatNum);
			FormIPS.ShowNoneButton=true;
			FormIPS.ViewRelat=false;
			FormIPS.ShowDialog();
			if(FormIPS.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormIPS.SelectedPlan==null) {
				AptCur.InsPlan2=0;
				textInsPlan2.Text="";
				return;
			}
			AptCur.InsPlan2=FormIPS.SelectedPlan.PlanNum;
			textInsPlan2.Text=InsPlans.GetCarrierName(AptCur.InsPlan2,PlanList);
		}

		private void butRequirement_Click(object sender,EventArgs e) {
			if(_isInsertRequired && !UpdateListAndDB(false)) {
				return;
			}			
			FormReqAppt FormR=new FormReqAppt();
			FormR.AptNum=AptCur.AptNum;
			FormR.PatNum=AptCur.PatNum;
			FormR.ShowDialog();
			if(FormR.DialogResult!=DialogResult.OK){
				return;
			}
			List<ReqStudent> listStudents=ReqStudents.GetForAppt(AptCur.AptNum);
			textRequirement.Text = string.Join("\r\n",listStudents
				.Select(x => new { Student = _listProvidersAll.First(y => y.ProvNum==x.ProvNum),Descript = x.Descript })
				.Select(x => x.Student.LName+", "+x.Student.FName+": "+x.Descript).ToList());
		}

		private void butSyndromicObservations_Click(object sender,EventArgs e) {
			FormEhrAptObses formE=new FormEhrAptObses(AptCur);
			formE.ShowDialog();
		}

		private void menuItemArrivedNow_Click(object sender,EventArgs e) {
			textTimeArrived.Text=DateTime.Now.ToShortTimeString();
		}

		private void menuItemSeatedNow_Click(object sender,EventArgs e) {
			textTimeSeated.Text=DateTime.Now.ToShortTimeString();
		}

		private void menuItemDismissedNow_Click(object sender,EventArgs e) {
			textTimeDismissed.Text=DateTime.Now.ToShortTimeString();
		}

		private void gridFields_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(_isInsertRequired && !UpdateListAndDB(false)) {
				return;
			}
			if(ApptFieldDefs.HasDuplicateFieldNames()) {//Check for duplicate field names.
				MsgBox.Show(this,"There are duplicate appointment field defs, go rename or delete the duplicates.");
				return;
			}
			ApptField field=ApptFields.GetOne(PIn.Long(_tableFields.Rows[e.Row]["ApptFieldNum"].ToString()));
			if(field==null) {
				field=new ApptField();
				field.AptNum=AptCur.AptNum;
				field.FieldName=_tableFields.Rows[e.Row]["FieldName"].ToString();
				ApptFieldDef fieldDef=ApptFieldDefs.GetFieldDefByFieldName(field.FieldName);
				if(fieldDef==null) {//This could happen if the field def was deleted while the appointment window was open.
					MsgBox.Show(this,"This Appointment Field Def no longer exists.");
				}
				else {
					if(fieldDef.FieldType==ApptFieldType.Text) {
						FormApptFieldEdit formAF=new FormApptFieldEdit(field);
						formAF.IsNew=true;
						formAF.ShowDialog();
					}
					else if(fieldDef.FieldType==ApptFieldType.PickList) {
						FormApptFieldPickEdit formAF=new FormApptFieldPickEdit(field);
						formAF.IsNew=true;
						formAF.ShowDialog();
					}
				}
			}
			else if(ApptFieldDefs.GetFieldDefByFieldName(field.FieldName)!=null) {
				if(ApptFieldDefs.GetFieldDefByFieldName(field.FieldName).FieldType==ApptFieldType.Text) {
					FormApptFieldEdit formAF=new FormApptFieldEdit(field);
					formAF.ShowDialog();
				}
				else if(ApptFieldDefs.GetFieldDefByFieldName(field.FieldName).FieldType==ApptFieldType.PickList) {
					FormApptFieldPickEdit formAF=new FormApptFieldPickEdit(field);
					formAF.ShowDialog();
				}
			}
			else {//This probably won't happen because a field def should not be able to be deleted while in use.
				MsgBox.Show(this,"This Appointment Field Def no longer exists.");
			}
			_tableFields=Appointments.GetApptFields(AptCur.AptNum);
			FillFields();
		}

		///<summary>Validates and saves appointment and procedure information to DB.</summary>
		private bool UpdateListAndDB(bool isClosing=true,bool doCreateSecLog=false,bool doInsertHL7=false) {
			DateTime datePrevious=AptCur.DateTStamp;
			_listProcsForAppt=Procedures.GetProcsForApptEdit(AptCur);//We need to refresh so we can check for concurrency issues.
			FillProcedures();//This refills the tags in the grid so we can use the tags below.  Will also show concurrent changes by other users.
			#region PrefName.ApptsRequireProc and Permissions.ProcComplCreate check
			//First check that they have an procedures attached to this appointment. If the appointment is an existing appointment that did not originally
			//have any procedures attached, the prompt will not come up.
			if((IsNew || _listProcNumsAttachedStart.Count>0)
				&& PrefC.GetBool(PrefName.ApptsRequireProc)
				&& gridProc.SelectedIndices.Length==0
				&& !AptCur.AptStatus.In(ApptStatus.PtNote,ApptStatus.PtNoteCompleted)) 
			{
				MsgBox.Show(this,"At least one procedure must be attached to the appointment.");
				return false;
			}
			if(AptCur.AptStatus==ApptStatus.Complete && gridProc.SelectedIndices.Select(x => (Procedure)gridProc.Rows[x].Tag).Any(x => x.ProcStatus!=ProcStat.C)) {//Appt is complete, but a selected proc is not.
				List<Procedure> listSelectedProcs=gridProc.SelectedIndices.Select(x => (Procedure)gridProc.Rows[x].Tag).ToList();
				foreach(Procedure proc in listSelectedProcs) {
					if(!Security.IsAuthorized(Permissions.ProcComplCreate,AptCur.AptDateTime,proc.CodeNum,proc.ProcFee)) {
						return false;
					}
				}
			}
			#endregion
			#region Check for Procs Attached to Another Appt
			//When _isInsertRequired is true AptCur.AptNum=0.
			//The below logic works when 0 due to AptCur.[Planned]AptNum!=0 checks.
			bool hasProcsConcurrent=false;
			for(int i=0;i<gridProc.Rows.Count;i++) {
				Procedure proc=(Procedure)gridProc.Rows[i].Tag;
				bool isAttaching=gridProc.SelectedIndices.Contains(i);
				bool isAttachedStart=_listProcNumsAttachedStart.Contains(proc.ProcNum);
				if(!isAttachedStart && isAttaching && _isPlanned) {//Attaching to this planned appointment.
					if(proc.PlannedAptNum != 0 && proc.PlannedAptNum != AptCur.AptNum) {//However, the procedure is attached to another planned appointment.
						hasProcsConcurrent=true;
						break;
					}
				}
				else if(!isAttachedStart && isAttaching && !_isPlanned) {//Attaching to this appointment.
					if(proc.AptNum != 0 && proc.AptNum != AptCur.AptNum) {//However, the procedure is attached to another appointment.
						hasProcsConcurrent=true;
						break;
					}
				}
			}
			if(hasProcsConcurrent && _isPlanned) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,
					"One or more procedures are attached to another planned appointment.\r\n"
					+"All selected procedures will be detached from the other planned appointment.\r\n"
					+"Continue?"))
				{
					return false;
				}
			}
			else if(hasProcsConcurrent && !_isPlanned) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,
					"One or more procedures are attached to another appointment.\r\n"
					+"All selected procedures will be detached from the other appointment.\r\n"
					+"Continue?"))
				{
					return false;
				}
			}
			#endregion Check for Procs Attached to Another Appt
			#region Validate Form Data
			if(AptOld.AptStatus!=ApptStatus.UnschedList && comboStatus.SelectedIndex==2) {//previously not on unsched list and sending to unscheduled list
				if(PatRestrictionL.IsRestricted(AptCur.PatNum,PatRestrict.ApptSchedule,true)) {
					MessageBox.Show(Lan.g(this,"Not allowed to send this appointment to the unscheduled list due to patient restriction")+" "
						+PatRestrictions.GetPatRestrictDesc(PatRestrict.ApptSchedule)+".");
					return false;
				}
			}
			DateTime dateTimeAskedToArrive=DateTime.MinValue;
			if((AptOld.AptStatus==ApptStatus.Complete && comboStatus.SelectedIndex!=1)
				|| (AptOld.AptStatus==ApptStatus.Broken && comboStatus.SelectedIndex!=4)) //Un-completing or un-breaking the appt.  We must use selectedindex due to AptCur gets updated later UpdateDB()
			{
				//If the insurance plans have changed since this appt was completed, warn the user that the historical data will be neutralized.
				List<PatPlan> listPatPlans=PatPlans.Refresh(pat.PatNum);
				InsSub sub1=InsSubs.GetSub(PatPlans.GetInsSubNum(listPatPlans,PatPlans.GetOrdinal(PriSecMed.Primary,listPatPlans,PlanList,SubList)),SubList);
				InsSub sub2=InsSubs.GetSub(PatPlans.GetInsSubNum(listPatPlans,PatPlans.GetOrdinal(PriSecMed.Secondary,listPatPlans,PlanList,SubList)),SubList);
				if(sub1.PlanNum!=AptCur.InsPlan1 || sub2.PlanNum!=AptCur.InsPlan2) {
					if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"The current insurance plans for this patient are different than the plans associated to this appointment.  They will be updated to the patient's current insurance plans.  Continue?")) {
						return false;
					}
					//Update the ins plans associated to this appointment so that they're the most accurate at this time.
					AptCur.InsPlan1=sub1.PlanNum;
					AptCur.InsPlan2=sub2.PlanNum;
				}
			}
			if(textTimeAskedToArrive.Text!=""){
				try{
					dateTimeAskedToArrive=AptCur.AptDateTime.Date+DateTime.Parse(textTimeAskedToArrive.Text).TimeOfDay;
				}
				catch{
					MsgBox.Show(this,"Time Asked To Arrive invalid.");
					return false;
				}
			}
			DateTime dateTimeArrived=AptCur.AptDateTime.Date;
			if(textTimeArrived.Text!=""){
				try{
					dateTimeArrived=AptCur.AptDateTime.Date+DateTime.Parse(textTimeArrived.Text).TimeOfDay;
				}
				catch{
					MsgBox.Show(this,"Time Arrived invalid.");
					return false;
				}
			}
			DateTime dateTimeSeated=AptCur.AptDateTime.Date;
			if(textTimeSeated.Text!=""){
				try{
					dateTimeSeated=AptCur.AptDateTime.Date+DateTime.Parse(textTimeSeated.Text).TimeOfDay;
				}
				catch{
					MsgBox.Show(this,"Time Seated invalid.");
					return false;
				}
			}
			DateTime dateTimeDismissed=AptCur.AptDateTime.Date;
			if(textTimeDismissed.Text!=""){
				try{
					dateTimeDismissed=AptCur.AptDateTime.Date+DateTime.Parse(textTimeDismissed.Text).TimeOfDay;
				}
				catch{
					MsgBox.Show(this,"Time Dismissed invalid.");
					return false;
				}
			}
			//This change was just slightly too risky to make to 6.9, so 7.0 only
			if(!PrefC.GetBool(PrefName.ApptAllowFutureComplete)//Not allowed to set future appts complete.
				&& AptCur.AptStatus!=ApptStatus.Complete//was not originally complete
				&& AptCur.AptStatus!=ApptStatus.PtNote
				&& AptCur.AptStatus!=ApptStatus.PtNoteCompleted
				&& comboStatus.SelectedIndex==1 //making it complete
				&& AptCur.AptDateTime.Date > DateTime.Today)//and future appt
			{
				MsgBox.Show(this,"Not allowed to set future appointments complete.");
				return false;
			}
			bool hasProcsAttached=gridProc.SelectedIndices
				//Get tags on rows as procedures if possible
				.Select(x=>gridProc.Rows[x].Tag as Procedure)
				//true if any row had a valid procedure as a tag
				.Any(x=>x!=null);
			if(!PrefC.GetBool(PrefName.ApptAllowEmptyComplete)
				&& AptCur.AptStatus!=ApptStatus.Complete//was not originally complete
				&& AptCur.AptStatus!=ApptStatus.PtNote
				&& AptCur.AptStatus!=ApptStatus.PtNoteCompleted
				&& comboStatus.SelectedIndex==1)//making it complete
			{				
				if(!hasProcsAttached) {
					MsgBox.Show(this,"Appointments without procedures attached can not be set complete.");
					return false;
				}
			}
			#region Security checks
			if(AptCur.AptStatus!=ApptStatus.Complete//was not originally complete
				&& _selectedApptStatus==ApptStatus.Complete //trying to make it complete
				&& hasProcsAttached
				&& !Security.IsAuthorized(Permissions.ProcComplCreate,AptCur.AptDateTime))//aren't authorized to complete procedures
			{
				return false;
			}
			#endregion
			if(comboStatus.SelectedIndex==1 && AptCur.AptDateTime > DateTime.Today.Date && !PrefC.GetBool(PrefName.FutureTransDatesAllowed)) {
				MsgBox.Show(this,"Not allowed to set procedures complete with future dates.");
				return false;
			}
			#endregion Validate Form Data
			//-----Point of no return-----
			#region Broken appt selections
			if(_formApptBreakSelection==ApptBreakSelection.Unsched && !AppointmentL.ValidateApptUnsched(AptCur)) {
				_formApptBreakSelection=ApptBreakSelection.None;//This way no additional logic runs below.
			}
			if(_formApptBreakSelection==ApptBreakSelection.Pinboard && !AppointmentL.ValidateApptToPinboard(AptCur)) {
				_formApptBreakSelection=ApptBreakSelection.None;//This way no additional logic runs below.
			}
			#endregion
			#region Set AptCur Fields
			AptCur.Pattern=Appointments.ConvertPatternTo5(strBTime.ToString());
			//Only run appt overlap check if editing an appt not in unscheduled list and in chart module and eCW program link not enabled.
			//Also need to see if there is a generic HL7 def enabled where Open Dental is not the filler application.
			//Open Dental is the filler application if appointments, schedules, and operatories are maintained by Open Dental and messages are sent out
			//to inform another software of any changes made.  If Open Dental is an auxiliary application, appointments are created from inbound SIU
			//messages and Open Dental no longer has control over whether the appointments overlap or which operatory/provider's schedule the appointment
			//belongs to.  In this case, we do not want to check for overlapping appointments and the appointment module should be hidden.
			HL7Def hl7DefEnabled=HL7Defs.GetOneDeepEnabled();//the ShowAppts check box is hidden for MedLab HL7 interfaces, so only need to check the others
			bool isAuxiliaryRole=false;
			if(hl7DefEnabled!=null && !hl7DefEnabled.ShowAppts) {//if the appts module is hidden
				//if an inbound SIU message is defined, OD is the auxiliary application which neither exerts control over nor requests changes to a schedule
				isAuxiliaryRole=hl7DefEnabled.hl7DefMessages.Any(x => x.MessageType==MessageTypeHL7.SIU && x.InOrOut==InOutHL7.Incoming);
			}
			if((IsInChartModule || IsInViewPatAppts)
				&& !Programs.UsingEcwTightOrFullMode()//if eCW Tight or Full mode, appts created from inbound SIU messages and appt module always hidden
				&& AptCur.AptStatus!=ApptStatus.UnschedList
				&& !isAuxiliaryRole)//generic HL7 def enabled, appt module hidden and an inbound SIU msg defined, appts created from msgs so no overlap check
			{
				//Adjusts AptCur.Pattern directly when necessary.
				if(ContrAppt.TryAdjustAppointmentPattern(AptCur)) {
					MsgBox.Show(this,"Appointment is too long and would overlap another appointment.  Automatically shortened to fit.");
				}
			}
			AptCur.Priority=checkASAP.Checked ? ApptPriority.ASAP : ApptPriority.Normal;
			AptCur.AptStatus=_selectedApptStatus;
			//set procs complete was moved further down
			if(comboUnschedStatus.SelectedIndex==0){//none
				AptCur.UnschedStatus=0;
			}
			else{
				AptCur.UnschedStatus=_listRecallUnschedStatusDefs[comboUnschedStatus.SelectedIndex-1].DefNum;
			}
			if(comboConfirmed.SelectedIndex!=-1){
				AptCur.Confirmed=_listApptConfirmedDefs[comboConfirmed.SelectedIndex].DefNum;
			}
			AptCur.TimeLocked=checkTimeLocked.Checked;
			AptCur.ColorOverride=butColor.BackColor;
			AptCur.Note=textNote.Text;
			AptCur.ClinicNum=_selectedClinicNum;
			AptCur.ProvNum=_selectedProvNum;
			AptCur.ProvHyg=_selectedProvHygNum;
			AptCur.IsHygiene=checkIsHygiene.Checked;
			if(comboAssistant.SelectedIndex==0) {//none
				AptCur.Assistant=0;
			}
			else {
				AptCur.Assistant=_listEmployees[comboAssistant.SelectedIndex-1].EmployeeNum;
			}
			AptCur.IsNewPatient=checkIsNewPatient.Checked;
			AptCur.DateTimeAskedToArrive=dateTimeAskedToArrive;
			AptCur.DateTimeArrived=dateTimeArrived;
			AptCur.DateTimeSeated=dateTimeSeated;
			AptCur.DateTimeDismissed=dateTimeDismissed;
			//AptCur.InsPlan1 and InsPlan2 already handled 
			if(comboApptType.SelectedIndex==0) {//0 index = none.
				AptCur.AppointmentTypeNum=0;
			}
			else {
				AptCur.AppointmentTypeNum=_listAppointmentType[comboApptType.SelectedIndex-1].AppointmentTypeNum;
			}
			#endregion Set AptCur Fields
			#region Update ProcDescript for Appt
			//Use the current selections to set AptCur.ProcDescript.
			List<Procedure> listGridSelectedProcs=new List<Procedure>();
			gridProc.SelectedIndices.ToList().ForEach(x => listGridSelectedProcs.Add(_listProcsForAppt[x].Copy()));
			foreach(Procedure proc in listGridSelectedProcs) {
				//This allows Appointments.SetProcDescript(...) to associate all the passed in procs into AptCur.ProcDescript
				//listGridSelectedProcs is only used here and contains copies of procs.
				proc.AptNum=AptCur.AptNum;
				proc.PlannedAptNum=AptCur.AptNum;
			}
			Appointments.SetProcDescript(AptCur,listGridSelectedProcs);
			#endregion Update ProcDescript for Appt
			#region Provider change and fee change check
			//Determins if we would like to update ProcFees when a provider changes, considers PrefName.ProcFeeUpdatePrompt.
			InsPlan aptInsPlan1=InsPlans.GetPlan(AptCur.InsPlan1,PlanList);//we only care about lining the fees up with the primary insurance plan
			bool updateProcFees=false;
			if(AptCur.AptStatus!=ApptStatus.Complete && (_selectedProvNum!=AptOld.ProvNum || _selectedProvHygNum!=AptOld.ProvHyg)) {//Either the primary or hygienist changed.
				List<Procedure> listNewProcs=gridProc.SelectedIndices.Select(x => Procedures.UpdateProcInAppointment(AptCur,((Procedure)gridProc.Rows[x].Tag).Copy())).ToList();
				List<Procedure> listOldProcs=gridProc.SelectedIndices.Select(x => ((Procedure)gridProc.Rows[x].Tag).Copy()).ToList();
				string promptText="";
				updateProcFees=Procedures.FeeUpdatePromptHelper(listNewProcs,listOldProcs,aptInsPlan1,ref promptText);
				if(updateProcFees && promptText!="" && !MsgBox.Show(this,MsgBoxButtons.YesNo,promptText)) {
					updateProcFees=false;
				}
			}
			bool removeCompleteProcs=ProcedureL.DoRemoveCompletedProcs(AptCur,_listProcsForAppt.FindAll(x => x.AptNum==AptCur.AptNum),true);
			#endregion
			#region Save to DB
			Appointments.ApptSaveHelperResult result;
			try {
				result=Appointments.ApptSaveHelper(AptCur,AptOld,_isInsertRequired,_listProcsForAppt,_listAppointments,
					gridProc.SelectedIndices.ToList(),_listProcNumsAttachedStart,_isPlanned,PlanList,SubList,_selectedProvNum,_selectedProvHygNum,
					listGridSelectedProcs,IsNew,pat,fam,updateProcFees,removeCompleteProcs,doCreateSecLog,doInsertHL7);
				AptCur=result.AptCur;
				_listProcsForAppt=result.ListProcsForAppt;
				_listAppointments=result.ListAppts;
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
				return false;
			}			
			if(_isInsertRequired && AptOld.AptNum==0) {
				//Update the the old AptNum since this is a new appointment.
				//This stops Appointments.Sync(...) from double insertings this new appointment.
				AptOld.AptNum=AptCur.AptNum;
				_listAppointmentsOld.FirstOrDefault(x => x.AptNum==0).AptNum=AptCur.AptNum;
			}
			_isInsertRequired=false;//Now that we have inserted the new appointment, let typical appointment logic handle from here on.
			#endregion Save changes to DB
			#region Update gridProc tags
			//update tags with changes made so that anyone accessing it later has an updated copy.
			foreach(int index in gridProc.SelectedIndices) {
				Procedure procNew=_listProcsForAppt.FirstOrDefault(x => x.ProcNum==((Procedure)gridProc.Rows[index].Tag).ProcNum);
				if(procNew==null) {
					continue;
				}
				gridProc.Rows[index].Tag=procNew.Copy();
			}
			#endregion
			#region Automation and Broken Appt Logic
			if(result.DoRunAutomation) {
				AutomationL.Trigger(AutomationTrigger.CompleteProcedure,_listProcsForAppt.FindAll(x => x.AptNum==AptCur.AptNum)
					.Select(x => ProcedureCodes.GetStringProcCode(x.CodeNum)).ToList(),AptCur.PatNum);
			}
			//Do the appointment "break" automation for appointments that were just broken.
			if(AptCur.AptStatus==ApptStatus.Broken && AptOld.AptStatus!=ApptStatus.Broken) {
				AppointmentL.BreakApptHelper(AptCur,pat,_procCodeBroken);
				if(isClosing) {
					switch(_formApptBreakSelection) {//ApptBreakSelection.None by default.
						case ApptBreakSelection.Unsched:
							AppointmentL.SetApptUnschedHelper(AptCur,pat);
							break;
						case ApptBreakSelection.Pinboard:
							AppointmentL.CopyAptToPinboardHelper(AptCur);
							break;
						case ApptBreakSelection.None://User did not makes selection
						case ApptBreakSelection.ApptBook://User made selection, no extra logic required.
							break;
					}
				}
			}
			#endregion Broken Appt Logic
			return true;
		}
		
		private void butPDF_Click(object sender,EventArgs e) {
			if(_isInsertRequired) {
				MsgBox.Show(this,"Please click OK to create this appointment before taking this action.");
				return;
			}
			//this will only happen for eCW HL7 interface users.
			List<Procedure> listProcsForAppt=Procedures.GetProcsForSingle(AptCur.AptNum,AptCur.AptStatus==ApptStatus.Planned);
			string duplicateProcs=ProcedureL.ProcsContainDuplicates(listProcsForAppt);
			if(duplicateProcs!="") {
				MessageBox.Show(duplicateProcs);
				return;
			}
			//Send DFT to eCW containing a dummy procedure with this appointment in a .pdf file.	
			//no security
			string pdfDataStr=GenerateProceduresIntoPdf();
			if(HL7Defs.IsExistingHL7Enabled()) {
				//PDF messages do not contain FT1 segments, so proc list can be empty
				//MessageHL7 messageHL7=MessageConstructor.GenerateDFT(procs,EventTypeHL7.P03,pat,Patients.GetPat(pat.Guarantor),AptCur.AptNum,"progressnotes",pdfDataStr);
				MessageHL7 messageHL7=MessageConstructor.GenerateDFT(new List<Procedure>(),EventTypeHL7.P03,pat,Patients.GetPat(pat.Guarantor),AptCur.AptNum,"progressnotes",pdfDataStr);
				if(messageHL7==null) {
					MsgBox.Show(this,"There is no DFT message type defined for the enabled HL7 definition.");
					return;
				}
				HL7Msg hl7Msg=new HL7Msg();
				//hl7Msg.AptNum=AptCur.AptNum;
				hl7Msg.AptNum=0;//Prevents the appt complete button from changing to the "Revise" button prematurely.
				hl7Msg.HL7Status=HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
				hl7Msg.MsgText=messageHL7.ToString();
				hl7Msg.PatNum=pat.PatNum;
				HL7Msgs.Insert(hl7Msg);
#if DEBUG
				MessageBox.Show(this,messageHL7.ToString());
#endif
			}
			else {
				//Note: AptCur.ProvNum may not reflect the selected provider in comboProv. This is still the Provider that the appointment was last saved with.
				Bridges.ECW.SendHL7(AptCur.AptNum,AptCur.ProvNum,pat,pdfDataStr,"progressnotes",true,null);//just pdf, passing null proc list
			}
			MsgBox.Show(this,"Notes PDF sent.");
		}

		///<summary>Creates a new .pdf file containing all of the procedures attached to this appointment and 
		///returns the contents of the .pdf file as a base64 encoded string.</summary>
		private string GenerateProceduresIntoPdf(){
			MigraDoc.DocumentObjectModel.Document doc=new MigraDoc.DocumentObjectModel.Document();
			doc.DefaultPageSetup.PageWidth=Unit.FromInch(8.5);
			doc.DefaultPageSetup.PageHeight=Unit.FromInch(11);
			doc.DefaultPageSetup.TopMargin=Unit.FromInch(.5);
			doc.DefaultPageSetup.LeftMargin=Unit.FromInch(.5);
			doc.DefaultPageSetup.RightMargin=Unit.FromInch(.5);
			MigraDoc.DocumentObjectModel.Section section=doc.AddSection();
			MigraDoc.DocumentObjectModel.Font headingFont=MigraDocHelper.CreateFont(13,true);
			MigraDoc.DocumentObjectModel.Font bodyFontx=MigraDocHelper.CreateFont(9,false);
			string text;
			//Heading---------------------------------------------------------------------------------------------------------------
			#region printHeading
			Paragraph par=section.AddParagraph();
			ParagraphFormat parformat=new ParagraphFormat();
			parformat.Alignment=ParagraphAlignment.Center;
			parformat.Font=MigraDocHelper.CreateFont(10,true);
			par.Format=parformat;
			text=Lan.g(this,"procedures").ToUpper();
			par.AddFormattedText(text,headingFont);
			par.AddLineBreak();
			text=pat.GetNameFLFormal();
			par.AddFormattedText(text,headingFont);
			par.AddLineBreak();
			text=DateTime.Now.ToShortDateString();
			par.AddFormattedText(text,headingFont);
			par.AddLineBreak();
			par.AddLineBreak();
			#endregion
			//Procedure List--------------------------------------------------------------------------------------------------------
			#region Procedure List
			ODGrid gridProg=new ODGrid();
			this.Controls.Add(gridProg);//Only added temporarily so that printing will work. Removed at end with Dispose().
			gridProg.BeginUpdate();
			gridProg.Columns.Clear();
			ODGridColumn col;
			List<DisplayField> fields=DisplayFields.GetDefaultList(DisplayFieldCategory.None);
			for(int i=0;i<fields.Count;i++){
				if(fields[i].InternalName=="User" || fields[i].InternalName=="Signed"){
					continue;
				}
				if(fields[i].Description==""){
					col=new ODGridColumn(fields[i].InternalName,fields[i].ColumnWidth);
				}
				else{
					col=new ODGridColumn(fields[i].Description,fields[i].ColumnWidth);
				}
				if(fields[i].InternalName=="Amount"){
					col.TextAlign=HorizontalAlignment.Right;
				}
				if(fields[i].InternalName=="Proc Code")
				{
					col.TextAlign=HorizontalAlignment.Center;
				}
				gridProg.Columns.Add(col);
			}
			gridProg.NoteSpanStart=2;
			gridProg.NoteSpanStop=7;
			gridProg.Rows.Clear();
			List<Procedure> procsForDay=Procedures.GetProcsForPatByDate(AptCur.PatNum,AptCur.AptDateTime);
			List<Def> listProgNoteColorDefs=Defs.GetDefsForCategory(DefCat.ProgNoteColors);
			List<Def> listMiscColorDefs=Defs.GetDefsForCategory(DefCat.MiscColors);
			for(int i=0;i<procsForDay.Count;i++){
				Procedure proc=procsForDay[i];
				ProcedureCode procCode=ProcedureCodes.GetProcCode(proc.CodeNum);
				Provider prov=_listProvidersAll.First(x => x.ProvNum==proc.ProvNum);
				Userod usr=Userods.GetUser(proc.UserNum);
				ODGridRow row=new ODGridRow();
				row.ColorLborder=System.Drawing.Color.Black;
				for(int f=0;f<fields.Count;f++) {
					switch(fields[f].InternalName){
						case "Date":
							row.Cells.Add(proc.ProcDate.Date.ToShortDateString());
							break;
						case "Time":
							row.Cells.Add(proc.ProcDate.ToString("h:mm")+proc.ProcDate.ToString("%t").ToLower());
							break;
						case "Th":
							row.Cells.Add(proc.ToothNum);
							break;
						case "Surf":
							row.Cells.Add(proc.Surf);
							break;
						case "Dx":
							row.Cells.Add(proc.Dx.ToString());
							break;
						case "Description":
							row.Cells.Add((procCode.LaymanTerm!="")?procCode.LaymanTerm:procCode.Descript);
							break;
						case "Stat":
							row.Cells.Add(Lans.g("enumProcStat",proc.ProcStatus.ToString()));
							break;
						case "Prov":
							row.Cells.Add(prov.Abbr.Left(5));
							break;
						case "Amount":
							row.Cells.Add(proc.ProcFee.ToString("F"));
							break;
						case "Proc Code":
							if(procCode.ProcCode.Length>5 && procCode.ProcCode.StartsWith("D")) {
								row.Cells.Add(procCode.ProcCode.Substring(0,5));//Remove suffix from all D codes.
							}
							else {
								row.Cells.Add(procCode.ProcCode);
							}
							break;
						case "User":
							row.Cells.Add(usr!=null?usr.UserName:"");
						  break;
					}
				}
				row.Note=proc.Note;
				//Row text color.
				switch(proc.ProcStatus) {
					case ProcStat.TP:
						row.ColorText=listProgNoteColorDefs[0].ItemColor;
						break;
					case ProcStat.C:
						row.ColorText=listProgNoteColorDefs[1].ItemColor;
						break;
					case ProcStat.EC:
						row.ColorText=listProgNoteColorDefs[2].ItemColor;
						break;
					case ProcStat.EO:
						row.ColorText=listProgNoteColorDefs[3].ItemColor;
						break;
					case ProcStat.R:
						row.ColorText=listProgNoteColorDefs[4].ItemColor;
						break;
					case ProcStat.D:
						row.ColorText=System.Drawing.Color.Black;
						break;
					case ProcStat.Cn:
						row.ColorText=listProgNoteColorDefs[22].ItemColor;
						break;
				}
				row.ColorBackG=System.Drawing.Color.White;
				if(proc.ProcDate.Date==DateTime.Today) {
					row.ColorBackG=listMiscColorDefs[6].ItemColor;
				}				
				gridProg.Rows.Add(row);
			}
			MigraDocHelper.DrawGrid(section,gridProg);
			#endregion		
			MigraDoc.Rendering.PdfDocumentRenderer pdfRenderer=new MigraDoc.Rendering.PdfDocumentRenderer(true,PdfFontEmbedding.Always);
			pdfRenderer.Document=doc;
			pdfRenderer.RenderDocument();
			MemoryStream ms=new MemoryStream();
			pdfRenderer.PdfDocument.Save(ms);
			byte[] pdfBytes=ms.GetBuffer();
			//#region Remove when testing is complete.
			//string tempFilePath=Path.GetTempFileName();
			//File.WriteAllBytes(tempFilePath,pdfBytes);
			//#endregion
			string pdfDataStr=Convert.ToBase64String(pdfBytes);
			ms.Dispose();
			return pdfDataStr;
		}

		private void butComplete_Click(object sender,EventArgs e) {
			//It is OK to let the user click the OK button as long as AptCur.AptNum is NOT used prior to UpdateListAndDB().
			//if(_isInsertRequired) {
			//	MsgBox.Show(this,"Please click OK to create this appointment before taking this action.");
			//	return;
			//}
			//This is only used with eCW HL7 interface.
			DateTime datePrevious=AptCur.DateTStamp;
			if(_isEcwHL7Sent) {
				if(!Security.IsAuthorized(Permissions.EcwAppointmentRevise)) {
					return;
				}
				MsgBox.Show(this,"Any changes that you make will not be sent to eCW.  You will also have to make the same changes in eCW.");
				//revise is only clickable if user has permission
				butOK.Enabled=true;
				gridProc.Enabled=true;
				listQuickAdd.Enabled=true;
				butAdd.Enabled=true;
				butDeleteProc.Enabled=true;
				return;
			}
			List<Procedure> listProcsForAppt=gridProc.SelectedIndices.OfType<int>().Select(x => (Procedure)gridProc.Rows[x].Tag).ToList();
			string duplicateProcs=ProcedureL.ProcsContainDuplicates(listProcsForAppt);
			if(duplicateProcs!="") {
				MessageBox.Show(duplicateProcs);
				return;
			}
			if(ProgramProperties.GetPropVal(ProgramName.eClinicalWorks,"ProcNotesNoIncomplete")=="1") {
				if(listProcsForAppt.Any(x => x.Note!=null && x.Note.Contains("\"\""))) {
					MsgBox.Show(this,"This appointment cannot be sent because there are incomplete procedure notes.");
					return;
				}
			}
			if(ProgramProperties.GetPropVal(ProgramName.eClinicalWorks,"ProcRequireSignature")=="1") {
				if(listProcsForAppt.Any(x => !string.IsNullOrEmpty(x.Note) && string.IsNullOrEmpty(x.Signature))) {
					MsgBox.Show(this,"This appointment cannot be sent because there are unsigned procedure notes.");
					return;
				}
			}
			//user can only get this far if aptNum matches visit num previously passed in by eCW.
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Send attached procedures to eClinicalWorks and exit?")) {
				return;
			}
			comboStatus.SelectedIndex=1;//Set the appointment status to complete. This will trigger the procedures to be completed in UpdateToDB() as well.
			if(!UpdateListAndDB()) {
				return;
			}
			listProcsForAppt=Procedures.GetProcsForSingle(AptCur.AptNum,AptCur.AptStatus==ApptStatus.Planned);
			//Send DFT to eCW containing the attached procedures for this appointment in a .pdf file.				
			string pdfDataStr=GenerateProceduresIntoPdf();
			if(HL7Defs.IsExistingHL7Enabled()) {
				//MessageConstructor.GenerateDFT(procs,EventTypeHL7.P03,pat,Patients.GetPat(pat.Guarantor),AptCur.AptNum,"progressnotes",pdfDataStr);
				MessageHL7 messageHL7=MessageConstructor.GenerateDFT(listProcsForAppt,EventTypeHL7.P03,pat,Patients.GetPat(pat.Guarantor),AptCur.AptNum,
					"progressnotes",pdfDataStr);
				if(messageHL7==null) {
					MsgBox.Show(this,"There is no DFT message type defined for the enabled HL7 definition.");
					return;
				}
				HL7Msg hl7Msg=new HL7Msg();
				hl7Msg.AptNum=AptCur.AptNum;
				hl7Msg.HL7Status=HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
				hl7Msg.MsgText=messageHL7.ToString();
				hl7Msg.PatNum=pat.PatNum;
				HL7ProcAttach hl7ProcAttach=new HL7ProcAttach();
				hl7ProcAttach.HL7MsgNum=HL7Msgs.Insert(hl7Msg);
				foreach(Procedure proc in listProcsForAppt) {
					hl7ProcAttach.ProcNum=proc.ProcNum;
					HL7ProcAttaches.Insert(hl7ProcAttach);
				}
			}
			else {
				Bridges.ECW.SendHL7(AptCur.AptNum,AptCur.ProvNum,pat,pdfDataStr,"progressnotes",false,listProcsForAppt);
			}
			CloseOD=true;
			if(IsNew) {
				SecurityLogs.MakeLogEntry(Permissions.AppointmentCreate,pat.PatNum,
				AptCur.AptDateTime.ToString()+", "+AptCur.ProcDescript,
				AptCur.AptNum,datePrevious);
			}
			DialogResult=DialogResult.OK;
			if(!this.Modal) {
				Close();
			}
		}

		private void butAudit_Click(object sender,EventArgs e) {
			if(_isInsertRequired) {
				MsgBox.Show(this,"Please click OK to create this appointment before taking this action.");
				return;
			}
			List<Permissions> perms=new List<Permissions>();
			perms.Add(Permissions.AppointmentCreate);
			perms.Add(Permissions.AppointmentEdit);
			perms.Add(Permissions.AppointmentMove);
			perms.Add(Permissions.AppointmentCompleteEdit);
			perms.Add(Permissions.ApptConfirmStatusEdit);
			FormAuditOneType FormA=new FormAuditOneType(pat.PatNum,perms,Lan.g(this,"Audit Trail for Appointment"),AptCur.AptNum);
			FormA.ShowDialog();
		}

		private void butTask_Click(object sender,EventArgs e) {
			if(_isInsertRequired && !UpdateListAndDB(false)) {
				return;
			}
			FormTaskListSelect FormT=new FormTaskListSelect(TaskObjectType.Appointment);//,AptCur.AptNum);
			FormT.Text=Lan.g(FormT,"Add Task")+" - "+FormT.Text;
			FormT.ShowDialog();
			if(FormT.DialogResult!=DialogResult.OK) {
				return;
			}
			Task task=new Task();
			task.TaskListNum=-1;//don't show it in any list yet.
			Tasks.Insert(task);
			Task taskOld=task.Copy();
			task.KeyNum=AptCur.AptNum;
			task.ObjectType=TaskObjectType.Appointment;
			task.TaskListNum=FormT.ListSelectedLists[0];
			task.UserNum=Security.CurUser.UserNum;
			FormTaskEdit FormTE=new FormTaskEdit(task,taskOld);
			FormTE.IsNew=true;
			FormTE.ShowDialog();
		}

		private void butPin_Click(object sender,System.EventArgs e) {
			if(AptCur.AptStatus.In(ApptStatus.UnschedList,ApptStatus.Planned)
				&& pat.PatStatus.In(PatientStatus.Archived,PatientStatus.Deceased)) 
			{
				MsgBox.Show(this,"Appointments cannot be scheduled for "+pat.PatStatus.ToString().ToLower()+" patients.");
				return;
			}
			if(!UpdateListAndDB()) {
				return;
			}
			PinClicked=true;
			DialogResult=DialogResult.OK;
			if(!this.Modal) {
				Close();
			}
		}

		///<summary>Returns true if the appointment type was successfully changed, returns false if the user decided to cancel out of doing so.</summary>
		private bool AptTypeHelper() {
			if(comboApptType.SelectedIndex==0) {//'None' is selected so maintain grid selections.
				return true;
			}
			if(AptCur.AptStatus.In(ApptStatus.PtNote,ApptStatus.PtNoteCompleted)) {
				return true;//Patient notes can't have procedures associated to them.
			}
			AppointmentType aptTypeCur=_listAppointmentType[comboApptType.SelectedIndex-1];
			List<ProcedureCode> listAptTypeProcs=ProcedureCodes.GetFromCommaDelimitedList(aptTypeCur.CodeStr);
			if(listAptTypeProcs.Count>0) {//AppointmentType is associated to procs.
				List<Procedure> listSelectedProcs=gridProc.Rows.Cast<ODGridRow>()
					.Where(x => gridProc.SelectedIndices.Contains(gridProc.Rows.IndexOf(x)))
					.Select(x => ((Procedure)x.Tag)).ToList();
				List<long> listProcCodeNumsToDetach=listSelectedProcs.Select(y => y.CodeNum).ToList()
				.Except(listAptTypeProcs.Select(x => x.CodeNum).ToList()).ToList();
				//if there are procedures that would get detached
				//and if they have the preference AppointmentTypeWarning on,
				//Display the warning
				if(listProcCodeNumsToDetach.Count>0 && PrefC.GetBool(PrefName.AppointmentTypeShowWarning)) {
					if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Selecting this appointment type will dissociate the current procedures from this "
						+"appointment and attach the procedures defined for this appointment type.  Do you want to continue?")) {
						return false;
					}
				}
				Appointments.ApptTypeMissingProcHelper(AptCur,aptTypeCur,_listProcsForAppt,pat,true,_listPatPlans,SubList,PlanList,_benefitList);
				FillProcedures();
				//Since we have detached and attached all pertinent procs by this point it is safe to just use the PlannedAptNum or AptNum.
				gridProc.SetSelected(false);
				foreach(ProcedureCode procCodeCur in listAptTypeProcs) {
					for(int i=0;i<gridProc.Rows.Count;i++) {
						Procedure rowProc=(Procedure)gridProc.Rows[i].Tag;
						if(rowProc.CodeNum==procCodeCur.CodeNum
							//if the procedure code already exists in the grid and it's not attached to another appointment or planned appointment
							&& (_isPlanned && (rowProc.PlannedAptNum==0 || rowProc.PlannedAptNum==AptCur.AptNum)
								|| (!_isPlanned && (rowProc.AptNum==0 || rowProc.AptNum==AptCur.AptNum)))
							//The row is not already selected. This is necessary so that Apt Types with two of the same procs will select both procs.
							&& !gridProc.SelectedIndices.Contains(i)) 
						{
							gridProc.SetSelected(i,true); //set procedures selected in the grid.
							break;
						}
					}
				}
			}
			butColor.BackColor=aptTypeCur.AppointmentTypeColor;
			if(aptTypeCur.Pattern!=null && aptTypeCur.Pattern!="") {
				strBTime=new StringBuilder();
				for(int i = 0;i<aptTypeCur.Pattern.Length;i++) {
					strBTime.Append(aptTypeCur.Pattern.Substring(i,1));
					if(PrefC.GetLong(PrefName.AppointmentTimeIncrement)==10) {
						i++;
					}
					else if(PrefC.GetLong(PrefName.AppointmentTimeIncrement)==15) {
						i++;
						i++;
					}
				}
			}
			//calculate the new time pattern.
			if(aptTypeCur!=null && listAptTypeProcs != null) {
				//Has Procs, but not time.
				if(aptTypeCur.Pattern=="" && listAptTypeProcs.Count > 0) {
					//Calculate and Fill
					CalculateTime(true);
					FillTime();
				}
				//Has fixed time
				else if(aptTypeCur.Pattern!="") {
					FillTime();
				}
				//No Procs, No time.
				else {
					//do nothing to the time pattern
				}
			}
			return true;
		}

		///<summary>Only catches user changes, not programatic changes. For instance this does not fire when loading the form.</summary>
		private void comboApptType_SelectionChangeCommitted(object sender,EventArgs e) {
			if(!AptTypeHelper()) {
				comboApptType.SelectedIndex=_aptTypeIndex;
				return;
			}
			_aptTypeIndex=comboApptType.SelectedIndex;
		}

		private void comboConfirmed_SelectionChangeCommitted(object sender,EventArgs e) {
			if(PrefC.GetLong(PrefName.AppointmentTimeArrivedTrigger)!=0 //Using appointmentTimeArrivedTrigger preference
				&& _listApptConfirmedDefs[comboConfirmed.SelectedIndex].DefNum==PrefC.GetLong(PrefName.AppointmentTimeArrivedTrigger) //selected index matches pref
				&& String.IsNullOrWhiteSpace(textTimeArrived.Text))//time not already set 
			{
				textTimeArrived.Text=DateTime.Now.ToShortTimeString();
			}
			if(PrefC.GetLong(PrefName.AppointmentTimeSeatedTrigger)!=0 //Using AppointmentTimeSeatedTrigger preference
				&& _listApptConfirmedDefs[comboConfirmed.SelectedIndex].DefNum==PrefC.GetLong(PrefName.AppointmentTimeSeatedTrigger) //selected index matches pref
				&& String.IsNullOrWhiteSpace(textTimeSeated.Text))//time not already set 
			{
				textTimeSeated.Text=DateTime.Now.ToShortTimeString();
			}
			if(PrefC.GetLong(PrefName.AppointmentTimeDismissedTrigger)!=0 //Using AppointmentTimeDismissedTrigger preference
				&& _listApptConfirmedDefs[comboConfirmed.SelectedIndex].DefNum==PrefC.GetLong(PrefName.AppointmentTimeDismissedTrigger) //selected index matches pref
				&& String.IsNullOrWhiteSpace(textTimeDismissed.Text))//time not already set 
			{
				textTimeDismissed.Text=DateTime.Now.ToShortTimeString();
			}
		}

		private void comboStatus_SelectionChangeCommitted(object sender,EventArgs e) {
			if(((ComboBox)sender).SelectedIndex!=indexStatusBroken) {
				return;
			}
			if(!AppointmentL.HasBrokenApptProcs()) {
				return;
			}
			FormApptBreak formAB=new FormApptBreak(AptCur);
			if(formAB.ShowDialog()!=DialogResult.OK) {
				comboStatus.SelectedIndex=(int)AptCur.AptStatus-1;//Sets status back to on load selection.
				_formApptBreakSelection=ApptBreakSelection.None;
				_procCodeBroken=null;
				return;
			}
			_formApptBreakSelection=formAB.FormApptBreakSelection;
			_procCodeBroken=formAB.SelectedProcCode;
		}

		private void checkASAP_CheckedChanged(object sender,EventArgs e) {
			if(checkASAP.Checked) {
				checkASAP.ForeColor=System.Drawing.Color.Red;
			}
			else {
				checkASAP.ForeColor=SystemColors.ControlText;
			}
		}

		private bool CheckFrequencies() {
			List<Procedure> listProcsForFrequency=new List<Procedure>();
			foreach(int index in gridProc.SelectedIndices) {
				Procedure proc=((Procedure)gridProc.Rows[index].Tag).Copy();
				if(proc.ProcStatus==ProcStat.TP) {
					listProcsForFrequency.Add(proc);
				}
			}
			if(listProcsForFrequency.Count>0) {
				string frequencyConflicts="";
				try {
					frequencyConflicts=Procedures.CheckFrequency(listProcsForFrequency,pat.PatNum,AptCur.AptDateTime);
				}
				catch(Exception e) {
					MessageBox.Show(Lan.g(this,"There was an error checking frequencies."
						+"  Disable the Insurance Frequency Checking feature or try to fix the following error:")
						+"\r\n"+e.Message);
					return false;
				}
				if(frequencyConflicts!="" && MessageBox.Show(Lan.g(this,"This appointment will cause frequency conflicts for the following procedures")
					+":\r\n"+frequencyConflicts+"\r\n"+Lan.g(this,"Do you want to continue?"),"",MessageBoxButtons.YesNo)==DialogResult.No)
				{
					return false;
				}
			}
			return true;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			DateTime datePrevious=AptCur.DateTStamp;
			if (AptCur.AptStatus == ApptStatus.PtNote || AptCur.AptStatus == ApptStatus.PtNoteCompleted) {
				if (!MsgBox.Show(this, true, "Delete Patient Note?")) {
					return;
				}
				if(textNote.Text != "") {
					if(MessageBox.Show(Commlogs.GetDeleteApptCommlogMessage(textNote.Text,AptCur.AptStatus),"Question...",MessageBoxButtons.YesNo) == DialogResult.Yes) {
						Commlog CommlogCur = new Commlog();
						CommlogCur.PatNum = AptCur.PatNum;
						CommlogCur.CommDateTime = DateTime.Now;
						CommlogCur.CommType = Commlogs.GetTypeAuto(CommItemTypeAuto.APPT);
						CommlogCur.Note = "Deleted Pt NOTE from schedule, saved copy: ";
						CommlogCur.Note += textNote.Text;
						CommlogCur.UserNum=Security.CurUser.UserNum;
						//there is no dialog here because it is just a simple entry
						Commlogs.Insert(CommlogCur);
					}
				}
			}
			else {//ordinary appointment
				if (MessageBox.Show(Lan.g(this, "Delete appointment?"), "", MessageBoxButtons.OKCancel) != DialogResult.OK) {
					return;
				}
				if(textNote.Text != "") {
					if(MessageBox.Show(Commlogs.GetDeleteApptCommlogMessage(textNote.Text,AptCur.AptStatus),"Question...",MessageBoxButtons.YesNo) == DialogResult.Yes) {
						Commlog CommlogCur = new Commlog();
						CommlogCur.PatNum = AptCur.PatNum;
						CommlogCur.CommDateTime = DateTime.Now;
						CommlogCur.CommType = Commlogs.GetTypeAuto(CommItemTypeAuto.APPT);
						CommlogCur.Note = "Deleted Appt. & saved note: ";
						if(AptCur.ProcDescript != "") {
							CommlogCur.Note += AptCur.ProcDescript + ": ";
						}
						CommlogCur.Note += textNote.Text;
						CommlogCur.UserNum=Security.CurUser.UserNum;
						//there is no dialog here because it is just a simple entry
						Commlogs.Insert(CommlogCur);
					}
				}
				//If there is an existing HL7 def enabled, send a SIU message if there is an outbound SIU message defined
				if(HL7Defs.IsExistingHL7Enabled()) {
					//S17 - Appt Deletion event
					MessageHL7 messageHL7=MessageConstructor.GenerateSIU(pat,fam.GetPatient(pat.Guarantor),EventTypeHL7.S17,AptCur);
					//Will be null if there is no outbound SIU message defined, so do nothing
					if(messageHL7!=null) {
						HL7Msg hl7Msg=new HL7Msg();
						hl7Msg.AptNum=AptCur.AptNum;
						hl7Msg.HL7Status=HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
						hl7Msg.MsgText=messageHL7.ToString();
						hl7Msg.PatNum=pat.PatNum;
						HL7Msgs.Insert(hl7Msg);
#if DEBUG
						MessageBox.Show(this,messageHL7.ToString());
#endif
					}
				}
			}
			_listAppointments.RemoveAll(x => x.AptNum==AptCur.AptNum);
			if(AptOld.AptStatus!=ApptStatus.Complete) { //seperate log entry for completed appointments
				SecurityLogs.MakeLogEntry(Permissions.AppointmentEdit,pat.PatNum,
					"Delete for date/time: "+AptCur.AptDateTime.ToString(),
					AptCur.AptNum,datePrevious);
			}
			else {
				SecurityLogs.MakeLogEntry(Permissions.AppointmentCompleteEdit,pat.PatNum,
					"Delete for date/time: "+AptCur.AptDateTime.ToString(),
					AptCur.AptNum,datePrevious);
			}
			if(IsNew) {
				DialogResult=DialogResult.Cancel;
				if(!this.Modal) {
					Close();
				}
			}
			else {
				DialogResult=DialogResult.OK;
				_isDeleted=true;
				if(!this.Modal) {
					Close();
				}
			}
			Plugins.HookAddCode(this,"FormApptEdit.butDelete_Click_end",AptCur);
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			DateTime datePrevious=AptCur.DateTStamp;
			if(_selectedProvNum==0) {
				MsgBox.Show(this,"Please select a provider.");
				return;
			}
			if(AptOld.AptStatus!=ApptStatus.UnschedList && AptCur.AptStatus==ApptStatus.UnschedList) {
				//Extra log entry if the appt was sent to the unscheduled list
				Permissions perm=Permissions.AppointmentMove;
				if(AptOld.AptStatus==ApptStatus.Complete) {
					perm=Permissions.AppointmentCompleteEdit;
				}
				SecurityLogs.MakeLogEntry(perm,AptCur.PatNum,AptCur.ProcDescript+", "+AptCur.AptDateTime.ToString()
					+", Sent to Unscheduled List",AptCur.AptNum,datePrevious);
			}
			#region Validate Apt Start and End
			string pattern=Appointments.ConvertPatternTo5(strBTime.ToString());
			int minutes=pattern.Length*5;
			//compare beginning of new appointment against end to see if they fall on different days
			if(AptCur.AptDateTime.Day!=AptCur.AptDateTime.AddMinutes(minutes).Day) {
				MsgBox.Show(this,"You cannot have an appointment that starts and ends on different days.");
				return;
			}
			#endregion
			if(!UpdateListAndDB(true,true,true)) {
				return;
			}
			Plugins.HookAddCode(this,"FormApptEdit.butOK_Click_end",AptCur,AptOld,pat);
			DialogResult=DialogResult.OK;
			if(!this.Modal) {
				Close();
			}
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
			if(!this.Modal) {
				Close();
			}
		}

		private void FormApptEdit_FormClosing(object sender,FormClosingEventArgs e) {
			//Do not use pat.PatNum here.  Use AptCur.PatNum instead.  Pat will be null in the case that the user does not have the appt create permission.
			DateTime datePrevious=AptCur.DateTStamp;
			if(DialogResult!=DialogResult.OK) {
				if(AptCur.AptStatus==ApptStatus.Complete) {
					//This is a completed appointment and we need to warn the user if they are trying to leave the window and need to detach procs first.
					foreach(ODGridRow row in gridProc.Rows) {
						bool attached=false;
						if(AptCur.AptStatus==ApptStatus.Planned && ((Procedure)row.Tag).PlannedAptNum==AptCur.AptNum) {
							attached=true;
						}
						else if(((Procedure)row.Tag).AptNum==AptCur.AptNum) {
							attached=true;
						}
						if(((Procedure)row.Tag).ProcStatus!=ProcStat.TP || !attached) {
							continue;
						}
						if(!Security.IsAuthorized(Permissions.AppointmentCompleteEdit,true)) {
							continue;
						}
						MsgBox.Show(this,"Detach treatment planned procedures or click OK in the appointment edit window to set them complete.");
						e.Cancel=true;
						return;
					}
				}
				if(IsNew) {
					SecurityLogs.MakeLogEntry(Permissions.AppointmentEdit,AptCur.PatNum,
						"Create cancel for date/time: "+AptCur.AptDateTime.ToString(),
						AptCur.AptNum,datePrevious);
					//If cancel was pressed we want to un-do any changes to other appointments that were done.
					_listAppointments=Appointments.GetAppointmentsForProcs(_listProcsForAppt);
					//Add the current appointment if it is not in this list so it can get properly deleted by the sync later.
					if(!_listAppointments.Exists(x => x.AptNum==AptCur.AptNum)) {
						_listAppointments.Add(AptCur);
					}
					//We need to add this current appointment to the list of old appointments so we run the Appointments.Delete fucntion on it
					//This will remove any procedure connections that we created while in this window.
					_listAppointmentsOld=_listAppointments.Select(x => x.Copy()).ToList();
					//Now we also have to remove the appointment that was pre-inserted and is in this list as well so it is deleted on sync.
					_listAppointments.RemoveAll(x => x.AptNum==AptCur.AptNum);
				}
				else {  //User clicked cancel (or X button) on an existing appt
					AptCur=AptOld.Copy();  //We do not want to save any other changes made in this form.
					if(AptCur.AptStatus==ApptStatus.Scheduled && PrefC.GetBool(PrefName.InsChecksFrequency) && !CheckFrequencies()) {
						e.Cancel=true;
						return;
					}
				}
			}
			else {//DialogResult==DialogResult.OK (User clicked OK or Delete)
				//Note that Procedures.Sync is never used.  This is intentional.  In order to properly use procedure.Sync logic in this form we would
				//need to enhance ProcEdit and all its possible child forms to also not insert into DB until OK is clicked.  This would be a massive undertaking
				//and as such we just immediately push changes to DB.
				if(AptCur.AptStatus==ApptStatus.Scheduled && !_isDeleted && PrefC.GetBool(PrefName.InsChecksFrequency) && !CheckFrequencies()) {
					e.Cancel=true;
					return;
				}
				if(AptCur.AptStatus==ApptStatus.Scheduled) {
					//find all procs that are currently attached to the appt that weren't when the form opened
					List<string> listProcCodes = _listProcsForAppt.FindAll(x => x.AptNum==AptCur.AptNum && !_listProcNumsAttachedStart.Contains(x.ProcNum))
						.Select(x => ProcedureCodes.GetStringProcCode(x.CodeNum)).Distinct().ToList();//get list of string proc codes
					AutomationL.Trigger(AutomationTrigger.ScheduleProcedure,listProcCodes,AptCur.PatNum);
				}
			}
			//Sync detaches any attached procedures within Appointments.Delete() but doesn't create any ApptComm items.
			Appointments.Sync(_listAppointments,_listAppointmentsOld,AptCur.PatNum);
			//Synch the recalls for this patient.  This is necessary in case the date of the appointment has change or has been deleted entirely.
			Recalls.Synch(AptCur.PatNum);
			Recalls.SynchScheduledApptFull(AptCur.PatNum);
		}

	}
}