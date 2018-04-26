namespace OpenDental{
	partial class FormSmsTextMessaging {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSmsTextMessaging));
			this.labelClinic = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.checkHidden = new System.Windows.Forms.CheckBox();
			this.contextMenuMessages = new System.Windows.Forms.ContextMenu();
			this.menuItemChangePat = new System.Windows.Forms.MenuItem();
			this.menuItemMarkUnread = new System.Windows.Forms.MenuItem();
			this.menuItemMarkRead = new System.Windows.Forms.MenuItem();
			this.menuItemHide = new System.Windows.Forms.MenuItem();
			this.menuItemUnhide = new System.Windows.Forms.MenuItem();
			this.menuItemBlockNumber = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItemGoToPatient = new System.Windows.Forms.MenuItem();
			this.textPatient = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.checkSent = new System.Windows.Forms.CheckBox();
			this.checkRead = new System.Windows.Forms.CheckBox();
			this.butSend = new OpenDental.UI.Button();
			this.smsThreadView = new OpenDental.SmsThreadView();
			this.butPatCurrent = new OpenDental.UI.Button();
			this.butPatAll = new OpenDental.UI.Button();
			this.butPatFind = new OpenDental.UI.Button();
			this.comboClinic = new OpenDental.UI.ComboBoxMulti();
			this.butRefresh = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioGroupByNone = new System.Windows.Forms.RadioButton();
			this.radioGroupByPatient = new System.Windows.Forms.RadioButton();
			this.textReply = new OpenDental.ODtextBox();
			this.gridMessages = new OpenDental.UI.ODGrid();
			this.textDateTo = new ODR.ValidDate();
			this.textDateFrom = new ODR.ValidDate();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(568, 36);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(43, 21);
			this.labelClinic.TabIndex = 6;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelClinic.Visible = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(15, 10);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(81, 21);
			this.label2.TabIndex = 8;
			this.label2.Text = "Date From";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 31);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(84, 21);
			this.label3.TabIndex = 10;
			this.label3.Text = "Date To";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkHidden
			// 
			this.checkHidden.Location = new System.Drawing.Point(482, 8);
			this.checkHidden.Name = "checkHidden";
			this.checkHidden.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkHidden.Size = new System.Drawing.Size(80, 16);
			this.checkHidden.TabIndex = 153;
			this.checkHidden.Text = "Hidden";
			this.checkHidden.UseVisualStyleBackColor = true;
			// 
			// contextMenuMessages
			// 
			this.contextMenuMessages.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemChangePat,
            this.menuItemMarkUnread,
            this.menuItemMarkRead,
            this.menuItemHide,
            this.menuItemUnhide,
            this.menuItemBlockNumber,
            this.menuItem1,
            this.menuItemGoToPatient});
			// 
			// menuItemChangePat
			// 
			this.menuItemChangePat.Index = 0;
			this.menuItemChangePat.Text = "Change Pat";
			this.menuItemChangePat.Click += new System.EventHandler(this.menuItemChangePat_Click);
			// 
			// menuItemMarkUnread
			// 
			this.menuItemMarkUnread.Index = 1;
			this.menuItemMarkUnread.Text = "Mark Unread";
			this.menuItemMarkUnread.Click += new System.EventHandler(this.menuItemMarkUnread_Click);
			// 
			// menuItemMarkRead
			// 
			this.menuItemMarkRead.Index = 2;
			this.menuItemMarkRead.Text = "Mark Read";
			this.menuItemMarkRead.Click += new System.EventHandler(this.menuItemMarkRead_Click);
			// 
			// menuItemHide
			// 
			this.menuItemHide.Index = 3;
			this.menuItemHide.Text = "Hide";
			this.menuItemHide.Click += new System.EventHandler(this.menuItemHide_Click);
			// 
			// menuItemUnhide
			// 
			this.menuItemUnhide.Index = 4;
			this.menuItemUnhide.Text = "Unhide";
			this.menuItemUnhide.Click += new System.EventHandler(this.menuItemUnhide_Click);
			// 
			// menuItemBlockNumber
			// 
			this.menuItemBlockNumber.Index = 5;
			this.menuItemBlockNumber.Text = "Block Number";
			this.menuItemBlockNumber.Click += new System.EventHandler(this.menuItemBlockNumber_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 6;
			this.menuItem1.Text = "-";
			// 
			// menuItemGoToPatient
			// 
			this.menuItemGoToPatient.Index = 7;
			this.menuItemGoToPatient.Text = "Go to Patient";
			this.menuItemGoToPatient.Click += new System.EventHandler(this.menuItemSelectPatient_Click);
			// 
			// textPatient
			// 
			this.textPatient.Location = new System.Drawing.Point(252, 10);
			this.textPatient.Name = "textPatient";
			this.textPatient.ReadOnly = true;
			this.textPatient.Size = new System.Drawing.Size(216, 20);
			this.textPatient.TabIndex = 156;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(181, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 13);
			this.label1.TabIndex = 155;
			this.label1.Text = "Patient";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkSent
			// 
			this.checkSent.Location = new System.Drawing.Point(482, 38);
			this.checkSent.Name = "checkSent";
			this.checkSent.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkSent.Size = new System.Drawing.Size(80, 16);
			this.checkSent.TabIndex = 161;
			this.checkSent.Text = "Sent";
			this.checkSent.UseVisualStyleBackColor = true;
			// 
			// checkRead
			// 
			this.checkRead.Location = new System.Drawing.Point(482, 23);
			this.checkRead.Name = "checkRead";
			this.checkRead.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkRead.Size = new System.Drawing.Size(80, 16);
			this.checkRead.TabIndex = 162;
			this.checkRead.Text = "Received";
			this.checkRead.UseVisualStyleBackColor = true;
			// 
			// butSend
			// 
			this.butSend.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSend.Autosize = true;
			this.butSend.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSend.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSend.CornerRadius = 4F;
			this.butSend.Enabled = false;
			this.butSend.Location = new System.Drawing.Point(1024, 556);
			this.butSend.Name = "butSend";
			this.butSend.Size = new System.Drawing.Size(43, 98);
			this.butSend.TabIndex = 166;
			this.butSend.Text = "Send";
			this.butSend.Click += new System.EventHandler(this.butSend_Click);
			// 
			// smsThreadView
			// 
			this.smsThreadView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.smsThreadView.BackColor = System.Drawing.SystemColors.Control;
			this.smsThreadView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.smsThreadView.ListSmsThreadMessages = null;
			this.smsThreadView.Location = new System.Drawing.Point(817, 61);
			this.smsThreadView.Name = "smsThreadView";
			this.smsThreadView.Size = new System.Drawing.Size(250, 493);
			this.smsThreadView.TabIndex = 164;
			// 
			// butPatCurrent
			// 
			this.butPatCurrent.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPatCurrent.Autosize = true;
			this.butPatCurrent.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPatCurrent.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPatCurrent.CornerRadius = 4F;
			this.butPatCurrent.Location = new System.Drawing.Point(252, 31);
			this.butPatCurrent.Name = "butPatCurrent";
			this.butPatCurrent.Size = new System.Drawing.Size(63, 24);
			this.butPatCurrent.TabIndex = 159;
			this.butPatCurrent.Text = "Current";
			this.butPatCurrent.Click += new System.EventHandler(this.butPatCurrent_Click);
			// 
			// butPatAll
			// 
			this.butPatAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPatAll.Autosize = true;
			this.butPatAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPatAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPatAll.CornerRadius = 4F;
			this.butPatAll.Location = new System.Drawing.Point(405, 31);
			this.butPatAll.Name = "butPatAll";
			this.butPatAll.Size = new System.Drawing.Size(63, 24);
			this.butPatAll.TabIndex = 158;
			this.butPatAll.Text = "All";
			this.butPatAll.Click += new System.EventHandler(this.butPatAll_Click);
			// 
			// butPatFind
			// 
			this.butPatFind.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPatFind.Autosize = true;
			this.butPatFind.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPatFind.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPatFind.CornerRadius = 4F;
			this.butPatFind.Location = new System.Drawing.Point(328, 31);
			this.butPatFind.Name = "butPatFind";
			this.butPatFind.Size = new System.Drawing.Size(63, 24);
			this.butPatFind.TabIndex = 157;
			this.butPatFind.Text = "Find";
			this.butPatFind.Click += new System.EventHandler(this.butPatFind_Click);
			// 
			// comboClinic
			// 
			this.comboClinic.ArraySelectedIndices = new int[0];
			this.comboClinic.BackColor = System.Drawing.SystemColors.Window;
			this.comboClinic.Items = ((System.Collections.ArrayList)(resources.GetObject("comboClinic.Items")));
			this.comboClinic.Location = new System.Drawing.Point(615, 36);
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboClinic.SelectedIndices")));
			this.comboClinic.Size = new System.Drawing.Size(200, 21);
			this.comboClinic.TabIndex = 154;
			this.comboClinic.Visible = false;
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(992, 36);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 24);
			this.butRefresh.TabIndex = 13;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(992, 660);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioGroupByNone);
			this.groupBox1.Controls.Add(this.radioGroupByPatient);
			this.groupBox1.Location = new System.Drawing.Point(615, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 36);
			this.groupBox1.TabIndex = 167;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Group Messages By";
			// 
			// radioGroupByNone
			// 
			this.radioGroupByNone.Checked = true;
			this.radioGroupByNone.Location = new System.Drawing.Point(96, 14);
			this.radioGroupByNone.Name = "radioGroupByNone";
			this.radioGroupByNone.Size = new System.Drawing.Size(98, 17);
			this.radioGroupByNone.TabIndex = 1;
			this.radioGroupByNone.TabStop = true;
			this.radioGroupByNone.Text = "None";
			this.radioGroupByNone.UseVisualStyleBackColor = true;
			// 
			// radioGroupByPatient
			// 
			this.radioGroupByPatient.Location = new System.Drawing.Point(6, 14);
			this.radioGroupByPatient.Name = "radioGroupByPatient";
			this.radioGroupByPatient.Size = new System.Drawing.Size(84, 17);
			this.radioGroupByPatient.TabIndex = 0;
			this.radioGroupByPatient.TabStop = true;
			this.radioGroupByPatient.Text = "Patient";
			this.radioGroupByPatient.UseVisualStyleBackColor = true;
			// 
			// textReply
			// 
			this.textReply.AcceptsTab = true;
			this.textReply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textReply.BackColor = System.Drawing.SystemColors.Window;
			this.textReply.DetectLinksEnabled = false;
			this.textReply.DetectUrls = false;
			this.textReply.Enabled = false;
			this.textReply.Location = new System.Drawing.Point(817, 556);
			this.textReply.Name = "textReply";
			this.textReply.QuickPasteType = OpenDentBusiness.QuickPasteType.TxtMsg;
			this.textReply.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textReply.Size = new System.Drawing.Size(201, 98);
			this.textReply.TabIndex = 165;
			this.textReply.Text = "";
			// 
			// gridMessages
			// 
			this.gridMessages.AllowSortingByColumn = true;
			this.gridMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMessages.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMessages.HasAddButton = false;
			this.gridMessages.HasDropDowns = false;
			this.gridMessages.HasMultilineHeaders = true;
			this.gridMessages.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMessages.HeaderHeight = 15;
			this.gridMessages.HScrollVisible = true;
			this.gridMessages.Location = new System.Drawing.Point(12, 61);
			this.gridMessages.Name = "gridMessages";
			this.gridMessages.ScrollValue = 0;
			this.gridMessages.Size = new System.Drawing.Size(803, 593);
			this.gridMessages.TabIndex = 4;
			this.gridMessages.Title = "Text Messages - Right click for options - Unread messages always shown";
			this.gridMessages.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMessages.TitleHeight = 18;
			this.gridMessages.TranslationName = "FormSmsTextMessaging";
			this.gridMessages.OnSelectionCommitted += new System.EventHandler(this.gridMessages_OnSelectionCommitted);
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(97, 31);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(81, 20);
			this.textDateTo.TabIndex = 9;
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(97, 10);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(81, 20);
			this.textDateFrom.TabIndex = 7;
			// 
			// FormSmsTextMessaging
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1079, 696);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butSend);
			this.Controls.Add(this.textReply);
			this.Controls.Add(this.smsThreadView);
			this.Controls.Add(this.gridMessages);
			this.Controls.Add(this.checkRead);
			this.Controls.Add(this.checkSent);
			this.Controls.Add(this.butPatCurrent);
			this.Controls.Add(this.butPatAll);
			this.Controls.Add(this.butPatFind);
			this.Controls.Add(this.textPatient);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.checkHidden);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textDateTo);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textDateFrom);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(990, 734);
			this.Name = "FormSmsTextMessaging";
			this.Text = "Text Messaging";
			this.Load += new System.EventHandler(this.FormSmsTextMessaging_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butClose;
		private UI.ODGrid gridMessages;
		private System.Windows.Forms.Label labelClinic;
		private ODR.ValidDate textDateFrom;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private ODR.ValidDate textDateTo;
		private UI.Button butRefresh;
		private System.Windows.Forms.CheckBox checkHidden;
		private System.Windows.Forms.ContextMenu contextMenuMessages;
		private System.Windows.Forms.MenuItem menuItemChangePat;
		private System.Windows.Forms.MenuItem menuItemMarkUnread;
		private System.Windows.Forms.MenuItem menuItemMarkRead;
		private System.Windows.Forms.MenuItem menuItemHide;
		private System.Windows.Forms.MenuItem menuItemUnhide;
		private UI.ComboBoxMulti comboClinic;
		private UI.Button butPatCurrent;
		private UI.Button butPatAll;
		private UI.Button butPatFind;
		private System.Windows.Forms.TextBox textPatient;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox checkSent;
		private System.Windows.Forms.CheckBox checkRead;
		private SmsThreadView smsThreadView;
		private ODtextBox textReply;
		private UI.Button butSend;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItemGoToPatient;
		private System.Windows.Forms.MenuItem menuItemBlockNumber;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioGroupByPatient;
		private System.Windows.Forms.RadioButton radioGroupByNone;
	}
}