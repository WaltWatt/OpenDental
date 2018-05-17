namespace OpenDental{
	partial class FormPhoneGraphDateEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPhoneGraphDateEdit));
			this.butCancel = new OpenDental.UI.Button();
			this.gridGraph = new OpenDental.UI.ODGrid();
			this.label11 = new System.Windows.Forms.Label();
			this.butEditSchedule = new OpenDental.UI.Button();
			this.groupGraphPrefs = new System.Windows.Forms.GroupBox();
			this.labelSave = new System.Windows.Forms.Label();
			this.butPrefUpdate = new OpenDental.UI.Button();
			this.textSuperPeak = new System.Windows.Forms.TextBox();
			this.textPeak = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupGraphPrefs.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
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
			this.butCancel.Location = new System.Drawing.Point(483, 556);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Close";
			this.butCancel.Click += new System.EventHandler(this.butClose_Click);
			// 
			// gridGraph
			// 
			this.gridGraph.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridGraph.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridGraph.HasAddButton = false;
			this.gridGraph.HasDropDowns = false;
			this.gridGraph.HasMultilineHeaders = false;
			this.gridGraph.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridGraph.HeaderHeight = 15;
			this.gridGraph.HScrollVisible = false;
			this.gridGraph.Location = new System.Drawing.Point(0, 19);
			this.gridGraph.Name = "gridGraph";
			this.gridGraph.ScrollValue = 0;
			this.gridGraph.Size = new System.Drawing.Size(540, 471);
			this.gridGraph.TabIndex = 48;
			this.gridGraph.Title = "";
			this.gridGraph.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridGraph.TitleHeight = 18;
			this.gridGraph.TranslationName = "TablePhoneGraphDate";
			this.gridGraph.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridGraph_CellClick);
			// 
			// label11
			// 
			this.label11.Dock = System.Windows.Forms.DockStyle.Top;
			this.label11.Location = new System.Drawing.Point(0, 0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(540, 19);
			this.label11.TabIndex = 49;
			this.label11.Text = "Click \'Set Graph Status\' column for a given employee to create an override for th" +
    "is date.";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butEditSchedule
			// 
			this.butEditSchedule.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEditSchedule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butEditSchedule.Autosize = true;
			this.butEditSchedule.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditSchedule.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditSchedule.CornerRadius = 4F;
			this.butEditSchedule.Location = new System.Drawing.Point(12, 556);
			this.butEditSchedule.Name = "butEditSchedule";
			this.butEditSchedule.Size = new System.Drawing.Size(92, 24);
			this.butEditSchedule.TabIndex = 50;
			this.butEditSchedule.Text = "&Edit Schedule";
			this.butEditSchedule.Click += new System.EventHandler(this.butEditSchedule_Click);
			// 
			// groupGraphPrefs
			// 
			this.groupGraphPrefs.Controls.Add(this.labelSave);
			this.groupGraphPrefs.Controls.Add(this.butPrefUpdate);
			this.groupGraphPrefs.Controls.Add(this.textSuperPeak);
			this.groupGraphPrefs.Controls.Add(this.textPeak);
			this.groupGraphPrefs.Controls.Add(this.label2);
			this.groupGraphPrefs.Controls.Add(this.label1);
			this.groupGraphPrefs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupGraphPrefs.Location = new System.Drawing.Point(3, 3);
			this.groupGraphPrefs.Name = "groupGraphPrefs";
			this.groupGraphPrefs.Size = new System.Drawing.Size(540, 47);
			this.groupGraphPrefs.TabIndex = 51;
			this.groupGraphPrefs.TabStop = false;
			this.groupGraphPrefs.Text = "Graph Time Preferences";
			// 
			// labelSave
			// 
			this.labelSave.Location = new System.Drawing.Point(425, 17);
			this.labelSave.Name = "labelSave";
			this.labelSave.Size = new System.Drawing.Size(67, 23);
			this.labelSave.TabIndex = 52;
			this.labelSave.Text = "Prefs saved.";
			this.labelSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelSave.Visible = false;
			// 
			// butPrefUpdate
			// 
			this.butPrefUpdate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrefUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrefUpdate.Autosize = true;
			this.butPrefUpdate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrefUpdate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrefUpdate.CornerRadius = 4F;
			this.butPrefUpdate.Location = new System.Drawing.Point(360, 16);
			this.butPrefUpdate.Name = "butPrefUpdate";
			this.butPrefUpdate.Size = new System.Drawing.Size(59, 24);
			this.butPrefUpdate.TabIndex = 51;
			this.butPrefUpdate.Text = "&Update";
			this.butPrefUpdate.Click += new System.EventHandler(this.butPrefUpdate_Click);
			// 
			// textSuperPeak
			// 
			this.textSuperPeak.Location = new System.Drawing.Point(240, 18);
			this.textSuperPeak.Name = "textSuperPeak";
			this.textSuperPeak.Size = new System.Drawing.Size(100, 20);
			this.textSuperPeak.TabIndex = 3;
			// 
			// textPeak
			// 
			this.textPeak.Location = new System.Drawing.Point(53, 18);
			this.textPeak.Name = "textPeak";
			this.textPeak.Size = new System.Drawing.Size(100, 20);
			this.textPeak.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(159, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(75, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "Super Peak";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Peak";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.groupGraphPrefs, 0, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 1);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(546, 549);
			this.tableLayoutPanel1.TabIndex = 52;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.gridGraph);
			this.panel1.Controls.Add(this.label11);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 56);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(540, 490);
			this.panel1.TabIndex = 0;
			// 
			// FormPhoneGraphDateEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(570, 592);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.butEditSchedule);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormPhoneGraphDateEdit";
			this.Text = "Phone Graph Edits";
			this.Load += new System.EventHandler(this.FormPhoneGraphDateEdit_Load);
			this.groupGraphPrefs.ResumeLayout(false);
			this.groupGraphPrefs.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butCancel;
		private UI.ODGrid gridGraph;
		private System.Windows.Forms.Label label11;
		private UI.Button butEditSchedule;
		private System.Windows.Forms.GroupBox groupGraphPrefs;
		private System.Windows.Forms.TextBox textSuperPeak;
		private System.Windows.Forms.TextBox textPeak;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel1;
		private UI.Button butPrefUpdate;
		private System.Windows.Forms.Label labelSave;
	}
}