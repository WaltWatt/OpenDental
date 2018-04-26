using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormJournal:ODForm {
		private OpenDental.UI.ODToolBar ToolBarMain;
		private OpenDental.UI.ODGrid gridMain;
		private IContainer components;
		private Account _acctCur;
		private ImageList imageListMain;
		private PrintDocument pd2;
		private bool headingPrinted;
		private int pagesPrinted;
		private int headingPrintH;
		private Label label1;
		private ValidDate textDateFrom;
		private ValidDate textDateTo;
		private Label label2;
		private OpenDental.UI.Button butRefresh;
		private MonthCalendar calendarFrom;
		private OpenDental.UI.Button butDropFrom;
		private OpenDental.UI.Button butDropTo;
		private MonthCalendar calendarTo;
		private bool isPrinting;
		private ValidDouble textAmt;
		private Label label3;
		private Label label4;
		private TextBox textFindText;
		//set this externally so that the ending balances will match what's showing in the Chart of Accounts.
		public DateTime InitialAsOfDate;

		///<summary></summary>
		public FormJournal(Account accountCur)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			_acctCur=accountCur;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJournal));
			this.imageListMain = new System.Windows.Forms.ImageList(this.components);
			this.gridMain = new OpenDental.UI.ODGrid();
			this.ToolBarMain = new OpenDental.UI.ODToolBar();
			this.label1 = new System.Windows.Forms.Label();
			this.textDateFrom = new OpenDental.ValidDate();
			this.textDateTo = new OpenDental.ValidDate();
			this.label2 = new System.Windows.Forms.Label();
			this.butRefresh = new OpenDental.UI.Button();
			this.calendarFrom = new System.Windows.Forms.MonthCalendar();
			this.butDropFrom = new OpenDental.UI.Button();
			this.butDropTo = new OpenDental.UI.Button();
			this.calendarTo = new System.Windows.Forms.MonthCalendar();
			this.textAmt = new OpenDental.ValidDouble();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textFindText = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// imageListMain
			// 
			this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
			this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListMain.Images.SetKeyName(0, "Add.gif");
			this.imageListMain.Images.SetKeyName(1, "print.gif");
			// 
			// gridMain
			// 
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
			this.gridMain.Location = new System.Drawing.Point(0, 56);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(1044, 615);
			this.gridMain.TabIndex = 11;
			this.gridMain.Title = null;
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableJournal";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// ToolBarMain
			// 
			this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.ToolBarMain.ImageList = this.imageListMain;
			this.ToolBarMain.Location = new System.Drawing.Point(0, 0);
			this.ToolBarMain.Name = "ToolBarMain";
			this.ToolBarMain.Size = new System.Drawing.Size(1044, 25);
			this.ToolBarMain.TabIndex = 0;
			this.ToolBarMain.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.ToolBarMain_ButtonClick);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(2, 31);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(75, 18);
			this.label1.TabIndex = 0;
			this.label1.Text = "From";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(78, 32);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(81, 20);
			this.textDateFrom.TabIndex = 1;
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(268, 32);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(81, 20);
			this.textDateTo.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(195, 31);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 18);
			this.label2.TabIndex = 0;
			this.label2.Text = "To";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(711, 31);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 23);
			this.butRefresh.TabIndex = 10;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.UseVisualStyleBackColor = true;
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// calendarFrom
			// 
			this.calendarFrom.Location = new System.Drawing.Point(5, 56);
			this.calendarFrom.MaxSelectionCount = 1;
			this.calendarFrom.Name = "calendarFrom";
			this.calendarFrom.TabIndex = 4;
			this.calendarFrom.Visible = false;
			this.calendarFrom.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calendarFrom_DateSelected);
			// 
			// butDropFrom
			// 
			this.butDropFrom.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDropFrom.Autosize = true;
			this.butDropFrom.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDropFrom.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDropFrom.CornerRadius = 4F;
			this.butDropFrom.Location = new System.Drawing.Point(161, 30);
			this.butDropFrom.Name = "butDropFrom";
			this.butDropFrom.Size = new System.Drawing.Size(22, 23);
			this.butDropFrom.TabIndex = 3;
			this.butDropFrom.Text = "V";
			this.butDropFrom.UseVisualStyleBackColor = true;
			this.butDropFrom.Click += new System.EventHandler(this.butDropFrom_Click);
			// 
			// butDropTo
			// 
			this.butDropTo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDropTo.Autosize = true;
			this.butDropTo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDropTo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDropTo.CornerRadius = 4F;
			this.butDropTo.Location = new System.Drawing.Point(351, 30);
			this.butDropTo.Name = "butDropTo";
			this.butDropTo.Size = new System.Drawing.Size(22, 23);
			this.butDropTo.TabIndex = 6;
			this.butDropTo.Text = "V";
			this.butDropTo.UseVisualStyleBackColor = true;
			this.butDropTo.Click += new System.EventHandler(this.butDropTo_Click);
			// 
			// calendarTo
			// 
			this.calendarTo.Location = new System.Drawing.Point(231, 56);
			this.calendarTo.MaxSelectionCount = 1;
			this.calendarTo.Name = "calendarTo";
			this.calendarTo.TabIndex = 7;
			this.calendarTo.Visible = false;
			this.calendarTo.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calendarTo_DateSelected);
			// 
			// textAmt
			// 
			this.textAmt.ForeColor = System.Drawing.SystemColors.WindowText;
			this.textAmt.Location = new System.Drawing.Point(450, 32);
			this.textAmt.MaxVal = 100000000D;
			this.textAmt.MinVal = -100000000D;
			this.textAmt.Name = "textAmt";
			this.textAmt.Size = new System.Drawing.Size(81, 20);
			this.textAmt.TabIndex = 8;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(387, 32);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(63, 18);
			this.label3.TabIndex = 0;
			this.label3.Text = "Find Amt";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(537, 32);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(68, 18);
			this.label4.TabIndex = 0;
			this.label4.Text = "Find Text";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// textFindText
			// 
			this.textFindText.Location = new System.Drawing.Point(605, 32);
			this.textFindText.Name = "textFindText";
			this.textFindText.Size = new System.Drawing.Size(78, 20);
			this.textFindText.TabIndex = 9;
			// 
			// FormJournal
			// 
			this.ClientSize = new System.Drawing.Size(1044, 671);
			this.Controls.Add(this.calendarFrom);
			this.Controls.Add(this.calendarTo);
			this.Controls.Add(this.textFindText);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textAmt);
			this.Controls.Add(this.butDropTo);
			this.Controls.Add(this.butDropFrom);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.textDateTo);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textDateFrom);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.ToolBarMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(890, 180);
			this.Name = "FormJournal";
			this.ShowInTaskbar = false;
			this.Text = "Transaction History";
			this.Load += new System.EventHandler(this.FormJournal_Load);
			this.ResizeEnd += new System.EventHandler(this.FormJournal_ResizeEnd);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormJournal_Load(object sender,EventArgs e) {
			DateTime firstofYear=new DateTime(InitialAsOfDate.Year,1,1);
			textDateTo.Text=InitialAsOfDate.ToShortDateString();
			if(_acctCur.AcctType==AccountType.Income || _acctCur.AcctType==AccountType.Expense){
				textDateFrom.Text=firstofYear.ToShortDateString();
			}
			LayoutToolBar();
			FillGrid();
			gridMain.ScrollToEnd();
		}

		///<summary>Causes the toolbar to be laid out again.</summary>
		public void LayoutToolBar() {
			ToolBarMain.Buttons.Clear();
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add Entry"),0,"","Add"));
			if(_acctCur.AcctType==AccountType.Asset){
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Reconcile"),-1,"","Reconcile"));
			}
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Print"),1,"","Print"));
			//ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			//ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Edit"),-1,Lan.g(this,"Edit Selected Account"),"Edit"));
			//ODToolBarButton button=new ODToolBarButton("",-1,"","PageNum");
			//button.Style=ODToolBarButtonStyle.Label;
			//ToolBarMain.Buttons.Add(button);
			//ToolBarMain.Buttons.Add(new ODToolBarButton("",2,"Go Forward One Page","Fwd"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Close"),-1,"Close This Window","Close"));
		}

		private void ToolBarMain_ButtonClick(object sender,OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			switch(e.Button.Tag.ToString()) {
				case "Add":
					Add_Click();
					break;
				case "Reconcile":
					Reconcile_Click();
					break;
				case "Print":
					//The reason we are using a delegate and BeginInvoke() is because of a Microsoft bug that causes the Print Dialog window to not be in focus			
					//when it comes from a toolbar click.
					//https://social.msdn.microsoft.com/Forums/windows/en-US/681a50b4-4ae3-407a-a747-87fb3eb427fd/first-mouse-click-after-showdialog-hits-the-parent-form?forum=winforms
					ToolBarClick toolClick=Print_Click;
					this.BeginInvoke(toolClick);
					break;
				case "Close":
					this.Close();
					break;
			}
		}

		private delegate void ToolBarClick();

		private void FillGrid(){
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!="" || textDateTo.errorProvider1.GetError(textDateTo)!="") {
				return;
			}
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=textDateTo.Text!=""?PIn.Date(textDateTo.Text):DateTime.MaxValue;
			double filterAmt=string.IsNullOrEmpty(textAmt.errorProvider1.GetError(textAmt))?PIn.Double(textAmt.Text):0;
			List<JournalEntry> listJEntries=JournalEntries.GetForAccount(_acctCur.AccountNum);
			List<Transaction> listTransactions=Transactions.GetManyTrans(listJEntries.Select(x => x.TransactionNum).ToList());
			//Resize grid to fit, important for later resizing
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Rows.Clear();
			gridMain.Width=isPrinting?1050:(this.Width-16);//gridMain.Location.XPos=0 and the grid is form width - 16
			gridMain.HideScrollBars=isPrinting;
			gridMain.EndUpdate();
			gridMain.BeginUpdate();
			gridMain.Title=_acctCur.Description+" ("+Lan.g("enumAccountType",_acctCur.AcctType.ToString())+")";
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableJournal","Chk #"),60,HorizontalAlignment.Center));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableJournal","Date"),75));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableJournal","Memo"),isPrinting?200:220));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableJournal","Splits"),isPrinting?200:220));
			int colClearWidth=40;//because the "clear" column has not been added yet.
			int colUserWidth=100;//because the user columns haven't been added yet.
			//if printing, total size=paper width-margins (1050), otherwise total size=gridWidth-scroll bar width of 19
			//divide remaining size, total size - grid col widths, into thirds for the debit, credit, and balance columns
			int colW=(gridMain.Width-(isPrinting?0:19)-gridMain.WidthAllColumns-colClearWidth-2*colUserWidth)/3;
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableJournal","Debit"+(Accounts.DebitIsPos(_acctCur.AcctType)?"(+)":"(-)")),colW,HorizontalAlignment.Right));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableJournal","Credit"+(Accounts.DebitIsPos(_acctCur.AcctType)?"(-)":"(+)")),colW,HorizontalAlignment.Right));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableJournal","Balance"),colW,HorizontalAlignment.Right));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableJournal","Created By"),colUserWidth));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableJournal","Last Edited By"),colUserWidth));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableJournal","Clear"),colClearWidth,HorizontalAlignment.Center));
			gridMain.Rows.Clear();
			ODGridRow row;
			decimal bal=0;
			foreach(JournalEntry jeCur in listJEntries) {
				if(jeCur.DateDisplayed>dateTo) {
					break;
				}
				if(new[] { AccountType.Income,AccountType.Expense }.Contains(_acctCur.AcctType) && jeCur.DateDisplayed<dateFrom) {
					continue;//for income and expense accounts, previous balances are not included. Only the current timespan.
				}
				//DebitIsPos=true for checking acct, bal+=DebitAmt-CreditAmt
				bal+=(Accounts.DebitIsPos(_acctCur.AcctType)?1:-1)*((decimal)jeCur.DebitAmt-(decimal)jeCur.CreditAmt);
				if(new[] { AccountType.Asset,AccountType.Liability,AccountType.Equity }.Contains(_acctCur.AcctType) && jeCur.DateDisplayed<dateFrom) {
					continue;//for asset, liability, and equity accounts, older entries do affect the current balance.
				}
				if(filterAmt!=0 && filterAmt!=jeCur.CreditAmt && filterAmt!=jeCur.DebitAmt){
					continue;
				}
				if(textFindText.Text!="" && new[] { jeCur.Memo,jeCur.CheckNumber,jeCur.Splits }.All(x => !x.ToUpper().Contains(textFindText.Text.ToUpper()))) {
					continue;
				}
				row=new ODGridRow();
				row.Cells.Add(jeCur.CheckNumber);
				row.Cells.Add(jeCur.DateDisplayed.ToShortDateString());
				row.Cells.Add(jeCur.Memo);
				row.Cells.Add(jeCur.Splits);
				row.Cells.Add(jeCur.DebitAmt==0?"":jeCur.DebitAmt.ToString("n"));
				row.Cells.Add(jeCur.CreditAmt==0?"":jeCur.CreditAmt.ToString("n"));
				row.Cells.Add(bal.ToString("n"));
				row.Cells.Add(Userods.GetName(listTransactions.FirstOrDefault(x => x.TransactionNum==jeCur.TransactionNum)?.UserNum??0));
				row.Cells.Add(Userods.GetName(jeCur.SecUserNumEdit));
				row.Cells.Add(jeCur.ReconcileNum==0?"":"X");
				row.Tag=jeCur.Copy();
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			gridMain.ScrollToEnd();
		}

		private void Add_Click(){
			Transaction trans=new Transaction();
			trans.UserNum=Security.CurUser.UserNum;
			Transactions.Insert(trans);//we now have a TransactionNum, and datetimeEntry has been set
			FormTransactionEdit FormT=new FormTransactionEdit(trans.TransactionNum,_acctCur.AccountNum);
			FormT.IsNew=true;
			FormT.ShowDialog();
			if(FormT.DialogResult==DialogResult.Cancel){
				//no need to try-catch, since no journal entries were saved.
				Transactions.Delete(trans);
			}
			FillGrid();
		}

		private void Reconcile_Click() {
			int selectedRow=gridMain.GetSelectedIndex();
			int scrollValue=gridMain.ScrollValue;
			FormReconciles FormR=new FormReconciles(_acctCur.AccountNum);
			FormR.ShowDialog();
			FillGrid();
			gridMain.SetSelected(selectedRow,true);
			gridMain.ScrollValue=scrollValue;
		}

		private void Print_Click(){
			pagesPrinted=0;
			headingPrinted=false;
			#if DEBUG
				PrintReport(true);
			#else
				PrintReport(false);	
			#endif
		}

		///<summary>Preview is only used for debugging.</summary>
		public void PrintReport(bool justPreview) {
			pd2=new PrintDocument();
			pd2.PrintPage += new PrintPageEventHandler(this.pd2_PrintPage);
			pd2.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			if(gridMain.WidthAllColumns>800) {
				pd2.DefaultPageSettings.Landscape=true;
			}
			//pd2.DefaultPageSettings.Margins=new Margins(0,0,0,0);
			//pd2.OriginAtMargins=true;
			if(pd2.DefaultPageSettings.PrintableArea.Height==0) {
				pd2.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			isPrinting=true;
			FillGrid();
			try {
				if(justPreview) {
					FormRpPrintPreview pView = new FormRpPrintPreview();
					pView.printPreviewControl2.Document=pd2;
					pView.ShowDialog();
				}
				else {
					if(PrinterL.SetPrinter(pd2,PrintSituation.Default,0,"Accounting transaction history for "+_acctCur.Description+" printed")) {
						pd2.Print();
					}
				}
			}
			catch {
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
			isPrinting=false;
			FillGrid();
		}

		private void pd2_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			//Rectangle bounds=new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!headingPrinted) {
				text=_acctCur.Description+" ("+Lan.g("enumAccountType",_acctCur.AcctType.ToString())+")";
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				text=DateTime.Today.ToShortDateString();
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
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

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			int selectedRow=e.Row;
			int scrollValue=gridMain.ScrollValue;
			FormTransactionEdit FormT=new FormTransactionEdit(((JournalEntry)gridMain.Rows[e.Row].Tag).TransactionNum,_acctCur.AccountNum);
			FormT.ShowDialog();
			if(FormT.DialogResult==DialogResult.Cancel) {
				return;
			}
			FillGrid();
			gridMain.SetSelected(selectedRow,true);
			gridMain.ScrollValue=scrollValue;
		}

		private void butDropFrom_Click(object sender,EventArgs e) {
			ToggleCalendars();
		}

		private void butDropTo_Click(object sender,EventArgs e) {
			ToggleCalendars();
		}

		private void ToggleCalendars(){
			if(calendarFrom.Visible){
				//hide the calendars
				calendarFrom.Visible=false;
				calendarTo.Visible=false;
			}
			else{
				//set the date on the calendars to match what's showing in the boxes
				if(textDateFrom.errorProvider1.GetError(textDateFrom)==""
					&& textDateTo.errorProvider1.GetError(textDateTo)=="")
				{//if no date errors
					if(textDateFrom.Text==""){
						calendarFrom.SetDate(DateTime.Today);
					}
					else{
						calendarFrom.SetDate(PIn.Date(textDateFrom.Text));
					}
					if(textDateTo.Text=="") {
						calendarTo.SetDate(DateTime.Today);
					}
					else {
						calendarTo.SetDate(PIn.Date(textDateTo.Text));
					}
				}
				//show the calendars
				calendarFrom.Visible=true;
				calendarTo.Visible=true;
			}
		}

		private void calendarFrom_DateSelected(object sender,DateRangeEventArgs e) {
			textDateFrom.Text=calendarFrom.SelectionStart.ToShortDateString();
		}

		private void calendarTo_DateSelected(object sender,DateRangeEventArgs e) {
			textDateTo.Text=calendarTo.SelectionStart.ToShortDateString();
		}

		///<summary>Fires when resizing is complete.  Window doesn't have maximize or minimize, so only need to handle manual resizing.</summary>
		private void FormJournal_ResizeEnd(object sender,EventArgs e) {
			FillGrid();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!=""
				|| textAmt.errorProvider1.GetError(textAmt)!=""
				)
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			calendarFrom.Visible=false;
			calendarTo.Visible=false;
			FillGrid();
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}





















