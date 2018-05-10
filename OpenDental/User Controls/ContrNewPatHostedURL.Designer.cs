namespace OpenDental.User_Controls {
	partial class ContrNewPatHostedURL {
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
			this.labelClinicName = new System.Windows.Forms.Label();
			this.textWebFormToLaunch = new System.Windows.Forms.TextBox();
			this.labelSchedulingURL = new System.Windows.Forms.Label();
			this.labelWebFormToLaunch = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.textSchedulingURL = new System.Windows.Forms.TextBox();
			this.gridOptions = new OpenDental.UI.ODGrid();
			this.butCopy = new OpenDental.UI.Button();
			this.butEdit = new OpenDental.UI.Button();
			this.butExpander = new OpenDental.UI.Button();
			this.labelEnabled = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelClinicName
			// 
			this.labelClinicName.Location = new System.Drawing.Point(28, 2);
			this.labelClinicName.Name = "labelClinicName";
			this.labelClinicName.Size = new System.Drawing.Size(287, 21);
			this.labelClinicName.TabIndex = 10;
			this.labelClinicName.Text = "Clinic Name";
			this.labelClinicName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textWebFormToLaunch
			// 
			this.textWebFormToLaunch.Location = new System.Drawing.Point(138, 67);
			this.textWebFormToLaunch.Name = "textWebFormToLaunch";
			this.textWebFormToLaunch.ReadOnly = true;
			this.textWebFormToLaunch.Size = new System.Drawing.Size(374, 20);
			this.textWebFormToLaunch.TabIndex = 8;
			// 
			// labelSchedulingURL
			// 
			this.labelSchedulingURL.Location = new System.Drawing.Point(6, 95);
			this.labelSchedulingURL.Name = "labelSchedulingURL";
			this.labelSchedulingURL.Size = new System.Drawing.Size(126, 13);
			this.labelSchedulingURL.TabIndex = 11;
			this.labelSchedulingURL.Text = "Scheduling URL";
			this.labelSchedulingURL.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// labelWebFormToLaunch
			// 
			this.labelWebFormToLaunch.Location = new System.Drawing.Point(6, 64);
			this.labelWebFormToLaunch.Name = "labelWebFormToLaunch";
			this.labelWebFormToLaunch.Size = new System.Drawing.Size(126, 26);
			this.labelWebFormToLaunch.TabIndex = 12;
			this.labelWebFormToLaunch.Text = "WebForm to launch after scheduling";
			this.labelWebFormToLaunch.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.textSchedulingURL);
			this.panel1.Controls.Add(this.gridOptions);
			this.panel1.Controls.Add(this.butCopy);
			this.panel1.Controls.Add(this.butEdit);
			this.panel1.Controls.Add(this.labelWebFormToLaunch);
			this.panel1.Controls.Add(this.labelSchedulingURL);
			this.panel1.Controls.Add(this.textWebFormToLaunch);
			this.panel1.Location = new System.Drawing.Point(2, 25);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(645, 148);
			this.panel1.TabIndex = 9;
			// 
			// textSchedulingURL
			// 
			this.textSchedulingURL.Location = new System.Drawing.Point(138, 94);
			this.textSchedulingURL.Multiline = true;
			this.textSchedulingURL.Name = "textSchedulingURL";
			this.textSchedulingURL.ReadOnly = true;
			this.textSchedulingURL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textSchedulingURL.Size = new System.Drawing.Size(374, 54);
			this.textSchedulingURL.TabIndex = 319;
			// 
			// gridOptions
			// 
			this.gridOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridOptions.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridOptions.HasAddButton = false;
			this.gridOptions.HasDropDowns = false;
			this.gridOptions.HasMultilineHeaders = false;
			this.gridOptions.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridOptions.HeaderHeight = 15;
			this.gridOptions.HScrollVisible = false;
			this.gridOptions.Location = new System.Drawing.Point(0, 0);
			this.gridOptions.Name = "gridOptions";
			this.gridOptions.ScrollValue = 0;
			this.gridOptions.Size = new System.Drawing.Size(645, 59);
			this.gridOptions.TabIndex = 318;
			this.gridOptions.Title = "Options";
			this.gridOptions.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridOptions.TitleHeight = 18;
			this.gridOptions.TranslationName = "FormEServicesSetup";
			this.gridOptions.WrapText = false;
			this.gridOptions.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridOptions_CellClick);
			// 
			// butCopy
			// 
			this.butCopy.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCopy.Autosize = true;
			this.butCopy.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCopy.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCopy.CornerRadius = 4F;
			this.butCopy.Location = new System.Drawing.Point(518, 125);
			this.butCopy.Name = "butCopy";
			this.butCopy.Size = new System.Drawing.Size(75, 23);
			this.butCopy.TabIndex = 14;
			this.butCopy.Text = "Copy";
			this.butCopy.UseVisualStyleBackColor = true;
			this.butCopy.Click += new System.EventHandler(this.butCopy_Click);
			// 
			// butEdit
			// 
			this.butEdit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEdit.Autosize = true;
			this.butEdit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEdit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEdit.CornerRadius = 4F;
			this.butEdit.Location = new System.Drawing.Point(518, 65);
			this.butEdit.Name = "butEdit";
			this.butEdit.Size = new System.Drawing.Size(75, 23);
			this.butEdit.TabIndex = 13;
			this.butEdit.Text = "Edit";
			this.butEdit.UseVisualStyleBackColor = true;
			this.butEdit.Click += new System.EventHandler(this.butEdit_Click);
			// 
			// butExpander
			// 
			this.butExpander.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butExpander.Autosize = true;
			this.butExpander.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butExpander.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butExpander.CornerRadius = 4F;
			this.butExpander.Location = new System.Drawing.Point(2, 2);
			this.butExpander.Name = "butExpander";
			this.butExpander.Size = new System.Drawing.Size(21, 21);
			this.butExpander.TabIndex = 8;
			this.butExpander.Text = "-";
			this.butExpander.UseVisualStyleBackColor = true;
			this.butExpander.Click += new System.EventHandler(this.butExpander_Click);
			// 
			// labelEnabled
			// 
			this.labelEnabled.Location = new System.Drawing.Point(362, 2);
			this.labelEnabled.Name = "labelEnabled";
			this.labelEnabled.Size = new System.Drawing.Size(285, 21);
			this.labelEnabled.TabIndex = 11;
			this.labelEnabled.Text = "Enabled";
			this.labelEnabled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ContrNewPatHostedURL
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.labelEnabled);
			this.Controls.Add(this.labelClinicName);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.butExpander);
			this.Name = "ContrNewPatHostedURL";
			this.Size = new System.Drawing.Size(650, 175);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private UI.Button butExpander;
		private System.Windows.Forms.Label labelClinicName;
		private System.Windows.Forms.TextBox textWebFormToLaunch;
		private System.Windows.Forms.Label labelSchedulingURL;
		private System.Windows.Forms.Label labelWebFormToLaunch;
		private UI.Button butEdit;
		private UI.Button butCopy;
		private System.Windows.Forms.Panel panel1;
		private UI.ODGrid gridOptions;
		private System.Windows.Forms.TextBox textSchedulingURL;
		private System.Windows.Forms.Label labelEnabled;
	}
}
