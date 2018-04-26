using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormPhoneNumbersManage:ODForm {
		public long PatNum;
		private Patient Pat;
		private List<PhoneNumber> otherList;

		public FormPhoneNumbersManage() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormPhoneNumbersManage_Load(object sender,EventArgs e) {
			Pat=Patients.GetPat(PatNum);
			textName.Text=Pat.LName+", "+Pat.FName;
			textWkPhone.Text=Pat.WkPhone;
			textHmPhone.Text=Pat.HmPhone;
			textWirelessPhone.Text=Pat.WirelessPhone;
			textAddrNotes.Text=Pat.AddrNote;
			FillList();
		}

		private void FillList(){
			listOther.Items.Clear();
			otherList=PhoneNumbers.GetPhoneNumbers(PatNum);
			for(int i=0;i<otherList.Count;i++){
				listOther.Items.Add(otherList[i].PhoneNumberVal);
			}
		}

		private void textAnyPhone_TextChanged(object sender,System.EventArgs e) {
			if(sender.GetType()!=typeof(TextBox)) {
				return;
			}
			TextBox textPhone=(TextBox)sender;
			int phoneTextPosition=textPhone.SelectionStart;
			int textLength=textPhone.Text.Length;
			textPhone.Text=TelephoneNumbers.AutoFormat(textPhone.Text);
			int diff=textPhone.Text.Length-textLength;
			textPhone.SelectionStart=phoneTextPosition+diff;
		}

		private void listOther_DoubleClick(object sender,EventArgs e) {
			int index=listOther.SelectedIndex;
			if(index==-1) {
				return;
			}
			InputBox input=new InputBox("Phone Number");
			input.textResult.Text=otherList[index].PhoneNumberVal;
			input.ShowDialog();
			if(input.DialogResult!=DialogResult.OK) {
				return;
			}
			otherList[index].PhoneNumberVal=TelephoneNumbers.AutoFormat(input.textResult.Text);
			PhoneNumbers.Update(otherList[index]);
			FillList();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			InputBox input=new InputBox("Phone Number");
			input.textResult.TextChanged+=new EventHandler(textAnyPhone_TextChanged);//Auto format the number as the user types.
			input.ShowDialog();
			if(input.DialogResult!=DialogResult.OK) {
				return;
			}
			PhoneNumber phoneNumber=new PhoneNumber();
			phoneNumber.PatNum=PatNum;
			phoneNumber.PhoneNumberVal=input.textResult.Text;
			PhoneNumbers.Insert(phoneNumber);
			FillList();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(listOther.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a phone number first.");
				return;
			}
			PhoneNumbers.DeleteObject(otherList[listOther.SelectedIndex].PhoneNumberNum);
			FillList();
		}

		private void butOK_Click(object sender,EventArgs e) {
			Patient PatOld=Pat.Copy();
			Pat.WkPhone=textWkPhone.Text;
			Pat.HmPhone=textHmPhone.Text;
			Pat.WirelessPhone=textWirelessPhone.Text;
			Pat.AddrNote=textAddrNotes.Text;
			Patients.Update(Pat,PatOld);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		
	}
}