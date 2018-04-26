namespace OpenDental{
	partial class FormAlertCategorySetup {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAlertCategorySetup));
			this.butDuplicate = new OpenDental.UI.Button();
			this.butCopy = new OpenDental.UI.Button();
			this.gridInternal = new OpenDental.UI.ODGrid();
			this.gridCustom = new OpenDental.UI.ODGrid();
			this.butClose = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// butDuplicate
			// 
			this.butDuplicate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDuplicate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDuplicate.Autosize = true;
			this.butDuplicate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDuplicate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDuplicate.CornerRadius = 4F;
			this.butDuplicate.Image = global::OpenDental.Properties.Resources.Add;
			this.butDuplicate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDuplicate.Location = new System.Drawing.Point(700, 565);
			this.butDuplicate.Name = "butDuplicate";
			this.butDuplicate.Size = new System.Drawing.Size(89, 24);
			this.butDuplicate.TabIndex = 13;
			this.butDuplicate.Text = "Duplicate";
			this.butDuplicate.Click += new System.EventHandler(this.butDuplicate_Click);
			// 
			// butCopy
			// 
			this.butCopy.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCopy.Autosize = true;
			this.butCopy.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCopy.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCopy.CornerRadius = 4F;
			this.butCopy.Image = global::OpenDental.Properties.Resources.Right;
			this.butCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCopy.Location = new System.Drawing.Point(403, 229);
			this.butCopy.Name = "butCopy";
			this.butCopy.Size = new System.Drawing.Size(75, 24);
			this.butCopy.TabIndex = 11;
			this.butCopy.Text = "Copy";
			this.butCopy.Click += new System.EventHandler(this.butCopy_Click);
			// 
			// gridInternal
			// 
			this.gridInternal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridInternal.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridInternal.HasAddButton = false;
			this.gridInternal.HasDropDowns = false;
			this.gridInternal.HasMultilineHeaders = false;
			this.gridInternal.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridInternal.HeaderHeight = 15;
			this.gridInternal.HScrollVisible = false;
			this.gridInternal.Location = new System.Drawing.Point(12, 17);
			this.gridInternal.Name = "gridInternal";
			this.gridInternal.ScrollValue = 0;
			this.gridInternal.Size = new System.Drawing.Size(370, 542);
			this.gridInternal.TabIndex = 9;
			this.gridInternal.Title = "Internal";
			this.gridInternal.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridInternal.TitleHeight = 18;
			this.gridInternal.TranslationName = "TableInternal";
			this.gridInternal.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridInternal_CellDoubleClick);
			// 
			// gridCustom
			// 
			this.gridCustom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridCustom.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridCustom.HasAddButton = false;
			this.gridCustom.HasDropDowns = false;
			this.gridCustom.HasMultilineHeaders = false;
			this.gridCustom.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridCustom.HeaderHeight = 15;
			this.gridCustom.HScrollVisible = false;
			this.gridCustom.Location = new System.Drawing.Point(493, 17);
			this.gridCustom.Name = "gridCustom";
			this.gridCustom.ScrollValue = 0;
			this.gridCustom.Size = new System.Drawing.Size(376, 542);
			this.gridCustom.TabIndex = 10;
			this.gridCustom.Title = "Custom";
			this.gridCustom.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridCustom.TitleHeight = 18;
			this.gridCustom.TranslationName = "TableCustom";
			this.gridCustom.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridCustom_CellDoubleClick);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(794, 565);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 14;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormAlertCategorySetup
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(881, 601);
			this.ControlBox = false;
			this.Controls.Add(this.butDuplicate);
			this.Controls.Add(this.butCopy);
			this.Controls.Add(this.gridInternal);
			this.Controls.Add(this.gridCustom);
			this.Controls.Add(this.butClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(0, 0);
			this.Name = "FormAlertCategorySetup";
			this.Text = "Alert Category Setup";
			this.Load += new System.EventHandler(this.FormAlertCategorySetup_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private UI.Button butDuplicate;
		private UI.Button butCopy;
		private UI.ODGrid gridInternal;
		private UI.ODGrid gridCustom;
		private UI.Button butClose;
	}
}