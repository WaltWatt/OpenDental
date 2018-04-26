using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SheetFieldDefs{
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

		private class SheetFieldDefCache : CacheListAbs<SheetFieldDef> {
			protected override List<SheetFieldDef> GetCacheFromDb() {
				string command="SELECT * FROM sheetfielddef ORDER BY SheetDefNum";
				return Crud.SheetFieldDefCrud.SelectMany(command);
			}
			protected override List<SheetFieldDef> TableToList(DataTable table) {
				return Crud.SheetFieldDefCrud.TableToList(table);
			}
			protected override SheetFieldDef Copy(SheetFieldDef sheetFieldDef) {
				return sheetFieldDef.Copy();
			}
			protected override DataTable ListToTable(List<SheetFieldDef> listSheetFieldDefs) {
				return Crud.SheetFieldDefCrud.ListToTable(listSheetFieldDefs,"SheetFieldDef");
			}
			protected override void FillCacheIfNeeded() {
				SheetFieldDefs.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static SheetFieldDefCache _sheetFieldDefCache=new SheetFieldDefCache();

		public static List<SheetFieldDef> GetWhere(Predicate<SheetFieldDef> match,bool isShort=false) {
			return _sheetFieldDefCache.GetWhere(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_sheetFieldDefCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_sheetFieldDefCache.FillCacheFromTable(table);
				return table;
			}
			return _sheetFieldDefCache.GetTableFromCache(doRefreshCache);
		}

		#endregion

		///<summary>Gets all internal SheetFieldDefs from the database for a specific sheet, used in FormSheetFieldExam.</summary>
		public static List<SheetFieldDef> GetForExamSheet(long sheetDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SheetFieldDef>>(MethodBase.GetCurrentMethod(),sheetDefNum);
			}
			string command="SELECT * FROM sheetfielddef WHERE SheetDefNum="+POut.Long(sheetDefNum)+" "
				+"AND ((FieldName!='misc' AND FieldName!='') OR (ReportableName!='')) "
				+"GROUP BY FieldName,ReportableName";
			return Crud.SheetFieldDefCrud.SelectMany(command);
		}

		///<summary>Gets all SheetFieldDefs from the database for a specific sheet, used in FormSheetFieldExam.</summary>
		public static List<SheetFieldDef> GetForSheetDef(long sheetDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SheetFieldDef>>(MethodBase.GetCurrentMethod(),sheetDefNum);
			}
			string command="SELECT * FROM sheetfielddef WHERE SheetDefNum="+POut.Long(sheetDefNum);
			return Crud.SheetFieldDefCrud.SelectMany(command);
		}

		///<Summary>Gets one SheetFieldDef from the database.</Summary>
		public static SheetFieldDef CreateObject(long sheetFieldDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<SheetFieldDef>(MethodBase.GetCurrentMethod(),sheetFieldDefNum);
			}
			return Crud.SheetFieldDefCrud.SelectOne(sheetFieldDefNum);
		}

		///<summary></summary>
		public static long Insert(SheetFieldDef sheetFieldDef) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				sheetFieldDef.SheetFieldDefNum=Meth.GetLong(MethodBase.GetCurrentMethod(),sheetFieldDef);
				return sheetFieldDef.SheetFieldDefNum;
			}
			return Crud.SheetFieldDefCrud.Insert(sheetFieldDef);
		}

		///<summary></summary>
		public static void Update(SheetFieldDef sheetFieldDef) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),sheetFieldDef);
				return;
			}
			Crud.SheetFieldDefCrud.Update(sheetFieldDef);
		}

		///<summary></summary>
		public static void Delete(long sheetFieldDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),sheetFieldDefNum);
				return;
			}
			Crud.SheetFieldDefCrud.Delete(sheetFieldDefNum);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list. Must always pass in sheetDefNum.
		///This function uses a DB comparison rather than a stale list because we are not worried about concurrency of a single sheet and enhancing the
		///functions that call this would take a lot of restructuring.</summary>
		public static void Sync(List<SheetFieldDef> listNew,long sheetDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listNew,sheetDefNum);//never pass DB list through the web service
				return;
			}
			List<SheetFieldDef> listDB=SheetFieldDefs.GetForSheetDef(sheetDefNum);
			Crud.SheetFieldDefCrud.Sync(listNew,listDB);
		}


		///<summary>Sorts fields in the order that they shoudl be drawn on top of eachother. First Images, then Drawings, Lines, Rectangles, Text, Check Boxes, and SigBoxes. In that order.</summary>
		public static int SortDrawingOrderLayers(SheetFieldDef f1,SheetFieldDef f2) {
			if(f1.FieldType!=f2.FieldType) {
				return SheetFields.FieldTypeSortOrder(f1.FieldType).CompareTo(SheetFields.FieldTypeSortOrder(f2.FieldType));
			}
			return f1.YPos.CompareTo(f2.YPos);
			//return f1.SheetFieldNum.CompareTo(f2.SheetFieldNum);
		}
	}
}