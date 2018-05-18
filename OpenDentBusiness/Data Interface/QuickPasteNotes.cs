using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;

namespace OpenDentBusiness{
	///<summary></summary>
	public class QuickPasteNotes {
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

		private class QuickPasteNoteCache : CacheListAbs<QuickPasteNote> {
			protected override List<QuickPasteNote> GetCacheFromDb() {
				string command=
					"SELECT * from quickpastenote "
					+"ORDER BY ItemOrder";
				return Crud.QuickPasteNoteCrud.SelectMany(command);
			}
			protected override List<QuickPasteNote> TableToList(DataTable table) {
				return Crud.QuickPasteNoteCrud.TableToList(table);
			}
			protected override QuickPasteNote Copy(QuickPasteNote QuickPasteNote) {
				return QuickPasteNote.Copy();
			}
			protected override DataTable ListToTable(List<QuickPasteNote> listQuickPasteNotes) {
				return Crud.QuickPasteNoteCrud.ListToTable(listQuickPasteNotes,"QuickPasteNote");
			}
			protected override void FillCacheIfNeeded() {
				QuickPasteNotes.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static QuickPasteNoteCache _QuickPasteNoteCache=new QuickPasteNoteCache();

		public static List<QuickPasteNote> GetDeepCopy(bool isShort=false) {
			return _QuickPasteNoteCache.GetDeepCopy(isShort);
		}

		public static List<QuickPasteNote> GetWhere(Predicate<QuickPasteNote> match,bool isShort=false) {
			return _QuickPasteNoteCache.GetWhere(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_QuickPasteNoteCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_QuickPasteNoteCache.FillCacheFromTable(table);
				return table;
			}
			return _QuickPasteNoteCache.GetTableFromCache(doRefreshCache);
		}

		#endregion

		///<summary></summary>
		public static long Insert(QuickPasteNote note) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				note.QuickPasteNoteNum=Meth.GetLong(MethodBase.GetCurrentMethod(),note);
				return note.QuickPasteNoteNum;
			}
			return Crud.QuickPasteNoteCrud.Insert(note);
		}

		///<summary></summary>
		public static void Update(QuickPasteNote note){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),note);
				return;
			}
			Crud.QuickPasteNoteCrud.Update(note);
		}
		
		///<summary></summary>
		public static void Delete(QuickPasteNote note){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),note);
				return;
			}
			string command="DELETE from quickpastenote WHERE QuickPasteNoteNum = '"
				+POut.Long(note.QuickPasteNoteNum)+"'";
 			Db.NonQ(command);
		}

		///<summary>When saving an abbrev, this makes sure that the abbreviation is not already in use.</summary>
		public static bool AbbrAlreadyInUse(QuickPasteNote note){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),note);
			}
			string command="SELECT * FROM quickpastenote WHERE "
				+"Abbreviation='"+POut.String(note.Abbreviation)+"' "
				+"AND QuickPasteNoteNum != '"+POut.Long (note.QuickPasteNoteNum)+"'";
 			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0){
				return false;
			}
			return true;
		}

		///<summary>Only used from FormQuickPaste to get all notes for the selected cat.</summary>
		public static QuickPasteNote[] GetForCat(long cat) {
			//No need to check RemotingRole; no call to db.
			return GetWhere(x => x.QuickPasteCatNum==cat).ToArray();
		}

		///<summary>Called on KeyUp from various textBoxes in the program to look for a ?abbrev and attempt to substitute.  Substitutes the text if found.</summary>
		public static string Substitute(string text,QuickPasteType type) {
			//No need to check RemotingRole; no call to db.
			int typeIndex=QuickPasteCats.GetDefaultType(type);
			if(typeIndex < 0) {
				return text;
			}
			QuickPasteCat quickPasteCatDefault=QuickPasteCats.GetDeepCopy()[typeIndex];
			List<QuickPasteNote> listQuickPasteCats=GetDeepCopy();
			for(int i=0;i<listQuickPasteCats.Count;i++) {
				if(listQuickPasteCats[i].Abbreviation=="") {
					continue;
				}
				if(listQuickPasteCats[i].QuickPasteCatNum!=quickPasteCatDefault.QuickPasteCatNum) {
					continue;
				}
				//We have to replace all $ chars with $$ because Regex.Replace allows "Substitutions" in the replacement parameter.
				//The replacement parameter specifies the string that is to replace each match in input. replacement can consist of any combination of literal
				//text and substitutions. For example, the replacement pattern a*${test}b inserts the string "a*" followed by the substring that is matched by
				//the test capturing group, if any, followed by the string "b". 
				//The * character is not recognized as a metacharacter within a replacement pattern.
				//See https://msdn.microsoft.com/en-us/library/taz3ak2f(v=vs.110).aspx for more information.
				string quicknote = listQuickPasteCats[i].Note.Replace("$","$$");
				//Techs were complaining about quick notes replacing text that was pasted into text boxes (e.g. when a URL happens to have ?... that matches a quick note abbr).
				//The easiest way to deal with this is to not allow the regular expression to replace strings that have a non-whitespace character before or after the abbr.
				//The regex of '...(?<!\S)...' is utilizing an optional space via a lookbehind and visa versa with '...(?!\S)...' as a lookahead.
				var pattern = @"(?<spaceBefore>(?<!\S))\?"+Regex.Escape(listQuickPasteCats[i].Abbreviation)+@"(?<spaceAfter>(?!\S))";
				var replacePattern = "${spaceBefore}"+(quicknote)+"${spaceAfter}";
				text=Regex.Replace(text,pattern,replacePattern,RegexOptions.None);
			}
			return text;
		}

		///<summary>This should not be passing in two lists. Consider rewriting to only pass in one list and an identifier to get list from DB.</summary>
		public static bool Sync(List<QuickPasteNote> listNew,List<QuickPasteNote> listOld) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),listNew,listOld);
			}
			//Eventually we may want to change this to not be passing in listOld.
			return Crud.QuickPasteNoteCrud.Sync(listNew.Select(x=>x.Copy()).ToList(),listOld.Select(x=>x.Copy()).ToList());
		}


		


	}

	


}









