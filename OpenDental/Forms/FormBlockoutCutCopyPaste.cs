using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormBlockoutCutCopyPaste:ODForm {
		private GroupBox groupBox1;
		private OpenDental.UI.Button butCopyWeek;
		private OpenDental.UI.Button butCopyDay;
		private GroupBox groupBox2;
		private OpenDental.UI.Button butRepeat;
		private Label label4;
		private CheckBox checkReplace;
		private TextBox textRepeat;
		private OpenDental.UI.Button butPaste;
		private CheckBox checkWeekend;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private TextBox textClipboard;
		private Label label3;
		private Label label1;
		private static DateTime DateCopyStart=DateTime.MinValue;
		private static DateTime DateCopyEnd=DateTime.MinValue;
		public long ApptViewNumCur;
		private static long ApptViewNumPrevious;
		private OpenDental.UI.Button butClearDay;
		public DateTime DateSelected;

		///<summary></summary>
		public FormBlockoutCutCopyPaste()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBlockoutCutCopyPaste));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textClipboard = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.butCopyWeek = new OpenDental.UI.Button();
			this.butCopyDay = new OpenDental.UI.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.butRepeat = new OpenDental.UI.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.checkReplace = new System.Windows.Forms.CheckBox();
			this.textRepeat = new System.Windows.Forms.TextBox();
			this.butPaste = new OpenDental.UI.Button();
			this.checkWeekend = new System.Windows.Forms.CheckBox();
			this.butClearDay = new OpenDental.UI.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textClipboard);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.butCopyWeek);
			this.groupBox1.Controls.Add(this.butCopyDay);
			this.groupBox1.Location = new System.Drawing.Point(26, 50);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(158, 198);
			this.groupBox1.TabIndex = 40;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Copy";
			// 
			// textClipboard
			// 
			this.textClipboard.Location = new System.Drawing.Point(6, 113);
			this.textClipboard.Name = "textClipboard";
			this.textClipboard.ReadOnly = true;
			this.textClipboard.Size = new System.Drawing.Size(146, 20);
			this.textClipboard.TabIndex = 30;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(149, 80);
			this.label1.TabIndex = 47;
			this.label1.Text = "Copying only applies to the visible operatories for the current appointment view." +
    " It also does not copy to a different operatory.";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(3, 96);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(146, 14);
			this.label3.TabIndex = 29;
			this.label3.Text = "Clipboard Contents";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butCopyWeek
			// 
			this.butCopyWeek.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCopyWeek.Autosize = true;
			this.butCopyWeek.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCopyWeek.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCopyWeek.CornerRadius = 4F;
			this.butCopyWeek.Location = new System.Drawing.Point(6, 165);
			this.butCopyWeek.Name = "butCopyWeek";
			this.butCopyWeek.Size = new System.Drawing.Size(75, 24);
			this.butCopyWeek.TabIndex = 28;
			this.butCopyWeek.Text = "Copy Week";
			this.butCopyWeek.Click += new System.EventHandler(this.butCopyWeek_Click);
			// 
			// butCopyDay
			// 
			this.butCopyDay.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCopyDay.Autosize = true;
			this.butCopyDay.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCopyDay.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCopyDay.CornerRadius = 4F;
			this.butCopyDay.Location = new System.Drawing.Point(6, 138);
			this.butCopyDay.Name = "butCopyDay";
			this.butCopyDay.Size = new System.Drawing.Size(75, 24);
			this.butCopyDay.TabIndex = 27;
			this.butCopyDay.Text = "Copy Day";
			this.butCopyDay.Click += new System.EventHandler(this.butCopyDay_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.butRepeat);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.checkReplace);
			this.groupBox2.Controls.Add(this.textRepeat);
			this.groupBox2.Controls.Add(this.butPaste);
			this.groupBox2.Location = new System.Drawing.Point(26, 263);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(158, 97);
			this.groupBox2.TabIndex = 45;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Paste";
			// 
			// butRepeat
			// 
			this.butRepeat.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRepeat.Autosize = true;
			this.butRepeat.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRepeat.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRepeat.CornerRadius = 4F;
			this.butRepeat.Location = new System.Drawing.Point(6, 64);
			this.butRepeat.Name = "butRepeat";
			this.butRepeat.Size = new System.Drawing.Size(75, 24);
			this.butRepeat.TabIndex = 30;
			this.butRepeat.Text = "Repeat";
			this.butRepeat.Click += new System.EventHandler(this.butRepeat_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(70, 70);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(37, 14);
			this.label4.TabIndex = 32;
			this.label4.Text = "#";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// checkReplace
			// 
			this.checkReplace.Checked = true;
			this.checkReplace.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkReplace.Location = new System.Drawing.Point(6, 14);
			this.checkReplace.Name = "checkReplace";
			this.checkReplace.Size = new System.Drawing.Size(146, 18);
			this.checkReplace.TabIndex = 31;
			this.checkReplace.Text = "Replace Existing";
			this.checkReplace.UseVisualStyleBackColor = true;
			// 
			// textRepeat
			// 
			this.textRepeat.Location = new System.Drawing.Point(110, 67);
			this.textRepeat.Name = "textRepeat";
			this.textRepeat.Size = new System.Drawing.Size(39, 20);
			this.textRepeat.TabIndex = 31;
			this.textRepeat.Text = "1";
			// 
			// butPaste
			// 
			this.butPaste.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPaste.Autosize = true;
			this.butPaste.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPaste.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPaste.CornerRadius = 4F;
			this.butPaste.Location = new System.Drawing.Point(6, 37);
			this.butPaste.Name = "butPaste";
			this.butPaste.Size = new System.Drawing.Size(75, 24);
			this.butPaste.TabIndex = 29;
			this.butPaste.Text = "Paste";
			this.butPaste.Click += new System.EventHandler(this.butPaste_Click);
			// 
			// checkWeekend
			// 
			this.checkWeekend.Location = new System.Drawing.Point(123, 16);
			this.checkWeekend.Name = "checkWeekend";
			this.checkWeekend.Size = new System.Drawing.Size(143, 18);
			this.checkWeekend.TabIndex = 46;
			this.checkWeekend.Text = "Include Weekends";
			this.checkWeekend.UseVisualStyleBackColor = true;
			// 
			// butClearDay
			// 
			this.butClearDay.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClearDay.Autosize = true;
			this.butClearDay.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClearDay.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClearDay.CornerRadius = 4F;
			this.butClearDay.Location = new System.Drawing.Point(32, 12);
			this.butClearDay.Name = "butClearDay";
			this.butClearDay.Size = new System.Drawing.Size(75, 24);
			this.butClearDay.TabIndex = 48;
			this.butClearDay.Text = "Clear Day";
			this.butClearDay.Click += new System.EventHandler(this.butClearDay_Click);
			// 
			// FormBlockoutCutCopyPaste
			// 
			this.ClientSize = new System.Drawing.Size(290, 383);
			this.Controls.Add(this.butClearDay);
			this.Controls.Add(this.checkWeekend);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormBlockoutCutCopyPaste";
			this.ShowInTaskbar = false;
			this.Text = "Blockout Cut-Copy-Paste";
			this.Load += new System.EventHandler(this.FormBlockoutCutCopyPaste_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormBlockoutCutCopyPaste_Load(object sender,EventArgs e) {
			if(ApptViewNumCur!=ApptViewNumPrevious){
				DateCopyStart=DateTime.MinValue;
				DateCopyEnd=DateTime.MinValue;
			}
			FillClipboard();
			ApptViewNumPrevious=ApptViewNumCur;//remember the appt view for next time.
		}

		private void butClearDay_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,true,"Clear all blockouts for day? (This may include blockouts not shown in the current appointment view)")) {
				return;
			}
			Schedules.ClearBlockoutsForDay(DateSelected);//works for daily or weekly
			Close();
		}

		private void FillClipboard(){
			if(DateCopyStart.Year<1880){
				textClipboard.Text="";
			}
			else if(DateCopyStart==DateCopyEnd) {
				textClipboard.Text=DateCopyStart.ToShortDateString();
			}
			else {
				textClipboard.Text=DateCopyStart.ToShortDateString()+"-"+DateCopyEnd.ToShortDateString();
			}
		}

		private void butCopyDay_Click(object sender,EventArgs e) {
			DateCopyStart=DateSelected;
			DateCopyEnd=DateSelected;
			//FillClipboard();
			Close();
		}

		private void butCopyWeek_Click(object sender,EventArgs e) {
			//Always start week on Monday
			if(DateSelected.DayOfWeek==DayOfWeek.Sunday) {//if selecting Sunday, go back to the previous Monday.
				DateCopyStart=DateSelected.AddDays(-6);
			}
			else {//Any other day. eg Wed.AddDays(1-3)=Wed.AddDays(-2)=Monday
				DateCopyStart=DateSelected.AddDays(1-(int)DateSelected.DayOfWeek);//eg Wed.AddDays(1-3)=Wed.AddDays(-2)=Monday
			}
			if(checkWeekend.Checked){
				DateCopyEnd=DateCopyStart.AddDays(6);
			}
			else{
				DateCopyEnd=DateCopyStart.AddDays(4);
			}
			//FillClipboard();
			Close();
		}

		private void butPaste_Click(object sender,EventArgs e) {
			CopyOverBlockouts(1,"Paste");
		}

		private void butRepeat_Click(object sender,EventArgs e) {
			try {
				int.Parse(textRepeat.Text);
			}
			catch {
				MsgBox.Show(this,"Please fix number box first.");
				return;
			}
			CopyOverBlockouts(PIn.Long(textRepeat.Text),"Repeat");
		}

		private void CopyOverBlockouts(long numRepeat,string type) {
			if(DateCopyStart.Year<1880) {
				MsgBox.Show(this,"Please copy a selection to the clipboard first.");
				return;
			}
			//calculate which day or week is currently selected.
			DateTime dateSelectedStart;
			DateTime dateSelectedEnd;
			bool isWeek=DateCopyStart!=DateCopyEnd;
			if(isWeek) {
				//Always start week on Monday
				if(DateSelected.DayOfWeek==DayOfWeek.Sunday) {//if selecting Sunday, go back to the previous Monday.
					dateSelectedStart=DateSelected.AddDays(-6);
				}
				else {//Any other day. eg Wed.AddDays(1-3)=Wed.AddDays(-2)=Monday
					dateSelectedStart=DateSelected.AddDays(1-(int)DateSelected.DayOfWeek);//eg Wed.AddDays(1-3)=Wed.AddDays(-2)=Monday
				}
				//DateCopyEnd is greater than DateCopyStart and is either 4 days greater or 6 days greater, so clear/paste the same number of days
				dateSelectedEnd=dateSelectedStart.AddDays((DateCopyEnd-DateCopyStart).Days);
			}
			else {
				dateSelectedStart=DateSelected;
				dateSelectedEnd=DateSelected;
			}
			//When pasting, it's not allowed to paste back over the same day or week.
			if(dateSelectedStart==DateCopyStart && type=="Paste") {
				MsgBox.Show(this,"Not allowed to paste back onto the same date as is on the clipboard.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			//it is allowed to paste back over the same day or week.
			List<long> opNums=ApptViewItems.GetOpsForView(ApptViewNumCur);
			List<Schedule> SchedList=Schedules.RefreshPeriodBlockouts(DateCopyStart,DateCopyEnd,opNums);
			//Build a list of blockouts that can't be Cut/Copy/Pasted
			List<Def> listUserBlockoutDefs=Defs.GetDefsForCategory(DefCat.BlockoutTypes, true)
				.FindAll(x => x.ItemValue.Contains(BlockoutType.DontCopy.GetDescription()));
			//No SchedList only contains blockouts that are NOT marked "Do not Cut/Copy/Paste"
			SchedList.RemoveAll(x => listUserBlockoutDefs.Any(y => y.DefNum==x.BlockoutType));
			Schedule sched;
			int weekDelta=0;
			TimeSpan span;
			if(isWeek) {
				span=dateSelectedStart-DateCopyStart;
				weekDelta=span.Days/7;//usually a positive # representing a future paste, but can be negative
			}
			int dayDelta=0;//this is needed when repeat pasting days in order to calculate skipping weekends.
			//dayDelta will start out zero and increment separately from r.
			for(int r=0;r<numRepeat;r++) {
				if(checkReplace.Checked) {
					if(type=="Repeat") {
						if(isWeek) {
							Schedules.ClearBlockouts(dateSelectedStart.AddDays(r*7),dateSelectedEnd.AddDays(r*7),opNums);
						}
						else {
							//dateSelectedStart will equal dateSelectedEnd if repeating a single day
							Schedules.ClearBlockouts(dateSelectedStart.AddDays(dayDelta),dateSelectedEnd.AddDays(dayDelta),opNums);
						}
					}
					else {
						Schedules.ClearBlockouts(dateSelectedStart,dateSelectedEnd,opNums);
					}
				}
				List<Schedule> listNewScheds=new List<Schedule>();
				for(int i=0;i<SchedList.Count;i++) {
					sched=SchedList[i].Copy();
					sched.ScheduleNum=0;//So that overlap logic works.
					if(isWeek) {
						sched.SchedDate=sched.SchedDate.AddDays((weekDelta+r)*7);
					}
					else {
						sched.SchedDate=dateSelectedStart.AddDays(dayDelta);
					}
					if(!checkReplace.Checked && Schedules.Overlaps(sched)) {
						MessageBox.Show(Lans.g("Schedule","A blockout overlaps with an existing blockout. Could not paste the blockout on")
							+" "+sched.SchedDate.ToShortDateString()+" "+sched.StartTime.ToShortTimeString());
						Cursor=Cursors.Default;
						return;
					}
					listNewScheds.Add(sched);
				}
				//dayDelta is only used for repeating single days, not for repeating weeks, so we don't need to determine whether or not they copied weekends, we can rely on checkWeekend.Checked
				if(!checkWeekend.Checked && dateSelectedStart.AddDays(dayDelta).DayOfWeek==DayOfWeek.Friday) {
					dayDelta+=3;
				}
				else {
					dayDelta++;
				}
				//This is located outside of the previous for loop because we do not want to insert any blockouts unless there were no overlaps.
				foreach(Schedule schedule in listNewScheds) {
					Schedules.Insert(schedule,true);//Doing it this way makes use of validation to prevent overlaps
				}
			}
			Cursor=Cursors.Default;
			Close();
		}
	}
}