using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using MySql.Data.MySqlClient;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public class FormFinanceCharges : ODForm {
		private OpenDental.ValidDate textDate;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radio30;
		private System.Windows.Forms.RadioButton radio90;
		private System.Windows.Forms.RadioButton radio60;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private OpenDental.ValidNum textAPR;
		private System.ComponentModel.Container components = null;
		//private ArrayList ALPosIndices;
		private ValidDate textDateLastRun;
		private Label label5;
		private OpenDental.UI.Button butUndo;
		private GroupBox groupBox2;
		private ValidDate textDateUndo;
		private Label label6;
		private ListBox listBillType;
		private Panel panel1;
		private Label label8;
		private ValidDouble textBillingCharge;
		private RadioButton radioBillingCharge;
		private RadioButton radioFinanceCharge;
		private Label label12;
		private Label label11;
		private ValidDouble textOver;
		private ValidDouble textAtLeast;
		private Label labelOver;
		private Label labelAtLeast;
		private CheckBox checkCompound;
		private Label labelCompound;
		private Label label7;
		private List<Def> _listBillingTypeDefs;

		//private int adjType;

		///<summary></summary>
		public FormFinanceCharges(){
			InitializeComponent();
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

		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFinanceCharges));
			this.textDate = new OpenDental.ValidDate();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radio30 = new System.Windows.Forms.RadioButton();
			this.radio90 = new System.Windows.Forms.RadioButton();
			this.radio60 = new System.Windows.Forms.RadioButton();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textAPR = new OpenDental.ValidNum();
			this.textDateLastRun = new OpenDental.ValidDate();
			this.label5 = new System.Windows.Forms.Label();
			this.butUndo = new OpenDental.UI.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.textDateUndo = new OpenDental.ValidDate();
			this.label6 = new System.Windows.Forms.Label();
			this.listBillType = new System.Windows.Forms.ListBox();
			this.label7 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.labelCompound = new System.Windows.Forms.Label();
			this.checkCompound = new System.Windows.Forms.CheckBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.textOver = new OpenDental.ValidDouble();
			this.textAtLeast = new OpenDental.ValidDouble();
			this.labelOver = new System.Windows.Forms.Label();
			this.labelAtLeast = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.radioFinanceCharge = new System.Windows.Forms.RadioButton();
			this.textBillingCharge = new OpenDental.ValidDouble();
			this.radioBillingCharge = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(171, 42);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(78, 20);
			this.textDate.TabIndex = 15;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(15, 46);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(154, 14);
			this.label1.TabIndex = 20;
			this.label1.Text = "Date of new charges";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radio30);
			this.groupBox1.Controls.Add(this.radio90);
			this.groupBox1.Controls.Add(this.radio60);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(58, 226);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(167, 98);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Calculate on balances aged";
			// 
			// radio30
			// 
			this.radio30.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radio30.Location = new System.Drawing.Point(13, 24);
			this.radio30.Name = "radio30";
			this.radio30.Size = new System.Drawing.Size(104, 16);
			this.radio30.TabIndex = 1;
			this.radio30.Text = "Over 30 Days";
			// 
			// radio90
			// 
			this.radio90.Checked = true;
			this.radio90.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radio90.Location = new System.Drawing.Point(13, 70);
			this.radio90.Name = "radio90";
			this.radio90.Size = new System.Drawing.Size(104, 18);
			this.radio90.TabIndex = 3;
			this.radio90.TabStop = true;
			this.radio90.Text = "Over 90 Days";
			// 
			// radio60
			// 
			this.radio60.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radio60.Location = new System.Drawing.Point(13, 46);
			this.radio60.Name = "radio60";
			this.radio60.Size = new System.Drawing.Size(104, 18);
			this.radio60.TabIndex = 2;
			this.radio60.Text = "Over 60 Days";
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
			this.butCancel.Location = new System.Drawing.Point(588, 392);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 19;
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
			this.butOK.Location = new System.Drawing.Point(588, 361);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
			this.butOK.TabIndex = 18;
			this.butOK.Text = "Run";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(67, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 14);
			this.label2.TabIndex = 22;
			this.label2.Text = "APR";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(194, 46);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(12, 14);
			this.label3.TabIndex = 23;
			this.label3.Text = "%";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(212, 46);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(102, 14);
			this.label4.TabIndex = 24;
			this.label4.Text = "(For Example: 18)";
			// 
			// textAPR
			// 
			this.textAPR.Location = new System.Drawing.Point(147, 43);
			this.textAPR.MaxVal = 255;
			this.textAPR.MinVal = 0;
			this.textAPR.Name = "textAPR";
			this.textAPR.Size = new System.Drawing.Size(42, 20);
			this.textAPR.TabIndex = 26;
			// 
			// textDateLastRun
			// 
			this.textDateLastRun.Location = new System.Drawing.Point(171, 16);
			this.textDateLastRun.Name = "textDateLastRun";
			this.textDateLastRun.ReadOnly = true;
			this.textDateLastRun.Size = new System.Drawing.Size(78, 20);
			this.textDateLastRun.TabIndex = 27;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(12, 20);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(157, 14);
			this.label5.TabIndex = 28;
			this.label5.Text = "Date last run";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butUndo
			// 
			this.butUndo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUndo.Autosize = true;
			this.butUndo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUndo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUndo.CornerRadius = 4F;
			this.butUndo.Location = new System.Drawing.Point(113, 48);
			this.butUndo.Name = "butUndo";
			this.butUndo.Size = new System.Drawing.Size(78, 25);
			this.butUndo.TabIndex = 30;
			this.butUndo.Text = "Undo";
			this.butUndo.Click += new System.EventHandler(this.butUndo_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.textDateUndo);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.butUndo);
			this.groupBox2.Location = new System.Drawing.Point(58, 330);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(263, 87);
			this.groupBox2.TabIndex = 31;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Undo finance/billing charges";
			// 
			// textDateUndo
			// 
			this.textDateUndo.Location = new System.Drawing.Point(113, 19);
			this.textDateUndo.Name = "textDateUndo";
			this.textDateUndo.ReadOnly = true;
			this.textDateUndo.Size = new System.Drawing.Size(78, 20);
			this.textDateUndo.TabIndex = 31;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 23);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(95, 14);
			this.label6.TabIndex = 32;
			this.label6.Text = "Date to undo";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// listBillType
			// 
			this.listBillType.Location = new System.Drawing.Point(388, 34);
			this.listBillType.Name = "listBillType";
			this.listBillType.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBillType.Size = new System.Drawing.Size(158, 186);
			this.listBillType.TabIndex = 32;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(387, 16);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(214, 16);
			this.label7.TabIndex = 33;
			this.label7.Text = "Only apply to these Billing Types";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.labelCompound);
			this.panel1.Controls.Add(this.checkCompound);
			this.panel1.Controls.Add(this.label12);
			this.panel1.Controls.Add(this.label11);
			this.panel1.Controls.Add(this.textOver);
			this.panel1.Controls.Add(this.textAtLeast);
			this.panel1.Controls.Add(this.labelOver);
			this.panel1.Controls.Add(this.labelAtLeast);
			this.panel1.Controls.Add(this.label8);
			this.panel1.Controls.Add(this.radioFinanceCharge);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.textBillingCharge);
			this.panel1.Controls.Add(this.textAPR);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.radioBillingCharge);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Location = new System.Drawing.Point(58, 68);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(319, 152);
			this.panel1.TabIndex = 34;
			// 
			// labelCompound
			// 
			this.labelCompound.Location = new System.Drawing.Point(28, 124);
			this.labelCompound.Name = "labelCompound";
			this.labelCompound.Size = new System.Drawing.Size(95, 14);
			this.labelCompound.TabIndex = 39;
			this.labelCompound.Text = "Compound interest";
			this.labelCompound.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkCompound
			// 
			this.checkCompound.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkCompound.Checked = true;
			this.checkCompound.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkCompound.Location = new System.Drawing.Point(145, 124);
			this.checkCompound.Margin = new System.Windows.Forms.Padding(0);
			this.checkCompound.Name = "checkCompound";
			this.checkCompound.Size = new System.Drawing.Size(16, 14);
			this.checkCompound.TabIndex = 35;
			this.checkCompound.UseVisualStyleBackColor = true;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(135, 73);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(12, 14);
			this.label12.TabIndex = 38;
			this.label12.Text = "$";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(135, 100);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(12, 14);
			this.label11.TabIndex = 37;
			this.label11.Text = "$";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textOver
			// 
			this.textOver.BackColor = System.Drawing.SystemColors.Window;
			this.textOver.Location = new System.Drawing.Point(147, 97);
			this.textOver.MaxVal = 100000000D;
			this.textOver.MinVal = -100000000D;
			this.textOver.Name = "textOver";
			this.textOver.Size = new System.Drawing.Size(42, 20);
			this.textOver.TabIndex = 36;
			// 
			// textAtLeast
			// 
			this.textAtLeast.BackColor = System.Drawing.SystemColors.Window;
			this.textAtLeast.Location = new System.Drawing.Point(147, 70);
			this.textAtLeast.MaxVal = 100000000D;
			this.textAtLeast.MinVal = -100000000D;
			this.textAtLeast.Name = "textAtLeast";
			this.textAtLeast.Size = new System.Drawing.Size(42, 20);
			this.textAtLeast.TabIndex = 35;
			// 
			// labelOver
			// 
			this.labelOver.Location = new System.Drawing.Point(28, 99);
			this.labelOver.Name = "labelOver";
			this.labelOver.Size = new System.Drawing.Size(95, 14);
			this.labelOver.TabIndex = 33;
			this.labelOver.Text = "Only if over";
			this.labelOver.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelAtLeast
			// 
			this.labelAtLeast.Location = new System.Drawing.Point(28, 73);
			this.labelAtLeast.Name = "labelAtLeast";
			this.labelAtLeast.Size = new System.Drawing.Size(95, 14);
			this.labelAtLeast.TabIndex = 34;
			this.labelAtLeast.Text = "Charge at least";
			this.labelAtLeast.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(135, 14);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(12, 14);
			this.label8.TabIndex = 28;
			this.label8.Text = "$";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// radioFinanceCharge
			// 
			this.radioFinanceCharge.AutoSize = true;
			this.radioFinanceCharge.Checked = true;
			this.radioFinanceCharge.Location = new System.Drawing.Point(11, 44);
			this.radioFinanceCharge.Name = "radioFinanceCharge";
			this.radioFinanceCharge.Size = new System.Drawing.Size(100, 17);
			this.radioFinanceCharge.TabIndex = 0;
			this.radioFinanceCharge.TabStop = true;
			this.radioFinanceCharge.Text = "Finance Charge";
			this.radioFinanceCharge.UseVisualStyleBackColor = true;
			this.radioFinanceCharge.CheckedChanged += new System.EventHandler(this.radioFinanceCharge_CheckedChanged);
			// 
			// textBillingCharge
			// 
			this.textBillingCharge.BackColor = System.Drawing.SystemColors.Window;
			this.textBillingCharge.Location = new System.Drawing.Point(147, 12);
			this.textBillingCharge.MaxVal = 100000000D;
			this.textBillingCharge.MinVal = -100000000D;
			this.textBillingCharge.Name = "textBillingCharge";
			this.textBillingCharge.ReadOnly = true;
			this.textBillingCharge.Size = new System.Drawing.Size(42, 20);
			this.textBillingCharge.TabIndex = 27;
			// 
			// radioBillingCharge
			// 
			this.radioBillingCharge.AutoSize = true;
			this.radioBillingCharge.Location = new System.Drawing.Point(11, 12);
			this.radioBillingCharge.Name = "radioBillingCharge";
			this.radioBillingCharge.Size = new System.Drawing.Size(89, 17);
			this.radioBillingCharge.TabIndex = 1;
			this.radioBillingCharge.TabStop = true;
			this.radioBillingCharge.Text = "Billing Charge";
			this.radioBillingCharge.UseVisualStyleBackColor = true;
			this.radioBillingCharge.CheckedChanged += new System.EventHandler(this.radioBillingCharge_CheckedChanged);
			// 
			// FormFinanceCharges
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(692, 440);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.listBillType);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.textDateLastRun);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textDate);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormFinanceCharges";
			this.ShowInTaskbar = false;
			this.Text = "Finance/Billing Charges";
			this.Load += new System.EventHandler(this.FormFinanceCharges_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormFinanceCharges_Load(object sender, System.EventArgs e) {
			if(PrefC.GetLong(PrefName.FinanceChargeAdjustmentType)==0){
				MsgBox.Show(this,"No finance charge adjustment type has been set.  Please go to Setup | Account to fix this.");
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(PrefC.GetLong(PrefName.BillingChargeAdjustmentType)==0){
				MsgBox.Show(this,"No billing charge adjustment type has been set.  Please go to Setup | Account to fix this.");
				DialogResult=DialogResult.Cancel;
				return;
			}
			_listBillingTypeDefs=Defs.GetDefsForCategory(DefCat.BillingTypes,true);
			if(_listBillingTypeDefs.Count==0){//highly unlikely that this would happen
				MsgBox.Show(this,"No billing types have been set up or are visible.");
				DialogResult=DialogResult.Cancel;
				return;
			}
			Action actionCloseAgingProgress=null;
			if(PrefC.GetBool(PrefName.AgingIsEnterprise)) {
				if(!RunAgingEnterprise(true)) {
					DialogResult=DialogResult.Cancel;
					return;
				}
			}
			else {
				try {
					DateTime asOfDate=(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)?PrefC.GetDate(PrefName.DateLastAging):DateTime.Today);
					actionCloseAgingProgress=ODProgressOld.ShowProgressStatus("ComputeAging",this,Lan.g(this,"Calculating aging for all patients as of")+" "
						+asOfDate.ToShortDateString()+"...");
					Cursor=Cursors.WaitCursor;
					Ledgers.RunAging();
				}
				catch(MySqlException ex) {
					actionCloseAgingProgress?.Invoke();//effectively terminates progress bar
					Cursor=Cursors.Default;
					if(ex==null || ex.Number!=1213) {//not a deadlock error, just throw
						throw;
					}
					MsgBox.Show(this,"Deadlock error detected in aging transaction and rolled back. Try again later.");
					DialogResult=DialogResult.Cancel;
					return;
				}
				finally {
					actionCloseAgingProgress?.Invoke();//effectively terminates progress bar
					Cursor=Cursors.Default;
				}
			}
			textDate.Text=DateTime.Today.ToShortDateString();		
			textAPR.MaxVal=100;
			textAPR.MinVal=0;
			textAPR.Text=PrefC.GetString(PrefName.FinanceChargeAPR);
			textBillingCharge.Text=PrefC.GetString(PrefName.BillingChargeAmount);
			for(int i=0;i<_listBillingTypeDefs.Count;i++) {
				listBillType.Items.Add(_listBillingTypeDefs[i].ItemName);
				listBillType.SetSelected(i,true);
			}
			string defaultChargeMethod = PrefC.GetString(PrefName.BillingChargeOrFinanceIsDefault);
			if (defaultChargeMethod == "Finance") {
				radioFinanceCharge.Checked = true;
				textDateLastRun.Text = PrefC.GetDate(PrefName.FinanceChargeLastRun).ToShortDateString();
				textDateUndo.Text = PrefC.GetDate(PrefName.FinanceChargeLastRun).ToShortDateString();
				textBillingCharge.ReadOnly=true;
				textBillingCharge.BackColor=System.Drawing.SystemColors.Control;
			}
			else if (defaultChargeMethod == "Billing") {
				radioBillingCharge.Checked = true;
				textDateLastRun.Text = PrefC.GetDate(PrefName.BillingChargeLastRun).ToShortDateString();
				textDateUndo.Text = PrefC.GetDate(PrefName.BillingChargeLastRun).ToShortDateString();
			}
			textAtLeast.Text=PrefC.GetString(PrefName.FinanceChargeAtLeast);
			textOver.Text=PrefC.GetString(PrefName.FinanceChargeOnlyIfOver);
		}

		///<summary>If !isPreCharges, a message box will display for any errors instructing users to try again.  If the failed aging attempt is after
		///charges have been added/deleted, we don't want to inform the user that the transaction failed so run again since the charges were successfully
		///inserted/deleted and it was only updating the aged balances that failed.  If isPreCharges, this won't run aging again if the last aging run was
		///today.  If !isPreCharges, we will run aging even if it was run today to update aged bals to include the charges added/deleted.</summary>
		private bool RunAgingEnterprise(bool isOnLoad=false) {
			DateTime dtNow=MiscData.GetNowDateTime();
			DateTime dtToday=dtNow.Date;
			DateTime dateLastAging=PrefC.GetDate(PrefName.DateLastAging);
			if(isOnLoad && dateLastAging.Date==dtToday) {
				return true;//this is prior to inserting/deleting charges and aging has already been run for this date
			}
			Prefs.RefreshCache();
			DateTime dateTAgingBeganPref=PrefC.GetDateT(PrefName.AgingBeginDateTime);
			if(dateTAgingBeganPref>DateTime.MinValue) {
				if(isOnLoad) {
					MessageBox.Show(this,Lan.g(this,"In order to add finance charges, aging must be calculated, but you cannot run aging until it has finished "
						+"the current calculations which began on")+" "+dateTAgingBeganPref.ToString()+".\r\n"+Lans.g(this,"If you believe the current aging "
						+"process has finished, a user with SecurityAdmin permission can manually clear the date and time by going to Setup | Miscellaneous and "
						+"pressing the 'Clear' button."));
				}
				return false;
			}
			Prefs.UpdateString(PrefName.AgingBeginDateTime,POut.DateT(dtNow,false));//get lock on pref to block others
			Signalods.SetInvalid(InvalidType.Prefs);//signal a cache refresh so other computers will have the updated pref as quickly as possible
			Action actionCloseProgress=null;
			try {
				actionCloseProgress=ODProgressOld.ShowProgressStatus("FinanceCharge",this,Lan.g(this,"Calculating enterprise aging for all patients as of")+" "
					+dtToday.ToShortDateString()+"...");
				Cursor=Cursors.WaitCursor;
				Ledgers.ComputeAging(0,dtToday);
				Prefs.UpdateString(PrefName.DateLastAging,POut.Date(dtToday,false));
			}
			catch(MySqlException ex) {
				actionCloseProgress?.Invoke();//effectively terminates progress bar
				Cursor=Cursors.Default;
				if(ex==null || ex.Number!=1213) {//not a deadlock error, just throw
					throw;
				}
				if(isOnLoad) {
					MsgBox.Show(this,"Deadlock error detected in enterprise aging transaction and rolled back. Try again later.");
				}
				return false;
			}
			finally {
				actionCloseProgress?.Invoke();//effectively terminates progress bar
				Cursor=Cursors.Default;
				Prefs.UpdateString(PrefName.AgingBeginDateTime,"");//clear lock on pref whether aging was successful or not
				Signalods.SetInvalid(InvalidType.Prefs);
			}
			return true;
		}

		private void radioFinanceCharge_CheckedChanged(object sender, EventArgs e) {
			textAPR.ReadOnly = false;
			textAPR.BackColor = System.Drawing.SystemColors.Window;
			textAtLeast.ReadOnly=false;
			textAtLeast.BackColor = System.Drawing.SystemColors.Window;
			labelAtLeast.Enabled=true;
			textOver.ReadOnly=false;
			textOver.BackColor = System.Drawing.SystemColors.Window;
			labelOver.Enabled=true;
			textBillingCharge.ReadOnly = true;
			textBillingCharge.BackColor = System.Drawing.SystemColors.Control;
			textDateLastRun.Text = PrefC.GetDate(PrefName.FinanceChargeLastRun).ToShortDateString();
			textDateUndo.Text = PrefC.GetDate(PrefName.FinanceChargeLastRun).ToShortDateString();
		}

		private void radioBillingCharge_CheckedChanged(object sender, EventArgs e) {
			textAPR.ReadOnly = true;
			textAPR.BackColor = System.Drawing.SystemColors.Control;
			textAtLeast.ReadOnly=true;
			textAtLeast.BackColor = System.Drawing.SystemColors.Control;
			labelAtLeast.Enabled=false;
			textOver.ReadOnly=true;
			textOver.BackColor = System.Drawing.SystemColors.Control;
			labelOver.Enabled=false;
			textBillingCharge.ReadOnly = false;
			textBillingCharge.BackColor = System.Drawing.SystemColors.Window;
			textDateLastRun.Text = PrefC.GetDate(PrefName.BillingChargeLastRun).ToShortDateString();
			textDateUndo.Text = PrefC.GetDate(PrefName.BillingChargeLastRun).ToShortDateString();
		}

		private void butUndo_Click(object sender,EventArgs e) {
			string chargeType=(radioFinanceCharge.Checked?"Finance":"Billing");
			if(MessageBox.Show(Lan.g(this,"Undo all "+chargeType.ToLower()+" charges for")+" "+textDateUndo.Text+"?","",MessageBoxButtons.OKCancel)
				!=DialogResult.OK)
			{
				return;
			}
			Action actionCloseProgress=null;
			int rowsAffected=0;
			try {
				actionCloseProgress=ODProgressOld.ShowProgressStatus(chargeType+"Charge",this,
					Lan.g(this,"Deleting "+chargeType.ToLower()+" charge adjustments")+"...");
				Cursor=Cursors.WaitCursor;
				rowsAffected=(int)Adjustments.UndoFinanceOrBillingCharges(PIn.Date(textDateUndo.Text),radioBillingCharge.Checked);
			}
			finally {
				actionCloseProgress?.Invoke();//effectively terminates progress bar
				Cursor=Cursors.Default;
			}
			MessageBox.Show(Lan.g(this,chargeType+" charge adjustments deleted")+": "+rowsAffected);
			if(rowsAffected==0) {
				DialogResult=DialogResult.OK;
				return;
			}
			actionCloseProgress=null;
			if(PrefC.GetBool(PrefName.AgingIsEnterprise)) {
				if(!RunAgingEnterprise()) {
					MsgBox.Show(this,"There was an error calculating aging after the "+chargeType.ToLower()+" charge adjustments were deleted.\r\n"
						+"You should run aging later to update affected accounts.");
				}
			}
			else {
				try {
					DateTime asOfDate=(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)?PrefC.GetDate(PrefName.DateLastAging):DateTime.Today);
					actionCloseProgress=ODProgressOld.ShowProgressStatus("FinanceCharge",this,Lan.g(this,"Calculating aging for all patients as of")+" "
						+asOfDate.ToShortDateString()+"...");
					Cursor=Cursors.WaitCursor;
					Ledgers.RunAging();
				}
				catch(MySqlException ex) {
					actionCloseProgress?.Invoke();//effectively terminates progress bar
					Cursor=Cursors.Default;
					if(ex==null || ex.Number!=1213) {//not a deadlock error, just throw
						throw;
					}
					MsgBox.Show(this,"There was a deadlock error calculating aging after the "+chargeType.ToLower()+" charge adjustments were deleted.\r\n"
						+"You should run aging later to update affected accounts.");
				}
				finally {
					actionCloseProgress?.Invoke();//effectively terminates progress bar
					Cursor=Cursors.Default;
				}
			}
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,chargeType+" Charges undo. Date "+textDateUndo.Text);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(textDate.errorProvider1.GetError(textDate)!=""
				|| textAPR.errorProvider1.GetError(textAPR)!=""
				|| textAtLeast.errorProvider1.GetError(textAtLeast)!=""
				|| textOver.errorProvider1.GetError(textOver)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			DateTime date=PIn.Date(textDate.Text);
			if(PrefC.GetDate(PrefName.FinanceChargeLastRun).AddDays(25)>date) {
				if(!MsgBox.Show(this,true,"Warning.  Finance charges should not be run more than once per month.  Continue?")) {
					return;
				}
			} 
			else if(PrefC.GetDate(PrefName.BillingChargeLastRun).AddDays(25)>date) {
				if(!MsgBox.Show(this,true,"Warning.  Billing charges should not be run more than once per month.  Continue?")) {
					return;
				}
			}
			if(listBillType.SelectedIndices.Count==0) {
				MsgBox.Show(this,"Please select at least one billing type first.");
				return;
			}
			if(PIn.Long(textAPR.Text) < 2) {
				if(!MsgBox.Show(this,true,"The APR is much lower than normal. Do you wish to proceed?")) {
					return;
				}
			}
			if(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily) && PrefC.GetDate(PrefName.DateLastAging).AddMonths(1)<=DateTime.Today) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"It has been more than a month since aging has been run.  It is recommended that you update the "
					+"aging date and run aging before continuing."))
				{
					return;
				}
				//we might also consider a warning if textDate.Text does not match DateLastAging.  Probably not needed for daily aging, though.
			}
			string chargeType=(radioFinanceCharge.Checked?"Finance":"Billing");//For display only
			List<long> listSelectedBillTypes=listBillType.SelectedIndices.OfType<int>().Select(x => _listBillingTypeDefs[x].DefNum).ToList();
			Action actionCloseProgress=null;
			int chargesAdded=0;
			try {
				actionCloseProgress=ODProgressOld.ShowProgressStatus("FinanceCharge",this,Lan.g(this,"Gathering patients with aged balances")+"...");
				List<PatAging> listPatAgings=Patients.GetAgingListSimple(listSelectedBillTypes,new List<long> { });//Ordered by PatNum, for thread concurrency
				long adjType=PrefC.GetLong(PrefName.FinanceChargeAdjustmentType);
				Dictionary<long,List<Adjustment>> dictPatAdjustments=new Dictionary<long, List<Adjustment>>();
				if(!checkCompound.Checked) {
					int daysOver=(radio30.Checked ? 30
						: radio60.Checked ? 60
						: 90);
					DateTime maxAdjDate=MiscData.GetNowDateTime().Date.AddDays(-daysOver);
					dictPatAdjustments=Adjustments.GetAdjustForPatsByType(listPatAgings.Select(x => x.PatNum).ToList(),adjType,maxAdjDate);
				}
				int chargesProcessed=0;
				List<Action> listActions=new List<Action>();
				foreach(PatAging patAgingCur in listPatAgings) {
					listActions.Add(new Action(() => {
						if(++chargesProcessed%5==0) {
							ODEvent.Fire(new ODEventArgs("FinanceCharge",Lan.g(this,"Processing "+chargeType+" charges")+": "+chargesProcessed+" out of "
								+listPatAgings.Count));
						}
						//This WILL NOT be the same as the patient's total balance. Start with BalOver90 since all options include that bucket. Add others if needed.
						double overallBalance=patAgingCur.BalOver90+(radio60.Checked?patAgingCur.Bal_61_90:radio30.Checked?(patAgingCur.Bal_31_60+patAgingCur.Bal_61_90):0);
						if(overallBalance<=.01d) {
							return;
						}
						if(radioBillingCharge.Checked) {
							AddBillingCharge(patAgingCur.PatNum,date,textBillingCharge.Text,patAgingCur.PriProv);
						}
						else {//Finance charge
							if(dictPatAdjustments.ContainsKey(patAgingCur.PatNum)) {//Only contains key if checkCompound is not checked.
								overallBalance-=dictPatAdjustments[patAgingCur.PatNum].Sum(x => x.AdjAmt);//Dict always contains patNum as key, but list can be empty.
							}
							if(!AddFinanceCharge(patAgingCur.PatNum,date,textAPR.Text,textAtLeast.Text,textOver.Text,overallBalance,patAgingCur.PriProv,adjType)) {
								return;
							}
						}
						chargesAdded++;
					}));
				}
				ODThread.RunParallel(listActions,TimeSpan.FromMinutes(2));//each group of actions gets X minutes.
				if(radioFinanceCharge.Checked) {
					if(Prefs.UpdateString(PrefName.FinanceChargeAPR,textAPR.Text) 
						| Prefs.UpdateString(PrefName.FinanceChargeLastRun,POut.Date(date,false))
						| Prefs.UpdateString(PrefName.FinanceChargeAtLeast,textAtLeast.Text)
						| Prefs.UpdateString(PrefName.FinanceChargeOnlyIfOver,textOver.Text)
						| Prefs.UpdateString(PrefName.BillingChargeOrFinanceIsDefault,"Finance"))
					{
						DataValid.SetInvalid(InvalidType.Prefs);
					}
				}
				else if(radioBillingCharge.Checked) {
					if(Prefs.UpdateString(PrefName.BillingChargeAmount,textBillingCharge.Text)
						| Prefs.UpdateString(PrefName.BillingChargeLastRun,POut.Date(date,false))
						| Prefs.UpdateString(PrefName.BillingChargeOrFinanceIsDefault,"Billing"))
					{
						DataValid.SetInvalid(InvalidType.Prefs);
					}
				}
			}
			finally {
				actionCloseProgress?.Invoke();//terminates progress bar
			}
			MessageBox.Show(Lan.g(this,chargeType+" charges added")+": "+chargesAdded);
			if(PrefC.GetBool(PrefName.AgingIsEnterprise)) {
				if(!RunAgingEnterprise()) {
					MsgBox.Show(this,"There was an error calculating aging after the "+chargeType.ToLower()+" charge adjustments were added.\r\n"
						+"You should run aging later to update affected accounts.");
				}
			}
			else {
				DateTime asOfDate=(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)?PrefC.GetDate(PrefName.DateLastAging):DateTime.Today);
				actionCloseProgress=ODProgressOld.ShowProgressStatus("FinanceCharge",this,Lan.g(this,"Calculating aging for all patients as of")+" "
					+asOfDate.ToShortDateString()+"...");
				Cursor=Cursors.WaitCursor;
				try {
					Ledgers.RunAging();
				}
				catch(MySqlException ex) {
					actionCloseProgress?.Invoke();//terminates progress bar
					Cursor=Cursors.Default;
					if(ex==null || ex.Number!=1213) {//not a deadlock error, just throw
						throw;
					}
					MsgBox.Show(this,"There was a deadlock error calculating aging after the "+chargeType.ToLower()+" charge adjustments were added.\r\n"
						+"You should run aging later to update affected accounts.");
				}
				finally {
					actionCloseProgress?.Invoke();//terminates progress bar
					Cursor=Cursors.Default;
				}
			}
			DialogResult = DialogResult.OK;
		}

		/// <summary>Returns true if a finance charge is added, false if one is not added</summary>
		private bool AddFinanceCharge(long PatNum,DateTime date,string APR,string atLeast,string ifOver,double OverallBalance,long PriProv,long adjType) {
			if(date > DateTime.Today && !PrefC.GetBool(PrefName.FutureTransDatesAllowed)) {
				MsgBox.Show(this,"Adjustments cannot be made for future dates. Finance charge was not added.");
				return false;
			}
			InstallmentPlan installPlan=InstallmentPlans.GetOneForFam(PatNum);
			if(installPlan!=null) {//Patient has an installment plan so use that APR instead.
				APR=installPlan.APR.ToString();
			}
			Adjustment AdjustmentCur = new Adjustment();
			AdjustmentCur.PatNum = PatNum;
			//AdjustmentCur.DateEntry=PIn.PDate(textDate.Text);//automatically handled
			AdjustmentCur.AdjDate = date;
			AdjustmentCur.ProcDate = date;
			AdjustmentCur.AdjType = adjType;
			AdjustmentCur.AdjNote = "";//"Finance Charge";
			AdjustmentCur.AdjAmt = Math.Round(((PIn.Double(APR) * .01d / 12d) * OverallBalance),2);
			if(AdjustmentCur.AdjAmt.IsZero() || AdjustmentCur.AdjAmt<PIn.Double(ifOver)) {
				//Don't add the charge if it is less than FinanceChargeOnlyIfOver; if the charge is exactly equal to FinanceChargeOnlyIfOver,
				//the charge will be added. Ex., AdjAmt=2.00 and FinanceChargeOnlyIfOver=2.00, the charge will be added.
				//Unless AdjAmt=0.00, in which case don't add a $0.00 finance charge
				return false;
			}
			//Add an amount that is at least the amount of FinanceChargeAtLeast 
			AdjustmentCur.AdjAmt=Math.Max(AdjustmentCur.AdjAmt,PIn.Double(atLeast));
			AdjustmentCur.ProvNum = PriProv;
			Adjustments.Insert(AdjustmentCur);
			return true;
		}

		private void AddBillingCharge(long PatNum,DateTime date,string BillingChargeAmount,long PriProv) {
			if(date > DateTime.Today && !PrefC.GetBool(PrefName.FutureTransDatesAllowed)) {
				MsgBox.Show(this,"Adjustments cannot be made for future dates");
				return;
			}
			Adjustment AdjustmentCur = new Adjustment();
			AdjustmentCur.PatNum = PatNum;
			//AdjustmentCur.DateEntry=PIn.PDate(textDate.Text);//automatically handled
			AdjustmentCur.AdjDate = date;
			AdjustmentCur.ProcDate = date;
			AdjustmentCur.AdjType = PrefC.GetLong(PrefName.BillingChargeAdjustmentType);
			AdjustmentCur.AdjNote = "";//"Billing Charge";
			AdjustmentCur.AdjAmt = PIn.Double(BillingChargeAmount);
			AdjustmentCur.ProvNum = PriProv;
			Adjustments.Insert(AdjustmentCur);
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		
	}
}
