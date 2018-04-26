using CodeBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	///<summary></summary>
	public class FeatureRequests {

		///<Summary>Checks requestIDs in list for incompletes. Returns false if incomplete exists.</Summary>
		public static bool CheckForCompletion(List<long> listRequestIDs) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),listRequestIDs);
			}
			//Create an ODThread so that we can safely change the database connection settings without affecting the calling method's connection.
			ODThread odThread=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				//Always set the thread static database connection variables to set the serviceshq db conn.
#if DEBUG
				new DataConnection().SetDbT("localhost","bugs","root","","","",DatabaseType.MySql,true);
#else
				new DataConnection().SetDbT("server","bugs","root","","","",DatabaseType.MySql,true);
#endif
				string command="SELECT COUNT(*) FROM request "
					+"WHERE Approval!=9 "//9=Complete
					+"AND RequestId IN ("+String.Join(",",listRequestIDs)+")";
				o.Tag=Db.GetCount(command);
			}));
			odThread.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception e) => { }));//Do nothing
			odThread.Name="featureCheckForCompletionThread";
			odThread.Start(true);
			if(!odThread.Join(2000)) { //Give this thread up to 2 seconds to complete.
				return true;
			}
			if(PIn.Int(odThread.Tag.ToString())!=0) {
				return false;
			}
			return true;
		}		
		
		///<Summary>Will not mark completed feature requests as in progress.</Summary>
		public static void MarkAsInProgress(List<long> listRequestIDs) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listRequestIDs);
				return;
			}
			//Create an ODThread so that we can safely change the database connection settings without affecting the calling method's connection.
			ODThread odThread=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				//Always set the thread static database connection variables to set the serviceshq db conn.
#if DEBUG
				new DataConnection().SetDbT("localhost","bugs","root","","","",DatabaseType.MySql,true);
#else
				new DataConnection().SetDbT("server","bugs","root","","","",DatabaseType.MySql,true);
#endif
				string command="UPDATE request SET Approval=8 "//InProgress
					+"WHERE Approval!=9 "//9=Complete
					+"AND RequestId IN ("+String.Join(",",listRequestIDs)+")";
				o.Tag=Db.NonQ(command);
			}));
			odThread.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception e) => { }));//Do nothing
			odThread.Name="featureMarkAsInProgressThread";
			odThread.Start(true);
			if(!odThread.Join(2000)) { //Give this thread up to 2 seconds to complete.
				return;
			}
		}		
		
		///<Summary>Will not mark completed feature requests as approved.</Summary>
		public static void MarkAsApproved(List<long> listRequestIDs) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listRequestIDs);
				return;
			}
			//Create an ODThread so that we can safely change the database connection settings without affecting the calling method's connection.
			ODThread odThread=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				//Always set the thread static database connection variables to set the serviceshq db conn.
#if DEBUG
				new DataConnection().SetDbT("localhost","bugs","root","","","",DatabaseType.MySql,true);
#else
				new DataConnection().SetDbT("server","bugs","root","","","",DatabaseType.MySql,true);
#endif
				string command="UPDATE request SET Approval=7 "//Approved
					+"WHERE Approval!=9 "//9=Complete
					+"AND RequestId IN ("+String.Join(",",listRequestIDs)+")";
				o.Tag=Db.NonQ(command);
			}));
			odThread.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception e) => { }));//Do nothing
			odThread.Name="featureMarkAsInProgressThread";
			odThread.Start(true);
			if(!odThread.Join(2000)) { //Give this thread up to 2 seconds to complete.
				return;
			}
		}
		
		///<Summary></Summary>
		public static void CompleteRequests(List<long> listRequestIDs,string versionText) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listRequestIDs,versionText);
			}
			//Create an ODThread so that we can safely change the database connection settings without affecting the calling method's connection.
			ODThread odThread=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				//Always set the thread static database connection variables to set the serviceshq db conn.
#if DEBUG
				new DataConnection().SetDbT("localhost","bugs","root","","","",DatabaseType.MySql,true);
#else
				new DataConnection().SetDbT("server","bugs","root","","","",DatabaseType.MySql,true);
#endif
				string versionString="\r\n\r\nCompleted in version "+versionText;
				string command="UPDATE request SET Approval=9, "//Complete
					+"Detail=CONCAT( Detail , '"+versionString+"' ) "
					+"WHERE RequestId IN ("+String.Join(",",listRequestIDs)+")";
				o.Tag=Db.NonQ(command);
			}));
			odThread.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception e) => { }));//Do nothing
			odThread.Name="featureCheckForCompletionThread";
			odThread.Start(true);
			if(!odThread.Join(2000)) { //Give this thread up to 2 seconds to complete.
				return;
			}
		}

		public static List<FeatureRequest> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<FeatureRequest>>(MethodBase.GetCurrentMethod());
			}
			//Create an ODThread so that we can safely change the database connection settings without affecting the calling method's connection.
			ODThread odThread=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				//Always set the thread static database connection variables to set the serviceshq db conn.
#if DEBUG
				new DataConnection().SetDbT("localhost","bugs","root","","","",DatabaseType.MySql,true);
#else
				new DataConnection().SetDbT("server","bugs","root","","","",DatabaseType.MySql,true);
#endif
				#region WebServiceCustomerUpdates.FeatureRequestGetList
				string command="SELECT request.RequestId,Approval,Description,Difficulty,"
					+"vote.AmountPledged,IFNULL(vote.Points,0) AS points,vote.IsCritical,request.PatNum,"
					+"TotalCritical,TotalPledged,TotalPoints,Weight "
					+"FROM request "
					+"LEFT JOIN vote ON vote.PatNum=1486 AND vote.RequestId=request.RequestId "
					+"WHERE (Approval="+POut.Int((int)ApprovalEnum.New)+" "
					+"OR Approval="+POut.Int((int)ApprovalEnum.Approved)+" "
					+"OR Approval="+POut.Int((int)ApprovalEnum.InProgress)+" "
					+"OR Approval="+POut.Int((int)ApprovalEnum.Complete)+") "
					+"ORDER BY Approval, Weight DESC, points DESC";
				DataTable raw=Db.GetTable(command);
				DataRow row;
				DataTable table=new DataTable();
				table.TableName="Table";
				table.Columns.Add("approval");
				table.Columns.Add("Description");
				table.Columns.Add("Difficulty");
				table.Columns.Add("isMine");
				table.Columns.Add("myVotes");
				table.Columns.Add("RequestId");
				table.Columns.Add("totalVotes");
				table.Columns.Add("Weight");
				table.Columns.Add("personalVotes");
				table.Columns.Add("personalCrit");
				table.Columns.Add("personalPledged");
				double myPledge;
				bool myCritical;
				double totalPledged;
				int totalCritical;
				for(int i=0;i<raw.Rows.Count;i++){
					row=table.NewRow();
					row["RequestId"]=raw.Rows[i]["RequestId"].ToString();
					//myVotes,myCritical,myPledge------------------------------------------------------
					row["myVotes"]=raw.Rows[i]["Points"].ToString();
					row["personalVotes"]=raw.Rows[i]["Points"].ToString();
					if(row["myVotes"].ToString()=="0"){
						row["myVotes"]="";
					}
					myCritical=PIn.Bool(raw.Rows[i]["IsCritical"].ToString());
					if(myCritical==true){
						row["personalCrit"]="1";
						if(row["myVotes"].ToString()!=""){
							row["myVotes"]+="\r\n";
						}
						row["myVotes"]+="Critical";
					}
					else {
						row["personalCrit"]="0";
					}
					myPledge=PIn.Double(raw.Rows[i]["AmountPledged"].ToString());
					if(myPledge!=0){
						if(row["myVotes"].ToString()!=""){
							row["myVotes"]+="\r\n";
						}
						row["myVotes"]+=myPledge.ToString("c0");
						row["personalPledged"]=myPledge.ToString();
					}
					else {
						row["personalPledged"]="0";
					}
					//TotalPoints,TotalCritical,TotalPledged-----------------------------------------------
					row["totalVotes"]=raw.Rows[i]["TotalPoints"].ToString();
					if(row["totalVotes"].ToString()=="0"){
						row["totalVotes"]="";
					}
					totalCritical=PIn.Int(raw.Rows[i]["TotalCritical"].ToString());
					if(totalCritical!=0){
						if(row["totalVotes"].ToString()!=""){
							row["totalVotes"]+="\r\n";
						}
						row["totalVotes"]+="Critical:"+totalCritical.ToString();
					}
					totalPledged=PIn.Double(raw.Rows[i]["TotalPledged"].ToString());
					if(totalPledged!=0){
						if(row["totalVotes"].ToString()!=""){
							row["totalVotes"]+="\r\n";
						}
						row["totalVotes"]+=totalPledged.ToString("c0");
					}
					//end
					row["approval"]=((ApprovalEnum)PIn.Int(raw.Rows[i]["Approval"].ToString())).ToString();
					if(raw.Rows[i]["PatNum"].ToString()=="1486") {
						row["isMine"]="X";
					}
					else{
						row["isMine"]="";
					}
					row["Difficulty"]=raw.Rows[i]["Difficulty"].ToString();
					row["Description"]=raw.Rows[i]["Description"].ToString();
					row["Weight"]=raw.Rows[i]["Weight"].ToString();
					table.Rows.Add(row);
				}
				o.Tag=table;
				#endregion
			}));
			odThread.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception e) => { }));//Do nothing
			odThread.Name="featureMarkAsInProgressThread";
			odThread.Start(true);
			if(!odThread.Join(2000)) { //Give this thread up to 2 seconds to complete.
				return new List<FeatureRequest>();
			}
			DataTable tableRequests=(DataTable)odThread.Tag;
			List<FeatureRequest> listFeatureRequests=new List<FeatureRequest>();
			foreach(DataRow dataRow in tableRequests.Rows) {
				FeatureRequest req=new FeatureRequest();
				#region Convert DataTable Into FeatureRequest
				long.TryParse(dataRow["RequestId"].ToString(),out req.FeatReqNum);
				string[] votes=dataRow["totalVotes"].ToString().Split(new string[] { "\r\n" },StringSplitOptions.RemoveEmptyEntries);
				string vote=votes.FirstOrDefault(x => !x.StartsWith("Critical") && !x.StartsWith("$"));
				if(!string.IsNullOrEmpty(vote)) {
					long.TryParse(vote,out req.Votes);
				}
				vote=votes.FirstOrDefault(x => x.StartsWith("Critical"));
				if(!string.IsNullOrEmpty(vote)) {
					long.TryParse(vote,out req.Critical);
				}
				vote=votes.FirstOrDefault(x => x.StartsWith("$"));
				if(!string.IsNullOrEmpty(vote)) {
					float.TryParse(vote,out req.Pledge);
				}
				req.Difficulty=PIn.Long(dataRow["Difficulty"].ToString());
				req.Weight=PIn.Float(dataRow["Weight"].ToString());
				req.Approval=dataRow["Weight"].ToString();
				req.Description=dataRow["Description"].ToString();
				#endregion
				listFeatureRequests.Add(req);
			}
			return listFeatureRequests;
		}

		///<summary>Inneficient. Use sparingly. Can be improved.</summary>
		public static List<FeatureRequest> GetMany(List<long> listFeatureRequestNums) {
			//No need for remoting role check; no db call.
			return GetAll().FindAll(x => listFeatureRequestNums.Contains(x.FeatReqNum));
		}

	}
}