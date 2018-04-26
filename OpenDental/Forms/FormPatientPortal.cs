using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Globalization;

namespace OpenDental {
	public partial class FormPatientPortal:ODForm {
		private Patient _patCur;
		///<summary>The current UserWeb instance from the passed in Patient.</summary>
		private UserWeb _userWebCur;
		///<summary>The unmodified UserWeb instance to compare to the current one when saving changes to the database.</summary>
		private UserWeb _userWebOld;
		///<summary>Keeps track if the user printed the patient's information.  Mainly used to show a reminder when the password changes and the user didn't print.</summary>
		private bool _wasPrinted;
		private bool _isNew;

		public FormPatientPortal(Patient patCur) {
			InitializeComponent();
			_patCur=patCur;
		}

		private void FormPatientPortal_Load(object sender,EventArgs e) {
			_userWebCur=UserWebs.GetByFKeyAndType(_patCur.PatNum,UserWebFKeyType.PatientPortal);
			if(_userWebCur==null) {
				_isNew=true;
				_userWebCur=new UserWeb();
				_userWebCur.UserName=UserWebs.CreateUserNameFromPat(_patCur,UserWebFKeyType.PatientPortal);
				_userWebCur.FKey=_patCur.PatNum;
				_userWebCur.FKeyType=UserWebFKeyType.PatientPortal;
				_userWebCur.RequireUserNameChange=true;
				_userWebCur.Password="";
				UserWebs.Insert(_userWebCur);
			}
			_userWebOld=_userWebCur.Copy();
			textOnlineUsername.Text=_userWebCur.UserName;
			textOnlinePassword.Text="";
			if(_userWebCur.Password!="") {//if a password was already filled in
				butGiveAccess.Text="Remove Online Access";
				//We do not want to show the password hash that is stored in the database so we will fill the online password with asterisks.
				textOnlinePassword.Text="********";
				textOnlinePassword.ReadOnly=false;
				textOnlineUsername.ReadOnly=false;
			}
			textPatientPortalURL.Text=PrefC.GetString(PrefName.PatientPortalURL);
		}

		private void menuItemSetup_Click(object sender,EventArgs e) {
			FormEServicesSetup formPPS=new FormEServicesSetup(FormEServicesSetup.EService.PatientPortal);
			formPPS.ShowDialog();
			textPatientPortalURL.Text=PrefC.GetString(PrefName.PatientPortalURL);
		}

		private void butGiveAccess_Click(object sender,EventArgs e) {
			if(butGiveAccess.Text=="Provide Online Access") {//When form open opens with a blank password
				if(PrefC.GetString(PrefName.PatientPortalURL)=="") {
					//User probably hasn't set up the patient portal yet.
					MsgBox.Show(this,"Patient Facing URL is required to be set before granting online access.  Click Setup to set the Patient Facing URL.");
					return;
				}
				string error;
				if(!UserWebs.ValidatePatientAccess(_patCur,out error)) { 
					MessageBox.Show(error);
					return;
				}
				Cursor=Cursors.WaitCursor;
				//1. Fill password.
				string passwordGenerated=UserWebs.GenerateRandomPassword(8);
				textOnlinePassword.Text=passwordGenerated;
				//2. Make the username and password editable in case they want to change it.
				textOnlineUsername.ReadOnly=false;
				textOnlinePassword.ReadOnly=false;
				//3. Save password to db.
				// We only save the hash of the generated password.
				string passwordHashed=Userods.HashPassword(passwordGenerated,false);
				_userWebCur.Password=passwordHashed;
				UserWebs.Update(_userWebCur,_userWebOld);
				_userWebOld.Password=passwordHashed;//Update _userWebOld in case the user changes password manually.
				//4. Insert EhrMeasureEvent
				EhrMeasureEvent newMeasureEvent=new EhrMeasureEvent();
				newMeasureEvent.DateTEvent=DateTime.Now;
				newMeasureEvent.EventType=EhrMeasureEventType.OnlineAccessProvided;
				newMeasureEvent.PatNum=_userWebCur.FKey;
				newMeasureEvent.MoreInfo="";
				EhrMeasureEvents.Insert(newMeasureEvent);
				//5. Rename button
				butGiveAccess.Text="Remove Online Access";
				Cursor=Cursors.Default;
			}
			else {//remove access
				Cursor=Cursors.WaitCursor;
				//1. Clear password
				textOnlinePassword.Text="";
				//2. Make in uneditable
				textOnlinePassword.ReadOnly=true;
				//3. Save password to db
				_userWebCur.Password=textOnlinePassword.Text;
				UserWebs.Update(_userWebCur,_userWebOld);
				_userWebOld.Password=textOnlinePassword.Text;//Update PatOld in case the user changes password manually.
				//4. Rename button
				butGiveAccess.Text="Provide Online Access";
				Cursor=Cursors.Default;
			}
		}

		private void butOpen_Click(object sender,EventArgs e) {
			if(textPatientPortalURL.Text=="") {
				MessageBox.Show("Please use Setup to set the Online Access Link first.");
				return;
			}
			try {
				System.Diagnostics.Process.Start(textPatientPortalURL.Text);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
		
		private void butGenerate_Click(object sender,EventArgs e) {
			if(textOnlinePassword.ReadOnly) {
				MessageBox.Show("Please use the Provide Online Access button first.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			string passwordGenerated=UserWebs.GenerateRandomPassword(8);
			textOnlinePassword.Text=passwordGenerated;
			// We only save the hash of the generated password.
			string passwordHashed=Userods.HashPassword(passwordGenerated,false);
			_userWebCur.Password=passwordHashed;
			UserWebs.Update(_userWebCur,_userWebOld);
			_userWebOld.Password=passwordHashed;//Update PatOld in case the user changes password manually.
			Cursor=Cursors.Default;
		}

		private void butPrint_Click(object sender,EventArgs e) {
			if(textPatientPortalURL.Text=="") {
				MsgBox.Show(this,"Online Access Link required. Please use Setup to set the Online Access Link first.");
				return;
			}
			if(textOnlinePassword.Text=="" || textOnlinePassword.Text=="********") {
				MessageBox.Show("Password required. Please generate a new password.");
				return;
			}
			string error=Patients.IsPortalPasswordValid(textOnlinePassword.Text);
			if(error!="") {//Non-empty string means it was invalid.
				MessageBox.Show(this,error);
				return;
			}
			_wasPrinted=true;
			//Then, print the info that the patient will be given in order for them to log in online.
			PrintPatientInfo();
		}

		private void PrintPatientInfo() {
			PrintDocument pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			try {
				#if DEBUG
				FormRpPrintPreview pView = new FormRpPrintPreview();
				pView.printPreviewControl2.Document=pd;
				pView.ShowDialog();
				#else
						if(PrinterL.SetPrinter(pd,PrintSituation.Default,_patCur.PatNum,"Patient portal login information printed")) {
							pd.Print();
						}
				#endif
			}
			catch {
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
		}

		private void pd_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			//new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font font=new Font("Arial",10,FontStyle.Regular);
			int yPos=bounds.Top+100;
			int center=bounds.X+bounds.Width/2;
			text="Online Access";
			g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,font).Width/2,yPos);
			yPos+=50;
			text="Website: "+textPatientPortalURL.Text;
			g.DrawString(text,font,Brushes.Black,bounds.Left+100,yPos);
			yPos+=25;
			text="Username: "+textOnlineUsername.Text;
			g.DrawString(text,font,Brushes.Black,bounds.Left+100,yPos);
			yPos+=25;
			text="Password: "+textOnlinePassword.Text;
			g.DrawString(text,font,Brushes.Black,bounds.Left+100,yPos);
			g.Dispose();
			e.HasMorePages=false;
		}

		private void butOK_Click(object sender,EventArgs e) {
			bool shouldUpdateUserWeb=false;
			bool shouldPrint=false;
			if(textOnlineUsername.ReadOnly==false) {
				if(textOnlineUsername.Text=="") {
					MsgBox.Show(this,"Online Username cannot be blank.");
					return;
				}
				else if(_userWebCur.UserName!=textOnlineUsername.Text) {
					if(UserWebs.UserNameExists(textOnlineUsername.Text,UserWebFKeyType.PatientPortal)) {
						MsgBox.Show(this,"The Online Username already exists.");
						return;
					}
					_userWebCur.UserName=textOnlineUsername.Text;
					shouldUpdateUserWeb=true;
					if(!_wasPrinted) {
						shouldPrint=true;
					}
				}
			}
			if(textOnlinePassword.Text!="" && textOnlinePassword.Text!="********") {
				string error=Patients.IsPortalPasswordValid(textOnlinePassword.Text);
				if(error!="") {//Non-empty string means it was invalid.
					MessageBox.Show(this,error);
					return;
				}
				if(!_wasPrinted) {
					shouldPrint=true;
				}
				shouldUpdateUserWeb=true;
				_userWebCur.Password=Userods.HashPassword(textOnlinePassword.Text,false);
			}
			if(shouldPrint) {
				DialogResult result=MessageBox.Show(Lan.g(this,"Online Username or Password changed but was not printed, would you like to print?")
					,Lan.g(this,"Print Patient Info")
					,MessageBoxButtons.YesNoCancel);
				if(result==DialogResult.Yes) {
					//Print the showing information.
					PrintPatientInfo();
				}
				else if(result==DialogResult.No) {
					//User does not want to print.  Do nothing.
				}
				else if(result==DialogResult.Cancel) {
					return;
				}
			}
			if(shouldUpdateUserWeb) {
				UserWebs.Update(_userWebCur,_userWebOld);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			if(_isNew) {
				UserWebs.Delete(_userWebCur.UserWebNum);
			}
			DialogResult=DialogResult.Cancel;
		}

	

		

		

		

	

	}
}
