namespace OpenDental{
	partial class FormPhoneConfs {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPhoneConfs));
			this.butClose = new OpenDental.UI.Button();
			this.gridConfRoom = new OpenDental.UI.ODGrid();
			this.butRefresh = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butKick = new OpenDental.UI.Button();
			this.checkHideEmpty = new System.Windows.Forms.CheckBox();
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
			this.butClose.Location = new System.Drawing.Point(590, 584);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(82, 24);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// gridConfRoom
			// 
			this.gridConfRoom.AllowSortingByColumn = true;
			this.gridConfRoom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridConfRoom.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridConfRoom.HasAddButton = false;
			this.gridConfRoom.HasDropDowns = false;
			this.gridConfRoom.HasMultilineHeaders = false;
			this.gridConfRoom.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridConfRoom.HeaderHeight = 15;
			this.gridConfRoom.HScrollVisible = false;
			this.gridConfRoom.Location = new System.Drawing.Point(12, 12);
			this.gridConfRoom.Name = "gridConfRoom";
			this.gridConfRoom.ScrollValue = 0;
			this.gridConfRoom.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridConfRoom.Size = new System.Drawing.Size(531, 596);
			this.gridConfRoom.TabIndex = 140;
			this.gridConfRoom.Title = "Conference Rooms";
			this.gridConfRoom.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridConfRoom.TitleHeight = 18;
			this.gridConfRoom.TranslationName = "TableConfRooms";
			this.gridConfRoom.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridConfRoom_CellDoubleClick);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(590, 12);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(82, 24);
			this.butRefresh.TabIndex = 141;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
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
			this.butAdd.Location = new System.Drawing.Point(590, 127);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(82, 26);
			this.butAdd.TabIndex = 142;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
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
			this.butDelete.Location = new System.Drawing.Point(590, 174);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(82, 25);
			this.butDelete.TabIndex = 143;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butKick
			// 
			this.butKick.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butKick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butKick.Autosize = true;
			this.butKick.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butKick.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butKick.CornerRadius = 4F;
			this.butKick.Location = new System.Drawing.Point(590, 298);
			this.butKick.Name = "butKick";
			this.butKick.Size = new System.Drawing.Size(82, 24);
			this.butKick.TabIndex = 144;
			this.butKick.Text = "Kick";
			this.butKick.Click += new System.EventHandler(this.butKick_Click);
			// 
			// checkHideEmpty
			// 
			this.checkHideEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkHideEmpty.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHideEmpty.Location = new System.Drawing.Point(549, 42);
			this.checkHideEmpty.Name = "checkHideEmpty";
			this.checkHideEmpty.Size = new System.Drawing.Size(123, 24);
			this.checkHideEmpty.TabIndex = 145;
			this.checkHideEmpty.Text = "Hide Empty Rooms";
			this.checkHideEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHideEmpty.UseVisualStyleBackColor = true;
			// 
			// FormPhoneConfs
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(684, 620);
			this.Controls.Add(this.checkHideEmpty);
			this.Controls.Add(this.butKick);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.gridConfRoom);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(700, 459);
			this.Name = "FormPhoneConfs";
			this.Text = "Conference Rooms";
			this.Load += new System.EventHandler(this.FormPhoneConf_Load);
			this.ResumeLayout(false);

		}

		#endregion
		private OpenDental.UI.Button butClose;
		private UI.ODGrid gridConfRoom;
		private UI.Button butRefresh;
		private UI.Button butAdd;
		private UI.Button butDelete;
		private UI.Button butKick;
		private System.Windows.Forms.CheckBox checkHideEmpty;
	}
}