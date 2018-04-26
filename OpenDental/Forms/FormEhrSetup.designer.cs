namespace OpenDental{
	partial class FormEhrSetup {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEhrSetup));
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.menuItemSettings = new System.Windows.Forms.MenuItem();
			this.groupCodeSystems = new System.Windows.Forms.GroupBox();
			this.butCodeImport = new OpenDental.UI.Button();
			this.butRxNorm = new OpenDental.UI.Button();
			this.butICD9s = new OpenDental.UI.Button();
			this.butSnomeds = new OpenDental.UI.Button();
			this.butProviderKeys = new OpenDental.UI.Button();
			this.butPortalSetup = new OpenDental.UI.Button();
			this.butOIDs = new OpenDental.UI.Button();
			this.butEhrTriggers = new OpenDental.UI.Button();
			this.butTimeSynch = new OpenDental.UI.Button();
			this.butLoincs = new OpenDental.UI.Button();
			this.butEducationalResources = new OpenDental.UI.Button();
			this.butInboundEmail = new OpenDental.UI.Button();
			this.butReminderRules = new OpenDental.UI.Button();
			this.panelEmergencyNow = new OpenDental.UI.PanelOD();
			this.butEmergencyNow = new OpenDental.UI.Button();
			this.butDrugUnit = new OpenDental.UI.Button();
			this.butDrugManufacturer = new OpenDental.UI.Button();
			this.butVaccineDef = new OpenDental.UI.Button();
			this.butAllergies = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.groupCodeSystems.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSettings});
			// 
			// menuItemSettings
			// 
			this.menuItemSettings.Index = 0;
			this.menuItemSettings.Text = "Settings";
			this.menuItemSettings.Click += new System.EventHandler(this.menuItemSettings_Click);
			// 
			// groupCodeSystems
			// 
			this.groupCodeSystems.Controls.Add(this.butCodeImport);
			this.groupCodeSystems.Controls.Add(this.butRxNorm);
			this.groupCodeSystems.Controls.Add(this.butICD9s);
			this.groupCodeSystems.Controls.Add(this.butSnomeds);
			this.groupCodeSystems.Location = new System.Drawing.Point(202, 0);
			this.groupCodeSystems.Name = "groupCodeSystems";
			this.groupCodeSystems.Size = new System.Drawing.Size(152, 203);
			this.groupCodeSystems.TabIndex = 136;
			this.groupCodeSystems.TabStop = false;
			this.groupCodeSystems.Text = "Code Systems";
			// 
			// butCodeImport
			// 
			this.butCodeImport.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCodeImport.Autosize = true;
			this.butCodeImport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCodeImport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCodeImport.CornerRadius = 4F;
			this.butCodeImport.Location = new System.Drawing.Point(12, 19);
			this.butCodeImport.Name = "butCodeImport";
			this.butCodeImport.Size = new System.Drawing.Size(128, 24);
			this.butCodeImport.TabIndex = 14;
			this.butCodeImport.Text = "Code System Importer";
			this.butCodeImport.UseVisualStyleBackColor = false;
			this.butCodeImport.Click += new System.EventHandler(this.butCodeImport_Click);
			// 
			// butRxNorm
			// 
			this.butRxNorm.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRxNorm.Autosize = true;
			this.butRxNorm.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRxNorm.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRxNorm.CornerRadius = 4F;
			this.butRxNorm.Location = new System.Drawing.Point(12, 95);
			this.butRxNorm.Name = "butRxNorm";
			this.butRxNorm.Size = new System.Drawing.Size(128, 24);
			this.butRxNorm.TabIndex = 10;
			this.butRxNorm.Text = "RxNorms";
			this.butRxNorm.Click += new System.EventHandler(this.butRxNorm_Click);
			// 
			// butICD9s
			// 
			this.butICD9s.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butICD9s.Autosize = true;
			this.butICD9s.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butICD9s.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butICD9s.CornerRadius = 4F;
			this.butICD9s.Location = new System.Drawing.Point(12, 57);
			this.butICD9s.Name = "butICD9s";
			this.butICD9s.Size = new System.Drawing.Size(128, 24);
			this.butICD9s.TabIndex = 9;
			this.butICD9s.Text = "ICD9s";
			this.butICD9s.Click += new System.EventHandler(this.butICD9s_Click);
			// 
			// butSnomeds
			// 
			this.butSnomeds.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSnomeds.Autosize = true;
			this.butSnomeds.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSnomeds.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSnomeds.CornerRadius = 4F;
			this.butSnomeds.Location = new System.Drawing.Point(12, 133);
			this.butSnomeds.Name = "butSnomeds";
			this.butSnomeds.Size = new System.Drawing.Size(128, 24);
			this.butSnomeds.TabIndex = 12;
			this.butSnomeds.Text = "SNOMED CTs";
			this.butSnomeds.Click += new System.EventHandler(this.butSnomeds_Click);
			// 
			// butProviderKeys
			// 
			this.butProviderKeys.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butProviderKeys.Autosize = true;
			this.butProviderKeys.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butProviderKeys.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butProviderKeys.CornerRadius = 4F;
			this.butProviderKeys.Location = new System.Drawing.Point(27, 323);
			this.butProviderKeys.Name = "butProviderKeys";
			this.butProviderKeys.Size = new System.Drawing.Size(128, 24);
			this.butProviderKeys.TabIndex = 142;
			this.butProviderKeys.Text = "Provider Keys";
			this.butProviderKeys.Click += new System.EventHandler(this.butProviderKeys_Click);
			// 
			// butPortalSetup
			// 
			this.butPortalSetup.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPortalSetup.Autosize = true;
			this.butPortalSetup.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPortalSetup.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPortalSetup.CornerRadius = 4F;
			this.butPortalSetup.Location = new System.Drawing.Point(27, 285);
			this.butPortalSetup.Name = "butPortalSetup";
			this.butPortalSetup.Size = new System.Drawing.Size(128, 24);
			this.butPortalSetup.TabIndex = 141;
			this.butPortalSetup.Text = "Patient Portal";
			this.butPortalSetup.Click += new System.EventHandler(this.butPortalSetup_Click);
			// 
			// butOIDs
			// 
			this.butOIDs.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOIDs.Autosize = true;
			this.butOIDs.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOIDs.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOIDs.CornerRadius = 4F;
			this.butOIDs.Location = new System.Drawing.Point(214, 285);
			this.butOIDs.Name = "butOIDs";
			this.butOIDs.Size = new System.Drawing.Size(128, 24);
			this.butOIDs.TabIndex = 140;
			this.butOIDs.Text = "Internal OID Registry";
			this.butOIDs.Click += new System.EventHandler(this.butOIDs_Click);
			// 
			// butEhrTriggers
			// 
			this.butEhrTriggers.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEhrTriggers.Autosize = true;
			this.butEhrTriggers.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEhrTriggers.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEhrTriggers.CornerRadius = 4F;
			this.butEhrTriggers.Location = new System.Drawing.Point(214, 209);
			this.butEhrTriggers.Name = "butEhrTriggers";
			this.butEhrTriggers.Size = new System.Drawing.Size(128, 24);
			this.butEhrTriggers.TabIndex = 138;
			this.butEhrTriggers.Text = "CDS Triggers";
			this.butEhrTriggers.Click += new System.EventHandler(this.butCdsTriggers_Click);
			// 
			// butTimeSynch
			// 
			this.butTimeSynch.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butTimeSynch.Autosize = true;
			this.butTimeSynch.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTimeSynch.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTimeSynch.CornerRadius = 4F;
			this.butTimeSynch.Location = new System.Drawing.Point(214, 247);
			this.butTimeSynch.Name = "butTimeSynch";
			this.butTimeSynch.Size = new System.Drawing.Size(128, 24);
			this.butTimeSynch.TabIndex = 15;
			this.butTimeSynch.Text = "Time Synchronization";
			this.butTimeSynch.Click += new System.EventHandler(this.butTimeSynch_Click);
			// 
			// butLoincs
			// 
			this.butLoincs.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLoincs.Autosize = true;
			this.butLoincs.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLoincs.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLoincs.CornerRadius = 4F;
			this.butLoincs.Location = new System.Drawing.Point(214, 171);
			this.butLoincs.Name = "butLoincs";
			this.butLoincs.Size = new System.Drawing.Size(128, 24);
			this.butLoincs.TabIndex = 11;
			this.butLoincs.Text = "Loincs";
			this.butLoincs.Click += new System.EventHandler(this.butLoincs_Click);
			// 
			// butEducationalResources
			// 
			this.butEducationalResources.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEducationalResources.Autosize = true;
			this.butEducationalResources.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEducationalResources.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEducationalResources.CornerRadius = 4F;
			this.butEducationalResources.Location = new System.Drawing.Point(27, 247);
			this.butEducationalResources.Name = "butEducationalResources";
			this.butEducationalResources.Size = new System.Drawing.Size(128, 24);
			this.butEducationalResources.TabIndex = 7;
			this.butEducationalResources.Text = "Educational Resources";
			this.butEducationalResources.Click += new System.EventHandler(this.butEducationalResources_Click);
			// 
			// butInboundEmail
			// 
			this.butInboundEmail.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butInboundEmail.Autosize = true;
			this.butInboundEmail.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butInboundEmail.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butInboundEmail.CornerRadius = 4F;
			this.butInboundEmail.Location = new System.Drawing.Point(27, 209);
			this.butInboundEmail.Name = "butInboundEmail";
			this.butInboundEmail.Size = new System.Drawing.Size(128, 24);
			this.butInboundEmail.TabIndex = 6;
			this.butInboundEmail.Text = "Inbound Email";
			this.butInboundEmail.Click += new System.EventHandler(this.butInboundEmail_Click);
			// 
			// butReminderRules
			// 
			this.butReminderRules.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butReminderRules.Autosize = true;
			this.butReminderRules.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butReminderRules.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butReminderRules.CornerRadius = 4F;
			this.butReminderRules.Location = new System.Drawing.Point(27, 171);
			this.butReminderRules.Name = "butReminderRules";
			this.butReminderRules.Size = new System.Drawing.Size(128, 24);
			this.butReminderRules.TabIndex = 5;
			this.butReminderRules.Text = "Reminder Rules";
			this.butReminderRules.Click += new System.EventHandler(this.butReminderRules_Click);
			// 
			// panelEmergencyNow
			// 
			this.panelEmergencyNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.panelEmergencyNow.Location = new System.Drawing.Point(504, 57);
			this.panelEmergencyNow.Name = "panelEmergencyNow";
			this.panelEmergencyNow.Size = new System.Drawing.Size(24, 24);
			this.panelEmergencyNow.TabIndex = 125;
			// 
			// butEmergencyNow
			// 
			this.butEmergencyNow.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEmergencyNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butEmergencyNow.Autosize = true;
			this.butEmergencyNow.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEmergencyNow.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEmergencyNow.CornerRadius = 4F;
			this.butEmergencyNow.Location = new System.Drawing.Point(400, 57);
			this.butEmergencyNow.Name = "butEmergencyNow";
			this.butEmergencyNow.Size = new System.Drawing.Size(98, 24);
			this.butEmergencyNow.TabIndex = 15;
			this.butEmergencyNow.Text = "Emergency Now";
			this.butEmergencyNow.Click += new System.EventHandler(this.butEmergencyNow_Click);
			// 
			// butDrugUnit
			// 
			this.butDrugUnit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDrugUnit.Autosize = true;
			this.butDrugUnit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDrugUnit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDrugUnit.CornerRadius = 4F;
			this.butDrugUnit.Location = new System.Drawing.Point(27, 133);
			this.butDrugUnit.Name = "butDrugUnit";
			this.butDrugUnit.Size = new System.Drawing.Size(128, 24);
			this.butDrugUnit.TabIndex = 4;
			this.butDrugUnit.Text = "Drug Unit";
			this.butDrugUnit.Click += new System.EventHandler(this.butDrugUnit_Click);
			// 
			// butDrugManufacturer
			// 
			this.butDrugManufacturer.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDrugManufacturer.Autosize = true;
			this.butDrugManufacturer.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDrugManufacturer.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDrugManufacturer.CornerRadius = 4F;
			this.butDrugManufacturer.Location = new System.Drawing.Point(27, 95);
			this.butDrugManufacturer.Name = "butDrugManufacturer";
			this.butDrugManufacturer.Size = new System.Drawing.Size(128, 24);
			this.butDrugManufacturer.TabIndex = 3;
			this.butDrugManufacturer.Text = "Drug Manufacturer";
			this.butDrugManufacturer.Click += new System.EventHandler(this.butDrugManufacturer_Click);
			// 
			// butVaccineDef
			// 
			this.butVaccineDef.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butVaccineDef.Autosize = true;
			this.butVaccineDef.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butVaccineDef.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butVaccineDef.CornerRadius = 4F;
			this.butVaccineDef.Location = new System.Drawing.Point(27, 57);
			this.butVaccineDef.Name = "butVaccineDef";
			this.butVaccineDef.Size = new System.Drawing.Size(128, 24);
			this.butVaccineDef.TabIndex = 2;
			this.butVaccineDef.Text = "Vaccine Def";
			this.butVaccineDef.Click += new System.EventHandler(this.butVaccineDef_Click);
			// 
			// butAllergies
			// 
			this.butAllergies.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAllergies.Autosize = true;
			this.butAllergies.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAllergies.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAllergies.CornerRadius = 4F;
			this.butAllergies.Location = new System.Drawing.Point(27, 19);
			this.butAllergies.Name = "butAllergies";
			this.butAllergies.Size = new System.Drawing.Size(128, 24);
			this.butAllergies.TabIndex = 1;
			this.butAllergies.Text = "Allergies";
			this.butAllergies.Click += new System.EventHandler(this.butAllergies_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(453, 332);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 16;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormEhrSetup
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(561, 374);
			this.Controls.Add(this.butProviderKeys);
			this.Controls.Add(this.butPortalSetup);
			this.Controls.Add(this.butOIDs);
			this.Controls.Add(this.butEhrTriggers);
			this.Controls.Add(this.butTimeSynch);
			this.Controls.Add(this.butLoincs);
			this.Controls.Add(this.groupCodeSystems);
			this.Controls.Add(this.butEducationalResources);
			this.Controls.Add(this.butInboundEmail);
			this.Controls.Add(this.butReminderRules);
			this.Controls.Add(this.panelEmergencyNow);
			this.Controls.Add(this.butEmergencyNow);
			this.Controls.Add(this.butDrugUnit);
			this.Controls.Add(this.butDrugManufacturer);
			this.Controls.Add(this.butVaccineDef);
			this.Controls.Add(this.butAllergies);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.Name = "FormEhrSetup";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Electronic Health Record (EHR) Setup";
			this.Load += new System.EventHandler(this.FormEhrSetup_Load);
			this.groupCodeSystems.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butClose;
		private UI.Button butICD9s;
		private UI.Button butAllergies;
		private UI.Button butVaccineDef;
		private UI.Button butDrugManufacturer;
		private UI.Button butDrugUnit;
		private UI.Button butEmergencyNow;
		private UI.PanelOD panelEmergencyNow;
		private UI.Button butReminderRules;
		private UI.Button butInboundEmail;
		private UI.Button butEducationalResources;
		private UI.Button butRxNorm;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItemSettings;
		private UI.Button butLoincs;
		private UI.Button butSnomeds;
		private UI.Button butCodeImport;
		private System.Windows.Forms.GroupBox groupCodeSystems;
		private UI.Button butTimeSynch;
		private UI.Button butEhrTriggers;
		private UI.Button butOIDs;
		private UI.Button butPortalSetup;
		private UI.Button butProviderKeys;
	}
}