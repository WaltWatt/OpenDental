using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormSheetFieldSpecial:ODForm {
		///<summary>This is the object we are editing.</summary>
		public SheetFieldDef SheetFieldDefCur;
		///<summary>We need access to a few other fields of the sheetDef.</summary>
		public SheetDef SheetDefCur;
		//private List<SheetFieldDef> AvailFields;
		public bool IsReadOnly;
		public bool IsNew;
		private List<SheetFieldDef> _listFieldDefsAvailable;

		public FormSheetFieldSpecial() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSheetFieldDefEdit_Load(object sender,EventArgs e) {
			textYPos.MaxVal=SheetDefCur.HeightTotal-1;//The maximum y-value of the sheet field must be within the page vertically.
			if(IsReadOnly){
				butOK.Enabled=false;
				butDelete.Enabled=false;
			}
			_listFieldDefsAvailable=SheetFieldsAvailable.GetSpecial(SheetDefCur.SheetType);
			listBoxAvailable.Items.AddRange(_listFieldDefsAvailable.Select(x => (object)x.FieldName).ToArray());
			if(IsNew) {
				listBoxAvailable.SetSelected(0,true);
				SheetFieldDefCur=_listFieldDefsAvailable[0];
			}
			else {
				listBoxAvailable.SetSelected(_listFieldDefsAvailable.FindIndex(x => x.FieldName==SheetFieldDefCur.FieldName),true);
				listBoxAvailable.Enabled=false;
			}
			FillFields();
		}

		///<summary>Each special field type is a little bit different, this allows each field to fill the form in its own way.</summary>
		private void FillFields() {
			textXPos.Text=SheetFieldDefCur.XPos.ToString();
			textYPos.Text=SheetFieldDefCur.YPos.ToString();
			textWidth.Text=SheetFieldDefCur.Width.ToString();
			textHeight.Text=SheetFieldDefCur.Height.ToString();
			labelSpecialInfo.Text="";
			//textXPos.Enabled=true;
			//textYPos.Enabled=true;
			textHeight.Enabled=true;
			textWidth.Enabled=true;
			switch(listBoxAvailable.SelectedItem.ToString()) {
				case "toothChart":
					labelSpecialInfo.Text=Lan.g(this,"The tooth chart will display a graphical toothchart based on which patient and treatment plan is selected. "+
					                                 "Fixed aspect ratio of 1.35");
					break;
				case "toothChartLegend":
					labelSpecialInfo.Text=Lan.g(this,"The tooth chart legend shows what the colors on the tooth chart mean.");
					textWidth.Text="600";
					textHeight.Text="14";
					textWidth.Enabled=false;
					textHeight.Enabled=false;
					break;
				case "toothGrid"://not used
				default:
					break;
			}
		}

		private void listBoxAvailable_SelectedIndexChanged(object sender,EventArgs e) {
			if(!IsNew) {
				return; //should never happen
			}
			if(listBoxAvailable.SelectedIndices.Count==0 || listBoxAvailable.SelectedIndex<0) {
				return;
			}
			SheetFieldDefCur=_listFieldDefsAvailable[listBoxAvailable.SelectedIndex];
			FillFields();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			SheetFieldDefCur=null;
			if(IsNew) {
				DialogResult=DialogResult.Cancel;
			}
			else {
				DialogResult=DialogResult.OK;
			}
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
			//don't save to database here.
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		

		
	}
}