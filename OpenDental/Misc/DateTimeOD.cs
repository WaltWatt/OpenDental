using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeBase;

namespace OpenDental {
	public class DateTimeOD {
		private DateSpan _dateSpan;

		public int YearsDiff {
			get {
				return _dateSpan.YearsDiff;
			}
		}

		public int MonthsDiff{
			get {
				return _dateSpan.MonthsDiff;
			}
		}

		public int DaysDiff{
			get {
				return _dateSpan.DaysDiff;
			}
		}

		///<summary>We are switching to using this method instead of DateTime.Today.  You can track actual Year/Month/Date differences by creating an 
		///instance of this class and passing in the two dates to compare.  The values will be stored in YearsDiff, MonthsDiff, and DaysDiff.</summary> 
		public static DateTime Today {
			//The problem is with dotNet serilazation to the middle tier.  It will tack on zulu change for UTC.  Like this:
			//2013-04-29T04:00:00-7:00
			//DateTime objects created with DateTimeKind.Local in one timezone and sent over the middle tier to another time zone will be different at their 
			//destination, because .NET will automatically try to adjust for the timezone change.
			//DateTime.Today uses DateTimeKind.Local and we want DateTimeKind.Unspecified.
			//DateTime DateTimeToday=DateTime.Today;//DateTimeKind.Local, so serialization seems attempt to convert it to z.
			//DateTime DateTimeSpecific=new DateTime(2013,4,29);//DateTimeKind.Unspecified.
			//DateTime DateTimeParsed=DateTime.Parse("4/29/2013");//DateTimeKind.Unspecified.
			get {
				return new DateTime(DateTime.Today.Year,DateTime.Today.Month,DateTime.Today.Day,0,0,0,DateTimeKind.Unspecified);//Today at midnight with no timezone information.
			}
		}

		///<summary>DEPRECATED. Use CodeBase.DateSpan.
		///Pass in the two dates that you want to compare. Results will be stored in YearsDiff, MonthsDiff, and DaysDiff.
		///Always subtracts the smaller date from the larger date to return a positive (or 0) value.</summary>
		public DateTimeOD(DateTime date1,DateTime date2) {
			_dateSpan=new DateSpan(date1,date2);
		}
		
		///<summary>Returns the most recent valid date possible based on the year and month passed in.
		///E.g. y:2017,m:4,d:31 is passed in (an invalid date) which will return a date of "04/30/2017" which is the most recent 'valid' date.
		///Throws an exception if the year is not between 1 and 9999, and if the month is not between 1 and 12.</summary>
		public static DateTime GetMostRecentValidDate(int year,int month,int day) {
			int maxDay=DateTime.DaysInMonth(year,month);
			return new DateTime(year,month,Math.Min(day,maxDay));
		}


	}
}
