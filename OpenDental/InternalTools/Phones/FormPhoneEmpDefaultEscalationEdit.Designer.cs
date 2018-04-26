namespace OpenDental {
	partial class FormPhoneEmpDefaultEscalationEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPhoneEmpDefaultEscalationEdit));
			this.gridEscalation = new OpenDental.UI.ODGrid();
			this.gridEmployees = new OpenDental.UI.ODGrid();
			this.butUp = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butRight = new OpenDental.UI.Button();
			this.butLeft = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.labelAvail = new System.Windows.Forms.Label();
			this.tabMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// gridEscalation
			// 
			this.gridEscalation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridEscalation.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridEscalation.HasAddButton = false;
			this.gridEscalation.HasDropDowns = false;
			this.gridEscalation.HasMultilineHeaders = false;
			this.gridEscalation.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridEscalation.HeaderHeight = 15;
			this.gridEscalation.HScrollVisible = false;
			this.gridEscalation.Location = new System.Drawing.Point(247, 37);
			this.gridEscalation.Name = "gridEscalation";
			this.gridEscalation.ScrollValue = 0;
			this.gridEscalation.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridEscalation.Size = new System.Drawing.Size(175, 464);
			this.gridEscalation.TabIndex = 65;
			this.gridEscalation.Title = "Escalation Order";
			this.gridEscalation.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridEscalation.TitleHeight = 18;
			this.gridEscalation.TranslationName = "TableEscalation";
			// 
			// gridEmployees
			// 
			this.gridEmployees.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridEmployees.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridEmployees.HasAddButton = false;
			this.gridEmployees.HasDropDowns = false;
			this.gridEmployees.HasMultilineHeaders = false;
			this.gridEmployees.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridEmployees.HeaderHeight = 15;
			this.gridEmployees.HScrollVisible = false;
			this.gridEmployees.Location = new System.Drawing.Point(12, 37);
			this.gridEmployees.Name = "gridEmployees";
			this.gridEmployees.ScrollValue = 0;
			this.gridEmployees.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridEmployees.Size = new System.Drawing.Size(175, 464);
			this.gridEmployees.TabIndex = 62;
			this.gridEmployees.Title = "Employees";
			this.gridEmployees.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridEmployees.TitleHeight = 18;
			this.gridEmployees.TranslationName = "TableEmployee";
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(444, 195);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(71, 24);
			this.butUp.TabIndex = 67;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(444, 229);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(71, 24);
			this.butDown.TabIndex = 66;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butRight
			// 
			this.butRight.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRight.Autosize = true;
			this.butRight.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRight.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRight.CornerRadius = 4F;
			this.butRight.Image = global::OpenDental.Properties.Resources.Right;
			this.butRight.Location = new System.Drawing.Point(201, 194);
			this.butRight.Name = "butRight";
			this.butRight.Size = new System.Drawing.Size(35, 26);
			this.butRight.TabIndex = 64;
			this.butRight.Click += new System.EventHandler(this.butRight_Click);
			// 
			// butLeft
			// 
			this.butLeft.AdjustImageLocation = new System.Drawing.Point(-1, 0);
			this.butLeft.Autosize = true;
			this.butLeft.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLeft.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLeft.CornerRadius = 4F;
			this.butLeft.Image = global::OpenDental.Properties.Resources.Left;
			this.butLeft.Location = new System.Drawing.Point(201, 228);
			this.butLeft.Name = "butLeft";
			this.butLeft.Size = new System.Drawing.Size(35, 26);
			this.butLeft.TabIndex = 63;
			this.butLeft.Click += new System.EventHandler(this.butLeft_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(442, 447);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 13;
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
			this.butCancel.Location = new System.Drawing.Point(442, 477);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 14;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tabPage1);
			this.tabMain.Controls.Add(this.tabPage2);
			this.tabMain.Controls.Add(this.tabPage3);
			this.tabMain.Controls.Add(this.tabPage4);
			this.tabMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabMain.Location = new System.Drawing.Point(12, 9);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(410, 22);
			this.tabMain.TabIndex = 90;
			this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(402, 0);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(402, 0);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tabPage3
			// 
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(402, 0);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "tabPage3";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// tabPage4
			// 
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(402, 0);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "tabPage4";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// labelAvail
			// 
			this.labelAvail.Location = new System.Drawing.Point(428, 265);
			this.labelAvail.Name = "labelAvail";
			this.labelAvail.Size = new System.Drawing.Size(96, 139);
			this.labelAvail.TabIndex = 91;
			this.labelAvail.Text = "The Avail view is ordered by those in Available status sorted from most amount of" +
    " time in this status to least then by those in Train status then by those in Bac" +
    "kup status.";
			// 
			// FormPhoneEmpDefaultEscalationEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(529, 514);
			this.Controls.Add(this.labelAvail);
			this.Controls.Add(this.tabMain);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.gridEscalation);
			this.Controls.Add(this.butRight);
			this.Controls.Add(this.butLeft);
			this.Controls.Add(this.gridEmployees);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(545, 552);
			this.Name = "FormPhoneEmpDefaultEscalationEdit";
			this.Text = "Escalation Edit";
			this.tabMain.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private UI.Button butOK;
		private UI.Button butCancel;
		private UI.ODGrid gridEmployees;
		private UI.Button butRight;
		private UI.Button butLeft;
		private UI.ODGrid gridEscalation;
		private UI.Button butUp;
		private UI.Button butDown;
		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.Label labelAvail;
	}
}