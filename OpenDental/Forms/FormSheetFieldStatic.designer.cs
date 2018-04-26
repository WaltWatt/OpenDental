namespace OpenDental{
	partial class FormSheetFieldStatic {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSheetFieldStatic));
			this.checkPmtOpt = new System.Windows.Forms.CheckBox();
			this.labelTextW = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.listFields = new System.Windows.Forms.ListBox();
			this.textFieldValue = new System.Windows.Forms.TextBox();
			this.comboGrowthBehavior = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label10 = new System.Windows.Forms.Label();
			this.butColor = new System.Windows.Forms.Button();
			this.comboTextAlign = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.checkFontIsBold = new System.Windows.Forms.CheckBox();
			this.textFontSize = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.comboFontName = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.checkIsLocked = new System.Windows.Forms.CheckBox();
			this.butExamSheet = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.textHeight = new OpenDental.ValidNum();
			this.textWidth = new OpenDental.ValidNum();
			this.textYPos = new OpenDental.ValidNum();
			this.textXPos = new OpenDental.ValidNum();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkPmtOpt
			// 
			this.checkPmtOpt.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPmtOpt.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPmtOpt.Location = new System.Drawing.Point(5, 639);
			this.checkPmtOpt.Name = "checkPmtOpt";
			this.checkPmtOpt.Size = new System.Drawing.Size(109, 20);
			this.checkPmtOpt.TabIndex = 236;
			this.checkPmtOpt.Text = "Is Payment Option";
			this.checkPmtOpt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelTextW
			// 
			this.labelTextW.Location = new System.Drawing.Point(176, 590);
			this.labelTextW.Name = "labelTextW";
			this.labelTextW.Size = new System.Drawing.Size(109, 16);
			this.labelTextW.TabIndex = 104;
			this.labelTextW.Text = "TextW: ";
			this.labelTextW.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(311, 355);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(107, 16);
			this.label1.TabIndex = 103;
			this.label1.Text = "Click to insert Field";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listFields
			// 
			this.listFields.FormattingEnabled = true;
			this.listFields.IntegralHeight = false;
			this.listFields.Location = new System.Drawing.Point(313, 374);
			this.listFields.MultiColumn = true;
			this.listFields.Name = "listFields";
			this.listFields.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listFields.Size = new System.Drawing.Size(559, 279);
			this.listFields.TabIndex = 0;
			this.listFields.TabStop = false;
			this.listFields.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listFields_MouseClick);
			// 
			// textFieldValue
			// 
			this.textFieldValue.AcceptsReturn = true;
			this.textFieldValue.AcceptsTab = true;
			this.textFieldValue.Location = new System.Drawing.Point(18, 17);
			this.textFieldValue.Multiline = true;
			this.textFieldValue.Name = "textFieldValue";
			this.textFieldValue.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textFieldValue.Size = new System.Drawing.Size(854, 335);
			this.textFieldValue.TabIndex = 102;
			this.textFieldValue.TextChanged += new System.EventHandler(this.textFieldValue_TextChanged);
			this.textFieldValue.Leave += new System.EventHandler(this.textFieldValue_Leave);
			// 
			// comboGrowthBehavior
			// 
			this.comboGrowthBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboGrowthBehavior.FormattingEnabled = true;
			this.comboGrowthBehavior.Location = new System.Drawing.Point(101, 508);
			this.comboGrowthBehavior.Name = "comboGrowthBehavior";
			this.comboGrowthBehavior.Size = new System.Drawing.Size(197, 21);
			this.comboGrowthBehavior.TabIndex = 99;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(-5, 509);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(107, 16);
			this.label9.TabIndex = 98;
			this.label9.Text = "Growth Behavior";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(31, 615);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(71, 16);
			this.label8.TabIndex = 96;
			this.label8.Text = "Height";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(31, 589);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(71, 16);
			this.label7.TabIndex = 94;
			this.label7.Text = "Width";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(31, 563);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(71, 16);
			this.label6.TabIndex = 92;
			this.label6.Text = "Y Pos";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(31, 537);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(71, 16);
			this.label5.TabIndex = 90;
			this.label5.Text = "X Pos";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.butColor);
			this.groupBox1.Controls.Add(this.comboTextAlign);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.checkFontIsBold);
			this.groupBox1.Controls.Add(this.textFontSize);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.comboFontName);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Location = new System.Drawing.Point(18, 361);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(287, 141);
			this.groupBox1.TabIndex = 88;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Font";
			// 
			// label10
			// 
			this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label10.Location = new System.Drawing.Point(10, 90);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(71, 16);
			this.label10.TabIndex = 240;
			this.label10.Text = "Color";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butColor
			// 
			this.butColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColor.Location = new System.Drawing.Point(83, 88);
			this.butColor.Name = "butColor";
			this.butColor.Size = new System.Drawing.Size(30, 20);
			this.butColor.TabIndex = 239;
			this.butColor.Click += new System.EventHandler(this.butColor_Click);
			// 
			// comboTextAlign
			// 
			this.comboTextAlign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTextAlign.FormattingEnabled = true;
			this.comboTextAlign.Location = new System.Drawing.Point(83, 114);
			this.comboTextAlign.Name = "comboTextAlign";
			this.comboTextAlign.Size = new System.Drawing.Size(197, 21);
			this.comboTextAlign.TabIndex = 107;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 115);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(78, 16);
			this.label2.TabIndex = 106;
			this.label2.Text = "Align";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkFontIsBold
			// 
			this.checkFontIsBold.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkFontIsBold.Location = new System.Drawing.Point(13, 66);
			this.checkFontIsBold.Name = "checkFontIsBold";
			this.checkFontIsBold.Size = new System.Drawing.Size(85, 20);
			this.checkFontIsBold.TabIndex = 90;
			this.checkFontIsBold.Text = "Bold";
			this.checkFontIsBold.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkFontIsBold.UseVisualStyleBackColor = true;
			// 
			// textFontSize
			// 
			this.textFontSize.Location = new System.Drawing.Point(83, 41);
			this.textFontSize.Name = "textFontSize";
			this.textFontSize.Size = new System.Drawing.Size(44, 20);
			this.textFontSize.TabIndex = 89;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(10, 42);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(71, 16);
			this.label4.TabIndex = 89;
			this.label4.Text = "Size";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboFontName
			// 
			this.comboFontName.FormattingEnabled = true;
			this.comboFontName.Location = new System.Drawing.Point(83, 14);
			this.comboFontName.Name = "comboFontName";
			this.comboFontName.Size = new System.Drawing.Size(197, 21);
			this.comboFontName.TabIndex = 88;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(10, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(71, 16);
			this.label3.TabIndex = 87;
			this.label3.Text = "Name";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkIsLocked
			// 
			this.checkIsLocked.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsLocked.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsLocked.Location = new System.Drawing.Point(196, 639);
			this.checkIsLocked.Name = "checkIsLocked";
			this.checkIsLocked.Size = new System.Drawing.Size(109, 20);
			this.checkIsLocked.TabIndex = 237;
			this.checkIsLocked.Text = "Lock Text Editing";
			this.checkIsLocked.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butExamSheet
			// 
			this.butExamSheet.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butExamSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butExamSheet.Autosize = true;
			this.butExamSheet.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butExamSheet.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butExamSheet.CornerRadius = 4F;
			this.butExamSheet.Location = new System.Drawing.Point(313, 659);
			this.butExamSheet.Name = "butExamSheet";
			this.butExamSheet.Size = new System.Drawing.Size(98, 24);
			this.butExamSheet.TabIndex = 105;
			this.butExamSheet.Text = "Exam Sheet Field";
			this.butExamSheet.Click += new System.EventHandler(this.butExamSheet_Click);
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
			this.butDelete.Location = new System.Drawing.Point(15, 659);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(77, 24);
			this.butDelete.TabIndex = 100;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textHeight
			// 
			this.textHeight.Location = new System.Drawing.Point(101, 614);
			this.textHeight.MaxVal = 2000;
			this.textHeight.MinVal = -100;
			this.textHeight.Name = "textHeight";
			this.textHeight.Size = new System.Drawing.Size(69, 20);
			this.textHeight.TabIndex = 97;
			// 
			// textWidth
			// 
			this.textWidth.Location = new System.Drawing.Point(101, 588);
			this.textWidth.MaxVal = 2000;
			this.textWidth.MinVal = -100;
			this.textWidth.Name = "textWidth";
			this.textWidth.Size = new System.Drawing.Size(69, 20);
			this.textWidth.TabIndex = 95;
			// 
			// textYPos
			// 
			this.textYPos.Location = new System.Drawing.Point(101, 562);
			this.textYPos.MaxVal = 2000;
			this.textYPos.MinVal = -100;
			this.textYPos.Name = "textYPos";
			this.textYPos.Size = new System.Drawing.Size(69, 20);
			this.textYPos.TabIndex = 93;
			// 
			// textXPos
			// 
			this.textXPos.Location = new System.Drawing.Point(101, 536);
			this.textXPos.MaxVal = 2000;
			this.textXPos.MinVal = -100;
			this.textXPos.Name = "textXPos";
			this.textXPos.Size = new System.Drawing.Size(69, 20);
			this.textXPos.TabIndex = 91;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(707, 659);
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
			this.butCancel.Location = new System.Drawing.Point(797, 659);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormSheetFieldStatic
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(884, 691);
			this.Controls.Add(this.checkIsLocked);
			this.Controls.Add(this.checkPmtOpt);
			this.Controls.Add(this.butExamSheet);
			this.Controls.Add(this.labelTextW);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listFields);
			this.Controls.Add(this.textFieldValue);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.comboGrowthBehavior);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.textHeight);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.textWidth);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textYPos);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textXPos);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormSheetFieldStatic";
			this.Text = "Edit Static Text Field";
			this.Load += new System.EventHandler(this.FormSheetFieldStatic_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox comboFontName;
		private System.Windows.Forms.CheckBox checkFontIsBold;
		private System.Windows.Forms.TextBox textFontSize;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private ValidNum textXPos;
		private ValidNum textYPos;
		private System.Windows.Forms.Label label6;
		private ValidNum textWidth;
		private System.Windows.Forms.Label label7;
		private ValidNum textHeight;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox comboGrowthBehavior;
		private System.Windows.Forms.Label label9;
		private OpenDental.UI.Button butDelete;
		private System.Windows.Forms.TextBox textFieldValue;
		private System.Windows.Forms.ListBox listFields;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelTextW;
		private UI.Button butExamSheet;
		private System.Windows.Forms.ComboBox comboTextAlign;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox checkPmtOpt;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Button butColor;
		private System.Windows.Forms.CheckBox checkIsLocked;
	}
}