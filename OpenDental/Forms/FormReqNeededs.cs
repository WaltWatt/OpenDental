using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.Crud;
using System.Linq;

namespace OpenDental{
///<summary></summary>
	public class FormReqNeededs:ODForm {
		private OpenDental.UI.Button butClose;
		private OpenDental.UI.Button butAdd;
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.ODGrid gridMain;
		private ComboBox comboClassFrom;
		private Label label1;
		private Label label2;
		private ComboBox comboCourseFrom;
		private GroupBox groupBox1;
		private List<ReqNeeded> _listReqsAll;
		///<summary>Stale deep copy of _listReqsAll to use with sync.</summary>
		private List<ReqNeeded> _listReqsAllOld;
		private List<ReqNeeded> _listReqsInGrid;
		private List<SchoolClass> _listSchoolClasses;
		private List<SchoolCourse> _listSchoolCourses;
		private Label label3;
		private ComboBox comboClassTo;
		private ComboBox comboCourseTo;
		private Label label4;
		private UI.Button butDeleteReq;
		private GroupBox groupBox3;
		private UI.Button butOk;
		private UI.Button butCopy;
		
		///<summary></summary>
		public FormReqNeededs(){
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReqNeededs));
      this.label1 = new System.Windows.Forms.Label();
      this.comboClassFrom = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.comboCourseFrom = new System.Windows.Forms.ComboBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.butCopy = new OpenDental.UI.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.comboClassTo = new System.Windows.Forms.ComboBox();
      this.comboCourseTo = new System.Windows.Forms.ComboBox();
      this.label4 = new System.Windows.Forms.Label();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.butAdd = new OpenDental.UI.Button();
      this.gridMain = new OpenDental.UI.ODGrid();
      this.butOk = new OpenDental.UI.Button();
      this.butDeleteReq = new OpenDental.UI.Button();
      this.butClose = new OpenDental.UI.Button();
      this.groupBox1.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(9, 28);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(61, 18);
      this.label1.TabIndex = 16;
      this.label1.Text = "Class";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // comboClassFrom
      // 
      this.comboClassFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboClassFrom.FormattingEnabled = true;
      this.comboClassFrom.Location = new System.Drawing.Point(73, 28);
      this.comboClassFrom.Name = "comboClassFrom";
      this.comboClassFrom.Size = new System.Drawing.Size(234, 21);
      this.comboClassFrom.TabIndex = 0;
      this.comboClassFrom.SelectionChangeCommitted += new System.EventHandler(this.comboClass_SelectionChangeCommitted);
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(9, 55);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(60, 18);
      this.label2.TabIndex = 18;
      this.label2.Text = "Course";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // comboCourseFrom
      // 
      this.comboCourseFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCourseFrom.FormattingEnabled = true;
      this.comboCourseFrom.Location = new System.Drawing.Point(73, 55);
      this.comboCourseFrom.Name = "comboCourseFrom";
      this.comboCourseFrom.Size = new System.Drawing.Size(234, 21);
      this.comboCourseFrom.TabIndex = 17;
      this.comboCourseFrom.SelectionChangeCommitted += new System.EventHandler(this.comboCourse_SelectionChangeCommitted);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.butCopy);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this.comboClassTo);
      this.groupBox1.Controls.Add(this.comboCourseTo);
      this.groupBox1.Controls.Add(this.label4);
      this.groupBox1.Location = new System.Drawing.Point(465, 133);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(316, 119);
      this.groupBox1.TabIndex = 19;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Copy Requirements To";
      // 
      // butCopy
      // 
      this.butCopy.AdjustImageLocation = new System.Drawing.Point(0, 0);
      this.butCopy.Autosize = true;
      this.butCopy.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
      this.butCopy.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
      this.butCopy.CornerRadius = 4F;
      this.butCopy.Location = new System.Drawing.Point(230, 86);
      this.butCopy.Name = "butCopy";
      this.butCopy.Size = new System.Drawing.Size(81, 23);
      this.butCopy.TabIndex = 24;
      this.butCopy.Text = "Copy";
      this.butCopy.UseVisualStyleBackColor = true;
      this.butCopy.Click += new System.EventHandler(this.butCopy_Click);
      // 
      // label3
      // 
      this.label3.Location = new System.Drawing.Point(6, 59);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(67, 18);
      this.label3.TabIndex = 23;
      this.label3.Text = "Course";
      this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // comboClassTo
      // 
      this.comboClassTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboClassTo.FormattingEnabled = true;
      this.comboClassTo.Location = new System.Drawing.Point(77, 32);
      this.comboClassTo.Name = "comboClassTo";
      this.comboClassTo.Size = new System.Drawing.Size(234, 21);
      this.comboClassTo.TabIndex = 20;
      // 
      // comboCourseTo
      // 
      this.comboCourseTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCourseTo.FormattingEnabled = true;
      this.comboCourseTo.Location = new System.Drawing.Point(77, 59);
      this.comboCourseTo.Name = "comboCourseTo";
      this.comboCourseTo.Size = new System.Drawing.Size(234, 21);
      this.comboCourseTo.TabIndex = 22;
      // 
      // label4
      // 
      this.label4.Location = new System.Drawing.Point(3, 32);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(71, 18);
      this.label4.TabIndex = 21;
      this.label4.Text = "Class";
      this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.comboClassFrom);
      this.groupBox3.Controls.Add(this.label1);
      this.groupBox3.Controls.Add(this.comboCourseFrom);
      this.groupBox3.Controls.Add(this.butAdd);
      this.groupBox3.Controls.Add(this.label2);
      this.groupBox3.Location = new System.Drawing.Point(465, 17);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(316, 114);
      this.groupBox3.TabIndex = 21;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Selected Class/Course";
      // 
      // butAdd
      // 
      this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
      this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.butAdd.Autosize = true;
      this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
      this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
      this.butAdd.CornerRadius = 4F;
      this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
      this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.butAdd.Location = new System.Drawing.Point(230, 82);
      this.butAdd.Name = "butAdd";
      this.butAdd.Size = new System.Drawing.Size(81, 26);
      this.butAdd.TabIndex = 10;
      this.butAdd.Text = "&Add";
      this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
      // 
      // gridMain
      // 
      this.gridMain.HasAddButton = false;
      this.gridMain.HasMultilineHeaders = false;
      this.gridMain.HScrollVisible = false;
      this.gridMain.Location = new System.Drawing.Point(16, 17);
      this.gridMain.Name = "gridMain";
      this.gridMain.ScrollValue = 0;
      this.gridMain.Size = new System.Drawing.Size(433, 556);
      this.gridMain.TabIndex = 13;
      this.gridMain.Title = "Requirements Needed";
      this.gridMain.TranslationName = "TableRequirementsNeeded";
      this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
      // 
      // butOk
      // 
      this.butOk.AdjustImageLocation = new System.Drawing.Point(0, 0);
      this.butOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.butOk.Autosize = true;
      this.butOk.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
      this.butOk.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
      this.butOk.CornerRadius = 4F;
      this.butOk.Location = new System.Drawing.Point(700, 547);
      this.butOk.Name = "butOk";
      this.butOk.Size = new System.Drawing.Size(82, 26);
      this.butOk.TabIndex = 22;
      this.butOk.Text = "&OK";
      this.butOk.Click += new System.EventHandler(this.butOk_Click);
      // 
      // butDeleteReq
      // 
      this.butDeleteReq.AdjustImageLocation = new System.Drawing.Point(0, 0);
      this.butDeleteReq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.butDeleteReq.Autosize = true;
      this.butDeleteReq.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
      this.butDeleteReq.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
      this.butDeleteReq.CornerRadius = 4F;
      this.butDeleteReq.Image = global::OpenDental.Properties.Resources.deleteX;
      this.butDeleteReq.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.butDeleteReq.Location = new System.Drawing.Point(16, 579);
      this.butDeleteReq.Name = "butDeleteReq";
      this.butDeleteReq.Size = new System.Drawing.Size(104, 26);
      this.butDeleteReq.TabIndex = 20;
      this.butDeleteReq.Text = "Delete All";
      this.butDeleteReq.Click += new System.EventHandler(this.butDeleteReq_Click);
      // 
      // butClose
      // 
      this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
      this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.butClose.Autosize = true;
      this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
      this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
      this.butClose.CornerRadius = 4F;
      this.butClose.Location = new System.Drawing.Point(699, 579);
      this.butClose.Name = "butClose";
      this.butClose.Size = new System.Drawing.Size(82, 26);
      this.butClose.TabIndex = 3;
      this.butClose.Text = "&Cancel";
      this.butClose.Click += new System.EventHandler(this.butClose_Click);
      // 
      // FormReqNeededs
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(793, 617);
      this.Controls.Add(this.butOk);
      this.Controls.Add(this.butDeleteReq);
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.gridMain);
      this.Controls.Add(this.butClose);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(809, 655);
      this.Name = "FormReqNeededs";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Requirements Needed";
      this.Load += new System.EventHandler(this.FormRequirementsNeeded_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.ResumeLayout(false);

		}
		#endregion

		private void FormRequirementsNeeded_Load(object sender, System.EventArgs e) {
			//comboClass.Items.Add(Lan.g(this,"All"));
			//comboClass.SelectedIndex=0;
			_listSchoolClasses=SchoolClasses.GetDeepCopy();
			_listSchoolCourses=SchoolCourses.GetDeepCopy();
			for(int i=0;i<_listSchoolClasses.Count;i++) {
				comboClassFrom.Items.Add(_listSchoolClasses[i].Descript);
				comboClassTo.Items.Add(_listSchoolClasses[i].Descript);
			}
			for(int i=0;i<_listSchoolCourses.Count;i++) {
				comboCourseFrom.Items.Add(_listSchoolCourses[i].Descript);
				comboCourseTo.Items.Add(_listSchoolCourses[i].Descript);
			}
			if(comboClassFrom.Items.Count>0) {
				comboClassFrom.SelectedIndex=0;
				comboClassTo.SelectedIndex=0;
			}
			if(comboCourseFrom.Items.Count>0) {
				comboCourseFrom.SelectedIndex=0;
				comboCourseTo.SelectedIndex=0;
			}
			ReloadReqList();
			FillGrid();
		}

		private void ReloadReqList() {
			_listReqsAll=ReqNeededs.GetListFromDb();
			_listReqsAllOld=_listReqsAll.Select(x => x.Copy()).ToList();
		}

		private bool RemoveReqFromAllList(ReqNeeded req) {
			for(int i=0;i<_listReqsAll.Count;i++) {
				if(_listReqsAll[i].ReqNeededNum==req.ReqNeededNum) {
					_listReqsAll.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		private void FillGrid() {
			if(comboClassFrom.SelectedIndex==-1 || comboCourseFrom.SelectedIndex==-1){
				return;
			}
			long selectedReqNum=0;
			if(gridMain.GetSelectedIndex()!=-1) {
				selectedReqNum=_listReqsInGrid[gridMain.GetSelectedIndex()].ReqNeededNum;
			}
			long schoolClass=_listSchoolClasses[comboClassFrom.SelectedIndex].SchoolClassNum;
			long schoolCourse=_listSchoolCourses[comboCourseFrom.SelectedIndex].SchoolCourseNum;
			int scroll=gridMain.ScrollValue;
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g("TableRequirementsNeeded","Description"),200);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			_listReqsInGrid=new List<ReqNeeded>();
			for(int i=0;i<_listReqsAll.Count;i++) {
				if(_listReqsAll[i].SchoolClassNum==schoolClass && _listReqsAll[i].SchoolCourseNum==schoolCourse) {
					_listReqsInGrid.Add(_listReqsAll[i].Copy());
				}
			}
			_listReqsInGrid=_listReqsInGrid.OrderBy(x => x.Descript).ToList();
			for(int i=0;i<_listReqsInGrid.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listReqsInGrid[i].Descript);
				gridMain.Rows.Add(row);
				if(_listReqsInGrid[i].ReqNeededNum==selectedReqNum) {
					gridMain.SetSelected(i,true);
					continue;
				}
			}
			gridMain.EndUpdate();
			gridMain.ScrollValue=scroll;
		}

		private void comboClass_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void comboCourse_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			if(comboClassFrom.SelectedIndex==-1 || comboCourseFrom.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a Class and Course first.");
				return;
			}
			FormReqNeededEdit FormR=new FormReqNeededEdit();
			FormR.ReqCur=new ReqNeeded();
			FormR.ReqCur.SchoolClassNum=_listSchoolClasses[comboClassFrom.SelectedIndex].SchoolClassNum;
			FormR.ReqCur.SchoolCourseNum=_listSchoolCourses[comboCourseFrom.SelectedIndex].SchoolCourseNum;
			FormR.IsNew=true;
			FormR.ShowDialog();
			if(FormR.DialogResult!=DialogResult.OK){
				return;
			}
			if(_listReqsInGrid.Any(x => x.Descript==FormR.ReqCur.Descript//Alternative to LINQ would be to create a method and loop through the whole list
					&& x.SchoolClassNum==FormR.ReqCur.SchoolClassNum 
					&& x.SchoolCourseNum==FormR.ReqCur.SchoolCourseNum)) {
				MsgBox.Show(this,"Requirement already exist.");
				return;
			}
			_listReqsAll.Add(FormR.ReqCur);
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormReqNeededEdit FormR=new FormReqNeededEdit();
			FormR.ReqCur=_listReqsInGrid[e.Row];//Previously got from the database but we want the copy from the list
			FormR.ShowDialog();
			if(FormR.DialogResult==DialogResult.OK) {
				if(FormR.ReqCur==null) {
					RemoveReqFromAllList(_listReqsInGrid[gridMain.GetSelectedIndex()]);
				}
				else {
					ReqNeeded reqNeeded=_listReqsAll.FirstOrDefault(x => x.ReqNeededNum==FormR.ReqCur.ReqNeededNum);
					if(reqNeeded != null) {//This should never be null.
						reqNeeded=FormR.ReqCur;
					}
				}
				FillGrid();
			}
		}

		private void butCopy_Click(object sender,EventArgs e) {
			if(comboClassTo.SelectedIndex==-1 ||comboCourseTo.SelectedIndex==-1) {
				MsgBox.Show(this,"Please select a Class and Course first.");
				return;
			}
			if(MsgBox.Show(this,MsgBoxButtons.OKCancel,"Are you sure you would like to copy over the requirements? Doing so will not replace any previous requirements.")) {
				long schoolClassFrom=_listSchoolClasses[comboClassFrom.SelectedIndex].SchoolClassNum;
				long schoolClassTo=_listSchoolClasses[comboClassTo.SelectedIndex].SchoolClassNum;
				long schoolCourseFrom=_listSchoolCourses[comboCourseFrom.SelectedIndex].SchoolCourseNum;
				long schoolCourseTo=_listSchoolCourses[comboCourseTo.SelectedIndex].SchoolCourseNum;
				if(schoolClassFrom==schoolClassTo && schoolCourseFrom==schoolCourseTo) {
						 return;
				}
				ReqNeeded reqCur;
				for(int i=0;i<_listReqsInGrid.Count;i++) {
					reqCur=new ReqNeeded();
					reqCur.Descript=_listReqsInGrid[i].Descript;
					reqCur.SchoolClassNum=schoolClassTo;
					reqCur.SchoolCourseNum=schoolCourseTo;
					if(_listReqsAll.Any(x => x.Descript==reqCur.Descript//Alternative to LINQ would be to create a method and loop through the whole list
							&& x.SchoolClassNum==reqCur.SchoolClassNum 
							&& x.SchoolCourseNum==reqCur.SchoolCourseNum)) 
          {
						continue;
					}
					_listReqsAll.Add(reqCur);
				}
				comboClassFrom.SelectedIndex=comboClassTo.SelectedIndex;
				comboCourseFrom.SelectedIndex=comboCourseTo.SelectedIndex;
				FillGrid();
			}
		}

		/*private void butSynch_Click(object sender,EventArgs e) {
			if(comboClass.SelectedIndex==-1 || comboCourse.SelectedIndex==-1) {
				MsgBox.Show(this,"Please select a Class and Course first.");
				return;
			}
			ReqNeededs.Synch(SchoolClasses.List[comboClass.SelectedIndex].SchoolClassNum,
				SchoolCourses.List[comboCourse.SelectedIndex].SchoolCourseNum);
			MsgBox.Show(this,"Done.");
		}*/

		private void butDeleteReq_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Are you sure you would like to delete all requirements needed?")) {
				return;
			}
			for(int i=0;i<_listReqsInGrid.Count;i++) {
				RemoveReqFromAllList(_listReqsInGrid[i]);
			}
			FillGrid();
		}

		private void butOk_Click(object sender,EventArgs e) {
			ReqNeededs.Sync(_listReqsAll,_listReqsAllOld);
			DialogResult=DialogResult.OK;
		}

		private void butClose_Click(object sender,System.EventArgs e) {
			Close();
		}

		



		

		

		

	

	}
}
