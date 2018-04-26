using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenDentBusiness;
using UnitTestsCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.UnitTests {
	[TestClass]
	public class SchedulesTest:TestBase {

		///<summary>A provider schedule from 8-4 and 1-5 results in a schedule of 8-5. </summary>
		[TestMethod]
		public void Schedules_ProvSchedsForProductionGoal8To5() {
			Schedule sched8to4=new Schedule() {
				ProvNum=1,
				ScheduleNum=1,
				SchedDate=new DateTime(2018,3,12),
				StartTime=new DateTime(2018,3,9,8,0,0).TimeOfDay,//8am
				StopTime=new DateTime(2018,3,9,16,0,0).TimeOfDay//4pm
			};
			Schedule sched1to5=new Schedule() {
				ProvNum=1,
				ScheduleNum=5,
				SchedDate=new DateTime(2018,3,12),
				StartTime=new DateTime(2018,3,9,13,0,0).TimeOfDay,//1pm
				StopTime=new DateTime(2018,3,9,17,0,0).TimeOfDay//5pm
			};
			Schedule sched8to5=new Schedule() {
				ProvNum=1,
				ScheduleNum=8,
				SchedDate=new DateTime(2018,3,12),
				StartTime=new DateTime(2018,3,9,8,0,0).TimeOfDay,//8am
				StopTime=new DateTime(2018,3,9,17,0,0).TimeOfDay//5pm
			};
			List<Schedule> listSchedules = new List<Schedule>() { sched8to4,sched1to5 };
			List<Schedule> listScheduleProvSched = new List<Schedule>() { sched8to5 };
			List<Schedule> listRetVal=Schedules.GetProvSchedsForProductionGoals(new Dictionary<long, List<Schedule>>() { { 1,listSchedules } });
			Assert.AreEqual(1,listScheduleProvSched.Count);
			Assert.AreEqual(listRetVal[0].StartTime,listScheduleProvSched[0].StartTime);
			Assert.AreEqual(listRetVal[0].StopTime,listScheduleProvSched[0].StopTime);
		}

		///<summary>A provider schedule from 8-12 and 1-5 results in a schedule of 8-12 and 1-5. </summary>
		[TestMethod]
		public void Schedules_ProvSchedsForProductionGoal8To12And1To5() {
			Schedule sched8to12=new Schedule() {
				ProvNum=1,
				ScheduleNum=2,
				SchedDate=new DateTime(2018,3,12),
				StartTime=new DateTime(2018,3,9,8,0,0).TimeOfDay,//8am
				StopTime=new DateTime(2018,3,9,12,0,0).TimeOfDay//12pm
			};
			Schedule sched1to5=new Schedule() {
				ProvNum=1,
				ScheduleNum=5,
				SchedDate=new DateTime(2018,3,12),
				StartTime=new DateTime(2018,3,9,13,0,0).TimeOfDay,//1pm
				StopTime=new DateTime(2018,3,9,17,0,0).TimeOfDay//5pm
			};
			List<Schedule> listSchedules = new List<Schedule>() { sched8to12,sched1to5 };
			List<Schedule> listScheduleProvSched = new List<Schedule>() { sched8to12,sched1to5 };
			List<Schedule> listRetVal=Schedules.GetProvSchedsForProductionGoals(new Dictionary<long, List<Schedule>>() { { 1,listSchedules } });
			Assert.AreEqual(2,listScheduleProvSched.Count);
			Assert.AreEqual(listScheduleProvSched[0].StartTime,listRetVal[0].StartTime);
			Assert.AreEqual(listScheduleProvSched[0].StopTime,listRetVal[0].StopTime);
			Assert.AreEqual(listScheduleProvSched[1].StartTime,listRetVal[1].StartTime);
			Assert.AreEqual(listScheduleProvSched[1].StopTime,listRetVal[1].StopTime);
		}

		///<summary>A provider schedule from 8-12,9-11,and 1-6 results in a schedule of 8-12 and 1-6. </summary>
		[TestMethod]
		public void Schedules_ProvSchedsForProductionGoal8To12And1To6() {
			Schedule sched8to12=new Schedule() {
				ProvNum=1,
				ScheduleNum=2,
				SchedDate=new DateTime(2018,3,12),
				StartTime=new DateTime(2018,3,9,8,0,0).TimeOfDay,//8am
				StopTime=new DateTime(2018,3,9,12,0,0).TimeOfDay//12pm
			};
			Schedule sched9to11=new Schedule() {
				ProvNum=1,
				ScheduleNum=3,
				SchedDate=new DateTime(2018,3,12),
				StartTime=new DateTime(2018,3,9,9,0,0).TimeOfDay,//9pm
				StopTime=new DateTime(2018,3,9,11,0,0).TimeOfDay//11am
			};
			Schedule sched1to6=new Schedule() {
				ProvNum=1,
				ScheduleNum=5,
				SchedDate=new DateTime(2018,3,12),
				StartTime=new DateTime(2018,3,9,13,0,0).TimeOfDay,//1pm
				StopTime=new DateTime(2018,3,9,18,0,0).TimeOfDay//6pm
			};
			List<Schedule> listSchedules = new List<Schedule>() { sched8to12,sched9to11,sched1to6 };
			List<Schedule> listScheduleProvSched = new List<Schedule>() { sched8to12,sched1to6 };
			List<Schedule> listRetVal=Schedules.GetProvSchedsForProductionGoals(new Dictionary<long, List<Schedule>>() { { 1,listSchedules } });
			Assert.AreEqual(2,listScheduleProvSched.Count);
			Assert.AreEqual(listScheduleProvSched[0].StartTime,listRetVal[0].StartTime);
			Assert.AreEqual(listScheduleProvSched[0].StopTime,listRetVal[0].StopTime);
			Assert.AreEqual(listScheduleProvSched[1].StartTime,listRetVal[1].StartTime);
			Assert.AreEqual(listScheduleProvSched[1].StopTime,listRetVal[1].StopTime);
		}

		///<summary>A provider schedule from 8-12, 9-3, and 4-5 results in a schedule of 8-3 and 4-5. </summary>
		[TestMethod]
		public void Schedules_ProvSchedsForProductionGoal8To3And4To5() {
			Schedule sched8to12=new Schedule() {
				ProvNum=1,
				ScheduleNum=2,
				SchedDate=new DateTime(2018,3,12),
				StartTime=new DateTime(2018,3,9,8,0,0).TimeOfDay,//8am
				StopTime=new DateTime(2018,3,9,12,0,0).TimeOfDay//12pm
			};
			Schedule sched9to3=new Schedule() {
				ProvNum=1,
				ScheduleNum=4,
				SchedDate=new DateTime(2018,3,12),
				StartTime=new DateTime(2018,3,9,9,0,0).TimeOfDay,//9pm
				StopTime=new DateTime(2018,3,9,15,0,0).TimeOfDay//3pm
			};

			Schedule sched4to5=new Schedule() {
				ProvNum=1,
				ScheduleNum=6,
				SchedDate=new DateTime(2018,3,12),
				StartTime=new DateTime(2018,3,9,16,0,0).TimeOfDay,//4pm
				StopTime=new DateTime(2018,3,9,17,0,0).TimeOfDay//5pm
			};
			Schedule sched8to3=new Schedule() {
				ProvNum=1,
				ScheduleNum=7,
				SchedDate=new DateTime(2018,3,12),
				StartTime=new DateTime(2018,3,9,8,0,0).TimeOfDay,//8am
				StopTime=new DateTime(2018,3,9,15,0,0).TimeOfDay//3pm
			};
			List<Schedule> listSchedules = new List<Schedule>() { sched8to12,sched9to3,sched4to5 };
			List<Schedule> listScheduleProvSched = new List<Schedule>() { sched8to3,sched4to5 };
			List<Schedule> listRetVal=Schedules.GetProvSchedsForProductionGoals(new Dictionary<long, List<Schedule>>() { { 1,listSchedules } });
			Assert.AreEqual(2,listScheduleProvSched.Count);
			Assert.AreEqual(listScheduleProvSched[0].StartTime,listRetVal[0].StartTime);
			Assert.AreEqual(listScheduleProvSched[0].StopTime,listRetVal[0].StopTime);
			Assert.AreEqual( listScheduleProvSched[1].StartTime,listRetVal[1].StartTime);
			Assert.AreEqual(listScheduleProvSched[1].StopTime,listRetVal[1].StopTime);
		}

		///<summary>A provider schedule from 8-11, 1-3, and 4-7 results in a schedule of 8-3 and 4-5. </summary>
		[TestMethod]
		public void Schedules_ProvSchedsForProductionGoal8To11And1To3And4To7() {
			Schedule sched8to11=new Schedule() {
				ProvNum=1,
				ScheduleNum=2,
				SchedDate=new DateTime(2018,3,12),
				StartTime=new DateTime(2018,3,9,8,0,0).TimeOfDay,//8am
				StopTime=new DateTime(2018,3,9,11,0,0).TimeOfDay//11pm
			};
			Schedule sched1to3=new Schedule() {
				ProvNum=1,
				ScheduleNum=4,
				SchedDate=new DateTime(2018,3,12),
				StartTime=new DateTime(2018,3,9,13,0,0).TimeOfDay,//1pm
				StopTime=new DateTime(2018,3,9,15,0,0).TimeOfDay//3pm
			};

			Schedule sched4to7=new Schedule() {
				ProvNum=1,
				ScheduleNum=6,
				SchedDate=new DateTime(2018,3,12),
				StartTime=new DateTime(2018,3,9,16,0,0).TimeOfDay,//4pm
				StopTime=new DateTime(2018,3,9,19,0,0).TimeOfDay//7pm
			};
			Schedule sched8to3=new Schedule() {
				ProvNum=1,
				ScheduleNum=7,
				SchedDate=new DateTime(2018,3,12),
				StartTime=new DateTime(2018,3,9,8,0,0).TimeOfDay,//8am
				StopTime=new DateTime(2018,3,9,15,0,0).TimeOfDay//3pm
			};
			List<Schedule> listSchedules = new List<Schedule>() { sched8to11,sched1to3,sched4to7 };
			List<Schedule> listScheduleProvSched = new List<Schedule>() { sched8to11,sched1to3,sched4to7  };
			List<Schedule> listRetVal=Schedules.GetProvSchedsForProductionGoals(new Dictionary<long, List<Schedule>>() { { 1,listSchedules } });
			Assert.AreEqual(3,listScheduleProvSched.Count);
			Assert.AreEqual(listScheduleProvSched[0].StartTime,listRetVal[0].StartTime);
			Assert.AreEqual(listScheduleProvSched[0].StopTime,listRetVal[0].StopTime);
			Assert.AreEqual(listScheduleProvSched[1].StartTime,listRetVal[1].StartTime);
			Assert.AreEqual(listScheduleProvSched[1].StopTime,listRetVal[1].StopTime);
			Assert.AreEqual(listScheduleProvSched[2].StartTime,listRetVal[2].StartTime);
			Assert.AreEqual(listScheduleProvSched[2].StopTime,listRetVal[2].StopTime);
		}
	}
}
