using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace OpenDentBusiness {
	///<summary></summary>
	public class PatFields {
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

		///<summary>Gets a list of all PatFields for a given patient.</summary>
		public static PatField[] Refresh(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<PatField[]>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM patfield WHERE PatNum="+POut.Long(patNum);
			return Crud.PatFieldCrud.SelectMany(command).ToArray();
		}

		///<summary>Get all PatFields for the given fieldName which belong to patients who have a corresponding entry in the RegistrationKey table. DO NOT REMOVE! Used by OD WebApps solution.</summary>
		public static List<PatField> GetPatFieldsWithRegKeys(string fieldName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PatField>>(MethodBase.GetCurrentMethod(),fieldName);
			}
			string command="SELECT * FROM patfield WHERE FieldName='"+POut.String(fieldName)+"' AND PatNum IN (SELECT PatNum FROM registrationkey)";
			return Crud.PatFieldCrud.SelectMany(command);
		}

		///<summary></summary>
		public static void Update(PatField patField) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patField);
				return;
			}
			Crud.PatFieldCrud.Update(patField);
		}

		///<summary></summary>
		public static long Insert(PatField patField) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				patField.SecUserNumEntry=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				patField.PatFieldNum=Meth.GetLong(MethodBase.GetCurrentMethod(),patField);
				return patField.PatFieldNum;
			}
			return Crud.PatFieldCrud.Insert(patField);
		}

		///<summary></summary>
		public static void Delete(PatField pf) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),pf);
				return;
			}
			string command="DELETE FROM patfield WHERE PatFieldNum ="+POut.Long(pf.PatFieldNum);
			Db.NonQ(command);
		}

		///<summary>Frequently returns null.</summary>
		public static PatField GetByName(string name,PatField[] fieldList){
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<fieldList.Length;i++){
				if(fieldList[i].FieldName==name){
					return fieldList[i];
				}
			}
			return null;
		}

		///<summary>A helper method to make a security log entry for deletion.  Because we have several patient field edit windows, this will allow us to change them all at once.</summary>
		public static void MakeDeleteLogEntry(PatField patField) {
			SecurityLogs.MakeLogEntry(Permissions.PatientFieldEdit,patField.PatNum,"Deleted patient field "+patField.FieldName+".  Value before deletion: \""+patField.FieldValue+"\"");
		}

		///<summary>A helper method to make a security log entry for an edit.  Because we have several patient field edit windows, this will allow us to change them all at once.</summary>
		public static void MakeEditLogEntry(PatField patFieldOld,PatField patFieldCur) {
			SecurityLogs.MakeLogEntry(Permissions.PatientFieldEdit,patFieldCur.PatNum
					,"Edited patient field "+patFieldCur.FieldName+"\r\n"
					+"Old value"+": \""+patFieldOld.FieldValue+"\"  New value: \""+patFieldCur.FieldValue+"\"");
		}

	}

		



		
	

	

	


}










