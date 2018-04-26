using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Collections.Generic;

namespace OpenDental{
///<summary></summary>
	public class FormSchoolCourses : ODForm {
		private OpenDental.UI.Button butClose;
		private OpenDental.UI.Button butAdd;
		private System.ComponentModel.Container components = null;
		private bool changed;
		public bool IsSelectionMode;
		private UI.ODGrid gridMain;
		private UI.Button butOK;
		public SchoolCourse CourseSelected;
		private List<SchoolCourse> _listSchoolCourses;

		///<summary></summary>
		public FormSchoolCourses(){
			InitializeComponent();
			//Providers.Selected=-1;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSchoolCourses));
			this.butClose = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butOK = new OpenDental.UI.Button();
			this.SuspendLayout();
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
			this.butClose.Location = new System.Drawing.Point(362, 473);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 3;
			this.butClose.Text = "&Close";
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
			this.butAdd.Location = new System.Drawing.Point(16, 472);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(79, 26);
			this.butAdd.TabIndex = 10;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(425, 423);
			this.gridMain.TabIndex = 61;
			this.gridMain.Title = "Courses";
			this.gridMain.TranslationName = "FormEvaluationDefEdit";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butOK.Location = new System.Drawing.Point(362, 441);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 62;
			this.butOK.Text = "&OK";
			this.butOK.Visible = false;
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// FormSchoolCourses
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(453, 515);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormSchoolCourses";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Dental School Courses";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormSchoolCourses_Closing);
			this.Load += new System.EventHandler(this.FormSchoolCourses_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormSchoolCourses_Load(object sender, System.EventArgs e) {
			if(IsSelectionMode) {
				butAdd.Visible=false;
				butOK.Visible=true;
				butClose.Text="Cancel";
			}
			_listSchoolCourses=SchoolCourses.GetDeepCopy();
			FillGrid();
		}

		private void FillGrid(){
			long previousSelected=-1;
			if(gridMain.GetSelectedIndex()!=-1){
				previousSelected=_listSchoolCourses[gridMain.GetSelectedIndex()].SchoolCourseNum;
			}
			SchoolCourses.RefreshCache();
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("FormSchoolCourses","Course ID"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormEvaluationDefEdit","Description"),80);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listSchoolCourses.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listSchoolCourses[i].CourseID);
				row.Cells.Add(_listSchoolCourses[i].Descript);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			SchoolCourse cur=new SchoolCourse();
			FormSchoolCourseEdit FormS=new FormSchoolCourseEdit(cur);
			FormS.IsNew=true;
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK){
				return;
			}
			changed=true; 
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				return;
			}
			if(IsSelectionMode) {
				CourseSelected=_listSchoolCourses[gridMain.GetSelectedIndex()];
				DialogResult=DialogResult.OK;
				return;
			}
			FormSchoolCourseEdit FormS=new FormSchoolCourseEdit(_listSchoolCourses[gridMain.GetSelectedIndex()]);
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				return;
			}
			changed=true;
			FillGrid();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a course.");
				return;
			}
			if(IsSelectionMode) {
				CourseSelected=_listSchoolCourses[gridMain.GetSelectedIndex()];
				DialogResult=DialogResult.OK;
			}
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormSchoolCourses_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(changed){
				DataValid.SetInvalid(InvalidType.DentalSchools);
			}
		}

	}
}
