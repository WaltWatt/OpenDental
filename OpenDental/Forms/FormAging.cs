using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using OpenDentBusiness;

namespace OpenDental{
	///<summary></summary>
	public class FormAging : ODForm {
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.TextBox textBox1;
		private OpenDental.ValidDate textDateLast;
		private OpenDental.ValidDate textDateCalc;
		private System.Windows.Forms.Label label2;
		private System.ComponentModel.Container components = null;

		///<summary></summary>
		public FormAging(){
			InitializeComponent();
			Lan.F(this);
			Lan.C(this,new Control[]
				{this.textBox1});
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAging));
			this.textDateLast = new OpenDental.ValidDate();
			this.label1 = new System.Windows.Forms.Label();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textDateCalc = new OpenDental.ValidDate();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// textDateLast
			// 
			this.textDateLast.Location = new System.Drawing.Point(173,79);
			this.textDateLast.Name = "textDateLast";
			this.textDateLast.ReadOnly = true;
			this.textDateLast.Size = new System.Drawing.Size(94,20);
			this.textDateLast.TabIndex = 12;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(23,83);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(146,16);
			this.label1.TabIndex = 13;
			this.label1.Text = "Last Calculated";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(440,138);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,26);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(440,104);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,26);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.SystemColors.Control;
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.Location = new System.Drawing.Point(25,12);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(476,62);
			this.textBox1.TabIndex = 16;
			this.textBox1.Text = "If you use monthly billing instead of daily, then this is where you change the ag" +
    "ing date every month.  Otherwise, it\'s not necessary to manually run aging.  It\'" +
    "s all handled automatically.";
			// 
			// textDateCalc
			// 
			this.textDateCalc.Location = new System.Drawing.Point(173,111);
			this.textDateCalc.Name = "textDateCalc";
			this.textDateCalc.Size = new System.Drawing.Size(94,20);
			this.textDateCalc.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(23,115);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(146,16);
			this.label2.TabIndex = 18;
			this.label2.Text = "Calculate as of";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// FormAging
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(532,180);
			this.Controls.Add(this.textDateCalc);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.textDateLast);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAging";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Calculate Aging";
			this.Load += new System.EventHandler(this.FormAging_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormAging_Load(object sender, System.EventArgs e) {
			DateTime dateLastAging=PrefC.GetDate(PrefName.DateLastAging);
			if(dateLastAging.Year<1880){
				textDateLast.Text="";
			}
			else{
				textDateLast.Text=dateLastAging.ToShortDateString();
			}
			if(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)){
				if(dateLastAging < DateTime.Today.AddDays(-15)) {
					textDateCalc.Text=dateLastAging.AddMonths(1).ToShortDateString();
				}
				else {
					textDateCalc.Text=dateLastAging.ToShortDateString();
				}
			}
			else{
				textDateCalc.Text=DateTime.Today.ToShortDateString();
				if(PrefC.GetBool(PrefName.AgingIsEnterprise)) {//enterprise aging requires daily not monthly calc
					textDateCalc.ReadOnly=true;
					textDateCalc.BackColor=SystemColors.Control;
				}
			}
		}

		private bool RunAgingEnterprise(DateTime dateCalc) {
			DateTime dateLastAging=PrefC.GetDate(PrefName.DateLastAging);
			if(dateLastAging.Date==dateCalc.Date) {
				if(MessageBox.Show(this,Lan.g(this,"Aging has already been calculated for")+" "+dateCalc.ToShortDateString()+" "
					+Lan.g(this,"and does not normally need to run more than once per day.\r\n\r\nRun anyway?"),"",MessageBoxButtons.YesNo)!=DialogResult.Yes)
				{
					return false;
				}
			}
			//Refresh prefs because AgingBeginDateTime is very time sensitive
			Prefs.RefreshCache();
			DateTime dateTAgingBeganPref=PrefC.GetDateT(PrefName.AgingBeginDateTime);
			if(dateTAgingBeganPref>DateTime.MinValue) {
				MessageBox.Show(this,Lan.g(this,"You cannot run aging until it has finished the current calculations which began on")+" "
					+dateTAgingBeganPref.ToString()+".\r\n"+Lans.g(this,"If you believe the current aging process has finished, a user with SecurityAdmin permission "
					+"can manually clear the date and time by going to Setup | Miscellaneous and pressing the 'Clear' button."));
				return false;
			}
			Prefs.UpdateString(PrefName.AgingBeginDateTime,POut.DateT(MiscData.GetNowDateTime(),false));//get lock on pref to block others
			Signalods.SetInvalid(InvalidType.Prefs);//signal a cache refresh so other computers will have the updated pref as quickly as possible
			Action actionCloseAgingProgress=null;
			try {
				actionCloseAgingProgress=ODProgressOld.ShowProgressStatus("ComputeAging",this,
					Lan.g(this,"Calculating enterprise aging for all patients as of")+" "+dateCalc.ToShortDateString()+"...");
				Cursor=Cursors.WaitCursor;
				Ledgers.ComputeAging(0,dateCalc);
				Prefs.UpdateString(PrefName.DateLastAging,POut.Date(dateCalc,false));
			}
			catch(MySqlException ex) {
				actionCloseAgingProgress?.Invoke();
				Cursor=Cursors.Default;
				if(ex==null || ex.Number!=1213) {//not a deadlock error, just throw
					throw;
				}
				MsgBox.Show(this,"Deadlock error detected in aging transaction and rolled back. Try again later.");
				return false;
			}
			finally {
				actionCloseAgingProgress?.Invoke();
				Cursor=Cursors.Default;
				Prefs.UpdateString(PrefName.AgingBeginDateTime,"");//clear lock on pref whether aging was successful or not
				Signalods.SetInvalid(InvalidType.Prefs);
			}
			return true;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textDateCalc.errorProvider1.GetError(textDateCalc)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			DateTime dateCalc=PIn.Date(textDateCalc.Text);
			Action actionCloseAgingProgress=null;
			if(PrefC.GetBool(PrefName.AgingIsEnterprise)) {
				//if this is true, dateCalc has to be DateTime.Today and aging calculated daily not monthly.
				if(!RunAgingEnterprise(dateCalc)) {
					//Errors displayed from RunAgingEnterprise
					return;
				}
			}
			else {
				try {
					actionCloseAgingProgress=ODProgressOld.ShowProgressStatus("ComputeAging",this,Lan.g(this,"Calculating aging for all patients as of")+" "
						+dateCalc.ToShortDateString()+"...");
					Cursor=Cursors.WaitCursor;
					Ledgers.ComputeAging(0,dateCalc);
				}
				catch(MySqlException ex) {
					actionCloseAgingProgress?.Invoke();
					Cursor=Cursors.Default;
					if(ex==null || ex.Number!=1213) {//not a deadlock error, just throw
						throw;
					}
					MsgBox.Show(this,"Deadlock error detected in aging transaction and rolled back. Try again later.");
					DialogResult=DialogResult.Cancel;
					return;
				}
				finally {
					actionCloseAgingProgress?.Invoke();
					Cursor=Cursors.Default;
				}
				if(Prefs.UpdateString(PrefName.DateLastAging,POut.Date(dateCalc,false))){
					DataValid.SetInvalid(InvalidType.Prefs);
				}
			}
			MsgBox.Show(this,"Aging Complete");
			DialogResult=DialogResult.OK;
		}

	}
}
