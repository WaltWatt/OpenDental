using CodeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace OpenDentBusiness {

	public class DoseSpot {

		private static string _doseSpotPatNum="25128";//25128 is DoseSpot's patnum in the OD HQ database.
		private static string _doseSpotOid="2.16.840.1.113883.3.4337.25128";
		private static string _randomPhrase32=null;

		#region OID

		///<summary>The PatNum associated to this office's regkey in the OD HQ database.  The patnum will be parsed from the database instead 
		/// of making web calls because Dose Spot would explode if the patnum from OD HQ changed ever</summary>
		public static long DoseSpotCustomerPatNum {
			get {
				string rootExternal=GetDoseSpotRoot();
				long retVal=0;
				//Gets the value of the last section of the root, which for DoseSpot/Internal OIDs is the PatNum of the office in the OD HQ database.
				//This is grabbed instead of asking OD HQ for the patnum associated to the account 
				// because there might be an edge case where the registration key for this office got moved to a new PatNum,
				// and that would hide every DoseSpot OID link.
				try {
					retVal=PIn.Long(rootExternal.Substring(rootExternal.LastIndexOf(".")+1));
				}
				catch(Exception ex) {
					ex.DoNothing();
				}
				return retVal;
			}
		}

		///<summary>Returns the rootExternal that identifies OIDs as Dose Spot </summary>
		public static string GetDoseSpotRoot() {
			//No need to check RemotingRole; no call to db.
			//The advantage of returning the root from the database is that there could be a scary edge case where 
			// the PatNum of the office's regkey in OD HQ database could have been changed
			OIDExternal root=OIDExternals.GetByPartialRootExternal(_doseSpotOid);
			if(root==null) {
				root=new OIDExternal();
				root.IDType=IdentifierType.Root;
				root.rootExternal=OIDInternals.OpenDentalOID+"."+_doseSpotPatNum+"."+OIDInternals.CustomerPatNum;
				OIDExternals.Insert(root);
			}
			return root.rootExternal;
		}

		///<summary>Gets the OIDExternal corresponding to Dose Spot and the patnum given.  Returns null if no match found.</summary>
		public static OIDExternal GetDoseSpotPatID(long patNum) {
			//No remoting role check needed
			return OIDExternals.GetOidExternal(GetDoseSpotRoot()+"."+POut.Int((int)IdentifierType.Patient),patNum,IdentifierType.Patient);
		}

		#endregion

		public static void MakeClinicErxAlert(ClinicErx clinicErx,bool clinicAutomaticallyAttached) {
			AlertItem alert=AlertItems.RefreshForType(AlertType.DoseSpotClinicRegistered)
				.FirstOrDefault(x => x.FKey==clinicErx.ClinicErxNum);
			if(alert!=null) {
				return;//alert already exists
			}
      Clinic clinic=Clinics.GetClinic(clinicErx.ClinicNum);
      List<ProgramProperty> listDoseSpotClinicProperties=ProgramProperties.GetForProgram(Programs.GetProgramNum(ProgramName.eRx))
          .FindAll(x => x.ClinicNum==0
            && (x.PropertyDesc==Erx.PropertyDescs.ClinicID || x.PropertyDesc==Erx.PropertyDescs.ClinicKey)
            && !string.IsNullOrWhiteSpace(x.PropertyValue));
      if(clinic!=null || clinicAutomaticallyAttached) {
        //A clinic was associated with the clinicerx successfully, no user action needed.
        alert=new AlertItem {
          Actions=ActionType.MarkAsRead | ActionType.Delete,
          Description=(clinicErx.ClinicDesc=="" ? "Headquarters" : clinicErx.ClinicDesc)+" "+Lans.g("DoseSpot","has been registered"),
          Severity=SeverityType.Low,
          Type=AlertType.DoseSpotClinicRegistered,
          FKey=clinicErx.ClinicErxNum,
          ClinicNum=clinicErx.ClinicNum,
        };
      }
      else {
        //User action needed to make a link to an existing clinic that was registered.
        alert=new AlertItem {
          Actions=ActionType.MarkAsRead | ActionType.Delete | ActionType.OpenForm,
          Description=Lans.g("DoseSpot","Select clinic to assign ID"),
          Severity=SeverityType.Low,
          Type=AlertType.DoseSpotClinicRegistered,
          ClinicNum=-1,//Show in all clinics.  We only want 1 alert, but that alert can be processed from any clinic because we don't know which clinic to display in.
          FKey=clinicErx.ClinicErxNum,
          FormToOpen=FormType.FormDoseSpotAssignClinicId,
        };
      }
      AlertItems.Insert(alert);
		}

		///<summary>Handles assigning Dose Spot ID to a user with matching NPI.
		///If multiple or no matches, creates form for manual selection of user.</summary>
		public static void MakeProviderErxAlert(ProviderErx providerErx) {
			AlertItem alert=AlertItems.RefreshForType(AlertType.DoseSpotProviderRegistered)
				.FirstOrDefault(x => x.FKey==providerErx.ProviderErxNum);
			if(alert!=null) {
				return;//alert already exists
			}
			//get a list of users that correspond to a non-hidden provider
			List<Provider> listProviders=Providers.GetWhere(x => x.NationalProvID==providerErx.NationalProviderID,true);
			List<Userod> listDoseUsers=Userods.GetWhere(x => listProviders.Exists(y => y.ProvNum==x.ProvNum),true);//Only consider non-hidden users.
			if(listDoseUsers.Count==1) {//One provider matched so simply notify the office and set the DoseSpot User Id.
				alert=new AlertItem {
					Actions=ActionType.MarkAsRead | ActionType.Delete | ActionType.ShowItemValue,
					Description=Lans.g("DoseSpot","User automatically assigned."),
					Severity=SeverityType.Low,
					Type=AlertType.DoseSpotProviderRegistered,
					FKey=providerErx.ProviderErxNum,
					ClinicNum=-1,//Show in all clinics.  We only want 1 alert, but that alert can be processed from any clinic because providers aren't clinic specific
					ItemValue=Lans.g("DoseSpot","User: ")+listDoseUsers[0].UserNum+", "+listDoseUsers[0].UserName+" "
					+Lans.g("DoseSpot","has been assigned a DoseSpot User ID of: ")+providerErx.UserId,
				};
				AlertItems.Insert(alert);
				//set userodpref to UserId
				Program programErx=Programs.GetCur(ProgramName.eRx);
				UserOdPref userDosePref=UserOdPrefs.GetByCompositeKey(listDoseUsers[0].UserNum,programErx.ProgramNum,UserOdFkeyType.Program);
				userDosePref.ValueString=providerErx.UserId;//assign DoseSpot User ID
				if(userDosePref.IsNew) {
					userDosePref.Fkey=programErx.ProgramNum;
					UserOdPrefs.Insert(userDosePref);
				}
				else {
					UserOdPrefs.Update(userDosePref);
				}
			}
			else {//More than one or no user associated to the NPI, generate alert with form to have the office choose which user to assign.
				alert=new AlertItem {
					Actions=ActionType.MarkAsRead | ActionType.Delete | ActionType.OpenForm,
					Description=Lans.g("DoseSpot","Select user to assign ID"),
					Severity=SeverityType.Low,
					Type=AlertType.DoseSpotProviderRegistered,
					FKey=providerErx.ProviderErxNum,
					ClinicNum=-1,//Show in all clinics.  We only want 1 alert, but that alert can be processed from any clinic because providers aren't clinic specific
					FormToOpen=FormType.FormDoseSpotAssignUserId,
				};
				AlertItems.Insert(alert);
			}
		}

		///<summary>Returns true if the passed in accountId is a DoseSpot account id.</summary>
		public static bool IsDoseSpotAccountId(string accountId) {
			//No need to check RemotingRole; no call to db.
			return accountId.ToUpper().StartsWith("DS;");//ToUpper because user might have manually typed in.
		}

		///<summary>Creates a unique account id for DoseSpot.  Uses the same generation logic as NewCrop, with DS; preceeding it.</summary>
		public static string GenerateAccountId(long patNum) {
			string retVal="DS;"+POut.Long(patNum);
			retVal+="-"+CodeBase.MiscUtils.CreateRandomAlphaNumericString(3);
			long checkSum=patNum;
			checkSum+=Convert.ToByte(retVal[retVal.IndexOf('-')+1])*3;
			checkSum+=Convert.ToByte(retVal[retVal.IndexOf('-')+2])*5;
			checkSum+=Convert.ToByte(retVal[retVal.IndexOf('-')+3])*7;
			retVal+=(checkSum%100).ToString().PadLeft(2,'0');
			return retVal;
		}

		///<summary>The comments on each line come directly from the DoseSpot API Guide 12.8.pdf.  Creates a Single Sign On Code for DoseSpot.</summary>
		public static string CreateSsoCode(string clinicKey,bool isQueryStr=true) {
			//No need to check RemotingRole; no call to db.
			string retVal="";//1. You have been provided a clinic key (in UTF8).
			string phrase=Get32CharPhrase();//2. Create a random phrase 32 characters long in UTF8
			string phraseAndKey=phrase;
			phraseAndKey+=clinicKey;//3. Append the key to the phrase
			byte[] arrayBytes=GetBytesFromUTF8(phraseAndKey);//4. Get the value in Bytes from UTF8 String
			byte[] arrayHashedBytes=GetSHA512Hash(arrayBytes);//5. Use SHA512 to hash the byte value you just received
			string base64hash=Convert.ToBase64String(arrayHashedBytes);//6. Get a Base64String out of the hash that you created
			base64hash=RemoveExtraEqualSigns(base64hash);//7. If there are two = signs at the end, then remove them
			retVal=phrase+base64hash;//8. Prepend the same random phrase from step 2 to your code
			if(isQueryStr) {//9. If the SingleSignOnCode is going to be passed in a query string, be sure to UrlEncode the entire code
				retVal=WebUtility.UrlEncode(retVal);
			}
			return retVal;
		}

		///<summary>The comments on each line come directly from the DoseSpot API Guide 12.8.pdf.  Creates a Single Sign On User ID Verify for DoseSpot.</summary>
		public static string CreateSsoUserIdVerify(string clinicKey,string userID,bool isQueryStr=true) {
			//No need to check RemotingRole; no call to db.
			string retVal="";
			string phrase=Get32CharPhrase();
			string idPhraseAndKey=phrase.Substring(0,22);//1. Grab the first 22 characters of the phrase used in CreateSSOCode from step 1 of CreateSsoCode
			idPhraseAndKey=userID+idPhraseAndKey;//2. Append to the UserID string the 22 characters grabbed from step one
			idPhraseAndKey+=clinicKey;//3. Append the key to the string created in 2b
			byte[] arrayBytes=GetBytesFromUTF8(idPhraseAndKey);//4. Get the Byte value of the string
			byte[] arrayHashedBytes=GetSHA512Hash(arrayBytes);//5. Use SHA512 to hash the byte value you just received
			string base64hash=Convert.ToBase64String(arrayHashedBytes);//6. Get a Base64String out of the hash that you created
			retVal=RemoveExtraEqualSigns(base64hash);//7. If there are two = signs at the end, then remove them
			if(isQueryStr) {//8. If the SingleSignOnUserIdVerify is going to be passed in a query string, be sure to UrlEncode the entire code
				retVal=WebUtility.UrlEncode(retVal);
			}
			_randomPhrase32=null;
			return retVal;
		}

		///<summary>Can throw exceptions.  Returns true if changes were made to medications.</summary>
		public static bool SyncPrescriptionsFromDoseSpot(string clinicID,string clinicKey,string userID,long patNum,Action<List<RxPat>> onRxAdd=null) {
			//No need to check RemotingRole; no call to db.
			OIDExternal oidPatID=DoseSpot.GetDoseSpotPatID(patNum);
			if(oidPatID==null) {
				return false;//We don't have a PatID from DoseSpot for this patient.  Therefore there is nothing to sync with.
			}
			DoseSpotService.API api=new DoseSpotService.API();
#if DEBUG
			api.Url="https://my.staging.dosespot.com/api/12/api.asmx?wsdl";
#endif
			DoseSpotService.GetMedicationListRequest req=new DoseSpotService.GetMedicationListRequest();
			req.SingleSignOn=GetSingleSignOn(clinicID,clinicKey,userID,false);
			req.PatientId=int.Parse(oidPatID.IDExternal);//If this fails (and throws an exception), we got the wrong oid
			req.Sources=new[] { DoseSpotService.MedicationSourceType.Imported,DoseSpotService.MedicationSourceType.Prescription,
				DoseSpotService.MedicationSourceType.SelfReported, DoseSpotService.MedicationSourceType.SurescriptsHistory };
			req.Status=new[] { DoseSpotService.MedicationStatusType.Active };
#if DEBUG
			//This code will output the XML into the console.  This may be needed for DoseSpot when troubleshooting issues.
			//This XML will be the soap body and exclude the header and envelope.
			System.Xml.Serialization.XmlSerializer xml=new System.Xml.Serialization.XmlSerializer(req.GetType());
			xml.Serialize(Console.Out,req);
#endif
			DoseSpotService.GetMedicationListResponse res=api.GetMedicationList(req);
			if(res.Result!=null && res.Result.ResultCode.ToLower().Contains("error")) {
				throw new Exception(res.Result.ResultDescription);
			}
			Patient patCur=Patients.GetPat(patNum);
			List<long> listActiveMedicationPatNums=new List<long>();
			Dictionary<int,string> dictPharmacyNames=new Dictionary<int,string>();
			List<RxPat> listNewRx=new List<RxPat>();
			foreach(DoseSpotService.MedicationListItem medication in res.Medications) {
				RxPat rxOld=null;
				if(medication.Source==DoseSpotService.MedicationSourceType.SelfReported) {
					rxOld=RxPats.GetErxByIdForPat(Erx.OpenDentalErxPrefix+medication.MedicationId.ToString(),patNum);
					if(rxOld==null) {
						rxOld=RxPats.GetErxByIdForPat(Erx.DoseSpotPatReportedPrefix+medication.MedicationId.ToString(),patNum);
					}
				}
				else {
					rxOld=RxPats.GetErxByIdForPat(medication.MedicationId.ToString(),patNum);
				}
				RxPat rx=new RxPat();
				long rxCui=medication.RxCUI.HasValue ? medication.RxCUI.Value : 0;//Cast from int to long is intentional.
				rx.IsControlled=(PIn.Int(medication.Schedule)!=0);//Controlled if Schedule is I,II,III,IV,V
				rx.DosageCode="";
				rx.SendStatus=RxSendStatus.Unsent;
				switch(medication.PrescriptionStatus) {
					case DoseSpotService.PrescriptionStatusType.eRxSent:
						rx.SendStatus=RxSendStatus.SentElect;
						break;
					case DoseSpotService.PrescriptionStatusType.FaxSent:
						rx.SendStatus=RxSendStatus.Faxed;
						break;
					case DoseSpotService.PrescriptionStatusType.Printed:
						rx.SendStatus=RxSendStatus.Printed;
						break;
					case DoseSpotService.PrescriptionStatusType.Sending:
						rx.SendStatus=RxSendStatus.Pending;
						break;
					case DoseSpotService.PrescriptionStatusType.Deleted:
					case DoseSpotService.PrescriptionStatusType.Error:
					case DoseSpotService.PrescriptionStatusType.EpcsError:
						continue;//Skip these medications since DoseSpot is saying that they are invalid
					case DoseSpotService.PrescriptionStatusType.Edited:
					case DoseSpotService.PrescriptionStatusType.Entered:
					case DoseSpotService.PrescriptionStatusType.EpcsSigned:
					case DoseSpotService.PrescriptionStatusType.ReadyToSign:
					case DoseSpotService.PrescriptionStatusType.Requested:
					default:
						rx.SendStatus=RxSendStatus.Unsent;
						break;
				}
				rx.Notes=medication.PharmacyNotes;
				rx.Refills=medication.Refills;
				rx.Disp=medication.Quantity;//In DoseSpot, the Quanitity textbox's label says "Dispense".
				rx.Drug=medication.DisplayName;
				if(medication.PharmacyId.HasValue) {
					try {
						if(!dictPharmacyNames.ContainsKey(medication.PharmacyId.Value)) {
							DoseSpotService.PharmacyValidateMessage reqPharmacy=new DoseSpotService.PharmacyValidateMessage();
							reqPharmacy.SingleSignOn=GetSingleSignOn(clinicID,clinicKey,userID,false);
							reqPharmacy.PharmacyId=medication.PharmacyId.Value;
							DoseSpotService.PharmacyValidateMessageResult resPharmacy=api.PharmacyValidate(reqPharmacy);
							dictPharmacyNames.Add(medication.PharmacyId.Value,resPharmacy.Pharmacy.StoreName);
						}
						rx.ErxPharmacyInfo=dictPharmacyNames[medication.PharmacyId.Value];
					}
					catch(Exception ex) {
						ex.DoNothing();
						//Do nothing.  It was a nicety anyways.
					}
				}
				rx.PatNum=patNum;
				rx.Sig=medication.Notes;
				//In the future, we could get medication.PharmacyId and get pharmacy info
				rx.RxDate=DateTime.MinValue;
				//If none of dates have values, the RxDate will be MinValue.
				//This is acceptable if DoseSpot doesn't give us anything, which should never happen.
				if(medication.DateWritten.HasValue) {
					rx.RxDate=medication.DateWritten.Value;
				}
				else if(medication.DateReported.HasValue) {
					rx.RxDate=medication.DateReported.Value;
				}
				else if(medication.DateLastFilled.HasValue) {
					rx.RxDate=medication.DateLastFilled.Value;
				}
				else if(medication.DateInactive.HasValue) {
					rx.RxDate=medication.DateInactive.Value;
				}
				//Save DoseSpot's unique ID into our rx
				int doseSpotMedId=medication.MedicationId;//If this changes, we need to ensure that Erx.IsFromDoseSpot() is updated to match.
				rx.ErxGuid=doseSpotMedId.ToString();
				bool isProv=false;
				if(medication.Source==DoseSpotService.MedicationSourceType.SelfReported) {//Self Reported medications won't have a prescriber number
					if(rxOld==null) {//Rx doesn't exist in the database.  This probably originated from DoseSpot
						MedicationPat medPat=MedicationPats.GetMedicationOrderByErxIdAndPat(Erx.OpenDentalErxPrefix+medication.MedicationId.ToString(),patNum);
						if(medPat==null) {//If there isn't a record of the medication 
							medPat=MedicationPats.GetMedicationOrderByErxIdAndPat(Erx.DoseSpotPatReportedPrefix+medication.MedicationId.ToString(),patNum);
						}
						if(medPat==null) {//If medPat is null at this point we don't have a record of this patient having the medication, so it probably was just made in DoseSpot.
							rx.ErxGuid=Erx.DoseSpotPatReportedPrefix+medication.MedicationId;
						}
						else {
							rx.ErxGuid=medPat.ErxGuid;//Maintain the ErxGuid that was assigned for the MedicationPat that already exists.
						}
					}
					else {
						rx.ErxGuid=rxOld.ErxGuid;//Maintain the ErxGuid that was already assigned for the Rx.
					}
				}
				else {
					//The prescriber ID for each medication is the doctor that approved the prescription.
					UserOdPref userPref=UserOdPrefs.GetByFkeyAndFkeyType(Programs.GetCur(ProgramName.eRx).ProgramNum,UserOdFkeyType.Program)
					.FirstOrDefault(x => x.ValueString==medication.PrescriberUserId.ToString());
					if(userPref==null) {//The Dose Spot User ID from this medication is not present in Open Dental.
						continue;//I don't know if we want to do anything with this.  Maybe we want to just get the ErxLog from before this medication was made.
					}
					Userod user=Userods.GetUser(userPref.UserNum);
					Provider prov=new Provider();
					isProv=!Erx.IsUserAnEmployee(user);
					if(isProv) {//A user always be a provider if there is a ProvNum > 0
						prov=Providers.GetProv(user.ProvNum);
					}
					else {
						prov=Providers.GetProv(patCur.PriProv);
					}
					rx.ProvNum=prov.ProvNum;
				}
				long medicationPatNum=0;
				if(Erx.IsDoseSpotPatReported(rx.ErxGuid)) {//For DoseSpot self reported, do not insert a prescription.
					medicationPatNum=Erx.InsertOrUpdateErxMedication(rxOld,rx,rxCui,medication.DisplayName,medication.GenericDrugName,isProv,false);
				}
				else {
					medicationPatNum=Erx.InsertOrUpdateErxMedication(rxOld,rx,rxCui,medication.DisplayName,medication.GenericDrugName,isProv);
				}
				if(rxOld==null) {//Only add the rx if it is new.  We don't want to trigger automation for existing prescriptions.
					listNewRx.Add(rx);
				}
				listActiveMedicationPatNums.Add(medicationPatNum);
			}
			List<MedicationPat> listAllMedicationsForPatient=MedicationPats.Refresh(patNum,false);
			foreach(MedicationPat medication in listAllMedicationsForPatient) {
				if(!Erx.IsFromDoseSpot(medication.ErxGuid)) {
					continue;//This medication is not an eRx medicaiton.  It was probably entered manually inside OD.
				}
				if(listActiveMedicationPatNums.Contains(medication.MedicationPatNum)) {
					continue;//The medication is still active.
				}
				//The medication was discontinued inside the eRx interface.
				medication.DateStop=DateTime.Today.AddDays(-1);//Discontinue the medication as of yesterday so that it will immediately show as discontinued.
				MedicationPats.Update(medication,false);//Discontinue the medication inside OD to match what shows in the eRx interface.
			}//end foreach
			if(onRxAdd!=null && listNewRx.Count!=0) {
				onRxAdd(listNewRx);
			}
			return true;
		}

		///<summary></summary>
		public static void SyncPrescriptionsToDoseSpot(string clinicID,string clinicKey,string userID,long patNum) {
			//No need to check RemotingRole; no call to db.
			OIDExternal oidPatID=DoseSpot.GetDoseSpotPatID(patNum);
			if(oidPatID==null) {
				return;//We don't have a PatID from DoseSpot for this patient.  Therefore there is nothing to sync with.
			}
			DoseSpotService.API api=new DoseSpotService.API();
#if DEBUG
			api.Url="https://my.staging.dosespot.com/api/12/api.asmx?wsdl";
#endif
			DoseSpotService.AddSelfReportedMedicationsRequest req=new DoseSpotService.AddSelfReportedMedicationsRequest();
			req.SingleSignOn=GetSingleSignOn(clinicID,clinicKey,userID,false);
			req.PatientId=PIn.Int(oidPatID.IDExternal);
			req.ReportingUserId=PIn.Int(userID);
			List<DoseSpotService.SelfReportedMedication> listDoseSpotMedications=new List<DoseSpotService.SelfReportedMedication>(); 
			List<MedicationPat> listAllMedicationsForPatient=MedicationPats.Refresh(patNum,true).FindAll(x => !Erx.IsFromDoseSpot(x.ErxGuid));
			if(listAllMedicationsForPatient.Count==0) {
				return;//There are no medications to send to DoseSpot.
			}
			foreach(MedicationPat medPat in listAllMedicationsForPatient) {
				if(Erx.IsFromDoseSpot(medPat.ErxGuid) 
					|| Erx.IsTwoWayIntegrated(medPat.ErxGuid)
					|| Erx.IsDoseSpotPatReported(medPat.ErxGuid))
				{//Only send medications that DoseSpot doesn't already have
					continue;
				}
				DoseSpotService.SelfReportedMedication medCur=new DoseSpotService.SelfReportedMedication();
				try {
					//Casting a long to an int sucks but I can't think of any other unique identifier.
					medCur.SelfReportedMedicationId=(int)medPat.MedicationPatNum;
				}
				catch(Exception ex) {
					ex.DoNothing();
					continue;
				}
				medCur.Status=DoseSpotService.PatientMedicationStatusType.Inactive;
				if(medPat.DateStop.Year<1880 || medPat.DateStop>=DateTime.Today) {
					medCur.Status=DoseSpotService.PatientMedicationStatusType.Active;
				}
				else if(medPat.DateStop<DateTime.Today) {
					medCur.Status=DoseSpotService.PatientMedicationStatusType.Discontinued;
					medCur.DiscontinuedDate=medPat.DateStop;
					//A comment is required when a medication has been discontinued (figured out through testing, not in docs)
					medCur.Comment="Discontinued in Open Dental";
				}
				//Either MedDescript should be set or MedicationNum should be non-zero.
				//MedicationNum will always be set for medications created inside OD.
				//MedDescript is used when a medication was imported via sync from eRx.
				if(String.IsNullOrEmpty(medPat.MedDescript) && medPat.MedicationNum!=0) {
					Medication med=Medications.GetMedication(medPat.MedicationNum);
					medCur.DisplayName=med.MedName;
				}
				else {
					medCur.DisplayName=medPat.MedDescript;
				}
				if(String.IsNullOrEmpty(medCur.DisplayName)) {
					continue;//Should not happen, but just in case.
				}
				listDoseSpotMedications.Add(medCur);
				//Update the medPat to store an ErxGuid so that we don't keep sending this to DoseSpot unnecessarily.
				medPat.ErxGuid=Erx.OpenDentalErxPrefix+medCur.SelfReportedMedicationId;//Will now be flagged as two-way integrated and will not send again until modified.
				MedicationPats.Update(medPat,false);//Don't check ErxGuid here, we just set it purposefully above.
			}
			if(listDoseSpotMedications.Count==0) {
				return;//There are no medications to send to DoseSpot.
			}
			req.Medications=listDoseSpotMedications.ToArray();
			DoseSpotService.AddSelfReportedMedicationsResponse res=api.AddUpdateSelfReportedMedications(req);
		}

		///<summary>Returns true if at least one of the counts is greater than 0.
		///This is used for notifying prescribers when they need to take action is DoseSpot.</summary>
		public static bool GetRefillRequestsAndTransmissionsErrorCounts(string clinicID,string clinicKey,string userID
			,out int countRefillReqs,out int countTransmissionErrors)
		{
			//No need to check RemotingRole; no call to db.
			countRefillReqs=0;
			countTransmissionErrors=0;
			DoseSpotService.API api=new DoseSpotService.API();
#if DEBUG
			api.Url="https://my.staging.dosespot.com/api/12/api.asmx?wsdl";
#endif
			DoseSpotService.RefillRequestsTransmissionErrorsMessageRequest req=new DoseSpotService.RefillRequestsTransmissionErrorsMessageRequest();
			req.SingleSignOn=GetSingleSignOn(clinicID,clinicKey,userID,false);
			req.ClinicianId=PIn.Int(userID);
			DoseSpotService.RefillRequestsTransmissionErrorsMessageResult res=api.GetRefillRequestsTransmissionErrors(req);
			if(res.Result!=null && res.Result.ResultCode.ToLower()=="error") {
				throw new Exception(res.Result.ResultDescription);
			}
			if(res.RefillRequestsTransmissionErrors.Length>0) {//In testing, calling the preproduction api always returned 1 item in the array
				countRefillReqs=res.RefillRequestsTransmissionErrors[0].RefillRequestsCount;
				countTransmissionErrors=res.RefillRequestsTransmissionErrors[0].TransactionErrorsCount;
			}
			return (countRefillReqs>0 || countTransmissionErrors>0);//Return true if there is anything that the prescriber needs to see/take action on.
		}

		///<summary>Returns true if at least one of the counts is greater than 0.
		///This is used for notifying prescribers when they need to take action is DoseSpot.</summary>
		public static bool GetPrescriberNotificationCounts(string clinicID,string clinicKey,string userID
			,out int countRefillReqs,out int countTransactionErrors,out int countPendingPrescriptionsCount)
		{
			//No need to check RemotingRole; no call to db.
			countRefillReqs=0;
			countTransactionErrors=0;
			DoseSpotService.API api=new DoseSpotService.API();
#if DEBUG
			api.Url="https://my.staging.dosespot.com/api/12/api.asmx?wsdl";
#endif
			DoseSpotService.GetPrescriberNotificationCountsRequest req=new DoseSpotService.GetPrescriberNotificationCountsRequest();
			req.SingleSignOn=GetSingleSignOn(clinicID,clinicKey,userID,false);
			DoseSpotService.GetPrescriberNotificationCountsResponse res=api.GetPrescriberNotificationCounts(req);
			if(res.Result!=null && res.Result.ResultCode.ToLower()=="error") {
				throw new Exception(res.Result.ResultDescription);
			}
			countRefillReqs=res.RefillRequestsCount;
			countTransactionErrors=res.TransactionErrorsCount;
			countPendingPrescriptionsCount=res.PendingPrescriptionsCount;
			return (countRefillReqs>0 || countTransactionErrors>0 || countPendingPrescriptionsCount>0);//Return true if there is anything that the prescriber needs to see/take action on.
		}

		///<summary>If pat is null, it is assumed that we are making a SSO request for errors and refill requests.
		///Throws exceptions for invalid Patient data.</summary>
		public static string GetSingleSignOnQueryString(string clinicID,string clinicKey,string userID,string onBehalfOfUserId,Patient pat) {
			//No need to check RemotingRole; no call to db.
			DoseSpotService.SingleSignOn sso=GetSingleSignOn(clinicID,clinicKey,userID,true);
			StringBuilder sb=new StringBuilder();
			QueryStringAddParameter(sb,"SingleSignOnCode",sso.SingleSignOnCode);
			QueryStringAddParameter(sb,"SingleSignOnUserId",POut.Int(sso.SingleSignOnUserId));
			QueryStringAddParameter(sb,"SingleSignOnUserIdVerify",sso.SingleSignOnUserIdVerify);
			QueryStringAddParameter(sb,"SingleSignOnClinicId",POut.Int(sso.SingleSignOnClinicId));
			if(!String.IsNullOrWhiteSpace(onBehalfOfUserId)) {
				QueryStringAddParameter(sb,"OnBehalfOfUserId",POut.String(onBehalfOfUserId));
			}
			if(pat==null) {
				QueryStringAddParameter(sb,"RefillsErrors",POut.Int(1));//Request transmission errors
			}
			else {
				sb.Append(GetPatientQueryString(pat));
			}
			return sb.ToString();
		}

		///<summary>Throws exceptions.
		///Gets the clinicID/clinicKey regarding the passed in clinicNum.
		///Will register the passed in clinicNum with DoseSpot if it isn't already.
		///Validates if the passed in doseSpotUserID is empty.</summary>
		public static void GetClinicIdAndKey(long clinicNum,string doseSpotUserID,Program programErx,List<ProgramProperty> listErxProperties
			,out string clinicID,out string clinicKey)
		{
			//No need to check RemotingRole; no call to db.
			clinicID="";
			clinicKey="";
			if(programErx==null) {
				programErx=Programs.GetCur(ProgramName.eRx);
			}
			if(listErxProperties==null) {
				listErxProperties=ProgramProperties.GetForProgram(programErx.ProgramNum)
					.FindAll(x => x.ClinicNum==clinicNum
						&& (x.PropertyDesc==Erx.PropertyDescs.ClinicID || x.PropertyDesc==Erx.PropertyDescs.ClinicKey));
			}
			ProgramProperty ppClinicID=listErxProperties.FirstOrDefault(x => x.ClinicNum==clinicNum && x.PropertyDesc==Erx.PropertyDescs.ClinicID);
			ProgramProperty ppClinicKey=listErxProperties.FirstOrDefault(x => x.ClinicNum==clinicNum && x.PropertyDesc==Erx.PropertyDescs.ClinicKey);
			//If the current clinic doesn't have a clinic id/key, use a different clinic to make them.
			if(ppClinicID==null || string.IsNullOrWhiteSpace(ppClinicID.PropertyValue)
				|| ppClinicKey==null || string.IsNullOrWhiteSpace(ppClinicKey.PropertyValue))
			{
				throw new ODException(((clinicNum==0)?"HQ ":Clinics.GetAbbr(clinicNum)+" ")
					+Lans.g("DoseSpot","is missing a valid ClinicID or Clinic Key.  This should have been entered when setting up DoseSpot."));
			}
			else {
				clinicID=ppClinicID.PropertyValue;
				clinicKey=ppClinicKey.PropertyValue;
			}
		}

		///<summary>Throws Exceptions if clinic is deemed invalid for DoseSpot.
		///Register the passed in clinicNum in DoseSpot, and pass out the registered ID/key.
		///Will save the new clinic id/clinic key into program properties for the given clinic and refreshes cache.</summary>
		public static void RegisterClinic(long clinicNum,string clinicID,string clinicKey,string userID
			,out string clinicIdNew,out string clinicKeyNew) 
		{
			//No need to check RemotingRole; no call to db.
			Clinic clinicCur=GetClinicOrPracticeInfo(clinicNum);
			DoseSpotService.API api=new DoseSpotService.API();
			DoseSpotService.ClinicAddMessage req=new DoseSpotService.ClinicAddMessage();
			req.Clinic=MakeDoseSpotClinic(clinicCur);
			req.SingleSignOn=GetSingleSignOn(clinicID,clinicKey,userID,false);
#if DEBUG
			//This code will output the XML into the console.  This may be needed for DoseSpot when troubleshooting issues.
			//This XML will be the soap body and exclude the header and envelope.
			System.Xml.Serialization.XmlSerializer xml=new System.Xml.Serialization.XmlSerializer(req.GetType());
			xml.Serialize(Console.Out,req);
#endif
			DoseSpotService.ClinicAddResultMessage res=api.ClinicAdd(req);
			if(res.Result!=null && res.Result.ResultCode.ToLower().Contains("error")) {
				throw new Exception(res.Result.ResultDescription);
			}
			clinicIdNew=res.Clinic.ClinicId.Value.ToString();
			clinicKeyNew=res.ClinicKey;
			long erxProgramNum=Programs.GetCur(ProgramName.eRx).ProgramNum;
			List<ProgramProperty> listPropsForClinic=ProgramProperties.GetListForProgramAndClinic(erxProgramNum,clinicNum);
			ProgramProperty ppClinicID=listPropsForClinic.FirstOrDefault(x => x.PropertyDesc==Erx.PropertyDescs.ClinicID);
			ProgramProperty ppClinicKey=listPropsForClinic.FirstOrDefault(x => x.PropertyDesc==Erx.PropertyDescs.ClinicKey);
			//Update the database with the new id/key.
			InsertOrUpdate(ppClinicID,erxProgramNum,Erx.PropertyDescs.ClinicID,clinicIdNew,clinicNum);
			InsertOrUpdate(ppClinicKey,erxProgramNum,Erx.PropertyDescs.ClinicKey,clinicKeyNew,clinicNum);
			//Ensure cache is not stale after setting the values.
			Cache.Refresh(InvalidType.Programs);
		}

		///<summary>Throws exceptions when validating clinic/practice info/provider.
		///Will register the passed in userNum if that user is not already registered.
		///Updates the passed in user's UserOdPref for DoseSpot User ID</summary>
		public static string GetUserID(Userod userCur,long clinicNum) {
			//No need to check RemotingRole; no call to db.
			string retVal="";
			Clinic clinicCur=GetClinicOrPracticeInfo(clinicNum);
			//At this point we know that we have a valid clinic/practice info and valid provider.
			Program programErx=Programs.GetCur(ProgramName.eRx);
			//Get the DoseSpotID for the current user
			UserOdPref userPrefDoseSpotID=GetDoseSpotUserIdFromPref(userCur.UserNum,clinicNum);
			//If the current user doesn't have a valid User ID, go retreive one from DoseSpot.
			if(userPrefDoseSpotID==null || string.IsNullOrWhiteSpace(userPrefDoseSpotID.ValueString)) {
				//If there is no UserId for this user, throw an exception.  The below code was when we thought the Podio database matched the DoseSpot database.
				//The below code would add a proxy clinician via the API and give back the DoseSpot User ID.
				//This was causing issues with Podio and making sure the proxy clinician has access to the appropriate clinics.
				throw new ODException("Missing DoseSpot User ID for user.  Call support to register provider or proxy user, then enter User ID into security window.");
				#region Old Proxy User Registration
				//        UserOdPref otherRegisteredClinician=UserOdPrefs.GetAllByFkeyAndFkeyType(programErx.ProgramNum,UserOdFkeyType.Program)
				//          .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.ValueString) && Userods.GetUser(x.UserNum).ProvNum!=0);
				//        //userCur.ProvNum==0 means that this is a real clinician.  
				//        //We can add proxy clinicians for no charge, but actual clinicians will incur a fee.
				//        if(!Erx.IsUserAnEmployee(userCur) || otherRegisteredClinician==null) {
				//          //Either the prov isn't registered, or there are no credentials to create the proxy clinician.
				//          //Either way, we want the user to know they need to register the provider.
				//          throw new ODException("Missing DoseSpot User ID for provider.  Call support to register provider, then enter User ID into security window.");
				//        }
				//        //Get the provider from the doseSpotUserID we are using.  This ensures that DoseSpot knows the provider is valid,
				//        //instead of passing in the patient's primary provider, which may not be registered in DoseSpot.
				//        Provider provOther=Providers.GetProv(Userods.GetUser(otherRegisteredClinician.UserNum).ProvNum);
				//				ValidateProvider(provOther,clinicNum);
				//				string defaultDoseSpotUserID=otherRegisteredClinician.ValueString;
				//				string clinicID="";
				//				string clinicKey="";
				//				GetClinicIdAndKey(clinicNum,defaultDoseSpotUserID,programErx,null,out clinicID,out clinicKey);
				//				DoseSpotService.API api=new DoseSpotService.API();
				//				DoseSpotService.ClinicianAddMessage req=new DoseSpotService.ClinicianAddMessage();
				//				req.SingleSignOn=GetSingleSignOn(clinicID,clinicKey,defaultDoseSpotUserID,false);
				//				EmailAddress email=EmailAddresses.GetForUser(userCur.UserNum);
				//				if(email==null || string.IsNullOrWhiteSpace(email.EmailUsername)) {
				//					throw new ODException("Invalid email address for the current user.");
				//				}
				//				req.Clinician=MakeDoseSpotClinician(provOther,clinicCur,email.EmailUsername,true);//If the user isn't a provider, they are a proxy clinician.
				//#if DEBUG
				//				//This code will output the XML into the console.  This may be needed for DoseSpot when troubleshooting issues.
				//				//This XML will be the soap body and exclude the header and envelope.
				//				System.Xml.Serialization.XmlSerializer xml=new System.Xml.Serialization.XmlSerializer(req.GetType());
				//				xml.Serialize(Console.Out,req);
				//#endif
				//				DoseSpotService.ClinicianAddResultsMessage res=api.ClinicianAdd(req);
				//				if(res.Result!=null && (res.Result.ResultCode.ToLower().Contains("error") || res.Clinician==null)) {
				//					throw new Exception(res.Result.ResultDescription);
				//				}
				//				retVal=res.Clinician.ClinicianId.ToString();
				//				//Since userPrefDoseSpotID can't be null, we just overwrite all of the fields to be sure that they are correct.
				//				userPrefDoseSpotID.UserNum=userCur.UserNum;
				//				userPrefDoseSpotID.Fkey=programErx.ProgramNum;
				//				userPrefDoseSpotID.FkeyType=UserOdFkeyType.Program;
				//				userPrefDoseSpotID.ValueString=retVal;
				//				if(userPrefDoseSpotID.IsNew) {
				//					UserOdPrefs.Insert(userPrefDoseSpotID);
				//				}
				//				else {
				//					UserOdPrefs.Update(userPrefDoseSpotID);
				//				}
				#endregion
			}
			else {
				retVal=userPrefDoseSpotID.ValueString;
			}
			return retVal;
		}

		///<summary>Parses the passed in queryString for the value of the passed in parameter name.
		///Returns null if a match isn't found.</summary>
		public static string GetQueryParameterFromQueryString(string queryString,string parameterNameToReturn) {
			//No need to check RemotingRole; no call to db.
			if(!string.IsNullOrWhiteSpace(parameterNameToReturn)) {
				if(queryString[0]=='?') {
					queryString=queryString.Substring(1);
				}
				string[] arrayQueries=queryString.Split('&');
				foreach(string queryCur in arrayQueries) {
					if(!String.IsNullOrWhiteSpace(queryCur) && queryCur.Contains("=")) {
						string[] arrayQueryCur=queryCur.Split('=');
						string paramNameCur=arrayQueryCur[0];
						if(paramNameCur!=null && paramNameCur.ToUpper().Equals(parameterNameToReturn.ToUpper())) {
							return arrayQueryCur[1];
						}
					}
				}
			}
			return null;
		}

		///<summary></summary>
		public static void SyncClinicErxsWithHQ() {
			//No need to check RemotingRole; no call to db.
			MakeClinicErxsForDoseSpot();
			List<ClinicErx> listClinicErxsToSend=ClinicErxs.GetWhere(x => x.EnabledStatus!=ErxStatus.Enabled);
			//Currently we do not have any intention of disabling clinics from HQ since there is no cost associated to adding a clinic.
			//Because of this, don't make extra web calls to check if HQ has tried to disable any clinics.
			XmlWriterSettings settings=new XmlWriterSettings();
			settings.Indent=true;
			settings.IndentChars=("    ");
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,settings)) {
				writer.WriteStartElement("ErxClinicAccessRequest");
				writer.WriteStartElement("RegistrationKey");
				writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));
				writer.WriteEndElement();//End reg key
				writer.WriteStartElement("RegKeyDisabledOverride");
				//Allow disabled regkeys to use eRx.  This functionality matches how we handle a disabled regkey for providererx
				//providererx in CustUpdates only cares that the regkey is valid and associated to a patnum in ODHQ
				writer.WriteString("true");
				writer.WriteEndElement();//End reg key disabled override
				foreach(ClinicErx clinicErx in listClinicErxsToSend) {
					writer.WriteStartElement("Clinic");
					writer.WriteAttributeString("ClinicDesc",clinicErx.ClinicDesc);
					writer.WriteAttributeString("EnabledStatus",((int)clinicErx.EnabledStatus).ToString());
					writer.WriteAttributeString("ClinicId",clinicErx.ClinicId);
					writer.WriteAttributeString("ClinicKey",clinicErx.ClinicKey);
					writer.WriteEndElement();//End Clinic
				}
				writer.WriteEndElement();//End ErxAccessRequest
			}
#if DEBUG
			OpenDentBusiness.localhost.Service1 updateService=new OpenDentBusiness.localhost.Service1();

#else
			OpenDentBusiness.customerUpdates.Service1 updateService=new OpenDentBusiness.customerUpdates.Service1();
			updateService.Url=PrefC.GetString(PrefName.UpdateServerAddress);
#endif
			try {
				string result=updateService.GetClinicErxAccess(strbuild.ToString());
				XmlDocument doc=new XmlDocument();
				doc.LoadXml(result);
				XmlNodeList listNodes=doc.SelectNodes("//Clinic");
				bool isCacheRefreshNeeded=false;
				for(int i=0;i<listNodes.Count;i++) {//Loop through clinics.
					XmlNode nodeClinic=listNodes[i];
					string clinicDesc="";
					string clinicId="";
					string clinicKey="";
					ErxStatus clinicEnabledStatus=ErxStatus.Disabled;
					for(int j=0;j<nodeClinic.Attributes.Count;j++) {//Loop through the attributes for the current provider.
						XmlAttribute attribute=nodeClinic.Attributes[j];
						if(attribute.Name=="ClinicDesc") {
							clinicDesc=attribute.Value;
						}
						else if(attribute.Name=="EnabledStatus") {
							clinicEnabledStatus=PIn.Enum<ErxStatus>(PIn.Int(attribute.Value));
						}
						else if(attribute.Name=="ClinicId") {
							clinicId=attribute.Value;
						}
						else if(attribute.Name=="ClinicKey") {
							clinicKey=attribute.Value;
						}
					}
					if(clinicDesc=="") {
						continue;
					}
					ClinicErx oldClinicErx=ClinicErxs.GetByClinicIdAndKey(clinicId,clinicKey);
					if(oldClinicErx==null) {
						continue;
					}
					ClinicErx clinicErxFromHqCur=oldClinicErx.Copy();
					clinicErxFromHqCur.EnabledStatus=clinicEnabledStatus;
					clinicErxFromHqCur.ClinicId=clinicId;
					clinicErxFromHqCur.ClinicKey=clinicKey;
					//Dont need to set the ErxType here because it's not something that can be changed by HQ.
					if(ClinicErxs.Update(clinicErxFromHqCur,oldClinicErx)) {
						isCacheRefreshNeeded=true;
					}
				}
				if(isCacheRefreshNeeded) {
					Cache.Refresh(InvalidType.ClinicErxs);
				}
			}
			catch(Exception ex) {
				ex.DoNothing();
				//Failed to contact server and/or update clinicerx row at ODHQ. We will simply use what we already know in the local database.
			}
		}

		public static UserOdPref GetDoseSpotUserIdFromPref(long userNum,long clinicNum) {
			Program programErx=Programs.GetCur(ProgramName.eRx);
			UserOdPref retVal=UserOdPrefs.GetByCompositeKey(userNum,programErx.ProgramNum,UserOdFkeyType.Program,clinicNum);
			if(clinicNum!=0 && retVal.IsNew || string.IsNullOrWhiteSpace(retVal.ValueString)) {
				retVal=UserOdPrefs.GetByCompositeKey(userNum,programErx.ProgramNum,UserOdFkeyType.Program,0);//Try the default userodpref if the clinic specific one is empty.
			}
			return retVal;
		}

		///<summary>Creates ClinicErx entries for every clinic that has DoseSpot ClinicID/ClinicKey values.
		///This will be used when sending the clinics to ODHQ for enabling/disabling.
		///This will create ClinicNum 0 entries, thus supporting offices without clinics enabled, as well as clinics using the "Headquarters" clinic.</summary>
		private static void MakeClinicErxsForDoseSpot() {
			long programNum=Programs.GetCur(ProgramName.eRx).ProgramNum;
			List<ProgramProperty> listClinicIDs=ProgramProperties.GetWhere(x => x.ProgramNum==programNum && x.PropertyDesc==Erx.PropertyDescs.ClinicID);
			List<ProgramProperty> listClinicKeys=ProgramProperties.GetWhere(x => x.ProgramNum==programNum && x.PropertyDesc==Erx.PropertyDescs.ClinicKey);
			bool isRefreshNeeded=false;
			foreach(ProgramProperty ppClinicId in listClinicIDs) {
				ProgramProperty ppClinicKey=listClinicKeys.FirstOrDefault(x => x.ClinicNum==ppClinicId.ClinicNum);
				if(ppClinicKey==null || string.IsNullOrWhiteSpace(ppClinicKey.PropertyValue)) {
					continue;
				}
				ClinicErx clinicErxCur=ClinicErxs.GetByClinicNum(ppClinicId.ClinicNum);
				if(clinicErxCur==null) {
					clinicErxCur=new ClinicErx();
					clinicErxCur.ClinicNum=ppClinicId.ClinicNum;
					clinicErxCur.ClinicId=ppClinicId.PropertyValue;
					clinicErxCur.ClinicKey=ppClinicKey.PropertyValue;
					clinicErxCur.ClinicDesc=Clinics.GetDesc(ppClinicId.ClinicNum);
					clinicErxCur.EnabledStatus=ErxStatus.PendingAccountId;
					ClinicErxs.Insert(clinicErxCur);
				}
				else {
					clinicErxCur.ClinicId=ppClinicId.PropertyValue;
					clinicErxCur.ClinicKey=ppClinicKey.PropertyValue;
					ClinicErxs.Update(clinicErxCur);
				}
				isRefreshNeeded=true;
			}
			if(isRefreshNeeded) {
				Cache.Refresh(InvalidType.ClinicErxs);
			}
		}

		///<summary>Throws exceptions if any of the clinic/practice info is invalid.
		///Ensures that the Clinic that is returned is a valid clinic that can turned 
		/// into a DoseSpot Clinic to be used for DoseSpot API calls that require a Clinic (like registering a new clinic). </summary>
		private static Clinic GetClinicOrPracticeInfo(long clinicNum) {
			Clinic clinicCur=Clinics.GetClinic(clinicNum);
			bool isPractice=false;
			if(clinicCur==null) {//Make a fake ClinicNum 0 clinic containing practice info for validation/registering a new clinician if needed.
				clinicCur=new Clinic();
				clinicCur.Abbr=PrefC.GetString(PrefName.PracticeTitle);
				clinicCur.Address=PrefC.GetString(PrefName.PracticeAddress);
				clinicCur.Address2=PrefC.GetString(PrefName.PracticeAddress2);
				clinicCur.City=PrefC.GetString(PrefName.PracticeCity);
				clinicCur.State=PrefC.GetString(PrefName.PracticeST);
				clinicCur.Zip=PrefC.GetString(PrefName.PracticeZip);
				clinicCur.Phone=PrefC.GetString(PrefName.PracticePhone);
				clinicCur.Fax=PrefC.GetString(PrefName.PracticeFax);
				isPractice=true;
			}
			ValidateClinic(clinicCur,isPractice);
			//At this point we know the clinic is valid since we did not throw an exception.
			return clinicCur;
		}

		///<summary>Inserts a new ProgramProperty into the database if the passed in ppCur is null.
		///If ppCur is not null, it just sets the PropertyValue and updates.</summary>
		private static void InsertOrUpdate(ProgramProperty ppCur,long programNum,string propDesc,string propValue,long clinicNum) {
			if(ppCur==null) {
				ppCur=new ProgramProperty();
				ppCur.ProgramNum=programNum;
				ppCur.PropertyDesc=propDesc;
				ppCur.PropertyValue=propValue;
				ppCur.ClinicNum=clinicNum;
				ProgramProperties.Insert(ppCur);
			}
			else {
				ppCur.PropertyValue=propValue;
				ProgramProperties.Update(ppCur);
			}
		}

		///<summary>Generates a DoseSpot clinic based on the passed in clinic.
		///The returned DoseSpot clinic does not have the ClinicID set.</summary>
		private static DoseSpotService.Clinic MakeDoseSpotClinic(Clinic clinic) {
			DoseSpotService.Clinic retVal=new DoseSpotService.Clinic();
			retVal.Address1=clinic.Address;
			retVal.Address2=clinic.Address2;
			retVal.City=clinic.City;
			retVal.ClinicName=clinic.Abbr;
			retVal.ClinicNameLongForm=clinic.Description;
			retVal.PrimaryFax=clinic.Fax;
			retVal.PrimaryPhone=clinic.Phone;
			//This is a required field but there is no way to set this value in Open Dental.
			//Since it's a clinic it seems safe to assume that it's a work number
			retVal.PrimaryPhoneType="Work";
			retVal.State=clinic.State.ToUpper();
			retVal.ZipCode=clinic.Zip;
			return retVal;
		}

		///<summary>Makes a clinician out of the tables in OD that make up what DoseSpot considers a clinician.
		///Clinic in this instance can also be a fake ClinicNum 0 clinic that contains practice information.
		///When an employee is the reason for making a clinician, isProxyClinician must be set to true.
		///If isProxyClinician is incorrectly set, the employee will have access to send prescriptions in DoseSpot.</summary>
		private static DoseSpotService.Clinician MakeDoseSpotClinician(Provider prov,Clinic clinic,string emailAddress,bool isProxyClinician) {
			DoseSpotService.Clinician retVal=new DoseSpotService.Clinician();
			retVal.Address1=clinic.Address;
			retVal.Address2=clinic.Address2;
			retVal.City=clinic.City;
			retVal.DateOfBirth=prov.Birthdate;
			retVal.DEANumber=ProviderClinics.GetDEANum(prov.ProvNum,clinic.ClinicNum);//
			retVal.Email=emailAddress;//Email should have been validated by now.
			retVal.FirstName=prov.FName;
			retVal.Gender="Unknown";//This is a required field but we do not store this information.
			retVal.IsProxyClinician=isProxyClinician;
			//retVal.IsReportingClinician=false;//This field was not present in the API documentation and weren't required when testing.
			retVal.LastName=prov.LName;
			retVal.MiddleName=prov.MI;
			retVal.NPINumber=prov.NationalProvID;
			retVal.PrimaryFax=clinic.Fax;
			retVal.PrimaryPhone=clinic.Phone;
			//This is a required field but there is no way to set this value in Open Dental.
			//Since it's a clinic phone number it seems safe to assume that it's a work number
			retVal.PrimaryPhoneType="Work";
			retVal.State=clinic.State.ToUpper();
			retVal.Suffix=prov.Suffix;
			retVal.ZipCode=clinic.Zip;
			return retVal;
		}

		///<summary>Adds a query parameter and that parameter's value to the string builder</summary>
		private static StringBuilder QueryStringAddParameter(StringBuilder queryString,string paramName,string paramValue) {
			queryString.Append("&"+paramName+"=");
			if(paramName!=null) {
				queryString.Append(Uri.EscapeDataString(paramValue));
			}
			return queryString;
		}

		///<summary>Generates a SingleSignOn to use for DoseSpot API calls.</summary>
		private static DoseSpotService.SingleSignOn GetSingleSignOn(string clinicID,string clinicKey,string userID,bool isQueryString) {
			string ssoCode=CreateSsoCode(clinicKey,isQueryString);
			string ssoUserIdVerify=CreateSsoUserIdVerify(clinicKey,userID,isQueryString);
			DoseSpotService.SingleSignOn retVal=new DoseSpotService.SingleSignOn();
			retVal.SingleSignOnClinicId=PIn.Int(clinicID);
			retVal.SingleSignOnUserId=PIn.Int(userID);
			retVal.SingleSignOnPhraseLength=32;
			retVal.SingleSignOnCode=ssoCode;
			retVal.SingleSignOnUserIdVerify=ssoUserIdVerify;
			return retVal;
		}

		///<summary>Throws exceptions if anything about the provider is invalid.
		///Not throwing an exception means that the provider is valid</summary>
		public static void ValidateProvider(Provider prov,long clinicNum=0) {
			//No need to check RemotingRole; no call to db.
			if(prov==null) {
				throw new Exception("Invalid provider.");
			}
			StringBuilder sbErrors=new StringBuilder();
			if(!prov.IsErxEnabled) {
				sbErrors.AppendLine(Lans.g("DoseSpot","Erx is disabled for provider.  "
					+"To enable, edit provider in Lists | Providers and acknowledge Electronic Prescription fees."));
			}
			if(prov.IsHidden) {
				sbErrors.AppendLine(Lans.g("DoseSpot","Provider is hidden"));
			}
			if(prov.IsNotPerson) {
				sbErrors.AppendLine(Lans.g("DoseSpot","Provider must be a person"));
			}
			string fname=prov.FName.Trim();
			if(fname=="") {
				sbErrors.AppendLine(Lans.g("DoseSpot","First name missing"));
			}
			if(Regex.Replace(fname,"[^A-Za-z\\- ]*","")!=fname) {
				sbErrors.AppendLine(Lans.g("DoseSpot","First name can only contain letters, dashes, or spaces"));
			}
			string lname=prov.LName.Trim();
			if(lname=="") {
				sbErrors.AppendLine(Lans.g("DoseSpot","Last name missing"));
			}
			string deaNum=ProviderClinics.GetDEANum(prov.ProvNum,clinicNum);
			if(deaNum.ToLower()!="none" && !Regex.IsMatch(deaNum,"^[A-Za-z]{2}[0-9]{7}$")) {
				sbErrors.AppendLine(Lans.g("DoseSpot","Provider DEA Number must be 2 letters followed by 7 digits.  If no DEA Number, enter NONE."));
			}
			string npi=Regex.Replace(prov.NationalProvID,"[^0-9]*","");//NPI with all non-numeric characters removed.
			if(npi.Length!=10) {
				sbErrors.AppendLine(Lans.g("DoseSpot","NPI must be exactly 10 digits"));
			}
			if(prov.StateLicense=="") {
				sbErrors.AppendLine(Lans.g("DoseSpot","State license missing"));
			}
			if(Erx.StateCodes.IndexOf(prov.StateWhereLicensed.ToUpper())<0) {
				sbErrors.AppendLine(Lans.g("DoseSpot","State where licensed invalid"));
			}
			if(prov.Birthdate.Year<1880) {
				sbErrors.AppendLine(Lans.g("DoseSpot","Birthdate invalid"));
			}
			if(sbErrors.ToString().Length>0) {
				throw new ODException(Lans.g("DoseSpot","Issues found for provider")+" "+prov.Abbr+":\r\n"+sbErrors.ToString());
			}
		}

		///<summary>Throws exceptions if anything about the clinic is invalid.
		///Not throwing an exception means that the clinic is valid.
		///In the case that the user is validating clinicNum 0 or doesn't have clinics enabled, the clinic here has 
		/// practice information in it.</summary>
		private static void ValidateClinic(Clinic clinic,bool isPractice = false) {
			if(clinic==null) {
				throw new Exception(Lans.g("DoseSpot","Invalid "+(isPractice ? "practice info." : "clinic.")));
			}
			StringBuilder sbErrors=new StringBuilder();
			if(clinic.IsHidden) {
				sbErrors.AppendLine(Lans.g("DoseSpot","Clinic is hidden"));
			}
			if(string.IsNullOrWhiteSpace(clinic.Phone)) {
				sbErrors.AppendLine(Lans.g("DoseSpot","Phone number is blank"));
			}
			else if(!IsPhoneNumberValid(clinic.Phone)) {//If the phone number isn't valid, DoseSpot will break.
				sbErrors.AppendLine(Lans.g("DoseSpot","Phone number invalid: ")+clinic.Phone);
			}
			if(string.IsNullOrWhiteSpace(clinic.Fax)) {
				sbErrors.AppendLine(Lans.g("DoseSpot","Fax number is blank"));
			}
			else if(!IsPhoneNumberValid(clinic.Fax)) {//If the fax number isn't valid, DoseSpot will break.
				sbErrors.AppendLine(Lans.g("DoseSpot","Fax number invalid: ")+clinic.Fax);
			}
			if(clinic.Address=="") {
				sbErrors.AppendLine(Lans.g("DoseSpot","Address is blank"));
			}
			if(Regex.IsMatch(clinic.Address,".*P\\.?O\\.? .*",RegexOptions.IgnoreCase)) {
				sbErrors.AppendLine(Lans.g("DoseSpot","Address cannot be a PO BOX"));
			}
			if(clinic.City=="") {
				sbErrors.AppendLine(Lans.g("DoseSpot","City is blank"));
			}
			if(string.IsNullOrWhiteSpace(clinic.State)) {
				sbErrors.AppendLine(Lans.g("DoseSpot","State abbreviation is blank"));
			}
			else if(clinic.State.Length<=2 && (clinic.State=="" || (clinic.State!="" && Erx.StateCodes.IndexOf(clinic.State.ToUpper())<0))) {
				//Don't validate state values that are longer than 2 characters.
				sbErrors.AppendLine(Lans.g("DoseSpot","State abbreviation is invalid"));
			}
			if(clinic.Zip=="" && !Regex.IsMatch(clinic.Zip,@"^[0-9]{5}\-?([0-9]{4})?$")) {//Blank, or #####, or #####-####, or #########
				sbErrors.AppendLine(Lans.g("DoseSpot","Zip invalid."));
			}
			if(sbErrors.ToString().Length>0) {
				if(isPractice) {
					throw new Exception(Lans.g("DoseSpot","Issues found for practice information:")+"\r\n"+sbErrors.ToString());
				}
				throw new Exception(Lans.g("DoseSpot","Issues found for clinic")+" "+clinic.Abbr+":\r\n"+sbErrors.ToString());
			}
		}

		///<summary>Throws exceptions for invalid Patient data.</summary>
		private static string GetPatientQueryString(Patient pat) {
			StringBuilder sb=new StringBuilder();
			StringBuilder sbErrors=new StringBuilder();
			string phoneType="";
			string primaryPhone=GetPhoneAndType(pat,0,out phoneType);
			//Validate patient data
			if(pat.FName=="") {
				sbErrors.AppendLine(Lans.g("DoseSpot","Missing first name."));
			}
			if(pat.LName=="") {
				sbErrors.AppendLine(Lans.g("DoseSpot","Missing last name."));
			}
			if(pat.Birthdate.Year<1880) {
				sbErrors.AppendLine(Lans.g("DoseSpot","Missing birthdate."));
			}
			if(pat.Birthdate>DateTime.Today) {
				sbErrors.AppendLine(Lans.g("DoseSpot","Invalid birthdate."));
			}
			if(pat.Address.Length==0) {
				sbErrors.AppendLine(Lans.g("DoseSpot","Missing address."));
			}
			if(pat.City.Length<2) {
				sbErrors.AppendLine(Lans.g("DoseSpot","Invalid city."));
			}
			if(string.IsNullOrWhiteSpace(pat.State)) {
				sbErrors.AppendLine(Lans.g("DoseSpot","Blank state abbreviation."));
			}
			else if(pat.State.Length<=2 && Erx.StateCodes.IndexOf(pat.State)<0) {//Don't validate state values that are longer than 2 characters.
				sbErrors.AppendLine(Lans.g("DoseSpot","Invalid state abbreviation."));
			}
			if(string.IsNullOrWhiteSpace(pat.Zip)) {
				sbErrors.AppendLine(Lans.g("DoseSpot","Blank zip."));
			}
			else if(!Regex.IsMatch(pat.Zip,@"([0-9]{9})|([0-9]{5}-[0-9]{4})|([0-9]{5})")) {//Regular expression taken from DoseSpot test app
				sbErrors.AppendLine(Lans.g("DoseSpot","Invalid zip."));
			}
			if(!IsPhoneNumberValid(primaryPhone)) {//If the primary phone number isn't valid, DoseSpot will break.
				sbErrors.AppendLine(Lans.g("DoseSpot","Invalid phone number: ")+primaryPhone);
			}
			if(sbErrors.ToString().Length>0) {
				throw new Exception(Lans.g("DoseSpot","Issues found for current patient:")+"\r\n"+sbErrors.ToString());
			}
			OIDExternal oidExternalPat=DoseSpot.GetDoseSpotPatID(pat.PatNum);
			if(oidExternalPat!=null) {
				sb=QueryStringAddParameter(sb,"PatientId",oidExternalPat.IDExternal);
			}
			sb=QueryStringAddParameter(sb,"Prefix",Tidy(pat.Title,10));
			sb=QueryStringAddParameter(sb,"FirstName",Tidy(pat.FName,35));
			sb=QueryStringAddParameter(sb,"MiddleName",Tidy(pat.MiddleI,35));
			sb=QueryStringAddParameter(sb,"LastName",Tidy(pat.LName,35));
			sb=QueryStringAddParameter(sb,"Suffix","");//I don't see where we store suffixes.
			sb=QueryStringAddParameter(sb,"DateOfBirth",pat.Birthdate.ToShortDateString());
			sb=QueryStringAddParameter(sb,"Gender",pat.Gender.ToString());
			sb=QueryStringAddParameter(sb,"Address1",Tidy(pat.Address,35));
			sb=QueryStringAddParameter(sb,"Address2",Tidy(pat.Address2,35));
			sb=QueryStringAddParameter(sb,"City",Tidy(pat.City,35));
			sb=QueryStringAddParameter(sb,"State",pat.State);
			sb=QueryStringAddParameter(sb,"ZipCode",pat.Zip);
			sb=QueryStringAddParameter(sb,"PrimaryPhone",primaryPhone);
			sb=QueryStringAddParameter(sb,"PrimaryPhoneType",phoneType);
			sb=QueryStringAddParameter(sb,"PhoneAdditional1",GetPhoneAndType(pat,1,out phoneType));
			sb=QueryStringAddParameter(sb,"PhoneAdditionalType1",phoneType);
			sb=QueryStringAddParameter(sb,"PhoneAdditional2",GetPhoneAndType(pat,2,out phoneType));
			sb=QueryStringAddParameter(sb,"PhoneAdditionalType2",phoneType);
			//sb=QueryStringAddParameter(sb,"Height",Height);
			//sb=QueryStringAddParameter(sb,"Weight",Weight);
			//sb=QueryStringAddParameter(sb,"HeightMetric",HeightMetric);
			//sb=QueryStringAddParameter(sb,"WeightMetric",WeightMetric);
			return sb.ToString();
		}

		private static string Tidy(string value,int length) {
			if(value.Length<=length) {
				return value;
			}
			return value.Substring(0,length);
		}

		///<summary>Valid values for ordinal are 0 (for primary),1, or 2.</summary>
		private static string GetPhoneAndType(Patient pat,int ordinal,out string phoneType) {
			List<string> listTypes=new List<string>();
			List<string> listPhones=new List<string>();
			if(pat.HmPhone!="") {
				listTypes.Add("Home");
				listPhones.Add(pat.HmPhone);
			}
			if(pat.WirelessPhone!="") {
				listTypes.Add("Cell");
				listPhones.Add(pat.WirelessPhone);
			}
			if(pat.WkPhone!="") {
				listTypes.Add("Work");
				listPhones.Add(pat.WkPhone);
			}
			if(ordinal >= listPhones.Count) {
				phoneType="";
				return "";
			}
			phoneType=listTypes[ordinal];
			string retVal=listPhones[ordinal].Replace("(","").Replace(")","").Replace("-","");//remove all formatting as DoseSpot doesn't allow it.
			if(retVal.Length==11 && retVal[0]=='1') {
				retVal=retVal.Substring(1);//Remove leading 1 from phone number since DoseSpot thinks that invalid.
			}
			return retVal;
		}

		private static bool IsPhoneNumberValid(string phoneNumber) {
			string patternPhone=@"^1?\s*-?\s*(\d{3}|\(\s*\d{3}\s*\))\s*-?\s*\d{3}\s*-?\s*\d{4}(X\d{0,9})?";
			Regex pattern=new Regex(patternPhone, RegexOptions.IgnoreCase);
			if(phoneNumber!=null) {
				phoneNumber=phoneNumber.Trim();
			}
			if(string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length>=35) {//Max length of 35 is what the DoseSpot example app checks for, there is no documentation supporting it.
				return false;
			}
			if(!pattern.IsMatch(phoneNumber)) {//The regex was taken directly from the DoseSpot example app
				return false;
			}
			if(!CheckAreaCode(phoneNumber)) {
				return false;
			}
			return true;
		}

		private static bool CheckAreaCode(string phoneNumber) {
			if(string.IsNullOrWhiteSpace(phoneNumber)) {
				return false;
			}
			Regex re=new Regex(@"/[^\d]/");
			phoneNumber=re.Replace(phoneNumber,"");
			string areaCode="";
			if(phoneNumber.Length<=3) {
				return false;
			}
			if(phoneNumber.Substring(0,1)=="1") {//Remove leading 1 for USA.
				areaCode=phoneNumber.Substring(1,3);
			}
			else {
				areaCode=phoneNumber.Substring(0,3);
			}
			//Extremely long list of valid area codes provided by DoseSpot.
			List<string> areaCodeList=new List<string>() { "201","202","203","204","205","206","207","208","209","210","212","213","214","215","216"
				,"217","218","219","224","225","226","228","229","231","234","239","240","242","246","248","250","251","252","253","254","256","260"
				,"262","264","267","268","269","270","276","281","284","289","301","302","303","304","305","306","307","308","309","310","312","313"
				,"314","315","316","317","318","319","320","321","323","325","330","331","334","336","337","339","340","343","345","347","351","352"
				,"360","361","385","386","401","402","403","404","405","406","407","408","409","410","412","413","414","415","416","417","418","419"
				,"423","424","425","430","432","434","435","438","440","441","442","443","450","456","458","469","470","473","475","478","479","480"
				,"484","500","501","502","503","504","505","506","507","508","509","510","512","513","514","515","516","517","518","519","520","530"
				,"533","534","539","540","541","551","559","561","562","563","567","570","571","573","574","575","579","580","581","585","586","587"
				,"600","601","602","603","604","605","606","607","608","609","610","612","613","614","615","616","617","618","619","620","623","626"
				,"630","631","636","641","646","647","649","650","651","657","660","661","662","664","670","671","678","681","682","684","700","701"
				,"702","703","704","705","706","707","708","709","710","712","713","714","715","716","717","718","719","720","724","727","731","732"
				,"734","740","747","754","757","758","760","762","763","765","767","769","770","772","773","774","775","778","779","780","781","784"
				,"785","786","787","800","801","802","803","804","805","806","807","808","809","810","812","813","814","815","816","817","818","819"
				,"828","829","830","831","832","843","845","847","848","849","850","855","856","857","858","859","860","862","863","864","865","866"
				,"867","868","869","870","872","876","877","878","888","900","901","902","903","904","905","906","907","908","909","910","912","913"
				,"914","915","916","917","918","919","920","925","928","929","931","936","937","938","939","940","941","947","949","951","952","954"
				,"956","970","971","972","973","978","979","980","985","989" };
			if(areaCodeList.Contains(areaCode)) {
				return true;
			}
			return false;
		}

		///<summary>This phrase is a fallback incase for some reason the implementor didn't pass in a phrase.
		///This is not recommended by DoseSpot because they will detect that the same phrase is being used for multiple requests</summary>
		private static string Get32CharPhrase() {
			if(_randomPhrase32==null) {
				_randomPhrase32=MiscUtils.CreateRandomAlphaNumericString(32);
			}
			return _randomPhrase32;
		}

		private static byte[] GetBytesFromUTF8(string val) {
			return new UTF8Encoding().GetBytes(val);//Get the value in Bytes from UTF8 String
		}

		private static byte[] GetSHA512Hash(byte[] arrayBytesToHash) {
			byte[] arrayHash;
			using(SHA512 shaM=new SHA512Managed()) {//Use SHA512 to hash the byte value you just received
				arrayHash=shaM.ComputeHash(arrayBytesToHash);
			}
			return arrayHash;
		}

		private static string RemoveExtraEqualSigns(string str) {
			if(str.EndsWith("==")) {//If there are two = signs at the end, then remove them
				str=str.Substring(0,str.Length-2);
			}
			return str;
		}
	}
}
