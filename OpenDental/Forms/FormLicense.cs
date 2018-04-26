using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenDental{
	/// <summary></summary>
	public class FormLicense : ODForm {
		private OpenDental.UI.Button butClose;
		private ListBox listLicense;
		private RichTextBox textLicense;
		private Label labelLicense;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		///<summary></summary>
		public FormLicense()
		{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLicense));
			this.butClose = new OpenDental.UI.Button();
			this.listLicense = new System.Windows.Forms.ListBox();
			this.textLicense = new System.Windows.Forms.RichTextBox();
			this.labelLicense = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(764, 482);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// listLicense
			// 
			this.listLicense.Items.AddRange(new object[] {
            "AForge",
            "Bouncy Castle",
            "BSD",
            "CDT",
            "Dropbox",
            "GPL",
            "Drifty",
            "Mentalis",
            "Microsoft",
            "MigraDoc",
            "NDde",
            "Newton Soft",
            "Oracle",
            "PDFSharp",
            "SharpDX",
            "SSHNet",
            "Stdole",
            "Tamir",
            "Tao_Freeglut",
            "Tao_OpenGL",
            "Twain Group"});
			this.listLicense.Location = new System.Drawing.Point(12, 55);
			this.listLicense.Name = "listLicense";
			this.listLicense.Size = new System.Drawing.Size(144, 277);
			this.listLicense.TabIndex = 20;
			this.listLicense.SelectedIndexChanged += new System.EventHandler(this.listLicense_SelectedIndexChanged);
			// 
			// textLicense
			// 
			this.textLicense.Location = new System.Drawing.Point(162, 35);
			this.textLicense.Name = "textLicense";
			this.textLicense.Size = new System.Drawing.Size(662, 425);
			this.textLicense.TabIndex = 21;
			this.textLicense.Text = "";
			// 
			// labelLicense
			// 
			this.labelLicense.Location = new System.Drawing.Point(10, 35);
			this.labelLicense.Name = "labelLicense";
			this.labelLicense.Size = new System.Drawing.Size(147, 17);
			this.labelLicense.TabIndex = 22;
			this.labelLicense.Text = "Select License:";
			this.labelLicense.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// FormLicense
			// 
			this.ClientSize = new System.Drawing.Size(851, 520);
			this.Controls.Add(this.labelLicense);
			this.Controls.Add(this.textLicense);
			this.Controls.Add(this.listLicense);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormLicense";
			this.ShowInTaskbar = false;
			this.Text = "Licenses";
			this.Load += new System.EventHandler(this.FormLicense_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormLicense_Load(object sender,EventArgs e) {
			listLicense.SetSelected(2,true);
		}

		private void FillLicense() {
			switch(listLicense.SelectedIndex) {
				case 0:
					textLicense.Text=Properties.Resources.AForge;
					break;
				case 1:
					textLicense.Text=Properties.Resources.BouncyCastle;
					break;
				case 2:
					textLicense.Text=Properties.Resources.Bsd;
					break;
				case 3:
					textLicense.Text=Properties.Resources.CDT_Content_End_User_License1;
					break;
				case 4:
					textLicense.Text=Properties.Resources.Dropbox_Api;
					break;
				case 5:
					textLicense.Text=Properties.Resources.GPL;
					break;
				case 6:
					textLicense.Text=Properties.Resources.Ionic;
					break;
				case 7:
					textLicense.Text=Properties.Resources.Mentalis;
					break;
				case 8:
					textLicense.Text=Properties.Resources.Microsoft;
					break;
				case 9:
					textLicense.Text=Properties.Resources.MigraDoc;
					break;
				case 10:
					textLicense.Text=Properties.Resources.NDde;
					break;
				case 11:
					textLicense.Text=Properties.Resources.NewtonSoft_Json;
					break;
				case 12:
					textLicense.Text=Properties.Resources.Oracle;
					break;
				case 13:
					textLicense.Text=Properties.Resources.PdfSharp;
					break;
				case 14:
					textLicense.Text=Properties.Resources.SharpDX;
					break;
				case 15:
					textLicense.Text=Properties.Resources.SshNet;
					break;
				case 16:
					textLicense.Text=Properties.Resources.stdole;
					break;
				case 17:
					textLicense.Text=Properties.Resources.Tamir;
					break;
				case 18:
					textLicense.Text=Properties.Resources.Tao_Freeglut;
					break;
				case 19:
					textLicense.Text=Properties.Resources.Tao_OpenGL;
					break;
				case 20:
					textLicense.Text=Properties.Resources.Twain;
					break;
			}
		}
		private void listLicense_SelectedIndexChanged(object sender,EventArgs e) {
			FillLicense();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}
	}
}





















