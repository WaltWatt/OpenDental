using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	
	///<summary></summary>
	public class PatientL{
		///<summary>Collection of Patient Names. The last five patients. Gets displayed on dropdown button.</summary>
		private static List<string> buttonLastFiveNames;
		///<summary>Collection of PatNums. The last five patients. Used when clicking on dropdown button.</summary>
		private static List<long> buttonLastFivePatNums;
		///<summary>Static variable to store the currently selected patient for updating the main title bar for the Update Time countdown.
		///Stored so we don't have to make a database call every second.</summary>
		private static Patient _patSelectedCur;
		///<summary>Static variable to store the currently selected clinicNum for updating the main title bar for the Update Time countdown.
		///Stored so we don't have to make a database call every second.</summary>
		private static long _clinSelectedCur;

		///<summary>Removes a patient from the dropdown menu.  Only used when Delete Patient is called.</summary>
		public static void RemoveFromMenu(string nameLF,long patNum) {
			//No need to check RemotingRole; no call to db.
			buttonLastFivePatNums.Remove(patNum);
			buttonLastFiveNames.Remove(nameLF);
		}

		///<summary>Removes all patients from the dropdown menu and from the history.  Only used when a user logs off and a different user logs on.  Important for enterprise customers with clinic restrictions for users.</summary>
		public static void RemoveAllFromMenu(ContextMenu menu) {
			menu.MenuItems.Clear();
			buttonLastFivePatNums.Clear();
			buttonLastFiveNames.Clear();
		}

		///<summary>The current patient will already be on the button.  This adds the family members when user clicks dropdown arrow. Can handle null values for pat and fam.  Need to supply the menu to fill as well as the EventHandler to set for each item (all the same).</summary>
		public static void AddFamilyToMenu(ContextMenu menu,EventHandler onClick,long patNum,Family fam) {
			//No need to check RemotingRole; no call to db.
			//fill menu
			menu.MenuItems.Clear();
			for(int i=0;i<buttonLastFiveNames.Count;i++) {
				menu.MenuItems.Add(buttonLastFiveNames[i].ToString(),onClick);
			}
			menu.MenuItems.Add("-");
			menu.MenuItems.Add("FAMILY");
			if(patNum!=0 && fam!=null) {
				for(int i=0;i<fam.ListPats.Length;i++) {
					menu.MenuItems.Add(fam.ListPats[i].GetNameLF(),onClick);
				}
			}
		}

		///<summary>Does not handle null values. Use zero.  Does not handle adding family members.  Returns true if patient has changed.</summary>
		public static bool AddPatsToMenu(ContextMenu menu,EventHandler onClick,string nameLF,long patNum) {
			//No need to check RemotingRole; no call to db.
			//add current patient
			if(buttonLastFivePatNums==null) {
				buttonLastFivePatNums=new List<long>();
			}
			if(buttonLastFiveNames==null) {
				buttonLastFiveNames=new List<string>();
			}
			if(patNum==0) {
				return false;
			}
			if(buttonLastFivePatNums.Count>0 && patNum==buttonLastFivePatNums[0]) {//same patient selected
				return false;
			}
			//Patient has changed
			int idx=buttonLastFivePatNums.IndexOf(patNum);
			if(idx>-1) {//It exists in this list of patnums
				buttonLastFivePatNums.RemoveAt(idx);
				buttonLastFiveNames.RemoveAt(idx);
			}
			buttonLastFivePatNums.Insert(0,patNum);
			buttonLastFiveNames.Insert(0,nameLF);
			if(buttonLastFivePatNums.Count>5) {
				buttonLastFivePatNums.RemoveAt(5);
				buttonLastFiveNames.RemoveAt(5);
			}
			return true;
		}

		///<summary>Determines which menu Item was selected from the Patient dropdown list and returns the patNum for that patient. This will not be activated when click on 'FAMILY' or on separator, because they do not have events attached.  Calling class then does a ModuleSelected.</summary>
		public static long ButtonSelect(ContextMenu menu,object sender,Family fam) {
			//No need to check RemotingRole; no call to db.
			int index=menu.MenuItems.IndexOf((MenuItem)sender);
			//Patients.PatIsLoaded=true;
			if(index<buttonLastFivePatNums.Count) {
				return (long)buttonLastFivePatNums[index];
			}
			if(fam==null) {
				return 0;//will never happen
			}
			return fam.ListPats[index-buttonLastFivePatNums.Count-2].PatNum;
		}

		///<summary>Accepts null for pat and 0 for clinicNum.</summary>
		public static string GetMainTitle(Patient pat,long clinicNum) {
			string retVal=PrefC.GetString(PrefName.MainWindowTitle);
			object[] parameters = { retVal };
			Plugins.HookAddCode(null,"PatientL.GetMainTitle_beginning",parameters);
			retVal = (string)parameters[0];
			_patSelectedCur=pat;
			_clinSelectedCur=clinicNum;
			if(!PrefC.GetBool(PrefName.EasyNoClinics) && clinicNum>0) {
				if(retVal!="") {
					retVal+=" - "+Lan.g("FormOpenDental","Clinic")+": ";
				}
				if(PrefC.GetBool(PrefName.TitleBarClinicUseAbbr)) {
					retVal+=Clinics.GetAbbr(clinicNum);
				}
				else {
					retVal+=Clinics.GetDesc(clinicNum);
				}		
			}
			if(Security.CurUser!=null){
				retVal+=" {"+Security.CurUser.UserName+"}";
			}
			if(pat==null || pat.PatNum==0 || pat.PatNum==-1){
				retVal+=MainTitleUpdateCountdown();
				if(FormOpenDental.RegKeyIsForTesting) {
					retVal+=" - "+Lan.g("FormOpenDental","Developer Only License")+" - "+Lan.g("FormOpenDental","Not for use with live patient data")+" - ";
				}
				return retVal;
			}
			retVal+=" - "+pat.GetNameLF();
			if(PrefC.GetLong(PrefName.ShowIDinTitleBar)==1) {
				retVal+=" - "+pat.PatNum.ToString();
			}
			else if(PrefC.GetLong(PrefName.ShowIDinTitleBar)==2) {
				retVal+=" - "+pat.ChartNumber;
			}
			else if(PrefC.GetLong(PrefName.ShowIDinTitleBar)==3) {
				if(pat.Birthdate.Year>1880) {
					retVal+=" - "+pat.Birthdate.ToShortDateString();
				}
			}
			if(pat.SiteNum!=0){
				retVal+=" - "+Sites.GetDescription(pat.SiteNum);
			}
			retVal+=MainTitleUpdateCountdown();
			if(FormOpenDental.RegKeyIsForTesting) {
				retVal+=" - "+Lan.g("FormOpenDental","Developer Only License")+" - "+Lan.g("FormOpenDental","Not for use with live patient data")+" - ";
			}
			return retVal;
		}

		///<summary>Used to update the main title bar when neither the patient nor the clinic need to change. 
		///Currently only used to refresh the title bar when the timer ticks for the update time countdown.</summary>
		public static string GetMainTitleSamePat() {
			string retVal=PrefC.GetString(PrefName.MainWindowTitle);
			if(!PrefC.GetBool(PrefName.EasyNoClinics) && _clinSelectedCur>0) {
				if(retVal!="") {
					retVal+=" - "+Lan.g("FormOpenDental","Clinic")+": ";
				}
				if(PrefC.GetBool(PrefName.TitleBarClinicUseAbbr)) {
					retVal+=Clinics.GetAbbr(_clinSelectedCur);
				}
				else {
					retVal+=Clinics.GetDesc(_clinSelectedCur);
				}
			}
			if(Security.CurUser!=null) {
				retVal+=" {"+Security.CurUser.UserName+"}";
			}
			if(_patSelectedCur==null || _patSelectedCur.PatNum==0 || _patSelectedCur.PatNum==-1) {
				retVal+=MainTitleUpdateCountdown();
				if(FormOpenDental.RegKeyIsForTesting) {
					retVal+=" - "+Lan.g("FormOpenDental","Developer Only License")+" - "+Lan.g("FormOpenDental","Not for use with live patient data")+" - ";
				}
				return retVal;
			}
			retVal+=" - "+_patSelectedCur.GetNameLF();
			if(PrefC.GetLong(PrefName.ShowIDinTitleBar)==1) {
				retVal+=" - "+_patSelectedCur.PatNum.ToString();
			}
			else if(PrefC.GetLong(PrefName.ShowIDinTitleBar)==2) {
				retVal+=" - "+_patSelectedCur.ChartNumber;
			}
			else if(PrefC.GetLong(PrefName.ShowIDinTitleBar)==3) {
				if(_patSelectedCur.Birthdate.Year>1880) {
					retVal+=" - "+_patSelectedCur.Birthdate.ToShortDateString();
				}
			}
			if(_patSelectedCur.SiteNum!=0) {
				retVal+=" - "+Sites.GetDescription(_patSelectedCur.SiteNum);
			}
			retVal+=MainTitleUpdateCountdown();
			if(FormOpenDental.RegKeyIsForTesting) {
				retVal+=" - "+Lan.g("FormOpenDental","Developer Only License")+" - "+Lan.g("FormOpenDental","Not for use with live patient data")+" - ";
			}
			return retVal;
		}

		public static string MainTitleUpdateCountdown(string titleText = "") {
			string retVal;
			TimeSpan timeLeft=PrefC.GetDateT(PrefName.UpdateDateTime) - DateTime.Now;
			string strTimeLeft="";
			if(timeLeft.TotalSeconds>=0) {
				if(timeLeft.Days>=1) {
					strTimeLeft=timeLeft.Days+" "+Lan.g("FormOpenDental","days")+", "+timeLeft.Hours+" "+Lan.g("FormOpenDental","hours");
				}
				else if(timeLeft.Hours>=1) {
					strTimeLeft=timeLeft.Hours+" "+Lan.g("FormOpenDental","hours")+", "+timeLeft.Minutes+" "+Lan.g("FormOpenDental","minutes");
				}
				else if(timeLeft.Minutes>=1) {
					strTimeLeft=timeLeft.Minutes+" "+Lan.g("FormOpenDental","minutes")+", "+timeLeft.Seconds+" "+Lan.g("FormOpenDental","seconds");
				}
				else if(timeLeft.Seconds>=0) {
					strTimeLeft=timeLeft.Seconds+" "+Lan.g("FormOpenDental","seconds");
				}
			}
			else {
				return titleText;
			}
			retVal=titleText+"        "+Lan.g("FormOpenDental","Update In")+": "+strTimeLeft;
			return retVal;
		}
	}


}










