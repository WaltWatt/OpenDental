using System;
using System.Windows.Forms;

namespace OpenDental{
	partial class FormCreditRecurringCharges {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCreditRecurringCharges));
			this.checkHideBold = new System.Windows.Forms.CheckBox();
			this.groupCounts = new System.Windows.Forms.GroupBox();
			this.labelUpdated = new System.Windows.Forms.Label();
			this.labelFailed = new System.Windows.Forms.Label();
			this.labelCharged = new System.Windows.Forms.Label();
			this.labelSelected = new System.Windows.Forms.Label();
			this.labelTotal = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.groupClinics = new System.Windows.Forms.GroupBox();
			this.checkAllClin = new System.Windows.Forms.CheckBox();
			this.listClinics = new System.Windows.Forms.ListBox();
			this.butPrintList = new OpenDental.UI.Button();
			this.butNone = new OpenDental.UI.Button();
			this.butAll = new OpenDental.UI.Button();
			this.butRefresh = new OpenDental.UI.Button();
			this.butSend = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.textDate = new OpenDental.ValidDate();
			this.butToday = new OpenDental.UI.Button();
			this.groupDateFilter = new System.Windows.Forms.GroupBox();
			this.checkForceDuplicates = new System.Windows.Forms.CheckBox();
			this.groupCounts.SuspendLayout();
			this.groupClinics.SuspendLayout();
			this.groupDateFilter.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkHideBold
			// 
			this.checkHideBold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkHideBold.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkHideBold.Location = new System.Drawing.Point(799, 98);
			this.checkHideBold.Name = "checkHideBold";
			this.checkHideBold.Size = new System.Drawing.Size(91, 18);
			this.checkHideBold.TabIndex = 55;
			this.checkHideBold.Text = "Hide Bold";
			this.checkHideBold.UseVisualStyleBackColor = true;
			this.checkHideBold.Click += new System.EventHandler(this.checkHideBold_Click);
			// 
			// groupCounts
			// 
			this.groupCounts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupCounts.Controls.Add(this.labelUpdated);
			this.groupCounts.Controls.Add(this.labelFailed);
			this.groupCounts.Controls.Add(this.labelCharged);
			this.groupCounts.Controls.Add(this.labelSelected);
			this.groupCounts.Controls.Add(this.labelTotal);
			this.groupCounts.Location = new System.Drawing.Point(799, 371);
			this.groupCounts.Name = "groupCounts";
			this.groupCounts.Size = new System.Drawing.Size(96, 115);
			this.groupCounts.TabIndex = 34;
			this.groupCounts.TabStop = false;
			this.groupCounts.Text = "Counts";
			// 
			// labelUpdated
			// 
			this.labelUpdated.Location = new System.Drawing.Point(6, 73);
			this.labelUpdated.Name = "labelUpdated";
			this.labelUpdated.Size = new System.Drawing.Size(84, 16);
			this.labelUpdated.TabIndex = 33;
			this.labelUpdated.Text = "Updated=0";
			this.labelUpdated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelFailed
			// 
			this.labelFailed.Location = new System.Drawing.Point(6, 92);
			this.labelFailed.Name = "labelFailed";
			this.labelFailed.Size = new System.Drawing.Size(84, 16);
			this.labelFailed.TabIndex = 32;
			this.labelFailed.Text = "Failed=0";
			this.labelFailed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelCharged
			// 
			this.labelCharged.Location = new System.Drawing.Point(6, 54);
			this.labelCharged.Name = "labelCharged";
			this.labelCharged.Size = new System.Drawing.Size(84, 16);
			this.labelCharged.TabIndex = 31;
			this.labelCharged.Text = "Charged=0";
			this.labelCharged.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelSelected
			// 
			this.labelSelected.Location = new System.Drawing.Point(6, 35);
			this.labelSelected.Name = "labelSelected";
			this.labelSelected.Size = new System.Drawing.Size(84, 16);
			this.labelSelected.TabIndex = 30;
			this.labelSelected.Text = "Selected=0";
			this.labelSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelTotal
			// 
			this.labelTotal.Location = new System.Drawing.Point(6, 16);
			this.labelTotal.Name = "labelTotal";
			this.labelTotal.Size = new System.Drawing.Size(84, 16);
			this.labelTotal.TabIndex = 29;
			this.labelTotal.Text = "Total=0";
			this.labelTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.AutoScroll = true;
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(769, 474);
			this.gridMain.TabIndex = 29;
			this.gridMain.Title = "Recurring Charges";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableRecurring";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// groupClinics
			// 
			this.groupClinics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupClinics.Controls.Add(this.checkAllClin);
			this.groupClinics.Controls.Add(this.listClinics);
			this.groupClinics.Location = new System.Drawing.Point(793, 146);
			this.groupClinics.Name = "groupClinics";
			this.groupClinics.Size = new System.Drawing.Size(169, 219);
			this.groupClinics.TabIndex = 52;
			this.groupClinics.TabStop = false;
			this.groupClinics.Text = "Clinic Filter";
			// 
			// checkAllClin
			// 
			this.checkAllClin.Checked = true;
			this.checkAllClin.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkAllClin.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllClin.Location = new System.Drawing.Point(6, 20);
			this.checkAllClin.Name = "checkAllClin";
			this.checkAllClin.Size = new System.Drawing.Size(91, 16);
			this.checkAllClin.TabIndex = 60;
			this.checkAllClin.Text = "All";
			this.checkAllClin.Click += new System.EventHandler(this.checkAllClin_Click);
			// 
			// listClinics
			// 
			this.listClinics.Location = new System.Drawing.Point(6, 39);
			this.listClinics.Name = "listClinics";
			this.listClinics.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listClinics.Size = new System.Drawing.Size(157, 173);
			this.listClinics.TabIndex = 65;
			this.listClinics.SelectedIndexChanged += new System.EventHandler(this.listClinics_SelectedIndexChanged);
			// 
			// butPrintList
			// 
			this.butPrintList.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrintList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrintList.Autosize = true;
			this.butPrintList.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrintList.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrintList.CornerRadius = 4F;
			this.butPrintList.Image = global::OpenDental.Properties.Resources.butPrint;
			this.butPrintList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrintList.Location = new System.Drawing.Point(377, 498);
			this.butPrintList.Name = "butPrintList";
			this.butPrintList.Size = new System.Drawing.Size(88, 24);
			this.butPrintList.TabIndex = 80;
			this.butPrintList.Text = "Print List";
			this.butPrintList.Click += new System.EventHandler(this.butPrintList_Click);
			// 
			// butNone
			// 
			this.butNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butNone.Autosize = true;
			this.butNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNone.CornerRadius = 4F;
			this.butNone.Location = new System.Drawing.Point(102, 498);
			this.butNone.Name = "butNone";
			this.butNone.Size = new System.Drawing.Size(75, 24);
			this.butNone.TabIndex = 75;
			this.butNone.Text = "&None";
			this.butNone.Click += new System.EventHandler(this.butNone_Click);
			// 
			// butAll
			// 
			this.butAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAll.Autosize = true;
			this.butAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAll.CornerRadius = 4F;
			this.butAll.Location = new System.Drawing.Point(12, 498);
			this.butAll.Name = "butAll";
			this.butAll.Size = new System.Drawing.Size(75, 24);
			this.butAll.TabIndex = 70;
			this.butAll.Text = "&All";
			this.butAll.Click += new System.EventHandler(this.butAll_Click);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(837, 12);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(81, 24);
			this.butRefresh.TabIndex = 40;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// butSend
			// 
			this.butSend.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSend.Autosize = true;
			this.butSend.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSend.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSend.CornerRadius = 4F;
			this.butSend.Location = new System.Drawing.Point(799, 498);
			this.butSend.Name = "butSend";
			this.butSend.Size = new System.Drawing.Size(75, 24);
			this.butSend.TabIndex = 1;
			this.butSend.Text = "&Send";
			this.butSend.Click += new System.EventHandler(this.butSend_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(887, 498);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "Close";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(6, 21);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(96, 20);
			this.textDate.TabIndex = 45;
			this.textDate.TextChanged += new System.EventHandler(this.textDate_TextChanged);
			// 
			// butToday
			// 
			this.butToday.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butToday.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butToday.Autosize = true;
			this.butToday.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butToday.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butToday.CornerRadius = 4F;
			this.butToday.Location = new System.Drawing.Point(104, 19);
			this.butToday.Name = "butToday";
			this.butToday.Size = new System.Drawing.Size(75, 24);
			this.butToday.TabIndex = 50;
			this.butToday.Text = "&Today";
			this.butToday.Click += new System.EventHandler(this.butToday_Click);
			// 
			// groupDateFilter
			// 
			this.groupDateFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupDateFilter.Controls.Add(this.butToday);
			this.groupDateFilter.Controls.Add(this.textDate);
			this.groupDateFilter.Location = new System.Drawing.Point(787, 40);
			this.groupDateFilter.Name = "groupDateFilter";
			this.groupDateFilter.Size = new System.Drawing.Size(183, 54);
			this.groupDateFilter.TabIndex = 35;
			this.groupDateFilter.TabStop = false;
			this.groupDateFilter.Text = "Date Filter";
			// 
			// checkForceDuplicates
			// 
			this.checkForceDuplicates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkForceDuplicates.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkForceDuplicates.Location = new System.Drawing.Point(799, 122);
			this.checkForceDuplicates.Name = "checkForceDuplicates";
			this.checkForceDuplicates.Size = new System.Drawing.Size(164, 18);
			this.checkForceDuplicates.TabIndex = 81;
			this.checkForceDuplicates.Text = "Force Duplicates";
			this.checkForceDuplicates.UseVisualStyleBackColor = true;
			// 
			// FormCreditRecurringCharges
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(974, 534);
			this.Controls.Add(this.checkForceDuplicates);
			this.Controls.Add(this.groupDateFilter);
			this.Controls.Add(this.checkHideBold);
			this.Controls.Add(this.butPrintList);
			this.Controls.Add(this.butNone);
			this.Controls.Add(this.butAll);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.groupCounts);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butSend);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.groupClinics);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(542, 416);
			this.Name = "FormCreditRecurringCharges";
			this.Text = "Credit Card Recurring Charges";
			this.Load += new System.EventHandler(this.FormRecurringCharges_Load);
			this.groupCounts.ResumeLayout(false);
			this.groupClinics.ResumeLayout(false);
			this.groupDateFilter.ResumeLayout(false);
			this.groupDateFilter.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butSend;
		private OpenDental.UI.Button butCancel;
		private UI.ODGrid gridMain;
		private System.Windows.Forms.GroupBox groupCounts;
		private System.Windows.Forms.Label labelFailed;
		private System.Windows.Forms.Label labelCharged;
		private System.Windows.Forms.Label labelSelected;
		private System.Windows.Forms.Label labelTotal;
		private UI.Button butRefresh;
		private UI.Button butNone;
		private UI.Button butAll;
		private UI.Button butPrintList;
		private System.Windows.Forms.CheckBox checkHideBold;
		private System.Windows.Forms.Label labelUpdated;
		private System.Windows.Forms.CheckBox checkAllClin;
		private System.Windows.Forms.ListBox listClinics;
		private System.Windows.Forms.GroupBox groupClinics;
		private ValidDate textDate;
		private UI.Button butToday;
		private GroupBox groupDateFilter;
		private CheckBox checkForceDuplicates;
	}
}