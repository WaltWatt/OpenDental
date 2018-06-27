namespace OpenDental{
	partial class FormReleaseCalculator {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReleaseCalculator));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panelExtra = new System.Windows.Forms.Panel();
			this.listEngNoJobs = new System.Windows.Forms.ListBox();
			this.label15 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.butJob1 = new OpenDental.UI.Button();
			this.butJob3 = new OpenDental.UI.Button();
			this.butJob2 = new OpenDental.UI.Button();
			this.label12 = new System.Windows.Forms.Label();
			this.labelJobHours = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.labelRatioHours = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.labelAfterBreak = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.labelEngHours = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.labelJobNumber = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.labelReleaseDate = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.gridCalculatedJobs = new OpenDental.UI.ODGrid();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label7 = new System.Windows.Forms.Label();
			this.listEngineers = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textBreakHours = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.butCalculate = new OpenDental.UI.Button();
			this.textEngJobPercent = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textAvgJobHours = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.listPhases = new System.Windows.Forms.ListBox();
			this.label5 = new System.Windows.Forms.Label();
			this.listCategories = new System.Windows.Forms.ListBox();
			this.label4 = new System.Windows.Forms.Label();
			this.listPriorities = new System.Windows.Forms.ListBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panelExtra.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.53846F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.46154F));
			this.tableLayoutPanel1.Controls.Add(this.tabControl1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(868, 392);
			this.tableLayoutPanel1.TabIndex = 9;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(537, 3);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(328, 386);
			this.tabControl1.TabIndex = 26;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.panel1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(320, 360);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Calculations";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.panelExtra);
			this.panel1.Controls.Add(this.label12);
			this.panel1.Controls.Add(this.labelJobHours);
			this.panel1.Controls.Add(this.label13);
			this.panel1.Controls.Add(this.labelRatioHours);
			this.panel1.Controls.Add(this.label11);
			this.panel1.Controls.Add(this.labelAfterBreak);
			this.panel1.Controls.Add(this.label9);
			this.panel1.Controls.Add(this.labelEngHours);
			this.panel1.Controls.Add(this.label10);
			this.panel1.Controls.Add(this.labelJobNumber);
			this.panel1.Controls.Add(this.label8);
			this.panel1.Controls.Add(this.labelReleaseDate);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(314, 354);
			this.panel1.TabIndex = 0;
			// 
			// panelExtra
			// 
			this.panelExtra.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panelExtra.Controls.Add(this.listEngNoJobs);
			this.panelExtra.Controls.Add(this.label15);
			this.panelExtra.Controls.Add(this.label14);
			this.panelExtra.Controls.Add(this.butJob1);
			this.panelExtra.Controls.Add(this.butJob3);
			this.panelExtra.Controls.Add(this.butJob2);
			this.panelExtra.Location = new System.Drawing.Point(3, 133);
			this.panelExtra.Name = "panelExtra";
			this.panelExtra.Size = new System.Drawing.Size(309, 224);
			this.panelExtra.TabIndex = 25;
			this.panelExtra.Visible = false;
			// 
			// listEngNoJobs
			// 
			this.listEngNoJobs.FormattingEnabled = true;
			this.listEngNoJobs.Location = new System.Drawing.Point(69, 135);
			this.listEngNoJobs.Name = "listEngNoJobs";
			this.listEngNoJobs.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listEngNoJobs.Size = new System.Drawing.Size(169, 82);
			this.listEngNoJobs.TabIndex = 23;
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(66, 119);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(165, 13);
			this.label15.TabIndex = 24;
			this.label15.Text = "Engineers with no calculated jobs";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(34, 9);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(206, 13);
			this.label14.TabIndex = 18;
			this.label14.Text = "Top 3 JobNums with most remaining hours";
			// 
			// butJob1
			// 
			this.butJob1.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butJob1.Autosize = true;
			this.butJob1.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butJob1.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butJob1.CornerRadius = 4F;
			this.butJob1.Location = new System.Drawing.Point(114, 25);
			this.butJob1.Name = "butJob1";
			this.butJob1.Size = new System.Drawing.Size(84, 23);
			this.butJob1.TabIndex = 20;
			this.butJob1.Text = "job1";
			this.butJob1.UseVisualStyleBackColor = true;
			this.butJob1.Click += new System.EventHandler(this.butJob1_Click);
			// 
			// butJob3
			// 
			this.butJob3.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butJob3.Autosize = true;
			this.butJob3.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butJob3.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butJob3.CornerRadius = 4F;
			this.butJob3.Location = new System.Drawing.Point(114, 83);
			this.butJob3.Name = "butJob3";
			this.butJob3.Size = new System.Drawing.Size(84, 23);
			this.butJob3.TabIndex = 22;
			this.butJob3.Text = "job3";
			this.butJob3.UseVisualStyleBackColor = true;
			this.butJob3.Click += new System.EventHandler(this.butJob3_Click);
			// 
			// butJob2
			// 
			this.butJob2.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butJob2.Autosize = true;
			this.butJob2.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butJob2.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butJob2.CornerRadius = 4F;
			this.butJob2.Location = new System.Drawing.Point(114, 54);
			this.butJob2.Name = "butJob2";
			this.butJob2.Size = new System.Drawing.Size(84, 23);
			this.butJob2.TabIndex = 21;
			this.butJob2.Text = "job2";
			this.butJob2.UseVisualStyleBackColor = true;
			this.butJob2.Click += new System.EventHandler(this.butJob2_Click);
			// 
			// label12
			// 
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.Location = new System.Drawing.Point(3, 88);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(311, 24);
			this.label12.TabIndex = 16;
			this.label12.Text = "Estimated Release Date";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelJobHours
			// 
			this.labelJobHours.AutoSize = true;
			this.labelJobHours.Location = new System.Drawing.Point(259, 29);
			this.labelJobHours.Name = "labelJobHours";
			this.labelJobHours.Size = new System.Drawing.Size(13, 13);
			this.labelJobHours.TabIndex = 15;
			this.labelJobHours.Text = "0";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(195, 29);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(58, 13);
			this.label13.TabIndex = 14;
			this.label13.Text = "Job Hours:";
			// 
			// labelRatioHours
			// 
			this.labelRatioHours.AutoSize = true;
			this.labelRatioHours.Location = new System.Drawing.Point(112, 55);
			this.labelRatioHours.Name = "labelRatioHours";
			this.labelRatioHours.Size = new System.Drawing.Size(13, 13);
			this.labelRatioHours.TabIndex = 13;
			this.labelRatioHours.Text = "0";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(3, 55);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(103, 13);
			this.label11.TabIndex = 12;
			this.label11.Text = "Hours After Percent:";
			// 
			// labelAfterBreak
			// 
			this.labelAfterBreak.AutoSize = true;
			this.labelAfterBreak.Location = new System.Drawing.Point(112, 29);
			this.labelAfterBreak.Name = "labelAfterBreak";
			this.labelAfterBreak.Size = new System.Drawing.Size(13, 13);
			this.labelAfterBreak.TabIndex = 11;
			this.labelAfterBreak.Text = "0";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(6, 29);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(100, 13);
			this.label9.TabIndex = 10;
			this.label9.Text = "Hours Minus Break:";
			// 
			// labelEngHours
			// 
			this.labelEngHours.AutoSize = true;
			this.labelEngHours.Location = new System.Drawing.Point(112, 3);
			this.labelEngHours.Name = "labelEngHours";
			this.labelEngHours.Size = new System.Drawing.Size(13, 13);
			this.labelEngHours.TabIndex = 9;
			this.labelEngHours.Text = "0";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(23, 3);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(83, 13);
			this.label10.TabIndex = 8;
			this.label10.Text = "Engineer Hours:";
			// 
			// labelJobNumber
			// 
			this.labelJobNumber.AutoSize = true;
			this.labelJobNumber.Location = new System.Drawing.Point(259, 3);
			this.labelJobNumber.Name = "labelJobNumber";
			this.labelJobNumber.Size = new System.Drawing.Size(13, 13);
			this.labelJobNumber.TabIndex = 7;
			this.labelJobNumber.Text = "0";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(195, 3);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(54, 13);
			this.label8.TabIndex = 6;
			this.label8.Text = "# of Jobs:";
			// 
			// labelReleaseDate
			// 
			this.labelReleaseDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelReleaseDate.Location = new System.Drawing.Point(3, 112);
			this.labelReleaseDate.Name = "labelReleaseDate";
			this.labelReleaseDate.Size = new System.Drawing.Size(309, 18);
			this.labelReleaseDate.TabIndex = 5;
			this.labelReleaseDate.Text = "releaseDate";
			this.labelReleaseDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelReleaseDate.Visible = false;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.gridCalculatedJobs);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(320, 360);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Jobs";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// gridCalculatedJobs
			// 
			this.gridCalculatedJobs.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridCalculatedJobs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridCalculatedJobs.HasAddButton = false;
			this.gridCalculatedJobs.HasDropDowns = false;
			this.gridCalculatedJobs.HasMultilineHeaders = false;
			this.gridCalculatedJobs.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridCalculatedJobs.HeaderHeight = 15;
			this.gridCalculatedJobs.HScrollVisible = false;
			this.gridCalculatedJobs.Location = new System.Drawing.Point(3, 3);
			this.gridCalculatedJobs.Name = "gridCalculatedJobs";
			this.gridCalculatedJobs.ScrollValue = 0;
			this.gridCalculatedJobs.Size = new System.Drawing.Size(314, 354);
			this.gridCalculatedJobs.TabIndex = 0;
			this.gridCalculatedJobs.Title = "";
			this.gridCalculatedJobs.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridCalculatedJobs.TitleHeight = 0;
			this.gridCalculatedJobs.TranslationName = "Jobs";
			this.gridCalculatedJobs.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridCalculatedJobs_CellDoubleClick);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.label7);
			this.panel2.Controls.Add(this.listEngineers);
			this.panel2.Controls.Add(this.label3);
			this.panel2.Controls.Add(this.textBreakHours);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Controls.Add(this.butCalculate);
			this.panel2.Controls.Add(this.textEngJobPercent);
			this.panel2.Controls.Add(this.label6);
			this.panel2.Controls.Add(this.textAvgJobHours);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.listPhases);
			this.panel2.Controls.Add(this.label5);
			this.panel2.Controls.Add(this.listCategories);
			this.panel2.Controls.Add(this.label4);
			this.panel2.Controls.Add(this.listPriorities);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(3, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(528, 386);
			this.panel2.TabIndex = 20;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(7, 58);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(88, 13);
			this.label7.TabIndex = 19;
			this.label7.Text = "Avg Break Hours";
			// 
			// listEngineers
			// 
			this.listEngineers.FormattingEnabled = true;
			this.listEngineers.Location = new System.Drawing.Point(9, 112);
			this.listEngineers.Name = "listEngineers";
			this.listEngineers.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listEngineers.Size = new System.Drawing.Size(170, 225);
			this.listEngineers.TabIndex = 12;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 96);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(54, 13);
			this.label3.TabIndex = 14;
			this.label3.Text = "Engineers";
			// 
			// textBreakHours
			// 
			this.textBreakHours.Location = new System.Drawing.Point(101, 55);
			this.textBreakHours.Name = "textBreakHours";
			this.textBreakHours.Size = new System.Drawing.Size(46, 20);
			this.textBreakHours.TabIndex = 18;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(31, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Job Percent";
			// 
			// butCalculate
			// 
			this.butCalculate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCalculate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.butCalculate.Autosize = false;
			this.butCalculate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCalculate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCalculate.CornerRadius = 4F;
			this.butCalculate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butCalculate.Location = new System.Drawing.Point(140, 341);
			this.butCalculate.Name = "butCalculate";
			this.butCalculate.Size = new System.Drawing.Size(215, 36);
			this.butCalculate.TabIndex = 4;
			this.butCalculate.Text = "&Calculate";
			this.butCalculate.Click += new System.EventHandler(this.butCalculate_Click);
			// 
			// textEngJobPercent
			// 
			this.textEngJobPercent.Location = new System.Drawing.Point(101, 3);
			this.textEngJobPercent.Name = "textEngJobPercent";
			this.textEngJobPercent.Size = new System.Drawing.Size(46, 20);
			this.textEngJobPercent.TabIndex = 6;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(177, 3);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(46, 13);
			this.label6.TabIndex = 17;
			this.label6.Text = "Priorities";
			// 
			// textAvgJobHours
			// 
			this.textAvgJobHours.Location = new System.Drawing.Point(101, 29);
			this.textAvgJobHours.Name = "textAvgJobHours";
			this.textAvgJobHours.Size = new System.Drawing.Size(46, 20);
			this.textAvgJobHours.TabIndex = 8;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(18, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(77, 13);
			this.label2.TabIndex = 9;
			this.label2.Text = "Avg Job Hours";
			// 
			// listPhases
			// 
			this.listPhases.FormattingEnabled = true;
			this.listPhases.Location = new System.Drawing.Point(181, 112);
			this.listPhases.Name = "listPhases";
			this.listPhases.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listPhases.Size = new System.Drawing.Size(170, 225);
			this.listPhases.TabIndex = 10;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(350, 96);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(57, 13);
			this.label5.TabIndex = 16;
			this.label5.Text = "Categories";
			// 
			// listCategories
			// 
			this.listCategories.FormattingEnabled = true;
			this.listCategories.Location = new System.Drawing.Point(353, 112);
			this.listCategories.Name = "listCategories";
			this.listCategories.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listCategories.Size = new System.Drawing.Size(170, 225);
			this.listCategories.TabIndex = 11;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(178, 96);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(42, 13);
			this.label4.TabIndex = 15;
			this.label4.Text = "Phases";
			// 
			// listPriorities
			// 
			this.listPriorities.FormattingEnabled = true;
			this.listPriorities.Location = new System.Drawing.Point(229, 3);
			this.listPriorities.Name = "listPriorities";
			this.listPriorities.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listPriorities.Size = new System.Drawing.Size(162, 82);
			this.listPriorities.TabIndex = 13;
			// 
			// FormReleaseCalculator
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(868, 392);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormReleaseCalculator";
			this.Text = "Release Calculator";
			this.Load += new System.EventHandler(this.FormReleaseCalculator_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panelExtra.ResumeLayout(false);
			this.panelExtra.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TextBox textEngJobPercent;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textAvgJobHours;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textBreakHours;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label labelJobHours;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label labelRatioHours;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label labelAfterBreak;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label labelEngHours;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label labelJobNumber;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label labelReleaseDate;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ListBox listEngineers;
		private System.Windows.Forms.Label label3;
		private UI.Button butCalculate;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ListBox listPhases;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ListBox listCategories;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListBox listPriorities;
		private System.Windows.Forms.Panel panelExtra;
		private System.Windows.Forms.ListBox listEngNoJobs;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label14;
		private UI.Button butJob1;
		private UI.Button butJob3;
		private UI.Button butJob2;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private UI.ODGrid gridCalculatedJobs;
	}
}