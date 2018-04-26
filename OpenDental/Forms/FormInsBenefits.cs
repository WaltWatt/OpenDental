using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.Eclaims;
using CodeBase;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormInsBenefits : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.Button butAdd;
		private OpenDental.UI.ODGrid gridBenefits;
		private CheckBox checkSimplified;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		///<summary>This needs to be set externally.  It will only be altered when user clicks OK and form closes.</summary>
		public List<Benefit> OriginalBenList;
		private long PlanNum;
		private Label label1;
		private ODtextBox textSubscNote;
		private Label labelSubscNote;
		private ValidDouble textAnnualMax;
		private long PatPlanNum;
		///<summary>The subscriber note.  Gets set before form opens.</summary>
		public string Note;
		private ValidDouble textDeductible;
		private Label label2;
		private ValidDouble textDeductPrevent;
		private Label label4;
		private Label label5;
		private ValidNumber textBW;
		private Label label6;
		private GroupBox groupBox1;
		private ComboBox comboExams;
		private ValidNumber textExams;
		private Label label8;
		private ComboBox comboPano;
		private ValidNumber textPano;
		private Label label7;
		private ComboBox comboBW;
		private GroupBox groupBox3;
		private ValidNumber textOrthoPercent;
		private Label label11;
		private ValidDouble textOrthoMax;
		private Label label10;
		private ValidNumber textStand1;
		private ValidNumber textStand2;
		private GroupBox groupBox4;
		private ValidNumber textOralSurg;
		private Label label20;
		private ValidNumber textPerio;
		private Label label19;
		private ValidNumber textEndo;
		private Label label18;
		private ValidNumber textRoutinePrev;
		private Label label9;
		private ValidNumber textCrowns;
		private Label label15;
		private ValidNumber textRestorative;
		private Label label16;
		private ValidNumber textDiagnostic;
		private Label label17;
		private ValidNumber textProsth;
		private Label label21;
		private ValidNumber textMaxProsth;
		private Label label22;
		private ValidNumber textAccident;
		private Label label23;
		private ValidNumber textFlo;
		private Panel panelSimple;
		///<summary>This is the list used to display on this form.</summary>
		private List<Benefit> benefitList;
		private ValidNumber textStand4;
		private Label label24;
		private Label label12;
		private Panel panel3;
		private Panel panel2;
		private Panel panel1;
		///<summary>This is the list of all benefits to display on this form.  Some will be in the simple view, and the rest will be transferred to benefitList for display in the grid.</summary>
		private List<Benefit> benefitListAll;
		private Label label13;
		private Label label14;
		private ValidDouble textAnnualMaxFam;
		private ValidDouble textDeductibleFam;
		private ValidDouble textDeductPreventFam;
		private ValidNumber textXray;
		private Label label25;
		private Label label29;
		private Label label27;
		private Label label26;
		private ValidDouble textDeductDiagFam;
		private ValidDouble textDeductXrayFam;
		private ValidDouble textDeductDiag;
		private Label label3;
		private ValidDouble textDeductXray;
		private bool dontAllowSimplified;
		private ValidNumber textMonth;
		private Label label30;
		private GroupBox groupYear;
		private CheckBox checkCalendarYear;
		///<summary>Set this externally before opening the form.  0 indicates calendar year, otherwise 1-12.  This forces all benefits to conform to this setting.  User can change as a whole.</summary>
		public byte MonthRenew;
		private Label labelOrthoThroughAge;
		private ValidNumber textOrthoAge;

		///<summary>Set this externally before opening the form.</summary>
		public InsSub SubCur;

		///<summary></summary>
		public FormInsBenefits(long planNum,long patPlanNum)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			PatPlanNum=patPlanNum;
			PlanNum=planNum;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInsBenefits));
			this.checkSimplified = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.labelSubscNote = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.comboExams = new System.Windows.Forms.ComboBox();
			this.label24 = new System.Windows.Forms.Label();
			this.textExams = new OpenDental.ValidNumber();
			this.label8 = new System.Windows.Forms.Label();
			this.comboPano = new System.Windows.Forms.ComboBox();
			this.textPano = new OpenDental.ValidNumber();
			this.label7 = new System.Windows.Forms.Label();
			this.comboBW = new System.Windows.Forms.ComboBox();
			this.textBW = new OpenDental.ValidNumber();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.textOrthoAge = new OpenDental.ValidNumber();
			this.labelOrthoThroughAge = new System.Windows.Forms.Label();
			this.textOrthoPercent = new OpenDental.ValidNumber();
			this.label11 = new System.Windows.Forms.Label();
			this.textOrthoMax = new OpenDental.ValidDouble();
			this.label10 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.textDeductDiagFam = new OpenDental.ValidDouble();
			this.textDeductXrayFam = new OpenDental.ValidDouble();
			this.textDeductDiag = new OpenDental.ValidDouble();
			this.label3 = new System.Windows.Forms.Label();
			this.textDeductXray = new OpenDental.ValidDouble();
			this.label29 = new System.Windows.Forms.Label();
			this.label27 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.textDeductPreventFam = new OpenDental.ValidDouble();
			this.textXray = new OpenDental.ValidNumber();
			this.label25 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.textStand4 = new OpenDental.ValidNumber();
			this.textAccident = new OpenDental.ValidNumber();
			this.label23 = new System.Windows.Forms.Label();
			this.textStand2 = new OpenDental.ValidNumber();
			this.textMaxProsth = new OpenDental.ValidNumber();
			this.textDeductPrevent = new OpenDental.ValidDouble();
			this.label22 = new System.Windows.Forms.Label();
			this.textStand1 = new OpenDental.ValidNumber();
			this.textProsth = new OpenDental.ValidNumber();
			this.label21 = new System.Windows.Forms.Label();
			this.textOralSurg = new OpenDental.ValidNumber();
			this.label20 = new System.Windows.Forms.Label();
			this.textPerio = new OpenDental.ValidNumber();
			this.label19 = new System.Windows.Forms.Label();
			this.textEndo = new OpenDental.ValidNumber();
			this.label18 = new System.Windows.Forms.Label();
			this.textRoutinePrev = new OpenDental.ValidNumber();
			this.label9 = new System.Windows.Forms.Label();
			this.textCrowns = new OpenDental.ValidNumber();
			this.label15 = new System.Windows.Forms.Label();
			this.textRestorative = new OpenDental.ValidNumber();
			this.label16 = new System.Windows.Forms.Label();
			this.textDiagnostic = new OpenDental.ValidNumber();
			this.label17 = new System.Windows.Forms.Label();
			this.panelSimple = new System.Windows.Forms.Panel();
			this.label14 = new System.Windows.Forms.Label();
			this.textAnnualMaxFam = new OpenDental.ValidDouble();
			this.textDeductibleFam = new OpenDental.ValidDouble();
			this.label13 = new System.Windows.Forms.Label();
			this.textAnnualMax = new OpenDental.ValidDouble();
			this.textFlo = new OpenDental.ValidNumber();
			this.textDeductible = new OpenDental.ValidDouble();
			this.label30 = new System.Windows.Forms.Label();
			this.groupYear = new System.Windows.Forms.GroupBox();
			this.checkCalendarYear = new System.Windows.Forms.CheckBox();
			this.textMonth = new OpenDental.ValidNumber();
			this.textSubscNote = new OpenDental.ODtextBox();
			this.gridBenefits = new OpenDental.UI.ODGrid();
			this.butDelete = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.panelSimple.SuspendLayout();
			this.groupYear.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkSimplified
			// 
			this.checkSimplified.Checked = true;
			this.checkSimplified.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkSimplified.Location = new System.Drawing.Point(12, 10);
			this.checkSimplified.Name = "checkSimplified";
			this.checkSimplified.Size = new System.Drawing.Size(123, 17);
			this.checkSimplified.TabIndex = 40;
			this.checkSimplified.Text = "Simplified View";
			this.checkSimplified.UseVisualStyleBackColor = true;
			this.checkSimplified.Click += new System.EventHandler(this.checkSimplified_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(62, 39);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 21);
			this.label1.TabIndex = 159;
			this.label1.Text = "Annual Max";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelSubscNote
			// 
			this.labelSubscNote.Location = new System.Drawing.Point(38, 563);
			this.labelSubscNote.Name = "labelSubscNote";
			this.labelSubscNote.Size = new System.Drawing.Size(74, 41);
			this.labelSubscNote.TabIndex = 160;
			this.labelSubscNote.Text = "Notes";
			this.labelSubscNote.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(15, 59);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(147, 21);
			this.label2.TabIndex = 163;
			this.label2.Text = "General Deductible";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(18, 86);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(144, 21);
			this.label4.TabIndex = 167;
			this.label4.Text = "Fluoride Through Age";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(27, 46);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(70, 21);
			this.label5.TabIndex = 168;
			this.label5.Text = "BWs";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(99, 30);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(39, 15);
			this.label6.TabIndex = 170;
			this.label6.Text = "#";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.comboExams);
			this.groupBox1.Controls.Add(this.label24);
			this.groupBox1.Controls.Add(this.textExams);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.comboPano);
			this.groupBox1.Controls.Add(this.textPano);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.comboBW);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.textBW);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Location = new System.Drawing.Point(67, 115);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(282, 114);
			this.groupBox1.TabIndex = 171;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Frequencies";
			// 
			// comboExams
			// 
			this.comboExams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboExams.FormattingEnabled = true;
			this.comboExams.Items.AddRange(new object[] {
            "Every # Years",
            "# Per Year",
            "Every # Months"});
			this.comboExams.Location = new System.Drawing.Point(142, 89);
			this.comboExams.Name = "comboExams";
			this.comboExams.Size = new System.Drawing.Size(136, 21);
			this.comboExams.TabIndex = 11;
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(6, 17);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(270, 13);
			this.label24.TabIndex = 180;
			this.label24.Text = "These do affect estimate calculations.";
			this.label24.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// textExams
			// 
			this.textExams.Location = new System.Drawing.Point(99, 89);
			this.textExams.MaxVal = 255;
			this.textExams.MinVal = 0;
			this.textExams.Name = "textExams";
			this.textExams.Size = new System.Drawing.Size(39, 20);
			this.textExams.TabIndex = 10;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(27, 88);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(70, 21);
			this.label8.TabIndex = 176;
			this.label8.Text = "Exams";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboPano
			// 
			this.comboPano.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPano.FormattingEnabled = true;
			this.comboPano.Items.AddRange(new object[] {
            "Every # Years",
            "# Per Year",
            "Every # Months"});
			this.comboPano.Location = new System.Drawing.Point(142, 68);
			this.comboPano.Name = "comboPano";
			this.comboPano.Size = new System.Drawing.Size(136, 21);
			this.comboPano.TabIndex = 9;
			// 
			// textPano
			// 
			this.textPano.Location = new System.Drawing.Point(99, 68);
			this.textPano.MaxVal = 255;
			this.textPano.MinVal = 0;
			this.textPano.Name = "textPano";
			this.textPano.Size = new System.Drawing.Size(39, 20);
			this.textPano.TabIndex = 8;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(27, 67);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(70, 21);
			this.label7.TabIndex = 173;
			this.label7.Text = "Pano/FMX";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboBW
			// 
			this.comboBW.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBW.FormattingEnabled = true;
			this.comboBW.Items.AddRange(new object[] {
            "Every # Years",
            "# Per Year",
            "Every # Months"});
			this.comboBW.Location = new System.Drawing.Point(142, 47);
			this.comboBW.Name = "comboBW";
			this.comboBW.Size = new System.Drawing.Size(136, 21);
			this.comboBW.TabIndex = 7;
			// 
			// textBW
			// 
			this.textBW.Location = new System.Drawing.Point(99, 47);
			this.textBW.MaxVal = 255;
			this.textBW.MinVal = 0;
			this.textBW.Name = "textBW";
			this.textBW.Size = new System.Drawing.Size(39, 20);
			this.textBW.TabIndex = 6;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.textOrthoAge);
			this.groupBox3.Controls.Add(this.labelOrthoThroughAge);
			this.groupBox3.Controls.Add(this.textOrthoPercent);
			this.groupBox3.Controls.Add(this.label11);
			this.groupBox3.Controls.Add(this.textOrthoMax);
			this.groupBox3.Controls.Add(this.label10);
			this.groupBox3.Location = new System.Drawing.Point(67, 228);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(282, 73);
			this.groupBox3.TabIndex = 175;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Ortho";
			// 
			// textOrthoAge
			// 
			this.textOrthoAge.Location = new System.Drawing.Point(142, 49);
			this.textOrthoAge.MaxVal = 255;
			this.textOrthoAge.MinVal = 0;
			this.textOrthoAge.Name = "textOrthoAge";
			this.textOrthoAge.Size = new System.Drawing.Size(39, 20);
			this.textOrthoAge.TabIndex = 177;
			// 
			// labelOrthoThroughAge
			// 
			this.labelOrthoThroughAge.Location = new System.Drawing.Point(43, 48);
			this.labelOrthoThroughAge.Name = "labelOrthoThroughAge";
			this.labelOrthoThroughAge.Size = new System.Drawing.Size(99, 18);
			this.labelOrthoThroughAge.TabIndex = 176;
			this.labelOrthoThroughAge.Text = "Ortho Through Age";
			this.labelOrthoThroughAge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// textOrthoPercent
			// 
			this.textOrthoPercent.Location = new System.Drawing.Point(142, 29);
			this.textOrthoPercent.MaxVal = 255;
			this.textOrthoPercent.MinVal = 0;
			this.textOrthoPercent.Name = "textOrthoPercent";
			this.textOrthoPercent.Size = new System.Drawing.Size(60, 20);
			this.textOrthoPercent.TabIndex = 13;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(72, 29);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(70, 20);
			this.label11.TabIndex = 174;
			this.label11.Text = "Percentage";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textOrthoMax
			// 
			this.textOrthoMax.Location = new System.Drawing.Point(142, 9);
			this.textOrthoMax.MaxVal = 100000000D;
			this.textOrthoMax.MinVal = -100000000D;
			this.textOrthoMax.Name = "textOrthoMax";
			this.textOrthoMax.Size = new System.Drawing.Size(73, 20);
			this.textOrthoMax.TabIndex = 12;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(47, 13);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(94, 14);
			this.label10.TabIndex = 163;
			this.label10.Text = "Lifetime Max";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.textDeductDiagFam);
			this.groupBox4.Controls.Add(this.textDeductXrayFam);
			this.groupBox4.Controls.Add(this.textDeductDiag);
			this.groupBox4.Controls.Add(this.label3);
			this.groupBox4.Controls.Add(this.textDeductXray);
			this.groupBox4.Controls.Add(this.label29);
			this.groupBox4.Controls.Add(this.label27);
			this.groupBox4.Controls.Add(this.label26);
			this.groupBox4.Controls.Add(this.textDeductPreventFam);
			this.groupBox4.Controls.Add(this.textXray);
			this.groupBox4.Controls.Add(this.label25);
			this.groupBox4.Controls.Add(this.label12);
			this.groupBox4.Controls.Add(this.panel3);
			this.groupBox4.Controls.Add(this.panel2);
			this.groupBox4.Controls.Add(this.panel1);
			this.groupBox4.Controls.Add(this.textStand4);
			this.groupBox4.Controls.Add(this.textAccident);
			this.groupBox4.Controls.Add(this.label23);
			this.groupBox4.Controls.Add(this.textStand2);
			this.groupBox4.Controls.Add(this.textMaxProsth);
			this.groupBox4.Controls.Add(this.textDeductPrevent);
			this.groupBox4.Controls.Add(this.label22);
			this.groupBox4.Controls.Add(this.textStand1);
			this.groupBox4.Controls.Add(this.textProsth);
			this.groupBox4.Controls.Add(this.label21);
			this.groupBox4.Controls.Add(this.textOralSurg);
			this.groupBox4.Controls.Add(this.label20);
			this.groupBox4.Controls.Add(this.textPerio);
			this.groupBox4.Controls.Add(this.label19);
			this.groupBox4.Controls.Add(this.textEndo);
			this.groupBox4.Controls.Add(this.label18);
			this.groupBox4.Controls.Add(this.textRoutinePrev);
			this.groupBox4.Controls.Add(this.label9);
			this.groupBox4.Controls.Add(this.textCrowns);
			this.groupBox4.Controls.Add(this.label15);
			this.groupBox4.Controls.Add(this.textRestorative);
			this.groupBox4.Controls.Add(this.label16);
			this.groupBox4.Controls.Add(this.textDiagnostic);
			this.groupBox4.Controls.Add(this.label17);
			this.groupBox4.Location = new System.Drawing.Point(357, 3);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(439, 298);
			this.groupBox4.TabIndex = 176;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Categories";
			// 
			// textDeductDiagFam
			// 
			this.textDeductDiagFam.Location = new System.Drawing.Point(348, 47);
			this.textDeductDiagFam.MaxVal = 100000000D;
			this.textDeductDiagFam.MinVal = -100000000D;
			this.textDeductDiagFam.Name = "textDeductDiagFam";
			this.textDeductDiagFam.Size = new System.Drawing.Size(62, 20);
			this.textDeductDiagFam.TabIndex = 21;
			// 
			// textDeductXrayFam
			// 
			this.textDeductXrayFam.Location = new System.Drawing.Point(348, 67);
			this.textDeductXrayFam.MaxVal = 100000000D;
			this.textDeductXrayFam.MinVal = -100000000D;
			this.textDeductXrayFam.Name = "textDeductXrayFam";
			this.textDeductXrayFam.Size = new System.Drawing.Size(62, 20);
			this.textDeductXrayFam.TabIndex = 22;
			// 
			// textDeductDiag
			// 
			this.textDeductDiag.Location = new System.Drawing.Point(280, 47);
			this.textDeductDiag.MaxVal = 100000000D;
			this.textDeductDiag.MinVal = -100000000D;
			this.textDeductDiag.Name = "textDeductDiag";
			this.textDeductDiag.Size = new System.Drawing.Size(62, 20);
			this.textDeductDiag.TabIndex = 18;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(275, 12);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(143, 15);
			this.label3.TabIndex = 206;
			this.label3.Text = "Deductibles (if different)";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// textDeductXray
			// 
			this.textDeductXray.Location = new System.Drawing.Point(280, 67);
			this.textDeductXray.MaxVal = 100000000D;
			this.textDeductXray.MinVal = -100000000D;
			this.textDeductXray.Name = "textDeductXray";
			this.textDeductXray.Size = new System.Drawing.Size(62, 20);
			this.textDeductXray.TabIndex = 19;
			// 
			// label29
			// 
			this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label29.Location = new System.Drawing.Point(348, 30);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(58, 15);
			this.label29.TabIndex = 204;
			this.label29.Text = "Family";
			this.label29.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label27
			// 
			this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label27.Location = new System.Drawing.Point(283, 30);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(58, 15);
			this.label27.TabIndex = 203;
			this.label27.Text = "Individual";
			this.label27.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// label26
			// 
			this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label26.Location = new System.Drawing.Point(148, 23);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(60, 22);
			this.label26.TabIndex = 202;
			this.label26.Text = "%";
			this.label26.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// textDeductPreventFam
			// 
			this.textDeductPreventFam.Location = new System.Drawing.Point(348, 87);
			this.textDeductPreventFam.MaxVal = 100000000D;
			this.textDeductPreventFam.MinVal = -100000000D;
			this.textDeductPreventFam.Name = "textDeductPreventFam";
			this.textDeductPreventFam.Size = new System.Drawing.Size(62, 20);
			this.textDeductPreventFam.TabIndex = 23;
			// 
			// textXray
			// 
			this.textXray.Location = new System.Drawing.Point(148, 67);
			this.textXray.MaxVal = 255;
			this.textXray.MinVal = 0;
			this.textXray.Name = "textXray";
			this.textXray.Size = new System.Drawing.Size(60, 20);
			this.textXray.TabIndex = 15;
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(26, 66);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(120, 21);
			this.label25.TabIndex = 200;
			this.label25.Text = "X-Ray (if different)";
			this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label12
			// 
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.Location = new System.Drawing.Point(213, 23);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(67, 22);
			this.label12.TabIndex = 199;
			this.label12.Text = "Quick %";
			this.label12.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel3.Location = new System.Drawing.Point(43, 244);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(246, 1);
			this.panel3.TabIndex = 198;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel2.Location = new System.Drawing.Point(43, 197);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(246, 1);
			this.panel2.TabIndex = 197;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel1.Location = new System.Drawing.Point(43, 110);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(380, 1);
			this.panel1.TabIndex = 196;
			// 
			// textStand4
			// 
			this.textStand4.Location = new System.Drawing.Point(214, 213);
			this.textStand4.MaxVal = 255;
			this.textStand4.MinVal = 0;
			this.textStand4.Name = "textStand4";
			this.textStand4.Size = new System.Drawing.Size(60, 20);
			this.textStand4.TabIndex = 31;
			this.textStand4.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textStand4_KeyUp);
			// 
			// textAccident
			// 
			this.textAccident.Location = new System.Drawing.Point(148, 268);
			this.textAccident.MaxVal = 255;
			this.textAccident.MinVal = 0;
			this.textAccident.Name = "textAccident";
			this.textAccident.Size = new System.Drawing.Size(60, 20);
			this.textAccident.TabIndex = 33;
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(26, 267);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(120, 21);
			this.label23.TabIndex = 194;
			this.label23.Text = "Accident";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textStand2
			// 
			this.textStand2.Location = new System.Drawing.Point(214, 144);
			this.textStand2.MaxVal = 255;
			this.textStand2.MinVal = 0;
			this.textStand2.Name = "textStand2";
			this.textStand2.Size = new System.Drawing.Size(60, 20);
			this.textStand2.TabIndex = 28;
			this.textStand2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textStand2_KeyUp);
			// 
			// textMaxProsth
			// 
			this.textMaxProsth.Location = new System.Drawing.Point(148, 248);
			this.textMaxProsth.MaxVal = 255;
			this.textMaxProsth.MinVal = 0;
			this.textMaxProsth.Name = "textMaxProsth";
			this.textMaxProsth.Size = new System.Drawing.Size(60, 20);
			this.textMaxProsth.TabIndex = 32;
			// 
			// textDeductPrevent
			// 
			this.textDeductPrevent.Location = new System.Drawing.Point(280, 87);
			this.textDeductPrevent.MaxVal = 100000000D;
			this.textDeductPrevent.MinVal = -100000000D;
			this.textDeductPrevent.Name = "textDeductPrevent";
			this.textDeductPrevent.Size = new System.Drawing.Size(62, 20);
			this.textDeductPrevent.TabIndex = 20;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(26, 247);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(120, 21);
			this.label22.TabIndex = 192;
			this.label22.Text = "Maxillofacial Prosth";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textStand1
			// 
			this.textStand1.Location = new System.Drawing.Point(214, 67);
			this.textStand1.MaxVal = 255;
			this.textStand1.MinVal = 0;
			this.textStand1.Name = "textStand1";
			this.textStand1.Size = new System.Drawing.Size(60, 20);
			this.textStand1.TabIndex = 17;
			this.textStand1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textStand1_KeyUp);
			// 
			// textProsth
			// 
			this.textProsth.Location = new System.Drawing.Point(148, 221);
			this.textProsth.MaxVal = 255;
			this.textProsth.MinVal = 0;
			this.textProsth.Name = "textProsth";
			this.textProsth.Size = new System.Drawing.Size(60, 20);
			this.textProsth.TabIndex = 30;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(26, 220);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(120, 21);
			this.label21.TabIndex = 190;
			this.label21.Text = "Prosthodontics";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textOralSurg
			// 
			this.textOralSurg.Location = new System.Drawing.Point(148, 174);
			this.textOralSurg.MaxVal = 255;
			this.textOralSurg.MinVal = 0;
			this.textOralSurg.Name = "textOralSurg";
			this.textOralSurg.Size = new System.Drawing.Size(60, 20);
			this.textOralSurg.TabIndex = 27;
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(26, 173);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(120, 21);
			this.label20.TabIndex = 188;
			this.label20.Text = "Oral Surgery";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPerio
			// 
			this.textPerio.Location = new System.Drawing.Point(148, 154);
			this.textPerio.MaxVal = 255;
			this.textPerio.MinVal = 0;
			this.textPerio.Name = "textPerio";
			this.textPerio.Size = new System.Drawing.Size(60, 20);
			this.textPerio.TabIndex = 26;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(26, 153);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(120, 21);
			this.label19.TabIndex = 186;
			this.label19.Text = "Perio";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textEndo
			// 
			this.textEndo.Location = new System.Drawing.Point(148, 134);
			this.textEndo.MaxVal = 255;
			this.textEndo.MinVal = 0;
			this.textEndo.Name = "textEndo";
			this.textEndo.Size = new System.Drawing.Size(60, 20);
			this.textEndo.TabIndex = 25;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(26, 133);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(120, 21);
			this.label18.TabIndex = 184;
			this.label18.Text = "Endo";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textRoutinePrev
			// 
			this.textRoutinePrev.Location = new System.Drawing.Point(148, 87);
			this.textRoutinePrev.MaxVal = 255;
			this.textRoutinePrev.MinVal = 0;
			this.textRoutinePrev.Name = "textRoutinePrev";
			this.textRoutinePrev.Size = new System.Drawing.Size(60, 20);
			this.textRoutinePrev.TabIndex = 16;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(26, 86);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(120, 21);
			this.label9.TabIndex = 182;
			this.label9.Text = "Routine Preventive";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textCrowns
			// 
			this.textCrowns.Location = new System.Drawing.Point(148, 201);
			this.textCrowns.MaxVal = 255;
			this.textCrowns.MinVal = 0;
			this.textCrowns.Name = "textCrowns";
			this.textCrowns.Size = new System.Drawing.Size(60, 20);
			this.textCrowns.TabIndex = 29;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(26, 200);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(120, 21);
			this.label15.TabIndex = 180;
			this.label15.Text = "Crowns";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textRestorative
			// 
			this.textRestorative.Location = new System.Drawing.Point(148, 114);
			this.textRestorative.MaxVal = 255;
			this.textRestorative.MinVal = 0;
			this.textRestorative.Name = "textRestorative";
			this.textRestorative.Size = new System.Drawing.Size(60, 20);
			this.textRestorative.TabIndex = 24;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(26, 113);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(120, 21);
			this.label16.TabIndex = 178;
			this.label16.Text = "Restorative";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDiagnostic
			// 
			this.textDiagnostic.Location = new System.Drawing.Point(148, 47);
			this.textDiagnostic.MaxVal = 255;
			this.textDiagnostic.MinVal = 0;
			this.textDiagnostic.Name = "textDiagnostic";
			this.textDiagnostic.Size = new System.Drawing.Size(60, 20);
			this.textDiagnostic.TabIndex = 14;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(3, 46);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(143, 21);
			this.label17.TabIndex = 176;
			this.label17.Text = "Diagnostic (includes x-ray)";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panelSimple
			// 
			this.panelSimple.Controls.Add(this.label14);
			this.panelSimple.Controls.Add(this.textAnnualMaxFam);
			this.panelSimple.Controls.Add(this.textDeductibleFam);
			this.panelSimple.Controls.Add(this.label13);
			this.panelSimple.Controls.Add(this.textAnnualMax);
			this.panelSimple.Controls.Add(this.label1);
			this.panelSimple.Controls.Add(this.textFlo);
			this.panelSimple.Controls.Add(this.label2);
			this.panelSimple.Controls.Add(this.groupBox4);
			this.panelSimple.Controls.Add(this.textDeductible);
			this.panelSimple.Controls.Add(this.groupBox3);
			this.panelSimple.Controls.Add(this.groupBox1);
			this.panelSimple.Controls.Add(this.label4);
			this.panelSimple.Location = new System.Drawing.Point(-2, 26);
			this.panelSimple.Name = "panelSimple";
			this.panelSimple.Size = new System.Drawing.Size(804, 305);
			this.panelSimple.TabIndex = 179;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(242, 23);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(76, 15);
			this.label14.TabIndex = 183;
			this.label14.Text = "Family";
			this.label14.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// textAnnualMaxFam
			// 
			this.textAnnualMaxFam.Location = new System.Drawing.Point(243, 39);
			this.textAnnualMaxFam.MaxVal = 100000000D;
			this.textAnnualMaxFam.MinVal = -100000000D;
			this.textAnnualMaxFam.Name = "textAnnualMaxFam";
			this.textAnnualMaxFam.Size = new System.Drawing.Size(73, 20);
			this.textAnnualMaxFam.TabIndex = 2;
			// 
			// textDeductibleFam
			// 
			this.textDeductibleFam.Location = new System.Drawing.Point(243, 59);
			this.textDeductibleFam.MaxVal = 100000000D;
			this.textDeductibleFam.MinVal = -100000000D;
			this.textDeductibleFam.Name = "textDeductibleFam";
			this.textDeductibleFam.Size = new System.Drawing.Size(73, 20);
			this.textDeductibleFam.TabIndex = 4;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(165, 23);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(78, 15);
			this.label13.TabIndex = 179;
			this.label13.Text = "Individual";
			this.label13.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// textAnnualMax
			// 
			this.textAnnualMax.Location = new System.Drawing.Point(166, 39);
			this.textAnnualMax.MaxVal = 100000000D;
			this.textAnnualMax.MinVal = -100000000D;
			this.textAnnualMax.Name = "textAnnualMax";
			this.textAnnualMax.Size = new System.Drawing.Size(73, 20);
			this.textAnnualMax.TabIndex = 1;
			// 
			// textFlo
			// 
			this.textFlo.Location = new System.Drawing.Point(166, 86);
			this.textFlo.MaxVal = 255;
			this.textFlo.MinVal = 0;
			this.textFlo.Name = "textFlo";
			this.textFlo.Size = new System.Drawing.Size(39, 20);
			this.textFlo.TabIndex = 5;
			// 
			// textDeductible
			// 
			this.textDeductible.Location = new System.Drawing.Point(166, 59);
			this.textDeductible.MaxVal = 100000000D;
			this.textDeductible.MinVal = -100000000D;
			this.textDeductible.Name = "textDeductible";
			this.textDeductible.Size = new System.Drawing.Size(73, 20);
			this.textDeductible.TabIndex = 3;
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(89, 15);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(56, 16);
			this.label30.TabIndex = 190;
			this.label30.Text = "Month";
			this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupYear
			// 
			this.groupYear.Controls.Add(this.checkCalendarYear);
			this.groupYear.Controls.Add(this.textMonth);
			this.groupYear.Controls.Add(this.label30);
			this.groupYear.Location = new System.Drawing.Point(141, 3);
			this.groupYear.Name = "groupYear";
			this.groupYear.Size = new System.Drawing.Size(181, 36);
			this.groupYear.TabIndex = 192;
			this.groupYear.TabStop = false;
			this.groupYear.Text = "Benefit Year";
			// 
			// checkCalendarYear
			// 
			this.checkCalendarYear.Location = new System.Drawing.Point(8, 16);
			this.checkCalendarYear.Name = "checkCalendarYear";
			this.checkCalendarYear.Size = new System.Drawing.Size(82, 17);
			this.checkCalendarYear.TabIndex = 41;
			this.checkCalendarYear.Text = "Calendar";
			this.checkCalendarYear.UseVisualStyleBackColor = true;
			this.checkCalendarYear.Click += new System.EventHandler(this.checkCalendarYear_Click);
			// 
			// textMonth
			// 
			this.textMonth.Location = new System.Drawing.Point(145, 13);
			this.textMonth.MaxVal = 12;
			this.textMonth.MinVal = 1;
			this.textMonth.Name = "textMonth";
			this.textMonth.Size = new System.Drawing.Size(28, 20);
			this.textMonth.TabIndex = 42;
			// 
			// textSubscNote
			// 
			this.textSubscNote.AcceptsTab = true;
			this.textSubscNote.BackColor = System.Drawing.SystemColors.Window;
			this.textSubscNote.DetectLinksEnabled = false;
			this.textSubscNote.DetectUrls = false;
			this.textSubscNote.Location = new System.Drawing.Point(112, 560);
			this.textSubscNote.Name = "textSubscNote";
			this.textSubscNote.QuickPasteType = OpenDentBusiness.QuickPasteType.InsPlan;
			this.textSubscNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textSubscNote.Size = new System.Drawing.Size(485, 98);
			this.textSubscNote.TabIndex = 100;
			this.textSubscNote.Text = "1 - Benefits\n2\n3 lines will show here in 46 vert.\n4 lines will show here in 59 ve" +
    "rt.\n5 lines in 72 vert\n6 lines in 85 vert\n7 lines in 98";
			// 
			// gridBenefits
			// 
			this.gridBenefits.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridBenefits.HasAddButton = false;
			this.gridBenefits.HasDropDowns = false;
			this.gridBenefits.HasMultilineHeaders = false;
			this.gridBenefits.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridBenefits.HeaderHeight = 15;
			this.gridBenefits.HScrollVisible = false;
			this.gridBenefits.Location = new System.Drawing.Point(23, 337);
			this.gridBenefits.Name = "gridBenefits";
			this.gridBenefits.ScrollValue = 0;
			this.gridBenefits.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridBenefits.Size = new System.Drawing.Size(574, 180);
			this.gridBenefits.TabIndex = 37;
			this.gridBenefits.Title = "Other Benefits";
			this.gridBenefits.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridBenefits.TitleHeight = 18;
			this.gridBenefits.TranslationName = "TableInsBenefits";
			this.gridBenefits.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridBenefits_CellDoubleClick);
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
			this.butDelete.Location = new System.Drawing.Point(110, 524);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(78, 24);
			this.butDelete.TabIndex = 36;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butClear_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(23, 524);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(78, 24);
			this.butAdd.TabIndex = 35;
			this.butAdd.Text = "Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(719, 591);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 38;
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
			this.butCancel.Location = new System.Drawing.Point(719, 632);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 39;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormInsBenefits
			// 
			this.ClientSize = new System.Drawing.Size(806, 670);
			this.Controls.Add(this.groupYear);
			this.Controls.Add(this.checkSimplified);
			this.Controls.Add(this.panelSimple);
			this.Controls.Add(this.textSubscNote);
			this.Controls.Add(this.labelSubscNote);
			this.Controls.Add(this.gridBenefits);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormInsBenefits";
			this.ShowInTaskbar = false;
			this.Text = "Edit Benefits";
			this.Load += new System.EventHandler(this.FormInsBenefits_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.panelSimple.ResumeLayout(false);
			this.panelSimple.PerformLayout();
			this.groupYear.ResumeLayout(false);
			this.groupYear.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormInsBenefits_Load(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsPlanEdit,true)) {
				butOK.Enabled=false;
				butAdd.Enabled=false;
				butDelete.Enabled=false;
			}
			benefitListAll=new List<Benefit>(OriginalBenList);
			if(CovCats.GetForEbenCat(EbenefitCategory.Accident)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.Crowns)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.Diagnostic)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.Endodontics)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.General)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.MaxillofacialProsth)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.OralSurgery)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.Orthodontics)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.Periodontics)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.Prosthodontics)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.Restorative)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.DiagnosticXRay)==null
				|| Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)
				)
			{
				dontAllowSimplified=true;
				checkSimplified.Checked=false;
				panelSimple.Visible=false;
				gridBenefits.Location=new Point(gridBenefits.Left,groupYear.Bottom+3);
				gridBenefits.Height=butAdd.Top-gridBenefits.Top-5;
			}
			FillCalendarYear();
			FillSimple();
			FillGrid();
			textSubscNote.Text=Note;
			if(SubCur==null) {
				textSubscNote.Visible=false;
				labelSubscNote.Visible=false;
			}
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				checkSimplified.Checked=false;
				checkSimplified.Visible=false;
			}
		}

		private void checkSimplified_Click(object sender,EventArgs e) {
			if(checkSimplified.Checked){
				if(dontAllowSimplified){
					checkSimplified.Checked=false;
					MsgBox.Show(this,"Not allowed to use simplified view until you fix your Insurance Categories from the setup menu.  At least one of each e-benefit category must be present.");
					return;
				}
				gridBenefits.Title=Lan.g(this,"Other Benefits");
				panelSimple.Visible=true;
				gridBenefits.Location=new Point(gridBenefits.Left,panelSimple.Bottom+4);
				gridBenefits.Height=butAdd.Top-gridBenefits.Top-5;
				//FillSimple handles all further logic.
			}
			else{
				if(!ConvertFormToBenefits()){
					checkSimplified.Checked=true;
					return;
				}
				gridBenefits.Title=Lan.g(this,"Benefits");
				panelSimple.Visible=false;
				gridBenefits.Location=new Point(gridBenefits.Left,groupYear.Bottom+3);
				gridBenefits.Height=butAdd.Top-gridBenefits.Top-5;
			}
			FillSimple();
			FillGrid();
		}

		///<summary>This will only be run when the form first opens or if user switches to simple view.  FillGrid should always be run after this.</summary>
		private void FillSimple(){
			if(!panelSimple.Visible){
				benefitList=new List<Benefit>(benefitListAll);
				return;
			}
			textAnnualMax.Text="";
			textDeductible.Text="";
			textAnnualMaxFam.Text="";
			textDeductibleFam.Text="";
			textFlo.Text="";
			textBW.Text="";
			textPano.Text="";
			textExams.Text="";
			comboBW.SelectedIndex=0;
			comboPano.SelectedIndex=0;
			comboExams.SelectedIndex=0;
			textOrthoAge.Text="";
			textOrthoMax.Text="";
			textOrthoPercent.Text="";
			textDeductDiag.Text="";
			textDeductXray.Text="";
			textDeductPrevent.Text="";
			textDeductDiagFam.Text="";
			textDeductXrayFam.Text="";
			textDeductPreventFam.Text="";
			textStand1.Text="";
			textStand2.Text="";
			textStand4.Text="";
			textDiagnostic.Text="";
			textXray.Text="";
			textRoutinePrev.Text="";
			textRestorative.Text="";
			textEndo.Text="";
			textPerio.Text="";
			textOralSurg.Text="";
			textCrowns.Text="";
			textProsth.Text="";
			textMaxProsth.Text="";
			textAccident.Text="";
			benefitList=new List<Benefit>();
			Benefit ben;
			for(int i=0;i<benefitListAll.Count;i++){
				#region Loop
				ben=benefitListAll[i];
				string benProcCode=ProcedureCodes.GetStringProcCode(ben.CodeNum);
				//annual max individual
				if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.Limitations
					&& (ben.CovCatNum==0
					|| ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.General).CovCatNum)
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)
					&& ben.CoverageLevel==BenefitCoverageLevel.Individual)
				{
					textAnnualMax.Text=ben.MonetaryAmt.ToString("n");
				}
				//annual max family
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.Limitations
					&& (ben.CovCatNum==0
					|| ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.General).CovCatNum)
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)
					&& ben.CoverageLevel==BenefitCoverageLevel.Family) 
				{
					textAnnualMaxFam.Text=ben.MonetaryAmt.ToString("n");
				}
				//deductible individual
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.Deductible
					&& (ben.CovCatNum==0
					|| ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.General).CovCatNum)
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)
					&& ben.CoverageLevel==BenefitCoverageLevel.Individual)
				{
					textDeductible.Text=ben.MonetaryAmt.ToString("n");
				}
				//deductible family
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.Deductible
					&& (ben.CovCatNum==0
					|| ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.General).CovCatNum)
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)
					&& ben.CoverageLevel==BenefitCoverageLevel.Family) 
				{
					textDeductibleFam.Text=ben.MonetaryAmt.ToString("n");
				}
				//Flo
				else if((benProcCode=="D1208" || benProcCode=="D1206")
					&& ben.BenefitType==InsBenefitType.Limitations
					//&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Db).CovCatNum//ignored
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Percent==-1
					&& ben.QuantityQualifier==BenefitQuantity.AgeLimit)
					//&& ben.TimePeriod might be none or calYear, or ServiceYear.
				{
					textFlo.Text=ben.Quantity.ToString();
				}
				//Canadian Flo
				else if(CultureInfo.CurrentCulture.Name.EndsWith("CA") && 
					((Canadian.IsQuebec() && benProcCode=="12400")//The proc code is different for Quebec!
					|| (!Canadian.IsQuebec() && benProcCode=="12101"))//The rest of Canada conforms to a standard.
					&& ben.BenefitType==InsBenefitType.Limitations
					//&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Db).CovCatNum//ignored
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Percent==-1
					&& ben.QuantityQualifier==BenefitQuantity.AgeLimit)
				{
					textFlo.Text=ben.Quantity.ToString();
				}
				//BWs group
				else if(ProcedureCodes.GetStringProcCode(ben.CodeNum)=="D0274"//4BW
					&& ben.BenefitType==InsBenefitType.Limitations
					//&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Db).CovCatNum//ignored
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Percent==-1
					&& (ben.QuantityQualifier==BenefitQuantity.Months 
						|| ben.QuantityQualifier==BenefitQuantity.Years
						|| ben.QuantityQualifier==BenefitQuantity.NumberOfServices))
					//&& ben.TimePeriod might be none, or calYear, or ServiceYear, or Years.
				{
					textBW.Text=ben.Quantity.ToString();
					if(ben.QuantityQualifier==BenefitQuantity.Months){
						comboBW.SelectedIndex=2;
					}
					else if(ben.QuantityQualifier==BenefitQuantity.Years){
						comboBW.SelectedIndex=0;
					}
					else{
						comboBW.SelectedIndex=1;//# per year
					}
				}
				//Canadian BWs
				else if(CultureInfo.CurrentCulture.Name.EndsWith("CA")//All of Canada, including Quebec (the proc codes are the same in this instance).
					&& ProcedureCodes.GetStringProcCode(ben.CodeNum)=="02144"//4BW
					&& ben.BenefitType==InsBenefitType.Limitations
					//&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Db).CovCatNum//ignored
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Percent==-1
					&& (ben.QuantityQualifier==BenefitQuantity.Months 
						|| ben.QuantityQualifier==BenefitQuantity.Years
						|| ben.QuantityQualifier==BenefitQuantity.NumberOfServices)) 
				{
					textBW.Text=ben.Quantity.ToString();
					if(ben.QuantityQualifier==BenefitQuantity.Months){
						comboBW.SelectedIndex=2;
					}
					else if(ben.QuantityQualifier==BenefitQuantity.Years){
						comboBW.SelectedIndex=0;
					}
					else{
						comboBW.SelectedIndex=1;//# per year
					}
				}
				//Pano
				else if(ProcedureCodes.GetStringProcCode(ben.CodeNum)=="D0330"//Pano
					&& ben.BenefitType==InsBenefitType.Limitations
					//&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Db).CovCatNum//ignored
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Percent==-1
					&& (ben.QuantityQualifier==BenefitQuantity.Months 
						|| ben.QuantityQualifier==BenefitQuantity.Years
						|| ben.QuantityQualifier==BenefitQuantity.NumberOfServices))
				//&& ben.TimePeriod might be none, or calYear, or ServiceYear, or Years.
				{
					textPano.Text=ben.Quantity.ToString();
					if(ben.QuantityQualifier==BenefitQuantity.Months) {
						comboPano.SelectedIndex=2;
					}
					else if(ben.QuantityQualifier==BenefitQuantity.Years) {
						comboPano.SelectedIndex=0;
					}
					else {
						comboPano.SelectedIndex=1;//# per year
					}
				}
				//Canadian Pano
				else if(CultureInfo.CurrentCulture.Name.EndsWith("CA") && 
					((Canadian.IsQuebec() && ProcedureCodes.GetStringProcCode(ben.CodeNum)=="02600")//The proc code is different for Quebec!
					|| (!Canadian.IsQuebec() && ProcedureCodes.GetStringProcCode(ben.CodeNum)=="02601"))//The rest of Canada conforms to a standard.
					&& ben.BenefitType==InsBenefitType.Limitations
					//&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Db).CovCatNum//ignored
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Percent==-1
					&& (ben.QuantityQualifier==BenefitQuantity.Months 
						|| ben.QuantityQualifier==BenefitQuantity.Years
						|| ben.QuantityQualifier==BenefitQuantity.NumberOfServices)) 
				//&& ben.TimePeriod might be none, or calYear, or ServiceYear, or Years.
				{
					textPano.Text=ben.Quantity.ToString();
					if(ben.QuantityQualifier==BenefitQuantity.Months) {
						comboPano.SelectedIndex=2;
					}
					else if(ben.QuantityQualifier==BenefitQuantity.Years) {
						comboPano.SelectedIndex=0;
					}
					else {
						comboPano.SelectedIndex=1;//# per year
					}
				}
				//Exam group
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.Limitations
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive).CovCatNum
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Percent==-1
					&& (ben.QuantityQualifier==BenefitQuantity.Months 
						|| ben.QuantityQualifier==BenefitQuantity.Years
						|| ben.QuantityQualifier==BenefitQuantity.NumberOfServices))
				{
					textExams.Text=ben.Quantity.ToString();
					if(ben.QuantityQualifier==BenefitQuantity.Months) {
						comboExams.SelectedIndex=2;
					}
					else if(ben.QuantityQualifier==BenefitQuantity.Years) {
						comboExams.SelectedIndex=0;
					}
					else {
						comboExams.SelectedIndex=1;//# per year
					}
				}
				//Ortho Age
				else if(ben.BenefitType==InsBenefitType.Limitations
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Orthodontics).CovCatNum
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Percent==-1
					&& ben.QuantityQualifier==BenefitQuantity.AgeLimit)
				{
					textOrthoAge.Text=ben.Quantity.ToString();
				}
				//OrthoMax
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.Limitations
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Orthodontics).CovCatNum
					&& ben.PatPlanNum==0
					&& ben.Percent==-1
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& ben.CoverageLevel!=BenefitCoverageLevel.Family
					&& ben.TimePeriod==BenefitTimePeriod.Lifetime)
				{
					textOrthoMax.Text=ben.MonetaryAmt.ToString("n");
				}
				//OrthoPercent
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.CoInsurance
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Orthodontics).CovCatNum
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear))
				{
					textOrthoPercent.Text=ben.Percent.ToString();
				}
				//deductible diagnostic individual
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.Deductible
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Diagnostic).CovCatNum
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)
					&& ben.CoverageLevel==BenefitCoverageLevel.Individual) {
					textDeductDiag.Text=ben.MonetaryAmt.ToString("n");
				}
				//deductible diagnostic family
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.Deductible
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Diagnostic).CovCatNum
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)
					&& ben.CoverageLevel==BenefitCoverageLevel.Family) {
					textDeductDiagFam.Text=ben.MonetaryAmt.ToString("n");
				}
				//deductible xray individual
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.Deductible
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.DiagnosticXRay).CovCatNum
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)
					&& ben.CoverageLevel==BenefitCoverageLevel.Individual) {
					textDeductXray.Text=ben.MonetaryAmt.ToString("n");
				}
				//deductible xray family
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.Deductible
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.DiagnosticXRay).CovCatNum
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)
					&& ben.CoverageLevel==BenefitCoverageLevel.Family) {
					textDeductXrayFam.Text=ben.MonetaryAmt.ToString("n");
				}
				//deductible preventive individual
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.Deductible
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive).CovCatNum
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)
					&& ben.CoverageLevel==BenefitCoverageLevel.Individual) {
					textDeductPrevent.Text=ben.MonetaryAmt.ToString("n");
				}
				//deductible preventive family
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.Deductible
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive).CovCatNum
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)
					&& ben.CoverageLevel==BenefitCoverageLevel.Family) {
					textDeductPreventFam.Text=ben.MonetaryAmt.ToString("n");
				}
				//Stand1
				//Stand2
				//Stand4
				//Diagnostic
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.CoInsurance
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Diagnostic).CovCatNum
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear))
				{
					textDiagnostic.Text=ben.Percent.ToString();
				}
				//X-Ray
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.CoInsurance
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.DiagnosticXRay).CovCatNum
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)) 
				{
					textXray.Text=ben.Percent.ToString();
				}
				//RoutinePreventive
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.CoInsurance
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive).CovCatNum
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)) 
				{
					textRoutinePrev.Text=ben.Percent.ToString();
				}
				//Restorative
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.CoInsurance
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Restorative).CovCatNum
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)) 
				{
					textRestorative.Text=ben.Percent.ToString();
				}
				//Endo
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.CoInsurance
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Endodontics).CovCatNum
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)) 
				{
					textEndo.Text=ben.Percent.ToString();
				}
				//Perio
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.CoInsurance
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Periodontics).CovCatNum
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)) {
					textPerio.Text=ben.Percent.ToString();
				}
				//OralSurg
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.CoInsurance
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.OralSurgery).CovCatNum
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)) {
					textOralSurg.Text=ben.Percent.ToString();
				}
				//Crowns
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.CoInsurance
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Crowns).CovCatNum
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)) {
					textCrowns.Text=ben.Percent.ToString();
				}
				//Prosth
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.CoInsurance
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Prosthodontics).CovCatNum
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)) {
					textProsth.Text=ben.Percent.ToString();
				}
				//MaxProsth
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.CoInsurance
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.MaxillofacialProsth).CovCatNum
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)) {
					textMaxProsth.Text=ben.Percent.ToString();
				}
				//Accident
				else if(ben.CodeNum==0
					&& ben.BenefitType==InsBenefitType.CoInsurance
					&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Accident).CovCatNum
					&& ben.MonetaryAmt==-1
					&& ben.PatPlanNum==0
					&& ben.Quantity==0
					&& ben.QuantityQualifier==BenefitQuantity.None
					&& (ben.TimePeriod==BenefitTimePeriod.CalendarYear	|| ben.TimePeriod==BenefitTimePeriod.ServiceYear)) {
					textAccident.Text=ben.Percent.ToString();
				}
				//any benefit that didn't get handled above
				else{
					benefitList.Add(ben);
				}
				#endregion Loop
			}
			if(textDiagnostic.Text !="" && textDiagnostic.Text==textRoutinePrev.Text
				&& textDiagnostic.Text==textXray.Text)
			{
				textStand1.Text=textDiagnostic.Text;
			}
			if(textRestorative.Text !="" && textRestorative.Text==textEndo.Text 
				&& textRestorative.Text==textPerio.Text	&& textRestorative.Text==textOralSurg.Text)
			{
				textStand2.Text=textRestorative.Text;
			}
			if(textCrowns.Text !="" && textCrowns.Text==textProsth.Text) {
				textStand4.Text=textCrowns.Text;
			}
		}

		private void textStand1_KeyUp(object sender,KeyEventArgs e) {
			textDiagnostic.Text=textStand1.Text;
			textXray.Text=textStand1.Text;
			textRoutinePrev.Text=textStand1.Text;
		}

		private void textStand2_KeyUp(object sender,KeyEventArgs e) {
			textRestorative.Text=textStand2.Text;
			textEndo.Text=textStand2.Text;
			textPerio.Text=textStand2.Text;
			textOralSurg.Text=textStand2.Text;
		}

		private void textStand4_KeyUp(object sender,KeyEventArgs e) {
			textCrowns.Text=textStand4.Text;
			textProsth.Text=textStand4.Text;
		}

		///<summary>only run once at startup.</summary>
		private void FillCalendarYear() {
			bool isCalendar= MonthRenew==0;
			for(int i=0;i<benefitListAll.Count;i++) {
				if(benefitListAll[i].TimePeriod==BenefitTimePeriod.CalendarYear && !isCalendar) {
					benefitListAll[i].TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				if(benefitListAll[i].TimePeriod==BenefitTimePeriod.ServiceYear && isCalendar) {
					benefitListAll[i].TimePeriod=BenefitTimePeriod.CalendarYear;
				}
			}
			if(isCalendar) {
				checkCalendarYear.Checked=true;
				textMonth.Text="";
				textMonth.Enabled=false;
			}
			else {
				checkCalendarYear.Checked=false;
				textMonth.Text=MonthRenew.ToString();
				textMonth.Enabled=true;
			}
		}

		///<summary>This only fills the grid on the screen.  It does not get any data from the database.</summary>
		private void FillGrid() {
			benefitList.Sort();
			gridBenefits.BeginUpdate();
			gridBenefits.Columns.Clear();
			ODGridColumn col=new ODGridColumn("Pat",35);
			gridBenefits.Columns.Add(col);
			col=new ODGridColumn("Level",60);
			gridBenefits.Columns.Add(col);
			col=new ODGridColumn("Type",90);
			gridBenefits.Columns.Add(col);
			col=new ODGridColumn("Category",90);
			gridBenefits.Columns.Add(col);
			col=new ODGridColumn("%",35);//,HorizontalAlignment.Right);
			gridBenefits.Columns.Add(col);
			col=new ODGridColumn("Amt",50);//,HorizontalAlignment.Right);
			gridBenefits.Columns.Add(col);
			col=new ODGridColumn("Time Period",80);
			gridBenefits.Columns.Add(col);
			col=new ODGridColumn("Quantity",115);
			gridBenefits.Columns.Add(col);
			gridBenefits.Rows.Clear();
			ODGridRow row;
			//bool isCalendarYear=true;
			for(int i=0;i<benefitList.Count;i++) {
				row=new ODGridRow();
				if(benefitList[i].PatPlanNum==0) {//attached to plan
					row.Cells.Add("");
				}
				else {
					row.Cells.Add("X");
				}
				if(benefitList[i].CoverageLevel==BenefitCoverageLevel.None){
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(Lan.g("enumBenefitCoverageLevel",benefitList[i].CoverageLevel.ToString()));
				}
				if(benefitList[i].BenefitType==InsBenefitType.CoInsurance && benefitList[i].Percent != -1) {
					row.Cells.Add("%");
				}
				//else if(((Benefit)benefitList[i]).BenefitType==InsBenefitType.Limitations
				//	&& (((Benefit)benefitList[i]).TimePeriod==BenefitTimePeriod.ServiceYear
				//	|| ((Benefit)benefitList[i]).TimePeriod==BenefitTimePeriod.CalendarYear)
				//	&& ((Benefit)benefitList[i]).QuantityQualifier==BenefitQuantity.None) {//annual max
				//	row.Cells.Add(Lan.g(this,"Annual Max"));
				//}
				else {
					row.Cells.Add(Lan.g("enumInsBenefitType",benefitList[i].BenefitType.ToString()));
				}
				if(benefitList[i].CodeNum==0) {
					row.Cells.Add(CovCats.GetDesc(benefitList[i].CovCatNum));
				}
				else {
					ProcedureCode proccode=ProcedureCodes.GetProcCode(benefitList[i].CodeNum);
					row.Cells.Add(proccode.ProcCode+"-"+proccode.AbbrDesc);
				}
				if(benefitList[i].Percent == -1) {
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(benefitList[i].Percent.ToString());
				}
				if(benefitList[i].MonetaryAmt==-1) {
					row.Cells.Add("");
				}
				//else{
				//	if(benefitList[i].BenefitType==InsBenefitType.Deductible) {
				//		row.Cells.Add(((Benefit)benefitList[i]).MonetaryAmt.ToString("n0"));
				//	}
				//	else {
				//		
				//	}
				//}
				else {
					row.Cells.Add(benefitList[i].MonetaryAmt.ToString("n0"));
				}
				if(benefitList[i].TimePeriod==BenefitTimePeriod.None) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(Lan.g("enumBenefitTimePeriod",benefitList[i].TimePeriod.ToString()));
				}
				if(benefitList[i].Quantity>0) {
					row.Cells.Add(benefitList[i].Quantity.ToString()+" "
						+Lan.g("enumBenefitQuantity",benefitList[i].QuantityQualifier.ToString()));
				}
				else {
					row.Cells.Add("");
				}
				gridBenefits.Rows.Add(row);
			}
			gridBenefits.EndUpdate();
		}

		private void gridBenefits_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e){
			int benefitListI=benefitList.IndexOf(benefitList[e.Row]);
			int benefitListAllI=benefitListAll.IndexOf(benefitList[e.Row]);
			FormBenefitEdit FormB=new FormBenefitEdit(PatPlanNum,PlanNum);
			FormB.BenCur=benefitList[e.Row];
			FormB.ShowDialog();
			if(FormB.BenCur==null){//user deleted
				benefitList.RemoveAt(benefitListI);
				benefitListAll.RemoveAt(benefitListAllI);
			}
			FillGrid();
		}

		///<summary>Returns true if there are a mixture of benefits with calendar and service year time periods.</summary>
		private bool HasInvalidTimePeriods() {
			//Similar code can be found in FillCalendarYear()
			bool isCalendar=(!textMonth.Enabled);
			if(panelSimple.Visible) {
				for(int i=0;i<benefitListAll.Count;i++) {
					if(benefitListAll[i].TimePeriod==BenefitTimePeriod.CalendarYear && !isCalendar) {
						return true;
					}
					if(benefitListAll[i].TimePeriod==BenefitTimePeriod.ServiceYear && isCalendar) {
						return true;
					}
				}
			}
			else {
				for(int i=0;i<benefitList.Count;i++) {
					if(benefitList[i].TimePeriod==BenefitTimePeriod.CalendarYear && !isCalendar) {
						return true;
					}
					if(benefitList[i].TimePeriod==BenefitTimePeriod.ServiceYear && isCalendar) {
						return true;
					}
				}
			}
			return false;
		}

		private void checkCalendarYear_Click(object sender,EventArgs e) {
			//checkstate will have already changed.
			//right now, change any benefits in the grid.  Upon closing, the ones in simple view will be changed.
			if(checkCalendarYear.CheckState==CheckState.Checked){//change all to calendarYear
				textMonth.Text="";
				textMonth.Enabled=false;
				for(int i=0;i<benefitList.Count;i++){
					if(benefitList[i].TimePeriod==BenefitTimePeriod.ServiceYear){
						benefitList[i].TimePeriod=BenefitTimePeriod.CalendarYear;
					}
				}
			}
			else if(checkCalendarYear.CheckState==CheckState.Unchecked) {//change all to serviceYear
				textMonth.Enabled=true;
				for(int i=0;i<benefitList.Count;i++) {
					if(benefitList[i].TimePeriod==BenefitTimePeriod.CalendarYear) {
						benefitList[i].TimePeriod=BenefitTimePeriod.ServiceYear;
					}
				}
			}
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			Benefit ben=new Benefit();
			ben.PlanNum=PlanNum;
			if(checkCalendarYear.CheckState==CheckState.Checked) {
				ben.TimePeriod=BenefitTimePeriod.CalendarYear;
			}
			if(checkCalendarYear.CheckState==CheckState.Unchecked) {
				ben.TimePeriod=BenefitTimePeriod.ServiceYear;
			}
			if(CovCats.GetCount(true) > 0){
				ben.CovCatNum=CovCats.GetFirst(true).CovCatNum;
			}
			ben.BenefitType=InsBenefitType.CoInsurance;
			FormBenefitEdit FormB=new FormBenefitEdit(PatPlanNum,PlanNum);
			FormB.IsNew=true;
			FormB.BenCur=ben;
			FormB.ShowDialog();
			if(FormB.DialogResult==DialogResult.OK){
				benefitList.Add(FormB.BenCur);
				benefitListAll.Add(FormB.BenCur);
			}
			FillGrid();
		}

		private void butClear_Click(object sender,EventArgs e) {
			if(gridBenefits.SelectedIndices.Length==0) {
				gridBenefits.SetSelected(true);
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete all benefits in list?")) {
					return;
				}
			}
			List<Benefit> listToClear=new List<Benefit>();
			for(int i=0;i<gridBenefits.SelectedIndices.Length;i++) {
				listToClear.Add(benefitList[gridBenefits.SelectedIndices[i]]);
			}
			for(int i=0;i<listToClear.Count;i++) {
				benefitList.Remove(listToClear[i]);
				benefitListAll.Remove(listToClear[i]);
			}
			FillGrid();
		}

		///<summary>Only called if in simple view.  This takes all the data on the form and converts it to benefit items.  A new benefitListAll is created based on a combination of benefitList and the new items from the form.  This is used when clicking OK from simple view, or when switching from simple view to complex view.</summary>
		private bool ConvertFormToBenefits(){
			if(textAnnualMax.errorProvider1.GetError(textAnnualMax) != ""
				|| textDeductible.errorProvider1.GetError(textDeductible) != ""
				|| textAnnualMaxFam.errorProvider1.GetError(textAnnualMaxFam) != ""
				|| textDeductibleFam.errorProvider1.GetError(textDeductibleFam) != ""
				|| textFlo.errorProvider1.GetError(textFlo) != ""
				|| textBW.errorProvider1.GetError(textBW) != ""
				|| textPano.errorProvider1.GetError(textPano) != ""
				|| textExams.errorProvider1.GetError(textExams) != ""
				|| textOrthoAge.errorProvider1.GetError(textOrthoAge) != ""
				|| textOrthoMax.errorProvider1.GetError(textOrthoMax) != ""
				|| textOrthoPercent.errorProvider1.GetError(textOrthoPercent) != ""
				|| textDeductDiag.errorProvider1.GetError(textDeductDiag) != ""
				|| textDeductXray.errorProvider1.GetError(textDeductXray) != ""
				|| textDeductPrevent.errorProvider1.GetError(textDeductPrevent) != ""
				|| textDeductDiagFam.errorProvider1.GetError(textDeductDiagFam) != ""
				|| textDeductXrayFam.errorProvider1.GetError(textDeductXrayFam) != ""
				|| textDeductPreventFam.errorProvider1.GetError(textDeductPreventFam) != ""
				|| textStand1.errorProvider1.GetError(textStand1) != ""
				|| textStand2.errorProvider1.GetError(textStand2) != ""
				|| textStand4.errorProvider1.GetError(textStand4) != ""
				|| textDiagnostic.errorProvider1.GetError(textDiagnostic) != ""
				|| textXray.errorProvider1.GetError(textXray) != ""
				|| textRoutinePrev.errorProvider1.GetError(textRoutinePrev) != ""
				|| textRestorative.errorProvider1.GetError(textRestorative) != ""
				|| textEndo.errorProvider1.GetError(textEndo) != ""
				|| textPerio.errorProvider1.GetError(textPerio) != ""
				|| textOralSurg.errorProvider1.GetError(textOralSurg) != ""
				|| textCrowns.errorProvider1.GetError(textCrowns) != ""
				|| textProsth.errorProvider1.GetError(textProsth) != ""
				|| textMaxProsth.errorProvider1.GetError(textMaxProsth) != ""
				|| textAccident.errorProvider1.GetError(textAccident) != ""
				|| textMonth.errorProvider1.GetError(textMonth) != ""
				)
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return false;
			}
			if(!checkCalendarYear.Checked && textMonth.Text=="") {
				MsgBox.Show(this,"Please enter a starting month for the benefit year.");
				return false;
			}
			/*bool hasIndivid=false;
			if(textAnnualMax.Text != "" || textDeductible.Text != "" || textDeductPrev.Text != "") {
				hasIndivid=true;
			}
			bool hasFam=false;
			if(textAnnualMaxFam.Text != "" || textDeductibleFam.Text != "" || textDeductPrevFam.Text != "") {
				hasFam=true;
			}
			if(hasIndivid && hasFam) {
				MsgBox.Show(this,"You can enter either Individual or Family benefits, but not both.");
				return false;
			}*/
			benefitListAll=new List<Benefit>(benefitList);
			Benefit ben;
			//annual max individual
			if(textAnnualMax.Text !=""){
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.Limitations;
				ben.CovCatNum=0;
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked){
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else{
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;					
				}
				ben.MonetaryAmt=PIn.Double(textAnnualMax.Text);
				ben.CoverageLevel=BenefitCoverageLevel.Individual;
				benefitListAll.Add(ben);
			}
			//annual max family
			if(textAnnualMaxFam.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.Limitations;
				ben.CovCatNum=0;
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				ben.MonetaryAmt=PIn.Double(textAnnualMaxFam.Text);
				ben.CoverageLevel=BenefitCoverageLevel.Family;
				benefitListAll.Add(ben);
			}
			//deductible individual
			if(textDeductible.Text !=""){
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.Deductible;
				ben.CovCatNum=0;
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else{
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;					
				}
				ben.MonetaryAmt=PIn.Double(textDeductible.Text);
				ben.CoverageLevel=BenefitCoverageLevel.Individual;
				benefitListAll.Add(ben);
			}
			//deductible family
			if(textDeductibleFam.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.Deductible;
				ben.CovCatNum=0;
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				ben.MonetaryAmt=PIn.Double(textDeductibleFam.Text);
				ben.CoverageLevel=BenefitCoverageLevel.Family;
				benefitListAll.Add(ben);
			}
			//Flo
			if(textFlo.Text !=""){
				ben=new Benefit();
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					if(Canadian.IsQuebec()) {//Quebec
						ben.CodeNum=ProcedureCodes.GetCodeNum("12400");
					}
					else {//The rest of Canada use the same procedure codes.
						ben.CodeNum=ProcedureCodes.GetCodeNum("12101");
					}
				}
				else{//USA
					ben.CodeNum=ProcedureCodes.GetCodeNum("D1208");
					Benefit benFlo=new Benefit();//Insert a benefit for D1206 for offices that use D1206 instead of D1208.
					benFlo.CodeNum=ProcedureCodes.GetCodeNum("D1206");
					benFlo.BenefitType=InsBenefitType.Limitations;
					benFlo.CovCatNum=0;
					benFlo.PlanNum=PlanNum;
					benFlo.QuantityQualifier=BenefitQuantity.AgeLimit;
					benFlo.Quantity=PIn.Byte(textFlo.Text);
					benefitListAll.Add(benFlo);
				}
				ben.BenefitType=InsBenefitType.Limitations;
				ben.CovCatNum=0;
				ben.PlanNum=PlanNum;
				ben.QuantityQualifier=BenefitQuantity.AgeLimit;
				ben.Quantity=PIn.Byte(textFlo.Text);
				benefitListAll.Add(ben);
			}
			//Frequency BW group
			if(textBW.Text !="") {
				ben=new Benefit();
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					ben.CodeNum=ProcedureCodes.GetCodeNum("02144");//4BW The same code for Quebec as well as the rest of Canada.
				}
				else {//USA
					ben.CodeNum=ProcedureCodes.GetCodeNum("D0274");//4BW
				}
				ben.BenefitType=InsBenefitType.Limitations;
				ben.CovCatNum=0;
				ben.PlanNum=PlanNum;
				if(comboBW.SelectedIndex==0){
					ben.QuantityQualifier=BenefitQuantity.Years;
				}
				else if(comboBW.SelectedIndex==1){
					ben.QuantityQualifier=BenefitQuantity.NumberOfServices;
					if(checkCalendarYear.Checked) {
						ben.TimePeriod=BenefitTimePeriod.CalendarYear;
					}
					else {
						ben.TimePeriod=BenefitTimePeriod.ServiceYear;
					}
				}
				else if(comboBW.SelectedIndex==2){
					ben.QuantityQualifier=BenefitQuantity.Months;
				}
				ben.Quantity=PIn.Byte(textBW.Text);
				//ben.TimePeriod is none for years or months, although calYear, or ServiceYear, or Years might work too
				benefitListAll.Add(ben);
			}
			//Frequency pano/FMX group
			if(textPano.Text !="") {
				ben=new Benefit();
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					if(Canadian.IsQuebec()) {
						ben.CodeNum=ProcedureCodes.GetCodeNum("02600");
					}
					else {//The rest of Canada use the same procedure codes.
						ben.CodeNum=ProcedureCodes.GetCodeNum("02601");
					}
				}
				else {//USA
					ben.CodeNum=ProcedureCodes.GetCodeNum("D0330");//Pano
				}
			
				ben.BenefitType=InsBenefitType.Limitations;
				ben.CovCatNum=0;
				ben.PlanNum=PlanNum;
				if(comboPano.SelectedIndex==0) {
					ben.QuantityQualifier=BenefitQuantity.Years;
				}
				else if(comboPano.SelectedIndex==1) {
					ben.QuantityQualifier=BenefitQuantity.NumberOfServices;
					if(checkCalendarYear.Checked) {
						ben.TimePeriod=BenefitTimePeriod.CalendarYear;
					}
					else {
						ben.TimePeriod=BenefitTimePeriod.ServiceYear;
					}
				}
				else if(comboPano.SelectedIndex==2) {
					ben.QuantityQualifier=BenefitQuantity.Months;
				}
				ben.Quantity=PIn.Byte(textPano.Text);
				//ben.TimePeriod is none for years or months, although calYear, or ServiceYear, or Years might work too
				benefitListAll.Add(ben);
			}
			//Frequency in Exams group
			if(textExams.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.Limitations;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive).CovCatNum;
				ben.PlanNum=PlanNum;
				if(comboExams.SelectedIndex==0) {
					ben.QuantityQualifier=BenefitQuantity.Years;
				}
				else if(comboExams.SelectedIndex==1) {
					ben.QuantityQualifier=BenefitQuantity.NumberOfServices;
					if(checkCalendarYear.Checked) {
						ben.TimePeriod=BenefitTimePeriod.CalendarYear;
					}
					else {
						ben.TimePeriod=BenefitTimePeriod.ServiceYear;
					}
				}
				else if(comboExams.SelectedIndex==2) {
					ben.QuantityQualifier=BenefitQuantity.Months;
				}
				ben.Quantity=PIn.Byte(textExams.Text);
				//ben.TimePeriod is none for years or months, although calYear, or ServiceYear, or Years might work too
				benefitListAll.Add(ben);
			}
			//ortho age
			if(textOrthoAge.Text!="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.Limitations;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Orthodontics).CovCatNum;
				ben.PlanNum=PlanNum;
				ben.QuantityQualifier=BenefitQuantity.AgeLimit;
				ben.Quantity=PIn.Byte(textOrthoAge.Text);
				benefitListAll.Add(ben);
			}
			//ortho max
			if(textOrthoMax.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.Limitations;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Orthodontics).CovCatNum;
				ben.PlanNum=PlanNum;
				ben.TimePeriod=BenefitTimePeriod.Lifetime;
				ben.MonetaryAmt=PIn.Double(textOrthoMax.Text);
				benefitListAll.Add(ben);
			}
			//ortho percent
			if(textOrthoPercent.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.CoInsurance;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Orthodontics).CovCatNum;
				ben.Percent=PIn.Int(textOrthoPercent.Text);
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				benefitListAll.Add(ben);
			}
			//deductible diagnostic individual
			if(textDeductDiag.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.Deductible;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Diagnostic).CovCatNum;
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				ben.MonetaryAmt=PIn.Double(textDeductDiag.Text);
				ben.CoverageLevel=BenefitCoverageLevel.Individual;
				benefitListAll.Add(ben);
			}
			//deductible diagnostic family
			if(textDeductDiagFam.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.Deductible;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Diagnostic).CovCatNum;
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				ben.MonetaryAmt=PIn.Double(textDeductDiagFam.Text);
				ben.CoverageLevel=BenefitCoverageLevel.Family;
				benefitListAll.Add(ben);
			}
			//deductible xray individual
			if(textDeductXray.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.Deductible;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.DiagnosticXRay).CovCatNum;
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				ben.MonetaryAmt=PIn.Double(textDeductXray.Text);
				ben.CoverageLevel=BenefitCoverageLevel.Individual;
				benefitListAll.Add(ben);
			}
			//deductible xray family
			if(textDeductXrayFam.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.Deductible;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.DiagnosticXRay).CovCatNum;
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				ben.MonetaryAmt=PIn.Double(textDeductXrayFam.Text);
				ben.CoverageLevel=BenefitCoverageLevel.Family;
				benefitListAll.Add(ben);
			}
			//deductible preventive individual
			if(textDeductPrevent.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.Deductible;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive).CovCatNum;
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				ben.MonetaryAmt=PIn.Double(textDeductPrevent.Text);
				ben.CoverageLevel=BenefitCoverageLevel.Individual;
				benefitListAll.Add(ben);
			}
			//deductible preventive family
			if(textDeductPreventFam.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.Deductible;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive).CovCatNum;
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				ben.MonetaryAmt=PIn.Double(textDeductPreventFam.Text);
				ben.CoverageLevel=BenefitCoverageLevel.Family;
				benefitListAll.Add(ben);
			}
			//Diagnostic
			if(textDiagnostic.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.CoInsurance;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Diagnostic).CovCatNum;
				ben.Percent=PIn.Int(textDiagnostic.Text);
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				benefitListAll.Add(ben);
			}
			//X-Ray
			if(textXray.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.CoInsurance;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.DiagnosticXRay).CovCatNum;
				ben.Percent=PIn.Int(textXray.Text);
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				benefitListAll.Add(ben);
			}
			//RoutinePreventive
			if(textRoutinePrev.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.CoInsurance;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive).CovCatNum;
				ben.Percent=PIn.Int(textRoutinePrev.Text);
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				benefitListAll.Add(ben);
			}
			//Restorative
			if(textRestorative.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.CoInsurance;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Restorative).CovCatNum;
				ben.Percent=PIn.Int(textRestorative.Text);
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				benefitListAll.Add(ben);
			}
			//Endo
			if(textEndo.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.CoInsurance;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Endodontics).CovCatNum;
				ben.Percent=PIn.Int(textEndo.Text);
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				benefitListAll.Add(ben);
			}
			//Perio
			if(textPerio.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.CoInsurance;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Periodontics).CovCatNum;
				ben.Percent=PIn.Int(textPerio.Text);
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				benefitListAll.Add(ben);
			}
			//OralSurg
			if(textOralSurg.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.CoInsurance;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.OralSurgery).CovCatNum;
				ben.Percent=PIn.Int(textOralSurg.Text);
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				benefitListAll.Add(ben);
			}
			//Crowns
			if(textCrowns.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.CoInsurance;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Crowns).CovCatNum;
				ben.Percent=PIn.Int(textCrowns.Text);
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				benefitListAll.Add(ben);
			}
			//Prosth
			if(textProsth.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.CoInsurance;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Prosthodontics).CovCatNum;
				ben.Percent=PIn.Int(textProsth.Text);
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				benefitListAll.Add(ben);
			}
			//MaxProsth
			if(textMaxProsth.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.CoInsurance;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.MaxillofacialProsth).CovCatNum;
				ben.Percent=PIn.Int(textMaxProsth.Text);
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				benefitListAll.Add(ben);
			}
			//Accident
			if(textAccident.Text !="") {
				ben=new Benefit();
				ben.CodeNum=0;
				ben.BenefitType=InsBenefitType.CoInsurance;
				ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Accident).CovCatNum;
				ben.Percent=PIn.Int(textAccident.Text);
				ben.PlanNum=PlanNum;
				if(checkCalendarYear.Checked) {
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
				}
				else {
					ben.TimePeriod=BenefitTimePeriod.ServiceYear;
				}
				benefitListAll.Add(ben);
			}
			return true;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(!checkCalendarYear.Checked && textMonth.Text=="") {
				MsgBox.Show(this,"Please enter a starting month for the benefit year.");
				return;
			}
			if(panelSimple.Visible) {
				if(!ConvertFormToBenefits()) {
					return;
				}
				if(HasInvalidTimePeriods()) {
					MsgBox.Show(this,"A mixture of calendar and service year time periods are not allowed.");
					return;
				}
				//OriginalBenList.Clear();
				//for(int i=0;i<benefitListAll.Count;i++){
				//	OriginalBenList.Add(benefitListAll[i]);
				//}
				//We can't just clear the list.  Then, we wouldn't be able to test it for most efficient db queries.
				for(int i=OriginalBenList.Count-1;i>=0;i--) {//loop through the old list, backwards.
					bool matchFound=false;
					for(int j=0;j<benefitListAll.Count;j++) {
						if(benefitListAll[j].IsSimilar(OriginalBenList[i])) {
							matchFound=true;
						}
					}
					if(!matchFound) {//If no match is found in the new list
						//delete the entry from the old list.  That will cause a deletion from the db later.
						OriginalBenList.RemoveAt(i);
					}
				}
				for(int j=0;j<benefitListAll.Count;j++) {//loop through the new list.
					bool matchFound=false;
					for(int i=0;i<OriginalBenList.Count;i++) {
						if(benefitListAll[j].IsSimilar(OriginalBenList[i])) {
							matchFound=true;
						}
					}
					if(!matchFound) {//If no match is found in the old list
						//add the entry to the old list.  This will cause an insert because BenefitNum will be 0.
						OriginalBenList.Add(benefitListAll[j]);
					}
				}
			}
			else {//not simple view.  Will optimize this later for speed.  Should be easier.
				if(HasInvalidTimePeriods()) {
					MsgBox.Show(this,"A mixture of calendar and service year time periods are not allowed.");
					return;
				}
				OriginalBenList.Clear();
				for(int i=0;i<benefitList.Count;i++) {
					OriginalBenList.Add(benefitList[i]);
				}
			}
			if(checkCalendarYear.Checked) {
				MonthRenew=0;
			}
			else {
				MonthRenew=PIn.Byte(textMonth.Text);
			}
			Note=textSubscNote.Text;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		

		

		

		


	}
}





















