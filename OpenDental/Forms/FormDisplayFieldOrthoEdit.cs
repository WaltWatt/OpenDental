using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary></summary>
	public class FormDisplayFieldOrthoEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private TextBox textInternalName;
		private Label label2;
		private Label label3;
		private ValidNum textWidth;
		private TextBox textWidthMin;
		private Label label4;
		private Label label5;
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.Container components = null;
		public DisplayField FieldCur;
		///<summary>Used to make sure the user is not adding a duplicate field.</summary>
		public List<DisplayField> ListAllFields;
		private Label labelLine;
		private TextBox textPickList;
		private UI.Button butDown;
		private UI.Button butUp;
		private CheckBox checkSignature;
		private TextBox textDisplayName;
		private Label label1;
		private Label label6;
		private Font headerFont=new Font(FontFamily.GenericSansSerif,8.5f,FontStyle.Bold);

		public FormDisplayFieldOrthoEdit()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDisplayFieldOrthoEdit));
			this.textInternalName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textWidthMin = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.labelLine = new System.Windows.Forms.Label();
			this.textPickList = new System.Windows.Forms.TextBox();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.textWidth = new OpenDental.ValidNum();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.checkSignature = new System.Windows.Forms.CheckBox();
			this.textDisplayName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// textInternalName
			// 
			this.textInternalName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textInternalName.Location = new System.Drawing.Point(142, 40);
			this.textInternalName.MaxLength = 255;
			this.textInternalName.Name = "textInternalName";
			this.textInternalName.Size = new System.Drawing.Size(249, 20);
			this.textInternalName.TabIndex = 5;
			this.textInternalName.TextChanged += new System.EventHandler(this.textInternalName_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(134, 17);
			this.label2.TabIndex = 4;
			this.label2.Text = "Internal Name";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 93);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(134, 17);
			this.label3.TabIndex = 6;
			this.label3.Text = "Column Width";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textWidthMin
			// 
			this.textWidthMin.Location = new System.Drawing.Point(142, 66);
			this.textWidthMin.Name = "textWidthMin";
			this.textWidthMin.ReadOnly = true;
			this.textWidthMin.Size = new System.Drawing.Size(71, 20);
			this.textWidthMin.TabIndex = 9;
			this.textWidthMin.TabStop = false;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(9, 67);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(131, 17);
			this.label4.TabIndex = 8;
			this.label4.Text = "Minimum Width";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(216, 67);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(175, 17);
			this.label5.TabIndex = 10;
			this.label5.Text = "(based on text above)";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelLine
			// 
			this.labelLine.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelLine.Location = new System.Drawing.Point(6, 140);
			this.labelLine.Name = "labelLine";
			this.labelLine.Size = new System.Drawing.Size(130, 14);
			this.labelLine.TabIndex = 89;
			this.labelLine.Text = "One Entry Per Line";
			this.labelLine.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// textPickList
			// 
			this.textPickList.AcceptsReturn = true;
			this.textPickList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textPickList.HideSelection = false;
			this.textPickList.Location = new System.Drawing.Point(142, 140);
			this.textPickList.Multiline = true;
			this.textPickList.Name = "textPickList";
			this.textPickList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textPickList.Size = new System.Drawing.Size(249, 250);
			this.textPickList.TabIndex = 20;
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(1, 0);
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.Location = new System.Drawing.Point(445, 164);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(25, 24);
			this.butDown.TabIndex = 30;
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(1, 0);
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.Location = new System.Drawing.Point(414, 164);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(25, 24);
			this.butUp.TabIndex = 25;
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// textWidth
			// 
			this.textWidth.Location = new System.Drawing.Point(142, 92);
			this.textWidth.MaxVal = 2000;
			this.textWidth.MinVal = 1;
			this.textWidth.Name = "textWidth";
			this.textWidth.Size = new System.Drawing.Size(71, 20);
			this.textWidth.TabIndex = 10;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(414, 329);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 35;
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
			this.butCancel.Location = new System.Drawing.Point(414, 364);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 40;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// checkSignature
			// 
			this.checkSignature.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSignature.Location = new System.Drawing.Point(142, 116);
			this.checkSignature.Name = "checkSignature";
			this.checkSignature.Size = new System.Drawing.Size(328, 18);
			this.checkSignature.TabIndex = 91;
			this.checkSignature.TabStop = false;
			this.checkSignature.Text = "Check to show a signature box in the Ortho Chart.";
			this.checkSignature.CheckedChanged += new System.EventHandler(this.checkSignature_CheckedChanged);
			// 
			// textDisplayName
			// 
			this.textDisplayName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDisplayName.Location = new System.Drawing.Point(142, 14);
			this.textDisplayName.MaxLength = 255;
			this.textDisplayName.Name = "textDisplayName";
			this.textDisplayName.Size = new System.Drawing.Size(249, 20);
			this.textDisplayName.TabIndex = 93;
			this.textDisplayName.TextChanged += new System.EventHandler(this.textDisplayName_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(134, 17);
			this.label1.TabIndex = 92;
			this.label1.Text = "Display Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(393, 15);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(101, 17);
			this.label6.TabIndex = 94;
			this.label6.Text = "(optional)";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormDisplayFieldOrthoEdit
			// 
			this.ClientSize = new System.Drawing.Size(510, 402);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textDisplayName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkSignature);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.labelLine);
			this.Controls.Add(this.textPickList);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textWidthMin);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textWidth);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textInternalName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormDisplayFieldOrthoEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Ortho Display Field";
			this.Load += new System.EventHandler(this.FormDisplayFieldOrthoEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormDisplayFieldOrthoEdit_Load(object sender,EventArgs e) {
			textDisplayName.Text=FieldCur.DescriptionOverride;
			textInternalName.Text=FieldCur.Description;
			textWidth.Text=FieldCur.ColumnWidth.ToString();
			textPickList.Text=FieldCur.PickList;
			if(FieldCur.InternalName=="Signature") {
				checkSignature.Checked=true;
				labelLine.Visible=false;
				textPickList.Visible=false;
				butUp.Visible=false;
				butDown.Visible=false;
			}
			FillWidth();
		}

		private void FillWidth(){
			Graphics g=this.CreateGraphics();
			//Use the display name text box by default because it is what the user will see if it is set.  It is optional so check for empty string.
			string text=string.IsNullOrEmpty(textDisplayName.Text) ? textInternalName.Text : textDisplayName.Text;
			int width=(int)g.MeasureString(text,headerFont).Width;
			textWidthMin.Text=width.ToString();
			g.Dispose();
		}

		private void textDisplayName_TextChanged(object sender,EventArgs e) {
			FillWidth();
		}

		private void textInternalName_TextChanged(object sender,EventArgs e) {
			FillWidth();
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(textPickList.Text==""){
				return;
			}
			int selectionStart=textPickList.SelectionStart;
			//calculate which row to highlight, based on selection start.
			int selectedRow=0;
			int sumPreviousLines=0;
			string[] linesOrig=new string[textPickList.Lines.Length];
			textPickList.Lines.CopyTo(linesOrig,0);
			for(int i=0;i<textPickList.Lines.Length;i++) {
				if(i>0) {
					sumPreviousLines+=textPickList.Lines[i-1].Length+2;//the 2 is for \r\n
				}
				if(selectionStart < sumPreviousLines+textPickList.Lines[i].Length) {
					selectedRow=i;
					break;
				}
			}
			//swap rows
			int newSelectedRow;
			if(selectedRow==0) {
				newSelectedRow=0;//and no swap
			}
			else {
				//doesn't allow me to directly set lines, so:
				string newtext="";
				for(int i=0;i<textPickList.Lines.Length;i++) {
					if(i>0) {
						newtext+="\r\n";
					}
					if(i==selectedRow) {
						newtext+=linesOrig[selectedRow-1];
					}
					else if(i==selectedRow-1) {
						newtext+=linesOrig[selectedRow];
					}
					else {
						newtext+=linesOrig[i];
					}
				}
				textPickList.Text=newtext;
				newSelectedRow=selectedRow-1;
			}
			//highlight the newSelectedRow
			sumPreviousLines=0;
			for(int i=0;i<textPickList.Lines.Length;i++) {
				if(i>0) {
					sumPreviousLines+=textPickList.Lines[i-1].Length+2;//the 2 is for \r\n
				}
				if(newSelectedRow==i) {
					textPickList.Select(sumPreviousLines,textPickList.Lines[i].Length);
					break;
				}
			}
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(textPickList.Text=="") {
				return;
			}
			int selectionStart=textPickList.SelectionStart;
			//calculate which row to highlight, based on selection start.
			int selectedRow=0;
			int sumPreviousLines=0;
			string[] linesOrig=new string[textPickList.Lines.Length];
			textPickList.Lines.CopyTo(linesOrig,0);
			for(int i=0;i<textPickList.Lines.Length;i++) {
				if(i>0) {
					sumPreviousLines+=textPickList.Lines[i-1].Length+2;//the 2 is for \r\n
				}
				if(selectionStart < sumPreviousLines+textPickList.Lines[i].Length) {
					selectedRow=i;
					break;
				}
			}
			//swap rows
			int newSelectedRow;
			if(selectedRow==textPickList.Lines.Length-1) {
				newSelectedRow=textPickList.Lines.Length-1;//and no swap
			}
			else {
				//doesn't allow me to directly set lines, so:
				string newtext="";
				for(int i=0;i<textPickList.Lines.Length;i++) {
					if(i>0) {
						newtext+="\r\n";
					}
					if(i==selectedRow) {
						newtext+=linesOrig[selectedRow+1];
					}
					else if(i==selectedRow+1) {
						newtext+=linesOrig[selectedRow];
					}
					else {
						newtext+=linesOrig[i];
					}
				}
				textPickList.Text=newtext;
				newSelectedRow=selectedRow+1;
			}
			//highlight the newSelectedRow
			sumPreviousLines=0;
			for(int i=0;i<textPickList.Lines.Length;i++) {
				if(i>0) {
					sumPreviousLines+=textPickList.Lines[i-1].Length+2;//the 2 is for \r\n
				}
				if(newSelectedRow==i) {
					textPickList.Select(sumPreviousLines,textPickList.Lines[i].Length);
					break;
				}
			}
		}

		private void checkSignature_CheckedChanged(object sender,EventArgs e) {
			if(checkSignature.Checked && textPickList.Text != "") {
				MsgBox.Show(this,"To make this display field a signature field, remove the pick list values first.");
				checkSignature.Checked=false;
				return;
			}
			labelLine.Visible=(!checkSignature.Checked);
			textPickList.Visible=(!checkSignature.Checked);
			butUp.Visible=(!checkSignature.Checked);
			butDown.Visible=(!checkSignature.Checked);
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textWidth.errorProvider1.GetError(textWidth)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(textInternalName.Text.Trim()=="") {
				MsgBox.Show(this,"Internal Name cannot be blank.");
				return;
			}
			//Verify that the user did not change the field name to the same name as another field.
			DisplayField displayFieldOther=ListAllFields.FirstOrDefault(x => x!=FieldCur && x.Description==FieldCur.Description);
			if(displayFieldOther!=null) {
				MsgBox.Show(this,"An ortho chart field with that Internal Name already exists.");
				return;
			}
			FieldCur.Description=textInternalName.Text;
			FieldCur.DescriptionOverride=textDisplayName.Text;
			FieldCur.ColumnWidth=PIn.Int(textWidth.Text);
			FieldCur.PickList=textPickList.Text;
			if(checkSignature.Checked) {
				FieldCur.InternalName="Signature";
			}
			else {
				FieldCur.InternalName="";
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}





















