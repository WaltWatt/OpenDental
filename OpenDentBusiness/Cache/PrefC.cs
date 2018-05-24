using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using OpenDentBusiness;
using CodeBase;
using System.IO;
using System.Net;

namespace OpenDentBusiness {
	public class PrefC {
		private static bool _isTreatPlanSortByTooth;
		private static YN _isVerboseLoggingSession;
	
		///<summary>This property is just a shortcut to this pref to make typing faster.  This pref is used a lot.</summary>
		public static bool RandomKeys {
			get {
				return GetBool(PrefName.RandomPrimaryKeys);
			}
		}

		///<summary>Logical shortcut to the ClaimPaymentNoShowZeroDate pref.  Returns 0001-01-01 if pref is disabled.</summary>
		public static DateTime DateClaimReceivedAfter {
			get {
				DateTime date=DateTime.MinValue;
				int days=GetInt(PrefName.ClaimPaymentNoShowZeroDate);
				if(days>=0) {
					date=DateTime.Today.AddDays(-days);
				}
				return date;
			}
		}

		///<summary>This property is just a shortcut to this pref to make typing faster.  This pref is used a lot.</summary>
		public static DataStorageType AtoZfolderUsed {
			get {
				return PIn.Enum<DataStorageType>(GetInt(PrefName.AtoZfolderUsed));
			}
		}

		///<summary>This property is just a shortcut to the negative of the EasyNoClinics pref to make logic easier to follow.</summary>
		public static bool HasClinicsEnabled {
			get {
				return !GetBool(PrefName.EasyNoClinics);
			}
		}

		///<summary>This property is just a shortcut.  Returns true if both StatementsUseSheets and ShowFeatureSuperFamilies are true.</summary>
		public static bool HasSuperStatementsEnabled {
			get {
				return (GetBool(PrefName.StatementsUseSheets) && GetBool(PrefName.ShowFeatureSuperfamilies));
			}
		}

		///<summary>Returns true if DockPhonePanelShow is enabled. Convenience function that should be used if for ODHQ only, and not resellers.</summary>
		/// <returns></returns>
		public static bool IsODHQ {
			get {
				return GetBool(PrefName.DockPhonePanelShow);
			}
		}
		
		///<summary>True if the computer name of this session is included in the HasVerboseLogging PrefValue.</summary>
		public static bool IsVerboseLoggingSession() {			
			try {
				if(Prefs.DictIsNull()) { //Do not allow PrefC.GetString below if we haven't loaded the Pref cache yet. This would cause a recursive loop and stack overflow.
					throw new Exception("Prefs cache is null");
				}
				if(_isVerboseLoggingSession==YN.Unknown) {
					if(PrefC.GetString(PrefName.HasVerboseLogging).ToLower().Split(',').ToList().Exists(x => x==Environment.MachineName.ToLower())) {
						_isVerboseLoggingSession=YN.Yes;
						//Switch logger to a directory that won't have permissions issues.
						Logger.UseMyDocsDirectory();
					}
					else {
						_isVerboseLoggingSession=YN.No;
					}
				}
				return _isVerboseLoggingSession==YN.Yes;
			}
			catch(Exception e) {
				e.DoNothing();
				return false;
			}			
		}

		///<summary>Returns the credentials (user name and password) used to access the voicemail share via SMB2.
		///Used by HQ only.  These preference will not be present in typical databases and this property will then throw an exception.</summary>
		public static NetworkCredential VoiceMailNetworkCredentialsSMB2 {
			get {
				return new NetworkCredential(PrefC.GetString(PrefName.VoiceMailSMB2UserName),PrefC.GetString(PrefName.VoiceMailSMB2Password));
			}
		}

		///<summary>Call this when we have a new Pref cache in order to re-establish logging preference from this computer.</summary>
		public static void InvalidateVerboseLogging() {
			_isVerboseLoggingSession=YN.Unknown;
		}

		///<summary>True if the practice has set a window to restrict the times that automatic communications will be sent out.</summary>
		public static bool DoRestrictAutoSendWindow {
			get {
				//Setting the auto start window equal to the auto stop window is how the restriction is removed.
				return GetDateT(PrefName.AutomaticCommunicationTimeStart).TimeOfDay!=GetDateT(PrefName.AutomaticCommunicationTimeEnd).TimeOfDay;
			}
		}

		///<summary>Returns a valid DateFormat for patient communications.
		///If the current preference is invalid, returns "d" which is equivalent to .ToShortDateString()</summary>
		public static string PatientCommunicationDateFormat {
			get {
				string format=GetString(PrefName.PatientCommunicationDateFormat);
				try {
					DateTime.Today.ToString(format);
				}
				catch(Exception ex) {
					ex.DoNothing();
					format="d";//Default to "d" which is equivalent to .ToShortDateString()
				}
				return format;
			}
		}

		///<summary>Gets a pref of type long.</summary>
		public static long GetLong(PrefName prefName) {
			return PIn.Long(Prefs.GetOne(prefName).ValueString);
		}

		///<summary>Gets a pref of type int32.  Used when the pref is an enumeration, itemorder, etc.  Also used for historical queries in ConvertDatabase.</summary>
		public static int GetInt(PrefName prefName) {
			return PIn.Int(Prefs.GetOne(prefName).ValueString);
		}

		///<summary>Gets a pref of type byte.  Used when the pref is a very small integer (0-255).</summary>
		public static byte GetByte(PrefName prefName) {
			return PIn.Byte(Prefs.GetOne(prefName).ValueString);
		}

		///<summary>Gets a pref of type double.</summary>
		public static double GetDouble(PrefName prefName) {
			return PIn.Double(Prefs.GetOne(prefName).ValueString);
		}

		///<summary>Gets a pref of type bool.</summary>
		public static bool GetBool(PrefName prefName) {
			return PIn.Bool(Prefs.GetOne(prefName).ValueString);
		}

		///<Summary>Gets a pref of type bool, but will not throw an exception if null or not found.  Indicate whether the silent default is true or false.</Summary>
		public static bool GetBoolSilent(PrefName prefName,bool silentDefault) {
			if(Prefs.DictIsNull()) {
				return silentDefault;
			}
			Pref pref=null;
			ODException.SwallowAnyException(() => {
				pref=Prefs.GetOne(prefName);
			});
			return (pref==null ? silentDefault : PIn.Bool(pref.ValueString));
		}

		///<summary>Gets a pref of type string.</summary>
		public static string GetString(PrefName prefName) {
			return Prefs.GetOne(prefName).ValueString;
		}

		///<summary>Gets a pref of type string without using the cache.</summary>
		public static string GetStringNoCache(PrefName prefName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),prefName);
			}
			string command="SELECT ValueString FROM preference WHERE PrefName='"+POut.String(prefName.ToString())+"'";
			return Db.GetScalar(command);
		}

		///<summary>Gets a pref of type string.  Will not throw an exception if null or not found.</summary>
		public static string GetStringSilent(PrefName prefName) {
			if(Prefs.DictIsNull()) {
				return "";
			}
			Pref pref=null;
			ODException.SwallowAnyException(() => {
				pref=Prefs.GetOne(prefName);
			});
			return (pref==null ? "" : pref.ValueString);
		}

		///<summary>Gets a pref of type date.</summary>
		public static DateTime GetDate(PrefName prefName) {
			return PIn.Date(Prefs.GetOne(prefName).ValueString);
		}

		///<summary>Gets a pref of type datetime.</summary>
		public static DateTime GetDateT(PrefName prefName) {
			return PIn.DateT(Prefs.GetOne(prefName).ValueString);
		}

		///<summary>Gets a color from an int32 pref.</summary>
		public static Color GetColor(PrefName prefName) {
			return Color.FromArgb(PIn.Int(Prefs.GetOne(prefName).ValueString));
		}

		///<summary>Used sometimes for prefs that are not part of the enum, especially for outside developers.</summary>
		public static string GetRaw(string prefName) {
			return Prefs.GetOne(prefName).ValueString;
		}

		///<summary>Gets culture info from DB if possible, if not returns current culture.</summary>
		public static CultureInfo GetLanguageAndRegion() {
			CultureInfo cultureInfo=CultureInfo.CurrentCulture;
			ODException.SwallowAnyException(() => {
				Pref pref=Prefs.GetOne("LanguageAndRegion");
				if(!string.IsNullOrEmpty(pref.ValueString)) {
					cultureInfo=CultureInfo.GetCultureInfo(pref.ValueString);
				}
			});
			return cultureInfo;
		}

		///<summary>Returns true if the XCharge program is enabled and at least one clinic has online payments enabled.</summary>
		public static bool HasOnlinePaymentEnabled() {
			Program prog=Programs.GetCur(ProgramName.Xcharge);
			if(!prog.Enabled) {
				return false;
			}
			List<ProgramProperty> listXChargeProps=ProgramProperties.GetForProgram(prog.ProgramNum);
			return listXChargeProps.Any(x => x.PropertyDesc=="IsOnlinePaymentsEnabled" && x.PropertyValue=="1");
		}

		///<summary>Used by an outside developer.</summary>
		public static bool ContainsKey(string prefName) {
			return Prefs.GetContainsKey(prefName);
		}

		///<summary>Used by an outside developer.</summary>
		public static bool HListIsNull() {
			return Prefs.DictIsNull();
		}
		
		///<summary>Static preference used to always reflect FormOpenDental.IsTreatPlanSortByTooth.  
		///This setter should only be called in FormOpenDental.IsTreatPlanSortByTooth.</summary>
		public static bool IsTreatPlanSortByTooth {
			get {
				return _isTreatPlanSortByTooth;
			}
			set {
				_isTreatPlanSortByTooth=value;
			}
		}

		///<summary>Returns the path to the temporary opendental directory, temp/opendental.  Also performs one-time cleanup, if necessary.  In FormOpenDental_FormClosing, the contents of temp/opendental get cleaned up.</summary>
		public static string GetTempFolderPath() {
			//Will clean up entire temp folder for a month after the enhancement of temp file cleanups as long as the temp\opendental folder doesn't already exist.
			string tempPathOD=ODFileUtils.CombinePaths(Path.GetTempPath(),"opendental");
			if(Directory.Exists(tempPathOD)) {
				//Cleanup has already run for the old temp folder.  Do nothing.
				return tempPathOD;
			}
			Directory.CreateDirectory(tempPathOD);
			if(DateTime.Today>PrefC.GetDate(PrefName.TempFolderDateFirstCleaned).AddMonths(1)) {
				return tempPathOD;
			}
			//This might be used if this is the first time running this version on the computer that did the db update.
			//This might also be used if this is a computer that was turned off for a few weeks around the time of update conversion.
			//We need some sort of time limit just in case it's annoying and keeps happening.
			//So this will have a small risk of missing a computer, but the benefit of limiting outweighs the risk.
			//Empty entire temp folder.  Blank folders will be left behind because they do not matter.
			string[] arrayFileNames=Directory.GetFiles(Path.GetTempPath());
			for(int i=0;i<arrayFileNames.Length;i++) {
				try {
					if(arrayFileNames[i].Substring(arrayFileNames[i].LastIndexOf('.'))==".exe" || arrayFileNames[i].Substring(arrayFileNames[i].LastIndexOf('.'))==".cs") {
						//Do nothing.  We don't care about .exe or .cs files and don't want to interrupt other programs' files.
					}
					else {
						File.Delete(arrayFileNames[i]);
					}
				}
				catch {
					//Do nothing because the file could have been in use or there were not sufficient permissions.
					//This file will most likely get deleted next time a temp file is created.
				}
			}
			return tempPathOD;
		}

		///<summary>Creates a new randomly named file in the given directory path with the given extension and returns the full path to the new file.</summary>
		public static string GetRandomTempFile(string ext) {
			return ODFileUtils.CreateRandomFile(GetTempFolderPath(),ext);
		}

		public static long GetDefaultSheetDefNum(SheetTypeEnum sheetType) {
			long retVal=0;
			switch(sheetType) {
				case SheetTypeEnum.Consent:
					retVal=GetLong(PrefName.SheetsDefaultConsent);
					break;
				case SheetTypeEnum.DepositSlip:
					retVal=GetLong(PrefName.SheetsDefaultDepositSlip);
					break;
				case SheetTypeEnum.ExamSheet:
					retVal=GetLong(PrefName.SheetsDefaultExamSheet);
					break;
				case SheetTypeEnum.LabelAppointment:
					retVal=GetLong(PrefName.SheetsDefaultLabelAppointment);
					break;
				case SheetTypeEnum.LabelCarrier:
					retVal=GetLong(PrefName.SheetsDefaultLabelCarrier);
					break;
				case SheetTypeEnum.LabelPatient:
					retVal=GetLong(PrefName.SheetsDefaultLabelPatient);
					break;
				case SheetTypeEnum.LabelReferral:
					retVal=GetLong(PrefName.SheetsDefaultLabelReferral);
					break;
				case SheetTypeEnum.LabSlip:
					retVal=GetLong(PrefName.SheetsDefaultLabSlip);
					break;
				case SheetTypeEnum.MedicalHistory:
					retVal=GetLong(PrefName.SheetsDefaultMedicalHistory);
					break;
				case SheetTypeEnum.MedLabResults:
					retVal=GetLong(PrefName.SheetsDefaultMedLabResults);
					break;
				case SheetTypeEnum.PatientForm:
					retVal=GetLong(PrefName.SheetsDefaultPatientForm);
					break;
				case SheetTypeEnum.PatientLetter:
					retVal=GetLong(PrefName.SheetsDefaultPatientLetter);
					break;
				case SheetTypeEnum.PaymentPlan:
					retVal=GetLong(PrefName.SheetsDefaultPaymentPlan);
					break;
				case SheetTypeEnum.ReferralLetter:
					retVal=GetLong(PrefName.SheetsDefaultReferralLetter);
					break;
				case SheetTypeEnum.ReferralSlip:
					retVal=GetLong(PrefName.SheetsDefaultReferralSlip);
					break;
				case SheetTypeEnum.RoutingSlip:
					retVal=GetLong(PrefName.SheetsDefaultRoutingSlip);
					break;
				case SheetTypeEnum.Rx:
					retVal=GetLong(PrefName.SheetsDefaultRx);
					break;
				case SheetTypeEnum.RxMulti:
					retVal=GetLong(PrefName.SheetsDefaultRxMulti);
					break;
				case SheetTypeEnum.Screening:
					retVal=GetLong(PrefName.SheetsDefaultScreening);
					break;
				case SheetTypeEnum.Statement:
					retVal=GetLong(PrefName.SheetsDefaultStatement);
					break;
				case SheetTypeEnum.TreatmentPlan:
					retVal=GetLong(PrefName.SheetsDefaultTreatmentPlan);
					break;
				default:
					throw new Exception(Lans.g("SheetDefs","Unsupported SheetTypeEnum")+"\r\n"+sheetType.ToString());
			}
			return retVal;
		}

		///<summary>Returns true if the office has a report server set up.</summary>
		public static bool HasReportServer {
			get {
				return !string.IsNullOrEmpty(ReportingServer.Server) || !string.IsNullOrEmpty(ReportingServer.URI);
			}
		}
		
		///<summary>A helper class to get Reporting Server preferences.</summary>
		public static class ReportingServer {
			public static string DisplayStr {
				get {
					if(Server=="") {
						if(URI!="") {
							return "Remote Server"; //will be blank if there is no reporting server set up.
						}
						else {
							return "";
						}
					}
					else {
						return Server+": "+Database;
					}
				}
			}
			public static string URI {
				get {
					return PrefC.GetString(PrefName.ReportingServerURI);
				}
			}
			public static string Server {
				get {
					return PrefC.GetString(PrefName.ReportingServerCompName);
				}
			}
			public static string Database {
				get {
					return PrefC.GetString(PrefName.ReportingServerDbName);
				}
			}
			public static string MySqlUser {
				get {
					return PrefC.GetString(PrefName.ReportingServerMySqlUser);
				}
			}
			public static string MySqlPass {
				get {
					string pass;
					CDT.Class1.Decrypt(PrefC.GetString(PrefName.ReportingServerMySqlPassHash),out pass);
					return pass;
				}
			}
			public static bool IsMiddleTier {
				get {
					return URI!="";
				}
			}
		}

	}
}
