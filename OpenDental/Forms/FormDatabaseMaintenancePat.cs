using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Text.RegularExpressions;
using OpenDental.UI;
using CodeBase;
using System.Runtime.ExceptionServices;
using System.Linq;

namespace OpenDental {
	public partial class FormDatabaseMaintenancePat:ODForm {
		private List<MethodInfo> _listDbmMethodsGrid;
		private long _patNum;

		public FormDatabaseMaintenancePat(List<MethodInfo> listDbmMethods,long patNum=0) {
			InitializeComponent();
			Lan.F(this);
			_patNum=patNum;
			_listDbmMethodsGrid=listDbmMethods;
		}

		private void FormDatabaseMaintenancePat_Load(object sender,EventArgs e) {
			if(PrefC.GetBool(PrefName.DatabaseMaintenanceSkipCheckTable)) {
				labelSkipCheckTable.Visible=true;
			}
			UpdateTextPatient();
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"Name"),300));
			gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"Break\r\nDown"),40,HorizontalAlignment.Center));
			gridMain.Columns.Add(new ODGridColumn(Lan.g(this,"Results"),0));
			gridMain.Rows.Clear();
			ODGridRow row;
			//_listDbmMethodsGrid has already been filled on load with the correct methods to display in the grid.
			foreach(MethodInfo meth in _listDbmMethodsGrid) {
				row=new ODGridRow();
				row.Cells.Add(meth.Name);
				row.Cells.Add(DatabaseMaintenances.MethodHasBreakDown(meth) ? "X" : "");
				row.Cells.Add("");
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void Run(DbmMode modeCur) {
			if(_patNum < 1) {
				MsgBox.Show(this,"Select a patient first.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			//Clear out the result column for all rows before every "run"
			for(int i=0;i<gridMain.Rows.Count;i++) {
				gridMain.Rows[i].Cells[2].Text="";//Don't use UpdateResultTextForRow here because users will see the rows clearing out one by one.
			}
			bool verbose=checkShow.Checked;
			StringBuilder logText=new StringBuilder();
			//Create a thread that will show a window and then stay open until the closing phrase is thrown from this form.
			Action actionCloseCheckTableProgress=ODProgressOld.ShowProgressStatus("CheckTableProgress",this);
			ODTuple<string,bool> tableCheckResult=DatabaseMaintenances.MySQLTables(verbose,modeCur);
			actionCloseCheckTableProgress();
			logText.Append(tableCheckResult.Item1);
			//No database maintenance methods should be run unless this passes.
			if(!tableCheckResult.Item2) {
				Cursor=Cursors.Default;
				MsgBoxCopyPaste msgBoxCP=new MsgBoxCopyPaste(tableCheckResult.Item1);//the Tuples result is already translated.
				msgBoxCP.Show();//Let this window be non-modal so that they can keep it open while they fix their problems.
				return;
			}
			if(gridMain.SelectedIndices.Length < 1) {
				//No rows are selected so the user wants to run all checks.
				gridMain.SetSelected(true);
			}
			string result;
			int[] selectedIndices=gridMain.SelectedIndices;
			for(int i=0;i<selectedIndices.Length;i++) {
				long userNum=0;
				DbmMethodAttr methodAttributes=(DbmMethodAttr)Attribute.GetCustomAttribute(_listDbmMethodsGrid[selectedIndices[i]],typeof(DbmMethodAttr));
				//We always send verbose and modeCur into all DBM methods.
				List<object> parameters=new List<object>() { verbose,modeCur };
				//There are optional paramaters available to some methods and adding them in the following order is very important.
				if(methodAttributes.HasUserNum) {
					parameters.Add(userNum);
				}
				if(methodAttributes.HasPatNum) {
					parameters.Add(_patNum);
				}
				gridMain.ScrollToIndexBottom(selectedIndices[i]);
				UpdateResultTextForRow(selectedIndices[i],Lan.g("FormDatabaseMaintenance","Running")+"...");
				try {
					result=(string)_listDbmMethodsGrid[selectedIndices[i]].Invoke(null,parameters.ToArray());
					if(modeCur==DbmMode.Fix) {
						DatabaseMaintenances.UpdateDateLastRun(_listDbmMethodsGrid[selectedIndices[i]].Name);
					}
				}
				catch(Exception ex) {
					if(ex.InnerException!=null) {
						ExceptionDispatchInfo.Capture(ex.InnerException).Throw();//This preserves the stack trace of the InnerException.
					}
					throw;
				}
				string status="";
				if(result=="") {//Only possible if running a check / fix in non-verbose mode and nothing happened or needs to happen.
					status=Lan.g("FormDatabaseMaintenance","Done.  No maintenance needed.");
				}
				UpdateResultTextForRow(selectedIndices[i],result+status);
				logText.Append(result);
			}
			gridMain.SetSelected(selectedIndices,true);//Reselect all rows that were originally selected.
			try {
				DatabaseMaintenances.SaveLogToFile(logText.ToString());
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MessageBox.Show(ex.Message);
			}
			Cursor=Cursors.Default;
			if(modeCur==DbmMode.Fix) {
				//_isCacheInvalid=true;//Flag cache to be invalidated on closing.  Some DBM fixes alter cached tables.
			}
		}

		///<summary>Updates the result column for the specified row in gridMain with the text passed in.</summary>
		private void UpdateResultTextForRow(int index,string text) {
			gridMain.BeginUpdate();
			//Checks to see if it has a breakdown, and if it needs any maintenenece to decide whether or not to apply the "X"
			if(!DatabaseMaintenances.MethodHasBreakDown(_listDbmMethodsGrid[index]) || text == "Done.  No maintenance needed.") {
				gridMain.Rows[index].Cells[1].Text="";
			}
			else {
				gridMain.Rows[index].Cells[1].Text="X";
			}
			gridMain.Rows[index].Cells[2].Text=text;
			gridMain.EndUpdate();
			Application.DoEvents();
		}

		private void UpdateTextPatient() {
			//For whatever reason GetLim() never returns null.
			textPatient.Text=Patients.GetLim(_patNum).GetNameFL();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!DatabaseMaintenances.MethodHasBreakDown(_listDbmMethodsGrid[e.Row])) {
				return;
			}
			//We know that this method supports giving the user a break down and shall call the method's fix section where the break down results should be.
			//TODO: Make sure that DBM methods with break downs ALWAYS have the break down in the fix section.
			long userNum=0;
			long patNum=0;
			DbmMethodAttr methodAttributes=(DbmMethodAttr)Attribute.GetCustomAttribute(_listDbmMethodsGrid[e.Row],typeof(DbmMethodAttr));
			//We always send verbose and modeCur into all DBM methods.
			List<object> parameters=new List<object>() { checkShow.Checked,DbmMode.Breakdown };
			//There are optional paramaters available to some methods and adding them in the following order is very important.
			if(methodAttributes.HasUserNum) {
				parameters.Add(userNum);
			}
			if(methodAttributes.HasPatNum) {
				parameters.Add(patNum);
			}
			Cursor=Cursors.WaitCursor;
			string result=(string)_listDbmMethodsGrid[e.Row].Invoke(null,parameters.ToArray());
			if(result=="") {//Only possible if running a check / fix in non-verbose mode and nothing happened or needs to happen.
				result=Lan.g("FormDatabaseMaintenance","Done.  No maintenance needed.");
			}
			//Show the result of the dbm method in a simple copy paste msg box.
			MsgBoxCopyPaste msgBoxCP=new MsgBoxCopyPaste(result);
			Cursor=Cursors.Default;
			msgBoxCP.Show();//Let this window be non-modal so that they can keep it open while they fix their problems.
		}

		private void butPatientSelect_Click(object sender,EventArgs e) {
			FormPatientSelect FormPs=new FormPatientSelect();
			FormPs.SelectionModeOnly=true;
			FormPs.ShowDialog();
			if(FormPs.DialogResult!=DialogResult.OK) {
				return;
			}
			_patNum=FormPs.SelectedPatNum;
			UpdateTextPatient();
		}

		private void butNone_Click(object sender,EventArgs e) {
			gridMain.SetSelected(false);
		}

		private void butCheck_Click(object sender,EventArgs e) {
			Run(DbmMode.Check);
		}

		private void butFix_Click(object sender,EventArgs e) {
			Run(DbmMode.Fix);
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
			Close();
		}

		private void FormDatabaseMaintenancePat_FormClosing(object sender,FormClosingEventArgs e) {
			//if(_isCacheInvalid) {
			//	Action actionCloseDBM=ODProgress.ShowProgressStatus("DatabaseMaintEvent",this,Lan.g(this,"Refreshing all caches, this can take a while..."));
			//	//Invalidate all cached tables.  DBM could have touched anything so blast them all.  
			//	//Failure to invalidate cache can cause UEs in the main program.
			//	DataValid.SetInvalid(Cache.GetAllCachedInvalidTypes().ToArray());
			//	actionCloseDBM?.Invoke();
			//}
		}
	}
}