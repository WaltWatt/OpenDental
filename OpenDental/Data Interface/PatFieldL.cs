using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public class PatFieldL {
		///<summary>Adds the passed in pat fields to the grid. Adds any fields that have been renamed at the end of the grid if the preference is
		///enabled. The tag on the row will be the PatFieldDef or the PatField if the PatFieldDef has been renamed.</summary>
		public static void AddPatFieldsToGrid(ODGrid grid,List<PatField> listPatFields,FieldLocations fieldLocation,
			List<FieldDefLink> listFieldDefLinks=null) 
		{
			List<PatFieldDef> listPatFieldDefs=PatFieldDefs.GetDeepCopy(true);
			listFieldDefLinks=listFieldDefLinks??FieldDefLinks.GetForLocation(fieldLocation)
				.FindAll(x => x.FieldDefType==FieldDefTypes.Patient);
			//Add a row for each existing PatFieldDef 
			foreach(PatFieldDef patFieldDef in listPatFieldDefs) {
				if(listFieldDefLinks.Exists(x => x.FieldDefNum==patFieldDef.PatFieldDefNum)) {
					continue;
				}
				ODGridRow row=new ODGridRow();
				PatField field=listPatFields.FirstOrDefault(x => x.FieldName==patFieldDef.FieldName);
				if(patFieldDef.FieldType.ToString()=="InCaseOfEmergency") {
					//Deprecated. Should never happen.
					continue;
				}
				row.Cells.Add(patFieldDef.FieldName);
				if(field==null) {
					row.Cells.Add("");
				}
				else {
					if(patFieldDef.FieldType==PatFieldType.Checkbox) {
						row.Cells.Add("X");
					}
					else if(patFieldDef.FieldType==PatFieldType.Currency) {
						row.Cells.Add(PIn.Double(field.FieldValue).ToString("c"));
					}
					else {
						row.Cells.Add(field.FieldValue);
					}
				}
				row.Tag=patFieldDef;
				grid.Rows.Add(row);
			}
			if(!PrefC.GetBool(PrefName.DisplayRenamedPatFields)) {
				return;
			}
			//Now loop through the PatFields that do not have a matching PatFieldDef.
			foreach(PatField patField in listPatFields.Where(x => !listPatFieldDefs.Any(y => y.FieldName==x.FieldName))) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(patField.FieldName);
				row.Cells.Add(patField.FieldValue);
				row.Tag=patField;
				row.ColorText=Color.DarkSlateGray;
				grid.Rows.Add(row);
			}
		}

		///<summary>Opens the appropriate form to edit the patient field. The patField argument can be null or the patFieldDef argument can be null,
		///but they cannot both be null.</summary>
		public static void OpenPatField(PatField patField,PatFieldDef patFieldDef,long patNum,bool isForOrtho=false) {
			if(patFieldDef!=null) { 
				if(patField==null) {
					patField=new PatField();
					patField.PatNum=patNum;
					patField.FieldName=patFieldDef.FieldName;
					if(patFieldDef.FieldType==PatFieldType.Text) {
						FormPatFieldEdit FormPF=new FormPatFieldEdit(patField);
						FormPF.IsLaunchedFromOrtho=isForOrtho;
						FormPF.IsNew=true;
						FormPF.ShowDialog();
					}
					if(patFieldDef.FieldType==PatFieldType.PickList) {
						FormPatFieldPickEdit FormPF=new FormPatFieldPickEdit(patField);
						FormPF.IsNew=true;
						FormPF.ShowDialog();
					}
					if(patFieldDef.FieldType==PatFieldType.Date) {
						FormPatFieldDateEdit FormPF=new FormPatFieldDateEdit(patField);
						FormPF.IsNew=true;
						FormPF.ShowDialog();
					}
					if(patFieldDef.FieldType==PatFieldType.Checkbox) {
						FormPatFieldCheckEdit FormPF=new FormPatFieldCheckEdit(patField);
						FormPF.IsNew=true;
						FormPF.ShowDialog();
					}
					if(patFieldDef.FieldType==PatFieldType.Currency) {
						FormPatFieldCurrencyEdit FormPF=new FormPatFieldCurrencyEdit(patField);
						FormPF.IsNew=true;
						FormPF.ShowDialog();
					}
					if(patFieldDef.FieldType==PatFieldType.InCaseOfEmergency) {
						//Deprecated
					}
				}
				else {//edit existing patfield
					if(patFieldDef.FieldType==PatFieldType.Text) {
						FormPatFieldEdit FormPF=new FormPatFieldEdit(patField);
						FormPF.IsLaunchedFromOrtho=isForOrtho;
						FormPF.ShowDialog();
					}
					if(patFieldDef.FieldType==PatFieldType.PickList) {
						FormPatFieldPickEdit FormPF=new FormPatFieldPickEdit(patField);
						FormPF.ShowDialog();
					}
					if(patFieldDef.FieldType==PatFieldType.Date) {
						FormPatFieldDateEdit FormPF=new FormPatFieldDateEdit(patField);
						FormPF.ShowDialog();
					}
					if(patFieldDef.FieldType==PatFieldType.Checkbox) {
						FormPatFieldCheckEdit FormPF=new FormPatFieldCheckEdit(patField);
						FormPF.ShowDialog();
					}
					if(patFieldDef.FieldType==PatFieldType.Currency) {
						FormPatFieldCurrencyEdit FormPF=new FormPatFieldCurrencyEdit(patField);
						FormPF.ShowDialog();
					}
					if(patFieldDef.FieldType==PatFieldType.InCaseOfEmergency) {
						//Deprecated
					}
				}
			}
			else if(patField!=null) {//PatField for a PatFieldDef that no longer exists
				FormPatFieldEdit FormPF=new FormPatFieldEdit(patField);
				FormPF.IsLaunchedFromOrtho=isForOrtho;
				FormPF.ShowDialog();
			}
		}
	}
}
