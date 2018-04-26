using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Reflection;

namespace OpenDentBusiness {
	public class AutoNoteControls {
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

		private class AutoNoteControlCache : CacheListAbs<AutoNoteControl> {
			protected override List<AutoNoteControl> GetCacheFromDb() {
				string command="SELECT * FROM autonotecontrol ORDER BY Descript";
				return Crud.AutoNoteControlCrud.SelectMany(command);
			}
			protected override List<AutoNoteControl> TableToList(DataTable table) {
				return Crud.AutoNoteControlCrud.TableToList(table);
			}
			protected override AutoNoteControl Copy(AutoNoteControl autoNoteControl) {
				return autoNoteControl.Copy();
			}
			protected override DataTable ListToTable(List<AutoNoteControl> listAutoNoteControls) {
				return Crud.AutoNoteControlCrud.ListToTable(listAutoNoteControls,"AutoNoteControl");
			}
			protected override void FillCacheIfNeeded() {
				AutoNoteControls.GetTableFromCache(false);
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static AutoNoteControlCache _autoNoteControlCache=new AutoNoteControlCache();

		public static List<AutoNoteControl> GetDeepCopy(bool isShort=false) {
			return _autoNoteControlCache.GetDeepCopy(isShort);
		}

		private static AutoNoteControl GetFirstOrDefault(Func<AutoNoteControl,bool> match,bool isShort=false) {
			return _autoNoteControlCache.GetFirstOrDefault(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_autoNoteControlCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_autoNoteControlCache.FillCacheFromTable(table);
				return table;
			}
			return _autoNoteControlCache.GetTableFromCache(doRefreshCache);
		}

		#endregion Cache Pattern

		public static long Insert(AutoNoteControl autoNoteControl) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				autoNoteControl.AutoNoteControlNum=Meth.GetLong(MethodBase.GetCurrentMethod(),autoNoteControl);
				return autoNoteControl.AutoNoteControlNum;
			}
			return Crud.AutoNoteControlCrud.Insert(autoNoteControl);
		}


		public static void Update(AutoNoteControl autoNoteControl) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),autoNoteControl);
				return;
			}
			Crud.AutoNoteControlCrud.Update(autoNoteControl);
		}

		public static void Delete(long autoNoteControlNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),autoNoteControlNum);
				return;
			}
			//no validation for now.
			string command="DELETE FROM autonotecontrol WHERE AutoNoteControlNum="+POut.Long(autoNoteControlNum);
			Db.NonQ(command);
		}

		///<summary>Will return null if can't match.</summary>
		public static AutoNoteControl GetByDescript(string descript) {
			//No need to check RemotingRole; no call to db.
			return GetFirstOrDefault(x => x.Descript==descript);
		}
	}
}

