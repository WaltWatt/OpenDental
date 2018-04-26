using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.ReportingComplex;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Data;
using System.IO;
using OpenDental.UI;
using CodeBase;
using System.Linq;

namespace OpenDental{
	public class FormRpProcNote:ODForm {
		private OpenDental.UI.Button butClose;
		private CheckBox checkNoNotes;
		private CheckBox checkUnsignedNote;
		private Label labelClin;
		private Label label1;
		private GroupBox groupFilter;
		private UI.ODGrid gridMain;
		private UI.Button butRefresh;
		private UI.ODDateRangePicker dateRangePicker;
		private UI.ComboBoxMulti comboBoxMultiProv;
		private UI.Button butExport;
		private UI.Button butPrint;
		private bool _headingPrinted;
		private int _pagesPrinted;
		private int _headingPrintH;
		private UI.ComboBoxClinicMulti comboBoxMultiClinics;
		private GroupBox groupBox1;
		private RadioButton radioPatient;
		private RadioButton radioProc;
		private RadioButton radioProcDate;
		private ContextMenuStrip contextMenuStrip;
		private ToolStripMenuItem goToChartToolStripMenuItem;
		private IContainer components;

		#region Properties
		///<summary></summary>
		private List<long> _listSelectedProvNums {
			get {
				if(!comboBoxMultiProv.ListSelectedItems.Select(x => ((ODBoxItem<Provider>)x).Tag).Contains(null)) { //"All" is not selected.
					return comboBoxMultiProv.ListSelectedItems.Select(x => ((ODBoxItem<Provider>)x).Tag.ProvNum).ToList();
				}
				return comboBoxMultiProv.Items.ToArray().Select(x => ((ODBoxItem<Provider>)x).Tag)
					.Where(x => x!=null)
					.Select(x => x.ProvNum).ToList();
			}
		}

		#endregion

		///<summary></summary>
		public FormRpProcNote() {
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpProcNote));
			this.butClose = new OpenDental.UI.Button();
			this.checkNoNotes = new System.Windows.Forms.CheckBox();
			this.checkUnsignedNote = new System.Windows.Forms.CheckBox();
			this.labelClin = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupFilter = new System.Windows.Forms.GroupBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioProcDate = new System.Windows.Forms.RadioButton();
			this.radioPatient = new System.Windows.Forms.RadioButton();
			this.radioProc = new System.Windows.Forms.RadioButton();
			this.comboBoxMultiClinics = new OpenDental.UI.ComboBoxClinicMulti();
			this.comboBoxMultiProv = new OpenDental.UI.ComboBoxMulti();
			this.dateRangePicker = new OpenDental.UI.ODDateRangePicker();
			this.butRefresh = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.goToChartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.butExport = new OpenDental.UI.Button();
			this.butPrint = new OpenDental.UI.Button();
			this.groupFilter.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.contextMenuStrip.SuspendLayout();
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
			this.butClose.Location = new System.Drawing.Point(796, 524);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 4;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// checkNoNotes
			// 
			this.checkNoNotes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkNoNotes.Location = new System.Drawing.Point(6, 16);
			this.checkNoNotes.Name = "checkNoNotes";
			this.checkNoNotes.Size = new System.Drawing.Size(350, 21);
			this.checkNoNotes.TabIndex = 0;
			this.checkNoNotes.Text = "Include procedures with no notes on any procedure for the same day";
			this.checkNoNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkNoNotes.UseVisualStyleBackColor = true;
			// 
			// checkUnsignedNote
			// 
			this.checkUnsignedNote.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUnsignedNote.Location = new System.Drawing.Point(6, 38);
			this.checkUnsignedNote.Name = "checkUnsignedNote";
			this.checkUnsignedNote.Size = new System.Drawing.Size(350, 21);
			this.checkUnsignedNote.TabIndex = 1;
			this.checkUnsignedNote.Text = "Include procedures with a note that is unsigned";
			this.checkUnsignedNote.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUnsignedNote.UseVisualStyleBackColor = true;
			// 
			// labelClin
			// 
			this.labelClin.Location = new System.Drawing.Point(379, 16);
			this.labelClin.Name = "labelClin";
			this.labelClin.Size = new System.Drawing.Size(85, 21);
			this.labelClin.TabIndex = 51;
			this.labelClin.Text = "Clinics";
			this.labelClin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(379, 38);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(85, 21);
			this.label1.TabIndex = 49;
			this.label1.Text = "Providers";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupFilter
			// 
			this.groupFilter.Controls.Add(this.groupBox1);
			this.groupFilter.Controls.Add(this.comboBoxMultiClinics);
			this.groupFilter.Controls.Add(this.comboBoxMultiProv);
			this.groupFilter.Controls.Add(this.dateRangePicker);
			this.groupFilter.Controls.Add(this.butRefresh);
			this.groupFilter.Controls.Add(this.checkUnsignedNote);
			this.groupFilter.Controls.Add(this.checkNoNotes);
			this.groupFilter.Controls.Add(this.labelClin);
			this.groupFilter.Controls.Add(this.label1);
			this.groupFilter.Location = new System.Drawing.Point(12, 0);
			this.groupFilter.Name = "groupFilter";
			this.groupFilter.Size = new System.Drawing.Size(859, 97);
			this.groupFilter.TabIndex = 0;
			this.groupFilter.TabStop = false;
			this.groupFilter.Text = "Filters";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioProcDate);
			this.groupBox1.Controls.Add(this.radioPatient);
			this.groupBox1.Controls.Add(this.radioProc);
			this.groupBox1.Location = new System.Drawing.Point(638, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(125, 79);
			this.groupBox1.TabIndex = 52;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Group By";
			// 
			// radioProcDate
			// 
			this.radioProcDate.Location = new System.Drawing.Point(8, 53);
			this.radioProcDate.Name = "radioProcDate";
			this.radioProcDate.Size = new System.Drawing.Size(111, 18);
			this.radioProcDate.TabIndex = 2;
			this.radioProcDate.Text = "Date and Patient";
			this.radioProcDate.UseVisualStyleBackColor = true;
			this.radioProcDate.MouseCaptureChanged += new System.EventHandler(this.radioProcDate_MouseCaptureChanged);
			// 
			// radioPatient
			// 
			this.radioPatient.Location = new System.Drawing.Point(8, 35);
			this.radioPatient.Name = "radioPatient";
			this.radioPatient.Size = new System.Drawing.Size(104, 18);
			this.radioPatient.TabIndex = 1;
			this.radioPatient.Text = "Patient";
			this.radioPatient.UseVisualStyleBackColor = true;
			this.radioPatient.MouseCaptureChanged += new System.EventHandler(this.radioPatient_MouseCaptureChanged);
			// 
			// radioProc
			// 
			this.radioProc.Checked = true;
			this.radioProc.Location = new System.Drawing.Point(8, 17);
			this.radioProc.Name = "radioProc";
			this.radioProc.Size = new System.Drawing.Size(104, 18);
			this.radioProc.TabIndex = 0;
			this.radioProc.TabStop = true;
			this.radioProc.Text = "Procedure";
			this.radioProc.UseVisualStyleBackColor = true;
			this.radioProc.MouseCaptureChanged += new System.EventHandler(this.radioProc_MouseCaptureChanged);
			// 
			// comboBoxMultiClinics
			// 
			this.comboBoxMultiClinics.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiClinics.Location = new System.Drawing.Point(468, 17);
			this.comboBoxMultiClinics.Name = "comboBoxMultiClinics";
			this.comboBoxMultiClinics.Size = new System.Drawing.Size(160, 21);
			this.comboBoxMultiClinics.TabIndex = 3;
			// 
			// comboBoxMultiProv
			// 
			this.comboBoxMultiProv.ArraySelectedIndices = new int[0];
			this.comboBoxMultiProv.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiProv.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiProv.Items")));
			this.comboBoxMultiProv.Location = new System.Drawing.Point(468, 38);
			this.comboBoxMultiProv.Name = "comboBoxMultiProv";
			this.comboBoxMultiProv.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiProv.SelectedIndices")));
			this.comboBoxMultiProv.Size = new System.Drawing.Size(160, 21);
			this.comboBoxMultiProv.TabIndex = 4;
			// 
			// dateRangePicker
			// 
			this.dateRangePicker.BackColor = System.Drawing.SystemColors.Control;
			this.dateRangePicker.DefaultDateTimeFrom = new System.DateTime(2017, 7, 1, 0, 0, 0, 0);
			this.dateRangePicker.DefaultDateTimeTo = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
			this.dateRangePicker.EnableWeekButtons = false;
			this.dateRangePicker.Location = new System.Drawing.Point(124, 64);
			this.dateRangePicker.MaximumSize = new System.Drawing.Size(0, 185);
			this.dateRangePicker.MinimumSize = new System.Drawing.Size(300, 21);
			this.dateRangePicker.Name = "dateRangePicker";
			this.dateRangePicker.Size = new System.Drawing.Size(491, 21);
			this.dateRangePicker.TabIndex = 5;
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(769, 17);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(82, 24);
			this.butRefresh.TabIndex = 6;
			this.butRefresh.Text = "&Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.ContextMenuStrip = this.contextMenuStrip;
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 103);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(859, 415);
			this.gridMain.TabIndex = 1;
			this.gridMain.Title = "Incomplete Procedure Notes";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableIncompleteProcNotes";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToChartToolStripMenuItem});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new System.Drawing.Size(125, 26);
			// 
			// goToChartToolStripMenuItem
			// 
			this.goToChartToolStripMenuItem.Name = "goToChartToolStripMenuItem";
			this.goToChartToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.goToChartToolStripMenuItem.Text = "See Chart";
			this.goToChartToolStripMenuItem.Click += new System.EventHandler(this.goToChartToolStripMenuItem_Click);
			// 
			// butExport
			// 
			this.butExport.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butExport.Autosize = true;
			this.butExport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butExport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butExport.CornerRadius = 4F;
			this.butExport.Image = global::OpenDental.Properties.Resources.butExport;
			this.butExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butExport.Location = new System.Drawing.Point(97, 526);
			this.butExport.Name = "butExport";
			this.butExport.Size = new System.Drawing.Size(79, 24);
			this.butExport.TabIndex = 3;
			this.butExport.Text = "&Export";
			this.butExport.Click += new System.EventHandler(this.butExport_Click);
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
			this.butPrint.Location = new System.Drawing.Point(12, 526);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(79, 24);
			this.butPrint.TabIndex = 2;
			this.butPrint.Text = "&Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// FormRpProcNote
			// 
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(891, 572);
			this.Controls.Add(this.butExport);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.groupFilter);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(907, 517);
			this.Name = "FormRpProcNote";
			this.Text = "Incomplete Procedure Notes Report";
			this.Load += new System.EventHandler(this.FormRpProcNote_Load);
			this.groupFilter.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.contextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormRpProcNote_Load(object sender,System.EventArgs e) {
			checkNoNotes.Checked=PrefC.GetBool(PrefName.ReportsIncompleteProcsNoNotes);
			checkUnsignedNote.Checked=PrefC.GetBool(PrefName.ReportsIncompleteProcsUnsigned);
			FillProvs();
			if(!PrefC.HasClinicsEnabled) {
				comboBoxMultiClinics.Visible=false;
				labelClin.Visible=false;
			}
			dateRangePicker.SetDateTimeFrom(DateTime.Today);
			dateRangePicker.SetDateTimeTo(DateTime.Today);
			FillGrid();
		}

		private void FillProvs() {
			comboBoxMultiProv.Items.Add(new ODBoxItem<Provider>(Lan.g(this,"All"))); //tag = null
			foreach(Provider provCur in Providers.GetListReports()) {
				comboBoxMultiProv.Items.Add(new ODBoxItem<Provider>(provCur.GetLongDesc(),provCur));
			}
			comboBoxMultiProv.SetSelected(0,true);
		}

		private void FillGrid() {
			DataTable table=GetIncompleteProcNotes();
			bool includeNoNotes=checkNoNotes.Checked;
			bool includeUnsignedNotes=checkUnsignedNote.Checked;
			gridMain.BeginUpdate();
			//Columns
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"Date"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Patient Name"),120);
			gridMain.Columns.Add(col);
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				col=new ODGridColumn(Lan.g(this,"Code"),150);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Description"),220);
				gridMain.Columns.Add(col);
			}
			else {
				col=new ODGridColumn(Lan.g(this,"Code"),150);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Description"),220);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Tth"),30);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Surf"),40);
				gridMain.Columns.Add(col);
			}
			if(includeUnsignedNotes || includeNoNotes) {
				col=new ODGridColumn(Lan.g(this,"Incomplete"),80);
				gridMain.Columns.Add(col);
			}
			if(includeNoNotes) {
				col=new ODGridColumn(Lan.g(this,"No Note"),60);
				gridMain.Columns.Add(col);
			}
			if(includeUnsignedNotes) {
				col=new ODGridColumn(Lan.g(this,"Unsigned"),70);
				gridMain.Columns.Add(col);
			}
			//Rows
			gridMain.Rows.Clear();
			foreach(DataRow row in table.Rows) {
				ODGridRow newRow=new ODGridRow();
				newRow.Cells.Add(PIn.Date(row["ProcDate"].ToString()).ToString("d"));
				newRow.Cells.Add(row["PatName"].ToString());
				if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
					newRow.Cells.Add(row["ProcCode"].ToString());
					newRow.Cells.Add(row["Descript"].ToString());
				}
				else {
					newRow.Cells.Add(row["ProcCode"].ToString());
					newRow.Cells.Add(row["Descript"].ToString());
					newRow.Cells.Add(row["ToothNum"].ToString());
					newRow.Cells.Add(row["Surf"].ToString());
				}
				if(includeUnsignedNotes || includeNoNotes) {
					newRow.Cells.Add(row["Incomplete"].ToString());
				}
				if(includeNoNotes) {
					newRow.Cells.Add(row["HasNoNote"].ToString());
				}
				if(includeUnsignedNotes) {
					newRow.Cells.Add(row["HasUnsignedNote"].ToString());
				}
				newRow.Tag=row["PatNum"].ToString();
				gridMain.Rows.Add(newRow);
			}
			gridMain.EndUpdate();
		}

		private DataTable GetIncompleteProcNotes() {
			RpProcNote.ProcNoteGroupBy groupBy;
			if(radioPatient.Checked) {
				groupBy=RpProcNote.ProcNoteGroupBy.Patient;
			}
			else if(radioProcDate.Checked) {
				groupBy=RpProcNote.ProcNoteGroupBy.DateAndPatient;
			}
			else {
				groupBy=RpProcNote.ProcNoteGroupBy.Procedure;
			}
			return RpProcNote.GetData(_listSelectedProvNums,comboBoxMultiClinics.ListSelectedClinicNums,dateRangePicker.GetDateTimeFrom(),
				dateRangePicker.GetDateTimeTo(),checkNoNotes.Checked,checkUnsignedNote.Checked,
				(ToothNumberingNomenclature)PrefC.GetInt(PrefName.UseInternationalToothNumbers),groupBy);
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			if(gridMain.SelectedIndices.Length!=1) {
				return;
			}
			long goToPatNum=PIn.Long(gridMain.Rows[e.Row].Tag.ToString());
			Patient pat=Patients.GetPat(goToPatNum);
			FormOpenDental.S_Contr_PatientSelected(pat,false);
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(gridMain.SelectedIndices.Length!=1) {
				return;
			}
			if(!Security.IsAuthorized(Permissions.ChartModule)) {
				return;
			}
			long goToPatNum=PIn.Long(gridMain.Rows[e.Row].Tag.ToString());
			SendToBack();
			GotoModule.GotoChart(goToPatNum);
		}

		private void goToChartToolStripMenuItem_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length!=1) {
				MsgBox.Show(this,"Please select a patient first.");
				return;
			}
			if(!Security.IsAuthorized(Permissions.ChartModule)) {
				return;
			}
			long patNum=PIn.Long(gridMain.Rows[gridMain.SelectedIndices[0]].Tag.ToString());
			Patient pat=Patients.GetPat(patNum);
			FormOpenDental.S_Contr_PatientSelected(pat,false);
			SendToBack();
			GotoModule.GotoChart(pat.PatNum);
		}

		private void radioProc_MouseCaptureChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void radioPatient_MouseCaptureChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void radioProcDate_MouseCaptureChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			_pagesPrinted=0;
			PrintDocument pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			pd.DefaultPageSettings.Landscape=true;
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			_headingPrinted=false;
			try {
#if DEBUG
				FormRpPrintPreview pView=new FormRpPrintPreview();
				pView.printPreviewControl2.Document=pd;
				pView.ShowDialog();
#else
					if(PrinterL.SetPrinter(pd,PrintSituation.Default,0,"Incomplete Procedure Notes report printed")) {
						pd.Print();
					}
#endif
			}
			catch(Exception ex) {
				FriendlyException.Show(Lan.g(this,"Printer not available"),ex);
			}
		}  
		
		private void pd_PrintPage(object sender,PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!_headingPrinted) {
				text=Lan.g(this,"Incomplete Procedure Notes");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				text=PrefC.GetString(PrefName.PracticeTitle);
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				if(_listSelectedProvNums.Count==0 || _listSelectedProvNums.Count==comboBoxMultiProv.Items.Count-1) {
					text=Lan.g(this,"All Providers");
				}
				else {
					text=Lan.g(this,"For Providers:")+" "+string.Join(",",_listSelectedProvNums.Select(provNum => Providers.GetAbbr(provNum)));
				}
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				if(comboBoxMultiClinics.ListSelectedClinicNums.Count==0 || comboBoxMultiClinics.IsAllSelected) {//No clinics selected or 'All' selected
					text=Lan.g(this,"All Clinics");
				}
				else {
					text=Lan.g(this,"For Clinics:")+" "+string.Join(",",comboBoxMultiClinics.ListSelectedClinics.Select(clinic => clinic.Abbr));
				}
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
				_headingPrinted=true;
				_headingPrintH=yPos;
			}
			#endregion
			yPos=gridMain.PrintPage(g,_pagesPrinted,bounds,_headingPrintH);
			_pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}
		
		private void butExport_Click(object sender,System.EventArgs e) {
			gridMain.Export("Incomplete Procedure Notes");
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}
	}
}




















