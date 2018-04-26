using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CodeBase;
using System.Text;

namespace OpenDentBusiness {
	
	///<summary></summary>
	public class Patients {
		#region Get Methods
		///<summary>Returns a list of all potential clones for the patient passed in.  The list returned will always contain the patNum passed in.
		///It is okay for patNum passed in to be a clone, a master, or even a patient that is not even related to clones at all.</summary>
		public static List<long> GetClonePatNumsAll(long patNum) {
			//No need to check RemotingRole; no call to db.
			long patNumOriginal=patNum;
			//Figure out if the patNum passed in is in fact the original patient and if it isn't, go get it from the database.
			if(PatientLinks.IsPatientAClone(patNum)) {
				patNumOriginal=PatientLinks.GetOriginalPatNumFromClone(patNum);
			}
			return PatientLinks.GetPatNumsLinkedFromRecursive(patNumOriginal,PatientLinkType.Clone);
		}

		///<summary>Gets all potential clones and their corresponding specialty for the patient passed in.
		///Even if the patNum passed in is itself a clone, all clones for the clone's master will still get returned.
		///The returned dictionary will always contain the master patient so that it can be displayed to the user if desired.
		///Specialties are only important if clinics are enabled.  If clinics are disabled then the corresponding Def will be null.</summary>
		public static SerializableDictionary<Patient,Def> GetClonesAndSpecialties(long patNum) {
			//No need to check RemotingRole; no call to db.
			return GetClonesAndSpecialtiesForPatients(GetClonePatNumsAll(patNum));
		}

		///<summary>Gets all potential clones and their corresponding specialty for the original patients passed in.
		///The returned dictionary will always contain the master patient so that it can be displayed to the user if desired.
		///Specialties are only important if clinics are enabled.  If clinics are disabled then the corresponding Def will be null.</summary>
		public static SerializableDictionary<Patient,Def> GetClonesAndSpecialtiesForPatients(List<long> listPatNums) {
			//No need to check RemotingRole; no call to db.
			//Get every single patientlink possible whether the patNum passed in was the master patient or the clone patient.
			SerializableDictionary<Patient,Def> dictCloneSpecialty=new SerializableDictionary<Patient, Def>();
			if(listPatNums==null || listPatNums.Count==0) {
				return dictCloneSpecialty;//No clones found.
			}
			//Get all of the clones.
			Patient[] arrayPatientClones=Patients.GetMultPats(listPatNums.Distinct().ToList());
			if(arrayPatientClones==null || arrayPatientClones.Length==0) {
				return dictCloneSpecialty;//No patients for clone links found.
			}
			List<DefLink> listPatDefLink=DefLinks.GetDefLinksByType(DefLinkType.Patient);
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.ClinicSpecialty);
			foreach(Patient clone in arrayPatientClones) {
				DefLink defLink=listPatDefLink.FirstOrDefault(x => x.FKey==clone.PatNum);
				Def specialty=null;
				if(defLink!=null) {
					specialty=Defs.GetDef(DefCat.ClinicSpecialty,defLink.DefNum,listDefs);//Can return null which is fine.
				}
				dictCloneSpecialty[clone]=specialty;
			}
			return dictCloneSpecialty;
		}
		
		///<summary>Returns the master or original patient for the clone passed in otherwise returns the patient passed in if patCur is not a clone.
		///Will return null if the patCur is a clone but the master or original patient could not be found in the database.</summary>
		public static Patient GetOriginalPatientForClone(Patient patCur) {
			//No need to check RemotingRole; no call to db.
			if(patCur==null || !IsPatientAClone(patCur.PatNum)) {
				return patCur;
			}
			//Go get the master or original patient from the database for the clone patient passed in.
			return GetPat(PatientLinks.GetOriginalPatNumFromClone(patCur.PatNum));
		}

		///<summary>Be careful with what you pass in as wirelessNumber. If you pass in '1', you will get almost every patient.</summary>
		public static List<Patient> GetPatientsByWirelessPhone(string wirelessNumber) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),wirelessNumber);
			}
			string phoneRegex=string.Join("[^0-9]*",wirelessNumber.Where(x=>char.IsDigit(x)));//So that any intervening non-digit characters will still match
			string command=@"SELECT *
				FROM patient 
				WHERE patient.WirelessPhone REGEXP '"+POut.String(phoneRegex)+@"' 
				AND patient.PatStatus!="+POut.Int((int)PatientStatus.Deleted);
			return Crud.PatientCrud.SelectMany(command);
		}

		public static List<PatAging> GetAgingList() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PatAging>>(MethodBase.GetCurrentMethod());
			}
			long collectionBillType=Defs.GetDefsForCategory(DefCat.BillingTypes,true).FirstOrDefault(x => x.ItemValue.ToLower()=="c")?.DefNum??0;
			string command="SELECT guar.PatNum,guar.Guarantor,guar.Bal_0_30,guar.Bal_31_60,guar.Bal_61_90,guar.BalOver90,guar.BalTotal,guar.InsEst,"
				+"guar.BalTotal-guar.InsEst AS $pat,guar.PayPlanDue,guar.LName,guar.FName,guar.Preferred,guar.MiddleI,guar.PriProv,guar.BillingType,"
				+"guar.ClinicNum,guar.Zip,COALESCE(guarPay.DateLastPay,'0001-01-01') DateLastPay "
				+"FROM patient guar "
				+"LEFT JOIN ("
					+"SELECT p.Guarantor,MAX(ps.DatePay) DateLastPay "
					+"FROM paysplit ps "
					+"INNER JOIN patient p ON p.PatNum=ps.PatNum "
					+"WHERE ps.SplitAmt != 0 "
					+"GROUP BY p.Guarantor"
				+") guarPay ON guar.PatNum=guarPay.Guarantor "
				+"WHERE guar.PatNum=guar.Guarantor "
				+"AND guar.PatStatus != "+POut.Int((int)PatientStatus.Deleted)+" "
				+"AND ("
					+"((guar.PatStatus!="+POut.Int((int)PatientStatus.Archived)+" OR ROUND(guar.BalTotal,3)!=0) "//Hide archived patients with PatBal=0.
						+"AND (guar.Bal_0_30  > 0.005 OR guar.Bal_31_60 > 0.005 OR guar.Bal_61_90 > 0.005 OR guar.BalOver90 > 0.005)) "
					+(collectionBillType>0?("OR guar.BillingType = "+POut.Long(collectionBillType)+" "):"")+") "
				+"ORDER BY guar.Lname,guar.FName";
			DataTable table=Db.GetTable(command);
			List<PatAging> agingList=new List<PatAging>();
			if(table.Rows.Count<1) {
				return agingList;
			}
			List<long> listAllPatNums=table.Select().Select(x => PIn.Long(x["PatNum"].ToString())).ToList();
			List<TsiTransLog> listAllLogs=TsiTransLogs.SelectMany(listAllPatNums);
			Dictionary<long,List<TsiTransLog>> dictPatNumListTrans=new Dictionary<long,List<TsiTransLog>>();
			if(listAllLogs.Count>0) {
				dictPatNumListTrans=listAllLogs.GroupBy(x => x.PatNum).ToDictionary(x => x.Key,x => x.OrderByDescending(y => y.TransDateTime).ToList());
			}
			string patNumString=string.Join(",",listAllPatNums.Select(x => POut.Long(x)));//guaranteed to not be empty
			command="SELECT DISTINCT patient.Guarantor "
				+"FROM patient "
				+"INNER JOIN claim ON patient.PatNum=claim.PatNum "
					+"AND claim.ClaimStatus IN ('U','H','W','S') "
					+"AND claim.ClaimType IN ('P','S','Other') "
				+"WHERE patient.Guarantor IN ("+patNumString+")";
			List<long> listGuarNumsInsPending=Db.GetListLong(command);
			command="SELECT DISTINCT patient.Guarantor "
				+"FROM patient "
				+"WHERE EXISTS ("
					+"SELECT * FROM procedurelog "
					+"INNER JOIN claimproc ON claimproc.ProcNum=procedurelog.ProcNum "
						+"AND claimproc.NoBillIns=0 "
						+"AND claimproc.Status="+POut.Int((int)ClaimProcStatus.Estimate)+" "
					+"WHERE procedurelog.PatNum=patient.PatNum "
					+"AND procedurelog.ProcFee>0 "
					+"AND procedurelog.ProcStatus="+POut.Int((int)ProcStat.C)+" "
					+"AND procedurelog.ProcDate>"+POut.Date(DateTime.Today.AddMonths(-6))
				+") "
				+"AND patient.Guarantor IN ("+patNumString+")";
			List<long> listGuarNumsUnsentProcs=Db.GetListLong(command);
			PatAging patage;
			foreach(DataRow rowCur in table.Rows) {
				patage=new PatAging();
				patage.PatNum=PIn.Long(rowCur["PatNum"].ToString());
				patage.Guarantor=PIn.Long(rowCur["Guarantor"].ToString());
				patage.Bal_0_30=PIn.Double(rowCur["Bal_0_30"].ToString());
				patage.Bal_31_60=PIn.Double(rowCur["Bal_31_60"].ToString());
				patage.Bal_61_90=PIn.Double(rowCur["Bal_61_90"].ToString());
				patage.BalOver90=PIn.Double(rowCur["BalOver90"].ToString());
				patage.BalTotal=PIn.Double(rowCur["BalTotal"].ToString());
				patage.InsEst=PIn.Double(rowCur["InsEst"].ToString());
				patage.AmountDue=PIn.Double(rowCur["$pat"].ToString());
				patage.PayPlanDue=PIn.Double(rowCur["PayPlanDue"].ToString());
				patage.PatName=Patients.GetNameLF(PIn.String(rowCur["LName"].ToString()),PIn.String(rowCur["FName"].ToString()),
					PIn.String(rowCur["Preferred"].ToString()),PIn.String(rowCur["MiddleI"].ToString()));
				patage.PriProv=PIn.Long(rowCur["PriProv"].ToString());
				patage.BillingType=PIn.Long(rowCur["BillingType"].ToString());
				patage.ClinicNum=PIn.Long(rowCur["ClinicNum"].ToString());
				patage.Zip=PIn.String(rowCur["Zip"].ToString());
				patage.DateLastPay=PIn.Date(rowCur["DateLastPay"].ToString());
				patage.HasInsPending=listGuarNumsInsPending.Contains(patage.PatNum);
				patage.HasUnsentProcs=listGuarNumsUnsentProcs.Contains(patage.PatNum);
				if(!dictPatNumListTrans.TryGetValue(patage.PatNum,out patage.ListTsiLogs)) {
					patage.ListTsiLogs=new List<TsiTransLog>();
				}
				agingList.Add(patage);
			}
			return agingList;
		}

		///<summary>Used by the OpenDentalService Transworld thread to sync accounts sent to collection.</summary>
		public static List<long> GetListCollectionGuarNums() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod());
			}
			List<Def> listBillTypes=Defs.GetDefsForCategory(DefCat.BillingTypes,true).FindAll(x => x.ItemValue.ToLower()=="c");
			List<long> listSuspendedGuarNums=TsiTransLogs.GetSuspendedGuarNums();
			if(listBillTypes.Count==0) {
				return listSuspendedGuarNums;//no collection billing type, return suspended guar nums, could be empty
			}
			string command="SELECT patient.Guarantor "
				+"FROM patient "
				+"WHERE patient.PatNum=patient.Guarantor "
				+"AND patient.BillingType IN ("+string.Join(",",listBillTypes.Select(x => POut.Long(x.DefNum)))+")";
			return Db.GetListLong(command).Union(listSuspendedGuarNums).ToList();
		}

		///<summary>Used to determine whether or not the guarantor of a family is sent to collections.  Used in order to prompt the user to specify
		///whether the payment or adjustment being entered on a collection patient came from Transworld and therefore shouldn't be sent to Transworld.</summary>
		public static bool IsGuarCollections(long guarNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),guarNum);
			}
			if(TsiTransLogs.IsGuarSuspended(guarNum)) {
				return true;
			}
			Def billTypeColl=Defs.GetDefsForCategory(DefCat.BillingTypes,true).FirstOrDefault(x => x.ItemValue.ToLower()=="c");
			if(billTypeColl==null) {
				return false;//if not suspended and no billing type marked as collection billing type, return false, guar not a collection guar
			}
			string command="SELECT 1 isGuarCollection "
				+"FROM patient "
				+"WHERE PatNum="+POut.Long(guarNum)+" "
				+"AND PatNum=Guarantor "
				+"AND BillingType="+POut.Long(billTypeColl.DefNum)+" "
				+DbHelper.LimitAnd(1);
			return PIn.Bool(Db.GetScalar(command));
		}

		#endregion

		#region Modification Methods

		#region Insert
		///<summary>Creates a clone from the patient passed in and then links them together as master and clone via the patientlink table.
		///After the patient has been cloned successfully, this method will call SynchCloneWithPatient().
		///That synch method will take care of synching all fields that should be synched for a brand new clone.</summary>
		public static Patient CreateCloneAndSynch(Patient patient,Family familyCur=null,List<InsPlan> listInsPlans=null,List<InsSub> listInsSubs=null
			,List<Benefit> listBenefits=null,long primaryProvNum=0,long clinicNum=0)
		{
			//No need to check RemotingRole; no call to db.
			Patient patientSynch=CreateClone(patient,primaryProvNum,clinicNum);
			SynchCloneWithPatient(patient,patientSynch,familyCur,listInsPlans,listInsSubs,listBenefits);
			return patientSynch;
		}

		///<summary>Creates a clone from the patient passed in and then links them together as master and clone via the patientlink table.
		///This method only sets a few crucial variables on the patient clone returned.  Call any additional synch methods afterwards.
		///The clone that was created will be returned.  Optionally pass in a primary provider and / or clinic that should be used.</summary>
		public static Patient CreateClone(Patient patient,long primaryProvNum=0,long clinicNum=0) {
			//No need to check RemotingRole; no call to db.
			Patient patientSynch=new Patient();
			patientSynch.LName=patient.LName.ToUpper();
			patientSynch.FName=patient.FName.ToUpper();
			patientSynch.MiddleI=patient.MiddleI.ToUpper();
			patientSynch.Birthdate=patient.Birthdate;
			//PriPro is intentionally not synched so the clone can be assigned to a different provider for tracking production.
			if(primaryProvNum==0) {
				primaryProvNum=PrefC.GetLong(PrefName.PracticeDefaultProv);
			}
			patientSynch.PriProv=primaryProvNum;
			patientSynch.ClinicNum=clinicNum;
			Patients.Insert(patientSynch,false);
			SecurityLogs.MakeLogEntry(Permissions.PatientCreate,patientSynch.PatNum,Lans.g("ContrFamily","Created from Family Module Clones Add button."));
			PatientLinks.Insert(new PatientLink() {
				PatNumFrom=patient.PatNum,
				PatNumTo=patientSynch.PatNum,
				LinkType=PatientLinkType.Clone,
			});
			#region Family / Super Family
			//Go get the clone from the database so that fields will be refreshed to their non-null values, i.e. '' instead of null
			patientSynch=Patients.GetPat(patientSynch.PatNum);
			Patient patientSynchOld=patientSynch.Copy();
			//Now that the clone has been inserted and has a primary key we can consider what family and/or super family the clone should be part of.
			if(PrefC.GetBool(PrefName.CloneCreateSuperFamily)) {
				//Put the clone into their own family.
				patientSynch.Guarantor=patientSynch.PatNum;
				//But then put the clone into the super family of the master (creating one if the master isn't already part of a super family).
				long superFamilyNum=patient.SuperFamily;
				if(superFamilyNum < 1) {
					//Forcefully create a new super family, make the master patient the super family head, and then put the clone into that super family.
					Patients.AssignToSuperfamily(patient.Guarantor,patient.Guarantor);//Moves other family members into the super family as well.
					superFamilyNum=patient.Guarantor;
				}
				//Do the guts of what AssignToSuperfamily() would have done but for our patientSynch object so that we save a db call.
				patientSynch.HasSuperBilling=true;
				patientSynch.SuperFamily=superFamilyNum;
			}
			else {
				//The preference to force using super families is off so we will only put the clone into a super family if the original is part of one.
				patientSynch.Guarantor=patient.Guarantor;
				patientSynch.SuperFamily=patient.SuperFamily;
			}
			//Save any family or super family changes to the database.  Other family members would have already been affected by this point.
			Update(patientSynch,patientSynchOld);
			#endregion
			return patientSynch;
		}
		#endregion

		#region Update
		///<summary>Synchs all clones related to the patient passed in with it's current information.  Returns a string representing what happened.
		///Optionally pass in the lists of insurance information to save db calls within a loop.</summary>
		public static string SynchClonesWithPatient(Patient patient,Family familyCur=null,List<InsPlan> listInsPlans=null
			,List<InsSub> listInsSubs=null,List<Benefit> listBenefits=null,List<PatPlan> listPatPlans=null) 
		{
			//No need to check RemotingRole; no call to db.
			StringBuilder stringBuilder=new StringBuilder();
			//Get all clones for the patient passed in and then synch each one and return a string regarding what happened during the synch.
			long patNumOriginal=patient.PatNum;
			//Figure out if the patNum passed in is in fact the original patient and if it isn't, go get it from the database.
			if(PatientLinks.IsPatientAClone(patient.PatNum)) {
				patNumOriginal=PatientLinks.GetOriginalPatNumFromClone(patient.PatNum);
			}
			//Now that we know the PatNum of the original or master patient we can get all corresponding clones.
			List<long> listPatNumClones=PatientLinks.GetPatNumsLinkedFromRecursive(patNumOriginal,PatientLinkType.Clone);
			//We now have every single clone PatNum but need to remove the one that is associated to patient so that we don't synch it.
			listPatNumClones.RemoveAll(x => x==patient.PatNum);
			Patient[] arraySynchPatients=GetMultPats(listPatNumClones);
			//Loop through all remaining clones and synch them with the patient that was passed in.
			foreach(Patient patientSynch in arraySynchPatients) {
				string changes=SynchCloneWithPatient(patient,patientSynch,familyCur,listInsPlans,listInsSubs,listBenefits,listPatPlans);
				if(!string.IsNullOrWhiteSpace(changes)) {
					stringBuilder.AppendLine(Lans.g("ContrFamily","The following changes were made to the patient")
							+" "+patientSynch.PatNum+" - "+Patients.GetNameFL(patientSynch.LName,patientSynch.FName,patientSynch.Preferred,patientSynch.MiddleI)
							+":\r\n"+changes);
				}
			}
			return stringBuilder.ToString();
		}

		///<summary>Synchs current information for patient to patientSynch passed in.  Returns a string representing what happened.
		///Optionally pass in the list of PatPlans for the clone and non-clone in order to potentially save db calls.</summary>
		public static string SynchCloneWithPatient(Patient patient,Patient patientSynch,Family familyCur=null,List<InsPlan> listInsPlans=null
			,List<InsSub> listInsSubs=null,List<Benefit> listBenefits=null,List<PatPlan> listPatPlans=null,List<PatPlan> listPatPlansForSynch=null) 
		{
			//No need to check RemotingRole; no call to db.
			Patient patCloneOld=patientSynch.Copy();
			PatientCloneDemographicChanges patientCloneDemoChanges=SynchCloneDemographics(patient,patientSynch);
			Patients.Update(patientSynch,patCloneOld);
			string strDataUpdated="";
			string strChngFrom=" "+Lans.g("ContrFamily","changed from")+" ";
			string strChngTo=" "+Lans.g("ContrFamily","to")+" ";
			string strBlank=Lans.g("ContrFamily","blank");
			foreach(PatientCloneField patientCloneField in patientCloneDemoChanges.ListFieldsUpdated) {
				strDataUpdated+=Lans.g("ContrFamily",patientCloneField.FieldName)+strChngFrom;
				strDataUpdated+=(string.IsNullOrEmpty(patientCloneField.OldValue)) ? strBlank : patientCloneField.OldValue;
				strDataUpdated+=strChngTo;
				strDataUpdated+=(string.IsNullOrEmpty(patientCloneField.NewValue)) ? strBlank : patientCloneField.NewValue;
				strDataUpdated+="\r\n";
			}
			if(familyCur==null) {
				familyCur=Patients.GetFamily(patient.PatNum);
			}
			if(listInsSubs==null) {
				listInsSubs=InsSubs.RefreshForFam(familyCur);
			}
			if(listInsPlans==null) {
				listInsPlans=InsPlans.RefreshForSubList(listInsSubs);
			}
			if(listBenefits==null) {
				listBenefits=Benefits.Refresh(PatPlans.Refresh(patient.PatNum),listInsSubs);
			}
			PatientClonePatPlanChanges patientClonePatPlanChanges=SynchClonePatPlans(patient,patientSynch,familyCur,listInsPlans,listInsSubs,listBenefits
				,listPatPlans,listPatPlansForSynch);
			strDataUpdated+=patientClonePatPlanChanges.StrDataUpdated;
			return strDataUpdated;
		}

		///<summary>Synchs the demographics from patient to patientSynch.
		///Returns a PatientCloneSynch object that represents specifics regarding anything that changed during the synching process.
		///This method does not synch the family or the super family on purpose.</summary>
		private static PatientCloneDemographicChanges SynchCloneDemographics(Patient patient,Patient patientSynch) {
			//No need to check RemotingRole; no call to db and private method.
			PatientCloneDemographicChanges patientCloneDemoChanges=new PatientCloneDemographicChanges();
			//We allow users to synch clones to clones now.  Therefore, we need to always go to the database and figure out the PatNum of the original.
			long patNumOriginal=patient.PatNum;
			if(PatientLinks.IsPatientAClone(patient.PatNum)) {
				//The patient that is going to synch its settings to patientSynch must be a clone, go get the PatNum of the original patient.
				patNumOriginal=PatientLinks.GetOriginalPatNumFromClone(patient.PatNum);
			}
			bool isSynchTheMaster=(patientSynch.PatNum==patNumOriginal);
			#region Synch Clone Data - Patient Demographics
			patientCloneDemoChanges.ListFieldsUpdated=new List<PatientCloneField>();
			patientCloneDemoChanges.ListFieldsCleared=new List<string>();
			if(patientSynch.FName.ToLower()!=patient.FName.ToLower()) {
				if(patientSynch.FName!="" && patient.FName=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("First Name");
				}
				string fName=patient.FName.ToUpper();
				if(isSynchTheMaster) {//We are synching a clone to the master, do NOT update the master's field to all caps.
					fName=patient.FName.ToUpperFirstOnly();
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("First Name",patientSynch.FName,fName));
				patientSynch.FName=fName;
			}
			if(patientSynch.LName.ToLower()!=patient.LName.ToLower()) {
				if(patientSynch.LName!="" && patient.LName=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("Last Name");
				}
				string lName=patient.LName.ToUpper();
				if(isSynchTheMaster) {//We are synching a clone to the master, do NOT update the master's field to all caps.
					lName=patient.LName.ToUpperFirstOnly();
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Last Name",patientSynch.LName,lName));
				patientSynch.LName=lName;
			}
			if(patientSynch.Title!=patient.Title) {
				if(patientSynch.Title!="" && patient.Title=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("Title");
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Title",patientSynch.Title,patient.Title));
				patientSynch.Title=patient.Title;
			}
			if(patientSynch.Preferred.ToLower()!=patient.Preferred.ToLower()) {
				if(patientSynch.Preferred!="" && patient.Preferred=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("Preferred Name");
				}
				string preferred=patient.Preferred.ToUpper();
				if(isSynchTheMaster) {//We are synching a clone to the master, do NOT update the master's field to all caps.
					preferred=patient.Preferred.ToUpperFirstOnly();
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Preferred Name",patientSynch.Preferred,preferred));
				patientSynch.Preferred=preferred;
			}
			if(patientSynch.MiddleI.ToLower()!=patient.MiddleI.ToLower()) {
				if(patientSynch.MiddleI!=""	&& patient.MiddleI=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("Middle Initial");
				}
				string middleI=patient.MiddleI.ToUpper();
				if(isSynchTheMaster) {//We are synching a clone to the master, do NOT update the master's field to all caps.
					middleI=patient.MiddleI.ToUpperFirstOnly();
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Middle Initial",patientSynch.MiddleI,middleI));
				patientSynch.MiddleI=middleI;
			}
			if(patientSynch.Birthdate!=patient.Birthdate) {
				if(patientSynch.Birthdate.Year > 1880 && patient.Birthdate.Year < 1880) {
					patientCloneDemoChanges.ListFieldsCleared.Add("Birthdate");
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Birthdate",patientSynch.Birthdate.ToShortDateString(),patient.Birthdate.ToShortDateString()));
				patientSynch.Birthdate=patient.Birthdate;
			}
			//As of v17.2, it is desirable to allow patient clones to be in different families... whatever.
			//if(patientSynch.Guarantor!=patient.Guarantor) {
			//	Patient patCloneGuar=Patients.GetPat(patientSynch.Guarantor);
			//	Patient patNonCloneGuar=Patients.GetPat(patient.Guarantor);
			//	string strPatCloneGuarName="";
			//	string strPatNonCloneGuarName="";
			//	if(patCloneGuar!=null) {
			//		strPatCloneGuarName=Patients.GetNameFL(patCloneGuar.LName,patCloneGuar.FName,patCloneGuar.Preferred,patCloneGuar.MiddleI);
			//	}
			//	if(patNonCloneGuar!=null) {
			//		strPatNonCloneGuarName=Patients.GetNameFL(patNonCloneGuar.LName,patNonCloneGuar.FName,patNonCloneGuar.Preferred,patNonCloneGuar.MiddleI);
			//	}
			//	patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Guarantor",patientSynch.Guarantor.ToString()+" - "+strPatCloneGuarName,patient.Guarantor.ToString()+" - "+strPatNonCloneGuarName));
			//	patientSynch.Guarantor=patient.Guarantor;
			//}
			if(patientSynch.ResponsParty!=patient.ResponsParty) {
				Patient patCloneRespPart=Patients.GetPat(patientSynch.ResponsParty);
				Patient patNonCloneRespPart=Patients.GetPat(patient.ResponsParty);
				string strPatCloneRespPartName="";
				string strPatNonCloneRespPartName="";
				if(patCloneRespPart!=null) {
					strPatCloneRespPartName=Patients.GetNameFL(patCloneRespPart.LName,patCloneRespPart.FName,patCloneRespPart.Preferred,patCloneRespPart.MiddleI);
				}
				if(patNonCloneRespPart!=null) {
					strPatNonCloneRespPartName=Patients.GetNameFL(patNonCloneRespPart.LName,patNonCloneRespPart.FName,patNonCloneRespPart.Preferred,patNonCloneRespPart.MiddleI);
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Responsible Party",patientSynch.ResponsParty.ToString()+" - "+strPatCloneRespPartName,patient.ResponsParty.ToString()+" - "+strPatNonCloneRespPartName ));
				patientSynch.ResponsParty=patient.ResponsParty;
			}
			//As of v17.2, it is desirable to allow patient clones to be in different super families... whatever.
			//if(patientSynch.SuperFamily!=patient.SuperFamily) {
			//	Patient patCloneSupFam=Patients.GetPat(patientSynch.SuperFamily);
			//	Patient patNonCloneSupFam=Patients.GetPat(patient.SuperFamily);
			//	string strPatCloneSupFamName="";
			//	string strPatNonCloneSupFamName="";
			//	if(patCloneSupFam!=null) {
			//		strPatCloneSupFamName=Patients.GetNameFL(patCloneSupFam.LName,patCloneSupFam.FName,patCloneSupFam.Preferred,patCloneSupFam.MiddleI);
			//	}
			//	if(patNonCloneSupFam!=null) {
			//		strPatNonCloneSupFamName=Patients.GetNameFL(patNonCloneSupFam.LName,patNonCloneSupFam.FName,patNonCloneSupFam.Preferred,patNonCloneSupFam.MiddleI);
			//	}
			//	patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Super Family",patientSynch.SuperFamily.ToString()+" - "+strPatCloneSupFamName,patient.SuperFamily.ToString()+" - "+strPatNonCloneSupFamName ));
			//	patientSynch.SuperFamily=patient.SuperFamily;
			//}
			if(patientSynch.PatStatus!=patient.PatStatus) {
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Patient Status",patientSynch.PatStatus.ToString(),patient.PatStatus.ToString() ));
				patientSynch.PatStatus=patient.PatStatus;
			}
			if(patientSynch.Gender!=patient.Gender) {
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Gender",patientSynch.Gender.ToString(),patient.Gender.ToString() ));
				patientSynch.Gender=patient.Gender;
			}
			if(patientSynch.Language!=patient.Language) {
				string strPatCloneLang="";
				string strPatNonCloneLang="";
				try {
					strPatCloneLang=CodeBase.MiscUtils.GetCultureFromThreeLetter(patientSynch.Language).DisplayName;
				}
				catch {
					strPatCloneLang=patientSynch.Language;
				}
				try {
					strPatNonCloneLang=CodeBase.MiscUtils.GetCultureFromThreeLetter(patient.Language).DisplayName;
				}
				catch {
					strPatNonCloneLang=patient.Language;
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Language",strPatCloneLang,strPatNonCloneLang));
				patientSynch.Language=patient.Language;
			}
			if(patientSynch.SSN!=patient.SSN) {
				if(patientSynch.SSN!=""	&& patient.SSN=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("SSN");
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("SSN",patientSynch.SSN,patient.SSN));
				patientSynch.SSN=patient.SSN;
			}
			if(patientSynch.Position!=patient.Position) {
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Position",patientSynch.Position.ToString(),patient.Position.ToString()));
				patientSynch.Position=patient.Position;
			}
			if(patientSynch.Address!=patient.Address) {
				if(patientSynch.Address!=""	&& patient.Address=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("Address");
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Address",patientSynch.Address,patient.Address));
				patientSynch.Address=patient.Address;
			}
			if(patientSynch.Address2!=patient.Address2) {
				if(patientSynch.Address2!="" && patient.Address2=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("Address2");
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Address2",patientSynch.Address2,patient.Address2));
				patientSynch.Address2=patient.Address2;
			}
			if(patientSynch.City!=patient.City) {
				if(patientSynch.City!="" && patient.City=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("City");
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("City",patientSynch.City,patient.City));
				patientSynch.City=patient.City;
			}
			if(patientSynch.State!=patient.State) {
				if(patientSynch.State!=""	&& patient.State=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("State");
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("State",patientSynch.State,patient.State));
				patientSynch.State=patient.State;
			}
			if(patientSynch.Zip!=patient.Zip) {
				if(patientSynch.Zip!=""	&& patient.Zip=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("Zip");
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Zip",patientSynch.Zip,patient.Zip));
				patientSynch.Zip=patient.Zip;
			}
			if(patientSynch.County!=patient.County) {
				if(patientSynch.County!=""	&& patient.County=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("County");
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("County",patientSynch.County,patient.County));
				patientSynch.County=patient.County;
			}
			if(patientSynch.AddrNote!=patient.AddrNote) {
				if(patientSynch.AddrNote!=""	&& patient.AddrNote=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("Address Note");
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Address Note",patientSynch.AddrNote,patient.AddrNote));
				patientSynch.AddrNote=patient.AddrNote;
			}
			if(patientSynch.HmPhone!=patient.HmPhone) {
				if(patientSynch.HmPhone!=""	&& patient.HmPhone=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("Home Phone");
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Home Phone",patientSynch.HmPhone,patient.HmPhone));
				patientSynch.HmPhone=patient.HmPhone;
			}
			if(patientSynch.WirelessPhone!=patient.WirelessPhone) {
				if(patientSynch.WirelessPhone!=""	&& patient.WirelessPhone=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("Wireless Phone");
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Wireless Phone",patientSynch.WirelessPhone,patient.WirelessPhone));
				patientSynch.WirelessPhone=patient.WirelessPhone;
			}
			if(patientSynch.WkPhone!=patient.WkPhone) {
				if(patientSynch.WkPhone!=""	&& patient.WkPhone=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("Work Phone");
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Work Phone",patientSynch.WkPhone,patient.WkPhone));
				patientSynch.WkPhone=patient.WkPhone;
			}
			if(patientSynch.Email!=patient.Email) {
				if(patientSynch.Email!=""	&& patient.Email=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("Email");
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Email",patientSynch.Email,patient.Email));
				patientSynch.Email=patient.Email;
			}
			if(patientSynch.TxtMsgOk!=patient.TxtMsgOk) {
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("TxtMsgOk",patientSynch.TxtMsgOk.ToString(),patient.TxtMsgOk.ToString()));
				patientSynch.TxtMsgOk=patient.TxtMsgOk;
			}
			if(patientSynch.BillingType!=patient.BillingType) {
				Def defCloneBillingType=Defs.GetDef(DefCat.BillingTypes,patientSynch.BillingType);
				Def defNonCloneBillingType=Defs.GetDef(DefCat.BillingTypes,patient.BillingType);
				string cloneBillType=(defCloneBillingType==null ? "" : defCloneBillingType.ItemName);
				string nonCloneBillType=(defNonCloneBillingType==null ? "" : defNonCloneBillingType.ItemName);
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Billing Type",cloneBillType,nonCloneBillType));
				patientSynch.BillingType=patient.BillingType;
			}
			if(patientSynch.FeeSched!=patient.FeeSched) {
				FeeSched cloneFeeSchedObj=FeeScheds.GetFirstOrDefault(x => x.FeeSchedNum==patientSynch.FeeSched);
				FeeSched nonCloneFeeSchedObj=FeeScheds.GetFirstOrDefault(x => x.FeeSchedNum==patient.FeeSched);
				string cloneFeeSched=(cloneFeeSchedObj==null ? "" : cloneFeeSchedObj.Description);
				string nonCloneFeeSched=(nonCloneFeeSchedObj==null ? "" : nonCloneFeeSchedObj.Description);
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Fee Schedule",cloneFeeSched,nonCloneFeeSched));
				patientSynch.FeeSched=patient.FeeSched;
			}
			if(patientSynch.CreditType!=patient.CreditType) {
				if(patientSynch.CreditType!=""	&& patient.CreditType=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("Credit Type");
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Credit Type",patientSynch.CreditType,patient.CreditType));
				patientSynch.CreditType=patient.CreditType;
			}
			if(patientSynch.MedicaidID!=patient.MedicaidID) {
				if(patientSynch.MedicaidID!=""	&& patient.MedicaidID=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("Medicaid ID");
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Medicaid ID",patientSynch.MedicaidID,patient.MedicaidID));
				patientSynch.MedicaidID=patient.MedicaidID;
			}
			if(patientSynch.MedUrgNote!=patient.MedUrgNote) {
				if(patientSynch.MedUrgNote!=""	&& patient.MedUrgNote=="") {
					patientCloneDemoChanges.ListFieldsCleared.Add("Medical Urgent Note");
				}
				patientCloneDemoChanges.ListFieldsUpdated.Add(new PatientCloneField("Medical Urgent Note",patientSynch.MedUrgNote,patient.MedUrgNote));
				patientSynch.MedUrgNote=patient.MedUrgNote;
			}
			#endregion Synch Clone Data - Patient Demographics
			return patientCloneDemoChanges;
		}

		///<summary>Synchs the pat plan information from patient to patientSynch passed in.
		///Returns a PatientClonePatPlanChanges object that represents specifics regarding anything that changed during the synch.
		///Optionally pass in the lists of insurance information in order to potentially save db calls.</summary>
		public static PatientClonePatPlanChanges SynchClonePatPlans(Patient patient,Patient patientSynch,Family familyCur,List<InsPlan> listInsPlans
			,List<InsSub> listInsSubs,List<Benefit> listBenefits,List<PatPlan> listPatPlans=null,List<PatPlan> listPatPlansForSynch=null)
		{
			//No need to check RemotingRole; no call to db and private method with arguments that act like out parameters.
			//TODO: correct all messages so that they don't refer to "the clone" or "the original".
			PatientClonePatPlanChanges patientClonePatPlanChanges=new PatientClonePatPlanChanges();
			#region Synch Clone Data - PatPlans
			patientClonePatPlanChanges.PatPlansChanged=false;
			patientClonePatPlanChanges.PatPlansInserted=false;
			patientClonePatPlanChanges.StrDataUpdated="";
			if(listPatPlans==null) {
				listPatPlans=PatPlans.Refresh(patient.PatNum);//ordered by ordinal
			}
			if(listPatPlansForSynch==null) {
				listPatPlansForSynch=PatPlans.Refresh(patientSynch.PatNum);//ordered by ordinal
			}
			List<Claim> claimList=Claims.Refresh(patientSynch.PatNum);//used to determine if the patplan we are going to drop is attached to a claim with today's date
			for(int i=claimList.Count-1;i>-1;i--) {//remove any claims that do not have a date of today, we are only concerned with claims with today's date
				if(claimList[i].DateService==DateTime.Today) {
					continue;
				}
				claimList.RemoveAt(i);
			}
			//if the clone has more patplans than the non-clone, drop the additional patplans
			//we will compute all estimates for the clone after all of the patplan adding/dropping/rearranging
			for(int i=listPatPlansForSynch.Count-1;i>listPatPlans.Count-1;i--) {
				InsSub insSubCloneCur=InsSubs.GetOne(listPatPlansForSynch[i].InsSubNum);
				//we will drop the clone's patplan because the clone has more patplans than the non-clone
				//before we can drop the plan, we have to make sure there is not a claim with today's date
				bool isAttachedToClaim=false;
				for(int j=0;j<claimList.Count;j++) {//claimList will only contain claims with DateService=Today
					if(claimList[j].PlanNum!=insSubCloneCur.PlanNum) {//different insplan
						continue;
					}
					patientClonePatPlanChanges.StrDataUpdated+=Lans.g("ContrFamily","Insurance Plans do not match.  "
						+"Due to a claim with today's date we cannot synch the plans, the issue must be corrected manually on the following plan")
						+": "+InsPlans.GetDescript(insSubCloneCur.PlanNum,familyCur,listInsPlans,listPatPlansForSynch[i].InsSubNum,listInsSubs)+".\r\n";
					isAttachedToClaim=true;
					break;
				}
				if(isAttachedToClaim) {//we will continue trying to drop non-clone additional plans, but only if no claim for today exists
					continue;
				}
				patientClonePatPlanChanges.StrDataUpdated+=Lans.g("ContrFamily","The following insurance plan was dropped due to it not existing with the same ordinal on the original patient")+": "
					+InsPlans.GetDescript(insSubCloneCur.PlanNum,familyCur,listInsPlans,listPatPlansForSynch[i].InsSubNum,listInsSubs)+".\r\n";
				patientClonePatPlanChanges.PatPlansChanged=true;
				PatPlans.DeleteNonContiguous(listPatPlansForSynch[i].PatPlanNum);
				listPatPlansForSynch.RemoveAt(i);
			}
			for(int i=0;i<listPatPlans.Count;i++) {
				InsSub insSubNonCloneCur=InsSubs.GetOne(listPatPlans[i].InsSubNum);
				string insPlanNonCloneDescriptCur=InsPlans.GetDescript(insSubNonCloneCur.PlanNum,familyCur,listInsPlans,listPatPlans[i].InsSubNum,listInsSubs);
				if(listPatPlansForSynch.Count<i+1) {//if there is not a PatPlan at this ordinal position for the clone, add a new one that is an exact copy, with correct PatNum of course
					PatPlan patPlanNew=listPatPlans[i].Copy();
					patPlanNew.PatNum=patientSynch.PatNum;
					PatPlans.Insert(patPlanNew);
					patientClonePatPlanChanges.PatPlansInserted=true;
					patientClonePatPlanChanges.StrDataUpdated+=Lans.g("ContrFamily","The following insurance was added")+": "+insPlanNonCloneDescriptCur+".\r\n";
					patientClonePatPlanChanges.PatPlansChanged=true;
					continue;
				}
				InsSub insSubCloneCur=InsSubs.GetOne(listPatPlansForSynch[i].InsSubNum);
				string insPlanCloneDescriptCur=InsPlans.GetDescript(insSubCloneCur.PlanNum,familyCur,listInsPlans,listPatPlansForSynch[i].InsSubNum,listInsSubs);
				if(listPatPlans[i].InsSubNum!=listPatPlansForSynch[i].InsSubNum) {//both pats have a patplan at this ordinal, but the clone's is pointing to a different inssub
					//we will drop the clone's patplan and add the non-clone's patplan
					//before we can drop the plan, we have to make sure there is not a claim with today's date
					bool isAttachedToClaim=false;
					for(int j=0;j<claimList.Count;j++) {//claimList will only contain claims with DateService=Today
						if(claimList[j].PlanNum!=insSubCloneCur.PlanNum) {//different insplan
							continue;
						}
						patientClonePatPlanChanges.StrDataUpdated+=Lans.g("ContrFamily","Insurance Plans do not match.  "
							+"Due to a claim with today's date we cannot synch the plans, the issue must be corrected manually on the following plan")
							+": "+insPlanCloneDescriptCur+".\r\n";
						isAttachedToClaim=true;
						break;
					}
					if(isAttachedToClaim) {//if we cannot change this plan to match the non-clone's plan at the same ordinal, we will synch the rest of the plans and let the user know to fix manually
						continue;
					}
					patientClonePatPlanChanges.StrDataUpdated+=Lans.g("ContrFamily","The following plan was updated to match the selected patient's plan")+": "+insPlanCloneDescriptCur+".\r\n";
					patientClonePatPlanChanges.PatPlansChanged=true;
					PatPlans.DeleteNonContiguous(listPatPlansForSynch[i].PatPlanNum);//we use the NonContiguous version because we are going to insert into this same ordinal, compute estimates will happen at the end of all the changes
					PatPlan patPlanCopy=listPatPlans[i].Copy();
					patPlanCopy.PatNum=patientSynch.PatNum;
					PatPlans.Insert(patPlanCopy);
				}
				else {
					//both clone and non-clone have the same patplan.InsSubNum at this position in their list, just make sure all data in the patplans match
					if(listPatPlans[i].Ordinal!=listPatPlansForSynch[i].Ordinal) {
						patientClonePatPlanChanges.StrDataUpdated+=Lans.g("ContrFamily","The ordinal of the insurance plan")+" "+insPlanCloneDescriptCur+" "
							+Lans.g("ContrFamily","was updated to")+" "+listPatPlans[i].Ordinal.ToString()+".\r\n";
						patientClonePatPlanChanges.PatPlansChanged=true;
						listPatPlansForSynch[i].Ordinal=listPatPlans[i].Ordinal;
					}
					if(listPatPlans[i].IsPending!=listPatPlansForSynch[i].IsPending) {
						patientClonePatPlanChanges.StrDataUpdated+=Lans.g("ContrFamily","The pending status of the insurance plan")+" "+insPlanCloneDescriptCur+" "
							+Lans.g("ContrFamily","was updated to")+" "+listPatPlans[i].IsPending.ToString()+".\r\n";
						patientClonePatPlanChanges.PatPlansChanged=true;
						listPatPlansForSynch[i].IsPending=listPatPlans[i].IsPending;
					}
					if(listPatPlans[i].Relationship!=listPatPlansForSynch[i].Relationship) {
						patientClonePatPlanChanges.StrDataUpdated+=Lans.g("ContrFamily","The relationship to the subscriber of the insurance plan")+" "+insPlanCloneDescriptCur+" "
							+Lans.g("ContrFamily","was updated to")+" "+listPatPlans[i].Relationship.ToString()+".\r\n";
						patientClonePatPlanChanges.PatPlansChanged=true;
						listPatPlansForSynch[i].Relationship=listPatPlans[i].Relationship;
					}
					if(listPatPlans[i].PatID!=listPatPlansForSynch[i].PatID) {
						patientClonePatPlanChanges.StrDataUpdated+=Lans.g("ContrFamily","The patient ID of the insurance plan")+" "+insPlanCloneDescriptCur+" "
							+Lans.g("ContrFamily","was updated to")+" "+listPatPlans[i].PatID+".\r\n";
						patientClonePatPlanChanges.PatPlansChanged=true;
						listPatPlansForSynch[i].PatID=listPatPlans[i].PatID;
					}
					PatPlans.Update(listPatPlansForSynch[i]);
				}
			}
			if(patientClonePatPlanChanges.PatPlansInserted) {
				SecurityLogs.MakeLogEntry(Permissions.PatPlanCreate,0,Lans.g("ContrFamily","One or more PatPlans created via Synch Clone tool."));
			}
			if(patientClonePatPlanChanges.PatPlansChanged) {
				//compute all estimates for clone after making changes to the patplans
				List<ClaimProc> claimProcs=ClaimProcs.Refresh(patientSynch.PatNum);
				List<Procedure> procs=Procedures.Refresh(patientSynch.PatNum);
				listPatPlansForSynch=PatPlans.Refresh(patientSynch.PatNum);
				listInsSubs=InsSubs.RefreshForFam(familyCur);
				listInsPlans=InsPlans.RefreshForSubList(listInsSubs);
				listBenefits=Benefits.Refresh(listPatPlansForSynch,listInsSubs);
				Procedures.ComputeEstimatesForAll(patientSynch.PatNum,claimProcs,procs,listInsPlans,listPatPlansForSynch,listBenefits,patientSynch.Age,listInsSubs);
				Patients.SetHasIns(patientSynch.PatNum);
			}
			#endregion Synch Clone Data - PatPlans
			return patientClonePatPlanChanges;
		}
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods

		///<summary>Returns true if the patient passed in is a clone otherwise false.</summary>
		public static bool IsPatientAClone(long patNum) {
			//No need to check RemotingRole; no call to db.
			return PatientLinks.IsPatientAClone(patNum);
		}

		///<summary>Returns true if the patient passed in is a clone or the original patient of clones, otherwise false.
		///This method is helpful when trying to determine if the patient passed in is related in any way to the clone system.</summary>
		public static bool IsPatientACloneOrOriginal(long patNum) {
			//No need to check RemotingRole; no call to db.
			return PatientLinks.IsPatientACloneOrOriginal(patNum);
		}

		///<summary>Returns true if one patient is a clone of the other or if both are clones of the same master, otherwise false.
		///Always returns false if patNum1 and patNum2 are the same PatNum.</summary>
		public static bool ArePatientsClonesOfEachOther(long patNum1,long patNum2) {
			//No need to check RemotingRole; no call to db.
			return PatientLinks.ArePatientsClonesOfEachOther(patNum1,patNum2);
		}

		///<summary>Replaces all patient fields in the given message with the given patient's information.  Returns the resulting string.
		///Replaces: [FName], [LName], [LNameLetter], [NameF], [NameFL], [PatNum], 
		///[ChartNumber], [HmPhone], [WkPhone], [WirelessPhone], [ReferredFromProvNameFL], etc.</summary>
		public static string ReplacePatient(string message,Patient pat) {
			if(pat==null) {
				return message;
			}
			string retVal=message;
			retVal=retVal.Replace("[FName]",pat.FName);
			retVal=retVal.Replace("[LName]",pat.LName);
			retVal=retVal.Replace("[LNameLetter]",pat.LName.Substring(0,1).ToUpper());
			retVal=retVal.Replace("[NameF]",pat.FName);
			retVal=retVal.Replace("[NameFL]",Patients.GetNameFL(pat.LName,pat.FName,"",""));
			retVal=retVal.Replace("[PatNum]",pat.PatNum.ToString());
			retVal=retVal.Replace("[ChartNumber]",pat.ChartNumber);
			retVal=retVal.Replace("[HmPhone]",pat.HmPhone);
			retVal=retVal.Replace("[WkPhone]",pat.WkPhone);
			retVal=retVal.Replace("[Gender]",pat.Gender.ToString());
			retVal=retVal.Replace("[Email]",pat.Email);
			retVal=retVal.Replace("[ProvNum]",pat.PriProv.ToString());
			retVal=retVal.Replace("[ClinicNum]",pat.ClinicNum.ToString());
			retVal=retVal.Replace("[WirelessPhone]",pat.WirelessPhone);
			retVal=retVal.Replace("[Birthdate]",pat.Birthdate.ToShortDateString());
			retVal=retVal.Replace("[Birthdate_yyyyMMdd]",pat.Birthdate.ToString("yyyyMMdd"));
			retVal=retVal.Replace("[SSN]",pat.SSN);
			retVal=retVal.Replace("[Address]",pat.Address);
			retVal=retVal.Replace("[Address2]",pat.Address2);
			retVal=retVal.Replace("[City]",pat.City);
			retVal=retVal.Replace("[State]",pat.State);
			retVal=retVal.Replace("[Zip]",pat.Zip);
			retVal=retVal.Replace("[MonthlyCardsOnFile]",CreditCards.GetMonthlyCardsOnFile(pat.PatNum));
			if(retVal.Contains("[ReferredFromProvNameFL]")) {
				Referral patRef = Referrals.GetReferralForPat(pat.PatNum);
				if(patRef!=null) {
					retVal=retVal.Replace("[ReferredFromProvNameFL]",Patients.GetNameFL(patRef.LName,patRef.FName,"",""));
				}
				else {
					retVal=retVal.Replace("[ReferredFromProvNameFL]","");
				}
			}
			return retVal;
		}

		///<summary>Returns true if the replacement field is PHI. Case insensitive.</summary>
		public static bool IsFieldPHI(string field) {
			return field.ToLower().In(ListPHIFields.Select(x => x.ToLower()));
		}

		///<summary>Returns true if the text contains a replacement field that is PHI. Case insensitive.</summary>
		public static bool DoesContainPHIField(string text) {
			string textLower=text.ToLower();
			return Patients.ListPHIFields.Select(x => x.ToLower()).Any(x => textLower.Contains(x));
		}

		///<summary>The list of fields that are considered PHI. 
		///<para/>According to the United States Electronic Code of Federal Regulations Title 45 160.103, protected health information is individually 
		///identifiable health information that:
		///"... (1) Is created or received by a health care provider, health plan, employer, or health care clearinghouse; and
		///(2) Relates to the past, present, or future physical or mental health or condition of an individual; the provision of health care to an individual; or the past, present, or future payment for the provision of health care to an individual; and
		///(i) That identifies the individual; or
		///(ii) With respect to which there is a reasonable basis to believe the information can be used to identify the individual".
		///(https://www.ecfr.gov/cgi-bin/text-idx?SID=2f948e08dbf4b32b8e30a4f0ac6f66cf&amp;mc=true&amp;node=se45.1.160_1103&amp;rgn=div8)</summary>
		public static List<string> ListPHIFields {
			get {
				return new List<string> {
					"[LName]",
					"[NameFL]",
					"[WirelessPhone]",
					"[HmPhone]",
					"[WkPhone]",
					"[Birthdate]",
					"[Birthdate_yyyyMMdd]",
					"[SSN]",
					"[Address]",
					"[Address2]",
					"[City]",
					"[Zip]",
				};
			}
		}

		#endregion


		///<summary>Creates and inserts a "new patient" using the information passed in.  Validation must be done prior to calling this.
		///securityLogMsg is typically set to something that lets the customer know where this new patient was created from.
		///Used by multiple applications so be very careful when changing this method.  E.g. Open Dental and Web Sched.</summary>
		public static Patient CreateNewPatient(string lName,string fName,DateTime birthDate,long priProv,long clinicNum,string securityLogMsg
			,LogSources logSource=LogSources.None,string email="",string hmPhone="",string wirelessPhone="") 
		{
			//No need to check RemotingRole; no call to db.
			Patient patient=new Patient();
			if(lName.Length>1) {//eg Sp
				patient.LName=lName.Substring(0,1).ToUpper()+lName.Substring(1);
			}
			if(fName.Length>1) {
				patient.FName=fName.Substring(0,1).ToUpper()+fName.Substring(1);
			}
			patient.Birthdate=birthDate;
			patient.PatStatus=PatientStatus.Patient;
			patient.BillingType=PrefC.GetLong(PrefName.PracticeDefaultBillType);
			patient.PriProv=priProv;
			patient.Gender=PatientGender.Unknown;
			patient.ClinicNum=clinicNum;
			patient.Email=email;
			patient.HmPhone=hmPhone;
			patient.WirelessPhone=wirelessPhone;
			Patients.Insert(patient,false);
			SecurityLogs.MakeLogEntry(Permissions.PatientCreate,patient.PatNum,securityLogMsg,logSource);
			CustReference custRef=new CustReference();
			custRef.PatNum=patient.PatNum;
			CustReferences.Insert(custRef);
			Patient PatOld=patient.Copy();
			patient.Guarantor=patient.PatNum;
			Patients.Update(patient,PatOld);
			return patient;
		}
		
		///<summary>Returns a Family object for the supplied patNum.  Use Family.GetPatient to extract the desired patient from the family.</summary>
		public static Family GetFamily(long patNum) {
			//No need to check RemotingRole; no call to db.
			return ODMethodsT.Coalesce(GetFamilies(new List<long>() { patNum }).FirstOrDefault(),new Family());
		}

		public static List<Family> GetFamilies(List<long> listPatNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Family>>(MethodBase.GetCurrentMethod(),listPatNums);
			}
			if(listPatNums==null || listPatNums.Count < 1) {
				return new List<OpenDentBusiness.Family>();
			}
			string command=@"SELECT f.*,CASE WHEN f.Guarantor != f.PatNum THEN 1 ELSE 0 END AS IsNotGuar 
				FROM patient p
				INNER JOIN patient f ON f.Guarantor=p.Guarantor
				WHERE p.PatNum IN ("+string.Join(",",listPatNums.Select(x => POut.Long(x)))+@")
				ORDER BY IsNotGuar, f.Birthdate";
			List<Family> listFamilies=new List<Family>();
			List<Patient> listPatients=Crud.PatientCrud.SelectMany(command);
			foreach(Patient patient in listPatients) {
				patient.Age = DateToAge(patient.Birthdate);
			}
			Dictionary<long,List<Patient>> dictFamilyPatients=listPatients.GroupBy(x => x.Guarantor)
				.ToDictionary(y => y.Key,y => y.ToList());
			foreach(KeyValuePair<long,List<Patient>> kvp in dictFamilyPatients) {
				Family family=new Family();
				family.ListPats=kvp.Value.ToArray();
				listFamilies.Add(family);
			}
			return listFamilies;
		}

		///<summary>Returns a list of patients that have the associated FeeSchedNum.  Used when attempting to hide FeeScheds.</summary>
		public static List<Patient> GetForFeeSched(long feeSchedNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),feeSchedNum);
			}
			string command="SELECT * FROM patient WHERE FeeSched="+POut.Long(feeSchedNum);
			return Crud.PatientCrud.SelectMany(command);
		}

		/// <summary>Returns a patient, or null, based on an internally defined or externally defined globaly unique identifier.  This can be an OID, GUID, IID, UUID, etc.</summary>
		/// <param name="IDNumber">The extension portion of the GUID/OID.  Example: 333224444 if using SSN as a the unique identifier</param>
		/// <param name="OID">root OID that the IDNumber extends.  Example: 2.16.840.1.113883.4.1 is the OID for the Social Security Numbers.</param>
		public static Patient GetByGUID(string IDNumber, string OID){
			if(OID==OIDInternals.GetForType(IdentifierType.Patient).IDRoot) {//OID matches the localy defined patnum OID.
				return Patients.GetPat(PIn.Long(IDNumber));
			}
			else {
				OIDExternal oidExt=OIDExternals.GetByRootAndExtension(OID,IDNumber);
				if(oidExt==null || oidExt.IDType!=IdentifierType.Patient) {
					return null;//OID either not found, or does not represent a patient.
				}
				return Patients.GetPat(oidExt.IDInternal);
			}
		}

		///<summary>This is a way to get a single patient from the database if you don't already have a family object to use.  Will return null if not found.</summary>
		public static Patient GetPat(long patNum) {
			if(patNum==0) {
				return null;
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Patient>(MethodBase.GetCurrentMethod(),patNum);
			} 
			string command="SELECT * FROM patient WHERE PatNum="+POut.Long(patNum);
			Patient pat=null;
			try {
				pat=Crud.PatientCrud.SelectOne(patNum);
			}
			catch { }
			if(pat==null) {
				return null;//used in eCW bridge
			}
			pat.Age = DateToAge(pat.Birthdate);
			return pat;
		}

		///<summary>Will return null if not found.</summary>
		public static Patient GetPatByChartNumber(string chartNumber) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Patient>(MethodBase.GetCurrentMethod(),chartNumber);
			}
			if(chartNumber=="") {
				return null;
			}
			string command="SELECT * FROM patient WHERE ChartNumber='"+POut.String(chartNumber)+"'";
			Patient pat=null;
			try {
				pat=Crud.PatientCrud.SelectOne(command);
			}
			catch { }
			if(pat==null) {
				return null;
			}
			pat.Age = DateToAge(pat.Birthdate);
			return pat;
		}

		///<summary>Will return null if not found.</summary>
		public static Patient GetPatBySSN(string ssn) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Patient>(MethodBase.GetCurrentMethod(),ssn);
			}
			if(ssn=="") {
				return null;
			}
			string command="SELECT * FROM patient WHERE SSN='"+POut.String(ssn)+"'";
			Patient pat=null;
			try {
				pat=Crud.PatientCrud.SelectOne(command);
			}
			catch { }
			if(pat==null) {
				return null;
			}
			pat.Age = DateToAge(pat.Birthdate);
			return pat;
		}

		///<summary>Gets all of the PatNums for the family members of the PatNums passed in.  Returns a distinct list of PatNums.</summary>
		public static List<long> GetAllFamilyPatNums(List<long> listPatNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),listPatNums);
			}
			if(listPatNums==null || listPatNums.Count<1) {
				return new List<long>();
			}
			string command="SELECT patient.PatNum FROM patient "
				+"INNER JOIN ("
					+"SELECT DISTINCT Guarantor FROM patient WHERE PatNum IN ("+string.Join(",",listPatNums)+")"
					+") guarnums ON guarnums.Guarantor=patient.Guarantor "
				+"WHERE patient.PatStatus!="+POut.Int((int)PatientStatus.Deleted);
			return Db.GetListLong(command);
		}

		///<summary>Gets all of the PatNums for the family members of the Guarantor nums passed in.  Returns a distinct list of PatNums that will include
		///the guarantor PatNums passed in and will include all PatStatuses including archived and deleted.  Used in Ledgers.cs for aging.</summary>
		public static List<long> GetAllFamilyPatNumsForGuars(List<long> listGuarNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),listGuarNums);
			}
			if(listGuarNums==null || listGuarNums.Count<1) {
				return new List<long>();
			}
			string command="SELECT PatNum FROM patient WHERE Guarantor IN ("+string.Join(",",listGuarNums)+")";
			return Db.GetListLong(command);
		}

		public static List<Patient> GetChangedSince(DateTime changedSince) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),changedSince);
			}
			string command="SELECT * FROM patient WHERE DateTStamp > "+POut.DateT(changedSince);
			//command+=" "+DbHelper.LimitAnd(1000);
			return Crud.PatientCrud.SelectMany(command);
		}

		///<summary>Used if the number of records are very large, in which case using GetChangedSince(DateTime changedSince) is not the preffered route due to memory problems caused by large recordsets. </summary>
		public static List<long> GetChangedSincePatNums(DateTime changedSince) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),changedSince);
			}
			string command="SELECT PatNum From patient WHERE DateTStamp > "+POut.DateT(changedSince);
			DataTable dt=Db.GetTable(command);
			List<long> patnums = new List<long>(dt.Rows.Count);
			for(int i=0;i<dt.Rows.Count;i++) {
				patnums.Add(PIn.Long(dt.Rows[i]["PatNum"].ToString()));
			}
			return patnums;
		}

		/// <summary>Gets PatNums of patients whose online password is  blank</summary>
		public static List<long> GetPatNumsForDeletion() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT PatNum FROM patient "
				+"LEFT JOIN userweb ON userweb.FKey=patient.PatNum "
					+"AND userweb.FKeyType="+POut.Int((int)UserWebFKeyType.PatientPortal)+" "
				+"WHERE userweb.FKey IS NULL OR userweb.Password='' ";
			return Db.GetListLong(command);
		}

		///<summary>ONLY for new patients. Set includePatNum to true for use the patnum from the import function.  Used in HL7.  Otherwise, uses InsertID to fill PatNum.</summary>
		public static long Insert(Patient pat,bool useExistingPK) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				pat.SecUserNumEntry=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				pat.PatNum=Meth.GetLong(MethodBase.GetCurrentMethod(),pat,useExistingPK);
				return pat.PatNum;
			}
			if(!useExistingPK) {
				return Crud.PatientCrud.Insert(pat);
			}
			return Crud.PatientCrud.Insert(pat,useExistingPK);
		}

		///<summary>Updates only the changed columns and returns the number of rows affected.  Supply the old Patient object to compare for changes.</summary>
		public static bool Update(Patient patient,Patient oldPatient) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),patient,oldPatient);
			}
			return Crud.PatientCrud.Update(patient,oldPatient);
		}

		///<summary>This is only used when entering a new patient and user clicks cancel.  It used to actually delete the patient, but that will mess up
		///UAppoint synch function.  DateTStamp needs to track deleted patients. So now, the PatStatus is simply changed to 4.</summary>
		public static void Delete(Patient pat) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),pat);
				return;
			}
			string command="UPDATE patient SET PatStatus="+POut.Long((int)PatientStatus.Deleted)+", "
				+"Guarantor=PatNum "
				+"WHERE PatNum ="+pat.PatNum.ToString();
			Db.NonQ(command);
		}

		///<summary>Only used for the Select Patient dialog.  Pass in a billing type of 0 for all billing types.</summary>
		public static DataTable GetPtDataTable(bool limit,string lname,string fname,string phone,
			string address,bool hideInactive,string city,string state,string ssn,string patNumStr,string chartnumber,
			long billingtype,bool guarOnly,bool showArchived,DateTime birthdate,
			long siteNum,string subscriberId,string email,string country,string regKey,string clinicNums,string invoiceNumber,
			List<long> explicitPatNums=null,long initialPatNum=0, bool showMerged=false)
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),limit,lname,fname,phone,address,hideInactive,city,state,ssn,patNumStr,chartnumber
					,billingtype,guarOnly,showArchived,birthdate,siteNum,subscriberId,email,country,regKey,clinicNums,invoiceNumber,explicitPatNums,
					initialPatNum,showMerged);
			}
			PatientSearchArgs searchArgs=new PatientSearchArgs() {
				limit=limit, //bool
				lname=POut.String(lname),
				fname=POut.String(fname),
				phone=POut.String(phone),
				address=POut.String(address),
				hideInactive=hideInactive, //bool
				city=POut.String(city),
				state=POut.String(state),
				ssn=POut.String(ssn),
				patNumStr=POut.String(patNumStr),
				chartnumber=POut.String(chartnumber),
				billingtype=billingtype, //long
				guarOnly=guarOnly, //bool
				showArchived=showArchived, //bool
				birthdate=birthdate, //date
				siteNum=siteNum, //long
				subscriberId=POut.String(subscriberId),
				email=POut.String(email),
				country=POut.String(country),
				invoicenumber=POut.String(invoiceNumber),
				regKey=POut.String(regKey)
			};
			string IsExactMatchSnippet=Patients.GetExactMatchSnippet(searchArgs);
			string billingsnippet=" ";
			if(billingtype!=0){
				billingsnippet+="AND patient.BillingType="+POut.Long(billingtype)+" ";
			}
			/*for(int i=0;i<billingtypes.Length;i++){//if length==0, it will get all billing types
				if(i==0){
					billingsnippet+="AND (";
				}
				else{
					billingsnippet+="OR ";
				}
				billingsnippet+="patient.BillingType ='"+billingtypes[i].ToString()+"' ";
				if(i==billingtypes.Length-1){//if there is only one row, this will also be triggered.
					billingsnippet+=") ";
				}
			}*/
			string phonedigits=new string(phone.Where(x=>char.IsDigit(x)).ToArray());
			//for(int i=0;i<phone.Length;i++){
			//	if(Regex.IsMatch(phone[i].ToString(),"[0-9]")){
			//		phonedigits=phonedigits+phone[i];
			//	}
			//}
			string regexp="";
			for(int i=0;i<phonedigits.Length;i++){
				if(i!=0){
					regexp+="[^0-9]*";//zero or more intervening digits that are not numbers
				}
				if(i==3) {//If there is more than three digits and the first digit is 1, make it optional.
					if(phonedigits.StartsWith("1")) {
						regexp="1?"+regexp.Substring(1);
					}
					else {
						regexp="1?[^0-9]*"+regexp;//add a leading 1 so that 1-800 numbers can show up simply by typing in 800 followed by the number.
					}
				}
				regexp+=phonedigits[i];
			}
			string command="SELECT DISTINCT patient.PatNum,patient.LName,patient.FName,patient.MiddleI,patient.Preferred,patient.Birthdate,patient.SSN"
				+",patient.HmPhone,patient.WkPhone,patient.Address,patient.PatStatus,patient.BillingType,patient.ChartNumber,patient.City,patient.State"
				+",patient.PriProv,patient.SiteNum,patient.Email,patient.Country,patient.ClinicNum "
				+",patient.SecProv,patient.WirelessPhone "
				+","+IsExactMatchSnippet+" AS isExactMatch ";
			if(PrefC.GetBool(PrefName.DistributorKey)) {//if for OD HQ, so never going to be Oracle
				command+=",GROUP_CONCAT(DISTINCT phonenumber.PhoneNumberVal) AS OtherPhone ";//this customer might have multiple extra phone numbers that match the param.
				command+=",registrationkey.RegKey ";
			}
			if(invoiceNumber!="") {
				command+=",statement.StatementNum ";
			}
			else {
				//The query needs to always include the StatementNum column regardless.
				command+=",'' StatementNum ";
			}
			command+="FROM patient ";
			if(PrefC.GetBool(PrefName.DistributorKey)) {//if for OD HQ, so never going to be Oracle
				command+="LEFT JOIN phonenumber ON phonenumber.PatNum=patient.PatNum ";
				if(regexp!="") {
					command+="AND phonenumber.PhoneNumberVal REGEXP '"+POut.String(regexp)+"' ";
				}
				command+="LEFT JOIN registrationkey ON patient.PatNum=registrationkey.PatNum ";
			}
			if(subscriberId!=""){
				command+="LEFT JOIN patplan ON patplan.PatNum=patient.PatNum "
					+"LEFT JOIN inssub ON patplan.InsSubNum=inssub.InsSubNum ";
			}
			if(invoiceNumber!="") {
				command+="LEFT JOIN statement ON statement.PatNum=patient.PatNum AND statement.IsInvoice=TRUE ";
			}
			command+="WHERE patient.PatStatus NOT IN("+POut.Int((int)PatientStatus.Deleted);
			if(hideInactive) {
				command+=","+POut.Int((int)PatientStatus.Inactive);
			}
			if(!showArchived) {
				command+=","+POut.Int((int)PatientStatus.Archived)+","+POut.Int((int)PatientStatus.Deceased);
			}
			command+=") ";
			if(showMerged==false) { //Filters out patients who have been merged, as to not clog up the results.
				//Ex 1: Patient A has been merged into Patient B. Don't show Patient A.
				//Ex 2: Patient A has been merged into Patient B. Patient B is later merged into Patient A.  Don't show Patient B.
				//Ex 3: Patient A has been merged into Patient B. Patient C is later merged into Patient A.  Don't show Patient C.
				command+=@" AND patient.PatNum NOT IN (
					SELECT DISTINCT pl1.PatNumFrom 
					FROM patientlink pl1
					LEFT JOIN patientlink pl2 ON pl2.PatNumTo=pl1.PatNumFrom
						AND	pl2.DateTimeLink > pl1.DateTimeLink						
					WHERE pl2.PatNumTo IS NULL AND pl1.LinkType="+POut.Int((int)PatientLinkType.Merge)+") ";
			}
			if(DataConnection.DBtype==DatabaseType.MySql) {//LIKE is case insensitive in mysql.
				if(lname.Length>0) {
					if(limit) {//normal behavior is fast
						if(PrefC.GetBool(PrefName.DistributorKey)) {
							command+="AND (patient.LName LIKE '"+POut.String(lname)+"%' OR patient.Preferred LIKE '"+POut.String(lname)+"%') ";
						}
						else {
							command+="AND patient.LName LIKE '"+POut.String(lname)+"%' ";
						}
					}
					else {//slower, but more inclusive.  User explicitly looking for all matches.
						if(PrefC.GetBool(PrefName.DistributorKey)) {
							command+="AND (patient.LName LIKE '%"+POut.String(lname)+"%' OR patient.Preferred LIKE '%"+POut.String(lname)+"%') ";
						}
						else {
							command+="AND patient.LName LIKE '%"+POut.String(lname)+"%' ";
						}
					}
				}
				if(fname.Length>0){
					if(PrefC.GetBool(PrefName.DistributorKey) || PrefC.GetBool(PrefName.PatientSelectUseFNameForPreferred)) {
						//Nathan has approved the preferred name search for first name only. It is not intended to work with last name for our customers.
						command+="AND (patient.FName LIKE '"+POut.String(fname)+"%' OR patient.Preferred LIKE '"+POut.String(fname)+"%') ";
					}
					else {
						command+="AND patient.FName LIKE '"+POut.String(fname)+"%' ";
					}
				}
			}
			else {//oracle, case matters in a like statement
				if(lname.Length>0) {
					if(limit) {
						command+="AND LOWER(patient.LName) LIKE '"+POut.String(lname)+"%' ";
					}
					else {
						command+="AND LOWER(patient.LName) LIKE '%"+POut.String(lname)+"%' ";
					}
				}
				if(fname.Length>0) {
					if(PrefC.GetBool(PrefName.PatientSelectUseFNameForPreferred)) {
						command+="AND (LOWER(patient.FName) LIKE '"+POut.String(fname)+"%' OR LOWER(patient.Preferred) LIKE '"+POut.String(fname)+"%') ";
					}
					else {
						command+="AND LOWER(patient.FName) LIKE '"+POut.String(fname)+"%' ";
					}
				}
			}
			if(regexp!="") {
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command+="AND (patient.HmPhone REGEXP '"+POut.String(regexp)+"' "
						+"OR patient.WkPhone REGEXP '"+POut.String(regexp)+"' "
						+"OR patient.WirelessPhone REGEXP '"+POut.String(regexp)+"' ";
					if(PrefC.GetBool(PrefName.DistributorKey)) {//if for OD HQ, so never going to be Oracle
						command+="OR phonenumber.PhoneNumberVal REGEXP '"+POut.String(regexp)+"' ";
					}
					command+=") ";
				}
				else {//oracle
					command+="AND ((SELECT REGEXP_INSTR(p.HmPhone,'"+POut.String(regexp)+"') FROM dual)<>0"
						+"OR (SELECT REGEXP_INSTR(p.WkPhone,'"+POut.String(regexp)+"') FROM dual)<>0 "
						+"OR (SELECT REGEXP_INSTR(p.WirelessPhone,'"+POut.String(regexp)+"') FROM dual)<>0) ";
				}
			}
			//Do a mathematical comparison for the patNumStr.
			command+=DbHelper.LongBetween("patient.PatNum",patNumStr);
			//Do a mathematical comparison for the invoiceNumber.
			command+=DbHelper.LongBetween("statement.StatementNum",invoiceNumber);
			//Replaces spaces and punctation with wildcards because users should be able to type the following example and match certain addresses:
			//Search term: "4145 S Court St" should match "4145 S. Court St." in the database.
			string strAddress=Regex.Replace(POut.String(address), @"[�\-.,:;_""'/\\)(#\s&]","%");
			char[] arrayRegKeyChars=regKey.Where(x => char.IsLetterOrDigit(x)).ToArray();
			string strRegKey=new string(arrayRegKeyChars);
			if(DataConnection.DBtype==DatabaseType.MySql) {
				if(PrefC.IsODHQ) {
					//Search both Address and Address2 for HQ
					command+=(strAddress.Length>0 ? "AND (patient.Address LIKE '%"+strAddress
						+"%' OR patient.Address2 LIKE '%"+strAddress+"%') " : "");//LIKE is case insensitive in mysql.
				}
				else {
					command+=(strAddress.Length>0 ? "AND patient.Address LIKE '%"+strAddress+"%' " : "");//LIKE is case insensitive in mysql
				}
				command+=(city.Length>0?"AND patient.City LIKE '"+POut.String(city)+"%' " : "")//LIKE is case insensitive in mysql.
					+(state.Length>0?"AND patient.State LIKE '"+POut.String(state)+"%' ":"")//LIKE is case insensitive in mysql.
					+(ssn.Length>0?"AND patient.SSN LIKE '"+POut.String(ssn)+"%' ":"")//LIKE is case insensitive in mysql.
					//+(patNumStr.Length>0?"AND patient.PatNum LIKE '"+POut.String(patNumStr)+"%' ":"")//LIKE is case insensitive in mysql.
					+(chartnumber.Length>0?"AND patient.ChartNumber LIKE '"+POut.String(chartnumber)+"%' ":"")//LIKE is case insensitive in mysql.
					+(email.Length>0?"AND patient.Email LIKE '%"+POut.String(email)+"%' ":"")//LIKE is case insensitive in mysql.
					+(country.Length>0?"AND patient.Country LIKE '%"+POut.String(country)+"%' ":"")//LIKE is case insensitive in mysql.
					+(regKey.Length>0?"AND registrationkey.RegKey LIKE '%"+POut.String(strRegKey)+"%' ":"");//LIKE is case insensitive in mysql.
			}
			else {//oracle
				command+=(address.Length>0 ? "AND LOWER(patient.Address) LIKE '%"+strAddress.ToLower()+"%' " : "")//case matters in a like statement in oracle.
					+(city.Length>0?"AND LOWER(patient.City) LIKE '"+POut.String(city).ToLower()+"%' ":"")//case matters in a like statement in oracle.
					+(state.Length>0?"AND LOWER(patient.State) LIKE '"+POut.String(state).ToLower()+"%' ":"")//case matters in a like statement in oracle.
					+(ssn.Length>0?"AND LOWER(patient.SSN) LIKE '"+POut.String(ssn).ToLower()+"%' ":"")//In case an office uses this field for something else.
					//+(patNumStr.Length>0?"AND patient.PatNum LIKE '"+POut.String(patNumStr)+"%' ":"")//case matters in a like statement in oracle.
					+(chartnumber.Length>0?"AND LOWER(patient.ChartNumber) LIKE '"+POut.String(chartnumber).ToLower()+"%' ":"")//case matters in a like statement in oracle.
					+(email.Length>0?"AND LOWER(patient.Email) LIKE '%"+POut.String(email).ToLower()+"%' ":"")//case matters in a like statement in oracle.
					+(country.Length>0?"AND LOWER(patient.Country) LIKE '%"+POut.String(country).ToLower()+"%' ":"")//case matters in a like statement in oracle.
					+(regKey.Length>0?"AND LOWER(registrationkey.RegKey) LIKE '%"+POut.String(strRegKey).ToLower()+"%' ":"");//case matters in a like statement in oracle.
			}
			if(birthdate.Year>1880 && birthdate.Year<2100){
				command+="AND patient.Birthdate ="+POut.Date(birthdate)+" ";
			}
			command+=billingsnippet;
			//if(showProspectiveOnly) {
			//	command+="AND patient.PatStatus = "+POut.Int((int)PatientStatus.Prospective)+" ";
			//}
			//if(!showProspectiveOnly) {
			//	command+="AND patient.PatStatus != "+POut.Int((int)PatientStatus.Prospective)+" ";
			//}
			if(guarOnly){
				command+="AND patient.PatNum = patient.Guarantor ";
			}
			if(clinicNums!="") {
				//Checking for completed or TP procedures was taking too long for large databases. We may revisit this in the future.
				//command+="AND patient.Guarantor IN ( "
				//+"SELECT DISTINCT Guarantor FROM patient "
				//+"LEFT JOIN procedurelog ON patient.PatNum=procedurelog.PatNum "
				//	+"AND (procedurelog.ProcStatus="+POut.Int((int)ProcStat.TP)+" OR procedurelog.ProcStatus="+POut.Int((int)ProcStat.C)+") "
				//	+"AND procedurelog.ClinicNum IN ("+POut.String(clinicNums)+") "
				//+"WHERE patient.PatStatus !="+POut.Int((int)PatientStatus.Deleted)+" "
				//+"AND (procedurelog.PatNum IS NOT NULL OR patient.ClinicNum IN (0,"+POut.String(clinicNums)+"))) "; //Includes patients that are not assigned to any clinic.  May need to restrict selection of these patients in the future.
				//Only include patients who are assigned to the clinic and also patients who are not assigned to any clinic
				command+="AND (patient.ClinicNum IN (0,"+POut.String(clinicNums)+") ";
				command+="OR patient.PatNum IN (SELECT patnum FROM appointment WHERE clinicnum IN ("+POut.String(clinicNums)+"))) ";
			}
			if(siteNum>0) {
				command+="AND patient.SiteNum="+POut.Long(siteNum)+" ";
			}
			if(subscriberId!=""){
				command+="AND inssub.SubscriberId LIKE '"+POut.String(subscriberId)+"%' ";
			}
			//NOTE: This filter will superceed all filters set above.
			if(explicitPatNums!=null && explicitPatNums.Count>0) {
				command+="AND FALSE ";//negate all filters above and select patients based solely on being in explicitPatNums
				command+="OR patient.PatNum IN ("+string.Join(",",explicitPatNums)+") ";
			}
			if(PrefC.GetBool(PrefName.DistributorKey)) { //if for OD HQ
				command+="GROUP BY patient.PatNum ";
			}
			if(initialPatNum!=0 && limit) {
				command+="ORDER BY patient.PatNum="+POut.Long(initialPatNum)+" DESC,"+IsExactMatchSnippet+" DESC,patient.LName,patient.FName ";//Make sure initial patnum is in results
			}
			else {
				command+="ORDER BY "+IsExactMatchSnippet+" DESC,patient.LName,patient.FName ";
			}
			if(limit) {
				command=DbHelper.LimitOrderBy(command,40);
			}
			DataTable table=Db.GetTable(command);
			List<string> listPatNums=new List<string>();
			if(limit) {//if the user hasn't hit "get all..."
				listPatNums=table.Select().Select(x => x["PatNum"].ToString()).ToList();
			}
			Dictionary<string,string> dictNextApts=new Dictionary<string,string>();
			Dictionary<string,string> dictLastApts=new Dictionary<string,string>();
			if(listPatNums.Count>0) {
				command=@"SELECT appointment.PatNum,MIN(appointment.AptDateTime) AS NextVisit 
					FROM appointment 
					WHERE appointment.AptStatus="+POut.Int((int)ApptStatus.Scheduled)
				  +" AND appointment.AptDateTime>= "+DbHelper.Now() //query for next visits
				  +" AND appointment.PatNum IN ("+string.Join(",",listPatNums)+")"
				  +" GROUP BY appointment.PatNum ";
				DataTable nextAptTable=Db.GetTable(command); //get table from database.
				for(int i=0;i<nextAptTable.Rows.Count;i++) { //put all of the results in a dictionary.
					dictNextApts.Add(nextAptTable.Rows[i]["PatNum"].ToString(),nextAptTable.Rows[i]["NextVisit"].ToString()); //Key: PatNum, Value: NextVisit
				}
				command=@"SELECT appointment.PatNum,MAX(appointment.AptDateTime) AS LastVisit 
					FROM appointment 
					WHERE appointment.AptStatus="+POut.Int((int)ApptStatus.Complete)
					+" AND appointment.AptDateTime<= "+DbHelper.Now()
					+" AND appointment.PatNum IN ("+string.Join(",",listPatNums)+")"
					+" GROUP BY appointment.PatNum ";
				DataTable lastAptTable=Db.GetTable(command); //get table from database.
				for(int i=0;i<lastAptTable.Rows.Count;i++) { //put into dictionary.
					dictLastApts.Add(lastAptTable.Rows[i]["PatNum"].ToString(),lastAptTable.Rows[i]["LastVisit"].ToString()); //Key:PatNum, Value: LastVisit
				}
			}
			DataTable PtDataTable=table.Clone();//does not copy any data
			PtDataTable.TableName="table";
			PtDataTable.Columns.Add("age");
			PtDataTable.Columns.Add("clinic");
			PtDataTable.Columns.Add("site");
			//lastVisit and nextVisit are not part of PtDataTable and need to be added manually from the corresponding dictionary.
			PtDataTable.Columns.Add("lastVisit");
			PtDataTable.Columns.Add("nextVisit");
			for(int i=0;i<PtDataTable.Columns.Count;i++){
				PtDataTable.Columns[i].DataType=typeof(string);
			}
			//if(limit && table.Rows.Count==36){
			//	retval=true;
			//}
			DataRow r;
			DateTime date;
			foreach(DataRow dRow in table.Rows){
				r=PtDataTable.NewRow();
				//PatNum,LName,FName,MiddleI,Preferred,Birthdate,SSN,HmPhone,WkPhone,Address,PatStatus"
				//+",BillingType,ChartNumber,City,State
				r["PatNum"]=dRow["PatNum"].ToString();
				r["LName"]=dRow["LName"].ToString();
				r["FName"]=dRow["FName"].ToString();
				r["MiddleI"]=dRow["MiddleI"].ToString();
				r["Preferred"]=dRow["Preferred"].ToString();
				date=PIn.Date(dRow["Birthdate"].ToString());
				if(date.Year>1880){
					r["age"]=DateToAge(date);
					r["Birthdate"]=date.ToShortDateString();
				}
				else{
					r["age"]="";
					r["Birthdate"]="";
				}
				r["SSN"]=dRow["SSN"].ToString();
				r["HmPhone"]=dRow["HmPhone"].ToString();
				r["WkPhone"]=dRow["WkPhone"].ToString();
				r["Address"]=dRow["Address"].ToString();
				r["PatStatus"]=((PatientStatus)PIn.Long(dRow["PatStatus"].ToString())).ToString();
				r["BillingType"]=Defs.GetName(DefCat.BillingTypes,PIn.Long(dRow["BillingType"].ToString()));
				r["ChartNumber"]=dRow["ChartNumber"].ToString();
				r["City"]=dRow["City"].ToString();
				r["State"]=dRow["State"].ToString();
				r["PriProv"]=Providers.GetAbbr(PIn.Long(dRow["PriProv"].ToString()));
				r["site"]=Sites.GetDescription(PIn.Long(dRow["SiteNum"].ToString()));
				r["Email"]=dRow["Email"].ToString();
				r["Country"]=dRow["Country"].ToString();
				r["clinic"]=Clinics.GetAbbr(PIn.Long(dRow["ClinicNum"].ToString()));
				if(PrefC.GetBool(PrefName.DistributorKey)) {//if for OD HQ
					r["OtherPhone"]=dRow["OtherPhone"].ToString();
					r["RegKey"]=dRow["RegKey"].ToString();
				}
				r["StatementNum"]=dRow["StatementNum"].ToString();
				r["WirelessPhone"]=dRow["WirelessPhone"].ToString();
				r["SecProv"]=Providers.GetAbbr(PIn.Long(dRow["SecProv"].ToString()));
				r["lastVisit"]="";
				if(dictLastApts.ContainsKey(dRow["PatNum"].ToString())) {
					date=PIn.Date(dictLastApts[dRow["PatNum"].ToString()]);
					if(date.Year>1880) {//if the date is valid
						r["lastVisit"]=date.ToShortDateString();
					}
				}
				r["nextVisit"]="";
				if(dictNextApts.ContainsKey(dRow["PatNum"].ToString())) {
					date=PIn.Date(dictNextApts[dRow["PatNum"].ToString()]);
					if(date.Year>1880) {//if the date is valid
						r["nextVisit"]=date.ToShortDateString();
					}
				}
				r["isExactMatch"]=dRow["isExactMatch"].ToString();
				PtDataTable.Rows.Add(r);
			}
			return PtDataTable;
		}

		///<summary>Constructs a query snippet that returns true (The snippet, not this method.) When used in the patient search query. 
		///Note: some of the clauses of this query are dependent on the join clauses of the query constructed in GetPtDataTable().</summary>
		private static string GetExactMatchSnippet(PatientSearchArgs args) {
			//No need to check RemotingRole; private method and no call to db.
			if(DataConnection.DBtype!=DatabaseType.MySql) {//Oracle
				return "'0'";
			}
			List<string> listClauses=new List<string>();
			listClauses.Add(string.IsNullOrEmpty(args.lname) ? "" : "(LName='"+args.lname+"')");
			listClauses.Add(string.IsNullOrEmpty(args.fname) ? "" : "(FName='"+args.fname+"')");
			listClauses.Add(string.IsNullOrEmpty(args.phone) ? "" : 
				"(WirelessPhone='"+args.phone+"' OR HmPhone='"+args.phone+"' OR WkPhone='"+args.phone+"'"
				+(!PrefC.GetBool(PrefName.DistributorKey)?"": " OR phonenumber.PhoneNumberVal='"+args.phone+"'") //Join
				+")");
			listClauses.Add(string.IsNullOrEmpty(args.address) ? "" : "(Address='"+args.address+"')");
			listClauses.Add(string.IsNullOrEmpty(args.city) ? "" : "(City='"+args.city+"')");
			listClauses.Add(string.IsNullOrEmpty(args.state) ? "" : "(State='"+args.state+"')");
			listClauses.Add(string.IsNullOrEmpty(args.ssn) ? "" : "(Ssn='"+args.ssn+"')");
			listClauses.Add(string.IsNullOrEmpty(args.patNumStr) ? "" : "(patient.PatNum='"+args.patNumStr+"')");
			listClauses.Add(string.IsNullOrEmpty(args.chartnumber) ? "" : "(ChartNumber='"+args.chartnumber+"')");
			listClauses.Add(string.IsNullOrEmpty(args.subscriberId) ? "" : "(SubscriberId='"+args.subscriberId+"')"); //Join
			listClauses.Add(string.IsNullOrEmpty(args.email) ? "" : "(Email='"+args.email+"')");
			listClauses.Add(string.IsNullOrEmpty(args.country) ? "" : "(Country='"+args.country+"')");
			listClauses.Add((string.IsNullOrEmpty(args.regKey) || !PrefC.GetBool(PrefName.DistributorKey)) ? "" : "(RegKey='"+args.regKey+"')"); //Join
			listClauses.Add(args.birthdate.Year<1880 ? "" : "(Birthdate="+POut.Date(args.birthdate)+")");
			listClauses.Add(string.IsNullOrEmpty(args.invoicenumber) ? "" : "(StatementNum='"+args.invoicenumber+"')");
			listClauses.RemoveAll(string.IsNullOrEmpty);
			if(listClauses.Count>0) {
				return "("+string.Join(" AND ",listClauses)+")";
			}
			return "'0'";
		}

		///<summary>All strings should be escaped using POut.string().</summary>
		private class PatientSearchArgs {
			public bool limit;
			public string lname;
			public string fname;
			public string phone;
			public string address;
			public bool hideInactive;
			public string city;
			public string state;
			public string ssn;
			public string patNumStr;
			public string chartnumber;
			public long billingtype;
			public bool guarOnly;
			public bool showArchived;
			public DateTime birthdate;
			public long siteNum;
			public string subscriberId;
			public string email;
			public string country;
			public string invoicenumber;
			public string regKey;
		}

		public static bool HasPatientPortalAccess(long patNum) {
			UserWeb uwCur=UserWebs.GetByFKeyAndType(patNum,UserWebFKeyType.PatientPortal);
			if(uwCur!=null && uwCur.Password!="") {
				return true;
			}
			return false;
		}

		///<summary>Used when filling appointments for an entire day. Gets a list of Pats, multPats, of all the specified patients.  Then, use GetOnePat to pull one patient from this list.  This process requires only one call to the database.</summary>
		public static Patient[] GetMultPats(List<long> patNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Patient[]>(MethodBase.GetCurrentMethod(),patNums);
			}
			DataTable table=new DataTable();
			if(patNums.Count>0) {
				string command="SELECT * FROM patient WHERE PatNum IN ("+String.Join<long>(",",patNums)+") ";
				table=Db.GetTable(command);
			}
			Patient[] multPats=Crud.PatientCrud.TableToList(table).ToArray();
			return multPats;
		}

		///<summary>Get all patients who have a corresponding entry in the RegistrationKey table. DO NOT REMOVE! Used by OD WebApps solution.</summary>
		public static List<Patient> GetPatientsWithRegKeys() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM patient WHERE PatNum IN (SELECT PatNum FROM registrationkey)";
			return Crud.PatientCrud.SelectMany(command);
		}

		///<summary>First call GetMultPats to fill the list of multPats. Then, use this to return one patient from that list.</summary>
		public static Patient GetOnePat(Patient[] multPats,long patNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<multPats.Length;i++){
				if(multPats[i].PatNum==patNum){
					return multPats[i];
				}
			}
			return new Patient();
		}

		/// <summary>Gets nine of the most useful fields from the db for the given patnum.  If invalid PatNum, returns new Patient rather than null.</summary>
		public static Patient GetLim(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Patient>(MethodBase.GetCurrentMethod(),patNum);
			}
			if(patNum==0){
				return new Patient();
			}
			string command= 
				"SELECT PatNum,LName,FName,MiddleI,Preferred,CreditType,Guarantor,HasIns,SSN " 
				+"FROM patient "
				+"WHERE PatNum = '"+patNum.ToString()+"'";
 			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0){
				return new Patient();
			}
			Patient Lim=new Patient();
			Lim.PatNum     = PIn.Long   (table.Rows[0][0].ToString());
			Lim.LName      = PIn.String(table.Rows[0][1].ToString());
			Lim.FName      = PIn.String(table.Rows[0][2].ToString());
			Lim.MiddleI    = PIn.String(table.Rows[0][3].ToString());
			Lim.Preferred  = PIn.String(table.Rows[0][4].ToString());
			Lim.CreditType = PIn.String(table.Rows[0][5].ToString());
			Lim.Guarantor  = PIn.Long   (table.Rows[0][6].ToString());
			Lim.HasIns     = PIn.String(table.Rows[0][7].ToString());
			Lim.SSN        = PIn.String(table.Rows[0][8].ToString());
			return Lim;
		}

		///<summary></summary>
		public static SerializableDictionary<long,string> GetStatesForPats(List<long> listPatNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<SerializableDictionary<long,string>>(MethodBase.GetCurrentMethod(),listPatNums);
			}
			SerializableDictionary<long,string> retVal=new SerializableDictionary<long,string>();
			if(listPatNums==null || listPatNums.Count < 1) {
				return new SerializableDictionary<long,string>();
			}
			string command= "SELECT PatNum,State " 
				+"FROM patient "
				+"WHERE PatNum IN ("+string.Join(",",listPatNums)+")";
			DataTable table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				Patient patLim=new Patient();
				patLim.PatNum     = PIn.Long	(table.Rows[i]["PatNum"].ToString());
				patLim.State      = PIn.String(table.Rows[i]["State"].ToString());
				retVal.Add(patLim.PatNum,patLim.State);
			}
			return retVal;
		}

		///<summary>Gets nine of the most useful fields from the db for the given PatNums.</summary>
		public static List<Patient> GetLimForPats(List<long> listPatNums) {
			if(listPatNums==null || listPatNums.Count < 1) {
				return new List<Patient>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),listPatNums);
			}
			List<Patient> retVal=new List<Patient>();
			string command= "SELECT PatNum,LName,FName,MiddleI,Preferred,CreditType,Guarantor,HasIns,SSN " 
				+"FROM patient "
				+"WHERE PatNum IN ("+string.Join(",",listPatNums)+")";
			DataTable table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				Patient patLim=new Patient();
				patLim.PatNum     = PIn.Long	(table.Rows[i]["PatNum"].ToString());
				patLim.LName      = PIn.String(table.Rows[i]["LName"].ToString());
				patLim.FName      = PIn.String(table.Rows[i]["FName"].ToString());
				patLim.MiddleI    = PIn.String(table.Rows[i]["MiddleI"].ToString());
				patLim.Preferred  = PIn.String(table.Rows[i]["Preferred"].ToString());
				patLim.CreditType = PIn.String(table.Rows[i]["CreditType"].ToString());
				patLim.Guarantor  = PIn.Long	(table.Rows[i]["Guarantor"].ToString());
				patLim.HasIns     = PIn.String(table.Rows[i]["HasIns"].ToString());
				patLim.SSN        = PIn.String(table.Rows[i]["SSN"].ToString());
				retVal.Add(patLim);
			}
			return retVal;
		}
		
		///<summary>Gets the patient and provider balances for all patients in the family.  Used from the payment window to help visualize and automate the family splits.</summary>
		public static DataTable GetPaymentStartingBalances(long guarNum,long excludePayNum) {
			return GetPaymentStartingBalances(guarNum,excludePayNum,false);
		}

		///<summary>Gets the patient and provider balances for all patients in the family.  Used from the payment window to help visualize and automate the family splits. groupByProv means group by provider only not provider/clinic.</summary>
		public static DataTable GetPaymentStartingBalances(long guarNum,long excludePayNum,bool groupByProv) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),guarNum,excludePayNum,groupByProv);
			}
			//This method no longer uses a temporary table due to the problems it was causing replication users.
			//The in-memory table name was left as "tempfambal" for nostalgic purposes because veteran engineers know exactly where to look when "tempfambal" is mentioned.
			//This query will be using UNION ALLs so that duplicate-row removal does not occur. 
			string command=@"
					SELECT tempfambal.PatNum,tempfambal.ProvNum,
						tempfambal.ClinicNum,ROUND(SUM(tempfambal.AmtBal),3) StartBal,
						ROUND(SUM(tempfambal.AmtBal-tempfambal.InsEst),3) AfterIns,patient.FName,patient.Preferred,0.0 EndBal,
						CASE WHEN patient.Guarantor!=patient.PatNum THEN 1 ELSE 0 END IsNotGuar,patient.Birthdate,tempfambal.UnearnedType
					FROM(
						/*Completed procedures*/
						(SELECT patient.PatNum,procedurelog.ProvNum,procedurelog.ClinicNum,
						SUM(procedurelog.ProcFee*(procedurelog.UnitQty+procedurelog.BaseUnits)) AmtBal,0 InsEst,0 UnearnedType
						FROM procedurelog,patient
						WHERE patient.PatNum=procedurelog.PatNum
						AND procedurelog.ProcStatus="+POut.Int((int)ProcStat.C)+@"
						AND patient.Guarantor="+POut.Long(guarNum)+@"
						GROUP BY patient.PatNum,procedurelog.ProvNum,procedurelog.ClinicNum)
					UNION ALL			
						/*Received insurance payments*/
						(SELECT patient.PatNum,claimproc.ProvNum,claimproc.ClinicNum,-SUM(claimproc.InsPayAmt)-SUM(claimproc.Writeoff) AmtBal,0 InsEst,0 UnearnedType
						FROM claimproc,patient
						WHERE patient.PatNum=claimproc.PatNum
						AND (claimproc.Status="+POut.Int((int)ClaimProcStatus.Received)+@" 
							OR claimproc.Status="+POut.Int((int)ClaimProcStatus.Supplemental)+@" 
							OR claimproc.Status="+POut.Int((int)ClaimProcStatus.CapClaim)+@" 
							OR claimproc.Status="+POut.Int((int)ClaimProcStatus.CapComplete)+@")
						AND patient.Guarantor="+POut.Long(guarNum)+@"
						AND claimproc.PayPlanNum = 0
						GROUP BY patient.PatNum,claimproc.ProvNum,claimproc.ClinicNum)
					UNION ALL
						/*Insurance estimates*/
						(SELECT patient.PatNum,claimproc.ProvNum,claimproc.ClinicNum,0 AmtBal,SUM(claimproc.InsPayEst)+SUM(claimproc.Writeoff) InsEst,0 UnearnedType
						FROM claimproc,patient
						WHERE patient.PatNum=claimproc.PatNum
						AND claimproc.Status="+POut.Int((int)ClaimProcStatus.NotReceived)+@"
						AND patient.Guarantor="+POut.Long(guarNum)+@"
						GROUP BY patient.PatNum,claimproc.ProvNum,claimproc.ClinicNum)
					UNION ALL
						/*Adjustments*/
						(SELECT patient.PatNum,adjustment.ProvNum,adjustment.ClinicNum,SUM(adjustment.AdjAmt) AmtBal,0 InsEst,0 UnearnedType
						FROM adjustment,patient
						WHERE patient.PatNum=adjustment.PatNum
						AND patient.Guarantor="+POut.Long(guarNum)+@"
						GROUP BY patient.PatNum,adjustment.ProvNum,adjustment.ClinicNum)
					UNION ALL
						/*Patient payments*/
						(SELECT patient.PatNum,paysplit.ProvNum,paysplit.ClinicNum,-SUM(SplitAmt) AmtBal,0 InsEst,paysplit.UnearnedType
						FROM paysplit,patient
						WHERE patient.PatNum=paysplit.PatNum
						AND paysplit.PayNum!="+POut.Long(excludePayNum)+@"
						AND patient.Guarantor="+POut.Long(guarNum);
			if(PrefC.GetInt(PrefName.PayPlansVersion)==1) { //for payplans v1, exclude paysplits attached to payplans
				command+=@"
						AND paysplit.PayPlanNum=0 ";
			}
			command+=@"
						GROUP BY patient.PatNum,paysplit.ProvNum,paysplit.ClinicNum)
					UNION ALL	
						(SELECT patient.PatNum,payplancharge.ProvNum,payplancharge.ClinicNum,-payplan.CompletedAmt ";
			if(PrefC.GetInt(PrefName.PayPlansVersion)==2) {
				command+="+ SUM(CASE WHEN payplancharge.ChargeType="+POut.Int((int)PayPlanChargeType.Debit)+@"
						AND payplancharge.ChargeDate <= CURDATE() THEN payplancharge.Principal + payplancharge.Interest ELSE 0 END) ";
			}
			command+=@"AmtBal,0 InsEst,0 UnearnedType
						FROM payplancharge
						INNER JOIN payplan ON payplan.PayPlanNum=payplancharge.PayPlanNum
						INNER JOIN patient ON patient.PatNum=payplancharge.PatNum AND patient.Guarantor="+POut.Long(guarNum)+@"
						GROUP BY payplan.PayPlanNum,payplan.CompletedAmt,patient.PatNum,payplancharge.ProvNum,payplancharge.ClinicNum)
					) tempfambal,patient
					WHERE tempfambal.PatNum=patient.PatNum 
					GROUP BY tempfambal.PatNum,tempfambal.ProvNum,";
			if(!groupByProv) {
				command+=@"tempfambal.ClinicNum,";
			}
			command+=@"patient.FName,patient.Preferred";
			//Probably an unnecessary MySQL / Oracle split but I didn't want to affect the old GROUP BY functionality for MySQL just be Oracle is lame.
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+=@"
					HAVING ((StartBal>0.005 OR StartBal<-0.005) OR (AfterIns>0.005 OR AfterIns<-0.005))
					ORDER BY IsNotGuar,Birthdate,ProvNum,FName,Preferred";
			}
			else {//Oracle.
				command+=@",(CASE WHEN Guarantor!=patient.PatNum THEN 1 ELSE 0 END),Birthdate
					HAVING ((SUM(AmtBal)>0.005 OR SUM(AmtBal)<-0.005) OR (SUM(AmtBal-tempfambal.InsEst)>0.005 OR SUM(AmtBal-tempfambal.InsEst)<-0.005))
					ORDER BY IsNotGuar,patient.Birthdate,tempfambal.ProvNum,patient.FName,patient.Preferred";
			}
			return Db.GetTable(command);
		}

		///<summary></summary>
		public static void ChangeGuarantorToCur(Family Fam,Patient Pat){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),Fam,Pat);
				return;
			}
			//Move famfinurgnote to current patient:
			string command= "UPDATE patient SET "
				+"FamFinUrgNote = '"+POut.String(Fam.ListPats[0].FamFinUrgNote)+"' "
				+"WHERE PatNum = "+POut.Long(Pat.PatNum);
 			Db.NonQ(command);
			command= "UPDATE patient SET FamFinUrgNote = '' "
				+"WHERE PatNum = '"+Pat.Guarantor.ToString()+"'";
			Db.NonQ(command);
			//Move family financial note to current patient:
			command="SELECT FamFinancial FROM patientnote "
				+"WHERE PatNum = "+POut.Long(Pat.Guarantor);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==1){
				command= "UPDATE patientnote SET "
					+"FamFinancial = '"+POut.String(table.Rows[0][0].ToString())+"' "
					+"WHERE PatNum = "+POut.Long(Pat.PatNum);
				Db.NonQ(command);
			}
			command= "UPDATE patientnote SET FamFinancial = '' "
				+"WHERE PatNum = "+POut.Long(Pat.Guarantor);
			Db.NonQ(command);
			//change guarantor of all family members:
			command= "UPDATE patient SET "
				+"Guarantor = "+POut.Long(Pat.PatNum)
				+" WHERE Guarantor = "+POut.Long(Pat.Guarantor);
			Db.NonQ(command);
		}
		
		///<summary></summary>
		public static void CombineGuarantors(Family Fam,Patient Pat){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),Fam,Pat);
				return;
			}
			//concat cur notes with guarantor notes
			string command= 
				"UPDATE patient SET "
				//+"addrnote = '"+POut.PString(FamilyList[GuarIndex].FamAddrNote)
				//									+POut.PString(cur.FamAddrNote)+"', "
				+"famfinurgnote = '"+POut.String(Fam.ListPats[0].FamFinUrgNote)
				+POut.String(Pat.FamFinUrgNote)+"' "
				+"WHERE patnum = '"+Pat.Guarantor.ToString()+"'";
 			Db.NonQ(command);
			//delete cur notes
			command= 
				"UPDATE patient SET "
				//+"famaddrnote = '', "
				+"famfinurgnote = '' "
				+"WHERE patnum = '"+Pat.PatNum+"'";
			Db.NonQ(command);
			//concat family financial notes
			PatientNote PatientNoteCur=PatientNotes.Refresh(Pat.PatNum,Pat.Guarantor);
			//patientnote table must have been refreshed for this to work.
			//Makes sure there are entries for patient and for guarantor.
			//Also, PatientNotes.cur.FamFinancial will now have the guar info in it.
			string strGuar=PatientNoteCur.FamFinancial;
			command= 
				"SELECT famfinancial "
				+"FROM patientnote WHERE patnum ='"+POut.Long(Pat.PatNum)+"'";
			//MessageBox.Show(string command);
			DataTable table=Db.GetTable(command);
			string strCur=PIn.String(table.Rows[0][0].ToString());
			command= 
				"UPDATE patientnote SET "
				+"famfinancial = '"+POut.String(strGuar+strCur)+"' "
				+"WHERE patnum = '"+Pat.Guarantor.ToString()+"'";
			Db.NonQ(command);
			//delete cur financial notes
			command= 
				"UPDATE patientnote SET "
				+"famfinancial = ''"
				+"WHERE patnum = '"+Pat.PatNum.ToString()+"'";
			Db.NonQ(command);
		}

		///<summary>Key=patNum, value=formatted name.</summary>
		public static Dictionary<long,string> GetPatientNames(List<long> listPatNums) {
			return Patients.GetLimForPats(listPatNums)
				.ToDictionary(x => x.PatNum,x => x.GetNameLF());
		}

		///<summary>DEPRECATED. This method should not be used because it will take a long time on large databases. 
		///Call Patients.GetPatientNames() instead.
		///Key=patNum, value=formatted name.</summary>
		public static Dictionary<long,string> GetAllPatientNames() {
			//No need to check RemotingRole; no call to db.
			DataTable table=GetAllPatientNamesTable();
			Dictionary<long,string> dict=new Dictionary<long,string>();
			long patnum;
			string lname,fname,middlei,preferred;
			for(int i=0;i<table.Rows.Count;i++) {
				patnum=PIn.Long(table.Rows[i][0].ToString());
				lname=PIn.String(table.Rows[i][1].ToString());
				fname=PIn.String(table.Rows[i][2].ToString());
				middlei=PIn.String(table.Rows[i][3].ToString());
				preferred=PIn.String(table.Rows[i][4].ToString());
				if(preferred=="") {
					dict.Add(patnum,lname+", "+fname+" "+middlei);
				}
				else {
					dict.Add(patnum,lname+", '"+preferred+"' "+fname+" "+middlei);
				}
			}
			return dict;
		}

		public static DataTable GetAllPatientNamesTable() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod());
			}
			string command="SELECT patnum,lname,fname,middlei,preferred "
				+"FROM patient";
			DataTable table=Db.GetTable(command);
			return table;
		}

		///<summary>Useful when you expect to individually examine most of the patients in the database during a data import.  Excludes deleted patients.
		///Saves time and database calls to call this method once and keep a short term cache than it is to run a series of select statements.</summary>
		public static List<Patient> GetAllPatients() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM patient WHERE PatStatus != "+POut.Int((int)PatientStatus.Deleted);
			return Crud.PatientCrud.SelectMany(command);
		}

		public static bool SuperFamHasSameAddrPhone(Patient pat) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),pat);
			}
			string command="SELECT HmPhone,Address,Address2,City,State,Zip FROM patient WHERE PatNum="+POut.Long(pat.SuperFamily);
			DataTable result=Db.GetTable(command);
			command="SELECT COUNT(*) FROM patient WHERE SuperFamily="+POut.Long(pat.SuperFamily)+" "
				+"AND (HmPhone!='"+POut.String(result.Rows[0]["HmPhone"].ToString())+"' "
							+"OR Address!='"+POut.String(result.Rows[0]["Address"].ToString())+"' "
							+"OR Address2!='"+POut.String(result.Rows[0]["Address2"].ToString())+"' "
							+"OR City!='"+POut.String(result.Rows[0]["City"].ToString())+"' "
							+"OR State!='"+POut.String(result.Rows[0]["State"].ToString())+"' "
							+"OR Zip!='"+POut.String(result.Rows[0]["Zip"].ToString())+"')";
			if(PIn.Int(Db.GetCount(command))==0) {//Everybody in the superfamily has the same information
				return true;
			}
			return false;//At least one patient in the superfamily has different information
		}

		///<summary></summary>
		public static void UpdateAddressForFam(Patient pat){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),pat);
				return;
			}
			string command= "UPDATE patient SET " 
				+"Address = '"    +POut.String(pat.Address)+"'"
				+",Address2 = '"   +POut.String(pat.Address2)+"'"
				+",City = '"       +POut.String(pat.City)+"'"
				+",State = '"      +POut.String(pat.State)+"'"
				+",Country = '"    +POut.String(pat.Country)+"'"
				+",Zip = '"        +POut.String(pat.Zip)+"'"
				+",HmPhone = '"    +POut.String(pat.HmPhone)+"'"
				+" WHERE guarantor = '"+POut.Long(pat.Guarantor)+"'";
			Db.NonQ(command);
		}

		public static void UpdateAddressForSuperFam(Patient pat){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),pat);
				return;
			}
			string command= "UPDATE patient SET " 
				+"Address = '"    +POut.String(pat.Address)+"'"
				+",Address2 = '"   +POut.String(pat.Address2)+"'"
				+",City = '"       +POut.String(pat.City)+"'"
				+",State = '"      +POut.String(pat.State)+"'"
				+",Country = '"    +POut.String(pat.Country)+"'"
				+",Zip = '"        +POut.String(pat.Zip)+"'"
				+",HmPhone = '"    +POut.String(pat.HmPhone)+"'"
				+" WHERE SuperFamily = '"+POut.Long(pat.SuperFamily)+"'";
			Db.NonQ(command);
		}

		public static void UpdateBillingProviderForFam(Patient pat,bool isAuthPriProvEdit) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),pat,isAuthPriProvEdit);
				return;
			}
			string command="UPDATE patient SET "
				+"credittype      = '"+POut.String(pat.CreditType)+"',";
			if(isAuthPriProvEdit) {
				command+="priprov = "+POut.Long(pat.PriProv)+",";
			}
			command+=
				 "secprov         = "+POut.Long(pat.SecProv)+","
				+"feesched        = "+POut.Long(pat.FeeSched)+","
				+"billingtype     = "+POut.Long(pat.BillingType)+" "
				+"WHERE guarantor = "+POut.Long(pat.Guarantor);
			Db.NonQ(command);
		}

		///<summary>Used in patient terminal, aka sheet import.  Synchs less fields than the normal synch.</summary>
		public static void UpdateAddressForFamTerminal(Patient pat) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),pat);
				return;
			}
			string command= "UPDATE patient SET " 
				+"Address = '"    +POut.String(pat.Address)+"'"
				+",Address2 = '"   +POut.String(pat.Address2)+"'"
				+",City = '"       +POut.String(pat.City)+"'"
				+",State = '"      +POut.String(pat.State)+"'"
				+",Zip = '"        +POut.String(pat.Zip)+"'"
				+",HmPhone = '"    +POut.String(pat.HmPhone)+"'"
				+" WHERE guarantor = '"+POut.Long(pat.Guarantor)+"'";
			Db.NonQ(command);
		}

		///<summary></summary>
		public static void UpdateArriveEarlyForFam(Patient pat){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),pat);
				return;
			}
			string command= "UPDATE patient SET " 
				+"AskToArriveEarly = '"   +POut.Int(pat.AskToArriveEarly)+"'"
				+" WHERE guarantor = '"+POut.Long(pat.Guarantor)+"'";
			DataTable table=Db.GetTable(command);
		}

		///<summary></summary>
		public static void UpdateNotesForFam(Patient pat){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),pat);
				return;
			}
			string command= "UPDATE patient SET " 
				+"addrnote = '"   +POut.String(pat.AddrNote)+"'"
				+" WHERE guarantor = '"+POut.Long(pat.Guarantor)+"'";
			Db.NonQ(command);
		}

		///<summary>Only used from FormRecallListEdit.  Updates two fields for family if they are already the same for the entire family.  If they start out different for different family members, then it only changes the two fields for the single patient.</summary>
		public static void UpdatePhoneAndNoteIfNeeded(string newphone,string newnote,long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),newphone,newnote,patNum);
				return;
			}
			string command="SELECT Guarantor,HmPhone,AddrNote FROM patient WHERE Guarantor="
				+"(SELECT Guarantor FROM patient WHERE PatNum="+POut.Long(patNum)+")";
			DataTable table=Db.GetTable(command);
			bool phoneIsSame=true;
			bool noteIsSame=true;
			long guar=PIn.Long(table.Rows[0]["Guarantor"].ToString());
			for(int i=0;i<table.Rows.Count;i++){
				if(table.Rows[i]["HmPhone"].ToString()!=table.Rows[0]["HmPhone"].ToString()){
					phoneIsSame=false;
				}
				if(table.Rows[i]["AddrNote"].ToString()!=table.Rows[0]["AddrNote"].ToString()) {
					noteIsSame=false;
				}
			}
			command="UPDATE patient SET HmPhone='"+POut.String(newphone)+"' WHERE ";
			if(phoneIsSame){
				command+="Guarantor="+POut.Long(guar);
			}
			else{
				command+="PatNum="+POut.Long(patNum);
			}
			Db.NonQ(command);
			command="UPDATE patient SET AddrNote='"+POut.String(newnote)+"' WHERE ";
			if(noteIsSame) {
				command+="Guarantor="+POut.Long(guar);
			}
			else {
				command+="PatNum="+POut.Long(patNum);
			}
			Db.NonQ(command);
		}

		///<summary>Updates every family members' Email, WirelessPhone, and WkPhone to the passed in patient object.</summary>
		public static void UpdateEmailPhoneForFam(Patient pat) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),pat);
				return;
			}
			string command= "UPDATE patient SET "
				+"Email='"+POut.String(pat.Email)+"'"
				+",WirelessPhone='"+POut.String(pat.WirelessPhone)+"'"
				+",WkPhone='"+POut.String(pat.WkPhone)+"'"
				+" WHERE guarantor='"+POut.Long(pat.Guarantor)+"'";
			Db.NonQ(command);
		}

		///<summary>This is only used in the Billing dialog</summary>
		public static List<PatAging> GetAgingList(string age,DateTime lastStatement,List<long> billingNums,bool excludeAddr,
			bool excludeNeg,double excludeLessThan,bool excludeInactive,bool includeChanged,bool excludeInsPending,
			bool excludeIfUnsentProcs,bool ignoreInPerson,List<long> clinicNums,bool isSuperStatements,bool isSinglePatient)
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PatAging>>(MethodBase.GetCurrentMethod(),age,lastStatement,billingNums,excludeAddr
					,excludeNeg,excludeLessThan,excludeInactive,includeChanged,excludeInsPending
					,excludeIfUnsentProcs,ignoreInPerson,clinicNums,isSuperStatements,isSinglePatient);
			}
			if(DataConnection.DBtype!=DatabaseType.MySql) {
				//We are going to purposefully throw an exception so that users will call in and complain.
				throw new ApplicationException(Lans.g("Patients","Aging not currently supported by Oracle.  Please call us for support."));
			}
			List <int> listPatStatusExclude=new List<int>();
			listPatStatusExclude.Add((int)PatientStatus.Deleted);//Always hide deleted.
			if(excludeInactive){
				listPatStatusExclude.Add((int)PatientStatus.Inactive);
			}
			string guarOrPat="";
			if(isSinglePatient) {
				guarOrPat="pat";
			}
			else {
				guarOrPat="guar";
			}
			List<string> listWhereAnds=new List<string>();
			string strMinusIns="";
			if(!PrefC.GetBool(PrefName.BalancesDontSubtractIns)) {
				strMinusIns="-guar.InsEst";
			}
			string strBalExclude="(ROUND(guar.BalTotal"+strMinusIns+",3) >= ROUND("+POut.Double(excludeLessThan)+",3) OR guar.PayPlanDue > 0";
			if(!excludeNeg) {//include credits
				strBalExclude+=" OR ROUND(guar.BalTotal"+strMinusIns+",3) < 0";
			}
			strBalExclude+=")";
			listWhereAnds.Add(strBalExclude);
			switch(age){//age 0 means include all
				case "30":
					listWhereAnds.Add("(guar.Bal_31_60>0 OR guar.Bal_61_90>0 OR guar.BalOver90>0 OR guar.PayPlanDue>0)");
					break;
				case "60":
					listWhereAnds.Add("(guar.Bal_61_90>0 OR guar.BalOver90>0 OR guar.PayPlanDue>0)");
					break;
				case "90":
					listWhereAnds.Add("(guar.BalOver90>0 OR guar.PayPlanDue>0)");
					break;
			}
			if(billingNums.Count>0) {//if billingNums.Count==0, then we'll include all billing types
				listWhereAnds.Add("guar.BillingType IN ("+string.Join(",",billingNums.Select(x => POut.Long(x)))+")");
			}
			if(excludeAddr){
				listWhereAnds.Add("guar.Zip!=''");
			}
			if(clinicNums.Count>0) {
				listWhereAnds.Add("guar.ClinicNum IN ("+string.Join(",",clinicNums.Select(x => POut.Long(x)))+")");
			}
			listWhereAnds.Add("(guar.PatStatus!="+POut.Int((int)PatientStatus.Archived)+" OR ROUND(guar.BalTotal,3) != 0)");//Hide archived patients with PatBal=0.
			string command="";
			command="SELECT "+guarOrPat+".PatNum,"+guarOrPat+".FName,"+guarOrPat+".MiddleI,"+guarOrPat+".Preferred,"+guarOrPat+".LName,"+guarOrPat+".ClinicNum,guar.SuperFamily,"
				+"guar.HasSuperBilling,guar.BillingType,"
				+"guar.Bal_0_30,guar.Bal_31_60,guar.Bal_61_90,guar.BalOver90,guar.BalTotal,guar.InsEst,guar.PayPlanDue,"
				+"COALESCE(MAX(statement.DateSent),'0001-01-01') AS lastStatement "
				+"FROM patient guar "
				+"INNER JOIN patient pat ON guar.PatNum=pat.Guarantor AND pat.PatStatus NOT IN ("+string.Join(",",listPatStatusExclude)+") "
				+"LEFT JOIN statement ON "+guarOrPat+".PatNum=statement.PatNum "
					+(ignoreInPerson?("AND statement.Mode_!="+POut.Int((int)StatementMode.InPerson)+" "):"")
				+"WHERE "+string.Join(" AND ",listWhereAnds)+" "
				+"GROUP BY "+guarOrPat+".PatNum "
				+"ORDER BY "+guarOrPat+".LName,"+guarOrPat+".FName ";
			DataTable table=Db.GetTable(command);
			List<PatAging> agingList=new List<PatAging>();
			if(table.Rows.Count<1) {
				return agingList;
			}
			List<string> listSuperFamNums=table.Select().Select(x => x["SuperFamily"].ToString()).Where(x => x!="0").Distinct().ToList();
			//Create a dictionary for each super family head member and create a PatAging object that will represent the entire super family.
			Dictionary<long,PatAging> dictSuperFamPatAging=new Dictionary<long,PatAging>();
			if(listSuperFamNums.Count>0) {
				command="SELECT supe.PatNum,supe.LName,supe.FName,supe.MiddleI,supe.Preferred,supe.SuperFamily,supe.BillingType,"
					+"COALESCE(MAX(statement.DateSent),'0001-01-01') lastSuperStatement "
					+"FROM patient supe "
					+"LEFT JOIN statement ON supe.PatNum=statement.SuperFamily "
						+(ignoreInPerson?("AND statement.Mode_!="+POut.Int((int)StatementMode.InPerson)+" "):"")
					+"WHERE supe.PatNum=supe.SuperFamily "
					+"AND supe.HasSuperBilling=1 "
					+"GROUP BY supe.PatNum "
					+"ORDER BY NULL";
				dictSuperFamPatAging=Db.GetTable(command).Select().Where(x => listSuperFamNums.Contains(x["PatNum"].ToString()))
					.ToDictionary(x => PIn.Long(x["PatNum"].ToString()),x => new PatAging() {
						PatNum=PIn.Long(x["PatNum"].ToString()),
						DateLastStatement=PIn.Date(x["lastSuperStatement"].ToString()),
						SuperFamily=PIn.Long(x["SuperFamily"].ToString()),
						HasSuperBilling=true,//query only returns super heads who do have super billing
						PatName=Patients.GetNameLF(PIn.String(x["LName"].ToString()),PIn.String(x["FName"].ToString()),PIn.String(x["Preferred"].ToString()),
							PIn.String(x["MiddleI"].ToString())),
						BillingType=PIn.Long(x["BillingType"].ToString())
					});
			}
			//Only worry about looping through the entire data table for super family shenanigans if there are any super family head members present.
			if(dictSuperFamPatAging.Count>0 && isSuperStatements) {
				PatAging patAgingCur;
				//Now that we know all of the super family heads, loop through all the other patients that showed up in the outstanding aging list
				//and add each super family memeber's aging to their corresponding super family head PatAging entry in the dictionary.
				foreach(DataRow rCur in table.Rows) {
					if(rCur["HasSuperBilling"].ToString()!="1" || rCur["SuperFamily"].ToString()=="0") {
						continue;
					}
					if(!dictSuperFamPatAging.TryGetValue(PIn.Long(rCur["SuperFamily"].ToString()),out patAgingCur)) {
						continue;//super head must not have super billing
					}
					patAgingCur.Bal_0_30+=PIn.Double(rCur["Bal_0_30"].ToString());
					patAgingCur.Bal_31_60+=PIn.Double(rCur["Bal_31_60"].ToString());
					patAgingCur.Bal_61_90+=PIn.Double(rCur["Bal_61_90"].ToString());
					patAgingCur.BalOver90+=PIn.Double(rCur["BalOver90"].ToString());
					patAgingCur.BalTotal+=PIn.Double(rCur["BalTotal"].ToString());
					patAgingCur.InsEst+=PIn.Double(rCur["InsEst"].ToString());
					patAgingCur.AmountDue=patAgingCur.BalTotal-patAgingCur.InsEst;
					patAgingCur.PayPlanDue+=PIn.Double(rCur["PayPlanDue"].ToString());
				}
			}
			List<string> listPatNums=table.Select().Select(x => x["PatNum"].ToString()).ToList();
			Dictionary<long,DateTime> dictPatNumMaxProcDate=new Dictionary<long,DateTime>();
			Dictionary<long,DateTime> dictPatNumMaxPayDate=new Dictionary<long,DateTime>();
			Dictionary<long,DateTime> dictPatNumMaxPPCDate=new Dictionary<long,DateTime>();
			string guarOrPatient="";
			if(isSinglePatient) {
				guarOrPatient="patient.PatNum";
			}
			else {
				guarOrPatient="patient.Guarantor";
			}
			if(includeChanged) {
				command="SELECT "+guarOrPatient+" AS PatNum,MAX(procedurelog.ProcDate) maxDate "
					+"FROM procedurelog "
					+"INNER JOIN patient ON patient.PatNum=procedurelog.PatNum "
					+"WHERE procedurelog.ProcFee>0 AND procedurelog.ProcStatus="+POut.Int((int)ProcStat.C)+" "
					+"GROUP BY "+guarOrPatient;
				dictPatNumMaxProcDate=Db.GetTable(command).Select().Where(x => listPatNums.Contains(x["PatNum"].ToString()))
					.ToDictionary(x => PIn.Long(x["PatNum"].ToString()),x => PIn.Date(x["maxDate"].ToString()));
				command="SELECT "+guarOrPatient+" AS PatNum,MAX(claimproc.DateCP) maxDateCP "
					+"FROM claimproc "
					+"INNER JOIN patient ON patient.PatNum=claimproc.PatNum "
					+"WHERE claimproc.InsPayAmt>0 "
					+"GROUP BY "+guarOrPatient;
				dictPatNumMaxPayDate=Db.GetTable(command).Select().Where(x => listPatNums.Contains(x["PatNum"].ToString()))
					.ToDictionary(x => PIn.Long(x["PatNum"].ToString()),x => PIn.Date(x["maxDateCP"].ToString()));
				command="SELECT "+guarOrPatient+" AS PatNum,MAX(payplancharge.ChargeDate) maxDatePPC "
					+"FROM payplancharge "
					+"INNER JOIN patient ON patient.PatNum=payplancharge.PatNum "
					+"INNER JOIN payplan ON payplan.PayPlanNum = payplancharge.PayPlanNum "
						+"AND payplan.PlanNum = 0 "//don't want insurance payment plans to make patients appear in the billing list
					+"WHERE payplancharge.Principal + payplancharge.Interest>0 "
					+"AND payplancharge.ChargeType ="+(int)PayPlanChargeType.Debit+" "
					//include all charges in the past or due 'PayPlanBillInAdvance' days into the future.
					+"AND payplancharge.ChargeDate<= "+POut.Date(DateTime.Today.AddDays(PrefC.GetDouble(PrefName.PayPlansBillInAdvanceDays)))+" " 
					+"GROUP BY "+guarOrPatient;
				dictPatNumMaxPPCDate=Db.GetTable(command).Select().Where(x => listPatNums.Contains(x["PatNum"].ToString()))
					.ToDictionary(x => PIn.Long(x["PatNum"].ToString()),x => PIn.Date(x["maxDatePPC"].ToString()));
			}
			List<long> listExcludePendingPatNums=new List<long>();
			if(excludeInsPending) {
				command="SELECT "+guarOrPatient+" AS PatNum "
					+"FROM claim "
					+"INNER JOIN patient ON patient.PatNum=claim.PatNum "
					+"WHERE claim.ClaimStatus IN ('U','H','W','S') AND claim.ClaimType IN ('P','S','Other') "
					+"GROUP BY "+guarOrPatient;
				listExcludePendingPatNums=Db.GetTable(command).Rows.OfType<DataRow>()
					.Where(x => listPatNums.Contains(x["PatNum"].ToString()))
					.Select(x => PIn.Long(x["PatNum"].ToString())).ToList();
			}
			List<long> listExcludeUnsentPatNums=new List<long>();
			if(excludeIfUnsentProcs) {
				command="SELECT "+guarOrPatient+" AS PatNum "
					+"FROM patient "
					+"INNER JOIN procedurelog ON procedurelog.PatNum=patient.PatNum "
					+"INNER JOIN claimproc ON claimproc.ProcNum=procedurelog.ProcNum "
					+"WHERE claimproc.NoBillIns=0 AND claimproc.Status="+POut.Int((int)ClaimProcStatus.Estimate)+" "
					+"AND procedurelog.ProcFee>0 AND procedurelog.ProcStatus="+POut.Int((int)ProcStat.C)+" "
					+"AND procedurelog.ProcDate>CURDATE()-INTERVAL 6 MONTH "//Oracle not supported, check above would've returned if Oracle
					+"GROUP BY "+guarOrPatient;
				listExcludeUnsentPatNums=Db.GetTable(command).Select()
					.Where(x => listPatNums.Contains(x["PatNum"].ToString()))
					.Select(x => PIn.Long(x["PatNum"].ToString())).ToList();
			}
			PatAging patage;
			DateTime dateLastStatement;
			DateTime dateLastProc;
			DateTime dateLastPay;
			DateTime dateLastPPC;
			foreach(DataRow rowCur in table.Rows) {
				patage=new PatAging();
				patage.PatNum=PIn.Long(rowCur["PatNum"].ToString());
				patage.SuperFamily=PIn.Long(rowCur["SuperFamily"].ToString());
				patage.HasSuperBilling=PIn.Bool(rowCur["HasSuperBilling"].ToString());
				patage.ClinicNum=PIn.Long(rowCur["ClinicNum"].ToString());
				dateLastStatement=DateTime.MinValue;
				PatAging superPat;
				if(patage.HasSuperBilling && dictSuperFamPatAging.TryGetValue(patage.SuperFamily,out superPat)) {
					dateLastStatement=superPat.DateLastStatement;
				}
				//If pat HasSuperBilling and super head has received a super statement, dateLastStatement will be the more recent date of the last super 
				//statement or the last family statement, regardless of the isSuperStatements flag.  If the guar is not in a super family with super billing, 
				//dateLastStatement will be the date of their last family statement (not-super statement).
				dateLastStatement=new[] { dateLastStatement,PIn.Date(rowCur["lastStatement"].ToString()) }.Max();
				dateLastProc=DateTime.MinValue;
				dateLastPay=DateTime.MinValue;
				dateLastPPC=DateTime.MinValue;
				if(includeChanged) {
					dictPatNumMaxProcDate.TryGetValue(patage.PatNum,out dateLastProc);
					dictPatNumMaxPayDate.TryGetValue(patage.PatNum,out dateLastPay);
					dictPatNumMaxPPCDate.TryGetValue(patage.PatNum,out dateLastPPC);
				}
				if(dateLastStatement.Date>=new[] { lastStatement.AddDays(1),dateLastProc,dateLastPay,dateLastPPC }.Max().Date
					|| (excludeInsPending && listExcludePendingPatNums.Contains(patage.PatNum))//exclude if ins pending
					|| (excludeIfUnsentProcs && listExcludeUnsentPatNums.Contains(patage.PatNum))//exclude if unsent procs exist
					|| (isSuperStatements && patage.HasSuperBilling && patage.PatNum!=patage.SuperFamily))//included in super statement and not the super head, skip
				{
					continue;//this patient is excluded, skip
				}
				//if not generating super statements OR this guar doesn't have super billing OR this guar's super family num isn't in the dictionary
				//then this guar will get a non-super statement (regular single family statement)
				if(!isSuperStatements || !patage.HasSuperBilling || !dictSuperFamPatAging.TryGetValue(patage.SuperFamily,out patage)) {
					patage.Bal_0_30=PIn.Double(rowCur["Bal_0_30"].ToString());
					patage.Bal_31_60=PIn.Double(rowCur["Bal_31_60"].ToString());
					patage.Bal_61_90=PIn.Double(rowCur["Bal_61_90"].ToString());
					patage.BalOver90=PIn.Double(rowCur["BalOver90"].ToString());
					patage.BalTotal=PIn.Double(rowCur["BalTotal"].ToString());
					patage.InsEst=PIn.Double(rowCur["InsEst"].ToString());
					patage.AmountDue=patage.BalTotal-patage.InsEst;
					patage.PayPlanDue =PIn.Double(rowCur["PayPlanDue"].ToString());
					patage.DateLastStatement=PIn.Date(rowCur["lastStatement"].ToString());
					patage.PatName=Patients.GetNameLF(PIn.String(rowCur["LName"].ToString()),PIn.String(rowCur["FName"].ToString()),
						PIn.String(rowCur["Preferred"].ToString()),PIn.String(rowCur["MiddleI"].ToString()));
					patage.BillingType=PIn.Long(rowCur["BillingType"].ToString());
				}
				agingList.Add(patage);
			}
			return agingList;
		}

		///<summary>Used only to run finance charges, so it ignores negative balances.</summary>
		public static List<PatAging> GetAgingListSimple(List<long> listBillingTypeNums,List<long> listGuarantors,bool doIncludeZeroBalance=false){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PatAging>>(MethodBase.GetCurrentMethod(),listBillingTypeNums,listGuarantors);
			}
			string command="SELECT PatNum,Bal_0_30,Bal_31_60,Bal_61_90,BalOver90,BalTotal,InsEst,LName,FName,MiddleI,PriProv,BillingType,Guarantor "
				+"FROM patient ";//actually only gets guarantors since others are 0.
			if(doIncludeZeroBalance) {
				command+="WHERE TRUE ";
			}
			else {
				command+="WHERE Bal_0_30 + Bal_31_60 + Bal_61_90 + BalOver90 - InsEst > '0.005' ";//more that 1/2 cent
			}
			if(listBillingTypeNums.Count>0) {
				command+="AND BillingType IN ("+string.Join(",",listBillingTypeNums)+") ";
			}
			if(listGuarantors.Count>0) {
				command+="AND PatNum IN ("+string.Join(",",listGuarantors)+") ";
			}
			command+="ORDER BY PatNum";
			return Db.GetTable(command).Select().Select(x => 
				new PatAging() {
					PatNum     = PIn.Long(x[0].ToString()),
					Bal_0_30   = PIn.Double(x[1].ToString()),
					Bal_31_60  = PIn.Double(x[2].ToString()),
					Bal_61_90  = PIn.Double(x[3].ToString()),
					BalOver90  = PIn.Double(x[4].ToString()),
					BalTotal   = PIn.Double(x[5].ToString()),
					InsEst     = PIn.Double(x[6].ToString()),
					PatName    = PIn.String(x[7].ToString())+", "+PIn.String(x[8].ToString())+" "+PIn.String(x[9].ToString()),
					AmountDue  = PIn.Double(x[5].ToString())-PIn.Double(x[6].ToString()),
					PriProv    = PIn.Long(x[10].ToString()),
					BillingType= PIn.Long(x[11].ToString()),
					Guarantor  = PIn.Long(x[12].ToString()),
				}
			).ToList();
		}

		///<summary>Used only by the OpenDentalService Transworld thread to sync accounts sent for collection.  Gets a list of PatAgings for the guars
		///identified by the PatNums in listGuarNums.  Will return all, even negative bals.  Does not consider SuperFamilies, only individual guars.</summary>
		public static List<PatAging> GetAgingListFromGuarNums(List<long> listGuarNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PatAging>>(MethodBase.GetCurrentMethod(),listGuarNums);
			}
			if(listGuarNums.Count<1) {
				return new List<PatAging>();
			}
			string command="SELECT PatNum,Guarantor,LName,FName,MiddleI,PriProv,BillingType,ClinicNum,Bal_0_30,Bal_31_60,Bal_61_90,BalOver90,BalTotal,InsEst "
				+"FROM patient "
				+"WHERE patient.PatNum IN ("+string.Join(",",listGuarNums.Select(x => POut.Long(x)))+") "
				+"AND patient.Guarantor=patient.PatNum";
			List<PatAging> listPatAgings=Db.GetTable(command).Select().Select(x => new PatAging() {
					PatNum     =PIn.Long(x["PatNum"].ToString()),
					Guarantor  =PIn.Long(x["Guarantor"].ToString()),
					PatName    =PIn.String(x["LName"].ToString())+", "+PIn.String(x["FName"].ToString())+" "+PIn.String(x["MiddleI"].ToString()),
					PriProv    =PIn.Long(x["PriProv"].ToString()),
					BillingType=PIn.Long(x["BillingType"].ToString()),
					ClinicNum  =PIn.Long(x["ClinicNum"].ToString()),
					Bal_0_30   =PIn.Double(x["Bal_0_30"].ToString()),
					Bal_31_60  =PIn.Double(x["Bal_31_60"].ToString()),
					Bal_61_90  =PIn.Double(x["Bal_61_90"].ToString()),
					BalOver90  =PIn.Double(x["BalOver90"].ToString()),
					BalTotal   =PIn.Double(x["BalTotal"].ToString()),
					InsEst     =PIn.Double(x["InsEst"].ToString()),
					AmountDue  =PIn.Double(x["BalTotal"].ToString())-PIn.Double(x["InsEst"].ToString()),
					ListTsiLogs=new List<TsiTransLog>()
				}).ToList();
			if(listPatAgings.Count==0) {
				return listPatAgings;
			}
			Dictionary<long,List<TsiTransLog>> dictPatNumListTrans=TsiTransLogs.SelectMany(listGuarNums)
				.GroupBy(x => x.PatNum)
				.ToDictionary(x => x.Key,x => x.OrderByDescending(y => y.TransDateTime).ToList());
			foreach(PatAging patAgingCur in listPatAgings) {
				if(!dictPatNumListTrans.TryGetValue(patAgingCur.Guarantor,out patAgingCur.ListTsiLogs)) {
					patAgingCur.ListTsiLogs=new List<TsiTransLog>();
				}
			}
			return listPatAgings;
		}

		///<summary>Gets the next available integer chart number.  Will later add a where clause based on preferred format.</summary>
		public static string GetNextChartNum(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			string command="SELECT ChartNumber from patient WHERE "
				+DbHelper.Regexp("ChartNumber","^[0-9]+$")+" "//matches any positive number of digits
				+"ORDER BY (chartnumber+0) DESC";//1/13/05 by Keyush Shaw-added 0.
			command=DbHelper.LimitOrderBy(command,1);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0){//no existing chart numbers
				return "1";
			}
			string lastChartNum=PIn.String(table.Rows[0][0].ToString());
			//or could add more match conditions
			try {
				return (Convert.ToInt64(lastChartNum)+1).ToString();
			}
			catch {
				throw new ApplicationException(lastChartNum+" is an existing ChartNumber.  It's too big to convert to a long int, so it's not possible to add one to automatically increment.");
			}
		}

		///<summary>Returns the name(only one) of the patient using this chartnumber.</summary>
		public static string ChartNumUsedBy(string chartNum,long excludePatNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),chartNum,excludePatNum);
			}
			string command="SELECT LName,FName from patient WHERE "
				+"ChartNumber = '"+POut.String(chartNum)
				+"' AND PatNum != '"+excludePatNum.ToString()+"'";
			DataTable table=Db.GetTable(command);
			string retVal="";
			if(table.Rows.Count!=0){//found duplicate chart number
				retVal=PIn.String(table.Rows[0][1].ToString())+" "+PIn.String(table.Rows[0][0].ToString());
			}
			return retVal;
		}

		///<summary>Used in the patient select window to determine if a trial version user is over their limit.</summary>
		public static int GetNumberPatients(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod());
			}
			string command="SELECT Count(*) FROM patient";
			DataTable table=Db.GetTable(command);
			return PIn.Int(table.Rows[0][0].ToString());
		}

		///<summary>Makes a call to the db to figure out if the current HasIns status is correct.  If not, then it changes it.</summary>
		public static void SetHasIns(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patNum);
				return;
			}
			string command="SELECT patient.HasIns,COUNT(patplan.PatNum) FROM patient "
				+"LEFT JOIN patplan ON patplan.PatNum=patient.PatNum"
				+" WHERE patient.PatNum="+POut.Long(patNum)
				+" GROUP BY patplan.PatNum,patient.HasIns";
			DataTable table=Db.GetTable(command);
			string newVal="";
			if(table.Rows[0][1].ToString()!="0"){
				newVal="I";
			}
			if(newVal!=table.Rows[0][0].ToString()){
				command="UPDATE patient SET HasIns='"+POut.String(newVal)
					+"' WHERE PatNum="+POut.Long(patNum);
				Db.NonQ(command);
			}
		}

		///<summary>Gets the provider for this patient.  If provNum==0, then it gets the practice default prov.
		///If no practice default set, returns the first non-hidden ProvNum from the provider cache.</summary>
		public static long GetProvNum(Patient pat) {
			//No need to check RemotingRole; no call to db.
			long retval=pat.PriProv;
			if(retval==0) {
				retval=PrefC.GetLong(PrefName.PracticeDefaultProv);
			}
			if(retval==0) {
				retval=Providers.GetFirstOrDefault(x => true,true)?.ProvNum??0;
			}
			return retval;
		}

		///<summary>Calls Patients.GetProvNum after getting the patient with this patNum. Gets the provider for this patient.  If pat.PriProv==0, then it
		///gets the practice default prov.  If no practice default set, returns the first non-hidden ProvNum from the provider cache.</summary>
		public static long GetProvNum(long patNum) {
			//No need to check RemotingRole; no call to db.
			return GetProvNum(GetPat(patNum));
		}

		///<summary>Gets the list of all valid patient primary keys. Allows user to specify whether to include non-deleted patients. Used when checking for missing ADA procedure codes after a user has begun entering them manually. This function is necessary because not all patient numbers are necessarily consecutive (say if the database was created due to a conversion from another program and the customer wanted to keep their old patient ids after the conversion).</summary>
		public static long[] GetAllPatNums(bool hasDeleted) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<long[]>(MethodBase.GetCurrentMethod(),hasDeleted);
			}
			string command="";
			if(hasDeleted) {
				command="SELECT PatNum FROM patient";
			}
			else {
				command="SELECT PatNum FROM patient WHERE patient.PatStatus!=4";
			}
			DataTable dt=Db.GetTable(command);
			long[] patnums=new long[dt.Rows.Count];
			for(int i=0;i<patnums.Length;i++){
				patnums[i]=PIn.Long(dt.Rows[i]["PatNum"].ToString());
			}
			return patnums;
		}

		///<summary>Converts a date to an age. If age is over 115, then returns 0.</summary>
		public static int DateToAge(DateTime date){
			//No need to check RemotingRole; no call to db.
			if(date.Year<1880)
				return 0;
			if(date.Month < DateTime.Now.Month){//birthday in previous month
				return DateTime.Now.Year-date.Year;
			}
			if(date.Month == DateTime.Now.Month && date.Day <= DateTime.Now.Day){//birthday in this month
				return DateTime.Now.Year-date.Year;
			}
			return DateTime.Now.Year-date.Year-1;
		}

		///<summary>Converts a date to an age. If age is over 115, then returns 0.</summary>
		public static int DateToAge(DateTime birthdate,DateTime asofDate) {
			//No need to check RemotingRole; no call to db.
			if(birthdate.Year<1880)
				return 0;
			if(birthdate.Month < asofDate.Month) {//birthday in previous month
				return asofDate.Year-birthdate.Year;
			}
			if(birthdate.Month == asofDate.Month && birthdate.Day <= asofDate.Day) {//birthday in this month
				return asofDate.Year-birthdate.Year;
			}
			return asofDate.Year-birthdate.Year-1;
		}

		///<summary>If zero, returns empty string.  Otherwise returns simple year.  Also see PatientLogic.DateToAgeString().</summary>
		public static string AgeToString(int age){
			//No need to check RemotingRole; no call to db.
			if(age==0) {
				return "";
			}
			else {
				return age.ToString();
			}
		}

		public static void ReformatAllPhoneNumbers() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			string oldTel;
			string newTel;
			string idNum;
			string command="select * from patient";
			DataTable table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				idNum=PIn.String(table.Rows[i][0].ToString());
				//home
				oldTel=PIn.String(table.Rows[i][15].ToString());
				newTel=TelephoneNumbers.ReFormat(oldTel);
				if(oldTel!=newTel) {
					command="UPDATE patient SET hmphone = '"
						+POut.String(newTel)+"' WHERE patNum = '"+idNum+"'";
					Db.NonQ(command);
				}
				//wk:
				oldTel=PIn.String(table.Rows[i][16].ToString());
				newTel=TelephoneNumbers.ReFormat(oldTel);
				if(oldTel!=newTel) {
					command="UPDATE patient SET wkphone = '"
						+POut.String(newTel)+"' WHERE patNum = '"+idNum+"'";
					Db.NonQ(command);
				}
				//wireless
				oldTel=PIn.String(table.Rows[i][17].ToString());
				newTel=TelephoneNumbers.ReFormat(oldTel);
				if(oldTel!=newTel) {
					command="UPDATE patient SET wirelessphone = '"
						+POut.String(newTel)+"' WHERE patNum = '"+idNum+"'";
					Db.NonQ(command);
				}
			}
			command="SELECT * from carrier";
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				idNum=PIn.String(table.Rows[i][0].ToString());
				//ph
				oldTel=PIn.String(table.Rows[i][7].ToString());
				newTel=TelephoneNumbers.ReFormat(oldTel);
				if(oldTel!=newTel) {
					command="UPDATE carrier SET Phone = '"
						+POut.String(newTel)+"' WHERE CarrierNum = '"+idNum+"'";
					Db.NonQ(command);
				}
			}
			command="SELECT PatNum,ICEPhone FROM patientnote";
			table=Db.GetTable(command);
			for(int i = 0;i<table.Rows.Count;i++) {
				idNum=PIn.String(table.Rows[i]["PatNum"].ToString());
				oldTel=PIn.String(table.Rows[i]["ICEPhone"].ToString());
				newTel=TelephoneNumbers.ReFormat(oldTel);
				if(oldTel!=newTel) {
					command="UPDATE patientnote SET ICEPhone='"+POut.String(newTel)+"' WHERE PatNum="+idNum;
					Db.NonQ(command);
				}
			}
		}

		public static DataTable GetGuarantorInfo(long PatientID) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),PatientID);
			}
			string command=@"SELECT FName,MiddleI,LName,Guarantor,Address,
								Address2,City,State,Zip,Email,EstBalance,
								BalTotal,Bal_0_30,Bal_31_60,Bal_61_90,BalOver90
						FROM Patient Where Patnum="+PatientID+
				" AND patnum=guarantor";
			return Db.GetTable(command);
		}

		///<summary>Will return 0 if can't find exact matching pat.</summary>
		public static long GetPatNumByNameAndBirthday(string lName,string fName,DateTime birthdate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),lName,fName,birthdate);
			}
			string command="SELECT PatNum FROM patient WHERE "
				+"LName='"+POut.String(lName)+"' "
				+"AND FName='"+POut.String(fName)+"' "
				+"AND Birthdate="+POut.Date(birthdate)+" "
				+"AND PatStatus!="+POut.Int((int)PatientStatus.Archived)+" "//Not Archived
				+"AND PatStatus!="+POut.Int((int)PatientStatus.Deleted);//Not Deleted
			return PIn.Long(Db.GetScalar(command));
		}

		///<summary>Will return an empty list if it can't find exact matching pat.</summary>
		public static List<long> GetListPatNumsByNameAndBirthday(string lName,string fName,DateTime birthdate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),lName,fName,birthdate);
			}
			string command="SELECT PatNum FROM patient "
				+"WHERE LName='"+POut.String(lName)+"' "
				+"AND FName='"+POut.String(fName)+"' "
				+"AND Birthdate="+POut.Date(birthdate)+" "
				+"AND PatStatus!="+POut.Int((int)PatientStatus.Archived)+" "//Not Archived
				+"AND PatStatus!="+POut.Int((int)PatientStatus.Deleted);//Not Deleted
			return Db.GetListLong(command);
		}

		///<summary>Returns a list of all patients within listSortedPatients which match the given pat.LName, pat.FName and pat.Birthdate.
		///Ignores case and leading/trailing space.  The listSortedPatients MUST be sorted by LName, then FName, then Birthdate or else the result will be
		///wrong.  Call listSortedPatients.Sort() before calling this function.  This function uses a binary search to much more efficiently locate
		///matches than a linear search would be able to.</summary>
		public static List<Patient> GetPatientsByNameAndBirthday(Patient pat,List <Patient> listSortedPatients) {
			if(pat.LName.Trim().ToLower().Length==0 || pat.FName.Trim().ToLower().Length==0 || pat.Birthdate.Year < 1880) {
				//We do not allow a match unless Last Name, First Name, AND birthdate are specified.  Otherwise at match could be meaningless.
				return new List<Patient>();
			}
			int patIdx=listSortedPatients.BinarySearch(pat);//If there are multiple matches, then this will only return one of the indexes randomly.
			if(patIdx < 0) {
				//No matches found.
				return new List<Patient>();
			}
			//The matched indicies will all be consecutive and will include the returned index from the binary search, because the list is sorted.
			int beginIdx=patIdx;
			for(int i=patIdx-1;i >= 0 && pat.CompareTo(listSortedPatients[i])==0;i--) {
				beginIdx=i;
			}
			int endIdx=patIdx;
			for(int i=patIdx+1;i < listSortedPatients.Count && pat.CompareTo(listSortedPatients[i])==0;i++) {
				endIdx=i;
			}
			List <Patient> listPatientMatches=new List<Patient>();
			for(int i=beginIdx;i<=endIdx;i++) {
				listPatientMatches.Add(listSortedPatients[i]);
			}
			return listPatientMatches;
		}

		///<summary>Returns the PatNums with the same name and birthday as passed in. The email and the phone numbers passed in will only be considered
		///if there is more than one patient with the same name and birthday. If a patient's family member's email or phone matches the ones passed in,
		///then that patient will be included.</summary>
		public static List<long> GetPatNumsByNameBirthdayEmailAndPhone(string lName,string fName,DateTime birthDate,string email,
			List<string> listPhones) 
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),lName,fName,birthDate,email,listPhones);
			}
			//Get all potential matches by name and birthdate first.
			List<long> listMatchingNameDOB=GetListPatNumsByNameAndBirthday(lName,fName,birthDate);
			if(listMatchingNameDOB.Count < 2) {
				return listMatchingNameDOB;//One or no matches via name and birth date so no need to waste time checking for phone/email matches in the fam.
			}
			//There are some potential duplicates found in the database.  Now we need to make sure that the email OR the phone is already on file.
			//We are going to look at every single phone number and email address on all family members just in case.
			string command="SELECT patient.PatNum,patient.Guarantor,fam.PatNum AS FamMemberPatNum,"
				+"fam.Email,fam.HmPhone,fam.WkPhone,fam.WirelessPhone,COALESCE(phonenumber.PhoneNumberVal,'') AS OtherPhone "
				+"FROM patient "
				+"INNER JOIN patient g ON g.PatNum=patient.Guarantor "
				+"INNER JOIN patient fam ON fam.Guarantor=g.PatNum "
				+"LEFT JOIN phonenumber ON phonenumber.PatNum=fam.PatNum "
				+"WHERE patient.PatNum IN ("+string.Join(",",listMatchingNameDOB)+") "
				+"AND fam.PatStatus != "+POut.Int((int)PatientStatus.Deleted);
			listPhones=listPhones.Where(x => !string.IsNullOrEmpty(x)) //Get rid of blank numbers
				.Select(x => x.StripNonDigits()).ToList();//Get rid of non-digit characters
			List<long> listMatchingContacts=Db.GetTable(command).Rows.Cast<DataRow>()
				.Where(x => PIn.String(x["Email"].ToString())==email
					|| listPhones.Any(y => y==PIn.String(x["HmPhone"].ToString()).StripNonDigits())
					|| listPhones.Any(y => y==PIn.String(x["WkPhone"].ToString()).StripNonDigits())
					|| listPhones.Any(y => y==PIn.String(x["WirelessPhone"].ToString()).StripNonDigits())
					|| listPhones.Any(y => y==PIn.String(x["OtherPhone"].ToString()).StripNonDigits()))
				.Select(x => PIn.Long(x["PatNum"].ToString())).Distinct().ToList();
			if(listMatchingContacts.Count > 0) {//We have found at least one match based on contact info.
				return listMatchingContacts;
			}
			//There weren't any matches found from contact info.
			return listMatchingNameDOB;
		}

		///<summary>Returns true if there is an exact match in the database based on the lName, fName, and birthDate passed in.
		///Also, the phone number or the email must match at least one phone number or email on file for any patient within the family.
		///Otherwise we assume a match is not within the database because some offices have multiple clinics and we need strict matching.</summary>
		public static bool GetHasDuplicateForNameBirthdayEmailAndPhone(string lName,string fName,DateTime birthDate,string email,string phone) {
			return GetHasDuplicateForNameBirthdayEmailAndPhone(lName,fName,birthDate,email,new List<string> { phone });
		}

		///<summary>Returns true if there is an exact match in the database based on the lName, fName, and birthDate passed in.
		///Also, one of the phone numbers or the email must match at least one phone number or email on file for any patient within the family.
		///Otherwise we assume a match is not within the database because some offices have multiple clinics and we need strict matching.</summary>
		public static bool GetHasDuplicateForNameBirthdayEmailAndPhone(string lName,string fName,DateTime birthDate,string email,
			List<string> listPhones) 
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),lName,fName,birthDate,email,listPhones);
			}
			//Get all potential matches by name and brith date first.
			List<long> listPatNums=GetListPatNumsByNameAndBirthday(lName,fName,birthDate);
			if(listPatNums.Count < 1) {
				return false;//No matches via name and birth date so no need to waste time checking for phone / email matches in the family.
			}
			string command="";
			//There are some potential duplicates found in the database.  Now we need to make sure that the email OR the phone is already on file.
			//We are going to look at every single phone number and email address on all family members just in case.
			List<long> listFamilyPatNums=GetAllFamilyPatNums(listPatNums);//Should never return an empty list.
			//Only waste time checking for patients with the same email address if an email was passed in.
			if(!string.IsNullOrEmpty(email)) {
				command="SELECT COUNT(*) FROM patient "
				+"WHERE patient.Email='"+POut.String(email)+"' "
				+"AND PatNum IN ("+string.Join(",",listFamilyPatNums)+")";
				if(Db.GetCount(command)!="0") {
					return true;//The name and birth date match AND someone in the family has the exact email address passed in.  This is consider a duplicate.
				}
			}
			//Query to get all phone numbers from both the patient table and the 
			command="SELECT HmPhone FROM patient WHERE PatNum IN ("+string.Join(",",listFamilyPatNums)+") "
				+"UNION SELECT WkPhone Phone FROM patient WHERE PatNum IN ("+string.Join(",",listFamilyPatNums)+") "
				+"UNION SELECT WirelessPhone Phone FROM patient WHERE PatNum IN ("+string.Join(",",listFamilyPatNums)+") "
				+"UNION SELECT PhoneNumberVal Phone FROM phonenumber WHERE PatNum IN ("+string.Join(",",listFamilyPatNums)+") ";
			List<string> listAllFamilyPhones=Db.GetListString(command).Where(x => !string.IsNullOrEmpty(x)).ToList();
			listPhones=listPhones.Where(x => x != null)
				.Select(x => x.StripNonDigits()).ToList();//Get rid of non-digit characters
			//Go through each phone number and strip out all non-digit chars and compare them to the phone passed in.
			foreach(string phoneFamily in listAllFamilyPhones) {
				string phoneFamDigitsOnly=phoneFamily.StripNonDigits();
				if(listPhones.Any(x => x.Contains(phoneFamDigitsOnly) || phoneFamDigitsOnly.Contains(x))) {
					return true;//The name and birth date match AND someone in the family has the exact phone passed in.  This is consider a duplicate.
				}
			}
			return false;
		}

		///<summary>Will return 0 if can't find an exact matching pat.  Because it does not include birthdate, it's not specific enough for most situations.</summary>
		public static long GetPatNumByName(string lName,string fName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),lName,fName);
			}
			string command="SELECT PatNum FROM patient WHERE "
				+"LName='"+POut.String(lName)+"' "
				+"AND FName='"+POut.String(fName)+"' "
				+"AND PatStatus!=4 "//not deleted
				+"LIMIT 1";
			return PIn.Long(Db.GetScalar(command));
		}

		///<summary>Gets a list of patients that have any part of their name (last, first, middle, preferred) that matches the given criteria.
		///Optionally give a clinicNum and the query will only include patients associated with that clinic (patient.ClinicNum).</summary>
		public static List<Patient> GetPatientsByPartialName(string partialName,long clinicNum=0) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),partialName,clinicNum);
			}
			string command="SELECT * FROM patient WHERE 1 ";
			List<string> listNames=partialName.Split().Select(x => POut.String(x.ToLower())).ToList();
			foreach(string name in listNames) {
				command+="AND (LName LIKE '%"+POut.String(name)+"%' "
				+"OR FName LIKE '%"+POut.String(name)+"%' "
				+"OR MiddleI LIKE '%"+POut.String(name)+"%' "
				+"OR Preferred LIKE '%"+POut.String(name)+"%') ";
			}
			if(clinicNum > 0) {
				command+="AND ClinicNum="+POut.Long(clinicNum)+" ";
			}
			return Crud.PatientCrud.SelectMany(command);
		}

		/// <summary>When importing webforms, if it can't find an exact match, this method attempts a similar match.</summary>
		public static List<Patient> GetSimilarList(string lName,string fName,DateTime birthdate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),lName,fName,birthdate);
			}
			int subStrIndexlName=2;
			int subStrIndexfName=2;
			if(lName.Length<2) {
				subStrIndexlName=lName.Length;
			}
			if(fName.Length<2) {
				subStrIndexfName=fName.Length;
			}
			string command="SELECT * FROM patient WHERE "
				+"LName LIKE '"+POut.String(lName.Substring(0,subStrIndexlName))+"%' "
				+"AND FName LIKE '"+POut.String(fName.Substring(0,subStrIndexfName))+"%' "
				+"AND (Birthdate="+POut.Date(birthdate)+" "//either a matching bd
				+"OR Birthdate < "+POut.Date(new DateTime(1880,1,1))+") "//or no bd
				+"AND PatStatus!="+POut.Int((int)PatientStatus.Archived)+" "//Not Archived
				+"AND PatStatus!="+POut.Int((int)PatientStatus.Deleted);//Not Deleted
			return Crud.PatientCrud.SelectMany(command);
		}

		///<summary>Returns a list of patients that match last and first name.  Case insensitive.</summary>
		public static List<Patient> GetListByName(string lName,string fName,long PatNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),lName,fName,PatNum);
			}
			string command="SELECT * FROM patient WHERE "
				+"LOWER(LName)=LOWER('"+POut.String(lName)+"') "
				+"AND LOWER(FName)=LOWER('"+POut.String(fName)+"') "
				+"AND PatNum!="+POut.Long(PatNum)
				+" AND PatStatus!=4";//not deleted
			return Crud.PatientCrud.SelectMany(command);
		}

		///<summary>Returns a list of patients that have the same last name, first name, and birthdate, ignoring case sensitivity, but different patNum.  Used to find duplicate patients that may be clones of the patient identified by the patNum parameter, or are the non-clone version of the patient.  Currently only used with GetCloneAndNonClone to find the non-clone and clone patients for the pateint sent in if they exist.</summary>
		public static List<Patient> GetListByNameAndBirthdate(long patNum,string lName,string fName,DateTime birthdate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),patNum,lName,fName,birthdate);
			}
			string command="SELECT * FROM patient WHERE LName LIKE '"+POut.String(lName)+"' AND FName LIKE '"+POut.String(fName)+"' "
				+"AND Birthdate="+POut.Date(birthdate,true)+" AND PatNum!="+POut.Long(patNum) +" AND PatStatus!="+POut.Int((int)PatientStatus.Deleted);
			return Crud.PatientCrud.SelectMany(command);
		}

		public static void UpdateFamilyBillingType(long billingType,long Guarantor) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),billingType,Guarantor);
				return;
			}
			string command="UPDATE patient SET BillingType="+POut.Long(billingType)+
				" WHERE Guarantor="+POut.Long(Guarantor);
			Db.NonQ(command);
		}

		public static void UpdateAllFamilyBillingTypes(long billingType,List<long> listGuarNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),billingType,listGuarNums);
				return;
			}
			if(listGuarNums.Count<1) {
				return;
			}
			string command="UPDATE patient SET BillingType="+POut.Long(billingType)+" "
				+"WHERE Guarantor IN ("+string.Join(",",listGuarNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

		public static DataTable GetPartialPatientData(long PatNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),PatNum);
			}
			string command="SELECT FName,LName,"+DbHelper.DateFormatColumn("birthdate","%m/%d/%Y")+" BirthDate,Gender "
				+"FROM patient WHERE patient.PatNum="+PatNum;
			return Db.GetTable(command);
		}

		public static DataTable GetPartialPatientData2(long PatNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),PatNum);
			}
			string command=@"SELECT FName,LName,"+DbHelper.DateFormatColumn("birthdate","%m/%d/%Y")+" BirthDate,Gender "
				+"FROM patient WHERE PatNum In (SELECT Guarantor FROM PATIENT WHERE patnum = "+PatNum+")";
			return Db.GetTable(command);
		}

		public static string GetEligibilityDisplayName(long patId) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),patId);
			}
			string command = @"SELECT FName,LName,"+DbHelper.DateFormatColumn("birthdate","%m/%d/%Y")+" BirthDate,Gender "
				+"FROM patient WHERE patient.PatNum=" + POut.Long(patId);
			DataTable table = Db.GetTable(command);
			if(table.Rows.Count == 0) {
				return "Patient(???) is Eligible";
			}
			return PIn.String(table.Rows[0][1].ToString()) + ", "+ PIn.String(table.Rows[0][0].ToString()) + " is Eligible";
		}

		///<summary>Only a partial folderName will be sent in.  Not the .rvg part.</summary>
		public static bool IsTrophyFolderInUse(string folderName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),folderName);
			}
			string command ="SELECT COUNT(*) FROM patient WHERE TrophyFolder LIKE '%"+POut.String(folderName)+"%'";
			if(Db.GetCount(command)=="0") {
				return false;
			}
			return true;
		}

		///<summary>Used to check if a billing type is in use when user is trying to hide it.</summary>
		public static bool IsBillingTypeInUse(long defNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),defNum);
			}
			string command ="SELECT COUNT(*) FROM patient WHERE BillingType="+POut.Long(defNum)+" AND PatStatus!="+POut.Int((int)PatientStatus.Deleted);
			if(Db.GetCount(command)!="0") {
				return true;
			}
			command ="SELECT COUNT(*) FROM insplan WHERE BillingType="+POut.Long(defNum);
			if(Db.GetCount(command)!="0") {
				return true;
			}
			//check any prefs that are FK's to the definition.DefNum column and warn if a pref is using the def
			if(new[] {
					PrefName.TransworldPaidInFullBillingType,PrefName.ApptEConfirmStatusSent,PrefName.ApptEConfirmStatusAccepted,
					PrefName.ApptEConfirmStatusDeclined,PrefName.ApptEConfirmStatusSendFailed,PrefName.ApptConfirmExcludeEConfirm,
					PrefName.ApptConfirmExcludeERemind,PrefName.ApptConfirmExcludeESend,PrefName.BrokenAppointmentAdjustmentType,PrefName.ConfirmStatusEmailed,
					PrefName.ConfirmStatusTextMessaged,PrefName.PrepaymentUnearnedType,PrefName.SalesTaxAdjustmentType }
				.Select(x => PrefC.GetString(x))
				.SelectMany(x => x.Split(',').Select(y => PIn.Long(y,false)).Where(y => y>0))//some prefs are comma delimited lists of longs. SelectMany will return a single list of longs
				.Any(x => x==defNum))
			{
				return true;
			}
			return false;
		}

		///<summary>Returns true if this is a valid U.S Social Security Number.</summary>
		///<param name="formattedSSN">9 digits with dashes.</param>
		public static bool IsValidSSN(string ssn,out string formattedSSN) {
			if(Regex.IsMatch(ssn,@"^\d{9}$")) {//if just 9 numbers, reformat with dashes.
				ssn=ssn.Substring(0,3)+"-"+ssn.Substring(3,2)+"-"+ssn.Substring(5,4);				
			}
			formattedSSN=ssn;
			return Regex.IsMatch(formattedSSN,@"^\d\d\d-\d\d-\d\d\d\d$");
		}

		///<summary>If the current culture is U.S. and the ssn is 9 digits with dashes, removes the dashes.</summary>
		public static string SSNRemoveDashes(string ssn) {
			if(CultureInfo.CurrentCulture.Name=="en-US") {
				if(Regex.IsMatch(ssn,@"^\d\d\d-\d\d-\d\d\d\d$")){
					return ssn.Replace("-","");
				}
			}
			return ssn; //other cultures
		}

		///<summary>Updated 10/29/2015 v15.4.  To prevent orphaned patients, if patFrom is a guarantor then all family members of patFrom are moved into the family patTo belongs to, and then the merge of the two specified accounts is performed.  Returns false if the merge was canceled by the user.</summary>
		public static bool MergeTwoPatients(long patTo,long patFrom) {
			//No need to check RemotingRole; no call to db.
			if(patTo==patFrom) {
				//Do not merge the same patient onto itself.
				return true;
			}
			//We need to test patfields before doing anything else because the user may wish to cancel and abort the merge.
			PatField[] patToFields=PatFields.Refresh(patTo);
			PatField[] patFromFields=PatFields.Refresh(patFrom);
			List<PatField> patFieldsToDelete=new List<PatField>();
			List<PatField> patFieldsToUpdate=new List<PatField>();
			for(int i=0;i<patFromFields.Length;i++) {
				bool hasMatch=false;
				for(int j=0;j<patToFields.Length;j++) {
					//Check patient fields that are the same to see if they have different values.
					if(patFromFields[i].FieldName==patToFields[j].FieldName) {
						hasMatch=true;
						if(patFromFields[i].FieldValue!=patToFields[j].FieldValue) {
							//Get input from user on which value to use.
							DialogResult result=MessageBox.Show("The two patients being merged have different values set for the patient field:\r\n\""+patFromFields[i].FieldName+"\"\r\n\r\n"
								+"The merge into patient has the value: \""+patToFields[j].FieldValue+"\"\r\n"
								+"The merge from patient has the value: \""+patFromFields[i].FieldValue+"\"\r\n\r\n"
								+"Would you like to overwrite the merge into value with the merge from value?\r\n(Cancel will abort the merge)","Warning",MessageBoxButtons.YesNoCancel);
							if(result==DialogResult.Yes) {
								//User chose to use the merge from patient field info.
								patFromFields[i].PatNum=patTo;
								patFieldsToUpdate.Add(patFromFields[i]);
								patFieldsToDelete.Add(patToFields[j]);
							}
							else if(result==DialogResult.Cancel) {
								return false;//Completely cancels the entire merge.  No changes have been made at this point.
							}
						}
					}
				}
				if(!hasMatch) {//The patient field does not exist in the merge into account.
					patFromFields[i].PatNum=patTo;
					patFieldsToUpdate.Add(patFromFields[i]);
				}
			}
			return MergeTwoPatientPointOfNoReturn(patTo,patFrom,patFieldsToDelete,patFieldsToUpdate);
		}

		///<summary>Only call this method after all checks have been done to make sure the user wants these patients merged.</summary>
		public static bool MergeTwoPatientPointOfNoReturn(long patTo,long patFrom,List<PatField> patFieldsToDelete,List<PatField> patFieldsToUpdate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),patTo,patFrom,patFieldsToDelete,patFieldsToUpdate);
			}
			string[] patNumForeignKeys=new string[]{
				"adjustment.PatNum",
				"allergy.PatNum",
				"anestheticrecord.PatNum",
				"anesthvsdata.PatNum",
				"appointment.PatNum",
				"claim.PatNum",
				"claimproc.PatNum",
				"commlog.PatNum",
				"creditcard.PatNum",
				"custrefentry.PatNumCust",
				"custrefentry.PatNumRef",
				//"custreference.PatNum",  //This is handled below.  We do not want to change patnum, the references form only shows entries for active patients.
				"disease.PatNum",
				//"document.PatNum",  //This is handled below when images are stored in the database and on the client side for images stored in the AtoZ folder due to the middle tier.
				"ehramendment.PatNum",
				"ehrcareplan.PatNum",
				"ehrlab.PatNum",
				"ehrmeasureevent.PatNum",
				"ehrnotperformed.PatNum",				
				//"ehrpatient.PatNum",  //This is handled below.  We do not want to change patnum here because there can only be one entry per patient.
				"ehrprovkey.PatNum",
				"ehrquarterlykey.PatNum",
				"ehrsummaryccd.PatNum",
				"emailmessage.PatNum",
				"emailmessage.PatNumSubj",
				"encounter.PatNum",
				"erxlog.PatNum",
				"etrans.PatNum",
				"familyhealth.PatNum",
				//formpat.FormPatNum IS NOT a PatNum so it is should not be merged.  It is the primary key.
				"formpat.PatNum",
				"guardian.PatNumChild",  //This may create duplicate entries for a single patient and guardian
				"guardian.PatNumGuardian",  //This may create duplicate entries for a single patient and guardian
				"hl7msg.PatNum",
				"inssub.Subscriber",
				"installmentplan.PatNum",
				"intervention.PatNum",
				"labcase.PatNum",
				"labpanel.PatNum",
				"medicalorder.PatNum",
				//medicationpat.MedicationPatNum IS NOT a PatNum so it is should not be merged.  It is the primary key.
				"medicationpat.PatNum",
				"medlab.PatNum",
				"mount.PatNum",
				"orthochart.PatNum",
				//"oidexternal.IDInternal",  //TODO:  Deal with these elegantly below, not always a patnum
				//"patfield.PatNum", //Taken care of below
				"patient.ResponsParty",
				//"patient.PatNum"  //We do not want to change patnum
				//"patient.Guarantor"  //This is taken care of below
				"patient.SuperFamily",  //The patfrom guarantor was changed, so this should be updated
				//"patientlink.PatNumFrom",//We want to leave the link history unchanged so that audit entries display correctly. If we start using this table for other types of linkage besides merges, then we might need to include this column.
				//"patientlink.PatNumTo",//^^Ditto
				//"patientnote.PatNum"	//The patientnote table is ignored because only one record can exist for each patient.  The record in 'patFrom' remains so it can be accessed again if needed.
				//"patientrace.PatNum", //The patientrace table is ignored because we don't want duplicate races.  We could merge them but we would have to add specific code to stop duplicate races being inserted.
				"patplan.PatNum",
				"payment.PatNum",
				"payortype.PatNum",
				"payplan.Guarantor",//Treated as a patnum, because it is actually a guarantor for the payment plan, and not a patient guarantor.
				"payplan.PatNum",				
				"payplancharge.Guarantor",//Treated as a patnum, because it is actually a guarantor for the payment plan, and not a patient guarantor.
				"payplancharge.PatNum",
				"paysplit.PatNum",
				"perioexam.PatNum",
				"phonenumber.PatNum",
				"plannedappt.PatNum",
				"popup.PatNum",
				"procedurelog.PatNum",
				"procnote.PatNum",
				"proctp.PatNum",
				"providererx.PatNum",  //For non-HQ this should always be 0.
				//question.FormPatNum IS NOT a PatNum so it is should not be merged.  It is a FKey to FormPat.FormPatNum
				"question.PatNum",
				//"recall.PatNum",  //We do not merge recall entries because it would cause duplicate recall entries.  Instead, update current recall entries.
				"refattach.PatNum",
				//"referral.PatNum",  //This is synched with the new information below.
				"registrationkey.PatNum",
				"repeatcharge.PatNum",
				"reqstudent.PatNum",
				"reseller.PatNum",
				"rxpat.PatNum",
				"screenpat.PatNum",
				//screenpat.ScreenPatNum IS NOT a PatNum so it is should not be merged.  It is a primary key.
				//"securitylog.FKey",  //This would only matter when the FKey pointed to a PatNum.  Currently this is only for the PatientPortal permission
				//  which per Allen is not needed to be merged. 11/06/2015.
				//"securitylog.PatNum",//Changing the PatNum of a securitylog record will cause it to show a red (untrusted) in the audit trail.
				//  Best to preserve history in the securitylog and leave the corresponding PatNums static.
				"sheet.PatNum",
				"smsfrommobile.PatNum",
				"smstomobile.PatNum",
				"statement.PatNum",
				//task.KeyNum,  //Taken care of in a seperate step, because it is not always a patnum.
				//taskhist.KeyNum,  //Taken care of in a seperate step, because it is not always a patnum.
				"terminalactive.PatNum",
				"toothinitial.PatNum",
				"treatplan.PatNum",
				"treatplan.ResponsParty",
				//vaccineobs.VaccinePatNum IS NOT a PatNum so it is should not be merged. It is the FK to the vaccinepat.VaccinePatNum.
				"vaccinepat.PatNum",
				//vaccinepat.VaccinePatNum IS NOT a PatNum so it is should not be merged. It is the primary key.
				"vitalsign.PatNum",
				"xchargetransaction.PatNum"
			};
			string command="";
			//Update and remove all patfields that were added to the list above.
			for(int i=0;i<patFieldsToDelete.Count;i++) {
				PatFields.Delete(patFieldsToDelete[i]);
			}
			for(int j=0;j<patFieldsToUpdate.Count;j++) {
				PatFields.Update(patFieldsToUpdate[j]);
			}
			Patient patientFrom=Patients.GetPat(patFrom);
			Patient patientTo=Patients.GetPat(patTo);
			//CustReference.  We need to combine patient from and patient into entries to have the into patient information from both.
			CustReference custRefFrom=CustReferences.GetOneByPatNum(patientFrom.PatNum);
			CustReference custRefTo=CustReferences.GetOneByPatNum(patientTo.PatNum);
			if(custRefFrom!=null && custRefTo!=null) { //If either of these are null, do nothing.  This is an internal only table so we didn't bother fixing it/warning users here.
				CustReference newCustRef=new CustReference();
				newCustRef.CustReferenceNum=custRefTo.CustReferenceNum; //Use same primary key so we can update.
				newCustRef.PatNum=patientTo.PatNum;
				if(custRefTo.DateMostRecent > custRefFrom.DateMostRecent) {
					newCustRef.DateMostRecent=custRefTo.DateMostRecent; //Use the most recent date.
				}
				else {
					newCustRef.DateMostRecent=custRefFrom.DateMostRecent; //Use the most recent date.
				}
				if(custRefTo.Note=="") {
					newCustRef.Note=custRefFrom.Note;
				}
				else if(custRefFrom.Note=="") {
					newCustRef.Note=custRefTo.Note;
				}
				else {//Both entries have a note
					newCustRef.Note=(custRefTo.Note+" | "+custRefFrom.Note); /*Combine in a | delimited string*/
				}
				newCustRef.IsBadRef=(custRefFrom.IsBadRef || custRefTo.IsBadRef);  //If either entry is a bad reference, count as a bad reference.
				CustReferences.Update(newCustRef); //Overwrites the old custRefTo entry.
			}
			//Merge ehrpatient.  We only do something here if there is a FROM patient entry and no INTO patient entry, in which case we change the patnum on the row to bring it over.
			EhrPatient ehrPatFrom=EhrPatients.GetOne(patientFrom.PatNum);
			EhrPatient ehrPatTo=EhrPatients.GetOne(patientTo.PatNum);
			if(ehrPatFrom!=null && ehrPatTo==null) {  //There is an entry for the FROM patient, but not the INTO patient.
				ehrPatFrom.PatNum=patientTo.PatNum;
				EhrPatients.Update(ehrPatFrom); //Bring the patfrom entry over to the new.
			}
			//Move the patient documents if they are stored in the database.
			//We do not have to worry about documents having the same name when storing within the database, only physical documents need to be renamed.
			//Physical documents are handled on the client side (not here) due to middle tier issues.
			if(PrefC.AtoZfolderUsed==DataStorageType.InDatabase) {
				//Storing documents in the database.  Simply update the PatNum column accordingly. 
				//This query cannot be ran below where all the other tables are handled dyncamically because we do NOT want to update the PatNums in the case that documents are stored physically.
				command="UPDATE document "
					+"SET PatNum="+POut.Long(patTo)+" "
					+"WHERE PatNum="+POut.Long(patFrom);
				Db.NonQ(command);
			}
			//If the 'patFrom' had any ties to guardians, they should be deleted to prevent duplicate entries.
			command="DELETE FROM guardian"
				+" WHERE PatNumChild="+POut.Long(patFrom)
				+" OR PatNumGuardian="+POut.Long(patFrom);
			Db.NonQ(command);
			//Update all guarantor foreign keys to change them from 'patFrom' to 
			//the guarantor of 'patTo'. This will effectively move all 'patFrom' family members 
			//to the family defined by 'patTo' in the case that 'patFrom' is a guarantor. If
			//'patFrom' is not a guarantor, then this command will have no effect and is
			//thus safe to always be run.
			command="UPDATE patient "
				+"SET Guarantor="+POut.Long(patientTo.Guarantor)+" "
				+"WHERE Guarantor="+POut.Long(patFrom);
			Db.NonQ(command);
			//At this point, the 'patFrom' is a regular patient and is absoloutely not a guarantor.
			//Now modify all PatNum foreign keys from 'patFrom' to 'patTo' to complete the majority of the
			//merge of the records between the two accounts.			
			for(int i=0;i<patNumForeignKeys.Length;i++) {
				if(DataConnection.DBtype==DatabaseType.Oracle 
					&& patNumForeignKeys[i]=="ehrlab.PatNum") //Oracle does not currently support EHR labs.
				{
					continue;
				}
				string[] tableAndKeyName=patNumForeignKeys[i].Split(new char[] {'.'});
				command="UPDATE "+tableAndKeyName[0]
					+" SET "+tableAndKeyName[1]+"="+POut.Long(patTo)
					+" WHERE "+tableAndKeyName[1]+"="+POut.Long(patFrom);
				Db.NonQ(command);
			}
			//We have to move over the tasks belonging to the 'patFrom' patient in a seperate step because
			//the KeyNum field of the task table might be a foreign key to something other than a patnum,
			//including possibly an appointment number.
			command="UPDATE task "
				+"SET KeyNum="+POut.Long(patTo)+" "
				+"WHERE KeyNum="+POut.Long(patFrom)+" AND ObjectType="+((int)TaskObjectType.Patient);
			Db.NonQ(command);
			//We have to move over the tasks belonging to the 'patFrom' patient in a seperate step because the KeyNum field of the taskhist table might be 
			//  a foreign key to something other than a patnum, including possibly an appointment number.
			command="UPDATE taskhist "
				+"SET KeyNum="+POut.Long(patTo)+" "
				+"WHERE KeyNum="+POut.Long(patFrom)+" AND ObjectType="+((int)TaskObjectType.Patient);
			Db.NonQ(command);
			//We have to move over the tasks belonging to the 'patFrom' patient in a seperate step because the IDInternal field of the oidexternal table 
			//  might be a foreign key to something other than a patnum depending on the IDType
			command="UPDATE oidexternal "
				+"SET IDInternal="+POut.Long(patTo)+" "
				+"WHERE IDInternal="+POut.Long(patFrom)+" AND IDType='"+(IdentifierType.Patient.ToString())+"'";
			Db.NonQ(command);
			//Mark the patient where data was pulled from as archived unless the patient is already marked as deceased.
			//We need to have the patient marked either archived or deceased so that it is hidden by default, and
			//we also need the customer to be able to access the account again in case a particular table gets missed
			//in the merge tool after an update to Open Dental. This will allow our customers to remerge the missing
			//data after a bug fix is released. 
			command="UPDATE patient "
				+"SET PatStatus="+((int)PatientStatus.Archived)+" "
				+"WHERE PatNum="+POut.Long(patFrom)+" "
				+"AND PatStatus<>"+((int)PatientStatus.Deceased)+" "
				+DbHelper.LimitAnd(1);
			Db.NonQ(command);
			//Update patplan.Ordinal in case multiple patplans wound up with the same Ordinal
			List<PatPlan> listPatPlans=PatPlans.GetPatPlansForPat(patTo).OrderBy(x => x.Ordinal).ToList();
			PatPlan patPlanPrimary=listPatPlans.FirstOrDefault();
			if(patPlanPrimary != null) {
				PatPlans.SetOrdinal(patPlanPrimary.PatPlanNum,patPlanPrimary.Ordinal);//Will reset all other Ordinals to consecutive values.
			}
			//This updates the referrals with the new patient information from the merge.
			Referral referral=Referrals.GetFirstOrDefault(x => x.PatNum==patFrom);
			if(referral!=null) {
				referral.PatNum=patientTo.PatNum;
				referral.LName=patientTo.LName;
				referral.FName=patientTo.FName;
				referral.MName=patientTo.MiddleI;
				referral.Address=patientTo.Address;
				referral.Address2=patientTo.Address2;
				referral.City=patientTo.City;
				referral.ST=patientTo.State;
				referral.SSN=patientTo.SSN;
				referral.Zip=patientTo.Zip;
				referral.Telephone=TelephoneNumbers.FormatNumbersExactTen(patientTo.HmPhone);
				referral.EMail=patientTo.Email;
				Referrals.Update(referral);
				Referrals.RefreshCache();
			}
			Recalls.Synch(patTo);  //Update patient's recalls now that merge is completed.
			//Create a link from the from patient to the to patient.
			PatientLink patLink=new PatientLink();
			patLink.PatNumFrom=patFrom;
			patLink.PatNumTo=patTo;
			patLink.LinkType=PatientLinkType.Merge;
			PatientLinks.Insert(patLink);
			return true;
		}

		///<summary>LName, 'Preferred' FName M</summary>
		public static string GetNameLF(string LName,string FName,string Preferred,string MiddleI) {
			//No need to check RemotingRole; no call to db.
			string retVal="";
			retVal+=LName;
			if(FName!="" || MiddleI!="" || Preferred!="") {
				retVal+=",";
			}
			if(Preferred!="") {
				retVal+=" '"+Preferred+"'";
			}
			if(FName!="") {
				retVal=AddSpaceIfNeeded(retVal);
				retVal+=FName;
			}
			if(MiddleI!="") {
				retVal=AddSpaceIfNeeded(retVal);
				retVal+=MiddleI;
			}
			return retVal;
		}

		///<summary>LName, 'Preferred' FName M for the patnum passed in.  Uses the database.</summary>
		public static string GetNameLF(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),patNum);
			}
			Patient pat=Patients.GetPat(patNum);
			return GetNameLF(pat);
		}

		///<summary>Does not call DB to retrieve a patient, only uses the passed in object.</summary>
		public static string GetNameLF(Patient pat) {
			string retVal="";
			retVal+=pat.LName;
			if(pat.FName!="" || pat.MiddleI!="" || pat.Preferred!="") {
				retVal+=",";
			}
			if(pat.Preferred!="") {
				retVal+=" '"+pat.Preferred+"'";
			}
			if(pat.FName!="") {
				retVal=AddSpaceIfNeeded(retVal);
				retVal+=pat.FName;
			}
			if(pat.MiddleI!="") {
				retVal=AddSpaceIfNeeded(retVal);
				retVal+=pat.MiddleI;
			}
			return retVal;
		}

		///<summary>LName, FName M</summary>
		public static string GetNameLFnoPref(string LName,string FName,string MiddleI) {
			return GetNameLF(LName,FName,"",MiddleI);
		}

		///<summary>FName 'Preferred' M LName</summary>
		public static string GetNameFL(string LName,string FName,string Preferred,string MiddleI) {
			//No need to check RemotingRole; no call to db.
			string retVal="";
			if(FName!="") {
				retVal+=FName;
			}
			if(!string.IsNullOrWhiteSpace(Preferred)) {
				retVal=AddSpaceIfNeeded(retVal);
				retVal+="'"+Preferred+"'";
			}
			if(!string.IsNullOrWhiteSpace(MiddleI)) {
				retVal=AddSpaceIfNeeded(retVal);
				retVal+=MiddleI;
			}
			retVal=AddSpaceIfNeeded(retVal);
			retVal+=LName;
			return retVal;
		}

		///<summary>FName M LName</summary>
		public static string GetNameFLnoPref(string LName,string FName,string MiddleI) {
			//No need to check RemotingRole; no call to db.
			string retVal="";
			retVal+=FName;
			if(!string.IsNullOrWhiteSpace(MiddleI)) {
				retVal=AddSpaceIfNeeded(retVal);
				retVal+=MiddleI;
			}
			retVal=AddSpaceIfNeeded(retVal);
			retVal+=LName;
			return retVal;
		}

		///<summary>FName/Preferred LName</summary>
		public static string GetNameFirstOrPrefL(string LName,string FName,string Preferred) {
			//No need to check RemotingRole; no call to db.
			string retVal="";
			if(Preferred=="") {
				retVal+=FName;
			}
			else {
				retVal+=Preferred;
			}
			retVal=AddSpaceIfNeeded(retVal);
			retVal+=LName;
			return retVal;
		}

		///<summary>FName/Preferred M. LName</summary>
		public static string GetNameFirstOrPrefML(string LName,string FName,string Preferred,string MiddleI) {
			//No need to check RemotingRole; no call to db.
			string retVal="";
			if(Preferred=="") {
				retVal+=FName;
			}
			else {
				retVal+=Preferred; ;
			}
			if(!string.IsNullOrWhiteSpace(MiddleI)) {
				retVal=AddSpaceIfNeeded(retVal);
				retVal+=MiddleI+".";
			}
			retVal=AddSpaceIfNeeded(retVal);
			retVal+=LName;
			return retVal;
		}

		///<summary>Title FName M LName</summary>
		public static string GetNameFLFormal(string LName,string FName,string MiddleI,string Title) {
			//No need to check RemotingRole; no call to db.
			return string.Join(" ",new[] {Title,FName,MiddleI,LName}.Where(x => !string.IsNullOrEmpty(x)));//returns "" if all strings are null or empty.
		}

		///<summary>Includes preferred.</summary>
		public static string GetNameFirst(string FName,string Preferred) {
			//No need to check RemotingRole; no call to db.
			string retVal=FName;
			if(Preferred!="") {
				retVal+=" '"+Preferred+"'";
			}
			return retVal;
		}

		///<summary>Returns preferred name if one exists, otherwise returns first name.</summary>
		public static string GetNameFirstOrPreferred(string FName,string Preferred) {
			//No need to check RemotingRole; no call to db.
			if(Preferred!="") {
				return Preferred;
			}
			return FName;
		}

		///<summary>Returns first name if one exists or returns preferred name,otherwise returns last name.</summary>
		public static string GetNameFirstOrPreferredOrLast(string FName,string Preferred,string LName) {
			//No need to check RemotingRole; no call to db.
			if(FName!="") {
				return FName;
			}
			if(Preferred !="") {
				return Preferred;
			}
			return LName;
		}

		///<summary>Adds a space if the passed in string is not empty.  Used for name functions to add a space only when needed.</summary>
		private static string AddSpaceIfNeeded(string name) {
			if(name!="") {
				return name+" ";
			}
			return name;
		}

		///<summary>Dear __.  Does not include the "Dear" or the comma.</summary>
		public static string GetSalutation(string Salutation,string Preferred,string FName) {
			//No need to check RemotingRole; no call to db.
			if(Salutation!="") {
				return Salutation;
			}
			if(Preferred!="") {
				return Preferred;
			}
			return FName;
		}

		/// <summary>Result will be multiline.</summary>
		public static string GetAddressFull(string address,string address2,string city,string state,string zip) {
			//No need to check RemotingRole; no call to db.
			string retVal=address;
			if(address2!="") {
				retVal+="\r\n"+address2;
			}
			retVal+="\r\n"+city+", "+state+" "+zip;
			return retVal;
		}

		/// <summary>Change preferred provider for all patients with provNumFrom to provNumTo.</summary>
		public static void ChangePrimaryProviders(long provNumFrom,long provNumTo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),provNumFrom,provNumTo);
				return;
			}
			string command="UPDATE patient SET PriProv="+POut.Long(provNumTo)+" WHERE PriProv="+POut.Long(provNumFrom);
			Db.NonQ(command);
		}

		///<summary>Change secondary provider for all patients with provNumFrom to provNumTo.</summary>
		public static void ChangeSecondaryProviders(long provNumFrom,long provNumTo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),provNumFrom,provNumTo);
				return;
			}
			string command="UPDATE patient " 
				+"SET SecProv = '"+provNumTo+"' "
				+"WHERE SecProv = '"+provNumFrom+"'";
			Db.NonQ(command);
		}
		
		/// <summary>Gets all patients whose primary provider PriProv is in the list provNums.</summary>
		public static DataTable GetPatNumsByPriProvs(List<long> listProvNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),listProvNums);
			}
			if(listProvNums==null || listProvNums.Count==0) {
				return new DataTable();
			}
			string command="SELECT PatNum,PriProv FROM patient WHERE PriProv IN ("+string.Join(",",listProvNums)+")";
			return Db.GetTable(command);
		}

		///<summary>Gets the PatNum and ClinicNum for all patients whose ClinicNum is in listClinicNums.</summary>
		public static DataTable GetPatNumsByClinic(List<long> listClinicNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),listClinicNums);
			}
			if(listClinicNums==null || listClinicNums.Count==0) {
				return new DataTable();
			}
			string command="SELECT PatNum,ClinicNum FROM patient WHERE ClinicNum IN ("+string.Join(",",listClinicNums)+")";
			return Db.GetTable(command);
		}
		
		/// <summary>Change clinic for all patients with clinicNumFrom to clinicNumTo.</summary>
		public static void ChangeClinicsForAll(long clinicNumFrom,long clinicNumTo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),clinicNumFrom,clinicNumTo);
				return;
			}
			string command="UPDATE patient SET ClinicNum="+POut.Long(clinicNumTo)+" WHERE ClinicNum="+POut.Long(clinicNumFrom);
			Db.NonQ(command);
		}
		/// <summary>Find the most used provider for a single patient. Bias towards the most recently used provider if they have done an equal number of procedures.</summary>
		public static long ReassignProvGetMostUsed(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT ProvNum,MAX(ProcDate) MaxProcDate,COUNT(ProvNum) ProcCount "
				+"FROM procedurelog "
				+"WHERE PatNum="+POut.Long(patNum)+" "
				+"AND ProcStatus="+POut.Int((int)ProcStat.C)+" "
				+"GROUP BY ProvNum";
			DataTable table=Db.GetTable(command);
			long newProv=0;
			int mostVisits=0;
			DateTime maxProcDate=new DateTime();
			for(int i=0;i<table.Rows.Count;i++) {//loop through providers
				if(PIn.Int(table.Rows[i]["ProcCount"].ToString())>mostVisits) {//New leader for most visits.
					mostVisits=PIn.Int(table.Rows[i]["ProcCount"].ToString());
					maxProcDate=PIn.DateT(table.Rows[i]["MaxProcDate"].ToString());
					newProv=PIn.Long(table.Rows[i]["ProvNum"].ToString());
				}
				else if(PIn.Int(table.Rows[i]["ProcCount"].ToString())==mostVisits) {//Tie for most visits, use MaxProcDate as a tie breaker.
					if(PIn.DateT(table.Rows[i]["MaxProcDate"].ToString())>maxProcDate) {
						//mostVisits same as before
						maxProcDate=PIn.DateT(table.Rows[i]["MaxProcDate"].ToString());
						newProv=PIn.Long(table.Rows[i]["ProvNum"].ToString());
					}
				}
			}
			return newProv;
		}

		/// <summary>Change preferred provider PriProv to provNum for patient with PatNum=patNum.</summary>
		public static void ReassignProv(long provNum,List<long> listPatNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),provNum,listPatNums);
				return;
			}
			if(listPatNums==null || listPatNums.Count==0) {
				return;
			}
			string command="UPDATE patient SET PriProv="+POut.Long(provNum)+" WHERE PatNum IN ("+string.Join(",",listPatNums)+")";
			Db.NonQ(command);
		}

		///<summary>Gets the number of patients with unknown Zip.</summary>
		public static int GetZipUnknown(DateTime dateFrom, DateTime dateTo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),dateFrom,dateTo);
			}
			string command="SELECT COUNT(*) "
				+"FROM patient "
				+"WHERE "+DbHelper.Regexp("Zip","^[0-9]{5}",false)+" "//Does not start with five numbers
				+"AND PatNum IN ( "
					+"SELECT DISTINCT PatNum FROM procedurelog "
					+"WHERE ProcStatus="+POut.Int((int)ProcStat.C)+" "
					+"AND DateEntryC >= "+POut.Date(dateFrom)+" "
					+"AND DateEntryC <= "+POut.Date(dateTo)+") "
				+"AND Birthdate<=CURDATE() "//Birthday not in the future (at least 0 years old)
				+"AND Birthdate>SUBDATE(CURDATE(),INTERVAL 200 YEAR) ";//Younger than 200 years old
			return PIn.Int(Db.GetCount(command));
		}

		///<summary>Gets the number of qualified patients (having a completed procedure within the given time frame) in zip codes with less than 9 other qualified patients in that same zip code.</summary>
		public static int GetZipOther(DateTime dateFrom, DateTime dateTo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),dateFrom,dateTo);
			}
			string command="SELECT SUM(Patients) FROM "
				+"(SELECT SUBSTR(Zip,1,5) Zip_Code,COUNT(*) Patients "//Column headings Zip_Code and Patients are provided by the USD 2010 Manual.
				+"FROM patient "
				+"WHERE "+DbHelper.Regexp("Zip","^[0-9]{5}")+" "//Starts with five numbers
				+"AND PatNum IN ( "
					+"SELECT DISTINCT PatNum FROM procedurelog "
					+"WHERE ProcStatus="+POut.Int((int)ProcStat.C)+" "
					+"AND DateEntryC >= "+POut.Date(dateFrom)+" "
					+"AND DateEntryC <= "+POut.Date(dateTo)+") "
				+"AND Birthdate<=CURDATE() "//Birthday not in the future (at least 0 years old)
				+"AND Birthdate>SUBDATE(CURDATE(),INTERVAL 200 YEAR) "//Younger than 200 years old
				+"GROUP BY Zip "
				+"HAVING COUNT(*) < 10) patzip";//Has less than 10 patients in that zip code for the given time frame.
			return PIn.Int(Db.GetCount(command));
		}
		
		///<summary>Gets the total number of patients with completed procedures between dateFrom and dateTo. Also checks for age between 0 and 200.</summary>
		public static int GetPatCount(DateTime dateFrom, DateTime dateTo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),dateFrom,dateTo);
			}
			string command="SELECT COUNT(*) "
				+"FROM patient "
				+"WHERE PatNum IN ( "
					+"SELECT DISTINCT PatNum FROM procedurelog "
					+"WHERE ProcStatus="+POut.Int((int)ProcStat.C)+" "
					+"AND DateEntryC >= "+POut.Date(dateFrom)+" "
					+"AND DateEntryC <= "+POut.Date(dateTo)+") "
				+"AND Birthdate<=CURDATE() "//Birthday not in the future (at least 0 years old)
				+"AND Birthdate>SUBDATE(CURDATE(),INTERVAL 200 YEAR) ";//Younger than 200 years old
			return PIn.Int(Db.GetCount(command));
		}

		///<summary>Counts all patients that are not deleted.</summary>
		public static int GetPatCountAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod());
			}
			string command="SELECT COUNT(*) FROM patient WHERE PatStatus!="+POut.Int((int)PatientStatus.Deleted);
			return PIn.Int(Db.GetCount(command));
		}


		///<summary>Gets the total number of patients with completed procedures between dateFrom and dateTo who are at least agelow and strictly younger than agehigh.</summary>
		public static int GetAgeGenderCount(int agelow,int agehigh,PatientGender gender,DateTime dateFrom, DateTime dateTo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),agelow,agehigh,gender,dateFrom,dateTo);
			}
			bool male=true;//Since all the numbers must add up to equal, we count unknown and other genders as female.
			if(gender!=0) {
				male=false;
			}
			string command="SELECT COUNT(*) "
				+"FROM patient pat "
				+"WHERE PatNum IN ( "
					+"SELECT DISTINCT PatNum FROM procedurelog "
					+"WHERE ProcStatus="+POut.Int((int)ProcStat.C)+" "
					+"AND DateEntryC >= "+POut.Date(dateFrom)+" "
					+"AND DateEntryC <= "+POut.Date(dateTo)+") "
				+"AND Gender"+(male?"=0":"!=0")+" "
				+"AND Birthdate<=SUBDATE(CURDATE(),INTERVAL "+agelow+" YEAR) "//Born before this date
				+"AND Birthdate>SUBDATE(CURDATE(),INTERVAL "+agehigh+" YEAR)";//Born after this date
			return PIn.Int(Db.GetCount(command));
		}

		///<summary>Gets completed procedures, adjustments, and pay plan charges for a superfamily, ordered by datetime.</summary>
		public static DataTable GetSuperFamProcAdjustsPPCharges(long superFamily) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),superFamily);
			}
			List<long> listPatNums=Patients.GetBySuperFamily(superFamily)
				.Where(x => x.HasSuperBilling)
				.Select(x => x.PatNum).ToList();
			string command="SELECT * FROM ("
				+"SELECT procedurelog.ProcNum AS 'PriKey', procedurelog.ProcDate AS 'Date', procedurelog.PatNum AS 'PatNum', procedurelog.ProvNum AS 'Prov' "
					+",procedurelog.ProcFee AS 'Amount', procedurelog.CodeNum AS 'Code', procedurelog.ToothNum AS 'Tooth', '' AS 'AdjType', '' AS 'ChargeType'"
					+", "+DbHelper.Concat("patient.LName","', '","patient.FName")+" AS 'PatName'"
				+"FROM procedurelog "
				+"INNER JOIN patient ON procedurelog.PatNum=patient.PatNum "
				+"WHERE procedurelog.PatNum IN ("+string.Join(",",listPatNums)+") "
				+"AND StatementNum=0 "
				+"AND procedurelog.ProcStatus="+POut.Int((int)ProcStat.C)+" "
			+"UNION ALL "
				+"SELECT adjustment.AdjNum AS 'PriKey', adjustment.AdjDate AS 'Date', adjustment.PatNum AS 'PatNum', adjustment.ProvNum AS 'Prov'"
					+", adjustment.AdjAmt AS 'Amount', '' AS 'Code', '' AS 'Tooth', adjustment.AdjType AS 'AdjType', '' AS 'ChargeType'"
					+", "+DbHelper.Concat("patient.LName","', '","patient.FName")+" AS 'PatName'"
				+"FROM adjustment "
				+"INNER JOIN patient ON adjustment.PatNum=patient.PatNum "
				+"WHERE adjustment.PatNum IN ("+string.Join(",",listPatNums)+") "
				+"AND StatementNum=0 "
			+"UNION ALL "
				+"SELECT payplancharge.PayPlanChargeNum AS 'PriKey', payplancharge.ChargeDate AS 'Date', payplancharge.PatNum AS 'PatNum'"
					+",payplancharge.ProvNum AS 'Prov', payplancharge.Principal+payplancharge.Interest AS 'Amount', '' AS 'Code', '' AS 'Tooth'"
					+",'' AS 'AdjType', payplancharge.ChargeType AS 'ChargeType',"+DbHelper.Concat("patient.LName","', '","patient.FName")+" AS 'PatName'"
				+"FROM payplancharge "
				+"INNER JOIN patient ON payplancharge.PatNum=patient.PatNum "
				+"WHERE payplancharge.PatNum IN ("+string.Join(",",listPatNums)+") "
				+"AND payplancharge.ChargeType="+POut.Int((int)PayPlanChargeType.Debit)+" "
				+"AND StatementNum=0 "
				+"AND "+POut.Bool(PrefC.GetInt(PrefName.PayPlansVersion)==(int)PayPlanVersions.AgeCreditsAndDebits)+" "
				+"AND payplancharge.ChargeDate<"+DbHelper.DateAddMonth(DbHelper.Now(),"3")+" "//Only show payplan charges less than 3 mos into the future
			+") procadj ORDER BY procadj.Date DESC";
			return Db.GetTable(command);
		}

		///<summary>Returns a list of patients belonging to the SuperFamily</summary>
		public static List<Patient> GetBySuperFamily(long SuperFamilyNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),SuperFamilyNum);
			}
			if(SuperFamilyNum==0) {
				return new List<Patient>();//return empty list
			}
			string command="SELECT * FROM patient WHERE SuperFamily="+POut.Long(SuperFamilyNum);
			return Crud.PatientCrud.TableToList(Db.GetTable(command));
		}

		///<summary>Returns a list of patients that are the guarantors for the patients in the Super Family</summary>
		public static List<Patient> GetSuperFamilyGuarantors(long SuperFamilyNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),SuperFamilyNum);
			}
			if(SuperFamilyNum==0) {
				return new List<Patient>();//return empty list
			}
			//Should also work in Oracle.
			//this query was taking 2.5 seconds on a large database
			//string command = "SELECT DISTINCT * FROM patient WHERE PatNum IN (SELECT Guarantor FROM patient WHERE SuperFamily="+POut.Long(SuperFamilyNum)+") "
			//	+"AND PatStatus!="+POut.Int((int)PatientStatus.Deleted);
			//optimized to 0.001 second runtime on same db
			string command = "SELECT DISTINCT * FROM patient WHERE SuperFamily="+POut.Long(SuperFamilyNum)
				+" AND PatStatus!="+POut.Int((int)PatientStatus.Deleted)+" AND PatNum=Guarantor";
			return Crud.PatientCrud.TableToList(Db.GetTable(command));
		}

		public static void AssignToSuperfamily(long guarantor,long superFamilyNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),guarantor,superFamilyNum);
				return;
			}
			string command="UPDATE patient SET SuperFamily="+POut.Long(superFamilyNum)+", HasSuperBilling=1 WHERE Guarantor="+POut.Long(guarantor);
			Db.NonQ(command);
		}

		public static void MoveSuperFamily(long oldSuperFamilyNum,long newSuperFamilyNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),oldSuperFamilyNum,newSuperFamilyNum);
				return;
			}
			if(oldSuperFamilyNum==0) {
				return;
			}
			string command="UPDATE patient SET SuperFamily="+newSuperFamilyNum+" WHERE SuperFamily="+oldSuperFamilyNum;
			Db.NonQ(command);
		}

		public static void DisbandSuperFamily(long SuperFamilyNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),SuperFamilyNum);
				return;
			}
			if(SuperFamilyNum==0) {
				return;
			}
			string command = "UPDATE patient SET SuperFamily=0 WHERE SuperFamily="+POut.Long(SuperFamilyNum);
			Db.NonQ(command);
		}

		public static List<Patient> GetPatsForScreenGroup(long screenGroupNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),screenGroupNum);
			}
			if(screenGroupNum==0) {
				return new List<Patient>();
			}
			string command = "SELECT * FROM patient WHERE PatNum IN (SELECT PatNum FROM screenpat WHERE ScreenGroupNum="+POut.Long(screenGroupNum)+")";
			return Crud.PatientCrud.SelectMany(command);
		}

		///<summary>Get a list of patients for FormEhrPatientExport. If provNum, clinicNum, or siteNum are =0 get all.</summary>
		public static DataTable GetExportList(long patNum, string firstName,string lastName,long provNum,long clinicNum,long siteNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),patNum,firstName,lastName,provNum,clinicNum,siteNum);
			}
			string command = "SELECT patient.PatNum, patient.FName, patient.LName, provider.Abbr AS Provider, clinic.Description AS Clinic, site.Description AS Site "
				+"FROM patient "
				+"INNER JOIN provider ON patient.PriProv=provider.ProvNum "
				+"LEFT JOIN clinic ON patient.ClinicNum=clinic.ClinicNum "
				+"LEFT JOIN site ON patient.SiteNum=site.SiteNum "
				+"WHERE patient.PatStatus=0 ";
			if(patNum != 0) {
				command+="AND patient.PatNum LIKE '%"+POut.Long(patNum)+"%' ";
			}
			if(firstName != "") {
				command+="AND patient.FName LIKE '%"+POut.String(firstName)+"%' ";
			}
			if(lastName != "") {
				command+="AND patient.LName LIKE '%"+POut.String(lastName)+"%' ";
			}
			if(provNum>0) {
				command+="AND provider.ProvNum = "+POut.Long(provNum)+" ";
			}
			if(clinicNum>0) {
				command+="AND clinic.ClinicNum = "+POut.Long(clinicNum)+" ";
			}
			if(siteNum>0) {
				command+="AND site.SiteNum = "+POut.Long(siteNum)+" ";
			}
			command+="ORDER BY patient.LName,patient.FName ";
			return (Db.GetTable(command));
		}

		///<summary>Returns a list of Patients of which this PatNum is eligible to view given PHI constraints.</summary>
		public static List<Patient> GetPatientsForPhi(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),patNum);
			}
			List<long> listPatNums=GetPatNumsForPhi(patNum);
			//If there are duplicates in listPatNums, then they will be removed because of the IN statement in the query below.
			string command="SELECT * FROM patient WHERE PatNum IN ("+string.Join(",",listPatNums)+")";
			return Crud.PatientCrud.SelectMany(command);
		}

		///<summary>Returns a list of PatNum(s) of which this PatNum is eligible to view given PHI constraints.  Used internally and also used by Patient Portal.</summary>
		public static List<long> GetPatNumsForPhi(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),patNum);
			}
			List<long> listPatNums=new List<long>();
			listPatNums.Add(patNum);
			string command="";
			if(PrefC.GetBool(PrefName.FamPhiAccess)) { //Include guarantor's family if pref is set.
				//Include any patient where this PatNum is the Guarantor.
				command="SELECT PatNum FROM patient WHERE Guarantor = "+POut.Long(patNum);
				DataTable tablePatientsG=Db.GetTable(command);
				for(int i=0;i<tablePatientsG.Rows.Count;i++) {
					listPatNums.Add(PIn.Long(tablePatientsG.Rows[i]["PatNum"].ToString()));
				}
			}
			//Include any patient where the given patient is the responsible party.
			command="SELECT PatNum FROM patient WHERE ResponsParty = "+POut.Long(patNum);
			DataTable tablePatientsR=Db.GetTable(command);
			for(int i=0;i<tablePatientsR.Rows.Count;i++) {
				listPatNums.Add(PIn.Long(tablePatientsR.Rows[i]["PatNum"].ToString()));
			}
			//Include any patient where this patient is the guardian.
			command="SELECT PatNum FROM patient "
				+"WHERE PatNum IN (SELECT guardian.PatNumChild FROM guardian WHERE guardian.IsGuardian = 1 AND guardian.PatNumGuardian="+POut.Long(patNum)+") ";
			DataTable tablePatientsD=Db.GetTable(command);
			for(int i=0;i<tablePatientsD.Rows.Count;i++) {
				listPatNums.Add(PIn.Long(tablePatientsD.Rows[i]["PatNum"].ToString()));
			}
			return listPatNums.Distinct().ToList();
		}

		///<summary>Validate password against strong password rules. Currently only used for patient portal passwords. Requirements: 8 characters, 1 uppercase character, 1 lowercase character, 1 number. Returns non-empty string if validation failed. Return string will be translated.</summary>
		public static string IsPortalPasswordValid(string newPassword) {
			//No need to check RemotingRole; no call to db.
			if(newPassword.Length<8) {
				return Lans.g("FormPatientPortal","Password must be at least 8 characters long.");
			}
			if(!Regex.IsMatch(newPassword,"[A-Z]+")) {
				return Lans.g("FormPatientPortal","Password must contain an uppercase letter.");
			}
			if(!Regex.IsMatch(newPassword,"[a-z]+")) {
				return Lans.g("FormPatientPortal","Password must contain an lowercase letter.");
			}
			if(!Regex.IsMatch(newPassword,"[0-9]+")) {
				return Lans.g("FormPatientPortal","Password must contain a number.");
			}
			return "";
		}

		///<summary>Returns a distinct list of PatNums for guarantors that have any family member with passed in clinics, or have had work done at passed in clinics.</summary>
		public static string GetClinicGuarantors(string clinicNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),clinicNums);
			}
			string clinicGuarantors="";
			//Get guarantor of patients with clinic from comma delimited list
			string command="SELECT DISTINCT Guarantor FROM patient WHERE ClinicNum IN ("+clinicNums+")";
			DataTable table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				if(i>0 || clinicGuarantors!="") {
					clinicGuarantors+=",";
				}
				clinicGuarantors+=PIn.String(table.Rows[i]["Guarantor"].ToString());
			}
			//Get guarantor of patients who have had work done at clinic in comma delimited list
			command="SELECT DISTINCT Guarantor "
				+"FROM procedurelog "
				+"INNER JOIN patient ON patient.PatNum=procedurelog.PatNum "
					+"AND patient.PatStatus !=4 "
				+"WHERE procedurelog.ProcStatus IN (1,2) "
				+"AND procedurelog.ClinicNum IN ("+clinicNums+")";
			table=Db.GetTable(command);
			for(int i=0;i<table.Rows.Count;i++) {
				if(i>0 || clinicGuarantors!="") {
					clinicGuarantors+=",";
				}
				clinicGuarantors+=PIn.String(table.Rows[i]["Guarantor"].ToString());
			}
			return clinicGuarantors;
		}

		public static List<Patient> GetPatsByEmailAddress(string emailAddress) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),emailAddress);
			}
			string command="SELECT * FROM patient WHERE Email LIKE '%"+POut.String(emailAddress)+"%'";
			return Crud.PatientCrud.SelectMany(command);
		}

		///<summary>Returns all PatNums for whom the specified PatNum is the Guarantor. If this patient is not a guarantor, returns an empty list. If the
		///patient is a guarantor, this patient's PatNum will be included in the list.</summary>
		public static List<long> GetDependents(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT PatNum FROM patient WHERE Guarantor="+POut.Long(patNum);
			return Db.GetListLong(command);
		}

		///<summary>Zeros securitylog FKey column for rows that are using the matching patNum as FKey and are related to Patient.
		///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the Patient table type.</summary>
		public static void ClearFkey(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patNum);
				return;
			}
			Crud.PatientCrud.ClearFkey(patNum);
		}

		///<summary>Zeros securitylog FKey column for rows that are using the matching patNums as FKey and are related to Patient.
		///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the Patient table type.</summary>
		public static void ClearFkey(List<long> listPatNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listPatNums);
				return;
			}
			Crud.PatientCrud.ClearFkey(listPatNums);
		}

		///<summary>List of all patients in the current family along with any patients associated to payment plans of which a member of this family is the guarantor.
		///Only gets patients that are associated to active plans.</summary>
		public static List<Patient> GetAssociatedPatients(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),patNum);
			}
			//patients associated to payment plans of which any member of this family is the guarantor UNION patients in the family
			string command="SELECT pplans.PatNum,pplans.LName,pplans.FName,pplans.MiddleI,pplans.Preferred,pplans.CreditType,pplans.Guarantor,pplans.HasIns,pplans.SSN "
				+"FROM patient pat "
				+"LEFT JOIN patient fam ON fam.Guarantor = pat.Guarantor "
				+"LEFT JOIN payplan ON payplan.Guarantor = fam.PatNum "
				+"LEFT JOIN patient pplans ON pplans.PatNum = payplan.PatNum "
				+"WHERE pat.PatNum = " +POut.Long(patNum)+" "
				+"AND payplan.IsClosed = 0 "
				+"GROUP BY pplans.PatNum,pplans.LName,pplans.FName,pplans.MiddleI,pplans.Preferred,pplans.CreditType,pplans.Guarantor,pplans.HasIns,pplans.SSN ";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0) {
				return new List<Patient>();
			}
			List<Patient> listPatLims=new List<Patient>();
			for(int i = 0;i < table.Rows.Count;i++) {
				Patient Lim=new Patient();
				Lim.PatNum     = PIn.Long(table.Rows[i]["PatNum"].ToString());
				Lim.LName      = PIn.String(table.Rows[i]["LName"].ToString());
				Lim.FName      = PIn.String(table.Rows[i]["FName"].ToString());
				Lim.MiddleI    = PIn.String(table.Rows[i]["MiddleI"].ToString());
				Lim.Preferred  = PIn.String(table.Rows[i]["Preferred"].ToString());
				Lim.CreditType = PIn.String(table.Rows[i]["CreditType"].ToString());
				Lim.Guarantor  = PIn.Long(table.Rows[i]["Guarantor"].ToString());
				Lim.HasIns     = PIn.String(table.Rows[i]["HasIns"].ToString());
				Lim.SSN        = PIn.String(table.Rows[i]["SSN"].ToString());
				listPatLims.Add(Lim);
			}
			return listPatLims;
		}

		public static List<PatComm> GetPatComms(List<long> patNums,Clinic clinic,bool isGetFamily=true) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PatComm>>(MethodBase.GetCurrentMethod(),patNums,clinic,isGetFamily);
			}
			List<PatComm> retVal = new List<PatComm>();
			if(patNums.Count<=0) {//efficient way to detect count>0; also returns false if list is empty;
				return retVal;
			}
			string command;
			List<long> patNumsSearch=new List<long>(patNums);
			if(isGetFamily) {
				command="SELECT Guarantor FROM patient WHERE PatNum IN ("+string.Join(",",patNumsSearch.Distinct())+")";
				patNumsSearch=patNumsSearch.Union(Db.GetListLong(command)).ToList();//combines and removes duplicates.
			}
			command="SELECT PatNum, PatStatus, PreferConfirmMethod, PreferContactMethod, PreferRecallMethod, PreferContactConfidential, "
				+"TxtMsgOk,HmPhone,WkPhone,WirelessPhone,Email,FName,LName,Guarantor FROM patient WHERE PatNum IN ("
				+string.Join(",",patNumsSearch.Distinct())+") ";			
			bool isUnknownNo=PrefC.GetBool(PrefName.TextMsgOkStatusTreatAsNo);
			bool isEmailValidForClinic=(EmailAddresses.GetFirstOrDefault(x => x.EmailAddressNum==clinic.EmailAddressNum)!=null);
			bool isTextingEnabledForClinic=Clinics.IsTextingEnabled(clinic.ClinicNum);
			string curCulture=System.Globalization.CultureInfo.CurrentCulture.Name.Right(2);
			return Db.GetTable(command).Select().Select(x => new PatComm(x,isEmailValidForClinic,isTextingEnabledForClinic,isUnknownNo,curCulture)).ToList();
		}

		public static List<PatComm> GetPatComms(List<Patient> listPats) {
			//No need to check RemotingRole; no call to db.
			bool isUnknownNo=PrefC.GetBool(PrefName.TextMsgOkStatusTreatAsNo);
			string curCulture=System.Globalization.CultureInfo.CurrentCulture.Name.Right(2);
			List<PatComm> listPatComms=new List<PatComm>();
			foreach(Patient pat in listPats) {
				Clinic clinic=Clinics.GetFirstOrDefault(x => x.ClinicNum==pat.ClinicNum)??Clinics.GetPracticeAsClinicZero();
				bool isEmailValidForClinic=(EmailAddresses.GetFirstOrDefault(x => x.EmailAddressNum==clinic.EmailAddressNum)!=null);
				bool isTextingEnabledForClinic=Clinics.IsTextingEnabled(clinic.ClinicNum);
				listPatComms.Add(new PatComm(pat,isEmailValidForClinic,isTextingEnabledForClinic,isUnknownNo,curCulture));
			}
			return listPatComms;
		}

		///<summary>Returns list of PatNums such that the PatNum is the max PatNum in it's group of numPerGroup PatNums ordered by PatNum ascending.
		///Example: If there are 1000 PatNums in the db and they are all sequential and each PatStatus is in the list of PatStatuses and the numPerGroup
		///is 500, the returned list would have 2 values in it, 500 and 1000.  Each number is the max PatNum such that if you selected the patients with
		///PatNum greater than the previous entry (or greater than 0 if it is the first entry) and less than or equal to the current entry you would get
		///at most numPerGroup patients (the last group could, of course, have fewer in it).</summary>
		public static List<long> GetPatNumMaxForGroups(int numPerGroup,List<PatientStatus> listPatStatuses) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),numPerGroup,listPatStatuses);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				throw new ApplicationException("GetPatNumMaxForGroups is not Oracle compatible.  Please call support.");
			}
			List<long> retval=new List<long>();
			if(numPerGroup<1) {
				return retval;
			}
			string whereClause="";
			if(listPatStatuses!=null && listPatStatuses.Count>0) {
				whereClause="WHERE PatStatus IN("+string.Join(",",listPatStatuses.Select(x => POut.Int((int)x)))+") ";
			}
			string command;
			long groupMaxPatNum=0;
			int groupNum=0;
			do {
				if(groupNum>0) {//after first loop, groupMaxPatNum will be set and guaranteed to be >0
					retval.Add(groupMaxPatNum);
				}
				command="SELECT MAX(PatNum) FROM (SELECT PatNum FROM patient "+whereClause+"ORDER BY PatNum LIMIT "+groupNum+","+numPerGroup+") patNumGroup";
				groupMaxPatNum=Db.GetLong(command);
				groupNum+=numPerGroup;
			} while(groupMaxPatNum>0);
			return retval;
		}
	}

	///<summary>A helper class to keep track of changes made to clone demographics when synching.</summary>
	[Serializable]
	public class PatientCloneDemographicChanges {
		///<summary>A list of patient fields that have changed for the clone.</summary>
		public List<PatientCloneField> ListFieldsUpdated;
		///<summary>A list of field names that have been cleared due to a clone synch.</summary>
		public List<string> ListFieldsCleared;
	}

	///<summary>A helper class to keep track of changes made to clone PatPlans when synching.</summary>
	[Serializable]
	public class PatientClonePatPlanChanges {
		///<summary>A boolean indicating if there were any PatPlan changes necessary due to a synch.</summary>
		public bool PatPlansChanged;
		///<summary>A boolean indicating if there were any PatPlan inserted due to a synch.</summary>
		public bool PatPlansInserted;
		///<summary>A string that represents all changes made to the clone's PatPlan due to a synch.</summary>
		public string StrDataUpdated;
	}

	///<summary>A helper class to keep track of changes to specific clone fields when synching.</summary>
	[Serializable]
	public class PatientCloneField {
		///<summary>The name of the field that would display to the user.  E.g. "First Name", "Middle Initial", etc.</summary>
		public string FieldName;
		///<summary>The original value of the corresponding FieldName before the synch.</summary>
		public string OldValue;
		///<summary>The value of the corresponding FieldName after the clone has been synched.</summary>
		public string NewValue;

		public PatientCloneField(string fieldName,string oldValue,string newValue) {
			FieldName=fieldName;
			OldValue=oldValue;
			NewValue=newValue;
		}
	}

	///<summary>PatComm gets the fields of the patient table that are needed to determine electronic communications.</summary>
	[Serializable]
	public class PatComm {
		public long PatNum;
		public PatientStatus PatStatus;
		public ContactMethod PreferConfirmMethod;
		public ContactMethod PreferContactMethod;
		public ContactMethod PreferRecallMethod;
		public ContactMethod PreferContactConfidential;
		public YN TxtMsgOk;
		public string HmPhone;
		public string WkPhone;
		///<summary>Do not use this number for texting. Use SmsPhone.</summary>
		public string WirelessPhone;
		public string Email;
		public string FName;
		public string LName;
		public long Guarantor;
		///<summary>Use this number for texting.</summary>
		public string SmsPhone;
		public bool IsEmailAnOption;
		public bool IsSmsAnOption;
		public bool IsSmsPhoneFormatOk;

		///<summary>Parameterless constructor required in order to be serialized.  E.g. returns a list of PatComms in Patients.GetPatComms()</summary>
		public PatComm() {
		}

		public PatComm(Patient pat,bool isEmailValidForClinic,bool isTextingEnabledForClinic,bool isUnknownNo,string curCulture) {
			PatNum=pat.PatNum;
			PatStatus=pat.PatStatus;
			PreferConfirmMethod=pat.PreferConfirmMethod;
			PreferContactMethod=pat.PreferContactMethod;
			PreferRecallMethod=pat.PreferRecallMethod;
			PreferContactConfidential=pat.PreferContactConfidential;
			TxtMsgOk=pat.TxtMsgOk;
			HmPhone=pat.HmPhone;
			WkPhone=pat.WkPhone;
			WirelessPhone=pat.WirelessPhone;
			Email=pat.Email;
			FName=pat.FName;
			LName=pat.LName;
			Guarantor=pat.Guarantor;
			SetSmsEmailFields(isEmailValidForClinic,isTextingEnabledForClinic,isUnknownNo,curCulture);
		}

		public PatComm(DataRow dataRow,bool isEmailValidForClinic,bool isTextingEnabledForClinic,bool isUnknownNo,string curCulture) {
			PatNum=PIn.Long(dataRow["PatNum"].ToString());
			PatStatus=(PatientStatus)PIn.Int(dataRow["PatStatus"].ToString());
			PreferConfirmMethod=(ContactMethod)PIn.Int(dataRow["PreferConfirmMethod"].ToString());
			PreferContactMethod=(ContactMethod)PIn.Int(dataRow["PreferContactMethod"].ToString());
			PreferRecallMethod=(ContactMethod)PIn.Int(dataRow["PreferRecallMethod"].ToString());
			PreferContactConfidential=(ContactMethod)PIn.Int(dataRow["PreferContactConfidential"].ToString());
			TxtMsgOk=(YN)PIn.Int(dataRow["TxtMsgOk"].ToString());
			HmPhone=PIn.String(dataRow["HmPhone"].ToString());
			WkPhone=PIn.String(dataRow["WkPhone"].ToString());
			WirelessPhone=PIn.String(dataRow["WirelessPhone"].ToString());
			Email=PIn.String(dataRow["Email"].ToString());
			FName=PIn.String(dataRow["FName"].ToString());
			LName=PIn.String(dataRow["LName"].ToString());
			Guarantor=PIn.Long(dataRow["Guarantor"].ToString());
			SetSmsEmailFields(isEmailValidForClinic,isTextingEnabledForClinic,isUnknownNo,curCulture);
		}
		
		private void SetSmsEmailFields(bool isEmailValidForClinic,bool isTextingEnabledForClinic,bool isUnknownNo,string curCulture) {
			IsSmsPhoneFormatOk=false;
			if(TxtMsgOk==YN.No||(isUnknownNo&&TxtMsgOk==YN.Unknown)) {
				SmsPhone="";
			}
			else {
				//Previously chose between WirelessPhone,HmPhone,WkPhone. Now chooses WirelessPhone or nothing at all.
				SmsPhone=new[] { WirelessPhone }.FirstOrDefault(y => !string.IsNullOrWhiteSpace(y))??"";
				try {
					SmsPhone=SmsToMobiles.ConvertPhoneToInternational(SmsPhone,curCulture);
					IsSmsPhoneFormatOk=true;
				}
				catch(Exception e) { //Formatting for sms failed to set to empty so we don't try to use it.
					e.DoNothing();
					SmsPhone="";
				}				
			}			
			IsSmsAnOption=
				//SmsPhone is in proper format for sms send.
				IsSmsPhoneFormatOk
				//Sms is allowed by practice and patient.
				&& (TxtMsgOk==YN.Yes || (TxtMsgOk==YN.Unknown&&!isUnknownNo))
				//Patient has a valid phone number.
				&& !string.IsNullOrWhiteSpace(SmsPhone)
				//Clinic linked to this PatComm supports texting.
				&& isTextingEnabledForClinic;			
			IsEmailAnOption=
				//Patient has a valid email.
				!string.IsNullOrWhiteSpace(Email)
				//Clinic linked to this PatComm has a valid email.
				&& isEmailValidForClinic;
		}	
	}

	///<summary>Not a database table.  Just used in billing and finance charges.</summary>
	public class PatAging{
		///<summary></summary>
		public long PatNum;
		///<summary></summary>
		public double Bal_0_30;
		///<summary></summary>
		public double Bal_31_60;
		///<summary></summary>
		public double Bal_61_90;
		///<summary></summary>
		public double BalOver90;
		///<summary></summary>
		public double InsEst;
		///<summary></summary>
		public string PatName;
		///<summary></summary>
		public double BalTotal;
		///<summary></summary>
		public double AmountDue;
		///<summary>The patient priprov to assign the finance charge to.</summary>
		public long PriProv;
		///<summary>The date of the last statement.</summary>
		public DateTime DateLastStatement;
		///<summary>FK to defNum.</summary>
		public long BillingType;
		///<summary>Only set in some areas.</summary>
		public long ClinicNum;
		///<summary></summary>
		public double PayPlanDue;
		///<summary></summary>
		public long SuperFamily;
		///<summary></summary>
		public bool HasSuperBilling;
		///<summary></summary>
		public long Guarantor;
		///<summary>Only used for Transworld AR Manager.</summary>
		public DateTime DateLastPay;
		///<summary>Used to exclude bad addresses from the list.  Currently only for A/R manager.</summary>
		public string Zip;
		///<summary>Only used for Transworld AR Manager.</summary>
		public bool HasUnsentProcs;
		///<summary>Only used for Transworld AR Manager.</summary>
		public bool HasInsPending;
		///<summary>For accounts sent to Transworld for collection.  All trans sent to TSI for this guarantor, ordered by TransDateTime descending.</summary>
		public List<TsiTransLog> ListTsiLogs;

		///<summary></summary>
		public PatAging Copy() {
			PatAging retval=(PatAging)this.MemberwiseClone();
			retval.ListTsiLogs=this.ListTsiLogs.Select(x => x.Copy()).ToList();
			return retval;
		}
	}

}