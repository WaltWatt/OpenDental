namespace OpenDental{
	partial class FormInsPlanSubstitution {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInsPlanSubstitution));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.gridInsPlanSubstInc = new OpenDental.UI.ODGrid();
			this.gridInsPlanSubstExc = new OpenDental.UI.ODGrid();
			this.butRight = new OpenDental.UI.Button();
			this.butLeft = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
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
			this.butOK.Location = new System.Drawing.Point(704, 422);
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
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(785, 422);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// gridInsPlanSubstInc
			// 
			this.gridInsPlanSubstInc.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridInsPlanSubstInc.HasAddButton = false;
			this.gridInsPlanSubstInc.HasDropDowns = false;
			this.gridInsPlanSubstInc.HasMultilineHeaders = false;
			this.gridInsPlanSubstInc.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridInsPlanSubstInc.HeaderHeight = 15;
			this.gridInsPlanSubstInc.HScrollVisible = false;
			this.gridInsPlanSubstInc.Location = new System.Drawing.Point(12, 64);
			this.gridInsPlanSubstInc.Name = "gridInsPlanSubstInc";
			this.gridInsPlanSubstInc.ScrollValue = 0;
			this.gridInsPlanSubstInc.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridInsPlanSubstInc.Size = new System.Drawing.Size(400, 348);
			this.gridInsPlanSubstInc.TabIndex = 4;
			this.gridInsPlanSubstInc.Title = "Codes To Substitute";
			this.gridInsPlanSubstInc.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridInsPlanSubstInc.TitleHeight = 18;
			this.gridInsPlanSubstInc.TranslationName = "TableInsPlanSubstInc";
			// 
			// gridInsPlanSubstExc
			// 
			this.gridInsPlanSubstExc.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridInsPlanSubstExc.HasAddButton = false;
			this.gridInsPlanSubstExc.HasDropDowns = false;
			this.gridInsPlanSubstExc.HasMultilineHeaders = false;
			this.gridInsPlanSubstExc.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridInsPlanSubstExc.HeaderHeight = 15;
			this.gridInsPlanSubstExc.HScrollVisible = false;
			this.gridInsPlanSubstExc.Location = new System.Drawing.Point(459, 64);
			this.gridInsPlanSubstExc.Name = "gridInsPlanSubstExc";
			this.gridInsPlanSubstExc.ScrollValue = 0;
			this.gridInsPlanSubstExc.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridInsPlanSubstExc.Size = new System.Drawing.Size(400, 348);
			this.gridInsPlanSubstExc.TabIndex = 5;
			this.gridInsPlanSubstExc.Title = "Codes To Exclude";
			this.gridInsPlanSubstExc.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridInsPlanSubstExc.TitleHeight = 18;
			this.gridInsPlanSubstExc.TranslationName = "TableInsPlanSubstExc";
			// 
			// butRight
			// 
			this.butRight.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRight.Autosize = true;
			this.butRight.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRight.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRight.CornerRadius = 4F;
			this.butRight.Image = global::OpenDental.Properties.Resources.Right;
			this.butRight.Location = new System.Drawing.Point(418, 177);
			this.butRight.Name = "butRight";
			this.butRight.Size = new System.Drawing.Size(35, 26);
			this.butRight.TabIndex = 55;
			this.butRight.Click += new System.EventHandler(this.butRight_Click);
			// 
			// butLeft
			// 
			this.butLeft.AdjustImageLocation = new System.Drawing.Point(-1, 0);
			this.butLeft.Autosize = true;
			this.butLeft.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLeft.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLeft.CornerRadius = 4F;
			this.butLeft.Image = global::OpenDental.Properties.Resources.Left;
			this.butLeft.Location = new System.Drawing.Point(418, 211);
			this.butLeft.Name = "butLeft";
			this.butLeft.Size = new System.Drawing.Size(35, 26);
			this.butLeft.TabIndex = 54;
			this.butLeft.Click += new System.EventHandler(this.butLeft_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(9, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(851, 52);
			this.label1.TabIndex = 56;
			this.label1.Text = resources.GetString("label1.Text");
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FormInsPlanSubstitution
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(872, 458);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butRight);
			this.Controls.Add(this.butLeft);
			this.Controls.Add(this.gridInsPlanSubstExc);
			this.Controls.Add(this.gridInsPlanSubstInc);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormInsPlanSubstitution";
			this.Text = "Insurance Plan Substitution Codes";
			this.Load += new System.EventHandler(this.FormInsPlanSubstitution_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private UI.ODGrid gridInsPlanSubstInc;
		private UI.ODGrid gridInsPlanSubstExc;
		private UI.Button butRight;
		private UI.Button butLeft;
		private System.Windows.Forms.Label label1;
	}
}