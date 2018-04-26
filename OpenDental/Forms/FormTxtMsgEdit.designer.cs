namespace OpenDental{
	partial class FormTxtMsgEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTxtMsgEdit));
			this.textWirelessPhone = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.radioPatient = new System.Windows.Forms.RadioButton();
			this.radioOther = new System.Windows.Forms.RadioButton();
			this.groupRecipient = new System.Windows.Forms.GroupBox();
			this.textPatient = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.butPatFind = new OpenDental.UI.Button();
			this.textMessage = new OpenDental.ODtextBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupRecipient.SuspendLayout();
			this.SuspendLayout();
			// 
			// textWirelessPhone
			// 
			this.textWirelessPhone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textWirelessPhone.Location = new System.Drawing.Point(219, 91);
			this.textWirelessPhone.Name = "textWirelessPhone";
			this.textWirelessPhone.Size = new System.Drawing.Size(140, 20);
			this.textWirelessPhone.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(216, 68);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(131, 20);
			this.label1.TabIndex = 6;
			this.label1.Text = "Wireless Phone Number";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(25, 158);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(86, 20);
			this.label2.TabIndex = 6;
			this.label2.Text = "Text Message";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// radioPatient
			// 
			this.radioPatient.Location = new System.Drawing.Point(6, 18);
			this.radioPatient.Name = "radioPatient";
			this.radioPatient.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.radioPatient.Size = new System.Drawing.Size(120, 17);
			this.radioPatient.TabIndex = 1;
			this.radioPatient.TabStop = true;
			this.radioPatient.Text = "Patient";
			this.radioPatient.UseVisualStyleBackColor = true;
			this.radioPatient.Click += new System.EventHandler(this.radioPatient_Click);
			// 
			// radioOther
			// 
			this.radioOther.Location = new System.Drawing.Point(132, 18);
			this.radioOther.Name = "radioOther";
			this.radioOther.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.radioOther.Size = new System.Drawing.Size(120, 17);
			this.radioOther.TabIndex = 2;
			this.radioOther.TabStop = true;
			this.radioOther.Text = "Another Person";
			this.radioOther.UseVisualStyleBackColor = true;
			this.radioOther.Click += new System.EventHandler(this.radioOther_Click);
			// 
			// groupRecipient
			// 
			this.groupRecipient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupRecipient.Controls.Add(this.radioPatient);
			this.groupRecipient.Controls.Add(this.radioOther);
			this.groupRecipient.Location = new System.Drawing.Point(28, 13);
			this.groupRecipient.Name = "groupRecipient";
			this.groupRecipient.Size = new System.Drawing.Size(331, 43);
			this.groupRecipient.TabIndex = 9;
			this.groupRecipient.TabStop = false;
			this.groupRecipient.Text = "Choose one of the following options:";
			// 
			// textPatient
			// 
			this.textPatient.Location = new System.Drawing.Point(28, 91);
			this.textPatient.Name = "textPatient";
			this.textPatient.ReadOnly = true;
			this.textPatient.Size = new System.Drawing.Size(185, 20);
			this.textPatient.TabIndex = 3;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(25, 67);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(70, 20);
			this.label4.TabIndex = 163;
			this.label4.Text = "Patient";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butPatFind
			// 
			this.butPatFind.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPatFind.Autosize = true;
			this.butPatFind.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPatFind.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPatFind.CornerRadius = 4F;
			this.butPatFind.Location = new System.Drawing.Point(28, 117);
			this.butPatFind.Name = "butPatFind";
			this.butPatFind.Size = new System.Drawing.Size(63, 24);
			this.butPatFind.TabIndex = 5;
			this.butPatFind.Text = "Find";
			this.butPatFind.Click += new System.EventHandler(this.butPatFind_Click);
			// 
			// textMessage
			// 
			this.textMessage.AcceptsTab = true;
			this.textMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textMessage.DetectUrls = false;
			this.textMessage.Location = new System.Drawing.Point(28, 181);
			this.textMessage.Name = "textMessage";
			this.textMessage.QuickPasteType = OpenDentBusiness.QuickPasteType.TxtMsg;
			this.textMessage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textMessage.Size = new System.Drawing.Size(331, 113);
			this.textMessage.TabIndex = 6;
			this.textMessage.Text = "";
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(209, 303);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 7;
			this.butOK.Text = "&Send";
			this.butOK.Click += new System.EventHandler(this.butSend_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(290, 303);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 8;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormTxtMsgEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(377, 339);
			this.Controls.Add(this.butPatFind);
			this.Controls.Add(this.groupRecipient);
			this.Controls.Add(this.textPatient);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textMessage);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.textWirelessPhone);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(393, 378);
			this.Name = "FormTxtMsgEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Text Message";
			this.Load += new System.EventHandler(this.FormTxtMsgEdit_Load);
			this.groupRecipient.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private ODtextBox textMessage;
		private System.Windows.Forms.TextBox textWirelessPhone;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RadioButton radioPatient;
		private System.Windows.Forms.RadioButton radioOther;
		private System.Windows.Forms.GroupBox groupRecipient;
		private System.Windows.Forms.TextBox textPatient;
		private System.Windows.Forms.Label label4;
		private UI.Button butPatFind;
	}
}