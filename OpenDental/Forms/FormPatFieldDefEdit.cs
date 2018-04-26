using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormPatFieldDefEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.TextBox textName;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public bool IsNew;
		public PatFieldDef FieldDef;
		private OpenDental.UI.Button buttonDelete;
		private Label labelStatus;
		private ComboBox comboFieldType;
		private Label labelFieldType;
		private TextBox textPickList;
		private Label labelWarning;
		private CheckBox checkHidden;
		private string OldFieldName;

		///<summary></summary>
		public FormPatFieldDefEdit()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPatFieldDefEdit));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.textName = new System.Windows.Forms.TextBox();
			this.buttonDelete = new OpenDental.UI.Button();
			this.labelStatus = new System.Windows.Forms.Label();
			this.comboFieldType = new System.Windows.Forms.ComboBox();
			this.labelFieldType = new System.Windows.Forms.Label();
			this.textPickList = new System.Windows.Forms.TextBox();
			this.labelWarning = new System.Windows.Forms.Label();
			this.checkHidden = new System.Windows.Forms.CheckBox();
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
			this.butCancel.Location = new System.Drawing.Point(254, 451);
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
			this.butOK.Location = new System.Drawing.Point(254, 416);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// textName
			// 
			this.textName.Location = new System.Drawing.Point(20, 27);
			this.textName.Name = "textName";
			this.textName.Size = new System.Drawing.Size(309, 20);
			this.textName.TabIndex = 0;
			// 
			// buttonDelete
			// 
			this.buttonDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonDelete.Autosize = true;
			this.buttonDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.buttonDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.buttonDelete.CornerRadius = 4F;
			this.buttonDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.buttonDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonDelete.Location = new System.Drawing.Point(20, 451);
			this.buttonDelete.Name = "buttonDelete";
			this.buttonDelete.Size = new System.Drawing.Size(82, 24);
			this.buttonDelete.TabIndex = 3;
			this.buttonDelete.Text = "&Delete";
			this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
			// 
			// labelStatus
			// 
			this.labelStatus.Location = new System.Drawing.Point(17, 9);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(143, 15);
			this.labelStatus.TabIndex = 81;
			this.labelStatus.Text = "Field Name";
			this.labelStatus.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboFieldType
			// 
			this.comboFieldType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFieldType.DropDownWidth = 177;
			this.comboFieldType.FormattingEnabled = true;
			this.comboFieldType.Location = new System.Drawing.Point(20, 68);
			this.comboFieldType.MaxDropDownItems = 30;
			this.comboFieldType.Name = "comboFieldType";
			this.comboFieldType.Size = new System.Drawing.Size(177, 21);
			this.comboFieldType.TabIndex = 82;
			this.comboFieldType.SelectedIndexChanged += new System.EventHandler(this.comboFieldType_SelectedIndexChanged);
			// 
			// labelFieldType
			// 
			this.labelFieldType.Location = new System.Drawing.Point(17, 50);
			this.labelFieldType.Name = "labelFieldType";
			this.labelFieldType.Size = new System.Drawing.Size(143, 15);
			this.labelFieldType.TabIndex = 83;
			this.labelFieldType.Text = "Field Type";
			this.labelFieldType.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textPickList
			// 
			this.textPickList.AcceptsReturn = true;
			this.textPickList.Location = new System.Drawing.Point(20, 109);
			this.textPickList.Multiline = true;
			this.textPickList.Name = "textPickList";
			this.textPickList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textPickList.Size = new System.Drawing.Size(309, 288);
			this.textPickList.TabIndex = 84;
			// 
			// labelWarning
			// 
			this.labelWarning.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelWarning.Location = new System.Drawing.Point(17, 92);
			this.labelWarning.Name = "labelWarning";
			this.labelWarning.Size = new System.Drawing.Size(180, 14);
			this.labelWarning.TabIndex = 85;
			this.labelWarning.Text = "One Per Line";
			this.labelWarning.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.labelWarning.Visible = false;
			// 
			// checkHidden
			// 
			this.checkHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHidden.Location = new System.Drawing.Point(203, 72);
			this.checkHidden.Name = "checkHidden";
			this.checkHidden.Size = new System.Drawing.Size(126, 17);
			this.checkHidden.TabIndex = 86;
			this.checkHidden.Text = "Hidden";
			this.checkHidden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHidden.UseVisualStyleBackColor = true;
			// 
			// FormPatFieldDefEdit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(349, 493);
			this.Controls.Add(this.checkHidden);
			this.Controls.Add(this.labelWarning);
			this.Controls.Add(this.textPickList);
			this.Controls.Add(this.labelFieldType);
			this.Controls.Add(this.comboFieldType);
			this.Controls.Add(this.labelStatus);
			this.Controls.Add(this.buttonDelete);
			this.Controls.Add(this.textName);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormPatFieldDefEdit";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Patient Field Def";
			this.Load += new System.EventHandler(this.FormPatFieldDefEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormPatFieldDefEdit_Load(object sender, System.EventArgs e) {
			textName.Text=FieldDef.FieldName;
			textPickList.Visible=false;
			textName.ReadOnly=false;
			comboFieldType.Items.Clear();
			comboFieldType.Items.AddRange(Enum.GetNames(typeof(PatFieldType)));
			//DEPRECATED. (Only used 16.3.1,deprecated by 16.3.4)
			comboFieldType.Items.Remove("InCaseOfEmergency");
			comboFieldType.SelectedIndex=(int)FieldDef.FieldType;
			if(!IsNew){
				OldFieldName=FieldDef.FieldName;
				checkHidden.Checked=FieldDef.IsHidden;
			}
			if(comboFieldType.SelectedIndex==(int)PatFieldType.PickList) {
				textPickList.Visible=true;
				labelWarning.Visible=true;
				textPickList.Text=FieldDef.PickList;
			}
		}

		private void comboFieldType_SelectedIndexChanged(object sender,EventArgs e) {
			if(!IsNew){
				//todo: check existing values to make sure that it makes sense to change the type.  Especially when moving to currency or date.
			}
			textPickList.Visible=false;
			labelWarning.Visible=false;
			textName.ReadOnly=false;
			if(comboFieldType.SelectedIndex==(int)PatFieldType.PickList) {
				textPickList.Visible=true;
				labelWarning.Visible=true;
			}
		}

		private void buttonDelete_Click(object sender,EventArgs e) {
			if(IsNew){
				DialogResult=DialogResult.Cancel;
				return;
			}
			try{
				PatFieldDefs.Delete(FieldDef);//Throws if in use.
				FieldDefLinks.DeleteForFieldDefNum(FieldDef.PatFieldDefNum,FieldDefTypes.Patient);//Delete any FieldDefLinks to this PatFieldDef
				FieldDef=null;
				DialogResult=DialogResult.OK;
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(OldFieldName!=textName.Text && !IsNew) {
				if(!PrefC.GetBool(PrefName.DisplayRenamedPatFields)
					&& !MsgBox.Show(this,MsgBoxButtons.YesNo,"Changing the field name will cause existing information for this field to be hidden.  Continue?")) 
				{
					textName.Text=OldFieldName;
					return;
				}
				if(PatFieldDefs.GetExists(x => x.FieldName==textName.Text)) {
					MsgBox.Show(this,"Field name currently being used.");
					return;
				}
			}
			FieldDef.FieldName=textName.Text;
			FieldDef.IsHidden=checkHidden.Checked;
			FieldDef.FieldType=(PatFieldType)comboFieldType.SelectedIndex;
			if(FieldDef.FieldType==PatFieldType.PickList) {
				if(textPickList.Text=="") {
					MsgBox.Show(this,"List cannot be blank.");
					return;
				}
				FieldDef.PickList=textPickList.Text;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}



	

		


	}
}





















