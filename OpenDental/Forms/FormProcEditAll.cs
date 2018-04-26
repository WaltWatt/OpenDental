using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	public partial class FormProcEditAll:ODForm {
		public List<Procedure> ProcList;
		private List<Procedure> ProcOldList;
		//private bool StartedAttachedToClaim;
		///<summary>True when any proc in ProcList has a Proc Status of C.</summary>
		private bool _hasCompletedProc;
		///<summary>True when any proc in ProcList has a Proc Status of EO or EC.</summary>
		private bool _hasExistingProc;

		public FormProcEditAll() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormProcEditAll_Load(object sender,EventArgs e) {
			ProcOldList=new List<Procedure>();
			for(int i=0;i<ProcList.Count;i++){
				ProcOldList.Add(ProcList[i].Copy());
			}
			_hasCompletedProc=false;
			_hasExistingProc=false;
			DateTime oldestProcDate=DateTime.Today;
			bool datesAreSame=true;
			bool areAllBypass=true;
			for(int i=0;i<ProcList.Count;i++){
				if(ProcList[i].ProcStatus==ProcStat.C) {
					_hasCompletedProc=true;
					if(ProcList[i].ProcDate < oldestProcDate){
						oldestProcDate=ProcList[i].ProcDate;
					}
				}
				else if(ProcList[i].ProcStatus.In(ProcStat.EO,ProcStat.EC)){
					_hasExistingProc=true;
					if(ProcList[i].ProcDate < oldestProcDate){
						oldestProcDate=ProcList[i].ProcDate;
					}
				}
				if(ProcList[0].ProcDate!=ProcList[i].ProcDate){
					datesAreSame=false;
				}
				if(!ProcedureCodes.CanBypassLockDate(ProcList[i].CodeNum,ProcList[i].ProcFee)) {
					areAllBypass=false;
				}
			}
			if(_hasCompletedProc || _hasExistingProc){
				if(areAllBypass) {
					if((_hasCompletedProc && !Security.IsAuthorized(Permissions.ProcComplEdit,oldestProcDate,ProcList[0].CodeNum,0))
					|| (_hasExistingProc && !Security.IsAuthorized(Permissions.ProcExistingEdit,oldestProcDate,ProcList[0].CodeNum,0)))
					{
						butOK.Enabled=false;
						butEditAnyway.Enabled=false;
					}
				}
				else if((_hasCompletedProc && !Security.IsAuthorized(Permissions.ProcComplEdit,oldestProcDate))
				|| (_hasExistingProc && !Security.IsAuthorized(Permissions.ProcExistingEdit,oldestProcDate)))
				{
					butOK.Enabled=false;
					butEditAnyway.Enabled=false;
				}
			}
			List<ClaimProc> ClaimProcList=ClaimProcs.Refresh(ProcList[0].PatNum);
			if(Procedures.IsAttachedToClaim(ProcList,ClaimProcList)){
				//StartedAttachedToClaim=true;
				//however, this doesn't stop someone from creating a claim while this window is open,
				//so this is checked at the end, too.
				textDate.Enabled=false;
				butToday.Enabled=false;
				butEditAnyway.Visible=true;
				labelClaim.Visible=true;
			}
			if(datesAreSame){
				textDate.Text=ProcList[0].ProcDate.ToShortDateString();
			}
		}

		private void butToday_Click(object sender,EventArgs e) {
			if(textDate.Enabled){
				textDate.Text=DateTime.Today.ToShortDateString();
			}
		}

		private void butEditAnyway_Click(object sender,EventArgs e) {
			textDate.Enabled=true;
			butToday.Enabled=true;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textDate.errorProvider1.GetError(textDate)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(textDate.Text!=""){
				DateTime procDate=PIn.Date(textDate.Text);
				Appointment apt;
				for(int i=0;i<ProcList.Count;i++){
					if(ProcList[i].AptNum==0){
						continue;
					}
					apt=Appointments.GetOneApt(ProcList[i].AptNum);
					if(ProcList[i].ProcDate!=procDate){
						if(!MsgBox.Show(this,true,"Date does not match appointment date.  Continue anyway?")){
							return;
						}
						break;
					}
				}
				for(int i=0;i<ProcList.Count;i++){
					if(ProcList[i].ProcStatus==ProcStat.C && ProcList[i].ProcDate > DateTime.Today.Date && !PrefC.GetBool(PrefName.FutureTransDatesAllowed)) {
						MsgBox.Show(this,"Completed procedures cannot be set for future dates.");
						return;
					}
					ProcList[i].ProcDate=procDate;
					Procedures.Update(ProcList[i],ProcOldList[i]);
				}
				ClaimProcs.UpdateProcDate(ProcList.Select(x => x.ProcNum).ToList(),procDate);
				Recalls.Synch(ProcList[0].PatNum);
				if(_hasCompletedProc || _hasExistingProc){
					Patient pat=Patients.GetPat(ProcList[0].PatNum);
					if(_hasCompletedProc) {
						string completeCodes=string.Join(", ",
							ProcList.Where(x => x.ProcStatus==ProcStat.C).Select(x => ProcedureCodes.GetProcCode(x.CodeNum))
						);
						SecurityLogs.MakeLogEntry(Permissions.ProcComplEdit,ProcList[0].PatNum,
							pat.GetNameLF()+" "+completeCodes+", New date:"+procDate.ToShortDateString());
					}
					if(_hasExistingProc) {
						string existingCodes=string.Join(", ",
							ProcList.Where(x => x.ProcStatus.In(ProcStat.EO,ProcStat.EC)).Select(x => ProcedureCodes.GetProcCode(x.CodeNum))
						);
						SecurityLogs.MakeLogEntry(Permissions.ProcExistingEdit,ProcList[0].PatNum,
							pat.GetNameLF()+" "+existingCodes+", New date:"+procDate.ToShortDateString());
					}
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		

		
	}
}