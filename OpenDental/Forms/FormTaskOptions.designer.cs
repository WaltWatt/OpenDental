namespace OpenDental{
	partial class FormTaskOptions {
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTaskOptions));
			this.butOK = new OpenDental.UI.Button();
			this.checkShowFinished = new System.Windows.Forms.CheckBox();
			this.textStartDate = new OpenDental.ValidDate();
			this.labelStartDate = new System.Windows.Forms.Label();
			this.checkTaskSortApptDateTime = new System.Windows.Forms.CheckBox();
			this.checkCollapsed = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(281, 96);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// checkShowFinished
			// 
			this.checkShowFinished.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.checkShowFinished.Location = new System.Drawing.Point(12, 12);
			this.checkShowFinished.Name = "checkShowFinished";
			this.checkShowFinished.Size = new System.Drawing.Size(151, 15);
			this.checkShowFinished.TabIndex = 11;
			this.checkShowFinished.Text = "Show Finished Tasks";
			this.checkShowFinished.UseVisualStyleBackColor = true;
			this.checkShowFinished.Click += new System.EventHandler(this.checkShowFinished_Click);
			// 
			// textStartDate
			// 
			this.textStartDate.Location = new System.Drawing.Point(192, 29);
			this.textStartDate.Name = "textStartDate";
			this.textStartDate.Size = new System.Drawing.Size(87, 20);
			this.textStartDate.TabIndex = 13;
			// 
			// labelStartDate
			// 
			this.labelStartDate.Location = new System.Drawing.Point(38, 30);
			this.labelStartDate.Name = "labelStartDate";
			this.labelStartDate.Size = new System.Drawing.Size(153, 17);
			this.labelStartDate.TabIndex = 14;
			this.labelStartDate.Text = "Finished Task Start Date";
			this.labelStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkTaskSortApptDateTime
			// 
			this.checkTaskSortApptDateTime.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.checkTaskSortApptDateTime.Location = new System.Drawing.Point(12, 55);
			this.checkTaskSortApptDateTime.Name = "checkTaskSortApptDateTime";
			this.checkTaskSortApptDateTime.Size = new System.Drawing.Size(299, 17);
			this.checkTaskSortApptDateTime.TabIndex = 15;
			this.checkTaskSortApptDateTime.Text = "Sort appointment type task lists by AptDateTime";
			this.checkTaskSortApptDateTime.UseVisualStyleBackColor = true;
			// 
			// checkCollapsed
			// 
			this.checkCollapsed.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.checkCollapsed.Location = new System.Drawing.Point(12, 78);
			this.checkCollapsed.Name = "checkCollapsed";
			this.checkCollapsed.Size = new System.Drawing.Size(299, 17);
			this.checkCollapsed.TabIndex = 16;
			this.checkCollapsed.Text = "Default tasks to collapsed state";
			this.checkCollapsed.UseVisualStyleBackColor = true;
			// 
			// FormTaskOptions
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(368, 132);
			this.ControlBox = false;
			this.Controls.Add(this.checkCollapsed);
			this.Controls.Add(this.checkTaskSortApptDateTime);
			this.Controls.Add(this.textStartDate);
			this.Controls.Add(this.labelStartDate);
			this.Controls.Add(this.checkShowFinished);
			this.Controls.Add(this.butOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormTaskOptions";
			this.Text = "Task Options";
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.CheckBox checkShowFinished;
		private ValidDate textStartDate;
		private System.Windows.Forms.Label labelStartDate;
		private System.Windows.Forms.CheckBox checkTaskSortApptDateTime;
		private System.Windows.Forms.CheckBox checkCollapsed;
	}
}