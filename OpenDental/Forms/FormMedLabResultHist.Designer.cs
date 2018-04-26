namespace OpenDental {
	partial class FormMedLabResultHist {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMedLabResultHist));
			this.gridResultHist = new OpenDental.UI.ODGrid();
			this.butClose = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// gridResultHist
			// 
			this.gridResultHist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridResultHist.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridResultHist.HasAddButton = false;
			this.gridResultHist.HasDropDowns = false;
			this.gridResultHist.HasMultilineHeaders = false;
			this.gridResultHist.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridResultHist.HeaderHeight = 15;
			this.gridResultHist.HScrollVisible = false;
			this.gridResultHist.Location = new System.Drawing.Point(12, 12);
			this.gridResultHist.Name = "gridResultHist";
			this.gridResultHist.ScrollValue = 0;
			this.gridResultHist.Size = new System.Drawing.Size(950, 352);
			this.gridResultHist.TabIndex = 5;
			this.gridResultHist.Title = "Result History";
			this.gridResultHist.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridResultHist.TitleHeight = 18;
			this.gridResultHist.TranslationName = "TableResult";
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(887, 374);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 335;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormMedLabResultHist
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(974, 409);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.gridResultHist);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(774, 156);
			this.Name = "FormMedLabResultHist";
			this.Text = "MedLab Result History";
			this.Load += new System.EventHandler(this.FormMedLabResultHist_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.ODGrid gridResultHist;
		private UI.Button butClose;
	}
}