using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using CodeBase;

namespace OpenDental
{
	/// <summary>
	/// Summary description for FormPaySplitEdit.
	/// </summary>
	public class FormPaySplitEdit : ODForm
	{
		#region Designer variables
		private OpenDental.UI.Button ButCancel;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butRemainder;
		private OpenDental.UI.Button butAttachProc;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.Button butDetachProc;
		private OpenDental.UI.Button butPickProv;
		private UI.Button butDetachPrepay;
		private UI.Button butAttachPrepay;
		private UI.Button butEditAnyway;		
		private System.Windows.Forms.CheckBox checkPayPlan;		
		private System.Windows.Forms.CheckBox checkPatOtherFam;
		private ComboBox comboUnearnedTypes;
		private ComboBox comboProvider;
		private ComboBox comboClinic;
		private System.ComponentModel.Container components = null;// Required designer variable.
		private GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupPatient;
		private System.Windows.Forms.GroupBox groupProcedure;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private Label label14;
		private System.Windows.Forms.Label label15;
		private Label label16;
		private Label label17;
		private Label label18;
		private Label label19;
		private Label label20;
		private Label label21;
		private Label label22;
		private Label labelClinic;
		private Label labelEditAnyway;
		private Label labelPrePayWarning;
		private Label textPrePaidRemain;
		private System.Windows.Forms.Label labelAmount;
		private System.Windows.Forms.Label labelProcRemain;
		private System.Windows.Forms.Label labelProcTooth;
		private System.Windows.Forms.Label labelRemainder;
		private System.Windows.Forms.ListBox listPatient;
		private System.Windows.Forms.TextBox textProcFee;
		private System.Windows.Forms.TextBox textProcInsPaid;
		private System.Windows.Forms.TextBox textProcInsEst;
		private System.Windows.Forms.TextBox textProcAdj;
		private System.Windows.Forms.TextBox textProcPrevPaid;
		private System.Windows.Forms.TextBox textPatient;
		private System.Windows.Forms.TextBox textProcDate2;
		private System.Windows.Forms.TextBox textProcDescription;
		private System.Windows.Forms.TextBox textProcProv;
		private System.Windows.Forms.TextBox textProcTooth;
		private System.Windows.Forms.TextBox textProcPaidHere;
		private TextBox textProcWriteoff;
		private TextBox textPrePayType;
		private TextBox textPrePayDate;		
		private TextBox textPrePaidHere;
		private TextBox textPrePayAmt;
		private TextBox textPrePaidElsewhere;
		private OpenDental.ValidDate textDatePay;
		private OpenDental.ValidDate textDateEntry;
		private OpenDental.ValidDouble textAmount;
		#endregion
		#region Public variables
		///<summary></summary>
		public bool IsNew;
		///<summary>The value needed to make the splits balance.</summary>
		public double Remain;
		///<summary>Used to figure out what procedures have amounts left due on them when attaching this splits to a proc. 
		///Splits from the current payment are also included in this calculation.</summary>
		public List<PaySplit> ListSplitsCur;
		///<summary></summary>
		public PaySplit PaySplitCur;
		///<summary>PaySplit associations for PaySplitCur</summary>
		public PaySplits.PaySplitAssociated SplitAssociated;
		///<summary>List of PaySplitAssociated for the current payment.</summary>
		public List<PaySplits.PaySplitAssociated> ListPaySplitAssociated;
		#endregion
		#region _private variables
		private bool _isEditAnyway;
		///<summary>Cached list of clinics available to user. Also includes a dummy Clinic at index 0 for "none".</summary>
		private List<Clinic> _listClinics;
		private decimal _remainAmt;
		private double ProcPaidHere;
		private double ProcFee;
		private double ProcWriteoff;
		private double ProcInsPaid;
		private double ProcInsEst;
		private double ProcAdj;
		private double ProcPrevPaid;	
		private Family _famCur;	
		///<summary>Filtered list of providers based on which clinic is selected. If no clinic is selected displays all providers. Also includes a dummy clinic at index 0 for "none"</summary>
		private List<Provider> _listProviders;
		private PaySplit _paySplitCopy;
		private Procedure ProcCur;
		private List<Def> _listPaySplitUnearnedTypeDefs;
		///<summary>True if the payment for this paysplit is an income transfer.</summary>
		private bool _isIncomeTransfer;
		#endregion
		///<summary>The PayPlanCharge that this paysplit is linked to. May be zero if the paysplit is not attached to a payplan or there are no charges
		///due for the payplan.</summary>
		public long PayPlanChargeNum {
			get;set;
		}

		public FormPaySplitEdit(Family famCur,bool isIncomeTransfer){
			InitializeComponent();
			_famCur=famCur;
			_isIncomeTransfer=isIncomeTransfer;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPaySplitEdit));
			this.labelRemainder = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.listPatient = new System.Windows.Forms.ListBox();
			this.labelAmount = new System.Windows.Forms.Label();
			this.checkPayPlan = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textPatient = new System.Windows.Forms.TextBox();
			this.checkPatOtherFam = new System.Windows.Forms.CheckBox();
			this.groupPatient = new System.Windows.Forms.GroupBox();
			this.groupProcedure = new System.Windows.Forms.GroupBox();
			this.textProcWriteoff = new System.Windows.Forms.TextBox();
			this.label16 = new System.Windows.Forms.Label();
			this.textProcTooth = new System.Windows.Forms.TextBox();
			this.labelProcTooth = new System.Windows.Forms.Label();
			this.textProcProv = new System.Windows.Forms.TextBox();
			this.textProcDescription = new System.Windows.Forms.TextBox();
			this.textProcDate2 = new System.Windows.Forms.TextBox();
			this.labelProcRemain = new System.Windows.Forms.Label();
			this.textProcPaidHere = new System.Windows.Forms.TextBox();
			this.textProcPrevPaid = new System.Windows.Forms.TextBox();
			this.textProcAdj = new System.Windows.Forms.TextBox();
			this.textProcInsEst = new System.Windows.Forms.TextBox();
			this.textProcInsPaid = new System.Windows.Forms.TextBox();
			this.textProcFee = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.butDetachProc = new OpenDental.UI.Button();
			this.butAttachProc = new OpenDental.UI.Button();
			this.label15 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.textDateEntry = new OpenDental.ValidDate();
			this.textDatePay = new OpenDental.ValidDate();
			this.butDelete = new OpenDental.UI.Button();
			this.textAmount = new OpenDental.ValidDouble();
			this.butRemainder = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.ButCancel = new OpenDental.UI.Button();
			this.comboUnearnedTypes = new System.Windows.Forms.ComboBox();
			this.comboProvider = new System.Windows.Forms.ComboBox();
			this.butPickProv = new OpenDental.UI.Button();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.labelPrePayWarning = new System.Windows.Forms.Label();
			this.textPrePaidElsewhere = new System.Windows.Forms.TextBox();
			this.label21 = new System.Windows.Forms.Label();
			this.textPrePaidRemain = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.textPrePaidHere = new System.Windows.Forms.TextBox();
			this.label20 = new System.Windows.Forms.Label();
			this.textPrePayAmt = new System.Windows.Forms.TextBox();
			this.label19 = new System.Windows.Forms.Label();
			this.textPrePayType = new System.Windows.Forms.TextBox();
			this.label18 = new System.Windows.Forms.Label();
			this.textPrePayDate = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.butDetachPrepay = new OpenDental.UI.Button();
			this.butAttachPrepay = new OpenDental.UI.Button();
			this.butEditAnyway = new OpenDental.UI.Button();
			this.labelEditAnyway = new System.Windows.Forms.Label();
			this.groupPatient.SuspendLayout();
			this.groupProcedure.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelRemainder
			// 
			this.labelRemainder.Location = new System.Drawing.Point(5, 336);
			this.labelRemainder.Name = "labelRemainder";
			this.labelRemainder.Size = new System.Drawing.Size(119, 88);
			this.labelRemainder.TabIndex = 5;
			this.labelRemainder.Text = "The Remainder button will calculate the value needed to make the splits balance.";
			this.labelRemainder.Visible = false;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(33, 169);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(95, 16);
			this.label5.TabIndex = 10;
			this.label5.Text = "Provider";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// listPatient
			// 
			this.listPatient.Location = new System.Drawing.Point(11, 34);
			this.listPatient.Name = "listPatient";
			this.listPatient.Size = new System.Drawing.Size(192, 108);
			this.listPatient.TabIndex = 3;
			this.listPatient.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listPatient_MouseDown);
			// 
			// labelAmount
			// 
			this.labelAmount.Location = new System.Drawing.Point(23, 96);
			this.labelAmount.Name = "labelAmount";
			this.labelAmount.Size = new System.Drawing.Size(104, 16);
			this.labelAmount.TabIndex = 15;
			this.labelAmount.Text = "Amount";
			this.labelAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkPayPlan
			// 
			this.checkPayPlan.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPayPlan.Location = new System.Drawing.Point(257, 511);
			this.checkPayPlan.Name = "checkPayPlan";
			this.checkPayPlan.Size = new System.Drawing.Size(198, 18);
			this.checkPayPlan.TabIndex = 20;
			this.checkPayPlan.Text = "Attached to Payment Plan";
			this.checkPayPlan.Click += new System.EventHandler(this.checkPayPlan_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(127, 16);
			this.label1.TabIndex = 23;
			this.label1.Text = "Payment Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textPatient
			// 
			this.textPatient.Location = new System.Drawing.Point(11, 33);
			this.textPatient.Name = "textPatient";
			this.textPatient.Size = new System.Drawing.Size(238, 20);
			this.textPatient.TabIndex = 111;
			// 
			// checkPatOtherFam
			// 
			this.checkPatOtherFam.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPatOtherFam.Location = new System.Drawing.Point(11, 15);
			this.checkPatOtherFam.Name = "checkPatOtherFam";
			this.checkPatOtherFam.Size = new System.Drawing.Size(192, 17);
			this.checkPatOtherFam.TabIndex = 110;
			this.checkPatOtherFam.TabStop = false;
			this.checkPatOtherFam.Text = "Is from another family";
			this.checkPatOtherFam.Click += new System.EventHandler(this.checkPatOtherFam_Click);
			// 
			// groupPatient
			// 
			this.groupPatient.Controls.Add(this.listPatient);
			this.groupPatient.Controls.Add(this.textPatient);
			this.groupPatient.Controls.Add(this.checkPatOtherFam);
			this.groupPatient.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupPatient.Location = new System.Drawing.Point(480, 3);
			this.groupPatient.Name = "groupPatient";
			this.groupPatient.Size = new System.Drawing.Size(265, 157);
			this.groupPatient.TabIndex = 112;
			this.groupPatient.TabStop = false;
			this.groupPatient.Text = "Patient";
			// 
			// groupProcedure
			// 
			this.groupProcedure.Controls.Add(this.textProcWriteoff);
			this.groupProcedure.Controls.Add(this.label16);
			this.groupProcedure.Controls.Add(this.textProcTooth);
			this.groupProcedure.Controls.Add(this.labelProcTooth);
			this.groupProcedure.Controls.Add(this.textProcProv);
			this.groupProcedure.Controls.Add(this.textProcDescription);
			this.groupProcedure.Controls.Add(this.textProcDate2);
			this.groupProcedure.Controls.Add(this.labelProcRemain);
			this.groupProcedure.Controls.Add(this.textProcPaidHere);
			this.groupProcedure.Controls.Add(this.textProcPrevPaid);
			this.groupProcedure.Controls.Add(this.textProcAdj);
			this.groupProcedure.Controls.Add(this.textProcInsEst);
			this.groupProcedure.Controls.Add(this.textProcInsPaid);
			this.groupProcedure.Controls.Add(this.textProcFee);
			this.groupProcedure.Controls.Add(this.label13);
			this.groupProcedure.Controls.Add(this.label12);
			this.groupProcedure.Controls.Add(this.label11);
			this.groupProcedure.Controls.Add(this.label10);
			this.groupProcedure.Controls.Add(this.label9);
			this.groupProcedure.Controls.Add(this.label8);
			this.groupProcedure.Controls.Add(this.label6);
			this.groupProcedure.Controls.Add(this.label4);
			this.groupProcedure.Controls.Add(this.label3);
			this.groupProcedure.Controls.Add(this.label2);
			this.groupProcedure.Controls.Add(this.butDetachProc);
			this.groupProcedure.Controls.Add(this.butAttachProc);
			this.groupProcedure.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupProcedure.Location = new System.Drawing.Point(130, 199);
			this.groupProcedure.Name = "groupProcedure";
			this.groupProcedure.Size = new System.Drawing.Size(615, 192);
			this.groupProcedure.TabIndex = 113;
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
			// textProcPaidHere
			// 
			this.textProcPaidHere.Location = new System.Drawing.Point(513, 139);
			this.textProcPaidHere.Name = "textProcPaidHere";
			this.textProcPaidHere.ReadOnly = true;
			this.textProcPaidHere.Size = new System.Drawing.Size(76, 20);
			this.textProcPaidHere.TabIndex = 40;
			this.textProcPaidHere.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textProcPrevPaid
			// 
			this.textProcPrevPaid.Location = new System.Drawing.Point(513, 119);
			this.textProcPrevPaid.Name = "textProcPrevPaid";
			this.textProcPrevPaid.ReadOnly = true;
			this.textProcPrevPaid.Size = new System.Drawing.Size(76, 20);
			this.textProcPrevPaid.TabIndex = 39;
			this.textProcPrevPaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
			this.label13.Text = "This Payment Split";
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
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(405, 61);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(104, 16);
			this.label8.TabIndex = 29;
			this.label8.Text = "Ins Paid";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(405, 21);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(104, 16);
			this.label6.TabIndex = 28;
			this.label6.Text = "Fee";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(9, 78);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(104, 16);
			this.label4.TabIndex = 27;
			this.label4.Text = "Provider";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(9, 118);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 16);
			this.label3.TabIndex = 26;
			this.label3.Text = "Description";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 57);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 16);
			this.label2.TabIndex = 25;
			this.label2.Text = "Date";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(1, 24);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(127, 16);
			this.label15.TabIndex = 115;
			this.label15.Text = "Entry Date";
			this.label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(3, 121);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(124, 16);
			this.label17.TabIndex = 116;
			this.label17.Text = "Unearned Type";
			this.label17.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDateEntry
			// 
			this.textDateEntry.Location = new System.Drawing.Point(129, 22);
			this.textDateEntry.Name = "textDateEntry";
			this.textDateEntry.ReadOnly = true;
			this.textDateEntry.Size = new System.Drawing.Size(92, 20);
			this.textDateEntry.TabIndex = 114;
			// 
			// textDatePay
			// 
			this.textDatePay.Location = new System.Drawing.Point(129, 46);
			this.textDatePay.Name = "textDatePay";
			this.textDatePay.ReadOnly = true;
			this.textDatePay.Size = new System.Drawing.Size(92, 20);
			this.textDatePay.TabIndex = 22;
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
			this.butDelete.Location = new System.Drawing.Point(15, 505);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(85, 24);
			this.butDelete.TabIndex = 21;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textAmount
			// 
			this.textAmount.Location = new System.Drawing.Point(129, 94);
			this.textAmount.MaxVal = 100000000D;
			this.textAmount.MinVal = -100000000D;
			this.textAmount.Name = "textAmount";
			this.textAmount.Size = new System.Drawing.Size(77, 20);
			this.textAmount.TabIndex = 1;
			this.textAmount.Validating += new System.ComponentModel.CancelEventHandler(this.textAmount_Validating);
			// 
			// butRemainder
			// 
			this.butRemainder.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRemainder.Autosize = true;
			this.butRemainder.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRemainder.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRemainder.CornerRadius = 4F;
			this.butRemainder.Location = new System.Drawing.Point(5, 304);
			this.butRemainder.Name = "butRemainder";
			this.butRemainder.Size = new System.Drawing.Size(92, 24);
			this.butRemainder.TabIndex = 7;
			this.butRemainder.Text = "&Remainder";
			this.butRemainder.Visible = false;
			this.butRemainder.Click += new System.EventHandler(this.butRemainder_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(589, 505);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 5;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// ButCancel
			// 
			this.ButCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.ButCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButCancel.Autosize = true;
			this.ButCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.ButCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.ButCancel.CornerRadius = 4F;
			this.ButCancel.Location = new System.Drawing.Point(670, 505);
			this.ButCancel.Name = "ButCancel";
			this.ButCancel.Size = new System.Drawing.Size(75, 24);
			this.ButCancel.TabIndex = 6;
			this.ButCancel.Text = "&Cancel";
			this.ButCancel.Click += new System.EventHandler(this.ButCancel_Click);
			// 
			// comboUnearnedTypes
			// 
			this.comboUnearnedTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboUnearnedTypes.FormattingEnabled = true;
			this.comboUnearnedTypes.Location = new System.Drawing.Point(129, 118);
			this.comboUnearnedTypes.Name = "comboUnearnedTypes";
			this.comboUnearnedTypes.Size = new System.Drawing.Size(165, 21);
			this.comboUnearnedTypes.TabIndex = 117;
			this.comboUnearnedTypes.SelectionChangeCommitted += new System.EventHandler(this.comboUnearnedTypes_SelectionChangeCommitted);
			// 
			// comboProvider
			// 
			this.comboProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProvider.FormattingEnabled = true;
			this.comboProvider.Location = new System.Drawing.Point(129, 168);
			this.comboProvider.Name = "comboProvider";
			this.comboProvider.Size = new System.Drawing.Size(145, 21);
			this.comboProvider.TabIndex = 118;
			this.comboProvider.SelectionChangeCommitted += new System.EventHandler(this.comboProvider_SelectionChangeCommitted);
			// 
			// butPickProv
			// 
			this.butPickProv.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickProv.Autosize = false;
			this.butPickProv.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickProv.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickProv.CornerRadius = 2F;
			this.butPickProv.Location = new System.Drawing.Point(276, 168);
			this.butPickProv.Name = "butPickProv";
			this.butPickProv.Size = new System.Drawing.Size(18, 20);
			this.butPickProv.TabIndex = 158;
			this.butPickProv.Text = "...";
			this.butPickProv.Click += new System.EventHandler(this.butPickProv_Click);
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.FormattingEnabled = true;
			this.comboClinic.Location = new System.Drawing.Point(129, 143);
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(165, 21);
			this.comboClinic.TabIndex = 160;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(3, 146);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(124, 16);
			this.labelClinic.TabIndex = 159;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.labelPrePayWarning);
			this.groupBox1.Controls.Add(this.textPrePaidElsewhere);
			this.groupBox1.Controls.Add(this.label21);
			this.groupBox1.Controls.Add(this.textPrePaidRemain);
			this.groupBox1.Controls.Add(this.label22);
			this.groupBox1.Controls.Add(this.textPrePaidHere);
			this.groupBox1.Controls.Add(this.label20);
			this.groupBox1.Controls.Add(this.textPrePayAmt);
			this.groupBox1.Controls.Add(this.label19);
			this.groupBox1.Controls.Add(this.textPrePayType);
			this.groupBox1.Controls.Add(this.label18);
			this.groupBox1.Controls.Add(this.textPrePayDate);
			this.groupBox1.Controls.Add(this.label14);
			this.groupBox1.Controls.Add(this.butDetachPrepay);
			this.groupBox1.Controls.Add(this.butAttachPrepay);
			this.groupBox1.Location = new System.Drawing.Point(129, 398);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(616, 101);
			this.groupBox1.TabIndex = 161;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Payment Split";
			// 
			// labelPrePayWarning
			// 
			this.labelPrePayWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelPrePayWarning.ForeColor = System.Drawing.Color.Firebrick;
			this.labelPrePayWarning.Location = new System.Drawing.Point(182, 20);
			this.labelPrePayWarning.Name = "labelPrePayWarning";
			this.labelPrePayWarning.Size = new System.Drawing.Size(228, 23);
			this.labelPrePayWarning.TabIndex = 57;
			this.labelPrePayWarning.Text = "ERROR: PAYMENT SPLIT DELETED!";
			this.labelPrePayWarning.Visible = false;
			// 
			// textPrePaidElsewhere
			// 
			this.textPrePaidElsewhere.Location = new System.Drawing.Point(514, 51);
			this.textPrePaidElsewhere.Name = "textPrePaidElsewhere";
			this.textPrePaidElsewhere.ReadOnly = true;
			this.textPrePaidElsewhere.Size = new System.Drawing.Size(76, 20);
			this.textPrePaidElsewhere.TabIndex = 56;
			this.textPrePaidElsewhere.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(318, 53);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(192, 16);
			this.label21.TabIndex = 55;
			this.label21.Text = "Allocated Elsewhere";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPrePaidRemain
			// 
			this.textPrePaidRemain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textPrePaidRemain.Location = new System.Drawing.Point(515, 72);
			this.textPrePaidRemain.Name = "textPrePaidRemain";
			this.textPrePaidRemain.Size = new System.Drawing.Size(73, 18);
			this.textPrePaidRemain.TabIndex = 52;
			this.textPrePaidRemain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(318, 73);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(192, 16);
			this.label22.TabIndex = 51;
			this.label22.Text = "= Remaining";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPrePaidHere
			// 
			this.textPrePaidHere.Location = new System.Drawing.Point(514, 33);
			this.textPrePaidHere.Name = "textPrePaidHere";
			this.textPrePaidHere.ReadOnly = true;
			this.textPrePaidHere.Size = new System.Drawing.Size(76, 20);
			this.textPrePaidHere.TabIndex = 52;
			this.textPrePaidHere.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(318, 35);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(192, 16);
			this.label20.TabIndex = 51;
			this.label20.Text = "Used Here";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPrePayAmt
			// 
			this.textPrePayAmt.Location = new System.Drawing.Point(514, 13);
			this.textPrePayAmt.Name = "textPrePayAmt";
			this.textPrePayAmt.ReadOnly = true;
			this.textPrePayAmt.Size = new System.Drawing.Size(76, 20);
			this.textPrePayAmt.TabIndex = 52;
			this.textPrePayAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(318, 15);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(192, 16);
			this.label19.TabIndex = 51;
			this.label19.Text = "Prepayment Amount";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPrePayType
			// 
			this.textPrePayType.Location = new System.Drawing.Point(116, 70);
			this.textPrePayType.Name = "textPrePayType";
			this.textPrePayType.ReadOnly = true;
			this.textPrePayType.Size = new System.Drawing.Size(155, 20);
			this.textPrePayType.TabIndex = 54;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(9, 72);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(104, 16);
			this.label18.TabIndex = 53;
			this.label18.Text = "Payment Type";
			this.label18.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textPrePayDate
			// 
			this.textPrePayDate.Location = new System.Drawing.Point(116, 49);
			this.textPrePayDate.Name = "textPrePayDate";
			this.textPrePayDate.ReadOnly = true;
			this.textPrePayDate.Size = new System.Drawing.Size(76, 20);
			this.textPrePayDate.TabIndex = 52;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(9, 51);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(104, 16);
			this.label14.TabIndex = 51;
			this.label14.Text = "Date";
			this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butDetachPrepay
			// 
			this.butDetachPrepay.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDetachPrepay.Autosize = true;
			this.butDetachPrepay.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDetachPrepay.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDetachPrepay.CornerRadius = 4F;
			this.butDetachPrepay.Location = new System.Drawing.Point(100, 19);
			this.butDetachPrepay.Name = "butDetachPrepay";
			this.butDetachPrepay.Size = new System.Drawing.Size(75, 24);
			this.butDetachPrepay.TabIndex = 52;
			this.butDetachPrepay.Text = "Detach";
			this.butDetachPrepay.Click += new System.EventHandler(this.butDetachPrepay_Click);
			// 
			// butAttachPrepay
			// 
			this.butAttachPrepay.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAttachPrepay.Autosize = true;
			this.butAttachPrepay.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAttachPrepay.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAttachPrepay.CornerRadius = 4F;
			this.butAttachPrepay.Location = new System.Drawing.Point(12, 19);
			this.butAttachPrepay.Name = "butAttachPrepay";
			this.butAttachPrepay.Size = new System.Drawing.Size(75, 24);
			this.butAttachPrepay.TabIndex = 51;
			this.butAttachPrepay.Text = "Attach";
			this.butAttachPrepay.Click += new System.EventHandler(this.butAttachPrepay_Click);
			// 
			// butEditAnyway
			// 
			this.butEditAnyway.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEditAnyway.Autosize = true;
			this.butEditAnyway.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditAnyway.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditAnyway.CornerRadius = 4F;
			this.butEditAnyway.Location = new System.Drawing.Point(670, 178);
			this.butEditAnyway.Name = "butEditAnyway";
			this.butEditAnyway.Size = new System.Drawing.Size(75, 24);
			this.butEditAnyway.TabIndex = 163;
			this.butEditAnyway.Text = "Edit Anyway";
			this.butEditAnyway.Visible = false;
			this.butEditAnyway.Click += new System.EventHandler(this.butEditAnyway_Click);
			// 
			// labelEditAnyway
			// 
			this.labelEditAnyway.Location = new System.Drawing.Point(414, 176);
			this.labelEditAnyway.Name = "labelEditAnyway";
			this.labelEditAnyway.Size = new System.Drawing.Size(250, 28);
			this.labelEditAnyway.TabIndex = 164;
			this.labelEditAnyway.Text = "This paysplit is attached to a \r\nprocedure and should not be edited.";
			this.labelEditAnyway.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelEditAnyway.Visible = false;
			// 
			// FormPaySplitEdit
			// 
			this.ClientSize = new System.Drawing.Size(801, 541);
			this.Controls.Add(this.butEditAnyway);
			this.Controls.Add(this.labelEditAnyway);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butPickProv);
			this.Controls.Add(this.comboProvider);
			this.Controls.Add(this.comboUnearnedTypes);
			this.Controls.Add(this.label17);
			this.Controls.Add(this.textDateEntry);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.groupProcedure);
			this.Controls.Add(this.groupPatient);
			this.Controls.Add(this.textDatePay);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.textAmount);
			this.Controls.Add(this.butRemainder);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.ButCancel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkPayPlan);
			this.Controls.Add(this.labelAmount);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.labelRemainder);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Location = new System.Drawing.Point(0, 400);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(766, 580);
			this.Name = "FormPaySplitEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Payment Split";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPaySplitEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormPaySplitEdit_Load);
			this.groupPatient.ResumeLayout(false);
			this.groupPatient.PerformLayout();
			this.groupProcedure.ResumeLayout(false);
			this.groupProcedure.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormPaySplitEdit_Load(object sender, System.EventArgs e) {
			_paySplitCopy=PaySplitCur.Copy();
			textDateEntry.Text=PaySplitCur.DateEntry.ToShortDateString();
			textDatePay.Text=PaySplitCur.DatePay.ToShortDateString();
			textAmount.Text=PaySplitCur.SplitAmt.ToString("F");
			comboUnearnedTypes.Items.Add(Lan.g(this,"None"));
			comboUnearnedTypes.SelectedIndex=0;
			_listPaySplitUnearnedTypeDefs=Defs.GetDefsForCategory(DefCat.PaySplitUnearnedType,true);
			for(int i=0;i<_listPaySplitUnearnedTypeDefs.Count;i++) {
				comboUnearnedTypes.Items.Add(_listPaySplitUnearnedTypeDefs[i].ItemName);
				if(_listPaySplitUnearnedTypeDefs[i].DefNum==PaySplitCur.UnearnedType) {
					comboUnearnedTypes.SelectedIndex=i+1;
				}
			}
			if(PrefC.HasClinicsEnabled) {
				_listClinics=new List<Clinic>() { new Clinic() { Abbr=Lan.g(this,"None") } }; //Seed with "None"
				Clinics.GetForUserod(Security.CurUser).ForEach(x => _listClinics.Add(x));//do not re-organize from cache. They could either be alphabetizeded or sorted by item order.
				_listClinics.ForEach(x => comboClinic.Items.Add(x.Abbr));
				comboClinic.IndexSelectOrSetText(_listClinics.FindIndex(x => x.ClinicNum==PaySplitCur.ClinicNum),() => { return Clinics.GetAbbr(PaySplitCur.ClinicNum); });
			}
			else {
				labelClinic.Visible=false;
				comboClinic.Visible=false;
			}
			comboProvider.SelectedIndex=-1;
			FillComboProv();
			if(PaySplitCur.ProvNum==0) {
				comboProvider.SelectedIndex=0;
			}
			if(PaySplitCur.PayPlanNum==0){
				checkPayPlan.Checked=false;
			}
			else{
				checkPayPlan.Checked=true;
			}
			if(Clinics.IsMedicalPracticeOrClinic(PaySplitCur.ClinicNum)) {
				textProcTooth.Visible=false;
				labelProcTooth.Visible=false;
			}
			if(SplitAssociated!=null) {
				FillSplitAssociated();
			}
			ProcCur=PaySplitCur.ProcNum==0 ? null : Procedures.GetOneProc(PaySplitCur.ProcNum,false);
			if(ProcCur!=null) {
				butAttachPrepay.Enabled=false;
				butDetachPrepay.Enabled=false;
			}
			SetEnabledProc();
			FillPatient();
			FillProcedure();
		}

		///<summary>Sets the patient GroupBox, provider combobox & picker button, 
		///and clinic combobox enabled/disabled depending on whether a proc is attached.</summary>
		private void SetEnabledProc() {
			if(ProcCur!=null && !_isEditAnyway && PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully) {
				groupPatient.Enabled=false;
				comboProvider.Enabled=false;
				butPickProv.Enabled=false;
				if(PrefC.HasClinicsEnabled) {
					comboClinic.Enabled=false;
				}
				if(Security.IsAuthorized(Permissions.Setup,true)) {
					labelEditAnyway.Visible=true;
					butEditAnyway.Visible=true;
				}
			}
			else {
				groupPatient.Enabled=true;
				comboProvider.Enabled=true;
				butPickProv.Enabled=true;
				if(PrefC.HasClinicsEnabled) {
					comboClinic.Enabled=true;
				}
				comboUnearnedTypes.Enabled=true;
				butAttachProc.Enabled=true;
				butDetachProc.Enabled=true;
				labelEditAnyway.Visible=false;
				butEditAnyway.Visible=false;
				butAttachPrepay.Enabled=true;
				butDetachPrepay.Enabled=true;
			}
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboClinic.SelectedIndex>-1) {
				PaySplitCur.ClinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
			}
			else {
				PaySplitCur.ClinicNum=0;
			}
			FillComboProv();
		}

		private void comboProvider_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboProvider.SelectedIndex>-1) {
				PaySplitCur.ProvNum=_listProviders[comboProvider.SelectedIndex].ProvNum;
			}
			else {
				PaySplitCur.ProvNum=0;
			}
			if(_isEditAnyway || PrefC.GetInt(PrefName.RigorousAccounting)!=(int)RigorousAccounting.EnforceFully 
				|| PrefC.GetBool(PrefName.AllowPrepayProvider)) 
			{
				return;
			}
			if(PaySplitCur.ProvNum>0) {
				comboUnearnedTypes.SelectedIndex=0;
				comboUnearnedTypes.Enabled=false;
				PaySplitCur.UnearnedType=0;
			}
			else {
				comboUnearnedTypes.Enabled=true;
			}
		}

		private void comboUnearnedTypes_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboUnearnedTypes.SelectedIndex>0) {
				PaySplitCur.UnearnedType=_listPaySplitUnearnedTypeDefs[comboUnearnedTypes.SelectedIndex-1].DefNum;
			}
			else {
				PaySplitCur.UnearnedType=0;
			}
			if(_isEditAnyway || PrefC.GetInt(PrefName.RigorousAccounting)!=(int)RigorousAccounting.EnforceFully 
				|| PrefC.GetBool(PrefName.AllowPrepayProvider)) 
			{
				return;
			}
			if(PaySplitCur.UnearnedType>0) {//If they use an unearned type the provnum must be zero if Edit Anyway isn't pressed
				PaySplitCur.ProvNum=0;
				comboProvider.SelectedIndex=0;
				comboProvider.Enabled=false;
				butPickProv.Enabled=false;
				checkPayPlan.Checked=false;
				checkPayPlan.Enabled=false;
			}
			else {
				comboProvider.Enabled=true;
				butPickProv.Enabled=true;
				checkPayPlan.Enabled=true;
			}
		}

		private void butPickProv_Click(object sender,EventArgs e) {
			FormProviderPick formp = new FormProviderPick(_listProviders);
			formp.SelectedProvNum=PaySplitCur.ProvNum;
			formp.ShowDialog();
			if(formp.DialogResult!=DialogResult.OK) {
				return;
			}
			PaySplitCur.ProvNum=formp.SelectedProvNum;
			comboProvider.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==PaySplitCur.ProvNum),() => { return Providers.GetAbbr(PaySplitCur.ProvNum); });
		}

		///<summary>Fills combo provider based on which clinic is selected and attempts to preserve provider selection if any.</summary>
		private void FillComboProv() {
			_listProviders=new List<Provider>() { new Provider() { Abbr=Lan.g(this,"None") } };
			_listProviders.AddRange(Providers.GetProvsForClinic(PaySplitCur.ClinicNum));
			_listProviders=_listProviders.OrderBy(x => x.ProvNum>0).ThenBy(x => x.ItemOrder).ToList();
			//Fill comboProv
			comboProvider.Items.Clear();
			_listProviders.ForEach(x => comboProvider.Items.Add(x.Abbr));
			comboProvider.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==PaySplitCur.ProvNum),() => { return Providers.GetAbbr(PaySplitCur.ProvNum); });
		}

		private void butRemainder_Click(object sender, System.EventArgs e) {
			textAmount.Text=Remain.ToString("F");
		}

		///<summary>PaySplit.Patient is one value that is always kept in synch with the display.  If program changes PaySplit.Patient, then it will run this method to update the display.  If user changes display, then _MouseDown is run to update the PaySplit.Patient.</summary>
		private void FillPatient(){
			listPatient.Items.Clear();
			for(int i=0;i<_famCur.ListPats.Length;i++){
				listPatient.Items.Add(_famCur.GetNameInFamLFI(i));
				if(PaySplitCur.PatNum==_famCur.ListPats[i].PatNum){
					listPatient.SelectedIndex=i;
				}
			}
			//this can happen if user unchecks the "Is From Other Fam" box. Need to reset.
			if(PaySplitCur.PatNum==0){
				listPatient.SelectedIndex=0;
				//the initial patient will be the first patient in the family, usually guarantor
				PaySplitCur.PatNum=_famCur.ListPats[0].PatNum;
			}
			if(listPatient.SelectedIndex==-1){//patient not in family
				checkPatOtherFam.Checked=true;
				textPatient.Visible=true;
				listPatient.Visible=false;
				textPatient.Text=Patients.GetLim(PaySplitCur.PatNum).GetNameLF();
			}
			else{//show the family list that was just filled
				checkPatOtherFam.Checked=false;
				textPatient.Visible=false;
				listPatient.Visible=true;
			}
		}

		private void checkPatOtherFam_Click(object sender, System.EventArgs e) {
			//this happens after the check change has been registered
			if(checkPatOtherFam.Checked){
				FormPatientSelect FormPS=new FormPatientSelect();
				FormPS.SelectionModeOnly=true;
				FormPS.ShowDialog();
				if(FormPS.DialogResult!=DialogResult.OK){
					checkPatOtherFam.Checked=false;
					return;
				}
				PaySplitCur.PatNum=FormPS.SelectedPatNum;
			}
			else{//switch to family view
				PaySplitCur.PatNum=0;//this will reset the selected patient to current patient
			}
			butAttachPrepay.Enabled=true;
			butDetachPrepay.Enabled=true;
			ProcCur=null;
			FillProcedure();
			FillPatient();
		}

		private void listPatient_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			if(listPatient.SelectedIndex==-1){
				return;
			}
			PaySplitCur.PatNum=_famCur.ListPats[listPatient.SelectedIndex].PatNum;
		}

		private void FillProcedure(){
			if(ProcCur==null){
				textProcDate2.Text="";
				textProcProv.Text="";
				textProcTooth.Text="";
				textProcDescription.Text="";
				ProcFee=0;
				textProcFee.Text="";
				ProcWriteoff=0;
				textProcWriteoff.Text="";
				ProcInsPaid=0;
				textProcInsPaid.Text="";
				ProcInsEst=0;
				textProcInsEst.Text="";
				ProcAdj=0;
				textProcAdj.Text="";
				ProcPrevPaid=0;
				textProcPrevPaid.Text="";
				ProcPaidHere=0;
				textProcPaidHere.Text="";
				labelProcRemain.Text="";
				butAttachPrepay.Enabled=true;
				butDetachPrepay.Enabled=true;
				butAttachProc.Enabled=true;
				comboProvider.Enabled=true;
				comboClinic.Enabled=true;
				comboUnearnedTypes.Enabled=true;
				textPatient.Enabled=true;
				groupPatient.Enabled=true;
				butPickProv.Enabled=true;
				checkPatOtherFam.Enabled=true;
				return;
			}
			List<ClaimProc> ClaimProcList=ClaimProcs.Refresh(ProcCur.PatNum);
			Adjustment[] AdjustmentList=Adjustments.Refresh(ProcCur.PatNum);
			PaySplit[] PaySplitList=PaySplits.Refresh(ProcCur.PatNum);
			textProcDate2.Text=ProcCur.ProcDate.ToShortDateString();
			textProcProv.Text=Providers.GetAbbr(ProcCur.ProvNum);
			textProcTooth.Text=Tooth.ToInternat(ProcCur.ToothNum);
			textProcDescription.Text=ProcedureCodes.GetProcCode(ProcCur.CodeNum).Descript;
			ProcFee=Procedures.CalculateProcCharge(ProcCur);
			ProcWriteoff=-ClaimProcs.ProcWriteoff(ClaimProcList,ProcCur.ProcNum);
			ProcInsPaid=-ClaimProcs.ProcInsPay(ClaimProcList,ProcCur.ProcNum);
			ProcInsEst=-ClaimProcs.ProcEstNotReceived(ClaimProcList,ProcCur.ProcNum);
			ProcAdj=Adjustments.GetTotForProc(ProcCur.ProcNum,AdjustmentList);
			//next line will still work even if IsNew
			ProcPrevPaid=-PaySplits.GetTotForProc(ProcCur.ProcNum,PaySplitList,PaySplitCur.SplitNum);
			textProcFee.Text=ProcFee.ToString("F");
			if(ProcWriteoff==0){
				textProcWriteoff.Text="";
			}
			else{
				textProcWriteoff.Text=ProcWriteoff.ToString("F");
			}
			if(ProcInsPaid==0){
				textProcInsPaid.Text="";
			}
			else{
				textProcInsPaid.Text=ProcInsPaid.ToString("F");
			}
			if(ProcInsEst==0){
				textProcInsEst.Text="";
			}
			else{
				textProcInsEst.Text=ProcInsEst.ToString("F");
			}
			if(ProcAdj==0){
				textProcAdj.Text="";
			}
			else{
				textProcAdj.Text=ProcAdj.ToString("F");
			}
			if(ProcPrevPaid==0){
				textProcPrevPaid.Text="";
			}
			else{
				textProcPrevPaid.Text=ProcPrevPaid.ToString("F");
			}
			if(_listClinics!=null) {
				comboClinic.SelectedIndex=_listClinics.FindIndex(x => x.ClinicNum==PaySplitCur.ClinicNum);
			}
			butAttachProc.Enabled=false;
			if(PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully) {
				comboProvider.Enabled=false;
				comboClinic.Enabled=false;
			}
			comboUnearnedTypes.SelectedIndex=0;//First item is always None, if there is a procedure it cannot be a prepayment, regardless of enforce fully.
			comboUnearnedTypes.Enabled=false;
			textPatient.Enabled=false;
			groupPatient.Enabled=false;
			checkPatOtherFam.Enabled=false;
			butPickProv.Enabled=false;
			//Find the combo option for the procedure's clinic and provider.  If they don't exist in the list (are hidden) then it will set the text of the combo box instead.
			comboProvider.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==PaySplitCur.ProvNum),() => { return Providers.GetAbbr(PaySplitCur.ProvNum); });
			if(PrefC.HasClinicsEnabled) {
				comboClinic.IndexSelectOrSetText(_listClinics.FindIndex(x => x.ClinicNum==PaySplitCur.ClinicNum),() => { return Clinics.GetAbbr(PaySplitCur.ClinicNum); });
			}
			//Proc selected will always be for the pat this paysplit was made for
			listPatient.SelectedIndex=_famCur.ListPats.ToList().FindIndex(x => x.PatNum==PaySplitCur.PatNum);
			ComputeProcTotals();
		}

		private void FillSplitAssociated() {
			textPrePayDate.Text="";
			textPrePayType.Text="";
			textPrePayAmt.Text="";
			textPrePaidHere.Text="";
			textPrePaidRemain.Text="";
			textPrePaidElsewhere.Text="";
			labelProcRemain.Text="";
			textProcPaidHere.Text="";
			if(SplitAssociated!=null && SplitAssociated.PaySplitOrig!=null) {
				SetSplitAssociatedText();
			}
		}

		private void SetSplitAssociatedText() {
			PaySplit paySplitPrePayOrig=PaySplits.GetOriginalPrepayment(SplitAssociated.PaySplitOrig);
			//if the paySplitPrePayOrig is still null, check to see if the original is in the list of ListPaySplitAssociation
			if(paySplitPrePayOrig==null && ListPaySplitAssociated!=null) {
				PaySplits.PaySplitAssociated splitAssociated=ListPaySplitAssociated.Find(x=>x.PaySplitLinked==SplitAssociated.PaySplitOrig);
				if(splitAssociated!=null) {
					paySplitPrePayOrig=PaySplits.GetOne(splitAssociated.PaySplitOrig.SplitNum);
				}
			}
			//if the paySplitPrePayOrig is still null, check to see if the original is SplitAssociated.PaySplitOrig
			if(paySplitPrePayOrig==null && SplitAssociated.PaySplitOrig.UnearnedType!=0) {
				paySplitPrePayOrig=SplitAssociated.PaySplitOrig;
			}
			List<PaySplit> listPaySplitAllocatedElseWhere=new List<PaySplit>();
			DateTime datePay=SplitAssociated.PaySplitOrig.DatePay;
			string prePayType=Defs.GetName(DefCat.PaySplitUnearnedType,SplitAssociated.PaySplitOrig.UnearnedType);
			decimal amt=PIn.Decimal(textAmount.Text);
			decimal prepayAmt=0;
			decimal usedHere=Math.Abs(amt);
			decimal prePayUsedElsewhere=0;
			if(paySplitPrePayOrig!=null) {
				//add the prepayments allocated from the database. Excludes the current payment.
				listPaySplitAllocatedElseWhere.AddRange(PaySplits.GetAllocatedElseWhere(paySplitPrePayOrig.SplitNum)
					.FindAll(x=>x.PayNum!=PaySplitCur.PayNum));
				List<PaySplits.PaySplitAssociated> listAssociated=ListPaySplitAssociated
					.FindAll(x=> x.PaySplitLinked==SplitAssociated.PaySplitOrig || x.PaySplitOrig==SplitAssociated.PaySplitLinked).ToList();
				List<PaySplit> listOrig=listAssociated.Select(x=>x.PaySplitOrig).ToList();
				List<PaySplit> listLinked=listAssociated.Select(x=>x.PaySplitLinked).ToList();
				List<PaySplit> listPaySplitFromGrid=ListSplitsCur.FindAll(x=> x.SplitAmt<0 && !listOrig.Exists(y => y==x) && !listLinked.Exists(y => y==x));
				//add only paysplits from the left grid. 
				listPaySplitAllocatedElseWhere.AddRange(listPaySplitFromGrid);
				prepayAmt=(decimal)paySplitPrePayOrig.SplitAmt;
			}
			prePayUsedElsewhere=Math.Abs(listPaySplitAllocatedElseWhere.Sum(y => (decimal)y.SplitAmt));
			textPrePayDate.Text=datePay.ToShortDateString();
			textPrePayType.Text=prePayType;
			textPrePayAmt.Text=prepayAmt.ToString("F");//Total Original prepay amount.
			textPrePaidHere.Text=usedHere.ToString("F");//The prepay amount used here
			textPrePaidElsewhere.Text=prePayUsedElsewhere.ToString("F");//paySplitTotal- Sum of all allocated paysplits
			textPrePaidRemain.Text=(prepayAmt-prePayUsedElsewhere-usedHere).ToString("F");
		}

		///<summary>Does not alter any of the proc amounts except PaidHere and Remaining.</summary>
		private void ComputeProcTotals() {
			ProcPaidHere=0;
			if(textAmount.errorProvider1.GetError(textAmount)==""){
				ProcPaidHere=-PIn.Double(textAmount.Text);	
			}
			if(ProcPaidHere==0){
				textProcPaidHere.Text="";
			}
			else{
				textProcPaidHere.Text=ProcPaidHere.ToString("F");
			}
			//most of these are negative values, so add
			_remainAmt=
				(decimal)ProcFee
				+(decimal)ProcWriteoff
				+(decimal)ProcInsPaid
				+(decimal)ProcInsEst
				+(decimal)ProcAdj
				+(decimal)ProcPrevPaid
				+(decimal)ProcPaidHere;
			labelProcRemain.Text=_remainAmt.ToString("c");
		}

		private void textAmount_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			//can not use textAmount_TextChanged without redesigning the validDouble control
			if(textPrePayAmt.Text!="") {
				return;
			}
			ComputeProcTotals();
		}

		///<summary>Attaches procedure, sets the selected provider, and fills Procedure information.</summary>
		private void butAttachProc_Click(object sender, System.EventArgs e) {
			FormProcSelect FormPS=new FormProcSelect(PaySplitCur.PatNum,false);
			FormPS.ListSplitsCur = ListSplitsCur;
			FormPS.ShowDialog();
			if(FormPS.DialogResult!=DialogResult.OK){
				return;
			}
			ProcCur=FormPS.ListSelectedProcs[0];
			PaySplitCur.ProvNum=FormPS.ListSelectedProcs[0].ProvNum;
			PaySplitCur.ClinicNum=FormPS.ListSelectedProcs[0].ClinicNum;
			comboUnearnedTypes.SelectedIndex=0;
			PaySplitCur.UnearnedType=0;
			SetEnabledProc();
			FillProcedure();
		}

		private void butDetachProc_Click(object sender, System.EventArgs e) {
			ProcCur=null;
			SetEnabledProc();
			FillProcedure();
		}

		private void butEditAnyway_Click(object sender,EventArgs e) {
			_isEditAnyway=true;
			SetEnabledProc();
		}

		private void butAttachPrepay_Click(object sender,EventArgs e) {
			if(!textAmount.IsValid) {
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			double amount=PIn.Double(textAmount.Text);
			if(amount==0) {
				MsgBox.Show(this,"Please enter an amount");
				return;
			}
			bool isNegSplit=true;
			FormPaySplitSelect FormPSS=new FormPaySplitSelect(PaySplitCur.PatNum);
			FormPSS.IsPrePay=true;
			if(amount>0) {
				isNegSplit=false;
				//pass over the list of prepayments from the ListSplitCur. This will allow users to associated the negative split with a procedure.
				//will only show the splits that are negative, prepayments, and not associated to another paysplit. 
				List<PaySplit> listNegSplitAssociated=ListPaySplitAssociated.Select(x=>x.PaySplitOrig).ToList();
				FormPSS.ListUnallocatedSplits=ListSplitsCur.FindAll(x => x.SplitAmt<0 && x.UnearnedType!=0 && !x.In(listNegSplitAssociated)).ToList();
			}
			FormPSS.AmtAllocated=ListSplitsCur.FindAll(x => x.SplitAmt<0 && x.UnearnedType!=0 && x.FSplitNum!=0).ToList().Sum(y => (decimal)y.SplitAmt);
			FormPSS.SplitNumCur=PaySplitCur.SplitNum;
			if(FormPSS.ShowDialog()!=DialogResult.OK) {
				return;
			}
			SplitAssociated=new PaySplits.PaySplitAssociated(FormPSS.ListSelectedSplits[0],PaySplitCur);
			if(isNegSplit) {//if negative then we're allocating money from the original prepayment.
				butAttachProc.Enabled=false;
				butDetachProc.Enabled=false;
				comboProvider.Enabled=false;
				if(PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully && !PrefC.GetBool(PrefName.AllowPrepayProvider)) {
					//this probably needs to set comboProvider disabled in here as well (instead of above). Leaving for now due to separate bug fix.
					butPickProv.Enabled=false;  
				}
				if(comboUnearnedTypes.SelectedIndex==0) {//unearned has not been set
					int selectedIndex=_listPaySplitUnearnedTypeDefs.FindIndex(x => x.DefNum==SplitAssociated.PaySplitOrig.UnearnedType)+1;//+1 because of 'None'
					comboUnearnedTypes.SelectIndex(selectedIndex,Defs.GetName(DefCat.PaySplitUnearnedType,SplitAssociated.PaySplitOrig.UnearnedType));
					PaySplitCur.UnearnedType=SplitAssociated.PaySplitOrig.UnearnedType;
				}
				comboUnearnedTypes.Enabled=false;
			}
			//Always switch when negative so it matches the original, only switch for positive proc splits when prepayment has provider
			if(isNegSplit || SplitAssociated.PaySplitOrig.ProvNum!=0) {
				comboProvider.SelectIndex(_listProviders.FindIndex(x => x.ProvNum==SplitAssociated.PaySplitOrig.ProvNum)
					,Providers.GetAbbr(SplitAssociated.PaySplitOrig.ProvNum));
				PaySplitCur.ProvNum=SplitAssociated.PaySplitOrig.ProvNum;
			}
			FillSplitAssociated();
		}

		private void butDetachPrepay_Click(object sender,EventArgs e) {
			labelPrePayWarning.Visible=false;
			butAttachProc.Enabled=true;
			butDetachProc.Enabled=true;
			PaySplitCur.FSplitNum=0;
			SplitAssociated=null;
			comboProvider.Enabled=true;
			butPickProv.Enabled=true;
			comboUnearnedTypes.Enabled=true;
			FillSplitAssociated();
		}

		///<summary>Get the selected pay plan's current charges. If there is a charge, attach the split to that charge.</summary>
		private void AttachPlanCharge(PayPlan payPlan, long guar) {
			//get all current charges for that pay plan. If there are no current charges, don't allow the pay plan attach. 
			List<PayPlanCharge> listPayPlanChargesCurrent=PayPlanCharges.GetDueForPayPlan(payPlan,guar);
				if(listPayPlanChargesCurrent.Count==0) {
					//No current payments due for patient. Payment may be made ahead of schedule if procedure is attached.
					PayPlanChargeNum = 0;
				}
				else { 
					PayPlanChargeNum=listPayPlanChargesCurrent.OrderBy(x => x.ChargeDate).First().PayPlanChargeNum;//get oldest
				}
		}

		private void checkPayPlan_Click(object sender, System.EventArgs e) {
			if(checkPayPlan.Checked){
				if(checkPatOtherFam.Checked){//prevents a bug.
					checkPayPlan.Checked=false;
					return;
				}
				//PayPlan[] planListAll=PayPlans.Refresh(FamCur.List[listPatient.SelectedIndex].PatNum,0);
				//get all plans where the selected patient is the patnum or the guarantor of the payplan. Do not include insurance payment plans
				List<PayPlan> payPlanList=PayPlans.GetForPatNum(_famCur.ListPats[listPatient.SelectedIndex].PatNum).Where(x => x.PlanNum == 0).ToList();
				if(payPlanList.Count==0){//no valid plans
					MsgBox.Show(this,"The selected patient is not the guarantor for any payment plans.");
					checkPayPlan.Checked=false;
					return;
				}
				if(payPlanList.Count==1){ //if there is only one valid payplan
					PaySplitCur.PayPlanNum=payPlanList[0].PayPlanNum;
					AttachPlanCharge(payPlanList[0],payPlanList[0].Guarantor);
					return;
				}
				//more than one valid PayPlan
				FormPayPlanSelect FormPPS=new FormPayPlanSelect(payPlanList);
				//FormPPS.ValidPlans=payPlanList;
				FormPPS.ShowDialog();
				if(FormPPS.DialogResult==DialogResult.Cancel){
					checkPayPlan.Checked=false;
					return;
				}
				PaySplitCur.PayPlanNum=FormPPS.SelectedPayPlanNum; 
				PayPlan selectPayPlan=payPlanList.FirstOrDefault(x => x.PayPlanNum==PaySplitCur.PayPlanNum);
				//get the selected pay plan's current charges. If there is a charge, attach the split to that charge.
				AttachPlanCharge(selectPayPlan,selectPayPlan.Guarantor);
			}
			else{//payPlan unchecked
				PaySplitCur.PayPlanNum=0;
			}
		}

		private string SecurityLogEntryHelper(string oldVal,string newVal,string textInLog) {
			if(oldVal!=newVal) {
				return "\r\n "+textInLog+" changed from '"+oldVal+"' to '"+newVal+"'";
			}
			return "";
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(!MsgBox.Show(this,true,"Delete Item?")) {
				return;
			}
			if(IsNew) {
				PaySplitCur=null;
				DialogResult=DialogResult.Cancel;
				return;
			}
			//This is the main problem with leaving it up to engineers to manually set public variables before showing forms...
			//We have been getting null reference reports from this security log entry.
			//Only check if PaySplitCur is null because _paySplitCopy gets set OnLoad() which must have been invoked especially if they clicked Delete.
			if(PaySplitCur!=null) {
				SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,PaySplitCur.PatNum,"Payment deleted for "+PaySplitCur.SplitAmt,0,_paySplitCopy.SecDateTEdit);
			}
			PaySplitCur=null;
			DialogResult=DialogResult.OK;
		}

		private bool IsValid() {
			if(textAmount.errorProvider1.GetError(textAmount)!=""	|| textDatePay.errorProvider1.GetError(textDatePay)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return false;
			}
			double amount=PIn.Double(textAmount.Text);
			if(amount==0) {
				MsgBox.Show(this,"Please enter an amount");
				return false;
			}
			if(PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully && PaySplitCur.UnearnedType!=0 && ProcCur!=null 
				&& !_isEditAnyway) 
			{
				MsgBox.Show(this,"Cannot have an unallocated split that also has an attached procedure.");
				return false;
			}
			if(_remainAmt<0 && ProcCur!=null) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Remaining amount is negative.  Continue?","Overpaid Procedure Warning")) {
					return false;
				}
			}
			if(checkPayPlan.Checked && checkPatOtherFam.Checked){
				MessageBox.Show(Lan.g(this,"You cannot split outside of the family for a payment plan.")+"  "
					+Lan.g(this,"Either uncheck the Attached to Payment Plan checkbox, or split to a family member instead."));
				return false;
			}
      if(PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully && ProcCur==null && PaySplitCur.UnearnedType==0) {
				MsgBox.Show(this,"You must attach a procedure to this payment.");
				return false;
      }
			if(_isIncomeTransfer && PaySplitCur.UnearnedType!=0 && amount<0 && SplitAssociated?.PaySplitOrig==null) {
				//To handle the case when they are manually making their prepayment and they forget to attach the original prepayment split.
				//Only when this split is negative because they are free to correct previous mistakes with a positive split. 
				MsgBox.Show(this,"You must attach a prepayment to this split.");
				return false;
			}
			//Provider and Unearned combos will be correct at this point, based on ProvNum or UnearnedType.
			//Unearned type and provider are set in SelectionChangeCommitted events for the respective combo boxes, when rigorous and provs not allowed
			if(!_isEditAnyway && PrefC.GetInt(PrefName.RigorousAccounting)==(int)RigorousAccounting.EnforceFully) {
				PaySplit split=SplitAssociated?.PaySplitOrig??new PaySplit();
				if(split.ProvNum!=0 && ProcCur!=null && split.ProvNum!=ProcCur.ProvNum) {
					MsgBox.Show(this,"Procedure provider and original paysplit provider do not match.");
					return false;
				}
				if(PaySplitCur.ProvNum>0 && !PrefC.GetBool(PrefName.AllowPrepayProvider)) {
					PaySplitCur.UnearnedType=0;
				}
				else if(PaySplitCur.ProvNum<=0){
					if(comboUnearnedTypes.SelectedIndex==0 
						&& !MsgBox.Show(this,MsgBoxButtons.YesNo,"Having a provider of \"None\" will mark this paysplit as a prepayment.  Continue?")) 
					{
						return false;
					}
					PaySplitCur.ProvNum=0;//This means it's unallocated.
					if(comboUnearnedTypes.SelectedIndex==0) {
						PaySplitCur.UnearnedType=PrefC.GetLong(PrefName.PrepaymentUnearnedType);
					}
					else {
						PaySplitCur.UnearnedType=_listPaySplitUnearnedTypeDefs[comboUnearnedTypes.SelectedIndex-1].DefNum;
					}
				}
			}
			else if(comboUnearnedTypes.SelectedIndex==0 && comboProvider.SelectedIndex==0) {//may want to change this to match the above in the future.
				MsgBox.Show(this,"Please select an unearned type or a provider.");
				return false;
			}
			return true;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(!IsValid()) {
				return;
			}
			double amount=PIn.Double(textAmount.Text);
			PaySplitCur.DatePay=PIn.Date(textDatePay.Text);//gets overwritten anyway
			PaySplitCur.SplitAmt=amount;
			PaySplitCur.ProcNum=ProcCur == null ? 0 : ProcCur.ProcNum;
			if(IsNew) {
				string secLogText="Paysplit created with provider "+Providers.GetAbbr(PaySplitCur.ProvNum);
				if(Clinics.GetAbbr(PaySplitCur.ClinicNum)!="") {
					secLogText+=", clinic "+Clinics.GetAbbr(PaySplitCur.ClinicNum);
				}
				secLogText+=", amount "+PaySplitCur.SplitAmt.ToString("F");
				SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,PaySplitCur.PatNum,secLogText);
			}
			else {
				string secLogText="Paysplit edited";
				secLogText+=SecurityLogEntryHelper(Providers.GetAbbr(_paySplitCopy.ProvNum),Providers.GetAbbr(PaySplitCur.ProvNum),"provider");
				secLogText+=SecurityLogEntryHelper(Clinics.GetAbbr(_paySplitCopy.ClinicNum),Clinics.GetAbbr(PaySplitCur.ClinicNum),"clinic");
				secLogText+=SecurityLogEntryHelper(_paySplitCopy.SplitAmt.ToString("F"),PaySplitCur.SplitAmt.ToString("F"),"amount");
				secLogText+=SecurityLogEntryHelper(_paySplitCopy.PatNum.ToString(),PaySplitCur.PatNum.ToString(),"patient number");
				SecurityLogs.MakeLogEntry(Permissions.PaymentEdit,PaySplitCur.PatNum,secLogText,0,_paySplitCopy.SecDateTEdit);
			}
			DialogResult=DialogResult.OK;
		}

		private void ButCancel_Click(object sender, System.EventArgs e) {
			if(IsNew) {
				PaySplitCur=null;
			}
			DialogResult=DialogResult.Cancel;
		}

		private void FormPaySplitEdit_FormClosing(object sender,FormClosingEventArgs e) {
			if(DialogResult==DialogResult.OK || PaySplitCur==null) {
				return;
			}
			PaySplitCur.ClinicNum=_paySplitCopy.ClinicNum;
			PaySplitCur.DateEntry=_paySplitCopy.DateEntry;
			PaySplitCur.DatePay=_paySplitCopy.DatePay;
			PaySplitCur.DiscountType=_paySplitCopy.DiscountType;
			PaySplitCur.IsDiscount=_paySplitCopy.IsDiscount;
			PaySplitCur.IsInterestSplit=_paySplitCopy.IsInterestSplit;
			PaySplitCur.PatNum=_paySplitCopy.PatNum;
			PaySplitCur.PayNum=_paySplitCopy.PayNum;
			PaySplitCur.PayPlanNum=_paySplitCopy.PayPlanNum;
			PaySplitCur.FSplitNum=_paySplitCopy.FSplitNum;
			PaySplitCur.ProcDate=_paySplitCopy.ProcDate;
			PaySplitCur.ProcNum=_paySplitCopy.ProcNum;
			PaySplitCur.ProvNum=_paySplitCopy.ProvNum;
			PaySplitCur.SecDateTEdit=_paySplitCopy.SecDateTEdit;
			PaySplitCur.SecUserNumEntry=_paySplitCopy.SecUserNumEntry;
			PaySplitCur.SplitAmt=_paySplitCopy.SplitAmt;
			PaySplitCur.SplitNum=_paySplitCopy.SplitNum;
			PaySplitCur.UnearnedType=_paySplitCopy.UnearnedType;
		}
	}
}
