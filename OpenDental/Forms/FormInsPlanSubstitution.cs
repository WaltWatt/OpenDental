using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormInsPlanSubstitution:ODForm {

		private InsPlan _insPlan=null;
		private List <ProcedureCode> _listAllProcCodes=null;
		///<summary>Codes which currently have a substituion code set.</summary>
		private List <ProcedureCode> _listSubstProcCodes=null;
		///<summary>Links currently in the database.  Used to load and to sync when saving changes.</summary>
		private List <SubstitutionLink> _listDbSubstLinks=null;

		public FormInsPlanSubstitution(InsPlan insPlan) {
			InitializeComponent();
			Lan.F(this);
			_insPlan=insPlan;
		}

		private void FormInsPlanSubstitution_Load(object sender,EventArgs e) {
			_listAllProcCodes=ProcedureCodes.GetAllCodes();
			_listSubstProcCodes=_listAllProcCodes.FindAll(x => !String.IsNullOrEmpty(x.SubstitutionCode));
			_listDbSubstLinks=SubstitutionLinks.GetAllForPlans(_insPlan.PlanNum);
			FillGridSubstitutionInclude();
			FillGridSubstitutionExclude();
		}

		private void FillGridSubstitutionInclude() {
			gridInsPlanSubstInc.BeginUpdate();
			gridInsPlanSubstInc.Rows.Clear();
			if(gridInsPlanSubstInc.Columns.Count==0) {
				gridInsPlanSubstInc.Columns.Add(new UI.ODGridColumn(Lan.g(gridInsPlanSubstInc.TranslationName,"ProcCode"),70));
				gridInsPlanSubstInc.Columns.Add(new UI.ODGridColumn(Lan.g(gridInsPlanSubstInc.TranslationName,"AbbrDesc"),80));
				gridInsPlanSubstInc.Columns.Add(new UI.ODGridColumn(Lan.g(gridInsPlanSubstInc.TranslationName,"SubstOnlyIf"),80));
				gridInsPlanSubstInc.Columns.Add(new UI.ODGridColumn(Lan.g(gridInsPlanSubstInc.TranslationName,"SubstCode"),70));
				gridInsPlanSubstInc.Columns.Add(new UI.ODGridColumn(Lan.g(gridInsPlanSubstInc.TranslationName,"SubstDesc"),0));				
			}
			foreach(ProcedureCode procCode in _listSubstProcCodes) {
				if(_listDbSubstLinks.Exists(x => x.CodeNum==procCode.CodeNum)) {
					continue;
				}
				AddRow(gridInsPlanSubstInc,procCode);
			}
			gridInsPlanSubstInc.EndUpdate();
		}

		private void FillGridSubstitutionExclude() {
			gridInsPlanSubstExc.BeginUpdate();
			if(gridInsPlanSubstExc.Columns.Count==0) {
				gridInsPlanSubstExc.Columns.Add(new UI.ODGridColumn(Lan.g(gridInsPlanSubstExc.TranslationName,"ProcCode"),70));
				gridInsPlanSubstExc.Columns.Add(new UI.ODGridColumn(Lan.g(gridInsPlanSubstExc.TranslationName,"AbbrDesc"),80));
				gridInsPlanSubstExc.Columns.Add(new UI.ODGridColumn(Lan.g(gridInsPlanSubstExc.TranslationName,"SubstOnlyIf"),80));
				gridInsPlanSubstExc.Columns.Add(new UI.ODGridColumn(Lan.g(gridInsPlanSubstExc.TranslationName,"SubstCode"),70));
				gridInsPlanSubstExc.Columns.Add(new UI.ODGridColumn(Lan.g(gridInsPlanSubstExc.TranslationName,"SubstDesc"),0));
			}
			foreach(ProcedureCode procCode in _listSubstProcCodes) {
				if(!_listDbSubstLinks.Exists(x => x.CodeNum==procCode.CodeNum)) {
					continue;
				}
				AddRow(gridInsPlanSubstExc,procCode);
			}
			gridInsPlanSubstExc.EndUpdate();
		}

		private void AddRow(UI.ODGrid grid,ProcedureCode procCode) {
			UI.ODGridRow row=new UI.ODGridRow();
			row.Tag=procCode;
			row.Cells.Add(procCode.ProcCode);
			row.Cells.Add(procCode.AbbrDesc);
			row.Cells.Add(Lan.g("enumSubstitutionCondition",procCode.SubstOnlyIf.ToString()));
			row.Cells.Add(procCode.SubstitutionCode);
			ProcedureCode procCodeSubst=_listAllProcCodes.FirstOrDefault(x => x.ProcCode==procCode.SubstitutionCode);
			row.Cells.Add((procCodeSubst==null)?"":procCodeSubst.AbbrDesc);
			grid.Rows.Add(row);
		}

		private void butRight_Click(object sender,EventArgs e) {
			if(gridInsPlanSubstInc.SelectedIndices.Length==0) {
				return;
			}
			List <UI.ODGridRow> listIncRows=new List<UI.ODGridRow>();
			List <ProcedureCode> listMoveCodes=new List<ProcedureCode>();
			for(int i=0;i<gridInsPlanSubstInc.Rows.Count;i++) {
				if(gridInsPlanSubstInc.SelectedIndices.Contains(i)) {
					listMoveCodes.Add((ProcedureCode)gridInsPlanSubstInc.Rows[i].Tag);
				}
				else {//The row is not selected.
					listIncRows.Add(gridInsPlanSubstInc.Rows[i]);
				}				
			}
			gridInsPlanSubstInc.BeginUpdate();
			gridInsPlanSubstInc.Rows.Clear();
			foreach(UI.ODGridRow row in listIncRows) {
				gridInsPlanSubstInc.Rows.Add(row);
			}
			gridInsPlanSubstInc.EndUpdate();
			gridInsPlanSubstExc.BeginUpdate();
			foreach(ProcedureCode procCode in listMoveCodes) {
				AddRow(gridInsPlanSubstExc,procCode);
			}
			SortGridByProc(gridInsPlanSubstExc);
			gridInsPlanSubstExc.EndUpdate();
		}

		private void butLeft_Click(object sender,EventArgs e) {
			if(gridInsPlanSubstExc.SelectedIndices.Length==0) {
				return;
			}
			List <UI.ODGridRow> listExRows=new List<UI.ODGridRow>();
			List <ProcedureCode> listMoveCodes=new List<ProcedureCode>();
			for(int i=0;i<gridInsPlanSubstExc.Rows.Count;i++) {
				if(gridInsPlanSubstExc.SelectedIndices.Contains(i)) {
					listMoveCodes.Add((ProcedureCode)gridInsPlanSubstExc.Rows[i].Tag);
				}
				else {//The row is not selected.
					listExRows.Add(gridInsPlanSubstExc.Rows[i]);
				}				
			}
			gridInsPlanSubstExc.BeginUpdate();
			gridInsPlanSubstExc.Rows.Clear();
			foreach(UI.ODGridRow row in listExRows) {
				gridInsPlanSubstExc.Rows.Add(row);
			}
			gridInsPlanSubstExc.EndUpdate();
			gridInsPlanSubstInc.BeginUpdate();
			foreach(ProcedureCode procCode in listMoveCodes) {
				AddRow(gridInsPlanSubstInc,procCode);
			}
			SortGridByProc(gridInsPlanSubstInc);
			gridInsPlanSubstInc.EndUpdate();
		}

		private void SortGridByProc(UI.ODGrid grid) {
			List <UI.ODGridRow> listRows=new List<UI.ODGridRow>();
			foreach(UI.ODGridRow row in grid.Rows) {
				listRows.Add(row);
			}
			listRows=listRows.OrderBy(x => ((ProcedureCode)x.Tag).ProcCode).ToList();
			grid.Rows.Clear();
			foreach(UI.ODGridRow row in listRows) {
				grid.Rows.Add(row);
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!_insPlan.CodeSubstNone && gridInsPlanSubstInc.Rows.Count==0 && gridInsPlanSubstExc.Rows.Count > 0) {
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"You have chosen to exclude all substituion codes.  "
					+"The checkbox option named 'Don't Substitute Codes (e.g. posterior composites)' "
					+"in the Other Ins Info tab of the Edit Insurance Plan window can be used to exclude all substitution codes.\r\n"
					+"Would you like to enable this option instead of excluding specific codes?")) 
				{
					_insPlan.CodeSubstNone=true;
					DialogResult=DialogResult.OK;
					return;
				}
			}
			List <SubstitutionLink> listSubstLinks=new List<SubstitutionLink>();
			foreach(UI.ODGridRow row in gridInsPlanSubstExc.Rows) {
				ProcedureCode procCode=(ProcedureCode)row.Tag;
				SubstitutionLink subLink=_listDbSubstLinks.FirstOrDefault(x => x.CodeNum==procCode.CodeNum);
				if(subLink==null) {
					subLink=new SubstitutionLink();
					subLink.PlanNum=_insPlan.PlanNum;
					subLink.CodeNum=procCode.CodeNum;
				}
				listSubstLinks.Add(subLink);
			}
			SubstitutionLinks.Sync(listSubstLinks,_listDbSubstLinks);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}