namespace OpenDental{
	partial class FormJobPermissions {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJobPermissions));
			this.butOK = new OpenDental.UI.Button();
			this.labelAvailable = new System.Windows.Forms.Label();
			this.listAvailable = new System.Windows.Forms.ListBox();
			this.butCancel = new OpenDental.UI.Button();
			this.butExpert = new OpenDental.UI.Button();
			this.butPreExpert = new OpenDental.UI.Button();
			this.butEngineer = new OpenDental.UI.Button();
			this.butCustomerManager = new OpenDental.UI.Button();
			this.butTechWriter = new OpenDental.UI.Button();
			this.butQueryManager = new OpenDental.UI.Button();
			this.butJobManager = new OpenDental.UI.Button();
			this.butFeatureManager = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.butQuoteManager = new OpenDental.UI.Button();
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
			this.butOK.Location = new System.Drawing.Point(185, 313);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 63;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// labelAvailable
			// 
			this.labelAvailable.Location = new System.Drawing.Point(18, 13);
			this.labelAvailable.Name = "labelAvailable";
			this.labelAvailable.Size = new System.Drawing.Size(161, 17);
			this.labelAvailable.TabIndex = 60;
			this.labelAvailable.Text = "(Highlight Multiple)";
			this.labelAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// listAvailable
			// 
			this.listAvailable.FormattingEnabled = true;
			this.listAvailable.IntegralHeight = false;
			this.listAvailable.Location = new System.Drawing.Point(21, 33);
			this.listAvailable.Name = "listAvailable";
			this.listAvailable.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listAvailable.Size = new System.Drawing.Size(158, 334);
			this.listAvailable.TabIndex = 59;
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(185, 343);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 57;
			this.butCancel.Text = "Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butExpert
			// 
			this.butExpert.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butExpert.Autosize = true;
			this.butExpert.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butExpert.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butExpert.CornerRadius = 4F;
			this.butExpert.Location = new System.Drawing.Point(185, 93);
			this.butExpert.Name = "butExpert";
			this.butExpert.Size = new System.Drawing.Size(75, 24);
			this.butExpert.TabIndex = 64;
			this.butExpert.Text = "Expert";
			this.butExpert.Click += new System.EventHandler(this.butExpert_Click);
			// 
			// butPreExpert
			// 
			this.butPreExpert.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPreExpert.Autosize = true;
			this.butPreExpert.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPreExpert.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPreExpert.CornerRadius = 4F;
			this.butPreExpert.Location = new System.Drawing.Point(185, 63);
			this.butPreExpert.Name = "butPreExpert";
			this.butPreExpert.Size = new System.Drawing.Size(75, 24);
			this.butPreExpert.TabIndex = 65;
			this.butPreExpert.Text = "Pre-Expert";
			this.butPreExpert.Click += new System.EventHandler(this.butPreExpert_Click);
			// 
			// butEngineer
			// 
			this.butEngineer.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEngineer.Autosize = true;
			this.butEngineer.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEngineer.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEngineer.CornerRadius = 4F;
			this.butEngineer.Location = new System.Drawing.Point(185, 33);
			this.butEngineer.Name = "butEngineer";
			this.butEngineer.Size = new System.Drawing.Size(75, 24);
			this.butEngineer.TabIndex = 66;
			this.butEngineer.Text = "Engineer";
			this.butEngineer.Click += new System.EventHandler(this.butEngineer_Click);
			// 
			// butCustomerManager
			// 
			this.butCustomerManager.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCustomerManager.Autosize = true;
			this.butCustomerManager.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCustomerManager.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCustomerManager.CornerRadius = 4F;
			this.butCustomerManager.Location = new System.Drawing.Point(185, 181);
			this.butCustomerManager.Name = "butCustomerManager";
			this.butCustomerManager.Size = new System.Drawing.Size(75, 24);
			this.butCustomerManager.TabIndex = 70;
			this.butCustomerManager.Text = "Cust. Mang.";
			this.butCustomerManager.Click += new System.EventHandler(this.butCustomerManager_Click);
			// 
			// butTechWriter
			// 
			this.butTechWriter.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butTechWriter.Autosize = true;
			this.butTechWriter.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTechWriter.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTechWriter.CornerRadius = 4F;
			this.butTechWriter.Location = new System.Drawing.Point(185, 123);
			this.butTechWriter.Name = "butTechWriter";
			this.butTechWriter.Size = new System.Drawing.Size(75, 24);
			this.butTechWriter.TabIndex = 71;
			this.butTechWriter.Text = "Tech. Writer";
			this.butTechWriter.Click += new System.EventHandler(this.butTechWriter_Click);
			// 
			// butQueryManager
			// 
			this.butQueryManager.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butQueryManager.Autosize = true;
			this.butQueryManager.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butQueryManager.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butQueryManager.CornerRadius = 4F;
			this.butQueryManager.Location = new System.Drawing.Point(185, 241);
			this.butQueryManager.Name = "butQueryManager";
			this.butQueryManager.Size = new System.Drawing.Size(75, 24);
			this.butQueryManager.TabIndex = 67;
			this.butQueryManager.Text = "Query Mang.";
			this.butQueryManager.Click += new System.EventHandler(this.butQueryManager_Click);
			// 
			// butJobManager
			// 
			this.butJobManager.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butJobManager.Autosize = true;
			this.butJobManager.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butJobManager.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butJobManager.CornerRadius = 4F;
			this.butJobManager.Location = new System.Drawing.Point(185, 153);
			this.butJobManager.Name = "butJobManager";
			this.butJobManager.Size = new System.Drawing.Size(75, 24);
			this.butJobManager.TabIndex = 69;
			this.butJobManager.Text = "Job Manager";
			this.butJobManager.Click += new System.EventHandler(this.butJobManager_Click);
			// 
			// butFeatureManager
			// 
			this.butFeatureManager.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butFeatureManager.Autosize = true;
			this.butFeatureManager.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butFeatureManager.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butFeatureManager.CornerRadius = 4F;
			this.butFeatureManager.Location = new System.Drawing.Point(185, 211);
			this.butFeatureManager.Name = "butFeatureManager";
			this.butFeatureManager.Size = new System.Drawing.Size(75, 24);
			this.butFeatureManager.TabIndex = 68;
			this.butFeatureManager.Text = "Feat. Mang.";
			this.butFeatureManager.Click += new System.EventHandler(this.butFeatureManager_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(130, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(130, 17);
			this.label1.TabIndex = 72;
			this.label1.Text = "Pre-Defined Roles";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butQuoteManager
			// 
			this.butQuoteManager.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butQuoteManager.Autosize = true;
			this.butQuoteManager.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butQuoteManager.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butQuoteManager.CornerRadius = 4F;
			this.butQuoteManager.Location = new System.Drawing.Point(185, 271);
			this.butQuoteManager.Name = "butQuoteManager";
			this.butQuoteManager.Size = new System.Drawing.Size(75, 24);
			this.butQuoteManager.TabIndex = 73;
			this.butQuoteManager.Text = "Quote Mang.";
			this.butQuoteManager.Click += new System.EventHandler(this.butQuoteManager_Click);
			// 
			// FormJobPermissions
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(278, 379);
			this.Controls.Add(this.butQuoteManager);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butTechWriter);
			this.Controls.Add(this.butCustomerManager);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butQueryManager);
			this.Controls.Add(this.labelAvailable);
			this.Controls.Add(this.butJobManager);
			this.Controls.Add(this.butFeatureManager);
			this.Controls.Add(this.listAvailable);
			this.Controls.Add(this.butExpert);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butEngineer);
			this.Controls.Add(this.butPreExpert);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(294, 284);
			this.Name = "FormJobPermissions";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Job Permissions";
			this.Load += new System.EventHandler(this.FormJobRoles_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private UI.Button butOK;
		private System.Windows.Forms.Label labelAvailable;
		private System.Windows.Forms.ListBox listAvailable;
		private UI.Button butCancel;
		private UI.Button butExpert;
		private UI.Button butPreExpert;
		private UI.Button butEngineer;
		private UI.Button butCustomerManager;
		private UI.Button butTechWriter;
		private UI.Button butQueryManager;
		private UI.Button butJobManager;
		private UI.Button butFeatureManager;
		private System.Windows.Forms.Label label1;
		private UI.Button butQuoteManager;

	}
}