using System;
using OpenDentBusiness;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
//using mshtml;

namespace OpenDental{
	///<summary></summary>
	public class FormAbout : ODForm{
		private System.Windows.Forms.Label labelVersion;
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.Label labelCopyright;
		private System.Windows.Forms.Label labelMySQLCopyright;
		private System.Windows.Forms.Label label4;
		private UI.Button butLicense;
		private Label label9;
		private Label labelName;
		private Label labelService;
		private Label labelMySqlVersion;
		private Label labelServComment;
		private Label label2;
		private GroupBox groupBox3;
		private UI.ODPictureBox pictureOpenDental;
		private Label labelMachineName;
		private System.ComponentModel.Container components = null;

		///<summary></summary>
		public FormAbout(){
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
			this.labelVersion = new System.Windows.Forms.Label();
			this.butClose = new OpenDental.UI.Button();
			this.labelCopyright = new System.Windows.Forms.Label();
			this.labelMySQLCopyright = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.butLicense = new OpenDental.UI.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.labelName = new System.Windows.Forms.Label();
			this.labelService = new System.Windows.Forms.Label();
			this.labelMySqlVersion = new System.Windows.Forms.Label();
			this.labelServComment = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.labelMachineName = new System.Windows.Forms.Label();
			this.pictureOpenDental = new OpenDental.UI.ODPictureBox();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelVersion
			// 
			this.labelVersion.Location = new System.Drawing.Point(257, 20);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(229, 20);
			this.labelVersion.TabIndex = 1;
			this.labelVersion.Text = "Version:";
			this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.butClose.Location = new System.Drawing.Point(541, 284);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 25);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// labelCopyright
			// 
			this.labelCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelCopyright.Location = new System.Drawing.Point(12, 248);
			this.labelCopyright.Name = "labelCopyright";
			this.labelCopyright.Size = new System.Drawing.Size(550, 20);
			this.labelCopyright.TabIndex = 3;
			this.labelCopyright.Text = "This program Copyright 2003-2007, Jordan S. Sparks, D.M.D.";
			this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelMySQLCopyright
			// 
			this.labelMySQLCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelMySQLCopyright.Location = new System.Drawing.Point(12, 292);
			this.labelMySQLCopyright.Name = "labelMySQLCopyright";
			this.labelMySQLCopyright.Size = new System.Drawing.Size(433, 20);
			this.labelMySQLCopyright.TabIndex = 6;
			this.labelMySQLCopyright.Text = "MySQL - Copyright 1995-2007, www.mysql.com";
			this.labelMySQLCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.Location = new System.Drawing.Point(12, 229);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(549, 20);
			this.label4.TabIndex = 7;
			this.label4.Text = "All parts of this program are licensed under the GPL, www.opensource.org/licenses" +
    "/gpl-license.php";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butLicense
			// 
			this.butLicense.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLicense.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butLicense.Autosize = true;
			this.butLicense.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLicense.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLicense.CornerRadius = 4F;
			this.butLicense.Location = new System.Drawing.Point(447, 284);
			this.butLicense.Name = "butLicense";
			this.butLicense.Size = new System.Drawing.Size(88, 25);
			this.butLicense.TabIndex = 50;
			this.butLicense.Text = "View Licenses";
			this.butLicense.Click += new System.EventHandler(this.butLicense_Click);
			// 
			// label9
			// 
			this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label9.Location = new System.Drawing.Point(12, 270);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(433, 20);
			this.label9.TabIndex = 51;
			this.label9.Text = "All CDT codes are Copyrighted by the ADA.";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelName
			// 
			this.labelName.Location = new System.Drawing.Point(10, 43);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(474, 20);
			this.labelName.TabIndex = 52;
			this.labelName.Text = "Server Name: ";
			this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelService
			// 
			this.labelService.Location = new System.Drawing.Point(10, 68);
			this.labelService.Name = "labelService";
			this.labelService.Size = new System.Drawing.Size(474, 20);
			this.labelService.TabIndex = 53;
			this.labelService.Text = "Service Name: ";
			this.labelService.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelMySqlVersion
			// 
			this.labelMySqlVersion.Location = new System.Drawing.Point(10, 93);
			this.labelMySqlVersion.Name = "labelMySqlVersion";
			this.labelMySqlVersion.Size = new System.Drawing.Size(474, 20);
			this.labelMySqlVersion.TabIndex = 54;
			this.labelMySqlVersion.Text = "Service Version: ";
			this.labelMySqlVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelServComment
			// 
			this.labelServComment.Location = new System.Drawing.Point(10, 118);
			this.labelServComment.Name = "labelServComment";
			this.labelServComment.Size = new System.Drawing.Size(474, 20);
			this.labelServComment.TabIndex = 55;
			this.labelServComment.Text = "Service Comment: ";
			this.labelServComment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label2.Location = new System.Drawing.Point(9, 222);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(600, 2);
			this.label2.TabIndex = 56;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.labelMachineName);
			this.groupBox3.Controls.Add(this.labelService);
			this.groupBox3.Controls.Add(this.labelName);
			this.groupBox3.Controls.Add(this.labelServComment);
			this.groupBox3.Controls.Add(this.labelMySqlVersion);
			this.groupBox3.Location = new System.Drawing.Point(60, 55);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(501, 147);
			this.groupBox3.TabIndex = 57;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Current Connection";
			// 
			// labelMachineName
			// 
			this.labelMachineName.Location = new System.Drawing.Point(10, 18);
			this.labelMachineName.Name = "labelMachineName";
			this.labelMachineName.Size = new System.Drawing.Size(474, 20);
			this.labelMachineName.TabIndex = 87;
			this.labelMachineName.Text = "Client or Remote Application Machine Name: ";
			this.labelMachineName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pictureOpenDental
			// 
			this.pictureOpenDental.HasBorder = false;
			this.pictureOpenDental.Image = global::OpenDental.Properties.Resources.ODLogo;
			this.pictureOpenDental.Location = new System.Drawing.Point(143, 5);
			this.pictureOpenDental.Name = "pictureOpenDental";
			this.pictureOpenDental.Size = new System.Drawing.Size(106, 48);
			this.pictureOpenDental.TabIndex = 58;
			this.pictureOpenDental.TextNullImage = null;
			// 
			// FormAbout
			// 
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(628, 332);
			this.Controls.Add(this.pictureOpenDental);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.butLicense);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.labelMySQLCopyright);
			this.Controls.Add(this.labelCopyright);
			this.Controls.Add(this.labelVersion);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAbout";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "About";
			this.Load += new System.EventHandler(this.FormAbout_Load);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormAbout_Load(object sender, System.EventArgs e) {
			string softwareName=PrefC.GetString(PrefName.SoftwareName);
			if(softwareName!="Open Dental Software") {
				pictureOpenDental.Visible=false;
			}
			labelVersion.Text=Lan.g(this,"Version:")+" "+Application.ProductVersion;
			UpdateHistory updateHistory=UpdateHistories.GetForVersion(Application.ProductVersion);
			if(updateHistory!=null) {
				labelVersion.Text+="  "+Lan.g(this,"Since:")+" "+updateHistory.DateTimeUpdated.ToShortDateString();
			}
			//keeps the trailing year up to date
			labelCopyright.Text=softwareName+" "+Lan.g(this,"Copyright 2003-")+DateTime.Now.ToString("yyyy")+", Jordan S. Sparks, D.M.D.";
			labelMySQLCopyright.Text=Lan.g(this,"MySQL - Copyright 1995-")+DateTime.Now.ToString("yyyy")+Lan.g(this,", www.mysql.com");
			//Database Server----------------------------------------------------------		
			List<string> serviceList=Computers.GetServiceInfo();
			labelName.Text+=serviceList[2].ToString();//MiscData.GetODServer();//server name
			labelService.Text+=serviceList[0].ToString();//service name
			labelMySqlVersion.Text+=serviceList[3].ToString();//service version
			labelServComment.Text+=serviceList[1].ToString();//service comment
			labelMachineName.Text+=Environment.MachineName.ToUpper();//current client or remote application machine name
		}

		private void butLicense_Click(object sender,EventArgs e) {
			FormLicense FormL=new FormLicense();
			FormL.ShowDialog();
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}
