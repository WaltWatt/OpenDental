using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Linq;

namespace OpenDental{
	/// <summary></summary>
	public class FormTasks:ODForm {
		//private System.ComponentModel.IContainer components;
		/////<summary>After closing, if this is not zero, then it will jump to the object specified in GotoKeyNum.</summary>
		//public TaskObjectType GotoType;
		private UserControlTasks userControlTasks1;
		private IContainer components=null;
		/////<summary>After closing, if this is not zero, then it will jump to the specified patient.</summary>
		//public long GotoKeyNum;
		private bool IsTriage;
		private FormWindowState windowStateOld;

		public UserControlTasksTab TaskTab {
			get {
				return userControlTasks1.TaskTab;
			}
			set {
				userControlTasks1.TaskTab=value;
			}
		}
	
		///<summary></summary>
		public FormTasks()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			//Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTasks));
			this.userControlTasks1 = new OpenDental.UserControlTasks();
			this.SuspendLayout();
			// 
			// userControlTasks1
			// 
			this.userControlTasks1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userControlTasks1.Location = new System.Drawing.Point(0, 0);
			this.userControlTasks1.Name = "userControlTasks1";
			this.userControlTasks1.Size = new System.Drawing.Size(885, 671);
			this.userControlTasks1.TabIndex = 0;
			this.userControlTasks1.FillGridEvent += new OpenDental.UserControlTasks.FillGridEventHandler(this.UserControlTasks1_FillGridEvent);
			this.userControlTasks1.Resize += new System.EventHandler(this.userControlTasks1_Resize);
			// 
			// FormTasks
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(885, 671);
			this.Controls.Add(this.userControlTasks1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormTasks";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Tasks";
			this.Load += new System.EventHandler(this.FormTasks_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormTasks_Load(object sender,EventArgs e) {
			windowStateOld=WindowState;
			userControlTasks1.InitializeOnStartup();
		}

		public override void OnProcessSignals(List<Signalod> listSignals) {
			if(listSignals.Exists(x => x.IType==InvalidType.Task || x.IType==InvalidType.TaskPopup)) {
				RefreshUserControlTasks();
			}
		}
		
		private void userControlTasks1_GoToChanged(object sender,EventArgs e) {
			TaskObjectType gotoType=userControlTasks1.GotoType;
			long gotoKeyNum=userControlTasks1.GotoKeyNum;
			if(gotoType==TaskObjectType.Patient){
				if(gotoKeyNum!=0){
					Patient pat=Patients.GetPat(gotoKeyNum);
					//OnPatientSelected(pat);
					if(IsTriage) {
						GotoModule.GotoChart(pat.PatNum);
					}
					else {
						GotoModule.GotoAccount(pat.PatNum);
					}
				}
			}
			if(gotoType==TaskObjectType.Appointment){
				if(gotoKeyNum!=0){
					Appointment apt=Appointments.GetOneApt(gotoKeyNum);
					if(apt==null){
						MsgBox.Show(this,"Appointment has been deleted, so it's not available.");
						return;
						//this could be a little better, because window has closed, but they will learn not to push that button.
					}
					DateTime dateSelected=DateTime.MinValue;
					if(apt.AptStatus==ApptStatus.Planned || apt.AptStatus==ApptStatus.UnschedList){
						//I did not add feature to put planned or unsched apt on pinboard.
						MsgBox.Show(this,"Cannot navigate to appointment.  Use the Other Appointments button.");
						//return;
					}
					else{
						dateSelected=apt.AptDateTime;
					}
					Patient pat=Patients.GetPat(apt.PatNum);
					//OnPatientSelected(pat);
					GotoModule.GotoAppointment(dateSelected,apt.AptNum);
				}
			}
			//DialogResult=DialogResult.OK;
		}

		///<summary>Used by OD HQ.</summary>
		public void ShowTriage() {
			userControlTasks1.FillGridWithTriageList();
			IsTriage=true;
		}

		///<summary>Simply tells the user task control to refresh the currently showing task list.</summary>
		public void RefreshUserControlTasks() {
			if(userControlTasks1!=null && !userControlTasks1.IsDisposed) {
				userControlTasks1.RefreshTasks();
			}
		}

		private void userControlTasks1_Resize(object sender,EventArgs e) {
			if(WindowState==FormWindowState.Minimized) {//Form currently minimized.
				windowStateOld=WindowState;
				return;//The window is invisble when minimized, so no need to refresh.
			}
			if(windowStateOld==FormWindowState.Minimized) {//Form was previously minimized (invisible) and is now in normal state or maximized state.
				RefreshUserControlTasks();//Refresh the grid height because the form height might have changed.
				windowStateOld=WindowState;
				return;
			}
			windowStateOld=WindowState;//Set the window state after every resize.
		}

		private void UserControlTasks1_FillGridEvent(object sender,EventArgs e) {
			this.Text=userControlTasks1.ControlParentTitle;
		}

		/* private void timer1_Tick(object sender,EventArgs e) {
				if(Security.CurUser!=null) {//Possible if OD auto logged a user off and they left the task window open in the background.
					userControlTasks1.RefreshTasks();
				}
				//this quick and dirty refresh is not as intelligent as the one used when tasks are docked.
				//Sound notification of new task is controlled from main form completely
				//independently of this visual refresh.
			}
		}
		*/

		

		

		

		

		
	



	}
}





















