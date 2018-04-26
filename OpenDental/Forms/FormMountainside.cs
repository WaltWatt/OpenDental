using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary> </summary>
	public class FormMountainside:ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.CheckBox checkEnabled;
		private IContainer components;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textProgName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textProgDesc;// Required designer variable.
		/// <summary>This Program link is new.</summary>
		public bool IsNew;
		public Program ProgramCur;
		private List<ProgramProperty> PropertyList;
		//private static Thread thread;
		private TextBox textHL7FolderOut;
		private Label labelHL7FolderOut;
		private GroupBox groupBox1;
		private Label label5;

		///<summary></summary>
		public FormMountainside() {
			components=new System.ComponentModel.Container();
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMountainside));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.checkEnabled = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textProgName = new System.Windows.Forms.TextBox();
			this.textProgDesc = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textHL7FolderOut = new System.Windows.Forms.TextBox();
			this.labelHL7FolderOut = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(524,224);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,24);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(443,224);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,24);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// checkEnabled
			// 
			this.checkEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEnabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkEnabled.Location = new System.Drawing.Point(161,60);
			this.checkEnabled.Name = "checkEnabled";
			this.checkEnabled.Size = new System.Drawing.Size(98,18);
			this.checkEnabled.TabIndex = 41;
			this.checkEnabled.Text = "Enabled";
			this.checkEnabled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEnabled.Click += new System.EventHandler(this.checkEnabled_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(58,10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(187,18);
			this.label1.TabIndex = 44;
			this.label1.Text = "Internal Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProgName
			// 
			this.textProgName.Location = new System.Drawing.Point(246,9);
			this.textProgName.Name = "textProgName";
			this.textProgName.ReadOnly = true;
			this.textProgName.Size = new System.Drawing.Size(275,20);
			this.textProgName.TabIndex = 45;
			// 
			// textProgDesc
			// 
			this.textProgDesc.Location = new System.Drawing.Point(246,34);
			this.textProgDesc.Name = "textProgDesc";
			this.textProgDesc.Size = new System.Drawing.Size(275,20);
			this.textProgDesc.TabIndex = 47;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(57,35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(187,18);
			this.label2.TabIndex = 46;
			this.label2.Text = "Description";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textHL7FolderOut
			// 
			this.textHL7FolderOut.Location = new System.Drawing.Point(234,48);
			this.textHL7FolderOut.Name = "textHL7FolderOut";
			this.textHL7FolderOut.Size = new System.Drawing.Size(275,20);
			this.textHL7FolderOut.TabIndex = 51;
			// 
			// labelHL7FolderOut
			// 
			this.labelHL7FolderOut.Location = new System.Drawing.Point(6,49);
			this.labelHL7FolderOut.Name = "labelHL7FolderOut";
			this.labelHL7FolderOut.Size = new System.Drawing.Size(226,18);
			this.labelHL7FolderOut.TabIndex = 50;
			this.labelHL7FolderOut.Text = "Out from Mountainside";
			this.labelHL7FolderOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.textHL7FolderOut);
			this.groupBox1.Controls.Add(this.labelHL7FolderOut);
			this.groupBox1.Location = new System.Drawing.Point(12,93);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(538,86);
			this.groupBox1.TabIndex = 52;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "HL7 Synch Folder";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(6,19);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(478,18);
			this.label5.TabIndex = 45;
			this.label5.Text = "Folder locations must be valid on the computer where the HL7 process is running";
			// 
			// FormMountainside
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(611,260);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.textProgDesc);
			this.Controls.Add(this.textProgName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkEnabled);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormMountainside";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Mountainside Setup";
			this.Load += new System.EventHandler(this.FormEClinicalWorks_Load);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormProgramLinkEdit_Closing);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormEClinicalWorks_Load(object sender, System.EventArgs e) {
			FillForm();
		}

		private void FillForm(){
			ProgramProperties.RefreshCache();
			PropertyList=ProgramProperties.GetForProgram(ProgramCur.ProgramNum);
			textProgName.Text=ProgramCur.ProgName;
			textProgDesc.Text=ProgramCur.ProgDesc;
			checkEnabled.Checked=ProgramCur.Enabled;
			textHL7FolderOut.Text=PrefC.GetString(PrefName.HL7FolderOut);
		}

		private void checkEnabled_Click(object sender,EventArgs e) {
			MsgBox.Show(this,"You will need to restart Open Dental to see the effects.");
		}

		private bool SaveToDb(){
			if(textProgDesc.Text==""){
				MsgBox.Show(this,"Description may not be blank.");
				return false;
			}
			if(textHL7FolderOut.Text=="") {
				MsgBox.Show(this,"HL7 out folder may not be blank.");
				return false;
			}
			ProgramCur.ProgDesc=textProgDesc.Text;
			ProgramCur.Enabled=checkEnabled.Checked;
			Programs.Update(ProgramCur);
			Prefs.UpdateString(PrefName.HL7FolderOut,textHL7FolderOut.Text);
			DataValid.SetInvalid(InvalidType.Programs,InvalidType.Prefs);
			return true;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(!SaveToDb()){
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormProgramLinkEdit_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			
		}

		

	

	

		

		

	

		

		

		
		


	}
}





















