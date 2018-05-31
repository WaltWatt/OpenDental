using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CodeBase;

namespace OpenDentBusiness {
	public class ApptEdit {

		///<summary>Gets the data necesary to load FormApptEdit.</summary>
		public static LoadData GetLoadData(Appointment AptCur,bool IsNew) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<LoadData>(MethodBase.GetCurrentMethod(),AptCur,IsNew);
			}
			LoadData data=new LoadData();
			data.ListProcsForAppt=Procedures.GetProcsForApptEdit(AptCur);
			data.ListAppointments=Appointments.GetAppointmentsForProcs(data.ListProcsForAppt);
			data.Family=Patients.GetFamily(AptCur.PatNum);
			data.ListPatPlans=PatPlans.Refresh(AptCur.PatNum);
			data.ListInsSubs=InsSubs.RefreshForFam(data.Family);
			data.ListBenefits=Benefits.Refresh(data.ListPatPlans,data.ListInsSubs);
			data.ListInsPlans=InsPlans.RefreshForSubList(data.ListInsSubs);
			data.TableApptFields=Appointments.GetApptFields(AptCur.AptNum);
			data.TableComms=Appointments.GetCommTable(AptCur.PatNum.ToString(),AptCur.AptNum);
			data.Lab=(IsNew ? null : LabCases.GetForApt(AptCur));
			data.PatientTable=Appointments.GetPatTable(AptCur.PatNum.ToString());
			if(!PrefC.GetBool(PrefName.EasyHideDentalSchools)) {
				data.ListStudents=ReqStudents.GetForAppt(AptCur.AptNum);
			}
			return data;
		}

		///<summary>Adds procedures to the appointment.</summary>
		///<returns>First item of tuple is the newly added procedures. Second item is all procedures for the appointment.</returns>
		public static ODTuple<List<Procedure>,List<Procedure>> QuickAddProcs(Appointment apt,Patient pat,List<string> listCodesToAdd,long provNum,
			long provHyg,List<InsSub> SubList,List<InsPlan> listInsPlans,List<PatPlan> listPatPlans,List<Benefit> listBenefits) 
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<ODTuple<List<Procedure>,List<Procedure>>>(MethodBase.GetCurrentMethod(),apt,pat,listCodesToAdd,provNum,
					provHyg,SubList,listInsPlans,listPatPlans,listBenefits);
			}
			Procedures.SetDateFirstVisit(apt.AptDateTime.Date,1,pat);
			List<ClaimProc> ClaimProcList=ClaimProcs.Refresh(apt.PatNum);
			List<Procedure> listAddedProcs=new List<Procedure>();
			foreach(string code in listCodesToAdd) {
				Procedure proc=new Procedure();
				proc.PatNum=apt.PatNum;
				ProcedureCode procCodeCur=ProcedureCodes.GetProcCode(code);
				proc.CodeNum=procCodeCur.CodeNum;
				proc.ProcDate=apt.AptDateTime.Date;
				proc.DateTP=DateTime.Today;
				#region ProvNum
				proc.ProvNum=provNum;
				if(procCodeCur.ProvNumDefault!=0) {//Override provider for procedures with a default provider
					//This provider might be restricted to a different clinic than this user.
					proc.ProvNum=procCodeCur.ProvNumDefault;
				}
				else if(procCodeCur.IsHygiene && provHyg!=0) {
					proc.ProvNum=provHyg;
				}
				#endregion ProvNum
				proc.ClinicNum=apt.ClinicNum;
				proc.MedicalCode=procCodeCur.MedicalCode;
				proc.ProcFee=Procedures.GetProcFee(pat,listPatPlans,SubList,listInsPlans,proc.CodeNum,proc.ProvNum,proc.ClinicNum,proc.MedicalCode);
				proc.ProcStatus=ProcStat.TP;
				proc.SiteNum=pat.SiteNum;
				proc.RevCode=procCodeCur.RevenueCodeDefault;
				proc.BaseUnits=procCodeCur.BaseUnits;
				proc.DiagnosticCode=PrefC.GetString(PrefName.ICD9DefaultForNewProcs);
				proc.PlaceService=(PlaceOfService)PrefC.GetInt(PrefName.DefaultProcedurePlaceService);//Default Proc Place of Service for the Practice is used.
				if(Userods.IsUserCpoe(Security.CurUser)) {
					//This procedure is considered CPOE because the provider is the one that has added it.
					proc.IsCpoe=true;
				}
				proc.Note=ProcCodeNotes.GetNote(proc.ProvNum,proc.CodeNum,proc.ProcStatus);
				Procedures.Insert(proc);//recall synch not required
				Procedures.ComputeEstimates(proc,pat.PatNum,ClaimProcList,false,listInsPlans,listPatPlans,listBenefits,pat.Age,SubList);
				listAddedProcs.Add(proc);
			}
			return new ODTuple<List<Procedure>,List<Procedure>>(listAddedProcs,Procedures.GetProcsForApptEdit(apt));
		}

		///<summary>The data necesary to load FormApptEdit.</summary>
		[Serializable]
		public class LoadData {
			public List<Procedure> ListProcsForAppt;
			public List<Appointment> ListAppointments;
			public Family Family;
			public List<PatPlan> ListPatPlans;
			public List<Benefit> ListBenefits;
			public List<InsSub> ListInsSubs;
			public List<InsPlan> ListInsPlans;
			[XmlIgnore]
			public DataTable TableApptFields;
			[XmlIgnore]
			public DataTable TableComms;
			public LabCase Lab;
			[XmlIgnore]
			public DataTable PatientTable;
			public List<ReqStudent> ListStudents;


			[XmlElement(nameof(TableApptFields))]
			public string TableApptFieldsXml {
				get {
					if(TableApptFields==null) {
						return null;
					}
					return XmlConverter.TableToXml(TableApptFields);
				}
				set {
					if(value==null) {
						TableApptFields=null;
						return;
					}
					TableApptFields=XmlConverter.XmlToTable(value);
				}
			}

			[XmlElement(nameof(TableComms))]
			public string TableCommsXml {
				get {
					if(TableComms==null) {
						return null;
					}
					return XmlConverter.TableToXml(TableComms);
				}
				set {
					if(value==null) {
						TableComms=null;
						return;
					}
					TableComms=XmlConverter.XmlToTable(value);
				}
			}

			[XmlElement(nameof(PatientTable))]
			public string PatientTableXml {
				get {
					if(PatientTable==null) {
						return null;
					}
					return XmlConverter.TableToXml(PatientTable);
				}
				set {
					if(value==null) {
						PatientTable=null;
						return;
					}
					PatientTable=XmlConverter.XmlToTable(value);
				}
			}

		}
	}
}
