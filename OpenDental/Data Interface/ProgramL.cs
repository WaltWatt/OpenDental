using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using OpenDental.Bridges;
using OpenDentBusiness;
using OpenDental.UI;
using System.Drawing;
using System.IO;

namespace OpenDental{
	

	///<summary></summary>
	public class ProgramL{

		///<summary>Typically used when user clicks a button to a Program link.  This method attempts to identify and execute the program based on the given programNum.</summary>
		public static void Execute(long programNum,Patient pat) {
			Program prog=Programs.GetFirstOrDefault(x => x.ProgramNum==programNum);
			if(prog==null) {//no match was found
				MessageBox.Show("Error, program entry not found in database.");
				return;
			}
			if(pat!=null && PrefC.GetBool(PrefName.ShowFeaturePatientClone)) {
				pat=Patients.GetOriginalPatientForClone(pat);
			}
			if(prog.PluginDllName!="") {
				if(pat==null) {
					Plugins.LaunchToolbarButton(programNum,0);
				}
				else{
					Plugins.LaunchToolbarButton(programNum,pat.PatNum);
				}
				return;
			}
			if(prog.ProgName==ProgramName.ActeonImagingSuite.ToString()) {
				ActeonImagingSuite.SendData(prog,pat);
				return;
			}
			if(prog.ProgName==ProgramName.Adstra.ToString()) {
				Adstra.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Apixia.ToString()) {
				Apixia.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Apteryx.ToString()) {
				Apteryx.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.AudaxCeph.ToString()) {
				AudaxCeph.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.BioPAK.ToString()) {
				BioPAK.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.CADI.ToString()) {
				CADI.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Camsight.ToString()) {
				Camsight.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.CaptureLink.ToString()) {
				CaptureLink.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Carestream.ToString()) {
				Carestream.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Cerec.ToString()) {
				Cerec.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.CleaRay.ToString()) {
				CleaRay.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.CliniView.ToString()) {
				Cliniview.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.ClioSoft.ToString()) {
				ClioSoft.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.DBSWin.ToString()) {
				DBSWin.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.DemandForce.ToString()) {
				DemandForce.SendData(prog,pat);
				return;
			}
#if !DISABLE_WINDOWS_BRIDGES
			else if(prog.ProgName==ProgramName.DentalEye.ToString()) {
				DentalEye.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.DentalStudio.ToString()) {
				DentalStudio.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.DentX.ToString()) {
				DentX.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.DrCeph.ToString()) {
				DrCeph.SendData(prog,pat);
				return;
			}
#endif
			else if(prog.ProgName==ProgramName.DentalTekSmartOfficePhone.ToString()) {
				DentalTek.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.DentForms.ToString()) {
				DentForms.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Dexis.ToString()) {
				Dexis.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Digora.ToString()) {
				Digora.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Dimaxis.ToString()) {
				Planmeca.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Office.ToString()) {
				Office.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Dolphin.ToString()) {
				Dolphin.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Dxis.ToString()) {
				Dxis.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.EvaSoft.ToString()) {
				EvaSoft.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.EwooEZDent.ToString()) {
				Ewoo.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.FloridaProbe.ToString()) {
				FloridaProbe.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Guru.ToString()) {
				Guru.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.HandyDentist.ToString()) {
				HandyDentist.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.HouseCalls.ToString()) {
				FormHouseCalls FormHC=new FormHouseCalls();
				FormHC.ProgramCur=prog;
				FormHC.ShowDialog();
				return;
			}
			else if(prog.ProgName==ProgramName.iCat.ToString()) {
				ICat.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.HdxWill.ToString()) {
				HdxWill.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.iDixel.ToString()) {
				iDixel.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.ImageFX.ToString()) {
				ImageFX.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.iRYS.ToString()) {
				Irys.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Lightyear.ToString()) {
				Lightyear.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.NewTomNNT.ToString()) {
				NewTomNNT.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.MediaDent.ToString()) {
				MediaDent.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.MiPACS.ToString()) {
				MiPACS.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.OrthoCAD.ToString()) {
				OrthoCad.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.OrthoInsight3d.ToString()) {
				OrthoInsight3d.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Owandy.ToString()) {
				Owandy.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.PandaPerio.ToString()) {
				PandaPerio.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Patterson.ToString()) {
				Patterson.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.PerioPal.ToString()) {
				PerioPal.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.PracticeByNumbers.ToString()) {
				PracticeByNumbers.ShowPage();
				return;
			}
			else if(prog.ProgName==ProgramName.Progeny.ToString()) {
				Progeny.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.PT.ToString()) {
				PaperlessTechnology.SendData(prog,pat,false);
				return;
			}
			else if(prog.ProgName==ProgramName.PTupdate.ToString()) {
				PaperlessTechnology.SendData(prog,pat,true);
				return;
			}
			else if(prog.ProgName==ProgramName.RayMage.ToString()) {
				RayMage.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Romexis.ToString()) {
				Romexis.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Scanora.ToString()) {
				Scanora.SendData(prog,pat);
				return;
			}
#if !DISABLE_WINDOWS_BRIDGES
			else if(prog.ProgName==ProgramName.Schick.ToString()) {
				Schick.SendData(prog,pat);
				return;
			}
#endif
			else if(prog.ProgName==ProgramName.Sirona.ToString()) {
				Sirona.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.SMARTDent.ToString()) {
				SmartDent.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Sopro.ToString()) {
				Sopro.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.TigerView.ToString()) {
				TigerView.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Triana.ToString()) {
				Triana.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Trophy.ToString()) {
				Trophy.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.TrophyEnhanced.ToString()) {
				TrophyEnhanced.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.Tscan.ToString()) {
				Tscan.SendData(prog,pat);
				return;
			}
#if !DISABLE_WINDOWS_BRIDGES
			else if(prog.ProgName==ProgramName.Vipersoft.ToString()) {
				Vipersoft.SendData(prog,pat);
				return;
			}
#endif
			else if(prog.ProgName==ProgramName.visOra.ToString()) {
				Visora.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.VistaDent.ToString()) {
				VistaDent.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.VixWin.ToString()) {
				VixWin.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.VixWinBase36.ToString()) {
				VixWinBase36.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.VixWinBase41.ToString()) {
				VixWinBase41.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.VixWinNumbered.ToString()) {
				VixWinNumbered.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.VixWinOld.ToString()) {
				VixWinOld.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.XDR.ToString()) {
				Dexis.SendData(prog,pat);//XDR uses the Dexis protocol
				return;
			}
			else if(prog.ProgName==ProgramName.XVWeb.ToString()) {
				XVWeb.SendData(prog,pat);
				return;
			}
			else if(prog.ProgName==ProgramName.ZImage.ToString()) {
				ZImage.SendData(prog,pat);
				return;
			}
			//all remaining programs:
			try{
				string cmdline=prog.CommandLine;
				string path=Programs.GetProgramPath(prog);
				string outputFilePath=prog.FilePath;
				string fileTemplate=prog.FileTemplate;
				if(pat!=null) {
					cmdline=Patients.ReplacePatient(cmdline,pat);
					path=Patients.ReplacePatient(path,pat);
					if(!String.IsNullOrEmpty(outputFilePath) && !String.IsNullOrEmpty(fileTemplate)) {
						fileTemplate=Patients.ReplacePatient(fileTemplate,pat);
						fileTemplate=fileTemplate.Replace("\n","\r\n");
						File.WriteAllText(outputFilePath,fileTemplate);
					}
				}
				Process.Start(path,cmdline);
			}
			catch{
				MessageBox.Show(prog.ProgDesc+" is not available.");
				return;
			}
		}

		public static void LoadToolbar(ODToolBar ToolBarMain,ToolBarsAvail toolBarsAvail) {
			List<ToolButItem> toolButItems=ToolButItems.GetForToolBar(toolBarsAvail);
			for(int i=0;i<toolButItems.Count;i++) {
				ToolButItem toolButItemCur=((ToolButItem)toolButItems[i]);
				Program programCur=Programs.GetProgram(toolButItemCur.ProgramNum);
				string key=programCur.ProgramNum.ToString()+programCur.ProgName.ToString();
				if(ToolBarMain.ImageList.Images.ContainsKey(key)) {
					//Dispose the existing image only if it already exists, because the image might have changed.
					ToolBarMain.ImageList.Images[ToolBarMain.ImageList.Images.IndexOfKey(key)].Dispose();
					ToolBarMain.ImageList.Images.RemoveByKey(key);
				}
				if(programCur.ButtonImage!="") {
					Image image=PIn.Bitmap(programCur.ButtonImage);
					ToolBarMain.ImageList.Images.Add(key,image);
				}
				if(toolBarsAvail!=ToolBarsAvail.MainToolbar) {
					ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
				}
				ToolBarMain.Buttons.Add(new ODToolBarButton(toolButItemCur.ButtonText,-1,"",programCur));
			}
			for(int i=0;i<ToolBarMain.Buttons.Count;i++) {//Reset the new index, because it might have changed due to removing/adding to the Images list.
				if(ToolBarMain.Buttons[i].Tag.GetType()!=typeof(Program)) {
					continue;
				}
				Program programCur=(Program)ToolBarMain.Buttons[i].Tag;
				string key=programCur.ProgramNum.ToString()+programCur.ProgName.ToString();
				if(ToolBarMain.ImageList.Images.ContainsKey(key)) {
					ToolBarMain.Buttons[i].ImageIndex=ToolBarMain.ImageList.Images.IndexOfKey(key);
				}
			}
		}

	}
}