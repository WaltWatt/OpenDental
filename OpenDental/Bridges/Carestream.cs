using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental.Bridges{
	public class Carestream {

		///<summary></summary>
		public Carestream(){
			
		}

		///<summary></summary>
		public static void SendData(Program ProgramCur,Patient pat) {
			string path=Programs.GetProgramPath(ProgramCur);
			if(pat==null) {
				MsgBox.Show("Carestream","Please select a patient first.");
				return;
			}
			string infoFile=ProgramProperties.GetPropVal(ProgramCur.ProgramNum,"Patient.ini path");
			if(infoFile.Length>150) {
				MsgBox.Show("Carestream","Patient.ini file folder path too long.  Must be 150 characters or less.");
				return;
			}
			string id="";
			if(ProgramProperties.GetPropVal(ProgramCur.ProgramNum,"Enter 0 to use PatientNum, or 1 to use ChartNum")=="0") {
				id=pat.PatNum.ToString();
			}
			else {
				id=pat.ChartNumber;
			}
			try {
				using(StreamWriter sw=new StreamWriter(infoFile,false)) {//Create file if does not exist.  Overwrite contents if exists.
					sw.WriteLine("[PATIENT]");
					sw.WriteLine("ID="+Tidy(id,15));
					sw.WriteLine("FIRSTNAME="+Tidy(pat.FName,255));
					if(!string.IsNullOrEmpty(pat.Preferred)) {
						sw.WriteLine("COMMONNAME="+Tidy(pat.Preferred,255));
					}
					sw.WriteLine("LASTNAME="+Tidy(pat.LName,255));
					if(!string.IsNullOrEmpty(pat.MiddleI)) {
						sw.WriteLine("MIDDLENAME="+Tidy(pat.MiddleI,255));
					}
					if(pat.Birthdate.Year>1880) {
						sw.WriteLine("DOB="+pat.Birthdate.ToString("yyyyMMdd"));
					}
					if(pat.Gender==PatientGender.Female) {
						sw.Write("GENDER=F");
					}
					else if(pat.Gender==PatientGender.Male) {
						sw.Write("GENDER=M");
					}
				}
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g("Carestream","Unable to write to file")+": "+infoFile+"\r\n"+Lan.g("Carestream","Error")+": "+ex.Message);
				return;
			}
			try {
				string arguments=@"-I """+infoFile+@"""";
				Process.Start(path,arguments);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		///<summary>Removes semicolons and spaces.</summary>
		private static string Tidy(string input,int maxLength) {
			string retVal=input.Replace(";","");//get rid of any semicolons.
			retVal=retVal.Replace(" ","");//remove spaces
			if(maxLength>0 && retVal.Length>maxLength) {
				retVal=retVal.Substring(0,maxLength);
			}
			return retVal;
		}

	}
}







