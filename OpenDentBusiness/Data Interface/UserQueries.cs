using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenDentBusiness{

///<summary></summary>
	public class UserQueries{
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
		///<summary>Splits the given query string on the passed-in split string parameters. 
		///DOES NOT split on the split strings when within single quotes, double quotes, parans, or case/if/concat statements.</summary>
		public static List<string> SplitQuery(string queryStr,bool includeDelimeters=false,params string[] listSplitStrs) {
			List<string> listStrSplit = new List<string>(); //returned list of strings.
			string totalStr = "";
			char quoteMode = '-'; //tracks whether we are currently within quotes.
			Stack<string> stackFuncs = new Stack<string>(); //tracks whether we are currently within a CASE, IF, or CONCAT statement.
			foreach(char c in queryStr) {
				if(quoteMode != '-') {
					if(c == quoteMode) {
						quoteMode='-';
					}
					totalStr+=c;
				}
				else if(stackFuncs.Count > 0) {
					if((totalStr + c).ToLower().EndsWith("case")) {
						stackFuncs.Push("end");
					}
					else if((totalStr + c).ToLower().EndsWith("(")) {
						stackFuncs.Push(")");
					}
					else if((totalStr + c).ToLower().EndsWith(stackFuncs.Peek())) {
						stackFuncs.Pop();
					}
					totalStr+=c;
				}
				else {
					if((totalStr + c).ToLower().EndsWith("case")) {
						stackFuncs.Push("end");
						totalStr+=c;
					}
					else if((totalStr + c).ToLower().EndsWith("(")) {
						stackFuncs.Push(")");
						totalStr+=c;
					}
					else if(listSplitStrs.Contains(c.ToString())) {
						listStrSplit.Add(totalStr);
						totalStr="";
						if(includeDelimeters) {
							totalStr+=c;
						}
					}
					else {
						if(c == '\'' || c =='"') {
							quoteMode = c;
						}
						totalStr+=c;
					}
				}
			}
			listStrSplit.Add(totalStr);
			return listStrSplit;
		}

		///<summary>Returns a string with SQL comments removed.
		///E.g. removes /**/ and -- SQL comments from the query passed in.</summary>
		public static string RemoveSQLComments(string queryText) {
			Regex blockComments = new Regex(@"/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/");
			Regex lineComments = new Regex(@"--.*");
			string retVal = blockComments.Replace(queryText,"");
			retVal = lineComments.Replace(retVal,"");
			return retVal;
		}

		///<summary>Helper method to remove leading and trailing spaces from every element in a list of strings.</summary>
		public static void TrimList(List<string> trimList) {
			for(int i = 0;i < trimList.Count;i++) {
				trimList[i]=trimList[i].Trim();
			}
		}

		///<summary>Takes the passed-in query text and returns a list of SET statements within the query.
		///Pass in the entire query.</summary>
		public static List<string> ParseSetStatements(string queryText) {
			queryText=RemoveSQLComments(queryText);
			List<string> stmts = SplitQuery(queryText,false,";");
			TrimList(stmts);
			stmts.RemoveAll(x => string.IsNullOrEmpty(x));
			return stmts.Where(x => x.ToLower().StartsWith("set ")).ToList();
		}
		#endregion
		
		#region CachePattern

		private class UserQueryCache:CacheListAbs<UserQuery> {
			protected override UserQuery Copy(UserQuery userQuery) {
				return userQuery.Copy();
			}

			protected override void FillCacheIfNeeded() {
				UserQueries.GetTableFromCache(false);
			}

			protected override List<UserQuery> GetCacheFromDb() {
				string command="SELECT * FROM userquery ORDER BY description";
				return Crud.UserQueryCrud.SelectMany(command);
			}

			protected override DataTable ListToTable(List<UserQuery> listUserQueries) {
				return Crud.UserQueryCrud.ListToTable(listUserQueries,"UserQuery");
			}

			protected override List<UserQuery> TableToList(DataTable table) {
				return Crud.UserQueryCrud.TableToList(table);
			}

			protected override bool IsInListShort(UserQuery userQuery) {
				return  userQuery.IsReleased;
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static UserQueryCache _userQueryCache=new UserQueryCache();

		public static List<UserQuery> GetDeepCopy(bool isShort=false) {
			return _userQueryCache.GetDeepCopy(isShort);
		}

		public static List<UserQuery> GetWhere(Predicate<UserQuery> match,bool isShort=false) {
			return _userQueryCache.GetWhere(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_userQueryCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_userQueryCache.FillCacheFromTable(table);
				return table;
			}
			return _userQueryCache.GetTableFromCache(doRefreshCache);
		}

		#endregion

		///<summary></summary>
		public static long Insert(UserQuery Cur){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Cur.QueryNum=Meth.GetLong(MethodBase.GetCurrentMethod(),Cur);
				return Cur.QueryNum;
			}
			return Crud.UserQueryCrud.Insert(Cur);
		}
		
		///<summary></summary>
		public static void Delete(UserQuery Cur){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),Cur);
				return;
			}
			string command = "DELETE from userquery WHERE querynum = '"+POut.Long(Cur.QueryNum)+"'";
			Db.NonQ(command);
		}

		///<summary></summary>
		public static void Update(UserQuery Cur){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),Cur);
				return;
			}
			Crud.UserQueryCrud.Update(Cur);
		}
	}

	

	
}













