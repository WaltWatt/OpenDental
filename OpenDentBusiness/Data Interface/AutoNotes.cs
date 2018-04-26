using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;

namespace OpenDentBusiness {
	public class AutoNotes {
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

		#region Cache Pattern

		private class AutoNoteCache : CacheListAbs<AutoNote> {
			protected override List<AutoNote> GetCacheFromDb() {
				string command="SELECT * FROM autonote ORDER BY AutoNoteName";
				return Crud.AutoNoteCrud.SelectMany(command);
			}
			protected override List<AutoNote> TableToList(DataTable table) {
				return Crud.AutoNoteCrud.TableToList(table);
			}
			protected override AutoNote Copy(AutoNote autoNote) {
				return autoNote.Copy();
			}
			protected override DataTable ListToTable(List<AutoNote> listAutoNotes) {
				return Crud.AutoNoteCrud.ListToTable(listAutoNotes,"AutoNote");
			}
			protected override void FillCacheIfNeeded() {
				AutoNotes.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static AutoNoteCache _autoNoteCache=new AutoNoteCache();

		public static List<AutoNote> GetDeepCopy(bool isShort=false) {
			return _autoNoteCache.GetDeepCopy(isShort);
		}

		public static List<AutoNote> GetWhere(Predicate<AutoNote> match,bool isShort=false) {
			return _autoNoteCache.GetWhere(match,isShort);
		}

		public static bool GetExists(Predicate<AutoNote> match,bool isShort=false) {
			return _autoNoteCache.GetExists(match,isShort);
		}

		private static AutoNote GetFirstOrDefault(Func<AutoNote,bool> match,bool isShort=false) {
			return _autoNoteCache.GetFirstOrDefault(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_autoNoteCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_autoNoteCache.FillCacheFromTable(table);
				return table;
			}
			return _autoNoteCache.GetTableFromCache(doRefreshCache);
		}

		#endregion Cache Pattern

		///<summary></summary>
		public static long Insert(AutoNote autonote) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				autonote.AutoNoteNum=Meth.GetLong(MethodBase.GetCurrentMethod(),autonote);
				return autonote.AutoNoteNum;
			}
			return Crud.AutoNoteCrud.Insert(autonote);
		}

		///<summary></summary>
		public static void Update(AutoNote autonote) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),autonote);
				return;
			}
			Crud.AutoNoteCrud.Update(autonote);
		}

		///<summary></summary>
		public static void Delete(long autoNoteNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),autoNoteNum);
				return;
			}
			string command="DELETE FROM autonote "
				+"WHERE AutoNoteNum = "+POut.Long(autoNoteNum);
			Db.NonQ(command);
		}

		public static string GetByTitle(string autoNoteTitle) {
			//No need to check RemotingRole; no call to db.
			AutoNote autoNote=GetFirstOrDefault(x => x.AutoNoteName==autoNoteTitle);
			return (autoNote==null ? "" : autoNote.MainText);
		}

		///<summary>Sets the autonote.Category=0 for the autonote category DefNum provided.  Returns the number of rows updated.</summary>
		public static long RemoveFromCategory(long autoNoteCatDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),autoNoteCatDefNum);
			}
			string command="UPDATE autonote SET Category=0 WHERE Category="+POut.Long(autoNoteCatDefNum);
			return Db.NonQ(command);
		}
	}
}
