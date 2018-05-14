using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenDentBusiness.UI {
	public class ApptSingleDrawing {
		public static float ApptSingleHeight;
		public static float ApptSingleWidth;
		private static Point location;

		///<summary>Set default fontSize to 8 unless printing.</summary>
		public static void DrawEntireAppt(Graphics g,DataRow dataRoww,string patternShowing,float totalWidth,float totalHeight,bool isSelected,bool thisIsPinBoard,long selectedAptNum,List<ApptViewItem> apptRows,ApptView apptViewCur,DataTable tableApptFields,DataTable tablePatFields,int fontSize,bool isPrinting) {
			Pen penB=new Pen(Color.Black);
			Pen penW=new Pen(Color.White);
			Pen penGr=new Pen(Color.SlateGray);
			Pen penDG=new Pen(Color.DarkSlateGray);
			Pen penO;//provider outline color
			Color backColor;
			Color provColor;
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.AppointmentColors);
			if(dataRoww["ProvNum"].ToString()!="0" && dataRoww["IsHygiene"].ToString()=="0") {//dentist
				provColor=Providers.GetColor(PIn.Long(dataRoww["ProvNum"].ToString()));
				penO=new Pen(Providers.GetOutlineColor(PIn.Long(dataRoww["ProvNum"].ToString())));
			}
			else if(dataRoww["ProvHyg"].ToString()!="0" && dataRoww["IsHygiene"].ToString()=="1") {//hygienist
				provColor=Providers.GetColor(PIn.Long(dataRoww["ProvHyg"].ToString()));
				penO=new Pen(Providers.GetOutlineColor(PIn.Long(dataRoww["ProvHyg"].ToString())));
			}
			else {//unknown
				provColor=Color.White;
				penO=new Pen(Color.Black);
			}
			backColor=provColor;//Default the appointment to the primary provider's color.
			if(PIn.Long(dataRoww["AptStatus"].ToString())==(int)ApptStatus.Complete) {
				backColor=listDefs[2].ItemColor;
			}
			else if(PIn.Long(dataRoww["AptStatus"].ToString())==(int)ApptStatus.PtNote) {
				backColor=listDefs[5].ItemColor;
				if(PIn.Int(dataRoww["ColorOverride"].ToString()) != 0) {//Patient note has an override.
					backColor=Color.FromArgb(PIn.Int(dataRoww["ColorOverride"].ToString()));
				}
			}
			else if(PIn.Long(dataRoww["AptStatus"].ToString())==(int)ApptStatus.PtNoteCompleted) {
				backColor=listDefs[6].ItemColor;
			}
			else if(PIn.Int(dataRoww["ColorOverride"].ToString()) != 0) {
				backColor=Color.FromArgb(PIn.Int(dataRoww["ColorOverride"].ToString()));
			}
			//Check to see if the patient is late for their appointment.  This 
			DateTime aptDateTime=PIn.DateT(dataRoww["AptDateTime"].ToString());
			DateTime aptDateTimeArrived=PIn.DateT(dataRoww["AptDateTimeArrived"].ToString());
			//If the appointment is scheduled and the patient was late for the appointment.
			if((PIn.Long(dataRoww["AptStatus"].ToString())==(int)ApptStatus.Scheduled)
				&& ((aptDateTimeArrived.TimeOfDay==TimeSpan.FromHours(0) && DateTime.Now>aptDateTime) 
					|| (aptDateTimeArrived.TimeOfDay>TimeSpan.FromHours(0) && aptDateTimeArrived>aptDateTime))) 
			{
				//Loop through all the appt view items to see if appointments should display the late color.
				for(int i=0;i<apptRows.Count;i++) {
					if(apptRows[i].ElementDesc=="LateColor") {
						backColor=apptRows[i].ElementColor;
						break;
					}
				}
			}
			//Do not use the code block below. We do not want to draw appt color based on appointment type. Logic for that is handled with the color override column.
			//else if(PIn.Long(dataRoww["AppointmentTypeNum"].ToString()) != 0) {
			//	AppointmentType t = AppointmentTypes.GetOne(PIn.Long(dataRoww["AppointmentTypeNum"].ToString()));
			//	backColor=t.AppointmentTypeColor;
			//}
			SolidBrush backBrush=new SolidBrush(backColor);
			g.FillRectangle(backBrush,7,0,totalWidth-7,(int)totalHeight);
			g.FillRectangle(Brushes.White,0,0,7,(int)totalHeight);
			Pen penTimediv=Pens.Silver;
			for(int i=0;i<patternShowing.Length;i++) {//Info.MyApt.Pattern.Length;i++){
				if(patternShowing.Substring(i,1)=="X") {
					if(isPrinting) {
						g.FillRectangle(new SolidBrush(provColor),0,i*ApptDrawing.LineH,7,ApptDrawing.LineH);
					}
					else {
						g.FillRectangle(new SolidBrush(provColor),1,i*ApptDrawing.LineH+1,6,ApptDrawing.LineH);
					}
				}
				else {
					//leave empty
				}
				if(Math.IEEERemainder((double)i,(double)ApptDrawing.RowsPerIncr)==0) {//0/1
					if(isPrinting) {
						g.DrawLine(penTimediv,0,i*ApptDrawing.LineH,7,i*ApptDrawing.LineH);
					}
					else {
						g.DrawLine(penTimediv,1,i*ApptDrawing.LineH,6,i*ApptDrawing.LineH);
					}
				}
			}
			g.DrawLine(penB,7,0,7,(int)totalHeight);
			#region Main rows
			Point drawLoc=new Point(9,0);
			int elementI=0;
			while(drawLoc.Y<totalHeight && elementI<apptRows.Count) {
				if(apptRows[elementI].ElementAlignment!=ApptViewAlignment.Main) {
					elementI++;
					continue;
				}
				drawLoc=DrawElement(g,elementI,drawLoc,ApptViewStackBehavior.Vertical,ApptViewAlignment.Main,backBrush,dataRoww,apptRows,tableApptFields,tablePatFields,totalWidth,totalHeight,fontSize,isPrinting);//set the drawLoc to a new point, based on space used by element
				elementI++;
			}
			#endregion
			#region Highlighting border
			if(isSelected	|| (!thisIsPinBoard && dataRoww["AptNum"].ToString()==selectedAptNum.ToString())) {
				//Left
				g.DrawLine(penO,8,1,8,totalHeight-2);
				g.DrawLine(penO,9,1,9,totalHeight-3);
				//Right
				g.DrawLine(penO,totalWidth-2,1,totalWidth-2,totalHeight-2);
				g.DrawLine(penO,totalWidth-3,2,totalWidth-3,totalHeight-3);
				//Top
				g.DrawLine(penO,8,1,totalWidth-2,1);
				g.DrawLine(penO,8,2,totalWidth-3,2);
				//bottom
				g.DrawLine(penO,9,totalHeight-2,totalWidth-2,totalHeight-2);
				g.DrawLine(penO,10,totalHeight-3,totalWidth-3,totalHeight-3);
			}
			#endregion
			#region UR
			drawLoc=new Point((int)totalWidth-1,0);//in the UR area, we refer to the upper right corner of each element.
			elementI=0;
			while(drawLoc.Y<totalHeight && elementI<apptRows.Count) {
				if(apptRows[elementI].ElementAlignment!=ApptViewAlignment.UR) {
					elementI++;
					continue;
				}
				drawLoc=DrawElement(g,elementI,drawLoc,apptViewCur.StackBehavUR,ApptViewAlignment.UR,backBrush,dataRoww,apptRows,tableApptFields,tablePatFields,totalWidth,totalHeight,fontSize,isPrinting);
				elementI++;
			}
			Plugins.HookAddCode(null,"OpenDentBusiness.UI.ApptSingleDrawing.DrawEntireAppt_UR",dataRoww,g,drawLoc);
			#endregion
			#region LR
			drawLoc=new Point((int)totalWidth-1,(int)totalHeight-1);//in the LR area, we refer to the lower right corner of each element.
			elementI=apptRows.Count-1;//For lower right, draw the list backwards.
			while(drawLoc.Y>0 && elementI>=0) {
				if(apptRows[elementI].ElementAlignment!=ApptViewAlignment.LR) {
					elementI--;
					continue;
				}
				drawLoc=DrawElement(g,elementI,drawLoc,apptViewCur.StackBehavLR,ApptViewAlignment.LR,backBrush,dataRoww,apptRows,tableApptFields,tablePatFields,totalWidth,totalHeight,fontSize,isPrinting);
				elementI--;
			}
			#endregion
			//Main outline
			if(isPrinting) {
				g.DrawRectangle(new Pen(Color.Black),0,0,totalWidth,totalHeight);
			}
			else {
				g.DrawRectangle(new Pen(Color.Black),0,0,totalWidth-1,totalHeight-1);
			}
			//broken X
			if(dataRoww["AptStatus"].ToString()==((int)ApptStatus.Broken).ToString()) {
				g.DrawLine(new Pen(Color.Black),8,1,totalWidth-1,totalHeight-1);
				g.DrawLine(new Pen(Color.Black),8,totalHeight-1,totalWidth-1,1);
			}
			//Dispose of the objects.
			DisposeObjects(penB,penW,penGr,penDG,penO,backBrush);
		}

		///<summary></summary>
		public static Point DrawElement(Graphics g,int elementI,Point drawLoc,ApptViewStackBehavior stackBehavior,ApptViewAlignment align,Brush backBrush,DataRow dataRoww,List<ApptViewItem> apptRows,DataTable tableApptFields,DataTable tablePatFields,float totalWidth,float totalHeight,int fontSize,bool isPrinting) {
			Font baseFont=new Font("Arial",fontSize);
			string text="";
			bool isNote=false;
			bool isInsuranceColor=false;
			#region FillText
			if(PIn.Long(dataRoww["AptStatus"].ToString()) == (int)ApptStatus.PtNote
				|| PIn.Long(dataRoww["AptStatus"].ToString()) == (int)ApptStatus.PtNoteCompleted) {
				isNote=true;
			}
			bool isGraphic=false;
			if(apptRows[elementI].ElementDesc=="ConfirmedColor") {
				isGraphic=true;
			}
			if(apptRows[elementI].ApptFieldDefNum>0) {
				string fieldName=ApptFieldDefs.GetFieldName(apptRows[elementI].ApptFieldDefNum);
				for(int i=0;i<tableApptFields.Rows.Count;i++) {
					if(tableApptFields.Rows[i]["AptNum"].ToString()!=dataRoww["AptNum"].ToString()) {
						continue;
					}
					if(tableApptFields.Rows[i]["FieldName"].ToString()!=fieldName) {
						continue;
					}
					text=tableApptFields.Rows[i]["FieldValue"].ToString();
				}
			}
			else if(apptRows[elementI].PatFieldDefNum>0) {
				string fieldName=PatFieldDefs.GetFieldName(apptRows[elementI].PatFieldDefNum);
				for(int i=0;i<tablePatFields.Rows.Count;i++) {
					if(tablePatFields.Rows[i]["PatNum"].ToString()!=dataRoww["PatNum"].ToString()) {
						continue;
					}
					if(tablePatFields.Rows[i]["FieldName"].ToString()!=fieldName) {
						continue;
					}
					text=tablePatFields.Rows[i]["FieldValue"].ToString();
				}
			}
			else switch(apptRows[elementI].ElementDesc) {
					case "Address":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["address"].ToString();
						}
						break;
					case "AddrNote":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["addrNote"].ToString();
						}
						break;
					case "Age":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["age"].ToString();
						}
						break;
					case "ASAP":
						if(isNote) {
							text="";
						}
						else {
							if(PIn.Long(dataRoww["Priority"].ToString())==(int)ApptPriority.ASAP) {
								text=Lans.g("ContrAppt","ASAP");
							}
						}
						break;
					case "ASAP[A]":
						if(PIn.Long(dataRoww["Priority"].ToString())==(int)ApptPriority.ASAP) {
							text=Lans.g("ContrAppt","A");
						}
						break;
					case "AssistantAbbr":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["assistantAbbr"].ToString();
						}
						break;
					case "Birthdate":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["Birthdate"].ToString();
						}
						break;
					case "ChartNumAndName":
						text=dataRoww["chartNumAndName"].ToString();
						break;
					case "ChartNumber":
						text=dataRoww["chartNumber"].ToString();
						break;
					case "CreditType":
						text=dataRoww["CreditType"].ToString();
						break;
					case "EstPatientPortion":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["estPatientPortion"].ToString();
						}
						break;
					case "Guardians":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["guardians"].ToString();
						}
						break;
					case "HasIns[I]":
						text=dataRoww["hasIns[I]"].ToString();
						break;
					case "HmPhone":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["hmPhone"].ToString();
						}
						break;
					case "Insurance":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["insurance"].ToString();
						}
						break;
					case "InsuranceColor":
						if(isNote) {
							text="";
						}
						else {
							isInsuranceColor=true;
							text=dataRoww["insurance"].ToString();
						}
						break;
					case "InsToSend[!]":
						text=dataRoww["insToSend[!]"].ToString();
						break;
					case "Lab":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["lab"].ToString();
						}
						break;
					case "MedOrPremed[+]":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["medOrPremed[+]"].ToString();
						}
						break;
					case "MedUrgNote":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["MedUrgNote"].ToString();
						}
						break;
					case "NetProduction":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["netProduction"].ToString();
						}
						break;
					case "Note":
						text=dataRoww["Note"].ToString();
						break;
					case "PatientName":
						text=dataRoww["patientName"].ToString();
						break;
					case "PatientNameF":
						text=dataRoww["patientNameF"].ToString();
						break;
					case "PatientNamePref":
						text=dataRoww["PatientNamePref"].ToString();
						break;
					case "PatNum":
						text=dataRoww["patNum"].ToString();
						break;
					case "PatNumAndName":
						text=dataRoww["patNumAndName"].ToString();
						break;
					case "PremedFlag":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["preMedFlag"].ToString();
						}
						break;
					case "Procs":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["procs"].ToString();
						}
						break;
					case "ProcsColored":
						string value=dataRoww["procsColored"].ToString();
						string[] lines=value.Split(new string[] { "</span>" },StringSplitOptions.RemoveEmptyEntries);
						Point tempPt=new Point();
						tempPt=drawLoc;
						int lastH=0;
						int count=1;
						for(int i=0;i<lines.Length;i++) {
							Match m=Regex.Match(lines[i],"^<span color=\"(-?[0-9]*)\">(.*)$");
							string rgbInt=m.Result("$1");
							string proc=m.Result("$2");
							if(lines[i]!=lines[lines.Length-1]) {
								proc+=",";
							}
							if(rgbInt=="") {
								rgbInt=apptRows[elementI].ElementColorXml.ToString();
							}
							Color c=Color.FromArgb(Convert.ToInt32(rgbInt));
							SizeF procSize=g.MeasureString(proc,baseFont,(int)totalWidth-9,new StringFormat(StringFormatFlags.MeasureTrailingSpaces));
							procSize.Width=(float)Math.Ceiling((double)procSize.Width);
							if(tempPt.X+procSize.Width>totalWidth) {
								tempPt.X=drawLoc.X;
								tempPt.Y+=lastH;
								count++;
							}
							//When printing the appointment module, make sure that we do not draw off of the appointment bubble.
							if(tempPt.Y+procSize.Height > totalHeight && isPrinting) {
								//No need to do this check when drawing to the appt screen cause its just an image on a control which clips itself.
								//When printing, we can't draw this so return with the new drawLoc. 
								break;
							}
							RectangleF procRect=new RectangleF(tempPt,procSize);
							SolidBrush sb=new SolidBrush(c);
							g.DrawString(proc,baseFont,sb,procRect);
							DisposeObjects(sb);
							tempPt.X+=(int)procRect.Width+3;//+3 is room for spaces
							if((int)procRect.Height>lastH) {
								lastH=(int)procRect.Height;
							}
						}
						drawLoc.Y+=lastH*count;
						return drawLoc;
					case "Production":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["production"].ToString();
						}
						break;
					case "Provider":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["provider"].ToString();
						}
						break;
					case "TimeAskedToArrive":
						if(isNote) {
							text="";
						}
						else {
							DateTime timeAskedToArrive;
							if(DateTime.TryParse(dataRoww["timeAskedToArrive"].ToString(),out timeAskedToArrive)) {//timeAskedToArrive value could be blank
								text=timeAskedToArrive.ToShortTimeString();
							}
							else {
								//probably just a blank string
								text=dataRoww["timeAskedToArrive"].ToString();
							}
						}
						break;
					case "WirelessPhone":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["wirelessPhone"].ToString();
						}
						break;
					case "WkPhone":
						if(isNote) {
							text="";
						}
						else {
							text=dataRoww["wkPhone"].ToString();
						}
						break;
					case "IsLate[L]":
						DateTime aptDateTime=PIn.DateT(dataRoww["AptDateTime"].ToString());
						DateTime aptDateTimeArrived=PIn.DateT(dataRoww["AptDateTimeArrived"].ToString());
						//If the appointment is scheduled, complete, or ASAP and the patient was late for the appointment.
						if((PIn.Long(dataRoww["AptStatus"].ToString())==(int)ApptStatus.Scheduled
							|| PIn.Long(dataRoww["AptStatus"].ToString())==(int)ApptStatus.Complete)
							&& ((aptDateTimeArrived.TimeOfDay==TimeSpan.FromHours(0) && DateTime.Now>aptDateTime) 
								|| (aptDateTimeArrived.TimeOfDay>TimeSpan.FromHours(0) && aptDateTimeArrived>aptDateTime))) 
						{
							text="L";
						}
						break;
				}
			#endregion
			object[] parameters={ dataRoww["PatNum"].ToString(),apptRows[elementI].PatFieldDefNum,text };
			Plugins.HookAddCode(null,"ApptSingleDrawing.DrawElement_afterFillText",parameters);
			text=(string)parameters[2];
			if(text=="" && !isGraphic) {
				return drawLoc;//next element will draw at the same position as this one would have.
			}
			SolidBrush brush=new SolidBrush(apptRows[elementI].ElementColor);
			SolidBrush brushWhite=new SolidBrush(Color.White);
			//SolidBrush noteTitlebrush = new SolidBrush(Defs.Long[(int)DefCat.AppointmentColors][8].ItemColor);
			StringFormat format=new StringFormat();
			format.Alignment=StringAlignment.Near;
			int charactersFitted;//not used, but required as 'out' param for measureString.
			int linesFilled;
			SizeF noteSize;
			RectangleF rect;
			RectangleF rectBack;
			#region Main
			if(align==ApptViewAlignment.Main) {//always stacks vertical
				if(isGraphic) {
					Bitmap bitmap=new Bitmap(12,12);
					noteSize=new SizeF(bitmap.Width,bitmap.Height);
					rect=new RectangleF(drawLoc,noteSize);
					using(Graphics gfx=Graphics.FromImage(bitmap)) {
						gfx.SmoothingMode=SmoothingMode.HighQuality;
						Color confirmColor=Defs.GetColor(DefCat.ApptConfirmed,PIn.Long(dataRoww["Confirmed"].ToString()));
						SolidBrush confirmBrush=new SolidBrush(confirmColor);
						gfx.FillEllipse(confirmBrush,0,0,11,11);
						gfx.DrawEllipse(Pens.Black,0,0,11,11);
						confirmBrush.Dispose();
					}
					g.DrawImage(bitmap,drawLoc.X,drawLoc.Y);
					DisposeObjects(brush,brushWhite,format,bitmap);
					return new Point(drawLoc.X,drawLoc.Y+(int)noteSize.Height);
				}
				else {
					//In rare scenarios the last character in some lines would get partially drawn (show as red / light gray).
					//By default the boundary rectangle returned by the MeasureString method excludes the space at the end of each line.
					//Make sure to use a string format with the MeasureTrailingSpaces flag as to correctly measure the width.
					noteSize=g.MeasureString(text,baseFont,(int)totalWidth-9,new StringFormat(StringFormatFlags.MeasureTrailingSpaces));
					//Problem: "limited-tooth bothering him ", the trailing space causes measuring error, resulting in m getting partially chopped off.
					//Tried TextRenderer, but it caused premature wrapping
					//Size noteSizeInt=TextRenderer.MeasureText(text,baseFont,new Size(totalWidth-9,1000));
					//noteSize=new SizeF(noteSizeInt.totalWidth,noteSizeInt.totalHeight);
					noteSize.Width=(float)Math.Ceiling((double)noteSize.Width);//round up to nearest int solves specific problem discussed above.
					if(drawLoc.Y+noteSize.Height>totalHeight && isPrinting) {
						//This keeps text from drawing off the appointment when font is large. Only if isPrinting cause not sure if this will cause bugs.
						//No need to do this check when drawing to the appt screen cause its just an image on a control which clips itself.
						noteSize.Height=totalHeight-drawLoc.Y;
					}
					SizeF stringSize=g.MeasureString(text,baseFont,noteSize,format,out charactersFitted,out linesFilled);
					if(isInsuranceColor) {
						if(dataRoww["insColor1"].ToString()!="") {
							Color color=Color.FromArgb(PIn.Int(dataRoww["insColor1"].ToString()));
							if(color!=Color.Black) {
								Point pt=new Point(drawLoc.X,drawLoc.Y+2);
								SizeF size=new SizeF(totalWidth,(linesFilled==0 ? linesFilled : stringSize.Height/linesFilled)); //avoid division by 0
								size.Height-=1;
								RectangleF rectF=new RectangleF(pt,size);
								g.FillRectangle(new SolidBrush(color),rectF);
							}
						}
						if(dataRoww["insColor2"].ToString()!="") {
							Color color=Color.FromArgb(PIn.Int(dataRoww["insColor2"].ToString()));
							if(color!=Color.Black) {
								Point pt=new Point(drawLoc.X,drawLoc.Y+2+(int)(linesFilled==0 ? linesFilled : noteSize.Height/linesFilled)); //avoid division by 0
								SizeF size=new SizeF(totalWidth,(linesFilled==0 ? linesFilled : stringSize.Height/linesFilled)); //avoid division by 0
								size.Height-=1;
								RectangleF rectF=new RectangleF(pt,size);
								g.FillRectangle(new SolidBrush(color),rectF);
							}
						}
					}
					rect=new RectangleF(drawLoc,noteSize);
					g.DrawString(text,baseFont,brush,rect,format);
					DisposeObjects(brush,brushWhite,format);
					return new Point(drawLoc.X,drawLoc.Y+linesFilled*ApptDrawing.LineH);
				}
			}
			#endregion
			#region UR
			else if(align==ApptViewAlignment.UR) {
				if(stackBehavior==ApptViewStackBehavior.Vertical) {
					float w=totalWidth-9;
					if(isGraphic) {
						Bitmap bitmap=new Bitmap(12,12);
						noteSize=new SizeF(bitmap.Width,bitmap.Height);
						Point drawLocThis=new Point(drawLoc.X-(int)noteSize.Width,drawLoc.Y+1);//upper left corner of this element
						rect=new RectangleF(drawLoc,noteSize);
						using(Graphics gfx=Graphics.FromImage(bitmap)) {
							gfx.SmoothingMode=SmoothingMode.HighQuality;
							Color confirmColor=Defs.GetColor(DefCat.ApptConfirmed,PIn.Long(dataRoww["Confirmed"].ToString()));
							SolidBrush confirmBrush=new SolidBrush(confirmColor);
							gfx.FillEllipse(confirmBrush,0,0,11,11);
							gfx.DrawEllipse(Pens.Black,0,0,11,11);
							confirmBrush.Dispose();
						}
						g.DrawImage(bitmap,drawLocThis.X,drawLocThis.Y);
						DisposeObjects(brush,brushWhite,format,bitmap);
						return new Point(drawLoc.X,drawLoc.Y+(int)noteSize.Height);
					}
					else {
						noteSize=g.MeasureString(text,baseFont,(int)w);
						noteSize=new SizeF(noteSize.Width,ApptDrawing.LineH+1);//only allowed to be one line high.
						if(noteSize.Width<5) {
							noteSize=new SizeF(5,noteSize.Height);
						}
						//g.MeasureString(text,baseFont,noteSize,format,out charactersFitted,out linesFilled);
						Point drawLocThis=new Point(drawLoc.X-(int)noteSize.Width,drawLoc.Y);//upper left corner of this element
						rect=new RectangleF(drawLocThis,noteSize);
						rectBack=new RectangleF(drawLocThis.X,drawLocThis.Y+1,noteSize.Width,ApptDrawing.LineH);
						if(apptRows[elementI].ElementDesc=="MedOrPremed[+]"
							|| apptRows[elementI].ElementDesc=="HasIns[I]"
							|| apptRows[elementI].ElementDesc=="InsToSend[!]") {
							g.FillRectangle(brush,rectBack);
							g.DrawString(text,baseFont,Brushes.Black,rect,format);
						}
						else {
							g.FillRectangle(brushWhite,rectBack);
							g.DrawString(text,baseFont,brush,rect,format);
						}
						g.DrawRectangle(Pens.Black,rectBack.X,rectBack.Y,rectBack.Width,rectBack.Height);
						DisposeObjects(brush,brushWhite,format);
						return new Point(drawLoc.X,drawLoc.Y+ApptDrawing.LineH);//move down a certain number of lines for next element.
					}
				}
				else {//horizontal
					int w=drawLoc.X-9;//drawLoc is upper right of each element.  The first element draws at (totalWidth-1,0).
					if(isGraphic) {
						Bitmap bitmap=new Bitmap(12,12);
						noteSize=new SizeF(bitmap.Width,bitmap.Height);
						Point drawLocThis=new Point(drawLoc.X-(int)noteSize.Width,drawLoc.Y+1);//upper left corner of this element
						rect=new RectangleF(drawLoc,noteSize);
						using(Graphics gfx=Graphics.FromImage(bitmap)) {
							gfx.SmoothingMode=SmoothingMode.HighQuality;
							Color confirmColor=Defs.GetColor(DefCat.ApptConfirmed,PIn.Long(dataRoww["Confirmed"].ToString()));
							SolidBrush confirmBrush=new SolidBrush(confirmColor);
							gfx.FillEllipse(confirmBrush,0,0,11,11);
							gfx.DrawEllipse(Pens.Black,0,0,11,11);
							confirmBrush.Dispose();
						}
						g.DrawImage(bitmap,drawLocThis.X,drawLocThis.Y);
						DisposeObjects(brush,brushWhite,format,bitmap);
						return new Point(drawLoc.X-(int)noteSize.Width-2,drawLoc.Y);
					}
					else {
						noteSize=g.MeasureString(text,baseFont,w);
						noteSize=new SizeF(noteSize.Width,ApptDrawing.LineH+1);//only allowed to be one line high.  Needs an extra pixel.
						if(noteSize.Width<5) {
							noteSize=new SizeF(5,noteSize.Height);
						}
						Point drawLocThis=new Point(drawLoc.X-(int)noteSize.Width,drawLoc.Y);//upper left corner of this element
						rect=new RectangleF(drawLocThis,noteSize);
						rectBack=new RectangleF(drawLocThis.X,drawLocThis.Y+1,noteSize.Width,ApptDrawing.LineH);
						if(apptRows[elementI].ElementDesc=="MedOrPremed[+]"
							|| apptRows[elementI].ElementDesc=="HasIns[I]"
							|| apptRows[elementI].ElementDesc=="InsToSend[!]") {
							g.FillRectangle(brush,rectBack);
							g.DrawString(text,baseFont,Brushes.Black,rect,format);
						}
						else {
							g.FillRectangle(brushWhite,rectBack);
							g.DrawString(text,baseFont,brush,rect,format);
						}
						g.DrawRectangle(Pens.Black,rectBack.X,rectBack.Y,rectBack.Width,rectBack.Height);
						DisposeObjects(brush,brushWhite,format);
						return new Point(drawLoc.X-(int)noteSize.Width-1,drawLoc.Y);//Move to left.  Might also have to subtract a little from x to space out elements.
					}
				}
			}
			#endregion
			#region LR
			else {//LR
				if(stackBehavior==ApptViewStackBehavior.Vertical) {
					float w=totalWidth-9;
					if(isGraphic) {
						Bitmap bitmap=new Bitmap(12,12);
						noteSize=new SizeF(bitmap.Width,bitmap.Height);
						Point drawLocThis=new Point(drawLoc.X-(int)noteSize.Width,drawLoc.Y+1-ApptDrawing.LineH);//upper left corner of this element
						rect=new RectangleF(drawLoc,noteSize);
						using(Graphics gfx=Graphics.FromImage(bitmap)) {
							gfx.SmoothingMode=SmoothingMode.HighQuality;
							Color confirmColor=Defs.GetColor(DefCat.ApptConfirmed,PIn.Long(dataRoww["Confirmed"].ToString()));
							SolidBrush confirmBrush=new SolidBrush(confirmColor);
							gfx.FillEllipse(confirmBrush,0,0,11,11);
							gfx.DrawEllipse(Pens.Black,0,0,11,11);
							confirmBrush.Dispose();
						}
						g.DrawImage(bitmap,drawLocThis.X,drawLocThis.Y);
						DisposeObjects(brush,brushWhite,format,bitmap);
						return new Point(drawLoc.X,drawLoc.Y-(int)noteSize.Height);
					}
					else {
						noteSize=g.MeasureString(text,baseFont,(int)w);
						noteSize=new SizeF(noteSize.Width,ApptDrawing.LineH+1);//only allowed to be one line high.  Needs an extra pixel.
						if(noteSize.Width<5) {
							noteSize=new SizeF(5,noteSize.Height);
						}
						//g.MeasureString(text,baseFont,noteSize,format,out charactersFitted,out linesFilled);
						Point drawLocThis=new Point(drawLoc.X-(int)noteSize.Width,drawLoc.Y-ApptDrawing.LineH);//upper left corner of this element
						rect=new RectangleF(drawLocThis,noteSize);
						rectBack=new RectangleF(drawLocThis.X,drawLocThis.Y+1,noteSize.Width,ApptDrawing.LineH);
						if(apptRows[elementI].ElementDesc=="MedOrPremed[+]"
							|| apptRows[elementI].ElementDesc=="HasIns[I]"
							|| apptRows[elementI].ElementDesc=="InsToSend[!]") {
							g.FillRectangle(brush,rectBack);
							g.DrawString(text,baseFont,Brushes.Black,rect,format);
						}
						else {
							g.FillRectangle(brushWhite,rectBack);
							g.DrawString(text,baseFont,brush,rect,format);
						}
						g.DrawRectangle(Pens.Black,rectBack.X,rectBack.Y,rectBack.Width,rectBack.Height);
						DisposeObjects(brush,brushWhite,format);
						return new Point(drawLoc.X,drawLoc.Y-ApptDrawing.LineH);//move up a certain number of lines for next element.
					}
				}
				else {//horizontal
					int w=drawLoc.X-9;//drawLoc is upper right of each element.  The first element draws at (totalWidth-1,0).
					if(isGraphic) {
						Bitmap bitmap=new Bitmap(12,12);
						noteSize=new SizeF(bitmap.Width,bitmap.Height);
						Point drawLocThis=new Point(drawLoc.X-(int)noteSize.Width+1,drawLoc.Y+1-ApptDrawing.LineH);//upper left corner of this element
						rect=new RectangleF(drawLoc,noteSize);
						using(Graphics gfx=Graphics.FromImage(bitmap)) {
							gfx.SmoothingMode=SmoothingMode.HighQuality;
							Color confirmColor=Defs.GetColor(DefCat.ApptConfirmed,PIn.Long(dataRoww["Confirmed"].ToString()));
							SolidBrush confirmBrush=new SolidBrush(confirmColor);
							gfx.FillEllipse(confirmBrush,0,0,11,11);
							gfx.DrawEllipse(Pens.Black,0,0,11,11);
							confirmBrush.Dispose();
						}
						g.DrawImage(bitmap,drawLocThis.X,drawLocThis.Y);
						DisposeObjects(brush,brushWhite,format,bitmap);
						return new Point(drawLoc.X-(int)noteSize.Width-1,drawLoc.Y);
					}
					else {
						noteSize=g.MeasureString(text,baseFont,w);
						noteSize=new SizeF(noteSize.Width,ApptDrawing.LineH+1);//only allowed to be one line high.  Needs an extra pixel.
						if(noteSize.Width<5) {
							noteSize=new SizeF(5,noteSize.Height);
						}
						Point drawLocThis=new Point(drawLoc.X-(int)noteSize.Width,drawLoc.Y-ApptDrawing.LineH);//upper left corner of this element
						rect=new RectangleF(drawLocThis,noteSize);
						rectBack=new RectangleF(drawLocThis.X,drawLocThis.Y+1,noteSize.Width,ApptDrawing.LineH);
						if(apptRows[elementI].ElementDesc=="MedOrPremed[+]"
							|| apptRows[elementI].ElementDesc=="HasIns[I]"
							|| apptRows[elementI].ElementDesc=="InsToSend[!]") {
							g.FillRectangle(brush,rectBack);
							g.DrawString(text,baseFont,Brushes.Black,rect,format);
						}
						else {
							g.FillRectangle(brushWhite,rectBack);
							g.DrawString(text,baseFont,brush,rect,format);
						}
						g.DrawRectangle(Pens.Black,rectBack.X,rectBack.Y,rectBack.Width,rectBack.Height);
						DisposeObjects(brush,brushWhite,format);
						return new Point(drawLoc.X-(int)noteSize.Width-1,drawLoc.Y);//Move to left.  Subtract a little from x to space out elements.
					}
				}
			}
			#endregion
		}

		///<summary>This is only called when viewing appointments on the Appt module.  For Planned apt and pinboard, use SetSize instead so that the location won't change.  Pass 0 for startHour unless printing.  Pass visible ops for colsPerPage unless printing.  Pass 0 for pageColumn unless printing.</summary>
		public static Point SetLocation(DataRow dataRoww,int beginHour,int colsPerPage,int pageColumn) {
			if(ApptDrawing.IsWeeklyView) {
				ApptSingleWidth=(int)ApptDrawing.ColAptWidth;
				location=new Point(ConvertToX(dataRoww,colsPerPage,pageColumn),ConvertToY(dataRoww,beginHour));
			}
			else {
				location=new Point(ConvertToX(dataRoww,colsPerPage,pageColumn)+2,ConvertToY(dataRoww,beginHour));
				ApptSingleWidth=ApptDrawing.ColWidth-5;
			}
			return location;
		}

		///<summary>Used for Planned apt and pinboard instead of SetLocation so that the location won't be altered.</summary>
		public static Size SetSize(string pattern) {
			ApptSingleWidth=ApptDrawing.ColWidth-5;
			if(ApptDrawing.IsWeeklyView) {
				ApptSingleWidth=(int)ApptDrawing.ColAptWidth;
			}
			//height is based on original 5 minute pattern. Might result in half-rows
			ApptSingleHeight=pattern.Length*ApptDrawing.LineH*ApptDrawing.RowsPerIncr;
			if(ApptDrawing.MinPerIncr==10) {
				ApptSingleHeight=ApptSingleHeight/2;
			}
			if(ApptDrawing.MinPerIncr==15) {
				ApptSingleHeight=ApptSingleHeight/3;
			}
			return new Size((int)ApptSingleWidth,(int)ApptSingleHeight);
		}

		///<summary>Called from SetLocation to establish X position of control.</summary>
		public static int ConvertToX(DataRow dataRoww,int colsPerPage,int pageColumn) {
			if(ApptDrawing.IsWeeklyView) {
				//the next few lines are because we start on Monday instead of Sunday
				int dayofweek=(int)PIn.DateT(dataRoww["AptDateTime"].ToString()).DayOfWeek-1;
				if(dayofweek==-1) {
					dayofweek=6;
				}
				return (int)(ApptDrawing.TimeWidth
					+ApptDrawing.ColDayWidth*(dayofweek)+1
					+(ApptDrawing.ColAptWidth*(ApptDrawing.GetIndexOp(PIn.Long(dataRoww["Op"].ToString()))-(colsPerPage*pageColumn))));
			}
			else {
				return (int)(ApptDrawing.TimeWidth+ApptDrawing.ProvWidth*ApptDrawing.ProvCount
					+ApptDrawing.ColWidth*(ApptDrawing.GetIndexOp(PIn.Long(dataRoww["Op"].ToString()))-(colsPerPage*pageColumn))+1);
				//Info.MyApt.Op))+1;
			}
		}

		///<summary>Called from SetLocation to establish Y position of control.  Also called from ContrAppt.RefreshDay when determining ProvBar markings. Does not round to the nearest row.</summary>
		public static int ConvertToY(DataRow dataRoww,int beginHour) {
			DateTime aptDateTime=PIn.DateT(dataRoww["AptDateTime"].ToString());
			int retVal=(int)(((double)(aptDateTime.Hour-beginHour)*(double)60
				/(double)ApptDrawing.MinPerIncr
				+(double)aptDateTime.Minute
				/(double)ApptDrawing.MinPerIncr
				)*(double)ApptDrawing.LineH*ApptDrawing.RowsPerIncr);
			return retVal;
		}

		///<summary>This converts the dbPattern in 5 minute interval into the pattern that will be viewed based on RowsPerIncrement and AppointmentTimeIncrement.  So it will always depend on the current view.Therefore, it should only be used for visual display purposes rather than within the FormAptEdit. If height of appointment allows a half row, then this includes an increment for that half row.</summary>
		public static string GetPatternShowing(string dbPattern) {
			StringBuilder strBTime=new StringBuilder();
			for(int i=0;i<dbPattern.Length;i++) {
				for(int j=0;j<ApptDrawing.RowsPerIncr;j++) {
					strBTime.Append(dbPattern.Substring(i,1));
				}
				if(ApptDrawing.MinPerIncr==10) {
					i++;//skip
				}
				if(ApptDrawing.MinPerIncr==15) {
					i++;
					i++;//skip two
				}
			}
			return strBTime.ToString();
		}

		///<summary>Tests if the appt is in the allotted time frame and is in a visible operatory.  Returns false in order to skip drawing for apptointment printing.</summary>
		public static bool ApptWithinTimeFrame(long opNum,DateTime aptDateTime,string pattern,DateTime beginTime,DateTime endTime,int colsPerPage,int pageColumn) {
			//Test if appts op is currently visible.
			bool visible=false;
			if(ApptDrawing.IsWeeklyView) {
				if(ApptDrawing.GetIndexOp(opNum) > -1) {
					visible=true;
				}
			}
			else {//Daily view
				for(int i=0;i<colsPerPage;i++) {
					if(i==ApptDrawing.VisOps.Count) {
						return false;
					}
					int k=colsPerPage*pageColumn+i;
					if(k>=ApptDrawing.VisOps.Count) {
						return false;
					}
					if(k==ApptDrawing.GetIndexOp(opNum)) {
						visible=true;
						break;
					}
				}
			}
			if(!visible) {//Op not visible so don't test time frame.
				return false;
			}
			//Test if any portion of appt is within time frame.
			TimeSpan aptTimeBegin=aptDateTime.TimeOfDay;
			TimeSpan aptTimeEnd=aptTimeBegin.Add(new TimeSpan(0,pattern.Length*5,0));
			int aptHourBegin=aptTimeBegin.Hours;
			int aptHourEnd=aptTimeEnd.Hours;
			if(aptHourEnd==0) {
				aptHourEnd=24;
			}
			int beginHour=beginTime.Hour;
			int endHour=endTime.Hour;
			if(endHour==0) {
				endHour=24;
			}
			//If the appointment begins on or after the stopping hour (because we don't support minutes currently) then this appointment is not visible.
			//However, we need to check the time portion of the appointment ending time in correlation to the begin hour 
			//because the appointment could end within the same hour that the printing begin hour is set to.
			//E.g. an appointment from 8 AM to 8:40 AM needs to show as visible when printing a schedule from 8 AM to 5 PM.
			TimeSpan timePrintBegin=new TimeSpan(beginHour,0,0);
			if(aptHourBegin>=endHour || aptTimeEnd<=timePrintBegin) {
				return false;
			}
			return true;
		}

		///<summary>Disposes objects with typeof Brush, Pen, StringFormat or Bitmap.</summary>
		private static void DisposeObjects(params object[] disposables) {
			for(int i=0;i<disposables.Length;i++) {
				if(disposables[i]==null) {
					continue;
				}
				if(disposables[i].GetType()==typeof(Brush)) {
					Brush b=(Brush)disposables[i];
					b.Dispose();
				}
				else if(disposables[i].GetType()==typeof(Pen)) {
					Pen p=(Pen)disposables[i];
					p.Dispose();
				}
				else if(disposables[i].GetType()==typeof(StringFormat)) {
					StringFormat sf=(StringFormat)disposables[i];
					sf.Dispose();
				}
				else if(disposables[i].GetType()==typeof(Bitmap)) {
					Bitmap bmp=(Bitmap)disposables[i];
					bmp.Dispose();
				}
			}
		}

	}
}
