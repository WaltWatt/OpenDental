using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;

namespace OpenDental {
	public partial class FormBenefitElectHistory:ODForm {
		private List<Etrans> list;
		private long PlanNum;
		private long PatPlanNum;
		public List<Benefit> BenList;
		private long SubNum;
		private Patient[] _listPatients;
		private long _subPatNum;
		private long _carrierNum;

		public FormBenefitElectHistory(long planNum,long patPlanNum,long subNum,long subPatNum,long carrierNum) {
			InitializeComponent();
			Lan.F(this);
			PlanNum=planNum;
			PatPlanNum=patPlanNum;
			SubNum=subNum;
			_subPatNum=subPatNum;
			_carrierNum=carrierNum;
		}

		private void FormBenefitElectHistory_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid(){
			list=Etranss.GetList270ForPlan(PlanNum,SubNum);
			List<long> listPatNums=list.Select(x => x.PatNum).ToList();
			listPatNums.Add(_subPatNum);
			_listPatients=Patients.GetMultPats(listPatNums);//Can contain 0.
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Date"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Patient"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Response"),100);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<list.Count;i++){
				row=new ODGridRow();
				row.Cells.Add(list[i].DateTimeTrans.ToShortDateString());
				//All old 270s do not have a patNum set, so they were subscriber request.
				string patName=Patients.GetOnePat(_listPatients,(list[i].PatNum==0?_subPatNum:list[i].PatNum)).GetNameLFnoPref();
				row.Cells.Add(patName);
				row.Cells.Add(list[i].Note);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Etrans etrans=list[e.Row];
			if(etrans.Etype==EtransType.Eligibility_CA) {
				FormEtransEdit formETE=new FormEtransEdit();
				formETE.EtransCur=etrans;
				formETE.ShowDialog();
			}
			else {
				string settingErrors271=X271.ValidateSettings();
				if(settingErrors271!="") {
					MessageBox.Show(settingErrors271);
					return;
				}
				bool isDependent=(etrans.PatNum!=0 && _subPatNum!=etrans.PatNum);//Old rows will be 0, but when 0 then request was for subscriber.
				Carrier carrierCur=Carriers.GetCarrier(_carrierNum);
				FormEtrans270Edit formE=new FormEtrans270Edit(PatPlanNum,PlanNum,SubNum,isDependent,_subPatNum,carrierCur.IsCoinsuranceInverted);
				formE.EtransCur=etrans;
				formE.benList=BenList;
				formE.ShowDialog();
			}
			FillGrid();
		}

		//private void butOK_Click(object sender,EventArgs e) {
		//	DialogResult=DialogResult.OK;
		//}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		
	}
}