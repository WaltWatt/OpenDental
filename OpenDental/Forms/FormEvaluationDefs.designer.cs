namespace OpenDental{
	partial class FormEvaluationDefs {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEvaluationDefs));
			this.butCancel = new OpenDental.UI.Button();
			this.butDuplicate = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.comboCourse = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(375, 453);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 5;
			this.butCancel.Text = "Close";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butDuplicate
			// 
			this.butDuplicate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDuplicate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDuplicate.Autosize = true;
			this.butDuplicate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDuplicate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDuplicate.CornerRadius = 4F;
			this.butDuplicate.Location = new System.Drawing.Point(374, 252);
			this.butDuplicate.Name = "butDuplicate";
			this.butDuplicate.Size = new System.Drawing.Size(75, 24);
			this.butDuplicate.TabIndex = 2;
			this.butDuplicate.Text = "Duplicate";
			this.butDuplicate.Click += new System.EventHandler(this.butDuplicate_Click);
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
			this.butAdd.Location = new System.Drawing.Point(375, 282);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 24);
			this.butAdd.TabIndex = 3;
			this.butAdd.Text = "Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(266, 465);
			this.gridMain.TabIndex = 14;
			this.gridMain.Title = "Evaluation Definitions";
			this.gridMain.TranslationName = "TableEvaluationSetup";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// comboCourse
			// 
			this.comboCourse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboCourse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCourse.FormattingEnabled = true;
			this.comboCourse.ItemHeight = 13;
			this.comboCourse.Location = new System.Drawing.Point(284, 33);
			this.comboCourse.Name = "comboCourse";
			this.comboCourse.Size = new System.Drawing.Size(166, 21);
			this.comboCourse.TabIndex = 1;
			this.comboCourse.SelectionChangeCommitted += new System.EventHandler(this.comboCourse_SelectionChangeCommitted);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(284, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(93, 18);
			this.label1.TabIndex = 24;
			this.label1.Text = "Course";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormEvaluationDefs
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(461, 489);
			this.Controls.Add(this.comboCourse);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butDuplicate);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(379, 388);
			this.Name = "FormEvaluationDefs";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Evaluation Definitions";
			this.Load += new System.EventHandler(this.FormEvaluationDefs_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butCancel;
		private UI.Button butDuplicate;
		private UI.Button butAdd;
		private UI.ODGrid gridMain;
		private System.Windows.Forms.ComboBox comboCourse;
		private System.Windows.Forms.Label label1;
	}
}