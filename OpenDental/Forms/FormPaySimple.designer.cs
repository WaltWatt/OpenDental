namespace OpenDental{
	partial class FormPaySimple {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPaySimple));
			this.butCancel = new OpenDental.UI.Button();
			this.checkOneTimePayment = new System.Windows.Forms.CheckBox();
			this.labelRefNumber = new System.Windows.Forms.Label();
			this.textRefNumber = new System.Windows.Forms.TextBox();
			this.groupTransType = new System.Windows.Forms.GroupBox();
			this.radioSale = new System.Windows.Forms.RadioButton();
			this.radioReturn = new System.Windows.Forms.RadioButton();
			this.radioAuthorization = new System.Windows.Forms.RadioButton();
			this.radioVoid = new System.Windows.Forms.RadioButton();
			this.textZipCode = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.textSecurityCode = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textNameOnCard = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textExpDate = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textCardNumber = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textAmount = new System.Windows.Forms.TextBox();
			this.labelAmount = new System.Windows.Forms.Label();
			this.butOK = new OpenDental.UI.Button();
			this.pd2 = new System.Drawing.Printing.PrintDocument();
			this.groupTransType.SuspendLayout();
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
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(318, 276);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// checkOneTimePayment
			// 
			this.checkOneTimePayment.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkOneTimePayment.Location = new System.Drawing.Point(24, 206);
			this.checkOneTimePayment.Name = "checkOneTimePayment";
			this.checkOneTimePayment.Size = new System.Drawing.Size(150, 17);
			this.checkOneTimePayment.TabIndex = 38;
			this.checkOneTimePayment.Text = "One-Time Payment";
			// 
			// labelRefNumber
			// 
			this.labelRefNumber.Location = new System.Drawing.Point(275, 80);
			this.labelRefNumber.Name = "labelRefNumber";
			this.labelRefNumber.Size = new System.Drawing.Size(117, 16);
			this.labelRefNumber.TabIndex = 28;
			this.labelRefNumber.Text = "Ref Number";
			this.labelRefNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelRefNumber.Visible = false;
			// 
			// textRefNumber
			// 
			this.textRefNumber.Location = new System.Drawing.Point(275, 97);
			this.textRefNumber.Name = "textRefNumber";
			this.textRefNumber.Size = new System.Drawing.Size(118, 20);
			this.textRefNumber.TabIndex = 37;
			this.textRefNumber.Visible = false;
			// 
			// groupTransType
			// 
			this.groupTransType.Controls.Add(this.radioSale);
			this.groupTransType.Controls.Add(this.radioReturn);
			this.groupTransType.Controls.Add(this.radioAuthorization);
			this.groupTransType.Controls.Add(this.radioVoid);
			this.groupTransType.Location = new System.Drawing.Point(24, 12);
			this.groupTransType.Name = "groupTransType";
			this.groupTransType.Size = new System.Drawing.Size(228, 50);
			this.groupTransType.TabIndex = 25;
			this.groupTransType.TabStop = false;
			this.groupTransType.Text = "Transaction Type";
			// 
			// radioSale
			// 
			this.radioSale.AutoSize = true;
			this.radioSale.Checked = true;
			this.radioSale.Location = new System.Drawing.Point(5, 19);
			this.radioSale.Name = "radioSale";
			this.radioSale.Size = new System.Drawing.Size(46, 17);
			this.radioSale.TabIndex = 0;
			this.radioSale.TabStop = true;
			this.radioSale.Text = "Sale";
			this.radioSale.UseVisualStyleBackColor = true;
			this.radioSale.Click += new System.EventHandler(this.radioSale_Click);
			// 
			// radioReturn
			// 
			this.radioReturn.AutoSize = true;
			this.radioReturn.Location = new System.Drawing.Point(156, 19);
			this.radioReturn.Name = "radioReturn";
			this.radioReturn.Size = new System.Drawing.Size(57, 17);
			this.radioReturn.TabIndex = 0;
			this.radioReturn.Text = "Return";
			this.radioReturn.UseVisualStyleBackColor = true;
			this.radioReturn.Click += new System.EventHandler(this.radioReturn_Click);
			// 
			// radioAuthorization
			// 
			this.radioAuthorization.AutoSize = true;
			this.radioAuthorization.Location = new System.Drawing.Point(55, 19);
			this.radioAuthorization.Name = "radioAuthorization";
			this.radioAuthorization.Size = new System.Drawing.Size(47, 17);
			this.radioAuthorization.TabIndex = 0;
			this.radioAuthorization.Text = "Auth";
			this.radioAuthorization.UseVisualStyleBackColor = true;
			this.radioAuthorization.Click += new System.EventHandler(this.radioAuthorization_Click);
			// 
			// radioVoid
			// 
			this.radioVoid.AutoSize = true;
			this.radioVoid.Location = new System.Drawing.Point(106, 19);
			this.radioVoid.Name = "radioVoid";
			this.radioVoid.Size = new System.Drawing.Size(46, 17);
			this.radioVoid.TabIndex = 0;
			this.radioVoid.Text = "Void";
			this.radioVoid.UseVisualStyleBackColor = true;
			this.radioVoid.Click += new System.EventHandler(this.radioVoid_Click);
			// 
			// textZipCode
			// 
			this.textZipCode.Location = new System.Drawing.Point(275, 183);
			this.textZipCode.Name = "textZipCode";
			this.textZipCode.Size = new System.Drawing.Size(118, 20);
			this.textZipCode.TabIndex = 35;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(275, 166);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(117, 16);
			this.label7.TabIndex = 20;
			this.label7.Text = "Zip Code";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textSecurityCode
			// 
			this.textSecurityCode.Location = new System.Drawing.Point(275, 140);
			this.textSecurityCode.Name = "textSecurityCode";
			this.textSecurityCode.Size = new System.Drawing.Size(118, 20);
			this.textSecurityCode.TabIndex = 34;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(275, 123);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(117, 16);
			this.label6.TabIndex = 24;
			this.label6.Text = "Security Code";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textNameOnCard
			// 
			this.textNameOnCard.Location = new System.Drawing.Point(24, 183);
			this.textNameOnCard.Name = "textNameOnCard";
			this.textNameOnCard.Size = new System.Drawing.Size(228, 20);
			this.textNameOnCard.TabIndex = 33;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(24, 166);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(150, 16);
			this.label5.TabIndex = 23;
			this.label5.Text = "Name On Card";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textExpDate
			// 
			this.textExpDate.Location = new System.Drawing.Point(24, 140);
			this.textExpDate.Name = "textExpDate";
			this.textExpDate.Size = new System.Drawing.Size(150, 20);
			this.textExpDate.TabIndex = 32;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(24, 123);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(150, 16);
			this.label4.TabIndex = 22;
			this.label4.Text = "Expiration (MMYY)";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textCardNumber
			// 
			this.textCardNumber.Location = new System.Drawing.Point(24, 97);
			this.textCardNumber.Name = "textCardNumber";
			this.textCardNumber.Size = new System.Drawing.Size(228, 20);
			this.textCardNumber.TabIndex = 30;
			this.textCardNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textCardNumber_KeyPress);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(24, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(150, 16);
			this.label3.TabIndex = 21;
			this.label3.Text = "Card Number";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textAmount
			// 
			this.textAmount.Location = new System.Drawing.Point(275, 226);
			this.textAmount.Name = "textAmount";
			this.textAmount.Size = new System.Drawing.Size(118, 20);
			this.textAmount.TabIndex = 36;
			// 
			// labelAmount
			// 
			this.labelAmount.Location = new System.Drawing.Point(275, 209);
			this.labelAmount.Name = "labelAmount";
			this.labelAmount.Size = new System.Drawing.Size(117, 16);
			this.labelAmount.TabIndex = 26;
			this.labelAmount.Text = "Amount";
			this.labelAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(227, 276);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// FormPaySimple
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(422, 312);
			this.Controls.Add(this.checkOneTimePayment);
			this.Controls.Add(this.labelRefNumber);
			this.Controls.Add(this.textRefNumber);
			this.Controls.Add(this.groupTransType);
			this.Controls.Add(this.textZipCode);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textSecurityCode);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textNameOnCard);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textExpDate);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textCardNumber);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textAmount);
			this.Controls.Add(this.labelAmount);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(438, 351);
			this.Name = "FormPaySimple";
			this.Text = "PaySimple Payment Information";
			this.Load += new System.EventHandler(this.FormPaySimple_Load);
			this.groupTransType.ResumeLayout(false);
			this.groupTransType.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.CheckBox checkOneTimePayment;
		private System.Windows.Forms.Label labelRefNumber;
		private System.Windows.Forms.TextBox textRefNumber;
		private System.Windows.Forms.GroupBox groupTransType;
		private System.Windows.Forms.RadioButton radioSale;
		private System.Windows.Forms.RadioButton radioReturn;
		private System.Windows.Forms.RadioButton radioVoid;
		private System.Windows.Forms.TextBox textZipCode;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textSecurityCode;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textNameOnCard;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textExpDate;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textCardNumber;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textAmount;
		private System.Windows.Forms.Label labelAmount;
		private System.Windows.Forms.RadioButton radioAuthorization;
		private System.Drawing.Printing.PrintDocument pd2;
	}
}