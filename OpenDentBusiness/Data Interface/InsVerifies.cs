using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using CodeBase;

namespace OpenDentBusiness{
	///<summary></summary>
	public class InsVerifies {
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
		
		///<summary>Calculates the time available to verify based on a variety of patient and appointment factors.  
		///Looks at Appointment date, appointment creation date, last verification date, next scheduled verification, insurance renewal
		///</summary>
		public static InsVerify SetTimeAvailableForVerify(InsVerify insVer,PlanToVerify planType,int appointmentScheduledDays,int insBenefitEligibilityDays,
			int patientEnrollmentDays) 
		{
			//DateAppointmentScheduled-DateAppointmentCreated
			DateTime dateTimeLatestApptScheduling;
			//DateAppointmentScheduled-appointmentScheduledDays (default is 7)
			DateTime dateTimeUntilAppt;
			//DateTime the appointment takes place.
			DateTime dateTimeAppointment;
			//DateTime the patient was scheduled to be re-verified
			DateTime dateTimeScheduledReVerify;
			//DateTime verification last took place.  Will be 01/01/0001 if verification has never happened.
			DateTime dateTimeLastVerified;
			//DateTime the insurance renewal month rolls over.
			//The month the renewal takes place is stored in the database.  If the month is 0, then it is actually january. 
			//It is the first day of the given month at midnight, or (Month#)/01/(year) @ 00:00AM. 
			//Set to max val by default in case the PrefName.InsVerifyFutureDateBenefitYear=false.
			//Since max val is later in time than the appointment time, it will be ignored.
			DateTime dateBenifitRenewalNeeded=DateTime.MaxValue;
			#region Appointment Dates
			dateTimeAppointment=insVer.AppointmentDateTime;
			dateTimeUntilAppt=insVer.AppointmentDateTime.AddDays(-appointmentScheduledDays);
			//Calculate when the appointment was put into it's current time slot.
			//This will be the earliest datetime where the scheduled appointment time is what it is now
			List<HistAppointment> listHistAppt=HistAppointments.GetForApt(insVer.AptNum);
			listHistAppt.RemoveAll(x => x.AptDateTime.Date!=insVer.AppointmentDateTime.Date);
			listHistAppt=listHistAppt.Where(x => x.AptStatus==ApptStatus.Scheduled).OrderBy(x => x.AptDateTime).ToList();
			if(listHistAppt.Count>0) {
				//If the appointment was moved to the current date after the (Apt.DateTime-appointmentScheduledDays),
				//we only had (Apt.DateTime-listHistAppt.First().HistDateTstamp) days instead of (appointmentScheduledDays)
				dateTimeLatestApptScheduling=listHistAppt.First().HistDateTStamp;
			}
			else { 
				//Just in case there's no history for an appointment for some reason.
				//Shouldn't happen because a log entry is created when the appointment is created.
				//Use the date the appointment was created.  This is better than nothing and should never happen anyways.
				dateTimeLatestApptScheduling=Appointments.GetOneApt(insVer.AptNum).SecDateEntry;
			}
			#endregion Appointment Dates
			#region Insurance Verification
			dateTimeLastVerified=insVer.DateLastVerified;
			//Add defined number of days to date last verified to calculate when the next verification date should have started.
			if(planType==PlanToVerify.InsuranceBenefits) {
				if(insVer.DateLastVerified==DateTime.MinValue) {//If it's the min value, the insurance has never been verified.
					dateTimeScheduledReVerify=insVer.DateTimeEntry;
				}
				else {
					dateTimeScheduledReVerify=insVer.DateLastVerified.AddDays(insBenefitEligibilityDays);
				}
			}
			else {//PlanToVerify.PatientEligibility
				if(insVer.DateLastVerified==DateTime.MinValue) {
					dateTimeScheduledReVerify=insVer.DateTimeEntry;
				}
				else {
					dateTimeScheduledReVerify=insVer.DateLastVerified.AddDays(patientEnrollmentDays);
				}
			}
			#endregion insurance verification
			#region Benifit Renewal
			if(PrefC.GetBool(PrefName.InsVerifyFutureDateBenefitYear)) {
				InsPlan insPlan=InsPlans.GetPlan(insVer.PlanNum,null);
				//Setup the month renew dates.  Need all 3 years in case the appointment verify window crosses over a year
				//e.g. Appt verify date: 12/30/2016 and Appt Date: 1/6/2017
				DateTime dateTimeOldestRenewal=new DateTime(DateTime.Now.Year-1,Math.Max((byte)1,insPlan.MonthRenew),1);
				DateTime dateTimeMiddleRenewal=new DateTime(DateTime.Now.Year,Math.Max((byte)1,insPlan.MonthRenew),1);
				DateTime dateTimeNewestRenewal=new DateTime(DateTime.Now.Year+1,Math.Max((byte)1,insPlan.MonthRenew),1);
				//We want to find the date closest to the appointment date without going past it.
				if(dateTimeMiddleRenewal>dateTimeAppointment) {
					dateBenifitRenewalNeeded=dateTimeOldestRenewal;
				}
				else {
					if(dateTimeNewestRenewal>dateTimeAppointment) {
						dateBenifitRenewalNeeded=dateTimeMiddleRenewal;
					}
					else {
						dateBenifitRenewalNeeded=dateTimeNewestRenewal;
					}
				}
			}
			#endregion Benifit Renewal
			DateTime dateTimeAbleToVerify=VerifyDateCalulation(dateTimeUntilAppt,dateTimeLatestApptScheduling,dateTimeScheduledReVerify,dateBenifitRenewalNeeded);
			insVer.HoursAvailableForVerification=insVer.AppointmentDateTime.Subtract(dateTimeAbleToVerify).TotalHours;
			return insVer;
		}
		
		///<summary>This calculates the datetime when a patient would appear on the "Needs verification" list.  </summary>
		///<param name="dateTimeDaysUntilAppt">The date and time to begin considering appointments for verification purposes.  
		///Calculated DateTime from AptDateTime - PrefName.InsVerifyDaysFromPastDueAppt</param>
		///<param name="dateTimeApptLastScheduled">The date and time of the most recent move of the appointment to the schedule.</param>
		///<param name="dateTimeVerificationExpired">The date and time that the verification has become invalid.
		///Calculated DateTime from DateTimeLastVerify + (PrefName.InsVerifyBenefitEligibilityDays or PrefName.InsVerifyPatientEnrollmentDays)</param>
		///<param name="dateBenefitRenewalNeeded">The date that the insurance benefit year expires and needs to be renewed regardless of the DateTimeLastVerify.</param>
		public static DateTime VerifyDateCalulation(DateTime dateTimeDaysUntilAppt,DateTime dateTimeApptLastScheduled,DateTime dateTimeVerificationExpired,DateTime dateBenefitRenewalNeeded) {
			//The date and time that the insurance verification has expired.  If it expired due to a benefit renewal, the time portion will assume midnight.
			DateTime dateTimeVerificationFirstNeeded=new DateTime(Math.Min(dateTimeVerificationExpired.Ticks, dateBenefitRenewalNeeded.Ticks));
			//The date and time that the patient associated to the patient was put on the verification list (this would happen for either plan or benefit insverify types)
			//To show on the verification list, an appointment must be made, and a verification must have expired.
			//Because of this, we get the most recent requirement.  This ensures the exact moment both requirements were present.
			DateTime dateTimeShowInVerificationList=new DateTime(Math.Max(dateTimeApptLastScheduled.Ticks, dateTimeVerificationFirstNeeded.Ticks));
			//The final requirement to show on the verification list is that the appointment needs to be X days away or sooner.
			//Because of this, we compare the X days and the exact date and time that the appointment requirements were met, and take the most recent one, since both need to be met.
			return new DateTime(Math.Max(dateTimeDaysUntilAppt.Ticks, dateTimeShowInVerificationList.Ticks));
		}
		#endregion


		///<summary>Gets one InsVerify from the db.</summary>
		public static InsVerify GetOne(long insVerifyNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<InsVerify>(MethodBase.GetCurrentMethod(),insVerifyNum);
			}
			return Crud.InsVerifyCrud.SelectOne(insVerifyNum);
		}
		
		///<summary>Gets one InsVerify from the db that has the given fkey and verify type.</summary>
		public static InsVerify GetOneByFKey(long fkey,VerifyTypes verifyType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<InsVerify>(MethodBase.GetCurrentMethod(),fkey,verifyType);
			}
			string command="SELECT * FROM insverify WHERE FKey="+POut.Long(fkey)+" AND VerifyType="+POut.Int((int)verifyType)+"";
			return Crud.InsVerifyCrud.SelectOne(command);
		}
		
		///<summary>Gets one InsVerifyNum from the db that has the given fkey and verify type.</summary>
		public static long GetInsVerifyNumByFKey(long fkey,VerifyTypes verifyType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),fkey,verifyType);
			}
			string command="SELECT * FROM insverify WHERE FKey="+POut.Long(fkey)+" AND VerifyType="+POut.Int((int)verifyType)+"";
			InsVerify insVerify=Crud.InsVerifyCrud.SelectOne(command);
			if(insVerify==null) {
				return 0;
			}
			return insVerify.InsVerifyNum;
		}

		///<summary></summary>
		public static long Insert(InsVerify insVerify) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				insVerify.InsVerifyNum=Meth.GetLong(MethodBase.GetCurrentMethod(),insVerify);
				return insVerify.InsVerifyNum;
			}
			return Crud.InsVerifyCrud.Insert(insVerify);
		}

		///<summary></summary>
		public static void Update(InsVerify insVerify) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),insVerify);
				return;
			}
			Crud.InsVerifyCrud.Update(insVerify);
		}

		///<summary></summary>
		public static void Delete(long insVerifyNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),insVerifyNum);
				return;
			}
			Crud.InsVerifyCrud.Delete(insVerifyNum);
		}
		
		///<summary>Inserts a default InsVerify into the database based on the passed in patplan.  Used when inserting a new patplan.
		///Returns the primary key of the new InsVerify.</summary>
		public static long InsertForPatPlanNum(long patPlanNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),patPlanNum);
			}
			InsVerify insVerify=new InsVerify();
			insVerify.VerifyType=VerifyTypes.PatientEnrollment;
			insVerify.FKey=patPlanNum;
			return Crud.InsVerifyCrud.Insert(insVerify);
		}
		
		///<summary>Inserts a default InsVerify into the database based on the passed in insplan.  Used when inserting a new insplan.
		///Returns the primary key of the new InsVerify.</summary>
		public static long InsertForPlanNum(long planNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),planNum);
			}
			InsVerify insVerify=new InsVerify();
			insVerify.VerifyType=VerifyTypes.InsuranceBenefit;
			insVerify.FKey=planNum;
			return Crud.InsVerifyCrud.Insert(insVerify);
		}
		
		///<summary>Deletes an InsVerify with the passed in FKey and VerifyType.</summary>
		public static void DeleteByFKey(long fkey,VerifyTypes verifyType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),fkey,verifyType);
				return;
			}
			long insVerifyNum=GetInsVerifyNumByFKey(fkey,verifyType);
			Crud.InsVerifyCrud.Delete(insVerifyNum);//Will do nothing if insVerifyNum was 0.
		}

		public static List<InsVerify> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<InsVerify>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM insverify";
			return Crud.InsVerifyCrud.SelectMany(command);
		}

		public static List<long> GetAllInsVerifyUserNums() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT DISTINCT UserNum FROM insverify";
			return Db.GetListLong(command);
		}

		///<summary>UserNum=-1 is "All", UserNum=0 is "Unassigned". 
		///statusDefNum=-1 or statusDefNum=0 is "All".  
		///listClinicNums containing -1 is "All". listClinicNums containing 0 is "Unassigned". 
		///listRegionDefNums containing 0 or -1 is "All".</summary>
		public static List<InsVerifyGridObject> GetVerifyGridList(DateTime startDate, DateTime endDate,DateTime datePatEligibilityLastVerified
			,DateTime datePlanBenefitsLastVerified,List<long> listClinicNums,List<long> listRegionDefNums,long statusDefNum
			,long userNum,string carrierName,bool excludePatVerifyWhenNoIns,bool excludePatClones) 
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<InsVerifyGridObject>>(MethodBase.GetCurrentMethod(),startDate,endDate,datePatEligibilityLastVerified
					,datePlanBenefitsLastVerified,listClinicNums,listRegionDefNums,statusDefNum,userNum,carrierName,excludePatVerifyWhenNoIns,excludePatClones);
			}
			//clinicJoin should only be used if the passed in clinicNum is a value other than 0 (Unassigned).
			string whereClinic="";
			if(listClinicNums.Contains(-1)) {//All clinics
				whereClinic="AND (clinic.IsInsVerifyExcluded=0 OR clinic.ClinicNum IS NULL) ";
				if(!listRegionDefNums.Contains(0) && !listRegionDefNums.Contains(-1) && listRegionDefNums.Count>0) {//Specific region
					whereClinic+=" AND clinic.Region IN("+string.Join(",",listRegionDefNums.Select(x => POut.Long(x)))+") ";
				}
			}
			else if(listClinicNums.Contains(0)) {//Unassigned clinics
				whereClinic="AND clinic.ClinicNum IS NULL ";
				if(listClinicNums.Count(x => x!=0)>0) {//Also has specific clinics selected
					whereClinic="AND (clinic.ClinicNum IS NULL OR ";
					whereClinic+="(clinic.IsInsVerifyExcluded=0 AND clinic.ClinicNum IN("+string.Join(",",listClinicNums.Select(x => POut.Long(x)))+") ";
					if(!listRegionDefNums.Contains(0) && !listRegionDefNums.Contains(-1) && listRegionDefNums.Count>0) {//Specific region
						whereClinic+=" AND clinic.Region IN("+string.Join(",",listRegionDefNums.Select(x => POut.Long(x)))+") ";
					}
					whereClinic+=")) ";
				}
			}
			else {//Specific Clinic
				whereClinic="AND clinic.IsInsVerifyExcluded=0 AND clinic.ClinicNum IN("+string.Join(",",listClinicNums.Select(x => POut.Long(x)))+") ";
				if(!listRegionDefNums.Contains(0) && !listRegionDefNums.Contains(-1) && listRegionDefNums.Count>0) {//Specific region
					whereClinic+=" AND clinic.Region IN("+string.Join(",",listRegionDefNums.Select(x => POut.Long(x)))+") ";
				}
			}
			bool checkBenefitYear=PrefC.GetBool(PrefName.InsVerifyFutureDateBenefitYear);
			string mainQuery=@"
				SELECT insverify.*,
				patient.LName,patient.FName,patient.Preferred,appointment.PatNum,appointment.AptNum,appointment.AptDateTime,patplan.PatPlanNum,insplan.PlanNum,carrier.CarrierName,
				COALESCE(clinic.Abbr,'None') AS ClinicName
				FROM appointment 
				LEFT JOIN clinic ON clinic.ClinicNum=appointment.ClinicNum 
				INNER JOIN patient ON patient.PatNum=appointment.PatNum 
				INNER JOIN patplan ON patplan.PatNum=appointment.PatNum 
				INNER JOIN inssub ON inssub.InsSubNum=patplan.InsSubNum 
				INNER JOIN insplan ON insplan.PlanNum=inssub.PlanNum 
					"+(excludePatVerifyWhenNoIns ? "AND insplan.HideFromVerifyList=0" : "")+@"
				INNER JOIN carrier ON carrier.CarrierNum=insplan.CarrierNum 
					"+(string.IsNullOrEmpty(carrierName) ? "" : "AND carrier.CarrierName LIKE '%"+POut.String(carrierName)+"%'")+@"
				"+(excludePatClones ? "LEFT JOIN patientlink ON patientlink.PatNumTo=patient.PatNum AND patientlink.LinkType="
					+POut.Int((int)PatientLinkType.Clone)+" " : "");
			string insVerifyJoin1=@"INNER JOIN insverify ON 
					(insverify.VerifyType="+POut.Int((int)VerifyTypes.InsuranceBenefit)+@" 
					AND insverify.FKey=insplan.PlanNum 
					AND (insverify.DateLastVerified<"+POut.Date(datePlanBenefitsLastVerified)+@"
						"+(checkBenefitYear?@"OR (insverify.DateLastVerified<DATE_FORMAT(appointment.AptDateTime,CONCAT('%Y-',LPAD(MonthRenew,2,'0'),'-01')) 
							AND DATE_FORMAT(appointment.AptDateTime,CONCAT('%Y-',LPAD(MonthRenew,2,'0'),'-01'))<="+DbHelper.DtimeToDate("appointment.AptDateTime")+")":"")+@") 
					"+(excludePatVerifyWhenNoIns ? "" : "AND insplan.HideFromVerifyList=0")+@") ";
			string insVerifyJoin2=@"INNER JOIN insverify ON 
					(insverify.VerifyType="+POut.Int((int)VerifyTypes.PatientEnrollment)+@"
					AND insverify.FKey=patplan.PatPlanNum
					AND (insverify.DateLastVerified<"+POut.Date(datePatEligibilityLastVerified)+@"
						"+(checkBenefitYear?@"OR (insverify.DateLastVerified<DATE_FORMAT(appointment.AptDateTime,CONCAT('%Y-',LPAD(MonthRenew,2,'0'),'-01')) 
							AND DATE_FORMAT(appointment.AptDateTime,CONCAT('%Y-',LPAD(MonthRenew,2,'0'),'-01'))<="+DbHelper.DtimeToDate("appointment.AptDateTime")+")":"")+@"))	";
			string whereClause=@"
				WHERE appointment.AptDateTime BETWEEN "+DbHelper.DtimeToDate(POut.Date(startDate))+" AND "+DbHelper.DtimeToDate(POut.Date(endDate.AddDays(1)))+@" 
				AND appointment.AptStatus IN ("+POut.Int((int)ApptStatus.Scheduled)+","+POut.Int((int)ApptStatus.Complete)+@")
				"+(userNum==-1 ? "" : "AND insverify.UserNum="+POut.Long(userNum))+@"
				"+(statusDefNum<1 ? "" : "AND insverify.DefNum="+POut.Long(statusDefNum))+@"
				"+(excludePatClones ? "AND patientlink.PatNumTo IS NULL" : "")+@"
				"+whereClinic;
			//Previously we joined the insverify table using a large OR clause. This caused MySQL to not be able to use any index on the insverify table.
			//Now we run two unioned queries, each with a different join clause for the insverify table, so that MySQL can use insverify.FKKey as an index.
			string command=
				mainQuery+
				insVerifyJoin1+
				whereClause+@"
				UNION ALL
				"+
				mainQuery+
				insVerifyJoin2+
				whereClause+@"
				ORDER BY AptDateTime";
			DataTable table=Db.GetTable(command);
			List<InsVerify> listInsVerifies=Crud.InsVerifyCrud.TableToList(table);
			List<InsVerifyGridObject> retVal=new List<InsVerifyGridObject>();
			for(int i=0;i<table.Rows.Count;i++) {
				DataRow row=table.Rows[i];
				InsVerify insVerifyCur=listInsVerifies[i].Clone();
				insVerifyCur.PatNum=PIn.Long(row["PatNum"].ToString());
				insVerifyCur.PlanNum=PIn.Long(row["PlanNum"].ToString());
				insVerifyCur.PatPlanNum=PIn.Long(row["PatPlanNum"].ToString());
				insVerifyCur.ClinicName=PIn.String(row["ClinicName"].ToString());
				string patName=PIn.String(row["LName"].ToString())
					+", ";
				if(PIn.String(row["Preferred"].ToString())!="") {
					patName+="'"+PIn.String(row["Preferred"].ToString())+"' ";
				}
				patName+=PIn.String(row["FName"].ToString());
				insVerifyCur.PatientName=patName;
				insVerifyCur.CarrierName=PIn.String(row["CarrierName"].ToString());
				insVerifyCur.AppointmentDateTime=PIn.DateT(row["AptDateTime"].ToString());
				insVerifyCur.AptNum=PIn.Long(row["AptNum"].ToString());
				if(insVerifyCur.VerifyType==VerifyTypes.InsuranceBenefit) {
					InsVerifyGridObject gridObjPlanExists=retVal.FirstOrDefault(x => x.PlanInsVerify!=null && x.PlanInsVerify.PlanNum==insVerifyCur.PlanNum);
					if(gridObjPlanExists==null) {
						InsVerifyGridObject gridObjExists=retVal.FirstOrDefault(x => x.PatInsVerify!=null 
							&& x.PatInsVerify.PatPlanNum==insVerifyCur.PatPlanNum 
							&& x.PatInsVerify.PlanNum==insVerifyCur.PlanNum 
							&& x.PatInsVerify.Note==insVerifyCur.Note 
							&& x.PatInsVerify.DefNum==insVerifyCur.DefNum 
							&& x.PlanInsVerify==null);
						if(gridObjExists!=null) {
							gridObjExists.PlanInsVerify=insVerifyCur;
						}
						else {
							retVal.Add(new InsVerifyGridObject(plan:insVerifyCur));
						}
					}
				}
				else if(insVerifyCur.VerifyType==VerifyTypes.PatientEnrollment) {
					InsVerifyGridObject gridObjPatExists=retVal.FirstOrDefault(x => x.PatInsVerify!=null && x.PatInsVerify.PatPlanNum==insVerifyCur.PatPlanNum);
					if(gridObjPatExists==null) {
						InsVerifyGridObject gridObjExists=retVal.FirstOrDefault(x => x.PlanInsVerify!=null 
						&& x.PlanInsVerify.PlanNum==insVerifyCur.PlanNum 
						&& x.PlanInsVerify.Note==insVerifyCur.Note 
						&& x.PlanInsVerify.DefNum==insVerifyCur.DefNum 
						&& x.PatInsVerify==null);
						if(gridObjExists!=null) {
							gridObjExists.PatInsVerify=insVerifyCur;
						}
						else {
							retVal.Add(new InsVerifyGridObject(pat:insVerifyCur));
						}
					}
				}
			}
			return retVal;
		}

		public static void CleanupInsVerifyRows(DateTime startDate, DateTime endDate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),startDate,endDate);
				return;
			}
			//Nathan OK'd the necessity for a complex update query like this to avoid looping through update statements.  This will be changed to a crud update method sometime in the future.
			string command="";
			List<long> listInsVerifyNums=Db.GetListLong(GetInsVerifyCleanupQuery(startDate,endDate));
			if(listInsVerifyNums.Count==0) {
				return;
			}
			command="UPDATE insverify "
				+"SET insverify.DateLastAssigned='0001-01-01', "
				+"insverify.DefNum=0, "
				+"insverify.Note='', "
				+"insverify.UserNum=0 "
				+"WHERE insverify.InsVerifyNum IN ("+string.Join(",",listInsVerifyNums)+")";
			Db.NonQ(command);
		}

		private static string GetInsVerifyCleanupQuery(DateTime startDate, DateTime endDate) {
			return @"SELECT InsVerifyNum
				FROM (
					SELECT InsVerifyNum,patplan.PatNum
					FROM patplan
					INNER JOIN inssub ON inssub.InsSubNum=patplan.InsSubNum
					INNER JOIN insplan ON insplan.PlanNum=inssub.PlanNum
						AND insplan.HideFromVerifyList=0
					INNER JOIN insverify ON VerifyType="+POut.Int((int)VerifyTypes.InsuranceBenefit)+@"
						AND insverify.FKey=insplan.PlanNum
					WHERE insverify.DateLastAssigned>'0001-01-01'
					AND insverify.DateLastAssigned<"+POut.Date(DateTime.Today.AddDays(-30))+@"
				
					UNION
					
					SELECT InsVerifyNum,patplan.PatNum
					FROM patplan
					INNER JOIN insverify ON VerifyType="+POut.Int((int)VerifyTypes.PatientEnrollment)+@"
						AND insverify.FKey=patplan.PatPlanNum
					WHERE insverify.DateLastAssigned>'0001-01-01'
					AND insverify.DateLastAssigned<"+POut.Date(DateTime.Today.AddDays(-30))+@"
				) insverifies
				LEFT JOIN appointment ON appointment.PatNum=insverifies.PatNum
					AND appointment.AptStatus IN ("+POut.Int((int)ApptStatus.Scheduled)+","+POut.Int((int)ApptStatus.Complete)+@")
					AND "+DbHelper.DtimeToDate("appointment.AptDateTime")+" BETWEEN "+POut.Date(startDate)+" AND "+POut.Date(endDate)+@"
				GROUP BY insverifies.InsVerifyNum
				HAVING MAX(appointment.AptNum) IS NULL";
		}
		
		public enum PlanToVerify {
			PatientEligibility,
			InsuranceBenefits
		}

		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern

		private class InsVerifyCache : CacheListAbs<InsVerify> {
			protected override List<InsVerify> GetCacheFromDb() {
				string command="SELECT * FROM InsVerify ORDER BY ItemOrder";
				return Crud.InsVerifyCrud.SelectMany(command);
			}
			protected override List<InsVerify> TableToList(DataTable table) {
				return Crud.InsVerifyCrud.TableToList(table);
			}
			protected override InsVerify Copy(InsVerify InsVerify) {
				return InsVerify.Clone();
			}
			protected override DataTable ListToTable(List<InsVerify> listInsVerifys) {
				return Crud.InsVerifyCrud.ListToTable(listInsVerifys,"InsVerify");
			}
			protected override void FillCacheIfNeeded() {
				InsVerifys.GetTableFromCache(false);
			}
			protected override bool IsInListShort(InsVerify InsVerify) {
				return !InsVerify.IsHidden;
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static InsVerifyCache _InsVerifyCache=new InsVerifyCache();

		///<summary>A list of all InsVerifys. Returns a deep copy.</summary>
		public static List<InsVerify> ListDeep {
			get {
				return _InsVerifyCache.ListDeep;
			}
		}

		///<summary>A list of all visible InsVerifys. Returns a deep copy.</summary>
		public static List<InsVerify> ListShortDeep {
			get {
				return _InsVerifyCache.ListShortDeep;
			}
		}

		///<summary>A list of all InsVerifys. Returns a shallow copy.</summary>
		public static List<InsVerify> ListShallow {
			get {
				return _InsVerifyCache.ListShallow;
			}
		}

		///<summary>A list of all visible InsVerifys. Returns a shallow copy.</summary>
		public static List<InsVerify> ListShort {
			get {
				return _InsVerifyCache.ListShallowShort;
			}
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_InsVerifyCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_InsVerifyCache.FillCacheFromTable(table);
				return table;
			}
			return _InsVerifyCache.GetTableFromCache(doRefreshCache);
		}

		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<InsVerify> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<InsVerify>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM insverify WHERE PatNum = "+POut.Long(patNum);
			return Crud.InsVerifyCrud.SelectMany(command);
		}
		*/
	}
}