namespace OpenDental{
	partial class FormSheetFillEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSheetFillEdit));
			this.textNote = new System.Windows.Forms.TextBox();
			this.labelNote = new System.Windows.Forms.Label();
			this.labelDateTime = new System.Windows.Forms.Label();
			this.panelScroll = new System.Windows.Forms.Panel();
			this.panelMain = new System.Windows.Forms.Panel();
			this.checkErase = new System.Windows.Forms.CheckBox();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.labelDescription = new System.Windows.Forms.Label();
			this.labelShowInTerminal = new System.Windows.Forms.Label();
			this.textDateTime = new System.Windows.Forms.TextBox();
			this.butUnlock = new OpenDental.UI.Button();
			this.textShowInTerminal = new OpenDental.ValidNumber();
			this.butDelete = new OpenDental.UI.Button();
			this.butPrint = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butChangePat = new OpenDental.UI.Button();
			this.butRestore = new OpenDental.UI.Button();
			this.butSaveSignature = new OpenDental.UI.Button();
			this.butSimplePrint = new OpenDental.UI.Button();
			this.butEmail = new OpenDental.UI.Button();
			this.butToKiosk = new OpenDental.UI.Button();
			this.butPDF = new OpenDental.UI.Button();
			this.panelScroll.SuspendLayout();
			this.SuspendLayout();
			// 
			// textNote
			// 
			this.textNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textNote.Location = new System.Drawing.Point(163, 608);
			this.textNote.Multiline = true;
			this.textNote.Name = "textNote";
			this.textNote.Size = new System.Drawing.Size(146, 90);
			this.textNote.TabIndex = 6;
			this.textNote.TabStop = false;
			// 
			// labelNote
			// 
			this.labelNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelNote.Location = new System.Drawing.Point(164, 590);
			this.labelNote.Name = "labelNote";
			this.labelNote.Size = new System.Drawing.Size(147, 16);
			this.labelNote.TabIndex = 7;
			this.labelNote.Text = "Internal Note";
			this.labelNote.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelDateTime
			// 
			this.labelDateTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelDateTime.Location = new System.Drawing.Point(10, 589);
			this.labelDateTime.Name = "labelDateTime";
			this.labelDateTime.Size = new System.Drawing.Size(84, 16);
			this.labelDateTime.TabIndex = 76;
			this.labelDateTime.Text = "Date time";
			this.labelDateTime.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// panelScroll
			// 
			this.panelScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelScroll.AutoScroll = true;
			this.panelScroll.Controls.Add(this.panelMain);
			this.panelScroll.Location = new System.Drawing.Point(3, 3);
			this.panelScroll.Name = "panelScroll";
			this.panelScroll.Size = new System.Drawing.Size(716, 580);
			this.panelScroll.TabIndex = 1;
			this.panelScroll.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelScroll_MouseUp);
			// 
			// panelMain
			// 
			this.panelMain.BackColor = System.Drawing.SystemColors.Window;
			this.panelMain.Location = new System.Drawing.Point(0, 0);
			this.panelMain.Name = "panelMain";
			this.panelMain.Size = new System.Drawing.Size(560, 494);
			this.panelMain.TabIndex = 0;
			// 
			// checkErase
			// 
			this.checkErase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkErase.Location = new System.Drawing.Point(321, 618);
			this.checkErase.Name = "checkErase";
			this.checkErase.Size = new System.Drawing.Size(89, 20);
			this.checkErase.TabIndex = 81;
			this.checkErase.TabStop = false;
			this.checkErase.Text = "Eraser Tool";
			this.checkErase.UseVisualStyleBackColor = true;
			this.checkErase.Click += new System.EventHandler(this.checkErase_Click);
			// 
			// textDescription
			// 
			this.textDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textDescription.Location = new System.Drawing.Point(11, 649);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(146, 20);
			this.textDescription.TabIndex = 84;
			this.textDescription.TabStop = false;
			// 
			// labelDescription
			// 
			this.labelDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelDescription.Location = new System.Drawing.Point(10, 630);
			this.labelDescription.Name = "labelDescription";
			this.labelDescription.Size = new System.Drawing.Size(147, 16);
			this.labelDescription.TabIndex = 85;
			this.labelDescription.Text = "Description";
			this.labelDescription.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelShowInTerminal
			// 
			this.labelShowInTerminal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelShowInTerminal.Location = new System.Drawing.Point(10, 679);
			this.labelShowInTerminal.Name = "labelShowInTerminal";
			this.labelShowInTerminal.Size = new System.Drawing.Size(120, 16);
			this.labelShowInTerminal.TabIndex = 86;
			this.labelShowInTerminal.Text = "Show Order In Kiosk";
			this.labelShowInTerminal.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textDateTime
			// 
			this.textDateTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textDateTime.Location = new System.Drawing.Point(13, 608);
			this.textDateTime.Name = "textDateTime";
			this.textDateTime.Size = new System.Drawing.Size(144, 20);
			this.textDateTime.TabIndex = 88;
			this.textDateTime.TabStop = false;
			// 
			// butUnlock
			// 
			this.butUnlock.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUnlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butUnlock.Autosize = true;
			this.butUnlock.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUnlock.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUnlock.CornerRadius = 4F;
			this.butUnlock.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUnlock.Location = new System.Drawing.Point(321, 646);
			this.butUnlock.Name = "butUnlock";
			this.butUnlock.Size = new System.Drawing.Size(81, 24);
			this.butUnlock.TabIndex = 89;
			this.butUnlock.TabStop = false;
			this.butUnlock.Text = "Unlock";
			this.butUnlock.Visible = false;
			this.butUnlock.Click += new System.EventHandler(this.butUnlock_Click);
			// 
			// textShowInTerminal
			// 
			this.textShowInTerminal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textShowInTerminal.Location = new System.Drawing.Point(128, 678);
			this.textShowInTerminal.MaxVal = 127;
			this.textShowInTerminal.MinVal = 1;
			this.textShowInTerminal.Name = "textShowInTerminal";
			this.textShowInTerminal.Size = new System.Drawing.Size(29, 20);
			this.textShowInTerminal.TabIndex = 87;
			this.textShowInTerminal.TabStop = false;
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(632, 588);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(81, 24);
			this.butDelete.TabIndex = 79;
			this.butDelete.TabStop = false;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(321, 676);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(81, 24);
			this.butPrint.TabIndex = 80;
			this.butPrint.TabStop = false;
			this.butPrint.Text = "Print/Email";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(632, 646);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(81, 24);
			this.butOK.TabIndex = 3;
			this.butOK.TabStop = false;
			this.butOK.Text = "OK";
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
			this.butCancel.Location = new System.Drawing.Point(632, 676);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(81, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.TabStop = false;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butChangePat
			// 
			this.butChangePat.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChangePat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butChangePat.Autosize = true;
			this.butChangePat.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChangePat.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChangePat.CornerRadius = 4F;
			this.butChangePat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butChangePat.Location = new System.Drawing.Point(521, 616);
			this.butChangePat.Name = "butChangePat";
			this.butChangePat.Size = new System.Drawing.Size(81, 24);
			this.butChangePat.TabIndex = 90;
			this.butChangePat.TabStop = false;
			this.butChangePat.Text = "Change Pat";
			this.butChangePat.UseVisualStyleBackColor = true;
			this.butChangePat.Click += new System.EventHandler(this.butChangePat_Click);
			// 
			// butRestore
			// 
			this.butRestore.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRestore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butRestore.Autosize = true;
			this.butRestore.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRestore.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRestore.CornerRadius = 4F;
			this.butRestore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butRestore.Location = new System.Drawing.Point(521, 646);
			this.butRestore.Name = "butRestore";
			this.butRestore.Size = new System.Drawing.Size(81, 24);
			this.butRestore.TabIndex = 91;
			this.butRestore.TabStop = false;
			this.butRestore.Text = "Restore";
			this.butRestore.UseVisualStyleBackColor = true;
			this.butRestore.Visible = false;
			this.butRestore.Click += new System.EventHandler(this.butRestore_Click);
			// 
			// butSaveSignature
			// 
			this.butSaveSignature.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSaveSignature.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSaveSignature.Autosize = true;
			this.butSaveSignature.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSaveSignature.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSaveSignature.CornerRadius = 4F;
			this.butSaveSignature.Location = new System.Drawing.Point(632, 617);
			this.butSaveSignature.Name = "butSaveSignature";
			this.butSaveSignature.Size = new System.Drawing.Size(81, 24);
			this.butSaveSignature.TabIndex = 92;
			this.butSaveSignature.TabStop = false;
			this.butSaveSignature.Text = "Save";
			this.butSaveSignature.Visible = false;
			this.butSaveSignature.Click += new System.EventHandler(this.butSaveSig);
			// 
			// butSimplePrint
			// 
			this.butSimplePrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSimplePrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSimplePrint.Autosize = true;
			this.butSimplePrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSimplePrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSimplePrint.CornerRadius = 4F;
			this.butSimplePrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSimplePrint.Location = new System.Drawing.Point(421, 676);
			this.butSimplePrint.Name = "butSimplePrint";
			this.butSimplePrint.Size = new System.Drawing.Size(81, 23);
			this.butSimplePrint.TabIndex = 93;
			this.butSimplePrint.TabStop = false;
			this.butSimplePrint.Text = "Print";
			this.butSimplePrint.UseVisualStyleBackColor = true;
			this.butSimplePrint.Click += new System.EventHandler(this.butSimplePrint_Click);
			// 
			// butEmail
			// 
			this.butEmail.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butEmail.Autosize = true;
			this.butEmail.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEmail.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEmail.CornerRadius = 4F;
			this.butEmail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEmail.Location = new System.Drawing.Point(521, 676);
			this.butEmail.Name = "butEmail";
			this.butEmail.Size = new System.Drawing.Size(81, 23);
			this.butEmail.TabIndex = 94;
			this.butEmail.TabStop = false;
			this.butEmail.Text = "Email";
			this.butEmail.UseVisualStyleBackColor = true;
			this.butEmail.Click += new System.EventHandler(this.butEmail_Click);
			// 
			// butToKiosk
			// 
			this.butToKiosk.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butToKiosk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butToKiosk.Autosize = true;
			this.butToKiosk.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butToKiosk.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butToKiosk.CornerRadius = 4F;
			this.butToKiosk.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.butToKiosk.Image = global::OpenDental.Properties.Resources.up;
			this.butToKiosk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butToKiosk.Location = new System.Drawing.Point(521, 589);
			this.butToKiosk.Name = "butToKiosk";
			this.butToKiosk.Size = new System.Drawing.Size(81, 23);
			this.butToKiosk.TabIndex = 95;
			this.butToKiosk.TabStop = false;
			this.butToKiosk.Text = "To Kiosk";
			this.butToKiosk.UseVisualStyleBackColor = true;
			this.butToKiosk.Click += new System.EventHandler(this.butToKiosk_Click);
			// 
			// butPDF
			// 
			this.butPDF.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPDF.Autosize = true;
			this.butPDF.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPDF.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPDF.CornerRadius = 4F;
			this.butPDF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPDF.Location = new System.Drawing.Point(421, 646);
			this.butPDF.Name = "butPDF";
			this.butPDF.Size = new System.Drawing.Size(81, 24);
			this.butPDF.TabIndex = 83;
			this.butPDF.TabStop = false;
			this.butPDF.Text = "Create PDF";
			this.butPDF.Click += new System.EventHandler(this.butPDF_Click);
			// 
			// FormSheetFillEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(724, 712);
			this.Controls.Add(this.butToKiosk);
			this.Controls.Add(this.butEmail);
			this.Controls.Add(this.butSimplePrint);
			this.Controls.Add(this.butSaveSignature);
			this.Controls.Add(this.butRestore);
			this.Controls.Add(this.butChangePat);
			this.Controls.Add(this.butUnlock);
			this.Controls.Add(this.textDateTime);
			this.Controls.Add(this.textShowInTerminal);
			this.Controls.Add(this.labelShowInTerminal);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.labelDescription);
			this.Controls.Add(this.butPDF);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.checkErase);
			this.Controls.Add(this.panelScroll);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.labelDateTime);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.labelNote);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(740, 250);
			this.Name = "FormSheetFillEdit";
			this.Text = "Fill Sheet";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSheetFillEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormSheetFillEdit_Load);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormSheetFillEdit_MouseUp);
			this.panelScroll.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.TextBox textNote;
		private System.Windows.Forms.Label labelNote;
		private System.Windows.Forms.Label labelDateTime;
		private System.Windows.Forms.Panel panelScroll;
		private System.Windows.Forms.Panel panelMain;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.Button butPrint;
		private System.Windows.Forms.CheckBox checkErase;
		private System.Windows.Forms.TextBox textDescription;
		private System.Windows.Forms.Label labelDescription;
		private System.Windows.Forms.Label labelShowInTerminal;
		private ValidNumber textShowInTerminal;
		private System.Windows.Forms.TextBox textDateTime;
		private UI.Button butUnlock;
		private UI.Button butChangePat;
		private UI.Button butRestore;
		private UI.Button butSaveSignature;
		private UI.Button butSimplePrint;
		private UI.Button butEmail;
		private UI.Button butToKiosk;
		private UI.Button butPDF;
	}
}