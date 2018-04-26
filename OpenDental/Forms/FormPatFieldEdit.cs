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
	public class FormPatFieldEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public bool IsNew;
		public bool IsLaunchedFromOrtho;
		private PatField _fieldCur;
		private PatField _fieldOld;
		private Label labelName;
		private UI.Button butUseAutoNote;
		private TextBox textValue;

		///<summary></summary>
		public FormPatFieldEdit(PatField field)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			_fieldCur=field;
			_fieldOld=_fieldCur.Copy();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPatFieldEdit));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.labelName = new System.Windows.Forms.Label();
			this.textValue = new System.Windows.Forms.TextBox();
			this.butUseAutoNote = new OpenDental.UI.Button();
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
			this.butCancel.Location = new System.Drawing.Point(280, 147);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
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
			this.butOK.Location = new System.Drawing.Point(186, 147);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// labelName
			// 
			this.labelName.Location = new System.Drawing.Point(19, 17);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(335, 20);
			this.labelName.TabIndex = 3;
			this.labelName.Text = "Field Name";
			this.labelName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textValue
			// 
			this.textValue.Location = new System.Drawing.Point(21, 40);
			this.textValue.Multiline = true;
			this.textValue.Name = "textValue";
			this.textValue.Size = new System.Drawing.Size(333, 74);
			this.textValue.TabIndex = 0;
			// 
			// butUseAutoNote
			// 
			this.butUseAutoNote.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUseAutoNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butUseAutoNote.Autosize = true;
			this.butUseAutoNote.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUseAutoNote.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUseAutoNote.CornerRadius = 4F;
			this.butUseAutoNote.Location = new System.Drawing.Point(21, 147);
			this.butUseAutoNote.Name = "butUseAutoNote";
			this.butUseAutoNote.Size = new System.Drawing.Size(75, 24);
			this.butUseAutoNote.TabIndex = 4;
			this.butUseAutoNote.Text = "Auto Note";
			this.butUseAutoNote.Visible = false;
			this.butUseAutoNote.Click += new System.EventHandler(this.butUseAutoNote_Click);
			// 
			// FormPatFieldEdit
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(375, 193);
			this.Controls.Add(this.butUseAutoNote);
			this.Controls.Add(this.textValue);
			this.Controls.Add(this.labelName);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormPatFieldEdit";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Patient Field";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPatFieldDefEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormPatFieldEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormPatFieldEdit_Load(object sender, System.EventArgs e) {
			labelName.Text=_fieldCur.FieldName;
			textValue.Text=_fieldCur.FieldValue;
			if(IsLaunchedFromOrtho) {
				butUseAutoNote.Visible=true;
			}
		}

		private void butUseAutoNote_Click(object sender,EventArgs e) {
			FormAutoNoteCompose FormA=new FormAutoNoteCompose();
			FormA.ShowDialog();
			if(FormA.DialogResult==DialogResult.OK) {
				textValue.AppendText(FormA.CompletedNote);
			}
		}

		/*private void buttonDelete_Click(object sender,EventArgs e) {
			
		}*/

		private void butOK_Click(object sender, System.EventArgs e) {
			_fieldCur.FieldValue=textValue.Text;
			if(_fieldCur.FieldValue==""){//if blank, then delete
				if(IsNew) {
					DialogResult=DialogResult.Cancel;
					return;
				}
				PatFields.Delete(_fieldCur);
				if(_fieldOld.FieldValue!="") {//We don't need to make a log for field values that were blank because the user simply clicked cancel.
					PatFields.MakeDeleteLogEntry(_fieldOld);
				}
				DialogResult=DialogResult.OK;
				return;
			}
			if(IsNew){
				PatFields.Insert(_fieldCur);
			}
			else{
				PatFields.Update(_fieldCur);
				PatFields.MakeEditLogEntry(_fieldOld,_fieldCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormPatFieldDefEdit_FormClosing(object sender,FormClosingEventArgs e) {
			/*if(DialogResult==DialogResult.OK){
				return;
			}
			if(IsNew) {
				PatFields.Delete(Field);
			}*/
		}
	

		


	}
}





















