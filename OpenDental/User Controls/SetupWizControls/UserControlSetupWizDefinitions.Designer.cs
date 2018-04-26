namespace OpenDental.User_Controls.SetupWizard {
	partial class UserControlSetupWizDefinitions {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlSetupWizDefinitions));
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.gridDefs = new OpenDental.UI.ODGrid();
			this.label14 = new System.Windows.Forms.Label();
			this.textGuide = new System.Windows.Forms.TextBox();
			this.groupEdit = new System.Windows.Forms.GroupBox();
			this.butHide = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.listCategory = new System.Windows.Forms.ListBox();
			this.label13 = new System.Windows.Forms.Label();
			this.groupEdit.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "iButton_Blue.png");
			// 
			// gridDefs
			// 
			this.gridDefs.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridDefs.HasAddButton = false;
			this.gridDefs.HasDropDowns = false;
			this.gridDefs.HasMultilineHeaders = false;
			this.gridDefs.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridDefs.HeaderHeight = 15;
			this.gridDefs.HScrollVisible = false;
			this.gridDefs.Location = new System.Drawing.Point(223, 23);
			this.gridDefs.Name = "gridDefs";
			this.gridDefs.ScrollValue = 0;
			this.gridDefs.Size = new System.Drawing.Size(488, 282);
			this.gridDefs.TabIndex = 29;
			this.gridDefs.Title = "Definitions";
			this.gridDefs.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridDefs.TitleHeight = 18;
			this.gridDefs.TranslationName = "SetupDefinitions";
			this.gridDefs.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridDefs_CellDoubleClick);
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(117, 408);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(100, 18);
			this.label14.TabIndex = 28;
			this.label14.Text = "Guidelines";
			this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textGuide
			// 
			this.textGuide.Location = new System.Drawing.Point(223, 408);
			this.textGuide.Multiline = true;
			this.textGuide.Name = "textGuide";
			this.textGuide.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textGuide.Size = new System.Drawing.Size(488, 64);
			this.textGuide.TabIndex = 26;
			// 
			// groupEdit
			// 
			this.groupEdit.Controls.Add(this.butHide);
			this.groupEdit.Controls.Add(this.butDown);
			this.groupEdit.Controls.Add(this.butUp);
			this.groupEdit.Controls.Add(this.butAdd);
			this.groupEdit.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupEdit.Location = new System.Drawing.Point(223, 329);
			this.groupEdit.Name = "groupEdit";
			this.groupEdit.Size = new System.Drawing.Size(488, 64);
			this.groupEdit.TabIndex = 25;
			this.groupEdit.TabStop = false;
			this.groupEdit.Text = "Edit Items";
			// 
			// butHide
			// 
			this.butHide.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butHide.Autosize = true;
			this.butHide.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butHide.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butHide.CornerRadius = 4F;
			this.butHide.Location = new System.Drawing.Point(152, 19);
			this.butHide.Name = "butHide";
			this.butHide.Size = new System.Drawing.Size(79, 24);
			this.butHide.TabIndex = 10;
			this.butHide.Text = "&Hide";
			this.butHide.Click += new System.EventHandler(this.butHide_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(360, 19);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(79, 24);
			this.butDown.TabIndex = 9;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(256, 19);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(79, 24);
			this.butUp.TabIndex = 8;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(48, 19);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(79, 24);
			this.butAdd.TabIndex = 6;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// listCategory
			// 
			this.listCategory.Location = new System.Drawing.Point(32, 41);
			this.listCategory.Name = "listCategory";
			this.listCategory.Size = new System.Drawing.Size(167, 264);
			this.listCategory.TabIndex = 24;
			this.listCategory.SelectedIndexChanged += new System.EventHandler(this.listCategory_SelectedIndexChanged);
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(29, 21);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(162, 17);
			this.label13.TabIndex = 30;
			this.label13.Text = "Select Category:";
			this.label13.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// UserControlSetupWizDefinitions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label13);
			this.Controls.Add(this.gridDefs);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.textGuide);
			this.Controls.Add(this.groupEdit);
			this.Controls.Add(this.listCategory);
			this.Name = "UserControlSetupWizDefinitions";
			this.Size = new System.Drawing.Size(834, 530);
			this.Load += new System.EventHandler(this.UserControlSetupWizDefinitions_Load);
			this.groupEdit.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ImageList imageList1;
		private UI.ODGrid gridDefs;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox textGuide;
		private System.Windows.Forms.GroupBox groupEdit;
		private UI.Button butHide;
		private UI.Button butDown;
		private UI.Button butUp;
		private UI.Button butAdd;
		private System.Windows.Forms.ListBox listCategory;
		private System.Windows.Forms.Label label13;
	}
}
