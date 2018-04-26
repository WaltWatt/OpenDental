﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using SparksToothChart;

namespace OpenDental {
	public partial class FormPerioGraphical:ODForm {
		public PerioExam PerioExamCur;
		public Patient PatCur;
		//public List<PerioMeasure> ListPerioMeasures; 

		public FormPerioGraphical(PerioExam perioExam,Patient patient,ToothChartData toothChartData) {
			PerioExamCur=perioExam;
			PatCur=patient;
			InitializeComponent();
			//ComputerPref localComputerPrefs=ComputerPrefs.GetForLocalComputer();
			toothChart.DeviceFormat=new ToothChartDirectX.DirectXDeviceFormat(ComputerPrefs.LocalComputer.DirectXFormat);//Must be set before draw mode
			toothChart.DrawMode=DrawingMode.DirectX;
			toothChart.SetToothNumberingNomenclature((ToothNumberingNomenclature)PrefC.GetInt(PrefName.UseInternationalToothNumbers));
			toothChart.ColorBackground=Color.White;
			toothChart.ColorText=Color.Black;
			toothChart.PerioMode=true;
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.MiscColors,true);
			toothChart.ColorBleeding=listDefs[1].ItemColor;
			toothChart.ColorSuppuration=listDefs[2].ItemColor;
			toothChart.ColorProbing=PrefC.GetColor(PrefName.PerioColorProbing);
			toothChart.ColorProbingRed=PrefC.GetColor(PrefName.PerioColorProbingRed);
			toothChart.ColorGingivalMargin=PrefC.GetColor(PrefName.PerioColorGM);
			toothChart.ColorCAL=PrefC.GetColor(PrefName.PerioColorCAL);
			toothChart.ColorMGJ=PrefC.GetColor(PrefName.PerioColorMGJ);
			toothChart.ColorFurcations=PrefC.GetColor(PrefName.PerioColorFurcations);
			toothChart.ColorFurcationsRed=PrefC.GetColor(PrefName.PerioColorFurcationsRed);
			toothChart.RedLimitProbing=PrefC.GetInt(PrefName.PerioRedProb);
			toothChart.RedLimitFurcations=PrefC.GetInt(PrefName.PerioRedFurc);
			List<PerioMeasure> listMeas=PerioMeasures.GetAllForExam(PerioExamCur.PerioExamNum);
			//compute CAL's for each site.  If a CAL is valid, pass it in.
			PerioMeasure measureProbe;
			PerioMeasure measureGM;
			int gm;
			int pd;
			int calMB;
			int calB;
			int calDB;
			int calML;
			int calL;
			int calDL;
			for(int t=1;t<=32;t++) {
				measureProbe=null;
				measureGM=null;
				for(int i=0;i<listMeas.Count;i++) {
					if(listMeas[i].IntTooth!=t) {
						continue;
					}
					if(listMeas[i].SequenceType==PerioSequenceType.Probing) {
						measureProbe=listMeas[i];
					}
					if(listMeas[i].SequenceType==PerioSequenceType.GingMargin) {
						measureGM=listMeas[i];
					}
				}
				if(measureProbe==null||measureGM==null) {
					continue;//to the next tooth
				}
				//mb
				calMB=-1;
				gm=measureGM.MBvalue;//MBvalue must stay over 100 for hyperplasia, because that's how we're storing it in ToothChartData.ListPerioMeasure.
				if(gm>100) {//hyperplasia
					gm=100-gm;//e.g. 100-103=-3
				}
				pd=measureProbe.MBvalue;
				if(measureGM.MBvalue!=-1 && pd!=-1) {
					calMB=gm+pd;
					if(calMB<0) {
						calMB=0;//CALs can't be negative
					}
				}
				//B
				calB=-1;
				gm=measureGM.Bvalue;
				if(gm>100) {//hyperplasia
					gm=100-gm;//e.g. 100-103=-3 
				}
				pd=measureProbe.Bvalue;
				if(measureGM.Bvalue!=-1&&pd!=-1) {
					calB=gm+pd;
					if(calB<0) {
						calB=0;
					}
				}
				//DB
				calDB=-1;
				gm=measureGM.DBvalue;
				if(gm>100) {//hyperplasia
					gm=100-gm;//e.g. 100-103=-3 
				}
				pd=measureProbe.DBvalue;
				if(measureGM.DBvalue!=-1&&pd!=-1) {
					calDB=gm+pd;
					if(calDB<0) {
						calDB=0;
					}
				}
				//ML
				calML=-1;
				gm=measureGM.MLvalue;
				if(gm>100) {//hyperplasia
					gm=100-gm;//e.g. 100-103=-3 
				}
				pd=measureProbe.MLvalue;
				if(measureGM.MLvalue!=-1&&pd!=-1) {
					calML=gm+pd;
					if(calML<0) {
						calML=0;
					}
				}
				//L
				calL=-1;
				gm=measureGM.Lvalue;
				if(gm>100) {//hyperplasia
					gm=100-gm;//e.g. 100-103=-3 
				}
				pd=measureProbe.Lvalue;
				if(measureGM.Lvalue!=-1&&pd!=-1) {
					calL=gm+pd;
					if(calL<0) {
						calL=0;
					}
				}
				//DL
				calDL=-1;
				gm=measureGM.DLvalue;
				if(gm>100) {//hyperplasia
					gm=100-gm;//e.g. 100-103=-3 
				}
				pd=measureProbe.DLvalue;
				if(measureGM.DLvalue!=-1&&pd!=-1) {
					calDL=gm+pd;
					if(calDL<0) {
						calDL=0;
					}
				}
				if(calMB!=-1||calB!=-1||calDB!=-1||calML!=-1||calL!=-1||calDL!=-1){
					toothChart.AddPerioMeasure(t,PerioSequenceType.CAL,calMB,calB,calDB,calML,calL,calDL);
				}
			}
			for(int i=0;i<listMeas.Count;i++) {
				if(listMeas[i].SequenceType==PerioSequenceType.SkipTooth) {
					toothChart.SetMissing(listMeas[i].IntTooth.ToString());
				} 
				else if(listMeas[i].SequenceType==PerioSequenceType.Mobility) {
					int mob=listMeas[i].ToothValue;
					Color color=Color.Black;
					if(mob>=PrefC.GetInt(PrefName.PerioRedMob)) {
						color=Color.Red;
					}
					if(mob!=-1) {//-1 represents no measurement taken.
						toothChart.SetMobility(listMeas[i].IntTooth.ToString(),mob.ToString(),color);
					}
				} 
				else {
					toothChart.AddPerioMeasure(listMeas[i]);
				}
			}


			/*
			toothChart.SetMissing("13");
			toothChart.SetMissing("14");
			toothChart.SetMissing("18");
			toothChart.SetMissing("25");
			toothChart.SetMissing("26");
			toothChart.SetImplant("14",Color.Gray);
			//Movements are too low of a priority to test right now.  We might not even want to implement them.
			//toothChart.MoveTooth("4",0,0,0,0,-5,0);
			//toothChart.MoveTooth("16",0,20,0,-3,0,0);
			//toothChart.MoveTooth("24",15,2,0,0,0,0);
			toothChart.SetMobility("2","3",Color.Red);
			toothChart.SetMobility("7","2",Color.Red);
			toothChart.SetMobility("8","2",Color.Red);
			toothChart.SetMobility("9","2",Color.Red);
			toothChart.SetMobility("10","2",Color.Red);
			toothChart.SetMobility("16","3",Color.Red);
			toothChart.SetMobility("24","2",Color.Red);
			toothChart.SetMobility("31","3",Color.Red);
			toothChart.AddPerioMeasure(1,PerioSequenceType.Furcation,-1,2,-1,1,-1,-1);
			toothChart.AddPerioMeasure(2,PerioSequenceType.Furcation,-1,2,-1,1,-1,-1);
			toothChart.AddPerioMeasure(3,PerioSequenceType.Furcation,-1,2,-1,1,-1,-1);
			toothChart.AddPerioMeasure(5,PerioSequenceType.Furcation,1,-1,-1,-1,-1,-1);
			toothChart.AddPerioMeasure(30,PerioSequenceType.Furcation,-1,-1,-1,-1,3,-1);
			for(int i=1;i<=32;i++) {
				//string tooth_id=i.ToString();
				//bleeding and suppuration on all MB sites
				//bleeding only all DL sites
				//suppuration only all B sites
				//blood=1, suppuration=2, both=3
				toothChart.AddPerioMeasure(i,PerioSequenceType.Bleeding,  3,2,-1,-1,-1,1);
				toothChart.AddPerioMeasure(i,PerioSequenceType.GingMargin,0,1,1,1,0,0);
				toothChart.AddPerioMeasure(i,PerioSequenceType.Probing,   3,2,3,4,2,3);
				toothChart.AddPerioMeasure(i,PerioSequenceType.CAL,       3,3,4,5,2,3);//basically GingMargin+Probing, unless one of them is -1
				toothChart.AddPerioMeasure(i,PerioSequenceType.MGJ,       5,5,5,6,6,6);
			}*/
			for(int i=0;i<toothChartData.ListToothGraphics.Count;i++) {
				if(!toothChartData.ListToothGraphics[i].IsImplant) {//Only care about implants currently.
					continue;
				}
				for(int j=0;j<toothChart.TcData.ListToothGraphics.Count;j++) {
					if(toothChart.TcData.ListToothGraphics[j].ToothID==toothChartData.ListToothGraphics[i].ToothID) {
						toothChart.TcData.ListToothGraphics[j]=toothChartData.ListToothGraphics[i].Copy();
						break;
					}
				}
			}
		}

		private void FormPerioGraphic_Load(object sender,EventArgs e) {
			
		}

		private void butPrint_Click(object sender,EventArgs e) {
			PrintDocument pd2=new PrintDocument();
			pd2.PrintPage+=new PrintPageEventHandler(this.pd2_PrintPage);
			pd2.OriginAtMargins=true;
			pd2.DefaultPageSettings.Margins=new Margins(0,0,0,0);
			if(!PrinterL.SetPrinter(pd2,PrintSituation.TPPerio,PatCur.PatNum,"Graphical perio chart printed")) {
				return;
			}
			pd2.Print();
		}

		private void pd2_PrintPage(object sender,PrintPageEventArgs ev) {//raised for each page to be printed.
			Graphics g=ev.Graphics;
			RenderPerioPrintout(g,PatCur,ev.MarginBounds);//,new Rectangle(0,50,ev.MarginBounds.Width,ev.MarginBounds.Height));
		}

		public void RenderPerioPrintout(Graphics g,Patient pat,Rectangle marginBounds) {
			string clinicName="";
			//This clinic name could be more accurate here in the future if we make perio exams clinic specific.
			//Perhaps if there were a perioexam.ClinicNum column.
			if(pat.ClinicNum!=0) {
				Clinic clinic=Clinics.GetClinic(pat.ClinicNum);
				clinicName=clinic.Description;
			} 
			else {
				clinicName=PrefC.GetString(PrefName.PracticeTitle);
			}
			float y=70;
			SizeF sizeFPage=new SizeF(marginBounds.Width,marginBounds.Height);
			SizeF sizeStr;
			Font font=new Font("Arial",15);
			string titleStr="PERIODONTAL EXAMINATION";
			sizeStr=g.MeasureString(titleStr,font);
			g.DrawString(titleStr,font,Brushes.Black,new PointF(sizeFPage.Width/2f-sizeStr.Width/2f,y));
			y+=sizeStr.Height;
			font=new Font("Arial",11);
			sizeStr=g.MeasureString(clinicName,font);
			g.DrawString(clinicName,font,Brushes.Black,new PointF(sizeFPage.Width/2f-sizeStr.Width/2f,y));
			y+=sizeStr.Height;
			string patNameStr=PatCur.GetNameFLFormal();
			sizeStr=g.MeasureString(patNameStr,font);
			g.DrawString(patNameStr,font,Brushes.Black,new PointF(sizeFPage.Width/2f-sizeStr.Width/2f,y));
			y+=sizeStr.Height;
			string examDateStr=PerioExamCur.ExamDate.ToShortDateString();//Locale specific exam date.
			sizeStr=g.MeasureString(examDateStr,font);
			g.DrawString(examDateStr,font,Brushes.Black,new PointF(sizeFPage.Width/2f-sizeStr.Width/2f,y));
			y+=sizeStr.Height;
			Bitmap bitmapTC=toothChart.GetBitmap();
			g.DrawImage(bitmapTC,sizeFPage.Width/2f-bitmapTC.Width/2f,y,bitmapTC.Width,bitmapTC.Height);
		}

		private void butSetup_Click(object sender,EventArgs e) {
			FormPerioGraphicalSetup fpgs=new FormPerioGraphicalSetup();
			if(fpgs.ShowDialog()==DialogResult.OK){
				toothChart.ColorCAL=PrefC.GetColor(PrefName.PerioColorCAL);
				toothChart.ColorFurcations=PrefC.GetColor(PrefName.PerioColorFurcations);
				toothChart.ColorFurcationsRed=PrefC.GetColor(PrefName.PerioColorFurcationsRed);
				toothChart.ColorGingivalMargin=PrefC.GetColor(PrefName.PerioColorGM);
				toothChart.ColorMGJ=PrefC.GetColor(PrefName.PerioColorMGJ);	
				toothChart.ColorProbing=PrefC.GetColor(PrefName.PerioColorProbing);
				toothChart.ColorProbingRed=PrefC.GetColor(PrefName.PerioColorProbingRed);
				this.toothChart.Invalidate();
			}
		}

		private void butSave_Click(object sender,EventArgs e) {
			long defNumToothCharts=Defs.GetImageCat(ImageCategorySpecial.T);
			if(defNumToothCharts==0) {
				MsgBox.Show(this,"In Setup, Definitions, Image Categories, a category needs to be set for graphical tooth charts.");
				return;
			}
			Bitmap bitmap=null;
			Graphics g=null;
			Document doc=new Document();
			bitmap=new Bitmap(750,1000);
			g=Graphics.FromImage(bitmap);
			g.Clear(Color.White);
			g.CompositingQuality=System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			RenderPerioPrintout(g,PatCur,new Rectangle(0,0,bitmap.Width,bitmap.Height));
			try {
				ImageStore.Import(bitmap,defNumToothCharts,ImageType.Photo,PatCur);
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Unable to save file: ") + ex.Message);
				bitmap.Dispose();
				bitmap=null;
				g.Dispose();
				return;
			}
			MsgBox.Show(this,"Saved.");
			if(g!=null) {
				g.Dispose();
				g=null;
			}
			if(bitmap!=null) {
				bitmap.Dispose();
				bitmap=null;
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		private void FormPerioGraphical_FormClosing(object sender,FormClosingEventArgs e) {
			//We need to clear out the tooth graphics of the local toothchart, since they are shallow copies of the tooth chart in the Chart module.
			//Otherwise, when the form disposes, the Chart module tooth graphics would also be disposed.
			toothChart.TcData.ListToothGraphics.Clear();
		}

	}
}
