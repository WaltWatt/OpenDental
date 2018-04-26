using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Prefs{
		#region Get Methods
		#endregion

		#region Modification Methods
		
		#region Insert
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion

		#region CachePattern

		///<summary>Utilizes the NonPkAbs version of CacheDict because it uses PrefName instead of the PK PrefNum.</summary>
		private class PrefCache : CacheDictNonPkAbs<Pref,string,Pref> {
			protected override List<Pref> GetCacheFromDb() {
				string command="SELECT * FROM preference";
				return Crud.PrefCrud.SelectMany(command);
			}
			protected override List<Pref> TableToList(DataTable table) {
				//Can't use Crud.PrefCrud.TableToList(table) because it will fail the first time someone runs 7.6 before conversion.
				List<Pref> listPrefs=new List<Pref>();
				bool containsPrefNum=(table.Columns.Contains("PrefNum"));
				foreach(DataRow row in table.Rows) {
					Pref pref=new Pref();
					if(containsPrefNum) {
						pref.PrefNum=PIn.Long(row["PrefNum"].ToString());
					}
					pref.PrefName=PIn.String(row["PrefName"].ToString());
					pref.ValueString=PIn.String(row["ValueString"].ToString());
					//no need to load up the comments.  Especially since this will fail when user first runs version 5.8.
					listPrefs.Add(pref);
				}
				return listPrefs;
			}
			protected override Pref Copy(Pref pref) {
				return pref.Copy();
			}
			protected override DataTable DictToTable(Dictionary<string,Pref> dictPrefs) {
				return Crud.PrefCrud.ListToTable(dictPrefs.Values.ToList(),"Pref");
			}
			protected override void FillCacheIfNeeded() {
				Prefs.GetTableFromCache(false);
			}
			protected override string GetDictKey(Pref pref) {
				return pref.PrefName;
			}
			protected override Pref GetDictValue(Pref pref) {
				return pref;
			}
			protected override Pref CopyDictValue(Pref pref) {
				return pref.Copy();
			}
			protected override Dictionary<string,Pref> GetDictFromList(List<Pref> listPrefs) {
				Dictionary<string,Pref> dictPrefs=new Dictionary<string,Pref>();
				List<string> listDuplicatePrefs=new List<string>();
				foreach(Pref pref in listPrefs) {
					if(dictPrefs.ContainsKey(pref.PrefName)) {
						listDuplicatePrefs.Add(pref.PrefName);//The current preference is a duplicate preference.
					}
					else {
						dictPrefs.Add(pref.PrefName,pref);
					}
				}
				if(listDuplicatePrefs.Count>0 &&                                        //Duplicate preferences found, and
					dictPrefs.ContainsKey(PrefName.CorruptedDatabase.ToString()) &&       //CorruptedDatabase preference exists (only v3.4+), and
					dictPrefs[PrefName.CorruptedDatabase.ToString()].ValueString!="0")    //The CorruptedDatabase flag is set.
				{
					throw new ApplicationException(Lans.g("Prefs","Your database is corrupted because an update failed.  Please contact us.  This database is unusable and you will need to restore from a backup."));
				}
				else if(listDuplicatePrefs.Count>0) {//Duplicate preferences, but the CorruptedDatabase flag is not set.
					throw new ApplicationException(Lans.g("Prefs","Duplicate preferences found in database")+": "+String.Join(",",listDuplicatePrefs));
				}
				return dictPrefs;
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static PrefCache _prefCache=new PrefCache();

		public static bool GetContainsKey(string prefName) {
			return _prefCache.GetContainsKey(prefName);
		}

		public static bool DictIsNull() {
			return _prefCache.DictIsNull();
		}

		///<summary>Returns a deep copy of the corresponding preference by name from the cache.
		///Throws an exception indicating that the prefName passed in is invalid if it cannot be found in the cache (old behavior).</summary>
		public static Pref GetOne(PrefName prefName) {
			return GetOne(prefName.ToString());
		}

		///<summary>Returns a deep copy of the corresponding preference by name from the cache.
		///Throws an exception indicating that the prefName passed in is invalid if it cannot be found in the cache (old behavior).</summary>
		public static Pref GetOne(string prefName) {
			if(!_prefCache.GetContainsKey(prefName)) {
				throw new Exception(prefName+" is an invalid pref name.");
			}
			return _prefCache.GetOne(prefName);
		}

		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_prefCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_prefCache.FillCacheFromTable(table);
				return table;
			}
			return _prefCache.GetTableFromCache(doRefreshCache);
		}

		public static void UpdateValueForKey(Pref pref) {
			_prefCache.SetValueForKey(pref.PrefName,pref);
		}

		#endregion

		///<summary>Gets a pref of type bool without using the cache.</summary>
		public static bool GetBoolNoCache(PrefName prefName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),prefName);
			}
			string command="SELECT ValueString FROM preference WHERE PrefName = '"+POut.String(prefName.ToString())+"'";
			return PIn.Bool(Db.GetScalar(command));
		}

		///<summary></summary>
		public static void Update(Pref pref) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),pref);
				return;
			}
			//Don't use CRUD here because we want to update based on PrefName instead of PrefNum.  Otherwise, it might fail the first time someone runs 7.6.
			string command= "UPDATE preference SET "
				+"ValueString = '"+POut.String(pref.ValueString)+"' "
				+" WHERE PrefName = '"+POut.String(pref.PrefName)+"'";
			Db.NonQ(command);
		}

		///<summary>Updates a pref of type int.  Returns true if a change was required, or false if no change needed.</summary>
		public static bool UpdateInt(PrefName prefName,int newValue) {
			return UpdateLong(prefName,newValue);
		}

		///<summary>Updates a pref of type byte.  Returns true if a change was required, or false if no change needed.</summary>
		public static bool UpdateByte(PrefName prefName,byte newValue) {
			return UpdateLong(prefName,newValue);
		}

		///<summary>Updates a pref of type int without using the cache.  Useful for multithreaded connections.</summary>
		public static void UpdateIntNoCache(PrefName prefName,int newValue) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),prefName,newValue);
				return;
			}
			string command="UPDATE preference SET ValueString='"+POut.Long(newValue)+"' WHERE PrefName='"+POut.String(prefName.ToString())+"'";
			Db.NonQ(command);
		}

		///<summary>Updates a pref of type long.  Returns true if a change was required, or false if no change needed.</summary>
		public static bool UpdateLong(PrefName prefName,long newValue) {
			//Very unusual.  Involves cache, so Meth is used further down instead of here at the top.
			long curValue=PrefC.GetLong(prefName);
			if(curValue==newValue) {
				return false;//no change needed
			}
			string command= "UPDATE preference SET "
				+"ValueString = '"+POut.Long(newValue)+"' "
				+"WHERE PrefName = '"+POut.String(prefName.ToString())+"'";
			bool retVal=true;
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				retVal=Meth.GetBool(MethodBase.GetCurrentMethod(),prefName,newValue);
			}
			else{
				Db.NonQ(command);
			}
			Pref pref=new Pref();
			pref.PrefName=prefName.ToString();
			pref.ValueString=newValue.ToString();
			Prefs.UpdateValueForKey(pref);
			return retVal;
		}

		///<summary>Updates a pref of type double.  Returns true if a change was required, or false if no change needed.
		///Set doRounding false when the double passed in needs to be Multiple Precision Floating-Point Reliable (MPFR).</summary>
		public static bool UpdateDouble(PrefName prefName,double newValue,bool doRounding=true) {
			//Very unusual.  Involves cache, so Meth is used further down instead of here at the top.
			double curValue=PrefC.GetDouble(prefName);
			if(curValue==newValue) {
				return false;//no change needed
			}
			string command = "UPDATE preference SET "
				+"ValueString = '"+((doRounding) ? POut.Double(newValue) : POut.Double(newValue,false))+"' "
				+"WHERE PrefName = '"+POut.String(prefName.ToString())+"'";
			bool retVal=true;
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				retVal=Meth.GetBool(MethodBase.GetCurrentMethod(),prefName,newValue);
			}
			else {
				Db.NonQ(command);
			}
			Pref pref=new Pref();
			pref.PrefName=prefName.ToString();
			pref.ValueString=newValue.ToString();
			Prefs.UpdateValueForKey(pref);
			return retVal;
		}

		///<summary>Returns true if a change was required, or false if no change needed.</summary>
		public static bool UpdateBool(PrefName prefName,bool newValue) {
			//No need to check RemotingRole; no call to db.
			return UpdateBool(prefName,newValue,false);
		}

		///<summary>Returns true if a change was required, or false if no change needed.</summary>
		public static bool UpdateBool(PrefName prefName,bool newValue,bool isForced) {
			//Very unusual.  Involves cache, so Meth is used further down instead of here at the top.
			bool curValue=PrefC.GetBool(prefName);
			if(!isForced && curValue==newValue) {
				return false;//no change needed
			}
			string command="UPDATE preference SET "
				+"ValueString = '"+POut.Bool(newValue)+"' "
				+"WHERE PrefName = '"+POut.String(prefName.ToString())+"'";
			bool retVal=true;
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				retVal=Meth.GetBool(MethodBase.GetCurrentMethod(),prefName,newValue,isForced);
			}
			else {
				Db.NonQ(command);
			}
			Pref pref=new Pref();
			pref.PrefName=prefName.ToString();
			pref.ValueString=POut.Bool(newValue);
			Prefs.UpdateValueForKey(pref);
			return retVal;
		}

		///<summary>Updates a bool without using cache classes.  Useful for multithreaded connections.</summary>
		public static void UpdateBoolNoCache(PrefName prefName,bool newValue) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),prefName,newValue);
				return;
			}
			string command="UPDATE preference SET ValueString='"+POut.Bool(newValue)+"' WHERE PrefName='"+POut.String(prefName.ToString())+"'";
			Db.NonQ(command);
		}

		///<summary>Returns true if a change was required, or false if no change needed.</summary>
		public static bool UpdateString(PrefName prefName,string newValue) {
			//Very unusual.  Involves cache, so Meth is used further down instead of here at the top.
			string curValue=PrefC.GetString(prefName);
			if(curValue==newValue) {
				return false;//no change needed
			}
			string command = "UPDATE preference SET "
				+"ValueString = '"+POut.String(newValue)+"' "
				+"WHERE PrefName = '"+POut.String(prefName.ToString())+"'";
			bool retVal=true;
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				retVal=Meth.GetBool(MethodBase.GetCurrentMethod(),prefName,newValue);
			}
			else {
				Db.NonQ(command);
			}
			Pref pref=new Pref();
			pref.PrefName=prefName.ToString();
			pref.ValueString=newValue;
			Prefs.UpdateValueForKey(pref);
			return retVal;
		}

		///<summary>Updates a pref string without using the cache classes.  Useful for multithreaded connections.</summary>
		public static void UpdateStringNoCache(PrefName prefName,string newValue) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),prefName,newValue);
				return;
			}
			string command="UPDATE preference SET ValueString='"+POut.String(newValue)+"' WHERE PrefName='"+POut.String(prefName.ToString())+"'";
			Db.NonQ(command);
		}

		///<summary>Used for prefs that are non-standard.  Especially by outside programmers. Returns true if a change was required, or false if no change needed.</summary>
		public static bool UpdateRaw(string prefName,string newValue) {
			//Very unusual.  Involves cache, so Meth is used further down instead of here at the top.
			string curValue=PrefC.GetRaw(prefName);
			if(curValue==newValue) {
				return false;//no change needed
			}
			string command = "UPDATE preference SET "
				+"ValueString = '"+POut.String(newValue)+"' "
				+"WHERE PrefName = '"+POut.String(prefName)+"'";
			bool retVal=true;
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				retVal=Meth.GetBool(MethodBase.GetCurrentMethod(),prefName,newValue);
			}
			else {
				Db.NonQ(command);
			}
			Pref pref=new Pref();
			pref.PrefName=prefName;
			pref.ValueString=newValue;
			Prefs.UpdateValueForKey(pref);
			return retVal;
		}

		///<summary>Returns true if a change was required, or false if no change needed.</summary>
		public static bool UpdateDateT(PrefName prefName,DateTime newValue) {
			//Very unusual.  Involves cache, so Meth is used further down instead of here at the top.
			DateTime curValue=PrefC.GetDateT(prefName);
			if(curValue==newValue) {
				return false;//no change needed
			}
			string command = "UPDATE preference SET "
				+"ValueString = '"+POut.DateT(newValue,false)+"' "
				+"WHERE PrefName = '"+POut.String(prefName.ToString())+"'";
			bool retVal=true;
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				retVal=Meth.GetBool(MethodBase.GetCurrentMethod(),prefName,newValue);
			}
			else{
				Db.NonQ(command);
			}
			Pref pref=new Pref();
			pref.PrefName=prefName.ToString();
			pref.ValueString=POut.DateT(newValue,false);
			Prefs.UpdateValueForKey(pref);
			return retVal;
		}

		///<summary>Only run from PrefL.CheckMySqlVersion41().</summary>
		public static void ConvertToMySqlVersion41() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			string command="SHOW FULL TABLES WHERE Table_type='BASE TABLE'";//Tables, not views.  Does not work in MySQL 4.1, however we test for MySQL version >= 5.0 in PrefL.
			DataTable table=Db.GetTable(command);//not MySQL 4.1 compatible. Should not be a problem if following reccomended update process.
			string[] tableNames=new string[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++) {
				tableNames[i]=table.Rows[i][0].ToString();
			}
			for(int i=0;i<tableNames.Length;i++) {
				if(tableNames[i]!="procedurecode") {
					command="ALTER TABLE "+tableNames[i]+" CONVERT TO CHARACTER SET utf8";
					Db.NonQ(command);
				}
			}
			string[] commands=new string[]
				{
					//"ALTER TABLE procedurecode CHANGE OldCode OldCode varchar(15) character set utf8 collate utf8_bin NOT NULL"
					//,"ALTER TABLE procedurecode DEFAULT character set utf8"
					"ALTER TABLE procedurecode MODIFY Descript varchar(255) character set utf8 NOT NULL"
					,"ALTER TABLE procedurecode MODIFY AbbrDesc varchar(50) character set utf8 NOT NULL"
					,"ALTER TABLE procedurecode MODIFY ProcTime varchar(24) character set utf8 NOT NULL"
					,"ALTER TABLE procedurecode MODIFY DefaultNote text character set utf8 NOT NULL"
					,"ALTER TABLE procedurecode MODIFY AlternateCode1 varchar(15) character set utf8 NOT NULL"
					//,"ALTER TABLE procedurelog MODIFY OldCode varchar(15) character set utf8 collate utf8_bin NOT NULL"
					//,"ALTER TABLE autocodeitem MODIFY OldCode varchar(15) character set utf8 collate utf8_bin NOT NULL"
					//,"ALTER TABLE procbuttonitem MODIFY OldCode varchar(15) character set utf8 collate utf8_bin NOT NULL"
					//,"ALTER TABLE covspan MODIFY FromCode varchar(15) character set utf8 collate utf8_bin NOT NULL"
					//,"ALTER TABLE covspan MODIFY ToCode varchar(15) character set utf8 collate utf8_bin NOT NULL"
					//,"ALTER TABLE fee MODIFY OldCode varchar(15) character set utf8 collate utf8_bin NOT NULL"
				};
			Db.NonQ(commands);
			//and set the default too
			command="ALTER DATABASE CHARACTER SET utf8";
			Db.NonQ(command);
			command="INSERT INTO preference VALUES('DatabaseConvertedForMySql41','1')";
			Db.NonQ(command);
		}

		///<summary>Gets a Pref object when the PrefName is provided</summary>
		public static Pref GetPref(String PrefName) {
			return Prefs.GetOne(PrefName);
		}

		///<summary>Gets the practice wide default PrefName that corresponds to the passed in sheet type.</summary>
		public static PrefName GetSheetDefPref(SheetTypeEnum sheetType) {
			PrefName retVal=PrefName.SheetsDefaultConsent;
			switch(sheetType) {
				case SheetTypeEnum.Consent:
					retVal=PrefName.SheetsDefaultConsent;
					break;
				case SheetTypeEnum.DepositSlip:
					retVal=PrefName.SheetsDefaultDepositSlip;
					break;
				case SheetTypeEnum.ExamSheet:
					retVal=PrefName.SheetsDefaultExamSheet;
					break;
				case SheetTypeEnum.LabelAppointment:
					retVal=PrefName.SheetsDefaultLabelAppointment;
					break;
				case SheetTypeEnum.LabelCarrier:
					retVal=PrefName.SheetsDefaultLabelCarrier;
					break;
				case SheetTypeEnum.LabelPatient:
					retVal=PrefName.SheetsDefaultLabelPatient;
					break;
				case SheetTypeEnum.LabelReferral:
					retVal=PrefName.SheetsDefaultLabelReferral;
					break;
				case SheetTypeEnum.LabSlip:
					retVal=PrefName.SheetsDefaultLabSlip;
					break;
				case SheetTypeEnum.MedicalHistory:
					retVal=PrefName.SheetsDefaultMedicalHistory;
					break;
				case SheetTypeEnum.MedLabResults:
					retVal=PrefName.SheetsDefaultMedLabResults;
					break;
				case SheetTypeEnum.PatientForm:
					retVal=PrefName.SheetsDefaultPatientForm;
					break;
				case SheetTypeEnum.PatientLetter:
					retVal=PrefName.SheetsDefaultPatientLetter;
					break;
				case SheetTypeEnum.PaymentPlan:
					retVal=PrefName.SheetsDefaultPaymentPlan;
					break;
				case SheetTypeEnum.ReferralLetter:
					retVal=PrefName.SheetsDefaultReferralLetter;
					break;
				case SheetTypeEnum.ReferralSlip:
					retVal=PrefName.SheetsDefaultReferralSlip;
					break;
				case SheetTypeEnum.RoutingSlip:
					retVal=PrefName.SheetsDefaultRoutingSlip;
					break;
				case SheetTypeEnum.Rx:
					retVal=PrefName.SheetsDefaultRx;
					break;
				case SheetTypeEnum.RxMulti:
					retVal=PrefName.SheetsDefaultRxMulti;
					break;
				case SheetTypeEnum.Screening:
					retVal=PrefName.SheetsDefaultScreening;
					break;
				case SheetTypeEnum.Statement:
					retVal=PrefName.SheetsDefaultStatement;
					break;
				case SheetTypeEnum.TreatmentPlan:
					retVal=PrefName.SheetsDefaultTreatmentPlan;
					break;
				default:
					throw new Exception(Lans.g("SheetDefs","Unsupported SheetTypeEnum")+"\r\n"+sheetType.ToString());
			}
			return retVal;
		}
	}

	


	


}










