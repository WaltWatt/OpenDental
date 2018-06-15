namespace OpenDental {
	partial class UserControlTasks {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlTasks));
			this.tabContr = new System.Windows.Forms.TabControl();
			this.tabUser = new System.Windows.Forms.TabPage();
			this.tabNew = new System.Windows.Forms.TabPage();
			this.tabOpenTickets = new System.Windows.Forms.TabPage();
			this.tabPatientTickets = new System.Windows.Forms.TabPage();
			this.tabMain = new System.Windows.Forms.TabPage();
			this.tabReminders = new System.Windows.Forms.TabPage();
			this.tabRepeating = new System.Windows.Forms.TabPage();
			this.tabDate = new System.Windows.Forms.TabPage();
			this.tabWeek = new System.Windows.Forms.TabPage();
			this.tabMonth = new System.Windows.Forms.TabPage();
			this.cal = new System.Windows.Forms.MonthCalendar();
			this.tree = new System.Windows.Forms.TreeView();
			this.imageListTree = new System.Windows.Forms.ImageList(this.components);
			this.imageListMain = new System.Windows.Forms.ImageList(this.components);
			this.menuEdit = new System.Windows.Forms.ContextMenu();
			this.menuItemDone = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItemEdit = new System.Windows.Forms.MenuItem();
			this.menuItemPriority = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItemCut = new System.Windows.Forms.MenuItem();
			this.menuItemCopy = new System.Windows.Forms.MenuItem();
			this.menuItemPaste = new System.Windows.Forms.MenuItem();
			this.menuItemDelete = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItemSubscribe = new System.Windows.Forms.MenuItem();
			this.menuItemUnsubscribe = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItemSendToMe = new System.Windows.Forms.MenuItem();
			this.menuItemAddCommlog = new System.Windows.Forms.MenuItem();
			this.menuItemGoto = new System.Windows.Forms.MenuItem();
			this.menuNavJob = new System.Windows.Forms.MenuItem();
			this.menuDeleteTaken = new System.Windows.Forms.MenuItem();
			this.timerDoneTaskListRefresh = new System.Windows.Forms.Timer(this.components);
			this.menuTask = new System.Windows.Forms.ContextMenu();
			this.menuItemTaskReminder = new System.Windows.Forms.MenuItem();
			this.menuJobs = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.gridMain = new OpenDental.UI.ODGrid();
			this.ToolBarMain = new OpenDental.UI.ODToolBar();
			this.menuItemMarkRead = new System.Windows.Forms.MenuItem();
			this.tabContr.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabContr
			// 
			this.tabContr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabContr.Controls.Add(this.tabUser);
			this.tabContr.Controls.Add(this.tabNew);
			this.tabContr.Controls.Add(this.tabOpenTickets);
			this.tabContr.Controls.Add(this.tabPatientTickets);
			this.tabContr.Controls.Add(this.tabMain);
			this.tabContr.Controls.Add(this.tabReminders);
			this.tabContr.Controls.Add(this.tabRepeating);
			this.tabContr.Controls.Add(this.tabDate);
			this.tabContr.Controls.Add(this.tabWeek);
			this.tabContr.Controls.Add(this.tabMonth);
			this.tabContr.Location = new System.Drawing.Point(0, 29);
			this.tabContr.Name = "tabContr";
			this.tabContr.SelectedIndex = 0;
			this.tabContr.Size = new System.Drawing.Size(941, 23);
			this.tabContr.TabIndex = 5;
			this.tabContr.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabContr_MouseDown);
			// 
			// tabUser
			// 
			this.tabUser.Location = new System.Drawing.Point(4, 22);
			this.tabUser.Name = "tabUser";
			this.tabUser.Size = new System.Drawing.Size(933, 0);
			this.tabUser.TabIndex = 5;
			this.tabUser.Text = "for User";
			this.tabUser.UseVisualStyleBackColor = true;
			// 
			// tabNew
			// 
			this.tabNew.Location = new System.Drawing.Point(4, 22);
			this.tabNew.Name = "tabNew";
			this.tabNew.Size = new System.Drawing.Size(933, 0);
			this.tabNew.TabIndex = 6;
			this.tabNew.Text = "New for User";
			this.tabNew.UseVisualStyleBackColor = true;
			// 
			// tabOpenTickets
			// 
			this.tabOpenTickets.Location = new System.Drawing.Point(4, 22);
			this.tabOpenTickets.Name = "tabOpenTickets";
			this.tabOpenTickets.Size = new System.Drawing.Size(933, 0);
			this.tabOpenTickets.TabIndex = 7;
			this.tabOpenTickets.Text = "My Open Tasks";
			this.tabOpenTickets.UseVisualStyleBackColor = true;
			// 
			// tabPatientTickets
			// 
			this.tabPatientTickets.Location = new System.Drawing.Point(4, 22);
			this.tabPatientTickets.Name = "tabPatientTickets";
			this.tabPatientTickets.Padding = new System.Windows.Forms.Padding(3);
			this.tabPatientTickets.Size = new System.Drawing.Size(933, 0);
			this.tabPatientTickets.TabIndex = 10;
			this.tabPatientTickets.Text = "Patient Tasks";
			this.tabPatientTickets.UseVisualStyleBackColor = true;
			// 
			// tabMain
			// 
			this.tabMain.Location = new System.Drawing.Point(4, 22);
			this.tabMain.Name = "tabMain";
			this.tabMain.Size = new System.Drawing.Size(933, 0);
			this.tabMain.TabIndex = 0;
			this.tabMain.Text = "Main";
			this.tabMain.UseVisualStyleBackColor = true;
			// 
			// tabReminders
			// 
			this.tabReminders.Location = new System.Drawing.Point(4, 22);
			this.tabReminders.Name = "tabReminders";
			this.tabReminders.Size = new System.Drawing.Size(933, 0);
			this.tabReminders.TabIndex = 8;
			this.tabReminders.Text = "Reminders";
			this.tabReminders.UseVisualStyleBackColor = true;
			// 
			// tabRepeating
			// 
			this.tabRepeating.Location = new System.Drawing.Point(4, 22);
			this.tabRepeating.Name = "tabRepeating";
			this.tabRepeating.Size = new System.Drawing.Size(933, 0);
			this.tabRepeating.TabIndex = 2;
			this.tabRepeating.Text = "Repeating (setup)";
			this.tabRepeating.UseVisualStyleBackColor = true;
			// 
			// tabDate
			// 
			this.tabDate.Location = new System.Drawing.Point(4, 22);
			this.tabDate.Name = "tabDate";
			this.tabDate.Size = new System.Drawing.Size(933, 0);
			this.tabDate.TabIndex = 1;
			this.tabDate.Text = "By Date";
			this.tabDate.UseVisualStyleBackColor = true;
			// 
			// tabWeek
			// 
			this.tabWeek.Location = new System.Drawing.Point(4, 22);
			this.tabWeek.Name = "tabWeek";
			this.tabWeek.Size = new System.Drawing.Size(933, 0);
			this.tabWeek.TabIndex = 3;
			this.tabWeek.Text = "By Week";
			this.tabWeek.UseVisualStyleBackColor = true;
			// 
			// tabMonth
			// 
			this.tabMonth.Location = new System.Drawing.Point(4, 22);
			this.tabMonth.Name = "tabMonth";
			this.tabMonth.Size = new System.Drawing.Size(933, 0);
			this.tabMonth.TabIndex = 4;
			this.tabMonth.Text = "By Month";
			this.tabMonth.UseVisualStyleBackColor = true;
			// 
			// cal
			// 
			this.cal.Location = new System.Drawing.Point(2, 53);
			this.cal.MaxSelectionCount = 1;
			this.cal.Name = "cal";
			this.cal.TabIndex = 6;
			this.cal.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.cal_DateSelected);
			// 
			// tree
			// 
			this.tree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tree.HideSelection = false;
			this.tree.ImageIndex = 0;
			this.tree.ImageList = this.imageListTree;
			this.tree.ItemHeight = 18;
			this.tree.Location = new System.Drawing.Point(0, 206);
			this.tree.Name = "tree";
			this.tree.Scrollable = false;
			this.tree.SelectedImageIndex = 0;
			this.tree.ShowPlusMinus = false;
			this.tree.Size = new System.Drawing.Size(941, 98);
			this.tree.TabIndex = 7;
			this.tree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tree_MouseDown);
			// 
			// imageListTree
			// 
			this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
			this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListTree.Images.SetKeyName(0, "TaskList.gif");
			this.imageListTree.Images.SetKeyName(1, "checkBoxChecked.gif");
			this.imageListTree.Images.SetKeyName(2, "checkBoxUnchecked.gif");
			this.imageListTree.Images.SetKeyName(3, "TaskListHighlight.gif");
			this.imageListTree.Images.SetKeyName(4, "checkBoxNew.gif");
			// 
			// imageListMain
			// 
			this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
			this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListMain.Images.SetKeyName(0, "TaskListAdd.gif");
			this.imageListMain.Images.SetKeyName(1, "Add.gif");
			// 
			// menuEdit
			// 
			this.menuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemDone,
            this.menuItem4,
            this.menuItemEdit,
            this.menuItemPriority,
            this.menuItem6,
            this.menuItemCut,
            this.menuItemCopy,
            this.menuItemPaste,
            this.menuItemDelete,
            this.menuItem2,
            this.menuItemSubscribe,
            this.menuItemUnsubscribe,
            this.menuItem3,
            this.menuItemSendToMe,
            this.menuItemAddCommlog,
            this.menuItemGoto,
            this.menuItemMarkRead,
            this.menuNavJob,
            this.menuDeleteTaken});
			this.menuEdit.Popup += new System.EventHandler(this.menuEdit_Popup);
			// 
			// menuItemDone
			// 
			this.menuItemDone.Index = 0;
			this.menuItemDone.Text = "Done (affects all users)";
			this.menuItemDone.Click += new System.EventHandler(this.menuItemDone_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			this.menuItem4.Text = "-";
			// 
			// menuItemEdit
			// 
			this.menuItemEdit.Index = 2;
			this.menuItemEdit.Text = "Edit Properties";
			this.menuItemEdit.Click += new System.EventHandler(this.menuItemEdit_Click);
			// 
			// menuItemPriority
			// 
			this.menuItemPriority.Index = 3;
			this.menuItemPriority.Text = "Set Priority";
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 4;
			this.menuItem6.Text = "-";
			// 
			// menuItemCut
			// 
			this.menuItemCut.Index = 5;
			this.menuItemCut.Text = "Cut";
			this.menuItemCut.Click += new System.EventHandler(this.menuItemCut_Click);
			// 
			// menuItemCopy
			// 
			this.menuItemCopy.Index = 6;
			this.menuItemCopy.Text = "Copy";
			this.menuItemCopy.Click += new System.EventHandler(this.menuItemCopy_Click);
			// 
			// menuItemPaste
			// 
			this.menuItemPaste.Index = 7;
			this.menuItemPaste.Text = "Paste";
			this.menuItemPaste.Click += new System.EventHandler(this.menuItemPaste_Click);
			// 
			// menuItemDelete
			// 
			this.menuItemDelete.Index = 8;
			this.menuItemDelete.Text = "Delete";
			this.menuItemDelete.Click += new System.EventHandler(this.menuItemDelete_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 9;
			this.menuItem2.Text = "-";
			// 
			// menuItemSubscribe
			// 
			this.menuItemSubscribe.Index = 10;
			this.menuItemSubscribe.Text = "Subscribe";
			this.menuItemSubscribe.Click += new System.EventHandler(this.menuItemSubscribe_Click);
			// 
			// menuItemUnsubscribe
			// 
			this.menuItemUnsubscribe.Index = 11;
			this.menuItemUnsubscribe.Text = "Unsubscribe";
			this.menuItemUnsubscribe.Click += new System.EventHandler(this.menuItemUnsubscribe_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 12;
			this.menuItem3.Text = "-";
			// 
			// menuItemSendToMe
			// 
			this.menuItemSendToMe.Index = 13;
			this.menuItemSendToMe.Text = "Send to Me";
			this.menuItemSendToMe.Click += new System.EventHandler(this.menuItemSendToMe_Click);
			// 
			// menuItemAddCommlog
			// 
			this.menuItemAddCommlog.Index = 14;
			this.menuItemAddCommlog.Text = "Send to Me && Create Commlog";
			this.menuItemAddCommlog.Visible = false;
			this.menuItemAddCommlog.Click += new System.EventHandler(this.menuItemAddCommlog_Click);
			// 
			// menuItemGoto
			// 
			this.menuItemGoto.Index = 15;
			this.menuItemGoto.Text = "Go To";
			this.menuItemGoto.Click += new System.EventHandler(this.menuItemGoto_Click);
			// 
			// menuNavJob
			// 
			this.menuNavJob.Index = 17;
			this.menuNavJob.Text = "Navigate to Job";
			this.menuNavJob.Visible = false;
			// 
			// menuDeleteTaken
			// 
			this.menuDeleteTaken.Index = 18;
			this.menuDeleteTaken.Text = "Delete Task Taken";
			this.menuDeleteTaken.Visible = false;
			this.menuDeleteTaken.Click += new System.EventHandler(this.menuDeleteTaken_Click);
			// 
			// timerDoneTaskListRefresh
			// 
			this.timerDoneTaskListRefresh.Interval = 2000;
			this.timerDoneTaskListRefresh.Tick += new System.EventHandler(this.timerDoneTaskListRefresh_Tick);
			// 
			// menuTask
			// 
			this.menuTask.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemTaskReminder});
			// 
			// menuItemTaskReminder
			// 
			this.menuItemTaskReminder.Index = 0;
			this.menuItemTaskReminder.Text = "Reminder";
			this.menuItemTaskReminder.Click += new System.EventHandler(this.menuItemTaskReminder_Click);
			// 
			// menuJobs
			// 
			this.menuJobs.Name = "menuJobs";
			this.menuJobs.Size = new System.Drawing.Size(61, 4);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(0, 310);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(941, 200);
			this.gridMain.TabIndex = 9;
			this.gridMain.Title = "Tasks";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableTasks";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridMain_MouseDown);
			// 
			// ToolBarMain
			// 
			this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.ToolBarMain.ImageList = this.imageListMain;
			this.ToolBarMain.Location = new System.Drawing.Point(0, 0);
			this.ToolBarMain.Name = "ToolBarMain";
			this.ToolBarMain.Size = new System.Drawing.Size(941, 25);
			this.ToolBarMain.TabIndex = 2;
			this.ToolBarMain.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.ToolBarMain_ButtonClick);
			// 
			// menuItemMarkRead
			// 
			this.menuItemMarkRead.Index = 16;
			this.menuItemMarkRead.Text = "Mark as Read";
			this.menuItemMarkRead.Click += new System.EventHandler(this.menuItemMarkRead_Click);
			// 
			// UserControlTasks
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.tree);
			this.Controls.Add(this.cal);
			this.Controls.Add(this.tabContr);
			this.Controls.Add(this.ToolBarMain);
			this.Name = "UserControlTasks";
			this.Size = new System.Drawing.Size(941, 510);
			this.Load += new System.EventHandler(this.UserControlTasks_Load);
			this.tabContr.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.ODToolBar ToolBarMain;
		private System.Windows.Forms.TabControl tabContr;
		private System.Windows.Forms.TabPage tabMain;
		private System.Windows.Forms.TabPage tabRepeating;
		private System.Windows.Forms.TabPage tabDate;
		private System.Windows.Forms.TabPage tabWeek;
		private System.Windows.Forms.TabPage tabMonth;
		private System.Windows.Forms.MonthCalendar cal;
		private System.Windows.Forms.TreeView tree;
		private System.Windows.Forms.ImageList imageListMain;
		private System.Windows.Forms.ContextMenu menuEdit;
		private System.Windows.Forms.MenuItem menuItemEdit;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItemCut;
		private System.Windows.Forms.MenuItem menuItemCopy;
		private System.Windows.Forms.MenuItem menuItemPaste;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItemGoto;
		private System.Windows.Forms.ImageList imageListTree;
		private System.Windows.Forms.TabPage tabUser;
		private System.Windows.Forms.MenuItem menuItemSubscribe;
		private System.Windows.Forms.MenuItem menuItemUnsubscribe;
		private System.Windows.Forms.MenuItem menuItem3;
		private OpenDental.UI.ODGrid gridMain;
		private System.Windows.Forms.TabPage tabNew;
		private System.Windows.Forms.TabPage tabOpenTickets;
		private System.Windows.Forms.MenuItem menuItemDone;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItemSendToMe;
		private System.Windows.Forms.Timer timerDoneTaskListRefresh;
		private System.Windows.Forms.TabPage tabReminders;
		private System.Windows.Forms.ContextMenu menuTask;
		private System.Windows.Forms.MenuItem menuItemTaskReminder;
		private System.Windows.Forms.TabPage tabPatientTickets;
		private System.Windows.Forms.MenuItem menuItemAddCommlog;
		private System.Windows.Forms.ContextMenuStrip menuJobs;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.MenuItem menuNavJob;
		private System.Windows.Forms.MenuItem menuItemPriority;
		private System.Windows.Forms.MenuItem menuDeleteTaken;
		private System.Windows.Forms.MenuItem menuItemMarkRead;
	}
}
