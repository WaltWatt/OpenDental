using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests.IntegrationTests {
	[TestClass]
	public class ProceduresTests:TestBase {

		[TestMethod]
		public void Procedures_GlobalUpdateFees() {
			string name=MethodBase.GetCurrentMethod().Name;
			Random rand=new Random();
			//set up fees
			List<Clinic> listClinics=new List<Clinic>();
			for(int i=0;i<3;i++) {
				listClinics.Add(ClinicT.CreateClinic(name+"_"+i));
			}
			List<long> listFeeSchedNums=new List<long>();
			for(int i=0;i<2;i++) {
				listFeeSchedNums.Add(FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,name+"_"+i,false));
			}
			List<long> listProvNums=new List<long>();
			for(int i=0;i<2;i++) {
				long feeSched=listFeeSchedNums[rand.Next(listFeeSchedNums.Count-1)];
				listProvNums.Add(ProviderT.CreateProvider(name+"_"+i,feeSchedNum:feeSched));
			}
			List<ProcedureCode> listProcCodes=ProcedureCodes.GetAllCodes();
			List<Fee> listFees=new List<Fee>();
			foreach(ProcedureCode procCode in listProcCodes) {
				foreach(long feeSched in listFeeSchedNums) {
					foreach(Clinic clinic in listClinics) {
						foreach(long provNum in listProvNums) {
							listFees.Add(FeeT.GetNewFee(feeSched,procCode.CodeNum,50*rand.NextDouble(),clinic.ClinicNum,provNum));
						}
					}
				}
			}
			//set up patients
			List<Patient> listPatients=new List<Patient>();
			for(int i=0;i<3;i++) {
				listPatients.Add(PatientT.CreatePatient(name+"_"+i,
					listProvNums[rand.Next(listProvNums.Count-1)],
					listClinics[rand.Next(listClinics.Count-1)].ClinicNum));
			}
			//TP some procedures
			List<Fee> listTPFees=new List<Fee>();
			for(int i=0;i<100;i++) {
				ProcedureCode procCode=listProcCodes[rand.Next(listProcCodes.Count-1)];
				Patient patient=listPatients[rand.Next(listPatients.Count-1)];
				Fee fee=Fees.GetFee(procCode.CodeNum,Providers.GetProv(patient.PriProv).FeeSched,patient.ClinicNum,patient.PriProv);
				Procedure proc=ProcedureT.CreateProcedure(patient,procCode.ProcCode,ProcStat.TP,"",fee.Amount);
				listTPFees.Add(fee);
			}
			//change some of the fees
			List<Fee> listTPFeesChanged=listTPFees.OrderBy(x => rand.Next()).Take(50).ToList();
			List<Fee> listNonTPFeesChanged=(listFees.Except(listTPFees)).OrderBy(x => rand.Next()).Take(50).ToList();
			FeeCache cache=new FeeCache(listTPFeesChanged.Union(listNonTPFeesChanged).ToList());
			cache.BeginTransaction();
			foreach(Fee fee in listTPFeesChanged) {
				fee.Amount=50*rand.NextDouble();
				cache.Update(fee);
			}
			foreach(Fee fee in listNonTPFeesChanged) {
				fee.Amount=50*rand.NextDouble();
				cache.Update(fee);
			}
			cache.SaveToDb();
			//Run the global update
			long updatedCount=0;
			cache=new FeeCache();
			for(int i=0;i<listClinics.Count;i++) {
				updatedCount+=Procedures.GlobalUpdateFees(cache.GetFeesForClinics(listClinics.Select(x => x.ClinicNum)),listClinics[i].ClinicNum, listClinics[i].Abbr);
			}
			//check the counts are the same
			List<Fee> listToCount=listTPFees.Where(x => listTPFeesChanged.Select(y => y.FeeNum).Contains(x.FeeNum)).ToList();
			Assert.AreEqual(listToCount.Count,updatedCount);
		}
	}
}
