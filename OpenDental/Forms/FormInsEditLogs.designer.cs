namespace OpenDental{
	partial class FormInsEditLogs {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInsEditLogs));
			this.butCancel = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.textDateTo = new System.Windows.Forms.TextBox();
			this.textDateFrom = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.timerSearch = new System.Windows.Forms.Timer(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
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
			this.butCancel.Location = new System.Drawing.Point(928, 570);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Close";
			this.butCancel.Click += new System.EventHandler(this.butClose_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = true;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(8, 34);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(995, 524);
			this.gridMain.TabIndex = 4;
			this.gridMain.Title = "Logs";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableLogs";
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(269, 8);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(137, 20);
			this.textDateTo.TabIndex = 185;
			this.textDateTo.TextChanged += new System.EventHandler(this.textSearches_TextChanged);
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(69, 8);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(137, 20);
			this.textDateFrom.TabIndex = 184;
			this.textDateFrom.TextChanged += new System.EventHandler(this.textSearches_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(208, 10);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(60, 15);
			this.label3.TabIndex = 19;
			this.label3.Text = "To:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 10);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(60, 15);
			this.label2.TabIndex = 17;
			this.label2.Text = "From:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// timerSearch
			// 
			this.timerSearch.Interval = 500;
			this.timerSearch.Tick += new System.EventHandler(this.timerSearch_Tick);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(412, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(587, 17);
			this.label1.TabIndex = 186;
			this.label1.Text = "Logs generated prior to version 17.3.22 will not display descriptions.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormInsEditLogs
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(1011, 602);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textDateTo);
			this.Controls.Add(this.textDateFrom);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.gridMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormInsEditLogs";
			this.Text = "Insurance Plan Edit Log";
			this.Load += new System.EventHandler(this.FormInsEditLogs_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private OpenDental.UI.Button butCancel;
		private UI.ODGrid gridMain;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Timer timerSearch;
		private System.Windows.Forms.TextBox textDateTo;
		private System.Windows.Forms.TextBox textDateFrom;
		private System.Windows.Forms.Label label1;
	}
}