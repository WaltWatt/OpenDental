using System;
using System.Diagnostics;
using System.IO;

namespace OpenDentBusiness.Eclaims
{
	///<summary>ClaimX. added by RSM 7/27/11</summary>
	public class ClaimX{
		///<summary></summary>
		public static string ErrorMessage="";
		public ClaimX()
		{
			
		}

		///<summary>Returns true if the communications were successful, and false if they failed.</summary>
		public static bool Launch(Clearinghouse clearinghouseClin,int batchNum) {//called from Eclaims.cs. Clinic-level clearinghouse passed in.
			try{
				//call the client program
				//Process process=
				Process.Start(clearinghouseClin.ClientProgram);
				//process.EnableRaisingEvents=true;
				//process.WaitForExit();
			}
			catch(Exception ex){
				//X12.Rollback(clearhouse,batchNum);//doesn't actually do anything
				ErrorMessage=ex.Message;
				return false;
			}
			return true;
		}


	}
}
