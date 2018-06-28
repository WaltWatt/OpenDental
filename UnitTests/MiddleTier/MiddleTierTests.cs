using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenDentBusiness;
using System.Data;
using System.Drawing;
using CodeBase;
using UnitTestsCore;
using System.Reflection;

namespace UnitTests.MiddleTier {
	[TestClass]
	public class MiddleTierTests:TestBase {

		///<summary>This method will get invoked before every single test.</summary>
		[TestInitialize]
		public void Initialize() {
			OpenDentBusiness.WebServices.OpenDentalServerProxy.MockOpenDentalServerCur=new OpenDentBusiness.WebServices.OpenDentalServerMockIIS();
			RemotingClient.RemotingRole=RemotingRole.ClientWeb;
		}

		///<summary>This method will get invoked after every single test.</summary>
		[TestCleanup]
		public void Cleanup() {
			OpenDentBusiness.WebServices.OpenDentalServerProxy.MockOpenDentalServerCur=null;
			RemotingClient.RemotingRole=RemotingRole.ClientDirect;
		}

		[TestMethod]
		public void MiddleTier_GetStringLong() {
			int intStrLen=1000000;
			string strAlphNumLong=CoreTypesT.CreateRandomAlphaNumericString(intStrLen);
			Assert.AreEqual(WebServiceTests.GetString(strAlphNumLong),"Processed: "+strAlphNumLong);
		}

		[TestMethod]
		public void MiddleTier_GetStringDirty() {
			Assert.AreEqual(WebServiceTests.GetString(WebServiceTests.DirtyString),"Processed: "+WebServiceTests.DirtyString);
		}

		[TestMethod]
		public void MiddleTier_GetStringNull() {
			Assert.AreEqual(null,WebServiceTests.GetStringNull());
		}

		[TestMethod]
		public void MiddleTier_GetStringCarriageReturn() {
			Assert.AreEqual(WebServiceTests.GetStringCarriageReturn(WebServiceTests.NewLineString),"Processed: "+WebServiceTests.NewLineString);
		}

		[TestMethod]
		public void MiddleTier_GetInt() {
			Assert.AreEqual(WebServiceTests.GetInt(1),2);
		}

		[TestMethod]
		public void MiddleTier_GetLong() {
			Assert.AreEqual(WebServiceTests.GetLong(1),2);
		}

		[TestMethod]
		public void MiddleTier_GetVoid() {
			WebServiceTests.GetVoid();
		}

		[TestMethod]
		public void MiddleTier_GetBool() {
			Assert.AreEqual(true,WebServiceTests.GetBool());
		}

		[TestMethod]
		public void MiddleTier_GetObjectPat() {
			Patient pat=WebServiceTests.GetObjectPat();
			List<string>  strErrors=new List<string>();
			if(pat==null) {
				strErrors.Add("The patient returned is null.");
			}
			else {
				if(pat.FName!=null) {
					strErrors.Add("The patient.FName should be null but returned '"+pat.FName+"'.");
				}
				if(pat.LName!="Smith") {
					strErrors.Add("The patient.LName should be 'Smith' but returned "+(pat.LName==null?"null":("'"+pat.LName+"'"))+"'.");
				}
				if(pat.AddrNote!=WebServiceTests.DirtyString) {
					strErrors.Add(string.Format(@"The patient.AddrNote should be '{0}' but returned {1}.",WebServiceTests.DirtyString,
						pat.AddrNote==null?"null":("'"+pat.AddrNote+"'")));
				}
			}
			Assert.AreEqual(0,strErrors.Count);
		}

		[TestMethod]
		public void MiddleTier_GetListPats() {
			List<Patient> listPats=WebServiceTests.GetListPats();
			List<string> strErrors=new List<string>();
			if(listPats==null) {
				strErrors.Add("The list of patients returned is null.");
			}
			else {
				if(listPats[0].FName!=null) {
					strErrors.Add("The first patient in the list of patients FName should be null but returned '"+listPats[0].FName+"'.");
				}
				if(listPats[0].LName!="Smith") {
					strErrors.Add("The first patient in the list of patients LName should be 'Smith' but returned "+(listPats[0].LName==null ? "null" : ("'"+listPats[0].LName+"'"))+"'.");
				}
				if(listPats[0].AddrNote!=WebServiceTests.DirtyString) {
					strErrors.Add(string.Format(@"The first patient in the list of patients AddrNote should be '{0}' but returned {1}.",WebServiceTests.DirtyString,
						listPats[0].AddrNote==null ? "null" : ("'"+listPats[0].AddrNote+"'")));
				}
			}
			Assert.AreEqual(0,strErrors.Count);
		}

		[TestMethod]
		public void MiddleTier_GetTable() {
			DataTable table=WebServiceTests.GetTable();
			Assert.IsTrue(table!=null && table.Rows!=null 
				&& table.Rows.Count>0 
				&& table.Rows[0]["Col1"]!=null 
				&& table.Rows[0]["Col1"].ToString()=="cell00");
		}

		[TestMethod]
		public void MiddleTier_GetTableCarriageReturn() {
			DataTable table=WebServiceTests.GetTableCarriageReturn();
			Assert.IsTrue(table!=null && table.Rows!=null 
				&& table.Rows.Count>0 
				&& table.Columns.Count>0 
				&& table.Rows[0]!=null 
				&& table.Rows[0][0]!=null
				&& table.Rows[0][0].ToString()==WebServiceTests.NewLineString);
		}

		[TestMethod]
		public void MiddleTier_GetTable2by3() {
			DataTable table=WebServiceTests.GetTable2by3();
			List<string> strErrors=new List<string>();
			for(int i = 0;i<table.Rows.Count;i++) {
				for(int j = 0;j<table.Columns.Count;j++) {
					if(table.Rows[i][j].ToString()!="cell"+i+j) {
						strErrors.Add(string.Format(@"The table cell should be '{0}' but returned '{1}'.","cell"+i+j,table.Rows[i][j]));
					}
				}
			}
			Assert.IsTrue(strErrors.Count==0);
		}

		[TestMethod]
		public void MiddleTier_GetTableSpecialChars() {
			DataTable table=WebServiceTests.GetTableSpecialChars();
			char[] chars={'|','<','>','&','\'','"','\\','/'};
			List<string> strErrors=new List<string>();
			for(int i=0;i<table.Rows.Count;i++) {
				for(int j=0;j<table.Columns.Count-1;j++) {//last column is for DirtyString
					if(table.Rows[i][j].ToString()!="cell"+i+j+chars[i*2+j]) {
						strErrors.Add(string.Format(@"The table cell should be '{0}' but returned '{1}'.","cell"+i+j+chars[i*2+j],table.Rows[i][j]));
					}
				}
			}
			if(table.Rows[0]["DirtyString"].ToString()!=WebServiceTests.DirtyString) {
				strErrors.Add(string.Format(@"The table cell should be '{0}' but returned '{1}'.",WebServiceTests.DirtyString,table.Rows[0]["DirtyString"]));
			}
			Assert.IsTrue(strErrors.Count==0);
		}

		[TestMethod]
		public void MiddleTier_GetTableDataTypes() {
			DataTable table=WebServiceTests.GetTableDataTypes();
			List<string> strErrors=new List<string>();
			if(table==null || table.Rows==null || table.Columns==null || table.Rows.Count<1 || table.Rows[0]==null) {
				strErrors.Add(table==null?"The DataTable is null.":table.Rows==null?"The DataRowCollection is null.":
					table.Columns==null?"The DataColumnCollection is null.":table.Rows.Count<1?"The DataRowCollection is empty.":"The DataRow is null.");
			}
			else {
				if(table.Columns.Count<1 || table.Rows[0][0]==null || table.Rows[0][0].GetType()!=typeof(string)) {
					strErrors.Add(string.Format("The cell DataType should be {0} but returned {1}.",typeof(string),
						table.Columns.Count<1?"an insufficient column count":table.Rows[0][0]==null?"a null cell":table.Rows[0][0].GetType().ToString()));
				}
				if(table.Columns.Count<2 || table.Rows[0][1]==null || table.Rows[0][1].GetType()!=typeof(decimal)) {
					strErrors.Add(string.Format("The cell DataType should be {0} but returned {1}.",typeof(decimal),
						table.Columns.Count<2?"an insufficient column count":table.Rows[0][1]==null?"a null cell":table.Rows[0][1].GetType().ToString()));
				}
				if(table.Columns.Count<3 || table.Rows[0][2]==null || table.Rows[0][2].GetType()!=typeof(DateTime)) {
					strErrors.Add(string.Format("The cell DataType should be {0} but returned {1}.",typeof(DateTime),
						table.Columns.Count<3?"an insufficient column count":table.Rows[0][2]==null?"a null cell":table.Rows[0][2].GetType().ToString()));
				}
			}
			Assert.IsTrue(strErrors.Count==0);
		}

		[TestMethod]
		public void MiddleTier_GetDataSet() {
			DataSet ds=WebServiceTests.GetDataSet();
			List<string> strErrors=new List<string>();
			if(ds==null || ds.Tables==null || ds.Tables.Count<1 || ds.Tables[0]==null || ds.Tables[0].TableName!="table0") {
				strErrors.Add(string.Format("The DataTable's name in the DataSet should be {0} but returned {1}.","table0",
					ds==null?"a null DataSet":ds.Tables==null?"a null DataTableCollection":ds.Tables.Count<1?"an empty DataTableCollection":
					ds.Tables[0]==null?"a null DataTable":ds.Tables[0].TableName??"a null TableName"));
			}
			if(ds==null || ds.Tables==null || ds.Tables.Count<1 || ds.Tables[0]==null || ds.Tables[0].Rows.Count<1
				|| ds.Tables[0].Rows[0]["DirtyString"].ToString()!=WebServiceTests.DirtyString)
			{
				strErrors.Add(string.Format(@"The cell value in the DataSet should be {0} but returned {1}.",WebServiceTests.DirtyString,
					ds==null?"a null DataSet":ds.Tables==null?"a null DataTableCollection":ds.Tables.Count<1?"an empty DataTableCollection":
					ds.Tables[0]==null?"a null DataTable":ds.Tables[0].Rows.Count<1?"an empty DataRowCollection":
					ds.Tables[0].Rows[0]["DirtyString"]??"a null cell"));
			}
			Assert.IsTrue(strErrors.Count==0);
		}

		[TestMethod]
		public void MiddleTier_GetListInt() {
			List<int> listInt=WebServiceTests.GetListInt();
			Assert.IsTrue(listInt!=null && listInt.Count>0 && listInt[0]==2);
		}

		[TestMethod]
		public void MiddleTier_GetListString() {
			List<string> listString=WebServiceTests.GetListString();
			Assert.IsTrue(listString!=null && listString.Count > 0 && listString[0]=="Clean");
		}

		[TestMethod]
		public void MiddleTier_GetListStringDirty() {
			List<string> listStringDirty=WebServiceTests.GetListStringDirty();
			Assert.IsTrue(listStringDirty!=null && listStringDirty.Count > 0 && listStringDirty[0]==WebServiceTests.DirtyString);
		}

		[TestMethod]
		public void MiddleTier_GetArrayString() {
			string[] arrayString=WebServiceTests.GetArrayString();
			Assert.IsTrue(arrayString!=null && arrayString.Length > 0 && arrayString[0]=="Clean");
		}

		[TestMethod]
		public void MiddleTier_GetArrayStringDirty() {
			string[] arrayStringDirty=WebServiceTests.GetArrayStringDirty();
			Assert.IsTrue(arrayStringDirty!=null && arrayStringDirty.Length > 0 && arrayStringDirty[0]==WebServiceTests.DirtyString);
		}

		[TestMethod]
		public void MiddleTier_GetArrayPatient() {
			Patient[] arrayPat=WebServiceTests.GetArrayPatient();
			List<string> strErrors=new List<string>();
			if(arrayPat==null || arrayPat.Length<2) {
				strErrors.Add(arrayPat==null?"The patient array is null.":"The patient array contains an insufficient number of patients.");
			}
			else {
				if(arrayPat[0]==null || arrayPat[0].LName!="Jones") {
					strErrors.Add(string.Format("The patient in the array should have the LName {0} but returned {1}.","Jones",
						arrayPat[0]==null?"a null patient":arrayPat[0].LName??"a null LName"));
				}
				if(arrayPat[0]==null || arrayPat[0].AddrNote!=WebServiceTests.DirtyString) {
					strErrors.Add(string.Format(@"The patient in the array should have the AddrNote {0} but returned {1}.",WebServiceTests.DirtyString,
						arrayPat[0]==null?"a null patient":arrayPat[0].AddrNote??"a null AddrNote"));
				}
				if(arrayPat[1]!=null) {
					strErrors.Add("The patient array should contain a null patient but returned a non-null patient.");
				}
			}
			Assert.IsTrue(strErrors.Count==0);
		}

		///<summary>Reflection treats methods that have an array as their only argument differently than methods with multiple arguments.
		///When an array is the only argument, the functionality somehow mimics what happens when a method utilizes the params keyword.
		///Meaning, an array of three objects will act like three separate objects instead of acting like an array of three objects.
		///This functionality goes away as soon as another argument is added to the method signature (why we haven't noticed this problem yet).</summary>
		[TestMethod]
		public void MiddleTier_SendArrayPatient() {
			Patient[] arrayPatients=new Patient[3];
			arrayPatients[0]=new Patient { LName="Jones",AddrNote=WebServiceTests.DirtyString };
			arrayPatients[1]=null;
			arrayPatients[2]=new Patient { SchedAfterTime=new TimeSpan(5,30,22) };
			Patient[] arrayPatientsReturned=WebServiceTests.SendArrayPatient(arrayPatients);
			Assert.AreNotEqual(arrayPatientsReturned,null);
			Assert.AreEqual(arrayPatientsReturned.Length,3);
			Assert.AreEqual(arrayPatientsReturned[0].LName,"Jones");
			Assert.AreEqual(arrayPatientsReturned[0].AddrNote,WebServiceTests.DirtyString);
			Assert.AreEqual(arrayPatientsReturned[1],null);
			Assert.AreEqual(arrayPatientsReturned[2].SchedAfterTime.Hours,5);
			Assert.AreEqual(arrayPatientsReturned[2].SchedAfterTime.Minutes,30);
			Assert.AreEqual(arrayPatientsReturned[2].SchedAfterTime.Seconds,22);
		}
		
		[TestMethod]
		public void MiddleTier_SendIntParams() {
			int[] arrayITypesReturned=WebServiceTests.SendIntParams(6,2,9);
			Assert.AreNotEqual(arrayITypesReturned,null);
			Assert.AreEqual(arrayITypesReturned.Length,3);
			Assert.AreEqual(arrayITypesReturned[0],6);
			Assert.AreEqual(arrayITypesReturned[1],2);
			Assert.AreEqual(arrayITypesReturned[2],9);
		}
		
		[TestMethod]
		public void MiddleTier_SendStringParams() {
			string[] arrayStringsReturned=WebServiceTests.SendStringParams("Str",null,WebServiceTests.DirtyString);
			Assert.AreNotEqual(arrayStringsReturned,null);
			Assert.AreEqual(arrayStringsReturned.Length,3);
			Assert.AreEqual(arrayStringsReturned[0],"Str");
			Assert.AreEqual(arrayStringsReturned[1],null);
			Assert.AreEqual(arrayStringsReturned[2],WebServiceTests.DirtyString);
		}
		
		[TestMethod]
		public void MiddleTier_SendEnumParams() {
			InvalidType[] arrayITypesReturned=WebServiceTests.SendEnumParams(InvalidType.Prefs,InvalidType.AccountingAutoPays,InvalidType.AlertSubs);
			Assert.AreNotEqual(arrayITypesReturned,null);
			Assert.AreEqual(arrayITypesReturned.Length,3);
			Assert.AreEqual(arrayITypesReturned[0],InvalidType.Prefs);
			Assert.AreEqual(arrayITypesReturned[1],InvalidType.AccountingAutoPays);
			Assert.AreEqual(arrayITypesReturned[2],InvalidType.AlertSubs);
		}
		
		[TestMethod]
		public void MiddleTier_SendEnumParams_WithArgs() {
			InvalidType[] arrayITypesReturned=WebServiceTests.SendEnumParamsWithArgs(true,WebServiceTests.DirtyString,
				InvalidType.Prefs,InvalidType.AccountingAutoPays,InvalidType.AlertSubs);
			Assert.AreNotEqual(arrayITypesReturned,null);
			Assert.AreEqual(arrayITypesReturned.Length,3);
			Assert.AreEqual(arrayITypesReturned[0],InvalidType.Prefs);
			Assert.AreEqual(arrayITypesReturned[1],InvalidType.AccountingAutoPays);
			Assert.AreEqual(arrayITypesReturned[2],InvalidType.AlertSubs);
		}

		[TestMethod]
		public void MiddleTier_SendNullParam() {
			string stringNull=WebServiceTests.SendNullParam(null);
			Assert.AreEqual(null,stringNull);
		}

		[TestMethod]
		public void MiddleTier_GetInterface() {
			LogWriter log=new LogWriter(LogLevel.Error,"LogWeb\\WebSchedRecall");
			LogWriter logReturned=(LogWriter)WebServiceTests.GetInterface(log);
			Assert.AreEqual(logReturned.LogLevel,LogLevel.Error);
			Assert.AreEqual(logReturned.BaseDirectory,"LogWeb\\WebSchedRecall");
		}

		[TestMethod]
		public void MiddleTier_SendInterface_WithArgs() {
			List<long> listLongs=WebServiceTests.SendInterfaceParamWithArgs(true,listArgLongs:new List<long>() { 6,2,9 });
			Assert.AreEqual(listLongs.Count,3);
			Assert.AreEqual(listLongs[0],6);
			Assert.AreEqual(listLongs[1],2);
			Assert.AreEqual(listLongs[2],9);
		}

		[TestMethod]
		public void MiddleTier_GetObjectNull() {
			Patient pat2=WebServiceTests.GetObjectNull();
			Assert.IsTrue(pat2==null);
		}

		///<summary>The purpose of this test is to make sure middle tier can invoke methods that utilize the params keyword.</summary>
		[TestMethod]
		public void MiddleTier_SendObjectParams() {
			List<Schedule> listRetVals=WebServiceTests.SendObjectParams(new Schedule() { ScheduleNum=5 },new Schedule() { ScheduleNum=23 });
			Assert.AreEqual(listRetVals.Count,2);
			Assert.AreEqual(listRetVals[0].ScheduleNum,5);
			Assert.AreEqual(listRetVals[1].ScheduleNum,23);
		}

		///<summary>The purpose of this test is to make sure middle tier can invoke methods that utilize the params keyword with other args.</summary>
		[TestMethod]
		public void MiddleTier_SendObjectParams_WithArgs() {
			List<Schedule> listRetVals=WebServiceTests.SendObjectParamsWithArgs(true,WebServiceTests.DirtyString
				,new Schedule() { ScheduleNum=5 }
				,new Schedule() { ScheduleNum=23 });
			Assert.AreEqual(listRetVals.Count,2);
			Assert.AreEqual(listRetVals[0].ScheduleNum,5);
			Assert.AreEqual(listRetVals[1].ScheduleNum,23);
		}

		[TestMethod]
		public void MiddleTier_SendColorParam() {
			Color colorResult=WebServiceTests.SendColorParam(Color.Green);
			Assert.IsTrue(colorResult!=null && colorResult.ToArgb()==Color.Green.ToArgb());
		}

		[TestMethod]
		public void MiddleTier_SendProviderColor() {
			Provider prov=new Provider();
			prov.ProvColor=Color.Fuchsia;
			Color colorResult=WebServiceTests.SendProviderColor(prov);
			Assert.IsTrue(colorResult!=null && colorResult.ToArgb()==Color.Fuchsia.ToArgb());
		}

		[TestMethod]
		public void MiddleTier_SendSheetParameter() {
			SheetParameter sheetParam=new SheetParameter(false,"ParamName");
			Assert.AreEqual("ParamName",WebServiceTests.SendSheetParameter(sheetParam));
		}

		[TestMethod]
		public void MiddleTier_SendSheetWithFields() {
			Sheet sheet=new Sheet();
			sheet.SheetFields=new List<SheetField>();
			sheet.Parameters=new List<SheetParameter>();
			SheetField field=new SheetField();
			field.FieldName="FieldName";
			sheet.SheetFields.Add(field);
			Assert.AreEqual("FieldName",WebServiceTests.SendSheetWithFields(sheet));
		}

		[TestMethod]
		public void MiddleTier_SendSheetDefWithFieldDefs() {
			SheetDef sheetdef=new SheetDef();
			sheetdef.SheetFieldDefs=new List<SheetFieldDef>();
			sheetdef.Parameters=new List<SheetParameter>();
			SheetFieldDef fielddef=new SheetFieldDef();
			fielddef.FieldName="FieldName";
			sheetdef.SheetFieldDefs.Add(fielddef);
			Assert.AreEqual("FieldName",WebServiceTests.SendSheetDefWithFieldDefs(sheetdef));
		}

		[TestMethod]
		public void MiddleTier_GetTimeSpan() {
			Assert.AreEqual(new TimeSpan(1,0,0),WebServiceTests.GetTimeSpan());
		}

		[TestMethod]
		public void MiddleTier_GetStringContainingCR() {
			Assert.AreEqual(WebServiceTests.NewLineString,WebServiceTests.GetStringContainingCR());
		}

		[TestMethod]
		public void MiddleTier_GetListTasksContainingCR() {
			OpenDentBusiness.Task t=WebServiceTests.GetListTasksContainingCR()[0];
			Assert.IsTrue(t!=null && t.Descript==WebServiceTests.NewLineString);
		}

		[TestMethod]
		public void MiddleTier_GetListTasksSpecialChars() {
			//Tests special chars, new lines, Date, DateTime, and enum values in a list of objects as the parameter and the return value
			List<OpenDentBusiness.Task> listTasks=new List<OpenDentBusiness.Task> { new OpenDentBusiness.Task {
				Descript=WebServiceTests.DirtyString,
				ParentDesc=WebServiceTests.NewLineString,
				DateTask=WebServiceTests.DateTodayTest,
				DateTimeEntry=WebServiceTests.DateTEntryTest,
				TaskStatus=TaskStatusEnum.Done } };
			List<OpenDentBusiness.Task> listTasksReturned=WebServiceTests.GetListTasksSpecialChars(listTasks);
			List<string> strErrors=new List<string>();
			if(listTasksReturned==null || listTasksReturned.Count<1) {
				strErrors.Add(listTasksReturned==null?"The list of tasks is null.":"The list of tasks contains an insufficient number of tasks.");
			}
			int idx=0;
			foreach(OpenDentBusiness.Task task in listTasksReturned) {
				if(task==null) {
					strErrors.Add("The tasklist contains a null task.");
					idx++;
					continue;
				}
				if(idx==0 && task.Descript!=WebServiceTests.DirtyString) {
					strErrors.Add(string.Format(@"The task.Descript should be {0} but returned {1}.",WebServiceTests.DirtyString,task.Descript??"null"));
				}
				if(task.ParentDesc!=WebServiceTests.NewLineString) {
					strErrors.Add(string.Format(@"The task.ParentDesc should be {0} but returned {1}.",WebServiceTests.NewLineString,task.ParentDesc??"null"));
				}
				if(task.DateTask==null || task.DateTask.Date!=WebServiceTests.DateTodayTest.Date) {
					strErrors.Add(string.Format("The task.DateTask should be {0} but returned {1}.",WebServiceTests.DateTodayTest.ToShortDateString(),
						task.DateTask==null?"null":task.DateTask.ToShortDateString()));
				}
				if(task.DateTimeEntry!=WebServiceTests.DateTEntryTest) {
					strErrors.Add(string.Format("The task.DateTimeEntry should be {0} but returned {1}.",WebServiceTests.DateTEntryTest.ToString(),
						task.DateTimeEntry==null?"null":task.DateTimeEntry.ToString()));
				}
				if(task.TaskStatus!=TaskStatusEnum.Done) {
					strErrors.Add(string.Format("The task.TaskStatus should be {0} but returned {1}.",TaskStatusEnum.Done,task.TaskStatus));
				}
				idx++;
			}
			Assert.IsTrue(strErrors.Count==0);
		}

		[TestMethod]
		public void MiddleTier_GetFamily() {
			Family result=WebServiceTests.GetFamily();
			List<string> strErrors=new List<string>();
			if(result==null || result.ListPats==null || result.ListPats.Length<2) {
				strErrors.Add("The Family"+result==null?" object is null.":result.ListPats==null?".ListPats is null.":
					".ListPats contains an insufficient number of patients.");
			}
			else {
				for(int i=0;i<result.ListPats.Length;i++) {
					Patient p=result.ListPats[i];
					if(p.FName!=(i==0?"John":"Jennifer")) {
						strErrors.Add(string.Format("The patient.FName should be {0} but returned {1}.",i==0?"John":"Jennifer",p.FName??"null"));
					}
					if(p.LName!=null) {
						strErrors.Add("The patient.LName should be null but returned "+p.LName+".");
					}
					if(i==0 && p.AddrNote!=WebServiceTests.DirtyString) {
						strErrors.Add(string.Format(@"The patient.AddrNote should be {0} but returned {1}.",WebServiceTests.DirtyString,WebServiceTests.GetObjectPat().AddrNote??"null"));
					}
					if(p.ApptModNote!=WebServiceTests.NewLineString) {
						strErrors.Add(string.Format(@"The patient.ApptModNote should be {0} but returned {1}.",WebServiceTests.NewLineString,p.ApptModNote??"null"));
					}
					if(p.Email!="service@opendental.com") {
						strErrors.Add("The patient.Email should be service@opendental.com but returned "+(p.Email??"null")+".");
					}
					if(p.PatStatus!=PatientStatus.NonPatient) {
						strErrors.Add("The patient.PatStatus should be "+PatientStatus.NonPatient+" but returned "+p.PatStatus+".");
					}
					if(p.AdmitDate==null || p.AdmitDate.Date!=WebServiceTests.DateTodayTest.Date) {
						strErrors.Add(string.Format("The patient.AdmitDate should be {0} but returned {1}.",WebServiceTests.DateTodayTest.ToShortDateString(),
							p.AdmitDate==null?"null":p.AdmitDate.ToShortDateString()));
					}
					if(p.DateTStamp!=WebServiceTests.DateTEntryTest) {
						strErrors.Add(string.Format("The patient.DateTStamp should be {0} but returned {1}.",WebServiceTests.DateTEntryTest.ToString(),
							p.DateTStamp==null?"null":p.DateTStamp.ToString()));
					}
				}
			}
			Assert.IsTrue(strErrors.Count==0);
		}

		[TestMethod]
		public void MiddleTier_GetListMedLabsSpecialChars() {
			List<MedLab> listMLabs=WebServiceTests.GetListMedLabsSpecialChars();
			List<string> strErrors=new List<string>();
			if(listMLabs==null || listMLabs.Count<1) {
				strErrors.Add("The list of MedLabs is "+listMLabs==null?"null.":"empty.");
			}
			else {
				MedLab mlab=listMLabs[0];
				if(mlab.MedLabNum!=1) {
					strErrors.Add("The MedLabNum should be 1 but returned "+mlab.MedLabNum+".");
				}
				if(mlab.NoteLab!=WebServiceTests.DirtyString) {
					strErrors.Add(string.Format(@"The MedLab.NoteLab should be {0} but returned {1}.",WebServiceTests.DirtyString,mlab.NoteLab??"null"));
				}
				if(mlab.NotePat!=WebServiceTests.NewLineString) {
					strErrors.Add(string.Format(@"The MedLab.NotePat should be {0} but returned {1}.",WebServiceTests.NewLineString,mlab.NotePat??"null"));
				}
				if(mlab.ResultStatus!=ResultStatus.P) {
					strErrors.Add("The MedLab.ResultStatus should be "+ResultStatus.P+" but returned "+mlab.ResultStatus+".");
				}
				if(mlab.DateTimeEntered==null || mlab.DateTimeEntered.Date!=WebServiceTests.DateTodayTest.Date) {
					strErrors.Add(string.Format("The MedLab.DateTimeEntered should be {0} but returned {1}.",WebServiceTests.DateTodayTest.ToShortDateString(),
						mlab.DateTimeEntered==null?"null":mlab.DateTimeEntered.ToShortDateString()));
				}
				if(mlab.DateTimeReported!=WebServiceTests.DateTEntryTest) {
					strErrors.Add(string.Format("The MedLab.DateTimeReported should be {0} but returned {1}.",WebServiceTests.DateTEntryTest.ToString(),
						mlab.DateTimeReported==null?"null":mlab.DateTimeReported.ToString()));
				}
				if(mlab.ListMedLabResults==null || mlab.ListMedLabResults.Count<1) {
					strErrors.Add("The list of MedLabResults for the MedLab is "+mlab.ListMedLabResults==null?"null.":"empty.");
				}
				else {
					MedLabResult mlr=mlab.ListMedLabResults[0];
					if(mlr.MedLabResultNum!=2) {
						strErrors.Add("The MedLabResultNum should be 2 but returned "+mlr.MedLabResultNum+".");
					}
					if(mlr.MedLabNum!=1) {
						strErrors.Add("The MedLabResult.MedLabNum should be 1 but returned "+mlr.MedLabNum+".");
					}
					if(mlr.Note!=WebServiceTests.DirtyString) {
						strErrors.Add(string.Format(@"The MedLabResult.Note should be {0} but returned {1}.",WebServiceTests.DirtyString,mlr.Note??"null"));
					}
					if(mlr.ObsText!=WebServiceTests.NewLineString) {
						strErrors.Add(string.Format(@"The MedLabResult.ObsText should be {0} but returned {1}.",WebServiceTests.NewLineString,mlr.ObsText??"null"));
					}
					if(mlr.ObsSubType!=DataSubtype.PDF) {
						strErrors.Add("The MedLabResult.ObsSubType should be "+DataSubtype.PDF+" but returned "+mlr.ObsSubType+".");
					}
					if(mlr.DateTimeObs!=WebServiceTests.DateTEntryTest) {
						strErrors.Add(string.Format("The MedLabResult.DateTimeObs should be {0} but returned {1}.",WebServiceTests.DateTEntryTest.ToString(),
							mlr.DateTimeObs==null?"null":mlr.DateTimeObs.ToString()));
					}
				}
			}
			Assert.IsTrue(strErrors.Count==0);
		}

		[TestMethod]
		public void MiddleTier_ObjectParamType() {
			Vitalsign vs=new Vitalsign { IsIneligible=true };
			vs=WebServiceTests.GetVitalsignFromObjectParam(vs);
			List<string> strErrors=new List<string>();
			if(vs.GetType()!=typeof(Vitalsign)) {
				strErrors.Add(string.Format("The object returned is not a {0} it is a {1}.",typeof(Vitalsign),vs.GetType()));
			}
			if(vs.IsIneligible) {
				strErrors.Add(string.Format("The vitalsign object IsIneligible flag should be {0} but returned {1}.","true",vs.IsIneligible.ToString()));
			}
			if(vs.Documentation!=WebServiceTests.DirtyString) {
				strErrors.Add("The vitalsign object returned did not have the correct dirty string.");
			}
			Assert.IsTrue(strErrors.Count==0);
		}

		[TestMethod]
		public void MiddleTier_GetProcCodeWithDirtyProperty() {
			Def d;
			if(Defs.GetDefsForCategory(DefCat.ProcCodeCats,true).Count==0) {
				d=new Def() { Category=DefCat.ProcCodeCats,ItemName=WebServiceTests.DirtyString };
				d.DefNum=Defs.Insert(d);
			}
			else {
				d=Defs.GetFirstForCategory(DefCat.ProcCodeCats,true);
				d.ItemName=WebServiceTests.DirtyString;
				Defs.Update(d);
			}
			Defs.RefreshCache();
			d=Defs.GetDef(DefCat.ProcCodeCats,d.DefNum);
			ProcedureCode pc=new ProcedureCode { IsNew=true,ProcCat=d.DefNum };
			ProcedureCode pc2=new ProcedureCode { IsNew=true };
			List<ProcedureCode> listPcs=new List<ProcedureCode>();
			List<string> strErrors=new List<string>();
			try {
				listPcs=WebServiceTests.GetProcCodeWithDirtyProperty(pc,pc2);
			}
			catch(Exception ex) {
				strErrors.Add("Cannot serialize a property with a getter that does not retrieve the same value the setter is manipulating.");
				strErrors.Add(ex.Message);
				strErrors.Add(ex.StackTrace);
			}
			if(listPcs.Count>0 && (listPcs[0].IsNew || listPcs[1].IsNew)) {
				strErrors.Add(string.Format("One or more of the returned ProcedureCode objects IsNew flag should be {0} but returned {1}.","false","true"));
			}
			if(listPcs.Count>0 && (listPcs[0].ProcCat!=d.DefNum||listPcs[1].ProcCat!=d.DefNum)) {
				strErrors.Add("One or more of the ProcedureCode objects returned did not have the correct ProcCat.");
			}
			if(listPcs.Count>0 && (listPcs[0].ProcCatDescript!=d.ItemName || listPcs[1].ProcCatDescript!=d.ItemName)) {
				strErrors.Add("One or more of the ProcedureCode objects returned did not have the correct dirty string.");
			}
			Assert.IsTrue(strErrors.Count==0);
		}

		[TestMethod]
		public void MiddleTier_SimulatedProcUpdate() {
			ProcedureCode pc=new ProcedureCode { Descript="periodic oral evaluation - established patient & stuff",ProcCatDescript="Exams & Xrays",DefaultNote=WebServiceTests.DirtyString };
			ProcedureCode pc2=pc.Copy();
			WebServiceTests.SimulatedProcUpdate(pc);
			List<string> strErrors=new List<string>();
			if(pc.Descript!=pc2.Descript || pc.ProcCatDescript!=pc2.ProcCatDescript) {
				strErrors.Add(string.Format(@"The Descript before is ""{0}"" and after is ""{1}"".  The ProcCatDescript before is ""{2}"" and after is ""{3}"".",pc2.Descript,pc.Descript,pc2.ProcCatDescript,pc.ProcCatDescript));
			}
			if(pc.DefaultNote!=pc2.DefaultNote) {
				strErrors.Add("The dirty string was altered from the simulated update call.");
			}
			Assert.IsTrue(strErrors.Count==0);
		}

		///<summary>This test explicitly tests that Middle Tier does not invoke polymorphisms when passed an incorrect number of parameters.</summary>
		[TestMethod]
		public void MiddleTier_InvalidWebMethod_TooFew() {
			bool hasFailed=false;
			try {
				WebServiceTests.InvalidWebMethod(true);
			}
			catch(Exception ex) {
				ex.DoNothing();
				hasFailed=true;
			}
			Assert.IsTrue(hasFailed);
		}

		///<summary>This test explicitly tests that Middle Tier does not invoke polymorphisms when passed an incorrect number of parameters.</summary>
		[TestMethod]
		public void MiddleTier_InvalidWebMethod_TooMany() {
			bool hasFailed=false;
			try {
				WebServiceTests.InvalidWebMethod();
			}
			catch(Exception ex) {
				ex.DoNothing();
				hasFailed=true;
			}
			Assert.IsTrue(hasFailed);
		}

		///<summary>Our CRUD method ListToTable is incorrectly setting TimeSpan columns by surrounding the time in single quotes.
		///TimeSpans that are formatted like '08:00:00' cannot be parsed correctly which causes the program to do two things:
		///1. It significantly slows down (due to trying its hardest to parse the string literal into a TimeSpan).
		///2. Throws an exception which we catch within PIn.TimeSpan() which sadly turns the end result to System.TimeSpan.Zero.
		///The scenario above describes how our cache classes work over the middle tier which ends up losing data and taking a long time.</summary>
		[TestMethod]
		public void MiddleTier_GetListToTable_TimeSpans() {
			ApptViewT.ClearApptView();
			long apptViewNum1=ApptViewT.CreateApptView(MethodBase.GetCurrentMethod().Name,new TimeSpan(5,20,13)).ApptViewNum;
			long apptViewNum2=ApptViewT.CreateApptView(MethodBase.GetCurrentMethod().Name,new TimeSpan(9,0,45)).ApptViewNum;
			long apptViewNum3=ApptViewT.CreateApptView(MethodBase.GetCurrentMethod().Name).ApptViewNum;
			List<ApptView> listApptViews=ApptViews.GetDeepCopy();
			Assert.AreEqual(listApptViews.Count,3);
			Assert.AreEqual(5,listApptViews.First(x => x.ApptViewNum==apptViewNum1).ApptTimeScrollStart.Hours);
			Assert.AreEqual(20,listApptViews.First(x => x.ApptViewNum==apptViewNum1).ApptTimeScrollStart.Minutes);
			Assert.AreEqual(13,listApptViews.First(x => x.ApptViewNum==apptViewNum1).ApptTimeScrollStart.Seconds);
			Assert.AreEqual(9,listApptViews.First(x => x.ApptViewNum==apptViewNum2).ApptTimeScrollStart.Hours);
			Assert.AreEqual(0,listApptViews.First(x => x.ApptViewNum==apptViewNum2).ApptTimeScrollStart.Minutes);
			Assert.AreEqual(45,listApptViews.First(x => x.ApptViewNum==apptViewNum2).ApptTimeScrollStart.Seconds);
			Assert.AreEqual(TimeSpan.Zero.Hours,listApptViews.First(x => x.ApptViewNum==apptViewNum3).ApptTimeScrollStart.Hours);
			Assert.AreEqual(TimeSpan.Zero.Minutes,listApptViews.First(x => x.ApptViewNum==apptViewNum3).ApptTimeScrollStart.Minutes);
			Assert.AreEqual(TimeSpan.Zero.Seconds,listApptViews.First(x => x.ApptViewNum==apptViewNum3).ApptTimeScrollStart.Seconds);
		}

	}
}
