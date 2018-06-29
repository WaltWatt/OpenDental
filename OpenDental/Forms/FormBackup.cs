using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using CodeBase;
using OpenDental.Bridges;
using OpenDentBusiness;

namespace OpenDental {
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormBackup : ODForm {
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button butRestore;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox4;
		private OpenDental.UI.Button butBackup;
		private OpenDental.UI.Button butBrowseTo;
		private OpenDental.UI.Button butBrowseFrom;
		private OpenDental.UI.Button butBrowseRestoreFrom;
		private System.Windows.Forms.TextBox textBox3;
		private OpenDental.UI.Button butBrowseRestoreTo;
		private System.Windows.Forms.TextBox textBackupToPath;
		private System.Windows.Forms.TextBox textBackupFromPath;
		private System.Windows.Forms.TextBox textBackupRestoreFromPath;
		private System.Windows.Forms.TextBox textBackupRestoreToPath;
		private System.Windows.Forms.TextBox textBox5;
		private System.Windows.Forms.TextBox textBackupRestoreAtoZToPath;
		private OpenDental.UI.Button butBrowseRestoreAtoZTo;
		private OpenDental.UI.Button butSave;
		//Required designer variable.
		private System.ComponentModel.Container components = null;
		private FormProgress FormP;
		///<summary>Only used by one worker thread at a time. The value of the progressbar (in MB). Still passed in by delegate.</summary>
		private double curVal;
		private GroupBox groupBox2;
		private CheckBox checkExcludeImages;
		private UI.ODPictureBox pictureCDS;
		private GroupBox groupManagedBackups;
		private TabControl tabControl1;
		private TabPage tabPageArchive;
		private TabPage tabPageBackup;
		private DateTimePicker dateTimeArchive;
		private Label label2;
		private TextBox textArchiveServerName;
		private TextBox textArchivePass;
		private TextBox textArchiveUser;
		private UI.Button butArchive;
		private GroupBox groupBox3;
		private Label label7;
		private UI.Button butSaveArchive;
		private Label labelWarning;
		private Label label3;
		private Label label8;
		private Label label5;

		//private bool usesInternalImages;
		///<summary>This message will only get filled when a backup attempt has failed.  It will hold the message text that we want to show to the user giving them more information about the failure.</summary>
		private string _errorMessage;

		///<summary></summary>
		public FormBackup()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		///<summary></summary>
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBackup));
			this.label1 = new System.Windows.Forms.Label();
			this.textBackupToPath = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.butBrowseRestoreAtoZTo = new OpenDental.UI.Button();
			this.textBackupRestoreAtoZToPath = new System.Windows.Forms.TextBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.butBrowseRestoreTo = new OpenDental.UI.Button();
			this.textBackupRestoreToPath = new System.Windows.Forms.TextBox();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.butBrowseRestoreFrom = new OpenDental.UI.Button();
			this.textBackupRestoreFromPath = new System.Windows.Forms.TextBox();
			this.butRestore = new OpenDental.UI.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBackupFromPath = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkExcludeImages = new System.Windows.Forms.CheckBox();
			this.butSave = new OpenDental.UI.Button();
			this.butBrowseFrom = new OpenDental.UI.Button();
			this.butBrowseTo = new OpenDental.UI.Button();
			this.butBackup = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.pictureCDS = new OpenDental.UI.ODPictureBox();
			this.groupManagedBackups = new System.Windows.Forms.GroupBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPageBackup = new System.Windows.Forms.TabPage();
			this.tabPageArchive = new System.Windows.Forms.TabPage();
			this.labelWarning = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.butSaveArchive = new OpenDental.UI.Button();
			this.textArchiveServerName = new System.Windows.Forms.TextBox();
			this.textArchivePass = new System.Windows.Forms.TextBox();
			this.textArchiveUser = new System.Windows.Forms.TextBox();
			this.butArchive = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.dateTimeArchive = new System.Windows.Forms.DateTimePicker();
			this.groupBox1.SuspendLayout();
			this.groupManagedBackups.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPageBackup.SuspendLayout();
			this.tabPageArchive.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(19, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(713, 28);
			this.label1.TabIndex = 2;
			this.label1.Text = "BACKUPS ARE USELESS UNLESS YOU REGULARLY VERIFY THEIR QUALITY BY TAKING A BACKUP " +
    "HOME AND RESTORING IT TO YOUR HOME COMPUTER.  We suggest an encrypted USB flash " +
    "drive for this purpose.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textBackupToPath
			// 
			this.textBackupToPath.Location = new System.Drawing.Point(19, 219);
			this.textBackupToPath.Name = "textBackupToPath";
			this.textBackupToPath.Size = new System.Drawing.Size(481, 20);
			this.textBackupToPath.TabIndex = 4;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textBox5);
			this.groupBox1.Controls.Add(this.butBrowseRestoreAtoZTo);
			this.groupBox1.Controls.Add(this.textBackupRestoreAtoZToPath);
			this.groupBox1.Controls.Add(this.textBox3);
			this.groupBox1.Controls.Add(this.butBrowseRestoreTo);
			this.groupBox1.Controls.Add(this.textBackupRestoreToPath);
			this.groupBox1.Controls.Add(this.textBox4);
			this.groupBox1.Controls.Add(this.butBrowseRestoreFrom);
			this.groupBox1.Controls.Add(this.textBackupRestoreFromPath);
			this.groupBox1.Controls.Add(this.butRestore);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(13, 262);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(747, 213);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Restore";
			// 
			// textBox5
			// 
			this.textBox5.BackColor = System.Drawing.SystemColors.Control;
			this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox5.Location = new System.Drawing.Point(7, 142);
			this.textBox5.Multiline = true;
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new System.Drawing.Size(396, 27);
			this.textBox5.TabIndex = 21;
			this.textBox5.Text = "Restore A-Z images to this folder: (example:)\r\nC:\\OpenDentImages\\";
			// 
			// butBrowseRestoreAtoZTo
			// 
			this.butBrowseRestoreAtoZTo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBrowseRestoreAtoZTo.Autosize = true;
			this.butBrowseRestoreAtoZTo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBrowseRestoreAtoZTo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBrowseRestoreAtoZTo.CornerRadius = 4F;
			this.butBrowseRestoreAtoZTo.Location = new System.Drawing.Point(500, 170);
			this.butBrowseRestoreAtoZTo.Name = "butBrowseRestoreAtoZTo";
			this.butBrowseRestoreAtoZTo.Size = new System.Drawing.Size(86, 26);
			this.butBrowseRestoreAtoZTo.TabIndex = 20;
			this.butBrowseRestoreAtoZTo.Text = "Browse";
			this.butBrowseRestoreAtoZTo.Click += new System.EventHandler(this.butBrowseRestoreAtoZTo_Click);
			// 
			// textBackupRestoreAtoZToPath
			// 
			this.textBackupRestoreAtoZToPath.Location = new System.Drawing.Point(6, 173);
			this.textBackupRestoreAtoZToPath.Name = "textBackupRestoreAtoZToPath";
			this.textBackupRestoreAtoZToPath.Size = new System.Drawing.Size(481, 20);
			this.textBackupRestoreAtoZToPath.TabIndex = 19;
			// 
			// textBox3
			// 
			this.textBox3.BackColor = System.Drawing.SystemColors.Control;
			this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox3.Location = new System.Drawing.Point(7, 81);
			this.textBox3.Multiline = true;
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(247, 27);
			this.textBox3.TabIndex = 18;
			this.textBox3.Text = "Restore database TO this folder: (example:)\r\nC:\\mysql\\data\\";
			// 
			// butBrowseRestoreTo
			// 
			this.butBrowseRestoreTo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBrowseRestoreTo.Autosize = true;
			this.butBrowseRestoreTo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBrowseRestoreTo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBrowseRestoreTo.CornerRadius = 4F;
			this.butBrowseRestoreTo.Location = new System.Drawing.Point(500, 109);
			this.butBrowseRestoreTo.Name = "butBrowseRestoreTo";
			this.butBrowseRestoreTo.Size = new System.Drawing.Size(86, 26);
			this.butBrowseRestoreTo.TabIndex = 17;
			this.butBrowseRestoreTo.Text = "Browse";
			this.butBrowseRestoreTo.Click += new System.EventHandler(this.butBrowseRestoreTo_Click);
			// 
			// textBackupRestoreToPath
			// 
			this.textBackupRestoreToPath.Location = new System.Drawing.Point(6, 112);
			this.textBackupRestoreToPath.Name = "textBackupRestoreToPath";
			this.textBackupRestoreToPath.Size = new System.Drawing.Size(481, 20);
			this.textBackupRestoreToPath.TabIndex = 16;
			// 
			// textBox4
			// 
			this.textBox4.BackColor = System.Drawing.SystemColors.Control;
			this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox4.Location = new System.Drawing.Point(7, 20);
			this.textBox4.Multiline = true;
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new System.Drawing.Size(280, 29);
			this.textBox4.TabIndex = 15;
			this.textBox4.Text = "Restore FROM this folder: (example:)\r\nD:\\";
			// 
			// butBrowseRestoreFrom
			// 
			this.butBrowseRestoreFrom.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBrowseRestoreFrom.Autosize = true;
			this.butBrowseRestoreFrom.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBrowseRestoreFrom.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBrowseRestoreFrom.CornerRadius = 4F;
			this.butBrowseRestoreFrom.Location = new System.Drawing.Point(500, 47);
			this.butBrowseRestoreFrom.Name = "butBrowseRestoreFrom";
			this.butBrowseRestoreFrom.Size = new System.Drawing.Size(86, 26);
			this.butBrowseRestoreFrom.TabIndex = 14;
			this.butBrowseRestoreFrom.Text = "Browse";
			this.butBrowseRestoreFrom.Click += new System.EventHandler(this.butBrowseRestoreFrom_Click);
			// 
			// textBackupRestoreFromPath
			// 
			this.textBackupRestoreFromPath.Location = new System.Drawing.Point(6, 50);
			this.textBackupRestoreFromPath.Name = "textBackupRestoreFromPath";
			this.textBackupRestoreFromPath.Size = new System.Drawing.Size(481, 20);
			this.textBackupRestoreFromPath.TabIndex = 13;
			// 
			// butRestore
			// 
			this.butRestore.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRestore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butRestore.Autosize = true;
			this.butRestore.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRestore.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRestore.CornerRadius = 4F;
			this.butRestore.Location = new System.Drawing.Point(648, 170);
			this.butRestore.Name = "butRestore";
			this.butRestore.Size = new System.Drawing.Size(86, 26);
			this.butRestore.TabIndex = 6;
			this.butRestore.Text = "Restore";
			this.butRestore.Click += new System.EventHandler(this.butRestore_Click);
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.SystemColors.Control;
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.Location = new System.Drawing.Point(20, 162);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(279, 55);
			this.textBox1.TabIndex = 9;
			this.textBox1.Text = "Backup TO this folder: (examples:)\r\nD:\\\r\nD:\\Backups\\\r\n\\\\frontdesk\\backups\\";
			// 
			// textBox2
			// 
			this.textBox2.BackColor = System.Drawing.SystemColors.Control;
			this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox2.Location = new System.Drawing.Point(20, 88);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(240, 43);
			this.textBox2.TabIndex = 12;
			this.textBox2.Text = "Backup database FROM this folder: (examples:)\r\nC:\\mysql\\data\\\r\n\\\\server\\mysql\\dat" +
    "a\\";
			// 
			// textBackupFromPath
			// 
			this.textBackupFromPath.Location = new System.Drawing.Point(19, 133);
			this.textBackupFromPath.Name = "textBackupFromPath";
			this.textBackupFromPath.Size = new System.Drawing.Size(481, 20);
			this.textBackupFromPath.TabIndex = 10;
			// 
			// groupBox2
			// 
			this.groupBox2.Location = new System.Drawing.Point(13, 72);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(747, 184);
			this.groupBox2.TabIndex = 14;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Backup";
			// 
			// checkExcludeImages
			// 
			this.checkExcludeImages.AutoSize = true;
			this.checkExcludeImages.Location = new System.Drawing.Point(13, 49);
			this.checkExcludeImages.Name = "checkExcludeImages";
			this.checkExcludeImages.Size = new System.Drawing.Size(221, 17);
			this.checkExcludeImages.TabIndex = 15;
			this.checkExcludeImages.Text = "Exclude image folder in backup or restore";
			this.checkExcludeImages.UseVisualStyleBackColor = true;
			this.checkExcludeImages.Click += new System.EventHandler(this.checkExcludeImages_Click);
			// 
			// butSave
			// 
			this.butSave.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSave.Autosize = true;
			this.butSave.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSave.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSave.CornerRadius = 4F;
			this.butSave.Location = new System.Drawing.Point(15, 491);
			this.butSave.Name = "butSave";
			this.butSave.Size = new System.Drawing.Size(86, 26);
			this.butSave.TabIndex = 13;
			this.butSave.Text = "Save Defaults";
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			// 
			// butBrowseFrom
			// 
			this.butBrowseFrom.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBrowseFrom.Autosize = true;
			this.butBrowseFrom.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBrowseFrom.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBrowseFrom.CornerRadius = 4F;
			this.butBrowseFrom.Location = new System.Drawing.Point(513, 130);
			this.butBrowseFrom.Name = "butBrowseFrom";
			this.butBrowseFrom.Size = new System.Drawing.Size(86, 26);
			this.butBrowseFrom.TabIndex = 11;
			this.butBrowseFrom.Text = "Browse";
			this.butBrowseFrom.Click += new System.EventHandler(this.butBrowseFrom_Click);
			// 
			// butBrowseTo
			// 
			this.butBrowseTo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBrowseTo.Autosize = true;
			this.butBrowseTo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBrowseTo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBrowseTo.CornerRadius = 4F;
			this.butBrowseTo.Location = new System.Drawing.Point(513, 216);
			this.butBrowseTo.Name = "butBrowseTo";
			this.butBrowseTo.Size = new System.Drawing.Size(86, 26);
			this.butBrowseTo.TabIndex = 5;
			this.butBrowseTo.Text = "Browse";
			this.butBrowseTo.Click += new System.EventHandler(this.butBrowseTo_Click);
			// 
			// butBackup
			// 
			this.butBackup.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBackup.Autosize = true;
			this.butBackup.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBackup.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBackup.CornerRadius = 4F;
			this.butBackup.Location = new System.Drawing.Point(666, 216);
			this.butBackup.Name = "butBackup";
			this.butBackup.Size = new System.Drawing.Size(86, 26);
			this.butBackup.TabIndex = 1;
			this.butBackup.Text = "Backup";
			this.butBackup.Click += new System.EventHandler(this.butBackup_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(663, 491);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(86, 26);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// pictureCDS
			// 
			this.pictureCDS.HasBorder = false;
			this.pictureCDS.Image = global::OpenDental.Properties.Resources.CDS_Button_green;
			this.pictureCDS.Location = new System.Drawing.Point(10, 22);
			this.pictureCDS.Name = "pictureCDS";
			this.pictureCDS.Size = new System.Drawing.Size(83, 24);
			this.pictureCDS.TabIndex = 16;
			this.pictureCDS.TextNullImage = null;
			this.pictureCDS.Click += new System.EventHandler(this.pictureCDS_Click);
			// 
			// groupManagedBackups
			// 
			this.groupManagedBackups.Controls.Add(this.pictureCDS);
			this.groupManagedBackups.Location = new System.Drawing.Point(319, 488);
			this.groupManagedBackups.Name = "groupManagedBackups";
			this.groupManagedBackups.Size = new System.Drawing.Size(114, 57);
			this.groupManagedBackups.TabIndex = 17;
			this.groupManagedBackups.TabStop = false;
			this.groupManagedBackups.Text = "Managed Backups";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPageBackup);
			this.tabControl1.Controls.Add(this.tabPageArchive);
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(780, 579);
			this.tabControl1.TabIndex = 1;
			// 
			// tabPageBackup
			// 
			this.tabPageBackup.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageBackup.Controls.Add(this.groupManagedBackups);
			this.tabPageBackup.Controls.Add(this.label1);
			this.tabPageBackup.Controls.Add(this.checkExcludeImages);
			this.tabPageBackup.Controls.Add(this.butSave);
			this.tabPageBackup.Controls.Add(this.groupBox1);
			this.tabPageBackup.Controls.Add(this.textBox2);
			this.tabPageBackup.Controls.Add(this.butCancel);
			this.tabPageBackup.Controls.Add(this.butBrowseFrom);
			this.tabPageBackup.Controls.Add(this.butBackup);
			this.tabPageBackup.Controls.Add(this.textBackupFromPath);
			this.tabPageBackup.Controls.Add(this.textBackupToPath);
			this.tabPageBackup.Controls.Add(this.textBox1);
			this.tabPageBackup.Controls.Add(this.butBrowseTo);
			this.tabPageBackup.Controls.Add(this.groupBox2);
			this.tabPageBackup.Location = new System.Drawing.Point(4, 22);
			this.tabPageBackup.Name = "tabPageBackup";
			this.tabPageBackup.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageBackup.Size = new System.Drawing.Size(772, 553);
			this.tabPageBackup.TabIndex = 0;
			this.tabPageBackup.Text = "Backup";
			// 
			// tabPageArchive
			// 
			this.tabPageArchive.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageArchive.Controls.Add(this.labelWarning);
			this.tabPageArchive.Controls.Add(this.label7);
			this.tabPageArchive.Controls.Add(this.groupBox3);
			this.tabPageArchive.Controls.Add(this.butArchive);
			this.tabPageArchive.Controls.Add(this.label2);
			this.tabPageArchive.Controls.Add(this.dateTimeArchive);
			this.tabPageArchive.Location = new System.Drawing.Point(4, 22);
			this.tabPageArchive.Name = "tabPageArchive";
			this.tabPageArchive.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageArchive.Size = new System.Drawing.Size(772, 553);
			this.tabPageArchive.TabIndex = 1;
			this.tabPageArchive.Text = "Archive";
			// 
			// labelWarning
			// 
			this.labelWarning.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.labelWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelWarning.ForeColor = System.Drawing.Color.Red;
			this.labelWarning.Location = new System.Drawing.Point(244, 389);
			this.labelWarning.Name = "labelWarning";
			this.labelWarning.Size = new System.Drawing.Size(509, 55);
			this.labelWarning.TabIndex = 13;
			this.labelWarning.Text = "Not available when using random primary keys.\r\nPlease call support.";
			this.labelWarning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelWarning.Visible = false;
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.Location = new System.Drawing.Point(19, 12);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(734, 58);
			this.label7.TabIndex = 12;
			this.label7.Text = resources.GetString("label7.Text");
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.Controls.Add(this.butSaveArchive);
			this.groupBox3.Controls.Add(this.textArchiveServerName);
			this.groupBox3.Controls.Add(this.textArchivePass);
			this.groupBox3.Controls.Add(this.textArchiveUser);
			this.groupBox3.Location = new System.Drawing.Point(140, 140);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(493, 223);
			this.groupBox3.TabIndex = 0;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Archive Connection Settings";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(7, 122);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(480, 18);
			this.label5.TabIndex = 37;
			this.label5.Text = "Password: For new installations, the password will be blank.";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(7, 78);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(480, 18);
			this.label8.TabIndex = 36;
			this.label8.Text = "User: When MySQL is first installed, the user is root.";
			this.label8.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(481, 36);
			this.label3.TabIndex = 35;
			this.label3.Text = "Server Name: The name of the computer where the archive server and database are l" +
    "ocated.\r\nIf running on a single computer only, Server Name may be localhost.";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butSaveArchive
			// 
			this.butSaveArchive.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSaveArchive.Autosize = true;
			this.butSaveArchive.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSaveArchive.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSaveArchive.CornerRadius = 4F;
			this.butSaveArchive.Location = new System.Drawing.Point(12, 180);
			this.butSaveArchive.Name = "butSaveArchive";
			this.butSaveArchive.Size = new System.Drawing.Size(86, 26);
			this.butSaveArchive.TabIndex = 4;
			this.butSaveArchive.Text = "Save Defaults";
			this.butSaveArchive.UseVisualStyleBackColor = true;
			this.butSaveArchive.Click += new System.EventHandler(this.butSaveArchive_Click);
			// 
			// textArchiveServerName
			// 
			this.textArchiveServerName.Location = new System.Drawing.Point(9, 55);
			this.textArchiveServerName.Name = "textArchiveServerName";
			this.textArchiveServerName.Size = new System.Drawing.Size(283, 20);
			this.textArchiveServerName.TabIndex = 0;
			// 
			// textArchivePass
			// 
			this.textArchivePass.Location = new System.Drawing.Point(10, 143);
			this.textArchivePass.Name = "textArchivePass";
			this.textArchivePass.Size = new System.Drawing.Size(283, 20);
			this.textArchivePass.TabIndex = 3;
			this.textArchivePass.TextChanged += new System.EventHandler(this.textArchivePass_TextChanged);
			this.textArchivePass.Leave += new System.EventHandler(this.textArchivePass_Leave);
			// 
			// textArchiveUser
			// 
			this.textArchiveUser.Location = new System.Drawing.Point(10, 99);
			this.textArchiveUser.Name = "textArchiveUser";
			this.textArchiveUser.Size = new System.Drawing.Size(283, 20);
			this.textArchiveUser.TabIndex = 2;
			// 
			// butArchive
			// 
			this.butArchive.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butArchive.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.butArchive.Autosize = true;
			this.butArchive.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butArchive.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butArchive.CornerRadius = 4F;
			this.butArchive.Location = new System.Drawing.Point(152, 404);
			this.butArchive.Name = "butArchive";
			this.butArchive.Size = new System.Drawing.Size(86, 26);
			this.butArchive.TabIndex = 2;
			this.butArchive.Text = "Archive";
			this.butArchive.UseVisualStyleBackColor = true;
			this.butArchive.Click += new System.EventHandler(this.butArchive_Click);
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label2.Location = new System.Drawing.Point(149, 76);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(272, 17);
			this.label2.TabIndex = 1;
			this.label2.Text = "Archive entries on or before:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// dateTimeArchive
			// 
			this.dateTimeArchive.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.dateTimeArchive.Location = new System.Drawing.Point(150, 96);
			this.dateTimeArchive.Name = "dateTimeArchive";
			this.dateTimeArchive.Size = new System.Drawing.Size(237, 20);
			this.dateTimeArchive.TabIndex = 1;
			// 
			// FormBackup
			// 
			this.ClientSize = new System.Drawing.Size(777, 582);
			this.Controls.Add(this.tabControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(793, 621);
			this.Name = "FormBackup";
			this.ShowInTaskbar = false;
			this.Text = "Backup & Archive";
			this.Load += new System.EventHandler(this.FormBackup_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupManagedBackups.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPageBackup.ResumeLayout(false);
			this.tabPageBackup.PerformLayout();
			this.tabPageArchive.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormBackup_Load(object sender, System.EventArgs e) {
			#region Backup Tab
			//already include the \
			checkExcludeImages.Checked=PrefC.GetBool(PrefName.BackupExcludeImageFolder);
			textBackupFromPath.Text=PrefC.GetString(PrefName.BackupFromPath);
			textBackupToPath.Text=PrefC.GetString(PrefName.BackupToPath);
			textBackupRestoreFromPath.Text=PrefC.GetString(PrefName.BackupRestoreFromPath);
			textBackupRestoreToPath.Text=PrefC.GetString(PrefName.BackupRestoreToPath);
			textBackupRestoreAtoZToPath.Text=PrefC.GetString(PrefName.BackupRestoreAtoZToPath);
			//usesInternalImages=(PrefC.GetString(PrefName.ImageStore)=="OpenDental.Imaging.SqlStore");
			textBackupRestoreAtoZToPath.Enabled=ShouldUseAtoZFolder();
			butBrowseRestoreAtoZTo.Enabled=ShouldUseAtoZFolder();
			if(ProgramProperties.IsAdvertisingDisabled(ProgramName.CentralDataStorage)) {
				groupManagedBackups.Visible=false;
			}
			#endregion
			#region Archive Tab
			string decryptedPass;
			CDT.Class1.Decrypt(PrefC.GetString(PrefName.ArchivePassHash),out decryptedPass);
			textArchivePass.Text=decryptedPass;
			textArchivePass.PasswordChar=(textArchivePass.Text=="" ? default(char) : '*');
			textArchiveServerName.Text=PrefC.GetString(PrefName.ArchiveServerName);
			textArchiveUser.Text=PrefC.GetString(PrefName.ArchiveUserName);
			//If pref is set, use it.  Otherwise, 3 years ago.
			dateTimeArchive.Value=PrefC.GetDate(PrefName.ArchiveDate)==DateTime.MinValue?DateTime.Today.AddYears(-3):PrefC.GetDate(PrefName.ArchiveDate);
			if(PrefC.GetBool(PrefName.RandomPrimaryKeys) || ReplicationServers.Server_id!=0) {//show warning if replication is enabled
				butArchive.Enabled=false;
				labelWarning.Visible=true;
			}
			#endregion
		}

		#region Backup Tab

		private bool ShouldUseAtoZFolder() {
			return (PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ //&& !usesInternalImages 
				&& !checkExcludeImages.Checked);
		}

		private void butBrowseFrom_Click(object sender, System.EventArgs e) {
			FolderBrowserDialog browserDlg=new FolderBrowserDialog();
			browserDlg.SelectedPath=textBackupFromPath.Text;
			if(browserDlg.ShowDialog()==DialogResult.Cancel){
				return;
			}
			textBackupFromPath.Text=ODFileUtils.CombinePaths(browserDlg.SelectedPath,"");//Add trail slash.
		}

		private void butBrowseTo_Click(object sender, System.EventArgs e) {
			FolderBrowserDialog browserDlg=new FolderBrowserDialog();
			browserDlg.SelectedPath=textBackupToPath.Text;
			if(browserDlg.ShowDialog()==DialogResult.Cancel){
				return;
			}
			textBackupToPath.Text=ODFileUtils.CombinePaths(browserDlg.SelectedPath,"");//Add trail slash.
		}

		private void butBrowseRestoreFrom_Click(object sender, System.EventArgs e) {
			FolderBrowserDialog browserDlg=new FolderBrowserDialog();
			browserDlg.SelectedPath=textBackupRestoreFromPath.Text;
			if(browserDlg.ShowDialog()==DialogResult.Cancel){
				return;
			}
			textBackupRestoreFromPath.Text=ODFileUtils.CombinePaths(browserDlg.SelectedPath,"");//Add trail slash.
		}

		private void butBrowseRestoreTo_Click(object sender, System.EventArgs e) {
			FolderBrowserDialog browserDlg=new FolderBrowserDialog();
			browserDlg.SelectedPath=textBackupRestoreToPath.Text;
			if(browserDlg.ShowDialog()==DialogResult.Cancel){
				return;
			}
			textBackupRestoreToPath.Text=ODFileUtils.CombinePaths(browserDlg.SelectedPath,"");//Add trail slash.
		}

		private void butBrowseRestoreAtoZTo_Click(object sender, System.EventArgs e) {
			FolderBrowserDialog browserDlg=new FolderBrowserDialog();
			browserDlg.SelectedPath=textBackupRestoreAtoZToPath.Text;
			if(browserDlg.ShowDialog()==DialogResult.Cancel){
				return;
			}
			textBackupRestoreAtoZToPath.Text=ODFileUtils.CombinePaths(browserDlg.SelectedPath,"");//Add trail slash.
		}

		private void butBackup_Click(object sender, System.EventArgs e) {
			//test for trailing slashes
			if(textBackupFromPath.Text!="" && !textBackupFromPath.Text.EndsWith(""+Path.DirectorySeparatorChar)){
				MsgBox.Show(this,"Paths must end with "+Path.DirectorySeparatorChar+".");
				return;
			}
			if(textBackupToPath.Text!="" && !textBackupToPath.Text.EndsWith(""+Path.DirectorySeparatorChar)){
				MsgBox.Show(this,"Paths must end with "+Path.DirectorySeparatorChar+".");
				return;
			}
			if(textBackupRestoreFromPath.Text!="" && !textBackupRestoreFromPath.Text.EndsWith(""+Path.DirectorySeparatorChar)) {
				MsgBox.Show(this,"Paths must end with "+Path.DirectorySeparatorChar+".");
				return;
			}
			if(textBackupRestoreToPath.Text!="" && !textBackupRestoreToPath.Text.EndsWith(""+Path.DirectorySeparatorChar)) {
				MsgBox.Show(this,"Paths must end with "+Path.DirectorySeparatorChar+".");
				return;
			}
			if(textBackupRestoreAtoZToPath.Text!="" && !textBackupRestoreAtoZToPath.Text.EndsWith(""+Path.DirectorySeparatorChar)) {
				MsgBox.Show(this,"Paths must end with "+Path.DirectorySeparatorChar+".");
				return;
			}
			//Ensure that the backup from and backup to paths are different. This is to prevent the live database
			//from becoming corrupt.
			if(this.textBackupFromPath.Text.Trim().ToLower()==this.textBackupToPath.Text.Trim().ToLower()) {
				MsgBox.Show(this,"The backup from path and backup to path must be different.");
				return;
			}
			//test saving defaults
			if(textBackupFromPath.Text!=PrefC.GetString(PrefName.BackupFromPath)
				|| textBackupToPath.Text!=PrefC.GetString(PrefName.BackupToPath)
				|| textBackupRestoreFromPath.Text!=PrefC.GetString(PrefName.BackupRestoreFromPath)
				|| textBackupRestoreToPath.Text!=PrefC.GetString(PrefName.BackupRestoreToPath)
				|| textBackupRestoreAtoZToPath.Text!=PrefC.GetString(PrefName.BackupRestoreAtoZToPath))
			{
				if(MsgBox.Show(this,true,"Set as default?")){
					Prefs.UpdateString(PrefName.BackupFromPath,textBackupFromPath.Text);
					Prefs.UpdateString(PrefName.BackupToPath,textBackupToPath.Text);
					Prefs.UpdateString(PrefName.BackupRestoreFromPath,textBackupRestoreFromPath.Text);
					Prefs.UpdateString(PrefName.BackupRestoreToPath,textBackupRestoreToPath.Text);
					Prefs.UpdateString(PrefName.BackupRestoreAtoZToPath,textBackupRestoreAtoZToPath.Text);
					DataValid.SetInvalid(InvalidType.Prefs);
				}
			}
			string dbName=MiscData.GetCurrentDatabase();
			if(!Directory.Exists(ODFileUtils.CombinePaths(textBackupFromPath.Text,dbName))){// C:\mysql\data\opendental
				MsgBox.Show(this,"Backup FROM path is invalid.");
				return;
			}
			if(!Directory.Exists(textBackupToPath.Text)){// D:\
				MsgBox.Show(this,"Backup TO path is invalid.");
				return;
			}
			_errorMessage="";
			FormP=new FormProgress();
			FormP.MaxVal=100;//We will be setting maxVal from worker thread.  (double)fileSize/1024;
			FormP.NumberMultiplication=100;
			FormP.DisplayText="";//We will set the text from the worker thread.
			FormP.NumberFormat="N1";
			//start the thread that will perform the database copy
			Thread workerThread=new Thread(new ThreadStart(InstanceMethodBackup));
			workerThread.Start();
			//display the progress dialog to the user:
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.Cancel){
				workerThread.Abort();
				return;
			}
			if(_errorMessage=="") {
				SecurityLogs.MakeLogEntry(Permissions.Backup,0,Lan.g(this,"Database backup created at ")+textBackupToPath.Text);
				MessageBox.Show(Lan.g(this,"Backup complete."));
			}
			else {//Backup failed for some reason.
				MessageBox.Show(_errorMessage);
			}
			Close();
		}

		///<summary>This is the function that the worker thread uses to perform the backup.</summary>
		private void InstanceMethodBackup(){
			curVal=0;
			Invoke(new PassProgressDelegate(PassProgressToDialog),new object [] { curVal,
				Lan.g(this,"Preparing to copy database"),//this happens very fast and probably won't be noticed.
				100,"" });//max of 100 keeps dlg from closing
			string dbName=MiscData.GetCurrentDatabase();
			ulong driveFreeSpace=0;
			double dbSize=GetFileSizes(textBackupFromPath.Text+dbName)/1024;
			//Attempt to get the free disk space on the drive or share of the destination folder.
			//If the free space cannot be determined the backup will be attempted anyway (old behavior).
			if(ODFileUtils.GetDiskFreeSpace(textBackupToPath.Text,out driveFreeSpace)) {
				if((ulong)dbSize*1024*1024>=driveFreeSpace) {//dbSize is in megabytes, cast to ulong to compare. It will never be negative so this is safe.
					Invoke(new ErrorMessageDelegate(SetErrorMessage),new object[] { Lan.g(this,"Not enough free disk space available on the destination drive to backup the database.") });
					//We now want to automatically close FormProgress.  This is done by clearing out the variables.
					Invoke(new PassProgressDelegate(PassProgressToDialog),new object[] { 0,"",0,"" });
					return;
				}
			}
			try{
				string dbtopath=ODFileUtils.CombinePaths(textBackupToPath.Text,dbName);
				if(Directory.Exists(dbtopath)){// D:\opendental
					int loopCount=1;
					while(Directory.Exists(dbtopath+"backup_"+loopCount)){
						loopCount++;
					}
				  Directory.Move(dbtopath,dbtopath+"backup_"+loopCount);
				}
				string fromPath=ODFileUtils.CombinePaths(textBackupFromPath.Text,dbName);
				string toPath=textBackupToPath.Text;
				DirectoryInfo dirInfo=new DirectoryInfo(fromPath);//does not check to see if dir exists
				Directory.CreateDirectory(ODFileUtils.CombinePaths(toPath,dirInfo.Name));
				FileInfo[] files=dirInfo.GetFiles();
				curVal=0;//curVal gets increased
				for(int i=0;i<files.Length;i++){
					string fromFile=files[i].FullName;
					string toFile=ODFileUtils.CombinePaths(new string[] { toPath,dirInfo.Name,files[i].Name });
					if(File.Exists(toFile)) {
						if(files[i].LastWriteTime!=File.GetLastWriteTime(toFile)) {//if modification dates don't match
							FileAttributes fa=File.GetAttributes(toFile);
							bool isReadOnly=((fa&FileAttributes.ReadOnly)==FileAttributes.ReadOnly);
							if(isReadOnly) {
								//If the destination file exists and is marked as read only, then we must mark it as a
								//normal read/write file before it may be overwritten.
								File.SetAttributes(toFile,FileAttributes.Normal);//Remove read only from the destination file.
							}
							File.Copy(fromFile,toFile,true);
						}
					} else {//file doesn't exist, so just copy
						File.Copy(fromFile,toFile);
					}
					curVal+=(double)files[i].Length/(double)1024/(double)1024;
					if(curVal<dbSize){//this avoids setting progress bar to max, which would close the dialog.
						Invoke(new PassProgressDelegate(PassProgressToDialog),new object [] { curVal,
							Lan.g(this,"Database: ?currentVal MB of ?maxVal MB copied"),
							dbSize,""});
					}
				}
			}
			catch{//for instance, if abort.
				//If the user aborted, FormP will return DialogResult.Cancel which will not cause this error text to be displayed to the user.  See butBackup_Click for more info.
				Invoke(new ErrorMessageDelegate(SetErrorMessage),new object[] { Lan.g(this,"Backup failed.") });
				//We now want to automatically close FormProgress.  This is done by clearing out the variables.
				Invoke(new PassProgressDelegate(PassProgressToDialog),new object[] { 0,"",0,"" });
				return;
			}
			//A to Z folder------------------------------------------------------------------------------------
			try {
				if(ShouldUseAtoZFolder()) {
					string atozFull=ODFileUtils.RemoveTrailingSeparators(ImageStore.GetPreferredAtoZpath());
					string atozDir=atozFull.Substring(atozFull.LastIndexOf(Path.DirectorySeparatorChar)+1);//OpenDentalData
					Invoke(new PassProgressDelegate(PassProgressToDialog),new object[] { 0,
					Lan.g(this,"Calculating size of files in A to Z folder."),
					100,"" });//max of 100 keeps dlg from closing
					long atozSize=GetFileSizes(ODFileUtils.CombinePaths(atozFull,""),
						ODFileUtils.CombinePaths(new string[] { textBackupToPath.Text,atozDir,"" }))/1024; 
					driveFreeSpace=0;
					//Attempt to get the free disk space on the drive or share of the destination folder.
					//If the free space cannot be determined the backup will be attempted anyway (old behavior).
					if(ODFileUtils.GetDiskFreeSpace(textBackupToPath.Text,out driveFreeSpace)) {
						if((ulong)(atozSize*1024*1024)>=driveFreeSpace) {//atozSize is in megabytes, cast to ulong in order to compare.  It will never be negative so it's safe.
							//Not enough free space to perform the backup.
							throw new ApplicationException(Lan.g(this,"Backing up A to Z images folder failed.  Not enough free disk space available on the destination drive.")
								+"\r\n"+Lan.g(this,"AtoZ folder size:")+" "+atozSize*1024*1024+"B\r\n"
								+Lan.g(this,"Destination available space:")+" "+driveFreeSpace+"B");
						}
					}
					if(!Directory.Exists(ODFileUtils.CombinePaths(textBackupToPath.Text,atozDir))) {// D:\OpenDentalData
						Directory.CreateDirectory(ODFileUtils.CombinePaths(textBackupToPath.Text,atozDir));// D:\OpenDentalData
					}
					curVal=0;
					CopyDirectoryIncremental(ODFileUtils.CombinePaths(atozFull,""),// C:\OpenDentalData\
						ODFileUtils.CombinePaths(new string[] { textBackupToPath.Text,atozDir,"" }),// D:\OpenDentalData\
						atozSize);
				}
			}
			catch(ApplicationException ex) {
				Invoke(new ErrorMessageDelegate(SetErrorMessage),new object[] { ex.Message }); 
			}
			catch {
				Invoke(new ErrorMessageDelegate(SetErrorMessage),new object[] { Lan.g(this,"Backing up A to Z images folder failed.  User might not have enough permissions or a file might be in use.") });
			}
			//force dialog to close even if no files copied or calculation was slightly off.
			Invoke(new PassProgressDelegate(PassProgressToDialog),new object[] { 0,"",0,"" });
		}

		///<summary>This is the function that the worker thread uses to restore the A-Z folder.</summary>
		private void InstanceMethodRestore(){
			curVal=0;
			string atozFull=textBackupRestoreAtoZToPath.Text;// C:\OpenDentalData\
			//remove the trailing \
			atozFull=atozFull.Substring(0,atozFull.Length-1);// C:\OpenDentalData
			string atozDir=atozFull.Substring(atozFull.LastIndexOf(Path.DirectorySeparatorChar)+1);// OpenDentalData
			Invoke(new PassProgressDelegate(PassProgressToDialog),new object [] { 0,
				Lan.g(this,"Database restored.\r\nCalculating size of files in A to Z folder."),
				100,"" });//max of 100 keeps dlg from closing
			long atozSize=GetFileSizes(ODFileUtils.CombinePaths(new string[] {textBackupRestoreFromPath.Text,atozDir,""}),
				ODFileUtils.CombinePaths(atozFull,""))/1024;// C:\OpenDentalData\
			if(!Directory.Exists(atozFull)){// C:\OpenDentalData\
				Directory.CreateDirectory(atozFull);// C:\OpenDentalData\
			}
			curVal=0;
			CopyDirectoryIncremental(ODFileUtils.CombinePaths(new string[] {textBackupRestoreFromPath.Text,atozDir,""}),
				ODFileUtils.CombinePaths(atozFull,""),// C:\OpenDentalData\
				atozSize);
			//force dlg to close even if no files copied or calculation was slightly off.
			Invoke(new PassProgressDelegate(PassProgressToDialog),new object[] { 0,"",0,"" });
		}

		///<summary>This function gets invoked from the worker threads.</summary>
		private void SetErrorMessage(string errorMessage) {
			_errorMessage=errorMessage;
		}

		///<summary>This function gets invoked from the worker threads.</summary>
		private void PassProgressToDialog(double currentVal,string displayText,double maxVal,string errorMessage){
			FormP.CurrentVal=currentVal;
			FormP.DisplayText=displayText;
			FormP.MaxVal=maxVal;
			FormP.ErrorMessage=errorMessage;
		}

		///<summary>Counts the total KB of all files that will need to be copied from one directory to another.  Recursive.  Only includes missing files, not changed files.  Used to display the progress bar.  Supplied paths must end in \. toPath might not exist.</summary>
		private long GetFileSizes(string fromPath,string toPath){
			long retVal=0;
			DirectoryInfo dirInfo=new DirectoryInfo(fromPath);
			DirectoryInfo[] dirs=dirInfo.GetDirectories();
			for(int i=0;i<dirs.Length;i++){
				retVal+=GetFileSizes(ODFileUtils.CombinePaths(dirs[i].FullName,""),
					ODFileUtils.CombinePaths(new string[] {toPath,dirs[i].Name,""}));
			}
			FileInfo[] files=dirInfo.GetFiles();//of fromPath
			for(int i=0;i<files.Length;i++){
				if(!File.Exists(ODFileUtils.CombinePaths(toPath,files[i].Name))){
					retVal+=(long)(files[i].Length/1024);
				}
			}
			return retVal;
		}

		///<summary>Counts the total KB of all files in the given directory.  Not recursive since it's just used for db files.  Used to display the progress bar.</summary>
		private long GetFileSizes(string fromPath) {
			long retVal=0;
			DirectoryInfo dirInfo=new DirectoryInfo(fromPath);
			FileInfo[] files=dirInfo.GetFiles();
			for(int i=0;i<files.Length;i++){
				retVal+=(long)(files[i].Length/1024);
			}
			return retVal;
		}

		///<summary>A recursive fuction that copies any new or changed files or folders from one directory to another.  An exception will be thrown if either directory does not already exist.  fromPath is the fully qualified path of the directory to copy.  toPath is the fully qualified path of the destination directory.  Both paths must include a trailing \.  The max size should be calculated ahead of time.  It's passed in for use in progress bar.</summary>
		private void CopyDirectoryIncremental(string fromPath,string toPath,double maxSize){
			if(!Directory.Exists(fromPath)){
				throw new Exception(fromPath+" does not exist.");
			}
			if(!Directory.Exists(toPath)){
				throw new Exception(toPath+" does not exist.");
			}
			DirectoryInfo dirInfo=new DirectoryInfo(fromPath);
			DirectoryInfo[] dirs=dirInfo.GetDirectories();
			for(int i=0;i<dirs.Length;i++){
				string destPath=ODFileUtils.CombinePaths(toPath,dirs[i].Name);
				if(!Directory.Exists(destPath)){
					Directory.CreateDirectory(destPath);
				}
				CopyDirectoryIncremental(ODFileUtils.CombinePaths(dirs[i].FullName,""),
					ODFileUtils.CombinePaths(destPath,""),maxSize);
			}
			FileInfo[] files=dirInfo.GetFiles();//of fromPath
			for(int i=0;i<files.Length;i++){
				string fromFile=files[i].FullName;
				string toFile=ODFileUtils.CombinePaths(toPath,files[i].Name);
				if(File.Exists(toFile)){
					if(files[i].LastWriteTime!=File.GetLastWriteTime(toFile)){//if modification dates don't match
						FileAttributes fa=File.GetAttributes(toFile);
						bool isReadOnly=((fa&FileAttributes.ReadOnly)==FileAttributes.ReadOnly);
						if(isReadOnly){
							//If the destination file exists and is marked as read only, then we must mark it as a
							//normal read/write file before it may be overwritten.
							File.SetAttributes(toFile,FileAttributes.Normal);//Remove read only from the destination file.
						}
						File.Copy(fromFile,toFile,true);
					}
				}
				else{//file doesn't exist, so just copy
					File.Copy(fromFile,toFile);
				}
				curVal+=(double)files[i].Length/1048576.0; //Number of megabytes.
				if(curVal<maxSize) {//this avoids setting progress bar to max, which would close the dialog.
					Invoke(new PassProgressDelegate(PassProgressToDialog),new object[] { curVal,
							Lan.g(this,"A to Z folder: ?currentVal MB of ?maxVal MB copied"),
							maxSize,""});
				}
			}
		}

		private void butRestore_Click(object sender, System.EventArgs e) {			
			if(textBackupRestoreFromPath.Text!="" && !textBackupRestoreFromPath.Text.EndsWith(""+Path.DirectorySeparatorChar)){
				MessageBox.Show(Lan.g(this,"Paths must end with ")+Path.DirectorySeparatorChar+".");
				return;
			}
			if(textBackupRestoreToPath.Text!="" && !textBackupRestoreToPath.Text.EndsWith(""+Path.DirectorySeparatorChar)){
				MessageBox.Show(Lan.g(this,"Paths must end with ")+Path.DirectorySeparatorChar+".");
				return;
			}
			if(ShouldUseAtoZFolder()) {
				if(textBackupRestoreAtoZToPath.Text!="" && !textBackupRestoreAtoZToPath.Text.EndsWith(""+Path.DirectorySeparatorChar)){
					MessageBox.Show(Lan.g(this,"Paths must end with ")+Path.DirectorySeparatorChar+".");
					return;
				}
			}
			if(Environment.OSVersion.Platform!=PlatformID.Unix){
				//dmg This check will not work on Linux, because mapped drives exist as regular (mounted) paths. Perhaps there
				//is another way to check for this on Linux.
				if(textBackupRestoreToPath.Text!="" && textBackupRestoreToPath.Text.StartsWith(""+Path.DirectorySeparatorChar)){
					MsgBox.Show(this,"The restore database TO folder must be on this computer.");
					return;
				}
			}
			//pointless to save defaults
			string dbName=MiscData.GetCurrentDatabase();
			if(!Directory.Exists(ODFileUtils.CombinePaths(textBackupRestoreFromPath.Text,dbName))){// D:\opendental
				MessageBox.Show(Lan.g(this,"Restore FROM path is invalid.  Unable to find folder named ")+dbName);
				return;
			}
			if(!Directory.Exists(ODFileUtils.CombinePaths(textBackupRestoreToPath.Text,dbName))) {// C:\mysql\data\opendental
				MessageBox.Show(Lan.g(this,"Restore TO path is invalid.  Unable to find folder named ")+dbName);
				return;
			}
			if(ShouldUseAtoZFolder()) {
				if(!Directory.Exists(textBackupRestoreAtoZToPath.Text)) {// C:\OpenDentalData\
					MsgBox.Show(this,"Restore A-Z images TO path is invalid.");
					return;
				}
				string atozFull=textBackupRestoreAtoZToPath.Text;// C:\OpenDentalData\
				//remove the trailing \
				atozFull=atozFull.Substring(0,atozFull.Length-1);// C:\OpenDentalData
				string atozDir=atozFull.Substring(atozFull.LastIndexOf(Path.DirectorySeparatorChar)+1);// OpenDentalData
				if(!Directory.Exists(ODFileUtils.CombinePaths(textBackupRestoreFromPath.Text,atozDir))){// D:\OpenDentalData
					MsgBox.Show(this,"Restore A-Z images FROM path is invalid.");
					return;
				}
			}
			string fromPath=ODFileUtils.CombinePaths(new string[] {textBackupRestoreFromPath.Text,dbName,""});// D:\opendental\
			DirectoryInfo dirInfo=new DirectoryInfo(fromPath);//does not check to see if dir exists
			if(MessageBox.Show(Lan.g(this,"Restore from backup created on")+"\r\n"
				+dirInfo.LastWriteTime.ToString("dddd")+"  "+dirInfo.LastWriteTime.ToString()
				,"",MessageBoxButtons.OKCancel,MessageBoxIcon.Question)==DialogResult.Cancel) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			//stop the service--------------------------------------------------------------------------------------
			ServiceController sc=new ServiceController("MySQL");
			if(!ServicesHelper.Stop(sc)) {
				MsgBox.Show(this,"Unable to stop MySQL service.");
				Cursor=Cursors.Default;
				return;
			}
			//rename the current database---------------------------------------------------------------------------
			//Get a name for the new directory
			string newDb=dbName+"backup_"+DateTime.Today.ToString("MM_dd_yyyy");
			if(Directory.Exists(ODFileUtils.CombinePaths(textBackupRestoreToPath.Text,newDb))){//if the new database name already exists
				//find a unique one
				int uniqueID=1;
				string originalNewDb=newDb;
				do{
					newDb=originalNewDb+"_"+uniqueID.ToString();
					uniqueID++;
				}
				while(Directory.Exists(ODFileUtils.CombinePaths(textBackupRestoreToPath.Text,newDb)));
			}
			//move the current db (rename)
			Directory.Move(ODFileUtils.CombinePaths(textBackupRestoreToPath.Text,dbName)
				,ODFileUtils.CombinePaths(textBackupRestoreToPath.Text,newDb));
			//Restore----------------------------------------------------------------------------------------------
			string toPath=textBackupRestoreToPath.Text;// C:\mysql\data\
			Directory.CreateDirectory(ODFileUtils.CombinePaths(toPath,dirInfo.Name));
			FileInfo[] files=dirInfo.GetFiles();
			curVal=0;//curVal gets increased
			for(int i=0;i<files.Length;i++){
				File.Copy(files[i].FullName,ODFileUtils.CombinePaths(new string[] {toPath,dirInfo.Name,files[i].Name}));
			}
			//start the service--------------------------------------------------------------------------------------
			ServicesHelper.Start(sc);
			Cursor=Cursors.Default;
			//restore A-Z folder, and give user a chance to cancel it.
			if(ShouldUseAtoZFolder()) {
				FormP=new FormProgress();
				FormP.MaxVal=100;//We will be setting maxVal from worker thread.  (double)fileSize/1024;
				FormP.NumberMultiplication=100;
				FormP.DisplayText="";//We will set the text from the worker thread.
				FormP.NumberFormat="N1";
				//start the thread that will perform the database copy
				Thread workerThread=new Thread(new ThreadStart(InstanceMethodRestore));
				workerThread.Start();
				//display the progress dialog to the user:
				FormP.ShowDialog();
				if(FormP.DialogResult==DialogResult.Cancel){
					workerThread.Abort();
					return;
				}
			}
			Version programVersionDb=new Version(PrefC.GetStringNoCache(PrefName.ProgramVersion));
			Version programVersionCur=new Version(Application.ProductVersion);
			if(programVersionDb!=programVersionCur) {
				MsgBox.Show(this,"The restored database version is different than the version installed and requires a restart.  The program will now close.");
				FormOpenDental.S_ProcessKillCommand();
				return;
			}
			else {
				DataValid.SetInvalid(Cache.GetAllCachedInvalidTypes().ToArray());
			}
			MsgBox.Show(this,"Done");
			Close();
			return;
		}

		/*
		private void FillTableWithData(StringBuilder tableData,string tempFile,string tableName){
			using(StreamWriter sw=new StreamWriter(tempFile,false)){//new file each time
				sw.Write(tableData.ToString());
			}
			string command="LOAD DATA INFILE '"+tempFile.Replace("\\","/")+"' INTO TABLE "+tableName;
			Db.NonQ(command);
		}*/

		private void butSave_Click(object sender, System.EventArgs e) {
			//test for trailing slashes
			if(textBackupFromPath.Text!="" && !textBackupFromPath.Text.EndsWith(""+Path.DirectorySeparatorChar)){
				MessageBox.Show(Lan.g(this,"Paths must end with ")+Path.DirectorySeparatorChar+".");
				return;
			}
			if(textBackupToPath.Text!="" && !textBackupToPath.Text.EndsWith(""+Path.DirectorySeparatorChar)) {
				MessageBox.Show(Lan.g(this,"Paths must end with ")+Path.DirectorySeparatorChar+".");
				return;
			}
			if(textBackupRestoreFromPath.Text!="" && !textBackupRestoreFromPath.Text.EndsWith(""+Path.DirectorySeparatorChar)) {
				MessageBox.Show(Lan.g(this,"Paths must end with ")+Path.DirectorySeparatorChar+".");
				return;
			}
			if(textBackupRestoreToPath.Text!="" && !textBackupRestoreToPath.Text.EndsWith(""+Path.DirectorySeparatorChar)) {
				MessageBox.Show(Lan.g(this,"Paths must end with ")+Path.DirectorySeparatorChar+".");
				return;
			}
			if(textBackupRestoreAtoZToPath.Text!="" && !textBackupRestoreAtoZToPath.Text.EndsWith(""+Path.DirectorySeparatorChar)) {
				MessageBox.Show(Lan.g(this,"Paths must end with ")+Path.DirectorySeparatorChar+".");
				return;
			}
			if(textBackupFromPath.Text!=PrefC.GetString(PrefName.BackupFromPath)
				|| textBackupToPath.Text!=PrefC.GetString(PrefName.BackupToPath)
				|| textBackupRestoreFromPath.Text!=PrefC.GetString(PrefName.BackupRestoreFromPath)
				|| textBackupRestoreToPath.Text!=PrefC.GetString(PrefName.BackupRestoreToPath)
				|| textBackupRestoreAtoZToPath.Text!=PrefC.GetString(PrefName.BackupRestoreAtoZToPath)
				|| checkExcludeImages.Checked!=PrefC.GetBool(PrefName.BackupExcludeImageFolder))
			{
				Prefs.UpdateString(PrefName.BackupFromPath,textBackupFromPath.Text);
				Prefs.UpdateString(PrefName.BackupToPath,textBackupToPath.Text);
				Prefs.UpdateString(PrefName.BackupRestoreFromPath,textBackupRestoreFromPath.Text);
				Prefs.UpdateString(PrefName.BackupRestoreToPath,textBackupRestoreToPath.Text);
				Prefs.UpdateString(PrefName.BackupRestoreAtoZToPath,textBackupRestoreAtoZToPath.Text);
				Prefs.UpdateBool(PrefName.BackupExcludeImageFolder,checkExcludeImages.Checked);
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void checkExcludeImages_Click(object sender,EventArgs e) {
			textBackupRestoreAtoZToPath.Enabled=ShouldUseAtoZFolder();
			butBrowseRestoreAtoZTo.Enabled=ShouldUseAtoZFolder();
		}

		private void pictureCDS_Click(object sender,EventArgs e) {
			CDS.ShowPage();
		}

		#endregion

		#region Archive Tab

		private void textArchivePass_TextChanged(object sender,EventArgs e) {
			//If text is cleared, turn off password char mask
			if(textArchivePass.Text=="") {
				textArchivePass.PasswordChar=default(char);
			}
		}

		private void textArchivePass_Leave(object sender,EventArgs e) {
			textArchivePass.PasswordChar=(textArchivePass.Text=="" ? default(char) : '*');
		}

		private void butArchive_Click(object sender,EventArgs e) {
			#region Connection settings UI validation
			if(string.IsNullOrWhiteSpace(textArchiveServerName.Text)) {
				MsgBox.Show(this,"Please specify a Server Name.");
				return;
			}
			if(string.IsNullOrWhiteSpace(textArchiveUser.Text)) {
				MsgBox.Show(this,"Please enter a User.");
				return;
			}
			#endregion
			DataConnection dcon=new DataConnection();
			//Keep track of the original connection settings so that we can revert back to them once finished archiving.
			string connectionStrOrig=DataConnection.GetCurrentConnectionString();
			DatabaseType dbTypeOrig=DataConnection.DBtype;
			//Keep track of what the current global exit code is (should always be zero) so that we can put it back to what it was after upgrading.
			int exitCodeOld=FormOpenDental.ExitCode;
			try {
				Version versionDbOrig=new Version(PrefC.GetString(PrefName.DataBaseVersion));
				string connectionStrArchive=DataConnection.BuildSimpleConnectionString(DatabaseType.MySql,
					textArchiveServerName.Text,
					MiscData.GetArchiveDatabaseName(),
					textArchiveUser.Text,
					textArchivePass.Text);
				DatabaseType dbTypeArchive=DataConnection.DBtype;
				#region Connect or create archive database
				//Attempt to connect to the archive database.
				try { 
					dcon.SetDb(connectionStrArchive,"",dbTypeArchive,true);
				}
				catch(Exception ex) {
					if(ex.Message=="Unable to connect to any of the specified MySQL hosts.") {
						//Server name incorrect - Message box and Return
						MsgBox.Show(this,"The specified server name is incorrect or the server is offline.\r\n"
							+"Please check and try again.");
						return;
					}
					else if(ex.Message.Contains("Access denied for user")) {
						//User name or password incorrect - Message box and Return
						MsgBox.Show(this,"The supplied User and/or Password are incorrect.\r\n"
							+"Please check and try again.");
						return;
					}
					else if(ex.Message.Contains("Unknown database")) {
						//Archive DB doesn't exist - Create it, then try to connect again.
						if(MsgBox.Show(this,MsgBoxButtons.YesNo,"The archive database doesn't exist at the specified server.\r\n"
							+"Would you like to create it?\r\n\r\n"
							+"WARNING: This can take a while, DO NOT CLOSE THE PROGRAM!"))
						{
							if(!CreateArchiveDB(dcon,connectionStrOrig,dbTypeOrig,connectionStrArchive,dbTypeArchive)) {
								return;//Creating archive database failed.
							}
						}
						else {
							return;
						}
					}
				}
				#endregion
				#region Validate archive database version
				//At this point there is an active connection to the archive database, validate the DataBaseVersion.
				string version=PrefC.GetStringNoCache(PrefName.DataBaseVersion);
				if(string.IsNullOrEmpty(version)) {
					//Preference table does not have version information.  Somehow they have a database with proper structure but no data.
					//This archive database can't be trusted and we have no idea what version the schema is at.
					//They need to call support so that we can take a look or they need to delete the invalid archive (or remove it from the data dir) 
					//so that a new archive database can be made from scratch.
					MsgBox.Show(this,"Invalid archive database detected.\r\n"
						+"Please call support.");
					return;
				}
				Version versionDbArchive=new Version(version);
				if(versionDbOrig>versionDbArchive) {
					//We need to update the archive version
					if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Archive database needs to be backed up and updated."
						+"Continue?\r\n\r\n"
						+"WARNING: This can take a while, DO NOT CLOSE THE PROGRAM!"))
					{
						return;
					}
					//Back the archive database up and upgrade it to the version that we are currently connected with.
					if(!new ClassConvertDatabase().Convert(versionDbArchive.ToString(),versionDbOrig.ToString(),true)) {
						MsgBox.Show(this,"Error backing up or upgrading archive database - error code: "+FormOpenDental.ExitCode+"\r\n"
							+"Please call support.");
						return;
					}
				}
				else if(versionDbArchive>versionDbOrig) {
					MsgBox.Show(this,"Archive database version is higher than the current database.  Process cannot continue.");
					return;
				}
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Archival process is about to commence.\r\n"
					+"Continue?\r\n\r\n"
					+"WARNING: This can take a while, DO NOT CLOSE THE PROGRAM!"))
				{
					return;
				}
				#endregion
				#region Commence archive process
				if(Archive(dcon,connectionStrOrig,dbTypeOrig,connectionStrArchive,dbTypeArchive)) {
					//Successful archive was made.
					MsgBox.Show(this,"Archive process completed successfully.");
				}
				#endregion
			}
			catch(Exception ex) {
				FriendlyException.Show("Unexpected error.",ex);
			}
			finally {//Always put the connection back to the original no matter what happened above when trying to make an archive.
				dcon.SetDb(connectionStrOrig,"",dbTypeOrig);//It is acceptable to crash the program if this fails.
				//Always set the global exit code back to whatever it was before we tried messing around with the archive.
				FormOpenDental.ExitCode=exitCodeOld;
			}
		}

		///<summary>Creates an archive database for the connection information passed in.  Shows error messages to the user.
		///Returns true if the archive database was created; Overwise, false.</summary>
		private bool CreateArchiveDB(DataConnection dcon,string connectionStrOrig,DatabaseType dbTypeOrig,string connectionStrArchive,
			DatabaseType dbTypeArchive) 
		{
			bool isSuccess=false;
			ODProgress.ShowProgressForThread(
				odThread => {
					#region Create archive database
					//Create the archive database with the current connection settings.
					MiscData.CreateArchiveDatabase();
					#endregion
					#region Generate table queries
					//Create the shell of an archive database from the original.
					//The following section of code was donated by the Crud Generator - A very selfless act (Also Form1 when running the crud)
					Type typeTableBase=typeof(TableBase);
					List<string> listTableCommands=new List<string>();
					Assembly assembly=Assembly.GetAssembly(typeTableBase);
					foreach(Type typeClass in assembly.GetTypes()) {
						if(typeClass.IsSubclassOf(typeTableBase)) {
							try { 
								//Some tables have a different name than the tabletype object from Assembly. (preference vs. pref for instance)
								string tableName=typeClass.Name.ToLower();
								object[] attributes=typeClass.GetCustomAttributes(typeof(CrudTableAttribute),true);
								for(int i=0;i<attributes.Length;i++) {
									if(attributes[i].GetType()!=typeof(CrudTableAttribute)) {
										continue;
									}
									if(((CrudTableAttribute)attributes[i]).TableName!="") {
										tableName=((CrudTableAttribute)attributes[i]).TableName;
									}
								}
								listTableCommands.Add(MiscData.GenerateTableQuery(tableName));//Generate CREATE TABLE statements from current db.
							}
							catch (Exception ex2) {
								if(ex2.Message.Contains("doesn't exist")) {//table doesn't exist.  Like phone table or other HQ specific tables.
									continue;
								}
							}
						}
					}
					#endregion
					#region Execute table queries and copy preferences
					DataTable preferences=Prefs.GetTableFromCache(true);//Copy preferences from current db.
					//Switch connection to archive db so we can create tables and copy over preferences (keeps track of db version)
					dcon.SetDb(connectionStrArchive,"",dbTypeArchive,true);
					odThread.ProgressLog.UpdateProgress("Making new tables");
					MiscData.MakeTables(listTableCommands);
					odThread.ProgressLog.UpdateProgress("Inserting preferences");
					MiscData.InsertPreferences(preferences);
					#endregion
					isSuccess=true;
				},
				this,
				"Creating archive database...",
				ProgBarStyle.Marquee,
				"CreateArchiveDB",
				e => this.Invoke(() => {
					FriendlyException.Show(Lan.g(this,"Error creating the archive database."),e);
				})
			);
			//No matter what happened above, set the db context back to the original connection.
			dcon.SetDb(connectionStrOrig,"",dbTypeOrig);
			return isSuccess;
		}

		///<summary>Performs the actual archive process.  Shows error messages to the user.
		///Returns true if the archive process was successful; Overwise, false.</summary>
		private bool Archive(DataConnection dcon,string connectionStrOrig,DatabaseType dbTypeOrig,string connectionStrArchive,
			DatabaseType dbTypeArchive) 
		{
			bool isSuccess=false;
			ODProgress.ShowProgressForThread(
				odThread => {
					#region Insert items from original to archive
					//Reset to the original db once again.  Bulk insert securitylog entries and securityloghash entries that are prior to the selected date.
					dcon.SetDb(connectionStrOrig,"",dbTypeOrig);
					//Insert security logs and security log hashes using our large table helper logic.
					//Uses insert batches, multiple threads, and makes sure inserts are under the max allowed packet size.
					odThread.ProgressLog.UpdateProgress("Inserting security logs");
					string errorMsg=LargeTableHelper.BulkInsertSecurityLogs(textArchiveServerName.Text,
						textArchiveUser.Text,
						textArchivePass.Text,
						dateTimeArchive.Value);
					//Grab security log max primary key for delete statements later.  
					long maxPriKeySecurityLog=(LargeTableHelper.ListPriKeyMaxPerBatch.Count>0 ? LargeTableHelper.ListPriKeyMaxPerBatch.Max() : 0);
					odThread.ProgressLog.UpdateProgress("Inserting security log hashes");
					//BulkInsertSecurityLogHashes must be run after BulkInsertSecurityLogs.
					errorMsg+=LargeTableHelper.BulkInsertSecurityLogHashes();
					//Grab security log hash max primary key for delete statements later.
					long maxPriKeySecurityLogHash=(LargeTableHelper.ListPriKeyMaxPerBatch.Count>0 ? LargeTableHelper.ListPriKeyMaxPerBatch.Max() : 0);
					if(errorMsg!="") {
						throw new ApplicationException(errorMsg);
					}
					#endregion
					#region Verify archive integrity
					dcon.SetDb(connectionStrArchive,"",dbTypeArchive,true);
					if(maxPriKeySecurityLog!=0 && SecurityLogs.GetOne(maxPriKeySecurityLog)==null) {
						throw new ApplicationException("Archival process failed.  The archive securitylog table does not have all of the archived rows.\r\n"
							+"Please call support.");
					}
					if(maxPriKeySecurityLogHash!=0 && SecurityLogHashes.GetOne(maxPriKeySecurityLogHash)==null) {
						throw new ApplicationException("Archival process failed.  The archive securityloghash table does not have all of the archived rows.\r\n"
							+"Please call support.");
					}
					#endregion
					#region Delete items from original database
					dcon.SetDb(connectionStrOrig,"",dbTypeOrig);//Reset to the original db once again.
					//Cleaning up - Delete SecurityLog and SecurityLogHash items in our original databasesecurity logs
					odThread.ProgressLog.UpdateProgress("Deleting security logs");
					SecurityLogs.DeleteWithMaxPriKey(maxPriKeySecurityLog);//Due to the nature of bulk inserts (and how the rows were selected for insert) we only know the maximum primary key.
					odThread.ProgressLog.UpdateProgress("Deleting security log hashes");
					SecurityLogHashes.DeleteWithMaxPriKey(maxPriKeySecurityLogHash);//Due to the nature of bulk inserts (and how the rows were selected for insert) we only know the maximum primary key.
					#endregion
					#region Save ArchiveDate and defaults
					Prefs.UpdateDateT(PrefName.ArchiveDate,dateTimeArchive.Value);
					Prefs.UpdateString(PrefName.ArchiveServerName,textArchiveServerName.Text);
					Prefs.UpdateString(PrefName.ArchiveUserName,textArchiveUser.Text);
					string encryptedPass;
					CDT.Class1.Encrypt(textArchivePass.Text,out encryptedPass);
					Prefs.UpdateString(PrefName.ArchivePassHash,encryptedPass);
					#endregion
					isSuccess=true;
				},
				this,
				"Archiving...",
				ProgBarStyle.Marquee,
				"ArchiveDB",
				ex => this.Invoke(() => {
					FriendlyException.Show(Lan.g(this,"Error during the archival process."),ex);
				})
			);
			//No matter what happened above, set the db context back to the original connection.
			dcon.SetDb(connectionStrOrig,"",dbTypeOrig);
			return isSuccess;
		}

		private void butSaveArchive_Click(object sender,EventArgs e) {
			Prefs.UpdateString(PrefName.ArchiveServerName,textArchiveServerName.Text);
			Prefs.UpdateString(PrefName.ArchiveUserName,textArchiveUser.Text);
			string encryptedPass;
      CDT.Class1.Encrypt(textArchivePass.Text,out encryptedPass);
			Prefs.UpdateString(PrefName.ArchivePassHash,encryptedPass);
			DataValid.SetInvalid(InvalidType.Prefs);
			MsgBox.Show(this,"Saved");
		}

		#endregion
	}

	///<summary>Backing up can fail at two points, when backing up the database or the A to Z images.  This delegate lets the backup thread manipulate a local variable so that we can let the user know at what point the backup failed.</summary>
	public delegate void ErrorMessageDelegate(string errorMessage);
}





















