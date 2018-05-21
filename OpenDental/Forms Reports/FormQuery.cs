//using Excel;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using System.Threading;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
	public class FormQuery : ODForm {
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.DataGrid grid2;
		private System.Windows.Forms.GroupBox groupBox1;
		private OpenDental.UI.Button butSubmit;
		private System.Windows.Forms.RadioButton radioRaw;
		///<summary></summary>
		public System.Windows.Forms.RadioButton radioHuman;
		private OpenDental.UI.Button butFavorite;
		private System.ComponentModel.Container components = null;// Required designer variable.
		private OpenDental.UI.Button butAdd;
		private DataGridTableStyle myGridTS;
		private System.Windows.Forms.PrintPreviewDialog printPreviewDialog2;
		private System.Drawing.Printing.PrintDocument pd2;
		private bool _totalsPrinted;
		private bool _summaryPrinted;
		private int _linesPrinted;
		private int _pagesPrinted;
		///<summary>Set to true if this form should instantly launch a print preview instead of showing the actual "user query" form.</summary>
		public bool IsReport;
		private bool _headerPrinted;
		private System.Windows.Forms.PrintPreviewControl printPreviewControl2;
		private bool _tablePrinted;
		private System.Drawing.Font titleFont = new System.Drawing.Font("Arial",14,FontStyle.Bold);
		private System.Drawing.Font subtitleFont=new System.Drawing.Font("Arial",8,FontStyle.Bold);
		private System.Drawing.Font colCaptFont=new System.Drawing.Font("Arial",8,FontStyle.Bold);
		private System.Drawing.Font bodyFont = new System.Drawing.Font("Arial", 8);
		private OpenDental.UI.Button butFullPage;
		private System.Windows.Forms.Panel panelZoom;
		private System.Windows.Forms.Label labelTotPages;
		private System.Windows.Forms.Label label1;
		///<summary></summary>
		public System.Windows.Forms.TextBox textTitle;
		private System.Windows.Forms.SaveFileDialog saveFileDialog2;
		private OpenDental.UI.Button butCopy;
		private OpenDental.UI.Button butPaste;
		private OpenDental.UI.Button butZoomIn;
		private OpenDental.UI.Button butPrint;
		private OpenDental.UI.Button butExport;
		private OpenDental.UI.Button butQView;
		private OpenDental.UI.Button butPrintPreview;
		private OpenDental.UI.Button butBack;
		private OpenDental.UI.Button butFwd;
		private OpenDental.UI.Button butExportExcel;
		///<summary></summary>
		public OpenDental.ODtextBox textQuery;
		private int _totalPages=0;
		///<summary>A hashtable of key: InsPlanNum, value: Carrier Name.</summary>
		private static Hashtable _hashListPlans;
		private UserQuery _userQueryCur;
		///<summary>A dictionary of patient names that only gets filled once and only if necessary.</summary>
		private static Dictionary<long,string> _dictPatientNames;
		private SplitContainer splitContainerQuery;
		private Panel panel1;
		///<summary></summary>
		private ReportSimpleGrid _reportSimpleGrid=new ReportSimpleGrid();
		///<summary>User queries run on a separate thread so they can be cancelled.</summary>
		private ODThread _threadQuery;
		///<summary>Tells the form whether to throw, show (interrupt), or suppress exceptions. Set manually in the code below.</summary>
		private QueryExceptionState _queryExceptionStateCur;
		///<summary>The server thread id that the current query is running on.</summary>
		private int _serverThreadID;
		///<summary>_reportSumpleGrid.TableQ should ALWAYS store the raw table, and _tableHuman will ALWAYS store the human-readable table.
		///This table only gets filled if radioHuman is clicked or checked when run. This way, when switching between the raw and human-readable formats, 
		///we merely have to set the data binding for the grid to their respective data tables instead of rerunning the entire query again.</summary>
		private DataTable _tableHuman;
		private CheckBox checkReportServer;
		private bool _submitOnLoad;

		///<summary>Can pass in null if not a report.</summary>
		public FormQuery(ReportSimpleGrid report, bool submitQueryOnLoad = false){
			InitializeComponent();// Required for Windows Form Designer support
			Lan.F(this,new System.Windows.Forms.Control[] {
				//exclude:
				labelTotPages,
			});
			if(report!=null) {
				this._reportSimpleGrid=report;
			}
			_submitOnLoad=submitQueryOnLoad;
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQuery));
			this.butClose = new OpenDental.UI.Button();
			this.grid2 = new System.Windows.Forms.DataGrid();
			this.textQuery = new OpenDental.ODtextBox();
			this.butExportExcel = new OpenDental.UI.Button();
			this.butPaste = new OpenDental.UI.Button();
			this.butCopy = new OpenDental.UI.Button();
			this.textTitle = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.butAdd = new OpenDental.UI.Button();
			this.butFavorite = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioHuman = new System.Windows.Forms.RadioButton();
			this.radioRaw = new System.Windows.Forms.RadioButton();
			this.butSubmit = new OpenDental.UI.Button();
			this.pd2 = new System.Drawing.Printing.PrintDocument();
			this.printPreviewDialog2 = new System.Windows.Forms.PrintPreviewDialog();
			this.printPreviewControl2 = new System.Windows.Forms.PrintPreviewControl();
			this.butFullPage = new OpenDental.UI.Button();
			this.panelZoom = new System.Windows.Forms.Panel();
			this.labelTotPages = new System.Windows.Forms.Label();
			this.butZoomIn = new OpenDental.UI.Button();
			this.butBack = new OpenDental.UI.Button();
			this.butFwd = new OpenDental.UI.Button();
			this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
			this.butPrint = new OpenDental.UI.Button();
			this.butExport = new OpenDental.UI.Button();
			this.butQView = new OpenDental.UI.Button();
			this.butPrintPreview = new OpenDental.UI.Button();
			this.splitContainerQuery = new System.Windows.Forms.SplitContainer();
			this.panel1 = new System.Windows.Forms.Panel();
			this.checkReportServer = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.grid2)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.panelZoom.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerQuery)).BeginInit();
			this.splitContainerQuery.Panel1.SuspendLayout();
			this.splitContainerQuery.Panel2.SuspendLayout();
			this.splitContainerQuery.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(879, 753);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 27);
			this.butClose.TabIndex = 5;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// grid2
			// 
			this.grid2.DataMember = "";
			this.grid2.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.grid2.Location = new System.Drawing.Point(3, 25);
			this.grid2.Name = "grid2";
			this.grid2.ReadOnly = true;
			this.grid2.Size = new System.Drawing.Size(957, 599);
			this.grid2.TabIndex = 2;
			// 
			// textQuery
			// 
			this.textQuery.AcceptsTab = true;
			this.textQuery.BackColor = System.Drawing.SystemColors.Window;
			this.textQuery.DetectLinksEnabled = false;
			this.textQuery.DetectUrls = false;
			this.textQuery.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textQuery.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textQuery.Location = new System.Drawing.Point(0, 0);
			this.textQuery.Margin = new System.Windows.Forms.Padding(3, 3, 3, 25);
			this.textQuery.MaximumSize = new System.Drawing.Size(557, 900);
			this.textQuery.Name = "textQuery";
			this.textQuery.QuickPasteType = OpenDentBusiness.QuickPasteType.Query;
			this.textQuery.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textQuery.Size = new System.Drawing.Size(557, 138);
			this.textQuery.SpellCheckIsEnabled = false;
			this.textQuery.TabIndex = 16;
			this.textQuery.Text = "";
			this.textQuery.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textQuery_KeyDown);
			// 
			// butExportExcel
			// 
			this.butExportExcel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butExportExcel.Autosize = true;
			this.butExportExcel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butExportExcel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butExportExcel.CornerRadius = 4F;
			this.butExportExcel.Image = global::OpenDental.Properties.Resources.butExportExcel;
			this.butExportExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butExportExcel.Location = new System.Drawing.Point(159, 71);
			this.butExportExcel.Name = "butExportExcel";
			this.butExportExcel.Size = new System.Drawing.Size(79, 26);
			this.butExportExcel.TabIndex = 7;
			this.butExportExcel.Text = "Excel";
			this.butExportExcel.Visible = false;
			this.butExportExcel.Click += new System.EventHandler(this.butExportExcel_Click);
			// 
			// butPaste
			// 
			this.butPaste.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPaste.Autosize = true;
			this.butPaste.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPaste.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPaste.CornerRadius = 4F;
			this.butPaste.Image = global::OpenDental.Properties.Resources.butPaste;
			this.butPaste.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPaste.Location = new System.Drawing.Point(78, 56);
			this.butPaste.Name = "butPaste";
			this.butPaste.Size = new System.Drawing.Size(65, 23);
			this.butPaste.TabIndex = 11;
			this.butPaste.Text = "Paste";
			this.butPaste.Click += new System.EventHandler(this.butPaste_Click);
			// 
			// butCopy
			// 
			this.butCopy.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCopy.Autosize = true;
			this.butCopy.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCopy.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCopy.CornerRadius = 4F;
			this.butCopy.Image = global::OpenDental.Properties.Resources.butCopy;
			this.butCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCopy.Location = new System.Drawing.Point(3, 56);
			this.butCopy.Name = "butCopy";
			this.butCopy.Size = new System.Drawing.Size(72, 23);
			this.butCopy.TabIndex = 10;
			this.butCopy.Text = "Copy";
			this.butCopy.Click += new System.EventHandler(this.butCopy_Click);
			// 
			// textTitle
			// 
			this.textTitle.Location = new System.Drawing.Point(63, 3);
			this.textTitle.Name = "textTitle";
			this.textTitle.Size = new System.Drawing.Size(219, 20);
			this.textTitle.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(3, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(54, 13);
			this.label1.TabIndex = 9;
			this.label1.Text = "Title";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Location = new System.Drawing.Point(3, 32);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(140, 23);
			this.butAdd.TabIndex = 3;
			this.butAdd.Text = "Add To Favorites";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butFavorite
			// 
			this.butFavorite.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butFavorite.Autosize = true;
			this.butFavorite.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butFavorite.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butFavorite.CornerRadius = 4F;
			this.butFavorite.Location = new System.Drawing.Point(3, 8);
			this.butFavorite.Name = "butFavorite";
			this.butFavorite.Size = new System.Drawing.Size(140, 23);
			this.butFavorite.TabIndex = 2;
			this.butFavorite.Text = "Favorites";
			this.butFavorite.Click += new System.EventHandler(this.butFavorites_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioHuman);
			this.groupBox1.Controls.Add(this.radioRaw);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(162, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(122, 58);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Format";
			// 
			// radioHuman
			// 
			this.radioHuman.Checked = true;
			this.radioHuman.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioHuman.Location = new System.Drawing.Point(10, 16);
			this.radioHuman.Name = "radioHuman";
			this.radioHuman.Size = new System.Drawing.Size(108, 16);
			this.radioHuman.TabIndex = 0;
			this.radioHuman.TabStop = true;
			this.radioHuman.Text = "Human-readable";
			this.radioHuman.Click += new System.EventHandler(this.radioHuman_Click);
			// 
			// radioRaw
			// 
			this.radioRaw.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioRaw.Location = new System.Drawing.Point(10, 34);
			this.radioRaw.Name = "radioRaw";
			this.radioRaw.Size = new System.Drawing.Size(104, 16);
			this.radioRaw.TabIndex = 1;
			this.radioRaw.Text = "Raw";
			this.radioRaw.Click += new System.EventHandler(this.radioRaw_Click);
			// 
			// butSubmit
			// 
			this.butSubmit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSubmit.Autosize = true;
			this.butSubmit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSubmit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSubmit.CornerRadius = 4F;
			this.butSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butSubmit.Location = new System.Drawing.Point(3, 80);
			this.butSubmit.Name = "butSubmit";
			this.butSubmit.Size = new System.Drawing.Size(102, 23);
			this.butSubmit.TabIndex = 6;
			this.butSubmit.Text = "&Submit Query";
			this.butSubmit.Click += new System.EventHandler(this.butSubmit_Click);
			// 
			// printPreviewDialog2
			// 
			this.printPreviewDialog2.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.printPreviewDialog2.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.printPreviewDialog2.ClientSize = new System.Drawing.Size(400, 300);
			this.printPreviewDialog2.Enabled = true;
			this.printPreviewDialog2.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog2.Icon")));
			this.printPreviewDialog2.Name = "printPreviewDialog2";
			this.printPreviewDialog2.Visible = false;
			// 
			// printPreviewControl2
			// 
			this.printPreviewControl2.AutoZoom = false;
			this.printPreviewControl2.Location = new System.Drawing.Point(6, 136);
			this.printPreviewControl2.Name = "printPreviewControl2";
			this.printPreviewControl2.Size = new System.Drawing.Size(313, 636);
			this.printPreviewControl2.TabIndex = 5;
			this.printPreviewControl2.Zoom = 1D;
			// 
			// butFullPage
			// 
			this.butFullPage.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butFullPage.Autosize = true;
			this.butFullPage.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butFullPage.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butFullPage.CornerRadius = 4F;
			this.butFullPage.Location = new System.Drawing.Point(9, 5);
			this.butFullPage.Name = "butFullPage";
			this.butFullPage.Size = new System.Drawing.Size(75, 27);
			this.butFullPage.TabIndex = 9;
			this.butFullPage.Text = "&Full Page";
			this.butFullPage.Visible = false;
			this.butFullPage.Click += new System.EventHandler(this.butFullPage_Click);
			// 
			// panelZoom
			// 
			this.panelZoom.Controls.Add(this.labelTotPages);
			this.panelZoom.Controls.Add(this.butFullPage);
			this.panelZoom.Controls.Add(this.butZoomIn);
			this.panelZoom.Controls.Add(this.butBack);
			this.panelZoom.Controls.Add(this.butFwd);
			this.panelZoom.Location = new System.Drawing.Point(337, 744);
			this.panelZoom.Name = "panelZoom";
			this.panelZoom.Size = new System.Drawing.Size(229, 37);
			this.panelZoom.TabIndex = 0;
			this.panelZoom.Visible = false;
			// 
			// labelTotPages
			// 
			this.labelTotPages.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTotPages.Location = new System.Drawing.Point(143, 10);
			this.labelTotPages.Name = "labelTotPages";
			this.labelTotPages.Size = new System.Drawing.Size(52, 18);
			this.labelTotPages.TabIndex = 11;
			this.labelTotPages.Text = "25 / 26";
			this.labelTotPages.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// butZoomIn
			// 
			this.butZoomIn.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butZoomIn.Autosize = true;
			this.butZoomIn.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butZoomIn.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butZoomIn.CornerRadius = 4F;
			this.butZoomIn.Image = global::OpenDental.Properties.Resources.butZoomIn;
			this.butZoomIn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butZoomIn.Location = new System.Drawing.Point(9, 5);
			this.butZoomIn.Name = "butZoomIn";
			this.butZoomIn.Size = new System.Drawing.Size(91, 26);
			this.butZoomIn.TabIndex = 12;
			this.butZoomIn.Text = "Zoom In";
			this.butZoomIn.Click += new System.EventHandler(this.butZoomIn_Click);
			// 
			// butBack
			// 
			this.butBack.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBack.Autosize = true;
			this.butBack.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBack.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBack.CornerRadius = 4F;
			this.butBack.Image = global::OpenDental.Properties.Resources.Left;
			this.butBack.Location = new System.Drawing.Point(123, 7);
			this.butBack.Name = "butBack";
			this.butBack.Size = new System.Drawing.Size(18, 23);
			this.butBack.TabIndex = 17;
			this.butBack.Click += new System.EventHandler(this.butBack_Click);
			// 
			// butFwd
			// 
			this.butFwd.AdjustImageLocation = new System.Drawing.Point(1, 0);
			this.butFwd.Autosize = true;
			this.butFwd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butFwd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butFwd.CornerRadius = 4F;
			this.butFwd.Image = global::OpenDental.Properties.Resources.Right;
			this.butFwd.Location = new System.Drawing.Point(195, 7);
			this.butFwd.Name = "butFwd";
			this.butFwd.Size = new System.Drawing.Size(18, 23);
			this.butFwd.TabIndex = 18;
			this.butFwd.Click += new System.EventHandler(this.butFwd_Click);
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(785, 753);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(79, 26);
			this.butPrint.TabIndex = 13;
			this.butPrint.Text = "&Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// butExport
			// 
			this.butExport.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butExport.Autosize = true;
			this.butExport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butExport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butExport.CornerRadius = 4F;
			this.butExport.Image = global::OpenDental.Properties.Resources.butExport;
			this.butExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butExport.Location = new System.Drawing.Point(690, 753);
			this.butExport.Name = "butExport";
			this.butExport.Size = new System.Drawing.Size(79, 26);
			this.butExport.TabIndex = 14;
			this.butExport.Text = "&Export";
			this.butExport.Click += new System.EventHandler(this.butExport_Click);
			// 
			// butQView
			// 
			this.butQView.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butQView.Autosize = true;
			this.butQView.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butQView.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butQView.CornerRadius = 4F;
			this.butQView.Image = global::OpenDental.Properties.Resources.butQView;
			this.butQView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butQView.Location = new System.Drawing.Point(574, 739);
			this.butQView.Name = "butQView";
			this.butQView.Size = new System.Drawing.Size(104, 26);
			this.butQView.TabIndex = 15;
			this.butQView.Text = "&Query View";
			this.butQView.Click += new System.EventHandler(this.butQView_Click);
			// 
			// butPrintPreview
			// 
			this.butPrintPreview.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrintPreview.Autosize = true;
			this.butPrintPreview.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrintPreview.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrintPreview.CornerRadius = 4F;
			this.butPrintPreview.Image = global::OpenDental.Properties.Resources.butPreview;
			this.butPrintPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrintPreview.Location = new System.Drawing.Point(573, 753);
			this.butPrintPreview.Name = "butPrintPreview";
			this.butPrintPreview.Size = new System.Drawing.Size(113, 26);
			this.butPrintPreview.TabIndex = 16;
			this.butPrintPreview.Text = "P&rint Preview";
			this.butPrintPreview.Click += new System.EventHandler(this.butPrintPreview_Click);
			// 
			// splitContainerQuery
			// 
			this.splitContainerQuery.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainerQuery.Location = new System.Drawing.Point(0, 0);
			this.splitContainerQuery.Name = "splitContainerQuery";
			this.splitContainerQuery.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainerQuery.Panel1
			// 
			this.splitContainerQuery.Panel1.Controls.Add(this.panel1);
			this.splitContainerQuery.Panel1.Controls.Add(this.textQuery);
			this.splitContainerQuery.Panel1MinSize = 105;
			// 
			// splitContainerQuery.Panel2
			// 
			this.splitContainerQuery.Panel2.Controls.Add(this.grid2);
			this.splitContainerQuery.Panel2.Controls.Add(this.label1);
			this.splitContainerQuery.Panel2.Controls.Add(this.textTitle);
			this.splitContainerQuery.Panel2MinSize = 200;
			this.splitContainerQuery.Size = new System.Drawing.Size(963, 733);
			this.splitContainerQuery.SplitterDistance = 140;
			this.splitContainerQuery.TabIndex = 17;
			this.splitContainerQuery.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.checkReportServer);
			this.panel1.Controls.Add(this.butFavorite);
			this.panel1.Controls.Add(this.butExportExcel);
			this.panel1.Controls.Add(this.butAdd);
			this.panel1.Controls.Add(this.butPaste);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Controls.Add(this.butSubmit);
			this.panel1.Controls.Add(this.butCopy);
			this.panel1.Location = new System.Drawing.Point(560, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(400, 143);
			this.panel1.TabIndex = 17;
			// 
			// checkReportServer
			// 
			this.checkReportServer.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkReportServer.Location = new System.Drawing.Point(5, 106);
			this.checkReportServer.Name = "checkReportServer";
			this.checkReportServer.Size = new System.Drawing.Size(243, 23);
			this.checkReportServer.TabIndex = 12;
			this.checkReportServer.Text = "Run on report server";
			this.checkReportServer.UseVisualStyleBackColor = true;
			this.checkReportServer.Visible = false;
			// 
			// FormQuery
			// 
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(963, 788);
			this.Controls.Add(this.splitContainerQuery);
			this.Controls.Add(this.butPrintPreview);
			this.Controls.Add(this.printPreviewControl2);
			this.Controls.Add(this.panelZoom);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.butExport);
			this.Controls.Add(this.butQView);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(894, 486);
			this.Name = "FormQuery";
			this.Text = "Query";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormQuery_FormClosing);
			this.Load += new System.EventHandler(this.FormQuery_Load);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.FormQuery_Layout);
			((System.ComponentModel.ISupportInitialize)(this.grid2)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.panelZoom.ResumeLayout(false);
			this.splitContainerQuery.Panel1.ResumeLayout(false);
			this.splitContainerQuery.Panel2.ResumeLayout(false);
			this.splitContainerQuery.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerQuery)).EndInit();
			this.splitContainerQuery.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormQuery_Layout(object sender, System.Windows.Forms.LayoutEventArgs e) {
			printPreviewControl2.Location=new System.Drawing.Point(0,0);
			printPreviewControl2.Height=ClientSize.Height-39;
			printPreviewControl2.Width=ClientSize.Width;	
			grid2.Height=ClientSize.Height-grid2.Location.Y-185;
			grid2.Width=ClientSize.Width-2;
			butClose.Location=new System.Drawing.Point(ClientSize.Width-90,ClientSize.Height-34);
			butExport.Location=new System.Drawing.Point(ClientSize.Width-180,ClientSize.Height-34);
			butPrint.Location=new System.Drawing.Point(ClientSize.Width-270,ClientSize.Height-34);
			butPrintPreview.Location=new System.Drawing.Point(ClientSize.Width-385,ClientSize.Height-34);
			butQView.Location=new System.Drawing.Point(ClientSize.Width-385,ClientSize.Height-34);
			panelZoom.Location=new System.Drawing.Point(ClientSize.Width-620,ClientSize.Height-38);
			splitContainerQuery.Height=ClientSize.Height-39;
			splitContainerQuery.Width=ClientSize.Width-2;
		}

		private void FormQuery_Load(object sender, System.EventArgs e) {
			grid2.Font=bodyFont;
			splitContainerQuery.SplitterDistance=140;
			if(IsReport){
				printPreviewControl2.Visible=true;
				splitContainerQuery.Visible=false;
				Text=Lan.g(this,"Report");
				butPrintPreview.Visible=false;
				panelZoom.Visible=true;
				PrintReport(true);
				labelTotPages.Text="/ "+_totalPages.ToString();
				if(PrefC.GetBool(PrefName.FuchsOptionsOn)) {
					butFullPage.Visible = true;
					butZoomIn.Visible = false;
					printPreviewControl2.Zoom = 1;
				}
				else {
					printPreviewControl2.Zoom = ((double)printPreviewControl2.ClientSize.Height
					/ (double)pd2.DefaultPageSettings.PaperSize.Height);
				}
      }
			else {
				printPreviewControl2.Visible=false;
				Text=Lan.g(this,"Query");
			}
			if(!Security.IsAuthorized(Permissions.UserQueryAdmin,true)) { 
				//lock the query textbox, add button, and paste button for users without permissions.
				textQuery.ReadOnly=true;
				butAdd.Enabled=false;
				butPaste.Enabled=false;
			}
			if(_submitOnLoad) {
				//Coming from FormOpenDental menu item click for query favorites.  Existence in this list is taken to mean sql in these queries is 
				//considered safe to run.
				SubmitQueryThreaded(true);
			}
			if(string.IsNullOrWhiteSpace(PrefC.GetString(PrefName.ReportingServerDbName)) 
				|| string.IsNullOrWhiteSpace(PrefC.GetString(PrefName.ReportingServerCompName))) {
				checkReportServer.Visible=false;
			}
			else {//default to report server when one is set up.
				checkReportServer.Visible=true;
				checkReportServer.Checked=true;
			}
		}

		///<summary>Sets the controls on the form to reflect the execute state passed in.  Typically used to disable all controls while executing.</summary>
		private void LayoutHelperForState(QueryExecuteState executeState) {
			switch (executeState) {
				case QueryExecuteState.Idle:
					butSubmit.Text=Lan.g(this,"Submit Query"); //all controls enabled
					this.Text=Lan.g(this,"Query");
					textQuery.Enabled=true;
					butSubmit.Enabled=true;
					butFavorite.Enabled=true;
					groupBox1.Enabled=true;
					butAdd.Enabled=true;
					butCopy.Enabled=true;
					butPaste.Enabled=true;
					butExportExcel.Enabled=true;
					butExport.Enabled=true;
					butPrintPreview.Enabled=true;
					butPrint.Enabled=true;
					panelZoom.Enabled=true;
					butQView.Enabled=true;
					textTitle.Enabled=true;
					butClose.Enabled=true;
					break;
				case QueryExecuteState.Executing:
					butSubmit.Text=Lan.g(this,"Stop Execution"); //the submit button and the close button should be enabled.
					butClose.Enabled=true;
					this.Text=Lans.g(this,"Query - Executing Query...");
					textQuery.Enabled=false;
					butFavorite.Enabled=false;
					groupBox1.Enabled=false;
					butAdd.Enabled=false;
					butCopy.Enabled=false;
					butPaste.Enabled=false;
					butExportExcel.Enabled=false;
					butExport.Enabled=false;
					butPrintPreview.Enabled=false;
					butPrint.Enabled=false;
					panelZoom.Enabled=false;
					butQView.Enabled=false;
					textTitle.Enabled=false;
					break;
				case QueryExecuteState.Formatting:
					butSubmit.Text=Lan.g(this,"Submit Query"); //no enabled controls
					this.Text=Lans.g(this,"Query - Formatting Query...");
					butSubmit.Enabled=false;
					textQuery.Enabled=false;
					butFavorite.Enabled=false;
					groupBox1.Enabled=false;
					butAdd.Enabled=false;
					butCopy.Enabled=false;
					butPaste.Enabled=false;
					butExportExcel.Enabled=false;
					butExport.Enabled=false;
					butPrintPreview.Enabled=false;
					butPrint.Enabled=false;
					panelZoom.Enabled=false;
					butQView.Enabled=false;
					textTitle.Enabled=false;
					butClose.Enabled=false;
					break;
			}
			if(!Security.IsAuthorized(Permissions.UserQueryAdmin,true)) {
				//lock the query textbox, add button, and paste button for users without permissions.
				textQuery.ReadOnly=true;
				butAdd.Enabled=false;
				butPaste.Enabled=false;
			}
		}

		///<summary>Fills the form with the results of the query and formats it. If the Human Readable checkbox is checked, that logic is also done here.
		///Depending on what was selected, the MakeReadable() method can take a very long time. We may want to include this in the query thread
		///at some point so users can cancel out of this as well.</summary>
		private void FillForm() {
			//if this was called from a different thread, invoke it.
			if(this.InvokeRequired) {
				this.Invoke((Action)(() => { FillForm(); }));
				return;
			}
			//there's nothing in the table, so do nothing.
			if(_reportSimpleGrid.TableQ==null) {
				LayoutHelperForState(QueryExecuteState.Idle);
				return;
			}
			Cursor=Cursors.WaitCursor;
			LayoutHelperForState(QueryExecuteState.Formatting);
			//Set the data binding based on the radio buttons.
			myGridTS = new DataGridTableStyle();
			grid2.TableStyles.Clear();
			grid2.TableStyles.Add(myGridTS);
			if(radioHuman.Checked) {
				//fill the Human Readable table if it is null. (set to null on Submit_Click)
				if(_tableHuman==null) {
					_tableHuman=MakeReadable(_reportSimpleGrid.TableQ,_reportSimpleGrid);
				}
				grid2.SetDataBinding(_tableHuman,"");
				AutoSizeColumns(_tableHuman);
			}
			else {
				grid2.SetDataBinding(_reportSimpleGrid.TableQ,"");
				AutoSizeColumns(_reportSimpleGrid.TableQ);
			}
			_reportSimpleGrid.Title=textTitle.Text;
			_reportSimpleGrid.SubTitle.Clear();
			_reportSimpleGrid.SubTitle.Add(PrefC.GetString(PrefName.PracticeTitle));
			for(int iCol = 0;iCol<_reportSimpleGrid.TableQ.Columns.Count;iCol++) {
				_reportSimpleGrid.ColCaption[iCol]=_reportSimpleGrid.TableQ.Columns[iCol].Caption;
				//again, I don't know why this would fail, so here's a check:
				if(myGridTS.GridColumnStyles.Count >= iCol+1) {
					myGridTS.GridColumnStyles[iCol].Alignment=_reportSimpleGrid.ColAlign[iCol];
				}
			}
			SecurityLogs.MakeLogEntry(Permissions.UserQuery,0,textQuery.Text);
			LayoutHelperForState(QueryExecuteState.Idle);
			Cursor=Cursors.Default;
		}

		#region Thread Stuff
		///Set isSqlValidated to true in order to skip SQL saftey validation.</summary>
		public void SubmitQueryThreaded(bool isSqlValidated=false) {
			if(_threadQuery!=null || _serverThreadID!=0) {
				return;//There is already a query executing.
			}
			//Clear the grid
			grid2.TableStyles.Clear();
			grid2.SetDataBinding(new DataTable(),"");
			//then create a new _reportSimpleGrid
			_reportSimpleGrid=new ReportSimpleGrid();
			_reportSimpleGrid.Query=textQuery.Text;
			_reportSimpleGrid.IsSqlValidated=isSqlValidated;
			if(DataConnection.DBtype==DatabaseType.Oracle) { //Can't cancel User queries for Oracle. this is still from the main thread so we should be ok.
				try {
					if(isSqlValidated || Db.IsSqlAllowed(_reportSimpleGrid.Query)) { //Throws Exception
						SubmitQuery();
					}
				}
				catch(Exception e){
					FriendlyException.Show(Lan.g(this,"Error submitting query."),e);
				}
				return;
			}
			_tableHuman=null;
			LayoutHelperForState(QueryExecuteState.Executing);
			_queryExceptionStateCur=QueryExceptionState.Throw;
			_threadQuery=new ODThread(OnThreadStart);
			_threadQuery.Name="UserQueryThread";
			_threadQuery.GroupName="UserQueryGroup";
			_threadQuery.AddExitHandler(OnThreadExit);
			_threadQuery.AddExceptionHandler(ExceptionThreadHandler);
			_threadQuery.Start(true); //run it once.
		}

		private void OnThreadStart(ODThread thread) {
			_serverThreadID=DataConnectionCancelable.GetServerThread(checkReportServer.Checked);
			_reportSimpleGrid.TableQ=DataConnectionCancelable.GetTableConAlreadyOpen(_serverThreadID,_reportSimpleGrid.Query,_reportSimpleGrid.IsSqlValidated);
			_reportSimpleGrid.InitializeColumns();
		}

		///<summary>_userQuery exitHandler. Fills the form if the form is not disposed.</summary>
		private void OnThreadExit(ODThread thread) {
			try {
				_threadQuery=null;
				_serverThreadID=0;
				FillForm();
			}
			catch(Exception e) {
				e.DoNothing();
			}
		}

		///<summary>_userQuery exceptionHandler. Displays a messagebox or suppresses exceptions based on _exceptionStateCur.</summary>
		private void ExceptionThreadHandler(Exception e) {
			if(this.InvokeRequired) {
				this.Invoke((Action)(() => { ExceptionThreadHandler(e); }));
				return;
			}
			switch(_queryExceptionStateCur) {
				case QueryExceptionState.Interrupt:
					_queryExceptionStateCur = QueryExceptionState.Suppress;
					MsgBox.Show(this,"Query execution interrupted.");
					break;
				case QueryExceptionState.Suppress:
					//Swallow any errors.
					break;
				case QueryExceptionState.Throw:
				default:
					if(e.Message.ToLower().Contains("critical error")) {
						//we want to to see all the details and stacktrace for critical errors.
						new MsgBoxCopyPaste(e.Message+"\r\n"+Lans.g(this,"Please call support.")+"\r\n\r\n"+e.StackTrace).ShowDialog();
					}
					else {
						MessageBox.Show(Lan.g(this,"Invalid query")+": "+e.Message);
					}
					break;
			}
		}
		#endregion

		#region Public Methods
		///<summary>At this point, this only gets called externally when we want to automate the submission of a user query (like for the patients raw report).
		///Most of these reports can also probably use SubmitQueryThreaded() but will stay using SubmitQuery() for now.
		///Oracle also calls this instead of the threaded method.
		///Column names will be handled automatically.</summary>
		public void SubmitQuery(){
			Cursor=Cursors.WaitCursor;
			_tableHuman=null;
			try {
				_reportSimpleGrid.SubmitQuery();
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MessageBox.Show(Lan.g(this,"Invalid Query")+": "+ex.Message);
				return;
			}
			FillForm();
		}

		///<summary>When used as a report, this is called externally. Used instead of SubmitQuery().
		///Column names can be handled manually by the external form calling this report.</summary>
		public void SubmitReportQuery() {
			Cursor=Cursors.WaitCursor;
			_reportSimpleGrid.SubmitQuery();
			Cursor=Cursors.Default;
			grid2.TableStyles.Clear();
			myGridTS = new DataGridTableStyle();
			grid2.TableStyles.Add(myGridTS);
			_tableHuman=MakeReadable(_reportSimpleGrid.TableQ,_reportSimpleGrid);
			grid2.SetDataBinding(_tableHuman,"");
		}

		///<summary>Clears the DataGridTableStyles for the current grid.</summary>
		public void ResetGrid(){ //not really sure what the point of this is.
			grid2.TableStyles.Clear();
			grid2.SetDataBinding(_reportSimpleGrid.TableQ,"");
			myGridTS = new DataGridTableStyle();
			grid2.TableStyles.Add(myGridTS);
		}

		///<summary>Starting to use this externally as well.  OK to pass in a null report. 
		///Pass false to isConvertPatNum if the patnum should be printed as is, true if the patnum should be converted to the patient's full name.</summary>
		public static DataTable MakeReadable(DataTable tableIn,ReportSimpleGrid reportIn,bool doConvertPatNum=true){
			//this can probably be improved upon later for speed
			if(_hashListPlans==null){
				_hashListPlans=InsPlans.GetHListAll();
			}
			DataTable tableOut=tableIn.Clone();//copies just the structure
			for(int j=0;j<tableOut.Columns.Count;j++){
				tableOut.Columns[j].DataType=typeof(string);
			}
			DataRow thisRow;
			//copy data from tableInput to tableOutput while converting to strings
			//string str;
			//Type t;
			for(int i=0;i<tableIn.Rows.Count;i++){
				thisRow=tableOut.NewRow();//new row with new schema
				for(int j=0;j<tableIn.Columns.Count;j++){
					thisRow[j]=PIn.ByteArray(tableIn.Rows[i][j]);
					//str=tableIn.Rows[i][j].ToString();
					//t=tableIn.Rows[i][j].GetType();
					//thisRow[j]=str;
				}
				tableOut.Rows.Add(thisRow);
			}
			DateTime date;
			decimal[] colTotals=new decimal[tableOut.Columns.Count];
			for(int j=0;j<tableOut.Columns.Count;j++){
				for(int i=0;i<tableOut.Rows.Count;i++){
					try{
					if(tableOut.Columns[j].Caption.Substring(0,1)=="$"){
						tableOut.Rows[i][j]=PIn.Double(tableOut.Rows[i][j].ToString()).ToString("F");
						if(reportIn!=null) {
							reportIn.ColAlign[j]=HorizontalAlignment.Right;
							colTotals[j]+=PIn.Decimal(tableOut.Rows[i][j].ToString());
						}
					}
					else if(tableOut.Columns[j].Caption.ToLower().StartsWith("date")){
						date=PIn.Date(tableOut.Rows[i][j].ToString());
						if(date.Year<1880){
							tableOut.Rows[i][j]="";
						}
						else{
							tableOut.Rows[i][j]=date.ToString("d");
						}
					}
					else switch(tableOut.Columns[j].Caption.ToLower())
					{
						//bool
						case "isprosthesis":
						case "ispreventive":
						case "ishidden":
						case "isrecall":
						case "usedefaultfee":
						case "usedefaultcov":
						case "isdiscount":
						case "removetooth":
						case "setrecall":
						case "nobillins":
						case "isprosth":
						case "ishygiene":
						case "issecondary":
						case "orpribool":
						case "orsecbool":
						case "issplit":
  					case "ispreauth":
 					  case "isortho":
            case "releaseinfo":
            case "assignben":
            case "enabled":
            case "issystem":
            case "usingtin":
            case "sigonfile": 
            case "notperson":
            //case "isfrom"://refattach.IsFrom is now refattach.RefType, values 0=ReferralType.RefTo,1=ReferralType.RefFrom,2=ReferralType.RefCustom
							tableOut.Rows[i][j]=PIn.Bool(tableOut.Rows[i][j].ToString()).ToString();
							break;
						//date. Some of these are actually handled further up.
						case "adjdate":
						case "baldate":
						case "dateservice":
						case "datesent":
						case "datereceived":
						case "priordate":
						case "date":
						case "dateviewing":
						case "datecreated":
						case "dateeffective":
						case "dateterm":
						case "paydate":
						case "procdate":
						case "rxdate":
						case "birthdate":
						case "monthyear":
            case "accidentdate":
						case "orthodate":
            case "checkdate":
						case "screendate":
						case "datedue":
						case "dateduecalc":
						case "datefirstvisit":
						case "mydate"://this is a workaround for the daily payment report
							tableOut.Rows[i][j]=PIn.Date(tableOut.Rows[i][j].ToString()).ToString("d");
							break;
						//age
						case "birthdateforage":
							tableOut.Rows[i][j]=PatientLogic.DateToAgeString(PIn.Date(tableOut.Rows[i][j].ToString()));
							break;
						//time 
						case "aptdatetime":
						case "nextschedappt":
						case "starttime":
						case "stoptime":
							tableOut.Rows[i][j]=PIn.DateT(tableOut.Rows[i][j].ToString()).ToString("t")+"   "
								+PIn.DateT(tableOut.Rows[i][j].ToString()).ToString("d");
							break;
						//TimeCardManage
						case "adjevent":
						case "adjreg":
						case "adjotime":
						case "breaktime":
						case "temptotaltime":
						case "tempreghrs":
						case "tempovertime":
							if(PrefC.GetBool(PrefName.TimeCardsUseDecimalInsteadOfColon)) {
								tableOut.Rows[i][j]=PIn.Time(tableOut.Rows[i][j].ToString()).TotalHours.ToString("n");
							}
							else if(PrefC.GetBool(PrefName.TimeCardShowSeconds)) {//Colon format with seconds
								tableOut.Rows[i][j]=PIn.Time(tableOut.Rows[i][j].ToString()).ToStringHmmss();
							}
							else {//Colon format without seconds
								tableOut.Rows[i][j]=PIn.Time(tableOut.Rows[i][j].ToString()).ToStringHmm();
							}
							break;
  					//double
						case "adjamt":
						case "monthbalance":
						case "claimfee":
						case "inspayest":
						case "inspayamt":
						case "dedapplied":
						case "amount":
						case "payamt":
						case "splitamt":
						case "balance":
						case "procfee":
						case "overridepri":
						case "overridesec":
						case "priestim":
						case "secestim":
						case "procfees":
						case "claimpays":
						case "insest":
						case "paysplits":
						case "adjustments":
						case "bal_0_30":
						case "bal_31_60":
						case "bal_61_90":
						case "balover90":
						case "baltotal":
						case "inswoest":
							tableOut.Rows[i][j]=PIn.Double(tableOut.Rows[i][j].ToString()).ToString("F");
							if(reportIn!=null) {
								reportIn.ColAlign[j]=HorizontalAlignment.Right;
								colTotals[j]+=PIn.Decimal(tableOut.Rows[i][j].ToString());
							}
							break;
						case "toothnum":
							tableOut.Rows[i][j]=Tooth.ToInternat(tableOut.Rows[i][j].ToString());
							break;
						//definitions:
						case "adjtype":
							tableOut.Rows[i][j]
								=Defs.GetName(DefCat.AdjTypes,PIn.Long(tableOut.Rows[i][j].ToString()));
							break;
						case "confirmed":
							tableOut.Rows[i][j]
								=Defs.GetValue(DefCat.ApptConfirmed,PIn.Long(tableOut.Rows[i][j].ToString()));
							break;
						case "dx":
							tableOut.Rows[i][j]
								=Defs.GetName(DefCat.Diagnosis,PIn.Long(tableOut.Rows[i][j].ToString()));
							break;
						case "discounttype":
							tableOut.Rows[i][j]
								=Defs.GetName(DefCat.DiscountTypes,PIn.Long(tableOut.Rows[i][j].ToString()));
							break;
						case "doccategory":
							tableOut.Rows[i][j]
								=Defs.GetName(DefCat.ImageCats,PIn.Long(tableOut.Rows[i][j].ToString()));
							break;
						case "op":
							tableOut.Rows[i][j]
								=Operatories.GetAbbrev(PIn.Long(tableOut.Rows[i][j].ToString()));
							break;
						case "paytype":
							tableOut.Rows[i][j]
								=Defs.GetName(DefCat.PaymentTypes,PIn.Long(tableOut.Rows[i][j].ToString()));
							break;
						case "proccat":
							tableOut.Rows[i][j]
								=Defs.GetName(DefCat.ProcCodeCats,PIn.Long(tableOut.Rows[i][j].ToString()));
							break;
						case "unschedstatus":
						case "recallstatus":
							tableOut.Rows[i][j]
								=Defs.GetName(DefCat.RecallUnschedStatus,PIn.Long(tableOut.Rows[i][j].ToString()));
							break;
						case "billingtype":
							tableOut.Rows[i][j]
								=Defs.GetName(DefCat.BillingTypes,PIn.Long(tableOut.Rows[i][j].ToString()));
							break;
						//patnums:
						case "patnum":
						case "guarantor":
						case "pripatnum":
						case "secpatnum":
						case "subscriber":
						case "withpat":
							if(!doConvertPatNum) {
								break;
							}
							if(_dictPatientNames==null) {
								_dictPatientNames=Patients.GetAllPatientNames();
							}
							long patNum=PIn.Long(tableOut.Rows[i][j].ToString());
							if(_dictPatientNames.ContainsKey(patNum)) {
								tableOut.Rows[i][j]=_dictPatientNames[patNum];
							}
							else {
								tableOut.Rows[i][j]="";
								Patient patNew=Patients.GetLim(patNum);
								if(patNew!=null) {		
									_dictPatientNames[patNew.PatNum]=patNew.GetNameLF();
									tableOut.Rows[i][j]=_dictPatientNames[patNum];
								}
							}
							break;
						//plannums:
						case "plannum":
						case "priplannum":
						case "secplannum":
							if(_hashListPlans.ContainsKey(PIn.Long(tableOut.Rows[i][j].ToString())))
								tableOut.Rows[i][j]=_hashListPlans[PIn.Long(tableOut.Rows[i][j].ToString())];
							else
								tableOut.Rows[i][j]="";
							break;
						//referralnum
						case "referralnum":
							Referral referral=null;
							Referrals.TryGetReferral(PIn.Long(tableOut.Rows[i][j].ToString()),out referral);
							tableOut.Rows[i][j]=(referral==null) ? "" : referral.LName+", "+referral.FName+" "+referral.MName;
							break; 
						//enumerations:
						case "aptstatus":
							tableOut.Rows[i][j]
								=((ApptStatus)PIn.Long(tableOut.Rows[i][j].ToString())).ToString();
							break;
						case "category":
							//There are several tables that share the same column name... do your best to determine which enum to use.
							if(reportIn!=null && reportIn.Query!=null && reportIn.Query.ToLower().Contains("displayfield")) {
								tableOut.Rows[i][j]=((DisplayFieldCategory)PIn.Long(tableOut.Rows[i][j].ToString())).ToString();
							}
							else {
								tableOut.Rows[i][j]=((DefCat)PIn.Long(tableOut.Rows[i][j].ToString())).ToString();
							}			
							break;
						case "renewmonth":
							tableOut.Rows[i][j]=((Month)PIn.Long(tableOut.Rows[i][j].ToString())).ToString();
							break;
						case "patstatus":
							tableOut.Rows[i][j]
								=((PatientStatus)PIn.Long(tableOut.Rows[i][j].ToString())).ToString();
							break;
						case "gender":
							tableOut.Rows[i][j]
								=((PatientGender)PIn.Long(tableOut.Rows[i][j].ToString())).ToString();
							break;
						//case "lab":
						//	tableOut.Rows[i][j]
						//		=((LabCaseOld)PIn.PInt(tableOut.Rows[i][j].ToString())).ToString();
						//  break;
						case "position":
							tableOut.Rows[i][j]
								=((PatientPosition)PIn.Long(tableOut.Rows[i][j].ToString())).ToString();
							break;
						case "deductwaivprev":
						case "flocovered":
						case "misstoothexcl":
						case "procstatus":
							tableOut.Rows[i][j]=((ProcStat)PIn.Long(tableOut.Rows[i][j].ToString())).ToString();
							break;
						case "majorwait":
						case "hascaries":
						case "needssealants":
						case "cariesexperience":
						case "earlychildcaries":
						case "existingsealants":
						case "missingallteeth":
							tableOut.Rows[i][j]=((YN)PIn.Long(tableOut.Rows[i][j].ToString())).ToString();
							break;
						case "prirelationship":
						case "secrelationship":
							tableOut.Rows[i][j]=((Relat)PIn.Long(tableOut.Rows[i][j].ToString())).ToString();
							break;
						case "treatarea":
							tableOut.Rows[i][j]
								=((TreatmentArea)PIn.Long(tableOut.Rows[i][j].ToString())).ToString();
							break;
						case "specialty":
							tableOut.Rows[i][j]
								=Defs.GetName(DefCat.ProviderSpecialties,PIn.Long(tableOut.Rows[i][j].ToString()));
							break;
						case "placeservice":
							tableOut.Rows[i][j]
								=((PlaceOfService)PIn.Long(tableOut.Rows[i][j].ToString())).ToString();
							break;
            case "employrelated": 
							tableOut.Rows[i][j]
								=((YN)PIn.Long(tableOut.Rows[i][j].ToString())).ToString();
							break;
            case "schedtype": 
							tableOut.Rows[i][j]
								=((ScheduleType)PIn.Long(tableOut.Rows[i][j].ToString())).ToString();
							break;
            case "dayofweek": 
							tableOut.Rows[i][j]
								=((DayOfWeek)PIn.Long(tableOut.Rows[i][j].ToString())).ToString();
							break;					
						case "raceOld":
							tableOut.Rows[i][j]
								=((PatientRaceOld)PIn.Long(tableOut.Rows[i][j].ToString())).ToString();
							break;
						case "gradelevel":
							tableOut.Rows[i][j]
								=((PatientGrade)PIn.Long(tableOut.Rows[i][j].ToString())).ToString();
							break;
						case "urgency":
							tableOut.Rows[i][j]
								=((TreatmentUrgency)PIn.Long(tableOut.Rows[i][j].ToString())).ToString();
							break;
						case "reftype":
							tableOut.Rows[i][j]=((ReferralType)PIn.Int(tableOut.Rows[i][j].ToString()));
							break;
						//miscellaneous:
						case "provnum":
						case "provhyg":
						case "priprov":
						case "secprov":
            case "provtreat":
            case "provbill":   
							tableOut.Rows[i][j]=Providers.GetAbbr(PIn.Long(tableOut.Rows[i][j].ToString()));
							break;

						case "covcatnum":
							tableOut.Rows[i][j]=CovCats.GetDesc(PIn.Long(tableOut.Rows[i][j].ToString()));
							break;
            case "referringprov": 
	//					  tableOut.Rows[i][j]=CovCats.GetDesc(PIn.PInt(tableOut.Rows[i][j].ToString()));
							break;			
            case "addtime":
							if(tableOut.Rows[i][j].ToString()!="0")
								tableOut.Rows[i][j]+="0";
							break;
						case "feesched":
						case "feeschednum":
							tableOut.Rows[i][j]=FeeScheds.GetDescription(PIn.Long(tableOut.Rows[i][j].ToString()));
							break;
					}//end switch column caption
					}//end try
					catch{
						//return tableOut;
					}
				}//end for i rows
			}//end for j cols
			if(reportIn!=null){
				for(int k=0;k<colTotals.Length;k++){
					reportIn.ColTotal[k]=PIn.Decimal(colTotals[k].ToString("n"));
				}
			}
			return tableOut;
		}

		#endregion

		///<summary>Sizes the columns based on the the length of the text in the columns of the passed-in DataTable.</summary>
		private void AutoSizeColumns(DataTable table) {
			Graphics grfx=this.CreateGraphics();
			//int colWidth;
			int tempWidth;
			for(int i = 0;i<_reportSimpleGrid.ColWidth.Length;i++) {
				_reportSimpleGrid.ColWidth[i]
					=(int)grfx.MeasureString(table.Columns[i].Caption,grid2.Font).Width;
				for(int j = 0;j<table.Rows.Count;j++) {
					tempWidth=(int)grfx.MeasureString(table.Rows[j][i].ToString(),grid2.Font).Width;
					if(tempWidth>_reportSimpleGrid.ColWidth[i])
						_reportSimpleGrid.ColWidth[i]=tempWidth;
				}
				if(_reportSimpleGrid.ColWidth[i]>400) {
					_reportSimpleGrid.ColWidth[i]=400;
				}
				//I have no idea why this is failing, so we'll just do a check:
				if(myGridTS.GridColumnStyles.Count >= i+1) {
					myGridTS.GridColumnStyles[i].Width=_reportSimpleGrid.ColWidth[i]+12;
				}
				_reportSimpleGrid.ColWidth[i]+=6;//so the columns don't touch
				_reportSimpleGrid.ColPos[i+1]=_reportSimpleGrid.ColPos[i]+_reportSimpleGrid.ColWidth[i];
			}
		}

		private void butSubmit_Click(object sender, System.EventArgs e) {
			if(butSubmit.Text==Lan.g(this,"Stop Execution")) { //Abort abort!
				//Flag the currently running query to stop.
				if(_threadQuery!=null) {
					_threadQuery.QuitAsync(true);
				}
				//Cancel the query that is currently running on MySQL using a KILL command.
				if(_serverThreadID!=0) {
					_queryExceptionStateCur = QueryExceptionState.Interrupt;
					DataConnectionCancelable.CancelQuery(_serverThreadID);
				}
			}
			else { //run query
				SubmitQueryThreaded();
			}
		}

		private void butFavorites_Click(object sender, System.EventArgs e) {
			FormQueryFavorites FormQF=new FormQueryFavorites();
			FormQF.UserQueryCur=_userQueryCur;
			FormQF.ShowDialog();
			if(FormQF.DialogResult!=DialogResult.OK) {
				return;
			}
			textQuery.Text=FormQF.UserQueryCur.QueryText;
			textTitle.Text=FormQF.UserQueryCur.Description;
			_userQueryCur=FormQF.UserQueryCur;
			SubmitQueryThreaded();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			FormQueryEdit FormQE=new FormQueryEdit();
			FormQE.UserQueryCur=new UserQuery();
			FormQE.UserQueryCur.QueryText=textQuery.Text;
			FormQE.IsNew=true;
			FormQE.ShowDialog();
			if(FormQE.DialogResult==DialogResult.OK){
				textQuery.Text=FormQE.UserQueryCur.QueryText;
				grid2.CaptionText=FormQE.UserQueryCur.Description;
			}
		}

		///<summary>Formats the current report to be human-readable. Does NOT run the whole query again.</summary>
		private void radioRaw_Click(object sender, System.EventArgs e) {
			FillForm();
		}

		///<summary>Formats the current report to be human-readable. Does NOT run the whole query again.</summary>
		private void radioHuman_Click(object sender, System.EventArgs e) {
			FillForm();
		}

		private void butPrint_Click(object sender, System.EventArgs e) {
			if(_reportSimpleGrid.TableQ==null) {
				MessageBox.Show(Lan.g(this,"Please run query first"));
				return;
			}
			PrintReport(false);
			if(IsReport){
				DialogResult=DialogResult.Cancel;
			}
		}

		private void butPrintPreview_Click(object sender, System.EventArgs e) {
			if(_reportSimpleGrid.TableQ==null) {
				MessageBox.Show(Lan.g(this,"Please run query first"));
				return;
			}
			butFullPage.Visible=false;
			butZoomIn.Visible=true;
			printPreviewControl2.Visible=true;
			butPrintPreview.Visible=false;
			butQView.Visible=true;
			panelZoom.Visible=true;
			splitContainerQuery.Visible=false;
			_totalPages=0;
			PrintReport(true);
			printPreviewControl2.Zoom=((double)printPreviewControl2.ClientSize.Height
				/(double)pd2.DefaultPageSettings.PaperSize.Height);
			labelTotPages.Text="/ "+_totalPages.ToString();
		}

		private void butQView_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.UserQuery)) {
				return;
			}
			printPreviewControl2.Visible=false;
			panelZoom.Visible=false;
			butPrintPreview.Visible=true;
			butQView.Visible=false;
			splitContainerQuery.Visible=true;
		}

		///<summary></summary>
		public void PrintReport(bool justPreview){
			pd2=new PrintDocument();
			pd2.PrintPage += new PrintPageEventHandler(this.pd2_PrintPage);
			pd2.DefaultPageSettings.Margins=new Margins(25,50,50,60);
			if(_reportSimpleGrid.IsLandscape) {
				pd2.DefaultPageSettings.Landscape=true;
				pd2.DefaultPageSettings.Margins=new Margins(25,120,50,60);
			}
			if(pd2.DefaultPageSettings.PrintableArea.Height==0){
				pd2.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			_pagesPrinted=0;
			_linesPrinted=0;
			try{
				if(justPreview){
					printPreviewControl2.Document=pd2;
				}
				else if(PrinterL.SetPrinter(pd2,PrintSituation.Default,0,"Query printed")){
					pd2.Print();
				}
			}
			catch{
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
		}
		
		///<summary>raised for each page to be printed.</summary>
		private void pd2_PrintPage(object sender, PrintPageEventArgs ev){
			Rectangle bounds=ev.MarginBounds;
			float yPos = bounds.Top;
			if(!_headerPrinted){
				ev.Graphics.DrawString(_reportSimpleGrid.Title
					,titleFont,Brushes.Black
					,bounds.Width/2
					-ev.Graphics.MeasureString(_reportSimpleGrid.Title,titleFont).Width/2,yPos);
				yPos+=titleFont.GetHeight(ev.Graphics);
				for(int i=0;i<_reportSimpleGrid.SubTitle.Count;i++){
					ev.Graphics.DrawString(_reportSimpleGrid.SubTitle[i]
						,subtitleFont,Brushes.Black
						,bounds.Width/2
						-ev.Graphics.MeasureString(_reportSimpleGrid.SubTitle[i],subtitleFont).Width/2,yPos);
					yPos+=subtitleFont.GetHeight(ev.Graphics)+2;
				}
				_headerPrinted=true;
			}
			yPos+=10;
			ev.Graphics.DrawString(Lan.g(this,"Date:")+" "+DateTime.Today.ToString("d")
				,bodyFont,Brushes.Black,bounds.Left,yPos);
			//if(totalPages==0){
			ev.Graphics.DrawString(Lan.g(this,"Page:")+" "+(_pagesPrinted+1).ToString()
				,bodyFont,Brushes.Black
				,bounds.Right
				-ev.Graphics.MeasureString(Lan.g(this,"Page:")+" "+(_pagesPrinted+1).ToString()
				,bodyFont).Width,yPos);
			/*}
			else{//maybe work on this later.  Need totalPages on first pass
				ev.Graphics.DrawString("Page: "+(pagesPrinted+1).ToString()+" / "+totalPages.ToString()
					,bodyFont,Brushes.Black
					,bounds.Right
					-ev.Graphics.MeasureString("Page: "+(pagesPrinted+1).ToString()+" / "
					+totalPages.ToString(),bodyFont).Width
					,yPos);
			}*/
			yPos+=bodyFont.GetHeight(ev.Graphics)+10;
			ev.Graphics.DrawLine(new Pen(Color.Black),bounds.Left,yPos-5,bounds.Right,yPos-5);
			//column captions:
			for(int i=0;i<_reportSimpleGrid.ColCaption.Length;i++){
				if(_reportSimpleGrid.ColAlign[i]==HorizontalAlignment.Right){
					ev.Graphics.DrawString(_reportSimpleGrid.ColCaption[i]
						,colCaptFont,Brushes.Black,new RectangleF(
						bounds.Left+_reportSimpleGrid.ColPos[i+1]
						-ev.Graphics.MeasureString(_reportSimpleGrid.ColCaption[i],colCaptFont).Width,yPos
						,_reportSimpleGrid.ColWidth[i],colCaptFont.GetHeight(ev.Graphics)));
				}
				else{
					ev.Graphics.DrawString(_reportSimpleGrid.ColCaption[i]
						,colCaptFont,Brushes.Black,bounds.Left+_reportSimpleGrid.ColPos[i],yPos);
				}
			}
			yPos+=bodyFont.GetHeight(ev.Graphics)+5;
			float fontHeight=bodyFont.GetHeight(ev.Graphics);
			float yPosTableTop=yPos;
			//table: each loop iteration prints one row in the grid.
			while(yPos<bounds.Top+bounds.Height-18//The 18 is minimum allowance for the line about to print. 
				&& _linesPrinted < _reportSimpleGrid.TableQ.Rows.Count)//Page might finish early on the last page.
			{
				bool isColWrap=PrefC.GetBool(PrefName.ReportsWrapColumns);
				if(isColWrap && yPos > yPosTableTop) {//First row always prints.  Otherwise the row might be pushed to next page if too tall.
					int cellWidth;//Width to be adjusted and used to calculate row height.
					bool isRowTooTall=false;//Bool to indicate if a row we are about to print is too tall for the avaible space on page.
					for(int iCol2=0;iCol2<_reportSimpleGrid.TableQ.Columns.Count;iCol2++){
						if(_reportSimpleGrid.ColAlign[iCol2]==HorizontalAlignment.Right) {
							cellWidth=_reportSimpleGrid.ColWidth[iCol2];
						}
						else {
							cellWidth=_reportSimpleGrid.ColPos[iCol2+1]-_reportSimpleGrid.ColPos[iCol2]+6;
						}
						//Current height of the string with given width.
						string cellText=grid2[_linesPrinted,iCol2].ToString();
						float rectHeight=ev.Graphics.MeasureString(cellText,bodyFont,cellWidth).Height;
						if(yPos+rectHeight > bounds.Bottom) {//Check for if we have enough height to print on current page.
							isRowTooTall=true;
							break;
						}
					}
					if(isRowTooTall) {
						break;//Break so current row goes to next page.
					}
				}
				float rowHeight=fontHeight;//When wrapping, we get the hight of the tallest cell in the row and increase yPos by it.
				for(int iCol=0;iCol<_reportSimpleGrid.TableQ.Columns.Count;iCol++){//For each cell in the row, print the cell contents.
					float cellHeight=rowHeight;
					if(isColWrap) {
						cellHeight=0;//Infinate height.
					}
					int cellWidth=0;
					string cellText=grid2[_linesPrinted,iCol].ToString();
					if(_reportSimpleGrid.ColAlign[iCol]==HorizontalAlignment.Right){
						cellWidth=_reportSimpleGrid.ColWidth[iCol];
						ev.Graphics.DrawString(cellText
							,bodyFont,Brushes.Black,new RectangleF(
							bounds.Left+_reportSimpleGrid.ColPos[iCol+1]
							-ev.Graphics.MeasureString(cellText,bodyFont).Width-1,yPos
							,cellWidth,cellHeight));
					}
					else{
						cellWidth=_reportSimpleGrid.ColPos[iCol+1]-_reportSimpleGrid.ColPos[iCol]+6;
						ev.Graphics.DrawString(cellText
							,bodyFont,Brushes.Black,new RectangleF(
							bounds.Left+_reportSimpleGrid.ColPos[iCol],yPos
							,cellWidth
							,cellHeight));
					}
					if(isColWrap) {
						rowHeight=Math.Max(rowHeight,ev.Graphics.MeasureString(cellText,bodyFont,cellWidth).Height);
					}
				}
				yPos+=rowHeight;
				_linesPrinted++;
				if(_linesPrinted==_reportSimpleGrid.TableQ.Rows.Count){
					_tablePrinted=true;
				}
			}
			if(_reportSimpleGrid.TableQ.Rows.Count==0){
				_tablePrinted=true;
			}
			//totals:
			if(_tablePrinted){
				if(yPos<bounds.Bottom){
					ev.Graphics.DrawLine(new Pen(Color.Black),bounds.Left,yPos+3,bounds.Right,yPos+3);
					yPos+=4;
					for(int iCol=0;iCol<_reportSimpleGrid.TableQ.Columns.Count;iCol++){
						if(_reportSimpleGrid.ColAlign[iCol]==HorizontalAlignment.Right){
							if(_reportSimpleGrid.TableQ.Columns[iCol].Caption.ToLower().StartsWith("count(")) {//"=="count(*)") {
								continue;
							}
							float textWidth=(float)(ev.Graphics.MeasureString
								(_reportSimpleGrid.ColTotal[iCol].ToString("n"),subtitleFont).Width);
							ev.Graphics.DrawString(_reportSimpleGrid.ColTotal[iCol].ToString("n")
								,subtitleFont,Brushes.Black,new RectangleF(
								bounds.Left+_reportSimpleGrid.ColPos[iCol+1]-textWidth+3,yPos//the 3 is arbitrary
								,textWidth,subtitleFont.GetHeight(ev.Graphics)));
						}
						//else{
						//	ev.Graphics.DrawString(grid2[linesPrinted,iCol].ToString()
						//		,bodyFont,Brushes.Black,new RectangleF(
						//		bounds.Left+report.ColPos[iCol],yPos
						//		,report.ColPos[iCol+1]-report.ColPos[iCol]
						//,bodyFont.GetHeight(ev.Graphics)));
						//}
					}
					_totalsPrinted=true;
					yPos+=subtitleFont.GetHeight(ev.Graphics);
				}
			}
			//Summary
			if(_totalsPrinted){
				if(yPos+_reportSimpleGrid.Summary.Count*subtitleFont.GetHeight(ev.Graphics)< bounds.Top+bounds.Height){
					ev.Graphics.DrawLine(new Pen(Color.Black),bounds.Left,yPos+2,bounds.Right,yPos+2);
					yPos+=bodyFont.GetHeight(ev.Graphics);
					for(int i=0;i<_reportSimpleGrid.Summary.Count;i++){
						if(_reportSimpleGrid.SummaryFont!=null && _reportSimpleGrid.SummaryFont!=""){
							Font acctnumFont=new Font(_reportSimpleGrid.SummaryFont,12);
							ev.Graphics.DrawString(_reportSimpleGrid.Summary[i],acctnumFont,Brushes.Black,bounds.Left,yPos);
							yPos+=acctnumFont.GetHeight(ev.Graphics);
						}
						else{
							ev.Graphics.DrawString(_reportSimpleGrid.Summary[i],subtitleFont,Brushes.Black,bounds.Left,yPos);
							yPos+=subtitleFont.GetHeight(ev.Graphics);
						}
					}
					_summaryPrinted=true;
				}
			}
			if(!_summaryPrinted){//linesPrinted < report.TableQ.Rows.Count){
				ev.HasMorePages = true;
				_pagesPrinted++;
			}
			else{
				ev.HasMorePages = false;
				//UpDownPage.Maximum=pagesPrinted+1;
				_totalPages=_pagesPrinted+1;
				labelTotPages.Text="1 / "+_totalPages.ToString();
				_pagesPrinted=0;
				_linesPrinted=0;
				_headerPrinted=false;
				_tablePrinted=false;
				_totalsPrinted=false;
				_summaryPrinted=false;
			}
		}

		private void splitContainer1_SplitterMoved(object sender,SplitterEventArgs e) {
			//Dynamically set the height of the grid so that the bottom scrollbar does not disappear when the user resizes the splitter.
			//Subtract 5 so that the grid's horizontal scroll bar does not get pushed off the screen.
			grid2.Height=splitContainerQuery.Height-grid2.Location.Y-splitContainerQuery.SplitterDistance-5;
		}

		private void butZoomIn_Click(object sender, System.EventArgs e){
			butFullPage.Visible=true;
			butZoomIn.Visible=false;
			printPreviewControl2.Zoom=1;
		}

		private void butFullPage_Click(object sender, System.EventArgs e){
			butFullPage.Visible=false;
			butZoomIn.Visible=true;
			printPreviewControl2.Zoom=((double)printPreviewControl2.ClientSize.Height
				/(double)pd2.DefaultPageSettings.PaperSize.Height);
		}

		private void butBack_Click(object sender, System.EventArgs e){
			if(printPreviewControl2.StartPage==0) return;
			printPreviewControl2.StartPage--;
			labelTotPages.Text=(printPreviewControl2.StartPage+1).ToString()
				+" / "+_totalPages.ToString();
		}

		private void butFwd_Click(object sender, System.EventArgs e){
			if(printPreviewControl2.StartPage==_totalPages-1) return;
			printPreviewControl2.StartPage++;
			labelTotPages.Text=(printPreviewControl2.StartPage+1).ToString()
				+" / "+_totalPages.ToString();
		}

		private void butExportExcel_Click(object sender, System.EventArgs e) {
			/*
			saveFileDialog2=new SaveFileDialog();
      saveFileDialog2.AddExtension=true;
			saveFileDialog2.Title=Lan.g(this,"Select Folder to Save File To");
		  if(IsReport){
				saveFileDialog2.FileName=report.Title;
			}
      else{
        saveFileDialog2.FileName=UserQueries.Cur.FileName;
			}
			if(!Directory.Exists( ((Pref)PrefC.HList["ExportPath"]).ValueString )){
				try{
					Directory.CreateDirectory( ((Pref)PrefC.HList["ExportPath"]).ValueString );
					saveFileDialog2.InitialDirectory=((Pref)PrefC.HList["ExportPath"]).ValueString;
				}
				catch{
					//initialDirectory will be blank
				}
			}
			else saveFileDialog2.InitialDirectory=((Pref)PrefC.HList["ExportPath"]).ValueString;
			//saveFileDialog2.DefaultExt="xls";
			//saveFileDialog2.Filter="txt files(*.txt)|*.txt|All files(*.*)|*.*";
      //saveFileDialog2.FilterIndex=1;
		  if(saveFileDialog2.ShowDialog()!=DialogResult.OK){
	   	  return;
			}
			Excel.Application excel=new Excel.ApplicationClass();
			excel.Workbooks.Add(Missing.Value);
			Worksheet worksheet = (Worksheet) excel.ActiveSheet;
			Range range=(Excel.Range)excel.Cells[1,1];
			range.Value2="test";
			range.Font.Bold=true;
			range=(Excel.Range)excel.Cells[1,2];
			range.ColumnWidth=30;
			range.FormulaR1C1="12345";
			excel.Save(saveFileDialog2.FileName);
	//this test case worked, so now it is just a matter of finishing this off, and Excel export will be done.
			MessageBox.Show(Lan.g(this,"File created successfully"));
			*/
		}

		private void butExport_Click(object sender, System.EventArgs e){
			if(_reportSimpleGrid.TableQ==null){
				MessageBox.Show(Lan.g(this,"Please run query first"));
				return;
			}
			saveFileDialog2=new SaveFileDialog();
      saveFileDialog2.AddExtension=true;
			//saveFileDialog2.Title=Lan.g(this,"Select Folder to Save File To");
		  if(IsReport){
				saveFileDialog2.FileName=_reportSimpleGrid.Title;
			}
      else{
				if(_userQueryCur==null || _userQueryCur.FileName==null || _userQueryCur.FileName=="")//.FileName==null)
					saveFileDialog2.FileName=_reportSimpleGrid.Title;
				else
					saveFileDialog2.FileName=_userQueryCur.FileName;
			}
			if(!Directory.Exists(PrefC.GetString(PrefName.ExportPath) )){
				try{
					Directory.CreateDirectory(PrefC.GetString(PrefName.ExportPath) );
					saveFileDialog2.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
				}
				catch{
					//initialDirectory will be blank
				}
			}
			else saveFileDialog2.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
			//saveFileDialog2.DefaultExt="txt";
			saveFileDialog2.Filter="Text files(*.txt)|*.txt|Excel Files(*.xls)|*.xls|All files(*.*)|*.*";
      saveFileDialog2.FilterIndex=0;
		  if(saveFileDialog2.ShowDialog()!=DialogResult.OK){
	   	  return;
			}
			try{
			  using(StreamWriter sw=new StreamWriter(saveFileDialog2.FileName,false))
					//new FileStream(,FileMode.Create,FileAccess.Write,FileShare.Read)))
				{
					String line="";
					for(int i=0;i<_reportSimpleGrid.ColCaption.Length;i++) {
						string columnCaption=_reportSimpleGrid.ColCaption[i];
						//Check for columns that start with special characters that spreadsheet programs treat differently than simply displaying them.
						if(columnCaption.StartsWith("-") || columnCaption.StartsWith("=")) {
							//Adding a space to the beginning of the cell will trick the spreadsheet program into not treating it uniquely.
							columnCaption=" "+columnCaption;
						}
						line+=columnCaption;
						if(i<_reportSimpleGrid.TableQ.Columns.Count-1){
							line+="\t";
						}
					}
					sw.WriteLine(line);
					string cell;
					for(int i=0;i<_reportSimpleGrid.TableQ.Rows.Count;i++){
						line="";
						for(int j=0;j<_reportSimpleGrid.TableQ.Columns.Count;j++){
							if(radioHuman.Checked && _tableHuman != null) {
								cell=_tableHuman.Rows[i][j].ToString();
							}
							else {
								cell=_reportSimpleGrid.TableQ.Rows[i][j].ToString();
							}
							cell=cell.Replace("\r","");
							cell=cell.Replace("\n","");
							cell=cell.Replace("\t","");
							cell=cell.Replace("\"","");
							line+=cell;
							if(j<_reportSimpleGrid.TableQ.Columns.Count-1){
								line+="\t";
							}
						}
						sw.WriteLine(line);
					}
				}
      }
      catch{
        MessageBox.Show(Lan.g(this,"File in use by another program.  Close and try again."));
				return;
			}
			MessageBox.Show(Lan.g(this,"File created successfully"));
		}

		private void butCopy_Click(object sender, System.EventArgs e){
			try {
				Clipboard.SetDataObject(textQuery.Text);
			}
			catch(Exception ex) {
				MsgBox.Show(this,"Could not copy contents to the clipboard.  Please try again.");
				ex.DoNothing();
			}
		}

		private void butPaste_Click(object sender, System.EventArgs e){
			IDataObject iData;
			try {
				iData=Clipboard.GetDataObject();
			}
			catch(Exception ex) {
				MsgBox.Show(this,"Could not paste contents from the clipboard.  Please try again.");
				ex.DoNothing();
				return;
			}
			if(iData.GetDataPresent(DataFormats.Text)){
				textQuery.Text=(String)iData.GetData(DataFormats.Text); 
			}
			else{
				MessageBox.Show(Lan.g(this,"Could not retrieve data off the clipboard."));
			}

		}

		///<summary>KeyDown instead of KeyPress because KeyDown provides e.KeyCode and KeyPress only provides e.KeyChar (which is a char value).</summary>
		private void textQuery_KeyDown(object sender,KeyEventArgs e) {
			if(e.KeyCode.ToString()=="F9") {
				butSubmit_Click(null,null);
			}
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
			Close();
		}

		///<summary>When the form is closing, try to cancel any running queries.</summary>
		private void FormQuery_FormClosing(object sender,FormClosingEventArgs e) {
			//Set the query exception state to suppress all errors because the user is trying to close the window.
			_queryExceptionStateCur=QueryExceptionState.Suppress;
			//Try to cancel any queries that could be running right now.
			if(_serverThreadID!=0) {
				DataConnectionCancelable.CancelQuery(_serverThreadID,false);
			}
			ODThread.QuitAsyncThreadsByGroupName("UserQueryGroup",true);
		}






		///<summary>Enum to determine how to handle exceptions thrown by the _userQuery thread.</summary>
		private enum QueryExceptionState {
			///<summary>1 - means that query execution was interrupted by the user and will display a messagebox stating so.</summary>
			Interrupt,
			///<summary>2 - suppress any exceptions that arise.</summary>
			Suppress,
			///<summary>3 - throw any exceptions that arise (but in a messagebox so it looks better than a UE).</summary>
			Throw
		}

		///<summary>Enum to determine how to handle the controls in this form based on what is currently happening.</summary>
		private enum QueryExecuteState {
			///<summary>1 - Nothing is happening. All controls are available to be clicked, and the Submit button says "Submit Query" on it.</summary>
			Idle,
			///<summary>2 - Query is excecuting. Only the Close button and Submit button is available to be clicked, and the Submit button says "Cancel Query" on it.</summary>
			Executing,
			///<summary>3 - Query is in the process of being displayed. No controls are available to be clicked.</summary>
			Formatting
		}
	}
}
