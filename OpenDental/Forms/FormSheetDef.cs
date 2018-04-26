using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormSheetDef:ODForm {
		///<summary></summary>
		public SheetDef SheetDefCur;
		//private List<SheetFieldDef> AvailFields;
		public bool IsReadOnly;
		///<summary>On creation of a new sheetdef, the user must pick a description and a sheettype before allowing to start editing the sheet.  After the initial sheettype selection, this will be false, indicating that the user may not change the type.</summary>
		public bool IsInitial;
		///<summary>SheetTypeEnum includes SheetTypeEnum.MedLabResults which is hidden in some cases. This allows us to keep track of actual SheetType.</summary>
		private List<int> _listSheetTypeIndexes;

		public FormSheetDef() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSheetDef_Load(object sender,EventArgs e) {
			setHeightWidthMin();
			if(IsReadOnly){
				butOK.Enabled=false;
			}
			if(!IsInitial){
				listSheetType.Enabled=false;
			}
			textDescription.Text=SheetDefCur.Description;
			_listSheetTypeIndexes=new List<int>();
			//not allowed to change sheettype once created.
			for(int i=0;i<Enum.GetNames(typeof(SheetTypeEnum)).Length;i++){
				if((SheetTypeEnum)i==SheetTypeEnum.MedLabResults) {
					continue;
				}
				listSheetType.Items.Add(Enum.GetNames(typeof(SheetTypeEnum))[i]);
				_listSheetTypeIndexes.Add(i);
				if((int)SheetDefCur.SheetType==i && !IsInitial){
					listSheetType.SelectedIndex=listSheetType.Items.Count-1;
				}
			}
			InstalledFontCollection fColl=new InstalledFontCollection();
			for(int i=0;i<fColl.Families.Length;i++){
				comboFontName.Items.Add(fColl.Families[i].Name);
			}
			checkBypassLockDate.Checked=(SheetDefCur.BypassGlobalLock==BypassLockStatus.BypassAlways);
			comboFontName.Text=SheetDefCur.FontName;
			textFontSize.Text=SheetDefCur.FontSize.ToString();
			textWidth.Text=SheetDefCur.Width.ToString();
			textHeight.Text=SheetDefCur.Height.ToString();
			checkIsLandscape.Checked=SheetDefCur.IsLandscape;
		}

		///<summary>Sets the minimum valid value (used for validation only) of the appropriate Height or Width field based on the bottom of the lowest field. 
		///Max values are set in the designer.</summary>
		private void setHeightWidthMin() {
			textHeight.MinVal=-100;//default values
			textWidth.MinVal=-100;//default values
			if(SheetDefCur.SheetFieldDefs==null) {
				//New sheet
				return;
			}
			int minVal=int.MaxValue;
			for(int i=0;i<SheetDefCur.SheetFieldDefs.Count;i++) {
				minVal=Math.Min(minVal,SheetDefCur.SheetFieldDefs[i].Bounds.Bottom/SheetDefCur.PageCount);
			}
			if(minVal==int.MaxValue) {
				//Sheet has no sheet fields.
				return;
			}
			if(checkIsLandscape.Checked) {
				//Because Width is used to measure vertical sheet size.
				textWidth.MinVal=minVal;
			}
			else {
				//Because Height is used to measure vertical sheet size.
				textHeight.MinVal=minVal;
			}
		}

		private void checkIsLandscape_Click(object sender,EventArgs e) {
			setHeightWidthMin();
		}

		private void listSheetType_Click(object sender,EventArgs e) {
			if(!IsInitial){
				return;
			}
			if(listSheetType.SelectedIndex==-1){
				return;
			}
			SheetDef sheetdef=null;
			switch((SheetTypeEnum)_listSheetTypeIndexes[listSheetType.SelectedIndex]){
				case SheetTypeEnum.LabelCarrier:
				case SheetTypeEnum.LabelPatient:
				case SheetTypeEnum.LabelReferral:
					sheetdef=SheetsInternal.GetSheetDef(SheetInternalType.LabelPatientMail);
					if(textDescription.Text==""){
						textDescription.Text=((SheetTypeEnum)_listSheetTypeIndexes[listSheetType.SelectedIndex]).ToString();
					}
					comboFontName.Text=sheetdef.FontName;
					textFontSize.Text=sheetdef.FontSize.ToString();
					textWidth.Text=sheetdef.Width.ToString();
					textHeight.Text=sheetdef.Height.ToString();
					checkIsLandscape.Checked=sheetdef.IsLandscape;
					break;
				case SheetTypeEnum.ReferralSlip:
					sheetdef=SheetsInternal.GetSheetDef(SheetInternalType.ReferralSlip);
					if(textDescription.Text==""){
						textDescription.Text=((SheetTypeEnum)_listSheetTypeIndexes[listSheetType.SelectedIndex]).ToString();
					}
					comboFontName.Text=sheetdef.FontName;
					textFontSize.Text=sheetdef.FontSize.ToString();
					textWidth.Text=sheetdef.Width.ToString();
					textHeight.Text=sheetdef.Height.ToString();
					checkIsLandscape.Checked=sheetdef.IsLandscape;
					break;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textWidth.errorProvider1.GetError(textWidth)!=""
				|| textHeight.errorProvider1.GetError(textHeight)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(listSheetType.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a sheet type first.");
				return;
			}
			if(textDescription.Text=="") {
				MsgBox.Show(this,"Description may not be blank.");
				return;
			}
			if((SheetTypeEnum)_listSheetTypeIndexes[listSheetType.SelectedIndex]==SheetTypeEnum.ExamSheet) {
				//make sure description for exam sheet does not contain a ':' or a ';' because this interferes with pulling the exam sheet fields to fill a patient letter
				if(textDescription.Text.Contains(":") || textDescription.Text.Contains(";")) {
					MsgBox.Show(this,"Description for an Exam Sheet may not contain a ':' or a ';'.");
					return;
				}
			}
			if(comboFontName.Text==""){
				//not going to bother testing for validity unless it will cause a crash.
				MsgBox.Show(this,"Please select a font name first.");
				return;
			}
			float fontSize;
			try{
				fontSize=float.Parse(textFontSize.Text);
				if(fontSize<2){
					MsgBox.Show(this,"Font size is invalid.");
					return;
				}
			}
			catch{
				MsgBox.Show(this,"Font size is invalid.");
				return;
			}
			SheetDefCur.Description=textDescription.Text;
			SheetDefCur.SheetType=(SheetTypeEnum)_listSheetTypeIndexes[listSheetType.SelectedIndex];
			if(checkBypassLockDate.Checked) {
				SheetDefCur.BypassGlobalLock=BypassLockStatus.BypassAlways;
			}
			else {
				SheetDefCur.BypassGlobalLock=BypassLockStatus.NeverBypass;
			}
			SheetDefCur.FontName=comboFontName.Text;
			SheetDefCur.FontSize=fontSize;
			SheetDefCur.Width=PIn.Int(textWidth.Text);
			SheetDefCur.Height=PIn.Int(textHeight.Text);
			SheetDefCur.IsLandscape=checkIsLandscape.Checked;
			//don't save to database here.
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		

		
	}
}