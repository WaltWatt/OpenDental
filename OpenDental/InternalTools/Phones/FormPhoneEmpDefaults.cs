using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormPhoneEmpDefaults:ODForm {
		private OpenDental.UI.ODGrid gridMain;
		private IContainer components=null;
		private OpenDental.UI.Button butClose;
		private UI.Button butAdd;
		private UI.Button butPhoneComps;
		private List<PhoneEmpDefault> ListPED;

		///<summary></summary>
		public FormPhoneEmpDefaults()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPhoneEmpDefaults));
			this.butClose = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butPhoneComps = new OpenDental.UI.Button();
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
			this.butClose.Location = new System.Drawing.Point(879, 546);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 11;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Location = new System.Drawing.Point(446, 546);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 24);
			this.butAdd.TabIndex = 12;
			this.butAdd.Text = "Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// gridMain
			// 
			this.gridMain.AllowSortingByColumn = true;
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(8, 14);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(946, 524);
			this.gridMain.TabIndex = 1;
			this.gridMain.Title = "Phone Settings";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butPhoneComps
			// 
			this.butPhoneComps.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPhoneComps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPhoneComps.Autosize = true;
			this.butPhoneComps.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPhoneComps.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPhoneComps.CornerRadius = 4F;
			this.butPhoneComps.Location = new System.Drawing.Point(8, 546);
			this.butPhoneComps.Name = "butPhoneComps";
			this.butPhoneComps.Size = new System.Drawing.Size(101, 24);
			this.butPhoneComps.TabIndex = 13;
			this.butPhoneComps.Text = "Phone Comps";
			this.butPhoneComps.Visible = false;
			this.butPhoneComps.Click += new System.EventHandler(this.butPhoneComps_Click);
			// 
			// FormPhoneEmpDefaults
			// 
			this.ClientSize = new System.Drawing.Size(966, 582);
			this.Controls.Add(this.butPhoneComps);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.gridMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormPhoneEmpDefaults";
			this.Text = "Phone Settings";
			this.Load += new System.EventHandler(this.FormAccountPick_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormAccountPick_Load(object sender,EventArgs e) {
			if(Security.IsAuthorized(Permissions.Setup,true)) {
				butPhoneComps.Visible=true;
			}
			FillGrid();
			gridMain.SortForced(1,true);
		}

		private void FillGrid(){
			int sortedColumnIdx=gridMain.SortedByColumnIdx;
      bool isSortAsc=gridMain.SortedIsAscending;
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("EmployeeNum",80,GridSortingStrategy.AmountParse);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("EmpName",90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("IsGraphed",65,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("HasColor",60,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Queue",65);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("PhoneExt",55,GridSortingStrategy.AmountParse);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("StatusOverride",90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Notes",250);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Private",50,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Triage",50,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			ListPED=PhoneEmpDefaults.Refresh();
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<ListPED.Count;i++){
				row=new ODGridRow();
				row.Cells.Add(ListPED[i].EmployeeNum.ToString());
				row.Cells.Add(ListPED[i].EmpName);
				row.Cells.Add(ListPED[i].IsGraphed?"X":"");
				row.Cells.Add(ListPED[i].HasColor?"X":"");
				row.Cells.Add(ListPED[i].RingGroups.ToString());
				row.Cells.Add(ListPED[i].PhoneExt.ToString());
				if(ListPED[i].StatusOverride==PhoneEmpStatusOverride.None) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(ListPED[i].StatusOverride.ToString());
				}
				row.Cells.Add(ListPED[i].Notes);
				row.Cells.Add(ListPED[i].IsPrivateScreen?"X":"");
				row.Cells.Add(ListPED[i].IsTriageOperator?"X":"");
				row.Tag=ListPED[i];
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			gridMain.SortForced(sortedColumnIdx,isSortAsc);
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormPhoneEmpDefaultEdit FormPED=new FormPhoneEmpDefaultEdit();
			FormPED.PedCur=(PhoneEmpDefault)gridMain.Rows[e.Row].Tag;
			FormPED.ShowDialog();
			FillGrid();
		}

		private void butPhoneComps_Click(object sender,EventArgs e) {
			FormPhoneComps FormPC=new FormPhoneComps();
			FormPC.ShowDialog();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormPhoneEmpDefaultEdit FormPED=new FormPhoneEmpDefaultEdit();
			FormPED.PedCur=new PhoneEmpDefault();
			FormPED.IsNew=true;
			FormPED.ShowDialog();
			FillGrid();
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		

		

		

	

		

		

	


	}
}





















