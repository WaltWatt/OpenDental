namespace OpenDental{
	partial class FormPatientRaceEthnicity {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPatientRaceEthnicity));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butRight = new OpenDental.UI.Button();
			this.butLeft = new OpenDental.UI.Button();
			this.gridRace = new OpenDental.UI.ODGrid();
			this.gridEthnicity = new OpenDental.UI.ODGrid();
			this.treeRaces = new System.Windows.Forms.TreeView();
			this.treeEthnicities = new System.Windows.Forms.TreeView();
			this.labelPosition = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.labelNeedCodes = new System.Windows.Forms.Label();
			this.butImport = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(498, 543);
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
			this.butCancel.Location = new System.Drawing.Point(588, 543);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butRight
			// 
			this.butRight.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRight.Autosize = true;
			this.butRight.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRight.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRight.CornerRadius = 4F;
			this.butRight.Image = global::OpenDental.Properties.Resources.Right;
			this.butRight.Location = new System.Drawing.Point(356, 284);
			this.butRight.Name = "butRight";
			this.butRight.Size = new System.Drawing.Size(35, 24);
			this.butRight.TabIndex = 59;
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
			this.butLeft.Location = new System.Drawing.Point(356, 246);
			this.butLeft.Name = "butLeft";
			this.butLeft.Size = new System.Drawing.Size(35, 24);
			this.butLeft.TabIndex = 58;
			this.butLeft.Click += new System.EventHandler(this.butLeft_Click);
			// 
			// gridRace
			// 
			this.gridRace.HasAddButton = false;
			this.gridRace.HasMultilineHeaders = false;
			this.gridRace.HeaderHeight = 15;
			this.gridRace.HScrollVisible = false;
			this.gridRace.Location = new System.Drawing.Point(12, 45);
			this.gridRace.Name = "gridRace";
			this.gridRace.ScrollValue = 0;
			this.gridRace.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridRace.Size = new System.Drawing.Size(321, 222);
			this.gridRace.TabIndex = 56;
			this.gridRace.Title = "Race";
			this.gridRace.TitleHeight = 18;
			this.gridRace.TranslationName = "FormPatientRaceEthnicity";
			// 
			// gridEthnicity
			// 
			this.gridEthnicity.HasAddButton = false;
			this.gridEthnicity.HasMultilineHeaders = false;
			this.gridEthnicity.HeaderHeight = 15;
			this.gridEthnicity.HScrollVisible = false;
			this.gridEthnicity.Location = new System.Drawing.Point(12, 295);
			this.gridEthnicity.Name = "gridEthnicity";
			this.gridEthnicity.ScrollValue = 0;
			this.gridEthnicity.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridEthnicity.Size = new System.Drawing.Size(321, 219);
			this.gridEthnicity.TabIndex = 60;
			this.gridEthnicity.Title = "Ethnicity";
			this.gridEthnicity.TitleHeight = 18;
			this.gridEthnicity.TranslationName = "FormPatientRaceEthnicity";
			// 
			// treeRaces
			// 
			this.treeRaces.Location = new System.Drawing.Point(412, 45);
			this.treeRaces.Name = "treeRaces";
			this.treeRaces.Size = new System.Drawing.Size(237, 222);
			this.treeRaces.TabIndex = 61;
			this.treeRaces.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeRaces_NodeMouseDoubleClick);
			// 
			// treeEthnicities
			// 
			this.treeEthnicities.Location = new System.Drawing.Point(412, 295);
			this.treeEthnicities.Name = "treeEthnicities";
			this.treeEthnicities.Size = new System.Drawing.Size(237, 222);
			this.treeEthnicities.TabIndex = 62;
			this.treeEthnicities.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeEthnicities_NodeMouseDoubleClick);
			// 
			// labelPosition
			// 
			this.labelPosition.Location = new System.Drawing.Point(409, 278);
			this.labelPosition.Name = "labelPosition";
			this.labelPosition.Size = new System.Drawing.Size(105, 14);
			this.labelPosition.TabIndex = 63;
			this.labelPosition.Text = "Ethnicities";
			this.labelPosition.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(409, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(105, 14);
			this.label1.TabIndex = 64;
			this.label1.Text = "Races";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelNeedCodes
			// 
			this.labelNeedCodes.Location = new System.Drawing.Point(12, 9);
			this.labelNeedCodes.Name = "labelNeedCodes";
			this.labelNeedCodes.Size = new System.Drawing.Size(240, 33);
			this.labelNeedCodes.TabIndex = 65;
			this.labelNeedCodes.Text = "CDCREC codes must be downloaded through the Code System Importer";
			this.labelNeedCodes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butImport
			// 
			this.butImport.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butImport.Autosize = true;
			this.butImport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butImport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butImport.CornerRadius = 4F;
			this.butImport.Location = new System.Drawing.Point(258, 12);
			this.butImport.Name = "butImport";
			this.butImport.Size = new System.Drawing.Size(75, 24);
			this.butImport.TabIndex = 66;
			this.butImport.Text = "&Import";
			this.butImport.Click += new System.EventHandler(this.butImport_Click);
			// 
			// FormPatientRaceEthnicity
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(675, 579);
			this.Controls.Add(this.butImport);
			this.Controls.Add(this.labelNeedCodes);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.labelPosition);
			this.Controls.Add(this.treeEthnicities);
			this.Controls.Add(this.treeRaces);
			this.Controls.Add(this.gridEthnicity);
			this.Controls.Add(this.butRight);
			this.Controls.Add(this.butLeft);
			this.Controls.Add(this.gridRace);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormPatientRaceEthnicity";
			this.Text = "Patient Race and Ethnicity";
			this.Load += new System.EventHandler(this.FormPatientRaceEthnicity_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private UI.Button butRight;
		private UI.Button butLeft;
		private UI.ODGrid gridRace;
		private UI.ODGrid gridEthnicity;
		private System.Windows.Forms.TreeView treeRaces;
		private System.Windows.Forms.TreeView treeEthnicities;
		private System.Windows.Forms.Label labelPosition;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelNeedCodes;
		private UI.Button butImport;
	}
}