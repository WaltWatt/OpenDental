namespace OpenDental{
	partial class FormWikiListHeaders {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWikiListHeaders));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.gridPickList = new OpenDental.UI.ODGrid();
			this.butAdd = new OpenDental.UI.Button();
			this.butRemove = new OpenDental.UI.Button();
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
			this.butOK.Location = new System.Drawing.Point(594, 468);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 21;
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
			this.butCancel.Location = new System.Drawing.Point(675, 468);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 20;
			this.butCancel.Text = "Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
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
			this.gridMain.Location = new System.Drawing.Point(12, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridMain.Size = new System.Drawing.Size(388, 433);
			this.gridMain.TabIndex = 26;
			this.gridMain.Title = "Wiki List Headers";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableWikiListHeaders";
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// gridPickList
			// 
			this.gridPickList.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridPickList.HasAddButton = false;
			this.gridPickList.HasDropDowns = false;
			this.gridPickList.HasMultilineHeaders = false;
			this.gridPickList.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridPickList.HeaderHeight = 15;
			this.gridPickList.HScrollVisible = false;
			this.gridPickList.Location = new System.Drawing.Point(406, 12);
			this.gridPickList.Name = "gridPickList";
			this.gridPickList.ScrollValue = 0;
			this.gridPickList.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridPickList.Size = new System.Drawing.Size(263, 433);
			this.gridPickList.TabIndex = 28;
			this.gridPickList.Title = "Pick List Options";
			this.gridPickList.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridPickList.TitleHeight = 18;
			this.gridPickList.TranslationName = "TableWikiListHeaderPickList";
			this.gridPickList.CellLeave += new OpenDental.UI.ODGridClickEventHandler(this.gridPickList_CellLeave);
			this.gridPickList.CellEnter += new OpenDental.UI.ODGridClickEventHandler(this.gridPickList_CellEnter);
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
			this.butAdd.Location = new System.Drawing.Point(675, 162);
			this.butAdd.Name = "butAdd";
			this.butAdd.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.butAdd.Size = new System.Drawing.Size(75, 23);
			this.butAdd.TabIndex = 29;
			this.butAdd.Text = "Add";
			this.butAdd.UseVisualStyleBackColor = true;
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butRemove
			// 
			this.butRemove.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRemove.Autosize = true;
			this.butRemove.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRemove.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRemove.CornerRadius = 4F;
			this.butRemove.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butRemove.Location = new System.Drawing.Point(675, 217);
			this.butRemove.Name = "butRemove";
			this.butRemove.Size = new System.Drawing.Size(75, 23);
			this.butRemove.TabIndex = 30;
			this.butRemove.Text = "Remove";
			this.butRemove.UseVisualStyleBackColor = true;
			this.butRemove.Click += new System.EventHandler(this.butRemove_Click);
			// 
			// FormWikiListHeaders
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(764, 504);
			this.Controls.Add(this.butRemove);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.gridPickList);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormWikiListHeaders";
			this.Text = "Edit Wiki List Headers";
			this.Load += new System.EventHandler(this.FormWikiListHeaders_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private UI.Button butOK;
		private UI.Button butCancel;
		private UI.ODGrid gridMain;
		private UI.ODGrid gridPickList;
		private UI.Button butAdd;
		private UI.Button butRemove;
	}
}