namespace OpenDental{
	partial class FormEtrans270Edit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEtrans270Edit));
			this.labelNote = new System.Windows.Forms.Label();
			this.textNote = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.butShowResponse = new OpenDental.UI.Button();
			this.butShowRequest = new OpenDental.UI.Button();
			this.labelImport = new System.Windows.Forms.Label();
			this.gridBen = new OpenDental.UI.ODGrid();
			this.gridDates = new OpenDental.UI.ODGrid();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.groupImport = new System.Windows.Forms.GroupBox();
			this.radioInNetwork = new System.Windows.Forms.RadioButton();
			this.radioOutNetwork = new System.Windows.Forms.RadioButton();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.radioModeElect = new System.Windows.Forms.RadioButton();
			this.radioModeMessage = new System.Windows.Forms.RadioButton();
			this.butPrint = new OpenDental.UI.Button();
			this.butImport = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupImportBenefit = new System.Windows.Forms.GroupBox();
			this.radioBenefitSendsPat = new System.Windows.Forms.RadioButton();
			this.radioBenefitSendsIns = new System.Windows.Forms.RadioButton();
			this.groupBox2.SuspendLayout();
			this.groupImport.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupImportBenefit.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelNote
			// 
			this.labelNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelNote.Location = new System.Drawing.Point(6, 580);
			this.labelNote.Name = "labelNote";
			this.labelNote.Size = new System.Drawing.Size(100, 17);
			this.labelNote.TabIndex = 15;
			this.labelNote.Text = "Note";
			this.labelNote.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textNote
			// 
			this.textNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textNote.Location = new System.Drawing.Point(9, 600);
			this.textNote.Multiline = true;
			this.textNote.Name = "textNote";
			this.textNote.Size = new System.Drawing.Size(355, 40);
			this.textNote.TabIndex = 14;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.butShowResponse);
			this.groupBox2.Controls.Add(this.butShowRequest);
			this.groupBox2.Location = new System.Drawing.Point(797, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(168, 49);
			this.groupBox2.TabIndex = 116;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Show Raw Message of...";
			// 
			// butShowResponse
			// 
			this.butShowResponse.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butShowResponse.Autosize = true;
			this.butShowResponse.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShowResponse.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShowResponse.CornerRadius = 4F;
			this.butShowResponse.Location = new System.Drawing.Point(87, 19);
			this.butShowResponse.Name = "butShowResponse";
			this.butShowResponse.Size = new System.Drawing.Size(75, 24);
			this.butShowResponse.TabIndex = 116;
			this.butShowResponse.Text = "Response";
			this.butShowResponse.Click += new System.EventHandler(this.butShowResponse_Click);
			// 
			// butShowRequest
			// 
			this.butShowRequest.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butShowRequest.Autosize = true;
			this.butShowRequest.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butShowRequest.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butShowRequest.CornerRadius = 4F;
			this.butShowRequest.Location = new System.Drawing.Point(6, 19);
			this.butShowRequest.Name = "butShowRequest";
			this.butShowRequest.Size = new System.Drawing.Size(75, 24);
			this.butShowRequest.TabIndex = 115;
			this.butShowRequest.Text = "Request";
			this.butShowRequest.Click += new System.EventHandler(this.butShowRequest_Click);
			// 
			// labelImport
			// 
			this.labelImport.Location = new System.Drawing.Point(445, 420);
			this.labelImport.Name = "labelImport";
			this.labelImport.Size = new System.Drawing.Size(133, 17);
			this.labelImport.TabIndex = 120;
			this.labelImport.Text = "Selected Benefits";
			this.labelImport.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// gridBen
			// 
			this.gridBen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridBen.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridBen.HasAddButton = false;
			this.gridBen.HasDropDowns = false;
			this.gridBen.HasMultilineHeaders = false;
			this.gridBen.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridBen.HeaderHeight = 15;
			this.gridBen.HScrollVisible = false;
			this.gridBen.Location = new System.Drawing.Point(582, 393);
			this.gridBen.Name = "gridBen";
			this.gridBen.ScrollValue = 0;
			this.gridBen.Size = new System.Drawing.Size(383, 246);
			this.gridBen.TabIndex = 118;
			this.gridBen.Title = "Current Benefits";
			this.gridBen.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridBen.TitleHeight = 18;
			this.gridBen.TranslationName = "FormEtrans270Edit";
			this.gridBen.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridBen_CellDoubleClick);
			// 
			// gridDates
			// 
			this.gridDates.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridDates.HasAddButton = false;
			this.gridDates.HasDropDowns = false;
			this.gridDates.HasMultilineHeaders = false;
			this.gridDates.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridDates.HeaderHeight = 15;
			this.gridDates.HScrollVisible = false;
			this.gridDates.Location = new System.Drawing.Point(9, 12);
			this.gridDates.Name = "gridDates";
			this.gridDates.ScrollValue = 0;
			this.gridDates.Size = new System.Drawing.Size(407, 119);
			this.gridDates.TabIndex = 117;
			this.gridDates.Title = "Dates";
			this.gridDates.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridDates.TitleHeight = 18;
			this.gridDates.TranslationName = "FormEtrans270Edit";
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
			this.gridMain.Location = new System.Drawing.Point(9, 137);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(956, 254);
			this.gridMain.TabIndex = 114;
			this.gridMain.Title = "Response Benefit Information";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "FormEtrans270Edit";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// groupImport
			// 
			this.groupImport.Controls.Add(this.radioInNetwork);
			this.groupImport.Controls.Add(this.radioOutNetwork);
			this.groupImport.Location = new System.Drawing.Point(470, 73);
			this.groupImport.Name = "groupImport";
			this.groupImport.Size = new System.Drawing.Size(167, 58);
			this.groupImport.TabIndex = 124;
			this.groupImport.TabStop = false;
			this.groupImport.Text = "Mark for import if";
			// 
			// radioInNetwork
			// 
			this.radioInNetwork.Checked = true;
			this.radioInNetwork.Location = new System.Drawing.Point(12, 16);
			this.radioInNetwork.Name = "radioInNetwork";
			this.radioInNetwork.Size = new System.Drawing.Size(121, 18);
			this.radioInNetwork.TabIndex = 121;
			this.radioInNetwork.TabStop = true;
			this.radioInNetwork.Text = "In network";
			this.radioInNetwork.UseVisualStyleBackColor = true;
			this.radioInNetwork.Click += new System.EventHandler(this.radioInNetwork_Click);
			// 
			// radioOutNetwork
			// 
			this.radioOutNetwork.Location = new System.Drawing.Point(12, 35);
			this.radioOutNetwork.Name = "radioOutNetwork";
			this.radioOutNetwork.Size = new System.Drawing.Size(121, 18);
			this.radioOutNetwork.TabIndex = 122;
			this.radioOutNetwork.Text = "Out of network";
			this.radioOutNetwork.UseVisualStyleBackColor = true;
			this.radioOutNetwork.Click += new System.EventHandler(this.radioOutNetwork_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.radioModeElect);
			this.groupBox3.Controls.Add(this.radioModeMessage);
			this.groupBox3.Location = new System.Drawing.Point(470, 12);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(167, 58);
			this.groupBox3.TabIndex = 126;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Viewing Mode";
			// 
			// radioModeElect
			// 
			this.radioModeElect.Checked = true;
			this.radioModeElect.Location = new System.Drawing.Point(12, 16);
			this.radioModeElect.Name = "radioModeElect";
			this.radioModeElect.Size = new System.Drawing.Size(121, 18);
			this.radioModeElect.TabIndex = 121;
			this.radioModeElect.TabStop = true;
			this.radioModeElect.Text = "Electronic Import";
			this.radioModeElect.UseVisualStyleBackColor = true;
			this.radioModeElect.Click += new System.EventHandler(this.radioModeElect_Click);
			// 
			// radioModeMessage
			// 
			this.radioModeMessage.Location = new System.Drawing.Point(12, 35);
			this.radioModeMessage.Name = "radioModeMessage";
			this.radioModeMessage.Size = new System.Drawing.Size(121, 18);
			this.radioModeMessage.TabIndex = 122;
			this.radioModeMessage.Text = "Message Text";
			this.radioModeMessage.UseVisualStyleBackColor = true;
			this.radioModeMessage.Click += new System.EventHandler(this.radioModeMessage_Click);
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Location = new System.Drawing.Point(644, 106);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(75, 24);
			this.butPrint.TabIndex = 127;
			this.butPrint.Text = "Print";
			this.butPrint.Visible = false;
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// butImport
			// 
			this.butImport.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butImport.Autosize = true;
			this.butImport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butImport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butImport.CornerRadius = 4F;
			this.butImport.Image = global::OpenDental.Properties.Resources.down;
			this.butImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butImport.Location = new System.Drawing.Point(497, 393);
			this.butImport.Name = "butImport";
			this.butImport.Size = new System.Drawing.Size(81, 24);
			this.butImport.TabIndex = 119;
			this.butImport.Text = "Import";
			this.butImport.Click += new System.EventHandler(this.butImport_Click);
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
			this.butDelete.Location = new System.Drawing.Point(9, 645);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(81, 24);
			this.butDelete.TabIndex = 113;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(809, 645);
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
			this.butCancel.Location = new System.Drawing.Point(890, 645);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// groupImportBenefit
			// 
			this.groupImportBenefit.Controls.Add(this.radioBenefitSendsPat);
			this.groupImportBenefit.Controls.Add(this.radioBenefitSendsIns);
			this.groupImportBenefit.Location = new System.Drawing.Point(9, 397);
			this.groupImportBenefit.Name = "groupImportBenefit";
			this.groupImportBenefit.Size = new System.Drawing.Size(200, 58);
			this.groupImportBenefit.TabIndex = 133;
			this.groupImportBenefit.TabStop = false;
			this.groupImportBenefit.Text = "Import Benefit Coinsurance";
			// 
			// radioBenefitSendsPat
			// 
			this.radioBenefitSendsPat.Location = new System.Drawing.Point(12, 16);
			this.radioBenefitSendsPat.Name = "radioBenefitSendsPat";
			this.radioBenefitSendsPat.Size = new System.Drawing.Size(182, 17);
			this.radioBenefitSendsPat.TabIndex = 1;
			this.radioBenefitSendsPat.TabStop = true;
			this.radioBenefitSendsPat.Text = "Carrier sends patient % (default)";
			this.radioBenefitSendsPat.UseVisualStyleBackColor = true;
			this.radioBenefitSendsPat.Click += new System.EventHandler(this.radioBenefitPerct_CheckedChanged);
			// 
			// radioBenefitSendsIns
			// 
			this.radioBenefitSendsIns.Location = new System.Drawing.Point(12, 35);
			this.radioBenefitSendsIns.Name = "radioBenefitSendsIns";
			this.radioBenefitSendsIns.Size = new System.Drawing.Size(182, 17);
			this.radioBenefitSendsIns.TabIndex = 0;
			this.radioBenefitSendsIns.TabStop = true;
			this.radioBenefitSendsIns.Text = "Carrier sends insurance %";
			this.radioBenefitSendsIns.UseVisualStyleBackColor = true;
			this.radioBenefitSendsIns.Click += new System.EventHandler(this.radioBenefitPerct_CheckedChanged);
			// 
			// FormEtrans270Edit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(974, 674);
			this.Controls.Add(this.groupImportBenefit);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupImport);
			this.Controls.Add(this.labelImport);
			this.Controls.Add(this.butImport);
			this.Controls.Add(this.gridBen);
			this.Controls.Add(this.gridDates);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.labelNote);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(990, 712);
			this.Name = "FormEtrans270Edit";
			this.Text = "Edit Electronic Benefit Request";
			this.Load += new System.EventHandler(this.FormEtrans270Edit_Load);
			this.Shown += new System.EventHandler(this.FormEtrans270Edit_Shown);
			this.groupBox2.ResumeLayout(false);
			this.groupImport.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupImportBenefit.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.Label labelNote;
		private System.Windows.Forms.TextBox textNote;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.ODGrid gridMain;
		private OpenDental.UI.Button butShowRequest;
		private System.Windows.Forms.GroupBox groupBox2;
		private OpenDental.UI.Button butShowResponse;
		private OpenDental.UI.ODGrid gridDates;
		private OpenDental.UI.ODGrid gridBen;
		private OpenDental.UI.Button butImport;
		private System.Windows.Forms.Label labelImport;
		private System.Windows.Forms.GroupBox groupImport;
		private System.Windows.Forms.RadioButton radioInNetwork;
		private System.Windows.Forms.RadioButton radioOutNetwork;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton radioModeElect;
		private System.Windows.Forms.RadioButton radioModeMessage;
		private OpenDental.UI.Button butPrint;
		private System.Windows.Forms.GroupBox groupImportBenefit;
		private System.Windows.Forms.RadioButton radioBenefitSendsPat;
		private System.Windows.Forms.RadioButton radioBenefitSendsIns;
	}
}