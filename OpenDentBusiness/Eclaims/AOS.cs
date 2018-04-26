using System;
using System.Diagnostics;
using OpenDentBusiness;

namespace OpenDentBusiness.Eclaims
{
	/// <summary>
	/// Summary description for AOS. added by SPK 7/13/05
	/// </summary>
	public class AOS{
		///<summary></summary>
		public static string ErrorMessage="";
		public AOS()
		{
			
		}

		///<summary>Returns true if the communications were successful, and false if they failed.</summary>
		public static bool Launch(Clearinghouse clearinghouseClin,int batchNum){ //called from Eclaims.cs. Clinic-level clearinghouse passed in.
			try{
				//call the client program
				//Process process=
				Process.Start(clearinghouseClin.ClientProgram);
				//process.EnableRaisingEvents=true;
				//process.WaitForExit();
			}
			catch(Exception ex){
				//X12.Rollback(clearinghouseClin,batchNum);//doesn't actually do anything
				ErrorMessage=ex.Message;
				return false;
			}
			return true;
		}


	}
}
