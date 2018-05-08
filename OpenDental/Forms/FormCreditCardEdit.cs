using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormCreditCardEdit:ODForm {
		private Patient PatCur;
		private List<PayPlan> PayPlanList;
		private CreditCard _creditCardOld;
		public CreditCard CreditCardCur;
		///<summary>True if X-Charge is enabled.  Recurring charge section will show if using X-Charge.</summary>
		private bool _isXChargeEnabled;
		///<summary>True if PayConnect is enabled.  Recurring charge section will show if using PayConnect.</summary>
		private bool _isPayConnectEnabled;
		///<summary>True if PaySimple is enabled.  Recurring charge section will show if using PaySimple.</summary>
		private bool _isPaySimpleEnabled;

		public FormCreditCardEdit(Patient pat) {
			InitializeComponent();
			Lan.F(this);
			PatCur=pat;
			_isXChargeEnabled=Programs.IsEnabled(ProgramName.Xcharge);
			_isPayConnectEnabled=Programs.IsEnabled(ProgramName.PayConnect);
			_isPaySimpleEnabled=Programs.IsEnabled(ProgramName.PaySimple);
		}

		private void FormCreditCardEdit_Load(object sender,EventArgs e) {
			_creditCardOld=CreditCardCur.Clone();
			FillData();
			checkExcludeProcSync.Checked=CreditCardCur.ExcludeProcSync;
			if((_isXChargeEnabled || _isPayConnectEnabled || _isPaySimpleEnabled) 
				&& !CreditCardCur.IsXWeb()) 
			{//Get recurring payment plan information if using X-Charge or PayConnect and the card is not from XWeb.
				PayPlanList=PayPlans.GetValidPlansNoIns(PatCur.PatNum);
				List<PayPlanCharge> chargeList=PayPlanCharges.GetForPayPlans(PayPlanList.Select(x => x.PayPlanNum).ToList());
				comboPaymentPlans.Items.Add("None");
				comboPaymentPlans.SelectedIndex=0;
				for(int i=0;i<PayPlanList.Count;i++) {
					comboPaymentPlans.Items.Add(PayPlans.GetTotalPrinc(PayPlanList[i].PayPlanNum,chargeList).ToString("F")
					+"  "+Patients.GetPat(PayPlanList[i].PatNum).GetNameFL());
					if(PayPlanList[i].PayPlanNum==CreditCardCur.PayPlanNum) {
						comboPaymentPlans.SelectedIndex=i+1;
					}
				}
				if(PrefC.IsODHQ) {
					groupProcedures.Visible=true;
					FillProcs();
				}
				else {
					this.ClientSize=new System.Drawing.Size(this.ClientSize.Width,this.ClientSize.Height-144);
				}
			}
			else {//This will hide the recurring section and change the window size.
				groupRecurringCharges.Visible=false;
				this.ClientSize=new System.Drawing.Size(this.ClientSize.Width,this.ClientSize.Height-356);
			}
			if(_isPaySimpleEnabled && !CreditCardCur.IsNew) {
				textCardNumber.ReadOnly=true;
			}
			Plugins.HookAddCode(this,"FormCreditCardEdit.Load_end",PatCur);
		}

		private void FillData() {
			if(!CreditCardCur.IsNew) {
				string ccNum=CreditCardCur.CCNumberMasked;
				if(Regex.IsMatch(ccNum,"^\\d{12}(\\d{0,7})")) {	//Credit cards can have a minimum of 12 digits, maximum of 19
					int idxLast4Digits=(ccNum.Length-4);
					ccNum=(new string('X',12))+ccNum.Substring(idxLast4Digits);//replace the first 12 with 12 X's
				}
				textCardNumber.Text=ccNum;
				textAddress.Text=CreditCardCur.Address;
				if(CreditCardCur.CCExpiration.Year>1800) {
					textExpDate.Text=CreditCardCur.CCExpiration.ToString("MMyy");
				}
				textZip.Text=CreditCardCur.Zip;
				if(_isXChargeEnabled || _isPayConnectEnabled || _isPaySimpleEnabled) {//Only fill information if using X-Charge, PayConnect, or PaySimple.
					if(CreditCardCur.ChargeAmt>0) {
						textChargeAmt.Text=CreditCardCur.ChargeAmt.ToString("F");
					}
					if(CreditCardCur.DateStart.Year>1880) {
						textDateStart.Text=CreditCardCur.DateStart.ToShortDateString();
					}
					if(CreditCardCur.DateStop.Year>1880) {
						textDateStop.Text=CreditCardCur.DateStop.ToShortDateString();
					}
					textNote.Text=CreditCardCur.Note;
				}
			}
		}

		private void FillProcs() {
			listProcs.Items.Clear();
			if(String.IsNullOrEmpty(CreditCardCur.Procedures)) {
				return;
			}
			string[] arrayProcCodes=CreditCardCur.Procedures.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries);
			for(int i=0;i<arrayProcCodes.Length;i++) {
				listProcs.Items.Add(arrayProcCodes[i]+"- "+ProcedureCodes.GetLaymanTerm(ProcedureCodes.GetProcCode(arrayProcCodes[i]).CodeNum));
			}
		}

		private bool VerifyData() {
			if(textCardNumber.Text.Trim().Length<5) {
				MsgBox.Show(this,"Invalid Card Number.");
				return false;
			}
			try {
				if(Regex.IsMatch(textExpDate.Text,@"^\d\d[/\- ]\d\d$")) {//08/07 or 08-07 or 08 07
					CreditCardCur.CCExpiration=new DateTime(Convert.ToInt32("20"+textExpDate.Text.Substring(3,2)),Convert.ToInt32(textExpDate.Text.Substring(0,2)),1);
				}
				else if(Regex.IsMatch(textExpDate.Text,@"^\d{4}$")) {//0807
					CreditCardCur.CCExpiration=new DateTime(Convert.ToInt32("20"+textExpDate.Text.Substring(2,2)),Convert.ToInt32(textExpDate.Text.Substring(0,2)),1);
				}
				else {
					MsgBox.Show(this,"Expiration format invalid.");
					return false;
				}
			}
			catch {
				MsgBox.Show(this,"Expiration format invalid.");
				return false;
			}
			if(textDateStop.Text.Trim()!="" && PIn.Date(textDateStart.Text)>PIn.Date(textDateStop.Text)) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"The recurring charge start date is after the stop date.  Continue?")) {
					return false;
				}
			}
			if(_isXChargeEnabled || _isPayConnectEnabled || _isPaySimpleEnabled) {//Only validate recurring setup if using X-Charge, PayConnect, or PaySimple.
				if(textDateStart.errorProvider1.GetError(textDateStart)!=""
					|| textDateStop.errorProvider1.GetError(textDateStop)!=""
					|| textChargeAmt.errorProvider1.GetError(textChargeAmt)!="")
				{
					MsgBox.Show(this,"Please fix data entry errors first.");
					return false;
				}
				if((textChargeAmt.Text=="" && comboPaymentPlans.SelectedIndex>0)
					|| (textChargeAmt.Text=="" && textDateStart.Text.Trim()!=""))
				{
					MsgBox.Show(this,"You need a charge amount for recurring charges.");
					return false;
				}
				if(textChargeAmt.Text!="" && textDateStart.Text.Trim()=="") {
					MsgBox.Show(this,"You need a start date for recurring charges.");
					return false;
				}
			}
			return true;
		}

		private void butClear_Click(object sender,EventArgs e) {
			//Only clear text boxes for recurring charges group.
			textChargeAmt.Text="";
			textDateStart.Text="";
			textDateStop.Text="";
			textNote.Text="";
		}

		private void butToday_Click(object sender,EventArgs e) {
			textDateStart.Text=DateTime.Today.ToShortDateString();
		}

		private void butAddProc_Click(object sender,EventArgs e) {
			FormProcCodes FormP=new FormProcCodes();
			FormP.IsSelectionMode=true;
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK) {
				return;
			}
			string procCode=ProcedureCodes.GetStringProcCode(FormP.SelectedCodeNum);
			List<string> procsOnCards=CreditCardCur.Procedures.Split(new string[] { "," },StringSplitOptions.RemoveEmptyEntries).ToList();
			//If the procedure is already attached to this card, return without adding the procedure again
			if(procsOnCards.Exists(x => x==procCode)) {
				return;
			}
			//Warn if attached to a different active card for this patient
			if(CreditCards.ProcLinkedToCard(CreditCardCur.PatNum,procCode,CreditCardCur.CreditCardNum)) {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"This procedure is already linked with another credit card on this patient's "
					+"account. Adding the procedure to this card will result in the patient being charged twice for this procedure. Add this procedure?")) {
					return;
				}
			}
			if(CreditCardCur.Procedures!="") {
				CreditCardCur.Procedures+=",";
			}
			CreditCardCur.Procedures+=procCode;
			FillProcs();
		}

		private void butRemoveProc_Click(object sender,EventArgs e) {
			if(listProcs.SelectedIndex==-1) {
				MsgBox.Show(this,"Please select a procedure first.");
				return;
			}
			List<string> strList=new List<string>(CreditCardCur.Procedures.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries));
			strList.RemoveAt(listProcs.SelectedIndex);
			CreditCardCur.Procedures=string.Join(",",strList);
			FillProcs();
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(CreditCardCur.IsNew) {
				DialogResult=DialogResult.Cancel;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Are you sure you want to delete this credit card?")) {
				return;
			}
			#region X-Charge
			//Delete the archived X-Charge token 
			if(_isXChargeEnabled && CreditCardCur.XChargeToken!="") { 
				if(CreditCardCur.IsXWeb()) {
					OpenDentBusiness.WebTypes.Shared.XWeb.XWebs.DeleteCreditCard(PatCur.PatNum,CreditCardCur.CreditCardNum);//Also deletes cc from db
				}
				else {
					DeleteXChargeAlias();
				}				
			}
			#endregion
			CreditCards.Delete(CreditCardCur.CreditCardNum);
			List<CreditCard> creditCards=CreditCards.Refresh(PatCur.PatNum);
			for(int i=0;i<creditCards.Count;i++) {
				creditCards[i].ItemOrder=creditCards.Count-(i+1);
				CreditCards.Update(creditCards[i]);//Resets ItemOrder.
			}
			DialogResult=DialogResult.OK;
		}
		
		private void DeleteXChargeAlias() {
			Program prog=Programs.GetCur(ProgramName.Xcharge);
			string path=Programs.GetProgramPath(prog);
			if(prog==null) {
				MsgBox.Show(this,"X-Charge entry is missing from the database.");//should never happen
				return;
			}
			if(!prog.Enabled) {
				if(Security.IsAuthorized(Permissions.Setup)) {
					FormXchargeSetup FormX=new FormXchargeSetup();
					FormX.ShowDialog();
				}
				return;
			}
			if(!File.Exists(path)) {
				MsgBox.Show(this,"Path is not valid.");
				if(Security.IsAuthorized(Permissions.Setup)) {
					FormXchargeSetup FormX=new FormXchargeSetup();
					FormX.ShowDialog();
				}
				return;
			}
			ProcessStartInfo info=new ProcessStartInfo(path);
			string resultfile=PrefC.GetRandomTempFile("txt");
			try {
				File.Delete(resultfile);//delete the old result file.
			}
			catch {
				MsgBox.Show(this,"Could not delete XResult.txt file.  It may be in use by another program, flagged as read-only, or you might not have "
					+"sufficient permissions.");
				return;
			}
			string xUsername=ProgramProperties.GetPropVal(prog.ProgramNum,"Username",Clinics.ClinicNum);
			string xPassword=CodeBase.MiscUtils.Decrypt(ProgramProperties.GetPropVal(prog.ProgramNum,"Password",Clinics.ClinicNum));
			info.Arguments+="/TRANSACTIONTYPE:ARCHIVEVAULTDELETE ";
			info.Arguments+="/XCACCOUNTID:"+CreditCardCur.XChargeToken+" ";
			info.Arguments+="/RESULTFILE:\""+resultfile+"\" ";
			info.Arguments+="/USERID:"+xUsername+" ";
			info.Arguments+="/PASSWORD:"+xPassword+" ";
			info.Arguments+="/AUTOPROCESS ";
			info.Arguments+="/AUTOCLOSE ";
			Cursor=Cursors.WaitCursor;
			Process process=new Process();
			process.StartInfo=info;
			process.EnableRaisingEvents=true;
			process.Start();
			while(!process.HasExited) {
				Application.DoEvents();
			}
			Thread.Sleep(200);//Wait 2/10 second to give time for file to be created.
			Cursor=Cursors.Default;
			string line="";
			try {
				using(TextReader reader = new StreamReader(resultfile)) {
					line=reader.ReadLine();
					while(line!=null) {
						if(line=="RESULT=SUCCESS") {
							break;
						}
						if(line.StartsWith("DESCRIPTION") && !line.Contains("Alias does not exist")) {//If token doesn't exist in X-Charge, still delete from OD
							MsgBox.Show(this,"There was a problem deleting this card within X-Charge.  Please try again.");
							return;//Don't delete the card from OD
						}
						line=reader.ReadLine();
					}
				}
			}
			catch {
				MsgBox.Show(this,"Could not read XResult.txt file.  It may be in use by another program, flagged as read-only, or you might not have "
					+"sufficient permissions.");
				return;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!VerifyData()) {
				return;
			}
			CreditCardCur.ExcludeProcSync=checkExcludeProcSync.Checked;
			CreditCardCur.Address=textAddress.Text;
			CreditCardCur.CCNumberMasked=textCardNumber.Text;
			CreditCardCur.PatNum=PatCur.PatNum;
			CreditCardCur.Zip=textZip.Text;
			if(_isXChargeEnabled || _isPayConnectEnabled || _isPaySimpleEnabled) {//Only update recurring if using X-Charge, PayConnect,or PaySimple.
				CreditCardCur.ChargeAmt=PIn.Double(textChargeAmt.Text);
				CreditCardCur.DateStart=PIn.Date(textDateStart.Text);
				CreditCardCur.DateStop=PIn.Date(textDateStop.Text);
				CreditCardCur.Note=textNote.Text;
				if(comboPaymentPlans.SelectedIndex>0) {
					CreditCardCur.PayPlanNum=PayPlanList[comboPaymentPlans.SelectedIndex-1].PayPlanNum;
				}
				else {
					CreditCardCur.PayPlanNum=0;//Allows users to change from a recurring payplan charge to a normal one.
				}
			}
			if(CreditCardCur.IsNew) {
				List<CreditCard> itemOrderCount=CreditCards.Refresh(PatCur.PatNum);
				CreditCardCur.ItemOrder=itemOrderCount.Count;
				CreditCardCur.CCSource=CreditCardSource.None;
				CreditCardCur.ClinicNum=Clinics.ClinicNum;
				CreditCards.Insert(CreditCardCur);
			}
			else {
				#region X-Charge
				//Special logic for had a token and changed number or expiration date X-Charge
				if(_isXChargeEnabled && CreditCardCur.XChargeToken!="" &&
					(_creditCardOld.CCNumberMasked!=CreditCardCur.CCNumberMasked || _creditCardOld.CCExpiration!=CreditCardCur.CCExpiration)) 
				{ 
					Program prog=Programs.GetCur(ProgramName.Xcharge);
					string path=Programs.GetProgramPath(prog);
					if(prog==null){
						MsgBox.Show(this,"X-Charge entry is missing from the database.");//should never happen
						return;
					}
					if(!prog.Enabled){
						if(Security.IsAuthorized(Permissions.Setup)){
							FormXchargeSetup FormX=new FormXchargeSetup();
							FormX.ShowDialog();
						}
						return;
					}
					if(!File.Exists(path)){
						MsgBox.Show(this,"Path is not valid.");
						if(Security.IsAuthorized(Permissions.Setup)){
							FormXchargeSetup FormX=new FormXchargeSetup();
							FormX.ShowDialog();
						}
						return;
					}
					//Either update the exp date or update credit card number by deleting archive so new token can be created next time it's used.
					ProcessStartInfo info=new ProcessStartInfo(path);
					string resultfile=PrefC.GetRandomTempFile("txt");
					try {
						File.Delete(resultfile);//delete the old result file.
					}
					catch {
						MsgBox.Show(this,"Could not delete XResult.txt file.  It may be in use by another program, flagged as read-only, or you might not have sufficient permissions.");
						return;
					}
					string xUsername=ProgramProperties.GetPropVal(prog.ProgramNum,"Username",Clinics.ClinicNum);
					string xPassword=CodeBase.MiscUtils.Decrypt(ProgramProperties.GetPropVal(prog.ProgramNum,"Password",Clinics.ClinicNum));
					if(_creditCardOld.CCNumberMasked!=CreditCardCur.CCNumberMasked) {//User changed card number, which invlidates the X-Charge token
						//delete archived token, a new one will be created the next time the card is charged.
						info.Arguments+="/TRANSACTIONTYPE:ARCHIVEVAULTDELETE ";
						info.Arguments+="/XCACCOUNTID:"+CreditCardCur.XChargeToken+" ";
						info.Arguments+="/RESULTFILE:\""+resultfile+"\" ";
						info.Arguments+="/USERID:"+xUsername+" ";
						info.Arguments+="/PASSWORD:"+xPassword+" ";
						info.Arguments+="/AUTOPROCESS ";
						info.Arguments+="/AUTOCLOSE ";
						CreditCardCur.XChargeToken="";//Clear the XChargeToken in our db.
						//To match PaySimple, this should be enhanced to validate the cc number and get a new token.
					}
					else {//We can only change exp date for X-Charge via ARCHIVEAULTUPDATE.
						info.Arguments+="/TRANSACTIONTYPE:ARCHIVEVAULTUPDATE ";
						info.Arguments+="/XCACCOUNTID:"+CreditCardCur.XChargeToken+" ";
						if(CreditCardCur.CCExpiration!=null && CreditCardCur.CCExpiration.Year>2005) {
							info.Arguments+="/EXP:"+CreditCardCur.CCExpiration.ToString("MMyy")+" ";
						}
						info.Arguments+="/RESULTFILE:\""+resultfile+"\" ";
						info.Arguments+="/USERID:"+xUsername+" ";
						info.Arguments+="/PASSWORD:"+xPassword+" ";
						info.Arguments+="/AUTOPROCESS ";
						info.Arguments+="/AUTOCLOSE ";
					}
					Cursor=Cursors.WaitCursor;
					Process process=new Process();
					process.StartInfo=info;
					process.EnableRaisingEvents=true;
					process.Start();
					while(!process.HasExited) {
						Application.DoEvents();
					}
					Thread.Sleep(200);//Wait 2/10 second to give time for file to be created.
					Cursor=Cursors.Default;
					string resulttext="";
					string line="";
					try {
						using(TextReader reader=new StreamReader(resultfile)) {
							line=reader.ReadLine();
							while(line!=null) {
								if(resulttext!="") {
									resulttext+="\r\n";
								}
								resulttext+=line;
								if(line.StartsWith("RESULT=")) {
									if(line!="RESULT=SUCCESS") {
										CreditCardCur=CreditCards.GetOne(CreditCardCur.CreditCardNum);
										FillData();
										return;
									}
								}
								line=reader.ReadLine();
							}
						}
					}
					catch {
						MsgBox.Show(this,"There was a problem creating or editing this card with X-Charge.  Please try again.");
						return;
					}
				}//End of special token logic
				#endregion
				#region PayConnect
				//Special logic for had a token and changed number or expiration date PayConnect
				//We have to compare the year and month of the expiration instead of just comparing expirations because the X-Charge logic stores the
				//expiration day of the month as the 1st in the db, but it makes more sense to set the expriation day of month to the last day in that month.
				//Since we only want to invalidate the PayConnect token if the expiration month or year is different, we will ignore any difference in day.
				if(_isPayConnectEnabled && CreditCardCur.PayConnectToken!=""
					&& (_creditCardOld.CCNumberMasked!=CreditCardCur.CCNumberMasked
						|| _creditCardOld.CCExpiration.Year!=CreditCardCur.CCExpiration.Year
						|| _creditCardOld.CCExpiration.Month!=CreditCardCur.CCExpiration.Month))
				{
					//if the number or expiration is changed, the token is no longer valid, so clear the token and token expiration so a new one can be
					//generated the next time a payment is processed using this card.
					CreditCardCur.PayConnectToken="";
					CreditCardCur.PayConnectTokenExp=DateTime.MinValue;
					//To match PaySimple, this should be enhanced to validate the cc number and get a new token.
				}
				#endregion
				#region PaySimple
				//Special logic for had a token and changed number or expiration date PaySimple
				//We have to compare the year and month of the expiration instead of just comparing expirations because the X-Charge logic stores the
				//expiration day of the month as the 1st in the db, but it makes more sense to set the expriation day of month to the last day in that month.
				//Since we only want to invalidate the PayConnect token if the expiration month or year is different, we will ignore any difference in day.
				if(_isPaySimpleEnabled && CreditCardCur.PaySimpleToken!=""
					&& (_creditCardOld.Zip!=CreditCardCur.Zip
						|| _creditCardOld.CCExpiration.Year!=CreditCardCur.CCExpiration.Year
						|| _creditCardOld.CCExpiration.Month!=CreditCardCur.CCExpiration.Month))
				{
					//TODO: Open form to have user enter the CC number.  Then make API call to update cc instead of wiping out token.
					//If the billing zip or the expiration changes, the token is invalid and they need to get a new one.
					CreditCardCur.PaySimpleToken="";
				}
				#endregion
				CreditCards.Update(CreditCardCur);
			}
			DialogResult=DialogResult.OK;
		}

	}
}