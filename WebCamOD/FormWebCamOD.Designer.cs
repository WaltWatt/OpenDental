namespace ProximityOD {
	partial class FormProximityOD {
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProximityOD));
			this.label3 = new System.Windows.Forms.Label();
			this.textProximityDev = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textRange = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textTriggerDev = new System.Windows.Forms.TextBox();
			this.textTriggerSoft = new System.Windows.Forms.TextBox();
			this.textProximitySoft = new System.Windows.Forms.TextBox();
			this.groupOverride = new System.Windows.Forms.GroupBox();
			this.radioOverrideAway = new System.Windows.Forms.RadioButton();
			this.radioOverridePres = new System.Windows.Forms.RadioButton();
			this.radioOverrideAuto = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.menuSetup = new System.Windows.Forms.ToolStripMenuItem();
			this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuOnTop = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.menuMinimize = new System.Windows.Forms.ToolStripMenuItem();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.exitProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.groupOverride.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 47);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(84, 16);
			this.label3.TabIndex = 8;
			this.label3.Text = "Proximity";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProximityDev
			// 
			this.textProximityDev.Location = new System.Drawing.Point(102, 46);
			this.textProximityDev.Name = "textProximityDev";
			this.textProximityDev.Size = new System.Drawing.Size(100, 20);
			this.textProximityDev.TabIndex = 7;
			this.textProximityDev.TabStop = false;
			this.textProximityDev.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(12, 99);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(84, 16);
			this.label4.TabIndex = 10;
			this.label4.Text = "Range";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textRange
			// 
			this.textRange.Location = new System.Drawing.Point(102, 98);
			this.textRange.Name = "textRange";
			this.textRange.Size = new System.Drawing.Size(206, 20);
			this.textRange.TabIndex = 9;
			this.textRange.TabStop = false;
			this.textRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 73);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 16);
			this.label1.TabIndex = 12;
			this.label1.Text = "Trigger Range";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTriggerDev
			// 
			this.textTriggerDev.Enabled = false;
			this.textTriggerDev.Location = new System.Drawing.Point(102, 72);
			this.textTriggerDev.Name = "textTriggerDev";
			this.textTriggerDev.Size = new System.Drawing.Size(100, 20);
			this.textTriggerDev.TabIndex = 11;
			this.textTriggerDev.TabStop = false;
			this.textTriggerDev.Text = "36\"";
			this.textTriggerDev.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// textTriggerSoft
			// 
			this.textTriggerSoft.Location = new System.Drawing.Point(208, 72);
			this.textTriggerSoft.Name = "textTriggerSoft";
			this.textTriggerSoft.Size = new System.Drawing.Size(100, 20);
			this.textTriggerSoft.TabIndex = 16;
			this.textTriggerSoft.TabStop = false;
			this.textTriggerSoft.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.textTriggerSoft.TextChanged += new System.EventHandler(this.textTriggerSoft_TextChanged);
			this.textTriggerSoft.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textTriggerSoft_KeyPress);
			this.textTriggerSoft.Validating += new System.ComponentModel.CancelEventHandler(this.textTriggerSoft_Validating);
			// 
			// textProximitySoft
			// 
			this.textProximitySoft.Location = new System.Drawing.Point(208, 46);
			this.textProximitySoft.Name = "textProximitySoft";
			this.textProximitySoft.Size = new System.Drawing.Size(100, 20);
			this.textProximitySoft.TabIndex = 15;
			this.textProximitySoft.TabStop = false;
			this.textProximitySoft.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// groupOverride
			// 
			this.groupOverride.Controls.Add(this.radioOverrideAway);
			this.groupOverride.Controls.Add(this.radioOverridePres);
			this.groupOverride.Controls.Add(this.radioOverrideAuto);
			this.groupOverride.Location = new System.Drawing.Point(314, 27);
			this.groupOverride.Name = "groupOverride";
			this.groupOverride.Size = new System.Drawing.Size(118, 91);
			this.groupOverride.TabIndex = 19;
			this.groupOverride.TabStop = false;
			this.groupOverride.Text = "Proximity Override";
			// 
			// radioOverrideAway
			// 
			this.radioOverrideAway.AutoSize = true;
			this.radioOverrideAway.Location = new System.Drawing.Point(9, 65);
			this.radioOverrideAway.Name = "radioOverrideAway";
			this.radioOverrideAway.Size = new System.Drawing.Size(51, 17);
			this.radioOverrideAway.TabIndex = 2;
			this.radioOverrideAway.TabStop = true;
			this.radioOverrideAway.Text = "Away";
			this.radioOverrideAway.UseVisualStyleBackColor = true;
			this.radioOverrideAway.CheckedChanged += new System.EventHandler(this.radioOverrideAuto_CheckedChanged);
			// 
			// radioOverridePres
			// 
			this.radioOverridePres.AutoSize = true;
			this.radioOverridePres.Location = new System.Drawing.Point(9, 42);
			this.radioOverridePres.Name = "radioOverridePres";
			this.radioOverridePres.Size = new System.Drawing.Size(61, 17);
			this.radioOverridePres.TabIndex = 1;
			this.radioOverridePres.TabStop = true;
			this.radioOverridePres.Text = "Present";
			this.radioOverridePres.UseVisualStyleBackColor = true;
			this.radioOverridePres.CheckedChanged += new System.EventHandler(this.radioOverrideAuto_CheckedChanged);
			// 
			// radioOverrideAuto
			// 
			this.radioOverrideAuto.AutoSize = true;
			this.radioOverrideAuto.Location = new System.Drawing.Point(9, 19);
			this.radioOverrideAuto.Name = "radioOverrideAuto";
			this.radioOverrideAuto.Size = new System.Drawing.Size(72, 17);
			this.radioOverrideAuto.TabIndex = 0;
			this.radioOverrideAuto.TabStop = true;
			this.radioOverrideAuto.Text = "Automatic";
			this.radioOverrideAuto.UseVisualStyleBackColor = true;
			this.radioOverrideAuto.CheckedChanged += new System.EventHandler(this.radioOverrideAuto_CheckedChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(102, 27);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 20;
			this.label2.Text = "Device";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(208, 27);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 16);
			this.label5.TabIndex = 21;
			this.label5.Text = "Software";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSetup,
            this.windowToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(444, 24);
			this.menuStrip1.TabIndex = 23;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// menuSetup
			// 
			this.menuSetup.CheckOnClick = true;
			this.menuSetup.Name = "menuSetup";
			this.menuSetup.Size = new System.Drawing.Size(49, 20);
			this.menuSetup.Text = "Setup";
			this.menuSetup.CheckedChanged += new System.EventHandler(this.checkSetup_CheckedChanged);
			// 
			// windowToolStripMenuItem
			// 
			this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOnTop,
            this.menuMinimize,
            this.toolStripMenuItem1,
            this.exitProgramToolStripMenuItem});
			this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
			this.windowToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
			this.windowToolStripMenuItem.Text = "Window";
			// 
			// menuOnTop
			// 
			this.menuOnTop.CheckOnClick = true;
			this.menuOnTop.Name = "menuOnTop";
			this.menuOnTop.Size = new System.Drawing.Size(162, 22);
			this.menuOnTop.Text = "Always On Top";
			this.menuOnTop.CheckedChanged += new System.EventHandler(this.checkAlwaysOnTop_CheckedChanged);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(159, 6);
			// 
			// menuMinimize
			// 
			this.menuMinimize.Name = "menuMinimize";
			this.menuMinimize.Size = new System.Drawing.Size(162, 22);
			this.menuMinimize.Text = "Minimize to Tray";
			this.menuMinimize.Click += new System.EventHandler(this.menuMinimize_Click);
			// 
			// notifyIcon
			// 
			this.notifyIcon.BalloonTipText = "Proximity OD has been minimized";
			this.notifyIcon.BalloonTipTitle = "Proximity OD";
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "Proximity OD";
			this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
			// 
			// exitProgramToolStripMenuItem
			// 
			this.exitProgramToolStripMenuItem.Name = "exitProgramToolStripMenuItem";
			this.exitProgramToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.exitProgramToolStripMenuItem.Text = "Exit Program";
			this.exitProgramToolStripMenuItem.Click += new System.EventHandler(this.exitProgramToolStripMenuItem_Click);
			// 
			// FormProximityOD
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(444, 125);
			this.ControlBox = false;
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.groupOverride);
			this.Controls.Add(this.textTriggerSoft);
			this.Controls.Add(this.textProximitySoft);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textTriggerDev);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textRange);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textProximityDev);
			this.Controls.Add(this.menuStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormProximityOD";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Proximity OD";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProximityOD_FormClosing);
			this.Load += new System.EventHandler(this.FormProximityOD_Load);
			this.groupOverride.ResumeLayout(false);
			this.groupOverride.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textProximityDev;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textRange;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textTriggerDev;
		private System.Windows.Forms.TextBox textTriggerSoft;
		private System.Windows.Forms.TextBox textProximitySoft;
		private System.Windows.Forms.GroupBox groupOverride;
		private System.Windows.Forms.RadioButton radioOverrideAway;
		private System.Windows.Forms.RadioButton radioOverridePres;
		private System.Windows.Forms.RadioButton radioOverrideAuto;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem menuSetup;
		private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem menuOnTop;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem menuMinimize;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.ToolStripMenuItem exitProgramToolStripMenuItem;
	}
}

