namespace OpenDental{
	partial class FormEmailAddresses {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailAddresses));
			this.label2 = new System.Windows.Forms.Label();
			this.groupEmailPrefs = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.butSetDefault = new OpenDental.UI.Button();
			this.butWebMailNotify = new OpenDental.UI.Button();
			this.textInboxCheckInterval = new System.Windows.Forms.TextBox();
			this.textInboxComputerName = new System.Windows.Forms.TextBox();
			this.butThisComputer = new OpenDental.UI.Button();
			this.labelInboxCheckUnits = new System.Windows.Forms.Label();
			this.labelInboxCheckInterval = new System.Windows.Forms.Label();
			this.labelInboxComputerName = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butAdd = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupEmailPrefs.SuspendLayout();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 5);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(393, 33);
			this.label2.TabIndex = 13;
			this.label2.Text = "Setup clinic, practice, and group email addresses here.\r\nIndividual user inboxes " +
    "can be setup in File | Change Password/Email.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupEmailPrefs
			// 
			this.groupEmailPrefs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupEmailPrefs.Controls.Add(this.label1);
			this.groupEmailPrefs.Controls.Add(this.butSetDefault);
			this.groupEmailPrefs.Controls.Add(this.butWebMailNotify);
			this.groupEmailPrefs.Location = new System.Drawing.Point(659, 35);
			this.groupEmailPrefs.Name = "groupEmailPrefs";
			this.groupEmailPrefs.Size = new System.Drawing.Size(102, 163);
			this.groupEmailPrefs.TabIndex = 3;
			this.groupEmailPrefs.TabStop = false;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(90, 91);
			this.label1.TabIndex = 12;
			this.label1.Text = "Highlight an email address in the grid, then use one of these buttons.  Not requi" +
    "red.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butSetDefault
			// 
			this.butSetDefault.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSetDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butSetDefault.Autosize = true;
			this.butSetDefault.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSetDefault.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSetDefault.CornerRadius = 4F;
			this.butSetDefault.Location = new System.Drawing.Point(5, 106);
			this.butSetDefault.Name = "butSetDefault";
			this.butSetDefault.Size = new System.Drawing.Size(90, 24);
			this.butSetDefault.TabIndex = 3;
			this.butSetDefault.Text = "Set Default";
			this.butSetDefault.Click += new System.EventHandler(this.butSetDefault_Click);
			// 
			// butWebMailNotify
			// 
			this.butWebMailNotify.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butWebMailNotify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butWebMailNotify.Autosize = true;
			this.butWebMailNotify.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butWebMailNotify.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butWebMailNotify.CornerRadius = 4F;
			this.butWebMailNotify.Location = new System.Drawing.Point(5, 133);
			this.butWebMailNotify.Name = "butWebMailNotify";
			this.butWebMailNotify.Size = new System.Drawing.Size(90, 24);
			this.butWebMailNotify.TabIndex = 3;
			this.butWebMailNotify.Text = "WebMail Notify";
			this.butWebMailNotify.UseVisualStyleBackColor = true;
			this.butWebMailNotify.Click += new System.EventHandler(this.butWebMailNotify_Click);
			// 
			// textInboxCheckInterval
			// 
			this.textInboxCheckInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textInboxCheckInterval.Location = new System.Drawing.Point(18, 550);
			this.textInboxCheckInterval.MaxLength = 2147483647;
			this.textInboxCheckInterval.Multiline = true;
			this.textInboxCheckInterval.Name = "textInboxCheckInterval";
			this.textInboxCheckInterval.Size = new System.Drawing.Size(30, 24);
			this.textInboxCheckInterval.TabIndex = 8;
			// 
			// textInboxComputerName
			// 
			this.textInboxComputerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textInboxComputerName.Location = new System.Drawing.Point(18, 506);
			this.textInboxComputerName.MaxLength = 2147483647;
			this.textInboxComputerName.Multiline = true;
			this.textInboxComputerName.Name = "textInboxComputerName";
			this.textInboxComputerName.Size = new System.Drawing.Size(318, 24);
			this.textInboxComputerName.TabIndex = 5;
			// 
			// butThisComputer
			// 
			this.butThisComputer.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butThisComputer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butThisComputer.Autosize = true;
			this.butThisComputer.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butThisComputer.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butThisComputer.CornerRadius = 4F;
			this.butThisComputer.Location = new System.Drawing.Point(342, 506);
			this.butThisComputer.Name = "butThisComputer";
			this.butThisComputer.Size = new System.Drawing.Size(87, 24);
			this.butThisComputer.TabIndex = 10;
			this.butThisComputer.Text = "This Computer";
			this.butThisComputer.Click += new System.EventHandler(this.butThisComputer_Click);
			// 
			// labelInboxCheckUnits
			// 
			this.labelInboxCheckUnits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelInboxCheckUnits.Location = new System.Drawing.Point(49, 553);
			this.labelInboxCheckUnits.Name = "labelInboxCheckUnits";
			this.labelInboxCheckUnits.Size = new System.Drawing.Size(198, 18);
			this.labelInboxCheckUnits.TabIndex = 9;
			this.labelInboxCheckUnits.Text = "minutes (1 to 60)";
			this.labelInboxCheckUnits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelInboxCheckInterval
			// 
			this.labelInboxCheckInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelInboxCheckInterval.Location = new System.Drawing.Point(16, 531);
			this.labelInboxCheckInterval.Name = "labelInboxCheckInterval";
			this.labelInboxCheckInterval.Size = new System.Drawing.Size(335, 18);
			this.labelInboxCheckInterval.TabIndex = 7;
			this.labelInboxCheckInterval.Text = "Inbox Receive Interval";
			this.labelInboxCheckInterval.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelInboxComputerName
			// 
			this.labelInboxComputerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelInboxComputerName.Location = new System.Drawing.Point(16, 487);
			this.labelInboxComputerName.Name = "labelInboxComputerName";
			this.labelInboxComputerName.Size = new System.Drawing.Size(335, 18);
			this.labelInboxComputerName.TabIndex = 6;
			this.labelInboxComputerName.Text = "Computer Name To Receive New Email From";
			this.labelInboxComputerName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
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
			this.gridMain.Location = new System.Drawing.Point(19, 41);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(631, 440);
			this.gridMain.TabIndex = 4;
			this.gridMain.Title = "Email Addresses";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableEmailAddresses";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(659, 248);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 24);
			this.butAdd.TabIndex = 3;
			this.butAdd.Text = "Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(680, 520);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 2;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(680, 550);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormEmailAddresses
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(775, 587);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.groupEmailPrefs);
			this.Controls.Add(this.textInboxCheckInterval);
			this.Controls.Add(this.textInboxComputerName);
			this.Controls.Add(this.butThisComputer);
			this.Controls.Add(this.labelInboxCheckUnits);
			this.Controls.Add(this.labelInboxCheckInterval);
			this.Controls.Add(this.labelInboxComputerName);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(545, 400);
			this.Name = "FormEmailAddresses";
			this.Text = "Email Addresses";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEmailAddresses_FormClosing);
			this.Load += new System.EventHandler(this.FormEmailAddresses_Load);
			this.groupEmailPrefs.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butSetDefault;
		private OpenDental.UI.Button butCancel;
		private UI.ODGrid gridMain;
		private UI.Button butAdd;
		private UI.Button butOK;
		private System.Windows.Forms.Label labelInboxComputerName;
		private System.Windows.Forms.Label labelInboxCheckInterval;
		private System.Windows.Forms.Label labelInboxCheckUnits;
		private UI.Button butThisComputer;
		private System.Windows.Forms.TextBox textInboxComputerName;
		private System.Windows.Forms.TextBox textInboxCheckInterval;
		private UI.Button butWebMailNotify;
		private System.Windows.Forms.GroupBox groupEmailPrefs;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
	}
}