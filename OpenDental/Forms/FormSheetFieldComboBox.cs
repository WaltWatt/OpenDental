using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormSheetFieldComboBox:ODForm {
		///<summary>This is the object we are editing.</summary>
		public SheetFieldDef SheetFieldDefCur;
		///<summary>We need access to a few other fields of the sheetDef.</summary>
		public SheetDef SheetDefCur;
		public bool IsReadOnly;
		private string _selectedOption;

		public FormSheetFieldComboBox() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSheetFieldComboBox_Load(object sender,EventArgs e) {
			textYPos.MaxVal=SheetDefCur.HeightTotal-1;//The maximum y-value of the sheet field must be within the page vertically.
			if(IsReadOnly){
				butOK.Enabled=false;
				butDelete.Enabled=false;
			}
			textXPos.Text=SheetFieldDefCur.XPos.ToString();
			textYPos.Text=SheetFieldDefCur.YPos.ToString();
			textWidth.Text=SheetFieldDefCur.Width.ToString();
			textHeight.Text=SheetFieldDefCur.Height.ToString();
			textTabOrder.Text=SheetFieldDefCur.TabOrder.ToString();
			textReportable.Text=SheetFieldDefCur.ReportableName;
			if(SheetFieldDefCur.FieldValue!="") {
				_selectedOption=SheetFieldDefCur.FieldValue.Split(';')[0];
				string[] arrayOptions=SheetFieldDefCur.FieldValue.Split(';')[1].Split('|');
				foreach(string option in arrayOptions) {
					if(String.IsNullOrWhiteSpace(option)) {
						continue;
					}
					listboxComboOptions.Items.Add(option);
				}
			}
		}

		private void textOption_KeyDown(object sender,KeyEventArgs e) {
			if(e.KeyData!=Keys.Enter) {
				return;
			}
			e.SuppressKeyPress=true;
			butAdd_Click(null,null);//If they press enter on the text, add the text to the listbox.
		}

		private void listComboType_SelectedIndexChanged(object sender,EventArgs e) {
			if(listComboType.SelectedIndex==0) {
				listboxComboOptions.Items.Clear();
				listboxComboOptions.Enabled=true;
				butRemove.Enabled=true;
				butUp.Enabled=true;
				butDown.Enabled=true;
			}
			else {
				if(listComboType.SelectedIndex==1) {//Patient Race
					listboxComboOptions.Items.Clear();
					string[] enumVals=Enum.GetNames(typeof(PatientRaceOld));
					listboxComboOptions.Items.AddRange(enumVals);
				}
				else if(listComboType.SelectedIndex==2) {//Patient Grade
					listboxComboOptions.Items.Clear();
					string[] enumVals=Enum.GetNames(typeof(PatientGrade));
					listboxComboOptions.Items.AddRange(enumVals);
				}
				else if(listComboType.SelectedIndex==3) {//Urgency
					listboxComboOptions.Items.Clear();
					string[] enumVals=Enum.GetNames(typeof(TreatmentUrgency));
					listboxComboOptions.Items.AddRange(enumVals);
				}
				listboxComboOptions.Enabled=false;
				butRemove.Enabled=false;
				butUp.Enabled=false;
				butDown.Enabled=false;
			}
		}

		private void butAdd_Click(object sender,EventArgs e) {
			if(String.IsNullOrWhiteSpace(textOption.Text)) {
				return;
			}
			listboxComboOptions.Items.Add(textOption.Text);
			textOption.Clear();
		}

		private void butRemove_Click(object sender,EventArgs e) {
			if(listboxComboOptions.SelectedIndex==-1) {
				return;
			}
			listboxComboOptions.Items.RemoveAt(listboxComboOptions.SelectedIndex);
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(listboxComboOptions.SelectedIndex==-1 || listboxComboOptions.SelectedIndex==0) {
				return;
			}
			int idx=listboxComboOptions.SelectedIndex;
			string item=listboxComboOptions.Items[idx].ToString();
			listboxComboOptions.Items.RemoveAt(idx);
			listboxComboOptions.Items.Insert(idx-1,item);
			listboxComboOptions.SelectedIndex=idx-1;
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(listboxComboOptions.SelectedIndex==-1 || listboxComboOptions.SelectedIndex==listboxComboOptions.Items.Count-1) {
				return;
			}
			int idx=listboxComboOptions.SelectedIndex;
			string item=listboxComboOptions.Items[idx].ToString();
			listboxComboOptions.Items.RemoveAt(idx);
			listboxComboOptions.Items.Insert(idx+1,item);
			listboxComboOptions.SelectedIndex=idx+1;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			SheetFieldDefCur=null;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			SaveAndClose();
		}

		private void SaveAndClose(){
			if(textXPos.errorProvider1.GetError(textXPos)!=""
				|| textYPos.errorProvider1.GetError(textYPos)!=""
				|| textWidth.errorProvider1.GetError(textWidth)!=""
				|| textHeight.errorProvider1.GetError(textHeight)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			SheetFieldDefCur.XPos=PIn.Int(textXPos.Text);
			SheetFieldDefCur.YPos=PIn.Int(textYPos.Text);
			SheetFieldDefCur.Width=PIn.Int(textWidth.Text);
			SheetFieldDefCur.Height=PIn.Int(textHeight.Text);
			SheetFieldDefCur.TabOrder=PIn.Int(textTabOrder.Text);
			SheetFieldDefCur.ReportableName=PIn.String(textReportable.Text);
			//ComboBox FieldValue will be:  selectedItem;all|possible|options|here|with|selectedItem|also
			//This is so we don't have to change the database schema for combo boxes.
			SheetFieldDefCur.FieldValue=_selectedOption+";";//NOTE: ; can change to whatever.  Maybe {?  Maybe something else not used often like @?
			for(int i=0;i<listboxComboOptions.Items.Count;i++) {
				if(i>0) {
					SheetFieldDefCur.FieldValue+="|";
				}
				SheetFieldDefCur.FieldValue+=listboxComboOptions.Items[i].ToString();
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}