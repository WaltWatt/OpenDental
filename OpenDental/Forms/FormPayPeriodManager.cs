using System;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Linq;

namespace OpenDental {

	public class FormPayPeriodManager:ODForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private UI.Button butCancel;
		private UI.Button butGenerate;
		private ODGrid gridMain;
		private UI.Button butOK;
		private DateTimePicker dateTimeStart;
		private GroupBox groupBox1;
		private Label label1;
		private RadioButton radioWeekly;
		private RadioButton radioMonthly;
		private RadioButton radioBiWeekly;
		private GroupBox groupBox2;
		private GroupBox groupBox3;
		private Label label4;
		private Label label2;
		private Label label5;
		private ValidNum numDaysAfterPayPeriod;
		private ValidNum numPayPeriods;
		private ComboBox comboDay;
		private RadioButton radioPayBefore;
		private RadioButton radioPayAfter;
		private CheckBox checkExcludeWeekends;
		private Label label3;
		private List<PayPeriod> _listPayPeriods;

		///<summary></summary>
		public FormPayPeriodManager()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPayPeriodManager));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.radioPayBefore = new System.Windows.Forms.RadioButton();
			this.radioPayAfter = new System.Windows.Forms.RadioButton();
			this.checkExcludeWeekends = new System.Windows.Forms.CheckBox();
			this.comboDay = new System.Windows.Forms.ComboBox();
			this.numDaysAfterPayPeriod = new OpenDental.ValidNum();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.radioWeekly = new System.Windows.Forms.RadioButton();
			this.numPayPeriods = new OpenDental.ValidNum();
			this.radioMonthly = new System.Windows.Forms.RadioButton();
			this.label5 = new System.Windows.Forms.Label();
			this.radioBiWeekly = new System.Windows.Forms.RadioButton();
			this.dateTimeStart = new System.Windows.Forms.DateTimePicker();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butOK = new OpenDental.UI.Button();
			this.butGenerate = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.groupBox3);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.groupBox2);
			this.groupBox1.Controls.Add(this.dateTimeStart);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(309, 290);
			this.groupBox1.TabIndex = 20;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Pay Period Options";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.Controls.Add(this.radioPayBefore);
			this.groupBox3.Controls.Add(this.radioPayAfter);
			this.groupBox3.Controls.Add(this.checkExcludeWeekends);
			this.groupBox3.Controls.Add(this.comboDay);
			this.groupBox3.Controls.Add(this.numDaysAfterPayPeriod);
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Location = new System.Drawing.Point(31, 120);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(262, 160);
			this.groupBox3.TabIndex = 7;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Pay Day";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(78, 51);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(23, 13);
			this.label3.TabIndex = 26;
			this.label3.Text = "OR";
			// 
			// radioPayBefore
			// 
			this.radioPayBefore.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioPayBefore.Location = new System.Drawing.Point(51, 127);
			this.radioPayBefore.Name = "radioPayBefore";
			this.radioPayBefore.Size = new System.Drawing.Size(94, 17);
			this.radioPayBefore.TabIndex = 24;
			this.radioPayBefore.TabStop = true;
			this.radioPayBefore.Text = "Pay Before";
			this.radioPayBefore.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioPayBefore.UseVisualStyleBackColor = true;
			// 
			// radioPayAfter
			// 
			this.radioPayAfter.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioPayAfter.Location = new System.Drawing.Point(151, 127);
			this.radioPayAfter.Name = "radioPayAfter";
			this.radioPayAfter.Size = new System.Drawing.Size(73, 17);
			this.radioPayAfter.TabIndex = 25;
			this.radioPayAfter.TabStop = true;
			this.radioPayAfter.Text = "Pay After";
			this.radioPayAfter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioPayAfter.UseVisualStyleBackColor = true;
			// 
			// checkExcludeWeekends
			// 
			this.checkExcludeWeekends.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkExcludeWeekends.Location = new System.Drawing.Point(92, 100);
			this.checkExcludeWeekends.Name = "checkExcludeWeekends";
			this.checkExcludeWeekends.Size = new System.Drawing.Size(119, 17);
			this.checkExcludeWeekends.TabIndex = 23;
			this.checkExcludeWeekends.Text = "Exclude Weekends";
			this.checkExcludeWeekends.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkExcludeWeekends.UseVisualStyleBackColor = true;
			this.checkExcludeWeekends.CheckedChanged += new System.EventHandler(this.checkExcludeWeekends_CheckedChanged);
			// 
			// comboDay
			// 
			this.comboDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDay.FormattingEnabled = true;
			this.comboDay.Items.AddRange(new object[] {
            "None",
            "Sunday",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday"});
			this.comboDay.Location = new System.Drawing.Point(91, 21);
			this.comboDay.Name = "comboDay";
			this.comboDay.Size = new System.Drawing.Size(94, 21);
			this.comboDay.TabIndex = 22;
			this.comboDay.SelectionChangeCommitted += new System.EventHandler(this.comboDay_SelectionChangeCommitted);
			// 
			// numDaysAfterPayPeriod
			// 
			this.numDaysAfterPayPeriod.Location = new System.Drawing.Point(132, 74);
			this.numDaysAfterPayPeriod.MaxVal = 200;
			this.numDaysAfterPayPeriod.MinVal = 0;
			this.numDaysAfterPayPeriod.Name = "numDaysAfterPayPeriod";
			this.numDaysAfterPayPeriod.Size = new System.Drawing.Size(79, 20);
			this.numDaysAfterPayPeriod.TabIndex = 21;
			this.numDaysAfterPayPeriod.TextChanged += new System.EventHandler(this.numDaysAfterPayPeriod_TextChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(6, 75);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(120, 17);
			this.label4.TabIndex = 2;
			this.label4.Text = "# Days After Pay Period";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(81, 17);
			this.label2.TabIndex = 0;
			this.label2.Text = "Day of Week";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 20);
			this.label1.TabIndex = 6;
			this.label1.Text = "Start Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.radioWeekly);
			this.groupBox2.Controls.Add(this.numPayPeriods);
			this.groupBox2.Controls.Add(this.radioMonthly);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.radioBiWeekly);
			this.groupBox2.Location = new System.Drawing.Point(31, 42);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(262, 72);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Interval";
			// 
			// radioWeekly
			// 
			this.radioWeekly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioWeekly.Location = new System.Drawing.Point(28, 19);
			this.radioWeekly.Name = "radioWeekly";
			this.radioWeekly.Size = new System.Drawing.Size(61, 17);
			this.radioWeekly.TabIndex = 1;
			this.radioWeekly.TabStop = true;
			this.radioWeekly.Text = "Weekly";
			this.radioWeekly.UseVisualStyleBackColor = true;
			this.radioWeekly.Click += new System.EventHandler(this.radioWeekly_Click);
			// 
			// numPayPeriods
			// 
			this.numPayPeriods.Location = new System.Drawing.Point(168, 42);
			this.numPayPeriods.MaxVal = 200;
			this.numPayPeriods.MinVal = 0;
			this.numPayPeriods.Name = "numPayPeriods";
			this.numPayPeriods.Size = new System.Drawing.Size(79, 20);
			this.numPayPeriods.TabIndex = 22;
			// 
			// radioMonthly
			// 
			this.radioMonthly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioMonthly.Location = new System.Drawing.Point(179, 19);
			this.radioMonthly.Name = "radioMonthly";
			this.radioMonthly.Size = new System.Drawing.Size(62, 17);
			this.radioMonthly.TabIndex = 3;
			this.radioMonthly.TabStop = true;
			this.radioMonthly.Text = "Monthly";
			this.radioMonthly.UseVisualStyleBackColor = true;
			this.radioMonthly.Click += new System.EventHandler(this.radioMonthly_Click);
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Location = new System.Drawing.Point(3, 43);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(162, 17);
			this.label5.TabIndex = 10;
			this.label5.Text = "# Pay Periods to Generate";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// radioBiWeekly
			// 
			this.radioBiWeekly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioBiWeekly.Location = new System.Drawing.Point(95, 19);
			this.radioBiWeekly.Name = "radioBiWeekly";
			this.radioBiWeekly.Size = new System.Drawing.Size(73, 17);
			this.radioBiWeekly.TabIndex = 2;
			this.radioBiWeekly.TabStop = true;
			this.radioBiWeekly.Text = "Bi-Weekly";
			this.radioBiWeekly.UseVisualStyleBackColor = true;
			this.radioBiWeekly.Click += new System.EventHandler(this.radioBiWeekly_Click);
			this.radioBiWeekly.MouseUp += new System.Windows.Forms.MouseEventHandler(this.radioBiWeekly_Click);
			// 
			// dateTimeStart
			// 
			this.dateTimeStart.CustomFormat = "";
			this.dateTimeStart.Location = new System.Drawing.Point(93, 19);
			this.dateTimeStart.Name = "dateTimeStart";
			this.dateTimeStart.Size = new System.Drawing.Size(200, 20);
			this.dateTimeStart.TabIndex = 0;
			// 
			// gridMain
			// 
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(327, 20);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(291, 282);
			this.gridMain.TabIndex = 11;
			this.gridMain.Title = "Pay Periods";
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TablePayPeriods";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(462, 310);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 18;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butGenerate
			// 
			this.butGenerate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGenerate.Autosize = true;
			this.butGenerate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGenerate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGenerate.CornerRadius = 4F;
			this.butGenerate.Image = global::OpenDental.Properties.Resources.Add;
			this.butGenerate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butGenerate.Location = new System.Drawing.Point(235, 308);
			this.butGenerate.Name = "butGenerate";
			this.butGenerate.Size = new System.Drawing.Size(86, 24);
			this.butGenerate.TabIndex = 10;
			this.butGenerate.Text = "&Generate";
			this.butGenerate.Click += new System.EventHandler(this.butGenerate_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(543, 310);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormPayPeriodManager
			// 
			this.ClientSize = new System.Drawing.Size(630, 346);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butGenerate);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormPayPeriodManager";
			this.ShowInTaskbar = false;
			this.Text = "Pay Period Manager";
			this.Load += new System.EventHandler(this.FormPayPeriodManager_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormPayPeriodManager_Load(object sender, System.EventArgs e) {
			PayPeriod payPeriod=PayPeriods.GetMostRecent();
			if(payPeriod==null) {
				dateTimeStart.Value=DateTime.Today;
			}
			else {
				dateTimeStart.Value=payPeriod.DateStop.AddDays(1);
			}
			PayPeriodInterval payPeriodInterval=(PayPeriodInterval)PrefC.GetInt(PrefName.PayPeriodIntervalSetting);
			if(payPeriodInterval==PayPeriodInterval.Weekly) {
				radioWeekly.Checked=true;
				numPayPeriods.Text="52";
			}
			else if(payPeriodInterval==PayPeriodInterval.BiWeekly) {
				radioBiWeekly.Checked=true;
				numPayPeriods.Text="21";
			}
			else if(payPeriodInterval==PayPeriodInterval.Monthly) {
				radioMonthly.Checked=true;
				numPayPeriods.Text="12";
			}
			int dayOfWeek=PrefC.GetInt(PrefName.PayPeriodPayDay);
			if(dayOfWeek!=0) {//They have a day of the week selected
				comboDay.SelectedIndex=dayOfWeek;
				numDaysAfterPayPeriod.Enabled=false;
				checkExcludeWeekends.Enabled=false;
				radioPayBefore.Enabled=false;
				radioPayAfter.Enabled=false;
			}
			else {
				comboDay.SelectedIndex=0;
				numDaysAfterPayPeriod.Text=PrefC.GetString(PrefName.PayPeriodPayAfterNumberOfDays);
				checkExcludeWeekends.Checked=PrefC.GetBool(PrefName.PayPeriodPayDateExcludesWeekends);
				if(checkExcludeWeekends.Checked) {
					if(PrefC.GetBool(PrefName.PayPeriodPayDateBeforeWeekend)) {
						radioPayBefore.Checked=true;
					}
					else {
						radioPayAfter.Checked=true;
					}
				}
				if(!checkExcludeWeekends.Checked) {
					radioPayBefore.Checked=false;
					radioPayBefore.Enabled=false;
					radioPayAfter.Checked=false;
					radioPayAfter.Enabled=false;
				}
				else {
					radioPayBefore.Enabled=true;
					radioPayAfter.Enabled=true;
				}
			}
			_listPayPeriods=new List<PayPeriod>();
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn("Start Date",80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("End Date",80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Paycheck Date",100);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listPayPeriods.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listPayPeriods[i].DateStart.ToShortDateString());
				row.Cells.Add(_listPayPeriods[i].DateStop.ToShortDateString());
				if(_listPayPeriods[i].DatePaycheck.Year<1880){
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(_listPayPeriods[i].DatePaycheck.ToShortDateString());
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender, ODGridClickEventArgs e) {
			//Allowing modification of pay periods here will cause insertions/updates.  It would require changing in FormPayPeriodEdit to not insert/update.
			FormPayPeriodEdit FormP=new FormPayPeriodEdit(_listPayPeriods[e.Row]);
			FormP.IsSaveToDb=false;
			FormP.ShowDialog();
			FillGrid();
		}

		private void butGenerate_Click(object sender, EventArgs e) {
			//Generate payperiods based on settings
			if(numPayPeriods.errorProvider1.GetError(numPayPeriods)!="") {
				MsgBox.Show(this,numPayPeriods.errorProvider1.GetError(numPayPeriods));
				return;
			}
			if(numDaysAfterPayPeriod.Enabled && numDaysAfterPayPeriod.errorProvider1.GetError(numDaysAfterPayPeriod)!="") {
				MsgBox.Show(this,numDaysAfterPayPeriod.errorProvider1.GetError(numDaysAfterPayPeriod));
				return;
			}
			if(numDaysAfterPayPeriod.Enabled && numDaysAfterPayPeriod.Text=="0") {
				MsgBox.Show(this,"# Days After Pay Period cannot be zero.");
				return;
			}
			_listPayPeriods.Clear();
			int numPeriods=PIn.Int(numPayPeriods.Text);
			PayPeriodInterval payPeriodInterval=PayPeriodInterval.Weekly;
			if(radioBiWeekly.Checked) {
				payPeriodInterval=PayPeriodInterval.BiWeekly;
			}
			else if(radioMonthly.Checked) {
				payPeriodInterval=PayPeriodInterval.Monthly;
			}
			DateTime startDate=dateTimeStart.Value;//Original value
			for(int i=0;i<numPeriods;i++) {
				PayPeriod payPeriod=new PayPeriod();
				payPeriod.DateStart=startDate;
				//Make PayDate information
				switch(payPeriodInterval) {//Add time to "startDate" to get the new start date for the next iteration as well as figuring out the end date for current payperiod.
					case PayPeriodInterval.Weekly:
						payPeriod.DateStop=startDate.AddDays(6);
						startDate=startDate.AddDays(7);
						break;
					case PayPeriodInterval.BiWeekly:
						payPeriod.DateStop=startDate.AddDays(13);
						startDate=startDate.AddDays(14);
						break;
					case PayPeriodInterval.Monthly:
						payPeriod.DateStop=startDate.AddMonths(1).Subtract(TimeSpan.FromDays(1));
						startDate=startDate.AddMonths(1);
						break;
				}
				if(comboDay.Enabled) {
					//Find the closest day specified after the end of the pay period.
					payPeriod.DatePaycheck=GetDateOfDay(payPeriod.DateStop,(DayOfWeek)(comboDay.SelectedIndex-1));
				}
				else {//# days specified, use "Exclude Weekends" checkbox as well as "Pay Before" and "Pay After" buttons.
					payPeriod.DatePaycheck=payPeriod.DateStop.AddDays(PIn.Int(numDaysAfterPayPeriod.Text));
					if(payPeriod.DatePaycheck.DayOfWeek==DayOfWeek.Saturday && checkExcludeWeekends.Checked) {
						if(radioPayBefore.Checked) {
							if(payPeriod.DatePaycheck.Subtract(TimeSpan.FromDays(1))<=payPeriod.DateStop) {//Can't move the paycheck date to the same day (or before) than the date end.
								payPeriod.DatePaycheck=payPeriod.DatePaycheck.Add(TimeSpan.FromDays(2));//Move it forward to monday
							}
							else {
								payPeriod.DatePaycheck=payPeriod.DatePaycheck.Subtract(TimeSpan.FromDays(1));//Move it back to friday
							}
						}
						else {//radioPayAfter
							payPeriod.DatePaycheck=payPeriod.DatePaycheck.Add(TimeSpan.FromDays(2));//Move it forward to monday
						}
					}
					else if(payPeriod.DatePaycheck.DayOfWeek==DayOfWeek.Sunday && checkExcludeWeekends.Checked) {
						if(radioPayBefore.Checked) {
							if(payPeriod.DatePaycheck.Subtract(TimeSpan.FromDays(2))<=payPeriod.DateStop) {//Can't move the paycheck date to the same day (or before) than the date end.
								payPeriod.DatePaycheck=payPeriod.DatePaycheck.Add(TimeSpan.FromDays(1));//Move it forward to monday
							}
							else {
								payPeriod.DatePaycheck=payPeriod.DatePaycheck.Subtract(TimeSpan.FromDays(2));//Move it back to friday
							}
						}
						else {//radioPayAfter
							payPeriod.DatePaycheck=payPeriod.DatePaycheck.Add(TimeSpan.FromDays(1));//Move it forward to monday
						}
					}
				}
				_listPayPeriods.Add(payPeriod);
			}
			FillGrid();
		}

		///<summary>Returns the DateTime of the first instance of DayOfWeek, given a specific start time.  It will not include the startDate as a result.</summary>
		private DateTime GetDateOfDay(DateTime startDate,DayOfWeek day) {
			DateTime result=startDate.AddDays(1);//PayDate cannot be the same as the last day of the pay period.
			while(result.DayOfWeek!=day) {
        result=result.AddDays(1);
			}
			return result;
		}

		private void radioMonthly_Click(object sender,EventArgs e) {
			numPayPeriods.Text="12";
		}

		private void radioBiWeekly_Click(object sender,EventArgs e) {
			numPayPeriods.Text="21";
		}

		private void radioWeekly_Click(object sender,EventArgs e) {
			numPayPeriods.Text="52";
		}

		private void comboDay_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboDay.SelectedIndex!=0) {
				numDaysAfterPayPeriod.Text="0";
				numDaysAfterPayPeriod.Enabled=false;
				checkExcludeWeekends.Enabled=false;
				checkExcludeWeekends.Checked=false;
				radioPayBefore.Enabled=false;
				radioPayBefore.Checked=false;
				radioPayAfter.Enabled=false;
				radioPayAfter.Checked=false;
			}
			else {//none selected
				numDaysAfterPayPeriod.Text="0";
				numDaysAfterPayPeriod.Enabled=true;
				checkExcludeWeekends.Enabled=true;
				radioPayBefore.Enabled=true;
				radioPayAfter.Enabled=true;
			}
		}

		private void numDaysAfterPayPeriod_TextChanged(object sender,EventArgs e) {
			if(numDaysAfterPayPeriod.Text!="0" && numDaysAfterPayPeriod.Text!="") {
				comboDay.SelectedIndex=0;
				comboDay.Enabled=false;
				checkExcludeWeekends.Enabled=true;
				radioPayBefore.Enabled=true;
				radioPayAfter.Enabled=true;
			}
			else {
				comboDay.Enabled=true;
			}
		}

		private void checkExcludeWeekends_CheckedChanged(object sender,EventArgs e) {
			if(!checkExcludeWeekends.Checked) {
				radioPayBefore.Checked=false;
				radioPayBefore.Enabled=false;
				radioPayAfter.Checked=false;
				radioPayAfter.Enabled=false;
			}
			else {
				radioPayBefore.Enabled=true;
				radioPayAfter.Enabled=true;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(gridMain.Rows.Count==0) {
				MsgBox.Show(this,"Pay periods must be generated first.");
				return;
			}
			if(numDaysAfterPayPeriod.errorProvider1.GetError(numDaysAfterPayPeriod)!="") {
				MsgBox.Show(this,numDaysAfterPayPeriod.errorProvider1.GetError(numDaysAfterPayPeriod));
				return;
			}
			PayPeriods.RefreshCache(); //Refresh the cache to include any other changes that might have been made in FormTimeCardSetup.
			//overlapping logic
			if(PayPeriods.AreAnyOverlapping(PayPeriods.GetDeepCopy(),_listPayPeriods)) {
				MsgBox.Show(this,"You have created pay periods that would overlap with existing pay periods. Please fix those pay periods first.");
				return;
			}
			//Save payperiods
			foreach(PayPeriod payPeriod in _listPayPeriods) {//PayPeriods are always new in this form.
				PayPeriods.Insert(payPeriod);
			}
			//Save Preferences
			if(radioWeekly.Checked) {
				Prefs.UpdateInt(PrefName.PayPeriodIntervalSetting,(int)PayPeriodInterval.Weekly);
			}
			else if(radioBiWeekly.Checked) {
				Prefs.UpdateInt(PrefName.PayPeriodIntervalSetting,(int)PayPeriodInterval.BiWeekly);
			}
			else {
				Prefs.UpdateInt(PrefName.PayPeriodIntervalSetting,(int)PayPeriodInterval.Monthly);
			}
			Prefs.UpdateInt(PrefName.PayPeriodPayDay,comboDay.SelectedIndex);
			Prefs.UpdateInt(PrefName.PayPeriodPayAfterNumberOfDays,PIn.Int(numDaysAfterPayPeriod.Text));
			Prefs.UpdateBool(PrefName.PayPeriodPayDateExcludesWeekends,checkExcludeWeekends.Checked);
			if(radioPayBefore.Checked) {
				Prefs.UpdateBool(PrefName.PayPeriodPayDateBeforeWeekend,true);
			}
			else if(radioPayAfter.Checked) {
				Prefs.UpdateBool(PrefName.PayPeriodPayDateBeforeWeekend,false);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}





















