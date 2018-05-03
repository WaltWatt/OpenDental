namespace OpenDental{
	partial class FormDefEditWSNPApptTypes {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDefEditWSNPApptTypes));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butSelect = new OpenDental.UI.Button();
			this.checkHidden = new System.Windows.Forms.CheckBox();
			this.butColor = new System.Windows.Forms.Button();
			this.textValue = new System.Windows.Forms.TextBox();
			this.textName = new System.Windows.Forms.TextBox();
			this.labelValue = new System.Windows.Forms.Label();
			this.labelName = new System.Windows.Forms.Label();
			this.labelColor = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
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
			this.butOK.Location = new System.Drawing.Point(311, 135);
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
			this.butCancel.Location = new System.Drawing.Point(392, 135);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butSelect
			// 
			this.butSelect.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSelect.Autosize = true;
			this.butSelect.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSelect.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSelect.CornerRadius = 4F;
			this.butSelect.Location = new System.Drawing.Point(371, 55);
			this.butSelect.Name = "butSelect";
			this.butSelect.Size = new System.Drawing.Size(21, 22);
			this.butSelect.TabIndex = 209;
			this.butSelect.Text = "...";
			this.butSelect.Click += new System.EventHandler(this.butSelect_Click);
			// 
			// checkHidden
			// 
			this.checkHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkHidden.Location = new System.Drawing.Point(12, 12);
			this.checkHidden.Name = "checkHidden";
			this.checkHidden.Size = new System.Drawing.Size(157, 18);
			this.checkHidden.TabIndex = 207;
			this.checkHidden.Text = "Hidden";
			this.checkHidden.Visible = false;
			// 
			// butColor
			// 
			this.butColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColor.Location = new System.Drawing.Point(437, 57);
			this.butColor.Name = "butColor";
			this.butColor.Size = new System.Drawing.Size(30, 20);
			this.butColor.TabIndex = 206;
			this.butColor.Visible = false;
			this.butColor.Click += new System.EventHandler(this.butColor_Click);
			// 
			// textValue
			// 
			this.textValue.Enabled = false;
			this.textValue.Location = new System.Drawing.Point(190, 55);
			this.textValue.MaxLength = 256;
			this.textValue.Multiline = true;
			this.textValue.Name = "textValue";
			this.textValue.Size = new System.Drawing.Size(178, 22);
			this.textValue.TabIndex = 204;
			// 
			// textName
			// 
			this.textName.Location = new System.Drawing.Point(12, 55);
			this.textName.Multiline = true;
			this.textName.Name = "textName";
			this.textName.Size = new System.Drawing.Size(178, 64);
			this.textName.TabIndex = 202;
			// 
			// labelValue
			// 
			this.labelValue.Location = new System.Drawing.Point(190, 25);
			this.labelValue.Name = "labelValue";
			this.labelValue.Size = new System.Drawing.Size(178, 28);
			this.labelValue.TabIndex = 205;
			this.labelValue.Text = "Appointment Type";
			this.labelValue.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelName
			// 
			this.labelName.Location = new System.Drawing.Point(12, 36);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(178, 17);
			this.labelName.TabIndex = 203;
			this.labelName.Text = "Reason";
			this.labelName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelColor
			// 
			this.labelColor.Location = new System.Drawing.Point(373, 39);
			this.labelColor.Name = "labelColor";
			this.labelColor.Size = new System.Drawing.Size(95, 15);
			this.labelColor.TabIndex = 208;
			this.labelColor.Text = "Color";
			this.labelColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelColor.Visible = false;
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(12, 135);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(79, 24);
			this.butDelete.TabIndex = 211;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// colorDialog1
			// 
			this.colorDialog1.FullOpen = true;
			// 
			// FormDefEditWSNPApptTypes
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(479, 171);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butSelect);
			this.Controls.Add(this.checkHidden);
			this.Controls.Add(this.butColor);
			this.Controls.Add(this.textValue);
			this.Controls.Add(this.textName);
			this.Controls.Add(this.labelValue);
			this.Controls.Add(this.labelName);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.labelColor);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(495, 210);
			this.Name = "FormDefEditWSNPApptTypes";
			this.Text = "Edit Definition";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private UI.Button butSelect;
		private System.Windows.Forms.CheckBox checkHidden;
		private System.Windows.Forms.Button butColor;
		private System.Windows.Forms.TextBox textValue;
		private System.Windows.Forms.TextBox textName;
		private System.Windows.Forms.Label labelValue;
		private System.Windows.Forms.Label labelName;
		private System.Windows.Forms.Label labelColor;
		private UI.Button butDelete;
		private System.Windows.Forms.ColorDialog colorDialog1;
	}
}