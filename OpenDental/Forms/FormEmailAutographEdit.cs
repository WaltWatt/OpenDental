using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	///<summary>Email autograph edit.</summary>
	public class FormEmailAutographEdit:ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textEmailAddress;
		private OpenDental.ODtextBox textAutographText;
		///<summary></summary>
		public bool IsNew;
		private Label label1;
		private TextBox textDescription;
		private Label label3;
		///<summary></summary>
		public EmailAutograph _emailAutograph;

		///<summary></summary>
		public FormEmailAutographEdit(EmailAutograph emailAutograph)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			_emailAutograph=emailAutograph;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailAutographEdit));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.textEmailAddress = new System.Windows.Forms.TextBox();
			this.textAutographText = new OpenDental.ODtextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
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
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(883, 656);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
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
			this.butOK.Location = new System.Drawing.Point(802, 656);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
			this.butOK.TabIndex = 4;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 33);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(102, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "Email Address";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textEmailAddress
			// 
			this.textEmailAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textEmailAddress.Location = new System.Drawing.Point(109, 33);
			this.textEmailAddress.MaxLength = 200;
			this.textEmailAddress.Name = "textEmailAddress";
			this.textEmailAddress.Size = new System.Drawing.Size(849, 20);
			this.textEmailAddress.TabIndex = 2;
			// 
			// textAutographText
			// 
			this.textAutographText.AcceptsTab = true;
			this.textAutographText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textAutographText.DetectUrls = false;
			this.textAutographText.Location = new System.Drawing.Point(109, 54);
			this.textAutographText.Name = "textAutographText";
			this.textAutographText.QuickPasteType = OpenDentBusiness.QuickPasteType.Email;
			this.textAutographText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textAutographText.Size = new System.Drawing.Size(849, 596);
			this.textAutographText.TabIndex = 3;
			this.textAutographText.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(3, 54);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(105, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Autograph Text";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDescription
			// 
			this.textDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDescription.Location = new System.Drawing.Point(109, 12);
			this.textDescription.MaxLength = 200;
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(849, 20);
			this.textDescription.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 12);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(102, 20);
			this.label3.TabIndex = 0;
			this.label3.Text = "Description";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormEmailAutographEdit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(974, 695);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textAutographText);
			this.Controls.Add(this.textEmailAddress);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(200, 200);
			this.Name = "FormEmailAutographEdit";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Email Autograph";
			this.Load += new System.EventHandler(this.FormEmailTemplateEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormEmailTemplateEdit_Load(object sender, System.EventArgs e) {
			textDescription.Text=_emailAutograph.Description;
			textEmailAddress.Text=_emailAutograph.EmailAddress;
			textAutographText.Text=_emailAutograph.AutographText;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textAutographText.Text=="") {
				MsgBox.Show(this,"Autograph Text cannot be blank.");
				return;
			}
			if(textDescription.Text==""){
				MsgBox.Show(this,"Description cannot be blank.");
				return;
			}
			_emailAutograph.Description=textDescription.Text;
			_emailAutograph.EmailAddress=textEmailAddress.Text;
			_emailAutograph.AutographText=textAutographText.Text;
			if(IsNew){
				EmailAutographs.Insert(_emailAutograph);
			}
			else{
				EmailAutographs.Update(_emailAutograph);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}





















