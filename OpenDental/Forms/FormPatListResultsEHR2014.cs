﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Drawing.Printing;

namespace OpenDental {
	public partial class FormPatListResultsEHR2014:ODForm {
		private List<EhrPatListElement2014> elementList;
		private DataTable table;
		private bool _headingPrinted;
		private int _pagesPrinted;

		public FormPatListResultsEHR2014(List<EhrPatListElement2014> ElementList) {
			InitializeComponent();
			elementList=ElementList;
		}

		private void FormPatListResults_Load(object sender,EventArgs e) {
			if(PrefC.GetString(PrefName.SoftwareName)!="") {
				this.Text+=" - "+PrefC.GetString(PrefName.SoftwareName);
			}
			EhrMeasureEvent measureEvent=new EhrMeasureEvent();
			measureEvent.DateTEvent=DateTime.Now;
			measureEvent.EventType=EhrMeasureEventType.PatientList;
			EhrMeasureEvents.Insert(measureEvent);
			FillGrid();
		}

		private void FillGrid() {
			table=EhrPatListElements.GetListOrderBy2014(elementList);
			int colWidth=0;
			Graphics g=CreateGraphics();
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("PatNum",60,HorizontalAlignment.Center);
			col.SortingStrategy=GridSortingStrategy.AmountParse;
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Full Name",200);
			col.SortingStrategy=GridSortingStrategy.StringCompare;
			gridMain.Columns.Add(col);
			for(int i=0;i<elementList.Count;i++) {
				switch(elementList[i].Restriction) {
					case EhrRestrictionType.Birthdate:
						col=new ODGridColumn("Birthdate",80,HorizontalAlignment.Center);
						col.SortingStrategy=GridSortingStrategy.DateParse;
						gridMain.Columns.Add(col);
						col=new ODGridColumn("Age",80,HorizontalAlignment.Center);
						col.SortingStrategy=GridSortingStrategy.AmountParse;
						gridMain.Columns.Add(col);
						break;
					case EhrRestrictionType.Gender:
						col=new ODGridColumn("Gender",80,HorizontalAlignment.Center);
						gridMain.Columns.Add(col);
						break;
					case EhrRestrictionType.LabResult:
						colWidth=System.Convert.ToInt32(g.MeasureString("Lab Value: "+elementList[i].CompareString,this.Font).Width);
						col.SortingStrategy=GridSortingStrategy.AmountParse;
						colWidth=colWidth+(colWidth/10);//Add 10%
						col=new ODGridColumn("Lab Value: "+elementList[i].CompareString,colWidth,HorizontalAlignment.Center);
						gridMain.Columns.Add(col);
						colWidth=System.Convert.ToInt32(g.MeasureString("Lab Date: "+elementList[i].CompareString,this.Font).Width);
						colWidth=colWidth+(colWidth/10);//Add 10%
						col=new ODGridColumn("Lab Date: "+elementList[i].CompareString,colWidth,HorizontalAlignment.Center);
						col.SortingStrategy=GridSortingStrategy.DateParse;
						gridMain.Columns.Add(col);
						break;
					case EhrRestrictionType.Medication:
						colWidth=System.Convert.ToInt32(g.MeasureString("Prescription Date: "+elementList[i].CompareString,this.Font).Width);
						colWidth=colWidth+(colWidth/10);//Add 10%
						col=new ODGridColumn("Prescription Date: "+elementList[i].CompareString,colWidth,HorizontalAlignment.Center);
						col.SortingStrategy=GridSortingStrategy.DateParse;
						gridMain.Columns.Add(col);
						break;
					case EhrRestrictionType.Problem:
						colWidth=System.Convert.ToInt32(g.MeasureString("Date Diagnosed: "+DiseaseDefs.GetNameByCode(elementList[i].CompareString),this.Font).Width);
						colWidth=colWidth+(colWidth/10);//Add 10%
						col=new ODGridColumn("Date Diagnosed: "+DiseaseDefs.GetNameByCode(elementList[i].CompareString),colWidth,HorizontalAlignment.Center);
						col.SortingStrategy=GridSortingStrategy.DateParse;
						gridMain.Columns.Add(col);
						break;
					case EhrRestrictionType.Allergy:
						colWidth=System.Convert.ToInt32(g.MeasureString("Date Alergic Reaction: "+elementList[i].CompareString,this.Font).Width);
						colWidth=colWidth+(colWidth/10);//Add 10%
						col=new ODGridColumn("Date Alergic Reaction: "+elementList[i].CompareString,colWidth,HorizontalAlignment.Center);
						col.SortingStrategy=GridSortingStrategy.DateParse;
						gridMain.Columns.Add(col);
						break;
					case EhrRestrictionType.CommPref:
						col=new ODGridColumn("Communication Preference",180,HorizontalAlignment.Center);
						gridMain.Columns.Add(col);
						break;
				}
			}
				//  colWidth=System.Convert.ToInt32(g.MeasureString(elementList[i].CompareString,this.Font).Width);
				//  colWidth=colWidth+(colWidth/10);//Add 10%
				//  if(colWidth<90) {
				//    colWidth=90;//Minimum of 90 width.
				//  }
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<table.Rows.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(table.Rows[i]["PatNum"].ToString());
				row.Cells.Add(table.Rows[i]["LName"].ToString()+", "+table.Rows[i]["FName"].ToString());
				//Add 3 to j to compensate for PatNum, LName and FName.
				int k=0;//added to j to iterate through the table columns as j itterates through the elementList 
				for(int j=0;j<elementList.Count;j++){//sometimes one element might pull two columns, Lab Results for instance.//<elementList.Count;j++) {
					switch(elementList[j].Restriction) {
						case EhrRestrictionType.Medication:
						case EhrRestrictionType.Problem:
						case EhrRestrictionType.Allergy:
							row.Cells.Add(table.Rows[i][j+k+3].ToString().Replace(" 12:00:00 AM",""));//safely remove irrelevant time entries.//dates
							break;
						case EhrRestrictionType.Birthdate:
							row.Cells.Add(table.Rows[i][j+k+3].ToString().Replace(" 12:00:00 AM",""));//safely remove irrelevant time entries.//date
							row.Cells.Add(table.Rows[i][j+k+4].ToString());//age
							k++;//to keep the count correct.
							break;
						case EhrRestrictionType.LabResult:
							row.Cells.Add(table.Rows[i][j+k+3].ToString());//obsVal
							row.Cells.Add(table.Rows[i][j+k+4].ToString().Replace(" 12:00:00 AM",""));//safely remove irrelevant time entries.//date
							k++;//to keep the count correct.
							break;
						case EhrRestrictionType.Gender:
							switch(table.Rows[i][j+k+3].ToString()) {
								case "0"://Male
									row.Cells.Add("Male");
									break;
								case "1"://Female
									row.Cells.Add("Female");
									break;
								case "2"://Unknown
								default:
									row.Cells.Add("Unknown");
									break;
							}
							break;
						case EhrRestrictionType.CommPref:
							switch(table.Rows[i][j+k+3].ToString()) {
								case "0"://None
									row.Cells.Add("None");
									break;
								case "1"://DoNotCall
									row.Cells.Add("Do Not Call");
									break;
								case "2"://HmPhone
									row.Cells.Add("Home Phone");
									break;
								case "3"://WkPhone
									row.Cells.Add("Work Phone");
									break;
								case "4"://WirelessPh
									row.Cells.Add("Wireless Phone");
									break;
								case "5"://Email
									row.Cells.Add("Email");
									break;
								case "6"://SeeNotes
									row.Cells.Add("See Notes");
									break;
								case "7"://Mail
									row.Cells.Add("Mail");
									break;
								case "8"://TextMessage		
									row.Cells.Add("TextMessage");
									break;
							}
							break;
					}
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			g.Dispose();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			_pagesPrinted=0;
			_headingPrinted=false;
			PrintDocument pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			//pd.OriginAtMargins=true;
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			#if DEBUG
				FormRpPrintPreview pView = new FormRpPrintPreview();
				pView.printPreviewControl2.Document=pd;
				pView.ShowDialog();
			#else
				if(!PrinterL.SetPrinter(pd,PrintSituation.Default,0,"Printed patient list from EHR")) {
					return;
				}
				try{
					pd.Print();
				}
				catch(Exception ex){
					MessageBox.Show(ex.Message);
				}
#endif	
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
			if(!_headingPrinted) {
				text="Patient List";
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				//Subheading required by some states for EHR reporting.  Shows software name, date the report was generated, and which provider is logged in at time of report.
				string providerName="";
				if(Security.CurUser.ProvNum != 0) {
					providerName=Providers.GetProv(Security.CurUser.ProvNum).GetLongDesc(); //Used GetLongDesc to match the EHR measures printout.  In the future we may want to enhance to use Formal Name and NPI
				}
				text=providerName;
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				text=PrefC.GetString(PrefName.SoftwareName);
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				text=DateTime.Now.ToShortDateString();
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				_headingPrinted=true;
				//headingPrintH=yPos;
			}
			#endregion
			yPos=gridMain.PrintPage(g,_pagesPrinted,bounds,yPos);
			_pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
