namespace OpenDental{
	partial class FormCdsTriggers {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCdsTriggers));
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butAddTrigger = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.menuItemSettings = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(683, 560);
			this.gridMain.TabIndex = 199;
			this.gridMain.Title = "CDS Triggers";
			this.gridMain.TranslationName = "TableCDSTriggers";
			this.gridMain.WrapText = false;
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butAddTrigger
			// 
			this.butAddTrigger.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butAddTrigger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAddTrigger.Autosize = true;
			this.butAddTrigger.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddTrigger.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddTrigger.CornerRadius = 4F;
			this.butAddTrigger.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddTrigger.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddTrigger.Location = new System.Drawing.Point(701, 12);
			this.butAddTrigger.Name = "butAddTrigger";
			this.butAddTrigger.Size = new System.Drawing.Size(75, 23);
			this.butAddTrigger.TabIndex = 201;
			this.butAddTrigger.Text = "Add";
			this.butAddTrigger.Click += new System.EventHandler(this.butAddTrigger_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(701, 548);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Close";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSettings});
			// 
			// menuItemSettings
			// 
			this.menuItemSettings.Index = 0;
			this.menuItemSettings.Text = "Setup";
			this.menuItemSettings.Click += new System.EventHandler(this.menuItemSettings_Click);
			// 
			// FormEhrTriggers
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(788, 584);
			this.Controls.Add(this.butAddTrigger);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.Name = "FormEhrTriggers";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CDS Triggers";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEhrTriggers_FormClosing);
			this.Load += new System.EventHandler(this.FormEhrTriggers_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butCancel;
		private UI.ODGrid gridMain;
		private UI.Button butAddTrigger;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItemSettings;
	}
}