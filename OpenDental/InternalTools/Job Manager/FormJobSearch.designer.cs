namespace OpenDental{
	partial class FormJobSearch {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJobSearch));
			this.label2 = new System.Windows.Forms.Label();
			this.textSearch = new System.Windows.Forms.TextBox();
			this.listBoxUsers = new System.Windows.Forms.ListBox();
			this.listBoxStatus = new System.Windows.Forms.ListBox();
			this.listBoxCategory = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.timerSearch = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(9, 11);
			this.label2.Margin = new System.Windows.Forms.Padding(0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(77, 20);
			this.label2.TabIndex = 229;
			this.label2.Text = "Search Terms";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSearch
			// 
			this.textSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textSearch.Location = new System.Drawing.Point(89, 12);
			this.textSearch.Name = "textSearch";
			this.textSearch.Size = new System.Drawing.Size(1155, 20);
			this.textSearch.TabIndex = 228;
			this.textSearch.TextChanged += new System.EventHandler(this.textSearch_TextChanged);
			// 
			// listBoxUsers
			// 
			this.listBoxUsers.FormattingEnabled = true;
			this.listBoxUsers.Location = new System.Drawing.Point(89, 38);
			this.listBoxUsers.Name = "listBoxUsers";
			this.listBoxUsers.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxUsers.Size = new System.Drawing.Size(144, 186);
			this.listBoxUsers.TabIndex = 230;
			this.listBoxUsers.SelectedIndexChanged += new System.EventHandler(this.listBoxUsers_SelectedIndexChanged);
			// 
			// listBoxStatus
			// 
			this.listBoxStatus.FormattingEnabled = true;
			this.listBoxStatus.Location = new System.Drawing.Point(89, 230);
			this.listBoxStatus.Name = "listBoxStatus";
			this.listBoxStatus.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxStatus.Size = new System.Drawing.Size(144, 95);
			this.listBoxStatus.TabIndex = 234;
			this.listBoxStatus.SelectedIndexChanged += new System.EventHandler(this.listBoxStatus_SelectedIndexChanged);
			// 
			// listBoxCategory
			// 
			this.listBoxCategory.FormattingEnabled = true;
			this.listBoxCategory.Location = new System.Drawing.Point(89, 331);
			this.listBoxCategory.Name = "listBoxCategory";
			this.listBoxCategory.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxCategory.Size = new System.Drawing.Size(144, 82);
			this.listBoxCategory.TabIndex = 245;
			this.listBoxCategory.SelectedIndexChanged += new System.EventHandler(this.listBoxCategory_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(31, 38);
			this.label3.Margin = new System.Windows.Forms.Padding(0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 20);
			this.label3.TabIndex = 249;
			this.label3.Text = "Users";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(31, 230);
			this.label1.Margin = new System.Windows.Forms.Padding(0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(55, 20);
			this.label1.TabIndex = 250;
			this.label1.Text = "Phase";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(31, 331);
			this.label5.Margin = new System.Windows.Forms.Padding(0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(55, 20);
			this.label5.TabIndex = 251;
			this.label5.Text = "Category";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gridMain
			// 
			this.gridMain.AllowSortingByColumn = true;
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = true;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(239, 38);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(1005, 570);
			this.gridMain.TabIndex = 227;
			this.gridMain.Title = "Details Grid";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(1077, 622);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(80, 24);
			this.butOK.TabIndex = 248;
			this.butOK.Text = "OK";
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
			this.butCancel.Location = new System.Drawing.Point(1164, 622);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(80, 24);
			this.butCancel.TabIndex = 247;
			this.butCancel.Text = "Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// timerSearch
			// 
			this.timerSearch.Interval = 500;
			this.timerSearch.Tick += new System.EventHandler(this.timerSearch_Tick);
			// 
			// FormJobSearch
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1256, 658);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.listBoxCategory);
			this.Controls.Add(this.listBoxStatus);
			this.Controls.Add(this.listBoxUsers);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textSearch);
			this.Controls.Add(this.gridMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormJobSearch";
			this.Text = "Search Jobs";
			this.Load += new System.EventHandler(this.FormJobNew_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private UI.ODGrid gridMain;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textSearch;
		private System.Windows.Forms.ListBox listBoxUsers;
		private System.Windows.Forms.ListBox listBoxStatus;
		private System.Windows.Forms.ListBox listBoxCategory;
		private UI.Button butCancel;
		private UI.Button butOK;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Timer timerSearch;
	}
}