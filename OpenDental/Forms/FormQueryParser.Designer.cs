namespace OpenDental{
	partial class FormQueryParser {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQueryParser));
			this.textQuery = new OpenDental.ODtextBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.butShowHide = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.label1 = new System.Windows.Forms.Label();
			this.timerRefreshGrid = new System.Windows.Forms.Timer(this.components);
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// textQuery
			// 
			this.textQuery.AcceptsTab = true;
			this.textQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textQuery.BackColor = System.Drawing.SystemColors.Window;
			this.textQuery.DetectLinksEnabled = false;
			this.textQuery.DetectUrls = false;
			this.textQuery.HideSelection = false;
			this.textQuery.Location = new System.Drawing.Point(2, 23);
			this.textQuery.Name = "textQuery";
			this.textQuery.QuickPasteType = OpenDentBusiness.QuickPasteType.Query;
			this.textQuery.Size = new System.Drawing.Size(785, 361);
			this.textQuery.TabIndex = 1;
			this.textQuery.Text = "";
			this.textQuery.WordWrap = false;
			this.textQuery.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textQuery_KeyPress);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(9, 32);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.butShowHide);
			this.splitContainer1.Panel1.Controls.Add(this.gridMain);
			this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.label1);
			this.splitContainer1.Panel2.Controls.Add(this.textQuery);
			this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.splitContainer1.Size = new System.Drawing.Size(793, 606);
			this.splitContainer1.SplitterDistance = 210;
			this.splitContainer1.SplitterWidth = 7;
			this.splitContainer1.TabIndex = 3;
			// 
			// butShowHide
			// 
			this.butShowHide.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butShowHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butShowHide.Autosize = true;
			this.butShowHide.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShowHide.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShowHide.CornerRadius = 4F;
			this.butShowHide.Image = global::OpenDental.Properties.Resources.arrowDownTriangle;
			this.butShowHide.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.butShowHide.Location = new System.Drawing.Point(0, 181);
			this.butShowHide.Name = "butShowHide";
			this.butShowHide.Size = new System.Drawing.Size(91, 24);
			this.butShowHide.TabIndex = 41;
			this.butShowHide.Text = "Show Text";
			this.butShowHide.UseVisualStyleBackColor = true;
			this.butShowHide.Click += new System.EventHandler(this.butShowHide_Click);
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
			this.gridMain.Location = new System.Drawing.Point(0, -1);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridMain.Size = new System.Drawing.Size(791, 181);
			this.gridMain.TabIndex = 0;
			this.gridMain.Title = "Set Variables";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableQueryVariables";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			this.gridMain.CellLeave += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellLeave);
			this.gridMain.CellEnter += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellEnter);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(3, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(127, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Query Text:";
			// 
			// timerRefreshGrid
			// 
			this.timerRefreshGrid.Interval = 500;
			this.timerRefreshGrid.Tick += new System.EventHandler(this.timerRefreshGrid_Tick);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(649, 646);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 23);
			this.butOK.TabIndex = 4;
			this.butOK.Text = "&OK";
			this.butOK.UseVisualStyleBackColor = true;
			this.butOK.Click += new System.EventHandler(this.butOk_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(730, 646);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 23);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "Cancel";
			this.butCancel.UseVisualStyleBackColor = true;
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormQueryParser
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(817, 681);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.splitContainer1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(100, 100);
			this.Name = "FormQueryParser";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Query Variables";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormQueryParser_FormClosing);
			this.Load += new System.EventHandler(this.FormQueryParser_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private UI.ODGrid gridMain;
		private OpenDental.ODtextBox textQuery;
		private UI.Button butCancel;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label label1;
		private UI.Button butOK;
		private UI.Button butShowHide;
		private System.Windows.Forms.Timer timerRefreshGrid;
	}
}