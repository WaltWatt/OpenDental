using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Linq;
using CodeBase;

namespace OpenDental {
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormTaskEdit:ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private IContainer components;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelDateTask;
		private System.Windows.Forms.Label labelDateAdvice;
		private System.Windows.Forms.Label labelDateType;
		private OpenDental.ODtextBox textDescript;
		private Task _taskCur;
		private Task _taskOld;
		private OpenDental.ValidDate textDateTask;
		private OpenDental.UI.Button butChange;
		private OpenDental.UI.Button butGoto;
		///<summary></summary>
		public bool IsNew;
		private System.Windows.Forms.CheckBox checkFromNum;
		private System.Windows.Forms.Label labelObjectDesc;
		private System.Windows.Forms.TextBox textObjectDesc;
		private System.Windows.Forms.ListBox listObjectType;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Panel panelObject;
		///<summary>After closing, if this is not zero, then it will jump to the object specified in GotoKeyNum.</summary>
		public TaskObjectType GotoType;
		private Label label5;
		private TextBox textDateTimeEntry;
		private OpenDental.UI.Button butNow;
		private OpenDental.UI.Button butDelete;
		private TextBox textUser;
		private Label label16;
		private OpenDental.UI.Button butNowFinished;
		private TextBox textDateTimeFinished;
		private Label label7;
		private TextBox textTaskNum;
		private Label labelTaskNum;
		///<summary>After closing, if this is not zero, then it will jump to the specified patient.</summary>
		public long GotoKeyNum;
		private Label labelReply;
		private OpenDental.UI.Button butReply;
		private OpenDental.UI.Button butSend;
		private TextBox textTaskList;
		private Label label10;
		private ComboBox comboDateType;
		private TaskList _taskListCur;
		private UI.ODGrid gridMain;
		///<summary>Will be set to true if any note was added or an existing note changed. Does not track changes in the description.</summary>
		public bool NotesChanged;
		private UI.Button butAddNote;
		private UI.Button butChangeUser;
		private List<TaskNote> _listTaskNotes;
		private CheckBox checkNew;
		private CheckBox checkDone;
		private Label labelDoneAffectsAll;
		///<summary>If the reply button is visible, this stores who to reply to.  It's determined when loading the form.</summary>
		private long _replyToUserNum;
		///<summary>Gets set to true externally if this window popped up without user interaction.  It will behave slightly differently.  
		///Specifically, the New checkbox will be unchecked so that if user clicks OK, the task will be marked as read.
		///Also if IsPop is set to true, this window will not steal focus from other windows when poping up.</summary>
		public bool IsPopup;
		///<summary>When tracking status by user, this tracks whether it has changed.  This is so that if it has changed, a signal can be sent for a refresh of lists.</summary>
		private bool _statusChanged;
		///<summary>If this task starts out 'unread', then this starts out true.  If the user changes the description or changes a note, then the task gets set to read.  But the user can manually change it back and this variable gets set to false.  From then on, any changes to description or note do not trigger the task to get set to read.  In other words, the automation only happens once.</summary>
		private bool _mightNeedSetRead;
		private UI.Button butCopy;
		private TextBox textBox1;
		private UI.Button butRed;
		private UI.Button butBlue;
		private ComboBox comboTaskPriorities;
		private Label label8;
		///<summary>When this window is first opened, if this task is in someone else's inbox, then the "new" status is meaningless and will not show.  In that case, this variable is set to true.  Only used when tracking new status by user.</summary>
		private bool _startedInOthersInbox;
		private System.Windows.Forms.Button butColor;
		///<summary>Filled on load with all non-hidden task priority definitions.</summary>
		private List<Def> _listTaskPriorities;
		private long _pritoryDefNumSelected;
		///<summary>Keeps track of the number of notes that were associated to this task on load and after refilling the task note grid.  Only used for HQ in order to keep track of task note manipulation.</summary>
		private int _numNotes=-1;
		///<summary>FK to the definition.DefNum at HQ for the triage priority color for red.</summary>
		private const long _triageRedNum=501;
		///<summary>FK to the definition.DefNum at HQ for the triage priority color for blue.</summary>
		private const long _triageBlueNum=502;
		///<summary>FK to the definition.DefNum at HQ for the triage priority color for white.</summary>
		private const long _triageWhiteNum=503;
		private UI.Button butAudit;
		private ComboBox comboJobs;
		private Label labelJobs;
		private List<JobLink> _jobLinks;
		private List<Job> _listJobs;
		private bool _isLoading;
		private ComboBox comboReminderRepeat;
		private Label labelReminderRepeat;
		private ValidNumber textReminderRepeatFrequency;
		private Label label2;
		private GroupBox groupReminder;
		private Label labelRemindFrequency;
		private Label labelReminderRepeatDays;
		private CheckBox checkReminderRepeatSunday;
		private CheckBox checkReminderRepeatSaturday;
		private CheckBox checkReminderRepeatFriday;
		private CheckBox checkReminderRepeatThursday;
		private CheckBox checkReminderRepeatWednesday;
		private CheckBox checkReminderRepeatTuesday;
		private CheckBox checkReminderRepeatMonday;
		private Label labelReminderRepeatDayKey;
		private PanelOD panelReminderDays;
		private PanelOD panelRepeating;
		private PanelOD panelReminderFrequency;
		private List<TaskReminderType> _listTaskReminderTypeNames;
		private SplitContainerNoFlicker splitContainerDescriptNote;
		private UI.Button butAutoNote;
		private DateTimePicker datePickerReminder;
		private DateTimePicker timePickerReminder;
		private UI.Button butCreateJob;
		private Label labelTaskChanged;
		private TextBox textBoxDateTimeCreated;
		private Label label3;
		private UI.Button butRefresh;

		///<summary>This is used to make the task window not steal focus when opening as a popup.</summary>
		protected override bool ShowWithoutActivation
		{
			get { return IsPopup; }
		}

		public long TaskNumCur {
			get {
				return _taskCur.TaskNum;
			}
		}

		///<summary>Task gets inserted ahead of time, then frequently altered before passing in here.  The taskOld that is passed in should be the task as it is in the database.  When saving, taskOld will be compared with db to make sure no changes.</summary>
		public FormTaskEdit(Task taskCur,Task taskOld) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			_taskCur=taskCur;
			_taskOld=taskOld;
			_taskListCur=TaskLists.GetOne(taskCur.TaskListNum);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTaskEdit));
			this.timePickerReminder = new System.Windows.Forms.DateTimePicker();
			this.butCreateJob = new OpenDental.UI.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.labelJobs = new System.Windows.Forms.Label();
			this.panelRepeating = new OpenDental.UI.PanelOD();
			this.labelDateTask = new System.Windows.Forms.Label();
			this.labelDateType = new System.Windows.Forms.Label();
			this.checkFromNum = new System.Windows.Forms.CheckBox();
			this.textDateTask = new OpenDental.ValidDate();
			this.comboDateType = new System.Windows.Forms.ComboBox();
			this.labelDateAdvice = new System.Windows.Forms.Label();
			this.groupReminder = new System.Windows.Forms.GroupBox();
			this.datePickerReminder = new System.Windows.Forms.DateTimePicker();
			this.panelReminderFrequency = new OpenDental.UI.PanelOD();
			this.labelRemindFrequency = new System.Windows.Forms.Label();
			this.textReminderRepeatFrequency = new OpenDental.ValidNumber();
			this.label2 = new System.Windows.Forms.Label();
			this.labelReminderRepeat = new System.Windows.Forms.Label();
			this.comboReminderRepeat = new System.Windows.Forms.ComboBox();
			this.panelReminderDays = new OpenDental.UI.PanelOD();
			this.labelReminderRepeatDayKey = new System.Windows.Forms.Label();
			this.labelReminderRepeatDays = new System.Windows.Forms.Label();
			this.checkReminderRepeatMonday = new System.Windows.Forms.CheckBox();
			this.checkReminderRepeatSunday = new System.Windows.Forms.CheckBox();
			this.checkReminderRepeatTuesday = new System.Windows.Forms.CheckBox();
			this.checkReminderRepeatSaturday = new System.Windows.Forms.CheckBox();
			this.checkReminderRepeatWednesday = new System.Windows.Forms.CheckBox();
			this.checkReminderRepeatFriday = new System.Windows.Forms.CheckBox();
			this.checkReminderRepeatThursday = new System.Windows.Forms.CheckBox();
			this.comboJobs = new System.Windows.Forms.ComboBox();
			this.butAudit = new OpenDental.UI.Button();
			this.butColor = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.comboTaskPriorities = new System.Windows.Forms.ComboBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textTaskNum = new System.Windows.Forms.TextBox();
			this.labelTaskNum = new System.Windows.Forms.Label();
			this.checkDone = new System.Windows.Forms.CheckBox();
			this.labelDoneAffectsAll = new System.Windows.Forms.Label();
			this.checkNew = new System.Windows.Forms.CheckBox();
			this.butChangeUser = new OpenDental.UI.Button();
			this.butAddNote = new OpenDental.UI.Button();
			this.textTaskList = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.butSend = new OpenDental.UI.Button();
			this.labelReply = new System.Windows.Forms.Label();
			this.butReply = new OpenDental.UI.Button();
			this.butNowFinished = new OpenDental.UI.Button();
			this.textDateTimeFinished = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.textUser = new System.Windows.Forms.TextBox();
			this.label16 = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
			this.butNow = new OpenDental.UI.Button();
			this.textDateTimeEntry = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.panelObject = new System.Windows.Forms.Panel();
			this.textObjectDesc = new System.Windows.Forms.TextBox();
			this.labelObjectDesc = new System.Windows.Forms.Label();
			this.butGoto = new OpenDental.UI.Button();
			this.butChange = new OpenDental.UI.Button();
			this.listObjectType = new System.Windows.Forms.ListBox();
			this.butCopy = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.splitContainerDescriptNote = new OpenDental.SplitContainerNoFlicker();
			this.butAutoNote = new OpenDental.UI.Button();
			this.butBlue = new OpenDental.UI.Button();
			this.butRed = new OpenDental.UI.Button();
			this.textDescript = new OpenDental.ODtextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.labelTaskChanged = new System.Windows.Forms.Label();
			this.butRefresh = new OpenDental.UI.Button();
			this.textBoxDateTimeCreated = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.panelRepeating.SuspendLayout();
			this.groupReminder.SuspendLayout();
			this.panelReminderFrequency.SuspendLayout();
			this.panelReminderDays.SuspendLayout();
			this.panelObject.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerDescriptNote)).BeginInit();
			this.splitContainerDescriptNote.Panel1.SuspendLayout();
			this.splitContainerDescriptNote.Panel2.SuspendLayout();
			this.splitContainerDescriptNote.SuspendLayout();
			this.SuspendLayout();
			// 
			// timePickerReminder
			// 
			this.timePickerReminder.CustomFormat = "hh:mm:ss tt";
			this.timePickerReminder.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.timePickerReminder.Location = new System.Drawing.Point(485, 37);
			this.timePickerReminder.Name = "timePickerReminder";
			this.timePickerReminder.ShowUpDown = true;
			this.timePickerReminder.Size = new System.Drawing.Size(89, 20);
			this.timePickerReminder.TabIndex = 4;
			this.timePickerReminder.Visible = false;
			// 
			// butCreateJob
			// 
			this.butCreateJob.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCreateJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCreateJob.Autosize = true;
			this.butCreateJob.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCreateJob.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCreateJob.CornerRadius = 4F;
			this.butCreateJob.Location = new System.Drawing.Point(702, 567);
			this.butCreateJob.Name = "butCreateJob";
			this.butCreateJob.Size = new System.Drawing.Size(75, 24);
			this.butCreateJob.TabIndex = 172;
			this.butCreateJob.Text = "Create Job";
			this.butCreateJob.Visible = false;
			this.butCreateJob.Click += new System.EventHandler(this.butCreateJob_Click);
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label6.Location = new System.Drawing.Point(312, 564);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(116, 19);
			this.label6.TabIndex = 14;
			this.label6.Text = "Object Type";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelJobs
			// 
			this.labelJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelJobs.Location = new System.Drawing.Point(382, 540);
			this.labelJobs.Name = "labelJobs";
			this.labelJobs.Size = new System.Drawing.Size(47, 19);
			this.labelJobs.TabIndex = 162;
			this.labelJobs.Text = "Jobs";
			this.labelJobs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelJobs.Visible = false;
			// 
			// panelRepeating
			// 
			this.panelRepeating.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.panelRepeating.BorderColor = System.Drawing.Color.Transparent;
			this.panelRepeating.Controls.Add(this.labelDateTask);
			this.panelRepeating.Controls.Add(this.labelDateType);
			this.panelRepeating.Controls.Add(this.checkFromNum);
			this.panelRepeating.Controls.Add(this.textDateTask);
			this.panelRepeating.Controls.Add(this.comboDateType);
			this.panelRepeating.Controls.Add(this.labelDateAdvice);
			this.panelRepeating.Location = new System.Drawing.Point(12, 535);
			this.panelRepeating.Name = "panelRepeating";
			this.panelRepeating.Size = new System.Drawing.Size(383, 75);
			this.panelRepeating.TabIndex = 170;
			// 
			// labelDateTask
			// 
			this.labelDateTask.Location = new System.Drawing.Point(1, 1);
			this.labelDateTask.Name = "labelDateTask";
			this.labelDateTask.Size = new System.Drawing.Size(102, 19);
			this.labelDateTask.TabIndex = 4;
			this.labelDateTask.Text = "Date";
			this.labelDateTask.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelDateType
			// 
			this.labelDateType.Location = new System.Drawing.Point(1, 27);
			this.labelDateType.Name = "labelDateType";
			this.labelDateType.Size = new System.Drawing.Size(102, 19);
			this.labelDateType.TabIndex = 7;
			this.labelDateType.Text = "Date Type";
			this.labelDateType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkFromNum
			// 
			this.checkFromNum.CheckAlign = System.Drawing.ContentAlignment.TopRight;
			this.checkFromNum.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkFromNum.Location = new System.Drawing.Point(1, 53);
			this.checkFromNum.Name = "checkFromNum";
			this.checkFromNum.Size = new System.Drawing.Size(116, 18);
			this.checkFromNum.TabIndex = 3;
			this.checkFromNum.Text = "Is From Repeating";
			this.checkFromNum.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDateTask
			// 
			this.textDateTask.Location = new System.Drawing.Point(105, 1);
			this.textDateTask.Name = "textDateTask";
			this.textDateTask.Size = new System.Drawing.Size(87, 20);
			this.textDateTask.TabIndex = 2;
			// 
			// comboDateType
			// 
			this.comboDateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDateType.FormattingEnabled = true;
			this.comboDateType.Location = new System.Drawing.Point(105, 27);
			this.comboDateType.Name = "comboDateType";
			this.comboDateType.Size = new System.Drawing.Size(145, 21);
			this.comboDateType.TabIndex = 148;
			// 
			// labelDateAdvice
			// 
			this.labelDateAdvice.Location = new System.Drawing.Point(193, -1);
			this.labelDateAdvice.Name = "labelDateAdvice";
			this.labelDateAdvice.Size = new System.Drawing.Size(185, 32);
			this.labelDateAdvice.TabIndex = 6;
			this.labelDateAdvice.Text = "Leave blank unless you want this task to show on a dated list";
			// 
			// groupReminder
			// 
			this.groupReminder.Controls.Add(this.datePickerReminder);
			this.groupReminder.Controls.Add(this.panelReminderFrequency);
			this.groupReminder.Controls.Add(this.labelReminderRepeat);
			this.groupReminder.Controls.Add(this.comboReminderRepeat);
			this.groupReminder.Controls.Add(this.panelReminderDays);
			this.groupReminder.Location = new System.Drawing.Point(336, 1);
			this.groupReminder.Name = "groupReminder";
			this.groupReminder.Size = new System.Drawing.Size(242, 84);
			this.groupReminder.TabIndex = 169;
			this.groupReminder.TabStop = false;
			this.groupReminder.Text = "Reminder";
			this.groupReminder.Visible = false;
			// 
			// datePickerReminder
			// 
			this.datePickerReminder.CustomFormat = "MMMM  dd,   yyyy";
			this.datePickerReminder.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.datePickerReminder.Location = new System.Drawing.Point(4, 36);
			this.datePickerReminder.Name = "datePickerReminder";
			this.datePickerReminder.Size = new System.Drawing.Size(144, 20);
			this.datePickerReminder.TabIndex = 2;
			this.datePickerReminder.Visible = false;
			// 
			// panelReminderFrequency
			// 
			this.panelReminderFrequency.BorderColor = System.Drawing.Color.Transparent;
			this.panelReminderFrequency.Controls.Add(this.labelRemindFrequency);
			this.panelReminderFrequency.Controls.Add(this.textReminderRepeatFrequency);
			this.panelReminderFrequency.Controls.Add(this.label2);
			this.panelReminderFrequency.Location = new System.Drawing.Point(1, 34);
			this.panelReminderFrequency.Name = "panelReminderFrequency";
			this.panelReminderFrequency.Size = new System.Drawing.Size(240, 22);
			this.panelReminderFrequency.TabIndex = 2;
			this.panelReminderFrequency.TabStop = true;
			// 
			// labelRemindFrequency
			// 
			this.labelRemindFrequency.Location = new System.Drawing.Point(157, 1);
			this.labelRemindFrequency.Name = "labelRemindFrequency";
			this.labelRemindFrequency.Size = new System.Drawing.Size(80, 20);
			this.labelRemindFrequency.TabIndex = 0;
			this.labelRemindFrequency.Text = "Days";
			this.labelRemindFrequency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textReminderRepeatFrequency
			// 
			this.textReminderRepeatFrequency.Location = new System.Drawing.Point(92, 1);
			this.textReminderRepeatFrequency.MaxVal = 999999999;
			this.textReminderRepeatFrequency.MinVal = 1;
			this.textReminderRepeatFrequency.Name = "textReminderRepeatFrequency";
			this.textReminderRepeatFrequency.Size = new System.Drawing.Size(50, 20);
			this.textReminderRepeatFrequency.TabIndex = 1;
			this.textReminderRepeatFrequency.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textReminderRepeatFrequency_KeyUp);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(1, 1);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(90, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "Every";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelReminderRepeat
			// 
			this.labelReminderRepeat.Location = new System.Drawing.Point(2, 13);
			this.labelReminderRepeat.Name = "labelReminderRepeat";
			this.labelReminderRepeat.Size = new System.Drawing.Size(90, 21);
			this.labelReminderRepeat.TabIndex = 0;
			this.labelReminderRepeat.Text = "Type";
			this.labelReminderRepeat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboReminderRepeat
			// 
			this.comboReminderRepeat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboReminderRepeat.FormattingEnabled = true;
			this.comboReminderRepeat.Location = new System.Drawing.Point(93, 12);
			this.comboReminderRepeat.Name = "comboReminderRepeat";
			this.comboReminderRepeat.Size = new System.Drawing.Size(145, 21);
			this.comboReminderRepeat.TabIndex = 1;
			this.comboReminderRepeat.SelectedIndexChanged += new System.EventHandler(this.comboReminderRepeat_SelectedIndexChanged);
			// 
			// panelReminderDays
			// 
			this.panelReminderDays.BorderColor = System.Drawing.Color.Transparent;
			this.panelReminderDays.Controls.Add(this.labelReminderRepeatDayKey);
			this.panelReminderDays.Controls.Add(this.labelReminderRepeatDays);
			this.panelReminderDays.Controls.Add(this.checkReminderRepeatMonday);
			this.panelReminderDays.Controls.Add(this.checkReminderRepeatSunday);
			this.panelReminderDays.Controls.Add(this.checkReminderRepeatTuesday);
			this.panelReminderDays.Controls.Add(this.checkReminderRepeatSaturday);
			this.panelReminderDays.Controls.Add(this.checkReminderRepeatWednesday);
			this.panelReminderDays.Controls.Add(this.checkReminderRepeatFriday);
			this.panelReminderDays.Controls.Add(this.checkReminderRepeatThursday);
			this.panelReminderDays.Location = new System.Drawing.Point(1, 55);
			this.panelReminderDays.Name = "panelReminderDays";
			this.panelReminderDays.Size = new System.Drawing.Size(240, 28);
			this.panelReminderDays.TabIndex = 3;
			this.panelReminderDays.TabStop = true;
			// 
			// labelReminderRepeatDayKey
			// 
			this.labelReminderRepeatDayKey.Location = new System.Drawing.Point(91, 15);
			this.labelReminderRepeatDayKey.Name = "labelReminderRepeatDayKey";
			this.labelReminderRepeatDayKey.Size = new System.Drawing.Size(109, 11);
			this.labelReminderRepeatDayKey.TabIndex = 0;
			this.labelReminderRepeatDayKey.Text = "M  T  W  R  F  S  U";
			// 
			// labelReminderRepeatDays
			// 
			this.labelReminderRepeatDays.Location = new System.Drawing.Point(18, 1);
			this.labelReminderRepeatDays.Name = "labelReminderRepeatDays";
			this.labelReminderRepeatDays.Size = new System.Drawing.Size(73, 17);
			this.labelReminderRepeatDays.TabIndex = 0;
			this.labelReminderRepeatDays.Text = "Days";
			this.labelReminderRepeatDays.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkReminderRepeatMonday
			// 
			this.checkReminderRepeatMonday.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkReminderRepeatMonday.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkReminderRepeatMonday.Location = new System.Drawing.Point(92, 0);
			this.checkReminderRepeatMonday.Name = "checkReminderRepeatMonday";
			this.checkReminderRepeatMonday.Size = new System.Drawing.Size(13, 17);
			this.checkReminderRepeatMonday.TabIndex = 1;
			this.checkReminderRepeatMonday.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkReminderRepeatSunday
			// 
			this.checkReminderRepeatSunday.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkReminderRepeatSunday.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkReminderRepeatSunday.Location = new System.Drawing.Point(176, 0);
			this.checkReminderRepeatSunday.Name = "checkReminderRepeatSunday";
			this.checkReminderRepeatSunday.Size = new System.Drawing.Size(13, 17);
			this.checkReminderRepeatSunday.TabIndex = 7;
			this.checkReminderRepeatSunday.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkReminderRepeatTuesday
			// 
			this.checkReminderRepeatTuesday.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkReminderRepeatTuesday.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkReminderRepeatTuesday.Location = new System.Drawing.Point(106, 0);
			this.checkReminderRepeatTuesday.Name = "checkReminderRepeatTuesday";
			this.checkReminderRepeatTuesday.Size = new System.Drawing.Size(13, 17);
			this.checkReminderRepeatTuesday.TabIndex = 2;
			this.checkReminderRepeatTuesday.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkReminderRepeatSaturday
			// 
			this.checkReminderRepeatSaturday.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkReminderRepeatSaturday.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkReminderRepeatSaturday.Location = new System.Drawing.Point(162, 0);
			this.checkReminderRepeatSaturday.Name = "checkReminderRepeatSaturday";
			this.checkReminderRepeatSaturday.Size = new System.Drawing.Size(13, 17);
			this.checkReminderRepeatSaturday.TabIndex = 6;
			this.checkReminderRepeatSaturday.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkReminderRepeatWednesday
			// 
			this.checkReminderRepeatWednesday.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkReminderRepeatWednesday.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkReminderRepeatWednesday.Location = new System.Drawing.Point(120, 0);
			this.checkReminderRepeatWednesday.Name = "checkReminderRepeatWednesday";
			this.checkReminderRepeatWednesday.Size = new System.Drawing.Size(13, 17);
			this.checkReminderRepeatWednesday.TabIndex = 3;
			this.checkReminderRepeatWednesday.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkReminderRepeatFriday
			// 
			this.checkReminderRepeatFriday.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkReminderRepeatFriday.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkReminderRepeatFriday.Location = new System.Drawing.Point(148, 0);
			this.checkReminderRepeatFriday.Name = "checkReminderRepeatFriday";
			this.checkReminderRepeatFriday.Size = new System.Drawing.Size(13, 17);
			this.checkReminderRepeatFriday.TabIndex = 5;
			this.checkReminderRepeatFriday.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkReminderRepeatThursday
			// 
			this.checkReminderRepeatThursday.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkReminderRepeatThursday.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkReminderRepeatThursday.Location = new System.Drawing.Point(134, 0);
			this.checkReminderRepeatThursday.Name = "checkReminderRepeatThursday";
			this.checkReminderRepeatThursday.Size = new System.Drawing.Size(13, 17);
			this.checkReminderRepeatThursday.TabIndex = 4;
			this.checkReminderRepeatThursday.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboJobs
			// 
			this.comboJobs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboJobs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboJobs.FormattingEnabled = true;
			this.comboJobs.Location = new System.Drawing.Point(431, 540);
			this.comboJobs.Name = "comboJobs";
			this.comboJobs.Size = new System.Drawing.Size(346, 21);
			this.comboJobs.TabIndex = 163;
			this.comboJobs.Visible = false;
			this.comboJobs.SelectedIndexChanged += new System.EventHandler(this.comboJobs_SelectedIndexChanged);
			// 
			// butAudit
			// 
			this.butAudit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAudit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAudit.Autosize = true;
			this.butAudit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAudit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAudit.CornerRadius = 4F;
			this.butAudit.Location = new System.Drawing.Point(887, 59);
			this.butAudit.Name = "butAudit";
			this.butAudit.Size = new System.Drawing.Size(61, 24);
			this.butAudit.TabIndex = 160;
			this.butAudit.Text = "History";
			this.butAudit.Click += new System.EventHandler(this.butAudit_Click);
			// 
			// butColor
			// 
			this.butColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butColor.Enabled = false;
			this.butColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butColor.Location = new System.Drawing.Point(857, 61);
			this.butColor.Name = "butColor";
			this.butColor.Size = new System.Drawing.Size(24, 21);
			this.butColor.TabIndex = 159;
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label8.Location = new System.Drawing.Point(625, 61);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(94, 21);
			this.label8.TabIndex = 158;
			this.label8.Text = "Task Priority";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboTaskPriorities
			// 
			this.comboTaskPriorities.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboTaskPriorities.FormattingEnabled = true;
			this.comboTaskPriorities.Location = new System.Drawing.Point(720, 61);
			this.comboTaskPriorities.Name = "comboTaskPriorities";
			this.comboTaskPriorities.Size = new System.Drawing.Size(134, 21);
			this.comboTaskPriorities.TabIndex = 157;
			this.comboTaskPriorities.SelectedIndexChanged += new System.EventHandler(this.comboTaskPriorities_SelectedIndexChanged);
			this.comboTaskPriorities.SelectionChangeCommitted += new System.EventHandler(this.comboTaskPriorities_SelectionChangeCommitted);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(454, -72);
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(54, 20);
			this.textBox1.TabIndex = 134;
			this.textBox1.Visible = false;
			// 
			// textTaskNum
			// 
			this.textTaskNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textTaskNum.Location = new System.Drawing.Point(894, 617);
			this.textTaskNum.Name = "textTaskNum";
			this.textTaskNum.ReadOnly = true;
			this.textTaskNum.Size = new System.Drawing.Size(54, 20);
			this.textTaskNum.TabIndex = 134;
			this.textTaskNum.Visible = false;
			// 
			// labelTaskNum
			// 
			this.labelTaskNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTaskNum.Location = new System.Drawing.Point(819, 618);
			this.labelTaskNum.Name = "labelTaskNum";
			this.labelTaskNum.Size = new System.Drawing.Size(73, 16);
			this.labelTaskNum.TabIndex = 133;
			this.labelTaskNum.Text = "TaskNum";
			this.labelTaskNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelTaskNum.Visible = false;
			// 
			// checkDone
			// 
			this.checkDone.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDone.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkDone.Location = new System.Drawing.Point(134, 3);
			this.checkDone.Name = "checkDone";
			this.checkDone.Size = new System.Drawing.Size(82, 17);
			this.checkDone.TabIndex = 153;
			this.checkDone.Text = "Done";
			this.checkDone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDone.Click += new System.EventHandler(this.checkDone_Click);
			// 
			// labelDoneAffectsAll
			// 
			this.labelDoneAffectsAll.Location = new System.Drawing.Point(217, 3);
			this.labelDoneAffectsAll.Name = "labelDoneAffectsAll";
			this.labelDoneAffectsAll.Size = new System.Drawing.Size(110, 17);
			this.labelDoneAffectsAll.TabIndex = 154;
			this.labelDoneAffectsAll.Text = "(affects all users)";
			this.labelDoneAffectsAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkNew
			// 
			this.checkNew.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkNew.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkNew.Location = new System.Drawing.Point(12, 3);
			this.checkNew.Name = "checkNew";
			this.checkNew.Size = new System.Drawing.Size(109, 17);
			this.checkNew.TabIndex = 152;
			this.checkNew.Text = "New";
			this.checkNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkNew.Click += new System.EventHandler(this.checkNew_Click);
			// 
			// butChangeUser
			// 
			this.butChangeUser.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChangeUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butChangeUser.Autosize = true;
			this.butChangeUser.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChangeUser.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChangeUser.CornerRadius = 4F;
			this.butChangeUser.Location = new System.Drawing.Point(857, 14);
			this.butChangeUser.Name = "butChangeUser";
			this.butChangeUser.Size = new System.Drawing.Size(24, 22);
			this.butChangeUser.TabIndex = 151;
			this.butChangeUser.Text = "...";
			this.butChangeUser.Click += new System.EventHandler(this.butChangeUser_Click);
			// 
			// butAddNote
			// 
			this.butAddNote.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAddNote.Autosize = true;
			this.butAddNote.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddNote.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddNote.CornerRadius = 4F;
			this.butAddNote.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddNote.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddNote.Location = new System.Drawing.Point(873, 540);
			this.butAddNote.Name = "butAddNote";
			this.butAddNote.Size = new System.Drawing.Size(75, 24);
			this.butAddNote.TabIndex = 150;
			this.butAddNote.Text = "Add";
			this.butAddNote.Click += new System.EventHandler(this.butAddNote_Click);
			// 
			// textTaskList
			// 
			this.textTaskList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textTaskList.Location = new System.Drawing.Point(720, 39);
			this.textTaskList.Name = "textTaskList";
			this.textTaskList.ReadOnly = true;
			this.textTaskList.Size = new System.Drawing.Size(134, 20);
			this.textTaskList.TabIndex = 146;
			// 
			// label10
			// 
			this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label10.Location = new System.Drawing.Point(625, 39);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(94, 20);
			this.label10.TabIndex = 147;
			this.label10.Text = "Task List";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butSend
			// 
			this.butSend.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSend.Autosize = true;
			this.butSend.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSend.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSend.CornerRadius = 4F;
			this.butSend.Location = new System.Drawing.Point(478, 649);
			this.butSend.Name = "butSend";
			this.butSend.Size = new System.Drawing.Size(75, 24);
			this.butSend.TabIndex = 142;
			this.butSend.Text = "Send To...";
			this.butSend.Click += new System.EventHandler(this.butSend_Click);
			// 
			// labelReply
			// 
			this.labelReply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelReply.Location = new System.Drawing.Point(312, 652);
			this.labelReply.Name = "labelReply";
			this.labelReply.Size = new System.Drawing.Size(162, 19);
			this.labelReply.TabIndex = 141;
			this.labelReply.Text = "(Send to author)";
			this.labelReply.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butReply
			// 
			this.butReply.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butReply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butReply.Autosize = true;
			this.butReply.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butReply.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butReply.CornerRadius = 4F;
			this.butReply.Location = new System.Drawing.Point(233, 649);
			this.butReply.Name = "butReply";
			this.butReply.Size = new System.Drawing.Size(75, 24);
			this.butReply.TabIndex = 140;
			this.butReply.Text = "Reply";
			this.butReply.Click += new System.EventHandler(this.butReply_Click);
			// 
			// butNowFinished
			// 
			this.butNowFinished.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNowFinished.Autosize = true;
			this.butNowFinished.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNowFinished.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNowFinished.CornerRadius = 4F;
			this.butNowFinished.Location = new System.Drawing.Point(264, 65);
			this.butNowFinished.Name = "butNowFinished";
			this.butNowFinished.Size = new System.Drawing.Size(62, 19);
			this.butNowFinished.TabIndex = 132;
			this.butNowFinished.Text = "Now";
			this.butNowFinished.Click += new System.EventHandler(this.butNowFinished_Click);
			// 
			// textDateTimeFinished
			// 
			this.textDateTimeFinished.Location = new System.Drawing.Point(107, 65);
			this.textDateTimeFinished.Name = "textDateTimeFinished";
			this.textDateTimeFinished.Size = new System.Drawing.Size(151, 20);
			this.textDateTimeFinished.TabIndex = 131;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(1, 64);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(105, 20);
			this.label7.TabIndex = 130;
			this.label7.Text = "Date/Time Finished";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textUser
			// 
			this.textUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textUser.Location = new System.Drawing.Point(720, 16);
			this.textUser.Name = "textUser";
			this.textUser.ReadOnly = true;
			this.textUser.Size = new System.Drawing.Size(134, 20);
			this.textUser.TabIndex = 0;
			// 
			// label16
			// 
			this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label16.Location = new System.Drawing.Point(625, 16);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(94, 20);
			this.label16.TabIndex = 125;
			this.label16.Text = "From User";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			this.butDelete.Location = new System.Drawing.Point(21, 649);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(80, 24);
			this.butDelete.TabIndex = 124;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butNow
			// 
			this.butNow.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNow.Autosize = true;
			this.butNow.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNow.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNow.CornerRadius = 4F;
			this.butNow.Location = new System.Drawing.Point(264, 43);
			this.butNow.Name = "butNow";
			this.butNow.Size = new System.Drawing.Size(62, 19);
			this.butNow.TabIndex = 19;
			this.butNow.Text = "Now";
			this.butNow.Click += new System.EventHandler(this.butNow_Click);
			// 
			// textDateTimeEntry
			// 
			this.textDateTimeEntry.Location = new System.Drawing.Point(107, 43);
			this.textDateTimeEntry.Name = "textDateTimeEntry";
			this.textDateTimeEntry.Size = new System.Drawing.Size(151, 20);
			this.textDateTimeEntry.TabIndex = 18;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(1, 42);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(105, 20);
			this.label5.TabIndex = 17;
			this.label5.Text = "Date/Time Task";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panelObject
			// 
			this.panelObject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.panelObject.Controls.Add(this.textObjectDesc);
			this.panelObject.Controls.Add(this.labelObjectDesc);
			this.panelObject.Controls.Add(this.butGoto);
			this.panelObject.Controls.Add(this.butChange);
			this.panelObject.Location = new System.Drawing.Point(3, 611);
			this.panelObject.Name = "panelObject";
			this.panelObject.Size = new System.Drawing.Size(590, 34);
			this.panelObject.TabIndex = 15;
			// 
			// textObjectDesc
			// 
			this.textObjectDesc.Location = new System.Drawing.Point(103, 0);
			this.textObjectDesc.Multiline = true;
			this.textObjectDesc.Name = "textObjectDesc";
			this.textObjectDesc.Size = new System.Drawing.Size(302, 34);
			this.textObjectDesc.TabIndex = 0;
			this.textObjectDesc.Text = "line 1\r\nline 2";
			// 
			// labelObjectDesc
			// 
			this.labelObjectDesc.Location = new System.Drawing.Point(26, 1);
			this.labelObjectDesc.Name = "labelObjectDesc";
			this.labelObjectDesc.Size = new System.Drawing.Size(77, 19);
			this.labelObjectDesc.TabIndex = 8;
			this.labelObjectDesc.Text = "ObjectDesc";
			this.labelObjectDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butGoto
			// 
			this.butGoto.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGoto.Autosize = true;
			this.butGoto.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGoto.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGoto.CornerRadius = 4F;
			this.butGoto.Location = new System.Drawing.Point(501, 5);
			this.butGoto.Name = "butGoto";
			this.butGoto.Size = new System.Drawing.Size(75, 24);
			this.butGoto.TabIndex = 12;
			this.butGoto.Text = "Go To";
			this.butGoto.Click += new System.EventHandler(this.butGoto_Click);
			// 
			// butChange
			// 
			this.butChange.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChange.Autosize = true;
			this.butChange.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChange.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChange.CornerRadius = 4F;
			this.butChange.Location = new System.Drawing.Point(418, 5);
			this.butChange.Name = "butChange";
			this.butChange.Size = new System.Drawing.Size(75, 24);
			this.butChange.TabIndex = 10;
			this.butChange.Text = "Change";
			this.butChange.Click += new System.EventHandler(this.butChange_Click);
			// 
			// listObjectType
			// 
			this.listObjectType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.listObjectType.Location = new System.Drawing.Point(431, 565);
			this.listObjectType.Name = "listObjectType";
			this.listObjectType.Size = new System.Drawing.Size(120, 43);
			this.listObjectType.TabIndex = 13;
			this.listObjectType.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listObjectType_MouseDown);
			// 
			// butCopy
			// 
			this.butCopy.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCopy.Autosize = true;
			this.butCopy.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCopy.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCopy.CornerRadius = 4F;
			this.butCopy.Image = global::OpenDental.Properties.Resources.butCopy;
			this.butCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCopy.Location = new System.Drawing.Point(791, 540);
			this.butCopy.Name = "butCopy";
			this.butCopy.Size = new System.Drawing.Size(75, 24);
			this.butCopy.TabIndex = 4;
			this.butCopy.Text = "Copy";
			this.butCopy.Click += new System.EventHandler(this.butCopy_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(791, 649);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 4;
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
			this.butCancel.Location = new System.Drawing.Point(873, 649);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 5;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// splitContainerDescriptNote
			// 
			this.splitContainerDescriptNote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainerDescriptNote.Location = new System.Drawing.Point(12, 85);
			this.splitContainerDescriptNote.Name = "splitContainerDescriptNote";
			this.splitContainerDescriptNote.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainerDescriptNote.Panel1
			// 
			this.splitContainerDescriptNote.Panel1.Controls.Add(this.butAutoNote);
			this.splitContainerDescriptNote.Panel1.Controls.Add(this.butBlue);
			this.splitContainerDescriptNote.Panel1.Controls.Add(this.butRed);
			this.splitContainerDescriptNote.Panel1.Controls.Add(this.textDescript);
			this.splitContainerDescriptNote.Panel1.Controls.Add(this.label1);
			this.splitContainerDescriptNote.Panel1MinSize = 106;
			// 
			// splitContainerDescriptNote.Panel2
			// 
			this.splitContainerDescriptNote.Panel2.Controls.Add(this.gridMain);
			this.splitContainerDescriptNote.Panel2MinSize = 50;
			this.splitContainerDescriptNote.Size = new System.Drawing.Size(936, 450);
			this.splitContainerDescriptNote.SplitterDistance = 106;
			this.splitContainerDescriptNote.TabIndex = 171;
			this.splitContainerDescriptNote.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainerNoFlicker1_SplitterMoved);
			// 
			// butAutoNote
			// 
			this.butAutoNote.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAutoNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAutoNote.Autosize = true;
			this.butAutoNote.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAutoNote.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAutoNote.CornerRadius = 4F;
			this.butAutoNote.Location = new System.Drawing.Point(9, 81);
			this.butAutoNote.Name = "butAutoNote";
			this.butAutoNote.Size = new System.Drawing.Size(80, 22);
			this.butAutoNote.TabIndex = 157;
			this.butAutoNote.Text = "Auto Note";
			this.butAutoNote.Click += new System.EventHandler(this.butAutoNote_Click);
			// 
			// butBlue
			// 
			this.butBlue.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBlue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butBlue.Autosize = true;
			this.butBlue.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBlue.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBlue.CornerRadius = 4F;
			this.butBlue.Location = new System.Drawing.Point(46, 27);
			this.butBlue.Name = "butBlue";
			this.butBlue.Size = new System.Drawing.Size(43, 24);
			this.butBlue.TabIndex = 156;
			this.butBlue.Text = "Blue";
			this.butBlue.Visible = false;
			this.butBlue.Click += new System.EventHandler(this.butBlue_Click);
			// 
			// butRed
			// 
			this.butRed.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butRed.Autosize = true;
			this.butRed.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRed.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRed.CornerRadius = 4F;
			this.butRed.Location = new System.Drawing.Point(46, 54);
			this.butRed.Name = "butRed";
			this.butRed.Size = new System.Drawing.Size(43, 24);
			this.butRed.TabIndex = 155;
			this.butRed.Text = "Red";
			this.butRed.Visible = false;
			this.butRed.Click += new System.EventHandler(this.butRed_Click);
			// 
			// textDescript
			// 
			this.textDescript.AcceptsTab = true;
			this.textDescript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDescript.BackColor = System.Drawing.SystemColors.Window;
			this.textDescript.DetectLinksEnabled = false;
			this.textDescript.DetectUrls = false;
			this.textDescript.Location = new System.Drawing.Point(92, 5);
			this.textDescript.Name = "textDescript";
			this.textDescript.QuickPasteType = OpenDentBusiness.QuickPasteType.Task;
			this.textDescript.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textDescript.Size = new System.Drawing.Size(841, 97);
			this.textDescript.TabIndex = 1;
			this.textDescript.Text = "";
			this.textDescript.TextChanged += new System.EventHandler(this.textDescript_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(9, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 20);
			this.label1.TabIndex = 2;
			this.label1.Text = "Description";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(0, 3);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(933, 333);
			this.gridMain.TabIndex = 149;
			this.gridMain.Title = "Notes";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "FormTaskEdit";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// labelTaskChanged
			// 
			this.labelTaskChanged.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTaskChanged.ForeColor = System.Drawing.Color.Red;
			this.labelTaskChanged.Location = new System.Drawing.Point(599, 624);
			this.labelTaskChanged.Name = "labelTaskChanged";
			this.labelTaskChanged.Size = new System.Drawing.Size(185, 23);
			this.labelTaskChanged.TabIndex = 173;
			this.labelTaskChanged.Text = "The task has been changed ";
			this.labelTaskChanged.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelTaskChanged.Visible = false;
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(648, 649);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 24);
			this.butRefresh.TabIndex = 174;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.UseVisualStyleBackColor = true;
			this.butRefresh.Visible = false;
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// textBoxDateTimeCreated
			// 
			this.textBoxDateTimeCreated.Location = new System.Drawing.Point(107, 21);
			this.textBoxDateTimeCreated.Name = "textBoxDateTimeCreated";
			this.textBoxDateTimeCreated.ReadOnly = true;
			this.textBoxDateTimeCreated.Size = new System.Drawing.Size(151, 20);
			this.textBoxDateTimeCreated.TabIndex = 175;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(1, 21);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(105, 20);
			this.label3.TabIndex = 176;
			this.label3.Text = "Date/Time Created";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormTaskEdit
			// 
			this.ClientSize = new System.Drawing.Size(974, 676);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textBoxDateTimeCreated);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.labelTaskChanged);
			this.Controls.Add(this.timePickerReminder);
			this.Controls.Add(this.butCreateJob);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.labelJobs);
			this.Controls.Add(this.panelRepeating);
			this.Controls.Add(this.groupReminder);
			this.Controls.Add(this.comboJobs);
			this.Controls.Add(this.butAudit);
			this.Controls.Add(this.butColor);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.comboTaskPriorities);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.textTaskNum);
			this.Controls.Add(this.labelTaskNum);
			this.Controls.Add(this.checkDone);
			this.Controls.Add(this.labelDoneAffectsAll);
			this.Controls.Add(this.checkNew);
			this.Controls.Add(this.butChangeUser);
			this.Controls.Add(this.butAddNote);
			this.Controls.Add(this.textTaskList);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.butSend);
			this.Controls.Add(this.labelReply);
			this.Controls.Add(this.butReply);
			this.Controls.Add(this.butNowFinished);
			this.Controls.Add(this.textDateTimeFinished);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textUser);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butNow);
			this.Controls.Add(this.textDateTimeEntry);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.panelObject);
			this.Controls.Add(this.listObjectType);
			this.Controls.Add(this.butCopy);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.splitContainerDescriptNote);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(990, 714);
			this.Name = "FormTaskEdit";
			this.Text = "Task";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTaskEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormTaskListEdit_Load);
			this.panelRepeating.ResumeLayout(false);
			this.panelRepeating.PerformLayout();
			this.groupReminder.ResumeLayout(false);
			this.panelReminderFrequency.ResumeLayout(false);
			this.panelReminderFrequency.PerformLayout();
			this.panelReminderDays.ResumeLayout(false);
			this.panelObject.ResumeLayout(false);
			this.panelObject.PerformLayout();
			this.splitContainerDescriptNote.Panel1.ResumeLayout(false);
			this.splitContainerDescriptNote.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerDescriptNote)).EndInit();
			this.splitContainerDescriptNote.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormTaskListEdit_Load(object sender,System.EventArgs e) {
			LoadTask();
			gridMain.ScrollToEnd();
		}

		private void LoadTask() { 
			#if DEBUG
				labelTaskNum.Visible=true;
				textTaskNum.Visible=true;
				textTaskNum.Text=_taskCur.TaskNum.ToString();
			#endif
			if(PrefC.IsODHQ) {//If HQ
				labelTaskNum.Visible=true;
				textTaskNum.Visible=true;
				textTaskNum.Text=_taskCur.TaskNum.ToString();
				if(!IsNew) {//to simplify the code, only allow jobs to be attached to existing tasks, not new tasks.
					comboJobs.Visible=true;
					labelJobs.Visible=true;
					FillComboJobs();
				}
			}
			if(IsNew) {
				//butDelete.Enabled always stays true
				//textDescript always editable
			}
			else {//trying to edit an existing task, so need to block some things
				bool isTaskForCurUser=true;
				if(_taskCur.UserNum!=Security.CurUser.UserNum) {//current user didn't write this task, so block them.
					isTaskForCurUser=false;//Delete will only be enabled if the user has the TaskEdit and TaskNoteEdit permissions.
				}
				if(_taskCur.TaskListNum!=Security.CurUser.TaskListInBox) {//the task is not in the logged-in user's inbox
					isTaskForCurUser=false;
				}
				if(isTaskForCurUser) {//this just allows getting the NoteList less often
					_listTaskNotes=TaskNotes.GetForTask(_taskCur.TaskNum);//so we can check so see if other users have added notes
					for(int i=0;i<_listTaskNotes.Count;i++) {
						if(Security.CurUser.UserNum!=_listTaskNotes[i].UserNum) {
							isTaskForCurUser=false;
							break;
						}
					}
				}
				if(!isTaskForCurUser && !Security.IsAuthorized(Permissions.TaskNoteEdit,true)) {//but only need to block them if they don't have TaskNoteEdit permission
					butDelete.Enabled=false;
				}
				if(!isTaskForCurUser && !Security.IsAuthorized(Permissions.TaskEdit,true)) {
					butDelete.Enabled=false;
					butAutoNote.Enabled=false;
					textDescript.ReadOnly=true;
					textDescript.BackColor=System.Drawing.SystemColors.Window;
				}
			}
			_listTaskPriorities=Defs.GetDefsForCategory(DefCat.TaskPriorities,true);//Fill list with non-hidden priorities.
			//There must be at least one priority in Setup | Definitions.  Do not let them load the task edit window without at least one priority.
			if(_listTaskPriorities.Count < 1) {
				MsgBox.Show(this,"There are no task priorities in Setup | Definitions.  There must be at least one in order to use the task system.");
				DialogResult=DialogResult.Cancel;
				Close();
			}
			bool hasDefault=false;
			_pritoryDefNumSelected=_taskCur.PriorityDefNum;
			if(_pritoryDefNumSelected==0 && IsNew && _taskCur.ReminderType!=TaskReminderType.NoReminder) {
				foreach(Def defTaskPriority in _listTaskPriorities) {
					if(defTaskPriority.ItemValue=="R") {
						_pritoryDefNumSelected=defTaskPriority.DefNum;
						break;
					}
				}
			}
			if(_pritoryDefNumSelected==0) {//The task does not yet have a priority assigned.  Find the default and assign it, if available.
				for(int i=0;i<_listTaskPriorities.Count;i++) {
					if(_listTaskPriorities[i].ItemValue=="D") {
						_pritoryDefNumSelected=_listTaskPriorities[i].DefNum;
						hasDefault=true;
						break;
					}
				}
			}
			comboTaskPriorities.Items.Clear();
			for(int i=0;i<_listTaskPriorities.Count;i++) {//Add non-hidden defs first
				comboTaskPriorities.Items.Add(_listTaskPriorities[i].ItemName);
				if(_pritoryDefNumSelected==_listTaskPriorities[i].DefNum) {//Use priority listed within the database.
					comboTaskPriorities.SelectedIndex=i;//Sets combo text too
				}
			}
			if((IsNew || _pritoryDefNumSelected==0) && !hasDefault) {//If no default has been set in the definitions, select the last item in the list.
				comboTaskPriorities.SelectedIndex=comboTaskPriorities.Items.Count-1;
				_pritoryDefNumSelected=_listTaskPriorities[_listTaskPriorities.Count-1].DefNum;
			}
			if(_taskListCur!=null && IsNew && _taskListCur.TaskListNum==1697 && PrefC.GetBool(PrefName.DockPhonePanelShow)) {//Set to triage blue if HQ, triage list, and is new.
				for(int i=0;i<_listTaskPriorities.Count;i++) {
					if(_listTaskPriorities[i].DefNum==_triageBlueNum) {//Finding the option that is triageBlue to select it in the combobox (Combobox mirrors _listTaskPriorityDefs)
						comboTaskPriorities.SelectedIndex=i;
						break;
					}
				}
				_pritoryDefNumSelected=_triageBlueNum;
			}
			if(comboTaskPriorities.SelectedIndex==-1) {//Priority for task wasn't found in the non-hidden priorities list (and isn't triageBlue), so it must be a hidden priority.
				List<Def> listTaskDefsLong=Defs.GetDefsForCategory(DefCat.TaskPriorities);//Get all priorities
				for(int i=0;i<listTaskDefsLong.Count;i++) {
					if(listTaskDefsLong[i].DefNum==_pritoryDefNumSelected) {//We find the hidden priority and set the text of the combo box.
						comboTaskPriorities.Text=(listTaskDefsLong[i].ItemName+" (Hidden)");
						butColor.BackColor=listTaskDefsLong[i].ItemColor;
					}
				}
			}
			textUser.Text=Userods.GetName(_taskCur.UserNum);//might be blank.
			if(_taskListCur!=null) {
				textTaskList.Text=_taskListCur.Descript;
			}
			if(_taskCur.DateTimeOriginal==DateTime.MinValue) {
				label3.Visible=false;
				textBoxDateTimeCreated.Visible=false;
			}
			else {
				textBoxDateTimeCreated.Text=_taskCur.DateTimeOriginal.ToString();
			}
			if(_taskCur.DateTimeEntry.Year<1880) {
				textDateTimeEntry.Text=DateTime.Now.ToString();
			}
			else {
				textDateTimeEntry.Text=_taskCur.DateTimeEntry.ToString();
			}
			if(_taskCur.DateTimeFinished.Year<1880) {
				textDateTimeFinished.Text="";//DateTime.Now.ToString();
			}
			else {
				textDateTimeFinished.Text=_taskCur.DateTimeFinished.ToString();
			}
			textDescript.Text=_taskCur.Descript;
			if(!IsPopup) {//otherwise, TextUser is selected, and it cannot accept input.  This prevents a popup from accepting using input accidentally.
				textDescript.Select();//Focus does not work for some reason.
				textDescript.Select(_taskCur.Descript.Length,0);//Place the cursor at the end of the description box.
			}
			if(PrefC.GetBool(PrefName.TasksNewTrackedByUser) && _taskCur.TaskListNum !=0) {
				long mailboxUserNum=TaskLists.GetMailboxUserNum(_taskCur.TaskListNum);
				if(mailboxUserNum != 0 && mailboxUserNum != Security.CurUser.UserNum) {
					_startedInOthersInbox=true;
					checkNew.Checked=false;
					checkNew.Enabled=false;
				}
			}
			//this section must come after textDescript is set:
			if(_taskCur.TaskStatus==TaskStatusEnum.Done) {//global even if new status is tracked by user
				checkDone.Checked=true;
			}
			else {//because it can't be both new and done.
				if(IsPopup) {//It clearly is Unread, but we don't want to leave it that way upon close OK.
					checkNew.Checked=false;
					_statusChanged=true;
				}
				else if(PrefC.GetBool(PrefName.TasksNewTrackedByUser)) {
					if(!_startedInOthersInbox && TaskUnreads.IsUnread(Security.CurUser.UserNum,_taskCur.TaskNum)) {
						checkNew.Checked=true;
						_mightNeedSetRead=true;
					}
				}
				else {//tracked globally, the old way
					if(_taskCur.TaskStatus==TaskStatusEnum.New) {
						checkNew.Checked=true;
					}
				}
			}
			groupReminder.Visible=(!PrefC.GetBool(PrefName.TasksUseRepeating));
			panelRepeating.Visible=PrefC.GetBool(PrefName.TasksUseRepeating);
			textReminderRepeatFrequency.Text=(IsNew?"1":_taskCur.ReminderFrequency.ToString());
			//Fill comboReminderRepeat with repeating options.
			_listTaskReminderTypeNames=new List<TaskReminderType>() {
				TaskReminderType.NoReminder,
				TaskReminderType.Once,
				TaskReminderType.Daily,
				TaskReminderType.Weekly,
				TaskReminderType.Monthly,
				TaskReminderType.Yearly
			};
			comboReminderRepeat.Items.Clear();
			for(int i=0;i<_listTaskReminderTypeNames.Count;i++) {
				comboReminderRepeat.Items.Add(_listTaskReminderTypeNames[i].ToString());
				if(_taskCur.ReminderType.HasFlag(_listTaskReminderTypeNames[i])) {
					comboReminderRepeat.SelectedIndex=i;
				}
			}
			if(_taskCur.ReminderType==TaskReminderType.Once) {
				if(IsNew) {
					datePickerReminder.Value=DateTime.Now;
					timePickerReminder.Value=DateTime.Now;
				}
				else {
					datePickerReminder.Value=_taskCur.DateTimeEntry;
					timePickerReminder.Value=_taskCur.DateTimeEntry;
				}
			}
			checkReminderRepeatMonday.Checked=_taskCur.ReminderType.HasFlag(TaskReminderType.Monday);
			checkReminderRepeatTuesday.Checked=_taskCur.ReminderType.HasFlag(TaskReminderType.Tuesday);
			checkReminderRepeatWednesday.Checked=_taskCur.ReminderType.HasFlag(TaskReminderType.Wednesday);
			checkReminderRepeatThursday.Checked=_taskCur.ReminderType.HasFlag(TaskReminderType.Thursday);
			checkReminderRepeatFriday.Checked=_taskCur.ReminderType.HasFlag(TaskReminderType.Friday);
			checkReminderRepeatSaturday.Checked=_taskCur.ReminderType.HasFlag(TaskReminderType.Saturday);
			checkReminderRepeatSunday.Checked=_taskCur.ReminderType.HasFlag(TaskReminderType.Sunday);
			if(_taskCur.DateTask.Year>1880) {
				textDateTask.Text=_taskCur.DateTask.ToShortDateString();
			}
			if(_taskCur.IsRepeating) {
				checkNew.Enabled=false;
				checkDone.Enabled=false;
				textDateTask.Enabled=false;
				listObjectType.Enabled=false;
				if(_taskCur.TaskListNum!=0) {//not a main parent
					comboDateType.Enabled=false;
				}
			}
			for(int i=0;i<Enum.GetNames(typeof(TaskDateType)).Length;i++) {
				comboDateType.Items.Add(Lan.g("enumTaskDateType",Enum.GetNames(typeof(TaskDateType))[i]));
				if((int)_taskCur.DateType==i) {
					comboDateType.SelectedIndex=i;
				}
			}
			if(_taskCur.FromNum==0) {
				checkFromNum.Checked=false;
				checkFromNum.Enabled=false;
			}
			else {
				checkFromNum.Checked=true;
			}
			for(int i=0;i<Enum.GetNames(typeof(TaskObjectType)).Length;i++) {
				listObjectType.Items.Add(Lan.g("enumTaskObjectType",Enum.GetNames(typeof(TaskObjectType))[i]));
			}
			_listTaskPriorities=Defs.GetDefsForCategory(DefCat.TaskPriorities,true);//Fill list with non-hidden priorities.
			//There must be at least one priority in Setup | Definitions.  Do not let them load the task edit window without at least one priority.
			if(_listTaskPriorities.Count < 1) {
				MsgBox.Show(this,"There are no task priorities in Setup | Definitions.  There must be at least one in order to use the task system.");
				DialogResult=DialogResult.Cancel;
				Close();
			}
			FillObject();
			FillGrid();//Need this in order to pick ReplyToUserNum next.
			if(IsNew) {
				labelReply.Visible=false;
				butReply.Visible=false;
			}
			else if(_taskListCur==null) {
				//|| TaskListCur.TaskListNum!=Security.CurUser.TaskListInBox) {//if this task is not in my inbox
				labelReply.Visible=false;
				butReply.Visible=false;
			}
			else if(_listTaskNotes.Count==0 && _taskCur.UserNum==Security.CurUser.UserNum) {//if this is my task
				labelReply.Visible=false;
				butReply.Visible=false;
			}
			else {//reply button will be visible
				if(_listTaskNotes.Count==0) {//no notes, so reply to owner
					_replyToUserNum=_taskCur.UserNum;
				}
				else {//reply to most recent author who is not me
					//loop backward through the notes to find who to reply to
					for(int i=_listTaskNotes.Count-1;i>=0;i--) {
						if(_listTaskNotes[i].UserNum!=Security.CurUser.UserNum) {
							_replyToUserNum=_listTaskNotes[i].UserNum;
							break;
						}
					}
					if(_replyToUserNum==0) {//can't figure out who to reply to.
						labelReply.Visible=false;
						butReply.Visible=false;
					}
				}
				labelReply.Text=Lan.g(this,"(Send to ")+Userods.GetName(_replyToUserNum)+")";
			}
			if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {//Show red and blue buttons for HQ always
				butRed.Visible=true;
				butBlue.Visible=true;
			}
			if(!Security.IsAuthorized(Permissions.TaskEdit,true)){
				butAudit.Visible=false;
			}
			SetTaskStartingLocation();
		}

		///<summary>Sets the starting location of this form. Should only be called on load.
		///The first Task window will default to CenterScreen. After that we will cascade down.
		///If any part of this form will be off screen we will default the next task to the top left of the primary monitor.</summary>
		private void SetTaskStartingLocation() { 
			List<FormTaskEdit> listTaskEdits=Application.OpenForms.OfType<FormTaskEdit>().ToList();
			if(listTaskEdits.Count==1) {//Since this form has already gone through the initilize, there will be at least 1.
				this.StartPosition=FormStartPosition.CenterScreen;
				return;
			}
			Point pointStart;
			//There are multiple task edit windows open, so offset the new window by a little so that it does not show up directly over the old one.
			const int OFFSET=20;//Sets how far to offset the cascaded form location.
			this.StartPosition=FormStartPosition.Manual;
			//Count is 1 based, list index is 0 based, -2 to get the "last" window
			FormTaskEdit formPrevious=listTaskEdits[listTaskEdits.Count-2];
			System.Windows.Forms.Screen screenCur=System.Windows.Forms.Screen.PrimaryScreen;
			//Figure out what monitor the previous task edit window is on.
			if(formPrevious!=null && !formPrevious.IsDisposed && formPrevious.WindowState!=FormWindowState.Minimized) {
				screenCur=System.Windows.Forms.Screen.FromControl(formPrevious);
				//Get the start point relative to the screen the form will open on.
				//pointStart=new Point(formPrevious.Location.X-screenCur.Bounds.Left,formPrevious.Location.Y-screenCur.Bounds.Top);
				pointStart=formPrevious.Location;
			}
			else {
				pointStart=new Point(screenCur.WorkingArea.X,screenCur.WorkingArea.Y);
			}
			//Temporarily apply the offset and see if that rectangle can fit on screenCur, if not, default to a high location on the primary screen.
			pointStart.X+=OFFSET;
			pointStart.Y+=OFFSET;
			Rectangle rect=new Rectangle(pointStart,this.Size);
			if(!screenCur.WorkingArea.Contains(rect)) {
				//A portion of the new window is outside of the usable area on the current monitor.
				//Force the new window to be at "0,50" (relatively) in regards to the primary monitor.
				pointStart=new Point(screenCur.WorkingArea.X,screenCur.WorkingArea.Y+50);
			}
			this.Location=pointStart;
		}

		private void FillComboJobs() {
			if(!PrefC.IsODHQ) {
				return;
			}
			_isLoading=true;
			comboJobs.Items.Clear();
			_jobLinks = JobLinks.GetForTask(this._taskCur.TaskNum);
			_listJobs = Jobs.GetMany(_jobLinks.Select(x => x.JobNum).ToList());
			foreach(Job job in _listJobs) {
				comboJobs.Items.Add(job.ToString());
			}
			if(_listJobs.Count==0) {
				comboJobs.Items.Add("None");
				if(JobPermissions.IsAuthorized(JobPerm.Concept,true)) {
					butCreateJob.Visible=true;
				}
			}
			comboJobs.Items.Add("Attach");
			comboJobs.SelectedIndex=0;
			labelJobs.Text=Lan.g(this,"Jobs")+" ("+_listJobs.Count+")";
			_isLoading=false;
		}

		private void comboJobs_SelectedIndexChanged(object sender,EventArgs e) {
			if(_isLoading) {
				return;
			}
			if(comboJobs.SelectedIndex<1 && _listJobs.Count==0) {
				return;//selected none
			}
			if(comboJobs.SelectedIndex==comboJobs.Items.Count-1) {
				//Atach new job
				FormJobSearch FormJS = new FormJobSearch();
				FormJS.ShowDialog();
				if(FormJS.DialogResult!=DialogResult.OK || FormJS.SelectedJob==null) {
					return;
				}
				JobLink jobLink = new JobLink();
				jobLink.JobNum=FormJS.SelectedJob.JobNum;
				jobLink.FKey=_taskCur.TaskNum;
				jobLink.LinkType=JobLinkType.Task;
				JobLinks.Insert(jobLink);
				Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,jobLink.JobNum);
				FillComboJobs();
				DataValid.SetInvalidTask(_taskCur.TaskNum,false);
				return;
			}
			FormOpenDental.S_GoToJob(_listJobs[comboJobs.SelectedIndex].JobNum);
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Date Time"),120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"User"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Note"),400);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			_listTaskNotes=TaskNotes.GetForTask(_taskCur.TaskNum);
			//Only do weird logic when editing a task associated with the triage task list.
			if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {
				if(_numNotes==-1) {//Only fill _numNotes here the first time FillGrid is called.  This is used for coloring triage tasks.
					_numNotes=_listTaskNotes.Count;
				}
				if(_pritoryDefNumSelected==_triageBlueNum && _numNotes==0 && _listTaskNotes.Count!=0) {//Blue triage task with an added note
					_pritoryDefNumSelected=_triageWhiteNum;//Change priority to white
					for(int i=0;i<_listTaskPriorities.Count;i++) {
						if(_listTaskPriorities[i].DefNum==_triageWhiteNum) {
							comboTaskPriorities.SelectedIndex=i;//Change selection to the triage white
						}
					}
				}
				else if(_pritoryDefNumSelected==_triageWhiteNum && _numNotes!=0 && _listTaskNotes.Count==0) {//White triage task with note that has been deleted, change it back to blue.
					_pritoryDefNumSelected=_triageBlueNum;
					for(int i=0;i<_listTaskPriorities.Count;i++) {
						if(_listTaskPriorities[i].DefNum==_triageBlueNum) {
							comboTaskPriorities.SelectedIndex=i;//Change selection to the triage blue
						}
					}
				}
				_numNotes=_listTaskNotes.Count;
			}
			for(int i=0;i<_listTaskNotes.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listTaskNotes[i].DateTimeNote.ToShortDateString()+" "+_listTaskNotes[i].DateTimeNote.ToShortTimeString());
				row.Cells.Add(Userods.GetName(_listTaskNotes[i].UserNum));
				row.Cells.Add(_listTaskNotes[i].Note);
				row.Tag=_listTaskNotes[i];
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(OwnedForms.Length>0) {
				MsgBox.Show(this,"One or more task note edit windows are open and must be closed.");
				return;
			}
			FormTaskNoteEdit form=new FormTaskNoteEdit();
			form.TaskNoteCur=(TaskNote)gridMain.Rows[e.Row].Tag;
			form.EditComplete=OnNoteEditComplete_CellDoubleClick;
			form.Show(this);//non-modal subwindow, but if the parent is closed by the user when the child is open, then the child is forced closed along with the parent and after the parent.
		}

		private void OnNoteEditComplete_CellDoubleClick(object sender) {
			NotesChanged=true;
			if(_taskOld.TaskStatus==TaskStatusEnum.Done) {//If task was marked Done when opened, we uncheck the Done checkbox so people can see the changes.
				checkDone.Checked=false;
			}
			FillGrid();
		}

		private void butAddNote_Click(object sender,EventArgs e) {
			if(OwnedForms.Length>0) {
				MsgBox.Show(this,"One or more task note edit windows are open and must be closed.");
				return;
			}
			FormTaskNoteEdit form=new FormTaskNoteEdit();
			form.TaskNoteCur=new TaskNote();
			form.TaskNoteCur.TaskNum=_taskCur.TaskNum;
			form.TaskNoteCur.DateTimeNote=DateTime.Now;//Will be slightly adjusted at server.
			form.TaskNoteCur.UserNum=Security.CurUser.UserNum;
			form.TaskNoteCur.IsNew=true;
			form.TaskNoteCur.Note="";
			form.EditComplete=OnNoteEditComplete_Add;
			form.Show(this);//non-modal subwindow, but if the parent is closed by the user when the child is open, then the child is forced closed along with the parent and after the parent.
		}

		private void OnNoteEditComplete_Add(object sender) {
			NotesChanged=true;
			if(_taskOld.TaskStatus==TaskStatusEnum.Done) {//If task was marked Done when opened, we uncheck the Done checkbox so people can see the changes.
				checkDone.Checked=false;
			}
			FillGrid();
			if(_mightNeedSetRead) {//'new' box is checked
				checkNew.Checked=false;
				_statusChanged=true;
				_mightNeedSetRead=false;//so that the automation won't happen again
			}
		}

		private void checkNew_Click(object sender,EventArgs e) {
			if(checkNew.Checked && checkDone.Checked) {
				checkDone.Checked=false;
			}
			_statusChanged=true;
			_mightNeedSetRead=false;//don't override user's intent
		}

		private void checkDone_Click(object sender,EventArgs e) {
			if(checkNew.Checked && checkDone.Checked) {
				checkNew.Checked=false;
			}
			_mightNeedSetRead=false;//don't override user's intent
		}

		private void FillObject() {
			if(_taskCur.ObjectType==TaskObjectType.None) {
				listObjectType.SelectedIndex=0;
				panelObject.Visible=false;
			}
			else if(_taskCur.ObjectType==TaskObjectType.Patient) {
				listObjectType.SelectedIndex=1;
				panelObject.Visible=true;
				labelObjectDesc.Text=Lan.g(this,"Patient Name");
				if(_taskCur.KeyNum>0) {
					textObjectDesc.Text=Patients.GetPat(_taskCur.KeyNum).GetNameLF();
				}
				else {
					textObjectDesc.Text="";
				}
			}
			else if(_taskCur.ObjectType==TaskObjectType.Appointment) {
				listObjectType.SelectedIndex=2;
				panelObject.Visible=true;
				labelObjectDesc.Text=Lan.g(this,"Appointment Desc");
				if(_taskCur.KeyNum>0) {
					Appointment AptCur=Appointments.GetOneApt(_taskCur.KeyNum);
					if(AptCur==null) {
						textObjectDesc.Text=Lan.g(this,"(appointment deleted)");
					}
					else {
						textObjectDesc.Text=Patients.GetPat(AptCur.PatNum).GetNameLF()
							+"  "+AptCur.AptDateTime.ToString()
							+"  "+AptCur.ProcDescript
							+"  "+AptCur.Note;
					}
				}
				else {
					textObjectDesc.Text="";
				}
			}
		}

		private void butNow_Click(object sender,EventArgs e) {
			textDateTimeEntry.Text=MiscData.GetNowDateTime().ToString();
		}

		private void butNowFinished_Click(object sender,EventArgs e) {
			textDateTimeFinished.Text=MiscData.GetNowDateTime().ToString();
		}

		private void comboReminderRepeat_SelectedIndexChanged(object sender,EventArgs e) {
			RefreshReminderGroup();
		}

		private void textReminderRepeatFrequency_KeyUp(object sender,KeyEventArgs e) {
			RefreshReminderGroup();
		}

		private void RefreshReminderGroup() {
			TaskReminderType taskReminderType=_listTaskReminderTypeNames[comboReminderRepeat.SelectedIndex];
			panelReminderFrequency.Visible=true;
			panelReminderDays.Visible=false;
			datePickerReminder.Visible=false;
			timePickerReminder.Visible=false;
			int reminderFrequency=PIn.Int(textReminderRepeatFrequency.Text,false);
			if(taskReminderType==TaskReminderType.NoReminder) {
				panelReminderFrequency.Visible=false;
			}
			else if(taskReminderType==TaskReminderType.Once) {
				panelReminderFrequency.Visible=false;
				datePickerReminder.Visible=true;
				timePickerReminder.Visible=true;
			}
			else if(taskReminderType==TaskReminderType.Daily) {
				if(reminderFrequency==1) {
					labelRemindFrequency.Text=Lan.g(this,"Day");
				}
				else {
					labelRemindFrequency.Text=Lan.g(this,"Days");
				}
			}
			else if(taskReminderType==TaskReminderType.Weekly) {
				panelReminderDays.Visible=true;
				if(reminderFrequency==1) {
					labelRemindFrequency.Text=Lan.g(this,"Week");
				}
				else {
					labelRemindFrequency.Text=Lan.g(this,"Weeks");
				}
			}
			else if(taskReminderType==TaskReminderType.Monthly) {
				if(reminderFrequency==1) {
					labelRemindFrequency.Text=Lan.g(this,"Month");
				}
				else {
					labelRemindFrequency.Text=Lan.g(this,"Months");
				}
			}
			else if(taskReminderType==TaskReminderType.Yearly) {
				if(reminderFrequency==1) {
					labelRemindFrequency.Text=Lan.g(this,"Year");
				}
				else {
					labelRemindFrequency.Text=Lan.g(this,"Years");
				}
			}
		}

		private void butBlue_Click(object sender,EventArgs e) {
			if(_pritoryDefNumSelected==_triageBlueNum) {//Blue button is clicked while it's already blue
				_pritoryDefNumSelected=_triageWhiteNum;//Change to white.
				for(int i=0;i<_listTaskPriorities.Count;i++) {
					if(_listTaskPriorities[i].DefNum==_triageWhiteNum) {
						comboTaskPriorities.SelectedIndex=i;//Change selection to the triage white
					}
				}	
			}
			else {//Blue button is clicked while it's red or white, simply change it to blue
				_pritoryDefNumSelected=_triageBlueNum;//Change to blue.
				for(int i=0;i<_listTaskPriorities.Count;i++) {
					if(_listTaskPriorities[i].DefNum==_triageBlueNum) {
						comboTaskPriorities.SelectedIndex=i;//Change selection to the triage blue
					}
				}	
			}
		}

		private void butRed_Click(object sender,EventArgs e) {
			if(_pritoryDefNumSelected==_triageRedNum) {//Red button is clicked while it's already red
				_pritoryDefNumSelected=_triageWhiteNum;//Change to white.
				for(int i=0;i<_listTaskPriorities.Count;i++) {
					if(_listTaskPriorities[i].DefNum==_triageWhiteNum) {
						comboTaskPriorities.SelectedIndex=i;//Change combo selection to the triage white
					}
				}	
			}
			else {//Red button is clicked while it's blue or white, simply change it to red
				_pritoryDefNumSelected=_triageRedNum;//Change to red.
				for(int i=0;i<_listTaskPriorities.Count;i++) {
					if(_listTaskPriorities[i].DefNum==_triageRedNum) {
						comboTaskPriorities.SelectedIndex=i;//Change combo selection to the triage red
					}
				}
				if(_taskListCur!=null && _taskListCur.TaskListNum==Tasks.TriageTaskListNum) {
					textDateTimeEntry.Text=MiscData.GetNowDateTime().ToString();
				}
			}
		}

		private void butAutoNote_Click(object sender,EventArgs e) {
			FormAutoNoteCompose FormA=new FormAutoNoteCompose();
			FormA.ShowDialog();
			if(FormA.DialogResult==DialogResult.OK) {
				textDescript.AppendText(FormA.CompletedNote);
			}
		}

		private void splitContainerNoFlicker1_SplitterMoved(object sender,SplitterEventArgs e) {
			textDescript.Invalidate();//We do this so that the scrollbar will not disappear. The size is set through the anchors.
		}

		///<summary>This event is fired whenever the combo box is changed manually or the index is changed programmatically.</summary>
		private void comboTaskPriorities_SelectedIndexChanged(object sender,EventArgs e) {
			_pritoryDefNumSelected=_listTaskPriorities[comboTaskPriorities.SelectedIndex].DefNum;
			butColor.BackColor=Defs.GetColor(DefCat.TaskPriorities,_pritoryDefNumSelected);//Change the color swatch so people know the priority's color
		}

		private void comboTaskPriorities_SelectionChangeCommitted(object sender,EventArgs e) {
			if(PrefC.IsODHQ 
				//Changing the priority to 'Red' from another priority for a Triage task
				&& _listTaskPriorities[comboTaskPriorities.SelectedIndex].DefNum==_triageRedNum 
				&& _taskCur.PriorityDefNum!=_triageRedNum
				&& _taskListCur!=null && _taskListCur.TaskListNum==Tasks.TriageTaskListNum) 
			{
				textDateTimeEntry.Text=MiscData.GetNowDateTime().ToString();
			}
		}

		private void listObjectType_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			if(_taskCur.KeyNum>0) {
				if(!MsgBox.Show(this,true,"The linked object will no longer be attached.  Continue?")) {
					FillObject();
					return;
				}
			}
			_taskCur.KeyNum=0;
			_taskCur.ObjectType=(TaskObjectType)listObjectType.SelectedIndex;
			FillObject();
		}

		private void butAudit_Click(object sender,EventArgs e) {
			FormTaskHist FormTH=new FormTaskHist();
			FormTH.TaskNumCur=_taskCur.TaskNum;
			if(Tasks.GetOne(_taskCur.TaskNum)==null) {//attempt to get this task from the DB to verify it is still valid
				MsgBox.Show(this,"Task has been deleted, no history can be retrieved.");
				return;
			}
			FormTH.Show();
		}

		private void butChange_Click(object sender,System.EventArgs e) {
			FormPatientSelect FormPS=new FormPatientSelect();
			FormPS.SelectionModeOnly=true;
			FormPS.ShowDialog();
			if(FormPS.DialogResult!=DialogResult.OK) {
				return;
			}
			if(_taskCur.ObjectType==TaskObjectType.Patient) {
				_taskCur.KeyNum=FormPS.SelectedPatNum;
			}
			if(_taskCur.ObjectType==TaskObjectType.Appointment) {
				FormApptsOther FormA=new FormApptsOther(FormPS.SelectedPatNum);
				FormA.SelectOnly=true;
				FormA.ShowDialog();
				if(FormA.DialogResult==DialogResult.Cancel) {
					return;
				}
				_taskCur.KeyNum=FormA.AptNumsSelected[0];
			}
			FillObject();
		}

		private void butGoto_Click(object sender,System.EventArgs e) {
			if(OwnedForms.Length>0) {
				MsgBox.Show(this,"One or more task note edit windows are open and must be closed.");
				return;
			}
			if(!SaveCur()) {
				return;
			}
			GotoType=_taskCur.ObjectType;
			GotoKeyNum=_taskCur.KeyNum;
			DialogResult=DialogResult.OK;
			Close();
			FormOpenDental.S_TaskGoTo(GotoType,GotoKeyNum);
		}

		private void butChangeUser_Click(object sender,EventArgs e) {
			FormLogOn FormChangeUser=new FormLogOn();
			FormChangeUser.IsSimpleSwitch=true;
			FormChangeUser.ShowDialog();
			if(FormChangeUser.DialogResult==DialogResult.OK) {
				_taskCur.UserNum=FormChangeUser.CurUserSimpleSwitch.UserNum; //assign task new UserNum
				textUser.Text=Userods.GetName(_taskCur.UserNum); //update user textbox on task.
			}
		}

		private void textDescript_TextChanged(object sender,EventArgs e) {
			if(_mightNeedSetRead) {//'new' box is checked
				checkNew.Checked=false;
				_statusChanged=true;
				_mightNeedSetRead=false;//so that the automation won't happen again
			}
			if(_taskOld.TaskStatus==TaskStatusEnum.Done && textDescript.Text!=_taskOld.Descript) {
				checkDone.Checked=false;
			}
		}

		private void butCopy_Click(object sender,EventArgs e) {
			try {
				Clipboard.SetText(CreateCopyTask());
			}
			catch(Exception ex) {
				MsgBox.Show(this,"Could not copy contents to the clipboard.  Please try again.");
				ex.DoNothing();
				return;
			}
			Tasks.TaskEditCreateLog(Lan.g(this,"Copied Task Note"),_taskCur);
		}

		private string CreateCopyTask() {
			string taskText=
				Lan.g(this,"Tasknum")+" #"+_taskCur.TaskNum //tasknum
				+((_taskCur.ObjectType==TaskObjectType.Patient && _taskCur.KeyNum!=0) ? (" - "+Lan.g(this,"Patnum")+" #"+_taskCur.KeyNum) : ("")) //patnum
				+"\r\n"+_taskCur.DateTimeEntry.ToShortDateString() //date
				+" "+_taskCur.DateTimeEntry.ToShortTimeString() //time
				+(textObjectDesc.Visible?" - "+textObjectDesc.Text:"")//patient name, time, etc
				+" - "+textUser.Text //user name
				+" - "+textDescript.Text; //task desc
			for(int i=0;i<_listTaskNotes.Count;i++) {
				taskText+="\r\n--------------------------------------------------\r\n";
				taskText+="=="+Userods.GetName(_listTaskNotes[i].UserNum)+" - "+_listTaskNotes[i].DateTimeNote.ToShortDateString()+" "+_listTaskNotes[i].DateTimeNote.ToShortTimeString()+" - "+_listTaskNotes[i].Note;
			}
			return taskText;
		}

		private void butCreateJob_Click(object sender,EventArgs e) {
			Job jobNew=new Job();
			List<string> categoryList=Enum.GetNames(typeof(JobCategory)).ToList();
			//Queries can't be created from here
			categoryList.Remove("Query");
			InputBox categoryChoose=new InputBox("Choose a job category",categoryList);
			if(categoryChoose.ShowDialog()!=DialogResult.OK) {
				return;
			}
			if(categoryChoose.comboSelection.SelectedIndex==-1) {
				MsgBox.Show(this,"You must choose a category to create a job.");
				return;
			}
			jobNew.Category=(JobCategory)Enum.GetNames(typeof(JobCategory)).ToList().IndexOf(categoryChoose.comboSelection.SelectedItem.ToString());
			InputBox titleBox=new InputBox("Provide a brief title for the job.");
			if(titleBox.ShowDialog()!=DialogResult.OK) {
				return;
			}
			if(String.IsNullOrEmpty(titleBox.textResult.Text)) {
				MsgBox.Show(this,"You must type a title to create a job.");
				return;
			}
			List<Def> listJobPriorities=Defs.GetDefsForCategory(DefCat.JobPriorities,true);
			if(listJobPriorities.Count==0) {
				MsgBox.Show(this,"You have no priorities setup in definitions.");
				return;
			}
			jobNew.Title=titleBox.textResult.Text;
			long priorityNum=0;
			priorityNum=listJobPriorities.FirstOrDefault(x => x.ItemValue.Contains("JobDefault")).DefNum;
			if(jobNew.Category.In(JobCategory.Bug,JobCategory.Conversion)) {
				priorityNum=listJobPriorities.FirstOrDefault(x => x.ItemValue.Contains("BugDefault")).DefNum;
				jobNew.Requirements=CreateCopyTask();
			}
			else {
				jobNew.Requirements=CreateCopyTask();
			}
			jobNew.Priority=priorityNum==0?listJobPriorities.First().DefNum:priorityNum;
			jobNew.PhaseCur=JobPhase.Concept;
			jobNew.UserNumConcept=Security.CurUser.UserNum;
			JobLink jobLinkNew=new JobLink();
			Bug bugNew=new Bug();
			if(jobNew.Category==JobCategory.Bug) {
				jobLinkNew.LinkType=JobLinkType.Bug;
				bugNew=Bugs.GetNewBugForUser();
				InputBox bugDescription=new InputBox("Provide a brief description for the bug. This will appear in the bug tracker.",jobNew.Title);
				if(bugDescription.ShowDialog()!=DialogResult.OK) {
					return;
				}
				if(String.IsNullOrEmpty(bugDescription.textResult.Text)) {
					MsgBox.Show(this,"You must type a description to create a bug.");
					return;
				}
				FormVersionPrompt FormVP=new FormVersionPrompt("Enter versions found");
				FormVP.ShowDialog();
				if(FormVP.DialogResult!=DialogResult.OK || string.IsNullOrEmpty(FormVP.VersionText)) {
					MsgBox.Show(this,"You must enter a version to create a bug.");
					return;
				}
				bugNew.Status_=BugStatus.Accepted;
				bugNew.VersionsFound=FormVP.VersionText;
				bugNew.Description=bugDescription.textResult.Text;
			}
			JobLink jobLinkTask=new JobLink();
			jobLinkTask.LinkType=JobLinkType.Task;
			jobLinkTask.FKey=_taskCur.TaskNum;
			Jobs.Insert(jobNew);
			jobLinkNew.JobNum=jobNew.JobNum;
			jobLinkTask.JobNum=jobNew.JobNum;
			if(jobNew.Category==JobCategory.Bug) {
				jobLinkNew.FKey=Bugs.Insert(bugNew);
				JobLinks.Insert(jobLinkNew);
			}
			JobLinks.Insert(jobLinkTask);
			Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,jobNew.JobNum);
			FillComboJobs();
			FormOpenDental.S_GoToJob(jobNew.JobNum);
			DataValid.SetInvalidTask(_taskCur.TaskNum,false);
		}

		public void OnTaskEdited() {
			Logger.LogToPath("",LogPath.Signals,LogPhase.Start,"TaskNum: "+_taskCur.TaskNum.ToString());
			Task taskDb=Tasks.GetOne(_taskCur.TaskNum);
			if(!taskDb.Equals(_taskOld)) {
				butRefresh.Visible=true;
				labelTaskChanged.Visible=true;
			}
			_taskOld=taskDb;
			FillGrid();
			FillComboJobs();
			FillObject();
			Logger.LogToPath("",LogPath.Signals,LogPhase.End,"TaskNum: "+_taskCur.TaskNum.ToString());
		}

		///<summary>Does validation and then updates the _taskCur object with the current content of the TaskEdit window.</summary>
		private bool SaveCur() {
			if(textDateTask.errorProvider1.GetError(textDateTask)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return false;
			}
			DateTime dateTimeEntry=DateTime.MinValue;
			TaskReminderType taskReminderType=_listTaskReminderTypeNames[comboReminderRepeat.SelectedIndex];
			if(textDateTimeEntry.Text!="" || taskReminderType!=TaskReminderType.NoReminder) {//Reminders always require a DateTimeEntry
				try {
					dateTimeEntry=DateTime.Parse(textDateTimeEntry.Text);
				}
				catch {
					MsgBox.Show(this,"Please fix Date/Time Entry.");
					return false;
				}
			}
			if(textDateTimeFinished.Text!="") {
				try {
					DateTime.Parse(textDateTimeFinished.Text);
				}
				catch {
					MsgBox.Show(this,"Please fix Date/Time Finished.");
					return false;
				}
			}
			if(_taskCur.TaskListNum==-1) {
				MsgBox.Show(this,"Since no task list is selected, the Send To button must be used.");
				return false;
			}
			if(textDescript.Text=="") {
				MsgBox.Show(this,"Please enter a description.");
				return false;
			}
			if(taskReminderType!=TaskReminderType.NoReminder && !PrefC.GetBool(PrefName.TasksUseRepeating)) {//Is a reminder and not using legacy task system
				if(taskReminderType!=TaskReminderType.Once &&
					(textReminderRepeatFrequency.errorProvider1.GetError(textReminderRepeatFrequency)!="" || PIn.Int(textReminderRepeatFrequency.Text) < 1))
				{
					MsgBox.Show(this,"Reminder frequency must be a positive number.");
					return false;
				}
				if(taskReminderType==TaskReminderType.Weekly && ! checkReminderRepeatMonday.Checked && !checkReminderRepeatTuesday.Checked
					&& !checkReminderRepeatWednesday.Checked && !checkReminderRepeatThursday.Checked && !checkReminderRepeatFriday.Checked
					&& !checkReminderRepeatSaturday.Checked && !checkReminderRepeatSunday.Checked)
				{
					MsgBox.Show(this,"Since the weekly reminder repeat option is selected, at least one day option must be chosen.");
					return false;
				}
				if(checkReminderRepeatMonday.Checked) {
					taskReminderType|=TaskReminderType.Monday;
				}
				if(checkReminderRepeatTuesday.Checked) {
					taskReminderType|=TaskReminderType.Tuesday;
				}
				if(checkReminderRepeatWednesday.Checked) {
					taskReminderType|=TaskReminderType.Wednesday;
				}
				if(checkReminderRepeatThursday.Checked) {
					taskReminderType|=TaskReminderType.Thursday;
				}
				if(checkReminderRepeatFriday.Checked) {
					taskReminderType|=TaskReminderType.Friday;
				}
				if(checkReminderRepeatSaturday.Checked) {
					taskReminderType|=TaskReminderType.Saturday;
				}
				if(checkReminderRepeatSunday.Checked) {
					taskReminderType|=TaskReminderType.Sunday;
				}
				_taskCur.ReminderType=taskReminderType;
				_taskCur.ReminderFrequency=0;
				if(taskReminderType!=TaskReminderType.Once) {
					_taskCur.ReminderFrequency=PIn.Int(textReminderRepeatFrequency.Text);
				}
				if(String.IsNullOrEmpty(_taskCur.ReminderGroupId)) {//Make a new ID if it's blank no matter what.  Could be an old task being changed.
					Tasks.SetReminderGroupId(_taskCur);
				}
			}
			else {
				_taskCur.ReminderType=TaskReminderType.NoReminder;
				_taskCur.ReminderGroupId="";
			}
			//Techs want to be notified of any changes made to completed tasks.
			//Check if the task list changed on a task marked Done.
			if(_taskCur.TaskListNum!=_taskOld.TaskListNum	&& _taskOld.TaskStatus==TaskStatusEnum.Done) {
				//Forcing the status to viewed will put the task in other user's "New for" task list but not the user that made the change.
				_taskCur.TaskStatus=TaskStatusEnum.Viewed;
				checkDone.Checked=false;
			}
			if(checkDone.Checked) {
				_taskCur.TaskStatus=TaskStatusEnum.Done;//global even if new status is tracked by user
				TaskUnreads.DeleteForTask(_taskCur.TaskNum);//clear out taskunreads. We have too many tasks to read the done ones.
			}
			else {//because it can't be both new and done.
				if(PrefC.GetBool(PrefName.TasksNewTrackedByUser)) {
					if(_taskCur.TaskStatus==TaskStatusEnum.Done) {
						_taskCur.TaskStatus=TaskStatusEnum.Viewed;
					}
					//This is done explicitly instead of automatically like it was the old way
					if(!_startedInOthersInbox) {
						//Because the task could have been modified by another user at this point
						//RefreshTask();
						if(checkNew.Checked) {
							TaskUnreads.SetUnread(Security.CurUser.UserNum,_taskCur.TaskNum);
						}
						else {
							TaskUnreads.SetRead(Security.CurUser.UserNum,_taskCur.TaskNum);
						}
					}
				}
				else {//tracked globally, the old way
					if(checkNew.Checked) {
						_taskCur.TaskStatus=TaskStatusEnum.New;
					}
					else {
						_taskCur.TaskStatus=TaskStatusEnum.Viewed;
					}
				}
			}
			//UserNum no longer allowed to change automatically
			//if(resetUser && TaskCur.Descript!=textDescript.Text){
			//	TaskCur.UserNum=Security.CurUser.UserNum;
			//}
			if(taskReminderType==TaskReminderType.Once) {
				_taskCur.DateTimeEntry=new DateTime(datePickerReminder.Value.Date.Year,datePickerReminder.Value.Date.Month,datePickerReminder.Value.Date.Day,
					timePickerReminder.Value.TimeOfDay.Hours,timePickerReminder.Value.TimeOfDay.Minutes,timePickerReminder.Value.TimeOfDay.Seconds);
			}
			else {
				_taskCur.DateTimeEntry=PIn.DateT(textDateTimeEntry.Text);
			}
			if(_taskCur.TaskStatus==TaskStatusEnum.Done && textDateTimeFinished.Text=="") {
				_taskCur.DateTimeFinished=DateTime.Now;
			}
			else {
				_taskCur.DateTimeFinished=PIn.DateT(textDateTimeFinished.Text);
			}
			_taskCur.Descript=textDescript.Text;
			_taskCur.DateTask=PIn.Date(textDateTask.Text);
			_taskCur.DateType=(TaskDateType)comboDateType.SelectedIndex;
			if(!checkFromNum.Checked) {//user unchecked the box. Never allowed to check if initially unchecked
				_taskCur.FromNum=0;
			}
			//ObjectType already handled
			//Cur.KeyNum already handled
			_taskCur.PriorityDefNum=_pritoryDefNumSelected;
			try {
				if(IsNew) {
					_taskCur.IsNew=true;
					Tasks.Update(_taskCur,_taskOld);
				}
				else {
					if(butRefresh.Visible) {	
						//force them to refresh before pressing ok.
						if(MsgBox.Show(this,MsgBoxButtons.YesNo,"There have been changes to the task since it has been loaded. "+
						" You must refresh before saving. Would you like to refresh now?")) 
						{
							RefreshTask();
						}
						return false;					
					}
					if(!_taskCur.Equals(_taskOld)) {//If user clicks OK without making any changes, then skip.
						Cursor=Cursors.WaitCursor;
						Tasks.Update(_taskCur,_taskOld);//if task has already been altered, then this is where it will fail.
						Cursor=Cursors.Default;
					}
					if(!_taskCur.Equals(_taskOld) || NotesChanged) {//We want to make a TaskHist entry if notes were changed as well as if the task was changed.
						TaskHist taskHist=new TaskHist(_taskOld);
						taskHist.UserNumHist=Security.CurUser.UserNum;
						taskHist.IsNoteChange=NotesChanged;
						TaskHists.Insert(taskHist);
					}
				}
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MessageBox.Show(ex.Message);
				return false;
			}
			return true;
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			RefreshTask();
		}

		private void RefreshTask() {
			if(_taskCur==null) {
				MsgBox.Show(this,"This task is in an invalid state. The task will now be closed so it can be opened again in a valid state.");
				DialogResult=DialogResult.Abort;
				Close();
				return;
			}
			_taskCur=Tasks.GetOne(_taskCur.TaskNum);
			if(_taskCur==null) {
				MsgBox.Show(this,"This task has been deleted and must be closed.");
				DialogResult=DialogResult.Abort;
				Close();
				return;
			}
			_taskListCur=TaskLists.GetOne(_taskCur.TaskListNum);
			LoadTask();
			butRefresh.Visible=false;
			labelTaskChanged.Visible=false;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			//NOTE: Any changes here need to be made to UserControlTasks.Delete_Clicked() as well.
			if(OwnedForms.Length>0) {
				MsgBox.Show(this,"One or more task note edit windows are open and must be closed.");
				return;
			}
			if(!IsNew) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete?")) {
					return;
				}
				if(Tasks.GetOne(_taskCur.TaskNum)==null) {
					MsgBox.Show(this,"Task already deleted.");//if task has remained open and has become stale on a workstation.
					butDelete.Enabled=false;
					butOK.Enabled=false;
					butSend.Enabled=false;
					butAddNote.Enabled=false;
					Text+=" - {"+Lan.g(this,"Deleted")+"}";
					return;
				}
				//TaskListNum=-1 is only possible if it's new.  This will never get hit if it's new.
				if(_taskCur.TaskListNum==0) {
					Tasks.TaskEditCreateLog(Lan.g(this,"Deleted task"),_taskCur);
				}
				else {
					string logText=Lan.g(this,"Deleted task from tasklist");
					TaskList tList=TaskLists.GetOne(_taskCur.TaskListNum);
					if(tList!=null) {
						logText+=" "+tList.Descript;
					}
					else {
						logText+=". Task list no longer exists";
					}
					logText+=".";
					Tasks.TaskEditCreateLog(logText,_taskCur);
				}
			}
			Tasks.Delete(_taskCur.TaskNum);//always do it this way to clean up all four tables
			DataValid.SetInvalidTask(_taskCur.TaskNum,false);//no popup
			TaskHist taskHistory=new TaskHist(_taskOld);
			taskHistory.IsNoteChange=NotesChanged;
			taskHistory.UserNum=Security.CurUser.UserNum;
			TaskHists.Insert(taskHistory);
			SecurityLogs.MakeLogEntry(Permissions.TaskEdit,0,"Task "+POut.Long(_taskCur.TaskNum)+" deleted",0);
			DialogResult=DialogResult.OK;
			Close();
		}

		private void butReply_Click(object sender,EventArgs e) {
			//This can't happen if IsNew
			//This also can't happen if the task is mine with no replies.
			//Button not visible unless a ReplyToUserNum has been calculated successfully.
			if(OwnedForms.Length>0) {
				MsgBox.Show(this,"One or more task note edit windows are open and must be closed.");
				return;
			}
			long inbox=Userods.GetInbox(_replyToUserNum);
			if(inbox==0) {
				MsgBox.Show(this,"No inbox has been set up for this user yet.");
				return;
			}
			if(!NotesChanged && textDescript.Text==_taskCur.Descript) {//nothing changed
				FormTaskNoteEdit form=new FormTaskNoteEdit();
				form.TaskNoteCur=new TaskNote();
				form.TaskNoteCur.TaskNum=_taskCur.TaskNum;
				form.TaskNoteCur.DateTimeNote=DateTime.Now;//Will be slightly adjusted at server.
				form.TaskNoteCur.UserNum=Security.CurUser.UserNum;
				form.TaskNoteCur.IsNew=true;
				form.TaskNoteCur.Note="";
				form.EditComplete=OnNoteEditComplete_Reply;
				form.Show(this);//non-modal subwindow, but if the parent is closed by the user when the child is open, then the child is forced closed along with the parent and after the parent.
				return;
			}
			_taskCur.TaskListNum=inbox;
			if(!SaveCur()) {
				return;
			}
			DataValid.SetInvalidTask(_taskCur.TaskNum,true);//popup
			DialogResult=DialogResult.OK;
			Close();
		}

		private void OnNoteEditComplete_Reply(object sender) {
			if(_mightNeedSetRead) {//'new' box is checked
				checkNew.Checked=false;
				_statusChanged=true;
				_mightNeedSetRead=false;//so that the automation won't happen again
			}
			_taskCur.TaskListNum=Userods.GetInbox(_replyToUserNum);
			if(!SaveCur()) {
				return;
			}
			DataValid.SetInvalidTask(_taskCur.TaskNum,true);//popup
			DialogResult=DialogResult.OK;
			Close();
		}

		///<summary>Send to another user.</summary>
		private void butSend_Click(object sender,EventArgs e) {
			//This button is always present.
			if(OwnedForms.Length>0) {
				MsgBox.Show(this,"One or more task note edit windows are open and must be closed.");
				return;
			}
			if(listObjectType.SelectedIndex==(int)TaskObjectType.Patient) {
				FormTaskListSelect FormT=new FormTaskListSelect(TaskObjectType.Patient,IsNew);
				FormT.ShowDialog();
				if(FormT.DialogResult!=DialogResult.OK) {
					return;
				}
				_taskCur.TaskListNum=FormT.ListSelectedLists[0];
				_taskListCur=TaskLists.GetOne(_taskCur.TaskListNum);
				textTaskList.Text=_taskListCur.Descript;
				if(!SaveCur()) {
					return;
				}
				SaveCopy(FormT.ListSelectedLists.Skip(1).ToList());
			}
			else if(listObjectType.SelectedIndex==(int)TaskObjectType.Appointment) {
				FormTaskListSelect FormT=new FormTaskListSelect(TaskObjectType.Appointment,IsNew);
				FormT.ShowDialog();
				if(FormT.DialogResult!=DialogResult.OK) {
					return;
				}
				_taskCur.TaskListNum=FormT.ListSelectedLists[0];
				_taskListCur=TaskLists.GetOne(_taskCur.TaskListNum);
				textTaskList.Text=_taskListCur.Descript;
				if(!SaveCur()) {
					return;
				}
				SaveCopy(FormT.ListSelectedLists.Skip(1).ToList());
			}
			else {//to an in-box
				FormTaskSendUser FormT=new FormTaskSendUser(IsNew);
				FormT.ShowDialog();
				if(FormT.DialogResult!=DialogResult.OK) {
					return;
				}
				_taskCur.TaskListNum=FormT.ListSelectedLists[0];
				_taskListCur=TaskLists.GetOne(_taskCur.TaskListNum);
				textTaskList.Text=_taskListCur.Descript;
				if(!SaveCur()) {
					return;
				}
				SaveCopy(FormT.ListSelectedLists.Skip(1).ToList());
			}
			//Check for changes.  If something changed, send a signal.
			if(NotesChanged || !_taskCur.Equals(_taskOld) || _statusChanged) {
				DataValid.SetInvalidTask(_taskCur.TaskNum,true);//popup
			}
			DialogResult=DialogResult.OK;
			Close();
		}

		///<summary>Saves a copy of the task</summary>
		private bool SaveCopy(List<long> listTaskListNums) {
			foreach(long taskListNum in listTaskListNums) {
				Task taskCopy=_taskCur.Copy();
				taskCopy.TaskListNum=taskListNum;
				taskCopy.IsUnread=true;
				taskCopy.ReminderGroupId="";
				if(taskCopy.ReminderType!=TaskReminderType.NoReminder && !PrefC.GetBool(PrefName.TasksUseRepeating)) {//Make a new ID if it's blank no matter what.  Could be an old task being changed.
					Tasks.SetReminderGroupId(taskCopy);
				}
				try {
					taskCopy.IsNew=true;
					Tasks.Insert(taskCopy);
					foreach(TaskNote note in _listTaskNotes) {
						TaskNote noteCopy=note.Copy();
						noteCopy.TaskNum=taskCopy.TaskNum;
						TaskNotes.Insert(noteCopy);
					}
				}
				catch(Exception ex) {
					MessageBox.Show(ex.Message);
					return false;
				}
				//tg:I don't know why this was here.  You just copied the task, you don't want it to be marked new for you.
				//Other users get Unreads in SetInvalidTask.
				//if(PrefC.GetBool(PrefName.TasksNewTrackedByUser)) {
				//	TaskUnreads.SetUnread(Security.CurUser.UserNum,taskCopy.TaskNum);
				//}
				DataValid.SetInvalidTask(taskCopy.TaskNum,true);//popup
			}
			return true;
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(OwnedForms.Length>0) {
				MsgBox.Show(this,"One or more task note edit windows are open and must be closed.");
				return;
			}
			if(!SaveCur()) {//If user clicked OK without changing anything, then this will have no effect.
				return;
			}
			if(_taskCur.Equals(_taskOld) && !_statusChanged) {//if there were no changes, then don't bother with the signal
				DialogResult=DialogResult.OK;
				Close();
				return;
			}
			if(IsNew || textDescript.Text!=_taskCur.Descript //notes or descript changed
				|| (NotesChanged && _taskOld.TaskStatus==TaskStatusEnum.Done) //Because the taskunread would not have been inserted when saving the note
				|| (_taskOld.ReminderType==TaskReminderType.NoReminder && _taskCur.ReminderType!=TaskReminderType.NoReminder)) //Add taskunread for when "due"
			{ 
				DataValid.SetInvalidTask(_taskCur.TaskNum,true);//popup
			}
			else {
				DataValid.SetInvalidTask(_taskCur.TaskNum,false);//no popup
			}
			DialogResult=DialogResult.OK;
			Close();
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			if(OwnedForms.Length>0) {
				MsgBox.Show(this,"One or more task note edit windows are open and must be closed.");
				return;
			}
			DialogResult=DialogResult.Cancel;
			Close();
		}

		private void FormTaskEdit_FormClosing(object sender,FormClosingEventArgs e) {
			if(DialogResult==DialogResult.Abort) {
				return;
			}
			if(DialogResult==DialogResult.None && OwnedForms.Length>0) {//This can only happen if the user closes the window using the X in the upper right.
				MsgBox.Show(this,"One or more task note edit windows are open and must be closed.");
				e.Cancel=true;
				return;
			}
			if(PrefC.GetBool(PrefName.TasksNewTrackedByUser)) {
				//No more automation here
			}
			else {
				if(Security.CurUser!=null) {//Because tasks are modal, user may log off and this form may close with no user.
					TaskUnreads.SetRead(Security.CurUser.UserNum,_taskCur.TaskNum);//no matter why it was closed
				}
			}
			if(DialogResult==DialogResult.OK) {
				return;
			}
			if(IsNew) {
				Tasks.Delete(_taskCur.TaskNum);
				SecurityLogs.MakeLogEntry(Permissions.TaskEdit,0,"Task "+POut.Long(_taskCur.TaskNum)+" deleted",0);
			}
			else if(NotesChanged) {//Note changed and dialogue result was not OK
				//This should only ever be hit if the user clicked cancel or X.  Everything else will have dialogue result OK and exit above.
				//Make a TaskHist entry to note that the task notes were changed.
				TaskHist taskHist = new TaskHist(_taskOld);
				taskHist.UserNumHist=Security.CurUser.UserNum;
				taskHist.IsNoteChange=true;
				TaskHists.Insert(taskHist);
				//Task has already been invalidated in FromTaskNoteEdit when the Note was added/edited.  Other Users have already been notified the task changed.
				//Do not send new TaskInvalid signal.
			}
			//If a note was added to a Done task and the user hits cancel, the task status is set to Viewed because the note is still there and the task didn't move lists.
			if(NotesChanged && _taskOld.TaskStatus==TaskStatusEnum.Done) {//notes changed on a task marked Done when the task was opened.
				if(checkDone.Checked) {//Will only happen when the Done checkbox has been manually re-checked by the user.
					return;
				}
				_taskCur.TaskStatus=TaskStatusEnum.Viewed;
				try {
					Tasks.Update(_taskCur,_taskOld);//if task has already been altered, then this is where it will fail.
				}
				catch {
					return;//Silently leave because the user could be trying to cancel out of a task that had been edited by another user.
				}
				DataValid.SetInvalidTask(_taskCur.TaskNum,true);//popup
			}
		}
	}
}