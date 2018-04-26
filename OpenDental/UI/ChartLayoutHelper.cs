using System.Drawing;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using SparksToothChart;
using System.Collections.Generic;

namespace OpenDental {
	public class ChartLayoutHelper {
		///<summary>This reduces the number of places where Programs.UsingEcwTight() is called.  This helps with organization.  All calls within this class must pass through here. </summary>
		private static bool UsingEcwTightOrFull() {
			return Programs.UsingEcwTightOrFullMode();
		}

		///<summary>If medical only, this will hide the tooth chart and fill the space with the treatement notes text box.  Not for eCWTightOrFull.
		///This is made public so that it can be called from ContrChart.</summary>
		public static void SetToothChartVisibleHelper(ToothChartWrapper toothChart,RichTextBox textTreatmentNotes,TrackBar trackBar,Label textTrack) {
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				toothChart.Visible=false;
				trackBar.Visible=false;
				textTrack.Visible=false;
				textTreatmentNotes.Location=new Point(0,toothChart.Top);
				textTreatmentNotes.Height=toothChart.Height+72;//textTreatmentNotes height is 71, +1 for distance between toothChart and textTreatmentNotes
			}
			else {
				toothChart.Visible=true;
				trackBar.Visible=true;
				textTrack.Visible=true;
				textTreatmentNotes.Location=new Point(0,trackBar.Bottom+1);
				textTreatmentNotes.Height=71;
			}
		}

		public static void Resize(ODGrid gridProg,Panel panelImages,Panel panelEcw,TabControl tabControlImages,Size ClientSize
			,ODGrid gridPtInfo,ToothChartWrapper toothChart,RichTextBox textTreatmentNotes,TrackBar trackBar,Label textTrackBarDate) 
		{
			if(Programs.HListIsNull()) {
				return;
			}
			if(UsingEcwTightOrFull()) {
				//gridProg.Width=524;
				if(gridProg.Columns !=null && gridProg.Columns.Count>0) {
					int gridW=0;
					for(int i=0;i<gridProg.Columns.Count;i++) {
						gridW+=gridProg.Columns[i].ColWidth;
					}
					if(gridW<524) {//for example, if not very many columns
						gridW=524;
					}
					if(gridW+20+toothChart.Width < ClientSize.Width) {//if space is big enough to allow full width
						gridProg.Width=gridW+20;
					}
					else {
						if(ClientSize.Width>0) {//prevents an error
							if(ClientSize.Width-toothChart.Width-1 < 524) {
								gridProg.Width=524;
							}
							else {
								gridProg.Width=ClientSize.Width-toothChart.Width-1;
							}
						}
					}
					//now, bump the other controls over
					toothChart.Location=new Point(gridProg.Width+2,26);
					trackBar.Location=new Point(gridProg.Width+2+textTrackBarDate.Width,toothChart.Bottom+1);
					textTrackBarDate.Location=new Point(gridProg.Width+2,toothChart.Bottom+1);
					textTreatmentNotes.Location=new Point(gridProg.Width+2,trackBar.Bottom+1);
					panelEcw.Location=new Point(gridProg.Width+2,textTreatmentNotes.Bottom+1);
				}
				if(panelImages.Visible) {
					panelEcw.Height=tabControlImages.Top-panelEcw.Top+1-(panelImages.Height+2);
				}
				else {
					panelEcw.Height=tabControlImages.Top-panelEcw.Top+1;
				}
				return;
			}
			if(Programs.UsingOrion) {//full width
				gridProg.Width=ClientSize.Width-gridProg.Location.X-1;
			}
			else if(gridProg.Columns !=null && gridProg.Columns.Count>0) {
				int gridW=0;
				for(int i=0;i<gridProg.Columns.Count;i++) {
					gridW+=gridProg.Columns[i].ColWidth;
				}
				if(gridProg.Location.X+gridW+20 < ClientSize.Width) {//if space is big enough to allow full width
					gridProg.Width=gridW+20;
				}
				else {
					if(ClientSize.Width>gridProg.Location.X) {//prevents an error
						gridProg.Width=ClientSize.Width-gridProg.Location.X-1;
					}
				}
			}
			if(Programs.UsingOrion) {
				//gridPtInfo is up in the tabs and does not need to be resized.
			}
			else{
				gridPtInfo.Height=tabControlImages.Top-gridPtInfo.Top;
			}
		}

		public static void InitializeOnStartup(ContrChart contrChart,TabControl tabProc,ODGrid gridProg,Panel panelEcw,
			TabControl tabControlImages,Size ClientSize,
			ODGrid gridPtInfo,ToothChartWrapper toothChart,RichTextBox textTreatmentNotes,OpenDental.UI.Button butECWup,
			OpenDental.UI.Button butECWdown,TabPage tabPatInfo,TrackBar trackBar,Label textTrack)
		{
			tabProc.SelectedIndex=0;
			tabProc.Height=259;
			if(UsingEcwTightOrFull()) {
				toothChart.Location=new Point(524+2,26);
				trackBar.Location=new Point(524+2+textTrack.Width,toothChart.Bottom+1);
				textTrack.Location=new Point(524+2,toothChart.Bottom+1);
				textTreatmentNotes.Location=new Point(524+2,trackBar.Bottom+1);
				textTreatmentNotes.Size=new Size(411,40);//make it a bit smaller than usual
				gridPtInfo.Visible=false;
				panelEcw.Visible=true;
				panelEcw.Location=new Point(524+2,textTreatmentNotes.Bottom+1);
				panelEcw.Size=new Size(411,tabControlImages.Top-panelEcw.Top+1);
				butECWdown.Location=butECWup.Location;//they will be at the same location, but just hide one or the other.
				butECWdown.Visible=false;
				tabProc.Location=new Point(0,28);
				gridProg.Location=new Point(0,tabProc.Bottom+2);
				gridProg.Height=ClientSize.Height-gridProg.Location.Y-2;
			}
			else {//normal:
				toothChart.Location=new Point(0,26);
				trackBar.Location=new Point(0+textTrack.Width,toothChart.Bottom+1);
				textTrack.Location=new Point(0,toothChart.Bottom+1);
				textTreatmentNotes.Location=new Point(0,trackBar.Bottom+1);
				textTreatmentNotes.Size=new Size(411,71);
				gridPtInfo.Visible=true;
				gridPtInfo.Location=new Point(0,textTreatmentNotes.Bottom+1);
				panelEcw.Visible=false;
				tabProc.Location=new Point(415,28);
				gridProg.Location=new Point(415,tabProc.Bottom+2);
				gridProg.Height=ClientSize.Height-gridProg.Location.Y-2;
				SetToothChartVisibleHelper(toothChart,textTreatmentNotes,trackBar,textTrack);
			}
			if(Programs.UsingOrion) {
				textTreatmentNotes.Visible=false;
				contrChart.Controls.Remove(gridPtInfo);
				gridPtInfo.Visible=true;
				gridPtInfo.Location=new Point(0,0);
				gridPtInfo.Size=new Size(tabPatInfo.ClientSize.Width,tabPatInfo.ClientSize.Height);
				gridPtInfo.Anchor=AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
				tabPatInfo.Controls.Add(gridPtInfo);
				tabProc.SelectedTab=tabPatInfo;
				tabProc.Height=toothChart.Height-1;
				gridProg.Location=new Point(0,trackBar.Bottom+2);
				gridProg.HScrollVisible=true;
				gridProg.Height=ClientSize.Height-gridProg.Location.Y-2;
				gridProg.Width=ClientSize.Width-gridProg.Location.X-1;//full width
			}
			else {
				tabProc.TabPages.Remove(tabPatInfo);
			}
		}

		public static void SetGridProgWidth(ODGrid gridProg,Size ClientSize,Panel panelEcw,RichTextBox textTreatmentNotes,ToothChartWrapper toothChart,TrackBar trackBar,Label textTrack) {
			if(UsingEcwTightOrFull()) {
				//gridProg.Width=524;
				if(gridProg.Columns !=null && gridProg.Columns.Count>0) {
					int gridW=0;
					for(int i=0;i<gridProg.Columns.Count;i++) {
						gridW+=gridProg.Columns[i].ColWidth;
					}
					if(gridW<524) {//for example, if not very many columns
						gridW=524;
					}
					if(gridW+20+toothChart.Width < ClientSize.Width) {//if space is big enough to allow full width
						gridProg.Width=gridW+20;
					}
					else {
						if(ClientSize.Width>0) {//prevents an error
							if(ClientSize.Width-toothChart.Width-1 < 524) {
								gridProg.Width=524;
							}
							else {
								gridProg.Width=ClientSize.Width-toothChart.Width-1;
							}
						}
					}
					//now, bump the other controls over
					toothChart.Location=new Point(gridProg.Width+2,26);
					trackBar.Location=new Point(gridProg.Width+2+textTrack.Width,toothChart.Bottom+1);
					textTrack.Location=new Point(gridProg.Width+2,toothChart.Bottom+1);
					textTreatmentNotes.Location=new Point(gridProg.Width+2,trackBar.Bottom+1);
					panelEcw.Location=new Point(gridProg.Width+2,textTreatmentNotes.Bottom+1);
				}
				else {
					gridProg.Width=524;
				}
				return;
			}
			if(Programs.UsingOrion) {//full width
				gridProg.Width=ClientSize.Width-gridProg.Location.X-1;
			}
			else if(gridProg.Columns !=null && gridProg.Columns.Count>0) {
				int gridW=0;
				for(int i=0;i<gridProg.Columns.Count;i++) {
					gridW+=gridProg.Columns[i].ColWidth;
				}
				if(gridProg.Location.X+gridW+20 < ClientSize.Width) {//if space is big enough to allow full width
					gridProg.Width=gridW+20;
				}
				else {
					gridProg.Width=ClientSize.Width-gridProg.Location.X-1;
				}
			}
		}

		public static void tabProc_MouseDown(int SelectedProcTab,ODGrid gridProg,TabControl tabProc,Size ClientSize,MouseEventArgs e) {
			if(Programs.UsingOrion) {
				return;//tabs never minimize
			}
			//selected tab will have changed, so we need to test the original selected tab:
			Rectangle rect=tabProc.GetTabRect(SelectedProcTab);
			if(rect.Contains(e.X,e.Y) && tabProc.Height>27) {//clicked on the already selected tab which was maximized
				tabProc.Height=27;
				tabProc.Refresh();
				gridProg.Location=new Point(tabProc.Left,tabProc.Bottom+1);
				gridProg.Height=ClientSize.Height-gridProg.Location.Y-2;
			}
			else if(tabProc.Height==27) {//clicked on a minimized tab
				tabProc.Height=259;
				tabProc.Refresh();
				gridProg.Location=new Point(tabProc.Left,tabProc.Bottom+1);
				gridProg.Height=ClientSize.Height-gridProg.Location.Y-2;
			}
			else {//clicked on a new tab
				//height will have already been set, so do nothing
			}
			SelectedProcTab=tabProc.SelectedIndex;
		}

		public static void GridPtInfoSetSize(ODGrid gridPtInfo,TabControl tabControlImages){
			if(!Programs.UsingOrion) {
				gridPtInfo.Height=tabControlImages.Top-gridPtInfo.Top;
			}
		}

		///<summary>Reorganizes controls when switching between TP charting mode and procedure notes mode in Chart Module.</summary>
		public static void SetTpChartingHelper(bool isTpChartingEnabled,Patient patCur,ODGrid gridProg,ListBox listBtnCats,CheckBox checkTPs,
			Panel panelTP,ODGrid gridTPs,ODGrid gridTpProcs,OpenDental.UI.Button butNewTP,ListBox listPriorities)
		{
			listBtnCats.BringToFront();
			if(!isTpChartingEnabled) {
				checkTPs.Checked=false;
				//set listBtnCats height so it will be 2 pixels below checkTPs Y pos + checkTPs height
				listBtnCats.Height=checkTPs.Location.Y+checkTPs.Height+2-listBtnCats.Location.Y;
				panelTP.Visible=false;
				gridProg.Visible=true;
				return;
			}
			//IsTpChartingEnabled is true, set panelTP and gridProg visibility based on whether or not checkTPs is checked
			panelTP.Visible=checkTPs.Checked;
			gridProg.Visible=!checkTPs.Checked;
			//adjust listBtnCats height so it will be 1 pixel above checkTPs Y pos
			listBtnCats.Height=checkTPs.Location.Y-listBtnCats.Location.Y-1;
			panelTP.Location=gridProg.Location;
			panelTP.Size=new Size(770,gridProg.Size.Height);
			butNewTP.Location=new Point(gridTPs.Location.X+gridTPs.Width+6,butNewTP.Location.Y);
			//Fill Priority list. Can probably be moved to the calling class.
			listPriorities.Items.Clear();
			listPriorities.Items.Add(Lan.g("ContrChart","no priority"));
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.TxPriorities,true);
			for(int i=0;i<listDefs.Count;i++) {
				listPriorities.Items.Add(listDefs[i].ItemName);
			}
			if(patCur==null) {
				gridTPs.Enabled=false;
				gridTpProcs.Enabled=false;
				listPriorities.Enabled=false;
				butNewTP.Enabled=false;
			}
			else {
				gridTPs.Enabled=true;
				gridTpProcs.Enabled=true;
				listPriorities.Enabled=true;
				butNewTP.Enabled=true;
			}
		}

	}

	//public enum ChartLayoutMode

}
