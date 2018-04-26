using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormApptViewEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butDelete;
		private System.Windows.Forms.Label labelOps;
		private System.Windows.Forms.ListBox listOps;
		private System.Windows.Forms.ListBox listProv;
		private System.Windows.Forms.Label label2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textDescription;
		private OpenDental.UI.Button butDown;
		private OpenDental.UI.Button butUp;
		private OpenDental.UI.Button butLeft;
		private OpenDental.UI.Button butRight;
		///<summary></summary>
		public bool IsNew;
		///<summary>A collection of strings of all available element descriptions.  This memory list is in English and is stored in db.  Items from this list get translated before display on in this window.</summary>
		private List<String> elementsAll;
		/// <summary>A list of indices to elementsAll.  Those elements which are showing in the list of available elements.  It must be done this way to support language translation.</summary>
		private List<int> displayedAvailable;
		///<summary>The actual ApptFieldDefNums of all available elements because no language translation is needed.</summary>
		private List<long> displayedAvailableApptFieldDefs;
		///<summary>The actual PatFieldDefNums of all available elements because no language translation is needed.</summary>
		private List<long> _listPatFieldDefNums;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textRowsPerIncr;
		///<summary>A local list of ApptViewItems which are displayed in all three lists on the right.  Not updated to db until the form is closed.</summary>
		private List<ApptViewItem> displayedElementsAll;
		private List<ApptViewItem> displayedElementsMain;
		private List<ApptViewItem> displayedElementsUR;
		private List<ApptViewItem> displayedElementsLR;
		private CheckBox checkOnlyScheduledProvs;
		private TextBox textBeforeTime;
		private GroupBox groupBox1;
		private Label labelBeforeTime;
		private Label labelAfterTime;
		private TextBox textAfterTime;
		private GroupBox groupBox2;
		private Label label8;
		private OpenDental.UI.ODGrid gridLR;
		private OpenDental.UI.ODGrid gridUR;
		private OpenDental.UI.ODGrid gridMain;
		private ODGrid gridAvailable;
		private ListBox listStackLR;
		private Label label4;
		private ListBox listStackUR;
		private Label label1;
		private ODGrid gridApptFieldDefs;
		private ODGrid gridPatFieldDefs;
		private ComboBox comboClinic;
		private Label labelClinic;
		///<summary>Set this value before opening the form.</summary>
		public ApptView ApptViewCur;
		///<summary>Set this value with the clinic selected in FormApptViews.</summary>
		public long ClinicNum;
		///<summary>Local cache of all of the clinic nums the current user has permission to access at the time the form loads.  Filled at the same time as comboAssignedClinic and is used to set apptview.AssignedClinic when saving.</summary>
		private List<long> _listUserClinicNums;
		private TextBox textScrollTime;
		private Label labelStartTime;
		private CheckBox checkDynamicScroll;
		private CheckBox checkApptBubblesDisabled;
		private List<ApptViewItem> _listApptViewItems;
		private List<ApptViewItem> _listApptViewItemDefs;

		///<summary>This is a list of all operatories available to add to this view based on AssignedClinicNum and the clinic the ops are assigned to.  If the clinics show feature is turned off (EasyNoClinics=true) or if the view is not assigned to a clinic, all unhidden ops will be available.  If an op is not assigned to a clinic, it will only be available to add to views also not assigned to a clinic.  If the view is assigned to a clinic, ops assigned to the same clinic will be available to add to the view.</summary>
		private List<long> _listOpNums;
		private List<Provider> _listProviders;

		///<summary></summary>
		public FormApptViewEdit()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApptViewEdit));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.labelOps = new System.Windows.Forms.Label();
			this.listOps = new System.Windows.Forms.ListBox();
			this.listProv = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butLeft = new OpenDental.UI.Button();
			this.butRight = new OpenDental.UI.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.textRowsPerIncr = new System.Windows.Forms.TextBox();
			this.checkOnlyScheduledProvs = new System.Windows.Forms.CheckBox();
			this.textBeforeTime = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.labelAfterTime = new System.Windows.Forms.Label();
			this.textAfterTime = new System.Windows.Forms.TextBox();
			this.labelBeforeTime = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.listStackLR = new System.Windows.Forms.ListBox();
			this.label4 = new System.Windows.Forms.Label();
			this.listStackUR = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.gridLR = new OpenDental.UI.ODGrid();
			this.gridUR = new OpenDental.UI.ODGrid();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.label8 = new System.Windows.Forms.Label();
			this.gridAvailable = new OpenDental.UI.ODGrid();
			this.gridApptFieldDefs = new OpenDental.UI.ODGrid();
			this.gridPatFieldDefs = new OpenDental.UI.ODGrid();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.textScrollTime = new System.Windows.Forms.TextBox();
			this.labelStartTime = new System.Windows.Forms.Label();
			this.checkDynamicScroll = new System.Windows.Forms.CheckBox();
			this.checkApptBubblesDisabled = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
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
			this.butCancel.Location = new System.Drawing.Point(752, 662);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(652, 662);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
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
			this.butDelete.Location = new System.Drawing.Point(32, 662);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(87, 24);
			this.butDelete.TabIndex = 38;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// labelOps
			// 
			this.labelOps.Location = new System.Drawing.Point(32, 123);
			this.labelOps.Name = "labelOps";
			this.labelOps.Size = new System.Drawing.Size(182, 16);
			this.labelOps.TabIndex = 39;
			this.labelOps.Text = "View Operatories";
			this.labelOps.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listOps
			// 
			this.listOps.Location = new System.Drawing.Point(32, 140);
			this.listOps.Name = "listOps";
			this.listOps.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listOps.Size = new System.Drawing.Size(120, 225);
			this.listOps.TabIndex = 40;
			// 
			// listProv
			// 
			this.listProv.Location = new System.Drawing.Point(32, 388);
			this.listProv.Name = "listProv";
			this.listProv.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listProv.Size = new System.Drawing.Size(120, 225);
			this.listProv.TabIndex = 42;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(32, 368);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(128, 18);
			this.label2.TabIndex = 41;
			this.label2.Text = "View Provider Bars";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(7, 4);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(217, 20);
			this.label3.TabIndex = 43;
			this.label3.Text = "Description";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(224, 4);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(160, 20);
			this.textDescription.TabIndex = 44;
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(297, 497);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(71, 24);
			this.butDown.TabIndex = 50;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(219, 497);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(71, 24);
			this.butUp.TabIndex = 51;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butLeft
			// 
			this.butLeft.AdjustImageLocation = new System.Drawing.Point(-1, 0);
			this.butLeft.Autosize = true;
			this.butLeft.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLeft.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLeft.CornerRadius = 4F;
			this.butLeft.Image = global::OpenDental.Properties.Resources.Left;
			this.butLeft.Location = new System.Drawing.Point(389, 326);
			this.butLeft.Name = "butLeft";
			this.butLeft.Size = new System.Drawing.Size(35, 26);
			this.butLeft.TabIndex = 52;
			this.butLeft.Click += new System.EventHandler(this.butLeft_Click);
			// 
			// butRight
			// 
			this.butRight.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRight.Autosize = true;
			this.butRight.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRight.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRight.CornerRadius = 4F;
			this.butRight.Image = global::OpenDental.Properties.Resources.Right;
			this.butRight.Location = new System.Drawing.Point(389, 292);
			this.butRight.Name = "butRight";
			this.butRight.Size = new System.Drawing.Size(35, 26);
			this.butRight.TabIndex = 53;
			this.butRight.Click += new System.EventHandler(this.butRight_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(7, 25);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(217, 20);
			this.label6.TabIndex = 54;
			this.label6.Text = "Rows per time increment (usually 1)";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textRowsPerIncr
			// 
			this.textRowsPerIncr.Location = new System.Drawing.Point(224, 25);
			this.textRowsPerIncr.Name = "textRowsPerIncr";
			this.textRowsPerIncr.Size = new System.Drawing.Size(56, 20);
			this.textRowsPerIncr.TabIndex = 55;
			this.textRowsPerIncr.Validating += new System.ComponentModel.CancelEventHandler(this.textRowsPerIncr_Validating);
			// 
			// checkOnlyScheduledProvs
			// 
			this.checkOnlyScheduledProvs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkOnlyScheduledProvs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkOnlyScheduledProvs.Location = new System.Drawing.Point(11, 17);
			this.checkOnlyScheduledProvs.Name = "checkOnlyScheduledProvs";
			this.checkOnlyScheduledProvs.Size = new System.Drawing.Size(238, 20);
			this.checkOnlyScheduledProvs.TabIndex = 56;
			this.checkOnlyScheduledProvs.Text = "Only show ops for scheduled provs";
			this.checkOnlyScheduledProvs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkOnlyScheduledProvs.UseVisualStyleBackColor = true;
			this.checkOnlyScheduledProvs.Click += new System.EventHandler(this.checkOnlyScheduledProvs_Click);
			// 
			// textBeforeTime
			// 
			this.textBeforeTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textBeforeTime.Location = new System.Drawing.Point(234, 38);
			this.textBeforeTime.Name = "textBeforeTime";
			this.textBeforeTime.Size = new System.Drawing.Size(56, 20);
			this.textBeforeTime.TabIndex = 57;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.labelAfterTime);
			this.groupBox1.Controls.Add(this.textAfterTime);
			this.groupBox1.Controls.Add(this.labelBeforeTime);
			this.groupBox1.Controls.Add(this.textBeforeTime);
			this.groupBox1.Controls.Add(this.checkOnlyScheduledProvs);
			this.groupBox1.Location = new System.Drawing.Point(430, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(397, 93);
			this.groupBox1.TabIndex = 58;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Display Filter";
			// 
			// labelAfterTime
			// 
			this.labelAfterTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelAfterTime.Location = new System.Drawing.Point(11, 62);
			this.labelAfterTime.Name = "labelAfterTime";
			this.labelAfterTime.Size = new System.Drawing.Size(222, 17);
			this.labelAfterTime.TabIndex = 60;
			this.labelAfterTime.Text = "Only if after time";
			this.labelAfterTime.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// textAfterTime
			// 
			this.textAfterTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textAfterTime.Location = new System.Drawing.Point(234, 62);
			this.textAfterTime.Name = "textAfterTime";
			this.textAfterTime.Size = new System.Drawing.Size(56, 20);
			this.textAfterTime.TabIndex = 59;
			// 
			// labelBeforeTime
			// 
			this.labelBeforeTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelBeforeTime.Location = new System.Drawing.Point(11, 38);
			this.labelBeforeTime.Name = "labelBeforeTime";
			this.labelBeforeTime.Size = new System.Drawing.Size(222, 17);
			this.labelBeforeTime.TabIndex = 58;
			this.labelBeforeTime.Text = "Only if before time";
			this.labelBeforeTime.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.listStackLR);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.listStackUR);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.gridLR);
			this.groupBox2.Controls.Add(this.gridUR);
			this.groupBox2.Controls.Add(this.gridMain);
			this.groupBox2.Controls.Add(this.butUp);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.butDown);
			this.groupBox2.Location = new System.Drawing.Point(430, 122);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(397, 529);
			this.groupBox2.TabIndex = 59;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Rows Displayed (double click to edit or to move to another corner)";
			// 
			// listStackLR
			// 
			this.listStackLR.FormattingEnabled = true;
			this.listStackLR.Location = new System.Drawing.Point(192, 283);
			this.listStackLR.Name = "listStackLR";
			this.listStackLR.Size = new System.Drawing.Size(175, 30);
			this.listStackLR.TabIndex = 66;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(190, 264);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(175, 17);
			this.label4.TabIndex = 65;
			this.label4.Text = "LR stack behavior";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listStackUR
			// 
			this.listStackUR.FormattingEnabled = true;
			this.listStackUR.Location = new System.Drawing.Point(192, 213);
			this.listStackUR.Name = "listStackUR";
			this.listStackUR.Size = new System.Drawing.Size(175, 30);
			this.listStackUR.TabIndex = 64;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(190, 194);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(175, 17);
			this.label1.TabIndex = 63;
			this.label1.Text = "UR stack behavior";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// gridLR
			// 
			this.gridLR.HasAddButton = false;
			this.gridLR.HasMultilineHeaders = false;
			this.gridLR.HScrollVisible = false;
			this.gridLR.Location = new System.Drawing.Point(192, 317);
			this.gridLR.Name = "gridLR";
			this.gridLR.ScrollValue = 0;
			this.gridLR.Size = new System.Drawing.Size(175, 174);
			this.gridLR.TabIndex = 62;
			this.gridLR.Title = "Lower Right Corner";
			this.gridLR.TranslationName = "TableLowerRight";
			this.gridLR.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridLR_CellDoubleClick);
			this.gridLR.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridLR_CellClick);
			// 
			// gridUR
			// 
			this.gridUR.HasAddButton = false;
			this.gridUR.HasMultilineHeaders = false;
			this.gridUR.HScrollVisible = false;
			this.gridUR.Location = new System.Drawing.Point(192, 18);
			this.gridUR.Name = "gridUR";
			this.gridUR.ScrollValue = 0;
			this.gridUR.Size = new System.Drawing.Size(175, 174);
			this.gridUR.TabIndex = 61;
			this.gridUR.Title = "Upper Right Corner";
			this.gridUR.TranslationName = "TableUpperRight";
			this.gridUR.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridUR_CellDoubleClick);
			this.gridUR.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridUR_CellClick);
			// 
			// gridMain
			// 
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(11, 18);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(175, 473);
			this.gridMain.TabIndex = 60;
			this.gridMain.Title = "Main List";
			this.gridMain.TranslationName = "TableMainList";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label8.Location = new System.Drawing.Point(11, 500);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(209, 17);
			this.label8.TabIndex = 59;
			this.label8.Text = "Move any item within its own list:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gridAvailable
			// 
			this.gridAvailable.HasAddButton = false;
			this.gridAvailable.HasMultilineHeaders = false;
			this.gridAvailable.HScrollVisible = false;
			this.gridAvailable.Location = new System.Drawing.Point(207, 140);
			this.gridAvailable.Name = "gridAvailable";
			this.gridAvailable.ScrollValue = 0;
			this.gridAvailable.Size = new System.Drawing.Size(175, 255);
			this.gridAvailable.TabIndex = 61;
			this.gridAvailable.Title = "Available Rows";
			this.gridAvailable.TranslationName = "TableAvailableRows";
			this.gridAvailable.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAvailable_CellClick);
			// 
			// gridApptFieldDefs
			// 
			this.gridApptFieldDefs.HasAddButton = false;
			this.gridApptFieldDefs.HasMultilineHeaders = false;
			this.gridApptFieldDefs.HScrollVisible = false;
			this.gridApptFieldDefs.Location = new System.Drawing.Point(207, 398);
			this.gridApptFieldDefs.Name = "gridApptFieldDefs";
			this.gridApptFieldDefs.ScrollValue = 0;
			this.gridApptFieldDefs.Size = new System.Drawing.Size(175, 106);
			this.gridApptFieldDefs.TabIndex = 62;
			this.gridApptFieldDefs.Title = "Appt Field Defs";
			this.gridApptFieldDefs.TranslationName = "TableApptFieldDefs";
			this.gridApptFieldDefs.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridApptFieldDefs_CellClick);
			// 
			// gridPatFieldDefs
			// 
			this.gridPatFieldDefs.HasAddButton = false;
			this.gridPatFieldDefs.HasMultilineHeaders = false;
			this.gridPatFieldDefs.HScrollVisible = false;
			this.gridPatFieldDefs.Location = new System.Drawing.Point(207, 507);
			this.gridPatFieldDefs.Name = "gridPatFieldDefs";
			this.gridPatFieldDefs.ScrollValue = 0;
			this.gridPatFieldDefs.Size = new System.Drawing.Size(175, 106);
			this.gridPatFieldDefs.TabIndex = 63;
			this.gridPatFieldDefs.Title = "Patient Field Defs";
			this.gridPatFieldDefs.TranslationName = "TablePatFieldDefs";
			this.gridPatFieldDefs.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPatFieldDefs_CellClick);
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(222, 104);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(160, 21);
			this.comboClinic.TabIndex = 133;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(5, 104);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(217, 20);
			this.labelClinic.TabIndex = 132;
			this.labelClinic.Text = "Assigned Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textScrollTime
			// 
			this.textScrollTime.Location = new System.Drawing.Point(224, 46);
			this.textScrollTime.Name = "textScrollTime";
			this.textScrollTime.Size = new System.Drawing.Size(56, 20);
			this.textScrollTime.TabIndex = 134;
			// 
			// labelStartTime
			// 
			this.labelStartTime.Location = new System.Drawing.Point(7, 46);
			this.labelStartTime.Name = "labelStartTime";
			this.labelStartTime.Size = new System.Drawing.Size(217, 20);
			this.labelStartTime.TabIndex = 135;
			this.labelStartTime.Text = "View start time on load";
			this.labelStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkDynamicScroll
			// 
			this.checkDynamicScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkDynamicScroll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDynamicScroll.Location = new System.Drawing.Point(8, 69);
			this.checkDynamicScroll.Name = "checkDynamicScroll";
			this.checkDynamicScroll.Size = new System.Drawing.Size(230, 17);
			this.checkDynamicScroll.TabIndex = 61;
			this.checkDynamicScroll.Text = "Dynamic start time based on schedule";
			this.checkDynamicScroll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDynamicScroll.UseVisualStyleBackColor = true;
			// 
			// checkApptBubblesDisabled
			// 
			this.checkApptBubblesDisabled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkApptBubblesDisabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptBubblesDisabled.Location = new System.Drawing.Point(8, 86);
			this.checkApptBubblesDisabled.Name = "checkApptBubblesDisabled";
			this.checkApptBubblesDisabled.Size = new System.Drawing.Size(230, 17);
			this.checkApptBubblesDisabled.TabIndex = 136;
			this.checkApptBubblesDisabled.Text = "Disable appointment bubbles";
			this.checkApptBubblesDisabled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptBubblesDisabled.UseVisualStyleBackColor = true;
			// 
			// FormApptViewEdit
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(852, 695);
			this.Controls.Add(this.checkApptBubblesDisabled);
			this.Controls.Add(this.checkDynamicScroll);
			this.Controls.Add(this.labelStartTime);
			this.Controls.Add(this.textScrollTime);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.gridPatFieldDefs);
			this.Controls.Add(this.gridApptFieldDefs);
			this.Controls.Add(this.gridAvailable);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.textRowsPerIncr);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.butRight);
			this.Controls.Add(this.butLeft);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.listProv);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.listOps);
			this.Controls.Add(this.labelOps);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormApptViewEdit";
			this.ShowInTaskbar = false;
			this.Text = "Appointment View Edit";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormApptViewEdit_Closing);
			this.Load += new System.EventHandler(this.FormApptViewEdit_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormApptViewEdit_Load(object sender, System.EventArgs e) {
			textDescription.Text=ApptViewCur.Description;
			if(ApptViewCur.RowsPerIncr==0){
				textRowsPerIncr.Text="1";
			}
			else{
				textRowsPerIncr.Text=ApptViewCur.RowsPerIncr.ToString();
			}
			checkOnlyScheduledProvs.Checked=ApptViewCur.OnlyScheduledProvs;
			if(ApptViewCur.OnlySchedBeforeTime > new TimeSpan(0,0,0)) {
				textBeforeTime.Text=(DateTime.Today+ApptViewCur.OnlySchedBeforeTime).ToShortTimeString();
			}
			if(ApptViewCur.OnlySchedAfterTime > new TimeSpan(0,0,0)) {
				textAfterTime.Text=(DateTime.Today+ApptViewCur.OnlySchedAfterTime).ToShortTimeString();
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				comboClinic.Visible=false;
				labelClinic.Visible=false;
				_listUserClinicNums=new List<long>() { 0 };//if clinics are disabled, apptview.ClinicNum will be set to 0
			}
			else {//Using clinics
				Clinics.RefreshCache();
				_listUserClinicNums=new List<long>();
				List<Clinic> listClinics=Clinics.GetForUserod(Security.CurUser);
				comboClinic.Items.Clear();
				if(!Security.CurUser.ClinicIsRestricted) {
					comboClinic.Items.Add(Lan.g(this,"All"));
					_listUserClinicNums.Add(0);//this way both lists have the same number of items in it and if 'All' is selected the view, ClinicNum will be set to 0
					comboClinic.SelectedIndex=0;
				}
				for(int i=0;i<listClinics.Count;i++) {
					comboClinic.Items.Add(listClinics[i].Abbr);
					_listUserClinicNums.Add(listClinics[i].ClinicNum);
					if(ClinicNum==listClinics[i].ClinicNum) {
						if(Security.CurUser.ClinicIsRestricted) {
							comboClinic.SelectedIndex=i;
						}
						else {
							comboClinic.SelectedIndex=i+1;//increment the SelectedIndex to account for 'All' in the list at position 0.
						}
					}
				}
			}
			UpdateDisplayFilterGroup();
			_listApptViewItems=ApptViewItems.GetWhere(x => x.ApptViewNum==ApptViewCur.ApptViewNum);
			_listApptViewItemDefs=_listApptViewItems.FindAll(x => x.OpNum==0 && x.ProvNum==0);
			FillOperatories();
			_listProviders=Providers.GetDeepCopy(true);
			for(int i=0;i<_listProviders.Count;i++) {
				listProv.Items.Add(_listProviders[i].GetLongDesc());
				if(_listApptViewItems.Select(x => x.ProvNum).Contains(_listProviders[i].ProvNum)) {
					listProv.SetSelected(i,true);
				}
			}
			for(int i=0;i<Enum.GetNames(typeof(ApptViewStackBehavior)).Length;i++){
				listStackUR.Items.Add(Lan.g("enumApptViewStackBehavior",Enum.GetNames(typeof(ApptViewStackBehavior))[i]));
				listStackLR.Items.Add(Lan.g("enumApptViewStackBehavior",Enum.GetNames(typeof(ApptViewStackBehavior))[i]));
			}
			listStackUR.SelectedIndex=(int)ApptViewCur.StackBehavUR;
			listStackLR.SelectedIndex=(int)ApptViewCur.StackBehavLR;
			elementsAll=new List<String>();
			elementsAll.Add("Address");
			elementsAll.Add("AddrNote");
			elementsAll.Add("Age");
			elementsAll.Add("ASAP");
			elementsAll.Add("ASAP[A]");
			elementsAll.Add("AssistantAbbr");
			elementsAll.Add("Birthdate");
			elementsAll.Add("ChartNumAndName");
			elementsAll.Add("ChartNumber");
			elementsAll.Add("ConfirmedColor");
			elementsAll.Add("CreditType");
			elementsAll.Add("EstPatientPortion");
			elementsAll.Add("Guardians");
			elementsAll.Add("HasIns[I]");
			elementsAll.Add("HmPhone");
			elementsAll.Add("InsToSend[!]");
			elementsAll.Add("Insurance");
			elementsAll.Add("InsuranceColor");
			elementsAll.Add("IsLate[L]");
			elementsAll.Add("Lab");
			elementsAll.Add("LateColor");
			elementsAll.Add("MedOrPremed[+]");
			elementsAll.Add("MedUrgNote");
			elementsAll.Add("NetProduction");
			elementsAll.Add("Note");
			elementsAll.Add("PatientName");
			elementsAll.Add("PatientNameF");
			elementsAll.Add("PatientNamePref");
			elementsAll.Add("PatNum");
			elementsAll.Add("PatNumAndName");
			elementsAll.Add("PremedFlag");
			elementsAll.Add("Procs");
			elementsAll.Add("ProcsColored");
			elementsAll.Add("Production");
			elementsAll.Add("Provider");
			elementsAll.Add("TimeAskedToArrive");
			elementsAll.Add("WirelessPhone");
			elementsAll.Add("WkPhone");
			displayedElementsAll=new List<ApptViewItem>(_listApptViewItemDefs);
			checkDynamicScroll.Checked=ApptViewCur.IsScrollStartDynamic;
			textScrollTime.Text=ApptViewCur.ApptTimeScrollStart.ToStringHmm();
			checkApptBubblesDisabled.Checked=ApptViewCur.IsApptBubblesDisabled;
			if(IsNew) {
				checkApptBubblesDisabled.Checked=PrefC.GetBool(PrefName.AppointmentBubblesDisabled);
			}
			FillElements();
		}

		///<summary>Fills the five lists based on the displayedElements lists. No database transactions are performed here.</summary>
		private void FillElements(){
			displayedElementsMain=new List<ApptViewItem>();
			displayedElementsUR=new List<ApptViewItem>();
			displayedElementsLR=new List<ApptViewItem>();
			for(int i=0;i<displayedElementsAll.Count;i++) {
				if(displayedElementsAll[i].ElementAlignment==ApptViewAlignment.Main) {
					displayedElementsMain.Add(displayedElementsAll[i]);
				}
				else if(displayedElementsAll[i].ElementAlignment==ApptViewAlignment.UR) {
					displayedElementsUR.Add(displayedElementsAll[i]);
				}
				else if(displayedElementsAll[i].ElementAlignment==ApptViewAlignment.LR) {
					displayedElementsLR.Add(displayedElementsAll[i]);
				}
			}
			//Now fill the lists on the screen--------------------------------------------------
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn("",100);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<displayedElementsMain.Count;i++){
				row=new ODGridRow();
				if(displayedElementsMain[i].ApptFieldDefNum>0){
					row.Cells.Add(ApptFieldDefs.GetFieldName(displayedElementsMain[i].ApptFieldDefNum));
				}
				else if(displayedElementsMain[i].PatFieldDefNum>0){
					row.Cells.Add(PatFieldDefs.GetFieldName(displayedElementsMain[i].PatFieldDefNum));
				}
				else{
					row.Cells.Add(displayedElementsMain[i].ElementDesc);
				}
				if(displayedElementsMain[i].ElementDesc=="MedOrPremed[+]"
					|| displayedElementsMain[i].ElementDesc=="HasIns[I]"
					|| displayedElementsMain[i].ElementDesc=="InsToSend[!]"
					|| displayedElementsMain[i].ElementDesc=="LateColor")
				{
					row.ColorBackG=displayedElementsMain[i].ElementColor;
				}
				else{
					row.ColorText=displayedElementsMain[i].ElementColor;
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			//gridUR---------------------------------------------------------
			gridUR.BeginUpdate();
			gridUR.Columns.Clear();
			col=new ODGridColumn("",100);
			gridUR.Columns.Add(col);
			gridUR.Rows.Clear();
			for(int i=0;i<displayedElementsUR.Count;i++) {
				row=new ODGridRow();
				if(displayedElementsUR[i].ApptFieldDefNum>0) {
					row.Cells.Add(ApptFieldDefs.GetFieldName(displayedElementsUR[i].ApptFieldDefNum));
				}
				else if(displayedElementsUR[i].PatFieldDefNum>0) {
					row.Cells.Add(PatFieldDefs.GetFieldName(displayedElementsUR[i].PatFieldDefNum));
				}
				else {
					row.Cells.Add(displayedElementsUR[i].ElementDesc);
				}
				if(displayedElementsUR[i].ElementDesc=="MedOrPremed[+]"
					|| displayedElementsUR[i].ElementDesc=="HasIns[I]"
					|| displayedElementsUR[i].ElementDesc=="InsToSend[!]"
					|| displayedElementsUR[i].ElementDesc=="LateColor")
				{
					row.ColorBackG=displayedElementsUR[i].ElementColor;
				}
				else{
					row.ColorText=displayedElementsUR[i].ElementColor;
				}
				gridUR.Rows.Add(row);
			}
			gridUR.EndUpdate();
			//gridLR-----------------------------------------------------------
			gridLR.BeginUpdate();
			gridLR.Columns.Clear();
			col=new ODGridColumn("",100);
			gridLR.Columns.Add(col);
			gridLR.Rows.Clear();
			for(int i=0;i<displayedElementsLR.Count;i++) {
				row=new ODGridRow();
				if(displayedElementsLR[i].ApptFieldDefNum>0) {
					row.Cells.Add(ApptFieldDefs.GetFieldName(displayedElementsLR[i].ApptFieldDefNum));
				}
				else if(displayedElementsLR[i].PatFieldDefNum>0) {
					row.Cells.Add(PatFieldDefs.GetFieldName(displayedElementsLR[i].PatFieldDefNum));
				}
				else {
					row.Cells.Add(displayedElementsLR[i].ElementDesc);
				}
				if(displayedElementsLR[i].ElementDesc=="MedOrPremed[+]"
					|| displayedElementsLR[i].ElementDesc=="HasIns[I]"
					|| displayedElementsLR[i].ElementDesc=="InsToSend[!]"
					|| displayedElementsLR[i].ElementDesc=="LateColor")
				{
					row.ColorBackG=displayedElementsLR[i].ElementColor;
				}
				else{
					row.ColorText=displayedElementsLR[i].ElementColor;
				}
				gridLR.Rows.Add(row);
			}
			gridLR.EndUpdate();
			//gridAvailable-----------------------------------------------------------
			gridAvailable.BeginUpdate();
			gridAvailable.Columns.Clear();
			col=new ODGridColumn("",100);
			gridAvailable.Columns.Add(col);
			gridAvailable.Rows.Clear();
			displayedAvailable=new List<int>();
			for(int i=0;i<elementsAll.Count;i++) {
				if(!ElementIsDisplayed(elementsAll[i])) {
					displayedAvailable.Add(i);
					row=new ODGridRow();
					row.Cells.Add(Lan.g(this,elementsAll[i]));
					gridAvailable.Rows.Add(row);
				}
			}
			gridAvailable.EndUpdate();
			//gridApptFieldDefs-----------------------------------------------------------
			gridApptFieldDefs.BeginUpdate();
			gridApptFieldDefs.Columns.Clear();
			col=new ODGridColumn("",100);
			gridApptFieldDefs.Columns.Add(col);
			gridApptFieldDefs.Rows.Clear();
			displayedAvailableApptFieldDefs=new List<long>();
			List<ApptFieldDef> listApptFieldDefs=ApptFieldDefs.GetDeepCopy();
			for(int i=0;i<listApptFieldDefs.Count;i++) {
				if(!ApptFieldIsDisplayed(listApptFieldDefs[i].ApptFieldDefNum)) {
					displayedAvailableApptFieldDefs.Add(listApptFieldDefs[i].ApptFieldDefNum);
					row=new ODGridRow();
					row.Cells.Add(listApptFieldDefs[i].FieldName);
					gridApptFieldDefs.Rows.Add(row);
				}
			}
			gridApptFieldDefs.EndUpdate();
			//gridPatFieldDefs-----------------------------------------------------------
			gridPatFieldDefs.BeginUpdate();
			gridPatFieldDefs.Columns.Clear();
			col=new ODGridColumn("",100);
			gridPatFieldDefs.Columns.Add(col);
			gridPatFieldDefs.Rows.Clear();
			_listPatFieldDefNums=new List<long>();
			List<PatFieldDef> listPatFieldDefs=PatFieldDefs.GetDeepCopy(true);
			for(int i=0;i<listPatFieldDefs.Count;i++) {
				if(!PatFieldIsDisplayed(listPatFieldDefs[i].PatFieldDefNum)) {
					_listPatFieldDefNums.Add(listPatFieldDefs[i].PatFieldDefNum);
					row=new ODGridRow();
					row.Cells.Add(listPatFieldDefs[i].FieldName);
					gridPatFieldDefs.Rows.Add(row);
				}
			}
			gridPatFieldDefs.EndUpdate();
		}

		///<summary>Fills the list box of operatories available for the view.  Considers clinics.</summary>
		private void FillOperatories() {
			listOps.ClearSelected();
			listOps.Items.Clear();
			_listOpNums=new List<long>();
			List<Operatory> listOperatories=Operatories.GetDeepCopy(true);
			for(int i=0;i<listOperatories.Count;i++) {
				if(PrefC.GetBool(PrefName.EasyNoClinics) //add op to list of ops available for the view if the clinics show feature is turned off
					|| ClinicNum==0 //or this view is not assigned to a clinic
					|| listOperatories[i].ClinicNum==ClinicNum) //or the operatory is assigned to same clinic as this view
				{
					listOps.Items.Add(listOperatories[i].OpName);
					_listOpNums.Add(listOperatories[i].OperatoryNum);
					if(_listApptViewItems.Select(x => x.OpNum).Contains(listOperatories[i].OperatoryNum)) {
						listOps.SetSelected(listOps.Items.Count-1,true);
					}
				}
			}
		}

		///<summary>Called from FillElements. Used to determine whether a given element is already displayed. If not, then it is displayed in the available rows on the left.</summary>
		private bool ElementIsDisplayed(string elementDesc){
			for(int i=0;i<displayedElementsAll.Count;i++){
				if(displayedElementsAll[i].ElementDesc==elementDesc){
					return true;
				}
			}
			return false;
		}

		///<summary>Called from FillElements. Used to determine whether a apptfield is already displayed. If not, then it is displayed in the apptFieldDef rows on the left.</summary>
		private bool ApptFieldIsDisplayed(long apptFieldDefNum){
			for(int i=0;i<displayedElementsAll.Count;i++){
				if(displayedElementsAll[i].ApptFieldDefNum==apptFieldDefNum){
					return true;
				}
			}
			return false;
		}

		///<summary>Called from FillElements. Used to determine whether a PatFieldDef is already displayed. If not, then it is displayed in the patFieldDef rows on the left.</summary>
		private bool PatFieldIsDisplayed(long patFieldDefNum){
			for(int i=0;i<displayedElementsAll.Count;i++){
				if(displayedElementsAll[i].PatFieldDefNum==patFieldDefNum){
					return true;
				}
			}
			return false;
		}

		private void checkOnlyScheduledProvs_Click(object sender,EventArgs e) {
			UpdateDisplayFilterGroup();
		}

		///<summary>Updates the display filter visibility based on the state of checkOnlyScheduledProvs.</summary>
		private void UpdateDisplayFilterGroup(){
			if(checkOnlyScheduledProvs.Checked) {
				labelBeforeTime.Visible=true;
				labelAfterTime.Visible=true;
				textBeforeTime.Visible=true;
				textAfterTime.Visible=true;
			}
			else {
				labelBeforeTime.Visible=false;
				labelAfterTime.Visible=false;
				textBeforeTime.Visible=false;
				textAfterTime.Visible=false;
			}
		}

		private void butLeft_Click(object sender, System.EventArgs e) {
			if(gridMain.SelectedIndices.Length>0) {
				displayedElementsAll.Remove(displayedElementsMain[gridMain.SelectedIndices[0]]);
			}
			else if(gridUR.SelectedIndices.Length>0) {
				displayedElementsAll.Remove(displayedElementsUR[gridUR.SelectedIndices[0]]);
			}
			else if(gridLR.SelectedIndices.Length>0) {
				displayedElementsAll.Remove(displayedElementsLR[gridLR.SelectedIndices[0]]);
			}
			FillElements();
		}

		private void butRight_Click(object sender, System.EventArgs e) {
			if(gridAvailable.GetSelectedIndex()!=-1) {
				//the item order is not used until saving to db.
				ApptViewItem item=new ApptViewItem(elementsAll[displayedAvailable[gridAvailable.GetSelectedIndex()]],0,Color.Black);
				if(gridMain.SelectedIndices.Length==1) {//insert
					int newIdx=displayedElementsAll.IndexOf(displayedElementsMain[gridMain.GetSelectedIndex()]);
					displayedElementsAll.Insert(newIdx,item);
				}
				else {//add to end
					displayedElementsAll.Add(item);
				}
				FillElements();
				for(int i=0;i<displayedElementsMain.Count;i++) {//the new item will always show first in the main list.
					if(displayedElementsMain[i]==item) {
						gridMain.SetSelected(i,true);//reselect the item
						break;
					}
				}
			}
			else if(gridApptFieldDefs.GetSelectedIndex()!=-1) {
				ApptViewItem item=new ApptViewItem();
				item.ElementColor=Color.Black;
				item.ApptFieldDefNum=displayedAvailableApptFieldDefs[gridApptFieldDefs.GetSelectedIndex()];
				if(gridMain.SelectedIndices.Length==1) {//insert
					int newIdx=displayedElementsAll.IndexOf(displayedElementsMain[gridMain.GetSelectedIndex()]);
					displayedElementsAll.Insert(newIdx,item);
				}
				else {//add to end
					displayedElementsAll.Add(item);
				}
				FillElements();
				for(int i=0;i<displayedElementsMain.Count;i++) {//the new item will always show first in the main list.
					if(displayedElementsMain[i]==item) {
						gridMain.SetSelected(i,true);//reselect the item
						break;
					}
				}
			}
			else if(gridPatFieldDefs.GetSelectedIndex()!=-1) {
				ApptViewItem item=new ApptViewItem();
				item.ElementColor=Color.Black;
				item.PatFieldDefNum=_listPatFieldDefNums[gridPatFieldDefs.GetSelectedIndex()]; 
				if(gridMain.SelectedIndices.Length==1) {//insert
					int newIdx=displayedElementsAll.IndexOf(displayedElementsMain[gridMain.GetSelectedIndex()]);
					displayedElementsAll.Insert(newIdx,item);
				}
				else {//add to end
					displayedElementsAll.Add(item);
				}
				FillElements();
				for(int i=0;i<displayedElementsMain.Count;i++) {//the new item will always show first in the main list.
					if(displayedElementsMain[i]==item) {
						gridMain.SetSelected(i,true);//reselect the item
						break;
					}
				}
			}
		}

		private void butUp_Click(object sender,System.EventArgs e) {
			int oldIdx;
			int newIdx;
			int newIdxAll;//within the list of all.
			ApptViewItem item;
			if(gridMain.GetSelectedIndex()!=-1) {
				oldIdx=gridMain.GetSelectedIndex();
				if(oldIdx==0) {
					return;//can't move up any more
				}
				item=displayedElementsMain[oldIdx];
				newIdx=oldIdx-1;
				newIdxAll=displayedElementsAll.IndexOf(displayedElementsMain[newIdx]);
				displayedElementsAll.Remove(item);
				displayedElementsAll.Insert(newIdxAll,item);
				FillElements();
				gridMain.SetSelected(newIdx,true);
			}
			else if(gridUR.GetSelectedIndex()!=-1) {
				oldIdx=gridUR.GetSelectedIndex();
				if(oldIdx==0) {
					return;//can't move up any more
				}
				item=displayedElementsUR[oldIdx];
				newIdx=oldIdx-1;
				newIdxAll=displayedElementsAll.IndexOf(displayedElementsUR[newIdx]);
				displayedElementsAll.Remove(item);
				displayedElementsAll.Insert(newIdxAll,item);
				FillElements();
				gridUR.SetSelected(newIdx,true);
			}
			else if(gridLR.GetSelectedIndex()!=-1) {
				oldIdx=gridLR.GetSelectedIndex();
				if(oldIdx==0) {
					return;//can't move up any more
				}
				item=displayedElementsLR[oldIdx];
				newIdx=oldIdx-1;
				newIdxAll=displayedElementsAll.IndexOf(displayedElementsLR[newIdx]);
				displayedElementsAll.Remove(item);
				displayedElementsAll.Insert(newIdxAll,item);
				FillElements();
				gridLR.SetSelected(newIdx,true);
			}
		}

		private void butDown_Click(object sender, System.EventArgs e) {
			int oldIdx;
			int newIdx;
			int newIdxAll;
			ApptViewItem item;
			if(gridMain.GetSelectedIndex()!=-1) {
				oldIdx=gridMain.GetSelectedIndex();
				if(oldIdx==displayedElementsMain.Count-1) {
					return;//can't move down any more
				}
				item=displayedElementsMain[oldIdx];
				newIdx=oldIdx+1;
				newIdxAll=displayedElementsAll.IndexOf(displayedElementsMain[newIdx]);
				displayedElementsAll.Remove(item);
				displayedElementsAll.Insert(newIdxAll,item);
				FillElements();
				gridMain.SetSelected(newIdx,true);
			}
			if(gridUR.GetSelectedIndex()!=-1) {
				oldIdx=gridUR.GetSelectedIndex();
				if(oldIdx==displayedElementsUR.Count-1) {
					return;//can't move down any more
				}
				item=displayedElementsUR[oldIdx];
				newIdx=oldIdx+1;
				newIdxAll=displayedElementsAll.IndexOf(displayedElementsUR[newIdx]);
				displayedElementsAll.Remove(item);
				displayedElementsAll.Insert(newIdxAll,item);
				FillElements();
				gridUR.SetSelected(newIdx,true);
			}
			if(gridLR.GetSelectedIndex()!=-1) {
				oldIdx=gridLR.GetSelectedIndex();
				if(oldIdx==displayedElementsLR.Count-1) {
					return;//can't move down any more
				}
				item=displayedElementsLR[oldIdx];
				newIdx=oldIdx+1;
				newIdxAll=displayedElementsAll.IndexOf(displayedElementsLR[newIdx]);
				displayedElementsAll.Remove(item);
				displayedElementsAll.Insert(newIdxAll,item);
				FillElements();
				gridLR.SetSelected(newIdx,true);
			}
		}

		private void gridAvailable_CellClick(object sender,ODGridClickEventArgs e) {
			if(gridAvailable.SelectedIndices.Length>0) {
				gridApptFieldDefs.SetSelected(false);
				gridPatFieldDefs.SetSelected(false);
			}
		}

		private void gridApptFieldDefs_CellClick(object sender,ODGridClickEventArgs e) {
			if(gridApptFieldDefs.SelectedIndices.Length>0) {
				gridAvailable.SetSelected(false);
				gridPatFieldDefs.SetSelected(false);
			}
		}

		private void gridPatFieldDefs_CellClick(object sender,ODGridClickEventArgs e) {
			if(gridPatFieldDefs.SelectedIndices.Length>0) {
				gridAvailable.SetSelected(false);
				gridApptFieldDefs.SetSelected(false);
			}
		}

		private void gridMain_CellClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			if(gridMain.SelectedIndices.Length>0) {
				gridUR.SetSelected(false);
				gridLR.SetSelected(false);
			}
		}

		private void gridUR_CellClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			if(gridUR.SelectedIndices.Length>0) {
				gridMain.SetSelected(false);
				gridLR.SetSelected(false);
			}
		}

		private void gridLR_CellClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			if(gridLR.SelectedIndices.Length>0) {
				gridUR.SetSelected(false);
				gridMain.SetSelected(false);
			}
		}

		private void gridMain_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			FormApptViewItemEdit formA=new FormApptViewItemEdit();
			formA.ApptVItem=displayedElementsMain[e.Row];
			formA.ShowDialog();
			FillElements();
			ReselectItem(formA.ApptVItem);
		}

		private void gridUR_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			FormApptViewItemEdit formA=new FormApptViewItemEdit();
			formA.ApptVItem=displayedElementsUR[e.Row];
			formA.ShowDialog();
			FillElements();
			ReselectItem(formA.ApptVItem);
		}

		private void gridLR_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			FormApptViewItemEdit formA=new FormApptViewItemEdit();
			formA.ApptVItem=displayedElementsLR[e.Row];
			formA.ShowDialog();
			FillElements();
			ReselectItem(formA.ApptVItem);
		}

		///<summary>When we know what item we want to select, but we don't know which of the three areas it might now be in.</summary>
		private void ReselectItem(ApptViewItem item){
			//another way of doing this would be to test which area it was in first, but that wouldn't make the code more compact.
			for(int i=0;i<displayedElementsMain.Count;i++) {
				if(displayedElementsMain[i]==item) {
					gridMain.SetSelected(i,true);
					break;
				}
			}
			for(int i=0;i<displayedElementsUR.Count;i++) {
				if(displayedElementsUR[i]==item) {
					gridUR.SetSelected(i,true);
					break;
				}
			}
			for(int i=0;i<displayedElementsLR.Count;i++) {
				if(displayedElementsLR[i]==item) {
					gridLR.SetSelected(i,true);
					break;
				}
			}
		}

		private void textRowsPerIncr_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			try{
				Convert.ToInt32(textRowsPerIncr.Text);
			}
			catch{
				MessageBox.Show(Lan.g(this,"Must be a number between 1 and 3."));
				e.Cancel=true;
				return;
			}
			if(PIn.Long(textRowsPerIncr.Text)<1 || PIn.Long(textRowsPerIncr.Text)>3){
				MessageBox.Show(Lan.g(this,"Must be a number between 1 and 3."));
				e.Cancel=true;
			}
		}

		///<summary>This will remove operatories from the list of ops available to assign to this view and fill the list with ops assigned to the same clinic or unassigned.  If the current view has operatories selected that are assigned to a different view.</summary>
		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			if(ClinicNum==_listUserClinicNums[comboClinic.SelectedIndex]) {
				return;
			}
			ClinicNum=_listUserClinicNums[comboClinic.SelectedIndex];
			FillOperatories();
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			//this does mess up the item orders a little, but missing numbers don't actually hurt anything.
			if(MessageBox.Show(Lan.g(this,"Delete this category?"),"",MessageBoxButtons.OKCancel)
				!=DialogResult.OK){
				return;
			}
			ApptViewItems.DeleteAllForView(ApptViewCur);
			ApptViews.Delete(ApptViewCur);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(listProv.SelectedIndices.Count==0){
				MsgBox.Show(this,"At least one provider must be selected.");
				return;
			}
			if(listOps.SelectedIndices.Count==0){// && !checkOnlyScheduledProvs.Checked) {
				MsgBox.Show(this,"At least one operatory must be selected.");
				return;
			}
			if(textDescription.Text==""){
				MessageBox.Show(Lan.g(this,"A description must be entered."));
				return;
			}
			if(displayedElementsMain.Count==0){
				MessageBox.Show(Lan.g(this,"At least one row type must be displayed."));
				return;
			}
			DateTime timeBefore=new DateTime();//only the time portion will be used.
			if(checkOnlyScheduledProvs.Checked && textBeforeTime.Text!="") {
				try {
					timeBefore=DateTime.Parse(textBeforeTime.Text);
				}
				catch {
					MsgBox.Show(this,"Time before invalid.");
					return;
				}
			}
			DateTime timeAfter=new DateTime();
			if(checkOnlyScheduledProvs.Checked && textAfterTime.Text!="") {
				try {
					timeAfter=DateTime.Parse(textAfterTime.Text);
				}
				catch {
					MsgBox.Show(this,"Time after invalid.");
					return;
				}
			}
			DateTime timeScroll=new DateTime();
			if(textScrollTime.Text=="") {
				timeScroll=DateTime.Parse("08:00:00");
			}
			else {
				try {
					timeScroll=DateTime.Parse(textScrollTime.Text);
				}
				catch {
					MsgBox.Show(this,"Scroll start time invalid.");
					return;
				}
			}
			ApptViewItems.DeleteAllForView(ApptViewCur);//start with a clean slate
			ApptViewItem item;
			for(int i=0;i<_listOpNums.Count;i++){
				if(listOps.SelectedIndices.Contains(i)){
					item=new ApptViewItem();
					item.ApptViewNum=ApptViewCur.ApptViewNum;
					item.OpNum=_listOpNums[i];
					ApptViewItems.Insert(item);
				}
			}
			for(int i=0;i<_listProviders.Count;i++){
				if(listProv.SelectedIndices.Contains(i)){
					item=new ApptViewItem();
					item.ApptViewNum=ApptViewCur.ApptViewNum;
					item.ProvNum=_listProviders[i].ProvNum;
					ApptViewItems.Insert(item);
				}
			}
			ApptViewCur.StackBehavUR=(ApptViewStackBehavior)listStackUR.SelectedIndex;
			ApptViewCur.StackBehavLR=(ApptViewStackBehavior)listStackLR.SelectedIndex;
			for(int i=0;i<displayedElementsMain.Count;i++){
				item=displayedElementsMain[i];
				item.ApptViewNum=ApptViewCur.ApptViewNum;
				//elementDesc, elementColor, and Alignment already handled.
				item.ElementOrder=(byte)i;
				ApptViewItems.Insert(item);
			}
			for(int i=0;i<displayedElementsUR.Count;i++) {
				item=displayedElementsUR[i];
				item.ApptViewNum=ApptViewCur.ApptViewNum;
				item.ElementOrder=(byte)i;
				ApptViewItems.Insert(item);
			}
			for(int i=0;i<displayedElementsLR.Count;i++) {
				item=displayedElementsLR[i];
				item.ApptViewNum=ApptViewCur.ApptViewNum;
				item.ElementOrder=(byte)i;
				ApptViewItems.Insert(item);
			}
			ApptViewCur.Description=textDescription.Text;
			ApptViewCur.RowsPerIncr=PIn.Byte(textRowsPerIncr.Text);
			ApptViewCur.OnlyScheduledProvs=checkOnlyScheduledProvs.Checked;
			ApptViewCur.OnlySchedBeforeTime=timeBefore.TimeOfDay;
			ApptViewCur.OnlySchedAfterTime=timeAfter.TimeOfDay;
			ApptViewCur.IsScrollStartDynamic=checkDynamicScroll.Checked;
			ApptViewCur.ApptTimeScrollStart=timeScroll.TimeOfDay;
			ApptViewCur.IsApptBubblesDisabled=checkApptBubblesDisabled.Checked;
			ApptViewCur.ClinicNum=0;//Default is all clinics
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				//_listUserClinicNums will contain only a 0 if the clinics show feature is disabled.
				//If the user is not restricted to a clinic, the list will contain 0 in the first position since comboClinic will contain 'All' as the first option.
				//Restricted users (Security.CurUser.ClinicsIsRestricted=true && Security.CurUser.ClinicNum>0) won't have access to the unassigned views (AssignedClinic=0)
				ApptViewCur.ClinicNum=_listUserClinicNums[comboClinic.SelectedIndex];
			}
			ApptViews.Update(ApptViewCur);//same whether isnew or not
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;;
		}

		private void FormApptViewEdit_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(DialogResult==DialogResult.OK) {
				return;
			}
			if(IsNew){
				ApptViewItems.DeleteAllForView(ApptViewCur);
				ApptViews.Delete(ApptViewCur);
			}
		}
	}
}





















