namespace OpenDental{
	partial class FormEtrans834Preview {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEtrans834Preview));
			this.checkIsPatientCreate = new System.Windows.Forms.CheckBox();
			this.gridInsPlans = new OpenDental.UI.ODGrid();
			this.label1 = new System.Windows.Forms.Label();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.labelProgress = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// checkIsPatientCreate
			// 
			this.checkIsPatientCreate.Location = new System.Drawing.Point(12, 96);
			this.checkIsPatientCreate.Name = "checkIsPatientCreate";
			this.checkIsPatientCreate.Size = new System.Drawing.Size(950, 20);
			this.checkIsPatientCreate.TabIndex = 8;
			this.checkIsPatientCreate.Text = "Automatically create new patients when importing plans for unknown patients.  If " +
    "unchecked, you can still add patients by selecting manually.";
			this.checkIsPatientCreate.UseVisualStyleBackColor = true;
			// 
			// gridInsPlans
			// 
			this.gridInsPlans.AllowSortingByColumn = true;
			this.gridInsPlans.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridInsPlans.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridInsPlans.HasAddButton = false;
			this.gridInsPlans.HasDropDowns = false;
			this.gridInsPlans.HasMultilineHeaders = false;
			this.gridInsPlans.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridInsPlans.HeaderHeight = 15;
			this.gridInsPlans.HScrollVisible = false;
			this.gridInsPlans.Location = new System.Drawing.Point(12, 122);
			this.gridInsPlans.Name = "gridInsPlans";
			this.gridInsPlans.ScrollValue = 0;
			this.gridInsPlans.Size = new System.Drawing.Size(950, 532);
			this.gridInsPlans.TabIndex = 4;
			this.gridInsPlans.Title = "Ins Plans";
			this.gridInsPlans.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridInsPlans.TitleHeight = 18;
			this.gridInsPlans.TranslationName = "TablePlan";
			this.gridInsPlans.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridInsPlans_CellDoubleClick);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(950, 84);
			this.label1.TabIndex = 9;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(806, 660);
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
			this.butCancel.Location = new System.Drawing.Point(887, 660);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// labelProgress
			// 
			this.labelProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelProgress.Location = new System.Drawing.Point(12, 662);
			this.labelProgress.Name = "labelProgress";
			this.labelProgress.Size = new System.Drawing.Size(788, 20);
			this.labelProgress.TabIndex = 10;
			this.labelProgress.Text = " ";
			this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormEtrans834Preview
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(974, 696);
			this.Controls.Add(this.labelProgress);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkIsPatientCreate);
			this.Controls.Add(this.gridInsPlans);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(990, 734);
			this.Name = "FormEtrans834Preview";
			this.Text = "Import Ins Plans 834 Preview";
			this.Load += new System.EventHandler(this.FormEtrans834Preview_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private UI.ODGrid gridInsPlans;
		private System.Windows.Forms.CheckBox checkIsPatientCreate;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelProgress;
	}
}