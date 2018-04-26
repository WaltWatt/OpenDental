using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormInsRemain:ODForm {

		private Patient _patCur;
		private Family _famCur;

		public FormInsRemain(long selectedPatNum) {
			InitializeComponent();
			Lan.F(this);
			_famCur=Patients.GetFamily(selectedPatNum);
			_patCur=_famCur.GetPatient(selectedPatNum);
		}

		private void FormInsRemain_Load(object sender,EventArgs e) {
			SetGridCols();
			FillGrid();
			FillSummary();
		}

		/// <summary>Column sizes can be changed as needed</summary>
		private void SetGridCols() { 
			ODGridColumn col;
			gridRemainTimeUnits.BeginUpdate();
			gridRemainTimeUnits.Columns.Clear();
			col=new ODGridColumn("Category",165);
			gridRemainTimeUnits.Columns.Add(col);
			col=new ODGridColumn("Qty",60);
			gridRemainTimeUnits.Columns.Add(col);
			col=new ODGridColumn("Used",60);
			gridRemainTimeUnits.Columns.Add(col);
			col=new ODGridColumn("Remaining",60);
			gridRemainTimeUnits.Columns.Add(col);
			gridRemainTimeUnits.EndUpdate();
		}

		private void FillGrid() {
			gridRemainTimeUnits.BeginUpdate();
			gridRemainTimeUnits.Rows.Clear();
			List<Benefit> listPatBenefits=Benefits.Refresh(PatPlans.Refresh(_patCur.PatNum),InsSubs.GetListForSubscriber(_patCur.PatNum));
			ODGridRow gridRow; 
			List<Procedure> listProcs;
			double amtUsed;
			int monthRenew;
			if(listPatBenefits.Count > 0) {
				for(int i=0;i<listPatBenefits.Count;i++) {
					if(listPatBenefits[i].CovCatNum==0 //no category
						|| listPatBenefits[i].BenefitType != InsBenefitType.Limitations //benefit type is not limitations
						|| (listPatBenefits[i].TimePeriod != BenefitTimePeriod.CalendarYear //neither calendar year nor serviceyear
							&& listPatBenefits[i].TimePeriod != BenefitTimePeriod.ServiceYear)
						|| listPatBenefits[i].Quantity < 0//quantity is negative (negatives are allowed in FormBenefitEdit)
						|| listPatBenefits[i].QuantityQualifier != BenefitQuantity.NumberOfServices//qualifier us not the number of services
						|| (listPatBenefits[i].CoverageLevel != BenefitCoverageLevel.Family //neither individual nor family coverage level
							&& listPatBenefits[i].CoverageLevel != BenefitCoverageLevel.Individual)) 
					{
						continue;
					}
					//for calendar year, get completed procs from January.01.CurYear ~ Curdate
					List<long> listPatNums=new List<long> {_patCur.PatNum};//for current patient.
					if(listPatBenefits[i].TimePeriod==BenefitTimePeriod.CalendarYear) {
						//01/01/CurYear. is there a better way?
						listProcs=Procedures.GetCompletedForDateRange(new DateTime(DateTimeOD.Today.Year,1,1),DateTimeOD.Today,null,listPatNums);
					}
					else { //if not calendar year, then it must be service year
						monthRenew=InsPlans.RefreshOne(listPatBenefits[i].PlanNum).MonthRenew; //monthrenew only stores the month as an int.
						if(DateTimeOD.Today.Month >= monthRenew) {//if the the current date is past the renewal month, use the current year
							listProcs=Procedures.GetCompletedForDateRange(new DateTime(DateTimeOD.Today.Year,monthRenew,1),DateTimeOD.Today,null,listPatNums);
						}
						else { //otherwise use the previous year
							listProcs=Procedures.GetCompletedForDateRange(new DateTime(DateTimeOD.Today.Year-1,monthRenew,1),DateTimeOD.Today,null,listPatNums);
						}
					}
					//Calculate the amount used for one benefit.
					amtUsed=GetAmtUsedForCat(listProcs,CovCats.GetCovCat(listPatBenefits[i].CovCatNum)); 
					gridRow=new ODGridRow();
					gridRow.Cells.Add(CovCats.GetCovCat(listPatBenefits[i].CovCatNum).EbenefitCat.ToString()); //Coverage Category
					gridRow.Cells.Add(listPatBenefits[i].Quantity.ToString()); //Quantity	
					gridRow.Cells.Add(amtUsed.ToString("F")); //Used
					double amtRemain=listPatBenefits[i].Quantity-amtUsed;
					if(amtRemain > 0) {
						gridRow.Cells.Add(amtRemain.ToString("F")); //quantity-used.
					}
					else { //if less then 0, just 0. 
						gridRow.Cells.Add("0");
					}
					gridRemainTimeUnits.Rows.Add(gridRow);
				}
			}
			gridRemainTimeUnits.EndUpdate();
		}
		
		///<summary>Pass in list of procedures and covCat, return the sum of all CanadaTimeUnits of the procedures in that covCat as a double.</summary>
		private double GetAmtUsedForCat(List<Procedure> listProcs,CovCat covCat) {
			List<ProcedureCode> listProcCodes=new List<ProcedureCode>();
			for(int i=0;i<listProcs.Count;i++) {
				listProcCodes.Add(ProcedureCodes.GetProcCode(listProcs[i].CodeNum));	//turn list of procedures into list of procedurecodes.
			}
			double total=0;//CanadaTimeUnits can be decimal numbers, like 0.5.
			for(int i=0;i<listProcCodes.Count;i++) { //for every procedurecode
				//Can be null if the procedure doesn't fall within any spans (like note proc, the code is "clinical" so doesn't fall inside any spans)
				CovCat benCat=CovCats.GetCovCat(CovSpans.GetCat(listProcCodes[i].ProcCode));
				//if the covCat of that code is the same as the passed-in covCat
				if(benCat!=null && benCat.EbenefitCat==covCat.EbenefitCat) { 
					total+=listProcCodes[i].CanadaTimeUnits; //add the Canada time units to the total.
				}
			}
			return total;
		}

		///<summary>All of the code from this method is copied directly from the account module, ContrAccount.FillSummary().</summary>
		private void FillSummary() {
			textFamPriMax.Text="";
			textFamPriDed.Text="";
			textFamSecMax.Text="";
			textFamSecDed.Text="";
			textPriMax.Text="";
			textPriDed.Text="";
			textPriDedRem.Text="";
			textPriUsed.Text="";
			textPriPend.Text="";
			textPriRem.Text="";
			textSecMax.Text="";
			textSecDed.Text="";
			textSecDedRem.Text="";
			textSecUsed.Text="";
			textSecPend.Text="";
			textSecRem.Text="";
			if(_patCur==null) {
				return;
			}
			double maxFam=0;
			double maxInd=0;
			double ded=0;
			double dedFam=0;
			double dedRem=0;
			double remain=0;
			double pend=0;
			double used=0;
			InsPlan PlanCur;
			InsSub SubCur;
			List<PatPlan> PatPlanList=PatPlans.Refresh(_patCur.PatNum);
			if(!PatPlans.IsPatPlanListValid(PatPlanList)) {
				//PatPlans had invalid references and need to be refreshed.
				PatPlanList=PatPlans.Refresh(_patCur.PatNum);
			}
			List<InsSub> subList=InsSubs.RefreshForFam(_famCur);
			List<InsPlan> InsPlanList=InsPlans.RefreshForSubList(subList);
			List<Benefit> BenefitList=Benefits.Refresh(PatPlanList,subList);
			List<Claim> ClaimList=Claims.Refresh(_patCur.PatNum);
			List<ClaimProcHist> HistList=ClaimProcs.GetHistList(_patCur.PatNum,BenefitList,PatPlanList,InsPlanList,DateTimeOD.Today,subList);
			if(PatPlanList.Count>0) {
				SubCur=InsSubs.GetSub(PatPlanList[0].InsSubNum,subList);
				PlanCur=InsPlans.GetPlan(SubCur.PlanNum,InsPlanList);
				pend=InsPlans.GetPendingDisplay(HistList,DateTimeOD.Today,PlanCur,PatPlanList[0].PatPlanNum,
					-1,_patCur.PatNum,PatPlanList[0].InsSubNum,BenefitList);
				used=InsPlans.GetInsUsedDisplay(HistList,DateTimeOD.Today,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,
					-1,InsPlanList,BenefitList,_patCur.PatNum,PatPlanList[0].InsSubNum);
				textPriPend.Text=pend.ToString("F");
				textPriUsed.Text=used.ToString("F");
				maxFam=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,true);
				maxInd=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,false);
				if(maxFam==-1) {
					textFamPriMax.Text="";
				}
				else {
					textFamPriMax.Text=maxFam.ToString("F");
				}
				if(maxInd==-1) {//if annual max is blank
					textPriMax.Text="";
					textPriRem.Text="";
				}
				else {
					remain=maxInd-used-pend;
					if(remain<0) {
						remain=0;
					}
					//textFamPriMax.Text=max.ToString("F");
					textPriMax.Text=maxInd.ToString("F");
					textPriRem.Text=remain.ToString("F");
				}
				//deductible:
				ded=Benefits.GetDeductGeneralDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,BenefitCoverageLevel.Individual);
				dedFam=Benefits.GetDeductGeneralDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,BenefitCoverageLevel.Family);
				if(ded!=-1) {
					textPriDed.Text=ded.ToString("F");
					dedRem=InsPlans.GetDedRemainDisplay(HistList,DateTimeOD.Today,PlanCur.PlanNum,PatPlanList[0].PatPlanNum,
						-1,InsPlanList,_patCur.PatNum,ded,dedFam);
					textPriDedRem.Text=dedRem.ToString("F");
				}
				if(dedFam!=-1) {
					textFamPriDed.Text=dedFam.ToString("F");
				}
			}
			if(PatPlanList.Count>1) {
				SubCur=InsSubs.GetSub(PatPlanList[1].InsSubNum,subList);
				PlanCur=InsPlans.GetPlan(SubCur.PlanNum,InsPlanList);
				pend=InsPlans.GetPendingDisplay(HistList,DateTimeOD.Today,PlanCur,PatPlanList[1].PatPlanNum,
					-1,_patCur.PatNum,PatPlanList[1].InsSubNum,BenefitList);
				textSecPend.Text=pend.ToString("F");
				used=InsPlans.GetInsUsedDisplay(HistList,DateTimeOD.Today,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,
					-1,InsPlanList,BenefitList,_patCur.PatNum,PatPlanList[1].InsSubNum);
				textSecUsed.Text=used.ToString("F");
				//max=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum);
				maxFam=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,true);
				maxInd=Benefits.GetAnnualMaxDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,false);
				if(maxFam==-1) {
					textFamSecMax.Text="";
				}
				else {
					textFamSecMax.Text=maxFam.ToString("F");
				}
				if(maxInd==-1) {//if annual max is blank
					textSecMax.Text="";
					textSecRem.Text="";
				}
				else {
					remain=maxInd-used-pend;
					if(remain<0) {
						remain=0;
					}
					//textFamSecMax.Text=max.ToString("F");
					textSecMax.Text=maxInd.ToString("F");
					textSecRem.Text=remain.ToString("F");
				}
				//deductible:
				ded=Benefits.GetDeductGeneralDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,BenefitCoverageLevel.Individual);
				dedFam=Benefits.GetDeductGeneralDisplay(BenefitList,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,BenefitCoverageLevel.Family);
				if(ded!=-1) {
					textSecDed.Text=ded.ToString("F");
					dedRem=InsPlans.GetDedRemainDisplay(HistList,DateTimeOD.Today,PlanCur.PlanNum,PatPlanList[1].PatPlanNum,
						-1,InsPlanList,_patCur.PatNum,ded,dedFam);
					textSecDedRem.Text=dedRem.ToString("F");
				}
				if(dedFam!=-1) {
					textFamSecDed.Text=dedFam.ToString("F");
				}
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}