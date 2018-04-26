namespace OpenDental{
	partial class FormRequestEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRequestEdit));
			this.label3 = new System.Windows.Forms.Label();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.checkIsMine = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textDetail = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textDifficulty = new System.Windows.Forms.TextBox();
			this.textApproval = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.groupMyVotes = new System.Windows.Forms.GroupBox();
			this.label9 = new System.Windows.Forms.Label();
			this.textMyPointsRemain = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.textMyPledge = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.checkIsCritical = new System.Windows.Forms.CheckBox();
			this.textMyPoints = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.textBounty = new System.Windows.Forms.TextBox();
			this.label18 = new System.Windows.Forms.Label();
			this.textWeight = new System.Windows.Forms.TextBox();
			this.label17 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.textTotalCritical = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.textTotalPledged = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.textTotalPoints = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.comboApproval = new System.Windows.Forms.ComboBox();
			this.textSubmitter = new System.Windows.Forms.TextBox();
			this.labelSubmitter = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.labelDiscuss = new System.Windows.Forms.Label();
			this.textNote = new System.Windows.Forms.TextBox();
			this.butAddDiscuss = new OpenDental.UI.Button();
			this.butJordan = new OpenDental.UI.Button();
			this.butResubmit = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.textRequestId = new System.Windows.Forms.TextBox();
			this.labelReqId = new System.Windows.Forms.Label();
			this.groupMyVotes.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(4, 3);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(105, 18);
			this.label3.TabIndex = 60;
			this.label3.Text = "Short Description";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDescription
			// 
			this.textDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDescription.BackColor = System.Drawing.Color.White;
			this.textDescription.Location = new System.Drawing.Point(111, 3);
			this.textDescription.Multiline = true;
			this.textDescription.Name = "textDescription";
			this.textDescription.ReadOnly = true;
			this.textDescription.Size = new System.Drawing.Size(410, 35);
			this.textDescription.TabIndex = 0;
			// 
			// checkIsMine
			// 
			this.checkIsMine.AutoCheck = false;
			this.checkIsMine.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsMine.Location = new System.Drawing.Point(1, 140);
			this.checkIsMine.Name = "checkIsMine";
			this.checkIsMine.Size = new System.Drawing.Size(124, 20);
			this.checkIsMine.TabIndex = 63;
			this.checkIsMine.Text = "Submitted by me";
			this.checkIsMine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsMine.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(2, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(107, 18);
			this.label2.TabIndex = 65;
			this.label2.Text = "Detail";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDetail
			// 
			this.textDetail.AcceptsReturn = true;
			this.textDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(229)))), ((int)(((byte)(233)))));
			this.textDetail.Location = new System.Drawing.Point(111, 40);
			this.textDetail.Multiline = true;
			this.textDetail.Name = "textDetail";
			this.textDetail.ReadOnly = true;
			this.textDetail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textDetail.Size = new System.Drawing.Size(410, 98);
			this.textDetail.TabIndex = 1;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(14, 163);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(95, 18);
			this.label4.TabIndex = 66;
			this.label4.Text = "Difficulty";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDifficulty
			// 
			this.textDifficulty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(239)))), ((int)(((byte)(243)))));
			this.textDifficulty.Location = new System.Drawing.Point(111, 162);
			this.textDifficulty.Name = "textDifficulty";
			this.textDifficulty.ReadOnly = true;
			this.textDifficulty.Size = new System.Drawing.Size(38, 20);
			this.textDifficulty.TabIndex = 67;
			this.textDifficulty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textApproval
			// 
			this.textApproval.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(239)))), ((int)(((byte)(243)))));
			this.textApproval.Location = new System.Drawing.Point(111, 184);
			this.textApproval.Name = "textApproval";
			this.textApproval.ReadOnly = true;
			this.textApproval.Size = new System.Drawing.Size(410, 20);
			this.textApproval.TabIndex = 69;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(-1, 185);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(110, 18);
			this.label5.TabIndex = 68;
			this.label5.Text = "Approval";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupMyVotes
			// 
			this.groupMyVotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupMyVotes.Controls.Add(this.label9);
			this.groupMyVotes.Controls.Add(this.textMyPointsRemain);
			this.groupMyVotes.Controls.Add(this.label8);
			this.groupMyVotes.Controls.Add(this.textMyPledge);
			this.groupMyVotes.Controls.Add(this.label7);
			this.groupMyVotes.Controls.Add(this.checkIsCritical);
			this.groupMyVotes.Controls.Add(this.textMyPoints);
			this.groupMyVotes.Controls.Add(this.label6);
			this.groupMyVotes.Location = new System.Drawing.Point(527, 3);
			this.groupMyVotes.Name = "groupMyVotes";
			this.groupMyVotes.Size = new System.Drawing.Size(347, 120);
			this.groupMyVotes.TabIndex = 3;
			this.groupMyVotes.TabStop = false;
			this.groupMyVotes.Text = "My Votes";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(2, 83);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(342, 34);
			this.label9.TabIndex = 75;
			this.label9.Text = "Pledges are neither required nor requested.  They are for unusual situations wher" +
    "e a feature is extremely important to someone.";
			// 
			// textMyPointsRemain
			// 
			this.textMyPointsRemain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(239)))), ((int)(((byte)(243)))));
			this.textMyPointsRemain.Location = new System.Drawing.Point(264, 17);
			this.textMyPointsRemain.Name = "textMyPointsRemain";
			this.textMyPointsRemain.ReadOnly = true;
			this.textMyPointsRemain.Size = new System.Drawing.Size(38, 20);
			this.textMyPointsRemain.TabIndex = 74;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(157, 18);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(105, 18);
			this.label8.TabIndex = 73;
			this.label8.Text = "Points Remaining";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMyPledge
			// 
			this.textMyPledge.BackColor = System.Drawing.SystemColors.Window;
			this.textMyPledge.Location = new System.Drawing.Point(108, 57);
			this.textMyPledge.Name = "textMyPledge";
			this.textMyPledge.Size = new System.Drawing.Size(63, 20);
			this.textMyPledge.TabIndex = 1;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(4, 58);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(103, 18);
			this.label7.TabIndex = 71;
			this.label7.Text = "Pledge Amount $";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkIsCritical
			// 
			this.checkIsCritical.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsCritical.Location = new System.Drawing.Point(26, 37);
			this.checkIsCritical.Name = "checkIsCritical";
			this.checkIsCritical.Size = new System.Drawing.Size(97, 20);
			this.checkIsCritical.TabIndex = 70;
			this.checkIsCritical.Text = "Is Critical";
			this.checkIsCritical.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsCritical.UseVisualStyleBackColor = true;
			this.checkIsCritical.Click += new System.EventHandler(this.checkIsCritical_Click);
			// 
			// textMyPoints
			// 
			this.textMyPoints.BackColor = System.Drawing.SystemColors.Window;
			this.textMyPoints.Location = new System.Drawing.Point(108, 16);
			this.textMyPoints.Name = "textMyPoints";
			this.textMyPoints.Size = new System.Drawing.Size(38, 20);
			this.textMyPoints.TabIndex = 0;
			this.textMyPoints.TextChanged += new System.EventHandler(this.textMyPoints_TextChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(37, 17);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(69, 18);
			this.label6.TabIndex = 68;
			this.label6.Text = "Points";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.textBounty);
			this.groupBox2.Controls.Add(this.label18);
			this.groupBox2.Controls.Add(this.textWeight);
			this.groupBox2.Controls.Add(this.label17);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.textTotalCritical);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.textTotalPledged);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.textTotalPoints);
			this.groupBox2.Controls.Add(this.label13);
			this.groupBox2.Location = new System.Drawing.Point(527, 126);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(347, 80);
			this.groupBox2.TabIndex = 72;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Totals - See manual for explanations";
			// 
			// textBounty
			// 
			this.textBounty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(239)))), ((int)(((byte)(243)))));
			this.textBounty.Location = new System.Drawing.Point(253, 57);
			this.textBounty.Name = "textBounty";
			this.textBounty.ReadOnly = true;
			this.textBounty.Size = new System.Drawing.Size(63, 20);
			this.textBounty.TabIndex = 76;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(179, 58);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(73, 18);
			this.label18.TabIndex = 77;
			this.label18.Text = "Bounty $";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textWeight
			// 
			this.textWeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(239)))), ((int)(((byte)(243)))));
			this.textWeight.Location = new System.Drawing.Point(264, 17);
			this.textWeight.Name = "textWeight";
			this.textWeight.ReadOnly = true;
			this.textWeight.Size = new System.Drawing.Size(38, 20);
			this.textWeight.TabIndex = 81;
			this.textWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(301, 18);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(38, 18);
			this.label17.TabIndex = 86;
			this.label17.Text = "/100";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(169, 18);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(95, 18);
			this.label11.TabIndex = 80;
			this.label11.Text = "Weight";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTotalCritical
			// 
			this.textTotalCritical.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(229)))), ((int)(((byte)(233)))));
			this.textTotalCritical.Location = new System.Drawing.Point(108, 37);
			this.textTotalCritical.Name = "textTotalCritical";
			this.textTotalCritical.ReadOnly = true;
			this.textTotalCritical.Size = new System.Drawing.Size(38, 20);
			this.textTotalCritical.TabIndex = 74;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(7, 38);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(99, 18);
			this.label10.TabIndex = 73;
			this.label10.Text = "Is Critical";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTotalPledged
			// 
			this.textTotalPledged.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(239)))), ((int)(((byte)(243)))));
			this.textTotalPledged.Location = new System.Drawing.Point(108, 57);
			this.textTotalPledged.Name = "textTotalPledged";
			this.textTotalPledged.ReadOnly = true;
			this.textTotalPledged.Size = new System.Drawing.Size(63, 20);
			this.textTotalPledged.TabIndex = 72;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(4, 58);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(103, 18);
			this.label12.TabIndex = 71;
			this.label12.Text = "Pledged $";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTotalPoints
			// 
			this.textTotalPoints.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(229)))), ((int)(((byte)(233)))));
			this.textTotalPoints.Location = new System.Drawing.Point(108, 17);
			this.textTotalPoints.Name = "textTotalPoints";
			this.textTotalPoints.ReadOnly = true;
			this.textTotalPoints.Size = new System.Drawing.Size(38, 20);
			this.textTotalPoints.TabIndex = 69;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(37, 18);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(69, 18);
			this.label13.TabIndex = 68;
			this.label13.Text = "Points";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboApproval
			// 
			this.comboApproval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboApproval.FormattingEnabled = true;
			this.comboApproval.Location = new System.Drawing.Point(434, 184);
			this.comboApproval.MaxDropDownItems = 20;
			this.comboApproval.Name = "comboApproval";
			this.comboApproval.Size = new System.Drawing.Size(87, 21);
			this.comboApproval.TabIndex = 73;
			this.comboApproval.SelectedIndexChanged += new System.EventHandler(this.comboApproval_SelectedIndexChanged);
			// 
			// textSubmitter
			// 
			this.textSubmitter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(239)))), ((int)(((byte)(243)))));
			this.textSubmitter.Location = new System.Drawing.Point(111, 140);
			this.textSubmitter.Name = "textSubmitter";
			this.textSubmitter.ReadOnly = true;
			this.textSubmitter.Size = new System.Drawing.Size(309, 20);
			this.textSubmitter.TabIndex = 76;
			this.textSubmitter.Visible = false;
			// 
			// labelSubmitter
			// 
			this.labelSubmitter.Location = new System.Drawing.Point(47, 140);
			this.labelSubmitter.Name = "labelSubmitter";
			this.labelSubmitter.Size = new System.Drawing.Size(62, 18);
			this.labelSubmitter.TabIndex = 77;
			this.labelSubmitter.Text = "Submitter";
			this.labelSubmitter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelSubmitter.Visible = false;
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(239)))), ((int)(((byte)(243)))));
			this.textBox1.Location = new System.Drawing.Point(405, 162);
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(38, 20);
			this.textBox1.TabIndex = 79;
			this.textBox1.Visible = false;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(321, 163);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 18);
			this.label1.TabIndex = 78;
			this.label1.Text = "Priority  /10";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label1.Visible = false;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(184, 162);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(143, 18);
			this.label14.TabIndex = 80;
			this.label14.Text = "(the lower the better)";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(148, 163);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(32, 18);
			this.label16.TabIndex = 85;
			this.label16.Text = "/10";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelDiscuss
			// 
			this.labelDiscuss.Location = new System.Drawing.Point(211, 537);
			this.labelDiscuss.Name = "labelDiscuss";
			this.labelDiscuss.Size = new System.Drawing.Size(655, 19);
			this.labelDiscuss.TabIndex = 86;
			this.labelDiscuss.Text = "This discussion is very leisurely.  Nobody necessarily checks it for new messages" +
    ".  Try to prepend your name to the note.";
			// 
			// textNote
			// 
			this.textNote.AcceptsReturn = true;
			this.textNote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textNote.BackColor = System.Drawing.Color.White;
			this.textNote.Location = new System.Drawing.Point(214, 556);
			this.textNote.Multiline = true;
			this.textNote.Name = "textNote";
			this.textNote.Size = new System.Drawing.Size(575, 89);
			this.textNote.TabIndex = 88;
			// 
			// butAddDiscuss
			// 
			this.butAddDiscuss.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddDiscuss.Autosize = true;
			this.butAddDiscuss.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddDiscuss.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddDiscuss.CornerRadius = 4F;
			this.butAddDiscuss.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddDiscuss.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddDiscuss.Location = new System.Drawing.Point(127, 556);
			this.butAddDiscuss.Name = "butAddDiscuss";
			this.butAddDiscuss.Size = new System.Drawing.Size(81, 24);
			this.butAddDiscuss.TabIndex = 87;
			this.butAddDiscuss.Text = "Save";
			this.butAddDiscuss.Click += new System.EventHandler(this.butAddDiscuss_Click);
			// 
			// butJordan
			// 
			this.butJordan.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butJordan.Autosize = true;
			this.butJordan.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butJordan.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butJordan.CornerRadius = 4F;
			this.butJordan.Location = new System.Drawing.Point(7, 96);
			this.butJordan.Name = "butJordan";
			this.butJordan.Size = new System.Drawing.Size(98, 24);
			this.butJordan.TabIndex = 84;
			this.butJordan.Text = "Jordan\'s Override";
			this.butJordan.Visible = false;
			this.butJordan.Click += new System.EventHandler(this.butJordan_Click);
			// 
			// butResubmit
			// 
			this.butResubmit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butResubmit.Autosize = true;
			this.butResubmit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butResubmit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butResubmit.CornerRadius = 4F;
			this.butResubmit.Location = new System.Drawing.Point(446, 140);
			this.butResubmit.Name = "butResubmit";
			this.butResubmit.Size = new System.Drawing.Size(75, 24);
			this.butResubmit.TabIndex = 74;
			this.butResubmit.Text = "Resubmit";
			this.butResubmit.Visible = false;
			this.butResubmit.Click += new System.EventHandler(this.butResubmit_Click);
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
			this.butDelete.Location = new System.Drawing.Point(15, 638);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75, 24);
			this.butDelete.TabIndex = 75;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(15, 208);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(775, 326);
			this.gridMain.TabIndex = 70;
			this.gridMain.Title = "Discussion";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableDiscussion";
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(793, 608);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 2;
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
			this.butCancel.Location = new System.Drawing.Point(793, 638);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// textRequestId
			// 
			this.textRequestId.Location = new System.Drawing.Point(64, 71);
			this.textRequestId.Name = "textRequestId";
			this.textRequestId.Size = new System.Drawing.Size(41, 20);
			this.textRequestId.TabIndex = 93;
			// 
			// labelReqId
			// 
			this.labelReqId.Location = new System.Drawing.Point(7, 71);
			this.labelReqId.Name = "labelReqId";
			this.labelReqId.Size = new System.Drawing.Size(56, 18);
			this.labelReqId.TabIndex = 92;
			this.labelReqId.Text = "Req Id";
			this.labelReqId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormRequestEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(883, 668);
			this.Controls.Add(this.textRequestId);
			this.Controls.Add(this.labelReqId);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.butAddDiscuss);
			this.Controls.Add(this.labelDiscuss);
			this.Controls.Add(this.textDifficulty);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.butJordan);
			this.Controls.Add(this.butResubmit);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.labelSubmitter);
			this.Controls.Add(this.checkIsMine);
			this.Controls.Add(this.textSubmitter);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.comboApproval);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupMyVotes);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.textApproval);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textDetail);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormRequestEdit";
			this.Text = "Edit Request";
			this.Load += new System.EventHandler(this.FormRequestEdit_Load);
			this.groupMyVotes.ResumeLayout(false);
			this.groupMyVotes.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textDescription;
		private System.Windows.Forms.CheckBox checkIsMine;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textDetail;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textDifficulty;
		private System.Windows.Forms.TextBox textApproval;
		private System.Windows.Forms.Label label5;
		private OpenDental.UI.ODGrid gridMain;
		private System.Windows.Forms.GroupBox groupMyVotes;
		private System.Windows.Forms.TextBox textMyPoints;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox textMyPointsRemain;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textMyPledge;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.CheckBox checkIsCritical;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox textTotalPledged;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox textTotalPoints;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.TextBox textTotalCritical;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ComboBox comboApproval;
		private OpenDental.UI.Button butResubmit;
		private OpenDental.UI.Button butDelete;
		private System.Windows.Forms.TextBox textSubmitter;
		private System.Windows.Forms.Label labelSubmitter;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textWeight;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label14;
		private OpenDental.UI.Button butJordan;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label labelDiscuss;
		private OpenDental.UI.Button butAddDiscuss;
		private System.Windows.Forms.TextBox textNote;
		private System.Windows.Forms.TextBox textRequestId;
		private System.Windows.Forms.Label labelReqId;
		private System.Windows.Forms.TextBox textBounty;
		private System.Windows.Forms.Label label18;
	}
}