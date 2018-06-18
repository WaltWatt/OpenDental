/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Linq;
using OpenDental.UI;
using OpenDentBusiness.HL7;
using SparksToothChart;
using OpenDentBusiness;
using CodeBase;
using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using MigraDoc.Rendering.Printing;
using Document=OpenDentBusiness.Document;

namespace OpenDental{
///<summary></summary>
	public class ContrTreat : System.Windows.Forms.UserControl{
		//private AxFPSpread.AxvaSpread axvaSpread2;
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.IContainer components;// Required designer variable.
		private System.Windows.Forms.ListBox listSetPr;
		//<summary></summary>
		//public static ArrayList TPLines2;
		//private bool[] selectedPrs;//had to use this because of deficiency in available Listbox events.
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		//private int linesPrinted=0;
		///<summary></summary>
    public FormRpPrintPreview pView;
//		private System.Windows.Forms.PrintDialog printDialog2;
		//private bool headingPrinted;
		//private bool graphicsPrinted;
		//private bool mainPrinted;
		//private bool benefitsPrinted;
		//private bool notePrinted;
		//private double[] ColTotal;
		private System.Drawing.Font bodyFont=new System.Drawing.Font("Arial",9);
		private System.Drawing.Font nameFont=new System.Drawing.Font("Arial",9,FontStyle.Bold);
		//private Font headingFont=new Font("Arial",13,FontStyle.Bold);
		private System.Drawing.Font subHeadingFont=new System.Drawing.Font("Arial",10,FontStyle.Bold);
		private System.Drawing.Printing.PrintDocument pd2;
		private System.Drawing.Font totalFont=new System.Drawing.Font("Arial",9,FontStyle.Bold);
		//private int yPos=938;
	  //private	int xPos=25;
		private System.Windows.Forms.TextBox textPriMax;
		private System.Windows.Forms.TextBox textSecUsed;
		private System.Windows.Forms.TextBox textSecDed;
		private System.Windows.Forms.TextBox textSecMax;
		private System.Windows.Forms.TextBox textPriRem;
		private System.Windows.Forms.TextBox textPriPend;
		private System.Windows.Forms.TextBox textPriUsed;
		private System.Windows.Forms.TextBox textPriDed;
		private System.Windows.Forms.TextBox textSecRem;
		private System.Windows.Forms.TextBox textSecPend;
		private System.Windows.Forms.TextBox textPriDedRem;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.TextBox textSecDedRem;
		private OpenDental.UI.ODToolBar ToolBarMain;
    private ArrayList ALPreAuth;
		///<summary>This is a list of all procedures for the patient.</summary>
		private List<Procedure> ProcList;
		///<summary>This is a filtered list containing only TP procedures.  It's also already sorted by priority and tooth number.</summary>
		private Procedure[] ProcListTP;
		///<summary>List of ClaimProcs with status of Estimate or CapEstimate for the patient.</summary>
		private List<ClaimProc> ClaimProcList;
		private Family FamCur;
		private Patient PatCur;
		private OpenDental.UI.ODGrid gridMain;
		private OpenDental.UI.ODGrid gridPrint;
		private OpenDental.UI.ODGrid gridPreAuth;
		private List<InsPlan> InsPlanList;
		private List<SubstitutionLink> _listSubLinks=null;
		private List<InsSub> SubList;
		private OpenDental.UI.ODGrid gridPlans;
		private List<TreatPlan> _listTreatPlans;
		//private List<TreatPlan> _listTPCurrent;
		///<summary>A list of all ProcTP objects for this patient.</summary>
		private ProcTP[] ProcTPList;
		private ODtextBox textNote;
		private System.Windows.Forms.ImageList imageListMain;
		///<summary>A list of all ProcTP objects for the selected tp.</summary>
		private ProcTP[] ProcTPSelectList;
		private List <PatPlan> PatPlanList;
		private List <Benefit> BenefitList;
		private List<Procedure> ProcListFiltered;
		///<summary>Only used for printing graphical chart.</summary>
		private List<ToothInitial> ToothInitialList;
		///<summary>Only used for printing graphical chart.</summary>
		private ToothChartWrapper toothChart;
		///<summary>Only used for printing graphical chart.</summary>
		private Bitmap chartBitmap;
		private List<Claim> ClaimList;
		private bool InitializedOnStartup;
		private List<ClaimProcHist> HistList;
		private TextBox textFamPriDed;
		private TextBox textFamSecDed;
		private Label label2;
		private GroupBox groupBoxFamilyIns;
		private GroupBox groupBoxIndIns;
		private Label label3;
		private Label label4;
		private TextBox textFamSecMax;
		private Label label5;
		private TextBox textFamPriMax;
		private List<ClaimProcHist> LoopList;
		private bool checkShowInsNotAutomatic;
		private bool checkShowDiscountNotAutomatic;
		private List<TpRow> RowsMain;
		private UI.Button butInsRem;
		private UI.Button butNewTP;
		private UI.Button butSaveTP;
		///<summary>Gets updated to PatCur.PatNum that the last security log was made with so that we don't make too many security logs for this patient.  When _patNumLast no longer matches PatCur.PatNum (e.g. switched to a different patient within a module), a security log will be entered.  Gets reset (cleared and the set back to PatCur.PatNum) any time a module button is clicked which will cause another security log to be entered.</summary>
		private long _patNumLast;
		private DateTimePicker dateTimeTP;
		private UI.Button butRefresh;
		private TabControl tabShowSort;
		private TabPage tabPageShow;
		private CheckBox checkShowDiscount;
		private CheckBox checkShowTotals;
		private CheckBox checkShowMaxDed;
		private CheckBox checkShowSubtotals;
		private CheckBox checkShowFees;
		private CheckBox checkShowIns;
		private CheckBox checkShowCompleted;
		private TabPage tabPageSort;
		private Label label6;
		private RadioButton radioTreatPlanSortTooth;
		private RadioButton radioTreatPlanSortOrder;
		private Label labelCheckInsFrequency;
		private TabPage tabPagePrint;
		private CheckBox checkPrintClassic;
		private UI.Button butPlannedAppt;

		///<summary>Set to true when TP Note changes.  Public so this can be checked from FormOpenDental and the note can be saved.  Necessary because in
		///some cases the leave event doesn't fire, like when a non-modal form is selected, like big phones, and the selected patient is changed from that form.</summary>
		public bool HasNoteChanged=false;

		///<summary></summary>
		public ContrTreat(){
			Logger.openlog.Log("Initializing treatment module...",Logger.Severity.INFO);
			InitializeComponent();// This call is required by the Windows.Forms Form Designer.
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code

		private void InitializeComponent(){
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContrTreat));
			this.label1 = new System.Windows.Forms.Label();
			this.listSetPr = new System.Windows.Forms.ListBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.textPriMax = new System.Windows.Forms.TextBox();
			this.textSecUsed = new System.Windows.Forms.TextBox();
			this.textSecDed = new System.Windows.Forms.TextBox();
			this.textSecMax = new System.Windows.Forms.TextBox();
			this.textPriRem = new System.Windows.Forms.TextBox();
			this.textPriPend = new System.Windows.Forms.TextBox();
			this.textPriUsed = new System.Windows.Forms.TextBox();
			this.textPriDed = new System.Windows.Forms.TextBox();
			this.textSecRem = new System.Windows.Forms.TextBox();
			this.textSecPend = new System.Windows.Forms.TextBox();
			this.pd2 = new System.Drawing.Printing.PrintDocument();
			this.textPriDedRem = new System.Windows.Forms.TextBox();
			this.label18 = new System.Windows.Forms.Label();
			this.textSecDedRem = new System.Windows.Forms.TextBox();
			this.imageListMain = new System.Windows.Forms.ImageList(this.components);
			this.textFamPriDed = new System.Windows.Forms.TextBox();
			this.textFamSecDed = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBoxFamilyIns = new System.Windows.Forms.GroupBox();
			this.textFamPriMax = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textFamSecMax = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBoxIndIns = new System.Windows.Forms.GroupBox();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.gridPrint = new OpenDental.UI.ODGrid();
			this.gridPreAuth = new OpenDental.UI.ODGrid();
			this.gridPlans = new OpenDental.UI.ODGrid();
			this.dateTimeTP = new System.Windows.Forms.DateTimePicker();
			this.tabShowSort = new System.Windows.Forms.TabControl();
			this.tabPageShow = new System.Windows.Forms.TabPage();
			this.checkShowDiscount = new System.Windows.Forms.CheckBox();
			this.checkShowTotals = new System.Windows.Forms.CheckBox();
			this.checkShowMaxDed = new System.Windows.Forms.CheckBox();
			this.checkShowSubtotals = new System.Windows.Forms.CheckBox();
			this.checkShowFees = new System.Windows.Forms.CheckBox();
			this.checkShowIns = new System.Windows.Forms.CheckBox();
			this.checkShowCompleted = new System.Windows.Forms.CheckBox();
			this.tabPageSort = new System.Windows.Forms.TabPage();
			this.label6 = new System.Windows.Forms.Label();
			this.radioTreatPlanSortTooth = new System.Windows.Forms.RadioButton();
			this.radioTreatPlanSortOrder = new System.Windows.Forms.RadioButton();
			this.tabPagePrint = new System.Windows.Forms.TabPage();
			this.checkPrintClassic = new System.Windows.Forms.CheckBox();
			this.butRefresh = new OpenDental.UI.Button();
			this.butSaveTP = new OpenDental.UI.Button();
			this.butNewTP = new OpenDental.UI.Button();
			this.butInsRem = new OpenDental.UI.Button();
			this.ToolBarMain = new OpenDental.UI.ODToolBar();
			this.textNote = new OpenDental.ODtextBox();
			this.labelCheckInsFrequency = new System.Windows.Forms.Label();
			this.butPlannedAppt = new OpenDental.UI.Button();
			this.groupBoxFamilyIns.SuspendLayout();
			this.groupBoxIndIns.SuspendLayout();
			this.tabShowSort.SuspendLayout();
			this.tabPageShow.SuspendLayout();
			this.tabPageSort.SuspendLayout();
			this.tabPagePrint.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(808, 178);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 15);
			this.label1.TabIndex = 4;
			this.label1.Text = "Set Priority";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listSetPr
			// 
			this.listSetPr.Location = new System.Drawing.Point(810, 195);
			this.listSetPr.Name = "listSetPr";
			this.listSetPr.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listSetPr.Size = new System.Drawing.Size(70, 199);
			this.listSetPr.TabIndex = 5;
			this.listSetPr.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listSetPr_MouseDown);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(73, 16);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(60, 15);
			this.label10.TabIndex = 31;
			this.label10.Text = "Primary";
			this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(4, 37);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(66, 15);
			this.label11.TabIndex = 32;
			this.label11.Text = "Annual Max";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(4, 57);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(66, 15);
			this.label12.TabIndex = 33;
			this.label12.Text = "Deductible";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(4, 97);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(66, 15);
			this.label13.TabIndex = 34;
			this.label13.Text = "Ins Used";
			this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(4, 137);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(66, 15);
			this.label14.TabIndex = 35;
			this.label14.Text = "Remaining";
			this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(4, 117);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(66, 15);
			this.label15.TabIndex = 36;
			this.label15.Text = "Pending";
			this.label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(130, 16);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(60, 14);
			this.label16.TabIndex = 37;
			this.label16.Text = "Secondary";
			this.label16.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// textPriMax
			// 
			this.textPriMax.BackColor = System.Drawing.Color.White;
			this.textPriMax.Location = new System.Drawing.Point(71, 35);
			this.textPriMax.Name = "textPriMax";
			this.textPriMax.ReadOnly = true;
			this.textPriMax.Size = new System.Drawing.Size(60, 20);
			this.textPriMax.TabIndex = 38;
			this.textPriMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textSecUsed
			// 
			this.textSecUsed.BackColor = System.Drawing.Color.White;
			this.textSecUsed.Location = new System.Drawing.Point(130, 95);
			this.textSecUsed.Name = "textSecUsed";
			this.textSecUsed.ReadOnly = true;
			this.textSecUsed.Size = new System.Drawing.Size(60, 20);
			this.textSecUsed.TabIndex = 39;
			this.textSecUsed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textSecDed
			// 
			this.textSecDed.BackColor = System.Drawing.Color.White;
			this.textSecDed.Location = new System.Drawing.Point(130, 55);
			this.textSecDed.Name = "textSecDed";
			this.textSecDed.ReadOnly = true;
			this.textSecDed.Size = new System.Drawing.Size(60, 20);
			this.textSecDed.TabIndex = 40;
			this.textSecDed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textSecMax
			// 
			this.textSecMax.BackColor = System.Drawing.Color.White;
			this.textSecMax.Location = new System.Drawing.Point(130, 35);
			this.textSecMax.Name = "textSecMax";
			this.textSecMax.ReadOnly = true;
			this.textSecMax.Size = new System.Drawing.Size(60, 20);
			this.textSecMax.TabIndex = 41;
			this.textSecMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textPriRem
			// 
			this.textPriRem.BackColor = System.Drawing.Color.White;
			this.textPriRem.Location = new System.Drawing.Point(71, 135);
			this.textPriRem.Name = "textPriRem";
			this.textPriRem.ReadOnly = true;
			this.textPriRem.Size = new System.Drawing.Size(60, 20);
			this.textPriRem.TabIndex = 42;
			this.textPriRem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textPriPend
			// 
			this.textPriPend.BackColor = System.Drawing.Color.White;
			this.textPriPend.Location = new System.Drawing.Point(71, 115);
			this.textPriPend.Name = "textPriPend";
			this.textPriPend.ReadOnly = true;
			this.textPriPend.Size = new System.Drawing.Size(60, 20);
			this.textPriPend.TabIndex = 43;
			this.textPriPend.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textPriUsed
			// 
			this.textPriUsed.BackColor = System.Drawing.Color.White;
			this.textPriUsed.Location = new System.Drawing.Point(71, 95);
			this.textPriUsed.Name = "textPriUsed";
			this.textPriUsed.ReadOnly = true;
			this.textPriUsed.Size = new System.Drawing.Size(60, 20);
			this.textPriUsed.TabIndex = 44;
			this.textPriUsed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textPriDed
			// 
			this.textPriDed.BackColor = System.Drawing.Color.White;
			this.textPriDed.Location = new System.Drawing.Point(71, 55);
			this.textPriDed.Name = "textPriDed";
			this.textPriDed.ReadOnly = true;
			this.textPriDed.Size = new System.Drawing.Size(60, 20);
			this.textPriDed.TabIndex = 45;
			this.textPriDed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textSecRem
			// 
			this.textSecRem.BackColor = System.Drawing.Color.White;
			this.textSecRem.Location = new System.Drawing.Point(130, 135);
			this.textSecRem.Name = "textSecRem";
			this.textSecRem.ReadOnly = true;
			this.textSecRem.Size = new System.Drawing.Size(60, 20);
			this.textSecRem.TabIndex = 46;
			this.textSecRem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textSecPend
			// 
			this.textSecPend.BackColor = System.Drawing.Color.White;
			this.textSecPend.Location = new System.Drawing.Point(130, 115);
			this.textSecPend.Name = "textSecPend";
			this.textSecPend.ReadOnly = true;
			this.textSecPend.Size = new System.Drawing.Size(60, 20);
			this.textSecPend.TabIndex = 47;
			this.textSecPend.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textPriDedRem
			// 
			this.textPriDedRem.BackColor = System.Drawing.Color.White;
			this.textPriDedRem.Location = new System.Drawing.Point(71, 75);
			this.textPriDedRem.Name = "textPriDedRem";
			this.textPriDedRem.ReadOnly = true;
			this.textPriDedRem.Size = new System.Drawing.Size(60, 20);
			this.textPriDedRem.TabIndex = 51;
			this.textPriDedRem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(2, 77);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(69, 15);
			this.label18.TabIndex = 50;
			this.label18.Text = "Ded Remain";
			this.label18.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textSecDedRem
			// 
			this.textSecDedRem.BackColor = System.Drawing.Color.White;
			this.textSecDedRem.Location = new System.Drawing.Point(130, 75);
			this.textSecDedRem.Name = "textSecDedRem";
			this.textSecDedRem.ReadOnly = true;
			this.textSecDedRem.Size = new System.Drawing.Size(60, 20);
			this.textSecDedRem.TabIndex = 52;
			this.textSecDedRem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// imageListMain
			// 
			this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
			this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListMain.Images.SetKeyName(0, "");
			this.imageListMain.Images.SetKeyName(1, "");
			this.imageListMain.Images.SetKeyName(2, "");
			this.imageListMain.Images.SetKeyName(3, "Add.gif");
			// 
			// textFamPriDed
			// 
			this.textFamPriDed.BackColor = System.Drawing.Color.White;
			this.textFamPriDed.Location = new System.Drawing.Point(72, 55);
			this.textFamPriDed.Name = "textFamPriDed";
			this.textFamPriDed.ReadOnly = true;
			this.textFamPriDed.Size = new System.Drawing.Size(60, 20);
			this.textFamPriDed.TabIndex = 65;
			this.textFamPriDed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textFamSecDed
			// 
			this.textFamSecDed.BackColor = System.Drawing.Color.White;
			this.textFamSecDed.Location = new System.Drawing.Point(131, 55);
			this.textFamSecDed.Name = "textFamSecDed";
			this.textFamSecDed.ReadOnly = true;
			this.textFamSecDed.Size = new System.Drawing.Size(60, 20);
			this.textFamSecDed.TabIndex = 64;
			this.textFamSecDed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 57);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 15);
			this.label2.TabIndex = 63;
			this.label2.Text = "Fam Ded";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBoxFamilyIns
			// 
			this.groupBoxFamilyIns.Controls.Add(this.textFamPriMax);
			this.groupBoxFamilyIns.Controls.Add(this.textFamPriDed);
			this.groupBoxFamilyIns.Controls.Add(this.label3);
			this.groupBoxFamilyIns.Controls.Add(this.label4);
			this.groupBoxFamilyIns.Controls.Add(this.textFamSecMax);
			this.groupBoxFamilyIns.Controls.Add(this.label5);
			this.groupBoxFamilyIns.Controls.Add(this.textFamSecDed);
			this.groupBoxFamilyIns.Controls.Add(this.label2);
			this.groupBoxFamilyIns.Location = new System.Drawing.Point(799, 411);
			this.groupBoxFamilyIns.Name = "groupBoxFamilyIns";
			this.groupBoxFamilyIns.Size = new System.Drawing.Size(193, 80);
			this.groupBoxFamilyIns.TabIndex = 66;
			this.groupBoxFamilyIns.TabStop = false;
			this.groupBoxFamilyIns.Text = "Family Insurance";
			// 
			// textFamPriMax
			// 
			this.textFamPriMax.BackColor = System.Drawing.Color.White;
			this.textFamPriMax.Location = new System.Drawing.Point(72, 35);
			this.textFamPriMax.Name = "textFamPriMax";
			this.textFamPriMax.ReadOnly = true;
			this.textFamPriMax.Size = new System.Drawing.Size(60, 20);
			this.textFamPriMax.TabIndex = 69;
			this.textFamPriMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(74, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(60, 15);
			this.label3.TabIndex = 66;
			this.label3.Text = "Primary";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(4, 37);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(66, 15);
			this.label4.TabIndex = 67;
			this.label4.Text = "Annual Max";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textFamSecMax
			// 
			this.textFamSecMax.BackColor = System.Drawing.Color.White;
			this.textFamSecMax.Location = new System.Drawing.Point(131, 35);
			this.textFamSecMax.Name = "textFamSecMax";
			this.textFamSecMax.ReadOnly = true;
			this.textFamSecMax.Size = new System.Drawing.Size(60, 20);
			this.textFamSecMax.TabIndex = 70;
			this.textFamSecMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(131, 16);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(60, 14);
			this.label5.TabIndex = 68;
			this.label5.Text = "Secondary";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// groupBoxIndIns
			// 
			this.groupBoxIndIns.Controls.Add(this.textPriDed);
			this.groupBoxIndIns.Controls.Add(this.textPriUsed);
			this.groupBoxIndIns.Controls.Add(this.textPriDedRem);
			this.groupBoxIndIns.Controls.Add(this.textPriPend);
			this.groupBoxIndIns.Controls.Add(this.textPriRem);
			this.groupBoxIndIns.Controls.Add(this.textPriMax);
			this.groupBoxIndIns.Controls.Add(this.textSecRem);
			this.groupBoxIndIns.Controls.Add(this.label10);
			this.groupBoxIndIns.Controls.Add(this.textSecPend);
			this.groupBoxIndIns.Controls.Add(this.label11);
			this.groupBoxIndIns.Controls.Add(this.label18);
			this.groupBoxIndIns.Controls.Add(this.label12);
			this.groupBoxIndIns.Controls.Add(this.label13);
			this.groupBoxIndIns.Controls.Add(this.textSecDedRem);
			this.groupBoxIndIns.Controls.Add(this.label14);
			this.groupBoxIndIns.Controls.Add(this.label15);
			this.groupBoxIndIns.Controls.Add(this.textSecMax);
			this.groupBoxIndIns.Controls.Add(this.label16);
			this.groupBoxIndIns.Controls.Add(this.textSecDed);
			this.groupBoxIndIns.Controls.Add(this.textSecUsed);
			this.groupBoxIndIns.Location = new System.Drawing.Point(799, 491);
			this.groupBoxIndIns.Name = "groupBoxIndIns";
			this.groupBoxIndIns.Size = new System.Drawing.Size(193, 160);
			this.groupBoxIndIns.TabIndex = 67;
			this.groupBoxIndIns.TabStop = false;
			this.groupBoxIndIns.Text = "Individual Insurance";
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = true;
			this.gridMain.Location = new System.Drawing.Point(0, 182);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(790, 469);
			this.gridMain.TabIndex = 59;
			this.gridMain.Title = "Procedures";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableTP";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// gridPrint
			// 
			this.gridPrint.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridPrint.HasAddButton = false;
			this.gridPrint.HasDropDowns = false;
			this.gridPrint.HasMultilineHeaders = false;
			this.gridPrint.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridPrint.HeaderHeight = 15;
			this.gridPrint.HScrollVisible = false;
			this.gridPrint.Location = new System.Drawing.Point(0, 0);
			this.gridPrint.Name = "gridPrint";
			this.gridPrint.ScrollValue = 0;
			this.gridPrint.Size = new System.Drawing.Size(150, 150);
			this.gridPrint.TabIndex = 0;
			this.gridPrint.Title = null;
			this.gridPrint.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridPrint.TitleHeight = 18;
			this.gridPrint.TranslationName = "TablePrint";
			// 
			// gridPreAuth
			// 
			this.gridPreAuth.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridPreAuth.HasAddButton = false;
			this.gridPreAuth.HasDropDowns = false;
			this.gridPreAuth.HasMultilineHeaders = false;
			this.gridPreAuth.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridPreAuth.HeaderHeight = 15;
			this.gridPreAuth.HScrollVisible = false;
			this.gridPreAuth.Location = new System.Drawing.Point(684, 29);
			this.gridPreAuth.Name = "gridPreAuth";
			this.gridPreAuth.ScrollValue = 0;
			this.gridPreAuth.Size = new System.Drawing.Size(252, 146);
			this.gridPreAuth.TabIndex = 62;
			this.gridPreAuth.Title = "Pre Authorizations";
			this.gridPreAuth.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridPreAuth.TitleHeight = 18;
			this.gridPreAuth.TranslationName = "TablePreAuth";
			this.gridPreAuth.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPreAuth_CellDoubleClick);
			this.gridPreAuth.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPreAuth_CellClick);
			// 
			// gridPlans
			// 
			this.gridPlans.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridPlans.HasAddButton = false;
			this.gridPlans.HasDropDowns = false;
			this.gridPlans.HasMultilineHeaders = false;
			this.gridPlans.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridPlans.HeaderHeight = 15;
			this.gridPlans.HScrollVisible = false;
			this.gridPlans.Location = new System.Drawing.Point(0, 29);
			this.gridPlans.Name = "gridPlans";
			this.gridPlans.ScrollValue = 0;
			this.gridPlans.Size = new System.Drawing.Size(426, 151);
			this.gridPlans.TabIndex = 60;
			this.gridPlans.Title = "Treatment Plans";
			this.gridPlans.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridPlans.TitleHeight = 18;
			this.gridPlans.TranslationName = "TableTPList";
			this.gridPlans.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPlans_CellDoubleClick);
			this.gridPlans.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPlans_CellClick);
			// 
			// dateTimeTP
			// 
			this.dateTimeTP.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateTimeTP.Location = new System.Drawing.Point(429, 99);
			this.dateTimeTP.Name = "dateTimeTP";
			this.dateTimeTP.Size = new System.Drawing.Size(81, 20);
			this.dateTimeTP.TabIndex = 71;
			this.dateTimeTP.CloseUp += new System.EventHandler(this.dateTimeTP_CloseUp);
			// 
			// tabShowSort
			// 
			this.tabShowSort.Controls.Add(this.tabPageShow);
			this.tabShowSort.Controls.Add(this.tabPageSort);
			this.tabShowSort.Controls.Add(this.tabPagePrint);
			this.tabShowSort.Location = new System.Drawing.Point(512, 29);
			this.tabShowSort.Name = "tabShowSort";
			this.tabShowSort.SelectedIndex = 0;
			this.tabShowSort.Size = new System.Drawing.Size(166, 151);
			this.tabShowSort.TabIndex = 73;
			// 
			// tabPageShow
			// 
			this.tabPageShow.Controls.Add(this.checkShowDiscount);
			this.tabPageShow.Controls.Add(this.checkShowTotals);
			this.tabPageShow.Controls.Add(this.checkShowMaxDed);
			this.tabPageShow.Controls.Add(this.checkShowSubtotals);
			this.tabPageShow.Controls.Add(this.checkShowFees);
			this.tabPageShow.Controls.Add(this.checkShowIns);
			this.tabPageShow.Controls.Add(this.checkShowCompleted);
			this.tabPageShow.Location = new System.Drawing.Point(4, 22);
			this.tabPageShow.Name = "tabPageShow";
			this.tabPageShow.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageShow.Size = new System.Drawing.Size(158, 125);
			this.tabPageShow.TabIndex = 0;
			this.tabPageShow.Text = "Show";
			this.tabPageShow.UseVisualStyleBackColor = true;
			// 
			// checkShowDiscount
			// 
			this.checkShowDiscount.Checked = true;
			this.checkShowDiscount.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowDiscount.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowDiscount.Location = new System.Drawing.Point(23, 71);
			this.checkShowDiscount.Name = "checkShowDiscount";
			this.checkShowDiscount.Size = new System.Drawing.Size(128, 17);
			this.checkShowDiscount.TabIndex = 32;
			this.checkShowDiscount.Text = "Discount";
			this.checkShowDiscount.Click += new System.EventHandler(this.checkShowDiscount_Click);
			// 
			// checkShowTotals
			// 
			this.checkShowTotals.Checked = true;
			this.checkShowTotals.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowTotals.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowTotals.Location = new System.Drawing.Point(23, 105);
			this.checkShowTotals.Name = "checkShowTotals";
			this.checkShowTotals.Size = new System.Drawing.Size(128, 15);
			this.checkShowTotals.TabIndex = 31;
			this.checkShowTotals.Text = "Totals";
			this.checkShowTotals.Click += new System.EventHandler(this.checkShowTotals_Click);
			// 
			// checkShowMaxDed
			// 
			this.checkShowMaxDed.Checked = true;
			this.checkShowMaxDed.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowMaxDed.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowMaxDed.Location = new System.Drawing.Point(5, 20);
			this.checkShowMaxDed.Name = "checkShowMaxDed";
			this.checkShowMaxDed.Size = new System.Drawing.Size(146, 17);
			this.checkShowMaxDed.TabIndex = 30;
			this.checkShowMaxDed.Text = "Use Ins Max and Deduct";
			this.checkShowMaxDed.Click += new System.EventHandler(this.checkShowMaxDed_Click);
			// 
			// checkShowSubtotals
			// 
			this.checkShowSubtotals.Checked = true;
			this.checkShowSubtotals.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowSubtotals.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowSubtotals.Location = new System.Drawing.Point(23, 88);
			this.checkShowSubtotals.Name = "checkShowSubtotals";
			this.checkShowSubtotals.Size = new System.Drawing.Size(128, 17);
			this.checkShowSubtotals.TabIndex = 29;
			this.checkShowSubtotals.Text = "Subtotals";
			this.checkShowSubtotals.Click += new System.EventHandler(this.checkShowSubtotals_Click);
			// 
			// checkShowFees
			// 
			this.checkShowFees.Checked = true;
			this.checkShowFees.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowFees.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowFees.Location = new System.Drawing.Point(5, 37);
			this.checkShowFees.Name = "checkShowFees";
			this.checkShowFees.Size = new System.Drawing.Size(146, 17);
			this.checkShowFees.TabIndex = 28;
			this.checkShowFees.Text = "Fees";
			this.checkShowFees.Click += new System.EventHandler(this.checkShowFees_Click);
			// 
			// checkShowIns
			// 
			this.checkShowIns.Checked = true;
			this.checkShowIns.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowIns.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowIns.Location = new System.Drawing.Point(23, 54);
			this.checkShowIns.Name = "checkShowIns";
			this.checkShowIns.Size = new System.Drawing.Size(128, 17);
			this.checkShowIns.TabIndex = 27;
			this.checkShowIns.Text = "Insurance Estimates";
			this.checkShowIns.Click += new System.EventHandler(this.checkShowIns_Click);
			// 
			// checkShowCompleted
			// 
			this.checkShowCompleted.Checked = true;
			this.checkShowCompleted.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowCompleted.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowCompleted.Location = new System.Drawing.Point(5, 3);
			this.checkShowCompleted.Name = "checkShowCompleted";
			this.checkShowCompleted.Size = new System.Drawing.Size(146, 17);
			this.checkShowCompleted.TabIndex = 26;
			this.checkShowCompleted.Text = "Graphical Completed Tx";
			// 
			// tabPageSort
			// 
			this.tabPageSort.Controls.Add(this.label6);
			this.tabPageSort.Controls.Add(this.radioTreatPlanSortTooth);
			this.tabPageSort.Controls.Add(this.radioTreatPlanSortOrder);
			this.tabPageSort.Location = new System.Drawing.Point(4, 22);
			this.tabPageSort.Name = "tabPageSort";
			this.tabPageSort.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageSort.Size = new System.Drawing.Size(158, 125);
			this.tabPageSort.TabIndex = 1;
			this.tabPageSort.Text = "Sort by";
			this.tabPageSort.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(3, 8);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(146, 33);
			this.label6.TabIndex = 74;
			this.label6.Text = "Order procedures by priority, then date, then";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// radioTreatPlanSortTooth
			// 
			this.radioTreatPlanSortTooth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.radioTreatPlanSortTooth.Location = new System.Drawing.Point(14, 44);
			this.radioTreatPlanSortTooth.Name = "radioTreatPlanSortTooth";
			this.radioTreatPlanSortTooth.Size = new System.Drawing.Size(116, 15);
			this.radioTreatPlanSortTooth.TabIndex = 73;
			this.radioTreatPlanSortTooth.Text = "Tooth";
			this.radioTreatPlanSortTooth.UseVisualStyleBackColor = true;
			// 
			// radioTreatPlanSortOrder
			// 
			this.radioTreatPlanSortOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.radioTreatPlanSortOrder.Checked = true;
			this.radioTreatPlanSortOrder.Location = new System.Drawing.Point(14, 59);
			this.radioTreatPlanSortOrder.Name = "radioTreatPlanSortOrder";
			this.radioTreatPlanSortOrder.Size = new System.Drawing.Size(126, 19);
			this.radioTreatPlanSortOrder.TabIndex = 72;
			this.radioTreatPlanSortOrder.TabStop = true;
			this.radioTreatPlanSortOrder.Text = "Order Entered";
			this.radioTreatPlanSortOrder.UseVisualStyleBackColor = true;
			// 
			// tabPagePrint
			// 
			this.tabPagePrint.Controls.Add(this.checkPrintClassic);
			this.tabPagePrint.Location = new System.Drawing.Point(4, 22);
			this.tabPagePrint.Name = "tabPagePrint";
			this.tabPagePrint.Padding = new System.Windows.Forms.Padding(3);
			this.tabPagePrint.Size = new System.Drawing.Size(158, 125);
			this.tabPagePrint.TabIndex = 2;
			this.tabPagePrint.Text = "Printing";
			this.tabPagePrint.UseVisualStyleBackColor = true;
			// 
			// checkPrintClassic
			// 
			this.checkPrintClassic.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPrintClassic.Location = new System.Drawing.Point(7, 12);
			this.checkPrintClassic.Name = "checkPrintClassic";
			this.checkPrintClassic.Size = new System.Drawing.Size(146, 17);
			this.checkPrintClassic.TabIndex = 27;
			this.checkPrintClassic.Text = "Print using classic view";
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butRefresh.Location = new System.Drawing.Point(429, 125);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(80, 23);
			this.butRefresh.TabIndex = 72;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// butSaveTP
			// 
			this.butSaveTP.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSaveTP.Autosize = true;
			this.butSaveTP.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSaveTP.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSaveTP.CornerRadius = 4F;
			this.butSaveTP.Image = global::OpenDental.Properties.Resources.butCopy;
			this.butSaveTP.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSaveTP.Location = new System.Drawing.Point(429, 58);
			this.butSaveTP.Name = "butSaveTP";
			this.butSaveTP.Size = new System.Drawing.Size(80, 23);
			this.butSaveTP.TabIndex = 70;
			this.butSaveTP.Text = "Save TP";
			this.butSaveTP.Click += new System.EventHandler(this.butSaveTP_Click);
			// 
			// butNewTP
			// 
			this.butNewTP.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNewTP.Autosize = true;
			this.butNewTP.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNewTP.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNewTP.CornerRadius = 4F;
			this.butNewTP.Image = global::OpenDental.Properties.Resources.Add;
			this.butNewTP.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butNewTP.Location = new System.Drawing.Point(429, 29);
			this.butNewTP.Name = "butNewTP";
			this.butNewTP.Size = new System.Drawing.Size(80, 23);
			this.butNewTP.TabIndex = 69;
			this.butNewTP.Text = "New TP";
			this.butNewTP.Click += new System.EventHandler(this.butNewTP_Click);
			// 
			// butInsRem
			// 
			this.butInsRem.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butInsRem.Autosize = true;
			this.butInsRem.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butInsRem.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butInsRem.CornerRadius = 4F;
			this.butInsRem.Location = new System.Drawing.Point(917, 400);
			this.butInsRem.Name = "butInsRem";
			this.butInsRem.Size = new System.Drawing.Size(75, 16);
			this.butInsRem.TabIndex = 68;
			this.butInsRem.Text = "Ins Rem";
			this.butInsRem.Visible = false;
			this.butInsRem.Click += new System.EventHandler(this.butInsRem_Click);
			// 
			// ToolBarMain
			// 
			this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.ToolBarMain.ImageList = this.imageListMain;
			this.ToolBarMain.Location = new System.Drawing.Point(0, 0);
			this.ToolBarMain.Name = "ToolBarMain";
			this.ToolBarMain.Size = new System.Drawing.Size(1195, 25);
			this.ToolBarMain.TabIndex = 58;
			this.ToolBarMain.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.ToolBarMain_ButtonClick);
			// 
			// textNote
			// 
			this.textNote.AcceptsTab = true;
			this.textNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textNote.BackColor = System.Drawing.SystemColors.Control;
			this.textNote.DetectLinksEnabled = false;
			this.textNote.DetectUrls = false;
			this.textNote.Location = new System.Drawing.Point(0, 654);
			this.textNote.Name = "textNote";
			this.textNote.QuickPasteType = OpenDentBusiness.QuickPasteType.TreatPlan;
			this.textNote.ReadOnly = true;
			this.textNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textNote.Size = new System.Drawing.Size(790, 52);
			this.textNote.SpellCheckIsEnabled = false;
			this.textNote.TabIndex = 54;
			this.textNote.Text = "";
			this.textNote.TextChanged += new System.EventHandler(this.textNote_TextChanged);
			this.textNote.Leave += new System.EventHandler(this.textNote_Leave);
			// 
			// labelCheckInsFrequency
			// 
			this.labelCheckInsFrequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCheckInsFrequency.Location = new System.Drawing.Point(426, 83);
			this.labelCheckInsFrequency.Name = "labelCheckInsFrequency";
			this.labelCheckInsFrequency.Size = new System.Drawing.Size(84, 15);
			this.labelCheckInsFrequency.TabIndex = 74;
			this.labelCheckInsFrequency.Text = "Estimates as of:";
			this.labelCheckInsFrequency.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butPlannedAppt
			// 
			this.butPlannedAppt.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPlannedAppt.Autosize = true;
			this.butPlannedAppt.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPlannedAppt.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPlannedAppt.CornerRadius = 4F;
			this.butPlannedAppt.Image = global::OpenDental.Properties.Resources.Add;
			this.butPlannedAppt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPlannedAppt.Location = new System.Drawing.Point(429, 154);
			this.butPlannedAppt.Name = "butPlannedAppt";
			this.butPlannedAppt.Size = new System.Drawing.Size(80, 23);
			this.butPlannedAppt.TabIndex = 75;
			this.butPlannedAppt.Text = "Plan Appt";
			this.butPlannedAppt.Click += new System.EventHandler(this.butPlannedAppt_Click);
			// 
			// ContrTreat
			// 
			this.Controls.Add(this.butPlannedAppt);
			this.Controls.Add(this.labelCheckInsFrequency);
			this.Controls.Add(this.tabShowSort);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.dateTimeTP);
			this.Controls.Add(this.butSaveTP);
			this.Controls.Add(this.butNewTP);
			this.Controls.Add(this.butInsRem);
			this.Controls.Add(this.groupBoxIndIns);
			this.Controls.Add(this.groupBoxFamilyIns);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.listSetPr);
			this.Controls.Add(this.ToolBarMain);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridPreAuth);
			this.Controls.Add(this.gridPlans);
			this.Controls.Add(this.textNote);
			this.Name = "ContrTreat";
			this.Size = new System.Drawing.Size(1195, 708);
			this.groupBoxFamilyIns.ResumeLayout(false);
			this.groupBoxFamilyIns.PerformLayout();
			this.groupBoxIndIns.ResumeLayout(false);
			this.groupBoxIndIns.PerformLayout();
			this.tabShowSort.ResumeLayout(false);
			this.tabPageShow.ResumeLayout(false);
			this.tabPageSort.ResumeLayout(false);
			this.tabPagePrint.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		///<summary>Only called on startup, but after local data loaded from db.</summary>
		public void InitializeOnStartup() {
			if(InitializedOnStartup) {
				return;
			}
			InitializedOnStartup=true;
			checkShowCompleted.Checked=PrefC.GetBool(PrefName.TreatPlanShowCompleted);
			if(PrefC.RandomKeys) {//random PKs don't get the option or sorting by order entered
				tabShowSort.TabPages.Remove(tabPageSort);
			}
			else {
				radioTreatPlanSortTooth.Checked=PrefC.GetBool(PrefName.TreatPlanSortByTooth);
			}
			//checkShowIns.Checked=PrefC.GetBool(PrefName.TreatPlanShowIns");
			//checkShowDiscount.Checked=PrefC.GetBool(PrefName.TreatPlanShowDiscount");
			//showHidden=true;//shows hidden priorities
			//can't use Lan.F(this);
			Lan.C(this,new Control[]
			{
				label1,
				tabShowSort,
				checkShowCompleted,
				checkShowIns,
				checkShowDiscount,
				checkShowMaxDed,
				checkShowFees,
				//checkShowStandard,
				checkShowSubtotals,
				checkShowTotals,
				label3,
				label10,
				label16,
				label11,
				label12,
				label18,
				label13,
				label15,
				label14,
				label2,
				gridMain,
				gridPlans,
				gridPreAuth,
				gridPrint,
				});
			LayoutToolBar();//redundant?
			tabShowSort.TabPages.Remove(tabPagePrint);//We may add this back in gridPlans_CellClick.
		}

		///<summary>Called every time local data is changed from any workstation.  Refreshes priority lists and lays out the toolbar.</summary>
		public void InitializeLocalData() {
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.TxPriorities,true);
			listSetPr.Items.Clear();
			listSetPr.Items.Add(Lan.g(this,"no priority"));
			for(int i=0;i<listDefs.Count;i++){
				listSetPr.Items.Add(listDefs[i].ItemName);
			}
			LayoutToolBar();
			if(PrefC.GetBool(PrefName.EasyHideInsurance)){
				checkShowIns.Visible=false;
				checkShowIns.Checked=false;
				checkShowMaxDed.Visible=false;
				//checkShowMaxDed.Checked=false;
			}
			else{
				checkShowIns.Visible=true;
				checkShowMaxDed.Visible=true;
			}
		}

		///<summary>Causes the toolbar to be laid out again.</summary>
		public void LayoutToolBar(){
			ToolBarMain.Buttons.Clear();
			//ODToolBarButton button;
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"PreAuthorization"),-1,"","PreAuth"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Discount"),-1,"","Discount"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Update Fees"),1,"","Update"));
			//ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Save TP"),3,"","Create"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Print TP"),2,"","Print"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Email TP"),-1,"","Email"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Sign TP"),-1,"","Sign"));
			ProgramL.LoadToolbar(ToolBarMain,ToolBarsAvail.TreatmentPlanModule);
			ToolBarMain.Invalidate();
			Plugins.HookAddCode(this,"ContrTreat.LayoutToolBar_end",PatCur);
			UpdateToolbarButtons();
		}

		///<summary></summary>
		public void ModuleSelected(long patNum,bool hasDefaultDate=false) {
			if(hasDefaultDate) {
				dateTimeTP.Value=DateTime.Today;
			}
			RefreshModuleData(patNum);
			RefreshModuleScreen(false);
			Plugins.HookAddCode(this,"ContrTreat.ModuleSelected_end",patNum);
		}

		///<summary></summary>
		public void ModuleUnselected(){
			UpdateTPNoteIfNeeded();//Handle this here because this happens before textNote_Leave() and we dont want anything nulled before saving;
			FamCur=null;
			PatCur=null;
			InsPlanList=null;
			//Claims.List=null;
			//ClaimProcs.List=null;
			//from FillMain:
			ProcList=null;
			ProcListTP=null;
			//Procedures.HList=null;
			//Procedures.MissingTeeth=null;
			_patNumLast=0;//Clear out the last pat num so that a security log gets entered that the module was "visited" or "refreshed".
			HasNoteChanged=false;
			Plugins.HookAddCode(this,"ContrTreat.ModuleUnselected_end");
		}

		private void RefreshModuleData(long patNum) {
			UpdateTPNoteIfNeeded();
			if(patNum!=0) {
				bool doMakeSecLog=false;
				if(_patNumLast!=patNum) {
					doMakeSecLog=true;
					_patNumLast=patNum;
				}
				TPModuleData tpData=TreatmentPlanModules.GetModuleData(patNum,doMakeSecLog);
				FamCur=tpData.Fam;
				PatCur=tpData.Pat;
				SubList=tpData.SubList;
				InsPlanList=tpData.InsPlanList;
				_listSubLinks=tpData.ListSubLinks;
				PatPlanList=tpData.PatPlanList;
				BenefitList=tpData.BenefitList;
				ClaimList=tpData.ClaimList;
				HistList=tpData.HistList;
				ProcList=tpData.ListProcedures;
				_listTreatPlans=tpData.ListTreatPlans;
				ProcTPList=tpData.ArrProcTPs;
			}
		}

		private void RefreshModuleScreen(bool doRefreshData=true){
			//ParentForm.Text=Patients.GetMainTitle(PatCur);
			FillPlans(doRefreshData);
			UpdateToolbarButtons();
			if(PatCur==null) {
				butNewTP.Enabled=false;
			}
			else {
				butNewTP.Enabled=true;
			}
			if(PrefC.GetBool(PrefName.InsChecksFrequency)) {
				butRefresh.Visible=true;
				labelCheckInsFrequency.Visible=true;
				dateTimeTP.Visible=true;
			}
			else {
				butRefresh.Visible=false;
				labelCheckInsFrequency.Visible=false;
				dateTimeTP.Visible=false;
			}
			FillMain();
			FillSummary();
      FillPreAuth();
			//FillMisc();
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				checkShowCompleted.Visible=false;
			}
			else {
				checkShowCompleted.Visible=true;
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				//Since the bonus information in FormInsRemain is currently only helpful in Canada,
				//we have decided not to show this button in other countries for now.
				butInsRem.Visible=true;
			}
			if(_listTreatPlans!=null && _listTreatPlans.Count==0) {//_listTreatPlans will be null when no patient is selected.
				textNote.Text="";
				HasNoteChanged=false;
			}
		}

		private delegate void ToolBarClick();

		private void ToolBarMain_ButtonClick(object sender, OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			if(e.Button.Tag.GetType()==typeof(string)){
				//standard predefined button
				switch(e.Button.Tag.ToString()){
					case "PreAuth":
						ToolBarMainPreAuth_Click();
						break;
					case "Discount":
						ToolBarMainDiscount_Click();
						break;
					case "Update":
						ToolBarMainUpdate_Click();
						break;
					//case "Create":
					//	ToolBarMainCreate_Click();
					//	break;
					case "Print":
						//The reason we are using a delegate and BeginInvoke() is because of a Microsoft bug that causes the Print Dialog window to not be in focus			
						//when it comes from a toolbar click.
						//https://social.msdn.microsoft.com/Forums/windows/en-US/681a50b4-4ae3-407a-a747-87fb3eb427fd/first-mouse-click-after-showdialog-hits-the-parent-form?forum=winforms
						ToolBarClick toolClick=ToolBarMainPrint_Click;
						this.BeginInvoke(toolClick);
						break;
					case "Email":
						ToolBarMainEmail_Click();
						break;
					case "Sign":
						ToolBarMainSign_Click();
						break;
				}
			}
			else if(e.Button.Tag.GetType()==typeof(Program)) {
				ProgramL.Execute(((Program)e.Button.Tag).ProgramNum,PatCur);
			}
			Plugins.HookAddCode(this,"ContrTreat.ToolBarMain_ButtonClick_end",PatCur,e);
		}

		private void butNewTP_Click(object sender,EventArgs e) {
			FormTreatPlanCurEdit FormTPCE=new FormTreatPlanCurEdit();
			FormTPCE.TreatPlanCur=new TreatPlan() {
				Heading="Inactive Treatment Plan",
				Note=PrefC.GetString(PrefName.TreatmentPlanNote),
				PatNum=PatCur.PatNum,
				TPStatus=TreatPlanStatus.Inactive,
			};
			FormTPCE.ShowDialog();
			if(FormTPCE.DialogResult!=DialogResult.OK) {
				return;
			}
			long tpNum=FormTPCE.TreatPlanCur.TreatPlanNum;
			ModuleSelected(PatCur.PatNum);//refreshes TPs
			for(int i=0;i<_listTreatPlans.Count;i++) {
				if(_listTreatPlans[i].TreatPlanNum==tpNum) {
					gridPlans.SetSelected(i,true);
				}
			}
			FillMain();
		}

		private void butSaveTP_Click(object sender,EventArgs e) {
			ToolBarMainCreate_Click();
		}

		private void FillPlans(bool doRefreshData=true){
			gridPlans.BeginUpdate();
			gridPlans.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableTPList","Date"),70);
			gridPlans.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableTPList","Status"),50);
			gridPlans.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableTPList","Heading"),230);
			gridPlans.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableTPList","Signed"),76,HorizontalAlignment.Center);
			gridPlans.Columns.Add(col);
			gridPlans.Rows.Clear();
			if(PatCur==null){
				gridPlans.EndUpdate();
				return;
			}
			if(doRefreshData || ProcList==null) {
				ProcList=Procedures.Refresh(PatCur.PatNum);
			}
			ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//_listTPCurrent=TreatPlans.Refresh(PatCur.PatNum,new[] {TreatPlanStatus.Active,TreatPlanStatus.Inactive});
			if(doRefreshData || _listTreatPlans==null) {
				_listTreatPlans=TreatPlans.GetAllForPat(PatCur.PatNum);
			}
			_listTreatPlans=_listTreatPlans
					.OrderBy(x => x.TPStatus!=TreatPlanStatus.Active)
					.ThenBy(x => x.TPStatus!=TreatPlanStatus.Inactive)
					.ThenBy(x => x.DateTP).ToList();
			if(doRefreshData || ProcTPList==null) {
				ProcTPList=ProcTPs.Refresh(PatCur.PatNum);
			}
			OpenDental.UI.ODGridRow row;
			//row=new ODGridRow();
			//row.Cells.Add("");//date empty
			//row.Cells.Add("");//date empty
			//row.Cells.Add(Lan.g(this,"Current Treatment Plans"));
			//gridPlans.Rows.Add(row);
			string str;
			for(int i=0;i<_listTreatPlans.Count;i++){
				row=new ODGridRow();
				row.Cells.Add(_listTreatPlans[i].TPStatus==TreatPlanStatus.Saved?_listTreatPlans[i].DateTP.ToShortDateString():"");
				row.Cells.Add(_listTreatPlans[i].TPStatus.ToString());
				str=_listTreatPlans[i].Heading;
				if(_listTreatPlans[i].ResponsParty!=0){
					str+="\r\n"+Lan.g(this,"Responsible Party: ")+Patients.GetLim(_listTreatPlans[i].ResponsParty).GetNameLF();
				}
				row.Cells.Add(str);
				if(_listTreatPlans[i].Signature==""){
					row.Cells.Add("");
				}
				else{
					row.Cells.Add("X");
				}
				gridPlans.Rows.Add(row);
			}
			gridPlans.EndUpdate();
			gridPlans.SetSelected(0,true);
		}

		private void FillMain() {
			if((gridPlans.GetSelectedIndex()>=0 && _listTreatPlans[gridPlans.SelectedIndices[0]].Signature!="")//disable changing priorities for signed TPs
				|| PatCur==null ||_listTreatPlans.Count<1)//disable if the patient has no TPs
			{
				listSetPr.Enabled=false; 
			}
			else {
				listSetPr.Enabled=true;//allow changing priority for un-signed TPs
			}
			FillMainData();
			FillMainDisplay();
		}

	/// <summary>Fills RowsMain list for gridMain display.</summary>
	private void FillMainData() {
		decimal subfee=0;
		decimal suballowed=0;
		decimal subpriIns=0;
		decimal subsecIns=0;
		decimal subdiscount=0;
		decimal subpat=0;
		decimal totFee=0;
		decimal totPriIns=0;
		decimal totSecIns=0;
		decimal totDiscount=0;
		decimal totPat=0;
		decimal totAllowed=0;
		RowsMain=new List<TpRow>();
		if(PatCur==null || gridPlans.Rows.Count==0) {
			return;
		}
		DiscountPlan discountPlan=null;
		Procedure proc=null;
		Fee discountPlanFee=null;
		if(PatCur.DiscountPlanNum!=0) {
			discountPlan=DiscountPlans.GetPlan(PatCur.DiscountPlanNum);
		}
		TpRow row;
		TreatPlan treatPlanTemp=_listTreatPlans[gridPlans.SelectedIndices[0]];
		//Active and Inactive Treatment Plans========================================================================
		if(treatPlanTemp.TPStatus==TreatPlanStatus.Active
		   || treatPlanTemp.TPStatus==TreatPlanStatus.Inactive)
		{
			LoadActiveTP(ref treatPlanTemp);
			return;
		}
		//Archived TPs below this point==============================================================================
		ProcTPSelectList=ProcTPs.GetListForTP(_listTreatPlans[gridPlans.SelectedIndices[0]].TreatPlanNum,ProcTPList);
		bool isDone;
		for(int i=0;i<ProcTPSelectList.Length;i++) {
			row=new TpRow();
			isDone=false;
			if(ProcTPSelectList[i].TagOD==null) {
				ProcTPSelectList[i].TagOD=0L;
			}
			for(int j=0;j<ProcList.Count;j++) {
				if(ProcList[j].ProcNum==ProcTPSelectList[i].ProcNumOrig) {
					if((long)ProcTPSelectList[i].TagOD==0) {
						ProcTPSelectList[i].TagOD=ProcList[j].ProcNumLab;
					}
					if(ProcList[j].ProcStatus==ProcStat.C) {
						isDone=true;
					}
				}
			}
			if(isDone) {
				row.Done="X";
			}
			row.ProcAbbr=ProcTPSelectList[i].ProcAbbr;
			row.Priority=Defs.GetName(DefCat.TxPriorities,ProcTPSelectList[i].Priority);
			row.Tth=ProcTPSelectList[i].ToothNumTP;
			row.Surf=ProcTPSelectList[i].Surf;
			row.Code=ProcTPSelectList[i].ProcCode;
			row.Description=ProcTPSelectList[i].Descript;
			row.Fee=(decimal)ProcTPSelectList[i].FeeAmt; //Fee
			row.PriIns=(decimal)ProcTPSelectList[i].PriInsAmt; //PriIns or DiscountPlan
			row.SecIns=(decimal)ProcTPSelectList[i].SecInsAmt; //SecIns
			row.FeeAllowed=(decimal)ProcTPSelectList[i].FeeAllowed;
			if(PatCur.DiscountPlanNum!=0) {
				proc=Procedures.GetOneProc(ProcTPSelectList[i].ProcNumOrig,false);
				discountPlanFee=Fees.GetFee(proc.CodeNum,discountPlan.FeeSchedNum,proc.ClinicNum,proc.ProvNum);
				if(discountPlanFee==null) {//No fee for discount plan's feesched and proc's provider
					Provider patProv=Providers.GetProv(PatCur.PriProv);
					discountPlanFee=Fees.GetFee(proc.CodeNum,patProv.FeeSched,proc.ClinicNum,patProv.ProvNum);
					if(discountPlanFee==null) {//No fee for pat's pri prov feesched and pat's pri prov
						patProv=Providers.GetProv(PrefC.GetLong(PrefName.PracticeDefaultProv));
						discountPlanFee=Fees.GetFee(proc.CodeNum,patProv.FeeSched,proc.ClinicNum,patProv.ProvNum);
					}
				}
				decimal procFeeAmt=(discountPlanFee == null) ? 0 : (decimal)discountPlanFee.Amount;
				if(checkShowIns.Checked) {
					row.PriIns=row.Fee-procFeeAmt > 0 ? row.PriIns=row.Fee-procFeeAmt : 0;//Amount that will be adjusted off
				}
				else {
					row.Fee=procFeeAmt;
				}
			}
			//Totals
			subfee+=row.Fee;
			suballowed+=row.FeeAllowed;
			totFee+=row.Fee;
			subpriIns+=(decimal)ProcTPSelectList[i].PriInsAmt;
			totPriIns+=PatCur.DiscountPlanNum==0 ? (decimal)ProcTPSelectList[i].PriInsAmt : row.PriIns;
			subsecIns+=(decimal)ProcTPSelectList[i].SecInsAmt;
			totSecIns+=(decimal)ProcTPSelectList[i].SecInsAmt;
			row.Discount=(decimal)ProcTPSelectList[i].Discount; //Discount
			subdiscount+=(decimal)ProcTPSelectList[i].Discount;
			totDiscount+=(decimal)ProcTPSelectList[i].Discount;
			row.Pat=(decimal)ProcTPSelectList[i].PatAmt; //Pat
			subpat+=(decimal)ProcTPSelectList[i].PatAmt;
			totPat+=(decimal)ProcTPSelectList[i].PatAmt;
			totAllowed+=(decimal)ProcTPSelectList[i].FeeAllowed;
			row.Prognosis=ProcTPSelectList[i].Prognosis; //Prognosis
			row.Dx=ProcTPSelectList[i].Dx;
			row.ColorText=Defs.GetColor(DefCat.TxPriorities,ProcTPSelectList[i].Priority);
			if(row.ColorText==System.Drawing.Color.White) {
				row.ColorText=System.Drawing.Color.Black;
			}
			row.Tag=ProcTPSelectList[i].Copy();
			RowsMain.Add(row);
			if(checkShowSubtotals.Checked &&
			   (i==ProcTPSelectList.Length-1 || ProcTPSelectList[i+1].Priority!=ProcTPSelectList[i].Priority)) {
				row=new TpRow();
				row.Description=Lan.g("TableTP","Subtotal");
				row.Fee=subfee;
				row.PriIns=subpriIns;
				row.SecIns=subsecIns;
				row.Discount=subdiscount;
				row.Pat=subpat;
				row.FeeAllowed=suballowed;
				row.ColorText=Defs.GetColor(DefCat.TxPriorities,ProcTPSelectList[i].Priority);
				if(row.ColorText==System.Drawing.Color.White) {
					row.ColorText=System.Drawing.Color.Black;
				}
				row.Bold=true;
				row.ColorLborder=System.Drawing.Color.Black;
				RowsMain.Add(row);
				subfee=0;
				subpriIns=0;
				subsecIns=0;
				subdiscount=0;
				subpat=0;
				suballowed=0;
			}
		}
		textNote.Text=_listTreatPlans[gridPlans.SelectedIndices[0]].Note;
		HasNoteChanged=false;
		if((_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Saved 
			&& _listTreatPlans[gridPlans.SelectedIndices[0]].Signature!="") 
			|| (_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Inactive 
			&& _listTreatPlans[gridPlans.SelectedIndices[0]].Heading==Lan.g("TreatPlan","Unassigned")))
		{
			textNote.ReadOnly=true;
		}
		else {
			textNote.ReadOnly=false;
		}
		if(checkShowTotals.Checked) {
			row=new TpRow();
			row.Description=Lan.g("TableTP","Total");
			row.Fee=totFee;
			row.PriIns=totPriIns;
			row.SecIns=totSecIns;
			row.Discount=totDiscount;
			row.Pat=totPat;
			row.FeeAllowed=totAllowed;
			row.Bold=true;
			row.ColorText=System.Drawing.Color.Black;
			RowsMain.Add(row);
		}
	}

	private void FillMainDisplay(){
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			List<DisplayField> fields=DisplayFields.GetForCategory(DisplayFieldCategory.TreatmentPlanModule);
			if(PatCur==null || gridPlans.Rows.Count==0 || _listTreatPlans[gridPlans.SelectedIndices[0]].TPType==TreatPlanType.Insurance) {
				fields.RemoveAll(x => x.InternalName=="DPlan");//If patient doesn't have discount plan, don't show column.
			}
			else {
				fields.RemoveAll(x => x.InternalName=="Pri Ins" || x.InternalName=="Sec Ins" || x.InternalName=="Allowed");//If patient does have discount plan, don't show Pri or Sec Ins or allowed fee.
			}
			for(int i=0;i<fields.Count;i++){
				if(fields[i].Description==""){
					col=new ODGridColumn(fields[i].InternalName,fields[i].ColumnWidth);
				}
				else{
					col=new ODGridColumn(fields[i].Description,fields[i].ColumnWidth);
				}
				if(fields[i].InternalName=="Fee" && !checkShowFees.Checked){
					continue;
				}
				if((fields[i].InternalName=="Pri Ins" || fields[i].InternalName=="Sec Ins" || fields[i].InternalName=="DPlan" || fields[i].InternalName=="Allowed") 
					&& !checkShowIns.Checked){
					continue;
				}
				if(fields[i].InternalName=="Discount" && !checkShowDiscount.Checked){
					continue;
				}
				if(fields[i].InternalName=="Pat" && !checkShowIns.Checked && !checkShowDiscount.Checked){
					continue;
				}
				if(fields[i].InternalName=="Fee" 
					|| fields[i].InternalName=="Pri Ins"
					|| fields[i].InternalName=="Sec Ins"
					|| fields[i].InternalName=="DPlan"
					|| fields[i].InternalName=="Discount"
					|| fields[i].InternalName=="Pat"
					|| fields[i].InternalName=="Allowed") 
				{
					col.TextAlign=HorizontalAlignment.Right;
				}
				if(fields[i].InternalName=="Sub") {
					col.TextAlign=HorizontalAlignment.Center;
				}
				gridMain.Columns.Add(col);
			}			
			gridMain.Rows.Clear();
			if(PatCur==null){
				gridMain.EndUpdate();
				return;
			}
			if(RowsMain==null || RowsMain.Count==0) {
				gridMain.EndUpdate();
				return;
			}
			ODGridRow row;
			for(int i=0;i<RowsMain.Count;i++){
				row=new ODGridRow();
				for(int j=0;j<fields.Count;j++) {
					switch(fields[j].InternalName) {
						case "Done":
							if(RowsMain[i].Done!=null) {
								row.Cells.Add(RowsMain[i].Done.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Priority":
							if(RowsMain[i].Priority!=null) {
								row.Cells.Add(RowsMain[i].Priority.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Tth":
							if(RowsMain[i].Tth!=null) {
								row.Cells.Add(RowsMain[i].Tth.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Surf":
							if(RowsMain[i].Surf!=null) {
								row.Cells.Add(RowsMain[i].Surf.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Code":
							if(RowsMain[i].Code!=null) {
								row.Cells.Add(RowsMain[i].Code.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Sub":
							//If any patient insplan allows subst codes (if !plan.CodeSubstNone) and the code has a valid substitution code, then indicate the substitution.
							//If it is not a valid substitution code or if none of the plans allow substitutions, leave the cell blank.
							ProcedureCode procCode=ProcedureCodes.GetProcCode(RowsMain[i].Code);
							if(!ProcedureCodes.IsValidCode(procCode.SubstitutionCode)) {
								row.Cells.Add("");
							}
							else {
								//The lists gotten at the beginning of ContrTreat are not patient specific with the exception of the PatPlanList.
								//Get all patient-specific InsSubs
								List<InsSub> listPatInsSubs=new List<InsSub>();
								foreach(PatPlan plan in PatPlanList) {
									if(SubList.Exists(x => x.InsSubNum==plan.InsSubNum)) {
										listPatInsSubs.Add(SubList.Find(x => x.InsSubNum==plan.InsSubNum));
									}
								}
								//Get all patient-specific InsPlans
								List<InsPlan> listPatInsPlans=new List<InsPlan>();
								foreach(InsSub sub in listPatInsSubs) {
									if(InsPlanList.Exists(x => x.PlanNum==sub.PlanNum)) {
										listPatInsPlans.Add(InsPlanList.Find(x => x.PlanNum==sub.PlanNum));
									}
								}
								//Now we have a list of patient-specific insplans.  Look through them to see if any allow substitutions.
								bool isFound=false;
								foreach(InsPlan plan in listPatInsPlans) {
									if(SubstitutionLinks.HasSubstCodeForPlan(plan,procCode.CodeNum,_listSubLinks)) {
										string toothNum=RowsMain[i].Tth.ToString();
										long subCodeNum=procCode.CodeNum;
										subCodeNum=ProcedureCodes.GetSubstituteCodeNum(procCode.ProcCode,toothNum);//for posterior composites
										if(procCode.CodeNum!=subCodeNum && subCodeNum>0) {
											row.Cells.Add("X");//They allow substitutions.
											isFound=true;
											break;
										}
									}
								}
								if(!isFound) {
									row.Cells.Add("");//They don't allow substitutions.
								}
							}
							break;
						case "Description":
							if(RowsMain[i].Description!=null) {
								string description=RowsMain[i].Description.ToString();
								if(ProcedureCodes.GetProcCode(RowsMain[i].Code).IsCanadianLab) {
									description="^ ^ "+description;
								}
								row.Cells.Add(description);
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Fee":
							if(checkShowFees.Checked) {
								row.Cells.Add(RowsMain[i].Fee.ToString("F"));
							}
							break;
						case "Pri Ins":
						case "DPlan":
							if(checkShowIns.Checked) {
								row.Cells.Add(RowsMain[i].PriIns.ToString("F"));
							}
							break;
						case "Sec Ins":
							if(checkShowIns.Checked) {
								row.Cells.Add(RowsMain[i].SecIns.ToString("F"));
							}
							break;
						case "Discount":
							if(checkShowDiscount.Checked) {
								row.Cells.Add(RowsMain[i].Discount.ToString("F"));
							}
							break;
						case "Pat":
							if(checkShowIns.Checked || checkShowDiscount.Checked) {
								row.Cells.Add(RowsMain[i].Pat.ToString("F"));
							}
							break;
						case "Prognosis":
							if(RowsMain[i].Prognosis!=null) {
								row.Cells.Add(RowsMain[i].Prognosis.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Dx":
							if(RowsMain[i].Dx!=null) {
								row.Cells.Add(RowsMain[i].Dx.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Abbr":
							if(!String.IsNullOrEmpty(RowsMain[i].ProcAbbr)){
								row.Cells.Add(RowsMain[i].ProcAbbr.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Allowed":
							if(checkShowIns.Checked) {
								row.Cells.Add(RowsMain[i].FeeAllowed.ToString("F"));
							}
							break;
					}
				}
				if(RowsMain[i].ColorText!=null) {
					row.ColorText=RowsMain[i].ColorText;
				}
				if(RowsMain[i].ColorLborder!=null) {
					row.ColorLborder=RowsMain[i].ColorLborder;
				}
				if(RowsMain[i].Tag!=null) {
					row.Tag=RowsMain[i].Tag;
				}
				row.Bold=RowsMain[i].Bold;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void FillGridPrint() {
			this.Controls.Add(gridPrint);
			gridPrint.BeginUpdate();
			gridPrint.Columns.Clear();
			ODGridColumn col;
			DisplayFields.RefreshCache();//probably needs to be removed.
			List<DisplayField> fields=DisplayFields.GetForCategory(DisplayFieldCategory.TreatmentPlanModule);
			if(PatCur==null || gridPlans.Rows.Count==0 || _listTreatPlans[gridPlans.SelectedIndices[0]].TPType==TreatPlanType.Insurance) {
				fields.RemoveAll(x => x.InternalName=="DPlan");//If patient doesn't have discount plan, don't show column.
			}
			else {
				fields.RemoveAll(x => x.InternalName=="Pri Ins" || x.InternalName=="Sec Ins");//If patient does have discount plan, don't show Pri or Sec Ins.
			}
			for(int i=0;i<fields.Count;i++) {
				if(fields[i].Description=="") {
					col=new ODGridColumn(fields[i].InternalName,fields[i].ColumnWidth);
				}
				else {
					col=new ODGridColumn(fields[i].Description,fields[i].ColumnWidth);
				}
				if(fields[i].InternalName=="Fee" && !checkShowFees.Checked) {
					continue;
				}
				if((fields[i].InternalName=="Pri Ins" || fields[i].InternalName=="Sec Ins" || fields[i].InternalName=="DPlan") && !checkShowIns.Checked) {
					continue;
				}
				if(fields[i].InternalName=="Discount" && !checkShowDiscount.Checked) {
					continue;
				}
				if(fields[i].InternalName=="Pat" && !checkShowIns.Checked && !checkShowDiscount.Checked) {
					continue;
				}
				if(fields[i].InternalName=="Fee" 
					|| fields[i].InternalName=="Pri Ins"
					|| fields[i].InternalName=="Sec Ins"
					|| fields[i].InternalName=="DPlan"
					|| fields[i].InternalName=="Discount"
					|| fields[i].InternalName=="Pat")
				{
					col.TextAlign=HorizontalAlignment.Right;
				}
				if(fields[i].InternalName=="Sub") {
					col.TextAlign=HorizontalAlignment.Center;
				}
				gridPrint.Columns.Add(col);
			}
			gridPrint.Rows.Clear();
			if(PatCur==null) {
				gridPrint.EndUpdate();
				return;
			}
			ODGridRow row;
			for(int i=0;i<RowsMain.Count;i++) {
				row=new ODGridRow();
				for(int j=0;j<fields.Count;j++) {
					switch(fields[j].InternalName) {
						case "Done":
							if(RowsMain[i].Done!=null) {
								row.Cells.Add(RowsMain[i].Done.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Priority":
							if(RowsMain[i].Priority!=null) {
								row.Cells.Add(RowsMain[i].Priority.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Tth":
							if(RowsMain[i].Tth!=null) {
								row.Cells.Add(RowsMain[i].Tth.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Surf":
							if(RowsMain[i].Surf!=null) {
								row.Cells.Add(RowsMain[i].Surf.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Code":
							if(RowsMain[i].Code!=null) {
								row.Cells.Add(RowsMain[i].Code.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Sub":
							//If any patient insplan allows subst codes (if !plan.CodeSubstNone) and the code has a valid substitution code, then indicate the substitution.
							//If it is not a valid substitution code or if none of the plans allow substitutions, leave the cell blank.
							ProcedureCode procCode=ProcedureCodes.GetProcCode(RowsMain[i].Code);
							if(!ProcedureCodes.IsValidCode(procCode.SubstitutionCode)) {
								row.Cells.Add("");
							}
							else { 
								//The lists gotten at the beginning of ContrTreat are not patient specific with the exception of the PatPlanList.
								//Get all patient-specific InsSubs
								List<InsSub> listPatInsSubs=new List<InsSub>();
								foreach(PatPlan plan in PatPlanList) {
									if(SubList.Exists(x => x.InsSubNum==plan.InsSubNum)) {
										listPatInsSubs.Add(SubList.Find(x => x.InsSubNum==plan.InsSubNum));
									}
								}
								//Get all patient-specific InsPlans
								List<InsPlan> listPatInsPlans=new List<InsPlan>();
								foreach(InsSub sub in listPatInsSubs) {
									if(InsPlanList.Exists(x => x.PlanNum==sub.PlanNum)) {
										listPatInsPlans.Add(InsPlanList.Find(x => x.PlanNum==sub.PlanNum));
									}
								}
								//Now we have a list of patient-specific insplans.  Look through them to see if any allow substitutions.
								bool isFound=false;
								foreach(InsPlan plan in listPatInsPlans) {
									if(SubstitutionLinks.HasSubstCodeForPlan(plan,procCode.CodeNum,_listSubLinks)) {
										row.Cells.Add("X");//They allow substitutions.
										isFound=true;
										break;
									}
								}
								if(!isFound) {
									row.Cells.Add("");//They don't allow substitutions.
								}
							}
							break;
						case "Description":
							if(RowsMain[i].Description!=null) {
								row.Cells.Add(RowsMain[i].Description.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Fee":
							if(checkShowFees.Checked) {
								if(PrefC.GetBool(PrefName.TreatPlanItemized) || RowsMain[i].Description.ToString()==Lan.g("TableTP","Total")
									|| RowsMain[i].Description.ToString()==Lan.g("TableTP","Subtotal")) 
								{
									row.Cells.Add(RowsMain[i].Fee.ToString("F"));
								}
								else {
									row.Cells.Add("");
								}
							}
							break;
						case "Pri Ins":
						case "DPlan":
							if(checkShowIns.Checked) {
								if(PrefC.GetBool(PrefName.TreatPlanItemized) || RowsMain[i].Description.ToString()==Lan.g("TableTP","Total")
									|| RowsMain[i].Description.ToString()==Lan.g("TableTP","Subtotal")) 
								{
									row.Cells.Add(RowsMain[i].PriIns.ToString("F"));
								}
								else {
									row.Cells.Add("");
								}
							}
							break;
						case "Sec Ins":
							if(checkShowIns.Checked) {
								if(PrefC.GetBool(PrefName.TreatPlanItemized) || RowsMain[i].Description.ToString()==Lan.g("TableTP","Total")
									|| RowsMain[i].Description.ToString()==Lan.g("TableTP","Subtotal")) 
								{
									row.Cells.Add(RowsMain[i].SecIns.ToString("F"));
								}
								else {
									row.Cells.Add("");
								}
							}
							break;
						case "Discount":
							if(checkShowDiscount.Checked) {
								if(PrefC.GetBool(PrefName.TreatPlanItemized) || RowsMain[i].Description.ToString()==Lan.g("TableTP","Total")
									|| RowsMain[i].Description.ToString()==Lan.g("TableTP","Subtotal"))
								{
									row.Cells.Add(RowsMain[i].Discount.ToString("F"));
								}
								else {
									row.Cells.Add("");
								}
							}
							break;
						case "Pat":
							if(checkShowIns.Checked || checkShowDiscount.Checked) {
								if(PrefC.GetBool(PrefName.TreatPlanItemized) || RowsMain[i].Description.ToString()==Lan.g("TableTP","Total")
									|| RowsMain[i].Description.ToString()==Lan.g("TableTP","Subtotal")) 
								{
									row.Cells.Add(RowsMain[i].Pat.ToString("F"));
								}
								else {
									row.Cells.Add("");
								}
							}
							break;
						case "Prognosis":
							if(RowsMain[i].Prognosis!=null) {
								row.Cells.Add(RowsMain[i].Prognosis.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Dx":
							if(RowsMain[i].Dx!=null) {
								row.Cells.Add(RowsMain[i].Dx.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Abbr":
							if(!String.IsNullOrEmpty(RowsMain[i].ProcAbbr)){
								row.Cells.Add(RowsMain[i].ProcAbbr.ToString());
							}
							else {
								row.Cells.Add("");
							}
							break;
					}
				}
				if(RowsMain[i].ColorText!=null) {
					row.ColorText=RowsMain[i].ColorText;
				}
				if(RowsMain[i].ColorLborder!=null) {
					row.ColorLborder=RowsMain[i].ColorLborder;
				}
				if(RowsMain[i].Tag!=null) {
					row.Tag=RowsMain[i].Tag;
				}
				row.Bold=RowsMain[i].Bold;
				gridPrint.Rows.Add(row);
			}
			gridPrint.EndUpdate();
		}

		private void FillSummary(){
			textFamPriMax.Text="";
			textFamPriDed.Text="";
			textFamSecMax.Text="";
			textFamSecDed.Text="";
			textPriMax.Text="";
			textPriDed.Text="";
			textPriDedRem.Text="";
			textPriUsed.Text="";
			textPriPend.Text="";
			textPriRem.Text="";
			textSecMax.Text="";
			textSecDed.Text="";
			textSecDedRem.Text="";
			textSecUsed.Text="";
			textSecPend.Text="";
			textSecRem.Text="";
			if(PatCur==null){
				return;
			}
			double maxFam=0;
			double maxInd=0;
			double ded=0;
			double dedFam=0;
			double dedRem=0;
			double remain=0;
			double pend=0;
			double used=0;
			InsPlan PlanCur;//=new InsPlan();
			InsSub SubCur;
			if(PatPlanList.Count>0){
				SubCur=InsSubs.GetSub(PatPlanList[0].InsSubNum,SubList);
				PlanCur=InsPlans.GetPlan(SubCur.PlanNum,InsPlanList);
				pend=InsPlans.GetPendingDisplay(HistList,DateTime.Today,PlanCur,PatPlanList[0].PatPlanNum,-1,PatCur.PatNum,PatPlanList[0].InsSubNum,BenefitList);
				used=InsPlans.GetInsUsedDisplay(HistList,DateTime.Today,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,-1,InsPlanList,BenefitList,PatCur.PatNum,PatPlanList[0].InsSubNum);
				textPriPend.Text=pend.ToString("F");
				textPriUsed.Text=used.ToString("F");
				maxFam=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,true);
				maxInd=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,false);
				if(maxFam==-1){
					textFamPriMax.Text="";
				}
				else{
					textFamPriMax.Text=maxFam.ToString("F");
				}
				if(maxInd==-1){//if annual max is blank
					textPriMax.Text="";
					textPriRem.Text="";
				}
				else{
					remain=maxInd-used-pend;
					if(remain<0){
						remain=0;
					}
					//textFamPriMax.Text=max.ToString("F");
					textPriMax.Text=maxInd.ToString("F");
					textPriRem.Text=remain.ToString("F");
				}
				//deductible:
				ded=Benefits.GetDeductGeneralDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,BenefitCoverageLevel.Individual);
				dedFam=Benefits.GetDeductGeneralDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,BenefitCoverageLevel.Family);
				if(ded!=-1){
					textPriDed.Text=ded.ToString("F");
					dedRem=InsPlans.GetDedRemainDisplay(HistList,DateTime.Today,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,-1,InsPlanList,PatCur.PatNum,ded,dedFam);
					textPriDedRem.Text=dedRem.ToString("F");
				}
				if(dedFam!=-1) {
					textFamPriDed.Text=dedFam.ToString("F");
				}
			}
			if(PatPlanList.Count>1){
				SubCur=InsSubs.GetSub(PatPlanList[1].InsSubNum,SubList);
				PlanCur=InsPlans.GetPlan(SubCur.PlanNum,InsPlanList);
				pend=InsPlans.GetPendingDisplay(HistList,DateTime.Today,PlanCur,PatPlanList[1].PatPlanNum,-1,PatCur.PatNum,PatPlanList[1].InsSubNum,BenefitList);
				textSecPend.Text=pend.ToString("F");
				used=InsPlans.GetInsUsedDisplay(HistList,DateTime.Today,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,-1,InsPlanList,BenefitList,PatCur.PatNum,PatPlanList[1].InsSubNum);
				textSecUsed.Text=used.ToString("F");
				//max=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum);
				maxFam=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,true);
				maxInd=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,false);
				if(maxFam==-1){
					textFamSecMax.Text="";
				}
				else{
					textFamSecMax.Text=maxFam.ToString("F");
				}
				if(maxInd==-1){//if annual max is blank
					textSecMax.Text="";
					textSecRem.Text="";
				}
				else{
					remain=maxInd-used-pend;
					if(remain<0){
						remain=0;
					}
					//textFamSecMax.Text=max.ToString("F");
					textSecMax.Text=maxInd.ToString("F");
					textSecRem.Text=remain.ToString("F");
				}
				//deductible:
				ded=Benefits.GetDeductGeneralDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,BenefitCoverageLevel.Individual);
				dedFam=Benefits.GetDeductGeneralDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,BenefitCoverageLevel.Family);
				if(ded!=-1){
					textSecDed.Text=ded.ToString("F");
					dedRem=InsPlans.GetDedRemainDisplay(HistList,DateTime.Today,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,-1,InsPlanList,PatCur.PatNum,ded,dedFam);
					textSecDedRem.Text=dedRem.ToString("F");
				}
				if(dedFam!=-1) {
					textFamSecDed.Text=dedFam.ToString("F");
				}
			}
		}		

    private void FillPreAuth(){
			gridPreAuth.BeginUpdate();
			gridPreAuth.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TablePreAuth","Date Sent"),80);
			gridPreAuth.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePreAuth","Carrier"),100);
			gridPreAuth.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePreAuth","Status"),53);
			gridPreAuth.Columns.Add(col);
			gridPreAuth.Rows.Clear();
      if(PatCur==null){
				gridPreAuth.EndUpdate();
				return;
			}
      ALPreAuth=new ArrayList();			
      for(int i=0;i<ClaimList.Count;i++){
        if(ClaimList[i].ClaimType=="PreAuth"){
          ALPreAuth.Add(ClaimList[i]);
        }
      }
			OpenDental.UI.ODGridRow row;
      for(int i=0;i<ALPreAuth.Count;i++){
				InsPlan PlanCur=InsPlans.GetPlan(((Claim)ALPreAuth[i]).PlanNum,InsPlanList);
				row=new ODGridRow();
				if(((Claim)ALPreAuth[i]).DateSent.Year<1880){
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(((Claim)ALPreAuth[i]).DateSent.ToShortDateString());
				}
				row.Cells.Add(Carriers.GetName(PlanCur.CarrierNum));
				row.Cells.Add(((Claim)ALPreAuth[i]).ClaimStatus.ToString());
				gridPreAuth.Rows.Add(row);
      }
			gridPreAuth.EndUpdate();
    }

		private void gridMain_CellClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			gridPreAuth.SetSelected(false);//is this a desirable behavior?
			if(gridMain.Rows[e.Row].Tag==null) {
				return;//skip any hightlighted subtotal lines
			}
			CanadianSelectedRowHelper(((ProcTP)gridMain.Rows[e.Row].Tag));
		}

		///<summary>Selects any associated lab procedures for the given selectedProcTp in gridMain.</summary>
		private void CanadianSelectedRowHelper(ProcTP selectedProcTp) {
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA")) {
				return;
			}
			long selectedProcNumLab=(long)selectedProcTp.TagOD;//0 or FK to parent proc
			for(int i=0;i<gridMain.Rows.Count;i++) {
				if(gridMain.Rows[i].Tag==null){
					continue;//skip any hightlighted subtotal lines
				}
				long rowProcNumOrig=((ProcTP)gridMain.Rows[i].Tag).ProcNumOrig;
				long rowParentProcNum=(long)((ProcTP)gridMain.Rows[i].Tag).TagOD;//0 or FK to parent proc
				if(rowProcNumOrig==selectedProcNumLab //User clicked lab, select parent proc too.
					|| (rowParentProcNum!=0 && rowParentProcNum==selectedProcNumLab)//User clicked lab, select other labs associated to same parent proc.
					|| (selectedProcTp.ProcNumOrig==rowParentProcNum))//User clicked parent, select associated lab procs.
				{
					gridMain.SetSelected(i,true);
				}
			}
		}

		private void gridMain_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			if(gridMain.Rows[e.Row].Tag==null){
				return;//user double clicked on a subtotal row
			}
			if(gridPlans.GetSelectedIndex()>-1 
				&& (_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Active 
					||_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Inactive))
			{//current plan
				Procedure ProcCur=Procedures.GetOneProc(((ProcTP)gridMain.Rows[e.Row].Tag).ProcNumOrig,true); 
				//generate a new loop list containing only the procs before this one in it
				LoopList=new List<ClaimProcHist>();
				for(int i=0;i<ProcListTP.Length;i++) {
					if(ProcListTP[i].ProcNum==ProcCur.ProcNum) {
						break;
					}
					LoopList.AddRange(ClaimProcs.GetHistForProc(ClaimProcList,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
				}
				FormProcEdit FormPE=new FormProcEdit(ProcCur,PatCur,FamCur);
				FormPE.LoopList=LoopList;
				FormPE.HistList=HistList;
				FormPE.ShowDialog();
				long treatPlanNum=_listTreatPlans[gridPlans.SelectedIndices[0]].TreatPlanNum;
				ModuleSelected(PatCur.PatNum);
				gridPlans.SetSelected(_listTreatPlans.IndexOf(_listTreatPlans.FirstOrDefault(x=>x.TreatPlanNum==treatPlanNum)),true);
				//This only updates the grid of procedures, in case any changes were made.
				FillMain();
				for(int i=0;i<gridMain.Rows.Count;i++){
					if(gridMain.Rows[i].Tag !=null && ((ProcTP)gridMain.Rows[i].Tag).ProcNumOrig==ProcCur.ProcNum){
						gridMain.SetSelected(i,true);
					}
				}
				return;
			}
			//any other TP
			ProcTP procT=(ProcTP)gridMain.Rows[e.Row].Tag;
			DateTime dateTP=_listTreatPlans[gridPlans.SelectedIndices[0]].DateTP;
			bool isSigned=false;
			if(_listTreatPlans[gridPlans.SelectedIndices[0]].Signature!="") {
				isSigned=true;
			}
			FormProcTPEdit FormP=new FormProcTPEdit(procT,dateTP,isSigned);
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.Cancel){
				return;
			}
			int selectedPlanI=gridPlans.SelectedIndices[0];
			long selectedProc=procT.ProcTPNum;
			ModuleSelected(PatCur.PatNum);
			gridPlans.SetSelected(selectedPlanI,true);
			FillMain();
			for(int i=0;i<gridMain.Rows.Count;i++){
				if(gridMain.Rows[i].Tag !=null && ((ProcTP)gridMain.Rows[i].Tag).ProcTPNum==selectedProc){ 
					gridMain.SetSelected(i,true);
				}
			}
		}

		private void gridPlans_CellClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			FillMain();
			gridPreAuth.SetSelected(false);
			if(gridPlans.SelectedIndices.Length > 0) {
				TreatPlan treatPlan=_listTreatPlans[gridPlans.SelectedIndices[0]];
				if(treatPlan.TPStatus==TreatPlanStatus.Saved && treatPlan.DateTP < UpdateHistories.GetDateForVersion(new Version(17,1,0,0))) {
					//In 17.1 we forced everyone to switch to using sheets for TPs. In order to avoid making it appear that historical data has changed,
					//we give the option to print using the classic view for treatment plans that were saved before updating to 17.1.
					if(!tabShowSort.TabPages.Contains(tabPagePrint)) {
						tabShowSort.TabPages.Add(tabPagePrint);
					}
				}
				else {
					if(tabShowSort.TabPages.Contains(tabPagePrint)) {
						tabShowSort.TabPages.Remove(tabPagePrint);
					}
				}
			}
		}

		private void gridPlans_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			//if(e.Row==0){
			//	return;//there is nothing to edit if user clicks on current.
			//}
			long tpNum=_listTreatPlans[e.Row].TreatPlanNum;
			TreatPlan tpSelected=_listTreatPlans[e.Row];
			if(tpSelected.TPStatus==TreatPlanStatus.Saved) {
				FormTreatPlanEdit FormT=new FormTreatPlanEdit(_listTreatPlans[e.Row]);
				FormT.ShowDialog();
			}
			else {
				FormTreatPlanCurEdit FormTPC=new FormTreatPlanCurEdit();
				FormTPC.TreatPlanCur=tpSelected;
				FormTPC.ShowDialog();
			}
			ModuleSelected(PatCur.PatNum);
			for(int i=0;i<_listTreatPlans.Count;i++){
				if(_listTreatPlans[i].TreatPlanNum==tpNum){
					gridPlans.SetSelected(i,true);
				}
			}
			FillMain();
		}

		private void listSetPr_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			int clickedRow=listSetPr.IndexFromPoint(e.X,e.Y);
			if(clickedRow==-1) {
				return;
			}
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.TxPriorities,true);
			if(_listTreatPlans.Count>0
			   && (_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Active
			       || _listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Inactive)) {
				List<TreatPlanAttach> listTreatPlanAttaches=TreatPlanAttaches.GetAllForTreatPlan(_listTreatPlans[gridPlans.SelectedIndices[0]].TreatPlanNum);
				foreach(int selectedIdx in gridMain.SelectedIndices) {
					if(gridMain.Rows[selectedIdx].Tag==null) {
						continue;
					}
					TreatPlanAttach tpa=listTreatPlanAttaches.FirstOrDefault(x => x.ProcNum==((ProcTP)gridMain.Rows[selectedIdx].Tag).ProcNumOrig);
					if(tpa==null) {
						continue;
					}
					tpa.Priority=0;
					if(clickedRow!=0) {
						tpa.Priority=listDefs[clickedRow-1].DefNum;
					}
					TreatPlanAttaches.Update(tpa);
				}
				long treatPlanNum=_listTreatPlans[gridPlans.SelectedIndices[0]].TreatPlanNum;
				List<long> selectedProcs=new List<long>();
				foreach(int selectedIdx in gridMain.SelectedIndices) {
					if(gridMain.Rows[selectedIdx].Tag==null) {
						continue;
					}
					selectedProcs.Add(((ProcTP)gridMain.Rows[selectedIdx].Tag).ProcNumOrig);
				}
				ModuleSelected(PatCur.PatNum);
				gridPlans.SetSelected(_listTreatPlans.IndexOf(_listTreatPlans.FirstOrDefault(x => x.TreatPlanNum==treatPlanNum)),true);
				FillMain();
				for(int i=0;i<gridMain.Rows.Count;i++) {
					if(gridMain.Rows[i].Tag!=null && selectedProcs.Contains(((ProcTP)gridMain.Rows[i].Tag).ProcNumOrig)) {
						gridMain.SetSelected(i,true);
					}
				}
			}
			else { //any Saved TP
				DateTime dateTP=_listTreatPlans[gridPlans.SelectedIndices[0]].DateTP;
				if(!Security.IsAuthorized(Permissions.TreatPlanEdit,dateTP)) {
					return;
				}
				int selectedTP=gridPlans.SelectedIndices[0];
				ProcTP proc;
				for(int i=0;i<gridMain.SelectedIndices.Length;i++) { //loop through the main list of selected procTPs
					if(gridMain.Rows[gridMain.SelectedIndices[i]].Tag==null) {
						//user must have highlighted a subtotal row, so ignore
						continue;
					}
					proc=(ProcTP)gridMain.Rows[gridMain.SelectedIndices[i]].Tag;
					if(clickedRow==0) { //set priority to "no priority"
						proc.Priority=0;
					}
					else {
						proc.Priority=listDefs[clickedRow-1].DefNum;
					}
					ProcTPs.InsertOrUpdate(proc,false);
				}
				ModuleSelected(PatCur.PatNum);
				gridPlans.SetSelected(selectedTP,true);
				FillMain();
			}
		}

		private void checkShowMaxDed_Click(object sender,EventArgs e) {
			FillMain();
		}

		private void checkShowFees_Click(object sender,EventArgs e) {
			if(checkShowFees.Checked){
				//checkShowStandard.Checked=true;
				if(!checkShowInsNotAutomatic){
					checkShowIns.Checked=true;
				}
				if(!checkShowDiscountNotAutomatic) {
					checkShowDiscount.Checked=true;
				}
				checkShowSubtotals.Checked=true;
				checkShowTotals.Checked=true;
			}
			else{
				//checkShowStandard.Checked=false;
				if(!checkShowInsNotAutomatic){
					checkShowIns.Checked=false;
				}
				if(!checkShowDiscountNotAutomatic) {
					checkShowDiscount.Checked=false;
				}
				checkShowSubtotals.Checked=false;
				checkShowTotals.Checked=false;
			}
			FillMain();
		}

		private void checkShowStandard_Click(object sender,EventArgs e) {
			FillMain();
		}

		private void checkShowIns_Click(object sender,EventArgs e) {
			if(!checkShowIns.Checked) {
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Turn off automatic checking of this box for the rest of this session?")) {
					checkShowInsNotAutomatic=true;
				}
			}
			FillMain();
		}

		private void checkShowDiscount_Click(object sender,EventArgs e) {
			if(!checkShowDiscount.Checked) {
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Turn off automatic checking of this box for the rest of this session?")) {
					checkShowDiscountNotAutomatic=true;
				}
			}
			FillMain();
		}

		private void checkShowSubtotals_Click(object sender,EventArgs e) {
			FillMain();
		}

		private void checkShowTotals_Click(object sender,EventArgs e) {
			FillMain();
		}

		private void ToolBarMainPrint_Click() {
			if(gridPlans.SelectedIndices.Length < 1) {
				MsgBox.Show(this,"Select a Treatment Plan to print.");
				return;
			}
			#region FuchsOptionOn
			if(PrefC.GetBool(PrefName.FuchsOptionsOn)) {
				if(checkShowDiscount.Checked || checkShowIns.Checked) {
					if(MessageBox.Show(this,string.Format(Lan.g(this,"Do you want to remove insurance estimates and discounts from printed treatment plan?")),"Open Dental",MessageBoxButtons.YesNo,MessageBoxIcon.Question) != DialogResult.No) {
						checkShowDiscount.Checked=false;
						checkShowIns.Checked=false;
						FillMain();
					}
				}
			}
			#endregion
			if(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Saved
				&& PrefC.GetBool(PrefName.TreatPlanSaveSignedToPdf)
			  && _listTreatPlans[gridPlans.SelectedIndices[0]].Signature!=""
			  && Documents.DocExists(_listTreatPlans[gridPlans.SelectedIndices[0]].DocNum)) 
			{
				//Open PDF and allow user to print from pdf software.
				Cursor=Cursors.WaitCursor;
				Documents.OpenDoc(_listTreatPlans[gridPlans.SelectedIndices[0]].DocNum);
				Cursor=Cursors.Default;
				return;
			}
			Sheet sheetTP=null;
			TreatPlan treatPlan;
			if(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Saved) {
				treatPlan=_listTreatPlans[gridPlans.SelectedIndices[0]].Copy();
				treatPlan.ListProcTPs=ProcTPs.RefreshForTP(treatPlan.TreatPlanNum);
			}
			else {
				treatPlan=_listTreatPlans[gridPlans.SelectedIndices[0]];
				LoadActiveTP(ref treatPlan);
			}
			if(DoPrintUsingSheets()) {
				sheetTP=TreatPlanToSheet(treatPlan);
				SheetPrinting.Print(sheetTP);
			}
			else { //clasic TPs
				PrepImageForPrinting();
				MigraDoc.DocumentObjectModel.Document doc=CreateDocument();
				MigraDoc.Rendering.Printing.MigraDocPrintDocument printdoc=new MigraDoc.Rendering.Printing.MigraDocPrintDocument();
				MigraDoc.Rendering.DocumentRenderer renderer=new MigraDoc.Rendering.DocumentRenderer(doc);
				renderer.PrepareDocument();
				printdoc.Renderer=renderer;
				//we might want to surround some of this with a try-catch
#if DEBUG
				pView=new FormRpPrintPreview();
				pView.printPreviewControl2.Document=printdoc;
				pView.ShowDialog();
#else
				if(PrinterL.SetPrinter(pd2,PrintSituation.TPPerio,PatCur.PatNum,"Treatment plan for printed")){
					printdoc.PrinterSettings=pd2.PrinterSettings;
					printdoc.Print();
				}
#endif
			}
			SaveTPAsDocument(false,sheetTP);
		}

		private void ToolBarMainEmail_Click() {
			if(!Security.IsAuthorized(Permissions.EmailSend)) {
				return;
			}
			#region FuchsOptionOn
			if(PrefC.GetBool(PrefName.FuchsOptionsOn)) {
				if(checkShowDiscount.Checked || checkShowIns.Checked) {
					if(MessageBox.Show(this,string.Format(Lan.g(this,"Do you want to remove insurance estimates and discounts from e-mailed treatment plan?")),"Open Dental",MessageBoxButtons.YesNo,MessageBoxIcon.Question) != DialogResult.No) {
						checkShowDiscount.Checked=false;
						checkShowIns.Checked=false;
						FillMain();
					}
				}
			}
			#endregion
			PrepImageForPrinting();
			string attachPath=EmailAttaches.GetAttachPath();
			Random rnd=new Random();
			string fileName=DateTime.Now.ToString("yyyyMMdd")+"_"+DateTime.Now.TimeOfDay.Ticks.ToString()+rnd.Next(1000).ToString()+".pdf";
			string filePathAndName=FileAtoZ.CombinePaths(attachPath,fileName);
			if(CloudStorage.IsCloudStorage) {
				filePathAndName=PrefC.GetRandomTempFile("pdf");//Save the pdf to a temp file and then upload it the Email Attachment folder later.
			}
			if(gridPlans.SelectedIndices[0]>0 //not the default plan.
				&& PrefC.GetBool(PrefName.TreatPlanSaveSignedToPdf) //preference enabled
			  && _listTreatPlans[gridPlans.SelectedIndices[0]].Signature!="" //and document is signed
			  && Documents.DocExists(_listTreatPlans[gridPlans.SelectedIndices[0]].DocNum)) //and file exists
			{
				string filePathAndNameTemp=Documents.GetPath(_listTreatPlans[gridPlans.SelectedIndices[0]].DocNum);
				//copy file to email attach folder so files will be where they are exptected to be.
				File.Delete(filePathAndName);
				File.Copy(filePathAndNameTemp,filePathAndName);
			}
			else if(DoPrintUsingSheets())	{
				TreatPlan treatPlan;
				if(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Saved) {
					treatPlan=_listTreatPlans[gridPlans.SelectedIndices[0]].Copy();
					treatPlan.ListProcTPs=ProcTPs.RefreshForTP(treatPlan.TreatPlanNum);
				}
				else {
					treatPlan=_listTreatPlans[gridPlans.SelectedIndices[0]];
					LoadActiveTP(ref treatPlan);
				}
				Sheet sheetTP=TreatPlanToSheet(treatPlan);
				SheetPrinting.CreatePdf(sheetTP,filePathAndName,null);
			}
			else{//generate and save a new document from scratch
				MigraDoc.Rendering.PdfDocumentRenderer pdfRenderer=new MigraDoc.Rendering.PdfDocumentRenderer(true,PdfFontEmbedding.Always);
				pdfRenderer.Document=CreateDocument();
				pdfRenderer.RenderDocument();
				pdfRenderer.PdfDocument.Save(filePathAndName);
			}
			//Process.Start(filePathAndName);
			if(CloudStorage.IsCloudStorage) {
				FileAtoZ.Copy(filePathAndName,FileAtoZ.CombinePaths(attachPath,fileName),FileAtoZSourceDestination.LocalToAtoZ);
			}
			EmailMessage message=new EmailMessage();
			message.PatNum=PatCur.PatNum;
			message.ToAddress=PatCur.Email;
			EmailAddress address=EmailAddresses.GetByClinic(PatCur.ClinicNum);
			message.FromAddress=address.GetFrom();
			message.Subject=Lan.g(this,"Treatment Plan");
			EmailAttach attach=new EmailAttach();
			attach.DisplayedFileName="TreatmentPlan.pdf";
			attach.ActualFileName=fileName;
			message.Attachments.Add(attach);
			FormEmailMessageEdit FormE=new FormEmailMessageEdit(message,address);
			FormE.IsNew=true;
			FormE.ShowDialog();
			//if(FormE.DialogResult==DialogResult.OK) {
			//	RefreshCurrentModule();
			//}
		}

		private void PrepImageForPrinting(){
			//linesPrinted=0;
			//ColTotal = new double[10];
			//headingPrinted=false;
			//graphicsPrinted=false;
			//mainPrinted=false;
			//benefitsPrinted=false;
			//notePrinted=false;
			//pagesPrinted=0;
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum))
			{
				return;
			}
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.ChartGraphicColors);
			//prints the graphical tooth chart and legend
			//Panel panelHide=new Panel();
			//panelHide.Size=new Size(600,500);
			//panelHide.BackColor=this.BackColor;
			//panelHide.SendToBack();
			//this.Controls.Add(panelHide);
			toothChart=new ToothChartWrapper();
			toothChart.ColorBackground=listDefs[14].ItemColor;
			toothChart.ColorText=listDefs[15].ItemColor;
			//toothChart.TaoRenderEnabled=true;
			//toothChart.TaoInitializeContexts();
			toothChart.Size=new Size(500,370);
			//toothChart.Location=new Point(-600,-500);//off the visible screen
			//toothChart.SendToBack();
			//ComputerPref computerPref=ComputerPrefs.GetForLocalComputer();
			toothChart.UseHardware=ComputerPrefs.LocalComputer.GraphicsUseHardware;
			toothChart.SetToothNumberingNomenclature((ToothNumberingNomenclature)PrefC.GetInt(PrefName.UseInternationalToothNumbers));
			toothChart.PreferredPixelFormatNumber=ComputerPrefs.LocalComputer.PreferredPixelFormatNum;
			toothChart.DeviceFormat=new ToothChartDirectX.DirectXDeviceFormat(ComputerPrefs.LocalComputer.DirectXFormat);
			//Must be last setting set for preferences, because
																													//this is the line where the device pixel format is
																													//recreated.
																													//The preferred pixel format number changes to the selected pixel format number after a context is chosen.
			toothChart.DrawMode=ComputerPrefs.LocalComputer.GraphicsSimple;
			ComputerPrefs.LocalComputer.PreferredPixelFormatNum=toothChart.PreferredPixelFormatNumber;
			ComputerPrefs.Update(ComputerPrefs.LocalComputer);
			this.Controls.Add(toothChart);
			toothChart.BringToFront();
			toothChart.ResetTeeth();
			ToothInitialList=ToothInitials.Refresh(PatCur.PatNum);
			//first, primary.  That way, you can still set a primary tooth missing afterwards.
			for(int i=0;i<ToothInitialList.Count;i++) {
				if(ToothInitialList[i].InitialType==ToothInitialType.Primary) {
					toothChart.SetPrimary(ToothInitialList[i].ToothNum);
				}
			}
			for(int i=0;i<ToothInitialList.Count;i++) {
				switch(ToothInitialList[i].InitialType) {
					case ToothInitialType.Missing:
						toothChart.SetMissing(ToothInitialList[i].ToothNum);
						break;
					case ToothInitialType.Hidden:
						toothChart.SetHidden(ToothInitialList[i].ToothNum);
						break;
					case ToothInitialType.Rotate:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,ToothInitialList[i].Movement,0,0,0,0,0);
						break;
					case ToothInitialType.TipM:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,ToothInitialList[i].Movement,0,0,0,0);
						break;
					case ToothInitialType.TipB:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,ToothInitialList[i].Movement,0,0,0);
						break;
					case ToothInitialType.ShiftM:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,ToothInitialList[i].Movement,0,0);
						break;
					case ToothInitialType.ShiftO:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,0,ToothInitialList[i].Movement,0);
						break;
					case ToothInitialType.ShiftB:
						toothChart.MoveTooth(ToothInitialList[i].ToothNum,0,0,0,0,0,ToothInitialList[i].Movement);
						break;
					case ToothInitialType.Drawing:
						toothChart.AddDrawingSegment(ToothInitialList[i].Copy());
						break;
				}
			}
			ComputeProcListFiltered();
			DrawProcsGraphics();
			toothChart.AutoFinish=true;
			chartBitmap=toothChart.GetBitmap();
			toothChart.Dispose();
		}

		private List<ProcTP> LoadActiveTP(ref TreatPlan treatPlan) {
			LoadActiveTPData loadActiveData=TreatmentPlanModules.GetLoadActiveTpData(PatCur.PatNum,treatPlan.TreatPlanNum,BenefitList,PatPlanList,
				InsPlanList,dateTimeTP.Value,SubList,PrefC.GetBool(PrefName.InsChecksFrequency));
			List<TreatPlanAttach> listTreatPlanAttaches=loadActiveData.ListTreatPlanAttaches;
			List<Procedure> listProcForTP=loadActiveData.listProcForTP;
			InsPlan priPlanCur=null;
			if(PatPlanList.Count>0) { //primary
				InsSub priSubCur=InsSubs.GetSub(PatPlanList[0].InsSubNum,SubList);
				priPlanCur=InsPlans.GetPlan(priSubCur.PlanNum,InsPlanList);
			}
			InsPlan secPlanCur=null;
			if(PatPlanList.Count>1) { //secondary
				InsSub secSubCur=InsSubs.GetSub(PatPlanList[1].InsSubNum,SubList);
				secPlanCur=InsPlans.GetPlan(secSubCur.PlanNum,InsPlanList);
			}
			ClaimProcList=loadActiveData.ClaimProcList;
			List<ClaimProc> claimProcListOld=ClaimProcList.Select(x => x.Copy()).ToList();
			LoopList=new List<ClaimProcHist>();
			//foreach(Procedure tpProc in listProcForTP){
			if(PrefC.GetBool(PrefName.InsChecksFrequency)) {
				//Taking into account insurance frequency, use the date picker date when loading or when the Refresh button is pressed.  Defaults to today.
				HistList=loadActiveData.HistList??ClaimProcs.GetHistList(PatCur.PatNum,BenefitList,PatPlanList,InsPlanList,-1,dateTimeTP.Value,SubList);
				for(int i=0;i<listProcForTP.Count;i++) {
					listProcForTP[i].ProcDate=dateTimeTP.Value;
					if(listProcForTP[i].ProcNumLab!=0){
							//Lab fees will be calculated and added to looplist when its parent is calculated.
							continue;
					}
					Procedures.ComputeEstimates(listProcForTP[i],PatCur.PatNum,ref ClaimProcList,false,InsPlanList,PatPlanList,BenefitList,HistList,LoopList
						,false,PatCur.Age,SubList,useProcDateOnProc: true
						,listSubLinks: loadActiveData.ListSubLinks);
					//then, add this information to loopList so that the next procedure is aware of it.
					LoopList.AddRange(ClaimProcs.GetHistForProc(ClaimProcList,listProcForTP[i].ProcNum,listProcForTP[i].CodeNum));
				}
				SyncCanadianLabs(ClaimProcList,listProcForTP);
				//We don't want to save the claimprocs if it's a date other than DateTime.Today, since they are calculated using modified date information.
				if(dateTimeTP.Value==DateTime.Today) {
					ClaimProcs.Synch(ref ClaimProcList,claimProcListOld);
				}
			}
			else { 
				for(int i=0;i<listProcForTP.Count;i++) {
					if(listProcForTP[i].ProcNumLab!=0){
							//Lab fees will be calculated and added to looplist when its parent is calculated.
							continue;
					}
					Procedures.ComputeEstimates(listProcForTP[i],PatCur.PatNum,ref ClaimProcList,false,InsPlanList,PatPlanList,BenefitList,HistList,LoopList
						,false,PatCur.Age,SubList,listSubLinks: loadActiveData.ListSubLinks);
					//then, add this information to loopList so that the next procedure is aware of it.
					LoopList.AddRange(ClaimProcs.GetHistForProc(ClaimProcList,listProcForTP[i].ProcNum,listProcForTP[i].CodeNum));
				}
				SyncCanadianLabs(ClaimProcList,listProcForTP);
				//save changes in the list to the database
				ClaimProcs.Synch(ref ClaimProcList,claimProcListOld);
			}
			//claimProcList=ClaimProcs.RefreshForTP(PatCur.PatNum);
			string estimateNote;
			if(!checkShowDiscountNotAutomatic) {
				checkShowDiscount.Checked=false;
			}
			decimal subfee,suballowed,totFee,priIns,secIns,subpriIns,allowed,totPriIns,subsecIns,totSecIns,subdiscount,totDiscount,subpat,totPat,totAllowed;
			subfee=suballowed=totFee=priIns=secIns=subpriIns=allowed=totPriIns=subsecIns=totSecIns=subdiscount=totDiscount=subpat=totPat=totAllowed=0;
			RowsMain.Clear();
			List<ProcTP> retVal=new List<ProcTP>();
			DiscountPlan discountPlan=null;
			if(PatCur.DiscountPlanNum!=0) {
				discountPlan=DiscountPlans.GetPlan(PatCur.DiscountPlanNum);
			}
			for(int i=0;i<listProcForTP.Count;i++) {
				ProcedureCode procCodeCur=ProcedureCodes.GetProcCode(listProcForTP[i].CodeNum);
				TpRow row=new TpRow();
				row.ProcAbbr=procCodeCur.AbbrDesc;
				decimal fee=(decimal)listProcForTP[i].ProcFee;
				int qty=listProcForTP[i].BaseUnits+listProcForTP[i].UnitQty;
				if(qty>0) {
					fee*=qty;
				}
				if(PatCur.DiscountPlanNum!=0) {
					Fee procFee=Fees.GetFee(procCodeCur.CodeNum,discountPlan.FeeSchedNum,listProcForTP[i].ClinicNum,listProcForTP[i].ProvNum);
					if(procFee==null) {//No fee for discount plan's feesched and proc's provider
						Provider patProv=Providers.GetProv(PatCur.PriProv);
						procFee=Fees.GetFee(procCodeCur.CodeNum,patProv.FeeSched,listProcForTP[i].ClinicNum,patProv.ProvNum);
						if(procFee==null) {//No fee for pat's pri prov feesched and pat's pri prov
							patProv=Providers.GetProv(PrefC.GetLong(PrefName.PracticeDefaultProv));
							procFee=Fees.GetFee(procCodeCur.CodeNum,patProv.FeeSched,listProcForTP[i].ClinicNum,patProv.ProvNum);
						}
					}
					decimal procFeeAmt=(procFee == null) ? fee : (decimal)procFee.Amount;
					if(checkShowIns.Checked) {
						priIns=fee-procFeeAmt > 0 ? fee-procFeeAmt : 0;
					}
					else {
						fee=procFeeAmt;
					}
				}
				subfee+=fee;
				totFee+=fee;
				#region ShowMaxDed
				string showPriDeduct="";
				string showSecDeduct="";
				ClaimProc claimproc; //holds the estimate.
				if(PatPlanList.Count>0) { //Primary
					claimproc=ClaimProcs.GetEstimate(ClaimProcList,listProcForTP[i].ProcNum,priPlanCur.PlanNum,PatPlanList[0].InsSubNum);
					if(claimproc==null || claimproc.EstimateNote.Contains("Frequency Limitation")) {
						if(claimproc!=null && claimproc.InsEstTotalOverride!=-1) {
							priIns=(decimal)claimproc.InsEstTotalOverride;
						}
						else { 
							priIns=0;
						}
					}
					else {
						if(checkShowMaxDed.Checked) { //whether visible or not
							priIns=(decimal)ClaimProcs.GetInsEstTotal(claimproc);
							double ded=ClaimProcs.GetDeductibleDisplay(claimproc);
							if(ded>0) {
								showPriDeduct="\r\n"+Lan.g(this,"Pri Deduct Applied: ")+ded.ToString("c");
							}
						}
						else {
							priIns=(decimal)claimproc.BaseEst;
						}
					}
					allowed=ComputeAllowedAmount(listProcForTP[i],claimproc,loadActiveData.ListSubLinks);
				}
				else if(PatCur.DiscountPlanNum==0) { //no primary ins and no discount plan
					priIns=0;
				}
				if(PatPlanList.Count>1) { //Secondary
					claimproc=ClaimProcs.GetEstimate(ClaimProcList,listProcForTP[i].ProcNum,secPlanCur.PlanNum,PatPlanList[1].InsSubNum);
					if(claimproc==null) {
						secIns=0;
					}
					else {
						if(checkShowMaxDed.Checked) {
							secIns=(decimal)ClaimProcs.GetInsEstTotal(claimproc);
							decimal ded=(decimal)ClaimProcs.GetDeductibleDisplay(claimproc);
							if(ded>0) {
								showSecDeduct="\r\n"+Lan.g(this,"Sec Deduct Applied: ")+ded.ToString("c");
							}
						}
						else {
							secIns=(decimal)claimproc.BaseEst;
						}
					}
				} //secondary
				else { //no secondary ins
					secIns=0;
				}
				#endregion ShowMaxDed
				subpriIns+=priIns;
				totPriIns+=priIns;
				subsecIns+=secIns;
				totSecIns+=secIns;
				suballowed+=allowed;
				totAllowed+=allowed;
				decimal discount=(decimal)ClaimProcs.GetTotalWriteOffEstimateDisplay(ClaimProcList,listProcForTP[i].ProcNum); 
				if(!checkShowDiscountNotAutomatic
					 && !checkShowDiscount.Checked
					 && (listProcForTP[i].Discount!=0
							 || ClaimProcs.GetTotalWriteOffEstimateDisplay(ClaimProcList,listProcForTP[i].ProcNum)!=0)) {
					checkShowDiscount.Checked=true;
				}
				discount+=(decimal)listProcForTP[i].Discount;
				subdiscount+=discount;
				totDiscount+=discount;
				decimal pat=fee-priIns-secIns-discount;
				if(pat<0) {
					pat=0;
				}
				subpat+=pat;
				totPat+=pat;
				//Fill TpRow object with information.
				row.Priority=Defs.GetName(DefCat.TxPriorities,listTreatPlanAttaches.FirstOrDefault(x => x.ProcNum==listProcForTP[i].ProcNum).Priority);//(Defs.GetName(DefCat.TxPriorities,listProcForTP[i].Priority));
				row.Tth=(Tooth.ToInternat(listProcForTP[i].ToothNum));
				if(ProcedureCodes.GetProcCode(listProcForTP[i].CodeNum).TreatArea==TreatmentArea.Surf) {
					row.Surf=(Tooth.SurfTidyFromDbToDisplay(listProcForTP[i].Surf,listProcForTP[i].ToothNum));
				}
				else if(ProcedureCodes.GetProcCode(listProcForTP[i].CodeNum).TreatArea==TreatmentArea.Sextant) {
					row.Surf=Tooth.GetSextant(listProcForTP[i].Surf,(ToothNumberingNomenclature)PrefC.GetInt(PrefName.UseInternationalToothNumbers));
				}
				else {
					row.Surf=(listProcForTP[i].Surf); //I think this will properly allow UR, L, etc.
				}
				row.Code=procCodeCur.ProcCode;
				string descript=ProcedureCodes.GetLaymanTerm(listProcForTP[i].CodeNum);
				if(listProcForTP[i].ToothRange!="") {
					descript+=" #"+Tooth.FormatRangeForDisplay(listProcForTP[i].ToothRange);
				}
				if(checkShowMaxDed.Checked) {
					estimateNote=ClaimProcs.GetEstimateNotes(listProcForTP[i].ProcNum,ClaimProcList);
					if(estimateNote!="") {
						descript+="\r\n"+estimateNote;
					}
				}
				row.Description=(descript);
				if(showPriDeduct!="") {
					row.Description+=showPriDeduct;
				}
				if(showSecDeduct!="") {
					row.Description+=showSecDeduct;
				}
				row.Prognosis=Defs.GetName(DefCat.Prognosis,PIn.Long(listProcForTP[i].Prognosis.ToString()));
				row.Dx=Defs.GetValue(DefCat.Diagnosis,PIn.Long(listProcForTP[i].Dx.ToString()));
				row.Fee=fee;
				row.PriIns=priIns;
				row.SecIns=secIns;
				row.Discount=discount;
				row.Pat=pat;
				row.FeeAllowed=allowed;
				row.ColorText=Defs.GetColor(DefCat.TxPriorities,listTreatPlanAttaches.FirstOrDefault(y => y.ProcNum==listProcForTP[i].ProcNum).Priority);
				if(row.ColorText==System.Drawing.Color.White) {
					row.ColorText=System.Drawing.Color.Black;
				}
				//row.Tag=listProcForTP[i].Copy();
				Procedure proc=listProcForTP[i].Copy();
				//procList.Add(proc);
				ProcTP procTP=new ProcTP();
				//procTP.TreatPlanNum=tp.TreatPlanNum;
				procTP.PatNum=PatCur.PatNum;
				procTP.ProcNumOrig=proc.ProcNum;
				procTP.ItemOrder=i;
				procTP.Priority=listTreatPlanAttaches.FirstOrDefault(x => x.ProcNum==proc.ProcNum).Priority;//proc.Priority;
				procTP.ToothNumTP=Tooth.ToInternat(proc.ToothNum);
				if(ProcedureCodes.GetProcCode(proc.CodeNum).TreatArea==TreatmentArea.Surf) {
					procTP.Surf=Tooth.SurfTidyFromDbToDisplay(proc.Surf,proc.ToothNum);
				}
				else {
					procTP.Surf=proc.Surf;//for UR, L, etc.
				}
				procTP.ProcCode=ProcedureCodes.GetStringProcCode(proc.CodeNum);
				procTP.Descript=row.Description;
				procTP.FeeAmt=PIn.Double(row.Fee.ToString());
				procTP.PriInsAmt=PIn.Double(row.PriIns.ToString());
				procTP.SecInsAmt=PIn.Double(row.SecIns.ToString());
				procTP.Discount=PIn.Double(row.Discount.ToString());
				procTP.PatAmt=PIn.Double(row.Pat.ToString());
				procTP.Prognosis=row.Prognosis;
				procTP.Dx=row.Dx;
				procTP.ProcAbbr=row.ProcAbbr;
				procTP.FeeAllowed=PIn.Double(row.FeeAllowed.ToString());
				retVal.Add(procTP);
				procTP.TagOD=proc.ProcNumLab;//Used for selection logic. See gridMain_CellClick(...).
				row.Tag=procTP;
				RowsMain.Add(row);
				#region subtotal
				if(checkShowSubtotals.Checked &&
					 (i==listProcForTP.Count-1 || listTreatPlanAttaches.FirstOrDefault(x => x.ProcNum==listProcForTP[i+1].ProcNum).Priority!=procTP.Priority)) {
					row=new TpRow();
					row.Description=Lan.g("TableTP","Subtotal");
					row.Fee=subfee;
					row.PriIns=subpriIns;
					row.SecIns=subsecIns;
					row.Discount=subdiscount;
					row.Pat=subpat;
					row.FeeAllowed=suballowed;
					row.ColorText=Defs.GetColor(DefCat.TxPriorities,listTreatPlanAttaches.FirstOrDefault(y => y.ProcNum==listProcForTP[i].ProcNum).Priority);
					if(row.ColorText==System.Drawing.Color.White) {
						row.ColorText=System.Drawing.Color.Black;
					}
					row.Bold=true;
					row.ColorLborder=System.Drawing.Color.Black;
					RowsMain.Add(row);
					subfee=0;
					subpriIns=0;
					subsecIns=0;
					subdiscount=0;
					subpat=0;
					suballowed=0;
				}
				#endregion subtotal
			}
			textNote.Text=_listTreatPlans[gridPlans.SelectedIndices[0]].Note;
			HasNoteChanged=false;
			if((_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Saved 
					&& _listTreatPlans[gridPlans.SelectedIndices[0]].Signature!="") 
				|| (_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus==TreatPlanStatus.Inactive 
					&& _listTreatPlans[gridPlans.SelectedIndices[0]].Heading==Lan.g(this,"Unassigned"))) 
			{
				textNote.ReadOnly=true;
			}
			else {
				textNote.ReadOnly=false;
			}
			#region Totals
			if(checkShowTotals.Checked) {
				TpRow row=new TpRow();
				row.Description=Lan.g("TableTP","Total");
				row.Fee=totFee;
				row.PriIns=totPriIns;
				row.SecIns=totSecIns;
				row.Discount=totDiscount;
				row.Pat=totPat;
				row.FeeAllowed=totAllowed;
				row.Bold=true;
				row.ColorText=System.Drawing.Color.Black;
				RowsMain.Add(row);
			}
			#endregion Totals
			treatPlan.ListProcTPs=retVal;
			return retVal;
		}

		///<summary>Refreshes Canadian Lab Fee ClaimProcs in listClaimProcs from the Db, and adds any new Canadian Lab Fee ClaimProcs that were 
		///added to Db when computing estimates</summary>
		private static void SyncCanadianLabs(List<ClaimProc> listClaimProcs,List<Procedure> listProcTp) {
			//1. Get all lab Cp for lab proc nums from Db
			//2. Copy Db lab Cp to original listClaimProcs (Db updates in ClaimProcs.ComputeBaseEst())
			//3. Add any new lab Cp (Db inserts in ClaimProcs.ComputeBaseEst())
			List<long> listLabProcNums=listProcTp.Where(x => x.ProcNumLab!=0).Select(x => x.ProcNum).ToList();
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA") || listLabProcNums.Count==0 ){//Not Canada or no labs to consider.
				return;
			}
			List<long> listOrigLabClaimProcNums=listClaimProcs.Where(x => listLabProcNums.Contains(x.ProcNum)).Select(x => x.ClaimProcNum).ToList();
			//Contains CPs we want to refresh and any that were added. Only look at estimates and cap estimates, see ClaimProcs.RefreshForTP(...)
			List<ClaimProc> listDbLabClaimProcs=ClaimProcs.RefreshForProcs(listLabProcNums)
				.Where(x => x.Status.In(ClaimProcStatus.Estimate,ClaimProcStatus.CapEstimate)).ToList();
			for(int i=0;i<listClaimProcs.Count;i++) {
				long claimProcNum=listClaimProcs[i].ClaimProcNum;
				if(!listOrigLabClaimProcNums.Contains(claimProcNum)) {
					continue;//listClaimProcs[i] is not associated to a lab
				}
				listClaimProcs[i]=ClaimProcs.GetFromList(listDbLabClaimProcs,claimProcNum);//Update listClaimProcs to reflect changed values.
			}
			//New estimates could have been added in ClaimProcs.CanadianLabBaseEstHelper(...).
			listClaimProcs.AddRange(listDbLabClaimProcs.Where(x => !listOrigLabClaimProcNums.Contains(x.ClaimProcNum)).ToList());
		}

		/// <summary>Returns in-memory TreatPlan representing the current treatplan. For displaying current treat-plan before saving it.</summary>
		private TreatPlan GetCurrentTPHelper() {
			TreatPlan retVal=new TreatPlan();
			retVal.Heading=Lan.g(this,"Proposed Treatment Plan");
			retVal.DateTP=DateTimeOD.Today;
			retVal.PatNum=PatCur.PatNum;
			retVal.Note=PrefC.GetString(PrefName.TreatmentPlanNote);
			retVal.ListProcTPs=new List<ProcTP>();
			ProcTP procTP;
			Procedure proc;
			int itemNo=0;
			List<Procedure> procList=new List<Procedure>();
			if(gridMain.SelectedIndices.Length==0 || gridMain.SelectedIndices.All(x=>gridMain.Rows[x].Tag==null)) {
				gridMain.SetSelected(true);//either no rows selected, or only total rows selected.
			}
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				if(gridMain.Rows[gridMain.SelectedIndices[i]].Tag==null) {
					//user must have highlighted a subtotal row.
					continue;
				}
				proc=(Procedure)gridMain.Rows[gridMain.SelectedIndices[i]].Tag;
				procList.Add(proc);
				procTP=new ProcTP();
				//procTP.TreatPlanNum=tp.TreatPlanNum;
				procTP.PatNum=PatCur.PatNum;
				procTP.ProcNumOrig=proc.ProcNum;
				procTP.ItemOrder=itemNo;
				procTP.Priority=proc.Priority;
				procTP.ToothNumTP=Tooth.ToInternat(proc.ToothNum);
				if(ProcedureCodes.GetProcCode(proc.CodeNum).TreatArea==TreatmentArea.Surf) {
					procTP.Surf=Tooth.SurfTidyFromDbToDisplay(proc.Surf,proc.ToothNum);
				}
				else {
					procTP.Surf=proc.Surf;//for UR, L, etc.
				}
				procTP.ProcCode=ProcedureCodes.GetStringProcCode(proc.CodeNum);
				procTP.Descript=RowsMain[gridMain.SelectedIndices[i]].Description;
				if(checkShowFees.Checked) {
					procTP.FeeAmt=PIn.Double(RowsMain[gridMain.SelectedIndices[i]].Fee.ToString());
				}
				if(checkShowIns.Checked) {
					procTP.PriInsAmt=PIn.Double(RowsMain[gridMain.SelectedIndices[i]].PriIns.ToString());
					procTP.SecInsAmt=PIn.Double(RowsMain[gridMain.SelectedIndices[i]].SecIns.ToString());
				}
				if(checkShowDiscount.Checked) {
					procTP.Discount=PIn.Double(RowsMain[gridMain.SelectedIndices[i]].Discount.ToString());
				}
				procTP.PatAmt=PIn.Double(RowsMain[gridMain.SelectedIndices[i]].Pat.ToString());
				procTP.Prognosis=RowsMain[gridMain.SelectedIndices[i]].Prognosis;
				procTP.Dx=RowsMain[gridMain.SelectedIndices[i]].Dx;
				retVal.ListProcTPs.Add(procTP);
				//ProcTPs.InsertOrUpdate(procTP,true);
				itemNo++;
			}
			return retVal;
		}

		///<summary>Simply creates a new sheet from a given treatment plan and adds parameters to the sheet based on which checkboxes are checked.</summary>
		private Sheet TreatPlanToSheet(TreatPlan treatPlan) {
			Sheet sheetTP=SheetUtil.CreateSheet(SheetDefs.GetInternalOrCustom(SheetInternalType.TreatmentPlan),PatCur.PatNum);
			sheetTP.Parameters.Add(new SheetParameter(true,"TreatPlan") { ParamValue=treatPlan });
			sheetTP.Parameters.Add(new SheetParameter(true,"checkShowDiscountNotAutomatic") { ParamValue=checkShowDiscountNotAutomatic });
			sheetTP.Parameters.Add(new SheetParameter(true,"checkShowDiscount") { ParamValue=checkShowDiscount.Checked });
			sheetTP.Parameters.Add(new SheetParameter(true,"checkShowMaxDed") { ParamValue=checkShowMaxDed.Checked });
			sheetTP.Parameters.Add(new SheetParameter(true,"checkShowSubTotals") { ParamValue=checkShowSubtotals.Checked });
			sheetTP.Parameters.Add(new SheetParameter(true,"checkShowTotals") { ParamValue=checkShowTotals.Checked });
			sheetTP.Parameters.Add(new SheetParameter(true,"checkShowCompleted") { ParamValue=checkShowCompleted.Checked });
			sheetTP.Parameters.Add(new SheetParameter(true,"checkShowFees") { ParamValue=checkShowFees.Checked });
			sheetTP.Parameters.Add(new SheetParameter(true,"checkShowIns") { ParamValue=checkShowIns.Checked });
			sheetTP.Parameters.Add(new SheetParameter(true,"toothChartImg") { ParamValue=SheetPrinting.GetToothChartHelper(PatCur.PatNum,checkShowCompleted.Checked,treatPlan) });
			//FormSheetFillEdit FormSFE=new FormSheetFillEdit(sheetTP);
			SheetFiller.FillFields(sheetTP);
			SheetUtil.CalculateHeights(sheetTP);
			return sheetTP;
		}

		private MigraDoc.DocumentObjectModel.Document CreateDocument(){
			MigraDoc.DocumentObjectModel.Document doc= new MigraDoc.DocumentObjectModel.Document();
			doc.DefaultPageSetup.PageWidth=Unit.FromInch(8.5);
			doc.DefaultPageSetup.PageHeight=Unit.FromInch(11);
			doc.DefaultPageSetup.TopMargin=Unit.FromInch(.5);
			doc.DefaultPageSetup.LeftMargin=Unit.FromInch(.5);
			doc.DefaultPageSetup.RightMargin=Unit.FromInch(.5);
			MigraDoc.DocumentObjectModel.Section section=doc.AddSection();
			string text;
			MigraDoc.DocumentObjectModel.Font headingFont=MigraDocHelper.CreateFont(13,true);
			MigraDoc.DocumentObjectModel.Font bodyFontx=MigraDocHelper.CreateFont(9,false);
			MigraDoc.DocumentObjectModel.Font nameFontx=MigraDocHelper.CreateFont(9,true);
			MigraDoc.DocumentObjectModel.Font totalFontx=MigraDocHelper.CreateFont(9,true);
			//Heading---------------------------------------------------------------------------------------------------------------
			#region printHeading
			Paragraph par=section.AddParagraph();
			ParagraphFormat parformat=new ParagraphFormat();
			parformat.Alignment=ParagraphAlignment.Center;
			parformat.Font=MigraDocHelper.CreateFont(10,true);
			par.Format=parformat;
			text=_listTreatPlans[gridPlans.SelectedIndices[0]].Heading;
			par.AddFormattedText(text,headingFont);
			par.AddLineBreak();
			if(PatCur.ClinicNum==0 || !PrefC.HasClinicsEnabled) {
				text=PrefC.GetString(PrefName.PracticeTitle);
				par.AddText(text);
				par.AddLineBreak();
				text=PrefC.GetString(PrefName.PracticePhone);
			}
			else {
				Clinic clinic=Clinics.GetClinic(PatCur.ClinicNum);
				text=clinic.Description;
				par.AddText(text);
				par.AddLineBreak();
				text=clinic.Phone;
			}
			if(text.Length==10 && Application.CurrentCulture.Name=="en-US") {
				text="("+text.Substring(0,3)+")"+text.Substring(3,3)+"-"+text.Substring(6);
			}
			par.AddText(text);
			par.AddLineBreak();
			text=PatCur.GetNameFLFormal()+", DOB "+PatCur.Birthdate.ToShortDateString();
			par.AddText(text);
			par.AddLineBreak();
			if(gridPlans.SelectedIndices[0]>0){//not the default plan
				if(_listTreatPlans[gridPlans.SelectedIndices[0]].ResponsParty!=0){
					text=Lan.g(this,"Responsible Party: ")
						+Patients.GetLim(_listTreatPlans[gridPlans.SelectedIndices[0]].ResponsParty).GetNameFL();
					par.AddText(text);
					par.AddLineBreak();
				}
			}
			if(new[] { TreatPlanStatus.Active,TreatPlanStatus.Inactive }.Contains(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus)) {//Active/Inactive TP
				text=DateTime.Today.ToShortDateString();
			}
			else {
				text=_listTreatPlans[gridPlans.SelectedIndices[0]].DateTP.ToShortDateString();
			}
			par.AddText(text);
			#endregion
			//Graphics---------------------------------------------------------------------------------------------------------------
			#region PrintGraphics
			TextFrame frame;
			int widthDoc=MigraDocHelper.GetDocWidth();
			if(!Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum))
			{	
				frame=MigraDocHelper.CreateContainer(section);
				MigraDocHelper.DrawString(frame,Lan.g(this,"Your")+"\r\n"+Lan.g(this,"Right"),bodyFontx,
					new RectangleF(widthDoc/2-toothChart.Width/2-50,toothChart.Height/2-10,50,100));
				MigraDocHelper.DrawBitmap(frame,chartBitmap,widthDoc/2-toothChart.Width/2,0);
				MigraDocHelper.DrawString(frame,Lan.g(this,"Your")+"\r\n"+Lan.g(this,"Left"),bodyFontx,
					new RectangleF(widthDoc/2+toothChart.Width/2+17,toothChart.Height/2-10,50,100));
				if(checkShowCompleted.Checked) {
					List<Def> listDefs=Defs.GetDefsForCategory(DefCat.ChartGraphicColors,true);
					float yPos=toothChart.Height+15;
					float xPos=225;
					MigraDocHelper.FillRectangle(frame,listDefs[3].ItemColor,xPos,yPos,14,14);
					xPos+=16;
					MigraDocHelper.DrawString(frame,Lan.g(this,"Existing"),bodyFontx,xPos,yPos);
					Graphics g=this.CreateGraphics();//for measuring strings.
					xPos+=(int)g.MeasureString(Lan.g(this,"Existing"),bodyFont).Width+23;
					//The Complete work is actually a combination of EC and C. Usually same color.
					//But just in case they are different, this will show it.
					MigraDocHelper.FillRectangle(frame,listDefs[2].ItemColor,xPos,yPos,7,14);
					xPos+=7;
					MigraDocHelper.FillRectangle(frame,listDefs[1].ItemColor,xPos,yPos,7,14);
					xPos+=9;
					MigraDocHelper.DrawString(frame,Lan.g(this,"Complete"),bodyFontx,xPos,yPos);
					xPos+=(int)g.MeasureString(Lan.g(this,"Complete"),bodyFont).Width+23;
					MigraDocHelper.FillRectangle(frame,listDefs[4].ItemColor,xPos,yPos,14,14);
					xPos+=16;
					MigraDocHelper.DrawString(frame,Lan.g(this,"Referred Out"),bodyFontx,xPos,yPos);
					xPos+=(int)g.MeasureString(Lan.g(this,"Referred Out"),bodyFont).Width+23;
					MigraDocHelper.FillRectangle(frame,listDefs[0].ItemColor,xPos,yPos,14,14);
					xPos+=16;
					MigraDocHelper.DrawString(frame,Lan.g(this,"Treatment Planned"),bodyFontx,xPos,yPos);
					g.Dispose();
				}
			}	
			#endregion
			MigraDocHelper.InsertSpacer(section,10);
			if(!PrefC.GetBool(PrefName.TreatPlanItemized)) {
				FillGridPrint();
				MigraDocHelper.DrawGrid(section,gridPrint);
				gridPrint.Visible=false;
				FillMainDisplay();
			}
			else {
				MigraDocHelper.DrawGrid(section,gridMain);
			}
			//Print benefits----------------------------------------------------------------------------------------------------
			#region printBenefits
			if(checkShowIns.Checked) {
				ODGrid gridFamIns=new ODGrid();
				gridFamIns.TranslationName="";
				this.Controls.Add(gridFamIns);
				gridFamIns.BeginUpdate();
				gridFamIns.Columns.Clear();
				ODGridColumn col=new ODGridColumn("",140);
				gridFamIns.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Primary"),70,HorizontalAlignment.Right);
				gridFamIns.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Secondary"),70,HorizontalAlignment.Right);
				gridFamIns.Columns.Add(col);
				gridFamIns.Rows.Clear();
				ODGridRow row;
				//Annual Family Max--------------------------
				row=new ODGridRow();
				row.Cells.Add(Lan.g(this,"Family Maximum"));
				row.Cells.Add(textFamPriMax.Text);
				row.Cells.Add(textFamSecMax.Text);
				gridFamIns.Rows.Add(row);
				//Family Deductible--------------------------
				row=new ODGridRow();
				row.Cells.Add(Lan.g(this,"Family Deductible"));
				row.Cells.Add(textFamPriDed.Text);
				row.Cells.Add(textFamSecDed.Text);
				gridFamIns.Rows.Add(row);
				//Print Family Insurance-----------------------
				MigraDocHelper.InsertSpacer(section,15);
				par=section.AddParagraph();
				par.Format.Alignment=ParagraphAlignment.Center;
				par.AddFormattedText(Lan.g(this,"Family Insurance Benefits"),totalFontx);
				MigraDocHelper.InsertSpacer(section,2);
				MigraDocHelper.DrawGrid(section,gridFamIns);
				gridFamIns.Dispose();
				//Individual Insurance---------------------
				ODGrid gridIns=new ODGrid();
				gridIns.TranslationName="";
				this.Controls.Add(gridIns);
				gridIns.BeginUpdate();
				gridIns.Columns.Clear();
				col=new ODGridColumn("",140);
				gridIns.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Primary"),70,HorizontalAlignment.Right);
				gridIns.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Secondary"),70,HorizontalAlignment.Right);
				gridIns.Columns.Add(col);
				gridIns.Rows.Clear();
				//Annual Max--------------------------
				row=new ODGridRow();
				row.Cells.Add(Lan.g(this,"Annual Maximum"));
				row.Cells.Add(textPriMax.Text);
				row.Cells.Add(textSecMax.Text);
				gridIns.Rows.Add(row);
				//Deductible--------------------------
				row=new ODGridRow();
				row.Cells.Add(Lan.g(this,"Deductible"));
				row.Cells.Add(textPriDed.Text);
				row.Cells.Add(textSecDed.Text);
				gridIns.Rows.Add(row);
				//Deductible Remaining--------------------------
				row=new ODGridRow();
				row.Cells.Add(Lan.g(this,"Deductible Remaining"));
				row.Cells.Add(textPriDedRem.Text);
				row.Cells.Add(textSecDedRem.Text);
				gridIns.Rows.Add(row);
				//Insurance Used--------------------------
				row=new ODGridRow();
				row.Cells.Add(Lan.g(this,"Insurance Used"));
				row.Cells.Add(textPriUsed.Text);
				row.Cells.Add(textSecUsed.Text);
				gridIns.Rows.Add(row);
				//Pending--------------------------
				row=new ODGridRow();
				row.Cells.Add(Lan.g(this,"Pending"));
				row.Cells.Add(textPriPend.Text);
				row.Cells.Add(textSecPend.Text);
				gridIns.Rows.Add(row);
				//Remaining--------------------------
				row=new ODGridRow();
				row.Cells.Add(Lan.g(this,"Remaining"));
				row.Cells.Add(textPriRem.Text);
				row.Cells.Add(textSecRem.Text);
				gridIns.Rows.Add(row);
				gridIns.EndUpdate();
				//Print Individual Insurance-------------------------
				MigraDocHelper.InsertSpacer(section,15);
				par=section.AddParagraph();
				par.Format.Alignment=ParagraphAlignment.Center;
				par.AddFormattedText(Lan.g(this,"Individual Insurance Benefits"),totalFontx);
				MigraDocHelper.InsertSpacer(section,2);
				MigraDocHelper.DrawGrid(section,gridIns);
				gridIns.Dispose();
			}
			#endregion
			//Note------------------------------------------------------------------------------------------------------------
			#region printNote
			string note="";
			if(gridPlans.SelectedIndices[0]==0) {//current TP
				note=PrefC.GetString(PrefName.TreatmentPlanNote);
			}
			else {
				note=_listTreatPlans[gridPlans.SelectedIndices[0]].Note;
			}
			char nbsp='\u00A0';
			if(note!="") {
				//to prevent collapsing of multiple spaces to single spaces.  We only do double spaces to leave single spaces in place.
				note=note.Replace("  ",nbsp.ToString()+nbsp.ToString());
				MigraDocHelper.InsertSpacer(section,20);
				par=section.AddParagraph(note);
				par.Format.Font=bodyFontx;
				par.Format.Borders.Color=Colors.Gray;
				par.Format.Borders.DistanceFromLeft=Unit.FromInch(.05);
				par.Format.Borders.DistanceFromRight=Unit.FromInch(.05);
				par.Format.Borders.DistanceFromTop=Unit.FromInch(.05);
				par.Format.Borders.DistanceFromBottom=Unit.FromInch(.05);
			}
			#endregion
			//Signature-----------------------------------------------------------------------------------------------------------
			#region signature
			if(gridPlans.SelectedIndices[0]!=0//can't be default TP
				&& _listTreatPlans[gridPlans.SelectedIndices[0]].Signature!="")
			{
				TreatPlan treatPlan = _listTreatPlans[gridPlans.SelectedIndices[0]];
				List<ProcTP> proctpList = ProcTPs.RefreshForTP(_listTreatPlans[gridPlans.SelectedIndices[0]].TreatPlanNum);
				System.Drawing.Bitmap sigBitmap = null;
				SignatureBoxWrapper sigBoxWrapper = new SignatureBoxWrapper();
				sigBoxWrapper.SignatureMode=SignatureBoxWrapper.SigMode.TreatPlan;
				string keyData = TreatPlans.GetKeyDataForSignatureHash(treatPlan,proctpList);
				sigBoxWrapper.FillSignature(treatPlan.SigIsTopaz,keyData,treatPlan.Signature);
				sigBitmap=sigBoxWrapper.GetSigImage();  //Previous tp code did not care if signature is valid or not.
				if(sigBitmap!=null) { 
					frame=MigraDocHelper.CreateContainer(section);
					MigraDocHelper.DrawBitmap(frame,sigBitmap,widthDoc/2-sigBitmap.Width/2,20);
				}
			}
			#endregion
			return doc;
		}

	///<summary>Just used for printing the 3D chart.</summary>
	private void ComputeProcListFiltered() {
		ProcListFiltered=new List<Procedure>();
		//first, add all completed work and conditions. C,EC,EO, and Referred
		for(int i=0;i<ProcList.Count;i++) {
			if(ProcList[i].ProcStatus==ProcStat.C
			   || ProcList[i].ProcStatus==ProcStat.EC
			   || ProcList[i].ProcStatus==ProcStat.EO) 
			{
				if(checkShowCompleted.Checked) {
					ProcListFiltered.Add(ProcList[i]);
				}
			}
			if(ProcList[i].ProcStatus==ProcStat.R) { //always show all referred
				ProcListFiltered.Add(ProcList[i]);
			}
			if(ProcList[i].ProcStatus==ProcStat.Cn) { //always show all conditions.
				ProcListFiltered.Add(ProcList[i]);
			}
		}
		//then add whatever is showing on the selected TP
		//Always select all procedures in TP.
		gridMain.SetSelected(true);
		if(new[] {TreatPlanStatus.Active,TreatPlanStatus.Inactive}.Contains(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus)) { //current plan
			ProcTPSelectList=gridMain.SelectedIndices.Where(x => gridMain.Rows[x].Tag!=null).Select(x => (ProcTP)gridMain.Rows[x].Tag).ToArray();
		}
		Procedure procDummy; //not a real procedure.  Just used to help display on graphical chart
		for(int i=0;i<ProcTPSelectList.Length;i++) {
			procDummy=new Procedure();
			//this next loop is a way to get missing fields like tooth range.  Could be improved.
			for(int j=0;j<ProcList.Count;j++) {
				if(ProcList[j].ProcNum==ProcTPSelectList[i].ProcNumOrig) {
					//but remember that even if the procedure is found, Status might have been altered
					procDummy=ProcList[j].Copy();
				}
			}
			if(Tooth.IsValidEntry(ProcTPSelectList[i].ToothNumTP)) {
				procDummy.ToothNum=Tooth.FromInternat(ProcTPSelectList[i].ToothNumTP);
			}
			if(ProcedureCodes.GetProcCode(ProcTPSelectList[i].ProcCode).TreatArea==TreatmentArea.Surf) {
				procDummy.Surf=Tooth.SurfTidyFromDisplayToDb(ProcTPSelectList[i].Surf,procDummy.ToothNum);
			}
			else {
				procDummy.Surf=ProcTPSelectList[i].Surf; //for quad, arch, etc.
			}
			if(procDummy.ToothRange==null) {
				procDummy.ToothRange="";
			}
			//procDummy.HideGraphical??
			procDummy.ProcStatus=ProcStat.TP;
			procDummy.CodeNum=ProcedureCodes.GetProcCode(ProcTPSelectList[i].ProcCode).CodeNum;
			ProcListFiltered.Add(procDummy);
		}
		ProcListFiltered.Sort(CompareProcListFiltered);
	}

	private int CompareProcListFiltered(Procedure proc1,Procedure proc2) {
			int dateFilter=proc1.ProcDate.CompareTo(proc2.ProcDate);
			if(dateFilter!=0) {
				return dateFilter;
			}
			//Dates are the same, filter by ProcStatus.
			int xIdx=GetProcStatusIdx(proc1.ProcStatus);
			int yIdx=GetProcStatusIdx(proc2.ProcStatus);
			return xIdx.CompareTo(yIdx);
		}

		private decimal ComputeAllowedAmount(Procedure proc,ClaimProc claimProcCur,List<SubstitutionLink> listSubLinks) {
			decimal allowed=0;
			if(claimProcCur!=null) {				
				if(claimProcCur.AllowedOverride!=-1) {//check for allowed override
					allowed=(decimal)claimProcCur.AllowedOverride;
				}
				else {
					allowed=InsPlans.GetAllowedForProc(proc,claimProcCur,InsPlanList,listSubLinks);
					if(allowed==-1) {//Carrier does not have an allowed fee entered
						allowed=0;
					}
				}
			}
			return allowed;
		}

		///<summary>Returns index for sorting based on this order: Cn,TP,R,EO,EC,C,D</summary>
		private int GetProcStatusIdx(ProcStat procStat) {
			switch(procStat) {
				case ProcStat.Cn:
					return 0;
				case ProcStat.TPi:
				case ProcStat.TP:
					return 1;
				case ProcStat.R:
					return 2;
				case ProcStat.EO:
					return 3;
				case ProcStat.EC:
					return 4;
				case ProcStat.C:
					return 5;
				case ProcStat.D:
					return 6;
			}
			return 0;
		}

		private void DrawProcsGraphics() {
			Procedure proc;
			string[] teeth;
			System.Drawing.Color cLight=System.Drawing.Color.White;
			System.Drawing.Color cDark=System.Drawing.Color.White;
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.ChartGraphicColors,true);
			for(int i=0;i<ProcListFiltered.Count;i++) {
				proc=ProcListFiltered[i];
				//if(proc.ProcStatus!=procStat) {
				//  continue;
				//}
				if(proc.HideGraphics) {
					continue;
				}
				if(ProcedureCodes.GetProcCode(proc.CodeNum).PaintType==ToothPaintingType.Extraction && (
					proc.ProcStatus==ProcStat.C
					|| proc.ProcStatus==ProcStat.EC
					|| proc.ProcStatus==ProcStat.EO
					)) {
					continue;//prevents the red X. Missing teeth already handled.
				}
				if(ProcedureCodes.GetProcCode(proc.CodeNum).GraphicColor==System.Drawing.Color.FromArgb(0)) {
					switch(proc.ProcStatus) {
						case ProcStat.C:
							cDark=listDefs[1].ItemColor;
							cLight=listDefs[6].ItemColor;
							break;
						case ProcStat.TP:
							cDark=listDefs[0].ItemColor;
							cLight=listDefs[5].ItemColor;
							break;
						case ProcStat.EC:
							cDark=listDefs[2].ItemColor;
							cLight=listDefs[7].ItemColor;
							break;
						case ProcStat.EO:
							cDark=listDefs[3].ItemColor;
							cLight=listDefs[8].ItemColor;
							break;
						case ProcStat.R:
							cDark=listDefs[4].ItemColor;
							cLight=listDefs[9].ItemColor;
							break;
						case ProcStat.Cn:
							cDark=listDefs[16].ItemColor;
							cLight=listDefs[17].ItemColor;
							break;
					}
				}
				else {
					cDark=ProcedureCodes.GetProcCode(proc.CodeNum).GraphicColor;
					cLight=ProcedureCodes.GetProcCode(proc.CodeNum).GraphicColor;
				}
				switch(ProcedureCodes.GetProcCode(proc.CodeNum).PaintType) {
					case ToothPaintingType.BridgeDark:
						if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,proc.ToothNum)) {
							toothChart.SetPontic(proc.ToothNum,cDark);
						}
						else {
							toothChart.SetCrown(proc.ToothNum,cDark);
						}
						break;
					case ToothPaintingType.BridgeLight:
						if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,proc.ToothNum)) {
							toothChart.SetPontic(proc.ToothNum,cLight);
						}
						else {
							toothChart.SetCrown(proc.ToothNum,cLight);
						}
						break;
					case ToothPaintingType.CrownDark:
						toothChart.SetCrown(proc.ToothNum,cDark);
						break;
					case ToothPaintingType.CrownLight:
						toothChart.SetCrown(proc.ToothNum,cLight);
						break;
					case ToothPaintingType.DentureDark:
						if(proc.Surf=="U") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+2).ToString();
							}
						}
						else if(proc.Surf=="L") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+18).ToString();
							}
						}
						else {
							teeth=proc.ToothRange.Split(new char[] { ',' });
						}
						for(int t=0;t<teeth.Length;t++) {
							if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,teeth[t])) {
								toothChart.SetPontic(teeth[t],cDark);
							}
							else {
								toothChart.SetCrown(teeth[t],cDark);
							}
						}
						break;
					case ToothPaintingType.DentureLight:
						if(proc.Surf=="U") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+2).ToString();
							}
						}
						else if(proc.Surf=="L") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+18).ToString();
							}
						}
						else {
							teeth=proc.ToothRange.Split(new char[] { ',' });
						}
						for(int t=0;t<teeth.Length;t++) {
							if(ToothInitials.ToothIsMissingOrHidden(ToothInitialList,teeth[t])) {
								toothChart.SetPontic(teeth[t],cLight);
							}
							else {
								toothChart.SetCrown(teeth[t],cLight);
							}
						}
						break;
					case ToothPaintingType.Extraction:
						toothChart.SetBigX(proc.ToothNum,cDark);
						break;
					case ToothPaintingType.FillingDark:
						toothChart.SetSurfaceColors(proc.ToothNum,proc.Surf,cDark);
						break;
					case ToothPaintingType.FillingLight:
						toothChart.SetSurfaceColors(proc.ToothNum,proc.Surf,cLight);
						break;
					case ToothPaintingType.Implant:
						toothChart.SetImplant(proc.ToothNum,cDark);
						break;
					case ToothPaintingType.PostBU:
						toothChart.SetBU(proc.ToothNum,cDark);
						break;
					case ToothPaintingType.RCT:
						toothChart.SetRCT(proc.ToothNum,cDark);
						break;
					case ToothPaintingType.Sealant:
						toothChart.SetSealant(proc.ToothNum,cDark);
						break;
					case ToothPaintingType.Veneer:
						toothChart.SetVeneer(proc.ToothNum,cLight);
						break;
					case ToothPaintingType.Watch:
						toothChart.SetWatch(proc.ToothNum,cDark);
						break;
				}
			}
		}

		private void ToolBarMainUpdate_Click() {
			if(!new[] { TreatPlanStatus.Active,TreatPlanStatus.Inactive }.Contains(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus)) {
				MsgBox.Show(this,"The update fee utility only works on current treatment plans, not any saved plans.");
				return;
			}
			if(!MsgBox.Show(this,true,"Update all fees and insurance estimates on this treatment plan to the current fees for this patient?")) {
				return;
			}
			Procedure procCur;
			List<ClaimProc> claimProcList=ClaimProcs.RefreshForTP(PatCur.PatNum);
			for(int i=0;i<ProcListTP.Length;i++) {
				procCur=ProcListTP[i];
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")
					&& procCur.ProcNumLab!=0) 
				{
					continue;//The proc fee for a lab is derived from the lab fee on the parent procedure.
				}
				procCur.ProcFee=Procedures.GetProcFee(PatCur,PatPlanList,SubList,InsPlanList,procCur.CodeNum,procCur.ProvNum,procCur.ClinicNum,
					procCur.MedicalCode);
				Procedures.ComputeEstimates(procCur,PatCur.PatNum,claimProcList,false,InsPlanList,PatPlanList,BenefitList,PatCur.Age,SubList);
				Procedures.UpdateFee(procCur.ProcNum,procCur.ProcFee);
				//no recall synch required 
			}
			long tpNum=_listTreatPlans[gridPlans.SelectedIndices[0]].TreatPlanNum;
			ModuleSelected(PatCur.PatNum);//refreshes TPs
			for(int i=0;i<_listTreatPlans.Count;i++) {
				if(_listTreatPlans[i].TreatPlanNum==tpNum) {
					gridPlans.SetSelected(i,true);
				}
			}
			FillMain();
		}

		private void ToolBarMainCreate_Click(){//Save TP
			//Cannot even click this button if user has not selected one of the treatment plans; Otherwise button is disabled.
			if(!new[]{TreatPlanStatus.Active,TreatPlanStatus.Inactive}.Contains(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus)){
			//if(gridPlans.SelectedIndices[0]!=0){
				MsgBox.Show(this,"An Active or Inactive TP must be selected before saving a TP.  You can highlight some procedures in the TP to save a TP with only those procedures in it.");
				return;
			}
			//Check for duplicate procedures on the appointment before sending the DFT to eCW.
			if(Programs.UsingEcwTightOrFullMode() && Bridges.ECW.AptNum!=0) {
				List<Procedure> procs=Procedures.GetProcsForSingle(Bridges.ECW.AptNum,false);
				string duplicateProcs=ProcedureL.ProcsContainDuplicates(procs);
				if(duplicateProcs!="") {
					MessageBox.Show(duplicateProcs);
					return;
				}
			}
			if(gridMain.SelectedIndices.Length==0){
				gridMain.SetSelected(true);//Select all if none selected.
			}
			List<TreatPlanAttach> listTreatPlanAttaches=TreatPlanAttaches.GetAllForTreatPlan(_listTreatPlans[gridPlans.SelectedIndices[0]].TreatPlanNum);
			TreatPlan tp=new TreatPlan();
			tp.Heading=_listTreatPlans[gridPlans.SelectedIndices[0]].Heading;
			tp.DateTP=DateTimeOD.Today;
			tp.PatNum=PatCur.PatNum;
			tp.Note=_listTreatPlans[gridPlans.SelectedIndices[0]].Note;
			tp.ResponsParty=PatCur.ResponsParty;
			if(_listTreatPlans[gridPlans.SelectedIndices[0]].UserNumPresenter==0) {
				tp.UserNumPresenter=Security.CurUser.UserNum;//TP never had UserNumPresenter updated. probably an old TP
			}
			else {
				tp.UserNumPresenter=_listTreatPlans[gridPlans.SelectedIndices[0]].UserNumPresenter;
			}
			tp.TPType=_listTreatPlans[gridPlans.SelectedIndices[0]].TPType;
			TreatPlans.Insert(tp);
			ProcTP procTP;
			ProcTP procTPGrid;
			Procedure proc;
			int itemNo=0;
			List<Procedure> procList=new List<Procedure>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++){
				if(gridMain.Rows[gridMain.SelectedIndices[i]].Tag==null){
					//user must have highlighted a subtotal row.
					continue;
				}
				procTPGrid=(ProcTP)gridMain.Rows[gridMain.SelectedIndices[i]].Tag;
				proc=Procedures.GetOneProc(((ProcTP)gridMain.Rows[gridMain.SelectedIndices[i]].Tag).ProcNumOrig,true);
				procList.Add(proc);
				procTP=new ProcTP();
				procTP.TreatPlanNum=tp.TreatPlanNum;
				procTP.PatNum=PatCur.PatNum;
				procTP.ProcNumOrig=proc.ProcNum;
				procTP.ItemOrder=itemNo;
				procTP.Priority=listTreatPlanAttaches.FirstOrDefault(x=>x.ProcNum==proc.ProcNum).Priority;
				procTP.ToothNumTP=Tooth.ToInternat(proc.ToothNum);
				if(ProcedureCodes.GetProcCode(proc.CodeNum).TreatArea==TreatmentArea.Surf){
					procTP.Surf=Tooth.SurfTidyFromDbToDisplay(proc.Surf,proc.ToothNum);
				}
				else{
					procTP.Surf=proc.Surf;//for UR, L, etc.
				}
				procTP.ProcCode=ProcedureCodes.GetStringProcCode(proc.CodeNum);
				procTP.Descript=procTPGrid.Descript;
				procTP.FeeAmt=procTPGrid.FeeAmt;
				procTP.PriInsAmt=procTPGrid.PriInsAmt;
				procTP.SecInsAmt=procTPGrid.SecInsAmt;
				procTP.Discount=procTPGrid.Discount;
				procTP.PatAmt=procTPGrid.PatAmt;
				procTP.Prognosis=procTPGrid.Prognosis;
				procTP.Dx=procTPGrid.Dx;
				procTP.ProcAbbr=procTPGrid.ProcAbbr;
				procTP.FeeAllowed=procTPGrid.FeeAllowed;
				ProcTPs.InsertOrUpdate(procTP,true);
				itemNo++;
				#region Canadian Lab Fees
				/*
				proc=(Procedure)gridMain.Rows[gridMain.SelectedIndices[i]].Tag;
				procTP=new ProcTP();
				procTP.TreatPlanNum=tp.TreatPlanNum;
				procTP.PatNum=PatCur.PatNum;
				procTP.ProcNumOrig=proc.ProcNum;
				procTP.ItemOrder=itemNo;
				procTP.Priority=proc.Priority;
				procTP.ToothNumTP="";
				procTP.Surf="";
				procTP.Code=proc.LabProcCode;
				procTP.Descript=gridMain.Rows[gridMain.SelectedIndices[i]]
					.Cells[gridMain.Columns.GetIndex(Lan.g("TableTP","Description"))].Text;
				if(checkShowFees.Checked) {
					procTP.FeeAmt=PIn.PDouble(gridMain.Rows[gridMain.SelectedIndices[i]]
						.Cells[gridMain.Columns.GetIndex(Lan.g("TableTP","Fee"))].Text);
				}
				if(checkShowIns.Checked) {
					procTP.PriInsAmt=PIn.PDouble(gridMain.Rows[gridMain.SelectedIndices[i]]
						.Cells[gridMain.Columns.GetIndex(Lan.g("TableTP","Pri Ins"))].Text);
					procTP.SecInsAmt=PIn.PDouble(gridMain.Rows[gridMain.SelectedIndices[i]]
						.Cells[gridMain.Columns.GetIndex(Lan.g("TableTP","Sec Ins"))].Text);
					procTP.PatAmt=PIn.PDouble(gridMain.Rows[gridMain.SelectedIndices[i]]
						.Cells[gridMain.Columns.GetIndex(Lan.g("TableTP","Pat"))].Text);
				}
				ProcTPs.InsertOrUpdate(procTP,true);
				itemNo++;*/
				#endregion Canadian Lab Fees
			}
			ModuleSelected(PatCur.PatNum);
			for(int i=0;i<_listTreatPlans.Count;i++){
				if(_listTreatPlans[i].TreatPlanNum==tp.TreatPlanNum){
					gridPlans.SetSelected(i,true);
					FillMain();
				}
			}
			//Send TP DFT HL7 message to ECW with embedded PDF when using tight or full integration only.
			if(Programs.UsingEcwTightOrFullMode() && Bridges.ECW.AptNum!=0){
				PrepImageForPrinting();
				MigraDoc.Rendering.PdfDocumentRenderer pdfRenderer=new MigraDoc.Rendering.PdfDocumentRenderer(true,PdfFontEmbedding.Always);
				pdfRenderer.Document=CreateDocument();
				pdfRenderer.RenderDocument();
				MemoryStream ms=new MemoryStream();
				pdfRenderer.PdfDocument.Save(ms);
				byte[] pdfBytes=ms.GetBuffer();
				//#region Remove when testing is complete.
				//string tempFilePath=Path.GetTempFileName();
				//File.WriteAllBytes(tempFilePath,pdfBytes);
				//#endregion
				string pdfDataStr=Convert.ToBase64String(pdfBytes);
				if(HL7Defs.IsExistingHL7Enabled()) {
					//DFT messages that are PDF's only and do not include FT1 segments, so proc list can be empty
					//MessageConstructor.GenerateDFT(procList,EventTypeHL7.P03,PatCur,Patients.GetPat(PatCur.Guarantor),Bridges.ECW.AptNum,"treatment",pdfDataStr);
					MessageHL7 messageHL7=MessageConstructor.GenerateDFT(new List<Procedure>(),EventTypeHL7.P03,PatCur,Patients.GetPat(PatCur.Guarantor),Bridges.ECW.AptNum,"treatment",pdfDataStr);
					if(messageHL7==null) {
						MsgBox.Show(this,"There is no DFT message type defined for the enabled HL7 definition.");
						return;
					}
					HL7Msg hl7Msg=new HL7Msg();
					hl7Msg.AptNum=0;//Prevents the appt complete button from changing to the "Revise" button prematurely.
					hl7Msg.HL7Status=HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
					hl7Msg.MsgText=messageHL7.ToString();
					hl7Msg.PatNum=PatCur.PatNum;
					HL7Msgs.Insert(hl7Msg);
				}
				else {
					Bridges.ECW.SendHL7(Bridges.ECW.AptNum,PatCur.PriProv,PatCur,pdfDataStr,"treatment",true,null);//just pdf, passing null proc list
				}
			}
		}

		private void ToolBarMainSign_Click() {
			if(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus!=TreatPlanStatus.Saved) {
				MsgBox.Show(this,"You may only sign a saved TP, not an Active or Inactive TP.");
				return;
			}
			//string patFolder=ImageStore.GetPatientFolder(PatCur,ImageStore.GetPreferredAtoZpath());
			if(PrefC.GetBool(PrefName.TreatPlanSaveSignedToPdf) //preference enabled
			   && _listTreatPlans[gridPlans.SelectedIndices[0]].Signature!="" //and document is signed
			   && Documents.DocExists(_listTreatPlans[gridPlans.SelectedIndices[0]].DocNum)) //and file exists
			{
				MsgBox.Show(this,"Document already signed and saved to PDF. Unsign treatment plan from edit window to enable resigning.");
				Cursor=Cursors.WaitCursor;
				Documents.OpenDoc(_listTreatPlans[gridPlans.SelectedIndices[0]].DocNum);
				Cursor=Cursors.Default;
				return;//cannot re-sign document.
			}
			if(_listTreatPlans[gridPlans.SelectedIndices[0]].DocNum>0 && !Documents.DocExists(_listTreatPlans[gridPlans.SelectedIndices[0]].DocNum)) {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Unable to open saved treatment plan. Would you like to recreate document using current information?")) {
					return;
				}
			}
			FormTPsign FormT=new FormTPsign();
			if(DoPrintUsingSheets()) {
				TreatPlan treatPlan;
				treatPlan=_listTreatPlans[gridPlans.SelectedIndices[0]].Copy();
				treatPlan.ListProcTPs=ProcTPs.RefreshForTP(treatPlan.TreatPlanNum);
				FormT.SheetTP=TreatPlanToSheet(treatPlan);
				//Just signing the TP, there is no way to print a Treat' Plan from the Sign TP window so suppress the printer dialogs.
				//Users will click the Print TP button from the Treat' Plan module when they want to print.
				FormT.Document=SheetPrinting.Print(FormT.SheetTP,isPrintDocument:false);
				if(FormT.Document==null) {
					//We have already told the user that they probably don't have any printers installed.
					return;
				}
				FormT.TotalPages=Sheets.CalculatePageCount(FormT.SheetTP,SheetPrinting.PrintMargin);
			}
			else {//Classic TPs
				PrepImageForPrinting();
				MigraDoc.DocumentObjectModel.Document doc=CreateDocument();
				MigraDocPrintDocument printdoc=new MigraDoc.Rendering.Printing.MigraDocPrintDocument();
				DocumentRenderer renderer=new MigraDoc.Rendering.DocumentRenderer(doc);
				renderer.PrepareDocument();
				printdoc.Renderer=renderer;
				FormT.Document=printdoc;
				FormT.TotalPages=renderer.FormattedDocument.PageCount;
			}
			FormT.SaveDocDelegate=SaveTPAsDocument;
			FormT.TPcur=_listTreatPlans[gridPlans.SelectedIndices[0]];
			FormT.DoPrintUsingSheets=DoPrintUsingSheets();
			FormT.ShowDialog();
			long tpNum=_listTreatPlans[gridPlans.SelectedIndices[0]].TreatPlanNum;
			ModuleSelected(PatCur.PatNum);//refreshes TPs
			for(int i=0;i<_listTreatPlans.Count;i++) {
				if(_listTreatPlans[i].TreatPlanNum==tpNum) {
					gridPlans.SetSelected(i,true);
				}
			}
			FillMain();
		}

		///<summary>Saves TP as PDF in each image category defined as TP category. 
		/// If TreatPlanSaveSignedToPdf enabled, will default to first non-hidden category if no TP categories are explicitly defined.</summary>
		private List<Document> SaveTPAsDocument(bool isSigSave,Sheet sheet=null) {
			if(DoPrintUsingSheets() && sheet==null) {
				MsgBox.Show(this,"An error has occured with the Treatment Plans to sheets feature.  Please contact support.");
				return new List<Document>();
			}
			List<Document> retVal=new List<Document>();
			//Determine each of the document categories that this TP should be saved to.
			//"R"==TreatmentPlan; see FormDefEditImages.cs
			List<Def> listImageCatDefs=Defs.GetDefsForCategory(DefCat.ImageCats,true);
			List<long> categories= listImageCatDefs.Where(x => x.ItemValue.Contains("R")).Select(x=>x.DefNum).ToList();
			if(isSigSave && categories.Count==0 && PrefC.GetBool(PrefName.TreatPlanSaveSignedToPdf)) {
				//we must save at least one document, pick first non-hidden image category.
				Def imgCat=listImageCatDefs.FirstOrDefault(x => !x.IsHidden);
				if(imgCat==null) {
					MsgBox.Show(this,"Unable to save treatment plan because all image categories are hidden.");
					return new List<Document>();
				}
				categories.Add(imgCat.DefNum);
			}
			//Gauranteed to have at least one image category at this point.
			//Saving pdf to tempfile first simplifies this code, but can use extra bandwidth copying the file to and from the temp directory/Open Dent imgs.
			string tempFile=PrefC.GetRandomTempFile(".pdf");
			string rawBase64="";
			if(DoPrintUsingSheets()) {
				SheetPrinting.CreatePdf(sheet,tempFile,null);
				if(PrefC.AtoZfolderUsed!=DataStorageType.LocalAtoZ) {
					rawBase64=Convert.ToBase64String(System.IO.File.ReadAllBytes(tempFile));//Todo test this
				}
			}
			else {//classic TPs
				MigraDoc.Rendering.PdfDocumentRenderer pdfRenderer;
				pdfRenderer=new MigraDoc.Rendering.PdfDocumentRenderer(false,PdfFontEmbedding.Always);
				pdfRenderer.Document=CreateDocument();
				pdfRenderer.RenderDocument();
				pdfRenderer.Save(tempFile);
				if(PrefC.AtoZfolderUsed!=DataStorageType.LocalAtoZ) {
					using(MemoryStream stream=new MemoryStream()) {
						pdfRenderer.Save(stream,false);
						rawBase64=Convert.ToBase64String(stream.ToArray());
						stream.Close();
					}
				}
			}
			foreach(long docCategory in categories) {//usually only one, but do allow them to be saved once per image category.
				OpenDentBusiness.Document docSave=new Document();
				docSave.DocNum=Documents.Insert(docSave);
				string fileName="TPArchive"+docSave.DocNum;
				docSave.ImgType=ImageType.Document;
				docSave.DateCreated=DateTime.Now;
				docSave.PatNum=PatCur.PatNum;
				docSave.DocCategory=docCategory;
				docSave.Description=fileName;//no extension.
				docSave.RawBase64=rawBase64;//blank if using AtoZfolder
				if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {
					string filePath=ImageStore.GetPatientFolder(PatCur,ImageStore.GetPreferredAtoZpath());
					while(File.Exists(filePath+"\\"+fileName+".pdf")) {
						fileName+="x";
					}
					File.Copy(tempFile,filePath+"\\"+fileName+".pdf");
				}
				else if(CloudStorage.IsCloudStorage) {
					//Upload file to patient's AtoZ folder
					FormProgress FormP=new FormProgress();
					FormP.DisplayText="Uploading Treatment Plan...";
					FormP.NumberFormat="F";
					FormP.NumberMultiplication=1;
					FormP.MaxVal=100;//Doesn't matter what this value is as long as it is greater than 0
					FormP.TickMS=1000;
					OpenDentalCloud.Core.TaskStateUpload state=CloudStorage.UploadAsync(ImageStore.GetPatientFolder(PatCur,"")
						,fileName+".pdf"
						,File.ReadAllBytes(tempFile)
						,new OpenDentalCloud.ProgressHandler(FormP.OnProgress));
					if(FormP.ShowDialog()==DialogResult.Cancel) {
						state.DoCancel=true;
						break;
					}
				}
				docSave.FileName=fileName+".pdf";//file extension used for both DB images and AtoZ images
				Documents.Update(docSave);
				retVal.Add(docSave);
			}
			try {
				File.Delete(tempFile); //cleanup the temp file.
			}
			catch {}
			return retVal;
		}

		///<summary>Returns true if the user has not checked 'Print using classic'.</summary>
		private bool DoPrintUsingSheets() {
			//If the Printing tab is visible and the print classic box is checked, then print classic.
			return (!tabShowSort.TabPages.Contains(tabPagePrint) || !checkPrintClassic.Checked);
		}

		///<summary>Similar method in Account</summary>
		private bool CheckClearinghouseDefaults() {
			if(PrefC.GetLong(PrefName.ClearinghouseDefaultDent)==0) {
				MsgBox.Show(this,"No default dental clearinghouse defined.");
				return false;
			}
			if(PrefC.GetBool(PrefName.ShowFeatureMedicalInsurance) && PrefC.GetLong(PrefName.ClearinghouseDefaultMed)==0) {
				MsgBox.Show(this,"No default medical clearinghouse defined.");
				return false;
			}
			return true;
		}

		private void ToolBarMainPreAuth_Click() {
			if(gridPlans.SelectedIndices.Length==0) {
				return;
			}
			if(!Security.IsAuthorized(Permissions.ClaimView)) {
				return;
			}
			if(!CheckClearinghouseDefaults()) {
				return;
			}
			if(!new[] { TreatPlanStatus.Active,TreatPlanStatus.Inactive }.Contains(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus)) {
				MsgBox.Show(this,"You can only send a preauth from a current TP, not a saved TP.");
				return;
			}
			if(gridMain.SelectedIndices.All(x => gridMain.Rows[x].Tag==null)) {
				MessageBox.Show(Lan.g(this,"Please select procedures first."));
				return;
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canada
				List<int> selectedIndices=new List<int>(gridMain.SelectedIndices);
				if(gridMain.SelectedIndices.Length>7) {
					gridMain.SetSelected(false);
					int selectedLabCount=0;
					foreach(int selectedIdx in selectedIndices) {
						if(gridMain.Rows[selectedIdx].Tag==null) {
							continue;//subtotal row.
						}
						Procedure proc=(Procedures.GetOneProc(((ProcTP)gridMain.Rows[selectedIdx].Tag).ProcNumOrig,false));
						if(proc!=null && proc.ProcNumLab!=0) {
								selectedLabCount++;
						}
						gridMain.SetSelected(selectedIdx,true);
						if(gridMain.SelectedIndices.Length-selectedLabCount>=7) {
							break;//we have found seven procedures.
						}
					}
					if(selectedIndices.FindAll(x => gridMain.Rows[x].Tag!=null).Count-selectedLabCount>7) {//only if they selected more than 7 procedures, not 7 rows.
						MsgBox.Show(this,"Only the first 7 procedures will be selected.  You will need to create another preauth for the remaining procedures.");
					}
				}
			}
			Claim ClaimCur=new Claim();
      FormInsPlanSelect FormIPS=new FormInsPlanSelect(PatCur.PatNum); 
			FormIPS.ViewRelat=true;
			if(FormIPS.SelectedPlan==null) {//Won't be null if there is only one PatPlan
				FormIPS.ShowDialog();
				if(FormIPS.DialogResult!=DialogResult.OK) {
					return;
				}
			}
			ClaimCur.PatNum=PatCur.PatNum;
			ClaimCur.ClaimNote="";
			ClaimCur.ClaimStatus="W";
			ClaimCur.DateSent=DateTimeOD.Today;
			ClaimCur.DateSentOrig=DateTime.MinValue;
			ClaimCur.PlanNum=FormIPS.SelectedPlan.PlanNum;
			ClaimCur.InsSubNum=FormIPS.SelectedSub.InsSubNum;
			ClaimCur.ProvTreat=0;
			ClaimCur.ClaimForm=FormIPS.SelectedPlan.ClaimFormNum;
			List<Procedure> listProcsSelected=new List<Procedure>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++){
				if(gridMain.Rows[gridMain.SelectedIndices[i]].Tag==null){
					continue;//skip any hightlighted subtotal lines
				}
				Procedure proc=Procedures.GetOneProc(((ProcTP)gridMain.Rows[gridMain.SelectedIndices[i]].Tag).ProcNumOrig,false);
				if(Procedures.NoBillIns(proc,ClaimProcList,ClaimCur.PlanNum)) {
					MsgBox.Show(this,"Not allowed to send procedures to insurance that are marked 'Do not bill to ins'.");
					return;
				}
				if(proc.ProcNumLab!=0) {
					continue;//Ignore Canadian labs. Labs are indirectly attached to the claim through a parent procedure
				}
				listProcsSelected.Add(proc);
				if(ClaimCur.ProvTreat==0){//makes sure that at least one prov is set
					ClaimCur.ProvTreat=proc.ProvNum;
				}
				if(!Providers.GetIsSec(proc.ProvNum)) {
					ClaimCur.ProvTreat=proc.ProvNum;
				}
			}
			//Make sure that all procedures share the same place of service and clinic.
			long procClinicNum=-1;
			PlaceOfService placeService=PlaceOfService.Office;//Old behavior was to always use Office.
			for(int i=0;i<listProcsSelected.Count;i++) {
				if(i==0) {
					procClinicNum=listProcsSelected[i].ClinicNum;
					placeService=listProcsSelected[i].PlaceService;
					continue;
				}
				Procedure proc=listProcsSelected[i];
				if(PrefC.HasClinicsEnabled && procClinicNum!=proc.ClinicNum) {
					MsgBox.Show(this,"All procedures do not have the same clinic.");
					return;
				}
				if(!PrefC.GetBool(PrefName.EasyHidePublicHealth) && proc.PlaceService!=placeService) {
					MsgBox.Show(this,"All procedures do not have the same place of service.");
					return;
				}
			}
			//Make the clinic on the claim match the clinic of the procedures.  Use the patients clinic if no procedures selected (shouldn't happen).
			ClaimCur.ClinicNum=(procClinicNum > -1) ? procClinicNum : PatCur.ClinicNum;
			if(Providers.GetIsSec(ClaimCur.ProvTreat)){
				ClaimCur.ProvTreat=PatCur.PriProv;
				//OK if 0, because auto select first in list when open claim
			}
			ClaimCur.ProvBill=Providers.GetBillingProvNum(ClaimCur.ProvTreat,ClaimCur.ClinicNum);
			Provider prov=Providers.GetProv(ClaimCur.ProvBill);
			if(prov.ProvNumBillingOverride!=0) {
				ClaimCur.ProvBill=prov.ProvNumBillingOverride;
			}
			ClaimCur.EmployRelated=YN.No;
      ClaimCur.ClaimType="PreAuth";
			ClaimCur.AttachedFlags="Mail";
			//this could be a little better if we automate figuring out the patrelat
			//instead of making the user enter it:
			ClaimCur.PatRelat=FormIPS.PatRelat;
			ClaimCur.PlaceService=placeService;
			Claims.Insert(ClaimCur);
			ClaimProc ClaimProcCur;
			ClaimProc cpExisting;
			List<ClaimProc> listClaimProcs=new List<ClaimProc>();
			List<long> listCodeNums=new List<long>();//List of codeNums that have had their default note added to the claim.
			foreach(Procedure procCur in listProcsSelected){
				cpExisting=ClaimProcs.GetEstimate(ClaimProcList,procCur.ProcNum,FormIPS.SelectedPlan.PlanNum,FormIPS.SelectedSub.InsSubNum);
				double insPayEst=0;
				if(cpExisting!=null) {
					insPayEst=cpExisting.InsPayEst;
				}
				ProcedureCode procCodeCur=ProcedureCodes.GetProcCode(procCur.CodeNum);
				ClaimProcCur=new ClaimProc();
				ClaimProcs.CreateEst(ClaimProcCur,procCur,FormIPS.SelectedPlan,FormIPS.SelectedSub,0,insPayEst,false,true);//preauth est
        ClaimProcCur.ClaimNum=ClaimCur.ClaimNum;
				ClaimProcCur.NoBillIns=(cpExisting!=null) ? cpExisting.NoBillIns : false;
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA") && cpExisting!=null) {
					//ClaimProc.Percentage is typically set in ClaimProcs.ComputeBaseEst(...), not for pre-auths. 
					ClaimProcCur.Percentage=cpExisting.Percentage;//Used for Canadian preauths with lab procedures.
				}
				ClaimProcCur.FeeBilled=procCur.ProcFee;
				if(FormIPS.SelectedPlan.UseAltCode && (procCodeCur.AlternateCode1!="")){
					ClaimProcCur.CodeSent=procCodeCur.AlternateCode1;
				}
				else if(FormIPS.SelectedPlan.IsMedical && procCur.MedicalCode!=""){
					ClaimProcCur.CodeSent=procCur.MedicalCode;
				}
				else{
					ClaimProcCur.CodeSent=ProcedureCodes.GetStringProcCode(procCur.CodeNum);
					if(ClaimProcCur.CodeSent.Length>5 && ClaimProcCur.CodeSent.Substring(0,1)=="D"){
						ClaimProcCur.CodeSent=ClaimProcCur.CodeSent.Substring(0,5);
					}
					if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
						if(ClaimProcCur.CodeSent.Length>5) { //In Canadian electronic claims, codes can contain letters or numbers and cannot be longer than 5 characters.
							ClaimProcCur.CodeSent=ClaimProcCur.CodeSent.Substring(0,5);
						}
					}
				}
				listClaimProcs.Add(ClaimProcCur);
				if(ClaimCur.ClaimNote==null) {
					ClaimCur.ClaimNote="";
				}
				if(!listCodeNums.Contains(procCodeCur.CodeNum)) {
					if(ClaimCur.ClaimNote.Length > 0 && !string.IsNullOrEmpty(procCodeCur.DefaultClaimNote)) {
						ClaimCur.ClaimNote+="\n";
					}
					ClaimCur.ClaimNote+=procCodeCur.DefaultClaimNote;
					listCodeNums.Add(procCodeCur.CodeNum);
				}
				//ProcCur.Update(ProcOld);
			}
			for(int i=0;i<listClaimProcs.Count;i++) {
				listClaimProcs[i].LineNumber=(byte)(i+1);
				ClaimProcs.Insert(listClaimProcs[i]);
			}
			ProcList=Procedures.Refresh(PatCur.PatNum);
			//ClaimProcList=ClaimProcs.Refresh(PatCur.PatNum);
			Claims.CalculateAndUpdate(ProcList,InsPlanList,ClaimCur,PatPlanList,BenefitList,PatCur.Age,SubList);
			FormClaimEdit FormCE=new FormClaimEdit(ClaimCur,PatCur,FamCur);
			//FormCE.CalculateEstimates(
			FormCE.IsNew=true;//this causes it to delete the claim if cancelling.
			FormCE.ShowDialog();
			ModuleSelected(PatCur.PatNum);
		}

		private void ToolBarMainDiscount_Click() {
			if(!new[] { TreatPlanStatus.Active,TreatPlanStatus.Inactive }.Contains(_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus)) {
				MsgBox.Show(this,"You can only create discounts from a current TP, not a saved TP.");
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {
				gridMain.SetSelected(true);
			}
			List<Procedure> listProcs=Procedures.GetManyProc(gridMain.SelectedIndices.ToList()
				.FindAll(x => gridMain.Rows[x].Tag!=null)
				.Select(x => ((ProcTP)gridMain.Rows[x].Tag).ProcNumOrig)
				.ToList(),false);
			if(listProcs.Count<=0) {
				MsgBox.Show(this,"There are no procedures selected in the treatment plan. Please add to, or select from, procedures attached to the treatment plan before applying a discount");
				return;
			}
			FormTreatmentPlanDiscount FormTPD=new FormTreatmentPlanDiscount(listProcs);
			FormTPD.ShowDialog();
			if(FormTPD.DialogResult==DialogResult.OK) {
				long tpNum=_listTreatPlans[gridPlans.SelectedIndices[0]].TreatPlanNum;
				ModuleSelected(PatCur.PatNum);//refreshes TPs
				for(int i=0;i<_listTreatPlans.Count;i++) {
					if(_listTreatPlans[i].TreatPlanNum==tpNum) {
						gridPlans.SetSelected(i,true);
					}
				}
				FillMain();
			}
		}

		private void gridPreAuth_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.ClaimView)) {
				return;
			}
			Claim claim=Claims.GetClaim(((Claim)ALPreAuth[e.Row]).ClaimNum);//gets attached images.
			if(claim==null) {
				MsgBox.Show(this,"The pre authorization has been deleted.");
				ModuleSelected(PatCur.PatNum);
				return;
			}
			FormClaimEdit FormC=new FormClaimEdit(claim,PatCur,FamCur);
      //FormClaimEdit2.IsPreAuth=true;
			FormC.ShowDialog();
			if(FormC.DialogResult!=DialogResult.OK){
				return;
			}
			ModuleSelected(PatCur.PatNum);    
		}

		private void gridPreAuth_CellClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			if(_listTreatPlans==null 
				|| _listTreatPlans.Count==0
				|| (_listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus!=TreatPlanStatus.Active 
				&& _listTreatPlans[gridPlans.SelectedIndices[0]].TPStatus!=TreatPlanStatus.Inactive))
			{
				return;
			}
			gridMain.SetSelected(false);
			Claim ClaimCur=(Claim)ALPreAuth[e.Row];
			List<ClaimProc> ClaimProcsForClaim=ClaimProcs.RefreshForClaim(ClaimCur.ClaimNum);
			for(int i=0;i<gridMain.Rows.Count;i++){//ProcListTP.Length;i++){
				if(gridMain.Rows[i].Tag==null){
					continue;//must be a subtotal row
				}
				ProcTP procTP=(ProcTP)gridMain.Rows[i].Tag;
				//proc=(Procedure)gridMain.Rows[i].Tag;
				for(int j=0;j<ClaimProcsForClaim.Count;j++){
					if(procTP.ProcNumOrig==ClaimProcsForClaim[j].ProcNum){
						gridMain.SetSelected(i,true);
						CanadianSelectedRowHelper(procTP);
						break;
					}
				}
			}
		}

		private void butInsRem_Click(object sender,EventArgs e) {
			if(PatCur==null) {
				MsgBox.Show(this,"Please select a patient before attempting to view insurance remaining.");
				return;
			}
			FormInsRemain FormIR=new FormInsRemain(PatCur.PatNum);
			FormIR.ShowDialog();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			//Update all unscheduled procs and claimprocs on the active TP to use the currently selected date as the ProcDate
			RefreshModuleScreen();
		}
		
		private void butPlannedAppt_Click(object sender,EventArgs e) {
			if(PatCur==null) {
				MsgBox.Show(this,"Please select a Patient.");
				return;
			}
			if(_listTreatPlans[gridPlans.GetSelectedIndex()].TPStatus!=TreatPlanStatus.Active || gridMain.SelectedIndices.Count()==0) {
				MsgBox.Show(this,"Please select at least one procedure on an Active treatment plan.");
				return;
			}
			//We only care about ShowAppointments in the ChartModuleComponentsToLoad, reduces Db calls
			ChartModuleComponentsToLoad chartComponents=new ChartModuleComponentsToLoad(false);
			chartComponents.ShowAppointments=true;
			//Makes sure ChartModules.rawApt is filled before calling ChartModules.GetPlannedApt
			ChartModules.GetProgNotes(PatCur.PatNum,false,chartComponents);
			int itemOrder=ChartModules.GetPlannedApt(PatCur.PatNum).Rows.Count+1;
			List<long> listSelectedProcNums=gridMain.SelectedGridRows
				.FindAll(x => x.Tag!=null && x.Tag.GetType()==typeof(ProcTP))//ProcTP's only
				.Select(x => ((ProcTP)(x.Tag)).ProcNumOrig).ToList();//get ProcNums
			//mimic calls to CreatePlannedAppt in ContrChart, no need for FillPlanned() since no gridPlanned
			AppointmentL.CreatePlannedAppt(PatCur,itemOrder,listSelectedProcNums);
		}

		private void dateTimeTP_CloseUp(object sender,EventArgs e) {
			//Update all unscheduled procs and claimprocs on the active TP to use the currently selected date as the ProcDate
			RefreshModuleScreen();
		}

		private void textNote_Leave(object sender,EventArgs e) {
			UpdateTPNoteIfNeeded();
		}

		private void textNote_TextChanged(object sender,EventArgs e) {
			HasNoteChanged=true;
		}

		///<summary>Saves TP note to the database if changes were made</summary>
		public void UpdateTPNoteIfNeeded() {
			if(textNote.ReadOnly
				|| !HasNoteChanged
				|| gridPlans.SelectedIndices.Length==0
				|| _listTreatPlans.Count<=gridPlans.SelectedIndices[0]) 
			{
				return;
			}
			_listTreatPlans[gridPlans.SelectedIndices[0]].Note=PIn.String(textNote.Text);
			TreatPlans.Update(_listTreatPlans[gridPlans.SelectedIndices[0]]);
			HasNoteChanged=false;
		}

		private void radioTreatPlanSort_CheckedChanged(object sender,EventArgs e) {
			FormOpenDental.IsTreatPlanSortByTooth=radioTreatPlanSortTooth.Checked;
			FillMainData(); //need to do this so that the data is refreshed as the order of the treatment plan may have changed.
			FillMainDisplay();
		}

		/// <summary></summary>
		private void UpdateToolbarButtons() {
			if(PatCur!=null && _listTreatPlans.Count>0) {
				gridMain.Enabled=true;
				tabShowSort.Enabled=true;
				listSetPr.Enabled=true;
				//panelSide.Enabled=true;
				ToolBarMain.Buttons["Discount"].Enabled=true;
				ToolBarMain.Buttons["PreAuth"].Enabled=true;
				ToolBarMain.Buttons["Update"].Enabled=true;
				//ToolBarMain.Buttons["Create"].Enabled=true;
				ToolBarMain.Buttons["Print"].Enabled=true;
				ToolBarMain.Buttons["Email"].Enabled=true;
				ToolBarMain.Buttons["Sign"].Enabled=true;
				butSaveTP.Enabled=true;
				ToolBarMain.Invalidate();
				if(PatPlanList.Count==0) {//patient doesn't have insurance
					checkShowIns.Checked=false;
					checkShowMaxDed.Visible=false;
					if(PatCur.DiscountPlanNum!=0) {
						checkShowIns.Checked=true;
					}
				}
				else {//patient has insurance
					if(!PrefC.GetBool(PrefName.EasyHideInsurance)) {//if insurance isn't hidden
						checkShowMaxDed.Visible=true;
						if(checkShowFees.Checked) {//if fees are showing
							if(!checkShowInsNotAutomatic) {
								checkShowIns.Checked=true;
							}
							InsSub sub=InsSubs.GetSub(PatPlanList[0].InsSubNum,SubList);
							InsPlan plan=InsPlans.GetPlan(sub.PlanNum,InsPlanList);
						}
					}
				}
			}
			else {
				gridMain.Enabled=false;
				tabShowSort.Enabled=false;
				listSetPr.Enabled=false;
				butSaveTP.Enabled=false;
				//panelSide.Enabled=false;
				ToolBarMain.Buttons["Discount"].Enabled=false;
				ToolBarMain.Buttons["PreAuth"].Enabled=false;
				ToolBarMain.Buttons["Update"].Enabled=false;
				//ToolBarMain.Buttons["Create"].Enabled=false;
				ToolBarMain.Buttons["Print"].Enabled=false;
				ToolBarMain.Buttons["Email"].Enabled=false;
				ToolBarMain.Buttons["Sign"].Enabled=false;
				ToolBarMain.Invalidate();
				//listPreAuth.Enabled=false;
			}
		}
	}



	public class TpRow {
		public string Done;
		public string Priority;
		public string Tth;
		public string Surf;
		public string Code;
		public string Description;
		public string Prognosis;
		public string Dx;
		public string ProcAbbr;
		public decimal Fee;
		public decimal PriIns;
		public decimal SecIns;
		public decimal Discount;
		public decimal Pat;
		public decimal FeeAllowed;
		public System.Drawing.Color ColorText;
		public System.Drawing.Color ColorLborder;
		public bool Bold;
		public object Tag;
	}
}
