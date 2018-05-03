using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using CodeBase;

namespace OpenDentBusiness {
	public class Cache {
		/// <summary>This is only used in the RefreshCache methods.  Used instead of Meth.  The command is only used if not ClientWeb.</summary>
		public static DataTable GetTableRemotelyIfNeeded(MethodBase methodBase,string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(methodBase);
			}
			else {
				return Db.GetTable(command);
			}
		}

		public static void Refresh(params InvalidType[] arrayITypes) {
			Refresh(true,arrayITypes);
		}

		///<summary>itypesStr= comma-delimited list of int.  Called directly from UI in one spot.  Called from above repeatedly.  The end result is that both server and client have been properly refreshed.</summary>
		public static void Refresh(bool doRefreshServerCache,params InvalidType[] arrayITypes) {
			DataSet ds=GetCacheDs(doRefreshServerCache,arrayITypes);
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				//Because otherwise it was handled just fine as part of the GetCacheDs
				FillCache(ds,arrayITypes);
			}
		}

		///<summary>Necessary for backwards compatibility when workstations version 17.1 or earlier connect to a 17.2 server.</summary>
		public static DataSet GetCacheDs(params InvalidType[] arrayITypes) {
			return GetCacheDs(false,arrayITypes);
		}

		///<summary>If ClientWeb, then this method is instead run on the server, and the result passed back to the client.  And since it's ClientWeb, FillCache will be run on the client.</summary>
		public static DataSet GetCacheDs(bool doRefreshServerCache,params InvalidType[] arrayITypes) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDS(MethodBase.GetCurrentMethod(),doRefreshServerCache,arrayITypes);
			}
			Logger.LogToPath("",LogPath.Signals,LogPhase.Start,"InvalidType(s): "+string.Join(" - ",arrayITypes.OrderBy(x => x.ToString())));
			List<InvalidType> listITypes=arrayITypes.ToList();
			//so this part below only happens if direct or server------------------------------------------------
			bool isAll=false;
			if(listITypes.Contains(InvalidType.AllLocal)) {
				isAll=true;
			}
			DataSet ds=new DataSet();
			//All Internal OD Tables that are cached go here
			if(PrefC.IsODHQ) {
				if(listITypes.Contains(InvalidType.JobPermission) || isAll) {
					ds.Tables.Add(JobPermissions.RefreshCache());
				}
				if(listITypes.Contains(InvalidType.PhoneComps) || isAll) {
					ds.Tables.Add(PhoneComps.GetTableFromCache(doRefreshServerCache));
				}
			}
			//All cached public tables go here
			if(listITypes.Contains(InvalidType.AccountingAutoPays) || isAll) {
				ds.Tables.Add(AccountingAutoPays.GetTableFromCache(doRefreshServerCache));
			}
			//if(listITypes.Contains(InvalidType.AlertItems) || isAll) {//THIS IS NOT CACHED. But is used to make server run the alert logic in OpenDentalService.
			//	ds.Tables.Add(AlertItems.RefreshCache());
			//}
			if(listITypes.Contains(InvalidType.AlertCategories) || isAll) {
				ds.Tables.Add(AlertCategories.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.AlertCategoryLinks) || isAll) {
				ds.Tables.Add(AlertCategoryLinks.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.AppointmentTypes) || isAll) {
				ds.Tables.Add(AppointmentTypes.GetTableFromCache(doRefreshServerCache));				
			}
			if(listITypes.Contains(InvalidType.AutoCodes) || isAll) {
				ds.Tables.Add(AutoCodes.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(AutoCodeItems.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(AutoCodeConds.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Automation) || isAll) {
				ds.Tables.Add(Automations.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.AutoNotes) || isAll) {
				ds.Tables.Add(AutoNotes.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(AutoNoteControls.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Carriers) || isAll) {
				ds.Tables.Add(Carriers.GetTableFromCache(doRefreshServerCache));//run on startup, after telephone reformat, after list edit.
			}
			if(listITypes.Contains(InvalidType.ClaimForms) || isAll) {
				ds.Tables.Add(ClaimFormItems.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(ClaimForms.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.ClearHouses) || isAll) {
				ds.Tables.Add(Clearinghouses.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.ClinicErxs) || isAll) {
				ds.Tables.Add(ClinicErxs.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.ClinicPrefs) || isAll) {
				ds.Tables.Add(ClinicPrefs.GetTableFromCache(doRefreshServerCache));
			}
			//InvalidType.Clinics see InvalidType.Providers
			if(listITypes.Contains(InvalidType.Computers) || isAll) {
				ds.Tables.Add(Computers.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(Printers.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Defs) || isAll) {
				ds.Tables.Add(Defs.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.DentalSchools) || isAll) {
				ds.Tables.Add(SchoolClasses.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(SchoolCourses.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.DictCustoms) || isAll) {
				ds.Tables.Add(DictCustoms.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Diseases) || isAll) {
				ds.Tables.Add(DiseaseDefs.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(ICD9s.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.DisplayFields) || isAll) {
				ds.Tables.Add(DisplayFields.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.DisplayReports) || isAll) {
				ds.Tables.Add(DisplayReports.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Ebills) || isAll) {
				ds.Tables.Add(Ebills.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.EhrCodes)) {
				EhrCodes.UpdateList();//Unusual pattern for an unusual "table".  Not really a table, but a mishmash of hard coded partial code systems that are needed for CQMs.
			}
			if(listITypes.Contains(InvalidType.ElectIDs) || isAll) {
				ds.Tables.Add(ElectIDs.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Email) || isAll) {
				ds.Tables.Add(EmailAddresses.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(EmailTemplates.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(EmailAutographs.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Employees) || isAll) {
				ds.Tables.Add(Employees.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(PayPeriods.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Employers) || isAll) {
				ds.Tables.Add(Employers.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Fees) || isAll) {
				//Fee Cache follows an unusual pattern. This fills the cache with the HQ fees, and whatever clinics happen to be currently cached.
				ds.Tables.Add(Fees.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.FeeScheds) || isAll) {
				ds.Tables.Add(FeeScheds.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.HL7Defs) || isAll) {
				ds.Tables.Add(HL7Defs.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(HL7DefMessages.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(HL7DefSegments.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(HL7DefFields.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.InsCats) || isAll) {
				ds.Tables.Add(CovCats.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(CovSpans.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.InsFilingCodes) || isAll) {
				ds.Tables.Add(InsFilingCodes.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(InsFilingCodeSubtypes.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Languages) || isAll) {
				if(CultureInfo.CurrentCulture.Name!="en-US") {
					ds.Tables.Add(Lans.GetTableFromCache(doRefreshServerCache));
				}
			}
			if(listITypes.Contains(InvalidType.Letters) || isAll) {
				ds.Tables.Add(Letters.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.LetterMerge) || isAll) {
				ds.Tables.Add(LetterMergeFields.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(LetterMerges.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Medications) || isAll) {
				ds.Tables.Add(Medications.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Operatories) || isAll) {
				ds.Tables.Add(Operatories.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.OrthoChartTabs) || isAll) {
				ds.Tables.Add(OrthoChartTabs.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(OrthoChartTabLinks.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.PatFields) || isAll) {
				ds.Tables.Add(PatFieldDefs.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(ApptFieldDefs.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Pharmacies) || isAll) {
				ds.Tables.Add(Pharmacies.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Prefs) || isAll) {
				ds.Tables.Add(Prefs.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.ProcButtons) || isAll) {
				ds.Tables.Add(ProcButtons.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(ProcButtonItems.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.ProcCodes) || isAll) {
				ds.Tables.Add(ProcedureCodes.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(ProcCodeNotes.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Programs) || isAll) {
				ds.Tables.Add(Programs.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(ProgramProperties.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.ProviderErxs) || isAll) {
				ds.Tables.Add(ProviderErxs.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.ProviderIdents) || isAll) {
				ds.Tables.Add(ProviderIdents.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Providers) || isAll) {
				ds.Tables.Add(Providers.GetTableFromCache(doRefreshServerCache));
				//Refresh the clinics as well because InvalidType.Providers has a comment that says "also includes clinics".  Also, there currently isn't an itype for Clinics.
				ds.Tables.Add(Clinics.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.QuickPaste) || isAll) {
				ds.Tables.Add(QuickPasteNotes.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(QuickPasteCats.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.RecallTypes) || isAll) {
				ds.Tables.Add(RecallTypes.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(RecallTriggers.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.ReplicationServers) || isAll) {
				ds.Tables.Add(ReplicationServers.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.RequiredFields) || isAll) {
				ds.Tables.Add(RequiredFields.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(RequiredFieldConditions.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Security) || isAll) {
				ds.Tables.Add(Userods.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(UserGroups.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(GroupPermissions.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(UserGroupAttaches.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Sheets) || isAll) {
				ds.Tables.Add(SheetDefs.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(SheetFieldDefs.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.SigMessages) || isAll) {
				ds.Tables.Add(SigElementDefs.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(SigButDefs.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Sites) || isAll) {
				ds.Tables.Add(Sites.GetTableFromCache(doRefreshServerCache));
				if(PrefC.IsODHQ) {
					ds.Tables.Add(SiteLinks.GetTableFromCache(doRefreshServerCache));
				}
			}
			if(listITypes.Contains(InvalidType.SmsBlockPhones) || isAll) {
				ds.Tables.Add(SmsBlockPhones.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Sops) || isAll) {  //InvalidType.Sops is currently never used 11/14/2014
				ds.Tables.Add(Sops.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.StateAbbrs) || isAll) {
				ds.Tables.Add(StateAbbrs.GetTableFromCache(doRefreshServerCache));
			}
			//InvalidTypes.Tasks not handled here.
			if(listITypes.Contains(InvalidType.TimeCardRules) || isAll) {
				ds.Tables.Add(TimeCardRules.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.ToolBut) || isAll) {
				ds.Tables.Add(ToolButItems.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.UserClinics) || isAll) {
				ds.Tables.Add(UserClinics.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.UserQueries) || isAll) {
				ds.Tables.Add(UserQueries.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Vaccines) || isAll) {
				ds.Tables.Add(VaccineDefs.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(DrugManufacturers.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(DrugUnits.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Views) || isAll) {
				ds.Tables.Add(ApptViews.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(ApptViewItems.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(AppointmentRules.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(ProcApptColors.GetTableFromCache(doRefreshServerCache));
			}
			if(listITypes.Contains(InvalidType.Wiki) || isAll) {
				ds.Tables.Add(WikiListHeaderWidths.GetTableFromCache(doRefreshServerCache));
				ds.Tables.Add(WikiPages.RefreshCache());
			}
			if(listITypes.Contains(InvalidType.ZipCodes) || isAll) {
				ds.Tables.Add(ZipCodes.GetTableFromCache(doRefreshServerCache));
			}
			Logger.LogToPath("",LogPath.Signals,LogPhase.End);
			return ds;
		}

		///<summary>only if ClientWeb</summary>
		public static void FillCache(DataSet ds,params InvalidType[] arrayITypes) {
			List<InvalidType> listITypes=arrayITypes.ToList();
			bool isAll=false;
			if(listITypes.Contains(InvalidType.AllLocal)) {
				isAll=true;
			}
			//All Internal OD Tables that are cached go here
			if(PrefC.IsODHQ) {
				if(listITypes.Contains(InvalidType.JobPermission) || isAll) {
					ds.Tables.Add(JobPermissions.RefreshCache());
				}
				if(listITypes.Contains(InvalidType.PhoneComps) || isAll) {
					PhoneComps.FillCacheFromTable(ds.Tables["PhoneComp"]);
				}
			}
			if(listITypes.Contains(InvalidType.AccountingAutoPays) || isAll) {
				AccountingAutoPays.FillCacheFromTable(ds.Tables["AccountingAutoPay"]);
			}
			//if(listITypes.Contains(InvalidType.AlertItems) || isAll) {//THIS IS NOT CACHED. But is used to make server run the alert logic in OpenDentalService.
			//	AlertSubs.FillCache(ds.Tables["AlertItem"]);
			//}
			if(listITypes.Contains(InvalidType.AlertCategories) || isAll) {
				AlertCategories.FillCacheFromTable(ds.Tables["AlertCategory"]);
			}
			if(listITypes.Contains(InvalidType.AlertCategoryLinks) || isAll) {
				AlertCategoryLinks.FillCacheFromTable(ds.Tables["AlertCategoryLink"]);
			}
			if(listITypes.Contains(InvalidType.AppointmentTypes) || isAll) {
				AppointmentTypes.FillCacheFromTable(ds.Tables["AppointmentType"]);
			}
			if(listITypes.Contains(InvalidType.AutoCodes) || isAll) {
				AutoCodes.FillCacheFromTable(ds.Tables["AutoCode"]);
				AutoCodeItems.FillCacheFromTable(ds.Tables["AutoCodeItem"]);
				AutoCodeConds.FillCacheFromTable(ds.Tables["AutoCodeCond"]);
			}
			if(listITypes.Contains(InvalidType.Automation) || isAll) {
				Automations.FillCacheFromTable(ds.Tables["Automation"]);
			}
			if(listITypes.Contains(InvalidType.AutoNotes) || isAll) {
				AutoNotes.FillCacheFromTable(ds.Tables["AutoNote"]);
				AutoNoteControls.FillCacheFromTable(ds.Tables["AutoNoteControl"]);
			}
			if(listITypes.Contains(InvalidType.Carriers) || isAll) {
				Carriers.FillCacheFromTable(ds.Tables["Carrier"]);//run on startup, after telephone reformat, after list edit.
			}
			if(listITypes.Contains(InvalidType.ClaimForms) || isAll) {
				ClaimFormItems.FillCacheFromTable(ds.Tables["ClaimFormItem"]);
				ClaimForms.FillCacheFromTable(ds.Tables["ClaimForm"]);
			}
			if(listITypes.Contains(InvalidType.ClearHouses) || isAll) {
				Clearinghouses.FillCacheFromTable(ds.Tables["Clearinghouse"]);
			}
			if(listITypes.Contains(InvalidType.ClinicErxs) || isAll) {
				ClinicErxs.FillCacheFromTable(ds.Tables["ClinicErx"]);
			}
			if(listITypes.Contains(InvalidType.ClinicPrefs)||isAll) {
				ClinicPrefs.FillCacheFromTable(ds.Tables["ClinicPref"]);
			}
			if(listITypes.Contains(InvalidType.Computers) || isAll) {
				Computers.FillCacheFromTable(ds.Tables["Computer"]);
				Printers.FillCacheFromTable(ds.Tables["Printer"]);
			}
			if(listITypes.Contains(InvalidType.Defs) || isAll) {
				Defs.FillCacheFromTable(ds.Tables["Def"]);
			}
			if(listITypes.Contains(InvalidType.DentalSchools) || isAll) {
				SchoolClasses.FillCacheFromTable(ds.Tables["SchoolClass"]);
				SchoolCourses.FillCacheFromTable(ds.Tables["SchoolCourse"]);
			}
			if(listITypes.Contains(InvalidType.DictCustoms) || isAll) {
				DictCustoms.FillCacheFromTable(ds.Tables["DictCustom"]);
			}
			if(listITypes.Contains(InvalidType.Diseases) || isAll) {
				DiseaseDefs.FillCacheFromTable(ds.Tables["DiseaseDef"]);
				ICD9s.FillCacheFromTable(ds.Tables["ICD9"]);
			}
			if(listITypes.Contains(InvalidType.DisplayFields) || isAll) {
				DisplayFields.FillCacheFromTable(ds.Tables["DisplayField"]);
			}
			if(listITypes.Contains(InvalidType.DisplayReports) || isAll) {
				DisplayReports.FillCacheFromTable(ds.Tables["DisplayReport"]);
			}
			if(listITypes.Contains(InvalidType.Ebills) || isAll) {
				Ebills.FillCacheFromTable(ds.Tables["Ebill"]);
			}
			if(listITypes.Contains(InvalidType.ElectIDs) || isAll) {
				ElectIDs.FillCacheFromTable(ds.Tables["ElectID"]);
			}
			if(listITypes.Contains(InvalidType.Email) || isAll) {
				EmailAddresses.FillCacheFromTable(ds.Tables["EmailAddress"]);
				EmailTemplates.FillCacheFromTable(ds.Tables["EmailTemplate"]);
			}
			if(listITypes.Contains(InvalidType.Employees) || isAll) {
				Employees.FillCacheFromTable(ds.Tables["Employee"]);
				PayPeriods.FillCacheFromTable(ds.Tables["PayPeriod"]);
			}
			if(listITypes.Contains(InvalidType.Employers) || isAll) {
				Employers.FillCacheFromTable(ds.Tables["Employer"]);
			}
			if(listITypes.Contains(InvalidType.Fees) || isAll) {
				Fees.FillCacheFromTable(ds.Tables["Fee"]);
			}
			if(listITypes.Contains(InvalidType.FeeScheds) || isAll) {
				FeeScheds.FillCacheFromTable(ds.Tables["FeeSched"]);
			}
			if(listITypes.Contains(InvalidType.HL7Defs) || isAll) {
				HL7Defs.FillCacheFromTable(ds.Tables["HL7Def"]);
				HL7DefMessages.FillCacheFromTable(ds.Tables["HL7DefMessage"]);
				HL7DefSegments.FillCacheFromTable(ds.Tables["HL7DefSegment"]);
				HL7DefFields.FillCacheFromTable(ds.Tables["HL7DefField"]);
			}
			if(listITypes.Contains(InvalidType.InsCats) || isAll) {
				CovCats.FillCacheFromTable(ds.Tables["CovCat"]);
				CovSpans.FillCacheFromTable(ds.Tables["CovSpan"]);
			}
			if(listITypes.Contains(InvalidType.InsFilingCodes) || isAll) {
				InsFilingCodes.FillCacheFromTable(ds.Tables["InsFilingCode"]);
				InsFilingCodeSubtypes.FillCacheFromTable(ds.Tables["InsFilingCodeSubtype"]);
			}
			if(listITypes.Contains(InvalidType.Languages) || isAll) {
				Lans.FillCacheFromTable(ds.Tables["Language"]);
			}
			if(listITypes.Contains(InvalidType.Letters) || isAll) {
				Letters.FillCacheFromTable(ds.Tables["Letter"]);
			}
			if(listITypes.Contains(InvalidType.LetterMerge) || isAll) {
				LetterMergeFields.FillCacheFromTable(ds.Tables["LetterMergeField"]);
				LetterMerges.FillCacheFromTable(ds.Tables["LetterMerge"]);
			}
			if(listITypes.Contains(InvalidType.Medications) || isAll) {
				Medications.FillCacheFromTable(ds.Tables["Medication"]);
			}
			if(listITypes.Contains(InvalidType.Operatories) || isAll) {
				Operatories.FillCacheFromTable(ds.Tables["Operatory"]);
			}
			if(listITypes.Contains(InvalidType.OrthoChartTabs) || isAll) {
				OrthoChartTabs.FillCacheFromTable(ds.Tables["OrthoChartTab"]);
				OrthoChartTabLinks.FillCacheFromTable(ds.Tables["OrthoChartTabLink"]);
			}
			if(listITypes.Contains(InvalidType.PatFields) || isAll) {
				PatFieldDefs.FillCacheFromTable(ds.Tables["PatFieldDef"]);
				ApptFieldDefs.FillCacheFromTable(ds.Tables["ApptFieldDef"]);
			}
			if(listITypes.Contains(InvalidType.Pharmacies) || isAll) {
				Pharmacies.FillCacheFromTable(ds.Tables["Pharmacy"]);
			}
			if(listITypes.Contains(InvalidType.Prefs) || isAll) {
				Prefs.FillCacheFromTable(ds.Tables["Pref"]);
			}
			if(listITypes.Contains(InvalidType.ProcButtons) || isAll) {
				ProcButtons.FillCacheFromTable(ds.Tables["ProcButton"]);
				ProcButtonItems.FillCacheFromTable(ds.Tables["ProcButtonItem"]);
			}
			if(listITypes.Contains(InvalidType.ProcCodes) || isAll) {
				ProcedureCodes.FillCacheFromTable(ds.Tables["ProcedureCode"]);
				ProcCodeNotes.FillCacheFromTable(ds.Tables["ProcCodeNote"]);
			}
			if(listITypes.Contains(InvalidType.Programs) || isAll) {
				Programs.FillCacheFromTable(ds.Tables["Program"]);
				ProgramProperties.FillCacheFromTable(ds.Tables["ProgramProperty"]);
			}
			if(listITypes.Contains(InvalidType.ProviderErxs) || isAll) {
				ProviderErxs.FillCacheFromTable(ds.Tables["ProviderErx"]);
			}
			if(listITypes.Contains(InvalidType.ProviderIdents) || isAll) {
				ProviderIdents.FillCacheFromTable(ds.Tables["ProviderIdent"]);
			}
			if(listITypes.Contains(InvalidType.Providers) || isAll) {
				Providers.FillCacheFromTable(ds.Tables["Provider"]);
				//Refresh the clinics as well because InvalidType.Providers has a comment that says "also includes clinics".  Also, there currently isn't an itype for Clinics.
				Clinics.FillCacheFromTable(ds.Tables["clinic"]);//Case must match the table name in Clinics.RefrechCache().
			}
			if(listITypes.Contains(InvalidType.QuickPaste) || isAll) {
				QuickPasteNotes.FillCacheFromTable(ds.Tables["QuickPasteNote"]);
				QuickPasteCats.FillCacheFromTable(ds.Tables["QuickPasteCat"]);
			}
			if(listITypes.Contains(InvalidType.RecallTypes) || isAll) {
				RecallTypes.FillCacheFromTable(ds.Tables["RecallType"]);
				RecallTriggers.FillCacheFromTable(ds.Tables["RecallTrigger"]);
			}
			if(listITypes.Contains(InvalidType.ReplicationServers) || isAll) {
				ReplicationServers.FillCacheFromTable(ds.Tables["ReplicationServer"]);
			}
			//if(itypes.Contains(InvalidType.RequiredFields) || isAll) {
			//	RequiredFields.FillCache(ds.Tables["RequiredField"]);
			//}
			if(listITypes.Contains(InvalidType.Security) || isAll) {
				Userods.FillCacheFromTable(ds.Tables["Userod"]);
				UserGroups.FillCacheFromTable(ds.Tables["UserGroup"]);
				UserGroupAttaches.FillCacheFromTable(ds.Tables["UserGroupAttach"]);
			}
			if(listITypes.Contains(InvalidType.Sheets) || isAll) {
				SheetDefs.FillCacheFromTable(ds.Tables["SheetDef"]);
				SheetFieldDefs.FillCacheFromTable(ds.Tables["SheetFieldDef"]);
			}
			if(listITypes.Contains(InvalidType.SigMessages) || isAll) {
				SigElementDefs.FillCacheFromTable(ds.Tables["SigElementDef"]);
				SigButDefs.FillCacheFromTable(ds.Tables["SigButDef"]);
			}
			if(listITypes.Contains(InvalidType.Sites) || isAll) {
				Sites.FillCacheFromTable(ds.Tables["Site"]);
				if(PrefC.IsODHQ) {
					SiteLinks.FillCacheFromTable(ds.Tables["SiteLink"]);
				}
			}
			if(listITypes.Contains(InvalidType.SmsBlockPhones) || isAll) {
				SmsBlockPhones.FillCacheFromTable(ds.Tables["SmsBlockPhone"]);
			}
			if(listITypes.Contains(InvalidType.Sops) || isAll) {
				Sops.FillCacheFromTable(ds.Tables["Sop"]);
			}
			if(listITypes.Contains(InvalidType.StateAbbrs) || isAll) {
				StateAbbrs.FillCacheFromTable(ds.Tables["StateAbbr"]);
			}
			if(listITypes.Contains(InvalidType.TimeCardRules) || isAll) {
				TimeCardRules.FillCacheFromTable(ds.Tables["TimeCardRule"]);
			}
			//InvalidTypes.Tasks not handled here.
			if(listITypes.Contains(InvalidType.ToolBut) || isAll) {
				ToolButItems.FillCacheFromTable(ds.Tables["ToolButItem"]);
			}
			if(listITypes.Contains(InvalidType.UserClinics) || isAll) {
				UserClinics.FillCacheFromTable(ds.Tables["UserClinic"]);
			}
			if(listITypes.Contains(InvalidType.UserQueries) || isAll) {
				UserQueries.FillCacheFromTable(ds.Tables["UserQuery"]);
			}
			if(listITypes.Contains(InvalidType.Vaccines) || isAll) {
				VaccineDefs.FillCacheFromTable(ds.Tables["VaccineDef"]);
				DrugManufacturers.FillCacheFromTable(ds.Tables["DrugManufacturer"]);
				DrugUnits.FillCacheFromTable(ds.Tables["DrugUnit"]);
			}
			if(listITypes.Contains(InvalidType.Views) || isAll) {
				ApptViews.FillCacheFromTable(ds.Tables["ApptView"]);
				ApptViewItems.FillCacheFromTable(ds.Tables["ApptViewItem"]);
				AppointmentRules.FillCacheFromTable(ds.Tables["AppointmentRule"]);
				ProcApptColors.FillCacheFromTable(ds.Tables["ProcApptColor"]);
			}
			if(listITypes.Contains(InvalidType.Wiki) || isAll) {
				WikiListHeaderWidths.FillCacheFromTable(ds.Tables["WikiListHeaderWidth"]);
				WikiPages.FillCache(ds.Tables["WikiPage"]);
			}
			if(listITypes.Contains(InvalidType.ZipCodes) || isAll) {
				ZipCodes.FillCacheFromTable(ds.Tables["ZipCode"]);
			}
		}

		///<summary>Returns a list of all invalid types that are used for the cache.  Currently only called from DBM.</summary>
		public static List<InvalidType> GetAllCachedInvalidTypes() {
			List<InvalidType> listInvalidTypes=new List<InvalidType>();
			//Below is a list of all invalid types in the same order the appear in the InvalidType enum.  
			//Comment out any rows that are not used for cache table refreshes.  See Cache.GetCacheDs() for more info.
			//listInvalidTypes.Add(InvalidType.None);  //No need to send a signal
			//listInvalidTypes.Add(InvalidType.Date);  //Not used with any other flags, not cached
			//listInvalidTypes.Add(InvalidType.AllLocal);  //Deprecated
			//listInvalidTypes.Add(InvalidType.Task);  //Not used with any other flags, not cached
			listInvalidTypes.Add(InvalidType.ProcCodes);
			listInvalidTypes.Add(InvalidType.Prefs);
			listInvalidTypes.Add(InvalidType.Views);
			listInvalidTypes.Add(InvalidType.AutoCodes);
			listInvalidTypes.Add(InvalidType.Carriers);
			listInvalidTypes.Add(InvalidType.ClearHouses);
			listInvalidTypes.Add(InvalidType.Computers);
			listInvalidTypes.Add(InvalidType.InsCats);
			listInvalidTypes.Add(InvalidType.Employees);
			//listInvalidTypes.Add(InvalidType.StartupOld);  //Deprecated
			listInvalidTypes.Add(InvalidType.Defs);
			listInvalidTypes.Add(InvalidType.Email);
			listInvalidTypes.Add(InvalidType.Fees);
			listInvalidTypes.Add(InvalidType.Letters);
			listInvalidTypes.Add(InvalidType.QuickPaste);
			listInvalidTypes.Add(InvalidType.Security);
			listInvalidTypes.Add(InvalidType.Programs);
			listInvalidTypes.Add(InvalidType.ToolBut);
			listInvalidTypes.Add(InvalidType.Providers);
			listInvalidTypes.Add(InvalidType.ClaimForms);
			listInvalidTypes.Add(InvalidType.ZipCodes);
			listInvalidTypes.Add(InvalidType.LetterMerge);
			listInvalidTypes.Add(InvalidType.DentalSchools);
			listInvalidTypes.Add(InvalidType.Operatories);
			//listInvalidTypes.Add(InvalidType.TaskPopup);  //Not needed, not cached
			listInvalidTypes.Add(InvalidType.Sites);
			listInvalidTypes.Add(InvalidType.Pharmacies);
			listInvalidTypes.Add(InvalidType.Sheets);
			listInvalidTypes.Add(InvalidType.RecallTypes);
			listInvalidTypes.Add(InvalidType.FeeScheds);
			//listInvalidTypes.Add(InvalidType.PhoneNumbers);  //Internal only, not cached
			//listInvalidTypes.Add(InvalidType.Signals);  //Deprecated
			listInvalidTypes.Add(InvalidType.DisplayFields);
			listInvalidTypes.Add(InvalidType.PatFields);
			listInvalidTypes.Add(InvalidType.AccountingAutoPays);
			listInvalidTypes.Add(InvalidType.ProcButtons);
			listInvalidTypes.Add(InvalidType.Diseases);
			listInvalidTypes.Add(InvalidType.Languages);
			listInvalidTypes.Add(InvalidType.AutoNotes);
			listInvalidTypes.Add(InvalidType.ElectIDs);
			listInvalidTypes.Add(InvalidType.Employers);
			listInvalidTypes.Add(InvalidType.ProviderIdents);
			//listInvalidTypes.Add(InvalidType.ShutDownNow);  //Do not want to send shutdown signal
			listInvalidTypes.Add(InvalidType.InsFilingCodes);
			listInvalidTypes.Add(InvalidType.ReplicationServers);
			listInvalidTypes.Add(InvalidType.Automation);
			//listInvalidTypes.Add(InvalidType.PhoneAsteriskReload);  //Internal only, not cached
			listInvalidTypes.Add(InvalidType.TimeCardRules);
			listInvalidTypes.Add(InvalidType.Vaccines);
			listInvalidTypes.Add(InvalidType.HL7Defs);
			listInvalidTypes.Add(InvalidType.DictCustoms);
			listInvalidTypes.Add(InvalidType.Wiki);
			listInvalidTypes.Add(InvalidType.Sops);
			listInvalidTypes.Add(InvalidType.EhrCodes);
			listInvalidTypes.Add(InvalidType.AppointmentTypes);
			listInvalidTypes.Add(InvalidType.Medications);
			//listInvalidTypes.Add(InvalidType.SmsTextMsgReceivedUnreadCount);  //Special InvalidType that would break things if we sent, not cached
			listInvalidTypes.Add(InvalidType.ProviderErxs);
			//listInvalidTypes.Add(InvalidType.Jobs);  //Internal only, not needed
			//listInvalidTypes.Add(InvalidType.JobRoles);  //Internal only, not needed
			listInvalidTypes.Add(InvalidType.StateAbbrs);
			listInvalidTypes.Add(InvalidType.RequiredFields);
			listInvalidTypes.Add(InvalidType.Ebills);
			listInvalidTypes.Add(InvalidType.UserClinics);
			listInvalidTypes.Add(InvalidType.OrthoChartTabs);
			listInvalidTypes.Add(InvalidType.SigMessages);
			//listInvalidTypes.Add(InvalidType.AlertItems);//THIS IS NOT CACHED. But is used to make server run the alert logic in OpenDentalService.
			listInvalidTypes.Add(InvalidType.ClinicPrefs);
			listInvalidTypes.Add(InvalidType.SmsBlockPhones);
			listInvalidTypes.Add(InvalidType.ClinicErxs);
			listInvalidTypes.Add(InvalidType.DisplayReports);
			listInvalidTypes.Add(InvalidType.UserQueries);
			return listInvalidTypes;
		}

	}
}
