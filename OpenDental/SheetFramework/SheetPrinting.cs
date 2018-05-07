using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace OpenDental {
	public class SheetPrinting {
		///<summary>If there is only one sheet, then this will stay 0.</Summary>
		private static int _sheetsPrinted;
		///<summary>Pages printed on current sheet.</summary>
		private static int _pagesPrinted;
		///<summary>Used for determining page breaks. When moving to next page, use this Y value to determine the next field to print.</summary>
		private static int _yPosPrint;
		///<summary>Print margin of the default printer. only used in page break calulations, and only top and bottom are used.</summary>
		private static Margins _printMargin=new Margins(0,0,40,60);
		///<summary>If not a batch, then there will just be one sheet in the list.</summary>
		private static List<Sheet> _sheetList;
#if DEBUG
		///<summary>Used to force old single page behavior. Used for labels.</summary>
		//private static bool _forceSinglePage;
		private static bool _printCalibration=false;//debug only
#endif
		private static bool _isPrinting=false;
		private static Statement _stmt;
		private static MedLab _medLab;
		///<summary>Used when printing statements that use the Statements use Sheets feature.  Pdf printing does not use this variable.</summary>
		private static DataSet _dataSet;
		///<summary>Used when printing ERAs to speed up the printing of the EraClaimsPaid grid.</summary>
		private static int idxPreClaimPaidPrinted=0;
		private static int yPosPrevious;

		///<summary>The treatment finder needs to be able to clear out the pages printed variable before it prints a batch.</summary>
		public static int PagesPrinted {
			get {
				return _pagesPrinted;
			}
			set {
				_pagesPrinted=value;
			}
		}

		///<summary>The treatment finder needs this so that it can use the same Margins in its page calculations.</summary>
		public static Margins PrintMargin {
			get {
				return _printMargin;
			}
		}

		/////<summary>Not used. This code is copied and pasted in several locations. Easiest to find by searching for "info.Verb="print";"</summary>
		//public static void PrintStatement(object parameters) {
		//	List<object> listParams=(List<object>)parameters;
		//	SheetDef sheetDef=(SheetDef)listParams[0];
		//	Statement stmt=(Statement)listParams[1];
		//	string filePath=(string)listParams[2];
		//	try {
		//		ProcessStartInfo info=new ProcessStartInfo();
		//		info.Arguments = "\"" + Printers.GetForSit(PrintSituation.Statement).PrinterName + "\"";
		//		info.UseShellExecute = true;
		//		info.Verb="PrintTo";
		//		info.FileName=filePath;
		//		info.CreateNoWindow=true;
		//		info.WindowStyle=ProcessWindowStyle.Hidden;
		//		Process p=new Process();
		//		p.StartInfo=info;
		//		p.Start();
		//		p.WaitForInputIdle();
		//		System.Threading.Thread.Sleep(3000);
		//		if(p.CloseMainWindow()==false) {
		//			p.Kill();
		//		}
		//	}
		//	catch(Exception ex) {
		//		//Must restet sheet, as PDF printing modifies fields.
		//		Sheet sheet=SheetUtil.CreateSheet(sheetDef,stmt.PatNum,stmt.HidePayment);
		//		SheetFiller.FillFields(sheet,stmt);
		//		SheetUtil.CalculateHeights(sheet,GraphicsHelper.GM,stmt);
		//		SheetPrinting.Print(sheet,1,false,stmt);//use GDI+ printing, which is slightly different than the pdf.
		//	}
		//}

		///<summary>Surround with try/catch.</summary>
		public static void PrintBatch(List<Sheet> sheetBatch){
			//currently no validation for parameters in a batch because of the way it was created.
			//could validate field names here later.
			_sheetList=sheetBatch;
			_sheetsPrinted=0;
			_pagesPrinted=0;
			_yPosPrint=0;
			PrintDocument pd=new PrintDocument();
			//Check to see if there is a default printer.  If there are no printers installed,
			//it will throw an exception when trying to set the papersize
			if(PrinterSettings.InstalledPrinters.Count==0) {
				MsgBox.Show("Sheet","Error: No Printers Installed\r\n"+
									"If you do have a printer installed, restarting the workstation may solve the problem."
				);
				return;
			}
			pd.OriginAtMargins=true;
			pd.PrintPage+=new PrintPageEventHandler(pd_PrintPage);
			if(sheetBatch[0].Width>0 && sheetBatch[0].Height>0){
				pd.DefaultPageSettings.PaperSize=new PaperSize("Default",sheetBatch[0].Width,sheetBatch[0].Height);
			}
			PrintSituation sit=PrintSituation.Default;
			pd.DefaultPageSettings.Landscape=sheetBatch[0].IsLandscape;
			switch(sheetBatch[0].SheetType){
				case SheetTypeEnum.LabelPatient:
				case SheetTypeEnum.LabelCarrier:
				case SheetTypeEnum.LabelReferral:
					sit=PrintSituation.LabelSingle;
					break;
				case SheetTypeEnum.ReferralSlip:
					sit=PrintSituation.Default;
					break;
				case SheetTypeEnum.RxMulti:
					sit=PrintSituation.RxMulti;
					break;
			}
			//Moved Calculate heights here because we need to caluclate height before printing, not while we are printing.
			foreach(Sheet s in _sheetList) {
				SheetUtil.CalculateHeights(s,null,null,_isPrinting,_printMargin.Top,_printMargin.Bottom);
			}
			//later: add a check here for print preview.
			#if DEBUG
				pd.DefaultPageSettings.Margins=new Margins(20,20,0,0);
				int pageCount=0;
				foreach(Sheet s in _sheetList) {
					//SetForceSinglePage(s);
					SheetUtil.CalculateHeights(s,null,null,_isPrinting,_printMargin.Top,_printMargin.Bottom);
					pageCount+=Sheets.CalculatePageCount(s,_printMargin);//(_forceSinglePage?1:Sheets.CalculatePageCount(s,_printMargin));
				}
				FormPrintPreview printPreview=new FormPrintPreview(sit,pd,pageCount,0,"Batch of "+sheetBatch[0].Description+" printed");
				printPreview.ShowDialog();
			#else
				try {
					foreach(Sheet s in _sheetList) {
						s.SheetFields.Sort(OpenDentBusiness.SheetFields.SortDrawingOrderLayers);
					}
					if(!PrinterL.SetPrinter(pd,sit,0,"Batch of "+sheetBatch[0].Description+" printed")) {
						return;
					}
					pd.DefaultPageSettings.Margins=new Margins(0,0,0,0);
					pd.Print();
				}
				catch(Exception ex){
					throw ex;
					//MessageBox.Show(Lan.g("Sheet","Printer not available"));
				}
			#endif
		}

#region Rx Sheets
		///<summary>Validates one Rx.  Returns a string of error messages.  Blank string indicates no errors.
		///Some Rx require certain data to be present when printing.</summary>
		public static string ValidateRxForSheet(RxPat rx) {
			if(!PrefC.GetBool(PrefName.RxHasProc)) {
				return "";//The global preference allows the user to completely disable Rx ProcCode validation, even if some Rx are flagged as required.
			}
			if(Clinics.ClinicNum!=0) {//Not HQ
				Clinic clinic=Clinics.GetClinic(Clinics.ClinicNum);
				if(!clinic.HasProcOnRx) {
					//The clinic option allows the user to completely disable Rx ProcCode validation for one clinic.
					//A customer who needs this feature has some clinics in states which require ProCode and some clinics in states which do not.
					return "";
				}
			}
			if(!rx.IsProcRequired) {
				return "";//No ProCode validation required for this specific Rx.
			}
			StringBuilder sb=new StringBuilder();
			if(rx.ProcNum==0) {
				if(sb.Length > 0) {
					sb.Append(", ");
				}
				sb.Append(Lans.g("Sheets","Procedure"));
			}
			if(rx.DaysOfSupply<=0) {
				if(sb.Length > 0) {
					sb.Append(", ");
				}
				sb.Append(Lans.g("Sheets","Days of Supply"));
			}
			return sb.ToString();
		}

		///<summary>Performs validation on the given Rx before printing.  Returns false if validation failed.</summary>
		public static bool PrintRx(Sheet sheet,RxPat rx){
			string validationErrors=ValidateRxForSheet(rx);
			if(validationErrors!="") {
				MessageBox.Show(Lan.g("Sheets","Cannot print until missing info is fixed: ")+validationErrors);
				return false;
			}
			Print(sheet,1,rx.IsControlled);
			return true;
		}

		///<summary>Performs validation on each Rx before printing.
		///If one of the given Rx does not pass validation, then the entire batch is aborted, because it is easy for user to fix errors.</summary>
		public static void PrintMultiRx(List<RxPat> listRxs) {
			SheetDef sheetDef=SheetDefs.GetInternalOrCustom(SheetInternalType.RxMulti);
			List<int> rxSheetCountList=GetSheetRxCount(sheetDef);//gets the number of rx available in the sheet
			if(sheetDef.Parameters.Count==0) {//adds parameters if internal sheet
				sheetDef.Parameters.Add(new SheetParameter(true,"ListRxNums"));
				sheetDef.Parameters.Add(new SheetParameter(true,"ListRxSheet"));
			}
			List<Sheet> batchRxList=new List<Sheet>();//list of sheets to be batch printed
			if(rxSheetCountList.Count==0) {
				MessageBox.Show(Lan.g("Sheets","MuitiRx sheet is invalid."
					+"Please visit the manual to see what output fields must be added to the MultiRx Sheet."));
				return;
			}
			//Sort RxPats into batches. rxSheetCount is most rx's we can print on one sheet.
			int batchSize=rxSheetCountList.Count;
			int batchIdx=0;
			List<List<RxPat>> batches=new List<List<RxPat>>();
			for(int i = 0;i<listRxs.Count;i++) {
				if(ValidateRxForSheet(listRxs[i])!="") {
					MsgBox.Show("Sheets","One or more of the selected prescriptions is missing information.\r\nPlease fix and try again.");
					return;
				}
				if(i>0 && i%batchSize==0) {
					batchIdx++;
				}
				if(i%batchSize==0) {
					batches.Add(new List<RxPat>());
				}
				batches[batchIdx].Add(listRxs[i]);
			}
			//Fill and add sheets to batchRxList to be printed
			foreach(List<RxPat> listBatch in batches) {
				Sheet sheet=SheetUtil.CreateSheet(sheetDef,listRxs[0].PatNum);
				SheetParameter.SetParameter(sheet,"ListRxNums",listBatch);
				SheetParameter.SetParameter(sheet,"ListRxSheet",rxSheetCountList);
				SheetFiller.FillFields(sheet);
				batchRxList.Add(sheet);
			}
			PrintBatch(batchRxList);
		}

		///<summary>Returns a list of the integer values corresponding to the Rx multi defs (1 through 6) found on the MultiRx sheet.
		///An rx is considered valid if it has ProvNameFL,PatNameFL,PatBirthdate, and Drug</summary>
		private static List<int> GetSheetRxCount(SheetDef sheetDef) {
			List<int> rxFieldList=new List<int>();
			bool hasProvNameFL=false;
			bool hasProvNameFL2=false;
			bool hasProvNameFL3=false;
			bool hasProvNameFL4=false;
			bool hasProvNameFL5=false;
			bool hasProvNameFL6=false;
			bool hasPatNameFL=false;
			bool hasPatNameFL2=false;
			bool hasPatNameFL3=false;
			bool hasPatNameFL4=false;
			bool hasPatNameFL5=false;
			bool hasPatNameFL6=false;
			bool hasPatBirthdate=false;
			bool hasPatBirthdate2=false;
			bool hasPatBirthdate3=false;
			bool hasPatBirthdate4=false;
			bool hasPatBirthdate5=false;
			bool hasPatBirthdate6=false;
			bool hasDrug=false;
			bool hasDrug2=false;
			bool hasDrug3=false;
			bool hasDrug4=false;
			bool hasDrug5=false;
			bool hasDrug6=false;
			foreach(SheetFieldDef field in sheetDef.SheetFieldDefs) {
				if(field.FieldName=="prov.nameFL") {
					hasProvNameFL=true;
				}
				if(field.FieldName=="prov.nameFL2") {
					hasProvNameFL2=true;
				}
				if(field.FieldName=="prov.nameFL3") {
					hasProvNameFL3=true;
				}
				if(field.FieldName=="prov.nameFL4") {
					hasProvNameFL4=true;
				}
				if(field.FieldName=="prov.nameFL5") {
					hasProvNameFL5=true;
				}
				if(field.FieldName=="prov.nameFL6") {
					hasProvNameFL6=true;
				}
				if(field.FieldName=="pat.nameFL") {
					hasPatNameFL=true;
				}
				if(field.FieldName=="pat.nameFL2") {
					hasPatNameFL2=true;
				}
				if(field.FieldName=="pat.nameFL3") {
					hasPatNameFL3=true;
				}
				if(field.FieldName=="pat.nameFL4") {
					hasPatNameFL4=true;
				}
				if(field.FieldName=="pat.nameFL5") {
					hasPatNameFL5=true;
				}
				if(field.FieldName=="pat.nameFL6") {
					hasPatNameFL6=true;
				}
				if(field.FieldName=="pat.Birthdate") {
					hasPatBirthdate=true;
				}
				if(field.FieldName=="pat.Birthdate2") {
					hasPatBirthdate2=true;
				}
				if(field.FieldName=="pat.Birthdate3") {
					hasPatBirthdate3=true;
				}
				if(field.FieldName=="pat.Birthdate4") {
					hasPatBirthdate4=true;
				}
				if(field.FieldName=="pat.Birthdate5") {
					hasPatBirthdate5=true;
				}
				if(field.FieldName=="pat.Birthdate6") {
					hasPatBirthdate6=true;
				}
				if(field.FieldName=="Drug") {
					hasDrug=true;
				}
				if(field.FieldName=="Drug2") {
					hasDrug2=true;
				}
				if(field.FieldName=="Drug3") {
					hasDrug3=true;
				}
				if(field.FieldName=="Drug4") {
					hasDrug4=true;
				}
				if(field.FieldName=="Drug5") {
					hasDrug5=true;
				}
				if(field.FieldName=="Drug6") {
					hasDrug6=true;
				}
			}	
			if(hasProvNameFL && hasPatNameFL && hasPatBirthdate && hasDrug) {
				rxFieldList.Add(1);
			}
			if(hasProvNameFL2 && hasPatNameFL2 && hasPatBirthdate2 && hasDrug2) {
				rxFieldList.Add(2);
			}
			if(hasProvNameFL3 && hasPatNameFL3 && hasPatBirthdate3 && hasDrug3) {
				rxFieldList.Add(3);
			}
			if(hasProvNameFL4 && hasPatNameFL4 && hasPatBirthdate4 && hasDrug4) {
				rxFieldList.Add(4);
			}
			if(hasProvNameFL5 && hasPatNameFL5 && hasPatBirthdate5 && hasDrug5) {
				rxFieldList.Add(5);
			}
			if(hasProvNameFL6 && hasPatNameFL6 && hasPatBirthdate6 && hasDrug6) {
				rxFieldList.Add(6);
			}
			return rxFieldList;
		}
#endregion Rx Sheets

		public static void SetZero() {
			_sheetsPrinted=0;
			_yPosPrint=0;
		}

		///<summary>If printing a statement, use the polymorphism that takes a DataSet otherwise this method will make another call to the db.</summary>
		public static PrintDocument Print(Sheet sheet,int copies=1,bool isRxControlled=false,Statement stmt=null,MedLab medLab=null,bool isPrintDocument=true,bool isPreviewMode=false) {
			if(sheet.SheetType==SheetTypeEnum.Statement && stmt!=null) {
				//This should never get hit.  This line of code is here just in case I forgot to update a random spot in our code.
				//Worst case scenario we will end up calling the database a few extra times for the same data set.
				//It use to call this method many, many times so anything is an improvement at this point.
				if(stmt.SuperFamily!=0) {
					_dataSet=AccountModules.GetSuperFamAccount(stmt);
				}
				else {
					_dataSet=AccountModules.GetAccount(stmt.PatNum,stmt);
				}
			}
			return Print(sheet,_dataSet,copies,isRxControlled,stmt,medLab,isPrintDocument,isPreviewMode);
		}

		///<Summary>DataSet should be prefilled with AccountModules.GetAccount() before calling this method if printing a statement.  
		///Returns null if document failed to print.</Summary>
		public static PrintDocument Print(Sheet sheet,DataSet dataSet,int copies=1,bool isRxControlled=false,Statement stmt=null,MedLab medLab=null,
			bool isPrintDocument=true,bool isPreviewMode=false)
		{
			PrintDocument retVal=null;
			try {
				//If we were unable to create the printdocument this will throw an exception.
				retVal=TryPrint(sheet,dataSet,copies,isRxControlled,stmt,medLab,isPrintDocument,isPreviewMode);
			}
			catch(InvalidPrinterException ex) {//Dont catch other exceptions so that customer calls and we can be aware.
				ex.DoNothing();
				MsgBox.Show("Sheets","Error: No Printers Installed\r\n"+
									"If you do have a printer installed, restarting the workstation may solve the problem."
				);
			}
			return retVal;
		}

		///<summary>Throws exception.</summary>
		public static PrintDocument TryPrint(Sheet sheet, DataSet dataSet, int copies=1, bool isRxControlled=false,Statement stmt=null,MedLab medLab=null,bool isPrintDocument=true,bool isPreviewMode = false) { 
			_dataSet=dataSet;
			//parameter null check moved to SheetFiller.
			//could validate field names here later.
			_stmt=stmt;
			_medLab=medLab;
			_isPrinting=true;
			_sheetsPrinted=0;
			_yPosPrint=0;// _printMargin.Top;
			PrintDocument pd=new PrintDocument();
			pd.OriginAtMargins=true;
			pd.PrintPage+=new PrintPageEventHandler(pd_PrintPage);
			if(pd.DefaultPageSettings.PrintableArea.Width==0) {
				//prevents bug in some printers that do not specify paper size
				pd.DefaultPageSettings.PaperSize=new PaperSize("paper",850,1100);
			}
			if(sheet.SheetType==SheetTypeEnum.LabelPatient
				|| sheet.SheetType==SheetTypeEnum.LabelCarrier
				|| sheet.SheetType==SheetTypeEnum.LabelAppointment
				|| sheet.SheetType==SheetTypeEnum.LabelReferral) 
			{//I think this causes problems for non-label sheet types.
				if(sheet.Width>0 && sheet.Height>0) {
					pd.DefaultPageSettings.PaperSize=new PaperSize("Default",sheet.Width,sheet.Height);
				}
			}
			pd.DefaultPageSettings.Margins=new Margins(0,0,0,0);
			pd.OriginAtMargins=true;
			PrintSituation sit=PrintSituation.Default;
			pd.DefaultPageSettings.Landscape=sheet.IsLandscape;
			switch(sheet.SheetType){
				case SheetTypeEnum.LabelPatient:
				case SheetTypeEnum.LabelCarrier:
				case SheetTypeEnum.LabelReferral:
				case SheetTypeEnum.LabelAppointment:
					sit=PrintSituation.LabelSingle;
					break;
				case SheetTypeEnum.ReferralSlip:
					sit=PrintSituation.Default;
					break;
				case SheetTypeEnum.Rx:
					if(isRxControlled){
						sit=PrintSituation.RxControlled;
					}
					else{
						sit=PrintSituation.Rx;
					}
					break;
				case SheetTypeEnum.Statement:
					sit= PrintSituation.Statement;
					break;
				case SheetTypeEnum.TreatmentPlan:
					sit=PrintSituation.TPPerio;
					break;
			}
			Sheets.SetPageMargin(sheet,_printMargin);
			foreach(SheetField field in sheet.SheetFields) {//validate all signatures before modifying any of the text fields.
				if(field.FieldType!= SheetFieldType.SigBox) {
					continue;
				}
				field.SigKey=Sheets.GetSignatureKey(sheet);
			}
			if(stmt!=null) {//Because most of the time statements do not display the Fill Sheet window, so there is no opportunity to select a combobox.
				SheetUtil.SetDefaultValueForComboBoxes(sheet);
			}
			SheetUtil.CalculateHeights(sheet,_dataSet,_stmt,_isPrinting,_printMargin.Top,_printMargin.Bottom,_medLab);
			_sheetList=new List<Sheet>();
			for(int i=0;i<copies;i++) {
				_sheetList.Add(sheet.Copy());
			}
			//later: add a check here for print preview.
#if DEBUG
			isPreviewMode=true;
#endif
			if(isPreviewMode) {
				FormPrintPreview printPreview;
				int pageCount=0;
				foreach(Sheet s in _sheetList) {
					pageCount+=Sheets.CalculatePageCount(s,_printMargin);
				}
				if(isPrintDocument) {
					printPreview=new FormPrintPreview(sit,pd,pageCount,sheet.PatNum,sheet.Description+" sheet from "+sheet.DateTimeSheet.ToShortDateString()+" printed");
					printPreview.ShowDialog();
				}
			}
			else {
				try {
					pd.DefaultPageSettings.Margins=new Margins(0,0,0,0);
					if(isPrintDocument) {//Only show the printer prompt if we're actually going to print the document.
						if(!PrinterL.SetPrinter(pd,sit,sheet.PatNum,sheet.Description+" sheet from "+sheet.DateTimeSheet.ToShortDateString()+" printed")) {
							return null;
						}
						pd.Print();
					}
				}
				catch(Exception ex) {
					throw ex;
				}
			}
			_isPrinting=false;
			GC.Collect();//We are done with printing so we can forcefully clean up all the objects and controls that were used in printing.
			return pd;
		}

		///<summary>This gets called for every page to be printed when sending to a printer.  Will stop printing when e.HasMorePages==false.  See also CreatePdfPage.</summary>
		private static void pd_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Graphics g=e.Graphics;
			g.SmoothingMode=SmoothingMode.HighQuality;
			g.InterpolationMode=InterpolationMode.HighQualityBicubic;//Necessary for very large images that need to be scaled down.
			Sheet sheet=_sheetList[_sheetsPrinted];
			Sheets.SetPageMargin(sheet,_printMargin);
			if(PagesPrinted==0 && sheet.SheetType==SheetTypeEnum.ERA) {
				idxPreClaimPaidPrinted=0;
				yPosPrevious=0;
			}
			try {
				pd_DrawFieldsHelper(sheet,g,null);
			}
			catch(OutOfMemoryException ex) {
				//Cancel the print job because there is a static image on this sheet which is to big for the printer to handle.
				MessageBox.Show(ex.Message);//Custom message that is already translated.
				e.Cancel=true;
				return;
			}
			drawHeader(sheet,g,null);
			drawFooter(sheet,g,null);
			#if DEBUG
				if(_printCalibration) {
					drawCalibration(sheet,g,e,null,null);
				}
			#endif
			g.Dispose();
			g=null;
			#region Set variables for next page to be printed
			_yPosPrint+=sheet.HeightPage-_printMargin.Bottom-_printMargin.Top;//move _yPosPrint down equal to the amount of printable area per page.
			_pagesPrinted++;
			if(_pagesPrinted<Sheets.CalculatePageCount(sheet,_printMargin)) {
				e.HasMorePages=true;
			}
			else {//we are printing the last page of the current sheet.
				_yPosPrint=0;
				_pagesPrinted=0;
				_sheetsPrinted++;
				if(_sheetsPrinted<_sheetList.Count){
					e.HasMorePages=true;
				}
				else{
					e.HasMorePages=false;
					_sheetsPrinted=0;
				}
			}
			#endregion
		}

		///<summary>Surround with try/catch.
		///Set parentSheet when working with a sheet withing a sheet (ERA).</summary>
		private static void pd_DrawFieldsHelper(Sheet sheet,Graphics g,XGraphics gx,Sheet parentSheet=null) {
			//Begin drawing.
			foreach(SheetField field in sheet.SheetFields) {
				if(parentSheet!=null && !fieldOnCurPageHelper(field,parentSheet,_printMargin,_yPosPrint)) {
					continue;
				}
				else if(parentSheet==null && !fieldOnCurPageHelper(field,sheet,_printMargin,_yPosPrint)) { 
					continue; 
				}
				switch(field.FieldType) {
					case SheetFieldType.Image:
					case SheetFieldType.PatImage:
						drawFieldImage(field,g,gx);
						break;
					case SheetFieldType.Drawing:
						drawFieldDrawing(field,g,gx);
						break;
					case SheetFieldType.Rectangle:
						drawFieldRectangle(field,g,gx);
						break;
					case SheetFieldType.Line:
						drawFieldLine(field,g,gx);
						break;
					case SheetFieldType.Special:
						drawFieldSpecial(sheet,field,g,gx);
						break;
					case SheetFieldType.Grid:
						drawFieldGrid(field,sheet,g,gx,_dataSet,_stmt,_medLab,true);
						break;
					case SheetFieldType.InputField:
					case SheetFieldType.OutputText:
					case SheetFieldType.StaticText:
						drawFieldText(field,sheet,g,gx);
						break;
					case SheetFieldType.CheckBox:
						drawFieldCheckBox(field,g,gx);
						break;
					case SheetFieldType.ComboBox:
						drawFieldComboBox(field,sheet,g,gx);
						break;
					case SheetFieldType.ScreenChart:
						drawFieldScreenChart(field,sheet,g,gx);
						break;
					case SheetFieldType.SigBox:
						drawFieldSigBox(field,sheet,g,gx);
						break;
					default:
						//Parameter or possibly new field type.
						break;
				}
			}//end foreach SheetField
		}

		private static bool fieldOnCurPageHelper(SheetField field,Sheet sheet,Margins _printMargin,int _yPosPrint) {
			//Even though _printMargins and _yPosPrint are available in this context they are passed in so for future compatibility with webforms.
			if(field.YPos>(_yPosPrint+sheet.HeightPage)){
				return false;//field is entirely on one of the next pages.
			}
			if(field.Bounds.Bottom<_yPosPrint && _pagesPrinted>0) {
				return false;//field is entirely on one of the previous pages. Unless we are on the first page, then it is in the top margin.
			}
			return true;//field is all or partially on current page.
		}

		#region Drawing Helpers. One for almost every field type. =====================================================================================

		///<summary>Draws the image to the graphics object passed in.  Can throw an OutOfMemoryException when printing that will have a message that should be displayed and the print job should be cancelled.</summary>
		public static void drawFieldImage(SheetField field,Graphics g,XGraphics gx,Bitmap image=null) {
			Bitmap bmpOriginal=null;
			ImageFormat bmpOriginalFormat=null;
			string filePathAndName="";
			if(image!=null) {
				bmpOriginal=image;
				filePathAndName="image Parameter";
			}
			else {
				#region Get the path for the image
				switch(field.FieldType) {
					case SheetFieldType.Image:
						filePathAndName=ODFileUtils.CombinePaths(SheetUtil.GetImagePath(),field.FieldName);
						break;
					case SheetFieldType.PatImage:
						if(field.FieldValue=="") {
							//There is no document object to use for display, but there may be a baked in image and that situation is dealt with below.
							filePathAndName="";
							break;
						}
						Document patDoc=Documents.GetByNum(PIn.Long(field.FieldValue));
						List<string> paths=Documents.GetPaths(new List<long> { patDoc.DocNum },ImageStore.GetPreferredAtoZpath());
						if(paths.Count < 1) {//No path was found so we cannot draw the image.
							return;
						}
						filePathAndName=paths[0];
						break;
					default:
						//not an image field
						return;
				}
				#endregion
				#region Load the image into bmpOriginal
				if(field.FieldName=="Patient Info.gif") {
					bmpOriginal=OpenDentBusiness.Properties.Resources.Patient_Info;
					bmpOriginalFormat=ImageFormat.Gif;
				}
				else if(CloudStorage.IsCloudStorage) {
					FormProgress FormP=new FormProgress();
					FormP.DisplayText=Lan.g(CloudStorage.LanThis,"Downloading...");
					FormP.NumberFormat="F";
					FormP.NumberMultiplication=1;
					FormP.MaxVal=100;//Doesn't matter what this value is as long as it is greater than 0
					FormP.TickMS=1000;
					OpenDentalCloud.Core.TaskStateDownload state=CloudStorage.DownloadAsync(SheetUtil.GetImagePath(),field.FieldName,
					new OpenDentalCloud.ProgressHandler(FormP.OnProgress));
					if(FormP.ShowDialog()==DialogResult.Cancel) {
						state.DoCancel=true;
						return;
					}
					if(state==null || state.FileContent==null) {
						return;//Unable to download the image
					}
					else {
						using(MemoryStream stream=new MemoryStream(state.FileContent)) {
							try {
								bmpOriginal=new Bitmap(Image.FromStream(stream));
								bmpOriginalFormat=ImageFormat.Bmp;
							}
							catch(Exception ex) {
								ex.DoNothing();
								return;//If the image is not an actual image file, leave the image field blank.
							}
						}
					}
				}
				else if(File.Exists(filePathAndName)) {//Local AtoZ
					try {
						bmpOriginal=new Bitmap(filePathAndName);
						bmpOriginalFormat=bmpOriginal.RawFormat;
					}
					catch {
						return;//If the image is not an actual image file, leave the image field blank.
					}
				}
				else {
					return;
				}
				#endregion
			}
			#region Calculate the image ratio and location, set values for imgDrawWidth and imgDrawHeight
			//inscribe image in field while maintaining aspect ratio.
			float imgRatio=(float)bmpOriginal.Width/(float)bmpOriginal.Height;
			float fieldRatio=(float)field.Width/(float)field.Height;
			float imgDrawHeight=field.Height;//drawn size of image
			float imgDrawWidth=field.Width;//drawn size of image
			int adjustY=0;//added to YPos
			int adjustX=0;//added to XPos
			//For patient images, we need to make sure the images will fit and can maintain aspect ratio.
			if(field.FieldType==SheetFieldType.PatImage && imgRatio>fieldRatio) {//image is too wide
				//X pos and width of field remain unchanged
				//Y pos and height must change
				imgDrawHeight=(float)bmpOriginal.Height*((float)field.Width/(float)bmpOriginal.Width);//img.Height*(width based scale) This also handles images that are too small.
				adjustY=(int)((field.Height-imgDrawHeight)/2f);//adjustY= half of the unused vertical field space
			}
			else if(field.FieldType==SheetFieldType.PatImage && imgRatio<fieldRatio) {//image is too tall
				//X pos and width must change
				//Y pos and height remain unchanged
				imgDrawWidth=(float)bmpOriginal.Width*((float)field.Height/(float)bmpOriginal.Height);//img.Height*(width based scale) This also handles images that are too small.
				adjustX=(int)((field.Width-imgDrawWidth)/2f);//adjustY= half of the unused horizontal field space
			}
			else {//image ratio == field ratio
				//do nothing
			}
			#endregion
			//We used to scale down bmpOriginal here to avoid memory exceptions.
			//Doing so was causing significant quality loss when printing or creating pdfs with very large images.
			if(gx==null) {
				try {
					//Always use the original BMP so that very large images can be scaled by the graphics class thus keeping a high quality image by using interpolation.
					g.DrawImage(bmpOriginal,
						new Rectangle(field.XPos+adjustX,field.YPos+adjustY-_yPosPrint,(int)imgDrawWidth,(int)imgDrawHeight),
						new Rectangle(0,0,bmpOriginal.Width,bmpOriginal.Height),
						GraphicsUnit.Pixel);
				}
				catch(OutOfMemoryException) {
					throw new OutOfMemoryException(Lan.g("Sheets","A static image on this sheet is too high in quality and cannot be printed.")+"\r\n"
						+Lan.g("Sheets","Try printing to a different printer or lower the quality of the static image")+":\r\n"
						+filePathAndName);
				}
			}
			else {
				MemoryStream ms=null;
				//For some reason PdfSharp's XImage cannot handle TIFF images.
				if(filePathAndName.ToLower().EndsWith(".tif") || filePathAndName.ToLower().EndsWith(".tiff")) {
					//Trick PdfSharp when we get a TIFF image into thinking it is a different image type.
					//Saving to BMP format will sometimes increase the file size dramatically.  E.g. an 11MB JPG turned into a 240MB BMP.
					//Instead of using BMP, we will use JPG which should have little to no quality loss and should be more compressed than BMP.
					ms=new MemoryStream();
					bmpOriginal.Save(ms,ImageFormat.Jpeg);
					bmpOriginal.Dispose();
					bmpOriginal=new Bitmap(ms);
				}
				// 07/24/2015 Task created by Brian for a customer stated that when creating a PDF of a sheet that is generated using an image as a background 
				// the PDF will sometimes distort the image beyond recognition.  This didn't happen in 14.3 and began happening in 15.1.  In versions 14.3 and 
				// earlier this section of code would resize the image gotten from the specified file and put it into a new Bitmap object.  It was discovered 
				// that the act of using the Bitmap created directly from the file would lead to the garbled image, so we decided to put the image Bitmap into
				// a new Bitmap without resizing which fixes the issue without the quality loss that was present when we resized in 14.3.
				//Bitmap bmpDraw=new Bitmap(bmpOriginal);
				//XImage xI=XImage.FromGdiPlusImage(bmpDraw);
				// 10/07/2015 We cannot do the above fix for the distorted images because it forcefully changes all images to be BMPs.  
				// Instead, we need to resave the image but in it's native "RawFormat" in order to preserve it's original format.
				// Forcefully making all images on PDFs use BMP causes the PDFs to bloat in size significantly (harder to email).  E.g. 2MB turns into 22MBs.
				Bitmap bmpQualityCopy=new Bitmap(bmpOriginal);//Convert to a BMP to preserve the quality of the original image.
				ms=new MemoryStream();
				bmpQualityCopy.Save(ms,bmpOriginalFormat);//Convert the BMP back to the original image format which will typically cut down the image size.
				bmpOriginal.Dispose();
				bmpOriginal=new Bitmap(ms);//Override the original image with the re-saved imaged in the memory stream (fixes corrupt images).
				XImage xI=XImage.FromGdiPlusImage(bmpOriginal);
				gx.DrawImage(xI,p(field.XPos+adjustX),p(field.YPos-_yPosPrint+adjustY),p(imgDrawWidth),p(imgDrawHeight));
				xI.Dispose();
				xI=null;
				if(ms!=null) {
					ms.Dispose();
					ms=null;
				}
				if(bmpQualityCopy!=null) {
					bmpQualityCopy.Dispose();
					bmpQualityCopy=null;
				}
			}
			if(bmpOriginal!=null) {
				bmpOriginal.Dispose();
				bmpOriginal=null;
			}
		}

		public static void drawFieldDrawing(SheetField field,Graphics g,XGraphics gx) {
			if(gx==null) {
				Pen pen=new Pen(Brushes.Black,2f);
				List<Point> points=new List<Point>();
				string[] pairs=field.FieldValue.Split(new string[] { ";" },StringSplitOptions.RemoveEmptyEntries);
				foreach(string p in pairs) {
					points.Add(new Point(PIn.Int(p.Split(',')[0]),PIn.Int(p.Split(',')[1])));
				}
				for(int i=1;i<points.Count;i++) {
					g.DrawLine(pen,points[i-1].X,points[i-1].Y-_yPosPrint,points[i].X,points[i].Y-_yPosPrint);
				}
				pen.Dispose();
				pen=null;
			}
			else {
				XPen pen=new XPen(XColors.Black,p(2));
				List<Point> points=new List<Point>();
				string[] pairs=field.FieldValue.Split(new string[] { ";" },StringSplitOptions.RemoveEmptyEntries);
				foreach(string p2 in pairs) {
					points.Add(new Point(PIn.Int(p2.Split(',')[0]),PIn.Int(p2.Split(',')[1])));
				}
				for(int i=1;i<points.Count;i++) {
					gx.DrawLine(pen,p(points[i-1].X),p(points[i-1].Y-_yPosPrint),p(points[i].X),p(points[i].Y-_yPosPrint));
				}
				pen=null;
			}
		}

		public static void drawFieldRectangle(SheetField field,Graphics g,XGraphics gx) {
			if(gx==null) {
				g.DrawRectangle(Pens.Black,field.XPos,field.YPos-_yPosPrint,field.Width,field.Height);
			}
			else {
				gx.DrawRectangle(XPens.Black,p(field.XPos),p(field.YPos-_yPosPrint),p(field.Width),p(field.Height));
			}
		}

		public static void drawFieldLine(SheetField field,Graphics g,XGraphics gx) {
			if(gx==null) {
				g.DrawLine((field.ItemColor.ToArgb()==Color.FromArgb(0).ToArgb() ? Pens.Black : new Pen(field.ItemColor,1)),
					field.XPos,field.YPos-_yPosPrint,
					field.XPos+field.Width,
					field.YPos-_yPosPrint+field.Height);
			}
			else {
				gx.DrawLine((field.ItemColor.ToArgb()==Color.FromArgb(0).ToArgb() ? XPens.Black : new XPen(field.ItemColor,1)),
					p(field.XPos),p(field.YPos-_yPosPrint),
					p(field.XPos+field.Width),
					p(field.YPos-_yPosPrint+field.Height));
			}
		}

		public static void drawFieldSpecial(Sheet sheet,SheetField field,Graphics g, XGraphics gx) {
			switch(field.FieldName) {
				case "toothChart":
					Image toothChart=(Image)SheetParameter.GetParamByName(sheet.Parameters,"toothChartImg").ParamValue;
					//Image toothChart=GetToothChartHelper(sheet.PatNum,treatPlan,true);
					Rectangle boundingBox=new Rectangle(field.XPos,field.YPos,field.Width,field.Height);
					float widthFactor=(float)boundingBox.Width/(float)toothChart.Width;
					float heightFactor=(float)boundingBox.Height/(float)toothChart.Height;
					int x,y,width,height;
					if(widthFactor<heightFactor) {
						//use width factor
						//img width will equal box width
						//offset height.
						x=field.XPos;
						y=(field.YPos-_yPosPrint)+(field.Height-(int)(toothChart.Height*widthFactor))/2;
						height=(int)(toothChart.Height*widthFactor);
						width=field.Width+1; //+1 to include the pixels
					}
					else {
						//use height factor
						//img height will equal box height
						//offset width
						x=field.XPos+(field.Width-(int)(toothChart.Width*heightFactor))/2;
						y=field.YPos-_yPosPrint;
						height=field.Height+1;
						width=(int)(toothChart.Width*heightFactor);
					}
					if(gx==null) {
						g.DrawImage(toothChart,new Rectangle(x,y,width,height));
						//g.DrawRectangle(Pens.LightGray,x,y,width,height); //outline tooth grid so user can see how much wasted space there is.
					}
					else {
						gx.DrawImage(XImage.FromGdiPlusImage(toothChart),new Rectangle((int)p(x),(int)p(y),(int)p(width),(int)p(height)));
					}
					break;
				case "toothChartLegend":
						List<Def> listDefs=Defs.GetDefsForCategory(DefCat.ChartGraphicColors,true);
						using(Brush brushEx=new SolidBrush(listDefs[3].ItemColor))
						using(Brush brushEc=new SolidBrush(listDefs[2].ItemColor))
						using(Brush brushCo=new SolidBrush(listDefs[1].ItemColor))
						using(Brush brushRo=new SolidBrush(listDefs[4].ItemColor))
						using(Brush brushTp=new SolidBrush(listDefs[0].ItemColor))
						using(Font bodyFont=new Font("Arial",9f,FontStyle.Regular,GraphicsUnit.Point))
						if(gx==null) {
							float yPos=field.YPos-_yPosPrint;
							//Always centered on page.
							float xPos=0.5f*(sheet.Width-
								                (TextRenderer.MeasureText(Lan.g("ContrTreat","Existing"),bodyFont).Width
								                +TextRenderer.MeasureText(Lan.g("ContrTreat","Complete"),bodyFont).Width
								                +TextRenderer.MeasureText(Lan.g("ContrTreat","Referred Out"),bodyFont).Width
								                +TextRenderer.MeasureText(Lan.g("ContrTreat","Treatment Planned"),bodyFont).Width
								                +123)); //inter-field spacing
							g.FillRectangle(Brushes.White,new Rectangle((int)xPos,field.YPos-_yPosPrint,sheet.Width-2*(int)xPos+10,14)); //buffer the image for smooth drawing.
							//Existing
							g.FillRectangle(brushEx,xPos,yPos,14,14);
							g.DrawString(Lan.g("ContrTreat","Existing"),bodyFont,Brushes.Black,xPos+16,yPos);
							xPos+=TextRenderer.MeasureText(Lan.g("ContrTreat","Existing"),bodyFont).Width+23+16;
							//Complete/ExistingComplete
							g.FillRectangle(brushCo,xPos,yPos,7,14);
							g.FillRectangle(brushEc,xPos+7,yPos,7,14);
							g.DrawString(Lan.g("ContrTreat","Complete"),bodyFont,Brushes.Black,xPos+16,yPos);
							xPos+=TextRenderer.MeasureText(Lan.g("ContrTreat","Complete"),bodyFont).Width+23+16;
							//ReferredOut
							g.FillRectangle(brushRo,xPos,yPos,14,14);
							g.DrawString(Lan.g("ContrTreat","Referred Out"),bodyFont,Brushes.Black,xPos+16,yPos);
							xPos+=TextRenderer.MeasureText(Lan.g("ContrTreat","Referred Out"),bodyFont).Width+23+16;
							//TreatmentPlanned
							g.FillRectangle(brushTp,xPos,yPos,14,14);
							g.DrawString(Lan.g("ContrTreat","Treatment Planned"),bodyFont,Brushes.Black,xPos+16,yPos);
						}
						else {
							XFont bodyFontX=new XFont(bodyFont.SystemFontName,bodyFont.Size,XFontStyle.Regular);
							float yPos=field.YPos-_yPosPrint;
							//Always centered on page.
							float xPos=0.5f*(sheet.Width-
								                (TextRenderer.MeasureText(Lan.g("ContrTreat","Existing"),bodyFont).Width
								                +TextRenderer.MeasureText(Lan.g("ContrTreat","Complete"),bodyFont).Width
								                +TextRenderer.MeasureText(Lan.g("ContrTreat","Referred Out"),bodyFont).Width
								                +TextRenderer.MeasureText(Lan.g("ContrTreat","Treatment Planned"),bodyFont).Width
								                +123)); //inter-field spacing
							gx.DrawRectangle(XBrushes.White,new RectangleF((float)p(xPos),(float)p(field.YPos-_yPosPrint),(float)p(sheet.Width-2*xPos+10),(float)p(14))); //buffer the image for smooth drawing.
							//Existing
							gx.DrawRectangle(brushEx,p(xPos),p(yPos),p(14),p(14));
							GraphicsHelper.DrawStringX(gx,Lan.g("ContrTreat","Existing"),bodyFontX,XBrushes.Black,
								new RectangleF(xPos+16,yPos-1,TextRenderer.MeasureText(Lan.g("ContrTreat","Existing"),bodyFont).Width,14),HorizontalAlignment.Left);
							//gx.DrawString(Lan.g("ContrTreat","Existing"),bodyFontX,Brushes.Black,p(xPos+16),p(yPos));
							xPos+=TextRenderer.MeasureText(Lan.g("ContrTreat","Existing"),bodyFont).Width+23+16;
							//Complete/ExistingComplete
							gx.DrawRectangle(brushCo,p(xPos),p(yPos),p(7),p(14));
							gx.DrawRectangle(brushEc,p(xPos+7),p(yPos),p(7),p(14));
							GraphicsHelper.DrawStringX(gx,Lan.g("ContrTreat","Complete"),bodyFontX,XBrushes.Black,
								new RectangleF(xPos+16,yPos-1,TextRenderer.MeasureText(Lan.g("ContrTreat","Complete"),bodyFont).Width,14),HorizontalAlignment.Left);
							//gx.DrawString(Lan.g("ContrTreat","Complete"),bodyFontX,Brushes.Black,p(xPos+16),p(yPos));
							xPos+=TextRenderer.MeasureText(Lan.g("ContrTreat","Complete"),bodyFont).Width+23+16;
							//ReferredOut
							gx.DrawRectangle(brushRo,p(xPos),p(yPos),p(14),p(14));
							GraphicsHelper.DrawStringX(gx,Lan.g("ContrTreat","Referred Out"),bodyFontX,XBrushes.Black,
								new RectangleF(xPos+16,yPos-1,TextRenderer.MeasureText(Lan.g("ContrTreat","Referred Out"),bodyFont).Width,14),HorizontalAlignment.Left);
							//gx.DrawString(Lan.g("ContrTreat","Referred Out"),bodyFontX,Brushes.Black,p(xPos+16),p(yPos));
							xPos+=TextRenderer.MeasureText(Lan.g("ContrTreat","Referred Out"),bodyFont).Width+23+16;
							//TreatmentPlanned
							gx.DrawRectangle(brushTp,p(xPos),p(yPos),p(14),p(14));
							GraphicsHelper.DrawStringX(gx,Lan.g("ContrTreat","Treatment Planned"),bodyFontX,XBrushes.Black,
								new RectangleF(xPos+16,yPos-1,TextRenderer.MeasureText(Lan.g("ContrTreat","Treatment Planned"),bodyFont).Width,14),HorizontalAlignment.Left);
							//gx.DrawString(Lan.g("ContrTreat","Treatment Planned"),bodyFontX,Brushes.Black,p(xPos+16),p(yPos));
						}
					break;
				default:
					//do nothing
					break;
			}
		}

		public static Image GetToothChartHelper(long patNum,bool showCompleted,TreatPlan treatPlan=null,List<Procedure> listProceduresFilteredOverride=null) {
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.ChartGraphicColors);
			SparksToothChart.ToothChartWrapper toothChart=new SparksToothChart.ToothChartWrapper();
			toothChart.ColorBackground=listDefs[14].ItemColor;
			toothChart.ColorText=listDefs[15].ItemColor;
			toothChart.Size=new Size(500,370);
			toothChart.UseHardware=ComputerPrefs.LocalComputer.GraphicsUseHardware;
			toothChart.SetToothNumberingNomenclature((ToothNumberingNomenclature)PrefC.GetInt(PrefName.UseInternationalToothNumbers));
			toothChart.PreferredPixelFormatNumber=ComputerPrefs.LocalComputer.PreferredPixelFormatNum;
			toothChart.DeviceFormat=new SparksToothChart.ToothChartDirectX.DirectXDeviceFormat(ComputerPrefs.LocalComputer.DirectXFormat);
			toothChart.DrawMode=ComputerPrefs.LocalComputer.GraphicsSimple;
			ComputerPrefs.LocalComputer.PreferredPixelFormatNum=toothChart.PreferredPixelFormatNumber;
			ComputerPrefs.Update(ComputerPrefs.LocalComputer);
			toothChart.ResetTeeth();
			List<ToothInitial> toothInitialList=patNum==0?new List<ToothInitial>():ToothInitials.Refresh(patNum);
			//first, primary.  That way, you can still set a primary tooth missing afterwards.
			for(int i=0;i<toothInitialList.Count;i++) {
				if(toothInitialList[i].InitialType==ToothInitialType.Primary) {
					toothChart.SetPrimary(toothInitialList[i].ToothNum);
				}
			}
			for(int i=0;i<toothInitialList.Count;i++) {
				switch(toothInitialList[i].InitialType) {
					case ToothInitialType.Missing:
						toothChart.SetMissing(toothInitialList[i].ToothNum);
						break;
					case ToothInitialType.Hidden:
						toothChart.SetHidden(toothInitialList[i].ToothNum);
						break;
					case ToothInitialType.Rotate:
						toothChart.MoveTooth(toothInitialList[i].ToothNum,toothInitialList[i].Movement,0,0,0,0,0);
						break;
					case ToothInitialType.TipM:
						toothChart.MoveTooth(toothInitialList[i].ToothNum,0,toothInitialList[i].Movement,0,0,0,0);
						break;
					case ToothInitialType.TipB:
						toothChart.MoveTooth(toothInitialList[i].ToothNum,0,0,toothInitialList[i].Movement,0,0,0);
						break;
					case ToothInitialType.ShiftM:
						toothChart.MoveTooth(toothInitialList[i].ToothNum,0,0,0,toothInitialList[i].Movement,0,0);
						break;
					case ToothInitialType.ShiftO:
						toothChart.MoveTooth(toothInitialList[i].ToothNum,0,0,0,0,toothInitialList[i].Movement,0);
						break;
					case ToothInitialType.ShiftB:
						toothChart.MoveTooth(toothInitialList[i].ToothNum,0,0,0,0,0,toothInitialList[i].Movement);
						break;
					case ToothInitialType.Drawing:
						toothChart.AddDrawingSegment(toothInitialList[i].Copy());
						break;
				}
			}
			//We passed in a list of procs we want to see on the toothchart.  Use that.
			List<Procedure> listProceduresFiltered;
			if(listProceduresFilteredOverride!=null) {
				listProceduresFiltered=listProceduresFilteredOverride;
			}
			else {//Custom list of procedures was not passed in, go get all for the patient like we always have.
				List<Procedure> listProceduresAll=Procedures.Refresh(patNum);
				listProceduresFiltered=listProceduresAll.FindAll(x => new[] { ProcStat.R,ProcStat.Cn }.Contains(x.ProcStatus));//always show referred and conditions
				if(showCompleted) {
					listProceduresFiltered.AddRange(listProceduresAll.FindAll(x => new[] {ProcStat.C,ProcStat.EC,ProcStat.EO}.Contains(x.ProcStatus)));//show complete
				}
				if(treatPlan!=null) {
					foreach(ProcTP procTP in treatPlan.ListProcTPs) {//Add procs for TP.
						Procedure procDummy=listProceduresAll.FirstOrDefault(x => x.ProcNum==procTP.ProcNumOrig)??new Procedure();
						if(Tooth.IsValidEntry(procTP.ToothNumTP)) {
							procDummy.ToothNum=Tooth.FromInternat(procTP.ToothNumTP);
						}
						if(ProcedureCodes.GetProcCode(procTP.ProcCode).TreatArea==TreatmentArea.Surf) {
							procDummy.Surf=Tooth.SurfTidyFromDisplayToDb(procTP.Surf,procDummy.ToothNum);
						}
						else {
							procDummy.Surf=procTP.Surf;//for quad, arch, etc.
						}
						if(procDummy.ToothRange==null) {
							procDummy.ToothRange="";
						}
						procDummy.ProcStatus=ProcStat.TP;
						procDummy.CodeNum=ProcedureCodes.GetProcCode(procTP.ProcCode).CodeNum;
						listProceduresFiltered.Add(procDummy);
					}
				}
			}
			listProceduresFiltered.Sort(CompareProcListFiltered);
			//Draw tooth chart
			DrawProcsGraphics(listProceduresFiltered,toothChart,toothInitialList);
			toothChart.AutoFinish=true;
			Image retVal=toothChart.GetBitmap();
			toothChart.Dispose();
			return retVal;
		}

		#region toothChartHelpers. These are cut and pasted from various parts of ContrTreat and can probably be optimized greatly

		private static int CompareProcListFiltered(Procedure proc1,Procedure proc2) {
			if(proc1.ProcDate!=proc2.ProcDate) {
				return proc1.ProcDate.CompareTo(proc2.ProcDate);
			}
			return GetProcStatusIdx(proc1.ProcStatus).CompareTo(GetProcStatusIdx(proc2.ProcStatus));
		}

		///<summary>Returns index for sorting based on this order: Cn,TP,R,EO,EC,C,D</summary>
		private static int GetProcStatusIdx(ProcStat procStat) {
			switch(procStat) {
				case ProcStat.Cn:
					return 0;
				case ProcStat.TP:
					return 1;
				case ProcStat.R:
					return 2;
				case ProcStat.EO:
					return 3;
				case ProcStat.EC:
					return 4;
				case ProcStat.C:
					return 5;
				case ProcStat.D:
					return 6;
			}
			return 0;
		}

		private static void DrawProcsGraphics(List<Procedure> procList,SparksToothChart.ToothChartWrapper toothChart,List<ToothInitial> toothInitialList) {
			Procedure proc;
			string[] teeth;
			System.Drawing.Color cLight=System.Drawing.Color.White;
			System.Drawing.Color cDark=System.Drawing.Color.White;
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.ChartGraphicColors,true);
			for(int i=0;i<procList.Count;i++) {
				proc=procList[i];
				//if(proc.ProcStatus!=procStat) {
				//  continue;
				//}
				if(proc.HideGraphics) {
					continue;
				}
				if(ProcedureCodes.GetProcCode(proc.CodeNum).PaintType==ToothPaintingType.Extraction && (
					proc.ProcStatus==ProcStat.C
					|| proc.ProcStatus==ProcStat.EC
					|| proc.ProcStatus==ProcStat.EO
					)) {
					continue;//prevents the red X. Missing teeth already handled.
				}
				if(ProcedureCodes.GetProcCode(proc.CodeNum).GraphicColor==System.Drawing.Color.FromArgb(0)) {
					switch(proc.ProcStatus) {
						case ProcStat.C:
							cDark=listDefs[1].ItemColor;
							cLight=listDefs[6].ItemColor;
							break;
						case ProcStat.TP:
							cDark=listDefs[0].ItemColor;
							cLight=listDefs[5].ItemColor;
							break;
						case ProcStat.EC:
							cDark=listDefs[2].ItemColor;
							cLight=listDefs[7].ItemColor;
							break;
						case ProcStat.EO:
							cDark=listDefs[3].ItemColor;
							cLight=listDefs[8].ItemColor;
							break;
						case ProcStat.R:
							cDark=listDefs[4].ItemColor;
							cLight=listDefs[9].ItemColor;
							break;
						case ProcStat.Cn:
							cDark=listDefs[16].ItemColor;
							cLight=listDefs[17].ItemColor;
							break;
					}
				}
				else {
					cDark=ProcedureCodes.GetProcCode(proc.CodeNum).GraphicColor;
					cLight=ProcedureCodes.GetProcCode(proc.CodeNum).GraphicColor;
				}
				switch(ProcedureCodes.GetProcCode(proc.CodeNum).PaintType) {
					case ToothPaintingType.BridgeDark:
						if(ToothInitials.ToothIsMissingOrHidden(toothInitialList,proc.ToothNum)) {
							toothChart.SetPontic(proc.ToothNum,cDark);
						}
						else {
							toothChart.SetCrown(proc.ToothNum,cDark);
						}
						break;
					case ToothPaintingType.BridgeLight:
						if(ToothInitials.ToothIsMissingOrHidden(toothInitialList,proc.ToothNum)) {
							toothChart.SetPontic(proc.ToothNum,cLight);
						}
						else {
							toothChart.SetCrown(proc.ToothNum,cLight);
						}
						break;
					case ToothPaintingType.CrownDark:
						toothChart.SetCrown(proc.ToothNum,cDark);
						break;
					case ToothPaintingType.CrownLight:
						toothChart.SetCrown(proc.ToothNum,cLight);
						break;
					case ToothPaintingType.DentureDark:
						if(proc.Surf=="U") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+2).ToString();
							}
						}
						else if(proc.Surf=="L") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+18).ToString();
							}
						}
						else {
							teeth=proc.ToothRange.Split(new char[] { ',' });
						}
						for(int t=0;t<teeth.Length;t++) {
							if(ToothInitials.ToothIsMissingOrHidden(toothInitialList,teeth[t])) {
								toothChart.SetPontic(teeth[t],cDark);
							}
							else {
								toothChart.SetCrown(teeth[t],cDark);
							}
						}
						break;
					case ToothPaintingType.DentureLight:
						if(proc.Surf=="U") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+2).ToString();
							}
						}
						else if(proc.Surf=="L") {
							teeth=new string[14];
							for(int t=0;t<14;t++) {
								teeth[t]=(t+18).ToString();
							}
						}
						else {
							teeth=proc.ToothRange.Split(new char[] { ',' });
						}
						for(int t=0;t<teeth.Length;t++) {
							if(ToothInitials.ToothIsMissingOrHidden(toothInitialList,teeth[t])) {
								toothChart.SetPontic(teeth[t],cLight);
							}
							else {
								toothChart.SetCrown(teeth[t],cLight);
							}
						}
						break;
					case ToothPaintingType.Extraction:
						toothChart.SetBigX(proc.ToothNum,cDark);
						break;
					case ToothPaintingType.FillingDark:
						toothChart.SetSurfaceColors(proc.ToothNum,proc.Surf,cDark);
						break;
					case ToothPaintingType.FillingLight:
						toothChart.SetSurfaceColors(proc.ToothNum,proc.Surf,cLight);
						break;
					case ToothPaintingType.Implant:
						toothChart.SetImplant(proc.ToothNum,cDark);
						break;
					case ToothPaintingType.PostBU:
						toothChart.SetBU(proc.ToothNum,cDark);
						break;
					case ToothPaintingType.RCT:
						toothChart.SetRCT(proc.ToothNum,cDark);
						break;
					case ToothPaintingType.Sealant:
						toothChart.SetSealant(proc.ToothNum,cDark);
						break;
					case ToothPaintingType.Veneer:
						toothChart.SetVeneer(proc.ToothNum,cLight);
						break;
					case ToothPaintingType.Watch:
						toothChart.SetWatch(proc.ToothNum,cDark);
						break;
				}
			}
		}
		#endregion

		///<summary>If drawing grids for a statement, use the polymorphism that takes a DataSet otherwise this method will make another call to the db.</summary>
		public static void drawFieldGrid(SheetField field,Sheet sheet,Graphics g,XGraphics gx,Statement stmt=null,MedLab medLab=null) {
			DataSet dataSet=null;
			if(sheet.SheetType==SheetTypeEnum.Statement && stmt!=null) {
				//This should never get hit.  This line of code is here just in case I forgot to update a random spot in our code.
				//Worst case scenario we will end up calling the database a few extra times for the same data set.
				//It use to call this method many, many times so anything is an improvement at this point.
				dataSet=AccountModules.GetAccount(stmt.PatNum,stmt);
			}
			drawFieldGrid(field,sheet,g,gx,dataSet,stmt,medLab);
		}

		///<Summary>DataSet should be prefilled with AccountModules.GetAccount() before calling this method if printing a statement.</Summary>
		public static void drawFieldGrid(SheetField field,Sheet sheet,Graphics g,XGraphics gx,DataSet dataSet,Statement stmt,MedLab medLab,
			bool isPrinting=false,Patient pat=null,Patient patGuar=null) 
		{
			if(sheet.SheetType!=SheetTypeEnum.ERA) {
				drawFieldGridHelper(field,sheet,g,gx,dataSet,stmt,medLab,isPrinting,pat: pat,patGuar: patGuar);
				return;
			}
			//ERA sheets can only have grid field such that field.FieldName=="EraClaimsPaid".
			SheetParameter param=SheetParameter.GetParamByName(sheet.Parameters,"IsSingleClaimPaid");
			bool isSingleClaim=(param.ParamValue==null)?false:true;//param is only set when true
			//This logic mimics SheetUtil.CalculateHeights(...)
			bool isOneClaimPerPage=PrefC.GetBool(PrefName.EraPrintOneClaimPerPage);
			if(isSingleClaim) {
				//When printing a single claim we do not want to print the claim on the next page like we would when printing every claim.
				isOneClaimPerPage=false;
			}
			if(isOneClaimPerPage && _pagesPrinted==0) {
				//First page is the ERA fields page.  Continue to next page unless this is a single claim being printed.
				return;
			}
			SheetDef gridHeaderSheetDef=SheetDefs.GetInternalOrCustom(SheetInternalType.ERAGridHeader);
			X835 era=(X835)SheetParameter.GetParamByName(sheet.Parameters,"ERA").ParamValue;//Required param
			DataTable tablePaidProcs=SheetUtil.GetDataTableForGridType(sheet,dataSet,field.FieldName,stmt,medLab);
			DataTable tableRemarks=SheetUtil.GetDataTableForGridType(sheet,dataSet,"EraClaimsPaidProcRemarks",stmt,medLab);
			Sheet gridHeaderSheet=SheetUtil.CreateSheet(gridHeaderSheetDef);
			int yPosStep=yPosPrevious;
			int printableHeight=sheet.Height-_printMargin.Top-_printMargin.Bottom;
			int pageMinY=(_pagesPrinted*printableHeight);
			int pageMaxY=((_pagesPrinted+1)*printableHeight);
			for(int i=idxPreClaimPaidPrinted; i<era.ListClaimsPaid.Count; i++) {
				Hx835_Claim claim=era.ListClaimsPaid[i];
				//Work with copy so that original position values do not change per itteration.
				SheetField fieldCopy=field.Copy();
				Sheet gridHeaderSheetCopy=gridHeaderSheet.Copy();
				if(isOneClaimPerPage) {
					fieldCopy.YPos=_printMargin.Top+1;//Set field to top of page.  Plus 1 for padding.
					yPosStep+=printableHeight;
				}
				fieldCopy.YPos+=yPosStep;
				if(fieldCopy.YPos>=pageMaxY//Field is on the next page.
					|| fieldCopy.YPos<=pageMaxY && fieldCopy.YPos+gridHeaderSheet.Height>pageMaxY)//Header is split between 2 pages.  Let Next page handle.
				{
					break;//Done with this page.
				}
				//At this point claim has not been drawn on any previous page.
				if(fieldCopy.YPos<=pageMinY) { //Claim is not drawn and YPos is above the current page, so we must adjust it.
					yPosStep+=((pageMinY+_printMargin.Top+1)-fieldCopy.YPos);//The difference of what we are about to add the YPos.
					fieldCopy.YPos=pageMinY+_printMargin.Top+1;//Set to top of page, plus 1 for padding.
				}
				gridHeaderSheetCopy.SheetFields.ForEach(x => {
					//Make header sheet possitions relative to parent sheet and grid field location.
					x.XPos+=fieldCopy.XPos;
					x.YPos+=fieldCopy.YPos;
				});
				SheetParameter.GetParamByName(gridHeaderSheetCopy.Parameters,"EraClaimPaid").ParamValue=claim;//Required param
				SheetParameter.GetParamByName(gridHeaderSheetCopy.Parameters,"ClaimIndexNum").ParamValue=(isSingleClaim?0:era.ListClaimsPaid.IndexOf(claim)+1);//0 index so +1
				SheetFiller.FillFields(gridHeaderSheetCopy);
				pd_DrawFieldsHelper(gridHeaderSheetCopy,g,gx,sheet);
				fieldCopy.YPos+=gridHeaderSheetCopy.Height;
				int procGridHeight=drawFieldGridHelper(fieldCopy,sheet,g,gx,dataSet,stmt,medLab,isPrinting,claim.ClpSegmentIndex,tablePaidProcs);
				SheetField fieldRemarks=fieldCopy.Copy();
				fieldRemarks.FieldName="EraClaimsPaidProcRemarks";
				fieldRemarks.YPos=fieldCopy.YPos+procGridHeight;
				int remarkGridHeight=drawFieldGridHelper(fieldRemarks,sheet,g,gx,dataSet,stmt,medLab,isPrinting,claim.ClpSegmentIndex,tableRemarks);
				int subFieldHeight=gridHeaderSheetCopy.Height+procGridHeight+remarkGridHeight;
				if(isOneClaimPerPage) {
					if(subFieldHeight>printableHeight) {//Grid wraps onto a second page.
						yPosStep+=printableHeight*(subFieldHeight/printableHeight);//Truncation intended.  Add extra pages when needed.
					}
				}
				else {
					subFieldHeight+=25;
					yPosStep+=subFieldHeight;
				}
				idxPreClaimPaidPrinted++;
				yPosPrevious=yPosStep;
			}
		}


		public static int drawFieldGridHelper(SheetField field,Sheet sheet,Graphics g,XGraphics gx,DataSet dataSet,Statement stmt,MedLab medLab,
			bool isPrinting=false,long eraClaimSegmentIdx=-1,DataTable table=null,Patient pat=null,Patient patGuar=null) 
		{
			if(stmt!=null && stmt.StatementType==StmtType.LimitedStatement && field.FieldName.StartsWith("StatementAging")) {
				return 0;
			}
			Sheets.SetPageMargin(sheet,_printMargin);
			UI.ODGrid odGrid=new UI.ODGrid();//Only used for measurements, also contains printing/drawing logic.
			odGrid.FontForSheets=new Font(field.FontName,field.FontSize,field.FontIsBold?FontStyle.Bold:FontStyle.Regular);
			int _yAdjCurRow=0;//used to adjust for Titles, Headers, Rows, and footers (all considered part of the same row).
			List<DisplayField> Columns=SheetUtil.GetGridColumnsAvailable(field.FieldName);
			if(table==null) {
				table=SheetUtil.GetDataTableForGridType(sheet,dataSet,field.FieldName,stmt,medLab,patGuar: patGuar);
			}
			if(field.FieldName=="TreatPlanMain") {
				TreatPlanType tpType=(TreatPlanType)PIn.Int(table.Rows[0]["paramTreatPlanType"].ToString());
				switch(tpType) {
					case TreatPlanType.Discount:
						Columns.RemoveAll(x => x.InternalName=="Pri Ins" || x.InternalName=="Sec Ins");
						break;
					case TreatPlanType.Insurance:
						Columns.RemoveAll(x => x.InternalName=="DPlan");
						break;
				}
			}
			filterColumnsHelper(sheet,field,Columns);
			odGrid.Width=Columns.Sum(x => x.ColumnWidth);//sum of empty list will return 0
			odGrid.Height=field.Height;
			odGrid.HideScrollBars=true;
			odGrid.YPosField=field.YPos;
			odGrid.Title=field.FieldName;
			if(stmt!=null) {
				odGrid.Title+=((stmt.Intermingled || stmt.SinglePatient)?".Intermingled":".NotIntermingled");//Important for calculating heights.
			}
			odGrid.TopMargin=_printMargin.Top;
			odGrid.BottomMargin=_printMargin.Bottom;
			odGrid.PageHeight=sheet.HeightPage;
			#region  Fill Grid, Set Text Alignment
			odGrid.BeginUpdate();
			odGrid.Columns.Clear();
			ODGridColumn col;
			foreach(DisplayField colCur in Columns) {
				if(string.IsNullOrEmpty(colCur.Description)) {
					col=new ODGridColumn(colCur.InternalName,colCur.ColumnWidth);
				}
				else {
					col=new ODGridColumn(colCur.Description,colCur.ColumnWidth);
				}
				switch(field.FieldName+"."+colCur.InternalName) {//Unusual switch statement to differentiate similar column names in different grids.
					case "StatementMain.charges":
					case "StatementMain.credits":
					case "StatementMain.balance":
					case "StatementPayPlan.charges":
					case "StatementPayPlan.credits":
					case "StatementPayPlan.balance":
					case "StatementInvoicePayment.amt":
					case "TreatPlanMain.Fee":
					case "TreatPlanMain.Pri Ins":
					case "TreatPlanMain.Sec Ins":
					case "TreatPlanMain.DPlan":
					case "TreatPlanMain.Discount":
					case "TreatPlanMain.Pat":
					case "TreatPlanBenefitsFamily.Primary":
					case "TreatPlanBenefitsFamily.Secondary":
					case "TreatPlanBenefitsIndividual.Primary":
					case "TreatPlanBenefitsIndividual.Secondary":
					case "PayPlanMain.Principal":
					case "PayPlanMain.Interest":
					case "PayPlanMain.due":
					case "PayPlanMain.payment":
					case "PayPlanMain.balance":
					case "PayPlanMain.Adjustment":
					case "EraClaimsPaid.FeeBilled":
					case "EraClaimsPaid.PatResp":
					case "EraClaimsPaid.Contractual":
					case "EraClaimsPaid.PayorReduct":
					case "EraClaimsPaid.OtherAdjust":
					case "EraClaimsPaid.InsPaid":
					case "EraClaimsPaid.RemarkCodes":
						col.TextAlign=HorizontalAlignment.Right;
						break;
					case "StatementAging.Age00to30":
					case "StatementAging.Age31to60":
					case "StatementAging.Age61to90":
					case "StatementAging.Age90plus":
					case "StatementAging.AcctTotal":
						if(sheet.SheetType==SheetTypeEnum.Statement && stmt!=null && stmt.SuperFamily!=0) {
							col.TextAlign=HorizontalAlignment.Right;
						}
						else {
							col.TextAlign=HorizontalAlignment.Center;
						}
						break;
					case "StatementEnclosed.AmountDue":
					case "StatementEnclosed.DateDue":
					case "EraClaimsPaid.ProcCode":
					case "EraClaimsPaidProcRemarks.RemarkCode":
						col.TextAlign=HorizontalAlignment.Center;
						break;
					default:
						col.TextAlign=HorizontalAlignment.Left;
						break;
				}
				odGrid.Columns.Add(col);
			}
			ODGridRow row;
			foreach(DataRow rowCur in table.Rows) {
				if(eraClaimSegmentIdx!=-1 && rowCur["ClpSegmentIndex"].ToString()!=eraClaimSegmentIdx.ToString()) {
					continue;
				}
				row=new ODGridRow();
				foreach(DisplayField colCur in Columns) {
					if(colCur.InternalName=="DPlan") {
						row.Cells.Add(rowCur["Pri Ins"].ToString());
					}
					else { 
						row.Cells.Add(rowCur[colCur.InternalName].ToString());//Selectively fill columns from the dataTable into the odGrid.
					}
				}
				if(table.Columns.Contains("PatNum")) {//Used for statments to determine account splitting.
					row.Tag=rowCur["PatNum"].ToString();
				}
				//Colored Text
				if(table.Columns.Contains("paramTextColor") && !string.IsNullOrEmpty(rowCur["paramTextColor"].ToString())) {
					Color cRowText=Color.FromArgb(PIn.Int(rowCur["paramTextColor"].ToString()));
					if(!cRowText.IsEmpty) {
						row.ColorText=cRowText;
					}
				}
				//Bold Text
				if(table.Columns.Contains("paramIsBold")) {
					row.Bold=(bool)rowCur["paramIsBold"];
				}
				if(table.Columns.Contains("paramIsBorderBoldBottom")) {
					if((bool)rowCur["paramIsBorderBoldBottom"]) {
						row.ColorLborder=Color.Black;
					}
				}
				odGrid.Rows.Add(row);
			}
			odGrid.EndUpdate(true);//Calls ComputeRows and ComputeColumns, meaning the RowHeights int[] has been filled.
			#endregion
			for(int i=0;i<odGrid.Rows.Count;i++) {
				ODPrintRow printRowCur=odGrid.PrintRows[i];
				if(_isPrinting
					&& (printRowCur.YPos-_printMargin.Top<_yPosPrint //rows at the end of previous page
						|| printRowCur.YPos-sheet.HeightPage+_printMargin.Bottom>_yPosPrint)) 
				{
					continue;//continue because we do not want to draw rows from other pages.
				}
				_yAdjCurRow=0;
				//if(printRowCur.YPos<_yPosPrint
				//	|| printRowCur.YPos-_yPosPrint>sheet.HeightPage) {
				//	continue;//skip rows on previous page and rows on next page.
				//}
				#region Draw Title
				if(printRowCur.IsTitleRow) {
					switch(field.FieldName) {//Draw titles differently for different grids.
						case "StatementMain":
							long patNum=PIn.Long(table.Rows[i]["PatNum"].ToString());
							Patient patient=(pat==null || pat.PatNum!=patNum ? Patients.GetPat(patNum) : pat);
							string patName="";
							if(patient!=null) {//should always be true
								patName=patient.GetNameFLnoPref()+(stmt.SuperFamily!=0 ? " - "+patient.PatNum : "");//Append patnum only if super statement
							}
							if(gx==null) {
								g.FillRectangle(Brushes.White,field.XPos-10,printRowCur.YPos-_yPosPrint,odGrid.Width+10,odGrid.TitleHeight);
								g.DrawString(patName,new Font("Arial",10,FontStyle.Bold),new SolidBrush(Color.Black),field.XPos-10,printRowCur.YPos-_yPosPrint);
							}
							else {
								gx.DrawRectangle(Brushes.White,p(field.XPos-10),p(printRowCur.YPos-_yPosPrint-1),p(odGrid.Width+10),p(odGrid.TitleHeight));
								using(Font _font=new Font("Arial",10,FontStyle.Bold)) {
									GraphicsHelper.DrawStringX(gx,patName,
										new XFont(_font.FontFamily.ToString(),_font.Size,XFontStyle.Bold),XBrushes.Black,
										new RectangleF(field.XPos-10,printRowCur.YPos-_yPosPrint-1,odGrid.Width+10,odGrid.TitleHeight),HorizontalAlignment.Left);
									//gx.DrawString(patName,new XFont(_font.FontFamily.ToString(),_font.Size,XFontStyle.Bold),new SolidBrush(Color.Black),field.XPos-10,yPosGrid);
								}
							}
							break;
						case "StatementPayPlan":
							SizeF sSize=TextRenderer.MeasureText("Payment Plans",new Font("Arial",10,FontStyle.Bold));
							if(gx==null) {
								g.FillRectangle(Brushes.White,field.XPos,printRowCur.YPos-_yPosPrint,odGrid.Width,odGrid.TitleHeight);
								g.DrawString("Payment Plans",new Font("Arial",10,FontStyle.Bold),new SolidBrush(Color.Black),field.XPos+(field.Width-sSize.Width)/2,printRowCur.YPos-_yPosPrint);
							}
							else {
								gx.DrawRectangle(Brushes.White,field.XPos,printRowCur.YPos-_yPosPrint-1,odGrid.Width,odGrid.TitleHeight);
								using(Font _font=new Font("Arial",10,FontStyle.Bold)) {
									GraphicsHelper.DrawStringX(gx,"Payment Plans",
										new XFont(_font.FontFamily.ToString(),_font.Size,XFontStyle.Bold),XBrushes.Black,
										new RectangleF(field.XPos,printRowCur.YPos-_yPosPrint-1,odGrid.Width,odGrid.TitleHeight),HorizontalAlignment.Center);
									//gx.DrawString("Payment Plans",new XFont(_font.FontFamily.ToString(),_font.Size,XFontStyle.Bold),new SolidBrush(Color.Black),field.XPos+(field.Width-sSize.Width)/2,yPosGrid);
								}
							}
							break;
							//grid that shows all payments made on the day of the sheet or attached to procedures on the invoice. 
							//only for invoices.
						case "StatementInvoicePayment":
							//drawing logic is mostly copied from PayPlan grid logic above.
							sSize=TextRenderer.MeasureText("Payments & WriteOffs",new Font("Arial",10,FontStyle.Bold));
							if(gx==null) {
								g.FillRectangle(Brushes.White,field.XPos,printRowCur.YPos-_yPosPrint,odGrid.Width,odGrid.TitleHeight);
								g.DrawString("Payments & WriteOffs",new Font("Arial",10,FontStyle.Bold),new SolidBrush(Color.Black),field.XPos,printRowCur.YPos-_yPosPrint);
							}
							else {
								gx.DrawRectangle(Brushes.White,field.XPos,printRowCur.YPos-_yPosPrint-1,odGrid.Width,odGrid.TitleHeight);
								using(Font _font = new Font("Arial",10,FontStyle.Bold)) {
									GraphicsHelper.DrawStringX(gx,"Payments",
										new XFont(_font.FontFamily.ToString(),_font.Size,XFontStyle.Bold),XBrushes.Black,
										new RectangleF(field.XPos,printRowCur.YPos-_yPosPrint-1,odGrid.Width,odGrid.TitleHeight),HorizontalAlignment.Center);
								}
							}
							break;
						case "TreatPlanBenefitsFamily":
							sSize=TextRenderer.MeasureText("Family Insurance Benefits",new Font("Arial",10,FontStyle.Bold));
							if(gx==null) {
								g.FillRectangle(Brushes.White,field.XPos,printRowCur.YPos-_yPosPrint,odGrid.Width,odGrid.TitleHeight);
								g.DrawString("Family Insurance Benefits",new Font("Arial",10,FontStyle.Bold),new SolidBrush(Color.Black),field.XPos+(field.Width-sSize.Width)/2,printRowCur.YPos-_yPosPrint);
							}
							else {
								gx.DrawRectangle(Brushes.White,field.XPos,printRowCur.YPos-_yPosPrint-1,odGrid.Width,odGrid.TitleHeight);
								using(Font _font=new Font("Arial",10,FontStyle.Bold)) {
									GraphicsHelper.DrawStringX(gx,"Family Insurance Benefits",
										new XFont(_font.FontFamily.ToString(),_font.Size,XFontStyle.Bold),XBrushes.Black,
										new RectangleF(field.XPos,printRowCur.YPos-_yPosPrint-1,odGrid.Width,odGrid.TitleHeight),HorizontalAlignment.Center);
									//gx.DrawString("Payment Plans",new XFont(_font.FontFamily.ToString(),_font.Size,XFontStyle.Bold),new SolidBrush(Color.Black),field.XPos+(field.Width-sSize.Width)/2,yPosGrid);
								}
							}
							break;
						case "TreatPlanBenefitsIndividual":
							sSize=TextRenderer.MeasureText("Individual Insurance Benefits",new Font("Arial",10,FontStyle.Bold));
							if(gx==null) {
								g.FillRectangle(Brushes.White,field.XPos,printRowCur.YPos-_yPosPrint,odGrid.Width,odGrid.TitleHeight);
								g.DrawString("Individual Insurance Benefits",new Font("Arial",10,FontStyle.Bold),new SolidBrush(Color.Black),field.XPos+(field.Width-sSize.Width)/2,printRowCur.YPos-_yPosPrint);
							}
							else {
								gx.DrawRectangle(Brushes.White,field.XPos,printRowCur.YPos-_yPosPrint-1,odGrid.Width,odGrid.TitleHeight);
								using(Font _font=new Font("Arial",10,FontStyle.Bold)) {
									GraphicsHelper.DrawStringX(gx,"Individual Insurance Benefits",
										new XFont(_font.FontFamily.ToString(),_font.Size,XFontStyle.Bold),XBrushes.Black,
										new RectangleF(field.XPos,printRowCur.YPos-_yPosPrint-1,odGrid.Width,odGrid.TitleHeight),HorizontalAlignment.Center);
									//gx.DrawString("Payment Plans",new XFont(_font.FontFamily.ToString(),_font.Size,XFontStyle.Bold),new SolidBrush(Color.Black),field.XPos+(field.Width-sSize.Width)/2,yPosGrid);
								}
							}
							break;
						default:
							if(gx==null) {
								odGrid.PrintTitle(g,field.XPos,printRowCur.YPos-_yPosPrint);
							}
							else {
								odGrid.PrintTitleX(gx,field.XPos,printRowCur.YPos-_yPosPrint);
							}
							break;
					}
					_yAdjCurRow+=odGrid.TitleHeight;
				}
				#endregion
				#region Draw Header
				if(printRowCur.IsHeaderRow) {
					if(gx==null) {
						odGrid.PrintHeader(g,field.XPos,printRowCur.YPos-_yPosPrint+_yAdjCurRow);
					}
					else {
						odGrid.PrintHeaderX(gx,field.XPos,printRowCur.YPos-_yPosPrint+_yAdjCurRow);
					}
					_yAdjCurRow+=odGrid.HeaderHeight;
				}
				#endregion
				#region Draw Row
				if(gx==null) {
					odGrid.PrintRow(i,g,field.XPos,printRowCur.YPos-_yPosPrint+_yAdjCurRow,printRowCur.IsBottomRow,true,isPrinting);
				}
				else {
					odGrid.PrintRowX(i,gx,field.XPos,printRowCur.YPos-_yPosPrint+_yAdjCurRow,printRowCur.IsBottomRow,true);
				}
				_yAdjCurRow+=odGrid.Rows[i].RowHeight;
				#endregion
				#region Draw Footer (rare)
				if(printRowCur.IsFooterRow) {
					_yAdjCurRow+=2;
					switch(field.FieldName) {
						case "StatementPayPlan":
							DataTable tableMisc=dataSet.Tables["misc"];
							if(tableMisc==null) {
								tableMisc=new DataTable();
							}
							double payPlanDue=tableMisc.Rows.OfType<DataRow>().Where(x => x["descript"].ToString()=="payPlanDue").Sum(x => PIn.Double(x["value"].ToString()));
							if(gx==null) {
								RectangleF rf=new RectangleF(sheet.Width-60-field.Width,printRowCur.YPos-_yPosPrint+_yAdjCurRow,field.Width,odGrid.TitleHeight);
								g.FillRectangle(Brushes.White,rf);
								StringFormat sf=new StringFormat();
								sf.Alignment=StringAlignment.Far;
								g.DrawString("Payment Plan Amount Due: "+payPlanDue.ToString("c"),new Font("Arial",9,FontStyle.Bold),new SolidBrush(Color.Black),rf,sf);
							}
							else {
								gx.DrawRectangle(Brushes.White,p(sheet.Width-field.Width-60),p(printRowCur.YPos-_yPosPrint+_yAdjCurRow),p(field.Width),p(odGrid.TitleHeight));
								using(Font _font=new Font("Arial",9,FontStyle.Bold)) {
									GraphicsHelper.DrawStringX(gx,"Payment Plan Amount Due: "+payPlanDue.ToString("c"),new XFont(_font.FontFamily.ToString(),_font.Size,XFontStyle.Bold),XBrushes.Black,new RectangleF(sheet.Width-field.Width-60,printRowCur.YPos-_yPosPrint+_yAdjCurRow,field.Width,odGrid.TitleHeight),HorizontalAlignment.Right);
								}
							}
							break;
						case "StatementInvoicePayment":
							//Table should be filled with payments for today & attached to procs on the invoice.
							//drawing logic copied from payplan logic above.
							if(table==null) {
								tableMisc=new DataTable();
							}
							double totalPayments=table.Select().Sum(x => PIn.Double(x["amt"].ToString()));
							if(gx==null) {
								RectangleF rf=new RectangleF(sheet.Width-60-field.Width,printRowCur.YPos-_yPosPrint+_yAdjCurRow,field.Width,odGrid.TitleHeight);
								g.FillRectangle(Brushes.White,rf);
								StringFormat sf=new StringFormat();
								sf.Alignment=StringAlignment.Far;
								if(PrefC.GetBool(PrefName.InvoicePaymentsGridShowNetProd)) {
									g.DrawString("Total Payments & WriteOffs: "+totalPayments.ToString("c"),new Font("Arial",9,FontStyle.Bold),new SolidBrush(Color.Black),rf,sf);
								}
								else {
									g.DrawString("Total Payments: "+totalPayments.ToString("c"),new Font("Arial",9,FontStyle.Bold),new SolidBrush(Color.Black),rf,sf);
								}
							}
							else {
								gx.DrawRectangle(Brushes.White,p(sheet.Width-field.Width-60),p(printRowCur.YPos-_yPosPrint+_yAdjCurRow),p(field.Width),p(odGrid.TitleHeight));
								using(Font _font = new Font("Arial",9,FontStyle.Bold)) {
									if(PrefC.GetBool(PrefName.InvoicePaymentsGridShowNetProd)) {
										GraphicsHelper.DrawStringX(gx,"Total Payments & WriteOffs: "+totalPayments.ToString("c"),new XFont(_font.FontFamily.ToString(),_font.Size,XFontStyle.Bold),XBrushes.Black,new RectangleF(sheet.Width-field.Width-60,printRowCur.YPos-_yPosPrint+_yAdjCurRow,field.Width,odGrid.TitleHeight),HorizontalAlignment.Right);
									}
									else {
										GraphicsHelper.DrawStringX(gx,"Total Payments: "+totalPayments.ToString("c"),new XFont(_font.FontFamily.ToString(),_font.Size,XFontStyle.Bold),XBrushes.Black,new RectangleF(sheet.Width-field.Width-60,printRowCur.YPos-_yPosPrint+_yAdjCurRow,field.Width,odGrid.TitleHeight),HorizontalAlignment.Right);
									}
								}
							}
							break;
					}
				}
				#endregion
			}
			odGrid.Dispose();
			return odGrid.PrintHeight;
		}

		private static void filterColumnsHelper(Sheet sheet,SheetField field,List<DisplayField> Columns) {
			switch(sheet.SheetType+"."+field.FieldName) {
				case "TreatmentPlan.TreatPlanMain":
					bool checkShowDiscount;
					bool checkShowFees;
					bool checkShowIns;
					try {
						checkShowDiscount=(bool)SheetParameter.GetParamByName(sheet.Parameters,"checkShowDiscount").ParamValue;
						checkShowFees=(bool)SheetParameter.GetParamByName(sheet.Parameters,"checkShowFees").ParamValue;
						checkShowIns=(bool)SheetParameter.GetParamByName(sheet.Parameters,"checkShowIns").ParamValue;
					}
					catch {
						//if unable to find any assume default values of true
						checkShowDiscount=true;
						checkShowFees=true;
						checkShowIns=true;
					}
					if(!checkShowFees) {
						Columns.RemoveAll(x => x.InternalName=="Fee");
					}
					if(!checkShowIns) {
						Columns.RemoveAll(x => x.InternalName=="Pri Ins" || x.InternalName=="Sec Ins" || x.InternalName=="DPlan");
					}
					if(!checkShowDiscount) {
						Columns.RemoveAll(x => x.InternalName=="Discount");
					}
					if(!checkShowIns && !checkShowDiscount) {
						Columns.RemoveAll(x => x.InternalName=="Pat");
					}
					//recenters the GridColumnStylesCollection on the page.
					field.XPos=(sheet.WidthPage-Columns.Sum(x => x.ColumnWidth))/2;
					break;
				case "Statement.StatementAging":
					Statement stmt=null;
					try {
						stmt=(Statement)SheetParameter.GetParamByName(sheet.Parameters,"Statement").ParamValue;
					}
					catch(Exception) {}
					if(sheet.SheetType==SheetTypeEnum.Statement && stmt!=null &&  stmt.SuperFamily==0) {
						Columns.RemoveAll(x => x.InternalName=="AcctTotal" || x.InternalName=="Account");
						Columns.RemoveAll(x => x.InternalName=="PatNum");
					}
					break;
			}
		}

		///<summary>Calculates the bottom of the current page assuming a 40px top margin (except for MedLabResults sheets which have a 120 top margin) and 60px bottom margin.</summary>
		public static int bottomCurPage(int yPos,Sheet sheet,out int pageCount) {
			Sheets.SetPageMargin(sheet,_printMargin);
			pageCount=Sheets.CalculatePageCount(sheet,_printMargin);
			if(pageCount==1 && sheet.SheetType!=SheetTypeEnum.MedLabResults) {
				return sheet.HeightPage;
			}
			int retVal=sheet.HeightPage-_printMargin.Bottom;//First page bottom is not changed by top margin. Example: 1100px page height, 60px bottom, 1040px is first page bottom
			pageCount=1;
			while(retVal<yPos){
				pageCount++;
				//each page bottom after the first, 1040px is first page break+1100px page height-top margin-bottom margin=2040px if top is 40px, 1960 if top is 120px
				retVal+=sheet.HeightPage-_printMargin.Bottom-_printMargin.Top;
			}
			return retVal;
		}

		public static void drawFieldText(SheetField field,Sheet sheet,Graphics g,XGraphics gx) {
			Plugins.HookAddCode(null,"SheetPrinting.pd_PrintPage_drawFieldLoop",field);
			RectangleF boundsActual;
			if(gx==null){
				FontStyle fontstyle=(field.FontIsBold?FontStyle.Bold:FontStyle.Regular);
				Font font=new Font(field.FontName,field.FontSize,fontstyle);
				Rectangle bounds=new Rectangle(field.XPos,field.YPos-_yPosPrint,field.Width,field.Height);//Math.Min(field.Height,_yPosPrint+sheet.HeightPage-_printMargin.Bottom-field.YPos));
				boundsActual=GraphicsHelper.DrawString(g,field.FieldValue,font,
					field.ItemColor.ToArgb()==Color.FromArgb(0).ToArgb() ? Brushes.Black : new SolidBrush(field.ItemColor),
					bounds,field.TextAlign);
				font.Dispose();
				font=null;
			}
			else{
				XFontStyle xfontstyle=(field.FontIsBold?XFontStyle.Bold:XFontStyle.Regular);
				XFont xfont;
				try {
					xfont=new XFont(field.FontName,field.FontSize,xfontstyle);
				}
				catch(Exception) {
					//There are some fonts that PdfSharp does not support.  Instead of showing an error, use our default font.
					xfont=new XFont("Courier New",field.FontSize,xfontstyle);
				}
				RectangleF rect=new RectangleF(field.XPos,field.YPos-_yPosPrint,field.Width,field.Height);
				boundsActual=GraphicsHelper.DrawStringX(gx,field.FieldValue,xfont,
					field.ItemColor.ToArgb()==Color.FromArgb(0).ToArgb() ? XBrushes.Black : new XSolidBrush(field.ItemColor),
					rect,field.TextAlign);
				//xfont.Dispose();
				xfont=null;
			}
			if(field.FieldType==SheetFieldType.OutputText) {
				switch(sheet.SheetType.ToString()+"."+field.FieldName) {
					case "TreatmentPlan.Note":
						//Add plus 4 to width and height to allow for border thickness.
						if(gx==null) {
							g.DrawRectangle(Pens.DarkGray,
								new Rectangle((int)boundsActual.X,(int)boundsActual.Y,(int)Math.Ceiling(boundsActual.Width+4),(int)Math.Ceiling(boundsActual.Height+4)));
						}
						else {
							gx.DrawRectangle(XPens.DarkGray,
								new XRect(p(boundsActual.X),p(boundsActual.Y),p(boundsActual.Width+4),p(boundsActual.Height+4)));
						}
						break;
				}
			}
		}

		public static void drawFieldCheckBox(SheetField field,Graphics g,XGraphics gx) {
			if(field.FieldValue!="X") {
				return;
			}
			if(gx==null) {
				Pen pen3=new Pen(Brushes.Black,1.6f);
				g.DrawLine(pen3,field.XPos,field.YPos-_yPosPrint,field.XPos+field.Width,field.YPos-_yPosPrint+field.Height);
				g.DrawLine(pen3,field.XPos+field.Width,field.YPos-_yPosPrint,field.XPos,field.YPos-_yPosPrint+field.Height);
				pen3.Dispose();
				pen3=null;
			}
			else {
				XPen pen3=new XPen(XColors.Black,p(1.6f));
				gx.DrawLine(pen3,p(field.XPos),p(field.YPos-_yPosPrint),p(field.XPos+field.Width),p(field.YPos-_yPosPrint+field.Height));
				gx.DrawLine(pen3,p(field.XPos+field.Width),p(field.YPos-_yPosPrint),p(field.XPos),p(field.YPos-_yPosPrint+field.Height));
				pen3=null;
			}
		}

		public static void drawFieldScreenChart(SheetField field,Sheet sheet,Graphics g,XGraphics gx) {
			string[] toothValues=field.FieldValue.Split(';');
			//The PDF and Print versions of the drawing are nearly identical, one uses PDF elements and the other uses general Graphics elements.
		  //The sizing is a bit different because PDF's take pixels so we have to convert the sizes.  
			//Additionally the inner boxes are done differently.  For printing it's one horizontal and one vertical line.  For PDF's they're all rectangles.
			if(gx==null) {
				int xPos=field.XPos;//Start positions.
				int yPos=field.YPos-_yPosPrint-15;
				int toothWidth=field.Width/8;
				int toothHeight=(field.Height/2)-10;
				Pen surfacePen=new Pen(Color.Black,0.5f);
				StringFormat stringFormat=new StringFormat();
				stringFormat.Alignment=StringAlignment.Center;
				stringFormat.LineAlignment=StringAlignment.Center;
				RectangleF rectFormat=new RectangleF();
				Font font=new Font(FontFamily.GenericMonospace.ToString(),10,FontStyle.Regular);
				for(int i=0;i<8;i++) {
					int topToothNum=0;
					int botToothNum=0;
					if(i<4) {
						topToothNum=i+2;//2, 3, 4, 5
						botToothNum=31-i;//31, 30, 29, 28
					}
					else {
						topToothNum=i+8;//12, 13, 14, 15
						botToothNum=25-i;//21, 20, 19, 18
					}
					string[] topToothSurfaces=toothValues[i].Split(',');//Getting teeth from indexes 0 to 7
					string[] botToothSurfaces=toothValues[toothValues.Length-(i+1)].Split(',');//Getting teeth from indexes 15 to 8
					rectFormat.Location=new PointF((float)xPos,(float)yPos);
					rectFormat.Size=new Size((int)toothWidth,15);
					g.DrawString(topToothNum.ToString(),font,Brushes.Black,rectFormat,stringFormat);
					rectFormat.Location=new PointF((float)xPos,(float)(yPos+2*toothHeight+15));//20 for top tooth, 2xtoothHeight to get to the bottom of the two teeth rectangles.
					g.DrawString(botToothNum.ToString(),font,Brushes.Black,rectFormat,stringFormat);
					g.DrawRectangle(Pens.Black,xPos,yPos+15,toothWidth,toothHeight);//Draw top tooth box
					g.DrawRectangle(Pens.Black,xPos,yPos+15+toothHeight,toothWidth,toothHeight);//Draw bottom tooth box
					if(i<2 || i>5) { //Only molar teeth have multiple surfaces.
						g.DrawLine(surfacePen,xPos,yPos+15+(toothHeight/2),xPos+toothWidth,yPos+15+(toothHeight/2));//Draw top tooth horizontal line
						g.DrawLine(surfacePen,xPos+(toothWidth/2),yPos+15,xPos+(toothWidth/2),yPos+15+(toothHeight/2));//Draw top tooth vertical line
						g.DrawLine(surfacePen,xPos,yPos+15+(toothHeight/2)+toothHeight,xPos+toothWidth,yPos+15+(toothHeight/2)+toothHeight);//Draw bottom tooth horizontal line
						g.DrawLine(surfacePen,xPos+(toothWidth/2),yPos+15+toothHeight,xPos+(toothWidth/2),yPos+15+(toothHeight/2)+toothHeight);//Draw bottom tooth vertical line
						rectFormat.Location=new PointF((float)xPos,(float)yPos+15);
						rectFormat.Size=new Size((int)toothWidth/2,(int)(toothHeight/2));
						g.DrawString(topToothSurfaces[0],font,(topToothSurfaces[0]=="d" || topToothSurfaces[0]=="m")?Brushes.LightGray : Brushes.Black,rectFormat,stringFormat);//Surface information
						rectFormat.Location=new PointF((float)(xPos+(toothWidth/2)),(float)yPos+15);
						rectFormat.Size=new Size((int)toothWidth/2,(int)(toothHeight/2));
						g.DrawString(topToothSurfaces[1],font,(topToothSurfaces[1]=="d" || topToothSurfaces[1]=="m")?Brushes.LightGray : Brushes.Black,rectFormat,stringFormat);
						rectFormat.Location=new PointF((float)xPos,(float)(yPos+(toothHeight/2)+15));
						rectFormat.Size=new Size((int)toothWidth,(int)(toothHeight/2));
						g.DrawString(topToothSurfaces[2],font,(topToothSurfaces[2]=="ling" || topToothSurfaces[2]=="buc")?Brushes.LightGray : Brushes.Black,rectFormat,stringFormat);
						rectFormat.Location=new PointF((float)xPos,(float)(yPos+15+toothHeight));
						rectFormat.Size=new Size((int)toothWidth/2,(int)(toothHeight/2));
						g.DrawString(botToothSurfaces[0],font,(botToothSurfaces[0]=="d" || botToothSurfaces[0]=="m")?Brushes.LightGray : Brushes.Black,rectFormat,stringFormat);//Surface information
						rectFormat.Location=new PointF((float)(xPos+(toothWidth/2)),(float)(yPos+15+toothHeight));
						rectFormat.Size=new Size((int)toothWidth/2,(int)(toothHeight/2));
						g.DrawString(botToothSurfaces[1],font,(botToothSurfaces[1]=="d" || botToothSurfaces[1]=="m")?Brushes.LightGray : Brushes.Black,rectFormat,stringFormat);
						rectFormat.Location=new PointF((float)xPos,(float)(yPos+15+toothHeight+(toothHeight/2)));
						rectFormat.Size=new Size((int)toothWidth,(int)(toothHeight/2));
						g.DrawString(botToothSurfaces[2],font,(botToothSurfaces[2]=="ling" || botToothSurfaces[2]=="buc")?Brushes.LightGray : Brushes.Black,rectFormat,stringFormat);
					}
					else {
						rectFormat.Location=new PointF((float)xPos,(float)yPos+15);
						rectFormat.Size=new Size((int)toothWidth,(int)toothHeight);
						g.DrawString(topToothSurfaces[2],font,Brushes.Black,rectFormat,stringFormat);//Will be empty or a selected option
						rectFormat.Location=new PointF((float)xPos,(float)(yPos+toothHeight+15));
						rectFormat.Size=new Size((int)toothWidth,(int)toothHeight);
						g.DrawString(botToothSurfaces[2],font,Brushes.Black,rectFormat,stringFormat);//Will be empty or a selected option
					}
					xPos+=toothWidth;
				}
			}
			else {
				double xPos=p(field.XPos);
				double yPos=p(field.YPos-_yPosPrint-15);//Start section for tooth chart control
				double toothWidth=p(field.Width/8);
				double toothHeight=p((field.Height/2)-20);
				XPen surfacePen=new XPen(Color.Black,0.5);
				XStringFormat stringFormat=new XStringFormat();
				stringFormat.Alignment=XStringAlignment.Center;	
				stringFormat.LineAlignment=XLineAlignment.Center;
				RectangleF rectFormat=new RectangleF();
				XFont xfont=new XFont(FontFamily.GenericMonospace.ToString(),toothHeight-20,XFontStyle.Regular);
				for(int i=0;i<8;i++) {
					int topToothNum=0;
					int botToothNum=0;
					if(i<4) {
						topToothNum=i+2;//2, 3, 4, 5
						botToothNum=31-i;//31, 30, 29, 28
					}
					else {
						topToothNum=i+8;//12, 13, 14, 15
						botToothNum=25-i;//21, 20, 19, 18
					}
					string[] topToothSurfaces=toothValues[i].Split(',');//Getting teeth from indexes 0 to 7
					string[] botToothSurfaces=toothValues[toothValues.Length-(i+1)].Split(',');//Getting teeth from indexes 15 to 8
					rectFormat.Location=new PointF((float)xPos,(float)yPos);
					rectFormat.Size=new Size((int)toothWidth,15);
					gx.DrawString(topToothNum.ToString(),xfont,XBrushes.Black,rectFormat,stringFormat);
					rectFormat.Location=new PointF((float)xPos,(float)(yPos+2*toothHeight+15));//20 for top tooth, 2xtoothHeight to get to the bottom of the two teeth rectangles.
					gx.DrawString(botToothNum.ToString(),xfont,XBrushes.Black,rectFormat,stringFormat);
					gx.DrawRectangle(XPens.Black,xPos,yPos+15,toothWidth,toothHeight);//Draw top tooth box
					gx.DrawRectangle(XPens.Black,xPos,yPos+15+toothHeight,toothWidth,toothHeight);//Draw bottom tooth box
					if(i<2 || i>5) { //Only molar teeth have multiple surfaces.
						gx.DrawRectangle(surfacePen,xPos,yPos+15,toothWidth/2,toothHeight/2);//Draw one top tooth surface
						gx.DrawRectangle(surfacePen,xPos+(toothWidth/2),yPos+15,toothWidth/2,toothHeight/2);//Draw second top tooth surface.
						gx.DrawRectangle(surfacePen,xPos,yPos+toothHeight+15,toothWidth/2,toothHeight/2);//Draw one bottom tooth surface
						gx.DrawRectangle(surfacePen,xPos+(toothWidth/2),yPos+toothHeight+15,toothWidth/2,toothHeight/2);//Draw second bottom tooth surface.
						rectFormat.Location=new PointF((float)xPos,(float)yPos+15);
						rectFormat.Size=new Size((int)toothWidth/2,(int)(toothHeight/2));
						gx.DrawString(topToothSurfaces[0],xfont,(topToothSurfaces[0]=="d" || topToothSurfaces[0]=="m")?XBrushes.LightGray : XBrushes.Black,rectFormat,stringFormat);//Surface information
						rectFormat.Location=new PointF((float)(xPos+(toothWidth/2)),(float)yPos+15);
						rectFormat.Size=new Size((int)toothWidth/2,(int)(toothHeight/2));
						gx.DrawString(topToothSurfaces[1],xfont,(topToothSurfaces[1]=="d" || topToothSurfaces[1]=="m")?XBrushes.LightGray : XBrushes.Black,rectFormat,stringFormat);
						rectFormat.Location=new PointF((float)xPos,(float)(yPos+(toothHeight/2)+15));
						rectFormat.Size=new Size((int)toothWidth,(int)(toothHeight/2));
						gx.DrawString(topToothSurfaces[2],xfont,(topToothSurfaces[2]=="ling" || topToothSurfaces[2]=="buc")?XBrushes.LightGray : XBrushes.Black,rectFormat,stringFormat);
						rectFormat.Location=new PointF((float)xPos,(float)(yPos+15+toothHeight));
						rectFormat.Size=new Size((int)toothWidth/2,(int)(toothHeight/2));
						gx.DrawString(botToothSurfaces[0],xfont,(botToothSurfaces[0]=="d" || botToothSurfaces[0]=="m")?XBrushes.LightGray : XBrushes.Black,rectFormat,stringFormat);//Surface information
						rectFormat.Location=new PointF((float)(xPos+(toothWidth/2)),(float)(yPos+15+toothHeight));
						rectFormat.Size=new Size((int)toothWidth/2,(int)(toothHeight/2));
						gx.DrawString(botToothSurfaces[1],xfont,(botToothSurfaces[1]=="d" || botToothSurfaces[1]=="m")?XBrushes.LightGray : XBrushes.Black,rectFormat,stringFormat);
						rectFormat.Location=new PointF((float)xPos,(float)(yPos+15+toothHeight+(toothHeight/2)));
						rectFormat.Size=new Size((int)toothWidth,(int)(toothHeight/2));
						gx.DrawString(botToothSurfaces[2],xfont,(botToothSurfaces[2]=="ling" || botToothSurfaces[2]=="buc")?XBrushes.LightGray : XBrushes.Black,rectFormat,stringFormat);
					}
					else {
						rectFormat.Location=new PointF((float)xPos,(float)yPos+15);
						rectFormat.Size=new Size((int)toothWidth,(int)toothHeight);
						gx.DrawString(topToothSurfaces[2],xfont,XBrushes.Black,rectFormat,stringFormat);//Will be empty or a selected option
						rectFormat.Location=new PointF((float)xPos,(float)(yPos+toothHeight+15));
						rectFormat.Size=new Size((int)toothWidth,(int)toothHeight);
						gx.DrawString(botToothSurfaces[2],xfont,XBrushes.Black,rectFormat,stringFormat);//Will be empty or a selected option
					}
					xPos+=toothWidth;
				}
			}
		}

		public static void drawFieldComboBox(SheetField field,Sheet sheet,Graphics g,XGraphics gx) {
			string comboChoice=field.FieldValue.Split(';')[0];
			if(gx==null){
				Font font=new Font(FontFamily.GenericMonospace,field.Height-7,FontStyle.Regular);
				Rectangle bounds=new Rectangle(field.XPos,field.YPos-_yPosPrint,field.Width,field.Height);
				GraphicsHelper.DrawString(g,comboChoice,font,Brushes.Black,bounds,HorizontalAlignment.Left);
				font.Dispose();
				font=null;
			}
			else{
				XFont xfont=new XFont(FontFamily.GenericMonospace.ToString(),field.Height-10,XFontStyle.Regular);
				RectangleF rect=new RectangleF(field.XPos,field.YPos-_yPosPrint,field.Width,field.Height);
				GraphicsHelper.DrawStringX(gx,comboChoice,xfont,XBrushes.Black,rect,HorizontalAlignment.Left);
				xfont=null;
			}
		}

		public static void drawFieldSigBox(SheetField field,Sheet sheet,Graphics g,XGraphics gx) {
			Bitmap sigImage=new Bitmap(field.Width,field.Height);
			if(sheet.SheetType==SheetTypeEnum.TreatmentPlan) {
				sigImage=GetSigTPHelper(sheet,field);
			}
			else if(sheet.SheetType==SheetTypeEnum.PaymentPlan) {
				sigImage=GetSigPPHelper(sheet,field);
			}
			else {
				SignatureBoxWrapper wrapper=new SignatureBoxWrapper();
				wrapper.Width=field.Width;
				wrapper.Height=field.Height;
				if(field.FieldValue.Length>0) { //a signature is present
					bool sigIsTopaz=false;
					if(field.FieldValue[0]=='1') {
						sigIsTopaz=true;
					}
					string signature="";
					if(field.FieldValue.Length>1) {
						signature=field.FieldValue.Substring(1);
					}
					//string keyData=Sheets.GetSignatureKey(sheet);//can't do this because some of the fields might have different new line characters. Sig will be invalid.
					wrapper.FillSignature(sigIsTopaz,field.SigKey,signature);
					sigImage=wrapper.GetSigImage();
				}
			}
			if(g!=null) {
				g.DrawImage(sigImage,field.XPos,field.YPos-_yPosPrint,field.Width-2,field.Height-2);
			}
			else {
				gx.DrawImage(XImage.FromGdiPlusImage(sigImage),p(field.XPos),p(field.YPos-_yPosPrint),p(field.Width-2),p(field.Height-2));
			}
			sigImage.Dispose();
			sigImage=null;
		}

		private static Bitmap GetSigTPHelper(Sheet sheet,SheetField field) {
			TreatPlan treatPlan=(TreatPlan)SheetParameter.GetParamByName(sheet.Parameters,"TreatPlan").ParamValue;
			if(treatPlan.Signature!="") {
				SignatureBoxWrapper sigBoxWrapper=new SignatureBoxWrapper();
				sigBoxWrapper.SignatureMode=SignatureBoxWrapper.SigMode.TreatPlan;
				string keyData=TreatPlans.GetKeyDataForSignatureHash(treatPlan,treatPlan.ListProcTPs);
				sigBoxWrapper.FillSignature(treatPlan.SigIsTopaz,keyData,treatPlan.Signature);
				if(sigBoxWrapper.GetNumberOfTabletPoints(treatPlan.SigIsTopaz)!=0) {
					return sigBoxWrapper.GetSigImage();
				}
			}
			return new Bitmap(field.Width,field.Height);
		}

		private static Bitmap GetSigPPHelper(Sheet sheet,SheetField field) {
			PayPlan payPlan=(PayPlan)SheetParameter.GetParamByName(sheet.Parameters,"payplan").ParamValue;
			string keyData=(string)SheetParameter.GetParamByName(sheet.Parameters,"keyData").ParamValue;
			if(payPlan.Signature!="") {
				SignatureBoxWrapper sigBoxWrapper=new SignatureBoxWrapper();
				sigBoxWrapper.FillSignature(payPlan.SigIsTopaz,keyData,payPlan.Signature);
				if(sigBoxWrapper.GetNumberOfTabletPoints(payPlan.SigIsTopaz)!=0) {
					return sigBoxWrapper.GetSigImage();
				}
			}
			return new Bitmap(field.Width,field.Height); //return blank image if sig invalid.
		}

		private static void drawHeader(Sheet sheet,Graphics g,XGraphics gx) {
			if(_pagesPrinted==0) {
				return;//Never draw header on first page
			}
			//white-out the header.
			if(gx==null) {
				g.FillRectangle(Brushes.White,0,0,sheet.WidthPage,_printMargin.Top);
			}
			else {
				gx.DrawRectangle(XPens.White,Brushes.White,p(0),p(0),p(sheet.WidthPage),p(_printMargin.Top));
			}
			if(sheet.SheetType==SheetTypeEnum.MedLabResults) {
				drawMedLabHeader(sheet,g,gx);
			}
		}

		private static void drawFooter(Sheet sheet,Graphics g,XGraphics gx) {
			if(Sheets.CalculatePageCount(sheet,_printMargin)==1 && sheet.SheetType!=SheetTypeEnum.MedLabResults) {
				return;//Never draw footers on single page sheets.
			}
			//whiteout footer.
			if(gx==null) {
				g.FillRectangle(Brushes.White,0,sheet.HeightPage-_printMargin.Bottom,sheet.WidthPage,sheet.HeightPage);
			}
			else {
				gx.DrawRectangle(XPens.White,Brushes.White,p(0),p(sheet.HeightPage-_printMargin.Bottom),p(sheet.WidthPage),p(sheet.HeightPage));
			}
			if(sheet.SheetType==SheetTypeEnum.MedLabResults) {
				drawMedLabFooter(sheet,g,gx);
			}
		}

		private static void drawMedLabFooter(Sheet sheet,Graphics g,XGraphics gx) {
			SheetField fieldCur=new SheetField();
			fieldCur.XPos=50;
			int pageCount;
			fieldCur.YPos=bottomCurPage(_yPosPrint+_printMargin.Bottom+_printMargin.Top+1,sheet,out pageCount)+1;
			fieldCur.Width=625;
			fieldCur.Height=20;
			drawFieldRectangle(fieldCur,g,gx);
			fieldCur.XPos=675;
			fieldCur.Width=125;
			drawFieldRectangle(fieldCur,g,gx);
			string patLName="";
			string patFName="";
			string patMiddleI="";
			string specNum="";
			foreach(SheetField sf in sheet.SheetFields) {
				switch(sf.FieldName) {
					case "patient.LName":
						patLName=sf.FieldValue;
						continue;
					case "patient.FName":
						patFName=sf.FieldValue;
						continue;
					case "patient.MiddleI":
						patMiddleI=sf.FieldValue;
						continue;
					case "medlab.PatIDLab":
						specNum=sf.FieldValue;
						continue;
					default:
						continue;
				}
			}
			fieldCur.FieldValue=patLName;
			if(patLName!="" && (patFName!="" || patMiddleI!="")) {
				fieldCur.FieldValue+=", ";
			}
			fieldCur.FieldValue+=patFName;
			if(fieldCur.FieldValue!="" && patMiddleI!="") {
				fieldCur.FieldValue+=" ";
			}
			fieldCur.FieldValue+=patMiddleI;
			fieldCur.FontSize=9;
			fieldCur.FontName="Arial";
			fieldCur.FontIsBold=false;
			fieldCur.XPos=53;
			fieldCur.YPos+=1;
			fieldCur.Width=245;
			fieldCur.Height=17;
			fieldCur.TextAlign=HorizontalAlignment.Left;
			fieldCur.ItemColor=Color.FromKnownColor(KnownColor.Black);
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue=specNum;
			fieldCur.XPos=678;
			fieldCur.Width=120;
			fieldCur.TextAlign=HorizontalAlignment.Center;
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue=DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
			fieldCur.FontSize=8.5f;
			fieldCur.FontName=sheet.FontName;
			fieldCur.XPos=50;//position the field at 50 for left margin
			fieldCur.YPos+=19;//drop down 19 pixels from the top of the text in the rect (17 pixel height of text box + 1 pixel to bottom of rect + 1 pixel)
			fieldCur.Width=150;
			fieldCur.TextAlign=HorizontalAlignment.Left;
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue=String.Format("Page {0} of {1}",_pagesPrinted+1,Sheets.CalculatePageCount(sheet,_printMargin));
			fieldCur.XPos=sheet.Width-200;//width of field is 150, with a right margin of 50 xPos is sheet width-150-50=width-200
			fieldCur.TextAlign=HorizontalAlignment.Right;
			drawFieldText(fieldCur,sheet,g,gx);
			if(_medLab.IsPreliminaryResult) {
				fieldCur.FieldValue="Preliminary Report";
			}
			else {
				fieldCur.FieldValue="Final Report";
			}
			fieldCur.FontSize=10.0f;
			fieldCur.FontIsBold=true;
			//field will be centered on page, since page count is taking up 150 pixels plus page right margin of 50 pixels on the right side of page
			//and date printed is taking up 50 pixel left margin plus 150 pixel field width on the left side of page
			//field width will be sheet.Width-400 and XPos will be 200
			fieldCur.XPos=200;
			fieldCur.YPos+=2;
			fieldCur.Width=sheet.Width-400;//sheet width-150 (date field width)-150 (page count field width)-50 (left margin)-50 (right margin)
			fieldCur.TextAlign=HorizontalAlignment.Center;
			drawFieldText(fieldCur,sheet,g,gx);
		}

		private static void drawMedLabHeader(Sheet sheet,Graphics g,XGraphics gx) {
			SheetField fieldCur=new SheetField();
			fieldCur.XPos=50;
			fieldCur.YPos=_yPosPrint+40;//top of the top rectangle
			fieldCur.Width=529;
			fieldCur.Height=40;
			drawFieldRectangle(fieldCur,g,gx);
			fieldCur.XPos=579;
			fieldCur.Width=221;
			drawFieldRectangle(fieldCur,g,gx);
			fieldCur.XPos=50;
			fieldCur.YPos+=40;//drop down an additional 40 pixels for second row of rectangles
			fieldCur.Width=100;
			drawFieldRectangle(fieldCur,g,gx);
			fieldCur.XPos=150;
			fieldCur.Width=140;
			drawFieldRectangle(fieldCur,g,gx);
			fieldCur.XPos=290;
			fieldCur.Width=100;
			drawFieldRectangle(fieldCur,g,gx);
			fieldCur.XPos=390;
			fieldCur.Width=145;
			drawFieldRectangle(fieldCur,g,gx);
			fieldCur.XPos=535;
			fieldCur.Width=100;
			drawFieldRectangle(fieldCur,g,gx);
			fieldCur.XPos=635;
			fieldCur.Width=65;
			drawFieldRectangle(fieldCur,g,gx);
			fieldCur.XPos=700;
			fieldCur.Width=100;
			drawFieldRectangle(fieldCur,g,gx);
			fieldCur.FieldValue="Patient Name";
			fieldCur.FontSize=8.5f;
			fieldCur.FontName="Arial";
			fieldCur.FontIsBold=false;
			fieldCur.XPos=54;
			fieldCur.YPos=_yPosPrint+44;//4 pixels down from the rectangle top for static text descriptions of text boxes in header
			fieldCur.Width=522;
			fieldCur.Height=15;
			fieldCur.TextAlign=HorizontalAlignment.Left;
			fieldCur.ItemColor=Color.FromKnownColor(KnownColor.GrayText);
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue="Specimen Number";
			fieldCur.XPos=583;
			fieldCur.Width=214;
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue="Account Number";
			fieldCur.XPos=54;
			fieldCur.YPos+=40;//drop down an additional 40 pixels for second row of static text descriptions
			fieldCur.Width=93;
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue="Patient ID";
			fieldCur.XPos=154;
			fieldCur.Width=133;
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue="Control Number";
			fieldCur.XPos=294;
			fieldCur.Width=93;
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue="Date & Time Collected";
			fieldCur.XPos=394;
			fieldCur.Width=138;
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue="Date Reported";
			fieldCur.XPos=539;
			fieldCur.Width=93;
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue="Gender";
			fieldCur.XPos=639;
			fieldCur.Width=58;
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue="Date of Birth";
			fieldCur.XPos=704;
			fieldCur.Width=93;
			drawFieldText(fieldCur,sheet,g,gx);
			string patLName="";
			string patFName="";
			string patMiddleI="";
			string specNum="";
			string acctNum="";
			string patId="";
			string ctrlNum="";
			string dateTCollected="";
			string dateReported="";
			string gender="";
			string birthdate="";
			foreach(SheetField sf in sheet.SheetFields) {
				switch(sf.FieldName) {
					case "patient.LName":
						patLName=sf.FieldValue;
						continue;
					case "patient.FName":
						patFName=sf.FieldValue;
						continue;
					case "patient.MiddleI":
						patMiddleI=sf.FieldValue;
						continue;
					case "medlab.PatIDLab":
						specNum=sf.FieldValue;
						continue;
					case "medlab.PatAccountNum":
						acctNum=sf.FieldValue;
						continue;
					case "medlab.PatIDAlt":
						patId=sf.FieldValue;
						continue;
					case "medlab.SpecimenIDAlt":
						ctrlNum=sf.FieldValue;
						continue;
					case "medlab.DateTimeCollected":
						dateTCollected=sf.FieldValue;
						continue;
					case "medlab.DateTimeReported":
						dateReported=PIn.DateT(sf.FieldValue).ToShortDateString();
						if(dateReported==DateTime.MinValue.ToShortDateString()) {
							dateReported="";
						}
						continue;
					case "patient.Gender":
						gender=sf.FieldValue;
						continue;
					case "patient.Birthdate":
						birthdate=sf.FieldValue;
						continue;
				}
			}
			fieldCur.FieldValue=patLName+", "+patFName+" "+patMiddleI;
			fieldCur.FontSize=9;
			fieldCur.FontName="Arial";
			fieldCur.FontIsBold=false;
			fieldCur.XPos=53;
			fieldCur.YPos=_yPosPrint+62;//22 pixels down from the rectangle top (second row of text is 20 pixels below static text descriptions)
			fieldCur.Width=524;
			fieldCur.Height=17;
			fieldCur.TextAlign=HorizontalAlignment.Left;
			fieldCur.ItemColor=Color.FromKnownColor(KnownColor.Black);
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue=specNum;
			fieldCur.XPos=582;
			fieldCur.Width=216;
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue=acctNum;
			fieldCur.XPos=53;
			fieldCur.YPos+=40;//drop down an additional 40 pixels for second row
			fieldCur.Width=95;
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue=patId;
			fieldCur.XPos=153;
			fieldCur.Width=135;
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue=ctrlNum;
			fieldCur.XPos=293;
			fieldCur.Width=95;
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue=dateTCollected;
			fieldCur.XPos=393;
			fieldCur.Width=140;
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue=dateReported;
			fieldCur.XPos=538;
			fieldCur.Width=95;
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue=gender;
			fieldCur.XPos=638;
			fieldCur.Width=60;
			drawFieldText(fieldCur,sheet,g,gx);
			fieldCur.FieldValue=birthdate;
			fieldCur.XPos=703;
			fieldCur.Width=95;
			drawFieldText(fieldCur,sheet,g,gx);
		}

		private static void drawCalibration(Sheet sheet,Graphics g,PrintPageEventArgs e,XGraphics gx, PdfPage page) {
			Font font=new Font("Calibri",10f,FontStyle.Regular);
			XFont xfont=new XFont("Calibri",p(10f),XFontStyle.Regular);
			int sLineSize=15;
			int mLineSize=45;
			int lLineSize=90;
			for(int pass=0;pass<3;pass++) {
				int xO=0;//xOrigin
				int yO=0;//yOrigin
				switch(pass) {
					case 0: xO=yO=0; break;
					case 1: xO=sheet.WidthPage/2; yO=sheet.HeightPage/2; break;
					case 2: xO=sheet.WidthPage; yO=sheet.HeightPage; break;
				}
				for(int i=-100;i<2000;i++) {
					if(i%100==0 && pass==0) {
						//label Axis
						if(g!=null) {
							if(i==0) {
								g.DrawString(i.ToString(),font,Brushes.Black,new PointF(4,4));//label 0
							}//don't draw the zero twice
							else {
								g.DrawString(i.ToString(),font,Brushes.Black,new PointF(xO+75,i+2));//label Y-axis
								g.DrawString(i.ToString(),font,Brushes.Black,new PointF(i+2,yO+75));//label X-axis
							}
						}
						else {
							if(i==0) {

							}//don't draw the zero twice
							else {
								gx.DrawString(i.ToString(),xfont,XBrushes.Black,p(xO+75),p(i+2));//label Y-axis
								gx.DrawString(i.ToString(),xfont,XBrushes.Black,p(i+2),p(yO+75));//label X-axis
							}
						}
					}
					if(i%100==0) {
						//draw large lines and label txt
						if(g!=null) {
							g.DrawLine(Pens.Black,new Point(-lLineSize+xO,i),new Point(+lLineSize+xO,i));//Allong Y-axis
							g.DrawLine(Pens.Black,new Point(i,-lLineSize+yO),new Point(i,+lLineSize+yO));//Allong X-axis
						}
						else {
							gx.DrawLine(XPens.Black,p(-lLineSize+xO),p(i),p(+lLineSize+xO),p(i));//Allong Y-axis
							gx.DrawLine(XPens.Black,p(i),p(-lLineSize+yO),p(i),p(+lLineSize+yO));//Allong X-axis
						}
					}
					else if(i%50==0) {
						//draw 50px lines
						if(g!=null) {
							g.DrawLine(Pens.Black,new Point(-mLineSize+xO,i),new Point(+mLineSize+xO,i));//Allong Y-axis
							g.DrawLine(Pens.Black,new Point(i,-mLineSize+yO),new Point(i,+mLineSize+yO));//Allong X-axis
						}
						else {
							gx.DrawLine(XPens.Black,p(-mLineSize+xO),p(i),p(+mLineSize+xO),p(i));//Allong Y-axis
							gx.DrawLine(XPens.Black,p(i),p(-mLineSize+yO),p(i),p(+mLineSize+yO));//Allong X-axis
						}
					}
					else if(i%10==0) {
						//draw small lines
						if(g!=null) {
							g.DrawLine(Pens.Black,new Point(-sLineSize+xO,i),new Point(+sLineSize+xO,i));//Allong Y-axis
							g.DrawLine(Pens.Black,new Point(i,-sLineSize+yO),new Point(i,+sLineSize+yO));//Allong X-axis
						}
						else {
							gx.DrawLine(XPens.Black,new Point(-sLineSize+xO,i),new Point(+sLineSize+xO,i));//Allong Y-axis
							gx.DrawLine(XPens.Black,p(i),p(-sLineSize+yO),p(i),p(+sLineSize+yO));//Allong X-axis
						}
					}
					else if(i%2==0) {
						//draw dots
						if(g!=null) {
							g.DrawLine(Pens.Black,new Point(-1+xO,i),new Point(+1+xO,i));//Allong Y-axis
							g.DrawLine(Pens.Black,new Point(i,-1+yO),new Point(i,+1+yO));//Allong X-axis
						}
						else {
							gx.DrawLine(XPens.Black,p(-1+xO),p(i),p(+1+xO),p(i));//Allong Y-axis
							gx.DrawLine(XPens.Black,p(i),p(-1+yO),p(i),p(+1+yO));//Allong X-axis
						}
					}
				}//end i -100=>2000
			}//end pass
			//infoBlock
			PrinterSettings settings = new PrinterSettings();
			if(g!=null) {
				g.FillRectangle(Brushes.White,110,110,480,100);
				g.DrawRectangle(Pens.Black,110,110,480,100);
				g.DrawString("Sheet Height = "+sheet.HeightPage.ToString(),font,Brushes.Black,112,112);
				g.DrawString("Sheet Width = "+sheet.WidthPage.ToString(),font,Brushes.Black,112,124);//12px per line
				g.DrawString("DefaultPrinter = "+settings.PrinterName,font,Brushes.Black,112,136);
				g.DrawString("HardMarginX = "+e.PageSettings.HardMarginX,font,Brushes.Black,112,148);
				g.DrawString("HardMarginY = "+e.PageSettings.HardMarginY,font,Brushes.Black,112,160);
			}
			else {
				gx.DrawRectangle(XPens.Black,Brushes.White,p(110),p(110),p(480),p(100));
				gx.DrawRectangle(XPens.Black,p(110),p(110),p(480),p(100));
				gx.DrawString("Sheet Height = "+sheet.HeightPage.ToString(),xfont,XBrushes.Black,p(112),p(112));
				gx.DrawString("Sheet Width = "+sheet.WidthPage.ToString(),xfont,XBrushes.Black,p(112),p(124));//12px per line
				gx.DrawString("DefaultPrinter = "+settings.PrinterName,xfont,XBrushes.Black,p(112),p(136));
				gx.DrawString("HardMarginX = "+settings.DefaultPageSettings.HardMarginX,xfont,XBrushes.Black,p(112),p(148));
				gx.DrawString("HardMarginY = "+settings.DefaultPageSettings.HardMarginY,xfont,XBrushes.Black,p(112),p(160));
				gx.DrawString("PDF TrimMargins ^v<> = "+page.TrimMargins.Top+","+page.TrimMargins.Bottom+","+page.TrimMargins.Left+","+page.TrimMargins.Right,xfont,XBrushes.Black,p(112),p(172));
			}
			font.Dispose();
			font=null;
			xfont=null;
		}

		#endregion

		///<summary>If making a statement, use the polymorphism that takes a DataSet otherwise this method will make another call to the db.
		///If fullFileName is in the A to Z folder, pass in fullFileName as a temp file and then upload that file to the cloud.</summary>
		public static void CreatePdf(Sheet sheet,string fullFileName,Statement stmt,MedLab medLab=null) {
			DataSet dataSet=null;
			if(sheet.SheetType==SheetTypeEnum.Statement && stmt!=null) {
				//This should never get hit.  This line of code is here just in case I forgot to update a random spot in our code.
				//Worst case scenario we will end up calling the database a few extra times for the same data set.
				//It use to call this method many, many times so anything is an improvement at this point.
				if(stmt.SuperFamily!=0) {
					dataSet=AccountModules.GetSuperFamAccount(stmt);
				}
				else {
					dataSet=AccountModules.GetAccount(stmt.PatNum,stmt);
				}
			}
			CreatePdf(sheet,fullFileName,stmt,dataSet,medLab);
		}

		///<summary>Creates a file where fullFileName is a local path. If fullFileName is in the A to Z folder, pass in fullFileName as a temp file
		///and then upload that file to the cloud.</summary>
		public static void CreatePdf(Sheet sheet,string fullFileName,Statement stmt,DataSet dataSet,MedLab medLab,Patient pat=null,Patient patGuar=null) {
			Sheets.SetPageMargin(sheet,_printMargin);
			_stmt=stmt;
			_medLab=medLab;
			_isPrinting=true;
			_yPosPrint=0;
			PdfDocument document=new PdfDocument();
			foreach(SheetField field in sheet.SheetFields) {//validate all signatures before modifying any of the text fields.
				if(field.FieldType!= SheetFieldType.SigBox) {
					continue;
				}
				field.SigKey=Sheets.GetSignatureKey(sheet);
			}
			if(stmt!=null) {//Because most of the time statements do not display the Fill Sheet window, so there is no opportunity to select a combobox.
				SheetUtil.SetDefaultValueForComboBoxes(sheet);
			}
			//this will set the page breaks as well as adjust for growth behavior
			SheetUtil.CalculateHeights(sheet,dataSet,_stmt,_isPrinting,_printMargin.Top,_printMargin.Bottom,_medLab,pat,patGuar);
			int pageCount=Sheets.CalculatePageCount(sheet,_printMargin);
			for(int i=0;i<pageCount;i++) {
				_pagesPrinted=i;
				PdfPage page=document.AddPage();
				CreatePdfPage(sheet,page,dataSet,pat: pat,patGuar: patGuar);
			}
			document.Save(fullFileName);
			_isPrinting=false;
			GC.Collect();//We are done creating the pdf so we can forcefully clean up all the objects and controls that were used.
		}

		///<summary>Called for every page that is generated for a PDF docuemnt. Pages and yPos must be tracked outside of this function. See also pd_PrintPage.
		///DataSet should be prefilled with AccountModules.GetAccount() before calling this method if making a statement.</Summary>
		public static void CreatePdfPage(Sheet sheet,PdfPage page,DataSet dataSet,Patient pat=null,Patient patGuar=null) {
			page.Width=p(sheet.Width);//XUnit.FromInch((double)sheet.Width/100);  //new XUnit((double)sheet.Width/100,XGraphicsUnit.Inch);
			page.Height=p(sheet.Height);//new XUnit((double)sheet.Height/100,XGraphicsUnit.Inch);
			if(sheet.IsLandscape){
				page.Orientation=PageOrientation.Landscape;
			}
			Sheets.SetPageMargin(sheet,_printMargin);
			XGraphics gx=XGraphics.FromPdfPage(page);
			gx.SmoothingMode=XSmoothingMode.HighQuality;
			foreach(SheetField field in sheet.SheetFields) {
				if(!fieldOnCurPageHelper(field,sheet,_printMargin,_yPosPrint)) { 
					continue; 
				}
				switch(field.FieldType) {
					case SheetFieldType.Image:
					case SheetFieldType.PatImage:
						drawFieldImage(field,null,gx);
						break;
					case SheetFieldType.Drawing:
						drawFieldDrawing(field,null,gx);
						break;
					case SheetFieldType.Rectangle:
						drawFieldRectangle(field,null,gx);
						break;
					case SheetFieldType.Line:
						drawFieldLine(field,null,gx);
						break;
					case SheetFieldType.Special:
						drawFieldSpecial(sheet,field,null,gx);
						break;
					case SheetFieldType.Grid:
						drawFieldGrid(field,sheet,null,gx,dataSet,_stmt,_medLab,pat: pat,patGuar: patGuar);
						break;
					case SheetFieldType.InputField:
					case SheetFieldType.OutputText:
					case SheetFieldType.StaticText:
						drawFieldText(field,sheet,null,gx);
						break;
					case SheetFieldType.CheckBox:
						drawFieldCheckBox(field,null,gx);
						break;
					case SheetFieldType.ComboBox:
						drawFieldComboBox(field,sheet,null,gx);
						break;
					case SheetFieldType.ScreenChart:
						drawFieldScreenChart(field,sheet,null,gx);
						break;
					case SheetFieldType.SigBox:
						drawFieldSigBox(field,sheet,null,gx);
						break;
					case SheetFieldType.Parameter:
					default:
						//Parameter or possibly new field type.
						break;
				}
			}//end foreach SheetField
			drawHeader(sheet,null,gx);
			drawFooter(sheet,null,gx);
			gx.Dispose();
			gx=null;
			#region Set variables for next page to be printed
			_yPosPrint+=sheet.HeightPage-(_printMargin.Bottom+_printMargin.Top);//move _yPosPrint down equal to the amount of printable area per page.
			_pagesPrinted++;
			if(_pagesPrinted<Sheets.CalculatePageCount(sheet,_printMargin)) {
				//More pages need to be created for this pdf.  Do not manipulate _yPosPrint and simply continue.
			}
			else {//we are printing the last page of the current sheet.
				_yPosPrint=0;
				_pagesPrinted=0;
				_sheetsPrinted++;
			}
			#endregion
		}

		///<summary>Draws all images from the sheet onto the graphic passed in.  Used when printing and rendering the sheet fill edit window.</summary>
		public static void DrawImages(Sheet sheet,Graphics graphic,bool drawAll=false) {
			DrawImages(sheet,graphic,null,drawAll);
		}

		///<summary>Draws all images from the sheet onto the graphic passed in.  Used when printing, exporting to pdfs, or rendering the sheet fill edit window.  graphic should be null for pdfs and xgraphic should be null for printing and rendering the sheet fill edit window.</summary>
		private static void DrawImages(Sheet sheet,Graphics graphic,XGraphics xGraphic,bool drawAll=false) {
			Sheets.SetPageMargin(sheet,_printMargin);
			Bitmap bmpOriginal=null;
			if(drawAll){// || _forceSinglePage) {//reset _yPosPrint because we are drawing all.
				_yPosPrint=0;
			}
			foreach(SheetField field in sheet.SheetFields) {
				if(!drawAll ){//&& !_forceSinglePage) {
					if(field.YPos<_yPosPrint) {
						continue; //skip if on previous page
					}
					if(field.Bounds.Bottom>_yPosPrint+sheet.HeightPage-_printMargin.Bottom
						&& field.YPos!= _yPosPrint+_printMargin.Top) {
						break; //Skip if on next page
					} 
				}
				if(field.Height==0 || field.Width==0) {
					continue;//might be possible with really old sheets.
				}
				#region Get the path for the image
				string filePathAndName="";
				switch(field.FieldType) {
					case SheetFieldType.Image:
						filePathAndName=ODFileUtils.CombinePaths(SheetUtil.GetImagePath(),field.FieldName);
						break;
					case SheetFieldType.PatImage:
						if(field.FieldValue=="") {
							//There is no document object to use for display, but there may be a baked in image and that situation is dealt with below.
							filePathAndName="";
							break;
						}
						Document patDoc=Documents.GetByNum(PIn.Long(field.FieldValue));
						List<string> paths=Documents.GetPaths(new List<long> { patDoc.DocNum },ImageStore.GetPreferredAtoZpath());
						if(paths.Count < 1) {//No path was found so we cannot draw the image.
							continue;
						}
						filePathAndName=paths[0];
						break;
					default:
						//not an image field
						continue;
				}
				#endregion
				#region Load the image into bmpOriginal
				if(field.FieldName=="Patient Info.gif") {
					bmpOriginal=OpenDentBusiness.Properties.Resources.Patient_Info;
				}
				else if(CloudStorage.IsCloudStorage) {
					FormProgress FormP=new FormProgress();
					FormP.DisplayText=Lan.g(CloudStorage.LanThis,"Downloading...");
					FormP.NumberFormat="F";
					FormP.NumberMultiplication=1;
					FormP.MaxVal=100;//Doesn't matter what this value is as long as it is greater than 0
					FormP.TickMS=1000;
					//FormP.DoHideCancel=true;//So that patients using the kiosk don't mess up their forms.
					OpenDentalCloud.Core.TaskStateDownload state=CloudStorage.DownloadAsync(SheetUtil.GetImagePath(),field.FieldName,
					new OpenDentalCloud.ProgressHandler(FormP.OnProgress));
					if(FormP.ShowDialog()==DialogResult.Cancel) {
						state.DoCancel=true;
						return;
					}
					if(state==null || state.FileContent==null) {
						continue;//Unable to download the image
					}
					else {
						using(MemoryStream stream=new MemoryStream(state.FileContent)) {
							try {
								bmpOriginal=new Bitmap(Image.FromStream(stream));
							}
							catch(Exception ex) {
								ex.DoNothing();
								continue;//If the image is not an actual image file, leave the image field blank.
							}
						}
					}
				}
				else if(File.Exists(filePathAndName)) {//Local AtoZ
					try {
						bmpOriginal=new Bitmap(filePathAndName);
					}
					catch {
						continue;//If the image is not an actual image file, leave the image field blank.
					}
				}
				else {
					continue;
				}
				#endregion
				#region Calculate the image ratio and location, set values for imgDrawWidth and imgDrawHeight
				//inscribe image in field while maintaining aspect ratio.
				float imgRatio=(float)bmpOriginal.Width/(float)bmpOriginal.Height;
				float fieldRatio=(float)field.Width/(float)field.Height;
				float imgDrawHeight=field.Height;//drawn size of image
				float imgDrawWidth=field.Width;//drawn size of image
				int adjustY=0;//added to YPos
				int adjustX=0;//added to XPos
				//For patient images, we need to make sure the images will fit and can maintain aspect ratio.
				if(field.FieldType==SheetFieldType.PatImage && imgRatio>fieldRatio) {//image is too wide
					//X pos and width of field remain unchanged
					//Y pos and height must change
					imgDrawHeight=(float)bmpOriginal.Height*((float)field.Width/(float)bmpOriginal.Width);//img.Height*(width based scale) This also handles images that are too small.
					adjustY=(int)((field.Height-imgDrawHeight)/2f);//adjustY= half of the unused vertical field space
				}
				else if(field.FieldType==SheetFieldType.PatImage && imgRatio<fieldRatio) {//image is too tall
					//X pos and width must change
					//Y pos and height remain unchanged
					imgDrawWidth=(float)bmpOriginal.Width*((float)field.Height/(float)bmpOriginal.Height);//img.Height*(width based scale) This also handles images that are too small.
					adjustX=(int)((field.Width-imgDrawWidth)/2f);//adjustY= half of the unused horizontal field space
				}
				else {//image ratio == field ratio
					//do nothing
				}
				#endregion
				#region Draw the image
				if(xGraphic!=null) {//Drawing an image to a pdf.
					XImage xI=XImage.FromGdiPlusImage((Bitmap)bmpOriginal.Clone());
					xGraphic.DrawImage(xI,p(field.XPos+adjustX),p(field.YPos-_yPosPrint+adjustY),p(imgDrawWidth),p(imgDrawHeight));
					if(xI!=null) {//should always happen
						xI.Dispose();
						xI=null;
					}
				}
				else if(graphic!=null) {//Drawing an image to a printer or the sheet fill edit window.
					graphic.DrawImage(bmpOriginal,field.XPos+adjustX,field.YPos+adjustY-_yPosPrint,imgDrawWidth,imgDrawHeight);
				}
				#endregion
			}
			if(bmpOriginal!=null) {
				bmpOriginal.Dispose();
				bmpOriginal=null;
			}
		}
		
		///<summary>Deprecated: See GraphicsHelper.PixelsToPoints().  Converts pixels used by us to points used by PdfSharp.</summary>
		private static double p(int pixels){
			XUnit xunit=XUnit.FromInch((double)pixels/100d);//100 ppi
			return xunit.Point;
				//XUnit.FromInch((double)pixels/100);
		}

		///<summary>Deprecated: See GraphicsHelper.PixelsToPoints().  Converts pixels used by us to points used by PdfSharp.</summary>
		private static double p(float pixels){
			XUnit xunit=XUnit.FromInch((double)pixels/100d);//100 ppi
			return xunit.Point;
		}
		
	}
}
