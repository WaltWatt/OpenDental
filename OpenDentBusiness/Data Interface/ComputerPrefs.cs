using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Net;
using System.Reflection;
using CodeBase;

namespace OpenDentBusiness {
	public class ComputerPrefs {
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

		///<summary>Only used by the client.</summary>
		private static ComputerPref _localComputer;

		public static ComputerPref LocalComputer{
			get {
				if(_localComputer==null) {
					_localComputer=GetForLocalComputer();
				}
				return _localComputer;
			}
			//set {
				//I don't think this gets used.
			//}
		}

		///<summary>Returns the computer preferences for the computer which this instance of Open Dental is running on.</summary>
		private static ComputerPref GetForLocalComputer(){
			return GetForComputer(Dns.GetHostName());
		}

		///<summary>Returns the computer preferences for the computer with the passed in computer name.</summary>
		public static ComputerPref GetForComputer(string computerName){
			//No need to check RemotingRole; no call to db.
			ComputerPref computerPref=new ComputerPref();
			//OpenGL tooth chart not supported on Unix systems.
			if(Environment.OSVersion.Platform==PlatformID.Unix) {
				computerPref.GraphicsSimple=DrawingMode.Simple2D;
			}
			//Default sensor values to start
			computerPref.SensorType="D";
			computerPref.SensorPort=0;
			computerPref.SensorExposure=1;
			computerPref.SensorBinned=false;
			//Default values to start
			computerPref.AtoZpath="";
			computerPref.TaskKeepListHidden=false; //show docked task list on this computer 
			computerPref.TaskDock=0; //bottom
			computerPref.TaskX=900;
			computerPref.TaskY=625;
			computerPref.ComputerName=computerName;
			computerPref.DirectXFormat="";
			computerPref.ScanDocSelectSource=false;
			computerPref.ScanDocShowOptions=false;
			computerPref.ScanDocDuplex=false;
			computerPref.ScanDocGrayscale=false;
			computerPref.ScanDocResolution=150;//default suggested in FormImagingSetup
			computerPref.ScanDocQuality=40;//default suggested in FormImagingSetup
			computerPref.GraphicsSimple=DrawingMode.DirectX;
			computerPref.NoShowLanguage=false;
			DataTable table=GetPrefsForComputer(computerName);
			if(table==null){
				//In case of database error, just use default graphics settings so that it is possible for the program to start.
				return computerPref;
			}
			if(table.Rows.Count==0){//Computer pref row does not yet exist for this computer.
				Insert(computerPref);//Create default prefs for the specified computer. Also sets primary key in our computerPref object.
				return computerPref;
			}
			if(table.Rows.Count>1){
				//Should never happen (would only happen if the primary key showed up more than once).
				Logger.openlog.LogMB("Corrupt computerpref table in database. The computer name '"
					+POut.String(computerName)+"' is a primary key in multiple records. Please run the "
					+"database maintenance tool, then call us for help if you still get this message.",
					Logger.Severity.WARNING);
			}
			computerPref=Crud.ComputerPrefCrud.TableToList(table)[0];
			return computerPref;
		}

		///<summary>Should not be called by external classes.</summary>
		public static DataTable GetPrefsForComputer(string computerName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),computerName);
			}
			string command="SELECT * FROM computerpref WHERE ComputerName='"+POut.String(computerName)+"'";
			try {
				return Db.GetTable(command);
			} 
			catch {
				return null;
			}
		}

		///<summary>Should not be called by external classes.</summary>
		public static long Insert(ComputerPref computerPref) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				computerPref.ComputerPrefNum=Meth.GetLong(MethodBase.GetCurrentMethod(),computerPref);
				return computerPref.ComputerPrefNum;
			}
			return Crud.ComputerPrefCrud.Insert(computerPref);
		}

		///<summary>Any time this is called, ComputerPrefs.LocalComputer will be passed in.  It will have already been changed for local use, and this saves it for next time.</summary>
		public static void Update(ComputerPref computerPref) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),computerPref);
				return;
			}
			Crud.ComputerPrefCrud.Update(computerPref);
		}

		///<summary>Sets the GraphicsSimple column to 1.  Added to fix machines (lately tablets) that are having graphics problems and cannot start OpenDental.</summary>
		public static void SetToSimpleGraphics(string computerName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),computerName);
				return;
			}
			string command="UPDATE computerpref SET GraphicsSimple=1 WHERE ComputerName='"+POut.String(computerName)+"'";
			Db.NonQ(command);
		}

		///<summary>Updates the local computerpref's ComputerOS if it is different than what is stored.</summary>
		public static void UpdateLocalComputerOS() {
			//No need to check RemotingRole; no call to db.
			string platformIdAsString=POut.String(Environment.OSVersion.Platform.ToString());
			if(LocalComputer.ComputerOS.ToString()==platformIdAsString) {//We only want to update this column if there is indeed a difference between values.
				return;
			}
			UpdateComputerOS(platformIdAsString,ComputerPrefs.LocalComputer.ComputerPrefNum);
			_localComputer=null;//Setting to null will trigger LocalComputer to update from DB next time it is accessed.
		}

		///<summary>Updates the ComputerOS for the computerPrefNum passed in.</summary>
		public static void UpdateComputerOS(string platformId,long computerPrefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),platformId,computerPrefNum);
				return;
			}
			//We have to use a query and not the normal Update(computerPref) method because the 
			//Environment.OSVersion.Platform enum is different than the computerpref.ComputerOS enum.
			string command="UPDATE computerpref SET ComputerOS = '"+platformId+"' "
				+"WHERE ComputerPrefNum = "+POut.Long(computerPrefNum);
			Db.NonQ(command);
		}

	}

	
}
