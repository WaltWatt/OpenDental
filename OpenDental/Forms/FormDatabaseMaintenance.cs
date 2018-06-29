using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	/// <summary>
	/// Summary description for FormCheckDatabase.
	/// </summary>
	public class FormDatabaseMaintenance:ODForm {
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.TextBox textBox1;
		private OpenDental.UI.Button butCheck;
		private System.ComponentModel.IContainer components;
		private System.Drawing.Printing.PrintDocument pd2;
		private CheckBox checkShow;
		private UI.Button butFix;
		private Label label6;
		private UI.Button butInnoDB;
		private Label label5;
		private Label labelApptProcs;
		private Label label3;
		private Label label2;
		private UI.Button butSpecChar;
		private UI.Button butApptProcs;
		private UI.Button butOptimize;
		private UI.Button butInsPayFix;
		private Label label7;
		private UI.Button butTokens;
		private OpenDental.UI.Button butPrint;
		private Label label8;
		private UI.Button butRemoveNulls;
		private ODGrid gridMain;
		private UI.Button butNone;
		///<summary>Holds any text from the log that still needs to be printed when the log spans multiple pages.</summary>
		private string LogTextPrint;
		private TextBox textBox2;
		///<summary>A list of every single DBM method in the database.  Filled on load right after "syncing" the DBM methods to the db.</summary>
		private List<DatabaseMaintenance> _listDatabaseMaintenances;
		///<summary>This is a filtered list of all methods from DatabaseMaintenances.cs that have the DbmMethod attribute.</summary>
		private List<MethodInfo> _listDbmMethods;
		///<summary>This is a filtered list of methods from DatabaseMaintenances.cs that have the DbmMethod attribute and are not hidden or old.  
		///This is used to populate gridMain.</summary>
		private List<MethodInfo> _listDbmMethodsGrid;
		///<summary>This is a filtered list of methods from DatabaseMaintenances.cs that have the DbmMethod attribute and are hidden.  
		///This is used to populate gridHidden.</summary>
		private List<MethodInfo> _listDbmMethodsGridHidden;
		///<summary>This is a filtered list of methods from DatabaseMaintenances.cs that have the DbmMethod attribute and are marked as old.  
		///This is used to populate gridOld.</summary>
		private List<MethodInfo> _listDbmMethodsGridOld;
		private Label label1;
		private UI.Button butEtrans;
		///<summary>Holds the date and time of the last time a Check or Fix was run.  Only used for printing.</summary>
		private DateTime _dateTimeLastRun;
		private Label label4;
		private UI.Button butActiveTPs;
		private TabControl tabControlDBM;
		private TabPage tabChecks;
		private TabPage tabTools;
		private Label label9;
		private UI.Button butRawEmails;
		private TextBox textBox3;
		private Label labelSkipCheckTable;
		private Label label10;
		private UI.Button butRecalcEst;
		private GroupBox groupBoxUpdateInProg;
		private Label labelUpdateInProgress;
		private TextBox textBoxUpdateInProg;
		private UI.Button butClearUpdateInProgress;
		private TabPage tabHidden;
		private TextBox textBox4;
		private ODGrid gridHidden;
		private TabPage tabOld;
		private TextBox textBox5;
		private ODGrid gridOld;
		private ContextMenuStrip contextMenuStrip1;
		private ToolStripMenuItem hideToolStripMenuItem;
		private ToolStripMenuItem unhideToolStripMenuItem;
		private UI.Button butFixOld;
		private UI.Button butCheckOld;
		private TextBox textNoneOld;
		private UI.Button butNoneOld;
		private UI.Button butSelectAll;
		private Label label11;
		private UI.Button butPayPlanPayments;
		private CheckBox checkShowHidden;

		///<summary>This bool keeps track of whether we need to invalidate cache for all users.</summary>
		private bool _isCacheInvalid; 

		///<summary></summary>
		public FormDatabaseMaintenance() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.C(this,new System.Windows.Forms.Control[]{
				this.textBox1,
				//this.textBox2
			});
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDatabaseMaintenance));
			this.butClose = new OpenDental.UI.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.butCheck = new OpenDental.UI.Button();
			this.pd2 = new System.Drawing.Printing.PrintDocument();
			this.checkShow = new System.Windows.Forms.CheckBox();
			this.butPrint = new OpenDental.UI.Button();
			this.butFix = new OpenDental.UI.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.butActiveTPs = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.butEtrans = new OpenDental.UI.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.butRemoveNulls = new OpenDental.UI.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.butTokens = new OpenDental.UI.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.butInnoDB = new OpenDental.UI.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.labelApptProcs = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.butSpecChar = new OpenDental.UI.Button();
			this.butApptProcs = new OpenDental.UI.Button();
			this.butOptimize = new OpenDental.UI.Button();
			this.butInsPayFix = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.unhideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.butNone = new OpenDental.UI.Button();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.tabControlDBM = new System.Windows.Forms.TabControl();
			this.tabChecks = new System.Windows.Forms.TabPage();
			this.tabHidden = new System.Windows.Forms.TabPage();
			this.butSelectAll = new OpenDental.UI.Button();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.gridHidden = new OpenDental.UI.ODGrid();
			this.tabOld = new System.Windows.Forms.TabPage();
			this.textNoneOld = new System.Windows.Forms.TextBox();
			this.butNoneOld = new OpenDental.UI.Button();
			this.butFixOld = new OpenDental.UI.Button();
			this.butCheckOld = new OpenDental.UI.Button();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.gridOld = new OpenDental.UI.ODGrid();
			this.tabTools = new System.Windows.Forms.TabPage();
			this.label11 = new System.Windows.Forms.Label();
			this.butPayPlanPayments = new OpenDental.UI.Button();
			this.groupBoxUpdateInProg = new System.Windows.Forms.GroupBox();
			this.labelUpdateInProgress = new System.Windows.Forms.Label();
			this.textBoxUpdateInProg = new System.Windows.Forms.TextBox();
			this.butClearUpdateInProgress = new OpenDental.UI.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.butRecalcEst = new OpenDental.UI.Button();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.butRawEmails = new OpenDental.UI.Button();
			this.labelSkipCheckTable = new System.Windows.Forms.Label();
			this.checkShowHidden = new System.Windows.Forms.CheckBox();
			this.contextMenuStrip1.SuspendLayout();
			this.tabControlDBM.SuspendLayout();
			this.tabChecks.SuspendLayout();
			this.tabHidden.SuspendLayout();
			this.tabOld.SuspendLayout();
			this.tabTools.SuspendLayout();
			this.groupBoxUpdateInProg.SuspendLayout();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(737, 587);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 1;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.SystemColors.Control;
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.Location = new System.Drawing.Point(6, 6);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(779, 40);
			this.textBox1.TabIndex = 1;
			this.textBox1.TabStop = false;
			this.textBox1.Text = "This tool will check the entire database for any improper settings, inconsistenci" +
    "es, or corruption.\r\nA log is automatically saved in RepairLog.txt if user has pe" +
    "rmission.";
			// 
			// butCheck
			// 
			this.butCheck.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCheck.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butCheck.Autosize = true;
			this.butCheck.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCheck.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCheck.CornerRadius = 4F;
			this.butCheck.Location = new System.Drawing.Point(301, 502);
			this.butCheck.Name = "butCheck";
			this.butCheck.Size = new System.Drawing.Size(75, 26);
			this.butCheck.TabIndex = 4;
			this.butCheck.Text = "C&heck";
			this.butCheck.Click += new System.EventHandler(this.butCheck_Click);
			// 
			// checkShow
			// 
			this.checkShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkShow.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShow.Location = new System.Drawing.Point(6, 447);
			this.checkShow.Name = "checkShow";
			this.checkShow.Size = new System.Drawing.Size(447, 20);
			this.checkShow.TabIndex = 1;
			this.checkShow.Text = "Show me everything in the log  (only for advanced users)";
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrint;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(6, 502);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(87, 26);
			this.butPrint.TabIndex = 3;
			this.butPrint.Text = "Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// butFix
			// 
			this.butFix.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butFix.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butFix.Autosize = true;
			this.butFix.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butFix.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butFix.CornerRadius = 4F;
			this.butFix.Location = new System.Drawing.Point(426, 502);
			this.butFix.Name = "butFix";
			this.butFix.Size = new System.Drawing.Size(75, 26);
			this.butFix.TabIndex = 5;
			this.butFix.Text = "&Fix";
			this.butFix.Click += new System.EventHandler(this.butFix_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(150, 409);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(631, 20);
			this.label4.TabIndex = 48;
			this.label4.Text = "Creates an active treatment plan for all pats with treatment planned procs.";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butActiveTPs
			// 
			this.butActiveTPs.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butActiveTPs.Autosize = true;
			this.butActiveTPs.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butActiveTPs.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butActiveTPs.CornerRadius = 4F;
			this.butActiveTPs.Location = new System.Drawing.Point(30, 405);
			this.butActiveTPs.Name = "butActiveTPs";
			this.butActiveTPs.Size = new System.Drawing.Size(114, 26);
			this.butActiveTPs.TabIndex = 8;
			this.butActiveTPs.Text = "Active TPs";
			this.butActiveTPs.Click += new System.EventHandler(this.butActiveTPs_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(150, 377);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(631, 20);
			this.label1.TabIndex = 46;
			this.label1.Text = "Clear out etrans entries older than a year old.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butEtrans
			// 
			this.butEtrans.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEtrans.Autosize = true;
			this.butEtrans.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEtrans.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEtrans.CornerRadius = 4F;
			this.butEtrans.Enabled = false;
			this.butEtrans.Location = new System.Drawing.Point(30, 373);
			this.butEtrans.Name = "butEtrans";
			this.butEtrans.Size = new System.Drawing.Size(114, 26);
			this.butEtrans.TabIndex = 7;
			this.butEtrans.Text = "Etrans";
			this.butEtrans.Click += new System.EventHandler(this.butEtrans_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(150, 345);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(631, 20);
			this.label8.TabIndex = 44;
			this.label8.Text = "Replace all null strings with empty strings.";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butRemoveNulls
			// 
			this.butRemoveNulls.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRemoveNulls.Autosize = true;
			this.butRemoveNulls.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRemoveNulls.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRemoveNulls.CornerRadius = 4F;
			this.butRemoveNulls.Location = new System.Drawing.Point(30, 341);
			this.butRemoveNulls.Name = "butRemoveNulls";
			this.butRemoveNulls.Size = new System.Drawing.Size(114, 26);
			this.butRemoveNulls.TabIndex = 6;
			this.butRemoveNulls.Text = "Remove Nulls";
			this.butRemoveNulls.Click += new System.EventHandler(this.butRemoveNulls_Click);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(150, 313);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(631, 20);
			this.label7.TabIndex = 42;
			this.label7.Text = "Validates tokens on file with the X-Charge server.";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butTokens
			// 
			this.butTokens.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butTokens.Autosize = true;
			this.butTokens.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTokens.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTokens.CornerRadius = 4F;
			this.butTokens.Location = new System.Drawing.Point(30, 309);
			this.butTokens.Name = "butTokens";
			this.butTokens.Size = new System.Drawing.Size(114, 26);
			this.butTokens.TabIndex = 5;
			this.butTokens.Text = "Tokens";
			this.butTokens.Click += new System.EventHandler(this.butTokens_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(150, 280);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(631, 20);
			this.label6.TabIndex = 40;
			this.label6.Text = "Converts database storage engine to/from InnoDb.";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butInnoDB
			// 
			this.butInnoDB.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butInnoDB.Autosize = true;
			this.butInnoDB.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butInnoDB.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butInnoDB.CornerRadius = 4F;
			this.butInnoDB.Location = new System.Drawing.Point(30, 277);
			this.butInnoDB.Name = "butInnoDB";
			this.butInnoDB.Size = new System.Drawing.Size(114, 26);
			this.butInnoDB.TabIndex = 4;
			this.butInnoDB.Text = "InnoDb";
			this.butInnoDB.Click += new System.EventHandler(this.butInnoDB_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(150, 248);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(631, 20);
			this.label5.TabIndex = 38;
			this.label5.Text = "Removes special characters from appt notes and appt proc descriptions.";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelApptProcs
			// 
			this.labelApptProcs.Location = new System.Drawing.Point(150, 216);
			this.labelApptProcs.Name = "labelApptProcs";
			this.labelApptProcs.Size = new System.Drawing.Size(631, 20);
			this.labelApptProcs.TabIndex = 37;
			this.labelApptProcs.Text = "Fixes procs in the Appt module that aren\'t correctly showing tooth nums.";
			this.labelApptProcs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(150, 184);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(631, 20);
			this.label3.TabIndex = 36;
			this.label3.Text = "Back up, optimize, and repair tables.";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(150, 152);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(631, 20);
			this.label2.TabIndex = 35;
			this.label2.Text = "Creates checks for insurance payments that are not attached to a check.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butSpecChar
			// 
			this.butSpecChar.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSpecChar.Autosize = true;
			this.butSpecChar.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSpecChar.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSpecChar.CornerRadius = 4F;
			this.butSpecChar.Location = new System.Drawing.Point(30, 245);
			this.butSpecChar.Name = "butSpecChar";
			this.butSpecChar.Size = new System.Drawing.Size(114, 26);
			this.butSpecChar.TabIndex = 3;
			this.butSpecChar.Text = "Spec Char";
			this.butSpecChar.Click += new System.EventHandler(this.butSpecChar_Click);
			// 
			// butApptProcs
			// 
			this.butApptProcs.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butApptProcs.Autosize = true;
			this.butApptProcs.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butApptProcs.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butApptProcs.CornerRadius = 4F;
			this.butApptProcs.Location = new System.Drawing.Point(30, 213);
			this.butApptProcs.Name = "butApptProcs";
			this.butApptProcs.Size = new System.Drawing.Size(114, 26);
			this.butApptProcs.TabIndex = 2;
			this.butApptProcs.Text = "Appt Procs";
			this.butApptProcs.Click += new System.EventHandler(this.butApptProcs_Click);
			// 
			// butOptimize
			// 
			this.butOptimize.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOptimize.Autosize = true;
			this.butOptimize.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOptimize.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOptimize.CornerRadius = 4F;
			this.butOptimize.Location = new System.Drawing.Point(30, 181);
			this.butOptimize.Name = "butOptimize";
			this.butOptimize.Size = new System.Drawing.Size(114, 26);
			this.butOptimize.TabIndex = 1;
			this.butOptimize.Text = "Optimize";
			this.butOptimize.Click += new System.EventHandler(this.butOptimize_Click);
			// 
			// butInsPayFix
			// 
			this.butInsPayFix.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butInsPayFix.Autosize = true;
			this.butInsPayFix.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butInsPayFix.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butInsPayFix.CornerRadius = 4F;
			this.butInsPayFix.Location = new System.Drawing.Point(30, 149);
			this.butInsPayFix.Name = "butInsPayFix";
			this.butInsPayFix.Size = new System.Drawing.Size(114, 26);
			this.butInsPayFix.TabIndex = 0;
			this.butInsPayFix.Text = "Ins Pay Fix";
			this.butInsPayFix.Click += new System.EventHandler(this.butInsPayFix_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.ContextMenuStrip = this.contextMenuStrip1;
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = true;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(6, 52);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(790, 389);
			this.gridMain.TabIndex = 0;
			this.gridMain.Title = "Database Checks";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableClaimPaySplits";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridMain_MouseUp);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hideToolStripMenuItem,
            this.unhideToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(113, 48);
			// 
			// hideToolStripMenuItem
			// 
			this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
			this.hideToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
			this.hideToolStripMenuItem.Text = "Hide";
			this.hideToolStripMenuItem.Click += new System.EventHandler(this.hideToolStripMenuItem_Click);
			// 
			// unhideToolStripMenuItem
			// 
			this.unhideToolStripMenuItem.Name = "unhideToolStripMenuItem";
			this.unhideToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
			this.unhideToolStripMenuItem.Text = "Unhide";
			this.unhideToolStripMenuItem.Click += new System.EventHandler(this.unhideToolStripMenuItem_Click);
			// 
			// butNone
			// 
			this.butNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butNone.Autosize = true;
			this.butNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNone.CornerRadius = 4F;
			this.butNone.Location = new System.Drawing.Point(721, 447);
			this.butNone.Name = "butNone";
			this.butNone.Size = new System.Drawing.Size(75, 26);
			this.butNone.TabIndex = 2;
			this.butNone.Text = "None";
			this.butNone.Click += new System.EventHandler(this.butNone_Click);
			// 
			// textBox2
			// 
			this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox2.BackColor = System.Drawing.SystemColors.Control;
			this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox2.Location = new System.Drawing.Point(350, 447);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(365, 26);
			this.textBox2.TabIndex = 99;
			this.textBox2.TabStop = false;
			this.textBox2.Text = "No selections will cause all database checks to run.\r\nOtherwise only selected che" +
    "cks will run.\r\n";
			this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// tabControlDBM
			// 
			this.tabControlDBM.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControlDBM.Controls.Add(this.tabChecks);
			this.tabControlDBM.Controls.Add(this.tabHidden);
			this.tabControlDBM.Controls.Add(this.tabOld);
			this.tabControlDBM.Controls.Add(this.tabTools);
			this.tabControlDBM.Location = new System.Drawing.Point(12, 12);
			this.tabControlDBM.Name = "tabControlDBM";
			this.tabControlDBM.SelectedIndex = 0;
			this.tabControlDBM.Size = new System.Drawing.Size(810, 560);
			this.tabControlDBM.TabIndex = 0;
			// 
			// tabChecks
			// 
			this.tabChecks.BackColor = System.Drawing.SystemColors.Control;
			this.tabChecks.Controls.Add(this.textBox1);
			this.tabChecks.Controls.Add(this.butFix);
			this.tabChecks.Controls.Add(this.butPrint);
			this.tabChecks.Controls.Add(this.textBox2);
			this.tabChecks.Controls.Add(this.butCheck);
			this.tabChecks.Controls.Add(this.checkShow);
			this.tabChecks.Controls.Add(this.butNone);
			this.tabChecks.Controls.Add(this.gridMain);
			this.tabChecks.Location = new System.Drawing.Point(4, 22);
			this.tabChecks.Name = "tabChecks";
			this.tabChecks.Padding = new System.Windows.Forms.Padding(3);
			this.tabChecks.Size = new System.Drawing.Size(802, 534);
			this.tabChecks.TabIndex = 0;
			this.tabChecks.Text = "Checks";
			// 
			// tabHidden
			// 
			this.tabHidden.BackColor = System.Drawing.Color.Transparent;
			this.tabHidden.Controls.Add(this.butSelectAll);
			this.tabHidden.Controls.Add(this.textBox4);
			this.tabHidden.Controls.Add(this.gridHidden);
			this.tabHidden.Location = new System.Drawing.Point(4, 22);
			this.tabHidden.Name = "tabHidden";
			this.tabHidden.Padding = new System.Windows.Forms.Padding(3);
			this.tabHidden.Size = new System.Drawing.Size(802, 534);
			this.tabHidden.TabIndex = 2;
			this.tabHidden.Text = "Hidden";
			// 
			// butSelectAll
			// 
			this.butSelectAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSelectAll.Autosize = true;
			this.butSelectAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSelectAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSelectAll.CornerRadius = 4F;
			this.butSelectAll.Location = new System.Drawing.Point(721, 447);
			this.butSelectAll.Name = "butSelectAll";
			this.butSelectAll.Size = new System.Drawing.Size(75, 26);
			this.butSelectAll.TabIndex = 101;
			this.butSelectAll.Text = "Select All";
			this.butSelectAll.Click += new System.EventHandler(this.butSelectAll_Click);
			// 
			// textBox4
			// 
			this.textBox4.BackColor = System.Drawing.SystemColors.Control;
			this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox4.Location = new System.Drawing.Point(6, 3);
			this.textBox4.Multiline = true;
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new System.Drawing.Size(779, 40);
			this.textBox4.TabIndex = 3;
			this.textBox4.TabStop = false;
			this.textBox4.Text = "This table shows all of the hidden database maintenance methods. You can unhide a" +
    " method by selecting a method, right clicking, and select Unhide.\r\n\r\n";
			// 
			// gridHidden
			// 
			this.gridHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridHidden.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridHidden.ContextMenuStrip = this.contextMenuStrip1;
			this.gridHidden.HasAddButton = false;
			this.gridHidden.HasDropDowns = false;
			this.gridHidden.HasMultilineHeaders = true;
			this.gridHidden.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridHidden.HeaderHeight = 15;
			this.gridHidden.HScrollVisible = false;
			this.gridHidden.Location = new System.Drawing.Point(6, 52);
			this.gridHidden.Name = "gridHidden";
			this.gridHidden.ScrollValue = 0;
			this.gridHidden.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridHidden.Size = new System.Drawing.Size(790, 389);
			this.gridHidden.TabIndex = 2;
			this.gridHidden.Title = "Hidden Methods";
			this.gridHidden.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridHidden.TitleHeight = 18;
			this.gridHidden.TranslationName = "TableHiddenDbmMethods";
			this.gridHidden.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridHidden_MouseUp);
			// 
			// tabOld
			// 
			this.tabOld.BackColor = System.Drawing.Color.Transparent;
			this.tabOld.Controls.Add(this.checkShowHidden);
			this.tabOld.Controls.Add(this.textNoneOld);
			this.tabOld.Controls.Add(this.butNoneOld);
			this.tabOld.Controls.Add(this.butFixOld);
			this.tabOld.Controls.Add(this.butCheckOld);
			this.tabOld.Controls.Add(this.textBox5);
			this.tabOld.Controls.Add(this.gridOld);
			this.tabOld.Location = new System.Drawing.Point(4, 22);
			this.tabOld.Name = "tabOld";
			this.tabOld.Padding = new System.Windows.Forms.Padding(3);
			this.tabOld.Size = new System.Drawing.Size(802, 534);
			this.tabOld.TabIndex = 3;
			this.tabOld.Text = "Old";
			// 
			// textNoneOld
			// 
			this.textNoneOld.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textNoneOld.BackColor = System.Drawing.SystemColors.Control;
			this.textNoneOld.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textNoneOld.Location = new System.Drawing.Point(350, 447);
			this.textNoneOld.Multiline = true;
			this.textNoneOld.Name = "textNoneOld";
			this.textNoneOld.Size = new System.Drawing.Size(365, 26);
			this.textNoneOld.TabIndex = 101;
			this.textNoneOld.TabStop = false;
			this.textNoneOld.Text = "No selections will cause all database checks to run.\r\nOtherwise only selected che" +
    "cks will run.\r\n";
			this.textNoneOld.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// butNoneOld
			// 
			this.butNoneOld.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNoneOld.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butNoneOld.Autosize = true;
			this.butNoneOld.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNoneOld.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNoneOld.CornerRadius = 4F;
			this.butNoneOld.Location = new System.Drawing.Point(721, 447);
			this.butNoneOld.Name = "butNoneOld";
			this.butNoneOld.Size = new System.Drawing.Size(75, 26);
			this.butNoneOld.TabIndex = 100;
			this.butNoneOld.Text = "None";
			this.butNoneOld.Click += new System.EventHandler(this.butNoneOld_Click);
			// 
			// butFixOld
			// 
			this.butFixOld.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butFixOld.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butFixOld.Autosize = true;
			this.butFixOld.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butFixOld.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butFixOld.CornerRadius = 4F;
			this.butFixOld.Location = new System.Drawing.Point(423, 502);
			this.butFixOld.Name = "butFixOld";
			this.butFixOld.Size = new System.Drawing.Size(75, 26);
			this.butFixOld.TabIndex = 7;
			this.butFixOld.Text = "&Fix";
			this.butFixOld.Click += new System.EventHandler(this.butFixOld_Click);
			// 
			// butCheckOld
			// 
			this.butCheckOld.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCheckOld.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butCheckOld.Autosize = true;
			this.butCheckOld.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCheckOld.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCheckOld.CornerRadius = 4F;
			this.butCheckOld.Location = new System.Drawing.Point(298, 502);
			this.butCheckOld.Name = "butCheckOld";
			this.butCheckOld.Size = new System.Drawing.Size(75, 26);
			this.butCheckOld.TabIndex = 6;
			this.butCheckOld.Text = "C&heck";
			this.butCheckOld.Click += new System.EventHandler(this.butCheckOld_Click);
			// 
			// textBox5
			// 
			this.textBox5.BackColor = System.Drawing.SystemColors.Control;
			this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox5.Location = new System.Drawing.Point(6, 3);
			this.textBox5.Multiline = true;
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new System.Drawing.Size(779, 40);
			this.textBox5.TabIndex = 5;
			this.textBox5.TabStop = false;
			this.textBox5.Text = "This table shows database maintenance methods that have been deemed no longer nec" +
    "essary by Open Dental. Should not be ran unless directly told to do so.\r\n\r\n";
			// 
			// gridOld
			// 
			this.gridOld.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridOld.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridOld.ContextMenuStrip = this.contextMenuStrip1;
			this.gridOld.HasAddButton = false;
			this.gridOld.HasDropDowns = false;
			this.gridOld.HasMultilineHeaders = true;
			this.gridOld.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridOld.HeaderHeight = 15;
			this.gridOld.HScrollVisible = false;
			this.gridOld.Location = new System.Drawing.Point(6, 52);
			this.gridOld.Name = "gridOld";
			this.gridOld.ScrollValue = 0;
			this.gridOld.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridOld.Size = new System.Drawing.Size(790, 389);
			this.gridOld.TabIndex = 4;
			this.gridOld.Title = "Old Methods";
			this.gridOld.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridOld.TitleHeight = 18;
			this.gridOld.TranslationName = "TableOldDbmMethods";
			this.gridOld.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridOld_MouseUp);
			// 
			// tabTools
			// 
			this.tabTools.BackColor = System.Drawing.SystemColors.Control;
			this.tabTools.Controls.Add(this.label11);
			this.tabTools.Controls.Add(this.butPayPlanPayments);
			this.tabTools.Controls.Add(this.groupBoxUpdateInProg);
			this.tabTools.Controls.Add(this.label10);
			this.tabTools.Controls.Add(this.butRecalcEst);
			this.tabTools.Controls.Add(this.textBox3);
			this.tabTools.Controls.Add(this.label9);
			this.tabTools.Controls.Add(this.butRawEmails);
			this.tabTools.Controls.Add(this.label4);
			this.tabTools.Controls.Add(this.butActiveTPs);
			this.tabTools.Controls.Add(this.butInsPayFix);
			this.tabTools.Controls.Add(this.label1);
			this.tabTools.Controls.Add(this.butOptimize);
			this.tabTools.Controls.Add(this.butEtrans);
			this.tabTools.Controls.Add(this.butApptProcs);
			this.tabTools.Controls.Add(this.label8);
			this.tabTools.Controls.Add(this.butSpecChar);
			this.tabTools.Controls.Add(this.butRemoveNulls);
			this.tabTools.Controls.Add(this.label2);
			this.tabTools.Controls.Add(this.label7);
			this.tabTools.Controls.Add(this.label3);
			this.tabTools.Controls.Add(this.butTokens);
			this.tabTools.Controls.Add(this.labelApptProcs);
			this.tabTools.Controls.Add(this.label6);
			this.tabTools.Controls.Add(this.label5);
			this.tabTools.Controls.Add(this.butInnoDB);
			this.tabTools.Location = new System.Drawing.Point(4, 22);
			this.tabTools.Name = "tabTools";
			this.tabTools.Padding = new System.Windows.Forms.Padding(3);
			this.tabTools.Size = new System.Drawing.Size(802, 534);
			this.tabTools.TabIndex = 1;
			this.tabTools.Text = "Tools";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(150, 504);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(631, 20);
			this.label11.TabIndex = 59;
			this.label11.Text = "Detaches patient payments attached to insurance payment plans and insurance payme" +
    "nts attached to patient payment plans.";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butPayPlanPayments
			// 
			this.butPayPlanPayments.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPayPlanPayments.Autosize = true;
			this.butPayPlanPayments.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPayPlanPayments.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPayPlanPayments.CornerRadius = 4F;
			this.butPayPlanPayments.Location = new System.Drawing.Point(30, 501);
			this.butPayPlanPayments.Name = "butPayPlanPayments";
			this.butPayPlanPayments.Size = new System.Drawing.Size(114, 26);
			this.butPayPlanPayments.TabIndex = 58;
			this.butPayPlanPayments.Text = "Pay Plan Payments";
			this.butPayPlanPayments.Click += new System.EventHandler(this.butPayPlanPayments_Click);
			// 
			// groupBoxUpdateInProg
			// 
			this.groupBoxUpdateInProg.Controls.Add(this.labelUpdateInProgress);
			this.groupBoxUpdateInProg.Controls.Add(this.textBoxUpdateInProg);
			this.groupBoxUpdateInProg.Controls.Add(this.butClearUpdateInProgress);
			this.groupBoxUpdateInProg.Location = new System.Drawing.Point(6, 8);
			this.groupBoxUpdateInProg.Name = "groupBoxUpdateInProg";
			this.groupBoxUpdateInProg.Size = new System.Drawing.Size(605, 78);
			this.groupBoxUpdateInProg.TabIndex = 57;
			this.groupBoxUpdateInProg.TabStop = false;
			this.groupBoxUpdateInProg.Text = "Update in progress on computer: ";
			// 
			// labelUpdateInProgress
			// 
			this.labelUpdateInProgress.Location = new System.Drawing.Point(21, 17);
			this.labelUpdateInProgress.Name = "labelUpdateInProgress";
			this.labelUpdateInProgress.Size = new System.Drawing.Size(578, 26);
			this.labelUpdateInProgress.TabIndex = 58;
			this.labelUpdateInProgress.Text = "Clear this value only if you are unable to start the program on other workstation" +
    "s and you are sure an update is not currently in progress.";
			// 
			// textBoxUpdateInProg
			// 
			this.textBoxUpdateInProg.Location = new System.Drawing.Point(24, 47);
			this.textBoxUpdateInProg.Name = "textBoxUpdateInProg";
			this.textBoxUpdateInProg.ReadOnly = true;
			this.textBoxUpdateInProg.Size = new System.Drawing.Size(149, 20);
			this.textBoxUpdateInProg.TabIndex = 55;
			// 
			// butClearUpdateInProgress
			// 
			this.butClearUpdateInProgress.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClearUpdateInProgress.Autosize = true;
			this.butClearUpdateInProgress.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClearUpdateInProgress.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClearUpdateInProgress.CornerRadius = 4F;
			this.butClearUpdateInProgress.Location = new System.Drawing.Point(179, 45);
			this.butClearUpdateInProgress.Name = "butClearUpdateInProgress";
			this.butClearUpdateInProgress.Size = new System.Drawing.Size(78, 23);
			this.butClearUpdateInProgress.TabIndex = 54;
			this.butClearUpdateInProgress.Text = "Clear";
			this.butClearUpdateInProgress.UseVisualStyleBackColor = true;
			this.butClearUpdateInProgress.Click += new System.EventHandler(this.butClearUpdateInProgress_Click);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(150, 472);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(631, 20);
			this.label10.TabIndex = 53;
			this.label10.Text = "Recalc estimates that are associated to non active coverage for the patient.";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butRecalcEst
			// 
			this.butRecalcEst.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRecalcEst.Autosize = true;
			this.butRecalcEst.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRecalcEst.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRecalcEst.CornerRadius = 4F;
			this.butRecalcEst.Location = new System.Drawing.Point(30, 469);
			this.butRecalcEst.Name = "butRecalcEst";
			this.butRecalcEst.Size = new System.Drawing.Size(114, 26);
			this.butRecalcEst.TabIndex = 52;
			this.butRecalcEst.Text = "Recalc Estimates";
			this.butRecalcEst.Click += new System.EventHandler(this.butRecalcEst_Click);
			// 
			// textBox3
			// 
			this.textBox3.BackColor = System.Drawing.SystemColors.Control;
			this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox3.Location = new System.Drawing.Point(6, 92);
			this.textBox3.Multiline = true;
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(790, 54);
			this.textBox3.TabIndex = 51;
			this.textBox3.TabStop = false;
			this.textBox3.Text = resources.GetString("textBox3.Text");
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(150, 441);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(631, 20);
			this.label9.TabIndex = 50;
			this.label9.Text = "Fixes emails which are encoded in the Chart progress notes.  Also clears unused a" +
    "ttachments.";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butRawEmails
			// 
			this.butRawEmails.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRawEmails.Autosize = true;
			this.butRawEmails.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRawEmails.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRawEmails.CornerRadius = 4F;
			this.butRawEmails.Location = new System.Drawing.Point(30, 437);
			this.butRawEmails.Name = "butRawEmails";
			this.butRawEmails.Size = new System.Drawing.Size(114, 26);
			this.butRawEmails.TabIndex = 9;
			this.butRawEmails.Text = "Raw Emails";
			this.butRawEmails.Click += new System.EventHandler(this.butRawEmails_Click);
			// 
			// labelSkipCheckTable
			// 
			this.labelSkipCheckTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelSkipCheckTable.ForeColor = System.Drawing.Color.Red;
			this.labelSkipCheckTable.Location = new System.Drawing.Point(587, 594);
			this.labelSkipCheckTable.Name = "labelSkipCheckTable";
			this.labelSkipCheckTable.Size = new System.Drawing.Size(144, 16);
			this.labelSkipCheckTable.TabIndex = 2;
			this.labelSkipCheckTable.Text = "Table check is disabled";
			this.labelSkipCheckTable.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.labelSkipCheckTable.Visible = false;
			// 
			// checkShowHidden
			// 
			this.checkShowHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkShowHidden.Location = new System.Drawing.Point(6, 514);
			this.checkShowHidden.Name = "checkShowHidden";
			this.checkShowHidden.Size = new System.Drawing.Size(134, 17);
			this.checkShowHidden.TabIndex = 103;
			this.checkShowHidden.Text = "Show Hidden";
			this.checkShowHidden.UseVisualStyleBackColor = true;
			this.checkShowHidden.CheckedChanged += new System.EventHandler(this.checkShowHidden_CheckedChanged);

			// 
			// FormDatabaseMaintenance
			// 
			this.AcceptButton = this.butCheck;
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(834, 625);
			this.Controls.Add(this.labelSkipCheckTable);
			this.Controls.Add(this.tabControlDBM);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(850, 458);
			this.Name = "FormDatabaseMaintenance";
			this.ShowInTaskbar = false;
			this.Text = "Database Maintenance";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDatabaseMaintenance_FormClosing);
			this.Load += new System.EventHandler(this.FormDatabaseMaintenance_Load);
			this.contextMenuStrip1.ResumeLayout(false);
			this.tabControlDBM.ResumeLayout(false);
			this.tabChecks.ResumeLayout(false);
			this.tabChecks.PerformLayout();
			this.tabHidden.ResumeLayout(false);
			this.tabHidden.PerformLayout();
			this.tabOld.ResumeLayout(false);
			this.tabOld.PerformLayout();
			this.tabTools.ResumeLayout(false);
			this.tabTools.PerformLayout();
			this.groupBoxUpdateInProg.ResumeLayout(false);
			this.groupBoxUpdateInProg.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormDatabaseMaintenance_Load(object sender,System.EventArgs e) {
			_listDbmMethods=DatabaseMaintenances.GetMethodsForDisplay(Clinics.ClinicNum);
			DatabaseMaintenances.InsertMissingDBMs(_listDbmMethods.Select(x => x.Name).ToList());
			_listDatabaseMaintenances=DatabaseMaintenances.GetAll();
			//Users get stopped from launching FormDatabaseMaintenance when they do not have the Setup permission.
			//Jordan wants some tools to only be accessible to users with the SecurityAdmin permission.
			if(Security.IsAuthorized(Permissions.SecurityAdmin,true)){
				butEtrans.Enabled=true;
			}
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				butApptProcs.Visible=false;
				labelApptProcs.Visible=false;
			}
			if(PrefC.GetBool(PrefName.DatabaseMaintenanceDisableOptimize)) {
				butOptimize.Enabled=false;
			}
			if(PrefC.GetBool(PrefName.DatabaseMaintenanceSkipCheckTable)) {
				labelSkipCheckTable.Visible=true;
			}
			FillGrid();
			FillGridHidden();
			FillGridOld();
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				butRemoveNulls.Visible=false;
			}
			textBoxUpdateInProg.Text=PrefC.GetString(PrefName.UpdateInProgressOnComputerName);
			if(string.IsNullOrWhiteSpace(textBoxUpdateInProg.Text)) {
				butClearUpdateInProgress.Enabled=false;
			}
		}

		private void FillGrid() {
			_listDbmMethodsGrid=GetDbmMethodsForGrid();
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"Name"),300));
			gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"Break\r\nDown"),40,HorizontalAlignment.Center));
			gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"Results"),0));
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listDbmMethodsGrid.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listDbmMethodsGrid[i].Name);
				row.Cells.Add(DatabaseMaintenances.MethodHasBreakDown(_listDbmMethodsGrid[i]) ? "X" : "");
				row.Cells.Add("");
				row.Tag=_listDbmMethodsGrid[i];
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void FillGridHidden() {
			_listDbmMethodsGridHidden=GetDbmMethodsForGrid(isHidden: true,isOld: false);
			gridHidden.BeginUpdate();
			gridHidden.Columns.Clear();
			gridHidden.Columns.Add(new ODGridColumn(Lan.g(this,"Name"),340));
			gridHidden.Rows.Clear();
			ODGridRow row;
			for(int i = 0;i<_listDbmMethodsGridHidden.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listDbmMethodsGridHidden[i].Name);
				row.Tag=_listDbmMethodsGridHidden[i];
				gridHidden.Rows.Add(row);
			}
			gridHidden.EndUpdate();
		}

		private void FillGridOld() {
			_listDbmMethodsGridOld=GetDbmMethodsForGrid(isHidden: false,isOld: true);
			_listDbmMethodsGridOld.AddRange(GetDbmMethodsForGrid(isHidden: true,isOld: true));
			_listDbmMethodsGridOld.Sort(new MethodInfoComparer());
			gridOld.BeginUpdate();
			gridOld.Columns.Clear();
			gridOld.Columns.Add(new ODGridColumn(Lan.g(this,"Name"),300));
			if(checkShowHidden.Checked) {
				gridOld.Columns.Add(new ODGridColumn(Lan.g(this,"Hidden"),45,HorizontalAlignment.Center));
			}
			gridOld.Columns.Add(new ODGridColumn(Lan.g(this,"Break\r\nDown"),40,HorizontalAlignment.Center));
			gridOld.Columns.Add(new ODGridColumn(Lan.g(this,"Results"),0));
			gridOld.Rows.Clear();
			ODGridRow row;
			for(int i = 0;i<_listDbmMethodsGridOld.Count;i++) {
				bool isMethodHidden=_listDatabaseMaintenances.Any(x => x.MethodName==_listDbmMethodsGridOld[i].Name && x.IsHidden);
				if(!checkShowHidden.Checked && isMethodHidden) {
					continue;
				}
				row=new ODGridRow();
				row.Cells.Add(_listDbmMethodsGridOld[i].Name);
				if(checkShowHidden.Checked) {
					row.Cells.Add(isMethodHidden ? "X" : "");
				}
				row.Cells.Add(DatabaseMaintenances.MethodHasBreakDown(_listDbmMethodsGridOld[i]) ? "X" : "");
				row.Cells.Add("");
				row.Tag=_listDbmMethodsGridOld[i];
				gridOld.Rows.Add(row);
			}
			gridOld.EndUpdate();
		}

		private List<MethodInfo> GetDbmMethodsForGrid(bool isHidden=false,bool isOld=false) {
			List<string> listMethods=_listDatabaseMaintenances.FindAll(x => x.IsHidden==isHidden && x.IsOld==isOld)
				.Select(y => y.MethodName).ToList();
			return _listDbmMethods.FindAll(x => x.Name.In(listMethods));
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!DatabaseMaintenances.MethodHasBreakDown(_listDbmMethodsGrid[e.Row])) {
				return;
			}
			//We know that this method supports giving the user a break down and shall call the method's fix section where the break down results should be.
			//TODO: Make sure that DBM methods with break downs ALWAYS have the break down in the fix section.
			long userNum=0;
			long patNum=0;
			DbmMethodAttr methodAttributes=(DbmMethodAttr)Attribute.GetCustomAttribute(_listDbmMethodsGrid[e.Row],typeof(DbmMethodAttr));
			//We always send verbose and modeCur into all DBM methods.
			List<object> parameters=new List<object>() { checkShow.Checked,DbmMode.Breakdown };
			//There are optional paramaters available to some methods and adding them in the following order is very important.
			if(methodAttributes.HasUserNum) {
				parameters.Add(userNum);
			}
			if(methodAttributes.HasPatNum) {
				parameters.Add(patNum);
			}
			Cursor=Cursors.WaitCursor;
			string result=(string)_listDbmMethodsGrid[e.Row].Invoke(null,parameters.ToArray());
			if(result=="") {//Only possible if running a check / fix in non-verbose mode and nothing happened or needs to happen.
				result=Lan.g("FormDatabaseMaintenance","Done.  No maintenance needed.");
			}
			//Show the result of the dbm method in a simple copy paste msg box.
			MsgBoxCopyPaste msgBoxCP=new MsgBoxCopyPaste(result);
			Cursor=Cursors.Default;
			msgBoxCP.Show();//Let this window be non-modal so that they can keep it open while they fix their problems.
		}

		private void gridMain_MouseUp(object sender,MouseEventArgs e) {
			OnMouseUp(e,gridMain);
		}

		private void gridHidden_MouseUp(object sender,MouseEventArgs e) {
			OnMouseUp(e,gridHidden);
		}

		private void gridOld_MouseUp(object sender,MouseEventArgs e) {
			OnMouseUp(e,gridOld);
		}

		private void OnMouseUp(MouseEventArgs e,ODGrid grid) {
			if(grid.SelectedIndices.Length==0 || e.Button!=MouseButtons.Right) {
				contextMenuStrip1.Hide();
				return;
			}
			MethodInfo method=(MethodInfo)grid.Rows[grid.SelectedIndices[0]].Tag;
			if(method!=null) {
				bool isMethodHidden=_listDatabaseMaintenances.Any(x => x.MethodName==method.Name && x.IsHidden);
				hideToolStripMenuItem.Visible=!isMethodHidden;
				unhideToolStripMenuItem.Visible=isMethodHidden;
			}
		}

		private void hideToolStripMenuItem_Click(object sender,EventArgs e) {
			//Users can only hide DBM methods from gridMain or gridOld.
			switch(tabControlDBM.SelectedIndex) {
				case 0://tabChecks
					UpdateDbmIsHiddenForGrid(gridMain,true);
					break;
				case 2://tabOld
					UpdateDbmIsHiddenForGrid(gridOld,true);
					break;
				case 1://tabHidden
				case 3://tabTools
				default:
					return;
			}
		}

		private void unhideToolStripMenuItem_Click(object sender,EventArgs e) {
			//Users can only unhide DBM methods from gridHidden or gridOld.
			switch(tabControlDBM.SelectedIndex) {
				case 1://tabHidden
					UpdateDbmIsHiddenForGrid(gridHidden,false);
					break;
				case 2://tabOld
					UpdateDbmIsHiddenForGrid(gridOld,false);
					break;
				case 0://tabChecks
				case 3://tabTools
				default:
					return;
			}
		}

		private void UpdateDbmIsHiddenForGrid(ODGrid grid,bool isHidden) {
			for(int i=0;i<grid.SelectedIndices.Length;i++) {
				MethodInfo method=(MethodInfo)grid.Rows[grid.SelectedIndices[i]].Tag;
				DatabaseMaintenance dbm=_listDatabaseMaintenances.FirstOrDefault(x=>x.MethodName==method.Name);
				if(dbm==null) {
					continue;
				}
				dbm.IsHidden=isHidden;
				DatabaseMaintenances.Update(dbm);
			}
			_listDatabaseMaintenances=DatabaseMaintenances.GetAll();
			FillGrid();
			FillGridHidden();
			FillGridOld();
		}

		private void butNone_Click(object sender,EventArgs e) {
			gridMain.SetSelected(false);
		}

		private void butNoneOld_Click(object sender,EventArgs e) {
			gridOld.SetSelected(false);
		}

		private void butSelectAll_Click(object sender,EventArgs e) {
			gridHidden.SetSelected(true);
		}

		#region Database Tools

		private void butClearUpdateInProgress_Click(object sender,EventArgs e) {
			Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName,"");
			DataValid.SetInvalid(InvalidType.Prefs);
			textBoxUpdateInProg.Text="";
		}

		private void butInsPayFix_Click(object sender,EventArgs e) {
			FormInsPayFix formIns=new FormInsPayFix();
			formIns.ShowDialog();
		}

		private void butOptimize_Click(object sender,EventArgs e) {
			if(MessageBox.Show(Lan.g("FormDatabaseMaintenance","This tool will backup, optimize, and repair all tables.")+"\r\n"+Lan.g("FormDatabaseMaintenance","Continue?")
				,Lan.g("FormDatabaseMaintenance","Backup Optimize Repair")
				,MessageBoxButtons.OKCancel)!=DialogResult.OK) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			string result="";
			if(Shared.BackupRepairAndOptimize(true,BackupLocation.OptimizeTool)) {
				result=DateTime.Now.ToString()+"\r\n"+Lan.g("FormDatabaseMaintenance","Repair and Optimization Complete");
			}
			else {
				result=DateTime.Now.ToString()+"\r\n";
				result+=Lan.g("FormDatabaseMaintenance","Backup, repair, or optimize has failed.  Your database has not been altered.")+"\r\n";
				result+=Lan.g("FormDatabaseMaintenance","Please call support for help, a manual backup of your data must be made before trying to fix your database.")+"\r\n";
			}
			Cursor=Cursors.Default;
			MsgBoxCopyPaste msgBoxCP=new MsgBoxCopyPaste(result);
			msgBoxCP.Show();//Let this window be non-modal so that they can keep it open while they fix their problems.
			try {
				DatabaseMaintenances.SaveLogToFile(result);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void butApptProcs_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will fix procedure descriptions in the Appt module that aren't correctly showing tooth numbers.\r\nContinue?")) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			Appointments.UpdateProcDescriptForAppts(Appointments.GetForPeriod(DateTime.Now.Date.AddMonths(-6),DateTime.MaxValue.AddDays(-10)).ToList());
			Cursor=Cursors.Default;
			MsgBox.Show(this,"Done. Please refresh Appt module to see the changes.");
		}

		private void butSpecChar_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This is only used if your mobile synch or middle tier is failing.  This cannot be undone.  Do you wish to continue?")) {
				return;
			}
			InputBox box=new InputBox("In our online manual, on the database maintenance page, look for the password and enter it below.");
			if(box.ShowDialog()!=DialogResult.OK) {
				return;
			}
			if(box.textResult.Text!="fix") {
				MessageBox.Show("Wrong password.");
				return;
			}
			DatabaseMaintenances.FixSpecialCharacters();
			MsgBox.Show(this,"Done.");
			_isCacheInvalid=true;//Definitions are cached and could have been changed from above DBM.
		}

		private void butInnoDB_Click(object sender,EventArgs e) {
			FormInnoDb form=new FormInnoDb();
			form.ShowDialog();
		}

		private void butTokens_Click(object sender,EventArgs e) {
			FormXchargeTokenTool FormXCT=new FormXchargeTokenTool();
			FormXCT.ShowDialog();
		}

		private void butRemoveNulls_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will replace ALL null strings in your database with empty strings.  This cannot be undone.  Do you wish to continue?")) {
				return;
			}
			MessageBox.Show(Lan.g(this,"Number of null strings replaced with empty strings")+": "+DatabaseMaintenances.MySqlRemoveNullStrings());
			_isCacheInvalid=true;//The above DBM could have potentially changed cached tables. 
		}

		private void butEtrans_Click(object sender,EventArgs e) {
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				MsgBox.Show(this,"Tool does not currently support Oracle.  Please call support to see if you need this fix.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will clear out etrans message text entries over a year old.  An automatic backup of the database will be created before deleting any entries.  This process may take a while to run depending on the size of your database.  Continue?")) {
				return;
			}
#if !DEBUG
			if(!Shared.MakeABackup(BackupLocation.DatabaseMaintenanceTool)) {
				MsgBox.Show(this,"Etrans message text entries were not altered.  Please try again.");
				return;
			}
#endif
			DatabaseMaintenances.ClearOldEtransMessageText();
			MsgBox.Show(this,"Etrans message text entries over a year old removed");
		}

		private void butActiveTPs_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			List<Procedure> listTpTpiProcs=DatabaseMaintenances.GetProcsNoActiveTp();
			Cursor=Cursors.Default;
			if(listTpTpiProcs.Count==0) {
				MsgBox.Show(this,"Done");
				return;
			}
			int numTPs=listTpTpiProcs.Select(x => x.PatNum).Distinct().ToList().Count;
			int numTPAs=listTpTpiProcs.Count;
			TimeSpan estRuntime=TimeSpan.FromSeconds((numTPs+numTPAs)*0.001d);
			//the factor 0.001 was determined by running tests on a large db
			//212631 TPAs and 30000 TPs were inserted in 225 seconds
			//225/(212631+30000)=0.0009273341 seconds per inserted row (rounded up to 0.001 seconds)
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"From your database size we estimate that this could take "+(estRuntime.Minutes+1)+" minutes to create "
				+numTPs+" treatment plans for "+numTPAs+" procedures if running form the server.\r\nDo you wish to continue?"))
			{
				return;
			}
			Cursor=Cursors.WaitCursor;
			string msg=DatabaseMaintenances.CreateMissingActiveTPs(listTpTpiProcs);
			Cursor=Cursors.Default;
			if(string.IsNullOrEmpty(msg)) {
				msg="Done";
			}
			MsgBox.Show(this,msg);
		}

		private void butRawEmails_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo
				,"This tool is only necessary to run if utilizing the email inbox feature.\r\n"
				+"Run this tool if email messages are encoded in the Chart progress notes, \r\n"
				+"or if the emailmessage table has grown to a large size.\r\n"
				+"This will decode any encoded clear text emails and will remove unused attachment content.\r\n\r\n"
				+"This tool could take a long time to finish, do you wish to continue?"))
			{
				return;
			}
			//Create a new thread that will show a progress window (takes a while even if no clean up needed) so the user know work is being done.
			Action actionCloseEmailProgress=ODProgressOld.ShowProgressStatus("RawEmailCleanUp",currentForm:this);
			string results="";
			try {
				results=DatabaseMaintenances.CleanUpRawEmails();
			}
			catch(Exception ex) {
				results=Lan.g(this,"There was an error cleaning up email bloat:")+"\r\n"+ex.Message;
			}
			finally {
				actionCloseEmailProgress();
			}
			MessageBox.Show(results);
		}

		private void butRecalcEst_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo
				,"This tool will mimic what happens when you click OK in the procedure edit window.  "
				+"The tool will identify patients with at least one estimate which belongs to a dropped insurance plan.  "
				+"For each such patient, estimates will be recalculated for current plans, and  "
				+"for plans which have been dropped, estimates associated to the dropped plans will be deleted.\r\n"
				+"This tool could take a long time to finish, do you wish to continue?"))
			{
				return;
			}
			//Create a new thread that will show a progress window (takes a while even if no clean up needed) so the user know work is being done.
			Action actionCloseInvalidEstProgress=ODProgressOld.ShowProgressStatus("Recalc Estimates",currentForm:this);
			DatabaseMaintenances.RecalcEstimates(Procedures.GetProcsWithOldEstimates());
			actionCloseInvalidEstProgress();
		}
		
		private void butPayPlanPayments_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo
				,"You are running a tool to detach patient payments from insurance payment plans and detach insurance payments from patient payment plans.  "
				+"The payments will still exist, and because they will now be reflected in the account instead of the payment plan, historical and "
				+"current account balances may change.  Proceed?"))
			{
				return;
			}
			//Create a new thread that will show a progress window (takes a while even if no clean up needed) so the user know work is being done.
			Action actionCloseInvalidEstProgress=ODProgressOld.ShowProgressStatus("Detaching payment plan payments",currentForm:this);
			string results=DatabaseMaintenances.DetachInvalidPaymentPlanPayments();
			actionCloseInvalidEstProgress();
			MsgBoxCopyPaste msgBoxCopyPaste=new MsgBoxCopyPaste(results);
			msgBoxCopyPaste.ShowInTaskbar=true;
			msgBoxCopyPaste.Text=Lan.g(this,"Payments Fixed");
			msgBoxCopyPaste.Show();
		}

		#endregion

		private void butTemp_Click(object sender,EventArgs e) {
			FormDatabaseMaintTemp form=new FormDatabaseMaintTemp();
			form.ShowDialog();
		}

		private void Run(ODGrid grid,DbmMode modeCur,bool isOld) {
			Cursor=Cursors.WaitCursor;
			if(grid.Rows.Count > 0 && grid.Columns.Count < 3) {//Enforce the requirement of having the Results column as the third column.
				MsgBox.Show(this,"Must have at least three columns in the grid.");
				return;
			}
			int colresults=2;
			if(grid==gridOld && checkShowHidden.Checked) {
				colresults=3;//There is an extra "Hidden" column to account for when setting the "Results" column.
			}
			//Clear out the result column for all rows before every "run"
			for(int i=0;i<grid.Rows.Count;i++) {
				grid.Rows[i].Cells[colresults].Text="";//Don't use UpdateResultTextForRow here because users will see the rows clearing out one by one.
			}
			bool verbose=checkShow.Checked;
			StringBuilder logText=new StringBuilder();
			//Create a thread that will show a window and then stay open until the closing phrase is thrown from this form.
			Action actionCloseCheckTableProgress=ODProgressOld.ShowProgressStatus("CheckTableProgress",this);
			ODTuple<string,bool> tableCheckResult=DatabaseMaintenances.MySQLTables(verbose,modeCur);
			actionCloseCheckTableProgress();
			logText.Append(tableCheckResult.Item1);
			//No database maintenance methods should be run unless this passes.
			if(!tableCheckResult.Item2) {
				Cursor=Cursors.Default;
				MsgBoxCopyPaste msgBoxCP=new MsgBoxCopyPaste(tableCheckResult.Item1);//tableCheckResult is already translated.
				msgBoxCP.Show();//Let this window be non-modal so that they can keep it open while they fix their problems.
				return;
			}
			string result;
			if(grid.SelectedIndices.Length < 1) {
				//No rows are selected so the user wants to run all checks.
				grid.SetSelected(true);
			}
			int[] selectedIndices=grid.SelectedIndices;
			for(int i=0;i<selectedIndices.Length;i++) {
				long userNum=0;
				long patNum=0;
				MethodInfo method;
				if(isOld) {
					method=_listDbmMethodsGridOld[selectedIndices[i]];
				}
				else {
					method=_listDbmMethodsGrid[selectedIndices[i]];
				}
				DbmMethodAttr methodAttributes=(DbmMethodAttr)Attribute.GetCustomAttribute(method,typeof(DbmMethodAttr));
				//We always send verbose and modeCur into all DBM methods.
				List<object> parameters=new List<object>() { verbose,modeCur };
				//There are optional paramaters available to some methods and adding them in the following order is very important.
				if(methodAttributes.HasUserNum) {
					parameters.Add(userNum);
				}
				if(methodAttributes.HasPatNum) {
					parameters.Add(patNum);
				}
				grid.ScrollToIndexBottom(selectedIndices[i]);
				UpdateResultTextForRow(grid,method,selectedIndices[i],Lan.g("FormDatabaseMaintenance","Running")+"...");
				try {
					result=(string)method.Invoke(null,parameters.ToArray());
					if(modeCur==DbmMode.Fix) {
						DatabaseMaintenances.UpdateDateLastRun(method.Name);
					}
				}
				catch(Exception ex) {
					if(ex.InnerException!=null) {
						ExceptionDispatchInfo.Capture(ex.InnerException).Throw();//This preserves the stack trace of the InnerException.
					}
					throw;
				}
				string status="";
				if(result=="") {//Only possible if running a check / fix in non-verbose mode and nothing happened or needs to happen.
					status=Lan.g("FormDatabaseMaintenance","Done.  No maintenance needed.");
				}
				UpdateResultTextForRow(grid,method,selectedIndices[i],result+status);
				logText.Append(result);
			}
			grid.SetSelected(selectedIndices,true);//Reselect all rows that were originally selected.
			try {
				DatabaseMaintenances.SaveLogToFile(logText.ToString());
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MessageBox.Show(ex.Message);
			}
			Cursor=Cursors.Default;
		}

		/// <summary>Updates the result column for the specified row in gridMain with the text passed in.</summary>
		private void UpdateResultTextForRow(ODGrid grid,MethodInfo method,int index,string text) {
			int colresults=2;
			if(grid==gridOld && checkShowHidden.Checked) {
				colresults=3;//There is an extra "Hidden" column to account for when setting the "Results" column.
			}
			grid.BeginUpdate();
			//Checks to see if it has a breakdown, and if it needs any maintenenece to decide whether or not to apply the "X"
			if(!DatabaseMaintenances.MethodHasBreakDown(method) || text == "Done.  No maintenance needed.") {
				grid.Rows[index].Cells[colresults-1].Text="";
			}
			else {
				grid.Rows[index].Cells[colresults-1].Text="X";
			}
			grid.Rows[index].Cells[colresults].Text=text;
			grid.EndUpdate();
			Application.DoEvents();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			if(_dateTimeLastRun==DateTime.MinValue) {
				_dateTimeLastRun=DateTime.Now;
			}
			StringBuilder strB=new StringBuilder();
			strB.Append(_dateTimeLastRun.ToString());
			strB.Append('-',65);
			strB.AppendLine();//New line.
			if(gridMain.SelectedIndices.Length < 1) {
				//No rows are selected so the user wants to run all checks.
				gridMain.SetSelected(true);
			}
			int[] selectedIndices=gridMain.SelectedIndices;
			for(int i=0;i<selectedIndices.Length;i++) {
				string resultText=gridMain.Rows[selectedIndices[i]].Cells[2].Text;
				if(!String.IsNullOrEmpty(resultText) && resultText!="Done.  No maintenance needed.") {
					strB.Append(gridMain.Rows[selectedIndices[i]].Cells[0].Text+"\r\n");
					strB.Append("---"+gridMain.Rows[selectedIndices[i]].Cells[2].Text+"\r\n");
					strB.AppendLine();
				}
			}
			strB.AppendLine(Lan.g("FormDatabaseMaintenance","Done"));
			LogTextPrint=strB.ToString();
			pd2 = new PrintDocument();
			pd2.PrintPage += new PrintPageEventHandler(this.pd2_PrintPage);
			pd2.DefaultPageSettings.Margins=new Margins(40,50,50,60);
			try {
#if DEBUG
				FormPrintPreview printPreview=new FormPrintPreview(PrintSituation.Default,pd2,0,0,"Database Maintenance log printed");
				printPreview.ShowDialog();
#else
				pd2.Print();
#endif
			}
			catch {
				MessageBox.Show("Printer not available");
			}
		}

		private void pd2_PrintPage(object sender,PrintPageEventArgs ev) {//raised for each page to be printed.
			int charsOnPage=0;
			int linesPerPage=0;
			Font font=new Font("Courier New",10);
			ev.Graphics.MeasureString(LogTextPrint,font,ev.MarginBounds.Size,StringFormat.GenericTypographic,out charsOnPage,out linesPerPage);
			ev.Graphics.DrawString(LogTextPrint,font,Brushes.Black,ev.MarginBounds,StringFormat.GenericTypographic);
			LogTextPrint=LogTextPrint.Substring(charsOnPage);
			ev.HasMorePages=(LogTextPrint.Length > 0);
		}

		private void checkShowHidden_CheckedChanged(object sender,EventArgs e) {
			FillGridOld();
		}

		private void butCheck_Click(object sender,System.EventArgs e) {
			Run(gridMain,DbmMode.Check,false);
		}

		private void butCheckOld_Click(object sender,EventArgs e) {
			Run(gridOld,DbmMode.Check,true);
		}

		private void butFix_Click(object sender,EventArgs e) {
			Fix();
		}

		private void butFixOld_Click(object sender,EventArgs e) {
			Fix(isOld:true);
		}

		private void Fix(bool isOld=false) {
			List<string> runningComps=Computers.GetRunningComputers();
			if(runningComps.Count>50) {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"WARNING!\r\nMore than 50 workstations are connected to this database. "
					+"Running DBM may cause severe network slowness. "
					+"We recommend running this tool when fewer users are connected (possibly after working hours). \r\n\r\n"
					+"Continue?")) {
					return;
				}
			}
			if(isOld) {
				Run(gridOld,DbmMode.Fix,true);
			}
			else {
				Run(gridMain,DbmMode.Fix,false);
			}
			_isCacheInvalid=true;//Flag cache to be invalidated on closing.  Some DBM fixes alter cached tables.
		}

		private void butClose_Click(object sender,System.EventArgs e) {
			Close();
		}

		private void FormDatabaseMaintenance_FormClosing(object sender,FormClosingEventArgs e) {
			if(_isCacheInvalid) {
				Action actionCloseDBM=ODProgressOld.ShowProgressStatus("DatabaseMaintEvent",this,Lan.g(this,"Refreshing all caches, this can take a while..."));
				//Invalidate all cached tables.  DBM could have touched anything so blast them all.  
				//Failure to invalidate cache can cause UEs in the main program.
				DataValid.SetInvalid(Cache.GetAllCachedInvalidTypes().ToArray());
				actionCloseDBM?.Invoke();
			}
		}
	}


}
