using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace OpenDentBusiness{
	///<summary></summary>
	public class WikiListHists {
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


		///<summary>Ordered by dateTimeSaved.  Case insensitive.</summary>
		public static List<WikiListHist> GetByName(string listName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<WikiListHist>>(MethodBase.GetCurrentMethod(),listName);
			}
			string command="SELECT * FROM wikilisthist WHERE ListName = '"+POut.String(listName)+"' ORDER BY DateTimeSaved";
			return Crud.WikiListHistCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(WikiListHist wikiListHist) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				wikiListHist.WikiListHistNum=Meth.GetLong(MethodBase.GetCurrentMethod(),wikiListHist);
				return wikiListHist.WikiListHistNum;
			}
			return Crud.WikiListHistCrud.Insert(wikiListHist);
		}

		///<summary>Does not save to DB. Return null if listName does not exist.
		///Pass in the userod.UserNum of the user that is making the change.  Typically Security.CurUser.UserNum.
		///Security.CurUser cannot be used within this method due to the server side of middle tier.</summary>
		public static WikiListHist GenerateFromName(string listName,long userNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<WikiListHist>(MethodBase.GetCurrentMethod(),listName,userNum);
			}
			if(!WikiLists.CheckExists(listName)) {
				return null;
			}
			WikiListHist retVal=new WikiListHist();
			retVal.UserNum=userNum;
			retVal.ListName=listName;
			retVal.DateTimeSaved=DateTime.Now;
			DataTable table=WikiLists.GetByName(listName);
			table.TableName=listName;//required for xmlwriter
			using(var writer = new System.IO.StringWriter()) {
				table.WriteXml(writer);
				retVal.ListContent=writer.ToString();
			}
			List<WikiListHeaderWidth> listHeaders=WikiListHeaderWidths.GetForList(listName);
			StringBuilder sb=new StringBuilder();
			for(int i=0;i<listHeaders.Count;i++) {
				if(i>0) {
					sb.Append(";");
				}
				sb.Append(listHeaders[i].ColName+","+listHeaders[i].ColWidth);
			}
			retVal.ListHeaders=sb.ToString();
			using(var writer=new StringWriter()) {
				table.WriteXml(writer,XmlWriteMode.WriteSchema);
				retVal.ListContent=writer.ToString();
			}
			return retVal;
		}

		///<summary>Drops table in DB.  Recreates Table, then fills with Data.
		///Pass in the userod.UserNum of the user that is making the change.  Typically Security.CurUser.UserNum.</summary>
		public static void RevertFrom(WikiListHist wikiListHist,long userNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),wikiListHist,userNum);
				return;
			}
			if(!WikiLists.CheckExists(wikiListHist.ListName)) {
				return;//should never happen.
			}
			DataTable tableRevertedContent=new DataTable();
			using(XmlReader xmlReader=XmlReader.Create(new StringReader(wikiListHist.ListContent))){
				tableRevertedContent.ReadXml(xmlReader);
			}
			//Begin the process of deleting old table and creating new one.
			//Save current wiki list content to the history
			WikiListHists.Insert(GenerateFromName(wikiListHist.ListName,userNum));
			//Delete current wiki list
			WikiLists.DeleteList(wikiListHist.ListName);
			//Create a new empty wiki list, except for the PK column
			WikiLists.CreateNewWikiList(wikiListHist.ListName);
			//Load header data
			List<WikiListHeaderWidth> listHeaders=WikiListHeaderWidths.GetFromListHist(wikiListHist);
			//Add one column per header, skipping the Pk column
			for(int i=1;i<listHeaders.Count;i++) {//skip the first one, because the first one is the primary key
				WikiLists.AddColumn(listHeaders[i].ListName);
			}
			//Update db with current names and current widths of columns in the WikiListHeaderWidth table to the reverted values
			WikiListHeaderWidths.UpdateNamesAndWidths(wikiListHist.ListName,listHeaders);
			//Fill db table with the reverted values.
			for(int i=0;i<tableRevertedContent.Rows.Count;i++) {
				DataRow row=tableRevertedContent.Rows[i];
				row[0]=WikiLists.AddItem(wikiListHist.ListName);//setPK and add new row
				DataTable tableOneItem=tableRevertedContent.Clone();
				tableOneItem.Rows.Add(tableOneItem.NewRow());
				tableOneItem.Rows[0].ItemArray=row.ItemArray;
				WikiLists.UpdateItem(wikiListHist.ListName,tableOneItem);
			}
		}

		///<summary>Checks remoting roles. Does not check permissions. Does not check for existing listname. If listname already exists it will "merge" the history.</summary>
		public static void Rename(string WikiListCurName,string WikiListNewName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),WikiListCurName,WikiListNewName);
				return;
			}
			string command="UPDATE wikilisthist SET ListName = '"+POut.String(WikiListNewName)+"' WHERE ListName='"+POut.String(WikiListCurName)+"'";
			Db.NonQ(command);
		}
	}
}