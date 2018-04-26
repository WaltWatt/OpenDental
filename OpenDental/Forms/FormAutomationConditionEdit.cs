using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;

namespace OpenDental {
	public partial class FormAutomationConditionEdit:ODForm {
		public bool IsNew;
		public AutomationCondition ConditionCur;

		public FormAutomationConditionEdit() {
			InitializeComponent();
			Lan.F(this);

		}

		private void FormAutomationConditionEdit_Load(object sender,EventArgs e) {
			for(int i=0;i<Enum.GetNames(typeof(AutoCondField)).Length;i++) {
				listCompareField.Items.Add(Lan.g("enumAutoCondField",Enum.GetNames(typeof(AutoCondField))[i]));
				listCompareField.SelectedIndex=0;
			}
			for(int i=0;i<Enum.GetNames(typeof(AutoCondComparison)).Length;i++) {
				if(Enum.GetNames(typeof(AutoCondComparison))[i]=="None") {//Skip none as an option for the comparison field
					continue;
				}
				listComparison.Items.Add(Lan.g("enumAutoCondComparison",Enum.GetNames(typeof(AutoCondComparison))[i]));
				listComparison.SelectedIndex=0;
			}
			if(!IsNew) {
				textCompareString.Text=ConditionCur.CompareString;
				listCompareField.SelectedIndex=(int)ConditionCur.CompareField;
				if(ConditionCur.CompareField!=AutoCondField.InsuranceNotEffective) {
					listComparison.SelectedIndex=(int)ConditionCur.Comparison;
				}
				ShowOrHideFields((AutoCondField)listCompareField.SelectedIndex);
			}
		}

		///<summary>Show or hides the form fileds based of needs.</summary>
		private void ShowOrHideFields(AutoCondField autoCondField) {
			if(autoCondField==AutoCondField.BillingType) {
				butBillingType.Visible=true;
				labelCompareString.Text=Lan.g(this,"Text (Type or pick from list) ");
			}
			else {
				butBillingType.Visible=false;
				labelCompareString.Text=Lan.g(this,"Text");
			}
			if(autoCondField==AutoCondField.InsuranceNotEffective) {//Hide fields
				//labelWarning.Text=""; //Currently, this is the only condition that shows the warning label.  No need to dynamically set the text.
				labelWarning.Visible=true;
				labelComparison.Visible=false;
				labelCompareString.Visible=false;
				listComparison.Visible=false;
				textCompareString.Visible=false;
			}
			else {//Show fields
				labelWarning.Visible=false;
				labelComparison.Visible=true;
				labelCompareString.Visible=true;
				listComparison.Visible=true;
				textCompareString.Visible=true;
			}
		}

		///<summary>Logic might get more complex as fields and comparisons are added so a seperate function was made.</summary>
		private bool ReasonableLogic() {
			AutoCondComparison comp=(AutoCondComparison)listComparison.SelectedIndex;
			AutoCondField cond=(AutoCondField)listCompareField.SelectedIndex;
			//So far Age is only thing that allows GreaterThan or LessThan.
			if(cond!=AutoCondField.Age) {
				if(comp==AutoCondComparison.GreaterThan || comp==AutoCondComparison.LessThan) {
					return false;
				}
			}
			else {
				int age;
				//Make sure that the user typed in an integer and not a word.
				if(!int.TryParse(textCompareString.Text,out age)) {
					return false;
				}
			}
			return true;
		}

		private void listCompareField_Click(object sender,EventArgs e) {
			ShowOrHideFields((AutoCondField)listCompareField.SelectedIndex);
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!MsgBox.Show(this,true,"Delete this condition?")) {
				return;
			}
			try {
				AutomationConditions.Delete(ConditionCur.AutomationConditionNum);
				DialogResult=DialogResult.OK;
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void butBillingType_Click(object sender,EventArgs e) {
			FormDefinitionPicker FormDP = new FormDefinitionPicker(DefCat.BillingTypes);
			FormDP.HasShowHiddenOption=false;
			FormDP.IsMultiSelectionMode=false;
			FormDP.ShowDialog();
			if(FormDP.DialogResult==DialogResult.OK) {
				textCompareString.Text=FormDP.ListSelectedDefs?[0]?.ItemName??"";
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if((AutoCondField)listCompareField.SelectedIndex==AutoCondField.InsuranceNotEffective) {
				ConditionCur.CompareString=""; 
				ConditionCur.CompareField=AutoCondField.InsuranceNotEffective;
				ConditionCur.Comparison=AutoCondComparison.None;
			}
			else {
				if(textCompareString.Text.Trim()=="") {
					MsgBox.Show(this,"Text not allowed to be blank.");
					return;
				}
				if(!ReasonableLogic()) {
					MsgBox.Show(this,"Comparison does not make sense with chosen field.");
					return;
				}
				if(((AutoCondField)listCompareField.SelectedIndex==AutoCondField.Gender
					&& !(textCompareString.Text.ToLower()=="m" || textCompareString.Text.ToLower()=="f"))) 
				{
					MsgBox.Show(this,"Allowed gender values are M or F.");
					return;
				}
				ConditionCur.CompareString=textCompareString.Text;
				ConditionCur.CompareField=(AutoCondField)listCompareField.SelectedIndex;
				ConditionCur.Comparison=(AutoCondComparison)listComparison.SelectedIndex;
			}
			if(IsNew) {
				AutomationConditions.Insert(ConditionCur);
			}
			else {
				AutomationConditions.Update(ConditionCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}