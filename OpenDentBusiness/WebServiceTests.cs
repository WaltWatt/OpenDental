using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using CodeBase;

namespace OpenDentBusiness {
	///<summary>These are only run from the Unit Testing framework</summary>
	public class WebServiceTests {
		private static string _dirtyString;
		private static string _newLineString;
		private static DateTime _dateTEntryTest;
		private static DateTime _dateTodayTest;

		public static string DirtyString {
			get {
				if(_dirtyString==null) {
					StringBuilder sb=new StringBuilder();
					for(int i = 0;i<0x10000;i++) {
						sb.Append((char)i);
					}
					_dirtyString=sb.ToString();
				}
				return _dirtyString;
			}
		}

		public static string NewLineString {
			get {
				if(_newLineString==null) {
					_newLineString="Line1\rLine2\nLine3\r\nLine4";
				}
				return _newLineString;
			}
		}

		public static DateTime DateTEntryTest {
			get {
				if(_dateTEntryTest==null) {
					_dateTEntryTest=new DateTime(DateTime.Today.Year,DateTime.Today.Month,DateTime.Today.Day,1,1,1);
				}
				return _dateTEntryTest;
			}
		}

		public static DateTime DateTodayTest {
			get {
				if(_dateTodayTest==null) {
					_dateTodayTest=DateTime.Today;
				}
				return _dateTodayTest;
			}
		}

		public static string GetString(string str) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),str);
			}
			return "Processed: "+str;
		}

		public static string GetStringNull() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			return null;
		}

		public static string GetStringCarriageReturn(string str) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),str);
			}
			return "Processed: "+str;
		}

		public static int GetInt(int intVal) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),intVal);
			}
			return 2;
		}

		public static long GetLong(long longVal) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),longVal);
			}
			return 2;
		}

		public static void GetVoid() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			return;
		}

		public static bool GetBool() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod());
			}
			return true;
		}

		public static Patient GetObjectPat() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Patient>(MethodBase.GetCurrentMethod());
			}
			return new Patient { LName="Smith",FName=null,AddrNote=DirtyString };
		}

		public static List<Patient> GetListPats() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod());
			}
			return new List<Patient>() {
				new Patient() { LName="Smith",FName=null,AddrNote=DirtyString },
			};
		}

		public static DataTable GetTable() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod());
			}
			string command="SELECT 'cell00' AS Col1";
			return Db.GetTable(command);
		}

		public static DataTable GetTableCarriageReturn() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod());
			}
			string command="SELECT '"+POut.String(NewLineString)+"'";
			return Db.GetTable(command);
		}

		public static DataTable GetTable2by3() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod());
			}
			string command="SELECT '"+POut.String("cell00")+"','"+POut.String("cell01")+"' "
				+"UNION ALL "
				+"SELECT '"+POut.String("cell10")+"','"+POut.String("cell11")+"' "
				+"UNION ALL "
				+"SELECT '"+POut.String("cell20")+"','"+POut.String("cell21")+"'";
			return Db.GetTable(command);
		}

		//also table with special chars: |, <, >, &, ', ", and \
		public static DataTable GetTableSpecialChars() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod());
			}
			//Special characters in the columns as well as in the column names
			string command="SELECT '"+POut.String("cell00|")+"' AS '|<>','"+POut.String("cell01<")+"' AS '&\\'\"\\\\' "
				+"UNION ALL "
				+"SELECT '"+POut.String("cell10>")+"','"+POut.String("cell11&")+"' "
				+"UNION ALL "
				+"SELECT '"+POut.String("cell20\'")+"','"+POut.String("cell21\"")+"' "
				+"UNION ALL "
				+"SELECT '"+POut.String("cell30\\")+"','"+POut.String("cell31/")+"'";
			DataTable table=Db.GetTable(command);
			table.TableName="Table|<>&'\"\\";
			table.Columns.Add("DirtyString");
			table.Rows[0]["DirtyString"]=DirtyString;
			return table;
		}

		public static DataTable GetTableDataTypes() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod());
			}
			Random rnd=new Random();
			string rndStr=rnd.Next(1000000).ToString();
			string command="DROP TABLE IF EXISTS tempdt"+rndStr+@";"
				+"CREATE TABLE tempdt"+rndStr+@" (TString VARCHAR(50),TDecimal DECIMAL(10,2),TDateTime DATETIME);"
				+"INSERT INTO tempdt"+rndStr+@" (TString,TDecimal,TDateTime) VALUES ('string',123.45,DATE('2013-04-11'));";
			Db.NonQ(command);
			command="SELECT * FROM tempdt"+rndStr+@";";
			DataTable table=Db.GetTable(command);
			command="DROP TABLE IF EXISTS tempdt"+rndStr+@";";
			Db.NonQ(command);
			return table;
		}

		public static DataSet GetDataSet() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDS(MethodBase.GetCurrentMethod());
			}
			string command="SELECT 'cell00' AS Col1";
			DataSet ds=new DataSet();
			DataTable table=Db.GetTable(command);
			table.TableName="table0";
			table.Columns.Add("DirtyString");
			table.Rows[0]["DirtyString"]=DirtyString;
			ds.Tables.Add(table);
			return ds;
		}

		public static List<int> GetListInt() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<int>>(MethodBase.GetCurrentMethod());
			}
			return new List<int> { 2 };
		}

		public static List<string> GetListString() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<string>>(MethodBase.GetCurrentMethod());
			}
			return new List<string> { "Clean" };
		}

		public static List<string> GetListStringDirty() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<string>>(MethodBase.GetCurrentMethod());
			}
			return new List<string> { DirtyString };
		}

		public static string[] GetArrayString() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<string[]>(MethodBase.GetCurrentMethod());
			}
			return new string[] { "Clean" };
		}

		public static string[] GetArrayStringDirty() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<string[]>(MethodBase.GetCurrentMethod());
			}
			return new string[] { DirtyString };
		}

		public static Patient[] GetArrayPatient() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Patient[]>(MethodBase.GetCurrentMethod());
			}
			Patient[] retVal=new Patient[2];
			retVal[0]=new Patient { LName="Jones",AddrNote=DirtyString };
			retVal[1]=null;
			return retVal;
		}

		public static Patient[] SendArrayPatient(Patient[] arrayPatients) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Patient[]>(MethodBase.GetCurrentMethod(),arrayPatients);
			}
			return arrayPatients;
		}

		public static string SendNullParam(string str) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),str);
			}
			return str;
		}

		public static Logger.IWriteLine GetInterface(Logger.IWriteLine log) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Logger.IWriteLine>(MethodBase.GetCurrentMethod(),log);
			}
			return log;
		}

		public static List<long> SendInterfaceParamWithArgs(bool argBool,Logger.IWriteLine argLogger=null,List<long> listArgLongs=null) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),argBool,argLogger,listArgLongs);
			}
			if(!argBool) {
				throw new ApplicationException("argBool must be true.");
			}
			return listArgLongs;
		}

		public static Patient GetObjectNull() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Patient>(MethodBase.GetCurrentMethod());
			}
			return null;
		}

		public static int[] SendIntParams(params int[] arrayInts) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<int[]>(MethodBase.GetCurrentMethod(),arrayInts);
			}
			return arrayInts;
		}

		public static string[] SendStringParams(params string[] arrayStrings) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<string[]>(MethodBase.GetCurrentMethod(),arrayStrings);
			}
			return arrayStrings;
		}

		public static InvalidType[] SendEnumParamsWithArgs(bool argBool,string argString,params InvalidType[] arrayITypes) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<InvalidType[]>(MethodBase.GetCurrentMethod(),argBool,argString,arrayITypes);
			}
			if(!argBool) {
				throw new ArgumentException("Invalid boolean value.  Required to be set to true.","argBool");
			}
			if(argString!=DirtyString) {
				throw new ArgumentException("Invalid string value.  Required to be equal to DirtyString.","argString");
			}
			return arrayITypes;
		}

		public static InvalidType[] SendEnumParams(params InvalidType[] arrayITypes) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<InvalidType[]>(MethodBase.GetCurrentMethod(),arrayITypes);
			}
			return arrayITypes;
		}

		public static List<Schedule> SendObjectParams(params Schedule[] arraySchedules) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Schedule>>(MethodBase.GetCurrentMethod(),arraySchedules);
			}
			return arraySchedules.ToList();
		}

		public static List<Schedule> SendObjectParamsWithArgs(bool argBool,string argString,params Schedule[] arraySchedules) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Schedule>>(MethodBase.GetCurrentMethod(),argBool,argString,arraySchedules);
			}
			if(!argBool) {
				throw new ArgumentException("Invalid boolean value.  Required to be set to true.","argBool");
			}
			if(argString!=DirtyString) {
				throw new ArgumentException("Invalid string value.  Required to be equal to DirtyString.","argString");
			}
			return arraySchedules.ToList();
		}

		public static Color SendColorParam(Color color) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Color>(MethodBase.GetCurrentMethod(),color);
			}
			return color;
		}

		public static Color SendProviderColor(Provider prov) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Color>(MethodBase.GetCurrentMethod(),prov);
			}
			return prov.ProvColor;
		}

		public static string SendSheetParameter(SheetParameter sheetParam) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),sheetParam);
			}
			return sheetParam.ParamName;
		}

		public static string SendSheetWithFields(Sheet sheet) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),sheet);
			}
			return sheet.SheetFields[0].FieldName;
		}

		public static string SendSheetDefWithFieldDefs(SheetDef sheetdef) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),sheetdef);
			}
			return sheetdef.SheetFieldDefs[0].FieldName;
		}

		public static TimeSpan GetTimeSpan() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<TimeSpan>(MethodBase.GetCurrentMethod());
			}
			return new TimeSpan(1,0,0);
		}

		public static string GetStringContainingCR() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			return NewLineString;
		}

		public static List<Task> GetListTasksContainingCR() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Task>>(MethodBase.GetCurrentMethod());
			}
			return new List<Task> { new Task { Descript=NewLineString } };
		}

		public static List<Task> GetListTasksSpecialChars(List<Task> listTasks) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Task>>(MethodBase.GetCurrentMethod(),listTasks);
			}
			List<Task> retVal=listTasks.Select(x => x.Copy()).ToList();
			retVal.Add(new Task {
				ParentDesc=NewLineString,
				DateTask=DateTodayTest,
				DateTimeEntry=DateTEntryTest,
				TaskStatus=TaskStatusEnum.Done
			});
			return retVal;
		}

		public static Family GetFamily() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Family>(MethodBase.GetCurrentMethod());
			}
			Patient pat=new Patient {
				FName="John",
				LName=null,
				AddrNote=DirtyString,
				ApptModNote=NewLineString,
				Email="service@opendental.com",
				PatStatus=PatientStatus.NonPatient,
				AdmitDate=DateTodayTest,
				DateTStamp=DateTEntryTest
			};
			Patient pat2=new Patient {
				FName="Jennifer",
				LName=null,
				ApptModNote=NewLineString,
				Email="service@opendental.com",
				PatStatus=PatientStatus.NonPatient,
				AdmitDate=DateTodayTest,
				DateTStamp=DateTEntryTest
			};
			Family retVal=new Family { ListPats=new[] { pat,pat2 } };
			return retVal;
		}

		///<summary>Returns a list of MedLabs containing one MedLab object which has a list of MedLabResults containing one MedLabResult as a field.
		///This will test whether or not we can handle a return value that is a list of objects with lists of objects as fields.
		///Both the MedLab and the MedLabResult contain longs, the DirtyString, the NewLineString, an enum, and Date/DateTime fields.</summary>
		public static List<MedLab> GetListMedLabsSpecialChars() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<MedLab>>(MethodBase.GetCurrentMethod());
			}
			MedLab medLabCur=new MedLab {
				MedLabNum=1,
				NoteLab=DirtyString,
				NotePat=NewLineString,
				ResultStatus=ResultStatus.P,
				DateTimeEntered=DateTodayTest,
				DateTimeReported=DateTEntryTest
			};
			MedLabResult medLabResultCur=new MedLabResult {
				MedLabResultNum=2,
				MedLabNum=1,
				Note=DirtyString,
				ObsText=NewLineString,
				ObsSubType=DataSubtype.PDF,
				DateTimeObs=DateTEntryTest
			};
			medLabCur.ListMedLabResults.Add(medLabResultCur);
			return new List<MedLab> { medLabCur };
		}

		public static List<CDSIntervention> TriggerMatch(object triggerObject) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<CDSIntervention>>(MethodBase.GetCurrentMethod(),triggerObject);
			}
			//create one of: DiseaseDef, ICD9, Icd10, Snomed, Medication, RxNorm, Cvx, AllergyDef, EhrLabResult, Patient, Vitalsign, MedicationPat
			//fill List<object> with above objects
			//create CDSIntervention and set TriggerObjects=above list of objects
			Vitalsign vitalSign=(Vitalsign)triggerObject;
			vitalSign.WeightCode=DirtyString;
			List<object> listObjs=new List<object> { vitalSign,
				new Patient				{	AddrNote=DirtyString },
				new MedicationPat	{	MedDescript=DirtyString	},
				new EhrLabResult	{ UnitsText=DirtyString,ListEhrLabResultNotes=new List<EhrLabNote> { new EhrLabNote { Comments=DirtyString } } },
				new AllergyDef		{ Description=DirtyString },
				new Cvx						{	Description=DirtyString	},
				new RxNorm				{ Description=DirtyString },
				new Medication		{ Notes=DirtyString },
				new Snomed				{ Description=DirtyString },
				new ICD9					{ Description=DirtyString },
				new Icd10					{	Description=DirtyString	},
				new DiseaseDef		{ DiseaseName=DirtyString }
			};
			return new List<CDSIntervention> { new CDSIntervention {
				InterventionMessage=DirtyString,
				TriggerObjects=EhrTriggers.ConvertListToKnowledgeRequests(listObjs) } };
		}

		public static Vitalsign GetVitalsignFromObjectParam(object obj) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Vitalsign>(MethodBase.GetCurrentMethod(),obj);
			}
			Vitalsign retVal=(Vitalsign)obj;
			retVal.IsIneligible=false;
			retVal.Documentation=DirtyString;
			return retVal;
		}

		public static object GetObjectFromVitalsignParam(Vitalsign vs) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<object>(MethodBase.GetCurrentMethod(),vs);
			}
			vs.IsIneligible=false;
			vs.Documentation=DirtyString;
			return (object)vs;
		}

		public static List<ProcedureCode> GetProcCodeWithDirtyProperty(ProcedureCode pc,ProcedureCode pc2) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ProcedureCode>>(MethodBase.GetCurrentMethod(),pc,pc2);
			}
			pc.IsNew=false;
			pc2.IsNew=false;
			pc2.ProcCat=pc.ProcCat;
			return new List<ProcedureCode> { pc,pc2 };
		}

		public static bool SimulatedProcUpdate(ProcedureCode pc) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),pc);
			}
			return true;
		}

		///<summary>This invalid web method should NOT invoke the polymorphism below.</summary>
		public static void InvalidWebMethod(bool argBool) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());//Purposefully invalid middle tier call.
				return;
			}
		}

		///<summary>This invalid web method should NOT invoke the polymorphism above.</summary>
		public static void InvalidWebMethod() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),true);//Purposefully invalid middle tier call.
				return;
			}
		}

	}
}
