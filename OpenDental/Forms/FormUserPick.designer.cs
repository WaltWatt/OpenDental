namespace OpenDental{
	partial class FormUserPick {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUserPick));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.listUser = new System.Windows.Forms.ListBox();
			this.butShow = new OpenDental.UI.Button();
			this.butNone = new OpenDental.UI.Button();
			this.butAll = new OpenDental.UI.Button();
			this.groupSelect = new System.Windows.Forms.GroupBox();
			this.groupSelect.SuspendLayout();
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
			this.butOK.Location = new System.Drawing.Point(215, 451);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
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
			this.butCancel.Location = new System.Drawing.Point(215, 492);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// listUser
			// 
			this.listUser.FormattingEnabled = true;
			this.listUser.Location = new System.Drawing.Point(12, 18);
			this.listUser.Name = "listUser";
			this.listUser.Size = new System.Drawing.Size(176, 498);
			this.listUser.TabIndex = 4;
			this.listUser.DoubleClick += new System.EventHandler(this.listUser_DoubleClick);
			// 
			// butShow
			// 
			this.butShow.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butShow.Autosize = true;
			this.butShow.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShow.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShow.CornerRadius = 4F;
			this.butShow.Location = new System.Drawing.Point(215, 18);
			this.butShow.Name = "butShow";
			this.butShow.Size = new System.Drawing.Size(75, 24);
			this.butShow.TabIndex = 5;
			this.butShow.Text = "Show All";
			this.butShow.Visible = false;
			this.butShow.Click += new System.EventHandler(this.butShow_Click);
			// 
			// butNone
			// 
			this.butNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butNone.Autosize = true;
			this.butNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNone.CornerRadius = 4F;
			this.butNone.Location = new System.Drawing.Point(21, 49);
			this.butNone.Name = "butNone";
			this.butNone.Size = new System.Drawing.Size(75, 24);
			this.butNone.TabIndex = 6;
			this.butNone.Text = "Select None";
			this.butNone.Visible = false;
			this.butNone.Click += new System.EventHandler(this.butNone_Click);
			// 
			// butAll
			// 
			this.butAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAll.Autosize = true;
			this.butAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAll.CornerRadius = 4F;
			this.butAll.Location = new System.Drawing.Point(21, 19);
			this.butAll.Name = "butAll";
			this.butAll.Size = new System.Drawing.Size(75, 24);
			this.butAll.TabIndex = 7;
			this.butAll.Text = "Select All";
			this.butAll.Visible = false;
			this.butAll.Click += new System.EventHandler(this.butAll_Click);
			// 
			// groupSelect
			// 
			this.groupSelect.Controls.Add(this.butAll);
			this.groupSelect.Controls.Add(this.butNone);
			this.groupSelect.Location = new System.Drawing.Point(194, 48);
			this.groupSelect.Name = "groupSelect";
			this.groupSelect.Size = new System.Drawing.Size(110, 83);
			this.groupSelect.TabIndex = 8;
			this.groupSelect.TabStop = false;
			this.groupSelect.Text = "Select";
			// 
			// FormUserPick
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(316, 534);
			this.Controls.Add(this.groupSelect);
			this.Controls.Add(this.butShow);
			this.Controls.Add(this.listUser);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormUserPick";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Pick User";
			this.Load += new System.EventHandler(this.FormUserPick_Load);
			this.groupSelect.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.ListBox listUser;
		private UI.Button butShow;
		private UI.Button butNone;
		private UI.Button butAll;
		private System.Windows.Forms.GroupBox groupSelect;
	}
}