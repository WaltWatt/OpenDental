using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestsCore;
using OpenDentBusiness;
using System.Reflection;

namespace UnitTests.UnitTests {
	[TestClass]
	public class PaymentsTest:TestBase {
		#region Prepayment Test
		private List<PaySplit> _listFamPrePaySplits;
		private List<PaySplit> _listPosPrePay;
		private List<PaySplit> _listNegPrePay;
		#endregion
		[ClassInitialize]
		public static void SetUp(TestContext context) {

		}

		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_SplitForPaymentLessThanTotalofProcs() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",40);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",40);
			Payment pay=PaymentT.MakePaymentNoSplits(pat.PatNum,50);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,pay,new List<long> { pat.PatNum },true,false);
			PaymentEdit.ConstructResults chargeResult=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum },pat.PatNum
				,loadData.ConstructChargesData.ListPaySplits,pay,new List<Procedure>());
			PaymentEdit.AutoSplit autoSplit=PaymentEdit.AutoSplitForPayment(chargeResult);
			Assert.AreEqual(2,autoSplit.ListAutoSplits.Count);
			Assert.AreEqual(1,autoSplit.ListAutoSplits.Count(x => x.SplitAmt.IsEqual(40)));
			Assert.AreEqual(1,autoSplit.ListAutoSplits.Count(x => x.SplitAmt.IsEqual(10)));
		}

		[TestMethod]
		public void PaymentEdit_AutoSplitForPayment_NoNegativeAutoSplits() {
			long provNumA=ProviderT.CreateProvider("provA");
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",70);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0150",ProcStat.C,"",20);
			//make an overpayment for one of the procedures so it spills over.
			DateTime payDate=DateTime.Today;
			Payment pay=PaymentT.MakePayment(pat.PatNum,71,payDate,procNum:proc1.ProcNum);//pre-existing payment
			//attempt to make another payment. Auto splits should not suggest a negative split.
			Payment newPayment=PaymentT.MakePaymentNoSplits(pat.PatNum,2,payDate,isNew:true,
				payType:Defs.GetDefsForCategory(DefCat.PaymentTypes,true)[0].DefNum);//current payment we're trying to make
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,newPayment,new List<long>() {pat.PatNum },true,false);
			PaymentEdit.ConstructChargesData chargeData=PaymentEdit.GetConstructChargesData(new List<long> {pat.PatNum },pat.PatNum,
				PaySplits.GetForPayment(pay.PayNum),pay.PayNum,false);
			PaymentEdit.ConstructResults constructResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum },pat.PatNum
				,chargeData.ListPaySplits,newPayment,new List<Procedure> ());
			PaymentEdit.AutoSplit autoSplits=PaymentEdit.AutoSplitForPayment(constructResults);
			Assert.AreEqual(0,autoSplits.ListAutoSplits.FindAll(x => x.SplitAmt<0).Count);//assert no negative auto splits were made.
			Assert.AreEqual(0,autoSplits.ListSplitsCur.FindAll(x => x.SplitAmt<0).Count);//auto splits not catching everything
		}

		[TestMethod]
		public void PaymentEdit_Init_CorrectlyOrderedAutoSplits() {//Legacy_TestFortyOne
			string suffix="41";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",50,DateTime.Now.AddDays(-1));
			Procedure procedure2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",40,DateTime.Now.AddDays(-2));
			Procedure procedure3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Now.AddDays(-3));
			Payment payment=PaymentT.MakePaymentNoSplits(patNum,150);
			Family famForPat=Patients.GetFamily(patNum);
			List<Patient> listFamForPat=famForPat.ListPats.ToList();
			PaymentEdit.InitData init=PaymentEdit.Init(listFamForPat,famForPat,new Family { },payment,new List<PaySplit>(),new List<Procedure>(),patNum);
			//Auto Splits will be in opposite order from least recent to most recent.
			Assert.AreEqual(3,init.AutoSplitData.ListSplitsCur.Count);
			Assert.IsFalse(init.AutoSplitData.ListSplitsCur[0].SplitAmt!=60 || init.AutoSplitData.ListSplitsCur[0].ProcNum!=procedure3.ProcNum);
			Assert.IsFalse(init.AutoSplitData.ListSplitsCur[1].SplitAmt!=40 || init.AutoSplitData.ListSplitsCur[1].ProcNum!=procedure2.ProcNum);
			Assert.IsFalse(init.AutoSplitData.ListSplitsCur[2].SplitAmt!=50 || init.AutoSplitData.ListSplitsCur[2].ProcNum!=procedure1.ProcNum);
		}

		[TestMethod]
		public void PaymentEdit_Init_CorrectlyOrderedAutoSplitsWithExistingPayment() {//Legacy_TestFortyTwo
			string suffix="42";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",40,DateTime.Now.AddDays(-1));
			Procedure procedure2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",60,DateTime.Now.AddDays(-2));
			Procedure procedure3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",80,DateTime.Now.AddDays(-3));
			Payment payment1=PaymentT.MakePayment(patNum,110,DateTime.Now.AddDays(-2));
			Payment payment2=PaymentT.MakePaymentNoSplits(patNum,80,DateTime.Today);
			Family famForPat=Patients.GetFamily(patNum);
			List<Patient> listFamForPat=famForPat.ListPats.ToList();
			PaymentEdit.InitData init=PaymentEdit.Init(listFamForPat,famForPat,new Family { },payment2,new List<PaySplit>(),new List<Procedure>(),patNum);
			//Auto Splits will be in opposite order from least recent to most recent.
			//ListSplitsCur should contain three paysplits, one for procedure1 for 40, and one for procedure2 for 30,
			//and an unallocated split for 10 with the remainder on the payment (40+30+10=80).
			Assert.AreEqual(3,init.AutoSplitData.ListSplitsCur.Count);
			Assert.IsFalse(init.AutoSplitData.ListSplitsCur[0].SplitAmt!=30 || init.AutoSplitData.ListSplitsCur[0].ProcNum!=procedure2.ProcNum);
			Assert.IsFalse(init.AutoSplitData.ListSplitsCur[1].SplitAmt!=40 || init.AutoSplitData.ListSplitsCur[1].ProcNum!=procedure1.ProcNum);
			Assert.IsFalse(init.AutoSplitData.ListSplitsCur[2].SplitAmt!=10 || init.AutoSplitData.ListSplitsCur[2].ProcNum!=0);
		}

		[TestMethod]
		public void PaymentEdit_Init_AutoSplitOverAllocation() {//Legacy_TestFortyThree
			string suffix="43";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",40,DateTime.Now.AddDays(-1));
			Procedure procedure2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",60,DateTime.Now.AddDays(-2));
			Procedure procedure3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",80,DateTime.Now.AddDays(-3));
			Payment payment1=PaymentT.MakePayment(patNum,200,DateTime.Now.AddDays(-2));
			Payment payment2=PaymentT.MakePaymentNoSplits(patNum,50,DateTime.Today);
			Family famForPat=Patients.GetFamily(patNum);
			List<Patient> listFamForPat=famForPat.ListPats.ToList();
			PaymentEdit.InitData init=PaymentEdit.Init(listFamForPat,famForPat,new Family { },payment2,new List<PaySplit>(),new List<Procedure>(),patNum);
			//Auto Splits will be in opposite order from least recent to most recent.
			//ListSplitsCur should contain one paysplit worth 50 and not attached to any procedures.
			Assert.AreEqual(1,init.AutoSplitData.ListSplitsCur.Count);
			Assert.IsFalse(init.AutoSplitData.ListSplitsCur[0].SplitAmt!=50 || init.AutoSplitData.ListSplitsCur[0].ProcNum!=0);
		}

		[TestMethod]
		public void PaymentEdit_Init_AutoSplitForPaymentNegativePaymentAmount() {//Legacy_TestFortyFour
			string suffix="44";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",40,DateTime.Now.AddDays(-1));
			Payment payment=PaymentT.MakePaymentNoSplits(patNum,-50,DateTime.Today);
			Family famForPat=Patients.GetFamily(patNum);
			List<Patient> listFamForPat=famForPat.ListPats.ToList();
			PaymentEdit.InitData init=PaymentEdit.Init(listFamForPat,famForPat,new Family { },payment,new List<PaySplit>(),new List<Procedure>(),patNum);
			//Auto Splits will be in opposite order from least recent to most recent.
			//ListSplitsCur should contain no paysplits since it doesn't make sense to create negative payments when there are outstanding charges.
			Assert.AreEqual(0,init.AutoSplitData.ListSplitsCur.Count);
		}

		[TestMethod]
		public void PaymentEdit_Init_AutoSplitWithAdjustmentAndExistingPayment() {//Legacy_TestFortyFive
			string suffix="45";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",40,DateTime.Now.AddDays(-1));
			Procedure procedure2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",60,DateTime.Now.AddDays(-2));
			Procedure procedure3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",80,DateTime.Now.AddDays(-3));
			Adjustment adjustment=AdjustmentT.MakeAdjustment(patNum,-40,procDate:DateTime.Now.AddDays(-2));
			Payment payment=PaymentT.MakePayment(patNum,100,DateTime.Now.AddDays(-2),procNum:procedure3.ProcNum);
			Payment payment2=PaymentT.MakePaymentNoSplits(patNum,50,DateTime.Today);
			Family famForPat=Patients.GetFamily(patNum);
			List<Patient> listFamForPat=famForPat.ListPats.ToList();
			PaymentEdit.InitData init=PaymentEdit.Init(listFamForPat,famForPat,new Family { },payment2,new List<PaySplit>(),new List<Procedure>(),patNum);
			//Auto Splits will be in opposite order from least recent to most recent.
			//ListSplitsCur should contain two paysplits, 40 attached to the D1110 and another for the remainder of 10, not attached to any procedure.
			Assert.AreEqual(2,init.AutoSplitData.ListSplitsCur.Count);
			Assert.IsFalse(init.AutoSplitData.ListSplitsCur[0].SplitAmt!=40 || init.AutoSplitData.ListSplitsCur[0].ProcNum!=procedure1.ProcNum);
			Assert.IsFalse(init.AutoSplitData.ListSplitsCur[1].SplitAmt!=10 || init.AutoSplitData.ListSplitsCur[1].ProcNum!=0);
		}

		[TestMethod]
		public void PaymentEdit_Init_AutoSplitForPaymentNegativePaymentAmountNegProcedure() {//Legacy_TestFortySix
			string suffix="46";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",-40,DateTime.Now.AddDays(-1));
			Payment payment=PaymentT.MakePaymentNoSplits(patNum,-50,DateTime.Today);
			Family famForPat=Patients.GetFamily(patNum);
			List<Patient> listFamForPat=famForPat.ListPats.ToList();
			PaymentEdit.InitData init=PaymentEdit.Init(listFamForPat,famForPat,new Family { },payment,new List<PaySplit>(),new List<Procedure>(),patNum);
			//Auto Splits will be in opposite order from least recent to most recent.
			//ListSplitsCur should contain one paysplit for the amount passed in that is unallocated.
			Assert.AreEqual(1,init.AutoSplitData.ListSplitsCur.Count);
			Assert.IsFalse(init.AutoSplitData.ListSplitsCur[0].SplitAmt!=-50 || init.AutoSplitData.ListSplitsCur[0].ProcNum!=0);
		}

		[TestMethod]
		public void PaymentEdit_Init_AutoSplitProcedureGuarantor() {//Legacy_TestFortySeven
			string suffix="47";
			Patient pat=PatientT.CreatePatient(suffix);
			Patient patOld=PatientT.CreatePatient(suffix+"fam");
			Patient pat2=patOld.Copy();
			long patNum=pat.PatNum;
			pat2.Guarantor=patNum;
			Patients.Update(pat2,patOld);
			long patNum2=pat2.PatNum;
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",50,DateTime.Now.AddDays(-1));
			Procedure procedure2=ProcedureT.CreateProcedure(pat2,"D0120",ProcStat.C,"",40,DateTime.Now.AddDays(-2));
			Procedure procedure3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Now.AddDays(-3));
			Payment payment=PaymentT.MakePaymentNoSplits(patNum,150,DateTime.Today);
			Family famForPat=Patients.GetFamily(patNum);
			List<Patient> listFamForPat=famForPat.ListPats.ToList();
			PaymentEdit.InitData init=PaymentEdit.Init(listFamForPat,famForPat,new Family { },payment,new List<PaySplit>(),new List<Procedure>(),patNum);
			//Auto Splits will be in opposite order from least recent to most recent.
			Assert.AreEqual(3,init.AutoSplitData.ListSplitsCur.Count);
			Assert.IsFalse(init.AutoSplitData.ListSplitsCur[0].SplitAmt!=60 || init.AutoSplitData.ListSplitsCur[0].ProcNum!=procedure3.ProcNum 
				|| init.AutoSplitData.ListSplitsCur[0].PatNum!=patNum);
			Assert.IsFalse(init.AutoSplitData.ListSplitsCur[1].SplitAmt!=40 || init.AutoSplitData.ListSplitsCur[1].ProcNum!=procedure2.ProcNum 
				|| init.AutoSplitData.ListSplitsCur[1].PatNum!=patNum2);
			Assert.IsFalse(init.AutoSplitData.ListSplitsCur[2].SplitAmt!=50 || init.AutoSplitData.ListSplitsCur[2].ProcNum!=procedure1.ProcNum 
				|| init.AutoSplitData.ListSplitsCur[2].PatNum!=patNum);
		}

		[TestMethod]
		public void PaymentEdit_Init_AutoSplitWithClaimPayments() {//Legacy_TestFortyEight
			string suffix="48";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			InsPlan insPlan=InsPlanT.CreateInsPlan(CarrierT.CreateCarrier(suffix).CarrierNum);
			InsSub insSub=InsSubT.CreateInsSub(patNum,insPlan.PlanNum);			
			PatPlan patPlan=PatPlanT.CreatePatPlan(1,patNum,insSub.InsSubNum);
			Procedure procedure1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",50,DateTime.Now.AddDays(-1));
			Procedure procedure2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",40,DateTime.Now.AddDays(-2));
			Procedure procedure3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Now.AddDays(-3));
			ClaimProcT.AddInsPaid(patNum,insPlan.PlanNum,procedure1.ProcNum,20,insSub.InsSubNum,0,0);
			ClaimProcT.AddInsPaid(patNum,insPlan.PlanNum,procedure2.ProcNum,5,insSub.InsSubNum,5,0);
			ClaimProcT.AddInsPaid(patNum,insPlan.PlanNum,procedure3.ProcNum,20,insSub.InsSubNum,0,10);
			Payment payment=PaymentT.MakePaymentNoSplits(patNum,150,DateTime.Today);
			Family famForPat=Patients.GetFamily(patNum);
			List<Patient> listFamForPat=famForPat.ListPats.ToList();
			PaymentEdit.InitData init=PaymentEdit.Init(listFamForPat,famForPat,new Family { },payment,new List<PaySplit>(),new List<Procedure>(),patNum);
			//Auto Splits will be in opposite order from least recent to most recent.
			//ListSplitsCur should contain four splits, 30, 35, and 30, then one unallocated for the remainder of the payment 55.
			Assert.AreEqual(4,init.AutoSplitData.ListSplitsCur.Count);
			Assert.IsFalse(init.AutoSplitData.ListSplitsCur[0].SplitAmt!=40 || init.AutoSplitData.ListSplitsCur[0].ProcNum!=procedure3.ProcNum 
				|| init.AutoSplitData.ListSplitsCur[0].PatNum!=patNum);
			Assert.IsFalse(init.AutoSplitData.ListSplitsCur[1].SplitAmt!=35 || init.AutoSplitData.ListSplitsCur[1].ProcNum!=procedure2.ProcNum 
				|| init.AutoSplitData.ListSplitsCur[1].PatNum!=patNum);
			Assert.IsFalse(init.AutoSplitData.ListSplitsCur[2].SplitAmt!=30 || init.AutoSplitData.ListSplitsCur[2].ProcNum!=procedure1.ProcNum 
				|| init.AutoSplitData.ListSplitsCur[2].PatNum!=patNum);
			Assert.IsFalse(init.AutoSplitData.ListSplitsCur[3].SplitAmt!=45 || init.AutoSplitData.ListSplitsCur[3].ProcNum!=0);
		}

		[TestMethod]
		public void PaymentEdit_Init_FIFOWithPosAdjustment() {
			PrefT.UpdateInt(PrefName.AutoSplitLogic,(int)AutoSplitPreference.FIFO);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("prov1");
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",75,DateTime.Today.AddMonths(-1),provNum:provNum);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",135,DateTime.Today.AddMonths(-1).AddDays(1),provNum:provNum);
			Adjustment adjustment=AdjustmentT.MakeAdjustment(pat.PatNum,20,DateTime.Today.AddDays(-15),provNum:provNum);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,20);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payCur,new List<long>{pat.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat.PatNum),new Family { },payCur
					,loadData.ListSplits,new List<Procedure>(),pat.PatNum,loadData:loadData);
			//Verify the logic pays starts to pay off the first procedure
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(Procedure) && x.AmountEnd==55));
		}

		[TestMethod]
		public void PaymentEdit_Init_AdjustmentPreferWithPosAdjustment() {
			PrefT.UpdateInt(PrefName.AutoSplitLogic,(int)AutoSplitPreference.Adjustments);
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("prov1");
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",75,DateTime.Today.AddMonths(-1),provNum:provNum);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",135,DateTime.Today.AddMonths(-1).AddDays(1),provNum:provNum);
			Adjustment adjustment=AdjustmentT.MakeAdjustment(pat.PatNum,20,DateTime.Today.AddDays(-15),provNum:provNum);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,20);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payCur,new List<long>{pat.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat.PatNum),new Family { },payCur
					,loadData.ListSplits,new List<Procedure>(),pat.PatNum,loadData:loadData);
			//Verify the logic chooses to pay off the adjustment first
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(Adjustment) && x.AmountEnd==0));
		}

		[TestMethod]
		public void PaymentEdit_Init_PayPlanChargesWithUnattachedCredits() {
			//new payplan
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",135,DateTime.Today.AddMonths(-4));
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Today.AddMonths(-4));
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,30,DateTime.Today.AddMonths(-3),0,totalAmt:195);
			//Go to make a payment for the charges due
			Payment pay=PaymentT.MakePaymentNoSplits(pat.PatNum,60,DateTime.Today);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,pay,new List<long> {pat.PatNum},true,false);
			PaymentEdit.ConstructResults constructResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum },pat.PatNum
				,loadData.ConstructChargesData.ListPaySplits,pay,new List<Procedure> (),loadData:loadData);
			Assert.AreEqual(6,constructResults.ListAccountCharges.Count);//2 procedures and 4 months of charges since unattached credits.
			Assert.AreEqual(2,constructResults.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(Procedure)));
			Assert.AreEqual(4,constructResults.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(PayPlanCharge)));
		}

		[TestMethod]
		public void PaymentEdit_Init_PayPlanChargesWithAttachedCredits() {
			//new payplan
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",135,DateTime.Today.AddMonths(-4));
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Today.AddMonths(-4));
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,30,DateTime.Today.AddMonths(-3),0,new List<Procedure>() {proc1,proc2});
			//Go to make a payment for the charges that are due
			Payment pay=PaymentT.MakePaymentNoSplits(pat.PatNum,60,DateTime.Today);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,pay,new List<long>{pat.PatNum},true,false);
			PaymentEdit.ConstructResults constructResults=PaymentEdit.ConstructAndLinkChargeCredits(new List<long> {pat.PatNum },pat.PatNum
				,loadData.ConstructChargesData.ListPaySplits,pay,new List<Procedure> (),loadData:loadData);
			Assert.AreEqual(4,constructResults.ListAccountCharges.FindAll(x => x.AmountStart>0).Count);//Procs shouldn't show - only the 4 pay plan charges
			Assert.AreEqual(4,constructResults.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(PayPlanCharge)));
		}

		[TestMethod]
		public void PaymentEdit_Init_ChargesWithUnattachedPayPlanCreditsWithPreviousPayments() {
			//new payplan
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long prov=ProviderT.CreateProvider("ProvA");
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",135,DateTime.Today.AddMonths(-4),provNum:prov);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Today.AddMonths(-4).AddDays(1),provNum:prov);
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,30,DateTime.Today.AddMonths(-3),prov,totalAmt:195);//totalAmt since unattached credits
			//Make initial payments.
			PaymentT.MakePayment(pat.PatNum,30,DateTime.Today.AddMonths(-2),payplan.PayPlanNum,prov,0,1);
			PaymentT.MakePayment(pat.PatNum,30,DateTime.Today.AddMonths(-1),payplan.PayPlanNum,prov,0,1);
			//Go to make another payment. 2 pay plan charges should have been "removed" (amtStart to 0) from being paid. 
			Payment pay=PaymentT.MakePaymentNoSplits(pat.PatNum,30,DateTime.Today);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,pay,new List<long>{pat.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat.PatNum),new Family { },pay
					,loadData.ListSplits,new List<Procedure>(),pat.PatNum,loadData:loadData);
			//2 procs and 2 pp charges
			Assert.AreEqual(4,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.AmountStart>0).Count);
			Assert.AreEqual(2,initData.AutoSplitData.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(Procedure)));
			Assert.AreEqual(4,initData.AutoSplitData.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(PayPlanCharge)));
		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_ChargesWithAttachedPayPlanCreditsWithPreviousPayments() {
			//new payplan
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1120",ProcStat.C,"",135,DateTime.Today.AddMonths(-4));
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",60,DateTime.Today.AddMonths(-4));
			PayPlan payplan=PayPlanT.CreatePayPlanWithCredits(pat.PatNum,30,DateTime.Today.AddMonths(-3),0,new List<Procedure>() {proc1,proc2});
			//Procedures's amount start should now be 0 from being attached. Make initial payments.
			PaymentT.MakePayment(pat.PatNum,30,DateTime.Today.AddMonths(-2),payplan.PayPlanNum,procNum:proc1.ProcNum);
			PaymentT.MakePayment(pat.PatNum,30,DateTime.Today.AddMonths(-1),payplan.PayPlanNum,procNum:proc1.ProcNum);
			//2 pay plan charges should have been removed from being paid. Make a new payment. 
			Payment pay=PaymentT.MakePaymentNoSplits(pat.PatNum,30,DateTime.Today,isNew:true,payType:1);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,pay,new List<long>{pat.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat.PatNum),new Family { },pay
					,loadData.ListSplits,new List<Procedure>(),pat.PatNum,loadData:loadData);
			//should only see 2 pay plan charges that have not been paid, along with 2 pay plan charges that have been paid. 
			Assert.AreEqual(2,initData.AutoSplitData.ListAccountCharges.FindAll(x => x.AmountStart>0).Count);
			Assert.AreEqual(4,initData.AutoSplitData.ListAccountCharges.Count(x => x.Tag.GetType()==typeof(PayPlanCharge)));
		}

		[TestMethod]
		public void PaymentEdit_Init_AttachedAdjustment() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("prov1");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",135,DateTime.Today.AddMonths(-1).AddDays(1),provNum:provNum);
			Adjustment adjustment=AdjustmentT.MakeAdjustment(pat.PatNum,20,DateTime.Today.AddDays(-15),provNum:provNum,procNum:proc.ProcNum);
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,20);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payCur,new List<long>{pat.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat.PatNum),new Family { },payCur
					,loadData.ListSplits,new List<Procedure>(),pat.PatNum,loadData:loadData);
			//Verify there is only one charge (the procedure's charge + the adjustment for the amount original)
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(typeof(Procedure),initData.AutoSplitData.ListAccountCharges[0].Tag.GetType());
			Assert.AreEqual(155,initData.AutoSplitData.ListAccountCharges[0].AmountOriginal);
		}

		[TestMethod]
		public void PaymentEdit_Init_UnattachedPaymentsAndAttachedAdjustments() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			long provNum=ProviderT.CreateProvider("prov1");
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",135,DateTime.Today.AddMonths(-1).AddDays(1),provNum:provNum);
			Adjustment adjustment=AdjustmentT.MakeAdjustment(pat.PatNum,20,DateTime.Today.AddDays(-15),provNum:provNum,procNum:proc.ProcNum);
			Payment existingPayment1=PaymentT.MakePayment(pat.PatNum,35,DateTime.Today.AddDays(-1));//no prov or proc because it's unattached.
			Payment existingPayment2=PaymentT.MakePayment(pat.PatNum,20,DateTime.Today.AddDays(-1));
			Payment payCur=PaymentT.MakePaymentNoSplits(pat.PatNum,100);
			PaymentEdit.LoadData loadData=PaymentEdit.GetLoadData(pat,payCur,new List<long>{pat.PatNum},true,false);
			PaymentEdit.InitData initData=PaymentEdit.Init(loadData.ListAssociatedPatients,Patients.GetFamily(pat.PatNum),new Family { },payCur
					,loadData.ListSplits,new List<Procedure>(),pat.PatNum,loadData:loadData);
			//Verify there is only one charge (the procedure's charge + the adjustment for the amount original)
			Assert.AreEqual(1,initData.AutoSplitData.ListAccountCharges.Count);
			Assert.AreEqual(typeof(Procedure),initData.AutoSplitData.ListAccountCharges[0].Tag.GetType());
			Assert.AreEqual(0,initData.AutoSplitData.ListAccountCharges[0].AmountEnd);
		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_PayPlanWithNegAdjustmentsForPlansWithUnattachedCredits() {

		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_PayPlanWithNegAdjustmentForPlansWithAttachedCredits() {

		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_PayPlanWithAdjustedChargeAlreadyPaidUnattached() {

		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_PayPlanWithAdjustedChargeAlreadyPaidAttached() {

		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_IncomeTransfer() {

		}

		[TestMethod]
		public void PaymentEdit_ConstructAndLinkChargeCredits_PrePayments() {

		}

		[TestMethod]
		public void PaymentEdit_MakePayment_PayPlanChargesWithAdjustment() {

		}

		[TestMethod]
		public void PaymentEdit_MakePayment_Adjustment() {

		}

		[TestMethod]
		public void PaymentEdit_AutoSplitForPayments_ProceduresAndUnattachedPayments() {

		}

		[TestMethod]
		public void PaymentEdit_NOTSURE_IncomeTransferFixWhenIncomeIncorrectlyAllocatedToOneProvider() {

		}

		[TestMethod]
		public void PaymentEdit_AllocateUnearned_LinkToOriginalPrepayment() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			//create prepayment of $100
			long provNum=ProviderT.CreateProvider("SG");
			Clinic clinic1=ClinicT.CreateClinic("Clinic1");
			Family fam=Patients.GetFamily(pat.PatNum);
			//create original prepayment.
			PaySplit prePay=PaySplitT.CreatePrepayment(pat.PatNum,100,DateTime.Today.AddDays(-1),provNum,clinic1.ClinicNum);
			//complete a procedure
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",50,provNum:provNum);
			//Setup to run the PaymentEdit.AllocateUnearned
			List<PaySplit> listPaySplits=new List<PaySplit>();
			List<PaySplit> listFamPrePaySplits=PaySplits.GetPrepayForFam(fam);
			//Unearned amount should be $100.
			double unearnedAmt=(double)PaySplits.GetUnearnedForFam(fam,listFamPrePaySplits);
			Assert.AreEqual(100,unearnedAmt);
			//Create the payment we will use to allocate some of the $100 prepayment.
			Payment pay=PaymentT.MakePaymentForPrepayment(pat,clinic1);
			//Run the AllocateUnearned method. This a list of paysplitAssociated.
			//The ref list of paysplits should also have the new paysplits that are associated to the original prepayment.
			List<PaySplits.PaySplitAssociated> listPaySplitAssociated=PaymentEdit.AllocateUnearned(new List<Procedure> { proc1 },ref listPaySplits,pay,unearnedAmt,fam);
			//Insert the paysplits and link the prepayment paysplits. This is similar to what is done when a user creates a payment from FormPayment.cs.
			PaySplitT.InsertPrepaymentSplits(listPaySplits,listPaySplitAssociated);
			//The ref list of paysplits should have the correct allocated prepayment amount. 
			Assert.AreEqual(-50,listPaySplits.Where(x => x.UnearnedType!=0).Sum(x => x.SplitAmt));
			//Create new procedure
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",100,provNum:provNum);
			//Now do what we just did again for a new procedure. The unallocated amount should be $50.
			listFamPrePaySplits=PaySplits.GetPrepayForFam(fam);
			unearnedAmt=(double)PaySplits.GetUnearnedForFam(fam,listFamPrePaySplits);
			Assert.AreEqual(50,unearnedAmt);
			List<PaySplit> listPaySplitsUnearned=new List<PaySplit>();
			pay=PaymentT.MakePaymentForPrepayment(pat,clinic1);
			List<PaySplits.PaySplitAssociated> retVal=PaymentEdit.AllocateUnearned(new List<Procedure> { proc2 },ref listPaySplitsUnearned,pay,unearnedAmt,fam);
			Assert.AreEqual(2,retVal.Count);
			Assert.AreEqual(-50,listPaySplitsUnearned.Where(x => x.UnearnedType!=0).Sum(x => x.SplitAmt));
		}

		[TestMethod]
		public void PaymentEdit_AllocateUnearned_NoLinkToOriginalPrepayment() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			//create prepayment of $100
			long provNum=ProviderT.CreateProvider("SG");
			Clinic clinic1=ClinicT.CreateClinic("Clinic1");
			//create original prepayment.
			PaySplit prePay=PaySplitT.CreatePrepayment(pat.PatNum,100,DateTime.Today.AddDays(-1),provNum,clinic1.ClinicNum);
			//complete a procedure
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",50,provNum:provNum);
			//Manually allocate prepayment without linking to the original prepayment.
			//We want to do it manually so we don't link this to the orginal prepayment correctly.
			//Not linking correctly will test that the AllocateUnearned method is implicitly linking prepayments correctly.
			PaySplitT.CreatePaySplitsForPrepayment(proc1,50,prov: provNum,clinic: clinic1);
			//Create new procedure
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",100,provNum:provNum);
			//test the PaymentEdit.AllocateUnearned() method.
			Family fam=Patients.GetFamily(pat.PatNum);
			List<PaySplit> listFamPrePaySplits=PaySplits.GetPrepayForFam(fam);
			//Should be $100
			double unearnedAmt=(double)PaySplits.GetUnearnedForFam(fam,listFamPrePaySplits);
			Assert.AreEqual(100,unearnedAmt);
			List<PaySplit> listPaySplitsUnearned=new List<PaySplit>();
			Payment pay=PaymentT.MakePaymentForPrepayment(pat,clinic1);
			List<PaySplits.PaySplitAssociated> retVal=PaymentEdit.AllocateUnearned(new List<Procedure> { proc2 },ref listPaySplitsUnearned,pay,unearnedAmt,fam);
			Assert.AreEqual(2,retVal.Count);
			//After running the AllocateUnearned, we should implicitly link the incorrect prepayment made when we call CreatePaySplitsForPrepayment above.
			Assert.AreEqual(-50,listPaySplitsUnearned.Where(x => x.UnearnedType!=0).Sum(x => x.SplitAmt));
		}

		[TestMethod]
		public void PaymentEdit_ImplicitlyLinkPrepaymentsHelper() {
			Patient pat=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Patient patFam=PatientT.CreatePatient(MethodBase.GetCurrentMethod().Name);
			Patient patFamOld=patFam.Copy();
			patFam.Guarantor=pat.PatNum;
			Patients.Update(patFam,patFamOld);
			//create prepayment of $100
			long provNum=ProviderT.CreateProvider("SG");
			Clinic clinic1=ClinicT.CreateClinic("Clinic1");
			//create original prepayment.
			PaySplit prePay=PaySplitT.CreatePrepayment(pat.PatNum,100,DateTime.Today.AddDays(-1),provNum,clinic1.ClinicNum);
			//complete a procedure
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",50,provNum:provNum);
			//Manually allocate prepayment without linking to the original prepayment.
			List<PaySplit> listPaySplits=PaySplitT.CreatePaySplitsForPrepayment(proc1,50,prov:provNum,clinic:clinic1);
			ResetPrepayments(pat);
			long nonMatch=100000;
			//test the PaymentEdit.AllocateUnearned() method.
			double unearnedAmt=(double)PaySplits.GetUnearnedForFam(Patients.GetFamily(pat.PatNum),_listFamPrePaySplits);
			//Logic check PatNum - match, ProvNum - match, ClinicNum - match
			double retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumMatch:true,isClinicNumMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - match, ClinicNum - zero
			ResetPrepayments(pat);
			//update the clinicnum to 0
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,provNum,0,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumMatch:true,isClinicNumZero:true);
			Assert.AreEqual(50,retVal);
			//previous Test one should be $100
			ResetPrepayments(pat);
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumMatch:true,isClinicNumMatch:true);
			Assert.AreEqual(100,retVal);
			//Logic check PatNum - match, ProvNum - match, ClinicNum - non zero & non match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,provNum,100000,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch: true,isProvNumMatch: true,isClinicNonZeroNonMatch: true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - zero, ClinicNum - match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,0,clinic1.ClinicNum,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch: true,isProvNumZero:true,isClinicNumMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - zero, ClinicNum - zero
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,0,0,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumZero:true,isClinicNumZero:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - zero, ClinicNum - non zero & non match 
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,0,nonMatch,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumZero:true,isClinicNonZeroNonMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - non zero & non match, ClinicNum - match 
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,nonMatch,clinic1.ClinicNum,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNonZeroNonMatch:true,isClinicNumMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - non zero & non match, ClinicNum - zero
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,nonMatch,0,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNonZeroNonMatch:true,isClinicNumZero:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - match, ProvNum - non zero & non match, ClinicNum - non zero & non match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,pat.PatNum,nonMatch,nonMatch,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNonZeroNonMatch:true,isClinicNonZeroNonMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - match, ClinicNum - match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,provNum,clinic1.ClinicNum,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumMatch:true,isClinicNumMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - match, ClinicNum - zero
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,provNum,0,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumMatch:true,isClinicNumZero:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - match, ClinicNum - non zero & non match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,provNum,nonMatch,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumMatch:true,isClinicNonZeroNonMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - zero, ClinicNum - match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,0,clinic1.ClinicNum,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumZero:true,isClinicNumMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - zero, ClinicNum - zero
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,0,0,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumZero:true,isClinicNumZero:true);
			Assert.AreEqual(50,retVal);
			//Logic checkPatNum - other family members, ProvNum - zero, ClinicNum - non zero & non match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,0,nonMatch,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNumZero:true,isClinicNonZeroNonMatch:true);
			Assert.AreEqual(50,retVal);
			//Old test from above
			ResetPrepayments(pat);
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isPatMatch:true,isProvNumMatch:true,isClinicNumMatch:true);
			Assert.AreEqual(100,retVal);
			//Logic checkPatNum - other family members, ProvNum - non zero & non match, ClinicNum - match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,nonMatch,clinic1.ClinicNum,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNonZeroNonMatch:true,isClinicNumMatch:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - non zero & non match, ClinicNum - zero
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,nonMatch,0,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNonZeroNonMatch:true,isClinicNumZero:true);
			Assert.AreEqual(50,retVal);
			//Logic check PatNum - other family members, ProvNum - non zero & non match, ClinicNum - non zero & non match
			ResetPrepayments(pat);
			_listFamPrePaySplits.ForEach(x => UpdatePaySplitHelper(x,patFam.PatNum,nonMatch,nonMatch,_listNegPrePay.First().PayNum));
			retVal=PaymentEdit.ImplicitlyLinkPrepaymentsHelper(_listPosPrePay,_listNegPrePay,unearnedAmt,isFamMatch:true,isProvNonZeroNonMatch:true,isClinicNonZeroNonMatch:true);
			Assert.AreEqual(50,retVal);
		}

		private void UpdatePaySplitHelper(PaySplit ps,long patNum,long provNum,long clinicNum,long payNum) {
			if(payNum!=0 && payNum==ps.PayNum) {
				ps.PatNum=patNum;
				ps.ProvNum=provNum;
				ps.ClinicNum=clinicNum;
				PaySplits.Update(ps);
			}
		}

		private void ResetPrepayments(Patient pat) {
			Family fam=Patients.GetFamily(pat.PatNum);
			_listFamPrePaySplits=PaySplits.GetPrepayForFam(fam);
			_listPosPrePay=_listFamPrePaySplits.FindAll(x => x.SplitAmt.IsGreaterThan(0));
			_listNegPrePay=_listFamPrePaySplits.FindAll(x => x.SplitAmt.IsLessThan(0));
		}
	}
}
