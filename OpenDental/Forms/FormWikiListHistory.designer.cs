namespace OpenDental{
	partial class FormWikiListHistory {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWikiListHistory));
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butRevert = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.gridOld = new OpenDental.UI.ODGrid();
			this.gridCur = new OpenDental.UI.ODGrid();
			this.SuspendLayout();
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(248, 614);
			this.gridMain.TabIndex = 5;
			this.gridMain.Title = "Wiki List History";
			this.gridMain.TranslationName = "TableWikiListHistory";
			this.gridMain.Click += new System.EventHandler(this.gridMain_Click);
			// 
			// butRevert
			// 
			this.butRevert.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRevert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRevert.Autosize = true;
			this.butRevert.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRevert.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRevert.CornerRadius = 4F;
			this.butRevert.Location = new System.Drawing.Point(887, 12);
			this.butRevert.Name = "butRevert";
			this.butRevert.Size = new System.Drawing.Size(75, 24);
			this.butRevert.TabIndex = 84;
			this.butRevert.Text = "Revert";
			this.butRevert.Click += new System.EventHandler(this.butRevert_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(887, 602);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// gridOld
			// 
			this.gridOld.AllowSortingByColumn = true;
			this.gridOld.EditableAcceptsCR = true;
			this.gridOld.HasAddButton = false;
			this.gridOld.HasMultilineHeaders = false;
			this.gridOld.HScrollVisible = true;
			this.gridOld.Location = new System.Drawing.Point(266, 12);
			this.gridOld.Name = "gridOld";
			this.gridOld.ScrollValue = 0;
			this.gridOld.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridOld.Size = new System.Drawing.Size(310, 614);
			this.gridOld.TabIndex = 85;
			this.gridOld.Title = "Old Revision";
			this.gridOld.TranslationName = "TableOldRevision";
			// 
			// gridCur
			// 
			this.gridCur.AllowSortingByColumn = true;
			this.gridCur.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.gridCur.EditableAcceptsCR = true;
			this.gridCur.HasAddButton = false;
			this.gridCur.HasMultilineHeaders = false;
			this.gridCur.HScrollVisible = true;
			this.gridCur.Location = new System.Drawing.Point(582, 12);
			this.gridCur.Name = "gridCur";
			this.gridCur.ScrollValue = 0;
			this.gridCur.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridCur.Size = new System.Drawing.Size(299, 614);
			this.gridCur.TabIndex = 86;
			this.gridCur.Title = "Current Revision";
			this.gridCur.TranslationName = "TableCurrentRevision";
			// 
			// FormWikiListHistory
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(974, 638);
			this.Controls.Add(this.gridCur);
			this.Controls.Add(this.gridOld);
			this.Controls.Add(this.butRevert);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(990, 676);
			this.Name = "FormWikiListHistory";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Wiki List History";
			this.Load += new System.EventHandler(this.FormWikiListHistory_Load);
			this.Resize += new System.EventHandler(this.FormWikiListHistory_Resize);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butClose;
		private UI.ODGrid gridMain;
		private UI.Button butRevert;
		private UI.ODGrid gridOld;
		private UI.ODGrid gridCur;
	}
}