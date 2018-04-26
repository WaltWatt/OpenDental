/* ====================================================================
    Copyright (C) 2004-2006  fyiReporting Software, LLC

    This file is part of the fyiReporting RDL project.
	
    The RDL project is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

    For additional information, email info@fyireporting.com or visit
    the website www.fyiReporting.com.
*/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;

namespace fyiReporting.RdlDesign
{
	/// <summary>
	/// Summary description for DialogAbout.
	/// </summary>
	public class DialogAbout : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button bOK;
		private System.Windows.Forms.TextBox tbLicense;
		private System.Windows.Forms.LinkLabel linkLabel3;
		private System.Windows.Forms.LinkLabel linkLabel4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Label lVersion;
		private System.Windows.Forms.Label lVMVersion;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DialogAbout()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

#if DEBUG
			tbLicense.Text = @"RDL Designer creates reports defined using the Report Definition Language Specification.
Copyright (C) 2004-2006  fyiReporting Software, LLC

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

Commercial licenses are available.  Contact fyiReporting Software for additional information.";
#else
			tbLicense.Text = @"RDL Designer creates reports defined using the Report Definition Language Specification.
Copyright (C) 2004-2006  fyiReporting Software, LLC

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.";
#endif
			lVersion.Text = "Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
			this.lVMVersion.Text = ".NET " + Environment.Version.ToString();
			return;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DialogAbout));
			this.bOK = new System.Windows.Forms.Button();
			this.tbLicense = new System.Windows.Forms.TextBox();
			this.linkLabel3 = new System.Windows.Forms.LinkLabel();
			this.linkLabel4 = new System.Windows.Forms.LinkLabel();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.lVersion = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.lVMVersion = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// bOK
			// 
			this.bOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bOK.Location = new System.Drawing.Point(200, 272);
			this.bOK.Name = "bOK";
			this.bOK.TabIndex = 0;
			this.bOK.Text = "OK";
			// 
			// tbLicense
			// 
			this.tbLicense.Location = new System.Drawing.Point(8, 120);
			this.tbLicense.Multiline = true;
			this.tbLicense.Name = "tbLicense";
			this.tbLicense.ReadOnly = true;
			this.tbLicense.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbLicense.Size = new System.Drawing.Size(448, 136);
			this.tbLicense.TabIndex = 9;
			this.tbLicense.Text = "";
			// 
			// linkLabel3
			// 
			this.linkLabel3.Location = new System.Drawing.Point(280, 88);
			this.linkLabel3.Name = "linkLabel3";
			this.linkLabel3.Size = new System.Drawing.Size(152, 16);
			this.linkLabel3.TabIndex = 15;
			this.linkLabel3.TabStop = true;
			this.linkLabel3.Tag = "mailto:comments@fyireporting.com";
			this.linkLabel3.Text = "comments@fyireporting.com";
			this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_LinkClicked);
			// 
			// linkLabel4
			// 
			this.linkLabel4.Location = new System.Drawing.Point(72, 88);
			this.linkLabel4.Name = "linkLabel4";
			this.linkLabel4.Size = new System.Drawing.Size(144, 16);
			this.linkLabel4.TabIndex = 14;
			this.linkLabel4.TabStop = true;
			this.linkLabel4.Tag = "http://www.fyireporting.com";
			this.linkLabel4.Text = "http://www.fyireporting.com";
			this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_LinkClicked);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(240, 88);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(40, 23);
			this.label5.TabIndex = 13;
			this.label5.Text = "E-mail:";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 88);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(56, 23);
			this.label6.TabIndex = 12;
			this.label6.Text = "Website:";
			// 
			// lVersion
			// 
			this.lVersion.Location = new System.Drawing.Point(264, 40);
			this.lVersion.Name = "lVersion";
			this.lVersion.Size = new System.Drawing.Size(136, 16);
			this.lVersion.TabIndex = 11;
			this.lVersion.Text = "Version x.x.x";
			this.lVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label8.Location = new System.Drawing.Point(240, 16);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(184, 24);
			this.label8.TabIndex = 10;
			this.label8.Text = "fyiReporting Designer";
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
			this.pictureBox2.Location = new System.Drawing.Point(8, 8);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(212, 72);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox2.TabIndex = 16;
			this.pictureBox2.TabStop = false;
			// 
			// lVMVersion
			// 
			this.lVMVersion.Location = new System.Drawing.Point(256, 64);
			this.lVMVersion.Name = "lVMVersion";
			this.lVMVersion.Size = new System.Drawing.Size(144, 16);
			this.lVMVersion.TabIndex = 17;
			this.lVMVersion.Text = ".NET x.x.xxxx.xxxx";
			this.lVMVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// DialogAbout
			// 
			this.AcceptButton = this.bOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bOK;
			this.ClientSize = new System.Drawing.Size(466, 304);
			this.Controls.Add(this.lVMVersion);
			this.Controls.Add(this.linkLabel3);
			this.Controls.Add(this.linkLabel4);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.lVersion);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.tbLicense);
			this.Controls.Add(this.bOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogAbout";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About";
			this.ResumeLayout(false);

		}
		#endregion

		private void lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs ea)
		{
			LinkLabel lnk = (LinkLabel) sender;
			lnk.Links[lnk.Links.IndexOf(ea.Link)].Visited = true;
			System.Diagnostics.Process.Start(lnk.Tag.ToString());
		}
	}

}
