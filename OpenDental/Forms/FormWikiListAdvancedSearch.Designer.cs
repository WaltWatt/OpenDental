namespace OpenDental {
	partial class FormWikiListAdvancedSearch {
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
			this.gridMain = new OpenDental.UI.ODGrid();
			this.labelSummary = new System.Windows.Forms.Label();
			this.butOkay = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(31, 49);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(253, 209);
			this.gridMain.TabIndex = 0;
			this.gridMain.Title = "Columns";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableWikiListAdvancedSearch";
			// 
			// labelSummary
			// 
			this.labelSummary.Location = new System.Drawing.Point(12, 9);
			this.labelSummary.Name = "labelSummary";
			this.labelSummary.Size = new System.Drawing.Size(307, 37);
			this.labelSummary.TabIndex = 1;
			this.labelSummary.Text = "Choose one or more columns to be used within the search of the Wiki List Edit win" +
    "dow.\r\n";
			// 
			// butOkay
			// 
			this.butOkay.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOkay.Autosize = true;
			this.butOkay.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOkay.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOkay.CornerRadius = 4F;
			this.butOkay.Location = new System.Drawing.Point(163, 271);
			this.butOkay.Name = "butOkay";
			this.butOkay.Size = new System.Drawing.Size(75, 23);
			this.butOkay.TabIndex = 2;
			this.butOkay.Text = "OK";
			this.butOkay.UseVisualStyleBackColor = true;
			this.butOkay.Click += new System.EventHandler(this.butOkay_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(244, 271);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 23);
			this.butCancel.TabIndex = 3;
			this.butCancel.Text = "Cancel";
			this.butCancel.UseVisualStyleBackColor = true;
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormWikiListAdvancedSearch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(335, 306);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOkay);
			this.Controls.Add(this.labelSummary);
			this.Controls.Add(this.gridMain);
			this.Name = "FormWikiListAdvancedSearch";
			this.Text = "Wiki List Advanced Search";
			this.Load += new System.EventHandler(this.FormWikiListAdvancedSearch_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private UI.ODGrid gridMain;
		private System.Windows.Forms.Label labelSummary;
		private UI.Button butOkay;
		private UI.Button butCancel;
	}
}