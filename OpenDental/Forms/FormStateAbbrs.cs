using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public class FormStateAbbrs:ODForm {
		private OpenDental.UI.Button butClose;
		private OpenDental.UI.Button butAdd;
		private System.ComponentModel.Container components = null;
		private bool _isChanged;
		public bool IsSelectionMode;
		private UI.ODGrid gridMain;
		private UI.Button butOK;
		public StateAbbr StateAbbrSelected;
		
		///<summary></summary>
		public FormStateAbbrs() {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStateAbbrs));
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
			this.butClose.Location = new System.Drawing.Point(245, 473);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 4;
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
			this.butAdd.Location = new System.Drawing.Point(16, 473);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(79, 26);
			this.butAdd.TabIndex = 2;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(16, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(304, 443);
			this.gridMain.TabIndex = 1;
			this.gridMain.Title = "States";
			this.gridMain.TranslationName = "FormStateAbbrs";
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
			this.butOK.Location = new System.Drawing.Point(159, 473);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Visible = false;
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// FormStateAbbrs
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(336, 515);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormStateAbbrs";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "State Abbreviations";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormStateAbbrs_Closing);
			this.Load += new System.EventHandler(this.FormStateAbbrs_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormStateAbbrs_Load(object sender, System.EventArgs e) {
			if(IsSelectionMode) {
				butAdd.Visible=false;
				butClose.Text="Cancel";
			}
			if(PrefC.GetBool(PrefName.EnforceMedicaidIDLength)) {
				this.Width+=100;
				gridMain.Width+=100;
			}
			FillGrid();
		}

		private void FillGrid(){
			long previousSelectedStateAbbrNum=-1;
			int newSelectedIdx=-1;
			if(gridMain.GetSelectedIndex()!=-1){
				previousSelectedStateAbbrNum=((StateAbbr)gridMain.Rows[gridMain.GetSelectedIndex()].Tag).StateAbbrNum;
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("FormStateAbbrs","Description"),175);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormStateAbbrs","Abbr"),70);
			gridMain.Columns.Add(col);
			if(PrefC.GetBool(PrefName.EnforceMedicaidIDLength)) {
				col=new ODGridColumn(Lan.g("FormStateAbbrs","Medicaid ID Length"),200);
				gridMain.Columns.Add(col);
			}
			gridMain.Rows.Clear();
			ODGridRow row;
			List<StateAbbr> stateAbbrs=StateAbbrs.GetDeepCopy();
			for(int i=0;i<stateAbbrs.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(stateAbbrs[i].Description);
				row.Cells.Add(stateAbbrs[i].Abbr);
				if(PrefC.GetBool(PrefName.EnforceMedicaidIDLength)) {
					if(stateAbbrs[i].MedicaidIDLength==0) {
						row.Cells.Add("");
					}
					else {
						row.Cells.Add(stateAbbrs[i].MedicaidIDLength.ToString());
					}
				}
				row.Tag=stateAbbrs[i];
				gridMain.Rows.Add(row);
				if(stateAbbrs[i].StateAbbrNum==previousSelectedStateAbbrNum) {
					newSelectedIdx=i;
				}
			}
			gridMain.EndUpdate();
			gridMain.SetSelected(newSelectedIdx,true);
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			StateAbbr stateAbbrCur=new StateAbbr();
			stateAbbrCur.IsNew=true;
			FormStateAbbrEdit FormSAE=new FormStateAbbrEdit(stateAbbrCur);
			FormSAE.ShowDialog();
			if(FormSAE.DialogResult!=DialogResult.OK) {
				return;
			}
			_isChanged=true;
			Cache.Refresh(InvalidType.StateAbbrs);
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				return;
			}
			if(IsSelectionMode) {
				StateAbbrSelected=(StateAbbr)gridMain.Rows[gridMain.GetSelectedIndex()].Tag;
				DialogResult=DialogResult.OK;
				return;
			}
			FormStateAbbrEdit FormSAE=new FormStateAbbrEdit((StateAbbr)gridMain.Rows[gridMain.GetSelectedIndex()].Tag);
			FormSAE.ShowDialog();
			if(FormSAE.DialogResult!=DialogResult.OK) {
				return;
			}
			_isChanged=true;
			Cache.Refresh(InvalidType.StateAbbrs);
			FillGrid();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!IsSelectionMode) {
				DialogResult=DialogResult.OK;
				return;
			}
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a state.");
				return;
			}
			StateAbbrSelected=(StateAbbr)gridMain.Rows[gridMain.GetSelectedIndex()].Tag;
			DialogResult=DialogResult.OK;
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormStateAbbrs_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(_isChanged){
				DataValid.SetInvalid(InvalidType.StateAbbrs);
			}
		}

	}
}
