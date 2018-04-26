namespace OpenDental{
	partial class FormDiscountPlanEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDiscountPlanEdit));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.textDescript = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textFeeSched = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.butFeeSched = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.comboBoxAdjType = new System.Windows.Forms.ComboBox();
			this.butDrop = new OpenDental.UI.Button();
			this.checkHidden = new System.Windows.Forms.CheckBox();
			this.butListPatients = new OpenDental.UI.Button();
			this.textNumPatients = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.comboPatient = new System.Windows.Forms.ComboBox();
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
			this.butOK.Location = new System.Drawing.Point(235, 141);
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
			this.butCancel.Location = new System.Drawing.Point(318, 141);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// textDescript
			// 
			this.textDescript.Location = new System.Drawing.Point(126, 30);
			this.textDescript.Name = "textDescript";
			this.textDescript.Size = new System.Drawing.Size(267, 20);
			this.textDescript.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(13, 31);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(113, 18);
			this.label1.TabIndex = 5;
			this.label1.Text = "Description";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textFeeSched
			// 
			this.textFeeSched.Location = new System.Drawing.Point(126, 56);
			this.textFeeSched.Name = "textFeeSched";
			this.textFeeSched.ReadOnly = true;
			this.textFeeSched.Size = new System.Drawing.Size(241, 20);
			this.textFeeSched.TabIndex = 6;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(13, 57);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(113, 18);
			this.label2.TabIndex = 7;
			this.label2.Text = "Fee Schedule";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butFeeSched
			// 
			this.butFeeSched.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butFeeSched.Autosize = true;
			this.butFeeSched.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butFeeSched.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butFeeSched.CornerRadius = 4F;
			this.butFeeSched.Location = new System.Drawing.Point(372, 56);
			this.butFeeSched.Name = "butFeeSched";
			this.butFeeSched.Size = new System.Drawing.Size(20, 20);
			this.butFeeSched.TabIndex = 8;
			this.butFeeSched.Text = "...";
			this.butFeeSched.Click += new System.EventHandler(this.butFeeSched_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(13, 83);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(113, 18);
			this.label3.TabIndex = 10;
			this.label3.Text = "Adjustment Type";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboBoxAdjType
			// 
			this.comboBoxAdjType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxAdjType.FormattingEnabled = true;
			this.comboBoxAdjType.Location = new System.Drawing.Point(126, 82);
			this.comboBoxAdjType.Name = "comboBoxAdjType";
			this.comboBoxAdjType.Size = new System.Drawing.Size(267, 21);
			this.comboBoxAdjType.TabIndex = 11;
			// 
			// butDrop
			// 
			this.butDrop.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDrop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDrop.Autosize = true;
			this.butDrop.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDrop.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDrop.CornerRadius = 4F;
			this.butDrop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDrop.Location = new System.Drawing.Point(12, 141);
			this.butDrop.Name = "butDrop";
			this.butDrop.Size = new System.Drawing.Size(75, 24);
			this.butDrop.TabIndex = 13;
			this.butDrop.Text = "Drop";
			this.butDrop.Visible = false;
			this.butDrop.Click += new System.EventHandler(this.butDrop_Click);
			// 
			// checkHidden
			// 
			this.checkHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHidden.Location = new System.Drawing.Point(36, 10);
			this.checkHidden.Name = "checkHidden";
			this.checkHidden.Size = new System.Drawing.Size(104, 18);
			this.checkHidden.TabIndex = 14;
			this.checkHidden.Text = "Hidden";
			this.checkHidden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHidden.UseVisualStyleBackColor = true;
			this.checkHidden.Click += new System.EventHandler(this.checkHidden_Click);
			// 
			// butListPatients
			// 
			this.butListPatients.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butListPatients.Autosize = true;
			this.butListPatients.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butListPatients.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butListPatients.CornerRadius = 4F;
			this.butListPatients.Location = new System.Drawing.Point(390, 109);
			this.butListPatients.Name = "butListPatients";
			this.butListPatients.Size = new System.Drawing.Size(188, 21);
			this.butListPatients.TabIndex = 22;
			this.butListPatients.Text = "List Patients";
			this.butListPatients.Click += new System.EventHandler(this.butListPatients_Click);
			// 
			// textNumPatients
			// 
			this.textNumPatients.Location = new System.Drawing.Point(127, 110);
			this.textNumPatients.Name = "textNumPatients";
			this.textNumPatients.ReadOnly = true;
			this.textNumPatients.Size = new System.Drawing.Size(72, 20);
			this.textNumPatients.TabIndex = 21;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(13, 112);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(113, 18);
			this.label4.TabIndex = 20;
			this.label4.Text = "Patients";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboPatient
			// 
			this.comboPatient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPatient.FormattingEnabled = true;
			this.comboPatient.Location = new System.Drawing.Point(205, 109);
			this.comboPatient.Name = "comboPatient";
			this.comboPatient.Size = new System.Drawing.Size(188, 21);
			this.comboPatient.TabIndex = 19;
			// 
			// FormDiscountPlanEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(405, 177);
			this.Controls.Add(this.butListPatients);
			this.Controls.Add(this.textNumPatients);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.comboPatient);
			this.Controls.Add(this.checkHidden);
			this.Controls.Add(this.butDrop);
			this.Controls.Add(this.comboBoxAdjType);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.butFeeSched);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textFeeSched);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textDescript);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(421, 199);
			this.Name = "FormDiscountPlanEdit";
			this.Text = "Discount Plan Edit";
			this.Load += new System.EventHandler(this.FormDiscountPlanEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.TextBox textDescript;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textFeeSched;
		private System.Windows.Forms.Label label2;
		private UI.Button butFeeSched;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox comboBoxAdjType;
		private UI.Button butDrop;
		private System.Windows.Forms.CheckBox checkHidden;
		private UI.Button butListPatients;
		private System.Windows.Forms.TextBox textNumPatients;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox comboPatient;
	}
}