using System;
using System.Collections.Generic;
using System.Text;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class PatientT {
		///<summary>Creates a patient.  Practice default provider and billing type.</summary>
		public static Patient CreatePatient(string suffix="",long priProvNum=0,long clinicNum=0,string email="",string phone="",
			ContactMethod contactMethod=ContactMethod.Email,string lName="",string fName="",string preferredName="",DateTime birthDate=default(DateTime)
			,long secProvNum=0) 
		{
			Patient pat=new Patient {
				Email=email,
				PreferConfirmMethod=contactMethod,
				PreferContactConfidential=contactMethod,
				PreferContactMethod=contactMethod,
				PreferRecallMethod=contactMethod,
				HmPhone=phone,
				WirelessPhone=phone,
				IsNew=true,
				LName=lName+suffix,
				FName=fName+suffix,
				BillingType=PrefC.GetLong(PrefName.PracticeDefaultBillType),
				ClinicNum=clinicNum,
				Preferred=preferredName,
				Birthdate=birthDate,
				SecProv=secProvNum,
			};
			if(priProvNum!=0) {
				pat.PriProv=priProvNum;
			}
			else {
				pat.PriProv=PrefC.GetLong(PrefName.PracticeDefaultProv);//This causes standard fee sched to be 53.
			}
			Patients.Insert(pat,false);
			Patient oldPatient=pat.Copy();
			pat.Guarantor=pat.PatNum;
			if(suffix=="" && lName=="" && fName=="") {
				pat.FName=pat.PatNum.ToString()+"F";
				pat.LName=pat.PatNum.ToString()+"L";
			}
			Patients.Update(pat,oldPatient);
			return pat;
		}

		public static void SetGuarantor(Patient pat,long guarantorNum){
			Patient oldPatient=pat.Copy();
			pat.Guarantor=guarantorNum;
			Patients.Update(pat,oldPatient);
		}
		
		///<summary>Deletes everything from the patient table.  Does not truncate the table so that PKs are not reused on accident.</summary>
		public static void ClearPatientTable() {
			string command="DELETE FROM patient WHERE PatNum > 0";
			DataCore.NonQ(command);
		}

	}
}
