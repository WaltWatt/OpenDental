using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	///<summary></summary>
	public class FormScheduleEdit : ODForm	{
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label labelStop;
		private System.Windows.Forms.Label labelStart;
		private OpenDental.ODtextBox textNote;
		private System.Windows.Forms.Label label4;
		private ComboBox comboStop;
		private ComboBox comboStart;
		private ListBox listOps;
		private Label labelOps;
		public Schedule SchedCur;
		///<summary>Filters the list of operatories available to the clinic passed in.  Set to 0 to show all operatories.  Also the clinic selected by
		///default for holidays and provider notes.</summary>
		public long ClinicNum;
		private ComboBox comboClinic;
		private Label labelClinic;
		///<summary>List of clinics for the current user.  Used to set the clinic for holidays and provider notes.</summary>
		private List<Clinic> _listClinics;
		///<summary>All ops if clinics not enabled, otherwise all ops for ClinicNum.</summary>
		private List<Operatory> _listOps;
		///<summary>List of schedules for the day set from FormScheduleDayEdit filled with the filtered list of schedules for the day.
		///Used to ensure there is only one holiday schedule item per day/clinic, since this list has not been synced to the db yet.</summary>
		public List<Schedule> ListScheds;
		private bool _isHolidayOrNote;
		public List<long> ListProvNums;

		///<summary></summary>
		public FormScheduleEdit(){
			InitializeComponent();
			Lan.F(this);
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormScheduleEdit));
			this.labelStop = new System.Windows.Forms.Label();
			this.labelStart = new System.Windows.Forms.Label();
			this.textNote = new OpenDental.ODtextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.comboStop = new System.Windows.Forms.ComboBox();
			this.comboStart = new System.Windows.Forms.ComboBox();
			this.listOps = new System.Windows.Forms.ListBox();
			this.labelOps = new System.Windows.Forms.Label();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// labelStop
			// 
			this.labelStop.Location = new System.Drawing.Point(6, 39);
			this.labelStop.Name = "labelStop";
			this.labelStop.Size = new System.Drawing.Size(89, 16);
			this.labelStop.TabIndex = 9;
			this.labelStop.Text = "Stop Time";
			this.labelStop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelStart
			// 
			this.labelStart.Location = new System.Drawing.Point(6, 12);
			this.labelStart.Name = "labelStart";
			this.labelStart.Size = new System.Drawing.Size(89, 16);
			this.labelStart.TabIndex = 7;
			this.labelStart.Text = "Start Time";
			this.labelStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textNote
			// 
			this.textNote.AcceptsTab = true;
			this.textNote.DetectUrls = false;
			this.textNote.Location = new System.Drawing.Point(97, 92);
			this.textNote.Name = "textNote";
			this.textNote.QuickPasteType = OpenDentBusiness.QuickPasteType.Schedule;
			this.textNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textNote.Size = new System.Drawing.Size(220, 113);
			this.textNote.TabIndex = 15;
			this.textNote.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(6, 93);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(89, 16);
			this.label4.TabIndex = 16;
			this.label4.Text = "Note";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboStop
			// 
			this.comboStop.FormattingEnabled = true;
			this.comboStop.Location = new System.Drawing.Point(97, 38);
			this.comboStop.MaxDropDownItems = 48;
			this.comboStop.Name = "comboStop";
			this.comboStop.Size = new System.Drawing.Size(120, 21);
			this.comboStop.TabIndex = 25;
			// 
			// comboStart
			// 
			this.comboStart.FormattingEnabled = true;
			this.comboStart.Location = new System.Drawing.Point(97, 11);
			this.comboStart.MaxDropDownItems = 48;
			this.comboStart.Name = "comboStart";
			this.comboStart.Size = new System.Drawing.Size(120, 21);
			this.comboStart.TabIndex = 24;
			// 
			// listOps
			// 
			this.listOps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listOps.IntegralHeight = false;
			this.listOps.Location = new System.Drawing.Point(348, 31);
			this.listOps.Name = "listOps";
			this.listOps.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listOps.Size = new System.Drawing.Size(243, 352);
			this.listOps.TabIndex = 27;
			// 
			// labelOps
			// 
			this.labelOps.Location = new System.Drawing.Point(348, 12);
			this.labelOps.Name = "labelOps";
			this.labelOps.Size = new System.Drawing.Size(95, 16);
			this.labelOps.TabIndex = 26;
			this.labelOps.Text = "Operatories";
			this.labelOps.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(516, 393);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 14;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(428, 393);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 12;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(97, 65);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(198, 21);
			this.comboClinic.TabIndex = 94;
			this.comboClinic.Visible = false;
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(6, 66);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(89, 16);
			this.labelClinic.TabIndex = 93;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelClinic.Visible = false;
			// 
			// FormScheduleEdit
			// 
			this.ClientSize = new System.Drawing.Size(603, 431);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.listOps);
			this.Controls.Add(this.labelOps);
			this.Controls.Add(this.comboStop);
			this.Controls.Add(this.comboStart);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.labelStop);
			this.Controls.Add(this.labelStart);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(539, 254);
			this.Name = "FormScheduleEdit";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Edit Schedule";
			this.Load += new System.EventHandler(this.FormScheduleEdit_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormScheduleEdit_Load(object sender, System.EventArgs e) {
			_isHolidayOrNote=(SchedCur.StartTime==TimeSpan.Zero && SchedCur.StopTime==TimeSpan.Zero);
			if(PrefC.HasClinicsEnabled) {
				if(ClinicNum==0) {
					Text+=" - "+Lan.g(this,"Headquarters");
				}
				else {
					string abbr=Clinics.GetAbbr(ClinicNum);
					if(!string.IsNullOrWhiteSpace(abbr)) {
						Text+=" - "+abbr;
					}
				}
				//if clinics are enabled and this is a holiday or practice note, set visible and fill the clinic combobox and private list of clinics
				if(_isHolidayOrNote && SchedCur.SchedType==ScheduleType.Practice) {
					comboClinic.Visible=true;//only visible for holidays and practice notes and only if clinics are enabled
					labelClinic.Visible=true;
					_listClinics=Clinics.GetForUserod(Security.CurUser);
					if(!Security.CurUser.ClinicIsRestricted) {
						comboClinic.Items.Add(Lan.g(this,"Headquarters"));
						if(SchedCur.ClinicNum==0) {//new sched and HQ selected or opened one from db for HQ
							comboClinic.SelectedIndex=0;
						}
					}
					foreach(Clinic clinicCur in _listClinics) {
						comboClinic.Items.Add(clinicCur.Abbr);
						if(clinicCur.ClinicNum==SchedCur.ClinicNum) {
							comboClinic.SelectedIndex=comboClinic.Items.Count-1;
						}
					}
					if(comboClinic.SelectedIndex<0) {//current sched's clinic not found or set to 0 and user is restricted, default to clinic sent in
						comboClinic.SelectedIndex=_listClinics.FindIndex(x => x.ClinicNum==ClinicNum)+(Security.CurUser.ClinicIsRestricted?0:1);//add one for HQ if not restricted
					}
				}
			}
			textNote.Text=SchedCur.Note;
			if(_isHolidayOrNote) {
				comboStart.Visible=false;
				labelStart.Visible=false;
				comboStop.Visible=false;
				labelStop.Visible=false;
				listOps.Visible=false;
				labelOps.Visible=false;
				textNote.Select();
				return;
			}
			//from here on, NOT a practice note or holiday
			DateTime time;
			for(int i=0;i<24;i++) {
				time=DateTime.Today+TimeSpan.FromHours(7)+TimeSpan.FromMinutes(30*i);
				comboStart.Items.Add(time.ToShortTimeString());
				comboStop.Items.Add(time.ToShortTimeString());
			}
			comboStart.Text=SchedCur.StartTime.ToShortTimeString();
			comboStop.Text=SchedCur.StopTime.ToShortTimeString();
			listOps.Items.Add(Lan.g(this,"not specified"));
			//filter list if using clinics and if a clinic filter was passed in to only ops assigned to the specified clinic, otherwise all non-hidden ops
			_listOps=Operatories.GetDeepCopy(true);
			if(PrefC.HasClinicsEnabled && ClinicNum>0) {
				_listOps.RemoveAll(x => x.ClinicNum!=ClinicNum);
			}
			foreach(Operatory opCur in _listOps) {
				int curIndex=listOps.Items.Add(opCur.OpName);
				//Select the item that was just added if the schedule's Ops contains the current OpNum.
				listOps.SetSelected(curIndex,SchedCur.Ops.Contains(opCur.OperatoryNum));
			}
			listOps.SetSelected(0,listOps.SelectedIndices.Count==0);//select 'not specified' if no ops were selected in the loop
			comboStart.Select();
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			#region Validation
			DateTime startDateT=DateTime.MinValue;
			DateTime stopDateT=DateTime.MinValue;
			if(!_isHolidayOrNote) {
				if(listOps.SelectedIndices.Count==0){
					MsgBox.Show(this,"Please select ops first.");
					return;
				}
				if(listOps.SelectedIndices.Count>1 && listOps.SelectedIndices.Contains(0)){
					MsgBox.Show(this,"Invalid selection of ops.");
					return;
				}
				startDateT=PIn.DateT(comboStart.Text);
				stopDateT=PIn.DateT(comboStop.Text);
				if(startDateT==DateTime.MinValue || stopDateT==DateTime.MinValue) {
					MsgBox.Show(this,"Incorrect time format");
					return;
				}
				if(startDateT>stopDateT) {
					MsgBox.Show(this,"Stop time must be later than start time.");
					return;
				}
				List<long> listSelectedOps=new List<long>();
				List<Schedule> listProvSchedsOnly=ListScheds.FindAll(x=>x.SchedType==ScheduleType.Provider);
				if(!listOps.SelectedIndices.Contains(0)) {//add selected operatories into listSelectedOps
					listSelectedOps=listOps.SelectedIndices.OfType<int>().Select(x =>_listOps[x-1].OperatoryNum).ToList();
				}
				SchedCur.Ops=listSelectedOps.ToList();//deep copy of list. (because it is value type.)
				SchedCur.StartTime=startDateT.TimeOfDay;
				SchedCur.StopTime=stopDateT.TimeOfDay;
				List<long> listProvsOverlap=new List<long>();
				foreach(long provNum in ListProvNums) {//Potentially check each provider for overlap.
					Schedule schedTemp=SchedCur.Copy();
					schedTemp.ProvNum=provNum;
					if(schedTemp.IsNew) {
						listProvSchedsOnly.Add(schedTemp);//new scheds will be added to check if overlap. 
					}
					bool isOverlapDetected=false;
					//====================Pre-Emptive Overlaps====================
					isOverlapDetected=ListProvNums.Count>1 && listSelectedOps.Count>0;
					//====================SIMPLE OVERLAP, No-Ops====================
					if(!isOverlapDetected) {
						isOverlapDetected=schedTemp.Ops.Count==0 //only look at schedules without operatories
							&& listProvSchedsOnly.FindAll(x => x.ProvNum==schedTemp.ProvNum //Only consider current provider for overlaps w/o Ops
								&& x.Ops.Count==0 //Also doesn't have an operatory
								&& schedTemp.StartTime<=x.StopTime  //Overlapping Time 
								&& schedTemp.StopTime>=x.StartTime) //Overlapping Time 	 
							.Count>1;//count scheds that overlap that also do not have operatories.
					}
					//====================COMPLEX OVERLAP, Ops and All====================
					if(!isOverlapDetected) {//If we did not find a simple overlap, attemptto find a "complicated" overlap
						isOverlapDetected=schedTemp.Ops.Count>0 &&
							listProvSchedsOnly //Select into groups of overlapping comparable schedule objects.
							.FindAll(x => x.Ops.Count>0)//can only overlap if Ops are involved
							.FindAll(x => schedTemp.StartTime<=x.StopTime && schedTemp.StopTime>=x.StartTime) //Find all overlaps
							.SelectMany(x => x.Ops.Intersect(listSelectedOps))//can/must contain duplicates. Intersect ignores Ops not used by SchedCur
							.GroupBy(x => x)//group each overlapping list into operatories and flatten into large collection
							.Any(x => x.Count()>1);//count any group that has more tha one schedule for the same time and operatory
					}
					if(isOverlapDetected) {
						listProvsOverlap.Add(provNum);
					}
				}//End foraech ProvNum
				listProvsOverlap=listProvsOverlap.Distinct().ToList();
				if(listProvsOverlap.Count>0 && MessageBox.Show(Lan.g(this,"Overlapping provider schedules detected, would you like to continue anyway?")+"\r\n"+Lan.g(this,"Providers affected")+":\r\n  "
					+string.Join("\r\n  ",listProvsOverlap.Select(x=>Providers.GetLongDesc(x))),"",MessageBoxButtons.YesNo)!=DialogResult.Yes) 
				{ 
					return;
				}
			}
			else if(SchedCur.Status!=SchedStatus.Holiday && textNote.Text=="") {//don't allow blank schedule notes
				MsgBox.Show(this,"Please enter a note first.");
				return;
			}
			long clinicNum=0;
			if(_isHolidayOrNote && SchedCur.SchedType==ScheduleType.Practice && PrefC.HasClinicsEnabled) {//prov notes do not have a clinic
				int indexCur=comboClinic.SelectedIndex;
				if(!Security.CurUser.ClinicIsRestricted) {//user isn't restricted, -1 for HQ
					indexCur--;
				}
				if(indexCur>-1) {//will be -1 if HQ is selected, leave clinicNum=0
					clinicNum=_listClinics[indexCur].ClinicNum;
				}
				if(SchedCur.Status==SchedStatus.Holiday) {//duplicate holiday check
					List<Schedule> listScheds=ListScheds.FindAll(x => x.SchedType==ScheduleType.Practice && x.Status==SchedStatus.Holiday);//scheds in local list
					listScheds.AddRange(Schedules.GetAllForDateAndType(SchedCur.SchedDate,ScheduleType.Practice)
						.FindAll(x => x.ScheduleNum!=SchedCur.ScheduleNum
							&& x.Status==SchedStatus.Holiday
							&& listScheds.All(y => y.ScheduleNum!=x.ScheduleNum)));//add any in db that aren't in local list
					if(listScheds.Any(x => x.ClinicNum==0 || x.ClinicNum==clinicNum)//already a holiday for HQ in db or duplicate holiday for a clinic
						|| (clinicNum==0 && listScheds.Count>0))//OR trying to create a HQ holiday when a clinic already has one for this day
					{
						MsgBox.Show(this,"There is already a Holiday for the practice or clinic on this date.");
						return;
					}
				}
			}
			#endregion Validation
			#region Set Schedule Fields
      SchedCur.Note=textNote.Text;
			SchedCur.Ops=new List<long>();
			if(listOps.SelectedIndices.Count>0 && !listOps.SelectedIndices.Contains(0)) {
				listOps.SelectedIndices.OfType<int>().ToList().ForEach(x => SchedCur.Ops.Add(_listOps[x-1].OperatoryNum));
			}
			SchedCur.ClinicNum=clinicNum;//0 if HQ selected or clinics not enabled or not a holiday or practice note
			#endregion Set Schedule Fields
			DialogResult=DialogResult.OK;		  
    }

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}






