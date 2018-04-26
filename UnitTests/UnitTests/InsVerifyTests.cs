using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests {
	[TestClass]
	public class InsVerifyTests:TestBase {

		[TestMethod]
		public void InsVerifies_VerifyDateTest() {
			List<List<int>> testCases=new List<List<int>>();
			#region testCase setup
			for(int i=0;i<14;i++) {
				testCases.Add(null);
				switch(i) {
					case 0:
						testCases[i]=new List<int> {
							(int)eventType.DateLastVerified,
							(int)eventType.DateTimeApptWasScheduled,
							(int)eventType.ApptSchedDaysMaxDaysForVerification,
							(int)eventType.NextScheduledVerifyDate,
							(int)eventType.DateTimeVerified,
							(int)eventType.AptDateTime
						};
						break;
					case 1:
						testCases[i]=new List<int> {
							(int)eventType.DateLastVerified,
							(int)eventType.NextScheduledVerifyDate,
							(int)eventType.ApptSchedDaysMaxDaysForVerification,
							(int)eventType.DateTimeApptWasScheduled,
							(int)eventType.DateTimeVerified,
							(int)eventType.AptDateTime
						};
						break;
					case 2:
						testCases[i]=new List<int> {
							(int)eventType.DateLastVerified,
							(int)eventType.NextScheduledVerifyDate,
							(int)eventType.DateTimeApptWasScheduled,
							(int)eventType.ApptSchedDaysMaxDaysForVerification,
							(int)eventType.DateTimeVerified,
							(int)eventType.AptDateTime
						};
						break;
					case 3:
						testCases[i]=new List<int> {
							(int)eventType.DateLastVerified,
							(int)eventType.DateTimeApptWasScheduled,
							(int)eventType.NextScheduledVerifyDate,
							(int)eventType.ApptSchedDaysMaxDaysForVerification,
							(int)eventType.DateTimeVerified,
							(int)eventType.AptDateTime
						};
						break;
					case 4:
						testCases[i]=new List<int> {
							(int)eventType.DateLastVerified,
							(int)eventType.ApptSchedDaysMaxDaysForVerification,
							(int)eventType.DateTimeApptWasScheduled,
							(int)eventType.NextScheduledVerifyDate,
							(int)eventType.DateTimeVerified,
							(int)eventType.AptDateTime
						};
						break;
					case 5:
						testCases[i]=new List<int> {
							(int)eventType.DateLastVerified,
							(int)eventType.ApptSchedDaysMaxDaysForVerification,
							(int)eventType.NextScheduledVerifyDate,
							(int)eventType.DateTimeApptWasScheduled,
							(int)eventType.DateTimeVerified,
							(int)eventType.AptDateTime
						};
						break;
					case 6:
						testCases[i]=new List<int> {
							(int)eventType.DateLastVerified,
							(int)eventType.DateTimeInsuranceRenewalNeeded,
							(int)eventType.DateTimeApptWasScheduled,
							(int)eventType.ApptSchedDaysMaxDaysForVerification,
							(int)eventType.NextScheduledVerifyDate,
							(int)eventType.DateTimeVerified,
							(int)eventType.AptDateTime
						};
						break;
					case 7:
						testCases[i]=new List<int> {
							(int)eventType.DateLastVerified,
							(int)eventType.NextScheduledVerifyDate,
							(int)eventType.ApptSchedDaysMaxDaysForVerification,
							(int)eventType.DateTimeApptWasScheduled,
							(int)eventType.DateTimeInsuranceRenewalNeeded,
							(int)eventType.DateTimeVerified,
							(int)eventType.AptDateTime
						};
						break;
					case 8:
						testCases[i]=new List<int> {
							(int)eventType.DateLastVerified,
							(int)eventType.NextScheduledVerifyDate,
							(int)eventType.DateTimeApptWasScheduled,
							(int)eventType.ApptSchedDaysMaxDaysForVerification,
							(int)eventType.DateTimeInsuranceRenewalNeeded,
							(int)eventType.DateTimeVerified,
							(int)eventType.AptDateTime
						};
						break;
					case 9:
						testCases[i]=new List<int> {
							(int)eventType.DateLastVerified,
							(int)eventType.DateTimeApptWasScheduled,
							(int)eventType.NextScheduledVerifyDate,
							(int)eventType.ApptSchedDaysMaxDaysForVerification,
							(int)eventType.DateTimeInsuranceRenewalNeeded,
							(int)eventType.DateTimeVerified,
							(int)eventType.AptDateTime
						};
						break;
					case 10:
						testCases[i]=new List<int> {
							(int)eventType.DateLastVerified,
							(int)eventType.DateTimeInsuranceRenewalNeeded,
							(int)eventType.ApptSchedDaysMaxDaysForVerification,
							(int)eventType.DateTimeApptWasScheduled,
							(int)eventType.NextScheduledVerifyDate,
							(int)eventType.DateTimeVerified,
							(int)eventType.AptDateTime
						};
						break;
					case 11:
						testCases[i]=new List<int> {
							(int)eventType.DateLastVerified,
							(int)eventType.DateTimeInsuranceRenewalNeeded,
							(int)eventType.ApptSchedDaysMaxDaysForVerification,
							(int)eventType.NextScheduledVerifyDate,
							(int)eventType.DateTimeApptWasScheduled,
							(int)eventType.DateTimeVerified,
							(int)eventType.AptDateTime
						};
						break;
					case 12:
						testCases[i]=new List<int> { 
							(int)eventType.DateLastVerified,
							(int)eventType.DateTimeApptWasScheduled,
							(int)eventType.ApptSchedDaysMaxDaysForVerification,
							(int)eventType.DateTimeInsuranceRenewalNeeded,
							(int)eventType.DateTimeVerified,
							(int)eventType.AptDateTime,
							(int)eventType.NextScheduledVerifyDate
						};
						break;
					case 13:
						testCases[i]=new List<int> { 
							(int)eventType.DateLastVerified,
							(int)eventType.ApptSchedDaysMaxDaysForVerification,
							(int)eventType.DateTimeInsuranceRenewalNeeded,
							(int)eventType.DateTimeApptWasScheduled,
							(int)eventType.DateTimeVerified,
							(int)eventType.AptDateTime,
							(int)eventType.NextScheduledVerifyDate
						};
						break;
					default:
						break;
				}
			}
			#endregion
			//This is a list of the possible dates times.
			//For each of the test cases the first items in the case list gets the first date, second gets the second etc...
			List<DateTime> dates=new List<DateTime> {
				new DateTime(2018,2,23,0,0,0),	//2-23-2018    0:00
				new DateTime(2018,2,23,1,0,0),	//2-23-2018		1:00
				new DateTime(2018,2,23,2,0,0),	//"       "		2:00
				new DateTime(2018,2,23,3,0,0),	//etc etc....
				new DateTime(2018,2,23,4,0,0),
				new DateTime(2018,2,23,5,0,0),
				new DateTime(2018,2,23,6,0,0)
			};
			//List of "Hours" we know are right. The hour corresponds to the index of the test case list that is correct.
			List<int> expectedResults=new List<int> {3,3,3,3,3,3,3,3,3,3,3,4,3,3};	//expectedResults[i] == results of passing test of testCases[i]
			//Loop through and verify test cases
			Console.WriteLine("See [[Insurance Verification - Hours Available for Verification]] for the visual diagram corresponding to the test number.");
			for(int i=0;i<testCases.Count;i++) { 
				List<int> items=testCases[i];
				DateTime daysForVerification=DateTime.MaxValue;	//Max value so they're out of the way
				DateTime appointmentDate=DateTime.MaxValue;
				DateTime needsVerifyDate=DateTime.MaxValue;
				DateTime lastVerifyDate=DateTime.MaxValue;
				DateTime benifitRenewalDate=DateTime.MaxValue;
				#region TimeCreation
				int currentDate=0;
				foreach(int index in items) {
					switch(index) {
						case (int)eventType.DateLastVerified:
							lastVerifyDate=dates[currentDate];
							currentDate++;
							break;
						case (int)eventType.NextScheduledVerifyDate:
							needsVerifyDate=dates[currentDate];
							currentDate++;
							break;
						case (int)eventType.DateTimeApptWasScheduled:
							appointmentDate=dates[currentDate];
							currentDate++;
							break;
						case (int)eventType.ApptSchedDaysMaxDaysForVerification:
							daysForVerification=dates[currentDate];
							currentDate++;
							break;
						case (int)eventType.DateTimeInsuranceRenewalNeeded:
							benifitRenewalDate=dates[currentDate];
							currentDate++;
							break;
						//These two aren't used in the calculation, so we just increment the datetime index
						case (int)eventType.AptDateTime:
							currentDate++;
							break;
						case (int)eventType.DateTimeVerified:
							currentDate++;
							break;
					}
				}
				#endregion
				DateTime resultDate=InsVerifies.VerifyDateCalulation(daysForVerification,appointmentDate,needsVerifyDate,benifitRenewalDate);
				Console.WriteLine((expectedResults[i]==resultDate.Hour ? "PASSED: " : "FAILED: ")+"Test "+(i+1));
				Assert.AreEqual(expectedResults[i],resultDate.Hour);//If the times are equal, then the result is correct.
			}
		}

		private enum eventType {
			DateLastVerified,
			NextScheduledVerifyDate,
			DateTimeApptWasScheduled,
			AptDateTime,
			ApptSchedDaysMaxDaysForVerification,
			DateTimeInsuranceRenewalNeeded,
			DateTimeVerified
		}
	}
}
