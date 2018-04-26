namespace OpenDental{
	partial class FormBugSubmission {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBugSubmission));
			this.butAddViewJob = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.textStack = new OpenDental.ODtextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.butBugTask = new OpenDental.UI.Button();
			this.labelLastCall = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.labelSubNum = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.butGoToAccount = new OpenDental.UI.Button();
			this.labelCustomerNum = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.labelCustomerPhone = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.labelCustomerState = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.labelCustomerName = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.gridOfficeInfo = new OpenDental.UI.ODGrid();
			this.labelRegKey = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.labelDateTime = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.labelVersion = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.butAddViewBug = new OpenDental.UI.Button();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// butAddViewJob
			// 
			this.butAddViewJob.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddViewJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAddViewJob.Autosize = true;
			this.butAddViewJob.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddViewJob.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddViewJob.CornerRadius = 4F;
			this.butAddViewJob.Location = new System.Drawing.Point(518, 612);
			this.butAddViewJob.MinimumSize = new System.Drawing.Size(80, 24);
			this.butAddViewJob.Name = "butAddViewJob";
			this.butAddViewJob.Size = new System.Drawing.Size(80, 24);
			this.butAddViewJob.TabIndex = 3;
			this.butAddViewJob.Text = "&Add Job";
			this.butAddViewJob.Click += new System.EventHandler(this.butAddViewJob_Click);
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
			this.butCancel.Location = new System.Drawing.Point(604, 612);
			this.butCancel.MinimumSize = new System.Drawing.Size(80, 24);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(80, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// textStack
			// 
			this.textStack.AcceptsTab = true;
			this.textStack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textStack.BackColor = System.Drawing.SystemColors.Window;
			this.textStack.DetectLinksEnabled = false;
			this.textStack.DetectUrls = false;
			this.textStack.Location = new System.Drawing.Point(12, 37);
			this.textStack.Name = "textStack";
			this.textStack.QuickPasteType = OpenDentBusiness.QuickPasteType.JobManager;
			this.textStack.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textStack.Size = new System.Drawing.Size(411, 445);
			this.textStack.SpellCheckIsEnabled = false;
			this.textStack.TabIndex = 5;
			this.textStack.Text = "";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox2.Controls.Add(this.butBugTask);
			this.groupBox2.Controls.Add(this.labelLastCall);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.labelSubNum);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.butGoToAccount);
			this.groupBox2.Controls.Add(this.labelCustomerNum);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.labelCustomerPhone);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.labelCustomerState);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.labelCustomerName);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Location = new System.Drawing.Point(12, 488);
			this.groupBox2.MinimumSize = new System.Drawing.Size(411, 148);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(411, 148);
			this.groupBox2.TabIndex = 17;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Submitter Info.";
			// 
			// butBugTask
			// 
			this.butBugTask.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBugTask.Autosize = true;
			this.butBugTask.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBugTask.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBugTask.CornerRadius = 4F;
			this.butBugTask.Location = new System.Drawing.Point(115, 122);
			this.butBugTask.Name = "butBugTask";
			this.butBugTask.Size = new System.Drawing.Size(104, 24);
			this.butBugTask.TabIndex = 30;
			this.butBugTask.Text = "&Create Task";
			this.butBugTask.Click += new System.EventHandler(this.butBugTask_Click);
			// 
			// labelLastCall
			// 
			this.labelLastCall.Location = new System.Drawing.Point(78, 106);
			this.labelLastCall.Name = "labelLastCall";
			this.labelLastCall.Size = new System.Drawing.Size(215, 13);
			this.labelLastCall.TabIndex = 29;
			this.labelLastCall.Text = "XXXXX";
			this.labelLastCall.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label12
			// 
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.Location = new System.Drawing.Point(7, 106);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(65, 13);
			this.label12.TabIndex = 28;
			this.label12.Text = "Last Call:";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelSubNum
			// 
			this.labelSubNum.Location = new System.Drawing.Point(78, 88);
			this.labelSubNum.Name = "labelSubNum";
			this.labelSubNum.Size = new System.Drawing.Size(215, 13);
			this.labelSubNum.TabIndex = 27;
			this.labelSubNum.Text = "XXXXX";
			this.labelSubNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.Location = new System.Drawing.Point(7, 88);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(65, 13);
			this.label10.TabIndex = 26;
			this.label10.Text = "Sub Num:";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butGoToAccount
			// 
			this.butGoToAccount.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGoToAccount.Autosize = true;
			this.butGoToAccount.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGoToAccount.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGoToAccount.CornerRadius = 4F;
			this.butGoToAccount.Location = new System.Drawing.Point(6, 122);
			this.butGoToAccount.Name = "butGoToAccount";
			this.butGoToAccount.Size = new System.Drawing.Size(103, 24);
			this.butGoToAccount.TabIndex = 21;
			this.butGoToAccount.Text = "&Go To Account";
			this.butGoToAccount.Click += new System.EventHandler(this.butGoToAccount_Click);
			// 
			// labelCustomerNum
			// 
			this.labelCustomerNum.Location = new System.Drawing.Point(78, 16);
			this.labelCustomerNum.Name = "labelCustomerNum";
			this.labelCustomerNum.Size = new System.Drawing.Size(215, 13);
			this.labelCustomerNum.TabIndex = 25;
			this.labelCustomerNum.Text = "XXXXX";
			this.labelCustomerNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.Location = new System.Drawing.Point(7, 16);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(65, 13);
			this.label11.TabIndex = 24;
			this.label11.Text = "Pat Num:";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelCustomerPhone
			// 
			this.labelCustomerPhone.Location = new System.Drawing.Point(78, 70);
			this.labelCustomerPhone.Name = "labelCustomerPhone";
			this.labelCustomerPhone.Size = new System.Drawing.Size(215, 13);
			this.labelCustomerPhone.TabIndex = 23;
			this.labelCustomerPhone.Text = "(555)555-5555";
			this.labelCustomerPhone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(7, 70);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(65, 13);
			this.label9.TabIndex = 22;
			this.label9.Text = "Work Phone:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelCustomerState
			// 
			this.labelCustomerState.Location = new System.Drawing.Point(78, 52);
			this.labelCustomerState.Name = "labelCustomerState";
			this.labelCustomerState.Size = new System.Drawing.Size(215, 13);
			this.labelCustomerState.TabIndex = 21;
			this.labelCustomerState.Text = "OR";
			this.labelCustomerState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(7, 52);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(65, 13);
			this.label8.TabIndex = 20;
			this.label8.Text = "State:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelCustomerName
			// 
			this.labelCustomerName.Location = new System.Drawing.Point(78, 34);
			this.labelCustomerName.Name = "labelCustomerName";
			this.labelCustomerName.Size = new System.Drawing.Size(215, 13);
			this.labelCustomerName.TabIndex = 19;
			this.labelCustomerName.Text = "John Doe";
			this.labelCustomerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(7, 34);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(65, 13);
			this.label6.TabIndex = 18;
			this.label6.Text = "Name:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gridOfficeInfo
			// 
			this.gridOfficeInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridOfficeInfo.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridOfficeInfo.HasAddButton = false;
			this.gridOfficeInfo.HasDropDowns = false;
			this.gridOfficeInfo.HasMultilineHeaders = false;
			this.gridOfficeInfo.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridOfficeInfo.HeaderHeight = 15;
			this.gridOfficeInfo.HScrollVisible = false;
			this.gridOfficeInfo.Location = new System.Drawing.Point(429, 37);
			this.gridOfficeInfo.Name = "gridOfficeInfo";
			this.gridOfficeInfo.ScrollValue = 0;
			this.gridOfficeInfo.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridOfficeInfo.Size = new System.Drawing.Size(255, 569);
			this.gridOfficeInfo.TabIndex = 20;
			this.gridOfficeInfo.Title = "Office Info";
			this.gridOfficeInfo.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridOfficeInfo.TitleHeight = 18;
			this.gridOfficeInfo.TranslationName = "TableOfficeInfo";
			// 
			// labelRegKey
			// 
			this.labelRegKey.Location = new System.Drawing.Point(81, 7);
			this.labelRegKey.Name = "labelRegKey";
			this.labelRegKey.Size = new System.Drawing.Size(215, 13);
			this.labelRegKey.TabIndex = 32;
			this.labelRegKey.Text = "XXXXX";
			this.labelRegKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(12, 7);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(65, 13);
			this.label2.TabIndex = 31;
			this.label2.Text = "RegKey:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelDateTime
			// 
			this.labelDateTime.Location = new System.Drawing.Point(81, 21);
			this.labelDateTime.Name = "labelDateTime";
			this.labelDateTime.Size = new System.Drawing.Size(215, 13);
			this.labelDateTime.TabIndex = 34;
			this.labelDateTime.Text = "XXXXX";
			this.labelDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(12, 21);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(65, 13);
			this.label4.TabIndex = 33;
			this.label4.Text = "DateTime:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelVersion
			// 
			this.labelVersion.Location = new System.Drawing.Point(371, 7);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(215, 13);
			this.labelVersion.TabIndex = 36;
			this.labelVersion.Text = "XXXXX";
			this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(302, 7);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(65, 13);
			this.label3.TabIndex = 35;
			this.label3.Text = "Version:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butAddViewBug
			// 
			this.butAddViewBug.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddViewBug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAddViewBug.Autosize = true;
			this.butAddViewBug.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddViewBug.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddViewBug.CornerRadius = 4F;
			this.butAddViewBug.Location = new System.Drawing.Point(432, 612);
			this.butAddViewBug.MinimumSize = new System.Drawing.Size(80, 24);
			this.butAddViewBug.Name = "butAddViewBug";
			this.butAddViewBug.Size = new System.Drawing.Size(80, 24);
			this.butAddViewBug.TabIndex = 37;
			this.butAddViewBug.Text = "&Add Bug";
			this.butAddViewBug.Click += new System.EventHandler(this.butAddViewBug_Click);
			// 
			// FormBugSubmission
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(692, 642);
			this.Controls.Add(this.butAddViewBug);
			this.Controls.Add(this.labelVersion);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.labelDateTime);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.labelRegKey);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.gridOfficeInfo);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.textStack);
			this.Controls.Add(this.butAddViewJob);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(708, 681);
			this.Name = "FormBugSubmission";
			this.Text = "Bug Submissions";
			this.Load += new System.EventHandler(this.FormBugSubmission_Load);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butAddViewJob;
		private OpenDental.UI.Button butCancel;
		private ODtextBox textStack;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label labelCustomerName;
		private System.Windows.Forms.Label labelCustomerPhone;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label labelCustomerState;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label labelCustomerNum;
		private System.Windows.Forms.Label label11;
		private UI.ODGrid gridOfficeInfo;
		private UI.Button butGoToAccount;
		private System.Windows.Forms.Label labelSubNum;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label labelLastCall;
		private System.Windows.Forms.Label label12;
		private UI.Button butBugTask;
		private System.Windows.Forms.Label labelRegKey;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label labelDateTime;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.Label label3;
		private UI.Button butAddViewBug;
	}
}