using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental.UI {
	public partial class PinBoard:Control {
		private List<ContrApptSingle> apptList;
		private int selectedIndex;
		///<summary></summary>
		[Category("Property Changed"),Description("Event raised when user _clicks_ to select a different appointment.")]
		public event EventHandler SelectedIndexChanged=null;

		public PinBoard() {
			InitializeComponent();
			apptList=new List<ContrApptSingle>();
		}

		///<Summary>Do not make changes here to the list.  This is for read-only purposes.</Summary>
		public List<ContrApptSingle> ApptList{
			get{
				return apptList;
			}
		}

		public ContrApptSingle SelectedAppt{
			get{
				if(selectedIndex==-1){
					return null;
				}
				return apptList[selectedIndex];
			}
		}

		///<Summary>Gets or sets the selected </Summary>
		public int SelectedIndex{
			get{
				return selectedIndex;
			}
			set{
				if(selectedIndex>apptList.Count-1){
					selectedIndex=-1;
				}
				else{
					selectedIndex=value;
				}
				for(int i=0;i<apptList.Count;i++) {
					if(i==value){
						apptList[i].IsSelected=true;
					}
					else{
						apptList[i].IsSelected=false;
					}
				}
				Invalidate();
			}
		}

		protected void OnSelectedIndexChanged() {
			if(SelectedIndexChanged!=null) {
				SelectedIndexChanged(this,new EventArgs());
			}
		}

		///<Summary>Supply an aptNum to get the corresponding DataRow and DataTables necessary to create the ContrApptSingle. Calls directly to database.</Summary>
		public ContrApptSingle AddAppointment(long aptNum) {			
			DataTable tableAppts=Appointments.RefreshOneApt(aptNum,false).Tables["Appointments"];
			DataRow row=tableAppts.Rows[0];
			if(row["AptStatus"].ToString()=="6") {//planned so do it again the right way
				tableAppts=Appointments.RefreshOneApt(aptNum,true).Tables["Appointments"];
				row=tableAppts.Rows[0];
			}
			//The appt fields are not in DS.Tables["ApptFields"] since the appt is not visible on the schedule.
			DataTable tableApptFields=Appointments.GetApptFields(tableAppts);
			return AddAppointment(row,tableApptFields,Appointments.GetPatFields(tableAppts.Select().Select(x => PIn.Long(x["PatNum"].ToString())).ToList()));
		}
		
		///<Summary>Supply a datarow that contains all the database values needed for the appointment that is being added.</Summary>
		public ContrApptSingle AddAppointment(DataRow row,DataTable tableApptFields,DataTable tablePatFields) {
			//if appointment is already on the pinboard, just select it.
			for(int i = 0;i<apptList.Count;i++) {
				if(apptList[i].AptNum.ToString()==row["AptNum"].ToString()) {
					//Highlight it
					selectedIndex=i;
					apptList[i].IsSelected=true;
					Invalidate();
				}
			}
			ContrApptSingle pinApptSingle=new ContrApptSingle(
				row,
				tableApptFields,
				tablePatFields,
				new Point(0,13*apptList.Count)) {
				ThisIsPinBoard=true,
				IsSelected=true,
			};
			pinApptSingle.Width=Width-2;			
			apptList.Add(pinApptSingle);
			selectedIndex=apptList.Count-1;
			Invalidate();
			return pinApptSingle;
		}

		public void ClearSelected(){
			if(selectedIndex==-1){
				return;
			}
			apptList.RemoveAt(selectedIndex);
			if(apptList.Count>=selectedIndex+1){
				//no change to selectedIndex
				apptList[selectedIndex].IsSelected=true;
			}
			else if(apptList.Count>0){
				//select the last one
				selectedIndex=apptList.Count-1;
				apptList[selectedIndex].IsSelected=true;
			}
			else{
				selectedIndex=-1;
			}
			//reset locations
			for(int i=0;i<apptList.Count;i++) {
				apptList[i].Location=new Point(0,13*i);
			}
			Invalidate();
		}

		///<summary>If an appt is already on the pinboard, and the information in it is change externally, this 'refreshes' the data.</summary>
		public void ResetData(long aptNum) {
			ContrApptSingle ctrl=apptList.FirstOrDefault(x => x.AptNum==aptNum);
			if(ctrl==null) {
				return;
			}
			DataTable tableAppts=Appointments.RefreshOneApt(aptNum,false).Tables["Appointments"];
			DataRow row=tableAppts.Rows[0];
			if(row["AptStatus"].ToString()=="6") {//planned so do it again the right way
				tableAppts=Appointments.RefreshOneApt(aptNum,true).Tables["Appointments"];
				row=tableAppts.Rows[0];
			}
			//The appt fields are not in DS.Tables["ApptFields"] since the appt is not visible on the schedule.
			DataTable tableApptFields=Appointments.GetApptFields(tableAppts);
			ctrl.ResetData(row
				,tableApptFields
				,Appointments.GetPatFields(tableAppts.Select().Select(x => PIn.Long(x["PatNum"].ToString())).ToList())
				,ctrl.Location
				,false);
			ctrl.Width=this.Width-2;			
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs pe) {
			Graphics g=pe.Graphics;
			try {
				g.Clear(Color.White);
				g.DrawRectangle(Pens.Black,0,0,Width-1,Height-1);
				for(int i=0;i<apptList.Count;i++) {
					apptList[i].CreateShadow();
					if(apptList[i].IsShadowValid) { //Stack each appt 13 pixels apart. They will still be drawn at their original height but subsequent appts will be drawn on top of their preceding sibling.
						g.DrawImage(apptList[i].Shadow,0,i*13);
					}
				}
				if(apptList.Count==0) {
					StringFormat format=new StringFormat();
					format.Alignment=StringAlignment.Center;
					format.LineAlignment=StringAlignment.Center;
					g.DrawString(Lan.g(this,"Drag Appointments to this Pinboard"),Font,Brushes.Gray,new RectangleF(0,0,Width,Height-20),format);
				}
			}
			catch { }
		}

		#region Mouse
		protected override void OnMouseDown(MouseEventArgs e) {
			//figure out which appt mouse is on.  Start at end and work backwards
			int index=-1;
			for(int i=apptList.Count-1;i>=0;i--){
				if(e.Y<apptList[i].Top || e.Y>apptList[i].Bottom){
					continue;
				}
				index=i;
				break;
			}
			if(index==-1){
				base.OnMouseDown(e);
				return;
			}
			if(index==selectedIndex){//no change
				base.OnMouseDown(e);
				return;//for now.
			}
			selectedIndex=index;
			for(int i=0;i<apptList.Count;i++){
				if(i==selectedIndex){
					apptList[i].IsSelected=true;
				}
				else{
					apptList[i].IsSelected=false;
				}
			}			
			Invalidate();
			OnSelectedIndexChanged();
			base.OnMouseDown(e);
		}		
		#endregion
	}
}
