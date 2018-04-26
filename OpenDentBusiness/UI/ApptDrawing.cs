using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDentBusiness.UI {
	public class ApptDrawing {
		///<summary>Stores the shading info for the provider bars on the left of the appointments module</summary>
		public static int[][] ProvBar;
		///<summary>The width of each operatory.</summary>
		public static float ColWidth;
		///<summary></summary>
		public static float TimeWidth=37;
		///<summary></summary>
		public static float ProvWidth=8;
		///<summary>Line height.  This is currently treated like a constant that the user has no control over.</summary>
		public static int LineH=12;
		///<summary>The number of columns.  Stays consistent even if weekly view.  The number of colums showing for one day.</summary>
		public static int ColCount;
		///<summary></summary>
		public static int ProvCount;
		///<summary>Based on the view.  If no view, then it is set to 1. Different computers can be showing different views.</summary>
		public static int RowsPerIncr;
		///<summary>Pulled from Prefs AppointmentTimeIncrement.  Either 5, 10, or 15. An increment can be one or more rows.</summary>
		public static int MinPerIncr;
		///<summary>Typical values would be 10,15,5,or 7.5.</summary>
		public static float MinPerRow;
		///<summary>Rows per hour, based on RowsPerIncr and MinPerIncr</summary>
		public static int RowsPerHr;
		///<summary>This gets set externally each time the module is selected.  It is the background schedule for the entire period.  Includes all types.</summary>
		public static List<Schedule> SchedListPeriod;
		///<summary></summary>
		public static bool IsWeeklyView;
		///<summary>Typically 5 or 7. Only used with weekview.</summary>
		public static int NumOfWeekDaysToDisplay=7;
		///<summary>The width of an entire day if using week view.</summary>
		public static float ColDayWidth;
		///<summary>Only used with weekview. The width of individual appointments within each day.  There might be rounding errors for now.</summary>
		public static float ColAptWidth;
		//these two are subsets of provs and ops. You can't include hidden prov or op in this list.
		///<summary>Visible provider bars in appt module.  This is a subset of the available provs.  You can't include a hidden prov in this list.</summary>
		public static List<Provider> VisProvs;
		///<summary>Visible ops in appt module.  List of visible operatories.  This is a subset of the available ops.  You can't include a hidden op in this list.  If user has set View.OnlyScheduledProvs, and not isWeekly, then the only ops to show will be for providers that have schedules for the day and ops with no provs assigned.</summary>
		public static List<Operatory> VisOps;
		///<summary>Previously, we looped through VisOps in order to find the 0-based column index for a given OpNum. This was too slow so we now use this helper dictionary to do the same lookup.</summary>
		public static Dictionary<long,int> DictOpNumToColumnNum=new Dictionary<long, int>();
		///<summary>Previously, we looped through VisProvs in order to find the 0-based column index for a given ProvNum. This was too slow so we now use this helper dictionary to do the same lookup.</summary>
		public static Dictionary<long,int> DictProvNumToColumnNum=new Dictionary<long, int>();
		///<summary></summary>
		public static float ApptSheetHeight;
		///<summary></summary>
		public static float ApptSheetWidth;

		///<summary>Draws the entire Appt background.  Used for main Appt module, for printing, and for mobile app.  Pass start and stop times of 12AM for 24 hours.  Set colsPerPage to VisOps.Count unless printing.  Set pageColumn to 0 unless printing.  Default fontSize is 8 unless printing.</summary>
		public static void DrawAllButAppts(Graphics g,bool showRedTimeLine,DateTime startTime,DateTime stopTime,int colsPerPage,int pageColumn,int fontSize,bool isPrinting) {
			//This will clear up the screen if the user clicked on a day where no providers are scheduled or any other scenario in which ColWidth will be 0.
			g.FillRectangle(new SolidBrush(SystemColors.Control),0,0,ApptSheetWidth,ApptSheetHeight);
			g.FillRectangle(new SolidBrush(Color.LightGray),0,0,TimeWidth,ApptSheetHeight);//L time bar
			g.FillRectangle(new SolidBrush(Color.LightGray),TimeWidth+ColWidth*ColCount+ProvWidth*ProvCount,0,TimeWidth,ApptSheetHeight);//R time bar
			DrawMainBackground(g,startTime,stopTime,colsPerPage,pageColumn);
			DrawBlockouts(g,startTime,stopTime,colsPerPage,pageColumn,fontSize,isPrinting);
			DrawWebSchedASAPSlots(g,startTime,stopTime,colsPerPage,pageColumn,fontSize,isPrinting);
			if(!IsWeeklyView) {
				DrawProvScheds(g,startTime,stopTime);
				DrawProvBars(g,startTime,stopTime);
			}
			DrawGridLines(g);
			if(showRedTimeLine) {
				DrawTimeIndicatorLine(g);
			}
			DrawMinutes(g,startTime,stopTime);
		}

		///<summary>Including the practice schedule.</summary>
		public static void DrawMainBackground(Graphics g,DateTime startTime,DateTime stopTime,int colsPerPage,int pageColumn) {
			Brush openBrush;
			Brush closedBrush;
			Brush holidayBrush;
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.AppointmentColors);
			try {
				openBrush=new SolidBrush(listDefs[0].ItemColor);
				closedBrush=new SolidBrush(listDefs[1].ItemColor);
				holidayBrush=new SolidBrush(listDefs[3].ItemColor);
			}
			catch {//this is just for design-time
				openBrush=new SolidBrush(Color.White);
				closedBrush=new SolidBrush(Color.LightGray);
				holidayBrush=new SolidBrush(Color.FromArgb(255,128,128));
			}
			List<Schedule> schedsForOp;
			//one giant rectangle for everything closed
			g.FillRectangle(closedBrush,TimeWidth,0,ColWidth*ColCount+ProvWidth*ProvCount,ApptSheetHeight);
			//then, loop through each day and operatory
			int startHour=startTime.Hour;
			if(IsWeeklyView) {
				for(int d=0;d<NumOfWeekDaysToDisplay;d++) {//for each day of the week displayed
					//if any schedule for this day is type practice and status holiday and not assigned to specific ops (so either clinics are not enabled
					//or the holiday applies to all ops for the clinic), color all of the op columns in the view for this day with the holiday brush
					if(SchedListPeriod.FindAll(x => x.SchedType==ScheduleType.Practice && x.Status==SchedStatus.Holiday) //find all holidays
						.Any(x => (int)x.SchedDate.DayOfWeek==d+1 //for this day of the week
							&& (x.ClinicNum==0 || (PrefC.HasClinicsEnabled && x.ClinicNum==Clinics.ClinicNum)))) //and either practice or for this clinic
					{
						g.FillRectangle(holidayBrush,TimeWidth+1+d*ColDayWidth,0,ColDayWidth,ApptSheetHeight);
					}
					//DayOfWeek enum goes from Sunday to Saturday 0-6, OD goes Monday to Sunday 0-6
					DayOfWeek dayofweek=(DayOfWeek)((d+1)%7);//when d==0, dayofweek=monday (or (DayOfWeek)1).  when d==6 we want dayofweek=sunday (or (DayOfWeek)0)
					for(int i=0;i<ColCount;i++) {//for each operatory visible for this view and day
						schedsForOp=Schedules.GetSchedsForOp(SchedListPeriod,dayofweek,VisOps[i]);
						//for each schedule assigned to this op
						foreach(Schedule schedCur in schedsForOp.Where(x => x.SchedType==ScheduleType.Provider)) {
							g.FillRectangle(openBrush
								,TimeWidth+1+d*ColDayWidth+(float)i*ColAptWidth
								,(schedCur.StartTime.Hours-startHour)*LineH*RowsPerHr+(int)schedCur.StartTime.Minutes*LineH/MinPerRow//RowsPerHr=6, MinPerRow=10
								,ColAptWidth
								,(schedCur.StopTime-schedCur.StartTime).Hours*LineH*RowsPerHr+(schedCur.StopTime-schedCur.StartTime).Minutes*LineH/MinPerRow);
						}
					}
				}
			}
			else {//only one day showing
				//if any schedule for the period is type practice and status holiday and either ClinicNum is 0 (HQ clinic or clinics not enabled) or clinics
				//are enabled and the schedule.ClinicNum is the currently selected clinic
				//SchedListPeriod contains scheds for only one day, not for a week
				if(SchedListPeriod.FindAll(x => x.SchedType==ScheduleType.Practice && x.Status==SchedStatus.Holiday) //find all holidays
					.Any(x => x.ClinicNum==0 || (PrefC.HasClinicsEnabled && x.ClinicNum==Clinics.ClinicNum)))//for the practice or clinic
				{
					g.FillRectangle(holidayBrush,TimeWidth+1,0,ColWidth*ColCount+ProvWidth*ProvCount,ApptSheetHeight);
				}
				for(int i=0;i<colsPerPage;i++) {//ops per page in day view
					if(i==ApptDrawing.VisOps.Count) {
						break;
					}
					int k=colsPerPage*pageColumn+i;
					if(k>=ApptDrawing.VisOps.Count) {
						break;
					}
					schedsForOp=Schedules.GetSchedsForOp(SchedListPeriod,VisOps[k]);
					foreach(Schedule schedCur in schedsForOp) {
						if(schedCur.StartTime.Hours>=24 || (stopTime.Hour!=0 && schedCur.StartTime.Hours>=stopTime.Hour)) {
							continue;
						}
						g.FillRectangle(openBrush
							,TimeWidth+ProvWidth*ProvCount+i*ColWidth
							,(schedCur.StartTime.Hours-startHour)*LineH*RowsPerHr+(int)schedCur.StartTime.Minutes*LineH/MinPerRow//6RowsPerHr 10MinPerRow
							,ColWidth
							,(schedCur.StopTime-schedCur.StartTime).Hours*LineH*RowsPerHr//6
								+(schedCur.StopTime-schedCur.StartTime).Minutes*LineH/MinPerRow);//10
					}
					//now, fill up to 2 timebars along the left side of each rectangle.
					foreach(Schedule schedCur in schedsForOp) {
						if(schedCur.Ops.Count==0) {//if this schedule is not assigned to specific ops, skip
							continue;
						}
						float provWidthAdj=0f;//use to adjust for primary and secondary provider bars in op
						if(Providers.GetIsSec(schedCur.ProvNum)) {
							provWidthAdj=ProvWidth;//drawing secondary prov bar so shift right
						}
						g.FillRectangle(new SolidBrush(Providers.GetColor(schedCur.ProvNum))
							,TimeWidth+ProvWidth*ProvCount+i*ColWidth+provWidthAdj
							,(schedCur.StartTime.Hours-startHour)*LineH*RowsPerHr+(int)schedCur.StartTime.Minutes*LineH/MinPerRow//6RowsPerHr 10MinPerRow
							,ProvWidth
							,(schedCur.StopTime-schedCur.StartTime).Hours*LineH*RowsPerHr//6
								+(schedCur.StopTime-schedCur.StartTime).Minutes*LineH/MinPerRow);//10
					}
				}
			}
			openBrush.Dispose();
			closedBrush.Dispose();
			holidayBrush.Dispose();
		}

		///<summary>Draws all the blockouts for the entire period.</summary>
		public static void DrawBlockouts(Graphics g,DateTime startTime,DateTime stopTime,int colsPerPage,int pageColumn,int fontSize,bool isPrinting) {
			Schedule[] schedForType=Schedules.GetForType(SchedListPeriod,ScheduleType.Blockout,0);
			SolidBrush blockBrush;
			Pen blockOutlinePen=new Pen(Color.Black,1);
			Pen penOutline;
			Font blockFont=new Font("Arial",fontSize);
			string blockText;
			RectangleF rect;
			Color colorBlockText=Defs.GetDefsForCategory(DefCat.AppointmentColors,true)[4].ItemColor;
			for(int i=0;i<schedForType.Length;i++) {
				blockBrush=new SolidBrush(Defs.GetColor(DefCat.BlockoutTypes,schedForType[i].BlockoutType));
				penOutline=new Pen(Defs.GetColor(DefCat.BlockoutTypes,schedForType[i].BlockoutType),2);
				blockText=Defs.GetName(DefCat.BlockoutTypes,schedForType[i].BlockoutType)+"\r\n"+schedForType[i].Note;
				for(int o=0;o<schedForType[i].Ops.Count;o++) {
					int startHour=startTime.Hour;
					if(isPrinting) {//Filtering logic for printing.
						int stopHour=stopTime.Hour;
						if(stopHour==0) {
							stopHour=24;
						}
						if(schedForType[i].StartTime.Hours>=stopHour) {
							continue;//Blockout starts after the current time frame.
						}
						if(schedForType[i].StopTime.Hours<=stopHour) {
							stopHour=schedForType[i].StopTime.Hours;
						}
						if(GetIndexOp(schedForType[i].Ops[o])>=(colsPerPage*pageColumn+colsPerPage)
						|| GetIndexOp(schedForType[i].Ops[o])<colsPerPage*pageColumn) {
							continue;//Blockout not on current page.
						}
					}
					if(IsWeeklyView) {
						if(GetIndexOp(schedForType[i].Ops[o])==-1) {
							continue;//don't display if op not visible
						}
						//this is a workaround because we start on Monday:
						int dayofweek=(int)schedForType[i].SchedDate.DayOfWeek-1;
						if(dayofweek==-1) {
							dayofweek=6;
						}
						rect=new RectangleF(
							TimeWidth+1+(dayofweek)*ColDayWidth
							+ColAptWidth*(GetIndexOp(schedForType[i].Ops[o],VisOps)-(colsPerPage*pageColumn))
							,(schedForType[i].StartTime.Hours-startHour)*LineH*RowsPerHr
							+schedForType[i].StartTime.Minutes*LineH/MinPerRow
							,ColAptWidth-1
							,(schedForType[i].StopTime-schedForType[i].StartTime).Hours*LineH*RowsPerHr
							+(schedForType[i].StopTime-schedForType[i].StartTime).Minutes*LineH/MinPerRow);
					}
					else {
						if(GetIndexOp(schedForType[i].Ops[o])==-1) {
							continue;//don't display if op not visible
						}
						rect=new RectangleF(
							TimeWidth+ProvWidth*ProvCount
							+ColWidth*(GetIndexOp(schedForType[i].Ops[o],VisOps)-(colsPerPage*pageColumn))
							+ProvWidth*2//so they don't overlap prov bars
							,(schedForType[i].StartTime.Hours-startHour)*LineH*RowsPerHr
							+schedForType[i].StartTime.Minutes*LineH/MinPerRow
							,ColWidth-1-ProvWidth*2
							,(schedForType[i].StopTime-schedForType[i].StartTime).Hours*LineH*RowsPerHr
							+(schedForType[i].StopTime-schedForType[i].StartTime).Minutes*LineH/MinPerRow);
					}
					//paint either solid block or outline
					if(PrefC.GetBool(PrefName.SolidBlockouts)) {
						g.FillRectangle(blockBrush,rect);
						g.DrawLine(blockOutlinePen,rect.X,rect.Y+1,rect.Right-1,rect.Y+1);
					}
					else {
						g.DrawRectangle(penOutline,rect.X+1,rect.Y+2,rect.Width-2,rect.Height-3);
					}
					g.DrawString(blockText,blockFont,new SolidBrush(colorBlockText),rect);
				}
				blockBrush.Dispose();
				penOutline.Dispose();
			}
			blockOutlinePen.Dispose();
		}


		private static void DrawWebSchedASAPSlots(Graphics g,DateTime startTime,DateTime stopTime,int colsPerPage,int pageColumn,int fontSize,bool isPrinting) {
			Schedule[] schedForType=Schedules.GetForType(SchedListPeriod,ScheduleType.WebSchedASAP,0);
			SolidBrush blockBrush;
			Pen blockOutlinePen=new Pen(Color.Black,1);
			Pen diagonalPen=new Pen(Color.LightGray,1);
			Pen penOutline;
			Font blockFont=new Font("Arial",fontSize);
			string blockText;
			RectangleF rect;
			Color colorBlockText=Defs.GetDefsForCategory(DefCat.AppointmentColors,true)[4].ItemColor;
			for(int i=0;i<schedForType.Length;i++) {
				blockBrush=new SolidBrush(Color.LightYellow);
				penOutline=new Pen(Color.LightYellow,2);
				blockText=Lans.g("ContrAppt","Web Sched ASAP Slot")+"\r\n"+schedForType[i].Note;
				for(int o=0;o<schedForType[i].Ops.Count;o++) {
					int startHour=startTime.Hour;
					if(isPrinting) {//Filtering logic for printing.
						int stopHour=stopTime.Hour;
						if(stopHour==0) {
							stopHour=24;
						}
						if(schedForType[i].StartTime.Hours>=stopHour) {
							continue;//Blockout starts after the current time frame.
						}
						if(schedForType[i].StopTime.Hours<=stopHour) {
							stopHour=schedForType[i].StopTime.Hours;
						}
						if(GetIndexOp(schedForType[i].Ops[o])>=(colsPerPage*pageColumn+colsPerPage)
						|| GetIndexOp(schedForType[i].Ops[o])<colsPerPage*pageColumn) {
							continue;//Blockout not on current page.
						}
					}
					if(IsWeeklyView) {
						if(GetIndexOp(schedForType[i].Ops[o])==-1) {
							continue;//don't display if op not visible
						}
						//this is a workaround because we start on Monday:
						int dayofweek=(int)schedForType[i].SchedDate.DayOfWeek-1;
						if(dayofweek==-1) {
							dayofweek=6;
						}
						rect=new RectangleF(
							TimeWidth+1+(dayofweek)*ColDayWidth
							+ColAptWidth*(GetIndexOp(schedForType[i].Ops[o],VisOps)-(colsPerPage*pageColumn))
							,(schedForType[i].StartTime.Hours-startHour)*LineH*RowsPerHr
							+schedForType[i].StartTime.Minutes*LineH/MinPerRow
							,ColAptWidth-5 //Shortened so that blockouts will be visible underneath
							,(schedForType[i].StopTime-schedForType[i].StartTime).Hours*LineH*RowsPerHr
							+(schedForType[i].StopTime-schedForType[i].StartTime).Minutes*LineH/MinPerRow);
					}
					else {//Daily view
						if(GetIndexOp(schedForType[i].Ops[o])==-1) {
							continue;//don't display if op not visible
						}
						rect=new RectangleF(
							TimeWidth+ProvWidth*ProvCount
							+ColWidth*(GetIndexOp(schedForType[i].Ops[o],VisOps)-(colsPerPage*pageColumn))
							+ProvWidth*2//so they don't overlap prov bars
							,(schedForType[i].StartTime.Hours-startHour)*LineH*RowsPerHr
							+schedForType[i].StartTime.Minutes*LineH/MinPerRow
							,ColWidth-8-ProvWidth*2 //Shortened so that blockouts will be visible underneath
							,(schedForType[i].StopTime-schedForType[i].StartTime).Hours*LineH*RowsPerHr
							+(schedForType[i].StopTime-schedForType[i].StartTime).Minutes*LineH/MinPerRow);
					}
					g.FillRectangle(blockBrush,rect);
					g.DrawLine(blockOutlinePen,rect.X,rect.Y+1,rect.Right-1,rect.Y+1);
					FillWithDiagonalLines(g,rect,diagonalPen,12);
					g.DrawString(blockText,blockFont,new SolidBrush(colorBlockText),rect);
				}
				blockBrush.Dispose();
				penOutline.Dispose();
			}
			blockOutlinePen.Dispose();
			diagonalPen.Dispose();
		}

		///<summary>Fills the rectangle with downward-sloping diagonal lines.</summary>
		public static void FillWithDiagonalLines(Graphics g,RectangleF rectToFill,Pen linePen,int pixelsBetweenLines) {
			//Walk along the bottom of the rectangle and draw lines that go to the left side or the top.
			for(float x=rectToFill.X+pixelsBetweenLines;x<rectToFill.Right;x+=pixelsBetweenLines) {
				float y2=rectToFill.Bottom-(x-rectToFill.X);
				float x2=rectToFill.Left+Math.Max(rectToFill.Top-y2,0);
				if(y2 < rectToFill.Top) {
					y2=rectToFill.Top;
				}
				g.DrawLine(linePen,x,rectToFill.Bottom,x2,y2);
			}
			//Walk along the right side of the rectangle a draw lines that go to the top or left side.
			float offsetY=pixelsBetweenLines-rectToFill.Width%pixelsBetweenLines;
			for(float y=rectToFill.Bottom-offsetY;y>rectToFill.Top;y-=pixelsBetweenLines) {
				float x2=rectToFill.Right-(y-rectToFill.Top);
				float y2=rectToFill.Top+Math.Max(rectToFill.Left-x2,0);
				if(x2 < rectToFill.Left) {
					x2=rectToFill.Left;
				}
				g.DrawLine(linePen,rectToFill.Right,y,x2,y2);
			}
		}

		///<summary>Returns the index of the opNum within VisOps.  Returns -1 if not in VisOps.</summary>
		public static int GetIndexOp(long opNum,List<Operatory> VisOps) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<VisOps.Count;i++) {
				if(VisOps[i].OperatoryNum==opNum)
					return i;
			}
			return -1;
		}

		///<summary>The background provider schedules for the provider bars on the left.</summary>
		public static void DrawProvScheds(Graphics g,DateTime startTime,DateTime stopTime) {
			Brush openBrush;
			try {
				openBrush=new SolidBrush(Defs.GetFirstForCategory(DefCat.AppointmentColors).ItemColor);
			}
			catch {//this is just for design-time
				openBrush=new SolidBrush(Color.White);
			}
			Provider provCur;
			Schedule[] schedForType;
			int startHour=startTime.Hour;
			int stopHour=stopTime.Hour;
			if(stopHour==0) {
				stopHour=24;
			}
			for(int j=0;j<VisProvs.Count;j++) {
				provCur=VisProvs[j];
				schedForType=Schedules.GetForType(SchedListPeriod,ScheduleType.Provider,provCur.ProvNum);
				for(int i=0;i<schedForType.Length;i++) {
					stopHour=stopTime.Hour;//Reset stopHour every time.
					if(stopHour==0) {
						stopHour=24;
					}
					if(schedForType[i].StartTime.Hours>=stopHour) {
						continue;
					}
					if(schedForType[i].StopTime.Hours<=stopHour) {
						stopHour=schedForType[i].StopTime.Hours;
					}
					g.FillRectangle(openBrush
						,TimeWidth+ProvWidth*j
						,(schedForType[i].StartTime.Hours-startHour)*LineH*RowsPerHr//6
						+(int)schedForType[i].StartTime.Minutes*LineH/MinPerRow//10
						,ProvWidth
						,(schedForType[i].StopTime-schedForType[i].StartTime).Hours*LineH*RowsPerHr//6
						+(schedForType[i].StopTime-schedForType[i].StartTime).Minutes*LineH/MinPerRow);//10
				}
			}
			openBrush.Dispose();
		}

		///<summary>Not the schedule, but just the indicators of scheduling.</summary>
		public static void DrawProvBars(Graphics g,DateTime startTime,DateTime stopTime) {
			int startingPoint=startTime.Hour*RowsPerHr;
			int stopHour=stopTime.Hour;
			if(stopHour==0) {
				stopHour=24;
			}
			int endingPoint=stopHour*RowsPerHr;
			for(int j=0;j<ProvBar.Length;j++) {
				for(int i=0;i<24*RowsPerHr;i++) {
					if(i<startingPoint) {
						continue;
					}
					if(i>=endingPoint) {
						break;
					}
					switch(ProvBar[j][i]) {
						case 0:
							break;
						case 1:
							try {
								g.FillRectangle(new SolidBrush(VisProvs[j].ProvColor)
									,TimeWidth+ProvWidth*j+1,((i-startingPoint)*LineH)+1,ProvWidth-1,LineH-1);
							}
							catch {//design-time
								g.FillRectangle(new SolidBrush(Color.White)
									,TimeWidth+ProvWidth*j+1,((i-startingPoint)*LineH)+1,ProvWidth-1,LineH-1);
							}
							break;
						case 2:
							g.FillRectangle(new HatchBrush(HatchStyle.DarkUpwardDiagonal
								,Color.Black,VisProvs[j].ProvColor)
								,TimeWidth+ProvWidth*j+1,((i-startingPoint)*LineH)+1,ProvWidth-1,LineH-1);
							break;
						default://more than 2
							g.FillRectangle(new SolidBrush(Color.Black)
								,TimeWidth+ProvWidth*j+1,((i-startingPoint)*LineH)+1,ProvWidth-1,LineH-1);
							break;
					}
				}
			}
		}

		///<summary></summary>
		public static void DrawGridLines(Graphics g) {
			//Vert
			if(IsWeeklyView) {
				g.DrawLine(new Pen(Color.DarkGray),0,0,0,ApptSheetHeight);
				g.DrawLine(new Pen(Color.White),TimeWidth-1,0,TimeWidth-1,ApptSheetHeight);
				g.DrawLine(new Pen(Color.DarkGray),TimeWidth,0,TimeWidth,ApptSheetHeight);
				for(int d=0;d<NumOfWeekDaysToDisplay;d++) {
					g.DrawLine(new Pen(Color.DarkGray),TimeWidth+ColDayWidth*d,0
						,TimeWidth+ColDayWidth*d,ApptSheetHeight);
				}
				g.DrawLine(new Pen(Color.DarkGray),TimeWidth+ColDayWidth*NumOfWeekDaysToDisplay,0
					,TimeWidth+1+ColDayWidth*NumOfWeekDaysToDisplay,ApptSheetHeight);
				g.DrawLine(new Pen(Color.DarkGray),TimeWidth*2+ColDayWidth*NumOfWeekDaysToDisplay,0
					,TimeWidth*2+1+ColDayWidth*NumOfWeekDaysToDisplay,ApptSheetHeight);
			}
			else {
				g.DrawLine(new Pen(Color.DarkGray),0,0,0,ApptSheetHeight);
				g.DrawLine(new Pen(Color.White),TimeWidth-2,0,TimeWidth-2,ApptSheetHeight);
				g.DrawLine(new Pen(Color.DarkGray),TimeWidth-1,0,TimeWidth-1,ApptSheetHeight);
				for(int i=0;i<ProvCount;i++) {
					g.DrawLine(new Pen(Color.DarkGray),TimeWidth+ProvWidth*i,0,TimeWidth+ProvWidth*i,ApptSheetHeight);
				}
				for(int i=0;i<ColCount;i++) {
					g.DrawLine(new Pen(Color.DarkGray),TimeWidth+ProvWidth*ProvCount+ColWidth*i,0
						,TimeWidth+ProvWidth*ProvCount+ColWidth*i,ApptSheetHeight);
				}
				g.DrawLine(new Pen(Color.DarkGray),TimeWidth+ProvWidth*ProvCount+ColWidth*ColCount,0
					,TimeWidth+ProvWidth*ProvCount+ColWidth*ColCount,ApptSheetHeight);
				g.DrawLine(new Pen(Color.DarkGray),TimeWidth*2+ProvWidth*ProvCount+ColWidth*ColCount,0
					,TimeWidth*2+ProvWidth*ProvCount+ColWidth*ColCount,ApptSheetHeight);
			}
			//horiz gray
			for(int i=0;i<(ApptSheetHeight);i+=LineH*RowsPerIncr) {
				g.DrawLine(new Pen(Color.LightGray),TimeWidth,i
					,TimeWidth+ColWidth*ColCount+ProvWidth*ProvCount,i);
			}
			//horiz Hour lines
			for(int i=0;i<ApptSheetHeight;i+=LineH*RowsPerHr) {
				g.DrawLine(new Pen(Color.LightGray),0,i-1//was white
					,TimeWidth*2+ColWidth*ColCount+ProvWidth*ProvCount,i-1);
				g.DrawLine(new Pen(Color.DarkSlateGray),0,i,TimeWidth,i);
				g.DrawLine(new Pen(Color.Black),TimeWidth,i
					,TimeWidth+ColWidth*ColCount+ProvWidth*ProvCount,i);
				g.DrawLine(new Pen(Color.DarkSlateGray),TimeWidth+ColWidth*ColCount+ProvWidth*ProvCount,i
					,TimeWidth*2+ColWidth*ColCount+ProvWidth*ProvCount,i);
			}
		}

		///<summary></summary>
		public static void DrawTimeIndicatorLine(Graphics g) {
			int curTimeY=(int)(DateTime.Now.Hour*LineH*RowsPerHr+DateTime.Now.Minute/60f*(float)LineH*RowsPerHr);
			g.DrawLine(new Pen(PrefC.GetColor(PrefName.AppointmentTimeLineColor)),0,curTimeY
				,TimeWidth*2+ProvWidth*ProvCount+ColWidth*ColCount,curTimeY);
			g.DrawLine(new Pen(PrefC.GetColor(PrefName.AppointmentTimeLineColor)),0,curTimeY+1
				,TimeWidth*2+ProvWidth*ProvCount+ColWidth*ColCount,curTimeY+1);
		}

		///<summary></summary>
		public static void DrawMinutes(Graphics g,DateTime startTime,DateTime stopTime) {
			Font font=new Font(FontFamily.GenericSansSerif,8);//was msSans
			Font bfont=new Font(FontFamily.GenericSansSerif,8,FontStyle.Bold);//was Arial
			g.TextRenderingHint=TextRenderingHint.SingleBitPerPixelGridFit;//to make printing clearer
			DateTime hour;
			CultureInfo ci=(CultureInfo)CultureInfo.CurrentCulture.Clone();
			string hFormat=Lans.GetShortTimeFormat(ci);
			string sTime;
			int stop=stopTime.Hour;
			if(stop==0) {//12AM, but we want to end on the next day so set to 24
				stop=24;
			}
			int index=0;//This will cause drawing times to always start at the top.
			for(int i=startTime.Hour;i<stop;i++) {
				hour=new DateTime(2000,1,1,i,0,0);//hour is the only important part of this time.
				sTime=hour.ToString(hFormat,ci);
				sTime=sTime.Replace("a. m.","am");//So that the times are not cutoff for foreign users
				sTime=sTime.Replace("p. m.","pm");
				SizeF sizef=g.MeasureString(sTime,bfont);
				g.DrawString(sTime,bfont,new SolidBrush(Color.Black),TimeWidth-sizef.Width-2,index*LineH*RowsPerHr+1);
				g.DrawString(sTime,bfont,new SolidBrush(Color.Black)
					,TimeWidth+ColWidth*ColCount+ProvWidth*ProvCount,index*LineH*RowsPerHr+1);
				if(MinPerIncr==5) {
					g.DrawString(":15",font,new SolidBrush(Color.Black)
						,TimeWidth-19,index*LineH*RowsPerHr+LineH*RowsPerIncr*3);
					g.DrawString(":30",font,new SolidBrush(Color.Black)
						,TimeWidth-19,index*LineH*RowsPerHr+LineH*RowsPerIncr*6);
					g.DrawString(":45",font,new SolidBrush(Color.Black)
						,TimeWidth-19,index*LineH*RowsPerHr+LineH*RowsPerIncr*9);
					g.DrawString(":15",font,new SolidBrush(Color.Black)
						,TimeWidth+ColWidth*ColCount+ProvWidth*ProvCount,index*LineH*RowsPerHr+LineH*RowsPerIncr*3);
					g.DrawString(":30",font,new SolidBrush(Color.Black)
						,TimeWidth+ColWidth*ColCount+ProvWidth*ProvCount,index*LineH*RowsPerHr+LineH*RowsPerIncr*6);
					g.DrawString(":45",font,new SolidBrush(Color.Black)
						,TimeWidth+ColWidth*ColCount+ProvWidth*ProvCount,index*LineH*RowsPerHr+LineH*RowsPerIncr*9);
				}
				else if(MinPerIncr==10) {
					g.DrawString(":10",font,new SolidBrush(Color.Black)
						,TimeWidth-19,index*LineH*RowsPerHr+LineH*RowsPerIncr);
					g.DrawString(":20",font,new SolidBrush(Color.Black)
						,TimeWidth-19,index*LineH*RowsPerHr+LineH*RowsPerIncr*2);
					g.DrawString(":30",font,new SolidBrush(Color.Black)
						,TimeWidth-19,index*LineH*RowsPerHr+LineH*RowsPerIncr*3);
					g.DrawString(":40",font,new SolidBrush(Color.Black)
						,TimeWidth-19,index*LineH*RowsPerHr+LineH*RowsPerIncr*4);
					g.DrawString(":50",font,new SolidBrush(Color.Black)
						,TimeWidth-19,index*LineH*RowsPerHr+LineH*RowsPerIncr*5);
					g.DrawString(":10",font,new SolidBrush(Color.Black)
						,TimeWidth+ColWidth*ColCount+ProvWidth*ProvCount,index*LineH*RowsPerHr+LineH*RowsPerIncr);
					g.DrawString(":20",font,new SolidBrush(Color.Black)
						,TimeWidth+ColWidth*ColCount+ProvWidth*ProvCount,index*LineH*RowsPerHr+LineH*RowsPerIncr*2);
					g.DrawString(":30",font,new SolidBrush(Color.Black)
						,TimeWidth+ColWidth*ColCount+ProvWidth*ProvCount,index*LineH*RowsPerHr+LineH*RowsPerIncr*3);
					g.DrawString(":40",font,new SolidBrush(Color.Black)
						,TimeWidth+ColWidth*ColCount+ProvWidth*ProvCount,index*LineH*RowsPerHr+LineH*RowsPerIncr*4);
					g.DrawString(":50",font,new SolidBrush(Color.Black)
						,TimeWidth+ColWidth*ColCount+ProvWidth*ProvCount,index*LineH*RowsPerHr+LineH*RowsPerIncr*5);
				}
				else {//15
					g.DrawString(":15",font,new SolidBrush(Color.Black)
						,TimeWidth-19,index*LineH*RowsPerHr+LineH*RowsPerIncr);
					g.DrawString(":30",font,new SolidBrush(Color.Black)
						,TimeWidth-19,index*LineH*RowsPerHr+LineH*RowsPerIncr*2);
					g.DrawString(":45",font,new SolidBrush(Color.Black)
						,TimeWidth-19,index*LineH*RowsPerHr+LineH*RowsPerIncr*3);
					g.DrawString(":15",font,new SolidBrush(Color.Black)
						,TimeWidth+ColWidth*ColCount+ProvWidth*ProvCount,index*LineH*RowsPerHr+LineH*RowsPerIncr);
					g.DrawString(":30",font,new SolidBrush(Color.Black)
						,TimeWidth+ColWidth*ColCount+ProvWidth*ProvCount,index*LineH*RowsPerHr+LineH*RowsPerIncr*2);
					g.DrawString(":45",font,new SolidBrush(Color.Black)
						,TimeWidth+ColWidth*ColCount+ProvWidth*ProvCount,index*LineH*RowsPerHr+LineH*RowsPerIncr*3);
				}
				index++;
			}
		}

		///<summary></summary>
		public static void ComputeColDayWidth() {
			ColDayWidth=(ApptSheetWidth-TimeWidth*2)/NumOfWeekDaysToDisplay;
		}

		///<summary></summary>
		public static void ComputeColAptWidth() {
			ColAptWidth=(float)(ColDayWidth-1)/(float)ColCount;
		}

		///<summary></summary>
		public static void SetLineHeight(int fontSize) {
			LineH=new Font("Arial",fontSize).Height;
		}

		///<summary></summary>
		public static int XPosToOpIdx(int xPos) {
			int retVal;
			if(IsWeeklyView) {
				int day=XPosToDay(xPos);
				retVal=(int)Math.Floor((double)(xPos-TimeWidth-day*ColDayWidth)/ColAptWidth);
			}
			else {
				retVal=(int)Math.Floor((double)(xPos-TimeWidth-ProvWidth*ProvCount)/ColWidth);
			}
			if(retVal>ColCount-1)
				retVal=ColCount-1;
			if(retVal<0)
				retVal=0;
			return retVal;
		}

		///<summary>If not weekview, then it always returns 0.  If weekview, then it gives the dayofweek as int. Always based on current view, so 0 will be first day showing.</summary>
		public static int XPosToDay(int xPos) {
			if(!IsWeeklyView) {
				return 0;
			}
			int retVal=(int)Math.Floor((double)(xPos-TimeWidth)/ColDayWidth);
			if(retVal>NumOfWeekDaysToDisplay-1)
				retVal=NumOfWeekDaysToDisplay-1;
			if(retVal<0)
				retVal=0;
			return retVal;
		}

		///<summary>Called when mouse down anywhere on apptSheet. Automatically rounds down.</summary>
		public static int YPosToHour(int yPos) {
			int retVal=yPos/LineH/RowsPerHr;//newY/LineH/6;
			return retVal;
		}

		///<summary>Called when mouse down anywhere on apptSheet. This will give very precise minutes. It is not rounded for accuracy.</summary>
		public static int YPosToMin(int yPos) {
			int hourPortion=YPosToHour(yPos)*LineH*RowsPerHr;
			float MinPerPixel=60/(float)LineH/(float)RowsPerHr;
			int minutes=(int)((yPos-hourPortion)*MinPerPixel);
			return minutes;
		}

		///<summary>Used when dropping an appointment to a new location.  Converts x-coordinate to operatory index of ApptCatItems.VisOps, rounding to the nearest.  In this respect it is very different from XPosToOp.</summary>
		public static int ConvertToOp(int newX) {
			int retVal=0;
			if(IsWeeklyView) {
				int dayI=XPosToDay(newX);//does not round
				int deltaDay=dayI*(int)ColDayWidth;
				int adjustedX=newX-(int)TimeWidth-deltaDay;
				retVal=(int)Math.Round((double)(adjustedX)/ColAptWidth);
				//when there are multiple days, special situation where x is within the last op for the day, so it goes to next day.
				if(retVal>VisOps.Count-1 && dayI<NumOfWeekDaysToDisplay-1) {
					retVal=0;
				}
			}
			else {
				retVal=(int)Math.Round((double)(newX-TimeWidth-ProvWidth*ProvCount)/ColWidth);
			}
			//make sure it's not outside bounds of array:
			if(retVal > VisOps.Count-1)
				retVal=VisOps.Count-1;
			if(retVal<0)
				retVal=0;
			return retVal;
		}

		///<summary>Used when dropping an appointment to a new location.  Converts x-coordinate to day index.  Only used in weekly view.</summary>
		public static int ConvertToDay(int newX) {
			int retVal=(int)Math.Floor((double)(newX-TimeWidth)/(double)ColDayWidth);
			//the above works for every situation except when in the right half of the last op for a day. Test for that situation:
			if(newX-TimeWidth > (retVal+1)*ColDayWidth-ColAptWidth/2) {
				retVal++;
			}
			//make sure it's not outside bounds of array:
			if(retVal>NumOfWeekDaysToDisplay-1)
				retVal=NumOfWeekDaysToDisplay-1;
			if(retVal<0)
				retVal=0;
			return retVal;
		}

		///<summary>Used when dropping an appointment to a new location. Rounds to the nearest increment.</summary>
		public static int ConvertToHour(int newY) {
			//return (int)((newY+LineH/2)/6/LineH);
			return (int)(((double)newY+(double)LineH*(double)RowsPerIncr/2)/(double)RowsPerHr/(double)LineH);
		}

		///<summary>Used when dropping an appointment to a new location. Rounds to the nearest increment.</summary>
		public static int ConvertToMin(int newY) {
			//int retVal=(int)(Decimal.Remainder(newY,6*LineH)/LineH)*10;
			//first, add pixels equivalent to 1/2 increment: newY+LineH*RowsPerIncr/2
			//Yloc     Height     Rows      1
			//---- + ( ------ x --------- x - )
			//  1       Row     Increment   2
			//then divide by height per hour: RowsPerHr*LineH
			//Rows   Height
			//---- * ------
			//Hour    Row
			int pixels=(int)Decimal.Remainder(
				(decimal)newY+(decimal)LineH*(decimal)RowsPerIncr/2
				,(decimal)RowsPerHr*(decimal)LineH);
			//We are only interested in the remainder, and this is called pixels.
			//Convert pixels to increments. Round down to nearest increment when converting to int.
			//pixels/LineH/RowsPerIncr:
			//pixels    Rows    Increment
			//------ x ------ x ---------
			//  1      pixels     Rows
			int increments=(int)((double)pixels/(double)LineH/(double)RowsPerIncr);
			//Convert increments to minutes: increments*MinPerIncr
			int retVal=increments*MinPerIncr;
			if(retVal==60)
				return 0;
			return retVal;
		}

		///<summary>Called from ContrAppt.comboView_SelectedIndexChanged and ContrAppt.RefreshVisops. Set colCountOverride to 0 unless printing.</summary>
		public static void ComputeColWidth(int colCountOverride) {
			if(VisOps==null || VisProvs==null) {
				return;
			}
			try {
				if(RowsPerIncr==0) {
					RowsPerIncr=1;
				}
				//Allow user to choose how many columns print per page.
				if(colCountOverride>0) {
					ColCount=colCountOverride;
				}
				else {
					ColCount=VisOps.Count;
				}
				if(IsWeeklyView) {
					ColCount=VisOps.Count;
					ProvCount=0;
				}
				else {
					ProvCount=VisProvs.Count;
				}
				if(ColCount==0) {
					ColWidth=0;
				}
				else {
					if(IsWeeklyView) {
						ColDayWidth=(ApptSheetWidth-TimeWidth*2)/NumOfWeekDaysToDisplay;
						ColAptWidth=(float)(ColDayWidth-1)/(float)ColCount;
						ColWidth=(ApptSheetWidth-TimeWidth*2-ProvWidth*ProvCount)/ColCount;
					}
					else {
						ColWidth=(ApptSheetWidth-TimeWidth*2-ProvWidth*ProvCount)/ColCount;
					}
				}
				MinPerIncr=PrefC.GetInt(PrefName.AppointmentTimeIncrement);
				MinPerRow=(float)MinPerIncr/RowsPerIncr;
				RowsPerHr=60/MinPerIncr*RowsPerIncr;
			}
			catch {
				MessageBox.Show("error computing width");
			}
		}

		///<summary>Returns the index of the opNum within VisOps.  Returns -1 if not in VisOps.</summary>
		public static int GetIndexOp(long opNum) {
			//No need to check RemotingRole; no call to db.
			int index;
			return DictOpNumToColumnNum.TryGetValue(opNum,out index) ? index : -1;
		}

		///<summary>Returns the index of the provNum within VisProvs.</summary>
		public static int GetIndexProv(long provNum) {
			//No need to check RemotingRole; no call to db.
			int index;
			return DictProvNumToColumnNum.TryGetValue(provNum,out index) ? index : -1;
		}

		public static void ProvBarShading(DataRow row) {
			string patternShowing=ApptSingleDrawing.GetPatternShowing(row["Pattern"].ToString());
			int indexProv=-1;
			if(row["IsHygiene"].ToString()=="1") {
				indexProv=GetIndexProv(PIn.Long(row["ProvHyg"].ToString()));
			}
			else {
				indexProv=GetIndexProv(PIn.Long(row["ProvNum"].ToString()));
			}
			if(indexProv!=-1 && row["AptStatus"].ToString()!=((int)ApptStatus.Broken).ToString()) {
				int startIndex=ApptSingleDrawing.ConvertToY(row,0)/LineH;//rounds down
				for(int k=0;k<patternShowing.Length;k++) {
					if(patternShowing.Substring(k,1)=="X") {
						try {
							ProvBar[indexProv][startIndex+k]++;
						}
						catch {
							//appointment must extend past midnight.  Very rare
						}
					}
				}
			}
		}

	}
}