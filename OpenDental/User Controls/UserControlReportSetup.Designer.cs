namespace OpenDental.User_Controls {
	partial class UserControlReportSetup {
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
			this.labelODInternal = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.labelUserGroup = new System.Windows.Forms.Label();
			this.comboUserGroup = new System.Windows.Forms.ComboBox();
			this.gridProdInc = new OpenDental.UI.ODGrid();
			this.gridDaily = new OpenDental.UI.ODGrid();
			this.gridMonthly = new OpenDental.UI.ODGrid();
			this.gridLists = new OpenDental.UI.ODGrid();
			this.butDown = new OpenDental.UI.Button();
			this.gridPublicHealth = new OpenDental.UI.ODGrid();
			this.butUp = new OpenDental.UI.Button();
			this.butSetAll = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// labelODInternal
			// 
			this.labelODInternal.Location = new System.Drawing.Point(311, 512);
			this.labelODInternal.Name = "labelODInternal";
			this.labelODInternal.Size = new System.Drawing.Size(161, 15);
			this.labelODInternal.TabIndex = 222;
			this.labelODInternal.Text = "None";
			this.labelODInternal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(264, 462);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(255, 42);
			this.label1.TabIndex = 221;
			this.label1.Text = "Move the selected item within its list.\r\nThe current selection\'s internal name is" +
    ":";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// labelUserGroup
			// 
			this.labelUserGroup.Location = new System.Drawing.Point(10, 5);
			this.labelUserGroup.Name = "labelUserGroup";
			this.labelUserGroup.Size = new System.Drawing.Size(100, 23);
			this.labelUserGroup.TabIndex = 224;
			this.labelUserGroup.Text = "User Group";
			this.labelUserGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboUserGroup
			// 
			this.comboUserGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboUserGroup.Location = new System.Drawing.Point(111, 5);
			this.comboUserGroup.Name = "comboUserGroup";
			this.comboUserGroup.Size = new System.Drawing.Size(147, 21);
			this.comboUserGroup.TabIndex = 225;
			this.comboUserGroup.SelectionChangeCommitted += new System.EventHandler(this.comboUserGroup_SelectionChangeCommitted);
			// 
			// gridProdInc
			// 
			this.gridProdInc.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridProdInc.HasAddButton = false;
			this.gridProdInc.HasDropDowns = false;
			this.gridProdInc.HasMultilineHeaders = true;
			this.gridProdInc.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridProdInc.HeaderHeight = 15;
			this.gridProdInc.HScrollVisible = false;
			this.gridProdInc.Location = new System.Drawing.Point(3, 35);
			this.gridProdInc.Name = "gridProdInc";
			this.gridProdInc.ScrollValue = 0;
			this.gridProdInc.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridProdInc.Size = new System.Drawing.Size(255, 151);
			this.gridProdInc.TabIndex = 214;
			this.gridProdInc.Title = "Production & Income";
			this.gridProdInc.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridProdInc.TitleHeight = 18;
			this.gridProdInc.TranslationName = "TableProductionIncome";
			this.gridProdInc.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellClick);
			this.gridProdInc.CellLeave += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellLeave);
			// 
			// gridDaily
			// 
			this.gridDaily.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridDaily.HasAddButton = false;
			this.gridDaily.HasDropDowns = false;
			this.gridDaily.HasMultilineHeaders = true;
			this.gridDaily.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridDaily.HeaderHeight = 15;
			this.gridDaily.HScrollVisible = false;
			this.gridDaily.Location = new System.Drawing.Point(3, 192);
			this.gridDaily.Name = "gridDaily";
			this.gridDaily.ScrollValue = 0;
			this.gridDaily.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridDaily.Size = new System.Drawing.Size(255, 151);
			this.gridDaily.TabIndex = 215;
			this.gridDaily.Title = "Daily";
			this.gridDaily.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridDaily.TitleHeight = 18;
			this.gridDaily.TranslationName = "TableDaily";
			this.gridDaily.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellClick);
			this.gridDaily.CellLeave += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellLeave);
			// 
			// gridMonthly
			// 
			this.gridMonthly.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMonthly.HasAddButton = false;
			this.gridMonthly.HasDropDowns = false;
			this.gridMonthly.HasMultilineHeaders = true;
			this.gridMonthly.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMonthly.HeaderHeight = 15;
			this.gridMonthly.HScrollVisible = false;
			this.gridMonthly.Location = new System.Drawing.Point(3, 346);
			this.gridMonthly.Name = "gridMonthly";
			this.gridMonthly.ScrollValue = 0;
			this.gridMonthly.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridMonthly.Size = new System.Drawing.Size(255, 261);
			this.gridMonthly.TabIndex = 216;
			this.gridMonthly.Title = "Monthly";
			this.gridMonthly.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMonthly.TitleHeight = 18;
			this.gridMonthly.TranslationName = "TableMonthly";
			this.gridMonthly.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellClick);
			this.gridMonthly.CellLeave += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellLeave);
			// 
			// gridLists
			// 
			this.gridLists.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridLists.HasAddButton = false;
			this.gridLists.HasDropDowns = false;
			this.gridLists.HasMultilineHeaders = true;
			this.gridLists.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridLists.HeaderHeight = 15;
			this.gridLists.HScrollVisible = false;
			this.gridLists.Location = new System.Drawing.Point(267, 35);
			this.gridLists.Name = "gridLists";
			this.gridLists.ScrollValue = 0;
			this.gridLists.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridLists.Size = new System.Drawing.Size(255, 308);
			this.gridLists.TabIndex = 217;
			this.gridLists.Title = "Lists";
			this.gridLists.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridLists.TitleHeight = 18;
			this.gridLists.TranslationName = "TableLists";
			this.gridLists.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellClick);
			this.gridLists.CellLeave += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellLeave);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(448, 5);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(71, 24);
			this.butDown.TabIndex = 220;
			this.butDown.Text = "Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// gridPublicHealth
			// 
			this.gridPublicHealth.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridPublicHealth.HasAddButton = false;
			this.gridPublicHealth.HasDropDowns = false;
			this.gridPublicHealth.HasMultilineHeaders = true;
			this.gridPublicHealth.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridPublicHealth.HeaderHeight = 15;
			this.gridPublicHealth.HScrollVisible = false;
			this.gridPublicHealth.Location = new System.Drawing.Point(267, 346);
			this.gridPublicHealth.Name = "gridPublicHealth";
			this.gridPublicHealth.ScrollValue = 0;
			this.gridPublicHealth.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridPublicHealth.Size = new System.Drawing.Size(255, 103);
			this.gridPublicHealth.TabIndex = 218;
			this.gridPublicHealth.Title = "Public Health";
			this.gridPublicHealth.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridPublicHealth.TitleHeight = 18;
			this.gridPublicHealth.TranslationName = "TableHealth";
			this.gridPublicHealth.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellClick);
			this.gridPublicHealth.CellLeave += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellLeave);
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
			this.butUp.Location = new System.Drawing.Point(371, 5);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(71, 24);
			this.butUp.TabIndex = 219;
			this.butUp.Text = "Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butSetAll
			// 
			this.butSetAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSetAll.Autosize = true;
			this.butSetAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSetAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSetAll.CornerRadius = 4F;
			this.butSetAll.Location = new System.Drawing.Point(267, 5);
			this.butSetAll.Name = "butSetAll";
			this.butSetAll.Size = new System.Drawing.Size(79, 24);
			this.butSetAll.TabIndex = 223;
			this.butSetAll.Text = "Set All";
			this.butSetAll.Click += new System.EventHandler(this.butSetAll_Click);
			// 
			// UserControlReportSetup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.comboUserGroup);
			this.Controls.Add(this.labelUserGroup);
			this.Controls.Add(this.gridProdInc);
			this.Controls.Add(this.gridDaily);
			this.Controls.Add(this.labelODInternal);
			this.Controls.Add(this.gridMonthly);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridLists);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.gridPublicHealth);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.butSetAll);
			this.Name = "UserControlReportSetup";
			this.Size = new System.Drawing.Size(525, 610);
			this.ResumeLayout(false);

		}

		#endregion

		private UI.Button butUp;
		private UI.ODGrid gridPublicHealth;
		private UI.Button butDown;
		private UI.ODGrid gridLists;
		private System.Windows.Forms.Label label1;
		private UI.ODGrid gridMonthly;
		private System.Windows.Forms.Label labelODInternal;
		private UI.ODGrid gridDaily;
		private UI.ODGrid gridProdInc;
		private UI.Button butSetAll;
		private System.Windows.Forms.Label labelUserGroup;
		private System.Windows.Forms.ComboBox comboUserGroup;
	}
}
