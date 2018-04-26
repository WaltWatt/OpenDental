namespace OpenDental{
	partial class FormRpTreatPlanPresentationStatistics {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpTreatPlanPresentationStatistics));
			this.checkAllUsers = new System.Windows.Forms.CheckBox();
			this.labelClin = new System.Windows.Forms.Label();
			this.checkAllClinics = new System.Windows.Forms.CheckBox();
			this.listClin = new System.Windows.Forms.ListBox();
			this.listUser = new System.Windows.Forms.ListBox();
			this.labelUsers = new System.Windows.Forms.Label();
			this.groupOrder = new System.Windows.Forms.GroupBox();
			this.radioFirstPresented = new System.Windows.Forms.RadioButton();
			this.radioLastPresented = new System.Windows.Forms.RadioButton();
			this.groupGrossNet = new System.Windows.Forms.GroupBox();
			this.radioGross = new System.Windows.Forms.RadioButton();
			this.radioNet = new System.Windows.Forms.RadioButton();
			this.date2 = new System.Windows.Forms.MonthCalendar();
			this.date1 = new System.Windows.Forms.MonthCalendar();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupUser = new System.Windows.Forms.GroupBox();
			this.radioPresenter = new System.Windows.Forms.RadioButton();
			this.radioEntryUser = new System.Windows.Forms.RadioButton();
			this.groupOrder.SuspendLayout();
			this.groupGrossNet.SuspendLayout();
			this.groupUser.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkAllUsers
			// 
			this.checkAllUsers.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllUsers.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllUsers.Location = new System.Drawing.Point(17, 203);
			this.checkAllUsers.Name = "checkAllUsers";
			this.checkAllUsers.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkAllUsers.Size = new System.Drawing.Size(118, 16);
			this.checkAllUsers.TabIndex = 65;
			this.checkAllUsers.Text = "All";
			this.checkAllUsers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllUsers.Click += new System.EventHandler(this.checkAllUsers_Click);
			// 
			// labelClin
			// 
			this.labelClin.Location = new System.Drawing.Point(142, 185);
			this.labelClin.Name = "labelClin";
			this.labelClin.Size = new System.Drawing.Size(136, 16);
			this.labelClin.TabIndex = 64;
			this.labelClin.Text = "Clinics";
			this.labelClin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkAllClinics
			// 
			this.checkAllClinics.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllClinics.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllClinics.Location = new System.Drawing.Point(146, 203);
			this.checkAllClinics.Name = "checkAllClinics";
			this.checkAllClinics.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkAllClinics.Size = new System.Drawing.Size(132, 16);
			this.checkAllClinics.TabIndex = 63;
			this.checkAllClinics.Text = "All";
			this.checkAllClinics.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllClinics.Click += new System.EventHandler(this.checkAllClinics_Click);
			// 
			// listClin
			// 
			this.listClin.Location = new System.Drawing.Point(145, 221);
			this.listClin.Name = "listClin";
			this.listClin.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listClin.Size = new System.Drawing.Size(133, 225);
			this.listClin.TabIndex = 62;
			this.listClin.Click += new System.EventHandler(this.listClin_Click);
			// 
			// listUser
			// 
			this.listUser.Location = new System.Drawing.Point(16, 221);
			this.listUser.Name = "listUser";
			this.listUser.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listUser.Size = new System.Drawing.Size(120, 225);
			this.listUser.TabIndex = 61;
			this.listUser.Click += new System.EventHandler(this.listUser_Click);
			// 
			// labelUsers
			// 
			this.labelUsers.Location = new System.Drawing.Point(14, 185);
			this.labelUsers.Name = "labelUsers";
			this.labelUsers.Size = new System.Drawing.Size(121, 16);
			this.labelUsers.TabIndex = 60;
			this.labelUsers.Text = "Users";
			this.labelUsers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupOrder
			// 
			this.groupOrder.Controls.Add(this.radioFirstPresented);
			this.groupOrder.Controls.Add(this.radioLastPresented);
			this.groupOrder.Location = new System.Drawing.Point(292, 258);
			this.groupOrder.Name = "groupOrder";
			this.groupOrder.Size = new System.Drawing.Size(185, 64);
			this.groupOrder.TabIndex = 34;
			this.groupOrder.TabStop = false;
			this.groupOrder.Text = "Order Presented";
			// 
			// radioFirstPresented
			// 
			this.radioFirstPresented.Checked = true;
			this.radioFirstPresented.Location = new System.Drawing.Point(6, 16);
			this.radioFirstPresented.Name = "radioFirstPresented";
			this.radioFirstPresented.Size = new System.Drawing.Size(174, 20);
			this.radioFirstPresented.TabIndex = 6;
			this.radioFirstPresented.TabStop = true;
			this.radioFirstPresented.Text = "First Presented";
			this.radioFirstPresented.UseVisualStyleBackColor = true;
			// 
			// radioLastPresented
			// 
			this.radioLastPresented.Location = new System.Drawing.Point(6, 40);
			this.radioLastPresented.Name = "radioLastPresented";
			this.radioLastPresented.Size = new System.Drawing.Size(174, 20);
			this.radioLastPresented.TabIndex = 7;
			this.radioLastPresented.Text = "Last Presented";
			this.radioLastPresented.UseVisualStyleBackColor = true;
			// 
			// groupGrossNet
			// 
			this.groupGrossNet.Controls.Add(this.radioGross);
			this.groupGrossNet.Controls.Add(this.radioNet);
			this.groupGrossNet.Location = new System.Drawing.Point(292, 189);
			this.groupGrossNet.Name = "groupGrossNet";
			this.groupGrossNet.Size = new System.Drawing.Size(185, 64);
			this.groupGrossNet.TabIndex = 33;
			this.groupGrossNet.TabStop = false;
			this.groupGrossNet.Text = "Calculated Production";
			// 
			// radioGross
			// 
			this.radioGross.Checked = true;
			this.radioGross.Location = new System.Drawing.Point(6, 16);
			this.radioGross.Name = "radioGross";
			this.radioGross.Size = new System.Drawing.Size(174, 20);
			this.radioGross.TabIndex = 6;
			this.radioGross.TabStop = true;
			this.radioGross.Text = "Gross Production";
			this.radioGross.UseVisualStyleBackColor = true;
			// 
			// radioNet
			// 
			this.radioNet.Location = new System.Drawing.Point(6, 40);
			this.radioNet.Name = "radioNet";
			this.radioNet.Size = new System.Drawing.Size(174, 20);
			this.radioNet.TabIndex = 7;
			this.radioNet.Text = "Net Production";
			this.radioNet.UseVisualStyleBackColor = true;
			// 
			// date2
			// 
			this.date2.Location = new System.Drawing.Point(250, 18);
			this.date2.MaxSelectionCount = 1;
			this.date2.Name = "date2";
			this.date2.TabIndex = 32;
			// 
			// date1
			// 
			this.date1.Location = new System.Drawing.Point(16, 18);
			this.date1.MaxSelectionCount = 1;
			this.date1.Name = "date1";
			this.date1.TabIndex = 31;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(403, 396);
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
			this.butCancel.Location = new System.Drawing.Point(403, 426);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// groupUser
			// 
			this.groupUser.Controls.Add(this.radioPresenter);
			this.groupUser.Controls.Add(this.radioEntryUser);
			this.groupUser.Location = new System.Drawing.Point(292, 327);
			this.groupUser.Name = "groupUser";
			this.groupUser.Size = new System.Drawing.Size(185, 64);
			this.groupUser.TabIndex = 35;
			this.groupUser.TabStop = false;
			this.groupUser.Text = "User Displayed";
			// 
			// radioPresenter
			// 
			this.radioPresenter.Checked = true;
			this.radioPresenter.Location = new System.Drawing.Point(6, 16);
			this.radioPresenter.Name = "radioPresenter";
			this.radioPresenter.Size = new System.Drawing.Size(174, 20);
			this.radioPresenter.TabIndex = 6;
			this.radioPresenter.TabStop = true;
			this.radioPresenter.Text = "Presenter";
			this.radioPresenter.UseVisualStyleBackColor = true;
			// 
			// radioEntryUser
			// 
			this.radioEntryUser.Location = new System.Drawing.Point(6, 40);
			this.radioEntryUser.Name = "radioEntryUser";
			this.radioEntryUser.Size = new System.Drawing.Size(174, 20);
			this.radioEntryUser.TabIndex = 7;
			this.radioEntryUser.Text = "Entry User";
			this.radioEntryUser.UseVisualStyleBackColor = true;
			// 
			// FormRpTreatPlanPresentationStatistics
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(490, 462);
			this.Controls.Add(this.groupUser);
			this.Controls.Add(this.checkAllUsers);
			this.Controls.Add(this.labelClin);
			this.Controls.Add(this.checkAllClinics);
			this.Controls.Add(this.listClin);
			this.Controls.Add(this.listUser);
			this.Controls.Add(this.labelUsers);
			this.Controls.Add(this.groupOrder);
			this.Controls.Add(this.groupGrossNet);
			this.Controls.Add(this.date2);
			this.Controls.Add(this.date1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormRpTreatPlanPresentationStatistics";
			this.Text = "Treatment Plan Presentation Statistics";
			this.Load += new System.EventHandler(this.FormRpTreatPlanPresenter_Load);
			this.groupOrder.ResumeLayout(false);
			this.groupGrossNet.ResumeLayout(false);
			this.groupUser.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.RadioButton radioGross;
		private System.Windows.Forms.RadioButton radioNet;
		private System.Windows.Forms.MonthCalendar date2;
		private System.Windows.Forms.MonthCalendar date1;
		private System.Windows.Forms.GroupBox groupGrossNet;
		private System.Windows.Forms.GroupBox groupOrder;
		private System.Windows.Forms.RadioButton radioFirstPresented;
		private System.Windows.Forms.RadioButton radioLastPresented;
		private System.Windows.Forms.CheckBox checkAllUsers;
		private System.Windows.Forms.Label labelClin;
		private System.Windows.Forms.CheckBox checkAllClinics;
		private System.Windows.Forms.ListBox listClin;
		private System.Windows.Forms.ListBox listUser;
		private System.Windows.Forms.Label labelUsers;
		private System.Windows.Forms.GroupBox groupUser;
		private System.Windows.Forms.RadioButton radioPresenter;
		private System.Windows.Forms.RadioButton radioEntryUser;
	}
}