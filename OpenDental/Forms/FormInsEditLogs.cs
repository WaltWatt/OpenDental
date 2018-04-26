using System;
using System.Collections.Generic;
using System.Linq;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormInsEditLogs:ODForm {
		private InsPlan _insPlan;
		private List<Benefit> _listBenefits;
		private List<InsEditLog> _listLogs= new List<InsEditLog>();

		///<summary>Opens the window with the passed-in parameters set as the default.</summary>
		public FormInsEditLogs(InsPlan insPlan, List<Benefit> listBenefits) {
			InitializeComponent();
			Lan.F(this);
			_insPlan=insPlan;
			_listBenefits=listBenefits;
		}

		private void FormInsEditLogs_Load(object sender,EventArgs e) {
			textDateFrom.Text=DateTime.Now.AddMonths(-1).ToString();
			textDateTo.Text=DateTime.Now.ToString();
			FillGrid();
		}

		///<summary>Set refreshFromDb to false to avoid running a query and use cached data.</summary>
		private void FillGrid(bool refreshFromDb=true) {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new ODGridColumn("",25)); //for the drop down arrows.
			gridMain.Columns.Add(new ODGridColumn("Log Date",135));
			gridMain.Columns.Add(new ODGridColumn("User",100));
			gridMain.Columns.Add(new ODGridColumn("Table",100));
			gridMain.Columns.Add(new ODGridColumn("Key",55));
			gridMain.Columns.Add(new ODGridColumn("Description",150));
			gridMain.Columns.Add(new ODGridColumn("Field",110));
			gridMain.Columns.Add(new ODGridColumn("Before",150));
			gridMain.Columns.Add(new ODGridColumn("After",150));
			gridMain.Rows.Clear();
			ConstructGridRows(refreshFromDb).ForEach(x => { gridMain.Rows.Add(x); });
			gridMain.EndUpdate();
		}

		///<summary>Actually creates the GridRows and returns them in a list. Takes care of linking dropdown rows.</summary>
		private List<ODGridRow> ConstructGridRows(bool refreshFromDb) {
			if(_listLogs == null || refreshFromDb) {
				_listLogs = InsEditLogs.GetLogsForPlan(_insPlan.PlanNum,_insPlan.CarrierNum,_insPlan.EmployerNum,PIn.Date(textDateFrom.Text),PIn.Date(textDateTo.Text));
			}
			ODGridRow row;
			List<ODGridRow> listRows=new List<ODGridRow>();
			Dictionary<long,Userod> dictUsers=Userods.GetDeepCopy().ToDictionary(x => x.UserNum,x => x);
			TranslateBeforeAndAfter();
			foreach(InsEditLog logCur in _listLogs) {
				row = new ODGridRow();
				row.Cells.Add("");
				row.Cells.Add(logCur.DateTStamp.ToString());
				row.Cells.Add(dictUsers[logCur.UserNum].UserName);
				row.Cells.Add(logCur.LogType.ToString());
				row.Cells.Add(logCur.FKey.ToString());
				row.Cells.Add(logCur.Description);
				row.Cells.Add(logCur.FieldName);
				row.Cells.Add(logCur.OldValue);
				row.Cells.Add(logCur.NewValue);
				row.Tag=logCur;
				listRows.Add(row);
			}
			//link drop down rows to drop down parents.
			listRows.Where(x => ((InsEditLog)x.Tag).LogType == InsEditLogType.Benefit
				&& ((InsEditLog)x.Tag).FieldName == "BenefitNum"
				&& ((InsEditLog)x.Tag).NewValue == "DELETED").ToList()
				.ForEach(y => {
					y.DropDownState=ODGridDropDownState.Up;
					listRows.Where(x => ((InsEditLog)x.Tag).LogType == InsEditLogType.Benefit
						&& ((InsEditLog)x.Tag).FieldName != "BenefitNum" && ((InsEditLog)x.Tag).FKey == ((InsEditLog)y.Tag).FKey
						&& ((InsEditLog)x.Tag).NewValue == "DELETED").ToList().ForEach(x => {
							x.DropDownParent=y;
						});
				});
			listRows.Where(x => ((InsEditLog)x.Tag).LogType == InsEditLogType.Carrier
				&& ((InsEditLog)x.Tag).FieldName == "CarrierNum"
				&& ((InsEditLog)x.Tag).NewValue == "DELETED").ToList()
				.ForEach(y => {
					y.DropDownState=ODGridDropDownState.Up;
					listRows.Where(x => ((InsEditLog)x.Tag).LogType == InsEditLogType.Carrier
						&& ((InsEditLog)x.Tag).FieldName != "CarrierNum" && ((InsEditLog)x.Tag).FKey == ((InsEditLog)y.Tag).FKey
						&& ((InsEditLog)x.Tag).NewValue == "DELETED").ToList().ForEach(x => {
							x.DropDownParent=y;
						});
				});
			listRows.Where(x => ((InsEditLog)x.Tag).LogType == InsEditLogType.Employer
				&& ((InsEditLog)x.Tag).FieldName == "EmployerNum"
				&& ((InsEditLog)x.Tag).NewValue == "DELETED").ToList()
				.ForEach(y => {
					y.DropDownState=ODGridDropDownState.Up;
					listRows.Where(x => ((InsEditLog)x.Tag).LogType == InsEditLogType.Employer
						&& ((InsEditLog)x.Tag).FieldName != "EmployerNum" && ((InsEditLog)x.Tag).FKey == ((InsEditLog)y.Tag).FKey
						&& ((InsEditLog)x.Tag).NewValue == "DELETED").ToList().ForEach(x => {
							x.DropDownParent=y;
						});
				});
			listRows.Where(x => ((InsEditLog)x.Tag).LogType == InsEditLogType.InsPlan
				&& ((InsEditLog)x.Tag).FieldName == "PlanNum"
				&& ((InsEditLog)x.Tag).NewValue == "DELETED").ToList()
				.ForEach(y => {
					y.DropDownState=ODGridDropDownState.Up;
					listRows.Where(x => ((InsEditLog)x.Tag).LogType == InsEditLogType.InsPlan
						&& ((InsEditLog)x.Tag).FieldName != "PlanNum" && ((InsEditLog)x.Tag).FKey == ((InsEditLog)y.Tag).FKey
						&& ((InsEditLog)x.Tag).NewValue == "DELETED").ToList().ForEach(x => {
							x.DropDownParent=y;
						});
				});
			return listRows;
		}
		
		///<summary>Makes the "Before" and "After" columns human-readable for certain logs.</summary>
		private void TranslateBeforeAndAfter() {
			foreach(InsEditLog logCur in _listLogs) {
				long beforeKey = PIn.Long(logCur.OldValue,false);
				long afterKey = PIn.Long(logCur.NewValue,false);
				switch(logCur.FieldName) {
					case "CarrierNum":
						if(logCur.LogType == InsEditLogType.Carrier) {
							break;
						}
						logCur.OldValue = beforeKey == 0 ? logCur.OldValue : Carriers.GetCarrier(beforeKey).CarrierName;
						logCur.NewValue = afterKey == 0 ? logCur.NewValue : Carriers.GetCarrier(afterKey).CarrierName;
						break;
					case "EmployerNum":
						if(logCur.LogType == InsEditLogType.Employer) {
							break;
						}
						logCur.OldValue = beforeKey == 0 ? logCur.OldValue : Employers.GetName(beforeKey);
						logCur.NewValue = afterKey == 0 ? logCur.NewValue : Employers.GetName(afterKey);
						break;
					case "FeeSched":
					case "CopayFeeSched":
					case "AllowedFeeSched":
						logCur.OldValue = beforeKey == 0 ? logCur.OldValue : FeeScheds.GetDescription(beforeKey);
						logCur.NewValue = afterKey == 0 ? logCur.NewValue : FeeScheds.GetDescription(afterKey);
						break;
					case "BenefitType":
						logCur.OldValue = beforeKey == 0 ? logCur.OldValue : Enum.GetName(typeof(InsBenefitType),beforeKey);
						logCur.NewValue = afterKey == 0 ? logCur.NewValue : Enum.GetName(typeof(InsBenefitType),afterKey);
						break;
					case "CovCatNum":
						logCur.OldValue = beforeKey == 0 ? logCur.OldValue : CovCats.GetDesc(beforeKey);
						logCur.NewValue = afterKey == 0 ? logCur.NewValue : CovCats.GetDesc(afterKey);
						break;
					case "CodeNum":
						logCur.OldValue = beforeKey == 0 ? logCur.OldValue : ProcedureCodes.GetStringProcCode(beforeKey);
						logCur.NewValue = afterKey == 0 ? logCur.NewValue : ProcedureCodes.GetStringProcCode(afterKey);
						break;
					default:
						break;
				}
			}
		}

		private void textSearches_TextChanged(object sender,EventArgs e) {
			timerSearch.Stop();
			timerSearch.Start();
		}

		private void timerSearch_Tick(object sender,EventArgs e) {
			timerSearch.Stop();
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}
	}
}