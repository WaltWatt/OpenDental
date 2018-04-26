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

namespace OpenDental{
///<summary></summary>
	public class FormDefEditImages:ODForm {
		private System.Windows.Forms.Label labelName;
		private System.Windows.Forms.TextBox textName;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.ComponentModel.Container components = null;// Required designer variable.
		///<summary></summary>
		public bool IsNew;
		private System.Windows.Forms.CheckBox checkHidden;
		private Def DefCur;
		private CheckBox checkT;
		private CheckBox checkS;
		private CheckBox checkP;
		private CheckBox checkX;
		private CheckBox checkF;
		private CheckBox checkR;
		private CheckBox checkL;
		private CheckBox checkE;
		private CheckBox checkA;
		private GroupBox groupBox1;
		
		///<summary></summary>
		public FormDefEditImages(Def defCur) {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDefEditImages));
			this.labelName = new System.Windows.Forms.Label();
			this.textName = new System.Windows.Forms.TextBox();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.checkHidden = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkA = new System.Windows.Forms.CheckBox();
			this.checkE = new System.Windows.Forms.CheckBox();
			this.checkL = new System.Windows.Forms.CheckBox();
			this.checkR = new System.Windows.Forms.CheckBox();
			this.checkF = new System.Windows.Forms.CheckBox();
			this.checkT = new System.Windows.Forms.CheckBox();
			this.checkS = new System.Windows.Forms.CheckBox();
			this.checkP = new System.Windows.Forms.CheckBox();
			this.checkX = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelName
			// 
			this.labelName.Location = new System.Drawing.Point(47, 24);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(150, 16);
			this.labelName.TabIndex = 0;
			this.labelName.Text = "Name";
			this.labelName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// textName
			// 
			this.textName.Location = new System.Drawing.Point(32, 40);
			this.textName.Name = "textName";
			this.textName.Size = new System.Drawing.Size(178, 20);
			this.textName.TabIndex = 0;
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
			this.butOK.Location = new System.Drawing.Point(396, 226);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
			this.butOK.TabIndex = 4;
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
			this.butCancel.Location = new System.Drawing.Point(491, 226);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 5;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// checkHidden
			// 
			this.checkHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkHidden.Location = new System.Drawing.Point(449, 38);
			this.checkHidden.Name = "checkHidden";
			this.checkHidden.Size = new System.Drawing.Size(99, 24);
			this.checkHidden.TabIndex = 3;
			this.checkHidden.Text = "Hidden";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.checkA);
			this.groupBox1.Controls.Add(this.checkE);
			this.groupBox1.Controls.Add(this.checkL);
			this.groupBox1.Controls.Add(this.checkR);
			this.groupBox1.Controls.Add(this.checkF);
			this.groupBox1.Controls.Add(this.checkT);
			this.groupBox1.Controls.Add(this.checkS);
			this.groupBox1.Controls.Add(this.checkP);
			this.groupBox1.Controls.Add(this.checkX);
			this.groupBox1.Location = new System.Drawing.Point(228, 24);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(215, 187);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Usage";
			// 
			// checkA
			// 
			this.checkA.AutoSize = true;
			this.checkA.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkA.Location = new System.Drawing.Point(8, 161);
			this.checkA.Name = "checkA";
			this.checkA.Size = new System.Drawing.Size(151, 18);
			this.checkA.TabIndex = 12;
			this.checkA.Text = "Payment Plans (only one)";
			this.checkA.CheckedChanged += new System.EventHandler(this.CheckA_CheckedChanged);
			// 
			// checkE
			// 
			this.checkE.AutoSize = true;
			this.checkE.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkE.Location = new System.Drawing.Point(8, 17);
			this.checkE.Name = "checkE";
			this.checkE.Size = new System.Drawing.Size(129, 18);
			this.checkE.TabIndex = 11;
			this.checkE.Text = "Expanded by default";
			this.checkE.UseVisualStyleBackColor = true;
			// 
			// checkL
			// 
			this.checkL.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkL.Location = new System.Drawing.Point(8, 71);
			this.checkL.Name = "checkL";
			this.checkL.Size = new System.Drawing.Size(201, 18);
			this.checkL.TabIndex = 10;
			this.checkL.Text = "Show in Patient Portal";
			// 
			// checkR
			// 
			this.checkR.AutoSize = true;
			this.checkR.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkR.Location = new System.Drawing.Point(8, 143);
			this.checkR.Name = "checkR";
			this.checkR.Size = new System.Drawing.Size(158, 18);
			this.checkR.TabIndex = 9;
			this.checkR.Text = "Treatment Plans (only one)";
			this.checkR.UseVisualStyleBackColor = true;
			this.checkR.CheckedChanged += new System.EventHandler(this.checkR_CheckedChanged);
			// 
			// checkF
			// 
			this.checkF.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkF.Location = new System.Drawing.Point(8, 53);
			this.checkF.Name = "checkF";
			this.checkF.Size = new System.Drawing.Size(201, 18);
			this.checkF.TabIndex = 8;
			this.checkF.Text = "Show in Patient Forms";
			// 
			// checkT
			// 
			this.checkT.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkT.Location = new System.Drawing.Point(8, 125);
			this.checkT.Name = "checkT";
			this.checkT.Size = new System.Drawing.Size(201, 18);
			this.checkT.TabIndex = 7;
			this.checkT.Text = "Graphical Tooth Charts (only one)";
			this.checkT.CheckedChanged += new System.EventHandler(this.checkT_CheckedChanged);
			// 
			// checkS
			// 
			this.checkS.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkS.Location = new System.Drawing.Point(8, 107);
			this.checkS.Name = "checkS";
			this.checkS.Size = new System.Drawing.Size(201, 18);
			this.checkS.TabIndex = 6;
			this.checkS.Text = "Statements (only one)";
			this.checkS.CheckedChanged += new System.EventHandler(this.checkS_CheckedChanged);
			// 
			// checkP
			// 
			this.checkP.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkP.Location = new System.Drawing.Point(8, 89);
			this.checkP.Name = "checkP";
			this.checkP.Size = new System.Drawing.Size(201, 18);
			this.checkP.TabIndex = 5;
			this.checkP.Text = "Patient Pictures (only one)";
			this.checkP.CheckedChanged += new System.EventHandler(this.checkP_CheckedChanged);
			// 
			// checkX
			// 
			this.checkX.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkX.Location = new System.Drawing.Point(8, 35);
			this.checkX.Name = "checkX";
			this.checkX.Size = new System.Drawing.Size(201, 18);
			this.checkX.TabIndex = 4;
			this.checkX.Text = "Show in Chart module";
			// 
			// FormDefEditImages
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(578, 263);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.checkHidden);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.textName);
			this.Controls.Add(this.labelName);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormDefEditImages";
			this.ShowInTaskbar = false;
			this.Text = "Edit Image Category";
			this.Load += new System.EventHandler(this.FormDefEdit_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormDefEdit_Load(object sender,System.EventArgs e) {
			//Also see Defs.GetImageCat and ImageCategorySpecial when reworking this form.
			textName.Text=DefCur.ItemName;
			//textValue.Text=DefCur.ItemValue;
			if(DefCur.ItemValue.Contains("X")) {
				checkX.Checked=true;
			}
			if(DefCur.ItemValue.Contains("F")) {
				checkF.Checked=true;
			}
			if(DefCur.ItemValue.Contains("L")) {
				checkL.Checked=true;
			}
			if(DefCur.ItemValue.Contains("P")) {
				checkP.Checked=true;
			}
			if(DefCur.ItemValue.Contains("S")) {
				checkS.Checked=true;
			}
			if(DefCur.ItemValue.Contains("T")) {
				checkT.Checked=true;
			}
			if(DefCur.ItemValue.Contains("R")) {
				checkR.Checked=true;
			}
			if(DefCur.ItemValue.Contains("E") || (IsNew && PrefC.GetInt(PrefName.ImagesModuleTreeIsCollapsed)!=1)) {
					checkE.Checked=true;
			}
			if(DefCur.ItemValue.Contains("A")) {
				checkA.Checked=true;
			}
			checkHidden.Checked=DefCur.IsHidden;
		}
		private void butOK_Click(object sender, System.EventArgs e) {
			if(checkHidden.Checked) {
				if(Defs.IsDefinitionInUse(DefCur)) {
					if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Warning: This definition is currently in use within the program.")) {
						return;
					}
				}
			}
			if(textName.Text==""){
				MsgBox.Show(this,"Name required.");
				return;
			}
			DefCur.ItemName=textName.Text;
			string itemVal="";
			if(checkX.Checked) {
				itemVal+="X";
			}
			if(checkF.Checked) {
				itemVal+="F";
			}
			if(checkL.Checked) {
				itemVal+="L";
			}
			if(checkP.Checked) {
				itemVal+="P";
			}
			if(checkS.Checked) {
				itemVal+="S";
			}
			if(checkT.Checked) {
				itemVal+="T";
			}
			if(checkR.Checked) {
				itemVal+="R";
			}
      if(checkE.Checked) {
				itemVal+="E";
			}
			if(checkA.Checked) {
				itemVal+="A";
			}
			if(!IsNew && checkE.Checked != DefCur.ItemValue.Contains("E")) {//If checkbox has been changed since opening form.
				if(MsgBox.Show(this,true,"Expanded by default option changed.  This change will affect all users.  Continue?")) {
					//Remove all user specific preferences to enforce the new default.
					UserOdPrefs.DeleteForFkey(0,UserOdFkeyType.Definition,DefCur.DefNum);
				}
			}
			DefCur.ItemValue=itemVal;
			DefCur.IsHidden=checkHidden.Checked;
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

		private void checkP_CheckedChanged(object sender,EventArgs e) {
			if(checkP.Checked) {
				checkS.Checked=false;
				checkR.Checked=false;
				checkT.Checked=false;
				checkA.Checked=false;
			}
		}

		private void checkS_CheckedChanged(object sender,EventArgs e) {
			if(checkS.Checked) {
				checkP.Checked=false;
				checkR.Checked=false;
				checkT.Checked=false;
				checkA.Checked=false;
			}
		}

		private void checkT_CheckedChanged(object sender,EventArgs e) {
			if(checkT.Checked) {
				checkS.Checked=false;
				checkR.Checked=false;
				checkP.Checked=false;
				checkA.Checked=false;
			}
		}

		private void checkR_CheckedChanged(object sender,EventArgs e) {
			if(checkR.Checked) {
				checkS.Checked=false;
				checkP.Checked=false;
				checkT.Checked=false;
				checkA.Checked=false;
			}
		}

		private void CheckA_CheckedChanged(object sender,EventArgs e) {
			if(checkA.Checked) {
				checkS.Checked=false;
				checkP.Checked=false;
				checkT.Checked=false;
				checkR.Checked=false;
			}
		}



	}
}
