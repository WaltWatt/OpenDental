﻿namespace OpenDental {
	partial class UserControlPhonePanel {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.menuNumbers = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemManage = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemAdd = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStatus = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemAvailable = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemTraining = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemTeamAssist = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemWrapUp = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemOfflineAssist = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemUnavailable = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemTCResponder = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemRinggroupAll = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemRinggroupNone = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemRinggroupsDefault = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemBackup = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemLunch = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemHome = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemBreak = new System.Windows.Forms.ToolStripMenuItem();
			this.labelMsg = new System.Windows.Forms.Label();
			this.timerMsgs = new System.Windows.Forms.Timer(this.components);
			this.butOverride = new OpenDental.UI.Button();
			this.gridEmp = new OpenDental.UI.ODGrid();
			this.menuNumbers.SuspendLayout();
			this.menuStatus.SuspendLayout();
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Interval = 1600;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// menuNumbers
			// 
			this.menuNumbers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemManage,
            this.menuItemAdd});
			this.menuNumbers.Name = "contextMenuStrip1";
			this.menuNumbers.Size = new System.Drawing.Size(291, 48);
			// 
			// menuItemManage
			// 
			this.menuItemManage.Name = "menuItemManage";
			this.menuItemManage.Size = new System.Drawing.Size(290, 22);
			this.menuItemManage.Text = "Manage Phone Numbers";
			this.menuItemManage.Click += new System.EventHandler(this.menuItemManage_Click);
			// 
			// menuItemAdd
			// 
			this.menuItemAdd.Name = "menuItemAdd";
			this.menuItemAdd.Size = new System.Drawing.Size(290, 22);
			this.menuItemAdd.Text = "Attach Phone Number to Current Patient";
			this.menuItemAdd.Click += new System.EventHandler(this.menuItemAdd_Click);
			// 
			// menuStatus
			// 
			this.menuStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAvailable,
            this.menuItemTraining,
            this.menuItemTeamAssist,
            this.menuItemTCResponder,
            this.menuItemWrapUp,
            this.menuItemOfflineAssist,
            this.menuItemUnavailable,
            this.toolStripMenuItem2,
            this.menuItemRinggroupAll,
            this.menuItemRinggroupNone,
            this.menuItemRinggroupsDefault,
            this.menuItemBackup,
            this.toolStripMenuItem1,
            this.menuItemLunch,
            this.menuItemHome,
            this.menuItemBreak});
			this.menuStatus.Name = "menuStatus";
			this.menuStatus.Size = new System.Drawing.Size(156, 346);
			// 
			// menuItemAvailable
			// 
			this.menuItemAvailable.Name = "menuItemAvailable";
			this.menuItemAvailable.Size = new System.Drawing.Size(155, 22);
			this.menuItemAvailable.Text = "Available";
			this.menuItemAvailable.Click += new System.EventHandler(this.menuItemAvailable_Click);
			// 
			// menuItemTraining
			// 
			this.menuItemTraining.Name = "menuItemTraining";
			this.menuItemTraining.Size = new System.Drawing.Size(155, 22);
			this.menuItemTraining.Text = "Training";
			this.menuItemTraining.Click += new System.EventHandler(this.menuItemTraining_Click);
			// 
			// menuItemTeamAssist
			// 
			this.menuItemTeamAssist.Name = "menuItemTeamAssist";
			this.menuItemTeamAssist.Size = new System.Drawing.Size(155, 22);
			this.menuItemTeamAssist.Text = "TeamAssist";
			this.menuItemTeamAssist.Click += new System.EventHandler(this.menuItemTeamAssist_Click);
			// 
			// menuItemWrapUp
			// 
			this.menuItemWrapUp.Name = "menuItemWrapUp";
			this.menuItemWrapUp.Size = new System.Drawing.Size(155, 22);
			this.menuItemWrapUp.Text = "WrapUp";
			this.menuItemWrapUp.Click += new System.EventHandler(this.menuItemWrapUp_Click);
			// 
			// menuItemOfflineAssist
			// 
			this.menuItemOfflineAssist.Name = "menuItemOfflineAssist";
			this.menuItemOfflineAssist.Size = new System.Drawing.Size(155, 22);
			this.menuItemOfflineAssist.Text = "OfflineAssist";
			this.menuItemOfflineAssist.Click += new System.EventHandler(this.menuItemOfflineAssist_Click);
			// 
			// menuItemUnavailable
			// 
			this.menuItemUnavailable.Name = "menuItemUnavailable";
			this.menuItemUnavailable.Size = new System.Drawing.Size(155, 22);
			this.menuItemUnavailable.Text = "Unavailable";
			this.menuItemUnavailable.Click += new System.EventHandler(this.menuItemUnavailable_Click);
			// 
			// menuItemTCResponder
			// 
			this.menuItemTCResponder.Name = "menuItemTCResponder";
			this.menuItemTCResponder.Size = new System.Drawing.Size(155, 22);
			this.menuItemTCResponder.Text = "TC/Responder";
			this.menuItemTCResponder.Click += new System.EventHandler(this.menuItemTCResponder_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 6);
			// 
			// menuItemRinggroupAll
			// 
			this.menuItemRinggroupAll.Name = "menuItemRinggroupAll";
			this.menuItemRinggroupAll.Size = new System.Drawing.Size(155, 22);
			this.menuItemRinggroupAll.Text = "Queues All";
			this.menuItemRinggroupAll.Click += new System.EventHandler(this.menuItemQueueTech_Click);
			// 
			// menuItemRinggroupNone
			// 
			this.menuItemRinggroupNone.Name = "menuItemRinggroupNone";
			this.menuItemRinggroupNone.Size = new System.Drawing.Size(155, 22);
			this.menuItemRinggroupNone.Text = "Queues None";
			this.menuItemRinggroupNone.Click += new System.EventHandler(this.menuItemQueueNone_Click);
			// 
			// menuItemRinggroupsDefault
			// 
			this.menuItemRinggroupsDefault.Name = "menuItemRinggroupsDefault";
			this.menuItemRinggroupsDefault.Size = new System.Drawing.Size(155, 22);
			this.menuItemRinggroupsDefault.Text = "Queues Default";
			this.menuItemRinggroupsDefault.Click += new System.EventHandler(this.menuItemQueueDefault_Click);
			// 
			// menuItemBackup
			// 
			this.menuItemBackup.Name = "menuItemBackup";
			this.menuItemBackup.Size = new System.Drawing.Size(155, 22);
			this.menuItemBackup.Text = "Backup";
			this.menuItemBackup.Click += new System.EventHandler(this.menuItemQueueBackup_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 6);
			// 
			// menuItemLunch
			// 
			this.menuItemLunch.Name = "menuItemLunch";
			this.menuItemLunch.Size = new System.Drawing.Size(155, 22);
			this.menuItemLunch.Text = "Lunch";
			this.menuItemLunch.Click += new System.EventHandler(this.menuItemLunch_Click);
			// 
			// menuItemHome
			// 
			this.menuItemHome.Name = "menuItemHome";
			this.menuItemHome.Size = new System.Drawing.Size(155, 22);
			this.menuItemHome.Text = "Home";
			this.menuItemHome.Click += new System.EventHandler(this.menuItemHome_Click);
			// 
			// menuItemBreak
			// 
			this.menuItemBreak.Name = "menuItemBreak";
			this.menuItemBreak.Size = new System.Drawing.Size(155, 22);
			this.menuItemBreak.Text = "Break";
			this.menuItemBreak.Click += new System.EventHandler(this.menuItemBreak_Click);
			// 
			// labelMsg
			// 
			this.labelMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelMsg.ForeColor = System.Drawing.Color.Firebrick;
			this.labelMsg.Location = new System.Drawing.Point(141, 2);
			this.labelMsg.Name = "labelMsg";
			this.labelMsg.Size = new System.Drawing.Size(198, 20);
			this.labelMsg.TabIndex = 25;
			this.labelMsg.Text = "Phone Messages: 0";
			this.labelMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// timerMsgs
			// 
			this.timerMsgs.Interval = 3000;
			this.timerMsgs.Tick += new System.EventHandler(this.timerMsgs_Tick);
			// 
			// butOverride
			// 
			this.butOverride.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOverride.Autosize = true;
			this.butOverride.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOverride.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOverride.CornerRadius = 4F;
			this.butOverride.Location = new System.Drawing.Point(0, 0);
			this.butOverride.Name = "butOverride";
			this.butOverride.Size = new System.Drawing.Size(75, 24);
			this.butOverride.TabIndex = 24;
			this.butOverride.Text = "Override";
			this.butOverride.Click += new System.EventHandler(this.butOverride_Click);
			// 
			// gridEmp
			// 
			this.gridEmp.AllowSelection = false;
			this.gridEmp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridEmp.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridEmp.HasAddButton = false;
			this.gridEmp.HasDropDowns = false;
			this.gridEmp.HasMultilineHeaders = false;
			this.gridEmp.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridEmp.HeaderHeight = 15;
			this.gridEmp.HScrollVisible = false;
			this.gridEmp.Location = new System.Drawing.Point(0, 24);
			this.gridEmp.Name = "gridEmp";
			this.gridEmp.ScrollValue = 0;
			this.gridEmp.Size = new System.Drawing.Size(428, 299);
			this.gridEmp.TabIndex = 22;
			this.gridEmp.Title = "Phones";
			this.gridEmp.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridEmp.TitleHeight = 18;
			this.gridEmp.TranslationName = "TableEmpClock";
			this.gridEmp.WrapText = false;
			this.gridEmp.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridEmp_CellClick);
			this.gridEmp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridEmp_MouseUp);
			// 
			// UserControlPhonePanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.labelMsg);
			this.Controls.Add(this.butOverride);
			this.Controls.Add(this.gridEmp);
			this.Name = "UserControlPhonePanel";
			this.Size = new System.Drawing.Size(428, 323);
			this.Load += new System.EventHandler(this.UserControlPhonePanel_Load);
			this.menuNumbers.ResumeLayout(false);
			this.menuStatus.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.ODGrid gridEmp;
		private System.Windows.Forms.Timer timer1;
		private OpenDental.UI.Button butOverride;
		private System.Windows.Forms.ContextMenuStrip menuNumbers;
		private System.Windows.Forms.ToolStripMenuItem menuItemManage;
		private System.Windows.Forms.ToolStripMenuItem menuItemAdd;
		private System.Windows.Forms.ContextMenuStrip menuStatus;
		private System.Windows.Forms.ToolStripMenuItem menuItemAvailable;
		private System.Windows.Forms.ToolStripMenuItem menuItemTraining;
		private System.Windows.Forms.ToolStripMenuItem menuItemTeamAssist;
		private System.Windows.Forms.ToolStripMenuItem menuItemWrapUp;
		private System.Windows.Forms.ToolStripMenuItem menuItemOfflineAssist;
		private System.Windows.Forms.ToolStripMenuItem menuItemUnavailable;
		private System.Windows.Forms.ToolStripMenuItem menuItemBreak;
		private System.Windows.Forms.ToolStripMenuItem menuItemLunch;
		private System.Windows.Forms.ToolStripMenuItem menuItemHome;
		private System.Windows.Forms.Label labelMsg;
		private System.Windows.Forms.Timer timerMsgs;
		private System.Windows.Forms.ToolStripMenuItem menuItemBackup;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem menuItemRinggroupAll;
		private System.Windows.Forms.ToolStripMenuItem menuItemRinggroupNone;
		private System.Windows.Forms.ToolStripMenuItem menuItemRinggroupsDefault;
		private System.Windows.Forms.ToolStripMenuItem menuItemTCResponder;
	}
}
