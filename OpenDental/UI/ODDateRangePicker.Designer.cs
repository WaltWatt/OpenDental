namespace OpenDental.UI {
	partial class ODDateRangePicker {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing&&(components!=null)) {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ODDateRangePicker));
			this.imageListCalendar = new System.Windows.Forms.ImageList(this.components);
			this.calendarFrom = new System.Windows.Forms.MonthCalendar();
			this.label1 = new System.Windows.Forms.Label();
			this.calendarTo = new System.Windows.Forms.MonthCalendar();
			this.label2 = new System.Windows.Forms.Label();
			this.panelWeek = new OpenDental.UI.PanelOD();
			this.butWeekPrevious = new OpenDental.UI.Button();
			this.butWeekNext = new OpenDental.UI.Button();
			this.panelMidLines = new OpenDental.UI.PanelOD();
			this.panelCalendarBoarders = new OpenDental.UI.PanelOD();
			this.panelCalendarGap = new OpenDental.UI.PanelOD();
			this.butDropTo = new OpenDental.UI.Button();
			this.butDropFrom = new OpenDental.UI.Button();
			this.textDateTo = new OpenDental.ValidDate();
			this.textDateFrom = new OpenDental.ValidDate();
			this.panelWeek.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageListCalendar
			// 
			this.imageListCalendar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListCalendar.ImageStream")));
			this.imageListCalendar.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListCalendar.Images.SetKeyName(0, "arrowDownTriangle.gif");
			this.imageListCalendar.Images.SetKeyName(1, "arrowUpTriangle.gif");
			// 
			// calendarFrom
			// 
			this.calendarFrom.Location = new System.Drawing.Point(0, 23);
			this.calendarFrom.MaxSelectionCount = 1;
			this.calendarFrom.Name = "calendarFrom";
			this.calendarFrom.TabIndex = 62;
			this.calendarFrom.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calendarFrom_DateSelected);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(21, 2);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(47, 18);
			this.label1.TabIndex = 58;
			this.label1.Text = "From";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// calendarTo
			// 
			this.calendarTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.calendarTo.Location = new System.Drawing.Point(226, 23);
			this.calendarTo.MaxSelectionCount = 1;
			this.calendarTo.Name = "calendarTo";
			this.calendarTo.TabIndex = 65;
			this.calendarTo.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calendarTo_DateSelected);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(265, 2);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(36, 18);
			this.label2.TabIndex = 60;
			this.label2.Text = "To";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panelWeek
			// 
			this.panelWeek.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.panelWeek.BorderColor = System.Drawing.Color.Transparent;
			this.panelWeek.Controls.Add(this.butWeekPrevious);
			this.panelWeek.Controls.Add(this.butWeekNext);
			this.panelWeek.Location = new System.Drawing.Point(189, -1);
			this.panelWeek.Name = "panelWeek";
			this.panelWeek.Size = new System.Drawing.Size(74, 24);
			this.panelWeek.TabIndex = 71;
			// 
			// butWeekPrevious
			// 
			this.butWeekPrevious.AdjustImageLocation = new System.Drawing.Point(-3, -1);
			this.butWeekPrevious.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.butWeekPrevious.Autosize = true;
			this.butWeekPrevious.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butWeekPrevious.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butWeekPrevious.CornerRadius = 4F;
			this.butWeekPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butWeekPrevious.Image = ((System.Drawing.Image)(resources.GetObject("butWeekPrevious.Image")));
			this.butWeekPrevious.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butWeekPrevious.Location = new System.Drawing.Point(3, 1);
			this.butWeekPrevious.Name = "butWeekPrevious";
			this.butWeekPrevious.Size = new System.Drawing.Size(33, 22);
			this.butWeekPrevious.TabIndex = 67;
			this.butWeekPrevious.Text = "W";
			this.butWeekPrevious.Click += new System.EventHandler(this.butWeekPrevious_Click);
			// 
			// butWeekNext
			// 
			this.butWeekNext.AdjustImageLocation = new System.Drawing.Point(5, -1);
			this.butWeekNext.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.butWeekNext.Autosize = false;
			this.butWeekNext.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butWeekNext.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butWeekNext.CornerRadius = 4F;
			this.butWeekNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butWeekNext.Image = ((System.Drawing.Image)(resources.GetObject("butWeekNext.Image")));
			this.butWeekNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.butWeekNext.Location = new System.Drawing.Point(39, 1);
			this.butWeekNext.Name = "butWeekNext";
			this.butWeekNext.Size = new System.Drawing.Size(33, 22);
			this.butWeekNext.TabIndex = 66;
			this.butWeekNext.Text = "W";
			this.butWeekNext.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butWeekNext.Click += new System.EventHandler(this.butWeekNext_Click);
			// 
			// panelMidLines
			// 
			this.panelMidLines.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.panelMidLines.BackColor = System.Drawing.SystemColors.Window;
			this.panelMidLines.BorderColor = System.Drawing.SystemColors.ControlDark;
			this.panelMidLines.Location = new System.Drawing.Point(225, 24);
			this.panelMidLines.Name = "panelMidLines";
			this.panelMidLines.Size = new System.Drawing.Size(3, 160);
			this.panelMidLines.TabIndex = 70;
			// 
			// panelCalendarBoarders
			// 
			this.panelCalendarBoarders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelCalendarBoarders.BackColor = System.Drawing.SystemColors.Window;
			this.panelCalendarBoarders.BorderColor = System.Drawing.SystemColors.Window;
			this.panelCalendarBoarders.Location = new System.Drawing.Point(217, 25);
			this.panelCalendarBoarders.Name = "panelCalendarBoarders";
			this.panelCalendarBoarders.Size = new System.Drawing.Size(19, 158);
			this.panelCalendarBoarders.TabIndex = 69;
			// 
			// panelCalendarGap
			// 
			this.panelCalendarGap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelCalendarGap.BackColor = System.Drawing.SystemColors.Window;
			this.panelCalendarGap.BorderColor = System.Drawing.SystemColors.ControlDark;
			this.panelCalendarGap.Location = new System.Drawing.Point(217, 24);
			this.panelCalendarGap.Name = "panelCalendarGap";
			this.panelCalendarGap.Size = new System.Drawing.Size(19, 160);
			this.panelCalendarGap.TabIndex = 68;
			// 
			// butDropTo
			// 
			this.butDropTo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDropTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDropTo.Autosize = true;
			this.butDropTo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDropTo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDropTo.CornerRadius = 4F;
			this.butDropTo.ImageIndex = 0;
			this.butDropTo.ImageList = this.imageListCalendar;
			this.butDropTo.Location = new System.Drawing.Point(391, 2);
			this.butDropTo.Name = "butDropTo";
			this.butDropTo.Size = new System.Drawing.Size(17, 18);
			this.butDropTo.TabIndex = 64;
			this.butDropTo.UseVisualStyleBackColor = true;
			this.butDropTo.Click += new System.EventHandler(this.butToggleCalendarTo_Click);
			// 
			// butDropFrom
			// 
			this.butDropFrom.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDropFrom.Autosize = true;
			this.butDropFrom.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDropFrom.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDropFrom.CornerRadius = 4F;
			this.butDropFrom.ImageIndex = 0;
			this.butDropFrom.ImageList = this.imageListCalendar;
			this.butDropFrom.Location = new System.Drawing.Point(155, 2);
			this.butDropFrom.Name = "butDropFrom";
			this.butDropFrom.Size = new System.Drawing.Size(17, 18);
			this.butDropFrom.TabIndex = 63;
			this.butDropFrom.UseVisualStyleBackColor = true;
			this.butDropFrom.Click += new System.EventHandler(this.butToggleCalendarFrom_Click);
			// 
			// textDateTo
			// 
			this.textDateTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textDateTo.Location = new System.Drawing.Point(307, 1);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(102, 20);
			this.textDateTo.TabIndex = 61;
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(71, 1);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(102, 20);
			this.textDateFrom.TabIndex = 59;
			// 
			// ODDateRangePicker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.panelWeek);
			this.Controls.Add(this.panelMidLines);
			this.Controls.Add(this.panelCalendarBoarders);
			this.Controls.Add(this.panelCalendarGap);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butDropTo);
			this.Controls.Add(this.butDropFrom);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textDateTo);
			this.Controls.Add(this.textDateFrom);
			this.Controls.Add(this.calendarFrom);
			this.Controls.Add(this.calendarTo);
			this.MaximumSize = new System.Drawing.Size(0, 185);
			this.MinimumSize = new System.Drawing.Size(453, 22);
			this.Name = "ODDateRangePicker";
			this.Size = new System.Drawing.Size(453, 185);
			this.Load += new System.EventHandler(this.ODDateRangePicker_Load);
			this.panelWeek.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		protected System.Windows.Forms.ImageList imageListCalendar;
		protected Button butWeekPrevious;
		protected Button butWeekNext;
		protected System.Windows.Forms.MonthCalendar calendarFrom;
		protected System.Windows.Forms.Label label1;
		protected System.Windows.Forms.MonthCalendar calendarTo;
		protected Button butDropTo;
		protected Button butDropFrom;
		protected ValidDate textDateFrom;
		protected System.Windows.Forms.Label label2;
		protected ValidDate textDateTo;
		protected PanelOD panelCalendarGap;
		protected PanelOD panelCalendarBoarders;
		protected PanelOD panelMidLines;
		protected PanelOD panelWeek;
	}
}
