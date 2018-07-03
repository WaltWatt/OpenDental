using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using SparksToothChart;
using System.Linq;
using OpenDental.UI.Voice;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormPerio : ODForm {
		private OpenDental.UI.Button but7;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioLeft;
		private System.Windows.Forms.RadioButton radioRight;
		private OpenDental.UI.Button but3;
		private OpenDental.UI.Button but2;
		private OpenDental.UI.Button but1;
		private OpenDental.UI.Button but6;
		private OpenDental.UI.Button but9;
		private OpenDental.UI.Button but5;
		private OpenDental.UI.Button but4;
		private OpenDental.UI.Button but8;
		private OpenDental.UI.Button butDelete;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button butColorBleed;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button butColorPus;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button butClose;
		private OpenDental.UI.Button butAdd;
		private System.Windows.Forms.Label label6;
		private OpenDental.UI.Button but0;
		private OpenDental.UI.Button but10;
		private OpenDental.UI.Button butBleed;
		private OpenDental.UI.Button butPus;
		private System.Windows.Forms.CheckBox checkThree;
		private bool localDefsChanged;
		private System.Windows.Forms.ListBox listExams;
		private OpenDental.UI.Button butSkip;
		private OpenDental.UI.Button butPrint;
		private System.Windows.Forms.Button butColorCalculus;
		private System.Windows.Forms.Button butColorPlaque;
		private OpenDental.UI.Button butCalculus;
		private OpenDental.UI.Button butPlaque;
		private System.Windows.Forms.TextBox textIndexPlaque;
		private System.Windows.Forms.TextBox textIndexSupp;
		private System.Windows.Forms.TextBox textIndexBleeding;
		private System.Windows.Forms.TextBox textIndexCalculus;
		private OpenDental.UI.Button butCalcIndex;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.TextBox textRedProb;
		private OpenDental.UI.Button butCount;
		private System.Windows.Forms.DomainUpDown updownProb;
		private System.Windows.Forms.TextBox textCountProb;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox textCountMGJ;
		private System.Windows.Forms.DomainUpDown updownMGJ;
		private System.Windows.Forms.TextBox textRedMGJ;
		private System.Windows.Forms.TextBox textCountGing;
		private System.Windows.Forms.DomainUpDown updownGing;
		private System.Windows.Forms.TextBox textRedGing;
		private System.Windows.Forms.TextBox textCountCAL;
		private System.Windows.Forms.DomainUpDown updownCAL;
		private System.Windows.Forms.TextBox textRedCAL;
		private System.Windows.Forms.TextBox textCountFurc;
		private System.Windows.Forms.DomainUpDown updownFurc;
		private System.Windows.Forms.TextBox textRedFurc;
		private System.Windows.Forms.TextBox textCountMob;
		private System.Windows.Forms.DomainUpDown updownMob;
		private System.Windows.Forms.TextBox textRedMob;
		//private OpenDental.ContrPerio gridP;
		//private OpenDental.ContrPerio contrPerio1;
		private OpenDental.ContrPerio gridP;
		private System.Drawing.Printing.PrintDocument pd2;
		private System.Windows.Forms.PrintDialog printDialog2;
		private bool TenIsDown;
		private System.Windows.Forms.PrintPreviewDialog printPreviewDlg;
		//private int pagesPrinted;
		///<summary>Gets a list of missing teeth as strings on load. Includes "1"-"32", and "A"-"Z".</summary>
		private List<string> _listMissingTeeth;
		private Patient PatCur;
		///<summary>This is not a list of valid procedures.  The only values to be trusted in this list are the ToothNum and CodeNum.</summary>
		private List<Procedure> _listPatProcs;
		private OpenDental.UI.Button butGraphical;
		private OpenDental.UI.Button butSave;
		private Label labelPlaqueHistory;
		private ListBox listPlaqueHistory;
		private CheckBox checkGingMarg;
		/// <summary>This control is located behind gridP in the upper left corner.  It is used to allow text voice activated perio charting.  This also allows text to be pasted in the perio chart.</summary>
		private TextBox textInputBox;
		private UI.Button butCopyPrevious;
		private PerioExam PerioExamCur;
		private CheckBox checkShowCurrent;
		private UserOdPref _userPrefCurrentOnly;

		///<summary>Used to pass ToothGraphic info to FormPerioGraphical.</summary>
		private ToothChartData _toothChartData;
		private UI.Button butListen;
		private Label labelListening;
		private VoiceController _voiceController;
		private PerioCell _prevLocation;
		private PerioCell _curLocation;
		private List<Def> _listMiscColorDefs;

		///<summary></summary>
		public FormPerio(Patient patCur,List<Procedure> listPatProcs,ToothChartData toothChartData)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			PatCur=patCur;
			_listPatProcs=listPatProcs;
			_toothChartData=toothChartData;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPerio));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioRight = new System.Windows.Forms.RadioButton();
			this.radioLeft = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.butColorBleed = new System.Windows.Forms.Button();
			this.butColorPus = new System.Windows.Forms.Button();
			this.butColorCalculus = new System.Windows.Forms.Button();
			this.butColorPlaque = new System.Windows.Forms.Button();
			this.checkThree = new System.Windows.Forms.CheckBox();
			this.checkGingMarg = new System.Windows.Forms.CheckBox();
			this.butCalcIndex = new OpenDental.UI.Button();
			this.butCalculus = new OpenDental.UI.Button();
			this.butPlaque = new OpenDental.UI.Button();
			this.butSkip = new OpenDental.UI.Button();
			this.butCount = new OpenDental.UI.Button();
			this.butPus = new OpenDental.UI.Button();
			this.butBleed = new OpenDental.UI.Button();
			this.but10 = new OpenDental.UI.Button();
			this.checkShowCurrent = new System.Windows.Forms.CheckBox();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.textCountMob = new System.Windows.Forms.TextBox();
			this.updownMob = new System.Windows.Forms.DomainUpDown();
			this.textRedMob = new System.Windows.Forms.TextBox();
			this.textCountFurc = new System.Windows.Forms.TextBox();
			this.updownFurc = new System.Windows.Forms.DomainUpDown();
			this.textRedFurc = new System.Windows.Forms.TextBox();
			this.textCountCAL = new System.Windows.Forms.TextBox();
			this.updownCAL = new System.Windows.Forms.DomainUpDown();
			this.textRedCAL = new System.Windows.Forms.TextBox();
			this.textCountGing = new System.Windows.Forms.TextBox();
			this.updownGing = new System.Windows.Forms.DomainUpDown();
			this.textRedGing = new System.Windows.Forms.TextBox();
			this.textCountMGJ = new System.Windows.Forms.TextBox();
			this.updownMGJ = new System.Windows.Forms.DomainUpDown();
			this.textRedMGJ = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.textCountProb = new System.Windows.Forms.TextBox();
			this.updownProb = new System.Windows.Forms.DomainUpDown();
			this.label13 = new System.Windows.Forms.Label();
			this.textRedProb = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.listExams = new System.Windows.Forms.ListBox();
			this.textIndexPlaque = new System.Windows.Forms.TextBox();
			this.textIndexSupp = new System.Windows.Forms.TextBox();
			this.textIndexBleeding = new System.Windows.Forms.TextBox();
			this.textIndexCalculus = new System.Windows.Forms.TextBox();
			this.pd2 = new System.Drawing.Printing.PrintDocument();
			this.printDialog2 = new System.Windows.Forms.PrintDialog();
			this.printPreviewDlg = new System.Windows.Forms.PrintPreviewDialog();
			this.labelPlaqueHistory = new System.Windows.Forms.Label();
			this.listPlaqueHistory = new System.Windows.Forms.ListBox();
			this.textInputBox = new System.Windows.Forms.TextBox();
			this.butCopyPrevious = new OpenDental.UI.Button();
			this.gridP = new OpenDental.ContrPerio();
			this.butSave = new OpenDental.UI.Button();
			this.butGraphical = new OpenDental.UI.Button();
			this.butPrint = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.but0 = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.but8 = new OpenDental.UI.Button();
			this.but4 = new OpenDental.UI.Button();
			this.but5 = new OpenDental.UI.Button();
			this.but9 = new OpenDental.UI.Button();
			this.but6 = new OpenDental.UI.Button();
			this.but1 = new OpenDental.UI.Button();
			this.but2 = new OpenDental.UI.Button();
			this.but3 = new OpenDental.UI.Button();
			this.but7 = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.butListen = new OpenDental.UI.Button();
			this.labelListening = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioRight);
			this.groupBox1.Controls.Add(this.radioLeft);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(765, 4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(185, 43);
			this.groupBox1.TabIndex = 13;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Auto Advance";
			// 
			// radioRight
			// 
			this.radioRight.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioRight.Location = new System.Drawing.Point(10, 20);
			this.radioRight.Name = "radioRight";
			this.radioRight.Size = new System.Drawing.Size(75, 18);
			this.radioRight.TabIndex = 1;
			this.radioRight.Text = "Right";
			this.radioRight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioRight.Click += new System.EventHandler(this.radioRight_Click);
			// 
			// radioLeft
			// 
			this.radioLeft.Checked = true;
			this.radioLeft.Location = new System.Drawing.Point(98, 20);
			this.radioLeft.Name = "radioLeft";
			this.radioLeft.Size = new System.Drawing.Size(75, 18);
			this.radioLeft.TabIndex = 0;
			this.radioLeft.TabStop = true;
			this.radioLeft.Text = "Left";
			this.radioLeft.Click += new System.EventHandler(this.radioLeft_Click);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(136, 102);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(18, 23);
			this.label2.TabIndex = 35;
			this.label2.Text = "F";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(136, 562);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(18, 23);
			this.label3.TabIndex = 36;
			this.label3.Text = "F";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(136, 428);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(18, 23);
			this.label4.TabIndex = 37;
			this.label4.Text = "L";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(136, 222);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(18, 23);
			this.label5.TabIndex = 38;
			this.label5.Text = "L";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butColorBleed
			// 
			this.butColorBleed.BackColor = System.Drawing.Color.Red;
			this.butColorBleed.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColorBleed.Location = new System.Drawing.Point(850, 302);
			this.butColorBleed.Name = "butColorBleed";
			this.butColorBleed.Size = new System.Drawing.Size(12, 24);
			this.butColorBleed.TabIndex = 43;
			this.toolTip1.SetToolTip(this.butColorBleed, "Edit Color");
			this.butColorBleed.UseVisualStyleBackColor = false;
			this.butColorBleed.Click += new System.EventHandler(this.butColorBleed_Click);
			// 
			// butColorPus
			// 
			this.butColorPus.BackColor = System.Drawing.Color.Gold;
			this.butColorPus.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColorPus.Location = new System.Drawing.Point(850, 332);
			this.butColorPus.Name = "butColorPus";
			this.butColorPus.Size = new System.Drawing.Size(12, 24);
			this.butColorPus.TabIndex = 50;
			this.toolTip1.SetToolTip(this.butColorPus, "Edit Color");
			this.butColorPus.UseVisualStyleBackColor = false;
			this.butColorPus.Click += new System.EventHandler(this.butColorPus_Click);
			// 
			// butColorCalculus
			// 
			this.butColorCalculus.BackColor = System.Drawing.Color.Green;
			this.butColorCalculus.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColorCalculus.Location = new System.Drawing.Point(850, 272);
			this.butColorCalculus.Name = "butColorCalculus";
			this.butColorCalculus.Size = new System.Drawing.Size(12, 24);
			this.butColorCalculus.TabIndex = 67;
			this.toolTip1.SetToolTip(this.butColorCalculus, "Edit Color");
			this.butColorCalculus.UseVisualStyleBackColor = false;
			this.butColorCalculus.Click += new System.EventHandler(this.butColorCalculus_Click);
			// 
			// butColorPlaque
			// 
			this.butColorPlaque.BackColor = System.Drawing.Color.RoyalBlue;
			this.butColorPlaque.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColorPlaque.Location = new System.Drawing.Point(850, 242);
			this.butColorPlaque.Name = "butColorPlaque";
			this.butColorPlaque.Size = new System.Drawing.Size(12, 24);
			this.butColorPlaque.TabIndex = 66;
			this.toolTip1.SetToolTip(this.butColorPlaque, "Edit Color");
			this.butColorPlaque.UseVisualStyleBackColor = false;
			this.butColorPlaque.Click += new System.EventHandler(this.butColorPlaque_Click);
			// 
			// checkThree
			// 
			this.checkThree.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkThree.Location = new System.Drawing.Point(765, 49);
			this.checkThree.Name = "checkThree";
			this.checkThree.Size = new System.Drawing.Size(100, 19);
			this.checkThree.TabIndex = 57;
			this.checkThree.Text = "Triplets";
			this.toolTip1.SetToolTip(this.checkThree, "Enter numbers three at a time");
			this.checkThree.Click += new System.EventHandler(this.checkThree_Click);
			// 
			// checkGingMarg
			// 
			this.checkGingMarg.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkGingMarg.Location = new System.Drawing.Point(871, 49);
			this.checkGingMarg.Name = "checkGingMarg";
			this.checkGingMarg.Size = new System.Drawing.Size(99, 19);
			this.checkGingMarg.TabIndex = 80;
			this.checkGingMarg.Text = "Ging Marg +";
			this.toolTip1.SetToolTip(this.checkGingMarg, "Or hold down the Ctrl key while you type numbers.  Affects gingival margins only." +
        "  Used to input positive gingival margins (hyperplasia).");
			this.checkGingMarg.Click += new System.EventHandler(this.checkGingMarg_CheckedChanged);
			// 
			// butCalcIndex
			// 
			this.butCalcIndex.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCalcIndex.Autosize = true;
			this.butCalcIndex.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCalcIndex.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCalcIndex.CornerRadius = 4F;
			this.butCalcIndex.Location = new System.Drawing.Point(863, 215);
			this.butCalcIndex.Name = "butCalcIndex";
			this.butCalcIndex.Size = new System.Drawing.Size(84, 24);
			this.butCalcIndex.TabIndex = 74;
			this.butCalcIndex.Text = "Calc Index %";
			this.toolTip1.SetToolTip(this.butCalcIndex, "Calculate the Index for all four types");
			this.butCalcIndex.Click += new System.EventHandler(this.butCalcIndex_Click);
			// 
			// butCalculus
			// 
			this.butCalculus.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCalculus.Autosize = true;
			this.butCalculus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCalculus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCalculus.CornerRadius = 4F;
			this.butCalculus.Location = new System.Drawing.Point(762, 272);
			this.butCalculus.Name = "butCalculus";
			this.butCalculus.Size = new System.Drawing.Size(88, 24);
			this.butCalculus.TabIndex = 16;
			this.butCalculus.Text = "Calculus";
			this.toolTip1.SetToolTip(this.butCalculus, "C on your keyboard");
			this.butCalculus.Click += new System.EventHandler(this.butCalculus_Click);
			// 
			// butPlaque
			// 
			this.butPlaque.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPlaque.Autosize = true;
			this.butPlaque.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPlaque.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPlaque.CornerRadius = 4F;
			this.butPlaque.Location = new System.Drawing.Point(762, 242);
			this.butPlaque.Name = "butPlaque";
			this.butPlaque.Size = new System.Drawing.Size(88, 24);
			this.butPlaque.TabIndex = 15;
			this.butPlaque.Text = "Plaque";
			this.toolTip1.SetToolTip(this.butPlaque, "P on your keyboard");
			this.butPlaque.Click += new System.EventHandler(this.butPlaque_Click);
			// 
			// butSkip
			// 
			this.butSkip.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSkip.Autosize = true;
			this.butSkip.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSkip.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSkip.CornerRadius = 4F;
			this.butSkip.Location = new System.Drawing.Point(764, 580);
			this.butSkip.Name = "butSkip";
			this.butSkip.Size = new System.Drawing.Size(88, 24);
			this.butSkip.TabIndex = 19;
			this.butSkip.Text = "SkipTeeth";
			this.toolTip1.SetToolTip(this.butSkip, "Toggle the selected teeth as skipped");
			this.butSkip.Click += new System.EventHandler(this.butSkip_Click);
			// 
			// butCount
			// 
			this.butCount.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCount.Autosize = true;
			this.butCount.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCount.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCount.CornerRadius = 4F;
			this.butCount.Location = new System.Drawing.Point(92, 18);
			this.butCount.Name = "butCount";
			this.butCount.Size = new System.Drawing.Size(84, 24);
			this.butCount.TabIndex = 6;
			this.butCount.Text = "Count Teeth";
			this.toolTip1.SetToolTip(this.butCount, "Count all six types");
			this.butCount.Click += new System.EventHandler(this.butCount_Click);
			// 
			// butPus
			// 
			this.butPus.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPus.Autosize = true;
			this.butPus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPus.CornerRadius = 4F;
			this.butPus.Location = new System.Drawing.Point(762, 332);
			this.butPus.Name = "butPus";
			this.butPus.Size = new System.Drawing.Size(88, 24);
			this.butPus.TabIndex = 18;
			this.butPus.Text = "Suppuration";
			this.toolTip1.SetToolTip(this.butPus, "S on your keyboard");
			this.butPus.Click += new System.EventHandler(this.butPus_Click);
			// 
			// butBleed
			// 
			this.butBleed.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBleed.Autosize = true;
			this.butBleed.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBleed.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBleed.CornerRadius = 4F;
			this.butBleed.Location = new System.Drawing.Point(762, 302);
			this.butBleed.Name = "butBleed";
			this.butBleed.Size = new System.Drawing.Size(88, 24);
			this.butBleed.TabIndex = 17;
			this.butBleed.Text = "Bleeding";
			this.toolTip1.SetToolTip(this.butBleed, "Space bar or B on your keyboard");
			this.butBleed.Click += new System.EventHandler(this.butBleed_Click);
			// 
			// but10
			// 
			this.but10.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.but10.Autosize = true;
			this.but10.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.but10.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.but10.CornerRadius = 4F;
			this.but10.Location = new System.Drawing.Point(833, 173);
			this.but10.Name = "but10";
			this.but10.Size = new System.Drawing.Size(32, 32);
			this.but10.TabIndex = 14;
			this.but10.Text = "10";
			this.toolTip1.SetToolTip(this.but10, "Or hold down the Ctrl key");
			this.but10.Click += new System.EventHandler(this.but10_Click);
			// 
			// checkShowCurrent
			// 
			this.checkShowCurrent.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowCurrent.Location = new System.Drawing.Point(7, 172);
			this.checkShowCurrent.Name = "checkShowCurrent";
			this.checkShowCurrent.Size = new System.Drawing.Size(150, 19);
			this.checkShowCurrent.TabIndex = 83;
			this.checkShowCurrent.Text = "Show current exam only";
			this.toolTip1.SetToolTip(this.checkShowCurrent, "Only show measurements for the currently selected exam");
			this.checkShowCurrent.Click += new System.EventHandler(this.checkShowCurrent_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.textCountMob);
			this.groupBox2.Controls.Add(this.updownMob);
			this.groupBox2.Controls.Add(this.textRedMob);
			this.groupBox2.Controls.Add(this.textCountFurc);
			this.groupBox2.Controls.Add(this.updownFurc);
			this.groupBox2.Controls.Add(this.textRedFurc);
			this.groupBox2.Controls.Add(this.textCountCAL);
			this.groupBox2.Controls.Add(this.updownCAL);
			this.groupBox2.Controls.Add(this.textRedCAL);
			this.groupBox2.Controls.Add(this.textCountGing);
			this.groupBox2.Controls.Add(this.updownGing);
			this.groupBox2.Controls.Add(this.textRedGing);
			this.groupBox2.Controls.Add(this.textCountMGJ);
			this.groupBox2.Controls.Add(this.updownMGJ);
			this.groupBox2.Controls.Add(this.textRedMGJ);
			this.groupBox2.Controls.Add(this.label14);
			this.groupBox2.Controls.Add(this.textCountProb);
			this.groupBox2.Controls.Add(this.updownProb);
			this.groupBox2.Controls.Add(this.label13);
			this.groupBox2.Controls.Add(this.textRedProb);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.butCount);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(764, 371);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(196, 201);
			this.groupBox2.TabIndex = 49;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Numbers in red";
			// 
			// textCountMob
			// 
			this.textCountMob.Location = new System.Drawing.Point(141, 170);
			this.textCountMob.Name = "textCountMob";
			this.textCountMob.ReadOnly = true;
			this.textCountMob.Size = new System.Drawing.Size(34, 20);
			this.textCountMob.TabIndex = 26;
			// 
			// updownMob
			// 
			this.updownMob.InterceptArrowKeys = false;
			this.updownMob.Location = new System.Drawing.Point(97, 170);
			this.updownMob.Name = "updownMob";
			this.updownMob.Size = new System.Drawing.Size(19, 20);
			this.updownMob.TabIndex = 25;
			this.updownMob.MouseDown += new System.Windows.Forms.MouseEventHandler(this.updownRed_MouseDown);
			// 
			// textRedMob
			// 
			this.textRedMob.Location = new System.Drawing.Point(70, 170);
			this.textRedMob.Name = "textRedMob";
			this.textRedMob.ReadOnly = true;
			this.textRedMob.Size = new System.Drawing.Size(27, 20);
			this.textRedMob.TabIndex = 24;
			// 
			// textCountFurc
			// 
			this.textCountFurc.Location = new System.Drawing.Point(141, 150);
			this.textCountFurc.Name = "textCountFurc";
			this.textCountFurc.ReadOnly = true;
			this.textCountFurc.Size = new System.Drawing.Size(34, 20);
			this.textCountFurc.TabIndex = 23;
			// 
			// updownFurc
			// 
			this.updownFurc.InterceptArrowKeys = false;
			this.updownFurc.Location = new System.Drawing.Point(97, 150);
			this.updownFurc.Name = "updownFurc";
			this.updownFurc.Size = new System.Drawing.Size(19, 20);
			this.updownFurc.TabIndex = 22;
			this.updownFurc.MouseDown += new System.Windows.Forms.MouseEventHandler(this.updownRed_MouseDown);
			// 
			// textRedFurc
			// 
			this.textRedFurc.Location = new System.Drawing.Point(70, 150);
			this.textRedFurc.Name = "textRedFurc";
			this.textRedFurc.ReadOnly = true;
			this.textRedFurc.Size = new System.Drawing.Size(27, 20);
			this.textRedFurc.TabIndex = 21;
			// 
			// textCountCAL
			// 
			this.textCountCAL.Location = new System.Drawing.Point(141, 130);
			this.textCountCAL.Name = "textCountCAL";
			this.textCountCAL.ReadOnly = true;
			this.textCountCAL.Size = new System.Drawing.Size(34, 20);
			this.textCountCAL.TabIndex = 20;
			// 
			// updownCAL
			// 
			this.updownCAL.InterceptArrowKeys = false;
			this.updownCAL.Location = new System.Drawing.Point(97, 130);
			this.updownCAL.Name = "updownCAL";
			this.updownCAL.Size = new System.Drawing.Size(19, 20);
			this.updownCAL.TabIndex = 19;
			this.updownCAL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.updownRed_MouseDown);
			// 
			// textRedCAL
			// 
			this.textRedCAL.Location = new System.Drawing.Point(70, 130);
			this.textRedCAL.Name = "textRedCAL";
			this.textRedCAL.ReadOnly = true;
			this.textRedCAL.Size = new System.Drawing.Size(27, 20);
			this.textRedCAL.TabIndex = 18;
			// 
			// textCountGing
			// 
			this.textCountGing.Location = new System.Drawing.Point(141, 110);
			this.textCountGing.Name = "textCountGing";
			this.textCountGing.ReadOnly = true;
			this.textCountGing.Size = new System.Drawing.Size(34, 20);
			this.textCountGing.TabIndex = 17;
			// 
			// updownGing
			// 
			this.updownGing.InterceptArrowKeys = false;
			this.updownGing.Location = new System.Drawing.Point(97, 110);
			this.updownGing.Name = "updownGing";
			this.updownGing.Size = new System.Drawing.Size(19, 20);
			this.updownGing.TabIndex = 16;
			this.updownGing.MouseDown += new System.Windows.Forms.MouseEventHandler(this.updownRed_MouseDown);
			// 
			// textRedGing
			// 
			this.textRedGing.Location = new System.Drawing.Point(70, 110);
			this.textRedGing.Name = "textRedGing";
			this.textRedGing.ReadOnly = true;
			this.textRedGing.Size = new System.Drawing.Size(27, 20);
			this.textRedGing.TabIndex = 15;
			// 
			// textCountMGJ
			// 
			this.textCountMGJ.Location = new System.Drawing.Point(141, 90);
			this.textCountMGJ.Name = "textCountMGJ";
			this.textCountMGJ.ReadOnly = true;
			this.textCountMGJ.Size = new System.Drawing.Size(34, 20);
			this.textCountMGJ.TabIndex = 14;
			// 
			// updownMGJ
			// 
			this.updownMGJ.InterceptArrowKeys = false;
			this.updownMGJ.Location = new System.Drawing.Point(97, 90);
			this.updownMGJ.Name = "updownMGJ";
			this.updownMGJ.Size = new System.Drawing.Size(19, 20);
			this.updownMGJ.TabIndex = 13;
			this.updownMGJ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.updownRed_MouseDown);
			// 
			// textRedMGJ
			// 
			this.textRedMGJ.Location = new System.Drawing.Point(70, 90);
			this.textRedMGJ.Name = "textRedMGJ";
			this.textRedMGJ.ReadOnly = true;
			this.textRedMGJ.Size = new System.Drawing.Size(27, 20);
			this.textRedMGJ.TabIndex = 12;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(125, 49);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(52, 16);
			this.label14.TabIndex = 11;
			this.label14.Text = "# Teeth";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textCountProb
			// 
			this.textCountProb.Location = new System.Drawing.Point(141, 70);
			this.textCountProb.Name = "textCountProb";
			this.textCountProb.ReadOnly = true;
			this.textCountProb.Size = new System.Drawing.Size(34, 20);
			this.textCountProb.TabIndex = 10;
			// 
			// updownProb
			// 
			this.updownProb.InterceptArrowKeys = false;
			this.updownProb.Location = new System.Drawing.Point(97, 70);
			this.updownProb.Name = "updownProb";
			this.updownProb.Size = new System.Drawing.Size(19, 20);
			this.updownProb.TabIndex = 9;
			this.updownProb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.updownRed_MouseDown);
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(7, 50);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(84, 16);
			this.label13.TabIndex = 8;
			this.label13.Text = "Red if >=";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textRedProb
			// 
			this.textRedProb.Location = new System.Drawing.Point(70, 70);
			this.textRedProb.Name = "textRedProb";
			this.textRedProb.ReadOnly = true;
			this.textRedProb.Size = new System.Drawing.Size(27, 20);
			this.textRedProb.TabIndex = 0;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(6, 152);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(64, 16);
			this.label12.TabIndex = 7;
			this.label12.Text = "Furc";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(6, 172);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(64, 16);
			this.label7.TabIndex = 6;
			this.label7.Text = "Mobility";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(6, 132);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(64, 16);
			this.label11.TabIndex = 5;
			this.label11.Text = "CAL";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(6, 112);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(64, 16);
			this.label10.TabIndex = 4;
			this.label10.Text = "Ging Marg";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(6, 92);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(64, 16);
			this.label9.TabIndex = 3;
			this.label9.Text = "MGJ (<=)";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(6, 72);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(64, 16);
			this.label8.TabIndex = 2;
			this.label8.Text = "Probing";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(5, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 19);
			this.label1.TabIndex = 51;
			this.label1.Text = "Exams";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(757, 641);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(123, 42);
			this.label6.TabIndex = 54;
			this.label6.Text = "(All exams are saved automatically)";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// listExams
			// 
			this.listExams.ItemHeight = 14;
			this.listExams.Location = new System.Drawing.Point(7, 37);
			this.listExams.Name = "listExams";
			this.listExams.Size = new System.Drawing.Size(124, 130);
			this.listExams.TabIndex = 59;
			this.listExams.DoubleClick += new System.EventHandler(this.listExams_DoubleClick);
			this.listExams.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listExams_MouseDown);
			// 
			// textIndexPlaque
			// 
			this.textIndexPlaque.Location = new System.Drawing.Point(868, 245);
			this.textIndexPlaque.Name = "textIndexPlaque";
			this.textIndexPlaque.ReadOnly = true;
			this.textIndexPlaque.Size = new System.Drawing.Size(38, 20);
			this.textIndexPlaque.TabIndex = 70;
			this.textIndexPlaque.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// textIndexSupp
			// 
			this.textIndexSupp.Location = new System.Drawing.Point(868, 335);
			this.textIndexSupp.Name = "textIndexSupp";
			this.textIndexSupp.ReadOnly = true;
			this.textIndexSupp.Size = new System.Drawing.Size(38, 20);
			this.textIndexSupp.TabIndex = 71;
			this.textIndexSupp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// textIndexBleeding
			// 
			this.textIndexBleeding.Location = new System.Drawing.Point(868, 305);
			this.textIndexBleeding.Name = "textIndexBleeding";
			this.textIndexBleeding.ReadOnly = true;
			this.textIndexBleeding.Size = new System.Drawing.Size(38, 20);
			this.textIndexBleeding.TabIndex = 72;
			this.textIndexBleeding.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// textIndexCalculus
			// 
			this.textIndexCalculus.Location = new System.Drawing.Point(868, 275);
			this.textIndexCalculus.Name = "textIndexCalculus";
			this.textIndexCalculus.ReadOnly = true;
			this.textIndexCalculus.Size = new System.Drawing.Size(38, 20);
			this.textIndexCalculus.TabIndex = 73;
			this.textIndexCalculus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// printPreviewDlg
			// 
			this.printPreviewDlg.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.printPreviewDlg.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.printPreviewDlg.ClientSize = new System.Drawing.Size(400, 300);
			this.printPreviewDlg.Enabled = true;
			this.printPreviewDlg.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDlg.Icon")));
			this.printPreviewDlg.Name = "printPreviewDlg";
			this.printPreviewDlg.Visible = false;
			// 
			// labelPlaqueHistory
			// 
			this.labelPlaqueHistory.Location = new System.Drawing.Point(5, 334);
			this.labelPlaqueHistory.Name = "labelPlaqueHistory";
			this.labelPlaqueHistory.Size = new System.Drawing.Size(126, 19);
			this.labelPlaqueHistory.TabIndex = 78;
			this.labelPlaqueHistory.Text = "Plaque Index History";
			this.labelPlaqueHistory.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.labelPlaqueHistory.Visible = false;
			// 
			// listPlaqueHistory
			// 
			this.listPlaqueHistory.ItemHeight = 14;
			this.listPlaqueHistory.Location = new System.Drawing.Point(7, 356);
			this.listPlaqueHistory.Name = "listPlaqueHistory";
			this.listPlaqueHistory.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listPlaqueHistory.Size = new System.Drawing.Size(124, 130);
			this.listPlaqueHistory.TabIndex = 79;
			this.listPlaqueHistory.Visible = false;
			// 
			// textInputBox
			// 
			this.textInputBox.Location = new System.Drawing.Point(168, 22);
			this.textInputBox.Name = "textInputBox";
			this.textInputBox.Size = new System.Drawing.Size(53, 20);
			this.textInputBox.TabIndex = 81;
			this.textInputBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.textInputBox.TextChanged += new System.EventHandler(this.textInputBox_TextChanged);
			this.textInputBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textInputBox_KeyDown);
			this.textInputBox.Leave += new System.EventHandler(this.textInputBox_Leave);
			// 
			// butCopyPrevious
			// 
			this.butCopyPrevious.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCopyPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCopyPrevious.Autosize = true;
			this.butCopyPrevious.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCopyPrevious.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCopyPrevious.CornerRadius = 4F;
			this.butCopyPrevious.Image = global::OpenDental.Properties.Resources.Add;
			this.butCopyPrevious.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCopyPrevious.Location = new System.Drawing.Point(7, 227);
			this.butCopyPrevious.Name = "butCopyPrevious";
			this.butCopyPrevious.Size = new System.Drawing.Size(124, 24);
			this.butCopyPrevious.TabIndex = 1;
			this.butCopyPrevious.Text = "Copy Previous";
			this.butCopyPrevious.Click += new System.EventHandler(this.butCopyPrevious_Click);
			// 
			// gridP
			// 
			this.gridP.BackColor = System.Drawing.SystemColors.Window;
			this.gridP.DoShowCurrentExamOnly = true;
			this.gridP.Location = new System.Drawing.Point(157, 11);
			this.gridP.Name = "gridP";
			this.gridP.SelectedExam = 0;
			this.gridP.Size = new System.Drawing.Size(595, 665);
			this.gridP.TabIndex = 75;
			this.gridP.Text = "contrPerio2";
			this.gridP.DirectionChangedRight += new System.EventHandler(this.gridP_DirectionChangedRight);
			this.gridP.DirectionChangedLeft += new System.EventHandler(this.gridP_DirectionChangedLeft);
			// 
			// butSave
			// 
			this.butSave.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSave.Autosize = true;
			this.butSave.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSave.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSave.CornerRadius = 4F;
			this.butSave.Location = new System.Drawing.Point(764, 616);
			this.butSave.Name = "butSave";
			this.butSave.Size = new System.Drawing.Size(88, 24);
			this.butSave.TabIndex = 21;
			this.butSave.Text = "Save to Images";
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			// 
			// butGraphical
			// 
			this.butGraphical.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGraphical.Autosize = true;
			this.butGraphical.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGraphical.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGraphical.CornerRadius = 4F;
			this.butGraphical.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butGraphical.Location = new System.Drawing.Point(885, 580);
			this.butGraphical.Name = "butGraphical";
			this.butGraphical.Size = new System.Drawing.Size(75, 24);
			this.butGraphical.TabIndex = 20;
			this.butGraphical.Text = "Graphical";
			this.butGraphical.Click += new System.EventHandler(this.butGraphical_Click);
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(885, 616);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(75, 24);
			this.butPrint.TabIndex = 22;
			this.butPrint.Text = "Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(7, 197);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(124, 24);
			this.butAdd.TabIndex = 0;
			this.butAdd.Text = "Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// but0
			// 
			this.but0.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.but0.Autosize = true;
			this.but0.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.but0.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.but0.CornerRadius = 4F;
			this.but0.Location = new System.Drawing.Point(763, 173);
			this.but0.Name = "but0";
			this.but0.Size = new System.Drawing.Size(67, 32);
			this.but0.TabIndex = 13;
			this.but0.Text = "0";
			this.but0.Click += new System.EventHandler(this.but0_Click);
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(7, 257);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(124, 24);
			this.butDelete.TabIndex = 2;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// but8
			// 
			this.but8.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.but8.Autosize = true;
			this.but8.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.but8.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.but8.CornerRadius = 4F;
			this.but8.Location = new System.Drawing.Point(798, 68);
			this.but8.Name = "but8";
			this.but8.Size = new System.Drawing.Size(32, 32);
			this.but8.TabIndex = 11;
			this.but8.Text = "8";
			this.but8.Click += new System.EventHandler(this.but8_Click);
			// 
			// but4
			// 
			this.but4.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.but4.Autosize = true;
			this.but4.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.but4.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.but4.CornerRadius = 4F;
			this.but4.Location = new System.Drawing.Point(763, 103);
			this.but4.Name = "but4";
			this.but4.Size = new System.Drawing.Size(32, 32);
			this.but4.TabIndex = 7;
			this.but4.Text = "4";
			this.but4.Click += new System.EventHandler(this.but4_Click);
			// 
			// but5
			// 
			this.but5.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.but5.Autosize = true;
			this.but5.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.but5.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.but5.CornerRadius = 4F;
			this.but5.Location = new System.Drawing.Point(798, 103);
			this.but5.Name = "but5";
			this.but5.Size = new System.Drawing.Size(32, 32);
			this.but5.TabIndex = 8;
			this.but5.Text = "5";
			this.but5.Click += new System.EventHandler(this.but5_Click);
			// 
			// but9
			// 
			this.but9.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.but9.Autosize = true;
			this.but9.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.but9.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.but9.CornerRadius = 4F;
			this.but9.Location = new System.Drawing.Point(833, 68);
			this.but9.Name = "but9";
			this.but9.Size = new System.Drawing.Size(32, 32);
			this.but9.TabIndex = 12;
			this.but9.Text = "9";
			this.but9.Click += new System.EventHandler(this.but9_Click);
			// 
			// but6
			// 
			this.but6.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.but6.Autosize = true;
			this.but6.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.but6.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.but6.CornerRadius = 4F;
			this.but6.Location = new System.Drawing.Point(833, 103);
			this.but6.Name = "but6";
			this.but6.Size = new System.Drawing.Size(32, 32);
			this.but6.TabIndex = 9;
			this.but6.Text = "6";
			this.but6.Click += new System.EventHandler(this.but6_Click);
			// 
			// but1
			// 
			this.but1.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.but1.Autosize = true;
			this.but1.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.but1.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.but1.CornerRadius = 4F;
			this.but1.Location = new System.Drawing.Point(763, 138);
			this.but1.Name = "but1";
			this.but1.Size = new System.Drawing.Size(32, 32);
			this.but1.TabIndex = 4;
			this.but1.Text = "1";
			this.but1.Click += new System.EventHandler(this.but1_Click);
			// 
			// but2
			// 
			this.but2.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.but2.Autosize = true;
			this.but2.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.but2.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.but2.CornerRadius = 4F;
			this.but2.Location = new System.Drawing.Point(798, 138);
			this.but2.Name = "but2";
			this.but2.Size = new System.Drawing.Size(32, 32);
			this.but2.TabIndex = 5;
			this.but2.Text = "2";
			this.but2.Click += new System.EventHandler(this.but2_Click);
			// 
			// but3
			// 
			this.but3.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.but3.Autosize = true;
			this.but3.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.but3.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.but3.CornerRadius = 4F;
			this.but3.Location = new System.Drawing.Point(833, 138);
			this.but3.Name = "but3";
			this.but3.Size = new System.Drawing.Size(32, 32);
			this.but3.TabIndex = 6;
			this.but3.Text = "3";
			this.but3.Click += new System.EventHandler(this.but3_Click);
			// 
			// but7
			// 
			this.but7.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.but7.Autosize = true;
			this.but7.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.but7.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.but7.CornerRadius = 4F;
			this.but7.Location = new System.Drawing.Point(763, 68);
			this.but7.Name = "but7";
			this.but7.Size = new System.Drawing.Size(32, 32);
			this.but7.TabIndex = 10;
			this.but7.Text = "7";
			this.but7.Click += new System.EventHandler(this.but7_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(885, 658);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 23;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butListen
			// 
			this.butListen.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butListen.Autosize = true;
			this.butListen.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butListen.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butListen.CornerRadius = 6F;
			this.butListen.Image = global::OpenDental.Properties.Resources.Microphone_22px;
			this.butListen.Location = new System.Drawing.Point(7, 286);
			this.butListen.Name = "butListen";
			this.butListen.Size = new System.Drawing.Size(30, 30);
			this.butListen.TabIndex = 3;
			this.butListen.Click += new System.EventHandler(this.butListen_Click);
			// 
			// labelListening
			// 
			this.labelListening.ForeColor = System.Drawing.Color.ForestGreen;
			this.labelListening.Location = new System.Drawing.Point(42, 292);
			this.labelListening.Name = "labelListening";
			this.labelListening.Size = new System.Drawing.Size(98, 19);
			this.labelListening.TabIndex = 84;
			this.labelListening.Text = "Listening";
			this.labelListening.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelListening.Visible = false;
			// 
			// FormPerio
			// 
			this.ClientSize = new System.Drawing.Size(982, 700);
			this.Controls.Add(this.labelListening);
			this.Controls.Add(this.butListen);
			this.Controls.Add(this.checkShowCurrent);
			this.Controls.Add(this.butCopyPrevious);
			this.Controls.Add(this.gridP);
			this.Controls.Add(this.textInputBox);
			this.Controls.Add(this.checkGingMarg);
			this.Controls.Add(this.listPlaqueHistory);
			this.Controls.Add(this.labelPlaqueHistory);
			this.Controls.Add(this.butSave);
			this.Controls.Add(this.butGraphical);
			this.Controls.Add(this.butCalcIndex);
			this.Controls.Add(this.textIndexCalculus);
			this.Controls.Add(this.textIndexBleeding);
			this.Controls.Add(this.textIndexSupp);
			this.Controls.Add(this.textIndexPlaque);
			this.Controls.Add(this.butColorCalculus);
			this.Controls.Add(this.butColorPlaque);
			this.Controls.Add(this.butCalculus);
			this.Controls.Add(this.butPlaque);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.butSkip);
			this.Controls.Add(this.listExams);
			this.Controls.Add(this.checkThree);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butColorPus);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.butColorBleed);
			this.Controls.Add(this.butPus);
			this.Controls.Add(this.butBleed);
			this.Controls.Add(this.but10);
			this.Controls.Add(this.but0);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.but8);
			this.Controls.Add(this.but4);
			this.Controls.Add(this.but5);
			this.Controls.Add(this.but9);
			this.Controls.Add(this.but6);
			this.Controls.Add(this.but1);
			this.Controls.Add(this.but2);
			this.Controls.Add(this.but3);
			this.Controls.Add(this.but7);
			this.Controls.Add(this.butClose);
			this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormPerio";
			this.ShowInTaskbar = false;
			this.Text = "Perio Chart";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormPerio_Closing);
			this.Load += new System.EventHandler(this.FormPerio_Load);
			this.Shown += new System.EventHandler(this.FormPerio_Shown);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormPerio_Load(object sender, System.EventArgs e) {
			_listMiscColorDefs=Defs.GetDefsForCategory(DefCat.MiscColors,true);
			butColorBleed.BackColor=_listMiscColorDefs[1].ItemColor;
			butColorPus.BackColor=_listMiscColorDefs[2].ItemColor;
			butColorPlaque.BackColor=_listMiscColorDefs[4].ItemColor;
			butColorCalculus.BackColor=_listMiscColorDefs[5].ItemColor;
			textRedProb.Text=PrefC.GetString(PrefName.PerioRedProb);
			textRedMGJ.Text =PrefC.GetString(PrefName.PerioRedMGJ);
			textRedGing.Text=PrefC.GetString(PrefName.PerioRedGing);
			textRedCAL.Text =PrefC.GetString(PrefName.PerioRedCAL);
			textRedFurc.Text=PrefC.GetString(PrefName.PerioRedFurc);
			textRedMob.Text =PrefC.GetString(PrefName.PerioRedMob);
			//Procedure[] procList=Procedures.Refresh(PatCur.PatNum);
			List<ToothInitial> initialList=ToothInitials.Refresh(PatCur.PatNum);
			_listMissingTeeth=ToothInitials.GetMissingOrHiddenTeeth(initialList);
			RefreshListExams();
			if(Programs.UsingOrion) {
				labelPlaqueHistory.Visible=true;
				listPlaqueHistory.Visible=true;
				RefreshListPlaque();
			}
			listExams.SelectedIndex=PerioExams.ListExams.Count-1;//this works even if no items.
			_userPrefCurrentOnly=UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.PerioCurrentExamOnly).FirstOrDefault();
			if(_userPrefCurrentOnly != null && PIn.Bool(_userPrefCurrentOnly.ValueString)) {
				checkShowCurrent.Checked=true;
			}
			FillGrid();
			Plugins.HookAddCode(this,"FormPerio.Load_end");
		}

		///<summary>Performs the action for the recognized command.</summary>
		private void VoiceController_SpeechRecognized(object sender,ODSpeechRecognizedEventArgs e) {
			if(listExams.SelectedIndex < 0 //No exam selected
				&& !e.Command.ActionToPerform.In(VoiceCommandAction.StartListening,
					VoiceCommandAction.StopListening,
					VoiceCommandAction.CreatePerioExam,
					VoiceCommandAction.CopyPrevious,
					VoiceCommandAction.DidntGetThat)) 
			{ 
				_voiceController?.StopListening();
				VoiceMsgBox.Show("Please select an exam first.");
				_voiceController?.StartListening();
				return;
			}
			DateTime dateSecurity;
			if(listExams.SelectedIndex > -1) {
				dateSecurity=PerioExams.ListExams[listExams.SelectedIndex].ExamDate;
			}
			else {
				dateSecurity=MiscData.GetNowDateTime();
			}
			if(!Security.IsAuthorized(Permissions.PerioEdit,dateSecurity,true) 
				&& !e.Command.ActionToPerform.In(VoiceCommandAction.StartListening,
					VoiceCommandAction.StopListening,
					VoiceCommandAction.DidntGetThat)) 
			{
				_voiceController?.StopListening();
				VoiceMsgBox.Show("Not authorized to edit the perio chart.");
				return;
			}
			gridP.Focus();
			_curLocation=gridP.GetCurrentCell();
			switch(e.Command.ActionToPerform) {
				#region Initialization
				case VoiceCommandAction.StartListening:
					labelListening.Visible=true;
					break;
				case VoiceCommandAction.StopListening:
					labelListening.Visible=false;
					break;
				case VoiceCommandAction.CreatePerioExam:
					butAdd.PerformClick();
					break;
				case VoiceCommandAction.CopyPrevious:
					if(listExams.SelectedIndex==-1) {
						VoiceMsgBox.Show("There are currently no exams to copy from.");
						return;
					}
					butCopyPrevious.PerformClick();
					break;
				#endregion Initialization
				#region Probing Depths
				case VoiceCommandAction.Zero:
					but0.PerformClick();
					break;
				case VoiceCommandAction.One:
					but1.PerformClick();
					break;
				case VoiceCommandAction.Two:
					but2.PerformClick();
					break;
				case VoiceCommandAction.Three:
					but3.PerformClick();
					break;
				case VoiceCommandAction.Four:
					but4.PerformClick();
					break;
				case VoiceCommandAction.Five:
					but5.PerformClick();
					break;
				case VoiceCommandAction.Six:
					but6.PerformClick();
					break;
				case VoiceCommandAction.Seven:
					but7.PerformClick();
					break;
				case VoiceCommandAction.Eight:
					but8.PerformClick();
					break;
				case VoiceCommandAction.Nine:
					but9.PerformClick();
					break;
				case VoiceCommandAction.Ten:
					but10.PerformClick();
					but0.PerformClick();
					break;
				case VoiceCommandAction.Eleven:
					but10.PerformClick();
					but1.PerformClick();
					break;
				case VoiceCommandAction.Twelve:
					but10.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.Thirteen:
					but10.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.Fourteen:
					but10.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.Fifteen:
					but10.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.Sixteen:
					but10.PerformClick();
					but6.PerformClick();
					break;
				case VoiceCommandAction.Seventeen:
					but10.PerformClick();
					but7.PerformClick();
					break;
				case VoiceCommandAction.Eighteen:
					but10.PerformClick();
					but8.PerformClick();
					break;
				case VoiceCommandAction.Nineteen:
					but10.PerformClick();
					but9.PerformClick();
					break;
				case VoiceCommandAction.ThreeTwoThree:
					but3.PerformClick();
					but2.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FourThreeFour:
					but4.PerformClick();
					but3.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.ThreeThreeThree:
					but3.PerformClick();
					but3.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.TwoTwoTwo:
					but2.PerformClick();
					but2.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FourFourFour:
					but4.PerformClick();
					but4.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.TwoOneTwo:
					but2.PerformClick();
					but1.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FourThreeThree:
					but4.PerformClick();
					but3.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ThreeThreeFour:
					but3.PerformClick();
					but3.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.TwoTwoThree:
					but2.PerformClick();
					but2.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.ThreeTwoTwo:
					but3.PerformClick();
					but2.PerformClick();
					but2.PerformClick();
					break;
				case VoiceCommandAction.FiveFourFive:
					but5.PerformClick();
					but4.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FiveThreeFive:
					but5.PerformClick();
					but3.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ThreeThreeFive:
					but3.PerformClick();
					but3.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FiveThreeThree:
					but5.PerformClick();
					but3.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FourFourFive:
					but4.PerformClick();
					but4.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FiveFourFour:
					but5.PerformClick();
					but4.PerformClick();
					but4.PerformClick();
					break;
				case VoiceCommandAction.FiveFiveFive:
					but5.PerformClick();
					but5.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.ThreeFourThree:
					but3.PerformClick();
					but4.PerformClick();
					but3.PerformClick();
					break;
				case VoiceCommandAction.FourThreeFive:
					but4.PerformClick();
					but3.PerformClick();
					but5.PerformClick();
					break;
				case VoiceCommandAction.FiveThreeFour:
					but5.PerformClick();
					but3.PerformClick();
					but4.PerformClick();
					break;
				#endregion Probing Depths
				#region Misc Buttons/Checkboxes
				case VoiceCommandAction.Triplets:
					checkThree.Checked=(!checkThree.Checked);
					gridP.ThreeAtATime=checkThree.Checked;
					break;
				case VoiceCommandAction.CheckTriplets:
					checkThree.Checked=true;
					gridP.ThreeAtATime=true;
					break;
				case VoiceCommandAction.UncheckTriplets:
					checkThree.Checked=false;
					gridP.ThreeAtATime=false;
					break;
				case VoiceCommandAction.Bleeding:
					butBleed.PerformClick();
					break;
				case VoiceCommandAction.Plaque:
					butPlaque.PerformClick();
					break;
				case VoiceCommandAction.Calculus:
					butCalculus.PerformClick();
					break;
				case VoiceCommandAction.Suppuration:
					butPus.PerformClick();
					break;
				#endregion Misc Buttons/Checkboxes
				#region Navigation Keys
				case VoiceCommandAction.Backspace:
					SendKeys.Send("{BACKSPACE}");
					break;
				case VoiceCommandAction.Right:
					SendKeys.Send("{RIGHT}");
					break;
				case VoiceCommandAction.Left:
					SendKeys.Send("{LEFT}");
					break;
				case VoiceCommandAction.Delete:
					SendKeys.Send("{DELETE}");
					break;
				#endregion Navigation Keys
				#region Go To Tooth
				case VoiceCommandAction.GoToToothOneFacial:
					GoToTooth(1,true);
					break;
				case VoiceCommandAction.GoToToothTwoFacial:
					GoToTooth(2,true);
					break;
				case VoiceCommandAction.GoToToothThreeFacial:
					GoToTooth(3,true);
					break;
				case VoiceCommandAction.GoToToothFourFacial:
					GoToTooth(4,true);
					break;
				case VoiceCommandAction.GoToToothFiveFacial:
					GoToTooth(5,true);
					break;
				case VoiceCommandAction.GoToToothSixFacial:
					GoToTooth(6,true);
					break;
				case VoiceCommandAction.GoToToothSevenFacial:
					GoToTooth(7,true);
					break;
				case VoiceCommandAction.GoToToothEightFacial:
					GoToTooth(8,true);
					break;
				case VoiceCommandAction.GoToToothNineFacial:
					GoToTooth(9,true);
					break;
				case VoiceCommandAction.GoToToothTenFacial:
					GoToTooth(10,true);
					break;
				case VoiceCommandAction.GoToToothElevenFacial:
					GoToTooth(11,true);
					break;
				case VoiceCommandAction.GoToToothTwelveFacial:
					GoToTooth(12,true);
					break;
				case VoiceCommandAction.GoToToothThirteenFacial:
					GoToTooth(13,true);
					break;
				case VoiceCommandAction.GoToToothFourteenFacial:
					GoToTooth(14,true);
					break;
				case VoiceCommandAction.GoToToothFifteenFacial:
					GoToTooth(15,true);
					break;
				case VoiceCommandAction.GoToToothSixteenFacial:
					GoToTooth(16,true);
					break;
				case VoiceCommandAction.GoToToothSeventeenFacial:
					GoToTooth(17,true);
					break;
				case VoiceCommandAction.GoToToothEighteenFacial:
					GoToTooth(18,true);
					break;
				case VoiceCommandAction.GoToToothNineteenFacial:
					GoToTooth(19,true);
					break;
				case VoiceCommandAction.GoToToothTwentyFacial:
					GoToTooth(20,true);
					break;
				case VoiceCommandAction.GoToToothTwentyOneFacial:
					GoToTooth(21,true);
					break;
				case VoiceCommandAction.GoToToothTwentyTwoFacial:
					GoToTooth(22,true);
					break;
				case VoiceCommandAction.GoToToothTwentyThreeFacial:
					GoToTooth(23,true);
					break;
				case VoiceCommandAction.GoToToothTwentyFourFacial:
					GoToTooth(24,true);
					break;
				case VoiceCommandAction.GoToToothTwentyFiveFacial:
					GoToTooth(25,true);
					break;
				case VoiceCommandAction.GoToToothTwentySixFacial:
					GoToTooth(26,true);
					break;
				case VoiceCommandAction.GoToToothTwentySevenFacial:
					GoToTooth(27,true);
					break;
				case VoiceCommandAction.GoToToothTwentyEightFacial:
					GoToTooth(28,true);
					break;
				case VoiceCommandAction.GoToToothTwentyNineFacial:
					GoToTooth(29,true);
					break;
				case VoiceCommandAction.GoToToothThirtyFacial:
					GoToTooth(30,true);
					break;
				case VoiceCommandAction.GoToToothThirtyOneFacial:
					GoToTooth(31,true);
					break;
				case VoiceCommandAction.GoToToothThirtyTwoFacial:
					GoToTooth(32,true);
					break;
				case VoiceCommandAction.GoToToothOneLingual:
					GoToTooth(1,false);
					break;
				case VoiceCommandAction.GoToToothTwoLingual:
					GoToTooth(2,false);
					break;
				case VoiceCommandAction.GoToToothThreeLingual:
					GoToTooth(3,false);
					break;
				case VoiceCommandAction.GoToToothFourLingual:
					GoToTooth(4,false);
					break;
				case VoiceCommandAction.GoToToothFiveLingual:
					GoToTooth(5,false);
					break;
				case VoiceCommandAction.GoToToothSixLingual:
					GoToTooth(6,false);
					break;
				case VoiceCommandAction.GoToToothSevenLingual:
					GoToTooth(7,false);
					break;
				case VoiceCommandAction.GoToToothEightLingual:
					GoToTooth(8,false);
					break;
				case VoiceCommandAction.GoToToothNineLingual:
					GoToTooth(9,false);
					break;
				case VoiceCommandAction.GoToToothTenLingual:
					GoToTooth(10,false);
					break;
				case VoiceCommandAction.GoToToothElevenLingual:
					GoToTooth(11,false);
					break;
				case VoiceCommandAction.GoToToothTwelveLingual:
					GoToTooth(12,false);
					break;
				case VoiceCommandAction.GoToToothThirteenLingual:
					GoToTooth(13,false);
					break;
				case VoiceCommandAction.GoToToothFourteenLingual:
					GoToTooth(14,false);
					break;
				case VoiceCommandAction.GoToToothFifteenLingual:
					GoToTooth(15,false);
					break;
				case VoiceCommandAction.GoToToothSixteenLingual:
					GoToTooth(16,false);
					break;
				case VoiceCommandAction.GoToToothSeventeenLingual:
					GoToTooth(17,false);
					break;
				case VoiceCommandAction.GoToToothEighteenLingual:
					GoToTooth(18,false);
					break;
				case VoiceCommandAction.GoToToothNineteenLingual:
					GoToTooth(19,false);
					break;
				case VoiceCommandAction.GoToToothTwentyLingual:
					GoToTooth(20,false);
					break;
				case VoiceCommandAction.GoToToothTwentyOneLingual:
					GoToTooth(21,false);
					break;
				case VoiceCommandAction.GoToToothTwentyTwoLingual:
					GoToTooth(22,false);
					break;
				case VoiceCommandAction.GoToToothTwentyThreeLingual:
					GoToTooth(23,false);
					break;
				case VoiceCommandAction.GoToToothTwentyFourLingual:
					GoToTooth(24,false);
					break;
				case VoiceCommandAction.GoToToothTwentyFiveLingual:
					GoToTooth(25,false);
					break;
				case VoiceCommandAction.GoToToothTwentySixLingual:
					GoToTooth(26,false);
					break;
				case VoiceCommandAction.GoToToothTwentySevenLingual:
					GoToTooth(27,false);
					break;
				case VoiceCommandAction.GoToToothTwentyEightLingual:
					GoToTooth(28,false);
					break;
				case VoiceCommandAction.GoToToothTwentyNineLingual:
					GoToTooth(29,false);
					break;
				case VoiceCommandAction.GoToToothThirtyLingual:
					GoToTooth(30,false);
					break;
				case VoiceCommandAction.GoToToothThirtyOneLingual:
					GoToTooth(31,false);
					break;
				case VoiceCommandAction.GoToToothThirtyTwoLingual:
					GoToTooth(32,false);
					break;
				#endregion Go To Tooth
				#region Skip Tooth
				case VoiceCommandAction.SkipToothOne:
					e.Command.DoSayResponse=SkipTooth(1);
					break;
				case VoiceCommandAction.SkipToothTwo:
					e.Command.DoSayResponse=SkipTooth(2);
					break;
				case VoiceCommandAction.SkipToothThree:
					e.Command.DoSayResponse=SkipTooth(3);
					break;
				case VoiceCommandAction.SkipToothFour:
					e.Command.DoSayResponse=SkipTooth(4);
					break;
				case VoiceCommandAction.SkipToothFive:
					e.Command.DoSayResponse=SkipTooth(5);
					break;
				case VoiceCommandAction.SkipToothSix:
					e.Command.DoSayResponse=SkipTooth(6);
					break;
				case VoiceCommandAction.SkipToothSeven:
					e.Command.DoSayResponse=SkipTooth(7);
					break;
				case VoiceCommandAction.SkipToothEight:
					e.Command.DoSayResponse=SkipTooth(8);
					break;
				case VoiceCommandAction.SkipToothNine:
					e.Command.DoSayResponse=SkipTooth(9);
					break;
				case VoiceCommandAction.SkipToothTen:
					e.Command.DoSayResponse=SkipTooth(10);
					break;
				case VoiceCommandAction.SkipToothEleven:
					e.Command.DoSayResponse=SkipTooth(11);
					break;
				case VoiceCommandAction.SkipToothTwelve:
					e.Command.DoSayResponse=SkipTooth(12);
					break;
				case VoiceCommandAction.SkipToothThirteen:
					e.Command.DoSayResponse=SkipTooth(13);
					break;
				case VoiceCommandAction.SkipToothFourteen:
					e.Command.DoSayResponse=SkipTooth(14);
					break;
				case VoiceCommandAction.SkipToothFifteen:
					e.Command.DoSayResponse=SkipTooth(15);
					break;
				case VoiceCommandAction.SkipToothSixteen:
					e.Command.DoSayResponse=SkipTooth(16);
					break;
				case VoiceCommandAction.SkipToothSeventeen:
					e.Command.DoSayResponse=SkipTooth(17);
					break;
				case VoiceCommandAction.SkipToothEighteen:
					e.Command.DoSayResponse=SkipTooth(18);
					break;
				case VoiceCommandAction.SkipToothNineteen:
					e.Command.DoSayResponse=SkipTooth(19);
					break;
				case VoiceCommandAction.SkipToothTwenty:
					e.Command.DoSayResponse=SkipTooth(20);
					break;
				case VoiceCommandAction.SkipToothTwentyOne:
					e.Command.DoSayResponse=SkipTooth(21);
					break;
				case VoiceCommandAction.SkipToothTwentyTwo:
					e.Command.DoSayResponse=SkipTooth(22);
					break;
				case VoiceCommandAction.SkipToothTwentyThree:
					e.Command.DoSayResponse=SkipTooth(23);
					break;
				case VoiceCommandAction.SkipToothTwentyFour:
					e.Command.DoSayResponse=SkipTooth(24);
					break;
				case VoiceCommandAction.SkipToothTwentyFive:
					e.Command.DoSayResponse=SkipTooth(25);
					break;
				case VoiceCommandAction.SkipToothTwentySix:
					e.Command.DoSayResponse=SkipTooth(26);
					break;
				case VoiceCommandAction.SkipToothTwentySeven:
					e.Command.DoSayResponse=SkipTooth(27);
					break;
				case VoiceCommandAction.SkipToothTwentyEight:
					e.Command.DoSayResponse=SkipTooth(28);
					break;
				case VoiceCommandAction.SkipToothTwentyNine:
					e.Command.DoSayResponse=SkipTooth(29);
					break;
				case VoiceCommandAction.SkipToothThirty:
					e.Command.DoSayResponse=SkipTooth(30);
					break;
				case VoiceCommandAction.SkipToothThirtyOne:
					e.Command.DoSayResponse=SkipTooth(31);
					break;
				case VoiceCommandAction.SkipToothThirtyTwo:
					e.Command.DoSayResponse=SkipTooth(32);
					break;
				case VoiceCommandAction.SkipCurrentTooth:
					e.Command.DoSayResponse=SkipTooth(_curLocation.ToothNum);
					break;
				#endregion Skip Tooth
				#region Position
				case VoiceCommandAction.Probing:
					GoToProbing();
					break;
				case VoiceCommandAction.MucoGingivalJunction:
					GoToMGJ();
					break;
				case VoiceCommandAction.GingivalMargin:
					GoToGingivalMargin();
					break;
				case VoiceCommandAction.Furcation:
					GoToFurcation();
					break;
				case VoiceCommandAction.Mobility:
					GoToMobility();
					break;
				#endregion Position
				#region Pluses
				case VoiceCommandAction.PlusOne:
					gridP.GingMargPlus=true;
					but1.PerformClick();
					gridP.GingMargPlus=false;
					break;
				case VoiceCommandAction.PlusTwo:
					gridP.GingMargPlus=true;
					but2.PerformClick();
					gridP.GingMargPlus=false;
					break;
				case VoiceCommandAction.PlusThree:
					gridP.GingMargPlus=true;
					but3.PerformClick();
					gridP.GingMargPlus=false;
					break;
				case VoiceCommandAction.PlusFour:
					gridP.GingMargPlus=true;
					but4.PerformClick();
					gridP.GingMargPlus=false;
					break;
				case VoiceCommandAction.PlusFive:
					gridP.GingMargPlus=true;
					but5.PerformClick();
					gridP.GingMargPlus=false;
					break;
				case VoiceCommandAction.PlusSix:
					gridP.GingMargPlus=true;
					but6.PerformClick();
					gridP.GingMargPlus=false;
					break;
				case VoiceCommandAction.PlusSeven:
					gridP.GingMargPlus=true;
					but7.PerformClick();
					gridP.GingMargPlus=false;
					break;
				case VoiceCommandAction.PlusEight:
					gridP.GingMargPlus=true;
					but8.PerformClick();
					gridP.GingMargPlus=false;
					break;
				case VoiceCommandAction.PlusNine:
					gridP.GingMargPlus=true;
					but9.PerformClick();
					gridP.GingMargPlus=false;
					break;
				#endregion Pluses
				default:
					return;//Do nothing for any other command
			}
			if(e.Command.DoSayResponse) {
				_voiceController.SayResponseAsync(e.Command.Response);
			}
			_curLocation=gridP.GetCurrentCell();
			SetAutoAdvance();
			_prevLocation=_curLocation;
		}
		
		///<summary>Goes to the first cell in the grid for this tooth.</summary>
		///<param name="isFacial">When false, equivalent to lingual.</param>
		private void GoToTooth(int toothNum,bool isFacial) {
			bool isLingual=(!isFacial);
			int x=-1;
			int y=-1;
			if(isFacial && toothNum.Between(1,16)) {
				x=(toothNum)*3-2;
				if(checkShowCurrent.Checked) {
					y=5;
				}
				else {
					y=6-Math.Min(6,gridP.SelectedExam+1);
				}
			}
			if(isFacial && toothNum.Between(17,32)) {
				x=(32-toothNum)*3+1;
				if(checkShowCurrent.Checked) {
					y=37;
				}
				else {
					y=36+Math.Min(6,gridP.SelectedExam+1);
				}
			}
			if(isLingual && toothNum.Between(1,16)) {
				x=(toothNum)*3;
				if(checkShowCurrent.Checked) {
					y=15;
				}
				else {
					y=14+Math.Min(6,gridP.SelectedExam+1);
				}
			}
			if(isLingual && toothNum.Between(17,32)) {
				x=(32-toothNum)*3+3;
				if(checkShowCurrent.Checked) {
					y=26;
				}
				else {
					y=27-Math.Min(6,gridP.SelectedExam+1);
				}
			}
			gridP.SetNewCell(x,y);
		}

		///<summary>Sets the approprate Auto Advance radio button.</summary>
		private void SetAutoAdvance() {
			if(_prevLocation.Surface==PerioSurface.Facial && _curLocation.Surface==PerioSurface.Lingual) {
				gridP.DirectionIsRight=true;
				radioRight.Checked=true;
			}
			if(_prevLocation.Surface==PerioSurface.Lingual && _curLocation.Surface==PerioSurface.Facial) {
				gridP.DirectionIsRight=false;
				radioLeft.Checked=true;
			}
		}

		///<summary>Marks the tooth as skipped.</summary>
		///<param name="doVerifyByVoice">If true, will verbally ask the user if they want to skip the tooth.</param>
		///<returns>True if the user does choose to skip the tooth.</returns>
		private bool SkipTooth(int toothNum,bool doVerifyByVoice=true) {
			if(doVerifyByVoice) {
				_voiceController?.StopListening();
				if(!VoiceMsgBox.Show("Mark tooth "+toothNum+" as skipped?",MsgBoxButtons.YesNo)) {
					_voiceController?.StartListening();
					return false;
				}
				_voiceController?.StartListening();
			}
			Point curPoint=gridP.CurCell;
			bool radioRightChecked=radioRight.Checked;
			gridP.SaveCurExam(PerioExamCur);
			int selectedExam=gridP.SelectedExam;
			List<int> listSkippedTeeth=new List<int>();//int 1-32
			if(PerioExams.ListExams.Count > 0) {
				//set skipped teeth based on the last exam in the list: 
				listSkippedTeeth=PerioMeasures.GetSkipped(PerioExams.ListExams[PerioExams.ListExams.Count-1].PerioExamNum);
			}
			if(!listSkippedTeeth.Contains(toothNum)) {
				listSkippedTeeth.Add(toothNum);
			}
			PerioMeasures.SetSkipped(PerioExamCur.PerioExamNum,listSkippedTeeth);
			RefreshListExams();
			listExams.SelectedIndex=selectedExam;
			FillGrid();
			gridP.SetNewCell(curPoint.X,curPoint.Y);
			radioRight.Checked=radioRightChecked;
			gridP.DirectionIsRight=radioRightChecked;
			return true;
		}

		///<summary>Moves the cursor to the probing row for the current tooth.</summary>
		private void GoToProbing() {
			int yVal;
			if(_curLocation.Surface==PerioSurface.Facial) {
				if(_curLocation.ToothNum<=16) {
					if(checkShowCurrent.Checked) {
						yVal=5;
					}
					else {
						yVal=6-Math.Min(6,gridP.SelectedExam+1);
					}
				}
				else {//ToothNum >= 17
					if(checkShowCurrent.Checked) {
						yVal=37;
					}
					else {
						yVal=36+Math.Min(6,gridP.SelectedExam+1);
					}
				}
			}
			else {//Lingual
				if(_curLocation.ToothNum<=16) {
					if(checkShowCurrent.Checked) {
						yVal=15;
					}
					else {
						yVal=14+Math.Min(6,gridP.SelectedExam+1);
					}
				}
				else {//ToothNum >= 17
					if(checkShowCurrent.Checked) {
						yVal=26;
					}
					else {
						yVal=27-Math.Min(6,gridP.SelectedExam+1);
					}
				}
			}
			gridP.SetNewCell(FirstEmptyPositionX(_curLocation.ToothNum,yVal),yVal);
		}

		///<summary>Moves the cursor to the mobility row for the current tooth.</summary>
		private void GoToMobility() {
			int yVal;
			int xVal;
			if(_curLocation.ToothNum <= 16) {
				xVal=(_curLocation.ToothNum-1)*3+2;//Middle cell
				yVal=10;
			}
			else {//ToothNum >= 17
				xVal=xVal=47-(_curLocation.ToothNum-17)*3;
				yVal=32;
			}
			gridP.SetNewCell(xVal,yVal);
		}

		///<summary>Moves the cursor to the gingival margin row for the current tooth.</summary>
		private void GoToGingivalMargin() {
			int yVal;
			if(_curLocation.Surface==PerioSurface.Facial) {
				if(_curLocation.ToothNum<=16) {
					yVal=7;
				}
				else {//ToothNum >= 17
					yVal=35;
				}
			}
			else {//Lingual
				if(_curLocation.ToothNum<=16) {
					yVal=14;
				}
				else {//ToothNum >= 17
					yVal=28;
				}
			}
			gridP.SetNewCell(FirstEmptyPositionX(_curLocation.ToothNum,yVal),yVal);
		}

		///<summary>Moves the cursor to the furcation row for the current tooth.</summary>
		private void GoToFurcation() {
			int yVal;
			if(_curLocation.Surface==PerioSurface.Facial) {
				if(_curLocation.ToothNum<=16) {
					yVal=9;
				}
				else {//ToothNum >= 17
					yVal=33;
				}
			}
			else {//Lingual
				if(_curLocation.ToothNum<=16) {
					yVal=12;
				}
				else {//ToothNum >= 17
					yVal=30;
				}
			}
			gridP.SetNewCell(FirstEmptyPositionX(_curLocation.ToothNum,yVal),yVal);
		}

		///<summary>Moves the cursor to the muco gingival junction row for the current tooth.</summary>
		private void GoToMGJ() {
			int yVal;
			if(_curLocation.Surface==PerioSurface.Facial) {
				if(_curLocation.ToothNum<=16) {
					yVal=6;
				}
				else {//ToothNum >= 17
					yVal=36;
				}
			}
			else {//Lingual
				yVal=27;
			}
			gridP.SetNewCell(FirstEmptyPositionX(_curLocation.ToothNum,yVal),yVal);
		}

		///<summary>Returns the first non-empty cell for the tooth.</summary>
		private int FirstEmptyPositionX(int toothNum,int yVal) {
			int xVal;
			if(_curLocation.Surface==PerioSurface.Facial) {
				if(toothNum<=16) {
					xVal=(toothNum-1)*3+1;
				}
				else {//ToothNum >= 17
					xVal=46-(toothNum-17)*3;
				}
				if(string.IsNullOrEmpty(gridP.GetCellText(xVal,yVal))) {
					return xVal;
				}
				xVal++;
				if(string.IsNullOrEmpty(gridP.GetCellText(xVal,yVal))) {
					return xVal;
				}
				return ++xVal;
			}
			else {//Lingual
				if(toothNum<=16) {
					xVal=(toothNum-1)*3+3;
				}
				else {//ToothNum >= 17
					xVal=48-(toothNum-17)*3;
				}
				if(string.IsNullOrEmpty(gridP.GetCellText(xVal,yVal))) {
					return xVal;
				}
				xVal--;
				if(string.IsNullOrEmpty(gridP.GetCellText(xVal,yVal))) {
					return xVal;
				}
				return --xVal;
			}
		}
		
		/// <summary>Used to force focus to the hidden textbox when showing this form.</summary>
		private void FormPerio_Shown(object sender,EventArgs e) {
			textInputBox.Focus();//This cannot go into load because focus must come after window has been shown.
		}

		///<summary>After this method runs, the selected index is usually set.</summary>
		private void RefreshListExams(){
			//most recent date at the bottom
			PerioExams.Refresh(PatCur.PatNum);
			PerioMeasures.Refresh(PatCur.PatNum,PerioExams.ListExams);
			listExams.Items.Clear();
			for(int i=0;i<PerioExams.ListExams.Count;i++) {
				listExams.Items.Add(PerioExams.ListExams[i].ExamDate.ToShortDateString()+"   "
					+Providers.GetAbbr(PerioExams.ListExams[i].ProvNum));
			}
		}

		///<summary>Orion only.</summary>
		private void RefreshListPlaque() {
			PerioExams.Refresh(PatCur.PatNum);
			PerioMeasures.Refresh(PatCur.PatNum,PerioExams.ListExams);
			listPlaqueHistory.Items.Clear();
			for(int i=0;i<PerioExams.ListExams.Count;i++) {
				string ph="";
				ph=PerioExams.ListExams[i].ExamDate.ToShortDateString()+"\t";
				gridP.SelectedExam=i;
				gridP.LoadData();
				ph+=gridP.ComputeOrionPlaqueIndex();
				listPlaqueHistory.Items.Add(ph);
			}
			//Not sure if necessary but set it back to what it was
			gridP.SelectedExam=listExams.SelectedIndex;
		}

		///<summary>Usually set the selected index first</summary>
		private void FillGrid(bool doSelectCell=true) {
			if(listExams.SelectedIndex!=-1){
				gridP.perioEdit=true;
				if(!Security.IsAuthorized(Permissions.PerioEdit,PerioExams.ListExams[listExams.SelectedIndex].ExamDate,true)) {
					gridP.perioEdit=false;
				}
				PerioExamCur=PerioExams.ListExams[listExams.SelectedIndex];
			}
			gridP.SelectedExam=listExams.SelectedIndex;
			gridP.DoShowCurrentExamOnly=checkShowCurrent.Checked;
			gridP.LoadData(doSelectCell);
			FillIndexes();
			FillCounts();
			gridP.Invalidate();
			gridP.Focus();//this still doesn't seem to work to enable first arrow click to move
		}

		private void listExams_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			//textInputBox has already gained focus before this so the selected index remains unchanged.
			if(e.Button==MouseButtons.Left) {
				try {
					listExams.SelectedIndex=e.Y/listExams.ItemHeight+listExams.TopIndex;
				}
				catch {
					return;//User might have clicked on some white space thus not selecting a valid item.
				}
			}
			if(listExams.SelectedIndex==gridP.SelectedExam)
				return;
			//Only continues if clicked on other than current exam
			gridP.SaveCurExam(PerioExamCur);
			//no need to RefreshListExams because it has not changed
			PerioExams.Refresh(PatCur.PatNum);//refresh instead
			PerioMeasures.Refresh(PatCur.PatNum,PerioExams.ListExams);
			FillGrid();
		}

		private void listExams_DoubleClick(object sender, System.EventArgs e) {
			//remember that the first click may not have triggered the mouse down routine
			//and the second click will never trigger it.
			if(listExams.SelectedIndex==-1) {
				return;
			}
			if(!Security.IsAuthorized(Permissions.PerioEdit,PerioExams.ListExams[listExams.SelectedIndex].ExamDate)) {
				return;
			}
			//a PerioExam.Cur will always have been set through mousedown(or similar),then FillGrid
			gridP.SaveCurExam(PerioExamCur);
			PerioExams.Refresh(PatCur.PatNum);//list will not change
			PerioMeasures.Refresh(PatCur.PatNum,PerioExams.ListExams);
			FormPerioEdit FormPE=new FormPerioEdit();
			FormPE.PerioExamCur=PerioExamCur;
			FormPE.ShowDialog();
			int curIndex=listExams.SelectedIndex;
			RefreshListExams();
			listExams.SelectedIndex=curIndex;
			FillGrid();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.PerioEdit,MiscData.GetNowDateTime())){
				return;
			}
			if(listExams.SelectedIndex!=-1){
				gridP.SaveCurExam(PerioExamCur);
			}
			CreateNewPerioChart();
			RefreshListExams();
			listExams.SelectedIndex=PerioExams.ListExams.Count-1;
			FillGrid();
			SecurityLogs.MakeLogEntry(Permissions.PerioEdit,PatCur.PatNum,"Perio exam created");
		}

		///<summary>Creates a new perio chart and marks any teeth missing as necessary.</summary>
		private void CreateNewPerioChart() {
			PerioExamCur=new PerioExam();
			PerioExamCur.PatNum=PatCur.PatNum;
			PerioExamCur.ExamDate=DateTimeOD.Today;
			PerioExamCur.ProvNum=PatCur.PriProv;
			PerioExamCur.DateTMeasureEdit=MiscData.GetNowDateTime();
			PerioExams.Insert(PerioExamCur);
			List<int> listSkippedTeeth=new List<int>();//int 1-32
			if(PerioExams.ListExams.Count > 0) {
				//set skipped teeth based on the last exam in the list: 
				listSkippedTeeth=PerioMeasures.GetSkipped(PerioExams.ListExams[PerioExams.ListExams.Count-1].PerioExamNum);
			}
			//For patient's first perio chart, any teeth marked missing are automatically marked skipped.
			if(PerioExams.ListExams.Count==0 || PrefC.GetBool(PrefName.PerioSkipMissingTeeth)) {
				for(int i=0;i<_listMissingTeeth.Count;i++) {
					if(_listMissingTeeth[i].CompareTo("A") >= 0 && _listMissingTeeth[i].CompareTo("Z") <= 0) {//if is a letter (not a number)
						continue;//Skipped teeth are only recorded by tooth number within the perio exam.
					}
					int toothNum=PIn.Int(_listMissingTeeth[i]);
					//Check if this tooth has had an implant done AND the office has the preference to SHOW implants
					if(PrefC.GetBool(PrefName.PerioTreatImplantsAsNotMissing) && ContrPerio.IsImplant(toothNum)) {
						listSkippedTeeth.RemoveAll(x => x==toothNum);//Remove the tooth from the list of skipped teeth if it exists.
						continue;//We do note want to add it back to the list below.
					}
					//This tooth is missing and we know it is not an implant OR the office has the preference to ignore implants.
					//Simply add it to our list of skipped teeth.
					listSkippedTeeth.Add(toothNum);
				}
			}
			PerioMeasures.SetSkipped(PerioExamCur.PerioExamNum,listSkippedTeeth);
		}

		private void butCopyPrevious_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.PerioEdit,MiscData.GetNowDateTime())) {
				return;
			}
			if(listExams.SelectedIndex==-1) {
				MsgBox.Show(this,"There are currently no exams to copy from.  Please create an initial exam.");
				return;
			}
			gridP.SaveCurExam(PerioExamCur);
			CreateNewPerioChart();
			//get meaures from last exam
			List<PerioMeasure> listPerio=PerioMeasures.GetAllForExam(PerioExams.ListExams[PerioExams.ListExams.Count-1].PerioExamNum);
			for(int i=0;i<listPerio.Count;i++) { //add all of the previous exam's measures to this perio exam.
				listPerio[i].PerioExamNum=PerioExamCur.PerioExamNum;
				PerioMeasures.Insert(listPerio[i]);
			}
			RefreshListExams();
			listExams.SelectedIndex=PerioExams.ListExams.Count-1; //select the exam that was just inserted.
			FillGrid();
			SecurityLogs.MakeLogEntry(Permissions.PerioEdit,PatCur.PatNum,"Perio exam copied.");
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(listExams.SelectedIndex==-1){
				MessageBox.Show(Lan.g(this,"Please select an item first."));
				return;
			}
			if(!Security.IsAuthorized(Permissions.PerioEdit,PerioExams.ListExams[listExams.SelectedIndex].ExamDate)) {
				return;
			}
			if(MessageBox.Show(Lan.g(this,"Delete Exam?"),"",MessageBoxButtons.OKCancel)!=DialogResult.OK){
				return;
			}
			int curselected=listExams.SelectedIndex;
			PerioExams.Delete(PerioExamCur);
			RefreshListExams();
			if(curselected < listExams.Items.Count)
				listExams.SelectedIndex=curselected;
			else
				listExams.SelectedIndex=PerioExams.ListExams.Count-1;
			FillGrid();
			SecurityLogs.MakeLogEntry(Permissions.PerioEdit,PatCur.PatNum,"Perio exam deleted");
		}

		private void butListen_Click(object sender,EventArgs e) {	
			if(_voiceController!=null && _voiceController.IsListening) {
				_voiceController.StopListening();
				labelListening.Visible=false;
				return;
			}
			try {
				if(_voiceController==null) {
					_voiceController=new VoiceController(VoiceCommandArea.PerioChart);
					_voiceController.SpeechRecognized+=VoiceController_SpeechRecognized;
				}
				_voiceController.StartListening();
				labelListening.Visible=true;
			}
			catch(Exception ex) {
				FriendlyException.Show(Lan.g(this,"Unable to initialize audio input. Try plugging a different microphone into the computer."),ex);
			}
		}

		private void checkShowCurrent_Click(object sender,EventArgs e) {
			if(listExams.SelectedIndex==-1) {
				return;
			}
			gridP.SaveCurExam(PerioExamCur);
			PerioExams.Refresh(PatCur.PatNum);
			PerioMeasures.Refresh(PatCur.PatNum,PerioExams.ListExams);
			FillGrid();
		}

		private void radioRight_Click(object sender, System.EventArgs e) {
			gridP.DirectionIsRight=true;
			gridP.Focus();
		}

		private void radioLeft_Click(object sender, System.EventArgs e) {
			gridP.DirectionIsRight=false;
			gridP.Focus();
		}

		private void gridP_DirectionChangedRight(object sender, System.EventArgs e) {
			radioRight.Checked=true;
		}

		private void gridP_DirectionChangedLeft(object sender, System.EventArgs e) {
			radioLeft.Checked=true;
		}

		private void checkThree_Click(object sender, System.EventArgs e) {
			gridP.ThreeAtATime=checkThree.Checked;
		}
		
		private void but0_Click(object sender, System.EventArgs e) {
			NumberClicked(0);
		}

		private void but1_Click(object sender, System.EventArgs e) {
			NumberClicked(1);
		}

		private void but2_Click(object sender, System.EventArgs e) {
			NumberClicked(2);
		}

		private void but3_Click(object sender, System.EventArgs e) {
			NumberClicked(3);
		}

		private void but4_Click(object sender, System.EventArgs e) {
			NumberClicked(4);
		}

		private void but5_Click(object sender, System.EventArgs e) {
			NumberClicked(5);
		}

		private void but6_Click(object sender, System.EventArgs e) {
			NumberClicked(6);
		}

		private void but7_Click(object sender, System.EventArgs e) {
			NumberClicked(7);
		}

		private void but8_Click(object sender, System.EventArgs e) {
			NumberClicked(8);
		}

		private void but9_Click(object sender, System.EventArgs e) {
			NumberClicked(9);
		}

		///<summary>The only valid numbers are 0 through 9</summary>
		private void NumberClicked(int number){
			if(gridP.SelectedExam==-1) {
				MessageBox.Show(Lan.g(this,"Please add or select an exam first in the list to the left."));
				return;
			}
			if(TenIsDown) {
				gridP.ButtonPressed(10+number);
			}
			else {
				gridP.ButtonPressed(number);
			}
			TenIsDown=false;
			gridP.Focus();
		}

		private void but10_Click(object sender, System.EventArgs e) {
			TenIsDown=true;
		}

		private void butCalcIndex_Click(object sender, System.EventArgs e) {
			FillIndexes();
			if(listPlaqueHistory.Visible) {
				gridP.SaveCurExam(PerioExamCur);
				RefreshListPlaque();
				FillGrid();
			}
		}

		private void FillIndexes(){
			textIndexPlaque.Text=gridP.ComputeIndex(BleedingFlags.Plaque);
			textIndexCalculus.Text=gridP.ComputeIndex(BleedingFlags.Calculus);
			textIndexBleeding.Text=gridP.ComputeIndex(BleedingFlags.Blood);
			textIndexSupp.Text=gridP.ComputeIndex(BleedingFlags.Suppuration);
		}

		private void butBleed_Click(object sender, System.EventArgs e) {
			TenIsDown=false;
			gridP.ButtonPressed("b");
			gridP.Focus();
		}

		private void butPus_Click(object sender, System.EventArgs e) {
			TenIsDown=false;
			gridP.ButtonPressed("s");
			gridP.Focus();
		}

		private void butPlaque_Click(object sender, System.EventArgs e) {
			TenIsDown=false;
			gridP.ButtonPressed("p");
			gridP.Focus();
		}

		private void butCalculus_Click(object sender, System.EventArgs e) {
			TenIsDown=false;
			gridP.ButtonPressed("c");
			gridP.Focus();
		}

		private void butColorBleed_Click(object sender, System.EventArgs e) {
			colorDialog1.Color=butColorBleed.BackColor;
			if(colorDialog1.ShowDialog()!=DialogResult.OK){
				return;
			}
			butColorBleed.BackColor=colorDialog1.Color;
			Def DefCur=_listMiscColorDefs[1].Copy();
			DefCur.ItemColor=colorDialog1.Color;
			Defs.Update(DefCur);
			Cache.Refresh(InvalidType.Defs);
			localDefsChanged=true;
			gridP.SetColors();
			gridP.Invalidate();
			gridP.Focus();
		}

		private void butColorPus_Click(object sender, System.EventArgs e) {
			colorDialog1.Color=butColorPus.BackColor;
			if(colorDialog1.ShowDialog()!=DialogResult.OK){
				return;
			}
			butColorPus.BackColor=colorDialog1.Color;
			Def DefCur=_listMiscColorDefs[2].Copy();
			DefCur.ItemColor=colorDialog1.Color;
			Defs.Update(DefCur);
			Cache.Refresh(InvalidType.Defs);
			localDefsChanged=true;
			gridP.SetColors();
			gridP.Invalidate();
			gridP.Focus();
		}

		private void butColorPlaque_Click(object sender, System.EventArgs e) {
			colorDialog1.Color=butColorPlaque.BackColor;
			if(colorDialog1.ShowDialog()!=DialogResult.OK){
				return;
			}
			butColorPlaque.BackColor=colorDialog1.Color;
			Def DefCur=_listMiscColorDefs[4].Copy();
			DefCur.ItemColor=colorDialog1.Color;
			Defs.Update(DefCur);
			Cache.Refresh(InvalidType.Defs);
			localDefsChanged=true;
			gridP.SetColors();
			gridP.Invalidate();
			gridP.Focus();
		}

		private void butColorCalculus_Click(object sender, System.EventArgs e) {
			colorDialog1.Color=butColorCalculus.BackColor;
			if(colorDialog1.ShowDialog()!=DialogResult.OK){
				return;
			}
			butColorCalculus.BackColor=colorDialog1.Color;
			Def DefCur=_listMiscColorDefs[5].Copy();
			DefCur.ItemColor=colorDialog1.Color;
			Defs.Update(DefCur);
			Cache.Refresh(InvalidType.Defs);
			localDefsChanged=true;
			gridP.SetColors();
			gridP.Invalidate();
			gridP.Focus();
		}

		private void butSkip_Click(object sender, System.EventArgs e) {
			if(listExams.SelectedIndex<0){//PerioExamCur could still be set to a deleted exam and would not be null even if there is no exam.
				MessageBox.Show(Lan.g(this,"Please select an exam first."));
				return;
			}
			gridP.ToggleSkip(PerioExamCur.PerioExamNum);
		}

		private void updownRed_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			//this is necessary because Microsoft's updown control is too buggy to be useful
			Cursor=Cursors.WaitCursor;
			PrefName prefname=PrefName.PerioRedProb;
			if(sender==updownProb){
				prefname=PrefName.PerioRedProb;
			}
			else if(sender==updownMGJ) {
				prefname=PrefName.PerioRedMGJ;
			}
			else if(sender==updownGing) {
				prefname=PrefName.PerioRedGing;
			}
			else if(sender==updownCAL) {
				prefname=PrefName.PerioRedCAL;
			}
			else if(sender==updownFurc) {
				prefname=PrefName.PerioRedFurc;
			}
			else if(sender==updownMob) {
				prefname=PrefName.PerioRedMob;
			}
			int currentValue=PrefC.GetInt(prefname);
			if(e.Y<8){//up
				currentValue++;
			}
			else{//down
				if(currentValue==0){
					Cursor=Cursors.Default;
					return;
				}
				currentValue--;
			}
			Prefs.UpdateLong(prefname,currentValue);
			//pref.ValueString=currentValue.ToString();
			//Prefs.Update(pref);
			localDefsChanged=true;
			Cache.Refresh(InvalidType.Prefs);
			if(sender==updownProb){
				textRedProb.Text=currentValue.ToString();
				textCountProb.Text=gridP.CountTeeth(PerioSequenceType.Probing).Count.ToString();
			}
			else if(sender==updownMGJ){
				textRedMGJ.Text=currentValue.ToString();
				textCountMGJ.Text=gridP.CountTeeth(PerioSequenceType.MGJ).Count.ToString();
			}
			else if(sender==updownGing){
				textRedGing.Text=currentValue.ToString();
				textCountGing.Text=gridP.CountTeeth(PerioSequenceType.GingMargin).Count.ToString();
			}
			else if(sender==updownCAL){
				textRedCAL.Text=currentValue.ToString();
				textCountCAL.Text=gridP.CountTeeth(PerioSequenceType.CAL).Count.ToString();
			}
			else if(sender==updownFurc){
				textRedFurc.Text=currentValue.ToString();
				textCountFurc.Text=gridP.CountTeeth(PerioSequenceType.Furcation).Count.ToString();
			}
			else if(sender==updownMob){
				textRedMob.Text=currentValue.ToString();
				textCountMob.Text=gridP.CountTeeth(PerioSequenceType.Mobility).Count.ToString();
			}
			gridP.Invalidate();
			Cursor=Cursors.Default;
			gridP.Focus();
		}

		private void butCount_Click(object sender, System.EventArgs e) {
			FillCounts();
			gridP.Focus();
		}

		private void FillCounts(){
			textCountProb.Text=gridP.CountTeeth(PerioSequenceType.Probing).Count.ToString();
			textCountMGJ.Text=gridP.CountTeeth(PerioSequenceType.MGJ).Count.ToString();
			textCountGing.Text=gridP.CountTeeth(PerioSequenceType.GingMargin).Count.ToString();
			textCountCAL.Text=gridP.CountTeeth(PerioSequenceType.CAL).Count.ToString();
			textCountFurc.Text=gridP.CountTeeth(PerioSequenceType.Furcation).Count.ToString();
			textCountMob.Text=gridP.CountTeeth(PerioSequenceType.Mobility).Count.ToString();
		}

		private void butPrint_Click(object sender, System.EventArgs e) {
			if(this.listExams.SelectedIndex<0) {
				MsgBox.Show(this,"Please select an exam first.");
				return;
			}
			pd2=new PrintDocument();
			pd2.PrintPage+=new PrintPageEventHandler(this.pd2_PrintPage);
			pd2.OriginAtMargins=true;
			pd2.DefaultPageSettings.Margins=new Margins(0,0,0,0);
			if(!PrinterL.SetPrinter(pd2,PrintSituation.TPPerio,PatCur.PatNum,"Perio chart from "+PerioExamCur.ExamDate+" printed")){
				return;
			}
			try{
				pd2.Print();
			}
			catch{
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
			gridP.Focus();
		}

		private void butSave_Click(object sender,EventArgs e) {
			if(this.listExams.SelectedIndex<0){
				MessageBox.Show(Lan.g(this,"Please select an exam first."));
				return;
			}
			gridP.SaveCurExam(PerioExamCur);
			PerioExams.Refresh(PatCur.PatNum);
			PerioMeasures.Refresh(PatCur.PatNum,PerioExams.ListExams);
			FillGrid();
			Bitmap gridImage=null;
			Bitmap perioPrintImage=null;
			Graphics g=null;
			//Document doc=new Document();
			//try {
			perioPrintImage=new Bitmap(750,1000);
			perioPrintImage.SetResolution(96,96);
			g=Graphics.FromImage(perioPrintImage);
			g.Clear(Color.White);
			g.CompositingQuality=System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			g.InterpolationMode=System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			g.TextRenderingHint=System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			string clinicName="";
			//This clinic name could be more accurate here in the future if we make perio exams clinic specific.
			//Perhaps if there were a perioexam.ClinicNum column.
			if(PatCur.ClinicNum!=0) {
				Clinic clinic=Clinics.GetClinic(PatCur.ClinicNum);
				clinicName=clinic.Description;
			} 
			else {
				clinicName=PrefC.GetString(PrefName.PracticeTitle);
			}
			float y=50f;
			SizeF m;
			Font font=new Font("Arial",15);
			string titleStr="PERIODONTAL EXAMINATION";
			m=g.MeasureString(titleStr,font);
			g.DrawString(titleStr,font,Brushes.Black,new PointF(perioPrintImage.Width/2f-m.Width/2f,y));
			y+=m.Height;
			font=new Font("Arial",11);
			m=g.MeasureString(clinicName,font);
			g.DrawString(clinicName,font,Brushes.Black,
				new PointF(perioPrintImage.Width/2f-m.Width/2f,y));
			y+=m.Height;
			string patNameStr=PatCur.GetNameFLFormal();
			m=g.MeasureString(patNameStr,font);
			g.DrawString(patNameStr,font,Brushes.Black,new PointF(perioPrintImage.Width/2f-m.Width/2f,y));
			y+=m.Height;
			DateTime serverTimeNow=MiscData.GetNowDateTime();
			string timeNowStr=serverTimeNow.ToShortDateString();//Locale specific date.
			m=g.MeasureString(timeNowStr,font);
			g.DrawString(timeNowStr,font,Brushes.Black,new PointF(perioPrintImage.Width/2f-m.Width/2f,y));
			y+=m.Height;
			gridImage=new Bitmap(gridP.Width,gridP.Height);
			gridP.DrawToBitmap(gridImage,new Rectangle(0,0,gridImage.Width,gridImage.Height));
			g.DrawImageUnscaled(gridImage,(int)((perioPrintImage.Width-gridImage.Width)/2f),(int)y);
			long defNumCategory=Defs.GetImageCat(ImageCategorySpecial.T);
			if(defNumCategory==0) {
				MsgBox.Show(this,"No image category set for tooth charts in definitions.");
				perioPrintImage.Dispose();
				gridImage.Dispose();
				perioPrintImage=null;
				gridImage=null;
				g.Dispose();
				return;
			}
			try {
				ImageStore.Import(perioPrintImage,defNumCategory,ImageType.Photo,PatCur);
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Unable to save file. ") + ex.Message);
				perioPrintImage.Dispose();
				gridImage.Dispose();
				perioPrintImage=null;
				gridImage=null;
				g.Dispose();
				return;
			}
			MsgBox.Show(this,"Saved.");
			/*
			string patImagePath=ImageStore.GetPatientFolder(PatCur);
			string filePath="";
			do {
				doc.DateCreated=MiscData.GetNowDateTime();
				doc.FileName="perioexam_"+doc.DateCreated.ToString("yyyy_MM_dd_hh_mm_ss")+".png";
				filePath=ODFileUtils.CombinePaths(patImagePath,doc.FileName);
			} while(File.Exists(filePath));//if a file with this name already exists, then it will stay in this loop
			doc.PatNum=PatCur.PatNum;
			doc.ImgType=ImageType.Photo;
			doc.DocCategory=Defs.GetByExactName(DefCat.ImageCats,"Tooth Charts");
			doc.Description="Perio Exam";
			Documents.Insert(doc,PatCur);
			docCreated=true;
			perioPrintImage.Save(filePath,System.Drawing.Imaging.ImageFormat.Png);
			MessageBox.Show(Lan.g(this,"Image saved."));*/
			/*} 
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Image failed to save: "+Environment.NewLine+ex.ToString()));
				if(docCreated) {
					Documents.Delete(doc);
				}
			} 
			finally {
				if(gridImage!=null){
					gridImage.Dispose();
				}
				if(g!=null) {
					g.Dispose();
					g=null;
				}
				if(perioPrintImage!=null) {
					perioPrintImage.Dispose();
					perioPrintImage=null;
				}
			}*/
			perioPrintImage.Dispose();
			gridImage.Dispose();
			perioPrintImage=null;
			gridImage=null;
			g.Dispose();
		}

		private void pd2_PrintPage(object sender, PrintPageEventArgs ev){//raised for each page to be printed.
			Graphics grfx=ev.Graphics;
			//MessageBox.Show(grfx.
			float yPos=67+25+20+20+6;
			float xPos=100;
			grfx.TranslateTransform(xPos,yPos);
			gridP.DrawChart(grfx);//have to print graphics first, or they cover up title.
			grfx.DrawString("F",new Font("Arial",15),Brushes.Black,new Point(-26,92));
			grfx.DrawString("L",new Font("Arial",15),Brushes.Black,new Point(-26,212));
			grfx.DrawString("L",new Font("Arial",15),Brushes.Black,new Point(-26,416));
			grfx.DrawString("F",new Font("Arial",15),Brushes.Black,new Point(-26,552));
			grfx.TranslateTransform(-xPos,-yPos);
			yPos=67;
			xPos=100;
			Font font=new Font("Arial",9);
			StringFormat format=new StringFormat();
			format.Alignment=StringAlignment.Center;
			grfx.DrawString("Periodontal Charting",new Font("Arial",15),Brushes.Black,
				new RectangleF(xPos,yPos,650,25),format);			
			yPos+=25;
			grfx.DrawString(PrefC.GetString(PrefName.PracticeTitle),new Font("Arial",11),Brushes.Black
				,new RectangleF(xPos,yPos,650,25),format);
			yPos+=20;
			grfx.DrawString(PatCur.GetNameFL(),new Font("Arial",11),Brushes.Black
				,new RectangleF(xPos,yPos,650,25),format);
			yPos+=20;
			//grfx.TranslateTransform(xPos,yPos);
			//gridP.DrawChart(grfx);
			//grfx.TranslateTransform(-xPos,-yPos);
			yPos+=688;
			grfx.FillEllipse(new SolidBrush(butColorPlaque.BackColor),xPos,yPos+3,8,8);
			grfx.DrawString("Plaque Index: "+gridP.ComputeIndex(BleedingFlags.Plaque)+" %"
				,font,Brushes.Black,xPos+12,yPos);
			yPos+=20;
			grfx.FillEllipse(new SolidBrush(butColorCalculus.BackColor),xPos,yPos+3,8,8);
			grfx.DrawString("Calculus Index: "+gridP.ComputeIndex(BleedingFlags.Calculus)+" %"
				,font,Brushes.Black,xPos+12,yPos);
			yPos+=20;
			grfx.FillEllipse(new SolidBrush(butColorBleed.BackColor),xPos,yPos+3,8,8);
			grfx.DrawString("Bleeding Index: "+gridP.ComputeIndex(BleedingFlags.Blood)+" %"
				,font,Brushes.Black,xPos+12,yPos);
			yPos+=20;
			grfx.FillEllipse(new SolidBrush(butColorPus.BackColor),xPos,yPos+3,8,8);
			grfx.DrawString("Suppuration Index: "+gridP.ComputeIndex(BleedingFlags.Suppuration)+" %"
				,font,Brushes.Black,xPos+12,yPos);
			yPos+=20;
			grfx.DrawString("Teeth with Probing greater than or equal to "+textRedProb.Text+" mm: "
				+ConvertALtoString(gridP.CountTeeth(PerioSequenceType.Probing))
				,font,Brushes.Black,xPos,yPos);
			yPos+=20;
			grfx.DrawString("Teeth with MGJ less than or equal to "+textRedMGJ.Text+" mm: "
				+ConvertALtoString(gridP.CountTeeth(PerioSequenceType.MGJ))
				,font,Brushes.Black,xPos,yPos);
			yPos+=20;
			grfx.DrawString("Teeth with Gingival Margin greater than or equal to "+textRedGing.Text+" mm: "
				+ConvertALtoString(gridP.CountTeeth(PerioSequenceType.GingMargin))
				,font,Brushes.Black,xPos,yPos);
			yPos+=20;
			grfx.DrawString("Teeth with CAL greater than or equal to "+textRedCAL.Text+" mm: "
				+ConvertALtoString(gridP.CountTeeth(PerioSequenceType.CAL))
				,font,Brushes.Black,xPos,yPos);
			yPos+=20;
			grfx.DrawString("Teeth with Furcations greater than or equal to class "+textRedFurc.Text+": "
				+ConvertALtoString(gridP.CountTeeth(PerioSequenceType.Furcation))
				,font,Brushes.Black,xPos,yPos);
			yPos+=20;
			grfx.DrawString("Teeth with Mobility greater than or equal to "+textRedMob.Text+": "
				+ConvertALtoString(gridP.CountTeeth(PerioSequenceType.Mobility))
				,font,Brushes.Black,xPos,yPos);
			//pagesPrinted++;
			ev.HasMorePages=false;
			grfx.Dispose();
		}

		private string ConvertALtoString(ArrayList ALteeth){
			if(ALteeth.Count==0){
				return "none";
			}
			string retVal="";
			for(int i=0;i<ALteeth.Count;i++){
				if(i>0)
					retVal+=",";
				retVal+=ALteeth[i];
			}
			return retVal;
		}

		private void butGraphical_Click(object sender,EventArgs e) {
			if(ComputerPrefs.LocalComputer.GraphicsSimple!=DrawingMode.DirectX) {
				MsgBox.Show(this,"In the Graphics setup window, you must first select DirectX.");
				return;
			}
			if(listExams.SelectedIndex==-1) {
				MsgBox.Show(this,"Exam must be selected first.");
				return;
			}
			if(localDefsChanged) {
				DataValid.SetInvalid(InvalidType.Defs,InvalidType.Prefs);
			}
			//if(listExams.SelectedIndex!=-1) {
			gridP.SaveCurExam(PerioExamCur);
			PerioExams.Refresh(PatCur.PatNum);//refresh instead
			PerioMeasures.Refresh(PatCur.PatNum,PerioExams.ListExams);
			FillGrid();
			FormPerioGraphical formg=new FormPerioGraphical(PerioExams.ListExams[listExams.SelectedIndex],PatCur,_toothChartData);
			formg.ShowDialog();
			formg.Dispose();
		}

		private void checkGingMarg_CheckedChanged(object sender,EventArgs e) {
			gridP.GingMargPlus=checkGingMarg.Checked;
			gridP.Focus();
		}

		///<summary>This ensures that the textbox will always have focus when using FormPerio.</summary>
		private void textInputBox_Leave(object sender,EventArgs e) {
			textInputBox.Focus();
		}

		///<summary>Catches any non-alphanumeric keypresses so that they can be passed into the grid separately.</summary>
		private void textInputBox_KeyDown(object sender,KeyEventArgs e) {
			if(e.KeyCode==Keys.Left
				|| e.KeyCode==Keys.Right
				|| e.KeyCode==Keys.Up
				|| e.KeyCode==Keys.Down
				|| e.KeyCode==Keys.Delete
				|| e.KeyCode==Keys.Back
				|| e.Modifiers==Keys.Control) 
			{
				gridP.KeyPressed(e);
			}
		}

		///<summary>Used to force buttons to be pressed as characters are typed/dictated/pasted into the textbox.</summary>
		private void textInputBox_TextChanged(object sender,EventArgs e) {
			Char[] arrInputChars=textInputBox.Text.ToLower().ToCharArray();
			for(int i=0;i<arrInputChars.Length;i++) {
				if(arrInputChars[i]>=48 && arrInputChars[i]<=57) {
					NumberClicked(arrInputChars[i]-48);
				}
				else if(arrInputChars[i]=='b'
					|| arrInputChars[i]=='c'
					|| arrInputChars[i]=='s'
					|| arrInputChars[i]=='p') 
				{
					gridP.ButtonPressed(arrInputChars[i].ToString());
				}
				else if(arrInputChars[i]==' ') {
					gridP.ButtonPressed("b");
				}
			}
			textInputBox.Clear();
		}

		private void butClose_Click(object sender,System.EventArgs e) {
			if(_userPrefCurrentOnly==null) {
				UserOdPrefs.Insert(new UserOdPref() {
					UserNum=Security.CurUser.UserNum,
					FkeyType=UserOdFkeyType.PerioCurrentExamOnly,
					ValueString=POut.Bool(checkShowCurrent.Checked)
				});
			}
			else {
				if(_userPrefCurrentOnly.ValueString != POut.Bool(checkShowCurrent.Checked)) {//The user preference has changed.
					_userPrefCurrentOnly.ValueString=POut.Bool(checkShowCurrent.Checked);
					UserOdPrefs.Update(_userPrefCurrentOnly);
				}
			}
			Close();
		}

		private void FormPerio_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(localDefsChanged){
				DataValid.SetInvalid(InvalidType.Defs, InvalidType.Prefs);
			}
			if(listExams.SelectedIndex!=-1){
				gridP.SaveCurExam(PerioExamCur);
			}
			_voiceController?.Dispose();
			Plugins.HookAddCode(this,"FormPerio.Closing_end");
		}
	}
}





















