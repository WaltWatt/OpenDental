using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Globalization;
using System.Drawing.Printing;
using System.Windows.Forms;
using OpenDental.ReportingComplex;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental
{
	/// <summary>
	/// Summary description for FormRpApptWithPhones.
	/// </summary>
	public class FormRpBirthday : ODForm
	{
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.GroupBox groupBox1;
		private OpenDental.UI.Button butRight;
		private OpenDental.UI.Button butLeft;
		private OpenDental.UI.Button butMonth;
		private GroupBox groupBox4;
		private Label label4;
		private OpenDental.UI.Button butPostcards;
		private int pagesPrinted;
		private ErrorProvider errorProvider1=new ErrorProvider();
		private DataTable BirthdayTable;
		private int patientsPrinted;
		private PrintDocument pd;
		private TextBox textPostcardMsg;
		private OpenDental.UI.FormPrintPreview printPreview;
		private ValidDate validDateFrom;
		private ValidDate validDateTo;

		///<summary></summary>
		public FormRpBirthday()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			OpenDental.UI.Button butSave;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpBirthday));
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.validDateTo = new OpenDental.ValidDate();
			this.validDateFrom = new OpenDental.ValidDate();
			this.butRight = new OpenDental.UI.Button();
			this.butMonth = new OpenDental.UI.Button();
			this.butLeft = new OpenDental.UI.Button();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.textPostcardMsg = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.butPostcards = new OpenDental.UI.Button();
			butSave = new OpenDental.UI.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// butSave
			// 
			butSave.AdjustImageLocation = new System.Drawing.Point(0,0);
			butSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			butSave.Autosize = true;
			butSave.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			butSave.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			butSave.CornerRadius = 4F;
			butSave.Location = new System.Drawing.Point(202,30);
			butSave.Name = "butSave";
			butSave.Size = new System.Drawing.Size(87,26);
			butSave.TabIndex = 44;
			butSave.Text = "Save Msg";
			butSave.Click += new System.EventHandler(this.butSave_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8,99);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(82,18);
			this.label3.TabIndex = 39;
			this.label3.Text = "To";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(10,73);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(82,18);
			this.label2.TabIndex = 37;
			this.label2.Text = "From";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(546,216);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,26);
			this.butCancel.TabIndex = 44;
			this.butCancel.Text = "&Cancel";
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(546,176);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,26);
			this.butOK.TabIndex = 43;
			this.butOK.Text = "Report";
			this.butOK.Click += new System.EventHandler(this.butReport_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.validDateTo);
			this.groupBox1.Controls.Add(this.validDateFrom);
			this.groupBox1.Controls.Add(this.butRight);
			this.groupBox1.Controls.Add(this.butMonth);
			this.groupBox1.Controls.Add(this.butLeft);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(21, 22);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(286,131);
			this.groupBox1.TabIndex = 45;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Date Range";
			// 
			// validDateTo
			// 
			this.validDateTo.Location = new System.Drawing.Point(98, 97);
			this.validDateTo.Name = "validDateTo";
			this.validDateTo.Size = new System.Drawing.Size(99, 20);
			this.validDateTo.TabIndex = 51;
			this.validDateTo.Validating += new System.ComponentModel.CancelEventHandler(this.validDateTo_Validating);
			// 
			// validDateFrom
			// 
			this.validDateFrom.Location = new System.Drawing.Point(98, 70);
			this.validDateFrom.Name = "validDateFrom";
			this.validDateFrom.Size = new System.Drawing.Size(99, 20);
			this.validDateFrom.TabIndex = 50;
			this.validDateFrom.Validating += new System.ComponentModel.CancelEventHandler(this.validDateFrom_Validating);
			// 
			// butRight
			// 
			this.butRight.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butRight.Autosize = true;
			this.butRight.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRight.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRight.CornerRadius = 4F;
			this.butRight.Image = global::OpenDental.Properties.Resources.Right;
			this.butRight.Location = new System.Drawing.Point(206,28);
			this.butRight.Name = "butRight";
			this.butRight.Size = new System.Drawing.Size(45,26);
			this.butRight.TabIndex = 49;
			this.butRight.Click += new System.EventHandler(this.butRight_Click);
			// 
			// butMonth
			// 
			this.butMonth.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butMonth.Autosize = true;
			this.butMonth.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMonth.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMonth.CornerRadius = 4F;
			this.butMonth.Location = new System.Drawing.Point(96,28);
			this.butMonth.Name = "butMonth";
			this.butMonth.Size = new System.Drawing.Size(101,26);
			this.butMonth.TabIndex = 48;
			this.butMonth.Text = "Next Month";
			this.butMonth.Click += new System.EventHandler(this.butMonth_Click);
			// 
			// butLeft
			// 
			this.butLeft.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butLeft.Autosize = true;
			this.butLeft.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLeft.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLeft.CornerRadius = 4F;
			this.butLeft.Image = global::OpenDental.Properties.Resources.Left;
			this.butLeft.Location = new System.Drawing.Point(42,28);
			this.butLeft.Name = "butLeft";
			this.butLeft.Size = new System.Drawing.Size(45,26);
			this.butLeft.TabIndex = 47;
			this.butLeft.Click += new System.EventHandler(this.butLeft_Click);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.textPostcardMsg);
			this.groupBox4.Controls.Add(butSave);
			this.groupBox4.Controls.Add(this.label4);
			this.groupBox4.Controls.Add(this.butPostcards);
			this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox4.Location = new System.Drawing.Point(332,22);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(306,131);
			this.groupBox4.TabIndex = 46;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Postcards";
			// 
			// textPostcardMsg
			// 
			this.textPostcardMsg.AcceptsReturn = true;
			this.textPostcardMsg.Location = new System.Drawing.Point(10,30);
			this.textPostcardMsg.Multiline = true;
			this.textPostcardMsg.Name = "textPostcardMsg";
			this.textPostcardMsg.Size = new System.Drawing.Size(186,87);
			this.textPostcardMsg.TabIndex = 45;
			this.textPostcardMsg.Text = "Dear ?FName,  Happy ?AgeOrdinal Birthday!  Now, you\'re ?Age years old.";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(7,12);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(158,17);
			this.label4.TabIndex = 18;
			this.label4.Text = "Message";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butPostcards
			// 
			this.butPostcards.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butPostcards.Autosize = true;
			this.butPostcards.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPostcards.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPostcards.CornerRadius = 4F;
			this.butPostcards.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPostcards.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPostcards.Location = new System.Drawing.Point(202,91);
			this.butPostcards.Name = "butPostcards";
			this.butPostcards.Size = new System.Drawing.Size(87,26);
			this.butPostcards.TabIndex = 16;
			this.butPostcards.Text = "Preview";
			this.butPostcards.Click += new System.EventHandler(this.butPostcards_Click);
			// 
			// FormRpBirthday
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(660,264);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormRpBirthday";
			this.Text = "Birthday Report";
			this.Load += new System.EventHandler(this.FormRpBirthday_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormRpBirthday_Load(object sender, System.EventArgs e){
			SetNextMonth();
			textPostcardMsg.Text=PrefC.GetString(PrefName.BirthdayPostcardMsg);
			Plugins.HookAddCode(this,"FormRpBirthday.Load_end");
		}

		private void butLeft_Click(object sender, System.EventArgs e) {
			if(!validDateFrom.IsValid || !validDateTo.IsValid) {
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			DateTime dateFrom=PIn.Date(validDateFrom.Text);
			DateTime dateTo=PIn.Date(validDateTo.Text);
			bool toLastDay=false;
			if(CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(dateTo.Year,dateTo.Month)==dateTo.Day){
				toLastDay=true;
			}
			validDateFrom.Text=dateFrom.AddMonths(-1).ToShortDateString();
			dateTo=dateTo.AddMonths(-1);
			validDateTo.Text=dateTo.ToShortDateString();
			if(toLastDay){
				validDateTo.Text=new DateTime(dateTo.Year,dateTo.Month,
					CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(dateTo.Year,dateTo.Month))
					.ToShortDateString();
			}
		}

		private void butMonth_Click(object sender, System.EventArgs e) {
			SetNextMonth();
		}

		private void butRight_Click(object sender, System.EventArgs e) {
			if(!validDateFrom.IsValid || !validDateTo.IsValid) {
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			DateTime dateFrom=PIn.Date(validDateFrom.Text);
			DateTime dateTo=PIn.Date(validDateTo.Text);
			bool toLastDay=false;
			if(CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(dateTo.Year,dateTo.Month)==dateTo.Day){
				toLastDay=true;
			}
			validDateFrom.Text=dateFrom.AddMonths(1).ToShortDateString();
			dateTo=dateTo.AddMonths(1);
			validDateTo.Text=dateTo.ToShortDateString();
			if(toLastDay){
				validDateTo.Text=new DateTime(dateTo.Year,dateTo.Month,
					CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(dateTo.Year,dateTo.Month))
					.ToShortDateString();
			}
		}

		private void SetNextMonth(){
			validDateFrom.Text
				=new DateTime(DateTime.Today.AddMonths(1).Year,DateTime.Today.AddMonths(1).Month,1)
				.ToShortDateString();
			validDateTo.Text
				=new DateTime(DateTime.Today.AddMonths(2).Year,DateTime.Today.AddMonths(2).Month,1).AddDays(-1)
				.ToShortDateString();
			validDateFrom.errorProvider1.SetError(validDateFrom,"");
			validDateTo.errorProvider1.SetError(validDateTo,"");
		}
		
		private void validDateFrom_Validating(object sender,CancelEventArgs e) {
			if(validDateFrom.Text=="") {
				validDateFrom.SetError("Date cannot be blank");
				return;
			}
			try {
				DateTime date = PIn.Date(validDateFrom.Text);
				if(date.Year < 1880) {
					validDateFrom.SetError("Date must be greater than 1880");
					return;
				}
				validDateFrom.SetError("");
			}
			catch {
				validDateFrom.SetError("Invalid Date");
			}
		}

		private void validDateTo_Validating(object sender,CancelEventArgs e) {
			if(validDateTo.Text=="") {
				validDateTo.SetError("Date cannot be blank");
				return;
			}
			try {
				DateTime date = PIn.Date(validDateTo.Text);
				if(date.Year < 1880) {
					validDateTo.SetError("Date must be greater than 1880");
					return;
				}
				validDateTo.SetError("");
			}
			catch {
				validDateTo.SetError("Invalid Date");
			}
		}

		private void butSave_Click(object sender,EventArgs e) {
			if(Prefs.UpdateString(PrefName.BirthdayPostcardMsg,textPostcardMsg.Text)){
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}

		private void butPostcards_Click(object sender,EventArgs e) {
			if(!validDateFrom.IsValid || !validDateTo.IsValid) {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			DateTime dateFrom=PIn.Date(validDateFrom.Text);
			DateTime dateTo=PIn.Date(validDateTo.Text);
			if(dateTo < dateFrom) {
				MsgBox.Show(this,"To date cannot be before From date.");
				return;
			}
			if(dateFrom.AddYears(1) <= dateTo) {
				MsgBox.Show(this,"Date range must not exceed 1 year.");
				return;
			}
			BirthdayTable=RpBirthday.GetBirthdayTable(dateFrom,dateTo);
			if(BirthdayTable.Rows.Count==0) {
				MsgBox.Show(this,"No postcards to preview.");
				return;
			}
			pagesPrinted=0;
			patientsPrinted=0;
			pd=new PrintDocument();
			pd.PrintPage+=new PrintPageEventHandler(this.pdCards_PrintPage);
			pd.OriginAtMargins=true;
			pd.DefaultPageSettings.Margins=new Margins(0,0,0,0);
			if(PrefC.GetLong(PrefName.RecallPostcardsPerSheet)==1) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("Postcard",400,600);
				pd.DefaultPageSettings.Landscape=true;
			}
			else if(PrefC.GetLong(PrefName.RecallPostcardsPerSheet)==3) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("Postcard",850,1100);
			}
			else {//4
				pd.DefaultPageSettings.PaperSize=new PaperSize("Postcard",850,1100);
				pd.DefaultPageSettings.Landscape=true;
			}
			printPreview=new FormPrintPreview(PrintSituation.Postcard,pd,
				(int)Math.Ceiling((double)BirthdayTable.Rows.Count/(double)PrefC.GetLong(PrefName.RecallPostcardsPerSheet)),0,"Birthday report postcards printed");
			printPreview.ShowDialog();
		}

		///<summary>raised for each page to be printed.</summary>
		private void pdCards_PrintPage(object sender,PrintPageEventArgs ev) {
			int totalPages=(int)Math.Ceiling((double)BirthdayTable.Rows.Count/(double)PrefC.GetLong(PrefName.RecallPostcardsPerSheet));
			Graphics g=ev.Graphics;
			float yPos=0;//these refer to the upper left origin of each postcard
			float xPos=0;
			string str;
			int age;
			DateTime birthdate;
			while(yPos<ev.PageBounds.Height-100 && patientsPrinted<BirthdayTable.Rows.Count) {
				//Return Address--------------------------------------------------------------------------
				if(PrefC.GetBool(PrefName.RecallCardsShowReturnAdd)) {
					str=PrefC.GetString(PrefName.PracticeTitle)+"\r\n";
					g.DrawString(str,new Font(FontFamily.GenericSansSerif,9,FontStyle.Bold),Brushes.Black,xPos+45,yPos+60);
					str=PrefC.GetString(PrefName.PracticeAddress)+"\r\n";
					if(PrefC.GetString(PrefName.PracticeAddress2)!="") {
						str+=PrefC.GetString(PrefName.PracticeAddress2)+"\r\n";
					}
					str+=PrefC.GetString(PrefName.PracticeCity)+",  "+PrefC.GetString(PrefName.PracticeST)+"  "+PrefC.GetString(PrefName.PracticeZip)+"\r\n";
					string phone=PrefC.GetString(PrefName.PracticePhone);
					if(CultureInfo.CurrentCulture.Name=="en-US"&& phone.Length==10) {
						str+="("+phone.Substring(0,3)+")"+phone.Substring(3,3)+"-"+phone.Substring(6);
					}
					else {//any other phone format
						str+=phone;
					}
					g.DrawString(str,new Font(FontFamily.GenericSansSerif,8),Brushes.Black,xPos+45,yPos+75);
				}
				//Body text-------------------------------------------------------------------------------
				str=textPostcardMsg.Text;
				if(BirthdayTable.Rows[patientsPrinted]["Preferred"].ToString()!=""){
					str=str.Replace("?FName",BirthdayTable.Rows[patientsPrinted]["Preferred"].ToString());
				}
				else{
					str=str.Replace("?FName",BirthdayTable.Rows[patientsPrinted]["FName"].ToString());
				}
				birthdate=PIn.Date(BirthdayTable.Rows[patientsPrinted]["Birthdate"].ToString());
				//age=Shared.DateToAge(birthdate,PIn.PDate(textDateTo.Text).AddDays(1));//age on the day after the range
				age=PIn.Int(BirthdayTable.Rows[patientsPrinted]["Age"].ToString());
				str=str.Replace("?AgeOrdinal",Shared.NumberToOrdinal(age));
				str=str.Replace("?Age",age.ToString());
				g.DrawString(str,new Font(FontFamily.GenericSansSerif,10),Brushes.Black,new RectangleF(xPos+45,yPos+180,250,190));
				//Patient's Address-----------------------------------------------------------------------
				str=BirthdayTable.Rows[patientsPrinted]["FName"].ToString()+" "
					//+BirthdayTable.Rows[patientsPrinted]["MiddleI"].ToString()+" "
					+BirthdayTable.Rows[patientsPrinted]["LName"].ToString()+"\r\n"
					+BirthdayTable.Rows[patientsPrinted]["Address"].ToString()+"\r\n";
				if(BirthdayTable.Rows[patientsPrinted]["Address2"].ToString()!="") {
					str+=BirthdayTable.Rows[patientsPrinted]["Address2"].ToString()+"\r\n";
				}
				str+=BirthdayTable.Rows[patientsPrinted]["City"].ToString()+", "
					+BirthdayTable.Rows[patientsPrinted]["State"].ToString()+"   "
					+BirthdayTable.Rows[patientsPrinted]["Zip"].ToString()+"\r\n";
				g.DrawString(str,new Font(FontFamily.GenericSansSerif,11),Brushes.Black,xPos+320,yPos+240);
				if(PrefC.GetLong(PrefName.RecallPostcardsPerSheet)==1) {
					yPos+=400;
				}
				else if(PrefC.GetLong(PrefName.RecallPostcardsPerSheet)==3) {
					yPos+=366;
				}
				else {//4
					xPos+=550;
					if(xPos>1000) {
						xPos=0;
						yPos+=425;
					}
				}
				patientsPrinted++;
			}//while
			pagesPrinted++;
			if(pagesPrinted==totalPages) {
				ev.HasMorePages=false;
				pagesPrinted=0;
				patientsPrinted=0;
			}
			else {
				ev.HasMorePages=true;
			}
		}

		private void butReport_Click(object sender, System.EventArgs e){
			if(!validDateFrom.IsValid || !validDateTo.IsValid) {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}

			DateTime dateFrom=PIn.Date(validDateFrom.Text);
			DateTime dateTo=PIn.Date(validDateTo.Text);
			if(dateTo < dateFrom) {
				MsgBox.Show(this,"To date cannot be before From date.");
				return;
			}
			if(dateFrom.AddYears(1) <= dateTo) {
				MsgBox.Show(this,"Date range must not exceed 1 year.");
				return;
			}
			ReportComplex report=new ReportComplex(true,false);
			Font font=new Font("Tahoma",9);
			Font fontTitle=new Font("Tahoma",17,FontStyle.Bold);
			Font fontSubTitle=new Font("Tahoma",10,FontStyle.Bold);
			report.ReportName=Lan.g(this,"Birthdays");
			report.AddTitle("Title",Lan.g(this,"Birthdays"),fontTitle);
			report.AddSubTitle("PracTitle",PrefC.GetString(PrefName.PracticeTitle),fontSubTitle);
			report.AddSubTitle("Date",dateFrom.ToShortDateString()+" - "+dateTo.ToShortDateString(),fontSubTitle);
			QueryObject query=report.AddQuery(RpBirthday.GetBirthdayTable(dateFrom,dateTo),"","",SplitByKind.None,1,true);
			query.AddColumn("LName",90,FieldValueType.String,font);
			query.AddColumn("FName",90,FieldValueType.String,font);
			query.AddColumn("Preferred",90,FieldValueType.String,font);
			query.AddColumn("Address",90,FieldValueType.String,font);
			query.AddColumn("Address2",90,FieldValueType.String,font);
			query.AddColumn("City",75,FieldValueType.String,font);
			query.AddColumn("State",60,FieldValueType.String,font);
			query.AddColumn("Zip",75,FieldValueType.String,font);
			query.AddColumn("Birthdate",75,FieldValueType.Date,font);
			query.GetColumnDetail("Birthdate").StringFormat="d";
			query.AddColumn("Age",45,FieldValueType.Integer,font);
			report.AddPageNum(font);
			if(!report.SubmitQueries()) {
				return;
			}
			FormReportComplex FormR=new FormReportComplex(report);
			FormR.ShowDialog();
			DialogResult=DialogResult.OK;
		}

		
	}
}
