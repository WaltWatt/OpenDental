using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Linq;
using CodeBase;

namespace OpenDental{
///<summary></summary>
	public class FormProviderSetup:ODForm {
		private OpenDental.UI.Button butClose;
		private OpenDental.UI.Button butDown;
		private OpenDental.UI.Button butUp;
		private OpenDental.UI.Button butAdd;
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.ODGrid gridMain;
		private GroupBox groupDentalSchools;
		private ComboBox comboClass;
		private Label label1;
		///<summary>Indicates that something about the providers has changed and needs to send out an invalid signal to the other workstations.</summary>
		private bool _hasChanged;
		private OpenDental.UI.Button butCreateUsers;
		private GroupBox groupCreateUsers;
		private Label label3;
		private GroupBox groupMovePats;
		private Label label2;
		private UI.Button butMovePri;
		private UI.Button butReassign;
		private Label labelReassign;
		private RadioButton radioInstructors;
		private RadioButton radioStudents;
		private RadioButton radioAll;
		private Label label4;
		private TextBox textLastName;
		private UI.Button butProvPick;
		private TextBox textMoveTo;
		//private User user;
		private DataTable _tableProvs;
		private Label label7;
		private TextBox textProvNum;
		private Label label6;
		private TextBox textFirstName;
		private UI.Button butStudBulkEdit;
		private Label label8;
		private UI.Button butMoveSec;
		///<summary>Set when prov picker button is used.  textMoveTo shows this prov in human readable format.</summary>
		private long _provNumMoveTo=-1;
		private CheckBox checkShowDeleted;
		private List<UserGroup> _listUserGroups;
		private GroupBox groupBox1;
		private UI.Button butAlphabetize;
		private CheckBox checkShowHidden;
		private ToolTip _priProvEditToolTip=new ToolTip() { ShowAlways=true };
		private ComboBoxMulti comboUserGroup;

		///<summary>A stale copy of all providers.  Gets a lazy update whenever needed (e.g. after ProvEdit window closes with changes)/</summary>
		private List<Provider> _listProvs;
		private List<SchoolClass> _listSchoolClasses;

		///<summary>Not used for selection.  Use FormProviderPick or FormProviderMultiPick for that.</summary>
		public FormProviderSetup(){
			InitializeComponent();
			Lan.F(this);
			if(PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
				this.Width=940;
			}
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

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProviderSetup));
			this.butClose = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.groupDentalSchools = new System.Windows.Forms.GroupBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.textProvNum = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textFirstName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textLastName = new System.Windows.Forms.TextBox();
			this.radioInstructors = new System.Windows.Forms.RadioButton();
			this.radioStudents = new System.Windows.Forms.RadioButton();
			this.radioAll = new System.Windows.Forms.RadioButton();
			this.comboClass = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.butCreateUsers = new OpenDental.UI.Button();
			this.groupCreateUsers = new System.Windows.Forms.GroupBox();
			this.comboUserGroup = new OpenDental.UI.ComboBoxMulti();
			this.label3 = new System.Windows.Forms.Label();
			this.groupMovePats = new System.Windows.Forms.GroupBox();
			this.butMoveSec = new OpenDental.UI.Button();
			this.butProvPick = new OpenDental.UI.Button();
			this.textMoveTo = new System.Windows.Forms.TextBox();
			this.butReassign = new OpenDental.UI.Button();
			this.labelReassign = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.butMovePri = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butStudBulkEdit = new OpenDental.UI.Button();
			this.checkShowDeleted = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butAlphabetize = new OpenDental.UI.Button();
			this.checkShowHidden = new System.Windows.Forms.CheckBox();
			this.groupDentalSchools.SuspendLayout();
			this.groupCreateUsers.SuspendLayout();
			this.groupMovePats.SuspendLayout();
			this.groupBox1.SuspendLayout();
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
			this.butClose.Location = new System.Drawing.Point(885, 665);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(82, 26);
			this.butClose.TabIndex = 8;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(6, 58);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(82, 26);
			this.butDown.TabIndex = 5;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(6, 19);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(82, 26);
			this.butUp.TabIndex = 4;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(885, 522);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(82, 26);
			this.butAdd.TabIndex = 6;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// groupDentalSchools
			// 
			this.groupDentalSchools.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupDentalSchools.Controls.Add(this.label8);
			this.groupDentalSchools.Controls.Add(this.label7);
			this.groupDentalSchools.Controls.Add(this.textProvNum);
			this.groupDentalSchools.Controls.Add(this.label6);
			this.groupDentalSchools.Controls.Add(this.textFirstName);
			this.groupDentalSchools.Controls.Add(this.label4);
			this.groupDentalSchools.Controls.Add(this.textLastName);
			this.groupDentalSchools.Controls.Add(this.radioInstructors);
			this.groupDentalSchools.Controls.Add(this.radioStudents);
			this.groupDentalSchools.Controls.Add(this.radioAll);
			this.groupDentalSchools.Controls.Add(this.comboClass);
			this.groupDentalSchools.Controls.Add(this.label1);
			this.groupDentalSchools.Location = new System.Drawing.Point(703, 12);
			this.groupDentalSchools.Name = "groupDentalSchools";
			this.groupDentalSchools.Size = new System.Drawing.Size(273, 174);
			this.groupDentalSchools.TabIndex = 1;
			this.groupDentalSchools.TabStop = false;
			this.groupDentalSchools.Text = "Dental Schools Search by:";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(116, 48);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(148, 50);
			this.label8.TabIndex = 26;
			this.label8.Text = "These selections will also affect the functionality of the Add button.";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 146);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(90, 18);
			this.label7.TabIndex = 25;
			this.label7.Text = "ProvNum";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProvNum
			// 
			this.textProvNum.Location = new System.Drawing.Point(98, 145);
			this.textProvNum.MaxLength = 15;
			this.textProvNum.Name = "textProvNum";
			this.textProvNum.Size = new System.Drawing.Size(166, 20);
			this.textProvNum.TabIndex = 6;
			this.textProvNum.TextChanged += new System.EventHandler(this.textProvNum_TextChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 124);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(90, 18);
			this.label6.TabIndex = 23;
			this.label6.Text = "First Name";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textFirstName
			// 
			this.textFirstName.Location = new System.Drawing.Point(98, 123);
			this.textFirstName.MaxLength = 15;
			this.textFirstName.Name = "textFirstName";
			this.textFirstName.Size = new System.Drawing.Size(166, 20);
			this.textFirstName.TabIndex = 5;
			this.textFirstName.TextChanged += new System.EventHandler(this.textFirstName_TextChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 102);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(90, 18);
			this.label4.TabIndex = 21;
			this.label4.Text = "Last Name";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textLastName
			// 
			this.textLastName.Location = new System.Drawing.Point(98, 101);
			this.textLastName.MaxLength = 15;
			this.textLastName.Name = "textLastName";
			this.textLastName.Size = new System.Drawing.Size(166, 20);
			this.textLastName.TabIndex = 4;
			this.textLastName.TextChanged += new System.EventHandler(this.textLastName_TextChanged);
			// 
			// radioInstructors
			// 
			this.radioInstructors.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioInstructors.Location = new System.Drawing.Point(6, 80);
			this.radioInstructors.Name = "radioInstructors";
			this.radioInstructors.Size = new System.Drawing.Size(104, 18);
			this.radioInstructors.TabIndex = 3;
			this.radioInstructors.Text = "Instructors";
			this.radioInstructors.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioInstructors.UseVisualStyleBackColor = true;
			this.radioInstructors.Click += new System.EventHandler(this.radioInstructors_Click);
			// 
			// radioStudents
			// 
			this.radioStudents.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioStudents.Location = new System.Drawing.Point(6, 63);
			this.radioStudents.Name = "radioStudents";
			this.radioStudents.Size = new System.Drawing.Size(104, 18);
			this.radioStudents.TabIndex = 2;
			this.radioStudents.Text = "Students";
			this.radioStudents.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioStudents.UseVisualStyleBackColor = true;
			this.radioStudents.Click += new System.EventHandler(this.radioStudents_Click);
			// 
			// radioAll
			// 
			this.radioAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioAll.Checked = true;
			this.radioAll.Location = new System.Drawing.Point(6, 46);
			this.radioAll.Name = "radioAll";
			this.radioAll.Size = new System.Drawing.Size(104, 18);
			this.radioAll.TabIndex = 1;
			this.radioAll.TabStop = true;
			this.radioAll.Text = "All";
			this.radioAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioAll.UseVisualStyleBackColor = true;
			this.radioAll.Click += new System.EventHandler(this.radioAll_Click);
			// 
			// comboClass
			// 
			this.comboClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClass.FormattingEnabled = true;
			this.comboClass.Location = new System.Drawing.Point(98, 19);
			this.comboClass.Name = "comboClass";
			this.comboClass.Size = new System.Drawing.Size(166, 21);
			this.comboClass.TabIndex = 0;
			this.comboClass.SelectionChangeCommitted += new System.EventHandler(this.comboClass_SelectionChangeCommitted);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(7, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(90, 18);
			this.label1.TabIndex = 16;
			this.label1.Text = "Classes";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butCreateUsers
			// 
			this.butCreateUsers.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCreateUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butCreateUsers.Autosize = true;
			this.butCreateUsers.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCreateUsers.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCreateUsers.CornerRadius = 4F;
			this.butCreateUsers.Location = new System.Drawing.Point(182, 42);
			this.butCreateUsers.Name = "butCreateUsers";
			this.butCreateUsers.Size = new System.Drawing.Size(82, 26);
			this.butCreateUsers.TabIndex = 15;
			this.butCreateUsers.Text = "Create";
			this.butCreateUsers.Click += new System.EventHandler(this.butCreateUsers_Click);
			// 
			// groupCreateUsers
			// 
			this.groupCreateUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupCreateUsers.Controls.Add(this.comboUserGroup);
			this.groupCreateUsers.Controls.Add(this.label3);
			this.groupCreateUsers.Controls.Add(this.butCreateUsers);
			this.groupCreateUsers.Location = new System.Drawing.Point(703, 192);
			this.groupCreateUsers.Name = "groupCreateUsers";
			this.groupCreateUsers.Size = new System.Drawing.Size(273, 76);
			this.groupCreateUsers.TabIndex = 2;
			this.groupCreateUsers.TabStop = false;
			this.groupCreateUsers.Text = "Create Users";
			// 
			// comboUserGroup
			// 
			this.comboUserGroup.ArraySelectedIndices = new int[0];
			this.comboUserGroup.BackColor = System.Drawing.SystemColors.Window;
			this.comboUserGroup.Items = ((System.Collections.ArrayList)(resources.GetObject("comboUserGroup.Items")));
			this.comboUserGroup.Location = new System.Drawing.Point(98, 14);
			this.comboUserGroup.Name = "comboUserGroup";
			this.comboUserGroup.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboUserGroup.SelectedIndices")));
			this.comboUserGroup.Size = new System.Drawing.Size(166, 21);
			this.comboUserGroup.TabIndex = 19;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 14);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(91, 18);
			this.label3.TabIndex = 18;
			this.label3.Text = "User Group";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupMovePats
			// 
			this.groupMovePats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupMovePats.Controls.Add(this.butMoveSec);
			this.groupMovePats.Controls.Add(this.butProvPick);
			this.groupMovePats.Controls.Add(this.textMoveTo);
			this.groupMovePats.Controls.Add(this.butReassign);
			this.groupMovePats.Controls.Add(this.labelReassign);
			this.groupMovePats.Controls.Add(this.label2);
			this.groupMovePats.Controls.Add(this.butMovePri);
			this.groupMovePats.Location = new System.Drawing.Point(703, 273);
			this.groupMovePats.Name = "groupMovePats";
			this.groupMovePats.Size = new System.Drawing.Size(273, 132);
			this.groupMovePats.TabIndex = 3;
			this.groupMovePats.TabStop = false;
			this.groupMovePats.Text = "Move Patients";
			// 
			// butMoveSec
			// 
			this.butMoveSec.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMoveSec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butMoveSec.Autosize = true;
			this.butMoveSec.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMoveSec.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMoveSec.CornerRadius = 4F;
			this.butMoveSec.Location = new System.Drawing.Point(182, 46);
			this.butMoveSec.Name = "butMoveSec";
			this.butMoveSec.Size = new System.Drawing.Size(82, 26);
			this.butMoveSec.TabIndex = 15;
			this.butMoveSec.Text = "Move Sec";
			this.butMoveSec.UseVisualStyleBackColor = true;
			this.butMoveSec.Click += new System.EventHandler(this.butMoveSec_Click);
			// 
			// butProvPick
			// 
			this.butProvPick.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butProvPick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butProvPick.Autosize = true;
			this.butProvPick.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butProvPick.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butProvPick.CornerRadius = 4F;
			this.butProvPick.Location = new System.Drawing.Point(237, 17);
			this.butProvPick.Name = "butProvPick";
			this.butProvPick.Size = new System.Drawing.Size(27, 26);
			this.butProvPick.TabIndex = 23;
			this.butProvPick.Text = "...";
			this.butProvPick.Click += new System.EventHandler(this.butProvPick_Click);
			// 
			// textMoveTo
			// 
			this.textMoveTo.Location = new System.Drawing.Point(98, 19);
			this.textMoveTo.MaxLength = 15;
			this.textMoveTo.Name = "textMoveTo";
			this.textMoveTo.ReadOnly = true;
			this.textMoveTo.Size = new System.Drawing.Size(135, 20);
			this.textMoveTo.TabIndex = 22;
			// 
			// butReassign
			// 
			this.butReassign.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butReassign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butReassign.Autosize = true;
			this.butReassign.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butReassign.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butReassign.CornerRadius = 4F;
			this.butReassign.Location = new System.Drawing.Point(182, 98);
			this.butReassign.Name = "butReassign";
			this.butReassign.Size = new System.Drawing.Size(82, 26);
			this.butReassign.TabIndex = 15;
			this.butReassign.Text = "Reassign";
			this.butReassign.Click += new System.EventHandler(this.butReassign_Click);
			// 
			// labelReassign
			// 
			this.labelReassign.Location = new System.Drawing.Point(8, 98);
			this.labelReassign.Name = "labelReassign";
			this.labelReassign.Size = new System.Drawing.Size(168, 31);
			this.labelReassign.TabIndex = 18;
			this.labelReassign.Text = "Reassigns primary provider to most-used provider\r\n";
			this.labelReassign.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(3, 21);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(94, 18);
			this.label2.TabIndex = 18;
			this.label2.Text = "To Provider";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butMovePri
			// 
			this.butMovePri.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMovePri.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butMovePri.Autosize = true;
			this.butMovePri.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMovePri.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMovePri.CornerRadius = 4F;
			this.butMovePri.Location = new System.Drawing.Point(94, 46);
			this.butMovePri.Name = "butMovePri";
			this.butMovePri.Size = new System.Drawing.Size(82, 26);
			this.butMovePri.TabIndex = 15;
			this.butMovePri.Text = "Move Pri";
			this.butMovePri.Click += new System.EventHandler(this.butMovePri_Click);
			// 
			// gridMain
			// 
			this.gridMain.AllowSortingByColumn = true;
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = true;
			this.gridMain.Location = new System.Drawing.Point(7, 31);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(688, 664);
			this.gridMain.TabIndex = 13;
			this.gridMain.Title = "Providers";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableProviderSetup";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butStudBulkEdit
			// 
			this.butStudBulkEdit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butStudBulkEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butStudBulkEdit.Autosize = true;
			this.butStudBulkEdit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butStudBulkEdit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butStudBulkEdit.CornerRadius = 4F;
			this.butStudBulkEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butStudBulkEdit.Location = new System.Drawing.Point(865, 554);
			this.butStudBulkEdit.Name = "butStudBulkEdit";
			this.butStudBulkEdit.Size = new System.Drawing.Size(102, 26);
			this.butStudBulkEdit.TabIndex = 7;
			this.butStudBulkEdit.Text = "Student Bulk Edit";
			this.butStudBulkEdit.Click += new System.EventHandler(this.butStudBulkEdit_Click);
			// 
			// checkShowDeleted
			// 
			this.checkShowDeleted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkShowDeleted.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowDeleted.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowDeleted.Location = new System.Drawing.Point(561, 12);
			this.checkShowDeleted.Name = "checkShowDeleted";
			this.checkShowDeleted.Size = new System.Drawing.Size(134, 14);
			this.checkShowDeleted.TabIndex = 27;
			this.checkShowDeleted.Text = "Show Deleted";
			this.checkShowDeleted.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowDeleted.CheckedChanged += new System.EventHandler(this.checkShowDeleted_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.butAlphabetize);
			this.groupBox1.Controls.Add(this.butUp);
			this.groupBox1.Controls.Add(this.butDown);
			this.groupBox1.Location = new System.Drawing.Point(703, 411);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(273, 91);
			this.groupBox1.TabIndex = 19;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Provider Order";
			// 
			// butAlphabetize
			// 
			this.butAlphabetize.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAlphabetize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAlphabetize.Autosize = true;
			this.butAlphabetize.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAlphabetize.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAlphabetize.CornerRadius = 4F;
			this.butAlphabetize.Location = new System.Drawing.Point(133, 38);
			this.butAlphabetize.Name = "butAlphabetize";
			this.butAlphabetize.Size = new System.Drawing.Size(131, 26);
			this.butAlphabetize.TabIndex = 16;
			this.butAlphabetize.Text = "Alphabetize Providers";
			this.butAlphabetize.Click += new System.EventHandler(this.butAlphabetize_Click);
			// 
			// checkShowHidden
			// 
			this.checkShowHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkShowHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowHidden.Checked = true;
			this.checkShowHidden.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowHidden.Location = new System.Drawing.Point(421, 12);
			this.checkShowHidden.Name = "checkShowHidden";
			this.checkShowHidden.Size = new System.Drawing.Size(134, 14);
			this.checkShowHidden.TabIndex = 28;
			this.checkShowHidden.Text = "Show Hidden";
			this.checkShowHidden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowHidden.Click += new System.EventHandler(this.checkShowHidden_Click);
			// 
			// FormProviderSetup
			// 
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(982, 707);
			this.Controls.Add(this.checkShowHidden);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.checkShowDeleted);
			this.Controls.Add(this.butStudBulkEdit);
			this.Controls.Add(this.groupMovePats);
			this.Controls.Add(this.groupCreateUsers);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.groupDentalSchools);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(553, 660);
			this.Name = "FormProviderSetup";
			this.ShowInTaskbar = false;
			this.Text = "Provider Setup";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormProviderSelect_Closing);
			this.Load += new System.EventHandler(this.FormProviderSetup_Load);
			this.groupDentalSchools.ResumeLayout(false);
			this.groupDentalSchools.PerformLayout();
			this.groupCreateUsers.ResumeLayout(false);
			this.groupMovePats.ResumeLayout(false);
			this.groupMovePats.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormProviderSetup_Load(object sender, System.EventArgs e) {
			//There are two permissions which allow access to this window: Providers and AdminDentalStudents.  SecurityAdmin allows some extra functions.
			if(!Security.IsAuthorized(Permissions.ProviderAlphabetize,true)) {
				butAlphabetize.Enabled=false;
			}
			_listProvs=Providers.GetDeepCopy();
			if(Security.IsAuthorized(Permissions.SecurityAdmin,true)){
				_listUserGroups=UserGroups.GetList();
				for(int i=0;i<_listUserGroups.Count;i++){
					comboUserGroup.Items.Add(new ODBoxItem<UserGroup>(_listUserGroups[i].Description,_listUserGroups[i]));
				}
				if(comboUserGroup.Items.Count>0) {
					comboUserGroup.SetSelected(0,true);
				}
			}
			else{
				groupCreateUsers.Enabled=false;
				groupMovePats.Enabled=false;
			}
			if(PrefC.GetBool(PrefName.EasyHideDentalSchools)){
				groupDentalSchools.Visible=false;
				butStudBulkEdit.Visible=false;
			}
			else{
				comboClass.Items.Add(Lan.g(this,"All"));
				comboClass.SelectedIndex=0;
				_listSchoolClasses=SchoolClasses.GetDeepCopy();
				for(int i=0;i<_listSchoolClasses.Count;i++){
					comboClass.Items.Add(SchoolClasses.GetDescript(_listSchoolClasses[i]));
				}
				butUp.Visible=false;
				butDown.Visible=false;
			}
			checkShowHidden.Checked=PrefC.GetBool(PrefName.EasyHideDentalSchools);
			if(!Security.IsAuthorized(Permissions.PatPriProvEdit,DateTime.MinValue,true,true)) {
				string strToolTip=Lan.g("Security","Not authorized for")+" "+GroupPermissions.GetDesc(Permissions.PatPriProvEdit);
				_priProvEditToolTip.SetToolTip(butReassign,strToolTip);
				_priProvEditToolTip.SetToolTip(butMovePri,strToolTip);
			}
			FillGrid();
		}

		///<summary>Refreshed the table that is showing in the grid.  Also corrects the item order of the provider table.</summary>
		private void RefreshTable(int comboClassSelectedIndex) {
			if(groupDentalSchools.Visible) {
				long schoolClass=0;
				if(comboClassSelectedIndex>0) {
					schoolClass=_listSchoolClasses[comboClassSelectedIndex-1].SchoolClassNum;
				}
				_tableProvs=Providers.RefreshForDentalSchool(schoolClass,textLastName.Text,textFirstName.Text,textProvNum.Text,radioInstructors.Checked,radioAll.Checked);
			}
			else {
				_tableProvs=Providers.RefreshStandard();
				//fix orders
				bool hasChanged=false;
				Provider prov;
				for(int i=0;i<_tableProvs.Rows.Count;i++) {
					if(_tableProvs.Rows[i]["ItemOrder"].ToString()!=i.ToString()) {
						prov=_listProvs.Find(x => x.ProvNum==PIn.Long(_tableProvs.Rows[i]["ProvNum"].ToString()));
						prov.ItemOrder=i;
						Providers.Update(prov);
						_tableProvs.Rows[i]["ItemOrder"]=i.ToString();
						hasChanged=true;
					}
				}
				if(hasChanged) {
					DataValid.SetInvalid(InvalidType.Providers);
				}
			}
		}

		private void FillGrid(bool needsRefresh=true) {
			if(needsRefresh) {
				int selectedIndex=comboClass.SelectedIndex;
				ODProgress.ShowProgressForThread(o => RefreshTable(selectedIndex),this,"Refreshing data...",ProgBarStyle.Marquee,"FormProviderSetup_FillGrid");
			}
			List<long> listSelectedProvNums=gridMain.SelectedIndices.OfType<int>().Select(x => ((Provider)gridMain.Rows[x].Tag).ProvNum).ToList();
			int scroll=gridMain.ScrollValue;
			int sortColIndx=gridMain.SortedByColumnIdx;
			bool isSortAsc=gridMain.SortedIsAscending;
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
				gridMain.Columns.Add(new ODGridColumn(Lan.g("TableProviderSetup","ProvNum"),60,GridSortingStrategy.AmountParse));
			}
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableProviderSetup","Abbrev"),90));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableProviderSetup","Last Name"),90));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableProviderSetup","First Name"),90));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableProviderSetup","User Name"),90));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableProviderSetup","Hidden"),50,HorizontalAlignment.Center));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableProviderSetup","HideOnReports"),100,HorizontalAlignment.Center));
			if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
				gridMain.Columns.Add(new ODGridColumn(Lan.g("TableProviderSetup","Class"),90));
				gridMain.Columns.Add(new ODGridColumn(Lan.g("TableProviderSetup","Instructor"),60,HorizontalAlignment.Center));
			}
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableProviderSetup","PriPats"),50,HorizontalAlignment.Center,GridSortingStrategy.AmountParse));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableProviderSetup","SecPats"),50,HorizontalAlignment.Center,GridSortingStrategy.AmountParse));
			gridMain.Rows.Clear();
			ODGridRow row;
			foreach(DataRow rowCur in _tableProvs.Rows) {
				if(!checkShowHidden.Checked && rowCur["IsHidden"].ToString()=="1") {
					continue;
				}
				row=new ODGridRow();
				if(rowCur["ProvStatus"].ToString()==((int)ProviderStatus.Deleted).ToString()) {
					if(!checkShowDeleted.Checked) {
						continue;
					}
					row.ColorText=Color.Red;
				}
				if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
					row.Cells.Add(rowCur["ProvNum"].ToString());
				}
				row.Cells.Add(rowCur["Abbr"].ToString());
				row.Cells.Add(rowCur["LName"].ToString());
				row.Cells.Add(rowCur["FName"].ToString());
				row.Cells.Add(rowCur["UserName"].ToString());
				row.Cells.Add(rowCur["IsHidden"].ToString()=="1"?"X":"");
				row.Cells.Add(rowCur["IsHiddenReport"].ToString()=="1"?"X":"");
				if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
					row.Cells.Add(rowCur["GradYear"].ToString()!=""?(rowCur["GradYear"].ToString()+"-"+rowCur["Descript"].ToString()):"");
					row.Cells.Add(rowCur["IsInstructor"].ToString()=="1"?"X":"");
				}
				row.Cells.Add(rowCur["PatCountPri"].ToString());
				row.Cells.Add(rowCur["PatCountSec"].ToString());
				long provNumCur=PIn.Long(rowCur["ProvNum"].ToString());
				row.Tag=_listProvs.Find(x => x.ProvNum==provNumCur);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			if(sortColIndx>-1) {
				gridMain.SortForced(sortColIndx,isSortAsc);
			}
			for(int i=0;i<gridMain.Rows.Count;i++) {
				long provNumCur=((Provider)gridMain.Rows[i].Tag).ProvNum;
				if(listSelectedProvNums.Contains(provNumCur)) {
					gridMain.SetSelected(i,true);
				}
			}
			gridMain.ScrollValue=scroll;
		}

		private void comboClass_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void textLastName_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textFirstName_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textProvNum_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void radioAll_Click(object sender,EventArgs e) {
			comboClass.SelectedIndex=0;//Only students are attached to classes
			FillGrid();
		}

		private void radioStudents_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void radioInstructors_Click(object sender,EventArgs e) {
			comboClass.SelectedIndex=0;//Only students are attached to classes
			FillGrid();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			FormProvEdit FormPE=new FormProvEdit();
			FormPE.ProvCur=new Provider();
			FormPE.ProvCur.IsNew=true;
			FormProvStudentEdit FormPSE=new FormProvStudentEdit();
			FormPSE.ProvStudent=new Provider();
			FormPSE.ProvStudent.IsNew=true;
			Provider provCur=new Provider();
			if(groupDentalSchools.Visible) {
				//Dental schools do not worry about item orders.
				if(radioStudents.Checked) {
					if(!Security.IsAuthorized(Permissions.AdminDentalStudents)) {
						return;
					}
					if(comboClass.SelectedIndex==0) {
						MsgBox.Show(this,"A class must be selected from the drop down box before a new student can be created");
						return;
					}
					FormPSE.ProvStudent.SchoolClassNum=_listSchoolClasses[comboClass.SelectedIndex-1].SchoolClassNum;
					FormPSE.ProvStudent.FName=textFirstName.Text;
					FormPSE.ProvStudent.LName=textLastName.Text;
				}
				if(radioInstructors.Checked && !Security.IsAuthorized(Permissions.AdminDentalInstructors)) {
					return;
				}
				FormPE.ProvCur.IsInstructor=radioInstructors.Checked;
				FormPE.ProvCur.FName=textFirstName.Text;
				FormPE.ProvCur.LName=textLastName.Text;
			}
			else {//Not using Dental Schools feature.
				if(gridMain.SelectedIndices.Length>0) {//place new provider after the first selected index. No changes are made to DB until after provider is actually inserted.
					FormPE.ProvCur.ItemOrder=((Provider)gridMain.Rows[gridMain.SelectedIndices[0]].Tag).ItemOrder;//now two with this itemorder
				}
				else if(gridMain.Rows.Count>0) {
					FormPE.ProvCur.ItemOrder=((Provider)gridMain.Rows[gridMain.Rows.Count-1].Tag).ItemOrder+1;
				}
				else {
					FormPE.ProvCur.ItemOrder=0;
				}
			}
			if(!radioStudents.Checked) {
				if(radioInstructors.Checked && PrefC.GetLong(PrefName.SecurityGroupForInstructors)==0) {
					MsgBox.Show(this,"Security Group for Instructors must be set from the Dental School Setup window before adding instructors.");
					return;
				}
				FormPE.IsNew=true;
				FormPE.ShowDialog();
				if(FormPE.DialogResult!=DialogResult.OK) {
					return;
				}
				provCur=FormPE.ProvCur;
			}
			else {
				if(radioStudents.Checked && PrefC.GetLong(PrefName.SecurityGroupForStudents)==0) {
					MsgBox.Show(this,"Security Group for Students must be set from the Dental School Setup window before adding students.");
					return;
				}
				FormPSE.ShowDialog();
				if(FormPSE.DialogResult!=DialogResult.OK) {
					return;
				}
				provCur=FormPSE.ProvStudent;
			}
			//new provider has already been inserted into DB from above
			Providers.MoveDownBelow(provCur);//safe to run even if none selected.
			_hasChanged=true;
			Cache.Refresh(InvalidType.Providers);
			_listProvs=Providers.GetDeepCopy();
			FillGrid();
			gridMain.ScrollToEnd();//should change this to scroll to the same place as before.
			for(int i=0;i<gridMain.Rows.Count;i++) {//Providers.ListShallow.Count;i++) {
				if(((Provider)gridMain.Rows[i].Tag).ProvNum==provCur.ProvNum) {
					gridMain.SetSelected(i,true);
					break;
				}
			}
		}

		private void butStudBulkEdit_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AdminDentalStudents)) {
				return;
			}
			FormProvStudentBulkEdit FormPSBE=new FormProvStudentBulkEdit();
			FormPSBE.ShowDialog();
		}

		///<summary>Won't be visible if using Dental Schools.  So list will be unfiltered and ItemOrders won't get messed up.</summary>
		private void butUp_Click(object sender, System.EventArgs e) {
			if(gridMain.SelectedIndices.Length!=1) {
				MsgBox.Show(this,"Please select exactly one provider first.");
				return;
			}
			if(gridMain.SelectedIndices[0]==0) {//already at top
				return;
			}
			//Note: sourceProv will always be the selected prov, but destProv isn't necessarily the provider that is +1 idx in the table.
			//The grid is filtered, the table is not.
			//The provider's position in the table needs to reflect their item orders.
			Provider sourceProv=((Provider)gridMain.Rows[gridMain.SelectedIndices[0]].Tag);
			Provider destProv=((Provider)gridMain.Rows[gridMain.SelectedIndices[0]-1].Tag);
			int sourceIdx=sourceProv.ItemOrder;
			sourceProv.ItemOrder=destProv.ItemOrder;
			Providers.Update(sourceProv);
			destProv.ItemOrder=sourceIdx;
			Providers.Update(destProv);	
			_hasChanged=true;
			int selectedIdx=gridMain.SelectedIndices[0];
			SwapGridMainLocations(selectedIdx,selectedIdx-1);
			gridMain.SetSelected(selectedIdx-1,true);
		}

		///<summary>Won't be visible if using Dental Schools.  So list will be unfiltered and ItemOrders won't get messed up.</summary>
		private void butDown_Click(object sender, System.EventArgs e) {
			if(gridMain.SelectedIndices.Length!=1) {
				MsgBox.Show(this,"Please select exactly one provider first.");
				return;
			}
			if(gridMain.SelectedIndices[0]==gridMain.Rows.Count-1) {//already at bottom
				return;
			}
			//Note: sourceProv will always be the selected prov, but destProv isn't necessarily the provider that is +1 idx in the table.
			//The grid is filtered, the table is not.
			//The provider's position in the table needs to reflect their item orders.
			Provider sourceProv=((Provider)gridMain.Rows[gridMain.SelectedIndices[0]].Tag);
			Provider destProv=((Provider)gridMain.Rows[gridMain.SelectedIndices[0]+1].Tag);
			int sourceIdx=sourceProv.ItemOrder;
			sourceProv.ItemOrder=destProv.ItemOrder;
			Providers.Update(sourceProv);
			destProv.ItemOrder=sourceIdx;
			Providers.Update(destProv);
			_hasChanged=true;		
			int selectedIdx=gridMain.SelectedIndices[0];	
			SwapGridMainLocations(selectedIdx,selectedIdx+1);
			gridMain.SetSelected(selectedIdx+1,true);
		}

		private void SwapGridMainLocations(int indxMoveFrom, int indxMoveTo) {
			gridMain.BeginUpdate();
			ODGridRow dataRow=gridMain.Rows[indxMoveFrom];
			gridMain.Rows.RemoveAt(indxMoveFrom);
			gridMain.Rows.Insert(indxMoveTo,dataRow);
			gridMain.EndUpdate();		
		}

		private void checkShowHidden_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Provider provSelected=(Provider)gridMain.Rows[gridMain.SelectedIndices[0]].Tag;
			if(!PrefC.GetBool(PrefName.EasyHideDentalSchools) && Providers.IsAttachedToUser(provSelected.ProvNum)) {//Dental schools is turned on and the provider selected is attached to a user.
				//provSelected could be a provider or a student at this point.
				if(!provSelected.IsInstructor && !Security.IsAuthorized(Permissions.AdminDentalStudents)) {
					return;
				}
				if(provSelected.IsInstructor && !Security.IsAuthorized(Permissions.AdminDentalInstructors)) {
					return;
				}
				if(!radioStudents.Checked) {
					FormProvEdit FormPE=new FormProvEdit();
					FormPE.ProvCur=(Provider)gridMain.Rows[e.Row].Tag;
					FormPE.ShowDialog();
					if(FormPE.DialogResult!=DialogResult.OK) {
						return;
					}
				}
				else {
					FormProvStudentEdit FormPSE=new FormProvStudentEdit();
					FormPSE.ProvStudent=(Provider)gridMain.Rows[e.Row].Tag;
					FormPSE.ShowDialog();
					if(FormPSE.DialogResult!=DialogResult.OK) {
						return;
					}
				}
			}
			else {//No Dental Schools or provider is not attached to a user
				FormProvEdit FormPE=new FormProvEdit();
				FormPE.ProvCur=(Provider)gridMain.Rows[e.Row].Tag;
				FormPE.ShowDialog();
				if(FormPE.DialogResult!=DialogResult.OK) {
					return;
				}
			}
			_hasChanged=true;
			Cache.Refresh(InvalidType.Providers);
			_listProvs=Providers.GetDeepCopy();
			FillGrid();
		}

		private void butProvPick_Click(object sender,EventArgs e) {
			//This button is used instead of a dropdown because the order of providers can frequently change in the grid.
			FormProviderPick formPick=new FormProviderPick();
			formPick.IsNoneAvailable=true;
			formPick.ShowDialog();
			if(formPick.DialogResult!=DialogResult.OK) {
				return;
			}
			_provNumMoveTo=formPick.SelectedProvNum;
			if(_provNumMoveTo>0) {
				Provider provTo=_listProvs.Find(x => x.ProvNum==_provNumMoveTo);
				textMoveTo.Text=provTo.GetLongDesc();
			}
			else {
				textMoveTo.Text="None";
			}
		}

		///<summary>Not possible if no security admin or no PatPriProvEdit permission.</summary>
		private void butMovePri_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.PatPriProvEdit)) {//shouldn't be possible, button should be disabled if not authorized, just in case
				return;
			}
			if(gridMain.SelectedIndices.Length<1) {
				MsgBox.Show(this,"You must select at least one provider to move patients from.");
				return;
			}
			List<Provider> listProvsFrom=gridMain.SelectedIndices.OfType<int>().Select(x => (Provider)gridMain.Rows[x].Tag).ToList();
			if(_provNumMoveTo==-1){
				MsgBox.Show(this,"You must pick a 'To' provider in the box above to move patients to.");
				return;
			}
			if(_provNumMoveTo==0) {
				MsgBox.Show(this,"'None' is not a valid primary provider.");
				return;
			}
			Provider provTo=_listProvs.FirstOrDefault(x => x.ProvNum==_provNumMoveTo);
			if(provTo==null) {
				MsgBox.Show(this,"The provider could not be found.");
				return;
			}
			Action actionCloseProgress=ODProgressOld.ShowProgressStatus("ProviderReassign",this,Lan.g(this,"Gathering patient data")+"...");
			Cursor=Cursors.WaitCursor;
			//get pats with original (from) priprov
			Dictionary<long,List<long>> dictPriProvPats=Patients.GetPatNumsByPriProvs(listProvsFrom.Select(x => x.ProvNum).ToList()).Select()
				.GroupBy(x => PIn.Long(x["PriProv"].ToString()),x => PIn.Long(x["PatNum"].ToString()))
				.ToDictionary(x => x.Key,x => x.ToList());
			actionCloseProgress?.Invoke();
			Cursor=Cursors.Default;
			int totalPatCount=dictPriProvPats.Sum(x => x.Value.Count);
			if(totalPatCount==0) {
				MsgBox.Show(this,"The selected providers are not primary providers for any patients.");
				return;
			}
			string strProvFromDesc=string.Join(", ",listProvsFrom.FindAll(x => dictPriProvPats.ContainsKey(x.ProvNum)).Select(x => x.Abbr));
			string strProvToDesc=provTo.Abbr;
			string msg=Lan.g(this,"Move all primary patients to")+" "+strProvToDesc+" "+Lan.g(this,"from the following providers")+": "+strProvFromDesc+"?";
			if(MessageBox.Show(msg,"",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
				return;
			}
			actionCloseProgress=ODProgressOld.ShowProgressStatus("ProviderReassign",this,Lan.g(this,"Moving patients")+"...");
			Cursor=Cursors.WaitCursor;
			int patsMoved=0;
			List<Action> listActions=dictPriProvPats.Select(x => new Action(() => {
				patsMoved+=x.Value.Count;
				ODEvent.Fire(new ODEventArgs("ProviderReassign",Lan.g(this,"Moving patients")+": "+patsMoved+" out of "+totalPatCount));
				Patients.ChangePrimaryProviders(x.Key,provTo.ProvNum);//update all priprovs to new provider
				SecurityLogs.MakeLogEntry(Permissions.PatPriProvEdit,0,"Primary provider changed for "+x.Value.Count+" patients from "
					+Providers.GetLongDesc(x.Key)+" to "+provTo.GetLongDesc()+".");
			})).ToList();
			ODThread.RunParallel(listActions,TimeSpan.FromMinutes(2));
			actionCloseProgress?.Invoke();
			Cursor=Cursors.Default;
			_hasChanged=true;
			FillGrid();
		}

		///<summary>Not possible if no security admin.</summary>
		private void butMoveSec_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length<1) {
				MsgBox.Show(this,"You must select at least one provider to move patients from.");
				return;
			}
			List<Provider> listProvsFrom=gridMain.SelectedIndices.OfType<int>().Select(x => (Provider)gridMain.Rows[x].Tag).ToList();
			if(_provNumMoveTo==-1) {
				MsgBox.Show(this,"You must pick a 'To' provider in the box above to move patients to.");
				return;
			}
			Provider provTo=_listProvs.FirstOrDefault(x => x.ProvNum==_provNumMoveTo);
			string msg;
			if(provTo==null) {
				msg=Lan.g(this,"Remove all secondary patients from the selected providers")+"?";
			}
			else {
				string strProvsFrom=string.Join(", ",listProvsFrom.Select(x => x.Abbr));
				msg=Lan.g(this,"Move all secondary patients to")+" "+provTo.Abbr+" "+Lan.g(this,"from the following providers")+": "+strProvsFrom+"?";
			}
			if(MessageBox.Show(msg,"",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
				return;
			}
			//display the progress bar, updated by events firing in threads
			Action actionCloseProgress=ODProgressOld.ShowProgressStatus("ProviderReassign",this,Lan.g(this,"Reassigning patients")+"...");
			Cursor=Cursors.WaitCursor;
			List<Action> listActions=listProvsFrom.Select(x => new Action(() => { Patients.ChangeSecondaryProviders(x.ProvNum,provTo?.ProvNum??0); })).ToList();
			ODThread.RunParallel(listActions,TimeSpan.FromMinutes(2));//each group of actions gets 2 minutes
			actionCloseProgress?.Invoke();
			Cursor=Cursors.Default;
			_hasChanged=true;
			FillGrid();
		}

		private void butReassign_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.PatPriProvEdit)) {//shouldn't be possible, button should be disabled if not authorized, just in case
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Ready to look for possible reassignments.  This will take a few minutes, and may make the program "
				+"unresponsive on other computers during that time.  You will be given one more chance after this to cancel before changes are made to the "
				+"database.  Continue?"))
			{
				return;
			}
			Action actionCloseProgress=ODProgressOld.ShowProgressStatus("ProviderReassign",this,Lan.g(this,"Gathering patient and provider details")+"...");
			Cursor=Cursors.WaitCursor;
			List<long> listProvNumsFrom=gridMain.SelectedIndices.OfType<int>().Select(x => ((Provider)gridMain.Rows[x].Tag).ProvNum).ToList();
			DataTable tablePatNums=Patients.GetPatNumsByPriProvs(listProvNumsFrom);//list of all patients who are using the selected providers.
			if(tablePatNums.Rows.Count==0 || gridMain.Rows.Count==0 || listProvNumsFrom.Count==0) {
				actionCloseProgress?.Invoke();
				Cursor=Cursors.Default;
				MsgBox.Show(this,"No patients to reassign.");
				return;
			}
			//Convert DataTable to a var that is a list of objects with PatNum and ProvNum properties.  var is temporary, used to create a list of actions
			var listPatProvFrom=tablePatNums.Select().Select(x => new { PatNum=PIn.Long(x["PatNum"].ToString()),ProvNum=PIn.Long(x["PriProv"].ToString()) }).ToList();
			//Create dictionary of key=PatNum, value=ProvNum for the pat's most used provider. Most used is the prov with the most completed procs with a
			//tie broken by the most recent ProcDate.
			Dictionary<long,long> dictPatProvTo=Procedures.GetTablePatProvUsed(listPatProvFrom.Select(x=>x.PatNum).ToList()).Select()
				.GroupBy(x => PIn.Long(x["PatNum"].ToString()),x => new {
					ProvNum =PIn.Long(x["ProvNum"].ToString()),
					procCount =PIn.Int(x["procCount"].ToString()),
					maxProcDate =PIn.Date(x["maxProcDate"].ToString()) })
				.ToDictionary(x => x.Key,x => x.OrderByDescending(y => y.procCount).ThenByDescending(y => y.maxProcDate).First().ProvNum);
			//Remove all pats who don't have any procedures or whose PriProv is already the most used prov
			listPatProvFrom.RemoveAll(x => !dictPatProvTo.ContainsKey(x.PatNum) || x.ProvNum==dictPatProvTo[x.PatNum]);
			actionCloseProgress?.Invoke();
			Cursor=Cursors.Default;
			string msg=Lan.g(this,"You are about to reassign")+" "+listPatProvFrom.Count+" "+Lan.g(this,"patients to different providers.  Continue?");
			if(MessageBox.Show(msg,"",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
				return;
			}
			//Create list of actions that will set the pat's PriProv to the most used, create a securitylog entry, and update the progress bar if necessary
			int patsReassigned=0;
			List<Action> listActions=listPatProvFrom.GroupBy(x=>new { From=x.ProvNum,To=dictPatProvTo[x.PatNum] })
				.ToDictionary(x => x.Key,x=>x.Select(y=>y.PatNum).ToList())
				.Select(x => new Action(() => {
					patsReassigned+=x.Value.Count;
					ODEvent.Fire(new ODEventArgs("ProviderReassign",Lan.g(this,"Reassigning patients")+": "+patsReassigned+" out of "+listPatProvFrom.Count));
					Patients.ReassignProv(x.Key.To,x.Value);
					SecurityLogs.MakeLogEntry(Permissions.PatPriProvEdit,0,"Primary provider changed for "+x.Value.Count+" patients from "
						+Providers.GetLongDesc(x.Key.From)+" to "+Providers.GetLongDesc(x.Key.To)+".");
				})).ToList();
			//display the progress bar, updated by events firing in threads
			actionCloseProgress=ODProgressOld.ShowProgressStatus("ProviderReassign",this,Lan.g(this,"Reassigning patients")+"...");
			Cursor=Cursors.WaitCursor;
			ODThread.RunParallel(listActions,TimeSpan.FromMinutes(2));//each group of actions gets 2 minutes
			actionCloseProgress?.Invoke();
			Cursor=Cursors.Default;
			//changed=true;//We didn't change any providers
			FillGrid();
		}

		///<summary>Not possible if no security admin.</summary>
		private void butCreateUsers_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0){
				MsgBox.Show(this,"Please select one or more providers first.");
				return;
			}
			for(int i=0;i<gridMain.SelectedIndices.Length;i++){
				if(Providers.IsAttachedToUser(((Provider)gridMain.Rows[gridMain.SelectedIndices[0]].Tag).ProvNum)) {
					MsgBox.Show(this,"Not allowed to create users on providers which already have users.");
					return;
				}
			}
			if(comboUserGroup.ListSelectedItems.Count == 0){
				MsgBox.Show(this,"Please select at least one User Group first.");
				return;
			}
			for(int i=0;i<gridMain.SelectedIndices.Length;i++){
				Provider prov=(Provider)gridMain.Rows[gridMain.SelectedIndices[0]].Tag;
				Userod user=new Userod();
				user.ProvNum=prov.ProvNum;
				user.UserName=GetUniqueUserName(prov.LName,prov.FName);
				user.Password=Userods.HashPassword(user.UserName);
				try{
					Userods.Insert(user,comboUserGroup.ListSelectedItems.OfType<ODBoxItem<UserGroup>>().Select(x => x.Tag.UserGroupNum).ToList());
				}
				catch(ApplicationException ex){
					MessageBox.Show(ex.Message);
					_hasChanged=true;
					return;
				}
			}
			_hasChanged=true;
			FillGrid();
		}

		private string GetUniqueUserName(string lname,string fname){
			string name=lname;
			if(fname.Length>0){
				name+=fname.Substring(0,1);
			}
			if(Userods.IsUserNameUnique(name,0,false)){
				return name;
			}
			int fnameI=1;
			while(fnameI<fname.Length){
				name+=fname.Substring(fnameI,1);
				if(Userods.IsUserNameUnique(name,0,false)) {
					return name;
				}
				fnameI++;
			}
			//should be entire lname+fname at this point, but still not unique
			do{
				name+="x";
			}
			while(!Userods.IsUserNameUnique(name,0,false));
			return name;
		}

		private void checkShowDeleted_CheckedChanged(object sender,EventArgs e) {
				FillGrid(checkShowDeleted.Checked);
		}

		private void butAlphabetize_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.ProviderAlphabetize,false)) {
				return;//should not be possible, button should be disabled. This is just in case.
			}
			if(!MsgBox.Show(this,true,"Alphabetize all providers (by Abbrev) and move hidden providers to the bottom, followed by all non-person providers? This cannot be undone.")) {
				return;
			}
			//According to original task the form should display providers in the following order:
			//1) Is a person, not hidden -sorted alphabetically by abbreviation
			//2) Is not a person, not hidden - sorted alphabetically by abbreviation
			//3) all hidden providers, sorted alphabetically by abbreviation (is a person and is not a person would be mixed)
			List<Provider> listProvsAll = Providers.GetAll()
				.OrderBy(x => x.IsHidden)
				.ThenBy(x => x.IsHidden || x.IsNotPerson)
				.ThenBy(x => x.GetAbbr()).ToList();
			bool changed = false; 
			for(int i = 0;i<listProvsAll.Count;i++) {
				Provider prov = listProvsAll[i];
				if(prov.ItemOrder==i) {
					continue;
				}
				prov.ItemOrder=i;
				Providers.Update(prov);
				changed=true;
			}
			if(changed) {
				Signalods.SetInvalid(InvalidType.Providers);
			}
			_listProvs=listProvsAll;
			FillGrid();
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormProviderSelect_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			string duplicates=Providers.GetDuplicateAbbrs();
			if(duplicates!="" && PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
				if(MessageBox.Show(Lan.g(this,"Warning.  The following abbreviations are duplicates.  Continue anyway?\r\n")+duplicates,
					"",MessageBoxButtons.OKCancel)!=DialogResult.OK)
				{
					e.Cancel=true;
					return;
				}
			}
			if(_hasChanged){
				DataValid.SetInvalid(InvalidType.Providers, InvalidType.Security);
			}
			//SecurityLogs.MakeLogEntry("Providers","Altered Providers",user);
		}
	}
}
