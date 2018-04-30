using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public class DefL {
		private static string _lanThis="FormDefinitions";
		#region GetMethods
		public static List<DefCatOptions> GetOptionsForDefCats(Array defCatVals) {
			List<DefCatOptions> listDefCatOptions = new List<DefCatOptions>();
			foreach(DefCat defCatCur in defCatVals) {
				if(defCatCur.GetDescription() == "NotUsed") {
					continue;
				}
				if(defCatCur.GetDescription().Contains("HqOnly") && !PrefC.IsODHQ) {
					continue;
				}
				DefCatOptions defCOption=new DefCatOptions(defCatCur);
				switch(defCatCur) {
					case DefCat.AccountColors:
						defCOption.CanEditName=false;
						defCOption.EnableColor=true;
						defCOption.HelpText=Lans.g("FormDefinitions","Changes the color of text for different types of entries in Account Module");
						break;
					case DefCat.AccountQuickCharge:
						defCOption.CanDelete=true;
						defCOption.EnableValue=true;
						defCOption.ValueText=Lans.g("FormDefinitions","Procedure Codes");
						defCOption.HelpText=Lans.g("FormDefinitions","Account Proc Quick Add items.  Each entry can be a series of procedure codes separated by commas (e.g. D0180,D1101,D8220).  Used in the account module to quickly charge patients for items.");
						break;
					case DefCat.AdjTypes:
						defCOption.EnableValue=true;
						defCOption.ValueText=Lans.g("FormDefinitions","+, -, or dp");
						defCOption.HelpText=Lans.g("FormDefinitions","Plus increases the patient balance.  Minus decreases it.  Dp means discount plan.  Not allowed to change value after creating new type since changes affect all patient accounts.");
						break;
					case DefCat.AppointmentColors:
						defCOption.CanEditName=false;
						defCOption.EnableColor=true;
						defCOption.HelpText=Lans.g("FormDefinitions","Changes colors of background in Appointments Module, and colors for completed appointments.");
						break;
					case DefCat.ApptConfirmed:
						defCOption.EnableColor=true;
						defCOption.EnableValue=true;
						defCOption.ValueText=Lans.g("FormDefinitions","Abbrev");
						defCOption.HelpText=Lans.g("FormDefinitions","Color shows on each appointment if Appointment View is set to show ConfirmedColor.");
						break;
					case DefCat.ApptProcsQuickAdd:
						defCOption.EnableValue=true;
						defCOption.ValueText=Lans.g("FormDefinitions","ADA Code(s)");
						if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
							defCOption.HelpText=Lans.g("FormDefinitions","These are the procedures that you can quickly add to the treatment plan from within the appointment editing window.  Multiple procedures may be separated by commas with no spaces. These definitions may be freely edited without affecting any patient records.");
						}
						else {
							defCOption.HelpText=Lans.g("FormDefinitions","These are the procedures that you can quickly add to the treatment plan from within the appointment editing window.  They must not require a tooth number. Multiple procedures may be separated by commas with no spaces. These definitions may be freely edited without affecting any patient records.");
						}
						break;
					case DefCat.AutoDeposit:
						defCOption.CanDelete=true;
						defCOption.CanHide=true;
						defCOption.EnableValue=true;
						defCOption.ValueText=Lans.g("FormDefinitions","Account Number");
						break;
					case DefCat.AutoNoteCats:
						defCOption.CanDelete=true;
						defCOption.CanHide=false;
						defCOption.EnableValue=true;
						defCOption.IsValueDefNum=true;
						defCOption.ValueText=Lans.g("FormDefinitions","Parent Category");
						defCOption.HelpText=Lans.g("FormDefinitions","Leave the Parent Category blank for categories at the root level. Assign a Parent Category to move a category within another. The order set here will only affect the order within the assigned Parent Category in the Auto Note list. For example, a category may be moved above its parent in this list, but it will still be within its Parent Category in the Auto Note list.");
						break;
					case DefCat.BillingTypes:
						defCOption.EnableValue=true;
						defCOption.ValueText=Lans.g("FormDefinitions","E=Email bill, C=Collection");
						defCOption.HelpText=Lans.g("FormDefinitions","It is recommended to use as few billing types as possible.  They can be useful when running reports to separate delinquent accounts, but can cause 'forgotten accounts' if used without good office procedures. Changes affect all patients.");
						break;
					case DefCat.BlockoutTypes:
						defCOption.EnableColor=true;
						defCOption.HelpText=Lans.g("FormDefinitions","Blockout types are used in the appointments module.");
						defCOption.EnableValue=true;
						defCOption.ValueText=Lans.g("FormDefinitions","Flags");
						break;
					case DefCat.ChartGraphicColors:
						defCOption.CanEditName=false;
						defCOption.EnableColor=true;
						if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
							defCOption.HelpText=Lans.g("FormDefinitions","These colors will be used to graphically display treatments.");
						}
						else {
							defCOption.HelpText=Lans.g("FormDefinitions","These colors will be used on the graphical tooth chart to draw restorations.");
						}
						break;
					case DefCat.ClaimCustomTracking:
						defCOption.CanDelete=true;
						defCOption.CanHide=false;
						defCOption.EnableValue=true;
						defCOption.ValueText=Lans.g("FormDefinitions","Days Suppressed");
						defCOption.HelpText=Lans.g("FormDefinitions","Some offices may set up claim tracking statuses such as 'review', 'hold', 'riskmanage', etc.")+"\r\n"
							+Lans.g("FormDefinitions","Set the value of 'Days Suppressed' to the number of days the claim will be suppressed from the Outstanding Claims Report "
							+"when the status is changed to the selected status.");
						break;
					case DefCat.ClaimErrorCode:
						defCOption.CanDelete=true;
						defCOption.CanHide=false;
						defCOption.EnableValue=true;
						defCOption.ValueText=Lans.g("FormDefinitions","Description");
						defCOption.HelpText=Lans.g("FormDefinitions","Used to track error codes when entering claim custom statuses.");
						break;
					case DefCat.ClaimPaymentTracking:
						defCOption.ValueText=Lans.g("FormDefinitions","Value");
						defCOption.HelpText=Lans.g("FormDefinitions","EOB adjudication method codes to be used for insurance payments.  Last entry cannot be hidden.");
						break;
					case DefCat.ClaimPaymentGroups:
						defCOption.ValueText=Lans.g("FormDefinitions","Value");
						defCOption.HelpText=Lans.g("FormDefinitions","Used to group claim payments in the daily payments report.");
						break;
					case DefCat.ClinicSpecialty:
						defCOption.CanHide=true;
						defCOption.CanDelete=false;
						defCOption.HelpText=Lans.g("FormDefinitions","You can add as many specialties as you want.  Changes affect all current records.");
						break;
					case DefCat.CommLogTypes:
						defCOption.EnableValue=true;
						defCOption.ValueText=Lans.g("FormDefinitions","APPT,FIN,RECALL,MISC,TEXT");
						defCOption.HelpText=Lans.g("FormDefinitions","Changes affect all current commlog entries.  In the second column, you can optionally specify APPT,FIN,RECALL,MISC,or TEXT. Only one of each. This helps automate new entries.");
						break;
					case DefCat.ContactCategories:
						defCOption.HelpText=Lans.g("FormDefinitions","You can add as many categories as you want.  Changes affect all current contact records.");
						break;
					case DefCat.Diagnosis:
						defCOption.EnableValue=true;
						defCOption.ValueText=Lans.g("FormDefinitions","1 or 2 letter abbreviation");
						defCOption.HelpText=Lans.g("FormDefinitions","The diagnosis list is shown when entering a procedure.  Ones that are less used should go lower on the list.  The abbreviation is shown in the progress notes.  BE VERY CAREFUL.  Changes affect all patients.");
						break;
					case DefCat.FeeColors:
						defCOption.CanEditName=false;
						defCOption.CanHide=false;
						defCOption.EnableColor=true;
						defCOption.HelpText=Lans.g("FormDefinitions","These are the colors associated to fee types.");
						break;
					case DefCat.ImageCats:
						defCOption.ValueText=Lans.g("FormDefinitions","Usage");
						defCOption.HelpText=Lans.g("FormDefinitions","These are the categories that will be available in the image and chart modules.  If you hide a category, images in that category will be hidden, so only hide a category if you are certain it has never been used.  Multiple categories can be set to show in the Chart module, but only one category should be set for patient pictures, statements, and tooth charts. Selecting multiple categories for treatment plans will save the treatment plan in each category. Affects all patient records.");
						break;
					case DefCat.InsurancePaymentType:
						defCOption.CanDelete=true;
						defCOption.CanHide=false;
						defCOption.EnableValue=true;
						defCOption.ValueText=Lans.g("FormDefinitions","N=Not selected for deposit");
						defCOption.HelpText=Lans.g("FormDefinitions","These are claim payment types for insurance payments attached to claims.");
						break;
					case DefCat.InsuranceVerificationStatus:
						defCOption.HelpText=Lans.g("FormDefinitions","These are statuses for the insurance verification list.");
						break;
					case DefCat.JobPriorities:
						defCOption.CanDelete=false;
						defCOption.CanHide=true;
						defCOption.EnableValue=true;
						defCOption.EnableColor=true;
						defCOption.ValueText=Lans.g("FormDefinitions","Comma-delimited keywords");
						defCOption.HelpText=Lans.g("FormDefinitions","These are job priorities that determine how jobs are sorted in the Job Manager System.  Required values are: OnHold, Low, Normal, MediumHigh, High, Urgent, BugDefault, JobDefault, DocumentationDefault.");
						break;
					case DefCat.LetterMergeCats:
						defCOption.HelpText=Lans.g("FormDefinitions","Categories for Letter Merge.  You can safely make any changes you want.");
						break;
					case DefCat.MiscColors:
						defCOption.CanEditName=false;
						defCOption.EnableColor=true;
						defCOption.HelpText="";
						break;
					case DefCat.PaymentTypes:
						defCOption.EnableValue=true;
						defCOption.ValueText=Lans.g("FormDefinitions","N=Not selected for deposit");
						defCOption.HelpText=Lans.g("FormDefinitions","Types of payments that patients might make. Any changes will affect all patients.");
						break;
					case DefCat.PayPlanCategories:
						defCOption.HelpText=Lans.g("FormDefinitions","Assign payment plans to different categories");
						break;
					case DefCat.PaySplitUnearnedType:
						defCOption.HelpText=Lans.g("FormDefinitions","Usually only used by offices that use accrual basis accounting instead of cash basis accounting. Any changes will affect all patients.");
						break;
					case DefCat.ProcButtonCats:
						defCOption.HelpText=Lans.g("FormDefinitions","These are similar to the procedure code categories, but are only used for organizing and grouping the procedure buttons in the Chart module.");
						break;
					case DefCat.ProcCodeCats:
						defCOption.HelpText=Lans.g("FormDefinitions","These are the categories for organizing procedure codes. They do not have to follow ADA categories.  There is no relationship to insurance categories which are setup in the Ins Categories section.  Does not affect any patient records.");
						break;
					case DefCat.ProgNoteColors:
						defCOption.CanEditName=false;
						defCOption.EnableColor=true;
						defCOption.HelpText=Lans.g("FormDefinitions","Changes color of text for different types of entries in the Chart Module Progress Notes.");
						break;
					case DefCat.Prognosis:
						//Nothing special. Might add HelpText later.
						break;
					case DefCat.ProviderSpecialties:
						defCOption.HelpText=Lans.g("FormDefinitions","Provider specialties cannot be deleted.  Changes to provider specialties could affect e-claims.");
						break;
					case DefCat.RecallUnschedStatus:
						defCOption.EnableValue=true;
						defCOption.ValueText=Lans.g("FormDefinitions","Abbreviation");
						defCOption.HelpText=Lans.g("FormDefinitions","Recall/Unsched Status.  Abbreviation must be 7 characters or less.  Changes affect all patients.");
						break;
					case DefCat.Regions:
						defCOption.CanHide=false;
						defCOption.HelpText=Lans.g("FormDefinitions","The region identifying the clinic it is assigned to.");
						break;
					case DefCat.SupplyCats:
						defCOption.CanDelete=true;
						defCOption.CanHide=false;
						defCOption.HelpText=Lans.g("FormDefinitions","The categories for inventory supplies.");
						break;
					case DefCat.TaskPriorities:
						defCOption.EnableColor=true;
						defCOption.EnableValue=true;
						defCOption.ValueText=Lans.g("FormDefinitions","D = Default, R = Reminder");
						defCOption.HelpText=Lans.g("FormDefinitions","Priorities available for selection within the task edit window.  Task lists are sorted using the order of these priorities.  They can have any description and color.  At least one priority should be Default (D).  If more than one priority is flagged as the default, the last default in the list will be used.  If no default is set, the last priority will be used.  Use (R) to indicate the initial reminder task priority to use when creating reminder tasks.  Changes affect all tasks where the definition is used.");
						break;
					case DefCat.TxPriorities:
						defCOption.EnableColor=true;
						defCOption.HelpText=Lans.g("FormDefinitions","Priorities available for selection in the Treatment Plan module.  They can be simple numbers or descriptive abbreviations 7 letters or less.  Changes affect all procedures where the definition is used.");
						break;
					case DefCat.WebSchedNewPatApptTypes:
						defCOption.CanDelete=true;
						defCOption.CanHide=false;
						defCOption.HelpText=Lans.g("FormDefinitions","Appointment types to be displayed in the Web Sched New Pat Appt web application.  These are selectable for the new patients and will be saved to the appointment note.");
						break;
					case DefCat.CarrierGroupNames:
						defCOption.CanHide=true;
						defCOption.HelpText=Lans.g("FormDefinitions","These are group names for Carriers.");
						break;
				}
				listDefCatOptions.Add(defCOption);
			}
			return listDefCatOptions;
		}

		private static string GetItemDescForImages(string itemValue) {
			List<string> listVals=new List<string>();
			if(itemValue.Contains("X")) {
				listVals.Add(Lan.g(_lanThis,"ChartModule"));
			}
			if(itemValue.Contains("F")) {
				listVals.Add(Lan.g(_lanThis,"PatientForm"));
			}
			if(itemValue.Contains("P")){
				listVals.Add(Lan.g(_lanThis,"PatientPic"));
			}
			if(itemValue.Contains("S")){
				listVals.Add(Lan.g(_lanThis,"Statement"));
			}
			if(itemValue.Contains("T")){
				listVals.Add(Lan.g(_lanThis,"ToothChart"));
			}
			if(itemValue.Contains("R")) {
				listVals.Add(Lan.g(_lanThis,"TreatPlans"));
			}
			if(itemValue.Contains("L")) {
				listVals.Add(Lan.g(_lanThis,"PatientPortal"));
			}
			if(itemValue.Contains("A")) {
				listVals.Add(Lan.g(_lanThis,"PayPlans"));
			}
			return string.Join(", ",listVals);
		}
		#endregion
		///<summary>Fills the passed in grid with the definitions in the passed in list.</summary>
		public static void FillGridDefs(ODGrid gridDefs,DefCatOptions selectedDefCatOpt,List<Def> listDefsCur) {
			Def selectedDef=null;
			if(gridDefs.GetSelectedIndex() > -1) {
				selectedDef=(Def)gridDefs.Rows[gridDefs.GetSelectedIndex()].Tag;
			}
			int scroll=gridDefs.ScrollValue;
			gridDefs.BeginUpdate();
			gridDefs.Columns.Clear();
			ODGridColumn col;
			col = new ODGridColumn(Lan.g("TableDefs","Name"),190);
			gridDefs.Columns.Add(col);
			col = new ODGridColumn(selectedDefCatOpt.ValueText,190);
			gridDefs.Columns.Add(col);
			col = new ODGridColumn(selectedDefCatOpt.EnableColor ? Lan.g("TableDefs","Color") : "",40);
			gridDefs.Columns.Add(col);
			col = new ODGridColumn(selectedDefCatOpt.CanHide ? Lan.g("TableDefs","Hide") : "",30,HorizontalAlignment.Center);
			gridDefs.Columns.Add(col);
			gridDefs.Rows.Clear();
			ODGridRow row;
			foreach(Def defCur in listDefsCur) {
				if(!PrefC.IsODHQ && defCur.ItemValue==CommItemTypeAuto.ODHQ.ToString()) {
					continue;
				}
				if(Defs.IsDefDeprecated(defCur)) {
					defCur.IsHidden=true;
				}
				row=new ODGridRow();
				if(selectedDefCatOpt.CanEditName) {
					row.Cells.Add(defCur.ItemName);
				}
				else {//Users cannot edit the item name so let them translate them.
					row.Cells.Add(Lan.g("FormDefinitions",defCur.ItemName));//Doesn't use 'this' so that renaming the form doesn't change the translation
				}
				if(selectedDefCatOpt.DefCat==DefCat.ImageCats) {
					row.Cells.Add(GetItemDescForImages(defCur.ItemValue));
				}
				else if(selectedDefCatOpt.DefCat==DefCat.AutoNoteCats) {
					Dictionary<string,string> dictAutoNoteDefs = new Dictionary<string,string>();
					dictAutoNoteDefs=listDefsCur.ToDictionary(x => x.DefNum.ToString(),x => x.ItemName);
					string nameCur;
					row.Cells.Add(dictAutoNoteDefs.TryGetValue(defCur.ItemValue,out nameCur) ? nameCur : defCur.ItemValue);
				}
				else if(selectedDefCatOpt.DefCat==DefCat.WebSchedNewPatApptTypes) {
					AppointmentType appointmentType=AppointmentTypes.GetWebSchedNewPatApptTypeByDef(defCur.DefNum);
					row.Cells.Add(appointmentType==null ? "" : appointmentType.AppointmentTypeName);
				}
				else if(selectedDefCatOpt.DoShowItemOrderInValue) {
					row.Cells.Add(defCur.ItemOrder.ToString());
				}
				else {
					row.Cells.Add(defCur.ItemValue);
				}
				row.Cells.Add("");
				if(selectedDefCatOpt.EnableColor) {
					row.Cells[row.Cells.Count-1].CellColor=defCur.ItemColor;
				}
				if(defCur.IsHidden) {
					row.Cells.Add("X");
				}
				else {
					row.Cells.Add("");
				}
				row.Tag=defCur;
				gridDefs.Rows.Add(row);
			}
			gridDefs.EndUpdate();
			if(selectedDef!=null) {
				for(int i=0;i < gridDefs.Rows.Count;i++) {
					if(((Def)gridDefs.Rows[i].Tag).DefNum == selectedDef.DefNum) {
						gridDefs.SetSelected(i,true);
						break;
					}
				}
			}
			gridDefs.ScrollValue=scroll;
		}

		public static bool GridDefsDoubleClick(Def selectedDef,ODGrid gridDefs,DefCatOptions selectedDefCatOpt,List<Def> listDefsCur,List<Def> listDefsAll,bool isDefChanged) {
			switch(selectedDefCatOpt.DefCat) {
				case DefCat.BlockoutTypes:
					FormDefEditBlockout FormDEB=new FormDefEditBlockout(selectedDef);
					FormDEB.ShowDialog();
					if(FormDEB.DialogResult==DialogResult.OK) {
						isDefChanged=true;
					}
					break;
				case DefCat.ImageCats:
					FormDefEditImages FormDEI=new FormDefEditImages(selectedDef);
					FormDEI.IsNew=false;
					FormDEI.ShowDialog();
					if(FormDEI.DialogResult==DialogResult.OK) {
						isDefChanged=true;
					}
					break;
				case DefCat.WebSchedNewPatApptTypes:
					FormDefEditWSNPApptTypes FormDEWSNPAT=new FormDefEditWSNPApptTypes(selectedDef);
					if(FormDEWSNPAT.ShowDialog()==DialogResult.OK) {
						if(FormDEWSNPAT.IsDeleted) {
							listDefsAll.Remove(selectedDef);
						}
						isDefChanged=true;
					}
					break;
				default://Show the normal FormDefEdit window.
					FormDefEdit FormDefEdit2=new FormDefEdit(selectedDef,listDefsCur,selectedDefCatOpt);
					FormDefEdit2.IsNew=false;
					FormDefEdit2.ShowDialog();
					if(FormDefEdit2.DialogResult==DialogResult.OK) {
						if(FormDefEdit2.IsDeleted) {
							listDefsAll.Remove(selectedDef);
						}
						isDefChanged=true;
					}
					break;
			}
			return isDefChanged;
		}

		public static bool AddDef(ODGrid gridDefs,DefCatOptions selectedDefCatOpt) {
			Def defCur=new Def();
			defCur.IsNew=true;
			int itemOrder=0;
			if(Defs.GetDefsForCategory(selectedDefCatOpt.DefCat).Count>0) {
				itemOrder=Defs.GetDefsForCategory(selectedDefCatOpt.DefCat).Max(x => x.ItemOrder) + 1;
			}
			defCur.ItemOrder=itemOrder;
			defCur.Category=selectedDefCatOpt.DefCat;
			defCur.ItemName="";
			defCur.ItemValue="";//necessary
			if(selectedDefCatOpt.DefCat==DefCat.InsurancePaymentType) {
				defCur.ItemValue="N";
			}
			switch(selectedDefCatOpt.DefCat) {
				case DefCat.BlockoutTypes:
					FormDefEditBlockout FormDEB=new FormDefEditBlockout(defCur);
					FormDEB.IsNew=true;
					if(FormDEB.ShowDialog()!=DialogResult.OK) {
						return false;
					}
					break;
				case DefCat.ImageCats:
					FormDefEditImages FormDEI=new FormDefEditImages(defCur);
					FormDEI.IsNew=true;
					FormDEI.ShowDialog();
					if(FormDEI.DialogResult!=DialogResult.OK) {
						return false;
					}
					break;
				case DefCat.WebSchedNewPatApptTypes:
					FormDefEditWSNPApptTypes FormDEWSNPAT=new FormDefEditWSNPApptTypes(defCur);
					if(FormDEWSNPAT.ShowDialog()!=DialogResult.OK) { 
						return false;
					}
					break;
				default:
					List<Def> listCurrentDefs=new List<Def>();
					foreach(ODGridRow rowCur in gridDefs.Rows) {
						listCurrentDefs.Add((Def)rowCur.Tag);
					}
					FormDefEdit FormDE=new FormDefEdit(defCur,listCurrentDefs,selectedDefCatOpt);
					FormDE.IsNew=true;
					FormDE.ShowDialog();
					if(FormDE.DialogResult!=DialogResult.OK) {
						return false;
					}
					break;
			}
			return true;
		}

		public static bool HideDef(ODGrid gridDefs,DefCatOptions selectedDefCatOpt) {
			if(gridDefs.GetSelectedIndex()==-1){
				MsgBox.Show(_lanThis,"Please select item first,");
				return false;
			}
			Def selectedDef = (Def)gridDefs.Rows[gridDefs.GetSelectedIndex()].Tag;
			//Warn the user if they are about to hide a billing type currently in use.
			if(selectedDefCatOpt.DefCat==DefCat.BillingTypes && Patients.IsBillingTypeInUse(selectedDef.DefNum)) {
				if(!MsgBox.Show(_lanThis,MsgBoxButtons.OKCancel,"Warning: Billing type is currently in use by patients, insurance plans, or preferences.")) {
					return false;
				}
			}
			if(selectedDef.Category==DefCat.ProviderSpecialties
				&& (Providers.IsSpecialtyInUse(selectedDef.DefNum)
				|| Referrals.IsSpecialtyInUse(selectedDef.DefNum)))
			{
				MsgBox.Show(_lanThis,"You cannot hide a specialty if it is in use by a provider or a referral source.");
				return false;
			}
			if(Defs.IsDefinitionInUse(selectedDef)) {
				if(selectedDef.DefNum==PrefC.GetLong(PrefName.BrokenAppointmentAdjustmentType)
					|| selectedDef.DefNum==PrefC.GetLong(PrefName.AppointmentTimeArrivedTrigger)
					|| selectedDef.DefNum==PrefC.GetLong(PrefName.AppointmentTimeSeatedTrigger)
					|| selectedDef.DefNum==PrefC.GetLong(PrefName.AppointmentTimeDismissedTrigger)
					|| selectedDef.DefNum==PrefC.GetLong(PrefName.TreatPlanDiscountAdjustmentType)
					|| selectedDef.DefNum==PrefC.GetLong(PrefName.BillingChargeAdjustmentType)
					|| selectedDef.DefNum==PrefC.GetLong(PrefName.PracticeDefaultBillType)
					|| selectedDef.DefNum==PrefC.GetLong(PrefName.FinanceChargeAdjustmentType)) 
				{
					MsgBox.Show(_lanThis,"You cannot hide a definition if it is in use within Module Preferences.");
					return false;
				}
				else {
					if(!MsgBox.Show(_lanThis,MsgBoxButtons.OKCancel,"Warning: This definition is currently in use within the program.")) {
						return false;
					}
				}
			}
			//Stop users from hiding the last definition in categories that must have at least one def in them.
			if(Defs.IsHidable(selectedDef.Category))	{
				List<Def> listDefsCurNotHidden = Defs.GetDefsForCategory(selectedDefCatOpt.DefCat,true);
				if(listDefsCurNotHidden.Count ==1) {
					MsgBox.Show(_lanThis,"You cannot hide the last definition in this category.");
					return false;
				}
			}
			Defs.HideDef(selectedDef);
			return true;
		}

		public static bool UpClick(ODGrid gridDefs,List<Def> listDefsCur) {
			if(gridDefs.GetSelectedIndex()==-1){
				MessageBox.Show(Lan.g("Defs","Please select an item first."));
				return false;
			}
			if(gridDefs.GetSelectedIndex()==0) {
				return false;
			}
			Def defSelected = listDefsCur[gridDefs.GetSelectedIndex()];
			Def defAbove = listDefsCur[gridDefs.GetSelectedIndex()-1];
			defSelected.ItemOrder--;
			defAbove.ItemOrder++;
			Defs.Update(defSelected);
			Defs.Update(defAbove);
			return true;
		}

		public static bool DownClick(ODGrid gridDefs,List<Def> listDefsCur) {
			if(gridDefs.GetSelectedIndex()==-1){
				MessageBox.Show(Lan.g("Defs","Please select an item first."));
				return false;
			}
			if(gridDefs.GetSelectedIndex()==gridDefs.Rows.Count-1) {
				return false;
			}
			Def defSelected = listDefsCur[gridDefs.GetSelectedIndex()];
			Def deBelow = listDefsCur[gridDefs.GetSelectedIndex()+1];
			defSelected.ItemOrder++;
			deBelow.ItemOrder--;
			Defs.Update(defSelected);
			Defs.Update(deBelow);
			return true;
		}

	}
}
