namespace OpenDental{
	partial class FormJobNew {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJobNew));
			this.controlJobEdit = new OpenDental.InternalTools.Job_Manager.UserControlJobEdit();
			this.SuspendLayout();
			// 
			// controlJobEdit
			// 
			this.controlJobEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.controlJobEdit.IsOverride = false;
			this.controlJobEdit.Location = new System.Drawing.Point(0, 0);
			this.controlJobEdit.Name = "controlJobEdit";
			this.controlJobEdit.Size = new System.Drawing.Size(1164, 713);
			this.controlJobEdit.TabIndex = 0;
			this.controlJobEdit.SaveClick += new System.EventHandler(this.userControlJobEdit1_SaveClick);
			this.controlJobEdit.TitleChanged += new OpenDental.InternalTools.Job_Manager.UserControlJobEdit.UpdateTitleEvent(this.controlJobEdit_TitleChanged);
			// 
			// FormJobNew
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1164, 713);
			this.Controls.Add(this.controlJobEdit);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormJobNew";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "New Job";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormJobNew_FormClosing);
			this.Load += new System.EventHandler(this.FormJobNew_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private InternalTools.Job_Manager.UserControlJobEdit controlJobEdit;

	}
}