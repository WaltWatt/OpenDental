using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental.Bridges{
	/// <summary>Also used by the XDR bridge.</summary>
	public class Dexis{

		/// <summary></summary>
		public Dexis(){
			
		}

		///<summary>Sends data for Patient.Cur to the InfoFile and launches the program.</summary>
		public static void SendData(Program ProgramCur, Patient pat){
			string path=Programs.GetProgramPath(ProgramCur);
			List<ProgramProperty> ForProgram =ProgramProperties.GetForProgram(ProgramCur.ProgramNum);
			ProgramProperty PPCur=ProgramProperties.GetCur(ForProgram,"InfoFile path");
			string infoFile=PPCur.PropertyValue;
			if(infoFile.Trim()=="") {
				infoFile=CodeBase.ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(),"infofile.txt");
			}
			Process process=new Process();
			if(pat!=null) {
				try {
					//patientID can be any string format, max 8 char.
					//There is no validation to ensure that length is 8 char or less.
					PPCur=ProgramProperties.GetCur(ForProgram,"Enter 0 to use PatientNum, or 1 to use ChartNum");
					string id="";
					if(PPCur.PropertyValue=="0") {
						id=pat.PatNum.ToString();
					} else {
						id=pat.ChartNumber;
					}
					//Encoding 1252 was specifically requested by the XDR development team to help with accented characters (ex Canadian customers).
					//On 05/19/2015, a reseller noticed UTF8 encoding in the Dexis bridge caused a similar issue.
					//We decided it was safe to switch Dexis from using UTF8 to code page 1252 because the bridge depends entirely on the bridge ID,
					//not the patient names.  Thus there is no chance of breaking the Dexis bridge by using code page 1252 instead.
					//06/01/2015 A customer tested and confirmed that using the XDR bridge and thus coding page 1252, solved the special characters issue.
					Encoding enc=Encoding.GetEncoding(1252);
					using(StreamWriter sw=new StreamWriter(infoFile,false,enc)) {
						sw.WriteLine(pat.LName+", "+pat.FName
							+"  "+pat.Birthdate.ToShortDateString()
							+"  ("+id+")");
						sw.WriteLine("PN="+id);
						//sw.WriteLine("PN="+pat.PatNum.ToString());
						sw.WriteLine("LN="+pat.LName);
						sw.WriteLine("FN="+pat.FName);
						sw.WriteLine("BD="+pat.Birthdate.ToShortDateString());
						if(pat.Gender==PatientGender.Female)
							sw.WriteLine("SX=F");
						else
							sw.WriteLine("SX=M");
					}
					process.StartInfo=new ProcessStartInfo(path,"\"@"+infoFile+"\"");
					process.Start();
				} catch {
					MessageBox.Show(path+" is not available.");
				}
			}//if patient is loaded
			else {
				try {
					process.StartInfo=new ProcessStartInfo(path);//should start Dexis without bringing up a pt.
					process.Start();
				} catch {
					MessageBox.Show(path+" is not available.");
				}
			}
		}

	}
}










