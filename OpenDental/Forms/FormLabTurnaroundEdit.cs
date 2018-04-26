using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormLabTurnaroundEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private TextBox textDescription;
		private Label label1;
		private Label label2;
		private Label label3;
		private ValidNum textDaysActual;
		private Label label4;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private ValidNumber textDaysPublished;
		public LabTurnaround LabTurnaroundCur;

		///<summary></summary>
		public FormLabTurnaroundEdit()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLabTurnaroundEdit));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textDaysActual = new OpenDental.ValidNum();
			this.label4 = new System.Windows.Forms.Label();
			this.textDaysPublished = new OpenDental.ValidNumber();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(326, 143);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 5;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(235, 143);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 4;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(164, 17);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(241, 20);
			this.textDescription.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(3, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(160, 17);
			this.label1.TabIndex = 4;
			this.label1.Text = "Service Description";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(33, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(130, 17);
			this.label2.TabIndex = 6;
			this.label2.Text = "Days Published";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(33, 70);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(130, 17);
			this.label3.TabIndex = 8;
			this.label3.Text = "Actual Days";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDaysActual
			// 
			this.textDaysActual.Location = new System.Drawing.Point(164, 69);
			this.textDaysActual.MaxVal = 255;
			this.textDaysActual.MinVal = 0;
			this.textDaysActual.Name = "textDaysActual";
			this.textDaysActual.Size = new System.Drawing.Size(43, 20);
			this.textDaysActual.TabIndex = 3;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(213, 68);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(174, 44);
			this.label4.TabIndex = 9;
			this.label4.Text = "Might be same as days published, or might include travel time.";
			// 
			// textDaysPublished
			// 
			this.textDaysPublished.Location = new System.Drawing.Point(164, 43);
			this.textDaysPublished.MaxVal = 255;
			this.textDaysPublished.MinVal = 0;
			this.textDaysPublished.Name = "textDaysPublished";
			this.textDaysPublished.Size = new System.Drawing.Size(43, 20);
			this.textDaysPublished.TabIndex = 2;
			// 
			// FormLabTurnaroundEdit
			// 
			this.ClientSize = new System.Drawing.Size(453, 194);
			this.Controls.Add(this.textDaysPublished);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textDaysActual);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormLabTurnaroundEdit";
			this.ShowInTaskbar = false;
			this.Text = "Lab Turnaround";
			this.Load += new System.EventHandler(this.FormLabTurnaroundEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormLabTurnaroundEdit_Load(object sender,EventArgs e) {
			textDescription.Text=LabTurnaroundCur.Description;
			if(LabTurnaroundCur.DaysPublished>0){
				textDaysPublished.Text=LabTurnaroundCur.DaysPublished.ToString();//otherwise, blank
			}
			textDaysActual.Text=LabTurnaroundCur.DaysActual.ToString();
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textDaysActual.errorProvider1.GetError(textDaysActual)!=""
				|| textDaysPublished.errorProvider1.GetError(textDaysPublished)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(PIn.Long(textDaysActual.Text)==0){
				MsgBox.Show(this,"Actual Days cannot be zero.");
				return;
			}
			if(textDescription.Text==""){
				MsgBox.Show(this,"Please enter a description.");
				return;
			}
			LabTurnaroundCur.Description=textDescription.Text;
			LabTurnaroundCur.DaysPublished=PIn.Int(textDaysPublished.Text);
			LabTurnaroundCur.DaysActual=PIn.Int(textDaysActual.Text);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		


	}
}





















