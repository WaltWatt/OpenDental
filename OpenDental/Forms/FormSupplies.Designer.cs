namespace OpenDental{
	partial class FormSupplies {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSupplies));
			this.label3 = new System.Windows.Forms.Label();
			this.comboSupplier = new System.Windows.Forms.ComboBox();
			this.checkShowHidden = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textFind = new System.Windows.Forms.TextBox();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butUp = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butPrint = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.checkShowShoppingList = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(569, 11);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(67, 18);
			this.label3.TabIndex = 14;
			this.label3.Text = "Supplier";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboSupplier
			// 
			this.comboSupplier.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSupplier.FormattingEnabled = true;
			this.comboSupplier.Location = new System.Drawing.Point(638, 10);
			this.comboSupplier.Name = "comboSupplier";
			this.comboSupplier.Size = new System.Drawing.Size(170, 21);
			this.comboSupplier.TabIndex = 13;
			this.comboSupplier.SelectionChangeCommitted += new System.EventHandler(this.comboSupplier_SelectionChangeCommitted);
			// 
			// checkShowHidden
			// 
			this.checkShowHidden.Location = new System.Drawing.Point(246, 13);
			this.checkShowHidden.Name = "checkShowHidden";
			this.checkShowHidden.Size = new System.Drawing.Size(99, 18);
			this.checkShowHidden.TabIndex = 12;
			this.checkShowHidden.Text = "Show Hidden";
			this.checkShowHidden.UseVisualStyleBackColor = true;
			this.checkShowHidden.Click += new System.EventHandler(this.checkShowHidden_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(343, 11);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(49, 18);
			this.label1.TabIndex = 18;
			this.label1.Text = "Search";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textFind
			// 
			this.textFind.Location = new System.Drawing.Point(393, 11);
			this.textFind.Name = "textFind";
			this.textFind.Size = new System.Drawing.Size(168, 20);
			this.textFind.TabIndex = 1;
			this.textFind.TextChanged += new System.EventHandler(this.textFind_TextChanged);
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
			this.gridMain.Location = new System.Drawing.Point(12, 38);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(796, 575);
			this.gridMain.TabIndex = 5;
			this.gridMain.Title = "Supplies";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableSupplies";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.Location = new System.Drawing.Point(12, 622);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(24, 24);
			this.butUp.TabIndex = 28;
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.Location = new System.Drawing.Point(42, 622);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(24, 24);
			this.butDown.TabIndex = 29;
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(652, 622);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 27;
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
			this.butCancel.Location = new System.Drawing.Point(733, 622);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 26;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrint;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(110, 622);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(80, 24);
			this.butPrint.TabIndex = 25;
			this.butPrint.Text = "Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
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
			this.butAdd.Location = new System.Drawing.Point(12, 7);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(69, 24);
			this.butAdd.TabIndex = 11;
			this.butAdd.Text = "Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// checkShowShoppingList
			// 
			this.checkShowShoppingList.Location = new System.Drawing.Point(114, 13);
			this.checkShowShoppingList.Name = "checkShowShoppingList";
			this.checkShowShoppingList.Size = new System.Drawing.Size(122, 18);
			this.checkShowShoppingList.TabIndex = 30;
			this.checkShowShoppingList.Text = "Show Shopping List";
			this.checkShowShoppingList.UseVisualStyleBackColor = true;
			this.checkShowShoppingList.Click += new System.EventHandler(this.checkShowShoppingList_Click);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(198, 623);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(447, 23);
			this.label2.TabIndex = 31;
			this.label2.Text = "Make sure to press OK to save all desired changes to the supply inventory.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FormSupplies
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(820, 657);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.checkShowShoppingList);
			this.Controls.Add(this.textFind);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.comboSupplier);
			this.Controls.Add(this.checkShowHidden);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.gridMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormSupplies";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Supplies";
			this.Load += new System.EventHandler(this.FormSupplies_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.ODGrid gridMain;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox comboSupplier;
		private System.Windows.Forms.CheckBox checkShowHidden;
		private OpenDental.UI.Button butAdd;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textFind;
		private UI.Button butPrint;
		private UI.Button butOK;
		private UI.Button butCancel;
		private UI.Button butUp;
		private UI.Button butDown;
		private System.Windows.Forms.CheckBox checkShowShoppingList;
		private System.Windows.Forms.Label label2;
	}
}