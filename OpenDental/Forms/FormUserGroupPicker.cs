using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental{
	public class FormUserGroupPicker:ODForm {
		private ListBox listGroups;
		private UI.Button butOK;
		private UI.Button butCancel;
		public UserGroup UserGroup;
		/// <summary>Set to true by default. If false usergroups with security admin permission will not show.</summary>
		public bool IsAdminMode=true;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private List<UserGroup> _listUserGroups;

		///<summary></summary>
		public FormUserGroupPicker()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUserGroupPicker));
			this.listGroups = new System.Windows.Forms.ListBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// listGroups
			// 
			this.listGroups.FormattingEnabled = true;
			this.listGroups.Location = new System.Drawing.Point(12, 12);
			this.listGroups.Name = "listGroups";
			this.listGroups.Size = new System.Drawing.Size(183, 355);
			this.listGroups.TabIndex = 1;
			this.listGroups.DoubleClick += new System.EventHandler(this.listGroups_DoubleClick);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(211, 313);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 5;
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
			this.butCancel.Location = new System.Drawing.Point(211, 343);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 4;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormUserGroupPicker
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(298, 380);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.listGroups);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormUserGroupPicker";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Select User Group";
			this.Load += new System.EventHandler(this.FormUserGroupPicker_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormUserGroupPicker_Load(object sender,EventArgs e) {
			FillList();
		}

		private void FillList(){
			UserGroups.RefreshCache();
			listGroups.Items.Clear();
			_listUserGroups=UserGroups.GetWhere(x => IsAdminMode || !GroupPermissions.HasPermission(x.UserGroupNum,Permissions.SecurityAdmin,0));
			for(int i=0;i<_listUserGroups.Count;i++) {
				listGroups.Items.Add(_listUserGroups[i].Description);
			}
		}

		private void listGroups_DoubleClick(object sender,EventArgs e) {
			if(listGroups.SelectedIndex==-1) {
				return;
			}
			UserGroup=_listUserGroups[listGroups.SelectedIndex];
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(listGroups.SelectedIndex==-1) {
				MsgBox.Show(this,"Select a group.");
				return;
			}
			UserGroup=_listUserGroups[listGroups.SelectedIndex];
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		

		


	}
}





















