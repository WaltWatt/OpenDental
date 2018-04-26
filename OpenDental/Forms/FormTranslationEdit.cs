using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Globalization;
using System.IO;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public class FormTranslationEdit : ODForm {
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textEnglish;
		private System.Windows.Forms.Label label4;
		private bool IsNew;
		//private bool IsNewCulTran;
		private System.Windows.Forms.TextBox textComments;
		private System.Windows.Forms.TextBox textTranslation;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textOtherTranslation;
		private System.ComponentModel.Container components = null;
		private Language LanCur;
		private LanguageForeign LanForeign;
		private string OtherTrans;

		///<summary>lanForeign might be null.</summary>
		public FormTranslationEdit(Language lanCur,LanguageForeign lanForeign,string otherTrans){
			InitializeComponent();
			//no need to translate anything here
			LanCur=lanCur;
			LanForeign=lanForeign;
			OtherTrans=otherTrans;
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

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTranslationEdit));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textEnglish = new System.Windows.Forms.TextBox();
			this.textTranslation = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textComments = new System.Windows.Forms.TextBox();
			this.textOtherTranslation = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(786,594);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,26);
			this.butOK.TabIndex = 0;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(786,628);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,26);
			this.butCancel.TabIndex = 1;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(43,36);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82,23);
			this.label1.TabIndex = 2;
			this.label1.Text = "English";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(38,168);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88,16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Translation";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textEnglish
			// 
			this.textEnglish.AcceptsReturn = true;
			this.textEnglish.Location = new System.Drawing.Point(127,34);
			this.textEnglish.Multiline = true;
			this.textEnglish.Name = "textEnglish";
			this.textEnglish.ReadOnly = true;
			this.textEnglish.Size = new System.Drawing.Size(672,130);
			this.textEnglish.TabIndex = 6;
			// 
			// textTranslation
			// 
			this.textTranslation.AcceptsReturn = true;
			this.textTranslation.Location = new System.Drawing.Point(127,166);
			this.textTranslation.Multiline = true;
			this.textTranslation.Name = "textTranslation";
			this.textTranslation.Size = new System.Drawing.Size(672,130);
			this.textTranslation.TabIndex = 0;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(44,434);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(82,14);
			this.label4.TabIndex = 10;
			this.label4.Text = "Comments";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textComments
			// 
			this.textComments.AcceptsReturn = true;
			this.textComments.Location = new System.Drawing.Point(127,432);
			this.textComments.Multiline = true;
			this.textComments.Name = "textComments";
			this.textComments.Size = new System.Drawing.Size(672,130);
			this.textComments.TabIndex = 11;
			// 
			// textOtherTranslation
			// 
			this.textOtherTranslation.Location = new System.Drawing.Point(127,299);
			this.textOtherTranslation.Multiline = true;
			this.textOtherTranslation.Name = "textOtherTranslation";
			this.textOtherTranslation.ReadOnly = true;
			this.textOtherTranslation.Size = new System.Drawing.Size(671,130);
			this.textOtherTranslation.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(4,301);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(121,16);
			this.label3.TabIndex = 12;
			this.label3.Text = "Other Translation";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// FormTranslationEdit
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(880,668);
			this.Controls.Add(this.textOtherTranslation);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textComments);
			this.Controls.Add(this.textTranslation);
			this.Controls.Add(this.textEnglish);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormTranslationEdit";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Translation Edit";
			this.Load += new System.EventHandler(this.FormTranslationEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormTranslationEdit_Load(object sender, System.EventArgs e){
			textEnglish.Text=LanCur.English;
			textOtherTranslation.Text=OtherTrans;
			if(LanForeign==null){
				LanForeign=new LanguageForeign();
				LanForeign.ClassType=LanCur.ClassType;
				LanForeign.English=LanCur.English;
				LanForeign.Culture=CultureInfo.CurrentCulture.Name;
				Text="Add Translation";
				IsNew=true;
			}
			else{
				//LanguageForeigns.Cur=((LanguageForeign)LanguageForeigns.HList[Lan.Cur.ClassType+Lan.Cur.English]);
				textTranslation.Text=LanForeign.Translation;
				textComments.Text=LanForeign.Comments;
				Text="Edit Translation";
				IsNew=false;
			}
		}

		private void butOK_Click(object sender, System.EventArgs e){
			if(textTranslation.Text=="" && textComments.Text==""){
				//If only the translation is "", then the Lan.g routine will simply ignore it and use English.
				if(!IsNew){
					if(MessageBox.Show("This translation is blank and will be deleted.  Continue?",""
						,MessageBoxButtons.OKCancel)!=DialogResult.OK)
					{
						return;
					}
					LanguageForeigns.Delete(LanForeign);
				}
				DialogResult=DialogResult.OK;
				return;
			}
			LanForeign.Translation=textTranslation.Text;
			LanForeign.Comments=textComments.Text;
			if(IsNew){
				LanguageForeigns.Insert(LanForeign);
			}
			else{
				LanguageForeigns.Update(LanForeign);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}
