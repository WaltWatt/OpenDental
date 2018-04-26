﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using EhrLaboratories;

namespace OpenDental {
	public partial class FormPatListElementEditEHR2014:ODForm {
		public EhrPatListElement2014 Element;
		public bool IsNew;
		public bool Delete;
		private List<Ucum> _listUCUM;

		public FormPatListElementEditEHR2014() {
			InitializeComponent();
		}

		private void FormPatListElementEdit_Load(object sender,EventArgs e) {
			listRestriction.Items.Clear();
			for(int i=0;i<Enum.GetNames(typeof(EhrRestrictionType)).Length;i++) {
				listRestriction.Items.Add(Enum.GetNames(typeof(EhrRestrictionType))[i]);
			}
			listRestriction.SelectedIndex=(int)Element.Restriction;
			listOperand.SelectedIndex=(int)Element.Operand;
			textCompareString.Text=Element.CompareString;
			if(Element.Restriction==EhrRestrictionType.Problem && !IsNew) {
				textCompareString.Text="";//clear text box for simplicity
				if(ICD9s.CodeExists(Element.CompareString)) {
					textCompareString.Text=Element.CompareString;
				}
				else if(Snomeds.CodeExists(Element.CompareString)) {
					textSNOMED.Text=Element.CompareString;
				}
				else {
					MsgBox.Show(this,"Problem code provided is not an existing ICD9 or SNOMED code.");
					//no harm in continuing since this form is error checked on OK click.
				}
			}
			fillCombos();
			if(!IsNew) {
				comboUnits.Text=Element.LabValueUnits;
				comboLabValueType.SelectedIndex=(int)Element.LabValueType;
			}
			textLabValue.Text=Element.LabValue;
			if(Element.StartDate.Year>1880) {
				textDateStart.Text=Element.StartDate.ToShortDateString();
			}
			if(Element.EndDate.Year>1880) {
				textDateStop.Text=Element.EndDate.ToShortDateString();
			}
			ChangeLayout();
		}

		private void fillCombos() {
			//Units of measure----------------------------------------
			_listUCUM=Ucums.GetAll();
			if(_listUCUM.Count==0) {
				MsgBox.Show(this,"Units of measure have not been imported. Go to the code system importer window to import UCUM codes to continue.");
				DialogResult=DialogResult.Cancel;
				return;
			}
			int _tempSelectedIndex=0;
			for(int i=0;i<_listUCUM.Count;i++) {
				comboUnits.Items.Add(_listUCUM[i].UcumCode);
				if(_listUCUM[i].UcumCode=="mg/dL") {//arbitrarily chosen common unit of measure.
					_tempSelectedIndex=i;
				}
			}
			comboUnits.SelectedIndex=_tempSelectedIndex;
			//Lab Value Type.  Based off of the HL70125 data type (Value Type)----------------------------------------
			comboLabValueType.Items.Add("Coded Entry");
			comboLabValueType.Items.Add("Coded with Exceptions");
			comboLabValueType.Items.Add("Date");
			comboLabValueType.Items.Add("Formatted Text");
			comboLabValueType.Items.Add("Numeric");//default
			comboLabValueType.Items.Add("Structured Numeric");
			comboLabValueType.Items.Add("String Data");
			comboLabValueType.Items.Add("Time");
			comboLabValueType.Items.Add("Time Stamp");
			comboLabValueType.Items.Add("Text Data");
			comboLabValueType.SelectedIndex=4;//Numeric.  This is what we used to assume all lab results were like.
		}

		private void listRestriction_SelectedIndexChanged(object sender,EventArgs e) {
			ChangeLayout();
		}

		private void ChangeLayout() {
			//All controls' visibility set to false, then set to true below if needed.
			//Labels
			labelOperand.Visible=false;
			labelCompareString.Visible=false;
			labelSNOMED.Visible=false;
			labelLabValue.Visible=false;
			labelAfterDate.Visible=false;
			labelBeforeDate.Visible=false;
			labelExample.Visible=false;
			labelProblemSuggest.Visible=false;
			labelLabValueType.Visible=false;
			//TextBoxes
			textCompareString.Visible=false;
			textSNOMED.Visible=false;
			textLabValue.Visible=false;
			textDateStart.Visible=false;
			textDateStop.Visible=false;
			//Buttons
			butPicker.Visible=false;
			butSNOMED.Visible=false;
			butProblem.Visible=false;
			//ComboBoxes
			comboUnits.Visible=false;
			comboLabValueType.Visible=false;
			//ListBoxes
			listOperand.Visible=false;
			switch(listRestriction.SelectedIndex) {
				case 0: //Birthdate------------------------------------------------------------------------------------------------------------
					//Labels
					labelOperand.Visible=true;
					labelCompareString.Visible=true;
					labelCompareString.Text="Enter age";
					labelExample.Visible=true;
					labelExample.Text="Ex: 22";
					//TextBoxes
					textCompareString.Visible=true;
					//ListBoxes
					listOperand.Visible=true;
					break;
				case 1: //Disease--------------------------------------------------------------------------------------------------------------
					//Labels
					labelCompareString.Visible=true;
					labelCompareString.Text="ICD9 Code";
					labelSNOMED.Visible=true;
					labelSNOMED.Text="SNOMED Code";
					labelAfterDate.Visible=true;
					labelBeforeDate.Visible=true;
					labelProblemSuggest.Visible=true;
					//TextBoxes
					textCompareString.Visible=true;
					textSNOMED.Visible=true;
					textDateStart.Visible=true;
					textDateStop.Visible=true;
					//Buttons
					butPicker.Visible=true;
					butSNOMED.Visible=true;
					butProblem.Visible=true;
					break;
				case 2: //Medication-----------------------------------------------------------------------------------------------------------
					//Labels
					labelCompareString.Visible=true;
					labelCompareString.Text="Medication Name";
					labelAfterDate.Visible=true;
					labelBeforeDate.Visible=true;
					labelExample.Visible=true;
					labelExample.Text="Ex: Albuterol";
					//TextBoxes
					textCompareString.Visible=true;
					textDateStart.Visible=true;
					textDateStop.Visible=true;
					//Buttons
					butPicker.Visible=true;
					break;
				case 3: //LabResult------------------------------------------------------------------------------------------------------------
					//Labels
					labelOperand.Visible=true;
					labelCompareString.Visible=true;
					labelCompareString.Text="Loinc Code";
					labelLabValue.Visible=true;
					labelAfterDate.Visible=true;
					labelBeforeDate.Visible=true;
					labelExample.Visible=true;
					labelExample.Text="Ex: 1004-1";
					labelLabValueType.Visible=true;
					//TextBoxes
					textCompareString.Visible=true;
					textLabValue.Visible=true;
					textDateStart.Visible=true;
					textDateStop.Visible=true;
					//Buttons
					butPicker.Visible=true;
					//ComboBoxes
					comboUnits.Visible=true;
					comboLabValueType.Visible=true;
					//ListBoxes
					listOperand.Visible=true;
					break;
				case 4: //Gender---------------------------------------------------------------------------------------------------------------
					//Labels
					labelCompareString.Visible=true;
					labelCompareString.Text="For display and sorting";
					break;
				case 5: //CommPref-------------------------------------------------------------------------------------------------------------
					//Labels
					labelCompareString.Visible=true;
					labelCompareString.Text="Communication Preference";
					labelExample.Visible=true;
					labelExample.Text="Ex: WirelessPh";
					//TextBoxes
					textCompareString.Visible=true;
					//Buttons
					butPicker.Visible=true;
					break;
				case 6: //Allergy--------------------------------------------------------------------------------------------------------------
					//Labels
					labelCompareString.Visible=true;
					labelCompareString.Text="Allergy Description (exact)";
					labelAfterDate.Visible=true;
					labelBeforeDate.Visible=true;
					//TextBoxes
					textCompareString.Visible=true;
					textDateStart.Visible=true;
					textDateStop.Visible=true;
					//Buttons
					butPicker.Visible=true;
					break;
				default://Default--------------------------------------------------------------------------------------------------------------
					//Nothing will show
					break;
			}
			#region old code
			//labelCompareString.Visible=true;
			//textCompareString.Visible=true;
			//labelExample.Visible=true;
			//labelLabValue.Visible=false;
			//textLabValue.Visible=false;
			//labelOperand.Visible=false;
			//listOperand.Visible=false;
			//if(listRestriction.SelectedIndex==0) {//Birthdate
			//  labelCompareString.Text="Enter age";
			//  labelExample.Text="Ex: 22";
			//  labelSNOMED.Visible=false;
			//  labelOperand.Visible=true;
			//  listOperand.Visible=true;
			//  butPicker.Visible=false;
			//  labelAfterDate.Visible=false;
			//  labelBeforeDate.Visible=false;
			//  textDateStart.Visible=false;
			//  textDateStop.Visible=false;
			//}
			//if(listRestriction.SelectedIndex==1) {//Disease/Problem
			//  labelCompareString.Text="ICD9 code";
			//  labelCompareString.Text="ICD9 code";
			//  labelExample.Text="Ex: 414.0";
			//  labelSNOMED.Visible=false;
			//  butPicker.Visible=true;
			//  labelAfterDate.Visible=true;
			//  labelBeforeDate.Visible=true;
			//  textDateStart.Visible=true;
			//  textDateStop.Visible=true;
			//}
			//if(listRestriction.SelectedIndex==2) {//Medication
			//  labelCompareString.Text="Medication name";
			//  labelExample.Text="Ex: Coumadin";
			//  labelSNOMED.Visible=false;
			//  butPicker.Visible=true;
			//  labelAfterDate.Visible=true;
			//  labelBeforeDate.Visible=true;
			//  textDateStart.Visible=true;
			//  textDateStop.Visible=true;
			//}
			//if(listRestriction.SelectedIndex==3) {//LabResult
			//  labelCompareString.Text="Test name (exact)";
			//  labelExample.Text="Ex: HDL-cholesterol";
			//  labelSNOMED.Visible=false;
			//  labelLabValue.Visible=true;
			//  textLabValue.Visible=true;
			//  labelOperand.Visible=true;
			//  listOperand.Visible=true;
			//  butPicker.Visible=false;
			//  labelAfterDate.Visible=true;
			//  labelBeforeDate.Visible=true;
			//  textDateStart.Visible=true;
			//  textDateStop.Visible=true;
			//}
			//if(listRestriction.SelectedIndex==4) {//Gender
			//  labelCompareString.Text="For display and sorting";
			//  labelExample.Visible=false;
			//  labelSNOMED.Visible=false;
			//  textCompareString.Visible=false;
			//  butPicker.Visible=false;
			//  labelAfterDate.Visible=false;
			//  labelBeforeDate.Visible=false;
			//  textDateStart.Visible=false;
			//  textDateStop.Visible=false;
			//}
			//if(listRestriction.SelectedIndex==5) {//CommPref
			//  labelCompareString.Text="Communication Method (exact)";
			//  labelExample.Text="Ex: WirelessPh";
			//  labelSNOMED.Visible=false;
			//  textCompareString.Visible=true;
			//  butPicker.Visible=true;
			//  labelAfterDate.Visible=false;
			//  labelBeforeDate.Visible=false;
			//  textDateStart.Visible=false;
			//  textDateStop.Visible=false;
			//}
			//if(listRestriction.SelectedIndex==6) {//Allergy
			//  labelCompareString.Text="Allergy Description (exact)";
			//  labelExample.Visible=false;
			//  textCompareString.Visible=true;
			//  butPicker.Visible=true;
			//  labelAfterDate.Visible=true;
			//  labelBeforeDate.Visible=true;
			//  textDateStart.Visible=true;
			//  textDateStop.Visible=true;
			//}
			#endregion
		}

		private bool IsValid() {
			int index=listRestriction.SelectedIndex;
			if(index!=3) {//Not LabResult
				textLabValue.Text="";
			}
			switch(index) {
				case 0: //Birthdate------------------------------------------------------------------------------------------------------------
					try {
						Convert.ToInt32(textCompareString.Text);//used intead of PIn so that an empty string is not evaluated as 0
					}
					catch {
						MsgBox.Show(this,"Please enter a valid age.");
						return false;
					}
					break;
				case 1: //Disease--------------------------------------------------------------------------------------------------------------
					if(textCompareString.Text=="" && textSNOMED.Text=="") {
						MsgBox.Show(this,"Please enter a valid SNOMED CT or ICD9 code.");
						return false;
					}
					if(textCompareString.Text!="") {
						if(!ICD9s.CodeExists(textCompareString.Text)) {
							MsgBox.Show(this,"ICD9 code does not exist in database, pick from list.");
							return false;
						}
					}
					if(textSNOMED.Text!=""){
						if(!Snomeds.CodeExists(textSNOMED.Text)) {
							MsgBox.Show(this,"SNOMED CT code does not exist in database, pick from list.");
							return false;
						}
					}
					if(textDateStart.errorProvider1.GetError(textDateStart)!=""
						|| textDateStop.errorProvider1.GetError(textDateStop)!=""
						) {
						MsgBox.Show(this,"Please fix date entry errors.");
						return false;
					}
					break;
				case 2: //Medication-----------------------------------------------------------------------------------------------------------
					if(textCompareString.Text=="") {
						MsgBox.Show(this,"Please enter a valid medication.");
						return false;
					}
					if(Medications.GetMedicationFromDbByName(textCompareString.Text)==null) {
						MsgBox.Show(this,"Medication does not exist in database, pick from list.");
						return false;
					}
					if(textDateStart.errorProvider1.GetError(textDateStart)!=""
						|| textDateStop.errorProvider1.GetError(textDateStop)!=""
						) {
						MsgBox.Show(this,"Please fix date entry errors.");
						return false;
					}
					break;
				case 3: //LabResult------------------------------------------------------------------------------------------------------------
					if(textCompareString.Text=="") {
						MsgBox.Show(this,"Please select a valid Loinc Code.");
						return false;
					}
					//if(Loincs.GetByCode(textCompareString.Text)==null) {
					//	MsgBox.Show(this,"Loinc code does not exist in database, pick from list.");
					//	return false;
					//}
					if(textDateStart.errorProvider1.GetError(textDateStart)!=""
						|| textDateStop.errorProvider1.GetError(textDateStop)!=""
						) {
						MsgBox.Show(this,"Please fix date entry errors.");
						return false;
					}
					break;
				case 4: //Gender---------------------------------------------------------------------------------------------------------------
					textCompareString.Text="";
					break;
				case 5: //CommPref-------------------------------------------------------------------------------------------------------------
					if(textCompareString.Text=="") {
						MsgBox.Show(this,"Please enter a communication preference.");
						return false;
					}
					if(!isContactMethod(textCompareString.Text)){
						MsgBox.Show(this,"Communication preference not defined, pick from list.");
						return false;
					}
					break;
				case 6: //Allergy--------------------------------------------------------------------------------------------------------------
					if(textCompareString.Text=="") {
						MsgBox.Show(this,"Please enter a valid allergy.");
						return false;
					}
					if(AllergyDefs.GetByDescription(textCompareString.Text)==null) {
						MsgBox.Show(this,"Allergy does not exist in database, pick from list.");
						return false;
					}
					if(textDateStart.errorProvider1.GetError(textDateStart)!=""
						|| textDateStop.errorProvider1.GetError(textDateStop)!=""
						){
						MsgBox.Show(this,"Please fix date entry errors.");
						return false;
					}
					break;
			}
			return true;
		}

		///<summary>Returns true if string matches ContactMethod enum names.</summary>
		private bool isContactMethod(string contactMethodName)
		{
			string[] contactNames=Enum.GetNames(typeof(ContactMethod));
			for(int i=0;i<contactNames.Length;i++) {
				if(contactNames[i]==contactMethodName) {
					return true;
				}
			}
			return false;
		}

		private void butPicker_Click(object sender,EventArgs e) {
			switch(listRestriction.SelectedIndex) {
				case 0://Birthdate
					//Not visible
					break;
				case 1://problem
					if(sender.Equals(butPicker)) {
						FormIcd9s FormI9=new FormIcd9s();
						FormI9.IsSelectionMode=true;
						FormI9.ShowDialog();
						if(FormI9.DialogResult!=DialogResult.OK) {
							return;
						}
						textCompareString.Text=FormI9.SelectedIcd9.ICD9Code;
						textSNOMED.Text="";
					}
					else if(sender.Equals(butSNOMED)) {
						FormSnomeds FormS=new FormSnomeds();
						FormS.IsSelectionMode=true;
						FormS.ShowDialog();
						if(FormS.DialogResult!=DialogResult.OK) {
							return;
						}
						textSNOMED.Text=FormS.SelectedSnomed.SnomedCode;
						textCompareString.Text="";
					}
					break;
				case 2://Medication
					FormMedications FormM=new FormMedications();
					FormM.IsSelectionMode=true;
					FormM.ShowDialog();
					if(FormM.DialogResult!=DialogResult.OK) {
						return;
					}
					textCompareString.Text=Medications.GetNameOnly(FormM.SelectedMedicationNum);
					break;
				case 3://LabResult
					FormLoincs FormL=new FormLoincs();
					FormL.IsSelectionMode=true;
					FormL.ShowDialog();
					if(FormL.DialogResult!=DialogResult.OK) {
						return;
					}
					textCompareString.Text=FormL.SelectedLoinc.LoincCode;
					comboUnits.Text=FormL.SelectedLoinc.UnitsUCUM;//may be valued, may be blank.
					break;
				case 4://Gender
					//Not visible
					break;
				case 5://Comm preference
					FormCommPrefPicker FormCPP = new FormCommPrefPicker();
					FormCPP.ShowDialog();
					if(FormCPP.DialogResult!=DialogResult.OK) {
						return;
					}
					textCompareString.Text=Enum.GetName(typeof(ContactMethod),FormCPP.ContMethCur);
					break;
				case 6://Alergy
					FormAllergySetup FormAS=new FormAllergySetup();
					FormAS.IsSelectionMode=true;
					FormAS.ShowDialog();
					if(FormAS.DialogResult!=DialogResult.OK) {
						return;
					}
					textCompareString.Text=AllergyDefs.GetDescription(FormAS.SelectedAllergyDefNum);
					break;
				default://should never happen
					break;
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!IsNew) {
				Delete=true;
			}
			DialogResult=DialogResult.Cancel;
		}

		private void textCompareString_TextChanged(object sender,EventArgs e) {
			if(textCompareString.Text!="") {
				textSNOMED.Text="";
			}
		}

		private void textSNOMED_TextChanged(object sender,EventArgs e) {
			if(textSNOMED.Text!="") {
				textCompareString.Text="";
			}
		}

		private void butProblem_Click(object sender,EventArgs e) {
			FormDiseaseDefs FormDD=new FormDiseaseDefs();
			FormDD.IsSelectionMode=true;
			FormDD.ShowDialog();
			if(FormDD.DialogResult!=DialogResult.OK) {
				return;
			}
			//the list should only ever contain one item.
			DiseaseDef dis=FormDD.ListSelectedDiseaseDefs[0];
			if(dis.SnomedCode!="") {
				textSNOMED.Text=dis.SnomedCode;
			}
			else if(dis.ICD9Code!="") {
				textCompareString.Text=dis.ICD9Code;
				MsgBox.Show(this,"Selected problem does not have a valid SNOMED CT code.");
			}
			else {
				MsgBox.Show(this,"Selected problem does not have a valid SNOMED CT code.");
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!IsValid()) {
				return;
			}
			Element.Restriction=(EhrRestrictionType)listRestriction.SelectedIndex;
			Element.Operand=(EhrOperand)listOperand.SelectedIndex;
				Element.CompareString=textCompareString.Text;
			if(Element.Restriction==EhrRestrictionType.Problem && textCompareString.Text=="") {
				Element.CompareString=textSNOMED.Text;
			}
			Element.LabValue=textLabValue.Text;
			Element.LabValueType=(HL70125)comboLabValueType.SelectedIndex;
			Element.LabValueUnits=comboUnits.Text;//UCUM units or blank.
			try {
				Element.StartDate=DateTime.Parse(textDateStart.Text);
			}
			catch {
				Element.StartDate=DateTime.MinValue;
			}
			try {
				Element.EndDate=DateTime.Parse(textDateStop.Text);
			}
			catch {
				Element.EndDate=DateTime.MinValue;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
