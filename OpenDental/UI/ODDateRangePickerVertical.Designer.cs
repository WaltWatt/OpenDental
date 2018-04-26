namespace OpenDental.UI {
	partial class ODDateRangePickerVertical {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ODDateRangePickerVertical));
			this.panelWeek.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageListCalendar
			// 
			this.imageListCalendar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListCalendar.ImageStream")));
			this.imageListCalendar.Images.SetKeyName(0, "arrowDownTriangle.gif");
			this.imageListCalendar.Images.SetKeyName(1, "arrowUpTriangle.gif");
			// 
			// butWeekPrevious
			// 
			this.butWeekPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
			this.butWeekPrevious.Location = new System.Drawing.Point(2, 0);
			// 
			// butWeekNext
			// 
			this.butWeekNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
			this.butWeekNext.Location = new System.Drawing.Point(37, 0);
			// 
			// calendarFrom
			// 
			this.calendarFrom.Location = new System.Drawing.Point(154, 0);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 1);
			// 
			// calendarTo
			// 
			this.calendarTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
			this.calendarTo.Location = new System.Drawing.Point(154, 21);
			// 
			// butDropTo
			// 
			this.butDropTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
			this.butDropTo.Location = new System.Drawing.Point(135, 22);
			// 
			// butDropFrom
			// 
			this.butDropFrom.Location = new System.Drawing.Point(135, 1);
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(50, 0);
			this.textDateFrom.Text = "1/1/2018";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.Location = new System.Drawing.Point(8, 22);
			// 
			// textDateTo
			// 
			this.textDateTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
			this.textDateTo.Location = new System.Drawing.Point(50, 21);
			this.textDateTo.Text = "4/9/2018";
			// 
			// panelCalendarGap
			// 
			this.panelCalendarGap.Size = new System.Drawing.Size(0, 163);
			// 
			// panelCalendarBoarders
			// 
			this.panelCalendarBoarders.BackColor = System.Drawing.Color.Transparent;
			this.panelCalendarBoarders.Location = new System.Drawing.Point(437, 8);
			this.panelCalendarBoarders.Size = new System.Drawing.Size(0, 0);
			this.panelCalendarBoarders.Visible = false;
			// 
			// panelMidLines
			// 
			this.panelMidLines.BackColor = System.Drawing.Color.Transparent;
			this.panelMidLines.Location = new System.Drawing.Point(409, 7);
			this.panelMidLines.Size = new System.Drawing.Size(0, 0);
			this.panelMidLines.Visible = false;
			// 
			// panelWeek
			// 
			this.panelWeek.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
			this.panelWeek.Location = new System.Drawing.Point(81, 43);
			this.panelWeek.Size = new System.Drawing.Size(73, 24);
			this.panelWeek.Visible = false;
			// 
			// ODDateRangePickerVertical
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.MaximumSize = new System.Drawing.Size(453, 185);
			this.MinimumSize = new System.Drawing.Size(381, 70);
			this.Name = "ODDateRangePickerVertical";
			this.Size = new System.Drawing.Size(381, 70);
			this.panelWeek.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
	}
}
