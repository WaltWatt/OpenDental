namespace OpenDental{
	partial class FormResellerEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormResellerEdit));
			this.label6 = new System.Windows.Forms.Label();
			this.textUserName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textPassword = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.labelCredentials = new System.Windows.Forms.Label();
			this.menuRightClick = new System.Windows.Forms.ContextMenu();
			this.menuItemAccount = new System.Windows.Forms.MenuItem();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.gridServices = new OpenDental.UI.ODGrid();
			this.label1 = new System.Windows.Forms.Label();
			this.labelTotal = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.butAdd = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label6.Location = new System.Drawing.Point(12, 393);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(371, 24);
			this.label6.TabIndex = 40;
			this.label6.Text = "The customers list is managed by the reseller using the Reseller Portal.";
			// 
			// textUserName
			// 
			this.textUserName.Location = new System.Drawing.Point(104, 26);
			this.textUserName.Name = "textUserName";
			this.textUserName.Size = new System.Drawing.Size(247, 20);
			this.textUserName.TabIndex = 245;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(6, 27);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(95, 19);
			this.label4.TabIndex = 247;
			this.label4.Text = "User Name";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPassword
			// 
			this.textPassword.Location = new System.Drawing.Point(104, 54);
			this.textPassword.Name = "textPassword";
			this.textPassword.Size = new System.Drawing.Size(247, 20);
			this.textPassword.TabIndex = 246;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(6, 55);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(95, 19);
			this.label5.TabIndex = 248;
			this.label5.Text = "Password";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.labelCredentials);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.textUserName);
			this.groupBox1.Controls.Add(this.textPassword);
			this.groupBox1.Location = new System.Drawing.Point(14, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(400, 99);
			this.groupBox1.TabIndex = 250;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Reseller Portal Credentials";
			// 
			// labelCredentials
			// 
			this.labelCredentials.Location = new System.Drawing.Point(104, 77);
			this.labelCredentials.Name = "labelCredentials";
			this.labelCredentials.Size = new System.Drawing.Size(290, 18);
			this.labelCredentials.TabIndex = 249;
			this.labelCredentials.Text = "Any user name and password will work.";
			// 
			// menuRightClick
			// 
			this.menuRightClick.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAccount});
			// 
			// menuItemAccount
			// 
			this.menuItemAccount.Index = 0;
			this.menuItemAccount.Text = "Go to Account";
			this.menuItemAccount.Click += new System.EventHandler(this.menuItemAccount_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridMain.HScrollVisible = true;
			this.gridMain.Location = new System.Drawing.Point(12, 113);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(516, 274);
			this.gridMain.TabIndex = 39;
			this.gridMain.Title = "Customers";
			this.gridMain.TranslationName = "TableCustomers";
			// 
			// gridServices
			// 
			this.gridServices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridServices.HScrollVisible = false;
			this.gridServices.Location = new System.Drawing.Point(534, 113);
			this.gridServices.Name = "gridServices";
			this.gridServices.ScrollValue = 0;
			this.gridServices.Size = new System.Drawing.Size(248, 274);
			this.gridServices.TabIndex = 252;
			this.gridServices.Title = "Available Services";
			this.gridServices.TranslationName = "TableAvailableServices";
			this.gridServices.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridServices_CellDoubleClick);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Location = new System.Drawing.Point(102, 456);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(426, 27);
			this.label1.TabIndex = 254;
			this.label1.Text = "Delete the reseller.\r\nSets reseller PatStatus to inactive and disables all reg ke" +
    "ys.";
			// 
			// labelTotal
			// 
			this.labelTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelTotal.Location = new System.Drawing.Point(389, 393);
			this.labelTotal.Name = "labelTotal";
			this.labelTotal.Size = new System.Drawing.Size(139, 24);
			this.labelTotal.TabIndex = 255;
			this.labelTotal.Text = "Total: $0.00";
			this.labelTotal.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.Location = new System.Drawing.Point(533, 73);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(251, 31);
			this.label3.TabIndex = 256;
			this.label3.Text = "Each reseller pays different prices and has different services available for thei" +
    "r customers.";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(682, 393);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(100, 26);
			this.butAdd.TabIndex = 253;
			this.butAdd.Text = "&Add Service";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(612, 458);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 251;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
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
			this.butDelete.Location = new System.Drawing.Point(12, 458);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(84, 24);
			this.butDelete.TabIndex = 41;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(707, 458);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 3;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormResellerEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(804, 494);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.labelTotal);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.gridServices);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormResellerEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Reseller Edit";
			this.Load += new System.EventHandler(this.FormResellerEdit_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butCancel;
		private UI.ODGrid gridMain;
		private System.Windows.Forms.Label label6;
		private UI.Button butDelete;
		private System.Windows.Forms.TextBox textUserName;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textPassword;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label labelCredentials;
		private UI.Button butOK;
		private System.Windows.Forms.ContextMenu menuRightClick;
		private System.Windows.Forms.MenuItem menuItemAccount;
		private UI.ODGrid gridServices;
		private UI.Button butAdd;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelTotal;
		private System.Windows.Forms.Label label3;
	}
}