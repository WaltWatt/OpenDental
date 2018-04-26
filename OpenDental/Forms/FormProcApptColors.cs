using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormProcApptColors:ODForm {
		private OpenDental.UI.Button butAdd;
		private OpenDental.UI.Button butClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.ODGrid gridMain;
		private Label label1;
		private OpenDental.UI.Button butOK;
		private List<ProcApptColor> _listProcApptColors;

		///<summary></summary>
		public FormProcApptColors()	{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )	{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProcApptColors));
			this.butOK = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butAdd = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(146, 388);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 15;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(17, 77);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(285, 300);
			this.gridMain.TabIndex = 11;
			this.gridMain.Title = "Proc Code Ranges";
			this.gridMain.TranslationName = "FormProcApptColors";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
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
			this.butAdd.Location = new System.Drawing.Point(17, 388);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 24);
			this.butAdd.TabIndex = 10;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(227, 388);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Cancel";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(288, 65);
			this.label1.TabIndex = 16;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// FormProcApptColors
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(320, 428);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormProcApptColors";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Proc Appt Colors";
			this.Load += new System.EventHandler(this.FormProcApptColors_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormProcApptColors_Load(object sender,System.EventArgs e) {
			FillGrid();
		}

		private void FillGrid(){
			ProcApptColors.RefreshCache();
			_listProcApptColors=ProcApptColors.GetDeepCopy();
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("FormProcApptColors","Code Range"),0);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listProcApptColors.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listProcApptColors[i].CodeRange);
				row.ColorText=_listProcApptColors[i].ColorText;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			FormProcApptColorEdit FormPACE=new FormProcApptColorEdit();
			FormPACE.ProcApptColorCur=new ProcApptColor();
			FormPACE.ProcApptColorCur.IsNew=true;
			FormPACE.ShowDialog();
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
				FormProcApptColorEdit FormP=new FormProcApptColorEdit();
				FormP.ProcApptColorCur=_listProcApptColors[e.Row];
				FormP.ShowDialog();
				FillGrid();
		}

		private void butClose_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		

		

		



		
	}
}





















