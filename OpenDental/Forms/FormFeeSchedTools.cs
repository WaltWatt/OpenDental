using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormFeeSchedTools :ODForm {
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button butCopy;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.ComboBox comboFeeSchedTo;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox groupBox2;
		private OpenDental.UI.Button butClear;
		private System.Windows.Forms.GroupBox groupBox3;
		private OpenDental.UI.Button butIncrease;
		private System.Windows.Forms.TextBox textPercent;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.RadioButton radioDollar;
		private System.Windows.Forms.RadioButton radioDime;
		private System.Windows.Forms.RadioButton radioPenny;
		private GroupBox groupBox5;
		private OpenDental.UI.Button butExport;
		private OpenDental.UI.Button butImport;
		private GroupBox groupBox6;
		private Label label4;
		private OpenDental.UI.Button butUpdate;
		private Label label5;
		private UI.Button butImportCanada;
		private GroupBox groupBox7;
		private ComboBox comboProvider;
		private ComboBox comboClinic;
		private ComboBox comboFeeSched;
		///<summary>The defNum of the fee schedule that is currently displayed in the main window.</summary>
		private long _schedNum;
		private List<FeeSched> _listFeeScheds;
		private List<Provider> _listProvs;
		private List<Clinic> _listClinics;
		private Label label12;
		private Label label10;
		private Label label8;
		private UI.Button butPickProv;
		private UI.Button butPickClinic;
		private UI.Button butPickSched;
		private Label label7;
		private Label label6;
		private UI.Button butPickProvTo;
		private ComboBox comboProviderTo;
		private UI.Button butPickClinicTo;
		private ComboBox comboClinicTo;
		private UI.Button butPickSchedTo;
		///<summary>Currently accurate list of fees from the ProcCodes window.
		///This list will get synced to the database if DialogResult==DialogResult.OK and _changed==true</summary>
		private FeeCache _feeCache;
		///<summary>Set to true if _listFees ever changes and needs to get synced to the database.</summary>
		private bool _changed=false;
		private Label labelClinic;
		private GroupBox groupBox8;
		private Label label9;
		private UI.Button butUpdateWriteoffs;
		private UI.ComboBoxClinicMulti comboBoxMultiClinics;
		private UI.Button butOK;
		private Label label11;
		///<summary>Used to keep track of the clinics that have been selected for 'Copy To'.</summary>
		private List<long> _listSelectedClinicNumsTo=new List<long>();

		///<summary>Supply the fee schedule num(DefNum) to which all these changes will apply</summary>
		public FormFeeSchedTools(long schedNum,List<FeeSched> listFeeScheds,List<Provider> listProvs,List<Clinic> listClinics,FeeCache cache) {
			// Required for Windows Form Designer support
			InitializeComponent();
			Lan.F(this);
			_schedNum=schedNum;
			_listFeeScheds=listFeeScheds;
			_feeCache=cache;
			_listProvs=listProvs;
			_listClinics=listClinics;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFeeSchedTools));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.butPickProvTo = new OpenDental.UI.Button();
			this.comboProviderTo = new System.Windows.Forms.ComboBox();
			this.butPickClinicTo = new OpenDental.UI.Button();
			this.comboClinicTo = new System.Windows.Forms.ComboBox();
			this.butPickSchedTo = new OpenDental.UI.Button();
			this.butCopy = new OpenDental.UI.Button();
			this.comboFeeSchedTo = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label11 = new System.Windows.Forms.Label();
			this.butClear = new OpenDental.UI.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.radioPenny = new System.Windows.Forms.RadioButton();
			this.radioDime = new System.Windows.Forms.RadioButton();
			this.radioDollar = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.butIncrease = new OpenDental.UI.Button();
			this.textPercent = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.butImportCanada = new OpenDental.UI.Button();
			this.butImport = new OpenDental.UI.Button();
			this.butExport = new OpenDental.UI.Button();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.comboBoxMultiClinics = new OpenDental.UI.ComboBoxClinicMulti();
			this.labelClinic = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.butUpdate = new OpenDental.UI.Button();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.butPickProv = new OpenDental.UI.Button();
			this.label12 = new System.Windows.Forms.Label();
			this.butPickClinic = new OpenDental.UI.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.butPickSched = new OpenDental.UI.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.comboProvider = new System.Windows.Forms.ComboBox();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.comboFeeSched = new System.Windows.Forms.ComboBox();
			this.butCancel = new OpenDental.UI.Button();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.label9 = new System.Windows.Forms.Label();
			this.butUpdateWriteoffs = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.butPickProvTo);
			this.groupBox1.Controls.Add(this.comboProviderTo);
			this.groupBox1.Controls.Add(this.butPickClinicTo);
			this.groupBox1.Controls.Add(this.comboClinicTo);
			this.groupBox1.Controls.Add(this.butPickSchedTo);
			this.groupBox1.Controls.Add(this.butCopy);
			this.groupBox1.Controls.Add(this.comboFeeSchedTo);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(12, 108);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(205, 186);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Copy To";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(10, 97);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(100, 16);
			this.label7.TabIndex = 39;
			this.label7.Text = "Provider";
			this.label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(10, 58);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 16);
			this.label6.TabIndex = 38;
			this.label6.Text = "Clinic";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butPickProvTo
			// 
			this.butPickProvTo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickProvTo.Autosize = false;
			this.butPickProvTo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickProvTo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickProvTo.CornerRadius = 2F;
			this.butPickProvTo.Location = new System.Drawing.Point(176, 113);
			this.butPickProvTo.Name = "butPickProvTo";
			this.butPickProvTo.Size = new System.Drawing.Size(23, 21);
			this.butPickProvTo.TabIndex = 36;
			this.butPickProvTo.Text = "...";
			this.butPickProvTo.Click += new System.EventHandler(this.butPickProvider_Click);
			// 
			// comboProviderTo
			// 
			this.comboProviderTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProviderTo.FormattingEnabled = true;
			this.comboProviderTo.Location = new System.Drawing.Point(10, 113);
			this.comboProviderTo.Name = "comboProviderTo";
			this.comboProviderTo.Size = new System.Drawing.Size(160, 21);
			this.comboProviderTo.TabIndex = 37;
			// 
			// butPickClinicTo
			// 
			this.butPickClinicTo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickClinicTo.Autosize = false;
			this.butPickClinicTo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickClinicTo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickClinicTo.CornerRadius = 2F;
			this.butPickClinicTo.Location = new System.Drawing.Point(176, 74);
			this.butPickClinicTo.Name = "butPickClinicTo";
			this.butPickClinicTo.Size = new System.Drawing.Size(23, 21);
			this.butPickClinicTo.TabIndex = 36;
			this.butPickClinicTo.Text = "...";
			this.butPickClinicTo.Click += new System.EventHandler(this.butPickClinicTo_Click);
			// 
			// comboClinicTo
			// 
			this.comboClinicTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinicTo.Location = new System.Drawing.Point(10, 75);
			this.comboClinicTo.Name = "comboClinicTo";
			this.comboClinicTo.Size = new System.Drawing.Size(160, 21);
			this.comboClinicTo.TabIndex = 35;
			this.comboClinicTo.SelectionChangeCommitted += new System.EventHandler(this.comboClinicTo_SelectionChangeCommitted);
			// 
			// butPickSchedTo
			// 
			this.butPickSchedTo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickSchedTo.Autosize = false;
			this.butPickSchedTo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickSchedTo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickSchedTo.CornerRadius = 2F;
			this.butPickSchedTo.Location = new System.Drawing.Point(176, 36);
			this.butPickSchedTo.Name = "butPickSchedTo";
			this.butPickSchedTo.Size = new System.Drawing.Size(23, 21);
			this.butPickSchedTo.TabIndex = 34;
			this.butPickSchedTo.Text = "...";
			this.butPickSchedTo.Click += new System.EventHandler(this.butPickFeeSched_Click);
			// 
			// butCopy
			// 
			this.butCopy.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCopy.Autosize = true;
			this.butCopy.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCopy.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCopy.CornerRadius = 4F;
			this.butCopy.Location = new System.Drawing.Point(10, 144);
			this.butCopy.Name = "butCopy";
			this.butCopy.Size = new System.Drawing.Size(75, 24);
			this.butCopy.TabIndex = 4;
			this.butCopy.Text = "Copy";
			this.butCopy.Click += new System.EventHandler(this.butCopy_Click);
			// 
			// comboFeeSchedTo
			// 
			this.comboFeeSchedTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFeeSchedTo.Location = new System.Drawing.Point(11, 36);
			this.comboFeeSchedTo.Name = "comboFeeSchedTo";
			this.comboFeeSchedTo.Size = new System.Drawing.Size(160, 21);
			this.comboFeeSchedTo.TabIndex = 0;
			this.comboFeeSchedTo.SelectionChangeCommitted += new System.EventHandler(this.comboGeneric_SelectionChangeCommitted);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 18);
			this.label1.TabIndex = 3;
			this.label1.Text = "Fee Schedule";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.butClear);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(12, 396);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(205, 79);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Clear";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(10, 16);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(188, 30);
			this.label11.TabIndex = 7;
			this.label11.Text = "Clears all values from selected fee sched for selected prov and clinic";
			// 
			// butClear
			// 
			this.butClear.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClear.Autosize = true;
			this.butClear.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClear.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClear.CornerRadius = 4F;
			this.butClear.Location = new System.Drawing.Point(11, 49);
			this.butClear.Name = "butClear";
			this.butClear.Size = new System.Drawing.Size(75, 24);
			this.butClear.TabIndex = 4;
			this.butClear.Text = "Clear";
			this.butClear.Click += new System.EventHandler(this.butClear_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.groupBox4);
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.Controls.Add(this.butIncrease);
			this.groupBox3.Controls.Add(this.textPercent);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(236, 108);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(205, 168);
			this.groupBox3.TabIndex = 4;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Increase by %";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(92, 142);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(90, 18);
			this.label5.TabIndex = 11;
			this.label5.Text = "(or decrease)";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.radioPenny);
			this.groupBox4.Controls.Add(this.radioDime);
			this.groupBox4.Controls.Add(this.radioDollar);
			this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox4.Location = new System.Drawing.Point(13, 47);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(169, 78);
			this.groupBox4.TabIndex = 10;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Round to nearest";
			// 
			// radioPenny
			// 
			this.radioPenny.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioPenny.Location = new System.Drawing.Point(14, 52);
			this.radioPenny.Name = "radioPenny";
			this.radioPenny.Size = new System.Drawing.Size(104, 17);
			this.radioPenny.TabIndex = 2;
			this.radioPenny.Text = "$.01";
			// 
			// radioDime
			// 
			this.radioDime.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioDime.Location = new System.Drawing.Point(14, 35);
			this.radioDime.Name = "radioDime";
			this.radioDime.Size = new System.Drawing.Size(104, 17);
			this.radioDime.TabIndex = 1;
			this.radioDime.Text = "$.10";
			// 
			// radioDollar
			// 
			this.radioDollar.Checked = true;
			this.radioDollar.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioDollar.Location = new System.Drawing.Point(14, 18);
			this.radioDollar.Name = "radioDollar";
			this.radioDollar.Size = new System.Drawing.Size(104, 17);
			this.radioDollar.TabIndex = 0;
			this.radioDollar.TabStop = true;
			this.radioDollar.Text = "$1";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(92, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(109, 19);
			this.label3.TabIndex = 6;
			this.label3.Text = "for example: 5";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butIncrease
			// 
			this.butIncrease.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butIncrease.Autosize = true;
			this.butIncrease.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butIncrease.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butIncrease.CornerRadius = 4F;
			this.butIncrease.Location = new System.Drawing.Point(11, 135);
			this.butIncrease.Name = "butIncrease";
			this.butIncrease.Size = new System.Drawing.Size(75, 24);
			this.butIncrease.TabIndex = 4;
			this.butIncrease.Text = "Increase";
			this.butIncrease.Click += new System.EventHandler(this.butIncrease_Click);
			// 
			// textPercent
			// 
			this.textPercent.Location = new System.Drawing.Point(42, 23);
			this.textPercent.Name = "textPercent";
			this.textPercent.Size = new System.Drawing.Size(46, 20);
			this.textPercent.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(3, 23);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 19);
			this.label2.TabIndex = 5;
			this.label2.Text = "%";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.butImportCanada);
			this.groupBox5.Controls.Add(this.butImport);
			this.groupBox5.Controls.Add(this.butExport);
			this.groupBox5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox5.Location = new System.Drawing.Point(12, 296);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(205, 84);
			this.groupBox5.TabIndex = 5;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Export/Import";
			// 
			// butImportCanada
			// 
			this.butImportCanada.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butImportCanada.Autosize = true;
			this.butImportCanada.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butImportCanada.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butImportCanada.CornerRadius = 4F;
			this.butImportCanada.Location = new System.Drawing.Point(98, 51);
			this.butImportCanada.Name = "butImportCanada";
			this.butImportCanada.Size = new System.Drawing.Size(84, 24);
			this.butImportCanada.TabIndex = 7;
			this.butImportCanada.Text = "Import Canada";
			this.butImportCanada.Click += new System.EventHandler(this.butImportCanada_Click);
			// 
			// butImport
			// 
			this.butImport.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butImport.Autosize = true;
			this.butImport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butImport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butImport.CornerRadius = 4F;
			this.butImport.Location = new System.Drawing.Point(98, 21);
			this.butImport.Name = "butImport";
			this.butImport.Size = new System.Drawing.Size(84, 24);
			this.butImport.TabIndex = 5;
			this.butImport.Text = "Import";
			this.butImport.Click += new System.EventHandler(this.butImport_Click);
			// 
			// butExport
			// 
			this.butExport.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butExport.Autosize = true;
			this.butExport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butExport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butExport.CornerRadius = 4F;
			this.butExport.Location = new System.Drawing.Point(17, 21);
			this.butExport.Name = "butExport";
			this.butExport.Size = new System.Drawing.Size(75, 24);
			this.butExport.TabIndex = 4;
			this.butExport.Text = "Export";
			this.butExport.Click += new System.EventHandler(this.butExport_Click);
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.comboBoxMultiClinics);
			this.groupBox6.Controls.Add(this.labelClinic);
			this.groupBox6.Controls.Add(this.label4);
			this.groupBox6.Controls.Add(this.butUpdate);
			this.groupBox6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox6.Location = new System.Drawing.Point(236, 279);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(205, 88);
			this.groupBox6.TabIndex = 6;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Global Update Fees";
			// 
			// comboBoxMultiClinics
			// 
			this.comboBoxMultiClinics.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiClinics.Location = new System.Drawing.Point(68, 19);
			this.comboBoxMultiClinics.Name = "comboBoxMultiClinics";
			this.comboBoxMultiClinics.Size = new System.Drawing.Size(131, 21);
			this.comboBoxMultiClinics.TabIndex = 8;
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(4, 22);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(60, 18);
			this.labelClinic.TabIndex = 7;
			this.labelClinic.Text = "Clinic:";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(90, 58);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(91, 18);
			this.label4.TabIndex = 5;
			this.label4.Text = "for all patients";
			// 
			// butUpdate
			// 
			this.butUpdate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUpdate.Autosize = true;
			this.butUpdate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUpdate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUpdate.CornerRadius = 4F;
			this.butUpdate.Location = new System.Drawing.Point(9, 52);
			this.butUpdate.Name = "butUpdate";
			this.butUpdate.Size = new System.Drawing.Size(75, 24);
			this.butUpdate.TabIndex = 4;
			this.butUpdate.Text = "Update";
			this.butUpdate.Click += new System.EventHandler(this.butUpdate_Click);
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.butPickProv);
			this.groupBox7.Controls.Add(this.label12);
			this.groupBox7.Controls.Add(this.butPickClinic);
			this.groupBox7.Controls.Add(this.label10);
			this.groupBox7.Controls.Add(this.butPickSched);
			this.groupBox7.Controls.Add(this.label8);
			this.groupBox7.Controls.Add(this.comboProvider);
			this.groupBox7.Controls.Add(this.comboClinic);
			this.groupBox7.Controls.Add(this.comboFeeSched);
			this.groupBox7.Location = new System.Drawing.Point(49, 6);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(349, 100);
			this.groupBox7.TabIndex = 28;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Choose Settings";
			// 
			// butPickProv
			// 
			this.butPickProv.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickProv.Autosize = false;
			this.butPickProv.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickProv.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickProv.CornerRadius = 2F;
			this.butPickProv.Location = new System.Drawing.Point(297, 72);
			this.butPickProv.Name = "butPickProv";
			this.butPickProv.Size = new System.Drawing.Size(23, 21);
			this.butPickProv.TabIndex = 35;
			this.butPickProv.Text = "...";
			this.butPickProv.Click += new System.EventHandler(this.butPickProvider_Click);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(6, 20);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(111, 17);
			this.label12.TabIndex = 35;
			this.label12.Text = "Fee Schedule";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPickClinic
			// 
			this.butPickClinic.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickClinic.Autosize = false;
			this.butPickClinic.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickClinic.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickClinic.CornerRadius = 2F;
			this.butPickClinic.Location = new System.Drawing.Point(297, 46);
			this.butPickClinic.Name = "butPickClinic";
			this.butPickClinic.Size = new System.Drawing.Size(23, 21);
			this.butPickClinic.TabIndex = 34;
			this.butPickClinic.Text = "...";
			this.butPickClinic.Click += new System.EventHandler(this.butPickClinic_Click);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(27, 47);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(90, 17);
			this.label10.TabIndex = 34;
			this.label10.Text = "Clinic";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPickSched
			// 
			this.butPickSched.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickSched.Autosize = false;
			this.butPickSched.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickSched.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickSched.CornerRadius = 2F;
			this.butPickSched.Location = new System.Drawing.Point(297, 19);
			this.butPickSched.Name = "butPickSched";
			this.butPickSched.Size = new System.Drawing.Size(23, 21);
			this.butPickSched.TabIndex = 33;
			this.butPickSched.Text = "...";
			this.butPickSched.Click += new System.EventHandler(this.butPickFeeSched_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(27, 73);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(90, 17);
			this.label8.TabIndex = 33;
			this.label8.Text = "Provider";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboProvider
			// 
			this.comboProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProvider.FormattingEnabled = true;
			this.comboProvider.Location = new System.Drawing.Point(118, 72);
			this.comboProvider.Name = "comboProvider";
			this.comboProvider.Size = new System.Drawing.Size(174, 21);
			this.comboProvider.TabIndex = 2;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.FormattingEnabled = true;
			this.comboClinic.Location = new System.Drawing.Point(118, 46);
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(174, 21);
			this.comboClinic.TabIndex = 1;
			// 
			// comboFeeSched
			// 
			this.comboFeeSched.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFeeSched.FormattingEnabled = true;
			this.comboFeeSched.Location = new System.Drawing.Point(118, 19);
			this.comboFeeSched.Name = "comboFeeSched";
			this.comboFeeSched.Size = new System.Drawing.Size(174, 21);
			this.comboFeeSched.TabIndex = 0;
			this.comboFeeSched.SelectionChangeCommitted += new System.EventHandler(this.comboGeneric_SelectionChangeCommitted);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(377, 485);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// groupBox8
			// 
			this.groupBox8.Controls.Add(this.label9);
			this.groupBox8.Controls.Add(this.butUpdateWriteoffs);
			this.groupBox8.Location = new System.Drawing.Point(236, 373);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(205, 102);
			this.groupBox8.TabIndex = 29;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "Global Update Writeoff Estimates";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(11, 16);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(188, 50);
			this.label9.TabIndex = 6;
			this.label9.Text = "Only for offices running reports on writeoff estimates after updating fee schedul" +
    "es prior to selecting patients.";
			// 
			// butUpdateWriteoffs
			// 
			this.butUpdateWriteoffs.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUpdateWriteoffs.Autosize = true;
			this.butUpdateWriteoffs.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUpdateWriteoffs.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUpdateWriteoffs.CornerRadius = 4F;
			this.butUpdateWriteoffs.Location = new System.Drawing.Point(11, 66);
			this.butUpdateWriteoffs.Name = "butUpdateWriteoffs";
			this.butUpdateWriteoffs.Size = new System.Drawing.Size(75, 24);
			this.butUpdateWriteoffs.TabIndex = 5;
			this.butUpdateWriteoffs.Text = "Update";
			this.butUpdateWriteoffs.Click += new System.EventHandler(this.butUpdateWriteoffs_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(296, 485);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 30;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// FormFeeSchedTools
			// 
			this.ClientSize = new System.Drawing.Size(464, 522);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.groupBox8);
			this.Controls.Add(this.groupBox7);
			this.Controls.Add(this.groupBox6);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormFeeSchedTools";
			this.ShowInTaskbar = false;
			this.Text = "Fee Tools";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFeeSchedTools_FormClosing);
			this.Load += new System.EventHandler(this.FormFeeSchedTools_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.groupBox7.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormFeeSchedTools_Load(object sender, System.EventArgs e) {
			FillComboBoxes();
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				butImportCanada.Visible=false;
			}
		}

		private void FillComboBoxes() {
			long feeSchedNum1Selected=0;//Default to the first 
			if(comboFeeSched.SelectedIndex > -1) {
				feeSchedNum1Selected=_listFeeScheds[comboFeeSched.SelectedIndex].FeeSchedNum;
			}
			long feeSchedNum2Selected=0;//Default to the first
			if(comboFeeSchedTo.SelectedIndex > -1) {
				feeSchedNum2Selected=_listFeeScheds[comboFeeSchedTo.SelectedIndex].FeeSchedNum;
			}
			//The number of clinics and providers cannot change while inside this window.  Always reselect exactly what the user had before.
			int comboClinicIdx=comboClinic.SelectedIndex;
			int comboProvIdx=comboProvider.SelectedIndex;
			int comboProvToIdx=comboProviderTo.SelectedIndex;
			comboFeeSched.Items.Clear();
			comboFeeSchedTo.Items.Clear();
			comboClinic.Items.Clear();
			comboClinicTo.Items.Clear();
			comboProvider.Items.Clear();
			comboProviderTo.Items.Clear();
			string str;
			for(int i=0;i<_listFeeScheds.Count;i++) {
				str=_listFeeScheds[i].Description;
				if(_listFeeScheds[i].FeeSchedType!=FeeScheduleType.Normal) {
					str+=" ("+_listFeeScheds[i].FeeSchedType.ToString()+")";
				}
				comboFeeSched.Items.Add(str);
				comboFeeSchedTo.Items.Add(str);
			}
			if(_listFeeScheds.Count==0) {//No fee schedules in the database so set the first item to none.
				comboFeeSched.Items.Add(Lan.g(this,"None"));
				comboFeeSchedTo.Items.Add(Lan.g(this,"None"));
			}
			comboFeeSched.SelectedIndex=0;
			comboFeeSchedTo.SelectedIndex=0;
			if(!PrefC.HasClinicsEnabled) {//No clinics
				//Add none even though clinics is turned off so that 0 is a valid index to select.
				comboClinic.Items.Add(Lan.g(this,"None"));
				comboClinicTo.Items.Add(new ODBoxItem<Clinic>(Lan.g(this,"None"),new Clinic() { ClinicNum=0 }));
				comboBoxMultiClinics.Items.Clear();
				comboBoxMultiClinics.Items.Add(new ODBoxItem<Clinic>(Lan.g(this,"None"),new Clinic() { ClinicNum=0 }));
				comboBoxMultiClinics.SetSelected(0,true);
				//For UI reasons, leave the clinic combo boxes visible for users not using clinics and they will just say "none".
				comboClinic.Enabled=false;
				comboClinicTo.Enabled=false;
				comboBoxMultiClinics.Visible=true;
				comboBoxMultiClinics.Enabled=false;
				butPickClinic.Enabled=false;
				butPickClinicTo.Enabled=false;
			}
			else {
				comboClinic.Items.Add(Lan.g(this,"Default"));
				comboClinicTo.Items.Add(new ODBoxItem<Clinic>(Lan.g(this,"Default"),new Clinic { Abbr="Default" }));
				for(int i=0;i<_listClinics.Count;i++) {
					comboClinic.Items.Add(_listClinics[i].Abbr);
					comboClinicTo.Items.Add(new ODBoxItem<Clinic>(_listClinics[i].Abbr,_listClinics[i]));
				}
			}
			comboProvider.Items.Add(Lan.g(this,"None"));
			comboProviderTo.Items.Add(Lan.g(this,"None"));
			for(int i=0;i<_listProvs.Count;i++) {
				comboProvider.Items.Add(_listProvs[i].Abbr);
				comboProviderTo.Items.Add(_listProvs[i].Abbr);
			}
			for(int i=0;i<_listFeeScheds.Count;i++) {
				if(_listFeeScheds[i].FeeSchedNum==feeSchedNum1Selected) {
					comboFeeSched.SelectedIndex=i;
				}
				if(_listFeeScheds[i].FeeSchedNum==feeSchedNum2Selected) {
					comboFeeSchedTo.SelectedIndex=i;
				}
			}
			comboClinic.SelectedIndex=comboClinicIdx > -1 ? comboClinicIdx:0;
			if(_listSelectedClinicNumsTo.Count > 1) {
				comboClinicTo.SelectedIndex=-1;
				comboClinicTo.DropDownStyle=ComboBoxStyle.DropDown;
				comboClinicTo.Text=Lans.g(this,"Multiple");
			}
			else if(_listSelectedClinicNumsTo.Count==0) {
				comboClinicTo.SelectedIndex=0;//'Default'
				_listSelectedClinicNumsTo.Add(comboClinicTo.SelectedTag<Clinic>().ClinicNum);
			}
			else {//One Clinic selected
				comboClinicTo.SetSelectedItem<Clinic>(x => x.ClinicNum==_listSelectedClinicNumsTo.First(),"");
			}
			comboProvider.SelectedIndex=comboProvIdx > -1 ? comboProvIdx:0;
			comboProviderTo.SelectedIndex=comboProvToIdx > -1 ? comboProvToIdx:0;
			if(_listFeeScheds[comboFeeSched.SelectedIndex].IsGlobal) {
				comboClinic.Enabled=false;
				butPickClinic.Enabled=false;
				comboClinic.SelectedIndex=0;				
				comboProvider.Enabled=false;
				butPickProv.Enabled=false;
				comboProvider.SelectedIndex=0;
			}
			else {
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
					comboClinic.Enabled=true;
					butPickClinic.Enabled=true;
				}
				comboProvider.Enabled=true;
				butPickProv.Enabled=true;
			}
			if(_listFeeScheds[comboFeeSchedTo.SelectedIndex].IsGlobal) {
				comboClinicTo.Enabled=false;
				butPickClinicTo.Enabled=false;
				comboClinicTo.SelectedIndex=0;
				_listSelectedClinicNumsTo=new List<long> { comboClinicTo.SelectedTag<Clinic>().ClinicNum };
				comboProviderTo.Enabled=false;
				butPickProvTo.Enabled=false;
				comboProviderTo.SelectedIndex=0;
			}
			else {
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
					comboClinicTo.Enabled=true;
					butPickClinicTo.Enabled=true;
				}
				comboProviderTo.Enabled=true;
				butPickProvTo.Enabled=true;
			}
		}

		private void butClear_Click(object sender, System.EventArgs e) {
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				if(!MsgBox.Show(this,true,"This will clear all values from the selected fee schedule for the currently selected clinic and provider.  Are you sure you want to continue?")) {
					return;
				}
			}
			else if(!MsgBox.Show(this,true,"This will clear all values from the selected fee schedule for the currently selected provider.  Are you sure you want to continue?")) {
				return;
			}
			Action actionCloseFeeSchedClearProgress=ODProgressOld.ShowProgressStatus("FeeSchedClear",this);
			long clinicNum=0;
			if(!PrefC.GetBool(PrefName.EasyNoClinics) && comboClinic.SelectedIndex!=0){
				clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;
			}
			long provNum=0;
			if(comboProvider.SelectedIndex!=0) {
				provNum=_listProvs[comboProvider.SelectedIndex-1].ProvNum;
			}
			long feeSchedNum=_listFeeScheds[comboFeeSched.SelectedIndex].FeeSchedNum;
			_feeCache.BeginTransaction();
			_feeCache.RemoveFees(feeSchedNum,clinicNum,provNum);
			string logText=Lan.g(this,"Procedures for Fee Schedule")+" "+FeeScheds.GetDescription(feeSchedNum)+" ";
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				if(Clinics.GetAbbr(Clinics.ClinicNum)=="") {
					logText+=Lan.g(this,"at Headquarters");
				}
				else {
					logText+=Lan.g(this,"at clinic")+" "+Clinics.GetAbbr(Clinics.ClinicNum);
				}
			}
			logText+=" "+Lan.g(this,"were all cleared.");
			SecurityLogs.MakeLogEntry(Permissions.ProcFeeEdit,0,logText);
			_changed=true;
			actionCloseFeeSchedClearProgress();
			DialogResult=DialogResult.OK;
		}

		private void butCopy_Click(object sender, System.EventArgs e) {
			if(PrefC.HasClinicsEnabled && _listSelectedClinicNumsTo.Count==0) {
				MsgBox.Show(this,"At least one \"Clinic To\" clinic must be selected.");
				return;
			}
			long toProvNum=0;
			if(comboProviderTo.SelectedIndex!=0) {
				toProvNum=_listProvs[comboProviderTo.SelectedIndex-1].ProvNum;
			}
			long toFeeSchedNum=_listFeeScheds[comboFeeSchedTo.SelectedIndex].FeeSchedNum;
			long fromClinicNum=0;
			if(!PrefC.GetBool(PrefName.EasyNoClinics) && comboClinic.SelectedIndex!=0){
				fromClinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;
			}
			long fromProvNum=0;
			if(comboProvider.SelectedIndex!=0) {
				fromProvNum=_listProvs[comboProvider.SelectedIndex-1].ProvNum;
			}
			long fromFeeSchedNum=_listFeeScheds[comboFeeSched.SelectedIndex].FeeSchedNum;
			if(fromFeeSchedNum==toFeeSchedNum
				&& fromProvNum==toProvNum
				&& fromClinicNum.In(_listSelectedClinicNumsTo)) 
			{
				MsgBox.Show(this,"Fee Schedules are not allowed to be copied into themselves. Please choose another fee schedule to copy.");
				return;
			}
			if(!MsgBox.Show(this,true,"The \"Choose Settings\" fee schedule will overwrite the \"Copy To\" fee schedule.  Are you sure you want to continue?")){
				return;
			}
			ODProgress.ShowProgressForThread(o => CopyFeeSchedule(o,fromFeeSchedNum,fromClinicNum,fromProvNum,toFeeSchedNum,_listSelectedClinicNumsTo,
					toProvNum),
				this,"Preparing to copy fees...",ProgBarStyle.Continuous,"FormFeeSchedTools_CopyFeeSchedule");
			//After finishing, clear the Copy To section, but leave the Copy From section as is.
			comboFeeSchedTo.SelectedIndex=0;
			comboClinicTo.SelectedIndex=0;
			_listSelectedClinicNumsTo.Clear();
			_listSelectedClinicNumsTo.Add(comboClinicTo.SelectedTag<Clinic>().ClinicNum);
			comboProviderTo.SelectedIndex=0;
			MsgBox.Show(this,"Done.");
		}

		///<summary>Copies one fee schedule to another.</summary>
		private void CopyFeeSchedule(ODThread odThread,long fromFeeSchedNum,long fromClinicNum,long fromProvNum,long toFeeSchedNum,
			List<long> listClinicNumsTo,long toProvNum) 
		{
			_feeCache.BeginTransaction();
			if(_listSelectedClinicNumsTo.Count==0) {
				_listSelectedClinicNumsTo.Add(0);
			}
			//copy any values over
			//Modifies in-memory list, adding 100% match entries based on combo boxes.
			//First, we've cleared out the "To" fee schedule based on the "To" combo box settings so that fee sched will have 0 entries for the criteria in the combo boxes.
			//Next, we take all the fees from the "From" fee schedule with those matching criteria, and copy them to the "To" fee schedule with the "To" schedule's criteria.
			listClinicNumsTo.ForEach(clinicNum => _feeCache.RemoveFees(toFeeSchedNum,clinicNum,toProvNum));
			List<Fee> listNewFees=_feeCache.GetListFees(fromFeeSchedNum,fromClinicNum,fromProvNum);
			//fill listBestFees with the "best" fee for each proccode.
			List<Fee> listBestFees= listNewFees
				.GroupBy(x => x.CodeNum)
				.Select(x => _feeCache.GetFee(x.Key,fromFeeSchedNum,fromClinicNum,fromProvNum,doGetExactMatch: true))
				.Where(x=>x!=null).ToList();
			int rowNum=0;
			object locker=new object();
			List<Action> listActions=new List<Action>();
			foreach(long clinicNumTo in listClinicNumsTo) {
				listActions.Add(() => {
					foreach(Fee fee in listBestFees) {
						Fee newFee=fee.Copy();
						newFee.FeeNum=0;
						newFee.ProvNum=toProvNum;
						newFee.ClinicNum=clinicNumTo;
						newFee.FeeSched=toFeeSchedNum;
						_feeCache.Add(newFee);//FeeCache can handle concurrent updates.
						odThread.ProgressLog.UpdateProgressDetailed(Lan.g(this,"Copying fees, please wait")+"...","","",rowNum
							,listBestFees.Count * listClinicNumsTo.Count,progStyle: ProgBarStyle.Continuous);
						lock(locker) {
							rowNum++;
						}
					}
				});
			}
			ODThread.RunParallel(listActions,TimeSpan.FromMinutes(30));
			string logText=Lan.g(this,"Fee Schedule:")+" "+FeeScheds.GetDescription(fromFeeSchedNum)+" "+Lan.g(this,"copied to")
					+" "+Lan.g(this,"Fee Schedule:")+" "+FeeScheds.GetDescription(toFeeSchedNum)+" "+Lan.g(this,"using Fee Tools.");
			SecurityLogs.MakeLogEntry(Permissions.ProcFeeEdit,0,logText);
			_changed=true;
		}

		private void butIncrease_Click(object sender, System.EventArgs e) {
			int percent=0;
			if(textPercent.Text==""){
				MsgBox.Show(this,"Please enter a percent first.");
				return;
			}
			try{
				percent=System.Convert.ToInt32(textPercent.Text);
			}
			catch{
				MsgBox.Show(this,"Percent is not a valid number.");
				return;
			}
			if(percent<-99 || percent>99){
				MsgBox.Show(this,"Percent must be between -99 and 99.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will overwrite all values of the selected fee schedule, causing all previously entered fee "
				+"amounts to be lost.  It is recommended to first create a backup copy of the original fee schedule, then update the original fee schedule "
				+"with the new fees.  Are you sure you want to continue?"))
			{
				return;
			}
			long clinicNum=0;
			if(!PrefC.GetBool(PrefName.EasyNoClinics) && comboClinic.SelectedIndex>0){
				clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;
			}
			long provNum=0;
			if(comboProvider.SelectedIndex>0){
				provNum=_listProvs[comboProvider.SelectedIndex-1].ProvNum;
			}
			long feeSchedNum=_listFeeScheds[comboFeeSched.SelectedIndex].FeeSchedNum;
			//Modifies in-memory list, all entries increased (if they exist) will be a 100% match to the combo box settings.
			List<Fee> listFees=_feeCache.GetListFees(feeSchedNum,clinicNum,provNum);
			bool doIncreaseFees=EvaluateOverrides(clinicNum,provNum,feeSchedNum,listFees);
			if(!doIncreaseFees) {
				return;//either no fees would be updated or the user chose to cancel and review so don't increase fees.
			}
			_feeCache.BeginTransaction();
			Action actionCloseFeeSchedIncreaseProgress=ODProgressOld.ShowProgressStatus("FeeSchedIncrease",this,Lan.g(this,"Preparing to modify fees")+"...");
			int round=0;//Default to dollar
			if(radioDime.Checked){
				round=1;
			}
			if(radioPenny.Checked){
				round=2;
			}
			listFees=Fees.Increase(feeSchedNum,percent,round,listFees,clinicNum,provNum);
			_feeCache.RemoveFees(feeSchedNum,clinicNum,provNum);
			string procCode;
			for(int i=0;i<listFees.Count;i++) {
				if(listFees[i].Amount==0) {
					continue;
				}
				try {
					procCode=ProcedureCodes.GetStringProcCode(listFees[i].CodeNum);
				}
				catch(Exception) {//if CodeNum is not in the procedurecode table, don't make securitylog entry
					continue;
				}
				_feeCache.Add(listFees[i]);
				string logText=Lan.g(this,"Procedure")+": "+procCode+", "
					+Lan.g(this,"Fee")+": "+listFees[i].Amount.ToString("c")+", "
					+Lan.g(this,"Fee Schedule")+": "+FeeScheds.GetDescription(listFees[i].FeeSched);
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
					if(Clinics.GetAbbr(clinicNum)=="") {
						logText+=Lan.g(this,"at Headquarters");
					}
					else {
						logText+=", "+Lan.g(this,"at clinic")+": "+Clinics.GetAbbr(clinicNum);
					}
				}
				if(provNum!=0) {
					logText+=", "+Lan.g(this,"for provider")+": "+Providers.GetAbbr(provNum);
				}
				logText+=". "+Lan.g(this,"Fee increased by")+" "+((float)percent/100.0f).ToString("p")+" "+Lan.g(this," using the increase button in the Fee Tools window.");
				SecurityLogs.MakeLogEntry(Permissions.ProcFeeEdit,0,logText,listFees[i].CodeNum,DateTime.MinValue);
				SecurityLogs.MakeLogEntry(Permissions.LogFeeEdit,0,Lan.g(this,"Fee Updated"),listFees[i].FeeNum,listFees[i].SecDateTEdit);
				ODEvent.Fire(new ODEventArgs("FeeSchedIncrease",new ProgressBarHelper(Lan.g(this,"Modifying fees, please wait")+"...","",i,listFees.Count,ProgBarStyle.Continuous)));
			}
			_changed=true;
			actionCloseFeeSchedIncreaseProgress();
			DialogResult=DialogResult.OK;
		}

		///<summary>Determines if there are overrides being updated, or just a regular feeSchedule. Returns true if there are overrides and user wants to 
		///continue, or if it is a regular fee schedule. Returns false is it is an override schedule and there are no overrides to update or if user 
		///chooses to cancel to review.</summary>
		private bool EvaluateOverrides(long clinicNum,long provNum,long feeSchedNum,List<Fee> listFees) {
			int countGeneralFees=_feeCache.GetListFees(feeSchedNum,0,0).Count; 
			string msgText="";
			string clinicName=Clinics.GetAbbr(clinicNum);
			string provName=Providers.GetAbbr(provNum);
			string feeSchedDesc=_listFeeScheds.FirstOrDefault(x => x.FeeSchedNum==feeSchedNum).Description;
			if(clinicNum!=0 && provNum!=0) {
				if(listFees.Count==0) {
					msgText=Lan.g(this,"There are no overrides for clinic")+" '"+clinicName+"' "+Lan.g(this,"and provider")+" '"+provName+"' "
						+Lan.g(this,"so no fees will be updated. If you want to create overrides, first enter or copy fees into that override fee schedule.");
					MessageBox.Show(msgText);
					return false;//don't run increase tool
				}
				if(listFees.Count!=countGeneralFees) {
					msgText=Lan.g(this,"There are")+" "+listFees.Count+" "+Lan.g(this,"override fees for clinic")+" '"+clinicName+"' "+Lan.g(this,"and provider")
						+" '"+provName+"' "+Lan.g(this,"and there are")+" "+(countGeneralFees-listFees.Count)+" "+Lan.g(this,"general fees for fee schedule")+" '"
						+feeSchedDesc+"'. "+Lan.g(this,"Only the")+" "+listFees.Count+" "+Lan.g(this,"fees will be increased, cancel if you want to review first.");
					if(MessageBox.Show(msgText,"",MessageBoxButtons.OKCancel)==DialogResult.Cancel) {
						return false;
					}
				}
				return true;
			}
			else if(clinicNum!=0) {
				if(listFees.Count==0) {
					msgText=Lan.g(this,"There are no overrides for clinic")+" '"+clinicName+"' "
						+Lan.g(this,"so no fees will be updated. If you want to create overrides, first enter or copy fees into that override fee schedule.");
					MessageBox.Show(msgText);
					return false;
				}
				if(listFees.Count!=countGeneralFees) {
					msgText=Lan.g(this,"There are")+" "+listFees.Count+" "+Lan.g(this,"override fees for clinic")+" '"+clinicName+"' "+Lan.g(this,"and there are")
						+" "+(countGeneralFees-listFees.Count)+" "+Lan.g(this,"general fees for fee schedule")+" '"+feeSchedDesc+"'. "+Lan.g(this,"Only the")+" "
						+listFees.Count+" "+Lan.g(this,"fees will be increased, cancel if you want to review first.");
					if(MessageBox.Show(msgText,"",MessageBoxButtons.OKCancel)==DialogResult.Cancel) {
						return false;
					}
				}
				return true;	
			}
			else if(provNum!=0) {
				if(listFees.Count==0) {
					msgText=Lan.g(this,"There are no overrides for provider")+" '"+provName+"' "
						+Lan.g(this,"so no fees will be updated. If you want to create overrides, first enter or copy fees into that override fee schedule.");
					MessageBox.Show(msgText);
					return false;
				}
				if(listFees.Count!=countGeneralFees) {
					msgText=Lan.g(this,"There are")+" "+listFees.Count+" "+Lan.g(this,"override fees for provider")+" '"+provName+"' "+Lan.g(this,"and there are")
						+" "+(countGeneralFees-listFees.Count)+" "+Lan.g(this,"general fees for fee schedule")+" '"+feeSchedDesc+"'. "+Lan.g(this,"Only the")+" "
						+listFees.Count+" "+Lan.g(this,"fees will be increased, cancel if you want to review first.");
					if(MessageBox.Show(msgText,"",MessageBoxButtons.OKCancel)==DialogResult.Cancel) {
						return false;
					}
				}
				return true;
			}
			return true;//clinicNum==0 && provNum==0
		}

		private void butExport_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			SaveFileDialog Dlg=new SaveFileDialog();
			if(Directory.Exists(PrefC.GetString(PrefName.ExportPath))){
				Dlg.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
			}
			else if(Directory.Exists("C:\\")) {
				Dlg.InitialDirectory="C:\\";
			}
			long feeSchedNum=_listFeeScheds[comboFeeSched.SelectedIndex].FeeSchedNum;
      string feeSchedDesc= FeeScheds.GetDescription(feeSchedNum);
      //scrub out any non-AlphaNumeric characters.
      feeSchedDesc=Regex.Replace(feeSchedDesc,"(?:[^a-z0-9 ]|(?<=['\"])s)","",RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);
      Dlg.FileName="Fees"+feeSchedDesc+".txt";
			if(Dlg.ShowDialog()!=DialogResult.OK){
				Cursor=Cursors.Default;
				return;
			}
			Action actionCloseFeeSchedExportProgress=ODProgressOld.ShowProgressStatus("FeeSchedExport",this,Lan.g(this,"Preparing to export fees")+"...");
			//CreateText will overwrite any content if the file already exists.
			using(StreamWriter sr=File.CreateText(Dlg.FileName)) {
				FeeSched feeSched=_listFeeScheds[comboFeeSched.SelectedIndex];
				long clinicNum=0;
				if(comboClinic.SelectedIndex!=0) {
					clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;
				}
				long provNum=0;
				if(comboProvider.SelectedIndex!=0) {
					provNum=_listProvs[comboProvider.SelectedIndex-1].ProvNum;
				}
				//Get every single procedure code from the cache which will already be ordered by ProcCat and then ProcCode.
				//Even if the code does not have a fee, include it in the export because that will trigger a 'deletion' when importing over other schedules.
				int rowNum=0;
				List<ProcedureCode> listProcCodes=ProcedureCodes.GetListDeep();
				foreach(ProcedureCode procCode in listProcCodes) {
					//Get the best matching fee for the current selections. 
					Fee fee=_feeCache.GetFee(procCode.CodeNum,feeSchedNum,clinicNum,provNum);
					sr.Write(procCode.ProcCode+"\t");
					if(fee!=null && fee.Amount!=-1) {
						sr.Write(fee.Amount.ToString("n"));
					}
					sr.Write("\t");
					sr.Write(procCode.AbbrDesc+"\t");
					sr.WriteLine(procCode.Descript);
					ODEvent.Fire(new ODEventArgs("FeeSchedExport",new ProgressBarHelper(Lan.g(this,"Exporting fees, please wait")+"...","",rowNum,listProcCodes.Count,ProgBarStyle.Continuous)));
					rowNum++;
				}
			}
			Cursor=Cursors.Default;
			actionCloseFeeSchedExportProgress();
			MsgBox.Show(this,"Fee schedule exported.");
			DialogResult=DialogResult.OK;
		}

		private void butImport_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,true,"If you want a clean slate, the current fee schedule should be cleared first.  When imported, any fees that are found in the text file will overwrite values of the Choose Settings fee schedule.  Are you sure you want to continue?")) 
			{
				return;
			}
			Cursor=Cursors.WaitCursor;
			OpenFileDialog Dlg=new OpenFileDialog();
			if(Directory.Exists(PrefC.GetString(PrefName.ExportPath))) {
				Dlg.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
			}
			else if(Directory.Exists("C:\\")) {
				Dlg.InitialDirectory="C:\\";
			}
			if(Dlg.ShowDialog()!=DialogResult.OK) {
				Cursor=Cursors.Default;
				return;
			}
			if(!File.Exists(Dlg.FileName)){
				Cursor=Cursors.Default;
				MsgBox.Show(this,"File not found");
				return;
			}
			//Import deletes fee of the given sched for the active clinic if it exists and inserts new fees based on fee settings.
			long clinicNum=0;
			if(comboClinic.SelectedIndex!=0) {
				clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;
			}
			long provNum=0;
			if(comboProvider.SelectedIndex!=0) {
				provNum=_listProvs[comboProvider.SelectedIndex-1].ProvNum;
			}
			FeeSched feeSched=_listFeeScheds[comboFeeSched.SelectedIndex];
			FeeL.ImportFees(feeSched.FeeSchedNum,clinicNum,provNum,Dlg.FileName,this);
			_changed=true;
			DialogResult=DialogResult.OK;
		}

		private void butImportCanada_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,true,"If you want a clean slate, the current fee schedule should be cleared first.  When imported, any fees that are found in the text file will overwrite values of the current fee schedule showing in the main window.  Are you sure you want to continue?")) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			FormFeeSchedPickRemote formPick=new FormFeeSchedPickRemote();
			formPick.Url=@"http://www.opendental.com/feescanada/";//points to index.php file
			if(formPick.ShowDialog()!=DialogResult.OK) {
				Cursor=Cursors.Default;
				return;
			}
			Cursor=Cursors.WaitCursor;//original wait cursor seems to go away for some reason.
			Application.DoEvents();
			string feeData="";
			if(formPick.IsFileChosenProtected) {
				string memberNumberODA="";
				string memberPasswordODA="";
				if(formPick.FileChosenName.StartsWith("ON_")) {//Any and all Ontario fee schedules
					FormFeeSchedPickAuthOntario formAuth=new FormFeeSchedPickAuthOntario();
					if(formAuth.ShowDialog()!=DialogResult.OK) {
						Cursor=Cursors.Default;
						return;
					}
					memberNumberODA=formAuth.ODAMemberNumber;
					memberPasswordODA=formAuth.ODAMemberPassword;
				}
				Action actionCloseFeeSchedImportCanadaProgress=ODProgressOld.ShowProgressStatus("FeeSchedImportCanada",this);
				//prepare the xml document to send--------------------------------------------------------------------------------------
				XmlWriterSettings settings = new XmlWriterSettings();
				settings.Indent = true;
				settings.IndentChars = ("    ");
				StringBuilder strbuild=new StringBuilder();
				using(XmlWriter writer=XmlWriter.Create(strbuild,settings)) {
					writer.WriteStartElement("RequestFeeSched");
					writer.WriteStartElement("RegistrationKey");
					writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));
					writer.WriteEndElement();//RegistrationKey
					writer.WriteStartElement("FeeSchedFileName");
					writer.WriteString(formPick.FileChosenName);
					writer.WriteEndElement();//FeeSchedFileName
					if(memberNumberODA!="") {
						writer.WriteStartElement("ODAMemberNumber");
						writer.WriteString(memberNumberODA);
						writer.WriteEndElement();//ODAMemberNumber
						writer.WriteStartElement("ODAMemberPassword");
						writer.WriteString(memberPasswordODA);
						writer.WriteEndElement();//ODAMemberPassword
					}
					writer.WriteEndElement();//RequestFeeSched
				}
#if DEBUG
				OpenDental.localhost.Service1 updateService=new OpenDental.localhost.Service1();
#else
				OpenDental.customerUpdates.Service1 updateService=new OpenDental.customerUpdates.Service1();
				updateService.Url=PrefC.GetString(PrefName.UpdateServerAddress);
#endif
				//Send the message and get the result-------------------------------------------------------------------------------------
				string result="";
				try {
					ODEvent.Fire(new ODEventArgs("FeeSchedImportCanada",Lan.g(this,"Retrieving fee schedule")+"..."));
					result=updateService.RequestFeeSched(strbuild.ToString());
				}
				catch(Exception ex) {
					actionCloseFeeSchedImportCanadaProgress();
					Cursor=Cursors.Default;
					MessageBox.Show("Error: "+ex.Message);
					return;
				}
				Cursor=Cursors.Default;
				XmlDocument doc=new XmlDocument();
				doc.LoadXml(result);
				//Process errors------------------------------------------------------------------------------------------------------------
				XmlNode node=doc.SelectSingleNode("//Error");
				if(node!=null) {
					actionCloseFeeSchedImportCanadaProgress();
					MessageBox.Show(node.InnerText,"Error");
					return;
				}
				node=doc.SelectSingleNode("//KeyDisabled");
				if(node==null) {
					//no error, and no disabled message
					if(Prefs.UpdateBool(PrefName.RegistrationKeyIsDisabled,false)) {//this is one of three places in the program where this happens.
						DataValid.SetInvalid(InvalidType.Prefs);
					}
				}
				else {
					actionCloseFeeSchedImportCanadaProgress();
					MessageBox.Show(node.InnerText);
					if(Prefs.UpdateBool(PrefName.RegistrationKeyIsDisabled,true)) {//this is one of three places in the program where this happens.
						DataValid.SetInvalid(InvalidType.Prefs);
					}
					return;
				}
				//Process a valid return value------------------------------------------------------------------------------------------------
				node=doc.SelectSingleNode("//ResultCSV64");
				string feeData64=node.InnerXml;
				byte[] feeDataBytes=Convert.FromBase64String(feeData64);
				feeData=Encoding.UTF8.GetString(feeDataBytes);
			}
			else {
				Action actionCloseFeeSchedImportCanadaProgress=ODProgressOld.ShowProgressStatus("FeeSchedImportCanada",this,
					Lan.g(this,"Downloading fee schedule")+"...");
				string tempFile=PrefC.GetRandomTempFile(".tmp");
				WebClient myWebClient=new WebClient();
				try {
					myWebClient.DownloadFile(formPick.FileChosenUrl,tempFile);
				}
				catch(Exception ex) {
					actionCloseFeeSchedImportCanadaProgress();
					MessageBox.Show(Lan.g(this,"Failed to download fee schedule file")+": "+ex.Message);
					Cursor=Cursors.Default;
					return;
				}
				feeData=File.ReadAllText(tempFile);
				File.Delete(tempFile);
			}
			string[] feeLines=feeData.Split('\n');
			double feeAmt;
			long numImported=0;
			long numSkipped=0;
			long clinicNum=0;
			if(comboClinic.SelectedIndex!=0) {
				clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;
			}
			long provNum=0;
			if(comboProvider.SelectedIndex!=0) {
				provNum=_listProvs[comboProvider.SelectedIndex-1].ProvNum;
			}
			FeeSched feeSched=_listFeeScheds[comboFeeSched.SelectedIndex];
			List<Fee> listNewFees=_feeCache.GetListFees(feeSched.FeeSchedNum,clinicNum,provNum);
			for(int i=0;i<feeLines.Length;i++) {
				string[] fields=feeLines[i].Split('\t');
				if(fields.Length>1) {// && fields[1]!=""){//we no longer skip blank fees
					string procCode=fields[0];
					if(ProcedureCodes.IsValidCode(procCode)) { //The Fees.Import() function will not import fees for codes that do not exist.
						if(fields[1]=="") {
							feeAmt=-1;//triggers deletion of existing fee, but no insert.
						}
						else {
							feeAmt=PIn.Double(fields[1],doUseEnUSFormat: true);//The fees are always in the format "1.00" so we need to parse accordingly.
						}
						listNewFees=Fees.Import(procCode,feeAmt,feeSched.FeeSchedNum,clinicNum,provNum,listNewFees);
						ODEvent.Fire(new ODEventArgs("FeeSchedImportCanada",new ProgressBarHelper(Lan.g(this,"Importing fees, please wait")+"...","",(int)numImported,feeLines.Length,ProgBarStyle.Continuous)));
						numImported++;
					}
					else {
						numSkipped++;
					}
				}
			}
			Cursor=Cursors.Default;
			_changed=true;
			ODEvent.Fire(new ODEventArgs("FeeSchedImportCanada","DEFCON 1"));//jsalmon - I don't think this is necessary but it doesn't hurt.
			_feeCache.BeginTransaction();
			_feeCache.RemoveFees(feeSched.FeeSchedNum,clinicNum,provNum);
			foreach(Fee fee in listNewFees) {
				_feeCache.Add(fee);
			}
			DialogResult=DialogResult.OK;
			string outputMessage=Lan.g(this,"Done. Number imported")+": "+numImported;
			if(numSkipped>0) {
				outputMessage+=" "+Lan.g(this,"Number skipped")+": "+numSkipped;
			}
			MessageBox.Show(outputMessage);
		}

		private void butUpdate_Click(object sender,EventArgs e) {
			long rowsChanged=0;
			if(!MsgBox.Show(this,true,"All treatment planned procedures for all patients will be updated.  Only the fee will be updated, not the insurance "
				+"estimate.  It might take a few minutes.  Continue?")) {
				return;
			}
			ODProgressExtended progExtended=new ODProgressExtended("FeeSchedEvent",new FeeSchedEvent(),this,
					tag: new ProgressBarHelper(Lan.g(this,"Fee Schedule Update Progress"),progressBarEventType:ProgBarEventType.Header));
			Cursor=Cursors.WaitCursor;
			try {	
				if(PrefC.HasClinicsEnabled) {
					int countClinicsSelected=comboBoxMultiClinics.ListSelectedClinics.Count;
					for(int i=0;i<countClinicsSelected;i++) {
						Clinic clinicCur=comboBoxMultiClinics.ListSelectedClinics[i];
						while(progExtended.IsPaused) {
							Thread.Sleep(10);
							if(progExtended.IsCanceled) {
								break;
							}
						}
						if(progExtended.IsCanceled) {
							break;
						}
						double percentComplete=(((double)i)/countClinicsSelected*100);
						if(countClinicsSelected>1) {
							progExtended.Fire(new ODEventArgs("FeeSchedEvent",new ProgressBarHelper(Lan.g(this, "Overall"),(int)percentComplete+"%",i,
							countClinicsSelected,tagString:"OverallStatus")));
							progExtended.Fire(new ODEventArgs("FeeSchedEvent",new ProgressBarHelper(Lan.g(this,clinicCur.Abbr),"0%",1,100,
							tagString:"Clinic")));
						}
						else {
							progExtended.Fire(new ODEventArgs("FeeSchedEvent",new ProgressBarHelper(Lan.g(this,clinicCur.Abbr),"0%",1,100,
								tagString:"Clinic")));
							progExtended.HideButtons();//can't pause or cancel with 1 clinic. This event needs to be called after the bar is instantiated. 
						}
						rowsChanged+=Procedures.GlobalUpdateFees(_feeCache.GetFeesForClinics(new List<long> { clinicCur.ClinicNum }),clinicCur.ClinicNum,clinicCur.Abbr);
						if(progExtended.IsPaused) {
							progExtended.AllowResume();
						}
					}
					if(countClinicsSelected>1) {
						progExtended.Fire(new ODEventArgs("FeeSchedEvent",new ProgressBarHelper(Lan.g(this,"Overall"),"100%",100,100,tagString:"OverallStatus")));
					}
				}
				else {//no clinic - "Clinic" here is just a reference to the progress bar that updates Clinic progress instead of overall progress
					progExtended.Fire(new ODEventArgs("FeeSchedEvent",new ProgressBarHelper(Lan.g(this,"Updating..."),"0%",1,100,tagString:"Clinic")));
					progExtended.HideButtons();
					rowsChanged=Procedures.GlobalUpdateFees(_feeCache.ToList());
				}
				progExtended.OnProgressDone();
				progExtended.Fire(new ODEventArgs("FeeSchedEvent"
					,new ProgressBarHelper(Lan.g(this,"Treatment planned procedure fees changed:")+" "+rowsChanged.ToString()+"\r\n"+Lan.g(this,"Done.")
					,progressBarEventType:ProgBarEventType.TextMsg)));
			}
			catch(ApplicationException ex) {
				Cursor=Cursors.Default;
				progExtended.Close();
				MessageBox.Show(ex.Message);
				return;	
			}
			finally {
				if(progExtended.IsCanceled) {//close
					progExtended.Close();
					DialogResult=DialogResult.OK;
				}
				
			}
			Cursor=Cursors.Default;
		}

		private void butUpdateWriteoffs_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Writeoff estimates will be recalculated for all treatment planned procedures.  "
				+"This tool should only be run if you have updated fee schedules and want to run reports on writeoff estimates for patients that have not been viewed.  "
				+"\r\n\r\nThis could take a very long time.  Continue?"))
			{
				return;
			}
			List<Clinic> listWriteoffClinics = _listClinics.Select(x => x.Copy()).ToList();
			if(listWriteoffClinics.Count==0) {
				listWriteoffClinics.Add(new Clinic {Abbr="Updating Writeoffs...",ClinicNum=0});
			}
			ODProgressExtended progress = new ODProgressExtended("FeeSchedEvent",new FeeSchedEvent(),this,
				tag: new ProgressBarHelper(Lan.g(this,"Writeoff Update Progress"),progressBarEventType: ProgBarEventType.Header));
			progress.Fire(new ODEventArgs("FeeSchedEvent",new ProgressBarHelper("","0%"
						,0,100,ProgBarStyle.Blocks,"WriteoffProgress"))); 
			Cursor=Cursors.WaitCursor;
			long totalWriteoffsUpdated=0;
			try {
				string progressText="";
				foreach(Clinic clinicCur in listWriteoffClinics) {
					progress.Fire(new ODEventArgs("FeeSchedEvent",new ProgressBarHelper(clinicCur.Abbr,"0%",0,100,ProgBarStyle.Blocks,"WriteoffProgress"))); 
					long rowCurIndex = 0; //reset for each clinic.  
					progress.Fire(new ODEventArgs("FeeSchedEvent",new ProgressBarHelper(Lans.g("FeeSchedEvent","Getting list to update writeoffs..."),
							progressBarEventType: ProgBarEventType.TextMsg)));
					List<Procedure> listProcedures = Procedures.GetAllTp(clinicCur.ClinicNum);
					List<long> listPatNums = listProcedures.Select(x => x.PatNum).Distinct().ToList();
					List<Family> listFamilies = Patients.GetFamilies(listPatNums);
					List<Patient> listPatients = listFamilies.SelectMany(x => x.ListPats).ToList();
					List<InsSub> listInsSubs = InsSubs.GetListInsSubs(listPatients.Select(x => x.PatNum).ToList());
					List<InsPlan> listInsPlans = InsPlans.RefreshForSubList(listInsSubs);
					List<PatPlan> listPatPlans = PatPlans.GetPatPlansForPats(listPatNums);
					List<Benefit> listBenefits = Benefits.Refresh(listPatPlans,listInsSubs);
					List<ClaimProc> listClaimProcs = ClaimProcs.GetForProcs(listProcedures.Select(x => x.ProcNum).ToList());
					progress.Fire(new ODEventArgs("FeeSchedEvent",new ProgressBarHelper(Lans.g("FeeSchedEvent","Updating writeoff estimates for patients..."),
							progressBarEventType: ProgBarEventType.TextMsg)));
					foreach(Procedure proc in listProcedures) {
						rowCurIndex++;
						long patNum = proc.PatNum;
						Patient patient = listPatients.FirstOrDefault(x => x.PatNum==patNum);
						if(patient==null) { //could happen if a deleted pat has a TP proc
							continue;
						}
						List<long> listFamPatNums=listFamilies
							.FindAll(x => x.ListPats.Select(y => y.PatNum).Contains(patNum))
							.SelectMany(x => x.ListPats.Select(y => y.PatNum)).ToList(); 
						List<InsSub> filteredListInsSub = listInsSubs.FindAll(x => x.InsSubNum.In(listFamPatNums)).ToList();
						List<ClaimProc> listClaimProcsCur = listClaimProcs.FindAll(x => x.ProcNum==proc.ProcNum);
						List<InsPlan> listInsPlansCur = listInsPlans.FindAll(x => listClaimProcsCur.Exists(y => y.PlanNum==x.PlanNum));
						List<Benefit> listBenefitsCur = listBenefits.FindAll(x => listInsPlansCur.Exists(y => y.PlanNum==x.PlanNum));
						List<PatPlan> listPatPlansCur = listPatPlans.FindAll(x => x.PatNum==patNum);
						Procedures.ComputeEstimates(proc,patNum,listClaimProcsCur,false,listInsPlansCur,listPatPlansCur,
							listBenefitsCur,patient.Age,filteredListInsSub);
						double percentage = Math.Ceiling(((double)rowCurIndex/listProcedures.Count)*100);
						progress.Fire(new ODEventArgs("FeeSchedEvent",
							new ProgressBarHelper(clinicCur.Abbr,(int)percentage+"%",(int)percentage,100,ProgBarStyle.Blocks,"WriteoffProgress")));
						while(progress.IsPaused) {
							progress.AllowResume();
							if(progress.IsCanceled) {
								break;
							}
						}
						if(progress.IsCanceled) {
							break;
						}
					}
					if(listWriteoffClinics.Count>1) {//only show if more than one clinic
						progressText=rowCurIndex+" "+Lans.g("FeeSchedTools","procedures processed from")+" "+clinicCur.Abbr;
						progress.Fire(new ODEventArgs("FeeSchedEvent",new ProgressBarHelper(progressText,progressBarEventType: ProgBarEventType.TextMsg)));
					}
					totalWriteoffsUpdated+=rowCurIndex;
					if(progress.IsCanceled) {
						break;
					}
				}
				progress.OnProgressDone();
				progressText=Lans.g("FeeSchedTools","Writeoffs updated.")+" "+totalWriteoffsUpdated+" "+Lans.g("FeeSchedTools","procedures processed.");
				progress.Fire(new ODEventArgs("FeeSchedEvent",new ProgressBarHelper(progressText,progressBarEventType: ProgBarEventType.TextMsg)));
			}
			catch(ApplicationException ex) {
				Cursor=Cursors.Default;
				progress.Close();
				MessageBox.Show(ex.Message);
			}
			finally {
				if(progress.IsCanceled) {
					progress.Close();
				}
				Cursor=Cursors.Default;
			}
		}

		private void butPickFeeSched_Click(object sender,EventArgs e) {
			int selectedIndex=GetFeeSchedIndexFromPicker();
			//If the selectedIndex is -1, simply return and do not do anything.  There is no such thing as picking 'None' from the picker window.
			if(selectedIndex==-1) {
				return;
			}
			UI.Button pickerButton=(UI.Button)sender;
			if(pickerButton==butPickSched) { //First FeeSched combobox doesn't have "None" option.
				comboFeeSched.SelectedIndex=selectedIndex;
			}
			else if(pickerButton==butPickSchedTo) {
				comboFeeSchedTo.SelectedIndex=selectedIndex;
			}
			FillComboBoxes();
		}

		private void butPickClinic_Click(object sender,EventArgs e){
			int selectedIndex=GetClinicIndexFromPicker()+1;//All clinic combo boxes have a none option, so always add 1.
			//If the selectedIndex is 0, simply return and do not do anything.  There is no such thing as picking 'None' from the picker window.
			if(selectedIndex==0) {
				return;
			}
			UI.Button pickerButton=(UI.Button)sender;
			if(pickerButton==butPickClinic) {
				comboClinic.SelectedIndex=selectedIndex;
			}
			else if(pickerButton==butPickClinicTo) {
				comboClinicTo.SelectedIndex=selectedIndex;
			}
		}

		private void butPickClinicTo_Click(object sender,EventArgs e){
			FormClinics FormC=new FormClinics();
			FormC.IsSelectionMode=true;
			FormC.IsMultiSelect=true;
			FormC.ListClinics=_listClinics;
			FormC.ShowDialog();
			_listSelectedClinicNumsTo=FormC.ListSelectedClinicNums;
			if(_listSelectedClinicNumsTo.Count==0) {
				return;
			}
			if(_listSelectedClinicNumsTo.Count > 1) {
				comboClinicTo.SelectedIndex=-1;
				comboClinicTo.DropDownStyle=ComboBoxStyle.DropDown;
				comboClinicTo.Text=Lans.g(this,"Multiple");
				return;
			}
			//One clinic selected
			comboClinicTo.DropDownStyle=ComboBoxStyle.DropDownList;
			comboClinicTo.SetSelectedItem<Clinic>(x => x.ClinicNum==_listSelectedClinicNumsTo.First(),"");
		}

		private void butPickProvider_Click(object sender,EventArgs e){
			int selectedIndex=GetProviderIndexFromPicker()+1;//All provider combo boxes have a none option, so always add 1.
			//If the selectedIndex is 0, simply return and do not do anything.  There is no such thing as picking 'None' from the picker window.
			if(selectedIndex==0) {
				return;
			}
			UI.Button pickerButton=(UI.Button)sender;
			if(pickerButton==butPickProv) {
				comboProvider.SelectedIndex=selectedIndex;
			}
			else if(pickerButton==butPickProvTo) {
				comboProviderTo.SelectedIndex=selectedIndex;
			}
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
			FormC.ListClinics=_listClinics;
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

		///<summary>Generic combo box change event.  No matter what combo box is changed we always want to simply FillGrid() with the new choices.</summary>
		private void comboGeneric_SelectionChangeCommitted(object sender,EventArgs e) {
			FillComboBoxes();
		}

		private void comboClinicTo_SelectionChangeCommitted(object sender,EventArgs e) {
			comboClinicTo.DropDownStyle=ComboBoxStyle.DropDownList;
			_listSelectedClinicNumsTo.Clear();
			_listSelectedClinicNumsTo.Add(comboClinicTo.SelectedTag<Clinic>().ClinicNum);
		}

		private void comboClinicGlobalUpdate_SelectionChangeCommitted(object sender,EventArgs e) {
			FillComboBoxes();
		}
		
		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormFeeSchedTools_FormClosing(object sender,FormClosingEventArgs e) {
			if(DialogResult==DialogResult.OK && _changed) {
				Cursor=Cursors.WaitCursor;
				_feeCache.SaveToDb();
				Cursor=Cursors.Default;
			}
		}
	}
}