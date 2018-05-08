using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using CodeBase;
using MigraDoc.DocumentObjectModel;

namespace OpenDental {
	public partial class FormPaySimple:ODForm {

		///<summary>Only to be used outside of the form.  
		///Set on OK_Click to discourage using it in the form.
		///Contains the important details of what happened with PaySimple</summary>
		public PaySimple.ApiResponse ApiResponseOut;

		private Patient _patCur;
		private MagstripCardParser _parser=null;
		private PaySimple.TransType _trantype=PaySimple.TransType.SALE;
		private CreditCard _creditCardCur;
		private bool _isAddingCard;
		private long _clinicNum;
		private Program _progCur;

		public FormPaySimple(long clinicNum,Patient pat,decimal amount,CreditCard creditCard,bool isAddingCard=false) {
			InitializeComponent();
			Lan.F(this);
			textAmount.Text=POut.Decimal(amount);
			_clinicNum=clinicNum;
			_patCur=pat;
			_creditCardCur=creditCard;
			_isAddingCard=isAddingCard;
		}

		private void FormPaySimple_Load(object sender,EventArgs e) {
			_progCur=Programs.GetCur(ProgramName.PaySimple);
			if(_progCur==null) {
				MsgBox.Show(this,"PaySimple does not exist in the database.");
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(_patCur==null || _patCur.PatNum==0) {//Prepaid card
				radioAuthorization.Enabled=false;
				checkOneTimePayment.Checked=true;
				checkOneTimePayment.Enabled=false;
			}
			else {
				checkOneTimePayment.Checked=!PrefC.GetBool(PrefName.StoreCCtokens);
				textZipCode.Text=_patCur.Zip;
				textNameOnCard.Text=_patCur.GetNameFL();
				if(_creditCardCur!=null) {
					FillFieldsFromCard();
				}
			}
			if(_isAddingCard) {
				radioAuthorization.Checked=true;
				_trantype=PaySimple.TransType.AUTH;
				groupTransType.Enabled=false;
				labelAmount.Visible=false;
				textAmount.Visible=false;
				checkOneTimePayment.Checked=false;
				checkOneTimePayment.Enabled=false;
			}
			textCardNumber.Select();
		}

		private void FillFieldsFromCard() {
			//User selected a credit card from drop down.
			if(_creditCardCur.CCNumberMasked!="") {
				string ccNum=_creditCardCur.CCNumberMasked;
				if(Regex.IsMatch(ccNum,"^\\d{12}(\\d{0,7})")) { //Minimum of 12 digits, maximum of 19
					int idxLast4Digits=(ccNum.Length-4);
					ccNum=(new string('X',12))+ccNum.Substring(idxLast4Digits);//replace the first 12 with 12 X's
				}
				textCardNumber.Text=ccNum;
			}
			if(_creditCardCur.CCExpiration!=null && _creditCardCur.CCExpiration.Year>2005) {
				textExpDate.Text=_creditCardCur.CCExpiration.ToString("MMyy");
			}
			if(_creditCardCur.Zip!="") {
				textZipCode.Text=_creditCardCur.Zip;
			}
			if(!string.IsNullOrWhiteSpace(_creditCardCur.PaySimpleToken)) {
				checkOneTimePayment.Checked=false;
				checkOneTimePayment.Enabled=false;
				textSecurityCode.ReadOnly=true;
				textZipCode.ReadOnly=true;
				textNameOnCard.ReadOnly=true;
				textCardNumber.ReadOnly=true;
				textExpDate.ReadOnly=true;
				radioAuthorization.Enabled=false;
			}
		}

		private void radioSale_Click(object sender,EventArgs e) {
			radioSale.Checked=true;
			radioAuthorization.Checked=false;
			radioVoid.Checked=false;
			radioReturn.Checked=false;
			textRefNumber.Visible=false;
			labelRefNumber.Visible=false;
			textAmount.Visible=true;
			_trantype=PaySimple.TransType.SALE;
			textCardNumber.Focus();//Usually transaction type is chosen before card number is entered, but textCardNumber box must be selected in order for card swipe to work.
		}

		private void radioAuthorization_Click(object sender,EventArgs e) {
			radioSale.Checked=false;
			radioAuthorization.Checked=true;
			radioVoid.Checked=false;
			radioReturn.Checked=false;
			textRefNumber.Visible=false;
			labelRefNumber.Visible=false;
			textAmount.Visible=false;
			_trantype=PaySimple.TransType.AUTH;
			textCardNumber.Focus();//Usually transaction type is chosen before card number is entered, but textCardNumber box must be selected in order for card swipe to work.
		}

		private void radioVoid_Click(object sender,EventArgs e) {
			radioSale.Checked=false;
			radioAuthorization.Checked=false;
			radioVoid.Checked=true;
			radioReturn.Checked=false;
			textRefNumber.Visible=true;
			labelRefNumber.Visible=true;
			labelRefNumber.Text=Lan.g(this,"Ref Number");
			textAmount.Visible=false;
			_trantype=PaySimple.TransType.VOID;
			textCardNumber.Focus();//Usually transaction type is chosen before card number is entered, but textCardNumber box must be selected in order for card swipe to work.
		}

		private void radioReturn_Click(object sender,EventArgs e) {
			radioSale.Checked=false;
			radioAuthorization.Checked=false;
			radioVoid.Checked=false;
			radioReturn.Checked=true;
			textRefNumber.Visible=true;
			labelRefNumber.Visible=true;
			labelRefNumber.Text=Lan.g(this,"Ref Number");
			textAmount.Visible=false;
			_trantype=PaySimple.TransType.RETURN;
			textCardNumber.Focus();//Usually transaction type is chosen before card number is entered, but textCardNumber box must be selected in order for card swipe to work.
		}

		private void textCardNumber_KeyPress(object sender,KeyPressEventArgs e) {
			if(String.IsNullOrEmpty(textCardNumber.Text)) {
				return;
			}
			if(textCardNumber.Text.StartsWith("%") && textCardNumber.Text.EndsWith("?") && e.KeyChar == 13) {
				e.Handled=true;
				ParseSwipedCard(textCardNumber.Text);
			}
		}

		private void ParseSwipedCard(string data) {
			Clear();
			try {
				_parser=new MagstripCardParser(data);
			}
			catch(MagstripCardParseException) {
				MessageBox.Show(this,"Could not read card, please try again.","Card Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
			if(_parser!=null) {
				textCardNumber.Text=_parser.AccountNumber;
				textExpDate.Text=_parser.ExpirationMonth.ToString().PadLeft(2,'0')+(_parser.ExpirationYear%100).ToString().PadLeft(2,'0');
				textNameOnCard.Text=_parser.FirstName+" "+_parser.LastName;
				GetNextControl(textNameOnCard,true).Focus();//Move forward to the next control in the tab order.
			}
		}

		private void Clear() {
			textCardNumber.Text="";
			textExpDate.Text="";
			textNameOnCard.Text="";
			textSecurityCode.Text="";
			textZipCode.Text="";
		}

		///<summary>Processes a PaySimple payment via the PaySimple API.</summary>
		private PaySimple.ApiResponse ProcessPayment(int expYear,int expMonth) {
			PaySimple.ApiResponse retVal=null;
			string refNumber="";
			if(_trantype==PaySimple.TransType.VOID || _trantype==PaySimple.TransType.RETURN) {
				refNumber=textRefNumber.Text;
			}
			string magData=null;
			if(_parser!=null) {
				magData=_parser.Track2;
			}
			string cardNumber=textCardNumber.Text;
			//if using a stored CC and there is an X-Charge token saved for the CC and the user enters the whole card number to get a PaySimple token
			//and the number entered doesn't have the same last 4 digits and exp date, then assume it's not the same card and clear out the X-Charge token.
			if(_creditCardCur!=null //using a saved CC
				&& !string.IsNullOrEmpty(_creditCardCur.XChargeToken) //there is an X-Charge token saved
				&& (cardNumber.Right(4)!=_creditCardCur.CCNumberMasked.Right(4) //the card number entered doesn't have the same last 4 digits
					|| expYear!=_creditCardCur.CCExpiration.Year //the card exp date entered doesn't have the same year
					|| expMonth!=_creditCardCur.CCExpiration.Month)) //the card exp date entered doesn't have the same month
			{
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"The card number or expiration date entered does not match the X-Charge card on file.  Do you wish "
					+"to replace the X-Charge card with this one?"))
				{
					_creditCardCur.XChargeToken="";
				}
				else {
					Cursor=Cursors.Default;
					return null;
				}
			}
			//if the user has chosen to store CC tokens and the stored CC has a token and the token is not expired,
			//then use it instead of the CC number and CC expiration.
			if(!checkOneTimePayment.Checked
				&& _creditCardCur!=null //if the user selected a saved CC
				&& !string.IsNullOrWhiteSpace(_creditCardCur.PaySimpleToken)) //there is a stored token for this card
			{
				cardNumber=_creditCardCur.PaySimpleToken;
				expYear=_creditCardCur.CCExpiration.Year;
				expMonth=_creditCardCur.CCExpiration.Month;
			}
			try {
				switch(_trantype) {
					case PaySimple.TransType.SALE:
						//If _patCur is null or the PatNum is 0, we will make a one time payment for an UNKNOWN patient.  This is currently only intended for prepaid insurance cards.
						retVal=PaySimple.MakePayment((_patCur==null ? 0 : _patCur.PatNum),_creditCardCur,PIn.Decimal(textAmount.Text),textCardNumber.Text
							,new DateTime(expYear,expMonth,1),checkOneTimePayment.Checked,textZipCode.Text,textSecurityCode.Text,_clinicNum);
						break;
					case PaySimple.TransType.AUTH:
						//Will retreive a new customer id from PaySimple if the patient doesn't exist already.
						long paySimpleCustomerId=PaySimple.GetCustomerIdForPat(_patCur.PatNum,_patCur.FName,_patCur.LName,_clinicNum);
						//I have no idea if an insurance can make an auth payment but incase they can I check for it.
						if(paySimpleCustomerId==0) {//Insurance payment, make a new customer id every time per Nathan on 04/26/2018
							if((_patCur==null || _patCur.PatNum==0)) {
								paySimpleCustomerId=PaySimple.AddCustomer("UNKNOWN","UNKNOWN","",_clinicNum);
							}
							else {
								throw new ODException(Lan.g(this,"Invalid PaySimple Customer Id found."));
							}
						}
						retVal=PaySimple.AddCreditCard(paySimpleCustomerId,textCardNumber.Text,new DateTime(expYear,expMonth,1),textZipCode.Text,_clinicNum);
						break;
					case PaySimple.TransType.RETURN:
						if(string.IsNullOrWhiteSpace(textRefNumber.Text)) {
							throw new ODException(Lan.g(this,"Invalid PaySimple Payment ID."));
						}
						if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"You are about to return a payment.  This action is irreversible.  Continue?")) {
							throw new ODException(Lan.g(this,"Payment return was cancelled by user."));
						}
						retVal=PaySimple.ReversePayment(textRefNumber.Text,_clinicNum);
						break;
					case PaySimple.TransType.VOID:
						if(string.IsNullOrWhiteSpace(textRefNumber.Text)) {
							throw new ODException(Lan.g(this,"Invalid PaySimple Payment ID."));
						}
						if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"You are about to void a payment.  This action is irreversible.  Continue?")) {
							throw new ODException(Lan.g(this,"Payment void was cancelled by user."));
						}
						retVal=PaySimple.VoidPayment(textRefNumber.Text,_clinicNum);
						break;
					default:
						throw new Exception("Invalid transmission type: "+_trantype.ToString());
				}
			}
			catch(ODException wex) {
				MessageBox.Show(wex.Message);//This should have already been Lans.g if applicable.
				return null;
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Error:")+" "+ex.Message);
				return null;
			}
			if(_trantype.In(PaySimple.TransType.SALE,PaySimple.TransType.RETURN,PaySimple.TransType.VOID)) {//Only print a receipt if transaction is an approved SALE, RETURN, or VOID			
				//The isSwiped boolean could be incorrectly set if the user swipes a card and then changes the data that was entered to a different card.
				retVal.BuildReceiptString(cardNumber,expMonth,expYear,textNameOnCard.Text,_clinicNum,_parser!=null);
				PrintReceipt(retVal.TransactionReceipt);
			}
			if(checkOneTimePayment.Checked) {//not storing the card token
				return retVal;
			}
			if(_creditCardCur==null) {//user selected Add new card from the payment window, save it or its token depending on settings
				_creditCardCur=new CreditCard();
				_creditCardCur.IsNew=true;
				_creditCardCur.PatNum=_patCur.PatNum;
				List<CreditCard> itemOrderCount=CreditCards.Refresh(_patCur.PatNum);
				_creditCardCur.ItemOrder=itemOrderCount.Count;
			}
			_creditCardCur.CCExpiration=new DateTime(expYear,expMonth,DateTime.DaysInMonth(expYear,expMonth));
			//I don't care about PrefName.StoreCCnumbers, I am always storing a masked number.
			_creditCardCur.CCNumberMasked=textCardNumber.Text.Right(4).PadLeft(textCardNumber.Text.Length,'X');
			_creditCardCur.Zip=textZipCode.Text;
			_creditCardCur.PaySimpleToken=retVal.PaySimpleToken;
			_creditCardCur.CCSource=CreditCardSource.PaySimple;
			if(_creditCardCur.IsNew) {
				_creditCardCur.ClinicNum=_clinicNum;
				_creditCardCur.Procedures=PrefC.GetString(PrefName.DefaultCCProcs);
				CreditCards.Insert(_creditCardCur);
			}
			else {
				CreditCards.Update(_creditCardCur);
			}
			return retVal;
		}

		private void PrintReceipt(string receiptStr) {
			string[] receiptLines=receiptStr.Split(new string[] { Environment.NewLine },StringSplitOptions.None);
			MigraDoc.DocumentObjectModel.Document doc=new MigraDoc.DocumentObjectModel.Document();
			doc.DefaultPageSetup.PageWidth=Unit.FromInch(3.0);
			doc.DefaultPageSetup.PageHeight=Unit.FromInch(0.181*receiptLines.Length+0.56);//enough to print receipt text plus 9/16 inch (0.56) extra space at bottom.
			doc.DefaultPageSetup.TopMargin=Unit.FromInch(0.25);
			doc.DefaultPageSetup.LeftMargin=Unit.FromInch(0.25);
			doc.DefaultPageSetup.RightMargin=Unit.FromInch(0.25);
			MigraDoc.DocumentObjectModel.Font bodyFontx=MigraDocHelper.CreateFont(8,false);
			bodyFontx.Name=FontFamily.GenericMonospace.Name;
			MigraDoc.DocumentObjectModel.Section section=doc.AddSection();
			Paragraph par=section.AddParagraph();
			ParagraphFormat parformat=new ParagraphFormat();
			parformat.Alignment=ParagraphAlignment.Left;
			parformat.Font=bodyFontx;
			par.Format=parformat;
			par.AddFormattedText(receiptStr,bodyFontx);
			MigraDoc.Rendering.Printing.MigraDocPrintDocument printdoc=new MigraDoc.Rendering.Printing.MigraDocPrintDocument();
			MigraDoc.Rendering.DocumentRenderer renderer=new MigraDoc.Rendering.DocumentRenderer(doc);
			renderer.PrepareDocument();
			printdoc.Renderer=renderer;
#if DEBUG
			FormRpPrintPreview pView=new FormRpPrintPreview();
			pView.printPreviewControl2.Document=printdoc;
			pView.ShowDialog();
#else
			try {
				if(PrinterL.SetPrinter(pd2,PrintSituation.Receipt,_patCur.PatNum,"PaySimple receipt printed")) {
					printdoc.PrinterSettings=pd2.PrinterSettings;
					printdoc.Print();
				}
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Printer not available.")+"\r\n"+Lan.g(this,"Original error")+": "+ex.Message);
			}
#endif
		}

		private bool VerifyData(out int expYear,out int expMonth) {
			expYear=0;
			expMonth=0;
			if(_trantype==PaySimple.TransType.SALE && !Regex.IsMatch(textAmount.Text,"^[0-9]+$") && !Regex.IsMatch(textAmount.Text,"^[0-9]*\\.[0-9]+$")) {
				MsgBox.Show(this,"Invalid amount.");
				return false;
			}
			if((_trantype==PaySimple.TransType.VOID || _trantype==PaySimple.TransType.RETURN)//The reference number is optional for terminal returns. 
				&& textRefNumber.Text=="") 
			{
				MsgBox.Show(this,"Ref Number required.");
				return false;
			}
			string paytype=ProgramProperties.GetPropValForClinicOrDefault(_progCur.ProgramNum,PaySimple.PropertyDescs.PaySimplePayType,_clinicNum);
			if(!Defs.GetDefsForCategory(DefCat.PaymentTypes,true).Any(x => x.DefNum.ToString()==paytype)) { //paytype is not a valid DefNum
				MsgBox.Show(this,"The PaySimple payment type has not been set.");
				return false;
			}
			//Processing through Web Service
			// Consider adding more advanced verification methods using PaySimple validation requests.
			if(textCardNumber.Text.Trim().Length<5) {
				MsgBox.Show(this,"Invalid Card Number.");
				return false;
			}
			try {//PIn.Int will throw an exception if not a valid format
				if(Regex.IsMatch(textExpDate.Text,@"^\d\d[/\- ]\d\d$")) {//08/07 or 08-07 or 08 07
					expYear=PIn.Int("20"+textExpDate.Text.Substring(3,2));
					expMonth=PIn.Int(textExpDate.Text.Substring(0,2));
				}
				else if(Regex.IsMatch(textExpDate.Text,@"^\d{4}$")) {//0807
					expYear=PIn.Int("20"+textExpDate.Text.Substring(2,2));
					expMonth=PIn.Int(textExpDate.Text.Substring(0,2));
				}
				else {
					MsgBox.Show(this,"Expiration format invalid.");
					return false;
				}
			}
			catch(Exception) {
				MsgBox.Show(this,"Expiration format invalid.");
				return false;
			}
			if(_creditCardCur==null) {//if the user selected a new CC, verify through PaySimple
				//using a new CC and the card number entered contains something other than digits
				if(textCardNumber.Text.Any(x => !char.IsDigit(x))) {
					MsgBox.Show(this,"Invalid card number.");
					return false;
				}
			}
			else if(_creditCardCur.PaySimpleToken=="" && Regex.IsMatch(textCardNumber.Text,@"X+[0-9]{4}")) {//using a stored CC
				MsgBox.Show(this,"There is no saved PaySimple token for this credit card.  The card number and expiration must be re-entered.");
				return false;
			}
			if(textNameOnCard.Text.Trim()=="" && _patCur!=null && _patCur.PatNum>0) {//Name required for patient credit cards, not prepaid cards.
				MsgBox.Show(this,"Name On Card required.");
				return false;
			}
			//verify the selected clinic has a username and password type entered
			if(string.IsNullOrWhiteSpace(ProgramProperties.GetPropValForClinicOrDefault(_progCur.ProgramNum,PaySimple.PropertyDescs.PaySimpleApiUserName,_clinicNum))
				|| string.IsNullOrWhiteSpace(ProgramProperties.GetPropValForClinicOrDefault(_progCur.ProgramNum,PaySimple.PropertyDescs.PaySimpleApiKey,_clinicNum))) //if username or password is blank
			{
				MsgBox.Show(this,"The PaySimple username and/or key has not been set.");
				return false;
			}
			return true;
		}

		private void butOK_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			int expYear;
			int expMonth;
			if(!VerifyData(out expYear,out expMonth)) {
				Cursor=Cursors.Default;
				return;
			}
			ApiResponseOut=ProcessPayment(expYear,expMonth);
			bool isSuccess=(ApiResponseOut!=null);
			Cursor=Cursors.Default;
			if(isSuccess) {
				DialogResult=DialogResult.OK;
			}
			else if(!_isAddingCard) {//If adding the card, leave the window open so the user can try again.
				DialogResult=DialogResult.Cancel;
			}
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}