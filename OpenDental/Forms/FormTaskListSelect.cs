using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormTaskListSelect : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.ListBox listMain;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private TaskObjectType OType;
		///<summary>If dialog result=ok, this will contain the TaskListNum that was selected.</summary>
		private List<TaskList> _taskListList;
		public string TaskListDescription;
		private Label labelMulti;
		private CheckBox checkMulti;
		public List<long> ListSelectedLists=new List<long>();

		///<summary></summary>
		public FormTaskListSelect(TaskObjectType oType,bool IsTaskNew=false)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			OType=oType;
			checkMulti.Visible=IsTaskNew;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTaskListSelect));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.listMain = new System.Windows.Forms.ListBox();
			this.labelMulti = new System.Windows.Forms.Label();
			this.checkMulti = new System.Windows.Forms.CheckBox();
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
			this.butCancel.Location = new System.Drawing.Point(207, 410);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 0;
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
			this.butOK.Location = new System.Drawing.Point(207, 380);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// listMain
			// 
			this.listMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listMain.Location = new System.Drawing.Point(12, 53);
			this.listMain.Name = "listMain";
			this.listMain.Size = new System.Drawing.Size(182, 381);
			this.listMain.TabIndex = 2;
			this.listMain.DoubleClick += new System.EventHandler(this.listMain_DoubleClick);
			// 
			// labelMulti
			// 
			this.labelMulti.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelMulti.Location = new System.Drawing.Point(12, 9);
			this.labelMulti.Name = "labelMulti";
			this.labelMulti.Size = new System.Drawing.Size(270, 18);
			this.labelMulti.TabIndex = 10;
			this.labelMulti.Text = "Pick task list to send to.";
			this.labelMulti.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkMulti
			// 
			this.checkMulti.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkMulti.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkMulti.Location = new System.Drawing.Point(12, 35);
			this.checkMulti.Name = "checkMulti";
			this.checkMulti.Size = new System.Drawing.Size(182, 18);
			this.checkMulti.TabIndex = 9;
			this.checkMulti.Text = "Send copies to multiple";
			this.checkMulti.UseVisualStyleBackColor = true;
			this.checkMulti.Visible = false;
			this.checkMulti.CheckedChanged += new System.EventHandler(this.checkMulti_CheckedChanged);
			// 
			// FormTaskListSelect
			// 
			this.ClientSize = new System.Drawing.Size(294, 446);
			this.Controls.Add(this.labelMulti);
			this.Controls.Add(this.checkMulti);
			this.Controls.Add(this.listMain);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(241, 219);
			this.Name = "FormTaskListSelect";
			this.ShowInTaskbar = false;
			this.Text = "Select Task List";
			this.Load += new System.EventHandler(this.FormTaskListSelect_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormTaskListSelect_Load(object sender, System.EventArgs e) {
			_taskListList=TaskLists.GetForObjectType(OType);
			if(OType==TaskObjectType.Appointment) {
				List<TaskList> TrunkList=TaskLists.RefreshMainTrunk(Security.CurUser.UserNum,TaskType.All)
					.FindAll(x => x.ObjectType!=TaskObjectType.Appointment);//TaskListList already contains all appt type tasklists
				List<long> listUserInboxNums=Userods.GetDeepCopy(true).Select(x => x.TaskListInBox).ToList();
				_taskListList.AddRange(TrunkList.FindAll(x => listUserInboxNums.Contains(x.TaskListNum)));
			}
			for(int i=0;i<_taskListList.Count;i++) {
				listMain.Items.Add(_taskListList[i].Descript);
			}
		}

		private void listMain_DoubleClick(object sender, System.EventArgs e) {
			if(listMain.SelectedIndex==-1){
				return;
			}
			for(int i=0;i<listMain.SelectedIndices.Count;i++) {
				ListSelectedLists.Add(_taskListList[listMain.SelectedIndices[i]].TaskListNum);
			}
			TaskListDescription=_taskListList[listMain.SelectedIndex].Descript;
			DialogResult=DialogResult.OK;
		}

		private void checkMulti_CheckedChanged(object sender,EventArgs e) {
			if(checkMulti.Checked) {
				listMain.SelectionMode=SelectionMode.MultiSimple;
				labelMulti.Text=Lan.g(this,"Pick task lists to send to.  Click on task lists to toggle.");
			}
			else {
				labelMulti.Text=Lan.g(this,"Pick task list to send to.");
				listMain.SelectionMode=SelectionMode.One;
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(listMain.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a task list first.");
				return;
			}
			for(int i=0;i<listMain.SelectedIndices.Count;i++) {
				ListSelectedLists.Add(_taskListList[listMain.SelectedIndices[i]].TaskListNum);
			}
			TaskListDescription=_taskListList[listMain.SelectedIndex].Descript;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		


	}
}





















