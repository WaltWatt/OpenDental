/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public class FormProcCodes:ODForm {
		private System.ComponentModel.Container components = null;
		///<summary>If IsSelectionMode=true and DialogResult=OK, then this will contain the selected CodeNum.</summary>
		public long SelectedCodeNum;
		//public string SelectedADA;	
		private bool changed;
		///<summary>Set to true externally in order to let user select one procedure code.</summary>
		public bool IsSelectionMode;
		///<summary>The list of definitions that is currently showing in the category list.</summary>
		private Def[] CatList;
		private UI.Button butOK;
		private UI.Button butCancel;
		private UI.Button butEditFeeSched;
		private UI.Button butTools;
		private GroupBox groupFeeScheds;
		private Label label2;
		private ListBox listCategories;
		private Label label3;
		private Label label1;
		private UI.Button butEditCategories;
		private CheckBox checkShowHidden;
		private Label label4;
		private UI.Button butAll;
		private TextBox textCode;
		private TextBox textAbbreviation;
		private TextBox textDescription;
		private UI.Button butShowHiddenDefault;
		private GroupBox groupBox1;
		private ODGrid gridMain;
		private Label label5;
		private UI.Button butNew;
		private UI.Button butExport;
		private UI.Button butImport;
		private UI.Button butProcTools;
		private GroupBox groupProcCodeSetup;
		private GroupBox groupBox2;
		private ComboBox comboProvider1;
		private ComboBox comboClinic1;
		private ComboBox comboFeeSched1;
		private GroupBox groupBox3;
		private ComboBox comboProvider2;
		private ComboBox comboClinic2;
		private ComboBox comboFeeSched2;
		private GroupBox groupBox4;
		private ComboBox comboProvider3;
		private ComboBox comboClinic3;
		private ComboBox comboFeeSched3;
		private List<FeeSched> _listFeeScheds; //Note to reviewer: I'm doing these to avoid using calls like FeeSchedC.ListShort[idx] later.
		private List<Clinic> _listClinics;
		private List<Provider> _listProviders;
		private Label labelSched1;
		private Label labelClinic1;
		private Label labelProvider1;
		private Label labelSched2;
		private Label labelClinic2;
		private Label labelProvider2;
		private Label labelSched3;
		private Label labelClinic3;
		private Label labelProvider3;
		private GroupBox groupBox5;
		private Label label22;
		private Label label20;
		private Label label19;
		private Label label21;
		private UI.Button butPickProv1;
		private UI.Button butPickClinic1;
		private UI.Button butPickSched1;
		private UI.Button butPickProv2;
		private UI.Button butPickClinic2;
		private UI.Button butPickSched2;
		private UI.Button butPickProv3;
		private UI.Button butPickClinic3;
		private UI.Button butPickSched3;
		private Color _colorClinic;
		private Color _colorProv;
		private Color _colorProvClinic;
		private Color _colorDefault;
		private System.Windows.Forms.Button butColorClinicProv;
		private System.Windows.Forms.Button butColorProvider;
		private System.Windows.Forms.Button butColorClinic;
		private System.Windows.Forms.Button butColorDefault;
		///<summary>Local copy of a FeeCache class that contains all fees, stored in memory for easy access and editing.  Synced on form closing.</summary>
		private FeeCache _feeCache;
		private ComboBox comboSort;
		private Label label6;
		public ProcCodeListSort ProcCodeSort;
		/// <summary> List should contain two logs per fee because we are inserting two security logs everytime we update a fee.</summary>
		private Dictionary<long,List<SecurityLog>> _dictFeeLogs;
		private bool _canShowHidden;
		///<summary>Contains all of the procedure codes that were selected if IsSelectionMode is true.
		///If IsSelectionMode is true and this list is prefilled with procedure codes then the grid will preselect as many codes as possible.
		///It is not guaranteed that all procedure codes will be selected due to filters.
		///This list should only be read from externally after DialogResult.OK has been returned.</summary>
		public List<ProcedureCode> ListSelectedProcCodes=new List<ProcedureCode>();
		///<summary>Set to true when IsSelectionMode is true and the user should be able to select multiple procedure codes instead of just one.
		///ListSelectedProcCodes will contain all of the procedure codes that the user selected.</summary>
		public bool AllowMultipleSelections;

		///<summary>When canShowHidden is true to the "Hidden" checkbox and "default" button are visible.</summary>
		public FormProcCodes(bool canShowHidden=false) {
			InitializeComponent();// Required for Windows Form Designer support
			Lan.F(this);
			_canShowHidden=canShowHidden;
		}

		///<summary></summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProcCodes));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butEditFeeSched = new OpenDental.UI.Button();
			this.butTools = new OpenDental.UI.Button();
			this.groupFeeScheds = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.listCategories = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.butEditCategories = new OpenDental.UI.Button();
			this.checkShowHidden = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.butAll = new OpenDental.UI.Button();
			this.textCode = new System.Windows.Forms.TextBox();
			this.textAbbreviation = new System.Windows.Forms.TextBox();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.butShowHiddenDefault = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.label5 = new System.Windows.Forms.Label();
			this.butNew = new OpenDental.UI.Button();
			this.butExport = new OpenDental.UI.Button();
			this.butImport = new OpenDental.UI.Button();
			this.butProcTools = new OpenDental.UI.Button();
			this.groupProcCodeSetup = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.butPickProv1 = new OpenDental.UI.Button();
			this.butPickClinic1 = new OpenDental.UI.Button();
			this.butPickSched1 = new OpenDental.UI.Button();
			this.labelSched1 = new System.Windows.Forms.Label();
			this.labelClinic1 = new System.Windows.Forms.Label();
			this.labelProvider1 = new System.Windows.Forms.Label();
			this.comboProvider1 = new System.Windows.Forms.ComboBox();
			this.comboClinic1 = new System.Windows.Forms.ComboBox();
			this.comboFeeSched1 = new System.Windows.Forms.ComboBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.butPickProv2 = new OpenDental.UI.Button();
			this.butPickClinic2 = new OpenDental.UI.Button();
			this.butPickSched2 = new OpenDental.UI.Button();
			this.labelSched2 = new System.Windows.Forms.Label();
			this.comboProvider2 = new System.Windows.Forms.ComboBox();
			this.labelClinic2 = new System.Windows.Forms.Label();
			this.comboClinic2 = new System.Windows.Forms.ComboBox();
			this.labelProvider2 = new System.Windows.Forms.Label();
			this.comboFeeSched2 = new System.Windows.Forms.ComboBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.butPickProv3 = new OpenDental.UI.Button();
			this.butPickClinic3 = new OpenDental.UI.Button();
			this.butPickSched3 = new OpenDental.UI.Button();
			this.labelSched3 = new System.Windows.Forms.Label();
			this.comboProvider3 = new System.Windows.Forms.ComboBox();
			this.labelClinic3 = new System.Windows.Forms.Label();
			this.labelProvider3 = new System.Windows.Forms.Label();
			this.comboClinic3 = new System.Windows.Forms.ComboBox();
			this.comboFeeSched3 = new System.Windows.Forms.ComboBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.butColorClinicProv = new System.Windows.Forms.Button();
			this.butColorProvider = new System.Windows.Forms.Button();
			this.butColorClinic = new System.Windows.Forms.Button();
			this.butColorDefault = new System.Windows.Forms.Button();
			this.label21 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.comboSort = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.groupFeeScheds.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupProcCodeSetup.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(794, 668);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 20;
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
			this.butCancel.Location = new System.Drawing.Point(889, 668);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 21;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butEditFeeSched
			// 
			this.butEditFeeSched.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEditFeeSched.Autosize = true;
			this.butEditFeeSched.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditFeeSched.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditFeeSched.CornerRadius = 4F;
			this.butEditFeeSched.Location = new System.Drawing.Point(12, 16);
			this.butEditFeeSched.Name = "butEditFeeSched";
			this.butEditFeeSched.Size = new System.Drawing.Size(81, 26);
			this.butEditFeeSched.TabIndex = 18;
			this.butEditFeeSched.Text = "Fee Scheds";
			this.butEditFeeSched.Click += new System.EventHandler(this.butEditFeeSched_Click);
			// 
			// butTools
			// 
			this.butTools.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butTools.Autosize = true;
			this.butTools.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTools.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTools.CornerRadius = 4F;
			this.butTools.Location = new System.Drawing.Point(109, 16);
			this.butTools.Name = "butTools";
			this.butTools.Size = new System.Drawing.Size(81, 26);
			this.butTools.TabIndex = 19;
			this.butTools.Text = "Fee Tools";
			this.butTools.Click += new System.EventHandler(this.butTools_Click);
			// 
			// groupFeeScheds
			// 
			this.groupFeeScheds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupFeeScheds.Controls.Add(this.butTools);
			this.groupFeeScheds.Controls.Add(this.butEditFeeSched);
			this.groupFeeScheds.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupFeeScheds.Location = new System.Drawing.Point(778, 603);
			this.groupFeeScheds.Name = "groupFeeScheds";
			this.groupFeeScheds.Size = new System.Drawing.Size(200, 51);
			this.groupFeeScheds.TabIndex = 14;
			this.groupFeeScheds.TabStop = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(3, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 20);
			this.label2.TabIndex = 17;
			this.label2.Text = "By Descript";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// listCategories
			// 
			this.listCategories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listCategories.FormattingEnabled = true;
			this.listCategories.Location = new System.Drawing.Point(10, 149);
			this.listCategories.Name = "listCategories";
			this.listCategories.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listCategories.Size = new System.Drawing.Size(145, 355);
			this.listCategories.TabIndex = 15;
			this.listCategories.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listCategories_MouseUp);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(3, 68);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(79, 20);
			this.label3.TabIndex = 19;
			this.label3.Text = "By Code";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(7, 123);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 23);
			this.label1.TabIndex = 16;
			this.label1.Text = "By Category";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butEditCategories
			// 
			this.butEditCategories.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEditCategories.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butEditCategories.Autosize = true;
			this.butEditCategories.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditCategories.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditCategories.CornerRadius = 4F;
			this.butEditCategories.Location = new System.Drawing.Point(10, 510);
			this.butEditCategories.Name = "butEditCategories";
			this.butEditCategories.Size = new System.Drawing.Size(94, 26);
			this.butEditCategories.TabIndex = 23;
			this.butEditCategories.Text = "Edit Categories";
			this.butEditCategories.Click += new System.EventHandler(this.butEditCategories_Click);
			// 
			// checkShowHidden
			// 
			this.checkShowHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkShowHidden.Location = new System.Drawing.Point(10, 542);
			this.checkShowHidden.Name = "checkShowHidden";
			this.checkShowHidden.Size = new System.Drawing.Size(90, 17);
			this.checkShowHidden.TabIndex = 24;
			this.checkShowHidden.Text = "Show Hidden";
			this.checkShowHidden.UseVisualStyleBackColor = true;
			this.checkShowHidden.Click += new System.EventHandler(this.checkShowHidden_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(3, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(79, 20);
			this.label4.TabIndex = 22;
			this.label4.Text = "By Abbrev";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butAll
			// 
			this.butAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAll.Autosize = true;
			this.butAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAll.CornerRadius = 4F;
			this.butAll.Location = new System.Drawing.Point(93, 123);
			this.butAll.Name = "butAll";
			this.butAll.Size = new System.Drawing.Size(62, 25);
			this.butAll.TabIndex = 22;
			this.butAll.Text = "All";
			this.butAll.Click += new System.EventHandler(this.butAll_Click);
			// 
			// textCode
			// 
			this.textCode.Location = new System.Drawing.Point(82, 69);
			this.textCode.Name = "textCode";
			this.textCode.Size = new System.Drawing.Size(73, 20);
			this.textCode.TabIndex = 2;
			this.textCode.TextChanged += new System.EventHandler(this.textCode_TextChanged);
			// 
			// textAbbreviation
			// 
			this.textAbbreviation.Location = new System.Drawing.Point(82, 17);
			this.textAbbreviation.Name = "textAbbreviation";
			this.textAbbreviation.Size = new System.Drawing.Size(73, 20);
			this.textAbbreviation.TabIndex = 0;
			this.textAbbreviation.TextChanged += new System.EventHandler(this.textAbbreviation_TextChanged);
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(82, 43);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(73, 20);
			this.textDescription.TabIndex = 1;
			this.textDescription.TextChanged += new System.EventHandler(this.textDescription_TextChanged);
			// 
			// butShowHiddenDefault
			// 
			this.butShowHiddenDefault.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butShowHiddenDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butShowHiddenDefault.Autosize = true;
			this.butShowHiddenDefault.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShowHiddenDefault.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShowHiddenDefault.CornerRadius = 4F;
			this.butShowHiddenDefault.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butShowHiddenDefault.Location = new System.Drawing.Point(100, 539);
			this.butShowHiddenDefault.Name = "butShowHiddenDefault";
			this.butShowHiddenDefault.Size = new System.Drawing.Size(56, 20);
			this.butShowHiddenDefault.TabIndex = 25;
			this.butShowHiddenDefault.Text = "default";
			this.butShowHiddenDefault.Click += new System.EventHandler(this.butShowHiddenDefault_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.comboSort);
			this.groupBox1.Controls.Add(this.butShowHiddenDefault);
			this.groupBox1.Controls.Add(this.textDescription);
			this.groupBox1.Controls.Add(this.textAbbreviation);
			this.groupBox1.Controls.Add(this.textCode);
			this.groupBox1.Controls.Add(this.butAll);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.checkShowHidden);
			this.groupBox1.Controls.Add(this.butEditCategories);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.listCategories);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(2, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(165, 571);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Search";
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(170, 8);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridMain.Size = new System.Drawing.Size(604, 686);
			this.gridMain.TabIndex = 19;
			this.gridMain.Title = "Procedures";
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableProcedures";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellLeave += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellLeave);
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Location = new System.Drawing.Point(779, 9);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(199, 17);
			this.label5.TabIndex = 21;
			this.label5.Text = "Compare Fee Schedules";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butNew
			// 
			this.butNew.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butNew.Autosize = true;
			this.butNew.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNew.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNew.CornerRadius = 4F;
			this.butNew.Image = global::OpenDental.Properties.Resources.Add;
			this.butNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butNew.Location = new System.Drawing.Point(85, 57);
			this.butNew.Name = "butNew";
			this.butNew.Size = new System.Drawing.Size(75, 26);
			this.butNew.TabIndex = 29;
			this.butNew.Text = "&New";
			this.butNew.Click += new System.EventHandler(this.butNew_Click);
			// 
			// butExport
			// 
			this.butExport.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butExport.Autosize = true;
			this.butExport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butExport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butExport.CornerRadius = 4F;
			this.butExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butExport.Location = new System.Drawing.Point(85, 19);
			this.butExport.Name = "butExport";
			this.butExport.Size = new System.Drawing.Size(75, 26);
			this.butExport.TabIndex = 27;
			this.butExport.Text = "Export";
			this.butExport.Click += new System.EventHandler(this.butExport_Click);
			// 
			// butImport
			// 
			this.butImport.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butImport.Autosize = true;
			this.butImport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butImport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butImport.CornerRadius = 4F;
			this.butImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butImport.Location = new System.Drawing.Point(6, 19);
			this.butImport.Name = "butImport";
			this.butImport.Size = new System.Drawing.Size(75, 26);
			this.butImport.TabIndex = 26;
			this.butImport.Text = "Import";
			this.butImport.Click += new System.EventHandler(this.butImport_Click);
			// 
			// butProcTools
			// 
			this.butProcTools.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butProcTools.Autosize = true;
			this.butProcTools.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butProcTools.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butProcTools.CornerRadius = 4F;
			this.butProcTools.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butProcTools.Location = new System.Drawing.Point(6, 57);
			this.butProcTools.Name = "butProcTools";
			this.butProcTools.Size = new System.Drawing.Size(75, 26);
			this.butProcTools.TabIndex = 28;
			this.butProcTools.Text = "Tools";
			this.butProcTools.Click += new System.EventHandler(this.butProcTools_Click);
			// 
			// groupProcCodeSetup
			// 
			this.groupProcCodeSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupProcCodeSetup.Controls.Add(this.butProcTools);
			this.groupProcCodeSetup.Controls.Add(this.butImport);
			this.groupProcCodeSetup.Controls.Add(this.butExport);
			this.groupProcCodeSetup.Controls.Add(this.butNew);
			this.groupProcCodeSetup.Location = new System.Drawing.Point(2, 603);
			this.groupProcCodeSetup.Name = "groupProcCodeSetup";
			this.groupProcCodeSetup.Size = new System.Drawing.Size(165, 91);
			this.groupProcCodeSetup.TabIndex = 26;
			this.groupProcCodeSetup.TabStop = false;
			this.groupProcCodeSetup.Text = "Procedure Codes";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.butPickProv1);
			this.groupBox2.Controls.Add(this.butPickClinic1);
			this.groupBox2.Controls.Add(this.butPickSched1);
			this.groupBox2.Controls.Add(this.labelSched1);
			this.groupBox2.Controls.Add(this.labelClinic1);
			this.groupBox2.Controls.Add(this.labelProvider1);
			this.groupBox2.Controls.Add(this.comboProvider1);
			this.groupBox2.Controls.Add(this.comboClinic1);
			this.groupBox2.Controls.Add(this.comboFeeSched1);
			this.groupBox2.Location = new System.Drawing.Point(780, 32);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(200, 157);
			this.groupBox2.TabIndex = 27;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Fee 1";
			// 
			// butPickProv1
			// 
			this.butPickProv1.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickProv1.Autosize = false;
			this.butPickProv1.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickProv1.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickProv1.CornerRadius = 2F;
			this.butPickProv1.Location = new System.Drawing.Point(167, 114);
			this.butPickProv1.Name = "butPickProv1";
			this.butPickProv1.Size = new System.Drawing.Size(23, 21);
			this.butPickProv1.TabIndex = 5;
			this.butPickProv1.Text = "...";
			this.butPickProv1.Click += new System.EventHandler(this.butPickProvider_Click);
			// 
			// butPickClinic1
			// 
			this.butPickClinic1.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickClinic1.Autosize = false;
			this.butPickClinic1.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickClinic1.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickClinic1.CornerRadius = 2F;
			this.butPickClinic1.Location = new System.Drawing.Point(167, 74);
			this.butPickClinic1.Name = "butPickClinic1";
			this.butPickClinic1.Size = new System.Drawing.Size(23, 21);
			this.butPickClinic1.TabIndex = 3;
			this.butPickClinic1.Text = "...";
			this.butPickClinic1.Click += new System.EventHandler(this.butPickClinic_Click);
			// 
			// butPickSched1
			// 
			this.butPickSched1.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickSched1.Autosize = false;
			this.butPickSched1.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickSched1.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickSched1.CornerRadius = 2F;
			this.butPickSched1.Location = new System.Drawing.Point(167, 34);
			this.butPickSched1.Name = "butPickSched1";
			this.butPickSched1.Size = new System.Drawing.Size(23, 21);
			this.butPickSched1.TabIndex = 1;
			this.butPickSched1.Text = "...";
			this.butPickSched1.Click += new System.EventHandler(this.butPickFeeSched_Click);
			// 
			// labelSched1
			// 
			this.labelSched1.Location = new System.Drawing.Point(14, 16);
			this.labelSched1.Name = "labelSched1";
			this.labelSched1.Size = new System.Drawing.Size(174, 17);
			this.labelSched1.TabIndex = 32;
			this.labelSched1.Text = "Fee Schedule";
			this.labelSched1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelClinic1
			// 
			this.labelClinic1.Location = new System.Drawing.Point(14, 56);
			this.labelClinic1.Name = "labelClinic1";
			this.labelClinic1.Size = new System.Drawing.Size(174, 17);
			this.labelClinic1.TabIndex = 30;
			this.labelClinic1.Text = "Clinic";
			this.labelClinic1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelProvider1
			// 
			this.labelProvider1.Location = new System.Drawing.Point(14, 96);
			this.labelProvider1.Name = "labelProvider1";
			this.labelProvider1.Size = new System.Drawing.Size(174, 17);
			this.labelProvider1.TabIndex = 28;
			this.labelProvider1.Text = "Provider";
			this.labelProvider1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboProvider1
			// 
			this.comboProvider1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProvider1.FormattingEnabled = true;
			this.comboProvider1.Location = new System.Drawing.Point(14, 114);
			this.comboProvider1.Name = "comboProvider1";
			this.comboProvider1.Size = new System.Drawing.Size(151, 21);
			this.comboProvider1.TabIndex = 4;
			this.comboProvider1.SelectionChangeCommitted += new System.EventHandler(this.comboGeneric_SelectionChangeCommitted);
			// 
			// comboClinic1
			// 
			this.comboClinic1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic1.FormattingEnabled = true;
			this.comboClinic1.Location = new System.Drawing.Point(14, 74);
			this.comboClinic1.Name = "comboClinic1";
			this.comboClinic1.Size = new System.Drawing.Size(151, 21);
			this.comboClinic1.TabIndex = 2;
			this.comboClinic1.SelectionChangeCommitted += new System.EventHandler(this.comboGeneric_SelectionChangeCommitted);
			// 
			// comboFeeSched1
			// 
			this.comboFeeSched1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFeeSched1.FormattingEnabled = true;
			this.comboFeeSched1.Location = new System.Drawing.Point(14, 34);
			this.comboFeeSched1.Name = "comboFeeSched1";
			this.comboFeeSched1.Size = new System.Drawing.Size(151, 21);
			this.comboFeeSched1.TabIndex = 0;
			this.comboFeeSched1.SelectionChangeCommitted += new System.EventHandler(this.comboGeneric_SelectionChangeCommitted);
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.butPickProv2);
			this.groupBox3.Controls.Add(this.butPickClinic2);
			this.groupBox3.Controls.Add(this.butPickSched2);
			this.groupBox3.Controls.Add(this.labelSched2);
			this.groupBox3.Controls.Add(this.comboProvider2);
			this.groupBox3.Controls.Add(this.labelClinic2);
			this.groupBox3.Controls.Add(this.comboClinic2);
			this.groupBox3.Controls.Add(this.labelProvider2);
			this.groupBox3.Controls.Add(this.comboFeeSched2);
			this.groupBox3.Location = new System.Drawing.Point(780, 192);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(200, 157);
			this.groupBox3.TabIndex = 28;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Fee 2";
			// 
			// butPickProv2
			// 
			this.butPickProv2.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickProv2.Autosize = false;
			this.butPickProv2.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickProv2.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickProv2.CornerRadius = 2F;
			this.butPickProv2.Location = new System.Drawing.Point(167, 114);
			this.butPickProv2.Name = "butPickProv2";
			this.butPickProv2.Size = new System.Drawing.Size(23, 21);
			this.butPickProv2.TabIndex = 11;
			this.butPickProv2.Text = "...";
			this.butPickProv2.Click += new System.EventHandler(this.butPickProvider_Click);
			// 
			// butPickClinic2
			// 
			this.butPickClinic2.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickClinic2.Autosize = false;
			this.butPickClinic2.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickClinic2.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickClinic2.CornerRadius = 2F;
			this.butPickClinic2.Location = new System.Drawing.Point(167, 74);
			this.butPickClinic2.Name = "butPickClinic2";
			this.butPickClinic2.Size = new System.Drawing.Size(23, 21);
			this.butPickClinic2.TabIndex = 9;
			this.butPickClinic2.Text = "...";
			this.butPickClinic2.Click += new System.EventHandler(this.butPickClinic_Click);
			// 
			// butPickSched2
			// 
			this.butPickSched2.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickSched2.Autosize = false;
			this.butPickSched2.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickSched2.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickSched2.CornerRadius = 2F;
			this.butPickSched2.Location = new System.Drawing.Point(167, 34);
			this.butPickSched2.Name = "butPickSched2";
			this.butPickSched2.Size = new System.Drawing.Size(23, 21);
			this.butPickSched2.TabIndex = 7;
			this.butPickSched2.Text = "...";
			this.butPickSched2.Click += new System.EventHandler(this.butPickFeeSched_Click);
			// 
			// labelSched2
			// 
			this.labelSched2.Location = new System.Drawing.Point(14, 16);
			this.labelSched2.Name = "labelSched2";
			this.labelSched2.Size = new System.Drawing.Size(174, 17);
			this.labelSched2.TabIndex = 35;
			this.labelSched2.Text = "Fee Schedule";
			this.labelSched2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboProvider2
			// 
			this.comboProvider2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProvider2.FormattingEnabled = true;
			this.comboProvider2.Location = new System.Drawing.Point(14, 114);
			this.comboProvider2.Name = "comboProvider2";
			this.comboProvider2.Size = new System.Drawing.Size(151, 21);
			this.comboProvider2.TabIndex = 10;
			this.comboProvider2.SelectionChangeCommitted += new System.EventHandler(this.comboGeneric_SelectionChangeCommitted);
			// 
			// labelClinic2
			// 
			this.labelClinic2.Location = new System.Drawing.Point(14, 56);
			this.labelClinic2.Name = "labelClinic2";
			this.labelClinic2.Size = new System.Drawing.Size(174, 17);
			this.labelClinic2.TabIndex = 34;
			this.labelClinic2.Text = "Clinic";
			this.labelClinic2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboClinic2
			// 
			this.comboClinic2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic2.FormattingEnabled = true;
			this.comboClinic2.Location = new System.Drawing.Point(14, 74);
			this.comboClinic2.Name = "comboClinic2";
			this.comboClinic2.Size = new System.Drawing.Size(151, 21);
			this.comboClinic2.TabIndex = 8;
			this.comboClinic2.SelectionChangeCommitted += new System.EventHandler(this.comboGeneric_SelectionChangeCommitted);
			// 
			// labelProvider2
			// 
			this.labelProvider2.Location = new System.Drawing.Point(14, 96);
			this.labelProvider2.Name = "labelProvider2";
			this.labelProvider2.Size = new System.Drawing.Size(174, 17);
			this.labelProvider2.TabIndex = 33;
			this.labelProvider2.Text = "Provider";
			this.labelProvider2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboFeeSched2
			// 
			this.comboFeeSched2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFeeSched2.FormattingEnabled = true;
			this.comboFeeSched2.Location = new System.Drawing.Point(14, 34);
			this.comboFeeSched2.Name = "comboFeeSched2";
			this.comboFeeSched2.Size = new System.Drawing.Size(151, 21);
			this.comboFeeSched2.TabIndex = 6;
			this.comboFeeSched2.SelectionChangeCommitted += new System.EventHandler(this.comboGeneric_SelectionChangeCommitted);
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.butPickProv3);
			this.groupBox4.Controls.Add(this.butPickClinic3);
			this.groupBox4.Controls.Add(this.butPickSched3);
			this.groupBox4.Controls.Add(this.labelSched3);
			this.groupBox4.Controls.Add(this.comboProvider3);
			this.groupBox4.Controls.Add(this.labelClinic3);
			this.groupBox4.Controls.Add(this.labelProvider3);
			this.groupBox4.Controls.Add(this.comboClinic3);
			this.groupBox4.Controls.Add(this.comboFeeSched3);
			this.groupBox4.Location = new System.Drawing.Point(780, 352);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(200, 157);
			this.groupBox4.TabIndex = 29;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Fee 3";
			// 
			// butPickProv3
			// 
			this.butPickProv3.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickProv3.Autosize = false;
			this.butPickProv3.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickProv3.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickProv3.CornerRadius = 2F;
			this.butPickProv3.Location = new System.Drawing.Point(167, 114);
			this.butPickProv3.Name = "butPickProv3";
			this.butPickProv3.Size = new System.Drawing.Size(23, 21);
			this.butPickProv3.TabIndex = 17;
			this.butPickProv3.Text = "...";
			this.butPickProv3.Click += new System.EventHandler(this.butPickProvider_Click);
			// 
			// butPickClinic3
			// 
			this.butPickClinic3.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickClinic3.Autosize = false;
			this.butPickClinic3.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickClinic3.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickClinic3.CornerRadius = 2F;
			this.butPickClinic3.Location = new System.Drawing.Point(167, 74);
			this.butPickClinic3.Name = "butPickClinic3";
			this.butPickClinic3.Size = new System.Drawing.Size(23, 21);
			this.butPickClinic3.TabIndex = 15;
			this.butPickClinic3.Text = "...";
			this.butPickClinic3.Click += new System.EventHandler(this.butPickClinic_Click);
			// 
			// butPickSched3
			// 
			this.butPickSched3.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickSched3.Autosize = false;
			this.butPickSched3.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickSched3.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickSched3.CornerRadius = 2F;
			this.butPickSched3.Location = new System.Drawing.Point(167, 34);
			this.butPickSched3.Name = "butPickSched3";
			this.butPickSched3.Size = new System.Drawing.Size(23, 21);
			this.butPickSched3.TabIndex = 13;
			this.butPickSched3.Text = "...";
			this.butPickSched3.Click += new System.EventHandler(this.butPickFeeSched_Click);
			// 
			// labelSched3
			// 
			this.labelSched3.Location = new System.Drawing.Point(14, 16);
			this.labelSched3.Name = "labelSched3";
			this.labelSched3.Size = new System.Drawing.Size(174, 17);
			this.labelSched3.TabIndex = 38;
			this.labelSched3.Text = "Fee Schedule";
			this.labelSched3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboProvider3
			// 
			this.comboProvider3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProvider3.FormattingEnabled = true;
			this.comboProvider3.Location = new System.Drawing.Point(14, 114);
			this.comboProvider3.Name = "comboProvider3";
			this.comboProvider3.Size = new System.Drawing.Size(151, 21);
			this.comboProvider3.TabIndex = 16;
			this.comboProvider3.SelectionChangeCommitted += new System.EventHandler(this.comboGeneric_SelectionChangeCommitted);
			// 
			// labelClinic3
			// 
			this.labelClinic3.Location = new System.Drawing.Point(14, 56);
			this.labelClinic3.Name = "labelClinic3";
			this.labelClinic3.Size = new System.Drawing.Size(174, 17);
			this.labelClinic3.TabIndex = 37;
			this.labelClinic3.Text = "Clinic";
			this.labelClinic3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelProvider3
			// 
			this.labelProvider3.Location = new System.Drawing.Point(14, 96);
			this.labelProvider3.Name = "labelProvider3";
			this.labelProvider3.Size = new System.Drawing.Size(174, 17);
			this.labelProvider3.TabIndex = 36;
			this.labelProvider3.Text = "Provider";
			this.labelProvider3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboClinic3
			// 
			this.comboClinic3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic3.FormattingEnabled = true;
			this.comboClinic3.Location = new System.Drawing.Point(14, 74);
			this.comboClinic3.Name = "comboClinic3";
			this.comboClinic3.Size = new System.Drawing.Size(151, 21);
			this.comboClinic3.TabIndex = 14;
			this.comboClinic3.SelectionChangeCommitted += new System.EventHandler(this.comboGeneric_SelectionChangeCommitted);
			// 
			// comboFeeSched3
			// 
			this.comboFeeSched3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFeeSched3.FormattingEnabled = true;
			this.comboFeeSched3.Location = new System.Drawing.Point(14, 34);
			this.comboFeeSched3.Name = "comboFeeSched3";
			this.comboFeeSched3.Size = new System.Drawing.Size(151, 21);
			this.comboFeeSched3.TabIndex = 12;
			this.comboFeeSched3.SelectionChangeCommitted += new System.EventHandler(this.comboGeneric_SelectionChangeCommitted);
			// 
			// groupBox5
			// 
			this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox5.Controls.Add(this.butColorClinicProv);
			this.groupBox5.Controls.Add(this.butColorProvider);
			this.groupBox5.Controls.Add(this.butColorClinic);
			this.groupBox5.Controls.Add(this.butColorDefault);
			this.groupBox5.Controls.Add(this.label21);
			this.groupBox5.Controls.Add(this.label22);
			this.groupBox5.Controls.Add(this.label20);
			this.groupBox5.Controls.Add(this.label19);
			this.groupBox5.Location = new System.Drawing.Point(780, 512);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(200, 87);
			this.groupBox5.TabIndex = 30;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Fee Colors";
			// 
			// butColorClinicProv
			// 
			this.butColorClinicProv.Enabled = false;
			this.butColorClinicProv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butColorClinicProv.Location = new System.Drawing.Point(89, 54);
			this.butColorClinicProv.Name = "butColorClinicProv";
			this.butColorClinicProv.Size = new System.Drawing.Size(20, 20);
			this.butColorClinicProv.TabIndex = 163;
			// 
			// butColorProvider
			// 
			this.butColorProvider.Enabled = false;
			this.butColorProvider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butColorProvider.Location = new System.Drawing.Point(10, 54);
			this.butColorProvider.Name = "butColorProvider";
			this.butColorProvider.Size = new System.Drawing.Size(20, 20);
			this.butColorProvider.TabIndex = 162;
			// 
			// butColorClinic
			// 
			this.butColorClinic.Enabled = false;
			this.butColorClinic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butColorClinic.Location = new System.Drawing.Point(89, 23);
			this.butColorClinic.Name = "butColorClinic";
			this.butColorClinic.Size = new System.Drawing.Size(20, 20);
			this.butColorClinic.TabIndex = 161;
			// 
			// butColorDefault
			// 
			this.butColorDefault.Enabled = false;
			this.butColorDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butColorDefault.Location = new System.Drawing.Point(10, 22);
			this.butColorDefault.Name = "butColorDefault";
			this.butColorDefault.Size = new System.Drawing.Size(20, 20);
			this.butColorDefault.TabIndex = 160;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(110, 24);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(48, 17);
			this.label21.TabIndex = 48;
			this.label21.Text = "= Clinic";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(31, 57);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(55, 17);
			this.label22.TabIndex = 46;
			this.label22.Text = "= Provider";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(110, 57);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(88, 17);
			this.label20.TabIndex = 44;
			this.label20.Text = "= Provider+Clinic";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(31, 24);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(60, 17);
			this.label19.TabIndex = 43;
			this.label19.Text = "= Default";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboSort
			// 
			this.comboSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSort.FormattingEnabled = true;
			this.comboSort.Location = new System.Drawing.Point(82, 95);
			this.comboSort.Name = "comboSort";
			this.comboSort.Size = new System.Drawing.Size(73, 21);
			this.comboSort.TabIndex = 33;
			this.comboSort.SelectionChangeCommitted += new System.EventHandler(this.comboSort_SelectionChangeCommitted);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(3, 94);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(79, 20);
			this.label6.TabIndex = 34;
			this.label6.Text = "Sort Order";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormProcCodes
			// 
			this.ClientSize = new System.Drawing.Size(982, 707);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupProcCodeSetup);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupFeeScheds);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(990, 734);
			this.Name = "FormProcCodes";
			this.ShowInTaskbar = false;
			this.Text = "Procedure Codes";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormProcedures_Closing);
			this.Load += new System.EventHandler(this.FormProcCodes_Load);
			this.groupFeeScheds.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupProcCodeSetup.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		
		private void FormProcCodes_Load(object sender,System.EventArgs e) {
			_dictFeeLogs=new Dictionary<long,List<SecurityLog>>();
			if(!Security.IsAuthorized(Permissions.Setup,DateTime.MinValue,true)) {
				groupFeeScheds.Visible=false;
				butEditCategories.Visible=false;
				groupProcCodeSetup.Visible=false;
			}
			if(!IsSelectionMode) {
				butOK.Visible=false;
				butCancel.Text=Lan.g(this,"Close");
			}
			else if(AllowMultipleSelections) {
				//Allow the user to select multiple rows by changing the grid selection mode.
				gridMain.SelectionMode=GridSelectionMode.MultiExtended;
			}
			if(_canShowHidden) {
				checkShowHidden.Checked=PrefC.GetBool(PrefName.ProcCodeListShowHidden);
			}
			else {//checkShowHidden will always be unchecked.
				checkShowHidden.Visible=false;
				butShowHiddenDefault.Visible=false;
			}
			_listFeeScheds=FeeScheds.GetDeepCopy(true);
			FillCats();
			for(int i=0;i<listCategories.Items.Count;i++) {
				listCategories.SetSelected(i,true);
			}
			_listClinics=Clinics.GetForUserod(Security.CurUser);
			_listProviders=Providers.GetDeepCopy(true);
			_feeCache=Fees.GetCache();
			_feeCache.BeginTransaction();
			_colorProv=Defs.GetColor(DefCat.FeeColors,Defs.GetByExactName(DefCat.FeeColors,"Provider"));
			_colorProvClinic=Defs.GetColor(DefCat.FeeColors,Defs.GetByExactName(DefCat.FeeColors,"Provider and Clinic"));
			_colorClinic=Defs.GetColor(DefCat.FeeColors,Defs.GetByExactName(DefCat.FeeColors,"Clinic"));
			_colorDefault=Defs.GetColor(DefCat.FeeColors,Defs.GetByExactName(DefCat.FeeColors,"Default"));
			butColorProvider.BackColor=_colorProv;
			butColorClinicProv.BackColor=_colorProvClinic;
			butColorClinic.BackColor=_colorClinic;
			butColorDefault.BackColor=_colorDefault;
			labelSched1.ForeColor=_colorDefault;
			labelSched2.ForeColor=_colorDefault;
			labelSched3.ForeColor=_colorDefault;
			labelClinic1.ForeColor=_colorClinic;
			labelClinic2.ForeColor=_colorClinic;
			labelClinic3.ForeColor=_colorClinic;
			labelProvider1.ForeColor=_colorProv;
			labelProvider2.ForeColor=_colorProv;
			labelProvider3.ForeColor=_colorProv;
			FillComboBoxes();
			for(int i=0;i<Enum.GetNames(typeof(ProcCodeListSort)).Length;i++) {
				comboSort.Items.Add(Enum.GetNames(typeof(ProcCodeListSort))[i]);
			}
			comboSort.SelectedIndex=(int)ProcCodeSort;
			FillGrid();
			//Preselect corresponding procedure codes once on load.  Do not do it within FillGrid().
			if(ListSelectedProcCodes.Count > 0) {
				for(int i=0;i<gridMain.Rows.Count;i++) {
					if(ListSelectedProcCodes.Any(x => x.CodeNum==((ProcedureCode)gridMain.Rows[i].Tag).CodeNum)) {
						gridMain.SetSelected(i,true);
					}
				}
			}
		}

		private void FillComboBoxes() {
			//Save combo box selected indexes prior to changing stuff.
			long feeSchedNum1Selected=0;//Default to the first 
			long feeSchedNum2Selected=0;//Default to none
			long feeSchedNum3Selected=0;//Default to none
			if(_listFeeScheds.Count > 0) {
				if(comboFeeSched1.SelectedIndex > -1) {
					feeSchedNum1Selected=_listFeeScheds[comboFeeSched1.SelectedIndex].FeeSchedNum;
				}
				if(comboFeeSched2.SelectedIndex > 0) {
					feeSchedNum2Selected=_listFeeScheds[comboFeeSched2.SelectedIndex-1].FeeSchedNum;
				}
				if(comboFeeSched3.SelectedIndex > 0) {
					feeSchedNum3Selected=_listFeeScheds[comboFeeSched3.SelectedIndex-1].FeeSchedNum;
				}
			}
			//Always update _listFeeScheds to reflect any potential changes.
			_listFeeScheds=FeeScheds.GetDeepCopy(true);
			//Check if feschednums from above were set to hidden, if so set selected index to 0 for the combo
			if(feeSchedNum1Selected > 0 && !_listFeeScheds.Any(x => x.FeeSchedNum==feeSchedNum1Selected)) {
				comboFeeSched1.SelectedIndex=0;
			}
			if(feeSchedNum2Selected > 0 && !_listFeeScheds.Any(x => x.FeeSchedNum==feeSchedNum2Selected)) {
				comboFeeSched2.SelectedIndex=0;
			}
			if(feeSchedNum3Selected > 0 && !_listFeeScheds.Any(x => x.FeeSchedNum==feeSchedNum3Selected)) {
				comboFeeSched3.SelectedIndex=0;
			}
			//The number of clinics and providers cannot change while inside this window.  Always reselect exactly what the user had before.
			int comboClinic1Idx=comboClinic1.SelectedIndex;
			int comboClinic2Idx=comboClinic2.SelectedIndex;
			int comboClinic3Idx=comboClinic3.SelectedIndex;
			int comboProv1Idx=comboProvider1.SelectedIndex;
			int comboProv2Idx=comboProvider2.SelectedIndex;
			int comboProv3Idx=comboProvider3.SelectedIndex;
			comboFeeSched1.Items.Clear();
			comboFeeSched2.Items.Clear();
			comboFeeSched3.Items.Clear();
			comboClinic1.Items.Clear();
			comboClinic2.Items.Clear();
			comboClinic3.Items.Clear();
			comboProvider1.Items.Clear();
			comboProvider2.Items.Clear();
			comboProvider3.Items.Clear();
			//Fill fee sched combo boxes (FeeSched 1 doesn't get the "None" option)
			comboFeeSched2.Items.Add("None");
			comboFeeSched3.Items.Add("None");
			string str;
			for(int i=0;i<_listFeeScheds.Count;i++) {
				str=_listFeeScheds[i].Description;
				if(_listFeeScheds[i].FeeSchedType!=FeeScheduleType.Normal) {
					str+=" ("+_listFeeScheds[i].FeeSchedType.ToString()+")";
				}
				comboFeeSched1.Items.Add(str);
				comboFeeSched2.Items.Add(str);
				comboFeeSched3.Items.Add(str);
			}
			if(_listFeeScheds.Count==0) {//No fee schedules in the database so set the first item to none.
				comboFeeSched1.Items.Add("None");
			}
			comboFeeSched1.SelectedIndex=0;
			comboFeeSched2.SelectedIndex=0;
			comboFeeSched3.SelectedIndex=0;
			//Fill clinic combo boxes
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {//No clinics
				//Add none even though clinics is turned off so that 0 is a valid index to select.
				comboClinic1.Items.Add(Lan.g(this,"None"));
				comboClinic2.Items.Add(Lan.g(this,"None"));
				comboClinic3.Items.Add(Lan.g(this,"None"));
				//For UI reasons, leave the clinic combo boxes visible for users not using clinics and they will just say "none".
				comboClinic1.Enabled=false;
				comboClinic2.Enabled=false;
				comboClinic3.Enabled=false;
				butPickClinic1.Enabled=false;
				butPickClinic2.Enabled=false;
				butPickClinic3.Enabled=false;
			}
			else {
				comboClinic1.Items.Add(Lan.g(this,"Default"));
				comboClinic2.Items.Add(Lan.g(this,"Default"));
				comboClinic3.Items.Add(Lan.g(this,"Default"));
				for(int i=0;i<_listClinics.Count;i++) {
					comboClinic1.Items.Add(_listClinics[i].Abbr);
					comboClinic2.Items.Add(_listClinics[i].Abbr);
					comboClinic3.Items.Add(_listClinics[i].Abbr);
				}
			}
			//Fill provider combo boxes
			comboProvider1.Items.Add(Lan.g(this,"None"));
			comboProvider2.Items.Add(Lan.g(this,"None"));
			comboProvider3.Items.Add(Lan.g(this,"None"));
			for(int i=0;i<_listProviders.Count;i++) {
				comboProvider1.Items.Add(_listProviders[i].Abbr);
				comboProvider2.Items.Add(_listProviders[i].Abbr);
				comboProvider3.Items.Add(_listProviders[i].Abbr);
			}
			//Reselect what they had selected (if any).  FeeScheds could have changed position.
			for(int i=0;i<_listFeeScheds.Count;i++) {
				if(_listFeeScheds[i].FeeSchedNum==feeSchedNum1Selected) {
					comboFeeSched1.SelectedIndex=i;
				}
				if(_listFeeScheds[i].FeeSchedNum==feeSchedNum2Selected) {
					comboFeeSched2.SelectedIndex=i+1; //Take into account the "None" option.
				}
				if(_listFeeScheds[i].FeeSchedNum==feeSchedNum3Selected) {
					comboFeeSched3.SelectedIndex=i+1; //Take into account the "None" option.
				}
			}
			comboClinic1.SelectedIndex=comboClinic1Idx > -1 ? comboClinic1Idx:0;
			comboClinic2.SelectedIndex=comboClinic2Idx > -1 ? comboClinic2Idx:0;
			comboClinic3.SelectedIndex=comboClinic3Idx > -1 ? comboClinic3Idx:0;
			comboProvider1.SelectedIndex=comboProv1Idx > -1 ? comboProv1Idx:0;
			comboProvider2.SelectedIndex=comboProv2Idx > -1 ? comboProv2Idx:0;
			comboProvider3.SelectedIndex=comboProv3Idx > -1 ? comboProv3Idx:0;
			//If previously selected FeeSched was global, and the newly selected FeeSched is NOT global, select OD's selected Clinic in the combo box.
			if(_listFeeScheds.Count > 0 && _listFeeScheds[comboFeeSched1.SelectedIndex].IsGlobal) {
				comboClinic1.Enabled=false;
				butPickClinic1.Enabled=false;
				comboClinic1.SelectedIndex=0;				
				comboProvider1.Enabled=false;
				butPickProv1.Enabled=false;
				comboProvider1.SelectedIndex=0;
			}
			else {//Newly selected FeeSched is NOT global
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
					if(feeSchedNum1Selected==0 || comboClinic1.Enabled==false) {
						//Previously selected FeeSched WAS global or there was none selected previously, select OD's selected Clinic
						comboClinic1.SelectedIndex=Clinics.ClinicNum==0 ? 0 : _listClinics.ToList().FindIndex(x => x.ClinicNum==Clinics.ClinicNum)+1;
					}
					comboClinic1.Enabled=true;
					butPickClinic1.Enabled=true;
				}
				comboProvider1.Enabled=true;
				butPickProv1.Enabled=true;
			}
			if(comboFeeSched2.SelectedIndex==0 || _listFeeScheds[comboFeeSched2.SelectedIndex-1].IsGlobal) {
				comboClinic2.Enabled=false;
				butPickClinic2.Enabled=false;
				comboClinic2.SelectedIndex=0;
				comboProvider2.Enabled=false;
				butPickProv2.Enabled=false;
				comboProvider2.SelectedIndex=0;
			}
			else {//Newly selected FeeSched is NOT global
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
					if(comboClinic2.Enabled==false) {
						//Previously selected FeeSched WAS global, select OD's selected Clinic
						comboClinic2.SelectedIndex=Clinics.ClinicNum==0 ? 0 : _listClinics.ToList().FindIndex(x => x.ClinicNum==Clinics.ClinicNum)+1;
					}
					comboClinic2.Enabled=true;
					butPickClinic2.Enabled=true;
				}
				comboProvider2.Enabled=true;
				butPickProv2.Enabled=true;
			}
			if(comboFeeSched3.SelectedIndex==0 || _listFeeScheds[comboFeeSched3.SelectedIndex-1].IsGlobal) {
				comboClinic3.Enabled=false;
				butPickClinic3.Enabled=false;
				comboClinic3.SelectedIndex=0;
				comboProvider3.Enabled=false;
				butPickProv3.Enabled=false;
				comboProvider3.SelectedIndex=0;
			}
			else {//Newly selected FeeSched is NOT global
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
					if(comboClinic3.Enabled==false) {//Previously selected FeeSched WAS global
						//Select OD's selected Clinic
						comboClinic3.SelectedIndex=Clinics.ClinicNum==0 ? 0 : _listClinics.ToList().FindIndex(x => x.ClinicNum==Clinics.ClinicNum)+1;
					}
					comboClinic3.Enabled=true;
					butPickClinic3.Enabled=true;
				}
				comboProvider3.Enabled=true;
				butPickProv3.Enabled=true;
			}
		}

		private void FillCats() {
			ArrayList selected=new ArrayList();
			for(int i=0;i<listCategories.SelectedIndices.Count;i++) {
				selected.Add(CatList[listCategories.SelectedIndices[i]].DefNum);
			}
			if(checkShowHidden.Checked) {
				CatList=Defs.GetDefsForCategory(DefCat.ProcCodeCats).ToArray();
			}
			else {
				CatList=Defs.GetDefsForCategory(DefCat.ProcCodeCats,true).ToArray();
			}
			listCategories.Items.Clear();
			for(int i=0;i<CatList.Length;i++) {
				listCategories.Items.Add(CatList[i].ItemName);
				if(selected.Contains(CatList[i].DefNum)) {
					listCategories.SetSelected(i,true);
				}
			}
		}

		private void FillGrid() {
			if(_listFeeScheds.Count==0) {
				gridMain.BeginUpdate();
				gridMain.Rows.Clear();
				gridMain.EndUpdate();
				MsgBox.Show(this,"You must have at least one fee schedule created.");
				return;
			}
			int scroll=gridMain.ScrollValue;
			List<Def> listCatDefs=new List<Def>();
			for(int i=0;i<listCategories.SelectedIndices.Count;i++) {
				listCatDefs.Add(CatList[listCategories.SelectedIndices[i]]);
			}
			FeeSched feeSched1=_listFeeScheds[comboFeeSched1.SelectedIndex]; //First feesched will always have something selected.
			FeeSched feeSched2=null;
			if(comboFeeSched2.SelectedIndex>0) {
				feeSched2=_listFeeScheds[comboFeeSched2.SelectedIndex-1];
			} 
			FeeSched feeSched3=null;
			if(comboFeeSched3.SelectedIndex>0){
				feeSched3=_listFeeScheds[comboFeeSched3.SelectedIndex-1];
			}
			//Provider nums will be 0 for "None" value.
			long provider1Num=0;
			long provider2Num=0;
			long provider3Num=0;
			if(comboProvider1.SelectedIndex>0) {
				provider1Num=_listProviders[comboProvider1.SelectedIndex-1].ProvNum;
			}
			if(comboProvider2.SelectedIndex>0) {
				provider2Num=_listProviders[comboProvider2.SelectedIndex-1].ProvNum;
			}
			if(comboProvider3.SelectedIndex>0) {
				provider3Num=_listProviders[comboProvider3.SelectedIndex-1].ProvNum;
			}
			//Clinic nums will be 0 for "Default" or "Off" value.
			long clinic1Num=0;
			long clinic2Num=0;
			long clinic3Num=0;
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) { //Clinics is on
				if(comboClinic1.SelectedIndex>0) {
					clinic1Num=_listClinics[comboClinic1.SelectedIndex-1].ClinicNum;
				}
				if(comboClinic2.SelectedIndex>0) {
					clinic2Num=_listClinics[comboClinic2.SelectedIndex-1].ClinicNum;
				}
				if(comboClinic3.SelectedIndex>0) {
					clinic3Num=_listClinics[comboClinic3.SelectedIndex-1].ClinicNum;
				}
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			//The order of these columns are important for gridMain_CellDoubleClick() and gridMain_CellLeave()
			ODGridColumn col=new ODGridColumn(Lan.g("TableProcedures","Category"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcedures","Description"),206);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcedures","Abbr"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcedures","Code"),50);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Fee 1",50,HorizontalAlignment.Right,true);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Fee 2",50,HorizontalAlignment.Right,true);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Fee 3",50,HorizontalAlignment.Right,true);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			string searchAbbr=textAbbreviation.Text;
			string searchDesc=textDescription.Text;
			string searchCode=textCode.Text;
			List<ProcedureCode> listProcsForCats=new List<ProcedureCode>();
			//Loop through the list of categories which are ordered by def.ItemOrder.
			if(ProcCodeSort==ProcCodeListSort.Category) {
				for(int i=0;i<listCatDefs.Count;i++) {
					//Get all procedure codes that are part of the selected category.  Then order the list of procedures by ProcCodes.
					//Append the list of ordered procedures to the master list of procedures for the selected categories.
					//Appending the procedure codes in this fashion keeps them ordered correctly via the definitions ItemOrder.
					listProcsForCats.AddRange(ProcedureCodes.GetWhereFromList(proc => proc.ProcCat==listCatDefs[i].DefNum)
						.OrderBy(proc => proc.ProcCode).ToList());
				}
			}
			else if(ProcCodeSort==ProcCodeListSort.ProcCode) {
				for(int i = 0;i<listCatDefs.Count;i++) {
					listProcsForCats.AddRange(ProcedureCodes.GetWhereFromList(proc => proc.ProcCat==listCatDefs[i].DefNum).ToList());
				}
				listProcsForCats=listProcsForCats.OrderBy(proc => proc.ProcCode).ToList();
			}
			//Remove any procedures that do not meet our filters.
			listProcsForCats.RemoveAll(proc => !proc.AbbrDesc.ToLower().Contains(searchAbbr.ToLower()));
			listProcsForCats.RemoveAll(proc => !proc.Descript.ToLower().Contains(searchDesc.ToLower()));
			listProcsForCats.RemoveAll(proc => !proc.ProcCode.ToLower().Contains(searchCode.ToLower()));
			if(IsSelectionMode) {
				listProcsForCats.RemoveAll(proc => proc.ProcCode==ProcedureCodes.GroupProcCode);
			}
			string lastCategoryName="";
			foreach(ProcedureCode procCode in listProcsForCats) { 
				row=new ODGridRow();
				row.Tag=procCode;
				//Only show the category on the first procedure code in that category.
				string categoryName=Defs.GetName(DefCat.ProcCodeCats,procCode.ProcCat);
				if(lastCategoryName!=categoryName && ProcCodeSort==ProcCodeListSort.Category) {
					row.Cells.Add(categoryName);
					lastCategoryName=categoryName;
				}
				else {//This proc code is in the same category or we are not sorting by category.
					row.Cells.Add("");
				}
				row.Cells.Add(procCode.Descript);
				row.Cells.Add(procCode.AbbrDesc);
				row.Cells.Add(procCode.ProcCode);
				Fee fee1=_feeCache.GetFee(procCode.CodeNum,feeSched1.FeeSchedNum,clinic1Num,provider1Num);
				Fee fee2=null;
				if(feeSched2!=null) {
					fee2=_feeCache.GetFee(procCode.CodeNum,feeSched2.FeeSchedNum,clinic2Num,provider2Num);
				}
				Fee fee3=null;
				if(feeSched3!=null) {
					fee3=_feeCache.GetFee(procCode.CodeNum,feeSched3.FeeSchedNum,clinic3Num,provider3Num);
				}
				if(fee1==null || fee1.Amount==-1) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(fee1.Amount.ToString("n"));
					row.Cells[row.Cells.Count-1].ColorText=GetColorForFee(fee1);
				}
				if(fee2==null || fee2.Amount==-1) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(fee2.Amount.ToString("n"));
					row.Cells[row.Cells.Count-1].ColorText=GetColorForFee(fee2);
				}
				if(fee3==null || fee3.Amount==-1) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(fee3.Amount.ToString("n"));
					row.Cells[row.Cells.Count-1].ColorText=GetColorForFee(fee3);
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			gridMain.ScrollValue=scroll;
		}

		private Color GetColorForFee(Fee fee) {
			if(fee.ClinicNum!=0 && fee.ProvNum!=0) {
				return _colorProvClinic;
			}
			if(fee.ClinicNum!=0) {
				return _colorClinic;
			}
			if(fee.ProvNum!=0) {
				return _colorProv;
			}
			return _colorDefault;
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(IsSelectionMode) {
				SelectedCodeNum=((ProcedureCode)gridMain.Rows[e.Row].Tag).CodeNum;
				ListSelectedProcCodes.Clear();
				ListSelectedProcCodes.Add(((ProcedureCode)gridMain.Rows[e.Row].Tag).Copy());
				DialogResult=DialogResult.OK;
				return;
			}
			//else not selecting a code
			if(!Security.IsAuthorized(Permissions.Setup,DateTime.MinValue,false)) {
				return;
			}
			if(e.Col>3) {
				//Do nothing. All columns > 3 are editable (You cannot double click).
				return;
			}
			//not on a fee: Edit code instead
			_feeCache.SaveToDb();
			SaveLogs();
			changed=false;//We just updated the database and synced our cache, set changed to false.
			ProcedureCode procCode=(ProcedureCode)gridMain.Rows[e.Row].Tag;
			Def defProcCat=Defs.GetDefsForCategory(DefCat.ProcCodeCats).FirstOrDefault(x => x.DefNum==procCode.ProcCat);
			FormProcCodeEdit FormPCE=new FormProcCodeEdit(procCode);
			FormPCE.IsNew=false;
			FormPCE.ShowHiddenCategories=(defProcCat!=null ? defProcCat.IsHidden : false);
			FormPCE.ShowDialog();
			//The user could have edited a fee within the Procedure Code Edit window or within one of it's children so we need to refresh our cache.
			//Yes, it could have even changed if the user Canceled out of the Proc Code Edit window (e.g. use FormProcCodeEditMore.cs)
			_feeCache=Fees.GetCache();
			_feeCache.BeginTransaction();
			FillGrid();
		}

		///<summary>Takes care of individual cell edits.  Calls FillGrid to refresh other columns using the same data.</summary>
		private void gridMain_CellLeave(object sender,ODGridClickEventArgs e) {
			//This is where the real fee editing logic is.
			//Logic only works for columns 4 to 6.
			long codeNum=((ProcedureCode)gridMain.Rows[e.Row].Tag).CodeNum;
			FeeSched feeSched=null;
			long provNum=0;
			long clinicNum=0;
			if(e.Col==4) {
				feeSched=_listFeeScheds[comboFeeSched1.SelectedIndex];
				if(comboProvider1.SelectedIndex>0) {
					provNum=_listProviders[comboProvider1.SelectedIndex-1].ProvNum;
				}
				if(!PrefC.GetBool(PrefName.EasyNoClinics) && comboClinic1.SelectedIndex>0) {
					clinicNum=_listClinics[comboClinic1.SelectedIndex-1].ClinicNum;
				}
			}
			else if(e.Col==5) {
				if(comboFeeSched2.SelectedIndex==0) {//It's on the "none" option
					gridMain.Rows[e.Row].Cells[e.Col].Text="";
					return;
				}
				feeSched=_listFeeScheds[comboFeeSched2.SelectedIndex-1];
				if(comboProvider2.SelectedIndex>0) {
					provNum=_listProviders[comboProvider2.SelectedIndex-1].ProvNum;
				}
				if(!PrefC.GetBool(PrefName.EasyNoClinics) && comboClinic2.SelectedIndex>0) {
					clinicNum=_listClinics[comboClinic2.SelectedIndex-1].ClinicNum;
				}
			}
			else if(e.Col==6) {
				if(comboFeeSched3.SelectedIndex==0) {//It's on the "none" option
					gridMain.Rows[e.Row].Cells[e.Col].Text="";
					return;
				}
				feeSched=_listFeeScheds[comboFeeSched3.SelectedIndex-1];
				if(comboProvider3.SelectedIndex>0) {
					provNum=_listProviders[comboProvider3.SelectedIndex-1].ProvNum;
				}
				if(!PrefC.GetBool(PrefName.EasyNoClinics) && comboClinic3.SelectedIndex>0) {
					clinicNum=_listClinics[comboClinic3.SelectedIndex-1].ClinicNum;
				}
			}
			Fee fee=_feeCache.GetFee(codeNum,feeSched.FeeSchedNum,clinicNum,provNum);
			DateTime datePrevious=DateTime.MinValue;
			string feeAmtOld="";
			if(fee!=null){
				datePrevious = fee.SecDateTEdit;
				feeAmtOld=fee.Amount.ToString("n");
			}
			if(!Security.IsAuthorized(Permissions.Setup)) { //Don't do anything if they don't have permission.
				gridMain.Rows[e.Row].Cells[e.Col].Text=feeAmtOld;
				gridMain.Invalidate();//Causes the grid to redraw itself so that the old value comes back.
				return;
			}
			string feeAmtNewStr=gridMain.Rows[e.Row].Cells[e.Col].Text;
			double feeAmtNew=0;
			//Attempt to parse the entered value for errors.
			if(feeAmtNewStr!="" && !Double.TryParse(gridMain.Rows[e.Row].Cells[e.Col].Text,out feeAmtNew)) {
				gridMain.SetSelected(new Point(e.Col,e.Row));
				gridMain.Rows[e.Row].Cells[e.Col].Text=feeAmtOld;
				MessageBox.Show(Lan.g(this,"Please fix data entry error first."));
				return;
			}
			if(!CanEditFee(fee,feeAmtNewStr,provNum,clinicNum)) {
				if(fee==null || fee.Amount==-1) {
					gridMain.Rows[e.Row].Cells[e.Col].Text="";
				}
				else {
					gridMain.Rows[e.Row].Cells[e.Col].Text=feeAmtOld;
				}
				gridMain.Invalidate();
				return;
			}
			if(feeAmtNewStr!="") {
				gridMain.Rows[e.Row].Cells[e.Col].Text=feeAmtNew.ToString("n"); //Fix the number formatting and display it.
			}
			if(feeSched.IsGlobal) { //Global fee schedules have only one fee so blindly insert/update the fee. There will be no more localized copy.
				if(fee==null) { //Fee doesn't exist, insert
					fee=new Fee();
					fee.FeeSched=feeSched.FeeSchedNum;
					fee.CodeNum=codeNum;
					fee.Amount=feeAmtNew;
					fee.ClinicNum=0;
					fee.ProvNum=0;
					_feeCache.Add(fee);
				}
				else { //Fee does exist, update or delete.
					if(feeAmtNewStr=="") { //They want to delete the fee
						_feeCache.Remove(fee);
					}
					else { //They want to update the fee
						fee.Amount=feeAmtNew;
						_feeCache.Update(fee);
					}
				}
			}
			else { //FeeSched isn't global.
				if(feeAmtNewStr=="") { //They want to delete the fee
					//NOTE: If they are deleting the HQ fee we insert a blank (-1) for the current settings.
					if((fee.ClinicNum==0 && fee.ProvNum==0)
						&& (clinicNum!=0 || provNum!=0))
					{ 
						//The best match found was the default fee which should never be deleted when editing a fee schedule for a clinic or provider.
						//In this specific scenario, we have to add a fee to the database for the selected clinic and/or provider with an amount of -1.
						fee=new Fee();
						fee.FeeSched=feeSched.FeeSchedNum;
						fee.CodeNum=codeNum;
						fee.Amount=-1.0;
						fee.ClinicNum=clinicNum;
						fee.ProvNum=provNum;
						_feeCache.Add(fee);
					}
					else {//They want to delete a fee for their current settings.
						_feeCache.Remove(fee);
					}
				}
				//The fee did not previously exist, or the fee found isn't for the currently set settings.
				else if(fee==null || fee.ClinicNum!=clinicNum || fee.ProvNum!=provNum) {
					fee=new Fee();
					fee.FeeSched=feeSched.FeeSchedNum;
					fee.CodeNum=codeNum;
					fee.Amount=feeAmtNew;
					fee.ClinicNum=clinicNum;
					fee.ProvNum=provNum;
					_feeCache.Add(fee);
				}
				else { //Fee isn't null, is for our current clinic, is for the selected provider, and they don't want to delete it.  Just update.
					fee.Amount=feeAmtNew;
					_feeCache.Update(fee);
				}
			}
			changed=true;//Cause a cache refresh signal to be sent on closing.
			SecurityLog secLog=SecurityLogs.MakeLogEntryNoInsert(Permissions.ProcFeeEdit,0,Lan.g(this,"Procedure")+": "+ProcedureCodes.GetStringProcCode(fee.CodeNum)
				+", "+Lan.g(this,"Fee")+": "+fee.Amount.ToString("c")+", "+Lan.g(this,"Fee Schedule")+": "+FeeScheds.GetDescription(fee.FeeSched)
				+". "+Lan.g(this,"Manual edit in grid from Procedure Codes list."),fee.CodeNum);
			_dictFeeLogs[fee.FeeNum]=new List<SecurityLog>();
			_dictFeeLogs[fee.FeeNum].Add(secLog);
			_dictFeeLogs[fee.FeeNum].Add(SecurityLogs.MakeLogEntryNoInsert(Permissions.LogFeeEdit,0,"Fee changed",fee.FeeNum,
				DateTPrevious:fee.SecDateTEdit));
			FillGrid();
		}

		///<summary>Returns true if the user currently logged in has permissions to edit the given column.</summary>
		private bool CanEditFee(Fee fee,string feeAmtNewStr,long provNum,long clinicNum) {
			//There is no fee in the database and the user didn't set a new fee value so there is nothing to do.
			if(fee==null && feeAmtNewStr=="") {
				return false;
			}
			//Also, don't waste any time doing any logic below if the fee was not changed.
			if(fee!=null && (feeAmtNewStr!="" && fee.Amount==PIn.Double(feeAmtNewStr) || (fee.Amount==-1 && feeAmtNewStr==""))) {
				return false;
			}
			//Check if a provider fee schedule is selected and if the current user has permissions to edit provider fees.
			if(provNum!=0 && !Security.IsAuthorized(Permissions.ProviderFeeEdit)) {
				return false;
			}
			//Make sure the user logged in has permission to edit the clinic of the fee schedule being edited.
			if(Security.CurUser.ClinicIsRestricted && clinicNum!=Clinics.ClinicNum) {
				return false;
			}
			return true;
		}

		#region Search

		private void butAll_Click(object sender,EventArgs e) {
			for(int i=0;i<listCategories.Items.Count;i++) {
				listCategories.SetSelected(i,true);
			}
			FillGrid();
		}

		private void butEditCategories_Click(object sender,EventArgs e) {
			//won't even be visible if no permission
			ArrayList selected=new ArrayList();
			for(int i=0;i<listCategories.SelectedIndices.Count;i++) {
				selected.Add(CatList[listCategories.SelectedIndices[i]].DefNum);
			}
			FormDefinitions FormD=new FormDefinitions(DefCat.ProcCodeCats);
			FormD.ShowDialog();
			DataValid.SetInvalid(InvalidType.Defs);
			changed=true;
			FillCats();
			for(int i=0;i<CatList.Length;i++) {
				if(selected.Contains(CatList[i].DefNum)) {
					listCategories.SetSelected(i,true);
				}
			}
			//we need to move security log to within the definition window for more complete tracking
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Definitions");
			FillGrid();
		}

		private void textAbbreviation_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textDescription_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textCode_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void listCategories_MouseUp(object sender,MouseEventArgs e) {
			FillGrid();
		}

		private void checkShowHidden_Click(object sender,EventArgs e) {
			FillCats();
			FillGrid();
		}

		private void butShowHiddenDefault_Click(object sender,EventArgs e) {
			Prefs.UpdateBool(PrefName.ProcCodeListShowHidden,checkShowHidden.Checked);
			string hiddenStatus="";
			if(checkShowHidden.Checked) {
				hiddenStatus=Lan.g(this,"checked.");
			}
			else {
				hiddenStatus=Lan.g(this,"unchecked.");
			}
			MessageBox.Show(Lan.g(this,"Show Hidden will default to")+" "+hiddenStatus);
		}

		#endregion

		#region Procedure Codes

		private void butEditFeeSched_Click(object sender,System.EventArgs e) {
			//We are launching in edit mode thus we must check the FeeSchedEdit permission type.
			if(!Security.IsAuthorized(Permissions.FeeSchedEdit)) {
				return;
			}
			FormFeeScheds FormF=new FormFeeScheds(false); //The Fee Scheds window can add or hide schedules.  It cannot delete schedules.
			FormF.ShowDialog();
			FillComboBoxes();
			FillGrid();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Fee Schedules");
		}

		private void butTools_Click(object sender,System.EventArgs e) {
			_feeCache.SaveToDb();
			SaveLogs();
			long schedNum=_listFeeScheds[comboFeeSched1.SelectedIndex].FeeSchedNum;
			FormFeeSchedTools FormF=new FormFeeSchedTools(schedNum,_listFeeScheds,_listProviders,_listClinics,_feeCache);
			FormF.ShowDialog();
			if(FormF.DialogResult==DialogResult.Cancel) {
				return;
			}
			//Fees could have changed from within the FeeSchedTools window.  Refresh our local dictionary to match the cache.
			changed=false;
			_feeCache=Fees.GetCache();
			_feeCache.BeginTransaction();
			if(Programs.IsEnabled(ProgramName.eClinicalWorks)) {
				FillComboBoxes();//To show possible added fee schedule.
			}
			FillGrid();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Fee Schedule Tools");
		}

		private void butExport_Click(object sender,EventArgs e) {
			if(ProcedureCodes.GetCount()==0) {
				MsgBox.Show(this,"No procedurecodes are displayed for export.");
				return;
			}
			if(!MsgBox.Show(this,true,"Only the codes showing in this list will be exported.  Continue?")) {
				return;
			}
			List<ProcedureCode> listCodes=new List<ProcedureCode>();
			for(int i=0;i<gridMain.Rows.Count;i++) {
				ProcedureCode procCode=(ProcedureCode)gridMain.Rows[i].Tag;
				if(procCode.ProcCode=="") {
					continue;
				}
				procCode.ProvNumDefault=0;  //We do not want to export ProvNumDefault because the receiving DB will not have the same exact provNums.
				listCodes.Add(procCode);
			}
			//ClaimForm ClaimFormCur=ClaimForms.ListLong[listClaimForms.SelectedIndex];
			SaveFileDialog saveDlg=new SaveFileDialog();
			string filename="ProcCodes.xml";
			saveDlg.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
			saveDlg.FileName=filename;
			if(saveDlg.ShowDialog()!=DialogResult.OK) {
				return;
			}
			//MessageBox.Show(saveDlg.FileName);
			XmlSerializer serializer=new XmlSerializer(typeof(List<ProcedureCode>));
			TextWriter writer=new StreamWriter(saveDlg.FileName);
			serializer.Serialize(writer,listCodes);
			writer.Close();
			MsgBox.Show(this,"Exported");
		}

		private void butImport_Click(object sender,EventArgs e) {
			OpenFileDialog openDlg=new OpenFileDialog();
			openDlg.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
			if(openDlg.ShowDialog()!=DialogResult.OK) {
				return;
			}
			int rowsInserted=0;
			try {
				rowsInserted=ImportProcCodes(openDlg.FileName,null,"");
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
				FillGrid();
				return;
			}
			MessageBox.Show(Lan.g(this,"Procedure codes inserted")+": "+rowsInserted);
			DataValid.SetInvalid(InvalidType.Defs,InvalidType.ProcCodes);
			ProcedureCodes.RefreshCache();
			FillCats();
			FillGrid();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,Lan.g(this,"Imported Procedure Codes"));
		}

		///<summary>Can be called externally.  Surround with try catch.  Returns number of codes inserted. 
		///Supply path to file to import or a list of procedure codes, or an xml string.  Make sure to set the other two values blank or null.</summary>
		public static int ImportProcCodes(string path,List<ProcedureCode> listCodes,string xmlData) {
			if(listCodes==null) {
				listCodes=new List<ProcedureCode>();
			}
			//xmlData should already be tested ahead of time to make sure it's not blank.
			XmlSerializer serializer=new XmlSerializer(typeof(List<ProcedureCode>));
			if(path!="") {
				if(!File.Exists(path)) {
					throw new ApplicationException(Lan.g("FormProcCodes","File does not exist."));
				}
				try {
					using(TextReader reader=new StreamReader(path)) {
						listCodes=(List<ProcedureCode>)serializer.Deserialize(reader);
					}
				}
				catch {
					throw new ApplicationException(Lan.g("FormProcCodes","Invalid file format"));
				}
			}
			else if(xmlData!="") {
				try {
					using(TextReader reader=new StringReader(xmlData)) {
						listCodes=(List<ProcedureCode>)serializer.Deserialize(reader);
					}
				}
				catch {
					throw new ApplicationException(Lan.g("FormProcCodes","xml format"));
				}
				XmlDocument xmlDocNcodes=new XmlDocument();
				xmlDocNcodes.LoadXml(xmlData);
				//Currently this will only run for NoFeeProcCodes.txt
				//If we run this for another file we will need to double check the structure of the file and make changes to this if needed.
				foreach(XmlNode procNode in xmlDocNcodes.ChildNodes[1]){//1=ArrayOfProcedureCode
					string procCode="";
					string procCatDescript="";
					foreach(XmlNode procFieldNode in procNode.ChildNodes) {
						if(procFieldNode.Name=="ProcCode") {
							procCode=procFieldNode.InnerText;
						}
						if(procFieldNode.Name=="ProcCatDescript") {
							procCatDescript=procFieldNode.InnerText;
						}
					}
					listCodes.First(x => x.ProcCode==procCode).ProcCatDescript=procCatDescript;
				}
			}
			int retVal=0;
			for(int i=0;i<listCodes.Count;i++) {
				if(ProcedureCodes.GetContainsKey(listCodes[i].ProcCode)) {
					continue;//don't import duplicates.
				}
				listCodes[i].ProcCat=Defs.GetByExactName(DefCat.ProcCodeCats,listCodes[i].ProcCatDescript);
				if(listCodes[i].ProcCat==0) {//no category exists with that name
					Def def=new Def();
					def.Category=DefCat.ProcCodeCats;
					def.ItemName=listCodes[i].ProcCatDescript;
					def.ItemOrder=Defs.GetDefsForCategory(DefCat.ProcCodeCats).Count;
					Defs.Insert(def);
					Cache.Refresh(InvalidType.Defs);
					listCodes[i].ProcCat=def.DefNum;
				}
				listCodes[i].ProvNumDefault=0;//Always import procedure codes with no specific provider set.  The incoming prov might not exist.
				ProcedureCodes.Insert(listCodes[i]);				
				SecurityLogs.MakeLogEntry(Permissions.ProcCodeEdit,0,"Code"+listCodes[i].ProcCode+" added from procedure code import.",listCodes[i].CodeNum,
					DateTime.MinValue);
				retVal++;
			}
			return retVal;
			//don't forget to refresh procedurecodes
		}

		private void butProcTools_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.SecurityAdmin, DateTime.MinValue)) {
				return;
			}
			FormProcTools FormP=new FormProcTools();
			FormP.ShowDialog();
			if(FormP.Changed) {
				changed=true;
				FillCats();
				ProcedureCodes.RefreshCache();
				FillGrid();
			}
		}

		private void butNew_Click(object sender,System.EventArgs e) {
			//won't be visible if no permission
			FormProcCodeNew FormPCN=new FormProcCodeNew();
			FormPCN.ShowDialog();
			if(FormPCN.Changed) {
				changed=true;
				ProcedureCodes.RefreshCache();
				FillGrid();
			}
		}

		#endregion

		#region Compare Fee Schedules

		///<summary>Generic combo box change event.  No matter what combo box is changed we always want to simply FillGrid() with the new choices.</summary>
		private void comboGeneric_SelectionChangeCommitted(object sender,EventArgs e) {
			FillComboBoxes();
			FillGrid();
		}

		private void butPickFeeSched_Click(object sender,EventArgs e) {
			int selectedIndex=GetFeeSchedIndexFromPicker();
			//If the selectedIndex is -1, simply return and do not do anything.  There is no such thing as picking 'None' from the picker window.
			if(selectedIndex==-1) {
				return;
			}
			UI.Button pickerButton=(UI.Button)sender;
			if(pickerButton==butPickSched1) { //First FeeSched combobox doesn't have "None" option.
				comboFeeSched1.SelectedIndex=selectedIndex;
			}
			else if(pickerButton==butPickSched2) {
				comboFeeSched2.SelectedIndex=selectedIndex+1;
			}
			else if(pickerButton==butPickSched3) {
				comboFeeSched3.SelectedIndex=selectedIndex+1;
			}
			FillComboBoxes();
			FillGrid();
		}

		private void butPickClinic_Click(object sender,EventArgs e){
			int selectedIndex=GetClinicIndexFromPicker()+1;//All clinic combo boxes have a none option, so always add 1.
			//If the selectedIndex is 0, simply return and do not do anything.  There is no such thing as picking 'None' from the picker window.
			if(selectedIndex==0) {
				return;
			}
			UI.Button pickerButton=(UI.Button)sender;
			if(pickerButton==butPickClinic1) {
				comboClinic1.SelectedIndex=selectedIndex;
			}
			else if(pickerButton==butPickClinic2) {
				comboClinic2.SelectedIndex=selectedIndex;
			}
			else if(pickerButton==butPickClinic3) {
				comboClinic3.SelectedIndex=selectedIndex;
			}
			FillGrid();
		}

		private void butPickProvider_Click(object sender,EventArgs e){
			int selectedIndex=GetProviderIndexFromPicker()+1;//All provider combo boxes have a none option, so always add 1.
			//If the selectedIndex is 0, simply return and do not do anything.  There is no such thing as picking 'None' from the picker window.
			if(selectedIndex==0) {
				return;
			}
			UI.Button pickerButton=(UI.Button)sender;
			if(pickerButton==butPickProv1) {
				comboProvider1.SelectedIndex=selectedIndex;
			}
			else if(pickerButton==butPickProv2) {
				comboProvider2.SelectedIndex=selectedIndex;
			}
			else if(pickerButton==butPickProv3) {
				comboProvider3.SelectedIndex=selectedIndex;
			}
			FillGrid();
		}

		///<summary>Launches the Provider picker and lets the user pick a specific provider.
		///Returns the index of the selected provider within the Provider Cache (short).  Returns -1 if the user cancels out of the window.</summary>
		private int GetProviderIndexFromPicker() {
			FormProviderPick FormP=new FormProviderPick();
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK) {
				return -1;
			}
			return Providers.GetIndex(FormP.SelectedProvNum);
		}

		///<summary>Launches the Clinics window and lets the user pick a specific clinic.
		///Returns the index of the selected clinic within _arrayClinics.  Returns -1 if the user cancels out of the window.</summary>
		private int GetClinicIndexFromPicker() {
			FormClinics FormC=new FormClinics();
			FormC.IsSelectionMode=true;
			FormC.ListClinics=_listClinics.ToList();
			FormC.ShowDialog();
			return _listClinics.FindIndex(x => x.ClinicNum==FormC.SelectedClinicNum);
		}

		///<summary>Launches the Fee Schedules window and lets the user pick a specific schedule.
		///Returns the index of the selected schedule within _listFeeScheds.  Returns -1 if the user cancels out of the window.</summary>
		private int GetFeeSchedIndexFromPicker() {
			//No need to check security because we are launching the form in selection mode.
			FormFeeScheds FormFS=new FormFeeScheds(true);
			FormFS.ShowDialog();
			return _listFeeScheds.FindIndex(x => x.FeeSchedNum==FormFS.SelectedFeeSchedNum);//Returns index of the found element or -1.
		}

		#endregion

		private void comboSort_SelectionChangeCommitted(object sender,EventArgs e) {
			ProcCodeSort=(ProcCodeListSort)comboSort.SelectedIndex;
			FillGrid();
		}

		private void SaveLogs() {
			foreach(long feeNum in _dictFeeLogs.Keys) {
				foreach(SecurityLog secLog in _dictFeeLogs[feeNum]) {
					SecurityLogs.MakeLogEntry(secLog);
				}
			}
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a procedure code first.");
				return;
			}
			ListSelectedProcCodes=gridMain.SelectedTags<ProcedureCode>().Select(x => x.Copy()).ToList();
			SelectedCodeNum=ListSelectedProcCodes.First().CodeNum;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormProcedures_Closing(object sender,System.ComponentModel.CancelEventArgs e) {
			if(changed) {
				_feeCache.SaveToDb();
				SaveLogs();
				DataValid.SetInvalid(InvalidType.ProcCodes);
			}
		}
	}
}
