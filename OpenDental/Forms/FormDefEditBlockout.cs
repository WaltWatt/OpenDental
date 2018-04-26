
/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Linq;

namespace OpenDental{
///<summary></summary>
	public class FormDefEditBlockout:ODForm {
		private System.Windows.Forms.Label labelName;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.ComponentModel.Container components = null;// Required designer variable.
		///<summary></summary>
		public bool IsNew;
		private System.Windows.Forms.CheckBox checkHidden;
		private Def DefCur;
		private CheckBox checkCutCopyPaste;
		private CheckBox checkOverlap;
		private Button butColor;
		private Label labelColor;
		private TextBox textName;
		private GroupBox groupBoxUsage;
		
		///<summary></summary>
		public FormDefEditBlockout(Def defCur) {
			InitializeComponent();// Required for Windows Form Designer support
			Lan.F(this);
			DefCur=defCur.Copy();
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDefEditBlockout));
			this.labelName = new System.Windows.Forms.Label();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.checkHidden = new System.Windows.Forms.CheckBox();
			this.groupBoxUsage = new System.Windows.Forms.GroupBox();
			this.checkOverlap = new System.Windows.Forms.CheckBox();
			this.checkCutCopyPaste = new System.Windows.Forms.CheckBox();
			this.butColor = new System.Windows.Forms.Button();
			this.labelColor = new System.Windows.Forms.Label();
			this.textName = new System.Windows.Forms.TextBox();
			this.groupBoxUsage.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelName
			// 
			this.labelName.Location = new System.Drawing.Point(9, 38);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(150, 16);
			this.labelName.TabIndex = 0;
			this.labelName.Text = "Name";
			this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// colorDialog1
			// 
			this.colorDialog1.FullOpen = true;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(325, 135);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
			this.butOK.TabIndex = 2;
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
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(406, 135);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 3;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// checkHidden
			// 
			this.checkHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkHidden.Location = new System.Drawing.Point(12, 11);
			this.checkHidden.Name = "checkHidden";
			this.checkHidden.Size = new System.Drawing.Size(99, 24);
			this.checkHidden.TabIndex = 3;
			this.checkHidden.Text = "Hidden";
			// 
			// groupBoxUsage
			// 
			this.groupBoxUsage.Controls.Add(this.checkOverlap);
			this.groupBoxUsage.Controls.Add(this.checkCutCopyPaste);
			this.groupBoxUsage.Location = new System.Drawing.Point(196, 54);
			this.groupBoxUsage.Name = "groupBoxUsage";
			this.groupBoxUsage.Size = new System.Drawing.Size(215, 62);
			this.groupBoxUsage.TabIndex = 7;
			this.groupBoxUsage.TabStop = false;
			this.groupBoxUsage.Text = "Usage";
			// 
			// checkOverlap
			// 
			this.checkOverlap.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkOverlap.Location = new System.Drawing.Point(8, 17);
			this.checkOverlap.Name = "checkOverlap";
			this.checkOverlap.Size = new System.Drawing.Size(196, 18);
			this.checkOverlap.TabIndex = 0;
			this.checkOverlap.Text = "Block appointments scheduling";
			this.checkOverlap.UseVisualStyleBackColor = true;
			// 
			// checkCutCopyPaste
			// 
			this.checkCutCopyPaste.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkCutCopyPaste.Location = new System.Drawing.Point(8, 35);
			this.checkCutCopyPaste.Name = "checkCutCopyPaste";
			this.checkCutCopyPaste.Size = new System.Drawing.Size(201, 18);
			this.checkCutCopyPaste.TabIndex = 1;
			this.checkCutCopyPaste.Text = "Disable Cut/Copy/Paste";
			// 
			// butColor
			// 
			this.butColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColor.Location = new System.Drawing.Point(419, 60);
			this.butColor.Name = "butColor";
			this.butColor.Size = new System.Drawing.Size(30, 20);
			this.butColor.TabIndex = 1;
			this.butColor.Click += new System.EventHandler(this.butColor_Click);
			// 
			// labelColor
			// 
			this.labelColor.Location = new System.Drawing.Point(419, 42);
			this.labelColor.Name = "labelColor";
			this.labelColor.Size = new System.Drawing.Size(74, 16);
			this.labelColor.TabIndex = 10;
			this.labelColor.Text = "Color";
			this.labelColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textName
			// 
			this.textName.Location = new System.Drawing.Point(12, 54);
			this.textName.Multiline = true;
			this.textName.Name = "textName";
			this.textName.Size = new System.Drawing.Size(178, 64);
			this.textName.TabIndex = 11;
			// 
			// FormDefEditBlockout
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(493, 172);
			this.Controls.Add(this.textName);
			this.Controls.Add(this.butColor);
			this.Controls.Add(this.labelColor);
			this.Controls.Add(this.groupBoxUsage);
			this.Controls.Add(this.checkHidden);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.labelName);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(509, 188);
			this.Name = "FormDefEditBlockout";
			this.ShowInTaskbar = false;
			this.Text = "Edit Blockout Type";
			this.Load += new System.EventHandler(this.FormDefEdit_Load);
			this.groupBoxUsage.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormDefEdit_Load(object sender,System.EventArgs e) {
			textName.Text=DefCur.ItemName;
			if(DefCur.ItemValue.Contains(BlockoutType.DontCopy.GetDescription())) {
				checkCutCopyPaste.Checked=true;
			}
			if(DefCur.ItemValue.Contains(BlockoutType.NoSchedule.GetDescription())) {
				checkOverlap.Checked=true;
			}
			checkHidden.Checked=DefCur.IsHidden;
			butColor.BackColor=DefCur.ItemColor;
		}

		private void butColor_Click(object sender,EventArgs e) {
			colorDialog1.Color=butColor.BackColor;
			colorDialog1.ShowDialog();
			butColor.BackColor=colorDialog1.Color;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textName.Text==""){
				MsgBox.Show(this,"Name required.");
				return;
			}
			DefCur.ItemName=textName.Text;
			List<string> itemVal=new List<string>();
			if(checkCutCopyPaste.Checked) {
				itemVal.Add(BlockoutType.DontCopy.GetDescription());
			}
			if(checkOverlap.Checked) {
				itemVal.Add(BlockoutType.NoSchedule.GetDescription());
			}
			DefCur.ItemValue=string.Join(",", itemVal);
			DefCur.IsHidden=checkHidden.Checked;
			DefCur.ItemColor=butColor.BackColor;
			if(IsNew){
				Defs.Insert(DefCur);
			}
			else{
				Defs.Update(DefCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}