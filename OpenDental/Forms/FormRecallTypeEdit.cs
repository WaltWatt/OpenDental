using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormRecallTypeEdit:ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textDescription;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private GroupBox groupInterval;
		private ValidNum textWeeks;
		private Label label7;
		private ValidNum textDays;
		private Label label6;
		private ValidNum textMonths;
		private Label label9;
		private ValidNum textYears;
		private Label label10;
		private TextBox textPattern;
		private Label label12;
		private Label label14;
		private Label label15;
		private ListBox listProcs;
		private GroupBox groupBox2;
		private OpenDental.UI.Button butRemoveProc;
		private OpenDental.UI.Button butAddProc;
		private OpenDental.UI.Button butRemoveTrigger;
		private OpenDental.UI.Button butAddTrigger;
		private ListBox listTriggers;
		private Label labelTriggers;
		private Label label3;
		private ComboBox comboSpecial;
		private Label labelSpecial;
		private Label labelTriggerDisable;
		public RecallType RecallTypeCur;
		private OpenDental.UI.Button butDelete;
		private Label label4;
		private List<RecallTrigger> TriggerList;
		private Interval defaultIntervalOld;
		private Label label5;
		private GroupBox groupAgeLimit;
		private ValidNum textRecallAgeAdult;
		private Label label8;
		private Label label17;
		private CheckBox checkAppendToSpecial;
		private Label label2;
		private int CountForType;

		///<summary></summary>
		public FormRecallTypeEdit()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecallTypeEdit));
			this.label1 = new System.Windows.Forms.Label();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.groupInterval = new System.Windows.Forms.GroupBox();
			this.textWeeks = new OpenDental.ValidNum();
			this.label7 = new System.Windows.Forms.Label();
			this.textDays = new OpenDental.ValidNum();
			this.label6 = new System.Windows.Forms.Label();
			this.textMonths = new OpenDental.ValidNum();
			this.label9 = new System.Windows.Forms.Label();
			this.textYears = new OpenDental.ValidNum();
			this.label10 = new System.Windows.Forms.Label();
			this.textPattern = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.listProcs = new System.Windows.Forms.ListBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.butRemoveProc = new OpenDental.UI.Button();
			this.butAddProc = new OpenDental.UI.Button();
			this.listTriggers = new System.Windows.Forms.ListBox();
			this.labelTriggers = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.comboSpecial = new System.Windows.Forms.ComboBox();
			this.labelSpecial = new System.Windows.Forms.Label();
			this.butRemoveTrigger = new OpenDental.UI.Button();
			this.butAddTrigger = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.labelTriggerDisable = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.groupAgeLimit = new System.Windows.Forms.GroupBox();
			this.textRecallAgeAdult = new OpenDental.ValidNum();
			this.label8 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.checkAppendToSpecial = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupInterval.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupAgeLimit.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(148, 17);
			this.label1.TabIndex = 2;
			this.label1.Text = "Description";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(160, 12);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(291, 20);
			this.textDescription.TabIndex = 0;
			// 
			// groupInterval
			// 
			this.groupInterval.Controls.Add(this.textWeeks);
			this.groupInterval.Controls.Add(this.label7);
			this.groupInterval.Controls.Add(this.textDays);
			this.groupInterval.Controls.Add(this.label6);
			this.groupInterval.Controls.Add(this.textMonths);
			this.groupInterval.Controls.Add(this.label9);
			this.groupInterval.Controls.Add(this.textYears);
			this.groupInterval.Controls.Add(this.label10);
			this.groupInterval.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupInterval.Location = new System.Drawing.Point(55, 282);
			this.groupInterval.Name = "groupInterval";
			this.groupInterval.Size = new System.Drawing.Size(170, 115);
			this.groupInterval.TabIndex = 116;
			this.groupInterval.TabStop = false;
			this.groupInterval.Text = "Default Interval";
			// 
			// textWeeks
			// 
			this.textWeeks.Location = new System.Drawing.Point(105, 64);
			this.textWeeks.MaxVal = 255;
			this.textWeeks.MinVal = 0;
			this.textWeeks.Name = "textWeeks";
			this.textWeeks.Size = new System.Drawing.Size(51, 20);
			this.textWeeks.TabIndex = 2;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(11, 64);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(92, 19);
			this.label7.TabIndex = 11;
			this.label7.Text = "Weeks";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDays
			// 
			this.textDays.Location = new System.Drawing.Point(105, 86);
			this.textDays.MaxVal = 255;
			this.textDays.MinVal = 0;
			this.textDays.Name = "textDays";
			this.textDays.Size = new System.Drawing.Size(51, 20);
			this.textDays.TabIndex = 3;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(11, 86);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(92, 19);
			this.label6.TabIndex = 9;
			this.label6.Text = "Days";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMonths
			// 
			this.textMonths.Location = new System.Drawing.Point(105, 41);
			this.textMonths.MaxVal = 255;
			this.textMonths.MinVal = 0;
			this.textMonths.Name = "textMonths";
			this.textMonths.Size = new System.Drawing.Size(51, 20);
			this.textMonths.TabIndex = 1;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(11, 41);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(92, 19);
			this.label9.TabIndex = 7;
			this.label9.Text = "Months";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textYears
			// 
			this.textYears.Location = new System.Drawing.Point(105, 18);
			this.textYears.MaxVal = 127;
			this.textYears.MinVal = 0;
			this.textYears.Name = "textYears";
			this.textYears.Size = new System.Drawing.Size(51, 20);
			this.textYears.TabIndex = 0;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(11, 18);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(92, 19);
			this.label10.TabIndex = 5;
			this.label10.Text = "Years";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPattern
			// 
			this.textPattern.Location = new System.Drawing.Point(105, 19);
			this.textPattern.Name = "textPattern";
			this.textPattern.Size = new System.Drawing.Size(170, 20);
			this.textPattern.TabIndex = 0;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(283, 21);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(117, 19);
			this.label12.TabIndex = 121;
			this.label12.Text = "(only /\'s and X\'s)";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(1, 23);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(100, 16);
			this.label14.TabIndex = 119;
			this.label14.Text = "Time Pattern";
			this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(1, 45);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(98, 33);
			this.label15.TabIndex = 120;
			this.label15.Text = "Procedures on Appointment";
			this.label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// listProcs
			// 
			this.listProcs.FormattingEnabled = true;
			this.listProcs.Location = new System.Drawing.Point(105, 45);
			this.listProcs.Name = "listProcs";
			this.listProcs.Size = new System.Drawing.Size(220, 108);
			this.listProcs.TabIndex = 1;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.butRemoveProc);
			this.groupBox2.Controls.Add(this.butAddProc);
			this.groupBox2.Controls.Add(this.textPattern);
			this.groupBox2.Controls.Add(this.listProcs);
			this.groupBox2.Controls.Add(this.label15);
			this.groupBox2.Controls.Add(this.label14);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Location = new System.Drawing.Point(55, 408);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(418, 160);
			this.groupBox2.TabIndex = 123;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "When automatically creating appointments";
			// 
			// butRemoveProc
			// 
			this.butRemoveProc.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRemoveProc.Autosize = true;
			this.butRemoveProc.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRemoveProc.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRemoveProc.CornerRadius = 4F;
			this.butRemoveProc.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butRemoveProc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butRemoveProc.Location = new System.Drawing.Point(331, 129);
			this.butRemoveProc.Name = "butRemoveProc";
			this.butRemoveProc.Size = new System.Drawing.Size(78, 24);
			this.butRemoveProc.TabIndex = 3;
			this.butRemoveProc.Text = "Remove";
			this.butRemoveProc.Click += new System.EventHandler(this.butRemoveProc_Click);
			// 
			// butAddProc
			// 
			this.butAddProc.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddProc.Autosize = true;
			this.butAddProc.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddProc.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddProc.CornerRadius = 4F;
			this.butAddProc.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddProc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddProc.Location = new System.Drawing.Point(331, 99);
			this.butAddProc.Name = "butAddProc";
			this.butAddProc.Size = new System.Drawing.Size(78, 24);
			this.butAddProc.TabIndex = 2;
			this.butAddProc.Text = "Add";
			this.butAddProc.Click += new System.EventHandler(this.butAddProc_Click);
			// 
			// listTriggers
			// 
			this.listTriggers.FormattingEnabled = true;
			this.listTriggers.Location = new System.Drawing.Point(160, 90);
			this.listTriggers.Name = "listTriggers";
			this.listTriggers.Size = new System.Drawing.Size(220, 186);
			this.listTriggers.TabIndex = 3;
			// 
			// labelTriggers
			// 
			this.labelTriggers.Location = new System.Drawing.Point(39, 90);
			this.labelTriggers.Name = "labelTriggers";
			this.labelTriggers.Size = new System.Drawing.Size(115, 41);
			this.labelTriggers.TabIndex = 125;
			this.labelTriggers.Text = "Procedures that trigger this recall type";
			this.labelTriggers.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 39);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(148, 17);
			this.label3.TabIndex = 129;
			this.label3.Text = "Special Type";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboSpecial
			// 
			this.comboSpecial.FormattingEnabled = true;
			this.comboSpecial.Location = new System.Drawing.Point(160, 38);
			this.comboSpecial.Name = "comboSpecial";
			this.comboSpecial.Size = new System.Drawing.Size(220, 21);
			this.comboSpecial.TabIndex = 1;
			this.comboSpecial.SelectionChangeCommitted += new System.EventHandler(this.comboSpecial_SelectionChangeCommitted);
			// 
			// labelSpecial
			// 
			this.labelSpecial.Location = new System.Drawing.Point(386, 39);
			this.labelSpecial.Name = "labelSpecial";
			this.labelSpecial.Size = new System.Drawing.Size(320, 22);
			this.labelSpecial.TabIndex = 131;
			this.labelSpecial.Text = "labelSpecial";
			// 
			// butRemoveTrigger
			// 
			this.butRemoveTrigger.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRemoveTrigger.Autosize = true;
			this.butRemoveTrigger.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRemoveTrigger.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRemoveTrigger.CornerRadius = 4F;
			this.butRemoveTrigger.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butRemoveTrigger.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butRemoveTrigger.Location = new System.Drawing.Point(386, 252);
			this.butRemoveTrigger.Name = "butRemoveTrigger";
			this.butRemoveTrigger.Size = new System.Drawing.Size(78, 24);
			this.butRemoveTrigger.TabIndex = 5;
			this.butRemoveTrigger.Text = "Remove";
			this.butRemoveTrigger.Click += new System.EventHandler(this.butRemoveTrigger_Click);
			// 
			// butAddTrigger
			// 
			this.butAddTrigger.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddTrigger.Autosize = true;
			this.butAddTrigger.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddTrigger.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddTrigger.CornerRadius = 4F;
			this.butAddTrigger.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddTrigger.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddTrigger.Location = new System.Drawing.Point(386, 222);
			this.butAddTrigger.Name = "butAddTrigger";
			this.butAddTrigger.Size = new System.Drawing.Size(78, 24);
			this.butAddTrigger.TabIndex = 4;
			this.butAddTrigger.Text = "Add";
			this.butAddTrigger.Click += new System.EventHandler(this.butAddTrigger_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(540, 622);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 6;
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
			this.butCancel.Location = new System.Drawing.Point(631, 622);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 7;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// labelTriggerDisable
			// 
			this.labelTriggerDisable.Location = new System.Drawing.Point(386, 90);
			this.labelTriggerDisable.Name = "labelTriggerDisable";
			this.labelTriggerDisable.Size = new System.Drawing.Size(263, 41);
			this.labelTriggerDisable.TabIndex = 132;
			this.labelTriggerDisable.Text = "A manual recall type will have no triggers.  To disable an automatic recall type," +
    " clear out the triggers.";
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
			this.butDelete.Location = new System.Drawing.Point(42, 622);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(78, 24);
			this.butDelete.TabIndex = 8;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.Location = new System.Drawing.Point(130, 604);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(263, 43);
			this.label4.TabIndex = 134;
			this.label4.Text = "There\'s no way yet to delete a recall type.  This deletes all patient recalls of " +
    "this type.";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(14, 26);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(418, 48);
			this.label5.TabIndex = 5;
			this.label5.Text = "Automatically used if a Prophy patient is under the age limit set below.  Does no" +
    "t include triggers or interval since the triggers and interval from the Prophy t" +
    "ype are used instead.";
			// 
			// groupAgeLimit
			// 
			this.groupAgeLimit.Controls.Add(this.textRecallAgeAdult);
			this.groupAgeLimit.Controls.Add(this.label5);
			this.groupAgeLimit.Controls.Add(this.label8);
			this.groupAgeLimit.Controls.Add(this.label17);
			this.groupAgeLimit.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupAgeLimit.Location = new System.Drawing.Point(55, 121);
			this.groupAgeLimit.Name = "groupAgeLimit";
			this.groupAgeLimit.Size = new System.Drawing.Size(448, 142);
			this.groupAgeLimit.TabIndex = 116;
			this.groupAgeLimit.TabStop = false;
			this.groupAgeLimit.Text = "Child Prophy";
			// 
			// textRecallAgeAdult
			// 
			this.textRecallAgeAdult.Location = new System.Drawing.Point(105, 80);
			this.textRecallAgeAdult.MaxVal = 127;
			this.textRecallAgeAdult.MinVal = 0;
			this.textRecallAgeAdult.Name = "textRecallAgeAdult";
			this.textRecallAgeAdult.Size = new System.Drawing.Size(51, 20);
			this.textRecallAgeAdult.TabIndex = 0;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(162, 81);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(74, 19);
			this.label8.TabIndex = 5;
			this.label8.Text = "Years";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(11, 80);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(92, 19);
			this.label17.TabIndex = 5;
			this.label17.Text = "Age Limit";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAppendToSpecial
			// 
			this.checkAppendToSpecial.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAppendToSpecial.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAppendToSpecial.Location = new System.Drawing.Point(9, 65);
			this.checkAppendToSpecial.Name = "checkAppendToSpecial";
			this.checkAppendToSpecial.Size = new System.Drawing.Size(163, 17);
			this.checkAppendToSpecial.TabIndex = 2;
			this.checkAppendToSpecial.TabStop = false;
			this.checkAppendToSpecial.Text = "Append to Special";
			this.checkAppendToSpecial.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(178, 65);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(528, 16);
			this.label2.TabIndex = 136;
			this.label2.Text = "If needed, automatically add this recall type when scheduling a special recall ty" +
    "pe.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormRecallTypeEdit
			// 
			this.ClientSize = new System.Drawing.Size(732, 666);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.checkAppendToSpecial);
			this.Controls.Add(this.groupAgeLimit);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.labelTriggerDisable);
			this.Controls.Add(this.labelSpecial);
			this.Controls.Add(this.comboSpecial);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.butRemoveTrigger);
			this.Controls.Add(this.butAddTrigger);
			this.Controls.Add(this.listTriggers);
			this.Controls.Add(this.labelTriggers);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupInterval);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormRecallTypeEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Recall Type";
			this.Load += new System.EventHandler(this.FormRecallTypeEdit_Load);
			this.groupInterval.ResumeLayout(false);
			this.groupInterval.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupAgeLimit.ResumeLayout(false);
			this.groupAgeLimit.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormRecallTypeEdit_Load(object sender, System.EventArgs e) {
			textDescription.Text=RecallTypeCur.Description;
		 	defaultIntervalOld=RecallTypeCur.DefaultInterval;
			comboSpecial.Items.Add(Lan.g(this,"none"));
			comboSpecial.Items.Add(Lan.g(this,"Prophy"));
			comboSpecial.Items.Add(Lan.g(this,"ChildProphy"));
			comboSpecial.Items.Add(Lan.g(this,"Perio"));
			SetSpecialIdx();
			CountForType=Recalls.GetCountForType(RecallTypeCur.RecallTypeNum);
			TriggerList=RecallTriggers.GetForType(RecallTypeCur.RecallTypeNum);//works if 0, too.
			SetSpecialText();
			FillTriggers();
			if(PrefC.GetLong(PrefName.RecallTypeSpecialChildProphy)==RecallTypeCur.RecallTypeNum){
				textRecallAgeAdult.Text=PrefC.GetInt(PrefName.RecallAgeAdult).ToString();
			}
			textYears.Text=RecallTypeCur.DefaultInterval.Years.ToString();
			textMonths.Text=RecallTypeCur.DefaultInterval.Months.ToString();
			textWeeks.Text=RecallTypeCur.DefaultInterval.Weeks.ToString();
			textDays.Text=RecallTypeCur.DefaultInterval.Days.ToString();
			textPattern.Text=RecallTypeCur.TimePattern;
			checkAppendToSpecial.Checked=RecallTypeCur.AppendToSpecial;
			FillProcs();
		}

		private void SetSpecialIdx() {
			comboSpecial.SelectedIndex=0;
			if(PrefC.GetLong(PrefName.RecallTypeSpecialProphy)==RecallTypeCur.RecallTypeNum){
				comboSpecial.SelectedIndex=1;
			}
			else if(PrefC.GetLong(PrefName.RecallTypeSpecialChildProphy)==RecallTypeCur.RecallTypeNum){
				comboSpecial.SelectedIndex=2;
			}
			else if(PrefC.GetLong(PrefName.RecallTypeSpecialPerio)==RecallTypeCur.RecallTypeNum){
				comboSpecial.SelectedIndex=3;
			}
		}

		private void comboSpecial_SelectionChangeCommitted(object sender,EventArgs e) {
			if(CountForType>0){
				MessageBox.Show(Lan.g(this,"Cannot change Special Type. Patients using this recall type: ") + CountForType.ToString());
				SetSpecialIdx();//sets back to what it was when form opened
				return;
			}
			//cannot change a special type to one that is already in set for another recall type if that recall type is in use by patients
			long recallTypeNumPrev=0;
			switch(comboSpecial.SelectedIndex) {
				case 1:
					recallTypeNumPrev=PrefC.GetLong(PrefName.RecallTypeSpecialProphy);
					break;
				case 2:
					recallTypeNumPrev=PrefC.GetLong(PrefName.RecallTypeSpecialChildProphy);
					break;
				case 3:
					recallTypeNumPrev=PrefC.GetLong(PrefName.RecallTypeSpecialPerio);
					break;
			}
			int countForTypePrev=0;
			if(recallTypeNumPrev>0) {
				countForTypePrev=Recalls.GetCountForType(recallTypeNumPrev);
			}
			if(countForTypePrev>0) {
				MessageBox.Show(Lan.g(this,"Cannot change Special Type to one that is set for another recall type and in use by patients.  "
					+"Patients using the other recall type: ")+countForTypePrev.ToString());
				SetSpecialIdx();//sets back to what it was when form opened
				return;
			}
			SetSpecialText();
		}

		private void SetSpecialText(){
			if(comboSpecial.SelectedIndex==0){
				labelSpecial.Text="";
				listTriggers.Visible=true;
				labelTriggers.Visible=true;
				labelTriggerDisable.Visible=true;
				butAddTrigger.Visible=true;
				butRemoveTrigger.Visible=true;
				groupInterval.Visible=true;
				groupAgeLimit.Visible=false;
			}
			else if(comboSpecial.SelectedIndex==1){//prophy
				labelSpecial.Text="Should include triggers for ChildProphy.";
				listTriggers.Visible=true;
				labelTriggers.Visible=true;
				labelTriggerDisable.Visible=true;
				butAddTrigger.Visible=true;
				butRemoveTrigger.Visible=true;
				groupInterval.Visible=true;
				groupAgeLimit.Visible=false;
			}
			else if(comboSpecial.SelectedIndex==2){//childProphy
				labelSpecial.Text="";//the description is now inside a special group box.
				listTriggers.Visible=false;
				labelTriggers.Visible=false;
				labelTriggerDisable.Visible=false;
				butAddTrigger.Visible=false;
				butRemoveTrigger.Visible=false;
				groupInterval.Visible=false;
				groupAgeLimit.Visible=true;
			}
			else if(comboSpecial.SelectedIndex==3){//Perio
				labelSpecial.Text="Should include triggers.";
				listTriggers.Visible=true;
				labelTriggers.Visible=true;
				labelTriggerDisable.Visible=true;
				butAddTrigger.Visible=true;
				butRemoveTrigger.Visible=true;
				groupInterval.Visible=true;
				groupAgeLimit.Visible=false;
			}
		}

		private void FillTriggers(){
			listTriggers.Items.Clear();
			if(TriggerList.Count==0 || comboSpecial.SelectedIndex==2) {//child prophy special type has no triggers, triggers from Prophy type are used
				return;
			}
			string str;
			for(int i=0;i<TriggerList.Count;i++){
				str=ProcedureCodes.GetStringProcCode(TriggerList[i].CodeNum);
				str+="- "+ProcedureCodes.GetLaymanTerm(TriggerList[i].CodeNum);
				listTriggers.Items.Add(str);
			}
		}

		private void butAddTrigger_Click(object sender,EventArgs e) {
			FormProcCodes FormP=new FormProcCodes();
			FormP.IsSelectionMode=true;
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK){
				return;
			}
			RecallTrigger trigger=new RecallTrigger();
			trigger.CodeNum=FormP.SelectedCodeNum;
			//RecallTypeNum handled during save.
			TriggerList.Add(trigger);
			FillTriggers();
		}

		private void butRemoveTrigger_Click(object sender,EventArgs e) {
			if(listTriggers.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a trigger code first.");
				return;
			}
			TriggerList.RemoveAt(listTriggers.SelectedIndex);
			FillTriggers();
		}

		private void FillProcs(){
			listProcs.Items.Clear();
			if(RecallTypeCur.Procedures==null || RecallTypeCur.Procedures==""){
				return;
			}
			string[] strArray=RecallTypeCur.Procedures.Split(',');
			string str;
			for(int i=0;i<strArray.Length;i++){
				str=strArray[i];
				str+="- "+ProcedureCodes.GetLaymanTerm(ProcedureCodes.GetProcCode(str).CodeNum);
				listProcs.Items.Add(str);
			}
		}

		private void butAddProc_Click(object sender,EventArgs e) {
			FormProcCodes FormP=new FormProcCodes();
			FormP.IsSelectionMode=true;
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK){
				return;
			}
			if(!String.IsNullOrEmpty(RecallTypeCur.Procedures)){
				RecallTypeCur.Procedures+=",";
			}
			RecallTypeCur.Procedures+=ProcedureCodes.GetStringProcCode(FormP.SelectedCodeNum);
			FillProcs();
		}

		private void butRemoveProc_Click(object sender,EventArgs e) {
			if(listProcs.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a procedure first.");
				return;
			}
			string[] strArray=RecallTypeCur.Procedures.Split(',');
			List<string> strList=new List<string>(strArray);
			strList.RemoveAt(listProcs.SelectedIndex);
			RecallTypeCur.Procedures="";
			for(int i=0;i<strList.Count;i++){
				if(i>0){
					RecallTypeCur.Procedures+=",";
				}
				RecallTypeCur.Procedures+=strList[i];
			}
			FillProcs();
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			//Before deleting the actual type, we would need to check special types.
			/*if(RecallCur.IsNew){
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!MsgBox.Show(this,true,"Delete this RecallType?")) {
				return;
			}
			try{
				Pharmacies.DeleteObject(PharmCur.PharmacyNum);
				DialogResult=DialogResult.OK;
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
			}*/
			if(!Security.IsAuthorized(Permissions.SecurityAdmin)) {
				return;
			}
			if(listTriggers.Items.Count>0) {
				MsgBox.Show(this,"All triggers must first be deleted.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Are you absolutely sure you want to delete all recalls of this type?")) {
				return;
			}
			SecurityLogs.MakeLogEntry(Permissions.RecallEdit,0,"Recall type deleted with description '"+RecallTypeCur.Description+"'");
			Recalls.DeleteAllOfType(RecallTypeCur.RecallTypeNum);
			CountForType=Recalls.GetCountForType(RecallTypeCur.RecallTypeNum);
			MsgBox.Show(this,"Done.");

		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textDescription.Text==""){
				MsgBox.Show(this,"Description cannot be blank.");
				return;
			}
			for(int i=0;i<textPattern.Text.Length;i++) {
				if(textPattern.Text[i]!='/' && textPattern.Text[i]!='X') {
					MsgBox.Show(this,"Time Pattern may only contain '/' and 'X'.  Please fix to continue.");
					return;
				}
			}
			if(  textYears.errorProvider1.GetError(textYears)!=""
				|| textMonths.errorProvider1.GetError(textMonths)!=""
				|| textWeeks.errorProvider1.GetError(textWeeks)!=""
				|| textDays.errorProvider1.GetError(textDays)!=""
				){
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			//if(RecallTypes.List comboSpecial.SelectedIndex
			/*
			if(listTriggers.Items.Count==0 && comboSpecial.SelectedIndex!=2) {//except child prophy
				if(!MsgBox.Show(this,true,"Warning! clearing all triggers for a recall type will cause all patient recalls of that type to be deleted, even those with notes.  Continue anyway?")){
					return;
				}
			}*/
			bool changed=false;
			if(comboSpecial.SelectedIndex==2){//childProphy
				if(textRecallAgeAdult.errorProvider1.GetError(textRecallAgeAdult)!=""){
					MsgBox.Show(this,"Please fix data entry errors first.");
					return;
				}
				if(Prefs.UpdateInt(PrefName.RecallAgeAdult,PIn.Int(textRecallAgeAdult.Text))) {
					changed=true;
				}
				TriggerList.Clear();//triggers for child prophy special type are handled by the prophy special type
			}
			else {//for child prophy, interval will default to 0, since this special type uses the Prophy default interval
				Interval interval=new Interval(
					PIn.Int(textDays.Text),
					PIn.Int(textWeeks.Text),
					PIn.Int(textMonths.Text),
					PIn.Int(textYears.Text));
				RecallTypeCur.DefaultInterval=interval;
			}
			RecallTypeCur.Description=textDescription.Text;
			RecallTypeCur.TimePattern=textPattern.Text;
			RecallTypeCur.AppendToSpecial=checkAppendToSpecial.Checked;
			if(listProcs.Items.Count==0){
				RecallTypeCur.Procedures="";
			}
			//otherwise, already taken care of.
			try{
				if(RecallTypeCur.IsNew) {
					RecallTypes.Insert(RecallTypeCur);
					SecurityLogs.MakeLogEntry(Permissions.RecallEdit,0,"Recall type added '"+RecallTypeCur.Description+"'");
				}
				else {
					SecurityLogs.MakeLogEntry(Permissions.RecallEdit,0,"Recall type having description '"+RecallTypeCur.Description+"' edited");
					RecallTypes.Update(RecallTypeCur);
				}
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
				return;
			}
			RecallTriggers.SetForType(RecallTypeCur.RecallTypeNum,TriggerList);
			//The combo for special type is allowed to be changed by user.  But since the field is in the pref table instead of in the RecallType table, there's extra work involved in saving the selection.
			if(comboSpecial.SelectedIndex==0){//none:  If this recall type is now not any special type
				if(PrefC.GetLong(PrefName.RecallTypeSpecialProphy)==RecallTypeCur.RecallTypeNum){//and it used to be the special prophy type
					Prefs.UpdateLong(PrefName.RecallTypeSpecialProphy,0);
					changed=true;
				}
				if(PrefC.GetLong(PrefName.RecallTypeSpecialChildProphy)==RecallTypeCur.RecallTypeNum){
					Prefs.UpdateLong(PrefName.RecallTypeSpecialChildProphy,0);
					changed=true;
				}
				if(PrefC.GetLong(PrefName.RecallTypeSpecialPerio)==RecallTypeCur.RecallTypeNum){
					Prefs.UpdateLong(PrefName.RecallTypeSpecialPerio,0);
					changed=true;
				}
			}
			else if(comboSpecial.SelectedIndex==1){//Prophy: If this recall type is now the prophy type.
				if(Prefs.UpdateLong(PrefName.RecallTypeSpecialProphy,RecallTypeCur.RecallTypeNum)){//and it was already the prophy type
					changed=true;
				}
				if(PrefC.GetLong(PrefName.RecallTypeSpecialChildProphy)==RecallTypeCur.RecallTypeNum){//but it used to be the childprophy type.
					Prefs.UpdateLong(PrefName.RecallTypeSpecialChildProphy,0);
					changed=true;
				}
				if(PrefC.GetLong(PrefName.RecallTypeSpecialPerio)==RecallTypeCur.RecallTypeNum){
					Prefs.UpdateLong(PrefName.RecallTypeSpecialPerio,0);
					changed=true;
				}
			}
			else if(comboSpecial.SelectedIndex==2){//ChildProphy
				if(PrefC.GetLong(PrefName.RecallTypeSpecialProphy)==RecallTypeCur.RecallTypeNum){
					Prefs.UpdateLong(PrefName.RecallTypeSpecialProphy,0);
					changed=true;
				}
				if(Prefs.UpdateLong(PrefName.RecallTypeSpecialChildProphy,RecallTypeCur.RecallTypeNum)){
					changed=true;
				}
				if(PrefC.GetLong(PrefName.RecallTypeSpecialPerio)==RecallTypeCur.RecallTypeNum){
					Prefs.UpdateLong(PrefName.RecallTypeSpecialPerio,0);
					changed=true;
				}
			}
			else if(comboSpecial.SelectedIndex==3){//Perio
				if(PrefC.GetLong(PrefName.RecallTypeSpecialProphy)==RecallTypeCur.RecallTypeNum){
					Prefs.UpdateLong(PrefName.RecallTypeSpecialProphy,0);
					changed=true;
				}
				if(PrefC.GetLong(PrefName.RecallTypeSpecialChildProphy)==RecallTypeCur.RecallTypeNum){
					Prefs.UpdateLong(PrefName.RecallTypeSpecialChildProphy,0);
					changed=true;
				}
				if(Prefs.UpdateLong(PrefName.RecallTypeSpecialPerio,RecallTypeCur.RecallTypeNum)){
					changed=true;
				}
			}
			DataValid.SetInvalid(InvalidType.RecallTypes);
			if(changed){
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			//Ask user to update recalls for patients if they changed the DefaultInterval.
			if(!RecallTypeCur.IsNew && defaultIntervalOld!=RecallTypeCur.DefaultInterval) {
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Default interval has been changed.  Reset all current patient intervals of this type?")) {
					Recalls.UpdateDefaultIntervalForPatients(RecallTypeCur.RecallTypeNum,defaultIntervalOld,RecallTypeCur.DefaultInterval);
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	

		

		

		

		

		

		

		

		


	}
}





















