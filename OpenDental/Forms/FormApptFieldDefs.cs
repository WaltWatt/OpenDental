using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental{
	/// <summary>
	/// </summary>
	public class FormApptFieldDefs:ODForm {
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.ListBox listMain;
		private OpenDental.UI.Button butAdd;
		private System.ComponentModel.IContainer components;
		private Label label1;
		private MainMenu mainMenu1;
		private MenuItem menuItemSetup;
		private System.Windows.Forms.ToolTip toolTip1;
		private List<ApptFieldDef> _listApptFieldDefs;

		///<summary></summary>
		public FormApptFieldDefs()
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApptFieldDefs));
			this.listMain = new System.Windows.Forms.ListBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.butClose = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.menuItemSetup = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// listMain
			// 
			this.listMain.Location = new System.Drawing.Point(18, 77);
			this.listMain.Name = "listMain";
			this.listMain.Size = new System.Drawing.Size(265, 173);
			this.listMain.TabIndex = 2;
			this.listMain.DoubleClick += new System.EventHandler(this.listMain_DoubleClick);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(15, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(373, 65);
			this.label1.TabIndex = 8;
			this.label1.Text = "This is only for advanced users.  This is a list of extra fields that you can set" +
    " up for appointments.  After adding fields to this list, you can set the values " +
    "in an appointment edit window.";
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(349, 271);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(79, 24);
			this.butClose.TabIndex = 1;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(18, 271);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(79, 24);
			this.butAdd.TabIndex = 7;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSetup});
			// 
			// menuItemSetup
			// 
			this.menuItemSetup.Index = 0;
			this.menuItemSetup.Text = "Setup";
			this.menuItemSetup.Click += new System.EventHandler(this.menuItemSetup_Click);
			// 
			// FormApptFieldDefs
			// 
			this.ClientSize = new System.Drawing.Size(447, 309);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listMain);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.butAdd);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Menu = this.mainMenu1;
			this.MinimizeBox = false;
			this.Name = "FormApptFieldDefs";
			this.ShowInTaskbar = false;
			this.Text = "Appointment Field Defs";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormApptFieldDefs_FormClosing_1);
			this.Load += new System.EventHandler(this.FormApptFieldDefs_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormApptFieldDefs_Load(object sender, System.EventArgs e) {
			FillGrid();
		}

		private void FillGrid(){
			ApptFieldDefs.RefreshCache();
			_listApptFieldDefs=ApptFieldDefs.GetDeepCopy();
			listMain.Items.Clear();
			for(int i=0;i<_listApptFieldDefs.Count;i++) {
				listMain.Items.Add(_listApptFieldDefs[i].FieldName);
			}
		}

		private void menuItemSetup_Click(object sender,EventArgs e) {
			FormFieldDefLink FormFDL=new FormFieldDefLink(FieldLocations.AppointmentEdit);
			FormFDL.ShowDialog();
		}

		private void listMain_DoubleClick(object sender, System.EventArgs e) {
			if(listMain.SelectedIndex==-1){
				return;
			}
			FormApptFieldDefEdit FormP=new FormApptFieldDefEdit(_listApptFieldDefs[listMain.SelectedIndex]);
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK)
				return;
			FillGrid();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			ApptFieldDef def=new ApptFieldDef();
			FormApptFieldDefEdit FormP=new FormApptFieldDefEdit(def);
			FormP.IsNew=true;
			FormP.ShowDialog();
			FillGrid();
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormApptFieldDefs_FormClosing_1(object sender,FormClosingEventArgs e) {
			DataValid.SetInvalid(InvalidType.PatFields);
		}
	}
}



























