using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.HL7;
using System.Threading;
using CodeBase;

namespace OpenDental {
	public partial class FormWebForms:ODForm {
		
		/// <summary>
		/// This Form does 3 things: 
		/// 1) Retrieve data of filled out web forms from a web service and convert them into sheets and patients. Using the first name, last name and birth date it will check for existing patients. If an existing patient is found a new sheet is created. If no patient is found, a  patient and a sheet is created.
		/// 2) Send a list of the Sheets that have been created to the Server for deletion.
		/// 3)Show all the sheets that have been created in 1) using a date filter.
		/// </summary>
		public FormWebForms() {
			InitializeComponent();
			gridMain.ContextMenu=menuWebFormsRight;
			Lan.F(this);
		}

		private void FormWebForms_Load(object sender,EventArgs e) {
			textDateStart.Text=DateTime.Today.ToShortDateString();
			textDateEnd.Text=DateTime.Today.ToShortDateString();
			FillGrid();
		}

		/// <summary>
		/// </summary>
		private void FillGrid() {
			DateTime dateFrom=DateTimeOD.Today;
			DateTime dateTo=DateTimeOD.Today;
			try {
				dateFrom=PIn.Date(textDateStart.Text);//handles blank
				if(textDateEnd.Text!=""){//if it is blank, default to today
					dateTo=PIn.Date(textDateEnd.Text);
				}
			}
			catch{
				MsgBox.Show(this,"Invalid date");
				return;
			}
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Date"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Time"),42);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Patient Last Name"),110);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Patient First Name"),110);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Description"),240);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Deleted"),0,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			DataTable table=Sheets.GetWebFormSheetsTable(dateFrom,dateTo,Clinics.ClinicNum);
			for(int i=0;i<table.Rows.Count;i++) {
				long patNum=PIn.Long(table.Rows[i]["PatNum"].ToString());
				long sheetNum=PIn.Long(table.Rows[i]["SheetNum"].ToString());
				Patient pat=Patients.GetPat(patNum);
				if(pat!=null) {
					ODGridRow row=new ODGridRow();
					row.Cells.Add(table.Rows[i]["date"].ToString());
					row.Cells.Add(table.Rows[i]["time"].ToString());
					row.Cells.Add(pat.LName);
					row.Cells.Add(pat.FName);
					row.Cells.Add(table.Rows[i]["description"].ToString());
					row.Cells.Add(table.Rows[i]["IsDeleted"].ToString()=="0" ? "" : "X");
					row.Tag=sheetNum;
					gridMain.Rows.Add(row);
				}
			} 
			gridMain.EndUpdate();
		}

		private void RetrieveAndSaveData() {
			try {
				#if DEBUG
				//IgnoreCertificateErrors();// used with faulty certificates only while debugging.
				#endif
				WebSheets.Sheets wh=new WebSheets.Sheets();
				wh.Timeout=300000;//5 minutes.  Default is 100000 (1.66667 minutes).
				wh.Url=PrefC.GetString(PrefName.WebHostSynchServerURL);
				string RegistrationKey=PrefC.GetString(PrefName.RegistrationKey);
				if(wh.GetDentalOfficeID(RegistrationKey)==0) {
					MsgBox.Show(this,"Registration key provided by the dental office is incorrect");
					return;
				}
				List<WebSheets.SheetAndSheetField> listSheets;
				List<long> listSkippedSheets=new List<long>();
				int iterations=0;
				do {//Because WebSheets are only downloaded 20 at a time, we need to get the sheets within a loop to download them all.
					listSheets=wh.GetSheets(RegistrationKey) //Only gets the first 20 sheets.
						//if we are not in HQ, filter out non-HQ sheets that don't match our current clinic
						.Where(x => x.web_sheet.ClinicNum==0 || Clinics.ClinicNum==0 || x.web_sheet.ClinicNum==Clinics.ClinicNum).ToList();
					iterations++;
					List<long> SheetsForDeletion=new List<long>();
					listSheets.RemoveAll(x => listSkippedSheets.Contains(x.web_sheet.SheetID));//Remove all sheets that the user has already skipped.
					if(listSheets.Count==0) {
						if(iterations==1) {
							MsgBox.Show(this,"No Patient forms retrieved from server");
						}
						else {
							MsgBox.Show(this,"All Patient forms retrieved from server");
						}
						return;
					}
					//loop through all incoming sheets
					for(int i=0;i<listSheets.Count;i++) {
						try { //this try catch is put so that a defective downloaded sheet does not stop other sheets from being downloaded.
							long patNum=0;
							string lName="";
							string fName="";
							List<string> listPhoneNumbers=new List<string>();
							string email="";
							DateTime bDate=DateTime.MinValue;
							//loop through each field in this sheet to find First name, last name, and DOB
							for(int j=0;j<listSheets[i].web_sheetfieldlist.Count();j++) {
								if(listSheets[i].web_sheetfieldlist[j].FieldName.ToLower().Contains("lname")
									|| listSheets[i].web_sheetfieldlist[j].FieldName.ToLower().Contains("lastname")) {
									lName=listSheets[i].web_sheetfieldlist[j].FieldValue;
								}
								if(listSheets[i].web_sheetfieldlist[j].FieldName.ToLower().Contains("fname")
									|| listSheets[i].web_sheetfieldlist[j].FieldName.ToLower().Contains("firstname")) {
									fName=listSheets[i].web_sheetfieldlist[j].FieldValue;
								}
								if(listSheets[i].web_sheetfieldlist[j].FieldName.ToLower().Contains("bdate")
									|| listSheets[i].web_sheetfieldlist[j].FieldName.ToLower().Contains("birthdate")) {
									bDate=PIn.Date(listSheets[i].web_sheetfieldlist[j].FieldValue);
								}
								if(listSheets[i].web_sheetfieldlist[j].FieldName.ToLower().In("hmphone","wkphone","wirelessphone")
									&& listSheets[i].web_sheetfieldlist[j].FieldValue != "") 
								{
									listPhoneNumbers.Add(listSheets[i].web_sheetfieldlist[j].FieldValue);
								}
								if(listSheets[i].web_sheetfieldlist[j].FieldName.ToLower()=="email") {
									email=listSheets[i].web_sheetfieldlist[j].FieldValue;
								}
							}
							if(bDate.Year<1880) {
								//log invalid birth date  format. Shouldn't happen, though.
							}
							List<long> listMatchingPats=Patients.GetPatNumsByNameBirthdayEmailAndPhone(lName,fName,bDate,email,listPhoneNumbers);
							FormPatientPickWebForm FormPpw=new FormPatientPickWebForm(listSheets[i]);
							FormPpw.LnameEntered=lName;
							FormPpw.FnameEntered=fName;
							FormPpw.BdateEntered=bDate;
							if(listMatchingPats.Count==0) {
								FormPpw.HasMoreThanOneMatch=false;
								FormPpw.ShowDialog();
								if(FormPpw.DialogResult==DialogResult.Cancel) {
									//user wants to stop importing altogether
									//we will pick up where we left off here next time
									wh.DeleteSheetData(RegistrationKey,SheetsForDeletion.ToArray());
									return;
								}
								else if(FormPpw.DialogResult==DialogResult.Ignore) {
									//user wants to skip this patient import only
									listSkippedSheets.Add(listSheets[i].web_sheet.SheetID);
									continue;
									//future feature suggestion... 4th state = discard
									//mark this patient's import sheet for delete go to the next patient
									//SheetsForDeletion.Add(listSheets[i].web_sheet.SheetID);
									//continue
								}
								patNum=FormPpw.SelectedPatNum;//might be zero to indicate new patient
							}
							else if(listMatchingPats.Count>1) {
								FormPpw.HasMoreThanOneMatch=true;
								FormPpw.ShowDialog();
								if(FormPpw.DialogResult==DialogResult.Cancel) {
									//user wants to stop importing altogether
									//we will pick up where we left off here next time
									wh.DeleteSheetData(RegistrationKey,SheetsForDeletion.ToArray());
									return;
								}
								else if(FormPpw.DialogResult==DialogResult.Ignore) {
									//user wants to skip this patient import only
									listSkippedSheets.Add(listSheets[i].web_sheet.SheetID);
									continue;
								}
								patNum=FormPpw.SelectedPatNum;//might be zero to indicate new patient
							}
							else {//Exactly one match was found so make a log entry what the match was.
								patNum=listMatchingPats[0];
								Patient pat=Patients.GetPat(patNum);
								//Security log for OD automatically importing a sheet into a patient.
								SecurityLogs.MakeLogEntry(Permissions.SheetEdit,patNum,Lan.g(this,"Web form import from:")
									+" "+lName+", "+fName+" "+bDate.ToShortDateString()+"\r\n"
									+Lan.g(this,"Auto imported into:")+" "+pat.LName+", "+pat.FName+" "+pat.Birthdate.ToShortDateString());
							}
							if(patNum==0) {
								Patient newPat=CreatePatient(lName,fName,bDate,listSheets[i]);
								patNum=newPat.PatNum;
								//Security log for user creating a new patient.
								SecurityLogs.MakeLogEntry(Permissions.SheetEdit,patNum,Lan.g(this,"Web form import from:")
									+" "+lName+", "+fName+" "+bDate.ToShortDateString()+"\r\n"
									+Lan.g(this,"User created new pat:")+" "+newPat.LName+", "+newPat.FName+" "+newPat.Birthdate.ToShortDateString());
							}
							//We should probably make a security log entry for a manually selected patient.
							Sheet newSheet=SheetUtil.CreateSheetFromWebSheet(patNum,listSheets[i]);
							Sheets.SaveNewSheet(newSheet);
							if(DataExistsInDb(newSheet)) {
								SheetsForDeletion.Add(listSheets[i].web_sheet.SheetID);
							}
						}
						catch(Exception e) {
							MessageBox.Show(e.Message);
						}
					}// end of for loop
					wh.DeleteSheetData(RegistrationKey,SheetsForDeletion.ToArray());
				} while(listSheets.Count>0);
			} 
			catch(Exception e) {
				MessageBox.Show(e.Message);
				return;
			}
		}

		/// <summary>Compares values of the sheet with values that have been inserted into the db.  Returns false if the data was not saved properly.</summary>
		private bool DataExistsInDb(Sheet sheet) {
			bool dataExistsInDb=true;
			if(sheet!=null) {
				long SheetNum=sheet.SheetNum;
				Sheet sheetFromDb=Sheets.GetSheet(SheetNum);
				if(sheetFromDb!=null) {
					dataExistsInDb=CompareSheets(sheetFromDb,sheet);
				}
			}
			return dataExistsInDb;
		}

		/// <summary>
		///  This method is used only for testing with security certificates that has problems.
		/// </summary>
		private void IgnoreCertificateErrors() {
			///the line below will allow the code to continue by not throwing an exception.
			///It will accept the security certificate if there is a problem with the security certificate.
			System.Net.ServicePointManager.ServerCertificateValidationCallback+=
			delegate(object sender,System.Security.Cryptography.X509Certificates.X509Certificate certificate,
									System.Security.Cryptography.X509Certificates.X509Chain chain,
									System.Net.Security.SslPolicyErrors sslPolicyErrors) {
				///do stuff here and return true or false accordingly.
				///In this particular case it always returns true i.e accepts any certificate.
				/* sample code 
				if(sslPolicyErrors==System.Net.Security.SslPolicyErrors.None) return true;
				// the sample below allows expired certificates
				foreach(X509ChainStatus s in chain.ChainStatus) {
					// allows expired certificates
					if(string.Equals(s.Status.ToString(),"NotTimeValid",
						StringComparison.OrdinalIgnoreCase)) {
						return true;
					}						
				}*/
				return true;
			};
		}

		/// <summary>
		/// </summary>
		private Patient CreatePatient(String LastName,String FirstName,DateTime birthDate,WebSheets.SheetAndSheetField sAnds) {
			Patient newPat=new Patient();
			newPat.LName=LastName;
			newPat.FName=FirstName;
			newPat.Birthdate=birthDate;
			newPat.ClinicNum=sAnds.web_sheet.ClinicNum;
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				//Set the patients primary provider to the practice default provider.
				newPat.PriProv=Providers.GetDefaultProvider().ProvNum;
			}
			else {//Using clinics.
				//Set the patients primary provider to the clinic default provider.
				newPat.PriProv=Providers.GetDefaultProvider(Clinics.ClinicNum).ProvNum;
			}
			Type t=newPat.GetType();
			FieldInfo[] fi=t.GetFields();
			foreach(FieldInfo field in fi) {
				// find match for fields in Patients in the web_sheetfieldlist
				var WebSheetFieldList=sAnds.web_sheetfieldlist.Where(sf=>sf.FieldName.ToLower()==field.Name.ToLower());
				if(WebSheetFieldList.Count()>0) {
					// this loop is used to fill a field that may generate mutiple values for a single field in the patient.
					//for example the field gender has 2 eqivalent sheet fields in the web_sheetfieldlist
					for(int i=0;i<WebSheetFieldList.Count();i++) {
						WebSheets.webforms_sheetfield sf=WebSheetFieldList.ElementAt(i);
						String SheetWebFieldValue=sf.FieldValue;
						String RadioButtonValue=sf.RadioButtonValue;
						FillPatientFields(newPat,field,SheetWebFieldValue,RadioButtonValue);
					}
				}
			}
			try{
				Patients.Insert(newPat,false);
				SecurityLogs.MakeLogEntry(Permissions.PatientCreate,newPat.PatNum,"Created from Web Forms.");
				//set Guarantor field the same as PatNum
				Patient patOld=newPat.Copy();
				newPat.Guarantor=newPat.PatNum;
				Patients.Update(newPat,patOld);
				//If there is an existing HL7 def enabled, send an ADT message if there is an outbound ADT message defined
				if(HL7Defs.IsExistingHL7Enabled()) {
					MessageHL7 messageHL7=MessageConstructor.GenerateADT(newPat,newPat,EventTypeHL7.A04);//patient is guarantor
					//Will be null if there is no outbound ADT message defined, so do nothing
					if(messageHL7!=null) {
						HL7Msg hl7Msg=new HL7Msg();
						hl7Msg.AptNum=0;
						hl7Msg.HL7Status=HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
						hl7Msg.MsgText=messageHL7.ToString();
						hl7Msg.PatNum=newPat.PatNum;
						HL7Msgs.Insert(hl7Msg);
#if DEBUG
						MessageBox.Show(this,messageHL7.ToString());
#endif
					}
				}
			}
			catch(Exception e) {
				gridMain.EndUpdate();
				MessageBox.Show(e.Message);
			}
			return newPat;
		}

		/// <summary>
		/// </summary>
		private Sheet CreateSheet(long PatNum,WebSheets.SheetAndSheetField sAnds) {
			Sheet newSheet=null;
			try{
				SheetDef sheetDef=new SheetDef((SheetTypeEnum)sAnds.web_sheet.SheetType);
					newSheet=SheetUtil.CreateSheet(sheetDef,PatNum);
					SheetParameter.SetParameter(newSheet,"PatNum",PatNum);
					newSheet.DateTimeSheet=sAnds.web_sheet.DateTimeSheet;
					newSheet.Description=sAnds.web_sheet.Description;
					newSheet.Height=sAnds.web_sheet.Height;
					newSheet.Width=sAnds.web_sheet.Width;
					newSheet.FontName=sAnds.web_sheet.FontName;
					newSheet.FontSize=sAnds.web_sheet.FontSize;
					newSheet.SheetType=(SheetTypeEnum)sAnds.web_sheet.SheetType;
					newSheet.IsLandscape=sAnds.web_sheet.IsLandscape==(sbyte)1?true:false;
					newSheet.InternalNote="";
					newSheet.IsWebForm=true;
					//loop through each variable in a single sheetfield
					for(int i=0;i<sAnds.web_sheetfieldlist.Count();i++) {
						SheetField sheetfield=new SheetField();
						sheetfield.FieldName=sAnds.web_sheetfieldlist[i].FieldName;
						sheetfield.FieldType=(SheetFieldType)sAnds.web_sheetfieldlist[i].FieldType;
						//sheetfield.FontIsBold=sAnds.web_sheetfieldlist[i].FontIsBold==(sbyte)1?true:false;
						if(sAnds.web_sheetfieldlist[i].FontIsBold==(sbyte)1) {
							sheetfield.FontIsBold=true;
						}else{
							sheetfield.FontIsBold=false;
						}
						sheetfield.FontIsBold=sAnds.web_sheetfieldlist[i].FontIsBold==(sbyte)1?true:false;
						sheetfield.FontName=sAnds.web_sheetfieldlist[i].FontName;
						sheetfield.FontSize=sAnds.web_sheetfieldlist[i].FontSize;
						sheetfield.Height=sAnds.web_sheetfieldlist[i].Height;
						sheetfield.Width=sAnds.web_sheetfieldlist[i].Width;
						sheetfield.XPos=sAnds.web_sheetfieldlist[i].XPos;
						sheetfield.YPos=sAnds.web_sheetfieldlist[i].YPos;
						//sheetfield.IsRequired=sAnds.web_sheetfieldlist[i].IsRequired==(sbyte)1?true:false;
						if(sAnds.web_sheetfieldlist[i].IsRequired==(sbyte)1) {
							sheetfield.IsRequired=true;
						}
						else {
							sheetfield.IsRequired=false;
						}
						sheetfield.TabOrder=sAnds.web_sheetfieldlist[i].TabOrder;
						sheetfield.ReportableName=sAnds.web_sheetfieldlist[i].ReportableName;
						sheetfield.RadioButtonGroup=sAnds.web_sheetfieldlist[i].RadioButtonGroup;
						sheetfield.RadioButtonValue=sAnds.web_sheetfieldlist[i].RadioButtonValue;
						sheetfield.GrowthBehavior=(GrowthBehaviorEnum)sAnds.web_sheetfieldlist[i].GrowthBehavior;
						sheetfield.FieldValue=sAnds.web_sheetfieldlist[i].FieldValue;
						sheetfield.TextAlign=(HorizontalAlignment)sAnds.web_sheetfieldlist[i].TextAlign;
						sheetfield.ItemColor=Color.FromArgb(sAnds.web_sheetfieldlist[i].ItemColor);
						newSheet.SheetFields.Add(sheetfield);
					}// end of j loop
					Sheets.SaveNewSheet(newSheet);
					return newSheet;
			}
			catch(Exception e) {
				gridMain.EndUpdate();
				MessageBox.Show(e.Message);
			}
			return newSheet;
		}

		/// <summary>
		/// </summary>
		private void FillPatientFields(Patient newPat,FieldInfo field,String SheetWebFieldValue,String RadioButtonValue) {
			try {
				switch(field.Name) {
					case "Birthdate":
						DateTime birthDate=PIn.Date(SheetWebFieldValue);
						field.SetValue(newPat,birthDate);
						break;
					case "Gender":
						if(RadioButtonValue=="Male") {
							if(SheetWebFieldValue=="X") {
								field.SetValue(newPat,PatientGender.Male);
							}
						}
						if(RadioButtonValue=="Female") {
							if(SheetWebFieldValue=="X") {
								field.SetValue(newPat,PatientGender.Female);
							}
						}
						break;
					case "Position":
						if(RadioButtonValue=="Married") {
							if(SheetWebFieldValue=="X") {
								field.SetValue(newPat,PatientPosition.Married);
							}
						}
						if(RadioButtonValue=="Single") {
							if(SheetWebFieldValue=="X") {
								field.SetValue(newPat,PatientPosition.Single);
							}
						}
						break;
					case "PreferContactMethod":
					case "PreferConfirmMethod":
					case "PreferRecallMethod":
						if(RadioButtonValue=="HmPhone") {
							if(SheetWebFieldValue=="X") {
								field.SetValue(newPat,ContactMethod.HmPhone);
							}
						}
						if(RadioButtonValue=="WkPhone") {
							if(SheetWebFieldValue=="X") {
								field.SetValue(newPat,ContactMethod.WkPhone);
							}
						}
						if(RadioButtonValue=="WirelessPh") {
							if(SheetWebFieldValue=="X") {
								field.SetValue(newPat,ContactMethod.WirelessPh);
							}
						}
						if(RadioButtonValue=="Email") {
							if(SheetWebFieldValue=="X") {
								field.SetValue(newPat,ContactMethod.Email);
							}
						}
						break;
					case "StudentStatus":
						if(RadioButtonValue=="Nonstudent") {
							if(SheetWebFieldValue=="X") {
								field.SetValue(newPat,"");
							}
						}
						if(RadioButtonValue=="Fulltime") {
							if(SheetWebFieldValue=="X") {
								field.SetValue(newPat,"F");
							}
						}
						if(RadioButtonValue=="Parttime") {
							if(SheetWebFieldValue=="X") {
								field.SetValue(newPat,"P");
							}
						}
						break;
					case "ins1Relat":
					case "ins2Relat":
						if(RadioButtonValue=="Self") {
							if(SheetWebFieldValue=="X") {
								field.SetValue(newPat,Relat.Self);
							}
						}
						if(RadioButtonValue=="Spouse") {
							if(SheetWebFieldValue=="X") {
								field.SetValue(newPat,Relat.Spouse);
							}
						}
						if(RadioButtonValue=="Child") {
							if(SheetWebFieldValue=="X") {
								field.SetValue(newPat,Relat.Child);
							}
						}
					break;
					default:
						field.SetValue(newPat,SheetWebFieldValue);
					break;
				}//switch case
			}
			catch(Exception e) {
				gridMain.EndUpdate();
				MessageBox.Show(field.Name+e.Message);
			}
		}

		///<summary>This is not a generic sheet comparer.  It is actually a web form sheet field comparer.  Returns false if any sheet fields that the web form cares about are not equal.</summary>
		private bool CompareSheets(Sheet sheetFromDb,Sheet newSheet) {
			//the 2 sheets are sorted before comparison because in some cases SheetFields[i] refers to a different field in sheetFromDb than in newSheet
			Sheet sortedSheetFromDb=new Sheet();
			Sheet sortedNewSheet=new Sheet();
			sortedSheetFromDb.SheetFields=sheetFromDb.SheetFields.OrderBy(sf => sf.SheetFieldNum).ToList();
			sortedNewSheet.SheetFields=newSheet.SheetFields.OrderBy(sf => sf.SheetFieldNum).ToList();
			for(int i=0;i<sortedSheetFromDb.SheetFields.Count;i++) {
				//Explicitly compare the sheet field values that can be imported via web forms.
				//This makes it so that any future columns added to the sheetfield table will not affect this comparer.
				//When new columns are added, we can now decide on a per column basis if the column matters for comparisons.
				//We will always add new columns below and simply comment them out if we do not want to use them for comparison, this way we know that the column was considered.
				if(sortedSheetFromDb.SheetFields[i].SheetNum!=sortedNewSheet.SheetFields[i].SheetNum
					|| sortedSheetFromDb.SheetFields[i].FieldType!=sortedNewSheet.SheetFields[i].FieldType
					|| sortedSheetFromDb.SheetFields[i].FieldName!=sortedNewSheet.SheetFields[i].FieldName
					|| sortedSheetFromDb.SheetFields[i].FieldValue!=sortedNewSheet.SheetFields[i].FieldValue
					|| sortedSheetFromDb.SheetFields[i].FontSize!=sortedNewSheet.SheetFields[i].FontSize
					|| sortedSheetFromDb.SheetFields[i].FontName!=sortedNewSheet.SheetFields[i].FontName
					|| sortedSheetFromDb.SheetFields[i].FontIsBold!=sortedNewSheet.SheetFields[i].FontIsBold
					|| sortedSheetFromDb.SheetFields[i].XPos!=sortedNewSheet.SheetFields[i].XPos
					|| sortedSheetFromDb.SheetFields[i].YPos!=sortedNewSheet.SheetFields[i].YPos
					|| sortedSheetFromDb.SheetFields[i].Width!=sortedNewSheet.SheetFields[i].Width
					|| sortedSheetFromDb.SheetFields[i].Height!=sortedNewSheet.SheetFields[i].Height
					|| sortedSheetFromDb.SheetFields[i].GrowthBehavior!=sortedNewSheet.SheetFields[i].GrowthBehavior
					|| sortedSheetFromDb.SheetFields[i].RadioButtonValue!=sortedNewSheet.SheetFields[i].RadioButtonValue
					|| sortedSheetFromDb.SheetFields[i].RadioButtonGroup!=sortedNewSheet.SheetFields[i].RadioButtonGroup
					|| sortedSheetFromDb.SheetFields[i].IsRequired!=sortedNewSheet.SheetFields[i].IsRequired
					|| sortedSheetFromDb.SheetFields[i].TabOrder!=sortedNewSheet.SheetFields[i].TabOrder
					|| sortedSheetFromDb.SheetFields[i].ReportableName!=sortedNewSheet.SheetFields[i].ReportableName
					|| sortedSheetFromDb.SheetFields[i].TextAlign!=sortedNewSheet.SheetFields[i].TextAlign
					|| sortedSheetFromDb.SheetFields[i].ItemColor!=sortedNewSheet.SheetFields[i].ItemColor
					) 
				{
					return false;//No need to keep looping, we know the sheets are not equal at this point.
				}
			}
			return true;//All web form sheet fields are equal.
		}

		private void butRetrieve_Click(object sender,System.EventArgs e) {
			if(textDateStart.errorProvider1.GetError(textDateStart)!=""
				|| textDateEnd.errorProvider1.GetError(textDateEnd)!=""
				) {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			//this.backgroundWorker1.RunWorkerAsync(); call this  method if theread is to be used later.
			RetrieveAndSaveData(); // if a thread is used this method will go into backgroundWorker1_DoWork
			FillGrid(); // if a thread is used this method will go into backgroundWorker1_RunWorkerCompleted
			Cursor=Cursors.Default;
		}

		private void butToday_Click(object sender,EventArgs e) {
			textDateStart.Text=DateTime.Today.ToShortDateString();
			textDateEnd.Text=DateTime.Today.ToShortDateString();
			FillGrid();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void menuItemSetup_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			try {
				FormWebFormSetup formW=new FormWebFormSetup();
				formW.ShowDialog();
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Web Forms Setup");
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			long sheetNum=(long)gridMain.Rows[e.Row].Tag;
			Sheet sheet=Sheets.GetSheet(sheetNum);
			GotoModule.GotoFamily(sheet.PatNum);
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			long sheetNum=(long)gridMain.Rows[e.Row].Tag;
			Sheet sheet=Sheets.GetSheet(sheetNum);
			FormSheetFillEdit.ShowForm(sheet,delegate { FillGrid(); }); //We must refresh the grid because the web form clicked might have been deleted by the user.
		}

		/*
		private void menuItemViewSheet_Click(object sender,EventArgs e) {
			long sheetNum=(long)gridMain.Rows[gridMain.SelectedIndices[0]].Tag;
			Sheet sheet=Sheets.GetSheet(sheetNum);
			FormSheetFillEdit FormSF=new FormSheetFillEdit(sheet);
			FormSF.ShowDialog();
		}

		private void menuItemImportSheet_Click(object sender,EventArgs e) {
			long sheetNum=(long)gridMain.Rows[gridMain.SelectedIndices[0]].Tag;
			Sheet sheet=Sheets.GetSheet(sheetNum);
			FormSheetImport formSI=new FormSheetImport();
			formSI.SheetCur=sheet;
			formSI.ShowDialog();
		}*/

		private void menuWebFormsRight_Popup(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				menuItemViewAllSheets.Visible=false;
			}
			else {
				menuItemViewAllSheets.Visible=true;
			}
		}

		private void menuItemViewAllSheets_Click(object sender,EventArgs e) {
			long sheetNum=(long)gridMain.Rows[gridMain.SelectedIndices[0]].Tag;
			Sheet sheet=Sheets.GetSheet(sheetNum);
			FormPatientForms formP=new FormPatientForms();
			formP.PatNum=sheet.PatNum;
			formP.ShowDialog();
		}

		private void butCancel_Click(object sender,EventArgs e) {
			Close();
		}

	









		

	











	}
}