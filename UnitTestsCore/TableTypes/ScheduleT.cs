using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class ScheduleT {

		///<summary></summary>
		public static Schedule CreateSchedule(DateTime schedDate,TimeSpan startTime,TimeSpan stopTime,ScheduleType schedType=ScheduleType.Practice
			,SchedStatus status=SchedStatus.Open,long blockoutType=0,long clinicNum=0,long employeNum=0,long provNum=0,List<long> listOpNums=null)
		{
			Schedule schedule=new Schedule();
			schedule.BlockoutType=blockoutType;
			schedule.ClinicNum=clinicNum;
			schedule.EmployeeNum=employeNum;
			schedule.ProvNum=provNum;
			schedule.SchedDate=schedDate;
			schedule.SchedType=schedType;
			schedule.StartTime=startTime;
			schedule.Status=status;
			schedule.StopTime=stopTime;
			Schedules.Insert(schedule,false);
			if(listOpNums!=null) {
				schedule.Ops=listOpNums;
				foreach(long opNum in listOpNums) {
					ScheduleOp schedOp=new ScheduleOp();
					schedOp.OperatoryNum=opNum;
					schedOp.ScheduleNum=schedule.ScheduleNum;
					ScheduleOps.Insert(schedOp);
				}
			}
			return schedule;
		}

		///<summary>Deletes everything from the schedule table.  Does not truncate the table so that PKs are not reused on accident.</summary>
		public static void ClearScheduleTable() {
			string command="DELETE FROM schedule WHERE ScheduleNum > 0";
			DataCore.NonQ(command);
		}
	}
}
