using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Drawing.Printing;
using System.Linq;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormLabCases : ODForm {
		private OpenDental.UI.Button butClose;
		private System.ComponentModel.Container components = null;
		private ODGrid gridMain;
		private Label label1;
		private ValidDate textDateFrom;
		private ValidDate textDateTo;
		private Label label2;
		private OpenDental.UI.Button butRefresh;// Required designer variable.
		private DataTable table;
		private CheckBox checkShowAll;
		private ContextMenu contextMenu1;
		private MenuItem menuItemGoTo;
		private CheckBox checkShowUnattached;
		private UI.Button butPrint;
		//<summary>Set this to the selected date on the schedule, and date range will start out based on this date.</summary>
		//public DateTime DateViewing;
		///<summary>If this is zero, then it's an ordinary close.</summary>
		public long GoToAptNum;
		public bool headingPrinted;
		public int headingPrintH;
		private Label labelClinic;
		private ComboBox comboClinic;
		public int pagesPrinted;
		private List<Clinic> _listClinics;

		///<summary></summary>
		public FormLabCases()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLabCases));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.checkShowAll = new System.Windows.Forms.CheckBox();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItemGoTo = new System.Windows.Forms.MenuItem();
			this.checkShowUnattached = new System.Windows.Forms.CheckBox();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butRefresh = new OpenDental.UI.Button();
			this.textDateTo = new OpenDental.ValidDate();
			this.textDateFrom = new OpenDental.ValidDate();
			this.butClose = new OpenDental.UI.Button();
			this.butPrint = new OpenDental.UI.Button();
			this.labelClinic = new System.Windows.Forms.Label();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 18);
			this.label1.TabIndex = 2;
			this.label1.Text = "From Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 18);
			this.label2.TabIndex = 4;
			this.label2.Text = "To Date";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkShowAll
			// 
			this.checkShowAll.Location = new System.Drawing.Point(361, 37);
			this.checkShowAll.Name = "checkShowAll";
			this.checkShowAll.Size = new System.Drawing.Size(131, 18);
			this.checkShowAll.TabIndex = 7;
			this.checkShowAll.Text = "Show Completed";
			this.checkShowAll.UseVisualStyleBackColor = true;
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemGoTo});
			// 
			// menuItemGoTo
			// 
			this.menuItemGoTo.Index = 0;
			this.menuItemGoTo.Text = "Go To Appointment";
			this.menuItemGoTo.Click += new System.EventHandler(this.menuItemGoTo_Click);
			// 
			// checkShowUnattached
			// 
			this.checkShowUnattached.Location = new System.Drawing.Point(361, 14);
			this.checkShowUnattached.Name = "checkShowUnattached";
			this.checkShowUnattached.Size = new System.Drawing.Size(131, 18);
			this.checkShowUnattached.TabIndex = 8;
			this.checkShowUnattached.Text = "Show Unattached";
			this.checkShowUnattached.UseVisualStyleBackColor = true;
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(17, 67);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(916, 420);
			this.gridMain.TabIndex = 1;
			this.gridMain.Title = "Lab Cases";
			this.gridMain.TranslationName = "TableLabCases";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(226, 32);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 24);
			this.butRefresh.TabIndex = 6;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(116, 35);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(100, 20);
			this.textDateTo.TabIndex = 5;
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(116, 9);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(100, 20);
			this.textDateFrom.TabIndex = 3;
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(858, 497);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(413, 497);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(79, 24);
			this.butPrint.TabIndex = 53;
			this.butPrint.Text = "Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(638, 41);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(52, 14);
			this.labelClinic.TabIndex = 55;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelClinic.Visible = false;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(692, 36);
			this.comboClinic.MaxDropDownItems = 40;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(241, 21);
			this.comboClinic.TabIndex = 54;
			this.comboClinic.Visible = false;
			this.comboClinic.SelectedIndexChanged += new System.EventHandler(this.comboClinic_SelectedIndexChanged);
			// 
			// FormLabCases
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(945, 533);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.checkShowUnattached);
			this.Controls.Add(this.checkShowAll);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.textDateTo);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textDateFrom);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormLabCases";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Lab Cases";
			this.Load += new System.EventHandler(this.FormLabCases_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormLabCases_Load(object sender,EventArgs e) {
			//ListClinics can be called even when Clinics is not turned on, therefore it needs to be set to something to avoid a null reference.
			_listClinics=new List<Clinic>();
			if(PrefC.HasClinicsEnabled) {
				labelClinic.Visible=true;
				comboClinic.Visible=true;
				comboClinic.Items.Add(Lan.g(this,"All"));//"All", or "All" of the clinics the user can access if user is clinic restricted.
				comboClinic.SelectedIndex=0;
				_listClinics=Clinics.GetForUserod(Security.CurUser);
				for(int i = 0;i<_listClinics.Count;i++) {
					comboClinic.Items.Add(_listClinics[i].Abbr);
					if(_listClinics[i].ClinicNum==Clinics.ClinicNum) {
						comboClinic.SelectedIndex=i+1;
					}
				}
			}
			gridMain.ContextMenu=contextMenu1;
			textDateFrom.Text="";//DateViewing.ToShortDateString();
			textDateTo.Text="";//DateViewing.AddDays(5).ToShortDateString();
			//checkShowAll.Checked=false
			FillGrid();
		}

		private void FillGrid() {
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateFrom.errorProvider1.GetError(textDateFrom)!="") {
				//MsgBox.Show(this,"Please fix errors first.");
				return;
			}
			DateTime dateMax = new DateTime(2100,1,1);
			if(textDateTo.Text!="") {
				dateMax=PIn.Date(textDateTo.Text);
			}
			table=LabCases.Refresh(PIn.Date(textDateFrom.Text),dateMax,checkShowAll.Checked,checkShowUnattached.Checked);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g("TableLabCases","Appt Date Time"),120);
			col.SortingStrategy=GridSortingStrategy.DateParse;
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableLabCases","Procedures"),200);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableLabCases","Patient"),120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableLabCases","Status"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableLabCases","Lab"),75);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableLabCases","Lab Phone"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableLabCases","Instructions"),100);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			List<long> operatoryNums = new List<long>();
			if(PrefC.HasClinicsEnabled) {
				if(comboClinic.SelectedIndex==0 && !Security.CurUser.ClinicIsRestricted) {//"All"
					operatoryNums=null;
				}
				else if(comboClinic.SelectedIndex==0) {//"All" that the user has access to.
					foreach(Clinic clinic in _listClinics) {
						operatoryNums.AddRange(Operatories.GetOpsForClinic(clinic.ClinicNum).Select(x => x.OperatoryNum));
					}
				}
				else {
					operatoryNums.AddRange(Operatories.GetOpsForClinic(_listClinics[comboClinic.SelectedIndex-1].ClinicNum).Select(x => x.OperatoryNum));
				}
			}
			for(int i=0;i<table.Rows.Count;i++){
				if(PrefC.HasClinicsEnabled //no filtering for non clinics.
					&& operatoryNums!=null //we don't have "All" selected for an unrestricted user.
					&& table.Rows[i]["AptNum"].ToString()!="0" //show unattached for any clinic 
					&& !operatoryNums.Contains(PIn.Long(table.Rows[i]["OpNum"].ToString()))) //Attached appointment is scheduled in an Op for the clinic
				{
					continue;//appointment scheduled in an operatory for another clinic.
				}
				row=new ODGridRow();
				row.Cells.Add(table.Rows[i]["aptDateTime"].ToString());
				row.Cells.Add(table.Rows[i]["ProcDescript"].ToString());
				row.Cells.Add(table.Rows[i]["patient"].ToString());
				row.Cells.Add(table.Rows[i]["status"].ToString());
				row.Cells.Add(table.Rows[i]["lab"].ToString());
				row.Cells.Add(table.Rows[i]["phone"].ToString());
				row.Cells.Add(table.Rows[i]["Instructions"].ToString());
				row.Tag=table.Rows[i];
				gridMain.Rows.Add(row);
			}
			gridMain.AllowSortingByColumn=true;
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			DataRow row=(DataRow)gridMain.Rows[e.Row].Tag;
			long selectedLabCase=PIn.Long(row["LabCaseNum"].ToString());
			FormLabCaseEdit FormL=new FormLabCaseEdit();
			FormL.CaseCur=LabCases.GetOne(selectedLabCase);
			FormL.ShowDialog();
			if(FormL.DialogResult!=DialogResult.OK) {
				return;//don't refresh unless we have to.  It messes up the user's ordering.
			}
			FillGrid();
			for(int i=0;i<table.Rows.Count;i++){
				if(table.Rows[i]["LabCaseNum"].ToString()==selectedLabCase.ToString()){
					gridMain.SetSelected(i,true);
					break;
				}
			}
		}

		private void comboClinic_SelectedIndexChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateFrom.errorProvider1.GetError(textDateFrom)!="")
			{
				MsgBox.Show(this,"Please fix errors first.");
				return;
			}
			FillGrid();
		}

		private void menuItemGoTo_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select a lab case first.");
				return;
			}
			DataRow row=gridMain.SelectedTag<DataRow>();
			if(row["AptNum"].ToString()=="0") {
				MsgBox.Show(this,"There are no appointments for unattached lab cases.");
				return;
			}
			Appointment apt=Appointments.GetOneApt(PIn.Long(row["AptNum"].ToString()));
			if(apt.AptStatus==ApptStatus.UnschedList){
				MsgBox.Show(this,"Cannot go to an unscheduled appointment");
				return;
			}
			GoToAptNum=apt.AptNum;
			Close();
		}

		private void pd_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			//new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!headingPrinted) {
				text=Lan.g(this,"Lab Cases");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				headingPrinted=true;
				headingPrintH=yPos;
			}
			#endregion
			yPos=gridMain.PrintPage(g,pagesPrinted,bounds,headingPrintH);
			pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			if(gridMain.Rows.Count<1) {
				MsgBox.Show(this,"Nothing to print.");
				return;
			}
			pagesPrinted=0;
			PrintDocument pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			//pd.OriginAtMargins=true;
			pd.DefaultPageSettings.Landscape=true;
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			headingPrinted=false;
			try {
#if DEBUG
				FormRpPrintPreview pView = new FormRpPrintPreview();
				pView.printPreviewControl2.Document=pd;
				pView.ShowDialog();
#else
					if(PrinterL.SetPrinter(pd,PrintSituation.Default,0,"Lab case list printed")) {
						pd.Print();
					}
#endif
			}
			catch {
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
		}



		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}
	}
}





















