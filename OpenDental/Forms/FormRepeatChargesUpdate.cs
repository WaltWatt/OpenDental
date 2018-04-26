using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormRepeatChargesUpdate :ODForm{
		// ReSharper disable once InconsistentNaming
		private UI.Button butCancel;
		// ReSharper disable once InconsistentNaming
		private UI.Button butOK;
		// ReSharper disable once InconsistentNaming
		private TextBox textBox1;		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		///<summary></summary>
		public FormRepeatChargesUpdate()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRepeatChargesUpdate));
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.SystemColors.Control;
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.Location = new System.Drawing.Point(43, 13);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(426, 175);
			this.textBox1.TabIndex = 3;
			this.textBox1.Text = resources.GetString("textBox1.Text");
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(393, 197);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 1;
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
			this.butCancel.Location = new System.Drawing.Point(393, 238);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormRepeatChargesUpdate
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(520, 289);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormRepeatChargesUpdate";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Update Repeating Charges";
			this.Load += new System.EventHandler(this.FormRepeatChargesUpdate_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormRepeatChargesUpdate_Load(object sender, EventArgs e) {
		
		}

		///<summary>Do not use this method in release code. This is only to be used for Unit Tests 53-56.</summary>
		public void RunRepeatingChargesForUnitTests(DateTime dateRun) {
			RepeatCharges.RunRepeatingCharges(dateRun);
		}

		private void butOK_Click(object sender,EventArgs e) {
			Prefs.RefreshCache();//Just to be sure we don't miss someone who has just started running repeating charges.
			if(PrefC.GetString(PrefName.RepeatingChargesBeginDateTime)!="") {
				MsgBox.Show(this,"Repeating charges already running on another workstation, you must wait for them to finish before continuing.");
				return;
			}
			RepeatChargeResult result=RepeatCharges.RunRepeatingCharges(MiscData.GetNowDateTime());
			SecurityLogs.MakeLogEntry(Permissions.RepeatChargeTool,0,"Repeat Charge Tool ran.");
			MessageBox.Show(result.ProceduresAddedCount+" "+Lan.g(this,"procedures added.")+"\r\n"+result.ClaimsAddedCount+" "
				+Lan.g(this,"claims added."));
			DialogResult=DialogResult.OK;
		}	

		private void butCancel_Click(object sender, EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		


	}
}





















