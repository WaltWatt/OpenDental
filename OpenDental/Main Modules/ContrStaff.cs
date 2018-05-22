using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentalCloud;
using OpenDentBusiness;

namespace OpenDental{

	///<summary></summary>
	public class ContrStaff : System.Windows.Forms.UserControl{
		private OpenDental.UI.Button butTimeCard;
		private System.Windows.Forms.ListBox listStatus;
		private System.Windows.Forms.Label textTime;
		private System.Windows.Forms.Timer timer1;
		private OpenDental.UI.Button butClockIn;
		private OpenDental.UI.Button butClockOut;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox textMessage;
		private System.Windows.Forms.Label labelCurrentTime;
    private System.ComponentModel.IContainer components;
		private OpenDental.UI.Button butSendClaims;
		private OpenDental.UI.Button butTasks;
		private OpenDental.UI.Button butBackup;
		private OpenDental.UI.ODGrid gridEmp;
		private System.Windows.Forms.GroupBox groupBox3;
		private OpenDental.UI.Button butDeposit;
		private OpenDental.UI.Button butBreaks;
		private OpenDental.UI.Button butBilling;
		private OpenDental.UI.Button butAccounting;
		private Label label7;
		private ListBox listMessages;
		private Label label5;
		private ListBox listExtras;
		private Label label4;
		private ListBox listFrom;
		private Label label3;
		private ListBox listTo;
		private Label label1;
		private ODGrid gridMessages;
		private CheckBox checkIncludeAck;
		///<summary>Server time minus local computer time, usually +/- 1 or 2 minutes</summary>
		private TimeSpan TimeDelta;
		private OpenDental.UI.Button butSend;
		private Label label6;
		private ComboBox comboViewUser;
		private Label labelDays;
		private TextBox textDays;
		/////<summary></summary>
		//[Category("Data"),Description("Occurs when user changes current patient, usually by clicking on the Select Patient button.")]
		//public event PatientSelectedEventHandler PatientSelected=null;
		///<summary>Collection of SigMessages</summary>
		private List<SigMessage> _listSigMessages;
		private SigElementDef[] sigElementDefUser;
		private SigElementDef[] sigElementDefExtras;
		private Label labelSending;
		private Timer timerSending;
		private ErrorProvider errorProvider1=new ErrorProvider();
		private OpenDental.UI.Button butAck;
		private SigElementDef[] sigElementDefMessages;
		private OpenDental.UI.Button butSupply;
		private Employee EmployeeCur;
		private FormBilling FormB;
		private FormClaimsSend FormCS;
		private UI.Button butClaimPay;
		private UI.Button butManage;
		private long PatCurNum;
		private UI.Button butEmailInbox;
		//private bool InitializedOnStartup;
		///<summary>This is public so that FormOpenDental can access it.</summary>
		public FormTasks FormT;
		public FormAccounting FormA;
		private UI.Button butImportInsPlans;
		private List<Employee> _listEmployees=new List<Employee>();
		private UI.Button butEras;
		private FormEtrans834Import FormE834I=null;
    private FormEmailInbox FormEmailInbox=null;
		private UI.Button butViewSched;
		private UI.Button butManageAR;
		private FormArManager _formAR;

		///<summary></summary>
		public ContrStaff(){
			Logger.openlog.Log("Initializing management module...",Logger.Severity.INFO);
			InitializeComponent();
			this.listStatus.Click += new System.EventHandler(this.listStatus_Click);
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

		#region Component Designer generated code

		private void InitializeComponent(){
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContrStaff));
			this.listStatus = new System.Windows.Forms.ListBox();
			this.textTime = new System.Windows.Forms.Label();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butViewSched = new OpenDental.UI.Button();
			this.butManage = new OpenDental.UI.Button();
			this.butBreaks = new OpenDental.UI.Button();
			this.gridEmp = new OpenDental.UI.ODGrid();
			this.labelCurrentTime = new System.Windows.Forms.Label();
			this.butClockOut = new OpenDental.UI.Button();
			this.butTimeCard = new OpenDental.UI.Button();
			this.butClockIn = new OpenDental.UI.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.listMessages = new System.Windows.Forms.ListBox();
			this.butSend = new OpenDental.UI.Button();
			this.butAck = new OpenDental.UI.Button();
			this.labelSending = new System.Windows.Forms.Label();
			this.textDays = new System.Windows.Forms.TextBox();
			this.labelDays = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.comboViewUser = new System.Windows.Forms.ComboBox();
			this.gridMessages = new OpenDental.UI.ODGrid();
			this.checkIncludeAck = new System.Windows.Forms.CheckBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.listExtras = new System.Windows.Forms.ListBox();
			this.label4 = new System.Windows.Forms.Label();
			this.listFrom = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.listTo = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textMessage = new System.Windows.Forms.TextBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.butEras = new OpenDental.UI.Button();
			this.butImportInsPlans = new OpenDental.UI.Button();
			this.butEmailInbox = new OpenDental.UI.Button();
			this.butSupply = new OpenDental.UI.Button();
			this.butClaimPay = new OpenDental.UI.Button();
			this.butBilling = new OpenDental.UI.Button();
			this.butAccounting = new OpenDental.UI.Button();
			this.butBackup = new OpenDental.UI.Button();
			this.butDeposit = new OpenDental.UI.Button();
			this.butSendClaims = new OpenDental.UI.Button();
			this.butTasks = new OpenDental.UI.Button();
			this.butManageAR = new OpenDental.UI.Button();
			this.timerSending = new System.Windows.Forms.Timer(this.components);
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// listStatus
			// 
			this.listStatus.Location = new System.Drawing.Point(367, 217);
			this.listStatus.Name = "listStatus";
			this.listStatus.Size = new System.Drawing.Size(107, 43);
			this.listStatus.TabIndex = 12;
			// 
			// textTime
			// 
			this.textTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textTime.Location = new System.Drawing.Point(365, 138);
			this.textTime.Name = "textTime";
			this.textTime.Size = new System.Drawing.Size(109, 21);
			this.textTime.TabIndex = 17;
			this.textTime.Text = "12:00:00 PM";
			this.textTime.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butViewSched);
			this.groupBox1.Controls.Add(this.butManage);
			this.groupBox1.Controls.Add(this.butBreaks);
			this.groupBox1.Controls.Add(this.gridEmp);
			this.groupBox1.Controls.Add(this.labelCurrentTime);
			this.groupBox1.Controls.Add(this.listStatus);
			this.groupBox1.Controls.Add(this.butClockOut);
			this.groupBox1.Controls.Add(this.butTimeCard);
			this.groupBox1.Controls.Add(this.textTime);
			this.groupBox1.Controls.Add(this.butClockIn);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(349, 5);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(510, 272);
			this.groupBox1.TabIndex = 18;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Time Clock";
			// 
			// butViewSched
			// 
			this.butViewSched.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butViewSched.Autosize = true;
			this.butViewSched.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butViewSched.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butViewSched.CornerRadius = 4F;
			this.butViewSched.Location = new System.Drawing.Point(366, 94);
			this.butViewSched.Name = "butViewSched";
			this.butViewSched.Size = new System.Drawing.Size(108, 25);
			this.butViewSched.TabIndex = 24;
			this.butViewSched.Text = "View Schedule";
			this.butViewSched.Click += new System.EventHandler(this.butViewSched_Click);
			// 
			// butManage
			// 
			this.butManage.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butManage.Autosize = true;
			this.butManage.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butManage.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butManage.CornerRadius = 4F;
			this.butManage.Location = new System.Drawing.Point(366, 13);
			this.butManage.Name = "butManage";
			this.butManage.Size = new System.Drawing.Size(108, 25);
			this.butManage.TabIndex = 23;
			this.butManage.Text = "Manage";
			this.butManage.Click += new System.EventHandler(this.butManage_Click);
			// 
			// butBreaks
			// 
			this.butBreaks.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBreaks.Autosize = true;
			this.butBreaks.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBreaks.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBreaks.CornerRadius = 4F;
			this.butBreaks.Location = new System.Drawing.Point(366, 67);
			this.butBreaks.Name = "butBreaks";
			this.butBreaks.Size = new System.Drawing.Size(108, 25);
			this.butBreaks.TabIndex = 22;
			this.butBreaks.Text = "View Breaks";
			this.butBreaks.Click += new System.EventHandler(this.butBreaks_Click);
			// 
			// gridEmp
			// 
			this.gridEmp.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridEmp.HasAddButton = false;
			this.gridEmp.HasDropDowns = false;
			this.gridEmp.HasMultilineHeaders = false;
			this.gridEmp.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridEmp.HeaderHeight = 15;
			this.gridEmp.HScrollVisible = false;
			this.gridEmp.Location = new System.Drawing.Point(22, 22);
			this.gridEmp.Name = "gridEmp";
			this.gridEmp.ScrollValue = 0;
			this.gridEmp.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridEmp.Size = new System.Drawing.Size(303, 238);
			this.gridEmp.TabIndex = 21;
			this.gridEmp.Title = "Employee";
			this.gridEmp.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridEmp.TitleHeight = 18;
			this.gridEmp.TranslationName = "TableEmpClock";
			this.gridEmp.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridEmp_CellDoubleClick);
			this.gridEmp.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridEmp_CellClick);
			// 
			// labelCurrentTime
			// 
			this.labelCurrentTime.Location = new System.Drawing.Point(376, 121);
			this.labelCurrentTime.Name = "labelCurrentTime";
			this.labelCurrentTime.Size = new System.Drawing.Size(88, 17);
			this.labelCurrentTime.TabIndex = 20;
			this.labelCurrentTime.Text = "Server Time";
			this.labelCurrentTime.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// butClockOut
			// 
			this.butClockOut.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClockOut.Autosize = true;
			this.butClockOut.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClockOut.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClockOut.CornerRadius = 4F;
			this.butClockOut.Location = new System.Drawing.Point(366, 189);
			this.butClockOut.Name = "butClockOut";
			this.butClockOut.Size = new System.Drawing.Size(108, 25);
			this.butClockOut.TabIndex = 14;
			this.butClockOut.Text = "Clock Out For:";
			this.butClockOut.Click += new System.EventHandler(this.butClockOut_Click);
			// 
			// butTimeCard
			// 
			this.butTimeCard.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butTimeCard.Autosize = true;
			this.butTimeCard.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTimeCard.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTimeCard.CornerRadius = 4F;
			this.butTimeCard.Location = new System.Drawing.Point(366, 40);
			this.butTimeCard.Name = "butTimeCard";
			this.butTimeCard.Size = new System.Drawing.Size(108, 25);
			this.butTimeCard.TabIndex = 16;
			this.butTimeCard.Text = "View Time Card";
			this.butTimeCard.Click += new System.EventHandler(this.butTimeCard_Click);
			// 
			// butClockIn
			// 
			this.butClockIn.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClockIn.Autosize = true;
			this.butClockIn.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClockIn.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClockIn.CornerRadius = 4F;
			this.butClockIn.Location = new System.Drawing.Point(366, 162);
			this.butClockIn.Name = "butClockIn";
			this.butClockIn.Size = new System.Drawing.Size(108, 25);
			this.butClockIn.TabIndex = 11;
			this.butClockIn.Text = "Clock In";
			this.butClockIn.Click += new System.EventHandler(this.butClockIn_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox2.Controls.Add(this.listMessages);
			this.groupBox2.Controls.Add(this.butSend);
			this.groupBox2.Controls.Add(this.butAck);
			this.groupBox2.Controls.Add(this.labelSending);
			this.groupBox2.Controls.Add(this.textDays);
			this.groupBox2.Controls.Add(this.labelDays);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.comboViewUser);
			this.groupBox2.Controls.Add(this.gridMessages);
			this.groupBox2.Controls.Add(this.checkIncludeAck);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.listExtras);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.listFrom);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.listTo);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.textMessage);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(3, 277);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(902, 422);
			this.groupBox2.TabIndex = 19;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Messaging";
			// 
			// listMessages
			// 
			this.listMessages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listMessages.FormattingEnabled = true;
			this.listMessages.Location = new System.Drawing.Point(252, 35);
			this.listMessages.Name = "listMessages";
			this.listMessages.Size = new System.Drawing.Size(98, 329);
			this.listMessages.TabIndex = 10;
			this.listMessages.Click += new System.EventHandler(this.listMessages_Click);
			// 
			// butSend
			// 
			this.butSend.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSend.Autosize = true;
			this.butSend.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSend.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSend.CornerRadius = 4F;
			this.butSend.Location = new System.Drawing.Point(252, 392);
			this.butSend.Name = "butSend";
			this.butSend.Size = new System.Drawing.Size(98, 25);
			this.butSend.TabIndex = 15;
			this.butSend.Text = "Send Text";
			this.butSend.Click += new System.EventHandler(this.butSend_Click);
			// 
			// butAck
			// 
			this.butAck.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAck.Autosize = true;
			this.butAck.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAck.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAck.CornerRadius = 4F;
			this.butAck.Location = new System.Drawing.Point(645, 10);
			this.butAck.Name = "butAck";
			this.butAck.Size = new System.Drawing.Size(67, 22);
			this.butAck.TabIndex = 25;
			this.butAck.Text = "Ack";
			this.butAck.Click += new System.EventHandler(this.butAck_Click);
			// 
			// labelSending
			// 
			this.labelSending.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelSending.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSending.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.labelSending.Location = new System.Drawing.Point(251, 368);
			this.labelSending.Name = "labelSending";
			this.labelSending.Size = new System.Drawing.Size(100, 21);
			this.labelSending.TabIndex = 24;
			this.labelSending.Text = "Sending";
			this.labelSending.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.labelSending.Visible = false;
			// 
			// textDays
			// 
			this.textDays.Location = new System.Drawing.Point(594, 12);
			this.textDays.Name = "textDays";
			this.textDays.Size = new System.Drawing.Size(45, 20);
			this.textDays.TabIndex = 19;
			this.textDays.Visible = false;
			this.textDays.TextChanged += new System.EventHandler(this.textDays_TextChanged);
			// 
			// labelDays
			// 
			this.labelDays.Location = new System.Drawing.Point(531, 14);
			this.labelDays.Name = "labelDays";
			this.labelDays.Size = new System.Drawing.Size(61, 16);
			this.labelDays.TabIndex = 18;
			this.labelDays.Text = "Days";
			this.labelDays.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelDays.Visible = false;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(725, 14);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(57, 16);
			this.label6.TabIndex = 17;
			this.label6.Text = "To User";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboViewUser
			// 
			this.comboViewUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboViewUser.FormattingEnabled = true;
			this.comboViewUser.Location = new System.Drawing.Point(783, 11);
			this.comboViewUser.Name = "comboViewUser";
			this.comboViewUser.Size = new System.Drawing.Size(114, 21);
			this.comboViewUser.TabIndex = 16;
			this.comboViewUser.SelectionChangeCommitted += new System.EventHandler(this.comboViewUser_SelectionChangeCommitted);
			// 
			// gridMessages
			// 
			this.gridMessages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridMessages.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMessages.HasAddButton = false;
			this.gridMessages.HasDropDowns = false;
			this.gridMessages.HasMultilineHeaders = false;
			this.gridMessages.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMessages.HeaderHeight = 15;
			this.gridMessages.HScrollVisible = false;
			this.gridMessages.Location = new System.Drawing.Point(356, 35);
			this.gridMessages.Name = "gridMessages";
			this.gridMessages.ScrollValue = 0;
			this.gridMessages.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMessages.Size = new System.Drawing.Size(540, 381);
			this.gridMessages.TabIndex = 13;
			this.gridMessages.Title = "Message History";
			this.gridMessages.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMessages.TitleHeight = 18;
			this.gridMessages.TranslationName = "TableTextMessages";
			// 
			// checkIncludeAck
			// 
			this.checkIncludeAck.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeAck.Location = new System.Drawing.Point(356, 16);
			this.checkIncludeAck.Name = "checkIncludeAck";
			this.checkIncludeAck.Size = new System.Drawing.Size(173, 18);
			this.checkIncludeAck.TabIndex = 14;
			this.checkIncludeAck.Text = "Include Acknowledged";
			this.checkIncludeAck.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeAck.UseVisualStyleBackColor = true;
			this.checkIncludeAck.Click += new System.EventHandler(this.checkIncludeAck_Click);
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label7.Location = new System.Drawing.Point(6, 377);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(100, 16);
			this.label7.TabIndex = 12;
			this.label7.Text = "Text Message";
			this.label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(250, 16);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 16);
			this.label5.TabIndex = 9;
			this.label5.Text = "Message (&& Send)";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listExtras
			// 
			this.listExtras.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listExtras.FormattingEnabled = true;
			this.listExtras.Location = new System.Drawing.Point(171, 35);
			this.listExtras.Name = "listExtras";
			this.listExtras.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listExtras.Size = new System.Drawing.Size(75, 329);
			this.listExtras.TabIndex = 8;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(169, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(78, 16);
			this.label4.TabIndex = 7;
			this.label4.Text = "Extras";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listFrom
			// 
			this.listFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listFrom.FormattingEnabled = true;
			this.listFrom.Location = new System.Drawing.Point(90, 35);
			this.listFrom.Name = "listFrom";
			this.listFrom.Size = new System.Drawing.Size(75, 329);
			this.listFrom.TabIndex = 6;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(88, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(78, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "From";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listTo
			// 
			this.listTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listTo.FormattingEnabled = true;
			this.listTo.Location = new System.Drawing.Point(9, 35);
			this.listTo.Name = "listTo";
			this.listTo.Size = new System.Drawing.Size(75, 329);
			this.listTo.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(7, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(78, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "To";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textMessage
			// 
			this.textMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textMessage.Location = new System.Drawing.Point(9, 396);
			this.textMessage.Name = "textMessage";
			this.textMessage.Size = new System.Drawing.Size(237, 20);
			this.textMessage.TabIndex = 1;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.butEras);
			this.groupBox3.Controls.Add(this.butImportInsPlans);
			this.groupBox3.Controls.Add(this.butEmailInbox);
			this.groupBox3.Controls.Add(this.butSupply);
			this.groupBox3.Controls.Add(this.butClaimPay);
			this.groupBox3.Controls.Add(this.butBilling);
			this.groupBox3.Controls.Add(this.butAccounting);
			this.groupBox3.Controls.Add(this.butBackup);
			this.groupBox3.Controls.Add(this.butDeposit);
			this.groupBox3.Controls.Add(this.butSendClaims);
			this.groupBox3.Controls.Add(this.butTasks);
			this.groupBox3.Controls.Add(this.butManageAR);
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(34, 5);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(286, 182);
			this.groupBox3.TabIndex = 23;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Daily";
			// 
			// butEras
			// 
			this.butEras.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEras.Autosize = true;
			this.butEras.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEras.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEras.CornerRadius = 4F;
			this.butEras.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEras.Location = new System.Drawing.Point(148, 123);
			this.butEras.Name = "butEras";
			this.butEras.Size = new System.Drawing.Size(104, 26);
			this.butEras.TabIndex = 30;
			this.butEras.Text = "ERAs";
			this.butEras.Click += new System.EventHandler(this.butEras_Click);
			// 
			// butImportInsPlans
			// 
			this.butImportInsPlans.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butImportInsPlans.Autosize = true;
			this.butImportInsPlans.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butImportInsPlans.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butImportInsPlans.CornerRadius = 4F;
			this.butImportInsPlans.Location = new System.Drawing.Point(148, 149);
			this.butImportInsPlans.Name = "butImportInsPlans";
			this.butImportInsPlans.Size = new System.Drawing.Size(104, 26);
			this.butImportInsPlans.TabIndex = 29;
			this.butImportInsPlans.Text = "Import Ins Plans";
			this.butImportInsPlans.Click += new System.EventHandler(this.butImportInsPlans_Click);
			// 
			// butEmailInbox
			// 
			this.butEmailInbox.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEmailInbox.Autosize = true;
			this.butEmailInbox.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEmailInbox.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEmailInbox.CornerRadius = 4F;
			this.butEmailInbox.Image = global::OpenDental.Properties.Resources.email1;
			this.butEmailInbox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEmailInbox.Location = new System.Drawing.Point(148, 97);
			this.butEmailInbox.Name = "butEmailInbox";
			this.butEmailInbox.Size = new System.Drawing.Size(104, 26);
			this.butEmailInbox.TabIndex = 28;
			this.butEmailInbox.Text = "Emails";
			this.butEmailInbox.Click += new System.EventHandler(this.butEmailInbox_Click);
			// 
			// butSupply
			// 
			this.butSupply.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSupply.Autosize = true;
			this.butSupply.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSupply.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSupply.CornerRadius = 4F;
			this.butSupply.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSupply.Location = new System.Drawing.Point(16, 123);
			this.butSupply.Name = "butSupply";
			this.butSupply.Size = new System.Drawing.Size(104, 26);
			this.butSupply.TabIndex = 26;
			this.butSupply.Text = "Supply Inventory";
			this.butSupply.Click += new System.EventHandler(this.butSupply_Click);
			// 
			// butClaimPay
			// 
			this.butClaimPay.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClaimPay.Autosize = true;
			this.butClaimPay.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClaimPay.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClaimPay.CornerRadius = 4F;
			this.butClaimPay.Location = new System.Drawing.Point(16, 45);
			this.butClaimPay.Name = "butClaimPay";
			this.butClaimPay.Size = new System.Drawing.Size(104, 26);
			this.butClaimPay.TabIndex = 25;
			this.butClaimPay.Text = "Batch Ins";
			this.butClaimPay.Click += new System.EventHandler(this.butClaimPay_Click);
			// 
			// butBilling
			// 
			this.butBilling.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBilling.Autosize = true;
			this.butBilling.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBilling.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBilling.CornerRadius = 4F;
			this.butBilling.Location = new System.Drawing.Point(16, 71);
			this.butBilling.Name = "butBilling";
			this.butBilling.Size = new System.Drawing.Size(104, 26);
			this.butBilling.TabIndex = 25;
			this.butBilling.Text = "Billing";
			this.butBilling.Click += new System.EventHandler(this.butBilling_Click);
			// 
			// butAccounting
			// 
			this.butAccounting.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAccounting.Autosize = true;
			this.butAccounting.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAccounting.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAccounting.CornerRadius = 4F;
			this.butAccounting.Image = ((System.Drawing.Image)(resources.GetObject("butAccounting.Image")));
			this.butAccounting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAccounting.Location = new System.Drawing.Point(148, 71);
			this.butAccounting.Name = "butAccounting";
			this.butAccounting.Size = new System.Drawing.Size(104, 26);
			this.butAccounting.TabIndex = 24;
			this.butAccounting.Text = "Accounting";
			this.butAccounting.Click += new System.EventHandler(this.butAccounting_Click);
			// 
			// butBackup
			// 
			this.butBackup.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBackup.Autosize = true;
			this.butBackup.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBackup.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBackup.CornerRadius = 4F;
			this.butBackup.Image = ((System.Drawing.Image)(resources.GetObject("butBackup.Image")));
			this.butBackup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butBackup.Location = new System.Drawing.Point(148, 45);
			this.butBackup.Name = "butBackup";
			this.butBackup.Size = new System.Drawing.Size(104, 26);
			this.butBackup.TabIndex = 22;
			this.butBackup.Text = "Backup";
			this.butBackup.Click += new System.EventHandler(this.butBackup_Click);
			// 
			// butDeposit
			// 
			this.butDeposit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDeposit.Autosize = true;
			this.butDeposit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDeposit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDeposit.CornerRadius = 4F;
			this.butDeposit.Image = ((System.Drawing.Image)(resources.GetObject("butDeposit.Image")));
			this.butDeposit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDeposit.Location = new System.Drawing.Point(16, 97);
			this.butDeposit.Name = "butDeposit";
			this.butDeposit.Size = new System.Drawing.Size(104, 26);
			this.butDeposit.TabIndex = 23;
			this.butDeposit.Text = "Deposits";
			this.butDeposit.Click += new System.EventHandler(this.butDeposit_Click);
			// 
			// butSendClaims
			// 
			this.butSendClaims.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSendClaims.Autosize = true;
			this.butSendClaims.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSendClaims.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSendClaims.CornerRadius = 4F;
			this.butSendClaims.Image = ((System.Drawing.Image)(resources.GetObject("butSendClaims.Image")));
			this.butSendClaims.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSendClaims.Location = new System.Drawing.Point(16, 19);
			this.butSendClaims.Name = "butSendClaims";
			this.butSendClaims.Size = new System.Drawing.Size(104, 26);
			this.butSendClaims.TabIndex = 20;
			this.butSendClaims.Text = "Send Claims";
			this.butSendClaims.Click += new System.EventHandler(this.butSendClaims_Click);
			// 
			// butTasks
			// 
			this.butTasks.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butTasks.Autosize = true;
			this.butTasks.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTasks.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTasks.CornerRadius = 4F;
			this.butTasks.Image = ((System.Drawing.Image)(resources.GetObject("butTasks.Image")));
			this.butTasks.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butTasks.Location = new System.Drawing.Point(148, 19);
			this.butTasks.Name = "butTasks";
			this.butTasks.Size = new System.Drawing.Size(104, 26);
			this.butTasks.TabIndex = 21;
			this.butTasks.Text = "Tasks";
			this.butTasks.Click += new System.EventHandler(this.butTasks_Click);
			// 
			// butManageAR
			// 
			this.butManageAR.AdjustImageLocation = new System.Drawing.Point(-2, 0);
			this.butManageAR.Autosize = true;
			this.butManageAR.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butManageAR.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butManageAR.CornerRadius = 4F;
			this.butManageAR.Image = global::OpenDental.Properties.Resources.TSI_Icon;
			this.butManageAR.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butManageAR.Location = new System.Drawing.Point(16, 149);
			this.butManageAR.Name = "butManageAR";
			this.butManageAR.Size = new System.Drawing.Size(104, 26);
			this.butManageAR.TabIndex = 31;
			this.butManageAR.Text = "Collections";
			this.butManageAR.Click += new System.EventHandler(this.butManageAR_Click);
			// 
			// timerSending
			// 
			this.timerSending.Interval = 1000;
			this.timerSending.Tick += new System.EventHandler(this.timerSending_Tick);
			// 
			// ContrStaff
			// 
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "ContrStaff";
			this.Size = new System.Drawing.Size(908, 702);
			this.Load += new System.EventHandler(this.ContrStaff_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void ContrStaff_Load(object sender, System.EventArgs e) {
		
		}

		///<summary>Only gets run on startup.</summary>
		public void InitializeOnStartup() {
			//if(InitializedOnStartup) {
			//	return;
			//}
			//InitializedOnStartup=true;
			//can't use Lan.F
			Lan.C(this,new Control[]
				{
				groupBox2,
				label1,
				butSend,
				groupBox1,
				butTimeCard,
				labelCurrentTime,
				butClaimPay,
				butClockIn,
				butClockOut,
				butEmailInbox,
				butSendClaims,
				butBilling,
				butDeposit,
				butSupply,
				butTasks,
				butBackup,
				butAccounting,
				butBreaks,
				label3,
				label4,
				label5,
				label7,
				labelSending,
				checkIncludeAck,
				labelDays,
				butAck,
				label6,
				gridEmp,
				gridMessages,
				});
			RefreshFullMessages();//after this, messages just get added to the list.
			//But if checkIncludeAck is clicked,then it does RefreshMessages again.
		}

		///<summary></summary>
		public void ModuleSelected(long patNum) {
			PatCurNum=patNum;
			RefreshModuleData(patNum);
			RefreshModuleScreen();
			Plugins.HookAddCode(this,"ContrStaff.ModuleSelected_end",patNum);
		}

		///<summary></summary>
		public void ModuleUnselected(){
			//this is not getting triggered yet.
			Plugins.HookAddCode(this,"ContrStaff.ModuleUnselected_end");
		}

		private void RefreshModuleData(long patNum) {
      if(PrefC.GetBool(PrefName.LocalTimeOverridesServerTime)) {
        TimeDelta=new TimeSpan(0);
      }
      else {
        TimeDelta=MiscData.GetNowDateTime()-DateTime.Now;
      }
			Employees.RefreshCache();
			//RefreshModulePatient(patNum);
		}

		private void RefreshModuleScreen(){
      if(PrefC.GetBool(PrefName.LocalTimeOverridesServerTime)) {
        labelCurrentTime.Text="Local Time";
      }
      else {
        labelCurrentTime.Text="Server Time";
      }
			textTime.Text=(DateTime.Now+TimeDelta).ToLongTimeString();
			FillEmps();
			FillMessageDefs();
			if(Security.IsAuthorized(Permissions.TimecardsEditAll,true)) {
				butManage.Enabled=true;
			}
			else {
				butManage.Enabled=false;
			}
			butImportInsPlans.Visible=true;
			if(PrefC.GetBool(PrefName.EasyHidePublicHealth)) {
				butImportInsPlans.Visible=false;//Import Ins Plans button is only visible when Public Health feature is enabled.
			}
			butManageAR.Visible=!ProgramProperties.IsAdvertisingDisabled(ProgramName.Transworld);
		}
		/*
		///<summary>Here so it's parallel with other modules.</summary>
		private void RefreshModulePatient(int patNum){
			PatCurNum=patNum;
			if(patNum==0){
				OnPatientSelected(patNum,"",false,"");
			}
			else{
				Patient pat=Patients.GetPat(patNum);
				OnPatientSelected(patNum,pat.GetNameLF(),pat.Email!="",pat.ChartNumber);
			}
		}*/

		private void butSendClaims_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.ClaimSend)) {
				return;
			}
			if(FormCS!=null && !FormCS.IsDisposed) {//Form is open
				FormCS.Focus();//Don't open a new form.
				//We may need to close and reopen the form in the future if the window is not being brought to the front.
				//It is complicated to Close() and reopen the form, because the user might be in the middle of a task.
				return;
			}
			Cursor=Cursors.WaitCursor;
			FormCS=new FormClaimsSend();
			FormCS.FormClosed+=(s,ea) => { ODEvent.Fired-=formClaimsSend_GoToChanged; };
			ODEvent.Fired+=formClaimsSend_GoToChanged;
			FormCS.Show();//FormClaimsSend has a GoTo option and is shown as a non-modal window.
			FormCS.BringToFront();
			Cursor=Cursors.Default;
		}
		
		private void butClaimPay_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsPayCreate,true) && !Security.IsAuthorized(Permissions.InsPayEdit,true)) {
				//Custom message for multiple permissions.
				MessageBox.Show(Lan.g(this,"Not authorized")+".\r\n"
					+Lan.g(this,"A user with the SecurityAdmin permission must grant you access for")+":\r\n"
					+Lan.g(this,"Insurance Payment Create or Insurance Payment Edit"));
				return;
			}
			FormClaimPayList FormCPL=new FormClaimPayList();
			FormCPL.Show();
		}

		private void butBilling_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Billing)) {
				return;
			}
			bool unsentStatementsExist=Statements.UnsentStatementsExist();
			if(unsentStatementsExist) {
				if(PrefC.HasClinicsEnabled) {//Using clinics.
					if(Statements.UnsentClinicStatementsExist(Clinics.ClinicNum)) {//Check if clinic has unsent bills.
						ShowBilling(Clinics.ClinicNum);//Clinic has unsent bills.  Simply show billing window.
					}
					else {//No unsent bills for clinic.  Show billing options to generate a billing list.
						ShowBillingOptions(Clinics.ClinicNum);
					}
				}
				else {//Not using clinics and has unsent bills.  Simply show billing window.
					ShowBilling(0);
				}
			}
			else {//No unsent statements exist.  Have user create a billing list.
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
					ShowBillingOptions(Clinics.ClinicNum);
				}
				else {
					ShowBillingOptions(0);
				}
			}
			SecurityLogs.MakeLogEntry(Permissions.Billing,0,"");
		}

		///<summary>Shows FormBilling and displays warning message if needed.  Pass 0 to show all clinics.  Make sure to check for unsent bills before calling this method.</summary>
		private void ShowBilling(long clinicNum) {
			bool hadListShowing=false;
			//Check to see if there is an instance of the billing list window already open that needs to be closed.
			//This can happen if multiple people are trying to send bills at the same time.
			if(FormB!=null && !FormB.IsDisposed) {
				hadListShowing=true;
				//It does not hurt to always close this window before loading a new instance, because the unsent bills are saved in the database and the entire purpose of FormBilling is the Go To feature.
				//Any statements that were showing in the old billing list window that we are about to close could potentially be stale and are now invalid and should not be sent.
				//Another good reason to close the window is when using clinics.  It was possible to show a different clinic billing list than the one chosen.
				for(int i=0;i<FormB.ListClinics.Count;i++) {
					if(FormB.ListClinics[i].ClinicNum!=clinicNum) {//For most users clinic nums will always be 0.
						//The old billing list was showing a different clinic.  No need to show the warning message in this scenario.
						hadListShowing=false;
					}
				}
				FormB.Close();
			}
			FormB=new FormBilling();
			FormB.ClinicNum=clinicNum;
			FormB.Show();//FormBilling has a Go To option and is shown as a non-modal window so the user can view the patient account and the billing list at the same time.
			FormB.BringToFront();
			if(hadListShowing) {
				MsgBox.Show(this,"These unsent bills must either be sent or deleted before a new list can be created.");
			}
		}

		///<summary>Shows FormBillingOptions and FormBilling if needed.  Pass 0 to show all clinics.  Make sure to check for unsent bills before calling this method.</summary>
		private void ShowBillingOptions(long clinicNum) {
			FormBillingOptions FormBO=new FormBillingOptions();
			FormBO.ClinicNum=clinicNum;
			FormBO.ShowDialog();
			if(FormBO.DialogResult==DialogResult.OK) {
				ShowBilling(clinicNum);
			}
		}

		private void butManageAR_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Billing)) {
				return;
			}
			if(!Programs.IsEnabled(ProgramName.Transworld)) {
				try {
					Process.Start("http://www.opendental.com/manual/transworldsystems.html");
				}
				catch(Exception ex) {
					ex.DoNothing();
					MsgBox.Show(this,"Failed to open web browser.  Please make sure you have a default browser set and are connected to the internet and then try again.");
				}
				return;
			}
			if(_formAR==null || _formAR.IsDisposed) {
				while(!ValidateConnectionDetails()) {//only validate connection details if the ArManager form does not exist yet
					string msgText="An SFTP connection could not be made using the connection details "+(PrefC.HasClinicsEnabled ? "for any clinic " : "")
						+"in the enabled Transworld (TSI) program link.  Would you like to edit the Transworld program link now?";
					if(!MsgBox.Show(this,MsgBoxButtons.YesNo,msgText)) {//if user does not want to edit program link, return
						return;
					}
					FormTransworldSetup FormTS=new FormTransworldSetup();
					if(FormTS.ShowDialog()!=DialogResult.OK) {//if user cancels edits in the setup window, return
						return;
					}
				}
				_formAR=new FormArManager();//connections settings have been validated, create a new ArManager form
			}
			_formAR.Restore();
			_formAR.Show();//form has a Go To option and is shown as a non-modal window so the user can view the pat account and the collection list at the same time.
			_formAR.BringToFront();
		}

		private bool ValidateConnectionDetails() {
			Program progCur=Programs.GetCur(ProgramName.Transworld);
			List<long> listClinicNums=new List<long>();
			if(PrefC.HasClinicsEnabled) {
				listClinicNums=Clinics.GetAllForUserod(Security.CurUser).Select(x => x.ClinicNum).ToList();
				if(!Security.CurUser.ClinicIsRestricted) {
					listClinicNums.Add(0);
				}
			}
			else {
				listClinicNums.Add(0);
			}
			List<ProgramProperty> listProperties=ProgramProperties.GetForProgram(progCur.ProgramNum);
			foreach(long clinicNum in listClinicNums) {
				List<ProgramProperty> listPropsForClinic=new List<ProgramProperty>();
				if(listProperties.All(x => x.ClinicNum!=clinicNum)) {//if no prog props exist for the clinic, continue, clinicNum 0 will be tested once as well
					continue;
				}
				listPropsForClinic=listProperties.FindAll(x => x.ClinicNum==clinicNum);
				string sftpAddress=listPropsForClinic.Find(x => x.PropertyDesc=="SftpServerAddress")?.PropertyValue??"";
				int sftpPort;
				if(!int.TryParse(listPropsForClinic.Find(x => x.PropertyDesc=="SftpServerPort")?.PropertyValue??"",out sftpPort)) {
					sftpPort=22;//default to port 22
				}
				string userName=listPropsForClinic.Find(x => x.PropertyDesc=="SftpUsername")?.PropertyValue??"";
				string userPassword=listPropsForClinic.Find(x => x.PropertyDesc=="SftpPassword")?.PropertyValue??"";
				if(Sftp.IsConnectionValid(sftpAddress,userName,userPassword,sftpPort)) {
					return true;
				}
			}
			return false;
		}

		private void formClaimsSend_GoToChanged(ODEventArgs e) {
			if(e.Name!="FormClaimSend_GoTo") {
				return;
			}
			ClaimSendQueueItem claimSendQueueItem=(ClaimSendQueueItem)e.Tag;
			Patient pat=Patients.GetPat(claimSendQueueItem.PatNum);
			FormOpenDental.S_Contr_PatientSelected(pat,false);
			GotoModule.GotoClaim(claimSendQueueItem.ClaimNum);
		}

		private void butDeposit_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.DepositSlips,DateTime.Today)){
				return;
			}
			FormDeposits FormD=new FormDeposits();
			FormD.ShowDialog();
		}

		private void butAccounting_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Accounting)) {
				return;
			}
			if(FormA==null || FormA.IsDisposed) {
				FormA=new FormAccounting();
			}
			FormA.Show();
			if(FormA.WindowState==FormWindowState.Minimized) {
				FormA.WindowState=FormWindowState.Normal;
			}
			FormA.BringToFront();
		}

		private void butBackup_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Backup)){
				return;
			}
			SecurityLogs.MakeLogEntry(Permissions.Backup,0,"FormBackup was accessed");
			FormBackup FormB=new FormBackup();
			FormB.ShowDialog();
			if(FormB.DialogResult==DialogResult.Cancel){
				return;
			}
			//ok signifies that a database was restored
			FormOpenDental.S_Contr_PatientSelected(new Patient(),false);//unload patient after restore.
			//ParentForm.Text=PrefC.GetString(PrefName.MainWindowTitle");
			DataValid.SetInvalid(true);
			ModuleSelected(PatCurNum);
		}

		private void butTasks_Click(object sender, System.EventArgs e) {
			LaunchTaskWindow(false);
			/*  //This is the old code exactly how it was before making the task window non-modal in case issues arise.
			FormTasks FormT=new FormTasks();
			FormT.ShowDialog();
			if(FormT.GotoType==TaskObjectType.Patient){
				if(FormT.GotoKeyNum!=0){
					Patient pat=Patients.GetPat(FormT.GotoKeyNum);
					OnPatientSelected(pat);
					GotoModule.GotoAccount(0);
				}
			}
			if(FormT.GotoType==TaskObjectType.Appointment){
				if(FormT.GotoKeyNum!=0){
					Appointment apt=Appointments.GetOneApt(FormT.GotoKeyNum);
					if(apt==null){
						MsgBox.Show(this,"Appointment has been deleted, so it's not available.");
						return;
						//this could be a little better, because window has closed, but they will learn not to push that button.
					}
					DateTime dateSelected=DateTime.MinValue;
					if(apt.AptStatus==ApptStatus.Planned || apt.AptStatus==ApptStatus.UnschedList){
						//I did not add feature to put planned or unsched apt on pinboard.
						MsgBox.Show(this,"Cannot navigate to appointment.  Use the Other Appointments button.");
						//return;
					}
					else{
						dateSelected=apt.AptDateTime;
					}
					Patient pat=Patients.GetPat(apt.PatNum);
					OnPatientSelected(pat);
					GotoModule.GotoAppointment(dateSelected,apt.AptNum);
				}
			}
			*/
		}

		///<summary>Only used internally to launch the task window with the Triage task list.</summary>
		public void JumpToTriageTaskWindow() {
			LaunchTaskWindow(true);
		}

		///<summary>Used to launch the task window preloaded with a certain task list open.  isTriage is only used at OD HQ.</summary>
		public void LaunchTaskWindow(bool isTriage,UserControlTasksTab tab=UserControlTasksTab.Invalid) {
			if(FormT==null || FormT.IsDisposed) {
				FormT=new FormTasks();
			}
			FormT.Show();
			if(isTriage) {
				FormT.ShowTriage();
			}
			else if(tab!=UserControlTasksTab.Invalid) {
				FormT.TaskTab=tab;
			}
			if(FormT.WindowState==FormWindowState.Minimized) {
				FormT.WindowState=FormWindowState.Normal;
			}
			FormT.BringToFront();
		}

		private void butSupply_Click(object sender,EventArgs e) {
			FormSupplyInventory FormS=new FormSupplyInventory();
			FormS.ShowDialog();
		}

		private void butEras_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsPayCreate)) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			FormEtrans835s FormE=new FormEtrans835s();
			FormE.Show();//non-modal
			Cursor=Cursors.Default;
		}

		private void butEmailInbox_Click(object sender,EventArgs e) {
      if(FormEmailInbox==null||FormEmailInbox.IsDisposed) {
        FormEmailInbox=null;
        FormEmailInbox=new FormEmailInbox();
        FormEmailInbox.Show();
      }
      else {
        if(FormEmailInbox.WindowState==FormWindowState.Minimized) {
          FormEmailInbox.WindowState=FormWindowState.Maximized;
        }
        FormEmailInbox.BringToFront();
      }
    }

		private void butImportInsPlans_Click(object sender,EventArgs e) {
			if(FormE834I!=null && FormE834I.FormE834P!=null && !FormE834I.FormE834P.IsDisposed) {
				FormE834I.FormE834P.Show();
				FormE834I.FormE834P.BringToFront();
				return;
			}
			if(FormE834I==null || FormE834I.IsDisposed) {
				FormE834I=new FormEtrans834Import();
			}
			FormE834I.Show();
			FormE834I.BringToFront();
		}

		//private void butClear_Click(object sender, System.EventArgs e) {
			//textMessage.Clear();
			//textMessage.Select();
		//}

		private void FillEmps(){
			gridEmp.BeginUpdate();
			gridEmp.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableEmpClock","Employee"),180);
			gridEmp.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEmpClock","Status"),104);
			gridEmp.Columns.Add(col);
			gridEmp.Rows.Clear();
			UI.ODGridRow row;
			if(PrefC.HasClinicsEnabled) {
				_listEmployees=Employees.GetEmpsForClinic(Clinics.ClinicNum,false,true);
			}
			else {
				_listEmployees=Employees.GetDeepCopy(true);
			}
			for(int i=0;i<_listEmployees.Count;i++) {
				row=new OpenDental.UI.ODGridRow();
				row.Cells.Add(Employees.GetNameFL(_listEmployees[i]));
				row.Cells.Add(_listEmployees[i].ClockStatus);
				row.Tag=_listEmployees[i];
				gridEmp.Rows.Add(row);
			}
			gridEmp.EndUpdate();
			listStatus.Items.Clear();
			for(int i=0;i<Enum.GetNames(typeof(TimeClockStatus)).Length;i++){
				listStatus.Items.Add(Lan.g("enumTimeClockStatus",Enum.GetNames(typeof(TimeClockStatus))[i]));
			}
			for(int i=0;i<_listEmployees.Count;i++) {
				if(_listEmployees[i].EmployeeNum==Security.CurUser.EmployeeNum) {
					SelectEmpI(i);
					return;
				}
			}
			SelectEmpI(-1);
		}

		///<summary>-1 is also valid.</summary>
		private void SelectEmpI(int index,bool doClearGridSelection=true){
			if(doClearGridSelection) {
				gridEmp.SetSelected(false);
			}
			if(index==-1){
				butClockIn.Enabled=false;
				butClockOut.Enabled=false;
				butTimeCard.Enabled=false;
				butBreaks.Enabled=false;
				listStatus.Enabled=false;
				return;
			}
			gridEmp.SetSelected(index,true);
			EmployeeCur=_listEmployees[index];
			ClockEvent clockEvent=ClockEvents.GetLastEvent(EmployeeCur.EmployeeNum);
			if(clockEvent==null) {//new employee.  They need to clock in.
				butClockIn.Enabled=true;
				butClockOut.Enabled=false;
				butTimeCard.Enabled=true;
				butBreaks.Enabled=true;
				listStatus.SelectedIndex=(int)TimeClockStatus.Home;
				listStatus.Enabled=false;
			}
			else if(clockEvent.ClockStatus==TimeClockStatus.Break) {//only incomplete breaks will have been returned.
				//clocked out for break, but not clocked back in
				butClockIn.Enabled=true;
				butClockOut.Enabled=false;
				butTimeCard.Enabled=true;
				butBreaks.Enabled=true;
				listStatus.SelectedIndex=(int)TimeClockStatus.Break;
				listStatus.Enabled=false;
			}
			else {//normal clock in/out
				if(clockEvent.TimeDisplayed2.Year<1880) {//clocked in to work, but not clocked back out.
					butClockIn.Enabled=false;
					butClockOut.Enabled=true;
					butTimeCard.Enabled=true;
					butBreaks.Enabled=true;
					listStatus.Enabled=true;
				}
				else {//clocked out for home or lunch.  Need to clock back in.
					butClockIn.Enabled=true;
					butClockOut.Enabled=false;
					butTimeCard.Enabled=true;
					butBreaks.Enabled=true;
					listStatus.SelectedIndex=(int)clockEvent.ClockStatus;
					listStatus.Enabled=false;
				}
			}
		}

		private void gridEmp_CellClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			if(gridEmp.SelectedIndices.Length>=2) {
				SelectEmpI(-1,false);//Disable various UI elements.
				return;
			}
			if(PrefC.GetBool(PrefName.TimecardSecurityEnabled)){
				if(Security.CurUser.EmployeeNum!=_listEmployees[e.Row].EmployeeNum) {
					if(!Security.IsAuthorized(Permissions.TimecardsEditAll,true)){
						SelectEmpI(-1,false);
						return;
					}
				}
			}
			SelectEmpI(e.Row);
		}

		private void listStatus_Click(object sender, System.EventArgs e) {
			//
		}

		private void butClockIn_Click(object sender,System.EventArgs e) {
			if(gridEmp.SelectedGridRows.Count>1) {
				SelectEmpI(-1);
				return;
			}
			if(PrefC.GetBool(PrefName.DockPhonePanelShow) && !Security.IsAuthorized(Permissions.TimecardsEditAll,true)) {
				//Check if the employee set their ext to 0 in the phoneempdefault table.
				if(PhoneEmpDefaults.GetByExtAndEmp(0,EmployeeCur.EmployeeNum)==null) {
					MessageBox.Show("Not allowed.  Use the small phone panel or the \"Big\" phone window to clock in.\r\nIf you are trying to clock in as a \"floater\", you need to set your extension to 0 first before using this Clock In button.");
					return;
				}
			}
			try{
				ClockEvents.ClockIn(EmployeeCur.EmployeeNum);
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
				return;
			}
			EmployeeCur.ClockStatus=Lan.g(this,"Working");
			Employees.Update(EmployeeCur);
			if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {
				Phones.SetPhoneStatus(ClockStatusEnum.Available,Phones.GetExtensionForEmp(EmployeeCur.EmployeeNum),EmployeeCur.EmployeeNum);
			}
			ModuleSelected(PatCurNum);
			if(!PayPeriods.HasPayPeriodForDate(DateTime.Today)) {
				MsgBox.Show(this,"No dates exist for this pay period.  Time clock events will not display until pay periods have been created for this date range");
			}
		}

		private void butClockOut_Click(object sender,System.EventArgs e) {
			if(gridEmp.SelectedGridRows.Count>1) {
				SelectEmpI(-1);
				return;
			}
			if(PrefC.GetBool(PrefName.DockPhonePanelShow) && !Security.IsAuthorized(Permissions.TimecardsEditAll,true)) {
				//Check if the employee set their ext to 0 in the phoneempdefault table.
				if(PhoneEmpDefaults.GetByExtAndEmp(0,EmployeeCur.EmployeeNum)==null) {
					MessageBox.Show("Not allowed.  Use the small phone panel or the \"Big\" phone window to clock out.\r\nIf you are trying to clock out as a \"floater\", you need to set your extension to 0 first before using this Clock Out For: button.");
					return;
				}
			}
			if(listStatus.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a status first.");
				return;
			}
			try{
				ClockEvents.ClockOut(EmployeeCur.EmployeeNum,(TimeClockStatus)listStatus.SelectedIndex);
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
				return;
			}
			EmployeeCur.ClockStatus=Lan.g("enumTimeClockStatus",((TimeClockStatus)listStatus.SelectedIndex).ToString());
			Employees.Update(EmployeeCur);
			ModuleSelected(PatCurNum);
			if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {
				Phones.SetPhoneStatus(Phones.GetClockStatusFromEmp(EmployeeCur.ClockStatus),Phones.GetExtensionForEmp(EmployeeCur.EmployeeNum),EmployeeCur.EmployeeNum);
			}
		}

		private void timer1_Tick(object sender, System.EventArgs e) {
			//this will happen once a second
			if(this.Visible){
				textTime.Text=(DateTime.Now+TimeDelta).ToLongTimeString();
			}
		}

		private void butManage_Click(object sender,EventArgs e) {
			FormTimeCardManage FormTCM=new FormTimeCardManage(_listEmployees);
			FormTCM.ShowDialog();
		}

		private void gridEmp_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(gridEmp.SelectedGridRows.Count>1) {//Just in case
				return;
			}
			if(PayPeriods.GetCount()==0) {
				MsgBox.Show(this,"The adminstrator needs to setup pay periods first.");
				return;
			}
			if(!butTimeCard.Enabled) {
				return;
			}
			FormTimeCard FormTC=new FormTimeCard(_listEmployees);
			FormTC.EmployeeCur=_listEmployees[e.Row];
			FormTC.ShowDialog();
			ModuleSelected(PatCurNum);
		}

		private void butTimeCard_Click(object sender, System.EventArgs e) {
			if(gridEmp.SelectedGridRows.Count>1) {
				SelectEmpI(-1);
				return;
			}
			if(PayPeriods.GetCount()==0){
				MsgBox.Show(this,"The adminstrator needs to setup pay periods first.");
				return;
			}
			FormTimeCard FormTC=new FormTimeCard(_listEmployees);
			FormTC.EmployeeCur=EmployeeCur;
			FormTC.ShowDialog();
			ModuleSelected(PatCurNum);
		}

		private void butBreaks_Click(object sender,EventArgs e) {
			if(gridEmp.SelectedGridRows.Count>1) {
				SelectEmpI(-1);
				return;
			}
			if(PayPeriods.GetCount()==0) {
				MsgBox.Show(this,"The adminstrator needs to setup pay periods first.");
				return;
			}
			FormTimeCard FormTC=new FormTimeCard(_listEmployees);
			FormTC.EmployeeCur=EmployeeCur;
			FormTC.IsBreaks=true;
			FormTC.ShowDialog();
			ModuleSelected(PatCurNum);
		}

		private void butViewSched_Click(object sender,EventArgs e) {
			List<long> listPreSelectedEmpNums=gridEmp.SelectedGridRows.Select(x => ((Employee)x.Tag).EmployeeNum).ToList();
			List<long> listPreSelectedProvNums=Userods.GetWhere(x => listPreSelectedEmpNums.Contains(x.EmployeeNum) && x.ProvNum!=0)
				.Select(x => x.ProvNum)
				.ToList();
			FormSchedule formSched=new FormSchedule(listPreSelectedEmpNums,listPreSelectedProvNums);
			formSched.ShowDialog();
		}

		#region Messaging
		///<summary>Gets run with each module selected.  Should be very fast.</summary>
		private void FillMessageDefs(){
			sigElementDefUser=SigElementDefs.GetSubList(SignalElementType.User);
			sigElementDefExtras=SigElementDefs.GetSubList(SignalElementType.Extra);
			sigElementDefMessages=SigElementDefs.GetSubList(SignalElementType.Message);
			listTo.Items.Clear();
			for(int i=0;i<sigElementDefUser.Length;i++){
				listTo.Items.Add(sigElementDefUser[i].SigText);
			}
			listFrom.Items.Clear();
			for(int i=0;i<sigElementDefUser.Length;i++) {
				listFrom.Items.Add(sigElementDefUser[i].SigText);
			}
			listExtras.Items.Clear();
			for(int i=0;i<sigElementDefExtras.Length;i++) {
				listExtras.Items.Add(sigElementDefExtras[i].SigText);
			}
			listMessages.Items.Clear();
			for(int i=0;i<sigElementDefMessages.Length;i++) {
				listMessages.Items.Add(sigElementDefMessages[i].SigText);
			}
			comboViewUser.Items.Clear();
			comboViewUser.Items.Add(Lan.g(this,"all"));
			for(int i=0;i<sigElementDefUser.Length;i++) {
				comboViewUser.Items.Add(sigElementDefUser[i].SigText);
			}
			comboViewUser.SelectedIndex=0;
		}

		///<summary>Gets all new data from the database for the text messages.  Not sure yet if this will also reset the lights along the left.</summary>
		private void RefreshFullMessages() {
			_listSigMessages=SigMessages.GetSigMessagesSinceDateTime(DateTimeOD.Today);//since midnight this morning.
			FillMessages();
		}

		///<summary>This does not refresh any data, just fills the grid.</summary>
		private void FillMessages(){
			if(textDays.Visible && errorProvider1.GetError(textDays) !=""){
				return;
			}
			long[] selected=new long[gridMessages.SelectedIndices.Length];
			for(int i=0;i<selected.Length;i++){
				selected[i]=_listSigMessages[gridMessages.SelectedIndices[i]].SigMessageNum;
			}
			gridMessages.BeginUpdate();
			gridMessages.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableTextMessages","To"),60);
			gridMessages.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableTextMessages","From"),60);
			gridMessages.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableTextMessages","Sent"),63);
			gridMessages.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableTextMessages","Ack'd"),63);
			col.TextAlign=HorizontalAlignment.Center;
			gridMessages.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableTextMessages","Text"),274);
			gridMessages.Columns.Add(col);
			gridMessages.Rows.Clear();
			ODGridRow row;
			string str;
			foreach(SigMessage sigMessage in _listSigMessages) {
				if(checkIncludeAck.Checked) {
					if(sigMessage.AckDateTime.Year>1880//if this is acked
						&& sigMessage.AckDateTime < DateTime.Today.AddDays(1-PIn.Long(textDays.Text))) {
						continue;
					}
				}
				else {//user does not want to include acked
					if(sigMessage.AckDateTime.Year > 1880) {//if this is acked
						continue;
					}
				}
				if(sigMessage.ToUser!=""//blank user always shows
					&& comboViewUser.SelectedIndex!=0 //anything other than 'all'
					&& sigElementDefUser!=null//for startup
					&& sigElementDefUser[comboViewUser.SelectedIndex-1].SigText!=sigMessage.ToUser)//and users don't match
				{
					continue;
				}
				row=new ODGridRow();
				row.Cells.Add(sigMessage.ToUser);
				row.Cells.Add(sigMessage.FromUser);
				if(sigMessage.MessageDateTime.Date==DateTime.Today) {
					row.Cells.Add(sigMessage.MessageDateTime.ToShortTimeString());
				}
				else {
					row.Cells.Add(sigMessage.MessageDateTime.ToShortDateString()+"\r\n"+sigMessage.MessageDateTime.ToShortTimeString());
				}
				if(sigMessage.AckDateTime.Year>1880) {//ok
					if(sigMessage.AckDateTime.Date==DateTime.Today) {
						row.Cells.Add(sigMessage.AckDateTime.ToShortTimeString());
					}
					else {
						row.Cells.Add(sigMessage.AckDateTime.ToShortDateString()+"\r\n"+sigMessage.AckDateTime.ToShortTimeString());
					}
				}
				else {
					row.Cells.Add("");
				}
				str=sigMessage.SigText;
				SigElementDef sigElementDefExtra=SigElementDefs.GetElementDef(sigMessage.SigElementDefNumExtra);
				if(sigElementDefExtra!=null && !string.IsNullOrEmpty(sigElementDefExtra.SigText)) {
					str+=(str=="") ? "" : ".  ";
					str+=sigElementDefExtra.SigText;
				}
				SigElementDef sigElementDefMsg=SigElementDefs.GetElementDef(sigMessage.SigElementDefNumMsg);
				if(sigElementDefMsg!=null && !string.IsNullOrEmpty(sigElementDefMsg.SigText)) {
					str+=(str=="") ? "" : ".  ";
					str+=sigElementDefMsg.SigText;
				}
				row.Cells.Add(str);
				row.Tag=sigMessage.Copy();
				gridMessages.Rows.Add(row);
				if(Array.IndexOf(selected,sigMessage.SigMessageNum)!=-1){
					gridMessages.SetSelected(gridMessages.Rows.Count-1,true);
				}
			}
			gridMessages.EndUpdate();
		}

		private void butSend_Click(object sender, System.EventArgs e) {
			if(textMessage.Text=="") {
				MsgBox.Show(this,"Please type in a message first.");
				return;
			}
			SigMessage sigMessage=new SigMessage();
			sigMessage.SigText=textMessage.Text;
			if(listTo.SelectedIndex!=-1) {
				sigMessage.ToUser=sigElementDefUser[listTo.SelectedIndex].SigText;
				sigMessage.SigElementDefNumUser=sigElementDefUser[listTo.SelectedIndex].SigElementDefNum;
			}
			if(listFrom.SelectedIndex!=-1) {
				sigMessage.FromUser=sigElementDefUser[listFrom.SelectedIndex].SigText;
			}
			SigMessages.Insert(sigMessage);
			textMessage.Text="";
			listFrom.SelectedIndex=-1;
			listTo.SelectedIndex=-1;
			listExtras.SelectedIndex=-1;
			listMessages.SelectedIndex=-1;
			labelSending.Visible=true;
			timerSending.Enabled=true;
			Signalods.SetInvalid(InvalidType.SigMessages,KeyType.SigMessage,sigMessage.SigMessageNum);
		}

		private void listMessages_Click(object sender,EventArgs e) {
			if(listMessages.SelectedIndex==-1){
				return;
			}
			SigMessage sigMessage=new SigMessage();
			sigMessage.SigText=textMessage.Text;
			if(listTo.SelectedIndex!=-1) {
				sigMessage.ToUser=sigElementDefUser[listTo.SelectedIndex].SigText;
				sigMessage.SigElementDefNumUser=sigElementDefUser[listTo.SelectedIndex].SigElementDefNum;
			}
			if(listFrom.SelectedIndex!=-1) {
				sigMessage.FromUser=sigElementDefUser[listFrom.SelectedIndex].SigText;
				//We do not set a SigElementDefNumUser for From.
			}
			if(listExtras.SelectedIndex!=-1) {
				sigMessage.SigElementDefNumExtra=sigElementDefExtras[listExtras.SelectedIndex].SigElementDefNum;
			}
			sigMessage.SigElementDefNumMsg=sigElementDefMessages[listMessages.SelectedIndex].SigElementDefNum;
			//need to do this all as a transaction, so need to do a writelock on the signal table first.
			//alternatively, we could just make sure not to retrieve any signals that were less the 300ms old.
			SigMessages.Insert(sigMessage);
			//reset the controls
			textMessage.Text="";
			listFrom.SelectedIndex=-1;
			listTo.SelectedIndex=-1;
			listExtras.SelectedIndex=-1;
			listMessages.SelectedIndex=-1;
			labelSending.Visible=true;
			timerSending.Enabled=true;
			Signalods.SetInvalid(InvalidType.SigMessages,KeyType.SigMessage,sigMessage.SigMessageNum);
		}

		///<summary>This processes timed messages coming in from the main form.
		///Buttons are handled in the main form, and then sent here for further display.
		///The list gets filtered before display.</summary>
		public void LogMsgs(List<SigMessage> listSigMessages) {
			foreach(SigMessage sigMessage in listSigMessages) {
				SigMessage sigMessageUpdate=_listSigMessages.FirstOrDefault(x => x.SigMessageNum==sigMessage.SigMessageNum);
				if(sigMessageUpdate==null) {
					_listSigMessages.Add(sigMessage.Copy());
				}
				else {//SigMessage is already in our list and we just need to update it.
					sigMessageUpdate.AckDateTime=sigMessage.AckDateTime;
				}
			}
			_listSigMessages.Sort();
			FillMessages();
		}

		private void butAck_Click(object sender,EventArgs e) {
			if(gridMessages.SelectedIndices.Length==0){
				MsgBox.Show(this,"Please select at least one item first.");
				return;
			}
			SigMessage sigMessage;
			for(int i=gridMessages.SelectedIndices.Length-1;i>=0;i--){//go backwards so that we can remove rows without problems.
				sigMessage=(SigMessage)gridMessages.Rows[gridMessages.SelectedIndices[i]].Tag;
				if(sigMessage.AckDateTime.Year > 1880) {
					continue;//totally ignore if trying to ack a previously acked signal
				}
				SigMessages.AckSigMessage(sigMessage);
				//change the grid temporarily until the next timer event.  This makes it feel more responsive.
				if(checkIncludeAck.Checked) {
					gridMessages.Rows[gridMessages.SelectedIndices[i]].Cells[3].Text=sigMessage.MessageDateTime.ToShortTimeString();
				}
				else {
					try {
						gridMessages.Rows.RemoveAt(gridMessages.SelectedIndices[i]);
					}
					catch {
						//do nothing
					}
				}
				Signalods.SetInvalid(InvalidType.SigMessages,KeyType.SigMessage,sigMessage.SigMessageNum);
			}
			gridMessages.SetSelected(false);
		}

		private void checkIncludeAck_Click(object sender,EventArgs e) {
			if(checkIncludeAck.Checked){
				textDays.Text="1";
				labelDays.Visible=true;
				textDays.Visible=true;
			}
			else{
				labelDays.Visible=false;
				textDays.Visible=false;
				_listSigMessages=SigMessages.GetSigMessagesSinceDateTime(DateTimeOD.Today);//since midnight this morning.
			}
			FillMessages();
		}

		private void textDays_TextChanged(object sender,EventArgs e) {
			if(!textDays.Visible){
				errorProvider1.SetError(textDays,"");
				return;
			}
			try{
				int days=int.Parse(textDays.Text);
				errorProvider1.SetError(textDays,"");
				_listSigMessages=SigMessages.GetSigMessagesSinceDateTime(DateTimeOD.Today.AddDays(-days));
				FillMessages();
			}
			catch{
				errorProvider1.SetError(textDays,Lan.g(this,"Invalid number.  Usually 1 or 2."));
			}
		}

		private void comboViewUser_SelectionChangeCommitted(object sender,EventArgs e) {
			FillMessages();
		}

		private void timerSending_Tick(object sender,EventArgs e) {
			labelSending.Visible=false;
			timerSending.Enabled=false;
		}

		#endregion Messaging







































		
	}

	public delegate void OnPatientSelected(Patient pat);

}












