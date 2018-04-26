using System;
using System.IO;
using OpenDentBusiness;
using Ionic.Zip;

namespace OpenDentBusiness.Eclaims {
	public class EmdeonMedical{
		
		private static string emdeonITSUrlTest="https://cert.its.changehealthcare.com/ITS/itsws.asmx";//test url
		private static string emdeonITSUrl="https://its.changehealthcare.com/ITS/ITSWS.asmx";//production url
		public static string ErrorMessage="";

		///<summary></summary>
		public EmdeonMedical()
		{			
		}

		///<summary>Returns true if the communications were successful, and false if they failed. If they failed, a rollback will happen automatically by deleting the previously created X12 file. The batchnum is supplied for the possible rollback.  Also used for mail retrieval.</summary>
		public static bool Launch(Clearinghouse clearinghouseClin,int batchNum,EnumClaimMedType medType){ //called from Eclaims.cs. Clinic-level clearinghouse passed in.
			string batchFile="";
			try {
				if(!Directory.Exists(clearinghouseClin.ExportPath)) {
					throw new Exception("Clearinghouse export path is invalid.");
				}
				//We make sure to only send the X12 batch file for the current batch, so that if there is a failure, then we know
				//for sure that we need to reverse the batch. This will also help us avoid any exterraneous/old batch files in the
				//same directory which might be left if there is a permission issue when trying to delete the batch files after processing.
				batchFile=Path.Combine(clearinghouseClin.ExportPath,"claims"+batchNum+".txt");
				//byte[] fileBytes=File.ReadAllBytes(batchFile);//unused
				MemoryStream zipMemoryStream=new MemoryStream();
				ZipFile tempZip=new ZipFile();
				tempZip.AddFile(batchFile,"");
				tempZip.Save(zipMemoryStream);
				tempZip.Dispose();
				zipMemoryStream.Position=0;
				byte[] zipFileBytes=zipMemoryStream.GetBuffer();
				string zipFileBytesBase64=Convert.ToBase64String(zipFileBytes);
				zipMemoryStream.Dispose();
				bool isTest=(clearinghouseClin.ISA15=="T");
				string messageType=(isTest?"MCT":"MCD");//medical
				if(medType==EnumClaimMedType.Institutional) {
					messageType=(isTest?"HCT":"HCD");
				}
				else if(medType==EnumClaimMedType.Dental) {
					//messageType=(isTest?"DCT":"DCD");//not used/tested yet, but planned for future.
				}
				EmdeonITS.ITSWS itsws=new EmdeonITS.ITSWS();
				itsws.Url=(isTest?emdeonITSUrlTest:emdeonITSUrl);
				EmdeonITS.ITSReturn response=itsws.PutFileExt(clearinghouseClin.LoginID,clearinghouseClin.Password,messageType,Path.GetFileName(batchFile),zipFileBytesBase64);
				if(response.ErrorCode!=0) { //Batch submission successful.
					throw new Exception("Emdeon rejected all claims in the current batch file "+batchFile+". Error number from Emdeon: "+response.ErrorCode+". Error message from Emdeon: "+response.Response);
				}
			}
			catch(Exception e) {
				ErrorMessage=e.Message;
				x837Controller.Rollback(clearinghouseClin,batchNum);
				return false;
			}
			finally {
				try {
					if(batchFile!="") {
						File.Delete(batchFile);
					}
				}
				catch {
					ErrorMessage="Failed to remove batch file"+batchFile+". Probably due to a permission issue.  Check folder permissions and manually delete.";
				}
			}
			return true;
		}

		public static bool Retrieve(Clearinghouse clearinghouseClin,IODProgressExtended progress=null) {//called from FormClaimReports. clinic-level clearinghouse passed in.
			progress=progress??new ODProgressExtendedNull();
			try {
				if(!Directory.Exists(clearinghouseClin.ResponsePath)) {
					throw new Exception(Lans.g(progress.LanThis,"Clearinghouse response path is invalid."));
				}
				progress.UpdateProgress(Lans.g(progress.LanThis,"Contacting web server"),"reports","17%",17);
				if(progress.IsPauseOrCancel()) {
					progress.UpdateProgress(Lans.g(progress.LanThis,"Canceled by user."));
					return false;
				}
				bool reportsDownloaded=false;
				bool isTest=(clearinghouseClin.ISA15=="T");
				string[] arrayMessageTypes=new string[] 
				{ 
					(isTest?"MCT":"MCD"), //Medical
					(isTest?"HCT":"HCD"), //Institutional
					//(isTest?"DCT":"DCD")  //Dental. Planned for future.
				};
				progress.UpdateProgress(Lans.g(progress.LanThis,"Downloading files"),"reports","33%",33);
				if(progress.IsPauseOrCancel()) {
					progress.UpdateProgress(Lans.g(progress.LanThis,"Canceled by user."));
					return false;
				}
				for(int i=0;i<arrayMessageTypes.Length;i++) {
					float overallpercent=33+(i/arrayMessageTypes.Length)*17;//33 is starting point. 17 is the amount of bar space we have before our next major spot (50%)
					progress.UpdateProgress(Lans.g(progress.LanThis,"Downloading files"),"reports",overallpercent+"%",(int)overallpercent);
					EmdeonITS.ITSWS itsws=new EmdeonITS.ITSWS();
					itsws.Url=(isTest?emdeonITSUrlTest:emdeonITSUrl);
					//Download the most up to date reports, but do not delete them from the server yet.
					EmdeonITS.ITSReturn response=itsws.GetFile(clearinghouseClin.LoginID,clearinghouseClin.Password,arrayMessageTypes[i]+"G");
					if(response.ErrorCode==0) { //Report retrieval successful.
						string reportFileDataBase64=response.Response;
						byte[] reportFileDataBytes=Convert.FromBase64String(reportFileDataBase64);
						string reportFilePath=CodeBase.ODFileUtils.CreateRandomFile(clearinghouseClin.ResponsePath,".zip");
						File.WriteAllBytes(reportFilePath,reportFileDataBytes);
						reportsDownloaded=true;
						//Now that the file has been saved, remove the report file from the Emdeon production server.
						//If deleting the report fails, we don't care because that will simply mean that we download it again next time.
						//Thus we don't need to check the status after this next call.
						progress.UpdateProgress(Lans.g(progress.LanThis,"Removing report file from server"));
						itsws.GetFile(clearinghouseClin.LoginID,clearinghouseClin.Password,arrayMessageTypes[i]+"D");
					}
					else if(response.ErrorCode!=209) { //Report retrieval failure, excluding the error that can be returned when the mailbox is empty.
						throw new Exception(Lans.g(progress.LanThis,"Failed to get reports. Error number from Emdeon:")+" "+response.ErrorCode+". "
							+Lans.g(progress.LanThis,"Error message from Emdeon: ")+response.Response);
					}
				}
				progress.UpdateProgress(Lans.g(progress.LanThis,"Download successful."));
				progress.UpdateProgress(Lans.g(progress.LanThis,"Finalizing"),"reports","50%",50);
				if(progress.IsPauseOrCancel()) {
					progress.UpdateProgress(Lans.g(progress.LanThis,"Canceled by user."));
					return false;
				}
				if(!reportsDownloaded) {
					ErrorMessage=Lans.g(progress.LanThis,"Report mailbox is empty.");
				}
			}
			catch(Exception ex) {
				ErrorMessage=ex.Message; 
				return false;
			}
			return true;
		}

	}
}