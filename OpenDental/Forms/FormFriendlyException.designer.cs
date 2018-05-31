namespace OpenDental{
	partial class FormFriendlyException {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFriendlyException));
			this.butClose = new OpenDental.UI.Button();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabStack = new System.Windows.Forms.TabPage();
			this.textDetails = new System.Windows.Forms.TextBox();
			this.tabQuery = new System.Windows.Forms.TabPage();
			this.textQuery = new System.Windows.Forms.TextBox();
			this.butPrint = new OpenDental.UI.Button();
			this.labelFriendlyMessage = new System.Windows.Forms.Label();
			this.labelDetails = new System.Windows.Forms.Label();
			this.butCopyAll = new OpenDental.UI.Button();
			this.tabControl.SuspendLayout();
			this.tabStack.SuspendLayout();
			this.tabQuery.SuspendLayout();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(394, 264);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabStack);
			this.tabControl.Controls.Add(this.tabQuery);
			this.tabControl.Location = new System.Drawing.Point(12, 68);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(457, 191);
			this.tabControl.TabIndex = 88;
			// 
			// tabStack
			// 
			this.tabStack.BackColor = System.Drawing.SystemColors.Control;
			this.tabStack.Controls.Add(this.textDetails);
			this.tabStack.Location = new System.Drawing.Point(4, 22);
			this.tabStack.Name = "tabStack";
			this.tabStack.Padding = new System.Windows.Forms.Padding(3);
			this.tabStack.Size = new System.Drawing.Size(449, 165);
			this.tabStack.TabIndex = 0;
			this.tabStack.Text = "StackTrace";
			// 
			// textDetails
			// 
			this.textDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDetails.BackColor = System.Drawing.SystemColors.Control;
			this.textDetails.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.textDetails.Location = new System.Drawing.Point(0, 0);
			this.textDetails.MaximumSize = new System.Drawing.Size(1200, 800);
			this.textDetails.Multiline = true;
			this.textDetails.Name = "textDetails";
			this.textDetails.ReadOnly = true;
			this.textDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textDetails.Size = new System.Drawing.Size(459, 165);
			this.textDetails.TabIndex = 87;
			this.textDetails.TabStop = false;
			this.textDetails.Text = "Error Details";
			// 
			// tabQuery
			// 
			this.tabQuery.BackColor = System.Drawing.SystemColors.Control;
			this.tabQuery.Controls.Add(this.textQuery);
			this.tabQuery.Location = new System.Drawing.Point(4, 22);
			this.tabQuery.Name = "tabQuery";
			this.tabQuery.Padding = new System.Windows.Forms.Padding(3);
			this.tabQuery.Size = new System.Drawing.Size(449, 165);
			this.tabQuery.TabIndex = 1;
			this.tabQuery.Text = "Query";
			// 
			// textQuery
			// 
			this.textQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textQuery.BackColor = System.Drawing.SystemColors.Control;
			this.textQuery.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.textQuery.Location = new System.Drawing.Point(0, 0);
			this.textQuery.MaximumSize = new System.Drawing.Size(1200, 800);
			this.textQuery.Multiline = true;
			this.textQuery.Name = "textQuery";
			this.textQuery.ReadOnly = true;
			this.textQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textQuery.Size = new System.Drawing.Size(449, 165);
			this.textQuery.TabIndex = 88;
			this.textQuery.TabStop = false;
			this.textQuery.Text = "Query Detail";
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(179, 263);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(79, 26);
			this.butPrint.TabIndex = 91;
			this.butPrint.Text = "&Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			this.butPrint.Image = ((System.Drawing.Image)(resources.GetObject("butPrint.Image")));
			// 
			// labelFriendlyMessage
			// 
			this.labelFriendlyMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelFriendlyMessage.Location = new System.Drawing.Point(12, 9);
			this.labelFriendlyMessage.Name = "labelFriendlyMessage";
			this.labelFriendlyMessage.Size = new System.Drawing.Size(457, 56);
			this.labelFriendlyMessage.TabIndex = 86;
			this.labelFriendlyMessage.Text = "Friendly Error Message";
			this.labelFriendlyMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelDetails
			// 
			this.labelDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelDetails.ForeColor = System.Drawing.Color.Blue;
			this.labelDetails.Location = new System.Drawing.Point(13, 262);
			this.labelDetails.Name = "labelDetails";
			this.labelDetails.Size = new System.Drawing.Size(98, 19);
			this.labelDetails.TabIndex = 85;
			this.labelDetails.Text = "Details";
			this.labelDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelDetails.Click += new System.EventHandler(this.labelDetails_Click);
			// 
			// butCopyAll
			// 
			this.butCopyAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCopyAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butCopyAll.Autosize = true;
			this.butCopyAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCopyAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCopyAll.CornerRadius = 4F;
			this.butCopyAll.Location = new System.Drawing.Point(98, 264);
			this.butCopyAll.Name = "butCopyAll";
			this.butCopyAll.Size = new System.Drawing.Size(75, 24);
			this.butCopyAll.TabIndex = 92;
			this.butCopyAll.Text = "Copy All";
			this.butCopyAll.Click += new System.EventHandler(this.butCopyAll_Click);
			// 
			// FormFriendlyException
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(481, 298);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.labelFriendlyMessage);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.butCopyAll);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.labelDetails);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormFriendlyException";
			this.Text = "Error Encountered";
			this.Load += new System.EventHandler(this.FormFriendlyException_Load);
			this.tabControl.ResumeLayout(false);
			this.tabStack.ResumeLayout(false);
			this.tabStack.PerformLayout();
			this.tabQuery.ResumeLayout(false);
			this.tabQuery.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.Label labelDetails;
		private System.Windows.Forms.Label labelFriendlyMessage;
		private System.Windows.Forms.TextBox textDetails;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabStack;
		private System.Windows.Forms.TabPage tabQuery;
		private System.Windows.Forms.TextBox textQuery;
		private UI.Button butPrint;
		private UI.Button butCopyAll;
	}
}