using CodeBase;
using Microsoft.Win32;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.Mobile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;
using System.Data;
using System.Linq;
using System.IO;
using WebServiceSerializer;
using OpenDentBusiness.WebServiceMainHQ;
using OpenDentBusiness.WebTypes.WebSched.TimeSlot;
using OpenDental.User_Controls;

namespace OpenDental {
	public partial class FormEServicesSetup {
		///<summary>Keeps track of the last selected index for the Web Sched New Pat Appt URL grid.</summary>
		private int _indexLastNewPatURL=-1;
		private ODThread _threadFillGridWebSchedNewPatApptTimeSlots=null;
		///<summary>Set to true whenever the Web Sched new pat appt thread is already running while another thing wants it to refresh yet again.
		///E.g. The window loads which initially starts a fill thread and then the user quickly starts changing filters.</summary>
		private bool _isWebSchedNewPatApptTimeSlotsOutdated=false;
		//List of a List that holds information on what preferences have been changed
		private List<ClinicPref> _listClinicPrefsWebSchedNewPats=new List<ClinicPref>();

		private bool IsTabValidWebSchedNewPat() {
			return true;
		}

		private void FillTabWebSchedNewPat() {
			int newPatApptDays=PrefC.GetInt(PrefName.WebSchedNewPatApptSearchAfterDays);
			textWebSchedNewPatApptMessage.Text=PrefC.GetString(PrefName.WebSchedNewPatApptMessage);
			textWebSchedNewPatApptSearchDays.Text=newPatApptDays>0 ? newPatApptDays.ToString() : "";
			checkWebSchedNewPatForcePhoneFormatting.Checked=PrefC.GetBool(PrefName.WebSchedNewPatApptForcePhoneFormatting);
			DateTime dateWebSchedNewPatSearch=DateTime.Now;
			dateWebSchedNewPatSearch=dateWebSchedNewPatSearch.AddDays(newPatApptDays);
			textWebSchedNewPatApptsDateStart.Text=dateWebSchedNewPatSearch.ToShortDateString();
			FillListBoxWebSchedBlockoutTypes(PrefC.GetString(PrefName.WebSchedNewPatApptIgnoreBlockoutTypes).Split(new char[] { ',' }),listboxWebSchedNewPatIgnoreBlockoutTypes);
			FillGridWebSchedNewPatApptHostedURLs();
			FillGridWSNPAReasons();
			FillGridWebSchedNewPatApptOps();
			//This needs to happen after all of the previous fills because it's asynchronous.
			FillGridWebSchedNewPatApptTimeSlotsThreaded();
			long defaultStatus=PrefC.GetLong(PrefName.WebSchedNewPatConfirmStatus);
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.ApptConfirmed,true);
			for(int i=0;i<listDefs.Count;i++) {
				int idx=comboWSNPConfirmStatuses.Items.Add(listDefs[i].ItemName);
				if(listDefs[i].DefNum==defaultStatus) {
					comboWSNPConfirmStatuses.SelectedIndex=idx;
				}
			}
			comboWSNPConfirmStatuses.IndexSelectOrSetText(listDefs.ToList().FindIndex(x => x.DefNum==defaultStatus),
			   () => { return defaultStatus==0 ? "" : Defs.GetName(DefCat.ApptConfirmed,defaultStatus)+" ("+Lan.g(this,"hidden")+")"; });
			checkNewPatAllowProvSelection.Checked=PrefC.GetBool(PrefName.WebSchedNewPatAllowProvSelection);
		}

		private void SaveTabWebSchedNewPat() {
			Prefs.UpdateString(PrefName.WebSchedNewPatApptMessage,textWebSchedNewPatApptMessage.Text);
			List<ClinicPref> listClinicPrefs=new List<ClinicPref>();
			foreach(Control control in panelHostedURLs.Controls) {
				if(control.GetType()!=typeof(ContrNewPatHostedURL)) {
					continue;
				}
				ContrNewPatHostedURL urlPanel=(ContrNewPatHostedURL)control;
				long clinicNum=urlPanel.GetClinicNum();
				string allowChildren=urlPanel.GetPrefValue(PrefName.WebSchedNewPatAllowChildren);
				string verifyInfo=urlPanel.GetPrefValue(PrefName.WebSchedNewPatVerifyInfo);
				string doAuthEmail=urlPanel.GetPrefValue(PrefName.WebSchedNewPatDoAuthEmail);
				string doAuthText=urlPanel.GetPrefValue(PrefName.WebSchedNewPatDoAuthText);
				string webFormsURL=urlPanel.GetPrefValue(PrefName.WebSchedNewPatWebFormsURL);
				if(clinicNum==0) {
					Prefs.UpdateString(PrefName.WebSchedNewPatAllowChildren,allowChildren);
					Prefs.UpdateString(PrefName.WebSchedNewPatVerifyInfo,verifyInfo);
					Prefs.UpdateString(PrefName.WebSchedNewPatDoAuthEmail,doAuthEmail);
					Prefs.UpdateString(PrefName.WebSchedNewPatDoAuthText,doAuthText);
					Prefs.UpdateString(PrefName.WebSchedNewPatWebFormsURL,webFormsURL);
					continue;
				}
				listClinicPrefs.Add(GetClinicPrefToSave(clinicNum,PrefName.WebSchedNewPatAllowChildren,allowChildren));
				listClinicPrefs.Add(GetClinicPrefToSave(clinicNum,PrefName.WebSchedNewPatVerifyInfo,verifyInfo));
				listClinicPrefs.Add(GetClinicPrefToSave(clinicNum,PrefName.WebSchedNewPatDoAuthEmail,doAuthEmail));
				listClinicPrefs.Add(GetClinicPrefToSave(clinicNum,PrefName.WebSchedNewPatDoAuthText,doAuthText));
				listClinicPrefs.Add(GetClinicPrefToSave(clinicNum,PrefName.WebSchedNewPatWebFormsURL,webFormsURL));
			}
			if(ClinicPrefs.Sync(listClinicPrefs,_listClinicPrefsWebSchedNewPats)) {
				DataValid.SetInvalid(InvalidType.ClinicPrefs);
			}
			if(comboWSNPConfirmStatuses.SelectedIndex!=-1) {
				Def newConfirmStatus=Defs.GetDefByExactName(DefCat.ApptConfirmed,comboWSNPConfirmStatuses.SelectedItem.ToString());
				Prefs.UpdateLong(PrefName.WebSchedNewPatConfirmStatus,newConfirmStatus.DefNum);
			}
			Prefs.UpdateBool(PrefName.WebSchedNewPatAllowProvSelection,checkNewPatAllowProvSelection.Checked);
		}

		private ClinicPref GetClinicPrefToSave(long clinicNum, PrefName prefName, string value) {
			ClinicPref clinicPref=_listClinicPrefsWebSchedNewPats.FirstOrDefault(x => x.ClinicNum==clinicNum && x.PrefName==prefName)?.Clone();
			if(clinicPref==null) {
				return new ClinicPref(clinicNum,prefName,value);
			}
			clinicPref.ValueString=value;
			return clinicPref;
		}

		private void AuthorizeTabWebSchedNewPat(bool allowEdit) {
			butWebSchedNewPatBlockouts.Enabled=allowEdit;
			textWebSchedNewPatApptSearchDays.Enabled=allowEdit;
		}

		private void textWebSchedNewPatApptSearchDays_Leave(object sender,EventArgs e) {
			//Only refresh if the value of this preference changed.  _indexLastNewPatURL will be set to -1 if a refresh is needed.
			if(_indexLastNewPatURL==-1) {
				FillGridWebSchedNewPatApptTimeSlotsThreaded();
			}
		}

		private void comboWSNPClinics_SelectionChangeCommitted(object sender,EventArgs e) {
			if(((ComboBox)sender).SelectedIndex!=_indexLastNewPatURL) {
				FillGridWebSchedNewPatApptTimeSlotsThreaded();
			}
		}

		private void FillGridWSNPAReasons() {
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.WebSchedNewPatApptTypes,true);
			List<DefLink> listDefLinks=DefLinks.GetDefLinksByType(DefLinkType.AppointmentType);
			List<AppointmentType> listApptTypes=AppointmentTypes.GetWhere(x => listDefLinks.Any(y => y.FKey==x.AppointmentTypeNum),true);
			//The combo box within the available times group box should always reflect the grid.
			comboWSNPADefApptType.Items.Clear();
			gridWSNPAReasons.BeginUpdate();
			gridWSNPAReasons.Columns.Clear();
			gridWSNPAReasons.Columns.Add(new ODGridColumn(Lan.g(this,"Reason"),120));
			gridWSNPAReasons.Columns.Add(new ODGridColumn(Lan.g(this,"Pattern"),0));
			gridWSNPAReasons.Rows.Clear();
			ODGridRow row;
			foreach(Def def in listDefs) {
				AppointmentType apptType=null;
				DefLink defLink=listDefLinks.FirstOrDefault(x => x.DefNum==def.DefNum);
				if(defLink==null) {
					continue;//Corruption?
				}
				apptType=listApptTypes.FirstOrDefault(x => x.AppointmentTypeNum==defLink.FKey);
				if(apptType==null) {
					continue;//Corruption?
				}
				row=new ODGridRow();
				row.Cells.Add(def.ItemName);
				row.Cells.Add((string.IsNullOrEmpty(apptType.Pattern) ? Lan.g(this,"(use procedure time pattern)") : apptType.Pattern));
				gridWSNPAReasons.Rows.Add(row);
				comboWSNPADefApptType.Items.Add(new ODBoxItem<Def>(def.ItemName,def));
			}
			gridWSNPAReasons.EndUpdate();
			comboWSNPADefApptType.SelectedIndex=0;//Always select Default.
		}

		private void FillGridWebSchedNewPatApptHostedURLs() {
			List<Clinic> clinicsAll=Clinics.GetDeepCopy();
			var eServiceData=WebServiceMainHQProxy.GetSignups<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService>(_signupOut,eServiceCode.WebSchedNewPatAppt)
				.Select(x => new {
					Signup=x,
					ClinicName=x.ClinicNum==0 ? Lan.g(this,"Headquarters") : (clinicsAll.FirstOrDefault(y => y.ClinicNum==x.ClinicNum)??new Clinic() { Abbr="N\\A" }).Abbr
				})
				.Where(x => 
					//Always show HQ
					x.Signup.ClinicNum==0 || 
					//If not HQ then only show if not hidden.
					clinicsAll.Any(y => y.ClinicNum==x.Signup.ClinicNum && !y.IsHidden)
				)
				//HQ to the top.
				.OrderBy(x => x.Signup.ClinicNum!=0)
				//Everything else is alpha sorted.
				.ThenBy(x => x.ClinicName);				
			_listClinicPrefsWebSchedNewPats.Clear();
			foreach(var clinic in eServiceData) {
				ContrNewPatHostedURL contr=new ContrNewPatHostedURL(clinic.Signup);
				Lan.C(this,contr);
				panelHostedURLs.Controls.Add(contr);
				comboWSNPClinics.Items.Add(new ODBoxItem<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService>(clinic.ClinicName,clinic.Signup));
				if(clinic.Signup.ClinicNum==0) {
					continue;
				}
				else {
					AddClinicPrefToList(PrefName.WebSchedNewPatAllowChildren,clinic.Signup.ClinicNum);
					AddClinicPrefToList(PrefName.WebSchedNewPatVerifyInfo,clinic.Signup.ClinicNum);
					AddClinicPrefToList(PrefName.WebSchedNewPatDoAuthEmail,clinic.Signup.ClinicNum);
					AddClinicPrefToList(PrefName.WebSchedNewPatDoAuthText,clinic.Signup.ClinicNum);
					AddClinicPrefToList(PrefName.WebSchedNewPatWebFormsURL,clinic.Signup.ClinicNum);
				}
			}
		}

		private void AddClinicPrefToList(PrefName prefName,long clinicNum) {
			ClinicPref clinicPref=ClinicPrefs.GetPref(prefName,clinicNum);
			if(clinicPref!=null) { 
				_listClinicPrefsWebSchedNewPats.Add(clinicPref);
			}
		}

		private void NavigateToURL(string URL) {
			try {
				Process.Start(URL);
			}
			catch(Exception) {
				MsgBox.Show(this,"There was a problem launching the URL with a web browser.  Make sure a default browser has been set.");
			}
		}

		private void FillGridWebSchedNewPatApptOps() {
			int opNameWidth=150;
			int clinicWidth=150;
			if(!PrefC.HasClinicsEnabled) {
				opNameWidth+=clinicWidth;
			}
			gridWebSchedNewPatApptOps.BeginUpdate();
			gridWebSchedNewPatApptOps.Columns.Clear();
			gridWebSchedNewPatApptOps.Columns.Add(new ODGridColumn(Lan.g("FormEServicesSetup","Op Name"),opNameWidth));
			gridWebSchedNewPatApptOps.Columns.Add(new ODGridColumn(Lan.g("FormEServicesSetup","Abbrev"),60));
			if(PrefC.HasClinicsEnabled) {
				gridWebSchedNewPatApptOps.Columns.Add(new ODGridColumn(Lan.g("FormEServicesSetup","Clinic"),clinicWidth));
			}
			gridWebSchedNewPatApptOps.Columns.Add(new ODGridColumn(Lan.g("FormEServicesSetup","Provider"),60));
			gridWebSchedNewPatApptOps.Columns.Add(new ODGridColumn(Lan.g("FormEServicesSetup","Hygienist"),60));
			gridWebSchedNewPatApptOps.Columns.Add(new ODGridColumn(Lan.g("FormEServicesSetup","ApptTypes"),0));
			gridWebSchedNewPatApptOps.Rows.Clear();
			//A list of all operatories that have IsWebSched set to true.
			List<Operatory> listWSNPAOps=Operatories.GetOpsForWebSchedNewPatAppts();
			List<DefLink> listWSNPAOperatoryDefLinks=DefLinks.GetDefLinksForWebSchedNewPatApptOperatories();
			List<DefLink> listWSNPAApptTypeDefLinks=DefLinks.GetDefLinksForWebSchedNewPatApptApptTypes();
			List<Def> listWSNPAApptTypeDefs=Defs.GetDefs(DefCat.WebSchedNewPatApptTypes,listWSNPAApptTypeDefLinks.Select(x => x.DefNum).ToList());
			ODGridRow row;
			foreach(Operatory op in listWSNPAOps) {
				row=new ODGridRow();
				row.Cells.Add(op.OpName);
				row.Cells.Add(op.Abbrev);
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(Clinics.GetAbbr(op.ClinicNum));
				}
				row.Cells.Add(Providers.GetAbbr(op.ProvDentist));
				row.Cells.Add(Providers.GetAbbr(op.ProvHygienist));
				//Figure out the DefNum that is associated to this New Pat Appt operatory
				long defNum=listWSNPAOperatoryDefLinks.First(x => x.FKey==op.OperatoryNum).DefNum;
				//Get all appointment type def links associated to the corresponding DefNum
				List<DefLink> listDefLinkApptTypes=listWSNPAApptTypeDefLinks.FindAll(x => x.DefNum==defNum);
				//Display the name of all "appointment types" (definition.ItemName) that are associated with the appt type defs that were just found.
				row.Cells.Add(string.Join(", ",listWSNPAApptTypeDefs.Where(x => listDefLinkApptTypes.Any(y => y.DefNum==x.DefNum)).Select(x => x.ItemName)));
				row.Tag=op;
				gridWebSchedNewPatApptOps.Rows.Add(row);
			}
			gridWebSchedNewPatApptOps.EndUpdate();
		}

		private void FillGridWebSchedNewPatApptTimeSlotsThreaded() {
			if(this.InvokeRequired) {
				this.BeginInvoke((Action)delegate () {
					FillGridWebSchedNewPatApptTimeSlotsThreaded();
				});
				return;
			}
			//Clear the current grid rows before starting the thread below. This allows that thread to exit at any time without leaving old rows in the grid.
			gridWebSchedNewPatApptTimeSlots.BeginUpdate();
			gridWebSchedNewPatApptTimeSlots.Rows.Clear();
			gridWebSchedNewPatApptTimeSlots.EndUpdate();
			//Validate time slot settings.
			if(textWebSchedNewPatApptsDateStart.errorProvider1.GetError(textWebSchedNewPatApptsDateStart)!="") {
				//Don't bother warning the user.  It will just be annoying.  The red indicator should be sufficient.
				return;
			}
			if(comboWSNPClinics.SelectedIndex<0) {
				return;//Nothing to do.
			}
			WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService signup=((ODBoxItem<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService>)comboWSNPClinics.SelectedItem).Tag;
			//Protect against re-entry
			if(_threadFillGridWebSchedNewPatApptTimeSlots!=null) {
				//A thread is already refreshing the time slots grid so we simply need to queue up another refresh once the one thread has finished.
				_isWebSchedNewPatApptTimeSlotsOutdated=true;
				return;
			}
			_isWebSchedNewPatApptTimeSlotsOutdated=false;
			_indexLastNewPatURL=comboWSNPClinics.SelectedIndex;
			DateTime dateStart=PIn.DateT(textWebSchedNewPatApptsDateStart.Text);
			DateTime dateEnd=dateStart.AddDays(30);
			if(!signup.IsEnabled) {
				return;//Do nothing, this clinic is excluded from New Pat Appts.
			}
			//Only get time slots for headquarters or clinics that are NOT excluded (aka included).
			var args=new {
				ClinicNum=signup.ClinicNum,
				DateStart=dateStart,
				DateEnd=dateStart.AddDays(30),
				DefApptType=comboWSNPADefApptType.SelectedTag<Def>(),
			};
			_threadFillGridWebSchedNewPatApptTimeSlots=new ODThread(new ODThread.WorkerDelegate((th) => {
				//The user might not have Web Sched ops set up correctly.  Don't warn them here because it is just annoying.  They'll figure it out.
				ODException.SwallowAnyException(() => {
					//Get the next 30 days of open time schedules with the current settings
					List<TimeSlot> listTimeSlots=TimeSlots.GetAvailableNewPatApptTimeSlots(args.DateStart,args.DateEnd,args.ClinicNum
						,args.DefApptType.DefNum);
					FillGridWebSchedNewPatApptTimeSlots(listTimeSlots);
				});
			})) { Name="ThreadWebSchedNewPatApptTimeSlots" };
			_threadFillGridWebSchedNewPatApptTimeSlots.AddExitHandler(new ODThread.WorkerDelegate((th) => {
				_threadFillGridWebSchedNewPatApptTimeSlots=null;
				//If something else wanted to refresh the grid while we were busy filling it then we need to refresh again.  A filter could have changed.
				if(_isWebSchedNewPatApptTimeSlotsOutdated) {
					FillGridWebSchedNewPatApptTimeSlotsThreaded();
				}
			}));
			_threadFillGridWebSchedNewPatApptTimeSlots.Start(true);
		}
		
		private void FillGridWebSchedNewPatApptTimeSlots(List<TimeSlot> listTimeSlots) {
			if(this.InvokeRequired) {
				this.Invoke((Action)delegate () { FillGridWebSchedNewPatApptTimeSlots(listTimeSlots); });
				return;
			}
			gridWebSchedNewPatApptTimeSlots.BeginUpdate();
			gridWebSchedNewPatApptTimeSlots.Columns.Clear();
			ODGridColumn col=new ODGridColumn("",0);
			col.TextAlign=HorizontalAlignment.Center;
			gridWebSchedNewPatApptTimeSlots.Columns.Add(col);
			gridWebSchedNewPatApptTimeSlots.Rows.Clear();
			ODGridRow row;
			DateTime dateTimeSlotLast=DateTime.MinValue;
			foreach(TimeSlot timeSlot in listTimeSlots) {
				//Make a new row for every unique day.
				if(dateTimeSlotLast.Date!=timeSlot.DateTimeStart.Date) {
					dateTimeSlotLast=timeSlot.DateTimeStart;
					row=new ODGridRow();
					row.ColorBackG=Color.LightBlue;
					row.Cells.Add(timeSlot.DateTimeStart.ToShortDateString());
					gridWebSchedNewPatApptTimeSlots.Rows.Add(row);
				}
				row=new ODGridRow();
				row.Cells.Add(timeSlot.DateTimeStart.ToShortTimeString()+" - "+timeSlot.DateTimeStop.ToShortTimeString());
				gridWebSchedNewPatApptTimeSlots.Rows.Add(row);
			}
			gridWebSchedNewPatApptTimeSlots.EndUpdate();
		}

		private void comboWebSchedNewPatApptsApptType_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGridWebSchedNewPatApptTimeSlotsThreaded();
		}

		private void gridWebSchedNewPatApptOps_DoubleClick(object sender,EventArgs e) {
			ShowOperatoryEditAndRefreshGrids();
		}

		private void gridWSNPAReasons_DoubleClick(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormDefinitions FormD=new FormDefinitions(DefCat.WebSchedNewPatApptTypes);
			FormD.ShowDialog();
			FillGridWSNPAReasons();
		}

		private void butWebSchedNewPatApptsToday_Click(object sender,EventArgs e) {
			textWebSchedNewPatApptsDateStart.Text=DateTime.Today.ToShortDateString();
		}

		private void textWebSchedNewPatApptSearchDays_Validated(object sender,EventArgs e) {
			if(textWebSchedNewPatApptSearchDays.errorProvider1.GetError(textWebSchedNewPatApptSearchDays)!="") {
				return;
			}
			int newPatApptDays=PIn.Int(textWebSchedNewPatApptSearchDays.Text);
			if(Prefs.UpdateInt(PrefName.WebSchedNewPatApptSearchAfterDays,newPatApptDays>0 ? newPatApptDays : 0)) {
				_indexLastNewPatURL=-1;//Force refresh of the grid in because this setting changed.
			}
		}

		private void textWebSchedNewPatApptsDateStart_TextChanged(object sender,EventArgs e) {
			//Only refresh the grid if the user has typed in a valid date.
			if(textWebSchedNewPatApptsDateStart.errorProvider1.GetError(textWebSchedNewPatApptsDateStart)=="") {
				FillGridWebSchedNewPatApptTimeSlotsThreaded();
			}
		}

		private void checkWebSchedNewPatForcePhoneFormatting_Click(object sender,EventArgs e) {
			Prefs.UpdateBool(PrefName.WebSchedNewPatApptForcePhoneFormatting,checkWebSchedNewPatForcePhoneFormatting.Checked);
		}

		private void butWebSchedNewPatBlockouts_Click(object sender,EventArgs e) {
			string[] arrayDefNums=PrefC.GetString(PrefName.WebSchedNewPatApptIgnoreBlockoutTypes).Split(new char[] {','}); //comma-delimited list.
			List<long> listBlockoutTypes=new List<long>();
			foreach(string strDefNum in arrayDefNums) {
				listBlockoutTypes.Add(PIn.Long(strDefNum));
			}
			List<Def> listBlockoutTypeDefs=Defs.GetDefs(DefCat.BlockoutTypes,listBlockoutTypes);
			FormDefinitionPicker FormDP=new FormDefinitionPicker(DefCat.BlockoutTypes,listBlockoutTypeDefs);
			FormDP.HasShowHiddenOption=true;
			FormDP.IsMultiSelectionMode=true;
			FormDP.ShowDialog();
			if(FormDP.DialogResult==DialogResult.OK) {
				listboxWebSchedNewPatIgnoreBlockoutTypes.Items.Clear();
				foreach(Def defCur in FormDP.ListSelectedDefs) {
					listboxWebSchedNewPatIgnoreBlockoutTypes.Items.Add(defCur.ItemName);
				}
				string strListWebSchedNewPatIgnoreBlockoutTypes=String.Join(",",FormDP.ListSelectedDefs.Select(x => x.DefNum));
				Prefs.UpdateString(PrefName.WebSchedNewPatApptIgnoreBlockoutTypes,strListWebSchedNewPatIgnoreBlockoutTypes);
			}
		}
	}
}
