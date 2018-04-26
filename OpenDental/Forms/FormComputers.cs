using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental{
///<summary></summary>
	public class FormComputers : ODForm {
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox listComputer;
		private OpenDental.UI.Button butDelete;
		private System.Windows.Forms.Label label2;
		private UI.Button butSetSimpleGraphics;
		private Label label3;
		private GroupBox groupBox1;// Required designer variable.
		private Label labelCurComp;
		private TextBox textCurComp;
		private GroupBox groupBox2;
		private GroupBox groupBox3;
		private TextBox textServComment;
		private TextBox textVersion;
		private TextBox textService;
		private TextBox textName;
		private Label labelServComment;
		private Label labelVersion;
		private Label labelService;
		private Label labelName;

		//private Programs Programs=new Programs();
		private bool changed;
		private List<Computer> _listComputers;

		///<summary></summary>
		public FormComputers(){
			InitializeComponent();// Required for Windows Form Designer support
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormComputers));
			this.listComputer = new System.Windows.Forms.ListBox();
			this.butClose = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.butSetSimpleGraphics = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textCurComp = new System.Windows.Forms.TextBox();
			this.labelCurComp = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.textServComment = new System.Windows.Forms.TextBox();
			this.textVersion = new System.Windows.Forms.TextBox();
			this.textService = new System.Windows.Forms.TextBox();
			this.textName = new System.Windows.Forms.TextBox();
			this.labelServComment = new System.Windows.Forms.Label();
			this.labelVersion = new System.Windows.Forms.Label();
			this.labelService = new System.Windows.Forms.Label();
			this.labelName = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// listComputer
			// 
			this.listComputer.Items.AddRange(new object[] {
            ""});
			this.listComputer.Location = new System.Drawing.Point(17, 265);
			this.listComputer.Name = "listComputer";
			this.listComputer.Size = new System.Drawing.Size(282, 277);
			this.listComputer.TabIndex = 2;
			this.listComputer.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listComputer_MouseDoubleClick);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(448, 613);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 3;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 62);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(409, 44);
			this.label1.TabIndex = 43;
			this.label1.Text = "Computers are added to this list every time you use Open Dental.  You can safely " +
    "delete unused computer names from this list to speed up messaging.";
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(5, 408);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75, 26);
			this.butDelete.TabIndex = 4;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 100);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(294, 15);
			this.label2.TabIndex = 45;
			this.label2.Text = "ComputerName";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butSetSimpleGraphics
			// 
			this.butSetSimpleGraphics.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSetSimpleGraphics.Autosize = true;
			this.butSetSimpleGraphics.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSetSimpleGraphics.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSetSimpleGraphics.CornerRadius = 4F;
			this.butSetSimpleGraphics.Location = new System.Drawing.Point(40, 182);
			this.butSetSimpleGraphics.Name = "butSetSimpleGraphics";
			this.butSetSimpleGraphics.Size = new System.Drawing.Size(115, 24);
			this.butSetSimpleGraphics.TabIndex = 3;
			this.butSetSimpleGraphics.Text = "Use Simple Graphics";
			this.butSetSimpleGraphics.Click += new System.EventHandler(this.butSetSimpleGraphics_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(18, 25);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(161, 151);
			this.label3.TabIndex = 82;
			this.label3.Text = resources.GetString("label3.Text");
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butSetSimpleGraphics);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Location = new System.Drawing.Point(310, 264);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(195, 219);
			this.groupBox1.TabIndex = 83;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Fix a Workstation";
			// 
			// textCurComp
			// 
			this.textCurComp.Enabled = false;
			this.textCurComp.Location = new System.Drawing.Point(119, 20);
			this.textCurComp.Name = "textCurComp";
			this.textCurComp.ReadOnly = true;
			this.textCurComp.Size = new System.Drawing.Size(282, 20);
			this.textCurComp.TabIndex = 1;
			// 
			// labelCurComp
			// 
			this.labelCurComp.Location = new System.Drawing.Point(6, 22);
			this.labelCurComp.Name = "labelCurComp";
			this.labelCurComp.Size = new System.Drawing.Size(110, 15);
			this.labelCurComp.TabIndex = 86;
			this.labelCurComp.Text = "Current Computer";
			this.labelCurComp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.textCurComp);
			this.groupBox2.Controls.Add(this.butDelete);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.labelCurComp);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Location = new System.Drawing.Point(12, 145);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(511, 447);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Workstation";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.textServComment);
			this.groupBox3.Controls.Add(this.textVersion);
			this.groupBox3.Controls.Add(this.textService);
			this.groupBox3.Controls.Add(this.textName);
			this.groupBox3.Controls.Add(this.labelServComment);
			this.groupBox3.Controls.Add(this.labelVersion);
			this.groupBox3.Controls.Add(this.labelService);
			this.groupBox3.Controls.Add(this.labelName);
			this.groupBox3.Location = new System.Drawing.Point(12, 12);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(511, 127);
			this.groupBox3.TabIndex = 1;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Database Server";
			// 
			// textServComment
			// 
			this.textServComment.Enabled = false;
			this.textServComment.Location = new System.Drawing.Point(119, 92);
			this.textServComment.Name = "textServComment";
			this.textServComment.ReadOnly = true;
			this.textServComment.Size = new System.Drawing.Size(282, 20);
			this.textServComment.TabIndex = 4;
			// 
			// textVersion
			// 
			this.textVersion.Enabled = false;
			this.textVersion.Location = new System.Drawing.Point(119, 68);
			this.textVersion.Name = "textVersion";
			this.textVersion.ReadOnly = true;
			this.textVersion.Size = new System.Drawing.Size(282, 20);
			this.textVersion.TabIndex = 3;
			// 
			// textService
			// 
			this.textService.Enabled = false;
			this.textService.Location = new System.Drawing.Point(119, 44);
			this.textService.Name = "textService";
			this.textService.ReadOnly = true;
			this.textService.Size = new System.Drawing.Size(282, 20);
			this.textService.TabIndex = 2;
			// 
			// textName
			// 
			this.textName.Enabled = false;
			this.textName.Location = new System.Drawing.Point(119, 20);
			this.textName.Name = "textName";
			this.textName.ReadOnly = true;
			this.textName.Size = new System.Drawing.Size(282, 20);
			this.textName.TabIndex = 1;
			// 
			// labelServComment
			// 
			this.labelServComment.Location = new System.Drawing.Point(6, 93);
			this.labelServComment.Name = "labelServComment";
			this.labelServComment.Size = new System.Drawing.Size(110, 17);
			this.labelServComment.TabIndex = 90;
			this.labelServComment.Text = "Service Comment";
			this.labelServComment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelVersion
			// 
			this.labelVersion.Location = new System.Drawing.Point(6, 69);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(110, 17);
			this.labelVersion.TabIndex = 89;
			this.labelVersion.Text = "Service Version";
			this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelService
			// 
			this.labelService.Location = new System.Drawing.Point(6, 45);
			this.labelService.Name = "labelService";
			this.labelService.Size = new System.Drawing.Size(110, 17);
			this.labelService.TabIndex = 88;
			this.labelService.Text = "Service Name";
			this.labelService.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelName
			// 
			this.labelName.Location = new System.Drawing.Point(6, 21);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(110, 17);
			this.labelName.TabIndex = 87;
			this.labelName.Text = "Server Name";
			this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormComputers
			// 
			this.AcceptButton = this.butClose;
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(535, 651);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.listComputer);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox3);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(551, 690);
			this.MinimumSize = new System.Drawing.Size(551, 690);
			this.Name = "FormComputers";
			this.ShowInTaskbar = false;
			this.Text = "Computers";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormComputers_Closing);
			this.Load += new System.EventHandler(this.FormComputers_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormComputers_Load(object sender, System.EventArgs e) {
			FillList();
			if(!Security.IsAuthorized(Permissions.GraphicsEdit,true)) {
				butSetSimpleGraphics.Enabled=false;
			}
		}

		private void FillList(){
			Computers.RefreshCache();
			_listComputers=Computers.GetDeepCopy();
			listComputer.Items.Clear();
			//Database Server----------------------------------------------------------		
			List<string> serviceList=Computers.GetServiceInfo();
			textName.Text=MiscData.GetODServer();//server name
			textService.Text=(serviceList[0].ToString());//service name
			textVersion.Text=(serviceList[3].ToString());//service version
			textServComment.Text=(serviceList[1].ToString());//service comment
			//workstation--------------------------------------------------------------
			textCurComp.Text=Environment.MachineName.ToUpper();//current computer name
			string itemName="";
			for(int i=0;i<_listComputers.Count;i++){
				itemName=_listComputers[i].CompName;
					//+" ("+Computers.List[i].PrinterName+")";
				listComputer.Items.Add(itemName);
			}
		}

		private void listComputer_MouseDoubleClick(object sender,MouseEventArgs e) {
			if(!Security.IsAuthorized(Permissions.GraphicsEdit)) {
				return;
			}
			FormGraphics FormG=new FormGraphics();
			FormG.ComputerPrefCur=ComputerPrefs.GetForComputer(_listComputers[listComputer.SelectedIndex].CompName);
			FormG.ShowDialog();
		}

		///<summary>Set graphics for selected computer to simple.  Makes audit log entry.</summary>
		private void butSetSimpleGraphics_Click(object sender,EventArgs e) {
			if(listComputer.SelectedIndex==-1) {
				MsgBox.Show(this,"You must select a computer name first.");
				return;
			}
			ComputerPrefs.SetToSimpleGraphics(_listComputers[listComputer.SelectedIndex].CompName);
			MsgBox.Show(this,"Done.");
			SecurityLogs.MakeLogEntry(Permissions.GraphicsEdit,0,"Set the graphics for computer "+_listComputers[listComputer.SelectedIndex].CompName+" to simple");
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(listComputer.SelectedIndex==-1) {
				return;
			}
			Computers.Delete(_listComputers[listComputer.SelectedIndex]);
			changed=true;
			FillList();
		}

		/*private void listProgram_DoubleClick(object sender, System.EventArgs e) {
			if(listProgram.SelectedIndex==-1)
				return;
			Programs.Cur=Programs.List[listProgram.SelectedIndex];
			FormProgramLinkEdit FormPE=new FormProgramLinkEdit();
			FormPE.ShowDialog();
			FillList();
		}*/

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormComputers_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(changed){
				DataValid.SetInvalid(InvalidType.Computers);
			}
		}


	}
}
