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
			this.labelDetails = new System.Windows.Forms.Label();
			this.labelFriendlyMessage = new System.Windows.Forms.Label();
			this.textDetails = new System.Windows.Forms.TextBox();
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
			// textDetails
			// 
			this.textDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDetails.BackColor = System.Drawing.SystemColors.Control;
			this.textDetails.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.textDetails.Location = new System.Drawing.Point(13, 64);
			this.textDetails.MaximumSize = new System.Drawing.Size(1200, 800);
			this.textDetails.Multiline = true;
			this.textDetails.Name = "textDetails";
			this.textDetails.ReadOnly = true;
			this.textDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textDetails.Size = new System.Drawing.Size(450, 195);
			this.textDetails.TabIndex = 87;
			this.textDetails.TabStop = false;
			this.textDetails.Text = "Error Details";
			// 
			// FormFriendlyException
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(481, 298);
			this.Controls.Add(this.textDetails);
			this.Controls.Add(this.labelFriendlyMessage);
			this.Controls.Add(this.labelDetails);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormFriendlyException";
			this.Text = "Error Encountered";
			this.Load += new System.EventHandler(this.FormFriendlyException_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.Label labelDetails;
		private System.Windows.Forms.Label labelFriendlyMessage;
		private System.Windows.Forms.TextBox textDetails;
	}
}