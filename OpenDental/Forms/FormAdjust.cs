/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Linq;
using CodeBase;

namespace OpenDental{
	///<summary></summary>
	public class FormAdjust : ODForm {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label labelAdditions;
		private System.Windows.Forms.Label label2;
		private System.ComponentModel.Container components = null;// Required designer variable.
		///<summary></summary>
		public bool IsNew;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.Label labelSubtractions;
		private OpenDental.UI.Button butDelete;
		private OpenDental.ValidDouble textAmount;
		private System.Windows.Forms.ListBox listTypePos;
		private System.Windows.Forms.ListBox listTypeNeg;
		private ArrayList PosIndex=new ArrayList();
		private OpenDental.ODtextBox textNote;
		private ArrayList NegIndex=new ArrayList();
		private Patient _patCur;
		private OpenDental.ValidDate textProcDate;
		private System.Windows.Forms.Label label7;
		private OpenDental.ValidDate textAdjDate;
		private Adjustment _adjustmentCur;
		private OpenDental.ValidDate textDateEntry;
		private System.Windows.Forms.Label label8;
		private OpenDental.UI.Button butPickProv;
		private ComboBox comboProv;
		private ComboBox comboClinic;
		private Label labelClinic;
		///<summary></summary>
		private DateTime dateLimit=DateTime.MinValue;
		private GroupBox groupProcedure;
		private TextBox textProcWriteoff;
		private Label label16;
		private TextBox textProcTooth;
		private Label labelProcTooth;
		private TextBox textProcProv;
		private TextBox textProcDescription;
		private TextBox textProcDate2;
		private Label labelProcRemain;
		private TextBox textProcAdjCur;
		private TextBox textProcPatPaid;
		private TextBox textProcAdj;
		private TextBox textProcInsEst;
		private TextBox textProcInsPaid;
		private TextBox textProcFee;
		private Label label13;
		private Label label12;
		private Label label11;
		private Label label10;
		private Label label9;
		private Label label14;
		private Label label15;
		private Label label17;
		private Label label18;
		private Label label19;
		private UI.Button butDetachProc;
		private UI.Button butAttachProc;
		///<summary>When true, the OK click will not let the user leave the window unless the check amount is 0.</summary>
		private bool _checkZeroAmount;
		///<summary>All positive adjustment defs.</summary>
		private List<Def> _listAdjPosCats;
		///<summary>All negative adjustment defs.</summary>
		private List<Def> _listAdjNegCats;
		///<summary>Cached list of clinics available to user. Also includes a dummy Clinic at index 0 for "none".</summary>
		private List<Clinic> _listClinics;
		///<summary>Filtered list of providers based on which clinic is selected. If no clinic is selected displays all providers. Also includes a dummy clinic at index 0 for "none"</summary>
		private List<Provider> _listProviders;
		///<summary>Used to keep track of the current clinic selected. This is because it may be a clinic that is not in _listClinics.</summary>
		private long _selectedClinicNum;
		///<summary>Instead of relying on _listProviders[comboProv.SelectedIndex] to determine the selected Provider we use this variable to store it explicitly.</summary>
		private long _selectedProvNum;
		private decimal _adjRemAmt;
		private UI.Button butEditAnyway;
		private Label labelEditAnyway;
		private bool _isTsiAdj;
		private bool _isEditAnyway;

		///<summary></summary>
		public FormAdjust(Patient patCur,Adjustment adjustmentCur,bool isTsiAdj=false){
			InitializeComponent();
			_patCur=patCur;
			_adjustmentCur=adjustmentCur;
			_isTsiAdj=isTsiAdj;
			Lan.F(this);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAdjust));
			this.label1 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.labelAdditions = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.textAdjDate = new OpenDental.ValidDate();
			this.labelSubtractions = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
			this.textAmount = new OpenDental.ValidDouble();
			this.listTypePos = new System.Windows.Forms.ListBox();
			this.listTypeNeg = new System.Windows.Forms.ListBox();
			this.textProcDate = new OpenDental.ValidDate();
			this.label7 = new System.Windows.Forms.Label();
			this.textDateEntry = new OpenDental.ValidDate();
			this.label8 = new System.Windows.Forms.Label();
			this.butPickProv = new OpenDental.UI.Button();
			this.comboProv = new System.Windows.Forms.ComboBox();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.textNote = new OpenDental.ODtextBox();
			this.groupProcedure = new System.Windows.Forms.GroupBox();
			this.textProcWriteoff = new System.Windows.Forms.TextBox();
			this.label16 = new System.Windows.Forms.Label();
			this.textProcTooth = new System.Windows.Forms.TextBox();
			this.labelProcTooth = new System.Windows.Forms.Label();
			this.textProcProv = new System.Windows.Forms.TextBox();
			this.textProcDescription = new System.Windows.Forms.TextBox();
			this.textProcDate2 = new System.Windows.Forms.TextBox();
			this.labelProcRemain = new System.Windows.Forms.Label();
			this.textProcAdjCur = new System.Windows.Forms.TextBox();
			this.textProcPatPaid = new System.Windows.Forms.TextBox();
			this.textProcAdj = new System.Windows.Forms.TextBox();
			this.textProcInsEst = new System.Windows.Forms.TextBox();
			this.textProcInsPaid = new System.Windows.Forms.TextBox();
			this.textProcFee = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.butDetachProc = new OpenDental.UI.Button();
			this.butAttachProc = new OpenDental.UI.Button();
			this.butEditAnyway = new OpenDental.UI.Button();
			this.labelEditAnyway = new System.Windows.Forms.Label();
			this.groupProcedure.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 54);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Adjustment Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(176, 396);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 16);
			this.label4.TabIndex = 3;
			this.label4.Text = "Note";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 102);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 16);
			this.label5.TabIndex = 4;
			this.label5.Text = "Amount";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelAdditions
			// 
			this.labelAdditions.Location = new System.Drawing.Point(299, 14);
			this.labelAdditions.Name = "labelAdditions";
			this.labelAdditions.Size = new System.Drawing.Size(202, 16);
			this.labelAdditions.TabIndex = 5;
			this.labelAdditions.Text = "Additions";
			this.labelAdditions.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 128);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 10;
			this.label2.Text = "Provider";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(614, 433);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 6;
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
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(614, 471);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 7;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// textAdjDate
			// 
			this.textAdjDate.Location = new System.Drawing.Point(109, 52);
			this.textAdjDate.Name = "textAdjDate";
			this.textAdjDate.Size = new System.Drawing.Size(80, 20);
			this.textAdjDate.TabIndex = 8;
			// 
			// labelSubtractions
			// 
			this.labelSubtractions.Location = new System.Drawing.Point(528, 14);
			this.labelSubtractions.Name = "labelSubtractions";
			this.labelSubtractions.Size = new System.Drawing.Size(182, 16);
			this.labelSubtractions.TabIndex = 16;
			this.labelSubtractions.Text = "Subtractions";
			this.labelSubtractions.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Location = new System.Drawing.Point(24, 469);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75, 24);
			this.butDelete.TabIndex = 17;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textAmount
			// 
			this.textAmount.Location = new System.Drawing.Point(109, 100);
			this.textAmount.MaxVal = 100000000D;
			this.textAmount.MinVal = -100000000D;
			this.textAmount.Name = "textAmount";
			this.textAmount.Size = new System.Drawing.Size(68, 20);
			this.textAmount.TabIndex = 0;
			this.textAmount.Validating += new System.ComponentModel.CancelEventHandler(this.textAmount_Validating);
			// 
			// listTypePos
			// 
			this.listTypePos.Location = new System.Drawing.Point(299, 34);
			this.listTypePos.Name = "listTypePos";
			this.listTypePos.Size = new System.Drawing.Size(202, 160);
			this.listTypePos.TabIndex = 3;
			this.listTypePos.SelectedIndexChanged += new System.EventHandler(this.listTypePos_SelectedIndexChanged);
			// 
			// listTypeNeg
			// 
			this.listTypeNeg.Location = new System.Drawing.Point(515, 34);
			this.listTypeNeg.Name = "listTypeNeg";
			this.listTypeNeg.Size = new System.Drawing.Size(206, 160);
			this.listTypeNeg.TabIndex = 4;
			this.listTypeNeg.SelectedIndexChanged += new System.EventHandler(this.listTypeNeg_SelectedIndexChanged);
			// 
			// textProcDate
			// 
			this.textProcDate.Location = new System.Drawing.Point(109, 76);
			this.textProcDate.Name = "textProcDate";
			this.textProcDate.Size = new System.Drawing.Size(80, 20);
			this.textProcDate.TabIndex = 9;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(4, 78);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(104, 16);
			this.label7.TabIndex = 18;
			this.label7.Text = "(procedure date)";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDateEntry
			// 
			this.textDateEntry.Location = new System.Drawing.Point(109, 28);
			this.textDateEntry.Name = "textDateEntry";
			this.textDateEntry.ReadOnly = true;
			this.textDateEntry.Size = new System.Drawing.Size(80, 20);
			this.textDateEntry.TabIndex = 21;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(4, 30);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(104, 16);
			this.label8.TabIndex = 20;
			this.label8.Text = "Entry Date";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butPickProv
			// 
			this.butPickProv.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickProv.Autosize = false;
			this.butPickProv.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickProv.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickProv.CornerRadius = 2F;
			this.butPickProv.Location = new System.Drawing.Point(268, 124);
			this.butPickProv.Name = "butPickProv";
			this.butPickProv.Size = new System.Drawing.Size(18, 21);
			this.butPickProv.TabIndex = 165;
			this.butPickProv.Text = "...";
			this.butPickProv.Click += new System.EventHandler(this.butPickProv_Click);
			// 
			// comboProv
			// 
			this.comboProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProv.Location = new System.Drawing.Point(109, 124);
			this.comboProv.MaxDropDownItems = 30;
			this.comboProv.Name = "comboProv";
			this.comboProv.Size = new System.Drawing.Size(158, 21);
			this.comboProv.TabIndex = 1;
			this.comboProv.SelectedIndexChanged += new System.EventHandler(this.comboProv_SelectedIndexChanged);
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(109, 149);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(177, 21);
			this.comboClinic.TabIndex = 2;
			this.comboClinic.SelectedIndexChanged += new System.EventHandler(this.comboClinic_SelectedIndexChanged);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(-7, 151);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(114, 16);
			this.labelClinic.TabIndex = 163;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textNote
			// 
			this.textNote.AcceptsTab = true;
			this.textNote.BackColor = System.Drawing.SystemColors.Window;
			this.textNote.DetectLinksEnabled = false;
			this.textNote.DetectUrls = false;
			this.textNote.Location = new System.Drawing.Point(176, 415);
			this.textNote.Name = "textNote";
			this.textNote.QuickPasteType = OpenDentBusiness.QuickPasteType.Adjustment;
			this.textNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textNote.Size = new System.Drawing.Size(355, 79);
			this.textNote.TabIndex = 5;
			this.textNote.Text = "";
			// 
			// groupProcedure
			// 
			this.groupProcedure.Controls.Add(this.butEditAnyway);
			this.groupProcedure.Controls.Add(this.textProcWriteoff);
			this.groupProcedure.Controls.Add(this.labelEditAnyway);
			this.groupProcedure.Controls.Add(this.label16);
			this.groupProcedure.Controls.Add(this.textProcTooth);
			this.groupProcedure.Controls.Add(this.labelProcTooth);
			this.groupProcedure.Controls.Add(this.textProcProv);
			this.groupProcedure.Controls.Add(this.textProcDescription);
			this.groupProcedure.Controls.Add(this.textProcDate2);
			this.groupProcedure.Controls.Add(this.labelProcRemain);
			this.groupProcedure.Controls.Add(this.textProcAdjCur);
			this.groupProcedure.Controls.Add(this.textProcPatPaid);
			this.groupProcedure.Controls.Add(this.textProcAdj);
			this.groupProcedure.Controls.Add(this.textProcInsEst);
			this.groupProcedure.Controls.Add(this.textProcInsPaid);
			this.groupProcedure.Controls.Add(this.textProcFee);
			this.groupProcedure.Controls.Add(this.label13);
			this.groupProcedure.Controls.Add(this.label12);
			this.groupProcedure.Controls.Add(this.label11);
			this.groupProcedure.Controls.Add(this.label10);
			this.groupProcedure.Controls.Add(this.label9);
			this.groupProcedure.Controls.Add(this.label14);
			this.groupProcedure.Controls.Add(this.label15);
			this.groupProcedure.Controls.Add(this.label17);
			this.groupProcedure.Controls.Add(this.label18);
			this.groupProcedure.Controls.Add(this.label19);
			this.groupProcedure.Controls.Add(this.butDetachProc);
			this.groupProcedure.Controls.Add(this.butAttachProc);
			this.groupProcedure.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupProcedure.Location = new System.Drawing.Point(104, 201);
			this.groupProcedure.Name = "groupProcedure";
			this.groupProcedure.Size = new System.Drawing.Size(615, 192);
			this.groupProcedure.TabIndex = 166;
			this.groupProcedure.TabStop = false;
			this.groupProcedure.Text = "Procedure";
			// 
			// textProcWriteoff
			// 
			this.textProcWriteoff.Location = new System.Drawing.Point(513, 39);
			this.textProcWriteoff.Name = "textProcWriteoff";
			this.textProcWriteoff.ReadOnly = true;
			this.textProcWriteoff.Size = new System.Drawing.Size(76, 20);
			this.textProcWriteoff.TabIndex = 50;
			this.textProcWriteoff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(405, 41);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(104, 16);
			this.label16.TabIndex = 49;
			this.label16.Text = "Writeoffs";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProcTooth
			// 
			this.textProcTooth.Location = new System.Drawing.Point(115, 95);
			this.textProcTooth.Name = "textProcTooth";
			this.textProcTooth.ReadOnly = true;
			this.textProcTooth.Size = new System.Drawing.Size(43, 20);
			this.textProcTooth.TabIndex = 46;
			// 
			// labelProcTooth
			// 
			this.labelProcTooth.Location = new System.Drawing.Point(9, 98);
			this.labelProcTooth.Name = "labelProcTooth";
			this.labelProcTooth.Size = new System.Drawing.Size(104, 16);
			this.labelProcTooth.TabIndex = 45;
			this.labelProcTooth.Text = "Tooth";
			this.labelProcTooth.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textProcProv
			// 
			this.textProcProv.Location = new System.Drawing.Point(115, 75);
			this.textProcProv.Name = "textProcProv";
			this.textProcProv.ReadOnly = true;
			this.textProcProv.Size = new System.Drawing.Size(76, 20);
			this.textProcProv.TabIndex = 44;
			// 
			// textProcDescription
			// 
			this.textProcDescription.Location = new System.Drawing.Point(115, 115);
			this.textProcDescription.Name = "textProcDescription";
			this.textProcDescription.ReadOnly = true;
			this.textProcDescription.Size = new System.Drawing.Size(241, 20);
			this.textProcDescription.TabIndex = 43;
			// 
			// textProcDate2
			// 
			this.textProcDate2.Location = new System.Drawing.Point(115, 55);
			this.textProcDate2.Name = "textProcDate2";
			this.textProcDate2.ReadOnly = true;
			this.textProcDate2.Size = new System.Drawing.Size(76, 20);
			this.textProcDate2.TabIndex = 42;
			// 
			// labelProcRemain
			// 
			this.labelProcRemain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelProcRemain.Location = new System.Drawing.Point(514, 166);
			this.labelProcRemain.Name = "labelProcRemain";
			this.labelProcRemain.Size = new System.Drawing.Size(73, 18);
			this.labelProcRemain.TabIndex = 41;
			this.labelProcRemain.Text = "$0.00";
			this.labelProcRemain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProcAdjCur
			// 
			this.textProcAdjCur.Location = new System.Drawing.Point(513, 139);
			this.textProcAdjCur.Name = "textProcAdjCur";
			this.textProcAdjCur.ReadOnly = true;
			this.textProcAdjCur.Size = new System.Drawing.Size(76, 20);
			this.textProcAdjCur.TabIndex = 40;
			this.textProcAdjCur.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textProcPatPaid
			// 
			this.textProcPatPaid.Location = new System.Drawing.Point(513, 119);
			this.textProcPatPaid.Name = "textProcPatPaid";
			this.textProcPatPaid.ReadOnly = true;
			this.textProcPatPaid.Size = new System.Drawing.Size(76, 20);
			this.textProcPatPaid.TabIndex = 39;
			this.textProcPatPaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textProcAdj
			// 
			this.textProcAdj.Location = new System.Drawing.Point(513, 99);
			this.textProcAdj.Name = "textProcAdj";
			this.textProcAdj.ReadOnly = true;
			this.textProcAdj.Size = new System.Drawing.Size(76, 20);
			this.textProcAdj.TabIndex = 38;
			this.textProcAdj.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textProcInsEst
			// 
			this.textProcInsEst.Location = new System.Drawing.Point(513, 79);
			this.textProcInsEst.Name = "textProcInsEst";
			this.textProcInsEst.ReadOnly = true;
			this.textProcInsEst.Size = new System.Drawing.Size(76, 20);
			this.textProcInsEst.TabIndex = 37;
			this.textProcInsEst.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textProcInsPaid
			// 
			this.textProcInsPaid.Location = new System.Drawing.Point(513, 59);
			this.textProcInsPaid.Name = "textProcInsPaid";
			this.textProcInsPaid.ReadOnly = true;
			this.textProcInsPaid.Size = new System.Drawing.Size(76, 20);
			this.textProcInsPaid.TabIndex = 36;
			this.textProcInsPaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textProcFee
			// 
			this.textProcFee.Location = new System.Drawing.Point(513, 19);
			this.textProcFee.Name = "textProcFee";
			this.textProcFee.ReadOnly = true;
			this.textProcFee.Size = new System.Drawing.Size(76, 20);
			this.textProcFee.TabIndex = 35;
			this.textProcFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(405, 141);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(104, 16);
			this.label13.TabIndex = 34;
			this.label13.Text = "This Adjustment";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(405, 167);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(104, 16);
			this.label12.TabIndex = 33;
			this.label12.Text = "= Remaining";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(382, 121);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(127, 16);
			this.label11.TabIndex = 32;
			this.label11.Text = "Patient Paid";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(405, 101);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(104, 16);
			this.label10.TabIndex = 31;
			this.label10.Text = "Adjustments";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(405, 81);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(104, 16);
			this.label9.TabIndex = 30;
			this.label9.Text = "Ins Est";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(405, 61);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(104, 16);
			this.label14.TabIndex = 29;
			this.label14.Text = "Ins Paid";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(405, 21);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(104, 16);
			this.label15.TabIndex = 28;
			this.label15.Text = "Fee";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(9, 78);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(104, 16);
			this.label17.TabIndex = 27;
			this.label17.Text = "Provider";
			this.label17.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(9, 118);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(104, 16);
			this.label18.TabIndex = 26;
			this.label18.Text = "Description";
			this.label18.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(8, 57);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(104, 16);
			this.label19.TabIndex = 25;
			this.label19.Text = "Date";
			this.label19.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butDetachProc
			// 
			this.butDetachProc.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDetachProc.Autosize = true;
			this.butDetachProc.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDetachProc.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDetachProc.CornerRadius = 4F;
			this.butDetachProc.Location = new System.Drawing.Point(99, 21);
			this.butDetachProc.Name = "butDetachProc";
			this.butDetachProc.Size = new System.Drawing.Size(75, 24);
			this.butDetachProc.TabIndex = 9;
			this.butDetachProc.Text = "Detach";
			this.butDetachProc.Click += new System.EventHandler(this.butDetachProc_Click);
			// 
			// butAttachProc
			// 
			this.butAttachProc.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAttachProc.Autosize = true;
			this.butAttachProc.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAttachProc.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAttachProc.CornerRadius = 4F;
			this.butAttachProc.Location = new System.Drawing.Point(12, 21);
			this.butAttachProc.Name = "butAttachProc";
			this.butAttachProc.Size = new System.Drawing.Size(75, 24);
			this.butAttachProc.TabIndex = 8;
			this.butAttachProc.Text = "Attach";
			this.butAttachProc.Click += new System.EventHandler(this.butAttachProc_Click);
			// 
			// butEditAnyway
			// 
			this.butEditAnyway.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEditAnyway.Autosize = true;
			this.butEditAnyway.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditAnyway.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditAnyway.CornerRadius = 4F;
			this.butEditAnyway.Location = new System.Drawing.Point(280, 143);
			this.butEditAnyway.Name = "butEditAnyway";
			this.butEditAnyway.Size = new System.Drawing.Size(75, 24);
			this.butEditAnyway.TabIndex = 167;
			this.butEditAnyway.Text = "Edit Anyway";
			this.butEditAnyway.Visible = false;
			this.butEditAnyway.Click += new System.EventHandler(this.butEditAnyway_Click);
			// 
			// labelEditAnyway
			// 
			this.labelEditAnyway.Location = new System.Drawing.Point(24, 141);
			this.labelEditAnyway.Name = "labelEditAnyway";
			this.labelEditAnyway.Size = new System.Drawing.Size(250, 28);
			this.labelEditAnyway.TabIndex = 168;
			this.labelEditAnyway.Text = "This adjustment is attached to a procedure and should not be edited";
			this.labelEditAnyway.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelEditAnyway.Visible = false;
			// 
			// FormAdjust
			// 
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(731, 528);
			this.Controls.Add(this.groupProcedure);
			this.Controls.Add(this.butPickProv);
			this.Controls.Add(this.comboProv);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.textDateEntry);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.textProcDate);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.listTypeNeg);
			this.Controls.Add(this.listTypePos);
			this.Controls.Add(this.textAmount);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.labelSubtractions);
			this.Controls.Add(this.textAdjDate);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.labelAdditions);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAdjust";
			this.ShowInTaskbar = false;
			this.Text = "Edit Adjustment";
			this.Load += new System.EventHandler(this.FormAdjust_Load);
			this.groupProcedure.ResumeLayout(false);
			this.groupProcedure.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormAdjust_Load(object sender, System.EventArgs e) {
			if(IsNew){
				if(!Security.IsAuthorized(Permissions.AdjustmentCreate,true)) {//Date not checked here.  Message will show later.
					if(!Security.IsAuthorized(Permissions.AdjustmentEditZero,true)) {//Let user create an adjustment of zero if they have this perm.
						MessageBox.Show(Lans.g("Security","Not authorized for")+"\r\n"+GroupPermissions.GetDesc(Permissions.AdjustmentCreate));
						DialogResult=DialogResult.Cancel;
						return;
					}
					//Make sure amount is 0 after OK click.
					_checkZeroAmount=true;
				}
			}
			else{
				if(!Security.IsAuthorized(Permissions.AdjustmentEdit,_adjustmentCur.AdjDate)){
					butOK.Enabled=false;
					butDelete.Enabled=false;
					//User can't edit but has edit zero amount perm.  Allow delete only if date is today.
					if(Security.IsAuthorized(Permissions.AdjustmentEditZero,true) 
						&& _adjustmentCur.AdjAmt==0
						&& _adjustmentCur.DateEntry.Date==MiscData.GetNowDateTime().Date) 
					{
						butDelete.Enabled=true;
					}
				}
				//Do not let the user change the adjustment type if the current adjustment is a "discount plan" adjustment type.
				if(Defs.GetValue(DefCat.AdjTypes,_adjustmentCur.AdjType)=="dp") {
					labelAdditions.Text=Lan.g(this,"Discount Plan")+": "+Defs.GetName(DefCat.AdjTypes,_adjustmentCur.AdjType);
					labelSubtractions.Visible=false;
					listTypePos.Visible=false;
					listTypeNeg.Visible=false;
				}
			}
			textDateEntry.Text=_adjustmentCur.DateEntry.ToShortDateString();
			textAdjDate.Text=_adjustmentCur.AdjDate.ToShortDateString();
			textProcDate.Text=_adjustmentCur.ProcDate.ToShortDateString();
			if(Defs.GetValue(DefCat.AdjTypes,_adjustmentCur.AdjType)=="+"){//pos
				textAmount.Text=_adjustmentCur.AdjAmt.ToString("F");
			}
			else if(Defs.GetValue(DefCat.AdjTypes,_adjustmentCur.AdjType)=="-"){//neg
				textAmount.Text=(-_adjustmentCur.AdjAmt).ToString("F");//shows without the neg sign
			}
			else if(Defs.GetValue(DefCat.AdjTypes,_adjustmentCur.AdjType)=="dp") {//Discount Plan (neg)
				textAmount.Text=(-_adjustmentCur.AdjAmt).ToString("F");//shows without the neg sign
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				labelClinic.Visible=false;
				comboClinic.Visible=false;
				_listClinics=new List<Clinic>();
				_selectedClinicNum=0;
			}
			else {
				_listClinics=new List<Clinic>() { new Clinic() { Abbr=Lan.g(this,"None") } }; //Seed with "None"
				Clinics.GetForUserod(Security.CurUser).ForEach(x => _listClinics.Add(x));//do not re-organize from cache. They could either be alphabetizeded or sorted by item order.
				_listClinics.ForEach(x => comboClinic.Items.Add(x.Abbr));
				_selectedClinicNum=_adjustmentCur.ClinicNum;
				comboClinic.IndexSelectOrSetText(_listClinics.FindIndex(x => x.ClinicNum==_selectedClinicNum),() => { return Clinics.GetAbbr(_selectedClinicNum); });
			}
			_selectedProvNum=_adjustmentCur.ProvNum;
			comboProv.SelectedIndex=-1;
			FillComboProvHyg();
			if(_adjustmentCur.ProcNum!=0 && PrefC.GetInt(PrefName.RigorousAdjustments)==(int)RigorousAdjustments.EnforceFully) {
				comboProv.Enabled=false;
				butPickProv.Enabled=false;
				comboClinic.Enabled=false;
				if(Security.IsAuthorized(Permissions.Setup,true)) {
					labelEditAnyway.Visible=true;
					butEditAnyway.Visible=true;
				}
			}
			//prevents FillProcedure from being called too many times.  Event handlers hooked back up after the lists are filled.
			listTypeNeg.SelectedIndexChanged-=listTypeNeg_SelectedIndexChanged;
			listTypePos.SelectedIndexChanged-=listTypePos_SelectedIndexChanged;
			List<Def> adjCat = Defs.GetDefsForCategory(DefCat.AdjTypes,true);
			//Positive adjustment types
			_listAdjPosCats=adjCat.FindAll(x => x.ItemValue=="+");
			_listAdjPosCats.ForEach(x => listTypePos.Items.Add(x.ItemName));
			listTypePos.SelectedIndex=_listAdjPosCats.FindIndex(x => x.DefNum==_adjustmentCur.AdjType);//can be -1
			//Negative adjustment types
			_listAdjNegCats=adjCat.FindAll(x => x.ItemValue=="-");
			_listAdjNegCats.ForEach(x => listTypeNeg.Items.Add(x.ItemName));
			listTypeNeg.SelectedIndex=_listAdjNegCats.FindIndex(x => x.DefNum==_adjustmentCur.AdjType);//can be -1
			listTypeNeg.SelectedIndexChanged+=listTypeNeg_SelectedIndexChanged;
			listTypePos.SelectedIndexChanged+=listTypePos_SelectedIndexChanged;
			FillProcedure();
			textNote.Text=_adjustmentCur.AdjNote;
		}

		private void listTypePos_SelectedIndexChanged(object sender,System.EventArgs e) {
			if(listTypePos.SelectedIndex>-1) {
				listTypeNeg.SelectedIndex=-1;
				FillProcedure();
			}
		}

		private void listTypeNeg_SelectedIndexChanged(object sender,System.EventArgs e) {
			if(listTypeNeg.SelectedIndex>-1) {
				listTypePos.SelectedIndex=-1;
				FillProcedure();
			}
		}

		private void textAmount_Validating(object sender,CancelEventArgs e) {
			FillProcedure();
		}

		private void butPickProv_Click(object sender,EventArgs e) {
			FormProviderPick FormPP = new FormProviderPick(_listProviders);
			FormPP.SelectedProvNum=_selectedProvNum;
			FormPP.ShowDialog();
			if(FormPP.DialogResult!=DialogResult.OK) {
				return;
			}
			_selectedProvNum=FormPP.SelectedProvNum;
			comboProv.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvNum),() => { return Providers.GetAbbr(_selectedProvNum); });
		}

		private void comboClinic_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboClinic.SelectedIndex>-1) {
				_selectedClinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
			}
			FillComboProvHyg();
		}

		private void comboProv_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboProv.SelectedIndex>-1) {
				_selectedProvNum=_listProviders[comboProv.SelectedIndex].ProvNum;
			}
		}

		///<summary>Fills combo provider based on which clinic is selected and attempts to preserve provider selection if any.</summary>
		private void FillComboProvHyg() {
			if(comboProv.SelectedIndex>-1) {//valid prov selected, not none or nothing.
				_selectedProvNum = _listProviders[comboProv.SelectedIndex].ProvNum;
			}
			_listProviders=Providers.GetProvsForClinic(_selectedClinicNum);
			comboProv.Items.Clear();
			_listProviders.ForEach(x => comboProv.Items.Add(x.Abbr));
			comboProv.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvNum),() => { return Providers.GetAbbr(_selectedProvNum); });
		}

		private void FillProcedure(){
			if(_adjustmentCur.ProcNum==0) {
				textProcDate2.Text="";
				textProcProv.Text="";
				textProcTooth.Text="";
				textProcDescription.Text="";
				textProcFee.Text="";
				textProcWriteoff.Text="";
				textProcInsPaid.Text="";
				textProcInsEst.Text="";
				textProcAdj.Text="";
				textProcPatPaid.Text="";
				textProcAdjCur.Text="";
				labelProcRemain.Text="";
				_adjRemAmt=0;
				return;
			}
			Procedure procCur=Procedures.GetOneProc(_adjustmentCur.ProcNum,false);
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(procCur.PatNum);
			textProcDate.Text=procCur.ProcDate.ToShortDateString();
			textProcDate2.Text=procCur.ProcDate.ToShortDateString();
			textProcProv.Text=Providers.GetAbbr(procCur.ProvNum);
			textProcTooth.Text=Tooth.ToInternat(procCur.ToothNum);
			textProcDescription.Text=ProcedureCodes.GetProcCode(procCur.CodeNum).Descript;
			double procWO=-ClaimProcs.ProcWriteoff(listClaimProcs,procCur.ProcNum);
			double procInsPaid=-ClaimProcs.ProcInsPay(listClaimProcs,procCur.ProcNum);
			double procInsEst=-ClaimProcs.ProcEstNotReceived(listClaimProcs,procCur.ProcNum);
			double procAdj=Adjustments.Refresh(procCur.PatNum).Where(x => x.ProcNum==procCur.ProcNum && x.AdjNum!=_adjustmentCur.AdjNum).Select(x => x.AdjAmt).Sum();
			double procPatPaid=-PaySplits.GetTotForProc(procCur);
			double procFee=Procedures.CalculateProcCharge(procCur);
			textProcFee.Text=procFee.ToString("F");
			textProcWriteoff.Text=procWO==0?"":procWO.ToString("F");
			textProcInsPaid.Text=procInsPaid==0?"":procInsPaid.ToString("F");
			textProcInsEst.Text=procInsEst==0?"":procInsEst.ToString("F");
			textProcAdj.Text=procAdj==0?"":procAdj.ToString("F");
			textProcPatPaid.Text=procPatPaid==0?"":procPatPaid.ToString("F");
			double procAdjCur=0;
			if(textAmount.errorProvider1.GetError(textAmount)==""){
				if(listTypePos.SelectedIndex>-1){//pos
					procAdjCur=PIn.Double(textAmount.Text);
				}
				else if(listTypeNeg.SelectedIndex>-1 || Defs.GetValue(DefCat.AdjTypes,_adjustmentCur.AdjType)=="dp"){//neg or discount plan
					procAdjCur=-PIn.Double(textAmount.Text);
				}
			}
			textProcAdjCur.Text=procAdjCur==0?"":procAdjCur.ToString("F");
			//most of these are negative values, so add
			_adjRemAmt = new[] { procFee,procWO,procInsPaid,procInsEst,procAdj,procPatPaid,procAdjCur }.Sum(x => (decimal)x);
			labelProcRemain.Text=_adjRemAmt.ToString("c");
		}

		private void butAttachProc_Click(object sender, System.EventArgs e) {
			FormProcSelect FormPS=new FormProcSelect(_adjustmentCur.PatNum,false);
			FormPS.ShowDialog();
			if(FormPS.DialogResult!=DialogResult.OK){
				return;
			}
			if(PrefC.GetInt(PrefName.RigorousAdjustments)<2) {//Enforce Linking
				_selectedProvNum=FormPS.ListSelectedProcs[0].ProvNum;
				_selectedClinicNum=FormPS.ListSelectedProcs[0].ClinicNum;
				comboProv.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvNum),() => { return Providers.GetAbbr(_selectedProvNum); });
				comboClinic.IndexSelectOrSetText(_listClinics.FindIndex(x => x.ClinicNum==_selectedClinicNum),() => { return Clinics.GetAbbr(_selectedClinicNum); });
				if(PrefC.GetInt(PrefName.RigorousAdjustments)==(int)RigorousAdjustments.EnforceFully && !_isEditAnyway) {
					if(Security.IsAuthorized(Permissions.Setup,true)) {
						labelEditAnyway.Visible=true;
						butEditAnyway.Visible=true;
					}
					comboProv.Enabled=false;//Don't allow changing if enforce fully
					butPickProv.Enabled=false;
					comboClinic.Enabled=false;
				}
			}
			_adjustmentCur.ProcNum=FormPS.ListSelectedProcs[0].ProcNum;
			FillProcedure();
			textProcDate.Text=FormPS.ListSelectedProcs[0].ProcDate.ToShortDateString();
		}

		private void butDetachProc_Click(object sender, System.EventArgs e) {
			comboProv.Enabled=true;
			butPickProv.Enabled=true;
			comboClinic.Enabled=true;
			labelEditAnyway.Visible=false;
			butEditAnyway.Visible=false;
			_adjustmentCur.ProcNum=0;
			FillProcedure();
		}

		private void butEditAnyway_Click(object sender,EventArgs e) {
			_isEditAnyway=true;
			comboClinic.Enabled=true;
			comboProv.Enabled=true;
			butPickProv.Enabled=true;
			labelEditAnyway.Visible=false;
			butEditAnyway.Visible=false;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			bool isDiscountPlanAdj=(Defs.GetValue(DefCat.AdjTypes,_adjustmentCur.AdjType)=="dp");
			if( textAdjDate.errorProvider1.GetError(textAdjDate)!=""
				|| textProcDate.errorProvider1.GetError(textProcDate)!=""
				|| textAmount.errorProvider1.GetError(textAmount)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(PIn.Date(textAdjDate.Text) > DateTime.Today && !PrefC.GetBool(PrefName.FutureTransDatesAllowed)) {
				MsgBox.Show(this,"Adjustment date can not be in the future.");
				return;
			}
			if(textAmount.Text==""){
				MessageBox.Show(Lan.g(this,"Please enter an amount."));	
				return;
			}
			if(!isDiscountPlanAdj && listTypeNeg.SelectedIndex==-1 && listTypePos.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a type first.");
				return;
			}
			if(PrefC.GetInt(PrefName.RigorousAdjustments)==0 && _adjustmentCur.ProcNum==0) {
				MsgBox.Show(this,"You must attach a procedure to the adjustment.");
				return;
			}
			if(_adjRemAmt < 0) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Remaining amount is negative.  Continue?","Overpaid Procedure Warning")) {
					return;
				}
			}
			if(IsNew){
				//prevents backdating of initial adjustment
				if(!Security.IsAuthorized(Permissions.AdjustmentCreate,PIn.Date(textAdjDate.Text),true)){//Give message later.
					if(!_checkZeroAmount) {//Let user create as long as Amount is zero and has edit zero permissions.  This was checked on load.
						MessageBox.Show(Lans.g("Security","Not authorized for")+"\r\n"+GroupPermissions.GetDesc(Permissions.AdjustmentCreate));
						return;
					}
				}
			}
			else{
				//Editing an old entry will already be blocked if the date was too old, and user will not be able to click OK button
				//This catches it if user changed the date to be older.
				if(!Security.IsAuthorized(Permissions.AdjustmentEdit,PIn.Date(textAdjDate.Text))){
					return;
				}
			}
			//DateEntry not allowed to change
			DateTime datePreviousChange=_adjustmentCur.SecDateTEdit;
			_adjustmentCur.AdjDate=PIn.Date(textAdjDate.Text);
			_adjustmentCur.ProcDate=PIn.Date(textProcDate.Text);
			_adjustmentCur.ProvNum=_selectedProvNum;
			_adjustmentCur.ClinicNum=_selectedClinicNum;
			if(listTypePos.SelectedIndex!=-1) {
				_adjustmentCur.AdjType=_listAdjPosCats[listTypePos.SelectedIndex].DefNum;
				_adjustmentCur.AdjAmt=PIn.Double(textAmount.Text);
			}
			if(listTypeNeg.SelectedIndex!=-1) {
				_adjustmentCur.AdjType=_listAdjNegCats[listTypeNeg.SelectedIndex].DefNum;
				_adjustmentCur.AdjAmt=-PIn.Double(textAmount.Text);
			}
			if(isDiscountPlanAdj) {
				//AdjustmentCur.AdjType is already set to a "discount plan" adj type.
				_adjustmentCur.AdjAmt=-PIn.Double(textAmount.Text);
			}
			if(_checkZeroAmount && _adjustmentCur.AdjAmt!=0) {
				MsgBox.Show(this,"Amount has to be 0.00 due to security permission.");
				return;
			}
			_adjustmentCur.AdjNote=textNote.Text;
			try{
				if(IsNew) {
					Adjustments.Insert(_adjustmentCur);
					SecurityLogs.MakeLogEntry(Permissions.AdjustmentCreate,_adjustmentCur.PatNum,
						_patCur.GetNameLF()+", "
						+_adjustmentCur.AdjAmt.ToString("c"));
					if(_isTsiAdj) {
						//insert tsitranslog for this transaction so the ODService won't send it to Transworld.  _isTsiAdj means Transworld received a payment on
						//behalf of this guar and took a percentage and send the rest to the office for the account.  This will result in a payment being entered
						//into the account, having been received from Transworld, and an adjustment to account for Transorld's cut.
						PatAging patAgingCur=Patients.GetAgingListFromGuarNums(new List<long>() { _patCur.Guarantor }).FirstOrDefault();//should only ever be 1
						double logAmt=patAgingCur.ListTsiLogs.FindAll(x => x.FKeyType==TsiFKeyType.Adjustment && x.FKey==_adjustmentCur.AdjNum).Sum(x => x.TransAmt);
						if(!_adjustmentCur.AdjAmt.IsEqual(logAmt)) {
							TsiTransLog logCur=new TsiTransLog() {
								PatNum=patAgingCur.PatNum,
								UserNum=Security.CurUser.UserNum,
								TransType=TsiTransType.None,
								//TransDateTime=DateTime.Now,//set on insert, not editable by user
								//DemandType=TsiDemandType.Accelerator,//only valid for placement msgs
								//ServiceCode=TsiServiceCode.Diplomatic,//only valid for placement msgs
								ClientId=patAgingCur.ListTsiLogs.FirstOrDefault()?.ClientId??"",//can be blank, not used since this isn't really sent to Transworld
								TransAmt=_adjustmentCur.AdjAmt-logAmt,
								AccountBalance=patAgingCur.AmountDue+_adjustmentCur.AdjAmt-logAmt,
								FKeyType=TsiFKeyType.Adjustment,
								FKey=_adjustmentCur.AdjNum,
								RawMsgText="This was not a message sent to Transworld.  This adjustment was entered due to a payment received from Transworld.",
								//TransJson=""//only valid for placement msgs
							};
							TsiTransLogs.InsertMany(new List<TsiTransLog>() { logCur });
						}
					}
				}
				else {
					Adjustments.Update(_adjustmentCur);
					SecurityLogs.MakeLogEntry(Permissions.AdjustmentEdit,_adjustmentCur.PatNum,_patCur.GetNameLF()+", "+_adjustmentCur.AdjAmt.ToString("c"),0
						,datePreviousChange);
				}
			}
			catch(Exception ex){//even though it doesn't currently throw any exceptions
				MessageBox.Show(ex.Message);
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(IsNew){
				DialogResult=DialogResult.Cancel;
			}
			else{
				SecurityLogs.MakeLogEntry(Permissions.AdjustmentEdit,_adjustmentCur.PatNum
					,"Delete for patient: "+_patCur.GetNameLF()+", "+_adjustmentCur.AdjAmt.ToString("c"),0,_adjustmentCur.SecDateTEdit);
				Adjustments.Delete(_adjustmentCur);
				DialogResult=DialogResult.OK;
			}
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}

	///<summary></summary>
	public struct AdjustmentItem{
		///<summary></summary>
		public string ItemText;
		///<summary></summary>
		public int ItemIndex;
	}

}
