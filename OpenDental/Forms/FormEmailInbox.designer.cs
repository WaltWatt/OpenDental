namespace OpenDental{
	partial class FormEmailInbox {
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.TabPage tabPage1;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailInbox));
			this.splitContainerNoFlicker = new OpenDental.SplitContainerNoFlicker();
			this.gridInbox = new OpenDental.UI.ODGrid();
			this.emailPreview = new OpenDental.EmailPreviewControl();
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.menuItemSetup = new System.Windows.Forms.MenuItem();
			this.imageListButtonSmall = new System.Windows.Forms.ImageList(this.components);
			this.imageListMailboxesLarge = new System.Windows.Forms.ImageList(this.components);
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.groupShowIn = new System.Windows.Forms.GroupBox();
			this.listShowIn = new System.Windows.Forms.ListBox();
			this.checkShowHiddenEmails = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.comboEmailAddress = new System.Windows.Forms.ComboBox();
			this.butMarkUnread = new OpenDental.UI.Button();
			this.butMarkRead = new OpenDental.UI.Button();
			this.butChangePat = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butCompose = new OpenDental.UI.Button();
			this.butReply = new OpenDental.UI.Button();
			this.groupSearch = new System.Windows.Forms.GroupBox();
			this.textDateTo = new OpenDental.ValidDate();
			this.label2 = new System.Windows.Forms.Label();
			this.butClear = new OpenDental.UI.Button();
			this.butSearch = new OpenDental.UI.Button();
			this.checkSearchAttach = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textSearchBody = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textSearchEmail = new System.Windows.Forms.TextBox();
			this.textDateFrom = new OpenDental.ValidDate();
			this.label4 = new System.Windows.Forms.Label();
			this.butPickPat = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.textSearchPat = new System.Windows.Forms.TextBox();
			this.butRefresh = new OpenDental.UI.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.splitContainerSent = new OpenDental.SplitContainerNoFlicker();
			this.gridSent = new OpenDental.UI.ODGrid();
			this.emailPreviewControl1 = new OpenDental.EmailPreviewControl();
			this.textComputerName = new OpenDental.ODtextBox();
			this.textComputerNameReceive = new OpenDental.ODtextBox();
			this.labelThisComputer = new System.Windows.Forms.Label();
			this.labelInboxComputerName = new System.Windows.Forms.Label();
			this.butClose = new OpenDental.UI.Button();
			this.odToolBarButton1 = new OpenDental.UI.ODToolBarButton();
			this.butReplyAll = new OpenDental.UI.Button();
			this.butForward = new OpenDental.UI.Button();
			tabPage1 = new System.Windows.Forms.TabPage();
			tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerNoFlicker)).BeginInit();
			this.splitContainerNoFlicker.Panel1.SuspendLayout();
			this.splitContainerNoFlicker.Panel2.SuspendLayout();
			this.splitContainerNoFlicker.SuspendLayout();
			this.groupShowIn.SuspendLayout();
			this.groupSearch.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerSent)).BeginInit();
			this.splitContainerSent.Panel1.SuspendLayout();
			this.splitContainerSent.Panel2.SuspendLayout();
			this.splitContainerSent.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabPage1
			// 
			tabPage1.Controls.Add(this.splitContainerNoFlicker);
			tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			tabPage1.ImageKey = "Email_Inbox.png";
			tabPage1.Location = new System.Drawing.Point(4, 39);
			tabPage1.Name = "tabPage1";
			tabPage1.Padding = new System.Windows.Forms.Padding(3);
			tabPage1.Size = new System.Drawing.Size(1071, 675);
			tabPage1.TabIndex = 0;
			tabPage1.Text = "     Inbox      ";
			tabPage1.UseVisualStyleBackColor = true;
			// 
			// splitContainerNoFlicker
			// 
			this.splitContainerNoFlicker.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerNoFlicker.Location = new System.Drawing.Point(3, 3);
			this.splitContainerNoFlicker.Name = "splitContainerNoFlicker";
			this.splitContainerNoFlicker.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainerNoFlicker.Panel1
			// 
			this.splitContainerNoFlicker.Panel1.Controls.Add(this.gridInbox);
			this.splitContainerNoFlicker.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			// 
			// splitContainerNoFlicker.Panel2
			// 
			this.splitContainerNoFlicker.Panel2.Controls.Add(this.emailPreview);
			this.splitContainerNoFlicker.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.splitContainerNoFlicker.Panel2Collapsed = true;
			this.splitContainerNoFlicker.Size = new System.Drawing.Size(1065, 669);
			this.splitContainerNoFlicker.SplitterDistance = 235;
			this.splitContainerNoFlicker.TabIndex = 148;
			// 
			// gridInbox
			// 
			this.gridInbox.AllowSortingByColumn = true;
			this.gridInbox.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridInbox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridInbox.HasAddButton = false;
			this.gridInbox.HasDropDowns = false;
			this.gridInbox.HasMultilineHeaders = false;
			this.gridInbox.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridInbox.HeaderHeight = 15;
			this.gridInbox.HScrollVisible = false;
			this.gridInbox.Location = new System.Drawing.Point(0, 0);
			this.gridInbox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gridInbox.Name = "gridInbox";
			this.gridInbox.ScrollValue = 0;
			this.gridInbox.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridInbox.Size = new System.Drawing.Size(1065, 669);
			this.gridInbox.TabIndex = 140;
			this.gridInbox.Title = "Inbox";
			this.gridInbox.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridInbox.TitleHeight = 18;
			this.gridInbox.TranslationName = "TableApptProcs";
			this.gridInbox.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridInboxSent_CellDoubleClick);
			this.gridInbox.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridInboxSent_CellClick);
			// 
			// emailPreview
			// 
			this.emailPreview.BackColor = System.Drawing.SystemColors.Control;
			this.emailPreview.BccAddress = "";
			this.emailPreview.BodyText = "";
			this.emailPreview.CcAddress = "";
			this.emailPreview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.emailPreview.IsPreview = true;
			this.emailPreview.Location = new System.Drawing.Point(0, 0);
			this.emailPreview.Name = "emailPreview";
			this.emailPreview.Size = new System.Drawing.Size(150, 46);
			this.emailPreview.Subject = "";
			this.emailPreview.TabIndex = 0;
			this.emailPreview.ToAddress = "";
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSetup});
			// 
			// menuItemSetup
			// 
			this.menuItemSetup.Index = 0;
			this.menuItemSetup.Text = "Setup";
			this.menuItemSetup.Click += new System.EventHandler(this.menuItemSetup_Click);
			// 
			// imageListButtonSmall
			// 
			this.imageListButtonSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListButtonSmall.ImageStream")));
			this.imageListButtonSmall.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListButtonSmall.Images.SetKeyName(0, "Email_Compose.png");
			this.imageListButtonSmall.Images.SetKeyName(1, "Email_Refresh.png");
			this.imageListButtonSmall.Images.SetKeyName(2, "Email_Reply.png");
			this.imageListButtonSmall.Images.SetKeyName(3, "Email_Search.png");
			this.imageListButtonSmall.Images.SetKeyName(4, "Email_ReplyAll.png");
			this.imageListButtonSmall.Images.SetKeyName(5, "Email_Forward.png");
			// 
			// imageListMailboxesLarge
			// 
			this.imageListMailboxesLarge.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMailboxesLarge.ImageStream")));
			this.imageListMailboxesLarge.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListMailboxesLarge.Images.SetKeyName(0, "Email_Inbox.png");
			this.imageListMailboxesLarge.Images.SetKeyName(1, "Email_SentMsgs.png");
			// 
			// groupShowIn
			// 
			this.groupShowIn.Controls.Add(this.listShowIn);
			this.groupShowIn.Controls.Add(this.checkShowHiddenEmails);
			this.groupShowIn.Location = new System.Drawing.Point(746, -2);
			this.groupShowIn.Name = "groupShowIn";
			this.groupShowIn.Size = new System.Drawing.Size(216, 118);
			this.groupShowIn.TabIndex = 171;
			this.groupShowIn.TabStop = false;
			this.groupShowIn.Text = "Show Email In";
			// 
			// listShowIn
			// 
			this.listShowIn.FormattingEnabled = true;
			this.listShowIn.Location = new System.Drawing.Point(6, 20);
			this.listShowIn.Name = "listShowIn";
			this.listShowIn.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.listShowIn.Size = new System.Drawing.Size(205, 69);
			this.listShowIn.TabIndex = 168;
			this.listShowIn.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listHideInFlags_MouseClick);
			// 
			// checkShowHiddenEmails
			// 
			this.checkShowHiddenEmails.Location = new System.Drawing.Point(6, 93);
			this.checkShowHiddenEmails.Name = "checkShowHiddenEmails";
			this.checkShowHiddenEmails.Size = new System.Drawing.Size(154, 20);
			this.checkShowHiddenEmails.TabIndex = 167;
			this.checkShowHiddenEmails.Text = "Show Hidden Emails";
			this.checkShowHiddenEmails.UseVisualStyleBackColor = true;
			this.checkShowHiddenEmails.CheckedChanged += new System.EventHandler(this.checkShowHiddenEmails_CheckedChanged);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(87, 27);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(229, 16);
			this.label3.TabIndex = 0;
			this.label3.Text = "View messages for address:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboEmailAddress
			// 
			this.comboEmailAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboEmailAddress.Location = new System.Drawing.Point(90, 44);
			this.comboEmailAddress.MaxDropDownItems = 40;
			this.comboEmailAddress.Name = "comboEmailAddress";
			this.comboEmailAddress.Size = new System.Drawing.Size(233, 21);
			this.comboEmailAddress.TabIndex = 1;
			this.comboEmailAddress.SelectionChangeCommitted += new System.EventHandler(this.comboEmailAddress_SelectionChangeCommitted);
			// 
			// butMarkUnread
			// 
			this.butMarkUnread.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMarkUnread.Autosize = true;
			this.butMarkUnread.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMarkUnread.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMarkUnread.CornerRadius = 4F;
			this.butMarkUnread.Location = new System.Drawing.Point(9, 393);
			this.butMarkUnread.Name = "butMarkUnread";
			this.butMarkUnread.Size = new System.Drawing.Size(75, 20);
			this.butMarkUnread.TabIndex = 8;
			this.butMarkUnread.Text = "Mark Unread";
			this.butMarkUnread.Click += new System.EventHandler(this.butMarkUnread_Click);
			// 
			// butMarkRead
			// 
			this.butMarkRead.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMarkRead.Autosize = true;
			this.butMarkRead.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMarkRead.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMarkRead.CornerRadius = 4F;
			this.butMarkRead.Location = new System.Drawing.Point(9, 367);
			this.butMarkRead.Name = "butMarkRead";
			this.butMarkRead.Size = new System.Drawing.Size(75, 20);
			this.butMarkRead.TabIndex = 7;
			this.butMarkRead.Text = "Mark Read";
			this.butMarkRead.Click += new System.EventHandler(this.butMarkRead_Click);
			// 
			// butChangePat
			// 
			this.butChangePat.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChangePat.Autosize = true;
			this.butChangePat.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChangePat.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChangePat.CornerRadius = 4F;
			this.butChangePat.Location = new System.Drawing.Point(9, 341);
			this.butChangePat.Name = "butChangePat";
			this.butChangePat.Size = new System.Drawing.Size(75, 20);
			this.butChangePat.TabIndex = 6;
			this.butChangePat.Text = "Change Pat";
			this.butChangePat.Click += new System.EventHandler(this.butChangePat_Click);
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
			this.butDelete.Location = new System.Drawing.Point(9, 775);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75, 20);
			this.butDelete.TabIndex = 9;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butCompose
			// 
			this.butCompose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCompose.Autosize = true;
			this.butCompose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCompose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCompose.CornerRadius = 4F;
			this.butCompose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCompose.ImageKey = "Email_Compose.png";
			this.butCompose.ImageList = this.imageListButtonSmall;
			this.butCompose.Location = new System.Drawing.Point(9, 216);
			this.butCompose.Name = "butCompose";
			this.butCompose.Size = new System.Drawing.Size(75, 20);
			this.butCompose.TabIndex = 4;
			this.butCompose.Text = "Compose";
			this.butCompose.Click += new System.EventHandler(this.butCompose_Click);
			// 
			// butReply
			// 
			this.butReply.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butReply.Autosize = true;
			this.butReply.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butReply.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butReply.CornerRadius = 4F;
			this.butReply.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butReply.ImageKey = "Email_Reply.png";
			this.butReply.ImageList = this.imageListButtonSmall;
			this.butReply.Location = new System.Drawing.Point(9, 242);
			this.butReply.Name = "butReply";
			this.butReply.Size = new System.Drawing.Size(75, 20);
			this.butReply.TabIndex = 5;
			this.butReply.Text = "Reply";
			this.butReply.Click += new System.EventHandler(this.butReply_Click);
			// 
			// groupSearch
			// 
			this.groupSearch.Controls.Add(this.textDateTo);
			this.groupSearch.Controls.Add(this.label2);
			this.groupSearch.Controls.Add(this.butClear);
			this.groupSearch.Controls.Add(this.butSearch);
			this.groupSearch.Controls.Add(this.checkSearchAttach);
			this.groupSearch.Controls.Add(this.label6);
			this.groupSearch.Controls.Add(this.textSearchBody);
			this.groupSearch.Controls.Add(this.label5);
			this.groupSearch.Controls.Add(this.textSearchEmail);
			this.groupSearch.Controls.Add(this.textDateFrom);
			this.groupSearch.Controls.Add(this.label4);
			this.groupSearch.Controls.Add(this.butPickPat);
			this.groupSearch.Controls.Add(this.label1);
			this.groupSearch.Controls.Add(this.textSearchPat);
			this.groupSearch.Location = new System.Drawing.Point(329, -2);
			this.groupSearch.Name = "groupSearch";
			this.groupSearch.Size = new System.Drawing.Size(412, 118);
			this.groupSearch.TabIndex = 2;
			this.groupSearch.TabStop = false;
			this.groupSearch.Text = "Search";
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(80, 67);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(70, 20);
			this.textDateTo.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(80, 50);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 16);
			this.label2.TabIndex = 166;
			this.label2.Text = "Date To:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butClear
			// 
			this.butClear.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClear.Autosize = true;
			this.butClear.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClear.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClear.CornerRadius = 4F;
			this.butClear.Enabled = false;
			this.butClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butClear.Location = new System.Drawing.Point(359, 92);
			this.butClear.Name = "butClear";
			this.butClear.Size = new System.Drawing.Size(45, 20);
			this.butClear.TabIndex = 8;
			this.butClear.Text = "Clear";
			this.butClear.Click += new System.EventHandler(this.butClear_Click);
			// 
			// butSearch
			// 
			this.butSearch.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSearch.Autosize = true;
			this.butSearch.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSearch.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSearch.CornerRadius = 4F;
			this.butSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSearch.ImageKey = "Email_Search.png";
			this.butSearch.ImageList = this.imageListButtonSmall;
			this.butSearch.Location = new System.Drawing.Point(283, 92);
			this.butSearch.Name = "butSearch";
			this.butSearch.Size = new System.Drawing.Size(73, 20);
			this.butSearch.TabIndex = 7;
			this.butSearch.Text = "Search";
			this.butSearch.Click += new System.EventHandler(this.butSearch_Click);
			// 
			// checkSearchAttach
			// 
			this.checkSearchAttach.Location = new System.Drawing.Point(6, 92);
			this.checkSearchAttach.Name = "checkSearchAttach";
			this.checkSearchAttach.Size = new System.Drawing.Size(258, 20);
			this.checkSearchAttach.TabIndex = 3;
			this.checkSearchAttach.Text = "Only include messages with attachments";
			this.checkSearchAttach.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(181, 50);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(147, 16);
			this.label6.TabIndex = 163;
			this.label6.Text = "Subject/Body:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textSearchBody
			// 
			this.textSearchBody.Location = new System.Drawing.Point(181, 67);
			this.textSearchBody.Name = "textSearchBody";
			this.textSearchBody.Size = new System.Drawing.Size(223, 20);
			this.textSearchBody.TabIndex = 6;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(181, 12);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(173, 16);
			this.label5.TabIndex = 161;
			this.label5.Text = "Email Address:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textSearchEmail
			// 
			this.textSearchEmail.Location = new System.Drawing.Point(181, 29);
			this.textSearchEmail.Name = "textSearchEmail";
			this.textSearchEmail.Size = new System.Drawing.Size(223, 20);
			this.textSearchEmail.TabIndex = 2;
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(6, 67);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(70, 20);
			this.textDateFrom.TabIndex = 4;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(6, 50);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(71, 16);
			this.label4.TabIndex = 158;
			this.label4.Text = "Date From:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butPickPat
			// 
			this.butPickPat.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickPat.Autosize = true;
			this.butPickPat.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickPat.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickPat.CornerRadius = 2F;
			this.butPickPat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPickPat.Location = new System.Drawing.Point(151, 29);
			this.butPickPat.Name = "butPickPat";
			this.butPickPat.Size = new System.Drawing.Size(20, 20);
			this.butPickPat.TabIndex = 1;
			this.butPickPat.Text = "...";
			this.butPickPat.Click += new System.EventHandler(this.butPickPat_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(6, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(173, 16);
			this.label1.TabIndex = 156;
			this.label1.Text = "Patient:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textSearchPat
			// 
			this.textSearchPat.Location = new System.Drawing.Point(6, 29);
			this.textSearchPat.Name = "textSearchPat";
			this.textSearchPat.ReadOnly = true;
			this.textSearchPat.Size = new System.Drawing.Size(144, 20);
			this.textSearchPat.TabIndex = 0;
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butRefresh.ImageKey = "Email_Refresh.png";
			this.butRefresh.ImageList = this.imageListButtonSmall;
			this.butRefresh.Location = new System.Drawing.Point(9, 151);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 20);
			this.butRefresh.TabIndex = 3;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabControl1.ImageList = this.imageListMailboxesLarge;
			this.tabControl1.ItemSize = new System.Drawing.Size(100, 35);
			this.tabControl1.Location = new System.Drawing.Point(90, 80);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1079, 718);
			this.tabControl1.TabIndex = 1;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.splitContainerSent);
			this.tabPage2.ImageKey = "Email_SentMsgs.png";
			this.tabPage2.Location = new System.Drawing.Point(4, 39);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(1071, 675);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Sent Messages";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// splitContainerSent
			// 
			this.splitContainerSent.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerSent.Location = new System.Drawing.Point(3, 3);
			this.splitContainerSent.Name = "splitContainerSent";
			this.splitContainerSent.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainerSent.Panel1
			// 
			this.splitContainerSent.Panel1.Controls.Add(this.gridSent);
			// 
			// splitContainerSent.Panel2
			// 
			this.splitContainerSent.Panel2.Controls.Add(this.emailPreviewControl1);
			this.splitContainerSent.Panel2Collapsed = true;
			this.splitContainerSent.Size = new System.Drawing.Size(1065, 669);
			this.splitContainerSent.SplitterDistance = 235;
			this.splitContainerSent.TabIndex = 142;
			// 
			// gridSent
			// 
			this.gridSent.AllowSortingByColumn = true;
			this.gridSent.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridSent.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridSent.HasAddButton = false;
			this.gridSent.HasDropDowns = false;
			this.gridSent.HasMultilineHeaders = false;
			this.gridSent.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridSent.HeaderHeight = 15;
			this.gridSent.HScrollVisible = false;
			this.gridSent.Location = new System.Drawing.Point(0, 0);
			this.gridSent.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gridSent.Name = "gridSent";
			this.gridSent.ScrollValue = 0;
			this.gridSent.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridSent.Size = new System.Drawing.Size(1065, 669);
			this.gridSent.TabIndex = 141;
			this.gridSent.Title = "Sent Messages";
			this.gridSent.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridSent.TitleHeight = 18;
			this.gridSent.TranslationName = "TableApptProcs";
			this.gridSent.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridInboxSent_CellDoubleClick);
			this.gridSent.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridInboxSent_CellClick);
			// 
			// emailPreviewControl1
			// 
			this.emailPreviewControl1.BackColor = System.Drawing.SystemColors.Control;
			this.emailPreviewControl1.BccAddress = "";
			this.emailPreviewControl1.BodyText = "";
			this.emailPreviewControl1.CcAddress = "";
			this.emailPreviewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.emailPreviewControl1.IsPreview = true;
			this.emailPreviewControl1.Location = new System.Drawing.Point(0, 0);
			this.emailPreviewControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.emailPreviewControl1.Name = "emailPreviewControl1";
			this.emailPreviewControl1.Size = new System.Drawing.Size(150, 46);
			this.emailPreviewControl1.Subject = "";
			this.emailPreviewControl1.TabIndex = 0;
			this.emailPreviewControl1.ToAddress = "";
			// 
			// textComputerName
			// 
			this.textComputerName.AcceptsTab = true;
			this.textComputerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textComputerName.BackColor = System.Drawing.SystemColors.Control;
			this.textComputerName.DetectLinksEnabled = false;
			this.textComputerName.DetectUrls = false;
			this.textComputerName.Location = new System.Drawing.Point(316, 822);
			this.textComputerName.Multiline = false;
			this.textComputerName.Name = "textComputerName";
			this.textComputerName.QuickPasteType = OpenDentBusiness.QuickPasteType.ReadOnly;
			this.textComputerName.ReadOnly = true;
			this.textComputerName.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textComputerName.Size = new System.Drawing.Size(142, 18);
			this.textComputerName.SpellCheckIsEnabled = false;
			this.textComputerName.TabIndex = 0;
			this.textComputerName.TabStop = false;
			this.textComputerName.Text = "";
			// 
			// textComputerNameReceive
			// 
			this.textComputerNameReceive.AcceptsTab = true;
			this.textComputerNameReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textComputerNameReceive.BackColor = System.Drawing.SystemColors.Control;
			this.textComputerNameReceive.DetectLinksEnabled = false;
			this.textComputerNameReceive.DetectUrls = false;
			this.textComputerNameReceive.Location = new System.Drawing.Point(316, 802);
			this.textComputerNameReceive.Multiline = false;
			this.textComputerNameReceive.Name = "textComputerNameReceive";
			this.textComputerNameReceive.QuickPasteType = OpenDentBusiness.QuickPasteType.ReadOnly;
			this.textComputerNameReceive.ReadOnly = true;
			this.textComputerNameReceive.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textComputerNameReceive.Size = new System.Drawing.Size(142, 18);
			this.textComputerNameReceive.SpellCheckIsEnabled = false;
			this.textComputerNameReceive.TabIndex = 0;
			this.textComputerNameReceive.TabStop = false;
			this.textComputerNameReceive.Text = "";
			// 
			// labelThisComputer
			// 
			this.labelThisComputer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelThisComputer.Location = new System.Drawing.Point(9, 822);
			this.labelThisComputer.Name = "labelThisComputer";
			this.labelThisComputer.Size = new System.Drawing.Size(307, 16);
			this.labelThisComputer.TabIndex = 0;
			this.labelThisComputer.Text = "This Computer Name:";
			this.labelThisComputer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelInboxComputerName
			// 
			this.labelInboxComputerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelInboxComputerName.Location = new System.Drawing.Point(12, 803);
			this.labelInboxComputerName.Name = "labelInboxComputerName";
			this.labelInboxComputerName.Size = new System.Drawing.Size(304, 16);
			this.labelInboxComputerName.TabIndex = 0;
			this.labelInboxComputerName.Text = "Computer Name Where New Email Is Received:";
			this.labelInboxComputerName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(1091, 816);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 10;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// odToolBarButton1
			// 
			this.odToolBarButton1.Bounds = new System.Drawing.Rectangle(0, 0, 0, 0);
			this.odToolBarButton1.DropDownMenu = null;
			this.odToolBarButton1.Enabled = true;
			this.odToolBarButton1.ImageIndex = -1;
			this.odToolBarButton1.PageMax = 0;
			this.odToolBarButton1.PageValue = 0;
			this.odToolBarButton1.Pushed = false;
			this.odToolBarButton1.State = OpenDental.UI.ToolBarButtonState.Normal;
			this.odToolBarButton1.Style = OpenDental.UI.ODToolBarButtonStyle.PushButton;
			this.odToolBarButton1.Tag = "";
			this.odToolBarButton1.Text = "";
			this.odToolBarButton1.ToolTipText = "";
			// 
			// butReplyAll
			// 
			this.butReplyAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butReplyAll.Autosize = true;
			this.butReplyAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butReplyAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butReplyAll.CornerRadius = 4F;
			this.butReplyAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butReplyAll.ImageKey = "Email_ReplyAll.png";
			this.butReplyAll.ImageList = this.imageListButtonSmall;
			this.butReplyAll.Location = new System.Drawing.Point(9, 268);
			this.butReplyAll.Name = "butReplyAll";
			this.butReplyAll.Size = new System.Drawing.Size(75, 20);
			this.butReplyAll.TabIndex = 11;
			this.butReplyAll.Text = "Reply All";
			this.butReplyAll.Click += new System.EventHandler(this.butReplyAll_Click);
			// 
			// butForward
			// 
			this.butForward.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butForward.Autosize = true;
			this.butForward.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butForward.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butForward.CornerRadius = 4F;
			this.butForward.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butForward.ImageKey = "Email_Forward.png";
			this.butForward.ImageList = this.imageListButtonSmall;
			this.butForward.Location = new System.Drawing.Point(9, 294);
			this.butForward.Name = "butForward";
			this.butForward.Size = new System.Drawing.Size(75, 20);
			this.butForward.TabIndex = 12;
			this.butForward.Text = "Forward";
			this.butForward.Click += new System.EventHandler(this.butForward_Click);
			// 
			// FormEmailInbox
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1179, 854);
			this.Controls.Add(this.groupShowIn);
			this.Controls.Add(this.butForward);
			this.Controls.Add(this.butReplyAll);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.comboEmailAddress);
			this.Controls.Add(this.butMarkUnread);
			this.Controls.Add(this.butMarkRead);
			this.Controls.Add(this.butChangePat);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butCompose);
			this.Controls.Add(this.butReply);
			this.Controls.Add(this.groupSearch);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.textComputerName);
			this.Controls.Add(this.textComputerNameReceive);
			this.Controls.Add(this.labelThisComputer);
			this.Controls.Add(this.labelInboxComputerName);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.MinimumSize = new System.Drawing.Size(990, 713);
			this.Name = "FormEmailInbox";
			this.Text = "Email Inbox";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEmailInbox_FormClosing);
			this.Load += new System.EventHandler(this.FormEmailInbox_Load);
			this.Resize += new System.EventHandler(this.FormEmailInbox_Resize);
			tabPage1.ResumeLayout(false);
			this.splitContainerNoFlicker.Panel1.ResumeLayout(false);
			this.splitContainerNoFlicker.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerNoFlicker)).EndInit();
			this.splitContainerNoFlicker.ResumeLayout(false);
			this.groupShowIn.ResumeLayout(false);
			this.groupSearch.ResumeLayout(false);
			this.groupSearch.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.splitContainerSent.Panel1.ResumeLayout(false);
			this.splitContainerSent.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerSent)).EndInit();
			this.splitContainerSent.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItemSetup;
		private UI.ODToolBarButton odToolBarButton1;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox comboEmailAddress;
    private UI.Button butMarkUnread;
    private UI.Button butMarkRead;
    private UI.Button butChangePat;
    private UI.Button butDelete;
    private UI.Button butCompose;
    private System.Windows.Forms.ImageList imageListButtonSmall;
    private UI.Button butReply;
    private System.Windows.Forms.GroupBox groupSearch;
    private UI.Button butClear;
    private UI.Button butSearch;
    private System.Windows.Forms.CheckBox checkSearchAttach;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox textSearchBody;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox textSearchEmail;
    private ValidDate textDateFrom;
    private System.Windows.Forms.Label label4;
    private UI.Button butPickPat;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textSearchPat;
    private UI.Button butRefresh;
    private System.Windows.Forms.TabControl tabControl1;
    private SplitContainerNoFlicker splitContainerNoFlicker;
    private UI.ODGrid gridInbox;
    private EmailPreviewControl emailPreview;
    private System.Windows.Forms.TabPage tabPage2;
    private SplitContainerNoFlicker splitContainerSent;
    private UI.ODGrid gridSent;
    private EmailPreviewControl emailPreviewControl1;
    private System.Windows.Forms.ImageList imageListMailboxesLarge;
    private ODtextBox textComputerName;
    private ODtextBox textComputerNameReceive;
    private System.Windows.Forms.Label labelThisComputer;
    private System.Windows.Forms.Label labelInboxComputerName;
    private UI.Button butClose;
    private System.Windows.Forms.ToolTip toolTip1;
    private ValidDate textDateTo;
    private System.Windows.Forms.Label label2;
		private UI.Button butReplyAll;
		private UI.Button butForward;
		private System.Windows.Forms.CheckBox checkShowHiddenEmails;
		private System.Windows.Forms.ListBox listShowIn;
		private System.Windows.Forms.GroupBox groupShowIn;
	}
}