using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenDental{
///<summary></summary>
	public class FormRpPrintPreview : ODForm {
		///<summary></summary>
		public System.Windows.Forms.PrintPreviewControl printPreviewControl2;
		private OpenDental.UI.Button butNext;
		private OpenDental.UI.Button butPrev;
		private System.ComponentModel.Container components = null;

		///<summary></summary>
		public FormRpPrintPreview(){
			InitializeComponent();
 			Lan.F(this);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpPrintPreview));
			this.printPreviewControl2 = new System.Windows.Forms.PrintPreviewControl();
			this.butNext = new OpenDental.UI.Button();
			this.butPrev = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// printPreviewControl2
			// 
			this.printPreviewControl2.AutoZoom = false;
			this.printPreviewControl2.Location = new System.Drawing.Point(0, 0);
			this.printPreviewControl2.Name = "printPreviewControl2";
			this.printPreviewControl2.Size = new System.Drawing.Size(842, 538);
			this.printPreviewControl2.TabIndex = 7;
			this.printPreviewControl2.Zoom = 1D;
			// 
			// button1
			// 
			this.butNext.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNext.Autosize = true;
			this.butNext.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNext.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNext.CornerRadius = 4F;
			this.butNext.Location = new System.Drawing.Point(323, 709);
			this.butNext.Name = "button1";
			this.butNext.Size = new System.Drawing.Size(75, 23);
			this.butNext.TabIndex = 8;
			this.butNext.Text = "Next Page";
			this.butNext.Click += new System.EventHandler(this.butNext_Click);
			// 
			// button2
			// 
			this.butPrev.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrev.Autosize = true;
			this.butPrev.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrev.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrev.CornerRadius = 4F;
			this.butPrev.Location = new System.Drawing.Point(12, 709);
			this.butPrev.Name = "button2";
			this.butPrev.Size = new System.Drawing.Size(75, 23);
			this.butPrev.TabIndex = 9;
			this.butPrev.Text = "Prev. Page";
			this.butPrev.Click += new System.EventHandler(this.butPrev_Click);
			// 
			// FormRpPrintPreview
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(842, 746);
			this.Controls.Add(this.butPrev);
			this.Controls.Add(this.butNext);
			this.Controls.Add(this.printPreviewControl2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormRpPrintPreview";
			this.ShowInTaskbar = false;
			this.Text = "FormRpPrintPreview";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.FormRpPrintPreview_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormRpPrintPreview_Load(object sender, System.EventArgs e) {
			butNext.Location=new Point(this.ClientRectangle.Width-100,this.ClientRectangle.Height-30);
			butPrev.Location=new Point(this.ClientRectangle.Width-butPrev.Width-110,this.ClientRectangle.Height-30);
			printPreviewControl2.Height=this.ClientRectangle.Height-40;
			printPreviewControl2.Width=this.ClientRectangle.Width;
			printPreviewControl2.Zoom=(double)printPreviewControl2.ClientSize.Height
				/1100;
		}

		private void butNext_Click(object sender,System.EventArgs e) {
			printPreviewControl2.StartPage++;
		}

		private void butPrev_Click(object sender,EventArgs e) {
			if(printPreviewControl2.StartPage>0) {
				printPreviewControl2.StartPage--;
			}
		}
	}
}
