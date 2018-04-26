using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests {
	[TestClass]
	public class PatientsTest:TestBase {

		[TestInitialize] 
		public void RunBeforeEachTest() {
			PatientT.ClearPatientTable();
		}

		#region GetPatientsByPartialName

		[TestMethod]
		public void Patients_GetPatientsByPartialName_LastAndFirst() {
			PatientT.CreatePatient(lName: "Owre",fName: "Sam");
			PatientT.CreatePatient(lName: "Owre",fName: "Sarah");
			List<Patient> listPats=Patients.GetPatientsByPartialName("sam owre");
			Assert.AreEqual(1,listPats.Count);
		}

		[TestMethod]
		public void Patients_GetPatientsByPartialName_MatchTwoLastAndFirst() {
			PatientT.CreatePatient(lName: "OWRE",fName: "sam");
			PatientT.CreatePatient(lName: "Owre",fName: "sarah");
			List<Patient> listPats=Patients.GetPatientsByPartialName("owre s");
			Assert.AreEqual(2,listPats.Count);
		}

		[TestMethod]
		public void Patients_GetPatientsByPartialName_JustFirst() {
			PatientT.CreatePatient(lName: "Owre",fName: "Sam");
			PatientT.CreatePatient(lName: "Owre",fName: "Sarah");
			List<Patient> listPats=Patients.GetPatientsByPartialName("SaRah");
			Assert.AreEqual(1,listPats.Count);
		}

		[TestMethod]
		public void Patients_GetPatientsByPartialName_JustLast() {
			PatientT.CreatePatient(lName: "Owre",fName: "Sam");
			PatientT.CreatePatient(lName: "Owre",fName: "Sarah");
			List<Patient> listPats=Patients.GetPatientsByPartialName("OWRE");
			Assert.AreEqual(2,listPats.Count);
		}

		[TestMethod]
		public void Patients_GetPatientsByPartialName_FirstNotPreferred() {
			PatientT.CreatePatient(lName: "Brock",fName: "Taylor");
			PatientT.CreatePatient(lName: "McGehee",fName: "Christopher",preferredName: "Chris");
			List<Patient> listPats=Patients.GetPatientsByPartialName("chris owre");
			Assert.AreEqual(0,listPats.Count);
		}

		[TestMethod]
		public void Patients_GetPatientsByPartialName_LastAndPreferred() {
			PatientT.CreatePatient(lName: "Jansen",fName: "Andrew");
			PatientT.CreatePatient(lName: "Montano",fName: "Joseph",preferredName: "Joe");
			List<Patient> listPats=Patients.GetPatientsByPartialName("Joe Montano");
			Assert.AreEqual(1,listPats.Count);
		}

		[TestMethod]
		public void Patients_GetPatientsByPartialName_TwoWordLastName() {
			PatientT.CreatePatient(lName: "Owre",fName: "Sam");
			PatientT.CreatePatient(lName: "Van Damme",fName: "Jean-Claude");
			List<Patient> listPats=Patients.GetPatientsByPartialName("van damme");
			Assert.AreEqual(1,listPats.Count);
		}

		[TestMethod]
		public void Patients_GetPatientsByPartialName_TwoWordLastNamePlusFirst() {
			PatientT.CreatePatient(lName: "Owre",fName: "Sam");
			PatientT.CreatePatient(lName: "Van Damme",fName: "Jean-Claude");
			List<Patient> listPats=Patients.GetPatientsByPartialName("sam van damme");
			Assert.AreEqual(0,listPats.Count);
		}

		[TestMethod]
		public void Patients_GetPatientsByPartialName_LotsOfNames() {
			PatientT.CreatePatient(lName: "Salmon",fName: "Jason");
			PatientT.CreatePatient(lName: "Jansen",fName: "Andrew");
			List<Patient> listPats=Patients.GetPatientsByPartialName("andrew jansen thinks programming is fun");
			Assert.AreEqual(0,listPats.Count);
		}

		[TestMethod]
		public void Patients_GetPatientsByPartialName_SameNames() {
			PatientT.CreatePatient(lName: "Owre",fName: "Sam");
			PatientT.CreatePatient(lName: "Owre",fName: "Sarah");
			List<Patient> listPats=Patients.GetPatientsByPartialName("sam sam");
			Assert.AreEqual(1,listPats.Count);
		}


		[TestMethod]
		public void Patients_GetPatientsByPartialName_AllCaps() {
			PatientT.CreatePatient(lName: "Buchanan",fName: "Cameron");
			PatientT.CreatePatient(lName: "Montano",fName: "Joseph");
			List<Patient> listPats=Patients.GetPatientsByPartialName("CAMERON BUCHANAN");
			Assert.AreEqual(1,listPats.Count);
		}


		[TestMethod]
		public void Patients_GetPatientsByPartialName_Everybody() {
			PatientT.CreatePatient(lName: "Buchanan",fName: "Cameron");
			PatientT.CreatePatient(lName: "Montano",fName: "Joseph");
			PatientT.CreatePatient(lName: "Owre",fName: "Sam");
			PatientT.CreatePatient(lName: "Owre",fName: "Sarah");
			PatientT.CreatePatient(lName: "Salmon",fName: "Jason");
			PatientT.CreatePatient(lName: "Jansen",fName: "Andrew");
			PatientT.CreatePatient(lName: "Van Damme",fName: "Jean-Claude");
			PatientT.CreatePatient(lName: "Brock",fName: "Taylor");
			PatientT.CreatePatient(lName: "McGehee",fName: "Christopher",preferredName: "Chris");
			List<Patient> listPats=Patients.GetPatientsByPartialName("");
			Assert.AreEqual(9,listPats.Count);
		}
		#endregion GetPatientsByPartialName

	}
}
