namespace OpenDental{
	partial class FormFHIRSetup {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFHIRSetup));
			this.butClose = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.labelPerm = new System.Windows.Forms.Label();
			this.treePermissions = new System.Windows.Forms.TreeView();
			this.imageListPerm = new System.Windows.Forms.ImageList(this.components);
			this.butGenerateKey = new OpenDental.UI.Button();
			this.butSetAll = new OpenDental.UI.Button();
			this.butUnsetAll = new OpenDental.UI.Button();
			this.checkEnabled = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textSubInterval = new OpenDental.ValidDouble();
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
			this.butClose.Location = new System.Drawing.Point(694, 504);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 74);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(493, 388);
			this.gridMain.TabIndex = 4;
			this.gridMain.Title = "API Keys";
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "tableAPIKeys";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// labelPerm
			// 
			this.labelPerm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelPerm.Location = new System.Drawing.Point(511, 51);
			this.labelPerm.Name = "labelPerm";
			this.labelPerm.Size = new System.Drawing.Size(236, 19);
			this.labelPerm.TabIndex = 6;
			this.labelPerm.Text = "Permissions for API key:";
			this.labelPerm.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// treePermissions
			// 
			this.treePermissions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treePermissions.HideSelection = false;
			this.treePermissions.ImageIndex = 0;
			this.treePermissions.ImageList = this.imageListPerm;
			this.treePermissions.ItemHeight = 15;
			this.treePermissions.Location = new System.Drawing.Point(511, 74);
			this.treePermissions.Name = "treePermissions";
			this.treePermissions.SelectedImageIndex = 0;
			this.treePermissions.ShowPlusMinus = false;
			this.treePermissions.ShowRootLines = false;
			this.treePermissions.Size = new System.Drawing.Size(233, 388);
			this.treePermissions.TabIndex = 7;
			this.treePermissions.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treePermissions_AfterSelect);
			this.treePermissions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treePermissions_MouseDown);
			// 
			// imageListPerm
			// 
			this.imageListPerm.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListPerm.ImageStream")));
			this.imageListPerm.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListPerm.Images.SetKeyName(0, "grayBox.gif");
			this.imageListPerm.Images.SetKeyName(1, "checkBoxUnchecked.gif");
			this.imageListPerm.Images.SetKeyName(2, "checkBoxChecked.gif");
			// 
			// butGenerateKey
			// 
			this.butGenerateKey.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGenerateKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butGenerateKey.Autosize = true;
			this.butGenerateKey.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGenerateKey.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGenerateKey.CornerRadius = 4F;
			this.butGenerateKey.Location = new System.Drawing.Point(12, 468);
			this.butGenerateKey.Name = "butGenerateKey";
			this.butGenerateKey.Size = new System.Drawing.Size(79, 24);
			this.butGenerateKey.TabIndex = 8;
			this.butGenerateKey.Text = "Generate Key";
			this.butGenerateKey.Click += new System.EventHandler(this.butGenerateKey_Click);
			// 
			// butSetAll
			// 
			this.butSetAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSetAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSetAll.Autosize = true;
			this.butSetAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSetAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSetAll.CornerRadius = 4F;
			this.butSetAll.Location = new System.Drawing.Point(511, 468);
			this.butSetAll.Name = "butSetAll";
			this.butSetAll.Size = new System.Drawing.Size(79, 24);
			this.butSetAll.TabIndex = 12;
			this.butSetAll.Text = "Set All";
			this.butSetAll.Click += new System.EventHandler(this.butSetAll_Click);
			// 
			// butUnsetAll
			// 
			this.butUnsetAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUnsetAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butUnsetAll.Autosize = true;
			this.butUnsetAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUnsetAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUnsetAll.CornerRadius = 4F;
			this.butUnsetAll.Location = new System.Drawing.Point(596, 468);
			this.butUnsetAll.Name = "butUnsetAll";
			this.butUnsetAll.Size = new System.Drawing.Size(79, 24);
			this.butUnsetAll.TabIndex = 13;
			this.butUnsetAll.Text = "Unset All";
			this.butUnsetAll.Click += new System.EventHandler(this.butUnsetAll_Click);
			// 
			// checkEnabled
			// 
			this.checkEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEnabled.Location = new System.Drawing.Point(279, 12);
			this.checkEnabled.Name = "checkEnabled";
			this.checkEnabled.Size = new System.Drawing.Size(104, 20);
			this.checkEnabled.TabIndex = 14;
			this.checkEnabled.Text = "Enabled";
			this.checkEnabled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEnabled.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Location = new System.Drawing.Point(33, 35);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(333, 36);
			this.label3.TabIndex = 58;
			this.label3.Text = "Process subscription interval in minutes. Leave blank to disable subscriptions.";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSubInterval
			// 
			this.textSubInterval.Location = new System.Drawing.Point(369, 38);
			this.textSubInterval.MaxVal = 100000000D;
			this.textSubInterval.MinVal = 0D;
			this.textSubInterval.Name = "textSubInterval";
			this.textSubInterval.Size = new System.Drawing.Size(70, 20);
			this.textSubInterval.TabIndex = 59;
			// 
			// FormFHIRSetup
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(781, 540);
			this.Controls.Add(this.textSubInterval);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.checkEnabled);
			this.Controls.Add(this.butUnsetAll);
			this.Controls.Add(this.butSetAll);
			this.Controls.Add(this.butGenerateKey);
			this.Controls.Add(this.treePermissions);
			this.Controls.Add(this.labelPerm);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormFHIRSetup";
			this.Text = "FHIR Setup";
			this.Load += new System.EventHandler(this.FormFHIRSetup_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private OpenDental.UI.Button butClose;
		private UI.ODGrid gridMain;
		private System.Windows.Forms.Label labelPerm;
		private System.Windows.Forms.TreeView treePermissions;
		private UI.Button butGenerateKey;
		private UI.Button butSetAll;
		private System.Windows.Forms.ImageList imageListPerm;
		private UI.Button butUnsetAll;
		private System.Windows.Forms.CheckBox checkEnabled;
		private System.Windows.Forms.Label label3;
		private ValidDouble textSubInterval;
	}
}