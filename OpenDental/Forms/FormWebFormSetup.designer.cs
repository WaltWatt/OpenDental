namespace OpenDental{
	partial class FormWebFormSetup {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWebFormSetup));
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.menuWebFormSetupRight = new System.Windows.Forms.ContextMenu();
			this.menuItemCopyURL = new System.Windows.Forms.MenuItem();
			this.menuItemNavigateURL = new System.Windows.Forms.MenuItem();
			this.groupConstructURL = new System.Windows.Forms.GroupBox();
			this.checkAutoFillNameAndBirthdate = new System.Windows.Forms.CheckBox();
			this.textURLs = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.butPickClinic = new OpenDental.UI.Button();
			this.labelClinic = new System.Windows.Forms.Label();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.butNextForms = new OpenDental.UI.Button();
			this.textNextForms = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textRedirectURL = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.butSave = new OpenDental.UI.Button();
			this.butChange = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butWebformBorderColor = new System.Windows.Forms.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.labelBorderColor = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.textboxWebHostAddress = new System.Windows.Forms.TextBox();
			this.labelWebhostURL = new System.Windows.Forms.Label();
			this.butCancel = new OpenDental.UI.Button();
			this.butUpdate = new OpenDental.UI.Button();
			this.butOk = new OpenDental.UI.Button();
			this.groupConstructURL.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuWebFormSetupRight
			// 
			this.menuWebFormSetupRight.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemCopyURL,
            this.menuItemNavigateURL});
			// 
			// menuItemCopyURL
			// 
			this.menuItemCopyURL.Index = 0;
			this.menuItemCopyURL.Text = "Copy URL to clipboard";
			this.menuItemCopyURL.Click += new System.EventHandler(this.menuItemCopyURL_Click);
			// 
			// menuItemNavigateURL
			// 
			this.menuItemNavigateURL.Index = 1;
			this.menuItemNavigateURL.Text = "Navigate to the URL on the web browser";
			this.menuItemNavigateURL.Click += new System.EventHandler(this.menuItemNavigateURL_Click);
			// 
			// groupConstructURL
			// 
			this.groupConstructURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupConstructURL.Controls.Add(this.checkAutoFillNameAndBirthdate);
			this.groupConstructURL.Controls.Add(this.textURLs);
			this.groupConstructURL.Controls.Add(this.label2);
			this.groupConstructURL.Controls.Add(this.butPickClinic);
			this.groupConstructURL.Controls.Add(this.labelClinic);
			this.groupConstructURL.Controls.Add(this.comboClinic);
			this.groupConstructURL.Controls.Add(this.butNextForms);
			this.groupConstructURL.Controls.Add(this.textNextForms);
			this.groupConstructURL.Controls.Add(this.label1);
			this.groupConstructURL.Controls.Add(this.textRedirectURL);
			this.groupConstructURL.Controls.Add(this.label3);
			this.groupConstructURL.Location = new System.Drawing.Point(12, 396);
			this.groupConstructURL.Name = "groupConstructURL";
			this.groupConstructURL.Size = new System.Drawing.Size(766, 204);
			this.groupConstructURL.TabIndex = 75;
			this.groupConstructURL.TabStop = false;
			this.groupConstructURL.Text = "Construct URL";
			// 
			// checkAutoFillNameAndBirthdate
			// 
			this.checkAutoFillNameAndBirthdate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAutoFillNameAndBirthdate.Location = new System.Drawing.Point(160, 99);
			this.checkAutoFillNameAndBirthdate.Name = "checkAutoFillNameAndBirthdate";
			this.checkAutoFillNameAndBirthdate.Size = new System.Drawing.Size(444, 18);
			this.checkAutoFillNameAndBirthdate.TabIndex = 43;
			this.checkAutoFillNameAndBirthdate.Text = "Inherit (Auto Fill) Name and Birthdate from Previous Form";
			this.checkAutoFillNameAndBirthdate.UseVisualStyleBackColor = true;
			this.checkAutoFillNameAndBirthdate.CheckedChanged += new System.EventHandler(this.checkAutoFillNameAndBirthdate_CheckedChanged);
			// 
			// textURLs
			// 
			this.textURLs.Location = new System.Drawing.Point(160, 122);
			this.textURLs.MaxLength = 100000;
			this.textURLs.Multiline = true;
			this.textURLs.Name = "textURLs";
			this.textURLs.ReadOnly = true;
			this.textURLs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textURLs.Size = new System.Drawing.Size(444, 70);
			this.textURLs.TabIndex = 42;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(22, 123);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(139, 17);
			this.label2.TabIndex = 41;
			this.label2.Text = "URLs";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPickClinic
			// 
			this.butPickClinic.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickClinic.Autosize = false;
			this.butPickClinic.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickClinic.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickClinic.CornerRadius = 2F;
			this.butPickClinic.Location = new System.Drawing.Point(329, 74);
			this.butPickClinic.Name = "butPickClinic";
			this.butPickClinic.Size = new System.Drawing.Size(23, 20);
			this.butPickClinic.TabIndex = 39;
			this.butPickClinic.Text = "...";
			this.butPickClinic.Click += new System.EventHandler(this.butPickClinic_Click);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(19, 74);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(139, 17);
			this.labelClinic.TabIndex = 40;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.FormattingEnabled = true;
			this.comboClinic.Location = new System.Drawing.Point(160, 73);
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(163, 21);
			this.comboClinic.TabIndex = 38;
			this.comboClinic.SelectedIndexChanged += new System.EventHandler(this.comboClinic_SelectedIndexChanged);
			// 
			// butNextForms
			// 
			this.butNextForms.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNextForms.Autosize = false;
			this.butNextForms.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNextForms.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNextForms.CornerRadius = 2F;
			this.butNextForms.Location = new System.Drawing.Point(610, 46);
			this.butNextForms.Name = "butNextForms";
			this.butNextForms.Size = new System.Drawing.Size(23, 20);
			this.butNextForms.TabIndex = 19;
			this.butNextForms.Text = "...";
			this.butNextForms.Click += new System.EventHandler(this.butNextForms_Click);
			// 
			// textNextForms
			// 
			this.textNextForms.Location = new System.Drawing.Point(160, 46);
			this.textNextForms.MaxLength = 100;
			this.textNextForms.Name = "textNextForms";
			this.textNextForms.ReadOnly = true;
			this.textNextForms.Size = new System.Drawing.Size(444, 20);
			this.textNextForms.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(20, 49);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(139, 14);
			this.label1.TabIndex = 5;
			this.label1.Text = "Next Forms";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textRedirectURL
			// 
			this.textRedirectURL.Location = new System.Drawing.Point(160, 19);
			this.textRedirectURL.MaxLength = 100;
			this.textRedirectURL.Name = "textRedirectURL";
			this.textRedirectURL.Size = new System.Drawing.Size(444, 20);
			this.textRedirectURL.TabIndex = 1;
			this.textRedirectURL.TextChanged += new System.EventHandler(this.textRedirectURL_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(20, 21);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(139, 14);
			this.label3.TabIndex = 2;
			this.label3.Text = "Redirect URL";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butSave
			// 
			this.butSave.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSave.Autosize = true;
			this.butSave.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSave.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSave.CornerRadius = 4F;
			this.butSave.Location = new System.Drawing.Point(622, 10);
			this.butSave.Name = "butSave";
			this.butSave.Size = new System.Drawing.Size(64, 24);
			this.butSave.TabIndex = 74;
			this.butSave.Text = "Save";
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			// 
			// butChange
			// 
			this.butChange.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChange.Autosize = true;
			this.butChange.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChange.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChange.CornerRadius = 4F;
			this.butChange.Location = new System.Drawing.Point(202, 37);
			this.butChange.Name = "butChange";
			this.butChange.Size = new System.Drawing.Size(68, 24);
			this.butChange.TabIndex = 72;
			this.butChange.Text = "Change";
			this.butChange.Click += new System.EventHandler(this.butChange_Click);
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(786, 247);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75, 24);
			this.butDelete.TabIndex = 58;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butWebformBorderColor
			// 
			this.butWebformBorderColor.BackColor = System.Drawing.Color.RoyalBlue;
			this.butWebformBorderColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butWebformBorderColor.Location = new System.Drawing.Point(172, 37);
			this.butWebformBorderColor.Name = "butWebformBorderColor";
			this.butWebformBorderColor.Size = new System.Drawing.Size(24, 24);
			this.butWebformBorderColor.TabIndex = 71;
			this.butWebformBorderColor.UseVisualStyleBackColor = false;
			this.butWebformBorderColor.Click += new System.EventHandler(this.butWebformBorderColor_Click);
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
			this.butAdd.Location = new System.Drawing.Point(786, 162);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 24);
			this.butAdd.TabIndex = 57;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// labelBorderColor
			// 
			this.labelBorderColor.Location = new System.Drawing.Point(53, 37);
			this.labelBorderColor.Name = "labelBorderColor";
			this.labelBorderColor.Size = new System.Drawing.Size(111, 19);
			this.labelBorderColor.TabIndex = 48;
			this.labelBorderColor.Text = "Border Color";
			this.labelBorderColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasLinkDetect = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(18, 67);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(760, 325);
			this.gridMain.TabIndex = 56;
			this.gridMain.Title = "Sheet Defs";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableSheetDefs";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridMain_MouseUp);
			// 
			// textboxWebHostAddress
			// 
			this.textboxWebHostAddress.Location = new System.Drawing.Point(171, 12);
			this.textboxWebHostAddress.Name = "textboxWebHostAddress";
			this.textboxWebHostAddress.Size = new System.Drawing.Size(445, 20);
			this.textboxWebHostAddress.TabIndex = 45;
			this.textboxWebHostAddress.TextChanged += new System.EventHandler(this.textboxWebHostAddress_TextChanged);
			// 
			// labelWebhostURL
			// 
			this.labelWebhostURL.Location = new System.Drawing.Point(0, 13);
			this.labelWebhostURL.Name = "labelWebhostURL";
			this.labelWebhostURL.Size = new System.Drawing.Size(169, 19);
			this.labelWebhostURL.TabIndex = 46;
			this.labelWebhostURL.Text = "Host Server Address";
			this.labelWebhostURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(795, 576);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Close";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butUpdate
			// 
			this.butUpdate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butUpdate.Autosize = true;
			this.butUpdate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUpdate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUpdate.CornerRadius = 4F;
			this.butUpdate.Location = new System.Drawing.Point(786, 205);
			this.butUpdate.Name = "butUpdate";
			this.butUpdate.Size = new System.Drawing.Size(75, 24);
			this.butUpdate.TabIndex = 76;
			this.butUpdate.Text = "Update";
			this.butUpdate.Click += new System.EventHandler(this.butUpdate_Click);
			// 
			// butOk
			// 
			this.butOk.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOk.Autosize = true;
			this.butOk.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOk.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOk.CornerRadius = 4F;
			this.butOk.Location = new System.Drawing.Point(795, 546);
			this.butOk.Name = "butOk";
			this.butOk.Size = new System.Drawing.Size(75, 24);
			this.butOk.TabIndex = 77;
			this.butOk.Text = "&Ok";
			this.butOk.Click += new System.EventHandler(this.butOk_Click);
			// 
			// FormWebFormSetup
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(884, 616);
			this.Controls.Add(this.butOk);
			this.Controls.Add(this.butUpdate);
			this.Controls.Add(this.groupConstructURL);
			this.Controls.Add(this.butSave);
			this.Controls.Add(this.butChange);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butWebformBorderColor);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.labelBorderColor);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.textboxWebHostAddress);
			this.Controls.Add(this.labelWebhostURL);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(773, 402);
			this.Name = "FormWebFormSetup";
			this.Text = "Web Form Setup";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormWebFormSetup_FormClosing);
			this.Load += new System.EventHandler(this.FormWebFormSetup_Load);
			this.Shown += new System.EventHandler(this.FormWebFormSetup_Shown);
			this.groupConstructURL.ResumeLayout(false);
			this.groupConstructURL.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.TextBox textboxWebHostAddress;
		private System.Windows.Forms.Label labelWebhostURL;
		private System.Windows.Forms.Label labelBorderColor;
		private System.Windows.Forms.Button butWebformBorderColor;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private OpenDental.UI.Button butChange;
		private OpenDental.UI.ODGrid gridMain;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.Button butAdd;
		private UI.Button butSave;
		private System.Windows.Forms.ContextMenu menuWebFormSetupRight;
		private System.Windows.Forms.MenuItem menuItemCopyURL;
		private System.Windows.Forms.MenuItem menuItemNavigateURL;
		private System.Windows.Forms.GroupBox groupConstructURL;
		private System.Windows.Forms.TextBox textRedirectURL;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textNextForms;
		private System.Windows.Forms.Label label1;
		private UI.Button butNextForms;
		private UI.Button butPickClinic;
		private System.Windows.Forms.Label labelClinic;
		private System.Windows.Forms.ComboBox comboClinic;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textURLs;
		private UI.Button butUpdate;
		private System.Windows.Forms.CheckBox checkAutoFillNameAndBirthdate;
		private UI.Button butOk;
	}
}