using System;
using CodeBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using System.Collections.Generic;
using UnitTestsCore;
using System.Reflection;

namespace UnitTests {
	[TestClass]
	public abstract class TestBase {

		protected const string UnitTestUserName="UnitTest";
		protected const string UnitTestPassword="password";
		public const string TestEmaiAddress="opendentalunittests@gmail.com";
		public const string TestEmaiPwd="welovetesting!";
		///<summary>This is the OD test department's Google Voice number. See [[eService Regression Test Database Setup]]. 
		///Linked to email eServices.od.test@gmail.com.</summary>
		public const string TestPatPhone="1(971)301-2247";
		public TestContext TestContext { get; set; }
		protected Logger.WriteLineDelegate _log;
		///<summary>Put this in the watch window to see the logger results.</summary>
		protected string _logStr="";

		public static string UnitTestDbName {
			get {
				Version versionBusiness=Assembly.GetAssembly(typeof(PatientStatus)).GetName().Version;
				return "unittest"+versionBusiness.Major+versionBusiness.Minor;
			}
		}

		public TestBase() {
			_log=new Logger.WriteLineDelegate((log,ll) => {
				_logStr+=log+"\r\n";
				//To view the output, in the Test Explorer in the bottom half, click 'Output'.
				Console.WriteLine(log);
			});
		}

		///<summary>Write to _logStr and the test output window.</summary>
		protected void LogWriteLine(string log) {
			_log(log,LogLevel.Verbose);
		}

		/// <summary> TestBase.Initialize will be called before the ClassInitialize and TestInitialize methods specific to each class.
		/// Do this first so that the time the Initialize and ClassInitialize methods take doesn't get counted in the test times. </summary>
		[AssemblyInitialize]
		public static void Initialize(TestContext context) {
			if(!UnitTestsCore.DatabaseTools.SetDbConnection(UnitTestDbName,"localhost","3306","root","",false)) {//Put this in a config file in the future.
				UnitTestsCore.DatabaseTools.SetDbConnection("","localhost","3306","root","",false);
				DatabaseTools.FreshFromDump("localhost","3306","root","",false);//this also sets database to be unittest.
			}
			else {
				//Clear the database before running the unittests (instead of after) for two reasons
				//1- if the cleanup is done using [TestCleanup], the cleanup will not be run if the user cancels in the middle of a test while debugging
				//2- if a test fails, we may want to look at the data in the db to see why it failed.
				UnitTestsCore.DatabaseTools.ClearDb();
			}
#if !DEBUG
			throw new Exception("You're running tests in release. BAD!!!");
#endif
			CreateUnitTestUser();
			//Get the Admin user, should always exist
			Security.CurUser=Userods.GetUserByName(UnitTestUserName,false);
			Security.PasswordTyped=UnitTestPassword;//For middle tier unit tests.
			//Uncomment the next line in order to run every single unit test method like it is using the middle tier.
			//RunTestsAgainstMiddleTier();
		}

		[TestInitialize]
		public void ResetTest() {
			PrefT.RevertPrefChanges();
		}

		private static void RunTestsAgainstMiddleTier() {
			RemotingClient.RemotingRole=RemotingRole.ClientWeb;
			OpenDentBusiness.WebServices.OpenDentalServerProxy.MockOpenDentalServerCur=new OpenDentBusiness.WebServices.OpenDentalServerMockIIS();
		}

		public static EmailAddress InsertEmailAddress() {
			EmailAddress email=new EmailAddress() {
				SenderAddress=TestEmaiAddress,
				EmailUsername=TestEmaiAddress,
				EmailPassword=MiscUtils.Encrypt(TestEmaiPwd),
				ServerPort=587,
				UseSSL=true,
				Pop3ServerIncoming="pop.gmail.com",
				ServerPortIncoming=110,
				SMTPserver="smtp.gmail.com",
				UserNum=0,
				WebmailProvNum=0,
			};
			EmailAddresses.Insert(email);
			return email;
		}

		public static void CreateUnitTestUser() {
			if(Userods.GetUserByName(UnitTestUserName,false)==null) {
				Userod newUser=new Userod()
				{
					UserName=UnitTestUserName,
					Password=Userods.HashPassword(UnitTestPassword),
				};
				try {
					Userods.Insert(newUser,new List<long> { 1 });
					Userods.RefreshCache();
				}
				catch(Exception e) {
					throw new Exception("Unable to create the default Unit Test user.",e);
				}
			}
		}
	}
}
