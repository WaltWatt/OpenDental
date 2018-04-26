namespace OpenDental{
	partial class FormSheetDefEdit {
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
			if(GraphicsBackground!=null) {
				GraphicsBackground.Dispose();
				GraphicsBackground=null;
			}
			if(BmBackground!=null) {
				BmBackground.Dispose();
				BmBackground=null;
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSheetDefEdit));
			this.textDescription = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panelMain = new OpenDental.PanelGraphics();
			this.labelInternal = new System.Windows.Forms.Label();
			this.listFields = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupAddNew = new System.Windows.Forms.GroupBox();
			this.butScreenChart = new OpenDental.UI.Button();
			this.butAddCombo = new OpenDental.UI.Button();
			this.butAddSpecial = new OpenDental.UI.Button();
			this.butAddGrid = new OpenDental.UI.Button();
			this.butAddPatImage = new OpenDental.UI.Button();
			this.butAddSigBox = new OpenDental.UI.Button();
			this.butAddCheckBox = new OpenDental.UI.Button();
			this.butAddRect = new OpenDental.UI.Button();
			this.butAddLine = new OpenDental.UI.Button();
			this.butAddImage = new OpenDental.UI.Button();
			this.butAddStaticText = new OpenDental.UI.Button();
			this.butAddInputField = new OpenDental.UI.Button();
			this.butAddOutputText = new OpenDental.UI.Button();
			this.linkLabelTips = new System.Windows.Forms.LinkLabel();
			this.groupPage = new System.Windows.Forms.GroupBox();
			this.butPageAdd = new OpenDental.UI.Button();
			this.butPageRemove = new OpenDental.UI.Button();
			this.groupAlignH = new System.Windows.Forms.GroupBox();
			this.butAlignRight = new OpenDental.UI.Button();
			this.butAlignCenterH = new OpenDental.UI.Button();
			this.butAlignLeft = new OpenDental.UI.Button();
			this.groupAlignV = new System.Windows.Forms.GroupBox();
			this.butAlignTop = new OpenDental.UI.Button();
			this.butTabOrder = new OpenDental.UI.Button();
			this.butPaste = new OpenDental.UI.Button();
			this.butCopy = new OpenDental.UI.Button();
			this.butEdit = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.panel1.SuspendLayout();
			this.groupAddNew.SuspendLayout();
			this.groupPage.SuspendLayout();
			this.groupAlignH.SuspendLayout();
			this.groupAlignV.SuspendLayout();
			this.SuspendLayout();
			// 
			// textDescription
			// 
			this.textDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textDescription.Location = new System.Drawing.Point(677, 3);
			this.textDescription.Name = "textDescription";
			this.textDescription.ReadOnly = true;
			this.textDescription.Size = new System.Drawing.Size(144, 20);
			this.textDescription.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.panelMain);
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(656, 783);
			this.panel1.TabIndex = 81;
			// 
			// panelMain
			// 
			this.panelMain.BackColor = System.Drawing.Color.Transparent;
			this.panelMain.Location = new System.Drawing.Point(0, 0);
			this.panelMain.Name = "panelMain";
			this.panelMain.Size = new System.Drawing.Size(549, 513);
			this.panelMain.TabIndex = 0;
			this.panelMain.TabStop = true;
			this.panelMain.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMain_Paint);
			this.panelMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.panelMain_MouseDoubleClick);
			this.panelMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelMain_MouseDown);
			this.panelMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelMain_MouseMove);
			this.panelMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelMain_MouseUp);
			this.panelMain.Resize += new System.EventHandler(this.panelMain_Resize);
			// 
			// labelInternal
			// 
			this.labelInternal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelInternal.Location = new System.Drawing.Point(679, 687);
			this.labelInternal.Name = "labelInternal";
			this.labelInternal.Size = new System.Drawing.Size(144, 46);
			this.labelInternal.TabIndex = 82;
			this.labelInternal.Text = "This is an internal sheet, so it may not be edited.  Make a copy instead.";
			this.labelInternal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// listFields
			// 
			this.listFields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listFields.FormattingEnabled = true;
			this.listFields.HorizontalScrollbar = true;
			this.listFields.Location = new System.Drawing.Point(679, 270);
			this.listFields.Name = "listFields";
			this.listFields.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listFields.Size = new System.Drawing.Size(142, 303);
			this.listFields.TabIndex = 83;
			this.listFields.Click += new System.EventHandler(this.listFields_Click);
			this.listFields.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listFields_MouseDoubleClick);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(677, 253);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(108, 16);
			this.label2.TabIndex = 84;
			this.label2.Text = "Fields";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupAddNew
			// 
			this.groupAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupAddNew.Controls.Add(this.butScreenChart);
			this.groupAddNew.Controls.Add(this.butAddCombo);
			this.groupAddNew.Controls.Add(this.butAddSpecial);
			this.groupAddNew.Controls.Add(this.butAddGrid);
			this.groupAddNew.Controls.Add(this.butAddPatImage);
			this.groupAddNew.Controls.Add(this.butAddSigBox);
			this.groupAddNew.Controls.Add(this.butAddCheckBox);
			this.groupAddNew.Controls.Add(this.butAddRect);
			this.groupAddNew.Controls.Add(this.butAddLine);
			this.groupAddNew.Controls.Add(this.butAddImage);
			this.groupAddNew.Controls.Add(this.butAddStaticText);
			this.groupAddNew.Controls.Add(this.butAddInputField);
			this.groupAddNew.Controls.Add(this.butAddOutputText);
			this.groupAddNew.Location = new System.Drawing.Point(677, 48);
			this.groupAddNew.Name = "groupAddNew";
			this.groupAddNew.Size = new System.Drawing.Size(144, 161);
			this.groupAddNew.TabIndex = 86;
			this.groupAddNew.TabStop = false;
			this.groupAddNew.Text = "Add new";
			// 
			// butScreenChart
			// 
			this.butScreenChart.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butScreenChart.Autosize = false;
			this.butScreenChart.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butScreenChart.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butScreenChart.CornerRadius = 4F;
			this.butScreenChart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butScreenChart.Location = new System.Drawing.Point(3, 115);
			this.butScreenChart.Name = "butScreenChart";
			this.butScreenChart.Size = new System.Drawing.Size(69, 20);
			this.butScreenChart.TabIndex = 97;
			this.butScreenChart.TabStop = false;
			this.butScreenChart.Text = "ScreenChart";
			this.butScreenChart.Visible = false;
			this.butScreenChart.Click += new System.EventHandler(this.butScreenChart_Click);
			// 
			// butAddCombo
			// 
			this.butAddCombo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddCombo.Autosize = true;
			this.butAddCombo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddCombo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddCombo.CornerRadius = 4F;
			this.butAddCombo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddCombo.Location = new System.Drawing.Point(3, 95);
			this.butAddCombo.Name = "butAddCombo";
			this.butAddCombo.Size = new System.Drawing.Size(69, 20);
			this.butAddCombo.TabIndex = 96;
			this.butAddCombo.TabStop = false;
			this.butAddCombo.Text = "ComboBox";
			this.butAddCombo.Click += new System.EventHandler(this.butAddCombo_Click);
			// 
			// butAddSpecial
			// 
			this.butAddSpecial.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddSpecial.Autosize = true;
			this.butAddSpecial.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddSpecial.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddSpecial.CornerRadius = 4F;
			this.butAddSpecial.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddSpecial.Location = new System.Drawing.Point(3, 135);
			this.butAddSpecial.Name = "butAddSpecial";
			this.butAddSpecial.Size = new System.Drawing.Size(69, 20);
			this.butAddSpecial.TabIndex = 94;
			this.butAddSpecial.TabStop = false;
			this.butAddSpecial.Text = "Special";
			this.butAddSpecial.Visible = false;
			this.butAddSpecial.Click += new System.EventHandler(this.butAddSpecial_Click);
			// 
			// butAddGrid
			// 
			this.butAddGrid.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddGrid.Autosize = true;
			this.butAddGrid.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddGrid.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddGrid.CornerRadius = 4F;
			this.butAddGrid.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddGrid.Location = new System.Drawing.Point(72, 115);
			this.butAddGrid.Name = "butAddGrid";
			this.butAddGrid.Size = new System.Drawing.Size(69, 20);
			this.butAddGrid.TabIndex = 95;
			this.butAddGrid.TabStop = false;
			this.butAddGrid.Text = "Grid";
			this.butAddGrid.Click += new System.EventHandler(this.butAddGrid_Click);
			// 
			// butAddPatImage
			// 
			this.butAddPatImage.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddPatImage.Autosize = true;
			this.butAddPatImage.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddPatImage.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddPatImage.CornerRadius = 4F;
			this.butAddPatImage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddPatImage.Location = new System.Drawing.Point(72, 95);
			this.butAddPatImage.Name = "butAddPatImage";
			this.butAddPatImage.Size = new System.Drawing.Size(69, 20);
			this.butAddPatImage.TabIndex = 93;
			this.butAddPatImage.TabStop = false;
			this.butAddPatImage.Text = "Pat Image";
			this.butAddPatImage.Click += new System.EventHandler(this.butAddPatImage_Click);
			// 
			// butAddSigBox
			// 
			this.butAddSigBox.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddSigBox.Autosize = true;
			this.butAddSigBox.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddSigBox.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddSigBox.CornerRadius = 4F;
			this.butAddSigBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddSigBox.Location = new System.Drawing.Point(72, 75);
			this.butAddSigBox.Name = "butAddSigBox";
			this.butAddSigBox.Size = new System.Drawing.Size(69, 20);
			this.butAddSigBox.TabIndex = 92;
			this.butAddSigBox.TabStop = false;
			this.butAddSigBox.Text = "Signature";
			this.butAddSigBox.Click += new System.EventHandler(this.butAddSigBox_Click);
			// 
			// butAddCheckBox
			// 
			this.butAddCheckBox.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddCheckBox.Autosize = true;
			this.butAddCheckBox.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddCheckBox.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddCheckBox.CornerRadius = 4F;
			this.butAddCheckBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddCheckBox.Location = new System.Drawing.Point(3, 55);
			this.butAddCheckBox.Name = "butAddCheckBox";
			this.butAddCheckBox.Size = new System.Drawing.Size(69, 20);
			this.butAddCheckBox.TabIndex = 91;
			this.butAddCheckBox.TabStop = false;
			this.butAddCheckBox.Text = "CheckBox";
			this.butAddCheckBox.Click += new System.EventHandler(this.butAddCheckBox_Click);
			// 
			// butAddRect
			// 
			this.butAddRect.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddRect.Autosize = true;
			this.butAddRect.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddRect.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddRect.CornerRadius = 4F;
			this.butAddRect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddRect.Location = new System.Drawing.Point(72, 55);
			this.butAddRect.Name = "butAddRect";
			this.butAddRect.Size = new System.Drawing.Size(69, 20);
			this.butAddRect.TabIndex = 90;
			this.butAddRect.TabStop = false;
			this.butAddRect.Text = "Rectangle";
			this.butAddRect.Click += new System.EventHandler(this.butAddRect_Click);
			// 
			// butAddLine
			// 
			this.butAddLine.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddLine.Autosize = true;
			this.butAddLine.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddLine.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddLine.CornerRadius = 4F;
			this.butAddLine.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddLine.Location = new System.Drawing.Point(72, 35);
			this.butAddLine.Name = "butAddLine";
			this.butAddLine.Size = new System.Drawing.Size(69, 20);
			this.butAddLine.TabIndex = 89;
			this.butAddLine.TabStop = false;
			this.butAddLine.Text = "Line";
			this.butAddLine.Click += new System.EventHandler(this.butAddLine_Click);
			// 
			// butAddImage
			// 
			this.butAddImage.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddImage.Autosize = true;
			this.butAddImage.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddImage.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddImage.CornerRadius = 4F;
			this.butAddImage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddImage.Location = new System.Drawing.Point(3, 75);
			this.butAddImage.Name = "butAddImage";
			this.butAddImage.Size = new System.Drawing.Size(69, 20);
			this.butAddImage.TabIndex = 88;
			this.butAddImage.TabStop = false;
			this.butAddImage.Text = "StaticImage";
			this.butAddImage.Click += new System.EventHandler(this.butAddImage_Click);
			// 
			// butAddStaticText
			// 
			this.butAddStaticText.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddStaticText.Autosize = true;
			this.butAddStaticText.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddStaticText.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddStaticText.CornerRadius = 4F;
			this.butAddStaticText.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddStaticText.Location = new System.Drawing.Point(72, 15);
			this.butAddStaticText.Name = "butAddStaticText";
			this.butAddStaticText.Size = new System.Drawing.Size(69, 20);
			this.butAddStaticText.TabIndex = 87;
			this.butAddStaticText.TabStop = false;
			this.butAddStaticText.Text = "StaticText";
			this.butAddStaticText.Click += new System.EventHandler(this.butAddStaticText_Click);
			// 
			// butAddInputField
			// 
			this.butAddInputField.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddInputField.Autosize = true;
			this.butAddInputField.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddInputField.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddInputField.CornerRadius = 4F;
			this.butAddInputField.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddInputField.Location = new System.Drawing.Point(3, 35);
			this.butAddInputField.Name = "butAddInputField";
			this.butAddInputField.Size = new System.Drawing.Size(69, 20);
			this.butAddInputField.TabIndex = 86;
			this.butAddInputField.TabStop = false;
			this.butAddInputField.Text = "InputField";
			this.butAddInputField.Click += new System.EventHandler(this.butAddInputField_Click);
			// 
			// butAddOutputText
			// 
			this.butAddOutputText.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddOutputText.Autosize = true;
			this.butAddOutputText.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddOutputText.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddOutputText.CornerRadius = 4F;
			this.butAddOutputText.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddOutputText.Location = new System.Drawing.Point(3, 15);
			this.butAddOutputText.Name = "butAddOutputText";
			this.butAddOutputText.Size = new System.Drawing.Size(69, 20);
			this.butAddOutputText.TabIndex = 85;
			this.butAddOutputText.TabStop = false;
			this.butAddOutputText.Text = "OutputText";
			this.butAddOutputText.Click += new System.EventHandler(this.butAddOutputText_Click);
			// 
			// linkLabelTips
			// 
			this.linkLabelTips.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.linkLabelTips.Location = new System.Drawing.Point(790, 691);
			this.linkLabelTips.Name = "linkLabelTips";
			this.linkLabelTips.Size = new System.Drawing.Size(31, 17);
			this.linkLabelTips.TabIndex = 93;
			this.linkLabelTips.TabStop = true;
			this.linkLabelTips.Text = "tips";
			this.linkLabelTips.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.linkLabelTips.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelTips_LinkClicked);
			// 
			// groupPage
			// 
			this.groupPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupPage.Controls.Add(this.butPageAdd);
			this.groupPage.Controls.Add(this.butPageRemove);
			this.groupPage.Location = new System.Drawing.Point(677, 215);
			this.groupPage.Name = "groupPage";
			this.groupPage.Size = new System.Drawing.Size(144, 39);
			this.groupPage.TabIndex = 95;
			this.groupPage.TabStop = false;
			this.groupPage.Text = "Pages";
			// 
			// butPageAdd
			// 
			this.butPageAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPageAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butPageAdd.Autosize = true;
			this.butPageAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPageAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPageAdd.CornerRadius = 4F;
			this.butPageAdd.Location = new System.Drawing.Point(3, 15);
			this.butPageAdd.Name = "butPageAdd";
			this.butPageAdd.Size = new System.Drawing.Size(69, 20);
			this.butPageAdd.TabIndex = 95;
			this.butPageAdd.TabStop = false;
			this.butPageAdd.Text = "Add";
			this.butPageAdd.Click += new System.EventHandler(this.butPageAdd_Click);
			// 
			// butPageRemove
			// 
			this.butPageRemove.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPageRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butPageRemove.Autosize = true;
			this.butPageRemove.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPageRemove.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPageRemove.CornerRadius = 4F;
			this.butPageRemove.Location = new System.Drawing.Point(72, 15);
			this.butPageRemove.Name = "butPageRemove";
			this.butPageRemove.Size = new System.Drawing.Size(69, 20);
			this.butPageRemove.TabIndex = 96;
			this.butPageRemove.TabStop = false;
			this.butPageRemove.Text = "Remove";
			this.butPageRemove.Click += new System.EventHandler(this.butPageRemove_Click);
			// 
			// groupAlignH
			// 
			this.groupAlignH.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupAlignH.Controls.Add(this.butAlignRight);
			this.groupAlignH.Controls.Add(this.butAlignCenterH);
			this.groupAlignH.Controls.Add(this.butAlignLeft);
			this.groupAlignH.Location = new System.Drawing.Point(679, 620);
			this.groupAlignH.Name = "groupAlignH";
			this.groupAlignH.Size = new System.Drawing.Size(144, 46);
			this.groupAlignH.TabIndex = 96;
			this.groupAlignH.TabStop = false;
			this.groupAlignH.Text = "Horizontal Align";
			// 
			// butAlignRight
			// 
			this.butAlignRight.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAlignRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAlignRight.Autosize = true;
			this.butAlignRight.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAlignRight.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAlignRight.CornerRadius = 4F;
			this.butAlignRight.Location = new System.Drawing.Point(94, 15);
			this.butAlignRight.Name = "butAlignRight";
			this.butAlignRight.Size = new System.Drawing.Size(43, 20);
			this.butAlignRight.TabIndex = 91;
			this.butAlignRight.TabStop = false;
			this.butAlignRight.Text = "Right";
			this.butAlignRight.Click += new System.EventHandler(this.butAlignRight_Click);
			// 
			// butAlignCenterH
			// 
			this.butAlignCenterH.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAlignCenterH.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAlignCenterH.Autosize = true;
			this.butAlignCenterH.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAlignCenterH.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAlignCenterH.CornerRadius = 4F;
			this.butAlignCenterH.Location = new System.Drawing.Point(50, 15);
			this.butAlignCenterH.Name = "butAlignCenterH";
			this.butAlignCenterH.Size = new System.Drawing.Size(43, 20);
			this.butAlignCenterH.TabIndex = 90;
			this.butAlignCenterH.TabStop = false;
			this.butAlignCenterH.Text = "Center";
			this.butAlignCenterH.Click += new System.EventHandler(this.butAlignCenterH_Click);
			// 
			// butAlignLeft
			// 
			this.butAlignLeft.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAlignLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAlignLeft.Autosize = true;
			this.butAlignLeft.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAlignLeft.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAlignLeft.CornerRadius = 4F;
			this.butAlignLeft.Location = new System.Drawing.Point(6, 15);
			this.butAlignLeft.Name = "butAlignLeft";
			this.butAlignLeft.Size = new System.Drawing.Size(43, 20);
			this.butAlignLeft.TabIndex = 89;
			this.butAlignLeft.TabStop = false;
			this.butAlignLeft.Text = "Left";
			this.butAlignLeft.Click += new System.EventHandler(this.butAlignLeft_Click);
			// 
			// groupAlignV
			// 
			this.groupAlignV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupAlignV.Controls.Add(this.butAlignTop);
			this.groupAlignV.Location = new System.Drawing.Point(679, 574);
			this.groupAlignV.Name = "groupAlignV";
			this.groupAlignV.Size = new System.Drawing.Size(142, 46);
			this.groupAlignV.TabIndex = 97;
			this.groupAlignV.TabStop = false;
			this.groupAlignV.Text = "Vertical Align";
			// 
			// butAlignTop
			// 
			this.butAlignTop.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAlignTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAlignTop.Autosize = true;
			this.butAlignTop.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAlignTop.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAlignTop.CornerRadius = 4F;
			this.butAlignTop.Location = new System.Drawing.Point(4, 19);
			this.butAlignTop.Name = "butAlignTop";
			this.butAlignTop.Size = new System.Drawing.Size(66, 20);
			this.butAlignTop.TabIndex = 88;
			this.butAlignTop.TabStop = false;
			this.butAlignTop.Text = "Top";
			this.butAlignTop.Click += new System.EventHandler(this.butAlignTop_Click);
			// 
			// butTabOrder
			// 
			this.butTabOrder.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butTabOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butTabOrder.Autosize = true;
			this.butTabOrder.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTabOrder.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTabOrder.CornerRadius = 4F;
			this.butTabOrder.Location = new System.Drawing.Point(679, 691);
			this.butTabOrder.Name = "butTabOrder";
			this.butTabOrder.Size = new System.Drawing.Size(70, 20);
			this.butTabOrder.TabIndex = 94;
			this.butTabOrder.TabStop = false;
			this.butTabOrder.Text = "Tab Order";
			this.butTabOrder.Click += new System.EventHandler(this.butTabOrder_Click);
			// 
			// butPaste
			// 
			this.butPaste.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPaste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPaste.Autosize = true;
			this.butPaste.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPaste.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPaste.CornerRadius = 4F;
			this.butPaste.Location = new System.Drawing.Point(749, 668);
			this.butPaste.Name = "butPaste";
			this.butPaste.Size = new System.Drawing.Size(72, 20);
			this.butPaste.TabIndex = 91;
			this.butPaste.TabStop = false;
			this.butPaste.Text = "Paste";
			this.butPaste.Click += new System.EventHandler(this.butPaste_Click);
			// 
			// butCopy
			// 
			this.butCopy.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCopy.Autosize = true;
			this.butCopy.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCopy.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCopy.CornerRadius = 4F;
			this.butCopy.Location = new System.Drawing.Point(679, 668);
			this.butCopy.Name = "butCopy";
			this.butCopy.Size = new System.Drawing.Size(70, 20);
			this.butCopy.TabIndex = 90;
			this.butCopy.TabStop = false;
			this.butCopy.Text = "Copy";
			this.butCopy.Click += new System.EventHandler(this.butCopy_Click);
			// 
			// butEdit
			// 
			this.butEdit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butEdit.Autosize = true;
			this.butEdit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEdit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEdit.CornerRadius = 4F;
			this.butEdit.Location = new System.Drawing.Point(731, 24);
			this.butEdit.Name = "butEdit";
			this.butEdit.Size = new System.Drawing.Size(90, 24);
			this.butEdit.TabIndex = 87;
			this.butEdit.TabStop = false;
			this.butEdit.Text = "Edit Properties";
			this.butEdit.Click += new System.EventHandler(this.butEdit_Click);
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(679, 762);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(70, 24);
			this.butDelete.TabIndex = 80;
			this.butDelete.TabStop = false;
			this.butDelete.Text = "Delete";
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
			this.butOK.Location = new System.Drawing.Point(749, 733);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(72, 24);
			this.butOK.TabIndex = 3;
			this.butOK.TabStop = false;
			this.butOK.Text = "OK";
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
			this.butCancel.Location = new System.Drawing.Point(749, 762);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(72, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.TabStop = false;
			this.butCancel.Text = "Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormSheetDefEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(828, 794);
			this.Controls.Add(this.groupAlignV);
			this.Controls.Add(this.groupAlignH);
			this.Controls.Add(this.groupPage);
			this.Controls.Add(this.butTabOrder);
			this.Controls.Add(this.linkLabelTips);
			this.Controls.Add(this.butPaste);
			this.Controls.Add(this.butCopy);
			this.Controls.Add(this.butEdit);
			this.Controls.Add(this.groupAddNew);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.listFields);
			this.Controls.Add(this.labelInternal);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormSheetDefEdit";
			this.Text = "Edit Sheet Def";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSheetDefEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormSheetDefEdit_Load);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormSheetDefEdit_KeyUp);
			this.panel1.ResumeLayout(false);
			this.groupAddNew.ResumeLayout(false);
			this.groupPage.ResumeLayout(false);
			this.groupAlignH.ResumeLayout(false);
			this.groupAlignV.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.TextBox textDescription;
		private OpenDental.UI.Button butDelete;
		private System.Windows.Forms.Panel panel1;
		private OpenDental.PanelGraphics panelMain;
		private System.Windows.Forms.Label labelInternal;
		private System.Windows.Forms.ListBox listFields;
		private System.Windows.Forms.Label label2;
		private OpenDental.UI.Button butAddOutputText;
		private System.Windows.Forms.GroupBox groupAddNew;
		private OpenDental.UI.Button butAddStaticText;
		private OpenDental.UI.Button butAddInputField;
		private OpenDental.UI.Button butEdit;
		private OpenDental.UI.Button butAddImage;
		private OpenDental.UI.Button butAddRect;
		private OpenDental.UI.Button butAddLine;
		private OpenDental.UI.Button butAddCheckBox;
		private OpenDental.UI.Button butAddSigBox;
		private OpenDental.UI.Button butAddPatImage;
		private UI.Button butAlignTop;
		private UI.Button butAlignLeft;
		private UI.Button butCopy;
		private UI.Button butPaste;
		private System.Windows.Forms.LinkLabel linkLabelTips;
		private UI.Button butTabOrder;
		private UI.Button butAddSpecial;
		private UI.Button butPageAdd;
		private UI.Button butPageRemove;
		private System.Windows.Forms.GroupBox groupPage;
		private UI.Button butAddGrid;
		private System.Windows.Forms.GroupBox groupAlignH;
		private UI.Button butAlignRight;
		private UI.Button butAlignCenterH;
		private System.Windows.Forms.GroupBox groupAlignV;
		private UI.Button butAddCombo;
		private UI.Button butScreenChart;
	}
}