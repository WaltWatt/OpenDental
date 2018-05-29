using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Linq;
using System.Xml.Serialization;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormUpdate : ODForm {
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.Label labelVersion;
		private OpenDental.UI.Button butDownload;
		private OpenDental.UI.Button butCheck;
		private System.Windows.Forms.TextBox textUpdateCode;
		private System.Windows.Forms.TextBox textWebsitePath;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private IContainer components;
		private TextBox textResult;
		private TextBox textResult2;
		private Label label4;
		private Label label6;
		private MainMenu mainMenu1;
		private MenuItem menuItemSetup;
		private Panel panelClassic;
		private TextBox textConnectionMessage;
		private GroupBox groupBuild;
		private Label label2;
		private TextBox textBuild;
		private OpenDental.UI.Button butInstallBuild;
		private GroupBox groupBeta;
		private TextBox textBeta;
		private OpenDental.UI.Button butInstallBeta;
		private Label label5;
		private GroupBox groupStable;
		private TextBox textStable;
		private OpenDental.UI.Button butInstallStable;
		private Label label11;
		private OpenDental.UI.Button butCheck2;//OD1
		//<summary>Includes path</summary>
		//string WriteToFile;
		private static string buildAvailable;
		private static string buildAvailableCode;
		private static string buildAvailableDisplay;
		private static string stableAvailable;
		private static string stableAvailableCode;
		private static string stableAvailableDisplay;
		private static string betaAvailable;
		private static string betaAvailableCode;
		private OpenDental.UI.Button butDownloadMsiBuild;
		private OpenDental.UI.Button butDownloadMsiBeta;
		private OpenDental.UI.Button butDownloadMsiStable;
		private UI.Button butShowPrev;
		private static string betaAvailableDisplay;

		///<summary></summary>
		public FormUpdate()
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUpdate));
			this.labelVersion = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textUpdateCode = new System.Windows.Forms.TextBox();
			this.textWebsitePath = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textResult = new System.Windows.Forms.TextBox();
			this.textResult2 = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.menuItemSetup = new System.Windows.Forms.MenuItem();
			this.panelClassic = new System.Windows.Forms.Panel();
			this.butCheck = new OpenDental.UI.Button();
			this.butDownload = new OpenDental.UI.Button();
			this.textConnectionMessage = new System.Windows.Forms.TextBox();
			this.groupBuild = new System.Windows.Forms.GroupBox();
			this.butDownloadMsiBuild = new OpenDental.UI.Button();
			this.textBuild = new System.Windows.Forms.TextBox();
			this.butInstallBuild = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBeta = new System.Windows.Forms.GroupBox();
			this.butDownloadMsiBeta = new OpenDental.UI.Button();
			this.textBeta = new System.Windows.Forms.TextBox();
			this.butInstallBeta = new OpenDental.UI.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.groupStable = new System.Windows.Forms.GroupBox();
			this.butDownloadMsiStable = new OpenDental.UI.Button();
			this.textStable = new System.Windows.Forms.TextBox();
			this.butInstallStable = new OpenDental.UI.Button();
			this.label11 = new System.Windows.Forms.Label();
			this.butCheck2 = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.butShowPrev = new OpenDental.UI.Button();
			this.panelClassic.SuspendLayout();
			this.groupBuild.SuspendLayout();
			this.groupBeta.SuspendLayout();
			this.groupStable.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelVersion
			// 
			this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelVersion.Location = new System.Drawing.Point(74, 9);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(280, 20);
			this.labelVersion.TabIndex = 10;
			this.labelVersion.Text = "Using Version ";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 23);
			this.label1.TabIndex = 0;
			// 
			// textUpdateCode
			// 
			this.textUpdateCode.Location = new System.Drawing.Point(129, 100);
			this.textUpdateCode.Name = "textUpdateCode";
			this.textUpdateCode.Size = new System.Drawing.Size(113, 20);
			this.textUpdateCode.TabIndex = 19;
			// 
			// textWebsitePath
			// 
			this.textWebsitePath.Location = new System.Drawing.Point(129, 77);
			this.textWebsitePath.Name = "textWebsitePath";
			this.textWebsitePath.Size = new System.Drawing.Size(388, 20);
			this.textWebsitePath.TabIndex = 24;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(24, 78);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(105, 19);
			this.label3.TabIndex = 26;
			this.label3.Text = "Website Path";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textResult
			// 
			this.textResult.AcceptsReturn = true;
			this.textResult.BackColor = System.Drawing.SystemColors.Window;
			this.textResult.Location = new System.Drawing.Point(129, 156);
			this.textResult.Name = "textResult";
			this.textResult.ReadOnly = true;
			this.textResult.Size = new System.Drawing.Size(388, 20);
			this.textResult.TabIndex = 34;
			// 
			// textResult2
			// 
			this.textResult2.AcceptsReturn = true;
			this.textResult2.BackColor = System.Drawing.SystemColors.Window;
			this.textResult2.Location = new System.Drawing.Point(129, 179);
			this.textResult2.Multiline = true;
			this.textResult2.Name = "textResult2";
			this.textResult2.ReadOnly = true;
			this.textResult2.Size = new System.Drawing.Size(388, 66);
			this.textResult2.TabIndex = 35;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(6, 100);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(120, 19);
			this.label4.TabIndex = 34;
			this.label4.Text = "Update Code";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(10, 8);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(555, 58);
			this.label6.TabIndex = 40;
			this.label6.Text = resources.GetString("label6.Text");
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSetup});
			// 
			// menuItemSetup
			// 
			this.menuItemSetup.Index = 0;
			this.menuItemSetup.Text = "Setup";
			this.menuItemSetup.Click += new System.EventHandler(this.menuItemSetup_Click);
			// 
			// panelClassic
			// 
			this.panelClassic.Controls.Add(this.textWebsitePath);
			this.panelClassic.Controls.Add(this.textUpdateCode);
			this.panelClassic.Controls.Add(this.butCheck);
			this.panelClassic.Controls.Add(this.label3);
			this.panelClassic.Controls.Add(this.textResult);
			this.panelClassic.Controls.Add(this.label4);
			this.panelClassic.Controls.Add(this.label6);
			this.panelClassic.Controls.Add(this.textResult2);
			this.panelClassic.Controls.Add(this.butDownload);
			this.panelClassic.Location = new System.Drawing.Point(471, 12);
			this.panelClassic.Name = "panelClassic";
			this.panelClassic.Size = new System.Drawing.Size(568, 494);
			this.panelClassic.TabIndex = 48;
			this.panelClassic.Visible = false;
			// 
			// butCheck
			// 
			this.butCheck.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCheck.Autosize = true;
			this.butCheck.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCheck.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCheck.CornerRadius = 4F;
			this.butCheck.Location = new System.Drawing.Point(129, 125);
			this.butCheck.Name = "butCheck";
			this.butCheck.Size = new System.Drawing.Size(117, 25);
			this.butCheck.TabIndex = 21;
			this.butCheck.Text = "Check for Updates";
			this.butCheck.Click += new System.EventHandler(this.butCheck_Click);
			// 
			// butDownload
			// 
			this.butDownload.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDownload.Autosize = true;
			this.butDownload.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDownload.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDownload.CornerRadius = 4F;
			this.butDownload.Location = new System.Drawing.Point(129, 251);
			this.butDownload.Name = "butDownload";
			this.butDownload.Size = new System.Drawing.Size(83, 25);
			this.butDownload.TabIndex = 20;
			this.butDownload.Text = "Download";
			this.butDownload.Click += new System.EventHandler(this.butDownload_Click);
			// 
			// textConnectionMessage
			// 
			this.textConnectionMessage.AcceptsReturn = true;
			this.textConnectionMessage.BackColor = System.Drawing.SystemColors.Window;
			this.textConnectionMessage.Location = new System.Drawing.Point(77, 62);
			this.textConnectionMessage.Multiline = true;
			this.textConnectionMessage.Name = "textConnectionMessage";
			this.textConnectionMessage.ReadOnly = true;
			this.textConnectionMessage.Size = new System.Drawing.Size(388, 66);
			this.textConnectionMessage.TabIndex = 50;
			// 
			// groupBuild
			// 
			this.groupBuild.Controls.Add(this.butDownloadMsiBuild);
			this.groupBuild.Controls.Add(this.textBuild);
			this.groupBuild.Controls.Add(this.butInstallBuild);
			this.groupBuild.Controls.Add(this.label2);
			this.groupBuild.Location = new System.Drawing.Point(77, 141);
			this.groupBuild.Name = "groupBuild";
			this.groupBuild.Size = new System.Drawing.Size(388, 111);
			this.groupBuild.TabIndex = 51;
			this.groupBuild.TabStop = false;
			this.groupBuild.Text = "A new build is available for the current version";
			this.groupBuild.Visible = false;
			// 
			// butDownloadMsiBuild
			// 
			this.butDownloadMsiBuild.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDownloadMsiBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDownloadMsiBuild.Autosize = true;
			this.butDownloadMsiBuild.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDownloadMsiBuild.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDownloadMsiBuild.CornerRadius = 4F;
			this.butDownloadMsiBuild.Location = new System.Drawing.Point(220, 80);
			this.butDownloadMsiBuild.Name = "butDownloadMsiBuild";
			this.butDownloadMsiBuild.Size = new System.Drawing.Size(83, 25);
			this.butDownloadMsiBuild.TabIndex = 52;
			this.butDownloadMsiBuild.Text = "Download msi";
			this.butDownloadMsiBuild.Click += new System.EventHandler(this.butDownMsiBuild_Click);
			// 
			// textBuild
			// 
			this.textBuild.AcceptsReturn = true;
			this.textBuild.BackColor = System.Drawing.SystemColors.Window;
			this.textBuild.Location = new System.Drawing.Point(6, 54);
			this.textBuild.Name = "textBuild";
			this.textBuild.ReadOnly = true;
			this.textBuild.Size = new System.Drawing.Size(376, 20);
			this.textBuild.TabIndex = 51;
			// 
			// butInstallBuild
			// 
			this.butInstallBuild.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butInstallBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butInstallBuild.Autosize = true;
			this.butInstallBuild.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butInstallBuild.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butInstallBuild.CornerRadius = 4F;
			this.butInstallBuild.Location = new System.Drawing.Point(309, 80);
			this.butInstallBuild.Name = "butInstallBuild";
			this.butInstallBuild.Size = new System.Drawing.Size(73, 25);
			this.butInstallBuild.TabIndex = 28;
			this.butInstallBuild.Text = "Install";
			this.butInstallBuild.Click += new System.EventHandler(this.butInstallBuild_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(374, 29);
			this.label2.TabIndex = 27;
			this.label2.Text = "These are typically bug fixes.  It is strongly recommended to install any availab" +
    "le fixes.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupBeta
			// 
			this.groupBeta.Controls.Add(this.butDownloadMsiBeta);
			this.groupBeta.Controls.Add(this.textBeta);
			this.groupBeta.Controls.Add(this.butInstallBeta);
			this.groupBeta.Controls.Add(this.label5);
			this.groupBeta.Location = new System.Drawing.Point(77, 393);
			this.groupBeta.Name = "groupBeta";
			this.groupBeta.Size = new System.Drawing.Size(388, 119);
			this.groupBeta.TabIndex = 52;
			this.groupBeta.TabStop = false;
			this.groupBeta.Text = "A new beta version is available";
			this.groupBeta.Visible = false;
			// 
			// butDownloadMsiBeta
			// 
			this.butDownloadMsiBeta.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDownloadMsiBeta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDownloadMsiBeta.Autosize = true;
			this.butDownloadMsiBeta.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDownloadMsiBeta.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDownloadMsiBeta.CornerRadius = 4F;
			this.butDownloadMsiBeta.Location = new System.Drawing.Point(220, 88);
			this.butDownloadMsiBeta.Name = "butDownloadMsiBeta";
			this.butDownloadMsiBeta.Size = new System.Drawing.Size(83, 25);
			this.butDownloadMsiBeta.TabIndex = 53;
			this.butDownloadMsiBeta.Text = "Download msi";
			this.butDownloadMsiBeta.Click += new System.EventHandler(this.butDownloadMsiBeta_Click);
			// 
			// textBeta
			// 
			this.textBeta.AcceptsReturn = true;
			this.textBeta.BackColor = System.Drawing.SystemColors.Window;
			this.textBeta.Location = new System.Drawing.Point(6, 62);
			this.textBeta.Name = "textBeta";
			this.textBeta.ReadOnly = true;
			this.textBeta.Size = new System.Drawing.Size(376, 20);
			this.textBeta.TabIndex = 51;
			// 
			// butInstallBeta
			// 
			this.butInstallBeta.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butInstallBeta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butInstallBeta.Autosize = true;
			this.butInstallBeta.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butInstallBeta.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butInstallBeta.CornerRadius = 4F;
			this.butInstallBeta.Location = new System.Drawing.Point(309, 88);
			this.butInstallBeta.Name = "butInstallBeta";
			this.butInstallBeta.Size = new System.Drawing.Size(73, 25);
			this.butInstallBeta.TabIndex = 28;
			this.butInstallBeta.Text = "Install";
			this.butInstallBeta.Click += new System.EventHandler(this.butInstallBeta_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(6, 13);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(374, 46);
			this.label5.TabIndex = 27;
			this.label5.Text = "This beta version will be very functional, but will have some bugs.  Use a beta v" +
    "ersion only if you demand the latest features.  Be sure to update regularly.";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupStable
			// 
			this.groupStable.Controls.Add(this.butDownloadMsiStable);
			this.groupStable.Controls.Add(this.textStable);
			this.groupStable.Controls.Add(this.butInstallStable);
			this.groupStable.Controls.Add(this.label11);
			this.groupStable.Location = new System.Drawing.Point(77, 267);
			this.groupStable.Name = "groupStable";
			this.groupStable.Size = new System.Drawing.Size(388, 111);
			this.groupStable.TabIndex = 53;
			this.groupStable.TabStop = false;
			this.groupStable.Text = "A new stable version is available";
			this.groupStable.Visible = false;
			// 
			// butDownloadMsiStable
			// 
			this.butDownloadMsiStable.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDownloadMsiStable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDownloadMsiStable.Autosize = true;
			this.butDownloadMsiStable.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDownloadMsiStable.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDownloadMsiStable.CornerRadius = 4F;
			this.butDownloadMsiStable.Location = new System.Drawing.Point(220, 80);
			this.butDownloadMsiStable.Name = "butDownloadMsiStable";
			this.butDownloadMsiStable.Size = new System.Drawing.Size(83, 25);
			this.butDownloadMsiStable.TabIndex = 53;
			this.butDownloadMsiStable.Text = "Download msi";
			this.butDownloadMsiStable.Click += new System.EventHandler(this.butDownloadMsiStable_Click);
			// 
			// textStable
			// 
			this.textStable.AcceptsReturn = true;
			this.textStable.BackColor = System.Drawing.SystemColors.Window;
			this.textStable.Location = new System.Drawing.Point(6, 54);
			this.textStable.Name = "textStable";
			this.textStable.ReadOnly = true;
			this.textStable.Size = new System.Drawing.Size(376, 20);
			this.textStable.TabIndex = 51;
			// 
			// butInstallStable
			// 
			this.butInstallStable.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butInstallStable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butInstallStable.Autosize = true;
			this.butInstallStable.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butInstallStable.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butInstallStable.CornerRadius = 4F;
			this.butInstallStable.Location = new System.Drawing.Point(309, 80);
			this.butInstallStable.Name = "butInstallStable";
			this.butInstallStable.Size = new System.Drawing.Size(73, 25);
			this.butInstallStable.TabIndex = 28;
			this.butInstallStable.Text = "Install";
			this.butInstallStable.Click += new System.EventHandler(this.butInstallStable_Click);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(6, 22);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(374, 29);
			this.label11.TabIndex = 27;
			this.label11.Text = "Will have nearly zero bugs.  Will provide many useful enhanced features.";
			this.label11.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butCheck2
			// 
			this.butCheck2.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCheck2.Autosize = true;
			this.butCheck2.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCheck2.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCheck2.CornerRadius = 4F;
			this.butCheck2.Location = new System.Drawing.Point(77, 31);
			this.butCheck2.Name = "butCheck2";
			this.butCheck2.Size = new System.Drawing.Size(117, 25);
			this.butCheck2.TabIndex = 54;
			this.butCheck2.Text = "Check for Updates";
			this.butCheck2.Visible = false;
			this.butCheck2.Click += new System.EventHandler(this.butCheck2_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(560, 520);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 25);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butShowPrev
			// 
			this.butShowPrev.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butShowPrev.Autosize = true;
			this.butShowPrev.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShowPrev.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShowPrev.CornerRadius = 4F;
			this.butShowPrev.Location = new System.Drawing.Point(333, 32);
			this.butShowPrev.Name = "butShowPrev";
			this.butShowPrev.Size = new System.Drawing.Size(132, 25);
			this.butShowPrev.TabIndex = 55;
			this.butShowPrev.Text = "Show Previous Versions";
			this.butShowPrev.Click += new System.EventHandler(this.butShowPrev_Click);
			// 
			// FormUpdate
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(647, 560);
			this.Controls.Add(this.butShowPrev);
			this.Controls.Add(this.panelClassic);
			this.Controls.Add(this.butCheck2);
			this.Controls.Add(this.groupStable);
			this.Controls.Add(this.groupBeta);
			this.Controls.Add(this.groupBuild);
			this.Controls.Add(this.textConnectionMessage);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.labelVersion);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Menu = this.mainMenu1;
			this.MinimizeBox = false;
			this.Name = "FormUpdate";
			this.Text = "Update";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormUpdate_FormClosing);
			this.Load += new System.EventHandler(this.FormUpdate_Load);
			this.panelClassic.ResumeLayout(false);
			this.panelClassic.PerformLayout();
			this.groupBuild.ResumeLayout(false);
			this.groupBuild.PerformLayout();
			this.groupBeta.ResumeLayout(false);
			this.groupBeta.PerformLayout();
			this.groupStable.ResumeLayout(false);
			this.groupStable.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormUpdate_Load(object sender, System.EventArgs e) {
			SetButtonVisibility();
			labelVersion.Text=Lan.g(this,"Using Version:")+" "+Application.ProductVersion;
			UpdateHistory updateHistory=UpdateHistories.GetForVersion(Application.ProductVersion);
			if(updateHistory!=null) {
				labelVersion.Text+="  "+Lan.g(this,"Since")+": "+updateHistory.DateTimeUpdated.ToShortDateString();
			}
			if(PrefC.GetBool(PrefName.UpdateWindowShowsClassicView)){
				//Default location is (74,9).  We move it 5 pixels up since butShowPrev is 5 pixels bigger then labelVersion
				butShowPrev.Location=new Point(74+labelVersion.Width+2,9-5);
				panelClassic.Visible=true;
				panelClassic.Location=new Point(67,29);
				textUpdateCode.Text=PrefC.GetString(PrefName.UpdateCode);
				textWebsitePath.Text=PrefC.GetString(PrefName.UpdateWebsitePath);//should include trailing /
				butDownload.Enabled=false;
				if(!Security.IsAuthorized(Permissions.Setup)){//gives a message box if no permission
					butCheck.Enabled=false;
				}
			}
			else{
				if(Security.IsAuthorized(Permissions.Setup,true)) {
					butCheck2.Visible=true;
				}
				else {
					textConnectionMessage.Text=Lan.g(this,"Not authorized for")+" "+GroupPermissions.GetDesc(Permissions.Setup);
				}
			}
		}

		private void menuItemSetup_Click(object sender,EventArgs e) {
			if(PrefC.GetBool(PrefName.UpdateWindowShowsClassicView)){
				return;
			}
			FormUpdateSetup FormU=new FormUpdateSetup();
			FormU.ShowDialog();
			SetButtonVisibility();
		}

		private void SetButtonVisibility() {
			DateTime updateTime = PrefC.GetDateT(PrefName.UpdateDateTime);
			bool showMsi = PrefC.GetBool(PrefName.UpdateShowMsiButtons);
			butDownloadMsiBuild.Visible=showMsi;
			butDownloadMsiStable.Visible=showMsi;
			butDownloadMsiBeta.Visible=showMsi;
			butDownloadMsiBuild.Enabled=updateTime < DateTime.Now;
			butDownloadMsiStable.Enabled=updateTime < DateTime.Now;
			butDownloadMsiBeta.Enabled=updateTime < DateTime.Now;
			butInstallBuild.Enabled=updateTime < DateTime.Now;
			butInstallStable.Enabled=updateTime < DateTime.Now;
			butInstallBeta.Enabled=updateTime < DateTime.Now;
		}

		private void butCheck2_Click(object sender,EventArgs e) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				MsgBox.Show(this,"Updates are only allowed from the web server");
				return;
			}
			if(PrefC.GetString(PrefName.WebServiceServerName)!="" //using web service
				&&!ODEnvironment.IdIsThisComputer(PrefC.GetString(PrefName.WebServiceServerName).ToLower()))//and not on web server 
			{
				MessageBox.Show(Lan.g(this,"Updates are only allowed from the web server")+": "+PrefC.GetString(PrefName.WebServiceServerName));
				return;
			}
			if(ReplicationServers.ServerIsBlocked()){
				MsgBox.Show(this,"Updates are not allowed on this replication server");
				return;
			}
			Cursor=Cursors.WaitCursor;
			groupBuild.Visible=false;
			groupStable.Visible=false;
			groupBeta.Visible=false;
			textConnectionMessage.Text=Lan.g(this,"Attempting to connect to web service......");
			Application.DoEvents();
			string result="";
			try {
				result=SendAndReceiveXml();
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MessageBox.Show("Error: "+ex.Message);
				return;
			}
			textConnectionMessage.Text=Lan.g(this,"Connection successful.");
			Cursor=Cursors.Default;
			//MessageBox.Show(result);
			try {
				ParseXml(result);//fills the six static variables with values.
			}
			catch(Exception ex) {
				textConnectionMessage.Text=ex.Message;
				MessageBox.Show(ex.Message,"Error");
				return;
			}
			if(buildAvailableDisplay!="") {
				groupBuild.Visible=true;
				textBuild.Text=buildAvailableDisplay;
			}
			if(stableAvailableDisplay!="") {
				groupStable.Visible=true;
				textStable.Text=stableAvailableDisplay;
			}
			if(betaAvailableDisplay!="") {
				groupBeta.Visible=true;
				textBeta.Text=betaAvailableDisplay;
			}
			if(betaAvailable=="" && stableAvailable=="" && buildAvailable=="") {
				textConnectionMessage.Text+=Lan.g(this,"  There are no downloads available.");
			}
			else {
				textConnectionMessage.Text+=Lan.g(this,"  The following downloads are available.  Be sure to stop the program on all other computers in the office before installing.");
			}
		}

		///<summary>Parses the xml result from the server and uses it to fill the 9 static variables.  Or can throw an exception if some sort of error.</summary>
		private static void ParseXml(string result){
			XmlDocument doc=new XmlDocument();
			doc.LoadXml(result);
			XmlNode node=doc.SelectSingleNode("//Error");
			if(node!=null) {
				//textConnectionMessage.Text=node.InnerText;
				//MessageBox.Show(node.InnerText,"Error");
				//return;
				throw new Exception(node.InnerText);
			}
			node=doc.SelectSingleNode("//KeyDisabled");
			if(node==null) {
				//no error, and no disabled message
				if(Prefs.UpdateBool(PrefName.RegistrationKeyIsDisabled,false)) {//this is one of two places in the program where this happens.
					DataValid.SetInvalid(InvalidType.Prefs);
				}
			}
			else {
				//textConnectionMessage.Text=node.InnerText;
				//MessageBox.Show(node.InnerText);
				if(Prefs.UpdateBool(PrefName.RegistrationKeyIsDisabled,true)) {//this is one of two places in the program where this happens.
					DataValid.SetInvalid(InvalidType.Prefs);
				}
				throw new Exception(node.InnerText);
				//return;
			}
			node=doc.SelectSingleNode("//BuildAvailable");
			buildAvailable="";
			buildAvailableCode="";
			buildAvailableDisplay="";
			if(node!=null) {
				node=doc.SelectSingleNode("//BuildAvailable/Display");
				if(node!=null) {
					buildAvailableDisplay=node.InnerText;
				}
				node=doc.SelectSingleNode("//BuildAvailable/MajMinBuildF");
				if(node!=null) {
					buildAvailable=node.InnerText;
				}
				node=doc.SelectSingleNode("//BuildAvailable/UpdateCode");
				if(node!=null) {
					buildAvailableCode=node.InnerText;
				}
			}
			node=doc.SelectSingleNode("//StableAvailable");
			stableAvailable="";
			stableAvailableCode="";
			stableAvailableDisplay="";
			if(node!=null) {
				node=doc.SelectSingleNode("//StableAvailable/Display");
				if(node!=null) {
					stableAvailableDisplay=node.InnerText;
				}
				node=doc.SelectSingleNode("//StableAvailable/MajMinBuildF");
				if(node!=null) {
					stableAvailable=node.InnerText;
				}
				node=doc.SelectSingleNode("//StableAvailable/UpdateCode");
				if(node!=null) {
					stableAvailableCode=node.InnerText;
				}
			}
			node=doc.SelectSingleNode("//BetaAvailable");
			betaAvailable="";
			betaAvailableCode="";
			betaAvailableDisplay="";
			if(node!=null) {
				node=doc.SelectSingleNode("//BetaAvailable/Display");
				if(node!=null) {
					betaAvailableDisplay=node.InnerText;
				}
				node=doc.SelectSingleNode("//BetaAvailable/MajMinBuildF");
				if(node!=null) {
					betaAvailable=node.InnerText;
				}
				node=doc.SelectSingleNode("//BetaAvailable/UpdateCode");
				if(node!=null) {
					betaAvailableCode=node.InnerText;
				}
			}
		}

		private static string SendAndReceiveXml(){ 
			List<string> listProgramsEnabled=Programs.GetWhere(x => x.Enabled && !string.IsNullOrWhiteSpace(x.ProgName))
				.Select(x => x.ProgName).ToList();
			//prepare the xml document to send--------------------------------------------------------------------------------------
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = ("    ");
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,settings)){
				writer.WriteStartElement("UpdateRequest");
				writer.WriteStartElement("RegistrationKey");
				writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));
				writer.WriteEndElement();
				writer.WriteStartElement("PracticeTitle");
				writer.WriteString(PrefC.GetString(PrefName.PracticeTitle));
				writer.WriteEndElement();
				writer.WriteStartElement("PracticePhone");
				writer.WriteString(PrefC.GetString(PrefName.PracticePhone));
				writer.WriteEndElement();
				writer.WriteStartElement("ProgramVersion");
				writer.WriteString(PrefC.GetString(PrefName.ProgramVersion));
				writer.WriteEndElement();
				writer.WriteStartElement("ClinicCount");
				writer.WriteString(PrefC.HasClinicsEnabled ? Clinics.GetCount(true).ToString() : "0");
				writer.WriteEndElement();
				writer.WriteStartElement("ListProgramsEnabled");
				new XmlSerializer(typeof(List<string>)).Serialize(writer,listProgramsEnabled);
				writer.WriteEndElement();
				writer.WriteEndElement();//UpdateRequest
			}
			return CustomerUpdatesProxy.GetWebServiceInstance().RequestUpdate(strbuild.ToString());
		}

		///<summary>Used if we already have the correct version of the program installed, but we need the UpdateCode in order to download the Setup.exe again.  Like when using multiple databases.</summary>
		public static string GetUpdateCodeForThisVersion() {
			string result=result=SendAndReceiveXml();//exception bubbles up.
			ParseXml(result);
			//see if any of the three versions exactly matches this current version.
			Version thisVersion=new Version(Application.ProductVersion);
			string thisVersStr=thisVersion.ToString(3);
			string testVers;
			testVers=buildAvailable.TrimEnd('f');
			if(testVers==thisVersStr) {
				return buildAvailableCode;
			}
			testVers=stableAvailable.TrimEnd('f');
			if(testVers==thisVersStr) {
				return stableAvailableCode;
			}
			testVers=betaAvailable.TrimEnd('f');
			if(testVers==thisVersStr) {
				return betaAvailableCode;
			}
			return "";
		}

		private void butShowPrev_Click(object sender,EventArgs e) {
			FormPreviousVersions FormSPV=new FormPreviousVersions();
			FormSPV.ShowDialog();
		}

		private void butInstallBuild_Click(object sender,EventArgs e) {
			string patchName="Setup.exe";
			string fileNameWithVers=buildAvailable;//6.9.23F
			fileNameWithVers=fileNameWithVers.Replace("F","");//6.9.23
			fileNameWithVers=fileNameWithVers.Replace(".","_");//6_9_23
			fileNameWithVers="Setup_"+fileNameWithVers+".exe";//Setup_6_9_23.exe
			string destDir=ImageStore.GetPreferredAtoZpath();
			string destPath2=null;
			if(destDir==null) {//Not using A to Z folders?
				destDir=PrefC.GetTempFolderPath();
				//destDir2=null;//already null
			}
			else {//using A to Z folders.
				destPath2=ODFileUtils.CombinePaths(destDir,"SetupFiles");
				if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ && !Directory.Exists(destPath2)) {
					Directory.CreateDirectory(destPath2);
				}
				else if(CloudStorage.IsCloudStorage) {
					destDir=PrefC.GetTempFolderPath();//Cloud needs it to be downloaded to a local temp folder
				}
				destPath2=ODFileUtils.CombinePaths(destPath2,fileNameWithVers);
			}
			DownloadInstallPatchFromURI(PrefC.GetString(PrefName.UpdateWebsitePath)+buildAvailableCode+"/"+patchName,//Source URI
				ODFileUtils.CombinePaths(destDir,patchName),//Local destination file.
				true,true,
				destPath2);//second destination file.  Might be null.
		}

		private void butInstallStable_Click(object sender,EventArgs e) {
			FormUpdateInstallMsg FormUIM=new FormUpdateInstallMsg();
			FormUIM.ShowDialog();
			if(FormUIM.DialogResult!=DialogResult.OK) {
				return;
			}
			string patchName="Setup.exe";
			string fileNameWithVers=stableAvailable;
			fileNameWithVers=fileNameWithVers.Replace("F","");
			fileNameWithVers=fileNameWithVers.Replace(".","_");
			fileNameWithVers="Setup_"+fileNameWithVers+".exe";
			string destDir=ImageStore.GetPreferredAtoZpath();
			string destPath2=null;
			if(destDir==null) {//Not using A to Z folders?
				destDir=PrefC.GetTempFolderPath();
			}
			else {
				destPath2=ODFileUtils.CombinePaths(destDir,"SetupFiles");
				if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ && !Directory.Exists(destPath2)) {
					Directory.CreateDirectory(destPath2);
				}
				else if(CloudStorage.IsCloudStorage) {
					destDir=PrefC.GetTempFolderPath();//Cloud storing needs it to be downloaded to a local temp folder
				}
				destPath2=ODFileUtils.CombinePaths(destPath2,fileNameWithVers);
			}
			DownloadInstallPatchFromURI(PrefC.GetString(PrefName.UpdateWebsitePath)+stableAvailableCode+"/"+patchName,//Source URI
				ODFileUtils.CombinePaths(destDir,patchName),
				true,true,
				destPath2);
		}

		private void butInstallBeta_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Are you sure you really want to install a beta version?  Do NOT do this unless you are OK with some bugs.  Continue?")){
				return;
			}
			FormUpdateInstallMsg FormUIM=new FormUpdateInstallMsg();
			FormUIM.ShowDialog();
			if(FormUIM.DialogResult!=DialogResult.OK) {
				return;
			}
			string patchName="Setup.exe";
			string fileNameWithVers=betaAvailable;
			fileNameWithVers=fileNameWithVers.Replace("F","");
			fileNameWithVers=fileNameWithVers.Replace(".","_");
			fileNameWithVers="Setup_"+fileNameWithVers+".exe";
			string destDir=ImageStore.GetPreferredAtoZpath();
			string destPath2=null;
			if(destDir==null) {//Not using A to Z folders?
				destDir=PrefC.GetTempFolderPath();
			}
			else {
				destPath2=ODFileUtils.CombinePaths(destDir,"SetupFiles");
				if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ && !Directory.Exists(destPath2)) {
					Directory.CreateDirectory(destPath2);
				}
				else if(CloudStorage.IsCloudStorage) {
					destDir=PrefC.GetTempFolderPath();//Cloud storing needs it to be downloaded to a local temp folder
				}
				destPath2=ODFileUtils.CombinePaths(destPath2,fileNameWithVers);
			}
			DownloadInstallPatchFromURI(PrefC.GetString(PrefName.UpdateWebsitePath)+betaAvailableCode+"/"+patchName,//Source URI
				ODFileUtils.CombinePaths(destDir,patchName),
				true,true,
				destPath2);
		}

		///<summary>Downloads the build update MSI file and starts the install, closing OpenDental.</summary>
		private void butDownMsiBuild_Click(object sender,EventArgs e) {
			string fileName=PrefC.GetString(PrefName.UpdateWebsitePath)+buildAvailableCode+"/OpenDental.msi";
			Process.Start(fileName);
		}

		///<summary>Downloads the stable update MSI file and starts the install, closing OpenDental.</summary>
		private void butDownloadMsiStable_Click(object sender,EventArgs e) {
			string fileName=PrefC.GetString(PrefName.UpdateWebsitePath)+stableAvailableCode+"/OpenDental.msi";
			Process.Start(fileName);
		}

		///<summary>Downloads the beta update MSI file and starts the install, closing OpenDental.</summary>
		private void butDownloadMsiBeta_Click(object sender,EventArgs e) {
			string fileName=PrefC.GetString(PrefName.UpdateWebsitePath)+betaAvailableCode+"/OpenDental.msi";
			Process.Start(fileName);
		}

		private void butCheck_Click(object sender, System.EventArgs e) {
			Cursor=Cursors.WaitCursor;
			SavePrefs();
			CheckMain();
			//CheckClaimForm();
			Cursor=Cursors.Default;
		}

		private void CheckMain() {
			butDownload.Enabled=false;
			textResult.Text="";
			textResult2.Text="";
			if(textUpdateCode.Text.Length==0) {
				textResult.Text+=Lan.g(this,"Registration number not valid.");
				return;
			}
			string updateInfoMajor="";
			string updateInfoMinor="";
			butDownload.Enabled=ShouldDownloadUpdate(textWebsitePath.Text,textUpdateCode.Text,out updateInfoMajor,out updateInfoMinor);
			textResult.Text=updateInfoMajor;
			textResult2.Text=updateInfoMinor;
		}

		///<summary>Returns true if the download at the specified remoteUri with the given registration code should be downloaded and installed as an update, and false is returned otherwise. Also, information about the decision making process is stored in the updateInfoMajor and updateInfoMinor strings, but only holds significance to a human user.</summary>
		public static bool ShouldDownloadUpdate(string remoteUri,string updateCode,out string updateInfoMajor,out string updateInfoMinor){
			updateInfoMajor="";
			updateInfoMinor="";
			bool shouldDownload=false;
			string fileName="Manifest.txt";
			WebClient myWebClient=new WebClient();
			string myStringWebResource=remoteUri+updateCode+"/"+fileName;
			Version versionNewBuild=null;
			string strNewVersion="";
			string newBuild="";
			bool buildIsBeta=false;
			bool versionIsBeta=false;
			try{
				using(StreamReader sr=new StreamReader(myWebClient.OpenRead(myStringWebResource))){
					newBuild=sr.ReadLine();//must be be 3 or 4 components (revision is optional)
					strNewVersion=sr.ReadLine();//returns null if no second line
				}
				if(newBuild.EndsWith("b")){
					buildIsBeta=true;
					newBuild=newBuild.Replace("b","");
				}
				versionNewBuild=new Version(newBuild);
				if(versionNewBuild.Revision==-1){
					versionNewBuild=new Version(versionNewBuild.Major,versionNewBuild.Minor,versionNewBuild.Build,0);
				}
				if(strNewVersion!=null && strNewVersion.EndsWith("b")){
					versionIsBeta=true;
					strNewVersion=strNewVersion.Replace("b","");
				}
			}catch{
				updateInfoMajor+=Lan.g("FormUpdate","Registration number not valid, or internet connection failed.  ");
				return false;
			}
			if(versionNewBuild==new Version(Application.ProductVersion)){
				updateInfoMajor+=Lan.g("FormUpdate","You are using the most current build of this version.  ");
			}else{
				//this also allows users to install previous versions.
				updateInfoMajor+=Lan.g("FormUpdate","A new build of this version is available for download:  ")
					+versionNewBuild.ToString();
				if(buildIsBeta){
					updateInfoMajor+=Lan.g("FormUpdate","(beta)  ");
				}
				shouldDownload=true;
			}
			//Whether or not build is current, we want to inform user about the next minor version
			if(strNewVersion!=null){//we don't really care what it is.
				updateInfoMinor+=Lan.g("FormUpdate","A newer version is also available.  ");
				if(versionIsBeta) {//(checkNewBuild.Checked || checkNewVersion.Checked) && versionIsBeta){
					updateInfoMinor+=Lan.g("FormUpdate","It is beta (test), so it has some bugs and "+
						"you will need to update it frequently.  ");
				}
				updateInfoMinor+=Lan.g("FormUpdate","Contact us for a new Registration number if you wish to use it.  ");
			}
			return shouldDownload;
		}

		private void butDownload_Click(object sender, System.EventArgs e){
			string patchName="Setup.exe";
			string destDir=ImageStore.GetPreferredAtoZpath();
			if(destDir==null || CloudStorage.IsCloudStorage){
				destDir=PrefC.GetTempFolderPath();
			}
			DownloadInstallPatchFromURI(textWebsitePath.Text+textUpdateCode.Text+"/"+patchName,//Source URI
				ODFileUtils.CombinePaths(destDir,patchName),true,false,null);//Local destination file.
		}

		/// <summary>destinationPath includes filename (Setup.exe).  destinationPath2 will create a second copy at the specified path/filename, or it will be skipped if null or empty.</summary>
		public static void DownloadInstallPatchFromURI(string downloadUri,string destinationPath,bool runSetupAfterDownload,bool showShutdownWindow,string destinationPath2){
			string[] dblist=PrefC.GetString(PrefName.UpdateMultipleDatabases).Split(new string[] {","},StringSplitOptions.RemoveEmptyEntries);
			bool isShutdownWindowNeeded=showShutdownWindow;
			while(isShutdownWindowNeeded) {
				//Even if updating multiple databases, extra shutdown signals are not needed.
				FormShutdown FormSD=new FormShutdown();
				FormSD.IsUpdate=true;
				FormSD.ShowDialog();
				if(FormSD.DialogResult==DialogResult.OK) {
					//turn off signal reception for 5 seconds so this workstation will not shut down.
					Signalods.SignalLastRefreshed=MiscData.GetNowDateTime().AddSeconds(5);
					Signalod sig=new Signalod();
					sig.IType=InvalidType.ShutDownNow;
					Signalods.Insert(sig);
					Computers.ClearAllHeartBeats(Environment.MachineName);//always assume success
					isShutdownWindowNeeded=false;
					//SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Shutdown all workstations.");//can't do this because sometimes no user.
				}
				else if(FormSD.DialogResult==DialogResult.Cancel){//Cancel
					if(MsgBox.Show("FormUpdate",MsgBoxButtons.YesNo,"Are you sure you want to cancel the update?")) {
						return;
					}
					continue;
				}
				//no other workstation will be able to start up until this value is reset.
				Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName,Environment.MachineName);
			}
			MiscData.LockWorkstationsForDbs(dblist);//lock workstations for other db's.
			try {
				File.Delete(destinationPath);
			}
			catch(Exception ex) {
				FriendlyException.Show(Lan.g("FormUpdate","Error deleting file:")+"\r\n"+ex.Message,ex);
				MiscData.UnlockWorkstationsForDbs(dblist);//unlock workstations since nothing was actually done.
				return;
			}
			WebRequest wr=WebRequest.Create(downloadUri);
			WebResponse webResp=null;
			try{
				webResp=wr.GetResponse();
			}
			catch(Exception ex){
				CodeBase.MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(ex.Message+"\r\nUri: "+downloadUri);
				msgbox.ShowDialog();
				MiscData.UnlockWorkstationsForDbs(dblist);//unlock workstations since nothing was actually done.
				return;
			}
			int fileSize=(int)webResp.ContentLength/1024;
			FormProgress FormP=new FormProgress();
			//start the thread that will perform the download
			ThreadStart downloadDelegate= delegate { DownloadInstallPatchWorker(downloadUri,destinationPath,webResp.ContentLength,ref FormP); };
			Thread workerThread=new Thread(downloadDelegate);
			workerThread.Start();
			//display the progress dialog to the user:
			FormP.MaxVal=(double)fileSize/1024;
			FormP.NumberMultiplication=100;
			FormP.DisplayText="?currentVal MB of ?maxVal MB copied";
			FormP.NumberFormat="F";
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.Cancel) {
				workerThread.Abort();
				MiscData.UnlockWorkstationsForDbs(dblist);//unlock workstations since nothing was actually done.
				return;
			}
			//copy to second destination directory
			if(!CloudStorage.IsCloudStorage) {
				if(destinationPath2!=null && destinationPath2!="") {
					if(File.Exists(destinationPath2)) {
						try {
							File.Delete(destinationPath2);
						}
						catch(Exception ex) {
							FriendlyException.Show(Lan.g("FormUpdate","Error deleting file:")+"\r\n"+ex.Message,ex);
							MiscData.UnlockWorkstationsForDbs(dblist);//unlock workstations since nothing was actually done.
							return;
						}
					}
					File.Copy(destinationPath,destinationPath2);
				}
			}
			else {//Cloud storing
				OpenDentalCloud.Core.TaskStateUpload state=null;
				byte[] arrayBytes=File.ReadAllBytes(destinationPath);
				FormP=new FormProgress();
				FormP.DisplayText=Lan.g("FormUpdate","Uploading Setup File...");//Upload unversioned setup file to AtoZ main folder.
				FormP.NumberFormat="F";
				FormP.NumberMultiplication=1;
				FormP.MaxVal=100;//Doesn't matter what this value is as long as it is greater than 0
				FormP.TickMS=1000;
				state=CloudStorage.UploadAsync(
					CloudStorage.AtoZPath
					,Path.GetFileName(destinationPath)
					,arrayBytes
					,new OpenDentalCloud.ProgressHandler(FormP.OnProgress));
				if(FormP.ShowDialog()==DialogResult.Cancel) {
					state.DoCancel=true;
					MiscData.UnlockWorkstationsForDbs(dblist);//unlock workstations since nothing was actually done.
					return;
				}
				if(destinationPath2!=null && destinationPath2!="") {//Upload a copy of the Setup.exe to a versioned setup file to SetupFiles folder.  Not always used.
					FormP=new FormProgress();
					FormP.DisplayText=Lan.g("FormUpdate","Uploading Setup File SetupFiles folder...");
					FormP.NumberFormat="F";
					FormP.NumberMultiplication=1;
					FormP.MaxVal=100;//Doesn't matter what this value is as long as it is greater than 0
					FormP.TickMS=1000;
					state=CloudStorage.UploadAsync(
						ODFileUtils.CombinePaths(CloudStorage.AtoZPath,"SetupFiles")
						,Path.GetFileName(destinationPath2)
						,arrayBytes
						,new OpenDentalCloud.ProgressHandler(FormP.OnProgress));
					if(FormP.ShowDialog()==DialogResult.Cancel) {
						state.DoCancel=true;
						MiscData.UnlockWorkstationsForDbs(dblist);//unlock workstations since nothing was actually done.
						return;
					}
				}
			}
			//copy the Setup.exe to the AtoZ folders for the other db's.
			List<string> atozNameList=MiscData.GetAtoZforDb(dblist);
			for(int i=0;i<atozNameList.Count;i++) {
				if(destinationPath==Path.Combine(atozNameList[i],"Setup.exe")) {//if they are sharing an AtoZ folder.
					continue;
				}
				if(Directory.Exists(atozNameList[i])) {
					File.Copy(destinationPath,//copy the Setup.exe that was just downloaded to this AtoZ folder
						Path.Combine(atozNameList[i],"Setup.exe"),//to the other atozFolder
						true);//overwrite
				}
			}
			if(!runSetupAfterDownload) {
				return;
			}
			string msg=Lan.g("FormUpdate","Download succeeded.  Setup program will now begin.  When done, restart the program on this computer, then on the other computers.");
			if(dblist.Length > 0){
				msg="Download succeeded.  Setup file probably copied to other AtoZ folders as well.  Setup program will now begin.  When done, restart the program for each database on this computer, then on the other computers.";
			}
			if(MessageBox.Show(msg,"",MessageBoxButtons.OKCancel) !=DialogResult.OK){
				//Clicking cancel gives the user a chance to avoid running the setup program,
				Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName,"");//unlock workstations, since nothing was actually done.
				return;
			}
			#region Stop OpenDent Services
			//If the update has been initiated from the designated update server then try and stop all "OpenDent..." services.
			//They will be automatically restarted once Open Dental has successfully upgraded.
			if(PrefC.GetString(PrefName.WebServiceServerName)!="" && ODEnvironment.IdIsThisComputer(PrefC.GetString(PrefName.WebServiceServerName))) {
				Action actionCloseStopServicesProgress=ODProgressOld.ShowProgressStatus("StopServicesProgress",null,"Stopping services...");
				List<ServiceController> listOpenDentServices=ServicesHelper.GetAllOpenDentServices();
				//Newer versions of Windows have heightened security measures for managing services.
				//We get lots of calls where users do not have the correct permissions to start and stop Open Dental services.
				//Open Dental services are not important enough to warrent "Admin" rights to manage so we want to allow "Everyone" to start and stop them.
				ServicesHelper.SetSecurityDescriptorToAllowEveryoneToManageServices(listOpenDentServices);
				//Loop through all Open Dental services and stop them if they have not stopped or are not pending a stop so that their binaries can be updated.
				string servicesNotStopped=ServicesHelper.StopServices(listOpenDentServices);
				actionCloseStopServicesProgress();
				//Notify the user to go manually stop the services that could not automatically stop.
				if(!string.IsNullOrEmpty(servicesNotStopped)) {
					MsgBoxCopyPaste msgBCP=new MsgBoxCopyPaste(Lan.g("FormUpdate","The following services could not be stopped.  You need to manually stop them before continuing.")
					+"\r\n"+servicesNotStopped);
					msgBCP.ShowDialog();
				}
			}
			#endregion
			try {
				Process.Start(destinationPath);
				FormOpenDental.S_ProcessKillCommand();
			}
			catch{
				Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName,"");//unlock workstations, since nothing was actually done.
				MsgBox.Show(FormP,"Could not launch setup");
			}
		}

		///<summary>This is the function that the worker thread uses to actually perform the download.
		///Can also call this method in the ordinary way if the file to be transferred is short.</summary>
		private static void DownloadInstallPatchWorker(string downloadUri,string destinationPath,long contentLength,ref FormProgress progressIndicator) {
			using(WebClient webClient=new WebClient())
			using(Stream streamRead=webClient.OpenRead(downloadUri))
			using(FileStream fileStream=new FileStream(destinationPath,FileMode.Create))
			{
				int bytesRead;
				long position=0;
				byte[] buffer=new byte[10 * 1024];
				try {
					while((bytesRead=streamRead.Read(buffer,0,buffer.Length)) > 0) {
						position+=bytesRead;
						if(position!=contentLength) {
							progressIndicator.CurrentVal=((double)position / 1024) / 1024;
						}
						fileStream.Write(buffer,0,bytesRead);
					}
				}
				catch(Exception ex) {
					//Set the error message so that the user can call in and complain and we can get more information about what went wrong.
					//This error message will NOT show if the user hit the Cancel button and a random exception happened (because the window will have closed).
					progressIndicator.ErrorMessage=ex.Message;
				}
			}
			//If the file was successfully downloaded, set the progress indicator to maximum so that it closes the progress window.
			//Otherwise leave the window open so that the error message can be displayed to the user in red text.
			if(string.IsNullOrEmpty(progressIndicator.ErrorMessage)) {
				progressIndicator.CurrentVal=(double)contentLength / 1024;
			}
			else {//There was an unexpected error.
				try {
					File.Delete(destinationPath);//Try to clean up after ourselves.
				}
				catch(Exception ex) {
					ex.DoNothing();
				}
			}
		}

		private void SavePrefs(){
			bool changed=false;
			if(Prefs.UpdateString(PrefName.UpdateCode,textUpdateCode.Text)){
				changed=true;
			}
			if(Prefs.UpdateString(PrefName.UpdateWebsitePath,textWebsitePath.Text)){
				changed=true;
			}
			if(changed){
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}

		///<summary>No longer used here. Moved to FormAbout.</summary>
		private void butLicense_Click(object sender,EventArgs e) {
			FormLicense FormL=new FormLicense();
			FormL.ShowDialog();
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormUpdate_FormClosing(object sender,FormClosingEventArgs e) {
			if(Security.IsAuthorized(Permissions.Setup,DateTime.Now,true)	&& PrefC.GetBool(PrefName.UpdateWindowShowsClassicView)){
				SavePrefs();
			}
		}

	}

	
}





















