using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	public class WebFormL {
		public static string SynchUrlStaging="https://10.10.1.196/WebHostSynch/Sheets.asmx";
		public static string SynchUrlDev="http://localhost:2923/Sheets.asmx";
		
		public static void IgnoreCertificateErrors() {
			ServicePointManager.ServerCertificateValidationCallback+=
			delegate (object sender,X509Certificate certificate,X509Chain chain,System.Net.Security.SslPolicyErrors sslPolicyErrors) {
				return true;//accept any certificate if debugging
			};
		}
		
		public static void LoadImagesToSheetDef(SheetDef sheetDefCur) {
			for(int j=0;j<sheetDefCur.SheetFieldDefs.Count;j++) {
				try {
					if(sheetDefCur.SheetFieldDefs[j].FieldType==SheetFieldType.Image) {
						string filePathAndName=ODFileUtils.CombinePaths(SheetUtil.GetImagePath(),sheetDefCur.SheetFieldDefs[j].FieldName);
						Image img=null;
						ImageFormat imgFormat=null;
						if(sheetDefCur.SheetFieldDefs[j].ImageField!=null) {//The image has already been downloaded.
							img=new Bitmap(sheetDefCur.SheetFieldDefs[j].ImageField);
							imgFormat=ImageFormat.Bmp;
						}
						else if(sheetDefCur.SheetFieldDefs[j].FieldName=="Patient Info.gif") {
							img=OpenDentBusiness.Properties.Resources.Patient_Info;
							imgFormat=img.RawFormat;
						}
						else if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ && File.Exists(filePathAndName)) {
							img=Image.FromFile(filePathAndName);
							imgFormat=img.RawFormat;
						}
						else if(CloudStorage.IsCloudStorage) {
							OpenDentalCloud.Core.TaskStateDownload state=CloudStorage.Download(SheetUtil.GetImagePath(),sheetDefCur.SheetFieldDefs[j].FieldName);
							if(state==null || state.FileContent==null) {
								throw new Exception(Lan.g(CloudStorage.LanThis,"Unable to download image."));
							}
							else {
								using(MemoryStream stream=new MemoryStream(state.FileContent)) {
									img=new Bitmap(Image.FromStream(stream));
								}
								imgFormat=ImageFormat.Bmp;
							}
						}
						//sheetDefCur.SheetFieldDefs[j].ImageData=POut.Bitmap(new Bitmap(img),ImageFormat.Png);//Because that's what we did before. Review this later. 
						long fileByteSize=0;
						using(MemoryStream ms=new MemoryStream()) {
							img.Save(ms,imgFormat); // done solely to compute the file size of the image
							fileByteSize=ms.Length;
						}
						if(fileByteSize>2000000) {
							//for large images greater that ~2MB use jpeg format for compression. Large images in the 4MB + range have difficulty being displayed. It could be an issue with MYSQL or ASP.NET
							sheetDefCur.SheetFieldDefs[j].ImageData=POut.Bitmap(new Bitmap(img),ImageFormat.Jpeg);
						}
						else {
							sheetDefCur.SheetFieldDefs[j].ImageData=POut.Bitmap(new Bitmap(img),imgFormat);
						}
					}
					else {
						sheetDefCur.SheetFieldDefs[j].ImageData="";// because null is not allowed
					}
				}
				catch(Exception ex) {
					sheetDefCur.SheetFieldDefs[j].ImageData="";
					MessageBox.Show(ex.Message);
				}
			}
		}

		///<summary>Returns true if the sheet has a FName, LName, and Birthdate field.</summary>
		public static bool VerifyRequiredFieldsPresent(SheetDef sheetDef) {
			bool hasFName=false;
			bool hasLName=false;
			bool hasBirthdate=false;
			for(int j=0;j<sheetDef.SheetFieldDefs.Count;j++) {
				if(sheetDef.SheetFieldDefs[j].FieldType!=SheetFieldType.InputField) {
					continue;
				}
				if(sheetDef.SheetFieldDefs[j].FieldName.ToLower().In("fname","firstname")) {
					hasFName=true;
				}
				else if(sheetDef.SheetFieldDefs[j].FieldName.ToLower().In("lname","lastname")) {
					hasLName=true;
				}
				else if(sheetDef.SheetFieldDefs[j].FieldName.ToLower().In("bdate","birthdate")) {
					hasBirthdate=true;
				}
			}
			if(!hasFName || !hasLName || !hasBirthdate) {
				MessageBox.Show(Lan.g("WebForms","The sheet called")+" \""+sheetDef.Description+"\" "
					+Lan.g("WebForms","does not contain all three required fields: LName, FName, and Birthdate."));
				return false;
			}
			return true;
		}

	}

}
